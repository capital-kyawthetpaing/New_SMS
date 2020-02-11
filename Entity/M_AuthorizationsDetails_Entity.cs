using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_AuthorizationsDetails_Entity : Base_Entity
    {
        public string AuthorizationsCD { get; set; }

        //KTP 2019-05-29 ほかの画面にも使うことがあるので　BaseEntity でやりました。
        //public string ChangeDate { get; set; }
        public string Insertable { get; set; }
        public string Updatable { get; set; }
        public string Deletable { get; set; }
        public string Inquirable { get; set; }
        public string Printable { get; set; }
        public string Outputable { get; set; }
        public string Runable { get; set; }

        //for AccessCheck
        public string StoreAuthorizationsCD { get; set; }
        public string StoreAuthorization_ChangeDate { get; set; }
        public string ProgramName { get; set; }

        public string ProgramType { get; set; }
        
    }
}
