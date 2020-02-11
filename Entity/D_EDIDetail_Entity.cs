using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_EDIDetail_Entity : Base_Entity
    {
        public string EDIImportNO { get; set; }
        public string EDIImportRows { get; set; }
        public string OrderNO { get; set; }
        public string OrderRows { get; set; }
        public string ArrivalPlanDate { get; set; }
        public string ArrivalPlanMonth { get; set; }
        public string ArrivalPlanCD { get; set; }
        public string ArrivalPlanSu { get; set; }
        public string VendorComment { get; set; }
        public string ErrorKBN { get; set; }
        public string ErrorText { get; set; }
        public string EDIOrderNO { get; set; }
        public string EDIOrderRows { get; set; }
        public string CSVRecordKBN { get; set; }
        public string CSVDataKBN { get; set; }
        public string CSVCapitalCD { get; set; }
        public string CSVCapitalName { get; set; }
        public string CSVOrderCD { get; set; }
        public string CSVOrderName { get; set; }
        public string CSVSalesCD { get; set; }
        public string CSVSalesName { get; set; }
        public string CSVDestinationCD { get; set; }
        public string CSVDestinationName { get; set; }
        public string CSVOrderNO { get; set; }
        public string CSVOrderRows { get; set; }
        public string CSVOrderLines { get; set; }
        public string CSVOrderDate { get; set; }
        public string CSVArriveDate { get; set; }
        public string CSVOrderKBN { get; set; }
        public string CSVMakerItemKBN { get; set; }
        public string CSVMakerItem { get; set; }
        public string CSVSKUCD { get; set; }
        public string CSVSizeName { get; set; }
        public string CSVColorName { get; set; }
        public string CSVTaniCD { get; set; }
        public string CSVOrderUnitPrice { get; set; }
        public string CSVOrderPriceWithoutTax { get; set; }
        public string CSVBrandName { get; set; }
        public string CSVSKUName { get; set; }
        public string CSVJanCD { get; set; }
        public string CSVOrderSu { get; set; }
        public string CSVOrderGroupNO { get; set; }
        public string CSVAnswerSu { get; set; }
        public string CSVNextDate { get; set; }
        public string CSVOrderGroupRows { get; set; }
        public string CSVErrorMessage { get; set; }

        //EDI回答納期登録のため
        public int ChkAnswer { get; set; }
        public int ChkNoAnswer { get; set; }

    }
}
