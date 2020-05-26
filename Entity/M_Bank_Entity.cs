using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Bank_Entity : Base_Entity
    {
        public string BankCD { get;set;}
        public string BankCDFrom { get; set; }
        public string BankCDTo { get; set; }
        public string BankName { get; set; }
        public string BankKana { get; set; }
        public string Remarks { get; set; }
        public string ChangeDate { get; set; }
    }
}
