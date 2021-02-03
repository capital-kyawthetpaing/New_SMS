using Entity;
using DL;
using System.Data;

namespace BL
{
    public class YamatoOkurijou_BL : Base_BL
    {
        public YamatoOkurijou_BL()
        {
     
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
            M_Control_DL dl = new M_Control_DL();
            DataTable dt = dl.M_Control_Select(me);

            return dt;
        }
        public bool D_Shipping_Update(D_Shipping_Entity de)
        {
            D_Shipping_DL dl = new D_Shipping_DL();
            return dl.D_Shipping_Update(de);
        }

    }
}
