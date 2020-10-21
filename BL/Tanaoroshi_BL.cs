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
    public class Tanaoroshi_BL : Base_BL
    {
        D_Inventory_DL mdl;
        public Tanaoroshi_BL()
        {
            mdl = new D_Inventory_DL();
        }
        public DataTable M_Souko_BindForTanaoroshi(M_Souko_Entity me)
        {
            M_Souko_DL dl = new M_Souko_DL();
            return dl.M_Souko_BindForTanaoroshi(me);
        }
        /// <summary>
        /// 棚卸入力にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Inventory_SelectAll(D_Inventory_Entity de)
        {
            return mdl.D_Inventory_SelectAll(de);
        }
        /// <summary>
        /// 棚卸入力にて更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool D_Inventory_Update(D_Inventory_Entity de, DataTable dt)
        {
            return mdl.D_Inventory_Update(de, dt);
        }
        /// <summary>
        /// 棚卸入力にて使用
        /// </summary>
        public bool M_Location_SelectData(M_Location_Entity me)
        {
            M_Location_DL mldl = new M_Location_DL();
            DataTable dt = mldl.M_Location_SelectData(me);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 棚卸入力にて使用
        /// </summary>
        public DataTable M_Souko_SelectData(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_SelectData(mse);
        }
        /// <summary>
        /// 棚卸締処理にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_InventoryProcessing_SelectAll(D_InventoryProcessing_Entity de)
        {
            D_InventoryProcessing_DL dl = new D_InventoryProcessing_DL();
            return dl.D_InventoryProcessing_SelectAll(de);
        }
        /// <summary>
        /// 棚卸締処理チェック処理
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public bool D_InventoryControl_Select(D_InventoryProcessing_Entity de)
        {
            D_InventoryControl_DL dl = new D_InventoryControl_DL();
            DataTable dt = dl.D_InventoryControl_Select(de);

            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public bool M_Location_SelectForTanaoroshi(D_InventoryProcessing_Entity de)
        {
            M_Location_DL dl = new M_Location_DL();
            DataTable dt = dl.M_Location_SelectForTanaoroshi(de);

            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 棚卸締処理チェック処理
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public bool D_InventoryProcessing_Select(D_InventoryProcessing_Entity de)
        {
            D_InventoryProcessing_DL dl = new D_InventoryProcessing_DL();
            DataTable dt = dl.D_InventoryProcessing_Select(de);

            if (dt.Rows.Count > 0)
            {
                de.InventoryKBN = dt.Rows[0]["InventoryKBN"].ToString();
                return true;

            }
            else
                return false;
        }
        /// <summary>
        /// 棚卸締処理
        /// TanaoroshiShimeShoriより更新時に使用
        /// </summary>
        /// <param name = "de" ></ param >
        /// <returns></returns>
        public bool D_InventoryProcessing_Exec(D_InventoryProcessing_Entity de)
        {
            D_InventoryProcessing_DL dl = new D_InventoryProcessing_DL();
            return dl.D_InventoryProcessing_Exec(de);
        }
        /// <summary>
        /// 棚卸差異表データ取得処理
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Inventory_SelectForPrint(D_Inventory_Entity de)
        {
            return mdl.D_Inventory_SelectForPrint(de);
        }
    }
}
