using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class D_DepositHistory_DL : Base_DL
    {

        public bool D_DepositＨistory_Insert_Update(D_DepositHistory_Entity mre)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.StoreCD } },
                { "@DenominationCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.DenominationCD} },
                { "@DepositKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.DepositKBN} },
                { "@DepositGaku", new ValuePair { value1 = SqlDbType.Money, value2 = mre.DepositGaku} },
                { "@Remark", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Remark } },
                { "@DataKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.DataKBN } },
                { "@ExchangeMoney", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ExchangeMoney } },
                { "@ExchangeDenomination", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ExchangeDenomination } },
                { "@ExchangeCount", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ExchangeCount } },
                { "@AdminNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.AdminNO } },
                { "@SalesSU", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesSU } },
                { "@SalesUnitPrice", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesUnitPrice } },
                { "@SalesGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesGaku } },
                { "@SalesTax", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesTax } },
                { "@TotalGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.TotalGaku } },
                { "@Refund", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Refund } },
                { "@IsIssued", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.IsIssued } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.StoreCD +" "+ mre.Key  } },
                { "@CancelKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.CancelKBN } },
                { "@RecordKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.RecoredKBN } },
                { "@AccountingDate", new ValuePair { value1 = SqlDbType.Date, value2 = mre.AccountingDate } },
                { "@Rows", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.Rows } },
                { "@SalesTaxRate", new ValuePair { value1 = SqlDbType.Int, value2 = mre.SalesTaxRate } },
            };
            return InsertUpdateDeleteData(dic, "D_DepositHistory_InsertUpdate");
        }
        public bool TempoRegiRyougaeNyuuryoku_Insert(D_DepositHistory_Entity mre)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.StoreCD } },
                { "@DataKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.DataKBN } },
                { "@DepositKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.DepositKBN } },
                { "@DepositKBN1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.DepositKBN1 } },
                { "@DenominationCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.DenominationCD } },
                { "@Remark", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Remark } },
                { "@ExchangeMoney", new ValuePair { value1 = SqlDbType.Money, value2 = mre.ExchangeMoney} },
                { "@ExchangeDenomination", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ExchangeDenomination} },
                { "@ExchangeCount", new ValuePair { value1 = SqlDbType.Int, value2 = mre.ExchangeCount} },
                { "@SalesSU", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesSU } },
                { "@SalesUnitPrice", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesUnitPrice } },
                { "@SalesGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesGaku } },
                { "@SalesTax", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesTax } },
                { "@TotalGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.TotalGaku } },
                { "@Refund", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Refund } },
                { "@ProperGaku",new ValuePair{value1=SqlDbType.VarChar,value2=mre.ProperGaku} },
                { "@DiscountGaku",new ValuePair{value1=SqlDbType.VarChar,value2=mre.DiscountGaku} },
                { "@CustomerCD",new ValuePair{value1=SqlDbType.VarChar,value2=mre.CustomerCD} },
                { "@IsIssued", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.IsIssued } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Key } },
                { "@CancelKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.CancelKBN } },
                { "@RecoredKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.RecoredKBN } },
                { "@Rows", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.Rows } },
                { "@SalesTaxRate", new ValuePair { value1 = SqlDbType.Int, value2 = mre.SalesTaxRate } },
                };
            return InsertUpdateDeleteData(dic, "D_DepositHistory_Insert");
        }
        
        public bool TempoRegiTsurisenJyunbi_Insert(D_DepositHistory_Entity mre)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.StoreCD } },
                { "@DataKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.DataKBN } },
                { "@DepositKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.DepositKBN } },
                { "@DenominationCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.DenominationCD} },
                { "@DepositGaku", new ValuePair { value1 = SqlDbType.Money, value2 = mre.DepositGaku} },
                { "@Remark", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Remark } },
                { "@ExchangeMoney", new ValuePair { value1 = SqlDbType.Money, value2 = mre.ExchangeMoney} },
                { "@ExchangeDenomination", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ExchangeDenomination} },
                { "@ExchangeCount", new ValuePair { value1 = SqlDbType.Int, value2 = mre.ExchangeCount} },
                { "@SalesSU", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesSU } },
                { "@SalesUnitPrice", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesUnitPrice } },
                { "@SalesGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesGaku } },
                { "@SalesTax", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.SalesTax } },
                { "@TotalGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.TotalGaku } },
                { "@Refund", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Refund } },
                { "@IsIssued", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.IsIssued } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Key  } },
                { "@CancelKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.CancelKBN } },
                { "@RecordKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.RecoredKBN } },
                { "@AccountingDate", new ValuePair { value1 = SqlDbType.Date, value2 = mre.AccountingDate } },
                { "@Rows", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.Rows } },
                { "@SalesTaxRate", new ValuePair { value1 = SqlDbType.Int, value2 = mre.SalesTaxRate } },
                { "@AdminNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.AdminNO } },
                };
            return InsertUpdateDeleteData(dic, "D_DepositHistory_InsertUpdate");
        }

        public DataTable D_DepositＨistory_Gaku_Select(D_DepositHistory_Entity mre)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DepositKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.DepositKBN} },
                { "@DepositKBN1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mre.DepositKBN1} },
            };
            return SelectData(dic,"D_DepositＨistory_Gaku_Select");
        }

    }
}
