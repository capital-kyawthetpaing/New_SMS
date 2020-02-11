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
    public class ShiireTankaTeiseiIraisho_BL : Base_BL
    {
        D_Purchase_DL ddl;
        public ShiireTankaTeiseiIraisho_BL()
        {
            ddl = new D_Purchase_DL();
        }

        /// <summary>
        /// 仕入単価訂正依頼書印刷よりデータ取得
        /// </summary>
        /// <param name="dpe"></param>
        /// <returns></returns>
        public DataTable D_Purchase_SelectForPrint(D_Purchase_Entity dpe)
        {
            return ddl.D_Purchase_SelectForPrint(dpe);
        }

        /// <summary>
        /// 仕入単価訂正依頼書印刷時のフラグ更新処理
        /// フラグ更新時に使用
        /// </summary>
        public bool D_Purchase_Update(D_Purchase_Entity dme, DataTable dt)
        {
            return ddl.D_Purchase_Update(dme, dt);
        }
    }
}
