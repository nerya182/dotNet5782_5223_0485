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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        BlApi.IBL blw;
        MainWindow main;
        bool flagClosure = true;
        public ParcelListWindow(BlApi.IBL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            blw = bl;
            WeightCategoriesSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            ParcelsListView.ItemsSource = blw.GetParcels();
            main = mainWindow;
            StatusSelector.Text = "Select status";
            WeightCategoriesSelector.Text = "Select weight";
            PrioritySelector.Text = "Select Priority";
            StatusSelector.IsEditable = true;
            WeightCategoriesSelector.IsEditable = true;
            PrioritySelector.IsEditable = true;
        }

        private void ParcelsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelsListView.ItemsSource = blw.GetParcels();
        }

        private void DoubleClickUpdateParcel(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ParcelPage parcelWindow = new ParcelPage(blw, ParcelsListView.SelectedItem, this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flagClosure = false;
            this.Close();
            main.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosed(e);
            e.Cancel = flagClosure;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ParcelPage(blw, this);
            //ParcelWindow parcelWindow = new ParcelWindow(blw, this);
            //this.Visibility = Visibility.Hidden;
            //parcelWindow.Show();
            ParcelsListView.Items.Refresh();
            ParcelsListView.ItemsSource = blw.GetParcels();
            if (WeightCategoriesSelector.SelectedItem != null)
            {
                BO.WeightCategories selectedWeight = (BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
                ParcelsListView.ItemsSource = blw.GetParcelByWeight(ParcelsListView.ItemsSource, selectedWeight);
            }
            if (StatusSelector.SelectedItem != null)
            {
                BO.ParcelStatus selectedStatus = (BO.ParcelStatus)StatusSelector.SelectedItem;
                ParcelsListView.ItemsSource = blw.GetParcelByStatus(ParcelsListView.ItemsSource, selectedStatus);
            }
            if (PrioritySelector.SelectedItem != null)
            {
                BO.Priorities selectedPriority = (BO.Priorities)PrioritySelector.SelectedItem;
                ParcelsListView.ItemsSource = blw.GetParcelByPriority(ParcelsListView.ItemsSource, selectedPriority);
            }        
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetParcelsList();
            if (StatusSelector.SelectedItem == null) return;
            BO.ParcelStatus selectedStatus = (BO.ParcelStatus)StatusSelector.SelectedItem;
            ParcelsListView.ItemsSource = blw.GetParcelByStatus(ParcelsListView.ItemsSource, selectedStatus);
            if (WeightCategoriesSelector.SelectedItem == null) return;
            BO.WeightCategories selectedWeight = (BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
            ParcelsListView.ItemsSource = blw.GetParcelByWeight(ParcelsListView.ItemsSource, selectedWeight);
            if (PrioritySelector.SelectedItem == null) return;
            BO.Priorities selectedPriority = (BO.Priorities)PrioritySelector.SelectedItem;
            ParcelsListView.ItemsSource = blw.GetParcelByPriority(ParcelsListView.ItemsSource, selectedPriority);
        }

        private void WeightCategoriesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetParcelsList();
            if (WeightCategoriesSelector.SelectedItem == null) return;
            BO.WeightCategories selectedWeight = (BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
            ParcelsListView.ItemsSource = blw.GetParcelByWeight(ParcelsListView.ItemsSource, selectedWeight);
            if (StatusSelector.SelectedItem == null) return;
            BO.ParcelStatus selectedStatus = (BO.ParcelStatus)StatusSelector.SelectedItem;
            ParcelsListView.ItemsSource = blw.GetParcelByStatus(ParcelsListView.ItemsSource, selectedStatus);           
            if (PrioritySelector.SelectedItem == null) return;
            BO.Priorities selectedPriority = (BO.Priorities)PrioritySelector.SelectedItem;
            ParcelsListView.ItemsSource = blw.GetParcelByPriority(ParcelsListView.ItemsSource, selectedPriority);
        }

        private void Priority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetParcelsList();
            if (PrioritySelector.SelectedItem == null) return;
            BO.Priorities selectedPriority = (BO.Priorities)PrioritySelector.SelectedItem;
            ParcelsListView.ItemsSource = blw.GetParcelByPriority(ParcelsListView.ItemsSource, selectedPriority);
            if (StatusSelector.SelectedItem == null) return;
            BO.ParcelStatus selectedStatus = (BO.ParcelStatus)StatusSelector.SelectedItem;
            ParcelsListView.ItemsSource = blw.GetParcelByStatus(ParcelsListView.ItemsSource, selectedStatus);
            if (WeightCategoriesSelector.SelectedItem == null) return;
            BO.WeightCategories selectedWeight = (BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
            ParcelsListView.ItemsSource = blw.GetParcelByWeight(ParcelsListView.ItemsSource, selectedWeight);
        }

        private void resetParcelsList()
        {
            ParcelsListView.ItemsSource = blw.GetParcels();
            ParcelsListView.Items.Refresh();
        }

        public void resetComboBoxes()
        {
            WeightCategoriesSelector.SelectedItem = null;
            StatusSelector.SelectedItem = null;
            PrioritySelector.SelectedItem = null;
        }
        public void resetParcelsList_click(object sender, RoutedEventArgs e)
        {
            resetParcelsList();
            resetComboBoxes();
        }

        private void Button_reset_Click(object sender, RoutedEventArgs e)
        {
            resetComboBoxes();
            ParcelsListView.ItemsSource = blw.GetParcels();
            StatusSelector.IsEditable = true;
            WeightCategoriesSelector.IsEditable = true;
            PrioritySelector.IsEditable = true;
            StatusSelector.Text = "Select status";
            WeightCategoriesSelector.Text = "Select max weight";
            PrioritySelector.Text = "Select priority";
        }

        public void Filterrefresh()
        {
            resetParcelsList();
            if (WeightCategoriesSelector.SelectedItem != null)
            {
                BO.WeightCategories selectedWeight = (BO.WeightCategories)WeightCategoriesSelector.SelectedItem;
                ParcelsListView.ItemsSource = blw.GetParcelByWeight(ParcelsListView.ItemsSource, selectedWeight);
            }
            if (StatusSelector.SelectedItem != null)
            {
                BO.ParcelStatus selectedStatus = (BO.ParcelStatus)StatusSelector.SelectedItem;
                ParcelsListView.ItemsSource = blw.GetParcelByStatus(ParcelsListView.ItemsSource, selectedStatus);
            }
            if (PrioritySelector.SelectedItem != null)
            {
                BO.Priorities selectedPriority = (BO.Priorities)PrioritySelector.SelectedItem;
                ParcelsListView.ItemsSource = blw.GetParcelByPriority(ParcelsListView.ItemsSource, selectedPriority);
            }
            ParcelsListView.Items.Refresh();
        }
    }
}
