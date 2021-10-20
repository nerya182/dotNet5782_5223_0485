using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    public class DataSource
    {
        internal class Config 
        {
            internal static int newCustomerId = 0;
            internal static int newStationId = 0;
            internal static int newDroneId=0;
            internal static int newParcelId=0;
            internal static int newDroneChargeId = 0;
        }
       

        public static Customer[] customers = new Customer[100];
        public static Station[] stations = new Station[5];
        public static Drone[] drones = new Drone[10];
        public static Parcel[] parcels = new Parcel[1000];
        public static DroneCharge[] droneCharges = new DroneCharge[5];

        
        public static void Initialize()
        {
            CreateStations();
            CreateDrones();
            CreateCustomers();
            CreateParcels();
            CreateDroneCharge();
        }

        
        private static void CreateStations()
        {
            stations[Config.newStationId] = new Station()
            {
                Id = Config.newStationId,
                Name= "Tachana Merkazit",
                AvailableChargeSlots = 10,
                Lattitude = 31.78907,
                Longitude = 35.20319
            };
            Config.newStationId++;

            stations[Config.newStationId]=new Station()
            {
                Id=Config.newStationId,
                Name= "Shuk Machane Yehudah",
                AvailableChargeSlots = 10,
                Lattitude = 31.78489,
                Longitude = 35.21257
            };
            Config.newStationId++;      
        }

        private static void  CreateDrones()
        {
            Random R = new Random();
            for (int i=0;i<5;i++, Config.newDroneId++)
            {
                NameDrone nameDrone = (NameDrone)R.Next(0,9);
                WeightCategories weightCategories = (WeightCategories)R.Next(1, 3);
                Drone drone = new Drone() { Id = Config.newDroneId, Model = nameDrone.ToString(), MaxWeight = weightCategories, Status = DroneStatuses.Available, Battery = 100 };
                drones[i] = drone;
            }
        }

        private static void CreateCustomers()
        {
            Random R = new Random();
            for (int i = 0; i < 10; i++, Config.newCustomerId++)
            {
                CustomerName customerName = (CustomerName)R.Next(0, 11);
                Customer customer = new Customer() { Id = Config.newDroneId, Name = customerName.ToString(), Phone = "050" + R.Next(111111, 999999), Lattitude = R.Next(3000000, 3700000), Longitude = R.Next(3000000, 3700000) };
                customers[i] = customer;
            }
        }

        private static void CreateParcels()
        {
            Random R = new Random();
            for (int i = 0; i < 10; i++, Config.newParcelId++)
            {
                Priorities priorities = (Priorities)R.Next(1, 3);
                WeightCategories weightCategories = (WeightCategories)R.Next(1, 3);
                Parcel parcel = new Parcel() { Id = Config.newParcelId, SenderId = R.Next(), TargetId = R.Next(), DroneId = R.Next(0,Config.newDroneId), Weight = weightCategories, Priority = priorities, Creating = DateTime.Now, Delivered = DateTime.Now
                    , PickedUp = DateTime.Now, Affiliation = DateTime.Now };
                parcels[i] = parcel;
            }
        }

        private static void CreateDroneCharge()
        {
            Random R = new Random();
            for (int i = 0; i < 5; i++, Config.newDroneChargeId++)
            {
                DroneCharge droneCharge = new DroneCharge() { DroneId = R.Next(0, Config.newDroneId), StationId = R.Next(0, Config.newStationId) };
                droneCharges[i] = droneCharge;
            }
        }
    }
}



























