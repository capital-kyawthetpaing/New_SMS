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
    public class M_Program_DL : Base_DL
    {
        public DataTable M_Program_Select(M_Program_Entity mpe)
        {
            string sp = "M_Program_Select";
            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            command.Parameters.Add("@ProgramID", SqlDbType.VarChar).Value = mpe.ProgramID;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ProgramID", new ValuePair { value1 = SqlDbType.VarChar, value2 = mpe.ProgramID } }
            };

            //return SelectData(sp);
            return SelectData(dic,sp);
            
        }
        //public DataTable M_Program_Select(M_Program_Entity mpe)
        //{

        //}

    //    public DataTable Store_SelectAll(M_Store_Entity mbe)
    //    {
    //        string sp = "M_Store_SelectAll";

    //        command = new SqlCommand(sp, GetConnection());
    //        command.CommandType = CommandType.StoredProcedure;
    //        command.CommandTimeout = 0;

    //        command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mbe.ChangeDate;
    //        command.Parameters.Add("@StoreCDFrom", SqlDbType.VarChar).Value = mbe.StoreCDFrom;
    //        command.Parameters.Add("@StoreCDTo", SqlDbType.VarChar).Value = mbe.StoreCDTo;
    //        command.Parameters.Add("@StoreName", SqlDbType.VarChar).Value = mbe.StoreName;
    //        command.Parameters.Add("@StoreKBN1", SqlDbType.TinyInt).Value = mbe.StoreKBN1;
    //        command.Parameters.Add("@StoreKBN2", SqlDbType.TinyInt).Value = mbe.StoreKBN2;
    //        command.Parameters.Add("@StoreKBN3", SqlDbType.TinyInt).Value = mbe.StoreKBN3;

    //        return SelectData(sp);
    //    }

    //    public bool Store_Exec(M_Store_Entity mbe, short operationMode, string operatorNm, string pc )
    //    {
    //        Dictionary<string, string> dic = new Dictionary<string, string>();

    //        dic.Add("@OperateMode", operationMode.ToString());
    //        dic.Add("@StoreCD", mbe.StoreCD);
    //        dic.Add("@ChangeDate", mbe.ChangeDate);

    //        dic.Add("@StoreName", mbe.StoreName);
    //        dic.Add("@StoreKBN", mbe.StoreKBN);
    //        dic.Add("@StorePlaceKBN", mbe.StorePlaceKBN);
    //        dic.Add("@MallCD", mbe.MallCD);
    //        dic.Add("@ZipCD1", mbe.ZipCD1);
    //        dic.Add("@ZipCD2", mbe.ZipCD2);
    //        dic.Add("@Address1", mbe.Address1);
    //        dic.Add("@Address2", mbe.Address2);
    //        dic.Add("@TelphoneNO", mbe.TelphoneNO);
    //        dic.Add("@FaxNO", mbe.FaxNO);
    //        dic.Add("@MailAddress", mbe.MailAddress);
    //        dic.Add("@ApprovalStaffCD11", mbe.ApprovalStaffCD11);
    //        dic.Add("@ApprovalStaffCD12", mbe.ApprovalStaffCD12);
    //        dic.Add("@ApprovalStaffCD21", mbe.ApprovalStaffCD21);
    //        dic.Add("@ApprovalStaffCD22", mbe.ApprovalStaffCD22);
    //        dic.Add("@ApprovalStaffCD31", mbe.ApprovalStaffCD31);
    //        dic.Add("@ApprovalStaffCD32", mbe.ApprovalStaffCD32);
    //        dic.Add("@DeliveryDate", mbe.DeliveryDate);
    //        dic.Add("@PaymentTerms", mbe.PaymentTerms);
    //        dic.Add("@DeliveryPlace", mbe.DeliveryPlace);
    //        dic.Add("@ValidityPeriod", mbe.ValidityPeriod);
    //        dic.Add("@Print1", mbe.Print1);
    //        dic.Add("@Print2", mbe.Print2);
    //        dic.Add("@Print3", mbe.Print3);
    //        dic.Add("@Print4", mbe.Print4);
    //        dic.Add("@Print5", mbe.Print5);
    //        dic.Add("@Print6", mbe.Print6);
    //        dic.Add("@KouzaCD", mbe.KouzaCD);
    //        dic.Add("@Remarks", mbe.Remarks);
    //        dic.Add("@DeleteFlg", mbe.DeleteFlg);
    //        dic.Add("@UsedFlg", mbe.UsedFlg);
    //        dic.Add("@Operator", operatorNm);
    //        dic.Add("@PC", pc);

    //        return InsertUpdateDeleteData(dic, "PRC_MasterTouroku_Tempo");
    //    }
    }
}
