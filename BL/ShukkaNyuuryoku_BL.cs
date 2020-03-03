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
    public class ShukkaNyuuryoku_BL : Base_BL
    {
        D_Shipping_DL dsdl;
        D_Instruction_DL didl;
        public ShukkaNyuuryoku_BL()
        {
            dsdl = new D_Shipping_DL();
            didl = new D_Instruction_DL();
        }

        /// <summary>
        /// 出荷番号検索にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Shipping_SelectAll(D_Shipping_Entity de)
        {
            return dsdl.D_Shipping_SelectAll(de);
        }
        public DataTable M_Souko_SelectForNyuuka(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_SelectForNyuuka(mse);
        }

        /// <summary>
        /// 出荷入力更新処理
        /// ShukkaNyuuryokuより更新時に使用
        /// </summary>
        public bool Shipping_Exec(D_Shipping_Entity dse, DataTable dt, short operationMode, string storeCD)
        {
            return dsdl.D_Shipping_Exec(dse, dt, operationMode, storeCD);
        }

        /// <summary>
        /// 出荷指示データチェック取得処理
        /// </summary>
        public DataTable CheckInstruction(D_Instruction_Entity de)
        {
            DataTable dt = didl.CheckInstruction(de);

            return dt;
        }

        /// <summary>
        /// 出荷データチェック取得処理
        /// </summary>
        public DataTable CheckShipping(D_Shipping_Entity de)
        {
            DataTable dt = dsdl.CheckShipping(de);

            return dt;
        }

        /// <summary>
        /// 出荷取得処理
        /// ShukkaNyuuryokuより出荷データ抽出時に使用
        /// 新規時以外
        /// </summary>
        public DataTable D_Shipping_SelectData(D_Shipping_Entity de)
        {
            DataTable dt = dsdl.D_Shipping_SelectData(de);

            return dt;
        }

        /// <summary>
        /// 出荷入力取得処理
        /// ShukkaNyuuryokuより出荷指示データ抽出時に使用
        /// 新規時
        /// </summary>
        public DataTable D_Instruction_SelectDataForShukka(D_Instruction_Entity de, string shippingDate)
        {
            DataTable dt = didl.D_Instruction_SelectDataForShukka(de, shippingDate);

            return dt;
        }
        /// <summary>
        /// 出荷済み・売上済みチェック
        /// </summary>
        /// <param name="D_Shipping_Entity"></param>
        /// <param name="errno"></param>
        /// <returns></returns>
        public bool CheckShukkaData(D_Shipping_Entity de, out string errno)
        {
            DataTable dt = dsdl.CheckShukkaData(de);

            bool ret = true;
            errno = "";

            if (dt.Rows.Count>0)
            {
                errno = dt.Rows[0]["errno"].ToString();
            }
            
            return ret;
        }

        public DataTable M_Carrier_SelectForShukka(M_Carrier_Entity mse)
        {
            M_Carrier_DL msdl = new M_Carrier_DL();
            return msdl.M_Carrier_SelectForShukka(mse);
        }

        public DataTable D_Juchuu_SelectData_ForShukka(D_Juchuu_Entity de)
        {
            D_Juchuu_DL mdl = new D_Juchuu_DL();
            return mdl.D_Juchuu_SelectData_ForShukka(de);
        }
    }
}
