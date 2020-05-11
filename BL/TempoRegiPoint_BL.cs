using DL;
using System.Data;

namespace BL
{
    /// <summary>
    /// 店舗レジポイント引換券印刷用BL
    /// </summary>
    public class TempoRegiPoint_BL : Base_BL
    {
        /// <summary>
        /// 会員情報取得
        /// </summary>
        /// <param name="customerCD">会員番号</param>
        /// <param name="changeDate">変更日付</param>
        /// <returns>会員情報</returns>
        public DataTable D_GetCustomer(string customerCD = "", string changeDate = "")
        {
            var dl = new TempoRegiPoint_DL();
            return dl.D_CustomerSelectData(customerCD, changeDate);
        }

        /// <summary>
        /// 保持ポイント取得
        /// </summary>
        /// <param name="customerCD">会員番号</param>
        /// <returns>保持ポイント</returns>
        public DataTable D_LastPointSelect(string customerCD = "")
        {
            var dl = new TempoRegiPoint_DL();
            return dl.D_LastPointSelectData(customerCD);
        }

        /// <summary>
        /// 引換券発行単位取得
        /// </summary>
        /// <param name="storeCD">店舗CD</param>
        /// <returns>引換券発行単位</returns>
        public DataTable D_TicketUnitSelect(string storeCD = "")
        {
            var dl = new TempoRegiPoint_DL();
            return dl.D_TicketUnitSelectData(storeCD);
        }

        /// <summary>
        /// 商品引換券情報取得
        /// </summary>
        /// <param name="storeCD">店舗CD</param>
        /// <returns>商品引換券情報</returns>
        public DataTable D_CouponSelect(string storeCD = "")
        {
            var dl = new TempoRegiPoint_DL();
            return dl.D_CouponSelectData(storeCD);
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
        public bool M_UpdateLastPoint(string customerCD, int issuePoint, string operatorCd, string programId, string pcId)
        {
            var dl = new TempoRegiPoint_DL();
            return dl.M_Customer_UpdateLastPoint(customerCD, issuePoint, operatorCd, programId, pcId);
        }
    }
}
