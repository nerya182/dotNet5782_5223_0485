using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using BO;
using DalApi;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL : IBL
    {
        static readonly IBL instance = new BL();
        public static IBL Instance { get => instance; }
        public List<DroneToList> listDrone = new List<DroneToList>();
        public DalApi.IDal dal { get; set; }
        public double AvailbleElec { get; set; }
        public double LightElec { get; set; }
        public double IntermeduateElec { get; set; }
        public double HeavyElec { get; set; }
        public double ChargeSpeed { get; set; }

        /// <summary>
        /// BL Ctor
        /// </summary> 
        internal BL()
        {

            Random R = new Random();
            dal = DalFactory.GetDal("DalXml");
            List<double> elecUsage = dal.GetElectricUsage();
            AvailbleElec = elecUsage[0];
            LightElec = elecUsage[1];
            IntermeduateElec = elecUsage[2];
            HeavyElec = elecUsage[3];
            ChargeSpeed = elecUsage[4];
            List<DO.Parcel> dalParcels = dal.ListParcel(i => true).ToList();
            List<DO.Drone> dalDrones = dal.ListDrone(i => true).ToList();
            for (int i = 0; i < dalDrones.Count; i++)
            {
                DroneToList droneToList = new DroneToList { Id = dalDrones[i].Id, Model = dalDrones[i].Model, MaxWeight = (WeightCategories)dalDrones[i].MaxWeight };
                listDrone.Add(droneToList);
            }
            foreach (DO.Parcel parcel in dal.ListParcel(i => true))
            {
                if (parcel.Delivered == null && parcel.DroneId != 0)
                {
                    for (int i = 0; i < listDrone.Count; i++)
                    {
                        if (listDrone[i].Id == parcel.DroneId)
                        {
                            listDrone[i].ParcelBeingPassedId = parcel.Id;
                            listDrone[i].Status = DroneStatuses.Delivery;
                            DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
                            DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
                            if (parcel.PickedUp == null)
                            {
                                DO.Station stationCloset = GetClosestCustomer(customerSender);
                                Location locationUpdate = new Location();
                                locationUpdate.Lattitude = stationCloset.Lattitude;
                                locationUpdate.Longitude = stationCloset.Longitude;
                                listDrone[i].Location = locationUpdate;
                            }
                            else
                            {
                                if (parcel.Delivered == null)
                                {
                                    Location locationUpdate = new Location();
                                    locationUpdate.Lattitude = customerSender.Lattitude;
                                    locationUpdate.Longitude = customerSender.Longitude;
                                    listDrone[i].Location = locationUpdate;
                                }
                            }
                            listDrone[i].Battery =
                                R.Next(
                                    (int)GetMinimumBatteryToShip(listDrone[i], customerSender, customerTarget,
                                        parcel.Weight), 100);
                        }
                    }
                }
            }
            foreach (DroneToList drone in listDrone)
            {
                if (drone.Status != DroneStatuses.Delivery)
                {
                    DO.DroneCharge droneInCharging = dal.ListDroneCharge().FirstOrDefault(d => d.DroneId == drone.Id);
                    if (droneInCharging.DroneId != 0)
                    {
                        drone.Status = DroneStatuses.Charging;
                        drone.ParcelBeingPassedId = 0;
                        Location locationDrone = new Location
                        { Lattitude = GetListStation().First(s => s.Id == droneInCharging.StationId).location.Lattitude, Longitude = GetListStation().FirstOrDefault(s => s.Id == droneInCharging.StationId).location.Longitude };
                        drone.Location = locationDrone;
                        drone.Battery = R.Next(0, 21);
                    }
                    else
                    {
                        drone.Status = (DroneStatuses)R.Next(1, 3);
                        drone.ParcelBeingPassedId = 0;
                        if (drone.Status == DroneStatuses.Charging)
                        {
                            DO.DroneCharge droneCharge = new DO.DroneCharge();
                            List<DO.Station> stations = dal.ListStationsWithOpenSlots().ToList();
                            DO.Station station = stations[R.Next(0, stations.Count)];
                            Location locationDrone = new Location
                            { Lattitude = station.Lattitude, Longitude = station.Longitude };
                            drone.Location = locationDrone;
                            drone.Battery = R.Next(0, 21);
                            droneCharge.StationId = station.Id;
                            droneCharge.DroneId = drone.Id;
                            droneCharge.EntryTime = DateTime.Now;
                            dal.AddDroneToCharge(droneCharge);
                        }

                        if (drone.Status == DroneStatuses.Available)
                        {
                            List<DO.Parcel> parcels = dal.ListParcel(i => i.Delivered != null).ToList();
                            DO.Parcel parcel = parcels.ElementAt(R.Next(0, parcels.Count));
                            DO.Customer customer = dal.GetCustomer(parcel.TargetId);
                            Location locationCustomer = new Location { Lattitude = customer.Lattitude, Longitude = customer.Longitude };
                            drone.Location = locationCustomer;
                            DO.Station station = GetClosestStation(drone);
                            double distance = (int)dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude,
                                drone.Location.Longitude, customer.Lattitude, customer.Longitude);
                            drone.Battery = R.Next((int)(distance/AvailbleElec) + 1, 101);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Adding a drone
        /// </summary>
        /// <param name="newDrone"> Drone to be added </param>
        /// <param name="chargingStationId"> number of station we wish to charge the drone </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(DroneToList newDrone, int chargingStationId)
        {
            lock (dal)
            {
                DO.Drone temp = new DO.Drone();
                temp.Id = newDrone.Id;
                temp.Model = newDrone.Model;
                temp.MaxWeight = (DO.WeightCategories)newDrone.MaxWeight;
                Random r = new Random();
                newDrone.Battery = r.Next(20, 41);
                newDrone.Status = DroneStatuses.Charging;
                newDrone.ParcelBeingPassedId = 0;
                DO.Station chargingStation = dal.ListBaseStation(i => true).ToList().Find(i => i.Id == chargingStationId);
                if (chargingStation.Id != 0)
                {
                    Location laLocationOfNewDrone = new Location()
                    { Lattitude = chargingStation.Longitude, Longitude = chargingStation.Longitude };
                    newDrone.Location = laLocationOfNewDrone;
                    DO.DroneCharge droneToCharge = new DO.DroneCharge()
                    { StationId = chargingStation.Id, DroneId = newDrone.Id, EntryTime = DateTime.Now };
                    dal.AddDroneToCharge(droneToCharge);
                    try
                    {
                        dal.AddDrone(temp);
                        listDrone.Add(newDrone);
                    }
                    catch (Exception e)
                    {
                        throw new ItemAlreadyExistsException(temp.Id, "Enter a new drone number", e);
                    }
                }
                else
                {
                    throw new ItemNotFoundException(chargingStationId,
                        "Enter an existing station number in the system for initial charging of the drone\n");
                }
            }
        }
        /// <summary>
        /// Updating a drone's model
        /// </summary>
        /// <param name="newDrone"> Drone to be updated </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(DroneToList newDrone)
        {
            lock (dal)
            {
                try
                {
                    DO.Drone updateDrone = new DO.Drone { Id = newDrone.Id, Model = newDrone.Model };
                    dal.UpdateDrone(updateDrone);
                    foreach (var drone in listDrone)
                    {
                        if (drone.Id == newDrone.Id)
                        {
                            drone.Model = newDrone.Model;
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ItemNotFoundException(newDrone.Id, "Enter an existing drone number in the system", e);
                }
            }
        }
        /// <summary>
        /// Returning Drone according to ID in order to be displayed
        /// </summary>
        /// <param name="id"> drone ID</param>
        /// <returns> Drone to be displayed </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DroneDisplay(int id)
        {
            lock (dal)
            {
                try
                {
                    IEnumerable<DO.Parcel> parcels = dal.ListParcel(i => true);
                    ParcelTransfer prclTrnsfr = new ParcelTransfer();
                    Location tempLocationSupply = new Location();
                    Location tempLocationCollect = new Location();
                    CustomerInParcel sender = new CustomerInParcel();
                    CustomerInParcel receiver = new CustomerInParcel();
                    Drone temp = new Drone();
                    DroneToList drone = GetDroneFromLstDrone(id);
                    temp.Id = drone.Id;
                    temp.Model = drone.Model;
                    temp.MaxWeight = drone.MaxWeight;
                    temp.Battery = drone.Battery;
                    temp.Status = drone.Status;
                    temp.Location = drone.Location;
                    foreach (var parcel in parcels)
                    {
                        if (parcel.DroneId == id && drone.ParcelBeingPassedId == parcel.Id)
                        {
                            prclTrnsfr.Id = parcel.Id;
                            prclTrnsfr.Weight = (WeightCategories)parcel.Weight;
                            prclTrnsfr.Priority = (Priorities)parcel.Priority;
                            if (parcel.PickedUp != null)
                                prclTrnsfr.ParcelSituation = true;
                            else
                                prclTrnsfr.ParcelSituation = false;
                            DO.Customer target1 = dal.GetCustomer(parcel.TargetId);
                            tempLocationSupply.Lattitude = target1.Lattitude;
                            tempLocationSupply.Longitude = target1.Longitude;
                            prclTrnsfr.SupplyPoint = tempLocationSupply;
                            DO.Customer sender1 = dal.GetCustomer(parcel.SenderId);
                            tempLocationCollect.Lattitude = sender1.Lattitude;
                            tempLocationCollect.Longitude = sender1.Longitude;
                            prclTrnsfr.collection = tempLocationCollect;
                            prclTrnsfr.distanceTransportation = dal.GetDistanceFromLatLonInKm(
                                tempLocationSupply.Lattitude,
                                tempLocationSupply.Longitude, tempLocationCollect.Lattitude,
                                tempLocationCollect.Longitude);
                            sender.Id = sender1.Id;
                            sender.Name = sender1.Name;
                            prclTrnsfr.Sender = sender;
                            receiver.Id = target1.Id;
                            receiver.Name = target1.Name;
                            prclTrnsfr.Receiver = receiver;
                        }
                    }
                    temp.ParcelTransfer = prclTrnsfr;
                    return temp;
                }
                catch (Exception e)
                {
                    throw new ItemNotFoundException(id, "Enter an existing drone in the system", e); ;
                }
            }
        }
        /// <summary>
        /// Retrieving the list of drones
        /// </summary>
        /// <returns> UEnumerable of drones </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetListDrone()
        {
            lock (dal)
            {
                List<DroneToList> temp = new List<DroneToList>();
                foreach (DroneToList drone in listDrone)
                {
                    Drone obj = DroneDisplay(drone.Id);
                    temp.Add(MakeDroneToList(obj));
                }
                return temp;
            }
        }
        /// <summary>
        /// Retrieving information to transform drone into dronetolist and returning it
        /// </summary>
        /// <param name="objDrone"> drone to be transformed to dronetolist</param>
        /// <returns> dronetolist after we've found necessary info</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneToList MakeDroneToList(Drone objDrone)
        {
            DroneToList droneToList = new DroneToList();
            droneToList.Id = objDrone.Id;
            droneToList.Location = objDrone.Location;
            droneToList.Battery = objDrone.Battery;
            droneToList.Model = objDrone.Model;
            droneToList.MaxWeight = objDrone.MaxWeight;
            droneToList.Status = objDrone.Status;
            if (objDrone.Status != DroneStatuses.Delivery)
                droneToList.ParcelBeingPassedId = 0;
            else
                droneToList.ParcelBeingPassedId = objDrone.ParcelTransfer.Id;
            return droneToList;
        }
        /// <summary>
        /// Drone delivering the parcel that answers the criterias that were given to us
        /// </summary>
        /// <param name="droneId"> Drone ID</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveryOfParcelByDrone(int droneId)
        {
            lock (dal)
            {
                DroneToList drone = listDrone.Find(i => i.Id == droneId);
                if (drone.Status != DroneStatuses.Delivery)
                {
                    throw new IllegalActionException("The drone is not in delivery mode");
                }

                DO.Parcel parcel = dal.GetParcel(drone.ParcelBeingPassedId);
                DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
                if (parcel.DroneId == droneId && parcel.PickedUp != null && parcel.Delivered == null)
                {
                    listDrone.Remove(drone);
                    double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                        customerTarget.Lattitude, customerTarget.Longitude);
                    drone.Battery -= distance/dal.GetElectricUsageNumber(parcel.Weight);
                    drone.Location.Lattitude = customerTarget.Lattitude;
                    drone.Location.Longitude = customerTarget.Longitude;
                    drone.Status = DroneStatuses.Available;
                    drone.ParcelBeingPassedId = 0;
                    dal.SupplyParcelUpdate(parcel.Id);
                    listDrone.Add(drone);
                }
                else
                {
                    throw new IllegalActionException("The parcel was not collected by the drone\n");
                }
            }
        }
        /// <summary>
        /// Drone picking up the parcel he was assigned to
        /// </summary>
        /// <param name="droneId"> Drone ID</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelCollectionByDrone(int droneId)
        {
            lock (dal)
            {
                DroneToList drone = listDrone.Find(i => i.Id == droneId);
                if (drone == null)
                {
                    throw new ItemNotFoundException(droneId, "Enter an existing skimmer number in the system");
                }

                if (drone.Status != DroneStatuses.Delivery)
                {
                    throw new IllegalActionException("The drone is not in delivery mode");
                }

                DO.Parcel parcel = dal.GetParcel(drone.ParcelBeingPassedId);
                DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
                if (parcel.Affiliation != null && parcel.PickedUp == null && parcel.DroneId == drone.Id && drone.Status == DroneStatuses.Delivery)
                {
                    listDrone.Remove(drone);
                    double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                        customerSender.Lattitude, customerSender.Longitude);

                    drone.Battery -= distance/dal.GetElectricUsage()[0];
                    drone.Location.Lattitude = customerSender.Lattitude;
                    drone.Location.Longitude = customerSender.Longitude;

                    dal.PickupParcelUpdate(parcel.Id);
                    listDrone.Add(drone);
                }
                else
                {
                    throw new IllegalActionException("The drone is not  not associated with this parcel");
                }
            }
        }
        /// <summary>
        /// Affiliating drone with the parcel that answers the criterias that were given to us
        /// </summary>
        /// <param name="droneId"> Drone ID</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AffiliateParcelToDrone(int droneId)
        {
            lock (dal)
            {
                bool flag = false;
                DroneToList drone = listDrone.Find(i => i.Id == droneId);
                //DO.Drone d = dal.ListDrone(i => true).ToList().Find(i => i.Id == droneId);
                if (drone == null)
                {
                    throw new ItemNotFoundException(droneId, "Enter an existing skimmer number in the system");
                }
                if (drone.Status == DroneStatuses.Charging || drone.Status == DroneStatuses.Delivery)
                {
                    throw new IllegalActionException("The drone is in charge / delivery mode");
                }
                List<DO.Parcel> parcels = dal.ListParcel(i => true).ToList();
                parcels = parcels.FindAll(p => p.DroneId == 0 && p.Affiliation == null && p.Weight <= (DO.WeightCategories)drone.MaxWeight);
                if (parcels.Count == 0)
                {
                    throw new IllegalActionException("There are no packages waiting to be shipped");
                }
                do
                {
                    List<DO.Parcel> parcelsFiltered = new List<DO.Parcel>();
                    parcelsFiltered = parcels.FindAll(i => i.Priority == DO.Priorities.Urgent);
                    if (parcelsFiltered.Count == 0) { parcelsFiltered = parcels.FindAll(i => i.Priority == DO.Priorities.Express); }
                    if (parcelsFiltered.Count == 0) { parcelsFiltered = parcels.FindAll(i => i.Priority == DO.Priorities.Regular); }

                    List<DO.Parcel> helpParcelsFiltered = parcelsFiltered;

                    parcelsFiltered = helpParcelsFiltered.FindAll(i => i.Weight == (DO.WeightCategories)drone.MaxWeight);
                    if (parcelsFiltered.Count == 0)
                        parcelsFiltered = helpParcelsFiltered.FindAll(i => i.Weight == (DO.WeightCategories)drone.MaxWeight - 1 && (drone.MaxWeight - 1) > 0);
                    if (parcelsFiltered.Count == 0)
                        parcelsFiltered = helpParcelsFiltered.FindAll(i => i.Weight == (DO.WeightCategories)drone.MaxWeight - 2 && (drone.MaxWeight - 2) > 0);

                    DO.Parcel parcel = GetClosestParcel(parcelsFiltered, drone);
                    DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
                    DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
                    double minimumBattery =
                        GetMinimumBatteryToShip(drone, customerSender, customerTarget, parcel.Weight);

                    if (drone.Battery > minimumBattery)
                    {
                        listDrone.Remove(drone);
                        drone.Status = DroneStatuses.Delivery;
                        dal.Affiliate(parcel.Id, drone.Id);
                        drone.ParcelBeingPassedId = parcel.Id;
                        listDrone.Add(drone);
                        flag = true;
                    }
                    parcels.Remove(parcel);

                } while (!flag && parcels.Count > 0);

                if (parcels.Count == 0)
                {
                    drone.Status = DroneStatuses.Available;
                    throw new IllegalActionException("There is not enough battery for the drone to make the shipment");
                }
            }
        }
        /// <summary>
        /// Returning the amount of battery needed in order to make the shipment
        /// </summary>
        /// <param name="drone"> Drone that needs to make the shipment</param>
        /// <param name="customerSender"> The sender</param>
        /// <param name="customerTarget"> The target</param>
        /// <param name="weight"> Weight the Drone can carry</param>
        /// <returns></returns>
        private double GetMinimumBatteryToShip(DroneToList drone, DO.Customer customerSender, DO.Customer customerTarget, DO.WeightCategories weight)
        {
            lock (dal)
            {
                double distance1 = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,//distance to send
                customerSender.Lattitude, customerSender.Longitude);
                double distance2 = dal.GetDistanceFromLatLonInKm(customerSender.Lattitude, customerSender.Longitude,//distance to target
                    customerTarget.Lattitude, customerTarget.Longitude);
                DroneToList droneInTarget = new DroneToList();
                Location locationDroneInTarget = new Location { Lattitude = customerTarget.Lattitude, Longitude = customerTarget.Longitude };
                droneInTarget.Location = locationDroneInTarget;
                double distance3 = dal.GetDistanceFromLatLonInKm(GetClosestStation(droneInTarget).Lattitude,// distance target to closest station
                    GetClosestStation(droneInTarget).Longitude, customerTarget.Lattitude, customerTarget.Longitude);
                return (distance2/GetElectricUsageNumber(weight) + distance1/AvailbleElec +
                        distance3/AvailbleElec);
            }
        }
        /// <summary>
        /// Releasing drone from charging according to ID
        /// </summary>
        /// <param name="droneId"> Drone ID</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromCharging(int droneId)
        {
            lock (dal)
            {
                DateTime? timeOfCharging = null;
                DroneToList drone = listDrone.Find(i => i.Id == droneId);
                if (drone == null)
                {
                    throw new ItemNotFoundException(droneId, "Enter an existing drone number in the system");
                }
                if (drone.Status != DroneStatuses.Charging)
                {
                    throw new IllegalActionException("The drone is not charging");
                }
                timeOfCharging = dal.ListDroneCharge().ToList().Find(i => i.DroneId == droneId).EntryTime;
                dal.ReleaseDroneFromCharger(droneId);
                TimeSpan? time = DateTime.Now - timeOfCharging;
                drone.Battery += (int)dal.GetChargeSpeed() * ((time.Value.Hours));
                drone.Status = DroneStatuses.Available;
                if (drone.Battery > 100)
                {
                    drone.Battery = 100;
                }
            }
        }
        /// <summary>
        /// Charging a drone according to ID
        /// </summary>
        /// <param name="droneId"> Drone ID</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendingDroneForCharging(int droneId)
        {
            lock (dal)
            {
                DroneToList drone = listDrone.Find(i => i.Id == droneId);
                if (drone == null)
                {
                    throw new ItemNotFoundException(droneId, "Enter an existing skimmer number in the system");
                }
                if (drone.Status == DroneStatuses.Charging)
                {
                    throw new IllegalActionException("The drone is already charging");
                }

                if (drone.Status == DroneStatuses.Delivery)
                {
                    throw new IllegalActionException("The drone is not available");
                }
                DO.Station closestStation = GetClosestStation(drone);
                double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                    closestStation.Lattitude, closestStation.Longitude);

                if (closestStation.AvailableChargeSlots > 0 && drone.Battery > distance * dal.GetElectricUsage()[0])
                {
                    DO.DroneCharge droneCharge = new()
                    { DroneId = drone.Id, StationId = closestStation.Id, EntryTime = DateTime.Now };
                    dal.AddDroneToCharge(droneCharge);
                    drone.Battery += distance/dal.GetElectricUsage()[0];
                    drone.Location.Lattitude = closestStation.Lattitude;
                    drone.Location.Longitude = closestStation.Longitude;
                    drone.Status = DroneStatuses.Charging;
                }
                else
                {
                    throw new IllegalActionException(
                        "There are no free charging stations at this station or there is not enough battery\n");
                }
            }
        }

        /// <summary>
        /// Recieving a dronetolist
        /// </summary>
        /// <param name="id"> Drone ID</param>
        /// <returns> Dronetolist we we're looking for</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneToList GetDroneFromLstDrone(int id)
        {
            bool flag = false;
            foreach (DroneToList drone in listDrone)
            {
                if (drone.Id == id)
                {
                    flag = true;
                    return drone;
                }
            }
            if (!flag)
                throw new ItemNotFoundException(id, "ERROR :id of drone not found\n");
            return null;
        }
        /// <summary>
        /// Retrieving the list of drones
        /// </summary>
        /// <returns> UEnumerable of drones </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones()
        {
            lock (dal)
            {
                List<Drone> temp = new List<Drone>();
                foreach (DO.Drone drone in dal.ListDrone(i => true))
                {
                    Drone obj = DroneDisplay(drone.Id);
                    temp.Add(obj);
                }
                return temp;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetByStatus(IEnumerable itemsSource, DroneStatuses selectedStatus)
        {
            List<DroneToList> temp = new List<DroneToList>();
            foreach (DroneToList drone in itemsSource)
            {
                if (drone.Status == selectedStatus)
                {
                    temp.Add(drone);
                }
            }
            return temp;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetByWeight(IEnumerable itemsSource, WeightCategories selectedWeight)
        {
            List<DroneToList> temp = new List<DroneToList>();
            foreach (DroneToList drone in itemsSource)
            {
                if (drone.MaxWeight == selectedWeight)
                {
                    temp.Add(drone);
                }
            }
            return temp;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<IGrouping<DroneStatuses, DroneToList>> GroupingStatus()
        {
            lock (dal)
            {
                return listDrone.GroupBy(d => d.Status);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<IGrouping<WeightCategories, DroneToList>> GroupingWeight()
        {
            lock (dal)
            {
                return listDrone.GroupBy(d => d.MaxWeight);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            lock(dal)
            { return dal.GetDistanceFromLatLonInKm(lat1, lon1, lat2, lon2); }
           
        }
        public void StartSimulator(int id, Action update, Func<bool> checkStop) => new Simulator(this, id, update, checkStop);
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double RequiredBattery(int id, Drone drone)
        {
            lock(dal)
            {
                DO.Parcel parcel = dal.ListParcel(p => p.Id == id).FirstOrDefault();
                DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
                DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
                DroneToList droneToList = MakeDroneToList(drone);
                return GetMinimumBatteryToShip(droneToList, customerSender, customerTarget, parcel.Weight);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<double> GetElectricUsage() 
        {
            lock (dal) { return dal.GetElectricUsage(); }
            }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void updateDrone(int droneId, int timerCheck)
        {
            lock (dal)
            {
                DroneToList drone = listDrone.Find(i => i.Id == droneId);
                Station stationId = GetListStation().FirstOrDefault(s => s.location.Lattitude == drone.Location.Lattitude);
                dal.ReleaseDroneFromCharger(droneId);
                listDrone.Remove(drone);
                drone.Battery += (timerCheck / 1000.0) * GetElectricUsage()[4];
                DO.DroneCharge droneCharge = new()
                { DroneId = drone.Id, StationId = stationId.Id, EntryTime = DateTime.Now };
                dal.AddDroneToCharge(droneCharge);
                listDrone.Add(drone);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void updateCollectionByDron(int droneId)
        {
            lock(dal)
            {
                DroneToList drone = listDrone.Find(i => i.Id == droneId);
                DO.Parcel parcel = dal.GetParcel(drone.ParcelBeingPassedId);
                DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
                if (parcel.Affiliation != null && parcel.PickedUp == null && parcel.DroneId == drone.Id && drone.Status == DroneStatuses.Delivery)
                {
                    dal.PickupParcelUpdate(parcel.Id);
                }
                else
                {
                    throw new IllegalActionException("The drone is not  not associated with this parcel");
                }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveryByDron(int droneId)
        {
            lock (dal)
            {
                DroneToList drone = listDrone.Find(i => i.Id == droneId);
                if (drone.Status != DroneStatuses.Delivery)
                {
                    throw new IllegalActionException("The drone is not in delivery mode");
                }
                DO.Parcel parcel = dal.GetParcel(drone.ParcelBeingPassedId);
                DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
                if (parcel.DroneId == droneId && parcel.PickedUp != null && parcel.Delivered == null)
                {
                    listDrone.Remove(drone);
                    double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                        customerTarget.Lattitude, customerTarget.Longitude);
                    drone.Status = DroneStatuses.Available;
                    drone.ParcelBeingPassedId = 0;
                    dal.SupplyParcelUpdate(parcel.Id);
                    listDrone.Add(drone);
                }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void updateReleaseDrone(int droneId)
        {
            lock(dal)
            {
                DroneToList drone = listDrone.Find(i => i.Id == droneId);
                if (drone == null)
                {
                    throw new ItemNotFoundException(droneId, "Enter an existing drone number in the system");
                }
                if (drone.Status != DroneStatuses.Charging)
                {
                    throw new IllegalActionException("The drone is not charging");
                }
                listDrone.Remove(drone);
                dal.ReleaseDroneFromCharger(droneId);
                drone.Status = DroneStatuses.Available;
                if (drone.Battery > 100)
                {
                    drone.Battery = 100;
                }
                listDrone.Add(drone);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Affiliate(int id, int? parcelId)
        {
            lock (dal)
            {
                bool flag = false;
                DroneToList drone = listDrone.Find(i => i.Id == id);
                if (drone == null)
                {
                    throw new ItemNotFoundException(id, "Enter an existing skimmer number in the system");
                }
                if (drone.Status == DroneStatuses.Charging || drone.Status == DroneStatuses.Delivery)
                {
                    throw new IllegalActionException("The drone is in charge / delivery mode");
                }

                Parcel parcel = GetListParcel().FirstOrDefault(p => p.Id == parcelId);
                DO.Customer customerSender = dal.GetCustomer(parcel.Sender.Id);
                DO.Customer customerTarget = dal.GetCustomer(parcel.Target.Id);
                double minimumBattery =
                    GetMinimumBatteryToShip(drone, customerSender, customerTarget, (DO.WeightCategories)parcel.Weight);

                if (drone.Battery > minimumBattery)
                {
                    listDrone.Remove(drone);
                    dal.Affiliate(parcel.Id, drone.Id);
                    drone.Status = DroneStatuses.Delivery;
                    drone.ParcelBeingPassedId = parcel.Id;
                    listDrone.Add(drone);

                }
            }
        
        }
    }   
        
}
