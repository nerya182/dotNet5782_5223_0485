using System;
using System.Collections;
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
        private DroneStatuses statuses=DroneStatuses.Charging;
        public DroneWindow(IBL.IBL blw)
        {
            InitializeComponent();
            bldw = blw;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            chargeStationId.ItemsSource = bldw.GetListStation(i => true);
            StatusAdd.Text = "Charging";
            StatusAdd.IsEditable = false;
            WeightSelector.Text = "Select max weight";
            chargeStationId.Text = "select BaseStation";
            WeightSelector.IsEditable = false;
            StatusAdd.IsEditable = false;
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
                newDrone.MaxWeight = (WeightCategories) WeightSelector.SelectedItem;
                chargingStationId = (int)chargeStationId.SelectedItem;
                bldw.AddDrone(newDrone, chargingStationId);
                TextBox_id.IsEnabled = false;
                chargeStationId.IsEnabled = false;
                TextBox_model.IsEnabled = false;
                WeightSelector.IsEnabled = false;
                add_button.IsEnabled = false;
                TextBoxLattitude.Text = bldw.BaseStationDisplay(chargingStationId).location.Lattitude.ToString();
                TextBoxLongitude.Text = bldw.BaseStationDisplay(chargingStationId).location.Longitude.ToString();
            } 
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }
    }
}
