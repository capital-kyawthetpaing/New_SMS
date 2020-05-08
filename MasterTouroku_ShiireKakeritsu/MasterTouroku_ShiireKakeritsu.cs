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
        M_Vendor_Entity mve = new M_Vendor_Entity();
        M_Brand_Entity mbe = new M_Brand_Entity();
        int type = 0;

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
            ckM_Button1.Text = "取込(F10)";
            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            BindCombo();
            SetRequiredField();
            scSupplierCD.SetFocus(1);
            txtDate1.Text = DateTime.Now.ToString("yyyy/MM/dd");
        }

        public void BindCombo()
        {
            cbo_Store.Bind(string.Empty, "2");
            cbo_Store.SelectedValue = StoreCD;
            string ymd = bbl.GetDate();
            cbo_Year.Bind(ymd);
            cbo_Year1.Bind(ymd);
            cbo_Season.Bind(ymd);
            cbo_Season1.Bind(ymd);
        }

        private void SetRequiredField()
        {
            scSupplierCD.TxtCode.Require(true);
            txtDate1.Require(true);
            txtRevisionDate.Require(true);
            txtRate1.Require(true);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        public void CancelData()
        {
            //scSupplierCD.Clear();
            //txtDate1.Text = string.Empty;
            //scBrandCD1.Clear();
            //scSportsCD1.Clear();
            //scSegmentCD1.Clear();
            //cbo_Year1.Text = string.Empty;
            //cbo_Season1.Text = string.Empty;
            //txtDate.Text = string.Empty;
            //txtCopy.Text = string.Empty;
            //scBrandCD.Clear();
            //scSportsCD.Clear();
            //scSegmentCD.Clear();
            //cbo_Year.Text = string.Empty;
            //cbo_Season.Text = string.Empty;
            //txtChangeDate.Text = string.Empty;
            //txtRate.Text = string.Empty;
            Clear(panelDetail);
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

        private bool ErrorCheck(int type)
        {
            if(type == 1)
            {
                if (!RequireCheck(new Control[] { scSupplierCD.TxtCode }))
                    return false;
                //else
                //{
                //    mve.VendorCD = scSupplierCD.TxtCode.Text;
                //    mve.ChangeDate = txtDate1.Text;
                //    DataTable dtvendor = new DataTable();
                //    dtvendor = mskbl.M_Vendor_Select(mve);
                //    if(dtvendor.Rows.Count == 0)
                //    {
                //        mskbl.ShowMessage("E101");
                //        scSupplierCD.SetFocus(1);
                //        return false;
                //    }
                //    else
                //    {
                //        if(dtvendor.Rows[0]["DeleteFlg"].ToString() == "1")
                //        {
                //            mskbl.ShowMessage("E119");
                //            scSupplierCD.SetFocus(1);
                //            return false;
                //        }
                //    }
                //}

                //if(string.IsNullOrWhiteSpace(txtDate1.Text))
                //{
                //    mskbl.ShowMessage("E102");
                //    txtDate1.Focus();
                //    return false;
                //}

                //if (string.IsNullOrWhiteSpace(txtRevisionDate.Text))
                //{
                //    mskbl.ShowMessage("E102");
                //    txtRevisionDate.Focus();
                //    return false;
                //}

                //if (string.IsNullOrWhiteSpace(txtRate1.Text))
                //{
                //    mskbl.ShowMessage("E102");
                //    txtRate1.Focus();
                //    return false;
                //}

                //if (!((rdoAllStores.Checked == true) || (rdoIndividualStores.Checked == true)))
                //{
                //    mskbl.ShowMessage("E102");
                //    return false;
                //}

                //if (scSupplierCD.IsExists(1))
                //{
                //    mskbl.ShowMessage("E119");
                //    scSupplierCD.SetFocus(1);
                //    return false;
                //}
                //if (!RequireCheck(new Control[] { txtDate1 }))
                //    return false;
            }
            else if(type == 2)
            {

                if (string.IsNullOrWhiteSpace(scBrandCD.TxtCode.Text))
                {
                    mskbl.ShowMessage("E102");
                    scBrandCD.SetFocus(1);
                    return false;
                }
                else
                {
                    mbe.BrandCD = scBrandCD.TxtCode.Text;
                    DataTable dtbrand = mskbl.M_BrandSelect(mbe);
                    if (dtbrand.Rows.Count == 0)
                    {
                        mskbl.ShowMessage("E101");
                        scBrandCD.SetFocus(1);
                        return false;
                    }
                    else
                    {
                        scBrandCD.LabelText = dtbrand.Rows[0]["BrandName"].ToString();
                    }
                }

                scSportsCD.ChangeDate = txtDate1.Text;
                if (!scSportsCD.SelectData())
                {
                    mskbl.ShowMessage("E101");
                    scSportsCD.SetFocus(1);
                    return false;
                }

                scSegmentCD.ChangeDate = txtDate1.Text;
                if (!scSegmentCD.SelectData())
                {
                    mskbl.ShowMessage("E101");
                    scSegmentCD.SetFocus(1);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtChangeDate.Text))
                {
                    mskbl.ShowMessage("E102");
                    txtChangeDate.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtRate.Text))
                {
                    mskbl.ShowMessage("E102");
                    txtRate.Focus();
                    return false;
                }
            }

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
                //LastYearTerm=txtYear.Text,
                LastYearTerm = cbo_Year1.SelectedText,
                LastSeason = cbo_Season1.SelectedText,
                //LastSeason = txtSeason.Text,
                ChangeDate = txtDate.Text,
                Rate = txtRate.Text
            };
            return moe;
        }
      
        private void SearchData()
        {
            if (ErrorCheck(1))
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
            //if (!string.IsNullOrWhiteSpace(txtYear.Text))
            //    searchCondition = "LastYearTerm='" + txtYear.Text + "'";
            //if (!string.IsNullOrWhiteSpace(txtSeason.Text))
            //    searchCondition = "LastSeason= '" + txtSeason.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtDate.Text))
                searchCondition = "ChangeDate= '" + txtDate.Text;

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

        private void dgv_ShiireKakeritsu_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if ((Convert.ToBoolean(dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells["colChk"].EditedFormattedValue) == true))
            {
                DataGridViewCheckBoxCell chk1 = dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells["colChk"] as DataGridViewCheckBoxCell;
                foreach (DataGridViewRow row1 in dgv_ShiireKakeritsu.Rows)
                {
                    DataGridViewCheckBoxCell colChk = row1.Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                    chk1.Value = chk1.TrueValue;
                }
                dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
            }
            else
            {
                dgv_ShiireKakeritsu.ClearSelection();
            }
    
            //if ((Convert.ToBoolean(dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells["colChk"].EditedFormattedValue) == true))
            //{
            //    foreach (DataGridViewRow row1 in dgv_ShiireKakeritsu.Rows)
            //    {
            //        DataGridViewCheckBoxCell chk1 = row1.Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
            //        chk1.Value = chk1.FalseValue;
            //    }
            //    dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
            //}
            //else
            //{
            //    dgv_ShiireKakeritsu.ClearSelection();
            //}

        }


        #region KeyDown Event For 【抽出条件】
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

        private void scSportsCD1_Enter(object sender, EventArgs e)
        {
            scSportsCD1.Value1 = "202";
        }

        private void scSegmentCD1_Enter(object sender, EventArgs e)
        {
            scSegmentCD1.Value1 = "203";
        }


        #endregion       
        

        #region ButtonClick for 【抽出条件】

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCopy.Text))
            {
                mskbl.ShowMessage("E102");
                txtCopy.Focus();
            }
            else
            {            
                foreach (DataGridViewRow row in dgv_ShiireKakeritsu.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                    if (chk.Value == chk.TrueValue)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }


        #endregion

        #region KeyDown Event For【追加・一括変更・選択】	
        private void scBrandCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {              
                if (!string.IsNullOrEmpty(scBrandCD1.TxtCode.Text))
                {
                    mbe.BrandCD = scBrandCD.TxtCode.Text;
                    mbe.ChangeDate = txtDate1.Text;
                    DataTable dtbrand = mskbl.M_BrandSelect(mbe);
                    if (dtbrand.Rows.Count > 0)
                    {
                        scBrandCD.LabelText = dtbrand.Rows[0]["BrandName"].ToString();
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
                    //else
                    //{
                    //    scSportsCD.SetFocus(1);
                    //}
                }
            }
        }

        private void scSegmentCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {            
                if (!string.IsNullOrEmpty(scSegmentCD.TxtCode.Text))
                {
                    scSegmentCD.ChangeDate = bbl.GetDate();
                    if (scSegmentCD.SelectData())
                    {
                        scSegmentCD.Value1 = scSegmentCD.TxtCode.Text;
                        scSegmentCD.Value2 = scSegmentCD.LabelText;
                    }
                    //else
                    //{
                    //    scSegmentCD.SetFocus(1);
                    //}
                }
            }
        }

        private void scSportsCD_Enter(object sender, EventArgs e)
        {
            scSportsCD.Value1 = "202";
        }

        private void scSegmentCD_Enter(object sender, EventArgs e)
        {
            scSegmentCD.Value1 = "203";
        }

        #endregion

        #region Button Click For 【追加・一括変更・選択】	

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
                    }
                }
                else
                {
                    dtGrid = dtMain;
                }

                dgv_ShiireKakeritsu.DataSource = dtMain;
                foreach (DataGridViewRow drow in dgv_ShiireKakeritsu.Rows)
                {
                    if (drow.Cells["col1"].Value.ToString() == "1")
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ErrorCheck(2))
            {
                if (dgv_ShiireKakeritsu.Rows.Count == 0)
                {
                    dt.Columns.Add("BrandCD");
                    dt.Columns.Add("SportsCD");
                    dt.Columns.Add("SegmentCD");
                    dt.Columns.Add("LastYearTerm");
                    dt.Columns.Add("LastSeason");
                    dt.Columns.Add("ChangeDate");
                    dt.Columns.Add("Rate");

                    DataRow dtRow = dt.NewRow();
                    dtRow["BrandCD"] = scBrandCD.TxtCode.Text;
                    dtRow["SportsCD"] = scSportsCD.TxtCode.Text;
                    dtRow["SegmentCD"] = scSegmentCD.TxtCode.Text;
                    dtRow["LastYearTerm"] = cbo_Year.Text;
                    dtRow["LastSeason"] = cbo_Season.Text;
                    dtRow["ChangeDate"] = txtChangeDate.Text;
                    dtRow["Rate"] = Convert.ToDecimal(txtRate.Text);
                    dt.Rows.Add(dtRow);
                    dgv_ShiireKakeritsu.DataSource = dt;
                }
                else
                {
                    DataRow row = dtMain.NewRow();
                    row["BrandCD"] = scBrandCD.TxtCode.Text;
                    row["SportsCD"] = scSportsCD.TxtCode.Text;
                    row["SegmentCD"] = scSegmentCD.TxtCode.Text;
                    row["LastYearTerm"] = cbo_Year.Text;
                    row["LastSeason"] = cbo_Season.Text;
                    row["ChangeDate"] = txtChangeDate.Text;
                    row["Rate"] = Convert.ToDecimal(txtRate.Text);
                    dtMain.Rows.Add(row);
                    dgv_ShiireKakeritsu.DataSource = dtMain;
                }

            }



        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<DataRow> toDelete = new List<DataRow>();
            for (int i = 0; i < dgv_ShiireKakeritsu.Rows.Count; i++)
            {
                {
                    DataGridViewRow row = dgv_ShiireKakeritsu.Rows[i];
                    DataGridViewCheckBoxCell check = row.Cells[0] as DataGridViewCheckBoxCell;
                    if (check.Value == check.TrueValue)
                    {
                        DataRow dataRow = (row.DataBoundItem as DataRowView).Row;
                        toDelete.Add(dataRow);
                    }
                }
            }
            toDelete.ForEach(row => row.Delete());
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_ShiireKakeritsu.Rows)
            {
                DataGridViewCheckBoxCell check = row.Cells[0] as DataGridViewCheckBoxCell;
                if (check.Value == check.TrueValue)
                {
                    row.Cells["colRate1"].Value = Convert.ToDecimal(txtRate.Text);
                }
            }
        }

        #endregion
    }
}