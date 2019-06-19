using Microsoft.Win32;
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
            CbBorder.ItemsSource = Db.Conn.Fabric.ToList();
            
        }

        //путь к изображениям
        //public string ImagesPath = System.Reflection.Assembly.GetEntryAssembly() + @"\images\";
        public string ImagesPath = @"pack://siteoforigin:,,,/Images/";

        private void CbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //MessageBox.Show(ImagesPath + @"Изделия/");
                ImProductView.Source = new BitmapImage(new Uri(ImagesPath + @"Изделия/" + CbProduct.SelectedValue + ".jpg", UriKind.RelativeOrAbsolute));
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
                ImFabricView.Source = new BitmapImage(new Uri(ImagesPath + @"Ткани\" + CbFabric.SelectedValue + ".jpg", UriKind.RelativeOrAbsolute));
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
                ImFurnitureView.Source = new BitmapImage(new Uri(ImagesPath + @"Фурнитура\" + CbFurniture.SelectedValue + ".jpg", UriKind.RelativeOrAbsolute));
            }
            catch
            {
                ImFurnitureView.Source = new BitmapImage(new Uri(@".\..\Resources\Images\System\no-image.jpg", UriKind.RelativeOrAbsolute));
            }
        }

        private void CbBorder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ImBorderView.Source = new BitmapImage(new Uri(ImagesPath + @"Ткани\" + CbBorder.SelectedValue + ".jpg", UriKind.RelativeOrAbsolute));
            }
            catch
            {
                ImBorderView.Source = new BitmapImage(new Uri(@".\..\Resources\Images\System\no-image.jpg", UriKind.RelativeOrAbsolute));
            }
        }


        //переменная для хранения угла поворота
        RotateTransform FurnitureRotation = new RotateTransform(0);

        private void BtnRotateLeft_Click(object sender, RoutedEventArgs e)
        {
            FurnitureRotation.Angle -= 10;
            if (FurnitureRotation.Angle == -360)
                FurnitureRotation.Angle = 0;
            ImFurnitureView.RenderTransform = FurnitureRotation;
            LabRotationDegree.Content = FurnitureRotation.Angle.ToString() + "°";
        }

        private void BtnRotateRight_Click(object sender, RoutedEventArgs e)
        {
            FurnitureRotation.Angle += 10;
            if (FurnitureRotation.Angle == 360)
                FurnitureRotation.Angle = 0;
            ImFurnitureView.RenderTransform = FurnitureRotation;
            LabRotationDegree.Content = FurnitureRotation.Angle.ToString() + "°";
        }

        //добавление собственной ткани
        private void BtnAddCustomFabric_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Выберите изображение";
            op.Filter = "Все поддерживаемые форматы|*.jpg;*.jpeg;*.png" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                if (sender.Equals(BtnAddCustomFabric))
                {
                    ImFabricView.Source = new BitmapImage(new Uri(op.FileName));
                    TbCustomFabricPath.Text = op.FileName;
                }
                else
                {
                    ImFurnitureView.Source = new BitmapImage(new Uri(op.FileName));
                    TbCustomFurniturePath.Text = op.FileName;
                }
                
            }
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {

            bool dataIsValid = true;
            string errorMessage = string.Empty;
            try { Convert.ToDouble(TbWidth.Text); }
            catch { errorMessage += "Неравильно заполнено поле \"Ширина\".\n"; dataIsValid = false; }
            try { Convert.ToDouble(TbHeight.Text); }
            catch { errorMessage += "Неравильно заполнено поле \"Высота\".\n"; dataIsValid = false; }
            try { Convert.ToInt32(TbProductAmount.Text); }
            catch { errorMessage += "Неравильно заполнено поле \"Количество\".\n"; dataIsValid = false; }
            if (dataIsValid)
            {

                double productPrice = (
                        Convert.ToDouble(Db.Conn.FabricStock.Where(u => u.IdFabric == (string)CbFabric.SelectedValue).FirstOrDefault().PurchasePrice) +
                        Convert.ToDouble(Db.Conn.FurnitureStock.Where(u => u.IdFurniture == (string)CbFurniture.SelectedValue).FirstOrDefault().PurchasePrice) +
                        Convert.ToDouble(Db.Conn.FabricStock.Where(u => u.IdFabric == (string)CbBorder.SelectedValue).FirstOrDefault().PurchasePrice)
                    ) * Convert.ToDouble(TbProductAmount.Text);


                //сохранение изделия в заказ
                Transfer.CurrentOrder.OrderItem.Add(new OrderItem
                {
                    IdProduct = (string)CbProduct.SelectedValue,
                    IdFabric = (string)CbFabric.SelectedValue,
                    IdFurniture = (string)CbFurniture.SelectedValue,
                    IdBorder = (string)CbBorder.SelectedValue,
                    RotationAngle = Convert.ToInt32(FurnitureRotation.Angle),
                    Width = Convert.ToDouble(TbWidth.Text),
                    Height = Convert.ToDouble(TbHeight.Text),
                    IdUnitHeight = (int)CbHeightUnit.SelectedValue,
                    IdUnitWidth = (int)CbWidthUnit.SelectedValue,
                    Amount = Convert.ToInt32(TbProductAmount.Text),
                    Price = productPrice
                });
                NavigationService.GetNavigationService(this).Navigate(new PgOrderList());
            }
            else
                MessageBox.Show(errorMessage, "Проверьте правильность заполнения данных");
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).GoBack();
        }
    }
}
