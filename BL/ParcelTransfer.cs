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
            public CustomerInParcel SenInShipment { get; set; }
            public CustomerInParcel GetInShipment { get; set; }
            public double distanceTransportation { get; set; }

            public override string ToString()
            {
                return $"Id: {Id}, weight:{Weight}, priority: {Priority}, collect location: {collection}, supply location: {SupplyPoint}, distance transportation:{distanceTransportation} ";
            }
        }
    }

}