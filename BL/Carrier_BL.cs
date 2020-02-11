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
    public class Carrier_BL : Base_BL
    {
        M_Carrier_DL mdl;
        public Carrier_BL()
        {
            mdl = new M_Carrier_DL();
        }

        //public bool M_Carrier_Select(M_Carrier_Entity mce)
        //{
        //    DataTable dt = mdl.M_Carrier_Select(mce);
        //    if (dt.Rows.Count > 0)
        //    {
        //        mce.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();
        //        mce.CarrierCD = dt.Rows[0]["CarrierCD"].ToString();
        //        mce.CarrierName = dt.Rows[0]["CarrierName"].ToString();
        //        mce.CarrierFlg = dt.Rows[0]["CarrierFlg"].ToString();
        //        mce.YahooCarrierCD = dt.Rows[0]["YahooCarrierCD"].ToString();
        //        mce.RakutenCarrierCD = dt.Rows[0]["RakutenCarrierCD"].ToString();
        //        mce.AmazonCarrierCD = dt.Rows[0]["AmazonCarrierCD"].ToString();
        //        mce.WowmaCarrierCD = dt.Rows[0]["WowmaCarrierCD"].ToString();
        //        mce.PonpareCarrierCD = dt.Rows[0]["PonpareCarrierCD"].ToString();

        //        mce.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
        //        mce.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();
        //        mce.InsertOperator = dt.Rows[0]["InsertOperator"].ToString();
        //        mce.InsertDateTime = dt.Rows[0]["InsertDateTime"].ToString();
        //        mce.UpdateOperator = dt.Rows[0]["UpdateOperator"].ToString();
        //        mce.UpdateDateTime = dt.Rows[0]["UpdateDateTime"].ToString();

        //        return true;
        //    }
        //    else
        //        return false;

        //}
        public DataTable M_Carrier_Bind(M_Carrier_Entity mce)
        {
            return mdl.M_Carrier_Bind(mce);
        }
    }
}
