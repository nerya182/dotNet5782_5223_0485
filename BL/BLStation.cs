using System;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL : IBL
    {
        /// <summary>
        /// Adding a station to our data source
        /// </summary>
        /// <param name="newStation"> IBL BO type station </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station newStation)
        {
            lock(dal)
            {
                DO.Station temp = new DO.Station();
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
            
        }
        /// <summary>
        /// Retrieving the closest station to the drone
        /// </summary>
        /// <param name="drone"> drone which we are looking for closest station to</param>
        /// <returns> closest station </returns>
       public DO.Station GetClosestStation(DroneToList drone)
        {
            lock(dal)
            {
                List<DO.Station> stations = dal.ListBaseStation(i => true).ToList();
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
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetStations()
        {
            lock(dal)
            {
                List<StationToList> temp = new List<StationToList>();
                foreach (DO.Station station in dal.ListBaseStation(i => true))
                {
                    Station obj = BaseStationDisplay(station.Id);
                    temp.Add(MakeStationToList(obj));
                }
                return temp;
            }
            
        }

        /// <summary>
        /// Returns our list of stations
        /// </summary>
        /// <returns> IEnumerable of stations</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetListStation()
        {
            lock(dal)
            {
                List<Station> temp = new List<Station>();
                foreach (DO.Station station in dal.ListBaseStation(i => true))
                {
                    Station obj = BaseStationDisplay(station.Id);
                    temp.Add(obj);
                }
                return temp;
            }
            
        }
        /// <summary>
        /// Returns the stations that have an open charge slot
        /// </summary>
        /// <returns> IEnumerable of stations </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetListStationsWithOpenSlots()
        {
            lock(dal)
            {
                IEnumerable<DO.Station> stations = dal.ListStationsWithOpenSlots();
                return (IEnumerable<Station>)stations;
            }
            
        }
        /// <summary>
        /// Retrieving info to transform station into station to list
        /// </summary>
        /// <param name="objStation"> Station that we will recieve rest of its info</param>
        /// <returns> stationto list after we recieved the necessary info </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public StationToList MakeStationToList( Station objStation)
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station BaseStationDisplay(int id)
        {
            lock(dal)
            {
                try
                {
                    DO.Station station = dal.GetStation(id);
                    IEnumerable<DO.DroneCharge> droneCharge = dal.ListDroneCharge();
                    List<DroneInCharging> lstDrnInChrg = new List<DroneInCharging>();
                    Station temp = new Station();
                    Location location = new Location();
                    location.Longitude = station.Longitude;
                    location.Lattitude = station.Lattitude;
                    temp.Id = station.Id;
                    temp.Name = station.Name;
                    temp.location = location;
                    temp.AvailableChargeSlots = station.AvailableChargeSlots;
                    List<DO.DroneCharge> asList = droneCharge.ToList();
                    foreach (var (drnChrg, DrnInChrg) in from drnChrg in droneCharge
                                                         where drnChrg.StationId == id
                                                         let DrnInChrg = new DroneInCharging()
                                                         select (drnChrg, DrnInChrg))
                    {
                        DrnInChrg.DroneId = drnChrg.DroneId;
                        DrnInChrg.Battery = GetDroneFromLstDrone(drnChrg.DroneId).Battery;
                        lstDrnInChrg.Add(DrnInChrg);
                    }

                    temp.droneInCharging = lstDrnInChrg;
                    return temp;
                }
                catch (Exception e)
                {
                    throw new ItemNotFoundException(id, "Enter an existing station in the system", e);
                }
            }
            
        }
        /// <summary>
        /// Updating the amount of available slots the station has
        /// </summary>
        /// <param name="stationId"> ID of Station </param>
        /// <param name="chargingPositions"> Amount of available charge slots </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationPositions(int stationId, int chargingPositions)
        {
            lock(dal)
            {
                try
                {
                    List<DO.Station> Stations = dal.ListBaseStation(i => true).ToList();
                    DO.Station station = new DO.Station { Id = stationId, AvailableChargeSlots = chargingPositions };
                    dal.UpdateStation(station);
                }
                catch (Exception e)
                {
                    throw new IllegalActionException("Enter the correct number of charging points", e);
                }
            }
            
        }
        /// <summary>
        /// Updating the stations name
        /// </summary>
        /// <param name="stationId"> Station ID</param>
        /// <param name="stationName"> The name that the station will be changed to </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationName(int stationId, string stationName)
        {
            lock(dal)
            {
                try
                {
                    List<DO.Station> stations = dal.ListBaseStation(i => true).ToList();
                    DO.Station station = new DO.Station { Id = stationId, Name = stationName };
                    dal.UpdateStation(station);
                }
                catch (Exception e)
                {
                    throw new IllegalActionException("Enter the correct number of charging points", e);
                }
            }
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(StationToList station)
        {
            lock(dal)
            {
                if (station == null) { throw new IllegalActionException("Please click once on a station and then click delete"); }
                if (station.UsedChargeSlots == 0)
                    dal.DeleteStation(station.Id);
                else
                {
                    throw new IllegalActionException("It is not possible to delete the station because there are drones in charge");
                }
            }
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<IGrouping<bool,StationToList>> GroupingAvailableChargeSlots()
        {
            lock(dal)
            {
                var listStation = GetStations();
                return listStation.GroupBy(s => s.AvailableChargeSlots > 0);
            }
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<IGrouping<int, StationToList>> GroupingChargeSlots()
        {
            lock(dal)
            {
                return (IEnumerable<IGrouping<int, StationToList>>)GetStations().GroupBy(s => s.AvailableChargeSlots);
            }
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Location GoTowards(int DroneId, Location Destination, double speedInKm, double Elec)
        {
            DroneToList drone = listDrone.First(d => d.Id == DroneId);
            if (drone.Location.Lattitude == Destination.Lattitude && drone.Location.Longitude == Destination.Longitude)
            {
                return Destination;
            }
            double speedInCoords = speedInKm / 111;
            if (drone.Location == Destination)
            {
                return Destination;
            }
            double distanceInKM = GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude, Destination.Lattitude, Destination.Longitude);
            Location Vector = new Location() { Lattitude = Destination.Lattitude - drone.Location.Lattitude, Longitude = Destination.Longitude - drone.Location.Longitude };
            double vectorLengthInCoords = GetDistanceFromLatLonInKm(0, 0, Vector.Lattitude, Vector.Longitude);
            Vector = new Location() { Lattitude = Vector.Lattitude / vectorLengthInCoords, Longitude = Vector.Longitude / vectorLengthInCoords };
            listDrone.Remove(drone);

            if (vectorLengthInCoords <= speedInCoords)
            {
                drone.Battery -= (GetDistanceFromLatLonInKm(0, 0, Vector.Lattitude, Vector.Longitude) / Elec);
                drone.Location = Destination;
                listDrone.Add(drone);
                return Destination;
            }

             drone.Battery -= (speedInKm / Elec);
            drone.Location = new Location()
            {
                Longitude = drone.Location.Longitude + (Vector.Longitude * speedInCoords),
                Lattitude = drone.Location.Lattitude + (Vector.Lattitude * speedInCoords)
            };
            listDrone.Add(drone);
            return drone.Location;
        }
    }
}
