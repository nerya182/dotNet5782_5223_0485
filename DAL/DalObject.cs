using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using static DalObject.DataSource;

namespace DalObject 
{
    public class DalObject
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
        public static Station GetStation(int StationId)  
        {
            return DataSource.Stations[StationId];
        }
        /// <summary>
        /// Returns the drone in the certain index
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns> Drone </returns>
        public static Drone GetDrone(int droneId)  
        {
            return DataSource.Drones[droneId];
        }
        /// <summary>
        /// Returns the customer in the certain index
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns> Customer</returns>
        public static Customer GetCustomer(int customerId)  
        {
            return DataSource.Customers[customerId];
        }
        /// <summary>
        /// Returns the parcel in the certain index
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> Parcel </returns>
        public static Parcel GetParcel(int parcelId)  
        {
            return DataSource.Parcels[parcelId];
        }
        /// <summary>
        /// Returns the time the parcel was created
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> DateTime </returns>
        public static DateTime GetParcelCreating(int parcelId) 
        {
            return DataSource.Parcels[parcelId].Creating;
        }
        /// <summary>
        /// Returns the time the parcel was affiliated with a drone
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> DateTime</returns>
        public static DateTime GetParcelAffiliation(int parcelId) 
        {
            return DataSource.Parcels[parcelId].Affiliation;
        }
        /// <summary>
        /// Returns the time the parcel was picked up
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> DateTime </returns>
        public static DateTime GetParcelPickedUp(int parcelId)   
        {
            return DataSource.Parcels[parcelId].PickedUp;
        }
        /// <summary>
        /// Returns the time the parcel was delivered
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns> DateTime </returns>
        public static DateTime GetParcelDelivered(int parcelId)  
        {
            return DataSource.Parcels[parcelId].Delivered;
        }
        /// <summary>
        /// Adding a station to the next open index
        /// </summary>
        /// <param name="newStation"></param>
        public static void AddStation(Station newStation)  
        {
            Stations[Config.NewStationId++] = newStation;
        }
        /// <summary>
        /// Adding a drone to the next open index
        /// </summary>
        /// <param name="newDrone"></param>
        public static void AddDrone(Drone newDrone)  
        {
            Drones[Config.NewDroneId++] = newDrone;
        }
        /// <summary>
        /// Adding a customer to the next open index
        /// </summary>
        /// <param name="newCustomer"></param>
        public static void AddCustomer(Customer newCustomer)   
        {
            Customers[Config.NewCustomerId++] = newCustomer;
        }
        /// <summary>
        /// Adding a parcel to the next open index
        /// </summary>
        /// <param name="newParcel"></param>
        public static void AddParcel(Parcel newParcel)    
        {
            Parcels[Config.NewParcelId++] = newParcel;
        }
        /// <summary>
        /// if we've found an available drone, we will affiliate the parcel with it
        /// </summary>
        /// <param name="idParcel"></param>
        /// <param name="droneId"></param>
        public static void Affiliate(int idParcel,int droneId)
        {

            for (int i = 0; i < Config.NewDroneId; i++)
            {
                if (Drones[i].Id == droneId)  
                {
                    Drones[i].Status = DroneStatuses.Delivery;
                    Parcels[idParcel].DroneId = Drones[i].Id;
                    Parcels[idParcel].Affiliation = DateTime.Now;
                    return;
                }
            }
        }
        /// <summary>
        /// Changing drone status, and time of pickup
        /// </summary>
        /// <param name="id"></param>
        public static void PickupParcelUpdate(int id)  
        {
            Drones[Parcels[id].DroneId].Status = DroneStatuses.Delivery;  
            Parcels[id].PickedUp = DateTime.Now;  
        }
        /// <summary>
        /// Retrieve the "running number" of the array
        /// </summary>
        /// <returns> Running Number </returns>
        public static int GetStationId()   
        {
            return DataSource.Config.NewStationId;
        }
        /// <summary>
        /// Retrieve the "running number" of the array
        /// </summary>
        /// <returns> Running Number </returns>
        public static int GetDroneId()  
        {
            return DataSource.Config.NewDroneId;
        }
        /// <summary>
        /// Retrieve the "running number" of the array
        /// </summary>
        /// <returns> Running Number </returns>
        public static int GetCustomerId()
        {
            return DataSource.Config.NewCustomerId;
        }
        /// <summary>
        /// Retrieve the "running number" of the array
        /// </summary>
        /// <returns> Running Number </returns>
        public static int GetParcelId()
        {
            return DataSource.Config.NewParcelId;
        }
        /// <summary>
        /// Delivering the parcel
        /// </summary>
        /// <param name="id"></param>
        public static void SupplyParcelUpdate(int id)   
        {
            Drones[Parcels[id].DroneId].Status = DroneStatuses.Available;
            Parcels[id].Delivered = DateTime.Now;
        }

        /// <summary>
        /// We find the charger and the station the drone is  charging again
        /// </summary>
        /// <param name="DroneId"></param>
        public static void ReleaseDroneFromCharger(int DroneId)
        {
            for (int i = 0; i < Config.NewDroneChargeId; i++)
            {
                if (DroneCharges[i].DroneId == DroneId)   
                { 
                    for (int k = 0; k < Config.NewStationId; k++)
                    {
                        if (DroneCharges[i].StationId == Stations[k].Id)  
                        {
                            Stations[k].AvailableChargeSlots++;
                            break;
                        }
                    }
                    DroneCharges[i].DroneId = -1;  
                    DroneCharges[i].StationId = -1;
                }
            }
            for (int i = 0; i < Config.NewDroneId; i++)
            {
                if (Drones[i].Id == DroneId)
                {
                    Drones[i].Status = DroneStatuses.Available;
                    Drones[i].Battery = 100;
                    break;
                }
            }
        }
        /// <summary>
        /// Returning a list of all the Stations
        /// </summary>
        /// <returns> List </returns>
        public static List<Station> PrintBaseStation()  
        {
            List<Station> PrintStation = new List<Station>();
            for (int i = 0; i < GetStationId(); i++)
            {
                PrintStation.Add(GetStation(i));  
            }
            return PrintStation;

        }
        /// <summary>
        /// Returning a list of all the Drones
        /// </summary>
        /// <returns> List </returns>
        public static List<Drone> PrintDrone()   
        {
            List<Drone> PrintDrone = new List<Drone>();
            for (int i = 0; i < GetDroneId(); i++)
            {
                PrintDrone.Add(GetDrone(i));
            }
            return PrintDrone;
        }

        /// <summary>
        /// Returning a list of all the Customers
        /// </summary>
        /// <returns> List </returns>
        public static List<Customer> PrintCustomer()   
        {
            List<Customer> PrintCustomer = new List<Customer>();
            for (int i = 0; i < GetCustomerId(); i++)
            {
                PrintCustomer.Add(GetCustomer(i));
            }
            return PrintCustomer;
        }

        /// <summary>
        /// Returning a list of all the Parcels
        /// </summary>
        /// <returns> List </returns>
        public static List<Parcel> PrintParcel()  
        {
            List<Parcel> PrintParcel = new List<Parcel>();
            for (int i = 0; i < GetParcelId(); i++)
            {
                PrintParcel.Add(GetParcel(i));
            }
            return PrintParcel;
        }

        /// <summary>
        /// Returning a list of all the Parcels that have not been affiliated witha Drone
        /// </summary>
        /// <returns> List </returns>
        public static List<Parcel> PrintParcelOnAir()  
        {
            List<Parcel> PrintParcelOnAir = new List<Parcel>();
            for (int i = 0; i < GetParcelId(); i++)
            {
                if (GetParcel(i).DroneId == -1)
                    PrintParcelOnAir.Add(GetParcel(i));
            }
            return PrintParcelOnAir;
        }

        /// <summary>
        /// Returning the correct Parcel in oreder to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Parcel </returns>
        public static Parcel ParcelDisplay(int id)  
        {
            return GetParcel(id);
        }

        /// <summary>
        /// Returning the correct Station in oreder to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Station </returns>
        public static Station BaseStationDisplay(int id)  
        {
            int i;
            for (i = 0; i < Config.NewStationId; i++)
            {
                if (GetStation(i).Id == id)
                    break;
            }
            return GetStation(i);
        }

        /// <summary>
        /// Adding the stations that have open slot to the list
        /// </summary>
        /// <returns> List </returns>
        public static List<Station> PrintStationsWithOpenSlots()
        {
            List<Station> PrintCustomer = new List<Station>();
            for (int i = 0; i < GetStationId(); i++)
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
        public static Drone DroneDisplay(int id)  
        {
            int i;
            for (i = 0; i < Config.NewDroneId; i++)
            {
                if (GetDrone(i).Id == id)
                    break;
            }
            return GetDrone(i);
        }

        /// <summary>
        /// Returning the correct Customer in oreder to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Customer </returns>
        public static Customer CustomerDisplay(int id)  
        {
            int i;
            for (i = 0; i < Config.NewCustomerId; i++)
            {
                if (GetCustomer(i).Id == id)
                    break;
            }
            return GetCustomer(i);
        }

        /// <summary>
        /// Charging Drone
        /// </summary>
        /// <param name="DroneId"></param>
        public static void FindDroneToCharge(int DroneId)
        {
            int i;
            for ( i = 0; i<Config.NewDroneId ; i++)
            {
                if (Drones[i].Id == DroneId)
                {
                    Drones[i].Status = DroneStatuses.Charging;
                    break;
                }
            }
        }

        /// <summary>
        /// Lowering the available slots by 1 because we've added a drone to charge there
        /// </summary>
        /// <param name="droneCharge"></param>
        /// <param name="StationId"></param>
        public static void AddDroneToCharge(DroneCharge droneCharge,int StationId)
        {
            for (int i = 0; i < Config.NewStationId; i++)
            {
                if (Stations[i].Id== StationId)
                {
                    Stations[i].AvailableChargeSlots--;  
                    break;
                }
            }
            DroneCharges[Config.NewDroneChargeId++] = droneCharge;
        }
    }
}



























