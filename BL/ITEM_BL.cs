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
    public class ITEM_BL : Base_BL
    {
        M_ITEM_DL mdl;
        public ITEM_BL()
        {
            mdl = new M_ITEM_DL();
        }

        /// <summary>
        /// 商品マスタメンテよりデータ取得
        /// </summary>
        /// <param name="me"></param>
        /// <remarks>指定した適用日のデータを取得</remarks>
        /// <returns></returns>
        public bool M_ITEM_Select(M_ITEM_Entity me)
        {
            DataTable dt = mdl.M_ITEM_Select(me);
            if (dt.Rows.Count > 0)
            {
                me.ITemCD = dt.Rows[0]["ITemCD"].ToString();
                me.ColorNO = dt.Rows[0]["ColorNO"].ToString();
                me.SizeNO = dt.Rows[0]["SizeNO"].ToString();
                me.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();

                me.VariousFLG = dt.Rows[0]["VariousFLG"].ToString();
                me.ITemName = dt.Rows[0]["ITemName"].ToString();
                me.KanaName = dt.Rows[0]["KanaName"].ToString();
                me.SKUShortName = dt.Rows[0]["SKUShortName"].ToString();
                me.EnglishName = dt.Rows[0]["EnglishName"].ToString();
                me.SetKBN = dt.Rows[0]["SetKBN"].ToString();
                me.PresentKBN = dt.Rows[0]["PresentKBN"].ToString();
                me.SampleKBN = dt.Rows[0]["SampleKBN"].ToString();
                me.DiscountKBN = dt.Rows[0]["DiscountKBN"].ToString();
                me.ColorName = dt.Rows[0]["ColorName"].ToString();
                me.SizeName = dt.Rows[0]["SizeName"].ToString();
                me.WebFlg = dt.Rows[0]["WebFlg"].ToString();
                me.RealStoreFlg = dt.Rows[0]["RealStoreFlg"].ToString();
                me.MainVendorCD = dt.Rows[0]["MainVendorCD"].ToString();
                me.MakerVendorCD = dt.Rows[0]["MakerVendorCD"].ToString();
                me.BrandCD = dt.Rows[0]["BrandCD"].ToString();
                me.MakerItem = dt.Rows[0]["MakerItem"].ToString();
                me.TaniCD = dt.Rows[0]["TaniCD"].ToString();
                me.SportsCD = dt.Rows[0]["SportsCD"].ToString();
                me.SegmentCD = dt.Rows[0]["SegmentCD"].ToString();
                me.ZaikoKBN = dt.Rows[0]["ZaikoKBN"].ToString();
                me.Rack = dt.Rows[0]["Rack"].ToString();
                me.VirtualFlg = dt.Rows[0]["VirtualFlg"].ToString();
                me.DirectFlg = dt.Rows[0]["DirectFlg"].ToString();
                me.ReserveCD = dt.Rows[0]["ReserveCD"].ToString();
                me.NoticesCD = dt.Rows[0]["NoticesCD"].ToString();
                me.PostageCD = dt.Rows[0]["PostageCD"].ToString();
                me.ManufactCD = dt.Rows[0]["ManufactCD"].ToString();
                me.ConfirmCD = dt.Rows[0]["ConfirmCD"].ToString();
                me.WebStockFlg = dt.Rows[0]["WebStockFlg"].ToString();
                me.StopFlg = dt.Rows[0]["StopFlg"].ToString();
                me.DiscontinueFlg = dt.Rows[0]["DiscontinueFlg"].ToString();
                me.InventoryAddFlg = dt.Rows[0]["InventoryAddFlg"].ToString();
                me.MakerAddFlg = dt.Rows[0]["MakerAddFlg"].ToString();
                me.StoreAddFlg = dt.Rows[0]["StoreAddFlg"].ToString();
                me.NoNetOrderFlg = dt.Rows[0]["NoNetOrderFlg"].ToString();
                me.EDIOrderFlg = dt.Rows[0]["EDIOrderFlg"].ToString();
                me.CatalogFlg = dt.Rows[0]["CatalogFlg"].ToString();
                me.ParcelFlg = dt.Rows[0]["ParcelFlg"].ToString();
                me.AutoOrderFlg = dt.Rows[0]["AutoOrderFlg"].ToString();
                me.TaxRateFLG = dt.Rows[0]["TaxRateFLG"].ToString();
                me.CostingKBN = dt.Rows[0]["CostingKBN"].ToString();
                me.SaleExcludedFlg = dt.Rows[0]["SaleExcludedFlg"].ToString();
                me.PriceWithTax = dt.Rows[0]["PriceWithTax"].ToString();
                me.PriceOutTax = dt.Rows[0]["PriceOutTax"].ToString();
                me.Rate = dt.Rows[0]["Rate"].ToString();
                me.OrderPriceWithTax = dt.Rows[0]["OrderPriceWithTax"].ToString();
                me.OrderPriceWithoutTax = dt.Rows[0]["OrderPriceWithoutTax"].ToString();
                me.SaleStartDate = dt.Rows[0]["SaleStartDate"].ToString();
                me.WebStartDate = dt.Rows[0]["WebStartDate"].ToString();
                me.OrderAttentionCD = dt.Rows[0]["OrderAttentionCD"].ToString();
                me.OrderAttentionNote = dt.Rows[0]["OrderAttentionNote"].ToString();
                me.CommentInStore = dt.Rows[0]["CommentInStore"].ToString();
                me.CommentOutStore = dt.Rows[0]["CommentOutStore"].ToString();
                me.ExhibitionSegmentCD = dt.Rows[0]["ExhibitionSegmentCD"].ToString();
                me.OrderLot = dt.Rows[0]["OrderLot"].ToString();
                me.LastYearTerm = dt.Rows[0]["LastYearTerm"].ToString();
                me.LastSeason = dt.Rows[0]["LastSeason"].ToString();
                me.LastCatalogNO = dt.Rows[0]["LastCatalogNO"].ToString();
                me.LastCatalogPage = dt.Rows[0]["LastCatalogPage"].ToString();
                me.LastCatalogText = dt.Rows[0]["LastCatalogText"].ToString();
                me.LastInstructionsNO = dt.Rows[0]["LastInstructionsNO"].ToString();
                me.LastInstructionsDate = dt.Rows[0]["LastInstructionsDate"].ToString();
                me.WebAddress = dt.Rows[0]["WebAddress"].ToString();
                me.ApprovalDate = dt.Rows[0]["ApprovalDate"].ToString();

                me.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
                me.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();
                me.InsertOperator = dt.Rows[0]["InsertOperator"].ToString();
                me.InsertDateTime = dt.Rows[0]["InsertDateTime"].ToString();
                me.UpdateOperator = dt.Rows[0]["UpdateOperator"].ToString();
                me.UpdateDateTime = dt.Rows[0]["UpdateDateTime"].ToString();

                return true;
            }
            
                return false;
        }
        /// <summary>
        /// 商品マスタメンテよりデータ取得
        /// </summary>
        /// <param name="me"></param>
        /// <remarks>指定した適用日以前で最新のデータを取得</remarks>
        /// <returns></returns> 
        public bool M_ITEM_SelectTop1(M_ITEM_Entity me)
        {
            DataTable dt = mdl.M_ITEM_SelectTop1(me);
            if (dt.Rows.Count > 0)
            {
                me.ITemCD = dt.Rows[0]["ITemCD"].ToString();
                me.ColorNO = dt.Rows[0]["ColorNO"].ToString();
                me.SizeNO = dt.Rows[0]["SizeNO"].ToString();
                me.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();

                me.VariousFLG = dt.Rows[0]["VariousFLG"].ToString();
                me.ITemName = dt.Rows[0]["ITemName"].ToString();
                me.KanaName = dt.Rows[0]["KanaName"].ToString();
                me.SKUShortName = dt.Rows[0]["SKUShortName"].ToString();
                me.EnglishName = dt.Rows[0]["EnglishName"].ToString();
                me.SetKBN = dt.Rows[0]["SetKBN"].ToString();
                me.PresentKBN = dt.Rows[0]["PresentKBN"].ToString();
                me.SampleKBN = dt.Rows[0]["SampleKBN"].ToString();
                me.DiscountKBN = dt.Rows[0]["DiscountKBN"].ToString();
                me.ColorName = dt.Rows[0]["ColorName"].ToString();
                me.SizeName = dt.Rows[0]["SizeName"].ToString();
                me.WebFlg = dt.Rows[0]["WebFlg"].ToString();
                me.RealStoreFlg = dt.Rows[0]["RealStoreFlg"].ToString();
                me.MainVendorCD = dt.Rows[0]["MainVendorCD"].ToString();
                me.MakerVendorCD = dt.Rows[0]["MakerVendorCD"].ToString();
                me.BrandCD = dt.Rows[0]["BrandCD"].ToString();
                me.MakerItem = dt.Rows[0]["MakerItem"].ToString();
                me.TaniCD = dt.Rows[0]["TaniCD"].ToString();
                me.SportsCD = dt.Rows[0]["SportsCD"].ToString();
                me.SegmentCD = dt.Rows[0]["SegmentCD"].ToString();
                me.ZaikoKBN = dt.Rows[0]["ZaikoKBN"].ToString();
                me.Rack = dt.Rows[0]["Rack"].ToString();
                me.VirtualFlg = dt.Rows[0]["VirtualFlg"].ToString();
                me.DirectFlg = dt.Rows[0]["DirectFlg"].ToString();
                me.ReserveCD = dt.Rows[0]["ReserveCD"].ToString();
                me.NoticesCD = dt.Rows[0]["NoticesCD"].ToString();
                me.PostageCD = dt.Rows[0]["PostageCD"].ToString();
                me.ManufactCD = dt.Rows[0]["ManufactCD"].ToString();
                me.ConfirmCD = dt.Rows[0]["ConfirmCD"].ToString();
                me.WebStockFlg = dt.Rows[0]["WebStockFlg"].ToString();
                me.StopFlg = dt.Rows[0]["StopFlg"].ToString();
                me.DiscontinueFlg = dt.Rows[0]["DiscontinueFlg"].ToString();
                me.InventoryAddFlg = dt.Rows[0]["InventoryAddFlg"].ToString();
                me.MakerAddFlg = dt.Rows[0]["MakerAddFlg"].ToString();
                me.StoreAddFlg = dt.Rows[0]["StoreAddFlg"].ToString();
                me.NoNetOrderFlg = dt.Rows[0]["NoNetOrderFlg"].ToString();
                me.EDIOrderFlg = dt.Rows[0]["EDIOrderFlg"].ToString();
                me.CatalogFlg = dt.Rows[0]["CatalogFlg"].ToString();
                me.ParcelFlg = dt.Rows[0]["ParcelFlg"].ToString();
                me.AutoOrderFlg = dt.Rows[0]["AutoOrderFlg"].ToString();
                me.TaxRateFLG = dt.Rows[0]["TaxRateFLG"].ToString();
                me.CostingKBN = dt.Rows[0]["CostingKBN"].ToString();
                me.SaleExcludedFlg = dt.Rows[0]["SaleExcludedFlg"].ToString();
                me.PriceWithTax = dt.Rows[0]["PriceWithTax"].ToString();
                me.PriceOutTax = dt.Rows[0]["PriceOutTax"].ToString();
                me.Rate = dt.Rows[0]["Rate"].ToString();
                me.OrderPriceWithTax = dt.Rows[0]["OrderPriceWithTax"].ToString();
                me.OrderPriceWithoutTax = dt.Rows[0]["OrderPriceWithoutTax"].ToString();
                me.SaleStartDate = dt.Rows[0]["SaleStartDate"].ToString();
                me.WebStartDate = dt.Rows[0]["WebStartDate"].ToString();
                me.OrderAttentionCD = dt.Rows[0]["OrderAttentionCD"].ToString();
                me.OrderAttentionNote = dt.Rows[0]["OrderAttentionNote"].ToString();
                me.CommentInStore = dt.Rows[0]["CommentInStore"].ToString();
                me.CommentOutStore = dt.Rows[0]["CommentOutStore"].ToString();
                me.LastYearTerm = dt.Rows[0]["LastYearTerm"].ToString();
                me.LastSeason = dt.Rows[0]["LastSeason"].ToString();
                me.LastCatalogNO = dt.Rows[0]["LastCatalogNO"].ToString();
                me.LastCatalogPage = dt.Rows[0]["LastCatalogPage"].ToString();
                me.LastCatalogText = dt.Rows[0]["LastCatalogText"].ToString();
                me.LastInstructionsNO = dt.Rows[0]["LastInstructionsNO"].ToString();
                me.LastInstructionsDate = dt.Rows[0]["LastInstructionsDate"].ToString();
                me.WebAddress = dt.Rows[0]["WebAddress"].ToString();
                me.ApprovalDate = dt.Rows[0]["ApprovalDate"].ToString();

                me.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
                me.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();
                me.InsertOperator = dt.Rows[0]["InsertOperator"].ToString();
                me.InsertDateTime = dt.Rows[0]["InsertDateTime"].ToString();
                me.UpdateOperator = dt.Rows[0]["UpdateOperator"].ToString();
                me.UpdateDateTime = dt.Rows[0]["UpdateDateTime"].ToString();

                return true;
            }

            return false;
        }
        /// <summary>
        /// M_SKUデータ抽出
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public DataTable M_SKU_SelectByItemCD(M_ITEM_Entity me)
        {
            M_SKU_DL msdl = new M_SKU_DL();
            return msdl.M_SKU_SelectByItemCD(me);
        }
        public DataTable M_Site_SelectByItemCD(M_ITEM_Entity me)
        {
            M_Site_DL msdl = new M_Site_DL();
            return msdl.M_Site_SelectByItemCD(me);
        }
        public bool M_ITEM_Exec(M_ITEM_Entity me, DataTable dt, DataTable dtSite, short operationMode)
        {
            return mdl.M_ITEM_Exec(me, dt, dtSite, operationMode);
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        public void Para_Add(DataTable dt)
        {
            dt.Columns.Add("RowNO", typeof(int));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("ChangeDate", typeof(DateTime));
            dt.Columns.Add("VariousFLG", typeof(int));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("KanaName", typeof(string));
            dt.Columns.Add("SKUShortName", typeof(string));
            dt.Columns.Add("EnglishName", typeof(string));
            dt.Columns.Add("ITemCD", typeof(string));
            dt.Columns.Add("ColorNO", typeof(int));
            dt.Columns.Add("SizeNO", typeof(int));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("SetKBN", typeof(int));
            dt.Columns.Add("PresentKBN", typeof(int));
            dt.Columns.Add("SampleKBN", typeof(int));
            dt.Columns.Add("DiscountKBN", typeof(int));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("WebFlg", typeof(int));
            dt.Columns.Add("RealStoreFlg", typeof(int));
            dt.Columns.Add("MainVendorCD", typeof(string));
            dt.Columns.Add("MakerVendorCD", typeof(string));
            dt.Columns.Add("BrandCD", typeof(string));
            dt.Columns.Add("MakerItem", typeof(string));
            dt.Columns.Add("TaniCD", typeof(string));
            dt.Columns.Add("SportsCD", typeof(string));
            dt.Columns.Add("SegmentCD", typeof(string));
            dt.Columns.Add("ZaikoKBN", typeof(int));
            dt.Columns.Add("Rack", typeof(string));
            dt.Columns.Add("VirtualFlg", typeof(int));
            dt.Columns.Add("DirectFlg", typeof(int));
            dt.Columns.Add("ReserveCD", typeof(string));
            dt.Columns.Add("NoticesCD", typeof(string));
            dt.Columns.Add("PostageCD", typeof(string));
            dt.Columns.Add("ManufactCD", typeof(string));
            dt.Columns.Add("ConfirmCD", typeof(string));
            dt.Columns.Add("WebStockFlg", typeof(int));
            dt.Columns.Add("StopFlg", typeof(int));
            dt.Columns.Add("DiscontinueFlg", typeof(int));
            dt.Columns.Add("InventoryAddFlg", typeof(int));
            dt.Columns.Add("MakerAddFlg", typeof(int));
            dt.Columns.Add("StoreAddFlg", typeof(int));
            dt.Columns.Add("NoNetOrderFlg", typeof(int));
            dt.Columns.Add("EDIOrderFlg", typeof(int));
            dt.Columns.Add("CatalogFlg", typeof(int));
            dt.Columns.Add("ParcelFlg", typeof(int));
            dt.Columns.Add("AutoOrderFlg", typeof(int));
            dt.Columns.Add("TaxRateFLG", typeof(int));
            dt.Columns.Add("CostingKBN", typeof(int));
            dt.Columns.Add("SaleExcludedFlg", typeof(int));
            dt.Columns.Add("PriceWithTax", typeof(decimal));
            dt.Columns.Add("PriceOutTax", typeof(decimal));
            dt.Columns.Add("OrderPriceWithTax", typeof(decimal));
            dt.Columns.Add("OrderPriceWithoutTax", typeof(decimal));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("SaleStartDate", typeof(DateTime));
            dt.Columns.Add("WebStartDate", typeof(DateTime));
            dt.Columns.Add("OrderAttentionCD", typeof(string));
            dt.Columns.Add("OrderAttentionNote", typeof(string));
            dt.Columns.Add("CommentInStore", typeof(string));
            dt.Columns.Add("CommentOutStore", typeof(string));
            dt.Columns.Add("ExhibitionSegmentCD", typeof(string));
            dt.Columns.Add("OrderLot", typeof(int));
            dt.Columns.Add("ExhibitionCommonCD", typeof(string));
            dt.Columns.Add("LastYearTerm", typeof(string));
            dt.Columns.Add("LastSeason", typeof(string));
            dt.Columns.Add("LastCatalogNO", typeof(string));
            dt.Columns.Add("LastCatalogPage", typeof(string));
            dt.Columns.Add("LastCatalogText", typeof(string));
            dt.Columns.Add("LastInstructionsNO", typeof(string));
            dt.Columns.Add("LastInstructionsDate", typeof(DateTime));
            dt.Columns.Add("WebAddress", typeof(string));
            dt.Columns.Add("ApprovalDate", typeof(DateTime));
            dt.Columns.Add("TagName1", typeof(string));
            dt.Columns.Add("TagName2", typeof(string));
            dt.Columns.Add("TagName3", typeof(string));
            dt.Columns.Add("TagName4", typeof(string));
            dt.Columns.Add("TagName5", typeof(string));
            dt.Columns.Add("TagName6", typeof(string));
            dt.Columns.Add("TagName7", typeof(string));
            dt.Columns.Add("TagName8", typeof(string));
            dt.Columns.Add("TagName9", typeof(string));
            dt.Columns.Add("TagName10", typeof(string));    

            dt.Columns.Add("SetAdminCD", typeof(int));
            dt.Columns.Add("SetItemCD", typeof(string));
            dt.Columns.Add("SetSKUCD", typeof(string));
            dt.Columns.Add("SetSU", typeof(int));
            dt.Columns.Add("DeleteFlg", typeof(int));

            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        public DataTable GetGridEntity(DataTable dtSKU)
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNO = 0;
            foreach (DataRow row in dtSKU.Rows)
            {
                rowNO++;

                dt.Rows.Add(
                 rowNO 
                 , Z_Set(row["AdminNO"])
                 , row["SKUCD"]
                 , row["ChangeDate"]
                 , Z_Set(row["VariousFLG"])
                 , row["SKUName"]
                 , row["KanaName"].ToString() == ""? null: row["KanaName"]
                 , row["SKUShortName"].ToString() == "" ? null : row["SKUShortName"]
                 , row["EnglishName"].ToString() == "" ? null : row["EnglishName"]
                 , row["ITemCD"].ToString() == "" ? null : row["ITemCD"]
                 , Z_Set(row["ColorNO"])
                 , Z_Set(row["SizeNO"])
                 , row["JanCD"].ToString() == "" ? null : row["JanCD"]
                 , Z_Set(row["SetKBN"])
                 , Z_Set(row["PresentKBN"])
                 , Z_Set(row["SampleKBN"])
                 , Z_Set(row["DiscountKBN"])
                 , row["ColorName"]
                 , row["SizeName"]
                 , Z_Set(row["WebFlg"])
                 , Z_Set(row["RealStoreFlg"])
                 , row["MainVendorCD"].ToString() == "" ? null : row["MainVendorCD"]
                 , row["MakerVendorCD"].ToString() == "" ? null : row["MakerVendorCD"]
                 , row["BrandCD"].ToString() == "" ? null : row["BrandCD"]
                 , row["MakerItem"].ToString() == "" ? null : row["MakerItem"]
                 , row["TaniCD"].ToString() == "" ? null : row["TaniCD"]
                 , row["SportsCD"].ToString() == "" ? null : row["SportsCD"]
                 , row["SegmentCD"].ToString() == "" ? null : row["SegmentCD"]
                 , Z_Set(row["ZaikoKBN"])
                 , row["Rack"].ToString() == "" ? null : row["Rack"]
                 , Z_Set(row["VirtualFlg"])
                 , Z_Set(row["DirectFlg"])
                 , row["ReserveCD"].ToString() == "" ? null : row["ReserveCD"]
                 , row["NoticesCD"].ToString() == "" ? null : row["NoticesCD"]
                 , row["PostageCD"].ToString() == "" ? null : row["PostageCD"]
                 , row["ManufactCD"].ToString() == "" ? null : row["ManufactCD"]
                 , row["ConfirmCD"].ToString() == "" ? null : row["ConfirmCD"]
                 , Z_Set(row["WebStockFlg"])
                 , Z_Set(row["StopFlg"])
                 , Z_Set(row["DiscontinueFlg"])
                 , Z_Set(row["InventoryAddFlg"])
                 , Z_Set(row["MakerAddFlg"])
                 , Z_Set(row["StoreAddFlg"])
                 , Z_Set(row["NoNetOrderFlg"])
                 , Z_Set(row["EDIOrderFlg"])
                 , Z_Set(row["CatalogFlg"])
                 , Z_Set(row["ParcelFlg"])
                 , Z_Set(row["AutoOrderFlg"])
                 , Z_Set(row["TaxRateFLG"])
                 , Z_Set(row["CostingKBN"])
                 , Z_Set(row["SaleExcludedFlg"])
                 , Z_Set(row["PriceWithTax"])
                 , Z_Set(row["PriceOutTax"])
                 , Z_Set(row["OrderPriceWithTax"])
                 , Z_Set(row["OrderPriceWithoutTax"])
                 , Z_Set(row["Rate"])
                 , row["SaleStartDate"].ToString() == "" ? null: row["SaleStartDate"]
                 , row["WebStartDate"].ToString() == "" ? null : row["WebStartDate"]
                 , row["OrderAttentionCD"].ToString() == "" ? null : row["OrderAttentionCD"]
                 , row["OrderAttentionNote"].ToString() == "" ? null : row["OrderAttentionNote"]
                 , row["CommentInStore"].ToString() == "" ? null : row["CommentInStore"]
                 , row["CommentOutStore"].ToString() == "" ? null : row["CommentOutStore"]
                 , row["ExhibitionSegmentCD"].ToString() == "" ? null : row["ExhibitionSegmentCD"]
                 , Z_Set(row["OrderLot"])
                 , row["ExhibitionCommonCD"].ToString() == "" ? null : row["ExhibitionCommonCD"]
                 , row["LastYearTerm"]
                 , row["LastSeason"]
                 , row["LastCatalogNO"]
                 , row["LastCatalogPage"]
                 , row["LastCatalogText"]
                 , row["LastInstructionsNO"]
                 , row["LastInstructionsDate"].ToString() == "" ? null : row["LastInstructionsDate"]
                 , row["WebAddress"]
                 , row["ApprovalDate"].ToString() == "" ? null : row["ApprovalDate"]
                 , row["TagName1"].ToString() == "" ? null : row["TagName1"]
                 , row["TagName2"].ToString() == "" ? null : row["TagName2"]
                 , row["TagName3"].ToString() == "" ? null : row["TagName3"]
                 , row["TagName4"].ToString() == "" ? null : row["TagName4"]
                 , row["TagName5"].ToString() == "" ? null : row["TagName5"]
                 , row["TagName6"].ToString() == "" ? null : row["TagName6"]
                 , row["TagName7"].ToString() == "" ? null : row["TagName7"]
                 , row["TagName8"].ToString() == "" ? null : row["TagName8"]
                 , row["TagName9"].ToString() == "" ? null : row["TagName9"]
                 , row["TagName10"].ToString() == "" ? null : row["TagName10"]
                 , Z_Set(row["SetAdminCD"])
                 , row["SetItemCD"].ToString() == "" ? null : row["SetItemCD"]
                 , row["SetSKUCD"].ToString() == "" ? null : row["SetSKUCD"]
                 , Z_Set(row["SetSU"])
                 , Z_Set(row["DeleteFlg"])
                 , Z_Set(row["AdminNO"]) == 0 ? 0: 1     //UpdateFlg
                 );

            }

            return dt;
        }

        public void Para_Add2(DataTable dt)
        {
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("ColorNO", typeof(int));
            dt.Columns.Add("SizeNO", typeof(int));
            dt.Columns.Add("APIKey", typeof(int));
            dt.Columns.Add("ShouhinCD", typeof(string));
            dt.Columns.Add("SiteURL", typeof(string));

            //dt.Columns.Add("UpdateFlg", typeof(int));
        }
        public DataTable GetSiteEntity(DataTable dtSite)
        {
            if (dtSite == null)
            {
                throw new ArgumentNullException(nameof(dtSite));
            }

            DataTable dt = new DataTable();
            Para_Add2(dt);

            foreach (DataRow row in dtSite.Rows)
            {
                dt.Rows.Add(
                  Z_Set(row["AdminNO"])
                 , Z_Set(row["ColorNO"])
                 , Z_Set(row["SizeNO"])
                 , Z_Set(row["APIKey"])
                 , row["ShouhinCD"].ToString() == "" ? null : row["ShouhinCD"]
                 , row["SiteURL"].ToString() == "" ? null : row["SiteURL"]
                 //, Z_Set(row["AdminNO"]) == 0 ? 0 : 1     //UpdateFlg
                 );

            }

            return dt;
        }
        public bool M_Store_SelectByKbn(M_Store_Entity me)
        {
            M_Store_DL dl = new M_Store_DL();
            DataTable dt = dl.M_Store_SelectByKbn(me);
            if(dt.Rows.Count > 0)
            {
                me.StoreCD = dt.Rows[0]["StoreCD"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool M_Souko_Search(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            DataTable dt = msdl.M_Souko_Search(mse);
            if (dt.Rows.Count > 0)
            {
                mse.SoukoCD = dt.Rows[0]["SoukoCD"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool M_Location_SelectData(M_Location_Entity me)
        {
            M_Location_DL mldl = new M_Location_DL();
            DataTable dt = mldl.M_Location_SelectData(me);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool M_SKU_SelectByJANCD(M_SKU_Entity me)
        {
            M_SKU_DL msdl = new M_SKU_DL();
            DataTable dt= msdl.M_SKU_SelectByJANCD(me);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public bool M_SKU_SelectBySKUCD(M_SKU_Entity me)
        {
            M_SKU_DL msdl = new M_SKU_DL();
            DataTable dt = msdl.M_SKU_SelectBySKUCD(me);
            if (dt.Rows.Count > 0)
            {
                me.SKUName = dt.Rows[0]["SKUName"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool M_JANCounter_Select(M_JANCounter_Entity me)
        {
            M_JANCounter_DL msdl = new M_JANCounter_DL();
            DataTable dt = msdl.M_JANCounter_Select(me);
            if (dt.Rows.Count > 0)
            {
                me.UpdatingFlg = dt.Rows[0]["UpdatingFlg"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool M_JANCounter_Update(M_JANCounter_Entity me)
        {
            M_JANCounter_DL msdl = new M_JANCounter_DL();
            DataTable dt = msdl.M_JANCounter_Update(me);
            if (dt.Rows.Count > 0)
            {
                me.JanCount = dt.Rows[0]["JanCount"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
