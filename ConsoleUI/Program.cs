/* Binyamin Mor- 317510485,  Nerya Baracassa- 208915223
 The program created a whole big Drone operating system. we've entered some info ourselves, but we are mainly setting the grounds for a much bigger project! */

using System;
using System.Collections.Generic;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DalObject.DalObject DO = new DalObject.DalObject();
            
            CHOICE choice;
            do {
                Console.WriteLine("\nMenu:\n" +
                       "ADD- Add a new base Station/Drone/Customer/Parcel.\n" +
                       "UPDATE- Update assignment/Collection /Delivery /Charging /Release.\n" +
                       "DISPLAY- Display of base stations/Drone/Customer/ Parcel\n" +
                       "VIEW_LIST- Print all base stations/Drone/Customer/Parcel/\n" +
                       "          Packages not yet associated/Base stations with available charging stations.\n" +
                       "DISTANCE- Prints distance between point and Station or from customer\n"+
                       "EXIT- Exit\n");

                while(!Enum.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Wrong Input!");
                }

                switch (choice)
                {
                    case CHOICE.ADD:
                        Console.WriteLine("What would you like to add? \n" +
                                         "1- Add a new base Station.\n" +
                                         "2- Add a new Drone.\n" +
                                         "3-Add a new Customer.\n" +
                                         "4-Add a new Parcel.\n");
                        int input;
                        int.TryParse(Console.ReadLine(), out input);
                        switch (input)
                        {
                            case 1:
                                AddStation();
                                break;
                            case 2:
                                AddDrone();
                                break;
                            case 3:
                                AddCustomer();
                                break;
                            case 4:
                                AddParcel();
                                break;
                            default:
                                break;
                        }
                        break;
                    case CHOICE.UPDATE:
                        Console.WriteLine("What would you like to do?\n" +
                            "1- Affiliate Parcel to Drone\n" +
                            "2- Pickup Parcel with Drone\n" +
                            "3- Deliver Parcel to Customer\n" +
                            "4- Send Drone to Charge\n" +
                            "5- Release Drone from Charge");
                        int.TryParse(Console.ReadLine(), out input);
                        switch (input)
                        {
                            case 1:
                                AffiliateOfDrone();
                                break;
                            case 2:
                                PickupParcel();
                                break;
                            case 3:
                                SupplyParcel();
                                break;
                            case 4:
                                SendDroneToCharge();
                                break;
                            case 5:
                                ReleaseDrone();
                                break;
                        }
                        break;
                    case CHOICE.DISPLAY:
                        Console.WriteLine("What would you like to do?\n" +
                           "1- Base Stations display\n" +
                           "2- Drones display \n" +
                           "3- Customers display \n" +
                           "4- parcels display \n");
                        int info, id;
                        int.TryParse(Console.ReadLine(), out info);
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
                                List<Station> PrintStation = new List<Station>();   
                                PrintStation = DalObject.DalObject.PrintBaseStation();  
                                PrintStation.ForEach(PrintAll<Station>);     
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
                    case CHOICE.DISTANCE:
                        distance();
                        break;
                    case CHOICE.EXIT:
                        break;
                    default:
                        break;
                }
            }
            while(choice != CHOICE.EXIT);          
        }
        /// <summary>
        /// distance calculation
        /// </summary>
        public static void distance()
        {
            double latitude, longitude,distance;
            char temp;
            int id;
            Console.WriteLine("What is your Latitude?");
            double.TryParse(Console.ReadLine(), out latitude);
            Console.WriteLine("What is your longitude?");
            double.TryParse(Console.ReadLine(), out longitude);
            Console.WriteLine("Enter 's'-distance from station or 'c'-distance from customer");
            char.TryParse(Console.ReadLine(), out temp);
            Console.WriteLine("What is the station or customer of Id?");
            int.TryParse(Console.ReadLine(), out id);
            distance =DalObject.DalObject.distanceCalculation(latitude, longitude,id,temp);
            Console.WriteLine( $"The distance is: {distance} km");
        }
        /// <summary>
        /// Delivering Parcel
        /// </summary>
        public static void SupplyParcel()
        {
            Console.WriteLine("What is the Parcel Id?");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            DalObject.DalObject.SupplyParcelUpdate(id);
        }
        /// <summary>
        /// Parcel picked up
        /// </summary>
        public static void PickupParcel()
        {
            int id;
            Console.WriteLine(" What is the Parcel Id? \n");
            int.TryParse(Console.ReadLine(), out id);
            DalObject.DalObject.PickupParcelUpdate(id);
        }
        /// <summary>
        /// Affiliating a drone to a parcel
        /// </summary>
        public static void AffiliateOfDrone()
        {
            int idParcel, idDrone;
            Console.WriteLine(" What is the Parcel Id? \n");
            int.TryParse(Console.ReadLine(), out idParcel);
            Console.WriteLine(" What is the drone Id? \n");
            int.TryParse(Console.ReadLine(), out idDrone);
            DalObject.DalObject.Affiliate(idParcel, idDrone);
        }
        /// <summary>
        /// Releasing Drone from charging
        /// </summary>
        public static void ReleaseDrone()
        {
            Console.WriteLine("What is the Drone Id? \n");
            int droneId;
            int.TryParse(Console.ReadLine(), out droneId);
            DalObject.DalObject.ReleaseDroneFromCharger(droneId);
        }
        /// <summary>
        /// Receiving an object and the Console.Writeline func goes to the object's PrintToString func and prints the object to the console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void PrintAll<T>(T t)
        {
            Console.WriteLine(t);  
        }
        /// <summary>
        /// Adding a Station with all its fields
        /// </summary>
        public static void AddStation() 
        {
            Station newStation = new Station();
            Console.WriteLine("Enter a unique ID number of staion");
            newStation.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the name of the station");
            newStation.Name = Console.ReadLine();
            Console.WriteLine("Enter the longitude of the station");
            newStation.Longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Lattitude of the station");
            newStation.Lattitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of charging points available at the station");
            newStation.AvailableChargeSlots = int.Parse(Console.ReadLine());
            DalObject.DalObject.AddStation(newStation);
        }
        /// <summary>
        /// Adding a Drone with all its fields
        /// </summary>
        public static void AddDrone()  
        {
            Drone newDrone = new Drone();
            Console.WriteLine("Enter a unique ID number");
            newDrone.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Insert the model of the drone");
            newDrone.Model = Console.ReadLine();
            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy");
            newDrone.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the condition of the drone 1- Available ,2- Delivery ,3-Charging");
            newDrone.Status = (DroneStatuses)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the battery status (in number)");
            newDrone.Battery = int.Parse(Console.ReadLine());
            DalObject.DalObject.AddDrone(newDrone);
        }
        /// <summary>
        /// Adding a Customer with all its fields
        /// </summary>
        public static void AddCustomer()  
        {
            Customer newCustomer = new Customer();
            Console.WriteLine("Enter a unique ID number");
            newCustomer.Id = int.Parse(Console.ReadLine());
            Console.WriteLine(" Enter the customer name");
            newCustomer.Name = Console.ReadLine();
            Console.WriteLine(" Enter a phone number");
            newCustomer.Phone = Console.ReadLine();
            Console.WriteLine("enter longitude");
            newCustomer.Longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("enter Lattitude");
            newCustomer.Lattitude = double.Parse(Console.ReadLine());
            DalObject.DalObject.AddCustomer(newCustomer);
        }
        /// <summary>
        /// Adding a Parcel with all its fields
        /// </summary>
        public static void AddParcel()  
        {
            Parcel newParcel = new Parcel();
            newParcel.Id = DalObject.DalObject.GetParcelId();
            Console.WriteLine("Enter a sending customer ID number");
            newParcel.SenderId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter a receives Customer  ID number");
            newParcel.TargetId = int.Parse(Console.ReadLine());
            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy");
            newParcel.Weight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("enter  1-Regular , 2-Express , 3-Urgent");
            newParcel.Priority = (Priorities)int.Parse(Console.ReadLine());
            newParcel.Creating = DateTime.Now;
            newParcel.DroneId = -1;
            DalObject.DalObject.AddParcel(newParcel);
        }

        /// <summary>
        /// Charging the drone, Adding a DroneCharger
        /// </summary>
        public static void SendDroneToCharge()
        {
            Console.WriteLine("What is the Drone Id?");
            int DroneId, StationId;
            int.TryParse(Console.ReadLine(), out DroneId);
            DalObject.DalObject.FindDroneToCharge(DroneId);

            List<Station> StationsWithOpenSlots = new List<Station>();
            StationsWithOpenSlots = DalObject.DalObject.PrintStationsWithOpenSlots();
            StationsWithOpenSlots.ForEach(PrintAll<Station>);
            
            Console.WriteLine("Which Sattion would you like to charge your Drone?");
            int.TryParse(Console.ReadLine(), out StationId);
            DroneCharge droneCharge = new DroneCharge();  
            droneCharge.StationId = StationId;
            droneCharge.DroneId = DroneId;
            DalObject.DalObject.AddDroneToCharge(droneCharge, StationId);
        }
    }   
}






















