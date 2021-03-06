using System;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL : IBL
    {
        /// <summary>
        /// Returns the closest station to the customer
        /// </summary>
        /// <param name="customerSender"> customer which we are looking for the cloosest station to</param>
        /// <returns> station that is closest to customer</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private DO.Station GetClosestCustomer(DO.Customer customerSender)
        {
            lock(dal)
            {
                List<DO.Station> stations = dal.ListBaseStation(i => true).ToList();
                Location closestStation = new Location();
                closestStation.Lattitude = stations[0].Lattitude;
                closestStation.Longitude = stations[0].Longitude;
                int i = 0, index = 0;
                foreach (var station in stations)
                {
                    if (dal.GetDistanceFromLatLonInKm(customerSender.Lattitude, customerSender.Longitude, station.Lattitude, station.Longitude) <
                        dal.GetDistanceFromLatLonInKm(customerSender.Lattitude, customerSender.Longitude, closestStation.Lattitude, closestStation.Longitude))
                    {
                        index = i;
                        closestStation.Lattitude = customerSender.Lattitude;
                        closestStation.Longitude = customerSender.Longitude;
                    }
                    i++;
                }
                return stations[index];
            }
        }

        /// <summary>
        /// Adding a customer to our list of customers in data source
        /// </summary>
        /// <param name="newCustomer"> Customer of type IBL BO</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer newCustomer)
        {
            lock(dal)
            {
                DO.Customer temp = new DO.Customer();

                if (newCustomer.Location.Lattitude > 90 || newCustomer.Location.Lattitude < -90)
                {
                    throw new IllegalActionException("Invalid lattitude value");
                }
                if (newCustomer.Location.Longitude > 180 || newCustomer.Location.Longitude < -180)
                {
                    throw new IllegalActionException("Invalid longitude value");
                }
                if (!dal.CheckId(newCustomer.Id))
                {
                    throw new IllegalActionException("Incorrect ID number");
                }
                char[] stringArray = newCustomer.Phone.ToCharArray();
                if (stringArray.Length != 10 || stringArray[0] != '0' || stringArray[1] != '5')
                {
                    throw new IllegalActionException("Invalid cell phone number");
                }
                try
                {
                    temp.Id = newCustomer.Id;
                    temp.Name = newCustomer.Name;
                    temp.Phone = newCustomer.Phone;
                    temp.Lattitude = newCustomer.Location.Lattitude;
                    temp.Longitude = newCustomer.Location.Longitude;
                    dal.AddCustomer(temp);
                }
                catch (Exception e)
                {
                    throw new ItemAlreadyExistsException(temp.Id, "Enter a new customer number\n", e);
                }
            }
        }
        /// <summary>
        /// Retrieving our list of customers of type IBL BO
        /// </summary>
        /// <returns> IEnumeravle of customers</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetListCustomer()
        {
            lock(dal)
            {
                IEnumerable<DO.Customer> customers = dal.ListCustomer(i => true);
                List<CustomerToList> temp = new List<CustomerToList>();
                foreach (var customer in customers)
                {
                    Customer obj = CustomerDisplay(customer.Id);
                    CustomerToList cus = MakeCustomerToList(obj);
                    temp.Add(cus);
                }
                return temp;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetListCustomers()
        {
            lock(dal)
            {
                IEnumerable<DO.Customer> customers = dal.ListCustomer(i => true);
                List<Customer> temp = new List<Customer>();
                foreach (var customer in customers)
                {
                    Customer obj = CustomerDisplay(customer.Id);
                    temp.Add(obj);
                }
                return temp;
            }
        }
        /// <summary>
        /// Retrieving the info needed to transform the 'customer' into a 'customertolist'
        /// </summary>
        /// <param name="objCustomer"> customer to be transformed</param>
        /// <returns> customertolist after we've found necessary info </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public CustomerToList MakeCustomerToList(Customer objCustomer)
        {
            CustomerToList customerToList = new CustomerToList();
            customerToList.Id = objCustomer.Id;
            customerToList.Name = objCustomer.Name;
            customerToList.Phone = objCustomer.Phone;
            int count1 = 0;
            if (objCustomer.ToCustomer.Count!=null)
            {
                foreach (ParceltAtCustomer parcelAtCustomer in objCustomer.ToCustomer)
                {
                    if (parcelAtCustomer.status == ParcelStatus.Supplied)
                        count1++;
                }
            }
            
            customerToList.ReceivedParcels = count1;
            customerToList.OnTheWayParcels = objCustomer.ToCustomer.Count - count1;
            int count2 = 0;
            if (objCustomer.FromCustomer.Count!=0)
            {
                foreach (ParceltAtCustomer parcelAtCustomer1 in objCustomer.FromCustomer)
                {
                    if (parcelAtCustomer1.status == ParcelStatus.Supplied)
                        count2++;
                }
            }
            customerToList.DeliveredSuppliedParcels = count2;
            customerToList.DeliveredNotSuppliedParcels = objCustomer.FromCustomer.Count - count2;
            return customerToList;
        }
        /// <summary>
        /// Retrieving Electric usage number
        /// </summary>
        /// <param name="weight"> Weight </param>
        /// <returns> Electric usage number</returns>
        private double GetElectricUsageNumber(DO.WeightCategories weight)
        {
            if (weight == DO.WeightCategories.Light) { return LightElec; }
            if (weight == DO.WeightCategories.Medium) { return IntermeduateElec; }
            if (weight == DO.WeightCategories.Heavy) { return HeavyElec; }
            return AvailbleElec;
        }

        /// <summary>
        /// Returning a customer of type IBL BO according to ID in order to display
        /// </summary>
        /// <param name="id"> Customer ID</param>
        /// <returns> Customer to be displayed</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer CustomerDisplay(int id)
        {
           lock(dal)
            {
                try
                {
                    DO.Customer customer = dal.GetCustomer(id);
                    IEnumerable<DO.Parcel> parcels = dal.ListParcel(i => true);
                    List<ParceltAtCustomer> lstSending = new List<ParceltAtCustomer>();
                    List<ParceltAtCustomer> lstReceived = new List<ParceltAtCustomer>();

                    CustomerInParcel cstmrInPrcl = new CustomerInParcel();

                    Customer temp = new Customer();
                    Location location = new Location();
                    location.Longitude = customer.Longitude;
                    location.Lattitude = customer.Lattitude;
                    temp.Id = customer.Id;
                    temp.Name = customer.Name;
                    temp.Phone = customer.Phone;
                    temp.Location = location;

                    foreach (var parcel in parcels)
                    {
                        if ((parcel.SenderId == customer.Id) && (parcel.Delivered == null))
                        {
                            ParceltAtCustomer parcelAtCstmr = new ParceltAtCustomer();
                            parcelAtCstmr.Id = parcel.Id;
                            parcelAtCstmr.status = ParcelStatus.Created;
                            parcelAtCstmr.Weight = (WeightCategories)parcel.Weight;
                            parcelAtCstmr.Priority = (Priorities)parcel.Priority;
                            DO.Customer cstmr = dal.GetCustomer(parcel.TargetId);
                            cstmrInPrcl.Id = cstmr.Id;
                            cstmrInPrcl.Name = cstmr.Name;
                            parcelAtCstmr.OpposingSide = cstmrInPrcl;
                            lstSending.Add(parcelAtCstmr);
                        }
                        else if ((parcel.TargetId == customer.Id) && (parcel.Delivered != null))
                        {
                            ParceltAtCustomer parcelAtCstmr = new ParceltAtCustomer();
                            parcelAtCstmr.Id = parcel.Id;
                            parcelAtCstmr.status = ParcelStatus.Supplied;
                            parcelAtCstmr.Weight = (WeightCategories)parcel.Weight;
                            parcelAtCstmr.Priority = (Priorities)parcel.Priority;
                            DO.Customer cstmr = dal.GetCustomer(parcel.SenderId);
                            cstmrInPrcl.Id = cstmr.Id;
                            cstmrInPrcl.Name = cstmr.Name;
                            parcelAtCstmr.OpposingSide = cstmrInPrcl;
                            lstReceived.Add(parcelAtCstmr);
                        }
                    }
                    temp.FromCustomer = lstSending;
                    temp.ToCustomer = lstReceived;
                    return temp;
                }
                catch (Exception e)
                {
                    throw new ItemNotFoundException(id, "Enter an existing customer in the system", e);
                }
            }
        }

        /// <summary>
        /// Updating the customer's info
        /// </summary>
        /// <param name="updateCustomer">Customer to be updated </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(CustomerToList updateCustomer)
        {
            lock(dal)
            {
                DO.Customer customer = dal.GetCustomer(updateCustomer.Id);
                if (updateCustomer.Name == "" && updateCustomer.Phone == "")
                {
                    throw new IllegalActionException("Must update at least one feature");
                }
                char[] stringArray = updateCustomer.Phone.ToCharArray();
                if (stringArray.Length != 10 || stringArray[0] != '0' || stringArray[1] != '5')
                {
                    throw new IllegalActionException("Invalid cell phone number");
                }
                try
                {
                    if (updateCustomer.Name != "")
                    {
                        customer.Name = updateCustomer.Name;

                    }
                    if (updateCustomer.Phone != "")
                    {
                        customer.Phone = updateCustomer.Phone;
                    }
                    dal.UpdateCustomer(customer);
                }
                catch (Exception e)
                {
                    throw new ItemNotFoundException(updateCustomer.Id, "Enter an existing customer in the system", e);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]public void DeleteCustomer(CustomerToList cus)
        {
            lock(dal)
            {
                if (cus == null) { throw new IllegalActionException("Please click once on a customer and then click delete"); }
                Customer customer = CustomerDisplay(cus.Id);
                bool flag = true;
                int count = customer.FromCustomer.Count;
                for (int i = 0; i < count; i++)
                {
                    if (customer.FromCustomer[i].status != ParcelStatus.Supplied)
                    {
                        flag = false;
                        break;
                    }
                    if (customer.ToCustomer[i].status != ParcelStatus.Supplied)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag == true)
                    dal.DeleteCustomer(customer.Id);
                else
                {
                    throw new IllegalActionException("The customer cannot be deleted because it is in use on the system");
                }
            }
        }
    }
          
}
        
  
