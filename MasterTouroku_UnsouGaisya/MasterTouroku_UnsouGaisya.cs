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
using CKM_Controls;
using Search;
using BL;
using Entity;

namespace MasterTouroku_UnsouGaisya
{
    public partial class FrmMasterTouroku_UnsouGaisya : FrmMainForm
    {
        int type = 0;//1 = normal, 2 = copy (for f11)

        MasterTouroku_UnsouGaisya_BL mtugsbl;
        M_Shipping_Entity mse;


        #region Necessary Function on FormLoad
        public FrmMasterTouroku_UnsouGaisya()
        {
            InitializeComponent();
            InProgramID = Application.ProductName;

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            mtugsbl = new MasterTouroku_UnsouGaisya_BL();

            SetRequireField();

            //SelectNextControl(PanelDetail, true, true, true, true);

            BindCombo();
            ScShippingCD.SetFocus(1);
        }
        private void BindCombo()
        {
            string ymd = bbl.GetDate();
            //cboIdentity.Bind(string.Empty,"26");
            cboIdentity.Bind(ymd);
            BindNormal();
        }

        private void BindNormal()
        {
            DataTable dtT = new DataTable();
            dtT.Columns.Add("TypeId", typeof(int));
            dtT.Columns.Add("TypeName", typeof(string));
            //dtT.Rows.Add(0, string.Empty);
            dtT.Rows.Add(0, "普通便");
            dtT.Rows.Add(1, "代引き");
            dtT.Rows.Add(2, "ネコポス");

            cboNormalType.ValueMember = "TypeId";
            cboNormalType.DisplayMember = "TypeName";
            cboNormalType.DataSource = dtT;
        }

        private void SetRequireField()
        {
            ScShippingCD.TxtCode.Require(true);
            ScShippingCD.TxtChangeDate.Require(true);
        }
        #endregion
        
        #region ButtonClick
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
                        ScShippingCD.SetFocus(1);
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
                    Clear(PanelNormal);
                    Clear(PanelCopy);
                    Clear(PanelDetail);
                    EnablePanel(PanelNormal);
                    EnablePanel(PanelCopy);
                    DisablePanel(PanelDetail);
                    ScShippingCD.SearchEnable = false;
                    ScCopyShippingCD.SearchEnable = true;
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
                    ScShippingCD.SearchEnable = true;
                    ScCopyShippingCD.SearchEnable = false;
                    F9Visible = true;
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
            }
            ScShippingCD.SetFocus(1);
        }

        protected override void EndSec()
        {
            this.Close();
        }
        #endregion

        #region DisplayData
        private bool DisplayData(CKM_SearchControl sc)
        {
            mtugsbl = new MasterTouroku_UnsouGaisya_BL();
            mse = new M_Shipping_Entity
            {
                ShippingCD = sc.Code,
                ChangeDate = sc.ChangeDate
            };
            mse = mtugsbl.M_Carrier_Select(mse);

            if (mse != null)
            {
                txtShippingName.Text = mse.ShippingName;
                cboIdentity.SelectedValue = mse.CarrierFlg;
                cboNormalType.SelectedValue = mse.NormalFlg;
                txtYahooCD.Text = mse.YahooCD;
                txtRakutenCD.Text = mse.RakutenCD;
                txtAmazonCD.Text = mse.AmazonCD;
                txtWowmaCD.Text = mse.WowmaCD;
                txtPonpareCD.Text = mse.PonpareCD;
                txtOtherCD.Text = mse.OtherCD;
                txtremark.Text = mse.Remarks;
                chkDeleteFlg.Checked = mse.DeleteFlg.Equals("1") ? true : false;

                txtShippingName.Focus();
                return true;
            }
            else
            {
                mtugsbl.ShowMessage("E133");
                return false;
            }

            //DataTable dtCarrier = mtugsbl.M_Carrier_Select(mse);
            //if (dtCarrier.Rows.Count > 0)
            //{
            //    txtShippingName.Text = dtCarrier.Rows[0]["CarrierName"].ToString();
            //    cboIdentity.SelectedValue = dtCarrier.Rows[0]["CarrierFlg"].ToString();
            //    txtYahooCD.Text = dtCarrier.Rows[0]["YahooCarrierCD"].ToString();
            //    txtRakutenCD.Text = dtCarrier.Rows[0]["RakutenCarrierCD"].ToString();
            //    txtAmazonCD.Text = dtCarrier.Rows[0]["AmazonCarrierCD"].ToString();
            //    txtWowmaCD.Text = dtCarrier.Rows[0]["WowmaCarrierCD"].ToString();
            //    txtPonpareCD.Text = dtCarrier.Rows[0]["PonpareCarrierCD"].ToString();
            //    txtOtherCD.Text = dtCarrier.Rows[0]["OtherCD"].ToString();
            //    txtremark.Text = dtCarrier.Rows[0]["Remarks"].ToString();
            //    if (dtCarrier.Rows[0]["DeleteFlg"].ToString() == "1")
            //        chkDeleteFlg.Checked = true;
            //    else chkDeleteFlg.Checked = false;
            //}
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
                            
                            ScCopyShippingCD.SetFocus(1);
                          
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(ScCopyShippingCD.ChangeDate) || (DisplayData(ScCopyShippingCD)))
                            {
                                DisablePanel(PanelNormal);
                                DisablePanel(PanelCopy);
                                EnablePanel(panel1);
                                btnDisplay.Enabled = false;
                                F11Enable = false;
                                txtShippingName.Focus();
                            }
                            //SelectNextControl(PanelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.UPDATE:
                            DisplayData(ScShippingCD);                      
                            DisablePanel(PanelNormal);
                            DisablePanel(PanelCopy);
                            btnDisplay.Enabled = false;
                            EnablePanel(PanelDetail);
                            F12Enable = true;
                            F11Enable = false;
                            SelectNextControl(PanelDetail, true, true, true, true);
                        
                        break;
                    case EOperationMode.DELETE:
                            DisplayData(ScShippingCD);                       
                            DisablePanel(PanelNormal);
                            DisablePanel(PanelCopy);
                            btnDisplay.Enabled = false;
                            DisablePanel(PanelDetail);
                            SelectNextControl(PanelDetail, true, true, true, true);
                            F12Enable = true;
                            F11Enable = false;
                        
                        break;
                    case EOperationMode.SHOW:
                            DisplayData(ScShippingCD);                       
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
        #endregion

        #region Insert/Update/Date
        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (mtugsbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    mse = GetShippingEntity();
                    switch (OperationMode)
                    {
                        case EOperationMode.INSERT:
                            InsertUpdate(1);
                            break;
                        case EOperationMode.UPDATE:
                            InsertUpdate(2);
                            break;
                        case EOperationMode.DELETE:
                            if(mse.UsedFlg == "1")
                            {
                                mtugsbl.ShowMessage("E154");
                            }
                            else
                            {
                                Delete();
                            }
                            
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
            if (mtugsbl.M_Gaisya_InsertUpdate(mse, mode))
            {

                ChangeMode(OperationMode);
                ScShippingCD.SetFocus(1);

                mtugsbl.ShowMessage("I101");
            }
            else
            {
                mtugsbl.ShowMessage("S001");
            }
        }

        private void Delete()
        {
            if (mtugsbl.M_Gaisya_Delect(mse))
            {
                ChangeMode(OperationMode);
                ScShippingCD.SetFocus(1);
                mtugsbl.ShowMessage("I102");
            }
            else
            {
                mtugsbl.ShowMessage("S001");
            }
        }
        #endregion

        /// <summary>
        /// To Get Data from Form Entity
        /// </summary>
        /// <returns></returns>
        private M_Shipping_Entity GetShippingEntity()
        {
            mse = new M_Shipping_Entity
            {
                ShippingCD = ScShippingCD.Code,
                ChangeDate = ScShippingCD.ChangeDate,
                ShippingName = txtShippingName.Text,
                CarrierFlg = cboIdentity.SelectedValue.ToString(),
                NormalFlg = cboNormalType.SelectedValue.ToString(),
                YahooCD = txtYahooCD.Text,
                RakutenCD = txtRakutenCD.Text,
                AmazonCD = txtAmazonCD.Text,
                WowmaCD = txtWowmaCD.Text,
                PonpareCD = txtPonpareCD.Text,
                OtherCD = txtOtherCD.Text,
                Remarks = txtremark.Text,
                DeleteFlg = chkDeleteFlg.Checked ? "1" : "0",
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID
            };
            return mse;
        }

        /// <summary>
        /// Error Check for F11 and F12 btn
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool ErrorCheck(int index)
        {
            mtugsbl = new MasterTouroku_UnsouGaisya_BL();
            if (index == 11)
            {
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 1)//New 
                    {
                        if (!RequireCheck(new Control[] { ScShippingCD.TxtCode, ScShippingCD.TxtChangeDate }))
                            return false;

                        //if (ScShippingCD.IsExists())
                        //{
                        //    mtugsbl.ShowMessage("E132");
                        //    ScShippingCD.SetFocus(1);
                        //    return false;
                        //}

                    }
                    else//Copy
                    {
                        if (!RequireCheck(new Control[] { ScShippingCD.TxtCode, ScShippingCD.TxtChangeDate }))
                            return false;

                        if (ScShippingCD.IsExists(1))
                        {
                            mtugsbl.ShowMessage("E132");
                            ScShippingCD.SetFocus(1);
                            return false;
                        }
                        else
                        { 
                            if(!string .IsNullOrWhiteSpace(ScCopyShippingCD.TxtCode.Text ))
                            {
                                if(!ScCopyShippingCD.IsExists(1))
                                {
                                    mtugsbl.ShowMessage("E133");
                                    ScCopyShippingCD.SetFocus(1);
                                    return false;
                                }
                            }
                        }                    
                    }
                }
                else
                {
                    if (!ScShippingCD.IsExists(1))
                    {
                        mtugsbl.ShowMessage("E133");
                        ScShippingCD.SetFocus(1);
                        return false;
                    }

                }
            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { ScShippingCD.TxtCode, ScShippingCD.TxtChangeDate }))
                    return false;

                if (OperationMode == EOperationMode.INSERT)
                {
                    if (ScShippingCD.IsExists(1))
                    {
                        mtugsbl.ShowMessage("E132");
                        ScShippingCD.SetFocus(1);
                        return false;
                    }
                }

                if (string.IsNullOrWhiteSpace(txtShippingName.Text))
                {
                    mtugsbl.ShowMessage("E102");
                    txtShippingName.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(cboIdentity.Text.ToString()))
                {
                    mtugsbl.ShowMessage("E102");
                    cboIdentity.Focus();
                    return false;
                }

                if(string.IsNullOrWhiteSpace(cboNormalType.Text.ToString()))
                {
                    mtugsbl.ShowMessage("E102");
                    cboNormalType.Focus();
                    return false;
                }

                //*** Insert Other Error Check
            }
            return true;
        }

        #region Form KeyDown and KeyUp Event
        private void ScShippingCD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 1;
                F11();
            }
        }

        private void ScCopyShippingCD_ChangeDateKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 2;
                F11();
            }
        }

        private void FrmMasterTouroku_UnsouGaisya_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        #endregion
    }
}
