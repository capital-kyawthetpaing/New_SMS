using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using GridBase;

namespace EDIKaitouNoukiTouroku
{
    /// <summary>
    /// EDIKaitouNoukiTouroku EDI回答納期登録
    /// </summary>
    internal partial class EDIKaitouNoukiTouroku : FrmMainForm
    {
        private const string ProID = "EDIKaitouNoukiTouroku";
        private const string ProNm = "EDI回答納期登録";
        private const string BatchProID = "EDIKaitouNoukiTourokuB.exe";
        private enum EIndex : int
        {
            BtnStart,
            BtnStop,
            CheckBox1,
            CheckBox2,
            CheckBox3,          
        }

        private enum EColNo : int
        {
            Chk,     //
            EDIImportNO,
            ImportDateTime,
            Vendor,
            VendorName,
            ImportDetailsSu,
            ErrorSu,
            ImportFile,

            COUNT
        }

        private Control[] detailControls;    
        private EDIKaitouNoukiTouroku_BL mibl;    
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避 
        private string mEdiMode;
        private DataTable dtEdi;

        public EDIKaitouNoukiTouroku()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.SHOW);   //照会プログラムとして起動
                this.InitialControlArray();

                F9Visible = false;
                Btn_F10.Text = "印刷(F10)";
                Btn_F12.Text = "最新化(F12)";

                //起動時共通処理
                base.StartProgram();
                
                mibl = new EDIKaitouNoukiTouroku_BL();



                //【受信履歴】
                //画面転送表01に従って、連携履歴情報を表示
                ExecSec();

                detailControls[0].Focus(); 
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
            detailControls = new Control[] { BtnStart, BtnStop, ckM_CheckBox1, ckM_CheckBox2, ckM_CheckBox3 };

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

            return true;

        }
        protected override void ExecDisp()
        {
            int selectRowIndex = -1;
            for (int row = 0; row < GvDetail.RowCount; row++)
            {
                if(GvDetail[(int)EColNo.Chk, row].Value != null)
                if (GvDetail[(int)EColNo.Chk, row].Value.ToString().Equals("true"))
                {
                    selectRowIndex = row;
                    break;
                }
            }

            //「結果」のチェックがONされている場合、画面転送表02に従って取込履歴の詳細情報を表示する。
            if(selectRowIndex>=0)
            {
                lblImportDateTime.Text = GvDetail[(int)EColNo.ImportDateTime, selectRowIndex].Value.ToString();
                lblVendor.Text = GvDetail[(int)EColNo.Vendor, selectRowIndex].Value.ToString() + " " + GvDetail[(int)EColNo.VendorName, selectRowIndex].Value.ToString();

                D_EDIDetail_Entity de = new D_EDIDetail_Entity
                {
                    EDIImportNO = GvDetail[(int)EColNo.EDIImportNO, selectRowIndex].Value.ToString(),
                    ErrorKBN = ckM_CheckBox1.Checked ? "1" : "0",
                    ChkAnswer = ckM_CheckBox2.Checked ? 1 : 0,
                    ChkNoAnswer = ckM_CheckBox3.Checked ? 1 : 0
                };

                dtEdi = mibl.D_EDIOrderDetails_SelectAll(de);

                GvDetail2.DataSource = dtEdi;

                if (dtEdi.Rows.Count > 0)
                {
                    GvDetail2.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GvDetail2.CurrentRow.Selected = true;
                    GvDetail2.Enabled = true;
                    GvDetail2.Focus();
                }
                else
                {
                    bbl.ShowMessage("E128");
                }
            }           
        }

        protected override void ExecSec()
        {
            try
            {
                //【取込履歴】
                M_MultiPorpose_Entity me = new M_MultiPorpose_Entity
                {
                    ID = MultiPorpose_BL.ID_EDI,
                    Key = "1"
                };

                MultiPorpose_BL mbl = new MultiPorpose_BL();
                DataTable dt = mbl.M_MultiPorpose_Select(me);
                if (dt.Rows.Count > 0)
                {
                    //[999日間の履歴を保持しています]
                    lblRireki.Text = dt.Rows[0]["Num3"].ToString() + "日間の履歴を保持しています";

                    mEdiMode = dt.Rows[0]["Num1"].ToString();

                    if (mEdiMode == "1")
                    {
                        //汎用マスター.	数字型１＝１なら、「処理実行中」として青色		
                        lblEdiMode.Text = "処理実行中";
                        lblEdiMode.BackColor = Color.DeepSkyBlue;
                    }
                    else
                    {
                        //汎用マスター.	数字型１＝０なら、「処理停止中」として黄色				
                        lblEdiMode.Text = "処理停止中";
                        lblEdiMode.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    //Ｅ１０１
                    bbl.ShowMessage("E101");
                    EndSec();
                }

                Scr_Clr(0);

                //履歴データ取得処理
                 dt = mibl.D_Edi_SelectAll();

                GvDetail.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GvDetail.CurrentRow.Selected = true;
                    GvDetail.Enabled = true;

                    GvDetail.ReadOnly = false;
                    for (int row = 0; row < GvDetail.RowCount; row++)
                    {
                        for (int col = (int)EColNo.Chk + 1; col < GvDetail.ColumnCount; col++)
                        {
                            GvDetail[col, row].ReadOnly = true;
                        }
                    }
                    GvDetail.Focus();
                }
                else
                {
                    bbl.ShowMessage("E128");
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        protected override void PrintSec()
        {
            // レコード定義を行う

            try
            {
                if (dtEdi == null || GvDetail2.Rows.Count == 0)
                {
                    return;
                }
                //xsdファイルを保存します。

                //DB　---→　xsd　----→　クリスタルレポート

                //というデータの流れになります
                //dtEdi.TableName = ProID;
                //dtEdi.WriteXmlSchema("DataTable" + ProID + ".xsd");

                //①保存した.xsdはプロジェクトに追加しておきます。
                DialogResult ret;
                EDIKaitouNoukiTouroku_Report Report = new EDIKaitouNoukiTouroku_Report();

                //DataTableのDetailOnが１かどうかで詳細セクションを印字するかどうかの設定を
                //している（セクションエキスパート）

                switch (PrintMode)
                {
                    case EPrintMode.DIRECT:

                        //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                        ret = bbl.ShowMessage("Q208");
                        if (ret == DialogResult.Cancel)
                        {
                            return;
                        }
                        Report.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        Report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                        // 印字データをセット
                        Report.SetDataSource(dtEdi);
                        Report.Refresh();

                        if (ret == DialogResult.Yes)
                        {
                            //プレビュー
                            var previewForm = new Viewer();
                            previewForm.CrystalReportViewer1.ShowPrintButton = true;
                            previewForm.CrystalReportViewer1.ReportSource = Report;
                            //previewForm.CrystalReportViewer1.Zoom(1);

                            previewForm.ShowDialog();
                        }
                        else
                        {
                            int marginLeft = 360;
                            CrystalDecisions.Shared.PageMargins margin = Report.PrintOptions.PageMargins;
                            margin.leftMargin = marginLeft; // mmの指定をtwip単位に変換する
                            margin.topMargin = marginLeft;
                            margin.bottomMargin = marginLeft;//mmToTwip(marginLeft);
                            margin.rightMargin = marginLeft;
                            Report.PrintOptions.ApplyPageMargins(margin);
                            // プリンタに印刷
                            Report.PrintToPrinter(0, false, 0, 0);
                        }
                        break;

                    case EPrintMode.PDF:
                        if (bbl.ShowMessage("Q204") != DialogResult.Yes)
                        {
                            return;
                        }
                        string filePath = "";
                        if (!ShowSaveFileDialog(InProgramNM, out filePath))
                        {
                            return;
                        }

                        // 印字データをセット
                        Report.SetDataSource(dtEdi);
                        Report.Refresh();

                        bool result = OutputPDF(filePath, Report);

                        //PDF出力が完了しました。
                        bbl.ShowMessage("I202");

                        break;
                }
            }
            finally
            {

            }

            //更新後画面そのまま
            detailControls[0].Focus();
        }

        private void Renew()
        {

        }
        private void CheckProcess()
        {
            System.Diagnostics.Process[] ps =
             System.Diagnostics.Process.GetProcessesByName(BatchProID);

            if (ps.Length > 0)
            {
                //MessageBox.Show(strProcessName + "を検出しました。");
            }
            else
            {
                //BatchProIDを起動
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + BatchProID;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID;
                    System.Diagnostics.Process.Start(filePath, cmdLine);
                }
                else
                {
                    //ファイルなし
                }
            }

        }
        private void  UpdateM_MultiPorpose()
        {
            M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity();
            mme.ID = MultiPorpose_BL.ID_EDI;
            mme.Key = "1";
            mme.UpdateOperator = InOperatorCD;
            mme.PC = InPcID;

            //　記憶していた初期表示値が１
            //＆画面・在庫SKS連携処理の処理モードが「処理停止中」の場合
            if (lblEdiMode.Text == "処理停止中")
            {
                mme.Num1 = "0";
            }
            else
            {
                //＆画面・在庫SKS連携処理の処理モードが「処理実行中」の場合
                mme.Num1 = "1";
            }          

            mibl.M_MultiPorpose_Update(mme);
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            ckM_CheckBox1.Checked = true;
            ckM_CheckBox2.Checked = true;
            ckM_CheckBox3.Checked = true;

            lblImportDateTime.Text = "";
            lblVendor.Text = "";

            GvDetail2.DataSource = null;
            GvDetail2.Refresh();
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

                        Scr_Clr(0);
                        break;
                    }
                case 9://F10:印刷
                    ExecDisp();

                    this.Cursor = Cursors.WaitCursor;
                    this.PrintMode = EPrintMode.DIRECT;
                    PrintSec();
                    this.Cursor = Cursors.Default;
                    break;

                case 11://F12:最新化
                    ExecSec();
                    break;
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
                        if (index == (int)EIndex.CheckBox3)
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

        private void BtnSubF10_Click(object sender, EventArgs e)
        {
            //印刷ボタンClick時   
            try
            {
                FunctionProcess(9);

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
        private void BtnSubF12_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
            try
            {
                FunctionProcess(FuncExec - 1);

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

        private void GvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void GvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            ////セルの列を確認
            //if (e.ColumnIndex == (int)EColNo.Chk)
            //{
            //    //セルの値により、背景色を変更する
            //    switch (e.Value)
            //    {
            //        case true:
            //            e.CellStyle.BackColor = Color.Yellow;
            //            GvDetail.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
            //            break;
            //        default:
            //            e.CellStyle.BackColor = ClsGridBase.GridColor;
            //            GvDetail.Rows[e.RowIndex].DefaultCellStyle.BackColor = ClsGridBase.GridColor;
            //            break;

            //    }
            //}
        }
        private void GvDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try {
                //列のインデックスを確認する
                if (e.ColumnIndex == (int)EColNo.Chk && e.RowIndex >= 0)
                {
                    //チェックONした場合
                    if (GvDetail[e.ColumnIndex, e.RowIndex].Value.ToString() == "true")
                    {
                        for (int row = 0; row < GvDetail.RowCount; row++)
                        {
                            if (row != e.RowIndex)
                            {
                                GvDetail[e.ColumnIndex, row].Value = false;

                                if (row % 2 != 0)
                                    GvDetail.Rows[row].DefaultCellStyle.BackColor =  Color.FromArgb(221, 235, 247);
                                else
                                GvDetail.Rows[row].DefaultCellStyle.BackColor = Color.White;// Color.FromArgb(191, 191, 191);

                            }
                            else
                            {
                                GvDetail.Rows[row].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                        }
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
        private void GvDetail_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (GvDetail.CurrentCellAddress.X == 0 && GvDetail.IsCurrentCellDirty)
                {
                    //コミットする
                    GvDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                //現状、「処理実行中」の場合、何もしない												
                if (lblEdiMode.Text != "処理実行中")
                {
                    //現用、「処理停止中」(黄色)の場合、「処理実行中」(青色)とする																
                    lblEdiMode.Text = "処理実行中";
                    lblEdiMode.BackColor = Color.DeepSkyBlue;

                }
                //どちらの場合でもプログラム「EDIKaitouNoukiTourokuB.exe」が起動中でなければ起動する。
                //CheckProcess();

                //テーブル転送仕様Ａ、テーブル転送仕様Ｚに従って、更新処理。
                UpdateM_MultiPorpose();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                //現状、「処理停止中」の場合、何もしない												
                if (lblEdiMode.Text != "処理停止中")
                {
                    //現用、「処理実行中」(青色)の場合、「処理停止中」(黄色)とする																
                    lblEdiMode.Text = "処理停止中";
                    lblEdiMode.BackColor = Color.Yellow;

                }
                //テーブル転送仕様Ａ、テーブル転送仕様Ｚに従って、更新処理。
                UpdateM_MultiPorpose();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        #endregion

    }
}








