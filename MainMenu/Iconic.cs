using DL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
   public class Iconic
    {
        public Iconic()
        {

        }
        public bool IsExistSettingIn(out String path)
        {
            try
            {
                var IsDeployed = false;
                var desti = @"C:\SMS\AppData";
                System.Uri ui = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                var localpath = path = System.IO.Path.GetDirectoryName(ui.LocalPath);
                if (desti.Replace("\\", "") == localpath.Replace("\\", "")) //Ondeployment
                {
                    IsDeployed = true;
                }
                path = localpath + @"\" + "Portfolio";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(localpath + @"\" + "CKM.ini"))
                {
                    return false;
                }
                IniFile_DL idl = new IniFile_DL(localpath + @"\" + "CKM.ini");
                var Exten = "";
                if ((idl.IniReadValue("Database", "Login_Type") == "CapitalStoreMenuLogin" ) || (idl.IniReadValue("Database", "Login_Type") == "CapitalMainMenuLogin"))
                {
                    Exten = "C_Logo";
                }
                else if (idl.IniReadValue("Database", "Login_Type") == "HaspoStoreMenuLogin" || idl.IniReadValue("Database", "Login_Type") == "HaspoMainMenuLogin")
                {
                    Exten = "H_Logo";
                }
                else
                {
                    Exten = "T_Logo";
                }
                var FullPathLogo = "";
                if (File.Exists(path + @"\" + Exten + ".png"))
                {
                    FullPathLogo = path + @"\" + Exten + ".png";
                }
                else if (File.Exists(path + @"\" + Exten + ".jpeg"))
                {
                    FullPathLogo = path + @"\" + Exten + ".jpeg";
                }
                else if (File.Exists(path + @"\" + Exten + ".jpg"))
                {
                    FullPathLogo = path + @"\" + Exten + ".jpg";
                }
                path = FullPathLogo;
                return true;
            }
            catch
            {
                path = null;
                return false;
            }
        }
    }
}
