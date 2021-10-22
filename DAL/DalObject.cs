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
        public static Station GetStation(int StationId)  /// Returns the station in the certain index
        {
            return DataSource.Stations[StationId];
        }
        public static Drone GetDrone(int droneId)  /// Returns the drone in the certain index
        {
            return DataSource.Drones[droneId];
        }
        public static Customer GetCustomer(int customerId)  /// Returns the customer in the certain index
        {
            return DataSource.Customers[customerId];
        }
        public static Parcel GetParcel(int parcelId)  /// Returns the parcel in the certain index
        {
            return DataSource.Parcels[parcelId];
        }
        public static DateTime GetParcelCreating(int parcelId) /// Returns the time the parcel was created
        {
            return DataSource.Parcels[parcelId].Creating;
        }
        public static DateTime GetParcelAffiliation(int parcelId) /// Returns the time the parcel was affiliated with a drone
        {
            return DataSource.Parcels[parcelId].Affiliation;
        }
        public static DateTime GetParcelPickedUp(int parcelId)   /// Returns the time the parcel was picked up
        {
            return DataSource.Parcels[parcelId].PickedUp;
        }
        public static DateTime GetParcelDelivered(int parcelId)   /// Returns the time the parcel was delivered
        {
            return DataSource.Parcels[parcelId].Delivered;
        }
        public static void AddStation(Station newStation) /// Adding a station to the next open index
        {
            Stations[Config.NewStationId++] = newStation;
        }
        public static void AddDrone(Drone newDrone)  /// Adding a drone to the next open index
        {
            Drones[Config.NewDroneId++] = newDrone;
        }
        public static void AddCustomer(Customer newCustomer)   /// Adding a customer to the next open index
        {
            Customers[Config.NewCustomerId++] = newCustomer;
        }
        public static void AddParcel(Parcel newParcel)    /// Adding a parcel to the next open index
        {
            Parcels[Config.NewParcelId++] = newParcel;
        }
        public static void Affiliate(int id)
        {

            for (int i = 0; i < Config.NewDroneId; i++)
            {
                if (Drones[i].Status == DroneStatuses.Available)  // if we've found an available drone, we will affiliate the parcel with it
                {
                    Drones[i].Status = DroneStatuses.Delivery;
                    Parcels[id].DroneId = Drones[i].Id;
                    Parcels[id].Affiliation = DateTime.Now;
                    return;
                }
            }
        }
        public static void PickupParcelUpdate(int id)  
        {
            Drones[Parcels[id].DroneId].Status = DroneStatuses.Delivery;  
            Parcels[id].PickedUp = DateTime.Now;
        }

        public static int GetStationId()
        {
            return DataSource.Config.NewStationId;
        }
        public static int GetDroneId()
        {
            return DataSource.Config.NewDroneId;
        }
        public static int GetCustomerId()
        {
            return DataSource.Config.NewCustomerId;
        }
        public static int GetParcelId()
        {
            return DataSource.Config.NewParcelId;
        }
        public static void SupplyParcelUpdate(int id)   /// Delivering the parcel
        {
            Drones[Parcels[id].DroneId].Status = DroneStatuses.Available;
            Parcels[id].Delivered = DateTime.Now;
        }

        public static void ReleaseDroneFromCharger(int DroneId)
        {
            for (int i = 0; i < Config.NewDroneChargeId; i++)
            {
                if (DroneCharges[i].DroneId == DroneId)
                {
                    for (int k = 0; k < Config.NewStationId; k++)
                    {
                        if (DroneCharges[i].StationId == Stations[k].Id)  /// And found the station the charger is in
                        {
                            Stations[k].AvailableChargeSlots++;
                            break;
                        }
                    }
                    DroneCharges[i].DroneId = -1;  /// Give the charger default values
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
        public static List<Station> PrintBaseStation()  /// Returning a list of all the Stations
        {
            List<Station> PrintStation = new List<Station>();
            for (int i = 0; i < GetStationId(); i++)
            {
                PrintStation.Add(GetStation(i));  /// Adding to a list
            }
            return PrintStation;

        }

        public static List<Drone> PrintDrone()   /// Returning a list of all the Drones
        {
            List<Drone> PrintDrone = new List<Drone>();
            for (int i = 0; i < GetDroneId(); i++)
            {
                PrintDrone.Add(GetDrone(i));
            }
            return PrintDrone;
        }

        public static List<Customer> PrintCustomer()   /// Returning a list of all the Customers
        {
            List<Customer> PrintCustomer = new List<Customer>();
            for (int i = 0; i < GetCustomerId(); i++)
            {
                PrintCustomer.Add(GetCustomer(i));
            }
            return PrintCustomer;
        }

        public static List<Parcel> PrintParcel()  /// Returning a list of all the Parcels
        {
            List<Parcel> PrintParcel = new List<Parcel>();
            for (int i = 0; i < GetParcelId(); i++)
            {
                PrintParcel.Add(GetParcel(i));
            }
            return PrintParcel;
        }

        public static List<Parcel> PrintParcelOnAir()  /// Returning a list of all the Parcels that have not been affiliated witha Drone
        {
            List<Parcel> PrintParcelOnAir = new List<Parcel>();
            for (int i = 0; i < GetParcelId(); i++)
            {
                if (GetParcel(i).DroneId == -1)
                    PrintParcelOnAir.Add(GetParcel(i));
            }
            return PrintParcelOnAir;
        }

        public static Parcel ParcelDisplay(int id)  /// Returning the correct Parcel in oreder to be displayed to the console
        {
            return GetParcel(id);
        }

        public static Station BaseStationDisplay(int id)  /// Returning the correct Station in oreder to be displayed to the console
        {
            int i;
            for (i = 0; i < Config.NewStationId; i++)
            {
                if (GetStation(i).Id == id)
                    break;
            }
            return GetStation(i);
        }

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

        public static Drone DroneDisplay(int id)  /// Returning the correct Drone in oreder to be displayed to the console
        {
            int i;
            for (i = 0; i < Config.NewDroneId; i++)
            {
                if (GetDrone(i).Id == id)
                    break;
            }
            return GetDrone(i);
        }

        public static Customer CustomerDisplay(int id)  /// Returning the correct Customer in oreder to be displayed to the console
        {
            int i;
            for (i = 0; i < Config.NewCustomerId; i++)
            {
                if (GetCustomer(i).Id == id)
                    break;
            }
            return GetCustomer(i);
        }

        public static int FindDroneToCharge(int DroneId)
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
            return i;
        }

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



























