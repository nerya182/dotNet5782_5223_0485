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
        BO.Parcel selected = new BO.Parcel();
        BO.Parcel parcelSelected = new BO.Parcel();
        public parcelPage()
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            label_DroneInParcel.Visibility = Visibility.Hidden;
            Sender.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            WeightSelector.Text = "Select weight";
            PrioritySelector.Text = "Select priority";
            StatusSelector.Text = "Select Status";
            WeightSelector.IsEditable = true;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
        public parcelPage(object selectedItem)
        {
            
        }
        public parcelPage(Parcel parcel)
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBl();
            WeightSelector.Visibility = Visibility.Hidden;
            label_SenderID.Visibility = Visibility.Hidden;
            selected = parcel;
            parcelSelected = bl.ParcelDisplay(selected.Id);
            if (parcelSelected.Affiliation == null || parcelSelected.Delivered != null)
                Update_parcel.Visibility = Visibility.Hidden;
            else if (parcelSelected.Delivered == null && parcelSelected.PickedUp != null)
                Update_parcel.Content = "Deliver Parcel";
            else if (parcelSelected.PickedUp == null && parcelSelected.Affiliation != null)
                Update_parcel.Content = "Pick-Up Parcel";
            TextBox_DisplayPriority.IsEnabled = false;
            label_TargetID.Content = "Target:";
            TextBox_DisplayWeight.Text = parcelSelected.Weight.ToString();
            TextBox_DisplayPriority.Text = parcelSelected.Priority.ToString();
            Listview_Sender.Items.Add(parcelSelected.Sender);
            Listview_Target.Items.Add(parcelSelected.Target);
            if (parcelSelected.Delivered != null)
            {
                TextBox_DisplayStatus.Text = "Delivered";
                Listview_droneinparcel.Visibility = Visibility.Hidden;
                label_DroneInParcel.Visibility = Visibility.Hidden;
            }
            else if (parcelSelected.PickedUp != null)
            {
                TextBox_DisplayStatus.Text = "Picked Up";
                Listview_droneinparcel.Items.Add(parcelSelected.drone);
            }
            else if (parcelSelected.Affiliation != null)
            {
                TextBox_DisplayStatus.Text = "Affiliated"; 
                Listview_droneinparcel.Items.Add(parcelSelected.drone);
            }
            else
            {
                TextBox_DisplayStatus.Text = "Created";
                Listview_droneinparcel.Visibility = Visibility.Hidden;
                label_DroneInParcel.Visibility = Visibility.Hidden;
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
        }

        private void Back_Button_Click1(object sender, RoutedEventArgs e)
        {
          
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

        private void OpenDroneInParcel(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void OpenSenderInParcel(object sender, MouseButtonEventArgs e)
        {
          
        }

        private void OpenTargetInParcel(object sender, MouseButtonEventArgs e)
        {
           
        }
    }
}
