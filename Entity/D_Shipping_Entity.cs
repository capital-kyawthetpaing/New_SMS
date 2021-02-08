using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Shipping_Entity:Base_Entity
    {
        public string ShippingDateFrom { get; set; }
        public string ShippingDateTo { get; set; }
        public string SoukoCD { get; set; }
        public string CarrierCD { get; set; }
        public string SaleFlg { get; set; }
        public string TransferFlg { get; set; }
        public string SaleAlreadyFlg { get; set; }
        public string SaleNotFlg { get; set; }
        public string ShippingKBN { get; set; }
        public string InvoiceNO { get; set; }

        public string ShippingNO { get; set; }
        public string InstructionNO { get; set; }
        public string ShippingDate { get; set; }
        public string InputDateTime { get; set; }
        public string StaffCD { get; set; }
        public string UnitsCount { get; set; }
        public string DecidedDeliveryDate { get; set; }
        public string DecidedDeliveryTime { get; set; }
        public string BoxSize { get; set; }
        public string PrintDate { get; set; }
        public string PrintStaffCD { get; set; }
        public string LinkageDateTime { get; set; }
        public string LinkageStaffCD { get; set; }
        public string InvNOLinkDateTime { get; set; }
        public string ReceiveStaffCD { get; set; }
        public string SalesDateTime { get; set; }

        //出荷検索用
        public string SKUCD { get; set; }
        public string JanCD { get; set; }
    }
}
