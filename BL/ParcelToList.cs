using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToList
        {
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string TargetName { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime Creating { get; set; }
            public DateTime Affiliation { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }
            public int parcelId { get; set; }

            public override string ToString()
            {
                return $"Parcel #{Id}, Sender name: #{SenderName}, Target  name: #{TargetName}," +
                    $"parcelId: #{parcelId}, {Weight}, {Priority},{Creating},{ Affiliation},{PickedUp},{Delivered}\n";
            }
        }
    }
    
} 

