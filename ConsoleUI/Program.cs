using System;
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
                                addDrone();
                                break;
                            case 'c':
                                AddCustomer();
                                break;
                            case 'd':
                                addParcel();
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
                        Console.WriteLine(" What would you like to do?" +
                           "1- Base Station display\n" +
                           "2- Drone display \n" +
                           "3- Customer display \n" +
                           "4- Send Drone display \n" 
                           );
                        int.TryParse(Console.ReadLine(), out info);
                        int id;
                        switch (info)
                        {
                            case 1:
                                Console.WriteLine(" Enter the station ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                DalObject.DalObject.BaseStationDisplay(id);
                                break;
                            case 2:
                                Console.WriteLine(" Enter the Drone ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                DalObject.DalObject.DroneDisplay(id);
                                break;
                            case 3:
                                Console.WriteLine(" Enter the Customer ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                DalObject.DalObject.CustomerDisplay(id);
                                break;
                            case 4:
                                Console.WriteLine("Enter a parcel ID number");
                                int.TryParse(Console.ReadLine(), out id);
                                DalObject.DalObject.ParcelDisplay(id);
                                break;

                        }
                        break;
                    case CHOICE.VIEW_LISTS:
                        Console.WriteLine("Which List would you like to view? \n" +
                            "s - Sations" + "d - Drones" + "c - Customers" + "p - Parcels" + "f - Parcels that have not yet been affiliated with a drone" + " o - Stations with open charge slots \n");
                        char pick;
                        string st;
                        char.TryParse(Console.ReadLine(), out pick);
                        switch (pick)
                        {
                           
                            case 's':
                                for (int i = 0; i < DalObject.DalObject.GetStationId(); i++)
                                {
                                    st = DalObject.DalObject.GetStation(i).ToString();
                                    Console.WriteLine(st);
                                }
                                    
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
                                for (int i = 0; i < DalObject.DalObject.GetParcelId(); i++)
                                {
                                    if (DalObject.DalObject.GetParcel(i).DroneId == -1)
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
                Console.WriteLine("\nMenu:\n" +
                      "ADD- Add a new base Station/Drone/Customer/Parcel.\n" +
                      "UPDATE- Update assignment/Collection /Delivery /Charging /Release.\n" +
                      "DISPLAY- Display of base stations/Drone/Customer/ Parcel\n" +
                      "VIEW_LIST- Print all bbase stations/Drone/Customer/Parcel/Packages not yet associated/Base stations with available charging stations.\n" +
                      "EXIT- Exit\n");
                Enum.TryParse(Console.ReadLine(), out choice);
            }

        }

        public static void AddStation()
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
        public static void addDrone()
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
            DalObject.DalObject.addDrone(newDrone);
        }
        public static void AddCustomer()
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
        public static void addParcel()
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
            DalObject.DalObject.addParcel(newParcel);
        }
    }
       
        
    
   
}






















