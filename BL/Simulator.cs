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
    internal class Simulator
    {
        const double DroneSpeed = 100;
        const int TimerCheck = 500;
        Drone drone;
        DroneToList droneToList;
        Parcel Parcel;
        Station stationCharge;
        int? parcelId=null;
        BlApi.IBL bl;
        DalApi.IDal dal;

        public Simulator(BlApi.IBL BL, int droneId, Action updateDrone, Func<bool> checkStop)
        {
            bl = BL;
            dal = bl.dal;
            while (!checkStop())
            {
                Thread.Sleep(TimerCheck);
                lock (bl)
                {
                    try { drone = bl.DroneDisplay(droneId); droneToList = bl.MakeDroneToList(drone); }
                    catch { return; }
                }
                switch(drone.Status)
                {
                    case DroneStatuses.Available:
                        Parcel? p = bl.GetListParcel().Where(p => p?.Affiliation == null
                                  && (WeightCategories)(p.Weight) <= drone.MaxWeight
                                  && bl.RequiredBattery((int)p?.Id, drone) < drone.Battery).OrderByDescending(p => p.Priority).ThenByDescending(p => p.Weight).FirstOrDefault();
                        if (p is null)
                        {
                            parcelId = 0;
                        }
                        else
                        {
                            parcelId = p.Id;
                        }
                        if (parcelId != 0 || drone.Battery > 95)
                        {
                            try { if(parcelId!=0)bl.Affiliate(drone.Id,parcelId); }
                            catch (BO.IllegalActionException ex)
                            {
                                updateDrone();
                                Thread.Sleep(3000);
                                continue;
                            }
                        }
                        else
                        {
                            try
                            {
                                int stationChargeId = bl.GetClosestStation(droneToList).Id;
                                stationCharge = bl.BaseStationDisplay(stationChargeId);
                                if (bl.GoTowards(droneId, stationCharge.location, DroneSpeed, bl.AvailbleElec) == stationCharge.location)
                                {
                                    bl.SendingDroneForCharging(droneId);
                                    Thread.Sleep(TimerCheck);
                                }
                            }
                            catch (BO.IllegalActionException ex)
                            {
                                updateDrone();
                                return;
                            }
                        }
                        break;
                    case DroneStatuses.Charging:
                        if (drone.Battery < 99) lock (bl) 
                        {
                                bl.updateDrone(droneId,TimerCheck);
                         }
                        else
                        {
                            lock (bl)
                            {
                                bl.updateReleaseDrone(droneId);
                                Thread.Sleep(TimerCheck);
                            }
                        }
                        break;
                    case DroneStatuses.Delivery:
                        if (drone.ParcelTransfer.Id!=0)
                        {
                            lock(bl)
                            {
                                Parcel = bl.ParcelDisplay(drone.ParcelTransfer.Id);
                                if (Parcel.PickedUp is null)
                                {
                                    lock (dal)
                                    {
                                        DO.Customer sender = dal.GetCustomer(Parcel.Sender.Id);
                                        Location senderL = new Location() { Lattitude = sender.Lattitude, Longitude =sender.Longitude };
                                        if (bl.GoTowards(droneId, senderL, DroneSpeed, bl.GetElectricUsage()[0])==senderL)
                                        {
                                            bl.updateCollectionByDron(droneId);
                                        }
                                    }
                                }
                                else if( Parcel.Delivered is  null)
                                {
                                    lock (dal)
                                    {
                                        DO.Customer target = dal.GetCustomer(Parcel.Target.Id);
                                        Location targetL = new Location() { Lattitude = target.Lattitude, Longitude = target.Longitude };
                                        if (bl.GoTowards(droneId, targetL, DroneSpeed, bl.GetElectricUsage()[(int)Parcel.Weight]) == targetL)
                                        {
                                            bl.DeliveryByDron(droneId);
                                            Thread.Sleep(TimerCheck);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
                updateDrone();
            }
        }
    }
}

