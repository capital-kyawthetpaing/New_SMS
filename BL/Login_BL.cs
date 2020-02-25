using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class Login_BL : Base_BL
    {
        /// <summary>
        /// INIファイル名
        /// </summary>
        /// 
        private const string IniFileName = "CKM.ini";

        M_Staff_DL msdl;
        M_Store_DL mstoredl;

        /// <summary>
        /// constructor
        /// </summary>
        public Login_BL()
        {
            msdl = new M_Staff_DL();
            mstoredl = new M_Store_DL();
        }

        // MH_Staff_LoginSelect

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

            }
           
            return mse;
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

        public M_Store_Entity M_Store_InitSelect(M_Staff_Entity mse,M_Store_Entity mste)
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
           // System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //  string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + IniFileName;
            string filePath = @"C:\\SMS\\AppData\\CKM.ini";
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
            }
            else if (idl.IniReadValue("Database", "Login_Type") == "CapitalMainMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "CapitalMainMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "CapitalMainMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "CapitalMainMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "CapitalMainMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "CapitalMainMenuLogin";
            }
            else if (idl.IniReadValue("Database", "Login_Type") == "HaspoMainMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "HaspoMainMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "HaspoMainMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "HaspoMainMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "HaspoMainMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "HaspoMainMenuLogin";
            }
            else if (idl.IniReadValue("Database", "Login_Type") == "CapitalStoreMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "CapitalStoreMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "CapitalStoreMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "CapitalStoreMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "CapitalStoreMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "CapitalStoreMenuLogin";
            }
            else if (idl.IniReadValue("Database", "Login_Type") == "HaspoStoreMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "HaspoStoreMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "HaspoStoreMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "HaspoStoreMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "HaspoStoreMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "HaspoStoreMenuLogin";
            }
            else if (idl.IniReadValue("Database", "Login_Type") == "TennicMainMenuLogin")
            {
                Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "TennicMainMenuLogin").Split(',')[0];
                Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "TennicMainMenuLogin").Split(',')[1];
                Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "TennicMainMenuLogin").Split(',')[2];
                Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "TennicMainMenuLogin").Split(',')[3];
                Base_DL.iniEntity.Login_Type = "TennicMainMenuLogin";
            }
            //Base_DL.iniEntity.DatabaseServer = idl.IniReadValue("Database", "ServerName");
            //Base_DL.iniEntity.DatabaseName = idl.IniReadValue("Database", "DatabaseName");
            //Base_DL.iniEntity.DatabaseLoginID = idl.IniReadValue("Database", "LoginID");
            //Base_DL.iniEntity.DatabasePassword = idl.IniReadValue("Database", "Password");
            //Base_DL.iniEntity.Login_Type = idl.IniReadValue("Database", "Login_Type");
            //暗号化されたパスワードを取得

            //暗号化されたパスワードを複合化

            Base_DL.iniEntity.TimeoutValues = idl.IniReadValue("Database", "Timeout");
        }
        public string GetInformationOfIniFileByKey(string key)
        {
            // INIﾌｧｲﾙ取得
            // 実行モジュールと同一フォルダのファイルを取得
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + IniFileName;

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
