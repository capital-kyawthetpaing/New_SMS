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
            F11Visible = false;

            txtArrivalDateFrom.Text = DateTime.Now.ToString("yyyy/MM/dd");
            txtArrivalDateTo.Text = DateTime.Now.ToString("yyyy/MM/dd");

            BindCombo();
            chkNotRegister.Checked = true;
            cboWarehouse.SelectedValue = SoukoCD;
            

            SetRequireField();

            txtArrivalDateFrom.Focus();
        }

        private void BindCombo()
        {
            cboWarehouse.Bind(string.Empty,"");
        }

        private void SetRequireField()
        {
            cboWarehouse.Require(true);
        }
        //ScStorage.Value1 = cboWarehouse.SelectedValue.ToString();

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
                case 6:
                    Clear();
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
                mle = GetEntity();
                
                dtstorage = tnbnBL.M_Location_DataSelect(mle);
                if (dtstorage.Rows.Count == 0)
                {
                    tnbnBL.ShowMessage("E128");
                }
                else
                {
                    dgvTanaban.DataSource = dtstorage;
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
                if (!string.IsNullOrWhiteSpace(txtArrivalDateTo.Text))
                {
                    int result = txtArrivalDateFrom.Text.CompareTo(txtArrivalDateTo.Text);
                    if (result >= 0)
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

                if (chkNotRegister.Checked == false || chkRegister.Checked == false)
                {
                    tnbnBL.ShowMessage("E111");
                    chkNotRegister.Focus();
                    return false;            
                }

                if(!string.IsNullOrWhiteSpace (ScStorage.TxtCode.Text))
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
               

                //if (!RequireCheck(new Control[] { ScStorage.TxtCode }))
                //    return false;
               
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

                foreach (DataGridViewRow row in dgvTanaban.Rows)
                {
                    string tana = row.Cells["colRackNo1"].Value.ToString();
                    if (string.IsNullOrWhiteSpace(tana))
                    {
                        tnbnBL.ShowMessage("E102");
                        dgvTanaban.ClearSelection();
                        row.Cells["colRackNo1"].Selected = true;
                        return false;
                    }
                    else
                    {
                        mle.SoukoCD = cboWarehouse.SelectedValue.ToString();
                        mle.TanaCD = tana;
                        DataTable dtLocation = new DataTable();
                        dtLocation = tnbnBL.M_LocationTana_Select(mle);
                        if (dtLocation.Rows.Count == 0)
                        {
                            tnbnBL.ShowMessage("E101");
                            dgvTanaban.ClearSelection();
                            row.Cells["colRackNo1"].Selected = true;
                            return false;
                        }
                    }
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
                        DataRow dtrow = dtUpdate.NewRow();
                        dtrow["RackNo"] = row.Cells["colRackNo1"].Value.ToString();
                        dtrow["StockNo"] = row.Cells["colStockNo"].Value.ToString();
                        dtUpdate.Rows.Add(dtrow);
                    }

                    dse = new D_Stock_Entity
                    {
                        dt1 = dtUpdate,
                    };
                    mle = GetEntity();
                    
                     InsertUpdate(dse,mle);
                }
                else PreviousCtrl.Focus();
            }
        }

        private void InsertUpdate(D_Stock_Entity dse,M_Location_Entity mle)
        {
            //*** Insert Update Function
            if (tnbnBL.M_Location_InsertUpdate (dse,mle))
            {
                Clear(PanelHeader);
                Clear(panelDetail);
                txtArrivalDateFrom.Focus();
                tnbnBL.ShowMessage("I101");
            }
            else
            {
                tnbnBL.ShowMessage("S001");
            }
        }
     
        private void Clear()
        {
            Clear(panelDetail);         
        }

        private void txtArrivalDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtArrivalDateTo.Text))
                {
                    int result = txtArrivalDateFrom.Text.CompareTo(txtArrivalDateTo.Text);
                    if (result >= 0)
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
                    }
                }
            }
        }

        private void FrmTanabanNyuuryoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
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
            if(e.RowIndex >= 0)
            {
                var row = this.dgvTanaban.Rows[e.RowIndex];
                if(senderGrid .Columns[e.ColumnIndex].ReadOnly == false)
                {
                    if(dgvTanaban .Columns ["colBtn"].Index == e.ColumnIndex)
                    {
                        Search_Location sl = new Search_Location(DateTime.Now.ToShortDateString() , cboWarehouse.SelectedValue.ToString());
                        sl.ShowDialog();
                        if(!string.IsNullOrWhiteSpace(sl.TanaCD))
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

        private void cboWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScStorage.Value1 = cboWarehouse.SelectedValue.ToString();
        }

        //private void CheckRowAdd(DataGridViewRow row)
        //{
        //    if(row.Index == dgvTanaban.Rows.Count -1)
        //    {
        //        if(!string.IsNullOrWhiteSpace(row.Cells[dgvTanaban.Columns["colbtn"].Index].Value.ToString()))
        //        {

        //        }
        //    }
        //}

    }
}
