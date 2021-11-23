using System;
using System.Collections.Generic;
using System.Linq;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        private IDAL.DO.Station GetClosestCustomer(IDAL.DO.Customer customerSender)
        {
            List<IDAL.DO.Station> stations = dal.ListBaseStation().ToList();
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

        public void AddCustomer(Customer newCustomer)
        {
            IDAL.DO.Customer temp = new IDAL.DO.Customer();

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

        public IEnumerable<Customer> GetListCustomer()
        {
            IEnumerable<IDAL.DO.Customer> customers = dal.ListCustomer();
            List<Customer> temp = new List<Customer>();
            foreach (var customer in customers)
            {
                Customer obj = CustomerDisplay(customer.Id);
                temp.Add(obj);
            }
            return temp;
        }

        public CustomerToList MakeCustomerToList(Customer objCustomer)
        {
            CustomerToList customerToList = new CustomerToList();
            customerToList.Id = objCustomer.Id;
            customerToList.Name = objCustomer.Name;
            customerToList.Phone = objCustomer.Phone;
            int count1 = 0;
            foreach (ParceltAtCustomer parceltAtCustomer in objCustomer.ToCustomer)
            {
                if (parceltAtCustomer.status == ParcelStatus.Supplied)
                    count1++;
            }
            customerToList.ReceivedParcels = count1;
            customerToList.OnTheWayParcels = objCustomer.ToCustomer.Count - count1;
            int count2 = 0;
            foreach (ParceltAtCustomer parceltAtCustomer1 in objCustomer.FromCustomer)
            {
                if (parceltAtCustomer1.status == ParcelStatus.Supplied)
                    count2++;
            }
            customerToList.DeliveredSuppliedParcels = count2;
            customerToList.DeliveredNotSuppliedParcels = objCustomer.FromCustomer.Count - count2;
            return customerToList;
        }

        private double GetElectricUsageNumber(IDAL.DO.WeightCategories weight)
        {
            if (weight == IDAL.DO.WeightCategories.Light) { return LightElec; }
            if (weight == IDAL.DO.WeightCategories.Medium) { return IntermeduateElec; }
            if (weight == IDAL.DO.WeightCategories.Heavy) { return HeavyElec; }

            return AvailbleElec;
        }

        public Customer CustomerDisplay(int id)
        {
            IDAL.DO.Customer customer = dal.GetCustomer(id);
            IEnumerable<IDAL.DO.Parcel> parcels = dal.ListParcel();
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
                if ((parcel.SenderId == customer.Id) && (parcel.Delivered == DateTime.MinValue))
                {
                    ParceltAtCustomer parcelAtCstmr = new ParceltAtCustomer();
                    parcelAtCstmr.Id = parcel.Id;
                    parcelAtCstmr.status = ParcelStatus.Created;
                    parcelAtCstmr.Weight = (WeightCategories)parcel.Weight;
                    parcelAtCstmr.Priority = (Priorities)parcel.Priority;
                    IDAL.DO.Customer cstmr = dal.GetCustomer(parcel.TargetId);
                    cstmrInPrcl.Id = cstmr.Id;
                    cstmrInPrcl.Name = cstmr.Name;
                    parcelAtCstmr.OpposingSide = cstmrInPrcl;
                    lstSending.Add(parcelAtCstmr);
                }
                else if ((parcel.TargetId == customer.Id) && (parcel.Delivered != DateTime.MinValue))
                {
                    ParceltAtCustomer parcelAtCstmr = new ParceltAtCustomer();
                    parcelAtCstmr.Id = parcel.Id;
                    parcelAtCstmr.status = ParcelStatus.Supplied;
                    parcelAtCstmr.Weight = (WeightCategories)parcel.Weight;
                    parcelAtCstmr.Priority = (Priorities)parcel.Priority;
                    IDAL.DO.Customer cstmr = dal.GetCustomer(parcel.SenderId);
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

        public void UpdateCustomer(Customer updateCustomer)
        {

            IDAL.DO.Customer customer = dal.GetCustomer(updateCustomer.Id);
            if (updateCustomer.Name == "" && updateCustomer.Phone == "")
            {
                throw new IllegalActionException("Must update at least one feature");
            } ;
            char[] stringArray = updateCustomer.Phone.ToCharArray();
            if (stringArray.Length!=10||stringArray[0]!='0'||stringArray[1]!='5')
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
          
}
        
  
