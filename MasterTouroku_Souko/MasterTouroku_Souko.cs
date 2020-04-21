using System.Windows.Forms;
using BL;
using Entity;
using Search;
using Base.Client;
using CKM_Controls;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;


namespace MasterTouroku_Souko
{
    public partial class FrmMasterTouroku_Souko : FrmMainForm
    {
        //***Declare Entity
        //***Declare BL
        int type = 0;//1 = normal, 2 = copy (for f11)
        DataTable dtLocation;
        MasterTouroku_Souko_BL mtsbl;
        M_Souko_Entity mse;
        M_ZipCode_Entity mze;
        string z1 = "";
        string z2 = "";
        public FrmMasterTouroku_Souko()
        {
            InitializeComponent();
            Load += new System.EventHandler(FormLoadEvent);
            PanelNormal.Enter += PanelNormal_Enter;
            PanelCopy.Enter += PanelCopy_Enter;
            ScSoukoCD.Leave += ScNormal_Leave;
            KeyUp += Form_KeyUp;
            //DisablePanel(PanelDetail);
            //*** bl = new bl();

        }
        private void FormLoadEvent(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            BindCombo();
            BindLocation();
            SetRequireField();
            SelectNextControl(PanelDetail, true, true, true, true);
            ScSoukoCD.SetFocus(1);

            GvTana.DuplicateCheckCol = new string[] { "colTanaCD" };
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
                    sc.ChangeDate = ScSoukoCD.ChangeDate;
                }
        }       
        public override void FunctionProcess(int index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
            mtsbl = new MasterTouroku_Souko_BL();            
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
                        ScSoukoCD.SetFocus(1);
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
        private M_Souko_Entity GetSoukoEntity()
        {
            mse = new M_Souko_Entity()
            {
                SoukoCD = ScSoukoCD.Code,
                ChangeDate = ScSoukoCD.ChangeDate,
                SoukoName = TxtSoukoName.Text,
                StoreCD = CboStoreCD.SelectedValue.ToString(),
                HikiateOrder = TxtHikiateOrder.Text,
                ZipCD1 = txtZipCD1.Text,
                ZipCD2 = txtZipCD2.Text,
                Address1 = TxtAddress1.Text,
                Address2 = TxtAddress2.Text,
                TelephoneNO = TxtTelePhoneNo.Text,
                FaxNO = TxtFaxNo.Text,
                SoukoType = CboSoukoType.SelectedValue.ToString(),
                MakerCD = ScSoukoMakerCD.Code,                
                UnitPriceCalcKBN = ChkUnitPriceCalcKBN.Checked ? "1" : "0",
                IdouCount = TxtIdouCount.Text,
                Remarks = TxtRemark.Text,
                DeleteFlg = ChkDeleteFlg.Checked ? "1" : "0",
                ProcessMode = ModeText,
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                Key = ScSoukoCD.Code + " " + ScSoukoCD.ChangeDate,
                PC = InPcID,
                dtTemp1 = dtLocation
              

        };        
            

            return mse;
        }
        private bool DisplayData(CKM_SearchControl sc)
        {
            mse = new M_Souko_Entity
            {
                SoukoCD = sc.Code,
                ChangeDate = sc.ChangeDate
            };

            dtLocation = mtsbl.M_Location_Select(mse);
            if (dtLocation != null)
                GvTana.DataSource = dtLocation;

            mse = mtsbl.M_Souko_Select(mse);
            if (mse != null)
            {
                TxtSoukoName.Text = mse.SoukoName;
                CboStoreCD.SelectedValue = mse.StoreCD;
                txtZipCD1.Text = mse.ZipCD1;
                txtZipCD2.Text = mse.ZipCD2;
                TxtAddress1.Text = mse.Address1;
                TxtAddress2.Text = mse.Address2;
                TxtTelePhoneNo.Text = mse.TelephoneNO;
                TxtFaxNo.Text = mse.FaxNO;
                CboSoukoType.SelectedValue = mse.SoukoType;
                ScSoukoMakerCD.Code = mse.MakerCD;
                ScSoukoMakerCD.LabelText = mse.MakerName;
                TxtHikiateOrder.Text = mse.HikiateOrder;
                ChkUnitPriceCalcKBN.Checked = mse.UnitPriceCalcKBN.Equals("1") ? true : false;
                TxtIdouCount.Text = mse.IdouCount;
                TxtRemark.Text = mse.Remarks;
                ChkDeleteFlg.Checked = mse.DeleteFlg.Equals("1") ? true : false;
                TxtSoukoName.Focus();
                return true;
            }
            else
            {
                mtsbl.ShowMessage("E133");
                return false;
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
                    EnablePanel(PanelHeader);
                    //GvTana.Enabled = false;
                    DisablePanel(PanelDetail);
                    ScSoukoCD.SearchEnable = false;
                    ScCopySoukoCD.SearchEnable = true;
                    ScCopySoukoCD.Enabled = true;
                    F9Visible = false;
                    F12Enable = true;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    //GvTana.Enabled = false;
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    ScSoukoCD.SearchEnable = true;
                    ScCopySoukoCD.Enabled = false;
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
            }
            ScSoukoCD.SetFocus(1);
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }            

  /// <summary>
/// F2 & F3 Mode
 /// </summary>
/// <param name="mode"></param>
#region Function 11 & 12
        private void F11()
        {

            if (ErrorCheck(11))
            {
                switch (OperationMode)
                {
                    case EOperationMode.INSERT:

                        if (type == 1)
                            ScCopySoukoCD.SetFocus(1);
                        else
                        {
                            if (string.IsNullOrWhiteSpace(ScCopySoukoCD.TxtChangeDate.Text) || DisplayData(ScCopySoukoCD))
                            {
                                DisablePanel(PanelHeader);
                                EnablePanel(PanelDetail);
                                GvTana.Enabled = true;
                                F11Enable = false;
                                btnDisplay.Enabled = false;
                                TxtSoukoName.Focus();
                            }

                        }


                        break;
                    case EOperationMode.UPDATE:
                        if (DisplayData(ScSoukoCD))
                        {
                            DisablePanel(PanelHeader);
                            EnablePanel(PanelDetail);
                            F12Enable = true;
                            F11Enable = false;
                            GvTana.Enabled = true;
                            btnDisplay.Enabled = false;
                            SelectNextControl(PanelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData(ScSoukoCD))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            F11Enable = false;
                            btnDisplay.Enabled = false;
                            SelectNextControl(PanelDetail, true, true, true, true);
                            F12Enable = true;
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData(ScSoukoCD))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            F11Enable = false;
                            btnDisplay.Enabled = false;
                            F12Enable = false;
                        }
                        break;
                }
                CustomEnable();
                //***Add Control Enable/Disable;
            }
        }
        private void F12()
        {
            if (ErrorCheck(12))
            {

                //*** Create Entity Object
                mse = GetSoukoEntity();
                if (bbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
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

 #endregion

#region ErrorCheck Function
        private bool ErrorCheck(int index)
        {

            if (index == 11)
            {               
                //HeaderCheck on F11
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 1)//New 
                    {
                        if (!RequireCheck(new Control[] { ScSoukoCD.TxtCode, ScSoukoCD.TxtChangeDate }))
                            return false;

                        if (ScSoukoCD.IsExists(1))
                        {
                            //***show Message mtsbl.ShowMessage("E132"); 
                            mtsbl.ShowMessage("E132");
                            ScSoukoCD.SetFocus(1);
                            return false;
                        }
                    }
                    else//Copy
                    {
                        if (!RequireCheck(new Control[] { ScSoukoCD.TxtCode, ScSoukoCD.TxtChangeDate }))
                            return false;

                        if (!RequireCheck(new Control[] { ScCopySoukoCD.TxtCode }, ScCopySoukoCD.TxtChangeDate))
                            return false;

                        if (!RequireCheck(new Control[] { ScCopySoukoCD.TxtChangeDate }, ScCopySoukoCD.TxtCode))
                            return false;

                        if (ScSoukoCD.IsExists(1))
                        {
                            mtsbl.ShowMessage("E132");
                            ScSoukoCD.SetFocus(1);
                            return false;
                        }

                        if (!string.IsNullOrWhiteSpace(ScCopySoukoCD.TxtCode.Text) && !string.IsNullOrWhiteSpace(ScCopySoukoCD.TxtChangeDate.Text))
                        {
                            if (!ScCopySoukoCD.IsExists(1))
                            {
                                //*** show Message
                                mtsbl.ShowMessage("E133");
                                ScCopySoukoCD.SetFocus(1);
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if (!RequireCheck(new Control[] { ScSoukoCD.TxtCode, ScSoukoCD.TxtChangeDate }))
                      return false;
                    //if (!ScSoukoCD.IsExists(1))
                    //{
                    //    //*** Show Message
                    //    mtsbl.ShowMessage("E133");
                    //    ScSoukoCD.SetFocus(1);
                    //    return false;
                    //}
                }
            }
            else if (index == 12)
            {

                if (!RequireCheck(new Control[] { ScSoukoCD.TxtCode, ScSoukoCD.TxtChangeDate, TxtSoukoName, CboStoreCD}))
                    return false;

                if (!RequireCheck(new Control[] { txtZipCD1 }, txtZipCD2))
                    return false;

                if (!RequireCheck(new Control[] { txtZipCD2 }, txtZipCD1))
                    return false;

                //if (!CheckZipCD(txtZipCD1))
                //    return false;

                if (!CheckZipCDEvent())
                    return false;

                if (!RequireCheck(new Control[] { CboSoukoType }))
                    return false;

                if (ScSoukoMakerCD.Enabled.Equals(true)) //△
                {
                    if (!RequireCheck(new Control[] { ScSoukoMakerCD.TxtCode }))
                        return false;
                }
               
                if (!MakerCheck())
                    return false;

                if (!TanaCDCheck())
                    return false;          

                if (OperationMode == EOperationMode.INSERT)
                {
                    if (ScSoukoCD.IsExists(1))
                    {
                        //*** ShowMessage
                        mtsbl.ShowMessage("E132");
                        ScSoukoCD.SetFocus(1);
                        return false;
                    }
                }

                if (OperationMode == EOperationMode.DELETE)
                {
                    
                    if (ScSoukoCD.IsExistsDeleteCheck())
                    {
                        mtsbl.ShowMessage("E154");
                        ScSoukoCD.SetFocus(1);
                        return false;
                    }
                }
                //*** Insert Other Error Check
            }
            return true;
        }
#endregion

#region Insert/Update/Delete Case      
        private void InsertUpdate(int mode)
        {
            //*** Insert Update Function      
            if (mtsbl.M_Souko_Insert_Update(mse, mode))
            {                
                ChangeMode(OperationMode);
                ScSoukoCD.SetFocus(1);

                mtsbl.ShowMessage("I101");
            }
            else
            {
                mtsbl.ShowMessage("S001");
            }
        }
        private void Delete()
        {
            //*** Delete Function
            if (mtsbl.M_Souko_Delete(mse))
            {
                
                ChangeMode(OperationMode);
                ScSoukoCD.SetFocus(1);

                mtsbl.ShowMessage("I102");
            }
            else
            {
                mtsbl.ShowMessage("S001");
            }
        }

 #endregion

        #region Recall_Function
        private void BindCombo()
        {
            CboStoreCD.Bind(string.Empty, "2");
            CboSoukoType.Bind(string.Empty);
        }
        private void BindLocation()
        {
            dtLocation = new DataTable();
            dtLocation.Columns.Add("TanaCD");
            GvTana.DataSource = dtLocation;
        }
        private void SetRequireField()
        {
            TxtSoukoName.Require(true);
            CboStoreCD.Require(true);
            CboSoukoType.Require(true);
            //soukotype==5
            //ScSoukoMakerCD.TxtCode.Require(true);
            //ScSoukoMakerCD.TxtChangeDate.Require(true);
            //soukotpe==8
            //TxtHikiateOrder.Require(true);
        }        
        private bool TanaCDCheck()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < dtLocation.Rows.Count; i++)
            {
                string tanaCD = dtLocation.Rows[i]["TanaCD"].ToString();
                if (tanaCD != "")
                {
                    if (dict.ContainsKey(tanaCD))
                    {
                        mtsbl.ShowMessage("E105");
                        /// GvTana.ClearSelection();
                        GvTana.CurrentCell = GvTana.Rows[i].Cells[0];
                        //  GvTana.BeginEdit(true);
                        return false;
                    }
                    else
                    {
                        dict.Add(tanaCD, dtLocation.Rows[i]["TanaCD"].ToString());
                    }
                }
                //else
                //{
                //    dtLocation.Rows.Remove(dtLocation.Rows[i]);
                //    //dtLocation.AcceptChanges();
                //}

            }

            return true;
        }
        private bool MakerCheck()
        {
            if (CboSoukoType.SelectedValue.Equals("5"))
            {
                if (!RequireCheck(new Control[] { ScSoukoMakerCD.TxtCode }))
                    return false;

                if (!ScSoukoMakerCD.IsExists(2))
                {
                    mtsbl.ShowMessage("E101");
                    ScSoukoMakerCD.Focus();
                    return false;
                }
            }

            return true;
        }

        //old[CheckZipCD]
        private bool CheckZipCD(CKM_TextBox txt)
        {

            M_ZipCode_Entity mze = new M_ZipCode_Entity
            {
                ZipCD1 = (txt == txtZipCD1) ? txt.Text : string.Empty,
                ZipCD2 = (txt == txtZipCD2) ? txt.Text : string.Empty
            };

            mtsbl = new MasterTouroku_Souko_BL();
            if (!mtsbl.M_ZipCode_Select(mze))
            {
                mtsbl.ShowMessage("E101");
                txt.Focus();
                txt.MoveNext = false;
                return false;
            }
            return true;
        }

        //RealUse[CheckZipCDEvent]
        private bool CheckZipCDEvent()
        {

            M_ZipCode_Entity mze = new M_ZipCode_Entity
            {
                ZipCD1 =txtZipCD1.Text,
                ZipCD2 = txtZipCD2.Text
            };

            mtsbl = new MasterTouroku_Souko_BL();
            if (!mtsbl.M_ZipCode_Select(mze))
            {
                mtsbl.ShowMessage("E101");
                txtZipCD2.Focus();
                //txt.MoveNext = false;
                return false;
            }
            return true;
        }
        private void SetAddress()
        {
            if (!string.IsNullOrWhiteSpace(txtZipCD1.Text) && !string.IsNullOrWhiteSpace(txtZipCD2.Text))
            {
                mtsbl = new MasterTouroku_Souko_BL();

                if (z1 != txtZipCD1.Text || z2 != txtZipCD2.Text)
                {

                    mze = new M_ZipCode_Entity();
                    mze.ZipCD1 = txtZipCD1.Text;
                    mze.ZipCD2 = txtZipCD2.Text;
                    DataTable dt = mtsbl.M_ZipCode_AddressSelect(mze);
                    if (dt.Rows.Count > 0)
                    {
                        TxtAddress1.Text = dt.Rows[0]["Address1"].ToString();
                        TxtAddress2.Text = dt.Rows[0]["Address2"].ToString();
                    }
                }
                else
                {
                    mse = new M_Souko_Entity();
                    mse.ZipCD1 = txtZipCD1.Text;
                    mse.ZipCD2 = txtZipCD2.Text;
                    mse.SoukoCD = ScSoukoCD.Code;
                    DataTable dt = mtsbl.M_Souko_ZipcodeAddressSelect(mse);
                    if(dt.Rows.Count > 0)
                    {
                        TxtAddress1.Text = dt.Rows[0]["Address1"].ToString();
                        TxtAddress2.Text = dt.Rows[0]["Address2"].ToString();
                    }
                }
                


                
            }
        }
        private void CustomEnable()
        {
            if (CboSoukoType.Enabled)
            {
                if (CboSoukoType.SelectedValue.Equals("5"))
                {
                    ScSoukoMakerCD.Enabled = true;
                    ScSoukoMakerCD.Focus();
                }
                else
                {
                    ScSoukoMakerCD.Clear();
                    ScSoukoMakerCD.Enabled = false;
                }

                if (CboSoukoType.SelectedValue.Equals("8"))
                {
                    TxtHikiateOrder.Text = string.Empty;
                    TxtHikiateOrder.Enabled = false;
                }
                else
                {
                    TxtHikiateOrder.Enabled = true;
                }
            }
        }

#endregion

        #region KeyDown Event Case
        private void ScNormal_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                type = 1;
                ScSoukoMakerCD.ChangeDate = ScSoukoCD.ChangeDate;
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
        private void ScCopySoukoCD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                type = 2;
                F11();
            }
            ScSoukoMakerCD.ChangeDate = ScSoukoCD.ChangeDate;
        }
        private void SupplierCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if(!string.IsNullOrEmpty(ScSoukoMakerCD.TxtCode.Text))
                {
                    if (ScSoukoMakerCD.SelectData())
                    {
                        ScSoukoMakerCD.Value1 = ScSoukoMakerCD.TxtCode.Text;
                        ScSoukoMakerCD.Value2 = ScSoukoMakerCD.LabelText;
                    }
                    else
                    {
                        mtsbl.ShowMessage("E101");
                        ScSoukoMakerCD.SetFocus(1);
                    }
                }
                else
                {
                    mtsbl.ShowMessage("E102");//△
                    ScSoukoMakerCD.SetFocus(1);
                }
            }
        }
        private void txtZipCD_KeyDown(object sender, KeyEventArgs e)

        {
            if (Keys.Enter == e.KeyCode)
            {
                if (!RequireCheck(new Control[] { txtZipCD1 }, txtZipCD2))
                    return;
                if (!RequireCheck(new Control[] { txtZipCD2 }, txtZipCD1))
                    return;

                 CKM_TextBox txt = sender as CKM_TextBox;
                if (!string.IsNullOrEmpty(txt.Text))
                    {
                    
                        if (CheckZipCDEvent())
                            SetAddress();
                    
                    //MoveNextControl(e);
                }
                MoveNextControl(e);
            }
        }
        #endregion    
      
        #region Enter Event Case
        private void PanelNormal_Enter(object sender, EventArgs e)
        {
            type = 1;
        }
        private void PanelCopy_Enter(object sender, EventArgs e)
        {
            type = 2;
        }
        /// <summary>
        /// Vendor Search of ChangeDate Case
        /// </summary>
        /// <param name="sender">ScSoukoMakerCD_Enter</param>
        /// <param name="e"></param>
        private void ScSoukoMakerCD_Enter(object sender, EventArgs e)
        {
            ScSoukoMakerCD.Value1 = "0";//NULL
            ScSoukoMakerCD.ChangeDate = ScSoukoCD.ChangeDate ;
        }
        #endregion

        #region Conbo_IndexChanged Case
        private void CboSoukoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomEnable();
        }
        #endregion
    }
}
