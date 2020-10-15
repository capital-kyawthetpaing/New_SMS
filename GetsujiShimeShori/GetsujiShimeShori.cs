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

namespace GetsujiShimeShori
{
    /// <summary>
    /// GetsujiShimeShori 月次締処理
    /// </summary>
    internal partial class GetsujiShimeShori : FrmMainForm
    {
        private const string ProID = "GetsujiShimeShori";
        private const string ProNm = "月次締処理";
        private const string GetsujiSaikenKeisanSyori = "GetsujiSaikenKeisanSyori.exe";
        private const string GetsujiSaimuKeisanSyori = "GetsujiSaimuKeisanSyori.exe";
        private const string GetsujiZaikoKeisanSyori = "GetsujiZaikoKeisanSyori.exe";
        private const string GetsujiShiireKeisanSyori = "GetsujiShiireKeisanSyori.exe";


        private enum EIndex : int
        {
            StoreCD,

            Uriage=0,
            Nyukin,
            Shiire,
            Shiharai,
            Zaiko,

            Saiken,
            Saimu,
            ZaikoShukei,
            All,

            Kakutei,
            Kaijo,

            COUNT
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] detailButtons;

        private GetsujiShimeShori_BL gsbl;
        private int Mode;
        private string InStoreCD;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
                
        public GetsujiShimeShori()
        {
            InitializeComponent();
                    }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM=ProNm;

                this.SetFunctionLabel(EProMode.BATCH);
                this.InitialControlArray();
                Btn_F12.Text = "";

                //起動時共通処理
                base.StartProgram();

                gsbl = new GetsujiShimeShori_BL();

                SetFuncKeyAll(this, "100001000001");
                Scr_Clr(0);

                //処理モードを覚える
                M_Control_Entity mce = new M_Control_Entity();
                mce.MainKey = "1";
                Mode= gsbl.GetMode(mce);

                if (Mode.Equals(1))
                {
                    //Mode＝1の場合 全社モード（店舗を選ぶことができない）
                    //画面はLable表示
                    CboStoreCD.Visible = false;
                    lblStore.Visible = true;
                }
                else
                {
                    //Mode＝2の場合 店舗モード（店舗は選ぶことができる）
                    CboStoreCD.Visible = true;
                    lblStore.Visible = false;
                    string ymd = gsbl.GetDate();
                    //画面はListBox表示
                    CboStoreCD.Bind(ymd, InOperatorCD);

                    //コマンドライン引数を配列で取得する
                    string[] cmds = System.Environment.GetCommandLineArgs();

                    if (cmds.Length-1 == (int)FrmMainForm.ECmdLine.PcID + 1)
                    {
                        InStoreCD = cmds[(int)FrmMainForm.ECmdLine.PcID + 1];
                        CboStoreCD.SelectedValue = InStoreCD;
                    }
                    else
                    {
                        CboStoreCD.SelectedValue = StoreCD;
                    }
                }

                //画面転送表00に従って、画面表示
                ExecDisp();

                //Mode		＝	2	の場合																			
                //レコ―ドが無い場合、 テーブル転送仕様Ａに従ってInsert M_StoreClose

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }    
        protected override void ExecDisp()
        {
            //if (CboStoreCD.SelectedValue == null)
            //    return;

            M_Store_Entity mse = new M_Store_Entity();
            mse.ChangeDate = gsbl.GetDate();
            mse.StoreCD = CboStoreCD.SelectedIndex > 0 ? CboStoreCD.SelectedValue.ToString() : "";

            if (Mode.Equals(1))
            {
                mse.StoreCD = gsbl.GetHonsha(mse);
            }

            Store_BL sbl = new Store_BL();
            DataTable dt = sbl.M_Store_Select(mse);
            if (dt.Rows.Count > 0)
            {
                lblChangeDate.Text = dt.Rows[0]["FiscalYYYYMM"].ToString().Substring(0, 4) + "/" + dt.Rows[0]["FiscalYYYYMM"].ToString().Substring(4, 2); 
            }

            M_StoreClose_Entity me = new M_StoreClose_Entity
            {
                StoreCD = CboStoreCD.SelectedIndex > 0 ?  CboStoreCD.SelectedValue.ToString() : "",
                FiscalYYYYMM = lblChangeDate.Text.Replace("/",""),
                Operator = InOperatorCD,
                Mode=Mode
            };

            //if (Mode.Equals(1))
            //{
            //    me.StoreCD = gsbl.GetHonsha(mse);
            //}

            bool  ret = gsbl.M_StoreClose_SelectAll(me);

            if(!ret)
            {
                //Insertしたあとに再度Select
                 ret = gsbl.M_StoreClose_SelectAll(me);
            }
            if (!string.IsNullOrWhiteSpace(me.FiscalYYYYMM))
            {
                lblChangeDate.Text = me.FiscalYYYYMM.Substring(0, 4) + "/" + me.FiscalYYYYMM.Substring(4, 2);
                lblKakutei.Text = lblChangeDate.Text;
            }
            SetProperty(me.ClosePosition1, lblUriage, BtnUriage);
            SetProperty(me.ClosePosition3, lblNyukin, BtnNyukin);
            SetProperty(me.ClosePosition2, lblShiire, BtnShiire);
            SetProperty(me.ClosePosition4, lblShiharai, BtnShiharai);
            SetProperty(me.ClosePosition5, lblZaiko, BtnZaiko);

            SetPropertyShukei(me.MonthlyClaimsFLG, lblSaiken, BtnSaiken);
            SetPropertyShukei(me.MonthlyDebtFLG, lblSaimu, BtnSaimu);
            SetPropertyShukei(me.MonthlyStockFLG, lblZaikoShukei, BtnZaikoShukei);

            BtnAllShukei.Text = "まとめて\n計算する";
            BtnKakutei.Text = "確定する(翌月度へ)";
            BtnKaijo.Text = "解除する(前月度へ)";

            if (me.ClosePosition1.Equals("1") && me.ClosePosition2.Equals("1") && me.ClosePosition3.Equals("1") && me.ClosePosition4.Equals("1") && me.ClosePosition5.Equals("1")
                    && me.MonthlyClaimsFLG.Equals("1") && me.MonthlyDebtFLG.Equals("1") && me.MonthlyStockFLG.Equals("1"))
                {
                BtnKakutei.BackColor = Color.FromArgb(255, 255, 0);
                BtnKakutei.Enabled = true;
                BtnKaijo.Enabled = true;
            }
            else
            {
                BtnKakutei.BackColor = Color.Silver;
                BtnKakutei.Enabled = false;
                BtnKaijo.Enabled = true;
            }
        }

        private void SetProperty(string position, Label label, Button button)
        {
            if (position.Equals("0"))
            {
                label.Text = "入力可";
                label.BackColor = Color.FromArgb(128, 255, 255);
                button.Enabled = true;
                button.Text = "仮締する";
            }
            else if (position.Equals("1"))
            {
                label.Text = "仮締中(入力不可)";
                label.BackColor = Color.FromArgb(255, 255, 0);
                button.Enabled = true;
                button.Text = "仮締キャンセルする";
            }
            else
            {
                label.Text = "締完了(入力不可)";
                label.BackColor = Color.FromArgb(255, 255, 0);
                button.Enabled = false;
                button.Text = "締完了済";
            }
        }
        private void SetPropertyShukei(string flg, Label label, Button button)
        {
            if (flg.Equals("0"))
            {
                label.Text = "必要";
                label.BackColor = Color.FromArgb(255, 192, 0);
                button.Enabled = true;
                button.Text = "計算する";
            }
            else if (flg.Equals("1"))
            {
                label.Text = "完了";
                label.BackColor = Color.FromArgb(255, 255, 0);
                button.Enabled = true;
                button.Text = "計算する";
            }
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { CboStoreCD};
            detailLabels = new Control[] { lblUriage,lblNyukin,lblShiire,lblShiharai,lblZaiko,lblSaiken,lblSaimu,lblZaikoShukei,lblKakutei };
            detailButtons = new Control[] { BtnUriage, BtnNyukin, BtnShiire, BtnShiharai, BtnZaiko, BtnSaiken, BtnSaimu, BtnZaikoShukei, BtnAllShukei, BtnKakutei, BtnKaijo };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
            foreach (Control ctl in detailButtons)
            {
                ctl.Click += new System.EventHandler(DetailBtn_Click);
            }

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
                case (int)EIndex.StoreCD:
                    if (CboStoreCD.SelectedValue.Equals("-1"))
                    {
                        bbl.ShowMessage("E102");
                        CboStoreCD.Focus();
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
                        ExecDisp();
                    }

                    break;
            }

            return true;
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
                ctl.Text = "";
            }

            foreach (Control ctl in detailButtons)
            {
                ctl.Text = "";
            }
            
            detailControls[0].Focus();
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

                int index = Array.IndexOf(detailControls, sender);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void CboStoreCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CboStoreCD.SelectedIndex > 0)
                    CheckDetail((int)EIndex.StoreCD);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void DetailBtn_Click(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int index = Array.IndexOf(detailButtons, sender);

                if (index <= (int)EIndex.All)
                    SubBtnClick(index);
                else
                {
                    if (index == (int)EIndex.Kakutei)
                    {
                        if (bbl.ShowMessage("Q323") != DialogResult.Yes)
                            return;
                    }
                    else
                    {
                        if (bbl.ShowMessage("Q322") != DialogResult.Yes)
                            return;
                    }
                    SubBtnClickKakutei(index);
                }
                //画面更新
                ExecDisp();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void SubBtnClick(int index)
        {
            int[] valC = { 0, 0, 0, 0, 0 };
            string OperateModeNm = "";
            bool ret = false;

            switch (index)
            {
                case (int)EIndex.Uriage:
                case (int)EIndex.Shiire:
                case (int)EIndex.Nyukin:
                case (int)EIndex.Shiharai:
                case (int)EIndex.Zaiko:
                    if (detailButtons[index].Text.Equals("仮締する"))
                    {
                        //①画面を変更⇒
                        detailLabels[index].Text = "仮締中(入力不可)";
                        detailLabels[index].BackColor = Color.FromArgb(255, 255, 0);
                        detailButtons[index].Enabled = true;
                        detailButtons[index].Text = "仮締キャンセルする";
                        valC[index] = 1;
                        OperateModeNm = "仮締 月次仮締・";
                    }
                    else
                    {
                        //①画面を変更⇒		
                        detailLabels[index].Text = "入力可";
                        detailLabels[index].BackColor = Color.FromArgb(255, 192, 0);
                        detailButtons[index].Enabled = true;
                        detailButtons[index].Text = "仮締する";
                        valC[index] = 0;
                        OperateModeNm = "キャンセル 月次仮締・";
                    }

                    switch (index)
                    {
                        case (int)EIndex.Uriage:
                            OperateModeNm += "売上";
                            break;
                        case (int)EIndex.Shiire:
                            OperateModeNm += "仕入";
                            break;
                        case (int)EIndex.Nyukin:
                            OperateModeNm += "入金";
                            break;
                        case (int)EIndex.Shiharai:
                            OperateModeNm += "支払";
                            break;
                        case (int)EIndex.Zaiko:
                            OperateModeNm += "在庫";
                            break;
                    }
                    break;

                case (int)EIndex.Saiken:
                case (int)EIndex.Saimu:
                case (int)EIndex.ZaikoShukei:

                    OperateModeNm = "集計 月次集計・";
                    switch (index)
                    {
                        case (int)EIndex.Saiken:
                            OperateModeNm += "債権";
                            //②計算処理を実行
                            //GetsujiSaikenKeisanSyori.exe を起動する
                        ret=    ExecSaikenSaimu(GetsujiSaikenKeisanSyori);
                            break;
                        case (int)EIndex.Saimu:
                            OperateModeNm += "債務";
                            //②計算処理を実行
                            //GetsujiSaimuKeisanSyori.exe を起動する
                            ret=ExecSaikenSaimu(GetsujiSaimuKeisanSyori);
                            break;
                        case (int)EIndex.ZaikoShukei:
                            OperateModeNm += "在庫";
                            //②計算処理を実行
                            //GetsujiZaikoKeisanSyori.exe を起動する
                            ret=ExecSaikenSaimu(GetsujiZaikoKeisanSyori);
                            if (!ret)
                                return;
                            //GetsujiShiireKeisanSyori.exe   を起動する
                            ret = ExecSaikenSaimu(GetsujiShiireKeisanSyori);
                            break;
                    }
                    if (!ret)
                        return;

                    //①画面を変更⇒
                    detailLabels[index].Text = "計算中";
                    detailLabels[index].BackColor = Color.White;
                    detailButtons[index].Enabled = false;
                    detailButtons[index].Text = "計算する";
                    break;

                case (int)EIndex.All:
                    SubBtnClick((int)EIndex.Saiken);
                    SubBtnClick((int)EIndex.Saimu);
                    SubBtnClick((int)EIndex.ZaikoShukei);

                    break;
            }
            //②Table更新
            //OperateModeNm:仮締 or キャンセル or 集計 or 確定 or 解除
            //月次仮締・売上,月次仮締・入金,月次仮締・仕入,月次仮締・支払,月次仮締・在庫,月次集計・債権,月次処理・債務,月次処理・在庫,月次確定
            M_StoreClose_Entity me = new M_StoreClose_Entity
            {
                StoreCD = CboStoreCD.SelectedValue == null ? "" : CboStoreCD.SelectedValue.ToString(),
                FiscalYYYYMM = lblChangeDate.Text.Replace("/", ""),
                ClosePosition1 = valC[0].ToString(),    //売上
                ClosePosition2 = valC[2].ToString(),    //仕入
                ClosePosition3 = valC[1].ToString(),    //入金
                ClosePosition4 = valC[3].ToString(),    //支払
                ClosePosition5 = valC[4].ToString(),    //在庫
                Mode = Mode,
                Kbn = index + 1,
                Operator = InOperatorCD,
                PC = InPcID,
                OperateModeNm = OperateModeNm
            };

            ret = gsbl.M_StoreClose_Update(me);
        }
        private void SubBtnClickKakutei(int index)
        {
            string OperateModeNm = "";
            string FiscalYYYYMM = lblChangeDate.Text;
            switch (index)
            {
                case (int)EIndex.Kakutei:
                    //「確定する(翌月度へ)」
                    //M_StoreClose					テーブル転送仕様Ｃに従ってUpdate								
                    OperateModeNm = "確定 月次確定";
                    FiscalYYYYMM = Convert.ToDateTime(FiscalYYYYMM + "/01").AddMonths(1).ToString("yyyyMM");
                    break;
                case (int)EIndex.Kaijo:
                    //「解除する(前月度へ)」
                    //M_StoreClose					テーブル転送仕様Ｄに従ってUpdate										
                    OperateModeNm = "解除 月次確定";
                    FiscalYYYYMM = FiscalYYYYMM.Replace("/", "");
                    break;
            }

            M_StoreClose_Entity me = new M_StoreClose_Entity
            {
                StoreCD = CboStoreCD.SelectedValue == null ? "" : CboStoreCD.SelectedValue.ToString(),
                FiscalYYYYMM = FiscalYYYYMM,
                ClosePosition1 = "0",    //売上
                ClosePosition2 = "0",    //仕入
                ClosePosition3 = "0",    //入金
                ClosePosition4 = "0",    //支払
                ClosePosition5 = "0",    //在庫
                Mode = Mode,
                Kbn = index + 1,
                Operator = InOperatorCD,
                PC = InPcID,
                OperateModeNm = OperateModeNm
            };

            bool ret = gsbl.M_StoreClose_Update(me);

            //更新後の表示処理のため
            switch (index)
            {
                case (int)EIndex.Kakutei:
                    lblChangeDate.Text = FiscalYYYYMM.Substring(0, 4) + "/" + FiscalYYYYMM.Substring(4, 2);
                    break;
                case (int)EIndex.Kaijo:
                    FiscalYYYYMM = Convert.ToDateTime(FiscalYYYYMM.Substring(0, 4) + "/" + FiscalYYYYMM.Substring(4, 2) + "/01").AddMonths(-1).ToString("yyyyMM");
                    lblChangeDate.Text = FiscalYYYYMM.Substring(0, 4) + "/" + FiscalYYYYMM.Substring(4, 2);
                    break;
            }
        }
        private bool ExecSaikenSaimu(string exe)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                          
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + exe;
                if (System.IO.File.Exists(filePath))
                {
                    string StoreCD = CboStoreCD.SelectedIndex > 0 ? CboStoreCD.SelectedValue.ToString() : "ALL";
                    string ProcessMode = "1";
                    string FiscalYYYYMM =lblChangeDate.Text;

                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + StoreCD + " " + ProcessMode + " " + FiscalYYYYMM;
                    //System.Diagnostics.Process p = System.Diagnostics.Process.Start(filePath, cmdLine);
                    System.Diagnostics.ProcessStartInfo psInfo = new System.Diagnostics.ProcessStartInfo();
                    psInfo.FileName = filePath; // 実行するファイル
                    psInfo.Arguments = cmdLine;
                    psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
                    psInfo.UseShellExecute = false; // シェル機能を使用しない
                    System.Diagnostics.Process p = System.Diagnostics.Process.Start(psInfo);

                    p.WaitForExit();

                    this.Cursor = Cursors.Default;
                    return true;
                }
                else
                {
                    //ファイルなし
                    bbl.ShowMessage("E120");
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

            this.Cursor = Cursors.Default;
            return false;
        }
    }
}








