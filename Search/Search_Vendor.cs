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
using Search;
using BL;
using Entity;


namespace Search
{
    public partial class Search_Vendor : FrmSubForm
    {
        #region"公開プロパティ"
        public string parCustomerCD = "";
        public string parChangeDate = "";
        public string parCustomerName = "";
        #endregion

        Vendor_BL vbl;
        M_Vendor_Entity mve;
        public string VendorCD = string.Empty;
        public string VendorName = string.Empty;
        public string ChangeDate = "";
   
        public Search_Vendor(string ChangeDate, string VendorKBN)
        {
            InitializeComponent();
            txtChangeDate.Text = ChangeDate;
            F9Visible = false;
            vbl = new Vendor_BL();
            lblVendorKBN.Text = VendorKBN;
            F11Visible = false;
            //dgvSearchVendor.DisabledColumn("colVendorCD,colVendorName,Column3,Column4,colChangeDate");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            F11();
        }
        private void F1()
        {
            this.Close();
        }
        private void F11()
        {
            if (ErrorCheck())
            {
                mve = GetSearchInfo();
                DataTable dt = vbl.M_SearchVendor(mve);
                if (dt.Rows.Count > 0)
                {
                    dgvSearchVendor.DataSource = dt;
                    dgvSearchVendor.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    dgvSearchVendor.CurrentRow.Selected = true;
                    dgvSearchVendor.Enabled = true;
                    dgvSearchVendor.Focus();
                }
                else
                {
                    vbl.ShowMessage("E128");
                    dgvSearchVendor.DataSource = null;
                    txtChangeDate.Focus();
                }
            }
        }
        private bool ErrorCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtSupplierNoFrom.Text) && !string.IsNullOrWhiteSpace(txtSupplierNoTo.Text))
            {
                if (string.Compare(txtSupplierNoFrom.Text, txtSupplierNoTo.Text)==1)
                    {
                      vbl.ShowMessage("E106");
                      return false;
                }
            }
            return true;
         }
        

        private void dgvSearchVendor_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }
        private M_Vendor_Entity GetSearchInfo()
        {
            string[] strlist = txtNotDisplayNote.Text.Split(',');
            mve = new M_Vendor_Entity()
            {
                VendorKBN = lblVendorKBN.Text,
                ChangeDate = txtChangeDate.Text,
                VendorName = txtVendorName.Text,
                VendorKana = txtVendorKana.Text,
                NotDisplyNote = txtNotDisplayNote.Text,
                VendorCDFrom = txtSupplierNoFrom.Text,
                VendorCDTo = txtSupplierNoTo.Text,
                Keyword1 = (strlist.Length > 0) ? strlist[0].ToString() : "",
                Keyword2 = (strlist.Length > 1) ? strlist[1].ToString() : "",
                Keyword3 = (strlist.Length > 2) ? strlist[2].ToString() : "",
            };
            return mve;
        }

        private void Search_Vendor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                F11();
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
            if (dgvSearchVendor.CurrentRow != null && dgvSearchVendor.CurrentRow.Index >= 0)
            {
                ChangeDate = dgvSearchVendor.CurrentRow.Cells["colChangeDate"].Value.ToString();
                VendorCD = dgvSearchVendor.CurrentRow.Cells["colVendorCD"].Value.ToString();
                VendorName = dgvSearchVendor.CurrentRow.Cells["colVendorName"].Value.ToString();
                this.Close();
            } 
        }

        private void txtNotDisplayNote_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (txtNotDisplayNote.Text.Length >= 13)
                {
                    txtNotDisplayNote.Text = txtNotDisplayNote.Text + ',';
                }
            }
        }

        private void txtSupplierNoTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtSupplierNoFrom.Text) && !string.IsNullOrWhiteSpace(txtSupplierNoTo.Text))
                {
                    if (string.Compare(txtSupplierNoFrom.Text, txtSupplierNoTo.Text) == 1)
                    {
                        vbl.ShowMessage("E106");
                        txtSupplierNoFrom.Focus();
                    }
                }
            }
        }

        private void Search_Vendor_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void Search_Vendor_Load(object sender, EventArgs e)
        {
            txtChangeDate.Focus();
        }
    }
}

