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

        int type = 0;

        public FrmSiharaiNyuuryoku()
        {
            InitializeComponent();
        }

        private void FrmSiharaiNyuuryoku_Load(object sender, EventArgs e)
        {
            InProgramID = "SiharaiNyuuryoku";

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            EnableDisable_Controls();

            mse.StaffCD = InOperatorCD;
            mse.ChangeDate = DateTime.Now.ToShortDateString();
            DataTable dtstaff = new DataTable();
            dtstaff = sibl.M_Staff_Select(mse);
            if (dtstaff.Rows.Count > 0)
            {
                ScStaff.LabelText = dtstaff.Rows[0]["StaffName"].ToString();
            }
            ScStaff.Code = InOperatorCD;
            SetRequireField();
            
            txtDueDate1.Focus();
            BindCombo();
        }

        private void EnableDisable_Controls()
        {
            ScPaymentProcessNum.Enabled = false;
            ScPaymentNum.Enabled = false;
            ScPaymentProcessNum.SearchEnable = false;
            ScPaymentNum.SearchEnable = false;
            txtPaymentDate.Enabled = false;
            cboPaymentSourceAcc.Enabled = false;
            cboPaymentType.Enabled = false;
            txtBillSettleDate.Enabled = false;

            btnReleaseAll.Enabled = false;
            btnSelectAll.Enabled = false;
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
                case 11:
                    F11();
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
                    Clear(panelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(panelDetail);
                    ScPaymentProcessNum.Enabled = false;
                    ScPaymentNum.Enabled = false;
                    txtDueDate1.Focus();
                    F9Visible = false;
                    F12Enable = true;
                    btnF11Show.Enabled = false;
                    F11Enable = false;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(panelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(panelDetail);
                    ScPaymentProcessNum.Enabled = false;
                    ScPaymentNum.Enabled = false;
                    ScPaymentProcessNum.SetFocus(1);
                    F12Enable = false;
                    btnF11Show.Enabled = F11Enable = true;
                    break;
            }
            ScPaymentProcessNum.SetFocus(1);
        }

        public void F11()
        {

        }

        public void F12()
        {

        }

        private bool ErrorCheck(int index)
        {
            if (index == 11)
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
                    if (!RequireCheck(new Control[] { ScPaymentProcessNum.TxtCode }))
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
                    }
                }

                if (type == 3)
                {
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
                            sibl.ShowMessage("101");
                            ScPayee.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            ScPayee.LabelText = dtvendor.Rows[0]["VendorName"].ToString();
                        }
                    }

                    if (!RequireCheck(new Control[] { txtDueDate2 }))
                        return false;

                    if(!RequireCheck(new Control[] { ScPayee.TxtCode}))
                    {
                        return false;
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
                    sibl.ShowMessage("101");
                    ScPaymentNum.SetFocus(1);
                    return false;
                }

                //店舗の締日チェック
                //店舗締マスターで判断
                M_StoreClose_Entity msce = new M_StoreClose_Entity();
                //mse.StoreCD = mStoreCD;
                //mse.FiscalYYYYMM = txtPaymentDate.Text.Replace("/", "").Substring(0, 6);
                //bool ret = tprg_Shukka_Bl.CheckStoreClose(mse);
                //if (!ret)
                //{
                //    txtPaymentDate.Focus();
                //    return false;
                //}


                if (!RequireCheck(new Control[] { ScStaff.TxtCode }))
                    return false;
                else
                {
                    mse.StaffCD = ScStaff.TxtCode.Text;
                    mse.ChangeDate = DateTime.Now.ToShortDateString();
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

        private void DataDisplay()
        {
            txtDueDate1.Enabled = false;
            txtDueDate2.Enabled = false;
            ScPayee.Enabled = false;
                   
            dpe.PayNo = ScPaymentNum.TxtCode.Text;
            dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;
            DataTable dtPay = new DataTable();
            dtPay = sibl.D_Pay_Select(dpe);
            if (dtPay.Rows.Count > 0)
            {
                ScPayee.TxtCode.Text = dtPay.Rows[0]["PayeeCD"].ToString();
                DataTable dtPayee = new DataTable();
                dpe.PayeeCD = ScPayee.TxtCode.Text;
                dpe.PayDate = DateTime.Now.ToShortDateString();
                dtPayee = sibl.M_Payee_Select(dpe);
                if (dtPayee.Rows.Count > 0)
                {
                    ScPayee.LabelText = dtPayee.Rows[0]["VendorName"].ToString();
                }

                btnF11Show.Enabled = false;
                txtPaymentDate.Text = dtPay.Rows[0]["PayDate"].ToString();
                ScStaff.TxtCode.Text = dtPay.Rows[0]["StaffCD"].ToString();

                mse.StaffCD = ScStaff.TxtCode.Text;
                mse.ChangeDate = dtPay.Rows[0]["PayDate"].ToString();
                DataTable dtstaff = new DataTable();
                dtstaff = sibl.M_Staff_Select(mse);
                if(dtstaff.Rows.Count > 0)
                {
                    ScStaff.LabelText = dtstaff.Rows[0]["StaffName"].ToString();
                }
                

                cboPaymentType.Enabled = false;
                cboPaymentSourceAcc.Enabled = false;
                txtBillSettleDate.Enabled = false;
                btnSelectAll.Enabled = false;
                btnReleaseAll.Enabled = false;

                dpe.PayNo = ScPaymentNum.TxtCode.Text;
                dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;
                DataTable dtPayDetail = new DataTable();
                dtPayDetail = sibl.D_PayDetail_Select(dpe);
                dgvPayment.DataSource = dtPayDetail;
                lblRequiredGaku.Text = dtPayDetail.Rows[0]["RequiredGaku"].ToString();
                lblPayConfirmGaku.Text = dtPayDetail.Rows[0]["PayConfirmGaku"].ToString();
                lblPayGaku.Text = dtPayDetail.Rows[0]["PayGaku"].ToString();
                lblTransferFeeGaku.Text = dtPayDetail.Rows[0]["TransferGaku"].ToString();
                lblTransferGaku.Text = dtPayDetail.Rows[0]["TransferFeeGaku"].ToString();
            }


        }

        private void ScPaymentNum_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                type = 2;
                if (ErrorCheck(11))
                {
                    DataDisplay();
                }
            }
        }

        private void btnF11Show_Click(object sender, EventArgs e)
        {
            type = 3;
            if (ErrorCheck(11))
            {
                //txtPaymentDate.Text = DateTime.Today.ToShortDateString();
                //ScStaff.TxtCode.Text = InOperatorCD;
                //mse.ChangeDate = DateTime.Now.ToShortDateString();
                //DataTable dtstaff = new DataTable();
                //dtstaff = sibl.M_Staff_Select(mse);
                //if (dtstaff.Rows.Count > 0)
                //{
                //    ScStaff.LabelText = dtstaff.Rows[0]["StaffName"].ToString();
                //}
                //dppe.PayPlanDateFrom = txtDueDate1.Text;
                //dppe.PayPlanDateTo = txtDueDate2.Text;
                //dppe.PayeeCD = ScPayee.TxtCode.Text;
                //mve.ChangeDate = txtPaymentDate.Text;
                //DataTable dtpayplan = new DataTable();
                //dtpayplan = sibl.D_PayPlan_Select(dppe, mve);
                mve.ChangeDate = txtPaymentDate.Text;
                dppe.PayPlanDateFrom = txtDueDate1.Text;
                dppe.PayPlanDateTo = txtDueDate2.Text;
               
            }
            


        }
    }
}
