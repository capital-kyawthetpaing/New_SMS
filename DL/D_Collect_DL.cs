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
    public class D_Collect_DL : Base_DL
    {
        public bool D_Collect_Insert(D_Collect_Entity dce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InputKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputKBN} },
                { "@StoreCD", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dce.StoreCD} },
                { "@StaffCD", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dce.StaffCD} },
                { "@CollectCustomerCD", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dce.CollectCustomerCD} },
                { "@PaymentMethodCD", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dce.PaymentMethodCD} },
                { "@CollectAmount", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectAmount} },
                { "@FeeDeduction", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.FeeDeduction} },
                { "@Deduction1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.Deduction1} },
                { "@Deduction2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.Deduction2} },
                { "@DeductionConfirm", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.DeductionConfirm} },
                { "@ConfirmSource", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dce.ConfirmSource} },
                { "@ConfirmAmount", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dce.ConfirmAmount} },
                { "@Remark", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dce.Remark} },
                { "@AdvanceFLG", new ValuePair { value1 =  SqlDbType.TinyInt, value2 = dce.AdvanceFLG} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StoreCD +" "+ dce.Key  } }

            };
            return InsertUpdateDeleteData(dic, "D_Collect_Insert");

        }
        /// <summary>
        /// 入金入力よりデータ抽出時に使用
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        public DataTable D_Collect_SelectAll(D_Collect_Entity dce)
        {
            string sp = "D_Collect_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CollectDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectDateFrom } },
                { "@CollectDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectDateTo } },
                { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateFrom } },
                { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StaffCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectCustomerCD } },
                { "@WebCollectType", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.WebCollectType } },
                { "@ChkZan", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.ChkZan.ToString() } },
            };
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 入金入力（修正時）よりデータ抽出時に使用
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        public DataTable D_Collect_SelectData(D_Collect_Entity dce)
        {
            string sp = "D_Collect_SelectData";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CollectNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectNO } },
                { "@ConfirmNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.ConfirmNO } },
            };
            return SelectData(dic, sp);
        }
        public DataTable SelectDataForNyukin(D_Collect_Entity dce)
        {
            string sp = "SelectDataForNyukin";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@RdoSyubetsu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.RdoSyubetsu.ToString() } },
                { "@WebCollectType", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.WebCollectType } },
                { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateFrom } },
                { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StoreCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectCustomerCD } },
            };
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 入金入力更新処理
        /// NyuukinNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Collect_Exec(D_Collect_Entity dce, DataTable dt, short operationMode)
        {
            string sp = "PRC_NyuukinNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@KidouMode", SqlDbType.Int, dce.KidouMode.ToString());
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dce.StoreCD);
            AddParam(command, "@CollectNO", SqlDbType.VarChar, dce.CollectNO);
            AddParam(command, "@ConfirmNO", SqlDbType.VarChar, dce.ConfirmNO);
            AddParam(command, "@CollectDate", SqlDbType.VarChar, dce.CollectDate);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, dce.StaffCD);
            AddParam(command, "@CollectCustomerCD", SqlDbType.VarChar, dce.CollectCustomerCD);
            AddParam(command, "@WebCollectNO", SqlDbType.VarChar, dce.WebCollectNO);
            AddParam(command, "@WebCollectType", SqlDbType.VarChar, dce.WebCollectType);
            AddParam(command, "@PaymentMethodCD", SqlDbType.VarChar, dce.PaymentMethodCD);
            AddParam(command, "@KouzaCD", SqlDbType.VarChar, dce.KouzaCD);
            AddParam(command, "@BillDate", SqlDbType.VarChar, dce.BillDate);
            AddParam(command, "@CollectClearDate", SqlDbType.VarChar, dce.CollectClearDate);

            AddParam(command, "@Head_CollectAmount", SqlDbType.Money, dce.CollectAmount);
            AddParam(command, "@FeeDeduction", SqlDbType.Money, dce.FeeDeduction);
            AddParam(command, "@Deduction1", SqlDbType.Money, dce.Deduction1);
            AddParam(command, "@Deduction2", SqlDbType.Money, dce.Deduction2);
            AddParam(command, "@DeductionConfirm", SqlDbType.Money, dce.DeductionConfirm);
            AddParam(command, "@ConfirmSource", SqlDbType.Money, dce.ConfirmSource);
            AddParam(command, "@Head_ConfirmAmount", SqlDbType.Money, dce.ConfirmAmount);

            AddParam(command, "@Remark", SqlDbType.VarChar, dce.Remark);
            AddParam(command, "@DetailCount", SqlDbType.Int, dt.Rows.Count.ToString());

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dce.Operator);
            AddParam(command, "@PC", SqlDbType.VarChar, dce.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutCollectNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dce.CollectNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 入金検索にて使用
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        public DataTable D_Collect_SelectForSearch(D_Collect_Entity dce)
        {
            string sp = "D_Collect_SelectForSearch";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CollectDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectDateFrom } },
                { "@CollectDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectDateTo } },
                { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateFrom } },
                { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateTo } },
                //{ "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StoreCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectCustomerCD } },
            };
            return SelectData(dic, sp);
        }

        public DataTable D_Collect_SelectMaeukeKin(D_Collect_Entity dce)
        {
            string sp = "D_Collect_SelectMaeukeKin";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StoreCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectCustomerCD } },
                { "@AdvanceFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.AdvanceFLG } },
            };
            return SelectData(dic, sp);
        }
    }
}
