using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;
using Microsoft.VisualBasic;
using TempoRegiKaiinTouroku_CustomerDetail;
using Search;


namespace TempoRegiKaiinTouroku
{
    public partial class TempoRegiKaiinTouroku : ShopBaseForm
    {
        //M_Customer_Entity cust　= new M_Customer_Entity();
      TempoRegiKaiinTouroku_BL tprg_Kaiin_Bl = new TempoRegiKaiinTouroku_BL();        
        public TempoRegiKaiinTouroku()
        {
            InitializeComponent();
        }
        private void TempoRegiKaiinTouroku_Load(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;           
            StartProgram();
            this.Text = "店舗レジ 会員登録";
            txtCustomerNo.Require(true);
            txtCustomerNo.Focus();
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private bool ErrorCheck()
        {
            if(!RequireCheck(new Control[] { txtCustomerNo}))
            {
                return false;
            }

            if (!CheckWidth(1))
                return false;

            if (!CheckWidth(2))
                return false;


            return true;
        }
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                    Save();
                    break;
            }
        }
        private void Save()
        {
            if(ErrorCheck())
            {
                if(tprg_Kaiin_Bl.IsExists(txtCustomerNo.Text))
                {
                    Frm_TempoRegiKaiinTouroku_CustomerDetail tprg_CustDetail = new Frm_TempoRegiKaiinTouroku_CustomerDetail(1, txtCustomerNo.Text);  
                    tprg_CustDetail.ShowDialog();
                }
                else
                {
                    Frm_TempoRegiKaiinTouroku_CustomerDetail tprg_CustDetail = new Frm_TempoRegiKaiinTouroku_CustomerDetail(0, txtCustomerNo.Text);
                    //TempoRegiKaiinTouroku_ShopForm.Frm_TempoRegiKaiinTouroku_ShopForm tprg_Kaiin = new TempoRegiKaiinTouroku_ShopForm.Frm_TempoRegiKaiinTouroku_ShopForm(1,txtCustomerNo.Text);
                    //txtCustomerNo.Focus();
                    tprg_CustDetail.ShowDialog();
                }

            }

        }       

        private void Customer_KeyDown(object sender, KeyEventArgs e)
        {           
            if(Keys.Enter==e.KeyCode)
            { 
                Save();
            }
        }
        private  bool CheckWidth(int type)
        {
            switch(type)
            {
                case 1:
                    string str = Encoding.GetEncoding(932).GetByteCount(txtCustomerNo.Text).ToString();
                    if (Convert.ToInt32(str) > 13)
                    {
                        MessageBox.Show("Bytes Count is Over!!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Focus();
                        return false;
                    }
                    break;

                case 2:
                    //int byteCount = Encoding.UTF8.GetByteCount(txtCustomerNo.Text);//FullWidth_Case
                    int byteCount = Encoding.GetEncoding("Shift_JIS").GetByteCount(txtCustomerNo.Text);
                    int onebyteCount= System.Text.ASCIIEncoding.ASCII.GetByteCount(txtCustomerNo.Text);//HalfWidth_Case
                    if (onebyteCount!=byteCount)
                    {
                        MessageBox.Show("Bytes Count is Over!!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Focus();
                        return false;
                    }                  
                  
                    break;
            }           
            return true;
        }

        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            TempoRegiKaiinKensaku tgkkk = new TempoRegiKaiinKensaku();
            tgkkk.ShowDialog();

            if(!string.IsNullOrEmpty(tgkkk.CustomerCD))
            {
                txtCustomerNo.Text = tgkkk.CustomerCD;
            }
        }
    }
}
