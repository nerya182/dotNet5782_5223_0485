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
                            if(objParcel.Delivered == DateTime.MinValue)
                            {
                                drn.Location = GetClosestStation(drn);
                            }
                            if(objParcel.PickedUp != DateTime.MinValue && objParcel.Delivered == DateTime.MinValue)
                            {
                               // drn.Location.Lattitude = objParcel.SenderId. getLocation of customer of sender ID
                            }


                            
                        }
                    }
                }
            }
        }

        private Location GetClosestStation(DroneToList drn)
        {
            throw new NotImplementedException();
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
            List <IDAL.DO.Drone>drones = dal.ListDrone().ToList();
            for (int i = 0; i < drones.Count(); i++)
            {
                if (drones[i].Id ==newDrone.Id)
                {
                    IDAL.DO.Drone d = drones[i];
                    d.Model= newDrone.Model;
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
                if (drone.Id==newDrone.Id)
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
        public DroneToList GetDroneFromLstDrone(int id)
        {
            bool flag = false;
            foreach(DroneToList drone in lstDrone)
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
            foreach(var drnChrg in droneCharge)
            {
                if(drnChrg.StationId == id)
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

                    if (updateCustomer.Phone!="no")
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

        public void UpdateStation(Station updateStation,int chargingPositions)
        {
            List<IDAL.DO.Station> Stations = dal.ListBaseStation().ToList();
            bool flag = false;
            for (int i = 0; i < Stations.Count(); i++)
            {
                if (Stations[i].Id == updateStation.Id)
                {
                    IDAL.DO.Station s = Stations[i];
                    if (updateStation.Name !="no")
                    {
                        s.Name = updateStation.Name;
                    }

                    if (updateStation.AvailableChargeSlots!=-1)
                    {
                        if (chargingPositions - dal.AvailableChargeSlotsInStation(updateStation.Id)<0)
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
