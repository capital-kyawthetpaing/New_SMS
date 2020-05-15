using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
    public class MasterTouroku_UnsouGaisya_BL : Base_BL
    {
        M_UnsouGaisya_DL mudl;


        public MasterTouroku_UnsouGaisya_BL()
        {
            mudl = new M_UnsouGaisya_DL();
        }

        //public DataTable CarrierSelect(M_Shipping_Entity mse)
        //{
        //    return mudl.M_CarrierSelect(mse);
        //}

        public M_Shipping_Entity M_Carrier_Select(M_Shipping_Entity mse)
        {
            DataTable dtCarrier = mudl.M_CarrierSelect(mse);
            if (dtCarrier.Rows.Count > 0)
            {
                mse.ShippingName = dtCarrier.Rows[0]["CarrierName"].ToString();
                mse.CarrierFlg = dtCarrier.Rows[0]["CarrierFlg"].ToString();
                mse.NormalFlg = dtCarrier.Rows[0]["NormalFLG"].ToString();
                mse.YahooCD = dtCarrier.Rows[0]["YahooCarrierCD"].ToString();
                mse.RakutenCD = dtCarrier.Rows[0]["RakutenCarrierCD"].ToString();
                mse.AmazonCD = dtCarrier.Rows[0]["AmazonCarrierCD"].ToString();
                mse.WowmaCD = dtCarrier.Rows[0]["WowmaCarrierCD"].ToString();
                mse.PonpareCD = dtCarrier.Rows[0]["PonpareCarrierCD"].ToString();
                mse.OtherCD = dtCarrier.Rows[0]["OtherCD"].ToString();
                mse.Remarks = dtCarrier.Rows[0]["Remarks"].ToString();
                mse.DeleteFlg = dtCarrier.Rows[0]["DeleteFlg"].ToString();
                return mse;
            }
            return null;
        }
        //public DataTable M_Carrier_Select(M_Shipping_Entity mse)
        //{
        //    return mudl.M_CarrierSelect(mse);
        //}

        public bool M_Gaisya_InsertUpdate(M_Shipping_Entity mse, int mode)
        {
            return mudl.M_Gaisya_InsertUpdate(mse, mode);
        }

        public bool M_Gaisya_Delect(M_Shipping_Entity mse)
        {
            return mudl.M_Gaisya_Delete(mse);
        }

        //public DataTable M_Carrirer_Bind(M_Carrier_Entity mce)
        //{
        //    return mudl.M_Carrirer_Bind(mce);
        //}
    }
}
