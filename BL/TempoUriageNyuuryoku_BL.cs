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
    public class TempoUriageNyuuryoku_BL : Base_BL
    {
        D_Juchuu_DL mdl;
        public TempoUriageNyuuryoku_BL()
        {
            mdl = new D_Juchuu_DL();
        }

        /// <summary>
        /// 店舗売上入力更新処理
        /// KaitouNoukiTourokuより更新時に使用
        /// </summary>
        public bool Uriage_Exec(D_Sales_Entity de, DataTable dt, short operationMode)
        {
            TempoShukkaNyuuryoku_DL dl = new TempoShukkaNyuuryoku_DL();
            return dl.PRC_TempoUriageNyuuryoku(de,dt, operationMode);
        }

        /// <summary>
        /// 店舗売上入力取得処理
        /// TempoUriageNyuuryoku よりデータ抽出時に使用
        /// </summary>
        public DataTable D_Juchu_SelectDataForTempoUriage(D_Juchuu_Entity de, short operationMode)
        {
            return mdl.D_Juchu_SelectDataForTempoUriage(de, operationMode);
            
        }

        public string D_Juchu_SelectData(D_Juchuu_Entity de)
        {
            DataTable dt= mdl.D_Juchu_SelectData(de, 2);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["SalesNO"].ToString();
            else
                return "";

        }
    }
}
