using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;

namespace BL
{
   public class TempoRegiHanbaiTouroku_BL : Base_BL
    {
        public TempoRegiHanbaiTouroku_BL()
        {
        }

        public DataTable CheckSalesNo(D_Sales_Entity dse, short operationMode)
        {
            D_Sales_DL dl = new D_Sales_DL();
           return dl.D_Sales_SelectData(dse, operationMode);
        }
        public DataTable D_Sales_SelectForRegi(D_Sales_Entity de, short operationMode)
        {
            D_Sales_DL dl = new D_Sales_DL();
            return dl.D_Sales_SelectForRegi(de, operationMode);
        }
        public string GetMaeukeKin(D_Collect_Entity de)
        {
            string ret = "";
            D_Collect_DL dl = new D_Collect_DL();
            DataTable dt = dl.D_Collect_SelectMaeukeKin(de);

            if (dt.Rows.Count > 0)
            {
                ret =Z_SetStr( dt.Rows[0]["MaeukeKin"]);
            }
            return ret;
        }
        public string GetHaspoMode(M_Control_Entity me)
        {
            string ret = "";
            M_Control_DL dl = new M_Control_DL();
            DataTable dt = dl.M_Control_Select(me);

            if (dt.Rows.Count > 0)
            {
                ret = Z_SetStr(dt.Rows[0]["Haspo"]);
            }
            return ret;
        }
        public bool D_StorePayment_Select(D_StorePayment_Entity dse)
        {
            D_StorePayment_DL dl = new D_StorePayment_DL();
            DataTable dt = dl.D_StorePayment_Select(dse);

            if(dt.Rows.Count > 0)
            {
                dse.PurchaseAmount = dt.Rows[0]["PurchaseAmount"].ToString();
                dse.TaxAmount = dt.Rows[0]["TaxAmount"].ToString();
                dse.DiscountAmount = dt.Rows[0]["DiscountAmount"].ToString();
                dse.BillingAmount = dt.Rows[0]["BillingAmount"].ToString();
                dse.PointAmount = dt.Rows[0]["PointAmount"].ToString();
                dse.CardDenominationName = dt.Rows[0]["CardDenominationName"].ToString();
                dse.CardDenominationCD = dt.Rows[0]["CardDenominationCD"].ToString();
                dse.CardAmount = dt.Rows[0]["CardAmount"].ToString();
                dse.CashAmount = dt.Rows[0]["CashAmount"].ToString();
                dse.DepositAmount = dt.Rows[0]["DepositAmount"].ToString();
                dse.RefundAmount = dt.Rows[0]["RefundAmount"].ToString();
                dse.CreditAmount = dt.Rows[0]["CreditAmount"].ToString();
                dse.DenominationName1 = dt.Rows[0]["DenominationName1"].ToString();
                dse.Denomination1CD = dt.Rows[0]["Denomination1CD"].ToString();
                dse.Denomination1Amount = dt.Rows[0]["Denomination1Amount"].ToString();
                dse.DenominationName2 = dt.Rows[0]["DenominationName2"].ToString();
                dse.Denomination2CD = dt.Rows[0]["Denomination2CD"].ToString();
                dse.Denomination2Amount = dt.Rows[0]["Denomination2Amount"].ToString();
                dse.AdvanceAmount = dt.Rows[0]["AdvanceAmount"].ToString();
                dse.TotalAmount = dt.Rows[0]["TotalAmount"].ToString();
                dse.SalesRate = dt.Rows[0]["SalesRate"].ToString();

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PRC_TempoRegiHanbaiTouroku(D_Sales_Entity dse, D_StorePayment_Entity dspe, DataTable dt, short operationMode)
        {
            TempoRegiHanbaiTouroku_DL dl = new TempoRegiHanbaiTouroku_DL();
            return dl.PRC_TempoRegiHanbaiTouroku(dse, dspe, dt, operationMode);

        }
        public bool PRC_TempoRegiDataUpdate(D_Sales_Entity dse, int operationMode)
        {
            TempoRegiHanbaiTouroku_DL dl = new TempoRegiHanbaiTouroku_DL();
            return dl.PRC_TempoRegiDataUpdate(dse, operationMode);

        }
        public bool D_StoreCalculation_Select(D_StoreCalculation_Entity dse)
        {
            TempoRegiHanbaiTouroku_DL dl = new TempoRegiHanbaiTouroku_DL();
            DataTable dt =   dl.D_StoreCalculation_Select(dse);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

    }
}
