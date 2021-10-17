using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            int input;
            do
            {
                Console.WriteLine("Menu:\n" +
                   "1- Add a new DroneCharge/Drone/Customer/Parcel.\n" +
                   "2- Update assignment/Collection /Delivery /Charging /Release.\n" +
                   "3- Display of base stations/Drone/Customer/ Parcel\n" +
                   "4- Print all bbase stations/Drone/Customer/Parcel/Packages not yet associated/Base stations with available charging stations.\n" +
                   "5- Exit");
                while(int.TryParse(Console.ReadLine(),out input))
                {
                    Console.WriteLine("ERROR, try again\n");
                }

            } while (input!=5);
        }

    }
}
