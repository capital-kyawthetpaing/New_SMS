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

namespace MasterTouroku_HanbaiTankaKakeritu
{
    public partial class FrmMasterTouroku_HanbaiTankaKakeritu :FrmMainForm
    {
        MasterTouroku_HanbaiTankaKakeritu_BL mhbtbl;
        EOperationMode OperationMode;
        public FrmMasterTouroku_HanbaiTankaKakeritu()
        {
            InitializeComponent();
            mhbtbl = new MasterTouroku_HanbaiTankaKakeritu_BL();
        }

        /// <summary>
        /// form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMasterTouroku_HanbaiTankaKakeritu_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_HanbaiTankaKakeritu";

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            KeyUp += FrmMasterTouroku_HanbaiTankaKakeritu_KeyUp;
            SetRequireField();

            OperationMode = EOperationMode.INSERT;
        }

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
            if (!RequireCheck(new Control[] { txtFromDate,ScTanka.TxtCode}))
                return false;
            if(!ScTanka.IsExists(2))
            {
                bbl.ShowMessage("E101");
                ScTanka.SetFocus(1);
                return false;
            }
            if(!string.IsNullOrWhiteSpace(ScBrand.TxtCode.Text))
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
                        }
                        //CancelData();
                        //scSupplierCD.Clear();
                        //txtRevisionDate.Clear();
                        //txtRate1.Clear();
                        //rdoAllStores.Checked = true;
                        //cbo_Store.SelectedValue = "0000";
                        //dgv_ShiireKakeritsu.DataSource = null;
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

        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            CheckState(true);
        }

        private void btnAllUnCheck_Click(object sender, EventArgs e)
        {
            CheckState(false);
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
            
        }
        private void F12()
        {
            
        }

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
            txtYearCopy.Text = string.Empty;
            txtSeason.Text = string.Empty;
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
                //base.OperationMode = OperationMode;
                //switch(OperationMode)
                //{
                //    case EOperationMode.INSERT:Display(1);
                //        break;
                //}
               if(OperationMode==EOperationMode.INSERT)
                {
                    Display(1);
                }
            }
         }



        #endregion

        private void Display(int mode)
        {
            if(mode==1)
            {
                if (string.IsNullOrWhiteSpace(txtDateCopy.Text))
                {

                }
            }
            else
            {

            }
            
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if(ErrorCheck())
            {

            }
        }

        private void FrmMasterTouroku_HanbaiTankaKakeritu_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
