using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

using System.Data.SqlClient;

namespace DL
{
    public class M_Kouza_DL : Base_DL
    {
        public DataTable M_Kouza_Select(M_Kouza_Entity mke)
        {
            string sp = "M_Kouza_Select";
            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@KouzaCD", SqlDbType.VarChar).Value = mke.KouzaCD;
            //command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mke.ChangeDate;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@KouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.KouzaCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.ChangeDate } }
            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }

        public DataTable M_Kouza_SelectByKouzaCD(M_Kouza_Entity mke)
        {
            string sp = "M_Kouza_SelectByKouzaCD";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@KouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.KouzaCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.ChangeDate } }
            };

            return SelectData(dic, sp);
        }

        public DataTable M_Kouza_SelectByDate(M_Kouza_Entity mke)
        {
            string sp = "M_Kouza_SelectByDate";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.ChangeDate } }
            };

            return SelectData(dic, sp);
        }

        public DataTable M_Kouza_SelectAll(M_Kouza_Entity mbe)
        {
            string sp = "M_Kouza_SelectAll";
            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@DisplayKbn", SqlDbType.TinyInt).Value = mbe.DisplayKbn;
            //command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mbe.ChangeDate;
            //command.Parameters.Add("@KouzaCDFrom", SqlDbType.VarChar).Value = mbe.KouzaCDFrom;
            //command.Parameters.Add("@KouzaCDTo", SqlDbType.VarChar).Value = mbe.KouzaCDTo;
            //command.Parameters.Add("@KouzaName", SqlDbType.VarChar).Value = mbe.KouzaName;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DisplayKbn", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.DisplayKbn } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ChangeDate } },
                { "@KouzaCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.KouzaCDFrom } },
                { "@KouzaCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.KouzaCDTo } },
                { "@KouzaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.KouzaName } },
            };
            //command.Parameters.Add("@KouzaKBN1", SqlDbType.TinyInt).Value = mbe.KouzaKBN1;
            //command.Parameters.Add("@KouzaKBN2", SqlDbType.TinyInt).Value = mbe.KouzaKBN2;
            //command.Parameters.Add("@KouzaKBN3", SqlDbType.TinyInt).Value = mbe.KouzaKBN3;

            //return SelectData(sp);
            return SelectData(dic, sp);
        }

        public bool M_Kouza_Insert_Update(M_Kouza_Entity mkze, int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@KouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.KouzaCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mkze.ChangeDate } },
                { "@KouzaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.KouzaName } },
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.BankCD } },
                { "@BranchCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.BranchCD } },
                { "@KouzaKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mkze.KouzaKBN } },
                { "@KouzaMeigi", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.KouzaMeigi } },
                { "@KouzaNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.KouzaNO } },
                { "@Print1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Print1 } },
                { "@Print2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Print2 } },
                { "@Print3", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Print3 } },
                { "@Print4", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Print4 } },

                { "@Fee11", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Fee11 } },
                { "@Tax11", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Tax11 } },
                { "@Amount1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Amount1 } },
                { "@Fee12", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Fee12 } },
                { "@Tax12", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Tax12 } },

                { "@Fee21", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Fee21 } },
                { "@Tax21", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Tax21 } },
                { "@Amount2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Amount2 } },
                { "@Fee22", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Fee22 } },
                { "@Tax22", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Tax22 } },

                { "@Fee31", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Fee31 } },
                { "@Tax31", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Tax31 } },
                { "@Amount3", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Amount3 } },
                { "@Fee32", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Fee32 } },
                { "@Tax32", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Tax32 } },

                { "@CompanyCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.CompanyCD } },
                { "@CompanyName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.CompanyName } },
                { "@Remarks", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Remarks } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mkze.DeleteFlg } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Key } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Kouza_Insert_Update");
        }

        public bool M_Kouza_Delete(M_Kouza_Entity mkze)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                { "@KouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.KouzaCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mkze.ChangeDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.Key } }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Kouza_Delete");
        }

        public DataTable M_Vendor_Kouza_Select(M_Kouza_Entity mke)
        {
            string sp = "M_Vendor_Kouza_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@KouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.KouzaCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.ChangeDate } }
            };

            return SelectData(dic, sp);
        }

        //public bool M_Kouza_Exec(M_Kouza_Entity mbe, short operationMode, string operatorNm, string pc)
        //{
        //    string sp = "PRC_MasterTouroku_Tempo";

        //    //Todo:パラメータを型指定に変更
        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    dic.Add("@OperateMode", operationMode.ToString());
        //    dic.Add("@KouzaCD", mbe.KouzaCD);
        //    dic.Add("@ChangeDate", mbe.ChangeDate);

        //    dic.Add("@KouzaName", mbe.KouzaName);
        //    dic.Add("@KouzaKBN", mbe.KouzaKBN);
        //    dic.Add("@KouzaPlaceKBN", mbe.KouzaPlaceKBN);
        //    dic.Add("@MallCD", mbe.MallCD);
        //    dic.Add("@ZipCD1", mbe.ZipCD1);
        //    dic.Add("@ZipCD2", mbe.ZipCD2);
        //    dic.Add("@Address1", mbe.Address1);
        //    dic.Add("@Address2", mbe.Address2);
        //    dic.Add("@TelphoneNO", mbe.TelphoneNO);
        //    dic.Add("@FaxNO", mbe.FaxNO);
        //    dic.Add("@MailAddress", mbe.MailAddress);
        //    dic.Add("@ApprovalStaffCD11", mbe.ApprovalStaffCD11);
        //    dic.Add("@ApprovalStaffCD12", mbe.ApprovalStaffCD12);
        //    dic.Add("@ApprovalStaffCD21", mbe.ApprovalStaffCD21);
        //    dic.Add("@ApprovalStaffCD22", mbe.ApprovalStaffCD22);
        //    dic.Add("@ApprovalStaffCD31", mbe.ApprovalStaffCD31);
        //    dic.Add("@ApprovalStaffCD32", mbe.ApprovalStaffCD32);
        //    dic.Add("@DeliveryDate", mbe.DeliveryDate);
        //    dic.Add("@PaymentTerms", mbe.PaymentTerms);
        //    dic.Add("@DeliveryPlace", mbe.DeliveryPlace);
        //    dic.Add("@ValidityPeriod", mbe.ValidityPeriod);
        //    dic.Add("@Print1", mbe.Print1);
        //    dic.Add("@Print2", mbe.Print2);
        //    dic.Add("@Print3", mbe.Print3);
        //    dic.Add("@Print4", mbe.Print4);
        //    dic.Add("@Print5", mbe.Print5);
        //    dic.Add("@Print6", mbe.Print6);
        //    dic.Add("@KouzaCD", mbe.KouzaCD);
        //    dic.Add("@Remarks", mbe.Remarks);
        //    dic.Add("@DeleteFlg", mbe.DeleteFlg);
        //    dic.Add("@UsedFlg", mbe.UsedFlg);
        //    dic.Add("@Operator", operatorNm);
        //    dic.Add("@PC", pc);

        //    return InsertUpdateDeleteData(dic, sp);
        //}

        public DataTable M_Kouza_Bind(M_Kouza_Entity mke)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mke.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mke.DeleteFlg } },
            };
            return SelectData(dic, "M_Kouza_Bind");
        }

        public DataTable M_Kouza_FeeSelect(M_Kouza_Entity mkze)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@KouzaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.KouzaCD } },
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.BankCD } },
                { "@BranchCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mkze.BranchCD } },
                { "@Amount", new ValuePair { value1 = SqlDbType.Money, value2 = mkze.Amount } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mkze.ChangeDate } },

            };
            return SelectData(dic, "M_Kouza_FeeSelect");
        }
    }
}
