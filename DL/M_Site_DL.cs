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
    public class M_Site_DL : Base_DL
    {
        public DataTable M_Site_Select(M_Site_Entity mse)
        {
            string sp = "M_Site_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ItemSKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ItemSKUCD } },
                { "@APIKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.APIKey } }
            };
            
            return SelectData(dic, sp);
        }

        public DataTable M_Site_SelectAll(M_Site_Entity mbe)
        {
            string sp = "M_Site_SelectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                //{ "@DisplayKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.DisplayKbn } },
                //{ "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ChangeDate } },
                //{ "@SiteCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.SiteCDFrom } },
                //{ "@SiteCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.SiteCDTo } },
                //{ "@SiteName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.SiteName } },
            };
            
            return SelectData(dic, sp);
        }

        //public bool M_Site_Exec(M_Site_Entity mbe, short operationMode, string operatorNm, string pc)
        //{
        //    string sp = "PRC_MasterTouroku_Tempo";
        
        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    dic.Add("@OperateMode", operationMode.ToString());
        //    dic.Add("@SiteCD", mbe.SiteCD);
        //    dic.Add("@ChangeDate", mbe.ChangeDate);

        //    dic.Add("@SiteName", mbe.SiteName);
        //    dic.Add("@SiteKBN", mbe.SiteKBN);
        //    dic.Add("@SitePlaceKBN", mbe.SitePlaceKBN);
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
        //    dic.Add("@SiteCD", mbe.SiteCD);
        //    dic.Add("@Remarks", mbe.Remarks);
        //    dic.Add("@DeleteFlg", mbe.DeleteFlg);
        //    dic.Add("@UsedFlg", mbe.UsedFlg);
        //    dic.Add("@Operator", operatorNm);
        //    dic.Add("@PC", pc);

        //    return InsertUpdateDeleteData(dic, sp);
        //}
    }
}
