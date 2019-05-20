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

            //Доступный функционал в зависимости от роли пользователя
            switch (Transfer.LoggedUser.IdRole)
            {

                case 1:
                    
                    break;
                case 2:
                    BtnAddProduct.Visibility = Visibility.Hidden;
                    BtnCreateOrder.Visibility = Visibility.Hidden;
                    break;
            }

            DgOrderList.ItemsSource = Transfer.CurrentOrder.OrderItem.ToList();
            LabOrderName.Content = "Заказ: " + (Transfer.CurrentOrder.Id == 0 ? "Новый" : "№ " + Transfer.CurrentOrder.Id.ToString());
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new PgProductConstructor());
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            //при нажатии на кнопку назад происходит удаление, если заказ пуст

            if (Transfer.CurrentOrder.OrderItem.Count() == 0 && Transfer.CurrentOrder.Id != 0)
            {
                MessageBox.Show("Заказ будет удален, т.к. в нем остуствуют изделия.");
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
            //Дополнение данных для оформления заказа
            Transfer.CurrentOrder.Date = DateTime.Now;
            Transfer.CurrentOrder.IdState = 1;
            Transfer.CurrentOrder.Price = Transfer.CurrentOrder.OrderItem.Sum(c => c.Price);

            //внесение записей в БД
            if (Db.Conn.Order.Where(o => o.Id == Transfer.CurrentOrder.Id).FirstOrDefault() == null)
            {
                Db.Conn.Order.Add(Transfer.CurrentOrder);
                Db.Conn.SaveChanges();
            }
            List<Order> tempOrder = Db.Conn.Order.Where(u => u.IdUser == Transfer.LoggedUser.Id).ToList();
            var idOrder = tempOrder.Last().Id;
            
            foreach (var item in Transfer.CurrentOrder.OrderItem.ToList())
            {
                if (Db.Conn.OrderItem.Where(oi => oi.Id == item.Id).FirstOrDefault() == null)
                {
                    item.IdOrder = idOrder;
                    Db.Conn.OrderItem.Add(item);
                }
                
            }
            

            Db.Conn.SaveChanges();
            NavigationService.GetNavigationService(this).Navigate(new PgOrders());
        }

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            //удаление изделия из заказа
            if (Transfer.CurrentOrder.IdState == 1)
            {
                if (MessageBox.Show("Вы действительно хотите удалить изделие?", "Удаление изделия", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    OrderItem orderItem = ((FrameworkElement)sender).DataContext as OrderItem;

                    Transfer.CurrentOrder.OrderItem.Remove(orderItem);
                    Db.Conn.OrderItem.Remove(orderItem);
                    Db.Conn.SaveChanges();
                    DgOrderList.ItemsSource = Transfer.CurrentOrder.OrderItem.ToList();
                }
                
                
            }
        }

    }
}
