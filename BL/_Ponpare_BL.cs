using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using System.Data;
using Entity;
namespace BL
{
   public class _Ponpare_BL:Base_BL
    {
        _Ponpare_DL pdl = new _Ponpare_DL();
        public _Ponpare_BL()
        {

        }
        public bool Allow_Check()
        {
            return Convert.ToInt32(pdl.Allow_Check().Rows[0]["Status"].ToString()) == 1 ? true : false;
        }
        public DataTable Ponpare_MAPI_Select()
        {
            return pdl.Ponpare_MAPI_Select();
        }
        public bool InsertRirekiDetail(string StoreCD, String APIKey,string from, string to, string xml)
        {
            return pdl.InsertRirekiDetail(StoreCD,APIKey,from,to,xml);
        }
        public bool InsertEFGH(_PonpareEntity pet)
        {
            return pdl.InsertEFGH(pet);
        }
    }
}
