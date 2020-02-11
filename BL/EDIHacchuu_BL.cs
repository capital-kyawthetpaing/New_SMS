using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class EDIHacchuu_BL : Base_BL
    {
        D_Hacchu_DL dl;
        D_EdiHacchuu_DL edl;

        public EDIHacchuu_BL()
        {
            dl = new D_Hacchu_DL();
            edl = new D_EdiHacchuu_DL();
        }

        /// <summary>
        /// EDI発注取得処理
        /// EDI処理番号検索時に使用
        /// </summary>
        public DataTable D_EDIOrder_Select(D_EDIOrder_Entity dee)
        {
            DataTable dt = edl.D_EDIOrder_Select(dee);

            return dt;
        }

        /// <summary>
        /// EDI発注取得処理
        /// EDI処理番号検索時に使用
        /// </summary>
        public DataTable D_EDIOrder_SelectAll(D_EDIOrder_Entity dee)
        {
            DataTable dt = edl.D_EDIOrder_SelectAll(dee);

            return dt;
        }

        /// <summary>
        /// 発注取得処理
        /// 発注番号チェック時に使用
        /// </summary>
        public DataTable D_Order_SelectForEDIHacchuu(string orderNo)
        {
            DataTable dt = edl.D_Order_SelectForEDIHacchuu(orderNo);

            return dt;
        }

        /// <summary>
        /// EDI発注追加用データ取得処理
        /// </summary>
        public DataTable D_Order_SelectAllForEDIHacchuu(D_Order_Entity doe)
        {
            return dl.D_Order_SelectAllForEDIHacchuu(doe);
        }

        /// <summary>
        /// EDI発注CSV出力用データ取得処理
        /// </summary>
        public DataTable D_EDIOrder_SelectForCSV(D_EDIOrder_Entity dee)
        {
            return edl.D_EDIOrder_SelectForCSV(dee);
        }

        public bool PRC_EDIOrder_Insert(D_Order_Entity doe, D_EDIOrder_Entity dee)
        {
            return edl.PRC_EDIOrder_Insert(doe, dee);

        }

        /// <summary>
        /// EDI発注メール追加処理
        /// </summary>
        public bool PRC_EDIOrder_MailInsert(D_EDIOrder_Entity dee)
        {
            return edl.PRC_EDIOrder_MailInsert(dee);
        }
        
    }
}
