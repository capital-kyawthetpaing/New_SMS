using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;

namespace DL
{
    public class M_Message_DL : Base_DL
    {
        /// <summary>
        /// select Message Information
        /// </summary>
        /// <param name="mme">Message Entity</param>
        /// <returns>return Datatable select by MessageID</returns>
        public DataTable M_Message_Select(M_Message_Entity mme)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();
            dic.Add("@MessageID", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.MessageID });
            return SelectData(dic, "M_Message_Select");
        }
    }
}
