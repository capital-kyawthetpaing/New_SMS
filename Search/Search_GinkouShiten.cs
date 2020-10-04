using Base.Client;
using Entity;
using BL;
using System.Data;
using System.Windows.Forms;

namespace Search
{
    public partial class FrmSearch_GinkouShiten : FrmSubForm
    {
        M_BankShiten_Entity mbse;

        Search_GinkouShiten_BL sgsbl;
        public string BranchCD = string.Empty;
        public string BranchName = string.Empty;
        public string ChangeDate = string.Empty;
        public FrmSearch_GinkouShiten(string changeDate,string bankCD,string bankName)
        {
            InitializeComponent();
            F9Visible = false;
            F11Visible = false;
            lblChangeDate.Text = changeDate;
            LblBankCD.Text = bankCD;
            LblBankName.Text = bankName;

            sgsbl = new Search_GinkouShiten_BL();
        }

        private void BtnDisplay_Click(object sender, System.EventArgs e)
        {
            F11();
        }

        private void F11()
        {
            if (ErrorCheck())
            {
                mbse = GetSearchInfo();
                DataTable dtShiten = sgsbl.M_BankShiten_Search(mbse);
                if (dtShiten.Rows.Count > 0)
                {
                    GvShiten.DataSource = dtShiten;
                    GvShiten.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GvShiten.CurrentRow.Selected = true;
                    GvShiten.Enabled = true;
                    GvShiten.Focus();
                }
                else
                {
                    GvShiten.DataSource = null;
                    sgsbl.ShowMessage("E128");
                }
               
            }
        }

        private bool ErrorCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtBranchCDFrom.Text) && !string.IsNullOrWhiteSpace(txtBranchCDTo.Text))
            {
                if (string.Compare(txtBranchCDFrom.Text, txtBranchCDTo.Text) == 1)
                {
                    sgsbl.ShowMessage("E106");
                    return false;
                }
                   
            }
            return true;
        }

        private M_BankShiten_Entity GetSearchInfo()
        {
            mbse = new M_BankShiten_Entity
            {
                BankCD = LblBankCD.Text,
                BranchCD_From = txtBranchCDFrom.Text,
                BranchCD_To = txtBranchCDTo.Text,
                ChangeDate = lblChangeDate.Text,
                BranchName = txtBranchName.Text,
                BranchKana = txtKanaName.Text,
                searchType = RdoKijunBi.Checked ? "1" : "2",
                DeleteFlg = "0",
            };

            return mbse;
        }

        private void GvShiten_DoubleClick(object sender, System.EventArgs e)
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
            if (GvShiten.CurrentRow != null && GvShiten.CurrentRow.Index >= 0)
            {
                BranchCD = GvShiten.CurrentRow.Cells["colBranchCD"].Value.ToString();
                BranchName = GvShiten.CurrentRow.Cells["colBranchName"].Value.ToString();
                //ChangeDate = lblChangeDate.Text; //GvShiten.CurrentRow.Cells["colChangeDate"].Value.ToString();
                ChangeDate= GvShiten.CurrentRow.Cells["colChangeDate"].Value.ToString();
               
            }
            this.Close();
        }

        private void FrmSearch_GinkouShiten_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                F11();
        }

        private void FrmSearch_GinkouShiten_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void txtBranchCDTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtBranchCDFrom.Text) && !string.IsNullOrWhiteSpace(txtBranchCDTo.Text))
                {
                    if (string.Compare(txtBranchCDFrom.Text, txtBranchCDTo.Text) == 1)
                    {
                        sgsbl.ShowMessage("E106");
                        txtBranchCDFrom.Focus();
                    }

                }
            }
        }
    }
}
