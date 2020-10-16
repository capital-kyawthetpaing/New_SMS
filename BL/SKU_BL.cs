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
        /// 商品マスタメンテよりデータ取得	
        /// </summary>	
        /// <param name="mse"></param>	
        /// <remarks>指定した適用日のデータを取得</remarks>	
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
                mse.KanaName = dt.Rows[0]["KanaName"].ToString();
                mse.SKUShortName = dt.Rows[0]["SKUShortName"].ToString();
                mse.EnglishName = dt.Rows[0]["EnglishName"].ToString();
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
                mse.WebFlg = dt.Rows[0]["WebFlg"].ToString();
                mse.RealStoreFlg = dt.Rows[0]["RealStoreFlg"].ToString();
                mse.MainVendorCD = dt.Rows[0]["MainVendorCD"].ToString();
                mse.MakerVendorCD = dt.Rows[0]["MakerVendorCD "].ToString();
                mse.BrandCD = dt.Rows[0]["BrandCD"].ToString();
                mse.MakerItem = dt.Rows[0]["MakerItem"].ToString();
                mse.TaniCD = dt.Rows[0]["TaniCD"].ToString();
                mse.SportsCD = dt.Rows[0]["SportsCD"].ToString();
                mse.ZaikoKBN = dt.Rows[0]["ZaikoKBN"].ToString();
                mse.Rack = dt.Rows[0]["Rack"].ToString();
                mse.VirtualFlg = dt.Rows[0]["VirtualFlg"].ToString();
                mse.DirectFlg = dt.Rows[0]["DirectFlg"].ToString();
                mse.ReserveCD = dt.Rows[0]["ReserveCD"].ToString();
                mse.NoticesCD = dt.Rows[0]["NoticesCD"].ToString();
                mse.PostageCD = dt.Rows[0]["PostageCD"].ToString();
                mse.ManufactCD = dt.Rows[0]["ManufactCD"].ToString();
                mse.ConfirmCD = dt.Rows[0]["ConfirmCD"].ToString();
                mse.WebStockFlg = dt.Rows[0]["WebStockFlg"].ToString();
                mse.StopFlg = dt.Rows[0]["StopFlg"].ToString();
                mse.DiscontinueFlg = dt.Rows[0]["DiscontinueFlg"].ToString();
                mse.InventoryAddFlg = dt.Rows[0]["InventoryAddFlg"].ToString();
                mse.MakerAddFlg = dt.Rows[0]["MakerAddFlg"].ToString();
                mse.StoreAddFlg = dt.Rows[0]["StoreAddFlg"].ToString();
                mse.NoNetOrderFlg = dt.Rows[0]["NoNetOrderFlg"].ToString();
                mse.EDIOrderFlg = dt.Rows[0]["EDIOrderFlg"].ToString();
                mse.CatalogFlg = dt.Rows[0]["CatalogFlg"].ToString();
                mse.ParcelFlg = dt.Rows[0]["ParcelFlg"].ToString();
                mse.AutoOrderFlg = dt.Rows[0]["AutoOrderFlg"].ToString();
                mse.TaxRateFLG = dt.Rows[0]["TaxRateFLG"].ToString();
                mse.CostingKBN = dt.Rows[0]["CostingKBN"].ToString();
                mse.SaleExcludedFlg = dt.Rows[0]["SaleExcludedFlg"].ToString();
                mse.PriceWithTax = dt.Rows[0]["PriceWithTax"].ToString();
                mse.PriceOutTax = dt.Rows[0]["PriceOutTax"].ToString();
                mse.OrderPriceWithTax = dt.Rows[0]["OrderPriceWithTax"].ToString();
                mse.OrderPriceWithoutTax = dt.Rows[0]["OrderPriceWithoutTax"].ToString();
                mse.SaleStartDate = dt.Rows[0]["SaleStartDate"].ToString();
                mse.WebStartDate = dt.Rows[0]["WebStartDate"].ToString();
                mse.OrderAttentionCD = dt.Rows[0]["OrderAttentionCD"].ToString();
                mse.OrderAttentionNote = dt.Rows[0]["OrderAttentionNote"].ToString();
                mse.CommentInStore = dt.Rows[0]["CommentInStore"].ToString();
                mse.CommentOutStore = dt.Rows[0]["CommentOutStore"].ToString();
                mse.LastYearTerm = dt.Rows[0]["LastYearTerm"].ToString();
                mse.LastSeason = dt.Rows[0]["LastSeason"].ToString();
                mse.LastCatalogNO = dt.Rows[0]["LastCatalogNO"].ToString();
                mse.LastCatalogPage = dt.Rows[0]["LastCatalogPage"].ToString();
                mse.LastCatalogText = dt.Rows[0]["LastCatalogText"].ToString();
                mse.LastInstructionsNO = dt.Rows[0]["LastInstructionsNO"].ToString();
                mse.LastInstructionsDate = dt.Rows[0]["LastInstructionsDate"].ToString();
                mse.WebAddress = dt.Rows[0]["WebAddress"].ToString();
                mse.SetAdminCD = dt.Rows[0]["SetAdminCD"].ToString();
                mse.SetItemCD = dt.Rows[0]["SetItemCD"].ToString();
                mse.SetSKUCD = dt.Rows[0]["SetSKUCD"].ToString();
                mse.SetSU = dt.Rows[0]["SetSU"].ToString();
                mse.ApprovalDate = dt.Rows[0]["ApprovalDate"].ToString();
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
        public bool M_SKU_Exec(M_SKU_Entity me, short operationMode)
        {
            return mdl.M_SKU_Exec(me, operationMode);
        }

        public DataTable M_SKU_SelectByJanCD_ForTenzikaishouhin(M_SKU_Entity mse)
        {
            return mdl.M_SKU_SelectByJanCD_ForTenzikaishouhin(mse);
        }
      
    }
}
