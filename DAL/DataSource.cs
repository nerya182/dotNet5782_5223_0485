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

        
        internal static void Initialize()
        {
            CreateStations();
            CreateDrones();
            CreateCustomers();
            CreateParcels();
        }

        
        private static void CreateStations()
        {
            stations[Config.newStationId] = new Station()
            {
                Id = Config.newStationId,
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
            Random R = new Random();
            for (int i=0;i<5;i++, Config.newDroneId++)
            {
                NameDrone nameDrone = (NameDrone)R.Next(0,9);
                WeightCategories weightCategories = (WeightCategories)R.Next(0, 2);
                Drone drone = new Drone() { Id = Config.newDroneId, Model = nameDrone.ToString(), MaxWeight = weightCategories, Status = DroneStatuses.Available, Battery = 100 };
                drones[i] = drone;
            }
        }

        private static void CreateCustomers()
        {


            customers[Config.newCustomerId] = new Customer()
            {
              Id = Config.newDroneId,
              Name = "Avi",
              Phone = "050-1112222",
              Longitude = 30.99999,
              Lattitude = 36.12457
            }; 
            Config.newCustomerId++;

        }

        private static void CreateParcels()
        {
            
        }
    }

}



























