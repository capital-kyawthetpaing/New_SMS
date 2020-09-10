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
    public class D_TenzikaiJuchuu_DL : Base_DL
    {
        /// <summary>
        /// 展示会受注情報振替処理よりデータ抽出時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <returns></returns>
        public DataTable D_TenzikaiJuchuu_SelectAll(M_TenzikaiShouhin_Entity me)
        {
            string sp = "D_TenzikaiJuchuu_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.VendorCD } },
                { "@LastYearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastYearTerm } },
                { "@LastSeason", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastSeason } },
            };
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 展示会受注情報振替処理
        /// TenzikaiJuchuuJouhouHurikaeShoriより更新時に使用
        /// </summary>
        /// <returns></returns>
        public bool D_TenzikaiJuchuu_Exec(M_TenzikaiShouhin_Entity me)
        {
            string sp = "PRC_TenzikaiJuchuuJouhouHurikaeShori";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.ProcessMode } },
                { "@TorokuDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ChangeDate } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.VendorCD } },
                { "@LastYearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastYearTerm } },
                { "@LastSeason", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastSeason } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Operator} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.PC} },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
