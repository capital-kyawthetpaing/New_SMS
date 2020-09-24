using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_CustomerSKUPrice_Entity:Base_Entity
    {
        public string CustomerCD { get; set; }
        public string CustomerName { get; set; }
        public string TekiyouKaisiDate { get; set; }
        public string TekiyouShuuryouDate { get; set; }
        public string AdminNO { get; set; }
        public string JanCD  { get; set; }
        public string SKUCD { get; set; }
        public string SKUName { get; set; }
        public string SalePriceOutTax { get; set; }
        public string Remarks { get; set; }

        //For MasterTouroku_CustomerSKUPrice
        public string TekiyouKaisiDate_From { get; set; }
        public string TekiyouKaisiDate_To { get; set; }
        public string SKUCD_From { get; set; }
        public string SKUCD_To { get; set; }
        public string CustomerCD_From { get; set; }
        public string CustomerCD_To { get; set; }
        public string DisplayKBN { get; set; }

    }
}
