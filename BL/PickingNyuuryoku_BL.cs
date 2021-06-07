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
    public class PickingNyuuryoku_BL : Base_BL
    {
        D_Picking_DL ddl;
        public PickingNyuuryoku_BL()
        {
            ddl = new D_Picking_DL();
        }

        /// <summary>
        /// ピッキング番号検索
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Picking_SelectAll(D_Picking_Entity de)
        {
            return ddl.D_Picking_SelectAll(de);
        }

        /// <summary>
        /// ピッキング入力更新処理(入荷から)
        /// PickingNyuuryokuより更新時に使用
        /// </summary>
        public bool Picking_Exec(D_Picking_Entity dme, DataTable dt, short operationMode)
        {
            return ddl.D_Picking_Exec(dme, dt, operationMode);
        }

        /// <summary>
        /// ピッキング入力データ取得処理
        /// PickingNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Picking_SelectData(D_Picking_Entity de, short operationMode)
        {
            DataTable dt = ddl.D_Picking_SelectData(de, operationMode);

            return dt;
        }

        public DataTable M_Souko_BindForPicking(M_Souko_Entity me)
        {
            M_Souko_DL dl = new M_Souko_DL();
            return dl.M_Souko_BindForPicking(me);
        }

        /// <summary>
        /// 締処理済チェック　
        /// D_PayPlanに、支払締番号がセットされていれば、エラー
        /// </summary>
        /// <param name="No"></param>
        /// <param name="errno"></param>
        /// <returns></returns>
        public bool CheckPayPlanData(string No, out string errno)
        {
            D_PayPlan_DL dpd = new D_PayPlan_DL();
            DataTable dt = dpd.CheckPayPlanData(No);

            bool ret = false;
            errno = "";

            if (dt.Rows.Count > 0)
            {
                errno = dt.Rows[0]["errno"].ToString();
            }

            return ret;
        }

        /// <summary>
        /// 締処理済の場合（以下のSelectができる場合）Error
        /// </summary>
        /// <param name="dpe"></param>
        /// <returns></returns>
        public bool CheckPayCloseHistory(D_PayCloseHistory_Entity dpe)
        {
            D_PayCloseHistory_DL dpd = new D_PayCloseHistory_DL();
            DataTable dt = dpd.CheckPayCloseHistory(dpe);

            bool ret = false;

            if (dt.Rows.Count > 0)
            {
                dpe.PayCloseNO = dt.Rows[0]["PayCloseNO"].ToString();
                ret = true;
            }

            return ret;
        }
    }
}
