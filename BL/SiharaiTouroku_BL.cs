using System.Data;
using DL;
using Entity;

namespace BL
{
    public class SiharaiTouroku_BL : Base_BL
    {
        D_Pay_DL dpdl ;
        M_Staff_DL msdl;
        M_Kouza_DL mkdl;
        M_Vendor_DL mvdl ;
        M_Payee_DL mpdl;
        D_PayDetail_DL dpddl;
        D_PayPlan_DL dppdl;
        M_MultiPorpose_DL mmdl;
        M_Kouza_DL mkzdl;

        public SiharaiTouroku_BL()
        {
            dpdl = new D_Pay_DL();
            msdl = new M_Staff_DL();
            mkdl = new M_Kouza_DL();
            mvdl = new M_Vendor_DL();
            mpdl = new M_Payee_DL();
            dpddl = new D_PayDetail_DL();
            dppdl = new D_PayPlan_DL();
            mmdl = new M_MultiPorpose_DL();
            mkzdl = new M_Kouza_DL();
        }

        public DataTable M_Staff_Select(M_Staff_Entity mse)
        {
            return msdl.M_Staff_Select(mse);
        }

        public DataTable M_Vendor_Select(M_Vendor_Entity mve)
        {
            return mvdl.M_Vendor_SelectForSiharaiNyuuroku(mve);
        }

        public DataTable D_Pay_Select01(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_Select01(dpe);
        }

        public DataTable D_Pay_Select02(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_Select02(dpe);
        }

        public DataTable D_PayPlan_Select(D_PayPlan_Entity dppe)
        {
            return dppdl.D_PayPlan_Select(dppe);
        }
        public DataTable D_PayPlan_SelectDetail(D_PayPlan_Entity dppe)
        {
            return dppdl.D_PayPlan_SelectDetail(dppe);
        }

        public DataTable M_Multipurpose_SelectIDName(string ID)
        {
            return mmdl.M_Multipurpose_SelectIDName(ID);
        }

        public DataTable M_Kouza_FeeSelect(M_Kouza_Entity mkze)
        {
            return mkzdl.M_Kouza_FeeSelect(mkze);
        }

        public bool D_Siharai_Exec(D_Pay_Entity dpe, DataTable dt, DataTable dtD, short operationMode)
        {
            return dpdl.D_Siharai_Exec(dpe, dt, dtD, operationMode);
        }
    }
}
