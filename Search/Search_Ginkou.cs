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
    public partial class FrmSearch_Ginkou : FrmSubForm
    {
        Search_Ginkou_BL sgbl;
        M_Bank_Entity mbe;
        public string BankCD = string.Empty;
        public string BankName = string.Empty;
        public string ChangeDate = string.Empty;
        public FrmSearch_Ginkou(string changeDate)
        {
            InitializeComponent();

            F9Visible = false;
            lblChangeDate.Text = changeDate;

            sgbl = new Search_Ginkou_BL();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void F11()
        {
            if (ErrorCheck())
            {
                mbe = GetSearchInfo();
                DataTable dtBank = sgbl.M_Bank_Search(mbe);
                if(dtBank.Rows.Count >0)
                {
                    GdvGinkou.DataSource = dtBank;
                    GdvGinkou.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GdvGinkou.CurrentRow.Selected = true;
                    GdvGinkou.Enabled = true;
                    GdvGinkou.Focus();
                }
                else
                {
                    GdvGinkou.DataSource = null;
                    sgbl.ShowMessage("E128");
                }
            }
        }

        private bool ErrorCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtBankCDFrom.Text) && !string.IsNullOrWhiteSpace(txtBankCDTo.Text))
            {
                if (string.Compare(txtBankCDFrom.Text, txtBankCDTo.Text) == 1)
                {
                    sgbl.ShowMessage("E106");
                    return false;
                }
                   
            }
            return true;
        }

        private M_Bank_Entity GetSearchInfo()
        {
            mbe = new M_Bank_Entity
            {
                BankCDFrom = txtBankCDFrom.Text,
                BankCDTo = txtBankCDTo.Text,
                ChangeDate = lblChangeDate.Text,
                BankName = txtBankName.Text,
                BankKana = txtBankKana.Text,
                searchType = RdoKijunBi.Checked ? "0" : "1",
                DeleteFlg = "0",
            };

            return mbe;
        }

        public override void FunctionProcess(int index)
        {
            if (index + 1 == 12)
            {
                GetData();
            }
        }

        private void FrmSearch_Ginkou_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                F11();
        }

        private void GdvGinkou_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (GdvGinkou.CurrentRow != null && GdvGinkou.CurrentRow.Index >= 0)
            {
                BankCD = GdvGinkou.CurrentRow.Cells["colBankCD"].Value.ToString();
                BankName = GdvGinkou.CurrentRow.Cells["colBankName"].Value.ToString();
                //ChangeDate = lblChangeDate.Text; 
                ChangeDate=GdvGinkou.CurrentRow.Cells["colChangeDate"].Value.ToString();
                
            }
            this.Close();
        }

        private void FrmSearch_Ginkou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
