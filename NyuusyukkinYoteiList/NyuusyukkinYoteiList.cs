using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;

namespace NyuusyukkinYoteiList
{
    /// <summary>
    /// NyuusyukkinYoteiList 入出金予定表印刷
    /// </summary>
    internal partial class NyuusyukkinYoteiList : FrmMainForm
    {
        private const string ProID = "NyuusyukkinYoteiList";
        private const string ProNm = "入出金予定表印刷";

        private enum EIndex : int
        {
            StoreCD,
            DayStart,
            DayEnd,
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private NyuusyukkinYoteiList_BL mibl;
        private D_PaymentConfirm_Entity dpe;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        
        private string mOldCustomerCD = "";

        private string StartUpKBN = "";

        public NyuusyukkinYoteiList()
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
                base.Btn_F10.Text = "";
                base.Btn_F11.Text = "";

                //起動時共通処理
                base.StartProgram();

                mibl = new NyuusyukkinYoteiList_BL();
                CboStoreCD.Bind(string.Empty);

                SetFuncKeyAll(this, "100001000001");
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
            detailControls = new Control[] { CboStoreCD, ckM_TextBox1, ckM_TextBox2 };
            detailLabels = new Control[] {};
            searchButtons = new Control[] {};

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
        
        /// <summary>
        /// 入出金予定表印刷データ取得処理
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

                dpe= GetSearchInfo();

            DataTable dt = mibl.D_PaymentConfirm_SelectForPrint(dpe);

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
                NyuusyukkinYoteiList_Report Report = new NyuusyukkinYoteiList_Report();

                //DataTableのDetailOnが１かどうかで詳細セクションを印字するかどうかの設定を
                //している（セクションエキスパート）

                switch (PrintMode)
                {
                    case EPrintMode.DIRECT:
                        if (StartUpKBN == "1")
                        {
                            ret = DialogResult.No;
                        }
                        else { 
                            //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                            ret = bbl.ShowMessage("Q208");
                            if (ret == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                        Report.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        Report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                        // 印字データをセット
                        Report.SetDataSource(table);
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
                        Report.SetDataSource(table);
                        Report.Refresh();

                        bool result = OutputPDF(filePath, Report);
                        
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

        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set=true)
        {
            bool ret;
            switch (index)
            {
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                    //入力必須(Entry required)	入力なければError(If there is no input, an error)			
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        mibl.ShowMessage("E102");
                        return false;
                    }

                    string  strYmd = bbl.FormatDate(detailControls[index].Text + "/01");

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!mibl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        mibl.ShowMessage("E103");
                        return false;
                    }
                    detailControls[index].Text = strYmd.Substring(0, 7);

                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.DayEnd )
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                mibl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }
                    

                    break;        

                case (int)EIndex.StoreCD:
                    if (CboStoreCD.SelectedValue == null)
                    {
                        return true;
                    }
                    else
                    {
                        if(!string.IsNullOrWhiteSpace(CboStoreCD.SelectedText))
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            bbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }
                        return true;
                    }

                    break;
            }

            return true;
        }

        private D_PaymentConfirm_Entity GetSearchInfo()
        {
            dpe = new D_PaymentConfirm_Entity
            {
                InputDateFrom = detailControls[(int)EIndex.DayStart].Text,
                InputDateTo = detailControls[(int)EIndex.DayEnd].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),            
            };

            return dpe;
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
            string ymd = mibl.GetDate();

            //スタッフマスター(M_Staff)に存在すること
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = mibl.GetDate()
            };
            Staff_BL bl = new Staff_BL();
            bool ret = bl.M_Staff_Select(mse);
            if (ret)
            {
                CboStoreCD.SelectedValue = mse.StoreCD;
            }

            //[M_Store]
            M_Store_Entity mse2 = new M_Store_Entity
            {
                StoreCD = mse.StoreCD,
                ChangeDate = ymd
            };
            Store_BL sbl = new Store_BL();
            DataTable dt = sbl.M_Store_Select(mse2);
            if (dt.Rows.Count > 0)
            {
            }
            else
            {
                bbl.ShowMessage("E133");
                EndSec();
            }

            //detailControls[(int)EIndex.DayStart].Text = ymd;
            //detailControls[(int)EIndex.DayEnd].Text = ymd;

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
            for (int i = 1; i <= (int)EIndex.DayEnd; i++)
            {
                item += "," + detailControls[i].Text;
            }

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
                        if(index == (int)EIndex.DayEnd)
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








