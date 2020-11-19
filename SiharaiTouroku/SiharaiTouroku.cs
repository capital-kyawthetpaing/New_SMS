using System;
using System.Data;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;

namespace SiharaiTouroku
{
    public partial class FrmSiharaiTouroku : FrmMainForm
    {
        private const string ProID = "SiharaiTouroku";

        SiharaiTouroku_BL sibl = new SiharaiTouroku_BL();
        D_Pay_Entity dpe = new D_Pay_Entity();
        M_Staff_Entity mse = new M_Staff_Entity();
        M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();

        M_Vendor_Entity mve = new M_Vendor_Entity();
        D_PayPlan_Entity dppe = new D_PayPlan_Entity();

        private int type = 0;
        //private string mode = "0";
        private string mPaymentDate = string.Empty;

        DataTable dtPayplan = new DataTable(); // data bind(insert mode) for Form1
        DataTable dtPay1 = new DataTable(); // data bind(update mode) for Form1
        DataTable dtPay1Detail = new DataTable();

        DataTable dtSiharai2 = new DataTable(); // checkbox click for form2
        DataTable dt2 = new DataTable(); // detail for form2(update mode)
        DataTable dt3 = new DataTable(); // Gridview bind for form2(update mode)
        DataTable dt4 = new DataTable(); // gridview bind for form2(insert mode)
        DataTable dt4Detail = new DataTable(); // detail for form2(insert mode)

        private string mOldPayNo = "";    //排他処理のため使用
        private DataTable dtForUpdate;  //排他用   

        public FrmSiharaiTouroku()
        {
            InitializeComponent();
        }

        private void FrmSiharaiTouroku_Load(object sender, EventArgs e)
        {
            InProgramID = ProID;

            SetFunctionLabel(EProMode.INPUT);
            StartProgram();

            ScPaymentProcessNum.Enabled = false;
            ScPaymentNum.Enabled = false;
            ScPaymentProcessNum.SearchEnable = false;
            ScPaymentNum.SearchEnable = false;

            Btn_F7.Enabled = false;
            Btn_F7.Text = "編集(F7)";
            Btn_F8.Text = "";
            Btn_F10.Text = "";
            Btn_F11.Enabled = true;

            btnF11Show.Enabled = true;
            txtPaymentDate.Enabled = false;

            ScPayee.Value1 = "3";

            cboPaymentSourceAcc.Enabled = false;
            cboPaymentType.Enabled = false;
            txtBillSettleDate.Enabled = false;

            dgvPayment.ReadOnly = false;
            for (int i = 1; i < dgvPayment.Columns.Count; i++)
                dgvPayment.Columns[i].ReadOnly = true;

            txtDueDate1.Focus();

            BindCombo();
            SetRequireField();

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
            cboPaymentSourceAcc.Bind(string.Empty);
        }

        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            switch (Index)
            {
                case 0: // F1:終了
                    {
                        break;
                    }
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会
                    {
                        ChangeMode((EOperationMode)Index);

                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        ChangeMode(base.OperationMode);

                        break;
                    }
                case 6:
                    F7();
                    break;
                case 10:
                    F11();
                    break;
                case 11:
                    F12();
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive()
        {
            if (mOldPayNo == "" && dtPayplan == null && dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity
                    {
                        DataKBN = Convert.ToInt16(dr["kbn"]),
                        Number = dr["no"].ToString()
                    };

                    ebl.D_Exclusive_Delete(de);
                }
                dtForUpdate = new DataTable();
            }
            if (mOldPayNo != "")
            {
                D_Exclusive_Entity dee = new D_Exclusive_Entity
                {
                    DataKBN = (int)Exclusive_BL.DataKbn.Shiharai,
                    Number = mOldPayNo,
                };

                bool ret = ebl.D_Exclusive_Delete(dee);
            }
            if (dtPayplan != null)
            {
                foreach (DataRow row in dtPayplan.Rows)
                {
                    mOldPayNo = row["PayNO"].ToString();

                    D_Exclusive_Entity dee = new D_Exclusive_Entity
                    {
                        DataKBN = (int)Exclusive_BL.DataKbn.Shiharai,
                        Number = mOldPayNo,
                    };

                    bool ret = ebl.D_Exclusive_Delete(dee);
                }
            }
            mOldPayNo = "";
        }
        private bool SelectAndInsertExclusive(Exclusive_BL.DataKbn kbn, string No)
        {
            if (OperationMode == EOperationMode.SHOW)
                return true;

            if (string.IsNullOrWhiteSpace(No))
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)kbn,
                Number = No,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                ScPaymentNum.Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldPayNo = ScPaymentNum.TxtCode.Text;
                return ret;
            }
        }
        private bool SelectAndInsertExclusive(DataTable dtUpdate)
        {
            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT)
                return true;


            if (string.IsNullOrWhiteSpace(ScPaymentProcessNum.TxtCode.Text))
                return true;

            if (dtPayplan != null)
            {
                //排他Tableに該当番号が存在するとError
                //[D_Exclusive]
                Exclusive_BL ebl = new Exclusive_BL();

                foreach (DataRow row in dtPayplan.Rows)
                {
                    string PayNo = row["PayNO"].ToString();

                    D_Exclusive_Entity dee = new D_Exclusive_Entity
                    {
                        DataKBN = (int)Exclusive_BL.DataKbn.Shiharai,
                        Number = PayNo,
                        Program = this.InProgramID,
                        Operator = this.InOperatorCD,
                        PC = this.InPcID
                    };
                    DataTable dt = ebl.D_Exclusive_Select(dee);

                    if (dt.Rows.Count > 0)
                    {
                        bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                        ScPaymentProcessNum.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        bool ret = ebl.D_Exclusive_Insert(dee);
                    }
                }
            }
            return true;
        }
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;

            //排他処理を解除
            DeleteExclusive();

            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    Clear(PanelHeader);
                    EnablePanel(PanelHeader);
                    ClearDetail();
                    ScPaymentProcessNum.Enabled = false;
                    ScPaymentNum.Enabled = false;
                    ScPayee.Enabled = true;
                    ScPayee.SearchEnable = true;
                    txtDueDate1.Focus();
                    F9Visible = false;
                    F12Enable = false;
                    F11Enable = true;
                    btnF11Show.Enabled = true;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    EnablePanel(PanelHeader);
                    ClearDetail();
                    ScPaymentProcessNum.Enabled = true;
                    ScPaymentProcessNum.SearchEnable = true;
                    ScPaymentNum.Enabled = true;
                    ScPaymentNum.SearchEnable = true;
                    ScPaymentProcessNum.SetFocus(1);
                    txtDueDate1.Enabled = false;
                    txtDueDate2.Enabled = false;
                    ScPayee.Enabled = false;
                    ScPayee.SearchEnable = false;
                    F12Enable = false;
                    F11Enable = false;
                    btnF11Show.Enabled = F11Enable = false;
                    break;
            }
            ScPaymentProcessNum.SetFocus(1);
        }
        private void ClearDetail()
        {
            Clear(PanelDetail);
            DisablePanel(PanelDetail);
            Clear();
            btnReleaseAll.Enabled = false;
            btnSelectAll.Enabled = false;
        }
        #region Function Click

        protected override void EndSec()
        {
            try
            {
                DeleteExclusive();
            }
            catch (Exception ex)
            {
                //例外は無視する
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            this.Close();
        }

        private void F7()
        {
            if (ErrorCheck(12))
            {
                if (dgvPayment.CurrentRow.Index >= 0)
                {
                    DataGridViewRow row = dgvPayment.CurrentRow;

                    //DataRow[] rows = dtPay1Detail.Select("PayeeCD <> '" + row.Cells["colPayeeCD"].Value.ToString() + "' OR PayPlanDate <> '" + row.Cells["colPaymentdueDate"].Value.ToString() + "'");
                    //dt2 = dtPay1Detail.Copy();
                    //foreach (DataRow rw in rows)
                    //    dt2.Rows.Remove(rw);
                    ////dt2 = sibl.D_Pay_Select02(dpe);

                    dppe.PayPlanDate = row.Cells["colPaymentdueDate"].Value.ToString();
                    dppe.PayeeCD = row.Cells["colPayeeCD"].Value.ToString();

                    dpe.PayDate = txtPaymentDate.Text;
                    dpe.PayeeCD = row.Cells["colPayeeCD"].Value.ToString();
                    dpe.PayeeName = row.Cells["colVendorName"].Value.ToString();
                    dpe.PayPlanDate = row.Cells["colPaymentdueDate"].Value.ToString();
                    dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;
                    dpe.PayNo = ScPaymentNum.TxtCode.Text;
                    dpe.MotoKouzaCD = cboPaymentSourceAcc.SelectedValue.ToString();

                    //if (OperationMode == EOperationMode.INSERT)
                    //{
                    SiharaiTouroku_2 f2 = new SiharaiTouroku_2(dpe, dtPayplan, dtPay1Detail);
                    f2.ProID = InProgramID;
                    f2.ProName = made.ProgramName;
                    f2.Operator = InOperatorCD;
                    f2.OperationMode = this.OperationMode;

                    f2.ShowDialog();
                    if (!f2.flgCancel)
                    {
                        dtPayplan = f2.dtGdv;
                        dtPay1Detail = f2.dtDetails;

                        //支払総額がゼロの状態で確定した場合、第一画面の該当行のチェックを外す
                        if (bbl.Z_Set(row.Cells["colPaymenttime"].Value) == 0)
                        {
                            row.Cells["colChk"].Value = 0;
                        }
                        dgvPayment.Refresh();
                    }
                    //}
                    LabelDataBind();
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

                dtPayplan = sibl.D_PayPlan_Select(dppe);
                if (dtPayplan.Rows.Count == 0)
                {
                    bbl.ShowMessage("E128");
                    ClearDetail();
                    return;
                }
                else
                {
                    dtPay1Detail = sibl.D_PayPlan_SelectDetail(dppe);

                    //テーブル転送仕様Ｘに従って排他テーブルに追加（D_PayPlan.Number）
                    DeleteExclusive();

                    dtForUpdate = new DataTable();
                    dtForUpdate.Columns.Add("kbn", Type.GetType("System.String"));
                    dtForUpdate.Columns.Add("no", Type.GetType("System.String"));

                    bool ret;

                    //排他処理
                    foreach (DataRow row in dtPay1Detail.Rows)
                    {
                        if (mOldPayNo != row["Number"].ToString())
                        {
                            ret = SelectAndInsertExclusive(Exclusive_BL.DataKbn.Shiire, row["Number"].ToString());
                            if (!ret)
                                return;

                            mOldPayNo = row["Number"].ToString();

                            // データを追加
                            DataRow rowForUpdate;
                            rowForUpdate = dtForUpdate.NewRow();
                            rowForUpdate["kbn"] = (int)Exclusive_BL.DataKbn.Shiire;
                            rowForUpdate["no"] = mOldPayNo;
                            dtForUpdate.Rows.Add(rowForUpdate);
                        }
                    }

                    dgvPayment.DataSource = dtPayplan;

                    txtPaymentDate.Text = sibl.GetDate();
                    ScStaff.TxtCode.Text = InOperatorCD;
                    ScStaff.LabelText = dtPayplan.Rows[0]["StaffName"].ToString();
                    cboPaymentType.SelectedValue = 1;   //振込
                    cboPaymentSourceAcc.SelectedValue = dtPayplan.Rows[0]["KouzaCD"].ToString();
                    txtBillSettleDate.Text = string.Empty;

                    Checkstate(true);
                    dgvPayment.Rows[0].Selected = true;
                    LabelDataBind();

                    Btn_F7.Enabled = true;

                    EnablePanel(PanelDetail);
                    for (int i = 1; i < dgvPayment.Columns.Count; i++)
                        dgvPayment.Columns[i].ReadOnly = true;

                    btnSelectAll.Enabled = true;
                    btnReleaseAll.Enabled = true;
                    F12Enable = true;
                    txtPaymentDate.Focus();
                }
            }
        }

        private void F12()
        {
            if (ErrorCheck(12))
            {
                dpe = GetPayData();
                if (bbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    DataTable dt = GetGridEntity();
                    DataTable dtD = GetDetailGridEntity();

                    if (sibl.D_Siharai_Exec(dpe, dt,dtD, (short)OperationMode))
                    {
                        sibl.ShowMessage("I101");

                        //更新後画面クリア
                        ChangeMode(OperationMode);
                    }
                }
                else
                    PreviousCtrl.Focus();
            }
        }

        #endregion

        #region btnClick
        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                Checkstate(true);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnReleaseAll_Click(object sender, EventArgs e)
        {
            try
            {
                Checkstate(false);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        #endregion

        #region KeyEvent
        private void FrmSiharaiTouroku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void ScPaymentNum_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            //Enterキー押下時処理
            //Returnキーが押されているか調べる
            //AltかCtrlキーが押されている時は、本来の動作をさせる
            if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
            {
                type = 2;
                if (ErrorCheck(11))
                {
                    if (DataDisplay())
                    {
                        txtPaymentDate.Focus();
                        return;
                    }
                }

                ClearDetail();
            }
        }

        private void ScPaymentProcessNum_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            //Enterキー押下時処理
            //Returnキーが押されているか調べる
            //AltかCtrlキーが押されている時は、本来の動作をさせる
            if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
            {
                type = 1;
                if (ErrorCheck(11))
                {
                    if (DataDisplay())
                    {
                        txtPaymentDate.Focus();
                        return;
                    }
                }
                else
                {
                    ScPaymentProcessNum.SetFocus(1);
                }
               
            }
        }

        private void ScPayee_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    F11();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void ScStaff_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckStaff())
                    {
                        ScStaff.SetFocus(1);
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnF11Show_Click(object sender, EventArgs e)
        {
            try
            {
                F11();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        #endregion

        #region GridView Cell Click
        private void dgvPayment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                F7();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void dgvPayment_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvPayment_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {

        }
        private void dgvPayment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvPayment.Rows.Count == 0 || e.ColumnIndex > 0)
                    return;

                if (e.ColumnIndex == 0)
                {
                    //ONにした明細に対して、シート「金種別セット内容」に従って、項目をセット
                    if ((Convert.ToBoolean(dgvPayment.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue) == true))
                    {
                        CheckClick(true, e.RowIndex);
                    }
                    else
                    {
                        CheckClick(false, e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        protected void CheckClick(bool check, int rowIndex)
        {
            //ONにした明細に対して、シート「金種別セット内容」に従って、項目をセット
            if (check)
            {
                if (cboPaymentType.SelectedIndex > 0)
                {
                    dppe.PayPlanDate = dgvPayment.Rows[rowIndex].Cells["colPaymentdueDate"].Value.ToString();
                    dppe.PayeeCD = dgvPayment.Rows[rowIndex].Cells["colPayeeCD"].Value.ToString();

                    //振込の場合　手数料を算出
                    if (cboPaymentType.SelectedValue.ToString() == "1")
                    {
                        if (dtPayplan != null)
                        {
                            DataRow[] tblROWS1 = dtPayplan.Select("PayeeCD = '" + dppe.PayeeCD + "'" + "and PayPlanDate = '" + dppe.PayPlanDate + "'");
                            if (tblROWS1.Length > 0)
                                dtSiharai2 = tblROWS1.CopyToDataTable();

                            M_Kouza_Entity mke = new M_Kouza_Entity
                            {
                                KouzaCD = dtSiharai2.Rows[0]["KouzaCD"].ToString(), // cboPaymentSourceAcc.SelectedValue.ToString(),
                                BankCD = dtSiharai2.Rows[0]["BankCD"].ToString(),
                                BranchCD = dtSiharai2.Rows[0]["BranchCD"].ToString(),
                                Amount = dtSiharai2.Rows[0]["TransferGaku"].ToString(),
                                ChangeDate = txtPaymentDate.Text,
                            };
                            DataTable dt = sibl.M_Kouza_FeeSelect(mke);
                            if (dt.Rows.Count > 0)
                                dgvPayment.Rows[rowIndex].Cells["colTransferFee"].Value = dt.Rows[0]["Fee"].ToString();
                        }

                        dgvPayment.Rows[rowIndex].Cells["colPaymenttime"].Value = bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colScheduledPayment"].Value) - bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colAmountPaid"].Value);
                        dgvPayment.Rows[rowIndex].Cells["colTransferAmount"].Value = bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colScheduledPayment"].Value) - bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colAmountPaid"].Value);

                        dgvPayment.Rows[rowIndex].Cells["colUnpaidAmount"].Value = "0";
                        dgvPayment.Rows[rowIndex].Cells["colOtherThanTransfer"].Value = "0";

                    }
                    else
                    {
                        dgvPayment.Rows[rowIndex].Cells["colPaymenttime"].Value = bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colScheduledPayment"].Value) - bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colAmountPaid"].Value);
                        dgvPayment.Rows[rowIndex].Cells["colTransferAmount"].Value = "0";
                        dgvPayment.Rows[rowIndex].Cells["colTransferFee"].Value = "0";
                        dgvPayment.Rows[rowIndex].Cells["colUnpaidAmount"].Value = "0";
                        dgvPayment.Rows[rowIndex].Cells["colOtherThanTransfer"].Value = bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colScheduledPayment"].Value) - bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colAmountPaid"].Value);
                    }

                    if (dtPay1Detail != null && dtPay1Detail.Rows.Count > 0)
                    {

                        DataRow[] tblROWS1 = dtPay1Detail.Select("PayeeCD = '" + dppe.PayeeCD + "'" + "and PayPlanDate = '" + dppe.PayPlanDate + "'");
                        foreach (DataRow row in tblROWS1)
                        {
                            row["Chk"] = 1;
                            row["UnpaidAmount1"] = bbl.Z_SetStr(bbl.Z_Set(row["PayPlanGaku"]) - bbl.Z_Set(row["PayConfirmGaku"]));
                            row["UnpaidAmount2"] = "0";
                        }
                    }
                }
            }
            else
            {
                //OFFにした明細に対して、今回支払額＝0、未支払額＝支払予定額-支払済額とし、第二画面の全入力項目をクリア
                dgvPayment.Rows[rowIndex].Cells["colPaymenttime"].Value = "0";
                dgvPayment.Rows[rowIndex].Cells["colTransferAmount"].Value = "0";  //振込額
                dgvPayment.Rows[rowIndex].Cells["colTransferFee"].Value = "0";  //振込手数料
                dgvPayment.Rows[rowIndex].Cells["colOtherThanTransfer"].Value = "0";  //振込以外
                dgvPayment.Rows[rowIndex].Cells["colUnpaidAmount"].Value = bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colScheduledPayment"].Value) - bbl.Z_Set(dgvPayment.Rows[rowIndex].Cells["colAmountPaid"].Value);

                if (dtPay1Detail != null)
                {
                    dppe.PayPlanDate = dgvPayment.Rows[rowIndex].Cells["colPaymentdueDate"].Value.ToString();
                    dppe.PayeeCD = dgvPayment.Rows[rowIndex].Cells["colPayeeCD"].Value.ToString();

                    DataRow[] tblROWS1 = dtPay1Detail.Select("PayeeCD = '" + dppe.PayeeCD + "'" + "and PayPlanDate = '" + dppe.PayPlanDate + "'");
                    foreach (DataRow row in tblROWS1)
                    {
                        row["Chk"] = 0;
                        row["UnpaidAmount1"] = "0";
                        row["UnpaidAmount2"] = "0";
                    }
                }
            }

            LabelDataBind();
        }
        #endregion

        /// <summary>
        /// Error Check for the whole form
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                if (type == 1 || type == 2)
                {
                    if (type == 1)
                    {
                        //入力無くても良い(It is not necessary to input)
                        if (string.IsNullOrWhiteSpace(ScPaymentProcessNum.TxtCode.Text))
                        {
                            return true;
                        }
                    }

                    else if (type == 2)
                    {
                        //支払処理番号未入力時、入力必須(Entry required)
                        if (!string.IsNullOrWhiteSpace(ScPaymentProcessNum.TxtCode.Text))
                        {
                            return true;
                        }

                        if (!RequireCheck(new Control[] { ScPaymentNum.TxtCode }))
                            return false;
                    }

                    DeleteExclusive();

                    dpe.PayNo = ScPaymentNum.TxtCode.Text;
                    dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;
                    //dtpayno = sibl.D_Pay_PayNoSelect(dpe);
                    dtPayplan = sibl.D_Pay_Select01(dpe);
                    if (dtPayplan.Rows.Count == 0)//dtPay1
                    {
                        bbl.ShowMessage("E138", "支払番号");
                        ScPaymentNum.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(dtPayplan.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            sibl.ShowMessage("E140", "支払番号");
                            ScPaymentNum.SetFocus(1);
                            return false;
                        }
                        if (!string.IsNullOrWhiteSpace(dtPayplan.Rows[0]["FBCreateDate"].ToString()))
                        {
                            sibl.ShowMessage("E144");
                            ScPaymentNum.SetFocus(1);
                            return false;
                        }
                        //入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(dtPayplan.Rows[0]["PayDate"].ToString()))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            return false;
                        }
                        bool ret;

                        if (OperationMode == EOperationMode.DELETE)
                        {
                            //店舗の締日チェック
                            //店舗締マスターで判断
                            M_StoreClose_Entity msce = new M_StoreClose_Entity
                            {
                                StoreCD = StoreCD,
                                FiscalYYYYMM = dtPayplan.Rows[0]["PayDate"].ToString().Replace("/", "").Substring(0, 6)
                            };
                            ret = bbl.CheckStoreClose(msce, false, false, false, true, false);
                            if (!ret)
                            {
                                return false;
                            }
                        }
                        //排他処理
                        ret = SelectAndInsertExclusive(Exclusive_BL.DataKbn.Shiharai, ScPaymentNum.TxtCode.Text);
                        if (!ret)
                            return false;

                        ret = SelectAndInsertExclusive(dtPayplan);
                        if (!ret)
                        {
                            dtPayplan = null;
                            return false;
                        }
                        dtPay1Detail = sibl.D_Pay_Select02(dpe);
                    }

                }
                else if (type == 3)
                {
                    if(!string.IsNullOrWhiteSpace(txtDueDate1.Text))
                        txtDueDate1.Text = bbl.FormatDate(txtDueDate1.Text);

                    if (!CheckDate2())
                    {
                        return false;
                    }

                    if (!CheckVendor())
                    {
                        return false;
                    }
                }
            }
            else if (index == 12)
            {
                if (OperationMode == EOperationMode.SHOW)
                    return true;

                if (!CheckPaymentDate())
                    return false;

                if (!CheckStaff())
                    return false;

                if (OperationMode == EOperationMode.INSERT)
                {
                    if (!RequireCheck(new Control[] { cboPaymentType }))
                        return false;

                    if (!CheckBillSettleDate())
                        return false;

                    //if (!RequireCheck(new Control[] { cboPaymentSourceAcc }))
                    //    return false;
                }
            }

            return true;
        }

        private bool CheckStaff()
        {
            ScStaff.LabelText = "";

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
                    sibl.ShowMessage("E101");
                    ScStaff.SetFocus(1);
                }
                else
                {
                    ScStaff.LabelText = dtstaff.Rows[0]["StaffName"].ToString();
                }
            }

            return true;
        }
        private bool CheckVendor()
        {
            ScPayee.LabelText = "";

            if (string.IsNullOrWhiteSpace(ScPayee.TxtCode.Text))
                return true;
            else
            {
                mve.VendorCD = ScPayee.TxtCode.Text;
                mve.ChangeDate = sibl.GetDate();
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
            }
            return true;
        }
        private bool CheckDate2()
        {
            if (!RequireCheck(new Control[] { txtDueDate2 }))
                return false;
            else
            {
                txtDueDate2.Text = bbl.FormatDate(txtDueDate2.Text);

                int result = txtDueDate1.Text.CompareTo(txtDueDate2.Text);
                if (result > 0)
                {
                    sibl.ShowMessage("E104");
                    txtDueDate2.Focus();
                    return false;
                }
            }

            return true;
        }
        private bool CheckBillSettleDate()
        {
            //支払金種＝手形の場合、入力必須(Entry required)
            if (cboPaymentType.SelectedIndex > 0 && cboPaymentType.SelectedValue.ToString().Equals("3"))
            {
                if (!RequireCheck(new Control[] { txtBillSettleDate }))
                    return false;

            }

            return true;
        }

        private bool CheckPaymentDate()
        {
            if (!RequireCheck(new Control[] { txtPaymentDate }))
                return false;

            //入力できる範囲内の日付であること
            if (!bbl.CheckInputPossibleDate(txtPaymentDate.Text))
            {
                //Ｅ１１５
                bbl.ShowMessage("E115");
                txtPaymentDate.Focus();
                return false;
            }

            //店舗の締日チェック
            //店舗締マスターで判断
            M_StoreClose_Entity msce = new M_StoreClose_Entity
            {
                StoreCD = StoreCD,
                FiscalYYYYMM = txtPaymentDate.Text.Replace("/", "").Substring(0, 6)
            };
            bool ret = bbl.CheckStoreClose(msce, false, false, false, true, false);
            if (!ret)
            {
                txtPaymentDate.Focus();
                return false;
            }

            //
            if (mPaymentDate != txtPaymentDate.Text)
            {
                if (!CheckVendor())
                {
                    return false;
                }

                if (!CheckStaff())
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Show Data in DataArea
        /// </summary>
        private bool DataDisplay()
        {

            //dpe.PayNo = ScPaymentNum.TxtCode.Text;
            //dpe.LargePayNO = ScPaymentProcessNum.TxtCode.Text;

            //dtPay1 = sibl.D_Pay_Select01(dpe);
            if (dtPayplan.Rows.Count > 0)//dtPay1
            {
                dgvPayment.DataSource = dtPayplan;// dtPay1;

                txtPaymentDate.Text = dtPayplan.Rows[0]["PayDate"].ToString();
                ScStaff.TxtCode.Text = dtPayplan.Rows[0]["StaffCD"].ToString();
                ScStaff.LabelText = dtPayplan.Rows[0]["StaffName"].ToString();

                Checkstate(true);
                dgvPayment.Rows[0].Selected = true;
                LabelDataBind();

                Btn_F7.Enabled = true;

                if (OperationMode == EOperationMode.SHOW)
                {
                    dgvPayment.Enabled = true;
                    for (int i = 0; i < dgvPayment.Columns.Count; i++)
                        dgvPayment.Columns[i].ReadOnly = true;
                    dgvPayment.Focus();
                }
                else
                {
                    EnablePanel(PanelDetail);
                    for (int i = 1; i < dgvPayment.Columns.Count; i++)
                        dgvPayment.Columns[i].ReadOnly = true;

                    btnSelectAll.Enabled = false;
                    btnReleaseAll.Enabled = false;
                    cboPaymentType.Enabled = false;
                    cboPaymentSourceAcc.Enabled = false;
                    txtBillSettleDate.Enabled = false;

                    F12Enable = true;
                    DisablePanel(PanelHeader);
                }

            }
            return true;
        }

        /// <summary>
        /// to show total data with Label below gridview
        /// </summary>
        public void LabelDataBind()
        {
            decimal sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0, sum5 = 0, sum6 = 0, sum7 = 0;
            for (int i = 0; i < dgvPayment.Rows.Count; ++i)
            {
                sum1 +=  bbl.Z_Set(dgvPayment.Rows[i].Cells[4].Value);
                sum2 +=  bbl.Z_Set(dgvPayment.Rows[i].Cells[5].Value);
                sum3 +=  bbl.Z_Set(dgvPayment.Rows[i].Cells[6].Value);
                sum4 +=  bbl.Z_Set(dgvPayment.Rows[i].Cells[7].Value);
                sum5 +=  bbl.Z_Set(dgvPayment.Rows[i].Cells[8].Value);
                sum6 +=  bbl.Z_Set(dgvPayment.Rows[i].Cells[10].Value);
                sum7 +=  bbl.Z_Set(dgvPayment.Rows[i].Cells[11].Value);

            }
            lblPayPlanGaku.Text = sum1.ToString("#,##0");
            lblPayConfirmGaku.Text = sum2.ToString("#,##0");
            lblPayGaku.Text = sum3.ToString("#,##0");
            lblTransferGaku.Text = sum4.ToString("#,##0");
            lblTransferFeeGaku.Text = sum5.ToString("#,##0");
            lblGakuTotal.Text = sum6.ToString("#,##0");
            lblPayPlan.Text = sum7.ToString("#,##0");
        }

        private void CboPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cboPaymentType.SelectedIndex > 0)
                {
                    
                    string type = cboPaymentType.SelectedValue.ToString();

                    if (dgvPayment.Rows.Count > 0)
                    {
                        for (int rowIndex = 0; rowIndex < dgvPayment.Rows.Count; rowIndex++)
                        {
                            dppe.PayPlanDate = dgvPayment.Rows[rowIndex].Cells["colPaymentdueDate"].Value.ToString();
                            dppe.PayeeCD = dgvPayment.Rows[rowIndex].Cells["colPayeeCD"].Value.ToString();

                            if (dtPayplan != null && dtPayplan.Rows.Count > 0)
                            {
                                DataRow[] tblROWS1 = dtPayplan.Select("PayeeCD = '" + dppe.PayeeCD + "'" + "and PayPlanDate = '" + dppe.PayPlanDate + "'");
                                foreach (DataRow row in tblROWS1)
                                {
                                    //row["Chk"] = 1;

                                    row["CashGaku"] = 0;
                                    row["OffsetGaku"] = 0;
                                    row["BillGaku"] = 0;
                                    row["BillDate"] = "";
                                    row["BillNO"] = "";
                                    row["ERMCGaku"] = 0;
                                    row["ERMCNO"] = "";
                                    row["ERMCDate"] = "";
                                    row["OtherGaku1"] = 0;
                                    row["Account1"] = "";
                                    row["start1"] = "";
                                    row["SubAccount1"] = "";
                                    row["end1label"] = "";
                                    row["OtherGaku2"] = 0;
                                    row["Account2"] = "";
                                    row["start2"] = "";
                                    row["SubAccount2"] = "";
                                    row["end2label"] = "";

                                    switch (type)
                                    {
                                        case "1"://振込の場合
                                            //振込額
                                            row["TransferGaku"] = bbl.Z_Set(row["PayPlanGaku"]) - bbl.Z_Set(row["PayConfirmGaku"]); //支払予定額-支払済額									
                                            break;

                                        case "2":
                                        case "3":
                                        case "4":
                                        case "5":
                                        case "6":
                                        case "7":
                                            row["TransferGaku"] = 0;
                                            row["BankCD"] = "";
                                            row["BankName"] = "";
                                            row["BranchCD"] = "";
                                            row["BranchName"] = "";
                                            row["KouzaKBN"] = DBNull.Value;
                                            row["KouzaNO"] = "";
                                            row["KouzaMeigi"] = "";
                                            row["FeeKBNVal"] = DBNull.Value;
                                            row["TransferFeeGaku"] =0;

                                            //現金
                                            if (type.Equals("2"))
                                            {
                                                row["CashGaku"] = row["PayGaku"];
                                            }
                                            //手形
                                            else if (type.Equals("3"))
                                            {
                                                row["BillGaku"] = row["PayGaku"];
                                                row["BillDate"] = txtBillSettleDate.Text;
                                            }
                                            //相殺
                                            else if (type.Equals("4"))
                                            {
                                                row["OffsetGaku"] = row["PayGaku"];
                                            }
                                            //電債
                                            else if (type.Equals("5"))
                                            {
                                                row["ERMCGaku"] = row["PayGaku"];
                                                row["ERMCDate"] = txtBillSettleDate.Text;
                                            }
                                            //その他１
                                            else if (type.Equals("6"))
                                            {
                                                row["OtherGaku1"] = row["PayGaku"];
                                            }
                                            //その他１
                                            else if (type.Equals("7"))
                                            {
                                                row["OtherGaku2"] = row["PayGaku"];
                                            }
                                            break;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }         
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
                CheckClick(flag, row1.Index);
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
                PayDate = txtPaymentDate.Text,
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                PayGakuTotol = lblPayGaku.Text.Replace(",", ""),
                PC = InPcID,
                PayNo = ScPaymentNum.TxtCode.Text,
                LargePayNO = ScPaymentProcessNum.TxtCode.Text,
            };
            return dpe;
        }

        private void Clear()
        {
            mPaymentDate = "";

            lblPayPlan.Text = string.Empty;
            lblGakuTotal.Text = string.Empty;
            lblPayConfirmGaku.Text = string.Empty;
            lblPayGaku.Text = string.Empty;
            lblPayPlanGaku.Text = string.Empty;
            lblTransferFeeGaku.Text = string.Empty;
            lblTransferGaku.Text = string.Empty;
        }


        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("Rows", typeof(int));
            dt.Columns.Add("PayNO", typeof(string));
            dt.Columns.Add("PayeeCD", typeof(string));
            dt.Columns.Add("PayPlanDate", typeof(DateTime));
            dt.Columns.Add("HontaiGaku8", typeof(decimal));
            dt.Columns.Add("HontaiGaku10", typeof(decimal));
            dt.Columns.Add("TaxGaku8", typeof(decimal));
            dt.Columns.Add("TaxGaku10", typeof(decimal));
            dt.Columns.Add("PayGaku", typeof(decimal));
            dt.Columns.Add("NotPaidGaku", typeof(decimal));
            dt.Columns.Add("TransferGaku", typeof(decimal));
            dt.Columns.Add("TransferFeeGaku", typeof(decimal));
            dt.Columns.Add("FeeKBN", typeof(int));
            dt.Columns.Add("MotoKouzaCD", typeof(string));
            dt.Columns.Add("BankCD", typeof(string));
            dt.Columns.Add("BranchCD", typeof(string));
            dt.Columns.Add("KouzaKBN", typeof(int));
            dt.Columns.Add("KouzaNO", typeof(string));
            dt.Columns.Add("KouzaMeigi", typeof(string));
            dt.Columns.Add("CashGaku", typeof(decimal));
            dt.Columns.Add("BillGaku", typeof(decimal));
            dt.Columns.Add("BillDate", typeof(DateTime));
            dt.Columns.Add("BillNO", typeof(string));
            dt.Columns.Add("ERMCGaku", typeof(decimal));
            dt.Columns.Add("ERMCDate", typeof(DateTime));
            dt.Columns.Add("ERMCNO", typeof(string));
            dt.Columns.Add("CardGaku", typeof(decimal));
            dt.Columns.Add("OffsetGaku", typeof(decimal));
            dt.Columns.Add("OtherGaku1", typeof(decimal));
            dt.Columns.Add("Account1", typeof(string));
            dt.Columns.Add("SubAccount1", typeof(string));
            dt.Columns.Add("OtherGaku2", typeof(decimal));
            dt.Columns.Add("Account2", typeof(string));
            dt.Columns.Add("SubAccount2", typeof(string));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }
        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rows = 0;
            foreach (DataRow row in dtPayplan.Rows)
            {
                rows++;

                if (bbl.Z_Set(row["PayGaku"]) > 0)
                    dt.Rows.Add(rows
                        , row["PayNO"]
                        , row["PayeeCD"]
                        , row["PayPlanDate"]
                        , bbl.Z_Set(row["HontaiGaku8"])
                        , bbl.Z_Set(row["HontaiGaku10"])
                        , bbl.Z_Set(row["TaxGaku8"])
                        , bbl.Z_Set(row["TaxGaku10"])
                        , bbl.Z_Set(row["PayGaku"])
                        , bbl.Z_Set(row["PayPlan"])     //NotPaidGaku
                        , bbl.Z_Set(row["TransferGaku"])
                        , bbl.Z_Set(row["TransferFeeGaku"])
                        , string.IsNullOrWhiteSpace(row["FeeKBNVal"].ToString()) ? null : row["FeeKBNVal"]
                        , string.IsNullOrWhiteSpace(row["KouzaCD"].ToString()) ? null : row["KouzaCD"]    //cboPaymentSourceAcc.SelectedIndex>0 ? cboPaymentSourceAcc.SelectedValue.ToString():""
                        , string.IsNullOrWhiteSpace(row["BankCD"].ToString()) ? null : row["BankCD"]
                        , string.IsNullOrWhiteSpace(row["BranchCD"].ToString()) ? null : row["BranchCD"]
                        , string.IsNullOrWhiteSpace(row["KouzaKBN"].ToString()) ? null : row["KouzaKBN"]
                        , string.IsNullOrWhiteSpace(row["KouzaNO"].ToString()) ? null : row["KouzaNO"]
                        , string.IsNullOrWhiteSpace(row["KouzaMeigi"].ToString()) ? null : row["KouzaMeigi"]
                        , bbl.Z_Set(row["CashGaku"])
                        , bbl.Z_Set(row["BillGaku"])
                        , string.IsNullOrWhiteSpace(row["BillDate"].ToString()) ? null : row["BillDate"]
                        , string.IsNullOrWhiteSpace(row["BillNO"].ToString()) ? null : row["BillNO"]
                        , bbl.Z_Set(row["ERMCGaku"])
                        , string.IsNullOrWhiteSpace(row["ERMCDate"].ToString()) ? null : row["ERMCDate"]
                        , string.IsNullOrWhiteSpace(row["ERMCNO"].ToString()) ? null : row["ERMCNO"]
                        , 0    //CardGaku
                        , bbl.Z_Set(row["OffsetGaku"])
                        , bbl.Z_Set(row["OtherGaku1"])
                        , string.IsNullOrWhiteSpace(row["Account1"].ToString()) ? null : row["Account1"]
                        , string.IsNullOrWhiteSpace(row["SubAccount1"].ToString()) ? null : row["SubAccount1"]
                        , bbl.Z_Set(row["OtherGaku2"])
                        , string.IsNullOrWhiteSpace(row["Account2"].ToString()) ? null : row["Account2"]
                        , string.IsNullOrWhiteSpace(row["SubAccount2"].ToString()) ? null : row["SubAccount2"]
                        , 0
                        );

            }

            return dt;
        }
        private void Para_AddD(DataTable dt)
        {
            dt.Columns.Add("PayeeCD", typeof(string));
            dt.Columns.Add("PayPlanDate", typeof(DateTime));
            dt.Columns.Add("PayNORows", typeof(int));
            dt.Columns.Add("PayPlanNO", typeof(int));
            dt.Columns.Add("PayGaku", typeof(decimal));
            dt.Columns.Add("PayConfirmFinishedKBN", typeof(int));
            dt.Columns.Add("ProcessingKBN", typeof(int));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }
        private DataTable GetDetailGridEntity()
        {
            DataTable dt = new DataTable();
            Para_AddD(dt);

            int rows = 0;
            int PayConfirmFinishedKBN = 0;  //未支払額＝0の場合、1：完了
            int ProcessingKBN = 0;//支払済額＜＞0の場合、3：確定, 上記以外の場合1：支払締(締中)
            string payeeCD = "";
            string payPlanDate = "";
            bool read = true;

            foreach (DataRow row in dtPay1Detail.Rows)
            {
                if (payeeCD != row["PayeeCD"].ToString() || payPlanDate != row["PayPlanDate"].ToString())
                {
                    rows = 0;
                    payeeCD = row["PayeeCD"].ToString();
                    payPlanDate = row["PayPlanDate"].ToString();
                    
                    DataRow[] head = dtPayplan.Select("PayeeCD = '" + payeeCD + "'" + "and PayPlanDate = '" + payPlanDate + "'");
                    if (bbl.Z_Set(head[0]["PayGaku"]) == 0)
                    {
                        read = false;
                        continue;
                    }
                    else
                    {
                        read = true;
                    }
                }
                else
                {
                    if (!read)
                        continue;
                }

                PayConfirmFinishedKBN = 0;
                ProcessingKBN = 1;

                if (bbl.Z_Set(row["Chk"]) > 0)
                {
                    rows++;

                    if (bbl.Z_Set(row["UnpaidAmount2"]) == 0)
                        PayConfirmFinishedKBN = 1;

                    if (bbl.Z_Set(row["PayConfirmGaku"]) != 0)
                        ProcessingKBN = 3;

                    dt.Rows.Add(row["PayeeCD"]
                        , row["PayPlanDate"]
                        , rows  //bbl.Z_Set(row["PayNORows"])
                        , bbl.Z_Set(row["PayPlanNO"])
                        , bbl.Z_Set(row["UnpaidAmount1"])   //PayGaku
                        , PayConfirmFinishedKBN
                        , ProcessingKBN
                        , 0
                        );
                }
            }

            return dt;
        }
        private void txtDueDate2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckDate2())
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void txtPaymentDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckPaymentDate())
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void txtBillSettleDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckBillSettleDate())
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void dgvPayment_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                if (e.Exception != null)
                {
                    //MessageBox.Show(this,
                    //    string.Format("({0}, {1}) のセルでエラーが発生しました。\n\n説明: {2}",
                    //    e.ColumnIndex, e.RowIndex, e.Exception.Message),
                    //    "エラーが発生しました",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
    }
}
