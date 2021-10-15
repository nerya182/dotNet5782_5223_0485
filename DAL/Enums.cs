using System;



namespace IDAL
{
    namespace DO
    {
        public enum WeightCategories
        {
            [Description("Light Weight")]
            Light,
            [Description("Medium Weight")]
            Medium,
            [Description("Heavy Weight")]
            Heavy
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
     
        
    }
}