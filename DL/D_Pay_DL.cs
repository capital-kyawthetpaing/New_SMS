using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class D_Pay_DL : Base_DL
    {
        public DataTable D_Pay_LargePayNoSelect(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@LargePayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.LargePayNO} }
            };
            return SelectData(dic, "D_Pay_LargePayNoSelect");
        }

        public DataTable D_Pay_PayNoSelect(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
           {
             {"@PayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.PayNo} }
           };
            return SelectData(dic, "D_Pay_PayNoSelect");
        }

        public DataTable D_Pay_SelectForPrint(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@PurchaseDateFrom" , new ValuePair{value1=SqlDbType.Date, value2=dpe.PurchaseDateFrom}},
                {"@PurchaseDateTo", new ValuePair{value1=SqlDbType.Date ,value2=dpe.PurchaseDateTo} },
                {"@VendorCD" ,new ValuePair{value1=SqlDbType.VarChar,value2=dpe.PayeeCD}},
                {"@StaffCD" ,new ValuePair{value1=SqlDbType.VarChar,value2=dpe.StaffCD} }
            };
            return SelectData(dic, "D_Pay_ShiharaiItianHyou");
        }

        public DataTable D_Pay_Select1(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@LargePayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.LargePayNO} },
                {"@PayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.PayNo} }
            };
            return SelectData(dic, "D_Pay_Select1");
        }

        public DataTable D_Pay_Select2(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@LargePayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.LargePayNO} },
                {"@PayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.PayNo} },
                {"@VendorCD", new ValuePair {value1 = SqlDbType.VarChar, value2= dpe.PayeeCD } },
                {"@PayeeDate", new ValuePair {value1 = SqlDbType.Date, value2= dpe.PayPlanDate} }
            };
            return SelectData(dic, "D_Pay_Select2");
        }

        public DataTable D_Pay_Select3(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@LargePayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.LargePayNO} },
                {"@PayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.PayNo} }
            };
            return SelectData(dic, "D_Pay_Select3");
        }

        public DataTable D_Pay_Search(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@PayDateFrom", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.PayDateFrom} },
                {"@PayDateTo", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.PayDateTo} },
                {"@InputDateTimeFrom", new ValuePair {value1 = SqlDbType.Int,value2 = dpe.InputDateTimeFrom} },
                {"@InputDateTimeTo", new ValuePair {value1 = SqlDbType.Int,value2 = dpe.InputDateTimeTo} }
            };
            return SelectData(dic, "D_Pay_Search");
        }

        public DataTable D_Pay_SelectForSiharaiNo(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@PayDateFrom", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.PayDateFrom} },
                {"@PayDateTo", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.PayDateTo} },
                {"@InputDateTimeFrom", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.InputDateTimeFrom} },
                {"@InputDateTimeTo", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.InputDateTimeTo} },
                {"@PayeeCD", new ValuePair {value1 = SqlDbType.VarChar, value2 = dpe.PayeeCD} }
            };
            return SelectData(dic, "D_Pay_SelectForSiharaiNo");
        }

        public bool D_Pay_Insert(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StaffCD } },
                { "@PayDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PayDate } },
                { "@LocationXml", new ValuePair { value1 = SqlDbType.Xml, value2 = dpe.LocationXml } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.Key } },
            };

            return InsertUpdateDeleteData(dic, "D_Pay_Insert");
        }
    }
}
