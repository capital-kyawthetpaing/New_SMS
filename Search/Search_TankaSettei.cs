﻿using System;
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
    /// <summary>
    /// 単価設定検索
    /// </summary>
    public partial class Search_TankaSettei : FrmSubForm
    {
        private const string ProNm = "単価設定検索";

        private enum EIndex : int
        {
            TankaSetteiFrom,
            TankaSetteiTo,
            TankaSetteiName
        }

        #region"公開プロパティ"
        public string parTankaCD = "";
        public string parChangeDate = "";
        public string parTankaName = "";    //2019.6.11 add
        #endregion

        private Control[] detailControls;

        TankaCD_BL mbl;
        M_TankaCD_Entity mke;

        public Search_TankaSettei()
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;
            BtnF9Text = "";
            dgvDetail.TopLeftHeaderCell.Value = "  №";
            
        }
        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2, ckM_TextBox3 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
            }

            DataGridViewCellStyle dataGridViewCellStyleRt = new DataGridViewCellStyle();
            dataGridViewCellStyleRt.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyleRt.Format = "N2";
            dataGridViewCellStyleRt.NullValue = null;
            this.GeneralRate.DefaultCellStyle = dataGridViewCellStyleRt;
            this.MemberRate.DefaultCellStyle = dataGridViewCellStyleRt;
            this.colClientRate.DefaultCellStyle = dataGridViewCellStyleRt;
            this.SaleRate.DefaultCellStyle = dataGridViewCellStyleRt;
            this.WebRate.DefaultCellStyle = dataGridViewCellStyleRt;
                  
    }
        /// <summary>
        /// 画面クリア
        /// </summary>
        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
                ctl.Text = "";

            //初期値設定
            if (DateTime.TryParse(parChangeDate , out DateTime dt))
                lblChangeDate.Text = parChangeDate;
            else
                lblChangeDate.Text = mbl.GetDate();

            radioButton1.Checked = true;
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
            mke = GetEntity();
            DataTable dt = mbl.M_TankaCD_SelectAll(mke);

            if (dt.Rows.Count == 0)
                return false;

            dgvDetail.DataSource = dt;
            return true;
        }

        private M_TankaCD_Entity GetEntity()
        {
            mke = new M_TankaCD_Entity();
            mke.DisplayKbn = radioButton1.Checked ? "0" : "1";
            mke.ChangeDate = lblChangeDate.Text;
            mke.TankaCDFrom = detailControls[(int)EIndex.TankaSetteiFrom].Text;

            if (detailControls[(int)EIndex.TankaSetteiTo].Text == "")
                mke.TankaCDTo = new string('Z', ((CKM_Controls.CKM_TextBox)detailControls[(int)EIndex.TankaSetteiTo]).MaxLength);
            else
                mke.TankaCDTo = detailControls[(int)EIndex.TankaSetteiTo].Text;

            mke.TankaName = detailControls[(int)EIndex.TankaSetteiName].Text;

            return mke;
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
        private void DgvDetail_DoubleClick(object sender, EventArgs e)
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
            parTankaCD = dgvDetail.CurrentRow.Cells["colTankaCD"].Value.ToString();
            parChangeDate = dgvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();
            parTankaName = dgvDetail.CurrentRow.Cells["colTankaName"].Value.ToString(); ;    //2019.6.11 add
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

            mbl = new TankaCD_BL();
            bool ret= BindGrid();
         
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
                            //あたかもTabキーが押されたかのようにする
                            //Shiftが押されている時は前のコントロールのフォーカスを移動
                            this.ProcessTabKey(!e.Shift);
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
                case (int)EIndex.TankaSetteiName:

                    break;

                case (int)EIndex.TankaSetteiTo:
                    if (detailControls[index].Text != "")
                    {
                        int result = detailControls[index].Text.CompareTo(detailControls[index-1].Text);
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

        private void BtnSubF11_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
            try
            {
                base.FunctionProcess(FuncDisp-1);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void DgvDetail_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
