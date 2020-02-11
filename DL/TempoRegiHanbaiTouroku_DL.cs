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
    public class TempoRegiHanbaiTouroku_DL : Base_DL
    {

        /// <summary>
        /// 店舗レジ出荷売上入力更新処理
        /// TempoRegiHanbaiTourokuより更新時に使用
        /// </summary>
        /// <param name="dse"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool PRC_TempoRegiHanbaiTouroku(D_Sales_Entity dse, D_StorePayment_Entity dspe, DataTable dt, short operationMode)
        {
            string sp = "PRC_TempoRegiHanbaiTouroku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int,operationMode.ToString());
            AddParam(command, "@SalesNO", SqlDbType.VarChar, dse.SalesNO);
            AddParam(command, "@JuchuuNO", SqlDbType.VarChar, dse.JuchuuNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dse.StoreCD);
            AddParam(command, "@CustomerCD", SqlDbType.VarChar, dse.CustomerCD);
            AddParam(command, "@Age", SqlDbType.TinyInt, dse.Age);

            AddParam(command, "@SalesRate", SqlDbType.Int, dspe.SalesRate);
            AddParam(command, "@AdvanceAmount", SqlDbType.Money, dspe.AdvanceAmount);
            AddParam(command, "@PointAmount", SqlDbType.Money, dspe.PointAmount);
            AddParam(command, "@CashAmount", SqlDbType.Money, dspe.CashAmount);
            AddParam(command, "@RefundAmount", SqlDbType.Money, dspe.RefundAmount);
            AddParam(command, "@DenominationCD", SqlDbType.VarChar, dspe.CardDenominationCD);
            AddParam(command, "@CardAmount", SqlDbType.Money, dspe.CardAmount);
            AddParam(command, "@Discount", SqlDbType.Money, dspe.DiscountAmount);
            AddParam(command, "@CreditAmount", SqlDbType.Money, dspe.CreditAmount);
            AddParam(command, "@Denomination1Amount", SqlDbType.Money, dspe.Denomination1Amount);
            AddParam(command, "@DenominationCD1", SqlDbType.VarChar, dspe.Denomination1CD);
            AddParam(command, "@Denomination2Amount", SqlDbType.Money, dspe.Denomination2Amount);
            AddParam(command, "@DenominationCD2", SqlDbType.VarChar, dspe.Denomination2CD);
            AddParam(command, "@DepositAmount", SqlDbType.Money, dspe.DepositAmount);
            AddParam(command, "@Keijobi", SqlDbType.VarChar, dse.SalesDate);
            
            AddParam(command, "@SalesGaku", SqlDbType.Money, dse.SalesGaku);
            AddParam(command, "@SalesTax", SqlDbType.Money, dse.SalesTax);
            AddParam(command, "@SalesHontaiGaku0", SqlDbType.Money, dse.SalesHontaiGaku0);
            AddParam(command, "@SalesHontaiGaku8", SqlDbType.Money, dse.SalesHontaiGaku8);
            AddParam(command, "@SalesHontaiGaku10", SqlDbType.Money, dse.SalesHontaiGaku10);
            AddParam(command, "@HanbaiTax10", SqlDbType.Money, dse.SalesTax10);
            AddParam(command, "@HanbaiTax8", SqlDbType.Money, dse.SalesTax8);
            AddParam(command, "@InvoiceGaku", SqlDbType.Money, dspe.BillingAmount);
            AddParam(command, "@Discount8", SqlDbType.Money, dse.Discount8);
            AddParam(command, "@Discount10", SqlDbType.Money, dse.Discount10);
            AddParam(command, "@DiscountTax", SqlDbType.Money, dse.DiscountTax);
            AddParam(command, "@DiscountTax8", SqlDbType.Money, dse.DiscountTax8);
            AddParam(command, "@DiscountTax10", SqlDbType.Money, dse.DiscountTax10);
            AddParam(command, "@TotalAmount", SqlDbType.Money, dspe.TotalAmount);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command,"@Operator", SqlDbType.VarChar, dse.Operator);
            AddParam(command,"@PC", SqlDbType.VarChar, dse.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutSalesNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret= InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dse.SalesNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 店舗レジ データ更新
        /// </summary>
        /// <param name="dse"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool PRC_TempoRegiDataUpdate(D_Sales_Entity dse,  int operationMode)
        {
            string sp = "PRC_TempoRegiDataUpdate";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@SalesNO", SqlDbType.VarChar, dse.SalesNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dse.StoreCD);
            AddParam(command, "@Operator", SqlDbType.VarChar, dse.Operator);
            AddParam(command, "@PC", SqlDbType.VarChar, dse.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutCollectPlanNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dse.SalesNO = outPutParam;

            return ret;
        }
    }
}
