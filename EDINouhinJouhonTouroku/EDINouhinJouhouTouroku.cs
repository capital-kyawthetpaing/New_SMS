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
using CrystalDecisions.Shared;

namespace EDINouhinJouhonTouroku
{
    public partial class FrmEDINouhinJouhouTouroku : FrmMainForm
    {
        M_MultiPorpose_Entity mmpe;
        EDINouhinJouhon_Batch_BL ediNHJ_bl;
        private DataTable dtDelivery;
        int selectRowIndex = -1;
        private string mEdiMode;

        private enum EColNo : int
        {
            Chk,
            SKENBangouA,
            SKENNouhinshoNO,
            ImportDateTime,
            Vendor,
            VendorName,
            ImportDetailsSu,
            ErrorSu,
            ImportFile,
            

            COUNT
        }
        public FrmEDINouhinJouhouTouroku()
        {
            InitializeComponent();
        }

        private void FrmEDINouhinJouhouTouroku_Load(object sender, EventArgs e)
        {
            InProgramID = "EDINouhinJouhouTouroku";

           
            //SetFunctionLabel(EProMode.MENTE);
            this.SetFunctionLabel(EProMode.SHOW);
            StartProgram();

            ediNHJ_bl = new EDINouhinJouhon_Batch_BL();
            SetEnableVisible();

            ExecSec();

        }

        private void SetEnableVisible()
        {
            ModeVisible = false;
            Btn_F2.Text = string.Empty;
            Btn_F3.Text = string.Empty;
            Btn_F4.Text = string.Empty;
            Btn_F5.Text = string.Empty;
            Btn_F12.Text = string.Empty;

            Btn_F10.Text = "印刷(F10)";
            Btn_F11.Text = "表示(F11)";
        }

        /// <summary>
        /// Control to run or not console
        /// </summary>

        private void UpdateM_MultiPorpose()
        {
            mmpe = new M_MultiPorpose_Entity();
            
            mmpe.ID = "322";
            mmpe.Key = "1";
            mmpe.UpdateOperator = InOperatorCD;
            mmpe.PC = InPcID;

            if (mEdiMode == "1")
            {
                //　記憶していた初期表示値が１
                //＆画面・在庫SKS連携処理の処理モードが「処理停止中」の場合
                if (lblEdiMode.Text == "処理停止中")
                {
                    mmpe.Num1 = "0";
                }
                else
                {
                    return;
                }
            }
            else
            {
                //　記憶していた初期表示値が０
                //＆画面・在庫SKS連携処理の処理モードが「処理実行中」の場合
                if (lblEdiMode.Text == "処理実行中")
                {
                    mmpe.Num1 = "1";
                }
                else
                {
                    return;
                }
            }

            ediNHJ_bl.M_MultiPorpose_Update(mmpe);
        }
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
        protected override void ExecDisp()
        {
             selectRowIndex = -1;
            for (int row = 0; row < gdvDSKENDelivery.RowCount; row++)
            {
                if (gdvDSKENDelivery[(int)EColNo.Chk, row].Value != null)
                if (gdvDSKENDelivery[(int)EColNo.Chk, row].Value.ToString().Equals("true"))
                {
                    selectRowIndex = row;
                    break;
                }
            }

            //「結果」のチェックがONされている場合、画面転送表02に従って取込履歴の詳細情報を表示する。
            if (selectRowIndex >= 0)
            {
                lblImportDateTime.Text = gdvDSKENDelivery[(int)EColNo.ImportDateTime, selectRowIndex].Value.ToString();
                lblVendor.Text = gdvDSKENDelivery[(int)EColNo.Vendor, selectRowIndex].Value.ToString();

                D_SKENDeliveryDetails_Entity de = new D_SKENDeliveryDetails_Entity();

                    //ImportDateTime = gdvDSKENDelivery[(int)EColNo.ImportDateTime, selectRowIndex].Value.ToString(),
                    //SKENNouhinshoNO = gdvDSKENDelivery[(int)EColNo.SKENNouhinshoNO, selectRowIndex].Value.ToString(),
                    de.SKENBangouA = gdvDSKENDelivery[(int)EColNo.SKENBangouA, selectRowIndex].Value.ToString();
                if ((chkError.Checked && chkCorrect.Checked) || (!chkError.Checked && !chkCorrect.Checked))
                    de.ChkFlg = "2";
                else if(chkError.Checked)
                    de.ChkFlg = "1";
                else if (chkCorrect.Checked)
                    de.ChkFlg = "0";
                


                dtDelivery = ediNHJ_bl.D_SKENDeliveryDetails_SelectAll(de);

                gdvDSKENDeliveryDetail.DataSource = dtDelivery;

                if (dtDelivery.Rows.Count > 0)
                {
                    gdvDSKENDeliveryDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    gdvDSKENDeliveryDetail.CurrentRow.Selected = true;
                    gdvDSKENDeliveryDetail.Enabled = true;
                    gdvDSKENDeliveryDetail.Focus();
                }
                else
                {
                    ediNHJ_bl.ShowMessage("E128");
                }
            }
        }
        protected override void ExecSec()
        {
            try
            {
                //【取込履歴】
                DataTable dt_Num = new DataTable();
                mmpe = new M_MultiPorpose_Entity
                {
                    ID = "322",
                    Key = "1"
                };

                dt_Num = ediNHJ_bl.M_MultiPorpose_SelectID(mmpe);
                if (dt_Num.Rows.Count > 0)
                {
                    //[999日間の履歴を保持しています]
                    lblRireki.Text = dt_Num.Rows[0]["Num3"].ToString() + "日間の履歴を保持しています";

                    mEdiMode = dt_Num.Rows[0]["Num1"].ToString();

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
                    ediNHJ_bl.ShowMessage("E101");
                    EndSec();
                }
                Scr_Clr(0);
                //履歴データ取得処理
                DataTable dt = ediNHJ_bl.D_SKENDelivery_SelectAll();

                gdvDSKENDelivery.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    gdvDSKENDelivery.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    gdvDSKENDelivery.CurrentRow.Selected = true;
                    gdvDSKENDelivery.Enabled = true;

                    gdvDSKENDelivery.ReadOnly = false;

                }
                else
                {
                    ediNHJ_bl.ShowMessage("E128");
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
                if (dtDelivery == null || gdvDSKENDeliveryDetail.Rows.Count == 0)
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

                EDINouhinJouhouTouroku.EDINouhinJouhouTouroku_Report Report = new EDINouhinJouhouTouroku.EDINouhinJouhouTouroku_Report();


                //DataTableのDetailOnが１かどうかで詳細セクションを印字するかどうかの設定を
                //している（セクションエキスパート）

                switch (PrintMode)
                {
                    case EPrintMode.DIRECT:

                        //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                        ret = ediNHJ_bl.ShowMessage("Q208");
                        if (ret == DialogResult.No)
                        {
                            return;
                        }
                        Report.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        Report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                        // 印字データをセット
                        Report.SetDataSource(dtDelivery);
                        Report.Refresh();
                        Report.SetParameterValue("txtImportDate", lblImportDateTime.Text.ToString() );
                        Report.SetParameterValue("txtVendor", lblVendor.Text.ToString());
                        Report.SetParameterValue("txtSKENNouhinshoNO", gdvDSKENDelivery[(int)EColNo.SKENBangouA, selectRowIndex].Value.ToString());

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
                        if (ediNHJ_bl.ShowMessage("Q204") != DialogResult.Yes)
                        {
                            return;
                        }
                        string filePath = "";
                        if (!ShowSaveFileDialog(InProgramNM, out filePath))
                        {
                            return;
                        }

                        // 印字データをセット
                        Report.SetDataSource(dtDelivery);
                        Report.Refresh();

                        bool result = OutputPDF(filePath, Report);

                        //PDF出力が完了しました。
                        ediNHJ_bl.ShowMessage("I202");

                        break;
                }
            }
            finally
            {

            }

            //更新後画面そのまま
            //detailControls[0].Focus();
        }

        private void gdvDSKENDelivery_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Maintained_CellClick(sender, e);
        }
        
        protected void Maintained_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((sender as DataGridView).CurrentCell is DataGridViewCheckBoxCell)
            {
                if ((Convert.ToBoolean(gdvDSKENDelivery.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue) == true))
                {
                    foreach (DataGridViewRow row1 in gdvDSKENDelivery.Rows)
                    {
                        DataGridViewCheckBoxCell chk1 = row1.Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                        chk1.Value = chk1.FalseValue;
                        if (row1.Index % 2 == 0)
                        {
                            row1.DefaultCellStyle.BackColor = Color.White;
                        }
                        else
                        {
                            row1.DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                        }
                    }

                    gdvDSKENDelivery.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;

                    gdvDSKENDelivery.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else
                {
                    gdvDSKENDelivery.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    gdvDSKENDelivery.ClearSelection();
                }

                gdvDSKENDelivery.EndEdit();
            }
        }

        private void Scr_Clr(short Kbn)
        {
            chkError.Checked = true;
            chkCorrect.Checked = true;

            lblImportDateTime.Text = "";
            lblVendor.Text = "";

            gdvDSKENDeliveryDetail.DataSource = null;
            gdvDSKENDeliveryDetail.Refresh();
            gdvDSKENDelivery.Refresh();
        }
        protected override void EndSec()
        {
            this.Close();
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

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                mmpe = new M_MultiPorpose_Entity();

                mmpe.ID = "322";
                mmpe.Key = "1";
                mmpe.UpdateOperator = InOperatorCD;
                mmpe.PC = InPcID;
                //現状、「処理実行中」の場合、何もしない												
                if (lblEdiMode.Text != "処理実行中")
                {
                    //現用、「処理停止中」(黄色)の場合、「処理実行中」(青色)とする																
                    lblEdiMode.Text = "処理実行中";
                    mmpe.Num1 = "1";
                    lblEdiMode.BackColor = Color.DeepSkyBlue;

                }

                ediNHJ_bl.M_MultiPorpose_Update(mmpe);
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
                mmpe = new M_MultiPorpose_Entity();

                mmpe.ID = "322";
                mmpe.Key = "1";
                mmpe.UpdateOperator = InOperatorCD;
                mmpe.PC = InPcID;

                //現状、「処理停止中」の場合、何もしない												
                if (lblEdiMode.Text != "処理停止中")
                {
                    //現用、「処理実行中」(青色)の場合、「処理停止中」(黄色)とする																
                    lblEdiMode.Text = "処理停止中";
                    mmpe.Num1 = "0";
                    lblEdiMode.BackColor = Color.Yellow;

                }

                ediNHJ_bl.M_MultiPorpose_Update(mmpe);
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
