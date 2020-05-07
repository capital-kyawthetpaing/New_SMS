using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class D_FTP_DL :Base_DL
    {
        public bool M_MultiPorpose_Update(M_MultiPorpose_Entity mme)
        {
            string sp = "M_MultiPorpose_Update";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mme.ID } },
                { "@Key", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.Key } },
                { "@Num1", new ValuePair { value1 = SqlDbType.Int, value2 = mme.Num1 } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.UpdateOperator } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.PC } },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }

        public DataTable D_FTP_SelectAll(string type)
        {
            string sp = "D_FTP_SelectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@type", new ValuePair { value1 = SqlDbType.Int, value2 = type } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }
        public bool InsertFiles(D_FTP_Entity dftpe)
        {
            string sp = "D_FTP_Insert";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@FTPType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dftpe.FTPType } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =dftpe.VendorCD} },
                { "@FTPFile", new ValuePair { value1 = SqlDbType.VarChar, value2 =dftpe.FTPFile } },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
