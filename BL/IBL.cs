using BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DalApi;

namespace BlApi
{
    public interface IBL
    {
        double AvailbleElec { get; set; }
        IDal dal { get; set; }

        /// <summary>
        /// Adding a station to our data source
        /// </summary>
        /// <param name="newStation"></param>
        void AddStation(Station newStation);
        /// <summary>
        /// Adding a drone
        /// </summary>
        /// <param name="newDrone"></param>
        /// <param name="chargingStationId"></param>
        void AddDrone(DroneToList newDrone, int chargingStationId);
        /// <summary>
        /// Adding a customer to our list of customers in data source
        /// </summary>
        /// <param name="newCustomer"></param>
        void AddCustomer(Customer newCustomer);
        /// <summary>
        /// Adding a display to our Data Source
        /// </summary>
        /// <param name="newParcel"></param>
        void AddParcel(Parcel newParcel);
        /// <summary>
        /// Updating a drone's model
        /// </summary>
        /// <param name="newDrone"></param>
        void UpdateDrone(DroneToList newDrone);
        /// <summary>
        /// Returns our list of stations
        /// </summary>
        /// <returns>IEnumerable of stations</returns>
        IEnumerable<Station> GetListStation();
        IEnumerable<DroneToList> GetByStatus(IEnumerable itemsSource, DroneStatuses selectedStatus);
        void Affiliate(int id, int? parcelId);

        /// <summary>
        /// Returning Drone according to ID in order to be displayed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Drone to be displayed</returns>
        Drone DroneDisplay(int id);
        IEnumerable<DroneToList> GetByWeight(IEnumerable itemsSource, WeightCategories selectedWeight);
        double RequiredBattery(int id,Drone drone);

        /// <summary>
        /// Returns the stations that have an open charge slot
        /// </summary>
        /// <returns>IEnumerable of stations</returns>
        IEnumerable<Station> GetListStationsWithOpenSlots();
        /// <summary>
        /// Returns the list of parcels
        /// </summary>
        /// <returns>IEnumeranle of parcels</returns>
        IEnumerable<Parcel> GetListParcel();
        /// <summary>
        /// Retrieving our list of customers of type IBL BO
        /// </summary>
        /// <returns>IEnumerable list of customer</returns>
        IEnumerable<CustomerToList> GetListCustomer();
        /// <summary>
        /// Retrieving the list of drones
        /// </summary>
        /// <returns>UEnumerable of drones</returns>
        /// 
        public IEnumerable<StationToList> GetStations();

        public IEnumerable<ParcelToList> GetParcels();
        IEnumerable<DroneToList> GetListDrone();
        void updateDrone(int droneId, int timerCheck);

        /// <summary>
        /// Retrieving the list of drones
        /// </summary>
        /// <returns>UEnumerable of drones</returns>
        IEnumerable<Drone> GetDrones();
        void updateReleaseDrone(int droneId);

        /// <summary>
        /// Retrieving info to transform parcel into parceltolist and returning it
        /// </summary>
        /// <param name="objParcel"></param>
        /// <returns>parceltolist after we've found all its necessary fields</returns>
        ParcelToList MakeParcelToList(Parcel objParcel);
        /// <summary>
        /// Retrieving the info needed to transform the 'customer' into a 'customertolist'
        /// </summary>
        /// <param name="objCustomer"></param>
        /// <returns>customertolist after we've found necessary info</returns>
        CustomerToList MakeCustomerToList(Customer objCustomer);
        /// <summary>
        /// Retrieving information to transform drone into dronetolist and returning it
        /// </summary>
        /// <param name="objDrone"></param>
        /// <returns>dronetolist after we've found necessary info</returns>
        DroneToList MakeDroneToList(Drone objDrone);
        void updateCollectionByDron(int droneId);

        /// <summary>
        /// Retrieving info to transform station into station to list
        /// </summary>
        /// <param name="objStation"></param>
        /// <returns>stationto list after we recieved the necessary info</returns>
        StationToList MakeStationToList(Station objStation);
        /// <summary>
        /// Drone delivering the parcel that answers the criterias that were given to us
        /// </summary>
        /// <param name="droneId"></param>
        void DeliveryOfParcelByDrone(int droneId);
        /// <summary>
        /// Drone picking up the parcel he was assigned to
        /// </summary>
        /// <param name="droneId"></param>
        void ParcelCollectionByDrone(int droneId);
        void DeliveryByDron(int droneId);

        /// <summary>
        /// Affiliating drone with the parcel that answers the criterias that were given to us
        /// </summary>
        /// <param name="droneId"></param>
        void AffiliateParcelToDrone(int droneId);
        /// <summary>
        ///  Releasing drone from charging according to ID
        /// </summary>
        /// <param name="droneId"></param>
        void ReleaseDroneFromCharging(int droneId);
        /// <summary>
        ///  Charging a drone according to ID
        /// </summary>
        /// <param name="droneId"></param>
        void SendingDroneForCharging(int droneId);
        /// <summary>
        /// Recieving a dronetolist
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Dronetolist we we're looking for</returns>
        DroneToList GetDroneFromLstDrone(int id);
        /// <summary>
        /// Returning a Station according to ID in order to be displayed
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Station to be displayed</returns>
        Station BaseStationDisplay(int id);
        /// <summary>
        /// Returning a customer of type IBL BO according to ID in order to display
        /// </summary>
        /// <param name="id"></param>
        /// <returns>display customer</returns>
        Customer CustomerDisplay(int id);
        /// <summary>
        /// returning parcel according to ID in order to display
        /// </summary>
        /// <param name="id"></param>
        /// <returns>rcel to be displayed</returns>
        Parcel ParcelDisplay(int id);
        /// <summary>
        /// Updating the customer's info
        /// </summary>
        /// <param name="updateCustomer"></param>
        void UpdateCustomer(Customer updateCustomer);
        /// <summary>
        /// Updating the amount of available slots the station has
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="chargingPositions"></param>
        void UpdateStationPositions(int stationId, int chargingPositions);
        /// <summary>
        /// Receives the last Parcel ID
        /// </summary>
        /// <returns>ID of parcel</returns>
        int GetParcelId();
        /// <summary>
        /// Updating the stations name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        void UpdateStationName(int id, string name);

         IEnumerable<ParcelToList> GetParcelByStatus(ParcelStatus selectedStatus);

         IEnumerable<ParcelToList> GetParcelByWeight(WeightCategories selectedWeight);

         IEnumerable<ParcelToList> GetParcelByPriority(Priorities selectedPriority);
         void DeleteParcel(ParcelToList parcel);
         void DeleteCustomer(CustomerToList customer);
         void DeleteStation(StationToList station);
        IEnumerable<IGrouping<DroneStatuses, DroneToList>> GroupingStatus();
        IEnumerable<IGrouping<WeightCategories, DroneToList>> GroupingWeight();
        IEnumerable<IGrouping<bool, StationToList>> GroupingAvailableChargeSlots();
        IEnumerable<IGrouping<int, StationToList>> GroupingChargeSlots();
        IEnumerable<IGrouping<string, ParcelToList>> GroupingTargetNam();
        IEnumerable<IGrouping<string, ParcelToList>> GroupingSenderName();
        IEnumerable<Customer> GetListCustomers();
        IEnumerable<ParcelToList> filterToday(int num);
        void StartSimulator(int droneId, Action func, Func<bool> checkStop);
        double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2);
        DO.Station GetClosestStation(DroneToList drone);
        Location GoTowards(int DroneId, Location Destination, double speedInKm, double Elec);
        List<double> GetElectricUsage();
    }
}
