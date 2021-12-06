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
        private MainWindow main;
        public DronesListWindow(IBL.IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            blw = bl;
            WeightCategoriesSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            DronesListView.ItemsSource = blw.GetListDrone(i => true);
            main = mainWindow;
        }
        //private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
       // {
           //DronesListView.ItemsSource = blw.GetListDrone(i => true);
       // }
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            new DroneWindow(blw).Show();
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
        }
        private void resetComboBoxes()
        {
            WeightCategoriesSelector.SelectedItem = null;
            StatusSelector.SelectedItem = null;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            main.Show();
        }

    }
}
