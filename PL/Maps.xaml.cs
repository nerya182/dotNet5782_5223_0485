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
using BlApi;
using BO;
using Microsoft.Maps.MapControl.WPF;


namespace PL
{
    /// <summary>
    /// Interaction logic for Maps.xaml
    /// </summary>
    public partial class Maps : Page
    {
        private IBL bl = BlApi.BlFactory.GetBl();
        public Maps(IEnumerable<DroneToList> listsDrone)
        {
            InitializeComponent();
            Pushpin pin;
            List<Pushpin> points = new();
            int i = 1;
            foreach (var item in listsDrone)
            {
                pin = new();
                pin.Location = new(item.Location.Lattitude, item.Location.Longitude);
                if (points.Find(p => p.Location == p.Location) == null)
                {
                    points.Add(pin);
                    myMap.Children.Add(pin);
                }
                else
                {
                    points.Add(pin);
                    pin.Location = new(item.Location.Lattitude + 0.0001 * i, item.Location.Longitude + 0.0001 * i);
                    i++;
                    myMap.Children.Add(pin);
                }

            }
        }
        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerPage manager = new();
            this.Content = manager;
        }
        public Maps(IEnumerable<Station> listStation)
        {
            InitializeComponent();
            Pushpin pin;
            List<Pushpin> points = new();
            int i = 1;
            foreach (var item in listStation)
            {
                pin = new();
                pin.Location = new(item.location.Lattitude, item.location.Longitude);
                if (points.Find(p => p.Location == p.Location) == null)
                {
                    points.Add(pin);
                    myMap.Children.Add(pin);
                }
                else
                {
                    points.Add(pin);
                    pin.Location = new(item.location.Lattitude + 0.0001 * i, item.location.Longitude + 0.0001 * i);
                    i++;
                    myMap.Children.Add(pin);
                }
            }
        }
        public Maps(IEnumerable<Customer> listcustomers)
        {
            InitializeComponent();
            Pushpin pin;
            List<Pushpin> points = new();
            int i = 1;
            foreach (var item in listcustomers)
            {
                pin = new();
                pin.Location = new(item.Location.Lattitude, item.Location.Longitude);
                if (points.Find(p => p.Location == p.Location) == null)
                {
                    points.Add(pin);
                    myMap.Children.Add(pin);
                }
                else
                {
                    points.Add(pin);
                    pin.Location = new(item.Location.Lattitude + 0.0001 * i, item.Location.Longitude + 0.0001 * i);
                    i++;
                    myMap.Children.Add(pin);
                }
            }
        }
    }
}

