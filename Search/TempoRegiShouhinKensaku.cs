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
        MasterTouroku_Brand_BL mbl = new MasterTouroku_Brand_BL();
        M_SKU_Entity mse = new M_SKU_Entity();
        M_Brand_Entity mbe = new M_Brand_Entity();

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
            mse.SportsCD = txtTanni.Text;
            mse.SKUName = txtSKUName.Text;
            mse.JanCD = txtJanCD.Text;
            mse.MainVendorCD = txtMakerCD.Text;
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
            Search_HanyouKey_Shop frmTanni = new Search_HanyouKey_Shop();
            frmTanni.parID = "202";
            frmTanni.ShowDialog();
            if (!frmTanni.flgCancel)
            {
                txtTanni.Text = frmTanni.parKey;
                lblTanniName.Text = frmTanni.parChar1;
                txtSKUName.Focus();
            }
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
                txtTanni.Focus();
            }
        }

        private void txtTanni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtTanni.Text.ToString()))
                {
                    lblTanniName.Text = "";
                }
                else
                {
                    M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity
                    {
                        ID = MultiPorpose_BL.ID_SPORTS,
                        Key = txtTanni.Text,
                        ChangeDate = string.IsNullOrWhiteSpace(this.ChangeDate) ? bbl.GetDate() : this.ChangeDate
                    };
                    MultiPorpose_BL mbl = new MultiPorpose_BL();
                    DataTable dt = mbl.M_MultiPorpose_Select(mme);
                    if (dt.Rows.Count > 0)
                    {
                        lblTanniName.Text = dt.Rows[0]["Char1"].ToString();
                    }
                    else
                    {
                        //Ｅ１０１
                        bbl.ShowMessage("E101");
                        lblTanniName.Text = "";
                        txtTanni.Focus();
                    }
                }
               
            }
        }

        private void txtMakerCD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtMakerCD.Text))
                {
                    lblMakerName.Text = "";
                }
                else
                {
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = txtMakerCD.Text,
                        ChangeDate = string.IsNullOrWhiteSpace(this.ChangeDate) ? bbl.GetDate() : this.ChangeDate,
                        DeleteFlg = "0"
                    };
                    Vendor_BL vbl = new Vendor_BL();
                    bool ret = vbl.M_Vendor_SelectTop1(mve);
                    if (ret)
                    {
                        lblMakerName.Text = mve.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        lblMakerName.Text = "";
                        txtMakerCD.Focus();
                    }
                }
            }
        }

        private void txtBrandCD_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtBrandCD.Text))
                {
                    DataTable dtbrand = new DataTable();
                    mbe.BrandCD = txtBrandCD.Text;
                    dtbrand = mbl.Brand_Select(mbe);
                    if (dtbrand.Rows.Count == 0)
                    {
                        mbl.ShowMessage("E101");
                        lblBrandName.Text = "";
                        txtBrandCD.Focus();
                    }
                    else
                    {
                        lblBrandName.Text = dtbrand.Rows[0]["BrandName"].ToString();
                    }
                }
                else
                {
                    lblBrandName.Text = "";
                }
            }
                
           
        }

    }
}
