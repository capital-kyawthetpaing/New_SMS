using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_TankaCD_DL : Base_DL
    {
        public DataTable M_TankaCD_Select(M_TankaCD_Entity mte)
        {
            string sp = "M_TankaCD_Select";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@TankaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mte.TankaCD        } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mte.ChangeDate } }
            };
            
            return SelectData(dic, sp);
        }

        public DataTable M_TankaCD_SelectAll(M_TankaCD_Entity mbe)
        {
            string sp = "M_TankaCD_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DisplayKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.DisplayKbn } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ChangeDate } },
                { "@TankaCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.TankaCDFrom } },
                { "@TankaCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.TankaCDTo } },
                { "@TankaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.TankaName } },
            };

            return SelectData(dic, sp);
        }

        //public bool M_TankaCD_Exec(M_TankaCD_Entity mbe, short operationMode, string operatorNm, string pc )
        //{
        //    string sp = "PRC_MasterTouroku_Tempo";

        //    //Todo:パラメータを型指定に変更
        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    dic.Add("@OperateMode", operationMode.ToString());
        //    dic.Add("@TankaCDCD", mbe.TankaCDCD);
        //    dic.Add("@ChangeDate", mbe.ChangeDate);

        //    dic.Add("@TankaCDName", mbe.TankaCDName);
        //    dic.Add("@TankaCDKBN", mbe.TankaCDKBN);
        //    dic.Add("@TankaCDPlaceKBN", mbe.TankaCDPlaceKBN);
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
        //    dic.Add("@TankaCDCD", mbe.TankaCDCD);
        //    dic.Add("@Remarks", mbe.Remarks);
        //    dic.Add("@DeleteFlg", mbe.DeleteFlg);
        //    dic.Add("@UsedFlg", mbe.UsedFlg);
        //    dic.Add("@Operator", operatorNm);
        //    dic.Add("@PC", pc);

        //    return InsertUpdateDeleteData(dic, sp);
        //}

        public DataTable M_TankaCD_SelectAll_NoPara()
        {
            string sp = "M_TankaCD_SelectAll_NoPara";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
            };
            return SelectData(dic, sp);
        }
    }
}
