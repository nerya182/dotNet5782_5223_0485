using System;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public int DroneId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public Datetime Requested { get; set; }
            public Datetime Scheduled { get; set; }
            public Datetime PickedUp { get; set; }
            public Datetime Delivered { get; set; }


            public override string ToString()
            {
                return $"Parcel #{Id}, SenderId: #{SenderId}, TargetId: #{TargetId}, DroneId: #{DroneId}, {Weight}, {Priority},{Requested},{Scheduled},{PickedUp},{Delivered},";
            }

        }
    }
}
   

