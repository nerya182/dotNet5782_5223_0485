﻿using System;
using System.Collections.Generic;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DalObject.DataSource.Initialize();
            Console.WriteLine("\nMenu:\n" +
                       "ADD- Add a new base Station/Drone/Customer/Parcel.\n" +
                       "UPDATE- Update assignment/Collection /Delivery /Charging /Release.\n" +
                       "DISPLAY- Display of base stations/Drone/Customer/ Parcel\n" +
                       "VIEW_LIST- Print all bbase stations/Drone/Customer/Parcel/Packages not yet associated/Base stations with available charging stations.\n" +
                       "EXIT- Exit\n");
            CHOICE choice;
            Enum.TryParse(Console.ReadLine(), out choice);
            while (choice != CHOICE.EXIT)
            {              
                switch (choice)
                {
                    case CHOICE.ADD:
                        Console.WriteLine("What would you like to add? \n" +
                                         "a- Add a new base Station.\n" +
                                         "b- Add a new Drone.\n" +
                                         "c-Add a new Customer.\n" +
                                         "d-Add a new Parcel.\n");
                        char input;
                        char.TryParse(Console.ReadLine(), out input);
                        switch (input)
                        {
                            case 'a':
                                AddStation();
                                break;
                            case 'b':
                                AddDrone();
                                break;
                            case 'c':
                                AddCustomer();
                                break;
                            case 'd':
                                AddParcel();
                                break;
                        }
                        break;
                    case CHOICE.UPDATE:
                        Console.WriteLine(" What would you like to do?" +
                            "1-Affiliate Parcel to Drone?\n" +
                            "2- Pickup Parcel with Drone? \n" +
                            "3- Deliver Parcel to Customer? \n" +
                            "4- Send Drone to Charge? \n" +
                            "5- Release Drone from Charge? \n");
                        int info;
                        int.TryParse(Console.ReadLine(), out info);
                        switch (info)
                        {
                            case 1:
                                DalObject.DalObject.Affiliate();
                                break;
                            case 2:
                                DalObject.DalObject.PickupParcel();
                                break;
                            case 3:
                                DalObject.DalObject.SupplyParcel();
                                break;
                            case 4:
                                DalObject.DalObject.SendDroneToCharge();
                                break;
                            case 5:
                                DalObject.DalObject.ReleaseDroneFromCharger();
                                break;
                        }
                        break;
                    case CHOICE.DISPLAY:
                        Console.WriteLine("What would you like to do?\n" +
                           "1- Base Stations display\n" +
                           "2- Drones display \n" +
                           "3- Customers display \n" +
                           "4- parcels display \n");
                        int.TryParse(Console.ReadLine(), out info);
                        int id;
                        switch (info)
                        {
                            case 1:
                                Console.WriteLine(" Enter the station ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                PrintAll(DalObject.DalObject.BaseStationDisplay(id));
                                break;
                            case 2:
                                Console.WriteLine(" Enter the Drone ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                PrintAll(DalObject.DalObject.DroneDisplay(id));
                                break;
                            case 3:
                                Console.WriteLine(" Enter the Customer ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                PrintAll(DalObject.DalObject.CustomerDisplay(id));
                                break;
                            case 4:
                                Console.WriteLine("Enter a parcel ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                PrintAll(DalObject.DalObject.ParcelDisplay(id));
                                break;

                        }
                        break;
                    case CHOICE.VIEW_LIST:
                        Console.WriteLine("Which List would you like to view? \n" +
                            "s - Sations\n" + 
                            "d - Drones\n" + 
                            "c - Customers\n" + 
                            "p - Parcels\n" + 
                            "f - Parcels that have not yet been affiliated with a drone\n" + 
                            "o - Stations with open charge slots \n");
                        char pick;                       
                        char.TryParse(Console.ReadLine(), out pick);
                        switch (pick)
                        {
                           
                            case 's':
                                List<Station> PrintStaion = new List<Station>();
                                PrintStaion= DalObject.DalObject.PrintBaseStation();
                                PrintStaion.ForEach(PrintAll<Station>);
                                break;
                            case 'd':
                                List<Drone> PrintDrone = new List<Drone>();
                                PrintDrone = DalObject.DalObject.PrintDrone();
                                PrintDrone.ForEach(PrintAll<Drone>);
                                break;
                            case 'c':
                                List<Customer> PrintCustomer = new List<Customer>();
                                PrintCustomer = DalObject.DalObject.PrintCustomer();
                                PrintCustomer.ForEach(PrintAll<Customer>);
                                break;
                            case 'p':
                                List<Parcel> PrintParcel = new List<Parcel>();
                                PrintParcel = DalObject.DalObject.PrintParcel();
                                PrintParcel.ForEach(PrintAll<Parcel>);
                                break;
                            case 'f':
                                List<Parcel> PrintParcelOnAir = new List<Parcel>();
                                PrintParcelOnAir = DalObject.DalObject.PrintParcelOnAir();
                                PrintParcelOnAir.ForEach(PrintAll<Parcel>);
                                break;
                            case 'o':
                                List<Station> PrintStationsWithOpenSlots = new List<Station>();
                                PrintStationsWithOpenSlots = DalObject.DalObject.PrintStationsWithOpenSlots();
                                PrintStationsWithOpenSlots.ForEach(PrintAll<Station>);
                                break;
                        }
                        break;
                    case CHOICE.EXIT:
                        break;
                    default:
                        break;
                }
                Console.WriteLine("\nMenu:\n" +
                      "ADD- Add a new base Station/Drone/Customer/Parcel.\n" +
                      "UPDATE- Update assignment/Collection /Delivery /Charging /Release.\n" +
                      "DISPLAY- Display of base stations/Drone/Customer/ Parcel\n" +
                      "VIEW_LIST- Print all bbase stations/Drone/Customer/Parcel/Packages not yet associated/Base stations with available charging stations.\n" +
                      "EXIT- Exit\n");
                Enum.TryParse(Console.ReadLine(), out choice);
            }
        }
        public static void PrintAll<T>(T t)
        {
            Console.WriteLine(t);
        }
        public static void AddStation() /// Adding a Station with all its fields
        {
            Station newStation = new Station();
            Console.WriteLine("Enter a unique ID number of staion\n");
            newStation.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the name of the station\n");
            newStation.Name = Console.ReadLine();
            Console.WriteLine("Enter the longitude of the station\n");
            newStation.Longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Lattitude of the station\n");
            newStation.Lattitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of charging points available at the station\n");
            newStation.AvailableChargeSlots = int.Parse(Console.ReadLine());
            DalObject.DalObject.AddStation(newStation);
        }
        public static void AddDrone()  /// Adding a Drone with all its fields
        {
            Drone newDrone = new Drone();
            Console.WriteLine("Enter a unique ID number\n");
            newDrone.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Insert the model of the drone\n");
            newDrone.Model = Console.ReadLine();
            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy\n");
            newDrone.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the condition of the drone 1- Available ,2- Delivery ,3-Charging\n");
            newDrone.Status = (DroneStatuses)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the battery status (in number)\n");
            newDrone.Battery = int.Parse(Console.ReadLine());
            DalObject.DalObject.AddDrone(newDrone);
        }
        public static void AddCustomer()  /// Adding a Customer with all its fields
        {
            Customer newCustomer = new Customer();
            Console.WriteLine("Enter a unique ID number\n");
            newCustomer.Id = int.Parse(Console.ReadLine());
            Console.WriteLine(" Enter the customer name\n");
            newCustomer.Name = Console.ReadLine();
            Console.WriteLine(" Enter a phone number\n");
            newCustomer.Phone = Console.ReadLine();
            Console.WriteLine("enter longitude\n");
            newCustomer.Longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("enter Lattitude\n");
            newCustomer.Lattitude = double.Parse(Console.ReadLine());
            DalObject.DalObject.AddCustomer(newCustomer);
        }
        public static void AddParcel()  /// Adding a Parcel with all its fields
        {
            Parcel newParcel = new Parcel();
            newParcel.Id = DalObject.DalObject.GetParcelId();
            Console.WriteLine("Enter a sending customer ID number\n");
            newParcel.SenderId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter a receives Customer  ID number\n");
            newParcel.TargetId = int.Parse(Console.ReadLine());
            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy\n");
            newParcel.Weight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("enter  1-Regular , 2-Express , 3-Urgent\n");
            newParcel.Priority = (Priorities)int.Parse(Console.ReadLine());
            newParcel.Creating = DateTime.Now;
            newParcel.DroneId = -1;
            DalObject.DalObject.AddParcel(newParcel);
        }
    }   
}






















