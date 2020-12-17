using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class D_FBData_DL:Base_DL
    {
        public bool D_FBData_Insert(D_FBData_Entity dfde)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PayeeCD", new ValuePair { value1 = SqlDbType.Date, value2 = dfde.PayeeCD } },
                { "@PayeeName", new ValuePair { value1 = SqlDbType.Date, value2 = dfde.PayeeName } },
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.BankCD } },
                { "@BranchCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.BranchCD } },
                { "@KouzaKBN", new ValuePair { value1 = SqlDbType.Date, value2 = dfde.KouzaKBN } },
                { "@KouzaNO", new ValuePair { value1 = SqlDbType.Date, value2 = dfde.KouzaNO } },
                { "@KouzaMeigi", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.KouzaMeigi } },
                { "@PayGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.PayGaku } },
                { "@TransferGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.TransferGaku } },
                { "@TransferFee", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.TransferFee } },
                { "@TransferFeeKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.TransferFeeKBN } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dfde.StaffCD } },
            };

            return InsertUpdateDeleteData(dic, "D_FBData_Insert");
        }
    }
}
