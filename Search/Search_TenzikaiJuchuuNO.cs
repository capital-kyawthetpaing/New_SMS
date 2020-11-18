﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;

namespace Search
{
    public partial class Search_TenzikaiJuchuuNO : FrmSubForm
    {
        TenzikaiJuchuuNo_BL tzkjbl;
        M_Vendor_Entity mve;
        M_Customer_Entity mce;
        D_TenzikaiJuchuu_Entity dtje;
        private Control PreCon = null;
        Control[] DetailControls;
        

        string year = string.Empty; string month = string.Empty; string day = string.Empty; string date = string.Empty;
        public string OrderNum = string.Empty;

        public Search_TenzikaiJuchuuNO()
        {
            InitializeComponent();
        }

        public void Search_TenzikaiJuchuuNO_Load(object sender, EventArgs e)
        {
            tzkjbl = new TenzikaiJuchuuNo_BL();
            mve = new M_Vendor_Entity();
            mce = new M_Customer_Entity();
            //DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, now.Month, 1);
            //txtOrderDateFrom.Text = startDate.ToString("yyyy/MM/dd");
            txtOrderDateTo.Text = bbl.GetDate();
            string[] date = txtOrderDateTo.Text.Split('/');
            month = date[date.Length - 2].PadLeft(2, '0');
            if (date.Length > 2)
                year = date[date.Length - 3];
            txtOrderDateFrom.Text = year + '/' + month + '/' + "01";
            BindCombo();
            txtSupplierName.Enabled = true;
            txtSupplierName.Text = string.Empty;
            txtCustomerName.Enabled = true;
            txtCustomerName.Text = string.Empty;
            SetRequiredField();
            txtOrderDateFrom.Focus();
            F9Visible = false;
            DetailControls = new Control[] {txtOrderDateTo, txtOrderDateFrom,txtKanaName, txtSupplierName, txtCustomerName, ScSupplier.TxtCode, scStaff.TxtCode , ScSKUCD.TxtCode, ScItem.TxtCode,ScJanCD.TxtCode,cboSeason,cboYear};
            foreach (var c in DetailControls)
            {
                c.Enter += Search_TenzikaiJuchuuNO_Enter;
                c.Leave += Search_TenzikaiJuchuuNO_Enter;
            }
        }
        private void Search_TenzikaiJuchuuNO_Enter(object sender, EventArgs e)
        {
                PreCon = sender as Control;
        }

        private void BindCombo()
        {
            string ymd = bbl.GetDate();
            cboYear.Bind(ymd);
            cboSeason.Bind(ymd);
        }

        public void SetRequiredField()
        {
            txtOrderDateFrom.Require(true);
            txtOrderDateTo.Require(true);
            //cboYear.Require(true);
            //cboSeason.Require(true);
        }

        public override void FunctionProcess(int index)
        {
            if (index + 1 == 12)
            {
                GetData();
            }
            else if (index + 1 == 11)
            {
                F11();
            }
        }

        private void F11()
        {
            if (ErrorCheck(11))
            {
                dtje = GetSearchInfo();
                DataTable dtcus = tzkjbl.D_TenzikaiJuchuu_SearchData(dtje);
                if(dtcus.Rows.Count > 0)
                {
                    dgvTenzikai.DataSource = dtcus;
                }
                else
                {
                    dgvTenzikai.DataSource = string.Empty;
                    bbl.ShowMessage("S013");
                    if (PreCon != null)
                    PreCon.Focus();
                    //ScSupplier.SetFocus(1);
                    //PreviousCtrl.Focus();

                }
                
            }
        }

        private D_TenzikaiJuchuu_Entity GetSearchInfo()
        {
            dtje = new D_TenzikaiJuchuu_Entity
            {
                JuchuuDateFrom = txtOrderDateFrom.Text,
                JuchuuDateTo = txtOrderDateTo.Text,
                VendorCD = ScSupplier.TxtCode.Text,
                year = cboYear.SelectedValue.ToString().Equals("-1") ? string.Empty : cboYear.SelectedValue.ToString(),
                season = cboSeason.SelectedValue.Equals("-1") ? string.Empty : cboSeason.SelectedValue.ToString(),
                StaffCD = scStaff.TxtCode.Text,
                CustomerCD = ScCustomer.TxtCode.Text,
                ProuductName = txtKanaName.Text,
                ItemCD = ScItem.TxtCode.Text,
                SKUCD = ScSKUCD.TxtCode.Text,
                JanCD = ScJanCD.TxtCode.Text,
            };
            return dtje;
        }

        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                if (!txtOrderDateFrom.DateCheck())
                    return false;

                if (!txtOrderDateTo.DateCheck())
                    return false;

                if (!RequireCheck(new Control[] { txtOrderDateFrom }))
                    return false;

                if (!RequireCheck(new Control[] { txtOrderDateTo }))
                    return false;
                else
                {
                    if (string.Compare(txtOrderDateFrom.Text, txtOrderDateTo.Text) == 1)
                    {
                        tzkjbl.ShowMessage("E104");
                        txtOrderDateTo.Focus();
                        return false;
                    }
                }

                if (string.IsNullOrWhiteSpace(txtOrderDateTo.Text))
                {
                    ScSupplier.ChangeDate = bbl.GetDate();
                    scStaff.ChangeDate = bbl.GetDate();
                    ScCustomer.ChangeDate = bbl.GetDate();
                }
                else
                {
                    ScSupplier.ChangeDate = txtOrderDateTo.Text;
                    scStaff.ChangeDate = txtOrderDateTo.Text;
                    ScCustomer.ChangeDate = txtOrderDateTo.Text;
                }
                //if (!string.IsNullOrWhiteSpace(ScSupplier.TxtCode.Text))
                //{                  
                //    if (!ScSupplier.IsExists(2))
                //    {
                //        bbl.ShowMessage("E101");
                //        ScSupplier.SetFocus(1);
                //        return false;
                //    }
                //}
                if (!string.IsNullOrWhiteSpace(ScSupplier.TxtCode.Text))
                {
                    //if (!ScSupplier.SelectData())
                    //{
                    //    bbl.ShowMessage("E101");
                    //    ScSupplier.SetFocus(1);
                    //}
                    mve.VendorCD = ScSupplier.TxtCode.Text;
                    mve.ChangeDate = ScSupplier.ChangeDate;
                    DataTable dtvendor = new DataTable();
                    dtvendor = tzkjbl.M_Vendor_SelectForJuchuu(mve);
                    if (dtvendor.Rows.Count > 0)
                    {
                        txtSupplierName.Text = dtvendor.Rows[0]["VendorName"].ToString();
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScSupplier.SetFocus(1);
                    }
                }
               

                //if (string.IsNullOrWhiteSpace(cboYear.Text.ToString()))
                //{
                //    tzkjbl.ShowMessage("E102");
                //    cboYear.Focus();
                //    return false;
                //}

                //if (string.IsNullOrWhiteSpace(cboSeason.Text.ToString()))
                //{
                //    tzkjbl.ShowMessage("E102");
                //    cboSeason.Focus();
                //    return false;
                //}

                if (!string.IsNullOrWhiteSpace(scStaff.TxtCode.Text))
                {                   
                    if (!scStaff.IsExists(2))
                    {
                        bbl.ShowMessage("E101");
                        scStaff.SetFocus(1);
                        return false;
                    }
                }

                if(!string.IsNullOrWhiteSpace(ScCustomer.TxtCode.Text))
                {
                    mce.CustomerCD = ScCustomer.TxtCode.Text;
                    mce.ChangeDate = ScCustomer.ChangeDate;
                    DataTable dtc = new DataTable();
                    dtc = tzkjbl.M_Customer_SelectForTenzikai(mce);
                    if (dtc.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E101");
                        ScCustomer.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        //if (dtc.Rows[0]["VariousFLG"].ToString() == "1")
                        //{
                        //    txtCustomerName.Text = dtc.Rows[0]["CustomerName"].ToString();
                        //    //txtCustomerName.BackColor = Color.White;
                        //    txtCustomerName.Enabled = true;
                        //}
                        //else if (dtc.Rows[0]["VariousFLG"].ToString() == "0")
                        //{
                        //    txtCustomerName.Text = dtc.Rows[0]["CustomerName"].ToString();
                        //    //txtCustomerName.BackColor = Color.FromArgb(218, 218, 212);
                        //    txtCustomerName.Enabled = false;
                        //}
                        txtCustomerName.Text = dtc.Rows[0]["CustomerName"].ToString();
                    }

                }

            }
            if (index == 12)
            {
                if (!txtOrderDateFrom.DateCheck())
                    return false;

                if (!txtOrderDateTo.DateCheck())
                    return false;
            }
                return true;

        }

        private void ScSupplier_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtOrderDateTo.Text))
                {
                    ScSupplier.ChangeDate = bbl.GetDate();                 
                }
                else
                {
                    ScSupplier.ChangeDate = txtOrderDateTo.Text;                   
                }
                if (!string.IsNullOrWhiteSpace(ScSupplier.TxtCode.Text))
                {
                    //if (!ScSupplier.SelectData())
                    //{
                    //    bbl.ShowMessage("E101");
                    //    ScSupplier.SetFocus(1);
                    //}
                    mve.VendorCD = ScSupplier.TxtCode.Text;
                    mve.ChangeDate = ScSupplier.ChangeDate;
                    DataTable dtvendor = new DataTable();
                    dtvendor = tzkjbl.M_Vendor_SelectForJuchuu(mve);
                    if(dtvendor.Rows.Count > 0 )
                    {
                        txtSupplierName.Text = dtvendor.Rows[0]["VendorName"].ToString();
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScSupplier.SetFocus(1);
                    }
                }
                else
                {
                    txtSupplierName.Text = string.Empty;
                }
            }
        }

        private void scStaff_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtOrderDateTo.Text))
                {
                    scStaff.ChangeDate = bbl.GetDate();
                }
                else
                {
                    scStaff.ChangeDate = txtOrderDateTo.Text;
                }
                if (!string.IsNullOrWhiteSpace(scStaff.TxtCode.Text))
                {
                    if (!scStaff.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scStaff.SetFocus(1);
                    }
                }
            }
        }

        private void ScCustomer_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtOrderDateTo.Text))
                {
                    ScCustomer.ChangeDate = bbl.GetDate();
                }
                else
                {
                    ScCustomer.ChangeDate = txtOrderDateTo.Text;
                }
                if(!string.IsNullOrWhiteSpace(ScCustomer.TxtCode.Text))
                {
                    mce.CustomerCD = ScCustomer.TxtCode.Text;
                    mce.ChangeDate = txtOrderDateTo.Text;
                    DataTable dtcus = new DataTable();
                    dtcus = tzkjbl.M_Customer_SelectForTenzikai(mce);
                    if (dtcus.Rows.Count > 0)
                    {
                        //if (dtcus.Rows[0]["VariousFLG"].ToString() == "1")
                        //{
                        //    txtCustomerName.Text = dtcus.Rows[0]["CustomerName"].ToString();
                        //    //txtCustomerName.BackColor = Color.White;
                        //    txtCustomerName.Enabled = true;
                        //}
                        //else if (dtcus.Rows[0]["VariousFLG"].ToString() == "0")
                        //{
                        //    txtCustomerName.Text = dtcus.Rows[0]["CustomerName"].ToString();
                        //    //txtCustomerName.BackColor = Color.LightGray;
                        //    txtCustomerName.Enabled = false;
                        //}
                        txtCustomerName.Text = dtcus.Rows[0]["CustomerName"].ToString();
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        txtCustomerName.Text = string.Empty;
                        ScCustomer.SetFocus(1);
                    }
                }
                else
                {
                    txtCustomerName.Text = string.Empty;
                }
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }

        //private void dgvTenzikai_KeyUp(object sender, KeyEventArgs e)
        //{
        //      MoveNextControl(e);
        //}

        private void GetData()
        {
            if (dgvTenzikai.CurrentRow != null && dgvTenzikai.CurrentRow.Index >= 0)
            {
                OrderNum = dgvTenzikai.CurrentRow.Cells["colOrderNum"].Value.ToString();              
                this.Close();
            }
        }

        private void dgvTenzikai_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }

        private void Search_TenzikaiJuchuuNO_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void txtOrderDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtOrderDateTo.Text))
                {
                    int result = txtOrderDateFrom.Text.CompareTo(txtOrderDateTo.Text);
                    if (result > 0)
                    {
                        tzkjbl.ShowMessage("E104");
                        txtOrderDateTo.Focus();
                    }
                }
            }
           
        }

        private void cboYear_KeyDown(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode == Keys.Enter)
            //{
            //    cboSeason.Focus();
            //}
        }

        private void cboSeason_KeyDown(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode == Keys.Enter)
            //{
            //    scStaff.SetFocus(1);
            //}
        }

    }
}
