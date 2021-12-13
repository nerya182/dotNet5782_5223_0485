using BlApi;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL bl = BlApi.BlFactory.GetBl();
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// first button clicked to view the list of drones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowDronesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            DronesListWindow dronesListWindow = new DronesListWindow(bl, this);
            dronesListWindow.Show();
        }
    }
}
