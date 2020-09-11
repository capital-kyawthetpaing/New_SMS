using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using System.Data;
namespace BL
{
   public class ShuukaSouko_BL
    {
        M_Souko_DL SSD;
        public ShuukaSouko_BL()
        {
            SSD = new M_Souko_DL();
        }
        
        public DataTable M_Souko_BindForShukka(string ChangeDate,string DeleteFg)
        {
            return SSD.M_Souko_BindForTenJiShukka(ChangeDate, DeleteFg);
        }
    }
}
