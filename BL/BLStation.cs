using System;
using System.Collections.Generic;
using System.Linq;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        /// <summary>
        /// Adding a station to our data source
        /// </summary>
        /// <param name="newStation"> IBL BO type station </param>
        public void AddStation(Station newStation)
        {
            IDAL.DO.Station temp = new IDAL.DO.Station();
            if (newStation.location.Lattitude > 90 || newStation.location.Lattitude < -90)
            {
                throw new IllegalActionException("Invalid lattitude value");
            } 
            if (newStation.location.Longitude > 180 || newStation.location.Longitude < -180)
            {
                throw new IllegalActionException("Invalid longitude value");
            }
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
        /// <summary>
        /// Retrieving the closest station to the drone
        /// </summary>
        /// <param name="drone"> drone which we are looking for closest station to</param>
        /// <returns> closest station </returns>
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
        /// <summary>
        /// Returns our list of stations
        /// </summary>
        /// <returns> IEnumerable of stations</returns>
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
        /// <summary>
        /// Returns the stations that have an open charge slot
        /// </summary>
        /// <returns> IEnumerable of stations </returns>
        public IEnumerable<Station> GetListStationsWithOpenSlots()
        {
            IEnumerable<IDAL.DO.Station> stations = dal.ListStationsWithOpenSlots();
            return (IEnumerable<Station>)stations;
        }
        /// <summary>
        /// Retrieving info to transform station into station to list
        /// </summary>
        /// <param name="objStation"> Station that we will recieve rest of its info</param>
        /// <returns> stationto list after we recieved the necessary info </returns>
        public StationToList MakeStationToList(Station objStation)
        {
            StationToList stationToList = new StationToList();
            stationToList.Id = objStation.Id;
            stationToList.Name = objStation.Name;
            stationToList.AvailableChargeSlots = objStation.AvailableChargeSlots;
            stationToList.UsedChargeSlots = objStation.droneInCharging.Count;
            return stationToList;
        }
        /// <summary>
        /// Returning a Station according to ID in order to be displayed
        /// </summary>
        /// <param name="id"> Id number </param>
        /// <returns> Station to be displayed </returns>
        public Station BaseStationDisplay(int id)
        {
            try
            {
                IDAL.DO.Station station = dal.GetStation(id);
                IEnumerable<IDAL.DO.DroneCharge> droneCharge = dal.ListDroneCharge();
                List<DroneInCharging> lstDrnInChrg = new List<DroneInCharging>();
                Station temp = new Station();
                Location location = new Location();
                location.Longitude = station.Longitude;
                location.Lattitude = station.Lattitude;
                temp.Id = station.Id;
                temp.Name = station.Name;
                temp.location = location;
                temp.AvailableChargeSlots = station.AvailableChargeSlots;
                List<IDAL.DO.DroneCharge> asList = droneCharge.ToList();
                foreach (var drnChrg in droneCharge)
                {
                    if (drnChrg.StationId == id)
                    {
                        DroneInCharging DrnInChrg = new DroneInCharging();
                        DrnInChrg.DroneId = drnChrg.DroneId;
                        DrnInChrg.Battery = GetDroneFromLstDrone(drnChrg.DroneId).Battery;
                        lstDrnInChrg.Add(DrnInChrg);
                    }
                }
                temp.droneInCharging = lstDrnInChrg;
                return temp;
            }
            catch (Exception e)
            {
                throw new ItemNotFoundException(id, "Enter an existing station in the system", e);
            }
        }
        /// <summary>
        /// Updating the amount of available slots the station has
        /// </summary>
        /// <param name="stationId"> ID of Station </param>
        /// <param name="chargingPositions"> Amount of available charge slots </param>
        public void UpdateStationPositions(int stationId, int chargingPositions)
        {
            try
            {
                List<IDAL.DO.Station> Stations = dal.ListBaseStation().ToList();
                IDAL.DO.Station station = new IDAL.DO.Station { Id =stationId,AvailableChargeSlots = chargingPositions};
                dal.UpdateStation(station);
            }
            catch (Exception e)
            {
                throw new IllegalActionException("Enter the correct number of charging points", e);
            }
        }
        /// <summary>
        /// Updating the stations name
        /// </summary>
        /// <param name="stationId"> Station ID</param>
        /// <param name="stationName"> The name that the station will be changed to </param>
        public void UpdateStationName(int stationId, string stationName)
        {
            try
            {
                List<IDAL.DO.Station> stations = dal.ListBaseStation().ToList();
                IDAL.DO.Station station = new IDAL.DO.Station { Id = stationId,Name = stationName };
                dal.UpdateStation(station);
            }
            catch (Exception e)
            {
                throw new IllegalActionException("Enter the correct number of charging points", e);
            }
        }
    }
}
