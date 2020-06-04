using DL;
using System;
using System.Data;

namespace BL
{
    /// <summary>
    /// 店舗レジ領収書印刷用BL
    /// </summary>
    public class TempoRegiRyousyuusyo_BL : Base_BL
    {
        /// <summary>
        /// 領収書データ取得
        /// </summary>
        /// <param name="salesNo">お買上番号</param>
        /// <returns>チェック結果(true=売上データあり、false=売上データなし)</returns>
        public bool D_CheckSalseNO(string salesNo = "")
        {
            var dl = new TempoRegiRyousyuusyo_DL();
            //if(dl.D_CheckSalseNO(salesNo).Rows.Count > 0)
            //{
            //    return Convert.ToInt32(dl.D_CheckSalseNO(salesNo).Rows[0][0]) > 0 ? true : false;
            //}

            return false;
        }

        /// <summary>
        /// 領収書データ取得
        /// </summary>
        /// <param name="salesNo">お買上番号</param>
        /// <param name="printDate">領収書印字日付</param>
        /// <returns>領収書データ</returns>
        public DataTable D_RyousyuusyoSelect(string salesNo = "", string printDate = "")
        {
            var dl = new TempoRegiRyousyuusyo_DL();
            return dl.D_RyousyuusyoSelectData(salesNo, printDate);
        }

        /// <summary>
        /// レシートデータ取得
        /// </summary>
        /// <param name="salesNo">お買上番号</param>
        /// <param name="isIssued">再発行(true=再発行、false=未発行)</param>
        /// <returns>レシートデータ</returns>
        public DataTable D_ReceiptSelect(string salesNo = "", bool isIssued = false)
        {
            var dl = new TempoRegiRyousyuusyo_DL();
            return dl.D_ReceiptSelectData(salesNo, isIssued);
        }

        /// <summary>
        /// 店舗取引履歴 発行済更新
        /// </summary>
        /// <param name="salesNo">お買上番号</param>
        /// <param name="isIssued">発行済</param>
        /// <param name="operatorCd"></param>
        /// <param name="programId"></param>
        /// <param name="pcId"></param>
        /// <returns></returns>
        public bool D_UpdateDepositHistory(string salesNo, bool isIssued, string operatorCd, string programId, string pcId)
        {
            var dl = new TempoRegiRyousyuusyo_DL();
            return dl.D_DepositHistory_UpdateIssued(salesNo, isIssued, operatorCd, programId, pcId);
        }
    }
}
