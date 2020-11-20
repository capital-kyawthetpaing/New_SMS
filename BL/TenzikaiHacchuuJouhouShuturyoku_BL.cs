using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
    public class TenzikaiHacchuuJouhouShuturyoku_BL:Base_BL
    {
        D_TenzikaiJuchuu_DL dtenzidl = new D_TenzikaiJuchuu_DL();
        MasterTouroku_TenzikaiShouhin_DL mtsdl = new MasterTouroku_TenzikaiShouhin_DL();

        public DataTable D_TenzikaiJuchuu_SelectForExcel(D_TenzikaiJuchuu_Entity dtje,int chk)
        {
            return dtenzidl.D_TenzikaiJuchuu_SelectForExcel(dtje,chk);
        }

        public DataTable M_TenzikaiShouhin_SelectForHachuu(M_TenzikaiShouhin_Entity mte)
        {
            return mtsdl.M_TenzikaiShouhin_SelectForHachuu(mte);
        }


    }
}
