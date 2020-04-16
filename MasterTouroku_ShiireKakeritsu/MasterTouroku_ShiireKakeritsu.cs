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
        DataTable dt = new DataTable();

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
            txtYear.Text = string.Empty;
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
                LastYearTerm=txtYear.Text,
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
            if (!string.IsNullOrWhiteSpace(txtYear.Text))
                searchCondition = "LastYearTerm='" + txtYear.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtSeason.Text))
                searchCondition = "LastSeason= '" + txtSeason.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtDate.Text))
                searchCondition = "ChangeDate= '" + txtDate.Text + "'";

            if (!string.IsNullOrWhiteSpace(searchCondition))
            {
                DataRow[] dr = dtMain.Select(searchCondition);
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


        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if(!string.IsNullOrWhiteSpace(scSupplierCD.TxtCode.Text))
            //{
            //    DataRow dtRow = dtMain.NewRow();
            //    dtRow["BrandCD"] = scBrandCD.TxtCode.Text;
            //    dtRow["SportsCD"] = scSportsCD.TxtCode.Text;
            //    dtRow["SegmentCD"] = scSegmentCD.TxtCode.Text;
            //    dtRow["LastSeason"] = txtLastSeason.Text;
            //    dtRow["ChangeDate"] = txtChangeDate.Text;
            //    dtRow["Rate"] = Convert.ToDecimal(txtRate.Text);
            //    dtMain.Rows.Add(dtRow);
            //    dgv_ShiireKakeritsu.DataSource = dtMain;
            //}
            //else
            //{
            if (dgv_ShiireKakeritsu.Rows.Count == 0)
            {

                dt.Columns.Add("BrandCD");
                dt.Columns.Add("SportsCD");
                dt.Columns.Add("SegmentCD");
                dt.Columns.Add("LastYearTerm");
                dt.Columns.Add("LastSeason");
                dt.Columns.Add("ChangeDate");
                dt.Columns.Add("Rate");
            }
            DataRow dtRow = dt.NewRow();
            dtRow["BrandCD"] = scBrandCD.TxtCode.Text;
            dtRow["SportsCD"] = scSportsCD.TxtCode.Text;
            dtRow["SegmentCD"] = scSegmentCD.TxtCode.Text;
           // dtRow["LastYearTeam"] = txtYear.Text;
            dtRow["LastSeason"] = txtLastSeason.Text;
            dtRow["ChangeDate"] = txtChangeDate.Text;
            dtRow["Rate"] = Convert.ToDecimal(txtRate.Text);
            dt.Rows.Add(dtRow);
            dgv_ShiireKakeritsu.DataSource = dt;

        }
        

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtCopy.Text))
            {
                mskbl.ShowMessage("E102");
                txtCopy.Focus();
            }
            else
            {
                if (this.dgv_ShiireKakeritsu.GetCellCount(DataGridViewElementStates.Selected) > 0)
                {

                    //dgv_ShiireKakeritsu.MultiSelect = true;
                    //dgv_ShiireKakeritsu.SelectAll();
                    //DataObject dataObj = dgv_ShiireKakeritsu.GetClipboardContent();
                    //if (dataObj != null)
                    //    Clipboard.SetDataObject(dataObj);
                    //var newline = System.Environment.NewLine;
                    //var tab = "\t";
                    //var clipboard_string = "";
                    //foreach (DataGridViewRow row in dgv_ShiireKakeritsu.Rows)
                    //{
                    //    for (int i = 0; i < row.Cells.Count; i++)
                    //    {
                    //        if (i == (row.Cells.Count - 1))
                    //            clipboard_string += row.Cells[i].Value + newline;
                    //        else
                    //            clipboard_string += row.Cells[i].Value + tab;
                    //    }
                    //Clipboard.SetText(clipboard_string);

                    foreach (DataGridViewRow row in dgv_ShiireKakeritsu.Rows)
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                        if (chk.Selected == true)
                        {

                            DataRow dtRow = dtMain.NewRow();
                            dtRow["BrandCD"] = row.Cells["colBrandCD1"].Value.ToString();
                            dtRow["SportsCD"] = row.Cells["colSportsCD1"].Value.ToString();
                            dtRow["SegmentCD"] = row.Cells["colSegmentCD1"].Value.ToString();
                            dtRow["LastYearTerm"] = row.Cells["colYear"].Value.ToString();
                            dtRow["LastSeason"] = row.Cells["colSeason"].Value.ToString();
                            dtRow["ChangeDate"] = txtCopy.Text;
                            dtRow["Rate"] = row.Cells["colRate1"].Value.ToString();
                            dtMain.Rows.Add(dtRow);
                            dgv_ShiireKakeritsu.DataSource = dtMain;
                        }
                    }
                }
            }
        }

        private void btnChoice_Click(object sender, EventArgs e)
        {
            dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
            if (dgv_ShiireKakeritsu.Rows.Count > 0)
            {
                string searchCondition = string.Empty;
                if (!string.IsNullOrWhiteSpace(scBrandCD.TxtCode.Text))
                    searchCondition = "BrandCD = '" + scBrandCD.TxtCode.Text + "'";
                if (!string.IsNullOrWhiteSpace(scSportsCD.TxtCode.Text))
                    searchCondition = "SportsCD='" + scSportsCD.TxtCode.Text + "'";
                if (!string.IsNullOrWhiteSpace(scSegmentCD.TxtCode.Text))
                    searchCondition = "SegmentCD= '" + scSegmentCD.TxtCode.Text + "'";
                if (!string.IsNullOrWhiteSpace(txtLastSeason.Text))
                    searchCondition = "LastSeason= '" + txtLastSeason.Text + "'";
                if (!string.IsNullOrWhiteSpace(txtChangeDate.Text))
                    searchCondition = "ChangeDate= '" + txtChangeDate.Text + "'";
                if (!string.IsNullOrWhiteSpace(txtRate.Text))
                    searchCondition = "Rate= '" + txtRate.Text + "'";
                if (!string.IsNullOrWhiteSpace(searchCondition))
                {
                    DataRow[] dr = dtMain.Select(searchCondition);
                    if (dr.Count() > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            dr[i]["Column1"] = "1";
                        }
                        //for (int i = 0; i < dtGrid.Rows.Count; i++)
                        //{
                        //    for (int j = 0; j < dtMain.Rows.Count; j++)
                        //    {
                        //        dtMain.Rows[j]["Column1"].ToString() = "1";
                        //    }

                        //}
                        //    //foreach (DataGridViewRow drow in dgv_ShiireKakeritsu.Rows)
                        //    //    {
                        //foreach (DataRow row in dtGrid.Rows)
                        //{
                        //    //    //if(row.Equals(drow))
                        //    //    //{
                        //    //    //    drow.Cells["colChk"].Value = true;
                        //    //    //}
                        //    row["Column1"] = "1";

                        //}
                        //dtMain.Merge(dtGrid);
                        //DataTable distinctTable = dtMain.DefaultView.ToTable(true, "BrandCD", "SportsCD", "SegmentCD", "LastSeason", "ChangeDate", "Rate");
                        //dgv_ShiireKakeritsu.DataSource = distinctTable;
                        //foreach (DataGridViewRow drow in dgv_ShiireKakeritsu.Rows)
                        //{

                        //    drow.Cells["colChk"].Value = true;
                        //}
                        //dtMain = dtGrid.Copy();

                        //if (dgv_ShiireKakeritsu.Contains(dtGrid.DataSet.ToString()))

                        //    dtGrid.Columns[0].DefaultValue = "1";

                    }
                }
                else
                {
                    dtGrid = dtMain;
                }

                dgv_ShiireKakeritsu.DataSource = dtMain;
                foreach (DataGridViewRow drow in dgv_ShiireKakeritsu.Rows)
                {
                    if (drow.Cells["Column1"].Value.ToString() == "1")
                    {
                        drow.Cells["colChk"].Value = true;
                    }
                    else
                    {
                        drow.Cells["colChk"].Value = false;
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if (this.dgv_ShiireKakeritsu.SelectedRows.Count > 0)
            //{
            //    dgv_ShiireKakeritsu.Rows.RemoveAt(this.dgv_ShiireKakeritsu.SelectedRows[0].Index);
            //}
            foreach (DataGridViewRow item in this.dgv_ShiireKakeritsu.SelectedRows)
            {
                dgv_ShiireKakeritsu.Rows.RemoveAt(item.Index);
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            DataGridViewRow dgvRow = new DataGridViewRow();
            dgvRow.Cells[0].Value = scBrandCD.TxtCode.Text;
            dgvRow.Cells[0].Value = scSportsCD.TxtCode.Text;
            dgvRow.Cells[0].Value = scSegmentCD.TxtCode.Text;
            dgvRow.Cells[0].Value = txtYear.Text;
            dgvRow.Cells[0].Value = txtLastSeason.Text;
            dgvRow.Cells[0].Value = txtChangeDate.Text;
            dgvRow.Cells[0].Value = txtRate.Text;
        }
       
        }
}
