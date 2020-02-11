using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
  public  class D_Purchase_Details_Entity : Base_Entity
    {
        public  string VendorCD { get; set; }
        public string JanCD { get; set; }
        public string SKUCD { get; set; }
        public string ItemCD { get; set; }
        public string ITemName { get; set; }
        public string MakerItemCD { get; set; }
        public string MakerName { get; set; }
        public string  ChkSumi { get; set; }
        public string ChkMi { get; set; }
        public string StaffCD { get; set; }
        public string Purchase_SDate { get; set; }
        public string Purchasee_EDate { get; set; }
        public string Plan_SDate { get; set; }
        public string Plan_EDate { get; set; }
        public string Order_SDate { get; set; }
        public string Order_EDate { get; set; }
        public string SKUName { get; set; }
        public string CheckValue { get; set; }
       
       
    }
}
