using System;
using System.Data;
using System.Windows.Forms;
using BL;
using Entity;
using Base.Client;
using Search;
using CrystalDecisions.CrystalReports.Engine;

namespace KaitounoukiKakuninsho
{
    /// <summary>
    /// KaitounoukiKakuninsho 回答納期確認書
    /// </summary>
    internal partial class KaitounoukiKakuninsho : FrmMainForm
    {
        private const string ProID = "KaitounoukiKakuninsho";
        private const string ProNm = "回答納期確認書";

        private enum EIndex : int
        {
            StoreCD,
            ArrivalPlanDateFrom,
            ArrivalPlanDateTo,
            ArrivalPlanMonthFrom,
            ArrivalPlanMonthTo,
            OrderDateFrom,
            OrderDateTo,

            ChkMikakutei,
            ArrivalPlanCD,
            CheckBox3,
            CheckBox4,
            OrderCD,
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private KaitouNoukiTouroku_BL mibl;
        private D_Order_Entity doe;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldMitsumoriNo = "";    //排他処理のため使用
        private string mOldCustomerCD = "";


        public KaitounoukiKakuninsho()
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
                base.Btn_F10.Text = "";
                
                //起動時共通処理
                base.StartProgram();    

                mibl = new  KaitouNoukiTouroku_BL();
                CboStoreCD.Bind(string.Empty);
                CboSoukoName.Bind(string.Empty);

                ScOrderCD.Value1 = "1";

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
            detailControls = new Control[] { CboStoreCD, ckM_TextBox1 ,ckM_TextBox2 ,ckM_TextBox3,ckM_TextBox4,ckM_TextBox5,ckM_TextBox6
                    ,ChkMikakutei, CboSoukoName,ChkKanbai,ChkFuyo, ScOrderCD.TxtCode
                         };
            detailLabels = new Control[] { ScOrderCD };
            searchButtons = new Control[] { ScOrderCD.BtnSearch};

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
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
        private bool SelectAndInsertExclusive(string mitsumoriNo)
        {
            //DeleteExclusive();

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Mitsumori,
                Number = mitsumoriNo,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(),dt.Rows[0]["Operator"].ToString());
                //detailControls[(int)EIndex.MitsumoriNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldMitsumoriNo = mitsumoriNo;
                return ret;
            }
        }
   
        /// <summary>
        /// 回答納期データ取得処理
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
            //入荷予定日 （To)							
            //入荷予定月 （To)							
            //発注日     （To)							
            //のどれかの入力があること
            //カーソルは               入荷予定日（To)		
            if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ArrivalPlanDateTo].Text) && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ArrivalPlanMonthTo].Text)
                && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.OrderDateTo].Text))
            {
                //Ｅ１８０
                bbl.ShowMessage("E180");
                detailControls[(int)EIndex.ArrivalPlanDateTo].Focus();
                return null;
            }

            //[D_ArrivalPlan_SelectForPrint]
            doe = GetEntity();

            DataTable dt = mibl.D_ArrivalPlan_SelectForPrint(doe);

            //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E128");
                previousCtrl.Focus();
                return null;
            }

            return dt;
        }
        protected override void PrintSec()
        {
            // レコード定義を行う
            DataTable table = CheckData();

                if (table == null)
                {
                    return;
                }
                //xsdファイルを保存します。

                //DB　---→　xsd　----→　クリスタルレポート

                //というデータの流れになります
                //table.TableName = ProID;
                //table.WriteXmlSchema(ProID + ".xsd");

                //①保存した.xsdはプロジェクトに追加しておきます。
                DialogResult ret;
                KaitounoukiKakuninsho_Report Report = new KaitounoukiKakuninsho_Report();

                switch (PrintMode)
                {
                    case EPrintMode.DIRECT:

                        //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                        ret = bbl.ShowMessage("Q208");
                        if (ret == DialogResult.Cancel)
                        {
                            return;
                        }

                        // 印字データをセット
                        Report.SetDataSource(table);
                        Report.Refresh();

                        if (ret == DialogResult.Yes)
                        {
                            //プレビュー
                            var previewForm = new Viewer1();
                            previewForm.CrystalReportViewer1.ShowPrintButton = true;
                            previewForm.CrystalReportViewer1.ReportSource = Report;
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
                    try
                    {
                        bool result = ChildOutputPDF(filePath, Report);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                        return;
                    }
                    //PDF出力が完了しました。
                    bbl.ShowMessage("I202");

                        break;
                }

            //ログファイルへの更新
            bbl.L_Log_Insert(Get_L_Log_Entity());

            //更新後画面そのまま
            detailControls[0].Focus();
        }

        /// <summary>
        /// get Log information
        /// print log
        /// </summary>
        private L_Log_Entity Get_L_Log_Entity()
        {
            string item = detailControls[0].Text;
            for (int i = 1; i <= (int)EIndex.OrderDateTo; i++)
            {
                item += "," + detailControls[i].Text;
            }
            if (ChkMikakutei.Checked)
                item += "," + ChkMikakutei.Text;
            if(!string.IsNullOrWhiteSpace( CboSoukoName.Text))
                item += "," + CboSoukoName.SelectedValue.ToString();
            if (ChkKanbai.Checked)
                item += "," + ChkKanbai.Text;
            if (ChkFuyo.Checked)
                item += "," + ChkFuyo.Text;

            item += "," + detailControls[(int)EIndex.OrderCD].Text;

            L_Log_Entity lle = new L_Log_Entity
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
                case (int)EIndex.OrderDateFrom:
                case (int)EIndex.OrderDateTo:
                case (int)EIndex.ArrivalPlanDateFrom:
                case (int)EIndex.ArrivalPlanDateTo:
                case (int)EIndex.ArrivalPlanMonthFrom:
                case (int)EIndex.ArrivalPlanMonthTo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    string strYmd = "";
                    switch (index)
                    {
                        case (int)EIndex.ArrivalPlanMonthFrom:
                        case (int)EIndex.ArrivalPlanMonthTo:
                            strYmd = bbl.FormatDate(detailControls[index].Text + "/01");

                            break;
                        default:
                            strYmd = bbl.FormatDate(detailControls[index].Text);
                            break;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    //if (!bbl.CheckDate(strYmd))
                    //{
                    //    //Ｅ１０３
                    //    bbl.ShowMessage("E103");
                    //    return false;
                    //}

                    //switch (index)
                    //{
                    //    case (int)EIndex.ArrivalPlanMonthFrom:
                    //    case (int)EIndex.ArrivalPlanMonthTo:
                    //        detailControls[index].Text = strYmd.Substring(0, 7);
                    //        break;
                    //    default:
                    //        detailControls[index].Text = strYmd;
                    //        break;
                    //}

                    //見積日(From) ≧ 見積日(To)である場合Error
                    if (index == (int)EIndex.OrderDateTo || index == (int)EIndex.ArrivalPlanDateTo || index == (int)EIndex.ArrivalPlanMonthTo)
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
                    //発注日のどちらかに入力があった場合に、未確定分、完売、不要のチェックボックスの入力を可能にする																											
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.OrderDateFrom].Text) || !string.IsNullOrWhiteSpace(detailControls[(int)EIndex.OrderDateTo].Text))
                    {
                        ChkMikakutei.Enabled = true;
                        ChkKanbai.Enabled = true;
                        ChkFuyo.Enabled = true;
                    }

                    break;

                case (int)EIndex.OrderCD:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //情報ALLクリア
                        ClearCustomerInfo();
                        return true;
                    }

                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        VendorFlg = "1",
                        ChangeDate = bbl.GetDate()
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    ret = sbl.M_Vendor_SelectTop1(mve);

                    if (ret)
                    {
                        if (mve.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            //顧客情報ALLクリア
                            ClearCustomerInfo();
                            return false;
                        }
                        ScOrderCD.LabelText = mve.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }

                    break;

                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    else
                    {
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            bbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }

                    }

                    break;
            }

            return true;
        }
        /// <summary>
        /// 仕入先情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            ScOrderCD.LabelText = "";
        }
        private D_Order_Entity GetEntity()
        {
            doe = new D_Order_Entity
            {
                ArrivalPlanDateFrom = detailControls[(int)EIndex.ArrivalPlanDateFrom].Text,
                ArrivalPlanDateTo = detailControls[(int)EIndex.ArrivalPlanDateTo].Text,
                ArrivalPlanMonthFrom = detailControls[(int)EIndex.ArrivalPlanMonthFrom].Text.Replace("/", ""),
                ArrivalPlanMonthTo = detailControls[(int)EIndex.ArrivalPlanMonthTo].Text.Replace("/", ""),
                OrderDateFrom = detailControls[(int)EIndex.OrderDateFrom].Text,
                OrderDateTo = detailControls[(int)EIndex.OrderDateTo].Text,

                StoreCD = CboStoreCD.SelectedValue.ToString(),
                OrderCD = detailControls[(int)EIndex.OrderCD].Text,
            };

            if (ChkMikakutei.Checked)
                doe.ChkMikakutei = 1;
            else
                doe.ChkMikakutei = 0;

            if (ChkKanbai.Checked)
                doe.ChkKanbai = 1;
            else
                doe.ChkKanbai = 0;

            if (ChkFuyo.Checked)
                doe.ChkFuyo = 1;
            else
                doe.ChkFuyo = 0;

            return doe;
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

            CboSoukoName.Enabled = false;
            detailControls[1].Focus();
        }

        private void Scr_Lock(short no1, short no2, short Kbn)
        {
            short i;
            for (i = no1; i <= no2; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            // ｷｰ部
                            break;
                        }

                    case 1:
                        {
                            // ｷｰ部(複写)
                            break;
                        }

                    case 2:
                        {
                            break;
                        }

                    case 3:
                        {
                            // 明細部
                            foreach (Control ctl in detailControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            ScOrderCD.BtnSearch.Enabled = Kbn == 0 ? true : false;

                            break;
                        }
                }
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
                        if (detailControls.Length - 1 > index)
                        {
                            if(index== (int)EIndex.OrderCD)
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                            else if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                        }
                        else
                        {
                            detailControls[0].Focus();
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

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int index = Array.IndexOf(detailControls, sender);
                switch(index)
                    {
                    case (int)EIndex.OrderCD:
                        F9Visible = true;
                        break;

                    default:
                        F9Visible = false;
                        break;
                }
                //SetFuncKeyAll(this, "111111000011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void ChkMikakutei_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //チェックがONのときのみ入力可
                CboSoukoName.Enabled = ChkMikakutei.Checked;
                if (!ChkMikakutei.Checked)
                    CboSoukoName.SelectedIndex = -1;

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








