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
    public class ZaikoIdouIraiNyuuryoku_BL : Base_BL
    {
        D_MoveRequest_DL ddl;

        public ZaikoIdouIraiNyuuryoku_BL()
        {
            ddl = new D_MoveRequest_DL();
        }

        public bool M_MovePurpose_Select(M_MovePurpose_Entity me)
        {
            M_MovePurpose_DL mmdl = new M_MovePurpose_DL();
            DataTable dt = mmdl.M_MovePurpose_Select(me);

            if(dt.Rows.Count==0)
            {
                return false;
            }
            else
            {
                me.MovePurposeName = dt.Rows[0]["MovePurposeName"].ToString();
                me.DisplayOrder = dt.Rows[0]["DisplayOrder"].ToString();
                me.MovePurposeType = dt.Rows[0]["MovePurposeType"].ToString();
                me.MoveRequestFLG = dt.Rows[0]["MoveRequestFLG"].ToString();
                me.MoveFLG = dt.Rows[0]["MoveFLG"].ToString();
                me.ToSoukoFLG = dt.Rows[0]["ToSoukoFLG"].ToString();    
                me.MailFLG = dt.Rows[0]["MailFLG"].ToString();
            }
            return true;
        }
        public DataTable M_Souko_SelectData(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_SelectData(mse);
        }

        /// <summary>
        /// 移動依頼入力更新処理
        /// ZaikoIdouIraiNyuuryokuより更新時に使用
        /// </summary>
        public bool D_MoveRequest_Exec(D_MoveRequest_Entity dme, DataTable dt, short operationMode)
        {
            return ddl.D_MoveRequest_Exec(dme, dt, operationMode);
        }

        /// <summary>
        /// 移動入力取得処理
        /// ZaikoIdouNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_MoveRequest_SelectDataForIdouIrai(D_MoveRequest_Entity de)
        {
            DataTable dt = ddl.D_MoveRequest_SelectDataForIdouIrai(de);

            return dt;
        }    
    }
}
