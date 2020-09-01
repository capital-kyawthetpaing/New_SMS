using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Instruction_Entity : Base_Entity
    {
        public string InstructionNO { get; set; }
        public string DeliveryPlanNO { get; set; }
        public string InstructionKBN { get; set; }
        public string InstructionDate { get; set; }
        public string DeliveryPlanDate { get; set; }
        public string FromSoukoCD { get; set; }
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
        public string CashOnDelivery { get; set; }
        public string PaymentMethodCD { get; set; }
        public string CommentOutStore { get; set; }
        public string CommentInStore { get; set; }
        public string InvoiceNO { get; set; }
        public string OntheDayFLG { get; set; }
        public string ExpressFLG { get; set; }
        public string PrintDate { get; set; }
        public string PrintStaffCD { get; set; }

        //出荷指示入力用Entity
        public string StoreCD { get; set; }
        public int Chk1 { get; set; }
        public int Chk2 { get; set; }
        public int Chk3 { get; set; }
        public int Chk4 { get; set; }
        public int Chk5 { get; set; }
        public int ChkHakkozumi { get; set; }
        public int ChkSyukkazumi { get; set; }
        public int ChkSyukkaFuka { get; set; }
        //検索用Entity
        public string InstructionDateFrom { get; set; }
        public string InstructionDateTo { get; set; }
        public string DeliveryPlanDateFrom { get; set; }
        public string DeliveryPlanDateTo { get; set; }
        public string JuchuuNO { get; set; }
        //出荷指示書用Entity
        public int ChkMihakko { get; set; }
        public int ChkSaihakko { get; set; }
        public int　ChkNohinSeikyu{ get; set; }
}
}
