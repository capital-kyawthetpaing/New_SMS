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

namespace Search
{
    public partial class FrmSearch_Souko : FrmSubForm
    {
        M_Souko_Entity mse;
        Search_Souko_BL ssbl;
        public string SoukoCD = string.Empty;
        public string SoukoName = string.Empty;
        public string ChangeDate = string.Empty;
        public FrmSearch_Souko(string changeDate)
        {
            InitializeComponent();

            F9Visible = false;
            F11Visible = false;
            lblChangeDate.Text = changeDate;

            CboStoreCD.Bind(changeDate,"2");
            CboSoukoType.Bind(changeDate);

            ssbl = new Search_Souko_BL();
        }

        private void BtnF11_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void F11()
        {
            if (ErrorCheck())
            {
                mse = GetSearchInfo();
                DataTable dtSouko = ssbl.M_Souko_Search(mse);
                if(dtSouko.Rows.Count >0)
                {
                    GvSouko.DataSource = dtSouko;
                    GvSouko.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GvSouko.CurrentRow.Selected = true;
                    GvSouko.Enabled = true;
                    GvSouko.Focus();
                }
                else
                {
                    GvSouko.DataSource = null;
                    ssbl.ShowMessage("E128");

                }
            }
        }

        private void FrmSearch_Souko_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                F11();
        }

        private bool ErrorCheck()
        {
            if(!string.IsNullOrWhiteSpace(TxtSoukoCDFrom.Text) && !string.IsNullOrWhiteSpace(TxtSoukoCDTo.Text))
            {
                if (string.Compare(TxtSoukoCDFrom.Text, TxtSoukoCDTo.Text) == 1)
                {
                    ssbl.ShowMessage("E106");
                    TxtSoukoCDFrom.Focus();
                    return false;
                }
                    
            }
            return true;
        }

        private M_Souko_Entity GetSearchInfo()
        {
            mse = new M_Souko_Entity
            {
                SoukoCDFrom = TxtSoukoCDFrom.Text,
                SoukoCDTo = TxtSoukoCDTo.Text,
                ChangeDate = lblChangeDate.Text,
                SoukoName = TxtSoukoName.Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
                SoukoType = CboSoukoType.SelectedValue.Equals("-1") ? string.Empty : CboSoukoType.SelectedValue.ToString(),
                DeleteFlg = "0",
                searchType = RdoKijunBi.Checked ? "1" : "2",
            };
            
            return mse;
        }

        private void GvSouko_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }

        public override void FunctionProcess(int index)
        {
            if (index + 1 == 12)
            {
                GetData();
            }
        }

        private void GetData()
        {
            if(GvSouko.CurrentRow != null &&  GvSouko.CurrentRow.Index >= 0)
            { 
                SoukoCD = GvSouko.CurrentRow.Cells["colSoukoCD"].Value.ToString();
                SoukoName = GvSouko.CurrentRow.Cells["colSoukoName"].Value.ToString();
                ChangeDate = GvSouko.CurrentRow.Cells["ColChangeDate"].Value.ToString();
                this.Close();
            }
        }

        private void GvSouko_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void FrmSearch_Souko_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void TxtSoukoCDTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(TxtSoukoCDFrom.Text) && !string.IsNullOrWhiteSpace(TxtSoukoCDTo.Text))
                {
                    if (string.Compare(TxtSoukoCDFrom.Text, TxtSoukoCDTo.Text) == 1)
                    {
                        ssbl.ShowMessage("E106");
                        TxtSoukoCDFrom.Focus();
                    }
                }
            }
        }
    }
}
