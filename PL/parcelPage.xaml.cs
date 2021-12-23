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
        BO.ParcelToList selected = new BO.ParcelToList();
        BO.Parcel parcelSelected = new BO.Parcel();
        public parcelPage(ManagerPage manager, MainWindow main)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            mainWindow = main;
            managerPage = manager;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag;
                int id;
                BO.Parcel newParcel = new BO.Parcel();
                flag = int.TryParse(TextBox_SenderID.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error, sender id was not entered ");
                    return;
                }
                BO.CustomerInParcel send = new CustomerInParcel();
                send.Id = int.Parse(TextBox_SenderID.Text);
                send.Name = bl.CustomerDisplay(int.Parse(TextBox_SenderID.Text)).Name;
                newParcel.Sender = send;
                flag = int.TryParse(TextBox_TargetID.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error, target id was not entered ");
                    return;
                }
                BO.CustomerInParcel receive = new CustomerInParcel();
                receive.Id = int.Parse(TextBox_TargetID.Text);
                receive.Name = bl.CustomerDisplay(int.Parse(TextBox_TargetID.Text)).Name;
                newParcel.Target = receive;
                if (WeightSelector.SelectedItem == null)
                {
                    MessageBox.Show("error, weight was not entered ");
                    return;
                }
                newParcel.Weight = (WeightCategories)WeightSelector.SelectedItem;
                if (PrioritySelector.SelectedItem == null)
                {
                    MessageBox.Show("error, priority was not entered ");
                    return;
                }
                newParcel.Priority = (Priorities)PrioritySelector.SelectedItem;
                bl.AddParcel(newParcel);
                managerPage.listParcel.Items.Refresh();
                MessageBox.Show("Added successfully");
                TextBox_SenderID.IsEnabled = false;
                TextBox_TargetID.IsEnabled = false;
                WeightSelector.IsEnabled = false;
                PrioritySelector.IsEnabled = false;
                add_button.IsEnabled = false;
                managerPage.FilterRefreshParcels();
                Cancel_Add_Button_Click(sender, e);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        public parcelPage(MainWindow main, object selectedItem, ManagerPage manager)
        {
            InitializeComponent();
            mainWindow = main;
            managerPage = manager;
            bl = BlApi.BlFactory.GetBl();
            WeightSelector.Visibility = Visibility.Hidden;
            PrioritySelector.Visibility = Visibility.Hidden;
            label_SenderID.Content = "Sender:";
            selected = (ParcelToList)selectedItem;
            parcelSelected = bl.ParcelDisplay(selected.Id);
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
            if (parcelSelected.Delivered != null)
            {
                TextBox_DisplayStatus.Text = "Delivered";
            }
            else if (parcelSelected.PickedUp != null)
            {
                TextBox_DisplayStatus.Text = "Picked Up";
                TextBox_DroneInParcel.Text = parcelSelected.drone.ToString();
            }
            else if (parcelSelected.Affiliation != null)
            {
                TextBox_DisplayStatus.Text = "Affiliated";
                TextBox_DroneInParcel.Text = parcelSelected.drone.ToString();
            }
            else
                TextBox_DisplayStatus.Text = "Created";

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

        }
    }
}
