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
    public class ZaikoIdouNyuuryoku_BL : Base_BL
    {
        D_Move_DL ddl;

        public ZaikoIdouNyuuryoku_BL()
        {
            ddl = new D_Move_DL();
        }

        /// <summary>
        /// 移動番号検索にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Move_SelectAll(D_Move_Entity de)
        {
            return ddl.D_Move_SelectAll(de);
        }
        /// <summary>
        /// 移動依頼番号検索にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_MoveRequest_SelectAll(D_Move_Entity de)
        {
            return ddl.D_MoveRequest_SelectAll(de);
        }
        /// <summary>
        /// 棚検索にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable M_Location_SelectAll(M_Location_Entity de)
        {
            M_Location_DL mdl = new M_Location_DL();
            return mdl.M_Location_SelectAll(de);
        }
        /// <summary>
        /// 移動区分コンボボックスBind処理用
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public DataTable M_MovePurpose_Bind(M_MovePurpose_Entity me)
        {
            M_MovePurpose_DL mmdl = new M_MovePurpose_DL();
            return mmdl.M_MovePurpose_Bind(me);
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
        public bool M_Location_SelectData(M_Location_Entity me)
        {
            M_Location_DL mldl = new M_Location_DL();
            DataTable dt = mldl.M_Location_SelectData(me);
            if(dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        public DataTable D_Stock_Select(D_Stock_Entity de)
        {
            D_Stock_DL dsdl = new D_Stock_DL();
          return dsdl.D_Stock_Select(de);

        }
        public DataTable D_Stock_SelectZaiko(D_Stock_Entity de)
        {
            D_Stock_DL dsdl = new D_Stock_DL();
            return dsdl.D_Stock_SelectZaiko(de);

        }
        public bool D_Stock_SelectSuryo(D_Stock_Entity de)
        {
            D_Stock_DL dsdl = new D_Stock_DL();
            DataTable dt = dsdl.D_Stock_SelectSuryo(de);

            if (dt.Rows.Count == 0)
            {
                return false;
            }
            if (Z_Set(dt.Rows[0]["StockSu"]) < Z_Set( de.StockSu) || Z_Set(dt.Rows[0]["AllowableSu"]) < Z_Set(de.StockSu))
                return false;

            return true;
        }
        public DataTable D_Stock_SelectRackNO(D_Stock_Entity de)
        {
            D_Stock_DL dsdl = new D_Stock_DL();
            return dsdl.D_Stock_SelectRackNO(de);

        }
        public DataTable M_Souko_BindForHenpin(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_BindForHenpin(mse);
        }
        /// <summary>
        /// 移動入力更新処理
        /// ZaikoIdouNyuuryokuより更新時に使用
        /// </summary>
        public bool D_Move_Exec(D_Move_Entity dme, DataTable dt, short operationMode)
        {
            return ddl.D_Move_Exec(dme, dt, operationMode);
        }

        /// <summary>
        /// 移動入力取得処理
        /// ZaikoIdouNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Move_SelectData(D_Move_Entity de, short operationMode)
        {
            DataTable dt = ddl.D_Move_SelectData(de, operationMode);

            return dt;
        }

        /// <summary>
        /// 移動入力取得処理
        /// ZaikoIdouNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_MoveRequest_SelectData(D_Move_Entity de)
        {
            DataTable dt = ddl.D_MoveRequest_SelectData(de);

            return dt;
        }     
    }
}
