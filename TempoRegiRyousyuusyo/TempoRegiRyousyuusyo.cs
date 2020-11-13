using Base.Client;
using BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Windows.Forms;

namespace TempoRegiRyousyuusyo
{
    /// <summary>
    /// 店舗レジ領収書印刷画面
    /// </summary>
    public partial class TempoRegiRyousyuusyo : ShopBaseForm
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

            /// <summary>お買上番号</summary>
            SalesNO,

            /// <summary>領収書出力チェック</summary>
            RyousyuushoCheck,

            /// <summary>レシート出力チェック</summary>
            ReceiptCheck,

            /// <summary>領収書印字日付</summary>
            PrintDate,

            /// <summary>再発行チェック</summary>
            ReissueCheck
        }
        private bool IsHanbaiTouroku = false;
        /// <summary>
        /// 製品名上段文字数
        /// </summary>
        private const int SKU_SHORTNAME_LENGTH = 23 * 2;

        /// <summary>
        /// BL
        /// </summary>
        TempoRegiRyousyuusyo_BL bl = new TempoRegiRyousyuusyo_BL();

        /// <summary>
        /// 店舗レジ領収収書印刷 コンストラクタ
        /// </summary>
        public TempoRegiRyousyuusyo()
        {
            string[] cmds = Environment.GetCommandLineArgs();
            if (cmds.Length - 1 > (int)ECmdLine.PcID)
            {
                IsHanbaiTouroku = true;
            }
            //else
            //{
                Start_Display();
               // IsHanbaiTouroku = false;
            //}
            InitializeComponent();


        }

        /// <summary>
        /// 画面読み込み処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempoRegiRyousyuusyo_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiRyousyuusyo";
            string data = InOperatorCD;

            StartProgram();

            Text = "店舗領収書印刷";
            btnProcess.Text = "印　刷";

            SetRequireField();
            txtSalesNO.Focus();

            //コマンドライン引数を配列で取得する
            string[] cmds = Environment.GetCommandLineArgs();
            if (cmds.Length - 1 > (int)ECmdLine.PcID)
            {
                try
                {
                    // 別プログラムからの呼び出し時
                    // フォームを表示させないように最小化してタスクバーにも表示しない
                    WindowState = System.Windows.Forms.FormWindowState.Minimized;
                    ShowInTaskbar = false;
                    txtSalesNO.Text = cmds[(int)CommandLine.SalesNO];
                    chkRyousyuusho.Checked = Convert.ToBoolean(Convert.ToInt32(cmds[(int)CommandLine.RyousyuushoCheck]));
                    chkReceipt.Checked = Convert.ToBoolean(Convert.ToInt32(cmds[(int)CommandLine.ReceiptCheck]));
                    txtPrintDate.Text = cmds[(int)CommandLine.PrintDate];
                    //chkReissue.Checked = Convert.ToBoolean(cmds[(int)CommandLine.ReissueCheck]);
                    IsHanbaiTouroku = true;
                    // 印刷後そくクローズ
                    Print();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
            Stop_DisplayService();
        }

        /// <summary>
        /// オブジェクトの設定
        /// </summary>
        private void SetRequireField()
        {
            txtSalesNO.Require(true);
            txtSalesNO.Clear();

            chkRyousyuusho.Checked = true;
            chkReceipt.Checked = false;

            txtPrintDate.Require(true);
            txtPrintDate.Clear();
            //txtPrintDate.Text = DateTime.Today.ToShortDateString();

            chkReissue.Checked = false;
        }

        private void DisplayData()
        {
            txtSalesNO.Focus();
            string data = InOperatorCD;
            //SetRequireField();
            //BindCombo();
        }

        /// <summary>
        /// エラーチェック
        /// </summary>
        /// <returns>true=エラーなし、false=エラーあり</returns>
        /// <remarks>領収書印字日付はコントロールにチェック処理あり</remarks>
        public bool ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(txtSalesNO.Text))
            {
                bl.ShowMessage("E102");
                txtSalesNO.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPrintDate.Text) && !bbl.CheckDate(txtPrintDate.Text))
            {
                // 日付エラー
                bbl.ShowMessage("E103");
                txtPrintDate.Focus();
                return false;
            }

            if (!bl.D_CheckSalseNO(txtSalesNO.Text))
            {
                // 売上データなし
                bl.ShowMessage("E138", "売上番号");
                txtSalesNO.Focus();
                return false;
            }
            else
            {
                // 売上データあり
                if (bl.D_CheckDeleteSalseNO(txtSalesNO.Text))
                {
                    // 削除済み売上データあり
                    bl.ShowMessage("E140", "売上番号");
                    txtSalesNO.Focus();
                    return false;
                }
            }

            return true;
        }

        protected override void EndSec()
        {
            
                RunDisplay_Service();
            
            this.Close();
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
                //var isPreview = bl.ShowMessage("Q202") == DialogResult.Yes;

                if (!chkRyousyuusho.Checked && !chkReceipt.Checked)
                {
                    // 領収書、レシートともにチェックなしの場合
                    bl.ShowMessage("E111");
                    chkRyousyuusho.Focus();
                }
                else
                {
                    // 領収書、レシートいずれかにチェックありの場合
                    bool Isboth = chkRyousyuusho.Checked && chkReceipt.Checked;
                    if (chkRyousyuusho.Checked)
                    {
                        // 領収書チェックボックスにチェックあり
                        var ryousyuusyo = bl.D_RyousyuusyoSelect(txtSalesNO.Text, txtPrintDate.Text);
                        if (ryousyuusyo.Rows.Count > 0)
                        {
                                //try
                                //{
                                //    cdo.RemoveDisplay(true);
                                //    cdo.RemoveDisplay(true);
                                //}
                                //catch { }
                                OutputRyouusyusyo(ryousyuusyo);
                            try
                            {
                                //Thread.Sleep(2000);
                                if (!Isboth)
                                    Stop_DisplayService();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            bl.ShowMessage("E198", "領収書");
                            txtSalesNO.Focus();
                        }
                    }

                    if (chkReceipt.Checked)
                    {
                        // レシートチェックボックスにチェックあり

                        // 店舗取引履歴
                        DataTable receiptData = bl.D_ReceiptSelect(txtSalesNO.Text, chkReissue.Checked);
                        //if ((!IsHanbaiTouroku))
                        //{
                            if (receiptData.Rows.Count > 0)
                            {
                                try
                                {
                                    cdo.RemoveDisplay(true);
                                    cdo.RemoveDisplay(true);
                                }
                                catch { }
                                OutputReceipt(receiptData);

                                Stop_DisplayService();
                            }
                            else
                            {
                                bl.ShowMessage("E128");
                                txtSalesNO.Focus();
                                // Stop_DisplayService();
                                //  return;
                            }
                        //}
                        //else
                        //{
                        //    if (receiptData.Rows.Count > 0)
                        //    {
                        //        OutputReceipt(receiptData);
                        //    }
                        //    else
                        //    {
                        //        bl.ShowMessage("E198", "領収書");
                        //        txtSalesNO.Focus();
                        //    }
                        //}
                  
                      
                    }

                }
            }
        }

        /// <summary>
        /// 領収書出力
        /// </summary>
        /// <param name="data">データ</param>
        private void OutputRyouusyusyo(DataTable data)
        {
            var row = data.Rows[0];

            var ryousyuusyoDataSet = new TempoRegiRyousyuusyo_DataSet();
            var ryousyuusyoRow = ryousyuusyoDataSet.D_SelectData_ForTempoRegiRyousyuusyo.NewD_SelectData_ForTempoRegiRyousyuusyoRow();

            // お買上番号
            ryousyuusyoRow.SalesNO = Convert.ToString(row["SalesNO"]);

            // 売上日付
            ryousyuusyoRow.UriageDateTime = ConvertDateTime(row["UriageDateTime"], !string.IsNullOrWhiteSpace(txtPrintDate.Text));

            // 相手名
            ryousyuusyoRow.AiteName = Convert.ToString(row["AiteName"]);

            // 売上額
            ryousyuusyoRow.SalesGaku = ConvertDecimal(row["SalesGaku"]);

            // 消費税額
            var salesTax = ConvertDecimal(row["SalesTax"]);
            ryousyuusyoRow.SalesTax = (string.IsNullOrWhiteSpace(salesTax) || salesTax == "0" ? "" : @"\") + ConvertDecimal(row["SalesTax"]);

            // 会社名
            ryousyuusyoRow.CompanyName = Convert.ToString(row["CompanyName"]);

            // 店舗名
            ryousyuusyoRow.StoreName = Convert.ToString(row["StoreName"]);

            // 住所１
            ryousyuusyoRow.Address1 = Convert.ToString(row["Address1"]);

            // 住所２
            ryousyuusyoRow.Address2 = Convert.ToString(row["Address2"]);

            // 電話番号
            ryousyuusyoRow.TelphoneNO = Convert.ToString(row["TelephoneNO"]);

            // 担当-店舗レシート表記
            ryousyuusyoRow.ReceiptPrint = Convert.ToString(row["ReceiptPrint"]);

            // 領収書コメント１
            ryousyuusyoRow.Char1 = Convert.ToString(row["Char1"]);

            // 領収書コメント２
            ryousyuusyoRow.Char2 = Convert.ToString(row["Char2"]);

            // MainKey
            ryousyuusyoRow.MainKey = Convert.ToByte(row["MainKey"]);

            // StoreCD
            ryousyuusyoRow.StoreCD = Convert.ToString(row["StoreCD"]);

            // 更新日付
            ryousyuusyoRow.ChangeDate = ConvertDateTime(row["ChangeDate"], false);

            // 売上日付
            ryousyuusyoRow.SalesDate = ConvertDateTime(row["SalesDate"], false);
            // データセットに追加
            ryousyuusyoDataSet.D_SelectData_ForTempoRegiRyousyuusyo.Rows.Add(ryousyuusyoRow);

            // 出力
            // mmmas
            try
            {
              
                var report = new TempoRegiRyousyuusyo_Report();
                report.SetDataSource(ryousyuusyoDataSet);
                report.Refresh();
              //  MessageBox.Show(report.PrintOptions.PrinterName + "  "+ StorePrinterName);
                report.PrintOptions.PrinterName = StorePrinterName;
             //   MessageBox.Show(report.PrintOptions.PrinterName);
                report.PrintToPrinter(0, false, 0, 0);
              //  MessageBox.Show(report.PrintOptions.PrinterName);
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// レシート出力
        /// </summary>
        /// <param name="data">データ</param>
        private void OutputReceipt(DataTable data)
        {
            var receiptDataSet = new Receipt_DataSet();

            for (var index = 0; index < data.Rows.Count; index++)
            {
                var row = data.Rows[index];

                if (string.IsNullOrWhiteSpace(ConvertDateTime(row["IssueDateTime"], false)))
                {
                    // 発行日時がないデータは出力対象外
                    continue;
                }

                // 共通データ
                var salesNO = Convert.ToString(row["SalesNO"]);                             // 売上番号
                var storeReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 店舗レシート表記
                var staffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 担当レシート表記

                #region 店舗データ
                if (receiptDataSet.StoreTable.Rows.Count == 0)
                {
                    var store = receiptDataSet.StoreTable.NewStoreTableRow();
                    store.Logo = (byte[])row["Logo"];
                    store.CompanyName = Convert.ToString(row["CompanyName"]);
                    store.StoreName = Convert.ToString(row["StoreName"]);                       // 店舗名
                    store.Address1 = Convert.ToString(row["Address1"]);                         // 住所1
                    store.Address2 = Convert.ToString(row["Address2"]);                         // 住所2
                    store.TelephoneNO = Convert.ToString(row["TelephoneNO"]);                   // 電話番号
                    store.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);       // 店舗レシート表記

                    // メッセージ
                    store.Char3 = Convert.ToString(row["Char3"]);
                    store.Char4 = Convert.ToString(row["Char4"]);
                    //
                    receiptDataSet.StoreTable.Rows.Add(store);
                }
                #endregion // 店舗データ

                #region 販売データ
                var sales = receiptDataSet.SalesTable.NewSalesTableRow();
                sales.StoreReceiptPrint = storeReceiptPrint;                                    // 店舗レシート表記
                sales.StaffReceiptPrint = staffReceiptPrint;                                    // 担当レシート表記
                sales.SalesNO = salesNO;                                                        // 売上番号
                sales.IssueDate = ConvertDateTime(row["IssueDateTime"], true);                  // 発行日
                sales.IssueDateTime = ConvertDateTime(row["IssueDateTime"], false);             // 発行日時

                // 再発行日時
                var reIssueDateTime = ConvertDateTime(row["ReIssueDateTime"], false);
                sales.ReIssueDateTime = string.IsNullOrWhiteSpace(reIssueDateTime) ? null : reIssueDateTime;
                sales.ReIssue = string.IsNullOrWhiteSpace(reIssueDateTime) ? null : "再発行";

                sales.JanCD = Convert.ToString(row["JanCD"]);                                   // JANCD
                var kakaku = ConvertDecimal(row["Kakaku"]);                                     // 価格

                //数量が0の場合は1として処理、その場合、単価は価格を割り当てる
                var salesSU = ConvertDecimal(row["SalesSU"]);
                if (string.IsNullOrWhiteSpace(salesSU))
                {
                    sales.SalesSU = "1";                                                        // 数量
                    sales.SalesUnitPrice = kakaku;                                              // 単価
                }
                else
                {
                    sales.SalesSU = salesSU;                                                    // 数量
                    sales.SalesUnitPrice = @"@" + ConvertDecimal(row["SalesUnitPrice"]);        // 単価
                }

                sales.SalesGaku = kakaku;                                                       // 価格
                sales.SalesTax = ConvertDecimal(row["SalesTax"]);                               // 売上消費税額
                sales.SalesTaxRate = ConvertDecimal(row["SalesTaxRate"]) == "8" ? "*" : "";     // 税率
                sales.TotalGaku = ConvertDecimal(row["TotalGaku"]);                             // 販売合計額

                // 商品名
                var encoding = System.Text.Encoding.GetEncoding("Shift_JIS");

                var skuShortName = Convert.ToString(row["SKUShortName"]);
                byte[] skuShortNameBT = encoding.GetBytes(skuShortName);
                if (skuShortNameBT.Length < SKU_SHORTNAME_LENGTH)
                {
                    sales.SKUShortName1 = skuShortName;
                    sales.SKUShortName2 = "";
                }
                else
                {
                    sales.SKUShortName1 = encoding.GetString(skuShortNameBT, 0, SKU_SHORTNAME_LENGTH);
                    sales.SKUShortName2 = encoding.GetString(skuShortNameBT, SKU_SHORTNAME_LENGTH, skuShortNameBT.Length - SKU_SHORTNAME_LENGTH);
                }

                #region 合計データ
                sales.SumSalesSU = ConvertDecimal(row["SumSalesSU"]);                           // 小計数量
                sales.Subtotal = ConvertDecimal(row["Subtotal"]);                               // 小計金額
                sales.TargetAmount8 = ConvertDecimal(row["TargetAmount8"]);                     // 消費税対象額8%
                sales.ConsumptionTax8 = ConvertDecimal(row["ConsumptionTax8"]);                 // 内消費税等8%
                sales.TargetAmount10 = ConvertDecimal(row["TargetAmount10"]);                   // 消費税対象額10%
                sales.ConsumptionTax10 = ConvertDecimal(row["ConsumptionTax10"]);               // 内消費税等10%
                sales.Total = ConvertDecimal(row["Total"]);                                     // 合計
                #endregion // 合計データ

                #region 支払方法
                sales.PaymentName1 = Convert.ToString(row["PaymentName1"]);                     // 支払方法名1
                sales.PaymentAmount1 = ConvertDecimal(row["AmountPay1"]);                       // 支払方法額1
                sales.PaymentName2 = Convert.ToString(row["PaymentName2"]);                     // 支払方法名2
                sales.PaymentAmount2 = ConvertDecimal(row["AmountPay2"]);                       // 支払方法額2
                sales.PaymentName3 = Convert.ToString(row["PaymentName3"]);                     // 支払方法名3
                sales.PaymentAmount3 = ConvertDecimal(row["AmountPay3"]);                       // 支払方法額3
                sales.PaymentName4 = Convert.ToString(row["PaymentName4"]);                     // 支払方法名4
                sales.PaymentAmount4 = ConvertDecimal(row["AmountPay4"]);                       // 支払方法額4
                sales.PaymentName5 = Convert.ToString(row["PaymentName5"]);                     // 支払方法名5
                sales.PaymentAmount5 = ConvertDecimal(row["AmountPay5"]);                       // 支払方法額5
                sales.PaymentName6 = Convert.ToString(row["PaymentName6"]);                     // 支払方法名6
                sales.PaymentAmount6 = ConvertDecimal(row["AmountPay6"]);                       // 支払方法額6
                sales.PaymentName7 = Convert.ToString(row["PaymentName7"]);                     // 支払方法名7
                sales.PaymentAmount7 = ConvertDecimal(row["AmountPay7"]);                       // 支払方法額7
                sales.PaymentName8 = Convert.ToString(row["PaymentName8"]);                     // 支払方法名8
                sales.PaymentAmount8 = ConvertDecimal(row["AmountPay8"]);                       // 支払方法額8
                sales.PaymentName9 = Convert.ToString(row["PaymentName9"]);                     // 支払方法名9
                sales.PaymentAmount9 = ConvertDecimal(row["AmountPay9"]);                       // 支払方法額9
                sales.PaymentName10 = Convert.ToString(row["PaymentName10"]);                   // 支払方法名10
                sales.PaymentAmount10 = ConvertDecimal(row["AmountPay10"]);                     // 支払方法額10
                #endregion // 支払方法

                #region お釣りデータ
                sales.Refund = ConvertDecimal(row["Refund"]);                                   // 釣銭
                sales.SalesLastPoint = ConvertDecimal(row["SalesLastPoint"]);                   // 今回ポイント
                sales.CustomerLastPoint = ConvertDecimal(row["CustomerLastPoint"]);             // 合計ポイント 
                sales.CustomerKBN = Convert.ToString(row["CustomerKBN"]);
                sales.CustomerLastPoint = string.IsNullOrWhiteSpace(sales.CustomerLastPoint) ? "0" : sales.CustomerLastPoint;
                #endregion // お釣りデータ

                #endregion // 販売データ

                sales.Row = receiptDataSet.SalesTable.Where(v => v.IssueDateTime == sales.IssueDateTime && v.SalesNO == sales.SalesNO).Count() + 1;

                receiptDataSet.SalesTable.Rows.Add(sales);
            }

            var report = new TempoRegiRyousyuusyo_Receipt();
            report.SetDataSource(receiptDataSet);
            report.Refresh();
            report.PrintOptions.PrinterName = StorePrinterName;
            report.PrintToPrinter(0, false, 0, 0);

            // 発行済更新、ログ更新
            bl.D_UpdateDepositHistory(txtSalesNO.Text, true, InOperatorCD, InProgramID, InPcID);
        }
        /// <summary>
        /// 日時をyyyy/MM/dd hh:miで取得
        /// </summary>
        /// <param name="value">元の日時</param>
        /// <returns>日時</returns>
        private string ConvertDateTime(object value, bool dateOnly)
        {
            var result = string.Empty;

            var dateTime = Convert.ToString(value);
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                result = dateOnly ? dateTime.Substring(0, "yyyy/MM/dd".Length) : dateTime.Substring(0, dateTime.LastIndexOf(':'));
            }

            return result;
        }

        /// <summary>
        /// Decimal型で取得
        /// </summary>
        /// <param name="value">元の値</param>
        /// <returns>Decimal型の値</returns>
        /// <remarks>NULL値は0で返す</remarks>
        private string ConvertDecimal(object value)
        {
            var result = 0;

            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return "";
            }
            else
            {
                var pos = value.ToString().LastIndexOf('.');
                if (pos < 0)
                {
                    result = string.IsNullOrWhiteSpace(value.ToString()) ? 0 : Convert.ToInt32(value.ToString());
                }
                else
                {
                    result = string.IsNullOrWhiteSpace(value.ToString()) ? 0 : Convert.ToInt32(value.ToString().Substring(0, pos));
                }

                return string.Format("{0:#,0}", result);
            }
        }

        /// <summary>
        /// 文字列を指定文字数で分割
        /// </summary>
        /// <param name="value">文字列</param>
        /// <param name="count">分割する文字数</param>
        /// <returns>分割した文字列の配列</returns>
        private string[] CountSplit(string value, int count)
        {
            var result = new List<string>();
            var length = (int)Math.Ceiling((double)value.Length / count);

            for (var index = 0; index < length; index++)
            {
                var start = count * index;

                if (value.Length <= start)
                {
                    break;
                }

                if (value.Length < start + count)
                {
                    result.Add(value.Substring(start));
                }
                else
                {
                    result.Add(value.Substring(start, count));
                }
            }

            return result.ToArray();
        }

        private void txtSalesNO_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ErrorCheck())
                {
                    chkRyousyuusho.Focus();
                }
            }
        }

        private void chkRyousyuusho_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                chkReceipt.Focus();
            }
        }

        private void chkReceipt_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPrintDate.Focus();
            }
        }

        private void txtPrintDate_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                chkReissue.Focus();
            }
        }

        private void chkReissue_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnClose.Focus();
            }
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
            
            //////if (Base_DL.iniEntity.IsDM_D30Used && !IsHanbaiTouroku)
            //////{

            //////    Login_BL bbl_1 = new Login_BL();
            //////    if (bbl_1.ReadConfig())
            //////    {
            //////        bbl_1.Display_Service_Update(false);
            //////        Thread.Sleep(2 * 1000);
            //////        bbl_1.Display_Service_Enabled(false);
            //////    }
            //////    else
            //////    {

            //////        bbl_1.Display_Service_Update(false);
            //////        Thread.Sleep(2 * 1000);
            //////        bbl_1.Display_Service_Enabled(false);
            //////    }
            //////    try
            //////    {
            //////        Kill("Display_Service");
            //////    }
            //////    catch (Exception ex)
            //////    {
            //////        MessageBox.Show(ex.StackTrace.ToString());
            //////    }
            //////    if (isForced) cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
            //////}
        }
        private void RunDisplay_Service()  // Make when we want to run display_service
        {
            //////try
            //////{
            //////    if (Base_DL.iniEntity.IsDM_D30Used && !IsHanbaiTouroku)
            //////    {
            //////        cdo.RemoveDisplay(true);
            //////        Login_BL bbl_1 = new Login_BL();
            //////        bbl_1.Display_Service_Update(true);
            //////        bbl_1.Display_Service_Enabled(true);
            //////    }
            //////}
            //////catch (Exception ex)
            //////{
            //////    MessageBox.Show("Error in removing display. . .");
            //////}
        }
        private void Start_Display()
        {
            ////////try
            ////////{
            ////////    if (Base_DL.iniEntity.IsDM_D30Used && !IsHanbaiTouroku)
            ////////    {
            ////////        Login_BL bbl_1 = new Login_BL();
            ////////        if (bbl_1.ReadConfig())
            ////////        {
            ////////            bbl_1.Display_Service_Update(false);
            ////////            Thread.Sleep(2 * 1000);
            ////////            bbl_1.Display_Service_Enabled(false);
            ////////        }
            ////////        else
            ////////        {
            ////////            bbl_1.Display_Service_Update(false);
            ////////            Thread.Sleep(2 * 1000);
            ////////            bbl_1.Display_Service_Enabled(false);
            ////////        }
            ////////        Kill("Display_Service");
            ////////    }
            ////////}
            ////////catch (Exception ex) { MessageBox.Show("Cant remove on second time" + ex.StackTrace); }
        }
        EPSON_TM30.CashDrawerOpen cdo = new EPSON_TM30.CashDrawerOpen();
    }
}
