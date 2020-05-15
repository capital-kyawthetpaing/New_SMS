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
    public class MasterTouroku_Shiiresaki_BL : Base_BL
    {
        M_Vendor_DL mvdl;
        M_ZipCode_DL mzdl;
        M_Bank_DL mbdl;
        M_Kouza_DL mkdl;
        M_BankShiten_DL mbsdl;
        M_Staff_DL msdl;

        public MasterTouroku_Shiiresaki_BL()
        {
            mvdl = new M_Vendor_DL();
            mzdl = new M_ZipCode_DL();
            mbdl = new M_Bank_DL();
            mkdl = new M_Kouza_DL();
            mbsdl = new M_BankShiten_DL();
            msdl = new M_Staff_DL();

        }

        public M_Vendor_Entity M_Vendor_Select(M_Vendor_Entity mve)
        {
            DataTable dtVendor = mvdl.M_Vendor_Select(mve);
            if (dtVendor.Rows.Count > 0)
            {
                mve.ShoguchiFlg = dtVendor.Rows[0]["ShoguchiFlg"].ToString();
                mve.VendorName = dtVendor.Rows[0]["VendorName"].ToString();
                mve.VendorShortName = dtVendor.Rows[0]["VendorShortName"].ToString(); // Add By SawLay
                mve.VendorKana = dtVendor.Rows[0]["VendorKana"].ToString();
                mve.VendorLongName1 = dtVendor.Rows[0]["VendorLongName1"].ToString();
                mve.VendorLongName2 = dtVendor.Rows[0]["VendorLongName2"].ToString();
                mve.VendorPostName = dtVendor.Rows[0]["VendorPostName"].ToString();
                mve.VendorPositionName = dtVendor.Rows[0]["VendorPositionName"].ToString();
                mve.VendorStaffName = dtVendor.Rows[0]["VendorStaffName"].ToString();
                mve.ZipCD1 = dtVendor.Rows[0]["ZipCD1"].ToString();
                mve.ZipCD2 = dtVendor.Rows[0]["ZipCD2"].ToString();
                mve.Address1 = dtVendor.Rows[0]["Address1"].ToString();
                mve.Address2 = dtVendor.Rows[0]["Address2"].ToString();
                mve.MailAddress1 = dtVendor.Rows[0]["MailAddress1"].ToString();
                mve.TelephoneNO = dtVendor.Rows[0]["TelephoneNO"].ToString();
                mve.FaxNO = dtVendor.Rows[0]["FaxNO"].ToString();
                mve.PayeeCD = dtVendor.Rows[0]["PayeeCD"].ToString();
                mve.payeeName = dtVendor.Rows[0]["payeeName"].ToString();
                mve.MoneyPayeeCD = dtVendor.Rows[0]["MoneyPayeeCD"].ToString();
                mve.moneypayeeName = dtVendor.Rows[0]["moneypayeeName"].ToString();
                mve.PaymentCloseDay = dtVendor.Rows[0]["PaymentCloseDay"].ToString();
                mve.PaymentPlanKBN = dtVendor.Rows[0]["PaymentPlanKBN"].ToString();
                mve.PaymentPlanDay = dtVendor.Rows[0]["PaymentPlanDay"].ToString();
                mve.HolidayKBN = dtVendor.Rows[0]["HolidayKBN"].ToString();
                mve.TaxTiming = dtVendor.Rows[0]["TaxTiming"].ToString();
                mve.TaxFractionKBN = dtVendor.Rows[0]["TaxFractionKBN"].ToString();
                mve.AmountFractionKBN = dtVendor.Rows[0]["AmountFractionKBN"].ToString();
                mve.BankCD = dtVendor.Rows[0]["BankCD"].ToString();
                mve.BankName = dtVendor.Rows[0]["BankName"].ToString();
                mve.BranchCD = dtVendor.Rows[0]["BranchCD"].ToString();
                mve.BranchName = dtVendor.Rows[0]["BranchName"].ToString();
                mve.KouzaKBN = dtVendor.Rows[0]["KouzaKBN"].ToString();
                mve.KouzaNO = dtVendor.Rows[0]["KouzaNO"].ToString();
                mve.KouzaMeigi = dtVendor.Rows[0]["KouzaMeigi"].ToString();
                mve.KouzaCD = dtVendor.Rows[0]["KouzaCD"].ToString();
                mve.KouzaName = dtVendor.Rows[0]["KouzaName"].ToString();
                mve.NetFlg = dtVendor.Rows[0]["NetFlg"].ToString();
                mve.EDIFlg = dtVendor.Rows[0]["EDIFlg"].ToString();
                mve.EDIVendorCD = dtVendor.Rows[0]["EDIVendorCD"].ToString();
                mve.StaffCD = dtVendor.Rows[0]["StaffCD"].ToString();
                mve.StaffName = dtVendor.Rows[0]["StaffName"].ToString();
                mve.AnalyzeCD1 = dtVendor.Rows[0]["AnalyzeCD1"].ToString();
                mve.AnalyzeCD2 = dtVendor.Rows[0]["AnalyzeCD2"].ToString();
                mve.AnalyzeCD3 = dtVendor.Rows[0]["AnalyzeCD3"].ToString();
                mve.DisplayOrder = dtVendor.Rows[0]["DisplayOrder"].ToString();
                mve.DisplayNote = dtVendor.Rows[0]["DisplayNote"].ToString();
                mve.NotDisplyNote = dtVendor.Rows[0]["NotDisplyNote"].ToString();
                mve.DeleteFlg = dtVendor.Rows[0]["DeleteFlg"].ToString();

                return mve;
            }
            return null;
        }

        public DataTable M_ZipCode_Select(M_ZipCode_Entity mze)
        {
            return mzdl.M_ZipCode_Select(mze);
        }

        public DataTable M_Vendor_ZipCodeSelect(M_Vendor_Entity mve)
        {
            return mvdl.M_Vendor_ZipCodeSelect(mve);
        }

        public DataTable Payee_Select(M_Vendor_Entity mve)
        {
            return mvdl.Payee_Select(mve);
        }

        public DataTable MoneyPayee_Select(M_Vendor_Entity mve)
        {
            return mvdl.MoneyPayee_Select(mve);
        }
        public DataTable Bank_Select(M_Bank_Entity mbe)
        {
            return mbdl.M_Vendor_Bank_Select(mbe);
        }

        public DataTable BankShiten_Select(M_BankShiten_Entity mbse)
        {
            return mbsdl.M_BankShiten_Select(mbse);
        }

        public DataTable Kouza_Select(M_Kouza_Entity mke)
        {
            return mkdl.M_Vendor_Kouza_Select(mke);
        }

        public DataTable Staff_Select(M_Staff_Entity mse)
        {
            return msdl.M_Vendor_Staff_Select(mse);
        }
        
        public bool M_Vendor_InsertUpdate(M_Vendor_Entity mve,int mode)
        {
            return mvdl.M_Vendor_InsertUpdate(mve, mode);
        }

        public bool M_Vendor_Delete(M_Vendor_Entity mve)
        {
            return mvdl.M_Vendor_Delete(mve);
        }

    }
}
