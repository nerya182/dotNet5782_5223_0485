using IBL.BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        IBL.IBL blw;
        MainWindow main;
        bool flagClosure = true;
        public DronesListWindow(IBL.IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            blw = bl;
            WeightCategoriesSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            DronesListView.ItemsSource = blw.GetListDrone(i => true);
            main = mainWindow;
            StatusSelector.Text = "Select status";
            WeightCategoriesSelector.Text = "Select max weight";
            StatusSelector.IsEditable = true;
            WeightCategoriesSelector.IsEditable = true;
        }
        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           DronesListView.ItemsSource = blw.GetListDrone(i => true);
        }
        public void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetDronesList();
            if (StatusSelector.SelectedItem == null) return;
            IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)StatusSelector.SelectedItem;
            DronesListView.ItemsSource = from IBL.BO.DroneToList d in DronesListView.ItemsSource
                                         where d.Status == selectedStatus
                                         select d;
            if (WeightCategoriesSelector.SelectedItem == null) return;
            IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
            DronesListView.ItemsSource = from IBL.BO.DroneToList d in DronesListView.ItemsSource
                                         where d.MaxWeight == selectedWeight
                                         select d;
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow droneWindow = new DroneWindow(blw,this);
            droneWindow.Show();
            DronesListView.Items.Refresh();
            DronesListView.ItemsSource = blw.GetListDrone(i => true);
            if (WeightCategoriesSelector.SelectedItem != null)
            {
                IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
                DronesListView.ItemsSource = from IBL.BO.DroneToList d in DronesListView.ItemsSource
                                             where d.MaxWeight == selectedWeight
                                             select d;
            } 
            if (StatusSelector.SelectedItem != null)
            {
                IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)StatusSelector.SelectedItem;
                DronesListView.ItemsSource = from IBL.BO.DroneToList d in DronesListView.ItemsSource
                                             where d.Status == selectedStatus
                                             select d;
            }
        }
        private void WeightCategoriesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetDronesList();
            if (WeightCategoriesSelector.SelectedItem == null) return;
            IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
            DronesListView.ItemsSource = from IBL.BO.DroneToList d in DronesListView.ItemsSource
                                         where d.MaxWeight == selectedWeight
                                         select d;
            if (StatusSelector.SelectedItem == null) return;
            IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)StatusSelector.SelectedItem;
            DronesListView.ItemsSource = from IBL.BO.DroneToList d in DronesListView.ItemsSource
                                         where d.Status == selectedStatus
                                         select d;
        }
        public void resetDronesList_click(object sender, RoutedEventArgs e)
        {
            resetDronesList();
            resetComboBoxes();
        }
        private void resetDronesList()
        {
            DronesListView.ItemsSource = blw.GetListDrone(i => true);
            DronesListView.Items.Refresh();
        }
        public void resetComboBoxes()
        {
            WeightCategoriesSelector.SelectedItem = null;
            StatusSelector.SelectedItem = null;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flagClosure = false;
            this.Close();
            main.Show();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosed(e);
            e.Cancel = flagClosure;
        }

        private void Button_reset_Click(object sender, RoutedEventArgs e)
        {
            resetComboBoxes();
            DronesListView.ItemsSource = blw.GetListDrone(i => true);
            StatusSelector.IsEditable = true;
            WeightCategoriesSelector.IsEditable = true;
            StatusSelector.Text = "Select status";
            WeightCategoriesSelector.Text = "Select max weight";
        }

        private void DoubleClickUpdateDrone(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new DroneWindow(blw, DronesListView.SelectedItem,this).Show();
        }
    }
}
