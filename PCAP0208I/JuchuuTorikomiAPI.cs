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
using Entity;
using BL;

namespace JuchuuTorikomiAPI
{
    public partial class frmJuchuuTorikomiAPI : FrmMainForm
    {
        JuchuuTorikomiAPI_BL pcapbl;
        D_APIRireki_Entity mre;
        private int showmode = 0;
        private int ApiKey = 0;
        private int state = 0;
        static string ErrChkFlg = string.Empty;
        public frmJuchuuTorikomiAPI()
        {
            InitializeComponent();
        }

        private void frmPCAP0208I_Load(object sender, EventArgs e)
        {
            pcapbl = new JuchuuTorikomiAPI_BL();
            InProgramID = "JuchuuTorikomiAPI";
            SetFunctionLabel(EProMode.JUCHUTORIKOMIAPI);
            StartProgram();
            Control();
            F12Enable = false;
            dgvJuchuuTorikomiAPI.AutoGenerateColumns = false;
            dgvJuchuuTorikomiAPI.DisabledColumn("colTorikomiNichiji,colMallStore,colTorikomiOpreator,colTorikomikensu,colErrorCount,colHoryuTaisho,colNayosemachi");
        }
        private void Control()
        {
            M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity
            {
                ID = "302",
                Key = "1"
            };

            String num1 = pcapbl.M_Multipurpose_Num1_Select(mme);
            if (num1 == "1")
            {
                lblKakuteiMode.Text = "処理実行中";
                lblKakuteiMode.BackColor = Color.FromArgb(0, 176, 240);
            }
            else
            {
                lblKakuteiMode.Text = "処理停止中";
                lblKakuteiMode.BackColor = Color.Yellow;
            }
            DataTable dt = pcapbl.D_APIControl_Select();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                 switch (dt.Rows[i]["APIKey"].ToString())
                {
                    case "1":

                        if (dt.Rows[i]["State"].ToString()=="1")
                        {
                            CheckBox1.Checked = true;
                        }  
                        break;
                    case "2":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox2.Checked = true;
                        }
                        break;
                    case "3":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox3.Checked = true;
                        }
                        break;
                    case "4":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox4.Checked = true;
                        }
                        break;
                    case "5":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox5.Checked = true;
                        }
                        break;
                    case "6":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox6.Checked = true;
                        }
                        break;
                    case "7":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox7.Checked = true;
                        }
                        break;
                    case "8":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox8.Checked = true;
                        }
                        break;
                    case "9":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox9.Checked = true;
                        }
                        break;
                    case "10":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox10.Checked = true;
                        }
                        break;
                    case "11":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox11.Checked = true;
                        }
                        break;
                    case "12":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox12.Checked = true;
                        }
                        break;
                    case "13":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox13.Checked = true;
                        }
                        break;
                    case "21":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox21.Checked = true;
                        }
                        break;
                    case "22":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox22.Checked = true;
                        }
                        break;
                    case "23":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox23.Checked = true;
                        }
                        break;
                    case "24":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox24.Checked = true;
                        }
                        break;
                    case "25":
                        if (dt.Rows[i]["State"].ToString() == "1")
                        {
                            CheckBox25.Checked = true;
                        }
                        break;
                }
            }
        }
        private D_APIRireki_Entity GetPCAP0208IEnity()
        {
            mre = new D_APIRireki_Entity
            {
                ShowMode = showmode.ToString(),
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID,
                Key = showmode.ToString(),
                APIKey=ApiKey.ToString(),
                State=state.ToString()
            };
            return mre;
        }
        public override void FunctionProcess(int index)
        {

            switch (index + 1)
            {
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
            dgvJuchuuTorikomiAPI.DisabledColumn("colTorikomiNichiji,colMallStore,colTorikomiOpreator,colTorikomikensu,colErrorCount,colHoryuTaisho,colNayosemachi");
            //Cursor.Current = Cursors.WaitCursor;
            DataTable dt = new DataTable();
            dt = pcapbl.JuchuuTorikomiAPI_Grid_Select();
            if (dt.Rows.Count > 0)
            {
                dgvJuchuuTorikomiAPI.DataSource = dt;
            }
            else
            {
                MessageBox.Show("There is no data to show!","Information",MessageBoxButtons.OK,MessageBoxIcon.Information); 
            }
            
            //dgvPCAP0208I.DataSource = dt;
            //Cursor.Current = Cursors.Default;
            btnF11Show.Focus();

        }
        private void F12()
        {

        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            lblKakuteiMode.Text = "処理実行中";
            lblKakuteiMode.BackColor = Color.FromArgb(0, 176, 240);
            showmode = 1;
            mre = GetPCAP0208IEnity();
            pcapbl.JuchuuTorikomiAPI_Insert_Update(mre);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lblKakuteiMode.Text = "処理停止中";
            lblKakuteiMode.BackColor = Color.FromArgb(255,255 , 0);
            showmode = 0;
            mre = GetPCAP0208IEnity();
            pcapbl.JuchuuTorikomiAPI_Insert_Update(mre);
        }
        private void InsertUpdate()
        {
            if (pcapbl.JuchuuTorikomiAPI_Insert_Update(mre))
            {
                pcapbl.ShowMessage("I001");
            }
            else
            {
                pcapbl.ShowMessage("S001");
            }
        }
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            CheckBox1.Checked = true;
            CheckBox2.Checked = true;
            CheckBox7.Checked = true;
            CheckBox11.Checked = true;
            CheckBox12.Checked = true;
            CheckBox8.Checked = true;
            CheckBox3.Checked = true;
            CheckBox13.Checked = true;
            CheckBox4.Checked = true;
            CheckBox9.Checked = true;
            CheckBox5.Checked = true;
            CheckBox10.Checked = true;
            CheckBox6.Checked = true;
            CheckBox21.Checked = true;
            CheckBox22.Checked = true;
            CheckBox23.Checked = true;
            CheckBox24.Checked = true;
            CheckBox25.Checked = true;
        }

        private void btnAllCancel_Click(object sender, EventArgs e)
        {
            CheckBox1.Checked = false;
            CheckBox2.Checked = false;
            CheckBox7.Checked = false;
            CheckBox11.Checked = false;
            CheckBox12.Checked = false;
            CheckBox8.Checked = false;
            CheckBox3.Checked = false;
            CheckBox13.Checked = false;
            CheckBox4.Checked = false;
            CheckBox9.Checked = false;
            CheckBox5.Checked = false;
            CheckBox10.Checked = false;
            CheckBox6.Checked = false;
            CheckBox21.Checked = false;
            CheckBox22.Checked = false;
            CheckBox23.Checked = false;
            CheckBox24.Checked = false;
            CheckBox25.Checked = false;
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
             ApiKey = 1;
            if (CheckBox1.Checked == true)
            {
                lblStop1.Text = "STOP";
                lblStop1.ForeColor = Color.Red;
                state = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop1.Text = "";
                state = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 2;
            if (CheckBox2.Checked == true)
            {
                lblStop2.Text = "STOP";
                lblStop2.ForeColor = Color.Red;
                state = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop2.Text = "";
                state = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }

        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 3;
            if (CheckBox3.Checked == true)
            {
                lblStop3.Text = "STOP";
                lblStop3.ForeColor = Color.Red;
                state = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop3.Text = "";
                state = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 4;
            if (CheckBox4.Checked == true)
            {
                lblStop4.Text = "STOP";
                lblStop4.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop4.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }

        private void CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 5;
            if (CheckBox5.Checked == true)
            {
                lblStop5.Text = "STOP";
                lblStop5.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop5.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }

        }

        private void CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 6;
            if (CheckBox6.Checked == true)
            {
                lblStop6.Text = "STOP";
                lblStop6.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop6.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }

        private void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 7;
            if (CheckBox7.Checked == true)
            {
                lblStop7.Text = "STOP";
                lblStop7.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop7.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }

        private void CheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 8;
            if (CheckBox8.Checked == true)
            {
                lblStop8.Text = "STOP";
                lblStop8.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop8.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }

        private void CheckBox9_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 9;
            if (CheckBox9.Checked == true)
            {
                lblStop9.Text = "STOP";
                lblStop9.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop9.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }

        private void CheckBox10_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 10;
            if (CheckBox10.Checked == true)
            {
                lblStop10.Text = "STOP";
                lblStop10.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop10.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }

        private void CheckBox11_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 11;
            if (CheckBox11.Checked == true)
            {
                lblStop11.Text = "STOP";
                lblStop11.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop11.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }

        private void CheckBox12_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 12;
            if (CheckBox12.Checked == true)
            {
                lblStop12.Text = "STOP";
                lblStop12.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop12.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }

        private void CheckBox13_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 13;
            if (CheckBox13.Checked == true)
            {
                lblStop13.Text = "STOP";
                lblStop13.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop13.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
        }


        private void CheckBox21_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 21;
            if (CheckBox21.Checked == true)
            {
                lblStop21.Text = "STOP";
                lblStop21.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop21.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }

        }

        private void CheckBox22_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 22;
            if (CheckBox22.Checked == true)
            {
                lblStop22.Text = "STOP";
                lblStop22.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop22.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }

        }

        private void CheckBox23_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 23;
            if (CheckBox23.Checked == true)
            {
                lblStop23.Text = "STOP";
                lblStop23.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop23.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }

        }

        private void CheckBox24_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 24;
            if (CheckBox24.Checked == true)
            {
                lblStop24.Text = "STOP";
                lblStop24.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop24.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }

        }

        private void CheckBox25_CheckedChanged(object sender, EventArgs e)
        {
            ApiKey = 25;
            if (CheckBox25.Checked == true)
            {
                lblStop25.Text = "STOP";
                lblStop25.ForeColor = Color.Red;
                showmode = 1;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }
            else
            {
                lblStop25.Text = "";
                showmode = 0;
                mre = GetPCAP0208IEnity();
                pcapbl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
            }

        }

        private void btnF11Show_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }
        protected void Maintained_CheckClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if ((sender as DataGridView).CurrentCell is DataGridViewCheckBoxCell)
                {
                    
                    ErrChkFlg = "";

                        foreach (DataGridViewRow row1 in dgvJuchuuTorikomiAPI.Rows)
                        {
                            DataGridViewCheckBoxCell chkResult = row1.Cells["chkResult"] as DataGridViewCheckBoxCell;
                            DataGridViewCheckBoxCell chkError = row1.Cells["chkError"] as DataGridViewCheckBoxCell;
                            if (dgvJuchuuTorikomiAPI.CurrentCell.ColumnIndex == dgvJuchuuTorikomiAPI.Columns["chkResult"].Index)
                            {
                                if (Convert.ToBoolean(dgvJuchuuTorikomiAPI.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue) == false && row1 == dgvJuchuuTorikomiAPI.CurrentRow)
                                {
                                    chkResult.Value = chkResult.TrueValue;
                                    chkError.Value = chkError.FalseValue;
                                    //dgvPCAP0208I.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                                    F12Enable = true;
                                }
                                else
                                {
                                    chkResult.Value = chkResult.FalseValue;
                                    chkError.Value = chkError.FalseValue;
                                    row1.DefaultCellStyle.BackColor = Color.White;
                                    F12Enable = false;
                            }
                            }
                            else if (dgvJuchuuTorikomiAPI.CurrentCell.ColumnIndex == dgvJuchuuTorikomiAPI.Columns["chkError"].Index)
                            {
                                if (Convert.ToBoolean(dgvJuchuuTorikomiAPI.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue) == false && row1 == dgvJuchuuTorikomiAPI.CurrentRow)
                                {
                                    chkResult.Value = chkResult.FalseValue;
                                    chkError.Value = chkError.TrueValue;
                                    //dgvPCAP0208I.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                                    F12Enable = true;
                                }
                                else
                                {
                                    chkResult.Value = chkResult.FalseValue;
                                    chkError.Value = chkError.FalseValue;
                                    row1.DefaultCellStyle.BackColor = Color.White;
                                    F12Enable = false;
                                }
                            }
                            else
                            {
                                chkResult.Value = chkResult.FalseValue;
                                chkError.Value = chkError.FalseValue;
                                row1.DefaultCellStyle.BackColor = Color.White;
                            }
                        }

                }
            }

        }

        private void dgvPCA0208I_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Maintained_CheckClick(sender, e);

        }

        private void dgvPCA0208I_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvPCA0208I_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)//must not header row
            {
                if ((sender as DataGridView).CurrentCell is DataGridViewCheckBoxCell)
                {
                    (sender as DataGridView).Cursor = Cursors.Arrow;
                }
            }
        }

        private void dgvPCA0208I_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Maintained_CheckClick(sender, e);
        }
    }
}
