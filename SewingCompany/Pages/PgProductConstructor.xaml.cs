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
    /// Interaction logic for PgProductConstructor.xaml
    /// </summary>
    public partial class PgProductConstructor : Page
    {
        public PgProductConstructor()
        {
            InitializeComponent();
            CbProduct.ItemsSource = Db.Conn.Product.ToList();
            CbWidthUnit.ItemsSource = Db.Conn.Unit.ToList();
            CbHeightUnit.ItemsSource = Db.Conn.Unit.ToList();

            CbFabric.ItemsSource = Db.Conn.Fabric.ToList();
            CbFurniture.ItemsSource = Db.Conn.Furniture.ToList();
            
        }

        private void CbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            try
            {
                ImProductView.Source = new BitmapImage(new Uri(@"E:\НЧ 2017\Resources\Сессия 1\images\Изделия\" + CbProduct.SelectedValue + ".jpg", UriKind.Absolute));
            }
            catch
            {
                ImProductView.Source = new BitmapImage(new Uri(@".\..\Resources\Images\System\no-image.jpg", UriKind.RelativeOrAbsolute));
            }
        }

        private void CbFabric_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ImFabricView.Source = new BitmapImage(new Uri(@"E:\НЧ 2017\Resources\Сессия 1\images\Ткани\" + CbFabric.SelectedValue + ".jpg", UriKind.Absolute));
            }
            catch
            {
                ImFabricView.Source = new BitmapImage(new Uri(@".\..\Resources\Images\System\no-image.jpg", UriKind.RelativeOrAbsolute));
            }
        }

        private void CbFurniture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ImFurnitureView.Source = new BitmapImage(new Uri(@"E:\НЧ 2017\Resources\Сессия 1\images\Фурнитура\" + CbFurniture.SelectedValue + ".jpg", UriKind.Absolute));
            }
            catch
            {
                ImFurnitureView.Source = new BitmapImage(new Uri(@".\..\Resources\Images\System\no-image.jpg", UriKind.RelativeOrAbsolute));
            }
        }
    }
}
