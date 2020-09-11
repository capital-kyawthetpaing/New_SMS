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
    public partial class Search_TenzikaiJuchuuNO :  FrmSubForm
    {
        TenzikaiJuchuuNo_BL tzkjbl;
        M_Vendor_Entity mve;
        M_Customer_Entity mce;
        public Search_TenzikaiJuchuuNO()
        {
            InitializeComponent();
        }

        public void Search_TenzikaiJuchuuNO_Load(object sender, EventArgs e)
        {
            tzkjbl = new TenzikaiJuchuuNo_BL();
            mve = new M_Vendor_Entity();
            mce = new M_Customer_Entity();
            txtOrderDateTo.Text = DateTime.Now.ToString();
            BindCombo();
        }

        private void BindCombo()
        {
            string ymd = bbl.GetDate();
            cboYear.Bind(ymd);
            cboSeason.Bind(ymd);
        }

        private void F11()
        {
            if (ErrorCheck(11))
            {
                //mse = GetSearchInfo();
                //DataTable dtSouko = ssbl.M_Souko_Search(mse);
                //GvSouko.DataSource = dtSouko;
            }
        }

        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                if (!txtOrderDateFrom.DateCheck())
                    return false;

                if (!txtOrderDateTo.DateCheck())
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
                if (string.IsNullOrWhiteSpace(ScSupplier.TxtCode.Text))
                {
                    if (ScSupplier.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScSupplier.SetFocus(1);
                        return false;
                    }
                }

                if (string.IsNullOrWhiteSpace(cboYear.Text.ToString()))
                {
                    tzkjbl.ShowMessage("E102");
                    cboYear.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(cboSeason.Text.ToString()))
                {
                    tzkjbl.ShowMessage("E102");
                    cboSeason.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(scStaff.TxtCode.Text))
                {
                    if (scStaff.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scStaff.SetFocus(1);
                        return false;
                    }
                }

                if(string.IsNullOrWhiteSpace(ScCustomer.TxtCode.Text))
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
                        if (dtc.Rows[0]["VariousFLG"].ToString() == "1")
                        {
                            txtCustomerName.Text = dtc.Rows[0]["CustomerName"].ToString();
                            txtCustomerName.BackColor = Color.White;
                            txtCustomerName.Enabled = true;
                        }
                        else if (dtc.Rows[0]["VariousFLG"].ToString() == "0")
                        {
                            txtCustomerName.Text = dtc.Rows[0]["CustomerName"].ToString();
                            txtCustomerName.BackColor = Color.Gray;
                            txtCustomerName.Enabled = false;
                        }
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
                if (string.IsNullOrWhiteSpace(ScSupplier.TxtCode.Text))
                {
                    if (ScSupplier.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScSupplier.SetFocus(1);
                    }
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
                mce.CustomerCD = ScCustomer.TxtCode.Text;
                mce.ChangeDate = ScCustomer.ChangeDate;
                DataTable dtcus = new DataTable();
                dtcus = tzkjbl.M_Customer_SelectForTenzikai(mce);
                if(dtcus.Rows.Count > 0)
                {
                    if(dtcus.Rows[0]["VariousFLG"].ToString() == "1")
                    {
                       txtCustomerName.Text = dtcus.Rows[0]["CustomerName"].ToString();
                        txtCustomerName.BackColor = Color.White;
                        txtCustomerName.Enabled = true;
                    }
                    else if (dtcus.Rows[0]["VariousFLG"].ToString() == "0")
                    {
                        txtCustomerName.Text = dtcus.Rows[0]["CustomerName"].ToString();
                        txtCustomerName.BackColor = Color.Gray;
                        txtCustomerName.Enabled = false;
                    }
                }
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void dgvTenzikai_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
