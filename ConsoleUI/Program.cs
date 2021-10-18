using System;
using DalObject;
using IDAL.DO;

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
                DalObject.DataSource.Initialize();

                switch (input)
                {
                    case 1:
                        Console.WriteLine("What would you like to add? \n" +
                                     "1- Add a new DroneCharge.\n" +
                                     "2- Add a new Drone.\n"+
                                     "3-Add a new Customer.\n"+
                                     "4-Add a new Parcel.\n");
                        int choice;
                        do
                        {
                            while (!int.TryParse(Console.ReadLine(), out choice))
                            {
                                Console.WriteLine("ERROR, try again\n");
                            }
                        } while (choice != 1 && choice != 2&&choice !=3 && choice !=4);
                        switch(choice)
                        {
                            case 1:

                        }


                }

            } while (input!=5);
        }

    }
}
