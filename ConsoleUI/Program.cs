﻿using System;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            CHOICE choice;
          
            Console.WriteLine("Menu:\n" +
                   "ADD- Add a new base Station/Drone/Customer/Parcel.\n" +
                   "UPDATE- Update assignment/Collection /Delivery /Charging /Release.\n" +
                   "DISPLAY- Display of base stations/Drone/Customer/ Parcel\n" +
                   "VIEW_LIST- Print all bbase stations/Drone/Customer/Parcel/Packages not yet associated/Base stations with available charging stations.\n" +
                   "EXIT- Exit");
           Enum.TryParse(Console.ReadLine(),out choice);
            switch (choice)
            {
                case CHOICE.ADD:
                    Console.WriteLine("What would you like to add? \n" +
                                     "a- Add a new base Station.\n" +
                                     "b- Add a new Drone.\n" +
                                     "c-Add a new Customer.\n" +
                                     "d-Add a new Parcel.\n");
                    char input;
                    int Id, SenderId, TargetId, Battery, AvailableChargeSlots;
                    string name, phone,model;
                    double Lattitude, Longitude;
                    WeightCategories myMaxWeight;
                    DroneStatuses myStatuses;
                    Priorities myPriority;
                    char.TryParse(Console.ReadLine(), out input);
                    switch (input)
                    {
                        case 'a':
                            Console.WriteLine("Enter a unique ID number of staion\n");
                            Id = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter the name of the station\n");
                            name =Console.ReadLine();
                            Console.WriteLine("Enter the longitude of the station\n");
                            Longitude = double.Parse(Console.ReadLine());
                            Console.WriteLine("Enter the Lattitude of the station\n");
                            Lattitude = double.Parse(Console.ReadLine());
                            Console.WriteLine("Enter the number of charging points available at the station\n");
                            AvailableChargeSlots = int.Parse(Console.ReadLine());
                            DalObject.DalObject.AddStation(Id,name,Longitude,Lattitude,AvailableChargeSlots);
                            break;
                        case 'b':
                            Console.WriteLine("Enter a unique ID number\n");
                            Id = int.Parse(Console.ReadLine());
                            Console.WriteLine("Insert the model of the drone\n");
                            model = Console.ReadLine();
                            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy\n");
                            myMaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter the condition of the drone 1- Available ,2- Delivery ,3-Charging\n");
                            myStatuses = (DroneStatuses)int.Parse(Console.ReadLine();
                            Console.WriteLine("Enter the battery status (in number)\n");
                            Battery= int.Parse(Console.ReadLine());
                            DalObject.DalObject.addDrone(Id,model,myMaxWeight,myStatuses,Battery);
                            break;
                        case 'c':
                            Console.WriteLine("Enter a unique ID number\n");
                            Id = int.Parse(Console.ReadLine());
                            Console.WriteLine(" Enter the customer name\n");
                            name = Console.ReadLine();
                            Console.WriteLine(" Enter a phone number\n");
                            phone = Console.ReadLine();
                            Console.WriteLine("enter longitude\n");
                            Longitude = double.Parse(Console.ReadLine());
                            Console.WriteLine("enter Lattitude\n");
                            Lattitude = double.Parse(Console.ReadLine());
                            DalObject.DalObject.AddCustomer(Id,name,phone,Longitude,Lattitude);
                            break;
                        case 'd':
                            Id = DalObject.DalObject.GetParcelId();
                            Console.WriteLine("Enter a sending customer ID number\n");
                            SenderId = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter a receives Customer  ID number\n");
                            TargetId = int.Parse(Console.ReadLine());
                            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy\n");
                            myMaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                            Console.WriteLine("enter  1-Regular , 2-Express , 3-Urgent\n");
                            myPriority = (Priorities)int.Parse(Console.ReadLine());
                            DalObject.DalObject.addParcel(Id, SenderId, TargetId, myMaxWeight, myPriority,-1, DateTime.Now);
                            break;
                    }
                    break;

                case CHOICE.UPDATE:
                    Console.WriteLine(" What would you like to do?" +
                        "a-Affiliate Parcel to Drone?\n" +
                        "b- Pickup Parcel with Drone? \n" +
                        "c- Deliver Parcel to Customer? \n" +
                        "d- Send Drone to Charge? \n" +
                        "e- Release Drone from Charge? \n");
                    int info;
                    int.TryParse(Console.ReadLine(), out info);
                    switch (info)
                    {
                        case 'a':
                            DalObject.DalObject.Affiliate();
                            break;
                        case 'b':
                            DalObject.DalObject.PickupParcel();
                            break;
                        case 'c':
                            DalObject.DalObject.SupplyParcel();
                            break;
                        case 'd':
                            DalObject.DalObject.SendDroneToCharge();
                            break;
                        case 'e':

                            break;


                    }

                    break;
                case CHOICE.DISPLAY:
                    break;
                case CHOICE.VIEW_LISTS:
                    Console.WriteLine("Which List would you like to view? \n" +
                        "s - Sations" + "d - Drones" + "c - Customers" + "p - Parcels" + "f - Parcels that have not yet been affiliated with a drone" + " o - Stations with open charge slots \n");
                    char pick;
                    char.TryParse(Console.ReadLine(), out pick);
                    switch (pick)
                    {
                        case 's':
                            for (int i = 0; i < DalObject.DalObject.GetStationId(); i++ )
                                Console.WriteLine(DalObject.DalObject.GetStation(i).ToString());
                            break;
                        case 'd':
                            for (int i = 0; i < DalObject.DalObject.GetDroneId(); i++)
                                Console.WriteLine(DalObject.DalObject.GetDrone(i).ToString());
                            break;
                        case 'c':
                            for (int i = 0; i < DalObject.DalObject.GetCustomerId(); i++)
                                Console.WriteLine(DalObject.DalObject.GetCustomer(i).ToString());
                            break;
                        case 'p':
                            for (int i = 0; i < DalObject.DalObject.GetParcelId(); i++)
                                Console.WriteLine(DalObject.DalObject.GetParcel(i).ToString());
                            break;
                        case 'f':
                            for(int i =0; i < DalObject.DalObject.GetParcelId(); i++)
                            {
                                if(DalObject.DalObject.GetParcel(i).DroneId == -1)
                                    Console.WriteLine(DalObject.DalObject.GetParcel(i).ToString());
                            }
                            break;
                        case 'o':
                            DalObject.DalObject.PrintStationsWithOpenSlots();
                                break;
                    }

                    break;
                case CHOICE.EXIT:
                    break;
                default:
                    break;
            }





        }

    }
}






















