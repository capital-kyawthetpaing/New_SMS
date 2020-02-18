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
    public class D_InventoryProcessing_DL : Base_DL
    {
        /// <summary>
        /// 棚卸締処理データ取得処理
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public DataTable D_InventoryProcessing_SelectAll(D_InventoryProcessing_Entity de)
        {
            string sp = "D_InventoryProcessing_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
        };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 棚卸締処理チェック処理
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public DataTable D_InventoryProcessing_Select(D_InventoryProcessing_Entity de)
        {
            string sp = "D_InventoryProcessing_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
                { "@InventoryDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InventoryDate } },
                { "@TanaCDFrom",new ValuePair { value1=SqlDbType.VarChar,value2=de.FromRackNO} },
                { "@TanaCDTo",new ValuePair { value1=SqlDbType.VarChar,value2=de.ToRackNO} },
                { "@InventoryKBN",new ValuePair { value1=SqlDbType.VarChar,value2=de.InventoryKBN} },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 棚卸締処理
        /// TanaoroshiShimeShoriより更新時に使用
        /// </summary>
        /// <param name = "de" ></ param >
        /// <returns></returns>
        public bool D_InventoryProcessing_Exec(D_InventoryProcessing_Entity de)
        {
            string sp = "PRC_TanaoroshiShimeShori";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Syori", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.Syori } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
                { "@InventoryDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InventoryDate } },
                { "@FromRackNO",new ValuePair { value1=SqlDbType.VarChar,value2=de.FromRackNO} },
                { "@ToRackNO",new ValuePair { value1=SqlDbType.VarChar,value2=de.ToRackNO} },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Operator} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.PC} },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
