using BO;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for DronePage.xaml
    /// </summary>
    public partial class DronePage : Page
    {
        BlApi.IBL bl;
        BO.DroneToList selected = new BO.DroneToList();
        BO.Drone droneSelected = new BO.Drone();
        MainWindow mainWindow;
        ManagerPage managerPage;

        /// <summary>
        /// constructor for add drone  window
        /// </summary>
        /// <param name="blw"> gives access to the BL functions</param>
        /// <param name="w"> gives access to the previous window</param>
        public DronePage(ManagerPage manager,MainWindow main)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            mainWindow = main;
            managerPage = manager;
            TextBoxParcelTransfer.Visibility = Visibility.Hidden;
            WeightTextBox.Visibility = Visibility.Hidden;
            label_id.Content = "Enter ID Number:";
            changeModelButton.Visibility = Visibility.Hidden;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            sendOrReleaseButton.Visibility = Visibility.Hidden;
            delivery.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            chargeStationId.ItemsSource = from BO.Station s in bl.GetListStation()
                                          where s.AvailableChargeSlots > 0
                                          select s.Id;
            WeightSelector.Text = "Select max weight";
            chargeStationId.Text = "select BaseStation";
            chargeStationId.IsEditable = true;
            WeightSelector.IsEditable = true;
            TextBoxDelivery.IsEnabled = false;
            TextBoxDelivery.Text = "0";
            TextBoxLattitude.IsEnabled = false;
            TextBoxLongitude.IsEnabled = false;
        }
        /// <summary>
        /// user has added a drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag;
                int id;
                int chargingStationId = 0;
                BO.DroneToList newDrone = new BO.DroneToList();
                flag = int.TryParse(TextBox_id.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error ,drone id was not entered ");
                    return;
                }
                newDrone.Id = int.Parse(TextBox_id.Text);
                if (TextBox_model.Text == null)
                {
                    MessageBox.Show("error ,model drone was not entered ");
                    return;
                }
                newDrone.Model = TextBox_model.Text;
                if (WeightSelector.SelectedItem == null)
                {
                    MessageBox.Show("error ,weight was not entered ");
                    return;
                }
                newDrone.MaxWeight = (WeightCategories)WeightSelector.SelectedItem;
                if (chargeStationId.SelectedItem == null)
                {
                    MessageBox.Show("error ,charge station id was not entered ");
                    return;
                }
                chargingStationId = (int)chargeStationId.SelectedItem;
                TextBoxLattitude.Text = bl.BaseStationDisplay(chargingStationId).location.Lattitude.ToString();
                TextBoxLongitude.Text = bl.BaseStationDisplay(chargingStationId).location.Longitude.ToString();
                bl.AddDrone(newDrone, chargingStationId);
                managerPage.listDrones.Items.Refresh();
                MessageBox.Show("Added successfully");
                TextBox_id.IsEnabled = false;
                chargeStationId.IsEnabled = false;
                TextBox_model.IsEnabled = false;
                WeightSelector.IsEnabled = false;
                add_button.IsEnabled = false;
                managerPage.FilterRefreshDrones();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        /// <summary>
        /// user would like to cancel the add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerPage page = new ManagerPage(mainWindow);
            mainWindow.Content = page;
            page.TabManager.SelectedIndex = 0;
            managerPage.TabManager.SelectedIndex = 0;
        }
        /// <summary>
        /// constructor for display drone window
        /// </summary>
        /// <param name="blw">gives access to the BL functions</param>
        /// <param name="selectedItem"></param>
        /// <param name="w">gives access to the previous window</param>
        public DronePage(object selectedItem, ManagerPage manager, MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            managerPage = manager;
            bl = BlApi.BlFactory.GetBl();
            label_id.Content = "ID Number:";
            label_model.Content = "Model:";
            WeightSelector.Visibility = Visibility.Hidden;
            label_weight.Content = "Weight:";
            label_charge_Station_id.Content = "Battery %:";
            add_button.Visibility = Visibility.Hidden;
            Delivery.Content = "Status:";
            TextBoxLattitude.FontSize = 10;
            Lattitude.Content = "Location: ";
            Longitud.Content = "Parcel Tranfer:";
            chargeStationId.Visibility = Visibility.Hidden;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            selected = (DroneToList)selectedItem;
            droneSelected = bl.DroneDisplay(selected.Id);
            TextBox_id.Text = droneSelected.Id.ToString();
            WeightTextBox.Text = droneSelected.MaxWeight.ToString();
            TextBox_model.Text = droneSelected.Model;
            TextBoxLattitude.Text = droneSelected.Location.ToString();
            TextBoxDelivery.Text = droneSelected.Status.ToString();
            TextBoxLongitude.Text = droneSelected.ParcelTransfer.ToString();
            TextBoxParcelTransfer.Text = droneSelected.Battery.ToString();
            TextBoxLongitude.Width = 300;
            TextBoxLongitude.Height = 100;
            TextBox_id.IsEnabled = false;
            TextBox_model.IsEnabled = false;
            WeightTextBox.IsEnabled = false;
            TextBoxDelivery.IsEnabled = false;
            TextBoxParcelTransfer.IsEnabled = false;
            TextBoxLattitude.IsEnabled = false;
            TextBoxLongitude.IsEnabled = false;
            TextBoxLongitude.FontSize = 10;
            switch (droneSelected.Status)
            {
                case DroneStatuses.Available:
                    changeModelButton.Visibility = Visibility.Visible;
                    sendOrReleaseButton.Content = "Sending to charging";
                    sendOrReleaseButton.Visibility = Visibility.Visible;
                    delivery.Content = "Affiliation";
                    delivery.Visibility = Visibility.Visible;
                    break;
                case DroneStatuses.Charging:
                    changeModelButton.Visibility = Visibility.Visible;
                    sendOrReleaseButton.Content = "Release drone";
                    sendOrReleaseButton.Visibility = Visibility.Visible;
                    delivery.Visibility = Visibility.Hidden;
                    break;
                case DroneStatuses.Delivery:
                    changeModelButton.Visibility = Visibility.Visible;
                    Parcel parcel = bl.GetListParcel().First(i => i.Id == selected.ParcelBeingPassedId);
                    if (parcel.PickedUp == null)
                    {
                        sendOrReleaseButton.Content = "Package collection";
                        sendOrReleaseButton.Visibility = Visibility.Visible;
                    }
                    else if (parcel.Delivered == null)
                    {
                        sendOrReleaseButton.Content = "Package delivery";
                        sendOrReleaseButton.Visibility = Visibility.Visible;
                    }
                    delivery.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// button that opens all the update options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            BO.DroneToList updateDrone = selected;
            try
            {
                updateDrone.Model = TextBoxNewModel.Text;
                bl.UpdateDrone(updateDrone);
                MessageBox.Show("Update successfully");
                TextBox_model.Text = TextBoxNewModel.Text;
                TextBoxNewModel.Visibility = Visibility.Hidden;
                labelTextBoxNewModel.Visibility = Visibility.Hidden;
                NewModel.Visibility = Visibility.Hidden;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            managerPage.listDrones.ItemsSource = bl.GetListDrone();
        }
        /// <summary>
        /// closing the current window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      
       
        /// <summary>
        /// model of drone has been entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            string text = TextBox_model.Text;
            if (text != null && text != "" && char.IsLetter(text.ElementAt(0)))
            {
                TextBox_model.BorderBrush = (Brush)bc.ConvertFrom("#FF99B4D1");
                TextBox_model.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
            else TextBox_model.Background = (Brush)bc.ConvertFrom("#FFFA8072");
        }
        /// <summary>
        /// id of drone has been entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void idInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (TextBox_id.Text != null && TextBox_id.Text != string.Empty && (TextBox_id.Text).All(char.IsDigit))
            {
                TextBox_id.BorderBrush = (Brush)bc.ConvertFrom("#FF99B4D1");
                TextBox_id.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
            else
            {
                TextBox_id.Background = (Brush)bc.ConvertFrom("#FFFA8072");
            }
        }

        private void ModelButton_Click(object sender, RoutedEventArgs e)
        {
            TextBoxNewModel.Visibility = Visibility.Visible;
            labelTextBoxNewModel.Visibility = Visibility.Visible;
            NewModel.Visibility = Visibility.Visible;
        }

        private void sendOrReleaseButtonButton_Click(object sender, RoutedEventArgs e)
        {
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            string str = (string)sendOrReleaseButton.Content;
            int droneId;
            if (str == "Sending to charging")
            {
                try
                {
                    droneId = selected.Id;
                    bl.SendingDroneForCharging(droneId);
                    TextBoxDelivery.Text = DroneStatuses.Charging.ToString();
                    TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    MessageBox.Show("Update successfully");
                    sendOrReleaseButton.Content = "Release drone";
                    sendOrReleaseButton.Visibility = Visibility.Visible;
                    delivery.Visibility = Visibility.Hidden;

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            if (str == "Release drone")
            {
                try
                {
                    droneId = selected.Id;
                    bl.ReleaseDroneFromCharging(droneId);
                    TextBoxParcelTransfer.Text = ((int)bl.DroneDisplay(droneId).Battery).ToString();
                    TextBoxDelivery.Text = DroneStatuses.Available.ToString();
                    TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    MessageBox.Show("Update successfully");
                    sendOrReleaseButton.Content = "Sending to charging";
                    sendOrReleaseButton.Visibility = Visibility.Visible;
                    delivery.Content = "Affiliation";
                    delivery.Visibility = Visibility.Visible;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            if (str == "Package collection")
            {
                try
                {
                    droneId = selected.Id;
                    bl.ParcelCollectionByDrone(droneId);
                    TextBoxLongitude.Text = bl.DroneDisplay(droneId).ParcelTransfer.ToString();
                    TextBoxParcelTransfer.Text = ((int)bl.DroneDisplay(droneId).Battery).ToString();
                    TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    MessageBox.Show("Update successfully");
                    sendOrReleaseButton.Content = "Package delivery";
                    sendOrReleaseButton.Visibility = Visibility.Visible;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else if (str == "Package delivery")
            {
                try
                {
                    droneId = selected.Id;
                    bl.DeliveryOfParcelByDrone(droneId);
                    TextBoxLongitude.Text = bl.DroneDisplay(droneId).ParcelTransfer.ToString();
                    TextBoxParcelTransfer.Text = ((int)bl.DroneDisplay(droneId).Battery).ToString();
                    TextBoxDelivery.Text = DroneStatuses.Available.ToString();
                    TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    MessageBox.Show("Update successfully");
                    sendOrReleaseButton.Content = "Sending to charging";
                    sendOrReleaseButton.Visibility = Visibility.Visible;
                    delivery.Content = "Affiliation";
                    delivery.Visibility = Visibility.Visible;

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            managerPage.listDrones.ItemsSource = bl.GetListDrone();
        }

        private void allDeliveryButtonButton_Click(object sender, RoutedEventArgs e)
        {
            int droneId;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            string str = (string)delivery.Content;
            if (str == "Affiliation")
            {
                try
                {
                    droneId = selected.Id;
                    bl.AffiliateParcelToDrone(droneId);
                    TextBoxLongitude.Text = bl.DroneDisplay(droneId).ParcelTransfer.ToString();
                    TextBoxParcelTransfer.Text = ((int)bl.DroneDisplay(droneId).Battery).ToString();
                    TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    TextBoxDelivery.Text = DroneStatuses.Delivery.ToString();
                    MessageBox.Show("Update successfully");
                    sendOrReleaseButton.Content = "Package collection";
                    sendOrReleaseButton.Visibility = Visibility.Visible;
                    delivery.Visibility = Visibility.Hidden;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            managerPage.listDrones.ItemsSource = bl.GetListDrone();
        }

       
    }
}

