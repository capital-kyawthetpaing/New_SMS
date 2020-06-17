using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_OrderDetails_Entity : Base_Entity
    {
        public string OrderNO { get; set; }
        public string OrderRows { get; set; }

        public string DisplayRows { get; set; }
        public string JuchuuNO { get; set; }
        public string JuchuuRows { get; set; }
        public string JuchuuOrderNO { get; set; }
        public string SKUCD { get; set; }
        public string AdminNO { get; set; }
        public string JanCD { get; set; }
        public string ItemName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string Remarks { get; set; }
        public string OrderSu { get; set; }
        public string TaniCD { get; set; }
        public string PriceOutTax { get; set; }
        public string Rate { get; set; }
        public string OrderUnitPrice { get; set; }
        public string OrderHontaiGaku { get; set; }
        public string OrderTax { get; set; }
        public string OrderTaxRitsu { get; set; }
        public string OrderGaku { get; set; }
        public string SoukoCD { get; set; }
        public string DirectFLG { get; set; }
        public string NotNetFLG { get; set; }
        public string EDIFLG { get; set; }
        public string DesiredDeliveryDate { get; set; }
        public string ArrivePlanDate { get; set; }
        public string TotalArrivalSu { get; set; }
        public string CommentOutStore { get; set; }
        public string CommentInStore { get; set; }
        public string FirstOrderNO { get; set; }
        public string FirstOrderRows { get; set; }
        public string CancelOrderNO { get; set; }
        public string AnswerFLG { get; set; }
        public string EDIOutputDatetime { get; set; }
        public string LastArrivePlanNO { get; set; }
        public string LastArriveDatetime { get; set; }
        public string LastArriveNO { get; set; }
        public string InsertOperator { get; set; }
        public string InsertDateTime { get; set; }
        public string UpdateOperator { get; set; }
        public string UpdateDateTime { get; set; }
        public string DeleteOperator { get; set; }
        public string DeleteDateTime { get; set; }
    }
}
