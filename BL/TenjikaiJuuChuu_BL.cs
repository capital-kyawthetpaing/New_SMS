using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using DL;
namespace BL
{
   public class TenjikaiJuuChuu_BL : Base_BL
    {
        TenjikaiJuuChuu_DL tdl = new TenjikaiJuuChuu_DL();
        public TenjikaiJuuChuu_BL()
        {

        }
        public bool Select_TenjiData(Tenjikai_Entity tje,out DataTable GridDt)
        {
            var ds = tdl.Select_TenjiData(tje) ;
            if (ds.Tables.Count == 2 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                GridDt = ds.Tables[1];
                var dtDetail = ds.Tables[0];
             var   tjei = new Tenjikai_Entity();
                 {

                    tje.Shiiresaki = dtDetail.Rows[0]["VendorCD"].ToString();
                    tje.Nendo = dtDetail.Rows[0]["LastYearTerm"].ToString();
                    tje.ShiZun = dtDetail.Rows[0]["LastSeason"].ToString();
                    tje.JuchuuBi = dtDetail.Rows[0]["JuChuuDate"].ToString();
                    tje.UriageYoteiBi = dtDetail.Rows[0]["SalesPlanDate"].ToString();
                    tje.ShuuKaSouKo = dtDetail.Rows[0]["SoukoName"].ToString();
                    tje.SouKoCD = dtDetail.Rows[0]["SoukoCD"].ToString();
                    tje.TantouStaffu = dtDetail.Rows[0]["StaffCD"].ToString();
                    tje.StaffName = dtDetail.Rows[0]["StaffName"].ToString();

                    tje.CVFlg = dtDetail.Rows[0]["CustomerVariousFlg"].ToString();// To make enable disable 0 > Dis 1 > Ena
                    tje.Kokyaku = dtDetail.Rows[0]["CustomerCD"].ToString();
                    tje.K_Name1 = dtDetail.Rows[0]["CustomerName"].ToString();
                    tje.K_name2 = dtDetail.Rows[0]["CustomerName2"].ToString();
                    tje.K_Address1 = dtDetail.Rows[0]["CAddress1"].ToString();
                    tje.K_Address2 = dtDetail.Rows[0]["CAddress2"].ToString();
                    tje.K_Denwa1 = dtDetail.Rows[0]["CTel11"].ToString();
                    tje.K_Denwa2 = dtDetail.Rows[0]["CTel12"].ToString();
                    tje.K_Denwa3 = dtDetail.Rows[0]["CTel13"].ToString();
                    tje.K_Zip1 = dtDetail.Rows[0]["CZipCD1"].ToString();
                    tje.K_Zip2 = dtDetail.Rows[0]["CZipCD2"].ToString();
                    tje.CKBN = dtDetail.Rows[0]["CKBN"].ToString();

                    tje.DVFlg = dtDetail.Rows[0]["DeliveryVariousFlg"].ToString(); // To make enable disable 0 > Dis 1 > Ena
                    tje.HaisoSaki = dtDetail.Rows[0]["DeliveryCD"].ToString();
                    tje.H_Name1 = dtDetail.Rows[0]["DeliveryName"].ToString();
                    tje.H_name2 = dtDetail.Rows[0]["DeliveryName2"].ToString();
                    tje.H_Address1 = dtDetail.Rows[0]["DeliveryAddress1"].ToString();
                    tje.H_Address2 = dtDetail.Rows[0]["DeliveryAddress2"].ToString();
                    tje.H_Denwa1 = dtDetail.Rows[0]["DeliveryTel11"].ToString();
                    tje.H_Denwa2 = dtDetail.Rows[0]["DeliveryTel12"].ToString();
                    tje.H_Denwa3 = dtDetail.Rows[0]["DeliveryTel13"].ToString();
                    tje.H_Zip1 = dtDetail.Rows[0]["DeliveryZipCD1"].ToString();
                    tje.H_Zip2 = dtDetail.Rows[0]["DeliveryZipCD2"].ToString();
                    tje.HKBN = dtDetail.Rows[0]["DKBN"].ToString();

                    tje.YoteiKinShu = dtDetail.Rows[0]["PaymentMethodCD"].ToString();
                };
              //  tje = tjei;
            }
            else
            {
                GridDt = null;
                return false;
            }
            return true;
        }
        public bool D_TenjiInsert( Tenjikai_Entity tje, string xml )
        {
            return tdl.D_TenjiInsert(tje,xml);
        }
        public bool D_TenjiUpdate(Tenjikai_Entity tje, string xml,string dxml)
        {
            return tdl.D_TenjiUpdate(tje, xml,dxml);
        }
        public bool D_TenjiDelete(Tenjikai_Entity tje, string xml)
        {
            return tdl.D_TenjiDelete(tje, xml);
        }
        public DataTable JuuChuuCheck(string JuuChuuBi)
        {
           return tdl.JuuChuuCheck(JuuChuuBi);
           
        }
        public DataTable ShuukaSouko(string SouKoCD, string ChangeDate)
        {
            return tdl.ShuukaSouko(SouKoCD, ChangeDate);
        }
        public DataTable M_TenjiKaiJuuChuu_Select(Tenjikai_Entity tje)
        {
            return tdl.M_TenjiKaiJuuChuu_Select(tje);
        }
        public DataTable M_TeniKaiSelectbyJAN(Tenjikai_Entity tje)
        {
            return tdl.M_TeniKaiSelectbyJAN(tje);
        }
        public DataTable GetTaxRate(string taxflg,string changeDtae)
        {
            return tdl.GetTaxRate(taxflg, changeDtae);
        }
        public DataTable Check_DTenjikaiJuuchuu(string tkb)
        {
            return tdl.Check_DTenjikaiJuuchuu(tkb);
        }
        public string D_TeniSelectbyTaniCD(string tkb)
        {
            var dt= tdl.D_TeniSelectbyTaniCD(tkb);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Char1"].ToString();
            }
            return "" ;
        }
    }
}
