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
using Search;



namespace MasterTouroku_Program
{
    public partial class FrmMasterTouroku_Program : FrmMainForm
    {
        M_Program_Entity mpe;
        int type = 0;
        public FrmMasterTouroku_Program()
        {
            InitializeComponent();
        }
        private void FormLoadEvent(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            SetRequireField();
            scProgramID.SetFocus(1);
        }
        private void SetRequireField()
        {
            txtProgramName.Require(true);
            txtExeName.Require(true);
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
                        scProgramID.SetFocus(1);
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
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelNormal);
                    EnablePanel(PanelCopy);
                    F11Enable = true;
                    DisablePanel(PanelDetail);
                    scProgramID.SearchEnable = false;
                    scProgramCopy.SearchEnable = true;
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
                    scProgramID.SearchEnable = true;
                    scProgramCopy.SearchEnable = false;
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    break;
            }
            scProgramID.SetFocus(1);
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
                            btnDisplay.Enabled = true;
                            F11Enable = true;
                            DisablePanel(PanelDetail);
                            scProgramCopy.SetFocus(1);
                        }

                        else
                        {
                            DisablePanel(PanelNormal);
                            DisablePanel(PanelCopy);
                            EnablePanel(PanelDetail);
                            btnDisplay.Enabled = false;
                            F11Enable = false;
                            //mbe.BrandCD = ScCopyBrand.Code;
                            DisplayData();
                            txtProgramName.Focus();
                        }
                        break;
                    case EOperationMode.UPDATE:
                        //mbe.BrandCD = ScBrandCD.Code;
                        DisplayData();
                        DisablePanel(PanelNormal);
                        DisablePanel(PanelCopy);
                        btnDisplay.Enabled = false;
                        EnablePanel(PanelDetail);
                        F12Enable = true;
                        F11Enable = false;
                        txtProgramName.Focus();
                        break;
                    case EOperationMode.DELETE:
                        //mbe.BrandCD = ScBrandCD.Code;
                        DisplayData();
                        DisablePanel(PanelNormal);
                        DisablePanel(PanelCopy);
                        btnDisplay.Enabled = false;
                        DisablePanel(PanelDetail);
                        F12Enable = true;
                        F11Enable = false;
                        break;
                    case EOperationMode.SHOW:
                        //mbe.BrandCD = ScBrandCD.Code;
                        //DisplayData();
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
        private void DisplayData()
        {
            DataTable dt = new DataTable();
            //dt = mtkbl.Brand_Select(mbe);
            //if (dt.Rows.Count > 0)
            //{
            //    txtBrandName.Text = dt.Rows[0]["BrandName"].ToString();
            //    txtKanaName.Text = dt.Rows[0]["BrandKana"].ToString();
            //}
        }
        private void F12()
        {
            //if (ErrorCheck(12))
            //{
            //    if (mtkbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
            //    {
            //        //*** Create Entity Object                  
            //        mbe = GetBrandEntity();
            //        switch (OperationMode)
            //        {
            //            case EOperationMode.INSERT:
            //                InsertUpdate(1);
            //                break;
            //            case EOperationMode.UPDATE:
            //                InsertUpdate(2);
            //                break;
            //            case EOperationMode.DELETE:
            //                Delete();
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        PreviousCtrl.Focus();
            //    }
        }
        private void InsertUpdate(int mode)
        {
            //if (mtkbl.M_Brand_Insert_Update(mbe, mode))
            //{
            //    ChangeMode(OperationMode);
            //    ScBrandCD.SetFocus(1);
            //    mtkbl.ShowMessage("I101");
            //}
            //else
            //{
            //    mtkbl.ShowMessage("S001");
            //}
        }
        private void Delete()
        {
            //if (mtkbl.M_Brand_Delete(mbe))
            //{
            //    ChangeMode(OperationMode);
            //    ScBrandCD.SetFocus(1);
            //    mtkbl.ShowMessage("I102");
            //}
            //else
            //{
            //    mtkbl.ShowMessage("S001");
            //}
        }
        private M_Program_Entity GetProgramEntity()
        {
            mpe = new M_Program_Entity
            {

            };
            return mpe;
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void scProgramID_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 1;
                F11();
            }
        }

        private void scProgramCopy_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 2;

                F11();
            }
        }
    }
}

