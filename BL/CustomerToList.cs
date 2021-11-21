using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerToList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int Delivered_Supplied_Parcels { get; set; }
            public int Delivered_NotSupplied_Parcels { get; set; }
            public int Received_Parcels { get; set; }
            public int OnTheWay_Parcels { get; set; }
            public override string ToString()
            {
                return $"Customer #{Id} , " +
                       $"Name:{Name}," +
                       $"phone:{Phone}," +
                       $"Delivered & Supplied Parcels:" +
                       $"{Delivered_Supplied_Parcels},\n" +
                    $"Delivered & NotSupplied Parcels:{Delivered_NotSupplied_Parcels}," +
                       $"Received Parcels:{Received_Parcels}," +
                       $"On the Way Parcels to him:{OnTheWay_Parcels}\n";
            }
        }
    }
    
}
