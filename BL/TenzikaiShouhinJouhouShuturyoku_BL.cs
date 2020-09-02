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
   public class TenzikaiShouhinJouhouShuturyoku_BL : Base_BL
    {
        TenzikaiShouhinJouhouShuturyoku_DL TenzikaiDl;
        public TenzikaiShouhinJouhouShuturyoku_BL()
        {
            TenzikaiDl = new TenzikaiShouhinJouhouShuturyoku_DL();
        }
        public DataTable Rpc_TenzikaiShouhinJouhouShuturyoku(M_TenzikaiShouhin_Entity mte)
        {
            return TenzikaiDl.Rpc_TenzikaiShouhinJouhouShuturyoku(mte);
        }
    }
}
