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
    public class SKUPrice_BL : Base_BL
    {
        M_SKUPrice_DL mdl;
        public SKUPrice_BL()
        {
            mdl = new M_SKUPrice_DL();
        }

        public DataTable M_SKUPrice_Select(M_SKUPrice_Entity mie)
        {
            return mdl.M_SKUPrice_Select(mie);
        }
        public bool M_SKUPrice_Insert_Update(M_SKUPrice_Entity mse,string Xml,int mode)
        {
            return true;
          //  return mdl.M_SKUPrice_Insert_Update(mse,Xml,mode); // PTK_M
        }
        //public bool M_SKUPrice_Update(M_SKUPrice_Entity mse, string updateXml)
        //{
        //    return mdl.M_SKUPrice_Update(mse, updateXml);
        //}
       
        //ses
        public DataTable M_SKUPrice_HanbaiTankaTennic_Select(M_SKUPrice_Entity mse,M_SKU_Entity ms, short operationMode)
        {
            return mdl.M_SKUPrice_HnabaiTankaTennic_Select(mse, ms,operationMode);
        }
        public DataTable M_SKUPrice_DataSelect(M_SKUPrice_Entity mse)
        {
            return mdl.M_SKUPrice_DataSelect(mse);
        }
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public bool M_SKUPrice_SelectTanka(M_SKUPrice_Entity me)
        {
            DataTable dt = mdl.M_SKUPrice_SelectTanka(me);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                me.GeneralPriceOutTax = dt.Rows[0]["GeneralPriceOutTax"].ToString();
            }
            return true;
        }
        //public DataTable M_Store_SelectAll(M_Store_Entity mse)
        //{
        //    return mdl.M_Store_SelectAll(mse);
        //}

        /// <summary>
        /// SKU販売単価マスタ更新処理
        /// MasterTouroku_HanbaiTankaより更新時に使用
        /// </summary>
        public bool SKUPrice_Exec(M_SKUPrice_Entity mse, DataTable dt, short operationMode, string operatorNm, string pc)
        {
            return mdl.M_SKUPrice_Exec(mse, dt, operationMode, operatorNm, pc);
        }

        /// <summary>
        /// SKU販売単価マスタ取得処理
        /// MasterTouroku_HanbaiTankaよりデータ抽出時に使用
        /// </summary>
        public DataTable SKUPrice_SelectData(M_SKUPrice_Entity mse, short operationMode)
        {
            return mdl.M_SKUPrice_SelectData(mse,  operationMode);
        }
}
}
