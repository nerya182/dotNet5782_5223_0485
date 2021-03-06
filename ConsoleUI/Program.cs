/* Binyamin Mor- 317510485,  Nerya Baracassa- 208915223
 The program created a whole big Drone operating system. we've entered some info ourselves, but we are mainly setting the grounds for a much bigger project! */

using System;
using System.Collections.Generic;
using DalObject;
using DO;



namespace ConsoleUI
{
    class Program
    {
        private static DalApi.IDal DO;
        static void Main(string[] args)
        {
            DO = DalApi.DalFactory.GetDal("List");
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
                                AddStation(DO);
                                break;
                            case 2:
                                AddDrone(DO);
                                break;
                            case 3:
                                AddCustomer(DO);
                                break;
                            case 4:
                                AddParcel(DO);
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
                                AffiliateOfDrone(DO);
                                break;
                            case 2:
                                PickupParcel(DO);
                                break;
                            case 3:
                                SupplyParcel(DO);
                                break;
                            case 4:
                                SendDroneToCharge(DO);
                                break;
                            case 5:
                                ReleaseDrone(DO);
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
                                int.TryParse(Console.ReadLine( ), out id);
                                Console.WriteLine(DO.BaseStationDisplay(id)); 
                                break;
                            case 2:
                                Console.WriteLine(" Enter the Drone ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                Console.WriteLine(DO.DroneDisplay(id));
                                break;
                            case 3:
                                Console.WriteLine(" Enter the Customer ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                Console.WriteLine(DO.CustomerDisplay(id));   
                                break;
                            case 4:
                                Console.WriteLine("Enter a parcel ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                Console.WriteLine(DO.ParcelDisplay(id));   
                                break;
                        }
                        break;
                    case CHOICE.VIEW_LIST:
                        Console.WriteLine("Which List would you like to view? \n" +
                            "s - Stations\n" +
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
                                foreach (Station objStation in DO.ListBaseStation(i => true))
                                {
                                    Console.WriteLine(objStation);
                                }
                                break;
                            case 'd':
                                foreach (Drone objDrone in DO.ListDrone(i=>true))
                                {
                                    Console.WriteLine(objDrone);
                                }
                                break;
                            case 'c':
                                foreach (Customer objCustomer in DO.ListCustomer(i=>true))
                                {
                                    Console.WriteLine(objCustomer);
                                }
                                break;
                            case 'p':
                                foreach (Parcel objParcel in DO.ListParcel(i => true))
                                {
                                    Console.WriteLine(objParcel);
                                }
                                break;
                            case 'f':
                                foreach (Parcel objParcel in DO.ListParcelOnAir())
                                {
                                    Console.WriteLine(objParcel);
                                }
                                break;
                            case 'o':
                                foreach (Station objStation in DO.ListStationsWithOpenSlots())
                                {
                                    Console.WriteLine(objStation);
                                }
                                break;
                        }
                        break;
                    case CHOICE.DISTANCE:
                        distance(DO);
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
        public static void distance(DalApi.IDal DO)
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
           // distance =DO.distanceCalculation(latitude, longitude,id,temp);
            //Console.WriteLine( $"The distance is: {distance} km");
        }
        /// <summary>
        /// Delivering Parcel
        /// </summary>
        public static void SupplyParcel(DalApi.IDal DO)
        {
            Console.WriteLine("What is the Parcel Id?");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            DO.SupplyParcelUpdate(id);
        }
        /// <summary>
        /// Parcel picked up
        /// </summary>
        public static void PickupParcel(DalApi.IDal DO)
        {
            int id;
            Console.WriteLine(" What is the Parcel Id? \n");
            int.TryParse(Console.ReadLine(), out id);
            DO.PickupParcelUpdate(id);
        }
        /// <summary>
        /// Affiliating a drone to a parcel
        /// </summary>
        public static void AffiliateOfDrone(DalApi.IDal DO)
        {
            int idParcel, idDrone;
            Console.WriteLine(" What is the Parcel Id? \n");
            int.TryParse(Console.ReadLine(), out idParcel);
            Console.WriteLine(" What is the drone Id? \n");
            int.TryParse(Console.ReadLine(), out idDrone);
            DO.Affiliate(idParcel, idDrone);
        }
        /// <summary>
        /// Releasing Drone from charging
        /// </summary>
        public static void ReleaseDrone(DalApi.IDal DO)
        {
            Console.WriteLine("What is the Drone Id? \n");
            int droneId;
            int.TryParse(Console.ReadLine(), out droneId);
            DO.ReleaseDroneFromCharger(droneId);
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
        private static void AddStation(DalApi.IDal DO) 
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
            DO.AddStation(newStation);
        }
        /// <summary>
        /// Adding a Drone with all its fields
        /// </summary>
        public static void AddDrone(DalApi.IDal DO)  
        {
            Drone newDrone = new Drone();
            Console.WriteLine("Enter a unique ID number");
            newDrone.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Insert the model of the drone");
            newDrone.Model = Console.ReadLine();
            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy");
            newDrone.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
            DO.AddDrone(newDrone);
        }
        /// <summary>
        /// Adding a Customer with all its fields
        /// </summary>
        public static void AddCustomer(DalApi.IDal DO)  
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
            DO.AddCustomer(newCustomer);
        }
        /// <summary>
        /// Adding a Parcel with all its fields
        /// </summary>
        public static void AddParcel(DalApi.IDal DO)  
        {
            Parcel newParcel = new Parcel();
            newParcel.Id = DO.GetParcelId();
            Console.WriteLine("Enter a sending customer ID number");
            newParcel.SenderId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter a receives Customer  ID number");
            newParcel.TargetId = int.Parse(Console.ReadLine());
            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy");
            newParcel.Weight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("enter  1-Regular , 2-Express , 3-Urgent");
            newParcel.Priority = (Priorities)int.Parse(Console.ReadLine());
            newParcel.Creating = DateTime.Now;
            newParcel.Affiliation = default(DateTime);
            newParcel.Delivered = default(DateTime);
            newParcel.PickedUp= default(DateTime);
            newParcel.DroneId = -1;
            DO.AddParcel(newParcel);
        }

        /// <summary>
        /// Charging the drone, Adding a DroneCharger
        /// </summary>
        public static void SendDroneToCharge(DalApi.IDal DO)
        {
            Console.WriteLine("What is the Drone Id?");
            int DroneId, StationId;
            int.TryParse(Console.ReadLine(), out DroneId);
            
            IEnumerable<Station> StationsWithOpenSlots = new List<Station>();
            StationsWithOpenSlots = DO.ListStationsWithOpenSlots();
            foreach (Station objStation in StationsWithOpenSlots)
            {
                PrintAll(objStation);
            }
            Console.WriteLine("Which Station would you like to charge your Drone?");
            int.TryParse(Console.ReadLine(), out StationId);
            DroneCharge droneCharge = new DroneCharge();  
            droneCharge.StationId = StationId;
            droneCharge.DroneId = DroneId;
            droneCharge.EntryTime=DateTime.Now;
            DO.AddDroneToCharge(droneCharge);
        }
    }   
}






















