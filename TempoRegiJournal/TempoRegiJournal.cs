using Base.Client;
using BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TempoRegiJournal
{
    public partial class TempoRegiJournal : ShopBaseForm
    {
        /// <summary>
        /// ジャーナル詳細データ種別
        /// </summary>
        private enum JournalDataKind
        {
            /// <summary>
            /// 雑入金
            /// </summary>
            Sales = 0,

            /// <summary>
            /// 雑入金
            /// </summary>
            MiscDeposit,

            /// <summary>
            /// 入金
            /// </summary>
            Deposit,

            /// <summary>
            /// 雑出金
            /// </summary>
            MiscPayment,

            /// <summary>
            /// 両替
            /// </summary>
            Exchange,

            /// <summary>
            /// 釣銭準備
            /// </summary>
            ChangePreparation,
        }

        /// <summary>
        /// 製品名上段文字数
        /// </summary>
        private const int SKU_SHORTNAME_LENGTH = 23*2;

        /// <summary>
        /// 最小の日付
        /// </summary>
        private const string DATE_MIN = "1900/01/01";
        private const string DATETIME_MIN = "1900/01/01 00:00:00";

        /// <summary>
        /// BL
        /// </summary>
        TempoRegiJournal_BL bl = new TempoRegiJournal_BL();

        /// <summary>
        /// 店舗ジャーナル印刷 コンストラクタ
        /// </summary>
        public TempoRegiJournal()
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
            InProgramID = "TempoRegiJournal";
            string data = InOperatorCD;

            StartProgram();

            this.Text = "店舗ジャーナル印刷";
            this.btnProcess.Text = "プレビュー";

            SetRequireField();
        }

        /// <summary>
        /// オブジェクトの設定
        /// </summary>
        private void SetRequireField()
        {
            txtPrintDateFrom.Require(true);
            txtPrintDateTo.Require(true);

            txtPrintDateFrom.Text = txtPrintDateTo.Text = DateTime.Today.ToShortDateString();
            txtPrintDateFrom.Focus();
        }

        private void DisplayData()
        {
            txtPrintDateFrom.Focus();
            string data = InOperatorCD;
        }

        /// <summary>
        /// エラーチェック
        /// </summary>
        /// <returns>true=エラーなし、false=エラーあり</returns>
        /// <remarks>領収書印字日付はコントロールにチェック処理あり</remarks>
        public bool ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(txtPrintDateFrom.Text))
            {
                bl.ShowMessage("E102");
                txtPrintDateFrom.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtPrintDateTo.Text))
            {
                bl.ShowMessage("E102");
                txtPrintDateTo.Focus();
                return false;
            }
            else if (!bbl.CheckDate(txtPrintDateFrom.Text))
            {
                // 日付エラー
                bbl.ShowMessage("E103");
                txtPrintDateFrom.Focus();
                return false;
            }
            else if (!bbl.CheckDate(txtPrintDateTo.Text))
            {
                // 日付エラー
                bbl.ShowMessage("E103");
                txtPrintDateTo.Focus();
                return false;
            }
            else if (Convert.ToDateTime(txtPrintDateFrom.Text).CompareTo(Convert.ToDateTime(txtPrintDateTo.Text)) > 0)
            {
                bl.ShowMessage("E130");
                txtPrintDateFrom.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 画面終了処理
        /// </summary>
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
                    // プレビュー実行
                    Preview();
                    break;
            }
        }

        /// <summary>
        /// プレビュー実行
        /// </summary>
        private void Preview()
        {
            if (ErrorCheck())
            {
                var dataNum = bl.D_CheckStoreCalculation(StoreCD, txtPrintDateFrom.Text, txtPrintDateTo.Text);

                var value = ConvertDecimal(dataNum.Rows[0]["DataNum"]);
                if (string.IsNullOrWhiteSpace(value) || Convert.ToInt32(value) == 0)
                {
                    bl.ShowMessage("E244");
                }
                else
                {
                    var journal = bl.D_JournalSelect(StoreCD, txtPrintDateFrom.Text, txtPrintDateTo.Text);
                    if (journal.Rows.Count > 0)
                    {
                        OutputJournal(journal);
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                }
            }
        }

        /// <summary>
        /// 商品引換券出力
        /// </summary>
        /// <param name="data">データ</param>
        private void OutputJournal(DataTable data)
        {
            var storeDataSet = CreateStoreDataSet(data, PrintCheckBox.Checked);
            if (storeDataSet.StoreTable.Count == 0)
            {
                bl.ShowMessage("E128");
            }
            else
            {
                // プレビューを開く
                var preview = new TempoRegiJournalPreview
                {
                    Store = storeDataSet,
                    StorePrinterName = StorePrinterName
                };

                preview.SetJournalDataSet();
                preview.ShowDialog();
            }
        }

        #region データセット

        /// <summary>
        /// データセットを作成
        /// </summary>
        /// <param name="data">データベースから取得したデータテーブル</param>
        /// <param name="isPrint">販売明細を印刷するかどうか(印刷する=True、印刷しない=False)</param>
        /// <returns>データセット</returns>
        private StoreDataSet CreateStoreDataSet(DataTable data, bool isPrint)
        {
            var stores = new StoreDataSet.StoreTableDataTable();
            var salesNos = new StoreDataSet.SalesNoTableDataTable();
            var sales = new StoreDataSet.SalesTableDataTable();

            var storeDataSet = new StoreDataSet();

            string OldDepositNo = "";

            for (var index = 0; index < data.Rows.Count; index++)
            {
                var row = data.Rows[index];

                // 共通データ
                var salesNO = Convert.ToString(row["SalesNO"]);                             // 売上番号
                var storeReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 店舗レシート表記
                var staffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 担当レシート表記
                var issueDate = ConvertDateTime(row["IssueDate"], true);                    // 発行日
                var issueDateTime = ConvertDateTime(row["IssueDate"], false);               // 発行日時

                #region 店舗データ
                if (index == 0)
                {
                    var store = CreateStoreTableRow(stores);
                    store.Logo = (byte[])row["Logo"];
                    store.StoreName = Convert.ToString(row["StoreName"]);                       // 店舗名
                    store.Address1 = Convert.ToString(row["Address1"]);                         // 住所1
                    store.Address2 = Convert.ToString(row["Address2"]);                         // 住所2
                    store.TelphoneNO = Convert.ToString(row["TelephoneNO"]);                    // 電話番号
                    store.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);       // 店舗レシート表記
                                                                                                //
                    stores.Rows.Add(store);
                }
                #endregion // 店舗データ

                #region 売上番号データ
                if (!string.IsNullOrWhiteSpace(salesNO))
                {
                    var salesNo = CreateSalesNoTableRow(salesNos);
                    salesNo.StoreReceiptPrint = storeReceiptPrint;                                    // 店舗レシート表記
                    salesNo.StaffReceiptPrint = staffReceiptPrint;                                    // 担当レシート表記
                    salesNo.SalesNO = salesNO;                                                        // 売上番号
                    salesNo.IssueDate = issueDate;  // ConvertDateTime(row["IssueDate"], true);                      // 発行日
                    salesNo.IssueDateTime = issueDateTime;  //  ConvertDateTime(row["IssueDate"], false);                 // 発行日時

                    if (salesNos.AsEnumerable().Where(s => s.SalesNO == salesNO).FirstOrDefault() == null)
                    {
                        salesNos.Rows.Add(salesNo);
                    }
                }
                #endregion  // 売上番号データ

                #region 販売データ
                if (OldDepositNo != row["DepositNO"].ToString() && !string.IsNullOrWhiteSpace(salesNO))
                {
                    OldDepositNo = row["DepositNO"].ToString();

                    var sale = CreateSalesTableRow(sales, JournalDataKind.Sales);
                    sale.StoreReceiptPrint = storeReceiptPrint;                                    // 店舗レシート表記
                    sale.StaffReceiptPrint = staffReceiptPrint;                                    // 担当レシート表記
                    sale.SalesNO = salesNO;                                                        // 売上番号
                    sale.IssueDate = issueDate;   // ConvertDateTime(row["IssueDate"], true);                      // 発行日
                    sale.IssueDateTime = issueDateTime;  // ConvertDateTime(row["IssueDate"], false);                 // 発行日時
                    sale.DispDateTime = "";
                    sale.JanCD = Convert.ToString(row["JanCD"]);                                   // JANCD

                    var kakaku = ConvertDecimal(row["Kakaku"]);                                     // 価格

                    //数量が0の場合は1として処理、その場合、単価は価格を割り当てる
                    var salesSU = ConvertDecimal(row["SalesSU"]);
                    if (string.IsNullOrWhiteSpace(salesSU))
                    {
                        sale.SalesSU = "1";                                                        // 数量
                        sale.SalesUnitPrice = kakaku;                                              // 単価
                    }
                    else
                    {
                        sale.SalesSU = salesSU;                                                    // 数量
                        sale.SalesUnitPrice = ConvertDecimal(row["SalesUnitPrice"]);               // 単価
                    }

                    sale.SalesGaku = kakaku;                                                       // 価格
                    sale.SalesTax = ConvertDecimal(row["SalesTax"]);                               // 売上消費税額
                    sale.SalesTaxRate = ConvertDecimal(row["SalesTaxRate"]) == "8" ? "*" : "";     // 税率
                    sale.TotalGaku = ConvertDecimal(row["TotalGaku"]);                             // 販売合計額

                    // 商品名
                    var encoding = System.Text.Encoding.GetEncoding("Shift_JIS");

                    var skuShortName = Convert.ToString(row["SKUShortName"]);
                    byte[] skuShortNameBT = encoding.GetBytes(skuShortName);
                    if (skuShortNameBT.Length < SKU_SHORTNAME_LENGTH)
                    {
                        sale.SKUShortName1 = skuShortName;
                        sale.SKUShortName2 = "";
                    }
                    else
                    {
                        sale.SKUShortName1 = encoding.GetString(skuShortNameBT, 0, SKU_SHORTNAME_LENGTH);
                        sale.SKUShortName2 = encoding.GetString(skuShortNameBT, SKU_SHORTNAME_LENGTH, skuShortNameBT.Length - SKU_SHORTNAME_LENGTH);
                    }

                    #region 合計データ
                    sale.SumSalesSU = ConvertDecimal(row["SumSalesSU"]);                           // 小計数量
                    sale.Subtotal = ConvertDecimal(row["Subtotal"]);                               // 小計金額
                    sale.TargetAmount8 = ConvertDecimal(row["TargetAmount8"]);                     // 消費税対象額8%
                    sale.ConsumptionTax8 = ConvertDecimal(row["ConsumptionTax8"]);                 // 内消費税等8%
                    sale.TargetAmount10 = ConvertDecimal(row["TargetAmount10"]);                   // 消費税対象額10%
                    sale.ConsumptionTax10 = ConvertDecimal(row["ConsumptionTax10"]);               // 内消費税等10%
                    sale.Total = ConvertDecimal(row["Total"]);                                     // 合計
                    #endregion // 合計データ

                    #region 支払方法
                    sale.PaymentName1 = Convert.ToString(row["PaymentName1"]);                     // 支払方法名1
                    sale.PaymentAmount1 = ConvertDecimal(row["AmountPay1"]);                       // 支払方法額1
                    sale.PaymentName2 = Convert.ToString(row["PaymentName2"]);                     // 支払方法名2
                    sale.PaymentAmount2 = ConvertDecimal(row["AmountPay2"]);                       // 支払方法額2
                    sale.PaymentName3 = Convert.ToString(row["PaymentName3"]);                     // 支払方法名3
                    sale.PaymentAmount3 = ConvertDecimal(row["AmountPay3"]);                       // 支払方法額3
                    sale.PaymentName4 = Convert.ToString(row["PaymentName4"]);                     // 支払方法名4
                    sale.PaymentAmount4 = ConvertDecimal(row["AmountPay4"]);                       // 支払方法額4
                    sale.PaymentName5 = Convert.ToString(row["PaymentName5"]);                     // 支払方法名5
                    sale.PaymentAmount5 = ConvertDecimal(row["AmountPay5"]);                       // 支払方法額5
                    sale.PaymentName6 = Convert.ToString(row["PaymentName6"]);                     // 支払方法名6
                    sale.PaymentAmount6 = ConvertDecimal(row["AmountPay6"]);                       // 支払方法額6
                    sale.PaymentName7 = Convert.ToString(row["PaymentName7"]);                     // 支払方法名7
                    sale.PaymentAmount7 = ConvertDecimal(row["AmountPay7"]);                       // 支払方法額7
                    sale.PaymentName8 = Convert.ToString(row["PaymentName8"]);                     // 支払方法名8
                    sale.PaymentAmount8 = ConvertDecimal(row["AmountPay8"]);                       // 支払方法額8
                    sale.PaymentName9 = Convert.ToString(row["PaymentName9"]);                     // 支払方法名9
                    sale.PaymentAmount9 = ConvertDecimal(row["AmountPay9"]);                       // 支払方法額9
                    sale.PaymentName10 = Convert.ToString(row["PaymentName10"]);                   // 支払方法名10
                    sale.PaymentAmount10 = ConvertDecimal(row["AmountPay10"]);                     // 支払方法額10
                    #endregion // 支払方法

                    #region お釣りデータ
                    sale.Refund = ConvertDecimal(row["Refund"]);                                   // 釣銭
                    sale.DiscountGaku = ConvertDecimal(row["DiscountGaku"]);                       // 値引額
                    #endregion // お釣りデータ

                    // 同一売上番号内で表示行を設定
                    sale.Row = sales.Where(v => v.IssueDateTime == sale.IssueDateTime && v.SalesNO == sale.SalesNO).Count() + 1;

                    sales.Rows.Add(sale);

                }
                #endregion // 販売データ

                #region 雑入金データ
                var calendarDate = Convert.ToString(row["CalendarDate"]);       // カレンダー日
                var remark = Convert.ToString(row["MiscDepositRemark"]);        // 雑入金備考

                SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate1"], row["MiscDepositName1"], row["MiscDepositAmount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate1"], row["MiscDepositName1"], row["MiscDepositAmount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate2"], row["MiscDepositName2"], row["MiscDepositAmount2"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate3"], row["MiscDepositName3"], row["MiscDepositAmount3"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate4"], row["MiscDepositName4"], row["MiscDepositAmount4"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate5"], row["MiscDepositName5"], row["MiscDepositAmount5"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate6"], row["MiscDepositName6"], row["MiscDepositAmount6"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate7"], row["MiscDepositName7"], row["MiscDepositAmount7"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate8"], row["MiscDepositName8"], row["MiscDepositAmount8"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate9"], row["MiscDepositName9"], row["MiscDepositAmount9"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscDepositTable(salesNO, sales, salesNos, calendarDate, row["MiscDepositDate10"], row["MiscDepositName10"], row["MiscDepositAmount10"], remark, storeReceiptPrint, staffReceiptPrint);
                #endregion // 雑入金データ

                #region 入金データ
                var customerCD = Convert.ToString(row["CustomerCD"]);           // 入金元CD
                var customerName = Convert.ToString(row["CustomerName"]);       // 入金元名
                remark = Convert.ToString(row["DepositRemark"]);                // 入金備考

                SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate1"], customerCD, customerName, row["DepositName1"], row["DepositAmount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate1"], customerCD, customerName, row["DepositName1"], row["DepositAmount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate2"], customerCD, customerName, row["DepositName2"], row["DepositAmount2"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate3"], customerCD, customerName, row["DepositName3"], row["DepositAmount3"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate4"], customerCD, customerName, row["DepositName4"], row["DepositAmount4"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate5"], customerCD, customerName, row["DepositName5"], row["DepositAmount5"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate6"], customerCD, customerName, row["DepositName6"], row["DepositAmount6"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate7"], customerCD, customerName, row["DepositName7"], row["DepositAmount7"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate8"], customerCD, customerName, row["DepositName8"], row["DepositAmount8"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate9"], customerCD, customerName, row["DepositName9"], row["DepositAmount9"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetDepositTable(salesNO, sales, salesNos, calendarDate, row["DepositDate10"], customerCD, customerName, row["DepositName10"], row["DepositAmount10"], remark, storeReceiptPrint, staffReceiptPrint);
                #endregion // 入金データ

                #region 雑出金データ
                remark = Convert.ToString(row["MiscPaymentRemark"]);        // 雑出金備考

                SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate1"], row["MiscPaymentName1"], row["MiscPaymentAmount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate1"], row["MiscPaymentName1"], row["MiscPaymentAmount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate2"], row["MiscPaymentName2"], row["MiscPaymentAmount2"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate3"], row["MiscPaymentName3"], row["MiscPaymentAmount3"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate4"], row["MiscPaymentName4"], row["MiscPaymentAmount4"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate5"], row["MiscPaymentName5"], row["MiscPaymentAmount5"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate6"], row["MiscPaymentName6"], row["MiscPaymentAmount6"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate7"], row["MiscPaymentName7"], row["MiscPaymentAmount7"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate8"], row["MiscPaymentName8"], row["MiscPaymentAmount8"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate9"], row["MiscPaymentName9"], row["MiscPaymentAmount9"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetMiscPaymentTable(salesNO, sales, salesNos, calendarDate, row["MiscPaymentDate10"], row["MiscPaymentName10"], row["MiscPaymentAmount10"], remark, storeReceiptPrint, staffReceiptPrint);
                #endregion // 雑出金データ

                #region 両替データ
                remark = Convert.ToString(row["ExchangeRemark"]);       // 両替備考

                SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate1"], row["ExchangeName1"], row["ExchangeAmount1"], row["ExchangeDenomination1"], row["ExchangeCount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate1"], row["ExchangeName1"], row["ExchangeAmount1"], row["ExchangeDenomination1"], row["ExchangeCount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate2"], row["ExchangeName2"], row["ExchangeAmount2"], row["ExchangeDenomination2"], row["ExchangeCount2"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate3"], row["ExchangeName3"], row["ExchangeAmount3"], row["ExchangeDenomination3"], row["ExchangeCount3"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate4"], row["ExchangeName4"], row["ExchangeAmount4"], row["ExchangeDenomination4"], row["ExchangeCount4"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate5"], row["ExchangeName5"], row["ExchangeAmount5"], row["ExchangeDenomination5"], row["ExchangeCount5"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate6"], row["ExchangeName6"], row["ExchangeAmount6"], row["ExchangeDenomination6"], row["ExchangeCount6"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate7"], row["ExchangeName7"], row["ExchangeAmount7"], row["ExchangeDenomination7"], row["ExchangeCount7"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate8"], row["ExchangeName8"], row["ExchangeAmount8"], row["ExchangeDenomination8"], row["ExchangeCount8"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate9"], row["ExchangeName9"], row["ExchangeAmount9"], row["ExchangeDenomination9"], row["ExchangeCount9"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetExchangTable(salesNO, sales, salesNos, calendarDate, row["ExchangeDate10"], row["ExchangeName10"], row["ExchangeAmount10"], row["ExchangeDenomination10"], row["ExchangeCount10"], remark, storeReceiptPrint, staffReceiptPrint);
                #endregion // 両替データ

                #region 釣銭準備
                remark = Convert.ToString(row["ChangePreparationRemark"]);      // 釣銭準備備考

                SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate1"], "現金", row["ChangePreparationAmount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate1"], "現金", row["ChangePreparationAmount1"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate2"], row["ChangePreparationName2"], row["ChangePreparationAmount2"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate3"], row["ChangePreparationName3"], row["ChangePreparationAmount3"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate4"], row["ChangePreparationName4"], row["ChangePreparationAmount4"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate5"], row["ChangePreparationName5"], row["ChangePreparationAmount5"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate6"], row["ChangePreparationName6"], row["ChangePreparationAmount6"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate7"], row["ChangePreparationName7"], row["ChangePreparationAmount7"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate8"], row["ChangePreparationName8"], row["ChangePreparationAmount8"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate9"], row["ChangePreparationName9"], row["ChangePreparationAmount9"], remark, storeReceiptPrint, staffReceiptPrint);
                //SetChangePreparationTable(salesNO, sales, salesNos, calendarDate, row["ChangePreparationDate10"], row["ChangePreparationName10"], row["ChangePreparationAmount10"], remark, storeReceiptPrint, staffReceiptPrint);
                #endregion // 釣銭準備

                #region 精算処理 現金残高
                var registDate = ConvertDateTime(row["CashBalanceRegistDate"], true);
                registDate = string.IsNullOrWhiteSpace(registDate) ? issueDate : registDate;
                if (storeDataSet.CashBalanceTable.Where(v => v.RegistDate == registDate).FirstOrDefault() == null)
                {
                    var cashBalance = storeDataSet.CashBalanceTable.NewCashBalanceTableRow();
                    cashBalance.StoreReceiptPrint = storeReceiptPrint;                                      // 店舗レシート表記
                    cashBalance.StaffReceiptPrint = staffReceiptPrint;                                      // 担当レシート表記
                    cashBalance.SalesNO = salesNO;                                                          // 売上番号
                    cashBalance.RegistDate = registDate;    // ConvertDateTime(row["CashBalanceRegistDate"], true);           // 登録日
                    
                    #region 現金残高
                    cashBalance.Num10000yen = ConvertValue(ConvertDecimal(row["10000yenNum"]));             // 現金残高10,000枚数
                    cashBalance.Num5000yen = ConvertValue(ConvertDecimal(row["5000yenNum"]));               // 現金残高5,000枚数
                    cashBalance.Num2000yen = ConvertValue(ConvertDecimal(row["2000yenNum"]));               // 現金残高2,000枚数
                    cashBalance.Num1000yen = ConvertValue(ConvertDecimal(row["1000yenNum"]));               // 現金残高1,000枚数
                    cashBalance.Num500yen = ConvertValue(ConvertDecimal(row["500yenNum"]));                 // 現金残高500枚数
                    cashBalance.Num100yen = ConvertValue(ConvertDecimal(row["100yenNum"]));                 // 現金残高100枚数
                    cashBalance.Num50yen = ConvertValue(ConvertDecimal(row["50yenNum"]));                   // 現金残高50枚数
                    cashBalance.Num10yen = ConvertValue(ConvertDecimal(row["10yenNum"]));                   // 現金残高10枚数
                    cashBalance.Num5yen = ConvertValue(ConvertDecimal(row["5yenNum"]));                     // 現金残高5枚数
                    cashBalance.Num1yen = ConvertValue(ConvertDecimal(row["1yenNum"]));                     // 現金残高1枚数
                    //
                    cashBalance.Gaku10000yen = ConvertValue(ConvertDecimal(row["10000yenGaku"]));           // 現金残高10,000金額
                    cashBalance.Gaku5000yen = ConvertValue(ConvertDecimal(row["5000yenGaku"]));             // 現金残高5,000金額
                    cashBalance.Gaku2000yen = ConvertValue(ConvertDecimal(row["2000yenGaku"]));             // 現金残高2,000金額
                    cashBalance.Gaku1000yen = ConvertValue(ConvertDecimal(row["1000yenGaku"]));             // 現金残高1,000金額
                    cashBalance.Gaku500yen = ConvertValue(ConvertDecimal(row["500yenGaku"]));               // 現金残高500金額
                    cashBalance.Gaku100yen = ConvertValue(ConvertDecimal(row["100yenGaku"]));               // 現金残高100金額
                    cashBalance.Gaku50yen = ConvertValue(ConvertDecimal(row["50yenGaku"]));                 // 現金残高50金額
                    cashBalance.Gaku10yen = ConvertValue(ConvertDecimal(row["10yenGaku"]));                 // 現金残高10金額
                    cashBalance.Gaku5yen = ConvertValue(ConvertDecimal(row["5yenGaku"]));                   // 現金残高5金額
                    cashBalance.Gaku1yen = ConvertValue(ConvertDecimal(row["1yenGaku"]));                   // 現金残高1金額
                    //
                    cashBalance.Etcyen = ConvertValue(ConvertDecimal(row["Etcyen"]));                       // その他金額
                    cashBalance.Change = ConvertValue(ConvertDecimal(row["Change"]));                       // 釣銭準備金
                    cashBalance.TotalGaku = ConvertValue(ConvertDecimal(row["DepositGaku"]));               // 現金残高 現金売上(+)
                    cashBalance.CashDeposit = ConvertValue(ConvertDecimal(row["CashDeposit"]));             // 現金残高 現金入金(+)
                    cashBalance.CashPayment = ConvertValue(ConvertDecimal(row["CashPayment"]));             // 現金残高 現金支払(-)
                    cashBalance.CashBalance = ConvertValue(ConvertDecimal(row["CashBalance"]));             // 現金残高 その他金額～現金残高現金支払(-)までの合計
                    cashBalance.ComputerTotal = ConvertValue(ConvertDecimal(row["ComputerTotal"]));         // ｺﾝﾋﾟｭｰﾀ計 現金残高 10,000金額～現金残高1金額までの合計
                    cashBalance.CashShortage = ConvertValue(ConvertDecimal(row["CashShortage"]));           // 現金残高 現金過不足
                    #endregion // 現金残高

                    #region 精算処理 総売
                    cashBalance.SalesNOCount = ConvertValue(ConvertDecimal(row["SalesNOCount"]));           // 総売 伝票数
                    cashBalance.CustomerCDCount = ConvertValue(ConvertDecimal(row["CustomerCDCount"]));     // 総売 客数(人)
                    cashBalance.SalesSUSum = ConvertValue(ConvertDecimal(row["SalesSUSum"]));               // 総売 売上数量
                    cashBalance.TotalGakuSum = ConvertValue(ConvertDecimal(row["TotalGakuSum"]));           // 総売 売上金額
                    #endregion // 精算処理 総売

                    #region 精算処理 取引別
                    cashBalance.ForeignTaxableAmount = ConvertValue(ConvertDecimal(row["ForeignTaxableAmount"]));         // 取引別 外税対象額
                    cashBalance.TaxableAmount = ConvertValue(ConvertDecimal(row["TaxableAmount"]));                       // 取引別 内税対象額
                    cashBalance.TaxExemptionAmount = ConvertValue(ConvertDecimal(row["TaxExemptionAmount"]));             // 取引別 非課税対象額
                    cashBalance.TotalWithoutTax = ConvertValue(ConvertDecimal(row["TotalWithoutTax"]));                   // 取引別 税抜合計
                    cashBalance.Tax = ConvertValue(ConvertDecimal(row["Tax"]));                                           // 取引別 内税
                    cashBalance.OutsideTax = ConvertValue(ConvertDecimal(row["OutsideTax"]));                             // 取引別 外税
                    cashBalance.ConsumptionTax = ConvertValue(ConvertDecimal(row["ConsumptionTax"]));                     // 取引別 消費税計
                    cashBalance.ForeignTaxableAmount = ConvertValue(ConvertDecimal(row["ForeignTaxableAmount"]));         // 取引別 外税対象額
                    cashBalance.TaxIncludedTotal = ConvertValue(ConvertDecimal(row["TaxIncludedTotal"]));                 // 取引別 税込合計
                    cashBalance.DiscountGaku = ConvertValue(ConvertDecimal(row["DiscountGaku"]));                         // 取引別 値引額
                    #endregion // 精算処理 取引別

                    #region 精算処理 決済別
                    // 決済別 金種1
                    cashBalance.DenominationName1 = Convert.ToString(row["DenominationName1"]);        // 区分名1
                    cashBalance.Kingaku1 = ConvertValue(ConvertDecimal(row["Kingaku1"]));              // 金額1

                    // 決済別 金種2
                    cashBalance.DenominationName2 = Convert.ToString(row["DenominationName2"]);        // 区分名2
                    cashBalance.Kingaku2 = ConvertValue(ConvertDecimal(row["Kingaku2"]));              // 金額2

                    // 決済別 金種3
                    cashBalance.DenominationName3 = Convert.ToString(row["DenominationName3"]);        // 区分名3
                    cashBalance.Kingaku3 = ConvertValue(ConvertDecimal(row["Kingaku3"]));              // 金額3

                    // 決済別 金種4
                    cashBalance.DenominationName4 = Convert.ToString(row["DenominationName4"]);        // 区分名4
                    cashBalance.Kingaku4 = ConvertValue(ConvertDecimal(row["Kingaku4"]));              // 金額4

                    // 決済別 金種5
                    cashBalance.DenominationName5 = Convert.ToString(row["DenominationName5"]);        // 区分名5
                    cashBalance.Kingaku5 = ConvertValue(ConvertDecimal(row["Kingaku5"]));              // 金額5

                    // 決済別 金種6
                    cashBalance.DenominationName6 = Convert.ToString(row["DenominationName6"]);        // 区分名6
                    cashBalance.Kingaku6 = ConvertValue(ConvertDecimal(row["Kingaku6"]));              // 金額6

                    // 決済別 金種7
                    cashBalance.DenominationName7 = Convert.ToString(row["DenominationName7"]);        // 区分名7
                    cashBalance.Kingaku7 = ConvertValue(ConvertDecimal(row["Kingaku7"]));              // 金額7

                    // 決済別 金種8
                    cashBalance.DenominationName8 = Convert.ToString(row["DenominationName8"]);        // 区分名8
                    cashBalance.Kingaku8 = ConvertValue(ConvertDecimal(row["Kingaku8"]));              // 金額8

                    // 決済別 金種9
                    cashBalance.DenominationName9 = Convert.ToString(row["DenominationName9"]);        // 区分名9
                    cashBalance.Kingaku9 = ConvertValue(ConvertDecimal(row["Kingaku9"]));              // 金額9

                    // 決済別 金種10
                    cashBalance.DenominationName10 = Convert.ToString(row["DenominationName10"]);      // 区分名10
                    cashBalance.Kingaku10 = ConvertValue(ConvertDecimal(row["Kingaku10"]));            // 金額10

                    // 決済別 金種11
                    cashBalance.DenominationName11 = Convert.ToString(row["DenominationName11"]);      // 区分名11
                    cashBalance.Kingaku11 = ConvertValue(ConvertDecimal(row["Kingaku11"]));            // 金額11

                    // 決済別 金種12
                    cashBalance.DenominationName12 = Convert.ToString(row["DenominationName12"]);      // 区分名12
                    cashBalance.Kingaku12 = ConvertValue(ConvertDecimal(row["Kingaku12"]));            // 金額12

                    // 決済別 金種13
                    cashBalance.DenominationName13 = Convert.ToString(row["DenominationName13"]);      // 区分名13
                    cashBalance.Kingaku13 = ConvertValue(ConvertDecimal(row["Kingaku13"]));            // 金額13

                    // 決済別 金種14
                    cashBalance.DenominationName14 = Convert.ToString(row["DenominationName14"]);      // 区分名14
                    cashBalance.Kingaku14 = ConvertValue(ConvertDecimal(row["Kingaku14"]));            // 金額14

                    // 決済別 金種15
                    cashBalance.DenominationName15 = Convert.ToString(row["DenominationName15"]);      // 区分名15
                    cashBalance.Kingaku15 = ConvertValue(ConvertDecimal(row["Kingaku15"]));            // 金額15

                    // 決済別 金種16
                    cashBalance.DenominationName16 = Convert.ToString(row["DenominationName16"]);      // 区分名16
                    cashBalance.Kingaku16 = ConvertValue(ConvertDecimal(row["Kingaku16"]));            // 金額16

                    // 決済別 金種17
                    cashBalance.DenominationName17 = Convert.ToString(row["DenominationName17"]);      // 区分名17
                    cashBalance.Kingaku17 = ConvertValue(ConvertDecimal(row["Kingaku17"]));            // 金額17

                    // 決済別 金種18
                    cashBalance.DenominationName18 = Convert.ToString(row["DenominationName18"]);      // 区分名18
                    cashBalance.Kingaku18 = ConvertValue(ConvertDecimal(row["Kingaku18"]));            // 金額18

                    // 決済別 金種19
                    cashBalance.DenominationName19 = Convert.ToString(row["DenominationName19"]);      // 区分名19
                    cashBalance.Kingaku19 = ConvertValue(ConvertDecimal(row["Kingaku19"]));            // 金額19

                    // 決済別 金種20
                    cashBalance.DenominationName20 = Convert.ToString(row["DenominationName20"]);      // 区分名20
                    cashBalance.Kingaku20 = ConvertValue(ConvertDecimal(row["Kingaku20"]));            // 金額20
                    #endregion // 精算処理 決済別

                    #region 精算処理 入金計
                    cashBalance.DepositTransfer = ConvertValue(ConvertDecimal(row["DepositTransfer"]));           // 入金計 振込
                    cashBalance.DepositCash = ConvertValue(ConvertDecimal(row["DepositCash"]));                   // 入金計 現金
                    cashBalance.DepositCheck = ConvertValue(ConvertDecimal(row["DepositCheck"]));                 // 入金計 小切手
                    cashBalance.DepositBill = ConvertValue(ConvertDecimal(row["DepositBill"]));                   // 入金計 手形
                    cashBalance.DepositOffset = ConvertValue(ConvertDecimal(row["DepositOffset"]));               // 入金計 相殺
                    cashBalance.DepositAdjustment = ConvertValue(ConvertDecimal(row["DepositAdjustment"]));       // 入金計 調整
                    #endregion // 精算処理 入金計

                    #region 精算処理 支払計
                    cashBalance.PaymentTransfer = ConvertValue(ConvertDecimal(row["PaymentTransfer"]));           // 支払計 振込
                    cashBalance.PaymentCash = ConvertValue(ConvertDecimal(row["PaymentCash"]));                   // 支払計 現金P
                    cashBalance.PaymentCheck = ConvertValue(ConvertDecimal(row["PaymentCheck"]));                 // 支払計 小切手
                    cashBalance.PaymentBill = ConvertValue(ConvertDecimal(row["PaymentBill"]));                   // 支払計 手形
                    cashBalance.PaymentOffset = ConvertValue(ConvertDecimal(row["PaymentOffset"]));               // 支払計 振込
                    cashBalance.PaymentAdjustment = ConvertValue(ConvertDecimal(row["PaymentAdjustment"]));       // 支払計 相殺
                    #endregion // 精算処理 支払計

                    #region 精算処理 他金額
                    cashBalance.Returns = ConvertValue(ConvertDecimal(row["OtherAmountReturns"]));          // 他金額 返品
                    cashBalance.Discount = ConvertValue(ConvertDecimal(row["OtherAmountDiscount"]));        // 他金額 値引
                    cashBalance.Cancel = ConvertValue(ConvertDecimal(row["OtherAmountCancel"]));            // 他金額 取消
                    cashBalance.Delivery = ConvertValue(ConvertDecimal(row["OtherAmountDelivery"]));        // 他金額 配達
                    cashBalance.ExchangeCount = ConvertValue(ConvertDecimal(row["ExchangeCount"]));         // 両替回数
                    #endregion // 精算処理 他金額

                    #region 精算処理 時間帯別件数
                    cashBalance.TimeZoneSalesFrom0000to0100 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0000_0100"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom0100to0200 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0100_0200"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom0200to0300 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0200_0300"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom0300to0400 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0300_0400"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom0400to0500 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0400_0500"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom0500to0600 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0500_0600"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom0600to0700 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0600_0700"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom0700to0800 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0700_0800"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom0800to0900 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0800_0900"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1000to1100 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1000_1100"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom0900to1000 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_0900_1000"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1100to1200 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1100_1200"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1200to1300 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1200_1300"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1300to1400 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1300_1400"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1400to1500 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1400_1500"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1500to1600 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1500_1600"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1600to1700 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1600_1700"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1700to1800 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1700_1800"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1800to1900 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1800_1900"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom1900to2000 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_1900_2000"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom2000to2100 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_2000_2100"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom2100to2200 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_2100_2200"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom2200to2300 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_2200_2300"]));              // 時間帯別件数 23:00～24:00
                    cashBalance.TimeZoneSalesFrom2300to2400 = ConvertValue(ConvertDecimal(row["ByTimeZoneSalesNO_2300_2400"]));              // 時間帯別件数 23:00～24:00
                    #endregion // 精算処理 時間帯別件数

                    #region 精算処理 時間帯別(税込)
                    cashBalance.TimeZoneTaxIncludedFrom0000to0100 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0000_0100"]));        // 時間帯別(税込) 00:00～01:00
                    cashBalance.TimeZoneTaxIncludedFrom0100to0200 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0100_0200"]));        // 時間帯別(税込) 01:00～02:00
                    cashBalance.TimeZoneTaxIncludedFrom0200to0300 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0200_0300"]));        // 時間帯別(税込) 02:00～03:00
                    cashBalance.TimeZoneTaxIncludedFrom0300to0400 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0300_0400"]));        // 時間帯別(税込) 03:00～04:00
                    cashBalance.TimeZoneTaxIncludedFrom0400to0500 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0400_0500"]));        // 時間帯別(税込) 04:00～05:00
                    cashBalance.TimeZoneTaxIncludedFrom0500to0600 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0500_0600"]));        // 時間帯別(税込) 05:00～06:00
                    cashBalance.TimeZoneTaxIncludedFrom0600to0700 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0600_0700"]));        // 時間帯別(税込) 06:00～07:00
                    cashBalance.TimeZoneTaxIncludedFrom0700to0800 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0700_0800"]));        // 時間帯別(税込) 07:00～08:00
                    cashBalance.TimeZoneTaxIncludedFrom0800to0900 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0800_0900"]));        // 時間帯別(税込) 08:00～09:00
                    cashBalance.TimeZoneTaxIncludedFrom0900to1000 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_0900_1000"]));        // 時間帯別(税込) 09:00～10:00
                    cashBalance.TimeZoneTaxIncludedFrom1000to1100 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1000_1100"]));        // 時間帯別(税込) 10:00～11:00
                    cashBalance.TimeZoneTaxIncludedFrom1100to1200 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1100_1200"]));        // 時間帯別(税込) 11:00～12:00
                    cashBalance.TimeZoneTaxIncludedFrom1200to1300 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1200_1300"]));        // 時間帯別(税込) 12:00～13:00
                    cashBalance.TimeZoneTaxIncludedFrom1300to1400 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1300_1400"]));        // 時間帯別(税込) 13:00～14:00
                    cashBalance.TimeZoneTaxIncludedFrom1400to1500 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1400_1500"]));        // 時間帯別(税込) 14:00～15:00
                    cashBalance.TimeZoneTaxIncludedFrom1500to1600 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1500_1600"]));        // 時間帯別(税込) 15:00～16:00
                    cashBalance.TimeZoneTaxIncludedFrom1600to1700 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1600_1700"]));        // 時間帯別(税込) 16:00～17:00
                    cashBalance.TimeZoneTaxIncludedFrom1700to1800 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1700_1800"]));        // 時間帯別(税込) 17:00～18:00
                    cashBalance.TimeZoneTaxIncludedFrom1800to1900 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1800_1900"]));        // 時間帯別(税込) 18:00～19:00
                    cashBalance.TimeZoneTaxIncludedFrom1900to2000 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_1900_2000"]));        // 時間帯別(税込) 19:00～20:00
                    cashBalance.TimeZoneTaxIncludedFrom2000to2100 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_2000_2100"]));        // 時間帯別(税込) 20:00～21:00
                    cashBalance.TimeZoneTaxIncludedFrom2100to2200 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_2100_2200"]));        // 時間帯別(税込) 21:00～22:00
                    cashBalance.TimeZoneTaxIncludedFrom2200to2300 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_2200_2300"]));        // 時間帯別(税込) 22:00～23:00
                    cashBalance.TimeZoneTaxIncludedFrom2300to2400 = ConvertValue(ConvertDecimal(row["ByTimeZoneTaxIncluded_2300_2400"]));        // 時間帯別(税込) 23:00～24:00
                    #endregion // 精算処理 時間帯別(税込)

                    storeDataSet.CashBalanceTable.Rows.Add(cashBalance);
                }
                #endregion // 精算処理 現金残高
            }

            #region 出力設定

            // StoreTable行追加
            AddStoreTableRow(storeDataSet.StoreTable, stores[0]);

            // SalesNoTable行一括追加
            AddSalesNoTableRows(storeDataSet.SalesNoTable, salesNos.OrderBy(v => v.IssueDateTime).ToList());

            // SalesTable行一括追加
            AddSalesTableRows(storeDataSet.SalesTable, sales.OrderBy(v => v.IssueDateTime).ThenBy(v => v.SalesNO).ThenBy(v => v.Row).ToList());

            if (storeDataSet.StoreTable.Rows.Count > 0)
            {
                // 各明細部印刷有無を印刷フラグで設定
                storeDataSet.StoreTable[0].DispSales = isPrint;

                if (isPrint)
                {
                    // 印刷するフラグON時、各明細部の出力件数が0件の場合は印刷フラグOFF

                    // 販売
                    if (storeDataSet.StoreTable.Count == 0)
                    {
                        storeDataSet.StoreTable[0].DispSales = false;
                    }
                }
            }

            #endregion // 出力設定

            return storeDataSet;
        }

        #region 雑入金データ
        /// <summary>
        /// 雑入金データをデータセットに設
        /// </summary>
        /// <param name="salesNO">売上番号</param>
        /// <param name="jornalTable">ジャーナル詳細データテーブル</param>
        /// <param name="date">雑入金日</param>
        /// <param name="name">雑入金名</param>
        /// <param name="amount">雑入金額</param>
        /// <param name="remark">雑入金備考</param>
        private void SetMiscDepositTable(string salesNO, StoreDataSet.SalesTableDataTable sales, StoreDataSet.SalesNoTableDataTable salesNos, string calendarDate, object date, object name, object amount, string remark, string storeReceiptPrint, string staffReceiptPrint)
        {
            var dateTime = "";

            if (date != DBNull.Value && Convert.ToDateTime(date).ToShortDateString() == Convert.ToDateTime(calendarDate).ToShortDateString())
            {
                dateTime = Convert.ToString(date);
            }

            var item = CreateSalesTableRow(sales, JournalDataKind.MiscDeposit);
            item.IssueDate = string.IsNullOrWhiteSpace(dateTime) ? DATE_MIN : dateTime.Substring(0, dateTime.LastIndexOf(" "));
            item.DispDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime.Substring(0, dateTime.LastIndexOf(":"));
            item.IssueDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime;
            item.Name = Convert.ToString(name);
            item.Amount = ConvertDecimal(amount);
            item.Remark = remark;
            item.StoreReceiptPrint = storeReceiptPrint;
            item.StaffReceiptPrint = staffReceiptPrint;

            if (sales.AsEnumerable().Where(
                    j => j.DataKind == item.DataKind &&
                         j.IssueDate == item.IssueDate &&
                         j.IssueDateTime == item.IssueDateTime &&
                         j.Name == item.Name &&
                         j.Amount == item.Amount &&
                         j.Remark == item.Remark).SingleOrDefault() == null)
            {
                sales.Rows.Add(item);

                // SalesNoTableに新規行追加
                var salesNo = CreateSalesNoTableRow(salesNos);
                salesNo.StoreReceiptPrint = storeReceiptPrint;        // 店舗レシート表記
                salesNo.StaffReceiptPrint = staffReceiptPrint;        // 担当レシート表記
                salesNo.IssueDate = item.IssueDate;                   // 発行日
                salesNo.IssueDateTime = item.IssueDateTime;           // 発行日時

                AddSalesNoTableRow(salesNos, salesNo);
            }
        }
        #endregion // 雑入金データ

        #region 入金データ
        /// <summary>
        /// 入金データをデータセットに設定
        /// </summary>
        /// <param name="salesNO">売上番号</param>
        /// <param name="jornalTable">ジャーナル詳細データテーブル</param>
        /// <param name="date">入金日</param>
        /// <param name="customerCD">入金元CD</param>
        /// <param name="customerName">入金元名</param>
        /// <param name="name">入金名</param>
        /// <param name="amount">入金額</param>
        /// <param name="remark">入金備考</param>
        private void SetDepositTable(string salesNO, StoreDataSet.SalesTableDataTable sales, StoreDataSet.SalesNoTableDataTable salesNos, string calendarDate, object date, string customerCD, string customerName, object name, object amount, string remark, string storeReceiptPrint, string staffReceiptPrint)
        {
            var dateTime = "";

            if (date != DBNull.Value && Convert.ToDateTime(date).ToShortDateString() == Convert.ToDateTime(calendarDate).ToShortDateString())
            {
                dateTime = Convert.ToString(date);
            }

            var item = CreateSalesTableRow(sales, JournalDataKind.Deposit);
            item.IssueDate = string.IsNullOrWhiteSpace(dateTime) ? DATE_MIN : dateTime.Substring(0, dateTime.LastIndexOf(" "));
            item.DispDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime.Substring(0, dateTime.LastIndexOf(":"));
            item.IssueDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime;
            item.CustomerCD = customerCD;
            item.CustomerName = customerName;
            item.Name = Convert.ToString(name);
            item.Amount = ConvertDecimal(amount);
            item.Remark = remark;
            item.StoreReceiptPrint = storeReceiptPrint;
            item.StaffReceiptPrint = staffReceiptPrint;

            if (sales.AsEnumerable().Where(
                    j => j.DataKind == item.DataKind &&
                         j.IssueDate == item.IssueDate &&
                         j.IssueDateTime == item.IssueDateTime &&
                         j.CustomerCD == item.CustomerCD &&
                         j.CustomerName == item.CustomerName &&
                         j.Name == item.Name &&
                         j.Amount == item.Amount &&
                         j.Remark == item.Remark).SingleOrDefault() == null)
            {
                sales.Rows.Add(item);

                // SalesNoTableに新規行追加
                var salesNo = CreateSalesNoTableRow(salesNos);
                salesNo.StoreReceiptPrint = storeReceiptPrint;        // 店舗レシート表記
                salesNo.StaffReceiptPrint = staffReceiptPrint;        // 担当レシート表記
                salesNo.IssueDate = item.IssueDate;                   // 発行日
                salesNo.IssueDateTime = item.IssueDateTime;           // 発行日時

                AddSalesNoTableRow(salesNos, salesNo);
            }
        }
        #endregion // 入金データ

        #region 雑出金データ
        /// <summary>
        /// 雑出金データをデータセットに設定
        /// </summary>
        /// <param name="salesNO">売上番号</param>
        /// <param name="jornalTable">ジャーナル詳細データテーブル</param>
        /// <param name="date">雑出金日</param>
        /// <param name="name">雑出金名</param>
        /// <param name="amount">雑出金額</param>
        /// <param name="remark">雑出金備考</param>
        private void SetMiscPaymentTable(string salesNO, StoreDataSet.SalesTableDataTable sales, StoreDataSet.SalesNoTableDataTable salesNos, string calendarDate, object date, object name, object amount, string remark, string storeReceiptPrint, string staffReceiptPrint)
        {
            var dateTime = "";

            if (date != DBNull.Value && Convert.ToDateTime(date).ToShortDateString() == Convert.ToDateTime(calendarDate).ToShortDateString())
            {
                dateTime = Convert.ToString(date);
            }

            var item = CreateSalesTableRow(sales, JournalDataKind.MiscPayment);
            item.IssueDate = string.IsNullOrWhiteSpace(dateTime) ? DATE_MIN : dateTime.Substring(0, dateTime.LastIndexOf(" "));
            item.DispDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime.Substring(0, dateTime.LastIndexOf(":"));
            item.IssueDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime;
            item.Name = Convert.ToString(name);
            item.Amount = ConvertDecimal(amount);
            item.Remark = remark;
            item.StoreReceiptPrint = storeReceiptPrint;
            item.StaffReceiptPrint = staffReceiptPrint;

            if (sales.AsEnumerable().Where(
                    j => j.DataKind == item.DataKind &&
                         j.IssueDate == item.IssueDate &&
                         j.IssueDateTime == item.IssueDateTime &&
                         j.Name == item.Name &&
                         j.Amount == item.Amount &&
                         j.Remark == item.Remark).SingleOrDefault() == null)
            {
                sales.Rows.Add(item);

                // SalesNoTableに新規行追加
                var salesNo = CreateSalesNoTableRow(salesNos);
                salesNo.StoreReceiptPrint = storeReceiptPrint;        // 店舗レシート表記
                salesNo.StaffReceiptPrint = staffReceiptPrint;        // 担当レシート表記
                salesNo.IssueDate = item.IssueDate;                   // 発行日
                salesNo.IssueDateTime = item.IssueDateTime;           // 発行日時

                AddSalesNoTableRow(salesNos, salesNo);
            }
        }
        #endregion // 雑出金データ

        #region 両替データ
        /// <summary>
        /// 両替データをデータセットに設定
        /// </summary>
        /// <param name="salesNO">売上番号</param>
        /// <param name="jornalTable">ジャーナル詳細データテーブル</param>
        /// <param name="date">両替日</param>
        /// <param name="name">両替名</param>
        /// <param name="amount">両替金額</param>
        /// <param name="denomination">両替紙幣</param>
        /// <param name="exchangeCount">両替枚数</param>
        /// <param name="remark">両替備考</param>
        private void SetExchangTable(string salesNO, StoreDataSet.SalesTableDataTable sales, StoreDataSet.SalesNoTableDataTable salesNos, string calendarDate, object date, object name, object amount, object denomination, object exchangeCount, string remark, string storeReceiptPrint, string staffReceiptPrint)
        {
            var dateTime = "";

            if (date != DBNull.Value && Convert.ToDateTime(date).ToShortDateString() == Convert.ToDateTime(calendarDate).ToShortDateString())
            {
                dateTime = Convert.ToString(date);
            }

            var item = CreateSalesTableRow(sales, JournalDataKind.Exchange);
            item.IssueDate = string.IsNullOrWhiteSpace(dateTime) ? DATE_MIN : dateTime.Substring(0, dateTime.LastIndexOf(" "));
            item.DispDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime.Substring(0, dateTime.LastIndexOf(":"));
            item.IssueDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime;
            item.Name = Convert.ToString(name);
            item.Amount = ConvertDecimal(amount);
            item.Denomination = Convert.ToString(denomination);
            item.ExchangeCount = ConvertDecimal(exchangeCount);
            item.Remark = remark;
            item.StoreReceiptPrint = storeReceiptPrint;
            item.StaffReceiptPrint = staffReceiptPrint;

            if (sales.AsEnumerable().Where(
                    j => j.DataKind == item.DataKind &&
                         j.IssueDate == item.IssueDate &&
                         j.IssueDateTime == item.IssueDateTime &&
                         j.Name == item.Name &&
                         j.Amount == item.Amount &&
                         j.Denomination == item.Denomination &&
                         j.ExchangeCount == item.ExchangeCount &&
                         j.Remark == item.Remark).SingleOrDefault() == null)
            {
                sales.Rows.Add(item);

                // SalesNoTableに新規行追加
                var salesNo = CreateSalesNoTableRow(salesNos);
                salesNo.StoreReceiptPrint = storeReceiptPrint;        // 店舗レシート表記
                salesNo.StaffReceiptPrint = staffReceiptPrint;        // 担当レシート表記
                salesNo.IssueDate = item.IssueDate;                   // 発行日
                salesNo.IssueDateTime = item.IssueDateTime;           // 発行日時

                AddSalesNoTableRow(salesNos, salesNo);
            }
        }
        #endregion // 両替データ

        #region 釣銭準備データ
        /// <summary>
        /// 釣銭準備データをデータセットに設定
        /// </summary>
        /// <param name="salesNO">売上番号</param>
        /// <param name="jornalTable">ジャーナル詳細データテーブル</param>
        /// <param name="date">釣銭準備日</param>
        /// <param name="name">釣銭準備名</param>
        /// <param name="amount">釣銭準備金額</param>
        /// <param name="remark">釣銭準備備考</param>
        private void SetChangePreparationTable(string salesNO, StoreDataSet.SalesTableDataTable sales, StoreDataSet.SalesNoTableDataTable salesNos, string calendarDate, object date, object name, object amount, string remark, string storeReceiptPrint, string staffReceiptPrint)
        {
            var dateTime = "";

            if (date != DBNull.Value && Convert.ToDateTime(date).ToShortDateString() == Convert.ToDateTime(calendarDate).ToShortDateString())
            {
                dateTime = Convert.ToString(date);
            }

            var item = CreateSalesTableRow(sales, JournalDataKind.ChangePreparation);
            item.IssueDate = string.IsNullOrWhiteSpace(dateTime) ? DATE_MIN : dateTime.Substring(0, dateTime.LastIndexOf(" "));
            item.DispDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime.Substring(0, dateTime.LastIndexOf(":"));
            item.IssueDateTime = string.IsNullOrWhiteSpace(dateTime) ? DATETIME_MIN : dateTime;
            item.Name = Convert.ToString(name);
            item.Amount = ConvertDecimal(amount);
            item.Remark = remark;
            item.StoreReceiptPrint = storeReceiptPrint;
            item.StaffReceiptPrint = staffReceiptPrint;

            if (sales.AsEnumerable().Where(
                    j => j.DataKind == item.DataKind &&
                         j.IssueDate == item.IssueDate &&
                         j.IssueDateTime == item.IssueDateTime &&
                         j.Name == item.Name &&
                         j.Amount == item.Amount &&
                         j.Remark == item.Remark).SingleOrDefault() == null)
            {
                sales.Rows.Add(item);

                // SalesNoTableに新規行追加
                var salesNo = CreateSalesNoTableRow(salesNos);
                salesNo.StoreReceiptPrint = storeReceiptPrint;        // 店舗レシート表記
                salesNo.StaffReceiptPrint = staffReceiptPrint;        // 担当レシート表記
                salesNo.IssueDate = item.IssueDate;                   // 発行日
                salesNo.IssueDateTime = item.IssueDateTime;           // 発行日時

                AddSalesNoTableRow(salesNos, salesNo);
            }
        }
        #endregion // 釣銭準備データ

        #region StoreTable

        #region StoreTableRow取得
        /// <summary>
        /// StoreTableの行を取得
        /// </summary>
        /// <param name="stores">StoreTable</param>
        /// <returns>StoreTableRow</returns>
        private StoreDataSet.StoreTableRow CreateStoreTableRow(StoreDataSet.StoreTableDataTable stores)
        {
            var store = stores.NewStoreTableRow();
            store.Logo = null;                  // ロゴ
            store.StoreName = "";               // 店舗名
            store.Address1 = "";                // 住所1
            store.Address2 = "";                // 住所2
            store.TelphoneNO = "";              // 電話番号
            store.StoreReceiptPrint = "";       // 店舗レシート表記
            
            return store;
        }
        #endregion // StoreTableRow取得

        #region StoreTable行追加
        /// <summary>
        /// StoreTableに行を追加
        /// </summary>
        /// <param name="stores">追加先StoreTable</param>
        /// <param name="store">追加する行</param>
        private void AddStoreTableRow(StoreDataSet.StoreTableDataTable stores, StoreDataSet.StoreTableRow store)
        {
            var row = CreateStoreTableRow(stores);
            row.Logo = store.Logo;                                  // ロゴ
            row.StoreName = store.StoreName;                        // 店舗名
            row.Address1 = store.Address1;                          // 住所1
            row.Address2 = store.Address2;                          // 住所2
            row.TelphoneNO = store.TelphoneNO;                      // 電話番号
            row.StoreReceiptPrint = store.StoreReceiptPrint;        // 店舗レシート表記

            stores.Rows.Add(row);
        }
        #endregion // StoreTable行追加

        #endregion // StoreTable

        #region SalesNoTable

        #region SalesNoTableRow取得
        /// <summary>
        /// SalesNoTableの行を取得
        /// </summary>
        /// <param name="salesNos">SalesNoTable</param>
        /// <returns>SalesNoTableRow</returns>
        private StoreDataSet.SalesNoTableRow CreateSalesNoTableRow(StoreDataSet.SalesNoTableDataTable salesNos)
        {
            var salesNo = salesNos.NewSalesNoTableRow();
            salesNo.StoreReceiptPrint = "";             // 店舗レシート表記
            salesNo.StaffReceiptPrint = "";             // 担当レシート表記
            salesNo.SalesNO = "";                       // 売上番号
            salesNo.IssueDate = "";                     // 発行日
            salesNo.IssueDateTime = "";                 // 発行日時

            return salesNo;
        }
        #endregion // SalesNoTableRow取得

        #region SalesNoTable行一括追加
        /// <summary>
        /// SalesNoTableに行を一括追加
        /// </summary>
        /// <param name="salesNos">追加先SalesNoTable</param>
        /// <param name="srcs">追加する行リスト</param>
        private void AddSalesNoTableRows(StoreDataSet.SalesNoTableDataTable salesNos, List<StoreDataSet.SalesNoTableRow> srcs)
        {
            foreach(var src in srcs)
            {
                AddSalesNoTableRow(salesNos, src);
            }
        }
        #endregion // SalesNoTable行一括追加

        #region SalesNoTable行追加
        /// <summary>
        /// SalesNoTableに行を追加
        /// </summary>
        /// <param name="salesNos">追加先SalesNoTable</param>
        /// <param name="salesNo">追加する行</param>
        private void AddSalesNoTableRow(StoreDataSet.SalesNoTableDataTable salesNos, StoreDataSet.SalesNoTableRow salesNo)
        {
            var row = CreateSalesNoTableRow(salesNos);
            row.StoreReceiptPrint = salesNo.StoreReceiptPrint;          // 店舗レシート表記
            row.StaffReceiptPrint = salesNo.StaffReceiptPrint;          // 担当レシート表記
            row.SalesNO = salesNo.SalesNO;                              // 売上番号
            row.IssueDate = salesNo.IssueDate;                          // 発行日
            row.IssueDateTime = salesNo.IssueDateTime;                  // 発行日時

            if (salesNos.AsEnumerable().Where(s => s.IssueDateTime == row.IssueDateTime).FirstOrDefault() == null)
            {
                salesNos.Rows.Add(row);
            }
        }
        #endregion // SalesNoTable行追加

        #endregion // SalesNoTable

        #region SalesTable

        #region SalesTableRow取得
        /// <summary>
        /// SalesTableの行を取得
        /// </summary>
        /// <param name="sales">SalesTable</param>
        /// <param name="dataKind">データ種別</param>
        /// <returns>SalesTableRow</returns>
        private StoreDataSet.SalesTableRow CreateSalesTableRow(StoreDataSet.SalesTableDataTable sales, JournalDataKind dataKind)
        {
            var sale = sales.NewSalesTableRow();
            sale.StoreReceiptPrint = "";            // 店舗レシート表記
            sale.StaffReceiptPrint = "";            // 担当レシート表記
            sale.SalesNO = "";                      // 売上番号
            sale.IssueDate = "";                    // 発行日
            sale.IssueDateTime = "";                // 発行日時
            sale.DispDateTime = "";                 // 表示用発行日時
            sale.Row = 1;                           // ROW
            sale.JanCD = "";                        // JANCD
            sale.SalesSU = "";                      // 数量
            sale.SalesUnitPrice = "";               // 単価
            sale.SalesGaku = "";                    // 価格
            sale.SalesTax = "";                     // 売上消費税額
            sale.SalesTaxRate = "";                 // 税率
            sale.TotalGaku = "";                    // 販売合計額
            sale.SKUShortName1 = "";                // 商品名1
            sale.SKUShortName2 = "";                // 商品名2
            sale.SumSalesSU = "";                   // 小計数量
            sale.Subtotal = "";                     // 小計金額
            sale.TargetAmount8 = "";                // 消費税対象額8%
            sale.ConsumptionTax8 = "";              // 内消費税等8%
            sale.TargetAmount10 = "";               // 消費税対象額10%
            sale.ConsumptionTax10 = "";             // 内消費税等10%
            sale.Total = "";                        // 合計
            sale.PaymentName1 = "";                 // 支払方法名1
            sale.PaymentAmount1 = "";               // 支払方法額1
            sale.PaymentName2 = "";                 // 支払方法名2
            sale.PaymentAmount2 = "";               // 支払方法額2
            sale.PaymentName3 = "";                 // 支払方法名3
            sale.PaymentAmount3 = "";               // 支払方法額3
            sale.PaymentName4 = "";                 // 支払方法名4
            sale.PaymentAmount4 = "";               // 支払方法額4
            sale.PaymentName5 = "";                 // 支払方法名5
            sale.PaymentAmount5 = "";               // 支払方法額5
            sale.PaymentName6 = "";                 // 支払方法名6
            sale.PaymentAmount6 = "";               // 支払方法額6
            sale.PaymentName7 = "";                 // 支払方法名7
            sale.PaymentAmount7 = "";               // 支払方法額7
            sale.PaymentName8 = "";                 // 支払方法名8
            sale.PaymentAmount8 = "";               // 支払方法額8
            sale.PaymentName9 = "";                 // 支払方法名9
            sale.PaymentAmount9 = "";               // 支払方法額9
            sale.PaymentName10 = "";                // 支払方法名10
            sale.PaymentAmount10 = "";              // 支払方法額10
            sale.Refund = "";                       // 釣銭
            sale.DiscountGaku = "";                 // 値引額
            sale.CustomerCD = "";                   // 入金元コード
            sale.CustomerName = "";                 // 入金元名 
            sale.Name = "";                         // 名前
            sale.Amount = "";                       // 金額
            sale.Denomination = "";                 // 両替紙幣
            sale.ExchangeCount = "";                // 両替回数
            sale.Remark = "";                       // 備考
            sale.DataKind = (short)dataKind;        // データ種別

            return sale;
        }
        #endregion // SalesTableRow取得

        #region SalesTable行一括追加
        /// <summary>
        /// SalesTableに行を一括追加
        /// </summary>
        /// <param name="sales">追加先SalesTable</param>
        /// <param name="srcs">追加する行リスト</param>
        private void AddSalesTableRows(StoreDataSet.SalesTableDataTable sales, List<StoreDataSet.SalesTableRow> srcs)
        {
            foreach(var src in srcs)
            {
                AddSalesTableRow(sales, src);
            }
        }
        #endregion // SalesTable行一括追加

        #region SalesTable行追加
        /// <summary>
        /// SalesNoTableに行を追加
        /// </summary>
        /// <param name="salesNos">追加先SalesNoTable</param>
        /// <param name="salesNo">追加する行</param>
        private void AddSalesTableRow(StoreDataSet.SalesTableDataTable sales, StoreDataSet.SalesTableRow sale)
        {
            var row = CreateSalesTableRow(sales, (JournalDataKind)sale.DataKind);
            row.StoreReceiptPrint = sale.StoreReceiptPrint;         // 店舗レシート表記
            row.StaffReceiptPrint = sale.StaffReceiptPrint;         // 担当レシート表記
            row.SalesNO = sale.SalesNO;                             // 売上番号
            row.IssueDate = sale.IssueDate;                         // 発行日
            row.IssueDateTime = sale.IssueDateTime;                 // 発行日時
            row.DispDateTime = sale.DispDateTime;                   // 表示用発行日時
            row.Row = sale.Row;                                     // ROW
            row.JanCD = sale.JanCD;                                 // JANCD
            row.SalesSU = sale.SalesSU;                             // 数量
            row.SalesUnitPrice = sale.SalesUnitPrice;               // 単価
            row.SalesGaku = sale.SalesGaku;                         // 価格
            row.SalesTax = sale.SalesTax;                           // 売上消費税額
            row.SalesTaxRate = sale.SalesTaxRate;                   // 税率
            row.TotalGaku = sale.TotalGaku;                         // 販売合計額
            row.SKUShortName1 = sale.SKUShortName1;                 // 商品名1
            row.SKUShortName2 = sale.SKUShortName2;                 // 商品名2
            row.SumSalesSU = sale.SumSalesSU;                       // 小計数量
            row.Subtotal = sale.Subtotal;                           // 小計金額
            row.TargetAmount8 = sale.TargetAmount8;                 // 消費税対象額8%
            row.ConsumptionTax8 = sale.ConsumptionTax8;             // 内消費税等8%
            row.TargetAmount10 = sale.TargetAmount10;               // 消費税対象額10%
            row.ConsumptionTax10 = sale.ConsumptionTax10;           // 内消費税等10%
            row.Total = sale.Total;                                 // 合計
            row.PaymentName1 = sale.PaymentName1;                   // 支払方法名1
            row.PaymentAmount1 = sale.PaymentAmount1;               // 支払方法額1
            row.PaymentName2 = sale.PaymentName2;                   // 支払方法名2
            row.PaymentAmount2 = sale.PaymentAmount2;               // 支払方法額2
            row.PaymentName3 = sale.PaymentName3;                   // 支払方法名3
            row.PaymentAmount3 = sale.PaymentAmount3;               // 支払方法額3
            row.PaymentName4 = sale.PaymentName4;                   // 支払方法名4
            row.PaymentAmount4 = sale.PaymentAmount4;               // 支払方法額4
            row.PaymentName5 = sale.PaymentName5;                   // 支払方法名5
            row.PaymentAmount5 = sale.PaymentAmount5;               // 支払方法額5
            row.PaymentName6 = sale.PaymentName6;                   // 支払方法名6
            row.PaymentAmount6 = sale.PaymentAmount6;               // 支払方法額6
            row.PaymentName7 = sale.PaymentName7;                   // 支払方法名7
            row.PaymentAmount7 = sale.PaymentAmount7;               // 支払方法額7
            row.PaymentName8 = sale.PaymentName8;                   // 支払方法名8
            row.PaymentAmount8 = sale.PaymentAmount8;               // 支払方法額8
            row.PaymentName9 = sale.PaymentName9;                   // 支払方法名9
            row.PaymentAmount9 = sale.PaymentAmount9;               // 支払方法額9
            row.PaymentName10 = sale.PaymentName10;                 // 支払方法名10
            row.PaymentAmount10 = sale.PaymentAmount10;             // 支払方法額10
            row.Refund = sale.Refund;                               // 釣銭
            row.DiscountGaku = sale.DiscountGaku;                   // 値引額
            row.CustomerCD = sale.CustomerCD;                       // 入金元コード
            row.CustomerName = sale.CustomerName;                   // 入金元名 
            row.Name = sale.Name;                                   // 名前
            row.Amount = sale.Amount;                               // 金額
            row.Denomination = sale.Denomination;                   // 両替紙幣
            row.ExchangeCount = sale.ExchangeCount;                 // 両替回数
            row.Remark = sale.Remark;                               // 備考

            //if (sales.AsEnumerable().Where(s => s.IssueDateTime == row.IssueDateTime).FirstOrDefault() == null)
            //{
            //    sales.Rows.Add(row);
            //}
            sales.Rows.Add(row);
        }
        #endregion // SalesTable行追加

        #endregion // SalesTable

        #endregion // データセット

        /// <summary>
        /// 日時をyyyy/MM/dd hh:miで取得
        /// </summary>
        /// <param name="value">元の日時</param>
        /// <returns>日時</returns>
        private string ConvertDateTime(object value, bool dateOnly)
        {
            var result = string.Empty;

            var dateTime = Convert.ToString(value);
            if(!string.IsNullOrWhiteSpace(dateTime))
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

        private string ConvertValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "0" : value;
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

        /// <summary>
        /// 日付(From)キーダウンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// エンターキー押下でエラーなしの場合、日付(To)へ
        /// 日付(To)が空白時、日付(From)の値を日付(To)へセット
        /// </remarks>
        private void txtPrintDateFrom_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(string.IsNullOrWhiteSpace(txtPrintDateTo.Text))
                {
                    txtPrintDateTo.Text = txtPrintDateFrom.Text;
                }

                txtPrintDateTo.Focus();
            }
        }

        /// <summary>
        /// 日付(To)キーダウンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrintDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(ErrorCheck())
                {
                    PrintCheckBox.Focus();
                }
            }
        }

        private void PrintCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnClose.Focus();
            }
        }
    }
}
