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
    public partial class Search_Brand : FrmSubForm
    {
        private const string ProNm = "ブランド検索";

        private enum EIndex : int
        {
            BrandName,
            MakerCD,
        }

        #region"公開プロパティ"
        public string parBrandCD = "";
        public string parBrandName = "";
        public string parMakerCD = "";
        public string parChangeDate = "";
        #endregion

        private Control[] detailControls;

        Brand_BL mbl;
        M_Brand_Entity mbe;
        //private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        public Search_Brand()
        {
            InitializeComponent();

            InitialControlArray();
            F9Visible = false;

           // dgvDetail.AllowUserToAddRows = false;
            dgvDetail.ColumnHeadersDefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);

            HeaderTitleText = "ブランド";
            this.Text = ProNm;

        }
        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox3, ScMaker.TxtCode };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

            //btnStoreCD.Click += new System.EventHandler(BtnSearch_Click);
            radioButton1.Enter += new System.EventHandler(RadioButton_Enter);
            radioButton2.Enter += new System.EventHandler(RadioButton_Enter);

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
                ckM_Label1.Text = parChangeDate;
            else
                ckM_Label1.Text = mbl.GetDate();

            ScMaker.LabelText = "";
            radioButton1.Checked = true;

            if(!string.IsNullOrWhiteSpace( parMakerCD))
            {
                ScMaker.TxtCode.Text = parMakerCD;
                CheckDetail((int)EIndex.MakerCD);
                ScMaker.TxtCode.Enabled = false;
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                mbl = new Brand_BL();
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
            mbe = GetEntity();
            DataTable dt = mbl.M_Brand_SelectAll(mbe);

            if (dt.Rows.Count == 0)
                return false;

            dgvDetail.DataSource = dt;
            return true;
        }

        private M_Brand_Entity GetEntity()
        {
            mbe = new M_Brand_Entity();
            mbe.DisplayKbn = radioButton1.Checked ? "0" : "1";
            mbe.ChangeDate = ckM_Label1.Text;
            mbe.MakerCD = detailControls[(int)EIndex.MakerCD].Text;
            mbe.BrandName = detailControls[(int)EIndex.BrandName].Text;

            return mbe;
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
            if (dgvDetail.CurrentRow is null)
                return;

            parBrandCD = dgvDetail.CurrentRow.Cells["colBrandCD"].Value.ToString();
            parBrandName = dgvDetail.CurrentRow.Cells["colBrandName"].Value.ToString();
            parChangeDate = dgvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();
            parMakerCD = dgvDetail.CurrentRow.Cells["MakerCD"].Value.ToString();

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

            bool ret = BindGrid();

            if (ret)
            {
                dgvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                //previousCtrl = this.ActiveControl;

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
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
                case (int)EIndex.MakerCD:
                    //入力なければチェックなし
                    if (detailControls[index].Text == "")
                    {
                        ScMaker.LabelText = "";
                        return true;
                    }
                    else
                    {
                        //仕入先マスタデータチェック
                        Vendor_BL mbl = new Vendor_BL();
                        M_Vendor_Entity mse = new M_Vendor_Entity
                        {
                            VendorCD = detailControls[index].Text,
                            ChangeDate = ckM_Label1.Text
                        };
                        bool ret = mbl.M_Vendor_SelectTop1(mse);
                        if (ret)
                        {
                            ScMaker.LabelText = mse.VendorName;
                        }
                        else
                        {
                            //Ｅ１３６
                            mbl.ShowMessage("E136");
                            ScMaker.LabelText = "";
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.BrandName:

                    break;


            }

            return true;
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

        //private void DgvDetail_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
    
    /// <summary>
    /// handle f1 to f12 click event
    /// implement base virtual function
    /// </summary>
    /// <param name="Index"></param>
    public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);
        }

        private void RadioButton_Enter(object sender, EventArgs e)
        {
            try
            {
                //previousCtrl = this.ActiveControl;

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvDetail_Paint(object sender, PaintEventArgs e)
        {
            string[] monthes = { "","メーカー (仕入先)",""};
            for (int j = 2; j < 3;)
            {
                Rectangle r1 =this.dgvDetail.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.dgvDetail.GetCellDisplayRectangle(j + 1, -1, true).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dgvDetail.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(monthes[j / 2],
                this.dgvDetail.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.dgvDetail.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 2;

            }
        }

     
    }
}
