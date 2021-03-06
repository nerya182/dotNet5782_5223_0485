using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    public class DataSource
    {
        internal class Config
        {
            internal static int NewParcelId = 1;

            internal static double available=30;
            internal static double lightWeight=35;
            internal static double mediumWeight=40;
            internal static double heavyWeight=45;
            internal static double chargeSpeed=10;
        }
     
        public static List<Customer> Customers = new List<Customer>();
        public static List<Station> Stations = new List<Station>();
        public static List<Drone> Drones = new List<Drone>();
        public static List<Parcel> Parcels = new List<Parcel>();
        public static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        public static void Initialize()
        {
            CreateStations();
            CreateDrones();
            CreateCustomers();
            CreateParcels();
         }
        /// <summary>
        /// Creating 2 stations
        /// </summary>
        private static void CreateStations()
        {
            Station newStation1 = new Station() { Id = 56, Name = "Tachana Merkazit", AvailableChargeSlots = 10, Lattitude = 31.78945, Longitude = 35.20301 };
            Stations.Add(newStation1);

            Station newStation2 = new Station() { Id = 67, Name = "Shuk Machane Yehudah", AvailableChargeSlots = 10, Lattitude = 31.78489, Longitude = 35.21257 };
            Stations.Add(newStation2);


        }
        /// <summary>
        /// Creating 5 Drones Randomly
        /// </summary>
        private static void CreateDrones()
        {
            Random R = new Random();
            for (int i = 0; i < 3; i++)//רחפנים מסוג קל
            {
                NameDrone nameDrone = (NameDrone)R.Next(0, 18);
                WeightCategories weightCategories = (WeightCategories)(R.Next(1,4));
                Drone newDrone = new Drone() { Id = R.Next(1000, 100001), Model = nameDrone.ToString(), MaxWeight = weightCategories};
                Drones.Add(newDrone);
            }
            for (int i = 0; i < 3; i++)//רחפנים מסוג בינוני
            {
                NameDrone nameDrone = (NameDrone)R.Next(0, 18);
                WeightCategories weightCategories = (WeightCategories)(2);
                Drone newDrone = new Drone() { Id = R.Next(1000, 100001), Model = nameDrone.ToString(), MaxWeight = weightCategories };
                Drones.Add(newDrone);
            }
            for (int i = 0; i < 4; i++)//רחפנים מסוג כבד
            {
                NameDrone nameDrone = (NameDrone)R.Next(0, 18);
                WeightCategories weightCategories = (WeightCategories)(3);
                Drone newDrone = new Drone() { Id = R.Next(1000, 100001), Model = nameDrone.ToString(), MaxWeight = weightCategories };
                Drones.Add(newDrone);
            }
        }
        /// <summar y>
        /// Creating 10 Customers Randomly
        /// </summary>
        private static void CreateCustomers()
        {
            Random R = new Random();
            int id;
            for (int i = 0; i < 10; i++)
            {
                do
                {
                    id = R.Next(100000000, 1000000000);
                } while (!CheckId(id));

                CustomerName customerName = (CustomerName)R.Next(0, 11);
                Customer newCustomer = new Customer() { Id =id , Name = customerName.ToString(), Phone = "050" + R.Next(1111111,9999999), Lattitude = (R.Next(31591956, 32049510)) / ((1000000.0)), Longitude = (R.Next(34802513, 35393515)) / ((1000000.0)) };
                Customers.Add(newCustomer);
            }
        }
        /// <summary>
        /// Creating 10 Parcels Randomly
        /// </summary>
        private static void CreateParcels()
        {
            Random R = new Random();
            for (int i = 0; i < 3; i++)//חבילות קלות לרחפנים קלים,שנוצרו אך לא שויכו
            {
                Priorities priorities = (Priorities)R.Next(1, 3);
                WeightCategories weightCategories = (WeightCategories)(R.Next(1,3)) ;
                Parcel newParcel = new Parcel()
                {
                    Id = Config.NewParcelId,
                    SenderId = Customers[R.Next(0,Customers.Count)].Id,
                    TargetId = Customers[R.Next(0, Customers.Count)].Id,
                    DroneId = 0,
                    Weight = weightCategories,
                    Priority = priorities,
                    Creating = DateTime.Now,
                    Delivered = null,
                    PickedUp = null,
                    Affiliation = null
                };
                Parcels.Add(newParcel);
                Config.NewParcelId++;
            }
            for (int i = 0; i < 3; i++)//חבילות שנוצרו ושויכו 
            {
                Priorities priorities = (Priorities)R.Next(1, 3);
                WeightCategories weightCategories = (WeightCategories)(2);
                Parcel newParcel = new Parcel()
                {
                    Id = Config.NewParcelId,
                    SenderId = Customers[R.Next(0, Customers.Count)].Id,
                    TargetId = Customers[R.Next(0, Customers.Count)].Id,
                    DroneId = Drones[i+3].Id,
                    Weight = weightCategories,
                    Priority = priorities,
                    Creating = DateTime.Now.AddMinutes(-60),
                    Affiliation = DateTime.Now.AddMinutes(-30),
                    Delivered = null,
                    PickedUp = null,
                   
                };
                Parcels.Add(newParcel);
                Config.NewParcelId++;
            }
            for (int i = 0; i < 2; i++)// חבילות שנוצרו ושויכו ונאספו אך לא סופקו  
            {
                Priorities priorities = (Priorities)R.Next(1, 3);
                WeightCategories weightCategories = (WeightCategories)(3);
                Parcel newParcel = new Parcel()
                {
                    Id = Config.NewParcelId,
                    SenderId = Customers[R.Next(0, Customers.Count)].Id,
                    TargetId = Customers[R.Next(0, Customers.Count)].Id,
                    DroneId = Drones[i+6].Id,
                    Weight = weightCategories,
                    Priority = priorities,
                    Creating = DateTime.Now.AddMinutes(-120),
                    Affiliation = DateTime.Now.AddMinutes(-90),
                    PickedUp = DateTime.Now.AddMinutes(-60),
                    Delivered = null
                };
                Parcels.Add(newParcel);
                Config.NewParcelId++;
            }
            for (int i = 0; i < 2; i++)//חבילות שנוצרו,שויכו, נאספו,סופקו 
            {
                Priorities priorities = (Priorities)R.Next(1, 3);
                WeightCategories weightCategories = (WeightCategories)(3);
                Parcel newParcel = new Parcel()
                {
                    Id = Config.NewParcelId,
                    SenderId = Customers[R.Next(0, Customers.Count)].Id,
                    TargetId = Customers[R.Next(0, Customers.Count)].Id,
                    DroneId =0,
                    Weight = weightCategories,
                    Priority = priorities,
                    Creating = DateTime.Now.AddMinutes(-180),
                    Delivered = DateTime.Now.AddMinutes(-30),
                    PickedUp = DateTime.Now.AddMinutes(-90),
                    Affiliation = DateTime.Now.AddMinutes(-120)
                };
                Parcels.Add(newParcel);
                Config.NewParcelId++;
            }
        }
        public static bool CheckId(int id)
        {
            int number = id / 10;
            int s = 0, kefel = 0, SumOfDigitsOfProduct = 0;
            for (int i = 1; i <= 8; i++)
            {
                if (i % 2 == 0)
                    kefel = (number % 10);
                else
                    kefel = (number % 10) * 2;
                number /= 10;
                SumOfDigitsOfProduct = sumDigits(kefel);
                s += SumOfDigitsOfProduct;
            }
            if (s % 10 == 0)
                return 0 == id % 10;
            else
                return (10 - s % 10) == id % 10;
        }
        /// <summary>
        /// help function
        /// </summary>
        /// <param name="num"></param>
        /// <returns>int</returns>
        private static int sumDigits(int num)
        {
            int s = 0;
            while (num > 0)
            {
                s = s + num % 10;
                num /= 10;
            }
            return s;
        }

    }
}



























