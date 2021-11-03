using System;

namespace IBL
{
    namespace BO
    {
        public class Customer : IBL
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public override string ToString()
            {
                return $"Customer #{Id}, Name:{Name}, Phone #:{Phone}, {Convert.ConvertLongitude(Longitude)}, {Convert.ConvertLattitude(Lattitude)}";
            }
        }
    }
    
}
