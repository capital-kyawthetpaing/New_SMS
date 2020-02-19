using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Inventory_Entity : Base_Entity
    {
        public string SoukoCD { get; set; }
        public string RackNO { get; set; }
        public string InventoryDate { get; set; }
        public string SKUCD { get; set; }
        public string AdminNO { get; set; }
        public string JanCD { get; set; }
        public string TheoreticalQuantity { get; set; }
        public string ActualQuantity { get; set; }
        public string DifferenceQuantity { get; set; }
        public string InventoryNO { get; set; }
        /// <summary>
        /// 棚卸差異表用Entity
        /// </summary>
        public int ChkSaiOnly { get; set; }
        public int KbnSai { get; set; }

        /// <summary>
        /// 棚卸表用Entity
        /// </summary>
        public int ChkKinyu { get; set; }
    }
}
