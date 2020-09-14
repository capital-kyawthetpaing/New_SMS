using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Fnc_Credit_Entity : Base_Entity
    {
        public string CustomerCD { get;set;}
        public string CreditCheckKBN { get; set; }
        public string CreditMessage { get; set; }
        public string SaikenGaku { get; set; }
        public string CreditAmount { get; set; }
    
    }
}
