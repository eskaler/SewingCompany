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
    /// Interaction logic for PgCutting.xaml
    /// </summary>
    public partial class PgCutting : Page
    {
        public PgCutting()
        {
            InitializeComponent();
            DgOrderList.ItemsSource = Transfer.CurrentOrder.OrderItem.OrderBy(oi => oi.Width).ThenBy(oi => oi.Height).ToList();
            LabOrderName.Content = "Заказ: " + (Transfer.CurrentOrder.Id == 0 ? "Новый" : "№ " + Transfer.CurrentOrder.Id.ToString());
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new PgOrders());
        }

        private void DgOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            OrderItem itemToCut = DgOrderList.SelectedItem as OrderItem;
            //OrderItem itemToCut = ((FrameworkElement)sender).DataContext as OrderItem;
            MessageBox.Show(itemToCut.ToString());
        }
    }
}
