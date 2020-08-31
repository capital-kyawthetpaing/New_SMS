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
    public class UriageNyuuryoku_BL : Base_BL
    {
        D_Sales_DL dsdl;
        public UriageNyuuryoku_BL()
        {
            dsdl = new D_Sales_DL();
        }
        public DataTable M_Souko_IsExists(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_IsExists(mse);
        }

        /// <summary>
        /// 売上入力更新処理
        /// UriageNyuuryokuより更新時に使用
        /// </summary>
        public bool Sales_Exec(D_Sales_Entity de, DataTable dt, short operationMode)
        {
            return dsdl.D_Sales_Exec(de, dt, operationMode);
        }

        /// <summary>
        /// 売上入力取得処理
        /// TempoJuchuuNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Sales_SelectData(D_Sales_Entity de, short operationMode, short tennic = 0)
        {
            DataTable dt = dsdl.D_Sales_SelectDataForUriageNyuuryoku(de, operationMode, tennic);

            return dt;
        }
        /// <summary>
        /// 進捗チェック　
        /// 既に入金消込済みの場合、エラー
        /// </summary>
        /// <param name="dse"></param>
        /// <param name="errno"></param>
        /// <returns></returns>
        public bool CheckSalesData(D_Sales_Entity dse, out string errno, short tennic = 0)
        {
            bool ret = false;
            errno = "";

            DataTable dt = dsdl.CheckSalesData(dse);

            if (dt.Rows.Count > 0)
            {
                errno = dt.Rows[0]["errno"].ToString();
                ret = true;
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
        ///// <summary>
        ///// 入荷進捗、出荷進捗
        ///// </summary>
        ///// <param name="juchuNo"></param>
        ///// <param name="status">入荷進捗</param>
        ///// <param name="status2">出荷進捗</param>
        ///// <returns></returns>
        //public bool CheckJuchuDetailsData(string juchuNo,string juchuGyoNo, out string status, out string status2)
        //{
        //    DataTable dt = dsdl.CheckJuchuDetailsData(juchuNo, juchuGyoNo);

        //    bool ret = false;
        //    status = "";
        //    status2 = "";

        //    if (dt.Rows.Count > 0)
        //    {
        //        status = dt.Rows[0]["STATUS"].ToString();
        //        status2 = dt.Rows[0]["STATUS2"].ToString();

        //        ret = true;
        //    }

        //    return ret;
        //}

        //public string GetNouki(string date,string storeCD)
        //{
        //    DataTable dt = dsdl.GetNouki(date, storeCD);
        //    if (dt.Rows.Count > 0)
        //        return dt.Rows[0]["Nouki"].ToString();

        //    return "";
        //}
    }
}
