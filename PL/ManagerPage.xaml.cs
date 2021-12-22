using System;
using System.Collections.Generic;
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
        MainWindow mainWindow;
        object selectedTab;
        public ManagerPage(MainWindow main)
        {
            InitializeComponent();
            bl= BlApi.BlFactory.GetBl();
            mainWindow = main;
            listDrones.ItemsSource = bl.GetListDrone();
            listCustomers.ItemsSource = bl.GetListCustomer();
            listStaions.ItemsSource = bl.GetStations();
            listParcel.ItemsSource = bl.GetParcels();
        }

       
        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            selectedTab = TabManager.SelectedIndex;
            switch (selectedTab)
            {
                case 0:
                    DronePage dronePage = new DronePage(this,mainWindow);
                    mainWindow.Content = dronePage;
                    break;
                case 1:
                    parcelPage parcelPage = new parcelPage(this, mainWindow);
                    mainWindow.Content = parcelPage;
                    break;
                case 2:
                    StationPage stationPage = new StationPage(this,mainWindow);
                    mainWindow.Content = stationPage;
                    break;
                case 3:
                    CustomerPage customerPage = new CustomerPage(this, mainWindow);
                    mainWindow.Content = customerPage;
                    break;
                default:
                    break;
            }
        }

        public void FilterRefreshStaions()
        {
            listStaions.ItemsSource = bl.GetStations();
            listStaions.Items.Refresh();
        }
        public void FilterRefreshDrones()
        {
            listDrones.ItemsSource = bl.GetListDrone();
            listDrones.Items.Refresh();
        }
        public void FilterRefreshCustomres()
        {
            listCustomers.ItemsSource = bl.GetListCustomer();
            listCustomers.Items.Refresh();
        }
        public void FilterRefreshParcels()
        {
            listParcel.ItemsSource = bl.GetParcels();
            listParcel.Items.Refresh();
        }

        private void DoubleClickUpdateDrone(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DronePage dronePage = new DronePage(listDrones.SelectedItem, this,mainWindow);
            mainWindow.Content = dronePage;
        }

        private void display_Click(object sender, RoutedEventArgs e)
        {
            var selected = TabManager.SelectedIndex;
            switch (selected)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    
                    break;
                default:
                    break;
            }
        }

        private void TabManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = TabManager.SelectedIndex;
            switch (selected)
            {
                case 0:
                    displayButton.Content = "display parcel in drone";
                    deleteButton.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    displayButton.Content = "parcel";
                    deleteButton.Content = "delete parcel";
                    deleteButton.Visibility = Visibility.Visible;
                    break;
                case 2:
                    displayButton.Content = "display drone in list charging drone";
                    deleteButton.Content = "delete station";
                    deleteButton.Visibility = Visibility.Visible;
                    break;
                case 3:
                    displayButton.Content = "customer";
                    deleteButton.Content = "delete customer";
                    deleteButton.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var selected = TabManager.SelectedIndex;
            object selectedDelete;
            switch (selected)
            {
                case 0:
                    selectedDelete =listDrones.SelectedItem;
                    break;
                case 1:
                    selectedDelete = listParcel.SelectedItem;
                    break;
                case 2:
                    selectedDelete = listStaions.SelectedItem;
                    break;
                case 3:
                    selectedDelete = listCustomers.SelectedItem;
                    break;
                default:
                    break;
            }
        }

        private void DoubleClickUpdateStaion(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StationPage dronePage = new StationPage(mainWindow,listStaions.SelectedItem,this);
            mainWindow.Content = dronePage;
        }

        private void DoubleClickUpdateCustomer(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CustomerPage dronePage = new CustomerPage(mainWindow,listCustomers.SelectedItem, this);
            mainWindow.Content = dronePage;
        }

        private void DoubleClickUpdateParcel(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            parcelPage parcelPage = new parcelPage(mainWindow, listParcel.SelectedItem, this);
            mainWindow.Content = parcelPage;
        }
    }
}
