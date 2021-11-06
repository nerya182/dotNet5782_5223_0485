using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
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

        public void UpdateDron(DroneToList newDrone)
        {
            bool flag = false;

            foreach (var objDrone in lstDrone)
            {
                if (objDrone.Id == newDrone.Id)
                {
                    objDrone.Model = newDrone.Model;
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                throw new ItemNotFoundException(newDrone.Id, "ERROR :id of drone not found\n");
            }

            for (int i = 0; i < dal.ListDrone().Count(); i++)
            {
                if (dal.ListDrone().ElementAt(i).Id == newDrone.Id)
                {
                    var elementAt = dal.ListDrone().ElementAt(i);
                    elementAt.Model = newDrone.Model;
                    break;
                }
            }




        }
    }
}
