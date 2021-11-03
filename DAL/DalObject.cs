using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;
using static DalObject.DataSource;

namespace DalObject 
{
    public class DalObject: IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// Returns the station in the certain index
        /// </summary>
        /// <param name="StationId"></param>
        /// <returns>Station</returns>
        public  Station GetStation(int StationId)  
        {
            Station ?newStation = null;
            foreach (Station objStation in DataSource.Stations)
            {
                if (objStation.Id==StationId)
                {
                    newStation = objStation;
                }
            }
            if (newStation==null)
            {
               throw new ItemNotFoundExcepton(StationId,"ERROR :id of Station not found\n");
            }
            return (Station)newStation;
       
        }
        /// <summary>
        /// Returns the drone in the certain index
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns> Drone </returns>
        public  Drone GetDrone(int droneId)  
        {
            Drone? newDrone = null;
            foreach (Drone objDrone in DataSource.Drones)
            {
                if (objDrone.Id == droneId)
                {
                    newDrone = objDrone;
                }
            }
            if (newDrone== null)
            {
                throw new ItemNotFoundExcepton(droneId, "ERROR :id of drone not found\n");
            }
            return (Drone)newDrone;
        }
        /// <summary>
        /// Returns the customer in the certain index
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns> Customer</returns>
        public  Customer GetCustomer(int customerId)  
        {
            Customer? newCustomer = null;
            foreach (Customer objCustomer in DataSource.Customers)
            {
                if (objCustomer.Id == customerId)
                {
                    newCustomer = objCustomer;
                }
            }
            if (newCustomer== null)
            {
                throw new ItemNotFoundExcepton(customerId, "ERROR :id of customer not found\n");
            }
            return (Customer)newCustomer;
        }
        /// <summary>
        /// Returns the parcel in the certain index
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> Parcel </returns>
        public  Parcel GetParcel(int parcelId)  
        {
            Parcel? newParcel = null;
            foreach (Parcel objParcel in DataSource.Parcels)
            {
                if (objParcel.Id == parcelId)
                {
                    newParcel = objParcel;
                }
            }
            if (newParcel == null)
            {
                throw new ItemNotFoundExcepton(parcelId, "ERROR :id of parcel not found\n");
            }
            return (Parcel)newParcel;
        }
        /// <summary>
        /// Returns the time the parcel was created
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> DateTime </returns>
        public  DateTime GetParcelCreating(int parcelId) 
        {
            DateTime time = DateTime.MinValue;
            foreach (Parcel objParcel in DataSource.Parcels)
            {
                if (objParcel.Id == parcelId)
                {
                    time = objParcel.Creating;
                }
            }
            if (time==DateTime.MinValue)
            {
                throw new ItemNotFoundExcepton(parcelId, "ERROR :id of parcel not found\n");
            }
            return time; 

        }
        /// <summary>
        /// Returns the time the parcel was affiliated with a drone
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> DateTime</returns>
        public  DateTime GetParcelAffiliation(int parcelId) 
        {
            DateTime time = DateTime.MinValue;
            foreach (Parcel objParcel in DataSource.Parcels)
            {
                if (objParcel.Id == parcelId)
                {
                    time = objParcel.Affiliation;
                }
            }
            if (time==DateTime.MinValue)
            {
                throw new ItemNotFoundExcepton(parcelId, "ERROR :id of parcel not found\n");
            }
            return time;
        }
        /// <summary>
        /// Returns the time the parcel was picked up
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> DateTime </returns>
        public  DateTime GetParcelPickedUp(int parcelId)   
        {
            DateTime time = DateTime.MinValue;
            foreach (Parcel objParcel in DataSource.Parcels)
            {
                if (objParcel.Id == parcelId)
                {
                    time = objParcel.PickedUp;
                }
            }
            if (time==DateTime.MinValue)
            {
                throw new ItemNotFoundExcepton(parcelId,"ERROR :id of parcel not found\n");
            }
            return time;
        }
        /// <summary>
        /// Returns the time the parcel was delivered
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> DateTime </returns>
        public  DateTime GetParcelDelivered(int parcelId)  
        {
            DateTime time = DateTime.MinValue;
            foreach (Parcel objParcel in DataSource.Parcels)
            {
                if (objParcel.Id == parcelId)
                {
                    time = objParcel.Delivered;
                }
            }
            if (time==DateTime.MinValue)
            {
                throw new ItemNotFoundExcepton(parcelId,"ERROR :id of parcel not found\n");
            }
            return time;
        }
        /// <summary>
        /// Adding a station to the next open index
        /// </summary>
        /// <param name="newStation"></param>
        public void AddStation(Station newStation)  
        {
            foreach (Station objStation in DataSource.Stations)
            {
                if (objStation.Id == newStation.Id)
                {
                    throw new ItemAlreadyExistsExcepton(newStation.Id, "ERROR: id of Station already exists\n");
                }
            }
            Stations.Add(newStation);
        }
        /// <summary>
        /// Adding a drone to the next open index
        /// </summary>
        /// <param name="newDrone"></param>
        public  void AddDrone(Drone newDrone)  
        {
            foreach (Drone objStation in DataSource.Drones)
            {
                if (objStation.Id == newDrone.Id)
                {
                    throw new ItemAlreadyExistsExcepton(newDrone.Id, "ERROR: id of Drone already exists\n");
                }
            }
            Drones[Drones.Count] = newDrone;
        }
        /// <summary>
        /// Adding a customer to the next open index
        /// </summary>
        /// <param name="newCustomer"></param>
        public  void AddCustomer(Customer newCustomer)   
        {
            foreach (Customer objStation in DataSource.Customers)
            {
                if (objStation.Id == newCustomer.Id)
                {
                    throw new ItemAlreadyExistsExcepton(newCustomer.Id, "ERROR: id of Customer already exists\n");
                }
            }
            Customers[Customers.Count] = newCustomer;
        }
        /// <summary>
        /// Adding a parcel to the next open index
        /// </summary>
        /// <param name="newParcel"></param>
        public  void AddParcel(Parcel newParcel)    
        {
            Parcels[Parcels.Count] = newParcel;
        }
        /// <summary>
        /// if we've found an available drone, we will affiliate the parcel with it
        /// </summary>
        /// <param name="idParcel"></param>
        /// <param name="droneId"></param>
        public  void Affiliate(int idParcel,int droneId)
        {

            for (int i = 0; i < Drones.Count; i++)
            {
                if (Drones[i].Id == droneId)
                {
                    Parcel p = Parcels[i];
                    p.DroneId = Drones[i].Id;
                    p.Affiliation = DateTime.Now;
                    Parcels[i] = p;
                    break;
                }
            }
        }
        /// <summary>
        /// Changing drone status, and time of pickup
        /// </summary>
        /// <param name="id"></param>
        public  void PickupParcelUpdate(int ParcelId)  
        {
            for (int i = 0; i < Parcels.Count; i++)
            {
                if (Parcels[i].Id == ParcelId)
                {
                    Parcel p = Parcels[i];
                    p.PickedUp = DateTime.Now;
                    Parcels[i] = p;
                    break;
                }
            } 
        }
        /// <summary>
        /// Delivering the parcel
        /// </summary>
        /// <param name="id"></param>
        public  void SupplyParcelUpdate(int ParcelId)   
        {
            for (int i = 0; i < Parcels.Count; i++)
            {
                if (Parcels[i].Id == ParcelId)
                {
                    Parcel p = Parcels[i];
                    p.Delivered = DateTime.Now;
                    Parcels[i] = p;
                    break;
                }
            }
        }

        /// <summary>
        /// We find the charger and the station the drone is  charging again
        /// </summary>
        /// <param name="DroneId"></param>
        public  void ReleaseDroneFromCharger(int DroneId)
        {
            for (int i = 0; i < DroneCharges.Count; i++)
            {
                if (DroneCharges[i].DroneId == DroneId)   
                { 
                    for (int k = 0; k < Stations.Count; k++)
                    {
                        if (DroneCharges[i].StationId == Stations[k].Id)  
                        {

                            Station s = Stations[k];
                            s.AvailableChargeSlots++;
                            Stations[k] = s;
                            break;
                        }
                    }
                    DroneCharge dc = DroneCharges[i];
                    dc.DroneId = -1;
                    dc.StationId = -1;
                    DroneCharges[i] = dc;
                    break;
                }
            }
        }
        /// <summary>
        /// Returning a list of all the Stations
        /// </summary>
        /// <returns> List </returns>
        public IEnumerable<Station> ListBaseStation()  
        {
            List<Station> PrintStation = new List<Station>();
            for (int i = 0; i < Stations.Count; i++)
            {
                PrintStation.Add(GetStation(Stations[i].Id));
            }
            return PrintStation;
        }
        /// <summary>
        /// Returning a list of all the Drones
        /// </summary>
        /// <returns> List </returns>
        public IEnumerable <Drone> ListDrone()   
        {
            List<Drone> PrintDrone = new List<Drone>();
            for (int i = 0; i < Drones.Count; i++)
            {
                PrintDrone.Add(GetDrone(Drones[i].Id));
            }
            return PrintDrone;
        }

        /// <summary>
        /// Returning a list of all the Customers
        /// </summary>
        /// <returns> List </returns>
        public IEnumerable<Customer> ListCustomer()   
        {
            List<Customer> PrintCustomer = new List<Customer>();
            for (int i = 0; i <Customers.Count; i++)
            {
                PrintCustomer.Add(GetCustomer(Customers[i].Id));
            }
            return PrintCustomer;
        }

        /// <summary>
        /// Returning a list of all the Parcels
        /// </summary>
        /// <returns> List </returns>
        public IEnumerable<Parcel> ListParcel()  
        {
            List<Parcel> PrintParcel = new List<Parcel>();
            for (int i = 0; i <Parcels.Count; i++)
            {
                PrintParcel.Add(GetParcel(Parcels[i].Id));
            }
            return PrintParcel;
        }
        /// <summary>
        /// distance calculation
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="id"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public  double distanceCalculation(double lat1, double lon1,int id,char temp)
        {
            double[] arr = new double[2];
            double lat2, lon2;
            if (temp=='s')
            {
                for (int i = 0; i <Stations.Count; i++)
                {
                    if (GetStation(i).Id == id)
                    {
                        arr[0] = GetStation(i).Lattitude;
                        arr[1] = GetStation(i).Longitude;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i <Customers.Count; i++)
                {
                    if (GetCustomer(i).Id == id)
                    {
                        arr[0] = GetCustomer(i).Lattitude;
                        arr[1] = GetCustomer(i).Longitude;
                        break;
                    }
                }
            }
            lat2 = arr[0];
            lon2 = arr[1];
            int R = 6371; 
            double φ1 = lat1 * Math.PI / 180; 
            double φ2 = lat2 * Math.PI / 180;
            double Δφ = (lat2 - lat1) * Math.PI / 180;
            double Δλ = (lon2 - lon1) * Math.PI / 180;

           double a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                      Math.Cos(φ1) * Math.Cos(φ2) *
                      Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return  R * c; 
        }

        /// <summary>
        /// Returning a list of all the Parcels that have not been affiliated witha Drone
        /// </summary>
        /// <returns> List </returns>
        public IEnumerable<Parcel> ListParcelOnAir()  
        {
            List<Parcel> PrintParcelOnAir = new List<Parcel>();
            for (int i = 0; i <Parcels.Count; i++)
            {
                if (GetParcel(i).DroneId == -1)
                    PrintParcelOnAir.Add(GetParcel(i));
            }
            return PrintParcelOnAir;
        }

        public  int GetParcelId()
        {
            return DataSource.Config.NewParcelId;
        }

        /// <summary>
        /// Returning the correct Parcel in oreder to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Parcel </returns>
        public  Parcel ParcelDisplay(int id)  
        {
            int i;
            for (i = 0; i < Parcels.Count; i++)
            {
                if (GetParcel(Parcels[i].Id).Id == id) { break; }
            }
            return GetParcel(Parcels[i].Id);
        }

        /// <summary>
        /// Returning the correct Station in oreder to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Station </returns>
        public Station BaseStationDisplay(int id)  
        {
            int i;
            for (i = 0; i < Stations.Count; i++)
            {
                if (GetStation(Stations[i].Id).Id == id) { break; }
            }
            return GetStation(Stations[i].Id);
        }

        /// <summary>
        /// Adding the stations that have open slot to the list
        /// </summary>
        /// <returns> List </returns>
        public IEnumerable<Station> ListStationsWithOpenSlots()
        {
            List<Station> PrintCustomer = new List<Station>();
            for (int i = 0; i <Stations.Count; i++)
            {
                if (GetStation(i).AvailableChargeSlots > 0)
                    PrintCustomer.Add(GetStation(i));  
            }
            return PrintCustomer;  
        }

        /// <summary>
        /// Returning the correct Drone in oreder to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Drone </returns>
        public  Drone DroneDisplay(int id)  
        {
            int i;
            for (i = 0; i < Drones.Count; i++)
            {
                if (GetDrone(Drones[i].Id).Id == id) { break; }
            }
            return GetDrone(Drones[i].Id);
        }

        /// <summary>
        /// Returning the correct Customer in oreder to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Customer </returns>
        public  Customer CustomerDisplay(int id)  
        {
            int i;
            for (i = 0; i < Customers.Count; i++)
            {
                if (GetCustomer(Customers[i].Id).Id == id) { break; }
            }
            return GetCustomer(Customers[i].Id);
        }

        /// <summary>
        /// Charging Drone
        /// </summary>
        /// <param name="DroneId"></param>
       /* public  void FindDroneToCharge(int DroneId)
        {
            int i;
            for ( i = 0; i<Drones.Count; i++)
            {
                if (Drones[i].Id == DroneId)
                {
                    Drones[i].Status = DroneStatuses.Charging;
                    break;
                }
            }
        }
        */
        /// <summary>
        /// Lowering the available slots by 1 because we've added a drone to charge there
        /// </summary>
        /// <param name="droneCharge"></param>
        /// <param name="StationId"></param>
        public  void AddDroneToCharge(DroneCharge droneCharge,int StationId)
        {
            for (int i = 0; i < Stations.Count; i++)
            {
                if (Stations[i].Id== StationId)
                {
                    Station s = Stations[i];
                    s.AvailableChargeSlots--;
                    Stations[i] = s;
                    break;
                }
            }
            DroneCharges.Add(droneCharge);
        }

        public  double[] GetElectricUsage()
        {
            double[] array = new double[5];
            array[0] = DataSource.Config.available;
            array[1] = DataSource.Config.lightWeight;
            array[2] = DataSource.Config.mediumWeight;
            array[3] = DataSource.Config.heavyWeight;
            array[4] = DataSource.Config.chargeSpeed;
            return array;
        }

        public double GetChargeSpeed()
        {
            return DataSource.Config.chargeSpeed;
        }
    }
}



























