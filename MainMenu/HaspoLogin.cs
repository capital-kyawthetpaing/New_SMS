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
    public partial class HaspoLogin : Form
    {

         








        Login_BL loginbl;
        M_Staff_Entity mse;
        public HaspoLogin()
        {
            InitializeComponent();
        }

        private void HaspoLogin_Load(object sender, EventArgs e)
        {
            loginbl = new Login_BL();
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
        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            Login_Click();
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
                            Haspo_MainMenu menuForm = new Haspo_MainMenu(GetInfo().StaffCD, GetInfo());
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
        private void HaspoLogin_KeyDown(object sender, KeyEventArgs e)
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

        private void ckM_Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }
    }
}
