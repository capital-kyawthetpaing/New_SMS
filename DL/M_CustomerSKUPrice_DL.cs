using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;
using System.Data.SqlClient;

namespace DL
{
    public class M_CustomerSKUPrice_DL : Base_DL
    {
        public DataTable M_CustomerSKUPriceSelectData(M_CustomerSKUPrice_Entity mcskue)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@TekiyouKaisiDate_From", new ValuePair { value1 = SqlDbType.VarChar, value2 = mcskue.TekiyouKaisiDate_From} },
                { "@TekiyouKaisiDate_To", new ValuePair { value1 = SqlDbType.VarChar, value2 = mcskue.TekiyouKaisiDate_To} },
                { "@CustomerCD_From", new ValuePair { value1 =  SqlDbType.VarChar, value2 = mcskue.CustomerCD_From} },
                { "@CustomerCD_To", new ValuePair { value1 =  SqlDbType.VarChar, value2 = mcskue.CustomerCD_To} },
                { "@SKUCD_From", new ValuePair { value1 =  SqlDbType.VarChar, value2 = mcskue.SKUCD_From} },
                { "@SKUCD_To", new ValuePair { value1 =  SqlDbType.VarChar, value2 = mcskue.SKUCD_To} },
                { "@SKUName", new ValuePair { value1 =  SqlDbType.VarChar, value2 = mcskue.SKUName} },
                { "@DisplayKBN", new ValuePair { value1 =  SqlDbType.TinyInt, value2 = mcskue.DisplayKBN} },

            };
            return SelectData(dic, "M_CustomerSKUPriceSelectData");
        }

        public bool M_CustomerSKUPrice_Exec(M_CustomerSKUPrice_Entity mcskue,int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xmlCustSKUPrice", new ValuePair { value1 = SqlDbType.VarChar, value2 = mcskue.xml1} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mcskue.InsertOperator } },
                { "@Pc", new ValuePair { value1 = SqlDbType.VarChar, value2 = mcskue.PC} },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mcskue.ProgramID} },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mcskue.ProcessMode} },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mcskue.Key } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }

        };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_CustomerSKUPrice_InsertUpdate");

        }
    }
}
