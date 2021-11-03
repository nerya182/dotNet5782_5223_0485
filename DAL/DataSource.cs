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
            internal static int NewParcelId = 0;
        }
    
        public static List<Customer> Customers = new List<Customer>();
        public static List<Station> Stations = new List<Station>();
        public static List<Drone> Drones = new List<Drone>();
        public static List<Parcel> Parcels = new List<Parcel>();
        public static List<DroneCharge> DroneCharges = new List<DroneCharge>();

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
            Station newStation1 = new Station() { Id = 56, Name = "Tachana Merkazit", AvailableChargeSlots = 10, Lattitude = 31.78945, Longitude = 35.20301 };
            Stations.Add(newStation1);

            Station newStation2 = new Station() { Id = 67, Name = "Shuk Machane Yehudah", AvailableChargeSlots = 10, Lattitude = 31.78489, Longitude = 35.21257 };
            Stations.Add(newStation2);
        }
        /// <summary>
        /// Creating 5 Drones Randomly
        /// </summary>
        private static void CreateDrones()
        {
            Random R = new Random();
            for (int i = 0; i < 5; i++)
            {
                NameDrone nameDrone = (NameDrone)R.Next(0, 9);
                WeightCategories weightCategories = (WeightCategories)R.Next(1, 3);
                Drone newDrone = new Drone() { Id = R.Next(1000, 100001), Model = nameDrone.ToString(), MaxWeight = weightCategories};
                Drones.Add(newDrone);
            }
        }
        /// <summary>
        /// Creating 10 Customers Randomly
        /// </summary>
        private static void CreateCustomers()
        {
            Random R = new Random();
            for (int i = 0; i < 10; i++)
            {
                CustomerName customerName = (CustomerName)R.Next(0, 11);
                Customer newCustomer = new Customer() { Id = R.Next(100000000, 1000000000), Name = customerName.ToString(), Phone = "050" + R.Next(111111, 999999), Lattitude = R.Next(3180424, 3276699) / 100000.0, Longitude = R.Next(3502056, 3520499) / 100000.0 };
                Customers.Add(newCustomer);
            }
        }
        /// <summary>
        /// Creating 10 Parcels Randomly
        /// </summary>
        private static void CreateParcels()
        {
            Random R = new Random();
            for (int i = 0; i < 10; i++)
            {
                Priorities priorities = (Priorities)R.Next(1, 3);
                WeightCategories weightCategories = (WeightCategories)R.Next(1, 3);
                Parcel newParcel = new Parcel()
                {
                    Id = Config.NewParcelId,
                    SenderId = R.Next(100000000, 1000000000),
                    TargetId = R.Next(100000000, 1000000000),
                    DroneId = R.Next(-1, Drones.Count),
                    Weight = weightCategories,
                    Priority = priorities,
                    Creating = DateTime.Now,
                    Delivered = DateTime.Now
                    ,
                    PickedUp = DateTime.Now,
                    Affiliation = DateTime.Now
                };
                Parcels.Add(newParcel);
                Config.NewParcelId++;
            }
        }
        /// <summary>
        /// Creating 2 DroneCharges Randomly
        /// </summary>
        private static void CreateDroneCharge()
        {
            Random R = new Random();
            for (int i = 0; i < 2; i++)
            {
                DroneCharge newDroneCharge = new DroneCharge() { DroneId = R.Next(0, Drones.Count), StationId = R.Next(0, Stations.Count) };
                DroneCharges.Add(newDroneCharge);
            }
        }
    }
}



























