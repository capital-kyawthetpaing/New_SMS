using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;

namespace WebJuchuuKakunin
{
    public partial class FrmHikiate : FrmSubForm
    {
        private const string ProNm = "WEB受注確認";

        public string OperatorCD = string.Empty;
        public string ChangeDate = string.Empty;
        public string JuchuuNO = string.Empty;
        public string JanCD = string.Empty;
        public string SKUName = string.Empty;
        public string BrandName = string.Empty;
        public string Suryo = string.Empty;

        D_Juchuu_Entity dje;
        WebJuchuuKakunin_BL wjbl;

        public FrmHikiate()
        {
            InitializeComponent();
            
            HeaderTitleText = ProNm;
            this.Text = ProNm;

            wjbl = new WebJuchuuKakunin_BL();
        }

        private void DgvDetail_KeyDown(object sender, KeyEventArgs e)
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

        private void GvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.ExecSec();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        protected override void ExecDisp()
        {
            dje = new D_Juchuu_Entity
            {
                JuchuuNO = JuchuuNO,
            };
            DataTable dt = wjbl.D_Juchuu_SelectForWebHikiate(dje);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count>0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
            }
            else
            {
               wjbl.ShowMessage("E128");
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                F9Visible = false;
                F11Visible = false;
                F12Visible = false;
                lblJanCD.Text = JanCD;
                lblSkuName.Text = SKUName;
                lblBrandName.Text = BrandName;
                lblSuryo.Text = Suryo;
                ExecDisp();
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
