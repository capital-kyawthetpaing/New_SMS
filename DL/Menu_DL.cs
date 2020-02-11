using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace DL
{
   public class Menu_DL:Base_DL
    {

        public Menu_DL()
        {

        }
        public DataTable D_MenuMessageSelect(string Staff_CD)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@Staff_CD", new ValuePair { value1 = SqlDbType.VarChar, value2 = Staff_CD });

            UseTransaction = true;
            return SelectData(dic, "D_MenuMessageSelect");
        }

        public DataTable getMenuNo(string Staff_CD)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@Staff_CD", new ValuePair { value1 = SqlDbType.VarChar, value2 = Staff_CD });

            UseTransaction = true;
            return SelectData(dic, "HMENU");
        }

    }
}
