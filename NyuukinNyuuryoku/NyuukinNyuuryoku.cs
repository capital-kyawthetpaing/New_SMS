using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;

namespace NyuukinNyuuryoku
{
    /// <summary>
    /// NyuukinNyuuryoku 入金入力
    /// </summary>
    internal partial class NyuukinNyuuryoku : FrmMainForm
    {
        private const string ProID = "NyuukinNyuuryoku";
        private const string ProNm = "入金入力";
        private const string NyuukinNyuuryoku_Uriage = "NyuukinNyuuryoku_Detail.exe";

        private enum EIndex : int
        {
            StoreCD,
            DayStart,
            DayEnd,
            InputDateFrom,
            InputDateTo,

            CboSyubetsu,
            CustomerCD,
            Staff,
            RdoZan,
            RdoAll,
            COUNT
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product
        }
        private Control[] detailControls;
        private D_Collect_Entity doe;
        private NyuukinNyuuryoku_BL nnbl;        

        public NyuukinNyuuryoku()
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

                Btn_F8.Text = "F8:新規消込";
                Btn_F10.Text = "F10:修正";

                //初期値セット
                nnbl = new NyuukinNyuuryoku_BL();
                string ymd = nnbl.GetDate();
                CboStoreCD.Bind(ymd);
                CboArrivalPlan.Bind(ymd);
                //パラメータ 基準日：Form.日付,店舗：Form.店舗	,得意先区分：3
                ScCustomer.Value1 = "3";

                SetFuncKeyAll(this, "100001000011");

                //画面転送表00に従って、画面情報を表示(Display screen information according to "画面転送表00")
                Scr_Clr(0);

                detailControls[0].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }
        private D_Collect_Entity GetSearchInfo()
        {
            doe = new D_Collect_Entity
            {
                CollectDateFrom = detailControls[(int)EIndex.DayStart].Text,
                CollectDateTo = detailControls[(int)EIndex.DayEnd].Text,
                InputDateFrom = detailControls[(int)EIndex.InputDateFrom].Text,
                InputDateTo = detailControls[(int)EIndex.InputDateTo].Text,

                CollectCustomerCD = ScCustomer.TxtCode.Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
                StaffCD = ScStaff.TxtCode.Text,
            };

            if (CboArrivalPlan.SelectedIndex != -1)
                if(!string.IsNullOrWhiteSpace(CboArrivalPlan.SelectedText))
                    doe.WebCollectType = CboArrivalPlan.SelectedValue.ToString();

            if (((RadioButton)detailControls[(int)EIndex.RdoZan]).Checked)
            {
                doe.ChkZan = 1;
            }

            return doe;
        }
        protected override void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            doe = GetSearchInfo();
            DataTable dt = nnbl.D_Collect_SelectAll(doe);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
                Btn_F10.Enabled = true;                  

            }
            else
            {
                nnbl.ShowMessage("E128");
            }
        }
        //// 外部プロセスのウィンドウを起動する
        //public static void WakeupWindow(IntPtr hWnd)
        //{
        //    // メイン・ウィンドウが最小化されていれば元に戻す
        //    if (IsIconic(hWnd))
        //    {
        //        ShowWindowAsync(hWnd, SW_RESTORE);
        //    }

        //    // メイン・ウィンドウを最前面に表示する
        //    SetForegroundWindow(hWnd);
        //}
        //// 外部プロセスのメイン・ウィンドウを起動するためのWin32 API
        //[DllImport("user32.dll")]
        //private static extern bool SetForegroundWindow(IntPtr hWnd);
        //[DllImport("user32.dll")]
        //private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        //[DllImport("user32.dll")]
        //private static extern bool IsIconic(IntPtr hWnd);
        //// ShowWindowAsync関数のパラメータに渡す定義値
        //private const int SW_RESTORE = 9;  // 画面を元の大きさに戻す

        private void ExecDetail(short kbn)
        {
            System.Diagnostics.Process[] hProcesses = System.Diagnostics.Process.GetProcessesByName("NyuukinNyuuryoku_Detail");
            if (hProcesses.Length > 0)
            {
                //SetForegroundWindow(hProcesses[0].MainWindowHandle);
                Microsoft.VisualBasic.Interaction.AppActivate(hProcesses[0].Id);
                return;
            }

            DataGridViewRow row = GvDetail.CurrentRow;

            string no = row.Cells["colCollectNO"].Value.ToString();

            string cmdLine ="";
            if (kbn.Equals(0))
            {
                //カーソルが明細に存在し、その明細の「消込残額≠０」場合に「新規消込(F9)」として表示
                //（入金額がすべて消込されている場合（消込残額＝０）の場合は、新規消込はできない）

                //新規消込モードで、入金入力を表示（売上単位）
                //新規消込モード:値9, 明細.入金番号
                cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " 9 " + no;
            }
            else
            {
                //修正モードで、入金入力を表示（売上単位）
                //修正モード:値10, 明細.入金番号, 明細.入金消込番号
                cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " 10 " + no;
            }

            //EXEが存在しない時ｴﾗｰ
            // 実行モジュールと同一フォルダのファイルを取得
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + NyuukinNyuuryoku_Uriage;
            if (System.IO.File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(filePath, cmdLine);
            }
            else
            {
                //ファイルなし
            }

        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { CboStoreCD,ckM_TextBox1, ckM_TextBox2, ckM_TextBox10, ckM_TextBox9
                ,CboArrivalPlan,ScCustomer.TxtCode , ScStaff.TxtCode
                ,ckM_RadioButton1,ckM_RadioButton2
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
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                case (int)EIndex.InputDateFrom:
                case (int)EIndex.InputDateTo:
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
                    if (index == (int)EIndex.DayEnd || index == (int)EIndex.InputDateTo)
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

                case (int)EIndex.CboSyubetsu:
                    //if (CboArrivalPlan.SelectedIndex == -1)
                    //{
                    //    bbl.ShowMessage("E102");
                    //    CboArrivalPlan.Focus();
                    //    return false;
                    //}
                    //else
                    //{

                    //}
                    break;

                case (int)EIndex.CustomerCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {   //顧客情報ALLクリア
                        ClearCustomerInfo();
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
                        //    //顧客情報ALLクリア
                        //    ClearCustomerInfo();
                        //    return false;
                        //}
                        ScCustomer.LabelText = mce.CustomerName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScCustomer.LabelText = "";
                        return false;
                    }

                    break;
                case (int)EIndex.Staff:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScStaff.LabelText = "";
                        return true;
                    }

                    //[M_Staff_Select]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate()
                    };
                    Staff_BL stbl = new Staff_BL();
                    ret = stbl.M_Staff_Select(mse);
                    if (ret)
                    {
                        ScStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScStaff.LabelText = "";
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
            string ymd = nnbl.GetDate();

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

            ((RadioButton)detailControls[(int)EIndex.RdoZan]).Checked = true;

            GvDetail.DataSource = null;
            GvDetail.Enabled = false;

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
                case 7://F8:新規消込
                    ExecDetail(0);
                    break;

                case 9://F10:修正
                    ExecDetail(1);
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
                        if ( index != (int)(int)EIndex.RdoZan && index != (int)(int)EIndex.RdoAll)
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
                switch (index)
                {
                    case (int)EIndex.CustomerCD:
                        case (int)EIndex.Staff:
                        F9Visible = true;
                        break;

                    default:
                        F9Visible = false;
                        break;
                }
                Btn_F8.Enabled = false;
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
        private void BtnSubF12_Click(object sender, EventArgs e)
        {
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








