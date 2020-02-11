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
            txtSKUName.Text = "";
            txtJanCD.Text = "";
            txtBrandCD.Focus();

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
                if (dt.Rows.Count > 0)
                {
                    dgvDetail.DataSource = dt;
                    dgvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    dgvDetail.CurrentRow.Selected = true;
                    //dgvDetail.Focus();
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
            AdminNO = dgvDetail.CurrentRow.Cells["colAdminNO"].Value.ToString();
            JANCD = dgvDetail.CurrentRow.Cells["colJANCD"].Value.ToString();
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ShowDetail();
        }

        private void TempoRegiKaiinKensaku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
