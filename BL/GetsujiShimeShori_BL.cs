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
    public class GetsujiShimeShori_BL : Base_BL 
    {
        M_StoreClose_DL mdl;
        public GetsujiShimeShori_BL()
        {
            mdl = new M_StoreClose_DL();
        }
        public int GetMode(M_Control_Entity me)
        {
            int ret = 0;
            M_Control_DL dl = new M_Control_DL();
            DataTable dt = dl.M_Control_Select(me);

            if (dt.Rows.Count > 0)
            {
                ret =Convert.ToInt16( Z_Set(dt.Rows[0]["MonthlySummuryKBN"]));
            }
            return ret;
        }
        public bool M_StoreClose_SelectAll(M_StoreClose_Entity me)
        {
            M_StoreClose_DL dl = new M_StoreClose_DL();
            DataTable dt = dl.M_StoreClose_SelectAll(me);
            if (dt.Rows.Count > 0)
            {
                me.FiscalYYYYMM = dt.Rows[0]["FiscalYYYYMM"].ToString();
                me.ClosePosition1 = dt.Rows[0]["ClosePosition1"].ToString();
                me.ClosePosition2 = dt.Rows[0]["ClosePosition2"].ToString();
                me.ClosePosition3 = dt.Rows[0]["ClosePosition3"].ToString();
                me.ClosePosition4 = dt.Rows[0]["ClosePosition4"].ToString();
                me.ClosePosition5 = dt.Rows[0]["ClosePosition5"].ToString();
                me.MonthlyClaimsFLG = dt.Rows[0]["MonthlyClaimsFLG"].ToString();
                me.MonthlyClaimsDateTime = dt.Rows[0]["MonthlyClaimsDateTime"].ToString();
                me.MonthlyDebtFLG = dt.Rows[0]["MonthlyDebtFLG"].ToString();
                me.MonthlyDebtDateTime = dt.Rows[0]["MonthlyDebtDateTime"].ToString();
                me.MonthlyStockFLG = dt.Rows[0]["MonthlyStockFLG"].ToString();
                me.MonthlyStockDateTime = dt.Rows[0]["MonthlyStockDateTime"].ToString();
                return true;
            }
            else
            {
                if(string.IsNullOrWhiteSpace( me.FiscalYYYYMM))
                {
                    string ymd = GetDate();
                    me.FiscalYYYYMM = ymd.Substring(0,4) + ymd.Substring(5, 2);
                }
                //レコ―ドが無い場合、 テーブル転送仕様Ａに従ってInsert	M_StoreClose
                dl.M_StoreClose_Insert(me);
            }

            return false;
        }
        public string GetHonsha(M_Store_Entity me)
        {
            M_Store_DL dl = new M_Store_DL();
            DataTable dt = dl.GetHonsha(me);
            if (dt.Rows.Count > 0)
            {
                me.StoreCD = dt.Rows[0]["StoreCD"].ToString();

                return me.StoreCD;
            }
            return "";
        }
        /// <summary>
        /// 月次締処理
        /// SeikyuuShimeShoriより更新時に使用
        /// </summary>
        public bool M_StoreClose_Update(M_StoreClose_Entity me)
        {
            M_StoreClose_DL dl = new M_StoreClose_DL();
            return dl.M_StoreClose_Update(me);
        }
        /// <summary>
        /// 月次債権計算処理
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool GetsujiSaikenKeisanSyori(D_MonthlyClaims_Entity de)
        {
            D_MonthlyClaims_DL dl = new D_MonthlyClaims_DL();
            return dl.D_MonthlyClaims_Exec(de);
        }
        /// <summary>
        /// 月次債務計算処理
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool GetsujiSaimuKeisanSyori(D_MonthlyDebt_Entity de) 
        {
            D_MonthlyDebt_DL dl = new D_MonthlyDebt_DL();
            return dl.D_MonthlyDebt_Exec(de);
        }
        /// <summary>
        /// 月次在庫計算処理
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool GetsujiZaikoKeisanSyori(D_MonthlyStock_Entity de)
        {
            D_MonthlyStock_DL dl = new D_MonthlyStock_DL();
            return dl.D_MonthlyStock_Exec(de);
        }
        /// <summary>
        /// 月次仕入計算処理
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool GetsujiShiireKeisanSyori(D_MonthlyPurchase_Entity de)
        {
            D_MonthlyPurchase_DL dl = new D_MonthlyPurchase_DL();
            return dl.D_MonthlyPurchase_Exec(de);
        }
    }
}
