using System;
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

        public static Parcel GetParcel(int parcelId)
        {
            return DataSource.parcels[parcelId];
        }
        public static Parcel GetParcelRequested(int parcelId)
        {
            return DataSource.parcels[parcelId].Requested();
        }
        public static Parcel GetParcelScheduled(int parcelId)
        {
            return DataSource.parcels[parcelId].Scheduled();
        }

        public static Parcel GetParcelPickedUp(int parcelId)
        {
            return DataSource.parcels[parcelId].Scheduled();
        }

        public static Parcel GetParcelDelivered(int parcelId)
        {
            return DataSource.parcels[parcelId].Delivered();
        }

        public static int GetNewParcelId()
        {
            return DataSource.Config.newParcelId;
        }

        public static void AddStation()
        {
            Station newStation = new Station();

            newStation.Id = Config.newStationId;

            Console.WriteLine("enter name a new staion\n");
            newStation.Name = Console.ReadLine();
           
            Console.WriteLine("Enter the longitude of the station\n");
            newStation.Longitude = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Enter the Lattitude of the station\n");
            newStation.Lattitude = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Enter the number of charging points available at the station\n");
            newStation.ChargeSlots = int.Parse(Console.ReadLine());

            stations[Config.newStationId++] = newStation;
        }
         
        public static void addDrone()
        {
            Drone newDrone = new Drone();

            newDrone.Id = Config.newDroneId;

            Console.WriteLine("enter name have new staion\n");
            newDrone.Model = Console.ReadLine();

            Console.WriteLine("enter Light/Medium/Heavy\n");
            WeightCategories myMaxWeight;
            Enum.TryParse(Console.ReadLine(), out myMaxWeight);
            newDrone.MaxWeight = (WeightCategories)myMaxWeight;

            Console.WriteLine("enter Available/Delivery/Charging\n");
            DroneStatuses myStatus;
            Enum.TryParse(Console.ReadLine(), out myStatus);
            newDrone.Status = (DroneStatuses)myStatus;

            newDrone.Battery = 100;

            drones[Config.newDroneId++] = newDrone;
        }

      
        
        
        
        
        
        
        public static void Affiliate()
        {
            Console.WriteLine(" What is the Parcel Id? \n");
            int ID;
            int.TryParse(Console.ReadLine(), out ID);
            //Parcel parcel = GetParcel(ID);
            for (int i = 0; i < Config.newDroneId; i++)
            {
                if (drones[i].Status == DroneStatuses.Available)
                {
                    drones[i].Status = DroneStatuses.Delivery;
                    parcels[ID].DroneId = drones[i].Id;
                    break;
                }
            }
        }
        public static void PickupParcel()
        {
            Console.WriteLine(" What is the Parcel Id? \n");
            int ID;
            int.TryParse(Console.ReadLine(), out ID);
            drones[parcels[ID].DroneId].Status = DroneStatuses.Delivery;
            parcels[ID].PickedUp = DateTime.Now;
        }

        public static void addParcel(int myId, int SenderId,int TargetId, WeightCategories myMaxWeight, Priorities myPriority,DateTime CreatePackage)
        {
            Parcel newParcel = new Parcel() { Id=myId,SenderId=SenderId,TargetId=TargetId,Weight=myMaxWeight,Priority=myPriority,da};
            parcels[myId] = newParcel;
        }




















        public static void SupplyParcel()
        {
            Console.WriteLine(" What is the Parcel Id? \n");
            int ID;
            int.TryParse(Console.ReadLine(), out ID);
            drones[parcels[ID].DroneId].Status = DroneStatuses.Available;
            parcels[ID].Delivered = DateTime.Now;
        }

        public static void SendDroneToCharge()
        {

        }


        public static void AddCustomer()
        {
            Customer newCustomer = new Customer();

            newCustomer.Id = Config.newCustomerId;

            Console.WriteLine("enter name have new Customer\n");
            newCustomer.Name = Console.ReadLine();

            Console.WriteLine("enter number of phone\n");
            newCustomer.Phone = Console.ReadLine();

            Console.WriteLine("enter longitude\n");
            newCustomer.Longitude = double.Parse(Console.ReadLine());

            Console.WriteLine("enter Lattitude\n");
            newCustomer.Lattitude = double.Parse(Console.ReadLine());

            customers[Config.newCustomerId++] = newCustomer;
        }
    }


    



}



























