using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL bl = BlApi.BlFactory.GetBl();
        ManagerPage managerPage;
        DronePage dronePage;
        parcelPage parcelPage;
        StationPage stationPage;
        CustomerPage customerPage;
        Customer customer;
        Drone drone;
        int selectedTab;
        object selectedItem;
        public MainWindow()
        {
            InitializeComponent();
            manager.Visibility = Visibility.Hidden;
            add.Visibility = Visibility.Hidden;
            enter.Visibility = Visibility.Hidden;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            start.Visibility = Visibility.Hidden;
            manager.Visibility = Visibility.Visible;
        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            managerPage= new ManagerPage();
            managerPage.ADD.Click += ADD_Click;
            managerPage.listStaions.MouseDoubleClick += ListStaions_MouseDoubleClick;
            managerPage.listParcel.MouseDoubleClick += ListParcel_MouseDoubleClick;
            managerPage.listCustomers.MouseDoubleClick += ListCustomers_MouseDoubleClick;
            managerPage.listDrones.MouseDoubleClick += ListDrones_MouseDoubleClick;
            managerPage.deleteButton.Click += DeleteButton_Click;
            managerPage.Grouping_first.Click += Grouping_first_Click1;
            managerPage.Grouping_seconde.Click += Grouping_seconde_Click;
            managerPage.ComboBox_WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            managerPage.ComboBox_StatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            managerPage.ComboBox_PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            managerPage.ComboBox_WeightSelector.SelectionChanged += WeightSelector_SelectionChanged;
            managerPage.ComboBox_StatusSelector.SelectionChanged += StatusSelector_SelectionChanged;
            managerPage.ComboBox_PrioritySelector.SelectionChanged += PrioritySelector_SelectionChanged;
            Observables();
            this.Content = managerPage;
        }

        private void Observables()
        {
            managerPage.stationToListObservabl = new ObservableCollection<StationToList>();
            managerPage.parcelToListObservabl= new ObservableCollection<ParcelToList>();
            managerPage.customerToListObservabl= new ObservableCollection<CustomerToList>();
            managerPage.droneToListObservabl = new ObservableCollection<DroneToList>();
            List<StationToList> stationToLists = (List<StationToList>)bl.GetStations();
            foreach (var item in stationToLists)
            {
                managerPage.stationToListObservabl.Add(item);
            }
            managerPage.listStaions.DataContext = managerPage.stationToListObservabl;

            List<DroneToList> droneToLists = (List<DroneToList>)bl.GetListDrone();
            foreach (var item in droneToLists)
            {
                managerPage.droneToListObservabl.Add(item);
            }
            managerPage.listDrones.DataContext = managerPage.droneToListObservabl;

            List<CustomerToList> customerToLists = (List<CustomerToList>)bl.GetListCustomer();
            foreach (var item in customerToLists)
            {
                managerPage.customerToListObservabl.Add(item);
            }
            managerPage.listCustomers.DataContext = managerPage.customerToListObservabl;

            List<ParcelToList> parcelToLists = (List<ParcelToList>)bl.GetParcels();
            foreach (var item in parcelToLists)
            {
                managerPage.parcelToListObservabl.Add(item);
            }
            managerPage.listParcel.DataContext = managerPage.parcelToListObservabl;
        }

        private void Grouping_first_Click1(object sender, RoutedEventArgs e)
        {
            switch (managerPage.TabManager.SelectedIndex)
            {
                case 0:
                    managerPage.listDrones.ItemsSource = bl.GroupingStatus();
                    break;
                case 2:
                    managerPage.listStaions.ItemsSource = bl.GroupingAvailableChargeSlots();
                    break;
                case 1:
                    managerPage.listParcel.ItemsSource = bl.GroupingSenderName();
                    break;
            }
            
        }

        private void Grouping_seconde_Click(object sender, RoutedEventArgs e)
        {
            switch (managerPage.TabManager.SelectedIndex)
            {
                case 0:
                    managerPage.listDrones.ItemsSource = bl.GroupingWeight();
                    break;
                case 2:
                    managerPage.listStaions.ItemsSource = bl.GroupingChargeSlots();
                    break;
                case 1:
                    managerPage.listParcel.ItemsSource = bl.GroupingTargetNam();
                    break;
            } 
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (managerPage.ComboBox_WeightSelector.SelectedItem != null)
            {
                BO.WeightCategories selectedWeight = (BO.WeightCategories)managerPage.ComboBox_WeightSelector.SelectedItem;
                managerPage.listParcel.ItemsSource = bl.GetParcelByWeight(selectedWeight);
            }
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (managerPage.ComboBox_StatusSelector.SelectedItem != null)
            {
                BO.ParcelStatus selectedStatus = (BO.ParcelStatus)managerPage.ComboBox_StatusSelector.SelectedItem;
                managerPage.listParcel.ItemsSource = bl.GetParcelByStatus(selectedStatus);
            }
        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (managerPage.ComboBox_PrioritySelector.SelectedItem != null)
            {
                BO.Priorities selectedPriority = (BO.Priorities)managerPage.ComboBox_PrioritySelector.SelectedItem;
                managerPage.listParcel.ItemsSource = bl.GetParcelByPriority(selectedPriority);
            }
        }



        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            selectedTab = managerPage.TabManager.SelectedIndex;
            object selectedDelete;
            switch (selectedTab)
            {
                case 1:
                    try
                    {
                        selectedDelete = (ParcelToList)managerPage.listParcel.SelectedItem;
                        bl.DeleteParcel((ParcelToList)selectedDelete);
                        managerPage.listParcel.ItemsSource = bl.GetParcels();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
                case 2:
                    try
                    {
                        selectedDelete = (StationToList)managerPage.listStaions.SelectedItem;
                        bl.DeleteStation((StationToList)selectedDelete);
                        managerPage.listStaions.ItemsSource = bl.GetStations();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    break;
            }
        }

        private void ListDrones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem =managerPage.listDrones.SelectedItem;
            DroneToList droneToList = (DroneToList)selectedItem;
            drone = bl.DroneDisplay(droneToList.Id);
            dronePage = new DronePage(drone);
            dronePage.changeModelButton.Click += ChangeModelButton_Click;
            dronePage.sendOrReleaseButton.Click += SendOrReleaseButton_Click;
            dronePage.Back_Button.Click += Back_Button_Click;
            dronePage.delivery.Click += Delivery_Click;
            dronePage.NewModel.Click += NewModel_Click;
            dronePage.ListParcelTransfer.MouseDoubleClick += ListParcelTransfer_MouseDoubleClick;
            this.Content = dronePage;
        }

        private void ListParcelTransfer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelTransfer temp = dronePage.ListParcelTransfer.SelectedItem as ParcelTransfer;
            Parcel parcel = bl.ParcelDisplay(temp.Id);
            parcelPage parcelPage = new parcelPage(parcel);          
            parcelPage.Back_Button.Click += Back_Button_Click4; 
            this.Content = parcelPage;
        }

        private void Back_Button_Click4(object sender, RoutedEventArgs e)
        {
            selectedItem = managerPage.listDrones.SelectedItem;
            DroneToList droneToList = (DroneToList)selectedItem;
            drone = bl.DroneDisplay(droneToList.Id);
            dronePage = new DronePage(drone);
            dronePage.changeModelButton.Click += ChangeModelButton_Click;
            dronePage.sendOrReleaseButton.Click += SendOrReleaseButton_Click;
            dronePage.Back_Button.Click += Back_Button_Click;
            dronePage.delivery.Click += Delivery_Click;
            dronePage.NewModel.Click += NewModel_Click;
            dronePage.ListParcelTransfer.MouseDoubleClick += ListParcelTransfer_MouseDoubleClick;
            this.Content = dronePage;
        }

        private void NewModel_Click(object sender, RoutedEventArgs e)
        {
            BO.DroneToList selected = new BO.DroneToList();
            selected = (DroneToList)managerPage.listDrones.SelectedItem;
            BO.DroneToList updateDrone = selected;
            try
            {
                updateDrone.Model = dronePage.TextBoxNewModel.Text;
                bl.UpdateDrone(updateDrone);
                MessageBox.Show("Update successfully");
                dronePage.TextBox_model.Text = dronePage.TextBoxNewModel.Text;
                dronePage.TextBoxNewModel.Visibility = Visibility.Hidden;
                dronePage.labelTextBoxNewModel.Visibility = Visibility.Hidden;
                dronePage.NewModel.Visibility = Visibility.Hidden;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Delivery_Click(object sender, RoutedEventArgs e)
        {
            BO.DroneToList selected = new BO.DroneToList();
            selected = (DroneToList)managerPage.listDrones.SelectedItem;
            int droneId = selected.Id;
            dronePage.TextBoxNewModel.Visibility = Visibility.Hidden;
            dronePage.labelTextBoxNewModel.Visibility = Visibility.Hidden;
            dronePage.NewModel.Visibility = Visibility.Hidden;
            string str = (string)dronePage.delivery.Content;
            if (str == "Affiliation")
            {
                try
                {
                    droneId = selected.Id;
                    bl.AffiliateParcelToDrone(droneId);
                    dronePage.TextBoxLongitude.Text = bl.DroneDisplay(droneId).ParcelTransfer.ToString();
                    //dronePage.TextBoxParcelTransfer.Text = ((int)bl.DroneDisplay(droneId).Battery).ToString();
                    dronePage.TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    dronePage.TextBoxDelivery.Text = DroneStatuses.Delivery.ToString();
                    MessageBox.Show("Update successfully");
                    dronePage.sendOrReleaseButton.Content = "Package collection";
                    dronePage.sendOrReleaseButton.Visibility = Visibility.Visible;
                    dronePage.delivery.Visibility = Visibility.Hidden;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void ListCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList temp = (CustomerToList)managerPage.listCustomers.SelectedItem;
            customer = bl.CustomerDisplay(temp.Id);
            customerPage = new CustomerPage(customer);
            customerPage.NameButton.Click += NameButton_Click;
            customerPage.NewUpdate.Click += NewUpdate_Click;
            customerPage.PhoneButton.Click += PhoneButton_Click;
            customerPage.close_customer.Click += Close_customer_Click;
            customerPage.listFromeCustomer.MouseDoubleClick += Listview_FromCustomer_MouseDoubleClick;
            customerPage.listToCustomer.MouseDoubleClick += Listview_ToCustomer_MouseDoubleClick;
            this.Content = customerPage;
        }

        private void PhoneButton_Click(object sender, RoutedEventArgs e)
        {
            customerPage.label_TextBoxNew.Content = "Insert a new phone";
            customerPage.label_TextBoxNew.Visibility = Visibility.Visible;
            customerPage.TextBoxNewModel.Visibility = Visibility.Visible;
            customerPage.NewUpdate.Visibility = Visibility.Visible;
        }

        private void NewUpdate_Click(object sender, RoutedEventArgs e)
        {
            string str = customerPage.label_TextBoxNew.Content.ToString();
            BO.Customer updateCustomer = (Customer)managerPage.listCustomers.SelectedItem;
            try
            {
                if (str == "Insert a new phone")
                {
                    updateCustomer.Phone = customerPage.TextBoxNewModel.Text;
                }
                else
                {
                    updateCustomer.Name = customerPage.TextBoxNewModel.Text;
                }
                bl.UpdateCustomer(updateCustomer);
                MessageBox.Show("Update successfully");
                customerPage.label_TextBoxNew.Visibility = Visibility.Hidden;
                customerPage.TextBoxNewModel.Visibility = Visibility.Hidden;
                customerPage.NewUpdate.Visibility = Visibility.Hidden;
                customerPage.PhoneTextBox.Text = updateCustomer.Phone;
                customerPage.TextBox_name.Text = updateCustomer.Name.ToString();
                managerPage.listCustomers.ItemsSource = bl.GetListCustomer();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void NameButton_Click(object sender, RoutedEventArgs e)
        {
            customerPage.label_TextBoxNew.Visibility = Visibility.Visible;
            customerPage.TextBoxNewModel.Visibility = Visibility.Visible;
            customerPage.NewUpdate.Visibility = Visibility.Visible;
        }

        private void ListParcel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList temp =(ParcelToList)managerPage.listParcel.SelectedItem;
            Parcel parcel = bl.ParcelDisplay(temp.Id);
            parcelPage = new parcelPage(parcel);
            parcelPage.Back_Button.Click += Back_Button_Click1;
            parcelPage.Update_parcel.Click += Update_parcel_Click;
            parcelPage.Listview_droneinparcel.MouseDoubleClick += Listview_droneinparcel_MouseDoubleClick;
            parcelPage.Listview_Target.MouseDoubleClick += Listview_Target_MouseDoubleClick;
            parcelPage.Listview_Sender.MouseDoubleClick += Listview_Sender_MouseDoubleClick;
            this.Content =parcelPage;
        }

        private void Update_parcel_Click(object sender, RoutedEventArgs e)
        {
            ParcelToList parcelToList = (ParcelToList)managerPage.listParcel.SelectedItem;
            Parcel parcelSelected = bl.ParcelDisplay(parcelToList.Id);
            if (parcelSelected.PickedUp == null)
            {
                bl.ParcelCollectionByDrone(parcelSelected.drone.DroneId);
                MessageBox.Show("Parcel has been picked up");
                parcelPage.TextBox_DisplayStatus.Text = "Picked-up";
            }
            else
            {
                bl.DeliveryOfParcelByDrone(parcelSelected.drone.DroneId);
                MessageBox.Show("Parcel has been delivered");
                parcelPage.TextBox_DisplayStatus.Text = "Delivered";
            }
            parcelPage.Update_parcel.Visibility = Visibility.Hidden;
        }

        private void Back_Button_Click1(object sender, RoutedEventArgs e)
        {
            this.Content = managerPage;
            managerPage.listParcel.ItemsSource = bl.GetParcels();
            managerPage.TabManager.SelectedIndex = 1;
        }

        private void Listview_Sender_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerInParcel temp = parcelPage.Listview_Sender.SelectedItem as CustomerInParcel;
            Customer customer = bl.CustomerDisplay(temp.Id);
            customerPage = new CustomerPage(customer);
            customerPage.close_customer.Click += Back_Button_Click3;
            this.Content = customerPage;
        }

        private void Listview_Target_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerInParcel temp = parcelPage.Listview_Target.SelectedItem as CustomerInParcel;
            Customer customer = bl.CustomerDisplay(temp.Id);
            customerPage = new CustomerPage(customer);
            customerPage.close_customer.Click += Back_Button_Click3;
            this.Content = customerPage;
        }

        private void Listview_droneinparcel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneInParcel temp =  parcelPage.Listview_droneinparcel.SelectedItem as DroneInParcel;
            Drone drone = bl.DroneDisplay(temp.DroneId);
            dronePage = new DronePage(drone);
            dronePage.Back_Button.Click += Back_Button_Click3;
            this.Content = dronePage;
        }

        private void Back_Button_Click3(object sender, RoutedEventArgs e)
        {
            ParcelToList temp = (ParcelToList)managerPage.listParcel.SelectedItem;
            Parcel parcel = bl.ParcelDisplay(temp.Id);
            parcelPage = new parcelPage(parcel);
            parcelPage.Back_Button.Click += Back_Button_Click1;
            parcelPage.Update_parcel.Click += Update_parcel_Click;
            parcelPage.Listview_droneinparcel.MouseDoubleClick += Listview_droneinparcel_MouseDoubleClick;
            parcelPage.Listview_Target.MouseDoubleClick += Listview_Target_MouseDoubleClick;
            parcelPage.Listview_Sender.MouseDoubleClick += Listview_Sender_MouseDoubleClick;
            this.Content = parcelPage;
        }

        private void ListStaions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = managerPage.listStaions.SelectedItem;
            stationPage = new StationPage(selectedItem);
            stationPage.close_button.Click += Close_button_Click1;
            stationPage.UpdateName.Click += UpdateName_Click;
            stationPage.NewName.Click += NewName_Click;
            stationPage.listOfDrones.MouseDoubleClick += ListOfDrones_MouseDoubleClick;
            this.Content = stationPage;
        }

        private void ListOfDrones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneInCharging temp = stationPage.listOfDrones.SelectedItem as DroneInCharging;
            Drone drone = bl.DroneDisplay(temp.DroneId);
            dronePage = new DronePage(drone);
            dronePage.Back_Button.Click += Back_Button_Click5; ;
            this.Content = dronePage;
        }

        private void Back_Button_Click5(object sender, RoutedEventArgs e)
        {
            selectedItem = managerPage.listStaions.SelectedItem;
            stationPage = new StationPage(selectedItem);
            stationPage.close_button.Click += Close_button_Click1;
            stationPage.UpdateName.Click += UpdateName_Click;
            stationPage.NewName.Click += NewName_Click;
            stationPage.listOfDrones.MouseDoubleClick += ListOfDrones_MouseDoubleClick;
            this.Content = stationPage;
        }

        private void UpdateName_Click(object sender, RoutedEventArgs e)
        {
            stationPage.labelTextBoxNewName.Visibility = Visibility.Visible;
            stationPage.TextBoxNewName.Visibility = Visibility.Visible;
            stationPage.NewName.Visibility = Visibility.Visible;   
        }

        private void Close_button_Click1(object sender, RoutedEventArgs e)
        {
            this.Content = managerPage;
            managerPage.TabManager.SelectedIndex = 2;
            managerPage.listStaions.ItemsSource = bl.GetStations();
        }

        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            selectedTab = managerPage.TabManager.SelectedIndex;
            switch (selectedTab)
            {
                case 0:
                    dronePage = new DronePage();
                    dronePage.add_button.Click += Add_button_Click;
                    dronePage.Back_Button.Click += Back_Button_Click;
                    this.Content = dronePage;
                    break;
                case 1:
                    parcelPage = new parcelPage();
                    parcelPage.add_button.Click += Add_button_Click1;
                    parcelPage.Back_Button.Click += Back_Button_Click1;
                    this.Content = parcelPage;
                    break;
                case 2:
                    stationPage = new StationPage();
                    stationPage.add_button.Click += Add_button_Click2;
                    stationPage.close_button.Click += Close_button_Click;
                    this.Content = stationPage;
                    break;
                case 3:
                    customerPage = new CustomerPage();
                    customerPage.add_button.Click += Add_button_Click3;
                    customerPage.close_customer.Click += Close_customer_Click;
                    this.Content = customerPage;
                    break;
            }
        }

        private void Close_customer_Click(object sender, RoutedEventArgs e)
        {
            this.Content = managerPage;
            managerPage.TabManager.SelectedIndex = 3;
            managerPage.listCustomers.ItemsSource = bl.GetListCustomer();
        }

        private void Add_button_Click3(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag;
                int id;
                double location;
                BO.Customer newCustomer = new BO.Customer();
                flag = int.TryParse(customerPage.TextBox_id.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error ,customer id was not entered ");
                    return;
                }
                newCustomer.Id = int.Parse(customerPage.TextBox_id.Text);
                if (customerPage.TextBox_name.Text == null)
                {
                    MessageBox.Show("error ,name customer was not entered ");
                    return;
                }
                newCustomer.Name = customerPage.TextBox_name.Text;
                if (customerPage.PhoneTextBox.Text == null)
                {
                    MessageBox.Show("error ,phone customer was not entered ");
                    return;
                }
                newCustomer.Phone = customerPage.PhoneTextBox.Text;
                BO.Location locationCustomer = new BO.Location();
                flag = double.TryParse(customerPage.TextBoxLatitude.Text, out location);
                if (!flag)
                {
                    MessageBox.Show("error ,latitude was not entered ");
                    return;
                }
                locationCustomer.Lattitude = location;
                flag = double.TryParse(customerPage.TextBox_longitude.Text, out location);
                if (!flag)
                {
                    MessageBox.Show("error ,longitude was not entered ");
                    return;
                }
                locationCustomer.Longitude = location;
                newCustomer.Location = locationCustomer;
                bl.AddCustomer(newCustomer);
                MessageBox.Show("Added successfully");
                customerPage.TextBox_id.IsEnabled = false;
                customerPage.TextBox_name.IsEnabled = false;
                customerPage.PhoneTextBox.IsEnabled = false;
                customerPage.TextBoxLatitude.IsEnabled = false;
                customerPage.TextBox_longitude.IsEnabled = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Content=managerPage;
            managerPage.TabManager.SelectedIndex = 2;
        }

        private void NewName_Click(object sender, RoutedEventArgs e)
        {
            BO.StationToList updatedStation = (StationToList)managerPage.listStaions.SelectedItem;
            try
            {
                updatedStation.Name = stationPage.TextBoxNewName.Text;
                bl.UpdateStationName(updatedStation.Id, updatedStation.Name);
                MessageBox.Show("Update successfully");
                stationPage.TextBox_name.Text = stationPage.TextBoxNewName.Text;
                stationPage.TextBoxNewName.Visibility = Visibility.Hidden;
                stationPage.labelTextBoxNewName.Visibility = Visibility.Hidden;
                stationPage.NewName.Visibility = Visibility.Hidden;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

     
        private void Add_button_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag;
                int id;
                double num;
                BO.Station newStation = new BO.Station();
                flag = int.TryParse(stationPage.TextBox_id.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("Error, station id was not entered ");
                    return;
                }
                newStation.Id = int.Parse(stationPage.TextBox_id.Text);
                if (stationPage.TextBox_name.Text == null)
                {
                    MessageBox.Show("Error, station name was not entered ");
                    return;
                }
                newStation.Name = stationPage.TextBox_name.Text;
                flag = int.TryParse(stationPage.TextBoxChargeSlots.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("Error, amount of slots was not entered ");
                    return;
                }
                newStation.AvailableChargeSlots = int.Parse(stationPage.TextBoxChargeSlots.Text);
                flag = double.TryParse(stationPage.TextBoxLattitude.Text, out num);
                if (!flag)
                {
                    MessageBox.Show("Error, Lattitude was not entered ");
                    return;
                }
                Location location = new Location();
                location.Lattitude = double.Parse(stationPage.TextBoxLattitude.Text);
                flag = double.TryParse(stationPage.TextBoxLongitude.Text, out num);
                if (!flag)
                {
                    MessageBox.Show("Error, Longtitude was not entered ");
                    return;
                }
                location.Lattitude = double.Parse(stationPage.TextBoxLongitude.Text);
                newStation.location = location;
                bl.AddStation(newStation);
                newStation = bl.GetListStation().First(i => i.Id == newStation.Id);
                managerPage.stationToListObservabl.Add(bl.MakeStationToList(newStation));
                MessageBox.Show("Added successfully");
                stationPage.TextBox_id.IsEnabled = false;
                stationPage.TextBox_name.IsEnabled = false;
                stationPage.TextBoxChargeSlots.IsEnabled = false;
                stationPage.TextBoxLattitude.IsEnabled = false;
                stationPage.TextBoxLongitude.IsEnabled = false;
                stationPage.add_button.IsEnabled = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Add_button_Click1(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag;
                int id;
                BO.Parcel newParcel = new BO.Parcel();
                flag = int.TryParse(parcelPage.TextBox_SenderID.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error, sender id was not entered ");
                    return;
                }
                BO.CustomerInParcel send = new CustomerInParcel();
                send.Id = int.Parse(parcelPage.TextBox_SenderID.Text);
                send.Name = bl.CustomerDisplay(int.Parse(parcelPage.TextBox_SenderID.Text)).Name;
                newParcel.Sender = send;
                flag = int.TryParse(parcelPage.TextBox_TargetID.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error, target id was not entered ");
                    return;
                }
                BO.CustomerInParcel receive = new CustomerInParcel();
                receive.Id = int.Parse(parcelPage.TextBox_TargetID.Text);
                receive.Name = bl.CustomerDisplay(int.Parse(parcelPage.TextBox_TargetID.Text)).Name;
                newParcel.Target = receive;
                if (parcelPage.WeightSelector.SelectedItem == null)
                {
                    MessageBox.Show("error, weight was not entered ");
                    return;
                }
                newParcel.Weight = (WeightCategories)parcelPage.WeightSelector.SelectedItem;
                if (parcelPage.PrioritySelector.SelectedItem == null)
                {
                    MessageBox.Show("error, priority was not entered ");
                    return;
                }
                newParcel.Priority = (Priorities)parcelPage.PrioritySelector.SelectedItem;
                bl.AddParcel(newParcel);
                MessageBox.Show("Added successfully");
                parcelPage.TextBox_SenderID.IsEnabled = false;
                parcelPage.TextBox_TargetID.IsEnabled = false;
                parcelPage.WeightSelector.IsEnabled = false;
                parcelPage.PrioritySelector.IsEnabled = false;
                parcelPage.add_button.IsEnabled = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Content = managerPage;
            managerPage.TabManager.SelectedIndex = 0;
            managerPage.listDrones.ItemsSource = bl.GetListDrone();
        }

        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag;
                int id;
                int chargingStationId = 0;
                BO.DroneToList newDrone = new BO.DroneToList();
                flag = int.TryParse(dronePage.TextBox_id.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error ,drone id was not entered ");
                    return;
                }
                newDrone.Id = int.Parse(dronePage.TextBox_id.Text);
                if (dronePage.TextBox_model.Text == null)
                {
                    MessageBox.Show("error ,model drone was not entered ");
                    return;
                }
                newDrone.Model = dronePage.TextBox_model.Text;
                if (dronePage.WeightSelector.SelectedItem == null)
                {
                    MessageBox.Show("error ,weight was not entered ");
                    return;
                }
                newDrone.MaxWeight = (WeightCategories)dronePage.WeightSelector.SelectedItem;
                if (dronePage.chargeStationId.SelectedItem == null)
                {
                    MessageBox.Show("error ,charge station id was not entered ");
                    return;
                }
                chargingStationId = (int)dronePage.chargeStationId.SelectedItem;
                dronePage.TextBoxLattitude.Text = bl.BaseStationDisplay(chargingStationId).location.Lattitude.ToString();
                dronePage.TextBoxLongitude.Text = bl.BaseStationDisplay(chargingStationId).location.Longitude.ToString();
                bl.AddDrone(newDrone, chargingStationId);
                MessageBox.Show("Added successfully");
                dronePage.TextBox_id.IsEnabled = false;
                dronePage.chargeStationId.IsEnabled = false;
                dronePage.TextBox_model.IsEnabled = false;
                dronePage.WeightSelector.IsEnabled = false;
                dronePage.add_button.IsEnabled = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SendOrReleaseButton_Click(object sender, RoutedEventArgs e)
        {
            dronePage.TextBoxNewModel.Visibility = Visibility.Hidden;
            dronePage.labelTextBoxNewModel.Visibility = Visibility.Hidden;
            dronePage.NewModel.Visibility = Visibility.Hidden;
            string str = (string)dronePage.sendOrReleaseButton.Content;
            BO.DroneToList selected = new BO.DroneToList();
            selected = (DroneToList)managerPage.listDrones.SelectedItem;
            int droneId = selected.Id;
            if (str == "Sending to charging")
            {
                try
                {
                    bl.SendingDroneForCharging(droneId);
                    dronePage.TextBoxDelivery.Text = DroneStatuses.Charging.ToString();
                    dronePage.TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    MessageBox.Show("Update successfully");
                    dronePage.sendOrReleaseButton.Content = "Release drone";
                    dronePage.sendOrReleaseButton.Visibility = Visibility.Visible;
                    dronePage.delivery.Visibility = Visibility.Hidden;

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            if (str == "Release drone")
            {
                try
                {
                    bl.ReleaseDroneFromCharging(droneId);
                    //dronePage.TextBoxParcelTransfer.Text = ((int)bl.DroneDisplay(droneId).Battery).ToString();
                    dronePage.TextBoxDelivery.Text = DroneStatuses.Available.ToString();
                    dronePage.TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    MessageBox.Show("Update successfully");
                    dronePage.sendOrReleaseButton.Content = "Sending to charging";
                    dronePage.sendOrReleaseButton.Visibility = Visibility.Visible;
                    dronePage.delivery.Content = "Affiliation";
                    dronePage.delivery.Visibility = Visibility.Visible;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            if (str == "Package collection")
            {
                try
                {
                    bl.ParcelCollectionByDrone(droneId);
                    dronePage.TextBoxLongitude.Text = bl.DroneDisplay(droneId).ParcelTransfer.ToString();
                    //dronePage.TextBoxParcelTransfer.Text = ((int)bl.DroneDisplay(droneId).Battery).ToString();
                    dronePage.TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    MessageBox.Show("Update successfully");
                    dronePage.sendOrReleaseButton.Content = "Package delivery";
                    dronePage.sendOrReleaseButton.Visibility = Visibility.Visible;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else if (str == "Package delivery")
            {
                try
                {
                    bl.DeliveryOfParcelByDrone(droneId);
                    dronePage.TextBoxLongitude.Text = bl.DroneDisplay(droneId).ParcelTransfer.ToString();
                   // dronePage.TextBoxParcelTransfer.Text = ((int)bl.DroneDisplay(droneId).Battery).ToString();
                    dronePage.TextBoxDelivery.Text = DroneStatuses.Available.ToString();
                    dronePage.TextBoxLattitude.Text = bl.DroneDisplay(droneId).Location.ToString();
                    MessageBox.Show("Update successfully");
                    dronePage.sendOrReleaseButton.Content = "Sending to charging";
                    dronePage.sendOrReleaseButton.Visibility = Visibility.Visible;
                    dronePage.delivery.Content = "Affiliation";
                    dronePage.delivery.Visibility = Visibility.Visible;

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void ChangeModelButton_Click(object sender, RoutedEventArgs e)
        {
            dronePage.TextBoxNewModel.Visibility = Visibility.Visible;
            dronePage.labelTextBoxNewModel.Visibility = Visibility.Visible;
            dronePage.NewModel.Visibility = Visibility.Visible;
        }

        private void Listview_FromCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParceltAtCustomer temp = customerPage.listFromeCustomer.SelectedItem as ParceltAtCustomer;
            Parcel parcel = bl.ParcelDisplay(temp.Id);
            parcelPage = new parcelPage(parcel);
            this.Content = parcelPage;
        }

        private void Listview_ToCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParceltAtCustomer temp = customerPage.listToCustomer.SelectedItem as ParceltAtCustomer;
            Parcel parcel = bl.ParcelDisplay(temp.Id);
            parcelPage = new parcelPage(parcel);
            this.Content = parcelPage;
        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            enter.Visibility = Visibility.Visible;
            add.Visibility = Visibility.Hidden;
        }

        private void New_Client_Click(object sender, RoutedEventArgs e)
        {
            add.Visibility = Visibility.Visible;
            enter.Visibility = Visibility.Hidden;
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            bool flag;
            int id;
            flag = int.TryParse(textbox_id.Text, out id);
            if (!flag)
            {
                MessageBox.Show("error, id was not entered ");
                return;
            }
            try
            {
                bl.CustomerDisplay(id);
            }
            catch 
            {
                MessageBox.Show("id of customer does not exist");
                return;
            }

        }

        private void AddNewClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag;
                int id;
                double location;
                BO.Customer newCustomer = new BO.Customer();
                flag = int.TryParse(textbox_newId.Text, out id);
                if (!flag)
                {
                    MessageBox.Show("error ,customer id was not entered ");
                    return;
                }
                newCustomer.Id = int.Parse(textbox_newId.Text);
                if (textbox_newName.Text == null)
                {
                    MessageBox.Show("error ,name customer was not entered ");
                    return;
                }
                newCustomer.Name = textbox_newName.Text;
                if (textbox_newNumber.Text == null)
                {
                    MessageBox.Show("error ,phone customer was not entered ");
                    return;
                }
                newCustomer.Phone = textbox_newNumber.Text;
                BO.Location locationCustomer = new BO.Location();
                flag = double.TryParse(textbox_newLatt.Text, out location);
                if (!flag)
                {
                    MessageBox.Show("error ,latitude was not entered ");
                    return;
                }
                locationCustomer.Lattitude = location;
                flag = double.TryParse(textbox_newLong.Text, out location);
                if (!flag)
                {
                    MessageBox.Show("error ,longitude was not entered ");
                    return;
                }
                locationCustomer.Longitude = location;
                newCustomer.Location = locationCustomer;
                bl.AddCustomer(newCustomer);
                MessageBox.Show("Added successfully");
                textbox_newId.Text = "";
                textbox_newName.Text = "";
                textbox_newNumber.Text = "";
                textbox_newLong.Text = "";
                textbox_newLatt.Text = "";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
