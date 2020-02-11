using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_EDI_Entity : Base_Entity
    {
        public string EDIImportNO { get; set; }
        public string ImportDateTime { get; set; }
        public string StaffCD { get; set; }
        public string VendorCD { get; set; }
        public string ImportFile { get; set; }
        public int OrderDetailsSu { get; set; }
        public int ImportDetailsSu { get; set; }
        public int ErrorSu { get; set; }

    }
}
