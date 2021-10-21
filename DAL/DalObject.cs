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
        public static DateTime GetParcelCreating(int parcelId)
        {
            return DataSource.Parcels[parcelId].Creating;
        }
        public static DateTime GetParcelAffiliation(int parcelId)
        {
            return DataSource.Parcels[parcelId].Affiliation;
        }
        public static DateTime GetParcelPickedUp(int parcelId)
        {
            return DataSource.Parcels[parcelId].PickedUp;
        }
        public static DateTime GetParcelDelivered(int parcelId)
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
        public static void Affiliate()
        {
            Console.WriteLine(" What is the Parcel Id? \n");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            for (int i = 0; i < Config.NewDroneId; i++)
            {
                if (Drones[i].Status == DroneStatuses.Available)
                {
                    Drones[i].Status = DroneStatuses.Delivery;
                    Parcels[id].DroneId = Drones[i].Id;
                    Parcels[id].Affiliation = DateTime.Now;
                    return;
                }
            }
        }
        public static void PickupParcel()
        {
            Console.WriteLine(" What is the Parcel Id? \n");
            int ID;
            int.TryParse(Console.ReadLine(), out ID);
            Drones[Parcels[ID].DroneId].Status = DroneStatuses.Delivery;
            Parcels[ID].PickedUp = DateTime.Now;
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
        public static void SupplyParcel()
        {
            Console.WriteLine("What is the Parcel Id?");
            int ID;
            int.TryParse(Console.ReadLine(), out ID);
            Drones[Parcels[ID].DroneId].Status = DroneStatuses.Available;
            Parcels[ID].Delivered = DateTime.Now;
        }

        public static void ReleaseDroneFromCharger(int DroneId)
        {
            for (int i = 0; i < Config.NewDroneChargeId; i++)
            {
                if (DroneCharges[i].DroneId == DroneId)
                {
                    for (int k = 0; i < Config.NewStationId; k++)
                    {
                        if (DroneCharges[i].StationId == Stations[k].Id)
                        {
                            Stations[k].AvailableChargeSlots--;
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
        public static List<Station> PrintBaseStation()
        {
            List<Station> PrintStaion = new List<Station>();
            for (int i = 0; i < GetStationId(); i++)
            {
                PrintStaion.Add(GetStation(i));
            }
            return PrintStaion;

        }

        public static List<Drone> PrintDrone()
        {
            List<Drone> PrintDrone = new List<Drone>();
            for (int i = 0; i < GetDroneId(); i++)
            {
                PrintDrone.Add(GetDrone(i));
            }
            return PrintDrone;
        }

        public static List<Customer> PrintCustomer()
        {
            List<Customer> PrintCustomer = new List<Customer>();
            for (int i = 0; i < GetCustomerId(); i++)
            {
                PrintCustomer.Add(GetCustomer(i));
            }
            return PrintCustomer;
        }

        public static List<Parcel> PrintParcel()
        {
            List<Parcel> PrintParcel = new List<Parcel>();
            for (int i = 0; i < GetParcelId(); i++)
            {
                PrintParcel.Add(GetParcel(i));
            }
            return PrintParcel;
        }

        public static List<Parcel> PrintParcelOnAir()
        {
            List<Parcel> PrintParcelOnAi = new List<Parcel>();
            for (int i = 0; i < GetParcelId(); i++)
            {
                if (GetParcel(i).DroneId == -1)
                    PrintParcelOnAi.Add(GetParcel(i));
            }
            return PrintParcelOnAi;
        }

        public static Parcel ParcelDisplay(int id)
        {
            return GetParcel(id);
        }

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



























