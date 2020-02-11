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
    public class ItemPrice_BL : Base_BL
    {
        M_ItemPrice_DL mdl;
        public ItemPrice_BL()
        {
            mdl = new M_ItemPrice_DL();
        }

        public DataTable M_ItemPrice_Select(M_ItemPrice_Entity mie)
        {
            return mdl.M_ItemPrice_Select(mie);
        }

        //public DataTable M_Store_SelectAll(M_Store_Entity mse)
        //{
        //    return mdl.M_Store_SelectAll(mse);
        //}

        /// <summary>
        /// ITEM販売単価マスタ更新処理
        /// MasterTouroku_HanbaiTankaより更新時に使用
        /// </summary>
        public bool ItemPrice_Exec(M_ItemPrice_Entity mse, DataTable dt, short operationMode, string operatorNm, string pc)
        {
            return mdl.M_ItemPrice_Exec(mse, dt, operationMode, operatorNm, pc);
        }

        /// <summary>
        /// ITEM販売単価マスタ取得処理
        /// MasterTouroku_HanbaiTankaよりデータ抽出時に使用
        /// </summary>
        public DataTable ItemPrice_SelectData(M_ItemPrice_Entity mie, short operationMode)
        {
            return mdl.M_ItemPrice_SelectData(mie, operationMode);
        }
}
}
