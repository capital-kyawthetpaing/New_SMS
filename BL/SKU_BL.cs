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
    public class SKU_BL : Base_BL
    {
        M_SKU_DL mdl;
        public SKU_BL()
        {
            mdl = new M_SKU_DL();
        }

        /// <summary>
        /// 恐らく未使用、マスタメンテで使う？？
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public bool M_SKU_Select(M_SKU_Entity mse)
        {

          DataTable dt = mdl.M_SKU_Select(mse);
            if (dt.Rows.Count > 0)
            {
                mse.SKUCD = dt.Rows[0]["SKUCD"].ToString();
                mse.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();
                mse.VariousFLG = dt.Rows[0]["VariousFLG"].ToString();
                mse.SKUName = dt.Rows[0]["SKUName"].ToString();
                mse.SKUShortName = dt.Rows[0]["SKUShortName"].ToString();
                mse.ITemCD = dt.Rows[0]["ITemCD"].ToString();
                mse.ColorNO = dt.Rows[0]["ColorNO"].ToString();
                mse.SizeNO = dt.Rows[0]["SizeNO"].ToString();
                mse.JanCD = dt.Rows[0]["JanCD"].ToString();
                mse.SetKBN = dt.Rows[0]["SetKBN"].ToString();
                mse.PresentKBN = dt.Rows[0]["PresentKBN"].ToString();
                mse.SampleKBN = dt.Rows[0]["SampleKBN"].ToString();
                mse.DiscountKBN = dt.Rows[0]["DiscountKBN"].ToString();
                mse.ColorName = dt.Rows[0]["ColorName"].ToString();
                mse.SizeName = dt.Rows[0]["SizeName"].ToString();
                mse.MainVendorCD = dt.Rows[0]["MainVendorCD"].ToString();
                mse.MakerVendorCD = dt.Rows[0]["MakerVendorCD "].ToString();
                mse.BrandCD = dt.Rows[0]["BrandCD"].ToString();
                mse.MakerItem = dt.Rows[0]["MakerItem"].ToString();
                mse.TaniCD = dt.Rows[0]["TaniCD"].ToString();
                mse.OrderAttentionCD = dt.Rows[0]["OrderAttentionCD"].ToString();
                mse.SportsCD = dt.Rows[0]["SportsCD"].ToString();
                //mse.ClassificationA = dt.Rows[0]["ClassificationA"].ToString();
                //mse.ClassificationB = dt.Rows[0]["ClassificationB"].ToString();
                //mse.ClassificationC = dt.Rows[0]["ClassificationC"].ToString();
                mse.ZaikoKBN = dt.Rows[0]["ZaikoKBN"].ToString();
                mse.Rack = dt.Rows[0]["Rack"].ToString();
                mse.TaxRateFLG = dt.Rows[0]["TaxRateFLG"].ToString();
                mse.PriceWithTax = dt.Rows[0]["PriceWithTax"].ToString();
                mse.PriceOutTax = dt.Rows[0]["PriceOutTax"].ToString();
                mse.OrderPriceWithTax = dt.Rows[0]["OrderPriceWithTax"].ToString();
                mse.OrderPriceWithoutTax = dt.Rows[0]["OrderPriceWithoutTax"].ToString();
                mse.CommentInStore = dt.Rows[0]["CommentInStore"].ToString();
                mse.CommentOutStore = dt.Rows[0]["CommentOutStore"].ToString();

                mse.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
                mse.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();
                mse.InsertOperator = dt.Rows[0]["InsertOperator"].ToString();
                mse.InsertDateTime = dt.Rows[0]["InsertDateTime"].ToString();
                mse.UpdateOperator = dt.Rows[0]["UpdateOperator"].ToString();
                mse.UpdateDateTime = dt.Rows[0]["UpdateDateTime"].ToString();

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 複数件取得される場合あり
        /// SKUCD、JANCD引数未設定OK
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable M_SKU_SelectAll(M_SKU_Entity mse)
        {
            return mdl.M_SKU_SelectAll(mse);
        }

        public DataTable M_SKU_SelectForSearchProduct(M_SKU_Entity mse, M_SKUInfo_Entity msie)
        {
            return mdl.M_SKU_SelectForSearchProduct(mse, msie);
        }

        /// <summary>
        /// </summary>
        public DataTable M_SKU_SelectAllForTempoShukka(M_SKU_Entity mse, string juchuuNo)
        {
            return mdl.M_SKU_SelectAllForTempoShukka(mse, juchuuNo);
        }

        public bool M_SKU_SelectByMaker(M_SKU_Entity mse)
        {
            DataTable dt = mdl.M_SKU_SelectByMaker(mse);
            if (dt.Rows.Count > 0)
            {
                mse.SKUName = dt.Rows[0]["SKUName"].ToString();
                return true;
            }
            return false;
        }

        public DataTable M_SKU_SelectAllForTempoRegiShohin(M_SKU_Entity mse)
        {
            return mdl.M_SKU_SelectAllForTempoRegiShohin(mse);
        }
    }
}
