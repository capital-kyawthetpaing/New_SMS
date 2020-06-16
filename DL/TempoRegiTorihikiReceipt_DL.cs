using System.Collections.Generic;
using System.Data;

namespace DL
{
    /// <summary>
    /// 店舗取引レシート印刷DL
    /// </summary>
    public class TempoRegiTorihikiReceipt_DL : Base_DL
    {
        /// <summary>
        /// 店舗取引レシート(雑入金)情報取得処理
        /// </summary>
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>店舗取引レシート(雑入金)情報取得</returns>
        public DataTable D_MiscDepositSelectData(string storeCD, string staffCD)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
                    { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = staffCD } },
                },
                "D_SelectMiscDeposit_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(入金)情報取得処理
        /// </summary>
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>店舗取引レシート(入金)情報取得</returns>
        public DataTable D_DepositSelectData(string storeCD, string staffCD)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
                    { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = staffCD } },
                },
                "D_SelectDeposit_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(雑出金)情報取得処理
        /// </summary>
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>店舗取引レシート(雑出金)情報取得</returns>
        public DataTable D_MiscPaymentSelectData(string storeCD, string staffCD)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
                    { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = staffCD } },
                },
                "D_SelectMiscPayment_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(両替)情報取得処理
        /// </summary>
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>店舗取引レシート(両替)情報取得</returns>
        public DataTable D_ExchangeSelectData(string storeCD, string staffCD)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
                    { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = staffCD } },
                },
                "D_SelectExchange_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(釣銭準備)情報取得処理
        /// </summary>
        /// <param name="storeCD">ストアコード</param>
        /// <param name="staffCD">スタッフコード</param>
        /// <returns>店舗取引レシート(釣銭準備)情報取得</returns>
        public DataTable D_ChangePreparationSelectData(string storeCD, string staffCD)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
                    { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = staffCD } },
                },
                "D_SelectChangePreparation_ForTempoTorihikiReceipt"
            );
        }
    }
}
