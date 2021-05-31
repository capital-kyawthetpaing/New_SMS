
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class NayoseKekkaTouroku_BL : Base_BL
    {
        D_Juchuu_DL dl;
        public NayoseKekkaTouroku_BL()
        {
            dl= new D_Juchuu_DL();
        }

        /// <summary>
        /// 名寄せ結果登録更新処理
        /// NayoseKekkaTourokuより更新時に使用
        /// </summary>
        public bool NayoseKekkaTouroku_Exec(D_Juchuu_Entity dme, DataTable dt)
        {
            return dl.NayoseKekkaTouroku_Exec(dme, dt);
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
