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
    public class SeikyuuShimeShori_BL : Base_BL
    {
        D_BillingProcessing_DL ddl;
        public SeikyuuShimeShori_BL()
        {
            ddl = new D_BillingProcessing_DL();
        }

        public DataTable D_BillingProcessing_SelectAll(D_BillingProcessing_Entity mie)
        {
            return ddl.D_BillingProcessing_SelectAll(mie);
        }

        /// <summary>
        /// 請求締処理前のチェック処理
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        public bool D_CollectPlan_Check(D_CollectPlan_Entity dce)
        {
            D_CollectPlan_DL dcl = new D_CollectPlan_DL();

            DataTable dt = dcl.D_CollectPlan_Check(dce);

            if (dt.Rows.Count > 0)
            {
                string messageID = dt.Rows[0]["MessageID"].ToString();
                dce.MessageID = messageID;

                if (string.IsNullOrWhiteSpace(messageID))
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 請求締更新処理
        /// SeikyuuShimeShoriより更新時に使用
        /// </summary>
        public bool D_BillingProcessing_Exec(D_CollectPlan_Entity dme, string operatorNm, string pc)
        {
            return ddl.D_BillingProcessing_Exec(dme, operatorNm, pc);
        }
    }
}
