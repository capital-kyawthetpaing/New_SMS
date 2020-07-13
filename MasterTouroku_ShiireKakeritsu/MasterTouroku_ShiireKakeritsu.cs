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
using System.Diagnostics;
using System.IO;
using ClosedXML.Excel;
using Search;
using ExcelDataReader;

namespace MasterTouroku_ShiireKakeritsu
{
    public partial class frmMasterTouroku_ShiireKakeritsu : FrmMainForm
    {
        MasterTouroku_ShiireKakeritsu_BL mskbl;
        M_OrderRate_Entity moe;
        DataTable dtMain;
        DataTable dtGrid;
        DataTable dt = new DataTable();
        M_Vendor_Entity mve;
        M_Brand_Entity mbe;
        DataView dvMain;
        L_Log_Entity log_data;
        DataTable dtAdd;
        int type = 0;
        string Xml;
        public bool IsNumber { get; set; } = true;
        public bool MoveNext { get; set; } = true;

        public frmMasterTouroku_ShiireKakeritsu()
        {
            InitializeComponent();
            mskbl = new MasterTouroku_ShiireKakeritsu_BL();
            moe = new M_OrderRate_Entity();
            mve = new M_Vendor_Entity();
            mbe = new M_Brand_Entity();
            dvMain = new DataView();
        }

        private void frmMasterTouroku_ShiireKakeritsu_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_ShiireKakeritsu";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            BindCombo();
            SetRequiredField();
            scSupplierCD.SetFocus(1);
            txtDate1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            RadioCheck();
            dgv_ShiireKakeritsu.AllowUserToAddRows = false;
        }
        private void RadioCheck()
        {
            if(rdoAllStores.Checked==true)
            {
                cbo_Store.SelectedValue = "0000";
                cbo_Store.Enabled = false;
            }
            else
            {
                cbo_Store.SelectedValue = StoreCD;
                cbo_Store.Enabled = true;
            }
        }
        private void rdoAllStores_CheckedChanged(object sender, EventArgs e)
        {
            RadioCheck();
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
            txtDate1.Text = string.Empty;
            scBrandCD1.Clear();
            scSportsCD1.Clear();
            scSegmentCD1.Clear();
            cbo_Year1.Text = string.Empty;
            cbo_Season1.Text = string.Empty;
            txtDate.Text = string.Empty;
            txtCopy.Text = string.Empty;
            scBrandCD.Clear();
            scSportsCD.Clear();
            scSegmentCD.Clear();
            cbo_Year.Text = string.Empty;
            cbo_Season.Text = string.Empty;
            txtChangeDate.Text = string.Empty;
            txtRate.Text = string.Empty;
            //dgv_ShiireKakeritsu.DataSource = null;
            //Clear(panelDetail);
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
                        dgv_ShiireKakeritsu.DataSource = null;
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

        private bool ErrorCheck(int type)
        {
            if (type == 1)
            {
                if (!RequireCheck(new Control[] { scSupplierCD.TxtCode }))
                    return false;
                //else
                //{
                //    mve.VendorCD = scSupplierCD.TxtCode.Text;
                //    mve.ChangeDate = txtDate1.Text;
                //    DataTable dtvendor = new DataTable();
                //    dtvendor = mskbl.M_Vendor_Select(mve);
                //    if (dtvendor.Rows.Count == 0)
                //    {
                //        mskbl.ShowMessage("E101");
                //        scSupplierCD.SetFocus(1);
                //        return false;
                //    }
                    //else
                    //{
                    //    if (dtMain.Rows[0]["DeleteFlg"].ToString() == "1")
                    //    {
                    //        mskbl.ShowMessage("E119");
                    //        scSupplierCD.SetFocus(1);
                    //        return false;
                    //    }
                    //}
                }
            //}
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
            else if (type == 2)
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
            else if (type == 3)
            {
                if (!RequireCheck(new Control[] { scSupplierCD.TxtCode }))
                    return false;

                if (string.IsNullOrWhiteSpace(txtRevisionDate.Text))
                {
                    mskbl.ShowMessage("E102");
                    txtRevisionDate.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtRate1.Text))
                {
                    mskbl.ShowMessage("E102");
                    txtRate1.Focus();
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
                StoreCD=cbo_Store.SelectedValue.ToString(),
                BrandCD = scBrandCD1.TxtCode.Text,
                SportsCD = scSportsCD1.TxtCode.Text,
                SegmentCD = scSegmentCD1.TxtCode.Text,
                LastYearTerm = cbo_Year1.SelectedText,
                LastSeason = cbo_Season1.SelectedText,
                ChangeDate = txtDate.Text,
                Rate = txtRate.Text,
                ProcessMode = ModeText,
                ProgramID = InProgramID,
                InsertOperator = InOperatorCD,
                Key = scSupplierCD.Code,
                PC = InPcID
            };
            return moe;
        }
       

        private void frmMasterTouroku_ShiireKakeritsu_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        
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
                        SearchData();
                    }
                    else
                    {
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
                        SearchData();
                    }
                    else
                    {
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
                        SearchData();
                    }
                    else
                    {
                        scSegmentCD1.SetFocus(1);
                    }
                }
            }
        }

        private void scSportsCD1_Enter(object sender, EventArgs e)
        {
            scSportsCD1.Value3 = "202";
        }

        private void scSegmentCD1_Enter(object sender, EventArgs e)
        {
            scSegmentCD1.Value3 = "203";
        }
        private void dgv_ShiireKakeritsu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                string ck = dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells["colChk"].State.ToString();
                if (ck == "Selected")
                {
                    dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells["colChk"].Value = "true";
                }
                else
                {
                    dgv_ShiireKakeritsu.Rows[e.RowIndex].Cells["colChk"].Value = "false";
                }
            }
        }

        #endregion

        #region ButtonClick for 【抽出条件】
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //SearchData();
            //BindGrid();
                string searchCondition = string.Empty;
                if (!string.IsNullOrWhiteSpace(scBrandCD1.TxtCode.Text))
                    searchCondition = "BrandCD = '" + scBrandCD1.TxtCode.Text + "'";
                if (!string.IsNullOrWhiteSpace(scSportsCD1.TxtCode.Text))
                    searchCondition = "SportsCD='" + scSportsCD1.TxtCode.Text + "'";
                if (!string.IsNullOrWhiteSpace(scSegmentCD1.TxtCode.Text))
                    searchCondition = "SegmentCD= '" + scSegmentCD1.TxtCode.Text + "'";
                if (!string.IsNullOrWhiteSpace(cbo_Year1.Text))
                    searchCondition = "LastYearTerm='" + cbo_Year1.Text + "'";
                if (!string.IsNullOrWhiteSpace(cbo_Season1.Text))
                    searchCondition = "LastSeason= '" + cbo_Season1.Text + "'";
                if (!string.IsNullOrWhiteSpace(txtDate.Text))
                    searchCondition = "ChangeDate= '" + txtDate.Text;
                if (dgv_ShiireKakeritsu.DataSource != null)
                {
                    dgv_ShiireKakeritsu.DataSource = dtMain;
                    DataView view = dgv_ShiireKakeritsu.DataSource as DataView;
                    dvMain.RowFilter = searchCondition;
                    dgv_ShiireKakeritsu.DataSource = dvMain;

                }
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
            if (!string.IsNullOrWhiteSpace(cbo_Year1.Text))
                searchCondition = "LastYearTerm='" + cbo_Year1.Text + "'";
            if (!string.IsNullOrWhiteSpace(cbo_Season1.Text))
                searchCondition = "LastSeason= '" + cbo_Season1.Text + "'";
            if (!string.IsNullOrWhiteSpace(txtDate.Text))
                searchCondition = "ChangeDate= '" + txtDate.Text;

            //if (!string.IsNullOrWhiteSpace(searchCondition))
            //{
            //dvMain = new DataView(dtMain, searchCondition, "", DataViewRowState.CurrentRows);

            moe = GetSearchInfo(); 
             dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
            dvMain = new DataView(dtMain);
            dgv_ShiireKakeritsu.DataSource = dvMain; 

            //    DataRow[] dr = dtMain.Select(searchCondition);
            //    if (dr.Count() > 0)
            //    {
            //        dtGrid = dvmain//dtMain.Select(searchCondition).CopyToDataTable();
            //    }
            //    else
            //        dtGrid = null;
            //}
            //else
            //{
            //    dtGrid = dtMain;
            //}
        }
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
                    if (row.Cells["colChk"].Value != null)
                    {
                        string check = row.Cells["colChk"].Value.ToString();
                        if (chk.Value == chk.TrueValue || check=="True")
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
            scSportsCD.Value3 = "202";
        }

        private void scSegmentCD_Enter(object sender, EventArgs e)
        {
            scSegmentCD.Value3 = "203";
        }

        #endregion

        #region Button Click For 【追加・一括変更・選択】	

        private void btnChoice_Click(object sender, EventArgs e)
        {
            //dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
            dgv_ShiireKakeritsu.DataSource = dtMain;
            if (dgv_ShiireKakeritsu.Rows.Count > 0)
            {
                string searchCondition = string.Empty;
                if (!string.IsNullOrWhiteSpace(scBrandCD.TxtCode.Text))
                    searchCondition = "BrandCD = '" + scBrandCD.TxtCode.Text + "'";
                if (!string.IsNullOrWhiteSpace(scSportsCD.TxtCode.Text))
                    searchCondition = "SportsCD='" + scSportsCD.TxtCode.Text + "'";
                if (!string.IsNullOrWhiteSpace(scSegmentCD.TxtCode.Text))
                    searchCondition = "SegmentCD= '" + scSegmentCD.TxtCode.Text + "'";
                if (!string.IsNullOrWhiteSpace(cbo_Year.Text))
                    searchCondition = "LastYearTerm='" + cbo_Year.Text + "'";
                if (!string.IsNullOrWhiteSpace(cbo_Season.Text))
                    searchCondition = "LastSeason= '" + cbo_Season.Text + "'";
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
            dgv_ShiireKakeritsu.RefreshEdit();
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
                    CancelData();
                    scBrandCD.SetFocus(1);
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
                    CancelData();
                    scBrandCD.SetFocus(1);
                }
            }
        }
       
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_ShiireKakeritsu.Rows)
            {
               DataGridViewCheckBoxCell check = row.Cells[0] as DataGridViewCheckBoxCell;
                if (row.Cells["colChk"].Value != null)
                {
                    string chk = row.Cells["colChk"].Value.ToString();
                    if (check.Value == check.TrueValue || chk=="True")
                    {
                        row.Cells["colRate1"].Value = Convert.ToDecimal(txtRate.Text);
                    }
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
                    if (row.Cells["colChk"].Value != null)
                    {
                        string chk = row.Cells["colChk"].Value.ToString();

                        if (check.Value == check.TrueValue || chk == "True")
                        {
                            DataRow dataRow = (row.DataBoundItem as DataRowView).Row;
                            toDelete.Add(dataRow);
                        }
                    }
                }
            }
            toDelete.ForEach(row => row.Delete());

            DataView view = dgv_ShiireKakeritsu.DataSource as DataView;
            //dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
            //dtMain = view.Table;
            dgv_ShiireKakeritsu.DataSource = dvMain;
            //dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
            //dvMain = new DataView(dtMain);
            //dgv_ShiireKakeritsu.DataSource = dvMain;
        }
        #endregion

        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (NumberCheck())
                {
                    if (!txtRate.Text.Contains("."))
                    {
                        txtRate.Text =txtRate.Text + ".00";
                    }
                } 
            }
        }

        /// <summary>
        /// For Rate Textbox
        /// </summary>
        /// <returns></returns>
        private bool NumberCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtRate.Text) && !bbl.IsInteger(txtRate.Text))
            {
                IsNumber = false;
                mskbl.ShowMessage("E118");
                return false;
            }
            MoveNext = true;
            return true;
        }
        public static bool IsInteger(string value)
        {
            value = value.Replace("-", "");
            if (Int64.TryParse(value, out Int64 Num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void F11()
        {
            moe = GetSearchInfo();
            dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
            if (dtMain.Rows.Count > 0)
            {
                dgv_ShiireKakeritsu.DataSource = dtMain;
            }
            else
            {
                mskbl.ShowMessage("E128");
                dgv_ShiireKakeritsu.DataSource = null;
                scSupplierCD.SetFocus(1);
            }
        }
        private void F12()
        {
            if (ErrorCheck(3))
            {
                if (mskbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                        UpdateInsert();
                }
            }
        }
        private void UpdateInsert()
        {
            dtMain = dtAdd;
            Xml = mskbl.DataTableToXml(dtMain);
            log_data = Get_Log_Data();
            moe.VendorCD = scSupplierCD.TxtCode.Text;
            moe.ChangeDate = txtRevisionDate.Text;
            moe.Rate = txtRate1.Text;
            if (mskbl.M_OrderRate_Update(moe, Xml, log_data))
            {
                Clear(PanelHeader);
                Clear(panelDetail);
                dgv_ShiireKakeritsu.DataSource = string.Empty;
                mskbl.ShowMessage("I101");
                scSupplierCD.SetFocus(1);
            }
            else
            {
                mskbl.ShowMessage("S001");
            }
        }

        private void txtRate1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (NumberCheck())
                {
                    if (!txtRate1.Text.Contains("."))
                    {
                        txtRate1.Text = txtRate1.Text + ".00";
                    }
                }
            }
        }
        protected DataTable ChangeDataColumnName(DataTable dtMain)
        {
            dtMain.Columns["仕入先CD"].ColumnName = "VendorCD";
            dtMain.Columns["ブランドCD"].ColumnName = "BrandCD";
            dtMain.Columns["競　技CD"].ColumnName = "SportsCD";
            dtMain.Columns["商品分類CD"].ColumnName = "SegmentCD";
            dtMain.Columns["年度"].ColumnName = "LastYearTerm";
            dtMain.Columns["シーズン"].ColumnName = "LastSeason";
            dtMain.Columns["改定日"].ColumnName = "ChangeDate";
            dtMain.Columns["掛率"].ColumnName = "Rate";
            //dtMain.Columns["VendorCD"].ColumnName = "仕入先CD";
            //dtMain.Columns["StoreCD"].ColumnName = "店舗CD";
            //dtMain.Columns["BrandCD"].ColumnName = "ブランドCD";
            //dtMain.Columns["SportsCD"].ColumnName = "競　技CD";
            //dtMain.Columns["SegmentCD"].ColumnName = "商品分類CD";
            //dtMain.Columns["LastYearTerm"].ColumnName = "年度";
            //dtMain.Columns["LastSeason"].ColumnName = "シーズン";
            //dtMain.Columns["ChangeDate"].ColumnName = "改定日";
            //dtMain.Columns["Rate"].ColumnName = "掛率";
            return dtMain;
        }
        protected DataTable ExcelToDatatable(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            string ext = Path.GetExtension(filePath);
            IExcelDataReader excelReader;
            if (ext.Equals(".xls"))
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else if (ext.Equals(".xlsx"))
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            else
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx) 
                excelReader = ExcelReaderFactory.CreateCsvReader(stream, null);

            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            bool useHeaderRow = true;

            DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = useHeaderRow,
                }
            });


            excelReader.Close();
            return result.Tables[0];
        }
        private void F10()
        {
            OpenFileDialog op = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                RestoreDirectory = true
            };
                if (op.ShowDialog() == DialogResult.OK)
                {
                    string str = op.FileName;
                    string ext = Path.GetExtension(str);
                    if (!(ext.Equals(".xls") || ext.Equals(".xlsx")))
                    {
                        mskbl.ShowMessage("E137");
                    }
                    else
                    {
                        DataTable dtExcel = ExcelToDatatable(str);
                        string[] colname = { "仕入先CD", "店舗CD", "改定日", "掛率" };
                    if (CheckColumn(colname, dtExcel))
                    {
                        //    foreach (DataRow row in dtMain.Rows)
                        //    {
                        //        if (row["仕入先CD"].ToString() != scSupplierCD.TxtCode.Text)
                        //        {
                        //            mskbl.ShowMessage("E230");
                        //        }
                        //        else if (row["店舗CD"] != DBNull.Value && row["店舗CD"].ToString() != "0000")
                        //        {
                        //            mskbl.ShowMessage("E138");
                        //        }
                        //        else if (row["ブランドCD"].ToString() == scBrandCD.TxtCode.Text)
                        //        {
                        //            mskbl.ShowMessage("E138");
                        //        }
                        //        else if (row["ブランドCD"] == DBNull.Value && row["競　技CD"] != DBNull.Value)
                        //        {
                        //            if (mskbl.SimpleSelect1("64", string.Empty, "202", row["競　技CD"].ToString()).Rows.Count < 0)
                        //            {
                        //                mskbl.ShowMessage("E138");
                        //            }
                        //        }
                        //        else if (row["競　技CD"] == DBNull.Value && row["商品分類CD"] != DBNull.Value)
                        //        {
                        //            mskbl.ShowMessage("E229");
                        //        }
                        //        else if (row["商品分類CD"] == DBNull.Value)
                        //        {
                        //            if (mskbl.SimpleSelect1("64", string.Empty, "203", row["商品分類CD"].ToString()).Rows.Count < 0)
                        //            {
                        //                mskbl.ShowMessage("E138");
                        //            }
                        //        }
                        //        else if (row["商品分類CD"] == DBNull.Value && row["年度"] != DBNull.Value)
                        //        {
                        //            mskbl.ShowMessage("E229");
                        //        }
                        //        else if (row["年度"] == DBNull.Value)
                        //        {
                        //            if (mskbl.SimpleSelect1("64", string.Empty, "307", row["年度"].ToString()).Rows.Count < 0)
                        //            {
                        //                mskbl.ShowMessage("E138");
                        //            }
                        //        }
                        //        else if (row["年度"] == DBNull.Value && row["シーズン"] != DBNull.Value)
                        //        {
                        //            mskbl.ShowMessage("E229");
                        //        }
                        //        else if (row["シーズン"] == DBNull.Value)
                        //        {
                        //            if (mskbl.SimpleSelect1("64", string.Empty, "308", row["シーズン"].ToString()).Rows.Count < 0)
                        //            {
                        //                mskbl.ShowMessage("E138");
                        //            }
                        //        }
                        //        else if (row["改定日"] == DBNull.Value)
                        //        {
                        //            mskbl.ShowMessage("E103");
                        //        }
                        //    }
                       // Xml = mskbl.DataTableToXml(dtExcel);
                        if (dtExcel.Rows.Count > 0)
                        {
                            //dgv_ShiireKakeritsu.DataSource = dtExcel;
                            //DataRow row1 = null;
                            //for (int i = 0; i < dtExcel.Rows.Count; i++)
                            //{
                            //row1["BrandCD"] = dtExcel.Rows[i]["仕入先CD"];
                            //row1["SportsCD"] = dtExcel.Rows[i]["SportsCD"];
                            //row1["SegmentCD"] = dtExcel.Rows[i]["SegmentCD"];
                            //row1["LastYearTerm"] = dtExcel.Rows[i]["LastYearTerm"];
                            //row1["LastSeason"] = dtExcel.Rows[i]["LastSeason"];
                            //row1["ChangeDate"] = dtExcel.Rows[i]["ChangeDate"];
                            //row1["Rate"] = dtExcel.Rows[i]["Rate"];
                            //dgv_ShiireKakeritsu.Rows("colBrandCD1") = dtExcel.Columns("BrandCD");
                            dtAdd = new DataTable();
                            dtAdd = ChangeDataColumnName(dtExcel);
                            dgv_ShiireKakeritsu.DataSource = dtAdd;
                        }
                    }
                    else
                    {
                        mskbl.ShowMessage("E137");
                    }
                }
                }
        }
        protected Boolean CheckColumn(String[] colName,DataTable dtMain) //Check Columns exist in import excel
        {
            DataColumnCollection col = dtMain.Columns;
            {
                if (!dtMain.Columns[0].ColumnName.ToString().Equals("仕入先CD"))
                {
                    return false;
                }
                else if (!dtMain.Columns[1].ColumnName.ToString().Equals("店舗CD"))
                {
                    return false;
                }
                else if (!dtMain.Columns[7].ColumnName.ToString().Equals("改定日"))
                {
                    return false;
                }
                else if (!dtMain.Columns[8].ColumnName.ToString().Equals("掛率"))
                {
                    return false;
                }
            }
            return true;
        }

        //private bool ErrorCheckForF10()
        //{
        //    if (dtMain != null)
        //    {
        //        foreach (DataRow row in dtMain.Rows)
        //        {
        //            if (row["仕入先CD"].ToString() != scSupplierCD.TxtCode.Text)
        //            {
        //                mskbl.ShowMessage("E230");
        //                return false;
        //            }
        //            //else if (row["店舗CD"] != DBNull.Value && row["店舗CD"].ToString() != "0000")
        //            //{
        //            //    mskbl.ShowMessage("E138");
        //            //    return false;
        //            //}
        //            else if (row["ブランドCD"].ToString() == scBrandCD.TxtCode.Text)
        //            {
        //                mskbl.ShowMessage("E138");
        //                return false;
        //            }
        //            else if (row["ブランドCD"] == DBNull.Value && row["競　技CD"] != DBNull.Value)
        //            {
        //                if (mskbl.SimpleSelect1("64", string.Empty, "202", row["競　技CD"].ToString()).Rows.Count < 0)
        //                {
        //                    mskbl.ShowMessage("E138");
        //                    return false;
        //                }
        //            }
        //            else if (row["競　技CD"] == DBNull.Value && row["商品分類CD"] != DBNull.Value)
        //            {
        //                mskbl.ShowMessage("E229");
        //                return false;
        //            }
        //            else if (row["商品分類CD"] == DBNull.Value)
        //            {
        //                if (mskbl.SimpleSelect1("64", string.Empty, "203", row["商品分類CD"].ToString()).Rows.Count < 0)
        //                {
        //                    mskbl.ShowMessage("E138");
        //                    return false;
        //                }
        //            }
        //            else if (row["商品分類CD"] == DBNull.Value && row["年度"] != DBNull.Value)
        //            {
        //                mskbl.ShowMessage("E229");
        //                return false;
        //            }
        //            else if (row["年度"] == DBNull.Value)
        //            {
        //                if (mskbl.SimpleSelect1("64", string.Empty, "307", row["年度"].ToString()).Rows.Count < 0)
        //                {
        //                    mskbl.ShowMessage("E138");
        //                    return false;
        //                }
        //            }
        //            else if (row["年度"] == DBNull.Value && row["シーズン"] != DBNull.Value)
        //            {
        //                mskbl.ShowMessage("E229");
        //                return false;
        //            }
        //            else if (row["シーズン"] == DBNull.Value)
        //            {
        //                if (mskbl.SimpleSelect1("64", string.Empty, "308", row["シーズン"].ToString()).Rows.Count < 0)
        //                {
        //                    mskbl.ShowMessage("E138");
        //                    return false;
        //                }
        //            }
        //            else if (row["改定日"] == DBNull.Value)
        //            {
        //                mskbl.ShowMessage("E103");
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            //moe = new M_OrderRate_Entity();
            //moe = GetSearchInfo();
            //DataTable dt = mskbl.M_ShiireKakeritsu_Select(moe);
            //if (dt.Rows.Count > 0)
            //{
            //    DataTable dtExport = dt;
            //dtExport = ChangeDataColumnName(dtMain);
            //    string folderPath = "C:\\SSS\\";
            //    if (!Directory.Exists(folderPath))
            //    {
            //        Directory.CreateDirectory(folderPath);
            //    }
            //    SaveFileDialog savedialog = new SaveFileDialog();
            //    savedialog.Filter = "Excel Files|*.xlsx;";
            //    savedialog.Title = "Save";
            //    savedialog.FileName = "仕入先別発注掛率マスタ";
            //    savedialog.InitialDirectory = folderPath;

            //    savedialog.RestoreDirectory = true;
            //    if (savedialog.ShowDialog() == DialogResult.OK)
            //    {
            //        if (Path.GetExtension(savedialog.FileName).Contains(".xlsx"))
            //        {
            //            using (XLWorkbook wb = new XLWorkbook())
            //            {
            //                wb.Worksheets.Add(dtMain, "Result");
            //                wb.SaveAs(savedialog.FileName);
            //            }
            //            //Process.Start(Path.GetDirectoryName(savedialog.FileName));
            //        }
            //    }
               F10();
            //}
        }

        private void cbo_Store_KeyDown(object sender, KeyEventArgs e)
        {
            if (!base.CheckAvailableStores(cbo_Store.SelectedValue.ToString()))
            {
                bbl.ShowMessage("E141");
                cbo_Store.Focus();
            }
        }
        private L_Log_Entity Get_Log_Data()
        {
            log_data = new L_Log_Entity()
            {
                Program = "MasterTouroku_ShiireKakeritsu",
                PC = InPcID,
                OperateMode = string.Empty,
                Operator = InOperatorCD,
                KeyItem = string.Empty
            };
            return log_data;
        }
        
    }
}