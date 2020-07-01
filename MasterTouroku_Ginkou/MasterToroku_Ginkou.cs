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
using Entity;
using BL;
using Search;
using CKM_Controls;

namespace MasterTouroku_Ginkou
{
    public partial class frmMasterTouroku_Ginkou : FrmMainForm
    {
        MasterToroku_Ginkou_Bl mgbl;
        M_Ginkou_Entity mge;
        int type = 0;//1 = ginkou, 2 = copy ginkou (for f11)
        public frmMasterTouroku_Ginkou()
        {
            InitializeComponent();
            panelNormal.Enter += panelNormal_Enter;
            panelCopy.Enter += panelCopy_Enter;
            ginKou_CD.Leave += ginKou_CD_Leave;
            mgbl = new MasterToroku_Ginkou_Bl();

        }

        private void frmMasterTouroku_Ginkou_Load(object sender, EventArgs e)
        {
            

            InOperatorCD = "0001";
            InProgramID = "MasterTouroku_Ginkou";
            mgbl = new MasterToroku_Ginkou_Bl();
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            ginKou_CD.Focus();
            SetRequiredField();
            //  ckmShop_ComboBox1.Bind(string.Empty);
            // ChangeBackground();
        }
        private void ginKou_CD_Leave(object sender, EventArgs e)
        {
            foreach (Control c in PanelDetail.Controls)
                if (c is CKM_SearchControl)
                {
                    CKM_SearchControl sc = c as CKM_SearchControl;
                    sc.ChangeDate = ginKou_CD.ChangeDate;
                }
        }
        private void panelNormal_Enter(object sender, EventArgs e)
        {
            type = 1;
        }

        private void panelCopy_Enter(object sender, EventArgs e)
        {
            type = 2;
        }
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {

                case EOperationMode.INSERT:
                    F9Visible = false;
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelHeader);
                    ginKou_CD.SearchEnable = false;
                    DisablePanel(PanelDetail);
                    BtnF11Show.Enabled = F11Enable = true;
                    copy_ginKou_CD.SearchEnable = true;
                    F9Visible = false;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(panelCopy);
                    Btn_F11.Enabled = true;
                    BtnF11Show.Enabled = true;
                    DisablePanel(PanelDetail);
                    ginKou_CD.SearchEnable = true;
                    copy_ginKou_CD.SearchEnable = false;
                    F9Visible = true;
                    break;
            }
            ginKou_CD.SetFocus(1);
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
                        ginKou_CD.SetFocus(1);
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
        private void InsertUpdate(int mode)
        {
            if (mgbl.insert_update_mginko(mge, mode))
            {
                Clear(PanelHeader);
                Clear(PanelDetail);
                ChangeMode(OperationMode);
                ginKou_CD.SetFocus(1);
                mgbl.ShowMessage("I101");
               
            }
            else
            {
                mgbl.ShowMessage("S001");
            }
        }

        private M_Ginkou_Entity GetGinkouEntity()
        {
            mge = new M_Ginkou_Entity {
                ginko_CD = ginKou_CD.Code,
                ginko_Changedate = ginKou_CD.ChangeDate,
                ginko_Name = ginko_name.Text,
                ginko_kananame = ginko_kananame.Text,
                ginko_remarks = ginko_remarks.Text,
                ginko_DeleteFlag = ChkDeleteFlg.Checked ? "1" : "0",
                ginko_insertOperator = InOperatorCD,
                ginko_useflag = "0",
                PC = InPcID,
                Program = InProgramID,
                ProcessMode = ModeText,
                KeyItem = ginKou_CD.Code +" " +ginKou_CD.ChangeDate
            };
            return mge;
        }

        private void Delete()
        {
            if (ginko_useflg.Text == "1")
            {
              //  mgbl.ShowMessage("E154");
            }
            else
            {
                if (mgbl.M_Ginkou_Delete(mge))
                {
                    Clear(PanelHeader);
                    Clear(PanelDetail);

                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    ginKou_CD.SetFocus(1);
                    mgbl.ShowMessage("I102");
                }
                else
                {
                    mgbl.ShowMessage("S001");
                }
            }
        }
        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (mgbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    mge = GetGinkouEntity();
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
                //else PreviousCtrl.Focus();
            }
        }

        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {

                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 1)
                    {

                        if (!RequireCheck(new Control[] { ginKou_CD.TxtCode, ginKou_CD.TxtChangeDate }))   // go that focus
                            return false;

                    }
                    else
                    {
                        if (!RequireCheck(new Control[] { ginKou_CD.TxtCode, ginKou_CD.TxtChangeDate }))
                            return false;

                        if (!RequireCheck(new Control[] { copy_ginKou_CD.TxtCode }, copy_ginKou_CD.TxtChangeDate))
                            return false;

                        if (!RequireCheck(new Control[] { copy_ginKou_CD.TxtChangeDate }, copy_ginKou_CD.TxtCode))
                            return false;

                        if (ginKou_CD.IsExists(1))
                        {
                            mgbl.ShowMessage("E132");
                            ginKou_CD.SetFocus(1);
                            return false;
                        }

                        if (!string.IsNullOrWhiteSpace(copy_ginKou_CD.TxtCode.Text) && !string.IsNullOrWhiteSpace(copy_ginKou_CD.TxtChangeDate.Text))
                        {
                            if (!copy_ginKou_CD.IsExists(1))
                            {
                                mgbl.ShowMessage("E133");
                                copy_ginKou_CD.SetFocus(1);
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if (!ginKou_CD.IsExists(1))
                    {
                        mgbl.ShowMessage("E133");
                        ginKou_CD.SetFocus(1);
                        return false;
                    }
                }
            }
            else if (index == 12)
            {
                if (OperationMode != EOperationMode.DELETE)
                {
                    if (!RequireCheck(new Control[] { ginKou_CD.TxtCode, ginKou_CD.TxtChangeDate, ginko_name, ginko_kananame }))
                        return false;
                }
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (ginKou_CD.IsExists(1))
                    {
                        mgbl.ShowMessage("E132");
                        ginKou_CD.SetFocus(1);
                        return false;
                    }
                }
                if (OperationMode == EOperationMode.DELETE)
                {

                    mge = GetGinkouEntity();
                   // Use - Flag == 1 Condition >> do nothing
                    if (mgbl.IsuseFlag(mge.ginko_CD, mge.ginko_Changedate) || mgbl.IsGinkoExistInShiten(mge.ginko_CD, mge.ginko_Changedate))
                    {
                        mgbl.ShowMessage("E154");
                        ginKou_CD.SetFocus(1);
                        return false;
                    }
                }
            }
            return true;
        }

        private bool GinkoExist(CKM_SearchControl sc)
        {
            if (string.IsNullOrWhiteSpace(sc.Code))
                return true;

            mge = new  M_Ginkou_Entity
            {
                FieldsName = "1",
                TableName = "M_bank",
                Condition = "BankCD = '" + sc.Code + "' and " +
                                "ChangeDate = '" + (string.IsNullOrWhiteSpace(sc.ChangeDate) ? DateTime.Now.ToString("yyyy/MM/dd") : sc.ChangeDate.Replace("/", "-")) + "'"
            };

            return mgbl.M_Ginkou_Exist(sc.Code,sc.ChangeDate);
        }
        private void HandleMode( EOperationMode mode, int index)
        {
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
                            copy_ginKou_CD.SetFocus(1);
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(copy_ginKou_CD.TxtChangeDate.Text) || DisplayData(copy_ginKou_CD))
                            {
                                EnablePanel(PanelDetail);
                                DisablePanel(PanelHeader);
                                BtnF11Show.Enabled = Btn_F11.Enabled = false;
                                ginko_name.Focus();
                            }
                        }
                        break;
                    case EOperationMode.UPDATE:
                    if (DisplayData(ginKou_CD))
                    {
                            DisablePanel(PanelHeader);
                            EnablePanel(PanelDetail);
                            BtnF11Show.Enabled = false;
                            Btn_F11.Enabled = false;
                            F12Enable = true;
                            ginko_name.Focus();
                    }
                    
                        break;
                    case EOperationMode.DELETE:
                    if (DisplayData(ginKou_CD))
                    {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            BtnF11Show.Enabled = false;
                            Btn_F11.Enabled = false;
                            F12Enable = true;
                            ginko_name.Focus();
                            
                    }
                    break;
                    case EOperationMode.SHOW:
                        if (DisplayData(ginKou_CD))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            BtnF11Show.Enabled = false;
                            Btn_F11.Enabled = false;
                            F12Enable = false;
                        }
                    break;
                }
            }
        }
        private bool DisplayData(CKM_SearchControl sc)
        {
            mge = new M_Ginkou_Entity
            {
                ginko_CD = sc.Code,
                ginko_Changedate = sc.ChangeDate
            };

            mge = mgbl.M_Ginkou_Entity_select(mge);

            if (mge != null)
            {
                ginko_name.Text = mge.ginko_Name;
                ginko_kananame.Text = mge.ginko_kananame;
                ginko_remarks.Text = mge.ginko_remarks;
                ChkDeleteFlg.Checked = mge.ginko_DeleteFlag.Equals("1") ? true : false;
                ginko_useflg.Text = mge.ginko_useflag;
                ginko_name.Focus();
                
                return true;
            }
            else
            {
                mgbl.ShowMessage("E133");
                return false;
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }

        private void ginKou_CD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 1;
                F11();
            }
        }

        private void copy_ginKou_CD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 2;
                F11();
            }
        }

        private void SetRequiredField()
        {
            //copy_ginKou_CD.TxtChangeDate.Require(true);
            //copy_ginKou_CD.TxtCode.Require(true);
            ginKou_CD.TxtChangeDate.Require(true);
            ginKou_CD.TxtCode.Require(true);
            ginko_kananame.Require(true);
            ginko_name.Require(true);

        }

        //private void ginKou_CD_Enter(object sender, EventArgs e)
        //{
        //    type = 1;
        //}

        //private void copy_ginKou_CD_Enter(object sender, EventArgs e)
        //{
        //    type = 2;
        //}

        //    private void frmMasterTouroku_Ginkou_KeyDown(object sender, KeyEventArgs e)
        //{
        //}
        //private void PanelDetail_Paint(object sender, PaintEventArgs e)
        //{

        //}
        private void frmMasterTouroku_Ginkou_KeyUp(object sender, KeyEventArgs e)
        {

            MoveNextControl(e);

        }




        private void BtnF11Show_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }

        private void PanelDetail_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
