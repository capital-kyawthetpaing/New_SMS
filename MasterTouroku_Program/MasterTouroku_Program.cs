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
using Search;
namespace MasterTouroku_Program
{
    public partial class MasterTouroku_Program : FrmMainForm
    {
        M_Program_Entity mpe;
        MasterTouroku_Program_BL mpbl;
        int type = 0;
        public MasterTouroku_Program()
        {
            InitializeComponent();
            mpbl = new MasterTouroku_Program_BL();
            mpe = new M_Program_Entity();
        }

        private void MasterTouroku_Program_Load(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            SetRequireField();
            
            BindType();
            scProgramID.SetFocus(1);

        }
        
        private void SetRequireField()
        {
            scProgramID.TxtCode.Require(true);
            txtProgramName.Require(true);
            cboType.Require(true);
            txtExeName.Require(true);
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }
        private void PanelNormal_Enter(object sender, EventArgs e)
        {
            type = 1;
        }

        private void PanelCopy_Enter(object sender, EventArgs e)
        {
            type = 2;
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
                        scProgramID.SetFocus(1);
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
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelNormal);
                    EnablePanel(PanelCopy);
                    F11Enable = true;
                    DisablePanel(PanelDetail);
                    scProgramID.SearchEnable = false;
                    scProgramCopy.SearchEnable = true;
                    F9Visible = false;
                    F12Enable = true;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelNormal);
                    DisablePanel(PanelCopy);
                    DisablePanel(PanelDetail);
                    scProgramID.SearchEnable = true;
                    scProgramCopy.SearchEnable = false;
                    F9Visible = true;
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
            }
            scProgramID.SetFocus(1);
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
                            btnDisplay.Enabled = true;
                            F11Enable = true;
                            DisablePanel(PanelDetail);
                            scProgramCopy.SetFocus(1);
                        }
                        else
                        {
                            DisablePanel(PanelNormal);
                            DisablePanel(PanelCopy);
                            EnablePanel(PanelDetail);
                            btnDisplay.Enabled = false;
                            F11Enable = false;
                            SelectNextControl(PanelDetail, true, true, true, true);
                            DisplayData();
                            txtProgramName.Focus();
                        }
                        break;
                    case EOperationMode.UPDATE:
                        mpe.ProgramID = scProgramID.Code;  //ses
                        DisplayData();
                        DisablePanel(PanelNormal);
                        DisablePanel(PanelCopy);
                        btnDisplay.Enabled = false;
                        EnablePanel(PanelDetail);
                        F12Enable = true;
                        F11Enable = false;
                        SelectNextControl(PanelDetail, true, true, true, true);
                        txtProgramName.Focus();
                        break;
                    case EOperationMode.DELETE:
                        mpe.ProgramID = scProgramID.Code; //ses
                        DisplayData();
                        DisablePanel(PanelNormal);
                        DisablePanel(PanelCopy);
                        btnDisplay.Enabled = false;
                        DisablePanel(PanelDetail);
                        SelectNextControl(PanelDetail, true, true, true, true);
                        F12Enable = true;
                        F11Enable = false;
                        //scProgramID.SetFocus(1);
                        break;
                    case EOperationMode.SHOW:
                        mpe.ProgramID = scProgramID.Code;
                        DisplayData();
                        DisablePanel(PanelNormal);
                        DisablePanel(PanelCopy);
                        btnDisplay.Enabled = false;
                        DisablePanel(PanelDetail);
                        F12Enable = false;
                        F11Enable = false;
                        break;
                }
            }
        }
        private void DisplayData()
        {
            DataTable dt = new DataTable();
            dt = mpbl.M_Program_Select(mpe);

            if (dt.Rows.Count > 0)
            {
                txtProgramName.Text = dt.Rows[0]["ProgramName"].ToString();
                cboType.SelectedValue = dt.Rows[0]["Type"].ToString();
                txtExeName.Text = dt.Rows[0]["ProgramEXE"].ToString();
                txtFileDrive.Text = dt.Rows[0]["FileDrive"].ToString();
                txtFilePass.Text = dt.Rows[0]["FilePass"].ToString();
                txtFileName.Text = dt.Rows[0]["FileName"].ToString();
                scProgramID.SetFocus(1);
            }
            //else
            //{
            //    mpbl.ShowMessage("E133");
            //}
        }
        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (mpbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)


                {
                    mpe = GetProgramEntity();
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
                else
                {
                    PreviousCtrl.Focus();
                }
            }
            }
        private void InsertUpdate(int mode)
        {
            if (mpbl.M_Program_Insert_Update(mpe, mode))
            {
                ChangeMode(OperationMode);
                scProgramID.SetFocus(1);
                mpbl.ShowMessage("I101");
            }
            else
            {
                mpbl.ShowMessage("S001");
            }
        }
        private void Delete()
        {
            if (mpbl.M_Program_Delete(mpe))
            {
                ChangeMode(OperationMode);
                scProgramID.SetFocus(1);
                mpbl.ShowMessage("I102");
            }
            else
            {
                mpbl.ShowMessage("S001");
            }
        }
        private M_Program_Entity GetProgramEntity()
        {
            mpe = new M_Program_Entity
            {
                Program_ID=scProgramID.Code, //ses
                ProgramName=txtProgramName.Text,
                Type=cboType.SelectedValue.ToString(),
                ProgramEXE=txtExeName.Text,
                FileDrive = txtFileDrive.Text,
                FilePass =txtFilePass.Text,
                FileName=txtFileName.Text,
                ProcessMode = ModeText,
                ProgramID=InProgramID,
                InsertOperator = InOperatorCD,
                Key = scProgramID.Code,
                PC = InPcID
            };
            return mpe;
        }

        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 1)
                    {
                        if (!RequireCheck(new Control[] { scProgramID.TxtCode }))
                            return false;
                        DataTable dtProgram = new DataTable();
                        mpe.Program_ID = scProgramID.Code; //ses
                        dtProgram = mpbl.M_Program_Select(mpe);
                        if (dtProgram.Rows.Count > 0)
                        {
                            mpbl.ShowMessage("E132");
                            scProgramID.SetFocus(1);
                            return false;
                        }
                    }
                    else//Copy
                    {
                        if (!RequireCheck(new Control[] { scProgramID.TxtCode }))
                            return false;
                        DataTable dtProgram = new DataTable();
                        mpe.Program_ID = scProgramID.Code;
                        dtProgram = mpbl.M_Program_Select(mpe);
                        if (dtProgram.Rows.Count > 0)
                        {
                            mpbl.ShowMessage("E132");
                            scProgramID.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(scProgramCopy.TxtCode.Text))
                            {
                                mpe.Program_ID = scProgramCopy.Code;
                                DataTable dtcopyprogram = new DataTable();
                                dtcopyprogram = mpbl.M_Program_Select(mpe);
                                if (dtcopyprogram.Rows.Count > 0)
                                {
                                    txtProgramName.Text = dtcopyprogram.Rows[0]["ProgramName"].ToString();
                                }
                                else
                                {
                                    mpbl.ShowMessage("E133");
                                    scProgramCopy.SetFocus(1);
                                    return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    DataTable dtprogram = new DataTable();
                    mpe.Program_ID = scProgramID.Code; //ses
                    dtprogram = mpbl.M_Program_Select(mpe);
                    if (dtprogram.Rows.Count == 0)
                    {
                        mpbl.ShowMessage("E133");
                        scProgramID.SetFocus(1);
                        return false;
                    }
                }
            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { scProgramID.TxtCode,txtProgramName,cboType,txtExeName }))
                    return false;
                if (string.IsNullOrWhiteSpace(txtProgramName.Text))
                {
                    mpbl.ShowMessage("E102");
                    txtProgramName.Focus();
                    return false;
                }
                if (string.IsNullOrWhiteSpace(txtExeName.Text))
                {
                    mpbl.ShowMessage("E102");
                    txtExeName.Focus();
                    return false;
                }

                if (OperationMode == EOperationMode.INSERT)
                {
                    DataTable dtprogram = new DataTable();
                    mpe = GetProgramEntity();
                    dtprogram = mpbl.M_Program_Select(mpe);
                    if (dtprogram.Rows.Count > 0)
                    {
                        mpbl.ShowMessage("E132");
                        scProgramID.SetFocus(1);
                        return false;
                    }
                }
            }
            return true;
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void BindType()
        {
            DataTable dtAdd = new DataTable();
            dtAdd.Columns.Add("TypeID", typeof(int));
            dtAdd.Columns.Add("TypeName", typeof(string));
            dtAdd.Rows.Add(0, string.Empty);
            dtAdd.Rows.Add(1, "入力");
            dtAdd.Rows.Add(2, "印刷");
            dtAdd.Rows.Add(3, "印刷+出力");
            dtAdd.Rows.Add(4, "出力");
            dtAdd.Rows.Add(5, "照会");
            dtAdd.Rows.Add(6, "更新");
            cboType.ValueMember = "TypeID";
            cboType.DisplayMember = "TypeName";
            cboType.DataSource = dtAdd;
        }

        private void scProgramID_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 1;
                F11();
            }
        }

        private void scProgramCopy_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 2;

                F11();
            }
        }

        private void MasterTouroku_Program_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}

