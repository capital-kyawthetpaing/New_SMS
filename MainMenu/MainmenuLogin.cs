using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using CKM_Controls;
using DL;
using Entity;
using Tulpep.NotificationWindow;
using System.IO;
using static CKM_Controls.CKM_Button;
using System.Diagnostics;

namespace MainMenu
{
    public partial class MainmenuLogin : Form
    {
        Login_BL loginbl;
        M_Staff_Entity mse;
        FTPData ftp = new FTPData();
        public MainmenuLogin(bool IsMainCall=false)
        {
            if (!IsMainCall)
            {
                if (CheckExistFormRunning())
                {
                    System.Environment.Exit(0);
                }
            }
            this.KeyPreview = true;
            InitializeComponent();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                label2.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);
            }
            else
                ckM_Button3.Visible = false;

            Login_BL.Ver = label2.Text;
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
        private bool ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(txtOperatorCD.Text))
            {

                loginbl.ShowMessage("E101");
                txtOperatorCD.Focus();
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
        private void MainmenuLogin_Load(object sender, EventArgs e)
        {
            loginbl = new Login_BL();
            txtOperatorCD.Focus();
            Add_ButtonDesign();
           
            //if (Login_BL.Islocalized)
            //{
            //    //var DefaultFlg = loginbl.CheckDefault("1"); // MenuFlg
            //    //if (DefaultFlg.Rows.Count > 0)
            //    //{
            //    //    if (DefaultFlg.Rows[0]["DefaultKBN"].ToString() == "0")
            //    //        Setting(DefaultFlg);
            //    //}
            //}


        }
        private void ChangeFont_(DataTable df)
        {
            var val = Convert.ToInt32(df.Rows[0]["FSKBN"].ToString());
            if (val == 1)
            {
                ChangeFont((CKM_FontSize.Normal));
            }
            else if (val == 2)
            {
                ChangeFont((CKM_FontSize.XSmall));
            }
            else if (val == 3)
            {
                ChangeFont((CKM_FontSize.Small));
            }
            else if (val == 4)
            {
                ChangeFont((CKM_FontSize.Medium));
            }
            else if (val == 5)
            {
                ChangeFont((CKM_FontSize.Large));
            }
            else if (val == 6)
            {
                ChangeFont((CKM_FontSize.XLarge));
            }
    
        }
        private void ChangeFont(CKM_Button.CKM_FontSize fs)
        {
            ckM_Button2.Font_Size = ckM_Button3.Font_Size = ckM_Button1.Font_Size = fs;
        }
        private void Setting(DataTable df)
        {
            //LoginLogo  
            //  var L_pth = Login_BL.FtpPath.Replace("Sync", "Setting")+ df.Rows[0]["L_LogoName"].ToString();    /// FTP Logic
            // var Bitmap = FTPData.GetImage(L_pth);
            //if (Bitmap == null)
            //{
            //    MessageBox.Show("Server error");
            //    return;
            //}
            //var I_Pth= Login_BL.FtpPath.Replace("Sync", "Setting") + df.Rows[0]["IconName"].ToString();        /// FTP Logic
            //  var FStream = FTPData.GetImageStream(I_Pth);
            pictureBox1.Image =Getbm((df.Rows[0]["L_LogoCD"] as byte[]));
            try
            {
                this.Icon = new System.Drawing.Icon(new MemoryStream(df.Rows[0]["IconCD"] as byte[]), 32, 32);
            }
            catch
            { }
            ChangeFont_(df);
        }
        private Bitmap Getbm(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }
        private void MainmenuLogin_KeyDown(object sender, KeyEventArgs e)
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

        private void ckM_Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            Login_Click();
            //Base_BL bbl = new Base_BL();
            //bbl.ShowMessage("I001", "テスト", "テスト1");
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
                    ftp.UpdateSyncData(Login_BL.SyncPath);
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Now AppData Files are updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // .. 
                }
                ckM_Button1.Focus();

            }
        }
        private void Login_Click()
        {
            if (loginbl.ReadConfig() == false)
            {
                //起動時エラー    DB接続不可能
                this.Close();
                System.Environment.Exit(0);
            }
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

                var mse = loginbl.MH_Staff_LoginSelect(GetInfo());
                if (mse.Rows.Count > 0)
                {
                    if (mse.Rows[0]["MessageID"].ToString() == "Allow")
                    {
                        if (loginbl.Check_RegisteredMenu(GetInfo()).Rows.Count > 0)
                        {
                            var mseinfo = loginbl.M_Staff_InitSelect(GetInfo());
                            Main_Menu menuForm = new Main_Menu(GetInfo().StaffCD, mseinfo);
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
        }

        private void ckM_Button3_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void MainmenuLogin_Paint(object sender, PaintEventArgs e)
        {
            txtOperatorCD.BorderStyle = BorderStyle.None;
            Pen p = new Pen(System.Drawing.ColorTranslator.FromHtml("#05af34"));
            Graphics g = e.Graphics;
            int variance = 2;
            g.DrawRectangle(p, new Rectangle(txtOperatorCD.Location.X - variance, txtOperatorCD.Location.Y - variance, txtOperatorCD.Width + variance, txtOperatorCD.Height + variance));
            txtPassword.BorderStyle = BorderStyle.None;
            g.DrawRectangle(p, new Rectangle(txtPassword.Location.X - variance, txtPassword.Location.Y - variance, txtPassword.Width + variance, txtPassword.Height + variance));

        }
        protected void ButtonState()
        {
            var c = GetAllControls(this);
            for (int i = 0; i < c.Count(); i++)
            {
                Control ctrl = c.ElementAt(i) as Control;
                if (ctrl is CKM_Button)
                {

                }
            }
        }
        public IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
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

        private void txtOperatorCD_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
