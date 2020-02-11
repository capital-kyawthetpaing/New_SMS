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
    public class HenpinNyuuryoku_BL : Base_BL
    {
        D_Purchase_DL ddl;
        public HenpinNyuuryoku_BL()
        {
            ddl = new D_Purchase_DL();
        }

        /// <summary>
        /// 返品入力更新処理
        /// HenpinNyuuryokuより更新時に使用
        /// </summary>
        public bool Purchase_Exec(D_Purchase_Entity dme, DataTable dt, short operationMode)
        {
            return ddl.D_Purchase_ExecH(dme, dt, operationMode);
        }

        /// <summary>
        /// 返品入力取得処理
        /// HenpinNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Purchase_SelectDataH(D_Purchase_Entity de, short operationMode)
        {
            DataTable dt = ddl.D_Purchase_SelectDataH(de, operationMode);

            return dt;
        }

        /// <summary>
        /// 返品入力取得処理(入荷から)
        /// HenpinNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Stock_SelectAllForShiireH(D_Stock_Entity de)
        {
            D_Stock_DL dadl = new D_Stock_DL();
            DataTable dt = dadl.D_Stock_SelectAllForShiireH(de);

            return dt;
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

            if (dt.Rows.Count>0)
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
