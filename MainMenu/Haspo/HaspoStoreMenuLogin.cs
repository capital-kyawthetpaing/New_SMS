using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using CKM_Controls;
using Entity;
using Microsoft.Win32;

namespace MainMenu.Haspo
{
    public partial class HaspoStoreMenuLogin : Form
    {
        Login_BL loginbl;
        M_Staff_Entity mse;

        public HaspoStoreMenuLogin(bool IsMainCall=false)
        {
           // base.Icon = Icon = Properties.Resources.Haspo1;
            SetAddRemoveProgramsIcon();
            ShowIcon = true;
            ShowInTaskbar = true;
          //  ShowInTaskbar = true;
            //var exe = Application.ExecutablePath;
            //System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //var f= u.LocalPath.Replace("bin", "*").Split('*').First() + "MainMenu\\Resources\\Haspo.ico";
            //using (var stream = File.OpenRead(f))
            //{
            //    MessageBox.Show(f);
            //    this.Icon = base.Icon = new Icon(stream);
            //}
            if (!IsMainCall)
            {
                if (CheckExistFormRunning())
                {
                    System.Environment.Exit(0);
                }
            }
            loginbl = new Login_BL();
            InitializeComponent();
            this.KeyPreview = true;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                label2.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);

            }
            else
                ckM_Button3.Visible = false;

        }

        private static void SetAddRemoveProgramsIcon()
        {
            //only run if deployed
            //if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            //     && ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                try
                {
                    Assembly code = Assembly.GetExecutingAssembly();
                    AssemblyDescriptionAttribute asdescription =
                        (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(code, typeof(AssemblyDescriptionAttribute));
                    // string assemblyDescription = asdescription.Description;

                    //the icon is included in this program
                    string iconSourcePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Haspo.ico");

                    if (!File.Exists(iconSourcePath))
                        return;

                    RegistryKey myUninstallKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                    string[] mySubKeyNames = myUninstallKey.GetSubKeyNames();
                    for (int i = 0; i < mySubKeyNames.Length; i++)
                    {
                        RegistryKey myKey = myUninstallKey.OpenSubKey(mySubKeyNames[i], true);
                        object myValue = myKey.GetValue("DisplayName");
                        if (myValue != null && myValue.ToString() == "MainMenu")
                        {
                            myKey.SetValue("DisplayIcon", iconSourcePath);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private bool CheckExistFormRunning()
        {
            Process[] localByName = Process.GetProcessesByName("MainMenu");
            if (localByName.Count() > 1)
            {
                MessageBox.Show("PLease close the running application before running the new instance one.");
                return true;
            }
            return false;
        }
        private void HaspoStoreMenuLogin_Load(object sender, EventArgs e)
        {
            loginbl = new Login_BL();
            txtOperatorCD.Focus();
            Add_ButtonDesign();
            txtOperatorCD.Focus();
            txtOperatorCD.Select();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private M_Staff_Entity GetInfo()
        {
            mse = new M_Staff_Entity
            {
                StaffCD = txtOperatorCD.Text,
                Password = txtPassword.Text
            };
            return mse;
        }
       
        private void Login_Click()
        {
            if (loginbl.ReadConfig() == false)
            {
                //起動時エラー    DB接続不可能
                this.Close();
                System.Environment.Exit(0);
            }
            
            if (!String.IsNullOrWhiteSpace(txtOperatorCD.Text))
            {
                if (loginbl.ReadConfig() == false)
                {
                    //起動時エラー    DB接続不可能
                    this.Close();
                    System.Environment.Exit(0);
                }
                var mse = loginbl.MH_Staff_LoginSelect(GetInfo());
                if (mse.Rows.Count > 0)
                {
                    if (mse.Rows[0]["MessageID"].ToString() == "Allow")
                    {
                        if (loginbl.Check_RegisteredMenu(GetInfo()).Rows.Count > 0)
                        {
                            var mseinfo = loginbl.M_Staff_InitSelect(GetInfo());
                            HapoStore_MainMenu hapomainmenu = new HapoStore_MainMenu(GetInfo().StaffCD,mseinfo);
                            this.Hide();
                            hapomainmenu.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            loginbl.ShowMessage("S018");
                            txtOperatorCD.Select();
                        }
                    }
                    else
                    {
                        loginbl.ShowMessage(mse.Rows[0]["MessageID"].ToString());
                        txtOperatorCD.Select();
                    }
                }
                else
                {
                    loginbl.ShowMessage("E101");
                    txtOperatorCD.Select();
                }
            }
            else
            {
                loginbl.ShowMessage("E101");
                txtOperatorCD.Focus();
            }
            
        }
        private void HaspoStoreMenuLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.SelectNextControl(ActiveControl, true, true, true, true);

            else if (e.KeyData == Keys.F1)
            {
                this.Close();
                System.Environment.Exit(0);
            }
            else if (e.KeyData == Keys.F12)
            {
                Login_Click();
            }
            else if (e.KeyData == Keys.F11)
            {
                F11();
            }
        }
        private void F11()
        {
            var result = MessageBox.Show("Do you want to asynchronize AppData Files?", "Synchronous Update Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                FTPData ftp = new FTPData(Login_BL.SyncPath, "HaspoStoreMenuLogin");
                ftp.UpdateSyncData();
                this.Cursor = Cursors.Default;
            }
            ckM_Button1.Focus();
        }
        private void  Async()
        {
            //progressBar1.Visible = true;
            FTPData ftp = new FTPData(Login_BL.SyncPath, "HaspoStoreMenuLogin");
            ftp.UpdateSyncData();
            //Task task = new Task(ftp.UpdateSync);
            //task.Start();
            //task.Wait();
           
        }
        private void ckM_Button1_Click_1(object sender, EventArgs e)
        {
            Login_Click();
        }

        protected void Add_ButtonDesign()
        {
            ckM_Button2.FlatStyle = FlatStyle.Flat;
            ckM_Button2.FlatAppearance.BorderSize = 0;
            ckM_Button2.FlatAppearance.BorderColor = Color.White;
            ckM_Button1.FlatStyle = FlatStyle.Flat;
            ckM_Button1.FlatAppearance.BorderSize = 0;
            ckM_Button1.FlatAppearance.BorderColor = Color.White;
            ckM_Button3.FlatStyle = FlatStyle.Flat;
            ckM_Button3.FlatAppearance.BorderSize = 0;
            ckM_Button3.FlatAppearance.BorderColor = Color.White;

        }
        private void HaspoStoreMenuLogin_Paint(object sender, PaintEventArgs e)
        {
            txtOperatorCD.BorderStyle = BorderStyle.None;
            Pen p = new Pen(System.Drawing.ColorTranslator.FromHtml("#05af34"));
            Graphics g = e.Graphics;
            int variance = 2;
            g.DrawRectangle(p, new Rectangle(txtOperatorCD.Location.X - variance, txtOperatorCD.Location.Y - variance, txtOperatorCD.Width + variance, txtOperatorCD.Height + variance));
            txtPassword.BorderStyle = BorderStyle.None;
            g.DrawRectangle(p, new Rectangle(txtPassword.Location.X - variance, txtPassword.Location.Y - variance, txtPassword.Width + variance, txtPassword.Height + variance));


        }
        private void ckM_Button2_MouseEnter(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bmback_3;
            (sender as CKM_Button).ForeColor = Color.Black;
        }

        private void ckM_Button2_MouseLeave(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bm_3;
            (sender as CKM_Button).ForeColor = Color.White;
        }

        private void ckM_Button3_Click_1(object sender, EventArgs e)
        {
            F11();
        }

        private void ckM_Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }
    }
}
