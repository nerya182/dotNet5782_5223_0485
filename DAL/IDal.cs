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
        public  Station GetStation(int id);
        public Drone GetDrone(int droneId);
        public  Customer GetCustomer(int customerId);
        public  Parcel GetParcel(int parcelId);
        public  DateTime GetParcelCreating(int parcelId);
        public DateTime GetParcelAffiliation(int parcelId);
        public DateTime GetParcelPickedUp(int parcelId);
        public DateTime GetParcelDelivered(int parcelId);
        public void AddStation(Station newStation);
        public void AddDrone(Drone newDrone);
        public void AddCustomer(Customer newCustomer);
        public void AddParcel(Parcel newParcel);
        public void Affiliate(int idParcel, int droneId);
        public void PickupParcelUpdate(int ParcelId);
        public void SupplyParcelUpdate(int ParcelId);
        public void ReleaseDroneFromCharger(int DroneId);
        public IEnumerable<Station> PrintBaseStation();
        public IEnumerable<Drone> PrintDrone();
        public IEnumerable<Customer> PrintCustomer();
        public IEnumerable<Parcel> PrintParcel();
        public double distanceCalculation(double lat1, double lon1, int id, char temp);
        public IEnumerable<Parcel> PrintParcelOnAir();
        public int GetParcelId();
        public Parcel ParcelDisplay(int id);
        public Station BaseStationDisplay(int id);
        public IEnumerable<Station> PrintStationsWithOpenSlots();
        public Drone DroneDisplay(int id);
        public Customer CustomerDisplay(int id);
        public void AddDroneToCharge(DroneCharge droneCharge, int StationId);
        public double[] GetElectricUsage();
        public double GetChargeSpeed();




    }
}
