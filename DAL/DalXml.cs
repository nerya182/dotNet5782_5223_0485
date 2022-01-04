using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DalApi;
using DalObject;
using DO;
namespace DalXml
{
    internal sealed class DalXml : IDal
    {
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }

        string dronesPath = @"DronesXml.xml";//XElement
        string stationPath = @"stationXml.xml";
        string parcelsPath = @"parcelsXml.xml";
        string droneChargePath = @"droneChargeXml.xml";
        string customerPath = @"customerXml.xml";
        string configPath= @"configXml.xml";

        DalXml()
        {
            //DataSource.Initialize();
            //XMLTools.SaveListToXMLSerializer<Customer>(DataSource.Customers, customerPath);
            //XMLTools.SaveListToXMLSerializer<Drone>(DataSource.Drones, dronesPath);
            //XMLTools.SaveListToXMLSerializer<Parcel>(DataSource.Parcels, parcelsPath);
            //XMLTools.SaveListToXMLSerializer<DroneCharge>(DataSource.DroneCharges, droneChargePath);
            //XMLTools.SaveListToXMLSerializer<Station>(DataSource.Stations, stationPath);
            //XMLTools.SaveListToXMLSerializer<double>(GetElectricUsage(), configPath);
        }

        public void AddDrone(Drone newDrone)
        {
            XElement droneRootElem = XMLTools.LoadListFromXMLElement(dronesPath);
            XElement New_drone = new XElement("Drone",
                                   new XElement("Id", newDrone.Id),
                                   new XElement("Model", newDrone.Model),
                                   new XElement("MaxWeight", newDrone.MaxWeight));
            droneRootElem.Add(New_drone);
            XMLTools.SaveListToXMLElement(droneRootElem, dronesPath);
        }
        public Drone GetDrone(int droneId) 
        {
            XElement droneRootElem = XMLTools.LoadListFromXMLElement(dronesPath);
            Drone? drone = (from d in droneRootElem.Elements()
                           where Convert.ToInt32(d.Element("Id").Value) == droneId
                           select new Drone()
                           {
                               Id = int.Parse(d.Element("Id").Value),
                               Model = d.Element("Model").Value,
                               MaxWeight = (WeightCategories)int.Parse(d.Element("MaxWeight").Value)
                           }).FirstOrDefault();
            if (drone == null)
                throw new ItemNotFoundException(droneId);
            return (Drone)drone;
        }
        public void Affiliate(int idParcel, int droneId)
        {
            List<Parcel> listParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            int indexaforParcel = listParcel.FindIndex(p => p.Id == idParcel);

            Parcel temp = listParcel[indexaforParcel];
            temp.DroneId = droneId;
            temp.Affiliation = DateTime.Now;
            listParcel[indexaforParcel] = temp;
            XMLTools.SaveListToXMLSerializer<Parcel>(listParcel, parcelsPath);
        }
        public void PickupParcelUpdate(int parcelId)
        {
            List<Parcel> listParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            int indexaforParcel = listParcel.FindIndex(p => p.Id == parcelId);
            Parcel temp = listParcel[indexaforParcel];
            temp.PickedUp = DateTime.Now;
            listParcel[indexaforParcel] = temp;
            XMLTools.SaveListToXMLSerializer<Parcel>(listParcel, parcelsPath);
        }
        public void SupplyParcelUpdate(int parcelId)
        {
            List<Parcel> listParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            int indexaforParcel = listParcel.FindIndex(p => p.Id == parcelId);
            Parcel temp = listParcel[indexaforParcel];
            temp.Delivered = DateTime.Now;
            listParcel[indexaforParcel] = temp;
            XMLTools.SaveListToXMLSerializer<Parcel>(listParcel, parcelsPath);
        }
        public void ReleaseDroneFromCharger(int droneId)
        {
            List<DroneCharge> list = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);

            DroneCharge droneCharge = list.FirstOrDefault(d => d.DroneId == droneId);
            XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath);
            XElement station = (from s in stationRootElem.Elements()
                              let id = int.Parse(s.Element("Id").Value)
                              where id==droneCharge.StationId
                              select s).FirstOrDefault();

            station.Element("AvailableChargeSlots").Value = (int.Parse(station.Element("AvailableChargeSlots").Value) + 1).ToString();
            list.Remove(droneCharge);
            XMLTools.SaveListToXMLSerializer<DroneCharge>(list, droneChargePath);
            XMLTools.SaveListToXMLElement(stationRootElem, stationPath);
        }
        public IEnumerable<DroneCharge> ListDroneCharge()
        {
            IEnumerable<DroneCharge> listDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            return listDroneCharge;
        }
        public IEnumerable<Drone> ListDrone(Predicate<Drone> predicate)
        {
            List<Drone> listDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
                listDrone = listDrone.FindAll(predicate);
            return listDrone;             
        }
        public  void AddDroneToCharge(DroneCharge droneCharge)
        {
            List<DroneCharge> list = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath);

            XElement station = (from s in stationRootElem.Elements()
                                let id = int.Parse(s.Element("Id").Value)
                                where id == droneCharge.StationId
                                select s).FirstOrDefault();

            station.Element("AvailableChargeSlots").Value = (int.Parse(station.Element("AvailableChargeSlots").Value) - 1).ToString();
            list.Add(droneCharge);
            XMLTools.SaveListToXMLSerializer<DroneCharge>(list, droneChargePath);
            XMLTools.SaveListToXMLElement(stationRootElem, stationPath);
        }
        public void UpdateDrone(Drone updateDrone)
        {
            XElement droneRootElem = XMLTools.LoadListFromXMLElement(dronesPath);
            XElement drone = (from d in droneRootElem.Elements()
                              let droneId = int.Parse(d.Element("Id").Value)
                              where droneId == updateDrone.Id
                              select d).FirstOrDefault();
            if (drone==null)
                throw new ItemNotFoundException(updateDrone.Id);
            drone.Element("Model").Value = updateDrone.Model.ToString();
            XMLTools.SaveListToXMLElement(droneRootElem,dronesPath);
        }
        public Station GetStation(int id)
        {
            XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath);
            Station? station = (from s in stationRootElem.Elements()
                            where Convert.ToInt32(s.Element("Id").Value) ==id
                            select new Station()
                            {
                                Id = int.Parse(s.Element("Id").Value), 
                                Name= s.Element("Name").Value,
                                Lattitude=double.Parse(s.Element("Lattitude").Value),
                                Longitude= double.Parse(s.Element("Longitude").Value),
                                AvailableChargeSlots=int.Parse(s.Element("AvailableChargeSlots").Value)
                            }).FirstOrDefault();
            if (station == null)
                throw new ItemNotFoundException(id);
            return (Station)station;
        }
        public Customer GetCustomer(int id)
        {
            XElement customerRootElem = XMLTools.LoadListFromXMLElement(customerPath);
            Customer? customer = (from c in customerRootElem.Elements()
                                where Convert.ToInt32(c.Element("Id").Value) == id
                                select new Customer()
                                {
                                    Id = int.Parse(c.Element("Id").Value),
                                    Name = c.Element("Name").Value,
                                    Phone=c.Element("Phone").Value,
                                    Lattitude = double.Parse(c.Element("Lattitude").Value),
                                    Longitude = double.Parse(c.Element("Longitude").Value),
                                }).FirstOrDefault();
            if (customer == null)
                throw new ItemNotFoundException(id);
            return (Customer)customer;
        }
        public Parcel GetParcel(int id)
        {
            Parcel parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath).FirstOrDefault(p=>p.Id==id);
            if (parcel.Id==0)
                throw new ItemNotFoundException(id);
            return (Parcel)parcel;
        }
        public void AddStation(Station newStation)
        {
            XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath);
            XElement station = (from s in stationRootElem.Elements()
                              let stationId = int.Parse(s.Element("Id").Value)
                              where stationId == newStation.Id
                              select s).FirstOrDefault();
            if (station == null)
            {

                XElement station_New = new XElement("Station",
                                       new XElement("Id", newStation.Id),
                                       new XElement("Name", newStation.Name),
                                       new XElement("Longitude",newStation.Longitude),
                                       new XElement("Lattitude", newStation.Lattitude),
                                       new XElement("AvailableChargeSlots",newStation.AvailableChargeSlots));

                stationRootElem.Add(station_New);
                XMLTools.SaveListToXMLElement(stationRootElem,stationPath);
            }
            else
                throw new ItemAlreadyExistsException(newStation.Id);
        }
        public void AddCustomer(Customer newCustomer)
        {
            XElement customerRootElem = XMLTools.LoadListFromXMLElement(customerPath);
            XElement customer = (from c in customerRootElem.Elements()
                                let customerId = int.Parse(c.Element("Id").Value)
                                where customerId == newCustomer.Id
                                select c).FirstOrDefault();
            if (customer == null)
            {
                XElement customer_New = new XElement("Customer",
                                       new XElement("Id", newCustomer.Id),
                                       new XElement("Name", newCustomer.Name),
                                       new XElement("Phone", newCustomer.Phone),
                                       new XElement("Longitude", newCustomer.Longitude),
                                       new XElement("Lattitude", newCustomer.Lattitude));

                customerRootElem.Add(customer_New);
                XMLTools.SaveListToXMLElement(customerRootElem, customerPath);
            }
            else
                throw new ItemAlreadyExistsException(newCustomer.Id);
        }
        public void AddParcel(Parcel newParcel)
        {
            XElement parcelootElem = XMLTools.LoadListFromXMLElement(parcelsPath);
            List<double> listConfig = XMLTools.LoadListFromXMLSerializer<double>(configPath);
           
            
                int CountIdPackage =(int)listConfig[5]++;
                XElement parcel_New = new XElement("Station",
                                       new XElement("Id", CountIdPackage),
                                       new XElement("SenderId", newParcel.SenderId),
                                       new XElement("TargetId", newParcel.TargetId),
                                       new XElement("DroneId", newParcel.DroneId),
                                       new XElement("Weight", newParcel.Weight),
                                       new XElement("Priority", newParcel.Priority),
                                       new XElement("Creating", newParcel.Creating),
                                       new XElement("Affiliation", newParcel.Affiliation),
                                       new XElement("PickedUp", newParcel.PickedUp),
                                       new XElement("Delivered", newParcel.Delivered));
                parcelootElem.Add(parcel_New);
                XMLTools.SaveListToXMLElement(parcelootElem,parcelsPath);
                XMLTools.SaveListToXMLSerializer<double>(listConfig, configPath);
        }
        public IEnumerable<Station> ListBaseStation(Predicate<DO.Station> predicate)
        {

            List<Station> listStation = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            listStation.FindAll(predicate);
            return listStation;
        }
        public IEnumerable<Customer> ListCustomer(Predicate<Customer> predicate)
        {
            List<Customer> listCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            listCustomer.FindAll(predicate);
            return listCustomer;
        }
        public IEnumerable<Parcel> ListParcel(Predicate<DO.Parcel> predicate)
        {
            List<Parcel> listParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath).ToList();
            listParcel.FindAll(predicate);
            return listParcel;
        }
        public  IEnumerable<Parcel> ListParcelOnAir()
        {
            List<Parcel> listParcelOnAir = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath).FindAll(p=>p.DroneId==0);
            return listParcelOnAir;
        }
        public int GetParcelId()
        {
            return DataSource.Config.NewParcelId;
        }
        public Parcel ParcelDisplay(int id)
        {
            return GetParcel(id);
        }

        public Station BaseStationDisplay(int id)
        {
            return GetStation(id);
        }
        public IEnumerable<Station> ListStationsWithOpenSlots()
        {
            List<Station> listStationsWithOpenSlots = XMLTools.LoadListFromXMLSerializer<Station>(stationPath).FindAll(s => s.AvailableChargeSlots > 0);
            return listStationsWithOpenSlots;
        }
        public Drone DroneDisplay(int id)
        {
            return GetDrone(id);
        }
        public Customer CustomerDisplay(int id)
        {
            return GetCustomer(id);
        }
        public List<double> GetElectricUsage()
        {
            List<double> list = new();
            list.Add(DataSource.Config.available);
            list.Add(DataSource.Config.lightWeight);
            list.Add(DataSource.Config.mediumWeight);
            list.Add(DataSource.Config.heavyWeight);
            list.Add(DataSource.Config.chargeSpeed);
            list.Add(DataSource.Config.NewParcelId);
            return list;
        }
        public double GetChargeSpeed()
        {
            return DataSource.Config.chargeSpeed;
        }
        public double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = Deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = Deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }
        public double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        public bool CheckId(int id)
        {
            int number = id / 10;
            int s = 0, kefel = 0, SumOfDigitsOfProduct = 0;
            for (int i = 1; i <= 8; i++)
            {
                if (i % 2 == 0)
                    kefel = (number % 10);
                else
                    kefel = (number % 10) * 2;
                number /= 10;
                SumOfDigitsOfProduct = sumDigits(kefel);
                s += SumOfDigitsOfProduct;
            }
            if (s % 10 == 0)
                return 0 == id % 10;
            else
                return (10 - s % 10) == id % 10;
        }
        private int sumDigits(int num)
        {
            int s = 0;
            while (num > 0)
            {
                s = s + num % 10;
                num /= 10;
            }
            return s;
        }
        public int AvailableChargeSlotsInStation(int id)
        {
            List<DroneCharge> listDroneChrage = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            listDroneChrage.FindAll(d => d.StationId == id);
            return listDroneChrage.Count;
        }
        public double GetElectricUsageNumber(WeightCategories weight)
        {
            if (weight == (WeightCategories)1) return DataSource.Config.lightWeight;

            if (weight == (WeightCategories)2) return DataSource.Config.mediumWeight;

            if (weight == (WeightCategories)3) return DataSource.Config.heavyWeight;

            return DataSource.Config.heavyWeight;
        }
        public void UpdateCustomer(Customer updateCustomer)
        {
            XElement customerRootElem = XMLTools.LoadListFromXMLElement(customerPath);
            XElement customer = (from c in customerRootElem.Elements()
                               let id = int.Parse(c.Element("Id").Value)
                               where id == updateCustomer.Id
                               select c).FirstOrDefault();
            if (updateCustomer.Name != "")
            {
                customer.Element("Name").Value = updateCustomer.Name;
            }
            if (updateCustomer.Phone != "")
            {
                customer.Element("Phone").Value = updateCustomer.Phone;
            }
            XMLTools.SaveListToXMLElement(customerRootElem, customerPath);
        }
        public void UpdateStation(Station updateStation)
        {
            XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath);
            XElement station = (from s in stationRootElem.Elements()
                                 let id = int.Parse(s.Element("Id").Value)
                                 where id ==updateStation.Id
                                 select s).FirstOrDefault();

            if (updateStation.Name != "") station.Element("Name").Value = updateStation.Name;
            if (updateStation.AvailableChargeSlots != 0)
            {
                if (updateStation.AvailableChargeSlots - AvailableChargeSlotsInStation(updateStation.Id) < 0)
                    throw new IllegalActionException("The total amount of charging stations is invalid\n");
                else
                    station.Element("AvailableChargeSlots").Value= 
                       (updateStation.AvailableChargeSlots - AvailableChargeSlotsInStation(updateStation.Id)).ToString();
            }
             XMLTools.SaveListToXMLElement(stationRootElem, stationPath);
        }
        public void DeleteParcel(int id)
        {
            List<Parcel> listParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            Parcel parcel = GetParcel(id);
            listParcel.Remove(parcel);
            XMLTools.SaveListToXMLSerializer<Parcel>(listParcel, parcelsPath);
        }
        public void DeleteStation(int id)
        {
            List<Station> listStation = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            Station station = GetStation(id);
            listStation.Remove(station);
            XMLTools.SaveListToXMLSerializer<Station>(listStation, stationPath);
        }
        public void DeleteCustomer(int id)
        {
            List<Customer> listCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer customer = GetCustomer(id);
            listCustomer.Remove(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(listCustomer,customerPath);
        }
    }

}
