using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_Staff_DL : Base_DL
    {
        /// <summary>
        /// Select Staff's info
        /// </summary>
        /// <param name="mse">staff info</param>
        /// <returns></returns>
        /// 


        public DataTable CheckDefault(string mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MenuCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse } }
            };
            return SelectData(dic, "SettingCheckDefault");
        }
        public DataTable Check_RegisteredMenu(M_Staff_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } }
            };
            return SelectData(dic, "Check_RegisteredMenu");
        }
        public DataTable M_Staff_LoginSelect(M_Staff_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } },
                { "@Password", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Password } }
            };
            return SelectData(dic, "M_Staff_LoginSelect");
        }
        public DataTable MH_Staff_LoginSelect(M_Staff_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } },
                { "@Password", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Password } }
            };
            return SelectData(dic, "M_StaffAccess");
        }
        //MH_Staff_LoginSelect

        /// <summary>	
        /// For Default Souko Bind
        /// </summary>	
        /// <param name="mse"></param>	
        /// <returns></returns>	
        public DataTable M_Souko_InitSelect(M_Staff_Entity mse)
        {
            string sp = "M_Souko_InitSelect";
            //KTP 2019-0529 全部のFunctionでしなくてもいいように共通のFunctionでやり方を更新しました。	
            //command = new SqlCommand(sp, GetConnection());	
            //command.CommandType = CommandType.StoredProcedure;	
            //command.CommandTimeout = 0;	
            //command.Parameters.Add("@StaffCD", SqlDbType.VarChar).Value = mse.StaffCD;	
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } }
            };
            //return SelectData(sp);	
            return SelectData(dic, sp);
        }
        /// <summary>	
        ///  	共通処理　Operator 確認	
        /// </summary>	
        /// <param name="mse"></param>	
        /// <returns></returns>	
        public DataTable M_Staff_InitSelect(M_Staff_Entity mse)
        {
            string sp = "M_Staff_InitSelect";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } }
            };
            return SelectData(dic, sp);
        }

        //M_Store_InitSelect
        public DataTable M_Store_InitSelect(M_Staff_Entity mse)
        {
            string sp = "M_Store_InitSelect";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } }
            };
            return SelectData(dic, sp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable M_Staff_Select(M_Staff_Entity mse)
        {
            string sp = "M_Staff_Select";

            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@StaffCD", SqlDbType.VarChar).Value = mse.StaffCD;
            //command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mse.ChangeDate;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } }
            };

            //return SelectData(sp);
            return SelectData(dic,sp);
        }

        public DataTable M_Staff_SelectAll(M_Staff_Entity mbe)
        {
            string sp = "M_Staff_SelectAll";
            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@DisplayKbn", SqlDbType.TinyInt).Value = mbe.DisplayKbn;
            //command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mbe.ChangeDate;
            //command.Parameters.Add("@StoreCD", SqlDbType.VarChar).Value = mbe.StoreCD;
            //command.Parameters.Add("@StaffCDFrom", SqlDbType.VarChar).Value = mbe.StaffCDFrom;
            //command.Parameters.Add("@StaffCDTo", SqlDbType.VarChar).Value = mbe.StaffCDTo;
            //command.Parameters.Add("@StaffName", SqlDbType.VarChar).Value = mbe.StaffName;
            //command.Parameters.Add("@StaffKana", SqlDbType.VarChar).Value = mbe.StaffKana;

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DisplayKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.DisplayKbn } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbe.ChangeDate } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StoreCD } },
                { "@StaffCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StaffCDFrom } },
                { "@StaffCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StaffCDTo } },
                { "@StaffName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StaffName } },
                { "@StaffKana", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StaffKana } },

            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }

        public DataTable M_Vendor_Staff_Select(M_Staff_Entity mse)
        {
            string sp = "M_Vendor_Staff_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } }
            };

            return SelectData(dic, sp);
        }

        public DataTable M_StaffDisplay(M_Staff_Entity mbe)
        {
            string sp = "M_Staff_Display";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.StaffCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbe.ChangeDate } }
            };

            return SelectData(dic, sp);
        }

        public bool M_Staff_Insert_Update(M_Staff_Entity mse, int mode)
        {
            string sp = "M_Staff_Insert_Update";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@StaffName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffName } },
                { "@StaffKana", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffKana } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@BMNCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.BMNCD } },
                { "@MenuCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.MenuCD } },
                { "@StoreMenuCD",new ValuePair{value1=SqlDbType.VarChar,value2=mse.StoreMenuCD } },
                { "@AuthorizationsCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.KengenCD } },
                { "@StoreAuthorizationsCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreAuthorizationsCD } },
                { "@PositionCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PositionCD } },
                { "@JoinDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.JoinDate } },
                { "@LeaveDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.LeaveDate } },
                { "@Passward", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Password } },
                { "@Remarks", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Remarks } },
                { "@ReceiptPrint", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ReceiptPrint } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.DeleteFlg } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Key } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }
            };

            return InsertUpdateDeleteData(dic, sp);
        }

        public bool M_Staff_Delete(M_Staff_Entity mse)
        {
            string sp = "M_Staff_Delete";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StaffCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.Key } }
            };

            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
