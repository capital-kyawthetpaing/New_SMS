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
using BL;
using Entity;

namespace Search
{
    public partial class Search_Vendor_Shop : Search_Base
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
        public Search_Vendor_Shop(string ChangeDate, string VendorKBN)
        {
            InitializeComponent();
            InitializeComponent();
            lblChangeDate.Text = ChangeDate;
            lblVendorKBN.Text = VendorKBN;
            ProgramName = "仕入先・支払先検索";
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            F11();
        }

        public override void FunctionProcess(int index)
        {
            if (index + 11 == 12)
            {
                GetData();
            }
        }

        private void F11()
        {
            if (ErrorCheck())
            {
                mve = GetSearchInfo();
                DataTable dt = vbl.M_SearchVendor(mve);
                if (dt.Rows.Count > 0)
                {
                    dgvVendor.DataSource = dt;
                    dgvVendor.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    dgvVendor.CurrentRow.Selected = true;
                    dgvVendor.Enabled = true;
                    dgvVendor.Focus();
                }
                else
                {
                    vbl.ShowMessage("E128");
                    dgvVendor.DataSource = null;
                    txtVendorName.Focus();
                }
            }
        }
        private M_Vendor_Entity GetSearchInfo()
        {
            string[] strlist = txtNotDisplayNote.Text.Split(',');
            mve = new M_Vendor_Entity()
            {
                VendorKBN = lblVendorKBN.Text,
                ChangeDate = lblChangeDate.Text,
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

        private bool ErrorCheck()
        {
            
            if (!string.IsNullOrWhiteSpace(txtSupplierNoFrom.Text) && !string.IsNullOrWhiteSpace(txtSupplierNoTo.Text))
            {
                if (string.Compare(txtSupplierNoFrom.Text, txtSupplierNoTo.Text) == 1)
                {
                    vbl.ShowMessage("E106");
                    return false;
                }
            }
            return true;
        }

        private void GetData()
        {
            if (dgvVendor.CurrentRow != null && dgvVendor.CurrentRow.Index >= 0)
            {
                ChangeDate = dgvVendor.CurrentRow.Cells["colChangeDate"].Value.ToString();
                VendorCD = dgvVendor.CurrentRow.Cells["colVendorCD"].Value.ToString();
                VendorName = dgvVendor.CurrentRow.Cells["colVendorName"].Value.ToString();
                this.Close();
            }
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void Search_Vendor_Shop_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void Search_Vendor_Shop_Load(object sender, EventArgs e)
        {
            vbl = new Vendor_BL();
            if (DateTime.TryParse(ChangeDate, out DateTime dt))
                lblChangeDate.Text = ChangeDate;
            else
                lblChangeDate.Text = vbl.GetDate();
            txtVendorName.Focus();
        }

        private void txtNotDisplayNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
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

        private void Search_Vendor_Shop_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                F11();
        }

        private void dgvVendor_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
