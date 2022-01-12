using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using BO;
using System.Runtime.CompilerServices;
using DalApi;

namespace BL
{
    internal partial class BL : IBL
    {
       
        /// <summary>
        /// Adding a Parcel to our Data Source
        /// </summary>
        /// <param name="newParcel"> Parecl to be added </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel newParcel)
        {
            lock(dal)
            {
                if (!dal.CheckId(newParcel.Sender.Id))
                {
                    throw new IllegalActionException("Incorrect ID number of sender");

                }
                if (!dal.CheckId(newParcel.Target.Id))
                {
                    throw new IllegalActionException("Incorrect ID number of target");
                }
                DO.Customer senderCustomer = dal.ListCustomer(i => true).ToList().Find(i => i.Id == newParcel.Sender.Id);
                if (senderCustomer.Id == 0) throw new IllegalActionException("The sender number does not exist in the system");
                DO.Customer targetCustomer = dal.ListCustomer(i => true).ToList().Find(i => i.Id == newParcel.Target.Id);
                if (senderCustomer.Id == 0) throw new IllegalActionException("The target number does not exist in the system");
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
            
        }
        /// <summary>
        /// Receives the last Parcel ID
        /// </summary>
        /// <returns> ID of parcel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetParcelId()
        {
            lock(dal)
            {
                return dal.GetParcelId() - 1;
            }
        }
        /// <summary>
        /// Returns the list of parcels
        /// </summary>
        /// <returns> IEnumeranle of parcels</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetListParcel()
        {
            lock(dal)
            {
                return (from DO.Parcel parcel in dal.ListParcel(i => true)
                        let obj = ParcelDisplay(parcel.Id)
                        select obj).ToList();
            }         
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcels()
        {
            lock(dal)
            {
                List<ParcelToList> temp = new List<ParcelToList>();
                foreach (DO.Parcel parcel in dal.ListParcel(i => true))
                {
                    Parcel obj = ParcelDisplay(parcel.Id);
                    temp.Add(MakeParcelToList(obj));
                }
                return temp;
            }          
        }

        /// <summary>
        /// Retrieving info to transform parcel into parceltolist and returning it
        /// </summary>
        /// <param name="objParcel"> Parcel to be transformed</param>
        /// <returns> parceltolist after we've found all its necessary fields</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
            lock(dal)
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
        }
        /// <summary>
        /// returning parcel according to ID in order to display
        /// </summary>
        /// <param name="id"> Id number</param>
        /// <returns> Parcel to be displayed </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel ParcelDisplay(int id)
        {
            lock(dal)
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
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcelByStatus(ParcelStatus selectedStatus)
        {
            lock(dal)
            {
                List<ParcelToList> temp = new List<ParcelToList>();
                foreach (ParcelToList parcel in GetParcels())
                {
                    if (parcel.ShipmentStatus == selectedStatus)
                    {
                        temp.Add(parcel);
                    }
                }
                return temp;
            }
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcelByWeight(WeightCategories selectedWeight)
        {
            lock(dal)
            {
                List<ParcelToList> temp = new List<ParcelToList>();
                foreach (ParcelToList parcel in GetParcels())
                {
                    if (parcel.Weight == selectedWeight)
                    {
                        temp.Add(parcel);
                    }
                }
                return temp;
            }
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcelByPriority(Priorities selectedPriority)
        {
            lock(dal)
            {
                List<ParcelToList> temp = new List<ParcelToList>();
                foreach (ParcelToList parcel in GetParcels())
                {
                    if (parcel.Priority == selectedPriority)
                    {
                        temp.Add(parcel);
                    }
                }
                return temp;
            }
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(ParcelToList parcel)
        {
            lock(dal)
            {
                if (parcel == null) { throw new IllegalActionException("Please click once on a parcel and then click delete"); }
                Parcel delParcel = ParcelDisplay(parcel.Id);
                if (delParcel.Affiliation == null)
                {
                    dal.DeleteParcel(delParcel.Id);
                }
                else
                {
                    throw new IllegalActionException("The package could not be deleted because it is in delivery mode");
                }
            }
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<IGrouping<string,ParcelToList>> GroupingSenderName()
        {
            lock(dal)
            {
                return GetParcels().GroupBy(p => p.SenderName);
            }
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<IGrouping<string, ParcelToList>> GroupingTargetNam()
        {
            lock(dal)
            {
                return GetParcels().GroupBy(p => p.TargetName);
            }
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> filterToday(int num)
        {
            lock(dal)
            {
                List<ParcelToList> parcels = new List<ParcelToList>();
                foreach (var item in GetListParcel())
                {
                    var a = (DateTime.Now - item.Creating).Value.Hours;
                    if ((DateTime.Now - item.Creating).Value.Hours < num || (DateTime.Now - item.Affiliation).Value.Hours < num || (DateTime.Now - item.PickedUp).Value.Hours < num || (DateTime.Now - item.Delivered).Value.Hours < num)
                    {
                        parcels.Add(MakeParcelToList(item));
                    }
                }
                return parcels;
            }
            
        }

    }
}

