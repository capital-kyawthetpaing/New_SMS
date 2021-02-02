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
    public class M_MultiPorpose_DL : Base_DL
    {
        public DataTable M_MultiPorpose_Select(M_MultiPorpose_Entity mme)
        {
            string sp = "M_MultiPorpose_Select";
            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。 
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@ID", SqlDbType.Int).Value = mme.ID;
            //command.Parameters.Add("@Key", SqlDbType.VarChar).Value = mme.Key;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mme.ID } },
                { "@Key", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.Key } }
            };

            return SelectData(dic,sp);
            
        }

        public DataTable M_MultiPorpose_SelectAll(M_MultiPorpose_Entity mme)
        {
            string sp = "M_MultiPorpose_SelectAll";
            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@ID", SqlDbType.Int).Value = mme.ID;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mme.ID } }
            };

            return SelectData(dic,sp);
        }

        public DataTable M_MultiPorpose_SupplierSelect(M_MultiPorpose_Entity mme)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mme.ID } }
            };

            return SelectData(dic, "M_MultiPorpose_SupplierSelect");
        }

        public DataTable M_MultiPorpose_SelectByChar1(M_MultiPorpose_Entity mme)
        {
            string sp = "M_MultiPorpose_SelectByChar1";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mme.ID } },
                { "@Char1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.Char1 } }
            };
            return SelectData(dic, sp);
        }

        public DataTable M_MultiPorpose_AuxiliarySelect(M_MultiPorpose_Entity mme)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mme.ID } },
                { "@Key", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.Key } },
                { "@Char1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.Char1 } }
            };

            return SelectData(dic, "M_MultiPorpose_AuxiliarySelect");
        }

        //public bool M_MultiPorpose_Exec(M_MultiPorpose_Entity mbe, short operationMode, string operatorNm, string pc )
        //{
        //    string sp = "PRC_MasterTouroku_Tempo";

        //    //Todo:パラメータを型指定に変更
        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    dic.Add("@OperateMode", operationMode.ToString());
        //    dic.Add("@MultiPorposeCD", mbe.MultiPorposeCD);
        //    dic.Add("@ChangeDate", mbe.ChangeDate);

        //    dic.Add("@MultiPorposeName", mbe.MultiPorposeName);
        //    dic.Add("@MultiPorposeKBN", mbe.MultiPorposeKBN);
        //    dic.Add("@MultiPorposePlaceKBN", mbe.MultiPorposePlaceKBN);
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

        public DataTable M_Multipurpose_SelectIDName(string ID)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = ID } }
            };

            return SelectData(dic, "M_Multipurpose_SelectIDName");
        }
        public DataTable M_MultiPorpose_SaleModeSelect(M_MultiPorpose_Entity mme)
        {
            string sp = "M_MultiPorpose_SaleModeSelect";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mme.ID } },
            };

            return SelectData(dic, sp);

        }

        /// <summary>
        /// For TenzikaiShouhin
        /// </summary>
        /// <returns></returns>
        public DataTable M_Multipurpose_SelectAll_ByID(int Type)
        {
            string sp = "M_Multipurpose_SelectAll_ByID";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@Type", new ValuePair { value1 = SqlDbType.Int, value2 = Type.ToString() } },
            };
            return SelectData(dic, sp);
        }
        public DataTable M_Multipurpose_SelectAll()
        {
            string sp = "M_Multipurpose_SelectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                
            };
            return SelectData(dic, sp);
        }
    }
}
