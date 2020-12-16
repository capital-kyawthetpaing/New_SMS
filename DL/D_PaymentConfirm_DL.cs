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
    public class D_PaymentConfirm_DL : Base_DL
    {
        /// <summary>
        /// 入金入力（修正時）よりデータ抽出時に使用
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        public DataTable D_PaymentConfirm_Select(D_Collect_Entity dce)
        {
            string sp = "D_PaymentConfirm_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CollectNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectNO } },
                { "@ConfirmNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.ConfirmNO } },
            };
            return SelectData(dic, sp);
        }
        //public DataTable SelectDataForNyukin(D_Collect_Entity dce)
        //{
        //    string sp = "SelectDataForNyukin";

        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        { "@RdoSyubetsu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.RdoSyubetsu.ToString() } },
        //        { "@WebCollectType", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.WebCollectType } },
        //        { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateFrom } },
        //        { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateTo } },
        //        { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StoreCD } },
        //        { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectCustomerCD } },
        //    };
        //    return SelectData(dic, sp);
        //}

        ///// <summary>
        ///// 入金入力更新処理
        ///// NyuukinNyuuryokuより更新時に使用
        ///// </summary>
        ///// <param name="dme"></param>
        ///// <param name="operationMode"></param>
        ///// <returns></returns>
        //public bool D_Collect_Exec(D_Collect_Entity dce, DataTable dt, short operationMode)
        //{
        //    string sp = "PRC_NyuukinNyuuryoku";

        //    command = new SqlCommand(sp, GetConnection());
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.CommandTimeout = 0;

        //    AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
        //    AddParam(command, "@OrderNO", SqlDbType.VarChar, dce.CollectNO);
        //    AddParam(command, "@StoreCD", SqlDbType.VarChar, dce.StoreCD);
        //    AddParam(command, "@OrderDate", SqlDbType.VarChar, dce.CollectDate);
        //    //AddParam(command, "@ReturnFLG", SqlDbType.TinyInt, dce.ReturnFLG);
        //    //AddParam(command, "@SoukoCD", SqlDbType.VarChar, dce.DestinationSoukoCD);
        //    AddParam(command, "@StaffCD", SqlDbType.VarChar, dce.StaffCD);
        //    AddParam(command, "@OrderCD", SqlDbType.VarChar, dce.CollectCustomerCD);
        //    //AddParam(command, "@OrderPerson", SqlDbType.VarChar, dce.OrderPerson);
        //    //AddParam(command, "@AliasKBN", SqlDbType.TinyInt, dce.AliasKBN);
        //    //AddParam(command, "@DestinationKBN", SqlDbType.TinyInt, dce.DestinationKBN);
        //    //AddParam(command, "@DestinationName", SqlDbType.VarChar, dce.DestinationName);
        //    //AddParam(command, "@ZipCD1", SqlDbType.VarChar, dce.DestinationZip1CD);
        //    //AddParam(command, "@ZipCD2", SqlDbType.VarChar, dce.DestinationZip2CD);
        //    //AddParam(command, "@Address1", SqlDbType.VarChar, dce.DestinationAddress1);
        //    //AddParam(command, "@Address2", SqlDbType.VarChar, dce.DestinationAddress2);
        //    //AddParam(command, "@DestinationTelphoneNO", SqlDbType.VarChar, dce.DestinationTelphoneNO);
        //    //AddParam(command, "@DestinationFaxNO", SqlDbType.VarChar, dce.DestinationFaxNO);
        //    //AddParam(command, "@DestinationSoukoCD", SqlDbType.VarChar, dce.DestinationSoukoCD);

        //    //AddParam(command, "@OrderHontaiGaku", SqlDbType.Money, dce.OrderHontaiGaku);
        //    //AddParam(command, "@OrderTax8", SqlDbType.Money, dce.OrderTax8);
        //    //AddParam(command, "@OrderTax10", SqlDbType.Money, dce.OrderTax10);
        //    //AddParam(command, "@OrderGaku", SqlDbType.Money, dce.OrderGaku);
        //    //AddParam(command, "@CommentOutStore", SqlDbType.VarChar, dce.CommentOutStore);
        //    //AddParam(command, "@CommentInStore", SqlDbType.VarChar, dce.CommentInStore);
        //    //AddParam(command, "@ApprovalEnabled", SqlDbType.TinyInt, dce.ApprovalEnabled);
        //    //AddParam(command, "@ApprovalStageFLG", SqlDbType.Int, dce.ApprovalStageFLG);

        //    AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
        //    AddParam(command, "@Operator", SqlDbType.VarChar, dce.Operator);
        //    AddParam(command, "@PC", SqlDbType.VarChar, dce.PC);

        //    //OUTパラメータの追加
        //    string outPutParam = "@OutOrderNo";
        //    command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
        //    command.Parameters[outPutParam].Direction = ParameterDirection.Output;

        //    UseTransaction = true;

        //    bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
        //    if (ret)
        //        dce.CollectNO = outPutParam;

        //    return ret;
        //}
        /// <summary>
        /// 入金消込検索にて使用
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        public DataTable D_PaymentConfirm_SelectForSearch(D_PaymentConfirm_Entity dce)
        {
            string sp = "D_PaymentConfirm_SelectForSearch";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CollectClearDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectClearDateFrom } },
                { "@CollectClearDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CollectClearDateTo } },
                { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateFrom } },
                { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.InputDateTo } },
            };
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 入出金予定表印刷
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_PaymentConfirm_SelectForPrint(D_PaymentConfirm_Entity de)
        {
            string sp = "D_PaymentConfirm_SelectForPrint";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InputDateFrom } },
                { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InputDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
            };
            return SelectData(dic, sp);
        }
    }
}
