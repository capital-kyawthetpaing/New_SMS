using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using DL;
namespace BL
{
   public class TenjikaiJuuChuu_BL : Base_BL
    {
        TenjikaiJuuChuu_DL tdl = new TenjikaiJuuChuu_DL();
        public TenjikaiJuuChuu_BL()
        {

        }
        public bool D_TenjiInsert( Tenjikai_Entity tje, string xml )
        {
            return tdl.D_TenjiInsert(tje,xml);
        }
        public DataTable JuuChuuCheck(string JuuChuuBi)
        {
           return tdl.JuuChuuCheck(JuuChuuBi);
           
        }
        public DataTable ShuukaSouko(string SouKoCD, string ChangeDate)
        {
            return tdl.ShuukaSouko(SouKoCD, ChangeDate);
        }
        public DataTable M_TenjiKaiJuuChuu_Select(Tenjikai_Entity tje)
        {
            return tdl.M_TenjiKaiJuuChuu_Select(tje);
        }
        public DataTable M_TeniKaiSelectbyJAN(Tenjikai_Entity tje)
        {
            return tdl.M_TeniKaiSelectbyJAN(tje);
        }
        public DataTable GetTaxRate(string taxflg,string changeDtae)
        {
            return tdl.GetTaxRate(taxflg, changeDtae);
        }
        public bool Check_DTenjikaiJuuchuu(string tkb)
        {
            return tdl.Check_DTenjikaiJuuchuu(tkb).Rows.Count > 0 ?  true : false;
        }
    }
}
