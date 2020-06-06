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
        M_Customer_Entity cust ;
        TempoRegiKaiinTouroku_BL tprg_Kaiin_Bl = new TempoRegiKaiinTouroku_BL();        
        public TempoRegiKaiinTouroku()
        {
            cust = new M_Customer_Entity();
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
            if(txtCustomerNo.Text.Length < 13)
            {
                bbl.ShowMessage("E238");
                txtCustomerNo.Focus();
                return false;
            }
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
                cust = new M_Customer_Entity();
                cust.CustomerCD = txtCustomerNo.Text;
                cust= tprg_Kaiin_Bl.M_Customer_Select(cust);
                if (cust !=null)
                {
                    string storeKBN = cust.StoreKBN;
                    string deleteFlg = cust.DeleteFlg;
                    if(deleteFlg == "1")
                    {
                        bbl.ShowMessage("E119");
                        txtCustomerNo.Focus();
                    }
                    else if(storeKBN == "1")
                    {
                        bbl.ShowMessage("E235");
                        txtCustomerNo.Focus();
                    }
                    else
                    {
                        Frm_TempoRegiKaiinTouroku_CustomerDetail tprg_CustDetail = new Frm_TempoRegiKaiinTouroku_CustomerDetail(1, txtCustomerNo.Text);
                        tprg_CustDetail.ShowDialog();
                    }
                }
                else
                {
                    Frm_TempoRegiKaiinTouroku_CustomerDetail tprg_CustDetail = new Frm_TempoRegiKaiinTouroku_CustomerDetail(0, txtCustomerNo.Text);
                   
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
