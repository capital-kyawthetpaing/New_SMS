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
    public class MasterTouroku_Souko_BL : Base_BL
    {
        M_Souko_DL msdl;
        M_ZipCode_DL mzdl;
        M_Location_DL mldl;
        public MasterTouroku_Souko_BL()
        {
            msdl = new M_Souko_DL();
            mzdl = new M_ZipCode_DL();
            mldl = new M_Location_DL();
        }

        public M_Souko_Entity M_Souko_Select(M_Souko_Entity mse)
        {
            DataTable dtSouko = msdl.M_Souko_Select(mse);
            if(dtSouko.Rows.Count > 0)
            {
                mse.SoukoName = dtSouko.Rows[0]["SoukoName"].ToString();
                mse.StoreCD = dtSouko.Rows[0]["StoreCD"].ToString();
                mse.ZipCD1 = dtSouko.Rows[0]["ZipCD1"].ToString();
                mse.ZipCD2 = dtSouko.Rows[0]["ZipCD2"].ToString();
                mse.Address1 = dtSouko.Rows[0]["Address1"].ToString();
                mse.Address2 = dtSouko.Rows[0]["Address2"].ToString();
                mse.TelephoneNO = dtSouko.Rows[0]["TelephoneNo"].ToString();
                mse.FaxNO = dtSouko.Rows[0]["FaxNO"].ToString();
                mse.SoukoType = dtSouko.Rows[0]["SoukoType"].ToString();
                mse.MakerCD = dtSouko.Rows[0]["MakerCD"].ToString();
                mse.MakerName = dtSouko.Rows[0]["MakerName"].ToString();
                mse.HikiateOrder = dtSouko.Rows[0]["HikiateOrder"].ToString();
                mse.UnitPriceCalcKBN = dtSouko.Rows[0]["UnitPriceCalcKBN"].ToString();
                mse.IdouCount = dtSouko.Rows[0]["IdouCount"].ToString();
                mse.Remarks = dtSouko.Rows[0]["Remarks"].ToString();
                mse.DeleteFlg = dtSouko.Rows[0]["DeleteFlg"].ToString();
                return mse;
            }
            return null;
        }

        public bool M_ZipCode_Select(M_ZipCode_Entity mze)
        {            
            if (mzdl.M_ZipCode_Select(mze).Rows.Count > 0)
                return true;
            return false;
        }

        public DataTable M_ZipCode_AddressSelect(M_ZipCode_Entity mze)
        {
            return mzdl.M_ZipCode_Select(mze);
        }
        public DataTable M_Souko_ZipcodeAddressSelect(M_Souko_Entity mse)
        {
            return msdl.M_Souko_ZipcodeAddressSelect(mse);
        }
      


        public M_Vendor_Entity M_Vendor_IsExists(M_Vendor_Entity mve)
        {
            M_Vendor_DL mvdl = new M_Vendor_DL();
            DataTable dtVendor = mvdl.M_Vendor_IsExists(mve);
            if (dtVendor.Rows.Count <= 0)
                return null;
            else
            {
                mve.VendorName = dtVendor.Rows[0]["VendorName"].ToString();
                return mve;
            }
        }

        public bool M_Souko_Insert_Update(M_Souko_Entity mse,int mode)
        {
            mse.LocationXml = DataTableToXml(mse.dtTemp1);
            return msdl.M_Souko_Insert_Update(mse,mode);
        }

        public bool M_Souko_Delete(M_Souko_Entity mse)
        {
            return msdl.M_Souko_Delete(mse);
        }
        public DataTable M_Location_Select(M_Souko_Entity mse)
        {
            return mldl.M_Location_Select(mse);
        }

    }
}
