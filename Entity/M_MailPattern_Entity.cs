using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_MailPattern_Entity : Base_Entity
    {
        public string MailPatternCD { get; set; }
        public string MailPatternName { get; set; }
        public string MailType { get; set; }
        public string MailKBN { get; set; }
        public string MailPriority { get; set; }
        public string MailSubject { get; set; }
        public string MailText { get; set; }
    }
}
