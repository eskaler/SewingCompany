using SewingCompany.Utilities;
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

namespace SewingCompany.Pages
{
    /// <summary>
    /// Interaction logic for PgOrderList.xaml
    /// </summary>
    public partial class PgOrders : Page
    {
        public PgOrders()
        {
            InitializeComponent();
            DgOrders.ItemsSource = Db.Conn.Order.Where(u => u.IdUser == Transfer.LoggedUser.Id).ToList();
        }

        private void BtnAddNewOrder_Click(object sender, RoutedEventArgs e)
        {
            Transfer.CurrentOrder = new Order
            {
                IdUser = Transfer.LoggedUser.Id
            };
            NavigationService.GetNavigationService(this).Navigate(new PgOrderList());
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            Order orderToEdit = ((FrameworkElement)sender).DataContext as Order;
            Transfer.OrderItems = orderToEdit.OrderList.ToList();
            Transfer.CurrentOrder = orderToEdit;
            NavigationService.GetNavigationService(this).Navigate(new PgOrderList());
        }
    }
}
