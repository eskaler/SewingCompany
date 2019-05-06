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
    /// Interaction logic for PgStockman.xaml
    /// </summary>
    public partial class PgStockman : Page
    {
        public PgStockman()
        {
            InitializeComponent();
            DgFabric.ItemsSource = Db.Conn.FabricStock.ToList();
            DgFurniture.ItemsSource = Db.Conn.FurnitureStock.ToList();
        }
    }
}
