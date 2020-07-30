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
using Search;
using BL;
using Entity;
using CKM_Controls;

namespace MasterTouroku_GinkouKouza
{
    public partial class FrmMasterTouroku_GinkouKouza : FrmMainForm
    {
         MasterTouroku_GinkouKouza_BL mtbbl;
         M_Kouza_Entity mkze;
        int type = 0;//1 = kouza, 2 = copy kouza (for f11)


        #region constructor
        /// <summary>
        /// default
        /// </summary>
        public FrmMasterTouroku_GinkouKouza()
        {
            InitializeComponent();
        }

        #endregion

        #region Form load
        private void FrmMasterTouroku_GinkouKouza_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_GinkouKouza";
            
            
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            SetRequireField();
        }

        #endregion

        #region ButtonClick
        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
            mtbbl = new MasterTouroku_GinkouKouza_BL();
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
                        ScKouzaCD.SetFocus(1);
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
       
        protected override void EndSec()
        {
            this.Close();
        }

        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(panel1);
                    EnablePanel(panel2);
                    DisablePanel(PanelDetail);
                    ScKouzaCD.SearchEnable = false;
                    F9Visible = false;
                    F12Enable = true;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(panel1);
                    EnablePanel(panel2);
                    DisablePanel(PanelDetail);
                    ScKouzaCD.SearchEnable = true;
                    ScCopyKouzaCD.Enabled = false;
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
            }
            ScKouzaCD.SetFocus(1);
        }
        #endregion

        #region ErrorCheck function helper
        private bool ErrorCheck(int index)
        {
            mtbbl = new MasterTouroku_GinkouKouza_BL();
            if (index == 11)
            {
                //HeaderCheck on F11
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 1)
                    {
                        if (!RequireCheck(new Control[] { ScKouzaCD.TxtCode, ScKouzaCD.TxtChangeDate }))   //,ScCopyKouzaCD.TxtCode,ScCopyKouzaCD.TxtChangeDate
                            return false;
                       
                    }


                    else
                    {
                        if (!RequireCheck(new Control[] { ScKouzaCD.TxtCode, ScKouzaCD.TxtChangeDate }))
                            return false;
                        if (!RequireCheck(new Control[] { ScCopyKouzaCD.TxtCode }, ScCopyKouzaCD.TxtChangeDate))
                            return false;

                        if (!RequireCheck(new Control[] { ScCopyKouzaCD.TxtChangeDate }, ScCopyKouzaCD.TxtCode))
                            return false;

                        if (ScKouzaCD.IsExists(1))
                        {
                            mtbbl.ShowMessage("E132");
                            ScKouzaCD.SetFocus(1);
                            return false;
                        }

                        if (!string.IsNullOrWhiteSpace(ScCopyKouzaCD.TxtCode.Text) && !string.IsNullOrWhiteSpace(ScCopyKouzaCD.TxtChangeDate.Text))
                        {
                            if (!ScCopyKouzaCD.IsExists(1))
                            {
                                mtbbl.ShowMessage("E133");
                                ScCopyKouzaCD.SetFocus(1);
                                return false;
                            }
                        }
                    }

                }
                else
                {
                    if (!ScKouzaCD.IsExists(1))
                    {
                        mtbbl.ShowMessage("E133");
                        ScKouzaCD.SetFocus(1);
                        return false;
                    }

                }
                
            }
            else if (index == 12)
            {

                if (!RequireCheck(new Control[] { ScKouzaCD.TxtCode, ScKouzaCD.TxtChangeDate}))
                    return false;
                if (OperationMode == EOperationMode.INSERT)
                {

                    if (ScKouzaCD.IsExists(1))
                    {
                        mtbbl.ShowMessage("E132");
                        ScKouzaCD.SetFocus(1);
                        return false;
                    }
                }

                if (!RequireCheck(new Control[] { txtKouzaName}))
                    return false;
                if(!RequireCheck(new Control[] { ScBankCD.TxtCode}))
                    return false;
                if (!ScBankCD.IsExists(2))
                {
                    mtbbl.ShowMessage("E101");
                    ScBankCD.SetFocus(1);
                    return false;
                }

                if (!RequireCheck(new Control[] { ScBranchCD.TxtCode }))
                    return false;
                if (!ScBranchCD.IsExists(2))
                {
                    mtbbl.ShowMessage("E101");
                    ScBranchCD.SetFocus(1);
                    return false;
                }

                if (!RequireCheck(new Control[] {  txtKouzaKBN }))
                    return false;

                if (txtKouzaKBN.Text!="1" && txtKouzaKBN.Text!="2")
                {
                    mtbbl.ShowMessage("E101");
                    ScBankCD.SetFocus(1);
                    return false;
                }

                if (!RequireCheck(new Control[] { txtKouzaNO, txtKouzaMeigi }))
                    return false;
                if (!RequireCheck(new Control[] { txtCompanyName },txtCompanyCD))
                    return false;
                if (!RequireCheck(new Control[] { txtTax11 }, txtFee11))
                    return false;
                if (!ReverseRequireCheck(new Control[] { txtFee12 }, txtAmount1))
                    return false;
                if (!RequireCheck(new Control[] { txtTax12 }, txtFee12))
                    return false;
                if (!RequireCheck(new Control[] { txtTax21 }, txtFee21))
                    return false;
                if (!RequireCheck(new Control[] { txtFee22 }, txtAmount2))
                    return false;
                if (!RequireCheck(new Control[] { txtTax22 }, txtFee22))
                    return false;
                if (!RequireCheck(new Control[] { txtTax31 }, txtFee31))
                    return false;
                if (!RequireCheck(new Control[] { txtFee32 }, txtAmount3))
                    return false;
                if (!RequireCheck(new Control[] { txtTax32 }, txtFee32))
                    return false;
                
            }
            return true;
        }

        private void SetRequireField()
        {
            ScKouzaCD.TxtCode.Require(true);
            ScKouzaCD.TxtChangeDate.Require(true);
            txtKouzaMeigi.Require(true);
            txtKouzaName.Require(true);
            ScBankCD.TxtCode.Require(true); 
            ScBranchCD.TxtCode.Require(true);
            txtKouzaKBN.Require(true);
            txtKouzaNO.Require(true);
            txtCompanyName.Require(true,txtCompanyCD);
            txtTax11.Require(true, txtFee11);
            txtFee12.ReverseCheck(true, txtAmount1);
            txtTax12.Require(true, txtFee12);

            txtTax21.Require(true, txtFee21);
            txtFee22.ReverseCheck(true,txtAmount2);
            txtTax22.Require(true, txtFee22);

            txtTax31.Require(true, txtFee31);
            txtFee32.ReverseCheck(true, txtAmount3);
            txtTax32.Require(true, txtFee32);
        }

     
        #endregion

        #region DisplayData
        private bool DisplayData(CKM_SearchControl sc)
        {
            mkze = new M_Kouza_Entity
            {
                KouzaCD = sc.Code,
                ChangeDate = sc.ChangeDate
            };

            mkze = mtbbl.M_Kouza_Select(mkze);

            if (mkze != null)
            {
                txtKouzaName.Text = mkze.KouzaName;
                ScBankCD.Code = mkze.BankCD;
                ScBankCD.LabelText = mkze.BankName;
                ScBranchCD.Value1 = mkze.BankCD;
                ScBranchCD.Value2 = mkze.BankName;
                ScBranchCD.Code= mkze.BranchCD;
                ScBranchCD.LabelText = mkze.BranchName;
                txtKouzaKBN.Text = mkze.KouzaKBN;
                txtKouzaMeigi.Text = mkze.KouzaMeigi;
                txtKouzaNO.Text = mkze.KouzaNO;
                txtPrint1.Text = mkze.Print1;
                txtPrint2.Text = mkze.Print2;
                txtPrint3.Text = mkze.Print3;
                txtPrint4.Text = mkze.Print4;

                txtFee11.Text = mkze.Fee11;
                txtTax11.Text = mkze.Tax11;
                txtAmount1.Text = mkze.Amount1;
                txtFee12.Text = mkze.Fee12;
                txtTax12.Text = mkze.Tax12;

                txtFee21.Text = mkze.Fee21;
                txtTax21.Text = mkze.Tax21;
                txtAmount2.Text = mkze.Amount2;
                txtFee22.Text = mkze.Fee22;
                txtTax22.Text = mkze.Tax22;

                txtFee31.Text = mkze.Fee31;
                txtTax31.Text = mkze.Tax31;
                txtAmount3.Text = mkze.Amount3;
                txtFee32.Text = mkze.Fee32;
                txtTax32.Text = mkze.Tax32;

                txtCompanyCD.Text = mkze.CompanyCD;
                txtCompanyName.Text = mkze.CompanyName;

                txtRemark.Text = mkze.Remarks;

                ChkDeleteFlg.Checked = mkze.DeleteFlg.Equals("1") ? true : false;

                txtKouzaName.Focus();

                return true;
            }
            else
            {
                mtbbl.ShowMessage("E133");
                return false;
            }
        }
        private void F11()
        {
            if (ErrorCheck(11))
            {
                switch (OperationMode)
                {
                    case EOperationMode.INSERT:


                        //if (type == 1)
                        //    txtKouzaName.Focus();
                        //else
                        //{
                        //    if (DisplayData(ScCopyKouzaCD))
                        //        txtKouzaName.Focus();
                        //}

                        if (type == 1)
                            ScCopyKouzaCD.SetFocus(1);
                        else
                        {
                            if(string.IsNullOrWhiteSpace(ScCopyKouzaCD.TxtChangeDate.Text)　|| (DisplayData(ScCopyKouzaCD)))
                            {
                                DisablePanel(panel1);
                                DisablePanel(panel2);
                                btnDisplay.Enabled = false;
                                Btn_F11.Enabled = false;
                                EnablePanel(PanelDetail);
                                txtKouzaName.Focus();
                            }
                            
                        }
                       

                        break;
                    case EOperationMode.UPDATE:
                        if (DisplayData(ScKouzaCD))
                        {
                            DisablePanel(panel1);
                            DisablePanel(panel2);
                            btnDisplay.Enabled = false;
                            Btn_F11.Enabled = false;
                            DisablePanel(PanelHeader);
                            EnablePanel(PanelDetail);
                            F12Enable = true;
                            txtKouzaName.Focus();
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData(ScKouzaCD))
                        {
                            DisablePanel(panel1);
                            DisablePanel(panel2);
                            DisablePanel(PanelDetail);
                            txtKouzaName.Focus();
                            F12Enable = true;
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData(ScKouzaCD))
                        {
                            DisablePanel(panel1);
                            DisablePanel(panel2);
                            DisablePanel(PanelDetail);
                            txtKouzaName.Focus();
                            F12Enable = false;
                        }
                        break;
                }
            }
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
             FunctionProcess(10);
          
        }

        #endregion

        #region Insert/Update/Date
        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (mtbbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    mkze = GetKouzaEntity();
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
                    PreviousCtrl.Focus();
            }
        }

        private void InsertUpdate(int mode)
        {
            if (mtbbl.M_Kouza_Insert_Update(mkze, mode))
            {
                mtbbl.ShowMessage("I101");
                Clear(PanelHeader);
                Clear(PanelDetail);

                ChangeMode(OperationMode);
                ScKouzaCD.SetFocus(1);
            }
            else
            {
                mtbbl.ShowMessage("S001");
            }
        }

        private void Delete()
        {
            if (mtbbl.M_Kouza_Delete(mkze))
            {
                Clear(PanelHeader);
                Clear(PanelDetail);

                EnablePanel(PanelHeader);
                DisablePanel(PanelDetail);
                ScKouzaCD.SetFocus(1);
            }
            else
            {
               mtbbl .ShowMessage("S001");
            }
        }

        #endregion

        #region Get Searching  Data
        private M_Kouza_Entity GetKouzaEntity()
        {
            mkze = new M_Kouza_Entity
            {
                KouzaCD = ScKouzaCD.Code,
                ChangeDate =ScKouzaCD.ChangeDate,
                KouzaName =txtKouzaName.Text,
                BankCD = ScBankCD.Code,
                BranchCD=ScBranchCD.Code,
                KouzaKBN=txtKouzaKBN.Text,
                KouzaNO = txtKouzaNO.Text,
                KouzaMeigi = txtKouzaMeigi.Text,
                CompanyCD = txtCompanyCD.Text,
                CompanyName = txtCompanyName.Text,
                Print1 = txtPrint1.Text,
                Print2 = txtPrint2.Text,
                Print3 = txtPrint3.Text,
                Print4 = txtPrint4.Text,

                Fee11 = txtFee11.Text,
                Tax11 = txtTax11.Text,
                Amount1 = txtAmount1.Text,
                Fee12 = txtFee12.Text,
                Tax12 = txtTax12.Text,

                Fee21 = txtFee21.Text,
                Tax21 = txtTax21.Text,
                Amount2 = txtAmount2.Text,
                Fee22 = txtFee22.Text,
                Tax22 = txtTax22.Text,

                Fee31 = txtFee31.Text,
                Tax31 = txtTax31.Text,
                Amount3 = txtAmount3.Text,
                Fee32 = txtFee32.Text,
                Tax32 = txtTax32.Text,

                Remarks = txtRemark.Text,
                DeleteFlg = ChkDeleteFlg.Checked ? "1" : "0",
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                Key = ScKouzaCD.Code + " " + ScKouzaCD.ChangeDate,
                PC = InPcID,
            };
            return mkze;
        }

        #endregion

        #region KeyDown Event
        private void ScKouzaCD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 1;
                F11();
            }
        }

        private void ScCopyKouzaCD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 0;
                F11();

                ScBranchCD.ChangeDate = ScBankCD.ChangeDate = ScKouzaCD.ChangeDate;
            }
        }

        private void ScKouzaCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {

        }

        //private void FrmMasterTouroku_GinkouKouza_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        if (ActiveControl is CKM_TextBox)
        //        {
                  
        //            if ((ActiveControl as CKM_TextBox).MoveNext)
        //            {
        //                if (this.Parent != null)
        //                    this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
        //                else
        //                    this.SelectNextControl(ActiveControl, true, true, true, true);
        //            }
        //            else
        //                (ActiveControl as CKM_TextBox).MoveNext = true;

        //        }
        //        else
        //        {
        //            if (this.Parent != null)
        //                this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
        //            else
        //                this.SelectNextControl(ActiveControl, true, true, true, true);
        //        }
        //    }
        //}

        private void txtKouzaKBN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtKouzaKBN.Text))
                {
                    if (txtKouzaKBN.Text != "1" && txtKouzaKBN.Text != "2")
                    {
                        mtbbl.ShowMessage("E101");
                        txtKouzaKBN.Focus();
                    }
                }
            }
        }

        #region Necessary function to move cursor when enter press

        private void ScBankCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScBankCD.TxtCode.Text))
                {
                    if (ScBankCD.SelectData())
                    {
                        ScBranchCD.Value1 = ScBankCD.TxtCode.Text;
                        ScBranchCD.Value2 = ScBankCD.LabelText;
                    }
                    else
                    {
                        mtbbl.ShowMessage("E101");
                        ScBankCD.SetFocus(1);
                    }

                }

            }

        }
        private void ScBranchCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScBranchCD.TxtCode.Text))
                {
                    if (!ScBranchCD.SelectData())
                    {
                        mtbbl.ShowMessage("E101");
                        ScBranchCD.SetFocus(1);
                    }

                }
                else
                {
                    mtbbl.ShowMessage("E101");
                    ScBranchCD.SetFocus(1);
                }

            }
        }

        #endregion

        private void FrmMasterTouroku_GinkouKouza_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        #endregion

        #region Leave Event
        private void ScKouzaCD_Leave(object sender, EventArgs e)
        {
            ScBranchCD.ChangeDate = ScBankCD.ChangeDate = ScKouzaCD.ChangeDate;
        }
        private void ScCopyKouzaCD_Leave(object sender, EventArgs e)
        {
            ScBranchCD.ChangeDate = ScBankCD.ChangeDate = ScCopyKouzaCD.ChangeDate;
        }
        private void ScBankCD_Leave(object sender, EventArgs e)
        {
            ScBranchCD.Value1 = ScBankCD.TxtCode.Text;
            ScBranchCD.Value2 = ScBankCD.LabelText;
        }

        #endregion

        #region Enter Event
        private void ScKouzaCD_Enter(object sender, EventArgs e)
        {
            type = 1;
        }

        private void ScCopyKouzaCD_Enter(object sender, EventArgs e)
        {
            type = 0;
        }

        #endregion

        private void ScBankCD_Enter(object sender, EventArgs e)
        {
            ScBankCD.ChangeDate = ScKouzaCD.ChangeDate;
        }

        private void ScBranchCD_Enter(object sender, EventArgs e)
        {
            ScBranchCD.ChangeDate = ScKouzaCD.ChangeDate;
        }
    }
}
