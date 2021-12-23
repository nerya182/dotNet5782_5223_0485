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
    /// Interaction logic for parcelPage.xaml
    /// </summary>
    public partial class parcelPage : Page
    {
        BlApi.IBL bl;
        ManagerPage managerPage;
        MainWindow mainWindow;
        BO.Parcel selected = new BO.Parcel();
        BO.Parcel parcelSelected = new BO.Parcel();
        public parcelPage(ManagerPage manager, MainWindow main)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            mainWindow = main;
            managerPage = manager;
            label_DroneInParcel.Visibility = Visibility.Hidden;
            TextBox_DroneInParcel.Visibility = Visibility.Hidden;
            TextBox_Sender.Visibility = Visibility.Hidden;
            TextBox_Target.Visibility = Visibility.Hidden;
            label_created.Visibility = Visibility.Hidden;
            label_affiliated.Visibility = Visibility.Hidden;
            label_pickedUp.Visibility = Visibility.Hidden;
            label_delivered.Visibility = Visibility.Hidden;
            label_status.Visibility = Visibility.Hidden;
            TextBox_Created.Visibility = Visibility.Hidden;
            TextBox_Affiliated.Visibility = Visibility.Hidden;
            TextBox_PickedUp.Visibility = Visibility.Hidden;
            TextBox_Delivered.Visibility = Visibility.Hidden;
            TextBox_DisplayWeight.Visibility = Visibility.Hidden;
            TextBox_DisplayPriority.Visibility = Visibility.Hidden;
            TextBox_DisplayStatus.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            WeightSelector.Text = "Select weight";
            PrioritySelector.Text = "Select priority";
            WeightSelector.IsEditable = true;
            PrioritySelector.IsEditable = true;
            Update.Visibility = Visibility.Hidden;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        public parcelPage(MainWindow main, Parcel parcel, ManagerPage manager)
        {
            InitializeComponent();
            mainWindow = main;
            managerPage = manager;
            bl = BlApi.BlFactory.GetBl();
            WeightSelector.Visibility = Visibility.Hidden;
            PrioritySelector.Visibility = Visibility.Hidden;
            label_SenderID.Content = "Sender:";
            selected = parcel;
            parcelSelected = bl.ParcelDisplay(selected.Id);
            if (parcelSelected.Affiliation == null || parcelSelected.Delivered != null)
                Update.Visibility = Visibility.Hidden;
            else if (parcelSelected.Delivered == null && parcelSelected.PickedUp != null)
                Update.Content = "Deliver Parcel";
            else if (parcelSelected.PickedUp == null && parcelSelected.Affiliation != null)
                Update.Content = "Pick-Up Parcel";
            TextBox_SenderID.Visibility = Visibility.Hidden;
            TextBox_Sender.Text = parcelSelected.Sender.ToString();
            TextBox_Sender.FontSize = 10;
            label_TargetID.Content = "Target:";
            TextBox_TargetID.Visibility = Visibility.Hidden;
            TextBox_Target.Text = parcelSelected.Target.ToString();
            TextBox_Target.FontSize = 10;
            TextBox_DisplayWeight.Text = parcelSelected.Weight.ToString();
            TextBox_DisplayPriority.Text = parcelSelected.Priority.ToString();
            TextBox_DroneInParcel.FontSize = 8;
            add_button.Visibility = Visibility.Hidden;
            Cancel_add_button.Visibility = Visibility.Hidden;
            Listview_Sender.Items.Add(parcelSelected.Sender);
            Listview_Target.Items.Add(parcelSelected.Target);
            if (parcelSelected.Delivered != null)
            {
                TextBox_DisplayStatus.Text = "Delivered";
                Listview_droneinparcel.Visibility = Visibility.Hidden;
            }
            else if (parcelSelected.PickedUp != null)
            {
                TextBox_DisplayStatus.Text = "Picked Up";
                TextBox_DroneInParcel.Text = parcelSelected.drone.ToString();
                Listview_droneinparcel.Items.Add(parcelSelected.drone);
            }
            else if (parcelSelected.Affiliation != null)
            {
                TextBox_DisplayStatus.Text = "Affiliated";
                TextBox_DroneInParcel.Text = parcelSelected.drone.ToString();
                Listview_droneinparcel.Items.Add(parcelSelected.drone);
            }
            else
            {
                TextBox_DisplayStatus.Text = "Created";
                Listview_droneinparcel.Visibility = Visibility.Hidden;
            }
            TextBox_Created.Text = parcelSelected.Creating.ToString();
            if (parcelSelected.Affiliation != null)
                TextBox_Affiliated.Text = parcelSelected.Affiliation.ToString();
            else
            {
                TextBox_Affiliated.Visibility = Visibility.Hidden;
                label_affiliated.Visibility = Visibility.Hidden;
            }
            if (parcelSelected.PickedUp != null)
                TextBox_PickedUp.Text = parcelSelected.PickedUp.ToString();
            else
            {
                TextBox_PickedUp.Visibility = Visibility.Hidden;
                label_pickedUp.Visibility = Visibility.Hidden;
            }
            if (parcelSelected.Delivered != null)
                TextBox_Delivered.Text = parcelSelected.Delivered.ToString();
            else
            {
                TextBox_Delivered.Visibility = Visibility.Hidden;
                label_delivered.Visibility = Visibility.Hidden;
            }

            TextBox_DisplayPriority.IsEnabled = false;
            TextBox_DroneInParcel.IsEnabled = false;
            TextBox_Sender.IsEnabled = false;
            TextBox_Target.IsEnabled = false;
            TextBox_DisplayWeight.IsEnabled = false;
            TextBox_DisplayPriority.IsEnabled = false;
            TextBox_DisplayStatus.IsEnabled = false;
            TextBox_Created.IsEnabled = false;
            TextBox_Affiliated.IsEnabled = false;
            TextBox_PickedUp.IsEnabled = false;
            TextBox_Delivered.IsEnabled = false;

        }

        private void Cancel_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            ManagerPage page = new ManagerPage(mainWindow);
            page.TabManager.SelectedIndex = 1;
            mainWindow.Content = page;
        }

        private void SenderID_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (TextBox_SenderID.Text != null && TextBox_SenderID.Text != string.Empty && (TextBox_SenderID.Text).All(char.IsDigit))
            {
                TextBox_SenderID.BorderBrush = (Brush)bc.ConvertFrom("#FF99B4D1");
                TextBox_SenderID.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
            else
            {
                TextBox_SenderID.Background = (Brush)bc.ConvertFrom("#FFFA8072");
            }
        }

        private void TargetID_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (TextBox_TargetID.Text != null && TextBox_TargetID.Text != string.Empty && (TextBox_TargetID.Text).All(char.IsDigit))
            {
                TextBox_TargetID.BorderBrush = (Brush)bc.ConvertFrom("#FF99B4D1");
                TextBox_TargetID.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
            else
            {
                TextBox_TargetID.Background = (Brush)bc.ConvertFrom("#FFFA8072");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if(parcelSelected.PickedUp == null)
            {
                bl.ParcelCollectionByDrone(parcelSelected.drone.DroneId);               
                MessageBox.Show("Parcel has been picked up");
                TextBox_DisplayStatus.Text = "Picked-up";
            }
            else
            {
                bl.DeliveryOfParcelByDrone(parcelSelected.drone.DroneId);
                MessageBox.Show("Parcel has been delivered");
                TextBox_DisplayStatus.Text = "Delivered";
            }
        }

        private void OpenDroneInParcel(object sender, MouseButtonEventArgs e)
        {
            DroneInParcel temp = Listview_droneinparcel.SelectedItem as DroneInParcel; 
            Drone drone = bl.DroneDisplay(temp.DroneId);
            DronePage dronePage = new DronePage(drone, managerPage, mainWindow);
            mainWindow.Content = dronePage;
        }

        private void OpenSenderInParcel(object sender, MouseButtonEventArgs e)
        {
            CustomerInParcel temp = Listview_Sender.SelectedItem as CustomerInParcel;
            Customer customer = bl.CustomerDisplay(temp.Id);
            CustomerPage customerPage = new CustomerPage(mainWindow, customer, managerPage);
            mainWindow.Content = customerPage;
        }

        private void OpenTargetInParcel(object sender, MouseButtonEventArgs e)
        {
            CustomerInParcel temp = Listview_Target.SelectedItem as CustomerInParcel;
            Customer customer = bl.CustomerDisplay(temp.Id);
            CustomerPage customerPage = new CustomerPage(mainWindow, customer, managerPage);
            mainWindow.Content = customerPage;
        }
    }
}
