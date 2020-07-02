using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using BL;
using DL;
using System.Data;

namespace BL
{
   public class MasterToroku_Ginkou_Bl : Base_BL
    {
        M_Bank_DL mgdl;

        public bool IsuseFlag(string code, string changedate)
        {
            DataTable dtGinkou = SimpleSelect1("3", changedate, code );
            return dtGinkou.Rows.Count > 0  && dtGinkou.Rows[0]["UsedFlg"].ToString() == "1" ? true : false;
        }
        public bool M_Ginkou_Exist(string code,string changedate)
        {
            DataTable dtGinkou = SimpleSelect1("3",changedate, code);
            return dtGinkou.Rows.Count > 0 ? true : false;
        }
        public bool IsGinkoExistInShiten(string code, string changedate)
        {
            DataTable dtGinkou = SimpleSelect1("6", changedate, code);
            return dtGinkou.Rows.Count > 0 ? true : false;
        }
        public M_Ginkou_Entity M_Ginkou_Entity_select(M_Ginkou_Entity mge)
        {
            DataTable dtginKou = mgdl.M_Ginkou_Select(mge);
            if (dtginKou.Rows.Count > 0)
            {
                mge.ginko_Name = dtginKou.Rows[0]["BankName"].ToString();
                mge.ginko_kananame = dtginKou.Rows[0]["BankKana"].ToString();
                mge.ginko_remarks = dtginKou.Rows[0]["Remarks"].ToString();
                mge.ginko_DeleteFlag = dtginKou.Rows[0]["DeleteFlg"].ToString();
                mge.ginko_useflag = dtginKou.Rows[0]["UsedFlg"].ToString();
                return mge;
            }
            return null;
        }

        public MasterToroku_Ginkou_Bl()
        {
            mgdl = new M_Bank_DL();
        }

        public bool insert_update_mginko(M_Ginkou_Entity mge, int mode)
        {
            return mgdl.M_Ginkou_Insert_Update(mge, mode);
        }
        public bool M_Ginkou_Delete(M_Ginkou_Entity mge)
        {
            return mgdl.M_Ginkou_Delete(mge);
        }

    }
}
