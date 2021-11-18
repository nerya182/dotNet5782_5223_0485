using System;
using System.Collections.Generic;
using System.Linq;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public void AddStation(Station newStation)
        {
            IDAL.DO.Station temp = new IDAL.DO.Station();
            try
            {
                temp.Id = newStation.Id;
                temp.Name = newStation.Name;
                temp.AvailableChargeSlots = newStation.AvailableChargeSlots;
                temp.Lattitude = newStation.location.Lattitude;
                temp.Longitude = newStation.location.Longitude;
                dal.AddStation(temp);
            }
            catch (Exception e)
            {
                throw new ItemAlreadyExistsException(temp.Id, "Enter a new station number\n", e);
            }
        }
        private IDAL.DO.Station GetClosestStation(DroneToList drone)
        {
            List<IDAL.DO.Station> stations = dal.ListBaseStation().ToList();
            Location closestStation = new Location();
            closestStation.Lattitude = stations[0].Lattitude;
            closestStation.Longitude = stations[0].Longitude;
            int i = 0, index = 0;
            foreach (var station in stations)
            {
                if (dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude, station.Lattitude, station.Longitude) <
                    dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude, closestStation.Lattitude, closestStation.Longitude) && station.AvailableChargeSlots > 0)
                {
                    index = i;
                    closestStation.Lattitude = drone.Location.Lattitude;
                    closestStation.Longitude = drone.Location.Longitude;
                }
                i++;
            }
            return stations[index];
        }

        public IEnumerable<Station> GetListStation()
        {
            IEnumerable<IDAL.DO.Station> stations = dal.ListBaseStation();
            List<Station> temp = new List<Station>();
            foreach (var station in stations)
            {
                Station obj = BaseStationDisplay(station.Id);
                temp.Add(obj);
            }
            return temp;
        }
        public IEnumerable<Station> GetListStationsWithOpenSlots()
        {
            IEnumerable<IDAL.DO.Station> stations = dal.ListStationsWithOpenSlots();
            return (IEnumerable<Station>)stations;
        }
        public StationToList MakeStationToList(Station objStation)
        {
            StationToList stationToList = new StationToList();
            stationToList.Id = objStation.Id;
            stationToList.Name = objStation.Name;
            stationToList.AvailableChargeSlots = objStation.AvailableChargeSlots;
            stationToList.UsedChargeSlots = objStation.droneInCharging.Count;
            return stationToList;
        }
        public Station BaseStationDisplay(int id)
        {
            IDAL.DO.Station station = dal.GetStation(id);
            IEnumerable<IDAL.DO.DroneCharge> droneCharge = dal.ListDroneCharge();
            List<DroneInCharging> lstDrnInChrg = new List<DroneInCharging>();
            DroneInCharging DrnInChrg = new DroneInCharging();
            Station temp = new Station();
            Location location = new Location();
            location.Longitude = station.Longitude;
            location.Lattitude = station.Lattitude;
            temp.Id = station.Id;
            temp.Name = station.Name;
            temp.location = location;
            temp.AvailableChargeSlots = station.AvailableChargeSlots;
            foreach (var drnChrg in droneCharge)
            {
                if (drnChrg.StationId == id)
                {
                    DrnInChrg.DroneId = drnChrg.DroneId;
                    DrnInChrg.Battery = GetDroneFromLstDrone(drnChrg.DroneId).Battery;
                    lstDrnInChrg.Add(DrnInChrg);
                }
            }

            temp.droneInCharging = lstDrnInChrg;
            return temp;
        }
        public void UpdateStation(Station updateStation, int chargingPositions)
        {
            List<IDAL.DO.Station> Stations = dal.ListBaseStation().ToList();
            bool flag = false;
            for (int i = 0; i < Stations.Count(); i++)
            {
                if (Stations[i].Id == updateStation.Id)
                {
                    IDAL.DO.Station s = Stations[i];
                    if (updateStation.Name != "no")
                    {
                        s.Name = updateStation.Name;
                    }

                    if (updateStation.AvailableChargeSlots != -1)
                    {
                        if (chargingPositions - dal.AvailableChargeSlotsInStation(updateStation.Id) < 0)
                        {
                            throw new IllegalActionException("The total amount of charging stations is invalid\n");
                        }
                        else
                        {
                            s.AvailableChargeSlots =
                                chargingPositions - dal.AvailableChargeSlotsInStation(updateStation.Id);
                        }
                    }
                    Stations[i] = s;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new ItemNotFoundException(updateStation.Id, "ERROR :id of station not found\n");
            }
        }
    }
}
