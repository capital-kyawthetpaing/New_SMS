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
    public partial class FrmSearch_SiharaiNO : FrmSubForm
    {
        Search_SiharaiNO_BL ssnbl = new Search_SiharaiNO_BL();
        M_Vendor_Entity mve = new M_Vendor_Entity();
        D_Pay_Entity dpe = new D_Pay_Entity();

        public string ID = "";
        public string date = "";
        public string parName = "";

        public FrmSearch_SiharaiNO()
        {
            InitializeComponent();

            F9Visible = false;

        }

        private void txtPaymentDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtPaymentDateTo.Text))
                {
                    int result = txtPaymentDateFrom.Text.CompareTo(txtPaymentDateTo.Text);
                    if (result > 0)
                    {
                        ssnbl.ShowMessage("E104");
                        txtPaymentDateTo.Focus();
                    }
                }
            }
        }

        private void txtPaymentInputDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtPaymentDateTo.Text))
            {
                int result = txtPaymentDateFrom.Text.CompareTo(txtPaymentDateTo.Text);
                if(result > 0)
                {
                    ssnbl.ShowMessage("E104");
                    txtPaymentDateTo.Focus();
                }
            }
        }

        public bool ErrorCheck()
        {
            string     strYmd = bbl.FormatDate(txtPaymentDateFrom.Text);
            //日付として正しいこと(Be on the correct date)Ｅ１０３
            if (!bbl.CheckDate(strYmd))
            {
                //Ｅ１０３
                bbl.ShowMessage("E103");
                return false;
            }
            txtPaymentDateFrom.Text = strYmd;

            strYmd = bbl.FormatDate(txtPaymentDateTo.Text);
            //日付として正しいこと(Be on the correct date)Ｅ１０３
            if (!bbl.CheckDate(strYmd))
            {
                //Ｅ１０３
                bbl.ShowMessage("E103");
                return false;
            }
            txtPaymentDateTo.Text = strYmd;

            if (!string.IsNullOrWhiteSpace(txtPaymentDateTo.Text))
            {
                int result = txtPaymentDateFrom.Text.CompareTo(txtPaymentDateTo.Text);
                if (result > 0)
                {
                    ssnbl.ShowMessage("E104");
                    txtPaymentDateTo.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtPaymentDateTo.Text))
            {
                int result = txtPaymentDateFrom.Text.CompareTo(txtPaymentDateTo.Text);
                if (result > 0)
                {
                    ssnbl.ShowMessage("E104");
                    txtPaymentDateTo.Focus();
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(Sc_PaymentDestination.TxtCode.Text))
            {
                mve.VendorCD = Sc_PaymentDestination.TxtCode.Text;
                mve.ChangeDate = DateTime.Today.ToShortDateString();
                mve.PayeeFlg = "1";
                DataTable dtvendor = new DataTable();
                dtvendor = ssnbl.M_Vendor_Select(mve);
                if (dtvendor.Rows.Count == 0)
                {
                    ssnbl.ShowMessage("E101");
                    Sc_PaymentDestination.Focus();
                    return false;
                }
                else
                {
                    Sc_PaymentDestination.LabelText = dtvendor.Rows[0]["VendorName"].ToString();
                }
            }
            else
            {
                Sc_PaymentDestination.LabelText = "";
            }
            return true;
        }

        private void Sc_PaymentDestination_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Sc_PaymentDestination.TxtCode.Text))
            {
                mve.VendorCD = Sc_PaymentDestination.TxtCode.Text;
                mve.ChangeDate = DateTime.Today.ToShortDateString();
                mve.PayeeFlg = "1";
                DataTable dtvendor = new DataTable();
                dtvendor = ssnbl.M_Vendor_Select(mve);
                if (dtvendor.Rows.Count == 0)
                {
                    ssnbl.ShowMessage("E101");
                    Sc_PaymentDestination.Focus();
                }
                else
                {
                    Sc_PaymentDestination.LabelText = dtvendor.Rows[0]["VendorName"].ToString();
                }
            }
            else
            {
                Sc_PaymentDestination.LabelText = "";
            }
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

        private void btnShow_Click(object sender, EventArgs e)
        {
            F11();
        }

        public void F11()
        {
            if(ErrorCheck())
            {
                dpe.PayDateFrom = txtPaymentDateFrom.Text;
                dpe.PayDateTo = txtPaymentDateTo.Text;
                dpe.InputDateTimeFrom = txtPaymentInputDateFrom.Text;
                dpe.InputDateTimeTo = txtPaymentInputDateTo.Text;
                dpe.PayeeCD = Sc_PaymentDestination.TxtCode.Text;
                DataTable dt = new DataTable();
                dt = ssnbl.D_Pay_SelectForSiharaiNo(dpe);

                if (dt.Rows.Count == 0)
                {
                    ssnbl.ShowMessage("E128");
                    dgvSiharaiNO.DataSource = null;
                    txtPaymentDateFrom.Focus();
                }
                else
                {
                    dgvSiharaiNO.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvSiharaiNO.DataSource = dt;
                    dgvSiharaiNO.Enabled = true;
                    dgvSiharaiNO.Focus();
                }
            }
        }

        public void SendData()
        {
            if (dgvSiharaiNO.CurrentRow != null && dgvSiharaiNO.CurrentRow.Index >= 0)
            {
                ID = dgvSiharaiNO.CurrentRow.Cells["colPaymentNO"].Value.ToString();
                date = dgvSiharaiNO.CurrentRow.Cells["colPaymentDate"].Value.ToString();
                parName = dgvSiharaiNO.CurrentRow.Cells["colPayeeName"].Value.ToString();
                this.Close();
            }
        }

        private void dgvSiharaiNO_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SendData();
        }

        private void FrmSearch_SiharaiNO_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void dgvSiharaiNO_Paint(object sender, PaintEventArgs e)
        {
            string[] data = { "No.", "支払処理番号", "支払先" };
            for (int j = 4; j < 6;)
            {
                Rectangle r1 = this.dgvSiharaiNO.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.dgvSiharaiNO.GetCellDisplayRectangle(j + 1, -1, true).Width;
                r1.X += 0;
                r1.Y += 0;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dgvSiharaiNO.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(data[j / 2],
                this.dgvSiharaiNO.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.dgvSiharaiNO.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 3;
            }
        }

        private void FrmSearch_SiharaiNO_Load(object sender, EventArgs e)
        {
            txtPaymentDateFrom.Focus();
        }
    }
}
