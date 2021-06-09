
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class JuchuuDataCheck_BL : Base_BL
    {
        D_Juchuu_DL dl;
        public JuchuuDataCheck_BL()
        {
            dl= new D_Juchuu_DL();
        }

        /// <summary>
        /// 受注データチェック処理
        /// JuchuuDataCheckより更新時に使用
        /// </summary>
        public bool JuchuuDataCheck_Exec(D_Juchuu_Entity dme)
        {
            return dl.JuchuuDataCheck_Exec(dme);
        }

        /// <summary>
        /// 名寄せ結果登録データ取得処理
        /// NayoseKekkaTourokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Juchu_SelectForNayose(D_Juchuu_Entity de)
        {
            DataTable dt = dl.D_Juchu_SelectForNayose(de);

            return dt;
        }
        /// <summary>
        /// 名寄せ結果登録データ取得処理
        /// NayoseKekkaTourokuよりデータ抽出時に使用
        /// </summary>
        public DataTable M_Customer_SelectForNayose(M_Customer_Entity me)
        {
            M_Customer_DL mdl = new M_Customer_DL();
            DataTable dt = mdl.M_Customer_SelectForNayose(me);

            return dt;
        }
    }
}
