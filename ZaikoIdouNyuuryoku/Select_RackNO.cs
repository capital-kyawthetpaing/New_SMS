using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using Base.Client;

namespace ZaikoIdouNyuuryoku
{
    public partial class Select_RackNO : FrmSubForm
    {
        private const string ProNm = "棚番選択";

        #region"公開プロパティ"
        public string parRackNO = "";
        #endregion

        ZaikoIdouNyuuryoku_BL zbl;
        D_Stock_Entity dse;

        public Select_RackNO(D_Stock_Entity ds)
        {
            InitializeComponent();
            
            HeaderTitleText = ProNm;
            this.Text = ProNm;

            dse = ds;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                lblJanCD.Text = dse.SKUCD;
                lblSKUName.Text = dse.SKUName;
                lblSoukoName.Text = dse.SoukoName;
                zbl = new ZaikoIdouNyuuryoku_BL();

                base.FunctionProcess(FuncDisp - 1);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private bool BindGrid()
        {
            DataTable dt = zbl.D_Stock_SelectRackNO(dse);

            if (dt.Rows.Count == 0)
                return false;

            dgvDetail.DataSource = dt;
            return true;
        }

        private void DgvStore_KeyDown(object sender, KeyEventArgs e)
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
        private void DgvStore_DoubleClick(object sender, EventArgs e)
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

        protected override void ExecSec()
        {
            if (dgvDetail.CurrentRow is null)
                return;

            parRackNO = dgvDetail.CurrentRow.Cells["ColRackNO"].Value.ToString();
            EndSec();
        }

        protected override void ExecDisp()
        {
            bool ret= BindGrid();
         
            if (ret)
            {
                dgvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dgvDetail.CurrentRow.Selected = true;
                dgvDetail.Enabled = true;
                dgvDetail.Focus();
                dgvDetail.Select();
            }
            else
            {
                zbl.ShowMessage("E128");
            }
        }

        private void DgvStore_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //列ヘッダーかどうか調べる
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                //セルを描画する
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                //行番号を描画する範囲を決定する
                //e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);
                //行番号を描画する
                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                //描画が完了したことを知らせる
                e.Handled = true;
            }

        }
    }
}
