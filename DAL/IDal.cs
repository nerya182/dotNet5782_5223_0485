using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;

namespace IDAL
{
    public interface IDal
    {
        /// <summary>
     ///  Returns the station in the certain index
     /// </summary>
     /// <param name="id"></param>
     /// <returns>Station</returns>
        Station GetStation(int id);
        /// <summary>
        /// Returns the drone in the certain index
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns>Drone</returns>
        Drone GetDrone(int droneId);
        /// <summary>
        /// Returns the customer in the certain index
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Customer</returns>
        Customer GetCustomer(int customerId);
        /// <summary>
        /// Returns the parcel in the certain index
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns>Parcel</returns>
        Parcel GetParcel(int parcelId);
        /// <summary>
        /// Returns the time the parcel was created
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns>DateTime</returns>
        DateTime GetParcelCreating(int parcelId);
        /// <summary>
        /// Returns the time the parcel was affiliated with a drone
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns>DateTime</returns>
        DateTime GetParcelAffiliation(int parcelId);
        /// <summary>
        /// Returns the time the parcel was picked up
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns>DateTime</returns>
        DateTime GetParcelPickedUp(int parcelId);
        /// <summary>
        /// Returns the time the parcel was delivered
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns>DateTime</returns>
        DateTime GetParcelDelivered(int parcelId);
        /// <summary>
        ///  Adding a station to the next open index
        /// </summary>
        /// <param name="newStation"></param>
        void AddStation(Station newStation);
        /// <summary>
        /// Adding a drone to the next open index
        /// </summary>
        /// <param name="newDrone"></param>
        void AddDrone(Drone newDrone);
        /// <summary>
        /// Adding a customer to the next open index
        /// </summary>
        /// <param name="newCustomer"></param>
        void AddCustomer(Customer newCustomer);
        /// <summary>
        /// Adding a parcel to the next open index
        /// </summary>
        /// <param name="newParcel"></param>
        void AddParcel(Parcel newParcel);
        /// <summary>
        /// if we've found an available drone, we will affiliate the parcel with it
        /// </summary>
        /// <param name="idParcel"></param>
        /// <param name="droneId"></param>
        void Affiliate(int idParcel, int droneId);
        /// <summary>
        /// Changing drone status, and time of pickup
        /// </summary>
        /// <param name="ParcelId"></param>
        void PickupParcelUpdate(int ParcelId);
        /// <summary>
        ///  Delivering the parcel
        /// </summary>
        /// <param name="ParcelId"></param>
        void SupplyParcelUpdate(int ParcelId);
        /// <summary>
        /// We find the charger and the station the drone is  charging again
        /// </summary>
        /// <param name="DroneId"></param>
        void ReleaseDroneFromCharger(int DroneId);
        /// <summary>
        ///  Returning a list of all the Stations
        /// </summary>
        /// <returns>list</returns>
        IEnumerable<Station> ListBaseStation();
        /// <summary>
        /// /Returning a list of all the drone charge
        /// </summary>
        /// <returns>list</returns>
        IEnumerable<DroneCharge> ListDroneCharge();
        /// <summary>
        /// Returning a list of all the Drones
        /// </summary>
        /// <returns>list</returns>
        IEnumerable<Drone> ListDrone();
        /// <summary>
        /// Returning a list of all the Customers
        /// </summary>
        /// <returns>List</returns>
        IEnumerable<Customer> ListCustomer();
        /// <summary>
        /// /Returning a list of all the Parcels
        /// </summary>
        /// <returns>List</returns>
        IEnumerable<Parcel> ListParcel();
        /// <summary>
        /// distance calculation
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="id"></param>
        /// <param name="temp"></param>
        /// <returns>double-distance</returns>
        double distanceCalculation(double lat1, double lon1, int id, char temp);
        /// <summary>
        /// /Returning a list of all the Parcels that have not been affiliated with a Drone
        /// </summary>
        /// <returns></returns>
        IEnumerable<Parcel> ListParcelOnAir();
        /// <summary>
        /// return the next number of new parcel 
        /// </summary>
        /// <returns>number parcel</returns>
        int GetParcelId();
        /// <summary>
        ///  Returning the correct Parcel in order to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns>parcel</returns>
        Parcel ParcelDisplay(int id);
        /// <summary>
        /// Returning the correct Station in order to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns>base station</returns>
        Station BaseStationDisplay(int id);
        /// <summary>
        /// return the stations that have open slot to the list
        /// </summary>
        /// <returns>StationsWithOpenSlots</returns>
        IEnumerable<Station> ListStationsWithOpenSlots();
        /// <summary>
        /// Returning the correct Drone in order to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns>drone</returns>
        Drone DroneDisplay(int id);
        /// <summary>
        /// Returning the correct Customer in order to be displayed to the console
        /// </summary>
        /// <param name="id"></param>
        /// <returns>customer</returns>
        Customer CustomerDisplay(int id);
        /// <summary>
        ///  Lowering the available slots by 1 because we've added a drone to charge there
        /// </summary>
        /// <param name="droneCharge"></param>
        void AddDroneToCharge(DroneCharge droneCharge);
        /// <summary>
        /// return power consumption
        /// </summary>
        /// <returns>double</returns>
        double[] GetElectricUsage();
        /// <summary>
        /// Returns the charging rate
        /// </summary>
        /// <returns>double</returns>
        double GetChargeSpeed();
        /// <summary>
        /// return distance Between two points
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns>double</returns>
        double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2);
        /// <summary>
        /// Conversion to degrees
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        double Deg2rad(double deg);
        /// <summary>
        /// Checks ID number according to a review digit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool CheckId(int id);
        /// <summary>
        /// Number of available seats at the station
        /// </summary>
        /// <param name="id"></param>
        /// <returns>int</returns>
        int AvailableChargeSlotsInStation(int id);
        /// <summary>
        /// Returns the power consumption according to the weight of the package
        /// </summary>
        /// <param name="weight"></param>
        /// <returns>double</returns>
        double GetElectricUsageNumber(WeightCategories weight);
        /// <summary>
        /// Update Customer-name/phone
        /// </summary>
        /// <param name="updateCustomer"></param>
        void UpdateCustomer(Customer updateCustomer);
        /// <summary>
        /// Update station name/available charge slots
        /// </summary>
        /// <param name="station"></param>
        void UpdateStation(Station station);
        /// <summary>
        /// Update drone-model
        /// </summary>
        /// <param name="updateDrone"></param>
        void UpdateDrone(Drone updateDrone);
   

    }
}
