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

namespace MasterTouroku_ShiireKakeritsu
{
    public partial class frmMasterTouroku_ShiireKakeritsu : FrmMainForm
    {
        MasterTouroku_ShiireKakeritsu_BL mskbl;
        M_OrderRate_Entity moe;
        DataTable dtMain;
        DataTable dtGrid;
        public frmMasterTouroku_ShiireKakeritsu()
        {
            InitializeComponent();
            mskbl = new MasterTouroku_ShiireKakeritsu_BL();
            moe = new M_OrderRate_Entity();
        }

        private void frmMasterTouroku_ShiireKakeritsu_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_ShiireKakeritsu";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            SetRequiredField();
            scSupplierCD.SetFocus(1);
        }
        private void SetRequiredField()
        {
            scSupplierCD.TxtCode.Require(true);
            txtRevisionDate.Require(true);
            txtRate1.Require(true);
        }
        protected override void EndSec()
        {
            this.Close();
        }
        public void CancelData()
        {
            scSupplierCD.Clear();
            txtDate1.Text = string.Empty;
            scBrandCD1.Clear();
            scSportsCD1.Clear();
            scSegmentCD1.Clear();
            txtSeason.Text = string.Empty;
            txtDate.Text = string.Empty;
            txtCopy.Text = string.Empty;
            scBrandCD.Clear();
            scSportsCD.Clear();
            scSegmentCD.Clear();
            txtLastSeason.Text = string.Empty;
            txtChangeDate.Text = string.Empty;
            txtRate.Text = string.Empty;
            scSupplierCD.SetFocus(1);
        }
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);
            switch (Index + 1)
            {
                case 6:
                    {
                        if (mskbl.ShowMessage("Q005") != DialogResult.Yes)
                            return;
                            CancelData();
                    }
                    break;
            }
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { scSupplierCD.TxtCode }))
                return false;
            //if (!String.IsNullOrEmpty(scSupplierCD.TxtCode.Text))
            //{
            //    if (!scSupplierCD.IsExists(2))
            //    {
            //        bbl.ShowMessage("E101");
            //        scSupplierCD.SetFocus(1);
            //        return false;
            //    }
            //}
            //if (scSupplierCD.IsExists(1))
            //{
            //    mskbl.ShowMessage("E119");
            //    scSupplierCD.SetFocus(1);
            //    return false;
            //}
            //if (!RequireCheck(new Control[] { txtDate1 }))
            //    return false;
            return true;
        }

        private M_OrderRate_Entity GetSearchInfo()
        {
            moe = new M_OrderRate_Entity()
            {
                VendorCD = scSupplierCD.TxtCode.Text,
                BrandCD = scBrandCD1.TxtCode.Text,
                SportsCD = scSportsCD1.TxtCode.Text,
                SegmentCD = scSegmentCD1.TxtCode.Text,
                LastSeason = txtSeason.Text,
                ChangeDate = txtDate.Text,
                Rate = txtRate.Text
            };
            return moe;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void SearchData()
        {
            if (ErrorCheck())
            {
                moe = GetSearchInfo();
                dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
                if (dtMain.Rows.Count > 0)
                {
                    BindGrid();
                }
                else
                {
                    mskbl.ShowMessage("E128");
                    dgv_ShiireKakeritsu.DataSource = null;
                }
            }
        }

        private void BindGrid()
        {
            string searchCondition = string.Empty;
            if (!string.IsNullOrWhiteSpace(scBrandCD1.TxtCode.Text))
                searchCondition = "BrandCD = '" + scBrandCD1.TxtCode.Text + "'";
            if (!string.IsNullOrWhiteSpace(scSportsCD1.TxtCode.Text))
                searchCondition = "SportsCD='" + scSportsCD1.TxtCode.Text + "'";
            if (!string.IsNullOrWhiteSpace(scSegmentCD1.TxtCode.Text))
                searchCondition = "SegmentCD= '" + scSegmentCD1.TxtCode.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtSeason.Text))
                searchCondition = "LastSeason= '" + txtSeason.Text+ "'";
            if (!string.IsNullOrWhiteSpace(txtDate.Text))
                searchCondition = "ChangeDate= '" + txtDate.Text + "'";

            if (!string.IsNullOrWhiteSpace(searchCondition))
            {               
                DataRow[] dr= dtMain.Select(searchCondition);
                if (dr.Count() > 0)
                {
                    dtGrid = dtMain.Select(searchCondition).CopyToDataTable();
                }
                else
                    dtGrid = null;
            }
            else
            {
                dtGrid = dtMain;
            }

            dgv_ShiireKakeritsu.DataSource = dtGrid;
        }
        
        private void frmMasterTouroku_ShiireKakeritsu_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            Checkstate(true);
        }
        private void btnReleaseAll_Click(object sender, EventArgs e)
        {
            Checkstate(false);
        }
        private void Checkstate(bool flag)
        {
            foreach (DataGridViewRow row1 in dgv_ShiireKakeritsu.Rows)
            {
                row1.Cells["colChk"].Value = flag;
            }
        }
        //private void dgv_ShiireKakeritsu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if ((Convert.ToBoolean(dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells["colChk"].EditedFormattedValue) == true))
        //    {
        //        foreach (DataGridViewRow row1 in dgv_ShiireKakeritsu.Rows)
        //        {
        //            DataGridViewCheckBoxCell chk1 = row1.Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
        //            chk1.Value = chk1.FalseValue;
        //        }
        //        dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
        //    }
        //    else
        //    {
        //        dgv_ShiireKakeritsu.ClearSelection();
        //    }
        //}
        private void scSupplierCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                scSupplierCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSupplierCD.TxtCode.Text))
                {
                    if (scSupplierCD.SelectData())
                    {
                        scSupplierCD.Value1 = scSupplierCD.TxtCode.Text;
                        scSupplierCD.Value2 = scSupplierCD.LabelText;
                        SearchData();
                    }
                    else
                    {
                        scSupplierCD.SetFocus(1);
                    }
                }
            }
        }

        private void scBrandCD1_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scBrandCD1.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scBrandCD1.TxtCode.Text))
                {
                    if (scBrandCD1.SelectData())
                    {
                        scBrandCD1.Value1 = scBrandCD1.TxtCode.Text;
                        scBrandCD1.Value2 = scBrandCD1.LabelText;
                        BindGrid();
                    }
                    else
                    {
                        BindGrid();
                        scBrandCD1.SetFocus(1);
                    }
                }

            }
        }

        private void scSportsCD1_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSportsCD1.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSportsCD1.TxtCode.Text))
                {
                    if (scSportsCD1.SelectData())
                    {
                        scSportsCD1.Value1 = scSportsCD1.TxtCode.Text;
                        scSportsCD1.Value2 = scSportsCD1.LabelText;
                        BindGrid();
                    }
                    else
                    {
                        BindGrid();
                        scSportsCD1.SetFocus(1);
                    }
                }

            }
        }

        private void scSegmentCD1_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSegmentCD1.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSegmentCD1.TxtCode.Text))
                {
                    if (scSegmentCD1.SelectData())
                    {
                        scSegmentCD1.Value1 = scSegmentCD1.TxtCode.Text;
                        scSegmentCD1.Value2 = scSegmentCD1.LabelText;
                        BindGrid();
                    }
                    else
                    {
                        BindGrid();
                        scSegmentCD1.SetFocus(1);
                    }
                }
            }
        }

        private void scSportsCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSportsCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSportsCD.TxtCode.Text))
                {
                    if (scSportsCD.SelectData())
                    {
                        scSportsCD.Value1 = scSportsCD.TxtCode.Text;
                        scSportsCD.Value2 = scSportsCD.LabelText;
                    }
                    else
                    {
                        scSportsCD.SetFocus(1);
                    }
                }
            }
        }
       
        private void scSegmentCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSegmentCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSegmentCD.TxtCode.Text))
                {
                    if (scSegmentCD.SelectData())
                    {
                        scSegmentCD.Value1 = scSegmentCD.TxtCode.Text;
                        scSegmentCD.Value2 = scSegmentCD.LabelText;
                    }
                    else
                    {
                        scSegmentCD.SetFocus(1);
                    }
                }
            }
        }
        private void scSportsCD1_Enter(object sender, EventArgs e)
        {
            scSportsCD1.Value1 = "202";
        }


        private void scSegmentCD1_Enter(object sender, EventArgs e)
        {
            scSegmentCD1.Value1 = "203";
        }

        private void scSportsCD_Enter(object sender, EventArgs e)
        {
            scSportsCD.Value1 = "202";
        }

        private void scSegmentCD_Enter(object sender, EventArgs e)
        {
            scSegmentCD.Value1 = "203";
        }

        private void dgv_ShiireKakeritsu_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if ((Convert.ToBoolean(dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells["colChk"].EditedFormattedValue) == true))
            {
                foreach (DataGridViewRow row1 in dgv_ShiireKakeritsu.Rows)
                {
                    DataGridViewCheckBoxCell chk1 = row1.Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                    chk1.Value = chk1.FalseValue;
                }
                dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
            }
            else
            {
                dgv_ShiireKakeritsu.ClearSelection();
            }
        }
    }
}
