using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class ZaikoReplicaSyori_BL : Base_BL
    {
        ZaikoReplicaSyori_DL zrsdl;

        public ZaikoReplicaSyori_BL()
        {
            zrsdl = new ZaikoReplicaSyori_DL();
        }

        public bool D_StockReplica_Insert()
        {
            return zrsdl.D_StockReplica_Insert();
        }
    }
}
