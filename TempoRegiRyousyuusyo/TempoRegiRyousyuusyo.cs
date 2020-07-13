using Base.Client;
using BL;
using System;
using System.Collections.Generic;
using System.Data;
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

        /// <summary>
        /// BL
        /// </summary>
        TempoRegiRyousyuusyo_BL bl = new TempoRegiRyousyuusyo_BL();

        /// <summary>
        /// 店舗レジ領収収書印刷 コンストラクタ
        /// </summary>
        public TempoRegiRyousyuusyo()
        {
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
                // 別プログラムからの呼び出し時

                // フォームを表示させないように最小化してタスクバーにも表示しない
                WindowState = System.Windows.Forms.FormWindowState.Minimized;
                ShowInTaskbar = false;

                txtSalesNO.Text = cmds[(int)CommandLine.SalesNO];
                chkRyousyuusho.Checked = Convert.ToBoolean(Convert.ToInt32(cmds[(int)CommandLine.RyousyuushoCheck]));
                chkReceipt.Checked = Convert.ToBoolean(Convert.ToInt32(cmds[(int)CommandLine.ReceiptCheck]));
                txtPrintDate.Text = cmds[(int)CommandLine.PrintDate];
                //chkReissue.Checked = Convert.ToBoolean(cmds[(int)CommandLine.ReissueCheck]);

                // 印刷後そくクローズ
                Print();
                Close();
            }
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
            //txtPrintDate.Clear();
            txtPrintDate.Text = DateTime.Today.ToShortDateString();

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

            if (!bl.D_CheckSalseNO(txtSalesNO.Text))
            {
                // 売上データなし
                bl.ShowMessage("E138");
                txtSalesNO.Focus();
                return false;
            }
            else
            {
                // 売上データあり
                if (bl.D_CheckDeleteSalseNO(txtSalesNO.Text))
                {
                    // 削除済み売上データあり
                    bl.ShowMessage("E140");
                    txtSalesNO.Focus();
                    return false;
                }
            }

            return true;
        }

        protected override void EndSec()
        {
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
                    if (chkRyousyuusho.Checked)
                    {
                        // 領収書チェックボックスにチェックあり
                        var ryousyuusyo = bl.D_RyousyuusyoSelect(txtSalesNO.Text, txtPrintDate.Text);
                        if (ryousyuusyo.Rows.Count > 0)
                        {
                            OutputRyouusyusyo(ryousyuusyo);
                        }
                        else
                        {
                            bl.ShowMessage("E128");
                            txtSalesNO.Focus();
                        }
                    }

                    if (chkReceipt.Checked)
                    {
                        // レシートチェックボックスにチェックあり

                        // 店舗取引履歴
                        var receiptData = bl.D_ReceiptSelect(txtSalesNO.Text, chkReissue.Checked);
                        if (receiptData.Rows.Count > 0)
                        {
                            OutputReceipt(receiptData);
                        }
                        else
                        {
                            bl.ShowMessage("E128");
                            txtSalesNO.Focus();
                        }
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
            ryousyuusyoRow.UriageDateTime = ConvertDateTime(row["UriageDateTime"]);

            // 相手名
            ryousyuusyoRow.AiteName = Convert.ToString(row["AiteName"]);

            // 売上額
            ryousyuusyoRow.SalesGaku = ConvertDecimal(row["SalesGaku"]);

            // 消費税額
            ryousyuusyoRow.SalesTax = ConvertDecimal(row["SalesTax"]);

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
            ryousyuusyoRow.ChangeDate = ConvertDateTime(row["ChangeDate"]);

            // 売上日付
            ryousyuusyoRow.SalesDate = ConvertDateTime(row["SalesDate"]);

            // データセットに追加
            ryousyuusyoDataSet.D_SelectData_ForTempoRegiRyousyuusyo.Rows.Add(ryousyuusyoRow);

            // 出力
            var report = new TempoRegiRyousyuusyo_Report();
            report.SetDataSource(ryousyuusyoDataSet);
            report.Refresh();
            report.PrintOptions.PrinterName = StorePrinterName;
            report.PrintToPrinter(0, false, 0, 0);
        }

        /// <summary>
        /// レシート出力
        /// </summary>
        /// <param name="data">データ</param>
        private void OutputReceipt(DataTable data)
        {
            var row = data.Rows[0];

            var receiptDataSet = new Receipt_DataSet();
            var receiptRow = receiptDataSet.ReceiptTable.NewReceiptTableRow();

            // ロゴ
            //receiptRow.Logo = Convert.ToBase64String((byte[])row["Logo"]);
            receiptRow.Logo = (byte[])row["Logo"];

            // 会社名/店舗名/住所1/住所2/電話番号
            receiptRow.CompanyName = Convert.ToString(row["CompanyName"]);
            receiptRow.StoreName = Convert.ToString(row["StoreName"]);
            receiptRow.Address1 = Convert.ToString(row["Address1"]);
            receiptRow.Address2 = Convert.ToString(row["Address2"]);
            receiptRow.TelphoneNO = "電話 " + Convert.ToString(row["TelephoneNO"]);

            // メッセージ
            receiptRow.Char3 = Convert.ToString(row["Char3"]);
            receiptRow.Char4 = Convert.ToString(row["Char4"]);

            // 発行日
            receiptRow.DepositDateTime = ConvertDateTime(row["DepositDateTime"]);

            // 再発行日
            receiptRow.IssuedDatetime = ConvertDateTime(row["IssuedDatetime"]);
            receiptRow.Issued = string.IsNullOrWhiteSpace(receiptRow.IssuedDatetime) ? null : "再発行";

            for (var index = 0; index < data.Rows.Count; index++)
            {
                row = data.Rows[index];

                var detailRow = receiptDataSet.ReceiptDetailTable.NewReceiptDetailTableRow();

                // SalesCD
                detailRow.SalesNO = Convert.ToString(row["SalesNO"]);

                // JanCD
                detailRow.JanCD = Convert.ToString(row["JanCD"]);

                // 商品名
                var skuShortNames = CountSplit(Convert.ToString(row["SKUShortName"]), 16);
                detailRow.SKUShortName1 = skuShortNames[0];

                if (skuShortNames.Length > 1)
                {
                    detailRow.SKUShortName2 = skuShortNames[1];
                }
                else
                {
                    detailRow.SKUShortName2 = "";
                }

                // 単価
                detailRow.SalesUnitPrice = ConvertDecimal(row["SalesUnitPrice"]);

                // 数量
                detailRow.SalesSu = ConvertDecimal(row["SalesSu"]);

                // 価格
                detailRow.SalesGaku = ConvertDecimal(row["SalesGaku"]);

                //
                receiptDataSet.ReceiptDetailTable.Rows.Add(detailRow);
            }

            // 合計数量
            receiptRow.SumSalesSu = ConvertDecimal(row["SumSalesSu"]);

            // 合計価格
            receiptRow.SumSalesGaku = ConvertDecimal(row["SumSalesGaku"]);

            // ８％対象額
            receiptRow.SalesHontaiGaku8 = ConvertDecimal(row["SalesHontaiGaku8"]);

            // 外税８％
            receiptRow.SalesTax8 = ConvertDecimal(row["SalesTax8"]);

            // １０％対象額
            receiptRow.SalesHontaiGaku10 = ConvertDecimal(row["SalesHontaiGaku10"]);

            // 外税１０％
            receiptRow.SalesTax10 = ConvertDecimal(row["SalesTax10"]);

            // 価格＋税額の合計
            receiptRow.TotalSalesGaku = ConvertDecimal(row["TotalSalesGaku"]);

            // お釣り/今回ポイント/合計ポイント
            receiptRow.Refund = ConvertDecimal(row["Refund"]);
            receiptRow.SalesLastPoint = ConvertDecimal(row["SalesLastPoint"]);
            receiptRow.CustomerLasPoint = ConvertDecimal(row["CustomerLasPoint"]);

            // 担当レシート表記/店舗レシート表記/売上番号
            receiptRow.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);
            receiptRow.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);
            receiptRow.SalesNO = Convert.ToString(row["SalesNO"]);

            // お預かり1～10
            for (var index = 1; index <= 10; index++)
            {
                var denominationName = Convert.ToString(row["DenominationName" + index]);
                var depositGaku = ConvertDecimal(row["DepositGaku" + index]);

                switch (index)
                {
                    case 1:
                        receiptRow.DenominationName1 = denominationName;
                        receiptRow.DepositGaku1 = depositGaku;
                        break;

                    case 2:
                        receiptRow.DenominationName2 = denominationName;
                        receiptRow.DepositGaku2 = depositGaku;
                        break;

                    case 3:
                        receiptRow.DenominationName3 = denominationName;
                        receiptRow.DepositGaku3 = depositGaku;
                        break;

                    case 4:
                        receiptRow.DenominationName4 = denominationName;
                        receiptRow.DepositGaku4 = depositGaku;
                        break;

                    case 5:
                        receiptRow.DenominationName5 = denominationName;
                        receiptRow.DepositGaku5 = depositGaku;
                        break;

                    case 6:
                        receiptRow.DenominationName6 = denominationName;
                        receiptRow.DepositGaku6 = depositGaku;
                        break;

                    case 7:
                        receiptRow.DenominationName7 = denominationName;
                        receiptRow.DepositGaku7 = depositGaku;
                        break;

                    case 8:
                        receiptRow.DenominationName8 = denominationName;
                        receiptRow.DepositGaku8 = depositGaku;
                        break;

                    case 9:
                        receiptRow.DenominationName9 = denominationName;
                        receiptRow.DepositGaku9 = depositGaku;
                        break;

                    case 10:
                        receiptRow.DenominationName10 = denominationName;
                        receiptRow.DepositGaku10 = depositGaku;
                        break;

                    default:
                        break;
                }
            }

            receiptDataSet.ReceiptTable.Rows.Add(receiptRow);

            var report = new TempoRegiRyousyuusyo_Receipt();
            report.SetDataSource(receiptDataSet);
            report.Refresh();
            report.PrintOptions.PrinterName = StorePrinterName;
            report.PrintToPrinter(0, false, 0, 0);

            // 発行済更新、ログ更新
            bl.D_UpdateDepositHistory(txtSalesNO.Text, true, InOperatorCD, InProgramID, InPcID);

            // ジャーナル印刷
            var journal = new TempoRegiRyousyuusyo_Journal();
            journal.SetDataSource(receiptDataSet);
            journal.Refresh();
            journal.PrintOptions.PrinterName = StorePrinterName;
            journal.PrintToPrinter(0, false, 0, 0);
        }

        /// <summary>
        /// 日時をyyyy/MM/dd hh:miで取得
        /// </summary>
        /// <param name="value">元の日時</param>
        /// <returns>日時</returns>
        private string ConvertDateTime(object value)
        {
            var dateTime = Convert.ToString(value);
            return string.IsNullOrWhiteSpace(dateTime) ? "" : dateTime.Substring(0, dateTime.LastIndexOf(':'));
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
    }
}
