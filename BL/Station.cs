using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string output = $"Station #{Id}, Name: {Name}, AvailableChargeSlots: {AvailableChargeSlots}, Location: {location}\n";
            if (droneInCharging.Count != 0)
            {
                output += $"List of Drones that are charging:\n";
                for (int i = 0; i < droneInCharging.Count; i++)
                {
                    output += droneInCharging[i];
                }
            }
            return output;
        }
    }
}

