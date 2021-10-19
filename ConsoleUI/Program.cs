using System;
using DalObject;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            CHOICE choice;
          
            Console.WriteLine("Menu:\n" +
                   "ADD- Add a new base Station/Drone/Customer/Parcel.\n" +
                   "UPDATE- Update assignment/Collection /Delivery /Charging /Release.\n" +
                   "DISPLAY- Display of base stations/Drone/Customer/ Parcel\n" +
                   "VIEW_LIST- Print all bbase stations/Drone/Customer/Parcel/Packages not yet associated/Base stations with available charging stations.\n" +
                   "EXIT- Exit");
           Enum.TryParse(Console.ReadLine(),out choice);
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
                        case 'a':DalObject.DalObject.AddStation();
                            break;
                        case 'b':
                            break;
                        case 'c':
                            break;
                        case 'd':
                            break;
                        
                    }

                    break;




















                case CHOICE.UPDATE:
                    Console.WriteLine(" What would you like to do?" +
                        "a-Affiliate Parcel to Drone?\n" +
                        "b- Pickup Parcel with Drone? \n" +
                        "c- Deliver Parcel to Customer? \n" +
                        "d- Send Drone to Charge? \n" +
                        "e- Release Drone from Charge? \n");
                    char input;
                    char.TryParse(Console.ReadLine(), out input);
                    switch (input)
                    {
                        case 'a':
                            DalObject.DalObject.Affiliate();
                            break;
                        case 'b':


                    }

                    break;
                case CHOICE.DISPLAY:
                    break;
                case CHOICE.VIEW_LISTS:
                    break;
                case CHOICE.EXIT:
                    break;
                default:
                    break;
            }





        }

    }
}
