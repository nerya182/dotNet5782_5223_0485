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
        ManagerPage managerPage;
        MainWindow mainWindow;
        BO.Customer selected = new BO.Customer();
        BO.Customer customerSelected = new BO.Customer();

        public CustomerPage(MainWindow main, Customer customer, ManagerPage manager)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            mainWindow = main;
            managerPage = manager;
            label_id.Content = "ID number";
            label_name.Content = "name";
            label_phon.Content = "phone number";
            label_latitude.Content = "Location";
            selected = customer;
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
        public CustomerPage(ManagerPage manager, MainWindow main)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            mainWindow = main;
            managerPage = manager;
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
            try
            {
                bool flag;
                int id;
                double location;
                BO.Customer newCustomer = new BO.Customer();
                flag = int.TryParse(TextBox_id.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error ,customer id was not entered ");
                    return;
                }
                newCustomer.Id = int.Parse(TextBox_id.Text);
                if (TextBox_name.Text == null)
                {
                    MessageBox.Show("error ,name customer was not entered ");
                    return;
                }
                newCustomer.Name = TextBox_name.Text;
                if (PhoneTextBox.Text == null)
                {
                    MessageBox.Show("error ,phone customer was not entered ");
                    return;
                }
                newCustomer.Phone = PhoneTextBox.Text;
                BO.Location locationCustomer = new BO.Location();
                flag = double.TryParse(TextBoxLatitude.Text, out location);
                if (!flag)
                {
                    MessageBox.Show("error ,latitude was not entered ");
                    return;
                }
                locationCustomer.Lattitude = location;
                flag = double.TryParse(TextBox_longitude.Text, out location);
                if (!flag)
                {
                    MessageBox.Show("error ,longitude was not entered ");
                    return;
                }
                locationCustomer.Longitude = location;
                newCustomer.Location = locationCustomer;
                bl.AddCustomer(newCustomer);
                MessageBox.Show("Added successfully");
                TextBox_id.IsEnabled = false;
                TextBox_name.IsEnabled = false;
                PhoneTextBox.IsEnabled = false;
                TextBoxLatitude.IsEnabled = false;
                TextBox_longitude.IsEnabled = false;
                managerPage.FilterRefreshCustomres();
                Cancel_Add_Button_Click(sender, e);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Cancel_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerPage page = new ManagerPage(mainWindow);
            page.TabManager.SelectedIndex = 3;
            mainWindow.Content = page;
        }
        
        private void NameButton_Click(object sender, RoutedEventArgs e)
        {
            label_TextBoxNew.Visibility = Visibility.Visible;
            TextBoxNewModel.Visibility = Visibility.Visible;
            NewUpdate.Visibility = Visibility.Visible;
        }

        private void PhoneButton_Click(object sender, RoutedEventArgs e)
        {
            label_TextBoxNew.Content = "Insert a new phone";
            label_TextBoxNew.Visibility = Visibility.Visible;
            TextBoxNewModel.Visibility = Visibility.Visible;
            NewUpdate.Visibility = Visibility.Visible;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            string str = label_TextBoxNew.Content.ToString();
            BO.Customer updateCustomer = customerSelected;
            try
            {
                if (str == "Insert a new phone")
                {
                    updateCustomer.Phone = TextBoxNewModel.Text;
                }
                else
                {
                    updateCustomer.Name = TextBoxNewModel.Text;
                }
                bl.UpdateCustomer(updateCustomer);
                MessageBox.Show("Update successfully");
                label_TextBoxNew.Visibility = Visibility.Hidden;
                TextBoxNewModel.Visibility = Visibility.Hidden;
                NewUpdate.Visibility = Visibility.Hidden;
                PhoneTextBox.Text = updateCustomer.Phone;
                TextBox_name.Text = updateCustomer.Name.ToString();
                managerPage.listCustomers.ItemsSource = bl.GetListCustomer();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            } 
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
    }
}

