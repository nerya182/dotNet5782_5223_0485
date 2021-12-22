using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ParcelPage.xaml
    /// </summary>
    public partial class ParcelPage : Page
    {
        BlApi.IBL bldw;
        BO.ParcelToList selected = new BO.ParcelToList();
        BO.Parcel parcelSelected = new BO.Parcel();
        ParcelListWindow parcelListWin;
        bool flagClosure = true;
        public ParcelPage(BlApi.IBL blw, ParcelListWindow w)
        {
            InitializeComponent();
            bldw = blw;
            parcelListWin = w;
            close_button.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
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
                send.Name = bldw.CustomerDisplay(int.Parse(TextBox_SenderID.Text)).Name;
                newParcel.Sender = send;
                flag = int.TryParse(TextBox_TargetID.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error, target id was not entered ");
                    return;
                }
                BO.CustomerInParcel receive = new CustomerInParcel();
                receive.Id = int.Parse(TextBox_TargetID.Text);
                receive.Name = bldw.CustomerDisplay(int.Parse(TextBox_TargetID.Text)).Name;
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
                bldw.AddParcel(newParcel);
                parcelListWin.ParcelsListView.Items.Refresh();
                MessageBox.Show("Added successfully");
                TextBox_SenderID.IsEnabled = false;
                TextBox_TargetID.IsEnabled = false;
                WeightSelector.IsEnabled = false;
                PrioritySelector.IsEnabled = false;
                add_button.IsEnabled = false;
                flagClosure = false;
                //this.Close();
                parcelListWin.Filterrefresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        public ParcelPage(BlApi.IBL blw, object selectedItem, ParcelListWindow w)
        {
            InitializeComponent();
            parcelListWin = w;
            bldw = blw;
            close_button.Visibility = Visibility.Visible;

        }

        private void Cancel_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            flagClosure = false;
            //this.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            flagClosure = false;
            //this.Close();
            parcelListWin.Show();
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

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

