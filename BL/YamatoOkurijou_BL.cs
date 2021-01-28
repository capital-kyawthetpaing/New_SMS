using Entity;
using DL;
using System.Data;

namespace BL
{
    public class YamatoOkurijou_BL : Base_BL
    {
        D_EdiHacchuu_DL edl;

        public YamatoOkurijou_BL()
        {
            edl = new D_EdiHacchuu_DL();
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
            D_Hacchu_DL dl = new D_Hacchu_DL();
            return dl.D_Order_SelectAllForEDIHacchuu(doe);
        }

        /// <summary>
        /// CSV出力用データ取得処理
        /// </summary>
        public DataTable D_Shipping_SelectForYamato(D_Shipping_Entity de)
        {
            D_Shipping_DL dl = new D_Shipping_DL();
            return dl.D_Shipping_SelectForYamato(de);
        }
        public DataTable M_Control_Select(M_Control_Entity me)
        {
            int ret = 0;
            M_Control_DL dl = new M_Control_DL();
            DataTable dt = dl.M_Control_Select(me);

            return dt;
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
