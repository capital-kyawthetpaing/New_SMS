using Base.Client;
using Entity;
using BL;
using System.Data;
using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace Search
{
    public partial class FrmSearch_Customer : FrmSubForm
    {
        #region"公開プロパティ"
        public string parCustomerCD = "";
        public string parChangeDate = "";
        public string parCustomerName = "";
        #endregion

        M_Customer_Entity mce;
        Search_Customer_BL scbl;
        public string CustomerCD = "";
        public string CustName = "";
        public string ChangeDate = "";
        DataTable dtCustomer;
        public FrmSearch_Customer(string ChangeDate,string CustomerKBN,string StoreCD)
        {
            InitializeComponent();
            txtRefDate.Text = ChangeDate;
            lblStoreKBN.Text = CustomerKBN;            
            BindStore(ChangeDate,StoreCD);   
            F9Visible = false;
            F11Visible = false;
            gv_CustomerSearch.Columns["BirthDate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight; // Doesn't want to right align

            scbl = new Search_Customer_BL();
        }

        private void ckM_Button1_Click(object sender, System.EventArgs e)
        {
            F11();
        }
        private void F11()
        {
            if(ErrorCheck())
            {
                mce = GetDataInfo();               
                dtCustomer = new DataTable();
                dtCustomer = scbl.M_Customer_Search(mce);
                if(dtCustomer.Rows.Count>0)
                {
                    gv_CustomerSearch.DataSource = dtCustomer;
                }
                else
                {
                    scbl.ShowMessage("E128");
                    gv_CustomerSearch.DataSource = null;
                    txtRefDate.Focus();
                }                
            }
            else
            {
                gv_CustomerSearch.DataSource = null;
            }
        }
        private bool ErrorCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtCustCDFrom.Text) && !string.IsNullOrWhiteSpace(txtCustCDTo.Text))
            {
                if (string.Compare(txtCustCDFrom.Text, txtCustCDTo.Text) == 1)
                {
                    scbl.ShowMessage("E106");
                    return false;
                }
            }


            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) && string.IsNullOrWhiteSpace(txtKanaName.Text)&& string.IsNullOrWhiteSpace(txtPhno.Text) && string.IsNullOrWhiteSpace(txtBirthDate.Text) &&
                 (!chk_Store.Checked) && (!chk_Web.Checked) && string.IsNullOrWhiteSpace(cbo_Store.Text)&& string.IsNullOrWhiteSpace(txtKeyword.Text)&& string.IsNullOrWhiteSpace(txtCustCDFrom.Text) && string.IsNullOrWhiteSpace(txtCustCDTo.Text))
            {
                scbl.ShowMessage("E111");
                txtCustomerName.Focus();
                return false;
            }

                return true;

            
        }

        private M_Customer_Entity GetDataInfo()
        {
            string[] strlist = txtKeyword.Text.Split(',');           

           M_Customer_Entity mce = new M_Customer_Entity()
            {
                CustomerKBN = lblStoreKBN.Text,
                RefDate = txtRefDate.Text,
                CustomerName = txtCustomerName.Text,
                KanaName = txtKanaName.Text,
                TelephoneNo = txtPhno.Text,
                Birthdate = txtBirthDate.Text,
                StoreKBN = CheckValue(),
                MainStoreCD = cbo_Store.SelectedValue == null || cbo_Store.SelectedValue.Equals("-1")?"" :  cbo_Store.SelectedValue.ToString(),
                Keyword1 = (strlist.Length > 0) ? strlist[0].ToString() : "",
                Keyword2 = (strlist.Length > 1) ? strlist[1].ToString() : "",
                Keyword3 = (strlist.Length > 2) ? strlist[2].ToString() : "",
                CustomerFrom = txtCustCDFrom.Text,
                CustomerTo = txtCustCDTo.Text,
                //KeyWordType = rdoOR.Checked ? "1" : "2"
            };
            return mce;
        }
        public string CheckValue()
        {
            string chk = string.Empty;

            if (chk_Store.Checked && chk_Web.Checked)
            {
                return string.Empty;
            }
            else if (!chk_Store.Checked && !chk_Web.Checked)
            {
                return string.Empty;
            }
            else if (chk_Store.Checked && !chk_Web.Checked)
            {
                chk = "1";
                return chk;
            }
            else 
            {
                chk = "2";
                return chk;
            }
        }
        public override void FunctionProcess(int index)
        {
            if (index + 1 == 12)
            {
                GetData();
            }
        }
        private void GetData()
        {
            if (gv_CustomerSearch.CurrentRow != null && gv_CustomerSearch.CurrentRow.Index >= 0)
            {
                ChangeDate = gv_CustomerSearch.CurrentRow.Cells["colRefDate"].Value.ToString();
                CustomerCD = gv_CustomerSearch.CurrentRow.Cells["CustomerNo"].Value.ToString();
                CustName = gv_CustomerSearch.CurrentRow.Cells["CustomerName"].Value.ToString();
                this.Close();
            }
        }
        private void BindStore(string changedate,string scd)
        {
            cbo_Store.Bind(changedate, "2");
            cbo_Store.SelectedValue = scd;
        }
        private void FrmSearch_Customer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                F11();
        }
        private void FrmSearch_Customer_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (txtKeyword.Text.Length >= 13)
                {
                    txtKeyword.Text = txtKeyword.Text + ",";
                }
            }
        }
        private void gv_CustomerSearch_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }
        private void txtCustCDTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtCustCDFrom.Text) && !string.IsNullOrWhiteSpace(txtCustCDTo.Text))
                {
                    if (string.Compare(txtCustCDFrom.Text, txtCustCDTo.Text) == 1)
                    {
                        scbl.ShowMessage("E106");
                        txtCustCDFrom.Focus();
                    }
                }
            }
            
        }
    }
}
