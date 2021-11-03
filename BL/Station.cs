﻿using System;
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
            //ךהכניס מיקום
            public int UsedChargeSlots { get; set; }
            public int AvailableChargeSlots { get; set; }
            public 
            public override string ToString()
            {
                return $"Station #{Id}, Name:{Name},UsedChargeSlots:{UsedChargeSlots},AvailableChargeSlots:{AvailableChargeSlots}\n";
            }
        }
    }
}
