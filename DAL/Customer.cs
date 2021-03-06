using System;

namespace DO
{
    public struct Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public override string ToString()
        {
            return $"Customer #{Id}, Name:{Name}, Phone #:{Phone}, {BO.Convert.ConvertLongitude(Longitude)}, {BO.Convert.ConvertLattitude(Lattitude)}";
        }
    }
}


