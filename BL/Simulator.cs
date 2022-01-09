using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;
using System.Diagnostics;
using BlApi;

namespace BL
{
    class Simulator
    {
        private Stopwatch stopWatch;
        private const double speedDrone = 1.5;
        private const int delay = 500;
        private const double time = delay / 1000.0;
        private const double step = speedDrone / time;

        Drone drone;
        BlApi.IBL bl;

        internal Simulator(BlApi.IBL BL, int droneId, Action action, Func<bool> checkStop)
        {
            bl = BL;
            drone = bl.DroneDisplay(droneId);
            int? parcelId = null;
            int? satationId = null;
            Station station = null;
            double distance = 0.0;
            int batteryUsage = 0;
            BO.Parcel parcel = null;
            bool pickedUp = false;
            Customer customer = null;
            int id = 0;

            void initDeliverry(int id)
            {
                parcel = bl.ParcelDisplay(id);
                batteryUsage = (int)parcel.Weight;
                pickedUp = parcel.PickedUp is not null;
                customer = bl.CustomerDisplay((int)(pickedUp ? parcel.Target.Id : parcel.Sender.Id));
            }

            do
            {
                switch (drone.Status)
                {
                    case DroneStatuses.Available:
                        {
                            if (!sleepDelayTime()) break;
                            lock (bl)
                            {
                                parcelId = bl.GetListParcel().Where(p => p?.Affiliation == null
                                && (WeightCategories)(p.Weight) <= drone.MaxWeight
                                && bl.RequiredBattery((int)p?.Id) < drone.Battery).OrderByDescending(p => p.Priority).ThenByDescending(p => p.Weight).FirstOrDefault().Id;
                            }
                        }
                        

                    case DroneStatuses.Charging:
                        break;
                    case DroneStatuses.Delivery:
                        break;
                   
                    default:
                        break;
                }

            } while ();





        }

        private bool sleepDelayTime()
        {
            throw new NotImplementedException();
        }
    }
}
