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
    public class TempoJuchuuNyuuryoku_BL : Base_BL
    {
        D_Juchuu_DL mdl;
        public TempoJuchuuNyuuryoku_BL()
        {
            mdl = new D_Juchuu_DL();
        }
        public DataTable M_Souko_IsExists(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_IsExists(mse);
        }
        public DataTable M_Souko_SelectForMitsumori(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_SelectForMitsumori(mse);
        }

        public bool M_SKULastCost_Select(M_SKULastCost_Entity mse)
        {
            M_SKULastCost_DL msdl = new M_SKULastCost_DL();
            DataTable dt = msdl.M_SKULastCost_Select(mse);

            if (dt.Rows.Count > 0)
            {
                mse.LastCost = dt.Rows[0]["LastCost"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 受注入力更新処理
        /// TempoJuchuuNyuuryokuより更新時に使用
        /// </summary>
        public bool Juchu_Exec(D_Juchuu_Entity dme, DataTable dt, short operationMode)
        {
            return mdl.D_Juchu_Exec(dme, dt, operationMode);
        }

        /// <summary>
        /// 受注入力取得処理
        /// TempoJuchuuNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Juchu_SelectData(D_Juchuu_Entity de, short operationMode, short tennic = 0)
        {
            DataTable dt = mdl.D_Juchu_SelectData(de, operationMode, tennic);

            return dt;
        }
        /// <summary>
        /// 進捗チェック　
        /// 既に売上済み,出荷済み,出荷指示済み,ピッキングリスト完了済み,仕入済み,入荷済み,発注済み警告
        /// </summary>
        /// <param name="juchuNo"></param>
        /// <param name="errno"></param>
        /// <returns></returns>
        public bool CheckJuchuData(string juchuNo, out string errno, short tennic = 0)
        {
            bool ret = false;
            errno = "";

            if (tennic.Equals(1))
            {
                DataTable dt = mdl.GetJuchuuNO(juchuNo);
                foreach (DataRow row in dt.Rows)
                {
                    ret = CheckJuchuData(row["JuchuuNo"].ToString(), out errno);
                    if (!ret)
                        return ret;
                }
            }
            else
            {
                return CheckJuchuData(juchuNo, out errno);
            }

            return ret;
        }
        /// <summary>
        /// 進捗チェック　
        /// 既に売上済み,出荷済み,出荷指示済み,ピッキングリスト完了済み,仕入済み,入荷済み,発注済み警告
        /// </summary>
        /// <param name="juchuNo"></param>
        /// <param name="errno"></param>
        /// <returns></returns>
        public bool CheckJuchuData(string juchuNo, out string errno)
        {
            DataTable dt = mdl.CheckJuchuData(juchuNo);

            bool ret = false;
            errno = "";

            if (dt.Rows.Count>0)
            {
                errno = dt.Rows[0]["errno"].ToString();
                ret = true;
            }
            
            return ret;
        }
        /// <summary>
        /// 入荷進捗、出荷進捗
        /// </summary>
        /// <param name="juchuNo"></param>
        /// <param name="status">入荷進捗</param>
        /// <param name="status2">出荷進捗</param>
        /// <returns></returns>
        public bool CheckJuchuDetailsData(string juchuNo,string juchuGyoNo, out string status, out string status2)
        {
            DataTable dt = mdl.CheckJuchuDetailsData(juchuNo, juchuGyoNo);

            bool ret = false;
            status = "";
            status2 = "";

            if (dt.Rows.Count > 0)
            {
                status = dt.Rows[0]["STATUS"].ToString();
                status2 = dt.Rows[0]["STATUS2"].ToString();

                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 店舗納品書更新処理
        /// TempoNouhinsyoよりフラグ更新時に使用
        /// </summary>
        public bool D_Juchu_Update(D_Mitsumori_Entity dme, DataTable dt, string operatorNm, string pc)
        {
            return mdl.D_Juchu_Update(dme, dt, operatorNm, pc);
        }

        public bool DeleteTemporaryReserve(D_Juchuu_Entity de)
        {
            return mdl.DeleteTemporaryReserve(de);
        }
        public string GetTemporaryReserveNO(string Denno)
        {
            DataTable dt = mdl.GetTemporaryReserveNO(Denno);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["OutNO"].ToString();

            return "";
        }
        public string GetNouki(string date,string storeCD)
        {
            DataTable dt = mdl.GetNouki(date, storeCD);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["Nouki"].ToString();

            return "";
        }
    }
}
