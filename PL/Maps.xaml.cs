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
using Microsoft.Maps.MapControl.WPF;


namespace PL
{
    /// <summary>
    /// Interaction logic for Maps.xaml
    /// </summary>
    public partial class Maps : Page
    {
        public Maps(BlApi.IBL IBL)
        {
            InitializeComponent();
            Pushpin pin;
            foreach (var item in IBL.GetListDrone())
            {
                pin = new();
                pin.Location = new(item.Location.Lattitude, item.Location.Longitude);
                myMap.Children.Add(pin);
            }
            var a = IBL.GetListDrone();
            foreach (var item in IBL.GetListStation())
            {
              // pin = new();
                //pin.Location = new(item.location.Lattitude, item.location.Longitude);
                //myMap.Children.Add(pin);
            }
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerPage manager = new();
            this.Content = manager;
        }
    }
}

