using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.IBL bldw;
        IBL.BO.DroneToList selected = new IBL.BO.DroneToList();
        IBL.BO.Drone droneSelected = new IBL.BO.Drone();
        DronesListWindow droneListWin;
        bool flagClosure = true;
        public DroneWindow(IBL.IBL blw, DronesListWindow w)
        {
            InitializeComponent();
            bldw = blw;
            droneListWin = w;
            close_button.Visibility = Visibility.Hidden;
            listViewUpdate.Visibility = Visibility.Hidden;
            UpdateOptions.Visibility = Visibility.Hidden;
            ComboBoxUpdateOptions.Visibility = Visibility.Hidden;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            chargeStationId.ItemsSource = from IBL.BO.Station s in bldw.GetListStation(i => true)
                                          select s.Id;
            StatusAdd.ItemsSource = from DroneStatuses ds in Enum.GetValues(typeof(DroneStatuses))
                                    where ds == DroneStatuses.Charging
                                    select ds;
            WeightSelector.Text = "Select max weight";
            chargeStationId.Text = "select BaseStation";
            StatusAdd.Text = "select status";
            chargeStationId.IsEditable = true;
            WeightSelector.IsEditable = true;
            StatusAdd.IsEditable = true;
            TextBoxDelivery.IsEnabled = false;
            TextBoxDelivery.Text = "0";
            TextBoxLattitude.IsEnabled = false;
            TextBoxLongitude.IsEnabled = false;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int chargingStationId = 0;
                IBL.BO.DroneToList newDrone = new IBL.BO.DroneToList();
                newDrone.Id = int.Parse(TextBox_id.Text);
                newDrone.Model = TextBox_model.Text;
                newDrone.MaxWeight = (WeightCategories)WeightSelector.SelectedItem;
                chargingStationId = (int)chargeStationId.SelectedItem;
                TextBoxLattitude.Text = bldw.BaseStationDisplay(chargingStationId).location.Lattitude.ToString();
                TextBoxLongitude.Text = bldw.BaseStationDisplay(chargingStationId).location.Longitude.ToString();
                bldw.AddDrone(newDrone, chargingStationId);
                droneListWin.DronesListView.ItemsSource = bldw.GetListDrone(i => true);
                MessageBox.Show("Added successfully");
                TextBox_id.IsEnabled = false;
                chargeStationId.IsEnabled = false;
                TextBox_model.IsEnabled = false;
                WeightSelector.IsEnabled = false;
                add_button.IsEnabled = false;
                flagClosure = false;
                this.Close();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        private void Cancel_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            flagClosure = false;
            this.Close();
        }
        public DroneWindow(IBL.IBL blw, object selectedItem, DronesListWindow w)
        {
            droneListWin = w;
            InitializeComponent();
            bldw = blw;
            close_button.Visibility = Visibility.Visible;
            label_id.Visibility = Visibility.Hidden;
            label_model.Visibility = Visibility.Hidden;
            WeightSelector.Visibility = Visibility.Hidden;
            label_weight.Visibility = Visibility.Hidden;
            TextBox_id.Visibility = Visibility.Hidden;
            TextBox_model.Visibility = Visibility.Hidden;
            label_charge_Station_id.Visibility = Visibility.Hidden;
            add_button.Visibility = Visibility.Hidden;
            statusch.Visibility = Visibility.Hidden;
            StatusAdd.Visibility = Visibility.Hidden;
            Delivery.Visibility = Visibility.Hidden;
            TextBoxDelivery.Visibility = Visibility.Hidden;
            Lattitude.Visibility = Visibility.Hidden;
            TextBoxLattitude.Visibility = Visibility.Hidden;
            Longitud.Visibility = Visibility.Hidden;
            TextBoxLongitude.Visibility = Visibility.Hidden;
            chargeStationId.Visibility = Visibility.Hidden;
            Cancel_add_button.Visibility = Visibility.Hidden;
            Cancel_add_button.Visibility = Visibility.Hidden;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            selected = (DroneToList)selectedItem;
            droneSelected = bldw.DroneDisplay(selected.Id);
            ComboBoxUpdateOptions.Text = "Select Update";
            listViewUpdate.ItemsSource = from IBL.BO.Drone d in bldw.GetDrones()
                                         where d.Id == selected.Id
                                         select d;
            ComboBoxUpdateOptions.Items.Add("drone model");
            ComboBoxUpdateOptions.Items.Add("Sending a drone for charging");
            ComboBoxUpdateOptions.Items.Add("Release drone from charging");
            ComboBoxUpdateOptions.Items.Add("Sending the drone for delivery");
            ComboBoxUpdateOptions.Items.Add("Package collection");
            ComboBoxUpdateOptions.Items.Add("Package delivery");
        }

        private void ComboBoxUpdateOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedView = ComboBoxUpdateOptions.SelectedIndex;
            int droneId;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            switch (selectedView)
            {

                case 0:
                    TextBoxNewModel.Visibility = Visibility.Visible;
                    labelTextBoxNewModel.Visibility = Visibility.Visible;
                    NewModel.Visibility = Visibility.Visible;
                    break;
                case 1:
                    try
                    {
                        droneId = selected.Id;
                        bldw.SendingDroneForCharging(droneId);
                        MessageBox.Show("Update successfully");
                        listViewUpdate.ItemsSource = bldw.GetDrones().Where(i=>i.Id==droneId);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 2:
                    try
                    {
                        droneId = selected.Id;
                        bldw.ReleaseDroneFromCharging(droneId);
                        MessageBox.Show("Update successfully");
                        listViewUpdate.ItemsSource = bldw.GetDrones().Where(i => i.Id == droneId);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 3:
                    try
                    {
                        droneId = selected.Id;
                        bldw.AffiliateParcelToDrone(droneId);
                        MessageBox.Show("Update successfully");
                        listViewUpdate.ItemsSource = bldw.GetDrones().Where(i => i.Id == droneId);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 4:
                    try
                    {
                        droneId = selected.Id;
                        bldw.ParcelCollectionByDrone(droneId);
                        MessageBox.Show("Update successfully");
                        listViewUpdate.ItemsSource = bldw.GetDrones().Where(i => i.Id == droneId);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 5:
                    try
                    {
                        droneId = selected.Id;
                        bldw.DeliveryOfParcelByDrone(droneId);
                        MessageBox.Show("Update successfully");
                        listViewUpdate.ItemsSource = bldw.GetDrones().Where(i => i.Id == droneId);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                default:
                    break;
            }
            droneListWin.DronesListView.ItemsSource= bldw.GetListDrone(i => true);
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            IBL.BO.DroneToList updateDrone = new IBL.BO.DroneToList();
            try
            {
                updateDrone.Model = TextBoxNewModel.Text;
                updateDrone.Id = selected.Id;
                bldw.UpdateDrone(updateDrone);
                MessageBox.Show("Update successfully");
                TextBoxNewModel.Visibility = Visibility.Hidden;
                labelTextBoxNewModel.Visibility = Visibility.Hidden;
                NewModel.Visibility = Visibility.Hidden;
                listViewUpdate.ItemsSource = bldw.GetDrones().Where(i => i.Id == selected.Id);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            flagClosure = false;
            this.Close();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosed(e);
            e.Cancel = flagClosure;
        }
    }
}
