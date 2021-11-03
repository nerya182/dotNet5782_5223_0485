﻿using System;

namespace IBL
{
    namespace BO
    {
        public class Customer 
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
                return $"Customer #{Id}, Name:{Name}, Phone #:{Phone},Delivered_Supplied_Parcels:{Delivered_Supplied_Parcels}" +
                    $",Delivered_NotSupplied_Parcels:{Delivered_NotSupplied_Parcels},Received_Parcels:{Received_Parcels},OnTheWay_Parcels :{OnTheWay_Parcels }\n";

            }
        }
    }
    
}
