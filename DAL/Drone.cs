using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string MOdel { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery ToString()
            {
                return $"Drone #{Id}: Model={MOdel}, { Status} , { MaxWeight}, battery={(int)Battery} ";
            }

        }
    }
   
}
