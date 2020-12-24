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
    public class D_FBControl_DL:Base_DL
    {
        public bool FBDataSakusei_Insert(D_FBControl_Entity dfe,D_FBData_Entity dfde,D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PayDate", new ValuePair { value1 = SqlDbType.Date, value2 = dfe.PayDate } },
                { "@ActualPayDate", new ValuePair { value1 = SqlDbType.Date, value2 = dfe.ActualPayDate } },
                { "@MotoKouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfe.MotoKouzaCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfe.StoreCD } },

                { "@PayeeCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.PayeeCD } },
                { "@PayeeName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.PayeeName } },
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.BankCD } },
                { "@BranchCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.BranchCD } },
                { "@KouzaKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dfde.KouzaKBN } },
                { "@KouzaNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.KouzaNO } },
                { "@KouzaMeigi", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.KouzaMeigi } },
                { "@PayGaku", new ValuePair { value1 = SqlDbType.Money, value2 = dfde.PayGaku } },
                { "@TransferGaku", new ValuePair { value1 = SqlDbType.Money, value2 = dfde.TransferGaku } },
                { "@TransferFee", new ValuePair { value1 = SqlDbType.Money, value2 = dfde.TransferFee } },
                { "@TransferFeeKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dfde.TransferFeeKBN } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfe.StoreCD } },

                {"@Flg", new ValuePair {value1 = SqlDbType.TinyInt,value2 = dpe.Flg} }
            };

            return InsertUpdateDeleteData(dic, "D_FBControl_Insert");
        }

        public bool FBDataSakusei_Update(D_FBControl_Entity dfe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            { 
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfe.Operator } },               
            };
            return InsertUpdateDeleteData(dic, "FBDataSakusei_Update");
        }

    }
}
