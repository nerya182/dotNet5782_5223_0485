﻿//using BL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
       public class DroneInParcel
       {
            public int DroneId { get; set; }
            public double Battery { get; set; }
            public Location location { get; set; }
            public override string ToString()
            {
                return $"DroneId: #{DroneId}, Battery: {Battery}%,location :{location}\n";
            }

        }
    
    }
}