﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Schema;
using DalObject;
using IBL.BO;
using IDAL;


namespace BL
{
    public partial class BL : IBL.IBL
    {
        public List<DroneToList> lstDrone = new List<DroneToList>();
        public IDal dal;
        public double AvailbleElec { get; set; }
        public  double LightElec { get; set; }
        public double IntermeduateElec { get; set; } 
        public  double HeavyElec { get; set; }
        public  double ChargePerHours { get; set; }

        public BL()
        {
            Random R = new Random();

            dal = new DalObject.DalObject();
            double[] elecUsage = dal.GetElectricUsage();
            AvailbleElec = elecUsage[0];
            LightElec = elecUsage[1];
            IntermeduateElec = elecUsage[2];
            HeavyElec = elecUsage[3];
            ChargePerHours = elecUsage[4];

            List<IDAL.DO.Parcel> dalParcels = dal.ListParcel().ToList();
            List<IDAL.DO.Drone> dalDrones = dal.ListDrone().ToList();

            foreach (IDAL.DO.Parcel parcel in dal.ListParcel())
            {
                if (parcel.Delivered==DateTime.MinValue&&parcel.DroneId!=-1)
                {
                    for (int i = 0; i < lstDrone.Count; i++)
                    {
                        if (lstDrone[i].Id==parcel.DroneId)
                        {
                            lstDrone[i].Status = DroneStatuses.Delivery;
                            if (parcel.PickedUp==DateTime.MinValue)//שוייך אבל לא נאסף
                            {
                                IDAL.DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
                                IDAL.DO.Station stationCloset =GetClosestCustomer(customerSender);
                                lstDrone[i].Location.Lattitude = stationCloset.Lattitude;
                                lstDrone[i].Location.Longitude = stationCloset.Longitude;
                            }

                            if (parcel.PickedUp!=DateTime.MinValue)
                            {
                                
                            }
                        }
                    }
                }
            }



        }

        private IDAL.DO.Station GetClosestCustomer(IDAL.DO.Customer customerSender)
        {
            List<IDAL.DO.Station> stations = dal.ListBaseStation().ToList();
            Location closestStation = new Location();
            closestStation.Lattitude = stations[0].Lattitude;
            closestStation.Longitude = stations[0].Longitude;
            int i = 0, index = -1;
            foreach (var station in stations)
            {
                if (dal.GetDistanceFromLatLonInKm(customerSender.Lattitude, customerSender.Longitude, station.Lattitude, station.Longitude) <
                    dal.GetDistanceFromLatLonInKm(customerSender.Lattitude, customerSender.Longitude, closestStation.Lattitude, closestStation.Longitude))
                {
                    index = i;
                    closestStation.Lattitude =customerSender.Lattitude;
                    closestStation.Longitude = customerSender.Longitude;
                }
                i++;
            }
            return stations[index];
        }

        private IDAL.DO.Station GetClosestStation(DroneToList drone)
        {
            List<IDAL.DO.Station> stations = dal.ListBaseStation().ToList();
            Location closestStation = new Location();
            closestStation.Lattitude = stations[0].Lattitude;
            closestStation.Longitude = stations[0].Longitude;
            int i = 0, index = -1;
            foreach (var station in stations)
            {
                if (dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude, station.Lattitude, station.Longitude) <
                    dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude, closestStation.Lattitude, closestStation.Longitude))
                {
                    index = i;
                    closestStation.Lattitude = drone.Location.Lattitude;
                    closestStation.Longitude = drone.Location.Longitude;
                }
                i++;
            }
            return stations[index];
        }

        private double GetMinCharge(DroneToList drone)
        {
            return 2.5;
        }

        public void AddStation(Station newStation)
        {
            try
            {
                IDAL.DO.Station temp = new IDAL.DO.Station();
                temp.Id = newStation.Id;
                temp.Name = newStation.Name;
                temp.AvailableChargeSlots = newStation.AvailableChargeSlots;
                temp.Lattitude = newStation.location.Lattitude;
                temp.Longitude = newStation.location.Longitude;
                dal.AddStation(temp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public void AddDrone(DroneToList newDrone, int chargingStationId)
        {
            bool flag = false;

            IDAL.DO.Drone temp = new IDAL.DO.Drone();
                temp.Id = newDrone.Id;
                temp.Model = newDrone.Model;
                temp.MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight;
                Random r = new Random();
                newDrone.Battery = r.Next(20, 41);
                newDrone.Status = DroneStatuses.Charging;
                newDrone.ParcelBeingPassedId = 0;
                foreach (var objStation in dal.ListBaseStation())
                {
                    if (objStation.Id == chargingStationId)
                    {
                        Location laLocationOfNewDrone = new Location();

                        laLocationOfNewDrone.Lattitude = objStation.Lattitude;
                        laLocationOfNewDrone.Longitude = objStation.Longitude;
                        newDrone.Location = laLocationOfNewDrone;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    throw new ItemNotFoundException(chargingStationId,"Enter an existing station number in the system for initial charging of the drone\n");
                }
                try
                {
                    dal.AddDrone(temp);
                    lstDrone.Add(newDrone);
                }
                catch (Exception e)
                {
                    throw new ItemAlreadyExistsException(temp.Id, "", e);
                }
        }

        public void AddCustomer(Customer newCustomer)
        {
            try
            {
                IDAL.DO.Customer temp = new IDAL.DO.Customer();
                temp.Id = newCustomer.Id;
                temp.Name = newCustomer.Name;
                temp.Phone = newCustomer.Phone;
                temp.Lattitude = newCustomer.Location.Lattitude;
                temp.Longitude = newCustomer.Location.Longitude;
                dal.AddCustomer(temp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AddParcel(Parcel newParcel)
        {
            try
            {
                IDAL.DO.Parcel temp = new IDAL.DO.Parcel
                {
                    SenderId = newParcel.Sender.Id,
                    TargetId = newParcel.Target.Id,
                    Weight = (IDAL.DO.WeightCategories)newParcel.Weight,
                    Priority = (IDAL.DO.Priorities)newParcel.Priority,
                    Creating = DateTime.Now,
                    Affiliation = DateTime.MinValue,
                    PickedUp = DateTime.MinValue,
                    Delivered = DateTime.MinValue,
                    DroneId = 0
                };
                dal.AddParcel(temp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void UpdateDrone(DroneToList newDrone)
        {
            //update in dataSource
            bool flag = false;
            List<IDAL.DO.Drone> drones = dal.ListDrone().ToList();
            for (int i = 0; i < drones.Count(); i++)
            {
                if (drones[i].Id == newDrone.Id)
                {
                    IDAL.DO.Drone d = drones[i];
                    d.Model = newDrone.Model;
                    drones[i] = d;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new ItemNotFoundException(newDrone.Id, "ERROR :id of drone not found\n");
            }
            //update list drone in BL
            foreach (var drone in lstDrone)
            {
                if (drone.Id == newDrone.Id)
                {
                    drone.Model = newDrone.Model;
                    break;
                }
            }
        }


        public object DroneDisplay(int id)
        {
            IEnumerable<IDAL.DO.Parcel> parcels = dal.ListParcel();
            ParcelTransfer prclTrnsfr = new ParcelTransfer();
            Location tempLocationSupply = new Location();
            Location tempLocationCollect = new Location();
            CustomerInParcel sender = new CustomerInParcel();
            CustomerInParcel receiver = new CustomerInParcel();
            Drone temp = new Drone();
            DroneToList drone = GetDroneFromLstDrone(id);
            temp.Id = drone.Id;
            temp.Model = drone.Model;
            temp.MaxWeight = drone.MaxWeight;
            temp.Battery = drone.Battery;
            temp.Status = drone.Status;
            temp.Location = drone.Location;
            foreach (var parcel in parcels)
            {
                if (parcel.DroneId == id)
                {
                    prclTrnsfr.Id = parcel.Id;
                    prclTrnsfr.Weight = (WeightCategories)parcel.Weight;
                    prclTrnsfr.Priority = (Priorities)parcel.Priority;

                    if (parcel.PickedUp != DateTime.MinValue)
                        prclTrnsfr.ParcelSituation = true;
                    else
                        prclTrnsfr.ParcelSituation = false;
                   
                    IDAL.DO.Customer target1 = dal.GetCustomer(parcel.TargetId);
                    tempLocationSupply.Lattitude = target1.Lattitude;
                    tempLocationSupply.Longitude = target1.Longitude;
                    prclTrnsfr.SupplyPoint = tempLocationSupply;

                    IDAL.DO.Customer sender1 = dal.GetCustomer(parcel.SenderId);
                    tempLocationCollect.Lattitude = sender1.Lattitude;
                    tempLocationCollect.Longitude = sender1.Longitude;
                    prclTrnsfr.collection = tempLocationCollect;

                    prclTrnsfr.distanceTransportation = dal.GetDistanceFromLatLonInKm(
                        tempLocationSupply.Lattitude,
                        tempLocationSupply.Longitude, tempLocationCollect.Lattitude,
                        tempLocationCollect.Longitude);

                    IDAL.DO.Customer sender2 = dal.GetCustomer(prclTrnsfr.Sender.Id);
                    sender.Id = sender2.Id;
                    sender.Name = sender2.Name;
                    prclTrnsfr.Sender = sender;

                    IDAL.DO.Customer target2 = dal.GetCustomer(prclTrnsfr.Receiver.Id);
                    receiver.Id = target2.Id;
                    receiver.Name = target2.Name;
                    prclTrnsfr.Receiver = receiver;
                }              
            }
            temp.ParcelTransfer = prclTrnsfr;
            return temp;
        }

        public ParcelToList MakeParcelToList(Parcel objParcel)
        {
            ParcelToList parcelToList = new ParcelToList();
            parcelToList.Id = objParcel.Id;
            parcelToList.Weight = objParcel.Weight;
            parcelToList.Priority = objParcel.Priority;
            parcelToList.SenderName = objParcel.Sender.Name;
            parcelToList.TargetName = objParcel.Target.Name;
            if (objParcel.Delivered != DateTime.MinValue)
                parcelToList.ShipmentStatus = ParcelStatus.Supplied;
            else if (objParcel.PickedUp != DateTime.MinValue)
                parcelToList.ShipmentStatus = ParcelStatus.PickedUp;
            else if (objParcel.Affiliation != DateTime.MinValue)
                parcelToList.ShipmentStatus = ParcelStatus.Assigned;
            else
                parcelToList.ShipmentStatus = ParcelStatus.Created;
            return parcelToList;
        }

        public CustomerToList MakeCustomerToList(Customer objCustomer)
        {
            CustomerToList customerToList = new CustomerToList();
            customerToList.Id = objCustomer.Id;
            customerToList.Name = objCustomer.Name;
            customerToList.Phone = objCustomer.Phone;
            int count1 = 0;
            foreach(ParceltAtCustomer parceltAtCustomer in objCustomer.ToCustomer)
            {
                if (parceltAtCustomer.status == ParcelStatus.Supplied)
                    count1++;
            }
            customerToList.Received_Parcels = count1;
            customerToList.OnTheWay_Parcels = objCustomer.ToCustomer.Count - count1;
            int count2 = 0;
            foreach(ParceltAtCustomer parceltAtCustomer1 in objCustomer.FromCustomer)
            {
                if (parceltAtCustomer1.status == ParcelStatus.Supplied)
                    count2++;
            }
            customerToList.Delivered_Supplied_Parcels = count2;
            customerToList.Delivered_NotSupplied_Parcels = objCustomer.FromCustomer.Count - count2;
            return customerToList;          
        }

        public DroneToList MakeDroneToList(Drone objDrone)
        {
            DroneToList droneToList = new DroneToList();
            droneToList.Id = objDrone.Id;
            droneToList.Location = objDrone.Location;
            droneToList.Battery = objDrone.Battery;
            droneToList.Model = objDrone.Model;
            droneToList.MaxWeight = objDrone.MaxWeight;
            droneToList.Status = objDrone.Status;
            droneToList.ParcelBeingPassedId = objDrone.ParcelTransfer.Id;
            return droneToList;
        }

        public StationToList MakeStationToList(Station objStation)
        {
            StationToList stationToList = new StationToList();
            stationToList.Id = objStation.Id;
            stationToList.Name = objStation.Name;
            stationToList.AvailableChargeSlots = objStation.AvailableChargeSlots;
            stationToList.UsedChargeSlots = objStation.droneInCharging.Count;
            return stationToList;
        }

        public void DeliveryOfParcelByDrone(int droneId)
        {
            DroneToList drone = lstDrone.Find(i => i.Id == droneId);
            IDAL.DO.Parcel parcel = dal.GetParcel(drone.ParcelBeingPassedId);
            IDAL.DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
            if (parcel.DroneId==droneId&&parcel.PickedUp!=DateTime.MinValue&&parcel.Delivered==DateTime.MinValue)
            {
                double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                    customerTarget.Lattitude, customerTarget.Longitude);
                drone.Battery-= distance * dal.GetElectricUsageNumber(parcel.Weight);
                drone.Location.Lattitude = customerTarget.Lattitude;
                drone.Location.Longitude = customerTarget.Longitude;
                drone.Status = DroneStatuses.Available;
                dal.SupplyParcelUpdate(parcel.Id);
            }
            else
            {
                throw new IllegalActionException("The parcel was not collected by the drone\n");
            }
        }

        public void ParcelCollectionByDrone(int droneId)
        {
            DroneToList drone = lstDrone.Find(i => i.Id == droneId);
            IDAL.DO.Parcel parcel = dal.GetParcel(drone.ParcelBeingPassedId);
            IDAL.DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
            if (parcel.Affiliation!=DateTime.MinValue&&parcel.PickedUp==DateTime.MinValue&&parcel.DroneId==drone.Id&&drone.Status==DroneStatuses.Delivery)
            {
                double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                    customerSender.Lattitude, customerSender.Longitude);

                drone.Battery -= distance * dal.GetElectricUsage()[0];
                drone.Location.Lattitude = customerSender.Lattitude;
                drone.Location.Longitude = customerSender.Longitude;
                dal.PickupParcelUpdate(parcel.Id);
            }
            else
            {
                throw new IllegalActionException("The drone is not in delivery mode / not associated with this parcel\n");
            }
        }

        public void AffiliateParcelToDrone(int droneId)
        {
            DroneToList drone = lstDrone.Find(i => i.Id == droneId);
            if (drone==null)
            {
                throw new ItemAlreadyExistsException(droneId);
            }
            List<IDAL.DO.Parcel> parcels = dal.ListParcel().ToList();
            List<IDAL.DO.Parcel> parcelsfiltered = new List<IDAL.DO.Parcel>();
            parcelsfiltered = parcels.FindAll(i => i.Priority == IDAL.DO.Priorities.Urgent);
            if (parcelsfiltered.Count==0) { parcels.FindAll(i => i.Priority == IDAL.DO.Priorities.Express); }
            if (parcelsfiltered.Count == 0) { parcels.FindAll(i => i.Priority == IDAL.DO.Priorities.Regular); }

            parcels = parcelsfiltered;

            parcelsfiltered = parcels.FindAll(i => i.Weight == (IDAL.DO.WeightCategories)drone.MaxWeight);
            if (parcelsfiltered.Count == 0)
                parcelsfiltered = parcels.FindAll(i => i.Weight == (IDAL.DO.WeightCategories) drone.MaxWeight - 1&& (drone.MaxWeight - 1)>0);
            if (parcelsfiltered.Count == 0) 
                parcelsfiltered= parcels.FindAll(i => i.Weight == (IDAL.DO.WeightCategories)drone.MaxWeight - 2 && (drone.MaxWeight - 2) > 0);

            IDAL.DO.Parcel parcel = GetClosestParcel(parcelsfiltered, drone);
            IDAL.DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
            IDAL.DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
            double distance1 = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,//distance to send
                customerSender.Lattitude, customerSender.Longitude);
            double distance2 = dal.GetDistanceFromLatLonInKm(customerSender.Lattitude, customerSender.Longitude,//distance to target
                                  customerTarget.Lattitude, customerTarget.Longitude);
            DroneToList droneInSender = new DroneToList();
            droneInSender.Location.Lattitude = customerTarget.Lattitude;
            droneInSender.Location.Longitude = customerTarget.Longitude;
            double distance3 = dal.GetDistanceFromLatLonInKm(GetClosestStation(droneInSender).Lattitude,
                GetClosestStation(droneInSender).Longitude, customerTarget.Lattitude, customerTarget.Longitude);


            if (drone.Status==DroneStatuses.Available&&drone.Battery>(distance2*dal.GetElectricUsageNumber(parcel.Weight)+distance1*dal.GetElectricUsage()[0]+distance3*dal.GetElectricUsage()[0]))
            {
                drone.Status = DroneStatuses.Delivery;
                dal.Affiliate(parcel.Id,drone.Id);
            }
            drone.ParcelBeingPassedId = parcel.Id;
        }
        private IDAL.DO.Parcel GetClosestParcel(List<IDAL.DO.Parcel> parcels, DroneToList drone)
        {
            int i = 0, index = 0;

            Location closestParcel = new Location();
            IDAL.DO.Customer customer = dal.GetCustomer(parcels[0].SenderId);
            closestParcel.Lattitude = customer.Lattitude;
            closestParcel.Longitude = customer.Longitude;
            foreach (var parcel in parcels)
            {
                customer = dal.GetCustomer(parcel.SenderId);
                if (dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                    customer.Lattitude, customer.Longitude) < dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude, closestParcel.Lattitude, closestParcel.Longitude))
                {
                    index = i;
                    closestParcel.Lattitude = customer.Lattitude;
                    closestParcel.Longitude = customer.Longitude;
                }
                i++;
            }
            return parcels[index];
        }

        public void ReleaseDroneFromCharging(int droneId, double time)
        {
            var drone = lstDrone.Find(i => i.Id == droneId);
            if (drone == null)
            {
                throw new ItemNotFoundException(droneId);
            }
            if (drone.Status != DroneStatuses.Charging)
            {
                throw new IllegalActionException("The drone is not charging");
            }
            dal.ReleaseDroneFromCharger(droneId);
            drone.Status = DroneStatuses.Available;
            drone.Battery = +(int)dal.GetChargeSpeed() * time;
            if (drone.Battery > 100)
            {
                drone.Battery = 100;
            }

        }

        public void SendingDroneForCharging(int droneId)
        {
            var drone = lstDrone.Find(i => i.Id == droneId);
            if (drone == null)
            {
                throw new ItemNotFoundException(droneId);
            }
            if (drone.Status != DroneStatuses.Available)
            {
                throw new IllegalActionException("The drone is not available");
            }
            IDAL.DO.Station closestStation = GetClosestStation(drone);
            double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                closestStation.Lattitude, closestStation.Longitude);

            if (closestStation.AvailableChargeSlots > 0 && drone.Battery > distance * dal.GetElectricUsage()[0])
            {
                IDAL.DO.DroneCharge droneCharge = new()
                { DroneId = drone.Id, StationId = closestStation.Id, EntryTime = DateTime.Now };
                dal.AddDroneToCharge(droneCharge);
                drone.Battery = distance * dal.GetElectricUsage()[0];
                drone.Location.Lattitude = closestStation.Lattitude;
                drone.Location.Longitude = closestStation.Longitude;
                drone.Status = DroneStatuses.Charging;
            }
            else
            {
                throw new IllegalActionException(
                    "There are no free charging stations at this station or there is not enough battery\n");
            }

        }

        public DroneToList GetDroneFromLstDrone(int id)
        {
            bool flag = false;
            foreach (DroneToList drone in lstDrone)
            {
                if (drone.Id == id)
                {
                    flag = true;
                    return drone;
                }
            }
            if (!flag)
                throw new ItemNotFoundException(id, "ERROR :id of drone not found\n");
            return null;
        }

        public Station BaseStationDisplay(int id)
        {
            IDAL.DO.Station station = dal.GetStation(id);
            IEnumerable<IDAL.DO.DroneCharge> droneCharge = dal.ListDroneCharge();
            List<DroneInCharging> lstDrnInChrg = new List<DroneInCharging>();
            DroneInCharging DrnInChrg = new DroneInCharging();
            Station temp = new Station();
            temp.Id = station.Id;
            temp.Name = station.Name;
            temp.location.Longitude = station.Longitude;
            temp.location.Lattitude = station.Lattitude;
            temp.AvailableChargeSlots = station.AvailableChargeSlots;
            foreach (var drnChrg in droneCharge)
            {
                if (drnChrg.StationId == id)
                {
                    DrnInChrg.DroneId = drnChrg.DroneId;
                    DrnInChrg.Battery = GetDroneFromLstDrone(drnChrg.DroneId).Battery;
                    lstDrnInChrg.Add(DrnInChrg);
                }
            }
            temp.droneInCharging = lstDrnInChrg;
            return temp;
        }

        public Customer CustomerDisplay(int id)
        {
            IDAL.DO.Customer customer = dal.GetCustomer(id);
            IEnumerable<IDAL.DO.Parcel> parcels = dal.ListParcel();
            List<ParceltAtCustomer> lstSending = new List<ParceltAtCustomer>();
            List<ParceltAtCustomer> lstReceived = new List<ParceltAtCustomer>();
            ParceltAtCustomer parcelAtCstmr = new ParceltAtCustomer();
            CustomerInParcel cstmrInPrcl = new CustomerInParcel();

            Customer temp = new Customer();
            temp.Id = customer.Id;
            temp.Name = customer.Name;
            temp.Phone = customer.Phone;
            temp.Location.Longitude = customer.Longitude;
            temp.Location.Lattitude = customer.Lattitude;

            foreach (var parcel in parcels)
            {
                if ((parcel.SenderId == customer.Id) && (parcel.Delivered == DateTime.MinValue))
                {
                    parcelAtCstmr.Id = parcel.Id;
                    parcelAtCstmr.status = ParcelStatus.Created;
                    parcelAtCstmr.Weight = (WeightCategories)parcel.Weight;
                    parcelAtCstmr.Priority = (Priorities)parcel.Priority;
                    IDAL.DO.Customer cstmr = dal.GetCustomer(parcel.TargetId);
                    cstmrInPrcl.Id = cstmr.Id;   
                    cstmrInPrcl.Name = cstmr.Name;
                    parcelAtCstmr.OpposingSide = cstmrInPrcl;
                    lstSending.Add(parcelAtCstmr);
                }
                else if ((parcel.TargetId == customer.Id) && (parcel.Delivered != DateTime.MinValue))
                {
                    parcelAtCstmr.Id = parcel.Id;
                    parcelAtCstmr.status = ParcelStatus.Supplied;
                    parcelAtCstmr.Weight = (WeightCategories)parcel.Weight;
                    parcelAtCstmr.Priority = (Priorities)parcel.Priority;
                    IDAL.DO.Customer cstmr = dal.GetCustomer(parcel.SenderId);
                    cstmrInPrcl.Id = cstmr.Id;
                    cstmrInPrcl.Name = cstmr.Name;
                    parcelAtCstmr.OpposingSide = cstmrInPrcl;
                    lstReceived.Add(parcelAtCstmr);
                }
            }
            temp.FromCustomer = lstSending;
            temp.ToCustomer = lstReceived;
            return temp;
        }

        public object ParcelDisplay(int id)
        {
            IDAL.DO.Parcel parcel = dal.GetParcel(id);
            DroneInParcel droneInParcel = new DroneInParcel();
            CustomerInParcel customerInParcel = new CustomerInParcel();
            Parcel temp = new Parcel();

            temp.Id = parcel.Id;
            temp.Weight = (WeightCategories)parcel.Weight;
            temp.Priority = (Priorities)parcel.Priority;
            temp.Affiliation = parcel.Affiliation;
            temp.Creating = parcel.Creating;
            temp.Delivered = parcel.Delivered;
            temp.PickedUp = parcel.PickedUp;

            DroneToList droneToList = GetDroneFromLstDrone(parcel.DroneId);
            droneInParcel.DroneId = droneToList.Id;
            droneInParcel.location = droneToList.Location;
            droneInParcel.Battery = droneToList.Battery;
            temp.drone = droneInParcel;

            IDAL.DO.Customer sender = dal.GetCustomer(parcel.SenderId);
            customerInParcel.Id = sender.Id;
            customerInParcel.Name = sender.Name;
            temp.Sender = customerInParcel;

            IDAL.DO.Customer target = dal.GetCustomer(parcel.TargetId);
            customerInParcel.Id = target.Id;
            customerInParcel.Name = target.Name;
            temp.Target = customerInParcel;

            return temp;
        }

        public void UpdateCustomer(Customer updateCustomer)
        {
            List<IDAL.DO.Customer> Customers = dal.ListCustomer().ToList();
            bool flag = false;
            for (int i = 0; i < Customers.Count(); i++)
            {
                if (Customers[i].Id == updateCustomer.Id)
                {
                    IDAL.DO.Customer c = Customers[i];
                    if (updateCustomer.Name != "no")
                    {
                        c.Name = updateCustomer.Name;
                    }

                    if (updateCustomer.Phone != "no")
                    {
                        c.Phone = updateCustomer.Phone;
                    }
                    Customers[i] = c;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new ItemNotFoundException(updateCustomer.Id, "ERROR :id of station not found\n");
            }
        }

        public void UpdateStation(Station updateStation, int chargingPositions)
        {
            List<IDAL.DO.Station> Stations = dal.ListBaseStation().ToList();
            bool flag = false;
            for (int i = 0; i < Stations.Count(); i++)
            {
                if (Stations[i].Id == updateStation.Id)
                {
                    IDAL.DO.Station s = Stations[i];
                    if (updateStation.Name != "no")
                    {
                        s.Name = updateStation.Name;
                    }

                    if (updateStation.AvailableChargeSlots != -1)
                    {
                        if (chargingPositions - dal.AvailableChargeSlotsInStation(updateStation.Id) < 0)
                        {
                            throw new IllegalActionException("The total amount of charging stations is invalid\n");
                        }
                        else
                        {
                            s.AvailableChargeSlots =
                                chargingPositions - dal.AvailableChargeSlotsInStation(updateStation.Id);
                        }
                    }
                    Stations[i] = s;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new ItemNotFoundException(updateStation.Id, "ERROR :id of station not found\n");
            }
        }
    }
}
