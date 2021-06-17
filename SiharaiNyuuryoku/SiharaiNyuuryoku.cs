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
using System.Collections;

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
        Exclusive_BL e_bl = new Exclusive_BL();
        D_Exclusive_Entity de_e = new D_Exclusive_Entity(); 

        int type = 0; string mode = "0";
        string vendorCD = string.Empty;
        string Num = string.Empty;

        DataTable dtpayplan = new DataTable(); // data bind(insert mode) for Form1
        DataTable dtPay1 = new DataTable(); // data bind(update mode) for Form1
        DataTable dtSiharai2 = new DataTable(); // checkbox click for form2
        DataTable dt2 = new DataTable(); // detail for form2(update mode)
        DataTable dt3 = new DataTable(); // Gridview bind for form2(update mode)
        DataTable dt4 = new DataTable(); // gridview bind for form2(insert mode)
        DataTable dt4Detail = new DataTable(); // detail for form2(insert mode)
        DataTable dtdup = new DataTable(); //duplicate datatable

        public FrmSiharaiNyuuryoku()
        {
            InitializeComponent();
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

            Btn_F7.Enabled = false;
            Btn_F7.Text = "編集(F7)";
            //Btn_F10.Text = "";
            Btn_F11.Enabled = true;

            btnF11Show.Enabled = true;
            txtPaymentDate.Enabled = false;

            ScStaff.Code = InOperatorCD;
            ScStaff.ChangeDate = DateTime.Today.ToShortDateString();
            ScStaff.SelectData();

            //cboPaymentSourceAcc.Enabled = false;
            cboPaymentType.Enabled = false;
            txtBillSettleDate.Enabled = false;

            btnReleaseAll.Enabled = false;
            btnSelectAll.Enabled = false;

            txtDueDate1.Focus();

            BindCombo();
            SetRequireField();
            DeleteExclusive();
        }

        #region Funtion For FormLoad

        private void SetRequireField()
        {
            ScPaymentNum.TxtCode.Require(true);
            //ScPayee.TxtCode.Require(true);
            txtPaymentDate.Require(true);
            ScStaff.TxtCode.Require(true);
            txtDueDate2.Require(true);
            txtPaymentDate.Require(true);
        }

        private void BindCombo()
        {
            cboPaymentType.Bind(string.Empty);
            //cboPaymentSourceAcc.Bind(string.Empty);
        }

        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                    ChangeMode(EOperationMode.INSERT);
                    F12Visible = true;
                    break;
                case 3:
                    ChangeMode(EOperationMode.UPDATE);
                    DeleteExclusive();
                    F12Visible = true;
                    break;
                case 4:
                    ChangeMode(EOperationMode.DELETE);
                    DeleteExclusive();
                    F12Visible = true;
                    break;
                case 5:
                    ChangeMode(EOperationMode.SHOW);
                    F12Visible = false;
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeMode(OperationMode);
                        DeleteExclusive();
                        ScPaymentProcessNum.SetFocus(1);
                    }
                    else
                        PreviousCtrl.Focus();
                    break;
                case 7:
                    F7();
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    F12();
                    break;
            }
        }

        #endregion

        private void DeleteExclusive()
        {
            de_e = GetExclusiveData();
            e_bl.D_Exclusive_DeleteByKBN(de_e);
        }

        private D_Exclusive_Entity GetExclusiveData()
        {
            de_e = new D_Exclusive_Entity()
            {
                DataKBN = 9,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };
            return de_e;
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
                    F11Enable = true;
                    btnF11Show.Enabled = true;
                    //F11Visible = false;
                    Clear();
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    ScPaymentProcessNum.Enabled = true;
                    ScPaymentProcessNum.SearchEnable = true;
                    ScPaymentNum.Enabled = true;
                    ScPaymentNum.SearchEnable = true;
                    ScPaymentProcessNum.SetFocus(1);
                    txtDueDate1.Enabled = false;
                    txtDueDate2.Enabled = false;
                    ScPayee.Enabled = false;
                    ScPayee.SearchEnable = false;
                    F12Enable = true;
                    F11Enable = false;
                    btnF11Show.Enabled = F11Enable = false;
                    //F11Visible = false;
                    Clear();
                    break;
            }
            ScPaymentProcessNum.SetFocus(1);
        }
        
        #region Function Click

        protected override void EndSec()
        {
            DeleteExclusive();
            this.Close();
        }

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
                //mke.KouzaCD = cboPaymentSourceAcc.SelectedValue.ToString();
                             
                if (OperationMode == EOperationMode.INSERT)
                {
                    mode = "1";
                    //dt4 = sibl.D_Pay_SelectForPayPlanDate2(dppe);
                    if(dt4.Rows.Count > 0)
                    {
                        SiharaiNyuuryoku_2 f2 = new SiharaiNyuuryoku_2(mke.KouzaCD,dppe.PayeeCD, dppe.PayPlanDate, dt4, dt4Detail);
                        f2.ShowDialog();
                        dt4 = f2.dtGdv;
                        dt4Detail = f2.dtDetails;
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
                        dt3 = f2.dtGdv;
                        dt2 = f2.dtDetails;
                    }                 
                }             
            }       
        }
        
        private void F11()
        {
            type = 3;
            if (ErrorCheck(11))
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
                    if(day.Length == 1)
                    {
                        day = 0 + DateTime.Today.Day.ToString();
                    }
                    txtPaymentDate.Text = year +"/"+ month +"/"+day;
                    ScStaff.TxtCode.Text = InOperatorCD;
                    ScStaff.LabelText = dtpayplan.Rows[0]["StaffName"].ToString();
                    cboPaymentType.SelectedValue = 1;
                    if(cboPaymentType.SelectedValue == null)
                    {
                        cboPaymentType.SelectedValue = -1;
                    }
                    //cboPaymentSourceAcc.SelectedValue = dtpayplan.Rows[0]["KouzaCD"].ToString();
                    txtBillSettleDate.Text = string.Empty;
                    dtpayplan.Columns.Add("colCheck", typeof(bool)); foreach (DataRow dr in dtpayplan.Rows) dr["colCheck"] = true;  ///PTK Addded
                    dgvPayment.DataSource = dtpayplan;
                    dgvPayment.Rows[0].Selected = true;
                    Checkstate(true);
                    LabelDataBind();
                    if(dgvPayment.Rows.Count > 0)
                    {
                        Btn_F7.Enabled = true;
                    }

                    //if (dgvPayment.SelectedRows.Count != 0)
                    //{
                        //DataGridViewRow row = this.dgvPayment.SelectedRows[0];
                        //dppe.PayPlanDate = row.Cells["colPaymentdueDate"].Value.ToString();
                        //dppe.PayeeCD = row.Cells["colPayeeCD"].Value.ToString();
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
                            dt4Detail.Columns.Remove("PayPlanNO");


                            dt4.Columns.Remove("TransferGaku");
                            //dt4.Columns.Remove("TransferFeeGaku");
                            dt4.Columns.Remove("VendorName");
                            dt4.Columns.Remove("BankCD");
                            dt4.Columns.Remove("BankName");
                            dt4.Columns.Remove("BranchCD");
                            dt4.Columns.Remove("BranchName");
                            dt4.Columns.Remove("KouzaKBN");
                            dt4.Columns.Remove("KouzaNO");
                            dt4.Columns.Remove("KouzaMeigi");
                            dt4.Columns.Remove("FeeKBN");
                            dt4.Columns.Remove("KouzaCD");
                            dt4.Columns.Remove("KouzaName");
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
                            dt4.Columns.Remove("start1");
                            dt4.Columns.Remove("SubAccount1");
                            dt4.Columns.Remove("end1label");
                            dt4.Columns.Remove("OtherGaku2");
                            dt4.Columns.Remove("Account2");
                            dt4.Columns.Remove("start2");
                            dt4.Columns.Remove("SubAccount2");
                            dt4.Columns.Remove("end2label");


                            dtdup= new DataView(dt4).ToTable(false, "Number");

                            ArrayList UniqueRecords = new ArrayList();
                            ArrayList DuplicateRecords = new ArrayList();

                            foreach (DataRow dRow in dtdup.Rows)
                            {
                                if (UniqueRecords.Contains(dRow["Number"]))
                                    DuplicateRecords.Add(dRow);
                                else
                                    UniqueRecords.Add(dRow["Number"]);
                            }

                            foreach (DataRow dRow in DuplicateRecords)
                            {
                                dtdup.Rows.Remove(dRow);
                            }


                            for (int p = 0; p < dtdup.Rows.Count; p++)
                            {
                                Num = dtdup.Rows[p]["Number"].ToString();
                                if(!string.IsNullOrWhiteSpace(Num))
                                {                              
                                    de_e = new D_Exclusive_Entity()
                                    {
                                        DataKBN = 9,
                                        Number = Num,
                                        Program = this.InProgramID,
                                        Operator = this.InOperatorCD,
                                        PC = this.InPcID
                                    };

                                    e_bl.D_Exclusive_Insert(de_e);
                                }
                               
                            }
                        }
                    //}                   
                }

                else
                {
                    sibl.ShowMessage("E128");
                    ScPayee.SetFocus(1);
                }
                txtPaymentDate.ReadOnly = true;
                ScStaff.TxtCode.ReadOnly = true;
                ScStaff.SearchEnable = false;
                EnablePanel(PanelDetail);             
                btnSelectAll.Enabled = true;
                btnReleaseAll.Enabled = true;
                
            }
        }

        private void DeleteDuplicateRow ()
        {
            dtdup = dt4.Copy();

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
        
        #region Process For F12
        private void Insert()
        {
            dpe.ProcessMode = "新規";
            if (sibl.D_Pay_Insert(dpe))
            {
                for (int p = 0; p < dt4.Rows.Count; p++)
                {
                    Num = dt4.Rows[p]["Number"].ToString();
                    if (!string.IsNullOrWhiteSpace(Num))
                    {
                        de_e = new D_Exclusive_Entity()
                        {
                            DataKBN = 9,
                            Number = Num,
                            Program = this.InProgramID,
                            Operator = this.InOperatorCD,
                            PC = this.InPcID
                        };

                        e_bl.D_Exclusive_DeleteForSiharai(de_e);
                    }

                }
                Clear(PanelHeader);
                Clear(PanelDetail);
                txtDueDate1.Focus();
                Clear();

                sibl.ShowMessage("I101");
            }
        }

        private void Update()
        {
            dpe.ProcessMode = "修正";
            if (sibl.D_Pay_Update(dpe))
            {
                de_e = new D_Exclusive_Entity()
                {
                    DataKBN = 9,
                    Number = ScPaymentNum.TxtCode.Text,
                    Program = this.InProgramID,
                    Operator = this.InOperatorCD,
                    PC = this.InPcID
                };
                e_bl.D_Exclusive_DeleteForSiharai(de_e);

                Clear(PanelHeader);
                Clear(PanelDetail);
                txtDueDate1.Focus();
                Clear();

                sibl.ShowMessage("I101");
            }
        }

        private void Delete()
        {
            dpe.ProcessMode = "削除";
            if (sibl.D_Pay_Delete(dpe))
            {
                de_e = new D_Exclusive_Entity()
                {
                    DataKBN = 9,
                    Number = ScPaymentNum.TxtCode.Text,
                    Program = this.InProgramID,
                    Operator = this.InOperatorCD,
                    PC = this.InPcID
                };
                e_bl.D_Exclusive_DeleteForSiharai(de_e);

                Clear(PanelHeader);
                Clear(PanelDetail);
                txtDueDate1.Focus();
                Clear();

                sibl.ShowMessage("I101");
            }
        }
        #endregion

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
                ErrorCheck(11);
            }
        }

        private void ScPayee_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                F11();
            }
        }

        private void ScStaff_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {               
                if (!string.IsNullOrWhiteSpace(ScStaff.TxtCode.Text))
                {
                    mse.StaffCD = ScStaff.TxtCode.Text;
                    mse.ChangeDate = txtPaymentDate.Text;
                    DataTable dtstaff = new DataTable();
                    dtstaff = sibl.M_Staff_Select(mse);
                    if (dtstaff.Rows.Count == 0)
                    {
                        sibl.ShowMessage("E101");
                        ScStaff.SetFocus(1);
                    }
                    else
                    {
                        ScStaff.LabelText = dtstaff.Rows[0]["StaffName"].ToString();
                    }
                }
            }
        }

        private void btnF11Show_Click(object sender, EventArgs e)
        {
            F11();
        }

        #endregion

        #region GridView Cell Click
        private void dgvPayment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            F7();
        }

        private void dgvPayment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPayment.CurrentCell.RowIndex > 0)    // PTK  added  (Error Occurred if click on header)
            if ((Convert.ToBoolean(dgvPayment.Rows[e.RowIndex].Cells["colChk"].EditedFormattedValue) == true))
            {   
                if(!string.IsNullOrWhiteSpace(cboPaymentType.SelectedValue.ToString()))
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
                        dppe.PayPlanDate = dgvPayment.Rows[e.RowIndex].Cells["colPaymentdueDate"].Value.ToString();
                        dppe.PayeeCD = dgvPayment.Rows[e.RowIndex].Cells["colPayeeCD"].Value.ToString();

                        if (dt4Detail != null)
                        {
                            DataRow[] tblROWS1 = dt4Detail.Select("PayeeCD = '" + dppe.PayeeCD + "'" + "and PayPlanDate = '" + dppe.PayPlanDate + "'");
                            if (tblROWS1.Length > 0)
                                dtSiharai2 = tblROWS1.CopyToDataTable();

                            mke = new M_Kouza_Entity
                            {
                                //KouzaCD = cboPaymentSourceAcc.SelectedValue.ToString(),
                                BankCD = dtSiharai2.Rows[0]["BankCD"].ToString(),
                                BranchCD = dtSiharai2.Rows[0]["BranchCD"].ToString(),
                                Amount = lblPayGaku.Text.Replace(",", ""),
                            };
                            DataTable dt = sibl.M_Kouza_FeeSelect(mke);
                            dgvPayment.Rows[e.RowIndex].Cells["colTransferFee"].Value = dt.Rows[0]["Fee"].ToString();
                        }

                        dgvPayment.Rows[e.RowIndex].Cells["colPaymenttime"].Value = Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colScheduledPayment"].Value) - Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colAmountPaid"].Value);
                        dgvPayment.Rows[e.RowIndex].Cells["colTransferAmount"].Value = Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colScheduledPayment"].Value) - Convert.ToInt32(dgvPayment.Rows[e.RowIndex].Cells["colAmountPaid"].Value);

                        dgvPayment.Rows[e.RowIndex].Cells["colUnpaidAmount"].Value = "0";
                        dgvPayment.Rows[e.RowIndex].Cells["colOtherThanTransfer"].Value = "0";

                        if (dt4 != null)
                        {
                            for (int i = 0; i < dt4.Rows.Count; i++)
                            {
                                double a = Convert.ToDouble(dt4.Rows[i]["PayPlanGaku"].ToString());
                                double b = Convert.ToDouble(dt4.Rows[i]["PayConfirmGaku"].ToString());
                                double result = a - b;

                                dt4.Rows[i]["UnpaidAmount1"] = result.ToString();
                                dt4.Rows[i]["UnpaidAmount2"] = "0";
                            }
                        }

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
                        if (dt4 != null)
                        {
                            for (int i = 0; i < dt4.Rows.Count; i++)
                            {
                                dt4.Rows[i]["UnpaidAmount1"] = Convert.ToInt32(dt4.Rows[i]["PayPlanGaku"].ToString()) - Convert.ToInt32(dt4.Rows[i]["PayConfirmGaku"].ToString());
                                dt4.Rows[i]["UnpaidAmount2"] = "0";
                            }
                        }

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

        #endregion

        /// <summary>
        /// Error Check for the whole form
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool ErrorCheck(int index)
        {            
            if(index == 11)
            {
                if (type == 1)
                {
                    DataTable dtpay = new DataTable();
                    dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;
                    dtpay = sibl.D_Pay_LargePayNoSelect(dpe);
                    if (dtpay.Rows.Count == 0)
                    {
                        sibl.ShowMessage("E138");
                        ScPaymentProcessNum.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(dtpay.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            sibl.ShowMessage("E140");
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
                        sibl.ShowMessage("E138");
                        ScPaymentNum.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(dtpayno.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            sibl.ShowMessage("E140");
                            ScPaymentNum.SetFocus(1);
                            return false;
                        }
                        if (!string.IsNullOrWhiteSpace(dtpayno.Rows[0]["FBCreateDate"].ToString()))
                        {
                            sibl.ShowMessage("E144");
                            ScPaymentNum.SetFocus(1);
                            return false;
                        }
                        txtPaymentDate.Text = dtpayno.Rows[0]["PayDate"].ToString();
                        mfye.InputPossibleStartDate = txtPaymentDate.Text;
                        mfye.InputPossibleEndDate = txtPaymentDate.Text;
                        DataTable dtcontrol = new DataTable();
                        dtcontrol = sibl.M_Control_PaymentSelect(mfye);
                        if (dtcontrol.Rows.Count == 0)
                        {
                            sibl.ShowMessage("E115");
                            ScPaymentNum.SetFocus(1);
                            return false;
                        }
                        //else
                        //{
                        //    DataDisplay();
                        //}
                    }
                }

                else if (type == 3)
                {
                    if (!RequireCheck(new Control[] { txtDueDate2 }))
                        return false;
                    else
                    {
                        int result = txtDueDate1.Text.CompareTo(txtDueDate2.Text);
                        if (result > 0)
                        {
                            sibl.ShowMessage("E104");
                            txtDueDate2.Focus();
                        }
                    }

                    //if (!RequireCheck(new Control[] { ScPayee.TxtCode }))
                    //    return false;
                    //else
                    //if(!string.IsNullOrWhiteSpace(ScPayee.TxtCode.Text))
                    //{
                        mve.VendorCD = ScPayee.TxtCode.Text;
                        mve.ChangeDate = DateTime.Now.ToString("yyyy/MM/dd");
                        mve.MoneyPayeeFlg = "1";
                        DataTable dtvendor = new DataTable();
                        dtvendor = sibl.M_Vendor_Select(mve);
                        if (dtvendor.Rows.Count == 0)
                        {
                            sibl.ShowMessage("E101");
                            ScPayee.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            ScPayee.LabelText = dtvendor.Rows[0]["VendorName"].ToString();
                        }
                    //}


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
                    sibl.ShowMessage("E115");
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
                    //if (!ScStaff.SelectData())
                    if (dtstaff.Rows.Count == 0)
                    {
                        sibl.ShowMessage("E101");
                        ScStaff.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        ScStaff.LabelText = dtstaff.Rows[0]["StaffName"].ToString();
                    }
                }

                //if (!RequireCheck(new Control[] {cboPaymentType}))
                if (string.IsNullOrWhiteSpace(cboPaymentType.SelectedValue.ToString()))
                {
                    sibl.ShowMessage("E102");
                    cboPaymentType.Focus();
                    return false;
                }
                else
                {
                    mmpe.ID = "314";
                    DataTable dtmulti = new DataTable();
                    dtmulti = sibl.M_MultiPorpose_Select(mmpe);
                    if (dtmulti.Rows.Count == 0)
                    {
                        sibl.ShowMessage("E128");
                        cboPaymentType.Focus();
                        return false;
                    }
                    else
                    {
                        string name = dtmulti.Rows[0]["Char1"].ToString();
                    }
                }
               
                //if (string.IsNullOrWhiteSpace(cboPaymentSourceAcc.SelectedValue.ToString()))
                //{
                //    sibl.ShowMessage("E102");
                //    cboPaymentSourceAcc.Focus();
                //    return false;
                //}
                //mke.ChangeDate = txtPaymentDate.Text;
                //DataTable dtkouza = new DataTable();
                //dtkouza = sibl.M_Kouza_SelectByDate(mke);
                //if (dtkouza.Rows.Count == 0)
                //{
                //    sibl.ShowMessage("E128");
                //    cboPaymentSourceAcc.Focus();
                //    return false;
                //}
                //else
                //{
                //    cboPaymentSourceAcc.SelectedValue = dtkouza.Rows[0]["KouzaCD"].ToString();
                //}
                if (string.IsNullOrWhiteSpace(txtBillSettleDate.Text))
                {
                    sibl.ShowMessage("E102");
                    txtBillSettleDate.Focus();
                    return false;
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
            btnF11Show.Enabled = false;

            cboPaymentType.Enabled = false;
            //cboPaymentSourceAcc.Enabled = false;
            txtBillSettleDate.Enabled = false;

            btnSelectAll.Enabled = true;
            btnReleaseAll.Enabled = true;
            dpe.PayNo = ScPaymentNum.TxtCode.Text;
            dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;

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
                if (dgvPayment.Rows.Count > 0)
                {
                    Btn_F7.Enabled = true;
                    if (dgvPayment.CurrentRow.Index > -1)
                    {
                        DataGridViewRow row = dgvPayment.CurrentRow;
                        dpe.PayeeCD = row.Cells["colPayeeCD"].Value.ToString();
                        dpe.PayPlanDate = row.Cells["colPaymentdueDate"].Value.ToString();

                        dt2 = sibl.D_Pay_Select2(dpe);
                        dt3 = sibl.D_Pay_Select3(dpe);
                    }
                }    
                //vendorCD = dtPay1.Rows[0]["PayeeCD"].ToString();
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

        /// <summary>
        /// For Gridview CheckBox checked or not
        /// </summary>
        /// <param name="flag"></param>
        private void Checkstate(bool flag)
        {
            foreach (DataGridViewRow row1 in dgvPayment.Rows)
            {
                row1.Cells["colChk"].Value = flag;
            }
        }

        /// <summary>
        /// Get D_Pay_Entity
        /// </summary>
        /// <returns></returns>
        private D_Pay_Entity GetPayData()
        {
            dpe = new D_Pay_Entity()
            {
                StaffCD = ScStaff.TxtCode.Text,
                StoreCD = InOperatorCD,
                PayDate = txtPaymentDate.Text,
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                PayGakuTotol = lblPayGaku.Text.Replace(",",""),
                PC = InPcID,
                PayNo = ScPaymentNum.TxtCode.Text,
                LargePayNO  = ScPaymentProcessNum.TxtCode.Text,
                dtTemp1 = dtpayplan,
                dtTemp2 = dt4Detail,
                dtTemp3 = dt4,
                dtTemp4 = dtPay1,
                dtTemp5 = dt2,
                dtTemp6 = dt3
            };
            return dpe;
        }

        private void Clear()
        {
            lblPayPlan.Text = string.Empty;
            lblGakuTotal.Text = string.Empty;
            lblPayConfirmGaku.Text = string.Empty;
            lblPayGaku.Text = string.Empty;
            lblPayPlanGaku.Text = string.Empty;
            lblTransferFeeGaku.Text = string.Empty;
            lblTransferGaku.Text = string.Empty;
        }

        private void txtDueDate2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtDueDate2.Text))
                {
                    int result = txtDueDate1.Text.CompareTo(txtDueDate2.Text);
                    if (result > 0)
                    {
                        sibl.ShowMessage("E104");
                        txtDueDate2.Focus();
                    }
                }
            }
        }

       
    }
}
