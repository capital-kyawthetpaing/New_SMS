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

namespace MainMenu
{
   
    public partial class TennicLogin : Form
    {
        Login_BL loginbl;
        M_Staff_Entity mse;
        public TennicLogin()
        {
            this.KeyPreview = true;
            InitializeComponent();
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
            loginbl = new Login_BL();
        }

        private void Tennic_MainMenu_KeyDown(object sender, KeyEventArgs e)
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
                            var mseinfo = loginbl.M_Staff_InitSelect(GetInfo());
                            Tennic_MainMenu menuForm = new Tennic_MainMenu(GetInfo().StaffCD, mseinfo);
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
    }
}
