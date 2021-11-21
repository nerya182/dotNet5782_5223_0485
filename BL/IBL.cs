using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace IBL
{
    public interface  IBL
    {
        void AddStation(Station newStation);
        void AddDrone(DroneToList newDrone, int chargingStationId);
        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newParcel);
        void UpdateDrone(DroneToList newDrone);
        IEnumerable<Station> GetListStation();
        Drone DroneDisplay(int id);
        IEnumerable<Station> GetListStationsWithOpenSlots();
        IEnumerable<Parcel> GetListParcelOnAir();
        IEnumerable<Parcel> GetListParcel();
        IEnumerable<Customer> GetListCustomer();
        IEnumerable<Drone> GetListDrone();
        ParcelToList MakeParcelToList(Parcel objParcel);
        CustomerToList MakeCustomerToList(Customer objCustomer);
        DroneToList MakeDroneToList(Drone objDrone);
        StationToList MakeStationToList(Station objStation);
        void DeliveryOfParcelByDrone(int droneId);
        void ParcelCollectionByDrone(int droneId);
        void AffiliateParcelToDrone(int droneId);
        void ReleaseDroneFromCharging(int droneId);
        void SendingDroneForCharging(int droneId);
        DroneToList GetDroneFromLstDrone(int id);
        Station BaseStationDisplay(int id);
        Customer CustomerDisplay(int id);
        Parcel ParcelDisplay(int id);
        void UpdateCustomer(Customer updateCustomer);
        void UpdateStation(Station updateStation, int chargingPositions);
        int GetParcelId();
    }
}
