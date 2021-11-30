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
        public DroneWindow(IBL.IBL blw)
        {
          InitializeComponent();
          bldw = blw;
          WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int chargingStationId=0;
            IBL.BO.DroneToList newDrone = new IBL.BO.DroneToList();
            newDrone.Id = int.Parse(TextBox_id.Text);
            newDrone.Model = TextBox_model.Text;
            newDrone.MaxWeight = (WeightCategories)WeightSelector.SelectedItem;
            chargingStationId = int.Parse(TextBox_charge_Station_id.Text);
            bldw.AddDrone(newDrone, chargingStationId);
        }
    }
}
