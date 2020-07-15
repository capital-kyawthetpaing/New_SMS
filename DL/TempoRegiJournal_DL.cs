using System.Collections.Generic;
using System.Data;

namespace DL
{
    /// <summary>
    /// 店舗レジジャーナル印刷DL
    /// </summary>
    public class TempoRegiJournal_DL : Base_DL
    {
        /// <summary>
        /// 店舗積算データ件数取得処理
        /// </summary>
        /// <param name="storeCd">店舗CD</param>
        /// <param name="dateFrom">日付(FROM)</param>
        /// <param name="dateTo">日付(TO)</param>
        /// <returns>店舗積算データ件数</returns>
        public DataTable D_CheckStoreCalculation(string storeCd = "", string dateFrom = "", string dateTo = "")
        {
            string sp = "D_CheckStoreCalculation_ForTempoRegiJournal";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCd } },
                { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dateFrom } },
                { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dateTo } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 店舗レジジャーナル情報取得処理
        /// </summary>
        /// <param name="storeCd">店舗CD</param>
        /// <param name="dateFrom">日付(FROM)</param>
        /// <param name="dateTo">日付(TO)</param>
        /// <returns>店舗レジジャーナル情報取得</returns>
        public DataTable D_JournalSelectData(string storeCd = "", string dateFrom = "", string dateTo = "")
        {
            string sp = "D_SelectData_ForTempoRegiJournal";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCd } },
                { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dateFrom } },
                { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dateTo } },
            };

            return SelectData(dic, sp);
        }
    }
}
