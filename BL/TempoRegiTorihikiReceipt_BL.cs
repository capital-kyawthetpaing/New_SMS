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
        /// <param name="staffCD">担当者CD</param>
        /// <returns>取引レシート(雑入金)データ</returns>
        public DataTable D_MiscDepositSelect(string depositNo, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_MiscDepositSelectData(depositNo, staffCD);
        }

        /// <summary>
        /// 取引レシート(入金)データ取得
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <param name="staffCD">担当者CD</param>
        /// <returns>取引レシート(入金)データ</returns>
        public DataTable D_DepositSelect(string depositNo, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_DepositSelectData(depositNo, staffCD);
        }

        /// <summary>
        /// 取引レシート(雑出金)データ取得
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <param name="staffCD">担当者CD</param>
        /// <returns>取引レシート(雑出金)データ</returns>
        public DataTable D_MiscPaymentSelect(string depositNo, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_MiscPaymentSelectData(depositNo, staffCD);
        }

        /// <summary>
        /// 取引レシート(両替)データ取得
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <param name="staffCD">担当者CD</param>
        /// <returns>取引レシート(両替)データ</returns>
        public DataTable D_ExchangeSelect(string depositNo, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_ExchangeSelectData(depositNo, staffCD);
        }

        /// <summary>
        /// 取引レシート(釣銭準備)データ取得
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <param name="staffCD">担当者CD</param>
        /// <returns>取引レシート(釣銭準備)データ</returns>
        public DataTable D_ChangePreparationSelect(string depositNo, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_ChangePreparationSelectData(depositNo, staffCD);
        }
    }
}
