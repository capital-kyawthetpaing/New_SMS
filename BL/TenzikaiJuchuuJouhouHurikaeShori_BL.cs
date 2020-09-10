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
    public class TenzikaiJuchuuJouhouHurikaeShori_BL : Base_BL
    {
        D_TenzikaiJuchuu_DL ddl;
        public TenzikaiJuchuuJouhouHurikaeShori_BL()
        {
            ddl = new D_TenzikaiJuchuu_DL();
        }

        public DataTable D_TenzikaiJuchuu_SelectAll(M_TenzikaiShouhin_Entity me)
        {
            return ddl.D_TenzikaiJuchuu_SelectAll(me);
        }
        /// <summary>
        /// 展示会受注情報振替処理
        /// TenzikaiJuchuuJouhouHurikaeShoriより更新時に使用
        /// </summary>
        public bool D_TenzikaiJuchuu_Exec(M_TenzikaiShouhin_Entity me)
        {
            return ddl.D_TenzikaiJuchuu_Exec(me);
        }
    }
}
