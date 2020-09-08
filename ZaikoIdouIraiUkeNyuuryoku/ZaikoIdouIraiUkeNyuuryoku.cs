using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using GridBase;

namespace ZaikoIdouIraiUkeNyuuryoku
{
    /// <summary>
    /// ZaikoIdouIraiUkeNyuuryoku 在庫移動依頼受け一覧
    /// </summary>
    internal partial class ZaikoIdouIraiUkeNyuuryoku : FrmMainForm
    {
        private const string ProID = "ZaikoIdouIraiUkeNyuuryoku";
        private const string ProNm = "在庫移動依頼受け一覧";
        private const short mc_L_END = 3; // ロック用
        private const string IdouNyuryoku = "ZaikoIdouNyuuryoku.exe";

        private enum EIndex : int
        {
            StoreCD,
            CheckBox1,
            DateFrom,
            DateTo,          
        }

        private enum EColNo : int
        {
            Btn,     //
            RequestNO,
            Date,
            AnswerDay,
            Store,
            SKUCD,
            SKUName,
            Color,            
            Size,
            Suryo,
            AnswerKBN_CNT1,
            AnswerKBN_CNT9,
            COUNT
        }

        private Control[] detailControls;
       
        private ZaikoIdouIraiUkeNyuuryoku_BL mibl;
        private D_MoveRequest_Entity doe;
        
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避


        public ZaikoIdouIraiUkeNyuuryoku()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.SHOW);   
                this.InitialControlArray();

                //起動時共通処理
                base.StartProgram();
                Btn_F6.Text = "";
                F9Visible = false;

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                mibl = new ZaikoIdouIraiUkeNyuuryoku_BL();
                CboFromStoreCD.Bind(ymd, "2");

                Scr_Clr(0);

                detailControls[(int)EIndex.StoreCD].Focus(); 
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { CboFromStoreCD, ckM_CheckBox1, ckM_TextBox1, ckM_TextBox2};

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
        
        /// <summary>
        /// コードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDetail(int index)
        {
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.StoreCD:
                    ////選択必須(Entry required)
                    //if (!RequireCheck(new Control[] { detailControls[index] }))
                    //{
                    //    return false;
                    //}
                    //else
                    //{
                    if (CboFromStoreCD.SelectedIndex > 0)
                    {
                        if (!base.CheckAvailableStores(CboFromStoreCD.SelectedValue.ToString()))
                        {
                            bbl.ShowMessage("E141");
                            return false;
                        }
                    }
                    //}
                    break;

                case (int)EIndex.DateFrom:
                case (int)EIndex.DateTo:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //大小チェック
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.DateTo)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }
                    break;

            }

            return true;

        }
        protected override void ExecDisp()
        {

            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            
            //更新処理
            doe = GetEntity();
            DataTable dt = mibl.D_MoveRequest_SelectAllForShoukai(doe);

            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;

                for (int wRow = 0; wRow < GvDetail.RowCount; wRow++)
                {
                    //AnswerKBN	＝	1が1件以上の場合、明細行の色は淡い青色
                    if (bbl.Z_Set(GvDetail["AnswerKBN_CNT1", wRow].Value) >= 1)
                    {
                        for (int wCol = 0; wCol < GvDetail.ColumnCount; wCol++)
                        {
                            //セルスタイルを変更する
                            GvDetail[wCol, wRow].Style.BackColor = Color.DeepSkyBlue;
                        }
                    }
                    else
                    {
                        //AnswerKBN＝	1が0件かつAnswerKBN＝	9が1件以上の場合、明細は淡い赤色	
                        if (bbl.Z_Set(GvDetail["AnswerKBN_CNT9", wRow].Value) >= 1)

                            for (int wCol = 0; wCol < GvDetail.ColumnCount; wCol++)
                            {
                                //セルスタイルを変更する
                                GvDetail[wCol, wRow].Style.BackColor = Color.LightSalmon;
                            }
                        else
                            //AnswerKBN＝	1が0件かつAnswerKBN＝	9が0件かつAnswerKBN＝	0が1件以上の場合、灰色
                            for (int wCol = 0; wCol < GvDetail.ColumnCount; wCol++)
                            {
                                //セルスタイルを変更する
                                GvDetail[wCol, wRow].Style.BackColor = ClsGridBase.GrayColor;
                            }
                    }
                }
                GvDetail.Enabled = true;
                GvDetail.Focus();
            }
            else
            {
                bbl.ShowMessage("E128");
            }
        }

        protected override void ExecSec()
        {
            try
            {
                //在庫移動入力を起動します
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + IdouNyuryoku;
                if (System.IO.File.Exists(filePath))
                {
                    string  RequestNO = GvDetail.CurrentRow.Cells["colRequestNO"].Value.ToString();
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + RequestNO;
                    System.Diagnostics.Process.Start(filePath, cmdLine);
                }
                else
                {
                    //ファイルなし
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_MoveRequest_Entity GetEntity()
        {
            doe = new D_MoveRequest_Entity();

            if (CboFromStoreCD.SelectedIndex > 0)
                doe.FromStoreCD = CboFromStoreCD.SelectedValue.ToString();
            else
                doe.FromStoreCD = "";

            doe.AnswerDateFrom = detailControls[(int)EIndex.DateFrom].Text;
            doe.AnswerDateTo = detailControls[(int)EIndex.DateTo].Text;

            if (ckM_CheckBox1.Checked)
                doe.AnswerKBN = 1;
            else
                doe.AnswerKBN = 0;
            doe.InsertOperator = InOperatorCD;

            return doe;
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            if (Kbn == 0)
            {
                foreach (Control ctl in detailControls)
                {
                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }
                
            }

            ckM_CheckBox1.Checked = true;
        }

        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            switch (Index)
            {
                case 0:     // F1:終了
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会
                    {
                        break;
                    }

                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        break;
                    }

            }   //switch end

        }

        // ==================================================
        // 終了処理
        // ==================================================
        protected override void EndSec()
        {
            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
        }

        #region "内部イベント"
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
                    int index = Array.IndexOf(detailControls, sender);
                    bool ret = CheckDetail(index);

                    if (ret)
                    {
                        if (index == (int)EIndex.DateTo)
                        {
                           BtnSubF11.Focus();
                        }
                        else
                            detailControls[index + 1].Focus();

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

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnF11_Click(object sender, EventArgs e)
        {
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

        private void GvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == (int)EColNo.Btn)
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
        //private void GvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    DataGridView dgv = (DataGridView)sender;

        //    //セルの列を確認
        //    if (e.ColumnIndex == (int)EColNo.AnswerKBN_CNT1)
        //    {
        //        //セルの値により、背景色を変更する

        //        //AnswerKBN	＝	1が1件以上の場合、明細行の色は淡い青色
        //        if (bbl.Z_Set( e.Value) >= 1)
        //        {
        //            e.CellStyle.BackColor = Color.DeepSkyBlue;
        //        }
        //        else
        //        {
        //            //AnswerKBN＝	1が0件かつAnswerKBN＝	9が1件以上の場合、明細は淡い赤色	
        //            if(bbl.Z_Set(dgv.CurrentRow.Cells["AnswerKBN_CNT9"].Value) >= 1 )
        //                e.CellStyle.BackColor = Color.LightSalmon;
        //            else
        //                //AnswerKBN＝	1が0件かつAnswerKBN＝	9が0件かつAnswerKBN＝	0が1件以上の場合、灰色
        //                e.CellStyle.BackColor = ClsGridBase.GrayColor;
        //        }
        //    }
        //}


        private void GvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            for (int wRow = 0; wRow < GvDetail.RowCount; wRow++)
            {
                //AnswerKBN	＝	1が1件以上の場合、明細行の色は淡い青色
                if (bbl.Z_Set(GvDetail["AnswerKBN_CNT1", wRow].Value) >= 1)
                {
                    for (int wCol = 0; wCol < GvDetail.ColumnCount; wCol++)
                    {
                        //セルスタイルを変更する
                        GvDetail[wCol, wRow].Style.BackColor = Color.DeepSkyBlue;
                    }
                }
                else
                {
                    //AnswerKBN＝	1が0件かつAnswerKBN＝	9が1件以上の場合、明細は淡い赤色	
                    if (bbl.Z_Set(GvDetail["AnswerKBN_CNT9", wRow].Value) >= 1)

                        for (int wCol = 0; wCol < GvDetail.ColumnCount; wCol++)
                        {
                            //セルスタイルを変更する
                            GvDetail[wCol, wRow].Style.BackColor = Color.LightSalmon;
                        }
                    else
                        //AnswerKBN＝	1が0件かつAnswerKBN＝	9が0件かつAnswerKBN＝	0が1件以上の場合、灰色
                        for (int wCol = 0; wCol < GvDetail.ColumnCount; wCol++)
                        {
                            //セルスタイルを変更する
                            GvDetail[wCol, wRow].Style.BackColor = ClsGridBase.GrayColor;
                        }
                }
            }
        }
        #endregion
    }
}








