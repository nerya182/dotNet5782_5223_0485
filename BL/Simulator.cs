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
        double speedDrone=4;
        double distance,timeTask;
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
                    case DroneStatuses.Available:
                        try
                        {
                            lock (bl) { bl.AffiliateParcelToDrone(drone.Id); }
                            action();
                            drone = bl.DroneDisplay(drone.Id);
                            distance = bl.GetDistanceFromLatLonInKm(drone.Location.Lattitude,drone.Location.Longitude,drone.ParcelTransfer.collection.Lattitude, drone.ParcelTransfer.collection.Longitude);
                            timeTask = distance / speedDrone;
                            
                            Thread.Sleep(1000);

                            lock (bl) { bl.ParcelCollectionByDrone(drone.Id); }
                            distance = bl.GetDistanceFromLatLonInKm(drone.ParcelTransfer.collection.Lattitude, drone.ParcelTransfer.collection.Longitude, drone.ParcelTransfer.SupplyPoint.Lattitude, drone.ParcelTransfer.SupplyPoint.Longitude);
                            timeTask = distance / speedDrone;
                             
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        break;
                    case DroneStatuses.Charging:
                        break;
                    case DroneStatuses.Delivery:
                        break;


                }

            }
        }

    }
}
