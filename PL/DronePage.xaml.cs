using BO;
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
        BO.Drone selected = new BO.Drone();
        BO.Drone droneSelected = new BO.Drone();
       
        bool simulatorIsActive = false;
        public BackgroundWorker SimulatorWorker = new BackgroundWorker();
        public Action Invoke;
       
        /// <summary>
        /// constructor for add drone  window
        /// </summary>
        /// <param name="blw"> gives access to the BL functions</param>
        /// <param name="w"> gives access to the previous window</param>
        public DronePage()
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            changeModelButton.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            WeightTextBox.Visibility = Visibility.Collapsed;
            simulator.Visibility = Visibility.Collapsed;
            label_id.Content = "Enter ID Number:";
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
        }
        /// <summary>
        /// constructor for display drone window
        /// </summary>
        /// <param name="blw">gives access to the BL functions</param>
        /// <param name="selectedItem"></param>
        /// <param name="w">gives access to the previous window</param>
        public DronePage(Drone drone)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            label_id.Content = "ID Number:";
            label_model.Content = "Model:";
            add_button.Visibility = Visibility.Hidden;
            labelTextBoxNewModel.Visibility = Visibility.Hidden;
            label_weight.Content = "Weight:";
            label_charge_Station_id.Content = "Battery %:";
            Delivery.Content = "Status:";
            TextBoxLattitude.FontSize = 10;
            Lattitude.Content = "Location: ";
            selected = drone;
            droneSelected = bl.DroneDisplay(selected.Id);
            mainDrone.DataContext = droneSelected;
            TextBoxDelivery.Text = selected.Status.ToString();
            WeightTextBox.Visibility = Visibility.Visible;
            if (droneSelected.ParcelTransfer.Id == 0)
            {
                ListParcelTransfer.Visibility = Visibility.Hidden;
            }
            else
            {
                List<ParcelTransfer> parcelT = new();
                parcelT.Add(droneSelected.ParcelTransfer);
                ListParcelTransfer.DataContext = parcelT;
            }
            TextBox_id.IsEnabled = false;
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
                    Parcel parcel = bl.GetListParcel().First(i => i.Id == bl.MakeDroneToList(selected).ParcelBeingPassedId);
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
            }
        }
        /// <summary>
        /// button that opens all the update options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

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
        }


        private void allDeliveryButtonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sendOrRelease_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenParcelTransfer(object sender, MouseButtonEventArgs e)
        {

        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Simulator_Click(object sender, RoutedEventArgs e)
        {

            if (!simulatorIsActive)
            {
                simulatorIsActive = true;
                resetSimulatorWoker();
                SimulatorWorker.RunWorkerAsync();
                simulator.Content= "Manual";
                changeModelButton.Visibility = Visibility.Collapsed;
                sendOrReleaseButton.Visibility = Visibility.Collapsed;
                delivery.Visibility = Visibility.Collapsed;
            }
            else
            {
                simulator.Content= "Simulator";
                simulatorIsActive = false;
                BO.Drone drone = bl.DroneDisplay(droneSelected.Id);
                if (drone.Status==DroneStatuses.Delivery)
                {
                    changeModelButton.Visibility = Visibility.Visible;
                    Parcel parcel = bl.GetListParcel().FirstOrDefault(p => p.Id == drone.ParcelTransfer.Id);
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
                }
                else if (drone.Status == DroneStatuses.Charging)
                {
                    changeModelButton.Visibility = Visibility.Visible;
                    sendOrReleaseButton.Content = "Release drone";
                    sendOrReleaseButton.Visibility = Visibility.Visible;
                    delivery.Visibility = Visibility.Hidden;
                }
                else
                {
                    changeModelButton.Visibility = Visibility.Visible;
                    sendOrReleaseButton.Content = "Release drone";
                    sendOrReleaseButton.Visibility = Visibility.Visible;
                    delivery.Visibility = Visibility.Hidden;
                }       
            }
        }
        private void resetSimulatorWoker()
        {
            Invoke += InvokeMainThread;
            SimulatorWorker.WorkerReportsProgress = true;
            SimulatorWorker.WorkerSupportsCancellation = true;
            SimulatorWorker.ProgressChanged += refresh;
            SimulatorWorker.DoWork += (s, e) =>
            {
                bl.StartSimulator(droneSelected.Id, Invoke, ToCancel);
            };
        }

        private bool ToCancel()
        {
            return !simulatorIsActive;
        }
        private void refresh(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                BO.Drone drone = bl.DroneDisplay(droneSelected.Id);
                if (drone.ParcelTransfer.Id == 0)
                {
                    ListParcelTransfer.Visibility = Visibility.Hidden;
                }
                else
                {
                    List<ParcelTransfer> parcelT = new();
                    parcelT.Add(drone.ParcelTransfer);
                    ListParcelTransfer.DataContext = parcelT;
                }
                this.Title = drone.Status.ToString();
                mainDrone.DataContext = drone;
            }
            catch (BO.ItemNotFoundException ex)
            {
                MessageBox.Show("Drone has crashed due to low battery");
            }
        }

        private void InvokeMainThread()
        {
            SimulatorWorker.ReportProgress(1);
        }
    }
}

