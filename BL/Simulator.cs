using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;
using System.Diagnostics;

namespace BL
{
    class Simulator
    {
        private Stopwatch stopWatch;
        double speedDrone=3;
        Drone drone;
        internal volatile bool stopSim = false;
        internal Simulator(BlApi.IBL bl, int droneId, Action action, Func<bool> checkStop) 
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
            drone = bl.DroneDisplay(droneId);
             while(checkStop())
            {
                switch(drone.Status)
                {
                 
              
                }

            }
        }

    }
}
