using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
public   class D_Mail_DL : Base_DL
    {
        public DataTable D_Mail_Select()
        {
            string sp = "D_Mail_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                //{"@Mode",new ValuePair{value1=SqlDbType.TinyInt,value2=Mode.ToString()} },
                //{"@MailCount",new ValuePair{value1=SqlDbType.TinyInt,value2=MailCount.ToString()}}
            };
            return SelectData(dic,sp);
        }

        public bool D_MailSend_Update(int MailCount)
        {
            string sp = "D_MailSend_Update";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MailCount",new ValuePair{value1=SqlDbType.TinyInt,value2=MailCount.ToString()}}
            };
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
