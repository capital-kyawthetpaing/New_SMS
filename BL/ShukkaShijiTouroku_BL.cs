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
    public class ShukkaShijiTouroku_BL : Base_BL
    {
        D_Instruction_DL ddl;
        public ShukkaShijiTouroku_BL()
        {
            ddl = new D_Instruction_DL();
        }

        /// <summary>
        /// 仕入番号検索にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Instruction_SelectAll(D_Instruction_Entity de)
        {
            return ddl.D_Instruction_SelectAll(de);
        }
        public DataTable M_Souko_Search(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_Search(mse);
        }
        public DataTable M_Souko_BindForShukka(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_BindForShukka(mse);
        }
        public DataTable D_DeliveryPlan_SelectData(D_DeliveryPlan_Entity de)
        {
            D_DeliveryPlan_DL mdl = new D_DeliveryPlan_DL();
            return mdl.D_DeliveryPlan_SelectData(de);
        }
        /// <summary>
        /// 出荷指示入力更新処理
        /// ShukkaShijiTourokuより更新時に使用
        /// </summary>
        public bool D_Instruction_Exec(D_Instruction_Entity die, DataTable dt)
        {
            return ddl.D_Instruction_Exec(die, dt);
        }

        /// <summary>
        /// 出荷指示入力取得処理
        /// ShukkaShijiTourokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Instruction_SelectData(D_Instruction_Entity de)
        {
            DataTable dt = ddl.D_Instruction_SelectData(de);

            return dt;
        }

        /// <summary>
        /// 出荷指示入力取得処理(入荷から)
        /// ShiireNyuuryokuFromNyuukaよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Delivery_SelectAll(D_Arrival_Entity de)
        {
            D_Arrival_DL dadl = new D_Arrival_DL();
            DataTable dt = dadl.D_Delivery_SelectAll(de);

            return dt;
        }
        /// <summary>
        /// 出荷指示書より出荷指示データの存在チェック
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool D_Instruction_Select(D_Instruction_Entity de)
        {
            DataTable dt = ddl.D_Instruction_Select(de);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 出荷指示書の印刷データ取得処理
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Instruction_SelectForPrint(D_Instruction_Entity de)
        {
            return ddl.D_Instruction_SelectForPrint(de);
        }

        /// <summary>
        /// 出荷指示書発行済みFLGの更新処理
        /// </summary>
        /// <param name="de"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool D_Instruction_Update(D_Instruction_Entity de, DataTable dt)
        {
            return ddl.D_Instruction_Update(de, dt);
        }
    }
}
