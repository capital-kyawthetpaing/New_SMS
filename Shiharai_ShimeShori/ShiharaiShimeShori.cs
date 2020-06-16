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
using CKM_Controls;
using BL;
using Search;
using Entity;

namespace Shiharai_ShimeShori
{
    public partial class Shiharai_ShimeShori : FrmMainForm
    {
        Shiharai_Shimeshori_BL sss_bl;
        D_PayCloseHistory_Entity dpch_entity;
        Base_BL bbl;
        DataTable dtDisplay;

        public Shiharai_ShimeShori()
        {
            InitializeComponent();
            dtDisplay = new DataTable();
            dpch_entity = new D_PayCloseHistory_Entity();
            sss_bl = new Shiharai_Shimeshori_BL();
            bbl = new Base_BL();
        }
        private void ShiharaiShimeShori_Load(object sender, EventArgs e)
        {
            InProgramID = "Shiharai_ShimeShori";
            SetFunctionLabel(EProMode.INPUT);
            StartProgram();
            BindCombo();            
            RequireFields();
            this.ModeVisible = false;
            this.ModeText = "修正";
            F7Visible = false;
            F8Visible = false;
            F10Visible = false;
        }
        private void RequireFields()
        {
            txtPayCloseDate.Require(true);
            //ScPaymentCD.TxtCode.Require(true);           
        }
        private void BindCombo()
        {
            cboProcessType.Items.Add("支払締");
            cboProcessType.Items.Add("支払締キャンセル");
        }
        public override void FunctionProcess(int index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
            switch (index + 1)
            {
                case 2:
                    ChangeMode(EOperationMode.INSERT);
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeMode(EOperationMode.INSERT);
                        cboProcessType.Focus();
                    }
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    F12();
                    break;
            }
        }
        private void F12()
        {
            dpch_entity = GetDataEntity();
            string ItemType = cboProcessType.Text;

            switch (ItemType)
            {
                case "支払締":
                    if (ErrorCheck(1))
                    {
                        if (bbl.ShowMessage("Q101") == DialogResult.Yes)
                        {
                            if (sss_bl.Insert_ShiHaRaiShime_PaymentClose(dpch_entity, 1) )
                            {
                                sss_bl.ShowMessage("I101");
                                ChangeMode(EOperationMode.INSERT);
                            }
                        }
                    }
                    break;

                case "支払締キャンセル":
                    if (ErrorCheck(2))
                    {
                        if (bbl.ShowMessage("Q102") == DialogResult.Yes)
                        {
                            if (sss_bl.Insert_ShiHaRaiShime_PaymentClose(dpch_entity, 2))
                            {
                                sss_bl.ShowMessage("I101");
                                ChangeMode(EOperationMode.INSERT);
                            }
                        }
                    }
                    break;
            }
        }
        private void F11()
        {
            dpch_entity = GetDataEntity();
            string ItemType = cboProcessType.Text;
            switch (ItemType)
            {
                case "支払締":
                    if (ErrorCheck(1))
                    {
                        BindGrid();
                    }
                    break;
                case "支払締キャンセル":
                    if (ErrorCheck(2))
                    {
                        BindGrid();
                    }

                    break;
            }
        }
        private void BindGrid()
        {
            dgvPaymentClose.ClearSelection();
           // dpch_entity = GetDataEntity();

            DataTable dtPayCost = sss_bl.D_PayClose_Search(dpch_entity);
            if (dtPayCost.Rows.Count > 0)
            {
                dgvPaymentClose.Refresh();
                dgvPaymentClose.DataSource = dtPayCost;
                dgvPaymentClose.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dgvPaymentClose.CurrentRow.Selected = true;
                dgvPaymentClose.Enabled = true;
                dgvPaymentClose.Focus();
            }
            else
            {
                dgvPaymentClose.DataSource = null;
                bbl.ShowMessage("E128");
            }
        }
        private bool ErrorCheck(int Type)
        {
            if (!string.IsNullOrEmpty(ScPaymentCD.TxtCode.Text))
            {
                if (!ScPaymentCD.IsExists(2))
                {
                    sss_bl.ShowMessage("E101");
                    ScPaymentCD.SetFocus(1);
                    return false;
                }
            }

            switch (Type)
            {
                case 1:
                    if (!RequireCheck(new Control[] { txtPayCloseDate })) //Step1
                        return false;

                    if (!sss_bl.Select_PaymentClose(dpch_entity, 1))//Step2
                    {
                        sss_bl.ShowMessage("S013");
                        cboProcessType.Focus();
                        return false;
                    }

                    if (sss_bl.Select_PaymentClose(dpch_entity, 2))//Step3
                    {
                        sss_bl.ShowMessage("S014");
                        cboProcessType.Focus();
                        return false;
                    }
                    break;

                case 2:
                    if (!RequireCheck(new Control[] { txtPayCloseDate })) //Step1
                        return false;

                    if (!sss_bl.Select_PaymentClose(dpch_entity, 3))//Step2
                    {
                        sss_bl.ShowMessage("S013");
                        cboProcessType.Focus();
                        return false;
                    }
                    if (sss_bl.Select_PaymentClose(dpch_entity, 4))//Step3
                    {
                        sss_bl.ShowMessage("S015");
                        cboProcessType.Focus();
                        return false;
                    }
                    break;
            }
            return true;
        }
        /// <summary>
        /// Get Data Value Bind
        /// </summary>
        /// <returns>Value Return</returns>
        private D_PayCloseHistory_Entity GetDataEntity()
        {
            dpch_entity = new D_PayCloseHistory_Entity()
            {
                PaymentDate = txtPayCloseDate.Text,
                PaymentCD = ScPaymentCD.TxtCode.Text,
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                Key = ScPaymentCD.TxtCode.Text + " " + txtPayCloseDate.Text,
                PC = InPcID
            };

            return dpch_entity;
        }
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    cboProcessType.Text = string.Empty;
                    cboProcessType.SelectedText = "支払締";
                    txtPayCloseDate.Text = string.Empty;
                    ScPaymentCD.TxtCode.Text = string.Empty;
                    ScPaymentCD.LabelText = string.Empty;
                    F2Visible = false;
                    F3Visible = false;
                    F4Visible = false;
                    F5Visible = false;
                    F9Visible = false;
                    F11Visible = false;
                    dgvPaymentClose.DataSource = null;
                    cboProcessType.Focus();
                    break;
            }
        }

        protected override void EndSec()
        {
            this.Close();
        }

        /// <summary>
        /// MoveNext Cursor Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShiharaiShimeShori_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        /// <summary>
        /// Vendor code and Name Case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Supplier_CodeKeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                if (!string.IsNullOrEmpty(ScPaymentCD.TxtCode.Text))
                {
                    ScPaymentCD.ChangeDate = bbl.GetDate();
                    if (ScPaymentCD.SelectData())
                    {
                        ScPaymentCD.Value1 = ScPaymentCD.TxtCode.Text;
                        ScPaymentCD.Value2 = ScPaymentCD.LabelText;
                        F11();
                    }
                    else
                    {
                        sss_bl.ShowMessage("E101");
                        ScPaymentCD.SetFocus(1);
                    }
                }
                else
                {

                }
            }
        }
        private void Supplier_Enter(object sender, EventArgs e)
        {
            //ScPaymentCD.ChangeDate = sss_bl.GetDate();
            ScPaymentCD.Value1 = "3";//仕入先区分：3
            ScPaymentCD.ChangeDate = txtPayCloseDate.Text;
        }
        private void txtPayCloseDate_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                DataTable dtResult = new DataTable();
                dtResult = sss_bl.Check_PayCloseDate(txtPayCloseDate.Text);
                if (dtResult.Rows.Count==0)
                {
                    sss_bl.ShowMessage("E115");
                    txtPayCloseDate.Focus();
                }
            }
        }
        private void dgvPaymentClose_Paint(object sender, PaintEventArgs e)
        {
            dgvPaymentClose.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPaymentClose.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPaymentClose.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPaymentClose.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPaymentClose.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            string[] monthes = { "","支払先" };
            for (int j = 2; j < 4;)
            {
                Rectangle r1 = this.dgvPaymentClose.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.dgvPaymentClose.GetCellDisplayRectangle(j + 1, -1, true).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dgvPaymentClose.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(monthes[j / 2],
                this.dgvPaymentClose.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.dgvPaymentClose.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 3;
            }
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            //F11();
        }
    }
}
