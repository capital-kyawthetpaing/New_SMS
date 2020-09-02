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
    public class ShukkaSiziTourokuFromJuchuu_BL : Base_BL
    {
        D_Instruction_DL ddl;
        public ShukkaSiziTourokuFromJuchuu_BL()
        {
            ddl = new D_Instruction_DL();
        }
        public DataTable D_DeliveryPlan_SelectData(D_DeliveryPlan_Entity de)
        {
            D_DeliveryPlan_DL mdl = new D_DeliveryPlan_DL();
            return mdl.D_DeliveryPlan_SelectData(de);
        }
        /// <summary>
        /// 出荷指示入力更新処理
        /// ShukkaSiziTourokuFromJuchuuより更新時に使用
        /// </summary>
        public bool D_Instruction_Exec(D_Instruction_Entity die, DataTable dt)
        {
            return ddl.D_Instruction_ExecFromJuchu(die, dt);
        }

        /// <summary>
        /// 出荷指示入力取得処理
        /// ShukkaSiziTourokuFromJuchuuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Instruction_SelectDataFromJuchu(D_Instruction_Entity de)
        {
            DataTable dt = ddl.D_Instruction_SelectDataFromJuchu(de);

            return dt;
        }
    }
}
