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
    public class WebJuchuuKakunin_BL : Base_BL
    {
        D_Juchuu_DL mdl;
        public WebJuchuuKakunin_BL()
        {
            mdl = new D_Juchuu_DL();
        }

        /// <summary>
        /// WEB受注確認にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Juchuu_SelectForWeb(D_Juchuu_Entity dje, D_JuchuuStatus_Entity djse)
        {
            return mdl.D_Juchuu_SelectForWeb(dje, djse);
        }

        public DataTable BindForCombo(D_Juchuu_Entity de, int kbn)
        {
            return mdl.BindForWebJuchuuKakunin(de, kbn);
        }
        public DataTable D_Juchuu_SelectForWebHikiate(D_Juchuu_Entity de)
        {
            D_Juchuu_DL dp = new D_Juchuu_DL();
            return dp.D_Juchuu_SelectForWebHikiate(de);
        }

    }
}
