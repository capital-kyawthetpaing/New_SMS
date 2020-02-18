using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_InventoryProcessing_Entity : Base_Entity
    {
        public string InventoryNO { get; set; }
        public string SoukoCD { get; set; }
        public string FromRackNO { get; set; }
        public string ToRackNO { get; set; }
        public string InventoryDate { get; set; }
        public string InventoryKBN { get; set; }
        public string ProcessingDateTime { get; set; }
        public string StaffCD { get; set; }

        /// <summary>
        /// 棚卸締処理用Entity
        /// </summary>
        public string Syori { get; set; }
        public string StoreCD { get; set; }
    }
}
