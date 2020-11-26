using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using CKM_Controls;
using Entity;
using Tulpep.NotificationWindow;
namespace MainMenu
{
    public partial class CapitalsportsLogin : Form
    {
        Login_BL loginbl;
        M_Staff_Entity mse;
        public CapitalsportsLogin(bool IsMainCall= false)
        {
            if (!IsMainCall)
            {
                if (CheckExistFormRunning())
                {
                    System.Environment.Exit(0);
                }
            }
            loginbl = new Login_BL();
            this.KeyPreview = true;
            InitializeComponent();
      
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var  val = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);
                label2.Text = val;

            }
            else
                ckM_Button3.Visible = false;
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
        private void CapitalsportsLogin_Load(object sender, EventArgs e)
        {
            loginbl = new Login_BL();

            //PopupNotifier pop = new PopupNotifier();
            //pop.TitleText = "New Updates are Available Now!";
            //pop.ContentText = "Press F11 to download new features";
            //pop.Popup();
            Add_ButtonDesign();
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
           //if (keyData == Keys.F1)
           // {
           //     this.Close();
           //     System.Environment.Exit(0);
              
           // }
           //else if (keyData ==Keys.F12)
           // {
           //     Login_Click();
           // }
      
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private bool ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(txtOperatorCD.Text))
            {

                loginbl.ShowMessage("E101");
                txtOperatorCD.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {

                loginbl.ShowMessage("E101");
                txtPassword.Focus();
                return false;
            }
            return true;
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
        private void CapitalsportsLogin_KeyDown(object sender, KeyEventArgs e)
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
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var result = MessageBox.Show("Do you want to asynchronize AppData Files?", "Synchronous Update Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    FTPData ftp = new FTPData();
                    try
                    {
                        ftp.UpdateSyncData(Login_BL.SyncPath);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.StackTrace.ToString());
                        MessageBox.Show(ex.StackTrace.ToString() + ftp.GetError() + Environment.NewLine + Login_BL.SyncPath);
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    MessageBox.Show("Now AppData Files are updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    this.Cursor = Cursors.Default;
                }
            }
            ckM_Button1.Focus();
        }
        private void ckM_Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }
        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            
            //Base_BL bbl = new Base_BL();
            //bbl.ShowMessage("I001", "テスト", "テスト1");
            
        }

        private void Login_Click()
        {
            if (loginbl.ReadConfig() == false)
            {
                //起動時エラー    DB接続不可能
                this.Close();
                System.Environment.Exit(0);
            }
            //  if (!String.IsNullOrWhiteSpace(txtOperatorCD.Text))
            if (ErrorCheck())
            {
                //共通処理　受取パラメータ、接続情報
                //コマンドライン引数より情報取得
                //Iniファイルより情報取得
                if (loginbl.ReadConfig() == false)
                {
                    //起動時エラー    DB接続不可能
                    this.Close();
                    System.Environment.Exit(0);
                }

                try
                {
                    var mse = loginbl.MH_Staff_LoginSelect(GetInfo());
                    if (mse.Rows.Count > 0)
                    {
                        if (mse.Rows[0]["MessageID"].ToString() == "Allow")
                        {
                            if (loginbl.Check_RegisteredMenu(GetInfo()).Rows.Count > 0)
                            {
                                var mseinfo = loginbl.M_Staff_InitSelect(GetInfo());
                                Capitalsports_MainMenu menuForm = new Capitalsports_MainMenu(GetInfo().StaffCD, mseinfo);
                                this.Hide();
                                menuForm.ShowDialog();
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
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        private void ckM_Button3_Click(object sender, EventArgs e)
        {
            
        }

        private void CapitalsportsLogin_Paint(object sender, PaintEventArgs e)
        {
            txtOperatorCD.BorderStyle = BorderStyle.None;
            Pen p = new Pen(System.Drawing.ColorTranslator.FromHtml("#05af34"));
            Graphics g = e.Graphics;
            int variance = 2;
            g.DrawRectangle(p, new Rectangle(txtOperatorCD.Location.X - variance, txtOperatorCD.Location.Y - variance, txtOperatorCD.Width + variance, txtOperatorCD.Height + variance));
            txtPassword.BorderStyle = BorderStyle.None;
            g.DrawRectangle(p, new Rectangle( txtPassword.Location.X - variance, txtPassword.Location.Y - variance, txtPassword.Width + variance, txtPassword.Height + variance));



        }

        private void ckM_Button3_Click_1(object sender, EventArgs e)
        {
            F11();
        }

        private void ckM_Button1_Click_1(object sender, EventArgs e)
        {
            Login_Click();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (ErrorCheck())
                {
                    ckM_Button1.PerformClick();
                }
            }
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
    }
}
