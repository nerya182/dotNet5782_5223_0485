using System;
using System.ComponentModel;



namespace IDAL
{
    namespace DO
    {
        public enum CHOICE
        {
			ADD,UPDATE, DISPLAY, VIEW_LISTS, EXIT
        }
        public enum WeightCategories
        {
            Light, Medium, Heavy
        }

        public enum Priorities {Regular, Express, Urgent }
        public enum DroneStatuses {Available, Delivery, Charging }
        public enum CustomerName{Avi, Benny, Gadi, Danny, Freddy, Alex, Nati, Oren, channan, Yair, Tal, Noam}
        public enum NameDrone{mavic, tello, syma, flybird, lion, cobra, cheetah, beck, worm, bean}   
    }
}