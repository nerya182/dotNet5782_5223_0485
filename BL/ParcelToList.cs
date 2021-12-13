using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelToList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus ShipmentStatus { get; set; }
        public override string ToString()
        {
            return $"Parcel #{Id}, Sender name: {SenderName}, Target  name: {TargetName}," +
                $"Weight: {Weight}, Priority: {Priority}, ShipmentStatus: {ShipmentStatus}\n";
        }
    }
}



