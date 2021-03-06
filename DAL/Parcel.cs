using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DateTime? Creating { get; set; }
        public DateTime? Affiliation { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }

        public override string ToString()
        {
            return $"Parcel #{Id}, SenderId: #{SenderId}, TargetId: #{TargetId}, " +
                $"DroneId: #{DroneId}, {Weight}, {Priority},{Creating},{ Affiliation},{PickedUp},{Delivered}\n";
        }
    }
}


