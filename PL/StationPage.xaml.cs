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
        ManagerPage managerPage;
        MainWindow mainWindow;
        BO.StationToList selected = new BO.StationToList();
        BO.Station stationSelected = new BO.Station();
        public StationPage(ManagerPage manager, MainWindow main)
        {
            InitializeComponent();
            bl= BlApi.BlFactory.GetBl();
            mainWindow = main;
            managerPage = manager;
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
            try
            {
                bool flag;
                int id;
                double num;
                BO.Station newStation = new BO.Station();
                flag = int.TryParse(TextBox_id.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("Error, station id was not entered ");
                    return;
                }
                newStation.Id = int.Parse(TextBox_id.Text);
                if (TextBox_name.Text == null)
                {
                    MessageBox.Show("Error, station name was not entered ");
                    return;
                }
                newStation.Name = TextBox_name.Text;
                flag = int.TryParse(TextBoxChargeSlots.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("Error, amount of slots was not entered ");
                    return;
                }
                newStation.AvailableChargeSlots = int.Parse(TextBoxChargeSlots.Text);
                flag = double.TryParse(TextBoxLattitude.Text, out num);
                if (!flag)
                {
                    MessageBox.Show("Error, Lattitude was not entered ");
                    return;
                }
                Location location = new Location();
                location.Lattitude = double.Parse(TextBoxLattitude.Text);
                flag = double.TryParse(TextBoxLongitude.Text, out num);
                if (!flag)
                {
                    MessageBox.Show("Error, Longtitude was not entered ");
                    return;
                }
                location.Lattitude = double.Parse(TextBoxLongitude.Text);
                newStation.location = location;
                bl.AddStation(newStation);
                managerPage.listStaions.Items.Refresh();
                MessageBox.Show("Added successfully");
                TextBox_id.IsEnabled = false;
                TextBox_name.IsEnabled = false;
                TextBoxChargeSlots.IsEnabled = false;
                TextBoxLattitude.IsEnabled = false;
                TextBoxLongitude.IsEnabled = false;
                add_button.IsEnabled = false;
                managerPage.FilterRefreshStaions();
                Cancel_Add_Button_Click(sender, e);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void Cancel_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            managerPage.FilterRefreshStaions();
            ManagerPage page = new ManagerPage(mainWindow);
            page.TabManager.SelectedIndex = 2;
            mainWindow.Content = page;
        }
        public StationPage(MainWindow main,object selectedItem, ManagerPage manager)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            mainWindow = main;
            managerPage = manager;
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
            TextBoxNewName.Visibility = Visibility.Hidden;
            labelTextBoxNewName.Visibility = Visibility.Hidden;
            NewName.Visibility = Visibility.Hidden;
            listOfDrones.ItemsSource = stationSelected.droneInCharging;
        }
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
            BO.StationToList updatedStation = new BO.StationToList();
            try
            {
                updatedStation.Name = TextBoxNewName.Text;
                updatedStation.Id = selected.Id;
                bl.UpdateStationName(updatedStation.Id, updatedStation.Name);
                MessageBox.Show("Update successfully");
                TextBox_name.Text = TextBoxNewName.Text;
                TextBoxNewName.Visibility = Visibility.Hidden;
                labelTextBoxNewName.Visibility = Visibility.Hidden;
                NewName.Visibility = Visibility.Hidden;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            managerPage.listStaions.ItemsSource = bl.GetStations();
        }

        private void UpdateName_Click_1(object sender, RoutedEventArgs e)
        {
            labelTextBoxNewName.Visibility = Visibility.Visible;
            TextBoxNewName.Visibility = Visibility.Visible;
            NewName.Visibility = Visibility.Visible;
        }
    }
}

