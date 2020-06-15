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
    public class MasterTouroku_HacchuuPrice_BL : Base_BL
    {
        MasterTouroku_HacchuuPrice_DL msdl;

        public MasterTouroku_HacchuuPrice_BL()
        {
            msdl = new MasterTouroku_HacchuuPrice_DL();
        }
           

        /// <summary>
        /// ワークテーブル用データ取得
        /// </summary>
        /// <param name="hhe"></param>
        /// <returns></returns>
        public DataTable M_ItemOrderPrice_SelectFromItem(M_ItemOrderPrice_Entity mie)
        {
            return msdl.M_ItemOrderPrice_SelectFromItem(mie);
        }

        public DataTable M_ItemOrderPrice_SelectFromSKU(M_ItemOrderPrice_Entity mie)
        {
            return msdl.M_ItemOrderPrice_SelectFromSKU(mie);
        }

        public bool M_ITEM_SelectForShiireTanka(M_ITEM_Entity me)
        {
            DataTable dt = msdl.M_ITEM_SelectForShiireTanka(me);
            if (dt.Rows.Count > 0)
            {
                me.ITemCD = dt.Rows[0]["ITemCD"].ToString();
                me.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();

                me.ITemName = dt.Rows[0]["ITemName"].ToString();
                me.BrandCD = dt.Rows[0]["BrandCD"].ToString();
                me.BrandName = dt.Rows[0]["BrandName"].ToString();
                me.MakerItem = dt.Rows[0]["MakerItem"].ToString();
                me.SportsCD = dt.Rows[0]["SportsCD"].ToString();
                me.SportsName = dt.Rows[0]["SportsName"].ToString();
                me.SegmentCD = dt.Rows[0]["SegmentCD"].ToString();
                me.SegmentName = dt.Rows[0]["SegmentName"].ToString();
                me.PriceOutTax = dt.Rows[0]["PriceOutTax"].ToString();
                me.LastYearTerm = dt.Rows[0]["LastYearTerm"].ToString();
                me.LastSeason = dt.Rows[0]["LastSeason"].ToString();

                me.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();

                return true;
            }

            return false;
        }

        public DataTable M_SKU_SelectForShiireTanka(M_SKU_Entity me)
        {
            return msdl.M_SKU_SelectForShiireTanka(me);            
        }

        /// <summary>
        /// 更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool PRC_MasterTouroku_HacchuuPrice(M_ItemOrderPrice_Entity mpe, DataTable dtOldITEM, DataTable dtOldSKU, DataTable dtITEM, DataTable dtSKU)
        {
            return msdl.PRC_MasterTouroku_HacchuuPrice(mpe, dtOldITEM, dtOldSKU, dtITEM, dtSKU);
        }

    }
}
