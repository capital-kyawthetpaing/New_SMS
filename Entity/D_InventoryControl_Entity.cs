using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_InventoryControl_Entity : Base_Entity
    {
        public string SoukoCD { get; set; }
        public string RackNO { get; set; }
        public string InventoryDate { get; set; }
        public string InventoryKBN { get; set; }
        public string InventoryNO { get; set; }
        public string AdditionDateTime { get; set; }
        public string AdditionStaffCD { get; set; }
    }
}
