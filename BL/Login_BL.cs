using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;
using System.Deployment;
using System.Diagnostics;
using EPSON_TM30;
using System.Data.SqlClient;
using System.IO;

namespace BL
{
    public class Login_BL : Base_BL
    {
        /// <summary>
        /// INIファイル名
        /// </summary>
        /// 
        private const string IniFileName = "CKM.ini";
        //  public static bool Islocalized =false;
        M_Staff_DL msdl;
        M_Store_DL mstoredl;
        Menu_DL mdl;
        public const bool isd = false;
        public static bool Islocalized = false;
        public static string Ver = "";
        public static string SyncPath = "";
        public static string FtpPath = "";
        public static string ID = "";
        public static string IP = "";
        public static string Password = "";
        /// <summary>
        /// constructor
        /// </summary>
        public Login_BL()
        {
            msdl = new M_Staff_DL();
            mstoredl = new M_Store_DL();
            mdl = new Menu_DL();
        }
        
        public DataTable CheckDefault(string mse,string SCD)
        {
            return mdl.CheckDefault(mse,SCD);
        }
        public bool GetState
        {
            get { return Debugger.IsAttached; }
        }
        // MH_Staff_LoginSelect
        public string StorePrinterName
        {
            get { return Base_DL.iniEntity.StorePrinterName; }
        }
        //Check_RegisteredMenu
        public DataTable Check_RegisteredMenu(M_Staff_Entity mse)
        {
            return msdl.Check_RegisteredMenu(mse);
        }
        public DataTable MH_Staff_LoginSelect(M_Staff_Entity mse)
        {
            return msdl.MH_Staff_LoginSelect(mse);
        }
        public M_Staff_Entity M_Staff_LoginSelect(M_Staff_Entity mse)
        {
            DataTable dtStaff = msdl.M_Staff_LoginSelect(mse);
            if (dtStaff.Rows.Count > 0)
            {
                mse.StaffName = dtStaff.Rows[0]["StaffName"].ToString();
                mse.StoreCD = dtStaff.Rows[0]["StoreCD"].ToString();
                mse.StaffKana = dtStaff.Rows[0]["StaffKana"].ToString();
                mse.BMNCD = dtStaff.Rows[0]["BMNCD"].ToString();
                mse.MenuCD = dtStaff.Rows[0]["MenuCD"].ToString();
                mse.KengenCD = dtStaff.Rows[0]["AuthorizationsCD"].ToString();
                mse.StoreAuthorizationsCD = dtStaff.Rows[0]["StoreAuthorizationsCD"].ToString();
                mse.PositionCD = dtStaff.Rows[0]["PositionCD"].ToString();

            }
            return mse;
        }


        /// <summary>
        /// 共通処理　Operator 確認
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public M_Staff_Entity M_Staff_InitSelect(M_Staff_Entity mse)
        {
            DataTable dt = msdl.M_Staff_InitSelect(mse);
            if (dt.Rows.Count > 0)
            {
                mse.StaffName = dt.Rows[0]["StaffName"].ToString();
                mse.SysDate = dt.Rows[0]["sysDate"].ToString();
                mse.StoreCD = dt.Rows[0]["StoreCD"].ToString();
                Base_DL.iniEntity.DatabaseDate = mse.SysDate;
                //mse.StoreName = dt.Rows[0]["StoreName"].ToString(); ;
            }

            return mse;
        }

        //M_Store_InitSelect
        public M_Staff_Entity M_Store_InitSelect(M_Staff_Entity mpe)
        {
           // M_Program_DL dl = new M_Program_DL();
            DataTable dt = msdl.M_Store_InitSelect(mpe);
            if (dt.Rows.Count > 0)
            {
                mpe.StoreName = dt.Rows[0]["StoreName"].ToString();
                //mpe.Type = dt.Rows[0]["Type"].ToString();

           
            }
            return mpe;
        }
        /// <summary>
        /// For Default Souko Bind
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public M_Staff_Entity M_Souko_InitSelect(M_Staff_Entity mse)
        {
            DataTable dt = msdl.M_Souko_InitSelect(mse);
            if (dt.Rows.Count > 0)
            {
                mse.SysDate = dt.Rows[0]["sysDate"].ToString();
                mse.SoukoCD = dt.Rows[0]["SoukoCD"].ToString();
                Base_DL.iniEntity.DatabaseDate = mse.SysDate;
            }

            return mse;
        }

        public M_Store_Entity M_Store_InitSelect(M_Staff_Entity mse, M_Store_Entity mste)
        {
            DataTable dt = mstoredl.M_Store_InitSelect(mse);
            if (dt.Rows.Count > 0)
            {
                mste.StoreName = dt.Rows[0]["StoreName"].ToString();
                mste.SysDate = dt.Rows[0]["sysDate"].ToString();
                mste.StoreCD = dt.Rows[0]["StoreCD"].ToString();
                Base_DL.iniEntity.DatabaseDate = mste.SysDate;
            }

            return mste;
        }

        /// <summary>
        /// 共通処理　プログラム
        /// </summary>
        /// <param name="mpe"></param>
        /// <returns></returns>
        public bool M_Program_InitSelect(M_Program_Entity mpe)
        {
            M_Program_DL dl = new M_Program_DL();
            DataTable dt = dl.M_Program_Select(mpe);
            if (dt.Rows.Count > 0)
            {
                mpe.ProgramName = dt.Rows[0]["ProgramName"].ToString();
                mpe.Type = dt.Rows[0]["Type"].ToString();

                return true;
            }

            else
                return false;
        }
        /// <summary>
        /// INIファイルより情報取得
        /// </summary>
        /// <returns></returns>
        public bool ReadConfig()
        {
            // INIﾌｧｲﾙ取得
            // 実行モジュールと同一フォルダのファイルを取得
            string filePath = "";
            //System.Diagnostics.Debug 



            if (Debugger.IsAttached || Islocalized)
            {
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + IniFileName;
            }
            else
            {
                filePath = @"C:\\SMS\\AppData\\CKM.ini";
            }
            // var f = Islocalized;
            //if(System.Deployment.Internal.)
            if (System.IO.File.Exists(filePath))
            {
                this.GetInformationOfIniFile(filePath);
            }
            else
            {
                //初期設定ファイルが取得できませんでした
                return false;
            }

            return true;

        }
        public void GetInformationOfIniFile(string filePath)
        {
            IniFile_DL idl = new IniFile_DL(filePath);
            if (idl.IniReadValue("Database", "Login_Type") == "MMLocal")   // Modified by PTK (2020/01/24) for Multiple Server Instances
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "MMLocal").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "MMLocal").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "MMLocal").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "MMLocal").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "MMLocal";
                Base_DL.iniEntity.StoreType = "0";
            }
            else if (idl.IniReadValue("Database", "Login_Type") == "CapitalMainMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "CapitalMainMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "CapitalMainMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "CapitalMainMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "CapitalMainMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "CapitalMainMenuLogin";
                Base_DL.iniEntity.StoreType = "0";
            }
            else if (idl.IniReadValue("Database", "Login_Type") == "HaspoMainMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "HaspoMainMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "HaspoMainMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "HaspoMainMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "HaspoMainMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "HaspoMainMenuLogin";
                Base_DL.iniEntity.StoreType = "0";
            }
            else if (idl.IniReadValue("Database", "Login_Type") == "CapitalStoreMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "CapitalStoreMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "CapitalStoreMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "CapitalStoreMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "CapitalStoreMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "CapitalStoreMenuLogin";
                Base_DL.iniEntity.StoreType = "1";

            }
            else if (idl.IniReadValue("Database", "Login_Type") == "HaspoStoreMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "HaspoStoreMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "HaspoStoreMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "HaspoStoreMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "HaspoStoreMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "HaspoStoreMenuLogin";
                Base_DL.iniEntity.StoreType = "1";
            }
            else if (idl.IniReadValue("Database", "Login_Type") == "TennicMainMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "TennicMainMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "TennicMainMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "TennicMainMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "TennicMainMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "TennicMainMenuLogin";
                Base_DL.iniEntity.StoreType = "0";
            }
            //Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "ServerName");
            //Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "DatabaseName");
            //Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "LoginID");
            //Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "Password");
            //Base_DL.iniEntity.Login_Type = idl.IniReadValue("Database", "Login_Type");
            //暗号化されたパスワードを取得

            //暗号化されたパスワードを複合化
            try
            {
                Base_DL.iniEntity.IsDM_D30Used =( idl.IniReadValue("Database", "Logical_Printer").ToString().Trim() == "EpsonTM-m30") &&
                    (idl.IniReadValue("Database", "Login_Type") == "CapitalStoreMenuLogin"  || idl.IniReadValue("Database", "Login_Type") == "HaspoStoreMenuLogin") ? true : false;

            }
            catch
            {
                Base_DL.iniEntity.IsDM_D30Used = false;
            }
            Base_DL.iniEntity.TimeoutValues = idl.IniReadValue("Database", "Timeout");
            // 店舗レジで使用するプリンター名
            Base_DL.iniEntity.StorePrinterName = idl.IniReadValue("Printer", "StorePrinterName");
            if (Base_DL.iniEntity.IsDM_D30Used)
                Base_DL.iniEntity.DefaultMessage = GetMessages();


        }
       
        protected string GetMessages()
        {
            var get = getd();
            var str = "";
            str += get.Rows[0]["Char1"] == null ? "" : get.Rows[0]["Char1"].ToString().Trim() + "     ";
            str += get.Rows[0]["Char2"] == null ? "" : get.Rows[0]["Char2"].ToString().Trim() + "     ";
            str += get.Rows[0]["Char3"] == null ? "" : get.Rows[0]["Char3"].ToString().Trim() + "     ";
            str += get.Rows[0]["Char4"] == null ? "" : get.Rows[0]["Char4"].ToString().Trim() + "     ";
            str += get.Rows[0]["Char5"] == null ? "" : get.Rows[0]["Char5"].ToString().Trim();
            int txt = Encoding.GetEncoding(932).GetByteCount(str);
            if (txt > 200)
            {
                str = str.Substring(0, 200);
            }
            return str;
        }
        protected DataTable getd()
        {
            Base_DL bdl = new Base_DL();
            var dt = new DataTable();
            var con = bdl.GetConnection();
            //SqlConnection conn = con;
            con.Open();
            SqlCommand command = new SqlCommand("Select Char1, Char2, Char3, Char4,Char5 from [M_Multiporpose] where [Key]='1' and Id='326'", con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            con.Close();
            return dt;
        }
        public void Display_Service_Update(bool Opened)
        {
            var val = (Opened) ? "1" : "0";
            Base_DL bdl = new Base_DL();
            using (SqlConnection con = bdl.GetConnection())
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("Update M_Multiporpose set Num1 = '" + val + "' where[Key] = '1' and Id = '326'", con))
                {
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
        }
        public void Display_Service_Enabled(bool Enabled, string val = null)
        {
            var pth = "";
            if (val == null)
                pth = @"C:\\SMS\\AppData\Display_Service.exe";
            else
                pth = @"C:\\SMS\\AppData\\" + val;
            if (Enabled)
            {
                try
                {
                    //   Kill(Path.GetFileNameWithoutExtension(pth));
                }
                catch { }
                try
                {
                    //System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo(pth);
                    //start.FileName = pth;
                    //start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    // Process pc = new Process();

                    Process[] processCollection = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(pth));
                    if (processCollection.Count() == 0)
                    {
                        // Process p = new Process();
                        System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo(pth);
                        start.FileName = pth;
                        start.UseShellExecute = true;
                        start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        start.CreateNoWindow = false;
                        //start.WindowStyle = ProcessWindowStyle.
                        //Process pc = new Process();
                        Process.Start(pth);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                try
                {
                    Kill(Path.GetFileNameWithoutExtension(pth));
                }
                catch { }
            }
        }
        protected void Kill(string pth)
        {
            Process[] processCollection = Process.GetProcessesByName(pth.Replace(".exe", ""));
            foreach (Process p in processCollection)
            {
                p.Kill();
            }

            Process[] processCollections = Process.GetProcessesByName(pth + ".exe");
            foreach (Process p in processCollections)
            {
                p.Kill();
            }
        }
        public string Display_Service_Status()  // 0 not display / 1 Display
        {
            Base_DL bdl = new Base_DL();
            var dt = new DataTable();
            var con = bdl.GetConnection();
            //   
            //SqlConnection conn = con;
            con.Open();
            SqlCommand command = new SqlCommand("Select Num1 from [M_Multiporpose] where [Key]='1' and Id='326'", con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            con.Close();
            return dt.Rows.Count > 0 ? dt.Rows[0]["Num1"].ToString() : "0";
        }

        public string GetInformationOfIniFileByKey(string key)
        {
            // INIﾌｧｲﾙ取得
            // 実行モジュールと同一フォルダのファイルを取得
            string filePath = "";
            //System.Diagnostics.Debug 
            if (Debugger.IsAttached)
            {
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + IniFileName;
            }
            else
            {
                filePath = @"C:\\SMS\\AppData\\CKM.ini";
            }
            IniFile_DL idl = new IniFile_DL(filePath);
            return idl.IniReadValue("FilePath", key);
        }

        /// <summary>
        ///     ''' ホスト名を取得
        ///     ''' </summary>
        ///     ''' <returns>ローカルホスト名</returns>
        ///     ''' <remarks></remarks>
        public static string GetHostName()
        {
            string Ret = "";

            Ret = System.Net.Dns.GetHostName();

            return Ret;
        }
    }
}
