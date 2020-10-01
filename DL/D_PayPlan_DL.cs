using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class D_PayPlan_DL : Base_DL
    {
        /// <summary>
        /// 締処理済チェック　D_PayPlanに、支払締番号がセットされていれば、エラー（下記のSelectができたらエラー）
        /// </summary>
        /// <param name="No"></param>
        /// <returns></returns>
        public DataTable CheckPayPlanData(string No)
        {
            string sp = "CheckPayPlanData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PurchaseNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = No } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 入金予定表よりデータ抽出時に使用
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        public DataTable D_PayPlan_SelectForPrint(D_PayPlan_Entity dppe, int type)
        {
            string sp = "D_PayPlan_SelectforPrint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PaymentDueDateFrom", new ValuePair { value1 = SqlDbType.Date, value2 = dppe.PaymentDueDateFrom } },
                { "@PaymentDueDateTo", new ValuePair { value1 = SqlDbType.Date, value2 = dppe.PaymenetDueDateTo } },
                { "@PaymentCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dppe.PayeeCD } },
                { "@ClosedStatusSumi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dppe.CloseStatusSumi } },
                { "@PaymentStatusUnpaid", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dppe.PaymentStatusUnpaid } },
                { "@Purchase", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dppe.Purchase } },
                { "@Expense", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dppe.Expense } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dppe.StoreCD } },
                {"@Type",new ValuePair{value1=SqlDbType.TinyInt,value2=type.ToString()} }
            };

            return SelectData(dic, sp);
        }
        public DataTable D_Pay_SelectForPayPlanDate1(D_PayPlan_Entity dppe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@PayeePlanDateFrom",new ValuePair {value1 = SqlDbType.Date,value2 = dppe.PayPlanDateFrom} },
                {"@PayeePlanDateTo",new ValuePair {value1 = SqlDbType.Date, value2= dppe.PayPlanDateTo} },
                {"@Operator",new ValuePair{value1 = SqlDbType.VarChar,value2 = dppe.Operator } },
                {"@PayeeCD",new ValuePair {value1 =SqlDbType.VarChar ,value2 =dppe.PayeeCD} }
            };
            return SelectData(dic, "D_Pay_SelectForPayPlanDate1");
        }

        public DataTable D_Pay_SelectForPayPlanDate2(D_PayPlan_Entity dppe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                //{"@PayPlanDate",new ValuePair {value1 = SqlDbType.Date,value2 = dppe.PayPlanDate} },            
                //{"@Operator",new ValuePair{value1 = SqlDbType.VarChar,value2 = dppe.Operator } },
                //{"@PayeeCD",new ValuePair {value1 =SqlDbType.VarChar ,value2 =dppe.PayeeCD} }
                {"@PayeePlanDateFrom",new ValuePair {value1 = SqlDbType.Date,value2 = dppe.PayPlanDateFrom} },
                {"@PayeePlanDateTo",new ValuePair {value1 = SqlDbType.Date, value2= dppe.PayPlanDateTo} },
                {"@Operator",new ValuePair{value1 = SqlDbType.VarChar,value2 = dppe.Operator } },
                {"@PayeeCD",new ValuePair {value1 =SqlDbType.VarChar ,value2 =dppe.PayeeCD} }
            };
            return SelectData(dic, "D_Pay_SelectForPayPlanDate2");
        }

        /// <summary>
        /// SiharaiTourokuデータ抽出時
        /// </summary>
        /// <param name="dppe"></param>
        /// <returns></returns>
        public DataTable D_PayPlan_Select(D_PayPlan_Entity dppe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@PayeePlanDateFrom",new ValuePair {value1 = SqlDbType.Date,value2 = dppe.PayPlanDateFrom} },
                {"@PayeePlanDateTo",new ValuePair {value1 = SqlDbType.Date, value2= dppe.PayPlanDateTo} },
                {"@Operator",new ValuePair{value1 = SqlDbType.VarChar,value2 = dppe.Operator } },
                {"@PayeeCD",new ValuePair {value1 =SqlDbType.VarChar ,value2 =dppe.PayeeCD} }
            };
            return SelectData(dic, "D_PayPlan_Select");
        }
        public DataTable D_PayPlan_SelectDetail(D_PayPlan_Entity dppe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@PayeePlanDateFrom",new ValuePair {value1 = SqlDbType.Date,value2 = dppe.PayPlanDateFrom} },
                {"@PayeePlanDateTo",new ValuePair {value1 = SqlDbType.Date, value2= dppe.PayPlanDateTo} },
                {"@Operator",new ValuePair{value1 = SqlDbType.VarChar,value2 = dppe.Operator } },
                {"@PayeeCD",new ValuePair {value1 =SqlDbType.VarChar ,value2 =dppe.PayeeCD} }
            };
            return SelectData(dic, "D_PayPlan_SelectDetail");
        }
        public DataTable D_PayPlanValue_Select(D_PayPlan_Entity dpp_e,string type)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {

                 {"@PaymentCloseDate",new ValuePair {value1 = SqlDbType.Date,value2 = dpp_e.PayCloseDate} },
                 {"@PayeeCD",new ValuePair {value1 = SqlDbType.VarChar,value2 = dpp_e.PayeeCD} },
                 {"@Type",new ValuePair {value1 = SqlDbType.VarChar,value2 = type} }
            };
            return SelectData(dic, "D_PayPlanValue_Select");
        }
    }
}
