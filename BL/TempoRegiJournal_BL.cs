using DL;
using System.Data;

namespace BL
{
    /// <summary>
    /// 店舗レジジャーナル印刷用BL
    /// </summary>
    public class TempoRegiJournal_BL : Base_BL
    {
        /// <summary>
        /// 店舗積算データ取得
        /// </summary>
        /// <param name="storeCd">店舗CD</param>
        /// <param name="dateFrom">日付(FROM)</param>
        /// <param name="dateTo">日付(TO)</param>
        /// <returns>店舗積算データ</returns>
        public DataTable D_CheckStoreCalculation(string storeCd = "", string dateFrom = "", string dateTo = "")
        {
            var dl = new TempoRegiJournal_DL();
            return dl.D_CheckStoreCalculation(storeCd, dateFrom, dateTo);
        }

        /// <summary>
        /// ジャーナルデータ取得
        /// </summary>
        /// <param name="storeCd">店舗CD</param>
        /// <param name="dateFrom">日付(FROM)</param>
        /// <param name="dateTo">日付(TO)</param>
        /// <returns>ジャーナルデータ</returns>
        public DataTable D_JournalSelect(string storeCd = "", string dateFrom = "", string dateTo = "")
        {
            var dl = new TempoRegiJournal_DL();
            return dl.D_JournalSelectData(storeCd, dateFrom, dateTo);
        }
    }
}
