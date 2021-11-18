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
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }
            public ParcelTransfer ParcelTransfer { get; set; }
            public Location Location { get; set; }
            public override string ToString()
            {
                return $"Drone id: {Id} , Model={Model} , Status: { Status} , MaxWeight:{ MaxWeight} , battery:{(int)Battery}" +
                    $"\n";
            }
        }
    }
}
