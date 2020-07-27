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
        #region 列挙値

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

            /// <summary>ホスト名</summary>
            HostName,

            /// <summary>モード</summary>
            Mode,

            /// <summary>DepositeNO</summary>
            DepositeNO,
        }

        #endregion // 列挙値

        #region 入出金区分

        /// <summary>
        /// 入出金区分：雑入金/入金
        /// </summary>
        private const string MODE_DEPOSIT = "2";

        /// <summary>
        /// 入出金区分：雑出金
        /// </summary>
        private const string MODE_PAYMENT = "3";

        /// <summary>
        /// 入出金区分：両替
        /// </summary>
        private const string MODE_EXCHANGE = "5";

        /// <summary>
        /// 入出金区分：釣銭準備
        /// </summary>
        private const string MODE_CHANGE_PREPARATION = "6";

        #endregion // 入出金区分

        /// <summary>
        /// モード
        /// </summary>
        private string InputMode { get; set; }

        /// <summary>
        /// 入出金No
        /// </summary>
        private string InputDepositNO { get; set; }

        /// <summary>
        /// BL
        /// </summary>
        TempoRegiTorihikiReceipt_BL bl = new TempoRegiTorihikiReceipt_BL();

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
        private void TempoRegiTorihikiReceipt_Load(object sender, EventArgs e)
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
                InputMode = cmds[(int)CommandLine.Mode];
                InputDepositNO = cmds[(int)CommandLine.DepositeNO];

                // 印刷
                Print();
            }

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
            var torihikiReceiptDataSet = new TorihikiReceiptDataSet();

            switch(InputMode)
            {
                case MODE_DEPOSIT:
                    // 雑入金取得
                    var miscDeposit = bl.D_MiscDepositSelect(InputDepositNO, InOperatorCD);
                    if (miscDeposit.Rows.Count > 0)
                    {
                        CreateMiscDepositDataSet(miscDeposit, torihikiReceiptDataSet);
                    }

                    // 入金取得
                    var deposit = bl.D_DepositSelect(InputDepositNO, InOperatorCD);
                    if (deposit.Rows.Count > 0)
                    {
                        CreateDepositDataSet(deposit, torihikiReceiptDataSet);
                    }

                    if (torihikiReceiptDataSet.MiscDepositTable.Rows.Count > 0 && torihikiReceiptDataSet.DepositTable.Rows.Count == 0)
                    {
                        // 雑入金のみデータあり
                        var reportModeDepositMiscDeposit = new TempoRegiTorihikiReceipt_MiscDeposit();

                        reportModeDepositMiscDeposit.SetDataSource(torihikiReceiptDataSet);
                        reportModeDepositMiscDeposit.Refresh();

                        reportModeDepositMiscDeposit.PrintOptions.PrinterName = StorePrinterName;
                        reportModeDepositMiscDeposit.PrintToPrinter(0, false, 0, 0);
                    }
                    else if (torihikiReceiptDataSet.MiscDepositTable.Rows.Count == 0 && torihikiReceiptDataSet.DepositTable.Rows.Count > 0)
                    {
                        // 入金のみデータあり
                        var reportModeDepositDeposit = new TempoRegiTorihikiReceipt_Deposit();

                        reportModeDepositDeposit.SetDataSource(torihikiReceiptDataSet);
                        reportModeDepositDeposit.Refresh();

                        reportModeDepositDeposit.PrintOptions.PrinterName = StorePrinterName;
                        reportModeDepositDeposit.PrintToPrinter(0, false, 0, 0);
                    }
                    else if (torihikiReceiptDataSet.MiscDepositTable.Rows.Count > 0 && torihikiReceiptDataSet.DepositTable.Rows.Count > 0)
                    {
                        // 雑入金・入金データあり
                        var reportModeDeposit = new TempoRegiTorihikiReceipt_Mode2();

                        reportModeDeposit.SetDataSource(torihikiReceiptDataSet);
                        reportModeDeposit.Refresh();

                        reportModeDeposit.PrintOptions.PrinterName = StorePrinterName;
                        reportModeDeposit.PrintToPrinter(0, false, 0, 0);
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;

                case MODE_PAYMENT:
                    // 雑出金取得
                    var miscPayment = bl.D_MiscPaymentSelect(InputDepositNO, InOperatorCD);
                    if (miscPayment.Rows.Count > 0)
                    {
                        CreateMiscPaymentDataSet(miscPayment, torihikiReceiptDataSet);

                        var reportModePayment = new TempoRegiTorihikiReceipt_MiscPayment();

                        reportModePayment.SetDataSource(torihikiReceiptDataSet);
                        reportModePayment.Refresh();

                        reportModePayment.PrintOptions.PrinterName = StorePrinterName;
                        reportModePayment.PrintToPrinter(0, false, 0, 0);
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;

                case MODE_EXCHANGE:
                    // 両替取得
                    var exchange = bl.D_ExchangeSelect(InputDepositNO, InOperatorCD);
                    if (exchange.Rows.Count > 0)
                    {
                        CreateExchangeDataSet(exchange, torihikiReceiptDataSet);

                        var reportModeExchange = new TempoRegiTorihikiReceipt_Exchange();

                        reportModeExchange.SetDataSource(torihikiReceiptDataSet);
                        reportModeExchange.Refresh();

                        reportModeExchange.PrintOptions.PrinterName = StorePrinterName;
                        reportModeExchange.PrintToPrinter(0, false, 0, 0);
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;

                case MODE_CHANGE_PREPARATION:
                    // 釣銭準備
                    var changePreparation = bl.D_ChangePreparationSelect(InputDepositNO, InOperatorCD);
                    if (changePreparation.Rows.Count > 0)
                    {
                        CreateChangePreparationDataSet(changePreparation, torihikiReceiptDataSet);

                        var reportModeChangePreparation = new TempoRegiTorihikiReceipt_ChangePreparation();

                        reportModeChangePreparation.SetDataSource(torihikiReceiptDataSet);
                        reportModeChangePreparation.Refresh();

                        reportModeChangePreparation.PrintOptions.PrinterName = StorePrinterName;
                        reportModeChangePreparation.PrintToPrinter(0, false, 0, 0);
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;
            }
            
            //var report = new TempoRegiTorihikiReceipt_Journal();

            //report.SetDataSource(torihikiReceiptDataSet);
            //report.Refresh();

            //report.PrintOptions.PrinterName = PRINTER;
            //report.PrintToPrinter(0, false, 0, 0);
        }

        #region 雑入金データセット
        /// <summary>
        /// 雑入金データをデータセットに設定
        /// </summary>
        /// <param name="dataTable">雑入金データテーブル</param>
        /// <param name="dataSet">データセット</param>
        private void CreateMiscDepositDataSet(DataTable dataTable, TorihikiReceiptDataSet dataSet)
        {
            foreach(DataRow row in dataTable.Rows)
            {
                var miscDeposit = dataSet.MiscDepositTable.NewMiscDepositTableRow();

                miscDeposit.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);     // 雑入金店舗レシート表記
                miscDeposit.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);     // 雑入金担当レシート表記
                miscDeposit.RegistDate = ConvertDateTime(row["RegistDate"]);                    // 雑入金登録日
                miscDeposit.DateTime1 = Convert.ToString(row["MiscDepositDate1"]);              // 雑入金日時1
                miscDeposit.Name1 = Convert.ToString(row["MiscDepositName1"]);                  // 雑入金名1
                miscDeposit.Amount1 = ConvertDecimal(row["MiscDepositAmount1"]);                // 雑入金額1
                miscDeposit.DateTime2 = Convert.ToString(row["MiscDepositDate2"]);              // 雑入金日時2
                miscDeposit.Name2 = Convert.ToString(row["MiscDepositName2"]);                  // 雑入金名2
                miscDeposit.Amount2 = ConvertDecimal(row["MiscDepositAmount2"]);                // 雑入金額2
                miscDeposit.DateTime3 = Convert.ToString(row["MiscDepositDate3"]);              // 雑入金日時3
                miscDeposit.Name3 = Convert.ToString(row["MiscDepositName3"]);                  // 雑入金名3
                miscDeposit.Amount3 = ConvertDecimal(row["MiscDepositAmount3"]);                // 雑入金額3
                miscDeposit.DateTime4 = Convert.ToString(row["MiscDepositDate4"]);              // 雑入金日時4
                miscDeposit.Name4 = Convert.ToString(row["MiscDepositName4"]);                  // 雑入金名4
                miscDeposit.Amount4 = ConvertDecimal(row["MiscDepositAmount4"]);                // 雑入金額4
                miscDeposit.DateTime5 = Convert.ToString(row["MiscDepositDate5"]);              // 雑入金日時5
                miscDeposit.Name5 = Convert.ToString(row["MiscDepositName5"]);                  // 雑入金名5
                miscDeposit.Amount5 = ConvertDecimal(row["MiscDepositAmount5"]);                // 雑入金額5
                miscDeposit.DateTime6 = Convert.ToString(row["MiscDepositDate6"]);              // 雑入金日時6
                miscDeposit.Name6 = Convert.ToString(row["MiscDepositName6"]);                  // 雑入金名6
                miscDeposit.Amount6 = ConvertDecimal(row["MiscDepositAmount6"]);                // 雑入金額6
                miscDeposit.DateTime7 = Convert.ToString(row["MiscDepositDate7"]);              // 雑入金日時7
                miscDeposit.Name7 = Convert.ToString(row["MiscDepositName7"]);                  // 雑入金名7
                miscDeposit.Amount7 = ConvertDecimal(row["MiscDepositAmount7"]);                // 雑入金額7
                miscDeposit.DateTime8 = Convert.ToString(row["MiscDepositDate8"]);              // 雑入金日時8
                miscDeposit.Name8 = Convert.ToString(row["MiscDepositName8"]);                  // 雑入金名8
                miscDeposit.Amount8 = ConvertDecimal(row["MiscDepositAmount8"]);                // 雑入金額8
                miscDeposit.DateTime9 = Convert.ToString(row["MiscDepositDate9"]);              // 雑入金日時9
                miscDeposit.Name9 = Convert.ToString(row["MiscDepositName9"]);                  // 雑入金名9
                miscDeposit.Amount9 = ConvertDecimal(row["MiscDepositAmount9"]);                // 雑入金額9
                miscDeposit.DateTime10 = Convert.ToString(row["MiscDepositDate10"]);            // 雑入金日時10
                miscDeposit.Name10 = Convert.ToString(row["MiscDepositName10"]);                // 雑入金名10
                miscDeposit.Amount10 = ConvertDecimal(row["MiscDepositAmount10"]);              // 雑入金額10

                dataSet.MiscDepositTable.Rows.Add(miscDeposit);
            }
        }
        #endregion // 雑入金データセット

        #region 入金データセット
        /// <summary>
        /// 入金データをデータセットに設定
        /// </summary>
        /// <param name="dataTable">入金データテーブル</param>
        /// <param name="dataSet">データセット</param>
        private void CreateDepositDataSet(DataTable dataTable, TorihikiReceiptDataSet dataSet)
        {
            foreach(DataRow row in dataTable.Rows)
            {
                var deposit = dataSet.DepositTable.NewDepositTableRow();

                deposit.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 入金店舗レシート表記
                deposit.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 入金担当レシート表記
                deposit.RegistDate = ConvertDateTime(row["RegistDate"]);                        // 入金登録日
                deposit.CustomerCD = Convert.ToString(row["CustomerCD"]);                       // 入金元CD
                deposit.CustomerName = Convert.ToString(row["CustomerName"]);                   // 入金元名
                deposit.DateTime1 = Convert.ToString(row["DepositDate1"]);                      // 入金区分日時1
                deposit.Name1 = Convert.ToString(row["DepositName1"]);                          // 入金区分名1
                deposit.Amount1 = ConvertDecimal(row["DepositAmount1"]);                        // 入金額1
                deposit.DateTime2 = Convert.ToString(row["DepositDate2"]);                      // 入金区分日時2
                deposit.Name2 = Convert.ToString(row["DepositName2"]);                          // 入金区分名2
                deposit.Amount2 = ConvertDecimal(row["DepositAmount2"]);                        // 入金額2
                deposit.DateTime3 = Convert.ToString(row["DepositDate3"]);                      // 入金区分日時3
                deposit.Name3 = Convert.ToString(row["DepositName3"]);                          // 入金区分名3
                deposit.Amount3 = ConvertDecimal(row["DepositAmount3"]);                        // 入金額3
                deposit.DateTime4 = Convert.ToString(row["DepositDate4"]);                      // 入金区分日時4
                deposit.Name4 = Convert.ToString(row["DepositName4"]);                          // 入金区分名4
                deposit.Amount4 = ConvertDecimal(row["DepositAmount4"]);                        // 入金額4
                deposit.DateTime5 = Convert.ToString(row["DepositDate5"]);                      // 入金区分日時5
                deposit.Name5 = Convert.ToString(row["DepositName5"]);                          // 入金区分名5
                deposit.Amount5 = ConvertDecimal(row["DepositAmount5"]);                        // 入金額5
                deposit.DateTime6 = Convert.ToString(row["DepositDate6"]);                      // 入金区分日時6
                deposit.Name6 = Convert.ToString(row["DepositName6"]);                          // 入金区分名6
                deposit.Amount6 = ConvertDecimal(row["DepositAmount6"]);                        // 入金額6
                deposit.DateTime7 = Convert.ToString(row["DepositDate7"]);                      // 入金区分日時7
                deposit.Name7 = Convert.ToString(row["DepositName7"]);                          // 入金区分名7
                deposit.Amount7 = ConvertDecimal(row["DepositAmount7"]);                        // 入金額7
                deposit.DateTime8 = Convert.ToString(row["DepositDate8"]);                      // 入金区分日時8
                deposit.Name8 = Convert.ToString(row["DepositName8"]);                          // 入金区分名8
                deposit.Amount8 = ConvertDecimal(row["DepositAmount8"]);                        // 入金額8
                deposit.DateTime9 = Convert.ToString(row["DepositDate9"]);                      // 入金区分日時9
                deposit.Name9 = Convert.ToString(row["DepositName9"]);                          // 入金区分名9
                deposit.Amount9 = ConvertDecimal(row["DepositAmount9"]);                        // 入金額9
                deposit.DateTime10 = Convert.ToString(row["DepositDate10"]);                    // 入金区分日時10
                deposit.Name10 = Convert.ToString(row["DepositName10"]);                        // 入金区分名10
                deposit.Amount10 = ConvertDecimal(row["DepositAmount10"]);                      // 入金額10
                dataSet.DepositTable.Rows.Add(deposit);
            }
        }
        #endregion // 入金データセット

        #region 雑出金データセット
        /// <summary>
        /// 雑出金データをデータセットに設定
        /// </summary>
        /// <param name="dataTable">雑出金データテーブル</param>
        /// <param name="dataSet">データセット</param>
        private void CreateMiscPaymentDataSet(DataTable dataTable, TorihikiReceiptDataSet dataSet)
        {
            foreach(DataRow row in dataTable.Rows)
            {
                var miscPayment = dataSet.MiscPaymentTable.NewMiscPaymentTableRow();

                miscPayment.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 雑出金店舗レシート表記
                miscPayment.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 雑出金担当レシート表記
                miscPayment.RegistDate = ConvertDateTime(row["RegistDate"]);                        // 雑出金登録日
                miscPayment.DateTime1 = Convert.ToString(row["MiscPaymentDateTime1"]);              // 雑出金区分日時1
                miscPayment.Name1 = Convert.ToString(row["MiscPaymentName1"]);                      // 雑出金区分名1
                miscPayment.Amount1 = ConvertDecimal(row["MiscPaymentAmount1"]);                    // 雑出金額1
                miscPayment.DateTime2 = Convert.ToString(row["MiscPaymentDateTime2"]);              // 雑出金区分日時2
                miscPayment.Name2 = Convert.ToString(row["MiscPaymentName2"]);                      // 雑出金区分名2
                miscPayment.Amount2 = ConvertDecimal(row["MiscPaymentAmount2"]);                    // 雑出金額2
                miscPayment.DateTime3 = Convert.ToString(row["MiscPaymentDateTime3"]);              // 雑出金区分日時3
                miscPayment.Name3 = Convert.ToString(row["MiscPaymentName3"]);                      // 雑出金区分名3
                miscPayment.Amount3 = ConvertDecimal(row["MiscPaymentAmount3"]);                    // 雑出金額3
                miscPayment.DateTime4 = Convert.ToString(row["MiscPaymentDateTime4"]);              // 雑出金区分日時4
                miscPayment.Name4 = Convert.ToString(row["MiscPaymentName4"]);                      // 雑出金区分名4
                miscPayment.Amount4 = ConvertDecimal(row["MiscPaymentAmount4"]);                    // 雑出金額4
                miscPayment.DateTime5 = Convert.ToString(row["MiscPaymentDateTime5"]);              // 雑出金区分日時5
                miscPayment.Name5 = Convert.ToString(row["MiscPaymentName5"]);                      // 雑出金区分名5
                miscPayment.Amount5 = ConvertDecimal(row["MiscPaymentAmount5"]);                    // 雑出金額5
                miscPayment.DateTime6 = Convert.ToString(row["MiscPaymentDateTime6"]);              // 雑出金区分日時6
                miscPayment.Name6 = Convert.ToString(row["MiscPaymentName6"]);                      // 雑出金区分名6
                miscPayment.Amount6 = ConvertDecimal(row["MiscPaymentAmount6"]);                    // 雑出金額6
                miscPayment.DateTime7 = Convert.ToString(row["MiscPaymentDateTime7"]);              // 雑出金区分日時7
                miscPayment.Name7 = Convert.ToString(row["MiscPaymentName7"]);                      // 雑出金区分名7
                miscPayment.Amount7 = ConvertDecimal(row["MiscPaymentAmount7"]);                    // 雑出金額7
                miscPayment.DateTime8 = Convert.ToString(row["MiscPaymentDateTime8"]);              // 雑出金区分日時8
                miscPayment.Name8 = Convert.ToString(row["MiscPaymentName8"]);                      // 雑出金区分名8
                miscPayment.Amount8 = ConvertDecimal(row["MiscPaymentAmount8"]);                    // 雑出金額8
                miscPayment.DateTime9 = Convert.ToString(row["MiscPaymentDateTime9"]);              // 雑出金区分日時9
                miscPayment.Name9 = Convert.ToString(row["MiscPaymentName9"]);                      // 雑出金区分名9
                miscPayment.Amount9 = ConvertDecimal(row["MiscPaymentAmount9"]);                    // 雑出金額9
                miscPayment.DateTime10 = Convert.ToString(row["MiscPaymentDateTime10"]);            // 雑出金区分日時10
                miscPayment.Name10 = Convert.ToString(row["MiscPaymentName10"]);                    // 雑出金区分名10
                miscPayment.Amount10 = ConvertDecimal(row["MiscPaymentAmount10"]);                  // 雑出金額10
                dataSet.MiscPaymentTable.Rows.Add(miscPayment);
            }
        }
        #endregion // 雑出金データセット

        #region 両替データセット
        /// <summary>
        /// 両替データをデータセットに設定
        /// </summary>
        /// <param name="dataTable">両替データテーブル</param>
        /// <param name="dataSet">データセット</param>
        private void CreateExchangeDataSet(DataTable dataTable, TorihikiReceiptDataSet dataSet)
        {
            foreach(DataRow row in dataTable.Rows)
            {
                var exchange = dataSet.ExchangeTable.NewExchangeTableRow();

                exchange.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);        // 両替店舗レシート表記
                exchange.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);        // 両替担当レシート表記
                exchange.RegistDate = ConvertDateTime(row["RegistDate"]);                       // 両替登録日
                exchange.ExchangeCount = ConvertDecimal(row["ExchangeCount"]);                  // 両替回数
                exchange.DateTime1 = Convert.ToString(row["ExchangeDateTime1"]);                // 両替日時1
                exchange.Name1 = Convert.ToString(row["ExchangeName1"]);                        // 両替名1
                exchange.Amount1 = ConvertDecimal(row["ExchangeAmount1"]);                      // 両替額1
                exchange.Denomination1 = Convert.ToString(row["ExchangeDenomination1"]);        // 両替紙幣1
                exchange.Count1 = ConvertDecimal(row["ExchangeCount1"]);                        // 両替枚数1
                exchange.DateTime2 = Convert.ToString(row["ExchangeDateTime2"]);                // 両替日時2
                exchange.Name2 = Convert.ToString(row["ExchangeName2"]);                        // 両替名2
                exchange.Amount2 = ConvertDecimal(row["ExchangeAmount2"]);                      // 両替額2
                exchange.Denomination2 = Convert.ToString(row["ExchangeDenomination2"]);        // 両替紙幣2
                exchange.Count2 = ConvertDecimal(row["ExchangeCount2"]);                        // 両替枚数2
                exchange.DateTime3 = Convert.ToString(row["ExchangeDateTime3"]);                // 両替日時3
                exchange.Name3 = Convert.ToString(row["ExchangeName3"]);                        // 両替名3
                exchange.Amount3 = ConvertDecimal(row["ExchangeAmount3"]);                      // 両替額3
                exchange.Denomination3 = Convert.ToString(row["ExchangeDenomination3"]);        // 両替紙幣3
                exchange.Count3 = ConvertDecimal(row["ExchangeCount3"]);                        // 両替枚数3
                exchange.DateTime4 = Convert.ToString(row["ExchangeDateTime4"]);                // 両替日時4
                exchange.Name4 = Convert.ToString(row["ExchangeName4"]);                        // 両替名4
                exchange.Amount4 = ConvertDecimal(row["ExchangeAmount4"]);                      // 両替額4
                exchange.Denomination4 = Convert.ToString(row["ExchangeDenomination4"]);        // 両替紙幣4
                exchange.Count4 = ConvertDecimal(row["ExchangeCount4"]);                        // 両替枚数4
                exchange.DateTime5 = Convert.ToString(row["ExchangeDateTime5"]);                // 両替日時5
                exchange.Name5 = Convert.ToString(row["ExchangeName5"]);                        // 両替名5
                exchange.Amount5 = ConvertDecimal(row["ExchangeAmount5"]);                      // 両替額5
                exchange.Denomination5 = Convert.ToString(row["ExchangeDenomination5"]);        // 両替紙幣5
                exchange.Count5 = ConvertDecimal(row["ExchangeCount5"]);                        // 両替枚数5
                exchange.DateTime6 = Convert.ToString(row["ExchangeDateTime6"]);                // 両替日時6
                exchange.Name6 = Convert.ToString(row["ExchangeName6"]);                        // 両替名6
                exchange.Amount6 = ConvertDecimal(row["ExchangeAmount6"]);                      // 両替額6
                exchange.Denomination6 = Convert.ToString(row["ExchangeDenomination6"]);        // 両替紙幣6
                exchange.Count6 = ConvertDecimal(row["ExchangeCount6"]);                        // 両替枚数6
                exchange.DateTime7 = Convert.ToString(row["ExchangeDateTime7"]);                // 両替日時7
                exchange.Name7 = Convert.ToString(row["ExchangeName7"]);                        // 両替名7
                exchange.Amount7 = ConvertDecimal(row["ExchangeAmount7"]);                      // 両替額7
                exchange.Denomination7 = Convert.ToString(row["ExchangeDenomination7"]);        // 両替紙幣7
                exchange.Count7 = ConvertDecimal(row["ExchangeCount7"]);                        // 両替枚数7
                exchange.DateTime8 = Convert.ToString(row["ExchangeDateTime8"]);                // 両替日時8
                exchange.Name8 = Convert.ToString(row["ExchangeName8"]);                        // 両替名8
                exchange.Amount8 = ConvertDecimal(row["ExchangeAmount8"]);                      // 両替額8
                exchange.Denomination8 = Convert.ToString(row["ExchangeDenomination8"]);        // 両替紙幣8
                exchange.Count8 = ConvertDecimal(row["ExchangeCount8"]);                        // 両替枚数8
                exchange.DateTime9 = Convert.ToString(row["ExchangeDateTime9"]);                // 両替日時9
                exchange.Name9 = Convert.ToString(row["ExchangeName9"]);                        // 両替名9
                exchange.Amount9 = ConvertDecimal(row["ExchangeAmount9"]);                      // 両替額9
                exchange.Denomination9 = Convert.ToString(row["ExchangeDenomination9"]);        // 両替紙幣9
                exchange.Count9 = ConvertDecimal(row["ExchangeCount9"]);                        // 両替枚数9
                exchange.DateTime10 = Convert.ToString(row["ExchangeDateTime10"]);              // 両替日時10
                exchange.Name10 = Convert.ToString(row["ExchangeName10"]);                      // 両替名10
                exchange.Amount10 = ConvertDecimal(row["ExchangeAmount10"]);                    // 両替額10
                exchange.Denomination10 = Convert.ToString(row["ExchangeDenomination10"]);      // 両替紙幣10
                exchange.Count10 = ConvertDecimal(row["ExchangeCount10"]);                      // 両替枚数10
                dataSet.ExchangeTable.Rows.Add(exchange);
            }
        }
        #endregion // 両替データセット

        #region 釣銭準備データセット
        /// <summary>
        /// 釣銭準備データをデータセットに設定
        /// </summary>
        /// <param name="dataTable">釣銭準備データテーブル</param>
        /// <param name="dataSet">データセット</param>
        private void CreateChangePreparationDataSet(DataTable dataTable, TorihikiReceiptDataSet dataSet)
        {
            foreach(DataRow row in dataTable.Rows)
            {
                var changePreparation = dataSet.ChangePreparationTable.NewChangePreparationTableRow();

                changePreparation.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 釣銭準備店舗レシート表記
                changePreparation.StaffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 釣銭準備担当レシート表記
                changePreparation.RegistDate = ConvertDateTime(row["RegistDate"]);                        // 釣銭準備登録日
                changePreparation.DateTime1 = Convert.ToString(row["ChangePreparationDate1"]);            // 釣銭準備名1
                changePreparation.Name1 = Convert.ToString(row["ChangePreparationName1"]);                // 釣銭準備名1
                changePreparation.Amount1 = ConvertDecimal(row["ChangePreparationAmount1"]);              // 釣銭準備額1
                changePreparation.DateTime2 = Convert.ToString(row["ChangePreparationDate2"]);            // 釣銭準備名2
                changePreparation.Name2 = Convert.ToString(row["ChangePreparationName2"]);                // 釣銭準備名2
                changePreparation.Amount2 = ConvertDecimal(row["ChangePreparationAmount2"]);              // 釣銭準備額2
                changePreparation.DateTime3 = Convert.ToString(row["ChangePreparationDate3"]);            // 釣銭準備名3
                changePreparation.Name3 = Convert.ToString(row["ChangePreparationName3"]);                // 釣銭準備名3
                changePreparation.Amount3 = ConvertDecimal(row["ChangePreparationAmount3"]);              // 釣銭準備額3
                changePreparation.DateTime4 = Convert.ToString(row["ChangePreparationDate4"]);            // 釣銭準備名4
                changePreparation.Name4 = Convert.ToString(row["ChangePreparationName4"]);                // 釣銭準備名4
                changePreparation.Amount4 = ConvertDecimal(row["ChangePreparationAmount4"]);              // 釣銭準備額4
                changePreparation.DateTime5 = Convert.ToString(row["ChangePreparationDate5"]);            // 釣銭準備名5
                changePreparation.Name5 = Convert.ToString(row["ChangePreparationName5"]);                // 釣銭準備名5
                changePreparation.Amount5 = ConvertDecimal(row["ChangePreparationAmount5"]);              // 釣銭準備額5
                changePreparation.DateTime6 = Convert.ToString(row["ChangePreparationDate6"]);            // 釣銭準備名6
                changePreparation.Name6 = Convert.ToString(row["ChangePreparationName6"]);                // 釣銭準備名6
                changePreparation.Amount6 = ConvertDecimal(row["ChangePreparationAmount6"]);              // 釣銭準備額6
                changePreparation.DateTime7 = Convert.ToString(row["ChangePreparationDate7"]);            // 釣銭準備名7
                changePreparation.Name7 = Convert.ToString(row["ChangePreparationName7"]);                // 釣銭準備名7
                changePreparation.Amount7 = ConvertDecimal(row["ChangePreparationAmount7"]);              // 釣銭準備額7
                changePreparation.DateTime8 = Convert.ToString(row["ChangePreparationDate8"]);            // 釣銭準備名8
                changePreparation.Name8 = Convert.ToString(row["ChangePreparationName8"]);                // 釣銭準備名8
                changePreparation.Amount8 = ConvertDecimal(row["ChangePreparationAmount8"]);              // 釣銭準備額8
                changePreparation.DateTime9 = Convert.ToString(row["ChangePreparationDate9"]);            // 釣銭準備名9
                changePreparation.Name9 = Convert.ToString(row["ChangePreparationName9"]);                // 釣銭準備名9
                changePreparation.Amount9 = ConvertDecimal(row["ChangePreparationAmount9"]);              // 釣銭準備額9
                changePreparation.DateTime10 = Convert.ToString(row["ChangePreparationDate10"]);          // 釣銭準備名10
                changePreparation.Name10 = Convert.ToString(row["ChangePreparationName10"]);              // 釣銭準備名10
                changePreparation.Amount10 = ConvertDecimal(row["ChangePreparationAmount10"]);            // 釣銭準備額10
                dataSet.ChangePreparationTable.Rows.Add(changePreparation);
            }
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
