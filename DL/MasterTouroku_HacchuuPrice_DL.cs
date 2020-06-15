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
    public class MasterTouroku_HacchuuPrice_DL : Base_DL
    {
        
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
        public bool PRC_MasterTouroku_HacchuuPrice(M_ItemOrderPrice_Entity mpe, DataTable dtOldITEM, DataTable dtOldSKU, DataTable dtITEM, DataTable dtSKU)
        {
            string sp = "PRC_MasterTouroku_HacchuuPrice";

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
