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
        public class ParceltAtCustomer
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatus status { get; set; }
            public CustomerInParcel OpposingSide { get; set; }
            public override string ToString()
            {
                return $"Id: {Id}" +
                    $", weight: {Weight}" +
                    $", priority: {Priority}" +
                    $", status: {status}," +
                    $" Opposing Customer: {OpposingSide}";
            }

        }
    }
    
}
