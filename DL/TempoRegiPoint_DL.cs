using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DL
{
    /// <summary>
    /// 店舗レジポイント引換券印刷DL
    /// </summary>
    public class TempoRegiPoint_DL : Base_DL
    {
        /// <summary>
        /// 会員情報取得処理
        /// </summary>
        /// <param name="customerCD">会員番号</param>
        /// <param name="changeDate">変更日付</param>
        /// <returns>会員情報</returns>
        public DataTable D_CustomerSelectData(string customerCD = "", string changeDate = "")
        {
            string sp = "M_Customer_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = customerCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = changeDate } }
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 保持ポイント取得処理
        /// </summary>
        /// <param name="customerCD">会員番号</param>
        /// <returns>保持ポイント</returns>
        public DataTable D_LastPointSelectData(string customerCD = "")
        {
            string sp = "D_SelectLastPoint_ForTempoRegiPoint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = customerCD } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 引換券発行単位取得処理
        /// </summary>
        /// <param name="storeCD">店舗CD</param>
        /// <returns>引換券発行単位</returns>
        public DataTable D_TicketUnitSelectData(string storeCD = "")
        {
            string sp = "D_SelectTicketUnit_ForTempRegiPoint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 商品引換券情報取得処理
        /// </summary>
        /// <param name="storeCD">店舗CD</param>
        /// <returns>商品引換券情報</returns>
        public DataTable D_CouponSelectData(string storeCD = "")
        {
            string sp = "D_Coupon_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 発行ポイント更新
        /// </summary>
        /// <param name="customerCD">会員番号</param>
        /// <param name="issuePoint">発行ポイント</param>
        /// <param name="operatorCd">該当スタッフCD</param>
        /// <param name="programId">起動プログラムID</param>
        /// <param name="pc">起動端末</param>
        /// <returns>更新結果(true:成功、false:失敗)</returns>
        public bool M_Customer_UpdateLastPoint(string customerCD, int issuePoint, string operatorCd, string programId, string pcId)
        {
            string sp = "M_Customer_UpdateLastPoint";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@CustomerCD", SqlDbType.VarChar, customerCD);
            AddParam(command, "@IssuePoint", SqlDbType.Money, issuePoint.ToString());
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
