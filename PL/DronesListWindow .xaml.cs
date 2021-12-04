using IBL.BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        IBL.IBL blw;
        public DronesListWindow(IBL.IBL bl)
        {
            InitializeComponent();
            blw = bl;
            WeightCategoriesSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            DronesListView.ItemsSource = blw.GetListDrone(i => true);
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = blw.GetListDrone(i => true);
        }

       

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IBL.BO.DroneStatuses statuses = (IBL.BO.DroneStatuses) (DroneStatuses) StatusSelector.SelectedItem;
            DronesListView.ItemsSource = blw.GetListDrone(i => i.Status == statuses);
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blw).Show();
        }

        private void WeightCategoriesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IBL.BO.WeightCategories weight =
                (IBL.BO.WeightCategories)(WeightCategories)WeightCategoriesSelector.SelectedItem;
            DronesListView.ItemsSource = blw.GetListDrone(i => i.MaxWeight == weight);
        }
    }
}
