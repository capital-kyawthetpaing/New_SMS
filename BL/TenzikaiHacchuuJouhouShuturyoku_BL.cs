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

        public DataTable D_TenzikaiJuchuu_SelectForExcel(D_TenzikaiJuchuu_Entity dtje,int chk)
        {
            return dtenzidl.D_TenzikaiJuchuu_SelectForExcel(dtje,chk);
        }
    }
}
