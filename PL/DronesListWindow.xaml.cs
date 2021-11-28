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
using IBL;
using IBL.BO;

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

        private void WeightCategoriesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)e.AddedItems[0];
          DronesListView.ItemsSource = blw.GetListDrone(i => i.MaxWeight ==(IDAL.DO.WeightCategories)weight);
        }
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DronesListView.ItemsSource = blw.GetListDrone
        }
    }
}
