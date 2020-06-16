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
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>取引レシート(雑入金)データ</returns>
        public DataTable D_MiscDepositSelect(string storeCD, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_MiscDepositSelectData(storeCD, staffCD);
        }

        /// <summary>
        /// 取引レシート(入金)データ取得
        /// </summary>
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>取引レシート(入金)データ</returns>
        public DataTable D_DepositSelect(string storeCD, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_DepositSelectData(storeCD, staffCD);
        }

        /// <summary>
        /// 取引レシート(雑出金)データ取得
        /// </summary>
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>取引レシート(雑出金)データ</returns>
        public DataTable D_MiscPaymentSelect(string storeCD, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_MiscPaymentSelectData(storeCD, staffCD);
        }

        /// <summary>
        /// 取引レシート(両替)データ取得
        /// </summary>
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>取引レシート(両替)データ</returns>
        public DataTable D_ExchangeSelect(string storeCD, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_ExchangeSelectData(storeCD, staffCD);
        }

        /// <summary>
        /// 取引レシート(釣銭準備)データ取得
        /// </summary>
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>取引レシート(釣銭準備)データ</returns>
        public DataTable D_ChangePreparationSelect(string storeCD, string staffCD)
        {
            var dl = new TempoRegiTorihikiReceipt_DL();
            return dl.D_ChangePreparationSelectData(storeCD, staffCD);
        }
    }
}
