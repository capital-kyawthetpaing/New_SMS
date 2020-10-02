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
    public partial class Search_Key : FrmSubForm
    {
        M_Hanyou_Entity mhe;
        Search_Hanyou_BL shbl;

        public string KeyCode = string.Empty;
        public string Char1 = string.Empty;
        public string Char2 = string.Empty; //218:補助科目CD
        public string Char3 = string.Empty; //218:補助科目名
        public string ID = string.Empty;
        public string IDName = string.Empty;

        public Search_Key(string ID,string IDName)
        {          
            InitializeComponent();

            F9Visible = false;
            shbl = new Search_Hanyou_BL();

            lblID.Text = ID;
            lblName.Text = IDName;
        }

        public Search_Key(string ID, string IDName,string Char1)
        {
            InitializeComponent();

            F9Visible = false;
            shbl = new Search_Hanyou_BL();

            lblID.Text = ID;
            lblName.Text = IDName;
            txtKey1.Text = Char1;
        }

        private void txtKey2_KeyDown(object sender, KeyEventArgs e)
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
            if (ErrorCheck())
            {
                mhe = GetData();
                DataTable dtHanyou = new DataTable();
                dtHanyou = shbl.M_Hanyou_KeySearch(mhe);
                if (dtHanyou.Rows.Count > 0)
                {
                    GvKey.DataSource = dtHanyou;
                    GvKey.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GvKey.CurrentRow.Selected = true;
                    GvKey.Enabled = true;
                    GvKey.Focus();
                }
                else
                {
                    shbl.ShowMessage("E128");
                    GvKey.DataSource = null;
                    txtKey1.Focus();
                }               
            }
            else
            {
                GvKey.DataSource = null;
            }
        }

        private bool ErrorCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtKey2.Text))
            {
                int result = txtKey1.Text.CompareTo(txtKey2.Text);
                if (result > 0)  
                {
                    shbl.ShowMessage("E106");
                    txtKey2.Focus();
                    return false;
                }
            }
            return true;
        }

        private M_Hanyou_Entity GetData()
        {
            mhe = new M_Hanyou_Entity
            {
                ID = lblID.Text,
                KeyFrom = txtKey1.Text,
                KeyTo = txtKey2.Text,
            };
            return mhe;
        }

        private void GvKey_DoubleClick(object sender, EventArgs e)
        {
            SendData();
        }

        private void SendData()
        {
            if (GvKey .CurrentRow != null && GvKey.CurrentRow.Index >= 0)
            {
                KeyCode = GvKey.CurrentRow.Cells["colKey"].Value.ToString();
                if (lblID.Text == "217")
                    Char1 = GvKey.CurrentRow.Cells["colChar1"].Value.ToString();
                if (lblID.Text == "218")
                {
                    Char1 = GvKey.CurrentRow.Cells["colChar1"].Value.ToString();
                    Char2 = GvKey.CurrentRow.Cells["colChar2"].Value.ToString();
                    Char3 = GvKey.CurrentRow.Cells["colText3"].Value.ToString();
                }
                this.Close();
            }
        }

        public override void FunctionProcess(int Index)
        {
            if (Index + 1 == 12)
            {
                SendData();
            }
            else if(Index + 1 == 11)
            {
                F11();
            }
        }

        private void Search_Key_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
