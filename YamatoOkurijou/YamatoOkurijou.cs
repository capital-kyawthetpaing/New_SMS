using System;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.IO;

using BL;
using Entity;
using Base.Client;

namespace YamatoOkurijou
{
    /// <summary>
    /// YamatoOkurijou　ヤマト送り状
    /// </summary>
    internal partial class YamatoOkurijou: FrmMainForm
    {
        private const string ProID = "YamatoOkurijou";
        private const string ProNm = "ヤマト送り状";

        private enum EIndex : int
        {
            ShippingNO
        }

        private Control[] detailControls;
        private Control[] searchButtons;

        private YamatoOkurijou_BL mibl;
        private D_Shipping_Entity dee;
        private DataTable dtCSV;

        public YamatoOkurijou         ()
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
                base.Btn_F12.Text = "データ出力(F12)";           
                
                //起動時共通処理
                base.StartProgram();

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                CboStoreCD.Bind(ymd);

                mibl = new YamatoOkurijou_BL();

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScShippingNO.Value1 = InOperatorCD;
                ScShippingNO.Value2 = stores;

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
                }


                //Parameter.出荷番号 is not null の場合	（出荷入力からの起動を想定）
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    string ShippingNO = cmds[(int)ECmdLine.PcID + 1];
                    ScShippingNO.Text = ShippingNO;

                    //画面を表示しない
                    //データ出力処理へ
                    ExecSec();

                    //データ作成後、そのままプログラム終了
                    EndSec();
                }
                //Parameter.出荷番号 is null の場合（メニューからの起動を想定）
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
            detailControls = new Control[] { CboStoreCD, ScShippingNO.TxtCode };
            searchButtons = new Control[] { ScShippingNO.BtnSearch };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
        
   
        /// <summary>
        /// エラーチェック処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData()
        {
            for (int i = 0; i < detailControls.Length; i++)
            {
                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return false;
                }
            }
            
            dee = new D_Shipping_Entity();
            dee.ShippingNO = ScShippingNO.TxtCode.Text;
            dee.Operator = this.InOperatorCD;
            dee.PC = this.InPcID;
            dtCSV = mibl.D_Shipping_SelectForYamato(dee);

            //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
            if (dtCSV.Rows.Count == 0)
            {
                bbl.ShowMessage("E133");
                PreviousCtrl.Focus();
                return false;
            }







            return true;
        }
        protected override void ExecSec()
        {
            bool ret = CheckData();
            if (ret == false)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            ExportCsv(dtCSV);


            //ret = mibl.PRC_EDIOrder_Insert(doe,dee);

            this.Cursor = Cursors.Default;

            if (ret == false)
            {
                bbl.ShowMessage("S002");
            }
            else
            {
                bbl.ShowMessage("I002");
            }

            //更新後画面そのまま
            detailControls[0].Focus();
        }
        /// <summary>
        /// CSV出力
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool ExportCsv(DataTable dt)
        {
            try
            {
                //CSV出力データ取得
                if (dt.Rows.Count == 0)
                {
                    return true;
                }

                //CSV出力処理
                string strFullPath = string.Empty;
                string strPath = string.Empty;
                string titleFlg = "0";

                //出力先取得
                M_Control_Entity mce = new M_Control_Entity();
                mce.MainKey = "1";

                DataTable data= mibl.M_Control_Select(mce);
                if (data.Rows.Count > 0)
                {
                    //サーバ名
                    strPath = data.Rows[0]["CreateServer"].ToString();

                    if (!strPath.EndsWith(@"\"))
                    {
                        strPath += @"\";
                    }
                    //フォルダ名
                    strPath += data.Rows[0]["CreateFolder"].ToString();
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (!strPath.EndsWith(@"\"))
                    {
                        strPath += @"\";
                    }
                    strFullPath = strPath + Path.GetFileNameWithoutExtension(data.Rows[0]["FileName"].ToString()) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                    titleFlg = data.Rows[0]["TitleFLG"].ToString();

                }

                //CSV出力
                //CSVファイルに書き込むときに使うEncoding
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");

                using (StreamWriter sw = new StreamWriter(strFullPath, false, enc))
                {

                    int colCount = dt.Columns.Count - 2;
                    int lastColIndex = colCount - 2;

                    //ヘッダを書き込む
                    if (titleFlg == "1")
                    {

                        string field = "出荷指示番号,出荷指示明細番号,送り状種別,サイズ品目コード,クール区分,ギフトフラグ,受注区分,送り状番号,出荷日,お届け予定日,納品時間指定区分" +
                            ",注文者コード,注文者電話番号,注文者郵便番号,注文者住所,注文者住所１,注文者住所２,注文者住所３,注文者氏名,注文者敬称" +
                            ",店舗コード,店舗電話番号,店舗FAX番号,店舗郵便番号,店舗住所,店舗住所１,店舗住所２,店舗住所３,店舗名,店舗URL,店舗メールアドレス" +
                            ",注文日,決済方法表示,小計,送料,決済料,手数料１,手数料２,手数料３,割引金額,請求金額" +
                            ",お届け先コード,お届け先電話番号,お届け先郵便番号,お届け先住所,お届け先住所１,お届け先住所２,お届け先住所３,お届け先部門名１,お届け先部門名２,お届け先名,お届け先名略称カナ,お届け先敬称" +
                            ",依頼主コード,依頼主電話番号,依頼主郵便番号,依頼主住所,依頼主住所１,依頼主住所２,依頼主住所３,依頼主名,依頼主名略称カナ" +
                            ",荷扱い１,荷扱い２,記事,品名コード１,品名１,品名コード２,品名２,コレクト代金引換額,コレクト内消費税額等,営業所止置き,営業所コード" +
                            ",個口数,個口数枠の印字,のし名入れ,出荷指示備考１,出荷指示備考２,出荷指示備考３,出荷指示備考４" +
                            ",運賃請求先コード,運賃請求先コード枝番,運賃管理番号,出荷区分,商品番号,商品名表示,バリエーション,商品個数,単位,商品税込単価,商品金額" +
                            ",明細予備項目１,明細予備項目２,お届け予定eメール　利用区分,お届け予定eメール　e-mailアドレス,入力機種,お届け予定eメール　メッセージ,お届け完了eメール　利用区分,お届け完了eメール　e-mailアドレス,お届け完了eメール　メッセージ" +
                            ",出荷拠点コード,あんしん決済　利用区分,あんしん決済　受付番号,あんしん決済　加盟店コード,あんしん決済　注文日,あんしん決済　取引先名,あんしん決済　決済金額（税込）,あんしん決済　会員ID" +
                            ",予備項目１,予備項目２,投函予定メール利用区分,投函予定メールアドレス,投函予定メールメッセージ,投函完了メール(お届け先宛)利用区分,投函完了メール(お届け先宛)アドレス,投函完了メール(お届け先宛)メッセージ,投函完了メール(ご依頼主宛)利用区分,投函完了メール(ご依頼主宛)アドレス,投函完了メール(ご依頼主宛)メッセージ" +
                            ",運送会社区分,西濃原票区分,重量,保険金額,データ識別区分,受取店ロゴ分類,受取選択連携管理番号,受取選択直営店コード,受取選択荷受人コード,真荷主コード,JPセキュリティフラグ";

                        sw.Write(field);
                        sw.Write("\r\n");
                    }

                    //レコードを書き込む
                    foreach (DataRow row in dt.Rows)
                    {
                        //最後の項目は書き込まない
                        for (int count = 2; count < colCount - 1; count++)
                        {
                            //書き込み
                            switch (count)
                            {
                                default:
                                    string field = EncloseDoubleQuotesIfNeed(row[count].ToString());
                                    //sw.Write(row[count]);
                                    sw.Write(field);
                                    break;
                            }

                            //カンマを書き込む
                            if (lastColIndex > count)
                            {
                                sw.Write(',');
                            }
                        }
                        //改行する
                        sw.Write("\r\n");
                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 必要ならば、文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
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
            DataTable dt;
            YamatoOkurijou_BL ble = new YamatoOkurijou_BL();

            switch (index)
            {
                //case (int)EIndex.SyoriNO:
                //    //入力不可の場合チェックなし
                //    if (!detailControls[index].Enabled)
                //    {
                //        return true;
                //    }

                //    //必須入力
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        bbl.ShowMessage("E102");
                //        return false;
                //    }

                //    //ヤマト送り状(D_EDIOrder)に存在すること
                //    //[D_EDIOrder]
                //    dee = GetEntityForEDIOrder();
                //    dt = ble.D_EDIOrder_Select(dee);
                //    if (dt.Rows.Count == 0)
                //    {
                //        bbl.ShowMessage("E138", "EDI処理番号");
                //        return false;
                //    }
                //    else
                //    {
                //        //権限がない場合（以下のSelectができない場合）Error　「権限のないヤマト送り状番号」
                //        if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                //        {
                //            bbl.ShowMessage("E139", "EDI処理番号");
                //            return false;
                //        }
                //        break;
                //    }
            
                case (int)EIndex.ShippingNO:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //存在する出荷番号であること
                    //[D_Order]
                    string orderNo = detailControls[index].Text;
                    dt = ble.D_Order_SelectForEDIHacchuu(orderNo);
                    if (dt.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E138", "発注番号");
                        return false;
                    }
                    else
                    {
                        //DeleteDateTime 「削除された発注番号」
                        if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            bbl.ShowMessage("E140", "発注番号");
                            return false;
                        }

                        //権限がない場合（以下のSelectができない場合）Error　「権限のないヤマト送り状番号」
                        if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E139", "発注番号");
                            return false;
                        }
                        
                        break;
                    }

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
            
            //初期値セット
            string ymd = mibl.GetDate();

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

            //出力対象
            ScShippingNO.Enabled = false;

            detailControls[0].Focus();
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
                            ScShippingNO.BtnSearch.Enabled = Kbn == 0 ? true : false;

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
                case 5:     //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        Scr_Clr(0);

                        break;
                    }

                case 11:   //F12:データ出力
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

        private void BtnSubF12_Click(object sender, EventArgs e)
        {
            //データ出力ボタンClick時   
            try
            {
                base.FunctionProcess(FuncExec - 1);

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
                PreviousCtrl = this.ActiveControl;

                //int index = Array.IndexOf(detailControls, sender);
                //switch(index)
                //    {
                //    case (int)EIndex.ShippingNO:
                //        F9Visible = true;
                //        break;

                //    default:
                //        F9Visible = false;
                //        break;
                //}
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








