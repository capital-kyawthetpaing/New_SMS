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
    public class Exclusive_BL : Base_BL
    {
        //データ区分
        public enum DataKbn : int
        {
            Null,
            Mitsumori,//１：見積、
            Jyuchu,//２：受注、
            Uriage,//３：売上
            Seikyu,//４：請求、
            Nyukin,//５：入金、
            NyukinKeshikomk,//６：入金消込
            Hacchu,//７：発注、
            Shiire,//８：仕入、
            Shiharai,//９：支払
            Zaiko,//10：在庫、
            Zaikoido,//11：在庫移動、
            Keihi//12：経費
            , Nyuka//13:入荷
            , Kessai//14：決済取込
            , SyukkaShiji//15：出荷指示
            , Syukka//16：出荷
            , TenjiNo = 32//32 TenjiCD 
        }

        D_Exclusive_DL mdl;
        public Exclusive_BL()
        {
            mdl = new D_Exclusive_DL();
        }

        public DataTable D_Exclusive_Select(D_Exclusive_Entity dee)
        {
            DataTable dt = mdl.D_Exclusive_Select(dee);
            return dt;
        }

        public bool D_Exclusive_Insert(D_Exclusive_Entity dee)
        {
            return mdl.D_Exclusive_Insert(dee);
        }
        public bool D_Exclusive_Delete(D_Exclusive_Entity dee)
        {
            return mdl.D_Exclusive_Delete(dee);
        }

        public bool D_Exclusive_DeleteByKBN(D_Exclusive_Entity dee)
        {
            return mdl.D_Exclusive_DeleteByKBN(dee);
        }
        /// <summary>
        /// </summary>
        //public bool ItemPrice_Exec(D_Exclusive_Entity dee, DataTable dt, short operationMode, string operatorNm, string pc)
        //{
        //    return mdl.D_Exclusive_Exec(dee, dt, operationMode, operatorNm, pc);
        //}

        /// <summary>
        /// </summary>
        //public DataTable D_Exclusive_SelectData(D_Exclusive_Entity dme, short operationMode)
        //{
        //    DataTable dt = mdl.D_Exclusive_SelectData(dme, operationMode);
        //    if (dt.Rows.Count > 0)
        //    {
        //        //dme.ExclusiveNO = dt.Rows[0]["ExclusiveNO"].ToString();

        //        //dme.DeliveryPlace = dt.Rows[0]["DeliveryPlace"].ToString();


        //                    }

        //    return dt;
        //}
    }
}
