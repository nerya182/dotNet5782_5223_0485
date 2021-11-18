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
            public Location Location { get; set; }
            public List<ParceltAtCustomer> FromCustomer { get; set; }
            public List<ParceltAtCustomer> ToCustomer { get; set; }
            public override string ToString()
            {
                return $"Customer id: #{Id} , Name:{Name} , Phone #:{Phone} , Location {Location}\n"; 
            }
        }
    }
    
}
