using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class ZaikoReplicaSyori_DL : Base_DL
    {
        public bool D_StockReplica_Insert()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {

            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "D_StockReplica_Insert");
        }
    }
}
