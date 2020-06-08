using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DL
{
    /// <summary>
    /// 店舗レジ領収書印刷DL
    /// </summary>
    public class TempoRegiRyousyuusyo_DL : Base_DL
    {
        /// <summary>
        /// お買上番号チェック
        /// </summary>
        /// <param name="salesNO">お買上番号</param>
        /// <returns>売上データ件数</returns>
        public DataTable D_CheckSalseNO(string salesNO = "")
        {
            string sp = "D_CheckSalseNO_ForTempoRegiRyousyuusyo";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = salesNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 店舗レジ領収書印刷データ取得処理
        /// TempoRegiRyousyuusyoよりデータ抽出時に使用
        /// </summary>
        /// <param name="salesNO">お買上番号</param>
        /// <param name="printDate">領収書印字日付</param>
        /// <returns>領収書データ</returns>
        public DataTable D_RyousyuusyoSelectData(string salesNO = "", string printDate = "")
        {
            string sp = "D_SelectData_ForTempoRegiRyousyuusyo";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = salesNO } },
                { "@PrintDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = printDate } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 店舗レジ領収書レシートデータ取得処理
        /// </summary>
        /// <param name="salesNO">お買上番号</param>
        /// <param name="isIssued">再発行(true=再発行、false=未発行)</param>
        /// <returns>レシートデータ</returns>
        public DataTable D_ReceiptSelectData(string salesNO = "", bool isIssued = false)
        {
            string sp = "D_Receipt_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = salesNO } },
                { "@IsIssued", new ValuePair { value1 = SqlDbType.TinyInt, value2 = Convert.ToByte(isIssued).ToString()} },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 店舗取引履歴 発行済更新
        /// </summary>
        /// <param name="salesNo"></param>
        /// <param name="isIssued"></param>
        /// <param name="operatorName"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_DepositHistory_UpdateIssued(string salesNo, bool isIssued, string operatorCd, string programId, string pcId)
        {
            string sp = "D_DepositHistory_UpdateIssued";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@IsIssued", SqlDbType.TinyInt, (isIssued ? 1 : 0).ToString());
            AddParam(command, "@SalesNO", SqlDbType.VarChar, salesNo);
            AddParam(command, "@Operator", SqlDbType.VarChar, operatorCd);
            AddParam(command, "@Program", SqlDbType.VarChar, programId);
            AddParam(command, "@PC", SqlDbType.VarChar, pcId);

            UseTransaction = true;

            string outPutParam = "";    //未使用
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }
    }
}
