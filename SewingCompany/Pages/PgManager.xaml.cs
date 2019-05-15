﻿using System;
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

namespace SewingCompany.Pages
{
    /// <summary>
    /// Interaction logic for PgManager.xaml
    /// </summary>
    public partial class PgManager : Page
    {
        public PgManager()
        {
            InitializeComponent();
            FrProductList.Navigate(new PgProductList());
            DgOrders.ItemsSource = Db.Conn.Order.ToList();
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            Order obj = ((FrameworkElement)sender).DataContext as Order;
            MessageBox.Show(obj.OrderState.Name);
        }
    }
}
