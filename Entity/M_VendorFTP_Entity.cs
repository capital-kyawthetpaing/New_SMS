using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_VendorFTP_Entity : Base_Entity
    {
        public string VendorCD { get; set; }
        public string FTPServer { get; set; }
        public string FTPFolder { get; set; }
        public string CreateServer { get; set; }
        public string CreateFolder { get; set; }
        public string FileName { get; set; }
        public string TitleFLG { get; set; }
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string MailTitle { get; set; }
        public string MailPatternCD { get; set; }
        public string SenderMailAddress { get; set; }
        public string MailPriority { get; set; }
        public string CapitalCD { get; set; }
        public string CapitalName { get; set; }
        public string OrderCD { get; set; }
        public string OrderName { get; set; }
        public string SalesCD { get; set; }
        public string SalesName { get; set; }
        public string DestinationCD { get; set; }
        public string DestinationName { get; set; }
        
    }
}
