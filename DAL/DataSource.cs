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

        }
       

        public static Customer[] customers = new Customer[100];
        public static Station[] stations = new Station[5];

        internal static void Initialize()
        {
            CreateStations();
        }
        private static void CreateStations()
        {

        }
        
    }
}

