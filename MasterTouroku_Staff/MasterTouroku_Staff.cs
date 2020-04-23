using System.Windows.Forms;
using BL;
using Entity;
using Search;
using Base.Client;
using CKM_Controls;
using System;
using System.Data;

namespace MasterTouroku_Staff
{
    public partial class frmMasterTouroku_Staff : FrmMainForm
    {
        M_Staff_Entity staffdata;
        M_Store_Entity storedata;
        MasterTouroku_Staff_BL mstaffBL;
        int type = 0;//1 = normal, 2 = copy (for f11)
        DataTable dtStaff;
        DataTable dtStore;
        string storeKBN = string.Empty;
        public frmMasterTouroku_Staff()
        {
            InitializeComponent();

            Load += new System.EventHandler(FormLoadEvent);
            PanelNormal.Enter += PanelNormal_Enter;
            PanelCopy.Enter += PanelCopy_Enter;
            ScStaff.Leave += ScNormal_Leave;
            //KeyDown += Form_KeyDown;
            mstaffBL = new MasterTouroku_Staff_BL();

        }
        private void ScNormal_Leave(object sender, EventArgs e)
        {
            foreach (Control c in PanelDetail.Controls)
                if (c is CKM_SearchControl)
                {
                    CKM_SearchControl sc = c as CKM_SearchControl;
                    sc.ChangeDate = ScStaff.ChangeDate;
                }
        }

        private void FormLoadEvent(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            SetRequireField();

            //SelectNextControl(PanelDetail, true, true, true, true);

            BindCombo();
         
        }

        private void PanelNormal_Enter(object sender, EventArgs e)
        {
            type = 1;
        }

        private void PanelCopy_Enter(object sender, EventArgs e)
        {
            type = 2;
        }

        private void SetRequireField()
        {
            ScStaff.TxtCode.Require(true);
            ScStaff.TxtChangeDate.Require(true);
            //ScStaffCopyCD.TxtCode.Require(true);
            //ScStaffCopyCD.TxtChangeDate.Require(true);
            txtStaffName.Require(true);
            txtStaffKana.Require(true);
            txtPassword.Require(true);
            txtConfirm.Require(true);
            txtJoinDate.Require(true);
        }

        private void BindCombo()
        {
            cboBMNCD.Bind(string.Empty);
            cboMenu.Bind(string.Empty);
            cboStoreMenu.Bind(string.Empty);
            cboAuthorizations.Bind(string.Empty);
            cboStoreAuthorizations.Bind(string.Empty);
            cboPosition.Bind(string.Empty);
        }
        
        public override void FunctionProcess(int index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
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
                        ScStaff.SetFocus(1);
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

        private void F11()
        {
            if (ErrorCheck(11))
            {
                switch (OperationMode)
                {
                    case EOperationMode.INSERT:
                        if (type == 1)
                        {
                            ScStaffCopyCD.SetFocus(1);
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(ScStaffCopyCD.ChangeDate) || (DisplayData(ScStaffCopyCD)))
                            {
                                DisablePanel(PanelHeader);
                                EnablePanel(PanelDetail);
                                btnDisplay.Enabled = F11Enable = false;
                                SelectNextControl(PanelDetail, true, true, true, true);
                            }
                                
                        }
                        break;
                    case EOperationMode.UPDATE:
                        if (DisplayData(ScStaff))
                        {
                            DisablePanel(PanelHeader);
                            EnablePanel(PanelDetail);
                            F11Enable = btnDisplay.Enabled = false;
                            F12Enable = true;
                            SelectNextControl(PanelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData(ScStaff))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            SelectNextControl(PanelDetail, true, true, true, true);
                            F11Enable = btnDisplay.Enabled = false;
                            F12Enable = true;
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData(ScStaff))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            F11Enable = btnDisplay.Enabled = false;
                            F12Enable = false;
                        }
                        break;
                }

                //***Add Control Enable/Disable;
            }
        }

        private void F12()
        {
            if (ErrorCheck(12))
            {
               if( mstaffBL.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
               {
                    //*** Create Entity Object
                   staffdata = GetStaffEntity();
                  switch (OperationMode)
                  {
                      case EOperationMode.INSERT:
                          InsertUpdate(1);
                          break;
                      case EOperationMode.UPDATE:
                          InsertUpdate(2);
                          break;
                      case EOperationMode.DELETE:
                          Delete();
                          break;
                  }
               
               }
                else PreviousCtrl.Focus();
            }
            
        }

        private bool DisplayData(CKM_SearchControl sc)
        {
            //*** Show Data
            staffdata = new M_Staff_Entity
            {
                StaffCD = sc.Code,
                ChangeDate = sc.ChangeDate
            };

            dtStaff = mstaffBL.StaffDisplay(staffdata);

            if (dtStaff.Rows.Count > 0)
            {
                txtStaffName.Text = dtStaff.Rows[0]["StaffName"].ToString();
                txtStaffKana.Text = dtStaff.Rows[0]["StaffKana"].ToString();
                ScStore.Code = dtStaff.Rows[0]["StoreCD"].ToString();
                ScStore.LabelText = dtStaff.Rows[0]["StoreName"].ToString();
                cboBMNCD.SelectedValue = dtStaff.Rows[0]["BMNCD"].ToString();
                cboMenu.SelectedValue = dtStaff.Rows[0]["MenuCD"].ToString();
                cboStoreMenu.SelectedValue = dtStaff.Rows[0]["StoreMenuCD"].ToString();
                cboAuthorizations.SelectedValue = dtStaff.Rows[0]["AuthorizationsCD"].ToString();
                cboStoreAuthorizations.SelectedValue = dtStaff.Rows[0]["StoreAuthorizationsCD"].ToString();
                cboPosition.SelectedValue = dtStaff.Rows[0]["PositionCD"].ToString();
                txtPassword.Text = dtStaff.Rows[0]["Password"].ToString();
                txtConfirm.Text = dtStaff.Rows[0]["Password"].ToString();
                txtJoinDate.Text = dtStaff.Rows[0]["JoinDate"].ToString();
                txtLeaveDate.Text = dtStaff.Rows[0]["LeaveDate"].ToString();
                txtReceiptPrint.Text = dtStaff.Rows[0]["ReceiptPrint"].ToString();
                txtRemark.Text = dtStaff.Rows[0]["Remarks"].ToString();
                if (dtStaff.Rows[0]["DeleteFlg"].ToString() == "1")
                    chkDelete.Checked = true;
                else chkDelete.Checked = false;
                
                ScStaff.SetFocus(1);

                return true;
            }
            else
            {
                mstaffBL.ShowMessage("E133");
                return false;
            }

          // return true;
        }

        private void InsertUpdate(int mode)
        {
            //*** Insert Update Function
            if (mstaffBL.M_Staff_Insert_Update(staffdata, mode))
            {
                Clear(PanelHeader);
                Clear(PanelDetail);

                ChangeMode(OperationMode);
                ScStaff.SetFocus(1);

                mstaffBL.ShowMessage("I101");
            }
            else
            {
                mstaffBL.ShowMessage("S001");
            }
        }

        private void Delete()
        {
            //*** Delete Function
            if (mstaffBL.M_Staff_Delete(staffdata))
            {
                Clear(PanelHeader);
                Clear(PanelDetail);

                EnablePanel(PanelNormal);
                DisablePanel(PanelCopy);
                DisablePanel(PanelDetail);
                ScStaff.SetFocus(1);

                mstaffBL.ShowMessage("I102");
            }
            else
            {
                mstaffBL.ShowMessage("S001");
            }
        }

        private M_Staff_Entity GetStaffEntity()
        {
            staffdata = new M_Staff_Entity()
            {
                StaffCD = ScStaff.Code,
                ChangeDate = ScStaff.ChangeDate,
                StaffName = txtStaffName.Text,
                StaffKana = txtStaffKana.Text,
                StoreCD = ScStore.Code,
                BMNCD = cboBMNCD.SelectedValue.ToString(),
                MenuCD = cboMenu.SelectedValue.ToString(),
                StoreMenuCD = cboStoreMenu.SelectedValue.ToString(),
                KengenCD = cboAuthorizations.SelectedValue.ToString(),
                StoreAuthorizationsCD = cboStoreAuthorizations.SelectedValue.ToString(),
                PositionCD = cboPosition.SelectedValue.ToString(),
                Password = txtPassword.Text,
                JoinDate = txtJoinDate.Text,
                LeaveDate = txtLeaveDate.Text,
                ReceiptPrint = txtReceiptPrint.Text,
                Remarks = txtRemark.Text,
                DeleteFlg = chkDelete.Checked ? "1" : "0",
                ProcessMode = ModeText,
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                Key = ScStaff.Code + " " + ScStaff.ChangeDate,
                PC = InPcID
            };
            return staffdata;
        }
        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                //HeaderCheck on F11
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 1)//New 
                    {
                        if (!RequireCheck(new Control[] { ScStaff.TxtCode, ScStaff.TxtChangeDate }))
                            return false;

                        if (ScStaff.IsExists(1))
                        {
                            mstaffBL.ShowMessage("E132");
                            ScStaff.SetFocus(1);
                            return false;
                        }
                    }
                    else//Copy
                    {
                        if (!RequireCheck(new Control[] { ScStaff.TxtCode, ScStaff.TxtChangeDate }))
                            return false;

                        if (!RequireCheck(new Control[] { ScStaffCopyCD.TxtCode }, ScStaffCopyCD.TxtChangeDate ))
                            return false;

                        if (!RequireCheck(new Control[] { ScStaffCopyCD.TxtChangeDate }, ScStaffCopyCD.TxtCode))
                            return false;

                        
                        if (!string.IsNullOrWhiteSpace(ScStaffCopyCD.TxtCode.Text) && !string.IsNullOrWhiteSpace(ScStaffCopyCD.ChangeDate))
                        {
                            if (!ScStaffCopyCD.IsExists(1))
                            {
                                mstaffBL.ShowMessage("E133");
                                ScStaffCopyCD.SetFocus(1);
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if (!ScStaff.IsExists(1))
                    {
                        mstaffBL.ShowMessage("E133");
                        ScStaff.SetFocus(1);
                        return false;
                    }
                }
            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { ScStaff.TxtCode, ScStaff.TxtChangeDate, txtStaffName, txtStaffKana, txtJoinDate, txtPassword, txtConfirm }))
                    return false;

                if (OperationMode == EOperationMode.INSERT)
                {
                    if (ScStaff.IsExists(1))
                    {
                        //*** ShowMessage
                        mstaffBL.ShowMessage("E132");
                        ScStaff.SetFocus(1);
                        return false;
                    }
                }

                if (!string.IsNullOrWhiteSpace(ScStore.Code))
                {
                    if(StoreData().Rows.Count <0)
                    {
                        mstaffBL.ShowMessage("E101");
                        ScStore.Focus();
                        //txt.MoveNext = false;
                        return false;
                    }

                    if (StoreData().Rows[0]["DeleteFlg"].ToString() == "1")
                    {
                        mstaffBL.ShowMessage("E158");
                        ScStore.SetFocus(1);
                        //txt.MoveNext = false;
                        return false;
                    }

                    if (StoreData().Rows[0]["StoreKBN"].ToString() == "1")
                    {
                        if (!RequireCheck(new Control[] { txtReceiptPrint }))
                            return false;
                    }
                }

                if (txtConfirm.Text != txtPassword.Text)
                {
                    mstaffBL.ShowMessage("E166");
                    txtConfirm.Focus();
                    return false;
                }

               
                //*** Insert Other Error Check
            }
            return true;
        }

        private DataTable StoreData()
        {
            storedata = new M_Store_Entity()
            {
                StoreCD = ScStore.Code,
                ChangeDate = ScStore.ChangeDate
            };
            dtStore = mstaffBL.StoreCheck(storedata);

            return dtStore;
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
                    ScStaff.SearchEnable = false;
                    ScStaffCopyCD.SearchEnable = true;
                    F9Visible = false;
                    F12Enable = true;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelCopy);
                    DisablePanel(PanelDetail);
                    ScStaff.SearchEnable = true;
                    ScStaffCopyCD.SearchEnable = false;
                    F9Visible = true;
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
            }
            ScStaff.SetFocus(1);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void ScStaff_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 1;
                F11();
            }
        }

        private void ScStaffCopyCD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 2;
                F11();
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }

        private void ScStore_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(!string.IsNullOrWhiteSpace(ScStore.Code))
                {
                    DataTable dt = StoreData();
                    if (!(dt.Rows.Count > 0))
                    {
                        mstaffBL.ShowMessage("E101");
                        ScStore.SetFocus(1);
                    }
                    else
                    {
                        if (dt.Rows[0]["DeleteFlg"].ToString() == "1")
                        {
                            mstaffBL.ShowMessage("E158");
                            ScStore.SetFocus(1);
                        }
                        else
                        {
                            storeKBN = dt.Rows[0]["StoreKBN"].ToString();
                            ScStore.LabelText = dt.Rows[0]["StoreName"].ToString();
                        }
                    }
                }
            }
        }

        private void txtReceiptPrint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScStore.Code))
                {
                    if (storeKBN == "1")
                    {
                        RequireCheck(new Control[] { txtReceiptPrint });
                    }
                }
            }
        }

        private void frmMasterTouroku_Staff_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        
        private void txtConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!(string.IsNullOrWhiteSpace(txtPassword.Text)) && (txtConfirm.Text != txtPassword.Text))
                {
                    mstaffBL.ShowMessage("E166");
                    txtConfirm.Focus();
                }
            }
        }
    }
}
