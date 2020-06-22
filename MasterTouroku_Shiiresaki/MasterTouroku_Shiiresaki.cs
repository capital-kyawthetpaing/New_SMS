using System.Windows.Forms;
using BL;
using Entity;
using Search;
using Base.Client;
using CKM_Controls;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace MasterTouroku_Shiiresaki
{
    public partial class MasterTouroku_Shiiresaki : FrmMainForm
    {
        MasterTouroku_Shiiresaki_BL mtsbl;
        M_Bank_Entity mbe;
        M_Kouza_Entity mke;
        M_BankShiten_Entity mbse;
        M_Staff_Entity mse;
        M_Vendor_Entity mve;
        M_ZipCode_Entity mze;
        int type = 0;//1 = normal, 2 = copy (for f11)
        string z1 = "";
        string z2 = "";

        public MasterTouroku_Shiiresaki()
        {
            InitializeComponent();

            Load += new System.EventHandler(FormLoadEvent);
            PanelNormal.Enter += PanelNormal_Enter;
            PanelCopy.Enter += PanelCopy_Enter;
            ScVendor.Leave += ScNormal_Leave;
            this.KeyUp += Form_KeyUp;

            mtsbl = new MasterTouroku_Shiiresaki_BL();
            mbe = new M_Bank_Entity();
            mbse = new M_BankShiten_Entity();
            mke = new M_Kouza_Entity();
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void ScNormal_Leave(object sender, EventArgs e)
        {
            foreach (Control c in PanelDetail.Controls)
                if (c is CKM_SearchControl)
                {
                    CKM_SearchControl sc = c as CKM_SearchControl;
                    sc.ChangeDate = ScVendor.ChangeDate;
                }
        }

        private void FormLoadEvent(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            BindCombo();

            SetRequireField();

            SelectNextControl(PanelDetail, true, true, true, true);
            ScVendor.SetFocus(1);
           
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
            ScVendor.TxtCode.Require(true);
            ScVendor.TxtChangeDate.Require(true);
            //ScCopyVendor.TxtChangeDate.Require(true);
            txtVendorName.Require(true);
            txtVendorKana.Require(true);
            txtLongName1.Require(true);
            //ScPayeeCD.TxtCode.Require(true);
            //ScMoneyPayeeCD.TxtCode.Require(true);
            txtPaymentCloseDay.Require(true);
            cboPaymentKBN.Require(true);
            txtPaymentPlanDay.Require(true);

            txtVendorShortName.Require(true);        // Add By SawLay
            cboTaxTiming.Require(true);                   // Add By SawLay
            cboTaxFractionKBN.Require(true);         // Add By SawLay
            cboAmountFractionKBN.Require(true); // Add By SawLay
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
                        ScVendor.SetFocus(1);
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
                            //ScPayeeCD.TxtCode.Text = ScVendor.TxtCode.Text;            
                            //ScMoneyPayeeCD.TxtCode.Text = ScVendor.TxtCode.Text;
                            //mve = new M_Vendor_Entity();
                            //mve.VendorCD = ScVendor.TxtCode.Text;
                            //mve.ChangeDate = ScVendor.ChangeDate;
                            //DataTable dtpayee = new DataTable();
                            //dtpayee = mtsbl.Payee_Select(mve);
                            //if (dtpayee.Rows.Count > 0)
                            //{
                            //    ScPayeeCD.TxtCode.Text = ScVendor.TxtCode.Text;
                            //    ScPayeeCD.LabelText = dtpayee.Rows[0]["VendorName"].ToString();
                            //    ScMoneyPayeeCD.TxtCode.Text = ScVendor.TxtCode.Text;
                            //    ScMoneyPayeeCD.LabelText = dtpayee.Rows[0]["VendorName"].ToString();
                            //}
                            //SelectNextControl(PanelDetail, true, true, true, true);
                            ScCopyVendor.SetFocus(1);
                        }   
                        else
                        {
                            if (string.IsNullOrWhiteSpace(ScCopyVendor.ChangeDate) || (DisplayData(ScCopyVendor)))
                            {
                                if (!string.IsNullOrWhiteSpace(ScVendor.ChangeDate))
                                {
                                    mbe.ChangeDate = ScVendor.ChangeDate;
                                    mbe.BankCD = ScBankCD.TxtCode.Text;
                                    DataTable dtbank = new DataTable();
                                    dtbank = mtsbl.Bank_Select(mbe);
                                   if (dtbank.Rows.Count > 0)
                                    {
                                        ScBankCD.LabelText = dtbank.Rows[0]["BankName"].ToString();
                                        ScBranchCD.Value1 = ScBankCD.TxtCode.Text;
                                        ScBranchCD.Value2 = ScBankCD.LabelText;
                                    }
                                    mbse.ChangeDate = ScVendor.ChangeDate;
                                    mbse.BranchCD = ScBranchCD.TxtCode.Text;
                                    mbse.BankCD = ScBankCD.TxtCode.Text;
                                    DataTable dtbranch = new DataTable();
                                    dtbranch = mtsbl.BankShiten_Select(mbse);
                                    if (dtbranch.Rows.Count > 0)
                                    {
                                        ScBranchCD.LabelText = dtbranch.Rows[0]["BranchName"].ToString();
                                    }

                                    mke.ChangeDate = ScVendor.ChangeDate;
                                    mke.KouzaCD = ScKouzaCD.TxtCode.Text;
                                    DataTable dtKouza = new DataTable();
                                    dtKouza = mtsbl.Kouza_Select(mke);
                                    if (dtKouza.Rows.Count > 0)
                                    {
                                        ScKouzaCD.LabelText = dtKouza.Rows[0]["KouzaName"].ToString();
                                    }
                                }
                                DisablePanel(PanelHeader);
                                EnablePanel(PanelDetail);
                                btnDisplay.Enabled = F11Enable = false;
                                SelectNextControl(PanelDetail, true, true, true, true);

                            }                          
                        }
                        break;
                    case EOperationMode.UPDATE:
                        if (DisplayData(ScVendor))
                        {
                            DisablePanel(PanelNormal);
                            DisablePanel(PanelCopy);
                            EnablePanel(PanelDetail);
                            btnDisplay.Enabled = false;
                            F11Enable = false;
                            F12Enable = true;
                            SelectNextControl(PanelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData(ScVendor))
                        {                                                     
                            DisablePanel(PanelNormal);
                            DisablePanel(PanelCopy);
                            DisablePanel(PanelDetail);
                            F11Enable = false;
                            btnDisplay.Enabled = false;
                            F12Enable = true;
                            SelectNextControl(PanelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData(ScVendor))
                        {
                            DisablePanel(PanelNormal);
                            DisablePanel(PanelCopy);
                            DisablePanel(PanelDetail);
                            F11Enable = false;
                            btnDisplay.Enabled = false;
                            F12Enable = false;
                        }
                        break;
                }
                
            }
        }

        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (mtsbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                { 
                    mve = GetVendorEntity();
                    if (ScVendor.TxtCode.Text == ScPayeeCD.TxtCode.Text)
                    {
                        mve.PayeeFlg = "1";
                    }
                    else
                    {
                        mve.PayeeFlg = "0";
                    }
                    if (ScVendor.TxtCode.Text == ScPayeeCD.TxtCode.Text)
                    {
                        mve.MoneyPayeeFlg = "1";
                    }
                    else
                    {
                        mve.MoneyPayeeFlg = "0";
                    }
                    if (rdo1.Checked)
                    {
                        mve.HolidayKBN = "0";
                    }
                    else if(rdo2.Checked)
                    {
                        mve.HolidayKBN = "1";
                    }
                    else
                    {
                        mve.HolidayKBN = "2";
                    }
                    if(chkEDIFlg.Checked)
                    {
                        mve.EDIFlg = "1";
                    }
                    else if(chkEDIMail.Checked)
                    {
                        mve.EDIFlg = "2";
                    }
                    else
                    {
                        mve.EDIFlg = "0";
                    }
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

        private M_Vendor_Entity GetVendorEntity()
        {
            mve = new M_Vendor_Entity
            {
                VendorCD = ScVendor.TxtCode.Text,
                ChangeDate = ScVendor.ChangeDate,
                ShoguchiFlg = chkShouguchiFlg.Checked ? "1" : "0",
                VendorName = txtVendorName.Text,    //Add By SawLay
                VendorShortName = txtVendorShortName.Text,
                VendorKana = txtVendorKana.Text,
                VendorLongName1 = txtLongName1.Text,
                VendorLongName2 = txtLongName2.Text,
                VendorPostName = txtPostName.Text,
                VendorPositionName = txtPositionName.Text,
                VendorStaffName = txtVendorStaffName.Text,
                VendorFlg = "1",    //Add By SawLay
                ZipCD1 = txtZipCD1.Text,
                ZipCD2 = txtZipCD2.Text,
                Address1 = txtAddress1.Text,
                Address2 = txtAddress2.Text,
                MailAddress1 = txtMailAddress.Text,
                TelephoneNO = txtTelno.Text,
                FaxNO = txtFaxno.Text,
                PayeeCD = ScPayeeCD.TxtCode.Text,
                MoneyPayeeCD = ScMoneyPayeeCD.TxtCode.Text,
                PaymentCloseDay = txtPaymentCloseDay.Text,
                PaymentPlanKBN = cboPaymentKBN.SelectedValue.ToString(),
                PaymentPlanDay = txtPaymentPlanDay.Text,
                BankCD = ScBankCD.TxtCode.Text,
                BranchCD = ScBranchCD.TxtCode.Text,
                KouzaKBN = txtKouzaKBN.Text,
                KouzaNO = txtKouzaNo.Text,
                KouzaMeigi = txtKouzaMeigi.Text,
                KouzaCD = ScKouzaCD.TxtCode.Text,
                TaxTiming = cboTaxTiming.SelectedValue.ToString(),            //Add By SawLay
                TaxFractionKBN = cboTaxFractionKBN.SelectedValue.ToString(),     //Add By SawLay
                AmountFractionKBN = cboAmountFractionKBN.SelectedValue.ToString(),    //Add By SawLay
                NetFlg = chkNetFlg.Checked ? "1" : "0",
                EDIFlg = chkEDIFlg.Checked ? "1" : "0",   //Add By SawLay
                EDIMail = chkEDIMail.Checked ? "2" : "0",
                EDIVendorCD = txtRegisterNum.Text, //Add By SawLay
                StaffCD = ScStaffCD.TxtCode.Text,
                AnalyzeCD1 = txtAnalyzeCD1.Text ,
                AnalyzeCD2 = txtAnalyzeCD2.Text,
                AnalyzeCD3 = txtAnalyzeCD3.Text ,
                DisplayOrder = txtDisplayOrder.Text,
                DisplayNote = txtDisplayNote.Text ,
                NotDisplyNote = txtNotDisplay.Text,
                DeleteFlg = chkDelFlg.Checked ? "1":"0",
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID
            };
            return mve;
        }

        private bool DisplayData(CKM_SearchControl sc)
        {
            mve = new M_Vendor_Entity
            {
                VendorCD = sc.Code,
                ChangeDate = sc.ChangeDate
            };

            mve = mtsbl.M_Vendor_Select(mve);

            if (mve != null)
            {
                chkShouguchiFlg.Checked = mve.ShoguchiFlg.Equals("1") ? true : false;
                txtVendorName.Text = mve.VendorName;
                txtVendorShortName.Text = mve.VendorShortName;  //Add By SawLay
                txtVendorKana.Text = mve.VendorKana;
                txtLongName1.Text = mve.VendorLongName1;
                txtLongName2.Text = mve.VendorLongName2;
                txtPostName.Text = mve.VendorPostName;
                txtPositionName.Text = mve.VendorPositionName;
                txtVendorStaffName.Text = mve.VendorStaffName;
                txtZipCD1.Text = mve.ZipCD1;
                txtZipCD2.Text = mve.ZipCD2;
                txtAddress1.Text = mve.Address1;
                txtAddress2.Text = mve.Address2;
                txtMailAddress.Text = mve.MailAddress1;
                txtTelno.Text = mve.TelephoneNO;
                txtFaxno.Text = mve.FaxNO;
                ScPayeeCD.TxtCode.Text = mve.PayeeCD;
                ScPayeeCD.LabelText = mve.payeeName;
                ScMoneyPayeeCD.TxtCode.Text = mve.MoneyPayeeCD;
                ScMoneyPayeeCD.LabelText = mve.moneypayeeName;
                txtPaymentCloseDay.Text = mve.PaymentCloseDay;
                txtPaymentPlanDay.Text = mve.PaymentPlanDay;
                cboPaymentKBN.SelectedValue = mve.PaymentPlanKBN;
                rdo1.Checked = mve.HolidayKBN.Equals("0") ? true : false;
                rdo2.Checked = mve.HolidayKBN.Equals("1") ? true : false;
                rdo3.Checked = mve.HolidayKBN.Equals("2") ? true : false;
                cboTaxTiming.SelectedValue = mve.TaxTiming;   //Add By SawLay
                cboTaxFractionKBN.SelectedValue = mve.TaxFractionKBN;   //Add By SawLay
                cboAmountFractionKBN.SelectedValue = mve.AmountFractionKBN;   //Add By SawLay
                ScBankCD.TxtCode.Text = mve.BankCD;
                ScBankCD.LabelText = mve.BankName;
                ScBranchCD.Value1 = mve.BankCD;
                ScBranchCD.Value2 = mve.BankName;
                ScBranchCD.TxtCode.Text = mve.BranchCD;
                ScBranchCD.LabelText = mve.BranchName;
                txtKouzaKBN.Text = mve.KouzaKBN;
                txtKouzaNo.Text = mve.KouzaNO;
                txtKouzaMeigi.Text = mve.KouzaMeigi;
                ScKouzaCD.TxtCode.Text = mve.KouzaCD;
                ScKouzaCD.LabelText = mve.KouzaName;
                chkNetFlg.Checked = mve.NetFlg.Equals("1") ? true : false;
                chkEDIFlg.Checked = mve.EDIFlg.Equals("1") ? true : false; //Add By SawLay
                chkEDIMail.Checked = mve.EDIFlg.Equals("2") ? true : false;
                txtRegisterNum.Text = mve.EDIVendorCD; //Add By SawLay
                ScStaffCD.TxtCode.Text = mve.StaffCD;
                ScStaffCD.LabelText = mve.StaffName;
                txtAnalyzeCD1.Text = mve.AnalyzeCD1;
                txtAnalyzeCD2.Text = mve.AnalyzeCD2;
                txtAnalyzeCD3.Text = mve.AnalyzeCD3;
                txtDisplayOrder.Text = mve.DisplayOrder;
                txtDisplayNote.Text = mve.DisplayNote;
                txtNotDisplay.Text = mve.NotDisplyNote;
                chkDelFlg.Checked = mve.DeleteFlg.Equals("1") ? true : false;
                //txtVendorName.Focus();
                //chkShouguchiFlg.Focus();

                return true;
            }
            else
            {
                mtsbl.ShowMessage("E133");
                return false;
            }
          
        }

        private void InsertUpdate(int mode)
        {
            
            if(mtsbl.M_Vendor_InsertUpdate(mve,mode))
            {
                ChangeMode(OperationMode);
                ScVendor.SetFocus(1);
                mtsbl.ShowMessage("I101");
            }
            else
            {
                mtsbl.ShowMessage("S001");
            }
        }

        private void Delete()
        {
            if (mtsbl.M_Vendor_Delete(mve))
            {
                ChangeMode(OperationMode);
                ScVendor.SetFocus(1);
                mtsbl.ShowMessage("I101");
            }
            else
            {
                mtsbl.ShowMessage("S001");
            }
        }
        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 1)//New 
                    {
                        if (!RequireCheck(new Control[] { ScVendor.TxtCode, ScVendor.TxtChangeDate }))
                            return false;
                        //mve.VendorCD = ScVendor.TxtCode.Text;
                        //mve.ChangeDate = ScVendor.TxtChangeDate.Text;
                        if (ScVendor.IsExists(1))
                        {
                            mtsbl.ShowMessage("E132"); 
                            ScVendor.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            ScPayeeCD.TxtCode.Text = ScVendor.TxtCode.Text;
                            ScMoneyPayeeCD.TxtCode.Text = ScVendor.TxtCode.Text;
                        }

                    }
                    else//Copy
                    {
                        if (!RequireCheck(new Control[] { ScVendor.TxtCode, ScVendor.TxtChangeDate }))
                            return false;

                        if (!RequireCheck(new Control[] { ScCopyVendor.TxtCode }, ScCopyVendor.TxtChangeDate))
                            return false;

                        if (!RequireCheck(new Control[] { ScCopyVendor.TxtChangeDate }, ScCopyVendor.TxtCode))
                            return false;
                        
                        if (!string.IsNullOrWhiteSpace(ScCopyVendor.TxtCode.Text) && !string.IsNullOrWhiteSpace(ScCopyVendor.ChangeDate))
                        {
                            if (!ScCopyVendor.IsExists(1))
                            {
                                mtsbl.ShowMessage("E133");
                                ScCopyVendor.SetFocus(1);
                                return false;
                            }
                        }

                    }
                }
                else
                {
                    if (!ScVendor.IsExists(1))
                    {
                        mtsbl.ShowMessage("E133");
                        ScVendor.SetFocus(1);
                        return false;
                    }
                }
            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { ScVendor.TxtCode, ScVendor.TxtChangeDate }))
                    return false;

                if (string.IsNullOrWhiteSpace(txtVendorName.Text)) // Error 2
                {
                    mtsbl.ShowMessage("E102");
                    txtVendorName.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtVendorKana.Text)) // Error 3
                {
                    mtsbl.ShowMessage("E102");
                    txtVendorKana.Focus();
                    return false;
                }

                if(string.IsNullOrWhiteSpace(txtVendorShortName.Text)) //Add By SawLay ErrorCheck For 略名
                {
                    mtsbl.ShowMessage("E102");
                    txtVendorShortName.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtLongName1.Text)) // Error 4
                {
                    mtsbl.ShowMessage("E102");
                    txtLongName1.Focus();
                    return false;
                }
                
                if (!string.IsNullOrWhiteSpace(txtZipCD1.Text)) // Error 10
                {
                    if(string.IsNullOrWhiteSpace (txtZipCD2.Text))
                    {
                        mtsbl.ShowMessage("E102");
                        txtZipCD2.Focus();
                        return false;
                    }                  
                }
                
                if(ScPayeeCD.TxtCode.Text != ScVendor.TxtCode.Text) // Error15
                {
                    if (!RequireCheck(new Control[] { ScPayeeCD.TxtCode}))
                        return false;
                    else
                    {
                        mve.PayeeCD = ScPayeeCD.TxtCode.Text;
                        mve.ChangeDate = ScPayeeCD.ChangeDate;
                        DataTable dtpayee = new DataTable();
                        dtpayee = mtsbl.Payee_Select(mve);
                        if (dtpayee.Rows.Count == 0)
                        {
                            mtsbl.ShowMessage("E101");
                            ScPayeeCD.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            //ScPayeeCD.LabelText = dtpayee.Rows[0]["VendorName"].ToString();
                            ScPayeeCD.LabelText = txtVendorName.Text;
                        }
                    }
                }
                else
                {
                    ScPayeeCD.LabelText = txtVendorName.Text;
                }

                if(ScMoneyPayeeCD.TxtCode.Text != ScVendor.TxtCode.Text) // Error16
                {
                    if (!RequireCheck(new Control[] { ScMoneyPayeeCD.TxtCode }))
                        return false;
                    else
                    {
                        mve.MoneyPayeeCD = ScMoneyPayeeCD.TxtCode.Text;
                        mve.ChangeDate = ScMoneyPayeeCD.ChangeDate;
                        DataTable dtpayee = new DataTable();
                        dtpayee = mtsbl.Payee_Select(mve);
                        if (dtpayee.Rows.Count == 0)
                        {
                            mtsbl.ShowMessage("E101");
                            ScMoneyPayeeCD.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            //ScMoneyPayeeCD.LabelText = dtpayee.Rows[0]["VendorName"].ToString();
                            ScMoneyPayeeCD.LabelText = txtVendorName.Text;
                        }
                    }
                }
                else
                {
                    ScMoneyPayeeCD.LabelText = txtVendorName.Text;
                }

                if (string.IsNullOrWhiteSpace(txtPaymentCloseDay.Text)) //Error 17
                {
                    mtsbl.ShowMessage("E102");
                    txtPaymentCloseDay.Focus();
                    return false;
                }
                else
                {
                    if ((Convert.ToInt32(txtPaymentCloseDay.Text) < 1) && (Convert.ToInt32(txtPaymentCloseDay.Text) > 31))
                    {
                        mtsbl.ShowMessage("E115");
                        txtPaymentCloseDay.Focus();
                        return false;
                    }
                }

                //if (string.IsNullOrWhiteSpace(cboPaymentKBN.SelectedValue.ToString()) || (cboPaymentKBN.SelectedValue.Equals(-1)))
                if (string.IsNullOrWhiteSpace(cboPaymentKBN.Text.ToString())) // Error18
                {
                    mtsbl.ShowMessage("E102");
                    cboPaymentKBN.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtPaymentPlanDay.Text)) //Error19
                {
                    mtsbl.ShowMessage("E102");
                    txtPaymentPlanDay.Focus();
                    return false;
                }
                else
                {
                    if ((Convert.ToInt32(txtPaymentPlanDay.Text) < 1) && (Convert.ToInt32(txtPaymentPlanDay.Text) > 31))
                    {
                        mtsbl.ShowMessage("E115");
                        txtPaymentPlanDay.Focus();
                        return false;
                    }
                }

                if(cboTaxTiming.SelectedValue.Equals(0)) //Add By SawLay   ErrorCheck for 消費税計算
                {
                    mtsbl.ShowMessage("E102");
                    cboTaxTiming.Focus();
                    return false;
                }

                if (cboTaxFractionKBN.SelectedValue.Equals(0)) //Add By SawLay   ErrorCheck for 消費税端数処理
                {
                    mtsbl.ShowMessage("E102");
                    cboTaxFractionKBN.Focus();
                    return false;
                }

                if (cboAmountFractionKBN.SelectedValue.Equals(0)) //Add By SawLay   ErrorCheck for 金額計算端数処理
                {
                    mtsbl.ShowMessage("E102");
                    cboAmountFractionKBN.Focus();
                    return false;
                }

                if (ScVendor.TxtCode.ToString () == ScMoneyPayeeCD.TxtCode.ToString ())
                {
                    if (!RequireCheck(new Control[] { ScBankCD.TxtCode})) // Error21
                        return false;
                    else
                    {
                        if (!ScBankCD.SelectData())
                        {
                            mtsbl.ShowMessage("E101");
                            ScBankCD.SetFocus(1);
                            return false;
                        }
                    }
                    
                    if (!RequireCheck(new Control[] { ScBranchCD.TxtCode })) // Error22
                        return false;
                    else
                    {
                        if (!ScBranchCD.SelectData())
                        {
                            mtsbl.ShowMessage("E101");
                            ScBranchCD.SetFocus(1);
                            return false;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(txtKouzaKBN.Text)) // Error 23
                    {
                        mtsbl.ShowMessage("E102"); 
                        txtKouzaKBN.Focus();
                        return false;
                    }
                    else 
                    {
                        if ((Convert.ToInt32(txtKouzaKBN.Text) < 1) || (Convert.ToInt32(txtKouzaKBN.Text) > 2))
                        {
                            mtsbl.ShowMessage("E101");
                            txtKouzaKBN.Focus();
                            return false;
                        }   
                    }

                    if (string.IsNullOrWhiteSpace(txtKouzaNo.Text)) // Error24
                    {
                        mtsbl.ShowMessage("E102");
                        txtKouzaNo.Focus();
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(txtKouzaMeigi.Text)) // Error25
                    {
                        mtsbl.ShowMessage("E102");
                        txtKouzaMeigi.Focus();
                        return false;
                    }
                    else
                    {
                        var sjis = System.Text.Encoding.GetEncoding("shift_JIS");
                        int byteCount = sjis.GetByteCount(txtKouzaMeigi.Text);
                        if (txtKouzaMeigi.Text.Length != byteCount)
                        {
                            mtsbl.ShowMessage("E118");
                            txtKouzaMeigi.Focus();
                            return false;
                        }
                    }

                    //if(chkEDIFlg.Checked == true) //Add By SawLay   ErrorCheck for EDI会社番号
                    //{
                    //    if(string.IsNullOrWhiteSpace(txtRegisterNum.Text))
                    //    {
                    //        mtsbl.ShowMessage("E102");
                    //        txtRegisterNum.Focus();
                    //        return false;
                    //    }
                    //}

                    if (!RequireCheck(new Control[] { ScKouzaCD.TxtCode })) // Error26
                        return false;
                    else
                    {
                        mke = new M_Kouza_Entity();
                        mke.KouzaCD = ScKouzaCD.TxtCode.Text;
                        mke.ChangeDate = ScVendor.ChangeDate;
                        DataTable dtkouza = new DataTable();
                        dtkouza = mtsbl.Kouza_Select(mke);
                        if (dtkouza.Rows.Count == 0)
                        {
                            mtsbl.ShowMessage("E101");
                            ScKouzaCD.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            if (dtkouza.Rows[0]["DeleteFlg"].ToString() == "1")
                            {
                                mtsbl.ShowMessage("E158");
                                ScKouzaCD.SetFocus(1);
                                return false;
                            }
                            else
                            {
                                ScKouzaCD.LabelText = dtkouza.Rows[0]["KouzaName"].ToString();
                            }
                        }
                    }
                   
                }

                if (chkEDIFlg.Checked == true) //Add By SawLay   ErrorCheck for EDI会社番号
                {
                    if (string.IsNullOrWhiteSpace(txtRegisterNum.Text))
                    {
                        mtsbl.ShowMessage("E102");
                        txtRegisterNum.Focus();
                        return false;
                    }
                }


                if (!string.IsNullOrWhiteSpace(ScStaffCD.TxtCode.Text)) // Error 28
                {
                    mse = new M_Staff_Entity();
                    mse.StaffCD = ScStaffCD.TxtCode.Text;
                    mse.ChangeDate = ScVendor.ChangeDate;
                    DataTable dtStaff = new DataTable();
                    dtStaff = mtsbl.Staff_Select(mse);
                    if (dtStaff.Rows.Count == 0)
                    {
                        mtsbl.ShowMessage("E101");
                        ScStaffCD.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        string date = dtStaff.Rows[0]["LeaveDate"].ToString();
                        int result = date.CompareTo(ScVendor.ChangeDate);
                        if (!string.IsNullOrWhiteSpace(date) && result <= 0)
                        {
                            mtsbl.ShowMessage("E135");
                            ScStaffCD.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            if (dtStaff.Rows[0]["DeleteFlg"].ToString() == "1")
                            {
                                mtsbl.ShowMessage("E158");
                                ScStaffCD.SetFocus(1);
                                return false;
                            }
                            else
                            {
                                ScStaffCD.LabelText = dtStaff.Rows[0]["StaffName"].ToString();
                            }
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(txtDisplayOrder.Text)) // Error 30
                {
                    txtDisplayOrder.Text = "0";
                }
                
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (ScVendor.IsExists(1))
                    {
                        mtsbl.ShowMessage("E132");
                        ScVendor.SetFocus(1);
                        return false;
                    }                  
                }               
            }
            return true;
        }

        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    Clear(PanelNormal);
                    Clear(PanelCopy);
                    Clear(PanelDetail);
                    EnablePanel(PanelNormal);
                    EnablePanel(PanelCopy);
                    DisablePanel(PanelDetail);
                    ScVendor.SearchEnable = false;
                    ScCopyVendor.SearchEnable = true;
                    F9Visible = false;
                    F12Enable = true;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelNormal);
                    Clear(PanelCopy);
                    Clear(PanelDetail);
                    EnablePanel(PanelNormal);
                    DisablePanel(PanelCopy);
                    DisablePanel(PanelDetail);
                    ScVendor.SearchEnable = true;
                    ScCopyVendor.SearchEnable = false;
                    F9Visible = true;
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
            }
            ScVendor.SetFocus(1);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void BindCombo()
        {
            cboPaymentKBN.Bind(string.Empty);
            BindTaxTiming();     //Add By SawLay
            BindTaxFractionKBN();     //Add By SawLay
            BindAmountFractionKBN();     //Add By SawLay
        }
        private void BindTaxTiming()     //Add By SawLay
        {
            DataTable dtT = new DataTable();
            dtT.Columns.Add("TaxTimeId", typeof(int));
            dtT.Columns.Add("TaxTimeName", typeof(string));
            dtT.Rows.Add(0, string.Empty);
            dtT.Rows.Add(1, "明細単位");
            dtT.Rows.Add(2, "伝票単位");
            dtT.Rows.Add(3, "締単位");

            cboTaxTiming.ValueMember = "TaxTimeId";
            cboTaxTiming.DisplayMember = "TaxTimeName";
            cboTaxTiming.DataSource = dtT;
        }
        private void BindTaxFractionKBN()     //Add By SawLay
        {
            DataTable dtF = new DataTable();
            dtF.Columns.Add("TaxFractionId",typeof(int));
            dtF.Columns.Add("TaxFractionName",typeof(string));
            dtF.Rows.Add(0, string.Empty);
            dtF.Rows.Add(1, "切捨て");
            dtF.Rows.Add(2, "四捨五入");
            dtF.Rows.Add(3, "切上げ");

            cboTaxFractionKBN.ValueMember = "TaxFractionId";
            cboTaxFractionKBN.DisplayMember = "TaxFractionName";
            cboTaxFractionKBN.DataSource = dtF;
        }
        private void BindAmountFractionKBN()     //Add By SawLay
        {
            DataTable dtA = new DataTable();
            dtA.Columns.Add("AmountFractionId", typeof(int));
            dtA.Columns.Add("AmountFractionName", typeof(string));
            dtA.Rows.Add(0, string.Empty);
            dtA.Rows.Add(1, "切捨て");
            dtA.Rows.Add(2, "四捨五入");
            dtA.Rows.Add(3, "切上げ");

            cboAmountFractionKBN.ValueMember = "AmountFractionId";
            cboAmountFractionKBN.DisplayMember = "AmountFractionName";
            cboAmountFractionKBN.DataSource = dtA;
        }

        private void txtZipCD2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(!String.IsNullOrWhiteSpace(txtZipCD1.Text))
                {                  
                    if (string.IsNullOrWhiteSpace(txtZipCD2.Text))
                    {
                        mtsbl.ShowMessage("E102");
                        txtZipCD2.Focus();
                    }
                    else
                    {
                        if (OperationMode == EOperationMode.UPDATE)
                        {
                            if (z1 != txtZipCD1.Text || z2 != txtZipCD2.Text)
                            {
                                mze = new M_ZipCode_Entity();
                                mze.ZipCD1 = txtZipCD1.Text;
                                mze.ZipCD2 = txtZipCD2.Text;

                                DataTable dtzip = new DataTable();
                                dtzip = mtsbl.M_ZipCode_Select(mze);

                                if (dtzip.Rows.Count > 0)
                                {
                                    txtAddress1.Text = dtzip.Rows[0]["Address1"].ToString();
                                    txtAddress2.Text = dtzip.Rows[0]["Address2"].ToString();
                                }
                                else
                                {
                                    txtAddress1.Text = string.Empty;
                                    txtAddress2.Text = string.Empty;
                                }
                            }
                            z1 = txtZipCD1.Text;
                            z2 = txtZipCD2.Text;
                        }
                        else if (OperationMode == EOperationMode.INSERT)
                        {
                            mze = new M_ZipCode_Entity();
                            mze.ZipCD1 = txtZipCD1.Text;
                            mze.ZipCD2 = txtZipCD2.Text;

                            DataTable dtzip = new DataTable();
                            dtzip = mtsbl.M_ZipCode_Select(mze);

                            if (dtzip.Rows.Count > 0)
                            {
                                txtAddress1.Text = dtzip.Rows[0]["Address1"].ToString();
                                txtAddress2.Text = dtzip.Rows[0]["Address2"].ToString();
                            }
                            else
                            {
                                txtAddress1.Text = string.Empty;
                                txtAddress2.Text = string.Empty;
                            }
                        }
                    } 
                }
            }
        }
        
        private void ScVendor_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                type = 1;
                F11();
                if (OperationMode == EOperationMode.UPDATE)
                {
                    if (!string.IsNullOrEmpty(txtZipCD1.Text))
                    {
                        z1 = txtZipCD1.Text;
                        z2 = txtZipCD2.Text;
                    }
                }
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }

        private void ScCopyVendor_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {               
                type = 2;
                F11();
                //chkShouguchiFlg.Focus();
            }
        }

        private void ScPayeeCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ScPayeeCD.TxtCode.ToString() != ScVendor.TxtCode.ToString())
                {
                    if (string.IsNullOrWhiteSpace(ScPayeeCD.TxtCode.Text))
                    {
                        mtsbl.ShowMessage("E102");
                        ScPayeeCD.SetFocus(1);
                    }
                    else
                    {
                        mve = new M_Vendor_Entity();
                        mve.PayeeCD = ScPayeeCD.TxtCode.Text;
                        mve.ChangeDate = ScPayeeCD.ChangeDate;
                        DataTable dtpayee = new DataTable();
                        dtpayee = mtsbl.Payee_Select(mve);
                        if (dtpayee.Rows.Count == 0)
                        {
                            mtsbl.ShowMessage("E101");
                            ScPayeeCD.SetFocus(1);
                        }
                        else
                        {
                            ScPayeeCD.LabelText = dtpayee.Rows[0]["VendorName"].ToString();
                            //ScPayeeCD.LabelText = txtVendorName.Text;
                        }
                        
                    }
                }
                else
                {
                    ScPayeeCD.LabelText = txtVendorName.Text;
                }
            }
        }
       
        private void ScMoneyPayeeCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ScMoneyPayeeCD.TxtCode.ToString() != ScVendor.TxtCode.ToString())
                {
                    if (string.IsNullOrWhiteSpace(ScMoneyPayeeCD.TxtCode.Text))
                    {
                        mtsbl.ShowMessage("E102");
                        ScMoneyPayeeCD.SetFocus(1);
                    }
                    else
                    {
                        mve = new M_Vendor_Entity();
                        mve.MoneyPayeeCD = ScMoneyPayeeCD.TxtCode.Text;
                        mve.ChangeDate = ScMoneyPayeeCD.ChangeDate;
                        DataTable dtpayee = new DataTable();
                        dtpayee = mtsbl.MoneyPayee_Select(mve);
                        if (dtpayee.Rows.Count == 0)
                        {
                            mtsbl.ShowMessage("E101");
                            ScMoneyPayeeCD.SetFocus(1);
                        }
                        else
                        {
                            ScMoneyPayeeCD.LabelText = dtpayee.Rows[0]["VendorName"].ToString();
                            //ScMoneyPayeeCD.LabelText = txtVendorName.Text;
                        }
                    }
                }
                else
                {
                    ScMoneyPayeeCD.LabelText = txtVendorName.Text;
                }
            }
        }

        private void txtPaymentCloseDay_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(string.IsNullOrWhiteSpace(txtPaymentCloseDay.Text))
                {
                    mtsbl.ShowMessage("E102");
                    ScPayeeCD.SetFocus(1);
                }
                else
                {   
                    if ((Convert.ToInt32(txtPaymentCloseDay.Text) < 1) || (Convert.ToInt32(txtPaymentCloseDay.Text) > 31))
                    {
                        mtsbl.ShowMessage("E115");
                        txtPaymentCloseDay.Focus();
                       
                    }
                }
            }
            
        }

        private void txtPaymentPlanDay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtPaymentPlanDay.Text))
                {
                    mtsbl.ShowMessage("E102");
                    ScPayeeCD.SetFocus(1);
                }
                else
                {
                    if ((Convert.ToInt32(txtPaymentPlanDay.Text) < 1) || (Convert.ToInt32(txtPaymentPlanDay.Text) > 31))
                    {
                        mtsbl.ShowMessage("E115");
                        txtPaymentPlanDay.Focus();
                        
                    }
                }
            }
        }

        private void txtKouzaKBN_KeyDown(object sender, KeyEventArgs e)

        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ScVendor.TxtCode.ToString() == ScMoneyPayeeCD.TxtCode.ToString())
                {
                    if (string.IsNullOrWhiteSpace(txtKouzaKBN.Text)) // Error 23
                    {
                        mtsbl.ShowMessage("E102");
                        txtKouzaKBN.Focus();

                    }
                }
                if (!string.IsNullOrWhiteSpace(txtKouzaKBN.Text))
                {
                    if ((Convert.ToInt32(txtKouzaKBN.Text) < 1) || (Convert.ToInt32(txtKouzaKBN.Text) > 2))
                    {
                        mtsbl.ShowMessage("E101");
                        txtKouzaKBN.Focus();

                    }
                }
            }
        }

        private void txtKouzaMeigi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ScVendor.TxtCode.ToString() == ScMoneyPayeeCD.TxtCode.ToString())
                {
                    if (string.IsNullOrWhiteSpace(txtKouzaMeigi.Text)) // Error25
                    {
                        mtsbl.ShowMessage("E102");
                        txtKouzaMeigi.Focus();
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtKouzaMeigi.Text))
                {
                    var sjis = System.Text.Encoding.GetEncoding("shift_JIS");
                    int byteCount = sjis.GetByteCount(txtKouzaMeigi.Text);
                    if (txtKouzaMeigi.Text.Length != byteCount)
                    {
                        mtsbl.ShowMessage("E118");
                        txtKouzaMeigi.Focus();
                    }
                }
            }
        }

        private void txtDisplayOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtDisplayOrder.Text)) // Error 30
                {
                    txtDisplayOrder.Text = "0";
                }
            }
        }

        private void ScBankCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ScVendor.TxtCode.ToString() == ScMoneyPayeeCD.TxtCode.ToString())
                {
                    if (string.IsNullOrWhiteSpace(ScBankCD.TxtCode.Text))
                    {
                        mtsbl.ShowMessage("E102");
                        ScBankCD.SetFocus(1);
                    }
                    else
                    {
                        if (ScBankCD.SelectData())
                        {
                            ScBranchCD.Value1 = ScBankCD.TxtCode.Text;
                            ScBranchCD.Value2 = ScBankCD.LabelText;
                        }
                        else
                        {
                            mtsbl.ShowMessage("E101");
                            ScBankCD.SetFocus(1);
                        }
                    }

                }

            }
        }

        private void ScBranchCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ScVendor.TxtCode.ToString() == ScMoneyPayeeCD.TxtCode.ToString())
                {
                    if (string.IsNullOrWhiteSpace(ScBranchCD.TxtCode.Text))
                    {
                        mtsbl.ShowMessage("E102");
                        ScBranchCD.SetFocus(1);
                    }
                    else
                    {
                        if (!ScBranchCD.SelectData())
                        {
                            mtsbl.ShowMessage("E101");
                            ScBranchCD.SetFocus(1);
                        }
                    }                   
                }
            }
        }

        private void ScKouzaCD_CodeKeyDownEvent(object sender, KeyEventArgs e)

        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ScVendor.TxtCode.ToString() == ScMoneyPayeeCD.TxtCode.ToString())
                {
                    if (string.IsNullOrWhiteSpace(ScKouzaCD.TxtCode.Text))
                    {
                        mtsbl.ShowMessage("E102");
                        ScKouzaCD.SetFocus(1);
                    }
                    else
                    {
                        if (!ScKouzaCD.SelectData())
                        {
                            mtsbl.ShowMessage("E101");
                            ScKouzaCD.SetFocus(1);
                        }
                        else
                        {
                            mke = new M_Kouza_Entity();
                            mke.KouzaCD = ScKouzaCD.TxtCode.Text;
                            mke.ChangeDate = ScKouzaCD.ChangeDate;
                            DataTable dtkouza = new DataTable();
                            dtkouza = mtsbl.Kouza_Select(mke);
                            if (dtkouza.Rows.Count > 0)
                            {
                                if (dtkouza.Rows[0]["DeleteFlg"].ToString() == "1")
                                {
                                    mtsbl.ShowMessage("E158");
                                    ScKouzaCD.SetFocus(1);
                                }
                                else
                                {
                                    ScKouzaCD.LabelText = dtkouza.Rows[0]["KouzaName"].ToString();
                                }
                            }
                        }
                    }
                }
                ScKouzaCD.TxtChangeDate.Text = ScVendor.TxtChangeDate.Text;
            }
        }

        private void ScStaffCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScStaffCD.TxtCode.Text))
                {
                    
                    if (!ScStaffCD.SelectData())
                    {
                        mtsbl.ShowMessage("E101");
                        ScStaffCD.SetFocus(1);
                    }
                    else
                    {
                        mse = new M_Staff_Entity();
                        mse.StaffCD = ScStaffCD.TxtCode.Text;
                        mse.ChangeDate = ScStaffCD.ChangeDate;
                        DataTable dtStaff = new DataTable();
                        dtStaff = mtsbl.Staff_Select(mse);
                        if(dtStaff.Rows.Count > 0)
                        {
                            string date = dtStaff.Rows[0]["LeaveDate"].ToString();
                            int result = date.CompareTo(ScVendor.ChangeDate);
                            if (!string.IsNullOrWhiteSpace(date) && result <= 0)
                            {
                                mtsbl.ShowMessage("E135");
                                ScStaffCD.SetFocus(1);
                            }
                            else
                            {
                                if (dtStaff.Rows[0]["DeleteFlg"].ToString() == "1")
                                {
                                    mtsbl.ShowMessage("E158");
                                    ScStaffCD.SetFocus(1);
                                }
                                else
                                {
                                    ScStaffCD.LabelText = dtStaff.Rows[0]["StaffName"].ToString();
                                }
                            }
                        }    
                    }
                }
               
            }
        }

        private void ScBankCD_Leave(object sender, EventArgs e)
        {
            ScBranchCD.Value1 = ScBankCD.Code;
            ScBranchCD.Value2 = ScBankCD.LabelText;
        }

        private void txtKouzaNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ScVendor.TxtCode.ToString() == ScMoneyPayeeCD.TxtCode.ToString())
                {
                    if (string.IsNullOrWhiteSpace(txtKouzaNo.Text)) // Error 23
                    {
                        mtsbl.ShowMessage("E102");
                        txtKouzaNo.Focus();
                    }
                }
            }
        }

        private void txtVendorShortName_KeyDown(object sender, KeyEventArgs e) //Add By SawLay
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(string.IsNullOrWhiteSpace(txtVendorShortName.Text))
                {
                    mtsbl.ShowMessage("E102");
                    txtVendorShortName.Focus();
                }
            }
        }
      
        private void chkEDIMail_CheckedChanged(object sender, EventArgs e)
        {
            if(chkEDIMail.Checked == true)
            {
                chkEDIFlg.Checked = false;
            }
        }

        private void chkEDIFlg_CheckedChanged(object sender, EventArgs e)
        {
            if(chkEDIFlg.Checked == true)
            {
                chkEDIMail.Checked = false;
            }
        }

        private void txtRegisterNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (chkEDIFlg.Checked == true)
                {
                    if (string.IsNullOrEmpty(txtRegisterNum.Text))
                    {
                        mtsbl.ShowMessage("E102");
                        txtRegisterNum.Focus();
                    }
                }
            }
        }
    }
}
