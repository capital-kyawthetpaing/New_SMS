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
using CKM_Controls;

namespace Search
{
    public partial class Search_Staff : FrmSubForm
    {
        private const string ProNm = "スタッフ検索";

        private enum EIndex : int
        {
            StoreCD,
            StaffFrom,
            StaffTo,
            StaffName,
            StaffKana
        }

        #region"公開プロパティ"
        public string parStaffCD = "";
        public string parStaffName = "";
        public string parChangeDate = "";
        #endregion

        private Control[] detailControls;

        Staff_BL mbl;
        M_Staff_Entity mke;
        //private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        public Search_Staff()
        {
            InitializeComponent();

            InitialControlArray();
            F9Visible = false;

            HeaderTitleText = "スタッフ";
            this.Text = ProNm;
          
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ScStore.TxtCode, ckM_TextBox1, ckM_TextBox2, ckM_TextBox3, ckM_TextBox4 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

            //btnStoreCD.Click += new System.EventHandler(BtnSearch_Click);
            radioButton1.Enter += new System.EventHandler(RadioButton_Enter);
            radioButton2.Enter += new System.EventHandler(RadioButton_Enter);
            radioButton1.KeyDown += new  KeyEventHandler(RadioButton_KeyDown);
            radioButton2.KeyDown += new KeyEventHandler(RadioButton_KeyDown);

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

            ScStore.LabelText = "";
            radioButton1.Checked = true;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                Scr_Clr();

                radioButton1.Focus();
                dgvDetail.AllowUserToAddRows = false;
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
            DataTable dt = mbl.M_Staff_SelectAll(mke);

            if (dt.Rows.Count == 0)
                return false;

            dgvDetail.DataSource = dt;
            return true;
        }

        private M_Staff_Entity GetEntity()
        {
            mke = new M_Staff_Entity();
            mke.DisplayKbn = radioButton1.Checked ? "0" : "1";
            mke.ChangeDate = ckM_Label1.Text;
            mke.StoreCD = detailControls[(int)EIndex.StoreCD].Text;
            mke.StaffCDFrom = detailControls[(int)EIndex.StaffFrom].Text;

            if (detailControls[(int)EIndex.StaffTo].Text == "")
                mke.StaffCDTo = "ZZZ";
            else
                mke.StaffCDTo = detailControls[(int)EIndex.StaffTo].Text;

            mke.StaffName = detailControls[(int)EIndex.StaffName].Text;
            mke.StaffKana = detailControls[(int)EIndex.StaffKana].Text;

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
            if (dgvDetail.CurrentRow is null)
                return;

            parStaffCD = dgvDetail.CurrentRow.Cells["colStaffCD"].Value.ToString();
            parStaffName = dgvDetail.CurrentRow.Cells["colStaffName"].Value.ToString();
            parChangeDate = dgvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();

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

            mbl = new Staff_BL();
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
        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                //previousCtrl = this.ActiveControl;

                //SetFuncKey(this, 8, false);

                //switch (Array.IndexOf(detailControls, sender))
                //{
                //    case (int)EIndex.StoreCD:

                //        SetFuncKey(this, 8, true);
                //        break;
                //}

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
                case (int)EIndex.StoreCD:
                    //入力なければチェックなし
                    if (detailControls[index].Text == "")
                    {
                        ScStore.LabelText = "";
                        return true;
                    }
                    else
                    {
                        //ストアマスタデータチェック
                        Store_BL mbl = new Store_BL();
                        M_Store_Entity mse = new M_Store_Entity
                        {
                            StoreCD = detailControls[index].Text,
                            ChangeDate = ckM_Label1.Text
                        };

                        DataTable dt = mbl.M_Store_Select(mse);
                        if (dt.Rows.Count > 0)
                        {
                            ScStore.LabelText = dt.Rows[0]["StoreName"].ToString();
                        }
                        else
                        {
                            //Ｅ１３６
                            mbl.ShowMessage("E136");
                            ScStore.LabelText = "";
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.StaffName:

                    break;

                case (int)EIndex.StaffTo:
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
        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            //switch (Index)
            //{
            //    case 8: //F9:検索
            //        if (Array.IndexOf(detailControls, previousCtrl) == (int)EIndex.StoreCD)
            //        {
            //            //店舗検索
            //            SearchData(EIndex.StoreCD, previousCtrl);
            //        }
            //        break;
            //}
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Control setCtl = null;

            //    //検索ボタンClick時
            //    if (((Button)sender).Name == btnStoreCD.Name)
            //    {
            //        //店舗検索
            //        setCtl = detailControls[(int)EIndex.StoreCD];
            //        SearchData(EIndex.StoreCD, setCtl);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //エラー時共通処理
            //    MessageBox.Show(ex.Message);
            //}
        }

        /// <summary>
        /// 検索フォーム起動処理
        /// </summary>
        /// <param name="kbn"></param>
        /// <param name="setCtl"></param>
        private void SearchData(EIndex kbn, Control setCtl)
        {
            //switch (kbn)
            //{
            //    case EIndex.StoreCD:
            //        using (Search_Store frmStaff = new Search_Store())
            //        {
            //            frmStaff.ShowDialog();
            //            setCtl.Text = frmStaff.parStoreCD;
            //        }
            //        break;

            //}

        }

        private void RadioButton_Enter(object sender, EventArgs e)
        {
            try
            {
                //previousCtrl = this.ActiveControl;

                //SetFuncKey(this, 8, false);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

    }
}
