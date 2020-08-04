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
    public partial class MasterTouroku_ShiireKakeritsu : FrmMainForm
    {
        MasterTouroku_ShiireKakeritsu_BL mskbl;
        M_OrderRate_Entity moe;
        DataTable dtMain;
        DataTable dtGrid;
        DataTable dtDel;
        DataTable dt;
        M_Vendor_Entity mve;
        M_Brand_Entity mbe;
        DataView dvMain;
        L_Log_Entity log_data;
        DataTable dtExcel;
        //int type = 0;

        public MasterTouroku_ShiireKakeritsu()
        {
            InitializeComponent();
            mskbl = new MasterTouroku_ShiireKakeritsu_BL();
            moe = new M_OrderRate_Entity();
            mve = new M_Vendor_Entity();
            mbe = new M_Brand_Entity();
            dvMain = new DataView();
            dtDel = new DataTable();
            dt = new DataTable();
        }
        private void MasterTouroku_ShiireKakeritsu_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_ShiireKakeritsu";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            F2Visible = false;
            BindCombo();
            SetRequiredField();
            scSupplierCD.SetFocus(1);
            txtDate1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            RadioCheck();
            dgv_ShiireKakeritsu.DisabledColumn("colBrandCD1,colBrandName,colSportsCD1,colSportsName,colSegmentCD1,colSegmentName,colYear,colSeason,colDate");
            ModeVisible = false;
            scSportsCD1.CodeWidth = 100;
            scSportsCD.CodeWidth = 100;
        }
        private void RadioCheck()
        {
            if (rdoAllStores.Checked == true)
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
            scBrandCD.TxtCode.Require(true);
            scSportsCD.TxtCode.Require(true);
            scSegmentCD.TxtCode.Require(true);
            txtDate1.Require(true);
            txtRevisionDate.Require(true);
            txtRate1.Require(true);
            txtRate.Require(true);
            txtCopy.Require(true);
        }
        private bool ErrorCheck(int type)
        {
            if (type == 1)
            {
                if (!RequireCheck(new Control[] { scSupplierCD.TxtCode, cbo_Store }))
                    return false;
                DataTable dtVedor = mskbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 4, scSupplierCD.TxtCode.Text);
                if (dtVedor.Rows[0]["DeleteFlg"].ToString() == "1")
                {
                    mskbl.ShowMessage("E119");
                    scSupplierCD.SetFocus(1);
                    return false;
                }
            }
            else if (type == 2)
            {
                if (!RequireCheck(new Control[] { scBrandCD.TxtCode, scSportsCD.TxtCode, scSegmentCD.TxtCode, cbo_Year, cbo_Season, txtChangeDate, txtRate }))
                    return false;
            }

            else if (type == 3)
            {
                if (!RequireCheck(new Control[] { scSupplierCD.TxtCode, txtRevisionDate, txtRate1 }))
                    return false;
            }
            return true;
        }
        protected override void EndSec()
        {
            this.Close();
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
                        scSupplierCD.Clear();
                        txtRevisionDate.Clear();
                        txtRate1.Clear();
                        rdoAllStores.Checked = true;
                        cbo_Store.SelectedValue = "0000";
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

        public void CancelData()
        {
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
            scSupplierCD.SetFocus(1);
        }

        private M_OrderRate_Entity GetSearchInfo()
        {
            moe = new M_OrderRate_Entity()
            {
                VendorCD = scSupplierCD.TxtCode.Text,
                StoreCD = cbo_Store.SelectedValue.ToString(),
                BrandCD = scBrandCD1.TxtCode.Text,
                BrandName=scBrandCD1.LabelText,
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
        private void scSupplierCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                scSupplierCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSupplierCD.TxtCode.Text))
                {
                    if (scSupplierCD.SelectData())
                    {
                        DataTable dtVedor = mskbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 4, scSupplierCD.TxtCode.Text);
                        string deflg = "";
                        if (dtVedor.Rows.Count > 0)
                        {
                            deflg = dtVedor.Rows[0]["DeleteFlg"].ToString();
                        }
                        if (deflg == "1")
                        {
                            bbl.ShowMessage("E119");
                            scSupplierCD.SetFocus(1);
                        }
                        SearchData();
                    }
                    else
                    {
                        mskbl.ShowMessage("E101");
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
                        scBrandCD1.SetFocus(1);
                    }
                    else
                    {
                        mskbl.ShowMessage("E101");
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
                        //scSportsCD1.Value1 = scSportsCD1.TxtCode.Text;
                        //scSportsCD1.Value2 = scSportsCD1.LabelText;
                        //SearchData();
                        scSportsCD1.SetFocus(1);
                    }
                    else
                    {
                        mskbl.ShowMessage("E101");
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
                        //scSegmentCD1.Value1 = scSegmentCD1.TxtCode.Text;
                        //scSegmentCD1.Value2 = scSegmentCD1.LabelText;
                        //SearchData();
                        scSegmentCD1.SetFocus(1);
                    }
                    else
                    {
                        mskbl.ShowMessage("E101");
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
        #region ButtonClick for 【抽出条件】
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchCondition = string.Empty;
            bool op = false;
            if (!string.IsNullOrWhiteSpace(scBrandCD1.TxtCode.Text))
            {
                searchCondition = "BrandCD = '" + scBrandCD1.TxtCode.Text + "'";
                op = true;
            }
            if (!string.IsNullOrWhiteSpace(scSportsCD1.TxtCode.Text))
            {
                if (op)
                {
                    searchCondition += " and ";
                }
                searchCondition += " SportsCD='" + scSportsCD1.TxtCode.Text + "'";
                op = true;
            }
            if (!string.IsNullOrWhiteSpace(scSegmentCD1.TxtCode.Text))
            {
                if (op)
                {
                    searchCondition += " and ";
                }
                op = true;
                searchCondition += "SegmentCD= '" + scSegmentCD1.TxtCode.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(cbo_Year1.Text))
            {
                if (op)
                {
                    searchCondition += " and ";
                }
                op = true;
                searchCondition += "LastYearTerm='" + cbo_Year1.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(cbo_Season1.Text))
            {
                if (op)
                {
                    searchCondition += " and ";
                }
                op = true;
                searchCondition += "LastSeason= '" + cbo_Season1.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(txtDate.Text))
            {
                if (op)
                {
                    searchCondition += " and ";
                }
                op = true;
                searchCondition += "ChangeDate= '" + txtDate.Text;
            }
            if (dgv_ShiireKakeritsu.DataSource != null)
            {
                DataView view = dgv_ShiireKakeritsu.DataSource as DataView;
                dvMain = new DataView(dtMain);
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
                    for (int i = 0; i < dtMain.Rows.Count; i++)
                    {
                        var Brand = dtMain.Rows[i]["BrandCD"].ToString();
                        var Sports = dtMain.Rows[i]["SportsCD"].ToString();
                        var Segment = dtMain.Rows[i]["SegmentCD"].ToString();
                        var LastYearTerm = dtMain.Rows[i]["LastYearTerm"].ToString();
                        var LastSeason = dtMain.Rows[i]["LastSeason"].ToString();
                        if (String.IsNullOrEmpty(Brand) || String.IsNullOrEmpty(Sports) || String.IsNullOrEmpty(Segment) || String.IsNullOrEmpty(LastYearTerm) || String.IsNullOrEmpty(LastSeason))
                        {
                            string date = dtMain.Rows[i][11].ToString();
                            DateTime dtime = Convert.ToDateTime(date);
                            txtRevisionDate.Text = dtime.ToShortDateString();
                            txtRate1.Text = dtMain.Rows[i][12].ToString();
                        }
                    }
                    dgv_ShiireKakeritsu.DataSource = dtMain;
                    dtDel = dtMain;
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
            moe = GetSearchInfo();
            dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
            dvMain = new DataView(dtMain);
            dgv_ShiireKakeritsu.DataSource = dvMain;
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
                        if (chk.Value == chk.TrueValue || check == "True")
                        {
                                DataRow dtRow = dtMain.NewRow();
                                dtRow["VendorCD"] = scSupplierCD.TxtCode.Text;
                                dtRow["StoreCD"] = cbo_Store.SelectedValue.ToString();
                                dtRow["BrandCD"] = row.Cells["colBrandCD1"].Value.ToString();
                                dtRow["BrandName"] = row.Cells["colBrandName"].Value.ToString();
                                dtRow["SportsCD"] = row.Cells["colSportsCD1"].Value.ToString();
                                dtRow["SportsName"] = row.Cells["colSportsName"].Value.ToString();
                                dtRow["SegmentCD"] = row.Cells["colSegmentCD1"].Value.ToString();
                                dtRow["SegmentCDName"] = row.Cells["colSegmentName"].Value.ToString();
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
                if (!string.IsNullOrEmpty(scBrandCD.TxtCode.Text))
                {
                    //mbe.BrandCD = scBrandCD.TxtCode.Text;
                    //mbe.ChangeDate = txtDate1.Text;
                    //DataTable dtbrand = mskbl.M_BrandSelect(mbe);
                    //if (dtbrand.Rows.Count > 0)
                    //{
                    //    scBrandCD.LabelText = dtbrand.Rows[0]["BrandName"].ToString();
                    //}        
                    if (scBrandCD.SelectData())
                    {
                        scBrandCD.SetFocus(1);
                    }
                    else
                    {
                        mskbl.ShowMessage("E101");
                        scBrandCD.SetFocus(1);
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
                        //scSportsCD.Value1 = "202";
                        //scSportsCD.Value2 = scSportsCD.LabelText;
                        scSportsCD.SetFocus(1);
                    }
                    else
                    {
                        mskbl.ShowMessage("E101");
                        scSportsCD.SetFocus(1);
                    }
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
                        //scSegmentCD.Value1 = "203";
                        //scSegmentCD.Value2 = scSegmentCD.LabelText;
                        scSegmentCD.SetFocus(1);
                    }
                    else
                    {
                        mskbl.ShowMessage("E101");
                        scSegmentCD.SetFocus(1);
                    }
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
                dgv_ShiireKakeritsu.DataSource = dtMain;
                if (dgv_ShiireKakeritsu.Rows.Count > 0)
                {
                    string searchCondition = string.Empty;
                    bool op = false;
                    if (!string.IsNullOrWhiteSpace(scBrandCD.TxtCode.Text))
                    {
                        searchCondition = "BrandCD = '" + scBrandCD.TxtCode.Text + "'";
                        op = true;
                    }
                    if (!string.IsNullOrWhiteSpace(scSportsCD.TxtCode.Text))
                    {
                        if (op)
                        {
                            searchCondition += " and ";
                        }
                        searchCondition += " SportsCD='" + scSportsCD.TxtCode.Text + "'";
                        op = true;
                    }
                    if (!string.IsNullOrWhiteSpace(scSegmentCD.TxtCode.Text))
                    {
                        if (op)
                        {
                            searchCondition += " and ";
                        }
                        op = true;
                        searchCondition += "SegmentCD= '" + scSegmentCD.TxtCode.Text + "'";
                    }
                    if (!string.IsNullOrWhiteSpace(cbo_Year.Text))
                    {
                        if (op)
                        {
                            searchCondition += " and ";
                        }
                        op = true;
                        searchCondition += "LastYearTerm='" + cbo_Year.Text + "'";
                    }
                    if (!string.IsNullOrWhiteSpace(cbo_Season.Text))
                    {
                        if (op)
                        {
                            searchCondition += " and ";
                        }
                        op = true;
                        searchCondition += "LastSeason= '" + cbo_Season.Text + "'";
                    }
                    if (!string.IsNullOrWhiteSpace(txtChangeDate.Text))
                    {
                        if (op)
                        {
                            searchCondition += " and ";
                        }
                        op = true;
                        searchCondition += " ChangeDate= '" + txtChangeDate.Text;
                    }
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
                }
            //else
            //{
            //    dgv_ShiireKakeritsu.DataSource = dt;
            //    if (dgv_ShiireKakeritsu.Rows.Count > 0)
            //    {
            //        string searchCondition = string.Empty;
            //        bool op = false;
            //        if (!string.IsNullOrWhiteSpace(scBrandCD.TxtCode.Text))
            //        {
            //            searchCondition = "BrandCD = '" + scBrandCD.TxtCode.Text + "'";
            //            op = true;
            //        }
            //        if (!string.IsNullOrWhiteSpace(scSportsCD.TxtCode.Text))
            //        {
            //            if (op)
            //            {
            //                searchCondition += " and ";
            //            }
            //            searchCondition += " SportsCD='" + scSportsCD.TxtCode.Text + "'";
            //            op = true;
            //        }
            //        if (!string.IsNullOrWhiteSpace(scSegmentCD.TxtCode.Text))
            //        {
            //            if (op)
            //            {
            //                searchCondition += " and ";
            //            }
            //            op = true;
            //            searchCondition += "SegmentCD= '" + scSegmentCD.TxtCode.Text + "'";
            //        }
            //        if (!string.IsNullOrWhiteSpace(cbo_Year.Text))
            //        {
            //            if (op)
            //            {
            //                searchCondition += " and ";
            //            }
            //            op = true;
            //            searchCondition += "LastYearTerm='" + cbo_Year.Text + "'";
            //        }
            //        if (!string.IsNullOrWhiteSpace(cbo_Season.Text))
            //        {
            //            if (op)
            //            {
            //                searchCondition += " and ";
            //            }
            //            op = true;
            //            searchCondition += "LastSeason= '" + cbo_Season.Text + "'";
            //        }
            //        if (!string.IsNullOrWhiteSpace(txtChangeDate.Text))
            //        {
            //            if (op)
            //            {
            //                searchCondition += " and ";
            //            }
            //            op = true;
            //            searchCondition += " ChangeDate= '" + txtChangeDate.Text;
            //        }
            //        if (!string.IsNullOrWhiteSpace(searchCondition))
            //        {
            //            DataRow[] dr = dt.Select(searchCondition);
            //            if (dr.Count() > 0)
            //            {
            //                for (int i = 0; i < dr.Length; i++)
            //                {
            //                    dr[i]["Column1"] = "1";
            //                }
            //            }
            //        }
            //        else
            //        {
            //            dtGrid = dt;
            //        }
            //        dgv_ShiireKakeritsu.DataSource = dt;
            //    }
            //}
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
            dgv_ShiireKakeritsu.RefreshEdit();
        }
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            CheckState(true);
        }
        private void btnReleaseAll_Click(object sender, EventArgs e)
        {
            CheckState(false);
        }
        private void CheckState(bool flag)
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
                    dt.Columns.Add("VendorCD");//ses
                    dt.Columns.Add("StoreCD");//ses
                    dt.Columns.Add("BrandCD");
                    dt.Columns.Add("BrandName");
                    dt.Columns.Add("SportsCD");
                    dt.Columns.Add("SportsName");
                    dt.Columns.Add("SegmentCD");
                    dt.Columns.Add("SegmentCDName");
                    dt.Columns.Add("LastYearTerm");
                    dt.Columns.Add("LastSeason");
                    dt.Columns.Add("ChangeDate");
                    dt.Columns.Add("Rate");

                    DataRow dtRow = dt.NewRow();
                    dtRow["VendorCD"] = scSupplierCD.TxtCode.Text;
                    dtRow["StoreCD"] = cbo_Store.SelectedValue.ToString();
                    dtRow["BrandCD"] = scBrandCD.TxtCode.Text;
                    dtRow["BrandName"] = scBrandCD.LabelText;
                    dtRow["SportsCD"] = scSportsCD.TxtCode.Text;
                    dtRow["SportsName"] = scSportsCD.LabelText;
                    dtRow["SegmentCD"] = scSegmentCD.TxtCode.Text;
                    dtRow["SegmentCDName"] =scSegmentCD.LabelText;
                    dtRow["LastYearTerm"] = cbo_Year.Text;
                    dtRow["LastSeason"] = cbo_Season.Text;
                    dtRow["ChangeDate"] = txtChangeDate.Text;
                    dtRow["Rate"] = Convert.ToDecimal(txtRate.Text);
                    dt.Rows.Add(dtRow);
                    CancelData();
                    scBrandCD.SetFocus(1);
                    dgv_ShiireKakeritsu.DataSource = dt;
                    dtMain = dt;
                }
                else
                {
                        DataRow row = dtMain.NewRow();
                        row["VendorCD"] = scSupplierCD.TxtCode.Text;
                        row["StoreCD"] = cbo_Store.SelectedValue.ToString();
                        row["BrandCD"] = scBrandCD.TxtCode.Text;
                        row["BrandName"] = scBrandCD.LabelText;
                        row["SportsCD"] = scSportsCD.TxtCode.Text;
                        row["SportsName"] = scSportsCD.LabelText;
                        row["SegmentCD"] = scSegmentCD.TxtCode.Text;
                        row["SegmentCDName"] = scSegmentCD.LabelText;
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
            if (string.IsNullOrWhiteSpace(txtRate.Text))
            {
                mskbl.ShowMessage("E102");
                txtRate.Focus();
            }
            else
            {
                foreach (DataGridViewRow row in dgv_ShiireKakeritsu.Rows)
                {
                    DataGridViewCheckBoxCell check = row.Cells[0] as DataGridViewCheckBoxCell;
                    if (row.Cells["colChk"].Value != null)
                    {
                        string chk = row.Cells["colChk"].Value.ToString();
                        if (check.Value == check.TrueValue || chk == "True")
                        {
                            row.Cells["colRate1"].Value = txtRate.Text;
                        }
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
            dgv_ShiireKakeritsu.DataSource = dtMain;
            DataView view = dgv_ShiireKakeritsu.DataSource as DataView;
            dvMain = new DataView(dtMain);
            dgv_ShiireKakeritsu.DataSource = dvMain;
            //dgv_ShiireKakeritsu.DataSource = dtMain;
            //DataView view = dgv_ShiireKakeritsu.DataSource as DataView;
            //dvMain = new DataView(dtMain);
            //DataView view = dgv_ShiireKakeritsu.DataSource as DataView;
            //dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
            //dtMain = view.Table;
            //dgv_ShiireKakeritsu.DataSource = dvMain;
        }
        #endregion

        private void F11()
        {
            moe = GetSearchInfo();
            RadioCheck();
            dtMain = mskbl.M_ShiireKakeritsu_Select(moe);
            if (dtMain.Rows.Count > 0)
            {
                for (int i = 0; i < dtMain.Rows.Count; i++)
                {
                    var Brand = dtMain.Rows[i]["BrandCD"].ToString();
                    var Sports = dtMain.Rows[i]["SportsCD"].ToString();
                    var Segment = dtMain.Rows[i]["SegmentCD"].ToString();
                    var LastYearTerm = dtMain.Rows[i]["LastYearTerm"].ToString();
                    var LastSeason = dtMain.Rows[i]["LastSeason"].ToString();
                    if (String.IsNullOrEmpty(Brand) || String.IsNullOrEmpty(Sports) || String.IsNullOrEmpty(Segment) || String.IsNullOrEmpty(LastYearTerm) || String.IsNullOrEmpty(LastSeason))
                    {
                        string date= dtMain.Rows[i][11].ToString();
                        DateTime dtime = Convert.ToDateTime(date);
                        txtRevisionDate.Text = dtime.ToShortDateString();
                        txtRate1.Text = dtMain.Rows[i][12].ToString();
                    }
                }
                dgv_ShiireKakeritsu.DataSource = dtMain;
                dtDel = dtMain;
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
            //if (dtExcel != null || dtMain !=null)
            //{
            //    dgv_ShiireKakeritsu.DataSource = dtExcel;
            //    //dtMain = dtAdd;
            //    Xml = mskbl.DataTableToXml(dtExcel);
            //    log_data = Get_Log_Data();
            //    moe.VendorCD = scSupplierCD.TxtCode.Text;
            //    moe.StoreCD = cbo_Store.SelectedValue.ToString();
            //    moe.ChangeDate = txtRevisionDate.Text;
            //    moe.Rate = txtRate1.Text;
            //}
            //else
            //{
                dtMain.AcceptChanges();
                for (int i = 0; i < dtMain.Rows.Count; i++)
                {
                    var Brand = dtMain.Rows[i]["BrandCD"].ToString();
                    var Sports = dtMain.Rows[i]["SportsCD"].ToString();
                    var Segment = dtMain.Rows[i]["SegmentCD"].ToString();
                    var LastYearTerm = dtMain.Rows[i]["LastYearTerm"].ToString();
                    var LastSeason = dtMain.Rows[i]["LastSeason"].ToString();
                    if (String.IsNullOrEmpty(Brand) || String.IsNullOrEmpty(Sports) || String.IsNullOrEmpty(Segment) || String.IsNullOrEmpty(LastYearTerm) || String.IsNullOrEmpty(LastSeason))
                    {
                        dtMain.Rows[i].Delete();
                    }
                }
                dgv_ShiireKakeritsu.DataSource = dtMain;
                dtDel = dtMain;
                string delData = mskbl.DataTableToXml(dtDel);
                string insertData = mskbl.DataTableToXml(dtMain);
                //log_data = Get_Log_Data();
                moe.VendorCD = scSupplierCD.TxtCode.Text;
                moe.StoreCD = cbo_Store.SelectedValue.ToString();
                moe.ChangeDate = txtRevisionDate.Text;
                moe.Rate = txtRate1.Text;
                if( mskbl.M_OrderRate_Update(moe,delData,insertData))
                {
                   scSupplierCD.Clear();
                   Clear(panelDetail);
                   dgv_ShiireKakeritsu.DataSource = string.Empty;
                   mskbl.ShowMessage("I101");
                   rdoAllStores.Checked = true;
                   cbo_Store.SelectedValue = "0000";
                   scSupplierCD.SetFocus(1);
                }
        }
        protected DataTable ChangeColumnName(DataTable dtMain)
        {
            dtMain.Columns["仕入先CD"].ColumnName = "VendorCD";
            dtMain.Columns["店舗CD"].ColumnName = "StoreCD";
            dtMain.Columns["ブランドCD"].ColumnName = "BrandCD";
            dtMain.Columns["競　技CD"].ColumnName = "SportsCD";
            dtMain.Columns["商品分類CD"].ColumnName = "SegmentCD";
            dtMain.Columns["年度"].ColumnName = "LastYearTerm";
            dtMain.Columns["シーズン"].ColumnName = "LastSeason";
            dtMain.Columns["改定日"].ColumnName = "ChangeDate";
            dtMain.Columns["掛率"].ColumnName = "Rate";
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
        private bool ErrorCheckForExcel()
        {

            if (String.IsNullOrWhiteSpace(scSupplierCD.TxtCode.Text))
            {
                mskbl.ShowMessage("E102");
                scSupplierCD.Focus();
                return false;
            }
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
                    return false;
                }
                else
                {
                    dtExcel = ExcelToDatatable(str);
                    string[] colname = { "仕入先CD", "店舗CD", "改定日", "掛率" };
                    if (ColumnCheck(colname, dtExcel))
                    {
                        for (int i = 0; i < dtExcel.Rows.Count; i++)
                        {
                            string vall = dtExcel.Rows[i][1].ToString();
                            String rowse = "0";
                            if (dtExcel.Rows[i][0].ToString() != scSupplierCD.TxtCode.Text)
                            {
                                mskbl.ShowMessage("E230");
                                rowse = "1";
                                dtExcel.Rows[i].Delete();
                                continue;
                            }
                            if (!String.IsNullOrEmpty(dtExcel.Rows[i][1].ToString()) && dtExcel.Rows[i][1].ToString() != "0000")
                            {
                                DataTable dtResult = mskbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 3, dtExcel.Rows[i][1].ToString());
                                if (dtResult.Rows.Count == 0)
                                {
                                    mskbl.ShowMessage("E138");
                                    rowse = "1";
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                                if (!base.CheckAvailableStores(dtExcel.Rows[i][1].ToString()))
                                {
                                    bbl.ShowMessage("E141");
                                    rowse = "1";
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                            }
                            if (!String.IsNullOrEmpty(dtExcel.Rows[i][2].ToString()))
                            {
                                DataTable dtResult = bbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 11, dtExcel.Rows[i][2].ToString());
                                if (dtResult.Rows.Count == 0)
                                {
                                    mskbl.ShowMessage("E138");
                                    rowse = "1";
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                                //if (mskbl.SimpleSelect1("64", string.Empty, "202", dtExcel.Columns["ブランドCD"].ToString()).Rows.Count < 0)
                                //{
                                //    mskbl.ShowMessage("E138");
                                //    rowse = "1";
                                //    //toDelete.Add(row);
                                //}
                            }
                            if (String.IsNullOrEmpty(dtExcel.Rows[i][2].ToString()) && !String.IsNullOrEmpty(dtExcel.Rows[i][3].ToString()))
                            {
                                if (mskbl.SimpleSelect1("64", string.Empty, "202", dtExcel.Rows[i][3].ToString()).Rows.Count < 0)
                                {
                                    mskbl.ShowMessage("E138");
                                    rowse = "1";
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                            }
                            if (String.IsNullOrEmpty(dtExcel.Rows[i][3].ToString()) && !string.IsNullOrEmpty(dtExcel.Rows[i][4].ToString()))
                            {
                                mskbl.ShowMessage("E229");
                                rowse = "1";
                                dtExcel.Rows[i].Delete();
                                continue;
                            }
                            if (string.IsNullOrEmpty(dtExcel.Rows[i][4].ToString()))
                            {
                                if (mskbl.SimpleSelect1("64", string.Empty, "203", dtExcel.Rows[i][4].ToString()).Rows.Count < 0)
                                {
                                    mskbl.ShowMessage("E138");
                                    rowse = "1";
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                            }
                            if (String.IsNullOrEmpty(dtExcel.Rows[i][4].ToString()) && !string.IsNullOrEmpty(dtExcel.Rows[i][5].ToString()))
                            {

                                mskbl.ShowMessage("E229");
                                rowse = "1";
                                dtExcel.Rows[i].Delete();
                                continue;
                            }

                            if (String.IsNullOrEmpty(dtExcel.Rows[i][5].ToString()))
                            {
                                if (mskbl.SimpleSelect1("64", string.Empty, "203", dtExcel.Rows[i][5].ToString()).Rows.Count < 0)
                                {
                                    mskbl.ShowMessage("E138");
                                    rowse = "1";
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                            }
                            if (String.IsNullOrEmpty(dtExcel.Rows[i][5].ToString()) && !string.IsNullOrEmpty(dtExcel.Rows[i][6].ToString()))
                            {
                                mskbl.ShowMessage("E229");
                                rowse = "1";
                                dtExcel.Rows[i].Delete();
                                continue;
                            }
                            if (String.IsNullOrEmpty(dtExcel.Rows[i][6].ToString()))
                            {
                                if (mskbl.SimpleSelect1("64", string.Empty, "308", dtExcel.Rows[i][6].ToString()).Rows.Count < 0)
                                {
                                    mskbl.ShowMessage("E138");
                                    rowse = "1";
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                            }
                            if (String.IsNullOrEmpty(dtExcel.Rows[i][7].ToString()))
                            {
                                mskbl.ShowMessage("E103");
                                rowse = "1";
                                dtExcel.Rows[i].Delete();
                                continue;
                            }
                            if (!String.IsNullOrWhiteSpace(dtExcel.Rows[i][7].ToString()))
                            {
                                string dates = dtExcel.Rows[i][7].ToString();
                                string[] format = { "MM/dd/yyyy" };
                                DateTime res;
                                if (DateTime.TryParse(dtExcel.Rows[i][7].ToString(), out res))
                                {
                                    dates = res.ToString("MM/dd/yyyy");
                                }
                                else
                                {
                                    mskbl.ShowMessage("E103");
                                    rowse = "1";
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                            }
                            //if (rowse == "1")
                            //{
                            //    dtExcel.Rows[i].Delete();
                            //}
                        }
                    }
                    else
                    {
                        mskbl.ShowMessage("E137");
                    }
                }
            }
            return true;
        }
        protected Boolean ColumnCheck(String[] colName, DataTable dtMain)
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
        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (ErrorCheckForExcel())
            {
                //dtExcel.AcceptChanges();
                //for (int i = 0; i < dtExcel.Rows.Count; i++)
                //{
                //    dtMain = ChangeColumnName(dtExcel);
                //    //var brand = dtExcel.Rows[i][3].ToString();
                //    //dt = mskbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 11,brand);
                //    //var sports = dtExcel.Rows[i][5].ToString();
                //    //dt = mskbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 12, sports);
                //    //var segment = dtExcel.Rows[i][7].ToString();
                //    //dt = mskbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 13, segment);
                //    //dtExcel.Rows.Add(dt);
                //}
                dgv_ShiireKakeritsu.DataSource = dtExcel;
                if(dtExcel!=null)
                {
                   dtExcel.Columns["仕入先CD"].ColumnName = "VendorCD";
                   dtExcel.Columns["店舗CD"].ColumnName = "StoreCD";
                   dtExcel.Columns["ブランドCD"].ColumnName = "BrandCD";
                   dtExcel.Columns.Add("BrandName");
                   dtExcel.Columns["競　技CD"].ColumnName = "SportsCD";
                   dtExcel.Columns.Add("SportsName");
                   dtExcel.Columns["商品分類CD"].ColumnName = "SegmentCD";
                   dtExcel.Columns.Add("SegmentCDName");
                   dtExcel.Columns["年度"].ColumnName = "LastYearTerm";
                   dtExcel.Columns["シーズン"].ColumnName = "LastSeason";
                   dtExcel.Columns["改定日"].ColumnName = "ChangeDate";
                   dtExcel.Columns["掛率"].ColumnName = "Rate";
                   dtMain = dtExcel;
                }
                if (dtMain.Rows.Count > 0)
                {
                    for(int i=0;i < dtMain.Rows.Count;i++)
                    {
                        dtMain.AcceptChanges();
                        DataTable dt = mskbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 11, dtMain.Rows[i]["BrandCD"].ToString());
                        dtMain.Rows[i]["BrandName"] = dt.Rows[0]["Name"].ToString();
                        DataTable dt1 = mskbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 12, dtMain.Rows[i]["SportsCD"].ToString(),"202");
                        dtMain.Rows[i]["SportsName"] = dt1.Rows[0]["Name"].ToString();
                        DataTable dt2 = mskbl.Select_SearchName(txtDate1.Text.Replace("/", "-"), 13, dtMain.Rows[i]["SegmentCD"].ToString(),"203");
                        dtMain.Rows[i]["SegmentCDName"] = dt2.Rows[0]["Name"].ToString();
                    }
                    dgv_ShiireKakeritsu.DataSource = dtMain;
                }
            }
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

        #endregion

        private void MasterTouroku_ShiireKakeritsu_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}