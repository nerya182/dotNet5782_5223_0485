using System;
using System.Collections.Generic;
using System.Linq;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public void AddParcel(Parcel newParcel)
        {
            IDAL.DO.Parcel temp = new IDAL.DO.Parcel();
            try
            {
                temp.SenderId = newParcel.Sender.Id;
                temp.TargetId = newParcel.Target.Id;
                temp.Weight = (IDAL.DO.WeightCategories)newParcel.Weight;
                temp.Priority = (IDAL.DO.Priorities)newParcel.Priority;
                temp.Creating = DateTime.Now;
                temp.Affiliation = DateTime.MinValue;
                temp.PickedUp = DateTime.MinValue;
                temp.Delivered = DateTime.MinValue;
                temp.DroneId = 0;
                temp.Id = dal.GetParcelId();
                dal.AddParcel(temp);

            }
            catch (Exception e)
            {
                throw new ItemAlreadyExistsException(temp.Id, "Enter a new customer number\n", e);
            }
        }
        public int GetParcelId()
        {
            return dal.GetParcelId() - 1;
        }


        public IEnumerable<Parcel> GetListParcel()
        {
            IEnumerable<IDAL.DO.Parcel> parcels = dal.ListParcel();
            List<Parcel> temp = new List<Parcel>();
            foreach (var parcel in parcels)
            {
                Parcel obj = ParcelDisplay(parcel.Id);
                temp.Add(obj);
            }
            return temp;
        }
        public ParcelToList MakeParcelToList(Parcel objParcel)
        {
            ParcelToList parcelToList = new ParcelToList();
            parcelToList.Id = objParcel.Id;
            parcelToList.Weight = objParcel.Weight;
            parcelToList.Priority = objParcel.Priority;
            parcelToList.SenderName = objParcel.Sender.Name;
            parcelToList.TargetName = objParcel.Target.Name;
            if (objParcel.Delivered != DateTime.MinValue)
                parcelToList.ShipmentStatus = ParcelStatus.Supplied;
            else if (objParcel.PickedUp != DateTime.MinValue)
                parcelToList.ShipmentStatus = ParcelStatus.PickedUp;
            else if (objParcel.Affiliation != DateTime.MinValue)
                parcelToList.ShipmentStatus = ParcelStatus.Assigned;
            else
                parcelToList.ShipmentStatus = ParcelStatus.Created;
            return parcelToList;
        }
        private IDAL.DO.Parcel GetClosestParcel(List<IDAL.DO.Parcel> parcels, DroneToList drone)
        {
            int i = 0, index = 0;

            Location closestParcel = new Location();
            IDAL.DO.Customer customer = dal.GetCustomer(parcels[0].SenderId);
            closestParcel.Lattitude = customer.Lattitude;
            closestParcel.Longitude = customer.Longitude;
            foreach (var parcel in parcels)
            {
                customer = dal.GetCustomer(parcel.SenderId);
                if (dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude,
                    customer.Lattitude, customer.Longitude) < dal.GetDistanceFromLatLonInKm(drone.Location.Lattitude, drone.Location.Longitude, closestParcel.Lattitude, closestParcel.Longitude))
                {
                    index = i;
                    closestParcel.Lattitude = customer.Lattitude;
                    closestParcel.Longitude = customer.Longitude;
                }
                i++;
            }
            return parcels[index];
        }
        public Parcel ParcelDisplay(int id)
        {
            IDAL.DO.Parcel parcel = dal.GetParcel(id);
            DroneInParcel droneInParcel = new DroneInParcel();
            CustomerInParcel customerInParcel1 = new CustomerInParcel();
            CustomerInParcel customerInParcel2 = new CustomerInParcel();
            Parcel temp = new Parcel();
             temp.Id = parcel.Id;
            temp.Weight = (WeightCategories)parcel.Weight;
            temp.Priority = (Priorities)parcel.Priority;
            temp.Affiliation = parcel.Affiliation;
            temp.Creating = parcel.Creating;
            temp.Delivered = parcel.Delivered;
            temp.PickedUp = parcel.PickedUp;

            if (parcel.DroneId != 0)
            {
                DroneToList droneToList = listDrone.Find(i => i.Id == parcel.DroneId);
                // DroneToList droneToList = GetDroneFromLstDrone(parcel.DroneId);
                droneInParcel.DroneId = droneToList.Id;
                droneInParcel.location = droneToList.Location;
                droneInParcel.Battery = droneToList.Battery;
                temp.drone = droneInParcel;
            }
            
            IDAL.DO.Customer sender = dal.GetCustomer(parcel.SenderId);
            customerInParcel1.Id = sender.Id;
            customerInParcel1.Name = sender.Name;
            temp.Sender = customerInParcel1;

            IDAL.DO.Customer target = dal.GetCustomer(parcel.TargetId);
            customerInParcel2.Id = target.Id;
            customerInParcel2.Name = target.Name;
            temp.Target = customerInParcel2;

            return temp;
        }
    }
}

