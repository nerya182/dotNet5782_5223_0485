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
        /// <summary>
        /// constructor for adding the drone list window
        /// </summary>
        /// <param name="bl"> gives acces to the BL functions</param>
        /// <param name="mainWindow"> access to the first window</param>
        public DronesListWindow(IBL.IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            blw = bl;
            WeightCategoriesSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories)); 
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            DronesListView.ItemsSource = blw.GetListDrone();
            main = mainWindow;
            StatusSelector.Text = "Select status";
            WeightCategoriesSelector.Text = "Select max weight";
            StatusSelector.IsEditable = true;  
            WeightCategoriesSelector.IsEditable = true;
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = blw.GetListDrone();
        }
        /// <summary>
        /// user selecting the drone's status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetDronesList();
            if (StatusSelector.SelectedItem == null) return;
            IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)StatusSelector.SelectedItem;
            DronesListView.ItemsSource = blw.GetByStatus(DronesListView.ItemsSource, selectedStatus);
            if (WeightCategoriesSelector.SelectedItem == null) return;
            IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
            DronesListView.ItemsSource = blw.GetByWeight(DronesListView.ItemsSource, selectedWeight);
        }
        /// <summary>
        /// button for adding a drone - opens all the necessary fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow droneWindow = new DroneWindow(blw,this);
            droneWindow.Show();
            DronesListView.Items.Refresh();
            DronesListView.ItemsSource = blw.GetListDrone();
            if (WeightCategoriesSelector.SelectedItem != null)
            {
                IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
                DronesListView.ItemsSource = blw.GetByWeight(DronesListView.ItemsSource, selectedWeight);
            } 
            if (StatusSelector.SelectedItem != null)
            {
                IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)StatusSelector.SelectedItem;
                DronesListView.ItemsSource = blw.GetByStatus(DronesListView.ItemsSource, selectedStatus);
            }
        }
        /// <summary>
        /// user selecting the drone's weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightCategoriesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetDronesList();
            if (WeightCategoriesSelector.SelectedItem == null) return;
            IBL.BO.WeightCategories selectedWeight = (IBL.BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
            DronesListView.ItemsSource = blw.GetByWeight(DronesListView.ItemsSource, selectedWeight);
            if (StatusSelector.SelectedItem == null) return;
            IBL.BO.DroneStatuses selectedStatus = (IBL.BO.DroneStatuses)StatusSelector.SelectedItem;
            DronesListView.ItemsSource = blw.GetByStatus(DronesListView.ItemsSource, selectedStatus);
        }
        /// <summary>
        /// reseting the list of drones that are displayed to all the drones
        /// </summary>
        private void resetDronesList()
        {
            DronesListView.ItemsSource = blw.GetListDrone();
            DronesListView.Items.Refresh();
        }
        /// <summary>
        /// reseting the selections
        /// </summary>
        public void resetComboBoxes()
        {
            WeightCategoriesSelector.SelectedItem = null;
            StatusSelector.SelectedItem = null;
        }
        public void resetDronesList_click(object sender, RoutedEventArgs e)
        {
            resetDronesList();
            resetComboBoxes();
        }
        /// <summary>
        /// closing the drone list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// user clicked the reset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_reset_Click(object sender, RoutedEventArgs e)
        {
            resetComboBoxes();
            DronesListView.ItemsSource = blw.GetListDrone();
            StatusSelector.IsEditable = true;
            WeightCategoriesSelector.IsEditable = true;
            StatusSelector.Text = "Select status";
            WeightCategoriesSelector.Text = "Select max weight";
        }
        /// <summary>
        /// user double clicked on a drone to update it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleClickUpdateDrone(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new DroneWindow(blw, DronesListView.SelectedItem,this).Show();
        }
    }
}
