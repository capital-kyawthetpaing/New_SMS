using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using System.IO;
using ClosedXML.Excel;

namespace WebJuchuuKakunin
{
    /// <summary>
    /// WebJuchuuKakunin WEB受注確認
    /// </summary>
    internal partial class WebJuchuuKakunin : FrmMainForm
    {
        private const string ProID = "WebJuchuuKakunin";
        private const string ProNm = "WEB受注確認";
        private const string WebJuchuu = "WebJuchuu.exe";//WEB受注入力
        private const string Nayose = "NayoseSyoriAll.exe";//名寄せ処理_要注意顧客
        private const string JuchuuTorikomi = "JuchuuTorikomi.exe";//受注取込
        
        private enum EIndex : int
        {
            CboJuchuu,
            CboHacchuu,
            CboShukka,
            CboShukkaJ,
            StoreCD,
            CustomerKanaName,
            Tel,
            CustomerCD,
            
            JuchuuNOFrom,
            JuchuuNOTo,
            SiteJuchuuDateFrom,
            SiteJuchuuDateTo,
            JuchuuDateFrom,
            JuchuuDateTo,
            OrderDateFrom,
            OrderDateTo,
            NyukinDateFrom,
            NyukinDateTo,

            DecidedDeliveryDateFrom,
            DecidedDeliveryDateTo,
            DeliveryPlanDateFrom,
            DeliveryPlanDateTo,
            DeliveryDateFrom,
            DeliveryDateTo,
            InvoiceNOFrom,
            InvoiceNOTo,

            SiteJuchuuNO,
            CboSyubetsu8,
            CboSyubetsu7,
            CboSyubetsu6,
            CboSyubetsu5,
            Chk1,
            Chk2,
            Chk3,
            Chk4,
            Chk5,
            Chk6,
            Chk7,
            Chk8,
            Chk9,
            Chk10,
            Chk11,
            COUNT
        }
        private enum EColNo : int
        {
            Btn,     //
            Chk,
            NO,
            Date,

            COUNT
        }
        private Control[] detailControls;
        private D_Juchuu_Entity dje;
        private WebJuchuuKakunin_BL wjbl;        

        public WebJuchuuKakunin()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM=ProNm;

                this.SetFunctionLabel(EProMode.SHOW);   //照会プログラムとして起動
                this.InitialControlArray();

                //起動時共通処理
                base.StartProgram();

                Btn_F2.Text = "新規受注(F2)";
                Btn_F3.Text = "受注取込(F3)";
                Btn_F4.Text = "名寄せのみ(F4)";
                Btn_F7.Text = "メール(F7)";
                Btn_F10.Text = "Excel出力(F10)";
                Btn_F12.Text = "";

                ModeVisible = true;
                ChangeMode(EOperationMode.INSERT);

                //初期値セット
                wjbl = new WebJuchuuKakunin_BL();
                string ymd = wjbl.GetDate();
                CboStoreCD.Bind(ymd);

                for (int i = (int)EIndex.CboJuchuu; i <= (int)EIndex.CboShukkaJ; i++)
                {
                      ((CKM_Controls.CKM_ComboBox)  detailControls[i]).Bind(ymd);                 
                    }
                for (int i = (int)EIndex.CboSyubetsu8; i <= (int)EIndex.CboSyubetsu5; i++)
                {
                    ((CKM_Controls.CKM_ComboBox)detailControls[i]).Bind(ymd);
                }

                //パラメータ 基準日：Form.日付,店舗：Form.店舗	,得意先区分：3
                ScCustomer.Value1 = "3";

                SetFuncKeyAll(this, "110001000010");

                //画面転送表00に従って、画面情報を表示(Display screen information according to "画面転送表00")
                Scr_Clr(0);

                //「再集計(F8)」ボタン
                btnSaiSyuukei.Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }
        private D_Juchuu_Entity GetSearchInfo()
        {
            dje = new D_Juchuu_Entity
            {
                CustomerKanaName = detailControls[(int)EIndex.CustomerKanaName].Text,
                Tel11 = detailControls[(int)EIndex.Tel].Text,
                CustomerCD = ScCustomer.TxtCode.Text,
                JuchuuNOFrom = detailControls[(int)EIndex.JuchuuNOFrom].Text,
                JuchuuNOTo = detailControls[(int)EIndex.JuchuuNOTo].Text,

                SiteJuchuuDateFrom = detailControls[(int)EIndex.SiteJuchuuDateFrom].Text,
                SiteJuchuuDateTo = detailControls[(int)EIndex.SiteJuchuuDateTo].Text,
                JuchuDateFrom = detailControls[(int)EIndex.JuchuuDateFrom].Text,
                JuchuDateTo = detailControls[(int)EIndex.JuchuuDateTo].Text,
                OrderDateFrom = detailControls[(int)EIndex.OrderDateFrom].Text,
                OrderDateTo = detailControls[(int)EIndex.OrderDateTo].Text,
                NyukinDateFrom = detailControls[(int)EIndex.NyukinDateFrom].Text,
                NyukinDateTo = detailControls[(int)EIndex.NyukinDateTo].Text,
                DecidedDeliveryDateFrom = detailControls[(int)EIndex.DecidedDeliveryDateFrom].Text,
                DecidedDeliveryDateTo = detailControls[(int)EIndex.DecidedDeliveryDateTo].Text,
                DeliveryPlanDateFrom = detailControls[(int)EIndex.DeliveryPlanDateFrom].Text,
                DeliveryPlanDateTo = detailControls[(int)EIndex.DeliveryPlanDateTo].Text,
                DeliveryDateFrom = detailControls[(int)EIndex.DeliveryDateFrom].Text,
                DeliveryDateTo = detailControls[(int)EIndex.DeliveryDateTo].Text,
                InvoiceNOFrom = detailControls[(int)EIndex.InvoiceNOFrom].Text,
                InvoiceNOTo = detailControls[(int)EIndex.InvoiceNOTo].Text,
                SiteJuchuuNO = detailControls[(int)EIndex.SiteJuchuuNO].Text,
                ComboBox8 = ckM_ComboBox8.SelectedIndex > 0 ? ckM_ComboBox8.SelectedValue.ToString() : "",
                ComboBox7 = ckM_ComboBox7.SelectedIndex > 0 ? ckM_ComboBox7.SelectedValue.ToString() : "",
                ComboBox6 = ckM_ComboBox6.SelectedIndex > 0 ? ckM_ComboBox6.SelectedValue.ToString() : "",
                ComboBox5 = ckM_ComboBox5.SelectedIndex > 0 ? ckM_ComboBox5.SelectedValue.ToString() : "",
                IncludeFLG=ChkIncludeFLG.Checked?"1":"0",
                GiftFLG = ChkGiftFLG.Checked ? "1" : "0",
                NoshiFLG = ChkNoshiFLG.Checked ? "1" : "0",
                NouhinsyoFLG = ChkNouhinsyoFLG.Checked ? "1" : "0",
                RyousyusyoFLG = ChkRyousyusyoFLG.Checked ? "1" : "0",
                SonotoFLG = ChkSonotoFLG.Checked ? "1" : "0",
                TelephoneContactKBN = ChkTelephoneContactKBN.Checked ? "1" : "0",
                IndividualContactKBN = ChkIndividualContactKBN.Checked ? "1" : "0",
                NoMailFLG = ChkNoMailFLG.Checked ? "1" : "0",
                CancelFLG = ChkCancelReasonKBN.Checked ? "1" : "0",
                ReturnFLG = ChkReturnFLG.Checked ? "1" : "0",
                StoreCD = CboStoreCD.SelectedIndex > 0 ? CboStoreCD.SelectedValue.ToString():"",
                InsertOperator = InOperatorCD,
            };

            return dje;
        }
        protected override void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            D_JuchuuStatus_Entity de = new D_JuchuuStatus_Entity();
            //de.WarningFLG = ChkWarningFLG.Checked ? "1" : "0";
            de.IncludeFLG = ChkIncludeFLG.Checked ? "1" : "0";
            de.GiftFLG = ChkGiftFLG.Checked ? "1" : "0";
            de.NoshiFLG = ChkNoshiFLG.Checked ? "1" : "0";
            //de.SpecFLG = ChkSpecFLG.Check ? "1" : "0";
            de.NouhinsyoFLG = ChkNouhinsyoFLG.Checked ? "1" : "0";
            de.RyousyusyoFLG = ChkRyousyusyoFLG.Checked ? "1" : "0";
            //de.SeikyuusyoFLG = ChkSeikyuusyoFLG.Checked ? "1" : "0";
            de.SonotoFLG = ChkSonotoFLG.Checked ? "1" : "0";
            //de.OrderMailFLG = ChkOrderMailFLG.Check ? "1" : "0";
            //de.NyuukaYoteiMailFLG = ChkNyuukaYoteiMailFLG.Checked ? "1" : "0";
            //de.SyukkaYoteiMailFLG = ChkSyukkaYoteiMailFLG.Checked ? "1" : "0";
            //de.SyukkaAnnaiMailFLG = ChkSyukkaAnnaiMailFLG.Checked ? "1" : "0";
            //de.NyuukinMailFLG = ChkNyuukinMailFLG.Checked ? "1" : "0";
            //de.FollowupMailFLG = ChkFollowupMailFLG.Checked ? "1" : "0";
            //de.Demand1MailFLG = ChkDemand1MailFLG.Checked ? "1" : "0";
            //de.Demand2MailFLG = ChkDemand2MailFLG.Checked ? "1" : "0";
            //de.Demand3MailFLG = ChkDemand3MailFLG.Checked ? "1" : "0";
            //de.Demand4MailFLG = ChkDemand4MailFLG.Checked ? "1" : "0";

            dje = GetSearchInfo();
            
            DataTable dt = wjbl.D_Juchuu_SelectForWeb(dje, de);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                Btn_NoSelect.Enabled = true;
                Btn_SelectAll.Enabled = true;

                //※明細部.メール不要＝「〇」の時、ロックしONに出来ないようにする。

                GvDetail.Focus();
                Btn_F10.Enabled = true;
                Btn_F7.Enabled = true;

            }
            else
            {
                wjbl.ShowMessage("E128");
            }
        }

        protected override void ExecSec()
        {
            //ボタン。押下時、変更モードでWEB受注入力を起動。
            ExecF2(2);
        }

        private void ExecF2(short kbn)
        {
            string ExeName = "";

            switch(kbn)
            {
                case 1:
                case 2:
                    ExeName = WebJuchuu;
                    break;
                case 3:
                    ExeName = Nayose;
                    break;
                case 4:
                    ExeName = JuchuuTorikomi;
                    break;
            }

            //WEB受注入力のプログラムを起動
            System.Diagnostics.Process[] hProcesses = System.Diagnostics.Process.GetProcessesByName(ExeName);
            if (hProcesses.Length > 0)
            {
                try
                {
                    //SetForegroundWindow(hProcesses[0].MainWindowHandle);
                    Microsoft.VisualBasic.Interaction.AppActivate(hProcesses[0].Id);
                    return;
                }
                catch(Exception ex)
                {
                    //Processes 終了してしまった場合は以下を続行
                }
            }

            string cmdLine = "";
                cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " 0 ";

            DataGridViewRow row = GvDetail.CurrentRow;
            string no = row.Cells["colCollectNO"].Value.ToString();
            string confirmNO = row.Cells["colConfirmNO"].Value.ToString();

            if (kbn.Equals(1))
            {
                //削除モードで、WEB受注確認を表示（売上単位）
                //削除モード:値11, 明細.入金番号, 明細.入金消込番号
                cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " 11 " + no + " " + confirmNO;
            }
            else if (kbn.Equals(2))
            {
                //修正モードで、WEB受注確認を表示（売上単位）
                //修正モード:値10, 明細.入金番号, 明細.入金消込番号
                cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " 10 " + no + " " + confirmNO;
            }

            //EXEが存在しない時ｴﾗｰ
            // 実行モジュールと同一フォルダのファイルを取得
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + ExeName;
            if (System.IO.File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(filePath, cmdLine);
            }
            else
            {
                //ファイルなし
            }

        }
        private void ExecF3()
        {
            //受注取込のプログラムを実行。
            ExecF2(4);

            ExecF4();
        }
        private void ExecF4()
        {
            //名寄せ処理_要注意顧客のプログラムを実行。
            ExecF2(3);

            //「再集計:F8」の処理を実行。
            ExecF8();
        }
        private void ExecF8()
        {
            //「再集計:F8」の処理を実行。
            if (bbl.ShowMessage("Q001", "再集計") == DialogResult.Yes)
            {
                D_Juchuu_Entity de = new D_Juchuu_Entity();
                de.ChangeDate = bbl.GetDate();

                //①	受注保留リストボックス　内容セット
                DataTable dtCombo1 = wjbl.BindForCombo(de, 1);
                ckM_ComboBox1.BindCombo("OnHoldCD", "OnHoldShortName", dtCombo1);

                //②	発注状況リストボックス　内容セット
                DataTable dtCombo2 = wjbl.BindForCombo(de, 2);
                ckM_ComboBox2.BindCombo("OnHoldCD", "OnHoldShortName", dtCombo2);

                //③	出荷保留リストボックス　内容セット
                DataTable dtCombo3 = wjbl.BindForCombo(de, 3);
                ckM_ComboBox3.BindCombo("OnHoldCD", "OnHoldShortName", dtCombo3);

                //④	出荷状態リストボックス　内容セット
                DataTable dtCombo4 = wjbl.BindForCombo(de, 4);
                ckM_ComboBox4.BindCombo("OnHoldCD", "OnHoldShortName", dtCombo4);

                bbl.ShowMessage("I001", "再集計");
            }

        }
        private void ExecMail()
        {
            //Select 明細部.ストア、明細部.顧客名(カナ)、明細部.メール発送状況																	
            //→	メール文章編集･送信画面に表示用データとして保持

            //メール文章編集･送信画面を起動

        }

        private void InitialControlArray()
        {
            detailControls = new Control[] {
                ckM_ComboBox1,ckM_ComboBox2,ckM_ComboBox3,ckM_ComboBox4,
                 CboStoreCD,txtKanaName,txtTel,ScCustomer.TxtCode ,txtJuchuuNOFrom,txtJuchuuNOTo
                 ,ckM_TextBox5, ckM_TextBox3, ckM_TextBox8, ckM_TextBox4
                 ,ckM_TextBox7, ckM_TextBox6, ckM_TextBox1, ckM_TextBox2
                 ,ckM_TextBox10, ckM_TextBox9, ckM_TextBox14, ckM_TextBox13
                 ,ckM_TextBox12, ckM_TextBox11, ckM_TextBox18, ckM_TextBox15
                 ,ckM_TextBox20,ckM_ComboBox8,ckM_ComboBox7,ckM_ComboBox6,ckM_ComboBox5
                ,ChkIncludeFLG,ChkGiftFLG,ChkNoshiFLG,ChkNouhinsyoFLG,ChkRyousyusyoFLG
                ,ChkSonotoFLG,ChkTelephoneContactKBN,ChkIndividualContactKBN,ChkNoMailFLG,ChkCancelReasonKBN,ChkReturnFLG
                 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
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
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.JuchuuNOFrom:
                case (int)EIndex.JuchuuNOTo:
                case (int)EIndex.InvoiceNOFrom:
                case (int)EIndex.InvoiceNOTo:

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.JuchuuNOTo || index== (int)EIndex.InvoiceNOTo)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E197");
                                return false;
                            }
                        }
                    }
                    break;

                case (int)EIndex.SiteJuchuuDateFrom:
                case (int)EIndex.SiteJuchuuDateTo:
                case (int)EIndex.JuchuuDateFrom:
                case (int)EIndex.JuchuuDateTo:
                case (int)EIndex.OrderDateFrom:
                case (int)EIndex.OrderDateTo:
                case (int)EIndex.NyukinDateFrom:
                case (int)EIndex.NyukinDateTo:
                case (int)EIndex.DecidedDeliveryDateFrom:
                case (int)EIndex.DecidedDeliveryDateTo:
                case (int)EIndex.DeliveryPlanDateFrom:
                case (int)EIndex.DeliveryPlanDateTo:
                case (int)EIndex.DeliveryDateFrom:
                case (int)EIndex.DeliveryDateTo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.SiteJuchuuDateTo || index == (int)EIndex.JuchuuDateTo || index == (int)EIndex.OrderDateTo || index == (int)EIndex.NyukinDateTo 
                         || index == (int)EIndex.DecidedDeliveryDateTo || index == (int)EIndex.DeliveryPlanDateTo || index == (int)EIndex.DeliveryDateTo)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                return false;
                            }
                        }
                    }

                    break;

                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboStoreCD.MoveNext = false;
                        return false;
                    }
                    else
                    {
                        //店舗権限のチェック
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            CboStoreCD.MoveNext = false;
                            bbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }

                        //パラメータ 基準日：Form.日付,店舗：Form.店舗	,得意先区分：3
                        ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
                    }
                    break;

                //case (int)EIndex.CboSyubetsu:
                    //if (CboArrivalPlan.SelectedIndex == -1)
                    //{
                    //    bbl.ShowMessage("E102");
                    //    CboArrivalPlan.Focus();
                    //    return false;
                    //}
                    //else
                    //{

                    //}
                    //break;

                case (int)EIndex.CustomerCD:  
                    //顧客情報ALLクリア
                    ClearCustomerInfo();

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    { 
                        return true;
                    }
                    
                    //[M_Customer_Select]
                    M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate()
                    };
                    Customer_BL sbl = new Customer_BL();
                    bool ret = sbl.M_Customer_Select(mce);
                    if (ret)
                    {
                        //if (mce.DeleteFlg == "1")
                        //{
                        //    bbl.ShowMessage("E119");
                        //    return false;
                        //}
                        ScCustomer.LabelText = mce.CustomerName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        return false;
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
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_RadioButton)))
                {
                    ((RadioButton)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CKM_Controls.CKM_CheckBox)ctl).Checked = false;
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

            //初期値セット
            string ymd = wjbl.GetDate();

            //スタッフマスター(M_Staff)に存在すること
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = ymd
            };
            Staff_BL bl = new Staff_BL();
            bool ret = bl.M_Staff_Select(mse);
            if (ret)
            {
                CboStoreCD.SelectedValue = mse.StoreCD;
                //パラメータ 基準日：Form.日付,店舗：Form.店舗	,得意先区分：3
                ScCustomer.Value2 = mse.StoreCD;
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
            
            GvDetail.DataSource = null;
            GvDetail.Enabled = false;
            Btn_NoSelect.Enabled = false;
            Btn_SelectAll.Enabled = false;

            Btn_F7.Enabled = false;
            Btn_F10.Enabled = false;

        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            ScCustomer.LabelText = "";
        }

        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            if (!ModeVisible)
                return;

            switch (Index)
            {
                case 0: // F1:終了
                    {
                        break;
                    }
                case 1:     //F2:新規受注(F2)
                    ExecF2( 1);
                    break;
                case 2:     //F3:
                    ExecF3();
                    break;
                case 3:     //F4:
                    ExecF4();
                    break;
                case 4:     //F5:
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
                case 6://F7:メール
                    ExecMail();
                    break;

                //case 7://F8:新規消込
                //    ExecDetail(0);
                //    break;

                case 9://F10:Excel
                    ExecExcel();
                    break;

                case 11://F12:新規入金
                    //   //Ｑ２０５				
                    //if (bbl.ShowMessage("Q205") != DialogResult.Yes)
                    //    return;

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
                        if ( index != (int)(int)EIndex.COUNT-1)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                        } else
                        {
                            btnSubF11.Focus();
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

                int index = Array.IndexOf(detailControls, sender);

                Btn_F7.Enabled = false;
                Btn_F10.Enabled = false;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
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

        private void CboStoreCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CboStoreCD.SelectedIndex > 0)
                    ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
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
                if (GvDetail.CurrentCell.RowIndex > 0)
                {
                    if (e.ColumnIndex == (int)EColNo.Btn)
                    {
                        //ボタン。押下時、変更モードでWEB受注入力を起動。
                        this.ExecSec();
                    }
                    else if ((Convert.ToBoolean(GvDetail.Rows[e.RowIndex].Cells["colChk"].EditedFormattedValue) == true))
                    {

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
        private void GvDetail_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (GvDetail.CurrentCell == null)
                    return;

                DataGridViewRow row = GvDetail.CurrentRow;

                if (bbl.Z_Set(row.Cells["colConfirmZan"].Value) != 0)
                {
                    //カーソルが明細に存在し、その明細の「消込残額≠０」場合に「新規消込(F9)」として表示
                    Btn_F8.Enabled = true;
                }
                else
                {
                    Btn_F8.Enabled = false;
                }

                //カーソルが明細に存在する場合に「修正(F10)」として表示
                Btn_F10.Enabled = true;
                Btn_F7.Enabled = true;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void btnChangeIkkatuHacchuuMode_Click(object sender, EventArgs e)
        {
            try
            {
                if (OperationMode == EOperationMode.UPDATE)
                {
                    ChangeMode( EOperationMode.INSERT);
                }
                else
                {
                    ChangeMode(EOperationMode.UPDATE);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void Btn_SelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheck(true);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void Btn_NoSelect_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheck(false);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        #endregion
        private void ChangeMode(EOperationMode mode)
        {
            OperationMode = mode;
            switch (mode)
            {
                case EOperationMode.INSERT:
                    ModeText = "受注";
                    btnChangeIkkatuHacchuuMode.Text = "商品モード";
               
                    SetVisible(true);
                    GvDetail.Columns[(int)EColNo.Chk].HeaderText = "ﾒｰﾙ";
                    break;

                case EOperationMode.UPDATE:
                    ModeText = "商品";
                    btnChangeIkkatuHacchuuMode.Text = "受注モード";

                    SetVisible(false);
                    GvDetail.Columns[(int)EColNo.Chk].HeaderText = "選択";
                    break;
            }
        }
        private void SetVisible(bool enabled)
        {
            panel1.Visible = enabled;
            pnlJuchuu.Visible = enabled;
            lblJuchuuDate.Visible = enabled;

            lblCapitalJuchuuDate.Visible = !enabled;
            ScSKUCD.Visible = !enabled;
        }
        private void ChangeCheck(bool check)
        {
            //明細部チェック
            for (int RW = 0; RW <= GvDetail.Rows.Count - 1; RW++)
            {

                //if (mGrid.g_MK_State[(int)ClsGridKaitouNouki.ColNO.Chk, RW].Cell_Enabled)
                GvDetail.Rows[RW].Cells["colChk"].Value = check;
            }
        }
        private void ExecExcel()
        {
            if (GvDetail.Rows.Count > 0)
            {
                DataTable dtExport = (DataTable)GvDetail.DataSource;
                dtExport = ChangeDataColumnName(dtExport);
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Filter = "Excel Files|*.xlsx;";
                savedialog.Title = "Save";
                savedialog.FileName = "ExportFile";
                savedialog.InitialDirectory = @"C:\";
                savedialog.RestoreDirectory = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(savedialog.FileName).Contains(".xlsx"))
                    {
                        Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
                        Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                        Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                        //worksheet = workbook.ActiveSheet; Todo:
                        worksheet.Name = "ExportedFromDatGrid";

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dtExport, "test");

                            wb.SaveAs(savedialog.FileName);
                            wjbl.ShowMessage("I203", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);  //Export Successful
                        }
                    }
                }
            }
        }
        protected DataTable ChangeDataColumnName(DataTable dt)
        {
            dt.Columns["ArrivalDate"].ColumnName = "入荷日";
            dt.Columns["CalcuArrivalPlanDate"].ColumnName = "入荷予定日";
            dt.Columns["PurchaseDate"].ColumnName = "仕入日";
            dt.Columns["Goods"].ColumnName = "入庫区分";
            dt.Columns["SKUCD"].ColumnName = "SKUCD";
            dt.Columns["JanCD"].ColumnName = "JANCD";
            dt.Columns["SKUName"].ColumnName = "商品名";
            dt.Columns["ColorName"].ColumnName = "カラー";
            dt.Columns["SizeName"].ColumnName = "サイズ";
            dt.Columns["ArrivalPlanSu"].ColumnName = "予定数";
            dt.Columns["ArrivalSu"].ColumnName = "入荷数";
            dt.Columns["VendorName"].ColumnName = "仕入先";
            dt.Columns["SoukoName"].ColumnName = "移動元倉庫";
            dt.Columns["Directdelivery"].ColumnName = "直送";
            dt.Columns["ReserveNumber"].ColumnName = "受注番号";
            dt.Columns["Number"].ColumnName = "発注番号";
            dt.Columns["ArrivalNO"].ColumnName = "入荷番号";
            dt.Columns["PurchaseNO"].ColumnName = "仕入番号";
            dt.Columns["VendorDeliveryNo"].ColumnName = "納品書番号";

            dt.Columns.RemoveAt(2);
            return dt;
        }

        private void btnSaiSyuukei_Click(object sender, EventArgs e)
        {
            ExecF8();
        }

        private void GvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (GvDetail.CurrentRow != null && GvDetail.CurrentRow.Index >= 0)
                {
                    //引当変更：その明細がひとつ以上引当済の場合、引当状況を表示・変更する画面に移動。

                    FrmHikiate frm = new FrmHikiate
                    {
                        JuchuuNO = GvDetail.CurrentRow.Cells["colJuchuuNO"].Value.ToString(),
                        ChangeDate = GvDetail.CurrentRow.Cells["colJuchuuDate"].Value.ToString(),
                        JanCD = GvDetail.CurrentRow.Cells["colJANCD"].Value.ToString(),
                        SKUName = GvDetail.CurrentRow.Cells["colSKUName"].Value.ToString(),
                        BrandName = GvDetail.CurrentRow.Cells["colBrandName"].Value.ToString()
                    };

                    frm.ShowDialog();
                }
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








