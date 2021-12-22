﻿using BlApi;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL bl = BlApi.BlFactory.GetBl();
        public MainWindow()
        {
            InitializeComponent();
            manager.Visibility = Visibility.Hidden;
        }
  
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            start.Visibility = Visibility.Hidden;
            manager.Visibility = Visibility.Visible;
        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ManagerPage(this);
        }
    }
}
