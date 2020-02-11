using System.Windows.Forms;
using BL;
using Entity;
using Search;
using Base.Client;
using CKM_Controls;
using System;

namespace SampleTemplate
{
    public partial class Form1 : FrmMainForm
    {
        //***Declare Entity
        //***Declare BL
        int type = 0;//1 = normal, 2 = copy (for f11)

        public Form1()
        {
            InitializeComponent();

            Load += new System.EventHandler(FormLoadEvent);
            PanelNormal.Enter += PanelNormal_Enter;
            PanelCopy.Enter += PanelCopy_Enter;
            ScNormal.Leave += ScNormal_Leave;
            KeyUp += Form_KeyUp;
            //*** bl = new bl();

        }

        private void Form_KeyUp(object sender,KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void ScNormal_Leave(object sender, EventArgs e)
        {
            foreach (Control c in PanelDetail.Controls)
                if (c is CKM_SearchControl)
                {
                    CKM_SearchControl sc = c as CKM_SearchControl;
                    sc.ChangeDate = ScNormal.ChangeDate;
                }
        }

        private void FormLoadEvent(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            SetRequireField();

            SelectNextControl(PanelDetail, true, true, true, true);
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
                        ScNormal.SetFocus(1);
                    }
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    //*** CreatedBL.ShowMessage if (bbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
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
                        DisablePanel(PanelHeader);
                        EnablePanel(PanelDetail);
                        if (type == 1)
                            SelectNextControl(PanelDetail, true, true, true, true);
                        else
                        {
                            if (DisplayData(ScCopy))
                                SelectNextControl(PanelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.UPDATE:
                        if (DisplayData(ScNormal))
                        {
                            DisablePanel(PanelHeader);
                            EnablePanel(PanelDetail);
                            F12Enable = true;
                            SelectNextControl(PanelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData(ScNormal))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            SelectNextControl(PanelDetail, true, true, true, true);
                            F12Enable = true;
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData(ScNormal))
                        {
                            DisablePanel(PanelHeader);
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
        }

        private bool DisplayData(CKM_SearchControl sc)
        {
            //*** Show Data
            //mse = new M_Souko_Entity
            //{
            //    SoukoCD = sc.Code,
            //    ChangeDate = sc.ChangeDate
            //};

            //mse = mtsbl.M_Souko_Select(mse);

            //if (mse != null)
            //{
            //    TxtSoukoName.Text = mse.SoukoName;
            //    CboStoreCD.SelectedValue = mse.StoreCD;
            //    txtZipCD1.Text = mse.ZipCD1;
            //    txtZipCD2.Text = mse.ZipCD2;
            //    TxtAddress1.Text = mse.Address1;
            //    TxtAddress2.Text = mse.Address2;
            //    TxtTelePhoneNo.Text = mse.TelephoneNO;
            //    TxtFaxNo.Text = mse.FaxNO;
            //    CboSoukoType.SelectedValue = mse.SoukoType;
            //    ScMakerCD.Code = mse.MakerCD;
            //    TxtHikiateOrder.Text = mse.HikiateOrder;
            //    ChkUnitPriceCalcKBN.Checked = mse.UnitPriceCalcKBN.Equals("1") ? true : false;
            //    TxtIdouCount.Text = mse.IdouCount;
            //    TxtRemarks.Text = mse.Remarks;
            //    ChkDeleteFlg.Checked = mse.DeleteFlg.Equals("1") ? true : false;

            //    TxtSoukoName.Focus();

            //    return true;
            //}
            //else
            //{
            //    mtsbl.ShowMessage("E133");
            //    return false;
            //}

            return true;
        }

        private void InsertUpdate(int mode)
        {
            //*** Insert Update Function
            //if (mtsbl.M_Souko_Insert_Update(mse, mode))
            //{
            //    Clear(PanelHeader);
            //    Clear(PanelDetail);

            //    ChangeMode(OperationMode);
            //    ScSoukoCD.SetFocus(1);

            //    mtsbl.ShowMessage("I101");
            //}
            //else
            //{
            //    mtsbl.ShowMessage("S001");
            //}
        }

        private void Delete()
        {
            //*** Delete Function
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
                        if (!RequireCheck(new Control[] { ScNormal.TxtCode, ScNormal.TxtChangeDate }))
                            return false;

                        if (ScNormal.IsExists())
                        {
                            //***show Message mtsbl.ShowMessage("E132"); 
                            ScNormal.SetFocus(1);
                            return false;
                        }
                    }
                    else//Copy
                    {
                        if (!RequireCheck(new Control[] { ScNormal.TxtCode, ScNormal.TxtChangeDate }))
                            return false;

                        if (!ScCopy.IsExists())
                        {
                            //*** show Message mtsbl.ShowMessage("E133");
                            return false;
                        }
                    }
                }
                else
                {
                    if (!ScNormal.IsExists())
                    {
                        //*** Show Message mtsbl.ShowMessage("E133");
                        ScNormal.SetFocus(1);
                        return false;
                    }
                }
            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { ScNormal.TxtCode, ScNormal.TxtChangeDate}))
                    return false;

                if (OperationMode == EOperationMode.INSERT)
                {
                    if (ScNormal.IsExists())
                    {
                        //*** ShowMessage
                        //mtsbl.ShowMessage("E132");
                        ScNormal.SetFocus(1);
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
                    ScNormal.SearchEnable = false;
                    ScCopy.SearchEnable = true;
                    ScCopy.Enabled = true;
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
                    DisablePanel(PanelDetail);
                    ScNormal.SearchEnable = true;
                    ScCopy.Enabled = false;
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;                  
            }
            ScNormal.SetFocus(1);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void ScNormal_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                type = 1;
                F11();
            }
        }

        private void ScCopy_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                type = 2;
                F11();
            }
        }
    }
}
