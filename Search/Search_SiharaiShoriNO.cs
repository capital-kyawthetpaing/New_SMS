using System;
using System.Data;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;

namespace Search
{
    public partial class Search_SiharaiShoriNO : FrmSubForm
    {
        Search_SiharaiShoriNO_BL sssbl;
        D_Pay_Entity dpe = new D_Pay_Entity();

        public string Sc_Code = "";
        public string date = "";
        public string parChangeDate = "";
        public string parName = "";

        public Search_SiharaiShoriNO()
        {
            InitializeComponent();
        }

        private void Search_SiharaiShoriNO_Load(object sender, EventArgs e)
        {
            F9Visible = false;
            sssbl = new Search_SiharaiShoriNO_BL();
        }

        public override void FunctionProcess(int Index)
        {
            if (Index + 1 == 12)
            {
                SendData();
            }
            else if (Index + 1 == 11)
            {
                F11();
            }
        }

        private void txtPaymentDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(!string.IsNullOrWhiteSpace(txtPaymentDateTo.Text))
                {                   
                     int result = txtPaymentDateFrom.Text.CompareTo(txtPaymentDateTo.Text);
                     if (result > 0)
                     {
                         sssbl.ShowMessage("E104");
                         txtPaymentDateTo.Focus();
                     }    
                }
            }
        }

        private void txtPaymentInputDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(!string.IsNullOrWhiteSpace(txtPaymentInputDateTo.Text))
                {
                    int result = txtPaymentInputDateFrom.Text.CompareTo(txtPaymentInputDateTo.Text);
                    if(result > 0)
                    {
                        sssbl.ShowMessage("E104");
                        txtPaymentInputDateTo.Focus();
                    }
                }
            }
        }

        public bool ErrorCheck()
        {
            if (!txtPaymentDateFrom.DateCheck())
                return false;
            if (!txtPaymentDateTo.DateCheck())
                return false;

            if (!string.IsNullOrWhiteSpace(txtPaymentDateTo.Text))
            {
                int result = txtPaymentDateFrom.Text.CompareTo(txtPaymentDateTo.Text);
                if (result > 0)
                {
                    sssbl.ShowMessage("E104");
                    txtPaymentDateTo.Focus();
                    return false;
                }
            }

            if (!txtPaymentInputDateFrom.DateCheck())
                return false;
            if (!txtPaymentInputDateTo.DateCheck())
                return false;

            if (!string.IsNullOrWhiteSpace(txtPaymentInputDateTo.Text))
            {
                int result = txtPaymentInputDateFrom.Text.CompareTo(txtPaymentInputDateTo.Text);
                if (result > 0)
                {
                    sssbl.ShowMessage("E104");
                    txtPaymentInputDateTo.Focus();
                    return false;
                }
            }
            return true;
        }

        private void btnF11Show_Click(object sender, EventArgs e)
        {          
            F11();          
        }

        public D_Pay_Entity GetData()
        {
            dpe = new D_Pay_Entity()
            {
                PayDateFrom = txtPaymentDateFrom.Text,
                PayDateTo = txtPaymentDateTo.Text,
                InputDateTimeFrom = txtPaymentInputDateFrom.Text ,
                InputDateTimeTo = txtPaymentInputDateTo.Text 
            };
            return dpe;

        }
        protected override void ExecDisp()
        {
            F11();
        }
        public void F11()
        {
            if (ErrorCheck())
            {
                dpe = GetData();
                DataTable dt = new DataTable();
                dt = sssbl.D_Pay_Search(dpe);

                if (dt.Rows.Count > 0)
                {
                    dgvSiharaiShoriNO.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvSiharaiShoriNO.DataSource = dt;
                    dgvSiharaiShoriNO.Enabled = true;
                    dgvSiharaiShoriNO.Focus();
                }
                else
                {
                    sssbl.ShowMessage("E128");
                    dgvSiharaiShoriNO.DataSource = null;
                    txtPaymentDateFrom.Focus();
                }
            }
               
        }

        public void SendData()
        {
            if (dgvSiharaiShoriNO.CurrentRow != null && dgvSiharaiShoriNO.CurrentRow.Index >= 0)
            {
                Sc_Code = dgvSiharaiShoriNO.CurrentRow.Cells["colPaymentNum"].Value.ToString();
                //date = dgvSiharaiShoriNO.CurrentRow.Cells["colDate"].Value.ToString();
                //parName = dgvSiharaiShoriNO.CurrentRow.Cells["colName"].Value.ToString();
                this.Close();
            }
        }

        private void dgvSiharaiShoriNO_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SendData();
        }

        private void Search_SiharaiShoriNO_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
