using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using System.Data.SqlClient;

namespace DL
{
    public class D_Shipping_DL: Base_DL
    {
        D_ShippingDetails_Entity dsde = new D_ShippingDetails_Entity();
        D_Shipping_Entity dshe = new D_Shipping_Entity();
        D_Instruction_Entity die = new D_Instruction_Entity();
        public DataTable D_Shipping_Select(D_Shipping_Entity dshe,D_ShippingDetails_Entity dsde, D_Instruction_Entity die)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@SoukoCD", new ValuePair { value1 = SqlDbType.NVarChar, value2 = dshe.SoukoCD } },
                {"@ShippingStartDate", new ValuePair {value1 = SqlDbType.Date, value2= dshe.ShippingDateFrom.Replace("/","-")} },
                {"@ShippingEndDate", new ValuePair {value1 = SqlDbType.Date , value2 = dshe.ShippingDateTo.Replace("/","-")} },
                {"@Number", new ValuePair {value1 = SqlDbType.VarChar ,value2 = dsde.Number} },
                {"@DeliverySoukoCD", new ValuePair {value1 = SqlDbType.NVarChar , value2 = die.DeliverySoukoCD} },
                {"@CarrierCD", new ValuePair {value1 = SqlDbType.NVarChar, value2 = dshe.CarrierCD} },           
                { "@ITEMCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsde.ItemCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsde.JanCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsde.SKUCD } },
                {"@ShippingKBN", new ValuePair {value1 = SqlDbType.TinyInt ,value2 = dshe.ShippingKBN} },
                {"@InvoiceNO", new ValuePair {value1 = SqlDbType.TinyInt ,value2 = dshe.InvoiceNO} },
               
            };
            UseTransaction = true;
            return SelectData(dic, "D_Shipping_Select");
        }

        /// <summary>	
        /// 出荷入力更新処理	
        /// NyuukaNyuuryokuより更新時に使用	
        /// </summary>	
        /// <param name="dme"></param>	
        /// <param name="operationMode"></param>	
        /// <returns></returns>	
        public bool D_Shipping_Exec(D_Shipping_Entity dse, DataTable dt, short operationMode, string storeCD)
        {
            string sp = "PRC_ShukkaNyuuryoku";
            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;
            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@ShippingNO", SqlDbType.VarChar, dse.ShippingNO);
            AddParam(command, "@ShippingDate", SqlDbType.VarChar, dse.ShippingDate);
            AddParam(command, "@ShippingKBN", SqlDbType.TinyInt, dse.ShippingKBN);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, storeCD);
            AddParam(command, "@InstructionNO", SqlDbType.VarChar, dse.InstructionNO);
            AddParam(command, "@SoukoCD", SqlDbType.VarChar, dse.SoukoCD);
            AddParam(command, "@CarrierCD", SqlDbType.VarChar, dse.CarrierCD);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, dse.StaffCD);
            AddParam(command, "@UnitsCount", SqlDbType.TinyInt, dse.UnitsCount);
            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dse.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dse.PC);
            //OUTパラメータの追加	
            string outPutParam = "@OutShippingNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;
            UseTransaction = true;
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dse.ShippingNO = outPutParam;
            return ret;
        }
        /// <summary>	
        /// 出荷番号検索	
        /// </summary>	
        /// <param name="de"></param>	
        /// <returns></returns>	
        public DataTable D_Shipping_SelectAll(D_Shipping_Entity de)
        {
            string sp = "D_Shipping_SelectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ShippingDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ShippingDateFrom } },
                { "@ShippingDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ShippingDateTo } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JanCD } },
            };
            return SelectData(dic, sp);
        }
        /// <summary>	
        /// 出荷入力データ取得処理	
        /// ShukkaNyuuryokuよりデータ抽出時に使用	
        /// 新規時以外	
        /// </summary>	
        public DataTable D_Shipping_SelectData(D_Shipping_Entity de)
        {
            string sp = "D_Shipping_SelectData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ShippingNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ShippingNO } },
            };
            return SelectData(dic, sp);
        }
        /// <summary>	
        ///出荷入力　出荷番号チェック	
        /// </summary>	
        /// <param name="ShippingNO"></param>	
        /// <returns></returns>	
        public DataTable CheckShipping(D_Shipping_Entity de)
        {
            string sp = "CheckShipping";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ShippingNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ShippingNO } },
            };
            return SelectData(dic, sp);
        }
        /// <summary>	
        ///出荷入力　出荷済み・売上済みチェック	
        /// </summary>	
        /// <param name="ShippingNO"></param>	
        /// <returns></returns>	
        public DataTable CheckShukkaData(D_Shipping_Entity de)
        {
            string sp = "CheckShukkaData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ShippingNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ShippingNO } },
                { "@InstructionNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InstructionNO } },
            };
            return SelectData(dic, sp);
        }
        /// <summary>	
        /// 出荷売上データ更新照会にて使用	
        /// </summary>	
        /// <returns></returns>	
        public DataTable D_Shipping_SelectAllForShoukai()
        {
            string sp = "D_Shipping_SelectAllForShoukai";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ShippingNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = "" } },
            };
            return SelectData(dic, sp);
        }
        /// <summary>
        /// ヤマト送り状データ抽出時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Shipping_SelectForYamato(D_Shipping_Entity de)
        {
            string sp = "D_Shipping_SelectForYamato";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ShippingNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ShippingNO } },
            };

            return SelectData(dic, sp);
        }
        public bool D_Shipping_Update(D_Shipping_Entity de)
        {
            string sp = "D_Shipping_Update";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Operator }},
                { "@ShippingNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ShippingNO } },
            };

            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
