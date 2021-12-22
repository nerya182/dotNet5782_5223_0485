/* Binyamin Mor- 317510485,  Nerya Baracassa- 208915223
 The program created a whole big Drone operating system. we've entered some info ourselves, but we are mainly setting the grounds for a much bigger project! */
using BO;
using System;
using System.Collections.Generic;
using BlApi;

namespace ConsoleUI_BL
{
    class Program
    {
        private static BlApi.IBL bl;
        static void Main(string[] args)
        {
            bl = BlFactory.GetBl();
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

        private static void AddOptions(BlApi.IBL bl, int input)
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

        private static void UpdateOptions(BlApi.IBL bl, int input)
        {
            Console.WriteLine("What would you like to do?\n" +
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

        public static void DisplayOptions(BlApi.IBL bl, int input)
        {
            Console.WriteLine("What would you like to do?\n" +
                           "1- Base Stations display\n" +
                           "2- Drones display \n" +
                           "3- Customers display \n" +
                           "4- parcels display \n");
            int id = new int();
            bool flag = new bool();
            int.TryParse(Console.ReadLine(), out input);
            switch (input)
            {
                case 1:
                    DisplayStation(bl, id, flag);
                    break;
                case 2:
                    DisplayDrone(bl, id, flag);
                    break;
                case 3:
                    DisplayCustomer(bl, id, flag);
                    break;
                case 4:
                    DisplayParcel(bl, id, flag);
                    break;
            }
        }

        private static void ViewListOptions(BlApi.IBL bl)
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
        /// <summary>
        /// View the list of stations of type stationtolist
        /// </summary>
        /// <param name="bl"></param>
        private static void ViewStationList(BlApi.IBL bl)
        {
            foreach (Station objStation in bl.GetListStation())
            {
                StationToList stationToList = bl.MakeStationToList(objStation);
                Console.WriteLine(stationToList);
            }
        }

        /// <summary>
        /// View the list of drones of type dronetolist
        /// </summary>
        /// <param name="bl"></param>
        private static void ViewDroneList(BlApi.IBL bl)
        {
            foreach (DroneToList obj in bl.GetListDrone())
            {
                Console.WriteLine(obj);
            }
        }

        /// <summary>
        /// view the list of customers of type customertolist
        /// </summary>
        /// <param name="bl"></param>
        private static void ViewCustomerList(BlApi.IBL bl)
        {
            foreach (CustomerToList objCustomer in bl.GetListCustomer())
                Console.WriteLine(objCustomer);
        }

        /// <summary>
        /// View the list of parcels of type parceltolist
        /// </summary>
        /// <param name="bl"></param>
        private static void ViewParcelList(BlApi.IBL bl)
        {
            foreach (Parcel objParcel in bl.GetListParcel())
            {
                ParcelToList parcelToList = bl.MakeParcelToList(objParcel);
                Console.WriteLine(parcelToList);
            }
        }

        /// <summary>
        /// view the list of parcels that have not been affiliated with a drone
        /// </summary>
        /// <param name="bl"></param>
        private static void ViewParcelOnAirList(BlApi.IBL bl)
        {
            foreach (Parcel objParcel in bl.GetListParcel())
            {
                if (objParcel.Affiliation==null)
                {
                    ParcelToList parcelToList = bl.MakeParcelToList(objParcel);
                    Console.WriteLine(parcelToList);
                }
            }
        }

        /// <summary>
        /// View the list of stations that have open charging slots
        /// </summary>
        /// <param name="bl"></param>
        private static void ViewStationsWithOpenSlotsList(BlApi.IBL bl)
        {
            foreach (Station objStation in bl.GetListStation())
            {
                if (objStation.AvailableChargeSlots>0)
                {
                    StationToList stationToList = bl.MakeStationToList(objStation);
                    Console.WriteLine(stationToList);
                }
            }
        }

        /// <summary>)
        /// Displaying a station according to its ID
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        private static void DisplayStation(BlApi.IBL bl, int id, bool flag)
        {
            do
            {
                Console.WriteLine("Enter the station ID number");
                flag = int.TryParse(Console.ReadLine(), out id);
            } while (!flag);
            Console.WriteLine(bl.BaseStationDisplay(id));
        }

        /// <summary>
        /// Displaying a drone according to its ID
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        private static void DisplayDrone(BlApi.IBL bl, int id, bool flag)
        {
            do
            {
                Console.WriteLine("Enter the Drone ID number");
                flag = int.TryParse(Console.ReadLine(), out id);
            } while (!flag);
            Console.WriteLine(bl.DroneDisplay(id));
        }

        /// <summary>
        /// Displaying a customer according to its ID
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        private static void DisplayCustomer(BlApi.IBL bl, int id, bool flag)
        {
            do
            {
                Console.WriteLine("Enter the Customer ID number");
                flag = int.TryParse(Console.ReadLine(), out id);
            } while (!flag);
            Console.WriteLine(bl.CustomerDisplay(id));
        }

        /// <summary>
        /// Displaying a parcel according to its ID
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        private static void DisplayParcel(BlApi.IBL bl, int id, bool flag)
        {
            do
            {
                Console.WriteLine("Enter the parcel ID number");
                flag = int.TryParse(Console.ReadLine(), out id);
            } while (!flag);
            Console.WriteLine(bl.ParcelDisplay(id));
        }

        /// <summary>
        /// Drone delivering a parcel
        /// </summary>
        /// <param name="bl"></param>
        private static void DeliveryOfParcelByDrone(BlApi.IBL bl)
        {
            try
            {
                int droneId;
                bool flag;
                do
                {
                    Console.WriteLine("Enter a unique ID number of drone");
                    flag = int.TryParse(Console.ReadLine(), out droneId);
                } while (!flag);
                bl.DeliveryOfParcelByDrone(droneId);
                Console.WriteLine("The update was successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        /// <summary>
        /// Drone picking up a parcel
        /// </summary>
        /// <param name="bl"></param>
        private static void ParcelCollectionByDrone(BlApi.IBL bl)
        {
            try
            {
                int droneId;
                bool flag;
                do
                {
                    Console.WriteLine("Enter a unique ID number of drone");
                    flag = int.TryParse(Console.ReadLine(), out droneId);
                } while (!flag);
                bl.ParcelCollectionByDrone(droneId);
                Console.WriteLine("The update was successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Assigning a parcel to a drone
        /// </summary>
        /// <param name="bl"></param>
        private static void AffiliateParcelToDrone(BlApi.IBL bl)
        {
            try
            {
                int droneId;
                bool flag;
                do
                {
                    Console.WriteLine("Enter a unique ID number of drone");
                    flag = int.TryParse(Console.ReadLine(), out droneId);
                } while (!flag);
                bl.AffiliateParcelToDrone(droneId);
                Console.WriteLine("The update was successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Drone being released from charger
        /// </summary>
        /// <param name="bl"></param>
        private static void ReleaseDroneFromCharging(BlApi.IBL bl)
        {
            try
            {
                int droneId;
                bool flag;
                do
                {
                    Console.WriteLine("Enter a unique ID number of drone");
                    flag = int.TryParse(Console.ReadLine(), out droneId);
                } while (!flag);
                bl.ReleaseDroneFromCharging(droneId);
                Console.WriteLine("The update was successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Charging a drone
        /// </summary>
        /// <param name="bl"></param>
        private static void SendingDroneForCharging(BlApi.IBL bl)
        {
            try
            {
                int droneId;
                bool flag;
                do
                {
                    Console.WriteLine("Enter a unique ID number of drone");
                    flag = int.TryParse(Console.ReadLine(), out droneId);
                } while (!flag);
                bl.SendingDroneForCharging(droneId);
                Console.WriteLine("The update was successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Updating a customer's info
        /// </summary>
        /// <param name="bl"></param>
        private static void UpdateCustomer(BlApi.IBL bl)
        {
            bool flag;
            int IdCustomer;

            try
            {
                Customer updateCustomer = new Customer();
                do
                {
                    Console.WriteLine("Enter a unique ID number of Customer");
                    flag = int.TryParse(Console.ReadLine(), out IdCustomer);
                } while (!flag);
                updateCustomer.Id = IdCustomer;
                Console.WriteLine("Insert the new name of the customer or click enter");
                updateCustomer.Name = Console.ReadLine();
                Console.WriteLine("Insert the new phone of the customer or click enter");
                updateCustomer.Phone = Console.ReadLine();
                bl.UpdateCustomer(updateCustomer);
                Console.WriteLine("The update was successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Updating a stations's info
        /// </summary>
        /// <param name="bl"></param>
        private static void UpdateStation(BlApi.IBL bl)
        {
            bool flag;
            int IdStation, positions = 0;
            string chargingPositions;
            try
            {
                Station updateStation = new Station();
                do
                {
                    Console.WriteLine("Enter a unique ID number of Station");
                    flag = int.TryParse(Console.ReadLine(), out IdStation);
                } while (!flag);
                updateStation.Id = IdStation;
                Console.WriteLine("Insert the new name of the station or click enter");
                updateStation.Name = Console.ReadLine();
                do
                {
                    Console.WriteLine("Insert the total amount of charging of the station or click enter");
                    chargingPositions = Console.ReadLine();
                    if (chargingPositions == "") { flag = true; }
                    else { flag = int.TryParse(chargingPositions, out positions); }
                } while (!flag);
                if (chargingPositions != "") bl.UpdateStationPositions(updateStation.Id, positions);
                if (updateStation.Name != "") bl.UpdateStationName(updateStation.Id, updateStation.Name);
                Console.WriteLine("The update was successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Updating a drone's info
        /// </summary>
        /// <param name="bl"></param>
        private static void UpdateDrone(BlApi.IBL bl)
        {
            bool flag;
            int idDrone;

            try
            {
                DroneToList updateDrone = new DroneToList();
                do
                {
                    Console.WriteLine("Enter a unique ID number of drone");
                    flag = int.TryParse(Console.ReadLine(), out idDrone);
                } while (!flag);
                updateDrone.Id = idDrone;
                Console.WriteLine("Insert the  new model of the drone");
                updateDrone.Model = Console.ReadLine();
                bl.UpdateDrone(updateDrone);
                Console.WriteLine("The update was successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Adding a customer
        /// </summary>
        /// <param name="bl"></param>
        private static void AddCustomer(BlApi.IBL bl)
        {
            bool flag;
            int IdCustomer;
            double num;
            try
            {
                Customer newCustomer = new Customer();
                Location locationOfnewCustomer = new Location();
                do
                {
                    Console.WriteLine("Enter a unique ID number of Customer");
                    flag = int.TryParse(Console.ReadLine(), out IdCustomer);
                } while (!flag);
                newCustomer.Id = IdCustomer;
                Console.WriteLine("Enter the customer name");
                newCustomer.Name = Console.ReadLine();
                Console.WriteLine("Enter a phone number");
                newCustomer.Phone = Console.ReadLine();
                do
                {
                    Console.WriteLine("What is your Latitude?");
                    flag = double.TryParse(Console.ReadLine(), out num);
                } while (!flag);
                locationOfnewCustomer.Lattitude = num;
                do
                {
                    Console.WriteLine("What is your longitude?");
                    flag = double.TryParse(Console.ReadLine(), out num);
                } while (!flag);
                locationOfnewCustomer.Longitude = num;
                newCustomer.Location = locationOfnewCustomer;
                bl.AddCustomer(newCustomer);
                Console.WriteLine("Added successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Adding a parcel
        /// </summary>
        /// <param name="bl"></param>
        private static void AddParcel(BlApi.IBL bl)
        {
            bool flag;
            int IdCustomer;
            try
            {
                Parcel newParcel = new Parcel();
                CustomerInParcel sender = new CustomerInParcel();
                CustomerInParcel target = new CustomerInParcel();
                DroneInParcel droneInParcel = new DroneInParcel();
                do
                {
                    Console.WriteLine("Enter a sending customer ID number");
                    flag = int.TryParse(Console.ReadLine(), out IdCustomer);
                } while (!flag);
                sender.Id = IdCustomer;
                newParcel.Sender = sender;
                do
                {
                    Console.WriteLine("Enter a target Customer  ID number");
                    flag = int.TryParse(Console.ReadLine(), out IdCustomer);
                } while (!flag);
                target.Id = IdCustomer;
                newParcel.Target = target;
                Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy");
                newParcel.Weight = (WeightCategories)int.Parse(Console.ReadLine());
                Console.WriteLine("enter  1-Regular , 2-Express , 3-Urgent");
                newParcel.Priority = (Priorities)int.Parse(Console.ReadLine());
                newParcel.Creating = DateTime.Now;
                newParcel.Affiliation = DateTime.MinValue;
                newParcel.Delivered = DateTime.MinValue;
                newParcel.PickedUp = DateTime.MinValue;
                droneInParcel.DroneId = 0;
                newParcel.drone = droneInParcel;
                bl.AddParcel(newParcel);
                Console.WriteLine("Added successfully");
                Console.WriteLine("Parcel #: " + bl.GetParcelId());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Adding a drone
        /// </summary>
        /// <param name="bl"></param>
        private static void AddDrone(BlApi.IBL bl)
        {
            bool flag;
            int Id;
            try
            {
                int chargingStationId;
                DroneToList newDrone = new DroneToList();
                do
                {
                    Console.WriteLine("Enter a unique ID number");
                    flag = int.TryParse(Console.ReadLine(), out Id);
                } while (!flag);
                newDrone.Id = Id;
                Console.WriteLine("Insert the model of the drone");
                newDrone.Model = Console.ReadLine();
                Console.WriteLine("enter 1-Light ,2- Medium ,3-Heavy");
                newDrone.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                do
                {
                    Console.WriteLine("Enter a unique ID number station to put the drone in initial charge");
                    flag = int.TryParse(Console.ReadLine(), out Id);
                } while (!flag);
                chargingStationId = Id;
                bl.AddDrone(newDrone, chargingStationId);
                Console.WriteLine("Added successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Adding a station
        /// </summary>
        /// <param name="bl"></param>
        public static void AddStation(BlApi.IBL bl)
        {
            bool flag;
            int idStation, number;
            double num;
            try
            {
                Station newStation = new Station();
                Location locationOfNewStation = new Location();
                do
                {
                    Console.WriteLine("Enter a unique ID number of Station");
                    flag = int.TryParse(Console.ReadLine(), out idStation);
                } while (!flag);
                newStation.Id = idStation;
                Console.WriteLine("Enter the name of the station");
                newStation.Name = Console.ReadLine();
                do
                {
                    Console.WriteLine("What is your Longtitude?");
                    flag = double.TryParse(Console.ReadLine(), out num);
                } while (!flag);
                locationOfNewStation.Longitude = num;
                do
                {
                    Console.WriteLine("What is your Lattitude?");
                    flag = double.TryParse(Console.ReadLine(), out num);
                } while (!flag);
                locationOfNewStation.Lattitude = num;
                newStation.location = locationOfNewStation;
                do
                {
                    Console.WriteLine("Enter the number of charging points available at the station");
                    flag = int.TryParse(Console.ReadLine(), out number);
                } while (!flag);
                newStation.AvailableChargeSlots = number;
                newStation.droneInCharging = new List<DroneInCharging>(0);
                bl.AddStation(newStation);
                Console.WriteLine("Added successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
