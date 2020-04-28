using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_Souko_DL : Base_DL
    {
        public DataTable M_Souko_Search(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Fields", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.FieldsName } },
                { "@SoukoCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCDFrom } },
                { "@SoukoCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCDTo } },
                { "@SoukoName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoName } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SoukoType } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.DeleteFlg } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
               // { "@OrderBy", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.OrderBy } },
                {"@SearchType" , new ValuePair{value1=SqlDbType.VarChar, value2=mse.searchType } },
            };
            return SelectData(dic, "M_Souko_Search");
        }

        public DataTable M_Souko_ZipcodeAddressSelect(M_Souko_Entity mse)
        {
            string sp = "M_Souko_ZipcodeAddressSelect";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                {"@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCD } },
                {"@ZipCD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ZipCD1 } },
                {"@ZipCD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ZipCD2 } }
            };

            return SelectData(dic, sp);

        }
        public DataTable M_Souko_Bind(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SoukoType } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.DeleteFlg } },
            };
            return SelectData(dic, "M_Souko_Bind");
        }


        /// <summary>
        /// Select Souko's info
        /// SoukoType 3,4のSouko情報をSelect
        /// </summary>
        /// <param name="mse">Souko info</param>
        /// <returns></returns>
        public DataTable M_Souko_SelectForMitsumori(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } }
            };
            return SelectData(dic, "M_Souko_SelectForMitsumori");
        }


        /// <summary>
        /// Select Souko's info
        /// SoukoType 3のSouko情報をSelect
        /// </summary>
        /// <param name="mse">Souko info</param>
        /// <returns></returns>
        public DataTable M_Souko_SelectForNyuuka(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SoukoType } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } }
            };
            return SelectData(dic, "M_Souko_SelectForNyuuka");
        }

        public DataTable M_Souko_BindForNyuuka(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SoukoType } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.DeleteFlg } },
            };
            return SelectData(dic, "M_Souko_BindForNyuuka");
        }
        public DataTable M_Souko_BindForHenpin(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SoukoType } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.DeleteFlg } },
            };
            return SelectData(dic, "M_Souko_BindForHenpin");
        }

        public DataTable M_Souko_BindForShukka(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.DeleteFlg } },
            };
            return SelectData(dic, "M_Souko_BindForShukka");
        }
        public DataTable M_Souko_Select(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } }
            };
            return SelectData(dic, "M_Souko_Select");
        }

        public DataTable M_Souko_IsExists(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCD });
            dic.Add("@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate });
            dic.Add("@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg });

            return SelectData(dic, "M_Souko_IsExists");
        }

        public bool M_Souko_Insert_Update(M_Souko_Entity mse, int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@SoukoName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoName } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@HikiateOrder", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.HikiateOrder } },
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SoukoType } },
                { "@UnitPriceCalcKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.UnitPriceCalcKBN } },
                { "@MakerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.MakerCD } },
                { "@IdouCount", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.IdouCount } },
                { "@ZipCD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ZipCD1 } },
                { "@ZipCD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ZipCD2 } },
                { "@Address1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Address1 } },
                { "@Address2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Address2 } },
                { "@TelephoneNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.TelephoneNO } },
                { "@FaxNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.FaxNO } },
                { "@Remarks", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Remarks } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Operator } },
                { "@LocationXml", new ValuePair { value1 = SqlDbType.Xml, value2 = mse.LocationXml } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Key } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }
            };

            return InsertUpdateDeleteData(dic, "M_Souko_Insert_Update");
        }

        public bool M_Souko_Delete(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Key } }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Souko_Delete");
        }

        public DataTable M_SoukoWarehouse_Select(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg });

            return SelectData(dic, "M_SoukoWarehouse_Select");
        }

        public DataTable M_SoukoName_Select(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCD });
            dic.Add("@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate });

            return SelectData(dic, "M_SoukoName_Select");
        }

        public DataTable M_Souko_BindAll(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                //{ "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                //{ "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.DeleteFlg } },
            };
            return SelectData(dic, "M_Souko_BindAll");
        }

        public DataTable M_Souko_AllBind(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SoukoType } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.DeleteFlg } },
            };
            return SelectData(dic, "M_Souko_AllBind");
        }

        public DataTable M_Souko_SelectData(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } }
            };
            return SelectData(dic, "M_Souko_SelectData");
        }

        /// <summary>
        /// Select Souko's info
        /// 指定したStoreCDの倉庫情報をSelect
        /// </summary>
        /// <param name="mse">Souko info</param>
        /// <returns></returns>
        public DataTable M_Souko_BindForTanaoroshi(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } }
            };
            return SelectData(dic, "M_Souko_BindForTanaoroshi");
        }

    }
}
