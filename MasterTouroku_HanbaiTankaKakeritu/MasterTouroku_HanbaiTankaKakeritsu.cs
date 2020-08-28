﻿using System;
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

namespace MasterTouroku_HanbaiTankaKakeritsu
{
    public partial class FrmMasterTouroku_HanbaiTankaKakeritsu :FrmMainForm
    {
        MasterTouroku_HanbaiTankaKakeritu_BL mhbtbl;
        M_SKU_Entity mskue;
        M_SKUPrice_Entity mskupe;
        EOperationMode OperationMode;
        DataTable dtData;

        public FrmMasterTouroku_HanbaiTankaKakeritsu()
        {
            InitializeComponent();
            mhbtbl = new MasterTouroku_HanbaiTankaKakeritu_BL();
            mskue = new M_SKU_Entity();
            mskupe = new M_SKUPrice_Entity();
        }

        /// <summary>
        /// form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMasterTouroku_HanbaiTankaKakeritsu_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_HanbaiTankaKakeritsu";

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            KeyUp += FrmMasterTouroku_HanbaiTankaKakeritu_KeyUp;
            SetRequireField();
            BindCombox();

            OperationMode = EOperationMode.INSERT;
        }

        #region Error Check

        /// <summary>
        /// Required Check
        /// </summary>
        private void SetRequireField()
        {
            txtFromDate.Require(true);
            ScTanka.TxtCode.Require(true);
            ScTankaCopy.TxtCode.Require(true, txtDateCopy);

        }

        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtFromDate, ScTanka.TxtCode }))
                return false;
            if (!ScTanka.IsExists(2))
            {
                bbl.ShowMessage("E101");
                ScTanka.SetFocus(1);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(ScBrand.TxtCode.Text))
                if (!ScBrand.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    ScBrand.SetFocus(1);
                    return false;
                }

            if (!string.IsNullOrWhiteSpace(ScSegment.TxtCode.Text))
                if (!ScSegment.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    ScSegment.SetFocus(1);
                    return false;
                }

            if (!string.IsNullOrWhiteSpace(txtPriceOutTaxFrom.Text))
            {
                int str2 = 0;
                int str1 = Convert.ToInt32(txtPriceOutTaxFrom.Text.ToString().Replace(",", ""));
                if (string.IsNullOrWhiteSpace(txtPriceOutTaxTo.Text.ToString()))
                    str2 = 0;
                else
                    str2 = Convert.ToInt32(txtPriceOutTaxTo.Text.ToString().Replace(",", ""));
                if (str1 > str2)
                {
                    bbl.ShowMessage("E104");
                    txtToDate.Focus();
                    return false;
                }

            }

            if (!RequireCheck(new Control[] { ScTankaCopy.TxtCode }, txtDateCopy))
                return false;

            return true;
        }

        #endregion
        private void BindCombox()
        {
            string ymd = bbl.GetDate();
            cboYear.Bind(ymd);
            cboSeason.Bind(ymd);
            cboYearCopy.Bind(ymd);
            cboSeasonCopy.Bind(ymd);
        }

        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            switch (Index + 1)
            {
                case 2: OperationMode = EOperationMode.INSERT;
                    break;
                case 3:
                         OperationMode = EOperationMode.UPDATE;
                    break;
                case 6:
                    {
                        if (bbl.ShowMessage("Q005") != DialogResult.Yes)
                        {
                            Clear();
                            txtFromDate.Focus();
                        }
                        else
                            PreviousCtrl.Focus();
                        
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

        protected override void EndSec()
        {
            this.Close();
        }

        private void CheckState(bool flag)
        {
            foreach (DataGridViewRow row1 in gdvHanbaiTankaKakeritsu.Rows)
            {
                row1.Cells["colchk"].Value = flag;
            }
        }

        private void F11()
        {
            if(ErrorCheck())
            {
                if (OperationMode == EOperationMode.INSERT)
                {
                    Display(1);
                }
                else if (OperationMode == EOperationMode.UPDATE)
                {
                    Display(2);

                }
            }
            
        }
        private void F12()
        {
            if (ErrorCheck())
            {
                if(dtData.Rows.Count > 0)
                {
                    if (bbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                    {
                        mskue = GetSKUEntity();
                        mskupe = GetSKUPriceEntity();
                        mskupe.dt1 = dtData;
                        switch (OperationMode)
                        {
                            case EOperationMode.INSERT:
                                InsertUpdate(1);
                                break;
                            case EOperationMode.UPDATE:
                                InsertUpdate(2);
                                break;
                        }
                    }
                }
              
            }
        }


        private void InsertUpdate(int mode)
        {
            if(mhbtbl.SaleRatePrice_InsertUpdate(mskue,mskupe,mode))
            {
                bbl.ShowMessage("I101");
                Clear();
                txtFromDate.Focus();
            }
            else
            {
                bbl.ShowMessage("S001");
            }

        }
        #region Get Searching  Data
        private M_SKU_Entity GetSKUEntity()
        {
            mskue = new M_SKU_Entity
            {
                ChangeDate = txtFromDate.Text,
                EndDate = txtToDate.Text,
                DateCopy = txtDateCopy.Text,
                BrandCD = ScBrand.Code,
                BrandCDCopy = ScBrandCopy.Code,
                ExhibitionSegmentCD = ScSegment.Code,
                ExhibitionSegmentCDCopy = ScSegmentCopy.Code,
                LastYearTerm = cboYear.SelectedValue.ToString(),
                LastYearTermCopy = cboYearCopy.SelectedValue.ToString(),
                LastSeason = cboSeason.SelectedValue.ToString(),
                LastSeasonCopy = cboSeasonCopy.SelectedValue.ToString(),
                PriceOutTaxFrom = txtPriceOutTaxFrom.Text,
                PriceOutTaxTo = txtPriceOutTaxTo.Text,
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID,
                Key = txtFromDate.Text.ToString() + " " + ScTanka.Code.ToString()
            };
            return mskue;
        }

        private M_SKUPrice_Entity GetSKUPriceEntity()
        {
            mskupe = new M_SKUPrice_Entity
            {
                TankaCD = ScTanka.Code,
                TankaName = ScTanka.LabelText,
                TankaCDCopy=ScTankaCopy.Code,
            };
            return mskupe;
        }
        #endregion

        private void Clear()
        {
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            ScTanka.Clear();
            ScBrand.Clear();
            ScSegment.Clear();
            cboYear.SelectedValue ="-1";
            cboSeason.SelectedValue = "-1";
            txtPriceOutTaxFrom.Text = string.Empty;
            txtPriceOutTaxTo.Text = string.Empty;
            txtDateCopy.Text = string.Empty;
            ScTankaCopy.Clear();
            ScBrandCopy.Clear();
            ScSegmentCopy.Clear();
            cboYearCopy.SelectedValue = "-1";
            cboSeasonCopy.SelectedValue = "-1";
            txtRate.Text = string.Empty;
            gdvHanbaiTankaKakeritsu.DataSource = null;

        }

        #region KeyDown/ Enter
        private void ScTanka_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScTanka.TxtCode.Text))
                {
                    ScTanka.ChangeDate = bbl.GetDate();
                    if (!ScTanka.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScTanka.SetFocus(1);
                    }

                }

            }
        }

        private void ScBrand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScBrand.TxtCode.Text))
                {
                    ScBrand.ChangeDate = bbl.GetDate();
                    if (!ScBrand.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScBrand.SetFocus(1);
                    }

                }

            }
        }

        private void ScSegment_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(ScSegment.TxtCode.Text))
                {
                    ScSegment.ChangeDate = bbl.GetDate();
                    if (!ScSegment.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScSegment.SetFocus(1);
                    }
                }
            }
        }

        private void ScSegment_Enter(object sender, EventArgs e)
        {
            ScSegment.Value1 = "226";
        }

        private void txtPriceOutTaxTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (!string.IsNullOrWhiteSpace(txtPriceOutTaxFrom.Text))
                {
                    int str2 = 0;
                    int str1 = Convert.ToInt32(txtPriceOutTaxFrom.Text.ToString().Replace(",", ""));
                    if (string.IsNullOrWhiteSpace(txtPriceOutTaxTo.Text.ToString()))
                        str2 = 0;
                    else
                        str2 = Convert.ToInt32(txtPriceOutTaxTo.Text.ToString().Replace(",", ""));
                    if (str1 > str2)
                    {
                        bbl.ShowMessage("E104");
                        txtToDate.Focus();
                    }

                }
            }
        }

      
        private void ScTankaCopy_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScTankaCopy.TxtCode.Text))
                {
                    ScTankaCopy.ChangeDate = bbl.GetDate();
                    if (!ScTankaCopy.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScTankaCopy.SetFocus(1);
                    }

                }

            }
        }

        private void ScBrandCopy_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScBrandCopy.TxtCode.Text))
                {
                    ScBrandCopy.ChangeDate = bbl.GetDate();
                    if (!ScBrandCopy.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScBrandCopy.SetFocus(1);
                    }

                }

            }
        }

        private void ScSegmentCopy_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(ScSegmentCopy.TxtCode.Text))
                {
                    ScSegmentCopy.ChangeDate = bbl.GetDate();
                    if (!ScSegmentCopy.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScSegmentCopy.SetFocus(1);
                    }
                }
            }
        }

        private void ScSegmentCopy_Enter(object sender, EventArgs e)
        {
            ScSegmentCopy.Value1 = "226";
        }

        private void txtSeason_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
              if(OperationMode==EOperationMode.INSERT)
                {
                    Display(1);
                }
              else if(OperationMode == EOperationMode.UPDATE)
                {
                    Display(2);

                }
            }
         }



        #endregion

        #region Button Click
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (ErrorCheck())
            {
                if (OperationMode == EOperationMode.INSERT)
                {
                    Display(1);
                }
                else if (OperationMode == EOperationMode.UPDATE)
                {
                    Display(2);

                }
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {

            if (RequireCheck(new Control[] { txtFromDate, ScTanka.TxtCode,txtRate }))
            {
                foreach (DataGridViewRow row in gdvHanbaiTankaKakeritsu.Rows)
                {
                    DataGridViewCheckBoxCell check = row.Cells[0] as DataGridViewCheckBoxCell;
                    if (row.Cells["colchk"].Value != null)
                    {
                        string chk = row.Cells["colchk"].Value.ToString();
                        if (check.Value == check.TrueValue || chk == "True")
                        {
                            row.Cells["colRate"].Value = txtRate.Text;
                        }
                    }
                }
            }
        }

        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            CheckState(true);
        }

        private void btnAllUnCheck_Click(object sender, EventArgs e)
        {
            CheckState(false);
        }
        #endregion

        private void Display(int mode)
        {
            mskue = GetSKUEntity();
            mskupe = GetSKUPriceEntity();
            dtData = new DataTable();
            if (mode==1)
            {
                if (string.IsNullOrWhiteSpace(txtDateCopy.Text))
                {
                    dtData = mhbtbl.Select_SKUData(mskue,mskupe,"1");
                    if (dtData.Rows.Count > 0)
                        gdvHanbaiTankaKakeritsu.DataSource = dtData;
                    else
                    {
                        bbl.ShowMessage("E128");
                        PreviousCtrl.Focus();

                    }
                        
                }
                else if (!string.IsNullOrWhiteSpace(txtDateCopy.Text))
                {
                    dtData = mhbtbl.Select_SKUData(mskue, mskupe, "2");
                    if (dtData.Rows.Count > 0)
                        gdvHanbaiTankaKakeritsu.DataSource = dtData;
                    else
                    {
                        bbl.ShowMessage("E128");
                        PreviousCtrl.Focus();

                    }
                }
            }
            else
            {
                 dtData = mhbtbl.Select_SKUData(mskue, mskupe, "3");
                if (dtData.Rows.Count > 0)
                    gdvHanbaiTankaKakeritsu.DataSource = dtData;
                else
                {
                    bbl.ShowMessage("E128");
                    PreviousCtrl.Focus();

                }
            }
            
        }

        private void FrmMasterTouroku_HanbaiTankaKakeritu_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void gdvHanbaiTankaKakeritsu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}