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
    public partial class PgOrderList : Page
    {
        public PgOrderList()
        {
            InitializeComponent();
            DgOrderList.ItemsSource = Transfer.OrderItems;
            LabOrderName.Content = "Заказ: " + (Transfer.CurrentOrder.Id == 0 ? "Новый" : "№ " + Transfer.CurrentOrder.Id.ToString());
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new PgProductConstructor());
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (Transfer.CurrentOrder.OrderList.Count() == 0)
            {
                MessageBox.Show("oj, there're actually zero items. let's delete this");
                var orderToDelete = Db.Conn.Order.Where(u => u.Id == Transfer.CurrentOrder.Id).First();
                if(orderToDelete != null)
                {
                    Db.Conn.Order.Remove(orderToDelete);
                    Db.Conn.SaveChanges();
                }
                Transfer.CurrentOrder = null;
            }
            NavigationService.GetNavigationService(this).Navigate(new PgOrders());
        }

        private void BtnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            Transfer.CurrentOrder.Date = DateTime.Now;
            Db.Conn.Order.Add(Transfer.CurrentOrder);
            Db.Conn.SaveChanges();
            List<Order> tempOrder = Db.Conn.Order.Where(u => u.IdUser == Transfer.LoggedUser.Id).ToList();
            var idOrder = tempOrder.Last().Id;
            foreach (var item in Transfer.OrderItems)
            {
                item.IdOrder = idOrder;
                Db.Conn.OrderList.Add(item);
            }

            Db.Conn.SaveChanges();
            NavigationService.GetNavigationService(this).Navigate(new PgOrders());
        }

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (Transfer.CurrentOrder.IdState == 1)
            {
                if (MessageBox.Show("Вы действительно хотите удалить изделие?", "Удаление изделия", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    OrderList orderItem = ((FrameworkElement)sender).DataContext as OrderList;

                    Transfer.CurrentOrder.OrderList.Remove(orderItem);
                    Transfer.OrderItems.Remove(orderItem);
                    Db.Conn.OrderList.Remove(orderItem);
                    Db.Conn.SaveChanges();
                    DgOrderList.ItemsSource = Transfer.OrderItems;
                }
                
                
            }
        }
    }
}
