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
    public partial class Search_Store : FrmSubForm
    {
        private const string ProNm = "店舗ストア検索";

        private enum EIndex : int
        {
            StoreFrom,
            StoreTo,
            StoreName
        }

        #region"公開プロパティ"
        public string parStoreCD = "";
        public string parChangeDate = "";
        //KTP 2019-05-29 to Return StoreName
        public string parStoreName = "";
        #endregion

        private Control[] detailControls;

        Store_BL mbl;
        M_Store_Entity mse;

        public Search_Store()
        {
            InitializeComponent();

            InitialControlArray();
            F9Visible = false;

            HeaderTitleText = "店舗ストア";
            this.Text = ProNm;
            BtnF9Text = "";

        }

        public Search_Store(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = "店舗ストア";
            lblChangeDate.Text = changeDate;

            this.Text = ProNm;
            BtnF9Text = "";

        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2, ckM_TextBox3 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
            }
        }
        /// <summary>
        /// 画面クリア
        /// </summary>
        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
                ctl.Text = "";

            //初期値設定
            if (DateTime.TryParse(parChangeDate, out DateTime dt))
                lblChangeDate.Text = parChangeDate;
            else
                lblChangeDate.Text = mbl.GetDate();

            radioButton1.Checked = true;
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                Scr_Clr();

                radioButton1.Focus();
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
            mse = GetEntity();
            DataTable dtStore = mbl.M_Store_SelectAll(mse);

            if (dtStore.Rows.Count == 0)
                return false;

            dgvDetail.DataSource = dtStore;
            return true;
        }

        private M_Store_Entity GetEntity()
        {
            mse = new M_Store_Entity();
            mse.DisplayKbn = radioButton1.Checked ? "0" : "1";
            mse.ChangeDate = lblChangeDate.Text;
            mse.StoreCDFrom = detailControls[(int)EIndex.StoreFrom].Text;

            if (detailControls[(int)EIndex.StoreTo].Text == "")
                mse.StoreCDTo = "ZZZZ";
            else
                mse.StoreCDTo = detailControls[(int)EIndex.StoreTo].Text;

            mse.StoreName = detailControls[(int)EIndex.StoreName].Text;
            mse.StoreKBN1 = checkBox1.Checked ? "1" : "0";
            mse.StoreKBN2 = checkBox2.Checked ? "2" : "0";
            mse.StoreKBN3 = checkBox3.Checked ? "3" : "0";

            return mse;
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
            if (dgvDetail.CurrentRow == null)
                return;

            parStoreCD = dgvDetail.CurrentRow.Cells["colStoreCD"].Value.ToString();
            parChangeDate = dgvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();
            parStoreName = dgvDetail.CurrentRow.Cells["colStoreName"].Value.ToString();
            EndSec();
        }

        protected override void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            mbl = new Store_BL();
            bool ret = BindGrid();

            if (ret)
            {
                dgvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dgvDetail.CurrentRow.Selected = true;
                dgvDetail.Enabled = true;
                dgvDetail.Focus();
            }
            else
            {
                mbl.ShowMessage("E128");
                dgvDetail.DataSource = null;
            }
        }

        private void RadioButton_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {

                    detailControls[0].Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

        }

        private void DetailControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    bool ret = CheckDetail(Array.IndexOf(detailControls, sender));
                    if (ret)
                    {
                        if (detailControls.Length - 1 > Array.IndexOf(detailControls, sender))
                            detailControls[Array.IndexOf(detailControls, sender) + 1].Focus();

                        else
                            checkBox1.Focus();
                    }
                    else
                    {
                        ((Control)sender).Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private bool CheckDetail(int index)
        {
            switch (index)
            {
                case (int)EIndex.StoreName:

                    break;

                case (int)EIndex.StoreTo:
                    if (detailControls[index].Text != "")
                    {
                        int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                        if (result < 0)
                        {
                            //Ｅ１０６
                            mbl.ShowMessage("E106");
                            detailControls[index].Focus();
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        private void CheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {   //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    //あたかもTabキーが押されたかのようにする
                    //Shiftが押されている時は前のコントロールのフォーカスを移動
                    this.ProcessTabKey(!e.Shift);
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnSubF11_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
            try
            {
                base.FunctionProcess(FuncDisp - 1);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
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
