using System;
using System.Collections.Generic;

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
            if (FromCustomer.Capacity!=0)
            {
                output += $"List of Parcel From Customer:\n";
                for (int i = 0; i < FromCustomer.Count; i++)
                {
                    output += FromCustomer[i];
                    output += "\n";
                }
            }
            if (ToCustomer!=null)
            {
                output += $"List of Parcel To Customer:\n";
                for (int i = 0; i < ToCustomer.Count; i++)
                {
                    output += ToCustomer[i].ToString();
                    output += "\n";
                }
            }
            return output;
        }
    }
}


