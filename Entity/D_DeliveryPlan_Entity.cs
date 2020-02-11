using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_DeliveryPlan_Entity : Base_Entity
    {
        public string DeliveryPlanNO { get; set; }
        public string DeliveryKBN { get; set; }
        public string Number { get; set; }
        public string DeliveryName { get; set; }
        public string DeliverySoukoCD { get; set; }
        public string DeliveryZip1CD { get; set; }
        public string DeliveryZip2CD { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string DeliveryMailAddress { get; set; }
        public string DeliveryTelphoneNO { get; set; }
        public string DeliveryFaxNO { get; set; }
        public string DecidedDeliveryDate { get; set; }
        public string DecidedDeliveryTime { get; set; }
        public string CarrierCD { get; set; }
        public string PaymentMethodCD { get; set; }
        public string CommentInStore { get; set; }
        public string CommentOutStore { get; set; }
        public string InvoiceNO { get; set; }
        public string DeliveryPlanDate { get; set; }
        public string HikiateFLG { get; set; }
        public string IncludeFLG { get; set; }
        public string OntheDayFLG { get; set; }
        public string ExpressFLG { get; set; }

        //
    }
}
