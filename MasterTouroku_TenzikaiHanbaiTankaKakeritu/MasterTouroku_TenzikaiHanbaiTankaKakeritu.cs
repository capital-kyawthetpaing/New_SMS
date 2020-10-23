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

namespace MasterTouroku_TenzikaiHanbaiTankaKakeritu
{
    public partial class FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu : FrmMainForm
    {
        M_TenzikaiShouhin_Entity mTSE;
        MasterTouroku_TenzikaiHanbaiTankaKakeritu_BL bl;
        DataTable dtSelect;
        public FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu()
        {
            InitializeComponent();
            mTSE = new M_TenzikaiShouhin_Entity();
            bl = new MasterTouroku_TenzikaiHanbaiTankaKakeritu_BL();
            dtSelect = new DataTable();
        }

        private void FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_TenzikaiHanbaiTankaKakeritu";
            StartProgram();
            ModeText = "登録";
            string ymd = bbl.GetDate();
            CB_Year.Bind(ymd);
            CB_Season.Bind(ymd);
            SC_Tanka.TxtCode.Require(true);
            GV_Tenzaishohin.CheckCol.Add("Rate");
            SC_Tanka.SetFocus(1);
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { SC_Tanka.TxtCode })) //Step1
                return false;

            if (!SC_Tanka.IsExists(2))
            {
                bbl.ShowMessage("E101");
                SC_Tanka.SetFocus(1);
                return false;
            }
            if (!String.IsNullOrEmpty(SC_Brand.TxtCode.Text))
            {
                if (!SC_Brand.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_Brand.SetFocus(1);
                    return false;
                }
            }

            if (!String.IsNullOrEmpty(Sc_Segment.TxtCode.Text))
            {
                if (!Sc_Segment.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    Sc_Segment.SetFocus(1);
                    return false;
                }
            }

            //Console.WriteLine(header.Trim(new Char[] { ' ', '*', '.' }));

            if (!String.IsNullOrEmpty(TB_PriceOutTaxF.Text))
            {
                int str2 = 0;
                int str1 = Convert.ToInt32(TB_PriceOutTaxF.Text.ToString().Replace(",", ""));
                if (string.IsNullOrWhiteSpace(TB_PriceOutTaxT.Text.ToString()))
                    str2 = 0;
                else
                    str2 = Convert.ToInt32(TB_PriceOutTaxT.Text.ToString().Replace(",", ""));
                if (str1 > str2)
                {
                    bbl.ShowMessage("E116");
                    TB_PriceOutTaxT.Focus();
                    return false;
                }
            }


            return true;

        }
        private bool ErrorCheckApply()
        {
            if (!RequireCheck(new Control[] { TB_Rate }))
                return false;
            return true;
        }
        private void SC_Tanka_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(SC_Tanka.TxtCode.Text))
                {
                    SC_Tanka.ChangeDate = bbl.GetDate();
                    if (!SC_Tanka.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_Tanka.SetFocus(1);
                    }
                }

            }
        }
        private void SC_Brand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(SC_Brand.TxtCode.Text))
                {
                    if (!SC_Brand.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_Brand.SetFocus(1);
                    }

                }
            }
        }
        private void Sc_Segment_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(Sc_Segment.TxtCode.Text))
                {
                    if (!Sc_Segment.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        Sc_Segment.SetFocus(1);
                    }

                }
            }
        }
        public override void FunctionProcess(int Index)
        {
            switch (Index + 1)
            {
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        CleanData();
                        SC_Tanka.SetFocus(1);
                    }
                    break;
                case 3:
                    ChangeMode(EOperationMode.UPDATE);
                    break;
                case 5:
                    ChangeMode(EOperationMode.SHOW);
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    F12();
                    break;
            }
        }
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.SHOW:
                    CleanData();
                    F12Enable = false;
                    SC_Tanka.SetFocus(1);
                    break;
                case EOperationMode.UPDATE:
                    Clear(panel1);
                    SC_Tanka.SetFocus(1);
                    break;
            }
        }
        private void CleanData()
        {
            Clear(panel1);
            GV_Tenzaishohin.DataSource = null;

        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void Sc_Segment_Enter(object sender, EventArgs e)
        {
            Sc_Segment.Value1 = "226";
        }
        private void BT_SelectAll_Click(object sender, EventArgs e)
        {
            if (GV_Tenzaishohin.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GV_Tenzaishohin.Rows)
                {

                    row.Cells["CheckBox"].Value = "1";
                }
            }
        }
        private void BT_DeseletAll_Click(object sender, EventArgs e)
        {
            if (GV_Tenzaishohin.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GV_Tenzaishohin.Rows)
                {
                    row.Cells["CheckBox"].Value = "0";
                }
            }
        }
        private void BT_Display_Click(object sender, EventArgs e)
        {
            F11();
        }
        private DataTable GenerateNo(DataTable dtSelect)
        {

            var dt = new DataTable();
            DataColumn AutoNumberColumn = new DataColumn();

            AutoNumberColumn.ColumnName = "colNo";

            AutoNumberColumn.DataType = typeof(int);

            AutoNumberColumn.AutoIncrement = true;

            AutoNumberColumn.AutoIncrementSeed = 1;

            AutoNumberColumn.AutoIncrementStep = 1;

            dt.Columns.Add(AutoNumberColumn);
            dt.Merge(dtSelect);
            return dt;
        }
        private void F11()
        {
            if (ErrorCheck())
            {
                mTSE = GetTenzikaiShouhinData();
                dtSelect = bl.M_TenzikaiShouhin_Select(mTSE);
                if (dtSelect.Rows.Count > 0)
                {
                    GV_Tenzaishohin.DataSource = GenerateNo(dtSelect);
                }
                else
                {
                    GV_Tenzaishohin.DataSource = null;
                    bl.ShowMessage("E128");
                }
            }
        }
        private void F12()
        {
            if (ErrorCheck())
            {
                if (dtSelect.Rows.Count > 0)
                {
                    mTSE = GetTenzikaiShouhinData();
                    mTSE.dt1 = dtSelect;
                    if (bl.InsertUpdate_TenzikaiHanbaiTankaKakeritu(mTSE))
                    {
                        bbl.ShowMessage("I101");
                        CleanData();
                        SC_Tanka.SetFocus(1);
                    }
                    else
                    {
                        bbl.ShowMessage("S001");
                    }
                }
            }
        }
        private M_TenzikaiShouhin_Entity GetTenzikaiShouhinData()
        {
            mTSE = new M_TenzikaiShouhin_Entity()
            {
                TanKaCD = SC_Tanka.TxtCode.Text,
                BranCDFrom = SC_Brand.TxtCode.Text,
                SegmentCDFrom = Sc_Segment.TxtCode.Text,
                LastYearTerm = CB_Year.SelectedValue.ToString(),
                LastSeason = CB_Season.SelectedValue.ToString(),
                SalePriceOutTaxF = TB_PriceOutTaxF.Text,
                SalePriceOutTaxT = TB_PriceOutTaxT.Text,
                InsertOperator = InOperatorCD,
                PC = InPcID,
                ProcessMode = "",
                ProgramID = InProgramID,
                Key = SC_Tanka.TxtCode.Text + " " + TB_Rate.Text,
            };

            return mTSE;
        }
        private void TB_PriceOutTaxT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_PriceOutTaxF.Text))
                {
                    int str2 = 0;
                    int str1 = Convert.ToInt32(TB_PriceOutTaxF.Text.ToString().Replace(",", ""));
                    if (string.IsNullOrWhiteSpace(TB_PriceOutTaxT.Text.ToString()))
                        str2 = 0;
                    else
                        str2 = Convert.ToInt32(TB_PriceOutTaxT.Text.ToString().Replace(",", ""));
                    if (str1 > str2)
                    {
                        bbl.ShowMessage("E116");
                        TB_PriceOutTaxT.Focus();
                    }
                }
            }
        }
        private void GV_Tenzaishohin_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch
            {
                MessageBox.Show("Enter valid no");
                GV_Tenzaishohin.RefreshEdit();
            }
        }
        private void BT_Apply_Click(object sender, EventArgs e)
        {
            if (ErrorCheck())
            {
                if (ErrorCheckApply())
                {
                    foreach (DataGridViewRow row in GV_Tenzaishohin.Rows)
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                        if (chk.Value == "1")
                        {
                            if (!String.IsNullOrEmpty(TB_Rate.Text))
                            {
                                string itemcd = row.Cells["Rate"].Value.ToString();
                                row.Cells["Rate"].Value = TB_Rate.Text;
                            }
                            else
                            {
                                bl.ShowMessage("E102");
                                TB_Rate.Focus();
                                break;
                            }
                        }
                    }
                }
            }
        }
        private void FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
       
        private void GV_Tenzaishohin_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (GV_Tenzaishohin.Columns[e.ColumnIndex].Name == "Rate")
            {
                string rate = GV_Tenzaishohin.Rows[e.RowIndex].Cells["Rate"].EditedFormattedValue.ToString();
                if (!String.IsNullOrEmpty(rate))
                {
                    if (!rate.Contains("."))
                    {
                        var isNumeric = int.TryParse(rate, out int n);
                        if (isNumeric)
                        {
                            if (rate.Length > 3)
                            {
                                MessageBox.Show("enter valid no");
                                GV_Tenzaishohin.RefreshEdit();
                            }
                        }
                        else
                        {
                            MessageBox.Show("enter valid no");
                            GV_Tenzaishohin.RefreshEdit();
                        }
                    }
                    else
                    {
                        int x = rate.IndexOf('.');
                        int count = rate.Count(f => f == '.');
                        string charre = rate.Remove(x, count);
                        var isNumeric = int.TryParse(charre, out int n);
                        if(isNumeric)
                        {
                            if (count != 1 || x >= 4)
                            {
                                MessageBox.Show("enter valid no");
                                GV_Tenzaishohin.RefreshEdit();
                            }
                        }
                        else
                        {
                            MessageBox.Show("enter valid no");
                            GV_Tenzaishohin.RefreshEdit();
                        }
                       
                    }
                }
            }

        }

        private void GV_Tenzaishohin_Paint(object sender, PaintEventArgs e)
        {
            string[] monthes = { "ブランド", "", "せグメト", "年度", "シーズン", "掛率" };
            for (int j = 2; j < 3;)
            {
                Rectangle r1 = this.GV_Tenzaishohin.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.GV_Tenzaishohin.GetCellDisplayRectangle(j + 1, -1, true).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.GV_Tenzaishohin.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;               
                e.Graphics.DrawString(monthes[j / 3],
               this.GV_Tenzaishohin.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.GV_Tenzaishohin.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 2;
            }

            for (int j = 4; j < 5;)
            {
                Rectangle r1 = this.GV_Tenzaishohin.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.GV_Tenzaishohin.GetCellDisplayRectangle(j + 1, -1, true).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.GV_Tenzaishohin.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(monthes[j / 2],
               this.GV_Tenzaishohin.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.GV_Tenzaishohin.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
            j += 2;
            }
        }

        private void GV_Tenzaishohin_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
    }
}
