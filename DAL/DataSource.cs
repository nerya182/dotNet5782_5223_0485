using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    public  class DataSource
    {
        internal class Config 
        {
            internal static int NewCustomerId = 0;
            internal static int NewStationId = 0;
            internal static int NewDroneId=0;
            internal static int NewParcelId=0;
            internal static int NewDroneChargeId = 0;
        }
       
        public static Customer[] Customers = new Customer[100];
        public static Station[] Stations = new Station[5];
        public static Drone[] Drones = new Drone[10];
        public static Parcel[] Parcels = new Parcel[1000];
        public static DroneCharge[] DroneCharges = new DroneCharge[5];
 
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
            Stations[Config.NewStationId] = new Station()
            {
                Id = 56,
                Name = "Tachana Merkazit",
                AvailableChargeSlots = 10,
                Lattitude = 31.78907,
                Longitude = 35.20319
            };
            Config.NewStationId++;

            Stations[Config.NewStationId]=new Station()
            {
                Id=67,
                Name= "Shuk Machane Yehudah",
                AvailableChargeSlots = 10,
                Lattitude = 31.78489,
                Longitude = 35.21257
            };
            Config.NewStationId++;      
        }

        private static void  CreateDrones()
        {
            Random R = new Random();
            for (int i=0;i<5;i++, Config.NewDroneId++) // Creating 5 Drones Randomly
            {
                NameDrone nameDrone = (NameDrone)R.Next(0,9);
                WeightCategories weightCategories = (WeightCategories)R.Next(1, 3);
                Drone drone = new Drone() { Id = Config.NewDroneId, Model = nameDrone.ToString(), MaxWeight = weightCategories, Status = DroneStatuses.Available, Battery = 100 };
                Drones[i] = drone;
            }
        }

        private static void CreateCustomers()  // Creating 10 Customers Randomly
        {
            Random R = new Random();
            for (int i = 0; i < 10; i++, Config.NewCustomerId++)
            {
                CustomerName customerName = (CustomerName)R.Next(0, 11);
                Customer customer = new Customer() { Id = R.Next(100000000,1000000000), Name = customerName.ToString(), Phone = "050" + R.Next(111111, 999999), Lattitude = R.Next(3000000, 3700000), Longitude = R.Next(3000000, 3700000) };
                Customers[i] = customer;
            }
        }

        private static void CreateParcels()  // Creating 10 Parcels Randomly
        {
            Random R = new Random();
            for (int i = 0; i < 10; i++, Config.NewParcelId++)
            {
                Priorities priorities = (Priorities)R.Next(1, 3);
                WeightCategories weightCategories = (WeightCategories)R.Next(1, 3);
                Parcel parcel = new Parcel() { Id = Config.NewParcelId, SenderId = R.Next(), TargetId = R.Next(), DroneId =R.Next(-1,Config.NewDroneId+1), Weight = weightCategories, Priority = priorities, Creating = DateTime.Now, Delivered = DateTime.Now
                    , PickedUp = DateTime.Now, Affiliation = DateTime.Now };
                Parcels[i] = parcel;
            }
        }

        private static void CreateDroneCharge()  // Creating 5 DroneCharges Randomly
        {
            Random R = new Random();
            for (int i = 0; i < 5; i++, Config.NewDroneChargeId++)
            {
                DroneCharge droneCharge = new DroneCharge() { DroneId = R.Next(0, Config.NewDroneId), StationId = R.Next(0, Config.NewStationId) };
                DroneCharges[i] = droneCharge;
            }
        }
    }
}



























