using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO


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
        }
        private static void CreateStations()
        {

        }
        private static void CreateStations()
        {

        }   
    }
}

