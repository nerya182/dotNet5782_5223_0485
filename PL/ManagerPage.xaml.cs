using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        BlApi.IBL bl;
        public ObservableCollection<DroneToList> droneToListObservabl;
        public ObservableCollection<CustomerToList> customerToListObservabl;
        public ObservableCollection<ParcelToList> parcelToListObservabl;
        public ObservableCollection<StationToList> stationToListObservabl;
        public ManagerPage()
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
        }

        private void DoubleClickUpdateDrone(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DroneToList temp = listDrones.SelectedItem as DroneToList;
            Drone drone = bl.DroneDisplay(temp.Id);
            DronePage dronePage = new DronePage(drone);
        } 


        private void TabManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = TabManager.SelectedIndex;
            switch (selected)
            {
                case 0:
                    ADD.Content = "add drone";
                    Grouping_first.Content = "Grouping of status drones";
                    Grouping_seconde.Content = "Grouping of weight drones";
                    deleteButton.Visibility = Visibility.Collapsed;
                    Grouping_first.Visibility = Visibility.Visible;
                    ComboBox_WeightSelector.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    ADD.Content = "add parcel";
                    deleteButton.Content = "delete parcel";
                    Grouping_first.Content = "Grouping of sender";
                    Grouping_seconde.Content = "Grouping of target";
                    Grouping_first.Visibility = Visibility.Visible;
                    deleteButton.Visibility = Visibility.Visible;
                    ComboBox_WeightSelector.Visibility = Visibility.Visible;
                    break;
                case 2:
                    ADD.Content = "add station";
                    Grouping_first.Content = "Grouping of charge-slots availables";
                    Grouping_seconde.Content = "Grouping of number charge-slots";
                    Grouping_first.Visibility = Visibility.Visible;
                    deleteButton.Visibility = Visibility.Collapsed;
                    ComboBox_WeightSelector.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    ADD.Content = "add customer";
                    Grouping_first.Visibility = Visibility.Hidden;
                    deleteButton.Visibility = Visibility.Collapsed;
                    ComboBox_WeightSelector.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DoubleClickUpdateStaion(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
         
        }

        private void DoubleClickUpdateCustomer(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
         
        private void DoubleClickUpdateParcel(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

      
        private void Grouping_Seconde_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Grouping_first_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void backToMain_Click(object sender, RoutedEventArgs e)
        {

        }
    }     
}
