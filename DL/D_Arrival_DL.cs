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
    public class D_Arrival_DL : Base_DL
    {
        public DataTable D_Arrival_Select(D_Arrival_Entity ode)
        {
            string sp = "D_Arrival_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ArrivalNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = ode.ArrivalNO } },
                //{ "@OrderRows", new ValuePair { value1 = SqlDbType.Int, value2 = ode.OrderRows } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = ode.VendorCD } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 入荷番号検索
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Arrival_SelectAll(D_Arrival_Entity de)
        {
            string sp = "D_Arrival_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ArrivalDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ArrivalDateFrom } },
                { "@ArrivalDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ArrivalDateTo } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JanCD } },
                { "@VendorDeliveryNo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorDeliveryNo } },
            };

            return SelectData(dic, sp);
        }
 
        /// <summary>
        /// 入荷入力更新処理
        /// NyuukaNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Arrival_Exec(D_Arrival_Entity dme, DataTable dt, short operationMode )
        {
            string sp = "PRC_NyuukaNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int,operationMode.ToString());
            AddParam(command, "@ArrivalNO", SqlDbType.VarChar, dme.ArrivalNO);
            AddParam(command, "@VendorDeliveryNo", SqlDbType.VarChar, dme.VendorDeliveryNo);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dme.StoreCD);
            AddParam(command, "@VendorCD", SqlDbType.VarChar, dme.VendorCD);
            AddParam(command, "@ArrivalDate", SqlDbType.VarChar, dme.ArrivalDate);
            AddParam(command, "@SoukoCD", SqlDbType.VarChar, dme.SoukoCD);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, dme.StaffCD);
            AddParam(command, "@JANCD", SqlDbType.VarChar, dme.JanCD);
            AddParam(command, "@AdminNO", SqlDbType.Int, dme.AdminNO);
            AddParam(command, "@SKUCD", SqlDbType.VarChar, dme.SKUCD);
            AddParam(command, "@MakerItem", SqlDbType.VarChar, dme.MakerItem);
            AddParam(command, "@ArrivalSu", SqlDbType.Int, dme.ArrivalSu);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command,"@Operator", SqlDbType.VarChar, dme.InsertOperator);
            AddParam(command,"@PC", SqlDbType.VarChar, dme.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutArrivalNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

          UseTransaction = true;

            bool ret= InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dme.ArrivalNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 入荷入力データ取得処理
        /// NyuukaNyuuryokuよりデータ抽出時に使用
        /// 新規時以外
        /// </summary>
        public DataTable D_Arrival_SelectData(D_Arrival_Entity de, short operationMode)
        {
            string sp = "D_Arrival_SelectData";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@ArrivalNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ArrivalNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 入荷入力データ取得処理
        /// NyuukaNyuuryokuよりデータ抽出時に使用
        /// 新規時
        /// </summary>
        public DataTable D_ArrivalPlan_SelectData(D_ArrivalPlan_Entity de, short operationMode)
        {
            string sp = "D_ArrivalPlan_SelectData";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@AdminNo", new ValuePair { value1 = SqlDbType.Int, value2 = de.AdminNO } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 入荷入力データ取得処理
        /// NyuukaNyuuryokuよりデータ抽出時に使用
        /// F10:入荷予定押下時
        /// </summary>
        public DataTable D_ArrivalPlan_SelectDataByOrderNO(D_Order_Entity de)
        {
            string sp = "D_ArrivalPlan_SelectDataByOrderNO";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.OrderNO } },
                { "@AdminNo", new ValuePair { value1 = SqlDbType.Int, value2 = de.AdminNO } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.DestinationSoukoCD } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 入荷入力　入荷番号進捗チェック　
        /// 既に出荷済み,出荷指示済み,ピッキングリスト完了済み警告
        /// </summary>
        /// <param name="arrivalNO"></param>
        /// <returns></returns>
        public DataTable CheckNyuukaData(D_Arrival_Entity de)
        {
            string sp = "CheckNyuukaData";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ArrivalNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ArrivalNO } },
            };

            return SelectData(dic, sp);
        }

        public DataTable D_Arrival_SelectAllForShiire(D_Arrival_Entity de)
        {
            string sp = "D_Arrival_SelectAllForShiire";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ArrivalDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ArrivalDateFrom } },
                { "@ArrivalDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ArrivalDateTo } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.SoukoType } },
                { "@DirectFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.DirectFLG } },
                { "@OrderKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.OrderKbn } },
            };

            return SelectData(dic, sp);
        }
        public DataTable D_Delivery_SelectAll(D_Arrival_Entity de)
        {
            string sp = "D_Delivery_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ArrivalDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ArrivalDateFrom } },
                { "@ArrivalDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ArrivalDateTo } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.SoukoType } },
                { "@DirectFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.DirectFLG } },
                { "@OrderKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.OrderKbn } },
                //{ "@VendorDeliveryNo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorDeliveryNo } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 入荷入力更新処理
        /// NyuukaNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Order_Delete(D_Arrival_Entity dme, DataTable dt, short operationMode)
        {
            string sp = "D_Order_Delete";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
       
            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dme.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dme.PC);
       
            UseTransaction = true;

            string outPutParam = "";
            return InsertUpdateDeleteData(sp, ref outPutParam);            
        }
    }
}
