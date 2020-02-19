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
    public class D_Inventory_DL : Base_DL
    {
        /// <summary>
        /// 棚卸入力データ取得処理
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public DataTable D_Inventory_SelectAll(D_Inventory_Entity de)
        {
            string sp = "D_Inventory_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InventoryDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InventoryDate } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
        };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 棚卸入力更新処理
        /// TanaoroshiNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_Inventory_Update(D_Inventory_Entity de, DataTable dt)
        {
            string sp = "D_Inventory_Update";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;
            
            AddParam(command, "@SoukoCD", SqlDbType.VarChar, de.SoukoCD);
            AddParam(command, "@InventoryDate", SqlDbType.VarChar, de.InventoryDate);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, de.Operator);
            AddParam(command, "@PC", SqlDbType.VarChar, de.PC);

            //OUTパラメータの追加
            string outPutParam = "";

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }
        /// <summary>
        /// 棚卸差異表,棚卸表データ取得処理
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Inventory_SelectForPrint(D_Inventory_Entity de)
        {
            string sp = "D_Inventory_SelectForPrint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InventoryDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InventoryDate } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SoukoCD } },
                { "@ChkSaiOnly", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkSaiOnly.ToString() } },
                { "@KbnSai", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.KbnSai.ToString() } },
                { "@ChkKinyu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkKinyu.ToString() } },
        };

            return SelectData(dic, sp);
        }

    }
}
