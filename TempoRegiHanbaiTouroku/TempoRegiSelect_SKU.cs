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

namespace TempoRegiHanbaiTouroku
{
    public partial class TempoRegiSelect_SKU : Form
    {
        SKU_BL sbl;
        M_SKU_Entity mse;
        DataTable dt;

        #region"公開プロパティ"
        public bool flgCancel = false;
        public string parAdminNO = "";
        public string parJANCD = "";
        public string parSKUCD = "";
        public string parChangeDate = "";
        #endregion

        public TempoRegiSelect_SKU()
        {
            InitializeComponent();
        }

        private void TempoRegiKaiinKensaku_Load(object sender, EventArgs e)
        {
            //InProgramID = "TempoRegiSelect_SKU";

            //string data = InOperatorCD;

            //StartProgram();

            lblJANCD.Text = parJANCD;

            //dgvDetail.RowHeadersVisible = false;
            ShowDetail();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            flgCancel = true;
            this.Close();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            ExecSec();
            this.Close();
        }

        private M_SKU_Entity GetEntity()
        {
            mse = new M_SKU_Entity
            {
                JanCD = parJANCD,
                ChangeDate = parChangeDate
            };

            return mse;
        }
        private void ShowDetail()
        {
            mse = GetEntity();
            sbl = new SKU_BL();

            DataTable dt = sbl.M_SKU_SelectAll(mse);
            if (dt.Rows.Count > 0)
            {
                dgvDetail.DataSource = dt;
                dgvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dgvDetail.CurrentRow.Selected = true;
                dgvDetail.Focus();
            }
            
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
            if (dgvDetail.CurrentRow is null)
                return;

            parAdminNO = dgvDetail.CurrentRow.Cells["ColAdminNO"].Value.ToString();
            parSKUCD = dgvDetail.CurrentRow.Cells["ColSKUCD"].Value.ToString();
            this.Close();
        }

        private void dgvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    if (e.Control == false)
                    {
                        this.ExecSec();
                    }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
    }
}
