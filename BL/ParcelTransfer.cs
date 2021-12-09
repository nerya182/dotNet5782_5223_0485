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
        public class ParcelTransfer 
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public bool ParcelSituation { get; set; }
            public Location collection { get; set; }
            public Location SupplyPoint { get; set; }
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Receiver { get; set; }
            public double distanceTransportation { get; set; }

            public override string ToString()
            {
                string output = $"Id: {Id}, Weight: {Weight}, Priority: {Priority} Collect location: {collection}, Supply location: {SupplyPoint}, distance transportation: {(int)distanceTransportation} k\"m " +
                    $"\nSender: {Sender}\n, Target: {Receiver}\n, Parcel Situation:";
                if (!ParcelSituation)
                    output += "Waiting to be picked up";
                else
                    output += "On its way to destination";
                return output;

            }
        }
    }

}