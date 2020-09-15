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

        public bool CheckTenzikaiJuchuu(M_TenzikaiShouhin_Entity me, out string errno)
        {
            DataTable dt = ddl.CheckTenzikaiJuchuu(me);

            bool ret = false;
            errno = "";

            if (dt.Rows.Count > 0)
            {
                errno = dt.Rows[0]["errno"].ToString();
                ret = true;
            }

            return ret;
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
