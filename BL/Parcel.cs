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
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Target { get; set; }
            public DroneInParcel drone { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime? Creating { get; set; }
            public DateTime? Affiliation { get; set; }
            public DateTime? PickedUp { get; set; }
            public DateTime? Delivered { get; set; }

            public override string ToString()
            {
                string output = $"Parcel #{Id}, Weight: {Weight}, Priority: {Priority}, Created: {Creating}, Affiliated: {Affiliation}, Picked-Up: {PickedUp}, Delivered: {Delivered}\n";
                output += $"Sender of Parcel: {Sender}, Target of Parcel: {Target}, Drone In Parcel: {drone}\n";
                return output;
            }
        }
    }
}
