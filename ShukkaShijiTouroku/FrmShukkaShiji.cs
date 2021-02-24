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

namespace ShukkaShijiTouroku
{
    public partial class FrmShukkaShiji : FrmSubForm
    {
        private const string ProNm = "出荷指示登録";
        
        public string ChangeDate = string.Empty;
        public string DeliveryPlanNO = string.Empty;
        public string JuchuNO = string.Empty;

        D_DeliveryPlan_Entity dje;
        ShukkaShijiTouroku_BL nnbl;

        public FrmShukkaShiji(string ProId)
        {
            InitializeComponent();
            
            HeaderTitleText = ProNm;
            this.Text = ProId;

            nnbl = new ShukkaShijiTouroku_BL();
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
            dje = new D_DeliveryPlan_Entity
            {
                DeliveryPlanNO = DeliveryPlanNO,
            };
            DataTable dt = nnbl.D_DeliveryPlan_SelectData(dje);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count>0)
            {
                lblJyuchuNo.Text =JuchuNO;
                lblDeliveryName.Text = dt.Rows[0]["DeliveryName"].ToString();
                lblDeliveryAddress1.Text = dt.Rows[0]["DeliveryAddress1"].ToString();// + " " + dt.Rows[0]["DeliveryAddress2"].ToString();
                lblDecidedDeliveryDate.Text = dt.Rows[0]["DecidedDeliveryDate"].ToString();

                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
            }
            else
            {
               nnbl.ShowMessage("E128");
                EndSec();
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                BtnF12Visible = false;
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
