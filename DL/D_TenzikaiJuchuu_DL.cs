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
        public DataTable CheckTenzikaiJuchuu(M_TenzikaiShouhin_Entity me)
        {
            string sp = "CheckTenzikaiJuchuu";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@TorokuDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ChangeDate } },
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

        public DataTable D_TenzikaiJuchuu_SearchData(D_TenzikaiJuchuu_Entity dtje)
        {
            string sp = "D_TenzikaiJuchuu_SearchData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuDateFrom", new ValuePair { value1 = SqlDbType.Date, value2 = dtje.JuchuuDateFrom } },
                { "@JuchuuDateTo", new ValuePair { value1 = SqlDbType.Date, value2 = dtje.JuchuuDateTo } },
                { "@Year", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.year } },
                { "@Season", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.season } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.StaffCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.CustomerCD } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.VendorCD } },
                { "@ProductName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.ProuductName } },
                { "@ItemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.ItemCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.JanCD } },
            };
            return SelectData(dic, sp);
        } //pnz

        public DataTable D_TenzikaiJuchuu_SelectForExcel(D_TenzikaiJuchuu_Entity dtje,int chk)
        {
            string sp = "D_TenzikaiJuchuu_SelectForExcel";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.VendorCD } },
                { "@Year", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.LastYearTerm } },
                { "@Season", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.season } },
                { "@CustomerCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.CustomerCDFrom } },
                { "@CustomerCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.CustomerCDTo } },
                { "@TenzikaiName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.ExhibitionName } },
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.BrandCD } },
                { "@SegmentCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.SegmentCD } },
                { "@Chk", new ValuePair { value1 = SqlDbType.VarChar, value2 = chk.ToString() } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtje.Key } }
                
            };
            return SelectData(dic, sp);
        } //pnz
    }
}
