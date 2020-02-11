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

        public DataTable M_Customer_Select(M_Customer_Entity mke)
        {
            //string sp = "M_Customer_Select";
            string sp = "M_Customer_SelectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.CustomerCD } }
                //{ "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.ChangeDate } }
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
                {"@MailAddress2" , new ValuePair{value1=SqlDbType.VarChar,value2=cust.MailAddress2}},
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
                { "@CustomerName",new ValuePair { value1=SqlDbType.VarChar,value2= customer.CustomerName } }
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
    }
}
