using System;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int StationId { get; set; }

            public DateTime EntryTime { get; set; }
            public override string ToString()
            {
                return $"DroneId: #{DroneId}, StationId: #{StationId}, enter Time:{EntryTime}" ;
            } 

        }
    }
}
