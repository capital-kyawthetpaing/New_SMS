using System;
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


namespace MainMenu.Haspo
{
    public partial class HaspoStoreMenuLogin : Form
    {
        Login_BL loginbl;
        M_Staff_Entity mse;

        public HaspoStoreMenuLogin()
        {
            loginbl = new Login_BL();
            InitializeComponent();
            this.KeyPreview = true;
         
        }
       
        private void HaspoStoreMenuLogin_Load(object sender, EventArgs e)
        {
            loginbl = new Login_BL();
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
        }

        private void ckM_Button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }

        private void ckM_Button1_Click_1(object sender, EventArgs e)
        {
            Login_Click();
        }
    }
}
;