using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class M_Vendor_DL : Base_DL
    {
        public DataTable M_Vendor_IsExists(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorCD });
            dic.Add("@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mve.ChangeDate });
            dic.Add("@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.DeleteFlg });
            UseTransaction = true;
            return SelectData(dic, "M_Vendor_IsExists");
        }

        public DataTable M_Vendor_Select(M_Vendor_Entity mve)
        {
            string sp = "M_Vendor_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ChangeDate } }
            };
            
            return SelectData(dic, sp);
        }

        public DataTable M_Vendor_ZipCodeSelect(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mve.ChangeDate } },
                { "@ZipCode1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ZipCD1 } },
                { "@ZipCode2", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.ZipCD2 } }               
            };
            UseTransaction = true;
            return SelectData(dic, "M_Vendor_ZipCodeSelect");
        }

        public DataTable M_Vendor_SelectTop1(M_Vendor_Entity mve)
        {
            string sp = "M_Vendor_SelectTop1";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ChangeDate } },
                { "@VendorFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorFlg } },
                //{ "@PayeeFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.PayeeFlg } },
                //{ "@MoneyPayeeFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.MoneyPayeeFlg } },
            };
            return SelectData(dic, sp);
        }
        public DataTable M_Vendor_SelectForPayeeCD(M_Vendor_Entity mve)
        {
            string sp = "M_Vendor_SelectForPayeeCD";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ChangeDate } }
            };
            return SelectData(dic, sp);
        }

        public DataTable M_Vendor_Search(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mve.ChangeDate } },
                { "@DisplayKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.DisplayKBN } },
                { "@VendorCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCDFrom } },
                { "@VendorCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCDTo } },
                { "@VendorName", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorName } },
                { "@VendorKana", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorKana } },
                {"@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.DeleteFlg } },

            };
            UseTransaction = true;
            return SelectData(dic,"M_Vendor_Search");
        }
        public DataTable M_Vendor_Select_Tenji(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.DateTime, value2 = mve.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.DeleteFlg } },
                {"@VendorFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorFlg } },
                {"@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorCD } },

            };
            UseTransaction = true;
            return SelectData(dic, "M_Vendor_Select_Tenji");
        }
        public DataTable Payee_Select(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.PayeeCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mve.ChangeDate } }
            };
            UseTransaction = true;
            return SelectData(dic, "M_Vendor_PayeeCD_Select");
        }

        public DataTable MoneyPayee_Select(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.MoneyPayeeCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mve.ChangeDate } }
            };
            UseTransaction = true;
            return SelectData(dic, "MoneyPayee_Select");
        }

        public bool M_Vendor_InsertUpdate(M_Vendor_Entity mve,int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ChangeDate } },
                { "@ShoguchiFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 =mve.ShoguchiFlg  } },
                { "@VendorName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorName } },
                { "@VendorShortName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorShortName } },  //Add By SawLay
                { "@VendorLongName1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorLongName1 } },
                { "@VendorLongName2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorLongName2 } },
                { "@VendorPostName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorPostName } },
                { "@VendorPositionName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorPositionName } },
                { "@VendorStaffName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorStaffName} },
                { "@VendorKana", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorKana } },
                { "@PayeeFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.PayeeFlg } },
                { "@MoneyPayeeFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.MoneyPayeeFlg } },
                { "@PayeeCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.PayeeCD } },
                { "@MoneyPayeeCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.MoneyPayeeCD } },
                { "@ZipCD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ZipCD1 } },
                { "@ZipCD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ZipCD2 } },
                { "@Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.Address1 } },
                { "@Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.Address2 } },
                { "@MailAddress ", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.MailAddress1 } },
                { "@TelphoneNo " , new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.TelephoneNO } },
                { "@FaxNo ", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.FaxNO } },
                { "@PaymentCloseDay", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.PaymentCloseDay } },
                { "@PaymentPlanKBN ", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.PaymentPlanKBN } },
                { "@PaymentPlanDay " , new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.PaymentPlanDay } },
                { "@HolidayKBN ", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.HolidayKBN } },
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.BankCD } },
                { "@BranchCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.BranchCD } },
                { "@KouzaKBN ", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.KouzaKBN } },
                { "@KouzaNo " , new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.KouzaNO } },
                { "@KouzaMeigi ", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.KouzaMeigi } },
                { "@KouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.KouzaCD } },
                { "@TaxTiming", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.TaxTiming } },     //Add By SawLay
                { "@TaxFractionKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.TaxFractionKBN } },   //Add By SawLay
                { "@AmountFractionKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.AmountFractionKBN } },   //Add By SawLay
                { "@NetFlg ", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.NetFlg } },
                { "@EDIFlg ", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.EDIFlg } },    //Add By SawLay
                { "@EDIVendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.EDIVendorCD } },   //Add By SawLay
                { "@StaffCD ", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.StaffCD } },
                { "@AnalyzeCD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.AnalyzeCD1 } },
                { "@AnalyzeCD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.AnalyzeCD2 } },
                { "@AnalyzeCD3 ", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.AnalyzeCD3 } },
                { "@DisplayOrder", new ValuePair { value1 = SqlDbType.Int, value2 = mve.DisplayOrder } },
                { "@DisplayNote ", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.DisplayNote } },
                { "@NotDisplayNote", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.NotDisplyNote } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.DeleteFlg } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorCD +""+ mve.ChangeDate } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Vendor_Insert_Update");
        }

        public bool M_Vendor_Delete(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ChangeDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.VendorCD +""+ mve.ChangeDate } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Vendor_Delete");
        }
        public DataTable M_Vendor_PayeeFlg_Select(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
            };
            UseTransaction = true;
            return SelectData(dic, "M_Vendor_PayeeFlg_Select");
        }

        public DataTable M_Vendor_DataSelect(M_Vendor_Entity mve)   // For NyuukaShoukai (pnz) 
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mve.ChangeDate } }
            };
            UseTransaction = true;
            return SelectData(dic, "M_Vendor_DataSelect");
        }
        public DataTable M_SearchVendor(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mve.ChangeDate } },
                { "@VendorKBN",new ValuePair{value1=SqlDbType.TinyInt,value2=mve.VendorKBN} },
                { "@VendorName", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorName } },
                { "@VendorKana", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorKana } },
                { "@VendorCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCDFrom } },
                { "@VendorCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCDTo } },
                { "@NotDisplayNote", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.NotDisplyNote } },
                { "@keyword1",new ValuePair{value1=SqlDbType.VarChar,value2=mve.Keyword1} },
                { "@keyword2",new ValuePair{value1=SqlDbType.VarChar,value2=mve.Keyword2} },
                { "@keyword3",new ValuePair{value1=SqlDbType.VarChar,value2=mve.Keyword3} }
            };
            UseTransaction = true;
            return SelectData(dic, "M_SearchVendor");
        }

        public DataTable M_Vendor_SelectForSiharaiNo(M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mve.ChangeDate } },
                { "@PayeeFlg",new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.PayeeFlg} }
            };
            UseTransaction = true;
            return SelectData(dic, "M_Vendor_SelectForSiharaiNo");
        }

        public DataTable M_Vendor_SelectForSiharaiNyuuroku (M_Vendor_Entity mve)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mve.ChangeDate } },
                { "@MoneyPayeeFlg",new ValuePair { value1 = SqlDbType.TinyInt, value2 = mve.MoneyPayeeFlg} }
            };
            UseTransaction = true;
            return SelectData(dic, "M_Vendor_SelectForSiharaiNyuuroku");
        }


        public DataTable M_Vendor_SelectForJuchuu(M_Vendor_Entity mve)
        {
            string sp = "M_Vendor_SelectForJuchuu";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ChangeDate } }
            };

            return SelectData(dic, sp);
        }


    }
}
