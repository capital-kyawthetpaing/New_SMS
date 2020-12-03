using Base.Client;
using BL;
using DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using TempoRegiTorihikiReceipt.Reports;
using Microsoft.PointOfService;

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
         //   Start_Display();
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
            //InPcID = "PTK";
            StartProgram();

            // フォームを表示させないように最小化してタスクバーにも表示しない
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            //コマンドライン引数を配列で取得する
            string[] cmds = Environment.GetCommandLineArgs();// 
                                                             //string[] cmds = new string[] { "C:\\", "01", "0001", "MYA040_PC", "5", "2368" };// 
                                                             //  MessageBox.Show(cmds.Length.ToString());
            if (cmds.Length - 1 > (int)ECmdLine.PcID)
            {
                //   MessageBox.Show("Hi");

                try
                {
                    InputMode = cmds[(int)CommandLine.Mode ];
                    InputDepositNO = cmds[(int)CommandLine.DepositeNO];
                    // MessageBox.Show(cmds.Length.ToString());
                    // 印刷
                   // MessageBox.Show(InputMode + " " + InputDepositNO);
                    Print();
                }
                catch (Exception ex)
                {
                 //   MessageBox.Show(ex.StackTrace.ToString());
                }
            }
            else
            {
                
            }

            EndSec();
        }

        /// <summary>
        /// 画面終了処理
        /// </summary>
        protected override void EndSec()
        {
            //RunDisplay_Service();
            this.Close();
        }
        private PosPrinter GetReceiptPrinter()
        {
            PosExplorer posExplorer = new PosExplorer();
            
            PosPrinter ppt = null;

            DeviceInfo receiptPrinterDevice = null;
            try
            {
               
                 receiptPrinterDevice = posExplorer.GetDevice(DeviceType.PosPrinter, "PosPrinter"); //May need to change this if you don't use a logicial name or use a different one.
                //MessageBox.Show( );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to get device information." + Environment.NewLine + ex.StackTrace.ToString(), "PrinterSample_Step16");
                // No device available.

            }
            try
            {
                ppt = (PosPrinter)posExplorer.CreateInstance(receiptPrinterDevice);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to instantiate device." + Environment.NewLine + ex.StackTrace.ToString(), "PrinterSample_Step16");
                // No device available.
            }
            return ppt;
        }
        private void ConnectToPrinter(PosPrinter printer)
        {
            try
            {
                printer.Open();
                printer.Claim(1000);
                printer.DeviceEnabled = true;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace.ToString());
            }
        }
         PosPrinter printer = null;
        private void PrintReceipt()
        {
            try
            {
                printer = GetReceiptPrinter();
                ConnectToPrinter(printer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace.ToString());
            }
        }
       
        private void DisconnectFromPrinter(PosPrinter printer)
        {
           
            printer.Release();
            printer.Close();
        }
        /// <summary>
        /// 印刷実行
        /// </summary>
        private void Print()
        {
            try
            {
              //  PrintReceipt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace.ToString());
            }

            var torihikiReceiptDataSet = new TorihikiReceiptDataSet();
            Object ReadyToPrinter=null;
            switch(InputMode)
            {
                case MODE_DEPOSIT:
                    // 雑入金取得
                    try
                    {
                        string y = "";
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
                            ReadyToPrinter = reportModeDepositMiscDeposit;
                            reportModeDepositMiscDeposit.PrintToPrinter(0, false, 0, 0);
                            y = "1 " + reportModeDepositMiscDeposit.PrintOptions.PrinterName;
                            //   MessageBox.Show("First Con" + Environment.NewLine + torihikiReceiptDataSet.MiscDepositTable.Rows.Count.ToString());
                        }
                        else if (torihikiReceiptDataSet.MiscDepositTable.Rows.Count == 0 && torihikiReceiptDataSet.DepositTable.Rows.Count > 0)
                        {
                            // 入金のみデータあり
                            var reportModeDepositDeposit = new TempoRegiTorihikiReceipt_Deposit();

                            reportModeDepositDeposit.SetDataSource(torihikiReceiptDataSet);
                            reportModeDepositDeposit.Refresh();

                            reportModeDepositDeposit.PrintOptions.PrinterName = StorePrinterName;
                            ReadyToPrinter = reportModeDepositDeposit;

                            reportModeDepositDeposit.PrintToPrinter(0, false, 0, 0);
                            y = "2 " + reportModeDepositDeposit.PrintOptions.PrinterName;
                            //   MessageBox.Show("Second Con");
                        }
                        else if (torihikiReceiptDataSet.MiscDepositTable.Rows.Count > 0 && torihikiReceiptDataSet.DepositTable.Rows.Count > 0)
                        {
                            // 雑入金・入金データあり
                            var reportModeDeposit = new TempoRegiTorihikiReceipt_Mode2();
                            reportModeDeposit.SetDataSource(torihikiReceiptDataSet);
                            reportModeDeposit.Refresh();
                            reportModeDeposit.PrintOptions.PrinterName = StorePrinterName;
                            ReadyToPrinter = reportModeDeposit;
                            reportModeDeposit.PrintToPrinter(0, false, 0, 0);
                            y = "3 " + reportModeDeposit.PrintOptions.PrinterName;
                            //     MessageBox.Show("Third Con");
                            

                        }
                        else
                        {
                            bl.ShowMessage("E128");
                        }

                       // DisconnectFromPrinter(printer);
                       // MessageBox.Show("Disconnected Printer " + Environment.NewLine +" Step went on named " + y.ToString());
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
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
                        ReadyToPrinter = reportModePayment;
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
                        ReadyToPrinter = reportModeExchange;
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
                        ReadyToPrinter = reportModeChangePreparation;
                        reportModeChangePreparation.PrintToPrinter(0, false, 0, 0);
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;
            }
            var RTP = (CrystalDecisions.CrystalReports.Engine.ReportClass)ReadyToPrinter;
            //MessageBox.Show(torihikiReceiptDataSet.MiscDepositTable)
            //if (ReadyToPrinter != null)
            //{
            //    //try
            //    //{
            //    //    cdo.RemoveDisplay(true);
            //    //}
            //    //catch { }
            //    var RTP = (CrystalDecisions.CrystalReports.Engine.ReportClass)ReadyToPrinter;
            //     RTP.PrintToPrinter(0, false, 0, 0);
            //   // Stop_DisplayService();
            //}

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
                miscDeposit.DateTime = Convert.ToString(row["MiscDepositDate"]);                // 雑入金日時
                miscDeposit.Name = Convert.ToString(row["MiscDepositName"]);                    // 雑入金名
                miscDeposit.Remark = Convert.ToString(row["MiscDepositRemark"]);                // 雑入金備考

                // 雑入金額
                var amount = ConvertDecimal(row["MiscDepositAmount"]);
                miscDeposit.Amount = string.IsNullOrWhiteSpace(amount) ? "0" : amount;

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
                deposit.DateTime = Convert.ToString(row["DepositDate"]);                        // 入金区分日時
                deposit.Name = Convert.ToString(row["DepositName"]);                            // 入金区分名
                deposit.Remark = Convert.ToString(row["DepositRemark"]);                        // 入金備考

                // 入金額
                var amount = ConvertDecimal(row["DepositAmount"]);
                deposit.Amount = string.IsNullOrWhiteSpace(amount) ? "0" : amount;

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
                miscPayment.DateTime = Convert.ToString(row["MiscPaymentDateTime"]);                // 雑出金区分日時
                miscPayment.Name = Convert.ToString(row["MiscPaymentName"]);                        // 雑出金区分名
                miscPayment.Remark = Convert.ToString(row["MiscPaymentRemark"]);                    // 雑出金備考

                // 雑出金額
                var amount = ConvertDecimal(row["MiscPaymentAmount"]);
                miscPayment.Amount = string.IsNullOrWhiteSpace(amount) ? "0" : amount;

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
                exchange.DateTime = Convert.ToString(row["ExchangeDateTime"]);                  // 両替日時
                exchange.Name = Convert.ToString(row["ExchangeName"]);                          // 両替名
                exchange.Denomination = Convert.ToString(row["ExchangeDenomination"]);          // 両替紙幣
                exchange.Remark = Convert.ToString(row["ExchangeRemark"]);                      // 両替備考

                // 両替額
                var value = ConvertDecimal(row["ExchangeAmount"]);
                exchange.Amount = string.IsNullOrWhiteSpace(value) ? "0" : value;

                // 両替枚数
                value = ConvertDecimal(row["ExchangeCount"]);
                exchange.Count = string.IsNullOrWhiteSpace(value) ? "0" : value;

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
                changePreparation.DateTime = Convert.ToString(row["ChangePreparationDate"]);              // 釣銭準備日時
                changePreparation.Name = Convert.ToString(row["ChangePreparationName"]);                  // 釣銭準備名
                changePreparation.Remark = Convert.ToString(row["ChangePreparationRemark"]);              // 釣銭準備備考

                // 釣銭準備額
                var amount = ConvertDecimal(row["ChangePreparationAmount"]);
                changePreparation.Amount = string.IsNullOrWhiteSpace(amount) ? "0" : amount;

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
        //protected void Kill(string pth)
        //{
        //    try
        //    {
        //        Process[] processCollection = Process.GetProcessesByName(pth.Replace(".exe", ""));
        //        foreach (Process p in processCollection)
        //        {
        //            p.Kill();
        //        }

        //        Process[] processCollections = Process.GetProcessesByName(pth + ".exe");
        //        foreach (Process p in processCollections)
        //        {
        //            p.Kill();
        //        }
        //    }
        //    catch { }
        //}
        //private void Stop_DisplayService(bool isForced = true)
        //{
        //    if (Base_DL.iniEntity.IsDM_D30Used)
        //    {

        //        Login_BL bbl_1 = new Login_BL();
        //        if (bbl_1.ReadConfig())
        //        {
        //            bbl_1.Display_Service_Update(false);
        //            Thread.Sleep(3 * 1000);
        //            bbl_1.Display_Service_Enabled(false);
        //        }
        //        else
        //        {
        //            bbl_1.Display_Service_Update(false);
        //            Thread.Sleep(3 * 1000);
        //            bbl_1.Display_Service_Enabled(false);
        //        }
        //        try
        //        {
        //            Kill("Display_Service");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.StackTrace.ToString());
        //        }
        //        if (isForced) cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
        //        //Base_DL.iniEntity.CDO_DISPLAY.SetDisplay(true, true,Base_DL.iniEntity.DefaultMessage);
        //    }
        //}
        //private void RunDisplay_Service()  // Make when we want to run display_service
        //{
        //    try
        //    {
        //        if (Base_DL.iniEntity.IsDM_D30Used)
        //        {
        //            cdo.RemoveDisplay(true);
        //            Login_BL bbl_1 = new Login_BL();
        //            bbl_1.Display_Service_Update(true);
        //            bbl_1.Display_Service_Enabled(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error in removing display. . .");
        //    }
        //}
        //private void Start_Display()
        //{
        //    try
        //    {
        //        if (Base_DL.iniEntity.IsDM_D30Used)
        //        {
        //            Login_BL bbl_1 = new Login_BL();
        //            if (bbl_1.ReadConfig())
        //            {
        //                bbl_1.Display_Service_Update(false);
        //                Thread.Sleep(3 * 1000);
        //                bbl_1.Display_Service_Enabled(false);
        //            }
        //            else
        //            {
        //                bbl_1.Display_Service_Update(false);
        //                Thread.Sleep(3 * 1000);
        //                bbl_1.Display_Service_Enabled(false);
        //            }
        //            Kill("Display_Service");
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show("Cant remove on second time" + ex.StackTrace); }
        //}
        //EPSON_TM30.CashDrawerOpen cdo = new EPSON_TM30.CashDrawerOpen();
    }
}
