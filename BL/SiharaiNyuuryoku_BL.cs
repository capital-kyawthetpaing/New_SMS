using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DL;
using Entity;

namespace BL
{
    public class SiharaiNyuuryoku_BL : Base_BL
    {
        D_Pay_DL dpdl ;
        M_Control_DL mcdl;
        M_Calendar_DL mcaldl;
        M_Staff_DL msdl;
        M_MultiPorpose_DL mmdl;
        M_Kouza_DL mkdl;
        M_Vendor_DL mvdl ;
        M_Payee_DL mpdl;
        D_PayDetail_DL dpddl;
        D_PayPlan_DL dppdl;
        M_StoreClose_DL mscdl;
        M_Kouza_DL mkzdl;

        public SiharaiNyuuryoku_BL()
        {
            dpdl = new D_Pay_DL();
            mcdl = new M_Control_DL();
            mcaldl = new M_Calendar_DL();
            msdl = new M_Staff_DL();
            mmdl = new M_MultiPorpose_DL();
            mkdl = new M_Kouza_DL();
            mvdl = new M_Vendor_DL();
            mpdl = new M_Payee_DL();
            dpddl = new D_PayDetail_DL();
            dppdl = new D_PayPlan_DL();
            mscdl = new M_StoreClose_DL();
            mkzdl = new M_Kouza_DL();
        }


        public DataTable D_Pay_LargePayNoSelect(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_LargePayNoSelect(dpe);
        }

        public DataTable D_Pay_PayNoSelect(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_PayNoSelect(dpe);
        }

        public DataTable M_Control_PaymentSelect(M_FiscalYear_Entity mfye)
        {
            return mcdl.M_Control_PaymentSelect(mfye);
        }

        public DataTable M_Calendar_SelectCalencarDate(M_Calendar_Entity mce)
        {
            return mcaldl.M_Calendar_SelectCalencarDate(mce);
        }

        public DataTable M_Staff_Select(M_Staff_Entity mse)
        {
            return msdl.M_Staff_Select(mse);
        }

        public DataTable M_MultiPorpose_Select(M_MultiPorpose_Entity mmpe)
        {
            return mmdl.M_MultiPorpose_SelectAll(mmpe);
        }

        public DataTable M_Kouza_SelectByDate(M_Kouza_Entity mke)
        {
            return mkdl.M_Kouza_SelectByDate(mke);
        }

        public DataTable M_Vendor_Select(M_Vendor_Entity mve)
        {
            return mvdl.M_Vendor_SelectForSiharaiNyuuroku(mve);
        }

        public DataTable M_Payee_Select(D_Pay_Entity dpe)
        {
            return mpdl.D_Payee_PayeeNameSelect(dpe);
        }

        public DataTable D_Pay_Select1(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_Select1(dpe);
        }

        public DataTable D_Pay_Select2(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_Select2(dpe);
        }

        public DataTable D_Pay_Select3(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_Select3(dpe);
        }

        public DataTable D_PayDetail_Select(D_Pay_Entity dpe)
        {
            return dpddl.D_PayDetail_Select(dpe);
        }

        public DataTable D_Pay_SelectForPayPlanDate1(D_PayPlan_Entity dppe)
        {
            return dppdl.D_Pay_SelectForPayPlanDate1(dppe);
        }
        public DataTable D_Pay_SelectForPayPlanDate2(D_PayPlan_Entity dppe)
        {
            return dppdl.D_Pay_SelectForPayPlanDate2(dppe);
        }

        public DataTable M_MultiPurpose_AccountSelect(M_MultiPorpose_Entity mme)
        {
            return mmdl.M_MultiPorpose_Select(mme);
        }
        public DataTable M_MultiPorpose_AuxiliarySelect(M_MultiPorpose_Entity mme)
        {
            return mmdl.M_MultiPorpose_AuxiliarySelect(mme);
        }

        public DataTable CheckClosePosition(M_StoreClose_Entity msce)
        {
            return mscdl.M_StoreClose_SelectAll(msce);
        }

        public DataTable M_Multipurpose_SelectIDName(string ID)
        {
            return mmdl.M_Multipurpose_SelectIDName(ID);
        }

        public DataTable M_Kouza_FeeSelect(M_Kouza_Entity mkze)
        {
            return mkzdl.M_Kouza_FeeSelect(mkze);
        }

        public bool D_Pay_Insert(D_Pay_Entity dpe)
        {
            dpe.xml1 = DataTableToXml(dpe.dtTemp1);
            dpe.xml2 = DataTableToXml(dpe.dtTemp2);
            dpe.xml3 = DataTableToXml(dpe.dtTemp3);
            return dpdl.D_Pay_Insert(dpe);
        }

        public bool D_Pay_Update(D_Pay_Entity dpe)
        {           
            dpe.xml4 = DataTableToXml(dpe.dtTemp4);
            dpe.xml5 = DataTableToXml(dpe.dtTemp5);
            dpe.xml6 = DataTableToXml(dpe.dtTemp6);
            return dpdl.D_Pay_Update(dpe);
        }

        public bool D_Pay_Delete(D_Pay_Entity dpe)
        {
            //dpe.xml1 = DataTableToXml(dpe.dtTemp4);
            //dpe.xml2 = DataTableToXml(dpe.dtTemp5);
            //dpe.xml3 = DataTableToXml(dpe.dtTemp6);
            return dpdl.D_Pay_Delete(dpe);
        }

    }
}
