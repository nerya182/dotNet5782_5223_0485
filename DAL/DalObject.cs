﻿using System;
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
            Station newStation = new Station();

            Console.WriteLine("enter name od staion\n");
            newStation.Name = Console.ReadLine();
           
            Console.WriteLine("Enter the longitude of the station\n");
            newStation.Longitude = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Enter the Lattitude of the station\n");
            newStation.Lattitude = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Enter the number of charging points available at the station\n");
            newStation.ChargeSlots = int.Parse(Console.ReadLine());

            newStation.Id = Config.newStationId;

            stations[Config.newStationId++] = newStation;
        }









    }
}




