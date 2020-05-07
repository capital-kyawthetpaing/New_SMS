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
    public class HikiateHenkouNyuuryoku_BL : Base_BL
    {
        HikiateHenkouNyuuryoku_DL hhdl;

        public HikiateHenkouNyuuryoku_BL()
        {
            hhdl = new HikiateHenkouNyuuryoku_DL();
        }
        
        /// <summary>
        /// 発注取得処理
        /// 発注番号チェック時に使用
        /// </summary>
        public DataTable D_Order_Select(string orderNo)
        {
            D_EdiHacchuu_DL edl = new D_EdiHacchuu_DL();
            DataTable dt = edl.D_Order_SelectForEDIHacchuu(orderNo);

            return dt;
        }
        /// <summary>
        /// 受注取得処理
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Juchuu_DataSelect_ForShukkaShoukai(D_Juchuu_Entity de)
        {
            D_Juchuu_DL mdl = new D_Juchuu_DL();
            return mdl.D_Juchuu_DataSelect_ForShukkaShoukai(de);
        }
    

        /// <summary>
        /// ワークテーブル用データ取得
        /// </summary>
        /// <param name="hhe"></param>
        /// <returns></returns>
        public DataTable D_Stock_SelectForHikiateZaiko(HikiateHenkouNyuuryoku_Entity hhe)
        {
            return hhdl.D_Stock_SelectForHikiateZaiko(hhe);
        }

        public DataTable D_Stock_SelectForHikiateJuchuu(HikiateHenkouNyuuryoku_Entity hhe)
        {
            return hhdl.D_Stock_SelectForHikiateJuchuu(hhe);
        }

        /// <summary>
        /// 引当更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool PRC_HikiateHenkouNyuuryoku(HikiateHenkouNyuuryoku_Entity hhe, DataTable zdt, DataTable jdt)
        {
            return hhdl.PRC_HikiateHenkouNyuuryoku(hhe, zdt, jdt);
        }

        public bool ALL_Hikiate(HikiateHenkouNyuuryoku_Entity hhe)
        {
            return hhdl.ALL_Hikiate(hhe);
        }
    }
}
