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
            int ID;
            int.TryParse(Console.ReadLine(), out ID);
            //Parcel parcel = GetParcel(ID);
            for (int i = 0; i < Config.newDroneId; i++)
            {
                if (drones[i].Status == DroneStatuses.Available)
                {
                    drones[i].Status = DroneStatuses.Delivery;
                    parcels[ID].DroneId = drones[i].Id;
                    break;
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

        public static void PrintStationsWithOpenSlots()
        {
            for (int i = 0; i < DalObject.GetStationId(); i++)
            {
                if (DalObject.GetStation(i).AvailableChargeSlots > 0)
                    Console.WriteLine(DalObject.GetStation(i).ToString());
            }
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

        public static void ParcelDisplay(int id)
        {
            string st;
            st=parcels[id].ToString()+"\n";
            Console.WriteLine(st);
        }

        public static void BaseStationDisplay(int id)
        {
            string st;
            for (int i = 0; i <= Config.newStationId; i++)
            {
                if (DataSource.stations[i].Id == id)
                {
                    st=DataSource.stations[i].ToString()+"\n";
                    Console.WriteLine(st);
                    return;
                }
            }
        }

        public static void DroneDisplay(int id)
        {
            for (int i = 0; i <= Config.newDroneId; i++)
            {
                if (DataSource.drones[i].Id == id)
                {
                    DataSource.drones[i].ToString();
                    return;
                }
            }
        }

        public static void CustomerDisplay(int id)
        {
            for (int i = 0; i <= Config.newCustomerId; i++)
            {
                if (DataSource.customers[i].Id == id)
                {
                    DataSource.customers[i].ToString();
                    return;
                }
            }
        }




    }
}



























