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
using Search;
using CKM_Controls;
using Entity;

namespace MasterTouroku_Renban
{
    public partial class FrmMasterTouroku_Renban : FrmMainForm
    {
        MasterTouroku_Renban_BL mtrbl;
        M_Renban_Entity mre;

        public FrmMasterTouroku_Renban()
        {
            InitializeComponent();
            mtrbl = new MasterTouroku_Renban_BL();
        }


        private void FrmMasterTouroku_Renban_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_Renban";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            txtPrefixValue.Focus();
        }

        private void CheckMode()
        {
            switch (OperationMode)
            {
                case EOperationMode.INSERT:

                    F9Visible = false;
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    BtnF11Show.Enabled = F11Enable = true;
                    F12Enable = true;
                    txtPrefixValue.Focus();
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    BtnF11Show.Enabled = F11Enable = true;
                    F12Enable = false;
                    txtPrefixValue.Focus();
                    break;
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void SetRequireField()
        {
            
            txtContinuous.Require(true);
            txtPrefixValue.Require(true);
        }
        private M_Renban_Entity GetRenbanEnity()
        {
            mre = new M_Renban_Entity
            {
                PrefixValue = txtPrefixValue.Text,
                Continuous = txtContinuous.Text,
                Operator = InOperatorCD,
                ProcessMode = ModeText,
                ProgramID = InProgramID,
                PC = InPcID,
                Key = txtPrefixValue.Text +" "+ txtContinuous.Text
            };
            return mre;
        }
        public override void FunctionProcess(int index)
        {

            switch (index + 1)
            {
                case 2:
                    HandleMode(EOperationMode.INSERT, 1);
                    break;
                case 3:
                    HandleMode(EOperationMode.UPDATE, 2);
                    break;
                case 4:
                    HandleMode(EOperationMode.DELETE, 3);
                    break;
                case 5:
                    HandleMode(EOperationMode.SHOW, 4);
                    CheckMode();
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                        CheckMode();
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    // if (mtrbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") != DialogResult.Yes) ;
                    F12();
                    break;
            }
        }

        private void HandleMode(EOperationMode mode, int index)
        {

            OperationMode = mode;
            CheckMode();
            txtPrefixValue.Focus();
        }
        private void F11()
        {
            if (ErrorCheck(11))
            {
                switch (OperationMode)
                {
                    case EOperationMode.INSERT:

                        if (DisplayDataInsert(txtPrefixValue.Text))
                        {
                            DisablePanel(PanelHeader);
                            EnablePanel(PanelDetail);
                            txtContinuous.Focus();
                        }
                        break;
                    case EOperationMode.UPDATE:
                        if (DisplayData(txtPrefixValue.Text))
                        {
                            DisablePanel(PanelHeader);
                            EnablePanel(PanelDetail);
                            F11Enable = false;
                            F12Enable = true;
                            txtContinuous.Focus();
                            BtnF11Show.Enabled = F11Enable = false;
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData(txtPrefixValue.Text))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            F11Enable = true;
                            txtContinuous.Focus();
                            BtnF11Show.Enabled = F11Enable = false;
                            F12Enable = true;
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData(txtPrefixValue.Text))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            F11Enable = false;
                            F12Enable = false;
                            BtnF11Show.Enabled = F11Enable = false;
                        }
                        break;
                }
            }
        }

        private bool DisplayDataInsert(string prefix)
        {
            mre = new M_Renban_Entity()
            {
                PrefixValue = txtPrefixValue.Text
            };

            mre = mtrbl.M_Renban_Select(mre);

            if (mre != null)
            {
                txtPrefixValue.Text = mre.PrefixValue;
                txtContinuous.Text = mre.Continuous;
                txtPrefixValue.Focus();
                return true;
            }
            else
            {
                txtContinuous.Focus();
                return false;
            }
        }

        private bool DisplayData(string prefix)
        {
            mre = new M_Renban_Entity()
            {
                PrefixValue = txtPrefixValue.Text
            };

            mre = mtrbl.M_Renban_Select(mre);

            if (mre != null)
            {
                txtPrefixValue.Text = mre.PrefixValue;
                txtContinuous.Text = mre.Continuous;
                txtPrefixValue.Focus();

                return true;
            }
            else
            {
                mtrbl.ShowMessage("E133");
                txtContinuous.Focus();
                return false;
            }
        }
        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (mtrbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {

                    mre = GetRenbanEnity();
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
                    txtContinuous.Focus();
                }
            }

        }
        private void InsertUpdate(int mode)
        {
            if (mode == 1)
            {
                if (mtrbl.M_Renban_Insert_Update(mre, mode))
                {
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    mtrbl.ShowMessage("I101");
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    txtPrefixValue.Focus();
                }
                else
                {
                    mtrbl.ShowMessage("S001");
                }
            }
            else
            {
                if (mtrbl.M_Renban_Insert_Update(mre, mode))
                {
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    mtrbl.ShowMessage("I101");
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    txtPrefixValue.Focus();
                    BtnF11Show.Enabled = F11Enable = true;
                }
                else
                {
                    mtrbl.ShowMessage("S001");
                }
            }

        }
        private void Delete()
        {
            if (mtrbl.M_Renban_Delete(mre))
            {
                Clear(PanelHeader);
                Clear(PanelDetail);

                EnablePanel(PanelHeader);
                DisablePanel(PanelDetail);
                txtPrefixValue.Focus();
                BtnF11Show.Enabled = F11Enable = true;
                mtrbl.ShowMessage("I102");
                
            }
            else
            {
                mtrbl.ShowMessage("S001");
            }

        }
        private bool PrefixExist()
        {
            if (string.IsNullOrWhiteSpace(txtPrefixValue.Text))
                return true;

            return mtrbl.M_Renban_Exists(txtPrefixValue.Text);
        }
        private bool ErrorCheck(int index)
        {

            if (index == 11)
            {
                if (string.IsNullOrWhiteSpace(txtPrefixValue.Text))

                {
                    mtrbl.ShowMessage("E102");
                    txtPrefixValue.Focus();
                    return false;
                }
                
                if (OperationMode == EOperationMode.INSERT)
                {

                    if (!string.IsNullOrWhiteSpace(txtPrefixValue.Text))
                    {
                        if (PrefixExist())
                        {
                            mtrbl.ShowMessage("E132");
                            txtPrefixValue.Focus();
                            return false;
                        }
                        else
                        {
                            DisablePanel(PanelHeader);
                            F11Enable = false;
                            BtnF11Show.Enabled = false;
                            EnablePanel(PanelDetail);
                            txtContinuous.Focus();
                        }
                    }
                   
                }

                else if (OperationMode == EOperationMode.UPDATE || OperationMode == EOperationMode.DELETE || OperationMode == EOperationMode.SHOW)
                {
                    if (!PrefixExist())
                    {
                        mtrbl.ShowMessage("E133");
                        txtPrefixValue.Focus();
                        return false;
                    }
                }

            }
            else if (index == 12)
            {

                if (OperationMode == EOperationMode.INSERT)
                {

                    if (!string.IsNullOrWhiteSpace(txtPrefixValue.Text))
                    {
                        if (PrefixExist())
                        {
                            mtrbl.ShowMessage("E132");
                            txtPrefixValue.Focus();
                            return false;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(txtPrefixValue.Text))
                        {
                            mtrbl.ShowMessage("E102");
                            txtPrefixValue.Focus();
                            return false;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(txtContinuous.Text))
                    {
                        mtrbl.ShowMessage("E102");
                        txtContinuous.Focus();
                        return false;
                    }
                }
                else if (OperationMode == EOperationMode.UPDATE || OperationMode == EOperationMode.DELETE)
                {
                    if (string.IsNullOrWhiteSpace(txtPrefixValue.Text))
                    {
                        mtrbl.ShowMessage("E102");
                        txtContinuous.Focus();
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(txtContinuous.Text))
                    {
                        mtrbl.ShowMessage("E102");
                        txtContinuous.Focus();
                        return false;
                    }
                }
            }

            return true;
        }

        private void BtnF11Show_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }

        private void txtPrefixValue_KeyDown(object sender, KeyEventArgs e)

        {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtPrefixValue != null)
                    {
                        F11();
                    }
                }
        }

        private void FrmMasterTouroku_Renban_KeyUp(object sender, KeyEventArgs e)
        {

            MoveNextControl(e);
        }

        private void txtContinuous_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPrefixValue != null)
                {
                    if (string.IsNullOrWhiteSpace(txtContinuous.Text))

                    {
                        mtrbl.ShowMessage("E102");
                        txtContinuous.Focus();
                       // return false;
                    }
                }
            }
        }
    }
}
