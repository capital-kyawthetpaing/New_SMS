using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using DL;
using System.Deployment;
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
            Form pgname =null;
            const string path= "ftp://202.223.48.145/Sync/";
            Login_BL.SyncPath = path;
            const string localpath = @"C:\SMS\AppData\CKM.ini";
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                FTPData ftp = new FTPData();
                if (!System.IO.File.Exists(localpath))
                {
                    FTPData.Download("CKM.ini", path, "Administrator", "c@p!+A1062O", @"C:\SMS\AppData\");
                }
                //else
                //{
                //    ftp.UpdateSyncData("ftp://202.223.48.145/NewlyModified/");
                //    // Cursor = Cursors.WaitCursor;
                //    // this.Cursor = Cursors.Default;
                //}

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
