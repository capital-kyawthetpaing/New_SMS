using System.Windows.Forms;
using BL;
using Entity;
using Search;
using Base.Client;
using CKM_Controls;
using System;
using System.Data;

namespace MasterTouroku_GinkouShiten
{
    public partial class MasterTouroku_GinkouShiten : FrmMainForm
    {
       
        MasterTouroku_GinkouShiten_BL mtbstbl;
        M_Bank_Entity mb;
        M_BankShiten_Entity mbste;
        string date;
        int type = 0;//1 = normal, 2 = copy (for f11)
          
        public MasterTouroku_GinkouShiten()
        {
            InitializeComponent();

            Load += new System.EventHandler(FormLoadEvent);
            PanelNormal.Enter += PanelNormal_Enter;
            PanelCopy.Enter += PanelCopy_Enter;
            ScBranchCD.Leave += ScNormal_Leave;
            KeyUp += Form_KeyUp;
        }
        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void ScNormal_Leave(object sender, EventArgs e)
        {

        }

        private void FormLoadEvent(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            mtbstbl = new MasterTouroku_GinkouShiten_BL();
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            SetRequireField();
            SelectNextControl(PanelDetail, true, true, true, true);         
            
            ScBankCD.SetFocus(1);
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
            //***SearchControl.TxtCode.Require(true);
            //***SearchControl.TxtChangeDate.Require(true);
            ScBankCD.TxtCode.Require(true);
            ScBranchCD.TxtCode.Require(true);
            ScBranchCD.TxtChangeDate.Require(true);
            //ScCopyBankCD.TxtCode.Require(true);//更新09月06日
            //ScCopyBranchCD.TxtCode.Require(true);//更新09月06日
            //ScCopyBranchCD.TxtChangeDate.Require(true);//更新09月06日
            TxtBankBranchName.Require(true);
            TxtKanaName.Require(true);
        }
        public override void FunctionProcess(int index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
            mtbstbl = new MasterTouroku_GinkouShiten_BL();
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
                        ScBankCD.SetFocus(1);
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
                            ScCopyBankCD.SetFocus(1);
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(ScCopyBranchCD.TxtChangeDate.Text) || DisplayData(ScCopyBranchCD))
                            {
                                DisablePanel(PanelHeader);
                                F11Enable = false;
                                btnDisplay.Enabled = false;
                                EnablePanel(PanelDetail);
                                SelectNextControl(PanelDetail, true, true, true, true);
                            }
                        }
                        break;

                    case EOperationMode.UPDATE:
                        if (DisplayData(ScBranchCD))
                        {
                            //DisablePanel(PanelCopy);
                            DisablePanel(PanelHeader);
                            EnablePanel(PanelDetail);
                            F11Enable = false;
                            btnDisplay.Enabled = false;
                            //DisablePanel(PanelNormal);
                            //F11Enable = false;                            
                            F12Enable = true;
                            SelectNextControl(PanelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData(ScBranchCD))
                        {
                            //DisablePanel(PanelCopy);
                            DisablePanel(PanelHeader);
                            F11Enable = false;
                            btnDisplay.Enabled = false;
                            //DisablePanel(PanelNormal);
                            DisablePanel(PanelDetail);
                            SelectNextControl(PanelDetail, true, true, true, true);
                            F12Enable = true;
                            F11Enable = false;
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData(ScBranchCD))
                        {
                            //DisablePanel(PanelCopy);
                            DisablePanel(PanelHeader);
                            F11Enable = false;
                            btnDisplay.Enabled = false;
                            //DisablePanel(PanelNormal);
                            DisablePanel(PanelDetail);
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
                //*** Create Entity Object
                //mse = GetSoukoEntity();
                if (mtbstbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    mbste = GetBankBranchEntity();
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

        private  M_BankShiten_Entity GetBankBranchEntity()
        {
            mbste = new M_BankShiten_Entity
            {               
                BankCD = ScBankCD.Code,
                BranchCD = ScBranchCD.Code,
                ChangeDate = ScBranchCD.ChangeDate,
                BranchName = TxtBankBranchName.Text,
                BranchKana = TxtKanaName.Text,
                Remarks = TxtRemark.Text,
                DeleteFlg = ChkDeleteFlg.Checked ? "1" : "0",
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                // Key = ScBankCD.Code + " " + ScBranchCD.Code + " " + ScBranchCD.ChangeDate,
                Key = ScBranchCD.Code + " " + ScBranchCD.ChangeDate,
                PC = InPcID
            };

            return mbste;
        }
      
        private bool DisplayData(CKM_SearchControl sc)
        {
            mbste = new M_BankShiten_Entity();

            if (type==1)
            {   
                mbste.BankCD = ScBankCD.Code;
                mbste.BranchCD = sc.Code;
                mbste.ChangeDate = sc.ChangeDate;
            }
            else
            {
                mbste.BankCD = ScCopyBankCD.Code;
                mbste.BranchCD = ScCopyBranchCD.Code;
                mbste.ChangeDate = ScCopyBranchCD.ChangeDate;
            }

  
            mbste = mtbstbl.M_BankShiten_Select(mbste);
            if(mbste!=null)
            {
                TxtBankBranchName.Text = mbste.BranchName;
                TxtKanaName.Text = mbste.BranchKana;
                TxtRemark.Text = mbste.Remarks;
                ChkDeleteFlg.Checked = mbste.DeleteFlg.Equals("1") ? true : false;
                TxtBankBranchName.Focus();

                return true;
            }
            else
            {
                mtbstbl.ShowMessage("E133");
                return false;
            }          
        }
        private void InsertUpdate(int mode)
        {
            //*** Insert Update Function
            if (mtbstbl.M_BankShiten_InsertUpdate(mbste,mode))
            {
                Clear(PanelHeader);
                Clear(PanelDetail);

                ChangeMode(OperationMode);
                ScBankCD.SetFocus(1);
                //show message==>I101
                mtbstbl.ShowMessage("I101");
            }
            else
            {
                mtbstbl.ShowMessage("S001");
            }
        }
        private void Delete()
        {
            //*** Delete Function
            if(mtbstbl.M_BankShiten_Delete(mbste))
            {
                Clear(PanelHeader);
                Clear(PanelDetail);
                ChangeMode(OperationMode);
                //show message ==> I102
                mtbstbl.ShowMessage("I102");
            }
            else
            {
                //show Message ==> E154
                mtbstbl.ShowMessage("S001");
            }
            //if (mtsbl.M_Souko_Delete(mse))
            //{
            //    Clear(PanelHeader);
            //    Clear(PanelDetail);

            //    EnablePanel(PanelHeader);
            //    DisablePanel(PanelDetail);
            //    ScSoukoCD.SetFocus(1);

            //    mtsbl.ShowMessage("I102");
            //}
            //else
            //{
            //    mtsbl.ShowMessage("S001");
            //}
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
                        if (!RequireCheck(new Control[] { ScBankCD.TxtCode, ScBranchCD.TxtCode, ScBranchCD.TxtChangeDate }))//E102
                            return false;
                       
                            if((Convert.ToDateTime(ScBranchCD.ChangeDate)) < (Convert.ToDateTime(date)))
                            {
                                bbl.ShowMessage("E133");
                                ScBankCD.SetFocus(1);
                                return false;
                            }


                        if (!ScBankCD.IsExists(2))
                        {
                            mtbstbl.ShowMessage("E133");
                            ScBankCD.SetFocus(1);
                            return false;
                        }

                        if (ScBranchCD.IsExists(1))
                        {
                            //***show Message mtsbl.ShowMessage("E132"); 
                            mtbstbl.ShowMessage("E132");
                            ScBranchCD.SetFocus(1);
                            return false;
                        }
                    }
                    else//Copy
                    {
                        //if (!RequireCheck(new Control[] { ScBankCD.TxtCode,ScBranchCD.TxtCode, ScBranchCD.TxtChangeDate,ScCopyBankCD.TxtCode,ScCopyBranchCD.TxtCode,ScCopyBranchCD.TxtChangeDate }))
                        
                        if (!RequireCheck(new Control[] { ScBankCD.TxtCode, ScBranchCD.TxtCode, ScBranchCD.TxtChangeDate }))
                            return false;

                        if (!RequireCheck(new Control[] { ScCopyBankCD.TxtCode },ScCopyBranchCD.TxtCode))
                            return false;

                        if (!RequireCheck(new Control[] { ScCopyBranchCD.TxtCode }, ScCopyBranchCD.TxtChangeDate))
                            return false;

                        if (!RequireCheck(new Control[] { ScCopyBranchCD.TxtChangeDate }, ScCopyBranchCD.TxtCode))
                            return false;
                        if ((Convert.ToDateTime(ScBranchCD.ChangeDate)) < (Convert.ToDateTime(date)))
                        {
                            bbl.ShowMessage("E133");
                            ScBankCD.SetFocus(1);
                            return false;
                        }

                        if (!ScBankCD.IsExists(2))
                        {
                            mtbstbl.ShowMessage("E133");
                            ScBankCD.SetFocus(1);
                            return false;
                        }
                        if (ScBranchCD.IsExists(1))
                        {
                            //***show Message mtsbl.ShowMessage("E132"); 
                            mtbstbl.ShowMessage("E132");
                            ScBranchCD.SetFocus(1);
                            return false;
                        }

                        if (!string.IsNullOrWhiteSpace(ScCopyBranchCD.TxtCode.Text) && !string.IsNullOrWhiteSpace(ScCopyBranchCD.TxtChangeDate.Text))
                        {
                            if (!ScCopyBranchCD.IsExists(1))
                            {
                                //*** show Message mtsbl.ShowMessage("E133");
                                mtbstbl.ShowMessage("E133");
                                ScCopyBranchCD.SetFocus(1);
                                return false;
                            }

                        }
                    }
                }
                else 
                {
                    if (!RequireCheck(new Control[] { ScBankCD.TxtCode, ScBranchCD.TxtCode, ScBranchCD.TxtChangeDate }))
                        return false;

                    //if (!string.IsNullOrWhiteSpace(ScCopyBranchCD.TxtCode.Text) && !string.IsNullOrWhiteSpace(ScCopyBranchCD.TxtChangeDate.Text))
                    //{
                    //    if (!ScBranchCD.IsExists(1))
                    //    {
                    //        //*** Show Message mtsbl.ShowMessage("E133");
                    //        mtbstbl.ShowMessage("E133");
                    //        ScBranchCD.SetFocus(1);
                    //        return false;
                    //    }
                    //}
                }
            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { ScBranchCD.TxtCode, ScBranchCD.TxtChangeDate   }))
                    return false;

                if (OperationMode == EOperationMode.INSERT)
                {
                    if ((Convert.ToDateTime(ScBranchCD.ChangeDate)) < (Convert.ToDateTime(date)))
                    {
                        bbl.ShowMessage("E133");
                        ScBankCD.SetFocus(1);
                        return false;
                    }

                    //DataTable dtResult = bbl.SimpleSelect1("4", ScBranchCD.ChangeDate, ScBankCD.TxtCode.Text);
                    //if (dtResult.Rows.Count < 1)
                    //{
                    //    bbl.ShowMessage("E133");
                    //    ScBankCD.SetFocus(1);
                    //    return false;
                    //}
                    if (!ScBankCD.IsExists(2))
                    {
                        mtbstbl.ShowMessage("E133");
                        ScBankCD.SetFocus(1);
                        return false;
                    }
                    if (ScBranchCD.IsExists(1))
                    {
                        //***show Message mtsbl.ShowMessage("E132"); 
                        mtbstbl.ShowMessage("E132");
                        ScBranchCD.SetFocus(1);
                        return false;
                    }
                }

                if (!RequireCheck(new Control[] { TxtBankBranchName, TxtKanaName }))
                    return false;
                if (OperationMode == EOperationMode.DELETE)
                {
                    if (ScBranchCD.IsExistsDeleteCheck())
                    {
                        //Show Message ==>E154
                        mtbstbl.ShowMessage("E154");
                        ScBranchCD.SetFocus(1);    
                        return false;
                    }
                }
                //*** Insert Other Error Check
            }
            return true;
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
                    ScBranchCD.SearchEnable = false;
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
                    ScBranchCD.SearchEnable = true;
                    DisablePanel(PanelCopy);
                    DisablePanel(PanelDetail);
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
            }
            ScBankCD.SetFocus(1);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        //private void MasterTouroku_GinkouShiten_Load(object sender, EventArgs e)
        //{
            
        //        InProgramID = Application.ProductName;
        //        SetFunctionLabel(EProMode.MENTE);
        //        StartProgram();
        //        mtbstbl = new MasterTouroku_GinkouShiten_BL();
        //        ScBankCD.SetFocus(1);
        //        SetRequireField();
        //}

        private void ScBranchCD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter || e.KeyCode==Keys.F11)
            {
                ScBankCD.ChangeDate = string.IsNullOrWhiteSpace(ScBranchCD.ChangeDate) ? bbl.GetDate() : ScBranchCD.ChangeDate; ;
                type = 1;
                F11();
            }
        }
     
        private void ScCopyBranchCD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            ScCopyBankCD.ChangeDate = string.IsNullOrWhiteSpace(ScCopyBranchCD.ChangeDate) ? bbl.GetDate() : ScCopyBranchCD.ChangeDate; ;
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 2;
                F11();
            }
        }
        private void ScBankCD_Leave(object sender, EventArgs e)
        {
            ScBranchCD.Value1 = ScBankCD.Code;
            ScBranchCD.Value2 = ScBankCD.LabelText;
           // ScBankCD.ChangeDate = ScBranchCD.TxtChangeDate.Text;
        }

        private void ScCopyBankCD_Leave(object sender, EventArgs e)
        {
            ScCopyBranchCD.Value1 = ScCopyBankCD.Code;
            ScCopyBranchCD.Value2 = ScCopyBankCD.LabelText;
            //ScCopyBankCD.ChangeDate = ScCopyBranchCD.TxtChangeDate.Text;
        }

        private void ChangeDate_Leave(object sender, EventArgs e)
        {
            //ScBankCD.ChangeDate = ScBranchCD.TxtChangeDate.Text;
        }

        private void CopyChangeDate_Leave(object sender, EventArgs e)

        {
            ScCopyBankCD.ChangeDate = string.IsNullOrWhiteSpace(ScCopyBranchCD.ChangeDate) ? bbl.GetDate() : ScBranchCD.ChangeDate; ;
           // ScCopyBankCD.ChangeDate = ScCopyBranchCD.TxtChangeDate.Text;
        }

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
                         mb = GetDate();
                        mb = mtbstbl.M_Bank_ChangeDate_Select(mb);
                        if (mb != null)
                        {
                            date = mb.ChangeDate;
                        }

                    }
                    else
                    {
                        mtbstbl.ShowMessage("E101");
                        ScBankCD.SetFocus(1);
                    }
                }

            }

        }
        private M_Bank_Entity GetDate()
        {
            mb = new M_Bank_Entity
            {
                BankCD = ScBankCD.TxtCode.Text,
                ChangeDate = string.IsNullOrWhiteSpace(ScBranchCD.ChangeDate) ? bbl.GetDate() : ScBranchCD.ChangeDate
            };
            return mb;
        }
        private void ScCopyBankCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ScCopyBankCD.ChangeDate = string.IsNullOrWhiteSpace(ScCopyBranchCD.ChangeDate) ? bbl.GetDate() : ScCopyBranchCD.ChangeDate; ;
                if (!string.IsNullOrWhiteSpace(ScCopyBankCD.TxtCode.Text))
                {
                    ScCopyBankCD.ChangeDate = string.IsNullOrWhiteSpace(ScCopyBranchCD.ChangeDate) ? bbl.GetDate() : ScCopyBranchCD.ChangeDate;

                    if (ScCopyBankCD.SelectData())
                    {
                        ScCopyBranchCD.Value1 = ScCopyBankCD.TxtCode.Text;
                        ScCopyBranchCD.Value2 = ScCopyBankCD.LabelText;
                    }
                    else
                    {
                        mtbstbl.ShowMessage("E101");
                        ScCopyBankCD.SetFocus(1);
                    }

                }

            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }

        private void ScBankCD_Enter(object sender, EventArgs e)
        {
             ScBankCD.ChangeDate = string.IsNullOrWhiteSpace(ScBranchCD.ChangeDate) ? bbl.GetDate() : ScBranchCD.ChangeDate; ;
        }

        private void ScCopyBankCD_Enter(object sender, EventArgs e)
        {
            ScCopyBankCD.ChangeDate = string.IsNullOrWhiteSpace(ScCopyBranchCD.ChangeDate) ? bbl.GetDate() : ScCopyBranchCD.ChangeDate; ;
        }
    }
}
