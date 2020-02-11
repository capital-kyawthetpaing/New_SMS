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
    public class MiNyuukinKakuninShoukai_BL : Base_BL
    {
        D_CollectPlan_DL mdl;
        public MiNyuukinKakuninShoukai_BL()
        {
            mdl = new D_CollectPlan_DL();
        }

        /// <summary>
        /// 未入金確認照会にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_CollectPlan_SelectAllForSyokai(D_CollectPlan_Entity de)
        {
            return mdl.D_CollectPlan_SelectAllForSyokai(de);
        }
        /// <summary>
        /// 未入金確認照会にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_CollectPlan_SelectAllForSyosai(D_CollectPlan_Entity de)
        {
            return mdl.D_CollectPlan_SelectAllForSyosai(de);
        }
    }
}
