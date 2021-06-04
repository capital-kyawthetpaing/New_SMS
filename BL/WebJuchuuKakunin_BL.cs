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
    public class WebJuchuuKakunin_BL : Base_BL
    {
        D_Juchuu_DL mdl;
        public WebJuchuuKakunin_BL()
        {
            mdl = new D_Juchuu_DL();
        }

        /// <summary>
        /// WEB受注確認にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Juchuu_SelectForWeb(D_Juchuu_Entity dje, D_JuchuuStatus_Entity djse)
        {
            return mdl.D_Juchuu_SelectForWeb(dje, djse);
        }

        public DataTable BindForCombo(D_Juchuu_Entity de, int kbn)
        {
            return mdl.BindForWebJuchuuKakunin(de, kbn);
        }
        //public bool D_PaymentConfirm_Select(D_Collect_Entity de)
        //{
        //    D_PaymentConfirm_DL dp = new D_PaymentConfirm_DL();
        //    DataTable dt = dp.D_PaymentConfirm_Select(de);

        //    if (dt.Rows.Count > 0)
        //        return true;
        //    else
        //        return false;
        //}
        ///// <summary>
        ///// 入金入力（新規時）にて使用
        ///// </summary>
        ///// <param name="de"></param>
        ///// <returns></returns>
        //public DataTable SelectDataForNyukin(D_Collect_Entity de)
        //{
        //    return mdl.SelectDataForNyukin(de);
        //}

        ///// <summary>
        ///// 入金入力更新処理
        ///// NyuukinNyuuryoku_Seikyuより更新時に使用
        ///// </summary>
        //public bool D_Collect_Exec(D_Collect_Entity dce, DataTable dt, short operationMode)
        //{
        //    return mdl.D_Collect_Exec(dce, dt, operationMode);
        //}

        ///// <summary>
        ///// 入金元検索よりデータ抽出時に使用
        ///// </summary>
        ///// <param name="de"></param>
        ///// <returns></returns>
        //public DataTable D_Billing_SelectForSearch(D_Billing_Entity de)
        //{
        //    D_Billing_DL dl = new D_Billing_DL();
        //    return dl.D_Billing_SelectForSearch(de);
        //}

        ///// <summary>
        ///// 入金検索にて使用
        ///// </summary>
        ///// <param name="de"></param>
        ///// <returns></returns>
        //public DataTable D_Collect_SelectForSearch(D_Collect_Entity de)
        //{
        //    return mdl.D_Collect_SelectForSearch(de);
        //}
        ///// <summary>
        ///// 入金消込検索にて使用
        ///// </summary>
        ///// <param name="de"></param>
        ///// <returns></returns>
        //public DataTable D_PaymentConfirm_SelectForSearch(D_PaymentConfirm_Entity de)
        //{
        //    D_PaymentConfirm_DL dpdl = new D_PaymentConfirm_DL();
        //    return dpdl.D_PaymentConfirm_SelectForSearch(de);
        //}

        ///// <summary>
        ///// 締処理済の場合（以下のSelectができる場合）Error
        ///// </summary>
        ///// <param name="de"></param>
        ///// <returns></returns>
        //public bool CheckBillingDate(D_BillingProcessing_Entity de)
        //{
        //    D_BillingProcessing_DL dpd = new D_BillingProcessing_DL();
        //    DataTable dt = dpd.CheckBillingDate(de);

        //    bool ret = false;

        //    if (dt.Rows.Count > 0)
        //    {
        //        de.ProcessingNO = dt.Rows[0]["ProcessingNO"].ToString();
        //        ret = true;
        //    }

        //    return ret;
        //}
    }
}
