using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BO;

namespace PO
{
    public class PODroneToList : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged("id"); }
        }

        private string model;
        public string Model
        {
            get => model;
            set { model = value; OnPropertyChanged("model"); }
        }
        
        private WeightCategories maxweight;
        public WeightCategories MaxWeight
        {
            get => maxweight;
            set { maxweight = value; OnPropertyChanged("max weight"); }
        }

        private DroneStatuses status;
        public DroneStatuses Status
        {
            get => status;
            set { status = value; OnPropertyChanged("status"); }
        }

        private double battery;
        public double Battery
        {
            get => battery;
            set { battery = value; OnPropertyChanged("battery"); }
        }

        private Location location;
        public Location Location
        {
            get => location;
            set { location = value; OnPropertyChanged("location"); }
        }

        private int parcelBeingPassedId;
        public int ParcelBeingPassedId
        {
            get => parcelBeingPassedId;
            set { parcelBeingPassedId = value; OnPropertyChanged("parcelBeingPassedId"); }
        }

        public override string ToString()
        {
            return $"DroneId # {Id}  " +
                   $"Model: {Model}   " +
                   $"MaxWeight: {MaxWeight}  " +
                   $"Status: {Status}   \n" +
                   $"Battery:{(int)Battery}%   " +
                   $"location: {Location}   " +
                   $"parcel number being passed: {ParcelBeingPassedId}\n";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
