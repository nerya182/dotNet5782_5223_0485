using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public enum CHOICE
        {
            ADD, UPDATE, DISPLAY, VIEW_LIST, DISTANCE, EXIT
        }
        public enum WeightCategories
        {
            Light = 1, Medium, Heavy
        }
        public enum ShipmentStatus { Created, Assigned, PickedUp, Supplied}
        public enum Priorities { Regular = 1, Express, Urgent }
        public enum DroneStatuses { Available = 1, Delivery, Charging }
        
    }
}
