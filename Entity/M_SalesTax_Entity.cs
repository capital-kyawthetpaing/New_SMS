using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_SalesTax_Entity : Base_Entity
    {
        public string TaxRate1 {get;set;}
        public string TaxRate2 {get;set;}
        public string FractionKBN { get; set; }

    }
}
