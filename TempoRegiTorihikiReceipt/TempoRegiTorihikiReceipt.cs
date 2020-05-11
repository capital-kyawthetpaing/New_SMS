using Base.Client;
using BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using TempoRegiTorihikiReceipt.Reports;

namespace TempoRegiTorihikiReceipt
{
    public partial class TempoRegiTorihikiReceipt : ShopBaseForm
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

            /// <summary>入出金区分</summary>
            DepositSection,

            /// <summary>入出金番号</summary>
            DepositNO,
        }

        /// <summary>
        /// 入出金区分
        /// </summary>
        private int DepositSection { get; set; }

        /// <summary>
        /// 入出金番号
        /// </summary>
        private int DepositNO { get; set; }

        /// <summary>
        /// BL
        /// </summary>
        TempoRegiTorihikiReceipt_BL bl = new TempoRegiTorihikiReceipt_BL();

        /// <summary>
        /// 本番用プリンタ名
        /// </summary>
        private string PRINTER = "EPSON TM-m30 Receipt";

        /// <summary>
        /// 店舗ジャーナル印刷 コンストラクタ
        /// </summary>
        public TempoRegiTorihikiReceipt()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 画面読み込み処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempoRegiJournal_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiTorihikiReceipt";
            string data = InOperatorCD;

            StartProgram();

            // フォームを表示させないように最小化してタスクバーにも表示しない
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            //コマンドライン引数を配列で取得する
            string[] cmds = Environment.GetCommandLineArgs();
            if (cmds.Length - 1 > (int)ECmdLine.PcID)
            {
                DepositSection = Convert.ToInt32(cmds[(int)CommandLine.DepositSection]);
                DepositNO = Convert.ToInt32(cmds[(int)CommandLine.DepositNO]);

                // 印刷
                Print();
            }

            #region DEBUG用
            //DepositSection = 2;
            //DepositNO = 17; // 37

            //DepositSection = 3;
            //DepositNO = 21;

            //DepositSection = 5;
            //DepositNO = 39;

            //DepositSection = 6;
            //DepositNO = 40;

            //Print();
            #endregion

            EndSec();
        }

        /// <summary>
        /// 画面終了処理
        /// </summary>
        protected override void EndSec()
        {
            this.Close();
        }

        /// <summary>
        /// 印刷実行
        /// </summary>
        private void Print()
        {
            switch(DepositSection)
            {
                case 2:
                    OutputMiscDepositReceipt();
                    break;

                case 3:
                    OutputMiscPaymentReceipt();
                    break;

                case 5:
                    OutputExchangeReceipt();
                    break;

                case 6:
                    OutputChangePreparationReceipt();
                    break;
            }
        }

        #region 雑入金・入金取引レシート出力
        /// <summary>
        /// 雑入金・入金取引レシート出力
        /// </summary>
        private void OutputMiscDepositReceipt()
        {
            var torihikiReceiptDataSet = new TorihikiReceiptDataSet();

            var miscDeposit = bl.D_MiscDepositSelect(DepositNO);
            if(miscDeposit.Rows.Count > 0)
            {
                CreateMiscDepositDataSet(miscDeposit.Rows[0], torihikiReceiptDataSet);

                var report = new TempoRegiTorihikiReceipt_MiscDeposit();

                report.SetDataSource(torihikiReceiptDataSet);
                report.Refresh();

                report.PrintOptions.PrinterName = PRINTER;
                report.PrintToPrinter(0, false, 0, 0);
            }

            var deposit = bl.D_DepositSelect(DepositNO);
            if (deposit.Rows.Count > 0)
            {
                CreateDepositDataSet(deposit.Rows[0], torihikiReceiptDataSet);

                var report = new TempoRegiTorihikiReceipt_Deposit();

                report.SetDataSource(torihikiReceiptDataSet);
                report.Refresh();

                report.PrintOptions.PrinterName = PRINTER;
                report.PrintToPrinter(0, false, 0, 0);
            }

            if (miscDeposit.Rows.Count == 0 && deposit.Rows.Count == 0)
            {
                // 雑入金・入金ともに対象データなしの場合
                bl.ShowMessage("E128");
            }
        }
        #endregion // 雑入金・入金取引レシート出力

        #region 雑出金取引レシート出力
        /// <summary>
        /// 雑出金取引レシート出力
        /// </summary>
        private void OutputMiscPaymentReceipt()
        {
            var torihikiReceiptDataSet = new TorihikiReceiptDataSet();

            var miscPayment = bl.D_MiscPaymentSelect(DepositNO);
            if (miscPayment.Rows.Count > 0)
            {
                CreateMiscPaymentDataSet(miscPayment.Rows[0], torihikiReceiptDataSet);

                var report = new TempoRegiTorihikiReceipt_MiscPayment();

                report.SetDataSource(torihikiReceiptDataSet);
                report.Refresh();

                report.PrintOptions.PrinterName = PRINTER;
                report.PrintToPrinter(0, false, 0, 0);
            }
            else
            {
                // 対象データなしの場合
                bl.ShowMessage("E128");
            }
        }
        #endregion // 雑出金取引レシート出力

        #region 両替取引レシート出力
        /// <summary>
        /// 両替取引レシート出力
        /// </summary>
        private void OutputExchangeReceipt()
        {
            var torihikiReceiptDataSet = new TorihikiReceiptDataSet();

            var exchange = bl.D_ExchangeSelect(DepositNO);
            if (exchange.Rows.Count > 0)
            {
                CreateExchangeDataSet(exchange.Rows[0], torihikiReceiptDataSet);

                var report = new TempoRegiTorihikiReceipt_Exchange();

                report.SetDataSource(torihikiReceiptDataSet);
                report.Refresh();

                report.PrintOptions.PrinterName = PRINTER;
                report.PrintToPrinter(0, false, 0, 0);
            }
            else
            {
                // 対象データなしの場合
                bl.ShowMessage("E128");
            }
        }
        #endregion // 両替取引レシート出力

        #region 釣銭準備取引レシート出力
        /// <summary>
        /// 釣銭準備取引レシート出力
        /// </summary>
        private void OutputChangePreparationReceipt()
        {
            var torihikiReceiptDataSet = new TorihikiReceiptDataSet();

            var changePreparation = bl.D_ChangePreparationSelect(DepositNO);
            if (changePreparation.Rows.Count > 0)
            {
                CreateChangePreparationDataSet(changePreparation.Rows[0], torihikiReceiptDataSet);

                var report = new TempoRegiTorihikiReceipt_ChangePreparation();

                report.SetDataSource(torihikiReceiptDataSet);
                report.Refresh();

                report.PrintOptions.PrinterName = PRINTER;
                report.PrintToPrinter(0, false, 0, 0);
            }
            else
            {
                // 対象データなしの場合
                bl.ShowMessage("E128");
            }
        }
        #endregion // 釣銭準備取引レシート出力

        #region 雑入金データセット
        /// <summary>
        /// 雑入金データをデータセットに設定
        /// </summary>
        /// <param name="row">雑入金データ</param>
        /// <param name="dataSet">データセット</param>
        private void CreateMiscDepositDataSet(DataRow row, TorihikiReceiptDataSet dataSet)
        {
            var miscDeposit = dataSet.MiscDepositTable.NewMiscDepositTableRow();
            miscDeposit.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);     // 雑入金店舗レシート表記
            miscDeposit.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);     // 雑入金担当レシート表記
            miscDeposit.RegistDate = ConvertDateTime(row["RegistDate"]);                    // 雑入金登録日
            miscDeposit.Name1 = Convert.ToString(row["Name1"]);                             // 雑入金名1
            miscDeposit.Amount1 = ConvertDecimal(row["Amount1"]);                           // 雑入金額1
            miscDeposit.Name2 = Convert.ToString(row["Name2"]);                             // 雑入金名2
            miscDeposit.Amount2 = ConvertDecimal(row["Amount2"]);                           // 雑入金額2
            miscDeposit.Name3 = Convert.ToString(row["Name3"]);                             // 雑入金名3
            miscDeposit.Amount3 = ConvertDecimal(row["Amount3"]);                           // 雑入金額3
            miscDeposit.Name4 = Convert.ToString(row["Name4"]);                             // 雑入金名4
            miscDeposit.Amount4 = ConvertDecimal(row["Amount4"]);                           // 雑入金額4
            miscDeposit.Name5 = Convert.ToString(row["Name5"]);                             // 雑入金名5
            miscDeposit.Amount5 = ConvertDecimal(row["Amount5"]);                           // 雑入金額5
            miscDeposit.Name6 = Convert.ToString(row["Name6"]);                             // 雑入金名6
            miscDeposit.Amount6 = ConvertDecimal(row["Amount6"]);                           // 雑入金額6
            miscDeposit.Name7 = Convert.ToString(row["Name7"]);                             // 雑入金名7
            miscDeposit.Amount7 = ConvertDecimal(row["Amount7"]);                           // 雑入金額7
            miscDeposit.Name8 = Convert.ToString(row["Name8"]);                             // 雑入金名8
            miscDeposit.Amount8 = ConvertDecimal(row["Amount8"]);                           // 雑入金額8
            miscDeposit.Name9 = Convert.ToString(row["Name9"]);                             // 雑入金名9
            miscDeposit.Amount9 = ConvertDecimal(row["Amount9"]);                           // 雑入金額9
            miscDeposit.Name10 = Convert.ToString(row["Name10"]);                           // 雑入金名10
            miscDeposit.Amount10 = ConvertDecimal(row["Amount10"]);                         // 雑入金額10

            dataSet.MiscDepositTable.Rows.Add(miscDeposit);
        }
        #endregion // 雑入金データセット

        #region 入金データセット
        /// <summary>
        /// 入金データをデータセットに設定
        /// </summary>
        /// <param name="row">入金データ</param>
        /// <param name="dataSet">データセット</param>
        private void CreateDepositDataSet(DataRow row, TorihikiReceiptDataSet dataSet)
        {
            var deposit = dataSet.DepositTable.NewDepositTableRow();
            deposit.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 入金店舗レシート表記
            deposit.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 入金担当レシート表記
            deposit.RegistDate = ConvertDateTime(row["RegistDate"]);                        // 入金登録日
            deposit.CustomerCD = Convert.ToString(row["CustomerCD"]);                       // 入金元CD
            deposit.CustomerName = Convert.ToString(row["CustomerName"]);                   // 入金元名
            deposit.Name1 = Convert.ToString(row["Name1"]);                                 // 入金区分名1
            deposit.Amount1 = ConvertDecimal(row["Amount1"]);                               // 入金額1
            deposit.Name2 = Convert.ToString(row["Name2"]);                                 // 入金区分名2
            deposit.Amount2 = ConvertDecimal(row["Amount2"]);                               // 入金額2
            deposit.Name3 = Convert.ToString(row["Name3"]);                                 // 入金区分名3
            deposit.Amount3 = ConvertDecimal(row["Amount3"]);                               // 入金額3
            deposit.Name4 = Convert.ToString(row["Name4"]);                                 // 入金区分名4
            deposit.Amount4 = ConvertDecimal(row["Amount4"]);                               // 入金額4
            deposit.Name5 = Convert.ToString(row["Name5"]);                                 // 入金区分名5
            deposit.Amount5 = ConvertDecimal(row["Amount5"]);                               // 入金額5
            deposit.Name6 = Convert.ToString(row["Name6"]);                                 // 入金区分名6
            deposit.Amount6 = ConvertDecimal(row["Amount6"]);                               // 入金額6
            deposit.Name7 = Convert.ToString(row["Name7"]);                                 // 入金区分名7
            deposit.Amount7 = ConvertDecimal(row["Amount7"]);                               // 入金額7
            deposit.Name8 = Convert.ToString(row["Name8"]);                                 // 入金区分名8
            deposit.Amount8 = ConvertDecimal(row["Amount8"]);                               // 入金額8
            deposit.Name9 = Convert.ToString(row["Name9"]);                                 // 入金区分名9
            deposit.Amount9 = ConvertDecimal(row["Amount9"]);                               // 入金額9
            deposit.Name10 = Convert.ToString(row["Name10"]);                               // 入金区分名10
            deposit.Amount10 = ConvertDecimal(row["Amount10"]);                             // 入金額10

            dataSet.DepositTable.Rows.Add(deposit);
        }
        #endregion // 入金データセット

        #region 雑出金データセット
        /// <summary>
        /// 雑出金データをデータセットに設定
        /// </summary>
        /// <param name="row">雑出金データ</param>
        /// <param name="dataSet">データセット</param>
        private void CreateMiscPaymentDataSet(DataRow row, TorihikiReceiptDataSet dataSet)
        {
            var miscPayment = dataSet.MiscPaymentTable.NewMiscPaymentTableRow();
            miscPayment.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 雑出金店舗レシート表記
            miscPayment.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 雑出金担当レシート表記
            miscPayment.RegistDate = ConvertDateTime(row["RegistDate"]);                        // 雑出金登録日
            miscPayment.Name1 = Convert.ToString(row["Name1"]);                                 // 雑出金区分名1
            miscPayment.Amount1 = ConvertDecimal(row["Amount1"]);                               // 雑出金額1
            miscPayment.Name2 = Convert.ToString(row["Name2"]);                                 // 雑出金区分名2
            miscPayment.Amount2 = ConvertDecimal(row["Amount2"]);                               // 雑出金額2
            miscPayment.Name3 = Convert.ToString(row["Name3"]);                                 // 雑出金区分名3
            miscPayment.Amount3 = ConvertDecimal(row["Amount3"]);                               // 雑出金額3
            miscPayment.Name4 = Convert.ToString(row["Name4"]);                                 // 雑出金区分名4
            miscPayment.Amount4 = ConvertDecimal(row["Amount4"]);                               // 雑出金額4
            miscPayment.Name5 = Convert.ToString(row["Name5"]);                                 // 雑出金区分名5
            miscPayment.Amount5 = ConvertDecimal(row["Amount5"]);                               // 雑出金額5
            miscPayment.Name6 = Convert.ToString(row["Name6"]);                                 // 雑出金区分名6
            miscPayment.Amount6 = ConvertDecimal(row["Amount6"]);                               // 雑出金額6
            miscPayment.Name7 = Convert.ToString(row["Name7"]);                                 // 雑出金区分名7
            miscPayment.Amount7 = ConvertDecimal(row["Amount7"]);                               // 雑出金額7
            miscPayment.Name8 = Convert.ToString(row["Name8"]);                                 // 雑出金区分名8
            miscPayment.Amount8 = ConvertDecimal(row["Amount8"]);                               // 雑出金額8
            miscPayment.Name9 = Convert.ToString(row["Name9"]);                                 // 雑出金区分名9
            miscPayment.Amount9 = ConvertDecimal(row["Amount9"]);                               // 雑出金額9
            miscPayment.Name10 = Convert.ToString(row["Name10"]);                               // 雑出金区分名10
            miscPayment.Amount10 = ConvertDecimal(row["Amount10"]);                             // 雑出金額10

            dataSet.MiscPaymentTable.Rows.Add(miscPayment);
        }
        #endregion // 雑出金データセット

        #region 両替データセット
        /// <summary>
        /// 両替データをデータセットに設定
        /// </summary>
        /// <param name="row">両替データ</param>
        /// <param name="dataSet">データセット</param>
        private void CreateExchangeDataSet(DataRow row, TorihikiReceiptDataSet dataSet)
        {
            var exchange = dataSet.ExchangeTable.NewExchangeTableRow();
            exchange.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 両替店舗レシート表記
            exchange.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 両替担当レシート表記
            exchange.RegistDate = ConvertDateTime(row["RegistDate"]);                        // 両替登録日
            exchange.ExchangeCount = ConvertDecimal(row["ExchangeCount"]);                   // 両替回数
            exchange.Name1 = Convert.ToString(row["Name1"]);                                 // 両替名1
            exchange.Amount1 = ConvertDecimal(row["Amount1"]);                               // 両替額1
            exchange.Denomination1 = Convert.ToString(row["Denomination1"]);                 // 両替紙幣1
            exchange.Count1 = ConvertDecimal(row["Count1"]);                                 // 両替枚数1
            exchange.Name2 = Convert.ToString(row["Name2"]);                                 // 両替名2
            exchange.Amount2 = ConvertDecimal(row["Amount2"]);                               // 両替額2
            exchange.Denomination2 = Convert.ToString(row["Denomination2"]);                 // 両替紙幣2
            exchange.Count2 = ConvertDecimal(row["Count2"]);                                 // 両替枚数2
            exchange.Name3 = Convert.ToString(row["Name3"]);                                 // 両替名3
            exchange.Amount3 = ConvertDecimal(row["Amount3"]);                               // 両替額3
            exchange.Denomination3 = Convert.ToString(row["Denomination3"]);                 // 両替紙幣3
            exchange.Count3 = ConvertDecimal(row["Count3"]);                                 // 両替枚数3
            exchange.Name4 = Convert.ToString(row["Name4"]);                                 // 両替名4
            exchange.Amount4 = ConvertDecimal(row["Amount4"]);                               // 両替額4
            exchange.Denomination4 = Convert.ToString(row["Denomination4"]);                 // 両替紙幣4
            exchange.Count4 = ConvertDecimal(row["Count4"]);                                 // 両替枚数4
            exchange.Name5 = Convert.ToString(row["Name5"]);                                 // 両替名5
            exchange.Amount5 = ConvertDecimal(row["Amount5"]);                               // 両替額5
            exchange.Denomination5 = Convert.ToString(row["Denomination5"]);                 // 両替紙幣5
            exchange.Count5 = ConvertDecimal(row["Count5"]);                                 // 両替枚数5
            exchange.Name6 = Convert.ToString(row["Name6"]);                                 // 両替名6
            exchange.Amount6 = ConvertDecimal(row["Amount6"]);                               // 両替額6
            exchange.Denomination6 = Convert.ToString(row["Denomination6"]);                 // 両替紙幣6
            exchange.Count6 = ConvertDecimal(row["Count6"]);                                 // 両替枚数6
            exchange.Name7 = Convert.ToString(row["Name7"]);                                 // 両替名7
            exchange.Amount7 = ConvertDecimal(row["Amount7"]);                               // 両替額7
            exchange.Denomination7 = Convert.ToString(row["Denomination7"]);                 // 両替紙幣7
            exchange.Count7 = ConvertDecimal(row["Count7"]);                                 // 両替枚数7
            exchange.Name8 = Convert.ToString(row["Name8"]);                                 // 両替名8
            exchange.Amount8 = ConvertDecimal(row["Amount8"]);                               // 両替額8
            exchange.Denomination8 = Convert.ToString(row["Denomination8"]);                 // 両替紙幣8
            exchange.Count8 = ConvertDecimal(row["Count8"]);                                 // 両替枚数8
            exchange.Name9 = Convert.ToString(row["Name9"]);                                 // 両替名9
            exchange.Amount9 = ConvertDecimal(row["Amount9"]);                               // 両替額9
            exchange.Denomination9 = Convert.ToString(row["Denomination9"]);                 // 両替紙幣9
            exchange.Count9 = ConvertDecimal(row["Count9"]);                                 // 両替枚数9
            exchange.Name10 = Convert.ToString(row["Name10"]);                               // 両替名10
            exchange.Amount10 = ConvertDecimal(row["Amount10"]);                             // 両替額10
            exchange.Denomination10 = Convert.ToString(row["Denomination10"]);               // 両替紙幣10
            exchange.Count10 = ConvertDecimal(row["Count10"]);                               // 両替枚数10

            dataSet.ExchangeTable.Rows.Add(exchange);
        }
        #endregion // 両替データセット

        #region 釣銭準備データセット
        /// <summary>
        /// 釣銭準備データをデータセットに設定
        /// </summary>
        /// <param name="row">釣銭準備データ</param>
        /// <param name="dataSet">データセット</param>
        private void CreateChangePreparationDataSet(DataRow row, TorihikiReceiptDataSet dataSet)
        {
            var changePreparation = dataSet.ChangePreparationTable.NewChangePreparationTableRow();
            changePreparation.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 釣銭準備店舗レシート表記
            changePreparation.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 釣銭準備担当レシート表記
            changePreparation.RegistDate = ConvertDateTime(row["RegistDate"]);                        // 釣銭準備登録日
            changePreparation.Name1 = Convert.ToString(row["Name1"]);                                 // 釣銭準備名1
            changePreparation.Amount1 = ConvertDecimal(row["Amount1"]);                               // 釣銭準備額1
            changePreparation.Name2 = Convert.ToString(row["Name2"]);                                 // 釣銭準備名2
            changePreparation.Amount2 = ConvertDecimal(row["Amount2"]);                               // 釣銭準備額2
            changePreparation.Name3 = Convert.ToString(row["Name3"]);                                 // 釣銭準備名3
            changePreparation.Amount3 = ConvertDecimal(row["Amount3"]);                               // 釣銭準備額3
            changePreparation.Name4 = Convert.ToString(row["Name4"]);                                 // 釣銭準備名4
            changePreparation.Amount4 = ConvertDecimal(row["Amount4"]);                               // 釣銭準備額4
            changePreparation.Name5 = Convert.ToString(row["Name5"]);                                 // 釣銭準備名5
            changePreparation.Amount5 = ConvertDecimal(row["Amount5"]);                               // 釣銭準備額5
            changePreparation.Name6 = Convert.ToString(row["Name6"]);                                 // 釣銭準備名6
            changePreparation.Amount6 = ConvertDecimal(row["Amount6"]);                               // 釣銭準備額6
            changePreparation.Name7 = Convert.ToString(row["Name7"]);                                 // 釣銭準備名7
            changePreparation.Amount7 = ConvertDecimal(row["Amount7"]);                               // 釣銭準備額7
            changePreparation.Name8 = Convert.ToString(row["Name8"]);                                 // 釣銭準備名8
            changePreparation.Amount8 = ConvertDecimal(row["Amount8"]);                               // 釣銭準備額8
            changePreparation.Name9 = Convert.ToString(row["Name9"]);                                 // 釣銭準備名9
            changePreparation.Amount9 = ConvertDecimal(row["Amount9"]);                               // 釣銭準備額9
            changePreparation.Name10 = Convert.ToString(row["Name10"]);                               // 釣銭準備名10
            changePreparation.Amount10 = ConvertDecimal(row["Amount10"]);                             // 釣銭準備額10

            dataSet.ChangePreparationTable.Rows.Add(changePreparation);
        }
        #endregion // 雑出金データセット

        /// <summary>
        /// 日時をyyyy/MM/dd hh:miで取得
        /// </summary>
        /// <param name="value">元の日時</param>
        /// <returns>日時</returns>
        private string ConvertDateTime(object value)
        {
            var result = string.Empty;

            var dateTime = Convert.ToString(value);
            if(!string.IsNullOrWhiteSpace(dateTime))
            {
                result = dateTime.Substring(0, dateTime.LastIndexOf(':'));
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
    }
}
