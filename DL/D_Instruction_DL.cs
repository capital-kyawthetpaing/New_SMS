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
    public class D_Instruction_DL : Base_DL
    {
        /// <summary>
        /// 出荷指示番号検索
        /// </summary>
        /// <param name="die"></param>
        /// <returns></returns>
        public DataTable D_Instruction_SelectAll(D_Instruction_Entity die)
        {
            string sp = "D_Instruction_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InstructionDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.@InstructionDateFrom } },
                { "@InstructionDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.InstructionDateTo } },
                { "@DeliveryPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryPlanDateFrom } },
                { "@DeliveryPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryPlanDateTo } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliverySoukoCD } },
                { "@Chk1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk1.ToString() } },
                { "@Chk2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk2.ToString() } },
                { "@Chk3", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk3.ToString() } },
                { "@Chk4", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk4.ToString() } },
                { "@Chk5", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk5.ToString() } },
                { "@DeliveryName", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryName } },
                { "@CarrierCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.CarrierCD } },
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.JuchuuNO } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.PrintStaffCD } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 出荷指示入力更新処理
        /// ShukkaShijiTourokuより更新時に使用
        /// </summary>
        /// <param name="die"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Instruction_Exec(D_Instruction_Entity die, DataTable dt)
        {
            string sp = "PRC_ShukkaShijiTouroku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;
            
            AddParam(command, "@StoreCD", SqlDbType.VarChar, die.StoreCD);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, die.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, die.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutInstructionNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                die.InstructionNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 出荷指示入力データ取得処理
        /// ShukkaShijiTourokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Instruction_SelectData(D_Instruction_Entity die)
        {
            string sp = "D_Instruction_SelectData";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DeliveryPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryPlanDateFrom } },
                { "@DeliveryPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryPlanDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.StoreCD } },
                { "@Chk1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk1.ToString() } },
                { "@Chk2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk2.ToString() } },
                { "@Chk3", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk3.ToString() } },
                { "@Chk4", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk4.ToString() } },
                { "@Chk5", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk5.ToString() } },
                { "@DeliveryName", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryName } },
                { "@ChkHakkozumi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.ChkHakkozumi.ToString() } },
                { "@ChkSyukkazumi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.ChkSyukkazumi.ToString() } },
                { "@ChkSyukkaFuka", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.ChkSyukkaFuka.ToString() } },
            };

            return SelectData(dic, sp);
        }


        /// <summary>
        /// 出荷入力データチェック処理
        /// ShukkaNyuuryokuより出荷指示データチェック時に使用
        /// </summary>
        public DataTable CheckInstruction(D_Instruction_Entity de)
        {
            string sp = "CheckInstruction";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InstructionNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InstructionNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 出荷入力データ取得処理
        /// ShukkaNyuuryokuより出荷指示データ抽出時に使用
        /// 新規時
        /// </summary>
        public DataTable D_Instruction_SelectDataForShukka(D_Instruction_Entity de, string shippingDate)
        {
            string sp = "D_Instruction_SelectDataForShukka";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InstructionNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InstructionNO } },
                { "@ShippingDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = shippingDate } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 出荷指示書より出荷指示データの存在チェック
        /// </summary>
        /// <param name="die"></param>
        /// <returns></returns>
        public DataTable D_Instruction_Select(D_Instruction_Entity die) 
        {
            string sp = "D_Instruction_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InstructionNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.InstructionNO } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliverySoukoCD } },
            };
            return SelectData(dic, sp);
        }
        /// </summary>
        /// <param name="die"></param>
        /// <returns></returns>
        public DataTable D_Instruction_SelectForPrint(D_Instruction_Entity die)
        {
            string sp = "D_Instruction_SelectForPrint";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DeliveryPlanDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryPlanDate } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliverySoukoCD } },
                { "@Chk1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk1.ToString() } },
                { "@Chk2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk2.ToString() } },
                { "@Chk3", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk3.ToString() } },
                { "@Chk4", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk4.ToString() } },
                { "@Chk5", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk5.ToString() } },
                { "@CarrierCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.CarrierCD } },
                { "@InstructionNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.InstructionNO  } },
                { "@ChkMihakko", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.ChkMihakko.ToString() } },
                { "@ChkSaihakko", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.ChkSaihakko.ToString() } },
            };
            return SelectData(dic, sp);
        }
        /// <summary>
        /// 出荷指示書発行済みFLGの更新処理
        /// </summary>
        /// <param name="de"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool D_Instruction_Update(D_Instruction_Entity de, DataTable dt)
        {
            string sp = "D_Instruction_Update";
            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;
            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, de.UpdateOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, de.PC);
            UseTransaction = true;
            string outPutParam = "";    //未使用
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            return ret;
        }

        /// <summary>
        /// 出荷指示入力データ取得処理
        /// ShukkaSiziTourokuFromJuchuuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Instruction_SelectDataFromJuchu(D_Instruction_Entity die)
        {
            string sp = "D_Instruction_SelectDataFromJuchu";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DeliveryPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryPlanDateFrom } },
                { "@DeliveryPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryPlanDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.StoreCD } },
                { "@Chk1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk1.ToString() } },
                { "@Chk2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk2.ToString() } },
                //{ "@Chk3", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk3.ToString() } },
                //{ "@Chk4", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk4.ToString() } },
                { "@Chk5", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.Chk5.ToString() } },
                { "@DeliveryName", new ValuePair { value1 = SqlDbType.VarChar, value2 = die.DeliveryName } },
                { "@ChkHakkozumi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.ChkHakkozumi.ToString() } },
                { "@ChkSyukkazumi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.ChkSyukkazumi.ToString() } },
                { "@ChkSyukkaFuka", new ValuePair { value1 = SqlDbType.TinyInt, value2 = die.ChkSyukkaFuka.ToString() } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 出荷指示入力更新処理
        /// ShukkaSiziTourokuFromJuchuuより更新時に使用
        /// </summary>
        /// <param name="die"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Instruction_ExecFromJuchu(D_Instruction_Entity die, DataTable dt)
        {
            string sp = "PRC_ShukkaSiziTourokuFromJuchuu";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@StoreCD", SqlDbType.VarChar, die.StoreCD);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, die.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, die.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutInstructionNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                die.InstructionNO = outPutParam;

            return ret;
        }
    }
}
