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
    public partial class FrmSearch_Carrier : FrmSubForm
    {
        Search_Carrier_BL scbl;
        M_Shipping_Entity mse;

        public string ID = "";
        public string date = "";
        public string parName = "";

        public string parChangeDate = "";
        public string ChangeDate = string.Empty;

        public FrmSearch_Carrier(string changeDate)
        {
            InitializeComponent();
            F9Visible = false;
            lblChangeDate.Text = changeDate;
            scbl = new Search_Carrier_BL();
        }

        private void Search_Carrier_Load(object sender, EventArgs e)
        {
            if (DateTime.TryParse(parChangeDate, out DateTime dt))
                lblChangeDate.Text = parChangeDate;
            else
                lblChangeDate.Text = scbl.GetDate();
        }

        private void txtShippingTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ErrorCheck())
                    txtShippingTo.Focus();
            }
        }

        private bool ErrorCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtShippingTo.Text))
            {
                int result = txtShippingFrom.Text.CompareTo(txtShippingTo.Text);
                if (result > 0)
                {
                    scbl.ShowMessage("E106");
                    txtShippingTo.Focus();
                    return false;
                }
            }

            return true;
        }

        private void F11Show_Click(object sender, EventArgs e)
        {
            F11();
        }

        private M_Shipping_Entity GetData()
        {
            mse = new M_Shipping_Entity
            {
                ChangeDate = lblChangeDate.Text,
                DisplayKBN = radioButton1.Checked ? "0" : "1",
                ShippingCDFrom = txtShippingFrom.Text,
                ShippingCDTo= txtShippingTo.Text,
                ShippingName = txtShippingName.Text,
                DeleteFlg = "0",
            };
            return mse;
        }

        private void F11()
        {
            if (ErrorCheck())
            {
                mse = GetData();
                DataTable dt = new DataTable();
                dt = scbl.M_Carrier_Search(mse);
                if (dt.Rows.Count > 0)
                {
                    gvShipping.DataSource = dt;
                    gvShipping.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    gvShipping.CurrentRow.Selected = true;
                    gvShipping.Enabled = true;
                    gvShipping.Focus();
                }
                else
                {
                    scbl.ShowMessage("E128");
                    gvShipping.DataSource = null;
                    txtShippingFrom.Focus();
                }

            }
            else
            {
                gvShipping.DataSource = null;
            }
        }

        public override void FunctionProcess(int Index)
        {
            if (Index + 1 == 12)
            {
                SendData();
            }
        }

        private void SendData()
        {
            if (gvShipping.CurrentRow != null && gvShipping.CurrentRow.Index >= 0)
            {
                ID = gvShipping.CurrentRow.Cells["colCD"].Value.ToString();
                date = gvShipping.CurrentRow.Cells["colDate"].Value.ToString();
                parName = gvShipping.CurrentRow.Cells["colName"].Value.ToString();
               
            }
            this.Close();
        }

        private void gvShipping_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SendData();
        }

        private void Search_Carrier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                F11();
        }

        private void FrmSearch_Carrier_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
