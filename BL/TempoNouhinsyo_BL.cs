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
    public class TempoNouhinsyo_BL : Base_BL
    {
        D_Sales_DL mdl;
        public TempoNouhinsyo_BL()
        {
            mdl = new D_Sales_DL();
        }

        public DataTable D_Sales_SelectForPrint(D_Sales_Entity mie)
        {
            return mdl.D_Sales_SelectForPrint(mie);
        }

        /// <summary>
        /// 売上入力取得処理
        /// UriageNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Sales_SelectData(D_Sales_Entity dse, short operationMode)
        {
            DataTable dt = mdl.D_Sales_SelectData(dse, operationMode);
         
            return dt;
        }

        /// <summary>
        /// 店舗納品書更新処理
        /// TempoNouhinsyoよりフラグ更新時に使用
        /// </summary>
        public bool D_Sales_Update(D_Sales_Entity dse, DataTable dt, string operatorNm, string pc)
        {
            return mdl.D_Sales_Update(dse, dt, operatorNm, pc);
        }
    }
}
