using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneToList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public override string ToString()
            {
                return $"DroneToList #{Id}, Model :{Model },MaxWeight:{MaxWeight}," +
                    $"Status :{Status}Battery:{Battery},{Convert.ConvertLongitude(Longitude)},{Convert.ConvertLattitude(Lattitude)}\n";
            }
        }
    }
    
}
