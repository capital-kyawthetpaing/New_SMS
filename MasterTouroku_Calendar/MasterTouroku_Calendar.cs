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
using Search;
using BL;
using Entity;
using CKM_Controls;

namespace MasterTouroku_Calendar
{
    public partial class FrmMasterTouroku_Calendar : FrmMainForm
    {
        MasterTouroku_Calendar_BL mtcbl;
        M_Calendar_Entity mce;
        DataTable dtDay= new DataTable();
        DataTable dtKBN;
        DataTable dtSetting= new DataTable();
        DateTime now;

        #region constructor
        /// <summary>
        /// default
        /// </summary>
        public FrmMasterTouroku_Calendar()
        {
            InitializeComponent();
        }

        #endregion

        #region Form load
        private void FrmMasterTouroku_Calendar_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_Calendar";

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            SetEnableVisible();
            SetRequireField();

            now = DateTime.Now;
            BindGridCalendar(now);

        }

        private void SetEnableVisible()
        {
            GvCalendar.RowHeadersVisible = false;
            ModeVisible = false;
            Btn_F2.Text = string.Empty;
            Btn_F3.Text = string.Empty;
            Btn_F4.Text = string.Empty;
            Btn_F5.Text = string.Empty;
            Btn_F6.Text = string.Empty;
            Btn_F11.Text = string.Empty;

            Btn_F7.Text = "前月(F7)";
            Btn_F8.Text = "次月(F8)";

        }

        private void BindGridCalendar(DateTime now)
        {
            mtcbl = new MasterTouroku_Calendar_BL();
            dtKBN = dtDay = new DataTable();
            lblMonth.Text = txtMonth.Text= now.Date.ToString("yyyy'/'MM'-'dd'T'HH':'mm':'ss.fffffffK").Substring(0, 7);
            dtDay = mtcbl.M_Calendar_Select(txtMonth.Text.ToString() + "/01 00:00:00");
            dtKBN = mtcbl.M_Calendar_SelectDayKBN(txtMonth.Text.ToString() + "/01 00:00:00");
            BindKBN(dtKBN);
            GvCalendar.DataSource = dtDay;

            if (dtDay.Columns.Count-1 < GvCalendar.Columns.Count-1)
            {

                for (int i = GvCalendar.Columns.Count-1; i > dtDay.Columns.Count-1; i--)
                {
                    Label lbl = this.Controls.Find("lbl" + (i - 1), true)[0] as Label;
                    lbl.Text = string.Empty;
                    lbl.BackColor = Color.White;
                    GvCalendar.Columns[i - 1].HeaderText = string.Empty;
                }
            }
            else if(dtDay.Columns.Count-1 == GvCalendar.Columns.Count-1)
            {
                for (int i = 29; i <= dtDay.Columns.Count-1; i++)
                {
                    string strH = GvCalendar.Columns[i-1].HeaderText;
                    GvCalendar.Columns[i-1].HeaderText =i.ToString();
                }

            }


        }

        private void BindKBN(DataTable dtKBN)
        {
            if (dtKBN != null || dtKBN.Rows.Count > 0)
            {
                dtKBN.Columns.Add("DayKBNText", typeof(string));
                for (int i = 0; i < dtKBN.Rows.Count; i++)
                {
                    switch (dtKBN.Rows[i]["DayKBN"].ToString())
                    {
                        case "1": dtKBN.Rows[i]["DayKBNText"] = "日"; break;
                        case "2": dtKBN.Rows[i]["DayKBNText"] = "月"; break;
                        case "3": dtKBN.Rows[i]["DayKBNText"] = "火"; break;
                        case "4": dtKBN.Rows[i]["DayKBNText"] = "水"; break;
                        case "5": dtKBN.Rows[i]["DayKBNText"] = "木"; break;
                        case "6": dtKBN.Rows[i]["DayKBNText"] = "金"; break;
                        case "7": dtKBN.Rows[i]["DayKBNText"] = "土"; break;

                    }

                }

                for (int i = 0; i < dtKBN.Rows.Count; i++)
                {
                    Label lbl = this.Controls.Find("lbl" + i, true)[0] as Label;
                    if (dtKBN.Rows[i]["DayKBNText"].ToString() == "日")
                    {
                        lbl.Text = dtKBN.Rows[i]["DayKBNText"].ToString();
                        lbl.BackColor = Color.LightPink;

                    }
                    else if (dtKBN.Rows[i]["DayKBNText"].ToString() == "土")
                    {
                        lbl.Text = dtKBN.Rows[i]["DayKBNText"].ToString();
                        lbl.BackColor = Color.LightBlue;
                    }
                    else
                    {
                        lbl.Text = dtKBN.Rows[i]["DayKBNText"].ToString();
                        lbl.BackColor = Color.White;
                    }
                }

            }
        }
        #endregion

        #region ButtonClick
        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 7:
                    F7();
                    break;
                case 8:
                    F8();
                    break;
                case 12:
                    //Btn_F12.Focus();
                    F12();
                    break;
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }


        #endregion

        #region ErrorCheck function helper
        private void SetRequireField()
        {
            txtMonth.Require(true);
        }

        #endregion

        #region DisplayData
        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtMonth.Text))
            {
                F8();
            }
        }

        private void btnPreviousMonth_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMonth.Text))
            {
                F7();
            }
        }

        public void F8()
        {
            if (!string.IsNullOrWhiteSpace(txtMonth.Text))
            {
                if (txtMonth.YearMonthCheck())
                {
                    CreateDataTable();
                    mce = GetCalendarEntity();
                    if (mtcbl.M_Calendar_Insert_Update(mce))
                    {
                        now = Convert.ToDateTime(txtMonth.Text.ToString() + "/01 00:00:00");
                        lblMonth.Text = txtMonth.Text = now.AddDays(1).AddMonths(1).AddDays(-1).ToString("yyyy-mm-dd");
                        BindGridCalendar(now.AddMonths(1));
                        txtMonth.Focus();
                    }
                }

            }


        }

        public void F7()
        {
            if (!string.IsNullOrWhiteSpace(txtMonth.Text))
            {
                if (txtMonth.YearMonthCheck())
                {
                    CreateDataTable();
                    mce = GetCalendarEntity();
                    if (mtcbl.M_Calendar_Insert_Update(mce))
                    {

                        now = Convert.ToDateTime(txtMonth.Text.ToString() + "/01 00:00:00");
                        lblMonth.Text = txtMonth.Text = now.AddMonths(-1).ToString("yyyy-mm-dd");
                        BindGridCalendar(now.AddMonths(-1));
                        txtMonth.Focus();
                    }
                }

            }


        }
        #endregion

        #region Insert/Update
        private void F12()
        {
            if (!string.IsNullOrWhiteSpace(txtMonth.Text))
            {
                
                if(txtMonth.YearMonthCheck())
                {
                    if (mtcbl.ShowMessage("Q101") == DialogResult.Yes)
                    {
                        CreateDataTable();
                        mce = GetCalendarEntity();
                        InsertUpdate();

                    }
                    else
                        PreviousCtrl.Focus();
                }
                

            }
            else
            {
                mtcbl.ShowMessage("E102");
                txtMonth.Focus();
            }

        }
        private void CreateDataTable()
        {
            dtSetting = new DataTable();
            dtSetting.Columns.Add("CalendarDate", typeof(string));
            dtSetting.Columns.Add("BankDayOff", typeof(string));
            dtSetting.Columns.Add("DayOff1", typeof(string));
            dtSetting.Columns.Add("DayOff2", typeof(string));
            dtSetting.Columns.Add("DayOff3", typeof(string));
            dtSetting.Columns.Add("DayOff4", typeof(string));
            dtSetting.Columns.Add("DayOff5", typeof(string));
            dtSetting.Columns.Add("DayOff6", typeof(string));
            dtSetting.Columns.Add("DayOff7", typeof(string));
            dtSetting.Columns.Add("DayOff8", typeof(string));
            dtSetting.Columns.Add("DayOff9", typeof(string));
            dtSetting.Columns.Add("DayOff10", typeof(string));

            for (int day = 0; day < dtDay.Columns.Count-1; day++)
            {
                dtSetting.Rows.Add();
                string dd = (day + 1).ToString().Count() == 1 ? "0" + (day + 1).ToString() : (day + 1).ToString();
                dtSetting.Rows[day]["CalendarDate"] = lblMonth.Text.Replace('/', '-').ToString() + "-" + dd.ToString();

                dtSetting.Rows[day]["BankDayOff"] = dtDay.Rows[0][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff1"] = dtDay.Rows[1][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff2"] = dtDay.Rows[2][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff3"] = dtDay.Rows[3][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff4"] = dtDay.Rows[4][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff5"] = dtDay.Rows[5][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff6"] = dtDay.Rows[6][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff7"] = dtDay.Rows[7][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff8"] = dtDay.Rows[8][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff9"] = dtDay.Rows[9][(day + 1).ToString()].ToString();
                dtSetting.Rows[day]["DayOff10"] = dtDay.Rows[10][(day + 1).ToString()].ToString();
            }

        }
        private void InsertUpdate()
        {
            if (mtcbl.M_Calendar_Insert_Update(mce))
            {
                //mtcbl.ShowMessage("S004", mce.ProgramID, "0001");
                mtcbl.ShowMessage("I101","Test","Test1");
                txtMonth.Focus();
            }
            else
                mtcbl.ShowMessage("S001");
        }

        #endregion

        #region Set Update Data
        private M_Calendar_Entity GetCalendarEntity()
        {
            mce = new M_Calendar_Entity
            {
                dtDayOff = dtSetting,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                ProcessMode = "変更",
                Key = txtMonth.Text,
                PC = InPcID,
            };
            return mce;

        }

        #endregion

        #region keydown event
        private void txtMonth_KeyDown(object sender, KeyEventArgs e)
       {
            if(e.KeyCode == Keys.Enter)
            {
                if(!string.IsNullOrWhiteSpace(txtMonth.Text))
                {
                    now =Convert.ToDateTime(txtMonth.Text.ToString() + "-01 00:00:00");
                    BindGridCalendar(now);
                }
            }
        }

        #endregion

        private void GvCalendar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if ((sender as DataGridView).CurrentCell is DataGridViewCheckBoxCell)
                {
                    if (GvCalendar.Rows[e.RowIndex].Cells["colFlag"].Value.ToString() == "0")
                    {
                        DataGridViewCheckBoxCell chk1 = GvCalendar.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                        GvCalendar.CancelEdit();

                    }

                }

            }
        }

        private void GvCalendar_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if ((sender as DataGridView).CurrentCell is DataGridViewCheckBoxCell)
                {
                    if (GvCalendar.Rows[e.RowIndex].Cells["colFlag"].Value.ToString() == "0")
                    {
                        DataGridViewCheckBoxCell chk1 = GvCalendar.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                        GvCalendar.CancelEdit();

                    }

                }

            }
        }

        private void FrmMasterTouroku_Calendar_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
