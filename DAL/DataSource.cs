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
        /// <summary>
        /// Creating 2 stations
        /// </summary>
        private static void CreateStations()
        {
            Stations[Config.NewStationId] = new Station()
            {
                Id = 56,
                Name = "Tachana Merkazit",
                AvailableChargeSlots = 10,
                Lattitude = 31.78945,
                Longitude = 35.20301
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
        /// <summary>
        /// Creating 5 Drones Randomly
        /// </summary>
        private static void  CreateDrones()
        {
            Random R = new Random();
            for (int i=0;i<5;i++, Config.NewDroneId++) 
            {
                NameDrone nameDrone = (NameDrone)R.Next(0,9);
                WeightCategories weightCategories = (WeightCategories)R.Next(1, 3);
                Drone drone = new Drone() { Id = R.Next(1000,100001), Model = nameDrone.ToString(), MaxWeight = weightCategories, Status = DroneStatuses.Available, Battery = 100 };
                Drones[i] = drone;
            }
        }
        /// <summary>
        /// Creating 10 Customers Randomly
        /// </summary>
        private static void CreateCustomers()  
        {
            Random R = new Random();
            for (int i = 0; i < 10; i++, Config.NewCustomerId++)
            {
                CustomerName customerName = (CustomerName)R.Next(0, 11);
                Customer customer = new Customer() { Id = R.Next(100000000, 1000000000), Name = customerName.ToString(), Phone = "050" + R.Next(111111, 999999), Lattitude = R.Next(3180424, 3276699) / 100000.0, Longitude = R.Next(3502056, 3520499) / 100000.0 };
                Customers[i] = customer;
            }
        }
        /// <summary>
        /// Creating 10 Parcels Randomly
        /// </summary>
        private static void CreateParcels()  
        {
            Random R = new Random();
            for (int i = 0; i < 10; i++, Config.NewParcelId++)
            {
                Priorities priorities = (Priorities)R.Next(1, 3);
                WeightCategories weightCategories = (WeightCategories)R.Next(1, 3);
                Parcel parcel = new Parcel() { Id = Config.NewParcelId, SenderId = R.Next(100000000, 1000000000), TargetId = R.Next(100000000, 1000000000), DroneId =R.Next(-1,Config.NewDroneId), Weight = weightCategories, Priority = priorities, Creating = DateTime.Now, Delivered = DateTime.Now
                    , PickedUp = DateTime.Now, Affiliation = DateTime.Now };
                Parcels[i] = parcel;
            }
        }
        /// <summary>
        /// Creating 2 DroneCharges Randomly
        /// </summary>
        private static void CreateDroneCharge()  
        {
            Random R = new Random();
            for (int i = 0; i < 2; i++, Config.NewDroneChargeId++)
            {
                DroneCharge droneCharge = new DroneCharge() { DroneId = R.Next(0, Config.NewDroneId), StationId = R.Next(0, Config.NewStationId) };
                DroneCharges[i] = droneCharge;
            }
        }
    }
}



























