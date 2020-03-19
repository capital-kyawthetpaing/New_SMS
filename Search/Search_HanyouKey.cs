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

namespace Search
{
    public partial class Search_HanyouKey : FrmSubForm
    {
        private const string ProNm = "汎用検索";

        #region"公開プロパティ"
        public string parID = "";
        public string parKey = "";
        public string parChar1 = "";
        #endregion

        MultiPorpose_BL mbl;
        M_MultiPorpose_Entity mme;

        public Search_HanyouKey()
        {
            InitializeComponent();
            F9Visible = false;
            F11Visible = false;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {

                string title = "";
                if (parID.Equals(MultiPorpose_BL.ID_Mail))
                {
                    title = "モール";
                }
                else if (parID.Equals(MultiPorpose_BL.ID_TANI))
                {
                    title = "単位";
                }
                else if (parID.Equals(MultiPorpose_BL.ID_SPORTS))
                {
                    title = "競技";
                }

                HeaderTitleText = title;
                this.Text = title + "検索";
                this.colChar1.HeaderText = title + "名";

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
            mme = GetEntity();
            DataTable dt = mbl.M_MultiPorpose_SelectAll(mme);

            if (dt.Rows.Count == 0)
                return false;

            dgvDetail.DataSource = dt;
            return true;
        }

        private M_MultiPorpose_Entity GetEntity()
        {
            mme = new M_MultiPorpose_Entity();
            mme.ID = parID;
            return mme;
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

            parKey = dgvDetail.CurrentRow.Cells["ColKEY"].Value.ToString();
            parChar1 = dgvDetail.CurrentRow.Cells["colChar1"].Value.ToString();
            EndSec();
        }

        protected override void ExecDisp()
        {
            mbl = new MultiPorpose_BL();
            bool ret= BindGrid();
         
            if (ret)
            {
                dgvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDetail.CurrentRow.Selected = true;
                dgvDetail.Enabled = true;
                dgvDetail.Focus();
                dgvDetail.Select();
            }
            else
            {
                mbl.ShowMessage("E128");
            }
        }

        //private void DgvStore_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    //列ヘッダーかどうか調べる
        //    if (e.ColumnIndex < 0 && e.RowIndex >= 0)
        //    {
        //        //セルを描画する
        //        e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

        //        //行番号を描画する範囲を決定する
        //        //e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
        //        Rectangle indexRect = e.CellBounds;
        //        indexRect.Inflate(-2, -2);
        //        //行番号を描画する
        //        TextRenderer.DrawText(e.Graphics,
        //            (e.RowIndex + 1).ToString(),
        //            e.CellStyle.Font,
        //            indexRect,
        //            e.CellStyle.ForeColor,
        //            TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
        //        //描画が完了したことを知らせる
        //        e.Handled = true;
        //    }

        //}
    }
}
