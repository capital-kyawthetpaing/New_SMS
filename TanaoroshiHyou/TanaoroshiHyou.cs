using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using CrystalDecisions.CrystalReports.Engine;

namespace TanaoroshiHyou
{
    /// <summary>
    /// TanaoroshiHyou 棚卸表
    /// </summary>
    internal partial class TanaoroshiHyou : FrmMainForm
    {
        private const string ProID = "TanaoroshiHyou";
        private const string ProNm = "棚卸表";

        private enum EIndex : int
        {
            SoukoCD,
            InventoryDate,

            Rdo2,
            Rdo1,
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private Tanaoroshi_BL tabl;
        private D_Inventory_Entity die;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        
        public TanaoroshiHyou()
        {
            InitializeComponent();
                    }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM=ProNm;

                this.SetFunctionLabel(EProMode.PRINT);
                this.InitialControlArray();
                base.Btn_F9.Text = "";
                base.Btn_F9.Enabled = false;
                base.Btn_F10.Text = "";     //CSVなし？？
                
                //起動時共通処理
                base.StartProgram();

                tabl = new Tanaoroshi_BL();
                string ymd = tabl.GetDate();

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = ymd
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);

                CboSoukoCD.Bind(ymd, InOperatorCD);

                SetFuncKeyAll(this, "100001000011");
                Scr_Clr(0);

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
            detailControls = new Control[] { CboSoukoCD, ckM_TextBox1, ckM_RadioButton2,ckM_RadioButton1};
            detailLabels = new Control[] { };
            searchButtons = new Control[] {};

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
            

        }
        
        /// <summary>
        /// 棚卸表データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private DataTable CheckData()
        {
            for (int i = 0; i < detailControls.Length; i++)
            {
                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return null;
                }
            }

            //[D_Inventory_SelectForPrint]
            die = GetSearchInfo();

            DataTable dt = tabl.D_Inventory_SelectForPrint(die);

            //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E133");
                previousCtrl.Focus();
                return null;
            }

            return dt;
        }
        protected override void PrintSec()
        {
            // レコード定義を行う
            DataTable table = CheckData();

            try
            {
                if (table == null)
                {
                    return;
                }
                //xsdファイルを保存します。

                //DB　---→　xsd　----→　クリスタルレポート

                //というデータの流れになります
                //table.TableName = ProID;
                //table.WriteXmlSchema("DataTable" + ProID + ".xsd");

                //①保存した.xsdはプロジェクトに追加しておきます。
                DialogResult ret;
                TanaoroshiHyou_Report Report = new TanaoroshiHyou_Report();

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
                        Report.SetDataSource(table);
                        Report.Refresh();

                        if (ret == DialogResult.Yes)
                        {
                            //プレビュー
                            var previewForm = new Viewer1();
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
                        Report.SetDataSource(table);
                        Report.Refresh();

                        bool result = ChildOutputPDF(filePath, Report);
                        
                        //PDF出力が完了しました。
                        bbl.ShowMessage("I202");

                        break;
                }
                
                //プログラム実行履歴
                InsertLog(Get_L_Log_Entity());
            }
            finally
            {
               
            }

            //更新後画面そのまま
            detailControls[0].Focus();
        }
        public bool ChildOutputPDF(string filePath, ReportClass report)
        {
            // PDF形式でファイル出力
            try
            {
                string fileName = "";
                if (System.IO.Path.GetExtension(filePath).ToLower() != ".pdf")
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(filePath) + ".pdf";
                }

                // 出力先ファイル名を指定
                CrystalDecisions.Shared.DiskFileDestinationOptions fileOption;
                fileOption = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                fileOption.DiskFileName = System.IO.Path.GetDirectoryName(filePath) + "\\" + fileName;

                // 外部ファイル出力をPDF出力として定義する
                CrystalDecisions.Shared.ExportOptions option;
                option = report.ExportOptions;
                option.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                option.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                option.FormatOptions = new CrystalDecisions.Shared.PdfRtfWordFormatOptions();
                option.DestinationOptions = fileOption;

                // pdfとして外部ファイル出力を行う
                report.Export();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return false;
            }
            return true;
        }
        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set=true)
        {
            switch (index)
            {
                case (int)EIndex.InventoryDate:

                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    string strYmd = "";
                    strYmd = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    detailControls[index].Text = strYmd;

                    break;

                case (int)EIndex.SoukoCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }

                    break;
            }

            return true;
        }

        private D_Inventory_Entity GetSearchInfo()
        {
            die = new D_Inventory_Entity
            {
                InventoryDate = detailControls[(int)EIndex.InventoryDate].Text,
                SoukoCD = CboSoukoCD.SelectedValue.ToString(),
            };

            if (ckM_RadioButton1.Checked)
                die.ChkKinyu = 1;
            else
                die.ChkKinyu = 2;

            die.KbnSai = 0;
            die.InsertOperator = InOperatorCD;
            die.PC = InPcID;

            return die;
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_RadioButton)))
                {
                    ckM_RadioButton2.Checked = true;
                }
                else if (ctl.GetType().Equals(typeof(Panel)))
                {
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex=-1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CKM_Controls.CKM_CheckBox)ctl).Checked = false;
                }
                else
                {
                    ctl.Text = "";
                }
            }

            foreach (Control ctl in detailLabels)
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }


            //初期値セット
            string ymd = tabl.GetDate();

            ////スタッフマスター(M_Staff)に存在すること
            ////[M_Staff]
            //M_Staff_Entity mse = new M_Staff_Entity
            //{
            //    StaffCD = InOperatorCD,
            //    ChangeDate = tabl.GetDate()
            //};
            //Staff_BL bl = new Staff_BL();
            //bool ret = bl.M_Staff_Select(mse);
            ////if (ret)
            ////{
            ////    CboSoukoCD.SelectedValue = mse.StoreCD;
            ////}

            ////[M_Store]
            //M_Store_Entity mse2 = new M_Store_Entity
            //{
            //    StoreCD = mse.StoreCD,
            //    ChangeDate = ymd
            //};
            //Store_BL sbl = new Store_BL();
            //DataTable dt = sbl.M_Store_Select(mse2);
            //if (dt.Rows.Count > 0)
            //{
            //}
            //else
            //{
            //    bbl.ShowMessage("E133");
            //    EndSec();
            //}

            //初期値
            if (CboSoukoCD.Items.Count > 1)
                CboSoukoCD.SelectedIndex = 1;

            detailControls[(int)EIndex.InventoryDate].Text = ymd;

            detailControls[0].Focus();
        }

        /// <summary>
        /// get Log information
        /// print log
        /// </summary>
        private L_Log_Entity Get_L_Log_Entity()
        {
            L_Log_Entity lle = new L_Log_Entity();
            string item = detailControls[0].Text;
            for (int i = 1; i <= (int)EIndex.InventoryDate; i++)
            {
                item += "," + detailControls[i].Text;
            }
            if (ckM_RadioButton2.Checked)
                item += "," + ckM_RadioButton2.Text;
            else
                item += "," + ckM_RadioButton1.Text;

            lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "",
                KeyItem = item
            };

            return lle;
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
                case 0: // F1:終了
                    {
                        break;
                    }
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

                case 11:   
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
                        if(index == (int)EIndex.Rdo2　|| index == (int)EIndex.Rdo1)
                        {
                            this.ProcessTabKey(!e.Shift);
                        }
                        else if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                        }
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
                
                //SetFuncKeyAll(this, "111111000011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}








