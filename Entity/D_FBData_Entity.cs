using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_FBData_Entity:Base_Entity
    {
        public string PayeeCD { get; set; }
        public string PayeeName { get; set; }
        public string BankCD { get; set; }
        public string BranchCD { get; set; }
	    public string KouzaKBN { get; set; }
        public string KouzaNO { get; set; }
        public string KouzaMeigi { get; set; }
        public string PayGaku { get; set; }
        public string TransferGaku { get; set; }
        public string TransferFee { get; set; }
        public string TransferFeeKBN { get; set; }
        public string StaffCD { get; set; }
    }
}
