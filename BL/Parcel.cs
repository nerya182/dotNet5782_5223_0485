using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; } 
            public Customer Sender { get; set; }
            public Customer Target { get; set; }
            public DroneInParcel drone { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime Creating { get; set; }
            public DateTime Affiliation { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public override string ToString()
            {
                return $"Parcel #{Id}, SenderId:, {Weight}, {Priority},{Creating},{ Affiliation},{PickedUp},{Delivered}\n";
            }
        }
    }
}
