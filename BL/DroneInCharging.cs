using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneInCharging
        {
            public int DroneId { get; set; }
            public double Battery { get; set; }
            public override string ToString()
            {
                return $"DroneId: #{DroneId}, Battery: {(int)Battery}%\n";
            }

        }
    
    }
}
