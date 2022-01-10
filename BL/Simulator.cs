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
using static System.Math;
using DalApi;

namespace BL
{
    class Simulator
    {
        private Stopwatch stopWatch;
        private const double speedDrone = 1.5;
        private const int delay = 500;
        private const double time = delay / 1000.0;
        private const double step = speedDrone / time;
        enum Maintenance { Starting,Going,Charging};
        DroneToList drone;
        BlApi.IBL bl;

        internal Simulator(BlApi.IBL BL, int droneId, Action action, Func<bool> checkStop)
        {
            bl = BL;
            var dal= DalFactory.GetDal("DalXml");
            drone = bl.GetListDrone().FirstOrDefault(d=>d.Id==droneId);
            int parcelId = 0;
            int satationId = 0;
            Station station = null;
            double distance = 0.0;
            int batteryUsage = 0;
            BO.Parcel parcel = null;
            bool pickedUp = false;
            Customer customer = null;
            int id = 0;
            Maintenance maintenance = Maintenance.Starting;
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
                                && bl.RequiredBattery((int)p?.Id,drone) < drone.Battery).OrderByDescending(p => p.Priority).ThenByDescending(p => p.Weight).FirstOrDefault().Id;
                                switch (parcelId,drone.Battery)
                                {
                                    case (0,10):
                                        break;
                                    case (0,_):
                                        satationId= bl.GetClosestStation(drone).Id;
                                        if (satationId!=0)
                                        {
                                            drone.Status = DroneStatuses.Charging;
                                            maintenance = Maintenance.Starting;
                                            bl.SendingDroneForCharging(drone.Id);
                                        }
                                        break;
                                    case (_, _):
                                        try
                                        {
                                            dal.Affiliate(parcelId, drone.Id);
                                            drone.ParcelBeingPassedId= parcelId;
                                            initDeliverry(parcelId);
                                            drone.Status = DroneStatuses.Delivery;
                                        }
                                        catch (DO.IllegalActionException exception)
                                        {
                                            ///לטפל בחריגה פה
                                            break;
                                        }
                                        break;
                                }
                            }
                            break;
                        }
                    case DroneStatuses.Charging:
                        switch(maintenance)
                        {
                            case Maintenance.Starting:
                                lock(bl)
                                {
                                    try { station = bl.BaseStationDisplay(satationId);}
                                    catch { }
                                    distance = Distance(drone, station.location);
                                    maintenance = Maintenance.Going;
                                }
                                break;
                            case Maintenance.Going:
                                if(distance<0.01)
                                {
                                    lock (bl)
                                    {
                                        drone.Location = station.location;
                                        maintenance = Maintenance.Charging;
                                    }
                                }
                                else
                                {
                                    if (!sleepDelayTime()) break;
                                    lock(bl)
                                    {
                                        double difference = distance < step ? distance : step;
                                        distance -= difference;
                                        drone.Battery = Max(0.0, drone.Battery - difference * dal.GetElectricUsage()[0]);
                                    }
                                }
                                break;
                            case Maintenance.Charging:
                                if (drone.Battery==100)
                                {
                                    lock(bl)
                                    {
                                        drone.Status = DroneStatuses.Available;
                                        bl.ReleaseDroneFromCharging(droneId);
                                    }
                                }
                                else
                                {
                                    if (!sleepDelayTime()) break;
                                    {
                                        lock (bl)
                                            drone.Battery = Min(100, drone.Battery + dal.GetElectricUsage()[4] * time);
                                    }
                                }
                                break;
                            default:
                                //זריקת חריגה
                                break;
                        }
                        break;
                    case DroneStatuses.Delivery:
                        lock(bl)
                        {
                            try
                            {
                                if (parcelId == 0) initDeliverry((int)drone.ParcelBeingPassedId);
                            }
                            catch (Exception exception)
                            {
                                throw;
                            }
                            distance= Distance(drone, customer.Location);
                        }
                        if(distance<0.01 ||drone.Battery==0)
                        {
                            lock(bl)
                            {
                                drone.Location = customer.Location;
                                if (pickedUp)
                                {
                                    dal.SupplyParcelUpdate(parcelId);
                                    drone.Status = DroneStatuses.Available;
                                }
                                else
                                {
                                    dal.PickupParcelUpdate(parcelId);
                                    customer = bl.CustomerDisplay(parcel.Target.Id);
                                    pickedUp = true;
                                }
                            }
                        }
                        else
                        {
                            if (!sleepDelayTime()) break;
                            lock(bl)
                            {
                                double difference = distance < step ? distance : step;
                                double p = difference / distance;
                                drone.Battery = Max(0, drone.Battery - difference * dal.GetElectricUsage()[pickedUp ? (int)drone.MaxWeight : 0]);
                                double Longitude = drone.Location.Lattitude + (customer.Location.Lattitude - drone.Location.Lattitude) * p;
                                double Lattitude = drone.Location.Longitude + (customer.Location.Longitude - drone.Location.Longitude) * p;
                                drone.Location = new() { Lattitude = Lattitude, Longitude = Longitude };
                            }
                        }
                        break;
                    default:
                        break;
                }
                action();
            } while (!checkStop());
        }

        private double Distance(DroneToList drone, Location location)
        {
             return bl.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude, location.Lattitude, location.Longitude);
        }

        private static bool sleepDelayTime()
        {
            try
            {
                Thread.Sleep(delay);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
