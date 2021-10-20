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

        public static Station GetStation(int StationId)
        {
            return DataSource.stations[StationId];
        }

        public static Drone GetDrone(int droneId)
        {
            return DataSource.drones[droneId];
        }

        public static Customer GetCustomer(int customerId)
        {
            return DataSource.customers[customerId];
        }

        public static Parcel GetParcel(int parcelId)
        {
            return DataSource.parcels[parcelId];
        }
        public static DateTime GetParcelCreating(int parcelId)
        {
            return DataSource.parcels[parcelId].Creating;
        }
        public static DateTime GetParcelAffiliation(int parcelId)
        {
            return DataSource.parcels[parcelId].Affiliation;
        }

        public static DateTime GetParcelPickedUp(int parcelId)
        {
            return DataSource.parcels[parcelId].PickedUp;
        }

        public static DateTime GetParcelDelivered(int parcelId)
        {
            return DataSource.parcels[parcelId].Delivered;
        }

        public static void AddStation(Station newStation)
        { 
            stations[Config.newStationId++] = newStation;
        }

        public static void addDrone(Drone newDrone)
        {
            drones[Config.newDroneId++] = newDrone;
        }

        public static void Affiliate()
        {
            Console.WriteLine(" What is the Parcel Id? \n");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            //Parcel parcel = GetParcel(ID);
            for (int i = 0; i < Config.newDroneId; i++)
            {
                if (drones[i].Status == DroneStatuses.Available)
                {
                    drones[i].Status = DroneStatuses.Delivery;
                    parcels[id].DroneId = drones[i].Id;
                    parcels[id].Affiliation = DateTime.Now ;
                    return;
                }
            }
        }
        public static void PickupParcel()
        {
            Console.WriteLine(" What is the Parcel Id? \n");
            int ID;
            int.TryParse(Console.ReadLine(), out ID);
            drones[parcels[ID].DroneId].Status = DroneStatuses.Delivery;
            parcels[ID].PickedUp = DateTime.Now;
        }

        public static void addParcel(Parcel newParcel)
        {
            parcels[Config.newParcelId++] = newParcel;  
        }

        public static int GetStationId()
        {
            return DataSource.Config.newStationId;
        }

        public static int GetDroneId()
        {
            return DataSource.Config.newDroneId;
        }
        public static int GetCustomerId()
        {
            return DataSource.Config.newCustomerId;
        }
        public static int GetParcelId()
        {
            return DataSource.Config.newParcelId;
        }
        public static void SupplyParcel()
        {
            Console.WriteLine(" What is the Parcel Id? \n");
            int ID;
            int.TryParse(Console.ReadLine(), out ID);
            drones[parcels[ID].DroneId].Status = DroneStatuses.Available;
            parcels[ID].Delivered = DateTime.Now;
        }
        public static void SendDroneToCharge()
        {
            Console.WriteLine(" What is the Drone Id? \n");
            int DroneId, StationId;
            int.TryParse(Console.ReadLine(), out DroneId);
            for (int i = 0; i < Config.newDroneId; i++)
            {
                if (drones[i].Id == DroneId)
                    drones[i].Status = DroneStatuses.Charging;
            }
            
            Console.WriteLine(" Which Sattion would you like to charge your Drone? \n");
            int.TryParse(Console.ReadLine(), out StationId);
            PrintStationsWithOpenSlots();
            for(int i = 0; i<Config.newStationId; i++)
            {
                if(stations[i].Id == StationId)
                    stations[i].AvailableChargeSlots--;
            }

            DroneCharge droneCharge = new DroneCharge();  // adding a DroneCharge
            droneCharge.StationId = StationId;
            droneCharge.DroneId = DroneId;
            droneCharges[Config.newDroneChargeId++] = droneCharge;

        }
        public static void AddCustomer(Customer newCustomer)
        {
            customers[Config.newCustomerId++] = newCustomer;
        }

        public static void ReleaseDroneFromCharger()
        {
            Console.WriteLine(" What is the Drone Id? \n");
            int DroneId;
            int.TryParse(Console.ReadLine(), out DroneId);
            for(int i = 0; i < Config.newDroneChargeId; i++)
            {
                if(droneCharges[i].DroneId == DroneId)
                {                   
                    for (int k = 0; i < Config.newStationId; k++)
                    {
                        if (droneCharges[i].StationId == stations[k].Id)
                        {
                            stations[k].AvailableChargeSlots--;   
                        }                           
                    }
                    droneCharges[i].DroneId = -1;
                    droneCharges[i].StationId = -1;
                }
            }
            for (int i = 0; i < Config.newDroneId; i++)
            {
                if (drones[i].Id == DroneId)
                {
                    drones[i].Status = DroneStatuses.Available;
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
            for (i = 0; i < Config.newStationId; i++)
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
                if (GetStation(i).AvailableChargeSlots>0)
                    PrintCustomer.Add(GetStation(i));
            }
            return PrintCustomer;
        }

        public static Drone DroneDisplay(int id)
        {
            int i;
            for ( i = 0; i < Config.newDroneId; i++)
            {
                if (GetDrone(i).Id == id)
                    break;
            }
            return GetDrone(i);
        }

        public static Customer CustomerDisplay(int id)
        {
            int i;
            for ( i = 0; i < Config.newCustomerId; i++)
            {
                if (GetCustomer(i).Id == id)
                    break;
            }
            return GetCustomer(i);
        }
      




    }
}



























