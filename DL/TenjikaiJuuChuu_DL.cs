using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;
namespace DL
{
    public class TenjikaiJuuChuu_DL : Base_DL
    {
        public TenjikaiJuuChuu_DL()
        {

        }
        //Select_TenjiData
        public DataSet Select_TenjiData(Tenjikai_Entity tje)
        {
            string sp = "M_TenjiDataSet_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@TenjiNo", new ValuePair { value1 = SqlDbType.VarChar, value2 =  tje.TenjiKaiOrderNo } },
            };

            UseTransaction = true;
            return SelectSetData(dic, sp);
        }
        public bool D_TenjiDelete(Tenjikai_Entity tje, string xml)
        {
            string sp = "D_TenjiI_Details_Delete";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.VarChar, value2 = xml} },
                { "@TenjiCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.TenjiKaiOrderNo} },
                { "@JuchuuBi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.JuchuuBi } },
                { "@ShuuKaSouKo", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ShuuKaSouKo } },
                { "@TantouStaffu", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.TantouStaffu } },
                { "@Shiiresaki", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Shiiresaki } },
                { "@Nendo", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Nendo } },
                { "@ShiZun", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ShiZun } },
                { "@Kokyaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Kokyaku } },
                { "@K_Name1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Name1 } },
                { "@K_name2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_name2 } },
                { "@K_radio", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_radio } },
                { "@K_Zip1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Zip1 } },
                { "@K_Zip2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Zip2 } },
                { "@K_Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Address1 } },
                { "@K_Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Address2 } },
                { "@K_Denwa1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Denwa1 } },
                { "@K_Denwa2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Denwa2 } },
                { "@K_Denwa3", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Denwa3 } },
                { "@KDenwa21", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KDenwa21 } },
                { "@KDenwa22", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KDenwa22 } },
                { "@KDenwa23", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KDenwa23 } },
                { "@KkanaMei", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KkanaMei } },
                { "@HaisoSaki", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HaisoSaki } },
                { "@H_Name1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Name1 } },
                { "@H_name2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_name2 } },
                { "@H_radio", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_radio } },
                { "@H_Zip1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Zip1 } },
                { "@H_Zip2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Zip2 } },
                { "@H_Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Address1 } },
                { "@H_Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Address2 } },
                { "@H_Denwa1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Denwa1 } },
                { "@H_Denwa2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Denwa2 } },
                { "@H_Denwa3", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Denwa3 } },
                { "@HDenwa21", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HDenwa21 } },
                { "@HDenwa22", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HDenwa22 } },
                { "@HDenwa23", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HDenwa23 } },
                { "@HkanaMei", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HkanaMei } },

                { "@ZeiKomi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ZeiKomi } },
                { "@Zeinu", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Zeinu } },
                { "@Keijen", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Keijen } },
                { "@Tsuujou", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Tsuujou } },
                { "@ZeiKomiSou", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ZeiKomi } },
                { "@GenkaGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.GenkaGaku } },
                { "@ArariGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ArariGaku } },
                { "@YoteiKinShu", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.YoteiKinShu } },
                { "@UriageYoteiBi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.UriageYoteiBi } },
                { "@Sumi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Sumi } },
                { "@Nichi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Nichi } },
                { "@InsertOpt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertOperator } },
                { "@InsertDt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertDt } },
                //{ "@InsertOpt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertOpt } },
                //{ "@InsertDt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertDt } },
                { "@DeleteOpt", new ValuePair { value1 = SqlDbType.VarChar, value2 = null } },
                { "@DeleteDt", new ValuePair { value1 = SqlDbType.VarChar, value2 =null } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.StoreCD } },
                { "@InsertOperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertOperator } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.PC } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Program  } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = "削除" } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ProgramID } },
                    };
            UseTransaction = true;
            try
            {
                return InsertUpdateDeleteData(dic, sp);
            }
            catch (Exception ex)
            {
                var msg = ex.StackTrace;

                return false;
            }
        }
        public bool D_TenjiUpdate(Tenjikai_Entity tje, string xml,string dxml)
        {
            string sp = "D_TenjiI_Details_Update";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.VarChar, value2 = xml} },
                { "@xml2", new ValuePair { value1 = SqlDbType.VarChar, value2 = dxml} },
                { "@TenjiCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.TenjiKaiOrderNo} },
                { "@JuchuuBi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.JuchuuBi } },
                { "@ShuuKaSouKo", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ShuuKaSouKo } },
                { "@TantouStaffu", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.TantouStaffu } },
                { "@Shiiresaki", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Shiiresaki } },
                { "@Nendo", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Nendo } },
                { "@ShiZun", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ShiZun } },
                { "@Kokyaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Kokyaku } },
                { "@K_Name1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Name1 } },
                { "@K_name2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_name2 } },
                { "@K_radio", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_radio } },
                { "@K_Zip1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Zip1 } },
                { "@K_Zip2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Zip2 } },
                { "@K_Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Address1 } },
                { "@K_Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Address2 } },
                { "@K_Denwa1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Denwa1 } },
                { "@K_Denwa2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Denwa2 } },
                { "@K_Denwa3", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Denwa3 } },
                { "@KDenwa21", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KDenwa21 } },
                { "@KDenwa22", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KDenwa22 } },
                { "@KDenwa23", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KDenwa23 } },
                { "@KkanaMei", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KkanaMei } },
                { "@HaisoSaki", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HaisoSaki } },
                { "@H_Name1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Name1 } },
                { "@H_name2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_name2 } },
                { "@H_radio", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_radio } },
                { "@H_Zip1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Zip1 } },
                { "@H_Zip2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Zip2 } },
                { "@H_Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Address1 } },
                { "@H_Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Address2 } },
                { "@H_Denwa1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Denwa1 } },
                { "@H_Denwa2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Denwa2 } },
                { "@H_Denwa3", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Denwa3 } },
                { "@HDenwa21", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HDenwa21 } },
                { "@HDenwa22", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HDenwa22 } },
                { "@HDenwa23", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HDenwa23 } },
                { "@HkanaMei", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HkanaMei } },

                { "@ZeiKomi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ZeiKomi } },
                { "@Zeinu", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Zeinu } },
                { "@Keijen", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Keijen } },
                { "@Tsuujou", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Tsuujou } },
                { "@ZeiKomiSou", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ZeiKomi } },
                { "@GenkaGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.GenkaGaku } },
                { "@ArariGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ArariGaku } },
                { "@YoteiKinShu", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.YoteiKinShu } },
                { "@UriageYoteiBi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.UriageYoteiBi } },
                { "@Sumi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Sumi } },
                { "@Nichi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Nichi } },
                { "@InsertOpt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertOperator } },
                { "@InsertDt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertDt } },
                //{ "@InsertOpt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertOpt } },
                //{ "@InsertDt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertDt } },
                { "@DeleteOpt", new ValuePair { value1 = SqlDbType.VarChar, value2 = null } },
                { "@DeleteDt", new ValuePair { value1 = SqlDbType.VarChar, value2 = null } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.StoreCD } },
                { "@InsertOperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertOperator } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.PC } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Program } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = "変更" } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ProgramID } },

                    };
            UseTransaction = true;
            try
            {
                return InsertUpdateDeleteData(dic, sp);
            }
            catch (Exception ex)
            {
                var msg = ex.StackTrace;

                return false;
            }
        }

        public bool D_TenjiInsert(Tenjikai_Entity tje, string xml)
        {
           
            string sp = "D_TenjiI_Details_insert";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.VarChar, value2 = xml} },
                { "@JuchuuBi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.JuchuuBi } },
                { "@ShuuKaSouKo", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ShuuKaSouKo } },
                { "@TantouStaffu", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.TantouStaffu } },
                { "@Shiiresaki", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Shiiresaki } },
                { "@Nendo", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Nendo } },
                { "@ShiZun", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ShiZun } },
                { "@Kokyaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Kokyaku } },
                { "@K_Name1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Name1 } },
                { "@K_name2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_name2 } },
                { "@K_radio", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_radio } },
                { "@K_Zip1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Zip1 } },
                { "@K_Zip2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Zip2 } },
                { "@K_Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Address1 } },
                { "@K_Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Address2 } },
                { "@K_Denwa1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Denwa1 } },
                { "@K_Denwa2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Denwa2 } },
                { "@K_Denwa3", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.K_Denwa3 } },
                { "@KDenwa21", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KDenwa21 } },
                { "@KDenwa22", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KDenwa22 } },
                { "@KDenwa23", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KDenwa23 } },
                { "@KkanaMei", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.KkanaMei } },
                { "@HaisoSaki", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HaisoSaki } },
                { "@H_Name1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Name1 } },
                { "@H_name2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_name2 } },
                { "@H_radio", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_radio } },
                { "@H_Zip1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Zip1 } },
                { "@H_Zip2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Zip2 } },
                { "@H_Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Address1 } },
                { "@H_Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Address2 } },
                { "@H_Denwa1", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Denwa1 } },
                { "@H_Denwa2", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Denwa2 } },
                { "@H_Denwa3", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.H_Denwa3 } },
                { "@HDenwa21", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HDenwa21 } },
                { "@HDenwa22", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HDenwa22 } },
                { "@HDenwa23", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HDenwa23 } },
                { "@HkanaMei", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.HkanaMei } },

                { "@ZeiKomi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ZeiKomi } },
                { "@Zeinu", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Zeinu } },
                { "@Keijen", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Keijen } },
                { "@Tsuujou", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Tsuujou } },
                { "@ZeiKomiSou", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ZeiKomi } },
                { "@GenkaGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.GenkaGaku } },
                { "@ArariGaku", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ArariGaku } },
                { "@YoteiKinShu", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.YoteiKinShu } },
                { "@UriageYoteiBi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.UriageYoteiBi } },
                { "@Sumi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Sumi } },
                { "@Nichi", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Nichi } },
                { "@InsertOpt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertOperator } },
                { "@InsertDt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertDt } },
                //{ "@InsertOpt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertOpt } },
                //{ "@InsertDt", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertDt } },
                { "@DeleteOpt", new ValuePair { value1 = SqlDbType.VarChar, value2 = null } },
                { "@DeleteDt", new ValuePair { value1 = SqlDbType.VarChar, value2 =  null } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.StoreCD } },

                { "@InsertOperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.InsertOperator } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.PC } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 =  tje.Program } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = "新規" } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ProgramID } },
              

            };
            UseTransaction = true;
            try
            {
                return InsertUpdateDeleteData(dic, sp);
            }
            catch (Exception ex)
            {
                var msg = ex.StackTrace;
              
                return false;
            }
        }

        public DataTable GetTaxRate(string Flg, string Date)
        {
            string sp = "GetMaxTaxRate";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Flg", new ValuePair { value1 = SqlDbType.VarChar, value2 = Flg } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = Date } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }
        public DataTable JuuChuuCheck(string Bi)
        {
            string sp = "JuuChuuBiCheck";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Bi", new ValuePair { value1 = SqlDbType.Date, value2 = Bi } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }
        public DataTable ShuukaSouko(string SouKoCD, string ChangeDate)
        {
            string sp = "ShuukaSoukoCheck";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = ChangeDate } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =  SouKoCD } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }
        public DataTable M_TenjiKaiJuuChuu_Select(Tenjikai_Entity tje)
        {
            string sp = "M_TenjiKaiJuuChuu_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@xml", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.xml } },
                 { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Kokyaku } },
                 { "@JuuChuuBi", new ValuePair { value1 = SqlDbType.Date, value2 = tje.JuchuuBi.Replace("/","-") } },
                 { "@LastyearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Nendo } },
                 { "@LastSeason", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ShiZun } },
                 { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Shiiresaki } },
                 { "@SoukoName", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ShuuKaSouKo } },
                 { "@DesiredDate1", new ValuePair { value1 = SqlDbType.Date, value2 = tje.KibouBi1 } },
                 { "@DesiredDate2", new ValuePair { value1 = SqlDbType.Date, value2 = tje.KibouBi2 } },

            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }
        public DataTable M_TeniKaiSelectbyJAN(Tenjikai_Entity tje)
        {
            string sp = "M_TeniKaiSelectbyJAN";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Kokyaku } },
                 { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =  tje.JanCD } },
                 { "@LastYearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Nendo } },
                 { "@LastSeason", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.ShiZun } },
                 { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = tje.Shiiresaki } },
                 { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = tje.ChangeDate.Replace("/","-") } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }
        public DataTable Check_DTenjikaiJuuchuu(string txt)
        {
            string sp = "D_TeniKaiSelectbyJAN";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@TenjiCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = txt } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }

        public DataTable D_TeniSelectbyTaniCD (string TaniCD)
        {
            string sp = "D_TeniSelectbyTaniCD";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@TeniCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = TaniCD } },
            };

            UseTransaction = true;
            return SelectData(dic, sp);
        }


    }
}
