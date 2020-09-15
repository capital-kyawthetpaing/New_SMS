using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;
namespace DL
{
    public class TenjikaiJuuChuu_DL : Base_DL
    {
        public TenjikaiJuuChuu_DL()
        {

        }

        public DataTable JuuChuuCheck(string Bi)
        {
            string sp = "JuuChuuBiCheck";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Bi", new ValuePair { value1 = SqlDbType.Date, value2 = Bi } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }
        public DataTable ShuukaSouko(string SouKoCD, string ChangeDate)
        {
            string sp = "ShuukaSoukoCheck";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = ChangeDate } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =  SouKoCD } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }
        public DataTable M_TenjiKaiJuuChuu_Select(string xml)
        {
            string sp = "M_TenjiKaiJuuChuu_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.Date, value2 = xml } }
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }



    }
}
