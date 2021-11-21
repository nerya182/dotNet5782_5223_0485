using IBL;
using IBL.BO;
using System;
using System.Collections.Generic;
using System.Threading.Channels;

namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {
            IBL.IBL bl =new BL.BL();
            int input = new int();
            CHOICE choice;
            do
            {
                Console.WriteLine("\nMenu:\n" +
                       "1-ADD- Add a new base Station/Drone/Customer/Parcel.\n" +
                       "2-UPDATE- Update assignment/Collection /Delivery /Charging /Release.\n" +
                       "3-DISPLAY- Display of base stations/Drone/Customer/ Parcel\n" +
                       "4-VIEW_LIST- Print all base stations/Drone/Customer/Parcel/\n" +
                       "          Packages not yet associated/Base stations with available charging stations.\n" +
                       "EXIT- Exit\n");

                while (!Enum.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Wrong Input!");
                }

                switch (choice)
                {
                    case CHOICE.ADD:
                        AddOptions(bl, input);
                        break;
                    case CHOICE.UPDATE:
                        UpdateOptions(bl, input);                      
                        break;
                    case CHOICE.DISPLAY:
                        DisplayOptions(bl, input);                      
                        break;
                    case CHOICE.VIEW_LIST:
                        ViewListOptions(bl);
                        break;
                    case CHOICE.EXIT:
                        break;
                    default:
                        break;
                }
            }
            while (choice != CHOICE.EXIT);
        }

        private static void AddOptions(IBL.IBL bl, int input)
        {
            Console.WriteLine("What would you like to add? \n" +
                              "1-Add a new base Station.\n" +
                              "2-Add a new Drone.\n" +
                              "3-Add a new Customer.\n" +
                              "4-Add a new Parcel.\n");
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
        }

        private static void UpdateOptions(IBL.IBL bl, int input)
        {
            Console.WriteLine("What would you like to bl?\n" +
                            "1- Update drone data\n" +
                            "2- Update station data\n" +
                            "3- Update customer data\n" +
                            "4- Sending a drone for charging\n" +
                            "5- Release drone from charging\n" +
                            "6- Affiliate Parcel to Drone\n" +
                            "7- Parcel collection by drone\n" +
                            "8- Delivery of a parcel by drone\n");
            int.TryParse(Console.ReadLine(), out input);
            switch (input)
            {
                case 1:
                    UpdateDrone(bl);
                    break;
                case 2:
                    UpdateStation(bl);
                    break;
                case 3:
                    UpdateCustomer(bl);
                    break;
                case 4:
                    SendingDroneForCharging(bl);
                    break;
                case 5:
                    ReleaseDroneFromCharging(bl);
                    break;
                case 6:
                    AffiliateParcelToDrone(bl);
                    break;
                case 7:
                    ParcelCollectionByDrone(bl);
                    break;
                case 8:
                    DeliveryOfParcelByDrone(bl);
                    break;
            }
        }

        public static void DisplayOptions(IBL.IBL bl, int input)
        {
            Console.WriteLine("What would you like to bl?\n" +
                           "1- Base Stations display\n" +
                           "2- Drones display \n" +
                           "3- Customers display \n" +
                           "4- parcels display \n");
            int id = new int();
            int.TryParse(Console.ReadLine(), out input);
            switch (input)
            {
                case 1:
                    DisplayStation(bl, id);
                    break;
                case 2:
                    DisplayDrone(bl, id);
                    break;
                case 3:
                    DisplayCustomer(bl, id);   
                    break;
                case 4:
                    DisplayParcel(bl, id);              
                    break;
            }
        }

        private static void ViewListOptions(IBL.IBL bl)
        {
            Console.WriteLine("Which List would you like to view? \n" +
                            "1 - Stations\n" +
                            "2 - Drones\n" +
                            "3 - Customers\n" +
                            "4 - Parcels\n" +
                            "5 - Parcels that have not yet been affiliated with a drone\n" +
                            "6 - Stations with open charge slots \n");
            char pick;
            char.TryParse(Console.ReadLine(), out pick);
            switch (pick)
            {

                case '1':
                    ViewStationList(bl);  
                    break;
                case '2':
                    ViewDroneList(bl);
                    break;
                case '3':
                    ViewCustomerList(bl); 
                    break;
                case '4':
                    ViewParcelList(bl);        
                    break;
                case '5':
                    ViewParcelOnAirList(bl);               
                    break;
                case '6':
                    ViewStationsWithOpenSlotsList(bl);    
                    break;
            }
        }
        private static void ViewStationList(IBL.IBL bl)
        {
            IEnumerable<Station> PrintStation = new List<Station>();
            PrintStation = bl.GetListStation();
            foreach (Station objStation in PrintStation)
            {
                StationToList stationToList = bl.MakeStationToList(objStation);
                Console.WriteLine(stationToList);
            }
        }

        private static void ViewDroneList(IBL.IBL bl)
        {
            IEnumerable<Drone> PrintDrone = new List<Drone>();
            PrintDrone = bl.GetListDrone();
            foreach (Drone objDrone in PrintDrone)
            {
                DroneToList droneToList = bl.MakeDroneToList(objDrone);
                Console.WriteLine(droneToList);
            }
        }

        private static void ViewCustomerList(IBL.IBL bl)
        {
            IEnumerable<Customer> PrintCustomer = new List<Customer>();
            PrintCustomer = bl.GetListCustomer();
            foreach (Customer objCustomer in PrintCustomer)
            {
                CustomerToList customerToList = bl.MakeCustomerToList(objCustomer);
                Console.WriteLine(customerToList);
            }
        }

        private static void ViewParcelList(IBL.IBL bl)
        {
            IEnumerable<Parcel> PrintParcel = new List<Parcel>();
            PrintParcel = bl.GetListParcel();
            foreach (Parcel objParcel in PrintParcel)
            {
                ParcelToList parcelToList = bl.MakeParcelToList(objParcel);
                Console.WriteLine(parcelToList);
            }
        }

        private static void ViewParcelOnAirList(IBL.IBL bl)
        {
            IEnumerable<Parcel> PrintParcelOnAir = new List<Parcel>();
            PrintParcelOnAir = bl.GetListParcelOnAir();
            foreach (Parcel objStation in PrintParcelOnAir)
            {
                Console.WriteLine(objStation);
            }
        }

        private static void ViewStationsWithOpenSlotsList(IBL.IBL bl)
        {
            IEnumerable<Station> PrintStationsWithOpenSlots = new List<Station>();
            PrintStationsWithOpenSlots = bl.GetListStationsWithOpenSlots();
            foreach (Station objStation in PrintStationsWithOpenSlots)
            {
                Console.WriteLine(objStation);
            }
        }

        private static void DisplayStation(IBL.IBL bl, int id)
        {
            Console.WriteLine("Enter the station ID number");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine(bl.BaseStationDisplay(id));
        }

        private static void DisplayDrone(IBL.IBL bl, int id)
        {
            Console.WriteLine("Enter the Drone ID number");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine(bl.DroneDisplay(id));
        }

        private static void DisplayCustomer(IBL.IBL bl, int id)
        {
            Console.WriteLine("Enter the Customer ID number");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine(bl.CustomerDisplay(id));
        }

        private static void DisplayParcel(IBL.IBL bl, int id)
        {
            Console.WriteLine("Enter a parcel ID number");
            int.TryParse(Console.ReadLine(), out id);
            Console.WriteLine(bl.ParcelDisplay(id));
        }

        private static void DeliveryOfParcelByDrone(IBL.IBL bl)
        {
            try
            {
                int droneId;
                Console.WriteLine("Enter a unique ID number of drone");
                droneId = int.Parse(Console.ReadLine());
                bl.DeliveryOfParcelByDrone(droneId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("The update was successful");
        }

        private static void ParcelCollectionByDrone(IBL.IBL bl)
        {
            try
            {
                int droneId;
                Console.WriteLine("Enter a unique ID number of drone");
                droneId = int.Parse(Console.ReadLine());
                bl.ParcelCollectionByDrone(droneId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("The update was successful");
        }

        private static void AffiliateParcelToDrone(IBL.IBL bl)
        {
            try
            {
                int droneId;
                Console.WriteLine("Enter a unique ID number of drone");
                droneId = int.Parse(Console.ReadLine());
                bl.AffiliateParcelToDrone(droneId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("The update was successful");
        }

        private static void ReleaseDroneFromCharging(IBL.IBL bl)
        {
            try
            {
                int droneId;
                Console.WriteLine("Enter a unique ID number of drone");
                droneId = int.Parse(Console.ReadLine());
                bl.ReleaseDroneFromCharging(droneId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("The update was successful");
        }

        private static void SendingDroneForCharging(IBL.IBL bl)
        {
            try
            {
                int droneId;
                Console.WriteLine("Enter a unique ID number of drone");
                droneId = int.Parse(Console.ReadLine());
                bl.SendingDroneForCharging(droneId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("The update was successful");
        }

        private static void UpdateCustomer(IBL.IBL bl)
        {
            try
            {
                Customer updateCustomer = new Customer();
                Console.WriteLine("Enter a unique ID number of Customer");
                updateCustomer.Id = int.Parse(Console.ReadLine());
                Console.WriteLine("Insert the new name of the customer or click enter");
                updateCustomer.Name = Console.ReadLine();
                Console.WriteLine("Insert the new phone of the customer or click enter");
                updateCustomer.Phone = Console.ReadLine();
                bl.UpdateCustomer(updateCustomer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("The update was successful");
        }
        private static void UpdateStation(IBL.IBL bl)
        {
            try
            {
                Station updateStation = new Station();
                Console.WriteLine("Enter a unique ID number of station");
                updateStation.Id = int.Parse(Console.ReadLine());
                Console.WriteLine("Insert the new name of the station or click enter");
                updateStation.Name = Console.ReadLine();
                Console.WriteLine("Insert the total amount of charging of the station or click ");
                int.TryParse(Console.ReadLine(), out int chargingPositions);
                bl.UpdateStation(updateStation, chargingPositions);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("The update was successful");
        }
        private static void UpdateDrone(IBL.IBL bl)
        {
            try
            {
                DroneToList updateDrone = new DroneToList();
                Console.WriteLine("Enter a unique ID number of drone");
                updateDrone.Id= int.Parse(Console.ReadLine());
                Console.WriteLine("Insert the  new model of the drone");
                updateDrone.Model = Console.ReadLine();
                bl.UpdateDrone(updateDrone);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("The update was successful");
        }

        private static void AddCustomer(IBL.IBL bl)
        {
            try
            {
                Customer newCustomer = new Customer();
                Location locationOfnewCustomer = new Location();
                Console.WriteLine("Enter a unique ID number");
                newCustomer.Id = int.Parse(Console.ReadLine());
                Console.WriteLine(" Enter the customer name");
                newCustomer.Name = Console.ReadLine();
                Console.WriteLine(" Enter a phone number");
                newCustomer.Phone = Console.ReadLine();
                Console.WriteLine("What is your Latitude?");
                locationOfnewCustomer.Lattitude=double.Parse(Console.ReadLine());
                Console.WriteLine("What is your longitude?");
                locationOfnewCustomer.Longitude = double.Parse(Console.ReadLine());
                newCustomer.Location = locationOfnewCustomer;
                bl.AddCustomer(newCustomer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Added successfully");
        }

        private static void AddParcel(IBL.IBL bl)
        {
            try
            {
                Parcel newParcel = new Parcel();
                CustomerInParcel sender = new CustomerInParcel();
                CustomerInParcel target = new CustomerInParcel();
                DroneInParcel droneInParcel = new DroneInParcel();
                Console.WriteLine("Enter a sending customer ID number");
                sender.Id = int.Parse(Console.ReadLine());
                newParcel.Sender = sender;
                Console.WriteLine("Enter a receives Customer  ID number");
                target.Id = int.Parse(Console.ReadLine());
                newParcel.Target = target;
                Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy");
                newParcel.Weight = (WeightCategories) int.Parse(Console.ReadLine());
                Console.WriteLine("enter  1-Regular , 2-Express , 3-Urgent");
                newParcel.Priority = (Priorities) int.Parse(Console.ReadLine());
                newParcel.Creating = DateTime.Now;
                newParcel.Affiliation = DateTime.MinValue;
                newParcel.Delivered = DateTime.MinValue;
                newParcel.PickedUp = DateTime.MinValue;
                droneInParcel.DroneId = 0;
                newParcel.drone = droneInParcel;
                bl.AddParcel(newParcel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Added successfully");
        }

        private static void AddDrone(IBL.IBL bl)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Added successfully");
        }

        public static void AddStation(IBL.IBL bl)
        {
            try
            {
                Station newStation = new Station();
                Location locationOfNewStation = new Location();
                Console.WriteLine("Enter a unique ID number of station");
                newStation.Id = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the name of the station");
                newStation.Name = Console.ReadLine();
                Console.WriteLine("Enter the longitude of the station");
                locationOfNewStation.Longitude= double.Parse(Console.ReadLine());
                Console.WriteLine("Enter the latitude of the station");
                locationOfNewStation.Lattitude = double.Parse(Console.ReadLine());
                newStation.location = locationOfNewStation;
                Console.WriteLine("Enter the number of charging points available at the station");
                newStation.AvailableChargeSlots = int.Parse(Console.ReadLine());
                newStation.droneInCharging = new List<DroneInCharging>(0);
                bl.AddStation(newStation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Added successfully");
        }

        public static void PrintAll<T>(T t)
        {
            Console.WriteLine(t);
        }
    }  
}
