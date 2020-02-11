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
    public class M_SalesTax_DL : Base_DL
    {
        public DataTable M_SalesTax_Select(M_SalesTax_Entity mke)
        {
            string sp = "M_SalesTax_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.ChangeDate } }
            };
            
            return SelectData(dic, sp);
        }

        //public DataTable M_SalesTax_SelectAll(M_SalesTax_Entity mbe)
        //{
        //    string sp = "M_SalesTax_SelectAll";
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        //{ "@DisplayKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.DisplayKbn } },
        //        //{ "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ChangeDate } },
        //        //{ "@SalesTaxCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.SalesTaxCDFrom } },
        //        //{ "@SalesTaxCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.SalesTaxCDTo } },
        //        //{ "@SalesTaxName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.SalesTaxName } },
        //    };
            
        //    return SelectData(dic, sp);
        //}

        //public bool M_Customer_Exec(M_Customer_Entity mbe, short operationMode, string operatorNm, string pc)
        //{
        //    string sp = "PRC_MasterTouroku_Tempo";
        
        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    dic.Add("@OperateMode", operationMode.ToString());
        //    dic.Add("@CustomerCD", mbe.CustomerCD);
        //    dic.Add("@ChangeDate", mbe.ChangeDate);

        //    dic.Add("@CustomerName", mbe.CustomerName);
        //    dic.Add("@CustomerKBN", mbe.CustomerKBN);
        //    dic.Add("@CustomerPlaceKBN", mbe.CustomerPlaceKBN);
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
        //    dic.Add("@CustomerCD", mbe.CustomerCD);
        //    dic.Add("@Remarks", mbe.Remarks);
        //    dic.Add("@DeleteFlg", mbe.DeleteFlg);
        //    dic.Add("@UsedFlg", mbe.UsedFlg);
        //    dic.Add("@Operator", operatorNm);
        //    dic.Add("@PC", pc);

        //    return InsertUpdateDeleteData(dic, sp);
        //}
    }
}
