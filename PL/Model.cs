using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BO;
using BlApi;
using System.Collections.ObjectModel;

namespace PL
{
    public class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        static readonly IBL bl = BlFactory.GetBl();

        Model() { }

       

        public static Model Instance { get; } = new Model();

        ObservableCollection<DroneToList> listDrone = new(bl.GetListDrone());
        public ObservableCollection<DroneToList> drones
        {
            get => listDrone;
            private set { listDrone = value; propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(drones))); }
        }

        DroneStatuses droneStatusesSelector = DroneStatuses.None;
        public DroneStatuses DroneStatusesSelector
        {
            get => droneStatusesSelector;
            set { droneStatusesSelector = value; DroneRefreash(); }
        }

        private void DroneRefreash()
        {
            drones = new(bl.GetListDrone().Where(d =>
               (droneWeightSelector == WeightCategories.None || droneWeightSelector == d.MaxWeight) &&
               (droneStatusesSelector == DroneStatuses.None || droneStatusesSelector == d.Status)));           
        }

        WeightCategories droneWeightSelector = WeightCategories.None;
        public WeightCategories DroneWeightSelector
        {
            get => droneWeightSelector;
            set { droneWeightSelector = value; DroneRefreash(); }
        }
        
    }
}
