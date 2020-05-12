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
        /// <param name="depositNO">入出金番号</param>
        /// <returns>店舗取引レシート(雑入金)情報取得</returns>
        public DataTable D_MiscDepositSelectData(int depositNO)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@DepositNO", new ValuePair { value1 = SqlDbType.Int, value2 = depositNO.ToString() } },
                }, 
                "D_SelectMiscDeposit_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(入金)情報取得処理
        /// </summary>
        /// <param name="depositNO">入出金番号</param>
        /// <returns>店舗取引レシート(入金)情報取得</returns>
        public DataTable D_DepositSelectData(int depositNO)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@DepositNO", new ValuePair { value1 = SqlDbType.Int, value2 = depositNO.ToString() } },
                },
                "D_SelectDeposit_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(雑出金)情報取得処理
        /// </summary>
        /// <param name="depositNO">入出金番号</param>
        /// <returns>店舗取引レシート(雑出金)情報取得</returns>
        public DataTable D_MiscPaymentSelectData(int depositNO)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@DepositNO", new ValuePair { value1 = SqlDbType.Int, value2 = depositNO.ToString() } },
                },
                "D_SelectMiscPayment_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(両替)情報取得処理
        /// </summary>
        /// <param name="depositNO">入出金番号</param>
        /// <returns>店舗取引レシート(両替)情報取得</returns>
        public DataTable D_ExchangeSelectData(int depositNO)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@DepositNO", new ValuePair { value1 = SqlDbType.Int, value2 = depositNO.ToString() } },
                },
                "D_SelectExchange_ForTempoTorihikiReceipt"
            );
        }

        /// <summary>
        /// 店舗取引レシート(釣銭準備)情報取得処理
        /// </summary>
        /// <param name="depositNO">入出金番号</param>
        /// <returns>店舗取引レシート(釣銭準備)情報取得</returns>
        public DataTable D_ChangePreparationSelectData(int depositNO)
        {
            return SelectData(
                new Dictionary<string, ValuePair>
                {
                    { "@DepositNO", new ValuePair { value1 = SqlDbType.Int, value2 = depositNO.ToString() } },
                },
                "D_SelectChangePreparation_ForTempoTorihikiReceipt"
            );
        }
    }
}
