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
                return $"Drone #{Id}: Model={Model}, { Status} , { MaxWeight}, battery:{(int)Battery}" +
                    $",Location:{location},shipment Transfer:{shipmentTransfer}\n";
            }
        }
    }
}
