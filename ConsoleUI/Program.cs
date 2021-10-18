using System;
using DalObject;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Char ch;
            do
            {
                Console.WriteLine("Menu:\n" +
                   "a- Add a new base Station/Drone/Customer/Parcel.\n" +
                   "u- Update assignment/Collection /Delivery /Charging /Release.\n" +
                   "d- Display of base stations/Drone/Customer/ Parcel\n" +
                   "p- Print all bbase stations/Drone/Customer/Parcel/Packages not yet associated/Base stations with available charging stations.\n" +
                   "e- Exit");
                while(char.TryParse(Console.ReadLine(),out ch))
                {
                    Console.WriteLine("ERROR, try again\n");
                }
                DalObject.DataSource.Initialize();

                switch (ch)
                {
                    case 'a':
                        Console.WriteLine("What would you like to add? \n" +
                                     "1- Add a new base Station.\n" +
                                     "2- Add a new Drone.\n" +
                                     "3-Add a new Customer.\n" +
                                     "4-Add a new Parcel.\n");
                        int choice;
                        do
                        {
                            while (!int.TryParse(Console.ReadLine(), out choice))
                            {
                                Console.WriteLine("ERROR, try again\n");
                            }
                        } while (choice != 1 && choice != 2 && choice != 3 && choice != 4);
                        switch (choice)
                        {
                            case '1': DalObject.DalObject.
                                break;
                            case '2':
                                break;
                            case '3':
                                break;
                            case '4':
                                break;

                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

            } while (ch!='e');
        }

    }
}
