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
                if (objParcel.Delivered == DateTime.MinValue)
                {
                    foreach (var drn in lstDrone)
                    {
                        if (drn.Id == objParcel.DroneId)
                        {
                            drn.Status = DroneStatuses.Delivery;
                            drn.Battery = R.Next(0, 100);  // צריך לשנות את ה0
                        }
                    }
                }
            }

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
                    //temp.ParcelTransfer = drone.
                    foreach (var parcel in parcels)
                    {
                        if (parcel.DroneId == id)
                        {
                            prclTrnsfr.Id = parcel.Id;
                            prclTrnsfr.Weight = (WeightCategories) parcel.Weight;

                            if (parcel.PickedUp != DateTime.MinValue)
                                prclTrnsfr.ParcelSituation = true;
                            else
                                prclTrnsfr.ParcelSituation = false;

                            prclTrnsfr.Priority = (Priorities) parcel.Priority;
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

                    flag1 = true;
                    break;
                }

                if (!flag1)
                {
                    throw new ItemNotFoundException(id, "ERROR :id of drone not found\n");
                }

                
            }
            return temp;
        }

        public object BaseStationDisplay(int id)
        {
            IEnumerable<IDAL.DO.Station> stations = dal.ListBaseStation();
            List<DroneInCharging> lstDrnInChrg = new List<DroneInCharging>();
            DroneInCharging DrnInChrg = new DroneInCharging();
            Station temp = new Station();
            bool flag = false;
            foreach(var statn in stations)
            {
                if(statn.Id == id)
                {
                    temp.Id = statn.Id;
                    temp.Name = statn.Name;
                    temp.location.Longitude = statn.Longitude;
                    temp.location.Lattitude = statn.Lattitude;
                    temp.AvailableChargeSlots = statn.AvailableChargeSlots;
                    foreach (var drn in lstDrone)
                    {
                        if ((drn.Status == DroneStatuses.Charging) && (drn.Location.Longitude == statn.Longitude) && (drn.Location.Lattitude == statn.Lattitude))
                        {
                            DrnInChrg.DroneId = drn.Id;
                            DrnInChrg.Battery = drn.Battery;
                            lstDrnInChrg.Add(DrnInChrg);
                        }                           
                    }
                    temp.droneInCharging = lstDrnInChrg;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new ItemNotFoundException(id, "ERROR :id of drone not found\n");
            }
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
