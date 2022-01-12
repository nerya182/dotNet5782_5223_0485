using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using static DalObject.DataSource;
using DalApi;
using System.Runtime.CompilerServices;

namespace DalObject 
{
    internal class DalObject: IDal
    {
        static readonly IDal instance = new DalObject();
        public static IDal Instance { get => instance; }
        private DalObject()
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// Returns the droneCharge in the certain index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int index)
        {
            return DroneCharges[index];
        }

        /// <summary>
        /// Returns the station in the certain index
        /// </summary>
        /// <param name="StationId"></param>
        /// <returns>Station</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  Station GetStation(int StationId)  
        {
            Station ?newStation = null;
            foreach (var objStation in from Station objStation in DataSource.Stations
                                       where objStation.Id == StationId
                                       select objStation)
            {
                newStation = objStation;
            }

            if (newStation==null)
            {
               throw new ItemNotFoundException(StationId,"ERROR :id of Station not found\n");
            }
            return (Station)newStation;
       
        }

        /// <summary>
        /// Returns the drone in the certain index
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns> Drone </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)  
        {
            Drone? newDrone = null;
            foreach (var objDrone in from Drone objDrone in DataSource.Drones
                                     where objDrone.Id == droneId
                                     select objDrone)
            {
                newDrone = objDrone;
            }

            if (newDrone== null)
            {
                throw new ItemNotFoundException(droneId, "ERROR :id of drone not found\n");
            }
            return (Drone)newDrone;
        }
        /// <summary>
        /// Returns the customer in the certain index
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns> Customer</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  Customer GetCustomer(int customerId)  
        {
            Customer? newCustomer = null;
            foreach (var objCustomer in from Customer objCustomer in DataSource.Customers
                                        where objCustomer.Id == customerId
                                        select objCustomer)
            {
                newCustomer = objCustomer;
            }

            if (newCustomer== null)
            {
                throw new ItemNotFoundException(customerId, "ERROR :id of customer not found\n");
            }
            return (Customer)newCustomer;
        }
        /// <summary>
        /// Returns the parcel in the certain index
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> Parcel </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  Parcel GetParcel(int parcelId)  
        {
            Parcel? newParcel = null;
            foreach (var objParcel in from Parcel objParcel in DataSource.Parcels
                                      where objParcel.Id == parcelId
                                      select objParcel)
            {
                newParcel = objParcel;
            }

            if (newParcel == null)
            {
                throw new ItemNotFoundException(parcelId, "ERROR :id of parcel not found\n");
            }
            return (Parcel)newParcel;
        }

        /// <summary>
        /// Adding a station to the next open index
        /// </summary>
        /// <param name="newStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station newStation)  
        {
            foreach (var _ in from Station objStation in DataSource.Stations
                              where objStation.Id == newStation.Id
                              select new { })
            {
                throw new ItemAlreadyExistsException(newStation.Id, "ERROR: id of Station already exists\n");
            }

            Stations.Add(newStation);
        }
        /// <summary>
        /// Adding a drone to the next open index
        /// </summary>
        /// <param name="newDrone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void AddDrone(Drone newDrone)  
        {
            foreach (var _ in from Drone objDrone in DataSource.Drones
                              where objDrone.Id == newDrone.Id
                              select new { })
            {
                throw new ItemAlreadyExistsException(newDrone.Id, "ERROR: id of Drone already exists\n");
            }

            Drones.Add(newDrone);
        }
        /// <summary>
        /// Adding a customer to the next open index
        /// </summary>
        /// <param name="newCustomer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void AddCustomer(Customer newCustomer)   
        {
            foreach (var _ in from Customer objCustomer in DataSource.Customers
                              where objCustomer.Id == newCustomer.Id
                              select new { })
            {
                throw new ItemAlreadyExistsException(newCustomer.Id, "ERROR: id of Customer already exists\n");
            }

            Customers.Add(newCustomer);
        }
        /// <summary>
        /// Adding a parcel to the next open index
        /// </summary>
        /// <param name="newParcel"></param> 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void AddParcel(Parcel newParcel)    
        {
            foreach (var _ in from Parcel objParcel in DataSource.Parcels
                              where objParcel.Id == newParcel.Id
                              select new { })
            {
                throw new ItemAlreadyExistsException(newParcel.Id, "ERROR: id of Parcel already exists\n");
            }

            Parcels.Add(newParcel);
            DataSource.Config.NewParcelId++;
            Console.WriteLine(Config.NewParcelId);
        }
        /// <summary>
        /// if we've found an available drone, we will affiliate the parcel with it
        /// </summary>
        /// <param name="idParcel"></param>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void Affiliate(int idParcel,int droneId)
        {
            bool flag = false;
            foreach (var _ in from Drone objStation in DataSource.Drones
                              where objStation.Id == droneId
                              select new { })
            {
                flag = true;
            }

            if (!flag)
            {
                throw new ItemNotFoundException(droneId, "ERROR: id of Drone not found\n");
            }

            for (int i = 0; i < Parcels.Count; i++)
            {
                if (Parcels[i].Id == idParcel)
                {
                    Parcel p = Parcels[i];
                    p.DroneId = droneId;
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void SupplyParcelUpdate(int ParcelId)   
        {
            for (int i = 0; i < Parcels.Count; i++)
            {
                if (Parcels[i].Id == ParcelId)
                {
                    Parcel p = Parcels[i];
                    p.Delivered = DateTime.Now;
                    p.DroneId = 0;
                    Parcels[i] = p;
                    break;
                }
            }
        }
        /// <summary>
        /// We find the charger and the station the drone is  charging again
        /// </summary>
        /// <param name="DroneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                    DroneCharges.Remove(DroneCharges[i]);
                    break;
                }
            }
        }
        /// <summary>
        /// Returning a list of all the Stations
        /// </summary>
        /// <returns> List </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> ListBaseStation(Predicate<Station> predicate)  
        {
            List<Station> PrintStation = DataSource.Stations.FindAll(predicate);          
            return PrintStation;
        }
        /// <summary>
        /// Returning a list of all the drone charge
        /// </summary>
        /// <returns>List</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> ListDroneCharge()
        {
            List<DroneCharge> PrintDroneCharge = new List<DroneCharge>();
            for(int i = 0; i< DroneCharges.Count;i++)
            {
                PrintDroneCharge.Add(GetDroneCharge(i));
            }
            return PrintDroneCharge;
        }
        /// <summary>
        /// Returning a list of all the Drones
        /// </summary>
        /// <returns> List </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> ListDrone(Predicate<Drone> predicate)
        {
            List<Drone> printDrone = DataSource.Drones.FindAll(predicate);
            return printDrone;
        }

        /// <summary>
        /// Returning a list of all the Customers
        /// </summary>
        /// <returns> List </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> ListCustomer(Predicate<Customer> predicate)
        {
            List<Customer> PrintCustomer = DataSource.Customers.FindAll(predicate);
            return PrintCustomer;
        }

        /// <summary>
        /// Returning a list of all the Parcels
        /// </summary>
        /// <returns> List </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> ListParcel(Predicate<Parcel> predicate)  
        {
            List<Parcel> PrintParcel = DataSource.Parcels.FindAll(predicate);
            return PrintParcel;
        }

        /// <summary>
        /// Returning a list of all the Parcels that have not been affiliated with a Drone
        /// </summary>
        /// <returns> List </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> ListParcelOnAir()  
        {
            List<Parcel> PrintParcelOnAir = new List<Parcel>();
            foreach(Parcel parcel in ListParcel(i => i.DroneId == 0))
            {
                PrintParcelOnAir.Add(parcel);   
            }
            return PrintParcelOnAir;
        }
        /// <summary>
        /// return the next number of new parcel 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  int GetParcelId()
        {
            return DataSource.Config.NewParcelId;
        }

        /// <summary>
        /// Returning the correct Parcel in oreder to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Parcel </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> ListStationsWithOpenSlots()
        {
            List<Station>PrintCustomer = new List<Station>();
            foreach (Station station in ListBaseStation(i => i.AvailableChargeSlots > 0))
            {
                PrintCustomer.Add(station);
            }
            return PrintCustomer;
        }

        /// <summary>
        /// Returning the correct Drone in oreder to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Drone </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// Lowering the available slots by 1 because we've added a drone to charge there
        /// </summary>
        /// <param name="droneCharge"></param>
        /// <param name="StationId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void AddDroneToCharge(DroneCharge droneCharge)
        {
            bool flag = false;
            foreach (Station objStation in DataSource.Stations)
            {
                if (objStation.Id == droneCharge.StationId)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                throw new ItemAlreadyExistsException(droneCharge.StationId);
            }
            for (int i = 0; i < Stations.Count; i++)
            {
                if (Stations[i].Id== droneCharge.StationId)
                {
                    Station s = Stations[i];
                    s.AvailableChargeSlots--;
                    Stations[i] = s;
                    break;
                }
            }
            DroneCharges.Add(droneCharge);
        }
        /// <summary>
        /// return power consumption
        /// </summary>
        /// <returns>double</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<double> GetElectricUsage()
        {
            List<double> list = new();
            list.Add(DataSource.Config.available);
            list.Add(DataSource.Config.lightWeight);
            list.Add(DataSource.Config.mediumWeight);
            list.Add(DataSource.Config.heavyWeight);
            list.Add(DataSource.Config.chargeSpeed);
            return list;
        }
        /// <summary>
        /// Returns the charging rate
        /// </summary>
        /// <returns>double</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double GetChargeSpeed()
        {
            return DataSource.Config.chargeSpeed;
        }
        /// <summary>
        /// return distance Between two points
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = Deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = Deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }
        /// <summary>
        /// Conversion to degrees
        /// </summary>
        /// <param name="deg"></param>
        /// <returns>double</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        /// <summary>
        /// Number of available seats at the station
        /// </summary>
        /// <param name="id"></param>
        /// <returns>int</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AvailableChargeSlotsInStation(int id)
        {
            int count = 0;
            foreach (DroneCharge objCharge in DataSource.DroneCharges)
            {
                if (objCharge.StationId==id)
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// Returns the power consumption according to the weight of the package
        /// </summary>
        /// <param name="weight"></param>
        /// <returns>double</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double GetElectricUsageNumber(WeightCategories weight)
        {
            if (weight == (WeightCategories)1)
            {
                return Config.lightWeight;
            }
            if (weight == (WeightCategories)2)
            {
                return Config.mediumWeight;
            } 
            if (weight == (WeightCategories)3)
            {
                return Config.heavyWeight;
            }

            return Config.heavyWeight;
        }
        /// <summary>
        /// Update Customer-name/phone
        /// </summary>
        /// <param name="updateCustomer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer updateCustomer)
        {
            for (int i = 0; i < Customers.Count; i++)
            {
                if (Customers[i].Id == updateCustomer.Id)
                {
                    Customer customer = Customers[i];
                    if (updateCustomer.Name != "")
                    {
                        customer.Name = updateCustomer.Name;
                    }
                    if (updateCustomer.Phone != "")
                    {
                        customer.Phone = updateCustomer.Phone;
                    }
                    Customers[i] = customer;
                    break;
                }
            }
        }
        /// <summary>
        /// Update station name/available charge slots
        /// </summary>
        /// <param name="updateStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station updateStation)
        {
            bool flag = false;
            for (int i = 0; i < Stations.Count(); i++)
            {
                if (Stations[i].Id == updateStation.Id)
                {
                    DO.Station s = Stations[i];
                    if (updateStation.Name != "")
                    {
                        s.Name = updateStation.Name;
                    }
                    if (updateStation.AvailableChargeSlots !=0)
                    {
                        if (updateStation.AvailableChargeSlots -AvailableChargeSlotsInStation(updateStation.Id) < 0)
                        {
                            throw new IllegalActionException("The total amount of charging stations is invalid\n");
                        }
                        else
                        {
                            s.AvailableChargeSlots =
                                updateStation.AvailableChargeSlots -AvailableChargeSlotsInStation(updateStation.Id);
                        }
                    }
                    Stations[i] = s;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new ItemNotFoundException(updateStation.Id, "Enter an existing station number in the system\n");
            }
        }
        /// <summary>
        /// Update drone-model
        /// </summary>
        /// <param name="updateDrone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone updateDrone)
        {
            bool flag = false;
            for (int i = 0; i < Drones.Count(); i++)
            {
                if (Drones[i].Id == updateDrone.Id)
                {
                    DO.Drone d = Drones[i];
                    d.Model = updateDrone.Model;
                   Drones[i] = d;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new ItemNotFoundException(updateDrone.Id, "Enter an existing drone number in the system\n");
            }
        }
        /// <summary>
        /// Checks ID number according to a review digit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckId(int id)
        {
            int number = id / 10;
            int s = 0, kefel=0, SumOfDigitsOfProduct=0;
            for (int i = 1; i <= 8; i++)
            {
                if (i % 2 == 0)
                    kefel = (number % 10);
                else
                    kefel = (number % 10) * 2;
                number /= 10;
                SumOfDigitsOfProduct = sumDigits(kefel);
                s += SumOfDigitsOfProduct;
            }
            if (s % 10 == 0)
                return 0==id%10;
            else
                return (10 - s % 10)==id%10;
        }
        /// <summary>
        /// help function
        /// </summary>
        /// <param name="num"></param>
        /// <returns>int</returns>
        private int sumDigits(int num)
        {
            int s = 0;
            while (num > 0)
            {
                s = s + num % 10;
                num /= 10;
            }
            return s;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int id)
        {
            Parcels.Remove(ParcelDisplay(id));          
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int id)
        {
            Stations.Remove(BaseStationDisplay(id));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int id)
        {
            Customers.Remove(CustomerDisplay(id));
        }
    }
}





























