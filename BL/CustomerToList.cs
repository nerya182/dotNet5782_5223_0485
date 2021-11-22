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
            public int DeliveredSuppliedParcels { get; set; }
            public int DeliveredNotSuppliedParcels { get; set; }
            public int ReceivedParcels { get; set; }
            public int OnTheWayParcels { get; set; }
            public override string ToString()
            {
                return $"Customer #{Id} , " +
                       $"Name:{Name}," +
                       $"phone:{Phone}," +
                       $"Delivered & Supplied Parcels:" +
                       $"{DeliveredSuppliedParcels},\n" +
                    $"Delivered & NotSupplied Parcels:{DeliveredNotSuppliedParcels}," +
                       $"Received Parcels:{ReceivedParcels}," +
                       $"On the Way Parcels to him:{OnTheWayParcels}\n";
            }
        }
    }
    
}
