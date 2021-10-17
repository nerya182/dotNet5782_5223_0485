using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

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









    }
}




