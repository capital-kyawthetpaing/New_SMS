﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using System.Deployment.Application;
using CKM_Controls;
using System.Diagnostics;

namespace MainMenu
{ 
    public partial class TennicLogin : Form
    {
        Login_BL loginbl;
        M_Staff_Entity mse;
        public TennicLogin(bool IsMainCall =false)
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
            Control.CheckForIllegalCrossThreadCalls = false;
            UpdatedFileList = null;
        }
        private bool  CheckExistFormRunning()
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
            mse = new M_Staff_Entity()
            {
                StaffCD=txtOperatorCD.Text,
                Password=txtPassword.Text
            };
            return mse;
        } 
        private void Tennic_MainMenu_Load(object sender, EventArgs e)
        {
            Iconic ic = new Iconic();
            if (ic.IsExistSettingIn(out string path))
            {
                try
                {
                    pictureBox1.Image = new Bitmap(path);
                }
                catch
                {
                }
            }
            loginbl = new Login_BL();

            Add_ButtonDesign();
            txtOperatorCD.Select();
            this.ActiveControl = txtOperatorCD;
            txtOperatorCD.Focus();
            Control.CheckForIllegalCrossThreadCalls = false;
        } 
        private void Tennic_MainMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.Shift && e.KeyCode == Keys.C)
            {
                var files = FTPData.GetFileList(Login_BL.SyncPath, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");
                if (files.Count() == 0)
                {
                    MessageBox.Show("There is no available file on server!");
                    return;
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("No", typeof(string));
                dt.Columns.Add("Check", typeof(bool));
                dt.Columns.Add("colFileName", typeof(string));
                dt.Columns.Add("colFileExe", typeof(string));
                dt.Columns.Add("colDate", typeof(string));
                int k = 0;
                foreach (var dr in files)
                {
                    k++;

                    dt.Rows.Add(new object[] { k.ToString(), 1, dr.ToString().Split('.').FirstOrDefault(), dr.ToString(), "00:00:00" });
                }
                FrmList frm = new FrmList(dt);
                frm.ShowDialog();
                UpdatedFileList = frm.dt;

            }
            if (e.Control && e.Alt && e.Shift && e.KeyCode == Keys.P)
            {
                Prerequisity pre = new Prerequisity();
                pre.Show();
            }
        }
        protected DataTable UpdatedFileList { get; set; }
        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            Login_Click();
        }
        private void Login_Click()
        {
            if (loginbl.ReadConfig() == false)
            {
                this.Close();
                System.Environment.Exit(0);
            }
            if(ErrorCheck())
            {
                if (loginbl.ReadConfig() == false)
                {
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
                            //loginbl.ShowMessage("S013");
                            //txtOperatorCD.Select();
                            //return;
                        
                        var mseinfo = loginbl.M_Staff_InitSelect(GetInfo());
                            Tennic_MainMenu menuForm = new Tennic_MainMenu(GetInfo().StaffCD, mseinfo);
                            this.Hide();   
                            menuForm.ShowDialog();
                            //this.Close();

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

        private void ckM_Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ckM_Button3_Click(object sender, EventArgs e)
        {
            F11();
        }
        private void F11()
        {
            var result = MessageBox.Show("サーバーから最新プログラムをダウンロードしますか？", "Synchronous Update Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (result == DialogResult.Yes)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                return;
            }
            this.Cursor = Cursors.Default;

            ckM_Button1.Focus();
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
        private void TennicLogin_Paint(object sender, PaintEventArgs e)
        {
                txtOperatorCD.BorderStyle = BorderStyle.None;
                Pen p = new Pen(System.Drawing.ColorTranslator.FromHtml("#05af34"));
                Graphics g = e.Graphics;
                int variance = 2;
                g.DrawRectangle(p, new Rectangle(txtOperatorCD.Location.X - variance, txtOperatorCD.Location.Y - variance, txtOperatorCD.Width + variance, txtOperatorCD.Height + variance));
                txtPassword.BorderStyle = BorderStyle.None;
                g.DrawRectangle(p, new Rectangle(txtPassword.Location.X - variance, txtPassword.Location.Y - variance, txtPassword.Width + variance, txtPassword.Height + variance));

            
        }
        protected string Maxcou = "";
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var files = FTPData.GetFileList(Login_BL.SyncPath, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");
            if (files.Count() == 0)
            {
                return;
            }
            progressBar1.Visible = true;
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            int max = files.Count();
            List<string> strList = new List<string>();
            if (UpdatedFileList != null)
            {
                max -= Convert.ToInt32(UpdatedFileList.Select("Check <> True").Count());

                foreach (DataRow dr in UpdatedFileList.Select("Check <> False").CopyToDataTable().Rows)
                {
                    if (files.Contains(dr["colFileExe"].ToString()))
                    {
                        strList.Add(dr["colFileExe"].ToString());
                    }
                }
                files = null;
                files = strList.ToArray();
            }
            Maxcou = max.ToString();
            int c = 0;
            lblProgress.Text = "0 of " + max.ToString() + " Completed!";//
            foreach (string file in files)
            {
                c++;
                double cent = (c * 100) / max;
                if (!backgroundWorker1.CancellationPending)
                {
                    backgroundWorker1.ReportProgress((int)cent);
                }
                lblProgress.Text = c.ToString() + " of " + max.ToString() + " Completed!";
                lblProgress.Update();
                FTPData ftp = new FTPData(Login_BL.SyncPath, "TennicLogin");
                ftp.Download("", file, Login_BL.SyncPath, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");
            }
            progressBar1.Enabled = progressBar1.Visible = false;
            progressBar1.Text = "";
            lblProgress.Text = "";
            lblProgress.Update();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblcent.Text = "";
            lblcent.Update();
            MessageBox.Show("ダウンロードが終わりました", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblcent.Text = $"{e.ProgressPercentage} %";
            lblcent.Update();
            progressBar1.Value = e.ProgressPercentage;
            progressBar1.Update();
        }

        private void TennicLogin_KeyUp(object sender, KeyEventArgs e)
        {
           
                if (ActiveControl.Name == "txtPassword" && e.KeyCode == Keys.Enter)
                {
                    ckM_Button1.Select();
                    ckM_Button1.Focus();
                    return;
                }
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
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

    }
}
