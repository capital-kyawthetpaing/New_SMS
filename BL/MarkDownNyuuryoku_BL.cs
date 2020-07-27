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
    public class MarkDownNyuuryoku_BL : Base_BL
    {
        D_MarkDown_DL mdl;
        public MarkDownNyuuryoku_BL()
        {
            mdl = new D_MarkDown_DL();
        }

        /// <summary>
        /// 倉庫取得
        /// </summary>
        /// <param name="StoreAuthorizationsCD"></param>
        /// <returns></returns>
        public DataTable M_Souko_BindForMarkDown(string StoreAuthorizationsCD)
        {
            M_Souko_DL sdl = new M_Souko_DL();
            return sdl.M_Souko_BindForMarkDown(StoreAuthorizationsCD);
        }

        public DataTable M_Souko_SelectData(M_Souko_Entity mse)
        {
            M_Souko_DL sdl = new M_Souko_DL();
            return sdl.M_Souko_SelectData(mse);
        }

        /// <summary>
        /// 在庫情報コンボボックス値取得
        /// </summary>
        /// <param name="StoreAuthorizationsCD"></param>
        /// <returns></returns>
        public DataTable D_StockReplica_Bind()
        {
            D_StockReplica_DL sdl = new D_StockReplica_DL();
            return sdl.D_StockReplica_Bind();
        }

        /// <summary>
        /// 在庫情報取得
        /// </summary>
        /// <returns></returns>
        public DataTable D_StockReplica_SelectForMarkDown(D_StockReplica_Entity dse)
        {
            D_StockReplica_DL sdl = new D_StockReplica_DL();
            return sdl.D_StockReplica_SelectForMarkDown(dse);
        }

        /// <summary>
        /// 倉庫取得
        /// </summary>
        /// <param name="dme"></param>
        /// <returns></returns>
        public DataTable D_MarkDown_SelectData(D_MarkDown_Entity dme)
        {
            return mdl.D_MarkDown_SelectData(dme);
        }

        /// <summary>
        /// 商品取得
        /// </summary>
        /// <param name="dme"></param>
        /// <returns></returns>
        public DataTable M_SKU_SelectForMarkDown(M_SKU_Entity mse)
        {
            return mdl.M_SKU_SelectForMarkDown(mse);
        }

        /// <summary>
        /// 締処理済の場合（以下のSelectができる場合）Error
        /// </summary>
        /// <param name="dpe"></param>
        /// <returns></returns>
        public bool CheckPayCloseHistory(D_PayCloseHistory_Entity dpe)
        {
            D_PayCloseHistory_DL dpd = new D_PayCloseHistory_DL();
            DataTable dt = dpd.CheckPayCloseHistory(dpe);

            bool ret = false;

            if (dt.Rows.Count > 0)
            {
                dpe.PayCloseNO = dt.Rows[0]["PayCloseNO"].ToString();
                ret = true;
            }

            return ret;
        }

        /// <summary>	
        /// 店舗の締日チェック	
        /// 店舗締マスターで判断	
        /// </summary>	
        /// <param name="mse">M_StoreClose_Entity</param>	
        /// <returns></returns>	
        public bool CheckStoreCloseForMarkDown(M_StoreClose_Entity mse, bool sir, bool sha)
        {
            M_StoreClose_DL msdl = new M_StoreClose_DL();
            DataTable dt = msdl.M_StoreClose_Select(mse);
            if (dt.Rows.Count > 0)
            {
                if (sir)
                {
                    if (!dt.Rows[0]["ClosePosition2"].ToString().Equals("0"))
                    {
                        return false;
                    }
                }

                if (sha)
                {
                    if (!dt.Rows[0]["ClosePosition4"].ToString().Equals("0"))
                    {
                        return false;
                    }
                }                
                
                return true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 返品入力更新処理
        /// MarkDownNyuuryokuより更新時に使用
        /// </summary>
        public bool PRC_MarkDownNyuuryoku(D_MarkDown_Entity dme, DataTable dt, short operationMode)
        {
            return mdl.PRC_MarkDownNyuuryoku(dme, dt, operationMode);
        }

    }
}
