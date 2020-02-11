using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_StoreAuthorizations_Entity : Base_Entity
    {
        public string StoreAuthorizationsCD { get; set; }

        //KTP 2019-05-29 ほかの画面にも使うことがあるので　BaseEntity でやりました。
        //public string ChangeDate { get; set; }
        public string StoreCD { get; set; }
    }
}
