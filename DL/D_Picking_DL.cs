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
    public class D_Picking_DL : Base_DL
    {
        
        /// <summary>
        /// ピッキング番号検索
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Picking_SelectAll(D_Picking_Entity de)
        {
            string sp = "D_Picking_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PrintDateTimeFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.PrintDateTimeFrom } },
                { "@PrintDateTimeTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.PrintDateTimeTo } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
            };

            return SelectData(dic, sp);
        }

        public DataTable Pickinglist_Select(string PickingNO)
        {
            string sp = "D_Picking_SelectToCheckPickingKBN";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Code", new ValuePair { value1 = SqlDbType.VarChar, value2 = PickingNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// ピッキング入力更新処理
        /// PickingNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Picking_Exec(D_Picking_Entity dme, DataTable dt, short operationMode)
        {
            string sp = "PRC_PickingNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@PickingDate", SqlDbType.VarChar, dme.PickingDate);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dme.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dme.PC);

            //OUTパラメータの追加
            string outPutParam = "";

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dme.PickingNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// ピッキング入力データ取得処理
        /// PickingNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Picking_SelectData(D_Picking_Entity de, short operationMode)
        {
            string sp = "D_Picking_SelectData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@PickingNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.PickingNO } },
                { "@ShippingPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ShippingPlanDateFrom } },
                { "@ShippingPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ShippingPlanDateTo } },
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNO } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JanCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SKUCD } },
            };

            return SelectData(dic, sp);
        }


        /// <summary>
        /// PickingList
        /// </summary>
        /// <param name="dpe"></param>
        /// <returns></returns>
        public DataTable PickingList_InsertUpdateSelect_Check1(D_Picking_Entity dpe)
        {
            string sp = "PickingList_InsertUpdateSelect_Check1";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.SoukoCD} },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StoreCD} },
                { "@ShippingPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpe.ShippingPlanDateFrom } },
                { "@ShippingPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpe.ShippingPlanDateTo } },
                { "@ShippingDate", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpe.ShippingDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpe.InsertOperator } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }

        /// <summary>
        /// PickingList
        /// </summary>
        /// <param name="dpe"></param>
        /// <returns></returns>
        public DataTable PickingList_InsertUpdateSelect_Check2(D_Picking_Entity dpe)
        {
            string sp = "PickingList_InsertUpdateSelect_Check2";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.SoukoCD} },
                { "@PickingNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PickingNO} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpe.InsertOperator } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }

        /// <summary>
        /// PickingList
        /// </summary>
        /// <param name="dpe"></param>
        /// <returns></returns>
        public DataTable PickingList_InsertUpdateSelect_Check3(D_Picking_Entity dpe)
        {
            string sp = "PickingList_InsertUpdateSelect_Check3";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.SoukoCD} },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StoreCD} },
                { "@ShippingPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpe.ShippingPlanDateFrom } },
                { "@ShippingPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpe.ShippingPlanDateTo } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpe.InsertOperator } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }

        /// <summary>
        /// PickingList
        /// </summary>
        /// <param name="dpe"></param>
        /// <returns></returns>
        public bool D_Picking_Update(string pickingNo,D_Picking_Entity dpe)
        {
            string sp = "D_Picking_Update";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command,"@PickingNO",SqlDbType.VarChar,pickingNo);
            AddParam(command, "@Operator", SqlDbType.VarChar, dpe.InsertOperator);
            AddParam(command, "@Program", SqlDbType.VarChar, pickingNo);
            AddParam(command, "@PC", SqlDbType.VarChar, dpe.InsertOperator);

            UseTransaction = true;

            string outPutParam = "";    //未使用

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }

  
    }
}
