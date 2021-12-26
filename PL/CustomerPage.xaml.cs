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
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        BlApi.IBL bl;
        BO.CustomerToList selected = new BO.CustomerToList();
        BO.Customer customerSelected = new BO.Customer();

        public CustomerPage(Customer customer)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            label_id.Content = "ID number";
            label_name.Content = "name";
            label_phon.Content = "phone number";
            label_latitude.Content = "Location";
            selected =bl.MakeCustomerToList(customer);
            customerSelected = bl.CustomerDisplay(selected.Id);
            TextBox_id.Text = customerSelected.Id.ToString();
            TextBox_name.Text = customerSelected.Name.ToString();
            PhoneTextBox.Text = customerSelected.Phone.ToString();
            TextBoxLatitude.Text = customerSelected.Location.ToString();
            TextBox_longitude.Visibility = Visibility.Hidden;
            listFromeCustomer.ItemsSource = customerSelected.FromCustomer;
            listToCustomer.ItemsSource = customerSelected.ToCustomer;
            label_longitude.Visibility = Visibility.Hidden;
            label_received.Visibility = Visibility.Hidden;
            label_TextBoxNew.Visibility = Visibility.Hidden;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            NewUpdate.Visibility = Visibility.Hidden;
            TextBox_id.IsEnabled = false;
            TextBox_name.IsEnabled = false;
            PhoneTextBox.IsEnabled = false;
            TextBoxLatitude.IsEnabled = false;
            TextBox_longitude.IsEnabled = false;
            add_button.Visibility = Visibility.Hidden;
        }
        public CustomerPage()
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            NameButton.Visibility = Visibility.Hidden;
            PhoneButton.Visibility = Visibility.Hidden;
            label_TextBoxNew.Visibility = Visibility.Hidden;
            TextBoxNewModel.Visibility = Visibility.Hidden;
            NewUpdate.Visibility = Visibility.Hidden;
            label_received.Visibility = Visibility.Hidden;
            listFromeCustomer.Visibility = Visibility.Hidden;
            listToCustomer.Visibility = Visibility.Hidden;
            lable_parcelsto.Visibility = Visibility.Hidden;
            lable_parcelsfo.Visibility = Visibility.Hidden;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Cancel_Add_Button_Click(object sender, RoutedEventArgs e)
        {
           
        }
        
        private void NameButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void PhoneButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void DroneName_TextChanged(object sender, TextChangedEventArgs e)
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

        private void DroneId_TextChanged(object sender, TextChangedEventArgs e)
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

        private void OpenLisrFromCustomer(object sender, MouseButtonEventArgs e)
        {

        }

        private void OpenListToCustomer(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

