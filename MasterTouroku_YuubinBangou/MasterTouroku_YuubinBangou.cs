using System.Windows.Forms;
using BL;
using Entity;
using Search;
using Base.Client;
using CKM_Controls;
using System.Data;
using System;
using System.Drawing;

namespace MasterTouroku_YuubinBangou
{
    public partial class frmMasterTouroku_YuubinBangou : FrmMainForm
    {
        M_ZipCode_Entity ZipCode;
        MasterTouroku_YuubinBangou_BL YuubinBangouBL;
        DataTable dtDisplay;
        string Xml;
        public frmMasterTouroku_YuubinBangou()
        {
            InitializeComponent();

            Load += new System.EventHandler(FormLoadEvent);
            //KeyDown += Form_KeyDown;
            
            YuubinBangouBL = new MasterTouroku_YuubinBangou_BL();
        }
        
        private void FormLoadEvent(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            
            //OperationMode = EOperationMode.UPDATE;
            //DisablePanel(PanelDetail);
            //EnablePanel(PanelHeader);
            //btnDisplay.Enabled = F11Enable = true;
            //F12Enable = false;

            SelectNextControl(PanelDetail, true, true, true, true);
            dgvYuubinBangou.Hiragana_Column("colAdd1,colAdd2");

            Btn_F2.Text = string.Empty;
            Btn_F4.Text = string.Empty;
            Btn_F9.Text = string.Empty;
            SetRequireFields();
            CreateDataTable();
            BindGridCombo();
            ChangeMode(EOperationMode.UPDATE);
            txtZip1from.Focus();
        }

        private void SetRequireFields()
        {
            txtZip1from.Require(true);
            txtZip1To.Require(true);
            txtZip2From.Require(true);
            txtZip2To.Require(true);
        }

        private void BindGridCombo()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CarrierName", typeof(string));
            dt.Columns.Add("CarrierCD", typeof(string));
            dt = YuubinBangouBL.SimpleSelect1("62");
            DataRow row = dt.NewRow();
            row["CarrierName"] = string.Empty;
            row["CarrierCD"] = "0";
            dt.Rows.InsertAt(row, 0);

            DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)dgvYuubinBangou.Columns["colCarrier"];
            col.DataPropertyName = "Carrier";
            col.ValueMember = "CarrierCD";
            col.DisplayMember = "CarrierName";
            col.FlatStyle = FlatStyle.Flat;
            ((DataGridViewComboBoxColumn)dgvYuubinBangou.Columns["colCarrier"]).DataSource = dt;
        }

        public void  CreateDataTable()
        {
            dtDisplay = new DataTable();
            dtDisplay.Columns.Add("ZipCD1", typeof(string));
            dtDisplay.Columns.Add("ZipCD2", typeof(string));
            dtDisplay.Columns.Add("Address1", typeof(string));
            dtDisplay.Columns.Add("Address2", typeof(string));
            dtDisplay.Columns.Add("CarrierName", typeof(string));
            dtDisplay.Columns.Add("CarrierCD", typeof(string));
            dtDisplay.Columns.Add("CarrierLeadDay", typeof(string));

            dgvYuubinBangou.DataSource = dtDisplay;
        }

        public override void FunctionProcess(int index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
            switch (index + 1)
            {
                case 3:
                    ChangeMode(EOperationMode.UPDATE);
                    break;
                case 5:
                    ChangeMode(EOperationMode.SHOW);
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeMode(OperationMode);
                        txtZip1from.Focus();
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

        private void F11()
        {
            if (ErrorCheck(11))
            {
                switch (OperationMode)
                {
                    case EOperationMode.UPDATE:
                        if (DisplayData())
                        {
                            DisablePanel(PanelHeader);
                            EnablePanel(PanelDetail);
                            btnDisplay.Enabled = F11Enable = false;
                            F12Enable = true;
                            SelectNextControl(PanelDetail, true, true, true, true);
                        }
                        break;
                    case EOperationMode.DELETE:
                        if (DisplayData())
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            btnDisplay.Enabled = F11Enable = false;
                            SelectNextControl(PanelDetail, true, true, true, true);
                            F12Enable = true;
                        }
                        break;
                    case EOperationMode.SHOW:
                        if (DisplayData())
                        {
                            DisablePanel(PanelHeader);
                            DisablePanel(PanelDetail);
                            btnDisplay.Enabled = F11Enable = false;
                            F12Enable = false;
                        }
                        break;
                }

                //***Add Control Enable/Disable;
            }
        }

        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (YuubinBangouBL.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    switch (OperationMode)
                    {
                        case EOperationMode.UPDATE:
                            UpdateInsert();
                            break;
                    }
                }
                else PreviousCtrl.Focus();
            }
        }

        private bool DisplayData()
        {
            //*** Show Data
            ZipCode = new M_ZipCode_Entity
            {
                ZipCD1 = txtZip1from.Text,
                ZipCD2 =txtZip2From.Text,
            };

            string ZipCD1To = txtZip1To.Text;
            string ZipCD2To = txtZip2To.Text;

            
            dtDisplay = YuubinBangouBL.M_ZipCode_YuubinBangou_Select(ZipCode,ZipCD1To,ZipCD2To);

            if (dtDisplay != null)
            {
                dgvYuubinBangou.DataSource = dtDisplay;
                int i = 0;
                foreach (DataRow dr in dtDisplay.Rows)
                {
                    if (dr["CarrierCD"] != DBNull.Value )
                    {
                        dgvYuubinBangou.Rows[i].Cells["colCarrier"].Value = dr["CarrierCD"]; i++;
                    }
                    else
                    {
                        dgvYuubinBangou.Rows[i].Cells["colCarrier"].Value = string.Empty; i++;
                    }
                }
                txtZip1from.Focus();

                return true;
            }
            else
            {
                YuubinBangouBL.ShowMessage("E133");
                return false;
            }
        }

       
        private void UpdateInsert()
        {
            //*** Insert Update Function
            ZipCode = GetZipCodeEntity();
            string ZipCD1To = txtZip1To.Text;
            string ZipCD2To = txtZip2To.Text;

            if (dtDisplay.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dtDisplay.Rows)
                {
                    dr["CarrierCD"] = dgvYuubinBangou.Rows[i].Cells["colCarrier"].Value; i++;

                    if(dr["ZipCD1"] == DBNull.Value)
                    {
                        dtDisplay.Rows.Remove(dr);
                    }
                    else if(dr["ZipCD1"] != DBNull.Value && dr["CarrierLeadDay"] == DBNull.Value)
                    {
                        dr["CarrierLeadDay"] = "0";
                    }
                }

                Xml = YuubinBangouBL.DataTableToXml(dtDisplay);

                if (YuubinBangouBL.M_ZipCode_Update(ZipCode, ZipCD1To, ZipCD2To, Xml))
                {
                    Clear(PanelHeader);
                    Clear(PanelDetail);

                    ChangeMode(OperationMode);
                    txtZip1from.Focus();

                    YuubinBangouBL.ShowMessage("I101");
                }
                else
                {
                    YuubinBangouBL.ShowMessage("S001");
                }
            }
        }

        public M_ZipCode_Entity GetZipCodeEntity()
        {
            ZipCode = new M_ZipCode_Entity
            {
                ZipCD1 = txtZip1from.Text,
                ZipCD2 = txtZip2From.Text,
                Operator = InOperatorCD,
                ProcessMode = ModeText,
                ProgramID = InProgramID,
                PC = InPcID,
                Key = txtZip1from.Text + "" + txtZip1To.Text + " " + txtZip2From.Text + "" + txtZip2To.Text
             };
            
            return ZipCode;
        }

       
        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                if (!RequireCheck(new Control[] { txtZip1from,txtZip1To,txtZip2From,txtZip2To }))
                    return false;

                //HeaderCheck on F11
                if ((!string.IsNullOrWhiteSpace(txtZip1from.Text)) && (string.IsNullOrWhiteSpace(txtZip2From.Text)))
                {
                    YuubinBangouBL.ShowMessage("E102");
                    txtZip2From.Focus();
                    return false;
                }

                if ((!string.IsNullOrWhiteSpace(txtZip1To.Text)) && (string.IsNullOrWhiteSpace(txtZip2To.Text)))
                {
                    YuubinBangouBL.ShowMessage("E102");
                    txtZip2To.Focus();
                    return false;
                }

                //if ((!string.IsNullOrWhiteSpace(txtZip2From.Text)) && (!string.IsNullOrWhiteSpace(txtZip2To.Text)))
                //{
                //    if ((Convert.ToInt32(txtZip2From.Text.ToString())) > (Convert.ToInt32(txtZip2To.Text.ToString())))
                //    {
                //        YuubinBangouBL.ShowMessage("E106");
                //        txtZip2To.Focus();
                //        return false;
                //    }
                //}

                if ((!string.IsNullOrWhiteSpace(txtZip1from.Text)) && (!string.IsNullOrWhiteSpace(txtZip1To.Text)))
                {
                    if ((Convert.ToInt32(txtZip1from.Text.ToString() + txtZip2From.Text.ToString())) > (Convert.ToInt32(txtZip1To.Text.ToString() + txtZip2To.Text.ToString())))
                    {
                        YuubinBangouBL.ShowMessage("E106");
                        txtZip2To.Focus();
                        return false;
                    }
                }
            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { txtZip1from, txtZip1To, txtZip2From, txtZip2To }))
                    return false;

                foreach (DataGridViewRow row in dgvYuubinBangou.Rows)
                {
                    if (!(row.Cells["colZipCD1"].Value == null))
                    {
                        if (!string.IsNullOrWhiteSpace(row.Cells["colZipCD1"].Value.ToString()))
                        {
                            if (!string.IsNullOrWhiteSpace(row.Cells["colZipCD1"].Value.ToString()) && string.IsNullOrWhiteSpace(row.Cells["colZipCD2"].Value.ToString()))
                            {
                                YuubinBangouBL.ShowMessage("E102");
                                dgvYuubinBangou.CurrentCell = row.Cells["colZipCD2"];
                                dgvYuubinBangou.BeginEdit(true);
                                return false;
                            }

                            DataRow[] dr = dtDisplay.Select("ZipCD1 = '" + row.Cells["colZipCD1"].Value + "' AND ZipCD2 = '" + row.Cells["colZipCD2"].Value + "'");
                            if (dr.Length > 1)
                            {
                                YuubinBangouBL.ShowMessage("E105");
                                dgvYuubinBangou.CurrentCell = row.Cells["colZipCD2"];
                                dgvYuubinBangou.BeginEdit(true);
                                return false;
                            }

                            if (!string.IsNullOrWhiteSpace(row.Cells["colZipCD1"].Value.ToString()) && !string.IsNullOrWhiteSpace(row.Cells["colZipCD2"].Value.ToString()))
                            {
                                foreach(DataRow r in dtDisplay.Rows)
                                {
                                    if(r.RowState == DataRowState.Added)
                                    {
                                        DataTable dt = new DataTable();
                                        dt = YuubinBangouBL.SimpleSelect1("30", null, r["ZipCD1"].ToString(), r["ZipCD2"].ToString());
                                        if(dt.Rows.Count > 0 )
                                        {
                                            YuubinBangouBL.ShowMessage("E105");
                                            return false;
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(row.Cells["colZipCD1"].Value.ToString()) && string.IsNullOrWhiteSpace(row.Cells["colAdd1"].Value.ToString()))
                            {
                                YuubinBangouBL.ShowMessage("E102");
                                dgvYuubinBangou.CurrentCell = row.Cells["colAdd1"];
                                dgvYuubinBangou.BeginEdit(true);
                                return false;
                            }

                            //if (!string.IsNullOrWhiteSpace(row.Cells["colZipCD1"].Value.ToString()) && string.IsNullOrWhiteSpace(row.Cells["colAdd2"].Value.ToString()))
                            //{
                            //    YuubinBangouBL.ShowMessage("E102");
                            //    dgvYuubinBangou.CurrentCell = row.Cells["colAdd2"];
                            //    dgvYuubinBangou.BeginEdit(true);
                            //    return false;
                            //}
                        }
                        //else if(!string.IsNullOrWhiteSpace(row.Cells["colZipCD2"].Value.ToString()) && (string.IsNullOrWhiteSpace(row.Cells["colZipCD1"].Value.ToString())))
                        //{
                        //    YuubinBangouBL.ShowMessage("E102");
                        //    dgvYuubinBangou.CurrentCell = row.Cells["colZipCD1"];
                        //    dgvYuubinBangou.BeginEdit(true);
                        //    return false;
                        //}
                        //else if (string.IsNullOrWhiteSpace(row.Cells["colZipCD1"].Value.ToString()))
                        //{
                        //    YuubinBangouBL.ShowMessage("E102");
                        //    dgvYuubinBangou.CurrentCell = row.Cells["colZipCD1"];
                        //    dgvYuubinBangou.BeginEdit(true);
                        //    return false;
                        //}
                        else
                        {
                            //row.Cells["colZipCD1"].Value = null;
                            //row.Cells["colZipCD2"].Value = null;
                            row.Cells["colAdd1"].Value = null;
                            row.Cells["colAdd2"].Value = null;
                            return true;
                        }
                    }
                }
            }
            return true;
        }

        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.UPDATE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(PanelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(PanelDetail);
                    F12Enable = false;
                    btnDisplay.Enabled = F11Enable = true;
                    BindGridCombo();
                    break;
            }
            PanelHeader.Focus();
            txtZip1from.Focus();
            
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void txtZip2From_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if ((!string.IsNullOrWhiteSpace(txtZip1from.Text)) && (string.IsNullOrWhiteSpace(txtZip2From.Text)))
                {
                    YuubinBangouBL.ShowMessage("E102");
                    txtZip2From.Focus();
                }
            }
        }

        private void txtZip2To_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if ((!string.IsNullOrWhiteSpace(txtZip1To.Text)) && (string.IsNullOrWhiteSpace(txtZip2To.Text)))
                {
                    YuubinBangouBL.ShowMessage("E102");
                    txtZip2To.Focus();
                }

                if(!string.IsNullOrWhiteSpace(txtZip1from.Text) && !string.IsNullOrWhiteSpace(txtZip1To.Text))
                {
                    if ((Convert.ToInt32(txtZip1from.Text.ToString())) > (Convert.ToInt32(txtZip1To.Text.ToString())))
                    {
                        YuubinBangouBL.ShowMessage("E106");
                        txtZip1To.Focus();
                    }
                }

                // if (!string.IsNullOrWhiteSpace(txtZip2From.Text) && !string.IsNullOrWhiteSpace(txtZip2To.Text))
                //{
                //    if (Convert.ToInt32(txtZip2From.Text.ToString()) > Convert.ToInt32(txtZip2To.Text.ToString()))
                //    {
                //        YuubinBangouBL.ShowMessage("E106");
                //        txtZip2To.Focus();
                //    }
                //}
            }
        }

        private void txtZip1To_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (!string.IsNullOrWhiteSpace(txtZip1from.Text) && !string.IsNullOrWhiteSpace(txtZip1To.Text))
            //    {
            //        if ((Convert.ToInt32(txtZip1from.Text.ToString())) > (Convert.ToInt32(txtZip1To.Text.ToString())))
            //        {
            //            YuubinBangouBL.ShowMessage("E106");
            //            txtZip1To.Focus();
            //        }
            //    }
            //}
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }
        
        private void dgvYuubinBangou_Paint(object sender, PaintEventArgs e)
        {
            string[] monthes = { "郵便番号", "住所1", "住所2" };
            for (int j = 0; j < 1;)
            {
                Rectangle r1 = this.dgvYuubinBangou.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.dgvYuubinBangou.GetCellDisplayRectangle(j + 1, -1, true).Width;
                //int w2 = this.dgvYuubinBangou.GetCellDisplayRectangle(j + 2, -1, true).Width;
                r1.X += 2;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dgvYuubinBangou.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(monthes[j / 2],
                this.dgvYuubinBangou.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.dgvYuubinBangou.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 2;
            }
        }

        private void dgvYuubinBangou_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
            }
        }
       
        private void dgvYuubinBangou_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((sender as DataGridView).CurrentCell is DataGridViewTextBoxCell)
            {
                if (dgvYuubinBangou.CurrentCell == dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"])
                {
                    if (!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"].Value.ToString()))
                    {
                        YuubinBangouBL.ShowMessage("E102");
                        dgvYuubinBangou.CurrentCell = dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"];
                       // dgvYuubinBangou.BeginEdit(true);
                    }

                    if (!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"].Value.ToString()) && string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()))
                    {
                        YuubinBangouBL.ShowMessage("E102");
                        dgvYuubinBangou.CurrentCell = dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"];
                      //dgvYuubinBangou.BeginEdit(true);
                        
                    }

                    if (!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && !string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"].Value.ToString()))
                    {
                        DataRow[] drarr = dtDisplay.Select("ZipCD1 = '" + dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value + "' AND ZipCD2 = '" + dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"].Value + "'");
                        if (drarr.Length > 1)
                        {
                            YuubinBangouBL.ShowMessage("E105");
                            dgvYuubinBangou.CurrentCell = dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"];
                           // dgvYuubinBangou.BeginEdit(true);
                        }
                    }
                }
                else if (dgvYuubinBangou.CurrentCell == dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"])
                {
                    if (!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"].Value.ToString()))
                    {
                        YuubinBangouBL.ShowMessage("E102");
                        dgvYuubinBangou.CurrentCell = dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"];
                        
                       // dgvYuubinBangou.BeginEdit(true);
                    }
                    else if (string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && !string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"].Value.ToString()))
                    {
                        dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"].Value = null;
                    }
                }
                else if (dgvYuubinBangou.CurrentCell == dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd2"])
                {
                    //if (!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd2"].Value.ToString()))
                    //{
                    //    YuubinBangouBL.ShowMessage("E102");
                    //    dgvYuubinBangou.CurrentCell = dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd2"];
                    //}

                    if (string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && !string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd2"].Value.ToString()))
                    {
                        dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd2"].Value = null;
                    }
                }
            }
        }

        private void frmMasterTouroku_YuubinBangou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void dgvYuubinBangou_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            
        }

        private void dgvYuubinBangou_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(dgvYuubinBangou.CurrentCell.EditedFormattedValue) < 256 && Convert.ToInt32(dgvYuubinBangou.CurrentCell.EditedFormattedValue) > 0)
                {
                    return;
                }
                else
                {
                    MessageBox.Show("Enter valid number. . . ");
                    dgvYuubinBangou.CurrentCell.Value = 0;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Enter valid number. . . ");
                dgvYuubinBangou.CurrentCell.Value = 0;
            }
            dgvYuubinBangou.RefreshEdit();
        }

        private void dgvYuubinBangou_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //dgvYuubinBangou.Rows[e.RowIndex].ErrorText = "0";

        }
    }
}
