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
        /// <param name="depositNo">入出金No</param>
        /// <returns>店舗取引レシート(雑入金)情報取得</returns>
        public DataTable D_MiscDepositSelectData(string depositNo)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@depositNo", new ValuePair { value1 = SqlDbType.Int, value2 = depositNo } },
                },
                "D_SelectMiscDeposit_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(入金)情報取得処理
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <returns>店舗取引レシート(入金)情報取得</returns>
        public DataTable D_DepositSelectData(string depositNo)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@depositNo", new ValuePair { value1 = SqlDbType.Int, value2 = depositNo } },
                },
                "D_SelectDeposit_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(雑出金)情報取得処理
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <returns>店舗取引レシート(雑出金)情報取得</returns>
        public DataTable D_MiscPaymentSelectData(string depositNo)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@depositNo", new ValuePair { value1 = SqlDbType.Int, value2 = depositNo } },
                },
                "D_SelectMiscPayment_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(両替)情報取得処理
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <returns>店舗取引レシート(両替)情報取得</returns>
        public DataTable D_ExchangeSelectData(string depositNo)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@depositNo", new ValuePair { value1 = SqlDbType.Int, value2 = depositNo } },
                },
                "D_SelectExchange_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(釣銭準備)情報取得処理
        /// </summary>
        /// <param name="depositNo">入出金No</param>
        /// <returns>店舗取引レシート(釣銭準備)情報取得</returns>
        public DataTable D_ChangePreparationSelectData(string depositNo)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@depositNo", new ValuePair { value1 = SqlDbType.Int, value2 = depositNo } },
                },
                "D_SelectChangePreparation_ForTempoTorihikiReceipt"
            );
        }
    }
}
