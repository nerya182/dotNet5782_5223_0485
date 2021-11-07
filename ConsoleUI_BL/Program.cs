using IBL;
using IBL.BO;
using System;
using System.Collections.Generic;

namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {

            BL.BL bl =new BL.BL();
            CHOICE choice;
            do
            {
                Console.WriteLine("\nMenu:\n" +
                       "ADD- Add a new base Station/Drone/Customer/Parcel.\n" +
                       "UPDATE- Update assignment/Collection /Delivery /Charging /Release.\n" +
                       "DISPLAY- Display of base stations/Drone/Customer/ Parcel\n" +
                       "VIEW_LIST- Print all base stations/Drone/Customer/Parcel/\n" +
                       "          Packages not yet associated/Base stations with available charging stations.\n" +
                       "DISTANCE- Prints distance between point and Station or from customer\n" +
                       "EXIT- Exit\n");

                while (!Enum.TryParse(Console.ReadLine(), out choice))
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
                                AddStation(bl);
                                break;
                            case 2:
                                AddDrone(bl);
                                break;
                            case 3:
                                AddCustomer(bl);
                                break;
                            case 4:
                                AddParcel(bl);
                                break;
                            default:
                                break;
                        }
                        break;
                    case CHOICE.UPDATE:
                        Console.WriteLine("What would you like to bl?\n" +
                            "1- Update drone data\n" +
                            "2- Update station data\n" +
                            "3- Update customer data\n" +
                            "4- Sending a drone for charging\n" +
                            "5- Release drone from charging"+
                            "6- Sending a drone for charging\n"+
                            "7- Affiliate Parcel to Drone\n"+
                            "8- Parcel collection by drone\n" +
                            "9- Delivery of a parcel by drone\n");
                        int.TryParse(Console.ReadLine(), out input);
                        switch (input)
                        {
                            case 1:
                                UpdateDron(bl);
                                break;
                            case 2:
                                PickupParcel(bl);
                                break;
                            case 3:
                                SupplyParcel(bl);
                                break;
                            case 4:
                                SendDroneToCharge(bl);
                                break;
                            case 5:
                                ReleaseDrone(bl);
                                break;
                            case 6:
                                ReleaseDrone(bl);
                                break;
                            case 7:
                                ReleaseDrone(bl);
                                break;
                            case 8:
                                ReleaseDrone(bl);
                                break;
                            case 9:
                                ReleaseDrone(bl);
                                break;
                        }
                        break;
                    case CHOICE.DISPLAY:
                        Console.WriteLine("What would you like to bl?\n" +
                           "1- Base Stations display\n" +
                           "2- Drones display \n" +
                           "3- Customers display \n" +
                           "4- parcels display \n");
                        int info, id;
                        int.TryParse(Console.ReadLine(), out info);
                        switch (info)
                        {
                            case 1:
                                try {
                                    Console.WriteLine(" Enter the station ID number");
                                    int.TryParse(Console.ReadLine(), out id);
                                    PrintAll(bl.BaseStationDisplay(id));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }

                                break;
                            case 2:
                                Console.WriteLine(" Enter the Drone ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                PrintAll(bl.DroneDisplay(id));
                                break;
                            case 3:
                                Console.WriteLine(" Enter the Customer ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                PrintAll(bl.CustomerDisplay(id));
                                break;
                            case 4:
                                Console.WriteLine("Enter a parcel ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                PrintAll(bl.ParcelDisplay(id));
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
                                IEnumerable<Station> PrintStation = new List<Station>();
                                PrintStation = bl.ListBaseStation();
                                foreach (Station objStation in PrintStation)
                                {
                                    PrintAll(objStation);
                                }
                                break;
                            case 'd':
                                IEnumerable<Drone> PrintDrone = new List<Drone>();
                                PrintDrone = bl.ListDrone();
                                foreach (Drone objStation in PrintDrone)
                                {
                                    PrintAll(objStation);
                                }
                                break;
                            case 'c':
                                IEnumerable<Customer> PrintCustomer = new List<Customer>();
                                PrintCustomer = bl.ListCustomer();
                                foreach (Customer objStation in PrintCustomer)
                                {
                                    PrintAll(objStation);
                                }
                                break;
                            case 'p':
                                IEnumerable<Parcel> PrintParcel = new List<Parcel>();
                                PrintParcel = bl.ListParcel();
                                foreach (Parcel objStation in PrintParcel)
                                {
                                    PrintAll(objStation);
                                }
                                break;
                            case 'f':
                                IEnumerable<Parcel> PrintParcelOnAir = new List<Parcel>();
                                PrintParcelOnAir = bl.ListParcelOnAir();
                                foreach (Parcel objStation in PrintParcelOnAir)
                                {
                                    PrintAll(objStation);
                                }
                                break;
                            case 'o':
                                IEnumerable<Station> PrintStationsWithOpenSlots = new List<Station>();
                                PrintStationsWithOpenSlots = bl.ListStationsWithOpenSlots();
                                foreach (Station objStation in PrintStationsWithOpenSlots)
                                {
                                    PrintAll(objStation);
                                }
                                break;
                        }
                        break;
                    case CHOICE.DISTANCE:
                        distance(bl);
                        break;
                    case CHOICE.EXIT:
                        break;
                    default:
                        break;
                }
            }
            while (choice != CHOICE.EXIT);
        }

        private static void UpdateDron(BL.BL bl)
        {
            try
            {
                DroneToList newDrone = new DroneToList();
                Console.WriteLine("Enter a unique ID number of drone");
                newDrone.Id = int.Parse(Console.ReadLine());
                Console.WriteLine("Insert the  new model of the drone");
                newDrone.Model = Console.ReadLine();
                bl.UpdateDron(newDrone);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            } 
        }

        private static void AddCustomer(BL.BL bl)
        {

            Customer newCustomer = new Customer();
            Console.WriteLine("Enter a unique ID number");
            newCustomer.Id = int.Parse(Console.ReadLine());
            Console.WriteLine(" Enter the customer name");
            newCustomer.Name = Console.ReadLine();
            Console.WriteLine(" Enter a phone number");
            newCustomer.Phone = Console.ReadLine();
            bl.AddCustomer(newCustomer);
        }

        private static void AddParcel(BL.BL bl)
        {
            Parcel newParcel = new Parcel();

            Console.WriteLine("Enter a sending customer ID number");
            newParcel.Sender.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter a receives Customer  ID number");
            newParcel.Target.Id = int.Parse(Console.ReadLine());

            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy");
            newParcel.Weight = (WeightCategories)int.Parse(Console.ReadLine());

            Console.WriteLine("enter  1-Regular , 2-Express , 3-Urgent");
            newParcel.Priority = (Priorities)int.Parse(Console.ReadLine());

            newParcel.Creating = DateTime.Now;
            newParcel.Affiliation = default(DateTime);
            newParcel.Delivered = default(DateTime);
            newParcel.PickedUp= default(DateTime);
            newParcel.drone.DroneId = -1;
            bl.AddParcel(newParcel);
        }
        

      
        private static void AddDrone(BL.BL bl)
        {
            int chargingStationId;
            DroneToList newDrone = new DroneToList();
            Console.WriteLine("Enter a unique ID number");
            newDrone.Id = int.Parse(Console.ReadLine());

            Console.WriteLine("Insert the model of the drone");
            newDrone.Model = Console.ReadLine();

            Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy");
            newDrone.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());

            Console.WriteLine("Enter a unique ID number station to put the drone initial charge ");
            chargingStationId = int.Parse(Console.ReadLine());
            bl.AddDrone(newDrone, chargingStationId);
        }

        private static void AddStation(BL.BL bl)
        {
            Station newStation = new Station();
            Console.WriteLine("Enter a unique ID number of station");
            newStation.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the name of the station"); 
            newStation.Name = Console.ReadLine();
            Console.WriteLine("Enter the longitude of the station");
            newStation.location.Longitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the lattitude of the station");
            newStation.location.Lattitude = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number of charging points available at the station");
            newStation.AvailableChargeSlots = int.Parse(Console.ReadLine());
            newStation.droneInCharging = new List<DroneInCharging>(0);
            bl.AddStation(newStation);
        }

        public static void PrintAll<T>(T t)
        {
            Console.WriteLine(t);
        }
    }
    
}
