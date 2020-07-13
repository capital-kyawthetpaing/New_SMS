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
        /// <param name="depositNo">入出金No</param>
        /// <returns>取引レシート(雑入金)データ</returns>
        public DataTable D_MiscDepositSelect(string depositNo)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_MiscDepositSelectData(depositNo);
        }

        /// <summary>
        /// 取引レシート(入金)データ取得
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <returns>取引レシート(入金)データ</returns>
        public DataTable D_DepositSelect(string depositNo)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_DepositSelectData(depositNo);
        }

        /// <summary>
        /// 取引レシート(雑出金)データ取得
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <returns>取引レシート(雑出金)データ</returns>
        public DataTable D_MiscPaymentSelect(string depositNo)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_MiscPaymentSelectData(depositNo);
        }

        /// <summary>
        /// 取引レシート(両替)データ取得
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <returns>取引レシート(両替)データ</returns>
        public DataTable D_ExchangeSelect(string depositNo)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_ExchangeSelectData(depositNo);
        }

        /// <summary>
        /// 取引レシート(釣銭準備)データ取得
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <returns>取引レシート(釣銭準備)データ</returns>
        public DataTable D_ChangePreparationSelect(string depositNo)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_ChangePreparationSelectData(depositNo);
        }
    }
}
