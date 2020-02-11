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
    public class MitsumoriNyuuryoku_BL : Base_BL
    {
        D_Mitsumori_DL mdl;
        public MitsumoriNyuuryoku_BL()
        {
            mdl = new D_Mitsumori_DL();
        }

        public DataTable D_Mitsumori_SelectAll(D_Mitsumori_Entity mie)
        {
            return mdl.D_Mitsumori_SelectAll(mie);
        }

        public DataTable D_Mitsumori_SelectForPrint(D_Mitsumori_Entity mie)
        {
            return mdl.D_Mitsumori_SelectForPrint(mie);
        }

        /// <summary>
        /// 見積入力マスタ更新処理
        /// MitsumoriNyuuryokuより更新時に使用
        /// </summary>
        public bool Mitsumori_Exec(D_Mitsumori_Entity dme, DataTable dt, short operationMode, string operatorNm, string pc)
        {
            return mdl.D_Mitsumori_Exec(dme, dt, operationMode, operatorNm, pc);
        }

        /// <summary>
        /// 見積入力取得処理
        /// MitsumoriNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Mitsumori_SelectData(D_Mitsumori_Entity dme, short operationMode)
        {
            DataTable dt = mdl.D_Mitsumori_SelectData(dme, operationMode);
            //if (dt.Rows.Count > 0)
            //{
            //    dme.MitsumoriNO = dt.Rows[0]["MitsumoriNO"].ToString();
            //    dme.StoreCD = dt.Rows[0]["StoreCD"].ToString();
            //    dme.MitsumoriDate = dt.Rows[0]["MitsumoriDate"].ToString();
            //    dme.StaffCD = dt.Rows[0]["StaffCD"].ToString();
            //    dme.CustomerCD = dt.Rows[0]["CustomerCD"].ToString();
            //    dme.CustomerName = dt.Rows[0]["CustomerName"].ToString();
            //    dme.CustomerName2 = dt.Rows[0]["CustomerName2"].ToString();
            //    dme.AliasKBN = dt.Rows[0]["AliasKBN"].ToString();
            //    dme.ZipCD1 = dt.Rows[0]["ZipCD1"].ToString();
            //    dme.ZipCD2 = dt.Rows[0]["ZipCD2"].ToString();
            //    dme.Address1 = dt.Rows[0]["Address1"].ToString();
            //    dme.Address2 = dt.Rows[0]["Address2"].ToString();
            //    dme.Tel11 = dt.Rows[0]["Tel11"].ToString();
            //    dme.Tel12 = dt.Rows[0]["Tel12"].ToString();
            //    dme.Tel13 = dt.Rows[0]["Tel13"].ToString();
            //    dme.Tel21 = dt.Rows[0]["Tel21"].ToString();
            //    dme.Tel22 = dt.Rows[0]["Tel22"].ToString();
            //    dme.Tel23 = dt.Rows[0]["Tel23"].ToString();
            //    dme.JuchuuChanceKBN = dt.Rows[0]["JuchuuChanceKBN"].ToString();
            //    dme.MitsumoriName = dt.Rows[0]["MitsumoriName"].ToString();
            //    dme.DeliveryDate = dt.Rows[0]["DeliveryDate"].ToString();
            //    dme.PaymentTerms = dt.Rows[0]["PaymentTerms"].ToString();
            //    dme.DeliveryPlace = dt.Rows[0]["DeliveryPlace"].ToString();
            //    dme.ValidityPeriod = dt.Rows[0]["ValidityPeriod"].ToString();
            //    dme.MitsumoriHontaiGaku = dt.Rows[0]["MitsumoriHontaiGaku"].ToString();
            //    dme.MitsumoriTax8 = dt.Rows[0]["MitsumoriTax8"].ToString();
            //    dme.MitsumoriTax10 = dt.Rows[0]["MitsumoriTax10"].ToString();
            //    dme.MitsumoriGaku = dt.Rows[0]["MitsumoriGaku"].ToString();
            //    dme.CostGaku = dt.Rows[0]["CostGaku"].ToString();
            //    dme.ProfitGaku = dt.Rows[0]["ProfitGaku"].ToString();
            //    dme.RemarksInStore = dt.Rows[0]["RemarksInStore"].ToString();
            //    dme.RemarksOutStore = dt.Rows[0]["RemarksOutStore"].ToString();
            //    dme.PrintDateTime = dt.Rows[0]["PrintDateTime"].ToString();
            //    dme.JuchuuFLG = dt.Rows[0]["JuchuuFLG"].ToString();

            //                }

            return dt;
        }

        /// <summary>
        /// 見積書更新処理
        /// Mitsumoriよりフラグ更新時に使用
        /// </summary>
        public bool D_Mitsumori_Update(D_Mitsumori_Entity dme, DataTable dt, string operatorNm, string pc)
        {
            return mdl.D_Mitsumori_Update(dme, dt, operatorNm, pc);
        }
    }
}
