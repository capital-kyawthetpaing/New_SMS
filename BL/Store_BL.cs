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
    public class Store_BL : Base_BL
    {
        M_Store_DL storedl;
        M_StoreAuthorizations_DL storeauthordl;
        public Store_BL()
        {
            storedl = new M_Store_DL();
            storeauthordl = new M_StoreAuthorizations_DL();
        }
        /// <summary>
        /// Select Store's info
        /// StoreKBN IN (1,3)のStore情報をBind
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable M_Store_Bind_Mitsumori(M_Store_Entity mse)
        {
            return storedl.M_Store_Bind_Mitsumori(mse);
        }
        /// <summary>
        /// StoreKBN IN 1のStore情報をBind
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable M_Store_Bind_Juchu(M_Store_Entity mse)
        {
            return storedl.M_Store_Bind_Juchu(mse);
        }
        /// <summary>	
        /// StoreKBN NOT IN 2のStore情報をBind（権限のある店舗のみ）	
        /// </summary>	
        /// <param name="mse"></param>	
        /// <returns></returns>	
        public DataTable M_Store_Bind_Getsuji(M_Store_Entity mse)
        {
            return storedl.M_Store_Bind_Getsuji(mse);
        }
        public DataTable M_Store_Select(M_Store_Entity mse)
        {
            return storedl.M_Store_Select(mse);
        }

        public DataTable M_Store_SelectAll(M_Store_Entity mse)
        {
            return storedl.M_Store_SelectAll(mse);
        }

        /// <summary>
        /// 店舗マスタ更新処理
        /// MasterTouroku_Tempoより更新時に使用
        /// </summary>
        public bool Store_Exec(M_Store_Entity mse, short operationMode, string operatorNm, string pc)
        {
            return storedl.M_Store_Exec(mse, operationMode, operatorNm, pc);
        }

        /// <summary>
        /// 店舗マスタ取得処理
        /// MasterTouroku_Tempoよりデータ抽出時に使用
        /// </summary>
        public DataTable Store_SelectData(M_Store_Entity mse)
        {
            return storedl.M_Store_SelectData(mse);
        }
        public DataTable BindStore(M_Store_Entity mse)// Bind datat to comboBox
        {
            return storedl.M_Store_Bind(mse);
            //return storedl.SimpleSelect("StoreName,StoreCD", "F_Store ('" + changedate + "')","DeleteFlg = 0 And (StoreKBN = 2 OR StoreKBN = 3) Order By StoreCD");
        }

        public DataTable BindData(M_Store_Entity mse)
        {
            return storedl.M_Store_BindData(mse);
        }
        /// <summary>	
        /// 改定日として有効なレコードで違う店舗で同じAPIKeyがあればTrue	
        /// </summary>	
        /// <param name="mse"></param>	
        /// <returns></returns>	
        public bool Store_SelectByApiKey(M_Store_Entity mse)
        {
            DataTable dt = storedl.M_Store_SelectByApiKey(mse);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
