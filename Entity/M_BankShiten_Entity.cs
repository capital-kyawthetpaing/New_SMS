using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_BankShiten_Entity : Base_Entity
    {
        public string BankCD {get;set;}
        public string BranchCD { get; set; }
        public string BranchName { get; set; }
        public string BranchKana { get; set; }
        public string Remarks { get; set; }

        public string BranchCD_From { get; set; }

        public string BranchCD_To { get; set; }
    }
}
