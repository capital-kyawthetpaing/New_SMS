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
    public class D_InventoryControl_DL : Base_DL
    {
        /// <summary>
        /// 棚卸締処理チェック処理
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public DataTable D_InventoryControl_Select(D_InventoryProcessing_Entity de)
        {
            string sp = "D_InventoryControl_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InventoryDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InventoryDate } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
                {"@TanaCDFrom",new ValuePair { value1=SqlDbType.VarChar,value2=de.FromRackNO} },
                {"@TanaCDTo",new ValuePair { value1=SqlDbType.VarChar,value2=de.ToRackNO} },
                //{"@InventoryKBN",new ValuePair { value1=SqlDbType.VarChar,value2=de.InventoryKBN} },
            };

            return SelectData(dic, sp);
        }
        public DataTable D_InventoryControl_SelectForTanaoroshiNyuuryoku(D_InventoryControl_Entity de)
        {
            string sp = "D_InventoryControl_SelectForTanaoroshiNyuuryoku";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InventoryDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InventoryDate } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
                { "@RackNO",new ValuePair { value1=SqlDbType.VarChar,value2=de.RackNO} },
            };

            return SelectData(dic, sp);
        }


    }
}
