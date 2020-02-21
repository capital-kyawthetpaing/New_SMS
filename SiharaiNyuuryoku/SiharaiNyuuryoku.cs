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
using CKM_Controls;

namespace SiharaiNyuuryoku
{
    public partial class FrmSiharaiNyuuryoku : FrmMainForm
    {
        SiharaiNyuuryoku_BL sibl = new SiharaiNyuuryoku_BL();
        D_Pay_Entity dpe = new D_Pay_Entity();
        M_FiscalYear_Entity mfye = new M_FiscalYear_Entity();
        M_Calendar_Entity mce = new M_Calendar_Entity();
        M_Staff_Entity mse = new M_Staff_Entity();
        M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        M_Kouza_Entity mke = new M_Kouza_Entity();
        M_Vendor_Entity mve = new M_Vendor_Entity();
        D_PayPlan_Entity dppe = new D_PayPlan_Entity();

        int type = 0; string mode = "0";
        string vendorCD = string.Empty;

        public FrmSiharaiNyuuryoku()
        {
            InitializeComponent();
        }
        protected override void EndSec()
        {
            this.Close();
        }

        private void FrmSiharaiNyuuryoku_Load(object sender, EventArgs e)
        {
            InProgramID = "SiharaiNyuuryoku";

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            ScPaymentProcessNum.Enabled = false;
            ScPaymentNum.Enabled = false;
            ScPaymentProcessNum.SearchEnable = false;
            ScPaymentNum.SearchEnable = false;

            Btn_F7.Enabled = true;
            Btn_F7.Text = "編集(F7)";
           

            btnF10Show.Enabled = true;
            txtPaymentDate.Enabled = false;

            mse.StaffCD = InOperatorCD;
            mse.ChangeDate = DateTime.Now.ToShortDateString();
            DataTable dtstaff = new DataTable();
            dtstaff = sibl.M_Staff_Select(mse);
            if (dtstaff.Rows.Count > 0)
            {
                ScStaff.LabelText = dtstaff.Rows[0]["StaffName"].ToString();
            }
            ScStaff.Code = InOperatorCD;

            cboPaymentSourceAcc.Enabled = false;
            cboPaymentType.Enabled = false;
            txtBillSettleDate.Enabled = false;

            btnReleaseAll.Enabled = false;
            btnSelectAll.Enabled = false;

            txtDueDate1.Focus();
           
        }

        private void SetRequireField()
        {
            ScPaymentNum.TxtCode.Require(true);
            ScPayee.TxtCode.Require(true);
            txtPaymentDate.Require(true);
            ScStaff.TxtCode.Require(true);
            txtDueDate2.Require(true);
            txtPaymentDate.Require(true);
        }

        private void BindCombo()
        {
            cboPaymentType.Bind(string.Empty);
            cboPaymentSourceAcc.Bind(string.Empty);
        }

        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                    ChangeMode(EOperationMode.INSERT);
                    break;
                case 3:
                    ChangeMode(EOperationMode.UPDATE);
                    break;
                case 4:
                    ChangeMode(EOperationMode.DELETE);
                    break;
                case 5:
                    ChangeMode(EOperationMode.SHOW);
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeMode(OperationMode);
                        ScPaymentProcessNum.SetFocus(1);
                    }
                    else
                        PreviousCtrl.Focus();
                    break;
                case 7:
                    foreach (DataGridViewRow row in dgvPayment.Rows)
                    {
                        bool isSelected = Convert.ToBoolean(row.Cells["colChk"].Value);
                        if (isSelected)
                        {
                            if (OperationMode == EOperationMode.INSERT)
                            {
                                mode = "1";
                            }
                            else
                            {
                                mode = "2";
                            }
                           FrmSiharaiNyuuryoku_2 f2 = new FrmSiharaiNyuuryoku_2(dppe.PayPlanDate,dppe.PayeeCD,mode);
                            f2.Show();                         
                        }
                    }
                    break;
                case 10:
                    F10();
                    break;
                case 12:
                    F12();
                    break;
            }
        }

        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    ScPaymentProcessNum.Enabled = false;
                    ScPaymentNum.Enabled = false;
                    txtDueDate1.Focus();
                    F9Visible = false;
                    F12Enable = true;
                    btnF10Show.Enabled = true;
                    F11Enable = false;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    ScPaymentProcessNum.Enabled = true;
                    ScPaymentNum.Enabled = true;
                    ScPaymentProcessNum.SetFocus(1);
                    txtDueDate1.Enabled = false;
                    txtDueDate2.Enabled = false;
                    ScPayee.Enabled = false;
                    ScPayee.SearchEnable = false;
                    F12Enable = false;
                    btnF10Show.Enabled = F11Enable = false;
                    break;
            }
            ScPaymentProcessNum.SetFocus(1);
        }

        private void F10()
        {
            type = 3;
            if (ErrorCheck(10))
            {
                dppe.PayPlanDateFrom = txtDueDate1.Text;
                dppe.PayPlanDateTo = txtDueDate2.Text;
                dppe.PayeeCD = ScPayee.TxtCode.Text;
                dppe.Operator = InOperatorCD;
                DataTable dtpayplan = new DataTable();
                dtpayplan = sibl.D_Pay_SelectForPayPlanDate1(dppe);
                if (dtpayplan.Rows.Count > 0)
                {
                    txtPaymentDate.Text = DateTime.Today.ToShortDateString();
                    ScStaff.TxtCode.Text = InOperatorCD;
                    ScStaff.LabelText = dtpayplan.Rows[0]["StaffName"].ToString();
                    cboPaymentType.SelectedText = "振込";
                    cboPaymentSourceAcc.SelectedValue = dtpayplan.Rows[0]["KouzaCD"].ToString();
                    txtBillSettleDate.Text = string.Empty;
                    dgvPayment.DataSource = dtpayplan;
                    foreach (DataGridViewRow row1 in dgvPayment.Rows)
                    {
                        if (Convert.ToBoolean(row1.Cells["colChk"].EditedFormattedValue) == false)
                        {
                            row1.Cells["colChk"].Value = true;
                        }
                    }
                    LabelDataBind();
                }

                //Search.Search_Payment sp = new Search.Search_Payment(dpe.LargePayNO, dpe.PayNo, vendorCD, txtPaymentDate.Text, "2");
                //sp.ShowDialog();
            }
        }

        private void F12()
        {

        }

        /// <summary>
        /// Error Check for the whole form
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool ErrorCheck(int index)
        {
            if (index == 10)
            {
                if (type == 1)
                {
                    DataTable dtpay = new DataTable();
                    dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;
                    dtpay = sibl.D_Pay_LargePayNoSelect(dpe);
                    if (dtpay.Rows.Count == 0)
                    {
                        sibl.ShowMessage("138");
                        ScPaymentProcessNum.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(dtpay.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            sibl.ShowMessage("140");
                            ScPaymentProcessNum.SetFocus(1);
                            return false;
                        }
                    }
                }

                else if (type == 2)
                {
                    if (!RequireCheck(new Control[] { ScPaymentNum.TxtCode }))
                        return false;

                    DataTable dtpayno = new DataTable();
                    dpe.PayNo = ScPaymentNum.TxtCode.Text;
                    dtpayno = sibl.D_Pay_PayNoSelect(dpe);
                    if (dtpayno.Rows.Count == 0)
                    {
                        sibl.ShowMessage("138");
                        ScPaymentNum.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(dtpayno.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            sibl.ShowMessage("140");
                            ScPaymentNum.SetFocus(1);
                            return false;
                        }
                        if (!string.IsNullOrWhiteSpace(dtpayno.Rows[0]["FBCreateDate"].ToString()))
                        {
                            sibl.ShowMessage("144");
                            ScPaymentNum.SetFocus(1);
                            return false;
                        }
                        mfye.InputPossibleStartDate = txtPaymentDate.Text;
                        mfye.InputPossibleEndDate = txtPaymentDate.Text;
                        DataTable dtcontrol = new DataTable();
                        dtcontrol = sibl.M_Control_PaymentSelect(mfye);
                        if (dtcontrol.Rows.Count == 0)
                        {
                            sibl.ShowMessage("115");
                            ScPaymentNum.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            DataDisplay();
                        }
                    }
                }

                if (type == 3)
                {
                    if (!RequireCheck(new Control[] { txtDueDate2 }))
                        return false;

                    if (!RequireCheck(new Control[] { ScPayee.TxtCode }))
                        return false;
                    else
                    {
                        mve.PayeeCD = ScPayee.TxtCode.Text;
                        mve.ChangeDate = DateTime.Now.ToShortDateString();
                        DataTable dtvendor = new DataTable();
                        dtvendor = sibl.M_Vendor_Select(mve);
                        if (dtvendor.Rows.Count == 0)
                        {
                            sibl.ShowMessage("115");
                            ScPayee.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            ScPayee.LabelText = dtvendor.Rows[0]["VendorName"].ToString();
                        }
                    }


                }

            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { txtPaymentDate }))
                    return false;

                mfye.InputPossibleStartDate = txtPaymentDate.Text;
                mfye.InputPossibleEndDate = txtPaymentDate.Text;
                DataTable dtcontrol = new DataTable();
                dtcontrol = sibl.M_Control_PaymentSelect(mfye);
                if (dtcontrol.Rows.Count == 0)
                {
                    sibl.ShowMessage("115");
                    ScPaymentNum.SetFocus(1);
                    return false;
                }

                //店舗の締日チェック
                //店舗締マスターで判断
                M_StoreClose_Entity msce = new M_StoreClose_Entity();
                msce.StoreCD = InOperatorCD;
                msce.FiscalYYYYMM = txtPaymentDate.Text.Replace("/", "").Substring(0, 6);
                DataTable dtposition = sibl.CheckClosePosition(msce);
                if (dtposition.Rows.Count > 0)
                {
                    if (dtposition.Rows[0]["ClosePosition2"].ToString() == "1")
                    {
                        sibl.ShowMessage("E203");
                        return false;
                    }
                    else if (dtposition.Rows[0]["ClosePosition2"].ToString() == "2")
                    {
                        sibl.ShowMessage("E194");
                        return false;
                    }
                }


                if (!RequireCheck(new Control[] { ScStaff.TxtCode }))
                    return false;
                else
                {
                    mse.StaffCD = ScStaff.TxtCode.Text;
                    mse.ChangeDate = txtPaymentDate.Text;
                    DataTable dtstaff = new DataTable();
                    dtstaff = sibl.M_Staff_Select(mse);
                    if (dtstaff.Rows.Count == 0)
                    {
                        sibl.ShowMessage("101");
                        ScStaff.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        ScStaff.LabelText = dtstaff.Rows[0]["StaffName"].ToString();
                    }
                }
                if (string.IsNullOrWhiteSpace(cboPaymentType.SelectedText.ToString()))
                {
                    sibl.ShowMessage("102");
                    cboPaymentType.Focus();
                    return false;
                }
                mmpe.ID = "314";
                DataTable dtmulti = new DataTable();
                dtmulti = sibl.M_MultiPorpose_Select(mmpe);
                if (dtmulti.Rows.Count == 0)
                {
                    sibl.ShowMessage("128");
                    cboPaymentType.Focus();
                    return false;
                }
                else
                {
                    string name = dtmulti.Rows[0]["Char1"].ToString();

                }
                if (string.IsNullOrWhiteSpace(cboPaymentSourceAcc.SelectedText.ToString()))
                {
                    sibl.ShowMessage("102");
                    cboPaymentSourceAcc.Focus();
                    return false;
                }
                mke.ChangeDate = txtPaymentDate.Text;
                DataTable dtkouza = new DataTable();
                dtkouza = sibl.M_Kouza_SelectByDate(mke);
                if (dtkouza.Rows.Count == 0)
                {
                    sibl.ShowMessage("128");
                    cboPaymentSourceAcc.Focus();
                    return false;
                }
                else
                {
                    cboPaymentSourceAcc.SelectedValue = dtkouza.Rows[0][""].ToString();
                }
                if (string.IsNullOrWhiteSpace(txtBillSettleDate.Text))
                {
                    sibl.ShowMessage("E102");
                    txtBillSettleDate.Focus();
                    return false;
                }

            }

            foreach (DataGridViewRow row1 in dgvPayment.Rows)
            {
                if (Convert.ToBoolean(row1.Cells["colChk"].EditedFormattedValue) == true)
                {
                    if (string.IsNullOrWhiteSpace(row1.Cells["colPaymenttime"].ToString()))
                    {
                        sibl.ShowMessage("E102");
                        return false;
                    }
                    else
                    {
                        string payment = row1.Cells["colPaymenttime"].ToString();
                        string unpaid = row1.Cells["colUnpaidAmount"].ToString();
                        int result = payment.CompareTo(unpaid);
                        if (result > 0)
                        {
                            sibl.ShowMessage("E143");
                            return false;
                        }

                        if (Convert.ToInt32(payment) < 0)
                        {
                            sibl.ShowMessage("E143");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Show Data in DataArea
        /// </summary>
        private void DataDisplay()
        {
            txtDueDate1.Enabled = false;
            txtDueDate2.Enabled = false;
            ScPayee.Enabled = false;
            btnF10Show.Enabled = false;

            cboPaymentType.Enabled = false;
            cboPaymentSourceAcc.Enabled = false;
            txtBillSettleDate.Enabled = false;

            btnSelectAll.Enabled = false;
            btnReleaseAll.Enabled = false;
            dpe.PayNo = ScPaymentNum.TxtCode.Text;
            dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;
            DataTable dtPay1 = new DataTable();
            dtPay1 = sibl.D_Pay_Select1(dpe);
            if (dtPay1.Rows.Count > 0)
            {
                dgvPayment.DataSource = dtPay1;
                txtPaymentDate.Text = dtPay1.Rows[0]["PayDate"].ToString();
                ScStaff.TxtCode.Text = dtPay1.Rows[0]["StaffCD"].ToString();
                ScStaff.LabelText = dtPay1.Rows[0]["StaffName"].ToString();

                LabelDataBind();
                vendorCD = dtPay1.Rows[0]["PayeeCD"].ToString();
            }
        }

        /// <summary>
        /// to show total data with Label below gridview
        /// </summary>
        private void LabelDataBind()
        {
            int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0, sum5 = 0, sum6 = 0, sum7 = 0;
            for (int i = 0; i < dgvPayment.Rows.Count; ++i)
            {
                sum1 += Convert.ToInt32(dgvPayment.Rows[i].Cells[4].Value);
                sum2 += Convert.ToInt32(dgvPayment.Rows[i].Cells[5].Value);
                sum3 += Convert.ToInt32(dgvPayment.Rows[i].Cells[6].Value);
                sum4 += Convert.ToInt32(dgvPayment.Rows[i].Cells[7].Value);
                sum5 += Convert.ToInt32(dgvPayment.Rows[i].Cells[8].Value);
                sum6 += Convert.ToInt32(dgvPayment.Rows[i].Cells[10].Value);
                sum7 += Convert.ToInt32(dgvPayment.Rows[i].Cells[11].Value);

            }
            lblPayPlanGaku.Text = sum1.ToString("#,##0");
            lblPayConfirmGaku.Text = sum2.ToString("#,##0");
            lblPayGaku.Text = sum3.ToString("#,##0");
            lblTransferGaku.Text = sum4.ToString("#,##0");
            lblTransferFeeGaku.Text = sum5.ToString("#,##0");
            lblGakuTotal.Text = sum6.ToString("#,##0");
            lblPayPlan.Text = sum7.ToString("#,##0");
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row1 in dgvPayment.Rows)
            {
                if (Convert.ToBoolean(row1.Cells["colChk"].EditedFormattedValue) == false)
                {
                    row1.Cells["colChk"].Value = true;
                }
            }
        }

        private void btnReleaseAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row1 in dgvPayment.Rows)
            {
                if (Convert.ToBoolean(row1.Cells["colChk"].EditedFormattedValue) == true)
                {
                    row1.Cells["colChk"].Value = false;
                }
            }
        }

        private void btnF10Show_Click(object sender, EventArgs e)
        {
            F10();
        }

        #region KeyEvent
        private void FrmSiharaiNyuuryoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void ScPaymentNum_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                type = 2;
                if (ErrorCheck(11))
                {
                    DataDisplay();
                    //Search.Search_Payment sp = new Search.Search_Payment(dpe.LargePayNO, dpe.PayNo, vendorCD, txtPaymentDate.Text, "1");
                    //sp.ShowDialog();
                }
            }
        }

        private void ScPaymentProcessNum_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                type = 1;
                //if (ErrorCheck(10))
                //{
                //DataDisplay();
                //}
                ErrorCheck(10);
            }
        }



        #endregion

        private void ScPayee_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                F10();
            }
        }

        private void dgvPayment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
                foreach (DataGridViewRow row in dgvPayment.Rows)           
                {
                 
                    //dppe.PayPlanDate = row.Cells["colPaymentdueDate"].Selected;                
                    //dppe.PayeeCD = row.Cells["colPayeeCD"].Selected.ToString(); 
                    if(OperationMode == EOperationMode.INSERT)
                    {
                       mode = "1";
                    }
                    else
                    {
                        mode = "2"; 
                    }
                    FrmSiharaiNyuuryoku_2 f2 = new FrmSiharaiNyuuryoku_2(dppe.PayPlanDate,dppe.PayeeCD,mode);
                    f2.Show();
                }
            
        }
    }
}
