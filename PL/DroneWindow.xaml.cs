﻿using System;
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
        /// <summary>
        /// constructor for "half" of the window
        /// </summary>
        /// <param name="blw"> gives access to the BL functions</param>
        /// <param name="w"> gives access to the previous window</param>
        public DroneWindow(IBL.IBL blw, DronesListWindow w)
        {
            InitializeComponent();
            bldw = blw;
            droneListWin = w;
            TextBoxParcelTransfer.Visibility = Visibility.Hidden;
            WeightTextBox.Visibility = Visibility.Hidden;
            close_button.Visibility = Visibility.Hidden;
            label_id.Content = "Enter ID Number:";
            changeModelButton.Visibility = Visibility.Hidden;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            sendOrReleaseButton.Visibility = Visibility.Hidden;
            delivery.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            chargeStationId.ItemsSource = from IBL.BO.Station s in bldw.GetListStation()
                                          where s.AvailableChargeSlots>0
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
                IBL.BO.DroneToList newDrone = new IBL.BO.DroneToList();
                flag = int.TryParse(TextBox_id.Text,out id);
                if (!flag)
                {
                    MessageBox.Show("error ,drone id was not entered ");
                    return;
                }
                newDrone.Id = int.Parse(TextBox_id.Text);
                if (TextBox_model.Text==null)
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
                TextBoxLattitude.Text = bldw.BaseStationDisplay(chargingStationId).location.Lattitude.ToString();
                TextBoxLongitude.Text = bldw.BaseStationDisplay(chargingStationId).location.Longitude.ToString();
                bldw.AddDrone(newDrone, chargingStationId);
                droneListWin.DronesListView.ItemsSource = bldw.GetListDrone();
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
        /// <summary>
        /// user would like to cancel the add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            flagClosure = false;
            this.Close();
        }
        /// <summary>
        /// constructor for "half" of the window
        /// </summary>
        /// <param name="blw">gives access to the BL functions</param>
        /// <param name="selectedItem"></param>
        /// <param name="w">gives access to the previous window</param>
        public DroneWindow(IBL.IBL blw, object selectedItem, DronesListWindow w)
        {
            InitializeComponent();
            droneListWin = w;
            bldw = blw;
            close_button.Visibility = Visibility.Visible;
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
            Cancel_add_button.Visibility = Visibility.Hidden;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            selected = (DroneToList)selectedItem;
            droneSelected = bldw.DroneDisplay(selected.Id);
            TextBox_id.Text = droneSelected.Id.ToString();
            WeightTextBox.Text = droneSelected.MaxWeight.ToString();
            TextBox_model.Text = droneSelected.Model;
            TextBoxLattitude.Text = droneSelected.Location.ToString();
            TextBoxDelivery.Text = droneSelected.Status.ToString();
            TextBoxLongitude.Text = droneSelected.ParcelTransfer.ToString();
            TextBoxParcelTransfer.Text = droneSelected.Battery.ToString();
            TextBoxLongitude.Width = 300;
            TextBoxLongitude.Height = 80;
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
                    Parcel parcel = blw.GetListParcel().First(i => i.Id == selected.ParcelBeingPassedId);
                    if (parcel.PickedUp==null)
                    {
                        sendOrReleaseButton.Content = "Package collection";
                        sendOrReleaseButton.Visibility = Visibility.Visible;
                    }
                    else if(parcel.Delivered==null)
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
               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            droneListWin.DronesListView.ItemsSource = bldw.GetListDrone();
        }
        /// <summary>
        /// closing the current window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            flagClosure = false;
            this.Close();
           
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            droneListWin.Visibility = Visibility.Visible;
            base.OnClosed(e);
            e.Cancel = flagClosure;
        }
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
            if (str=="Sending to charging")
            {
                try
                {
                    droneId = selected.Id;
                    bldw.SendingDroneForCharging(droneId);
                    MessageBox.Show("Update successfully");

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            if (str== "Release drone")
            {
                try
                {
                    droneId = selected.Id;
                    bldw.ReleaseDroneFromCharging(droneId);
                    MessageBox.Show("Update successfully");

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
                    bldw.ParcelCollectionByDrone(droneId);
                    MessageBox.Show("Update successfully");

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
                    bldw.DeliveryOfParcelByDrone(droneId);
                    MessageBox.Show("Update successfully");

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            droneListWin.DronesListView.ItemsSource = bldw.GetListDrone();
        }

        private void allDeliveryButtonButton_Click(object sender, RoutedEventArgs e)
        {
            int droneId;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            NewModel.Visibility = Visibility.Hidden;
            string str = (string)delivery.Content;
            try
            {
                droneId = selected.Id;
                bldw.AffiliateParcelToDrone(droneId);
                MessageBox.Show("Update successfully");

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            droneListWin.DronesListView.ItemsSource = bldw.GetListDrone();
        }
    }
}
