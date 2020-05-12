using DL;
using System.Data;

namespace BL
{
    /// <summary>
    /// 店舗取引レシート印刷用BL
    /// </summary>
    public class TempoRegiTorihikiReceipt_BL : Base_BL
    {
        /// <summary>
        /// 取引レシート(雑入金)データ取得
        /// </summary>
        /// <param name="depositNO">入出金番号</param>
        /// <returns>取引レシート(雑入金)データ</returns>
        public DataTable D_MiscDepositSelect(int depositNO)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_MiscDepositSelectData(depositNO);
        }

        /// <summary>
        /// 取引レシート(入金)データ取得
        /// </summary>
        /// <param name="depositNO">入出金番号</param>
        /// <returns>取引レシート(入金)データ</returns>
        public DataTable D_DepositSelect(int depositNO)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_DepositSelectData(depositNO);
        }

        /// <summary>
        /// 取引レシート(雑出金)データ取得
        /// </summary>
        /// <param name="depositNO">入出金番号</param>
        /// <returns>取引レシート(雑出金)データ</returns>
        public DataTable D_MiscPaymentSelect(int depositNO)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_MiscPaymentSelectData(depositNO);
        }

        /// <summary>
        /// 取引レシート(両替)データ取得
        /// </summary>
        /// <param name="depositNO">入出金番号</param>
        /// <returns>取引レシート(両替)データ</returns>
        public DataTable D_ExchangeSelect(int depositNO)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_ExchangeSelectData(depositNO);
        }

        /// <summary>
        /// 取引レシート(釣銭準備)データ取得
        /// </summary>
        /// <param name="depositNO">入出金番号</param>
        /// <returns>取引レシート(釣銭準備)データ</returns>
        public DataTable D_ChangePreparationSelect(int depositNO)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_ChangePreparationSelectData(depositNO);
        }
    }
}
