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
public   class D_Mail_DL : Base_DL
    {
        public DataTable D_Mail_SelectAll(D_Mail_Entity de)
        {
            string sp = "D_Mail_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MailDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.MailDateFrom } },
                { "@MailDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.MailDateTo } },
                { "@MailTimeFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.MailTimeFrom } },
                { "@MailTimeTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.MailTimeTo } },
                { "@MailType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.MailType } },
                { "@MailKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.MailKBN } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CustomerCD } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorCD } },
            };

            return SelectData(dic, sp);
        }
        public DataTable D_Mail_Select()
        {
            string sp = "D_Mail_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                //{"@Mode",new ValuePair{value1=SqlDbType.TinyInt,value2=Mode.ToString()} }
            };
            return SelectData(dic,sp);
        }

        public bool D_MailSend_Update(int MailCount)
        {
            string sp = "D_MailSend_Update";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MailCount",new ValuePair{value1=SqlDbType.Int,value2=MailCount.ToString()}}
            };
            return InsertUpdateDeleteData(dic, sp);
        }


        ///Mail Receive
        public DataTable M_ReceiveMailServer_Select()
        {
            string sp = "M_ReceiveMailServer_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                //{"@Mode",new ValuePair{value1=SqlDbType.TinyInt,value2=Mode.ToString()} }
            };
            return SelectData(dic, sp);
        }
        public DataTable M_ReceiveOfVendorMail_Select(string AddressFrom)
        {
            string sp = "M_ReceiveOfVendorMail_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@AddressFrom",new ValuePair{value1=SqlDbType.VarChar,value2=AddressFrom} }
            };
            return SelectData(dic, sp);
        }
    }
}
