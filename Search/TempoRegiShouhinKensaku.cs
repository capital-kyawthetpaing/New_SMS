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
    public partial class TempoRegiShouhinKensaku : ShopBaseForm
    {
        SKU_BL skuBL = new SKU_BL();
        M_SKU_Entity mse = new M_SKU_Entity();
        DataTable dt;
        public string AdminNO = "";
        public string JANCD = "";
        public string SKUName = "";
        public string Color = "";
        public string Size = "";

        public TempoRegiShouhinKensaku(string Operator)
        {
            InOperatorCD = Operator;
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            //起動時処理
            StartProgramForForm();

            txtBrandCD.Text = "";
            lblBrandName.Text = "";
            txtTanni.Text = "";
            lblTanniName.Text = "";
            txtMakerCD.Text = "";
            lblMakerName.Text = "";
            txtSKUName.Text = "";
            txtJanCD.Text = "";
            txtBrandCD.Focus();

            ShowCloseMessage = false; //added by ETZ 2020-06-22
            //dgvKaniiKensaku.RowHeadersVisible = false;
        }

        protected override void EndSec()
        {
            this.Close();
        }

        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                    ExecSec();
                    break;
            }
        }

        private void ShowDetail()
        {
            mse = GetEntity();
            {
                dt = new DataTable();
                dt = skuBL.M_SKU_SelectAllForTempoRegiShohin(mse);
                dgvDetail.DataSource = dt;
                if (dt.Rows.Count > 0)
                {
                    //dgvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    dgvDetail.CurrentRow.Selected = true;
                    dgvDetail.Focus();
                }
                else
                {
                    skuBL.ShowMessage("E128");
                }
            }
        }

        private M_SKU_Entity GetEntity()
        {
            mse = new M_SKU_Entity();
            mse.BrandCD = txtBrandCD.Text;
            mse.SKUName = txtSKUName.Text;
            mse.JanCD = txtJanCD.Text;
            mse.ChangeDate = skuBL.GetDate();

            return mse;
        }

        private void dgvKaniiKensaku_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ExecSec();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
       
        private void ExecSec()
        {
            if (dgvDetail.CurrentRow != null && dgvDetail.CurrentRow.Index >= 0)
            {
                AdminNO = dgvDetail.CurrentRow.Cells["colAdminNO"].Value.ToString();
                SKUName = dgvDetail.CurrentRow.Cells["colSKUName"].Value.ToString();    //added by ETZ 2020-06-22
                JANCD = dgvDetail.CurrentRow.Cells["colJANCD"].Value.ToString();
                Color = dgvDetail.CurrentRow.Cells["colColorName"].Value.ToString();    //added by ETZ 2020-07-21
                Size = dgvDetail.CurrentRow.Cells["colSizeName"].Value.ToString();      //added by ETZ 2020-07-21
            }
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            //ブランド名、商品名、JANCDのすべての入力が無ければエラーにする
            //エラーの場合、カーソルはブランド名に。Ｅ１１１
            if (string.IsNullOrWhiteSpace(txtBrandCD.Text) && string.IsNullOrWhiteSpace(txtSKUName.Text) && string.IsNullOrWhiteSpace(txtJanCD.Text))
            {
                bbl.ShowMessage("E111");
                txtBrandCD.Focus();
                return;
            }

            ShowDetail();
        }

        private void TempoRegiKaiinKensaku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        private void txtJanCD_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {

                    btnShow_Click(sender, new EventArgs());

                    if (dgvDetail.CurrentRow != null && dgvDetail.CurrentRow.Index >= 0)
                    {
                        btnShow.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        
        private void btnTanni_Click(object sender, EventArgs e)
        {

        }

        private void btnMaker_Click(object sender, EventArgs e)
        {
            Search_Vendor_Shop frmVendor = new Search_Vendor_Shop(bbl.GetDate(), "1");
            //Search_Vendor frmVendor = new Search_Vendor(bbl.GetDate(), "1");
            frmVendor.ShowDialog();
            if (!frmVendor.flgCancel)
            {
                txtMakerCD.Text = frmVendor.VendorCD;
                lblMakerName.Text = frmVendor.VendorName;
            }
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            Search_Brand_Shop frmbrand = new Search_Brand_Shop();
            frmbrand.ShowDialog();
            if (!frmbrand.flgCancel)
            {
                txtBrandCD.Text = frmbrand.parBrandCD;
                lblBrandName.Text = frmbrand.parBrandName;
            }
        }

        private void txtTanni_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtMakerCD_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtBrandCD_KeyDown(object sender, KeyEventArgs e)
        {

        }

    }
}
