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

namespace TanabanNyuuryoku
{
    public partial class FrmTanabanNyuuryoku : FrmMainForm
    {
        TanabanNyuuryoku_BL tnbnBL = new TanabanNyuuryoku_BL();
        M_Souko_Entity mse = new M_Souko_Entity();
        M_Location_Entity mle = new M_Location_Entity();
        D_Stock_Entity dse = new D_Stock_Entity();
        ZaikoIdouNyuuryoku_BL zibl;

        DataTable dtstorage = new DataTable();

        public FrmTanabanNyuuryoku()
        {
            InitializeComponent();
        }

        private void FrmTanabanNyuuryoku_Load(object sender, EventArgs e)
        {
            InProgramID = "TanabanNyuuryoku";

            //SetFunctionLabel(EProMode.MENTE);
            this.OperationMode = EOperationMode.UPDATE;
            this.ModeText = "登録";
            
            StartProgram();

            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            F7Visible = false;
            F8Visible = false;
            F10Visible = false;
            //F11Visible = false;
            F9Visible = false;

            //txtArrivalDateFrom.Text = DateTime.Now.ToString("yyyy/MM/dd");
            txtArrivalDateTo.Text = DateTime.Now.ToString("yyyy/MM/dd");

            BindCombo();
            chkNotRegister.Checked = true;
            ScStorage.Value1 = cboWarehouse.SelectedValue.ToString();

            SetRequireField();

            txtArrivalDateFrom.Focus();

            dgvTanaban.CheckCol.Add("colRackNo1");
        }

        private void BindCombo()
        {
            cboWarehouse.Bind(string.Empty, "");
        }

        private void SetRequireField()
        {
            txtArrivalDateTo.Require(true);
            cboWarehouse.Require(true);
        }


        /// <summary>
        /// override F1 Button click
        /// </summary>
        protected override void EndSec()
        {
            this.Close();
        }

        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                case 3:
                case 4:
                case 5:
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear();
                        //ScVendor.SetFocus(1);
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
                dse = GetStockEntity();
                
                dtstorage = tnbnBL.M_Location_DataSelect(dse);
                if (dtstorage.Rows.Count == 0)
                {
                    tnbnBL.ShowMessage("E128");
                    dgvTanaban.DataSource = string.Empty;
                    txtArrivalDateFrom.Focus();
                }
                else
                {
                    dgvTanaban.DataSource = dtstorage;
                    dgvTanaban.CurrentCell = dgvTanaban[0, 0];
                }
            }
            else
            {
                dgvTanaban.DataSource = string.Empty;
            }
        }

        private M_Location_Entity GetEntity()
        {
             mle.Unregister = chkNotRegister.Checked ? "1" : "0";
             mle.Register = chkRegister.Checked ? "1" : "0";
             mle.ProcessMode = ModeText;
             mle.Operator = InOperatorCD;
             mle.ProgramID = InProgramID;
             mle.Key = cboWarehouse.SelectedValue.ToString();
             mle.PC = InPcID;
             return mle;
        }

        private D_Stock_Entity GetStockEntity()
        {
            dse.ArrivalStartDate = txtArrivalDateFrom.Text;
            dse.ArrivalEndDate = txtArrivalDateTo.Text;
            dse.SoukoCD = cboWarehouse.SelectedValue.ToString();
            dse.UnregisterFlg = chkNotRegister.Checked ? "1" : "0";
            dse.RegisterFlg = chkRegister.Checked ? "1" : "0";
            dse.ProcessMode = ModeText;
            dse.Operator = InOperatorCD;
            dse.ProgramID = InProgramID;
            dse.PC = InPcID;
            return dse;
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }
        #endregion

        #region ErrorCheck 
        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                if (!RequireCheck(new Control[] { txtArrivalDateTo }))
                    return false;
                if (!string.IsNullOrWhiteSpace(txtArrivalDateTo.Text))
                {
                    int result = txtArrivalDateFrom.Text.CompareTo(txtArrivalDateTo.Text);
                    if (result > 0)
                    {
                        tnbnBL.ShowMessage("E104");
                        txtArrivalDateTo.Focus();
                        return false;
                    }
                }

                if (cboWarehouse.SelectedValue.ToString() == "-1")
                {
                    tnbnBL.ShowMessage("E102");
                    cboWarehouse.Focus();
                    return false;
                }
                else
                {
                    mse.DeleteFlg = "0";
                    DataTable dtsouko = new DataTable();
                    dtsouko = tnbnBL.D_Souko_Select(mse);
                    if (dtsouko.Rows.Count == 0)
                    {
                        tnbnBL.ShowMessage("E128");
                        cboWarehouse.Focus();
                        return false;
                    }

                }

                if (chkNotRegister.Checked == false)
                {
                    if (chkRegister.Checked == false)
                    {
                        tnbnBL.ShowMessage("E111");
                        chkNotRegister.Focus();
                        return false;
                    }               
                }

                //if (!RequireCheck(new Control[] { ScStorage.TxtCode }))
                //    return false;

                if (!string.IsNullOrWhiteSpace (ScStorage.TxtCode.Text))
                { 
                    mle.SoukoCD = cboWarehouse.SelectedValue.ToString();
                    mle.TanaCD = ScStorage.TxtCode.Text;
                    DataTable dtLocation = new DataTable();
                    dtLocation = tnbnBL.M_LocationTana_Select(mle);
                    if (dtLocation.Rows.Count == 0)
                    {
                        tnbnBL.ShowMessage("E101");
                        ScStorage.SetFocus(1);
                        return false;
                    }
                }

            }
            else if (index == 12)
            {
                //if (!RequireCheck(new Control[] { ScStorage.TxtCode }))
                //    return false;
                //else
                //{
                //    mle.SoukoCD = cboWarehouse.SelectedValue.ToString();
                //    mle.TanaCD = ScStorage.TxtCode.Text;
                //    DataTable dtLocation = new DataTable();
                //    dtLocation = tnbnBL.M_LocationTana_Select(mle);
                //    if (dtLocation.Rows.Count == 0)
                //    {
                //        tnbnBL.ShowMessage("E101");
                //        ScStorage.SetFocus(1);
                //        return false;
                //    }
                //}


                //if(dgvTanaban.Rows.Count > 0 )
                //{
                //    foreach (DataGridViewRow row in dgvTanaban.Rows)
                //    {
                //        string tana = row.Cells["colRackNo1"].Value.ToString();
                //        if (string.IsNullOrWhiteSpace(tana))
                //        {
                //            tnbnBL.ShowMessage("E102");
                //            dgvTanaban.ClearSelection();
                //            row.Cells["colRackNo1"].Selected = true;
                //            return false;
                //        }
                //        else
                //        {
                //            mle.SoukoCD = cboWarehouse.SelectedValue.ToString();
                //            mle.TanaCD = tana;
                //            DataTable dtLocation = new DataTable();
                //            dtLocation = tnbnBL.M_LocationTana_Select(mle);
                //            if (dtLocation.Rows.Count == 0)
                //            {
                //                tnbnBL.ShowMessage("E101");
                //                dgvTanaban.ClearSelection();
                //                row.Cells["colRackNo1"].Selected = true;
                //                return false;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    tnbnBL.ShowMessage("E128");
                //    txtArrivalDateFrom.Focus();
                //    return false;
                //} //02.10.2020 pnz

                if (dgvTanaban.Rows.Count == 0)
                {
                    tnbnBL.ShowMessage("E128");
                    txtArrivalDateFrom.Focus();
                    return false;
                }
              
            }
            return true;
        }
        #endregion

        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (tnbnBL.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    mle = new M_Location_Entity();
                    dse = new D_Stock_Entity();
                    DataTable dtUpdate = new DataTable();
                    dtUpdate.Columns.Add("RackNo");
                    dtUpdate.Columns.Add("StockNo");
                   
                    foreach(DataGridViewRow row in dgvTanaban.Rows)
                    {
                        //bool chk =(bool)row.Cells["colChk"].Value;
                        //if(chk)
                        //{
                            DataRow dtrow = dtUpdate.NewRow();
                            dtrow["RackNo"] = row.Cells["colRackNo1"].Value.ToString();                          
                            dtrow["StockNo"] = row.Cells["colStockNo"].Value.ToString();
                            dtUpdate.Rows.Add(dtrow);
                        //}
                       
                    }

                    
                   
                    dse = new D_Stock_Entity
                    {
                        dt1 = dtUpdate,
                    };
                    mle = GetEntity();
                    
                     InsertUpdate(dse,mle);
                }
                else
                    PreviousCtrl.Focus();
            }
        }

        private void InsertUpdate(D_Stock_Entity dse,M_Location_Entity mle)
        {
            //*** Insert Update Function
            if (tnbnBL.M_Location_InsertUpdate (dse,mle))
            {
                //Clear(PanelHeader);
                ////Clear(panelDetail);
                //BindCombo();
                //chkNotRegister.Checked = true;
                //ScStorage.Value1 = cboWarehouse.SelectedValue.ToString();
                //dgvTanaban.ClearSelection();
                //txtArrivalDateFrom.Focus();
                Clear();
                tnbnBL.ShowMessage("I101");
            }
            else
            {
                tnbnBL.ShowMessage("S001");
            }
        }
     
        private void Clear()
        {
            //txtArrivalDateFrom.Text = DateTime.Now.ToString("yyyy/MM/dd");
            txtArrivalDateFrom.Text = string.Empty;
            txtArrivalDateTo.Text = DateTime.Now.ToString("yyyy/MM/dd");

            BindCombo();
            chkNotRegister.Checked = true;
            chkRegister.Checked = false;
            //Clear(panel1);

            dgvTanaban.DataSource = string.Empty;
            txtArrivalDateFrom.Focus();
        }

        private void txtArrivalDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtArrivalDateTo.Text))
                {
                    int result = txtArrivalDateFrom.Text.CompareTo(txtArrivalDateTo.Text);
                    if (result > 0)
                    {
                        tnbnBL.ShowMessage("E104");
                        txtArrivalDateTo.Focus();
                    }
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row1 in dgvTanaban.Rows)
            {
                if (Convert.ToBoolean(row1.Cells["colChk"].EditedFormattedValue) == false)
                {
                    row1.Cells["colChk"].Value = true;
                }
            }
        }

        private void btnReleaseAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row1 in dgvTanaban.Rows)
            {
                if (Convert.ToBoolean(row1.Cells["colChk"].EditedFormattedValue) == true)
                {
                    row1.Cells["colChk"].Value = false;
                }
            }
        }

        private void btnApplicable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ScStorage.TxtCode.Text))
            {
                tnbnBL.ShowMessage("E102");
                ScStorage.SetFocus(1);
            }
            else
            {
                foreach (DataGridViewRow row in dgvTanaban.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["colChk"].EditedFormattedValue) == true)
                    {
                        row.Cells["colRackNo1"].Value = ScStorage.TxtCode.Text;
                        row.Cells["colChk"].Value = false;
                    }
                }
            }
        }

        private void FrmTanabanNyuuryoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void cboWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScStorage.Value1 = cboWarehouse.SelectedValue.ToString();
        }

        private void ScStorage_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScStorage.TxtCode.Text))
                {
                    mle = new M_Location_Entity();
                    tnbnBL = new TanabanNyuuryoku_BL();

                    mle.SoukoCD = cboWarehouse.SelectedValue.ToString();
                    mle.TanaCD = ScStorage.TxtCode.Text;
                    DataTable dtLocation = new DataTable();
                    dtLocation = tnbnBL.M_LocationTana_Select(mle);
                    if (dtLocation.Rows.Count == 0)
                    {
                        tnbnBL.ShowMessage("E101");
                        ScStorage.SetFocus(1);                     
                    }

                }
                //else
                //{
                //    tnbnBL.ShowMessage("E102");
                //    ScStorage.SetFocus(1);
                //}
            }
        }

        private M_Location_Entity GetSearchInfo()
        {
            string ymd = bbl.GetDate();
            mle = new M_Location_Entity
            {
                ChangeDate =ymd,
                SoukoCD = cboWarehouse.SelectedValue.ToString().Equals("-1") ? string.Empty : cboWarehouse.SelectedValue.ToString(),
                TanaCD = ScStorage.TxtCode.Text,
            };
            return mle;
        }

        private void dgvTanaban_Paint(object sender, PaintEventArgs e)
        {
            string[] monthes = { "棚番" };
            for (int j = 1; j < 3;)
            {
                Rectangle r1 = this.dgvTanaban.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.dgvTanaban.GetCellDisplayRectangle(j + 1, -1, true).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dgvTanaban.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(monthes[j / 2],
                this.dgvTanaban.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.dgvTanaban.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 3;
            }
        }

        private void dgvTanaban_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.RowIndex >= 0)
            {
                var row = this.dgvTanaban.Rows[e.RowIndex];
                if (senderGrid.Columns[e.ColumnIndex].ReadOnly == false)
                {
                    if (dgvTanaban.Columns["colBtn"].Index == e.ColumnIndex)
                    {
                        Search_Location sl = new Search_Location(DateTime.Now.ToShortDateString(), cboWarehouse.SelectedValue.ToString());
                        sl.ShowDialog();
                        if (!string.IsNullOrWhiteSpace(sl.TanaCD))
                        {
                            row.Cells[dgvTanaban.Columns[e.ColumnIndex - 1].Index].Value = sl.TanaCD;

                            if (!string.IsNullOrWhiteSpace(ScStorage.TxtCode.Text))
                            {
                                row.Cells[dgvTanaban.Columns["colRackNo1"].Index].Value = sl.TanaCD;

                            }
                        }
                    }
                }
            }
        }

        private void dgvTanaban_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height;
                r2.Height = e.CellBounds.Height;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
            }
        }

        private void dgvTanaban_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvTanaban.Columns[e.ColumnIndex + 1].Name == "colRackNo1")
            {
                F9Visible = true;
            }
            //else if (dgvTanaban.Columns[e.ColumnIndex + 1].Name == "colBtn")
            //{
            //    var row = this.dgvTanaban.Rows[e.RowIndex];
            //    Search_Location sl = new Search_Location(DateTime.Now.ToShortDateString(), cboWarehouse.SelectedValue.ToString());
            //    sl.ShowDialog();
            //    if (!string.IsNullOrWhiteSpace(sl.TanaCD))
            //    {
            //        row.Cells[dgvTanaban.Columns[e.ColumnIndex - 1].Index].Value = sl.TanaCD;

            //        if (!string.IsNullOrWhiteSpace(ScStorage.TxtCode.Text))
            //        {
            //            row.Cells[dgvTanaban.Columns["colRackNo1"].Index].Value = sl.TanaCD;

            //        }
            //    }
            //}
            else if (dgvTanaban.Columns[e.ColumnIndex].Name == "colRackNo1")
            {
                F9Visible = true;
            }
          
                if (dgvTanaban.lastKey)
                if (dgvTanaban.Columns[e.ColumnIndex].Name == "colRackNo1")
                {
                    string rate = dgvTanaban.Rows[e.RowIndex].Cells["colRackNo1"].EditedFormattedValue.ToString();
                    if (!String.IsNullOrWhiteSpace(rate))
                    {
                        mle = new M_Location_Entity();
                        tnbnBL = new TanabanNyuuryoku_BL();

                        mle.SoukoCD = cboWarehouse.SelectedValue.ToString();
                        mle.TanaCD = rate;
                        DataTable dtLocation = new DataTable();
                        dtLocation = tnbnBL.M_LocationTana_Select(mle);
                        if (dtLocation.Rows.Count == 0)
                        {
                            tnbnBL.ShowMessage("E101");
                            //dgvTanaban.RefreshEdit();
                            e.Cancel = true;
                            
                        }
                        dgvTanaban.lastKey = false;
                    }
                    //else
                    //{
                    //    //MessageBox.Show("enter valid no");
                    //    bbl.ShowMessage("E102");
                    //    //dgvTanaban.BeginEdit(true);
                    e.Cancel = true;
                    dgvTanaban.lastKey = false;
                    //}
                }
        }

        private void dgvTanaban_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch
            {
                //MessageBox.Show("Enter valid no");
                //dgvTanaban.RefreshEdit();
                e.Cancel = false;
            }
        }

        private void dgvTanaban_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //最終行最終列の場合は、F1へ
                if ((dgvTanaban.CurrentCellAddress.X == dgvTanaban.ColumnCount - 9) &&
                    (dgvTanaban.CurrentCellAddress.Y == dgvTanaban.RowCount - 1))
                {
                    Btn_F1.Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
    }
}
