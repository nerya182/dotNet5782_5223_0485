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
        public static Parcel GetParcelRequested(int parcelId)
        {
            return DataSource.parcels[parcelId].Requested();
        }
        public static Parcel GetParcelScheduled(int parcelId)
        {
            return DataSource.parcels[parcelId].Scheduled();
        }

        public static Parcel GetParcelPickedUp(int parcelId)
        {
            return DataSource.parcels[parcelId].Scheduled();
        }

        public static Parcel GetParcelDelivered(int parcelId)
        {
            return DataSource.parcels[parcelId].Delivered();
        }

        public static void AddStation(int myId,string  myName, double myLongitude,double myLattitude,  int myAvailableChargeSlots)
        {
            Station newStation = new Station() { Id=myId, Name=myName, Longitude=myLongitude, Lattitude=myLattitude, AvailableChargeSlots=myAvailableChargeSlots};
            stations[Config.newStationId++] = newStation;
        }

        public static void addDrone(int myId, string myModel, WeightCategories myMaxWeight, DroneStatuses myStatuses,  double myBattery)
        {
            Drone newDrone = new Drone() { Id = myId, Model = myModel, MaxWeight = myMaxWeight, Status = myStatuses, Battery = myBattery };
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

        public static void addParcel(int myId, int SenderId, int TargetId, WeightCategories myMaxWeight, Priorities myPriority,int myDroneId, DateTime CreatePackage)
        {
            Parcel newParcel = new Parcel() { Id = myId, SenderId = SenderId, TargetId = TargetId, Weight = myMaxWeight, Priority = myPriority,DroneId=myDroneId};
            parcels[myId] = newParcel;
            Config.newParcelId++;
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
            int ID;
            int.TryParse(Console.ReadLine(), out ID);
            Console.WriteLine(" Which Sattion would you like to charge your Drone? \n");
            PrintStationsWithOpenSlots();

        }
        public static void AddCustomer(int myId, string myName, string myPhone,Double myLongitude, double myLattitude)
        {
            Customer newCustomer = new Customer() {Id=myId,Name=myName,Phone=myPhone,Longitude=myLongitude,Lattitude=myLattitude };
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
    }
}



























