using System;
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
        public BL()
        {
            Random R = new Random();
            dal = new DalObject.DalObject();
            double[] elecUsage = dal.GetElectricUsage();
            double chargeSpeed = dal.GetChargeSpeed();

            IEnumerable<IDAL.DO.Parcel> parcels = dal.ListParcel();
            IEnumerable<IDAL.DO.Drone> drones = dal.ListDrone();
            DroneToList temp = new DroneToList();

            foreach (var objDrone in drones)
            {

                temp.Id = objDrone.Id;
                temp.Model = objDrone.Model;
                temp.MaxWeight = (WeightCategories)objDrone.MaxWeight;
                lstDrone.Add(temp);
            }

            foreach (IDAL.DO.Parcel objParcel in parcels)
            {
                if (objParcel.PickedUp == DateTime.MinValue)
                {
                    foreach (var drn in lstDrone)
                    {
                        if (drn.Id == objParcel.DroneId)
                        {

                            drn.Status = DroneStatuses.Delivery;
                            drn.Battery = R.Next((int)GetMinCharge(drn), 100);  // צריך לשנות את ה0
                            if (objParcel.Delivered == DateTime.MinValue)
                            {
                                drn.Location = GetClosestStation(drn);
                            }
                            if (objParcel.PickedUp != DateTime.MinValue && objParcel.Delivered == DateTime.MinValue)
                            {
                                // drn.Location.Lattitude = objParcel.SenderId. getLocation of customer of sender ID
                            }



                        }
                    }
                }
            }
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
            try
            {
                IDAL.DO.Drone temp = new IDAL.DO.Drone();
                temp.Id = newDrone.Id;
                temp.Model = newDrone.Model;
                temp.MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight;
                dal.AddDrone(temp);
                Random r = new Random();
                newDrone.Battery = r.Next(20, 41);
                newDrone.Status = DroneStatuses.Charging;
                newDrone.ParcelBeingPassedId = 0;
                foreach (var objStation in dal.ListBaseStation())
                {
                    if (objStation.Id == chargingStationId)
                    {
                        newDrone.Location.Lattitude = objStation.Lattitude;
                        newDrone.Location.Longitude = objStation.Longitude;
                        break;
                    }
                }

                lstDrone.Add(newDrone);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                throw new ItemAlreadyExistsExcepton(newCustomer.Id, "ERROR: id of Customer already exists\n");
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

            IEnumerable<IDAL.DO.Drone> drones = dal.ListDrone();
            IEnumerable<IDAL.DO.Station> stations = dal.ListBaseStation();
            IEnumerable<IDAL.DO.Parcel> parcels = dal.ListParcel();
            IEnumerable<IDAL.DO.Customer> customers = dal.ListCustomer();
            ParcelTransfer prclTrnsfr = new ParcelTransfer();
            Location tempLocationSupply = new Location();
            Location tempLocationCollect = new Location();
            CustomerInParcel sender = new CustomerInParcel();
            CustomerInParcel receiver = new CustomerInParcel();
            Drone temp = new Drone();
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;

            foreach (var drone in lstDrone)
            {
                if (drone.Id == id)
                {
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

                            if (parcel.PickedUp != DateTime.MinValue)
                                prclTrnsfr.ParcelSituation = true;
                            else
                                prclTrnsfr.ParcelSituation = false;

                            prclTrnsfr.Priority = (Priorities)parcel.Priority;
                            foreach (var customer in customers)
                            {
                                if ((customer.Id == parcel.TargetId) && (!flag2))
                                {
                                    tempLocationSupply.Lattitude = customer.Lattitude;
                                    tempLocationSupply.Longitude = customer.Longitude;
                                    flag2 = true;
                                }

                                if ((customer.Id == parcel.SenderId) && (!flag3))
                                {
                                    tempLocationCollect.Lattitude = customer.Lattitude;
                                    tempLocationCollect.Longitude = customer.Longitude;
                                    flag3 = true;
                                }

                                if (flag2 && flag3)
                                    break;
                            }

                            flag2 = false;
                            flag3 = false;
                            prclTrnsfr.SupplyPoint = tempLocationSupply;
                            prclTrnsfr.collection = tempLocationCollect;
                            prclTrnsfr.distanceTransportation = dal.GetDistanceFromLatLonInKm(
                                tempLocationSupply.Lattitude,
                                tempLocationSupply.Longitude, tempLocationCollect.Lattitude,
                                tempLocationCollect.Longitude);
                            foreach (var cstmr in customers)
                            {
                                if ((cstmr.Id == prclTrnsfr.Sender.Id) && (!flag2))
                                {
                                    sender.Id = cstmr.Id;
                                    sender.Name = cstmr.Name;
                                    flag2 = true;
                                }
                                else if ((cstmr.Id == prclTrnsfr.Receiver.Id) && (!flag3))
                                {
                                    receiver.Id = cstmr.Id;
                                    receiver.Name = cstmr.Name;
                                    flag3 = true;
                                }

                                if (flag2 && flag3)
                                    break;
                            }

                            prclTrnsfr.Sender = sender;
                            prclTrnsfr.Receiver = receiver;
                        }
                    }
                    temp.ParcelTransfer = prclTrnsfr;
                    flag1 = true;
                    break;
                }
            }
            if (!flag1)
            {
                throw new ItemNotFoundException(id, "ERROR :id of drone not found\n");
            }
            return temp;
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
                throw new ItemAlreadyExistsExcepton(droneId);
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
                    return drone;
                    flag = true;
                    break;
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
