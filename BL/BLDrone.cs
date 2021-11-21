using System;
using System.Collections.Generic;
using System.Linq;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public List<DroneToList> listDrone = new List<DroneToList>();
        public IDal dal;
        public double AvailbleElec { get; set; }
        public double LightElec { get; set; }
        public double IntermeduateElec { get; set; }
        public double HeavyElec { get; set; }
        public double ChargePerHours { get; set; }
        public BL()
        {
            Random R = new Random();

            dal = new DalObject.DalObject();
            double[] elecUsage = dal.GetElectricUsage();
            AvailbleElec = elecUsage[0];
            LightElec = elecUsage[1];
            IntermeduateElec = elecUsage[2];
            HeavyElec = elecUsage[3];
            ChargePerHours = elecUsage[4];

            List<IDAL.DO.Parcel> dalParcels = dal.ListParcel().ToList();
            List<IDAL.DO.Drone> dalDrones = dal.ListDrone().ToList();

            for (int i = 0; i < dalDrones.Count; i++)
            {
                DroneToList droneToList = new DroneToList { Id = dalDrones[i].Id, Model = dalDrones[i].Model, MaxWeight = (WeightCategories)dalDrones[i].MaxWeight };
                listDrone.Add(droneToList);
            }

            foreach (IDAL.DO.Parcel parcel in dal.ListParcel())
            {
                if (parcel.Delivered == DateTime.MinValue && parcel.DroneId != 0)
                {
                    for (int i = 0; i < listDrone.Count; i++)
                    {
                        if (listDrone[i].Id == parcel.DroneId)
                        {
                            listDrone[i].ParcelBeingPassedId = parcel.Id;
                            listDrone[i].Status = DroneStatuses.Delivery;
                            IDAL.DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
                            IDAL.DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
                            if (parcel.PickedUp == DateTime.MinValue)//שוייך אבל לא נאסף
                            {
                                IDAL.DO.Station stationCloset = GetClosestCustomer(customerSender);
                                Location locationUpdate = new Location();
                                locationUpdate.Lattitude = stationCloset.Lattitude;
                                locationUpdate.Longitude = stationCloset.Longitude;
                                listDrone[i].Location = locationUpdate;
                            }
                            else
                            {
                                if (parcel.Delivered == DateTime.MinValue)//נאספה אך לא סופקה
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

            foreach (DroneToList drone in listDrone)//נעדכן את הרחפן במקרה שהוא לא עושה משלוח
            {
                if (drone.Status != DroneStatuses.Delivery)
                {
                    drone.Status = (DroneStatuses)R.Next(1, 3);
                    drone.ParcelBeingPassedId = 0;
                    if (drone.Status == DroneStatuses.Charging)
                    {
                        IDAL.DO.DroneCharge droneCharge = new IDAL.DO.DroneCharge();
                        List<IDAL.DO.Station> stations = dal.ListStationsWithOpenSlots().ToList();
                        IDAL.DO.Station station = stations[R.Next(0, stations.Count)];
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
                        List<IDAL.DO.Parcel> parcels = dal.ListParcel().ToList().FindAll(i => i.Delivered != DateTime.MinValue);
                        IDAL.DO.Parcel parcel = parcels.ElementAt(R.Next(0, parcels.Count));
                        IDAL.DO.Customer customer = dal.GetCustomer(parcel.TargetId);
                        Location locationCustomer = new Location { Lattitude = customer.Lattitude, Longitude = customer.Longitude };
                        drone.Location = locationCustomer;
                        IDAL.DO.Station station = GetClosestStation(drone);
                        double distance = (int)dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude,
                            drone.Location.Longitude, customer.Lattitude, customer.Longitude);
                        drone.Battery = R.Next((int)(distance * AvailbleElec) + 1, 101);
                    }
                }
            }
        }
        public void AddDrone(DroneToList newDrone, int chargingStationId)
        {
            bool flag = false;

            IDAL.DO.Drone temp = new IDAL.DO.Drone();
            temp.Id = newDrone.Id;
            temp.Model = newDrone.Model;
            temp.MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight;
            Random r = new Random();
            newDrone.Battery = r.Next(20, 41);
            newDrone.Status = DroneStatuses.Charging;
            newDrone.ParcelBeingPassedId = 0;
            foreach (var objStation in dal.ListBaseStation())
            {
                if (objStation.Id == chargingStationId)
                {
                    Location laLocationOfNewDrone = new Location();
                    laLocationOfNewDrone.Lattitude = objStation.Lattitude;
                    laLocationOfNewDrone.Longitude = objStation.Longitude;
                    newDrone.Location = laLocationOfNewDrone;
                    IDAL.DO.DroneCharge droneToCharge = new IDAL.DO.DroneCharge() { StationId = objStation.Id, DroneId = newDrone.Id, EntryTime = DateTime.Now };
                    dal.AddDroneToCharge(droneToCharge);
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new ItemNotFoundException(chargingStationId, "Enter an existing station number in the system for initial charging of the drone\n");
            }
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
        public void UpdateDrone(DroneToList newDrone)
        {
            try
            {
                IDAL.DO.Drone updateDrone = new IDAL.DO.Drone { Id = newDrone.Id, Model = newDrone.Model };
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
                Console.WriteLine(e);
            }
        }
        public Drone DroneDisplay(int id)
        {
            IEnumerable<IDAL.DO.Parcel> parcels = dal.ListParcel();
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
                if (parcel.DroneId == id)
                {
                    prclTrnsfr.Id = parcel.Id;
                    prclTrnsfr.Weight = (WeightCategories)parcel.Weight;
                    prclTrnsfr.Priority = (Priorities)parcel.Priority;

                    if (parcel.PickedUp != DateTime.MinValue)
                        prclTrnsfr.ParcelSituation = true;
                    else
                        prclTrnsfr.ParcelSituation = false;

                    IDAL.DO.Customer target1 = dal.GetCustomer(parcel.TargetId);
                    tempLocationSupply.Lattitude = target1.Lattitude;
                    tempLocationSupply.Longitude = target1.Longitude;
                    prclTrnsfr.SupplyPoint = tempLocationSupply;

                    IDAL.DO.Customer sender1 = dal.GetCustomer(parcel.SenderId);
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
        public IEnumerable<Drone> GetListDrone()
        {
            IEnumerable<IDAL.DO.Drone> drones = dal.ListDrone();
            List<Drone> temp = new List<Drone>();
            foreach (var drone in drones)
            {
                Drone obj = DroneDisplay(drone.Id);
                temp.Add(obj);
            }
            return temp;
        }
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

        public void DeliveryOfParcelByDrone(int droneId)
        {
            DroneToList drone = listDrone.Find(i => i.Id == droneId);
            IDAL.DO.Parcel parcel = dal.GetParcel(drone.ParcelBeingPassedId);
            IDAL.DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
            if (parcel.DroneId == droneId && parcel.PickedUp != DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
            {
                double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                    customerTarget.Lattitude, customerTarget.Longitude);
                drone.Battery -= distance * dal.GetElectricUsageNumber(parcel.Weight);
                drone.Location.Lattitude = customerTarget.Lattitude;
                drone.Location.Longitude = customerTarget.Longitude;
                drone.Status = DroneStatuses.Available;
                dal.SupplyParcelUpdate(parcel.Id);
            }
            else
            {
                throw new IllegalActionException("The parcel was not collected by the drone\n");
            }
        }
        public void ParcelCollectionByDrone(int droneId)
        {
            DroneToList drone = listDrone.Find(i => i.Id == droneId);
            IDAL.DO.Parcel parcel = dal.GetParcel(drone.ParcelBeingPassedId);
            IDAL.DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
            if (parcel.Affiliation != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue && parcel.DroneId == drone.Id && drone.Status == DroneStatuses.Delivery)
            {
                double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                    customerSender.Lattitude, customerSender.Longitude);

                drone.Battery -= distance * dal.GetElectricUsage()[0];
                drone.Location.Lattitude = customerSender.Lattitude;
                drone.Location.Longitude = customerSender.Longitude;
                dal.PickupParcelUpdate(parcel.Id);
            }
            else
            {
                throw new IllegalActionException("The drone is not in delivery mode / not associated with this parcel\n");
            }
        }

        public void AffiliateParcelToDrone(int droneId)
        {
            DroneToList drone = listDrone.Find(i => i.Id == droneId);
             if (drone == null)
            {
                throw new ItemAlreadyExistsException(droneId);
            }
            List<IDAL.DO.Parcel> parcels = dal.ListParcel().ToList();
            List<IDAL.DO.Parcel> parcelsFiltered = new List<IDAL.DO.Parcel>();
            parcelsFiltered = parcels.FindAll(i => i.Priority == IDAL.DO.Priorities.Urgent);
            if (parcelsFiltered.Count == 0) { parcelsFiltered= parcels.FindAll(i => i.Priority == IDAL.DO.Priorities.Express); }
            if (parcelsFiltered.Count == 0) { parcelsFiltered= parcels.FindAll(i => i.Priority == IDAL.DO.Priorities.Regular); }

            parcels = parcelsFiltered;

            parcelsFiltered = parcels.FindAll(i => i.Weight == (IDAL.DO.WeightCategories)drone.MaxWeight);
            if (parcelsFiltered.Count == 0)
                parcelsFiltered = parcels.FindAll(i => i.Weight == (IDAL.DO.WeightCategories)drone.MaxWeight - 1 && (drone.MaxWeight - 1) > 0);
            if (parcelsFiltered.Count == 0)
                parcelsFiltered = parcels.FindAll(i => i.Weight == (IDAL.DO.WeightCategories)drone.MaxWeight - 2 && (drone.MaxWeight - 2) > 0);

            IDAL.DO.Parcel parcel = GetClosestParcel(parcelsFiltered, drone);
            IDAL.DO.Customer customerSender = dal.GetCustomer(parcel.SenderId);
            IDAL.DO.Customer customerTarget = dal.GetCustomer(parcel.TargetId);
            double minimumBattery =
                GetMinimumBatteryToShip(drone, customerSender, customerTarget, parcel.Weight);

            if (drone.Status == DroneStatuses.Available && drone.Battery > minimumBattery)
            {
                drone.Status = DroneStatuses.Delivery;
                dal.Affiliate(parcel.Id, drone.Id);
            }
            drone.ParcelBeingPassedId = parcel.Id;
        }

        private double GetMinimumBatteryToShip(DroneToList drone, IDAL.DO.Customer customerSender, IDAL.DO.Customer customerTarget, IDAL.DO.WeightCategories weight)
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
            return (distance2 * GetElectricUsageNumber(weight) + distance1 * AvailbleElec +
                    distance3 * AvailbleElec);
        }
        public void ReleaseDroneFromCharging(int droneId, double time)
        {
            var drone = listDrone.Find(i => i.Id == droneId);
            if (drone == null)
            {
                throw new ItemNotFoundException(droneId, "Enter an existing drone number in the system");
            }
            if (drone.Status != DroneStatuses.Charging)
            {
                throw new IllegalActionException("The drone is not charging");
            }
            dal.ReleaseDroneFromCharger(droneId);
            drone.Status = DroneStatuses.Available;
            drone.Battery = +(int)dal.GetChargeSpeed() * time;
            if (drone.Battery > 100)
            {
                drone.Battery = 100;
            }
        }
        public void SendingDroneForCharging(int droneId)
        {
            var drone = listDrone.Find(i => i.Id == droneId);
            if (drone == null)
            {
                throw new ItemNotFoundException(droneId);
            }
            if (drone.Status != DroneStatuses.Available)
            {
                throw new IllegalActionException("The drone is not available");
            }
            IDAL.DO.Station closestStation = GetClosestStation(drone);
            double distance = dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                closestStation.Lattitude, closestStation.Longitude);

            if (closestStation.AvailableChargeSlots > 0 && drone.Battery > distance * dal.GetElectricUsage()[0])
            {
                IDAL.DO.DroneCharge droneCharge = new()
                { DroneId = drone.Id, StationId = closestStation.Id, EntryTime = DateTime.Now };
                dal.AddDroneToCharge(droneCharge);
                drone.Battery = distance * dal.GetElectricUsage()[0];
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

    }
}
