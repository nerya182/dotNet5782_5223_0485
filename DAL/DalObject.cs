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
        private static object sataion;

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

        public static Parcel GeParcel(int parcelId)
        {
            return DataSource.parcels[parcelId];
        }
        public static Parcel GeParcelRequested(int parcelId)
        {
            return DataSource.parcels[parcelId].Requested();
        }
        public static Parcel GeParcelScheduled(int parcelId)
        {
            return DataSource.parcels[parcelId].Scheduled();
        }

        public static Parcel GeParcelPickedUp(int parcelId)
        {
            return DataSource.parcels[parcelId].Scheduled();
        }

        public static Parcel GeParcelDelivered(int parcelId)
        {
            return DataSource.parcels[parcelId].Delivered();
        }

        public static void AddStation()
        {
            Console.WriteLine("enter name od staion\n");
            string nameStaion = Console.ReadLine();
            Console.WriteLine("Enter the longitude of the station\n");
            double longitudeStaion = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Lattitude of the station\n");
            double lattitudeStaion = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of charging points available at the station\n");
            int chargeSlotsStaion = int.Parse(Console.ReadLine());
            Station newStation = new Station() { Id = Config.newStationId,Name= nameStaion ,Longitude=longitudeStaion ,Lattitude=lattitudeStaion,ChargeSlots=chargeSlotsStaion};
            stations[Config.newStationId] = newStation;
        }









    }
}




