using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class M_Customer_DL : Base_DL
    {
        /// <summary>	
        /// 指定した得意先CDで適用日が指定した日付以前で最新のデータを取得	
        /// ※DeleteFlgの指定なし。各プログラムでDeleteFlgの値を参照しエラー処理等を行ってください	
        /// </summary>	
        /// <param name="me"></param>	
        /// <returns></returns>
        public DataTable M_Customer_Select(M_Customer_Entity me)
        {
            string sp = "M_Customer_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.CustomerCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ChangeDate } }
            };

            return SelectData(dic, sp);
        }

        public DataTable M_Customer_SelectAll(M_Customer_Entity mke)
        {
            string sp = "M_Customer_SelectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.CustomerCD } }
            };
            return SelectData(dic, sp);
        }

        public DataTable Select_M_Customer_CustomerName(string janCD)
        {
            string sp = "M_Customer_CustomerName_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = janCD } },

            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }
        public bool M_Customer_Insert_Update(M_Customer_Entity cust, int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = cust.CustomerCD } },
                {"@StoreCD" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.StroeCD}},
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = cust.CustomerName } },
                { "@LastName" ,new ValuePair{value1=SqlDbType.VarChar,value2=cust.LastName}},
                {"@FirstName", new ValuePair {value1=SqlDbType.VarChar,value2=cust.FirstName} },
                {"@LongName1" ,new ValuePair{value1=SqlDbType.VarChar,value2=cust.LongName1}},
                {"@KanaName" ,new ValuePair{value1=SqlDbType.VarChar,value2=cust.KanaName}},
                {"@StoreKBN", new ValuePair{value1=SqlDbType.TinyInt,value2=cust.StoreKBN}},
                {"@CustomerKBN" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.CustomerKBN}},
                {"@AliasKBN" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.AliasKBN}},
                {"@BillingType" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.BillingType}},
                {"@GroupName" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.GroupName}},
                {"@BillingFLG" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.BillingFLG}},
                {"@CollectFLG" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.CollectFLG}},
                {"@BillingCD" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.BillingCD}},
                {"@CollectCD" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.CollectCD}},
                {"@Birthdate" , new ValuePair{value1=SqlDbType.Date,value2=cust.Birthdate}},
                {"@Sex" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.Sex}},
                {"@Tel11" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.TelephoneNo1}},
                {"@Tel12" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.TelephoneNo2}},
                {"@Tel13" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.TelephoneNo3}},
                {"@Tel21" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.HomephoneNo1}},
                {"@Tel22" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.HomephoneNo2}},
                {"@Tel23" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.HomephoneNo3}},
                {"@ZipCD1" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.ZipCD1}},
                {"@ZipCD2" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.ZipCD2}},
                {"@Address1" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.Address1}},
                {"@Address2" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.Address2}},
                {"@MailAddress" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.MailAddress}},
              //  {"@MailAddress2" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.MailAddress2}},
                {"@TankaCD" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.TankaCD}},
                {"@PointFLG" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.PointFLG}},
                {"@MainStoreCD" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.MainStoreCD}},
                {"@StaffCD" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.StaffCD}},
                {"@BillingCloseDate" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.BillingCloseDate}},
                {"@CollectPlanDate" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.CollectPlanDate}},
                {"@TaxFractionKBN" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.TaxFractionKBN}},
                {"@AmountFractionKBN" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.AmountFractionKBN}},
                {"@PaymentUnit" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.PaymentUnit}},
                {"@DMFlg" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.DMFlg}},
                {"@DeleteFlg" , new ValuePair{value1=SqlDbType.TinyInt,value2=cust.DeleteFlg}},
                {"@Operator" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.Operator}},
                {"@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = cust.ProgramID } },
                {"@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = cust.PC } },
                {"@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = cust.ProcessMode } },
                {"@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = cust.Key } },
                {"@Mode" , new ValuePair{value1=SqlDbType.TinyInt,value2=mode.ToString()}},

            };

            return InsertUpdateDeleteData(dic, "M_Customer_Insert_Update");
        }
        public DataTable M_Customer_Display(M_Customer_Entity customer) //TempoRegiKaiiKensaku
        {
            string sp = "M_Customer_Display";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Telephone",new ValuePair { value1=SqlDbType.VarChar,value2= customer.TelephoneNo1 } },
                //{ "@Tel2", new ValuePair { value1 = SqlDbType.VarChar, value2 = customer.TelephoneNo2 } },
                //{ "@Tel3",new ValuePair { value1=SqlDbType.VarChar,value2= customer.TelephoneNo3 } },
                { "@BirthDate", new ValuePair { value1 = SqlDbType.Date, value2 = customer.Birthdate } },
                { "@CustomerCD",new ValuePair { value1=SqlDbType.VarChar,value2= customer.CustomerCD } },
                { "@KanaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = customer.KanaName } },
                { "@CustomerName",new ValuePair { value1=SqlDbType.VarChar,value2= customer.CustomerName } },
                { "@MainStoreCD",new ValuePair { value1=SqlDbType.VarChar,value2= customer.MainStoreCD } }
            };
            return SelectData(dic, sp);
        }
       

        //search_customer
        public DataTable M_Customer_Search(M_Customer_Entity mce)
        {
            string sp = "M_Customer_Search";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@RefDate",new ValuePair{value1=SqlDbType.Date,value2=mce.RefDate} },
                {"@CustomerKBN",new ValuePair{value1=SqlDbType.TinyInt,value2=mce.CustomerKBN} },
                {"@CustName",new ValuePair{value1=SqlDbType.VarChar,value2=mce.CustomerName} },
                {"@KanaName",new ValuePair{value1=SqlDbType.VarChar,value2=mce.KanaName} },
                {"@BirthDate",new ValuePair{value1=SqlDbType.Date,value2=mce.Birthdate}},
                {"@TelephoneNo",new ValuePair{value1=SqlDbType.VarChar,value2=mce.TelephoneNo} },
                {"@StoreKBN" ,new ValuePair{value1=SqlDbType.TinyInt,value2=mce.StoreKBN} },
                {"@keyword1",new ValuePair{value1=SqlDbType.VarChar,value2=mce.Keyword1} },
                {"@keyword2",new ValuePair{value1=SqlDbType.VarChar,value2=mce.Keyword2} },
                {"@keyword3",new ValuePair{value1=SqlDbType.VarChar,value2=mce.Keyword3} },           
                {"@StoreCD",new ValuePair{value1=SqlDbType.VarChar,value2=mce.MainStoreCD} },
                {"@CustFrom",new ValuePair{value1=SqlDbType.VarChar,value2=mce.CustomerFrom} },
                {"@CustTo",new ValuePair{value1=SqlDbType.VarChar,value2=mce.CustomerTo} }
                //{"@KeyWordType",new ValuePair{value1=SqlDbType.TinyInt,value2=mce.KeyWordType} }
            };

            return SelectData(dic, sp);
        }

        /// <summary>	
        /// 得意先マスタ更新処理	
        /// MasterTouroku_Tokuisakiより更新時に使用	
        /// </summary>	
        /// <param name="mbe"></param>	
        /// <param name="operationMode"></param>	
        /// <param name="operatorNm"></param>	
        /// <param name="pc"></param>	
        /// <returns></returns>	
        public bool M_Customer_Exec(M_Customer_Entity me, short operationMode)
        {
            string sp = "PRC_MasterTouroku_Tokuisaki";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = operationMode.ToString() } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.CustomerCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = me.ChangeDate} },
                { "@VariousFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.VariousFLG } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.CustomerName } },
                { "@LastName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastName } },
                { "@FirstName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.FirstName} },
                { "@LongName1", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LongName1} },
                { "@LongName2", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LongName2} },
                { "@KanaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.KanaName } },
                { "@StoreKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.StoreKBN } },
                { "@CustomerKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.CustomerKBN } },
                { "@StoreTankaKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.StoreTankaKBN } },
                { "@AliasKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.AliasKBN } },
                { "@BillingType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.BillingType } },
                { "@GroupName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.GroupName} },
                { "@BillingFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.BillingFLG } },
                { "@CollectFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.CollectFLG } },
                { "@BillingCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.BillingCD} },
                { "@CollectCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.CollectCD} },
                { "@Birthdate", new ValuePair { value1 = SqlDbType.Date, value2 = me.Birthdate } },
                { "@Sex", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.Sex } },
                { "@Tel11", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Tel11 } },
                { "@Tel12", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Tel12 } },
                { "@Tel13", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Tel13 } },
                { "@Tel21", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Tel21 } },
                { "@Tel22", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Tel22 } },
                { "@Tel23", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Tel23 } },
                { "@ZipCD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ZipCD1} },
                { "@ZipCD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ZipCD2} },
                { "@Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Address1} },
                { "@Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Address2} },
                { "@MailAddress", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.MailAddress} },
                { "@TankaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.TankaCD} },
                { "@PointFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.PointFLG } },
                { "@LastPoint", new ValuePair { value1 = SqlDbType.Money , value2 = me.LastPoint } },
                { "@WaitingPoint", new ValuePair { value1 = SqlDbType.Money , value2 = me.WaitingPoint } },
                { "@TotalPoint", new ValuePair { value1 = SqlDbType.Money , value2 = me.TotalPoint } },	
                //{ "@TotalPurchase", new ValuePair { value1 = SqlDbType.Money , value2 = me.TotalPurchase } },	
                //{ "@UnpaidAmount", new ValuePair { value1 = SqlDbType.Money , value2 = me.UnpaidAmount } },	
                //{ "@UnpaidCount", new ValuePair { value1 = SqlDbType.Money , value2 = me.UnpaidCount } },	
                //{ "@LastSalesDate", new ValuePair { value1 = SqlDbType.Date, value2 = me.LastSalesDate} },	
                //{ "@LastSalesStoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastSalesStoreCD} },	
                { "@MainStoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.MainStoreCD} },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.StaffCD} },
                { "@AttentionFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.AttentionFLG} },
                { "@ConfirmFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.ConfirmFLG } },
                { "@ConfirmComment", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ConfirmComment} },
                { "@BillingCloseDate", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.BillingCloseDate} },
                { "@CollectPlanMonth", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.CollectPlanMonth} },
                { "@CollectPlanDate", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.CollectPlanDate } },
                { "@HolidayKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.HolidayKBN } },
                { "@TaxTiming", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.TaxTiming } },
                { "@TaxPrintKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.TaxPrintKBN  } },
                { "@TaxFractionKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.TaxFractionKBN } },
                { "@AmountFractionKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.AmountFractionKBN } },
                { "@CreditLevel", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.CreditLevel } },
                { "@CreditCard", new ValuePair { value1 = SqlDbType.Money , value2 = me.CreditCard } },
                { "@CreditInsurance", new ValuePair { value1 = SqlDbType.Money , value2 = me.CreditInsurance } },
                { "@CreditDeposit", new ValuePair { value1 = SqlDbType.Money , value2 = me.CreditDeposit } },
                { "@CreditETC", new ValuePair { value1 = SqlDbType.Money , value2 = me.CreditETC } },
                { "@CreditAmount", new ValuePair { value1 = SqlDbType.Money , value2 = me.CreditAmount } },
                { "@CreditWarningAmount", new ValuePair { value1 = SqlDbType.Money , value2 = me.CreditWarningAmount } },
                { "@CreditAdditionAmount", new ValuePair { value1 = SqlDbType.Money , value2 = me.CreditAdditionAmount } },
                { "@PaymentMethodCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.PaymentMethodCD } },
                { "@KouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.KouzaCD} },
                { "@DisplayOrder", new ValuePair { value1 = SqlDbType.Int, value2 = me.DisplayOrder } },
                { "@PaymentUnit", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.PaymentUnit } },
                { "@NoInvoiceFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.NoInvoiceFlg} },
                { "@CountryKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.CountryKBN } },
                { "@CountryName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.CountryName} },
                { "@RegisteredNumber", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.RegisteredNumber} },
                { "@DMFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.DMFlg } },
                { "@RemarksOutStore", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.RemarksOutStore} },
                { "@RemarksInStore", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.RemarksInStore} },
                { "@AnalyzeCD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.AnalyzeCD1} },
                { "@AnalyzeCD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.AnalyzeCD2} },
                { "@AnalyzeCD3", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.AnalyzeCD3} },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.DeleteFlg} },
                { "@UsedFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.UsedFlg} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.InsertOperator} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.PC} },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        }
}
