using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using DL;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           
            Application.Run(LoginFormName());
        }

       static Form LoginFormName()
        {
            Form pgname =null;
            if (lbl.ReadConfig())
            {
                if (Base_DL.iniEntity.Login_Type == "CapitalMainMenuLogin")       // Capital MainMenu
                {
                    pgname = new MainmenuLogin();
                }
                else if (Base_DL.iniEntity.Login_Type == "HaspoMainMenuLogin")   // Haspo MainMenu
                {
                    pgname = new HaspoLogin();
                }
                else if (Base_DL.iniEntity.Login_Type == "CapitalStoreMenuLogin")   // Capital StoreMenu
                {
                    pgname = new CapitalsportsLogin();
                }
                else if (Base_DL.iniEntity.Login_Type == "HaspoStoreMenuLogin")   // Haspo StoreMenu
                {
                 

                }
                else if (Base_DL.iniEntity.Login_Type == "TennicMainMenuLogin")   // Tennic MainMenu
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
