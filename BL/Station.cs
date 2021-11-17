using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int AvailableChargeSlots { get; set; }
            public Location location { get; set; }
            public List<DroneInCharging> droneInCharging { get; set; }
            public override string ToString()
            {
                string output = $"Station #{Id}, Name:{Name},AvailableChargeSlots:{AvailableChargeSlots},location :{location}";
                if (droneInCharging.Count != 0)
                  output += $", List of Drones that are charging: { droneInCharging}";
                   
                return output;   
            }
        }
    }
}
