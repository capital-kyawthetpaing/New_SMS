using Base.Client;
using BL;
using CrystalDecisions.CrystalReports.Engine;
using DL;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using static TempoRegiPoint.Coupon_DataSet;

namespace TempoRegiPoint
{
    public partial class TempoRegiPoint : ShopBaseForm
    {
        /// <summary>
        /// コマンドライン参照
        /// </summary>
        private enum CommandLine
        {
            /// <summary>パス</summary>
            Path = 0,

            /// <summary>会社コード</summary>
            CompanyCD,

            /// <summary>オペレータコード</summary>
            OperatorCD,

            /// <summary>プログラムID</summary>
            PcID,

            /// <summary>会員番号</summary>
            CustomerCD,

            /// <summary>発行ポイント</summary>
            IssuePoint,
        }
        EPSON_TM30.CashDrawerOpen cdo = new EPSON_TM30.CashDrawerOpen();
        /// <summary>
        /// フォントの種類
        /// </summary>
        private const string FONT_TYPE = "ＭＳ ゴシック";

        /// <summary>
        /// BL
        /// </summary>
        TempoRegiPoint_BL bl = new TempoRegiPoint_BL();

        /// <summary>
        /// 発行枚数
        /// </summary>
        private int IssuedNumber = 0;

        /// <summary>
        /// 保持ポイント参照
        /// </summary>
        private int LastPoint
        {
            get
            {
                return int.Parse(TxtLastPoint.Text.Replace(",", ""));
            }
        }

        /// <summary>
        /// 発行ポイント参照
        /// </summary>
        private int IssuePoint
        {
            get
            {
                return int.Parse(TxtIssuePoint.Text.Replace(",", ""));
            }
        }

        /// <summary>
        /// 店舗ポイント引換券印刷 コンストラクタ
        /// </summary>
        public TempoRegiPoint()
        {
            InitializeComponent();

          //  Start_Display();
        }
        protected void Kill(string pth)
        {
            try
            {
                Process[] processCollection = Process.GetProcessesByName(pth.Replace(".exe", ""));
                foreach (Process p in processCollection)
                {
                    p.Kill();
                }

                Process[] processCollections = Process.GetProcessesByName(pth + ".exe");
                foreach (Process p in processCollections)
                {
                    p.Kill();
                }
            }
            catch { }
        }
        private void Stop_DisplayService(bool isForced = true)
        {
            if (Base_DL.iniEntity.IsDM_D30Used)
            {

                Login_BL bbl_1 = new Login_BL();
                if (bbl_1.ReadConfig())
                {
                    bbl_1.Display_Service_Update(false);
                    Thread.Sleep(3 * 1000);
                    bbl_1.Display_Service_Enabled(false);
                }
                else
                {
                    bbl_1.Display_Service_Update(false);
                    Thread.Sleep(3 * 1000);
                    bbl_1.Display_Service_Enabled(false);
                }
                try
                {
                    Kill("Display_Service");
                }
                catch(Exception ex) {
                    MessageBox.Show(ex.StackTrace.ToString());
                }
             if (isForced)    cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
                //Base_DL.iniEntity.CDO_DISPLAY.SetDisplay(true, true,Base_DL.iniEntity.DefaultMessage);
            }
        }
        private void RunDisplay_Service()  // Make when we want to run display_service
        {
            try
            {
                if (Base_DL.iniEntity.IsDM_D30Used)
                {
                    cdo.RemoveDisplay(true);
                    Login_BL bbl_1 = new Login_BL();
                    bbl_1.Display_Service_Update(true);
                    bbl_1.Display_Service_Enabled(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in removing display. . .");
            }
        }

        private void TempoRegiPoint_Load(object sender, EventArgs e)
        {
        
            InProgramID = "TempoRegiPoint";
            string data = InOperatorCD;

            StartProgram();

            Text = "店舗ポイント引換券印刷";
            btnProcess.Text = "印　刷";

            SetRequireField();

            //コマンドライン引数を配列で取得する
            string[] cmds = Environment.GetCommandLineArgs();
            if (cmds.Length - 1 > (int)ECmdLine.PcID)
            {
                // 別プログラムからの呼び出し時

                // フォームを表示させないように最小化してタスクバーにも表示しない
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;

                TxtCustomerCD.Text = cmds[(int)CommandLine.CustomerCD];
                SearchCustomer();
                TxtIssuePoint.Text = cmds[(int)CommandLine.IssuePoint];

                // 印刷後そくクローズ
                Print();
                Close();
            }
           // Stop_DisplayService();
        }

        /// <summary>
        /// オブジェクトの設定
        /// </summary>
        private void SetRequireField()
        {
            BtnSearchCustomer.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            //btnSearchCustomer.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Medium;

            TxtCustomerCD.Require(true);
            TxtCustomerCD.Clear();
            TxtCustomerCD.Focus();

            LblCustomerName.Text = string.Empty;

            TxtLastPoint.Require(true);
            TxtLastPoint.Text = "";
            SetLastPointColor();

            TxtIssuePoint.Require(true);
            TxtIssuePoint.Text = "";
        }

        private void DisplayData()
        {
            BtnSearchCustomer.Focus();
            string data = InOperatorCD;
        }

        /// <summary>
        /// エラーチェック
        /// </summary>
        /// <returns>true=エラーなし、false=エラーあり</returns>
        /// <remarks>領収書印字日付はコントロールにチェック処理あり</remarks>
        public bool ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(TxtCustomerCD.Text))
            {
                bl.ShowMessage("E102");
                TxtCustomerCD.Focus();
                return false;
            }
            else
            {
                if (!SearchCustomer())
                {
                    return false;
                }
            }

            if (IssuePoint == 0 || LastPoint < IssuePoint)
            {
                bl.ShowMessage("E117", "1", TxtLastPoint.Text);
                TxtIssuePoint.Focus();
                return false;
            }

            var ticketUnit = bl.D_TicketUnitSelect(StoreCD);
            var TU= Convert.ToInt32(ticketUnit.Rows[0]["TicketUnit"]);
            if (ticketUnit.Rows.Count == 0 || (IssuePoint % TU ) != 0)
            {
                bl.ShowMessage("E198", "該当店舗の引換券発行単位の倍数以外", TxtLastPoint.Text);
                TxtIssuePoint.Focus();
                return false;
            }
            else
            {
                // 発行枚数計算
                IssuedNumber = IssuePoint / Convert.ToInt32(ticketUnit.Rows[0]["TicketUnit"]);
            }

            return true;
        }

        /// <summary>
        /// 画面終了処理
        /// </summary>
        protected override void EndSec()
        {
            // RunDisplay_Service();
            Close();
        }

        /// <summary>
        /// ファンクション処理
        /// </summary>
        /// <param name="index"></param>
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                    // 印刷実行
                    Print();
                    break;
            }
        }

        /// <summary>
        /// 印刷実行
        /// </summary>
        private void Print()
        {
            if (ErrorCheck())
            {
                var coupon = bl.D_CouponSelect(StoreCD);
                if (coupon.Rows.Count > 0)
                {
                    OutputCoupon(coupon);
                }
                else
                {
                    bl.ShowMessage("E128");
                }
            }
        }
      
        /// <summary>
        /// 商品引換券出力
        /// </summary>
        /// <param name="data">データ</param>
        private void OutputCoupon(DataTable data)
        {
            var couponDataSet = CreateDataSet(data);
            var couponRow = couponDataSet.StorePointTable.Rows[0] as StorePointTableRow;

            // 出力
            var report = new TempoRegiPoint_Coupon();
            report.SetDataSource(couponDataSet);

            // フォント設定
            ApplyFont(report, "Print1", couponRow.Size1, couponRow.Bold1);      //  1行目
            ApplyFont(report, "Print2", couponRow.Size2, couponRow.Bold2);      //  2行目
            ApplyFont(report, "Print3", couponRow.Size3, couponRow.Bold3);      //  3行目
            ApplyFont(report, "Print4", couponRow.Size4, couponRow.Bold4);      //  4行目
            ApplyFont(report, "Print5", couponRow.Size5, couponRow.Bold5);      //  5行目
            ApplyFont(report, "Print6", couponRow.Size6, couponRow.Bold6);      //  6行目
            ApplyFont(report, "Print7", couponRow.Size7, couponRow.Bold7);      //  7行目
            ApplyFont(report, "Print8", couponRow.Size8, couponRow.Bold8);      //  8行目
            ApplyFont(report, "Print9", couponRow.Size9, couponRow.Bold9);      //  9行目
            ApplyFont(report, "Print10", couponRow.Size10, couponRow.Bold10);   // 10行目
            ApplyFont(report, "Print11", couponRow.Size11, couponRow.Bold11);   // 11行目
            ApplyFont(report, "Print12", couponRow.Size12, couponRow.Bold12);   // 12行目

            report.Refresh();
            report.PrintOptions.PrinterName = StorePrinterName;
            
            try
            {
             //   cdo.RemoveDisplay(true);  // 2020/12/04
            }
            catch { }
            for (var count = 0; count < IssuedNumber; count++)
            {
                report.PrintToPrinter(0, false, 0, 0);
            }
          //  Stop_DisplayService(); // 2020/12/04
            // 発行ポイント更新、ログ更新
            bl.M_UpdateLastPoint(TxtCustomerCD.Text, IssuePoint, InOperatorCD, InProgramID, InPcID);
        }

        /// <summary>
        /// クーポンデータセット作成
        /// </summary>
        /// <param name="data">データ</param>
        /// <returns>クーポンデータセット</returns>
        private Coupon_DataSet CreateDataSet(DataTable data)
        {
            var row = data.Rows[0];
            var couponDataSet = new Coupon_DataSet();
            var couponRow = couponDataSet.StorePointTable.NewStorePointTableRow();

            // 引換券発行単位
            couponRow.TicketUnit = Convert.ToString(row["TicketUnit"]);

            // 1行目
            couponRow.Print1 = Convert.ToString(row["Print1"]);     // 文章
            couponRow.Bold1 = Convert.ToInt32(row["Bold1"]);        // 太字
            couponRow.Size1 = Convert.ToInt32(row["Size1"]);        // サイズ

            // 2行目
            couponRow.Print2 = Convert.ToString(row["Print2"]);     // 文章
            couponRow.Bold2 = Convert.ToInt32(row["Bold2"]);        // 太字
            couponRow.Size2 = Convert.ToInt32(row["Size2"]);        // サイズ

            // 3行目
            couponRow.Print3 = Convert.ToString(row["Print3"]);     // 文章
            couponRow.Bold3 = Convert.ToInt32(row["Bold3"]);        // 太字
            couponRow.Size3 = Convert.ToInt32(row["Size3"]);        // サイズ

            // 4行目
            couponRow.Print4 = Convert.ToString(row["Print4"]);     // 文章
            couponRow.Bold4 = Convert.ToInt32(row["Bold4"]);        // 太字
            couponRow.Size4 = Convert.ToInt32(row["Size4"]);        // サイズ

            // 5行目
            couponRow.Print5 = Convert.ToString(row["Print5"]);     // 文章
            couponRow.Bold5 = Convert.ToInt32(row["Bold5"]);        // 太字
            couponRow.Size5 = Convert.ToInt32(row["Size5"]);        // サイズ

            // 6行目
            couponRow.Print6 = Convert.ToString(row["Print6"]);     // 文章
            couponRow.Bold6 = Convert.ToInt32(row["Bold6"]);        // 太字
            couponRow.Size6 = Convert.ToInt32(row["Size6"]);        // サイズ

            // 7行目
            couponRow.Print7 = Convert.ToString(row["Print7"]);     // 文章
            couponRow.Bold7 = Convert.ToInt32(row["Bold7"]);        // 太字
            couponRow.Size7 = Convert.ToInt32(row["Size7"]);        // サイズ

            // 8行目
            couponRow.Print8 = Convert.ToString(row["Print8"]);     // 文章
            couponRow.Bold8 = Convert.ToInt32(row["Bold8"]);        // 太字
            couponRow.Size8 = Convert.ToInt32(row["Size8"]);        // サイズ

            // 9行目
            couponRow.Print9 = Convert.ToString(row["Print9"]) + "　　　　　　　.";     // 文章
            couponRow.Bold9 = Convert.ToInt32(row["Bold9"]);        // 太字
            couponRow.Size9 = Convert.ToInt32(row["Size9"]);        // サイズ

            // 10行目
            couponRow.Print10 = Convert.ToString(row["Print10"]);   // 文章
            couponRow.Bold10 = Convert.ToInt32(row["Bold10"]);      // 太字
            couponRow.Size10 = Convert.ToInt32(row["Size10"]);      // サイズ

            // 11行目
            couponRow.Print11 = Convert.ToString(row["Print11"]);   // 文章
            couponRow.Bold11 = Convert.ToInt32(row["Bold11"]);      // 太字
            couponRow.Size11 = Convert.ToInt32(row["Size11"]);      // サイズ 

            // 12行目
            couponRow.Print12 = Convert.ToString(row["Print12"]);   // 文章
            couponRow.Bold12 = Convert.ToInt32(row["Bold12"]);      // 太字
            couponRow.Size12 = Convert.ToInt32(row["Size12"]);      // サイズ 

            // データセットに追加
            couponDataSet.StorePointTable.Rows.Add(couponRow);

            return couponDataSet;
        }

        /// <summary>
        /// レポートファイルの行オブジェクトにフォントを設定
        /// </summary>
        /// <param name="report">レポートファイルオブジェクト</param>
        /// <param name="name">行オブジェクト名</param>
        /// <param name="size">フォントサイズ</param>
        /// <param name="bold">ボールド指定(0:指定なし、1:指定あり)</param>
        /// <remarks>フォントサイズが0の場合は設定しない</remarks>
        private void ApplyFont(TempoRegiPoint_Coupon report, string name, float size, int bold)
        {
            if (size > 0)
            {
                ((TextObject)report.Section3.ReportObjects[name]).ApplyFont(
                    new Font(FONT_TYPE, size, bold == 1 ? FontStyle.Bold : FontStyle.Regular)
                );
            }
        }

        /// <summary>
        /// 会員の保持ポイント取得
        /// </summary>
        private void GetLastPoint()
        {
            var lastPointDt = bl.D_LastPointSelect(TxtCustomerCD.Text);
            if (lastPointDt.Rows.Count > 0)
            {
                TxtLastPoint.Text = Convert.ToInt32(lastPointDt.Rows[0]["LastPoint"]).ToString();
            }
        }

        /// <summary>
        /// 会員番号ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearchCustomer_Click(object sender, EventArgs e)
        {
            var search = new Search.TempoRegiKaiinKensaku();
            var result = search.ShowDialog();

            if (!string.IsNullOrEmpty(search.CustomerCD))
            {
                TxtCustomerCD.Text = search.CustomerCD;
                LblCustomerName.Text = search.CustomerName;

                GetLastPoint();
            }
        }

        /// <summary>
        /// 会員番号入力イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCustomerCD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SearchCustomer())
                {
                    TxtIssuePoint.Focus();
                }
            }
            else
            {
                // 入力中
                LblCustomerName.Text = string.Empty;
                TxtLastPoint.Text = string.Empty;
            }
        }

        /// <summary>
        /// 保持ポイント入力変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtLastPoint_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(TxtLastPoint.Text, out int value))
            {
                TxtLastPoint.Text = string.Format("{0:#,##0}", value);
            }
        }

        /// <summary>
        /// 保持ポイント入力アクティブ時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtLastPoint_Enter(object sender, EventArgs e)
        {
            SetLastPointColor();
        }

        /// <summary>
        /// 保持ポイント入力フォーカス移動時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtLastPoint_Leave(object sender, EventArgs e)
        {
            SetLastPointColor();
        }

        /// <summary>
        /// 発行ポイント入力イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtIssuePoint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ErrorCheck())
                {
                    this.btnClose.Focus();
                }
            }
        }

        /// <summary>
        /// 発行ポイント入力フォーカス移動時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtIssuePoint_Leave(object sender, EventArgs e)
        {
            if (int.TryParse(TxtIssuePoint.Text, out int value))
            {
                TxtIssuePoint.Text = string.Format("{0:#,##0}", value);
            }
        }

        /// <summary>
        /// 発行ポイントアクティブ時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtIssuePoint_Enter(object sender, EventArgs e)
        {
            if (TxtIssuePoint.Text.Contains(","))
            {
                TxtIssuePoint.Text = TxtIssuePoint.Text.Replace(",", "");
            }
        }

        /// <summary>
        /// 会員を検索
        /// </summary>
        /// <returns>処理結果(true=有効な会員、false=無効な会員)</returns>
        private bool SearchCustomer()
        {
            bool result;

            var customerDt = bl.D_GetCustomer(TxtCustomerCD.Text);
            if (customerDt.Rows.Count > 0)
            {
                if (customerDt.Rows[0]["DeleteFlg"].ToString() == "0")
                {
                    // 有効な会員
                    TxtCustomerCD.Text = customerDt.Rows[0]["CustomerCD"].ToString();
                    LblCustomerName.Text = customerDt.Rows[0]["CustomerName"].ToString();

                    GetLastPoint();
                    result = true;
                }
                else
                {
                    // 削除された会員
                    bl.ShowMessage("E140");
                    TxtCustomerCD.Focus();
                    LblCustomerName.Text = string.Empty;
                    TxtLastPoint.Text = string.Empty;
                    result = false;
                }
            }
            else
            {
                // 該当なし
                bl.ShowMessage("E138");
                TxtCustomerCD.Focus();
                LblCustomerName.Text = string.Empty;
                TxtLastPoint.Text = string.Empty;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 保持ポイント入力ボックスの色を設定
        /// </summary>
        private void SetLastPointColor()
        {
            TxtLastPoint.BackColor = Color.White;
            TxtLastPoint.ForeColor = Color.Black;
        }
        private void Start_Display()
        {
            try
            {
                if (Base_DL.iniEntity.IsDM_D30Used)
                {
                    Login_BL bbl_1 = new Login_BL();
                    if (bbl_1.ReadConfig())
                    {
                        bbl_1.Display_Service_Update(false);
                        Thread.Sleep(3 * 1000);
                        bbl_1.Display_Service_Enabled(false);
                    }
                    else
                    {
                        bbl_1.Display_Service_Update(false);
                        Thread.Sleep(3 * 1000);
                        bbl_1.Display_Service_Enabled(false);
                    }
                    Kill("Display_Service");
                }
            }
            catch (Exception ex) { MessageBox.Show("Cant remove on second time" + ex.StackTrace); }
        }
    }
}
