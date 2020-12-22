using System.Collections.Generic;
using Entity;
using System.Data;
using System.Data.SqlClient;

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
                {"@InputDateTimeFrom", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.InputDateTimeFrom} },
                {"@InputDateTimeTo", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.InputDateTimeTo} }
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
                { "@Xml1", new ValuePair { value1 = SqlDbType.Xml, value2 = dpe.xml1 } },
                { "@Xml2", new ValuePair{value1 = SqlDbType.Xml, value2 = dpe.xml2 } },
                { "@Xml3", new ValuePair{value1 = SqlDbType.Xml,value2 = dpe.xml3} },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PC } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.Operator } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StoreCD } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.ProcessMode } },
                { "@TotalPayGaku", new ValuePair { value1 = SqlDbType.Money, value2 = dpe.PayGakuTotol } },
                //{ "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StoreCD +" "+ dpe.Key  } }
            };

            return InsertUpdateDeleteData(dic, "D_Pay_Insert");
        }

        public bool D_Pay_Update(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StaffCD } },
                { "@PayDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PayDate } },
                { "@Xml1", new ValuePair { value1 = SqlDbType.Xml, value2 = dpe.xml4 } },
                { "@Xml2", new ValuePair{value1 = SqlDbType.Xml, value2 = dpe.xml5 } },
                { "@Xml3", new ValuePair{value1 = SqlDbType.Xml,value2 = dpe.xml6} },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PC } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.Operator } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StoreCD } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.ProcessMode } },
                { "@TotalPayGaku", new ValuePair { value1 = SqlDbType.Money, value2 = dpe.PayGakuTotol } },
                { "@PayNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PayNo } },
                {"@LargePayNO", new ValuePair{value1 = SqlDbType.VarChar, value2 = dpe.LargePayNO} }

            };

            return InsertUpdateDeleteData(dic, "D_Pay_Update");
        }

        public bool D_Pay_Delete(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PayNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PayNo } },
                { "@LargePayNO", new ValuePair{value1 = SqlDbType.VarChar, value2 = dpe.LargePayNO} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PC } },               
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.ProcessMode } }           
                //{ "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StoreCD +" "+ dpe.Key  } }
            };

            return InsertUpdateDeleteData(dic, "D_Pay_Delete");
        }

        public bool D_Siharai_Exec(D_Pay_Entity dpe, DataTable dt, DataTable dtD, short operationMode)
        {
            string sp = "PRC_SiharaiToroku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@PayNo", SqlDbType.VarChar, dpe.PayNo);
            AddParam(command, "@LargePayNO", SqlDbType.VarChar, dpe.LargePayNO);
            //AddParam(command, "@StoreCD", SqlDbType.VarChar, dpe.StoreCD);

            AddParam(command, "@PayDate", SqlDbType.VarChar, dpe.PayDate);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, dpe.StaffCD);      

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParamForDataTable(command, "@TableD", SqlDbType.Structured, dtD);
            AddParam(command, "@Operator", SqlDbType.VarChar, dpe.Operator);
            AddParam(command, "@PC", SqlDbType.VarChar, dpe.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutPayNo";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dpe.PayNo = outPutParam;

            return ret;
        }
        public DataTable D_Pay_Select01(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@LargePayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.LargePayNO} },
                {"@PayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.PayNo} }
            };
            return SelectData(dic, "D_Pay_Select01");
        }
        public DataTable D_Pay_Select02(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@LargePayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.LargePayNO} },
                {"@PayNo", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.PayNo} },
                //{"@VendorCD", new ValuePair {value1 = SqlDbType.VarChar, value2= dpe.PayeeCD } },
                //{"@PayeeDate", new ValuePair {value1 = SqlDbType.Date, value2= dpe.PayPlanDate} }
            };
            return SelectData(dic, "D_Pay_Select02");
        }

        public DataTable D_Pay_SelectForFB(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@MotoKouzaCD", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.MotoKouzaCD} },
                {"@PayDate", new ValuePair {value1 = SqlDbType.Date,value2 = dpe.PayDate} },
                {"@Flg", new ValuePair {value1 = SqlDbType.TinyInt,value2 = dpe.Flg} }
            };
            return SelectData(dic, "D_Pay_SelectForFB");
        }

        public DataTable D_Pay_SelectForText(D_Pay_Entity dpe,D_FBControl_Entity dfbe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@MotoKouzaCD", new ValuePair {value1 = SqlDbType.VarChar,value2 = dfbe.MotoKouzaCD} },
                {"@PayDate", new ValuePair {value1 = SqlDbType.Date,value2 = dfbe.PayDate} },
                {"@Flg", new ValuePair {value1 = SqlDbType.TinyInt,value2 = dpe.Flg} },
                { "@ActualPayDate", new ValuePair { value1 = SqlDbType.Date, value2 = dfbe.ActualPayDate } },
            };
            return SelectData(dic, "D_Pay_SelectForText");
        }
    }
}
