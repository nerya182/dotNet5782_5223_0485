using System;
using System.Collections.Generic;

namespace IBL
{
    namespace BO
    {
        public class Customer 
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location location { get; set; }

            public List<ShipmentAtCustomer> fromCustomer { get; set; }
            public List<ShipmentAtCustomer> toCustomer { get; set; }
            public override string ToString()
            {
                return $"Customer #{Id}, Name:{Name}, Phone #:{Phone} $\n"; 



            }
        }
    }
    
}
