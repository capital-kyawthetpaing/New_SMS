using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using BL;
using Entity;
using Base.Client;
using Search;

namespace KeihiNyuuryoku
{
    public partial class frmKeihiNyuuryoku : FrmMainForm
    {
        D_Cost_Entity cost;
        M_Staff_Entity staff;
        KeihiNyuuryoku_BL khnyk_BL;
        int type = 0;//1 = normal, 2 = copy (for f11)
        string keijoudate, staffName = string.Empty;
        decimal TotalGaku;
        DataTable dtcost, dtcontrol, dtpayplan, dtVendor, dtStaff, dt;
        private int i;

        public frmKeihiNyuuryoku()
        {
            InitializeComponent();
            Load += new System.EventHandler(FormLoadEvent);
            PanelNormal.Enter += PanelNormal_Enter;
            PanelCopy.Enter += PanelCopy_Enter;
           // dgvKehiNyuuryoku.DataError += dgvKehiNyuuryoku_DataError;
            khnyk_BL = new KeihiNyuuryoku_BL();
        }

        private void PanelNormal_Enter(object sender, EventArgs e)
        {
            type = 1;
        }

        private void PanelCopy_Enter(object sender, EventArgs e)
        {
            type = 2;
        }

        private void FormLoadEvent(object sender, EventArgs e)
        {
            InProgramID = "KeihiNyuuryoku";
            SetFunctionLabel(EProMode.KehiNyuuryoku);
            //SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            SetRequireField();
            
            Btn_F7.Text = "行削除(F7)";
            Btn_F8.Text = "行追加(F8)";
            Btn_F9.Text = "検索(F9)";
            Btn_F10.Text = "行複写(F10)";
            Btn_F11.Text = "印刷(F11)";
            Btn_F11.Text = string.Empty;
           
            lblTotalGaku.AutoSize = false;
            lblTotalGaku.Width = 90;
            lblTotalGaku.Height = 16;
            lblTotalGaku.TextAlign = ContentAlignment.MiddleRight;

            CreateDataTable();
            ScStaff.TxtCode.Text = InOperatorCD;
            ScStaff.LabelText = Bind_StaffName(ScStaff.Code);
            txtKeijouDate.Text = System.DateTime.Now.ToString("yyyy/MM/dd");
            ScVendor.SetFocus(1);
            dgvKehiNyuuryoku.CheckCol.Add("colCostCD");
            dgvKehiNyuuryoku.CheckCol.Add("colDepartment");
        }

        private void CreateDataTable()
        {
            dt = new DataTable();
            dt.Columns.Add("CostCD", typeof(string));
            dt.Columns.Add("Summary", typeof(string));
            dt.Columns.Add("DepartmentCD", typeof(string));
            dt.Columns.Add("CostGaku", typeof(int));
            //dt.Columns.Add("index", typeof(string)); //2020-06-16 ptk

            DataTable dtDepartment = new DataTable();
            dtDepartment = khnyk_BL.SimpleSelect1("38", null, "209");
            DataRow dr = dtDepartment.NewRow();
            dr["Key"] = "0";
            dr["Char1"] = string.Empty;
            dtDepartment.Rows.InsertAt(dr, 0);
            if (dtDepartment.Rows.Count > 0)
            {
                DataGridViewComboBoxColumn cbocolD = (DataGridViewComboBoxColumn)dgvKehiNyuuryoku.Columns["colDepartment"];
                cbocolD.DataPropertyName = "DepartmentCD";
                cbocolD.DisplayMember = "Char1";
                cbocolD.ValueMember = "Key";
                cbocolD.DataSource = dtDepartment;
            }

            DataTable dtCostCD = new DataTable();
            dtCostCD = khnyk_BL.SimpleSelect1("37", null, "208");
            DataRow row = dtCostCD.NewRow();
            row["Key"] = "0";
            row["Char1"] = string.Empty;
            dtCostCD.Rows.InsertAt(row, 0);
            if (dtCostCD.Rows.Count > 0)
            {
                DataGridViewComboBoxColumn cbocolV = (DataGridViewComboBoxColumn)dgvKehiNyuuryoku.Columns["colCostCD"];
                cbocolV.DataPropertyName = "CostCD";
                cbocolV.DisplayMember = "Char1";
                cbocolV.ValueMember = "Key";
                cbocolV.DataSource = dtCostCD;
            }

            for (int i = 0; i < 300; i++)
            {
                dt.Rows.Add();
            }
            //int g = 0; foreach (DataRow dr in dt.Rows) { g++; dr["index"] = g.ToString(); } //2020-06-16 ptk
            dgvKehiNyuuryoku.DataSource = dt; 
            BindTotalGaku(dt);
        }

        private void SetRequireField()
        {
            ScCost.TxtCode.Require(true);
            ScVendor.TxtCode.Require(true);
            txtKeijouDate.Require(true);
            ScStaff.TxtCode.Require(true);
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
                        ScVendor.SetFocus(1);
                    }
                    break;
                case 7:
                    F7();
                    break;
                case 8:
                    F8();
                    break;
                case 10:
                    F10();
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    F12();
                    break;
            }
        }

        private void F11()
        {
            if (ErrorCheck(11))
            {
                switch (OperationMode)
                {
                    case EOperationMode.INSERT:
                         if (DisplayData(ScCost_Copy))
                         {
                            
                         }
                        break;
                    case EOperationMode.UPDATE:
                        if (DisplayData(ScCost))
                        {
                            DisablePanel(PanelHeader);
                            EnablePanel(panelDetail);
                            F11Enable = false;
                            F12Enable = true;
                            SelectNextControl(panelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData(ScCost))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(panelDetail);
                            SelectNextControl(panelDetail, true, true, true, true);
                            F11Enable = false;
                            F12Enable = true;
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData(ScCost))
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(panelDetail);
                            F12Enable = false;
                        }
                        break;
                }
                //***Add Control Enable/Disable;
            }
        }

        private bool DisplayData(CKM_SearchControl sc) //Display data for ScCost and Copy_ScCost
        {
            if (type == 1)
            {
                cost = new D_Cost_Entity
                {
                    CostNO = sc.Code
                };
                
                dtcost = khnyk_BL.D_Cost_Display(cost);
                if (dtcost.Rows.Count > 0)
                {
                    ScVendor.Code = dtcost.Rows[0]["VendorCD"].ToString();
                    ScVendor.LabelText = dtcost.Rows[0]["VendorName"].ToString();
                    txtKeijouDate.Text = dtcost.Rows[0]["RecordedDate"].ToString();
                    txtShihraiYoteiDate.Text = dtcost.Rows[0]["PayPlanDate"].ToString();
                    ScStaff.Code = dtcost.Rows[0]["StaffCD"].ToString();
                    ScStaff.LabelText = dtcost.Rows[0]["StaffName"].ToString();
                    dtcost.Columns.Remove("No");
                    dtcost.Columns.Remove("VendorCD");
                    dtcost.Columns.Remove("VendorName");
                    dtcost.Columns.Remove("RecordedDate");
                    dtcost.Columns.Remove("PayPlanDate");
                    dtcost.Columns.Remove("StaffCD");
                    dtcost.Columns.Remove("StaffName");
                    dtcost.Columns.Remove("Department");
                    dtcost.Columns.Remove("TotalGaku");

                    for (int i= dtcost.Rows.Count; i<300; i++)
                    {
                        dtcost.Rows.Add();
                    }
                    
                    dt = dtcost;
                    dgvKehiNyuuryoku.DataSource = dt;
                    BindTotalGaku(dt);

                    return true;
                }
                else
                    return false;

            }
            else
            {
                cost = new D_Cost_Entity()
                {
                    VendorCD = ScVendor.Code,
                    RecordedDate = txtKeijouDate.Text,
                    CostNO = sc.Code
                };
                
                dtcost = khnyk_BL.D_Cost_Copy_Display(cost);
                if (dtcost.Rows.Count > 0)
                {
                    ScVendor.Code = dtcost.Rows[0]["VendorCD"].ToString();
                    ScVendor.LabelText = dtcost.Rows[0]["VendorName"].ToString();
                    txtKeijouDate.Text = dtcost.Rows[0]["RecordedDate"].ToString();
                    txtShihraiYoteiDate.Text = dtcost.Rows[0]["PayPlanDate"].ToString();
                    ScStaff.Code = dtcost.Rows[0]["StaffCD"].ToString();
                    ScStaff.LabelText = dtcost.Rows[0]["StaffName"].ToString();
                    dtcost.Columns.Remove("No");
                    dtcost.Columns.Remove("VendorCD");
                    dtcost.Columns.Remove("VendorName");
                    dtcost.Columns.Remove("RecordedDate");
                    dtcost.Columns.Remove("PayPlanDate");
                    dtcost.Columns.Remove("StaffCD");
                    dtcost.Columns.Remove("StaffName");
                    dtcost.Columns.Remove("Department");
                    dtcost.Columns.Remove("TotalGaku");

                    for (int i = dtcost.Rows.Count; i < 300; i++)
                    {
                        dtcost.Rows.Add();
                    }

                    dt = dtcost;
                    dgvKehiNyuuryoku.DataSource = dt;
                    BindTotalGaku(dt);

                    return true;
                }
                else
                    return false;
            }
        }

        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (khnyk_BL.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    //*** Create Entity Object
                    cost = GetD_Cost_data();
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
            }
            //else PreviousCtrl.Focus();
        } // Insert/Update/Delete Data

        private D_Cost_Entity GetD_Cost_data()
        {
            cost = new D_Cost_Entity();
            if (type == 2)
                cost.CostNO = ScCost_Copy.Code;
            else cost.CostNO = ScCost.Code;
            cost.PayeeCD = ScVendor.Code;
            cost.RecordedDate = txtKeijouDate.Text;
            cost.PayPlanDate = txtShihraiYoteiDate.Text;
            cost.StaffCD = ScStaff.Code;
            cost.TotalGaku = lblTotalGaku.Text;
            cost.Operator = InOperatorCD;
            cost.Store = StoreCD;
            if (chkRegularFlg.Checked == true)
                cost.RegularlyFLG = "1";
            else cost.RegularlyFLG = "0";
            cost.xml1 = khnyk_BL.DataTableToXml(dt);
            cost.ProcessMode = ModeText;
            cost.Operator = InOperatorCD;
            cost.ProgramID = InProgramID;
            cost.Key = ScCost_Copy.Code;
            cost.PC = InPcID;

            return cost;
        }
        
        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                //HeaderCheck on F11
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 2)
                    {
                        //if (!RequireCheck(new Control[] { ScVendor.TxtCode, txtKeijouDate, ScStaff.TxtCode }))
                        // return false;

                        if (!string.IsNullOrWhiteSpace(ScCost_Copy.Code))
                        {
                            dtcost = khnyk_BL.SimpleSelect1("10", null, ScCost_Copy.Code);
                            if (dtcost.Rows.Count < 1)
                            {
                                khnyk_BL.ShowMessage("E138");
                                return false;
                            }
                            if (!string.IsNullOrWhiteSpace(dtcost.Rows[0]["DeleteDateTime"].ToString()))
                            {
                                khnyk_BL.ShowMessage("E140");
                                ScCost.SetFocus(1);
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if (!RequireCheck(new Control[] { ScCost.TxtCode }))
                        return false;

                    dtcost = khnyk_BL.SimpleSelect1("10", null, ScCost.Code);
                    dtcontrol = khnyk_BL.M_Control_RecordCheck(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    dtpayplan = khnyk_BL.SimpleSelect1("19",null,ScCost.Code);

                    if (dtcost.Rows.Count < 1)
                    {
                        khnyk_BL.ShowMessage("E138");
                        return false;
                    }
                    if (!string.IsNullOrWhiteSpace(dtcost.Rows[0]["DeleteDateTime"].ToString()))
                    {
                        khnyk_BL.ShowMessage("E140");
                        ScCost.SetFocus(1);
                        return false;
                    }
                    if (dtcontrol.Rows.Count < 1)
                    {
                        khnyk_BL.ShowMessage("E115");
                        ScCost.SetFocus(1);
                        return false;
                    }
                    if (dtpayplan.Rows.Count > 0)
                    {
                        khnyk_BL.ShowMessage("S014");
                        ScCost.SetFocus(1);
                        return false;
                    }
                }
            }
            //DetailCheck on F12
            else if (index == 12)
            {
                 if (!RequireCheck(new Control[] { ScVendor.TxtCode, txtKeijouDate, ScStaff.TxtCode }))
                     return false;

                 if (string.IsNullOrWhiteSpace(txtKeijouDate.Text))
                     keijoudate = System.DateTime.Now.ToString("yyyy-MM-dd");

                 else
                     keijoudate = txtKeijouDate.Text;
                 dtVendor = khnyk_BL.Select_SearchName(keijoudate,4,ScVendor.Code);

                 if (dtVendor.Rows.Count < 1)
                 {
                     khnyk_BL.ShowMessage("E101");
                     ScVendor.SetFocus(1);
                     return false;
                 }
                 dtcontrol = khnyk_BL.M_Control_RecordCheck(txtKeijouDate.Text.ToString());
                 if (dtcontrol.Rows.Count < 1)
                 {
                     khnyk_BL.ShowMessage("E115");
                     txtKeijouDate.Focus();
                     return false;
                 }
                 
                 staff = new M_Staff_Entity();
                 staff.StaffCD = ScStaff.Code;
                 staff.ChangeDate = keijoudate;
                 dtStaff = khnyk_BL.Select_SearchName(keijoudate,5,ScStaff.Code);
                 if (dtStaff.Rows.Count < 1)
                 {
                     khnyk_BL.ShowMessage("E101");
                     ScStaff.SetFocus(1);
                     return false;
                 }

                DataTable dta = new DataTable();
                dta = dt.Copy();
                DataRow[] drs = dta.Select("(CostCD IS  NULL ) " +
                                             "AND (Summary IS  NULL) " +
                                             "AND (DepartmentCD IS NULL) " +
                                             "AND (CostGaku IS  NULL)");
                if(drs.Count() != dgvKehiNyuuryoku.Rows.Count )
                {
                    foreach(DataRow r in drs)
                    {
                        dta.Rows.Remove(r);
                    }
                    foreach (DataRow dr in dta.Rows)
                    {
                        
                        if (string.IsNullOrWhiteSpace(dr["CostCD"].ToString()))
                        {
                            khnyk_BL.ShowMessage("E102");
                            //dgvKehiNyuuryoku.ClearSelection(); //2020-06-16 ptk
                            //dgvKehiNyuuryoku.Refresh(); //2020-06-16 ptk
                            //dgvKehiNyuuryoku.Rows[ Convert.ToInt32 (dr["index"].ToString())-1].Selected =true; //2020-06-16 ptk
                            // dgvKehiNyuuryoku.CurrentCell.Selected = true;
                            // dgvKehiNyuuryoku.CurrentCell = dgvKehiNyuuryoku[dgvKehiNyuuryoku.Columns["colCostCD"].Index, Convert.ToInt16(dr)];
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(dr["DepartmentCD"].ToString())) // Check ComboBox is selected or not
                        {
                            khnyk_BL.ShowMessage("E102");
                            dgvKehiNyuuryoku.Select();
                            //dgvKehiNyuuryoku.CurrentCell = dgvKehiNyuuryoku[dgvKehiNyuuryoku.Columns["colDepartment"].Index, Convert.ToInt16(drs[0]["colDepartment"].ToString()) - 1];
                            return false;
                        }
                    }
                }
                else
                {
                    khnyk_BL.ShowMessage("E189");
                    dgvKehiNyuuryoku.Select();
                    //dgvKehiNyuuryoku.CurrentCell = dgvKehiNyuuryoku[dgvKehiNyuuryoku.Columns["colCostCD"].Index, Convert.ToInt16(drs[0]["colCostCD"].ToString()) - 1];
                    return false;
                }
                
            }
            return true;
        }

        
        private void InsertUpdate(int mode)
        {
            //*** Insert Update Function
            if (khnyk_BL.KehiNyuuryoku_Insert_Update(cost, mode))
            {
                Clear(PanelHeader);
                Clear(panelDetail);

                ChangeMode(OperationMode);
                ScCost.SetFocus(1);
                
                khnyk_BL.ShowMessage("I101");
            }
            else
            {
                khnyk_BL.ShowMessage("S001");
            }
        }

       
        private void Delete()
        {
            //*** Delete Function
            if (khnyk_BL.KehiNyuuryoku_Delete(cost))
            {
                Clear(PanelHeader);
                Clear(panelDetail);

                ChangeMode(OperationMode);
                ScCost.SetFocus(1);

                khnyk_BL.ShowMessage("I102");
            }
            else
            {
                khnyk_BL.ShowMessage("S001");
            }
        }
        
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    Clear(PanelHeader);
                    Clear(panelDetail);
                    chkRegularFlg.Checked = true;
                    EnablePanel(PanelCopy);
                    EnablePanel(panelDetail);
                    DisablePanel(PanelNormal);
                    ScCost.SearchEnable = false;
                    ScCost_Copy.SearchEnable = true;
                    CreateDataTable();
                    F9Visible = false;
                    F12Enable = true;
                    F11Enable = true;
                    ScStaff.TxtCode.Text = InOperatorCD;
                    ScStaff.LabelText = Bind_StaffName(ScStaff.Code);
                    txtKeijouDate.Text = System.DateTime.Now.ToString("yyyy/MM/dd");
                    ScVendor.SetFocus(1);
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(panelDetail);
                    chkRegularFlg.Checked = true;
                    CreateDataTable();
                    EnablePanel(PanelNormal);
                    DisablePanel(PanelCopy);
                    DisablePanel(panelDetail);
                    ScCost.SearchEnable = true;
                    ScCost_Copy.SearchEnable = false;
                    F9Visible = true;
                    F12Enable = false;
                    F11Enable = true;
                    ScCost.SetFocus(1);
                    break;
            }
            // ScVendor.SetFocus(1);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void dgvKehiNyuuryoku_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvKehiNyuuryoku.Columns["colCostCD"].Index)
            {
                if (string.IsNullOrWhiteSpace(dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostCD"].Value.ToString()))
                {
                    khnyk_BL.ShowMessage("E102");
                    //dgvKehiNyuuryoku.CurrentCell.Selected = true;
                    //dgvKehiNyuuryoku.NotifyCurrentCellDirty(true);
                    //dgvKehiNyuuryoku.BeginEdit(true);
                    dgvKehiNyuuryoku.CurrentCell = dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostCD"];
                }
            }
            else if (e.ColumnIndex == dgvKehiNyuuryoku.Columns["colDepartment"].Index)
            {
                if (string.IsNullOrWhiteSpace(dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colDepartment"].Value.ToString()))
                {
                    khnyk_BL.ShowMessage("E102");
                    dgvKehiNyuuryoku.CurrentCell = dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colDepartment"];
                }
            }
            else if (e.ColumnIndex == dgvKehiNyuuryoku.Columns["colCostGaku"].Index)
            {
                if (dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostGaku"].Value.ToString().Contains("-"))
                    dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostGaku"].Style.ForeColor = Color.Red;
                else
                    dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostGaku"].Style.ForeColor = Color.Black;

                BindTotalGaku(dt);
            }

        }

        private void frmKeihiNyuuryoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        
        private void ScCost_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 1;
                F11();
            }
        }
        private void ScCost_Copy_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 2;
                F11();
            }
        }

        private void txtKeijouDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtKeijouDate.Text) && !(string.IsNullOrWhiteSpace(ScVendor.Code)))
                {
                    txtShihraiYoteiDate.Text = khnyk_BL.GetYoteibi("1", ScVendor.Code, txtKeijouDate.Text, "0");

                    dtVendor = new DataTable();
                    dtVendor = khnyk_BL.Select_SearchName(txtKeijouDate.Text, 4, ScVendor.Code);
                    if (dtVendor.Rows.Count < 1)
                    {
                        khnyk_BL.ShowMessage("E101");
                        ScVendor.SetFocus(1);
                    }
                    else
                        ScVendor.LabelText = dtVendor.Rows[0]["Name"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(txtKeijouDate.Text) && !(string.IsNullOrWhiteSpace(ScStaff.Code)))
                {
                    dtStaff = new DataTable();
                    dtStaff = khnyk_BL.Select_SearchName(keijoudate, 5, ScStaff.Code);
                    if (dtStaff.Rows.Count < 1)
                    {
                        khnyk_BL.ShowMessage("E101");
                        ScStaff.SetFocus(1);
                    }
                    else
                        ScStaff.LabelText = dtStaff.Rows[0]["Name"].ToString();
                    }
                
            }
        }
        
        private void ScVendor_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScVendor.Code))
                {
                    dtVendor = new DataTable();
                    if (string.IsNullOrWhiteSpace(txtKeijouDate.Text))
                        keijoudate = System.DateTime.Now.ToString("yyyy/MM/dd");
                    else keijoudate = txtKeijouDate.Text;

                    dtVendor = khnyk_BL.Select_SearchName(keijoudate, 4, ScVendor.Code);
                    if (dtVendor.Rows.Count < 1)
                    {
                        khnyk_BL.ShowMessage("E101");
                        ScVendor.SetFocus(1);
                    }
                    else
                        ScVendor.LabelText = dtVendor.Rows[0]["Name"].ToString();
                }
            }
        }

        private void ScStaff_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScStaff.Code))
                {
                    staffName = Bind_StaffName(ScStaff.Code);
                    if (string.IsNullOrWhiteSpace(staffName))
                    {
                        khnyk_BL.ShowMessage("E101");
                        ScStaff.SetFocus(1);
                    }
                    else
                        ScStaff.LabelText = staffName;
                }
            }
        }
        private void ScVendor_Enter(object sender, EventArgs e)
        {
            keijoudate = string.IsNullOrWhiteSpace(txtKeijouDate.Text) ? txtKeijouDate.Text : System.DateTime.Now.ToString("yyyy/MM/dd");
            ScVendor.ChangeDate = keijoudate;
            ScVendor.Value1 = "2";
        }
        private void F7() // Delete current row and recalculate the TotalGaku
        {
            int row = dgvKehiNyuuryoku.CurrentCell.RowIndex;
            dgvKehiNyuuryoku.Rows.RemoveAt(row);

            var tb = (DataTable)dgvKehiNyuuryoku.DataSource;
            tb.AcceptChanges();
            BindTotalGaku(tb);
            for (int i = 0; i < (tb.Rows.Count); i++)
            {
                //dgvKehiNyuuryoku.Rows[dgvKehiNyuuryoku.Rows.Count - 1].Selected = true;
                dgvKehiNyuuryoku.Rows[i].Cells[0].Selected = true;
                dgvKehiNyuuryoku.Focus();
            }
        }

        private void dgvKehiNyuuryoku_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (dgvKehiNyuuryoku.Columns[e.ColumnIndex].Name == "colCostCD")
            //{
            //if (dgvKehiNyuuryoku.CurrentCell == dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostCD"])
            //{
            //    if (string.IsNullOrWhiteSpace(dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostCD"].Value.ToString()))
            //    {
            //        khnyk_BL.ShowMessage("E102");
            //        dgvKehiNyuuryoku.CurrentCell = dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostCD"];
            //    }
            //}
            //else if (dgvKehiNyuuryoku.CurrentCell == dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colDepartment"])
            //{
            //    if (string.IsNullOrWhiteSpace(dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colDepartment"].Value.ToString()))
            //    {
            //        khnyk_BL.ShowMessage("E102");
            //        dgvKehiNyuuryoku.CurrentCell = dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colDepartment"];
            //    }
            //}
            //}
            if (e.ColumnIndex == dgvKehiNyuuryoku.Columns["colCostCD"].Index)
            {
                if (string.IsNullOrWhiteSpace(dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostCD"].Value.ToString()))
                {
                    khnyk_BL.ShowMessage("E102");
                    //dgvKehiNyuuryoku.CurrentCell.Selected = true;
                    //dgvKehiNyuuryoku.NotifyCurrentCellDirty(true);
                    //dgvKehiNyuuryoku.BeginEdit(true);
                    dgvKehiNyuuryoku.CurrentCell = dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostCD"];
                }
            }
            else if (e.ColumnIndex == dgvKehiNyuuryoku.Columns["colDepartment"].Index)
            {
                if (string.IsNullOrWhiteSpace(dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colDepartment"].Value.ToString()))
                {
                    khnyk_BL.ShowMessage("E102");
                    dgvKehiNyuuryoku.CurrentCell = dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colDepartment"];
                }
            }
            else if (e.ColumnIndex == dgvKehiNyuuryoku.Columns["colCostGaku"].Index)
            {
                if (dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostGaku"].Value.ToString().Contains("-"))
                    dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostGaku"].Style.ForeColor = Color.Red;
                else
                    dgvKehiNyuuryoku.Rows[e.RowIndex].Cells["colCostGaku"].Style.ForeColor = Color.Black;

                BindTotalGaku(dt);
            }

        }

        private void F8() // Insert new row upon current row
        {
            int r = dgvKehiNyuuryoku.CurrentCell.RowIndex;
            var tb = (DataTable)dgvKehiNyuuryoku.DataSource;
            var row = tb.NewRow();
            tb.Rows.InsertAt(row, r);
            tb.AcceptChanges();
        }

        private void F10() // Insert new row, copy/paste data from upper row and Recalculate TotalGaku
        {
            int r = dgvKehiNyuuryoku.CurrentCell.RowIndex;
            var tb = (DataTable)dgvKehiNyuuryoku.DataSource;
            var row = tb.NewRow();
            tb.Rows.InsertAt(row, r);
            tb.AcceptChanges();

            for (int i = 0; i < dgvKehiNyuuryoku.Rows[r - 1].Cells.Count; i++)
            {
                dgvKehiNyuuryoku.Rows[r].Cells[i].Value = dgvKehiNyuuryoku.Rows[r - 1].Cells[i].Value;
            }

            BindTotalGaku(tb);
        }

        private void BindTotalGaku(DataTable dt) //Calculate TotalGaku and bind data to footer label
        {
            TotalGaku = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row["CostGaku"].ToString()))
                    TotalGaku += Convert.ToDecimal(row["CostGaku"]);
                
                if (TotalGaku.ToString().Equals("0"))
                    lblTotalGaku.Text = "0";
                else
                    lblTotalGaku.Text =  TotalGaku.ToString("#,##0");
            }

            if (!string.IsNullOrWhiteSpace(lblTotalGaku.Text) & lblTotalGaku.Text.ToString().Contains("-"))
                lblTotalGaku.ForeColor = Color.Red;
            else
                lblTotalGaku.ForeColor = Color.Black;
        }

       private string Bind_StaffName(string stCode)
        {
            dtStaff = new DataTable();
            string name = string.Empty;
            if (string.IsNullOrWhiteSpace(txtKeijouDate.Text))
                keijoudate = System.DateTime.Now.ToString("yyyy/MM/dd");
            else keijoudate = txtKeijouDate.Text;

            dtStaff = khnyk_BL.Select_SearchName(keijoudate, 5, stCode);
            if (dtStaff.Rows.Count > 0)
            {
                name = dtStaff.Rows[0]["Name"].ToString();
            }
            return name;
        }

        private void dgvKehiNyuuryoku_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow r in dgvKehiNyuuryoku.Rows)
            {
                if (r.Cells["colCostGaku"].Value.ToString().Contains("-"))
                    r.Cells["colCostGaku"].Style.ForeColor = Color.Red;
                else
                    r.Cells["colCostGaku"].Style.ForeColor = Color.Black;


            }
            //if (dgvKehiNyuuryoku.Rows.Count >1)  //2020-06-16 ptk
            //    dgvKehiNyuuryoku.CurrentRow.Selected = true; //2020-06-16 ptk
            ////dgvKehiNyuuryoku.CurrentRow.Selected = true;  //2020-06-16 ptk
        }
        private void dgvKehiNyuuryoku_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
            //try
            //{
            //    if (Convert.ToInt32(dgvKehiNyuuryoku.CurrentCell.EditedFormattedValue) > 0)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Enter valid number. . . ");
            //        dgvKehiNyuuryoku.CurrentCell.Value = 0;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Enter valid number. . . ");
            //    dgvKehiNyuuryoku.CurrentCell.Value = 0;
            //}
            //dgvKehiNyuuryoku.RefreshEdit();
        }
    }
}
