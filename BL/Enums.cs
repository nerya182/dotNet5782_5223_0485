using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum CHOICE
    {
        ADD = 1, UPDATE, DISPLAY, VIEW_LIST, DISTANCE, EXIT
    }
    public enum WeightCategories
    {
        Light = 1, Medium, Heavy
    }
    public enum ParcelStatus { Created, Assigned, PickedUp, Supplied }
    public enum Priorities { Regular = 1, Express, Urgent }
    public enum DroneStatuses { Available = 1, Charging, Delivery }

}

