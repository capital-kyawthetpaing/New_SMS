using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_TankaCD_Entity : Base_Entity
    {
        public string TankaCD {get;set;}
        public string TankaName { get; set; }
        public string GeneralRate { get; set; }
        public string MemberRate { get; set; }
        public string ClientRate { get; set; }
        public string SaleRate { get; set; }
        public string WebRate { get; set; }
        public string Remarks { get; set; }
        public string RoundKBN { get; set; }

        //検索用Entity
        public string DisplayKbn { get; set; }
        public string TankaCDFrom { get; set; }
        public string TankaCDTo { get; set; }
    }
}
