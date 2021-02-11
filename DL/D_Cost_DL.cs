using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;

namespace DL
{
    public class D_Cost_DL : Base_DL
    {
        public DataTable D_Cost_Search(D_Cost_Entity dcoste)
        {

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@RecordDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dcoste.RecordedDateFrom} },
                { "@RecordDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dcoste.RecordedDateTo} },
                { "@ExpanseEntryDateFrom", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dcoste.ExpanseEntryDateFrom} },
                { "@ExpanseEntryDateTo", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dcoste.ExpanseEntryDateTo} },
                { "@PaymentDueDateFrom", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dcoste.PaymentDueDateFrom} },
                { "@PaymentDueDateTo", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dcoste.PaymentDueDateTo} },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dcoste.StaffCD} },
                { "@PaymentDestinationCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dcoste.PayeeCD} },
                { "@Paid", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dcoste.Paid} },
                { "@Unpaid", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dcoste.Unpaid} },
                {"@RegFlg",new ValuePair{value1=SqlDbType.TinyInt,value2=dcoste.RegularlyFLG} },
                { "@PaymentDateFrom", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dcoste.PaymentDateFrom} },
                { "@PaymentDateTo", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dcoste.PaymentDateTo} },
            };
            return SelectData(dic, "D_Cost_Search");
        }
        public DataTable getPrintData(D_Cost_Entity dce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@RecordDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.RecordedDateFrom} },
                { "@RecordDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.RecordedDateTo} },
                { "@ExpanseEntryDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.PaymentDateFrom} },
                { "@ExpanseEntryDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.PaymentDateTo} },
                { "@InsertDateTimeFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.ExpanseEntryDateFrom} },
                { "@InsertDateTimeTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.ExpanseEntryDateTo} },
                { "@printTarget", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.PrintTarget} },
                { "@StoreName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.Store} },

            };
            return SelectData(dic, "D_KeihiPrint");

        }

        public DataTable D_Cost_Display(D_Cost_Entity cost)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CostNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.CostNO} }
            };
            return SelectData(dic, "D_Cost_Display");
        }

        public DataTable D_Cost_Copy_Display(D_Cost_Entity cost)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CostNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.CostNO} },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.VendorCD} },
                { "@RecordDate", new ValuePair { value1 = SqlDbType.Date, value2 = cost.RecordedDate} }
            };
            return SelectData(dic, "D_Cost_Copy_Display");
        }

        public bool KehiNyuuryoku_Insert_Update(D_Cost_Entity cost,int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@mode", new ValuePair { value1 = SqlDbType.Int, value2 = mode.ToString()} },
                { "@CostNOUpdate", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.CostNO} },
                { "@PayeeCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.PayeeCD} },
                { "@RecordDate", new ValuePair { value1 = SqlDbType.Date, value2 = cost.RecordedDate} },
                { "@PayPlanDate", new ValuePair { value1 = SqlDbType.Date, value2 = cost.PayPlanDate} },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.StaffCD} },
                { "@RegularFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = cost.RegularlyFLG} },
                { "@TotalGaku", new ValuePair { value1 = SqlDbType.Money, value2 = cost.TotalGaku} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.Operator} },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.Store} },
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = cost.xml1} },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.ProgramID} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.PC} },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.ProcessMode} }
                //{ "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.Key} }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "KehiNyuuryoku_Insert_Update");
        }

        public bool KehiNyuuryoku_Delete(D_Cost_Entity cost)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CostNOUpdate", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.CostNO} },
                { "@PayeeCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.PayeeCD} },
                { "@RecordDate", new ValuePair { value1 = SqlDbType.Date, value2 = cost.RecordedDate} },
                { "@PayPlanDate", new ValuePair { value1 = SqlDbType.Date, value2 = cost.PayPlanDate} },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.StaffCD} },
                { "@RegularFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = cost.RegularlyFLG} },
                { "@TotalGaku", new ValuePair { value1 = SqlDbType.Money, value2 = cost.TotalGaku} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.Operator} },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.Store} },
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = cost.xml1} },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.ProgramID} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.PC} },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = cost.ProcessMode} }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "KehiNyuuryoku_Delete");
        }
    }
}
