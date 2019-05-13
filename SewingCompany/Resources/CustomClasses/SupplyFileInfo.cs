using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingCompany.Resources.CustomClasses
{
    class SupplyFileInfo
    {
        public string Id { get; set; }
        public int Width { get; set; }
        public int IdUnitWidth { get; set; }
        public int Height { get; set; }
        public int IdUnitHeight { get; set; }
        public int PurchasePrice { get; set; }
    }
}
