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
using CKM_Controls;

namespace MasterTouroku_Settouchi
{
    public partial class frmMasterTouroku_Settouchi : FrmMainForm
    {
        MasterTouroku_Settouchi_BL mtscbl;
        M_Prefix_Entity prefixdata;
        M_Store_Entity storedata;
        M_PrefixNumber_Entity prefixnumber;
        DataTable dtStore;
        DataTable dtPrefix;
        DataTable dtkbn;
        DataTable dtPreNumber;
        string changedate = string.Empty;


        public frmMasterTouroku_Settouchi()
        {
            InitializeComponent();
        }

        private void MasterTouroku_Settouchi_Load(object sender, EventArgs e)
        {
            //InOperatorCD = "0001";
            InProgramID = "MasterTouroku_Settouchi";

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            mtscbl = new MasterTouroku_Settouchi_BL();
            cboStore.Focus();

            //Bind RequireCombo
            BindCombo();
            
            //Show ErrorMessage on EnterKeyPress with Blank
            SetRequireField();
           
        }

        private void BindCombo()
        {
            if (string.IsNullOrWhiteSpace(txtDate.Text.ToString()))
                changedate = System.DateTime.Now.ToString("yyy/MM/dd");
            else changedate = txtDate.Text.ToString();

            cboStore.Bind(changedate,"3");
            cboSeqKBN.Bind(string.Empty);
        }

        //private void BindStore()
        //{
        //    storedata = new M_Store_Entity();
        //    dtStore = new DataTable();

        //    if (string.IsNullOrWhiteSpace(txtDate.Text.ToString()))
        //        storedata.ChangeDate = System.DateTime.Now.ToString("yyy/MM/dd");
        //    else storedata.ChangeDate = txtDate.Text.ToString();
            
        //    dtStore = mtscbl.BindStore(storedata);
        //    cboStore.DisplayMember = "StoreName";
        //    cboStore.ValueMember = "StoreCD";
        //    cboStore.DataSource = dtStore;
        // }

        //private void BindSeqKBN()
        //{
        //    dtkbn = new DataTable();
        //    dtkbn = mtscbl.BindSeqKBN();
        //    cboSeqKBN.DisplayMember = "Char1";
        //    cboSeqKBN.ValueMember = "Column1";
        //    cboSeqKBN.DataSource = dtkbn;
        //}

        private void SetRequireField()
        {
            cboStore.Require(true);
            cboSeqKBN.Require(true);
            txtDate.Require(true);
        }

        #region ButtonClick
        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int index)
        {
            //CKM_SearchControl sc = new CKM_SearchControl();
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
                        cboStore.Focus();
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

        /// <summary>
        /// Disable/Enable,Clear Handle on ModeChange
        /// </summary>
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                case EOperationMode.UPDATE:
                    F9Visible = false;
                    Clear(panel1);
                    Clear(panelDetail);
                    EnablePanel(panel1);
                    //EnablePanel(panelDetail);
                    DisablePanel(panelDetail);
                    BtnF11Show.Enabled = F11Enable = true;
                    F12Enable = true;
                    break;

                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    F9Visible = false;
                    Clear(panel1);
                    Clear(panelDetail);
                    EnablePanel(panel1);
                    Btn_F11.Enabled = true;
                    BtnF11Show.Enabled = true;
                    DisablePanel(panelDetail);
                    break;
            }
            cboStore.Focus();
        }

        /// <summary>
        /// override F1 Button click
        /// </summary>
        protected override void EndSec()
        {
            this.Close();
        }
        #endregion

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
                            DisablePanel(panel1);
                            EnablePanel(panelDetail);
                            BtnF11Show.Enabled = false;
                            Btn_F11.Enabled = false;
                            txtPrefix.Focus();
                        break;
                    case EOperationMode.UPDATE:
                        if (DisplayData())
                        {
                            DisablePanel(panel1);
                            EnablePanel(panelDetail);
                            BtnF11Show.Enabled = false;
                            Btn_F11.Enabled = false;
                            F12Enable = true;
                            txtPrefix.Focus();
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData())
                        {
                            DisablePanel(panel1);
                            DisablePanel(panelDetail);
                            BtnF11Show.Enabled = false;
                            Btn_F11.Enabled = false;
                            F12Enable = true;
                            txtPrefix.Focus();
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData())
                        {
                            DisablePanel(panel1);
                            DisablePanel(panelDetail);
                            BtnF11Show.Enabled = false;
                            Btn_F11.Enabled = false;
                            F12Enable = false;
                        }
                        break;
                }

                //CustomEnable();
            }
        }

        private bool DisplayData()
        {
            prefixdata = new M_Prefix_Entity
            {
                StoreCD = cboStore.SelectedValue.ToString(),
                SeqKBN = cboSeqKBN.SelectedValue.ToString(),
                ChangeDate = txtDate.Text
            };

            dtStore = mtscbl.ShowPrefix(prefixdata);
            if (dtStore.Rows.Count > 0)
            { 
                txtPrefix.Text = dtStore.Rows[0]["Prefix"].ToString();
                return true;
            }
            else
            {
                mtscbl.ShowMessage("E133");
                return false;
            }
        }
        #endregion
        
        #region ErrorCheck function helper
        private bool ErrorCheck(int index)
        {
             if (index == 11)
             {
                 if (!RequireCheck(new Control[] { cboStore, cboSeqKBN, txtDate }))
                     return false;

                 if (StoreExist())
                 {
                     //HeaderCheck on F11
                     if (OperationMode == EOperationMode.INSERT)
                     {
                         if (PrefixExist())
                         {
                             mtscbl.ShowMessage("E132");
                             cboStore.Focus();
                             return false;
                         }
                     }
                     else
                     {
                         if (!PrefixExist())
                         {
                             mtscbl.ShowMessage("E133");
                             cboStore.Focus();
                             return false;
                         }
                         
                     }
                 }
                 else
                {
                    mtscbl.ShowMessage("E134");
                    cboStore.Focus();
                    return false;
                }
             }
             else if (index == 12)
             {
                 if (!RequireCheck(new Control[] { cboStore, cboSeqKBN, txtDate, txtPrefix }))
                     return false;

                 if (StoreExist())
                 {
                     if (OperationMode == EOperationMode.INSERT)
                     {
                         if (PrefixExist())
                         {
                             mtscbl.ShowMessage("E132");
                             cboStore.Focus();
                             return false;
                         }
                         else if (!Prefix_NumberCheck())
                         {
                             mtscbl.ShowMessage("E101");
                             txtPrefix.Focus();
                             return false;
                         }
                     }
                     else if (OperationMode == EOperationMode.UPDATE)
                     {
                         if (!PrefixExist())
                         {
                             mtscbl.ShowMessage("E133");
                             cboStore.Focus();
                             return false;
                         }
                         else if (!Prefix_NumberCheck())
                         {
                             mtscbl.ShowMessage("E101");
                             txtPrefix.Focus();
                             return false;
                         }
                     }
                     else
                     {
                         if (!PrefixExist())
                         {
                             mtscbl.ShowMessage("E133");
                             cboStore.Focus();
                             return false;
                         }
                     }
                 }
                else
                {
                    mtscbl.ShowMessage("E134");
                    cboStore.Focus();
                    return false;
                }
            }
             return true;
        }

        private void txtPrefix_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtPrefix.Text))
                {
                    if (!Prefix_NumberCheck())
                    {
                        mtscbl.ShowMessage("E101");
                        txtPrefix.Focus();
                    }
                }
                else
                {
                    mtscbl.ShowMessage("E102");
                    txtPrefix.Focus();
                }
            }
        }
        #endregion

        private M_Prefix_Entity GetPrefixEntity()
        {
            prefixdata = new M_Prefix_Entity
            {
                StoreCD = cboStore.SelectedValue.ToString(),
                SeqKBN = cboSeqKBN.SelectedValue.ToString(),
                ChangeDate = txtDate.Text.ToString(),
                Prefix = txtPrefix.Text.ToString(),
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID,
                Key = cboStore.AccessibilityObject.Value.ToString() + " " + cboSeqKBN.AccessibilityObject.Value.ToString() + " " + txtPrefix.Text
            };
            return prefixdata;
        }

        private bool PrefixExist()
        {
            prefixdata = new M_Prefix_Entity
            {
                StoreCD = cboStore.SelectedValue.ToString(),
                SeqKBN = cboSeqKBN.SelectedValue.ToString(),
                ChangeDate = txtDate.Text
            };
            dtPrefix = mtscbl.ShowPrefix(prefixdata);

            return dtPrefix.Rows.Count > 0 ? true : false;
        }

        private bool StoreExist()
        {
            dtStore = mtscbl.StoreExist(cboStore.SelectedValue.ToString(),txtDate.Text,"0");

            return dtStore.Rows.Count > 0 ? true : false;
        }

        private bool Prefix_NumberCheck()
        {
            prefixnumber = new M_PrefixNumber_Entity
            {
                Prefix = txtPrefix.Text,
            };
            dtPreNumber = mtscbl.prefixCheck(prefixnumber);

            return dtPreNumber.Rows.Count > 0 ? true : false;
        }

        #region Insert/Update/Delete
        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (mtscbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    prefixdata = GetPrefixEntity();
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
                else PreviousCtrl.Focus();
            }
        }

        private void InsertUpdate(int mode)
        {
            if (mtscbl.M_Prefix_Insert_Update(prefixdata,mode))
            {
                mtscbl.ShowMessage("I101");
                Clear(panel1);
                Clear(panelDetail);

                ChangeMode(OperationMode);
                cboStore.Focus();
            }
            else
            {
                mtscbl.ShowMessage("S001");
            }
        }

        private void Delete()
        {
            if (mtscbl.M_Prefix_Delete(prefixdata))
            {
                Clear(panel1);
                Clear(panelDetail);

                EnablePanel(panel1);
                DisablePanel(panelDetail);
                cboStore.Focus();

                mtscbl.ShowMessage("I102");
            }
            else
            {
                mtscbl.ShowMessage("S001");
            }
        }
        #endregion

        private void BtnF11Show_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }
        
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                F11();
            }
        }

        private void frmMasterTouroku_Settouchi_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
