using BO;
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
    /// Interaction logic for StationPage.xaml
    /// </summary>
    public partial class StationPage : Page
    { 
        BlApi.IBL bl;
        BO.StationToList selected = new BO.StationToList();
        BO.Station stationSelected = new BO.Station();
        public StationPage()
        {
            InitializeComponent();
            bl= BlApi.BlFactory.GetBl();
            close_button.Visibility = Visibility.Hidden;
            TextBoxNewName.Visibility = Visibility.Hidden;
            labelTextBoxNewName.Visibility = Visibility.Hidden;
            NewName.Visibility = Visibility.Hidden;
            close_button.Visibility = Visibility.Visible;
            UpdateName.Visibility = Visibility.Hidden;
            listOfDrones.Visibility = Visibility.Hidden;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
          
        }
        private void Cancel_Add_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        public StationPage(object selectedItem)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            close_button.Visibility = Visibility.Visible;
            label_id.Content = "ID Number:";
            label_name.Content = "Name:";
            add_button.Visibility = Visibility.Hidden;
            TextBoxLattitude.FontSize = 10;
            label_number_of_chargeslots.Content = "Available Charge Slots:";
            Lattitude.Content = "Location: ";
            Longitud.Content = "List of Drones:";
            selected = (StationToList)selectedItem;
            stationSelected = bl.BaseStationDisplay(selected.Id);
            TextBox_id.Text = stationSelected.Id.ToString();
            TextBox_name.Text = stationSelected.Name;
            TextBoxLattitude.Text = stationSelected.location.ToString();
            TextBoxLongitude.Visibility = Visibility.Hidden;
            TextBoxChargeSlots.Text = stationSelected.AvailableChargeSlots.ToString();
            TextBoxLongitude.Width = 300;
            TextBoxLongitude.Height = 100;
            TextBox_id.IsEnabled = false;
            TextBox_name.IsEnabled = false;
            TextBoxChargeSlots.IsEnabled = false;
            TextBoxLattitude.IsEnabled = false;
            TextBoxLongitude.IsEnabled = false;
            TextBoxLongitude.FontSize = 10;
            close_button.Visibility = Visibility.Visible;
            labelTextBoxNewName.Visibility = Visibility.Hidden;
            listOfDrones.ItemsSource = stationSelected.droneInCharging;
        }
        private void idInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (TextBox_id.Text != null && TextBox_id.Text != string.Empty && TextBox_id.Text.All(char.IsDigit))
            {
                TextBox_id.BorderBrush = (Brush)bc.ConvertFrom("#FF99B4D1");
                TextBox_id.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
            else
            {
                TextBox_id.Background = (Brush)bc.ConvertFrom("#FFFA8072");
            }
        }

        private void StationName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            string text = TextBox_name.Text;
            if (text != null && text != "" && char.IsLetter(text.ElementAt(0)))
            {
                TextBox_name.BorderBrush = (Brush)bc.ConvertFrom("#FF99B4D1");
                TextBox_name.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
            else TextBox_name.Background = (Brush)bc.ConvertFrom("#FFFA8072");
        }

        
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
        
        
        
        
        private void UpdateName_Click_1(object sender, RoutedEventArgs e)
        {
           
        }
        private void DoubleClickOpenDrone(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}

