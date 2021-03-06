using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public int AvailableChargeSlots { get; set; }

        public override string ToString()
        {
            return $"Station #{Id}, Name:{Name},{BO.Convert.ConvertLongitude(Longitude)},{BO.Convert.ConvertLattitude(Lattitude)},{AvailableChargeSlots}";
        }
    }
}

