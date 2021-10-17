using System;
using System.ComponentModel



namespace IDAL
{
    namespace DO
    {
        public enum WeightCategories
        {
            Light, Medium, Heavy
        }

        public enum Priorities {Regular, Express, Urgent }

        public enum DroneStatuses
        {
            [Description("Free for delivery")]
            Available,
            [Description("Doing delivery")]
            Delivery,
            [Description("Being charged")]
            Charging
        }

        public enum NameDrone
        { 
            mavic, tello, syma, flybird, lion, cobra, cheetah, beck, worm, bean
        }
     
        
    }
}