using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using BO;

namespace BL
{
    internal partial class BL : IBL
    {
        /// <summary>
        /// Adding a Parcel to our Data Source
        /// </summary>
        /// <param name="newParcel"> Parecl to be added </param>
        public void AddParcel(Parcel newParcel)
        {
            if (!dal.CheckId(newParcel.Sender.Id))
            {
                throw new IllegalActionException("Incorrect ID number of sender");

            }
            if (!dal.CheckId(newParcel.Target.Id))
            {
                throw new IllegalActionException("Incorrect ID number of target");
            }
            DO.Customer senderCustomer = dal.ListCustomer(i=>true).ToList().Find(i => i.Id == newParcel.Sender.Id);
            if (senderCustomer.Id==0) throw new IllegalActionException("The sender number does not exist in the system");
            DO.Customer targetCustomer = dal.ListCustomer(i=>true).ToList().Find(i => i.Id == newParcel.Target.Id);
            if (senderCustomer.Id ==0) throw new IllegalActionException("The target number does not exist in the system");
            DO.Parcel temp = new DO.Parcel();
            try
            {
                temp.SenderId = newParcel.Sender.Id;
                temp.TargetId = newParcel.Target.Id;
                temp.Weight = (DO.WeightCategories)newParcel.Weight;
                temp.Priority = (DO.Priorities)newParcel.Priority;
                temp.Creating = DateTime.Now;
                temp.Affiliation = null;
                temp.PickedUp = null;
                temp.Delivered = null;
                temp.DroneId = 0;
                temp.Id = dal.GetParcelId();
                dal.AddParcel(temp);
            }
            catch (Exception e)
            {
                throw new ItemAlreadyExistsException(temp.Id, "Enter a new customer number\n", e);
            }
        }
        /// <summary>
        /// Receives the last Parcel ID
        /// </summary>
        /// <returns> ID of parcel</returns>
        public int GetParcelId()
        {
            return dal.GetParcelId() - 1;
        }
        /// <summary>
        /// Returns the list of parcels
        /// </summary>
        /// <returns> IEnumeranle of parcels</returns>
        public IEnumerable<Parcel> GetListParcel()
        {
            List<Parcel> temp = new List<Parcel>();
            foreach (DO.Parcel parcel in dal.ListParcel(i=>true))
            {
                Parcel obj = ParcelDisplay(parcel.Id);
                temp.Add(obj);
            }
            return temp;
        }

        public IEnumerable<ParcelToList> GetParcels()
        {
            List<ParcelToList> temp = new List<ParcelToList>();
            foreach (DO.Parcel parcel in dal.ListParcel(i => true))
            {
                Parcel obj = ParcelDisplay(parcel.Id);
                temp.Add(MakeParcelToList(obj));
            }
            return temp;
        }

        /// <summary>
        /// Retrieving info to transform parcel into parceltolist and returning it
        /// </summary>
        /// <param name="objParcel"> Parcel to be transformed</param>
        /// <returns> parceltolist after we've found all its necessary fields</returns>
        public ParcelToList MakeParcelToList(Parcel objParcel)
        {
            ParcelToList parcelToList = new ParcelToList();
            parcelToList.Id = objParcel.Id;
            parcelToList.Weight = objParcel.Weight;
            parcelToList.Priority = objParcel.Priority;
            parcelToList.SenderName = objParcel.Sender.Name;
            parcelToList.TargetName = objParcel.Target.Name;
            if (objParcel.Delivered != null)
                parcelToList.ShipmentStatus = ParcelStatus.Supplied;
            else if (objParcel.PickedUp != null)
                parcelToList.ShipmentStatus = ParcelStatus.PickedUp;
            else if (objParcel.Affiliation != null)
                parcelToList.ShipmentStatus = ParcelStatus.Assigned;
            else
                parcelToList.ShipmentStatus = ParcelStatus.Created;
            return parcelToList;
        }
        /// <summary>
        /// Returns the closest parcel to the drone in order for him to pick it up
        /// </summary>
        /// <param name="parcels"> list of parcels </param>
        /// <param name="drone"> drone thta will take the parcel</param>
        /// <returns> parcel that is closest </returns>
        private DO.Parcel GetClosestParcel(List<DO.Parcel> parcels, DroneToList drone)
        {
            int i = 0, index = 0;
            Location closestParcel = new Location();
            DO.Customer customer = dal.GetCustomer(parcels[0].SenderId);
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
        /// <summary>
        /// returning parcel according to ID in order to display
        /// </summary>
        /// <param name="id"> Id number</param>
        /// <returns> Parcel to be displayed </returns>
        public Parcel ParcelDisplay(int id)
        {
            DO.Parcel parcel = dal.GetParcel(id);
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
                droneInParcel.DroneId = droneToList.Id;
                droneInParcel.location = droneToList.Location;
                droneInParcel.Battery = droneToList.Battery;
                temp.drone = droneInParcel;
            }
            DO.Customer sender = dal.GetCustomer(parcel.SenderId);
            customerInParcel1.Id = sender.Id;
            customerInParcel1.Name = sender.Name;
            temp.Sender = customerInParcel1;
            DO.Customer target = dal.GetCustomer(parcel.TargetId);
            customerInParcel2.Id = target.Id;
            customerInParcel2.Name = target.Name;
            temp.Target = customerInParcel2;
            return temp;
        }

        public IEnumerable<ParcelToList> GetParcelByStatus(IEnumerable itemsSource, ParcelStatus selectedStatus)
        {
            List<ParcelToList> temp = new List<ParcelToList>();
            foreach (ParcelToList parcel in itemsSource)
            {
                if (parcel.ShipmentStatus == selectedStatus)
                {
                    temp.Add(parcel);
                }
            }
            return temp;
        }
        public IEnumerable<ParcelToList> GetParcelByWeight(IEnumerable itemsSource, WeightCategories selectedWeight)
        {
            List<ParcelToList> temp = new List<ParcelToList>();
            foreach (ParcelToList parcel in itemsSource)
            {
                if (parcel.Weight == selectedWeight)
                {
                    temp.Add(parcel);
                }
            }
            return temp;
        }

        public IEnumerable<ParcelToList> GetParcelByPriority(IEnumerable itemsSource, Priorities selectedPriority)
        {
            List<ParcelToList> temp = new List<ParcelToList>();
            foreach (ParcelToList parcel in itemsSource)
            {
                if (parcel.Priority == selectedPriority)
                {
                    temp.Add(parcel);
                }
            }
            return temp;
        }

        public void DeleteParcel(Parcel newParcel)
        {
            if(newParcel.Affiliation == null || newParcel.Delivered != null)
            {
                dal.DeleteParcel(newParcel.Id);
            }
        }
    }
}

