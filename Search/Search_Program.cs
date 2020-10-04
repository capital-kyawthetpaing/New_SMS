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

namespace Search
{
    public partial class Search_Program : FrmSubForm
    {
        MasterTouroku_Program_BL mpb;
        M_Program_Entity mpe;
        public string ProgramID =string.Empty;
        public Search_Program(string ProgramID)
        {
            InitializeComponent();
            F9Visible = false;
            mpe = new M_Program_Entity();
            mpb = new MasterTouroku_Program_BL();
            HeaderTitleText = "プログラム";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            F11();
        }
        private void F1()
        {
            this.Close();
        }
        private void F11()
        {
            mpe = GetSearchInfo();
            DataTable dt = new DataTable();
            dt = mpb.M_ProgramSearch(mpe);
            if (dt.Rows.Count > 0)
            {
                dgvSearchProgram.DataSource = dt;
                dgvSearchProgram.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dgvSearchProgram.CurrentRow.Selected = true;
                dgvSearchProgram.Enabled = true;
                dgvSearchProgram.Focus();
            }
            else
            {
                mpb.ShowMessage("E128");
                dgvSearchProgram.DataSource = null;
                txtProgram_ID.Focus();
            }
        }
        private M_Program_Entity GetSearchInfo()
        {
            mpe = new M_Program_Entity()
            {
                Program_ID=txtProgram_ID.Text,
                ProgramName=txtProgramName.Text,
            };
            return mpe;
        }

        private void dgvSearchProgram_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }
        public override void FunctionProcess(int index)
        {
            if (index + 1 == 11)
            {
                F11();
            }
            if (index + 1 == 12)
            {
                GetData();
            }
        }
        private void Search_Program_KeyDown(object sender, KeyEventArgs e)

        {
            if (e.KeyCode == Keys.F11)
                F11();
        }
        private void GetData()
        {
            if (dgvSearchProgram.CurrentRow != null && dgvSearchProgram.CurrentRow.Index >= 0)
            {
                ProgramID = dgvSearchProgram.CurrentRow.Cells["colProgramID"].Value.ToString();
               
            }
            this.Close();
        }

        private void Search_Program_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
