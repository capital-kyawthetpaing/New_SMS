using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Mail_Entity : Base_Entity
    {
        public string MailCounter { get; set; }
        public string MailType { get; set; }
        public string MailKBN { get; set; }
        public string Number { get; set; }
        public string MailNORows { get; set; }
        public string MailDateTime { get; set; }
        public string StaffCD { get; set; }
        public string ContactKBN { get; set; }
        public string MailPatternCD { get; set; }
        public string MailSubject { get; set; }
        public string MailPriority { get; set; }
        public string ReMailFlg { get; set; }
        public string UnitKBN { get; set; }
        public string SendedDateTime { get; set; }
        public string SenderKBN { get; set; }
        public string SenderCD { get; set; }
        public string SenderAddress { get; set; }
        public string MailContent { get; set; }

        //照会用Entity
        public string MailDateFrom { get; set; }
        public string MailDateTo { get; set; }
        public string MailTimeFrom { get; set; }
        public string MailTimeTo { get; set; }
        public string CustomerCD { get; set; }
        public string VendorCD { get; set; }
    }
}
