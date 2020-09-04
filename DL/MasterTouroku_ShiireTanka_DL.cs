using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

using System.Data.SqlClient;

namespace DL
{
    public class MasterTouroku_ShiireTanka_DL : Base_DL
    {
        public DataTable MastertorokuShiiretanka_Select(M_ItemOrderPrice_Entity mio)
        {
            string sp = "MastertorokuShiiretanka_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.VendorCD } },
                { "@storecd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.StoreCD } },
                { "@changedate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.ChangeDate } },
                //{ "@makerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.MakerItem } },
                { "@display", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.Display } },
                //{ "@brandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.BrandCD } },
                //{ "@sportcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.SportsCD} },
                //{ "@segmentcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.SegmentCD } },
                //{ "@lastyearterm", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastYearTerm } },
                //{ "@lastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastSeason } },
                //{ "@heardate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.Headerdate } },
                //{ "@itemcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.ITemCD } },
                //{ "@dateadd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.AddDate } },
                //{ "@rate", new ValuePair { value1 = SqlDbType.Decimal, value2 = mio.Rate} },
                //{ "@priceouttax", new ValuePair { value1 = SqlDbType.Money, value2 = mi.PriceOutTax } },
                //{ "@priceoutwithouttax", new ValuePair { value1 = SqlDbType.Money, value2 = mio.PriceWithoutTax } },
                //{ "@insertoperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.InsertOperator } },

            };
            return SelectData(dic, sp);
        }

        //public DataTable MastertorokuShiiretanka_Select(M_ItemOrderPrice_Entity mio, M_ITEM_Entity mi, string itemorsku)
        //{
        //    string sp = "MastertorokuShiiretanka_Select";
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.VendorCD } },
        //        { "@storecd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.StoreCD } },
        //        { "@changedate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.ChangeDate } },
        //        { "@makerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.MakerItem } },
        //        { "@display", new ValuePair { value1 = SqlDbType.VarChar, value2 = itemorsku } },
        //        { "@brandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.BrandCD } },
        //        { "@sportcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.SportsCD} },
        //        { "@segmentcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.SegmentCD } },
        //        { "@lastyearterm", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastYearTerm } },
        //        { "@lastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.LastSeason } },
        //        { "@heardate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.Headerdate } },
        //        { "@itemcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.ITemCD } },
        //        { "@dateadd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.AddDate } },
        //        { "@rate", new ValuePair { value1 = SqlDbType.Decimal, value2 = mio.Rate} },
        //        { "@priceouttax", new ValuePair { value1 = SqlDbType.Money, value2 = mi.PriceOutTax } },
        //        { "@priceoutwithouttax", new ValuePair { value1 = SqlDbType.Money, value2 = mio.PriceWithoutTax } },
        //        { "@insertoperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mio.InsertOperator } },

        //    };
        //    return SelectData(dic, sp);
        //}

        public bool Mastertoroku_Shiretanka_Insert(M_ItemOrderPrice_Entity mi)
        {
            string sp = "Mastertoroku_Shiretanka_Insert";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@tbitem", new ValuePair { value1 =SqlDbType.VarChar,value2 = mi.xml3 } },
                { "@tbsku", new ValuePair { value1 =SqlDbType.VarChar,value2 = mi.xml4 } },
                { "@tbdelItem", new ValuePair { value1 =SqlDbType.VarChar,value2 = mi.xml1 } },
                { "@tbdelJan", new ValuePair { value1 =SqlDbType.VarChar,value2 = mi.xml2 } },
                { "@operator", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.InsertOperator  } },
                { "@Program", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.ProgramID  } },
                { "@PC", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.PC  } },
                { "@OperateMode", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.ProcessMode  } },
                { "@KeyItem", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.Key  } }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        //public DataTable Mastertoroku_Shiretanka_Insert(string tbitem, string tbsku, string tbdelitem, string tbdeljan, M_ItemOrderPrice_Entity mi)
        //{
        //    string sp = "Mastertoroku_Shiretanka_Insert";
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        { "@tbitem", new ValuePair { value1 =SqlDbType.VarChar,value2 = tbitem } },
        //        { "@tbsku", new ValuePair { value1 =SqlDbType.VarChar,value2 = tbsku } },
        //        { "@tbdelItem", new ValuePair { value1 =SqlDbType.VarChar,value2 = tbdelitem } },
        //        { "@tbdelJan", new ValuePair { value1 =SqlDbType.VarChar,value2 = tbdeljan } },
        //        { "@operator", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.InsertOperator  } },
        //        { "@Program", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.ProgramID  } },
        //        { "@PC", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.PC  } },
        //        { "@OperateMode", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.ProcessMode  } },
        //        { "@KeyItem", new ValuePair { value1 =SqlDbType.VarChar,value2 =mi.Key  } }
        //    };
        //    return SelectData(dic, sp);
        //}
        public DataTable M_SKU_SelectFor_SKU_Update(string itemtb,string skutb,string itemcd,string date,string type)
        {
            string sp = "M_SKU_SelectFor_SKU_Update";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@itemtb", new ValuePair { value1 =SqlDbType.VarChar,value2 = itemtb } },
                { "@skutb", new ValuePair { value1 =SqlDbType.VarChar,value2 = skutb } },
                { "@itemcd", new ValuePair { value1 =SqlDbType.VarChar,value2 = itemcd } },
                { "@date", new ValuePair { value1 =SqlDbType.VarChar,value2 = date } },
                { "@type", new ValuePair { value1 =SqlDbType.VarChar,value2 = type } },
            };
            return SelectData(dic, sp);
        }

        public DataTable M_ITEM_SelectBy_ItemCD(M_ITEM_Entity mie)
        {
            string sp = "M_ITEM_SelectBy_ItemCD";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@itemcd", new ValuePair { value1 =SqlDbType.VarChar,value2 = mie.ITemCD } },
              
            };
            return SelectData(dic, sp);
        }
        public DataTable M_ITem_ItemNandPriceoutTax_Select(M_ITEM_Entity mi)
        {
            string sp = "M_ITem_ItemNandPriceoutTax_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mi.ITemCD } },
                { "@AddDate", new ValuePair { value1 = SqlDbType.Date, value2 = mi.AddDate } },

            };
            return SelectData(dic, sp);
        }

        /// <summary>
        /// ワークテーブル作成用データ取得
        /// </summary>
        /// <param name="hhe"></param>
        /// <returns></returns>
        public DataTable M_ItemOrderPrice_SelectFromItem(M_ItemOrderPrice_Entity mie)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                    {"@VendorCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.VendorCD} },
                    {"@StoreCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.StoreCD} } ,
                    {"@DispKbn",new ValuePair { value1=SqlDbType.TinyInt,value2=mie.DispKbn} } ,
                    {"@BaseDate",new ValuePair { value1=SqlDbType.VarChar,value2=mie.BaseDate} } ,
                    {"@BrandCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.BrandCD} } ,
                    {"@SportsCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.SportsCD} } ,
                    {"@SegmentCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.SegmentCD} } ,
                    {"@LastYearTerm",new ValuePair { value1=SqlDbType.VarChar,value2=mie.LastYearTerm} } ,
                    {"@LastSeason",new ValuePair { value1=SqlDbType.VarChar,value2=mie.LastSeason} } ,
                    {"@ChangeDate",new ValuePair { value1=SqlDbType.VarChar,value2=mie.ChangeDate} } ,
                    {"@MakerItem",new ValuePair { value1=SqlDbType.VarChar,value2=mie.MakerItem} }
               };
            return SelectData(dic, "M_ItemOrderPrice_SelectFromItem");
        }

        public DataTable M_ItemOrderPrice_SelectFromSKU(M_ItemOrderPrice_Entity mie)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                    {"@VendorCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.VendorCD} },
                    {"@StoreCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.StoreCD} } ,
                    {"@DispKbn",new ValuePair { value1=SqlDbType.TinyInt,value2=mie.DispKbn} } ,
                    {"@BaseDate",new ValuePair { value1=SqlDbType.VarChar,value2=mie.BaseDate} } ,
                    {"@BrandCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.BrandCD} } ,
                    {"@SportsCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.SportsCD} } ,
                    {"@SegmentCD",new ValuePair { value1=SqlDbType.VarChar,value2=mie.SegmentCD} } ,
                    {"@LastYearTerm",new ValuePair { value1=SqlDbType.VarChar,value2=mie.LastYearTerm} } ,
                    {"@LastSeason",new ValuePair { value1=SqlDbType.VarChar,value2=mie.LastSeason} } ,
                    {"@ChangeDate",new ValuePair { value1=SqlDbType.VarChar,value2=mie.ChangeDate} } ,
                    {"@MakerItem",new ValuePair { value1=SqlDbType.VarChar,value2=mie.MakerItem} }
               };
            return SelectData(dic, "M_ItemOrderPrice_SelectFromSKU");
        }

        public DataTable M_ITEM_SelectForShiireTanka(M_ITEM_Entity me)
        {
            string sp = "M_ITEM_SelectForShiireTanka";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                    { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ITemCD } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ChangeDate } },
                    { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.DeleteFlg } },
            };
            return SelectData(dic, sp);
        }

        public DataTable M_SKU_SelectForShiireTanka(M_SKU_Entity me)
        {
            string sp = "M_SKU_SelectForShiireTanka";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                    { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.MakerItem } },
                    { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = me.AdminNO } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ChangeDate } },
            };
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="hhe"></param>
        /// <param name="zdt"></param>
        /// <param name="jdt"></param>
        /// <returns></returns>
        public bool PRC_MasterTouroku_ShiireTanka(M_ItemOrderPrice_Entity mpe, DataTable dtOldITEM, DataTable dtOldSKU, DataTable dtITEM, DataTable dtSKU)
        {
            string sp = "PRC_MasterTouroku_ShiireTanka";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@VendorCD", SqlDbType.VarChar, mpe.VendorCD);
            AddParamForDataTable(command, "@OldITMTable", SqlDbType.Structured, dtOldITEM);
            AddParamForDataTable(command, "@OldSKUTable", SqlDbType.Structured, dtOldSKU);
            AddParamForDataTable(command, "@ITMTable", SqlDbType.Structured, dtITEM);
            AddParamForDataTable(command, "@SKUTable", SqlDbType.Structured, dtSKU);
            AddParam(command, "@Operator", SqlDbType.VarChar, mpe.Operator);
            AddParam(command, "@PC", SqlDbType.VarChar, mpe.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutVendorCD";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 30);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                mpe.VendorCD = outPutParam;

            return ret;
        }

    }
}
