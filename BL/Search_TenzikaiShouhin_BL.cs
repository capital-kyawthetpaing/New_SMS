using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class Search_TenzikaiShouhin_BL:Base_BL
    {
        Search_TenzikaiShouhin_DL dl;
       
        public Search_TenzikaiShouhin_BL()
        {
            dl = new Search_TenzikaiShouhin_DL();
        }
        public DataTable M_Tenzikaishouhin_Search(M_TenzikaiShouhin_Entity mt)
        {
            return dl.M_Tenzikaishouhin_Search(mt);
        }
    }
}
