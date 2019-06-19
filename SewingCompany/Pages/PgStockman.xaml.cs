using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
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

using System.IO;

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
            FillAcceptedSupplyFilesList();
        }

        private void FillAcceptedSupplyFilesList()
        {
            string[] supplyFiles = Directory.GetFiles(@".\Storage\SupplyFiles\", "*.csv");
            LbAcceptedsupplyFiles.ItemsSource = supplyFiles;
        }

        public string SupplyFileName = string.Empty;

        private void BtUploadSupplyFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Выберите файл";
            op.Filter = "CSV (*.csv)|*.csv";
            if (op.ShowDialog() == true)
            {
                TbSupplyFilePath.Text = op.FileName;
                SupplyFileName = op.SafeFileName;
                ParseCsvSupplyFile(op.FileName);
                BtnAcceptSupplyFile.IsEnabled = true;
                
            }

        }

        private void BtnAcceptSupplyFile_Click(object sender, RoutedEventArgs e)
        {
            if (TbSupplyFilePath.Text != string.Empty)
            {
                foreach (FabricStock supplyInfo in DgSupplyFileContnet.Items)
                {
                    Db.Conn.FabricStock.Add(supplyInfo);
                    Db.Conn.SaveChanges();
                }
                string acceptDate = DateTime.Now.ToString("dd-MM-yy_");
                File.Copy(TbSupplyFilePath.Text, @"./Storage/SupplyFiles/" + acceptDate + SupplyFileName);
                FillAcceptedSupplyFilesList();
                DgSupplyFileContnet.Items.Clear();
                TbSupplyFilePath.Text = string.Empty;
                BtnAcceptSupplyFile.IsEnabled = false;
            }
        }

        private void LbAcceptedsupplyFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAcceptSupplyFile.IsEnabled = false;
            ParseCsvSupplyFile(LbAcceptedsupplyFiles.SelectedValue.ToString());

        }

        private void ParseCsvSupplyFile(string filePath)
        {

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                try
                {
                    DgSupplyFileContnet.Items.Clear();
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");
                    string[] fields = parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        fields = parser.ReadFields();
                        int IdUnitWidth_ = Convert.ToInt32(fields[2]);
                        int IdUnitHeight_ = Convert.ToInt32(fields[4]);
                        FabricStock supplyFileInfo = new FabricStock
                        {
                            IdFabric = fields[0],
                            Width = Convert.ToInt32(fields[1]),
                            IdUnitWidth = Convert.ToInt32(fields[2]),
                            Unit = Db.Conn.Unit.Where(u => u.Id == IdUnitWidth_).First(),
                            Height = Convert.ToInt32(fields[3]),
                            IdUnitHeight = Convert.ToInt32(fields[4]),
                            Unit1 = Db.Conn.Unit.Where(u => u.Id == IdUnitHeight_).First(),
                            PurchasePrice = Convert.ToDouble(fields[5])
                        };
                        DgSupplyFileContnet.Items.Add(supplyFileInfo);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения файла \n" + ex.ToString());
                }


            }
        }
    }
}
