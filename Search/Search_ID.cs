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

namespace Search
{
    public partial class Search_ID : FrmSubForm
    {       
        Search_Hanyou_BL shbl;
        M_Hanyou_Entity mhe;

        public string ID = string.Empty;
        public string IDName = string.Empty;

        public Search_ID()
        {
            InitializeComponent();
            F9Visible = false;
            shbl = new Search_Hanyou_BL();
        }

        private void txtID2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                F11();
            }
        }

        private void F11Show_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void F11()
        {
            if(ErrorCheck())
            {
                mhe = GetData();
                DataTable dtHanyou = new DataTable();
                dtHanyou = shbl.M_Hanyou_IDSearch(mhe);
                if(dtHanyou.Rows.Count > 0)
                {
                    GvID.DataSource = dtHanyou;
                    GvID.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GvID.CurrentRow.Selected = true;
                    GvID.Enabled = true;
                    GvID.Focus();
                }
                else
                {
                    shbl.ShowMessage("E128");
                    GvID.DataSource = null;
                    txtID1.Focus();
                }
               
            }
            else
            {
                GvID.DataSource = null;
            }
        }

        private bool ErrorCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtID2.Text))
            {
                int result = txtID1.Text.CompareTo(txtID2.Text);            
                if (result > 0)
                {
                    shbl.ShowMessage("E106");
                    txtID2.Focus();
                    return false;
                }
            }
            return true;
        }

        private M_Hanyou_Entity GetData()
        {
            mhe = new M_Hanyou_Entity
            {
                IDFrom = txtID1.Text,
                IDTo = txtID2.Text,
            };

            return mhe;
        }
       
        public override void FunctionProcess(int Index)
        {
            if(Index + 1 == 12)
            {
                SendData();
            }
            else if(Index + 1 == 11)
            {
                F11();
            }
        }

        private void SendData()
        {
            if(GvID.CurrentRow != null && GvID .CurrentRow .Index >= 0)
            {
                ID = GvID.CurrentRow.Cells["colID"].Value.ToString();
                IDName = GvID.CurrentRow.Cells["colIDName"].Value.ToString();
                
            }
            this.Close();
        }

        private void GvID_DoubleClick(object sender, EventArgs e)
        {
            SendData();
        }

        private void Search_ID_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

    }
}
