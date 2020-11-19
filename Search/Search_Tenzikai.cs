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
    public partial class Search_Tenzikai : FrmSubForm
    {
        M_TenzikaiShouhin_Entity mte;
        MasterTouroku_TenzikaiHanbaiTankaKakeritu_BL mtbl;
        public string TenzikaiName = "";
        public string VendorName = "";
        public string LastYearTerm = "";
        public string LastSeason = "";
        public string VendorCD = "";
        public Search_Tenzikai(string ChangeDate)
        {
            InitializeComponent();
            txtDate.Text = ChangeDate;
            mte = new M_TenzikaiShouhin_Entity();
            mtbl = new MasterTouroku_TenzikaiHanbaiTankaKakeritu_BL();
        }
        private void Search_Tenzikai_Load(object sender, EventArgs e)
        {
            BindCombo();
            SetRequiredField();
            txtDate.Focus();
        }
        public void BindCombo()
        {
            string ymd = bbl.GetDate();
            cbo_Year.Bind(ymd);
            cbo_Season.Bind(ymd);
        }
        private void SetRequiredField()
        {
            cbo_Year.Require(true);
            cbo_Season.Require(true);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            F11();
        }
        private M_TenzikaiShouhin_Entity GetSearchInfo()
        {
            mte = new M_TenzikaiShouhin_Entity()
            {
                RefDate = txtDate.Text,
                TenzikaiName =txtExhibitionName.Text,
                VendorCDFrom=scSupplierCDFrom.TxtCode.Text,
                VendorCDTo=scSupplierCDTo.TxtCode.Text,
                LastYearTerm=cbo_Year.SelectedValue.ToString(),
                LastSeason=cbo_Season.SelectedValue.ToString(),
                NewRStartDate=NRegistrationDateFrom.Text,
                NewREndDate=NRegistrationDateTo.Text,
                LastCStartDate= LModifiedDateFrom.Text,
                LastCEndDate=LModifiedDateTo.Text
            };
            return mte;
        }
        private void F11()
        {
            if (ErrorCheck())
            {
                mte = GetSearchInfo();
                DataTable dt = mtbl.M_SearchTenzikai(mte);
                if(dt.Rows.Count>0)
                {
                    dgvSearch_Tenzikai.DataSource = dt;
                    dgvSearch_Tenzikai.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    dgvSearch_Tenzikai.CurrentRow.Selected = true;
                    dgvSearch_Tenzikai.Enabled = true;
                    dgvSearch_Tenzikai.Focus();
                }
                else
                {
                    mtbl.ShowMessage("E128");
                    dgvSearch_Tenzikai.DataSource = null;
                    txtDate.Focus();
                }
            }
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { cbo_Year, cbo_Season }))
                return false;

            if (!string.IsNullOrEmpty(scSupplierCDFrom.Text) && !string.IsNullOrEmpty(scSupplierCDTo.Text))
            {
                if (string.Compare(scSupplierCDFrom.Text, scSupplierCDTo.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    scSupplierCDTo.Focus();
                }
            }

            if (!string.IsNullOrEmpty(NRegistrationDateFrom.Text) && !string.IsNullOrEmpty(NRegistrationDateTo.Text))
            {
                if (string.Compare(NRegistrationDateFrom.Text, NRegistrationDateTo.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    NRegistrationDateTo.Focus();
                }
            }

            if (!string.IsNullOrEmpty(LModifiedDateFrom.Text) && !string.IsNullOrEmpty(LModifiedDateTo.Text))
            {
                if (string.Compare(LModifiedDateFrom.Text, LModifiedDateTo.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    LModifiedDateTo.Focus();
                }
            }
            if (string.IsNullOrWhiteSpace(txtExhibitionName.Text) && string.IsNullOrWhiteSpace(scSupplierCDFrom.TxtCode.Text) && string.IsNullOrWhiteSpace(scSupplierCDTo.TxtCode.Text) && 
                string.IsNullOrWhiteSpace(cbo_Year.Text) && string.IsNullOrWhiteSpace(cbo_Season.Text) && string.IsNullOrWhiteSpace(NRegistrationDateFrom.Text) && string.IsNullOrWhiteSpace(NRegistrationDateTo.Text) &&
                string.IsNullOrWhiteSpace(LModifiedDateFrom.Text) && string.IsNullOrWhiteSpace(LModifiedDateTo.Text))
            {
                bbl.ShowMessage("E111");
                txtExhibitionName.Focus();
                return false;
            }
            return true;
        }
        public override void FunctionProcess(int index)
        {
            if (index + 1 == 12)
            {
                GetData();
            }
        }
        private void dgvSearch_Tenzikai_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }
        private void GetData()
        {
            if (dgvSearch_Tenzikai.CurrentRow != null && dgvSearch_Tenzikai.CurrentRow.Index >= 0)
            {
                TenzikaiName = dgvSearch_Tenzikai.CurrentRow.Cells["colTenzikaiName"].Value.ToString();
                VendorName = dgvSearch_Tenzikai.CurrentRow.Cells["colVendorName"].Value.ToString();
                LastYearTerm = dgvSearch_Tenzikai.CurrentRow.Cells["colLastYearTerm"].Value.ToString();
                LastSeason = dgvSearch_Tenzikai.CurrentRow.Cells["colSeason"].Value.ToString();
                VendorCD = dgvSearch_Tenzikai.CurrentRow.Cells["colVendorCD"].Value.ToString();
                this.Close();
            }
        }
        private void scBrandCDTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(scSupplierCDFrom.Text) && !string.IsNullOrEmpty(scSupplierCDTo.Text))
                {
                    if (string.Compare(scSupplierCDFrom.Text, scSupplierCDTo.Text) == 1)
                    {
                        bbl.ShowMessage("E104");
                        scSupplierCDTo.Focus();
                    }
                }
            }
        }

        private void NRegistrationDateTo_KeyDown(object sender, KeyEventArgs e)
        {
           if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrEmpty(NRegistrationDateFrom.Text) && !string.IsNullOrEmpty(NRegistrationDateTo.Text))
                {
                    if (string.Compare(NRegistrationDateFrom.Text, NRegistrationDateTo.Text) == 1)
                    {
                        bbl.ShowMessage("E104");
                        NRegistrationDateTo.Focus();
                    }
                }
            }
        }

        private void LModifiedDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrEmpty(LModifiedDateFrom.Text) && !string.IsNullOrEmpty(LModifiedDateTo.Text))
                {
                    if (string.Compare(LModifiedDateFrom.Text, LModifiedDateTo.Text) == 1)
                    {
                        bbl.ShowMessage("E104");
                        LModifiedDateTo.Focus();
                    }
                }
            }
        }
       
        private void Search_Tenzikai_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                F11();
        }
        private void Search_Tenzikai_KeyUp(object sender, KeyEventArgs e)
        {
            if ((ActiveControl is CKM_Controls.CKM_ComboBox cb))
            {
                if (string.IsNullOrEmpty(cb.Text))
                {
                    return;
                }
            }
            MoveNextControl(e);
        }

        private void cbo_Year_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace((sender as CKM_Controls.CKM_ComboBox).Text))
                {
                    bbl.ShowMessage("E102");
                    (sender as CKM_Controls.CKM_ComboBox).Focus();
                    return;
                }
            }
        }
    }
}
