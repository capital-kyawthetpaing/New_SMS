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
    public class NyuukaNyuuryoku_BL : Base_BL
    {
        D_Arrival_DL dadl;
        public NyuukaNyuuryoku_BL()
        {
            dadl = new D_Arrival_DL();
        }

        /// <summary>
        /// 入荷番号検索にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Arrival_SelectAll(D_Arrival_Entity de)
        {
            return dadl.D_Arrival_SelectAll(de);
        }

        /// <summary>
        /// 入荷入力更新処理
        /// NyuukaNyuuryokuより更新時に使用
        /// </summary>
        public bool Arrival_Exec(D_Arrival_Entity dae, DataTable dt, short operationMode)
        {
            return dadl.D_Arrival_Exec(dae, dt, operationMode);
        }

        /// <summary>
        /// 入荷入力取得処理
        /// NyuukaNyuuryokuよりデータ抽出時に使用
        /// 新規時以外
        /// </summary>
        public DataTable D_Arrival_SelectData(D_Arrival_Entity de, short operationMode)
        {
            DataTable dt = dadl.D_Arrival_SelectData(de, operationMode);

            return dt;
        }
        /// <summary>
        /// 入荷入力取得処理
        /// NyuukaNyuuryokuよりデータ抽出時に使用
        /// 新規時
        /// </summary>
        public DataTable D_ArrivalPlan_SelectData(D_ArrivalPlan_Entity de, short operationMode)
        {
            DataTable dt = dadl.D_ArrivalPlan_SelectData(de, operationMode);

            return dt;
        }
        /// <summary>
        /// 進捗チェック　
        /// 既に出荷済み,出荷指示済み,ピッキングリスト完了済み警告
        /// </summary>
        /// <param name="hacchuNo"></param>
        /// <param name="errno"></param>
        /// <returns></returns>
        public bool CheckNyuukaData(D_Arrival_Entity de, out string errno)
        {
            DataTable dt = dadl.CheckNyuukaData(de);

            bool ret = false;
            errno = "";

            if (dt.Rows.Count>0)
            {
                errno = dt.Rows[0]["errno"].ToString();
            }
            
            return ret;
        }

        public DataTable M_Souko_SelectForNyuuka(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_SelectForNyuuka(mse);
        }
        public DataTable M_Souko_BindForNyuuka(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_BindForNyuuka(mse);
        }
        public DataTable D_Juchuu_SelectData_ForNyuuka(D_Juchuu_Entity de)
        {
            D_Juchuu_DL mdl = new D_Juchuu_DL();
            return mdl.D_Juchuu_SelectData_ForNyuuka(de);
        }
        /// <summary>
        /// 入荷入力より入荷予定処理
        /// NyuukaNyuuryokuより在庫データ複製時に使用
        /// </summary>
        public DataTable Order_Exec(D_Order_Entity de)
        {
            D_Hacchu_DL dl = new D_Hacchu_DL();
            bool ret = dl.D_Order_ExecForNyuka(de);

            DataTable dt = new DataTable();
            D_Arrival_DL adl = new D_Arrival_DL();
            if (ret)
            {
                dt = adl.D_ArrivalPlan_SelectDataByOrderNO(de);
            }

            return dt;
        }
        /// <summary>
        /// 入荷入力更新処理
        /// NyuukaNyuuryokuより更新時に使用
        /// </summary>
        public bool D_Order_Delete(D_Arrival_Entity dae, DataTable dt, short operationMode)
        {
            return dadl.D_Order_Delete(dae, dt, operationMode);
        }
    }
}
