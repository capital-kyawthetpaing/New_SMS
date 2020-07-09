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
    public class ZaikoIdouIraiUkeNyuuryoku_BL : Base_BL
    {
        D_MoveRequest_DL ddl;

        public ZaikoIdouIraiUkeNyuuryoku_BL()
        {
            ddl = new D_MoveRequest_DL();
        }

        /// <summary>
        /// 在庫移動依頼受け一覧データ取得処理
        /// ZaikoIdouIraiUkeNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_MoveRequest_SelectAllForShoukai(D_MoveRequest_Entity de)
        {
            DataTable dt = ddl.D_MoveRequest_SelectAllForShoukai(de);

            return dt;
        }
    }
}
