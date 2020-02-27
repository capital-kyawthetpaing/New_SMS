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

        DataTable dtpayplan = new DataTable();

        DataTable dt2 = new DataTable(); DataTable dt3 = new DataTable();
        DataTable dt4 = new DataTable(); DataTable dt4Detail = new DataTable();

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

            BindCombo();
            SetRequireField();
           
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
                    F7();
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
                    ScPayee.Enabled = true;
                    ScPayee.SearchEnable = true;
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

        #region Function Click
        private void F7()
        {
            if(dgvPayment.CurrentRow.Index > -1)
            {
                DataGridViewRow row = dgvPayment.CurrentRow;
                //}
                //if (dgvPayment.SelectedRows.Count != 0)
                //{
                //DataGridViewRow row = this.dgvPayment.SelectedRows[0];
                dppe.PayPlanDate = row.Cells["colPaymentdueDate"].Value.ToString();
                dppe.PayeeCD = row.Cells["colPayeeCD"].Value.ToString();

                dpe.PayeeCD = row.Cells["colPayeeCD"].Value.ToString();
                dpe.PayPlanDate = row.Cells["colPaymentdueDate"].Value.ToString();
                dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;
                dpe.PayNo = ScPaymentNum.TxtCode.Text;
                mke.KouzaCD = cboPaymentSourceAcc.SelectedValue.ToString();
                             
                if (OperationMode == EOperationMode.INSERT)
                {
                    mode = "1";
                    //dt4 = sibl.D_Pay_SelectForPayPlanDate2(dppe);
                    if(dt4.Rows.Count > 0)
                    {
                        SiharaiNyuuryoku_2 f2 = new SiharaiNyuuryoku_2(mke.KouzaCD,dppe.PayeeCD, dppe.PayPlanDate, dt4, dt4Detail);
                        f2.ShowDialog();
                    }                  
                }
                else
                {
                    mode = "2";
                    dt2 = sibl.D_Pay_Select2(dpe);
                    dt3 = sibl.D_Pay_Select3(dpe);
                    if (dt3.Rows.Count > 0)
                    {
                        SiharaiNyuuryoku_2 f2 = new SiharaiNyuuryoku_2(mke.KouzaCD, dppe.PayeeCD, dppe.PayPlanDate, dt3, dt2);
                        f2.ShowDialog();
                    }                 
                }             
            }
        
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
                
                dtpayplan = sibl.D_Pay_SelectForPayPlanDate1(dppe);
                if (dtpayplan.Rows.Count > 0)
                {
                    string year = DateTime.Today.Year.ToString();
                    string month = DateTime.Today.Month.ToString();
                    if (month.Length == 1)
                    {
                        month = 0 + DateTime.Now.Month.ToString();
                    }
                    string day = DateTime.Today.Day.ToString();
                    txtPaymentDate.Text = year +"/"+month+"/"+day;
                    ScStaff.TxtCode.Text = InOperatorCD;
                    ScStaff.LabelText = dtpayplan.Rows[0]["StaffName"].ToString();
                    cboPaymentType.SelectedValue = "1";
                    cboPaymentSourceAcc.SelectedValue = dtpayplan.Rows[0]["KouzaCD"].ToString();
                    txtBillSettleDate.Text = string.Empty;
                    dgvPayment.DataSource = dtpayplan;
                    dgvPayment.Rows[0].Selected = true;
                    Checkstate(true);
                    
                    LabelDataBind();

                    if (dgvPayment.SelectedRows.Count != 0)
                    {
                        DataGridViewRow row = this.dgvPayment.SelectedRows[0];
                        dppe.PayPlanDate = row.Cells["colPaymentdueDate"].Value.ToString();
                        dppe.PayeeCD = row.Cells["colPayeeCD"].Value.ToString();
                        dt4 = sibl.D_Pay_SelectForPayPlanDate2(dppe);
                        if(dt4.Rows.Count > 0)
                        {
                            dt4Detail = dt4.Copy();
                            dt4Detail.Columns.Remove("Number");
                            dt4Detail.Columns.Remove("RecordedDate");
                            dt4Detail.Columns.Remove("PayPlanGaku");
                            dt4Detail.Columns.Remove("PayConfirmGaku");
                            dt4Detail.Columns.Remove("UnpaidAmount1");
                            dt4Detail.Columns.Remove("UnpaidAmount2");

                           
                            dt4.Columns.Remove("VendorName");
                            dt4.Columns.Remove("BankCD");
                            dt4.Columns.Remove("BankName");
                            dt4.Columns.Remove("BranchCD");
                            dt4.Columns.Remove("BranchName");
                            dt4.Columns.Remove("KouzaKBN");
                            dt4.Columns.Remove("KouzaNO");
                            dt4.Columns.Remove("KouzaMeigi");
                            dt4.Columns.Remove("FeeKBN");
                            dt4.Columns.Remove("Fee");
                            dt4.Columns.Remove("CashGaku");
                            dt4.Columns.Remove("OffsetGaku");
                            dt4.Columns.Remove("BillGaku");
                            dt4.Columns.Remove("BillDate");
                            dt4.Columns.Remove("BillNO");
                            dt4.Columns.Remove("ERMCGaku");
                            dt4.Columns.Remove("ERMCNO");
                            dt4.Columns.Remove("ERMCDate");
                            dt4.Columns.Remove("OtherGaku1");
                            dt4.Columns.Remove("Account1");
                            dt4.Columns.Remove("SubAccount1");
                            dt4.Columns.Remove("OtherGaku2");
                            dt4.Columns.Remove("Account2");
                            dt4.Columns.Remove("SubAccount2");

                        }
                        
                        
                    }                   
                }

                EnablePanel(PanelDetail);             
                btnSelectAll.Enabled = true;
                btnReleaseAll.Enabled = true;
                
            }
        }

        private void F12()
        {
            if (ErrorCheck(12))
            {
                dpe = GetPayData();
                if (bbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    switch (OperationMode)
                    {
                        case EOperationMode.INSERT:
                            Insert();
                            break;
                        case EOperationMode.UPDATE:
                            Update();
                            break;
                        case EOperationMode.DELETE:
                            Delete();
                            break;
                    }
                }
                else
                    PreviousCtrl.Focus();
            }
        }
        #endregion

        private void Checkstate(bool flag)
        {
            foreach (DataGridViewRow row1 in dgvPayment.Rows)
            {
                row1.Cells["colChk"].Value = flag;
            }
        }
       
        private D_Pay_Entity GetPayData()
        {
            dpe = new D_Pay_Entity()
            {
                StaffCD = ScStaff.TxtCode.Text,
                PayDate = txtPaymentDate.Text,
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID,
                dtTemp1 = dtpayplan
            };
            return dpe;
        }

        #region Process For F12
        private void Insert()
        {
            if (sibl.D_Pay_Insert(dpe))
            {
                Clear(PanelHeader);
                Clear(PanelDetail);
                txtDueDate1.Focus();

                sibl.ShowMessage("I101");
            }
        }

        private void Update()
        {

        }

        private void Delete()
        {

        }
        #endregion

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

            btnSelectAll.Enabled = true;
            btnReleaseAll.Enabled = true;
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
                Checkstate(true);
                dgvPayment.Rows[0].Selected = true;

                LabelDataBind();
                vendorCD = dtPay1.Rows[0]["PayeeCD"].ToString();
            }
            EnablePanel(PanelDetail);
          
        }

        /// <summary>
        /// to show total data with Label below gridview
        /// </summary>
        public void LabelDataBind()
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

        #region btnClick
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            Checkstate(true);   
        }

        private void btnReleaseAll_Click(object sender, EventArgs e)
        {
            //row1.Cells["colPaymenttime"].Value = 0; 
            //row1.Cells["colUnpaidAmount"].Value = Convert.ToInt32( row1.Cells["colScheduledPayment"].Value) - Convert.ToInt32( row1.Cells["colAmountPaid"].Value);
            Checkstate(false);
        }

        private void btnF10Show_Click(object sender, EventArgs e)
        {
            F10();
        }
        #endregion
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

        private void ScPayee_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                F10();
            }
        }
        #endregion
        
        private void dgvPayment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            F7();
        }

        private void cboPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboPaymentType.SelectedValue.ToString() == "1")
            //{
            //    foreach (DataGridViewRow row in dgvPayment.Rows)
            //    {
            //        row.Cells["colPaymenttime"].Value = Convert.ToInt32(row.Cells["colScheduledPayment"].Value) - Convert.ToInt32(row.Cells["colAmountPaid"].Value);
            //        row.Cells["colTransferAmount"].Value = Convert.ToInt32(row.Cells["colScheduledPayment"].Value) - Convert.ToInt32(row.Cells["colAmountPaid"].Value);
            //        row.Cells["colUnpaidAmount"].Value = "0";
            //        row.Cells["colOtherThanTransfer"].Value = "0";
            //    }
            //}
            //else
            //{
            //    foreach (DataGridViewRow row in dgvPayment.Rows)
            //    {
            //        row.Cells["colPaymenttime"].Value = Convert.ToInt32(row.Cells["colScheduledPayment"].Value) - Convert.ToInt32(row.Cells["colAmountPaid"].Value);
            //        row.Cells["colTransferAmount"].Value = "0";
            //        row.Cells["colTransferFee"].Value = "0";
            //        row.Cells["colUnpaidAmount"].Value = "0";
            //        row.Cells["colOtherThanTransfer"].Value = Convert.ToInt32(row.Cells["colScheduledPayment"].Value) - Convert.ToInt32(row.Cells["colAmountPaid"].Value);
            //    }
            //}
        }

        private void dgvPayment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((Convert.ToBoolean(dgvPayment.Rows[e.RowIndex].Cells["colChk"].EditedFormattedValue) == true))
            {
                if (cboPaymentType.SelectedValue.ToString() == "1")
                {
                    //foreach (DataGridViewRow row in dgvPayment.Rows)
                    //{
                    //    row.Cells["colPaymenttime"].Value = Convert.ToInt32(row.Cells["colScheduledPayment"].Value) - Convert.ToInt32(row.Cells["colAmountPaid"].Value);
                    //    row.Cells["colTransferAmount"].Value = Convert.ToInt32(row.Cells["colScheduledPayment"].Value) - Convert.ToInt32(row.Cells["colAmountPaid"].Value);
                    //    row.Cells["colUnpaidAmount"].Value = "0";
                    //    row.Cells["colOtherThanTransfer"].Value = "0";
                    //}
                    dgvPayment.Rows[e.RowIndex].Cells["colPaymenttime"].Value = Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colScheduledPayment"].Value) - Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colAmountPaid"].Value);
                    dgvPayment.Rows[e.RowIndex].Cells["colTransferAmount"].Value = Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colScheduledPayment"].Value) - Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colAmountPaid"].Value);
                    dgvPayment.Rows[e.RowIndex].Cells["colUnpaidAmount"].Value = "0";
                    dgvPayment.Rows[e.RowIndex].Cells["colOtherThanTransfer"].Value = "0";

                }
                else
                {
                    //foreach (DataGridViewRow row in dgvPayment.Rows)
                    //{
                    //    row.Cells["colPaymenttime"].Value = Convert.ToInt32(row.Cells["colScheduledPayment"].Value) - Convert.ToInt32(row.Cells["colAmountPaid"].Value);
                    //    row.Cells["colTransferAmount"].Value = "0";
                    //    row.Cells["colTransferFee"].Value = "0";
                    //    row.Cells["colUnpaidAmount"].Value = "0";
                    //    row.Cells["colOtherThanTransfer"].Value = Convert.ToInt32(row.Cells["colScheduledPayment"].Value) - Convert.ToInt32(row.Cells["colAmountPaid"].Value);
                    //}

                    dgvPayment.Rows[e.RowIndex].Cells["colPaymenttime"].Value = Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colScheduledPayment"].Value) - Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colAmountPaid"].Value);
                    dgvPayment.Rows[e.RowIndex].Cells["colTransferAmount"].Value = "0";
                    dgvPayment.Rows[e.RowIndex].Cells["colTransferFee"].Value = "0";
                    dgvPayment.Rows[e.RowIndex].Cells["colUnpaidAmount"].Value = "0";
                    dgvPayment.Rows[e.RowIndex].Cells["colOtherThanTransfer"].Value = Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colScheduledPayment"].Value) - Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colAmountPaid"].Value);
                }

                LabelDataBind();
            }
        }
        
    }
}
