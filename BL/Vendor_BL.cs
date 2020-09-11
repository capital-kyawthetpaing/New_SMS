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
    public class Vendor_BL : Base_BL
    {
        M_Vendor_DL mvdl;
        public Vendor_BL()
        {
            mvdl = new M_Vendor_DL();
        }

        public bool M_Vendor_Select(M_Vendor_Entity mve)
        {
            DataTable dt = mvdl.M_Vendor_Select(mve);
            if (dt.Rows.Count > 0)
            {
                mve.VendorCD = dt.Rows[0]["VendorCD"].ToString();
                mve.VendorName = dt.Rows[0]["VendorName"].ToString();
                mve.ShoguchiFlg = dt.Rows[0]["ShoguchiFlg"].ToString();
                mve.VendorLongName1 = dt.Rows[0]["VendorLongName1"].ToString();
                mve.VendorLongName2 = dt.Rows[0]["VendorLongName2"].ToString();
                mve.VendorPostName = dt.Rows[0]["VendorPostName"].ToString();
                mve.VendorPositionName = dt.Rows[0]["VendorPositionName"].ToString();
                mve.VendorStaffName = dt.Rows[0]["VendorStaffName"].ToString();
                mve.VendorKana = dt.Rows[0]["VendorKana"].ToString();
                mve.VendorFlg = dt.Rows[0]["VendorFlg"].ToString();
                mve.PayeeFlg = dt.Rows[0]["PayeeFlg"].ToString();
                mve.MoneyPayeeFlg = dt.Rows[0]["MoneyPayeeFlg"].ToString();
                mve.PayeeCD = dt.Rows[0]["PayeeCD"].ToString();
                mve.MoneyPayeeCD = dt.Rows[0]["MoneyPayeeCD"].ToString();
                mve.ZipCD1 = dt.Rows[0]["ZipCD1"].ToString();
                mve.ZipCD2 = dt.Rows[0]["ZipCD2"].ToString();
                mve.Address1 = dt.Rows[0]["Address1"].ToString();
                mve.Address2 = dt.Rows[0]["Address2"].ToString();
                mve.MailAddress1 = dt.Rows[0]["MailAddress"].ToString();
                mve.TelephoneNO = dt.Rows[0]["TelephoneNO"].ToString();
                mve.FaxNO = dt.Rows[0]["FaxNO"].ToString();
                mve.PaymentCloseDay = dt.Rows[0]["PaymentCloseDay"].ToString();
                mve.PaymentPlanKBN = dt.Rows[0]["PaymentPlanKBN"].ToString();
                mve.PaymentPlanDay = dt.Rows[0]["PaymentPlanDay"].ToString();
                mve.HolidayKBN = dt.Rows[0]["HolidayKBN"].ToString();
                mve.BankCD = dt.Rows[0]["BankCD"].ToString();
                mve.BranchCD = dt.Rows[0]["BranchCD"].ToString();
                mve.KouzaKBN = dt.Rows[0]["KouzaKBN"].ToString();
                mve.KouzaNO = dt.Rows[0]["KouzaNO"].ToString();
                mve.KouzaMeigi = dt.Rows[0]["KouzaMeigi"].ToString();
                mve.KouzaCD = dt.Rows[0]["KouzaCD"].ToString();
                mve.TaxTiming = dt.Rows[0]["TaxTiming"].ToString();
                mve.TaxFractionKBN = dt.Rows[0]["TaxFractionKBN"].ToString();
                mve.AmountFractionKBN = dt.Rows[0]["AmountFractionKBN"].ToString();
                mve.NetFlg = dt.Rows[0]["NetFlg"].ToString();
                mve.LastOrderDate = dt.Rows[0]["LastOrderDate"].ToString();
                mve.StaffCD = dt.Rows[0]["StaffCD"].ToString();
                mve.AnalyzeCD1 = dt.Rows[0]["AnalyzeCD1"].ToString();
                mve.AnalyzeCD2 = dt.Rows[0]["AnalyzeCD2"].ToString();
                mve.AnalyzeCD3 = dt.Rows[0]["AnalyzeCD3"].ToString();
                mve.DisplayOrder = dt.Rows[0]["DisplayOrder"].ToString();
                mve.NotDisplyNote = dt.Rows[0]["NotDisplyNote"].ToString();
                mve.DisplayNote = dt.Rows[0]["DisplayNote"].ToString();
                mve.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();

                return true;
            }
            else
                return false;
        }



        public bool M_Vendor_SelectTop1(M_Vendor_Entity mve)
        {
            DataTable dt = mvdl.M_Vendor_SelectTop1(mve);
            if (dt.Rows.Count > 0)
            {
                mve.VendorCD = dt.Rows[0]["VendorCD"].ToString();
                mve.VendorName = dt.Rows[0]["VendorName"].ToString();
                mve.ShoguchiFlg = dt.Rows[0]["ShoguchiFlg"].ToString();
                mve.VendorLongName1 = dt.Rows[0]["VendorLongName1"].ToString();
                mve.VendorLongName2 = dt.Rows[0]["VendorLongName2"].ToString();
                mve.VendorPostName = dt.Rows[0]["VendorPostName"].ToString();
                mve.VendorPositionName = dt.Rows[0]["VendorPositionName"].ToString();
                mve.VendorStaffName = dt.Rows[0]["VendorStaffName"].ToString();
                mve.VendorKana = dt.Rows[0]["VendorKana"].ToString();
                mve.VendorFlg = dt.Rows[0]["VendorFlg"].ToString();
                mve.PayeeFlg = dt.Rows[0]["PayeeFlg"].ToString();
                mve.MoneyPayeeFlg = dt.Rows[0]["MoneyPayeeFlg"].ToString();
                mve.PayeeCD = dt.Rows[0]["PayeeCD"].ToString();
                mve.MoneyPayeeCD = dt.Rows[0]["MoneyPayeeCD"].ToString();
                mve.ZipCD1 = dt.Rows[0]["ZipCD1"].ToString();
                mve.ZipCD2 = dt.Rows[0]["ZipCD2"].ToString();
                mve.Address1 = dt.Rows[0]["Address1"].ToString();
                mve.Address2 = dt.Rows[0]["Address2"].ToString();
                mve.MailAddress1 = dt.Rows[0]["MailAddress1"].ToString();
                mve.MailAddress2 = dt.Rows[0]["MailAddress2"].ToString();
                mve.MailAddress3 = dt.Rows[0]["MailAddress3"].ToString();
                mve.TelephoneNO = dt.Rows[0]["TelephoneNO"].ToString();
                mve.FaxNO = dt.Rows[0]["FaxNO"].ToString();
                mve.PaymentCloseDay = dt.Rows[0]["PaymentCloseDay"].ToString();
                mve.PaymentPlanKBN = dt.Rows[0]["PaymentPlanKBN"].ToString();
                mve.PaymentPlanDay = dt.Rows[0]["PaymentPlanDay"].ToString();
                mve.HolidayKBN = dt.Rows[0]["HolidayKBN"].ToString();
                mve.BankCD = dt.Rows[0]["BankCD"].ToString();
                mve.BranchCD = dt.Rows[0]["BranchCD"].ToString();
                mve.KouzaKBN = dt.Rows[0]["KouzaKBN"].ToString();
                mve.KouzaNO = dt.Rows[0]["KouzaNO"].ToString();
                mve.KouzaMeigi = dt.Rows[0]["KouzaMeigi"].ToString();
                mve.KouzaCD = dt.Rows[0]["KouzaCD"].ToString();
                mve.TaxTiming = dt.Rows[0]["TaxTiming"].ToString();
                mve.TaxFractionKBN = dt.Rows[0]["TaxFractionKBN"].ToString();
                mve.AmountFractionKBN = dt.Rows[0]["AmountFractionKBN"].ToString();
                mve.NetFlg = dt.Rows[0]["NetFlg"].ToString();
                mve.EDIFlg = dt.Rows[0]["EDIFlg"].ToString();
                mve.LastOrderDate = dt.Rows[0]["LastOrderDate"].ToString();
                mve.StaffCD = dt.Rows[0]["StaffCD"].ToString();
                mve.AnalyzeCD1 = dt.Rows[0]["AnalyzeCD1"].ToString();
                mve.AnalyzeCD2 = dt.Rows[0]["AnalyzeCD2"].ToString();
                mve.AnalyzeCD3 = dt.Rows[0]["AnalyzeCD3"].ToString();
                mve.DisplayOrder = dt.Rows[0]["DisplayOrder"].ToString();
                mve.NotDisplyNote = dt.Rows[0]["NotDisplyNote"].ToString();
                mve.DisplayNote = dt.Rows[0]["DisplayNote"].ToString();
                mve.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
                return true;
            }
            else
                return false;
        }


        public bool M_Vendor_Search(M_Vendor_Entity mve)
        {
            DataTable dt = mvdl.M_Vendor_Search(mve);
            if (dt.Rows.Count > 0)
            {
                mve.VendorCD = dt.Rows[0]["VendorCD"].ToString();
                mve.VendorName = dt.Rows[0]["VendorName"].ToString();
                return true;
            }
            else
                return false;
        }
        public bool M_Vendor_Select_Tenji(M_Vendor_Entity mve)
        {
            DataTable dt = mvdl.M_Vendor_Select_Tenji(mve);
            if (dt.Rows.Count > 0)
            {
                mve.VendorCD = dt.Rows[0]["VendorCD"].ToString();
                mve.VendorName = dt.Rows[0]["VendorName"].ToString();
                return true;
            }
            else
                return false;
        }
        public bool M_Vendor_SelectForPayeeCD(M_Vendor_Entity mve)
        {
            DataTable dt = mvdl.M_Vendor_SelectForPayeeCD(mve);
            if (dt.Rows.Count > 0)
            {
                mve.VendorCD = dt.Rows[0]["VendorCD"].ToString();
                mve.VendorName = dt.Rows[0]["VendorName"].ToString();
                mve.TaxTiming = dt.Rows[0]["TaxTiming"].ToString();

                return true;
            }
            else
                return false;
        }
        public DataTable M_SearchVendor(M_Vendor_Entity mve)
        {
            return mvdl.M_SearchVendor(mve);
        }
    }
}
