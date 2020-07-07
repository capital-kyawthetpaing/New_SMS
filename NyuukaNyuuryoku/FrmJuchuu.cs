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
    public partial class FrmJuchuu : FrmSubForm
    {
        private const string ProNm = "受注照会";

        public string OperatorCD = string.Empty;
        public string ChangeDate = string.Empty;
        public string JuchuuNO = string.Empty;
        public string CustomerName = string.Empty;
        
        D_Juchuu_Entity dje;
        NyuukaNyuuryoku_BL nnbl;

        public FrmJuchuu()
        {
            InitializeComponent();
            
            HeaderTitleText = ProNm;
            this.Text = ProNm;

            nnbl = new NyuukaNyuuryoku_BL();
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
            DataTable dt = nnbl.D_Juchuu_SelectData_ForNyuuka(dje);
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
               nnbl.ShowMessage("E128");
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                F9Visible = false;
                F11Visible = false;
                F12Visible = false;
                lblJyuchuNo.Text = JuchuuNO;
                lblCustomer.Text = CustomerName;
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
