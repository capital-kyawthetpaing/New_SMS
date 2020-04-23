using System.Windows.Forms;
using BL;
using Entity;
using Search;
using Base.Client;
using CKM_Controls;
using System;
using System.Data;

namespace MasterTouroku_Brand
{
    public partial class FrmMasterTouroku_Brand : FrmMainForm
    {
        M_Brand_Entity mbe;    
        MasterTouroku_Brand_BL mtkbl;
        int type = 0;//1 = normal, 2 = copy (for f11) 

        public FrmMasterTouroku_Brand()
        {
            InitializeComponent();

            Load += new System.EventHandler(FormLoadEvent);
           
            mtkbl = new MasterTouroku_Brand_BL();          
        }

        private void FormLoadEvent(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            
            SetRequireField();

            //SelectNextControl(PanelDetail, true, true, true, true);
            ScBrandCD.SetFocus(1);
        }
     
        private void SetRequireField()
        {
            //***SearchControl.TxtCode.Require(true);         
            //ScBrandCD.TxtCode.Require(true);
            txtBrandName.Require(true);
            txtKanaName.Require(true);
        }

        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
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
                        ScBrandCD.SetFocus(1);
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

        /// <summary>
        /// Disable/Enable,Clear Handle on ModeChange
        /// </summary>
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
                    ScBrandCD.SearchEnable = false;
                    ScCopyBrand.SearchEnable = true;
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
                    ScBrandCD.SearchEnable = true;
                    ScCopyBrand.SearchEnable = false;
                    F9Visible = true;
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
            }
            ScBrandCD.SetFocus(1);
        }

        #region DisplayData
        /// <summary>
        /// Display event
        /// </summary>
        /// <param name="type"></param>
        /// 
        private void F11()
        {         
            if (ErrorCheck(11))
            {
                switch (OperationMode)
                {
                    case EOperationMode.INSERT:
                        if (type == 1)
                        {
                            //DisablePanel(PanelNormal);
                            //DisablePanel(PanelCopy);
                            btnDisplay.Enabled = true;
                            F11Enable = true;
                            DisablePanel(PanelDetail);
                            //txtBrandName.Focus();
                            ScCopyBrand.SetFocus(1);
                        }
                            
                        else
                        {
                            DisablePanel(PanelNormal);
                            DisablePanel(PanelCopy);
                            EnablePanel(PanelDetail);
                            btnDisplay.Enabled = false;
                            F11Enable = false;
                            mbe.BrandCD = ScCopyBrand.Code;
                            DisplayData();
                            txtBrandName.Focus();
                        }
                        break;
                    case EOperationMode.UPDATE:                                          
                        mbe.BrandCD = ScBrandCD.Code;
                        DisplayData();
                        DisablePanel(PanelNormal);
                        DisablePanel(PanelCopy);
                        btnDisplay.Enabled = false;
                        EnablePanel(PanelDetail);
                        F12Enable = true;
                        F11Enable = false; 
                        txtBrandName.Focus();                      
                        break;
                    case EOperationMode.DELETE:                 
                        mbe.BrandCD = ScBrandCD.Code;
                        DisplayData();
                        DisablePanel(PanelNormal);
                        DisablePanel(PanelCopy);
                        btnDisplay.Enabled = false;
                        DisablePanel(PanelDetail);                     
                        F12Enable = true;
                        F11Enable = false;
                        break;
                    case EOperationMode.SHOW:
                        mbe.BrandCD = ScBrandCD.Code;
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

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }

        private void DisplayData()
        {
            DataTable dt = new DataTable();
            dt = mtkbl.Brand_Select(mbe);
            if(dt.Rows.Count > 0)
            {
                txtBrandName.Text = dt.Rows[0]["BrandName"].ToString();
                txtKanaName.Text = dt.Rows[0]["BrandKana"].ToString();
            }
        }
        #endregion

        #region Insert/Update/Date
        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (mtkbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    //*** Create Entity Object                  
                    mbe = GetBrandEntity();
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
            if (mtkbl.M_Brand_Insert_Update(mbe, mode))
            {
                ChangeMode(OperationMode);
                ScBrandCD.SetFocus(1);
                mtkbl.ShowMessage("I101");
            }
            else
            {
                mtkbl.ShowMessage("S001");
            }
        }

        private void Delete()
        {           
            if(mtkbl.M_Brand_Delete(mbe))
            {
                ChangeMode(OperationMode);
                ScBrandCD.SetFocus(1);
                mtkbl.ShowMessage("I102");
            }
            else
            {
                mtkbl.ShowMessage("S001");
            }
        }
        #endregion

        private M_Brand_Entity GetBrandEntity()
        {
            mbe = new M_Brand_Entity
            {
                BrandCD = ScBrandCD.Code,
                BrandName = txtBrandName.Text,
                BrandKana=txtKanaName.Text,
                ChangeDate = DateTime.Today.ToShortDateString(),
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID
            };
            return mbe;
        }

        /// <summary>
        /// ErrorCheck
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool ErrorCheck(int index)
        {        
            if (index == 11)
            {
                
                //HeaderCheck on F11
                mbe = GetBrandEntity();
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 1)//New 
                    {
                        if (!RequireCheck(new Control[] { ScBrandCD.TxtCode }))
                            return false;

                        DataTable dtbrand = new DataTable();
                        mbe.BrandCD = ScBrandCD.Code;
                        dtbrand = mtkbl.Brand_Select(mbe);
                        if (dtbrand.Rows.Count > 0)
                        {
                            mtkbl.ShowMessage("E132");
                            ScBrandCD.SetFocus(1);
                            return false;
                        }
                    }
                    else//Copy
                    {
                        if (!RequireCheck(new Control[] { ScBrandCD.TxtCode }))
                            return false;
                        DataTable dtbrand = new DataTable();
                        mbe.BrandCD = ScBrandCD.Code;
                        dtbrand = mtkbl.Brand_Select(mbe);
                        if (dtbrand.Rows.Count > 0)
                        {
                            mtkbl.ShowMessage("E132");
                            ScBrandCD.SetFocus(1);
                            return false;
                        }
                        else
                        {
                            
                            if (!string.IsNullOrWhiteSpace(ScCopyBrand.TxtCode.Text))
                            {
                                mbe.BrandCD = ScCopyBrand.Code;
                                DataTable dtcopybrand = new DataTable();
                                dtcopybrand = mtkbl.Brand_Select(mbe);
                                if (dtcopybrand.Rows.Count > 0)
                                {
                                    txtBrandName.Text = dtcopybrand.Rows[0]["BrandName"].ToString();
                                }
                                else
                                {
                                    mtkbl.ShowMessage("E133");
                                    ScCopyBrand.SetFocus(1);
                                    return false;
                                }
                            }
                           
                        }

                    }
                }
                else
                {
                    DataTable dtbrand = new DataTable();
                    mbe.BrandCD = ScBrandCD.Code;
                    dtbrand = mtkbl.Brand_Select(mbe);
                    if(dtbrand .Rows .Count == 0)
                    {
                        mtkbl.ShowMessage("E133");
                        ScBrandCD.SetFocus(1);
                        return false;
                    }                    
                }
            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { ScBrandCD.TxtCode}))
                    return false;
                if (string.IsNullOrWhiteSpace(txtBrandName.Text))
                {
                    mtkbl.ShowMessage("E102");
                    txtBrandName.Focus();
                    return false;
                }
                if(string.IsNullOrWhiteSpace(txtKanaName.Text))
                {
                    mtkbl.ShowMessage("E102");
                    txtKanaName.Focus();
                    return false;
                }

                if (OperationMode == EOperationMode.INSERT)
                {
                    DataTable dtbrand = new DataTable();
                    mbe = GetBrandEntity();
                    dtbrand = mtkbl.Brand_Select(mbe);
                    if (dtbrand.Rows.Count > 0)
                    {
                        mtkbl.ShowMessage("E132");
                        ScBrandCD.SetFocus(1);
                        return false;
                    }                   
                }
                else if(OperationMode == EOperationMode.DELETE)
                {
                    DataTable dtbrand = new DataTable();
                    mbe = GetBrandEntity();
                    dtbrand = mtkbl.Brand_Select(mbe);
                    if (dtbrand.Rows.Count > 0)
                    {
                        if (dtbrand.Rows[0]["UsedFlg"].ToString() == "1")
                        {
                            mtkbl.ShowMessage("E154");
                            ScBrandCD.SetFocus(1);
                            return false;
                        }
                    }
                }               
            }
            return true;
        }

        /// <summary>
        /// override F1 Button click
        /// </summary>
        protected override void EndSec()
        {
            this.Close();
        }

        #region KeyDown_Event
        private void ScBrand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 1;
                F11();
            }
        }

        private void ScCopyBrand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 2;

                F11();
            }
        }
        #endregion

        #region Leave_Event
        private void ScBrandCD_Leave(object sender, EventArgs e)
        {
            type = 1;
        }

        private void ScCopyBrand_Leave(object sender, EventArgs e)
        {
            type = 2;
        }
        #endregion

        #region Necessary function to move cursor when enter press
        //private void FrmMasterTouroku_Brand_KeyDown(object sender, KeyEventArgs e)
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

      

        private void FrmMasterTouroku_Brand_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        #endregion

        
    }
}
