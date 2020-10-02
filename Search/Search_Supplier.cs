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
    public partial class Search_Supplier : FrmSubForm
    {
        Search_Supplier_BL ssbl;
        M_Vendor_Entity mve;

        public string ID = "";
        public string date = "";
        public string parChangeDate = "";
        public string parName = "";
        // public string ChangeDate = string.Empty;
        public string ChangeDate = string.Empty;

        public Search_Supplier(string changeDate)
        {
            InitializeComponent();
            F9Visible = false;
            lblChangeDate.Text = changeDate;
            ssbl = new Search_Supplier_BL();
        }

        private void Search_Supplier_Load(object sender, EventArgs e)
        {
            if (DateTime.TryParse(parChangeDate, out DateTime dt))
                lblChangeDate.Text = parChangeDate;
            else
                lblChangeDate.Text = ssbl.GetDate();
        }

        private void txtSupplierTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ErrorCheck())
                    txtSupplierName.Focus();
            }
        }

        private bool ErrorCheck()
        {
            if(!string .IsNullOrWhiteSpace (txtSupplierTo .Text))
            {
                int result = txtSupplierFrom.Text.CompareTo(txtSupplierTo.Text); 
                if(result > 0 )
                {
                    ssbl.ShowMessage("E106");
                    txtSupplierTo.Focus();
                    return false;
                }
            }

            return true;
        }

        private void F11Show_Click(object sender, EventArgs e)
        {
            F11();
        }

        private M_Vendor_Entity GetData()
        {
            mve = new M_Vendor_Entity
            {
                ChangeDate = lblChangeDate.Text,
                DisplayKBN = radioButton1.Checked ? "0" : "1",
                VendorCDFrom = txtSupplierFrom.Text,
                VendorCDTo = txtSupplierTo.Text,
                VendorName = txtSupplierName.Text,
                VendorKana = txtKanaName.Text,
                DeleteFlg = "0",
            };
            return mve;
        }

        private void F11()
        {
            if (ErrorCheck())
            {
                mve = GetData();
                DataTable dt = new DataTable();
                dt = ssbl.M_Vendor_Search(mve);
                if (dt.Rows.Count > 0)
                {
                    gvSupplier.DataSource = dt;
                    gvSupplier.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    gvSupplier.CurrentRow.Selected = true;
                    gvSupplier.Enabled = true;
                    gvSupplier.Focus();
                }
                else
                {
                    ssbl.ShowMessage("E128");
                    gvSupplier.DataSource = null;
                    txtSupplierFrom.Focus();
                }
                
            }
            else
            {
                gvSupplier.DataSource = null;
            }
        }

        public override void FunctionProcess(int Index)
        {
            if (Index + 1 == 12)
            {
                SendData();
            }
            else if(Index + 1 == 11)
            {
                F11();
            }
        }

        private void SendData()
        {
            if (gvSupplier.CurrentRow != null && gvSupplier.CurrentRow.Index >= 0)
            {
                ID = gvSupplier.CurrentRow.Cells["colCD"].Value.ToString();
                date = gvSupplier.CurrentRow.Cells["colDate"].Value.ToString();
                parName = gvSupplier.CurrentRow.Cells["colName"].Value.ToString();
                this.Close();
            }
        }

        private void gvSupplier_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SendData();
        }

        private void Search_Supplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                F11();
        }

        private void Search_Supplier_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
