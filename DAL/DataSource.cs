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
        }
       

        public static Customer[] customers = new Customer[100];
        public static Station[] stations = new Station[5];
        public static Drone[] drones = new Drone[10];
        public static Parcel[] parcels = new Parcel[1000];

        public static string Avi { get; private set; }

        internal static void Initialize()
        {
            CreateStations();
            CreateDrones();
            CreateCustomers();
            CreateParcels();
        }

        public static Random
        private static void CreateStations()
        {
            stations[Config.newStationId]= new Station()
            {
                Id=Config.newStationId,
                Name= "Tachana Merkazit",
                ChargeSlots = 10,
                Lattitude = 31.78907,
                Longitude = 35.20319
            };
            Config.newStationId++;

            stations[Config.newStationId]=new Station()
            {
                Id=Config.newStationId,
                Name= "Shuk Machane Yehudah",
                ChargeSlots = 10,
                Lattitude = 31.78489,
                Longitude = 35.21257
            };
            Config.newStationId++;      
        }
        private static void  CreateDrones()
        {
            drones[Config.newDroneId]= new Drone()
            {
                Id=Config.newDroneId,
                Model= "mavic",
                MaxWeight = WeightCategories.Heavy,
                Status = DroneStatuses.Available,
                Battery = 100
            };
            Config.newDroneId++;

            drones[Config.newDroneId]= new Drone()
            {
                Id=Config.newDroneId,
                Model= "tello",
                MaxWeight = WeightCategories.Light,
                Status = DroneStatuses.Available,
                Battery = 100
            };
            Config.newDroneId++;

            drones[Config.newDroneId] = new Drone()
            {
                Id = Config.newDroneId,
                Model = "syma",
                MaxWeight = WeightCategories.Medium,
                Status = DroneStatuses.Available,
                Battery = 100
            };
            Config.newDroneId++;

            drones[Config.newDroneId] = new Drone()
            {
                Id = Config.newDroneId,
                Model = "cobra",
                MaxWeight = WeightCategories.Medium,
                Status = DroneStatuses.Available,
                Battery = 100
            };
            Config.newDroneId++;

            drones[Config.newDroneId] = new Drone()
            {
                Id = Config.newDroneId,
                Model = "parrot",
                MaxWeight = WeightCategories.Light,
                Status = DroneStatuses.Available,
                Battery = 100
            };
            Config.newDroneId++;
        }
        private static void CreateCustomers()
        {
            customers[Config.newCustomerId] = new Customer()
            {
              Id = 111111111,
              Name = "Avi",
              Phone = "050-1112222",
              Longitude = 30.99999,
              Lattitude = 36.12457
            }; 
            Config.newCustomerId++;

            customers[Config.newCustomerId] = new Customer()
            {
                Id = 111111111,
                Name = "Benny",
                Phone = "050 - 1112222",
                Longitude = 30.99999,
                Lattitude = 36.12457
            };
            Config.newCustomerId++;

            customers[Config.newCustomerId] = new Customer()
            {
                Id = Config.newCustomerId,
                Name = Danny,
                Phone = 050 - 1112222,
                Longitude = 30.99999,
                Lattitude = 36.12457
            };
            Config.newCustomerId++;

            customers[Config.newCustomerId] = new Customer()
            {
                Id = Config.newCustomerId,
                Name = Avi,
                Phone = 050 - 1112222,
                Longitude = 30.99999,
                Lattitude = 36.12457
            };
            Config.newCustomerId++;

            customers[Config.newCustomerId] = new Customer()
            {
                Id = Config.newCustomerId,
                Name = Avi,
                Phone = 050 - 1112222,
                Longitude = 30.99999,
                Lattitude = 36.12457
            };
            Config.newCustomerId++;

            customers[Config.newCustomerId] = new Customer()
            {
                Id = Config.newCustomerId,
                Name = Avi,
                Phone = 050 - 1112222,
                Longitude = 30.99999,
                Lattitude = 36.12457
            };
            Config.newCustomerId++;

            customers[Config.newCustomerId] = new Customer()
            {
                Id = Config.newCustomerId,
                Name = Avi,
                Phone = 050 - 1112222,
                Longitude = 30.99999,
                Lattitude = 36.12457
            };
            Config.newCustomerId++;

            customers[Config.newCustomerId] = new Customer()
            {
                Id = Config.newCustomerId,
                Name = Avi,
                Phone = 050 - 1112222,
                Longitude = 30.99999,
                Lattitude = 36.12457
            };
            Config.newCustomerId++;

            customers[Config.newCustomerId] = new Customer()
            {
                Id = Config.newCustomerId,
                Name = Avi,
                Phone = 050 - 1112222,
                Longitude = 30.99999,
                Lattitude = 36.12457
            };
            Config.newCustomerId++;

            customers[Config.newCustomerId] = new Customer()
            {
                Id = Config.newCustomerId,
                Name = Avi,
                Phone = 050 - 1112222,
                Longitude = 30.99999,
                Lattitude = 36.12457
            };
        }

        private static void CreateParcels()
        {
            
        }
    }

}



























