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
    public class Customer_BL : Base_BL
    {
        M_Customer_DL mmdl;
        public Customer_BL()
        {
            mmdl = new M_Customer_DL();
        }

        /// <summary>
        /// 得意先データ取得処理
        /// </summary>
        /// <param name="mce"></param>
        /// <param name="kbn">CustomerKBN 1:店舗会員、2:店舗現金会員、3:団体法人</param>
        /// <returns></returns>
        public bool M_Customer_Select(M_Customer_Entity mce, short kbn = 0)
        {
            DataTable dt = mmdl.M_Customer_Select(mce);

            if (kbn > 0)
            {
                DataRow[] rows = dt.Select("CustomerKBN <>" + kbn.ToString());
                foreach (DataRow row in rows)
                    dt.Rows.Remove(row);
            }

            if (dt.Rows.Count > 0)
            {
                mce.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();
                mce.VariousFLG = dt.Rows[0]["VariousFLG"].ToString();
                mce.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                mce.LastName = dt.Rows[0]["LastName"].ToString();
                mce.FirstName = dt.Rows[0]["FirstName"].ToString();
                mce.LongName1 = dt.Rows[0]["LongName1"].ToString();
                mce.LongName2 = dt.Rows[0]["LongName2"].ToString();
                mce.KanaName = dt.Rows[0]["KanaName"].ToString();
                mce.StoreKBN = dt.Rows[0]["StoreKBN"].ToString();
                mce.CustomerKBN = dt.Rows[0]["CustomerKBN"].ToString();
                mce.StoreTankaKBN = dt.Rows[0]["StoreTankaKBN"].ToString();
                mce.AliasKBN = dt.Rows[0]["AliasKBN"].ToString();
                mce.BillingType = dt.Rows[0]["BillingType"].ToString();
                mce.GroupName = dt.Rows[0]["GroupName"].ToString();
                mce.BillingFLG = dt.Rows[0]["BillingFLG"].ToString();
                mce.CollectFLG = dt.Rows[0]["CollectFLG"].ToString();
                mce.BillingCD = dt.Rows[0]["BillingCD"].ToString();
                mce.CollectCD = dt.Rows[0]["CollectCD"].ToString();
                mce.Birthdate = dt.Rows[0]["Birthdate"].ToString();
                mce.Sex = dt.Rows[0]["Sex"].ToString();
                mce.Tel11 = dt.Rows[0]["Tel11"].ToString();
                mce.Tel12 = dt.Rows[0]["Tel12"].ToString();
                mce.Tel13 = dt.Rows[0]["Tel13"].ToString();
                mce.Tel21 = dt.Rows[0]["Tel21"].ToString();
                mce.Tel22 = dt.Rows[0]["Tel22"].ToString();
                mce.Tel23 = dt.Rows[0]["Tel23"].ToString();
                mce.ZipCD1 = dt.Rows[0]["ZipCD1"].ToString();
                mce.ZipCD2 = dt.Rows[0]["ZipCD2"].ToString();
                mce.Address1 = dt.Rows[0]["Address1"].ToString();
                mce.Address2 = dt.Rows[0]["Address2"].ToString();
                mce.MailAddress = dt.Rows[0]["MailAddress"].ToString();
                mce.TankaCD = dt.Rows[0]["TankaCD"].ToString();
                mce.PointFLG = dt.Rows[0]["PointFLG"].ToString();
                mce.LastPoint = dt.Rows[0]["LastPoint"].ToString();
                mce.WaitingPoint = dt.Rows[0]["WaitingPoint"].ToString();
                mce.TotalPoint = dt.Rows[0]["TotalPoint"].ToString();
                mce.TotalPurchase = dt.Rows[0]["TotalPurchase"].ToString();
                mce.UnpaidAmount = dt.Rows[0]["UnpaidAmount"].ToString();
                mce.UnpaidCount = dt.Rows[0]["UnpaidCount"].ToString();
                mce.LastSalesDate = dt.Rows[0]["LastSalesDate"].ToString();
                mce.LastSalesStoreCD = dt.Rows[0]["LastSalesStoreCD"].ToString();
                mce.MainStoreCD = dt.Rows[0]["MainStoreCD"].ToString();
                mce.StaffCD = dt.Rows[0]["StaffCD"].ToString();
                mce.AttentionFLG = dt.Rows[0]["AttentionFLG"].ToString();
                mce.ConfirmFLG = dt.Rows[0]["ConfirmFLG"].ToString();
                mce.ConfirmComment = dt.Rows[0]["ConfirmComment"].ToString();
                mce.BillingCloseDate = dt.Rows[0]["BillingCloseDate"].ToString();
                mce.CollectPlanMonth = dt.Rows[0]["CollectPlanMonth"].ToString();
                mce.CollectPlanDate = dt.Rows[0]["CollectPlanDate"].ToString();
                mce.HolidayKBN = dt.Rows[0]["HolidayKBN"].ToString();
                mce.TaxTiming = dt.Rows[0]["TaxTiming"].ToString();
                mce.TaxFractionKBN = dt.Rows[0]["TaxFractionKBN"].ToString();
                mce.AmountFractionKBN = dt.Rows[0]["AmountFractionKBN"].ToString();
                mce.CreditLevel = dt.Rows[0]["CreditLevel"].ToString();
                mce.CreditCard = dt.Rows[0]["CreditCard"].ToString();
                mce.CreditInsurance = dt.Rows[0]["CreditInsurance"].ToString();
                mce.CreditDeposit = dt.Rows[0]["CreditDeposit"].ToString();
                mce.CreditETC = dt.Rows[0]["CreditETC"].ToString();
                mce.CreditAmount = dt.Rows[0]["CreditAmount"].ToString();
               // mce.CreditWarningAmount = dt.Rows[0]["CreditWarningAmount"].ToString();
                mce.CreditAdditionAmount = dt.Rows[0]["CreditAdditionAmount"].ToString();
                mce.CreditCheckKBN = dt.Rows[0]["CreditCheckKBN"].ToString();
                mce.CreditMessage = dt.Rows[0]["CreditMessage"].ToString();
                mce.FareLevel = dt.Rows[0]["FareLevel"].ToString();
                mce.Fare = dt.Rows[0]["Fare"].ToString();
                mce.PaymentMethodCD = dt.Rows[0]["PaymentMethodCD"].ToString();
                mce.KouzaCD = dt.Rows[0]["KouzaCD"].ToString();
                mce.DisplayOrder = dt.Rows[0]["DisplayOrder"].ToString();
                mce.PaymentUnit = dt.Rows[0]["PaymentUnit"].ToString();
                mce.NoInvoiceFlg = dt.Rows[0]["NoInvoiceFlg"].ToString();
                mce.CountryKBN = dt.Rows[0]["CountryKBN"].ToString();
                mce.CountryName = dt.Rows[0]["CountryName"].ToString();
                mce.RegisteredNumber = dt.Rows[0]["RegisteredNumber"].ToString();
                mce.DMFlg = dt.Rows[0]["DMFlg"].ToString();
                mce.RemarksOutStore = dt.Rows[0]["RemarksOutStore"].ToString();
                mce.RemarksInStore = dt.Rows[0]["RemarksInStore"].ToString();
                mce.AnalyzeCD1 = dt.Rows[0]["AnalyzeCD1"].ToString();
                mce.AnalyzeCD2 = dt.Rows[0]["AnalyzeCD2"].ToString();
                mce.AnalyzeCD3 = dt.Rows[0]["AnalyzeCD3"].ToString();
                mce.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
                mce.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();
                mce.InsertOperator = dt.Rows[0]["InsertOperator"].ToString();
                mce.InsertDateTime = dt.Rows[0]["InsertDateTime"].ToString();
                mce.UpdateOperator = dt.Rows[0]["UpdateOperator"].ToString();
                mce.UpdateDateTime = dt.Rows[0]["UpdateDateTime"].ToString();

                return true;
            }
            else
                return false;

        }
        /// <summary>
        /// 得意先更新処理
        /// MasterTouroku_Tokuisakiより更新時に使用
        /// </summary>
        public bool Customer_Exec(M_Customer_Entity me, short operationMode)
        {
            return mmdl.M_Customer_Exec(me, operationMode);
        }

        public bool M_Customer_Select_Update(string No,M_Customer_Entity mce,short kbn = 0, bool IsUpdate=false)
        {
            var tbl = new TenjikaiJuuChuu_BL();
            var tje = new Tenjikai_Entity() { TenjiKaiOrderNo = No };

            var res = tbl.Select_TenjiData(tje, out DataTable GridDt);
            var cs = M_Customer_Select(mce,-1);
            if (!cs)
                return false;
            if (res)
            {
                if (kbn == 0)  // Kokyaku
                {
                    mce.Tel11 = tje.K_Denwa1;
                    mce.Tel12 = tje.K_Denwa2;
                    mce.Tel13 = tje.K_Denwa3;
                    mce.ZipCD1 = tje.K_Zip1;
                    mce.ZipCD2 = tje.K_Zip2;
                    mce.Address1 = tje.K_Address1;
                    mce.Address2 = tje.K_Address2;
                    mce.CustomerName = tje.K_Name1;
                    mce.VariousFLG = tje.CVFlg;
                    mce.AliasKBN = tje.CKBN == "様" ? "1" : "0";
                    //mce.TaxTiming = "";
                    //mce.TaxFractionKBN = "";
                }
                else  // Haisou
                {
                    mce.Tel11 = tje.H_Denwa1;
                    mce.Tel12 = tje.H_Denwa2;
                    mce.Tel13 = tje.H_Denwa3;
                    mce.ZipCD1 = tje.H_Zip1;
                    mce.ZipCD2 = tje.H_Zip2;
                    mce.Address1 = tje.H_Address1;
                    mce.Address2 = tje.H_Address2;
                    mce.CustomerName = tje.H_Name1;
                    mce.VariousFLG = tje.DVFlg;
                    mce.AliasKBN = tje.HKBN == "様" ? "1" : "0";
                    //mce.TaxTiming = "";
                    //mce.TaxFractionKBN = "";
                }


            }
            else
                return false;
            return true;
        }
    }
}
