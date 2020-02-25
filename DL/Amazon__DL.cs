using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
namespace DL
{
   public class Amazon__DL:Base_DL
    {

        public Amazon__DL() { }

        public DataTable Allow_Check()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
            };
            return SelectData(dic, "Allow_Check");
        }


    }
}
