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
                string output = $"Customer id: #{Id} , Name:{Name} , Phone #:{Phone} , Location {Location}\n";
                if (FromCustomer.Count != 0)
                {
                    output += $"List of Parcel At Customer:\n";
                    for(int i =0;i<FromCustomer.Count; i++)
                    {
                        output += FromCustomer[i];
                    }
                }                 
                if (ToCustomer.Count != 0)
                {
                    output += $"List of Parcel At Customer:\n";
                    for (int i = 0; i < ToCustomer.Count; i++)
                    {
                        output += ToCustomer[i];
                    }
                }                 
                return output;
            }
        }
    }
    
}
