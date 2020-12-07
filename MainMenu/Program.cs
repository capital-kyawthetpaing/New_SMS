using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using DL;
using System.Deployment;
using System.Runtime.InteropServices;
using System.Text;

namespace MainMenu
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
       static Login_BL lbl = new Login_BL();
        [STAThread]
        static void Main()
        {


         // var f=  IsInteger("1234567890123");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           
            Application.Run(LoginFormName());
        }
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        public static bool IsInteger(string value)
        {
            value = value.Replace("-", "");
            if (Int64.TryParse(value, out Int64 Num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static Form LoginFormName()
        {

            Form pgname = null;
            var localpath = @"C:\SMS\AppData\CKM.ini";
            if (!System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                localpath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + "CKM.ini";
            }
           
            var Id = "";
            var pass = "";
            var path = "";
            Login_BL lbl = new Login_BL();
            IniFile_DL idl = new IniFile_DL(localpath);
            if (lbl.ReadConfig())
            {
                byte[] buffer = new byte[2048];

                GetPrivateProfileSection("ServerAuthen", buffer, 2048, localpath);
                String[] tmp = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0');
                Id = tmp[1].Replace("\"", "").Split('=').Last();
                pass =  tmp[2].Replace("\"", "").Split('=').Last();
                path=  tmp[3].Replace("\"", "").Split('=').Last();
                Login_BL.SyncPath = path;
                Login_BL.ID = Id;
                Login_BL.Password = pass;
                Login_BL.FtpPath = path;
                //FTPData ftp1 = new FTPData();
                //ftp1.UpdateSyncData(path);
            }
          //  else
                //return;
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                FTPData ftp = new FTPData();
                if (!System.IO.File.Exists(localpath))
                {
                    FTPData.Download("CKM.ini", path,Id, pass, @"C:\SMS\AppData\");
                }
                Login_BL.Islocalized = false;
            }
            else
            {
                Login_BL.Islocalized = true;
            }
            if (lbl.ReadConfig())
            {
                if (Base_DL.iniEntity.Login_Type == "CapitalMainMenuLogin")
                {
                    pgname = new MainmenuLogin();
                }
                else if (Base_DL.iniEntity.Login_Type == "HaspoMainMenuLogin")
                {
                    pgname = new HaspoLogin();
                }
                else if (Base_DL.iniEntity.Login_Type == "CapitalStoreMenuLogin")
                {
                 //   var f = Base_DL.iniEntity.IsDM_D30Used;
                    
                         
                    pgname = new CapitalsportsLogin();
                }
                else if (Base_DL.iniEntity.Login_Type == "HaspoStoreMenuLogin")
                {
                    pgname = new Haspo.HaspoStoreMenuLogin();
                }
                else if (Base_DL.iniEntity.Login_Type == "TennicMainMenuLogin")
                {
                    pgname = new TennicLogin();
                }
                else
                {
                    MessageBox.Show("The program cannot initialize with the specified server instaces  inside CKM.ini file. PLease fix ini file!!!!");
                    Environment.Exit(0);
                }
            }
            return pgname;  
        }
    }
}
