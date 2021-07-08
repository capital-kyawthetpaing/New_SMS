using Dropbox.Api;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DL;
using BL;
using System.IO.Compression;
using System.Security.AccessControl;
using System.Security.Principal;

namespace MainMenu
{
    public partial class Prerequisity : Form
    {
        string APD = "";
        string OPOS = "";
        string MPOS = "";
        string UTILITY = "";
        string CR32 = "";
        string CR64 = "";
        string TOKEN = "";
        string CPOS = "";
        string Soft = @"C:\SMS\Softwares\";
        string lbl_Status = "Already Installed";
        _Select_Source ss = new _Select_Source();
        public Prerequisity()
        {
            InitializeComponent();
        }
        private void Prerequisity_Load(object sender, EventArgs e)
        {
            lbl_PCName.Text =  "("+Environment.MachineName+")";
            CheckForIllegalCrossThreadCalls = false;
            var localpath = @"C:\SMS\AppData\CKM.ini";
            Login_BL lbl = new Login_BL();
            IniFile_DL idl = new IniFile_DL(localpath);
            if (lbl.ReadConfig())
            {
                var getdata = ss._Select_Source_("1");
                if (getdata != null)
                {
                    if (getdata.Rows.Count > 0)
                    {
                        UTILITY = getdata.Rows[0]["PUtility"].ToString();
                        APD = getdata.Rows[0]["APD_M30"].ToString();
                        MPOS = getdata.Rows[0]["MPOS"].ToString();
                        OPOS = getdata.Rows[0]["OPOS"].ToString();
                        TOKEN = getdata.Rows[0]["DropboxAPIToken"].ToString();
                        CR32 = getdata.Rows[0]["CRMSI32"].ToString();
                        CR64 = getdata.Rows[0]["CRMSI64"].ToString();
                        CPOS = getdata.Rows[0]["CPOS"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Please fill data setting in server . . .");
                    return;
                }

                CheckVersion();
            }
            //GetPair("SAP"); 
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirName);
            DirectorySecurity security = sourceDirectory.GetAccessControl();
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
            security.SetAccessRuleProtection(true, true);
            
            sourceDirectory.SetAccessControl(security);
            security.SetOwner(sid);

            DirectoryInfo destinationDirectory = new DirectoryInfo(destDirName);
            DirectorySecurity security1 = destinationDirectory.GetAccessControl(); 
            security1.SetAccessRuleProtection(true, true);
          
            //destinationDirectory.SetAccessControl(security1);
            //security1.SetOwner(sid);
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                FileSecurity securityf = file.GetAccessControl();

                System.Security.Principal.NTAccount ntAccount = new System.Security.Principal.NTAccount("Mygroup"); 

                FileSystemAccessRule rule = new FileSystemAccessRule(ntAccount, FileSystemRights.Read, AccessControlType.Deny);

                securityf.AddAccessRule(rule);

                file.SetAccessControl(securityf);
               
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        protected Tuple<string, bool> GetPair(string Key1, string Key2 = null)
        {
            var li = new List<string>();
            var SysWoW = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";
            //    Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(SysWoW))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    if (subkey_name.Contains("81CC"))
                    {
                    }
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        var f = subkey.GetValue("DisplayName") == null ? "" : subkey.GetValue("DisplayName").ToString();
                        //var productVersion = FileVersionInfo.GetVersionInfo(Pversion.Assembly.Location).ProductVersion;
                        if (f.ToString().Contains(Key1))
                        {
                            //if (Key2 != null)
                            //{
                            //    f.ToString().Contains(Key2);
                            //    li.Add(subkey.ToString());
                            //    break;
                            //}
                            li.Add(subkey.ToString());
                            //break;
                        }
                    }
                }
            }
            // var v64 = (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{822D6DBD-02BA-4FBC-9C44-F146D9AC4DF3}", true)).GetValue("DisplayVersion");

            if (li.Count >0)
            {
                var reg_pth = SysWoW;
                var valVer = (Registry.LocalMachine.OpenSubKey(reg_pth + li[0].Split('\\').Last())).GetValue("DisplayVersion").ToString();
                return Tuple.Create(valVer, true);
            }
            return   Tuple.Create("", false); ;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    //  Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(@"C:\SMS\Softwares\Point Of Service\Point Of Service\Hi", @"C:\ProgramData\Microsoft\Point Of Service\ptk" );
            //  //  DirectoryCopy(@"C:\SMS\Softwares\Point Of Service\Point Of Service\Hi", @"C:\ProgramData\Microsoft\Point Of Service\ptk",true);
            //}
            //catch (Exception ex){
            //    var f = ex.Message;

            //}
                // Fileio .Copy(@"‪C:\SMS\Softwares\Point Of Service\Point Of Service\t.txt", @"C:\ProgramData\Microsoft\Point Of Service");
            //var d=  Task.Run(()=> 
            if (((sender as Control).Name == "button5"))
                Down(UTILITY, sender);
            else if ((sender as Control).Name == "button4")
                Down(APD, sender);
            else if ((sender as Control).Name == "button3")
                Down(MPOS, sender);
            else if ((sender as Control).Name == "button6")
                Down(OPOS, sender);
            else if ((sender as Control).Name == "button1")
                Down(CR32, sender);
            else if ((sender as Control).Name == "button2")
                Down(CR64, sender);
            else if ((sender as Control).Name == "button8")
                Down(CPOS, sender);

            //); 
        }
        protected async void Down(string Keys, object sender)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ChangeStatus(sender, "Processing . . . ");
                
                if (!Directory.Exists(Soft))
                    Directory.CreateDirectory(Soft);
                DropboxClient client2 = new DropboxClient(TOKEN);
                using (var response = await client2.Files.DownloadAsync(Keys))
                {
                    using (var fileStream = File.Create(Soft + Keys.Split('/').Last()))
                    {
                        (await response.GetContentAsStreamAsync()).CopyTo(fileStream);
                    }
                }
                bool st = false;
                bool stz = false;
                while (!st)
                {
                    System.Threading.Thread.Sleep(3000);
                    try
                    {
                        string startPath = Soft + Keys.Split('/').Last().Replace(".zip", "");
                        string zipPath = Soft + Keys.Split('/').Last();
                        string extractPath = Soft;
                        if ((sender as Control).Name == "button4")
                        {
                            if (!stz)
                                ZipFile.ExtractToDirectory(zipPath, startPath);
                            Process.Start(Soft + Keys.Split('/').Last().Replace(".zip", "") + "/APD_512_m30.exe");
                        }
                        else if ((sender as Control).Name == "button5")
                        {
                            Process.Start(Soft + Keys.Split('/').Last());
                        }
                        else if ((sender as Control).Name == "button3")
                        {
                            Process.Start(Soft + Keys.Split('/').Last());
                        }
                        else if ((sender as Control).Name == "button6")
                        {
                            if (!stz)
                                ZipFile.ExtractToDirectory(zipPath, startPath);
                            Process.Start(Soft + Keys.Split('/').Last().Replace(".zip", "") + "/Ver1.14.16/setup.exe");
                        }
                        else if ((sender as Control).Name == "button1" || ((sender as Control).Name == "button2"))
                        {
                            Process.Start(Soft + Keys.Split('/').Last());
                        }
                        else if ((sender as Control).Name == "button8")
                        {
                            if (!stz)
                                ZipFile.ExtractToDirectory(zipPath, startPath);
                           // Process.Start(Soft + Keys.Split('/').Last().Replace(".zip", "") + "/" + Keys.Split('/').Last().Replace(".zip", ""));
                        }
                        st = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("already"))
                        {
                            stz = true;
                        }
                    }
                }
                ChangeStatus(sender, lbl_Status);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Default;
            }
        }
        protected void ChangeStatus(object sender, string lbl_Status)
        {
            if ((sender as Button).Name == "button5")
            {
                lbl_utility.Visible = true;
                lbl_utility.Text = lbl_Status; 
                status_utility.BackgroundImage =   Properties.Resources.Ok_icon;
            }
            else if ((sender as Button).Name == "button4")
            {
                lbl_apd.Visible = true;
                lbl_apd.Text = lbl_Status;
                status_apd.BackgroundImage = Properties.Resources.Ok_icon;
            }
            else if ((sender as Button).Name == "button3")
            {
                lbl_mpos.Visible = true;
                lbl_mpos.Text = lbl_Status;
                status_mpos.BackgroundImage = Properties.Resources.Ok_icon;
            }
            else if ((sender as Button).Name == "button6")
            {
                lbl_opos.Visible = true;
                lbl_opos.Text = lbl_Status;
                status_opos.BackgroundImage = Properties.Resources.Ok_icon;
            }
            else if ((sender as Button).Name == "button1")
            {
                lbl_32.Visible = true;
                lbl_32.Text = lbl_Status;
                status_32.BackgroundImage = Properties.Resources.Ok_icon;
            }
            else if ((sender as Button).Name == "button2")
            {
                lbl_64.Visible = true;
                lbl_64.Text = lbl_Status;
                status_64.BackgroundImage = Properties.Resources.Ok_icon;
            }
            else if ((sender as Button).Name == "button8")
            {
                lbl_config.Text = lbl_Status;
                lbl_config.Visible = true;
                status_config.BackgroundImage = Properties.Resources.Ok_icon;
                label11.Visible=  !ExistCheck(true);
            }

            (sender as Button).Enabled = false;
        }

        private void button13_Click(object sender, EventArgs e)
        { 
            CheckVersion();
        }
        
       
        private  Tuple<string, bool> FindInstalls(string values)
        {

           // RegistryKey regKey =  
            List<string> installs = new List<string>();

            Collect(RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64), installs); 
            Collect(RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64), installs);
            installs = installs.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            installs.Sort(); // The list of ALL installed applications

            if (installs.Contains(values))
            {
                return Tuple.Create("", true);
            }
            else
                return Tuple.Create("", false);

        }

        protected List<string> Collect(RegistryKey regKey, List<string> installs)
        {
            List<string> keyss = new List<string>() { @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall" };
            foreach (string key in keyss)
            {
                using (RegistryKey rk = regKey.OpenSubKey(key))
                {
                    if (rk == null)
                    {
                        continue;
                    }
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName))
                        {
                            try
                            {
                                installs.Add(Convert.ToString(sk.GetValue("DisplayName")));
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                }
            }
            return installs;
            //  RegistryHive.CurrentUser
        }
        protected void CheckVersion()
        {
            try
            {
                var mpos = GetPair("Microsoft POS");
            
              
                if (File.Exists(@"C:\Program Files (x86)\EPSON\TM-m30 Software\TMm30UTL\TMm30UTL.EXE"))
                {
                    button5.Enabled = false;
                    lbl_utility.Visible = true;
                    status_utility.BackgroundImage = Properties.Resources.Ok_icon;
                }
                else
                {
                    button5.Enabled = true;
                    lbl_utility.Visible = false;
                    status_utility.BackgroundImage = Properties.Resources.Close_2_icon;
                }
                if (File.Exists(@"C:\Program Files (x86)\EPSON\EPSON Advanced Printer Driver 5\PrinterReg.exe"))
                {
                    button4.Enabled = false;
                    lbl_apd.Visible = true;
                    status_apd.BackgroundImage = Properties.Resources.Ok_icon;
                }
                else
                {
                    button4.Enabled = true;
                    lbl_apd.Visible = false;
                    status_apd.BackgroundImage = Properties.Resources.Close_2_icon;
                }
                if (mpos.Item2)
                {
                    button3.Enabled = false;
                    lbl_mpos.Visible = true;
                    status_mpos.BackgroundImage = new Bitmap(MainMenu.Properties.Resources.Ok_icon);  
                }
                else
                {
                    button3.Enabled = true;
                    lbl_mpos.Visible = false;
                    status_mpos.BackgroundImage = Properties.Resources.Close_2_icon;
                }
                mpos = GetPair("OPOS ADK");
                if (mpos.Item2)
                {
                    button6.Enabled = false;
                    lbl_opos.Visible = true;
                    status_opos.BackgroundImage = Properties.Resources.Ok_icon;
                }
                else
                {
                    button6.Enabled = true;
                    lbl_opos.Visible = false;
                    status_opos.BackgroundImage = Properties.Resources.Close_2_icon;
                }
                mpos = FindInstalls("SAP Crystal Reports runtime engine for .NET Framework (32-bit)");
                if (mpos.Item2)
                {
                    button1.Enabled = false;
                    lbl_32.Visible = true;
                    status_32.BackgroundImage = Properties.Resources.Ok_icon;
                }
                else
                {
                    button1.Enabled = true;
                    lbl_32.Visible = false;
                    status_32.BackgroundImage = Properties.Resources.Close_2_icon;
                }
                mpos = FindInstalls("SAP Crystal Reports runtime engine for .NET Framework (64-bit)");
                if (mpos.Item2)
                {
                    button2.Enabled = false;
                    lbl_64.Visible = true;
                    status_64.BackgroundImage = Properties.Resources.Ok_icon;
                }
                else
                {
                    button2.Enabled = true;
                    lbl_64.Visible = false;
                    status_64.BackgroundImage = Properties.Resources.Close_2_icon;
                }
              //  mpos = ExistCheck //FindInstalls("SAP Crystal Reports runtime engine for .NET Framework (64-bit)");
                if (ExistCheck())
                {
                    button8.Enabled = false;
                    lbl_config.Visible = true;
                    status_config.BackgroundImage = Properties.Resources.Ok_icon;
                    label11.Visible = !ExistCheck(true); 
                }
                else
                {
                    button8.Enabled = true;
                    lbl_config.Visible = false;
                    status_config.BackgroundImage = Properties.Resources.Close_2_icon;
                    label11.Visible = !ExistCheck(true);
                }
                //Process.Start(Soft + Keys.Split('/').Last().Replace(".zip", "") + "/" + Keys.Split('/').Last().Replace(".zip", ""));
            }
            catch { }

        }

        protected bool ExistCheck(bool IsSystem =false )
        {
            try
            {
                var pth = @"C:\ProgramData\Microsoft\Point Of Service\Configuration\Configuration.xml";
                var pth1 = @"C:\SMS\Softwares\Point Of Service\Point Of Service\Configuration\Configuration.xml";
                if (File.Exists(pth1) )
                {

                    var red = File.ReadAllText(pth1);
                    var iscon = red.Contains("TM-m30(3)") && red.Contains("TM-m30(2)");
                    if (!IsSystem)
                        return iscon;
                    else
                    {
                        red = File.ReadAllText(pth);
                        return red.Contains("TM-m30(3)") && red.Contains("TM-m30(2)"); ;
                    }
                }
                //string Keys = CPOS;
                //if (File.Exists(pth))
                //{
                //    return true;
                //}
            }
            catch {

            }
            return false;
        }
        protected void HideShow(bool Visible)
        {
            lbl_utility.Text = lbl_apd.Text = lbl_mpos.Text = lbl_opos.Text = lbl_32.Text =lbl_64.Text= "";
            status_utility.Visible = status_apd.Visible = status_mpos.Visible = status_opos.Visible = status_32.Visible = status_64.Visible = Visible;

        }

        private void btn_All_Click(object sender, EventArgs e)
        {
            button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = button5.Enabled = button6.Enabled = button8.Enabled= true;
            label11.Visible = false;
        }
    }

}
