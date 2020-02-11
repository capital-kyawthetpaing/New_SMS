using System;
using System.Windows.Forms;
using BL;
using Entity;
using Menu.MenuForm;

namespace Menu.Login
{
    public partial class frmLogin : Form
    {
        Login_BL loginbl;
        M_Staff_Entity mse;
        /// <summary>
        /// Constructor
        /// Nothing special
        /// </summary>
        public frmLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            loginbl = new Login_BL();
        }

        /// <summary>
        /// Login button click
        /// Check Operator Code & password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            //Base_BL bbl = new Base_BL();
            //bbl.ShowMessage("I001", "テスト", "テスト1");
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

                mse = loginbl.M_Staff_LoginSelect(GetInfo());
                if (mse != null)
                {
                    FrmMenu menuForm = new FrmMenu(mse);
                    this.Hide();
                    menuForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    loginbl.ShowMessage("");
                    txtOperatorCD.Select();
                }
            }
        }

        /// <summary>
        /// get staff's information
        /// </summary>
        /// <returns></returns>
        private M_Staff_Entity GetInfo()
        {
            mse = new M_Staff_Entity
            {
                CompanyCD = txtCompanyCD.Text,
                StaffCD = txtOperatorCD.Text,
                Password = txtPassword.Text
            };
            return mse;
        }

        /// <summary>
        /// errorcheck
        /// </summary>
        /// <returns></returns>
        private bool ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(txtOperatorCD.Text))
            {
                loginbl.ShowMessage("");
                txtOperatorCD.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// on enter key down,select next control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                this.SelectNextControl(ActiveControl, true, true, true, true);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
