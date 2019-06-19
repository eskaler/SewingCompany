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

        public OrderItem ItemToCut;

        private void DgOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ItemToCut = DgOrderList.SelectedItem as OrderItem;

            var unitToConvert = Math.Max(ItemToCut.IdUnitHeight, ItemToCut.IdUnitWidth);
            
            ItemToCut.Width = ItemToCut.Width * Db.Conn.UnitConvert.Where(uc => uc.IdUnit1 == ItemToCut.IdUnitWidth && uc.IdUnit2 == unitToConvert).First().Ratio;
            ItemToCut.IdUnitWidth = unitToConvert;
            ItemToCut.Height = ItemToCut.Height * Db.Conn.UnitConvert.Where(uc => uc.IdUnit1 == ItemToCut.IdUnitHeight && uc.IdUnit2 == unitToConvert).First().Ratio;
            ItemToCut.Height = unitToConvert;

            LabProductId.Content = ItemToCut.Id.ToString();
            LabFabricId.Content = ItemToCut.IdFabric.ToString();
            LabProductArea.Content = string.Format("{0}{1}^2",
                (ItemToCut.Width * ItemToCut.Height).ToString(), ItemToCut.Unit.Name);
            LabProductAmount.Content = ItemToCut.Amount.ToString();
            LabFabricNeeded.Content = string.Format("{0}{1}^2", 
                (ItemToCut.Amount * ItemToCut.Width * ItemToCut.Height).ToString(), ItemToCut.Unit.Name);

            CbFabricStock.ItemsSource = Db.Conn.FabricStock.Where(fs => fs.IdFabric == ItemToCut.IdFabric).ToList();
            CbFabricStock.SelectedIndex = 0;

            
            
        }

        FabricStock fabricStock;
        public bool FabricEnough = false;
        private void CbFabricStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                fabricStock = CbFabricStock.SelectedItem as FabricStock;
                fabricStock.Height = fabricStock.Height * Db.Conn.UnitConvert.Where(uc => uc.IdUnit1 == fabricStock.IdUnitHeight && uc.IdUnit2 == ItemToCut.IdUnitHeight).First().Ratio;
                fabricStock.IdUnitHeight = ItemToCut.IdUnitHeight;
                fabricStock.Width = fabricStock.Width * Db.Conn.UnitConvert.Where(uc => uc.IdUnit1 == fabricStock.IdUnitWidth && uc.IdUnit2 == ItemToCut.IdUnitWidth).First().Ratio;
                fabricStock.IdUnitWidth = ItemToCut.IdUnitWidth;

                double fabricArea = Convert.ToDouble(fabricStock.Width * fabricStock.Height);
                LabFabricStock.Content = string.Format("{0}{1}^2", fabricArea.ToString(), ItemToCut.Unit.Name);
                LabFabricEnoughFor.Content = Convert.ToInt32(Math.Floor((fabricArea / (ItemToCut.Width * ItemToCut.Height)))).ToString();
                FabricEnough = Convert.ToInt32(Math.Floor((fabricArea / (ItemToCut.Width * ItemToCut.Height)))) > 1;
            }
            catch
            {

            }
        }

        private void BtnCut_Click(object sender, RoutedEventArgs e)
        {
            //First Fit Decreasing Height
            var tubeW = (double)fabricStock.Width;
            var tubeH = (double)fabricStock.Height;

            List<double> h = new List<double>();
            List<double> w = new List<double>();
            h.Add(ItemToCut.Height);
            w.Add(0);
            var level = 1;

            for (var i = 0; i < ItemToCut.Amount; i++)
            {
                for(var j = 0; j < level; j++)
                {
                    if(w[j] + ItemToCut.Width <= tubeW && ItemToCut.Height <= h[j])
                    {
                        w[j] += ItemToCut.Width;
                    }
                    else
                    {
                        h.Add(ItemToCut.Height);
                    }
                }
            }
        }
    }
}
