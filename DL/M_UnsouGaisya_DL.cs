using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using System.Data;
using Entity;

namespace DL
{
   public class M_UnsouGaisya_DL:Base_DL
    {
        public DataTable M_CarrierSelect(M_Shipping_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   { "@ShippingCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ShippingCD } },
                   { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
               };
            UseTransaction = true;
            return SelectData(dic, "M_Carrier_Select");
        }

        public bool M_Gaisya_InsertUpdate(M_Shipping_Entity mse,int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   { "@ShippingCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ShippingCD } },
                   { "@ChangeDate", new ValuePair { value1 = System.Data.SqlDbType.Date, value2 = mse.ChangeDate } },
                   { "@ShippingName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ShippingName } },
                   { "@Identify", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.CarrierFlg } },
                   { "@Normal", new ValuePair {value1 = SqlDbType.TinyInt, value2 = mse.NormalFlg} },
                   { "@YahooCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.YahooCD } },
                   { "@RakutenCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.RakutenCD } },
                   { "@AmazonCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.AmazonCD } },
                   { "@WowmaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.WowmaCD } },
                   { "@PonpareCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PonpareCD } },
                   { "@OtherCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.OtherCD } },
                   { "@Remarks", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Remarks } },
                   { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } },
                   { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.InsertOperator } },
                   { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProgramID } },
                   { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PC } },
                   { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProcessMode } },
                   { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ShippingCD +" "+ mse.ChangeDate } },
                   { "@Mode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mode.ToString() } }
               };
            return InsertUpdateDeleteData(dic, "M_Carrier_Insert_Update");
        }

        public bool M_Gaisya_Delete(M_Shipping_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ShippingCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mse.ShippingCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ShippingCD +""+ mse.ChangeDate } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Carrier_Delete");
        }

        public DataTable M_Carrier_Search(M_Shipping_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DisplayKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 =mse.DisplayKBN } },
                { "@ShippingCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 =mse.ShippingCDFrom } },
                { "@ShippingCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 =mse.ShippingCDTo } },
                { "@ShippingName", new ValuePair { value1 = SqlDbType.VarChar, value2 =mse.ShippingName } },
                {"@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.DeleteFlg } },

            };
            UseTransaction = true;
            return SelectData(dic, "M_Carrier_Search");
        }

        //public DataTable M_Carrirer_Bind(M_Carrier_Entity mce)
        //{
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        {"ChangeDate", new ValuePair {value1= SqlDbType.Date,value2 = mce.ChangeDate} },
        //        {"DeleteFlg", new ValuePair {value1 = SqlDbType.TinyInt ,value2 = mce.DeleteFlg} }
        //    };
        //    UseTransaction = true;
        //    return SelectData(dic,"M_Carrier_Bind");
        //}

    }
}
