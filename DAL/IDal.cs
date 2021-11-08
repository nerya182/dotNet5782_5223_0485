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
       Station GetStation(int id);
        Drone GetDrone(int droneId);
        Customer GetCustomer(int customerId);
        Parcel GetParcel(int parcelId);
        DateTime GetParcelCreating(int parcelId);
        DateTime GetParcelAffiliation(int parcelId);
        DateTime GetParcelPickedUp(int parcelId);
        DateTime GetParcelDelivered(int parcelId);
        void AddStation(Station newStation);
        void AddDrone(Drone newDrone);
        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newParcel);
        void Affiliate(int idParcel, int droneId);
        void PickupParcelUpdate(int ParcelId);
        void SupplyParcelUpdate(int ParcelId);
        void ReleaseDroneFromCharger(int DroneId);
        IEnumerable<Station> ListBaseStation();
        IEnumerable<DroneCharge> ListDroneCharge();
        IEnumerable<Drone> ListDrone();
        IEnumerable<Customer> ListCustomer();
        IEnumerable<Parcel> ListParcel();
        double distanceCalculation(double lat1, double lon1, int id, char temp);
        IEnumerable<Parcel> ListParcelOnAir();
        int GetParcelId(); 
        Parcel ParcelDisplay(int id);
        Station BaseStationDisplay(int id);
        IEnumerable<Station> ListStationsWithOpenSlots();
        Drone DroneDisplay(int id);
        Customer CustomerDisplay(int id);
        void AddDroneToCharge(DroneCharge droneCharge, int StationId);
        double[] GetElectricUsage();
        double GetChargeSpeed();

        public double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2);
        public double Deg2rad(double deg);
    }
}
