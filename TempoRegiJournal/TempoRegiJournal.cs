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
            MiscDeposit = 1,

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
            var storeDataSet = new StoreDataSet();
            var journalTable = new StoreDataSet.JournalTableDataTable();

            for (var index = 0; index < data.Rows.Count; index++)
            {
                var row = data.Rows[index];

                if (string.IsNullOrWhiteSpace(ConvertDateTime(row["IssueDate"], false)))
                {
                    // 発行日時がないデータは出力対象外
                    continue;
                }

                // 共通データ
                var salesNO = Convert.ToString(row["SalesNO"]);                             // 売上番号
                var storeReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);         // 店舗レシート表記
                var staffReceiptPrint = Convert.ToString(row["StaffReceiptPrint"]);         // 担当レシート表記

                #region 店舗データ
                if (storeDataSet.StoreTable.Rows.Count == 0)
                {
                    var store = storeDataSet.StoreTable.NewStoreTableRow();
                    store.Logo = (byte[])row["Logo"];
                    store.StoreName = Convert.ToString(row["StoreName"]);                       // 店舗名
                    store.Address1 = Convert.ToString(row["Address1"]);                         // 住所1
                    store.Address2 = Convert.ToString(row["Address2"]);                         // 住所2
                    store.TelphoneNO = Convert.ToString(row["TelephoneNO"]);                    // 電話番号
                    store.StoreReceiptPrint = Convert.ToString(row["StoreReceiptPrint"]);       // 店舗レシート表記
                                                                                                //
                    storeDataSet.StoreTable.Rows.Add(store);
                }
                #endregion // 店舗データ

                #region 売上番号データ
                var salesNos = storeDataSet.SalesNoTable.NewSalesNoTableRow();
                salesNos.StoreReceiptPrint = storeReceiptPrint;                                    // 店舗レシート表記
                salesNos.StaffReceiptPrint = staffReceiptPrint;                                    // 担当レシート表記
                salesNos.SalesNO = salesNO;                                                        // 売上番号
                salesNos.IssueDate = ConvertDateTime(row["IssueDate"], true);                      // 発行日
                salesNos.IssueDateTime = ConvertDateTime(row["IssueDate"], false);                 // 発行日時

                if (storeDataSet.SalesNoTable.AsEnumerable().Where(s => s.SalesNO == salesNO).FirstOrDefault() == null)
                {
                    storeDataSet.SalesNoTable.Rows.Add(salesNos);
                }
                #endregion  // 売上番号データ

                #region 販売データ
                var sales = storeDataSet.SalesTable.NewSalesTableRow();
                sales.StoreReceiptPrint = storeReceiptPrint;                                    // 店舗レシート表記
                sales.StaffReceiptPrint = staffReceiptPrint;                                    // 担当レシート表記
                sales.SalesNO = salesNO;                                                        // 売上番号
                sales.IssueDate = ConvertDateTime(row["IssueDate"], true);                      // 発行日
                sales.IssueDateTime = ConvertDateTime(row["IssueDate"], false);                 // 発行日時
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
                    sales.SalesUnitPrice = ConvertDecimal(row["SalesUnitPrice"]);               // 単価
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
                sales.DiscountGaku = ConvertDecimal(row["DiscountGaku"]);                       // 値引額
                #endregion // お釣りデータ
                //
                storeDataSet.SalesTable.Rows.Add(sales);

                #endregion // 販売データ

                #region 雑入金データ
                var remark = Convert.ToString(row["MiscDepositRemark"]);        // 雑入金備考

                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate1"], row["MiscDepositName1"], row["MiscDepositAmount1"], remark);
                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate2"], row["MiscDepositName2"], row["MiscDepositAmount2"], remark);
                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate3"], row["MiscDepositName3"], row["MiscDepositAmount3"], remark);
                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate4"], row["MiscDepositName4"], row["MiscDepositAmount4"], remark);
                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate5"], row["MiscDepositName5"], row["MiscDepositAmount5"], remark);
                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate6"], row["MiscDepositName6"], row["MiscDepositAmount6"], remark);
                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate7"], row["MiscDepositName7"], row["MiscDepositAmount7"], remark);
                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate8"], row["MiscDepositName8"], row["MiscDepositAmount8"], remark);
                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate9"], row["MiscDepositName9"], row["MiscDepositAmount9"], remark);
                SetMiscDepositTable(salesNO, journalTable, row["MiscDepositDate10"], row["MiscDepositName10"], row["MiscDepositAmount10"], remark);
                #endregion // 雑入金データ

                #region 入金データ
                var customerCD = Convert.ToString(row["CustomerCD"]);           // 入金元CD
                var customerName = Convert.ToString(row["CustomerName"]);       // 入金元名
                remark = Convert.ToString(row["DepositRemark"]);                // 入金備考

                SetDepositTable(salesNO, journalTable, row["DepositDate1"], customerCD, customerName, row["DepositName1"], row["DepositAmount1"], remark);
                SetDepositTable(salesNO, journalTable, row["DepositDate2"], customerCD, customerName, row["DepositName2"], row["DepositAmount2"], remark);
                SetDepositTable(salesNO, journalTable, row["DepositDate3"], customerCD, customerName, row["DepositName3"], row["DepositAmount3"], remark);
                SetDepositTable(salesNO, journalTable, row["DepositDate4"], customerCD, customerName, row["DepositName4"], row["DepositAmount4"], remark);
                SetDepositTable(salesNO, journalTable, row["DepositDate5"], customerCD, customerName, row["DepositName5"], row["DepositAmount5"], remark);
                SetDepositTable(salesNO, journalTable, row["DepositDate6"], customerCD, customerName, row["DepositName6"], row["DepositAmount6"], remark);
                SetDepositTable(salesNO, journalTable, row["DepositDate7"], customerCD, customerName, row["DepositName7"], row["DepositAmount7"], remark);
                SetDepositTable(salesNO, journalTable, row["DepositDate8"], customerCD, customerName, row["DepositName8"], row["DepositAmount8"], remark);
                SetDepositTable(salesNO, journalTable, row["DepositDate9"], customerCD, customerName, row["DepositName9"], row["DepositAmount9"], remark);
                SetDepositTable(salesNO, journalTable, row["DepositDate10"], customerCD, customerName, row["DepositName10"], row["DepositAmount10"], remark);
                #endregion // 入金データ

                #region 雑出金データ
                remark = Convert.ToString(row["MiscPaymentRemark"]);        // 雑出金備考

                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate1"], row["MiscPaymentName1"], row["MiscPaymentAmount1"], remark);
                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate2"], row["MiscPaymentName2"], row["MiscPaymentAmount2"], remark);
                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate3"], row["MiscPaymentName3"], row["MiscPaymentAmount3"], remark);
                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate4"], row["MiscPaymentName4"], row["MiscPaymentAmount4"], remark);
                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate5"], row["MiscPaymentName5"], row["MiscPaymentAmount5"], remark);
                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate6"], row["MiscPaymentName6"], row["MiscPaymentAmount6"], remark);
                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate7"], row["MiscPaymentName7"], row["MiscPaymentAmount7"], remark);
                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate8"], row["MiscPaymentName8"], row["MiscPaymentAmount8"], remark);
                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate9"], row["MiscPaymentName9"], row["MiscPaymentAmount9"], remark);
                SetMiscPaymentTable(salesNO, journalTable, row["MiscPaymentDate10"], row["MiscPaymentName10"], row["MiscPaymentAmount10"], remark);
                #endregion // 雑出金データ

                #region 両替データ
                remark = Convert.ToString(row["ExchangeRemark"]);       // 両替備考

                SetExchangTable(salesNO, journalTable, row["ExchangeDate1"], row["ExchangeName1"], row["ExchangeAmount1"], row["ExchangeDenomination1"], row["ExchangeCount1"], remark);
                SetExchangTable(salesNO, journalTable, row["ExchangeDate2"], row["ExchangeName2"], row["ExchangeAmount2"], row["ExchangeDenomination2"], row["ExchangeCount2"], remark);
                SetExchangTable(salesNO, journalTable, row["ExchangeDate3"], row["ExchangeName3"], row["ExchangeAmount3"], row["ExchangeDenomination3"], row["ExchangeCount3"], remark);
                SetExchangTable(salesNO, journalTable, row["ExchangeDate4"], row["ExchangeName4"], row["ExchangeAmount4"], row["ExchangeDenomination4"], row["ExchangeCount4"], remark);
                SetExchangTable(salesNO, journalTable, row["ExchangeDate5"], row["ExchangeName5"], row["ExchangeAmount5"], row["ExchangeDenomination5"], row["ExchangeCount5"], remark);
                SetExchangTable(salesNO, journalTable, row["ExchangeDate6"], row["ExchangeName6"], row["ExchangeAmount6"], row["ExchangeDenomination6"], row["ExchangeCount6"], remark);
                SetExchangTable(salesNO, journalTable, row["ExchangeDate7"], row["ExchangeName7"], row["ExchangeAmount7"], row["ExchangeDenomination7"], row["ExchangeCount7"], remark);
                SetExchangTable(salesNO, journalTable, row["ExchangeDate8"], row["ExchangeName8"], row["ExchangeAmount8"], row["ExchangeDenomination8"], row["ExchangeCount8"], remark);
                SetExchangTable(salesNO, journalTable, row["ExchangeDate9"], row["ExchangeName9"], row["ExchangeAmount9"], row["ExchangeDenomination9"], row["ExchangeCount9"], remark);
                SetExchangTable(salesNO, journalTable, row["ExchangeDate10"], row["ExchangeName10"], row["ExchangeAmount10"], row["ExchangeDenomination10"], row["ExchangeCount10"], remark);
                #endregion // 両替データ

                #region 釣銭準備
                remark = Convert.ToString(row["ChangePreparationRemark"]);      // 釣銭準備備考

                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate1"], "現金", row["ChangePreparationAmount1"], remark);
                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate2"], row["ChangePreparationName2"], row["ChangePreparationAmount2"], remark);
                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate3"], row["ChangePreparationName3"], row["ChangePreparationAmount3"], remark);
                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate4"], row["ChangePreparationName4"], row["ChangePreparationAmount4"], remark);
                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate5"], row["ChangePreparationName5"], row["ChangePreparationAmount5"], remark);
                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate6"], row["ChangePreparationName6"], row["ChangePreparationAmount6"], remark);
                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate7"], row["ChangePreparationName7"], row["ChangePreparationAmount7"], remark);
                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate8"], row["ChangePreparationName8"], row["ChangePreparationAmount8"], remark);
                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate9"], row["ChangePreparationName9"], row["ChangePreparationAmount9"], remark);
                SetChangePreparationTable(salesNO, journalTable, row["ChangePreparationDate10"], row["ChangePreparationName10"], row["ChangePreparationAmount10"], remark);
                #endregion // 釣銭準備

                #region 精算処理 現金残高
                if (storeDataSet.CashBalanceTable.Where(s => s.SalesNO == salesNO).FirstOrDefault() == null)
                {
                    var cashBalance = storeDataSet.CashBalanceTable.NewCashBalanceTableRow();
                    cashBalance.StoreReceiptPrint = storeReceiptPrint;                              // 店舗レシート表記
                    cashBalance.StaffReceiptPrint = staffReceiptPrint;                              // 担当レシート表記
                    cashBalance.SalesNO = salesNO;                                                  // 売上番号
                    cashBalance.RegistDate = ConvertDateTime(row["CashBalanceRegistDate"], true);   // 登録日
                    cashBalance.Num10000yen = ConvertDecimal(row["10000yenNum"]);                   // 現金残高10,000枚数
                    cashBalance.Num5000yen = ConvertDecimal(row["5000yenNum"]);                     // 現金残高5,000枚数
                    cashBalance.Num2000yen = ConvertDecimal(row["2000yenNum"]);                     // 現金残高2,000枚数
                    cashBalance.Num1000yen = ConvertDecimal(row["1000yenNum"]);                     // 現金残高1,000枚数
                    cashBalance.Num500yen = ConvertDecimal(row["500yenNum"]);                       // 現金残高500枚数
                    cashBalance.Num100yen = ConvertDecimal(row["100yenNum"]);                       // 現金残高100枚数
                    cashBalance.Num50yen = ConvertDecimal(row["50yenNum"]);                         // 現金残高50枚数
                    cashBalance.Num10yen = ConvertDecimal(row["10yenNum"]);                         // 現金残高10枚数
                    cashBalance.Num5yen = ConvertDecimal(row["5yenNum"]);                           // 現金残高5枚数
                    cashBalance.Num1yen = ConvertDecimal(row["1yenNum"]);                           // 現金残高1枚数
                    cashBalance.Gaku10000yen = ConvertDecimal(row["10000yenGaku"]);                 // 現金残高10,000金額
                    cashBalance.Gaku5000yen = ConvertDecimal(row["5000yenGaku"]);                   // 現金残高5,000金額
                    cashBalance.Gaku2000yen = ConvertDecimal(row["2000yenGaku"]);                   // 現金残高2,000金額
                    cashBalance.Gaku1000yen = ConvertDecimal(row["1000yenGaku"]);                   // 現金残高1,000金額
                    cashBalance.Gaku500yen = ConvertDecimal(row["500yenGaku"]);                     // 現金残高500金額
                    cashBalance.Gaku100yen = ConvertDecimal(row["100yenGaku"]);                     // 現金残高100金額
                    cashBalance.Gaku50yen = ConvertDecimal(row["50yenGaku"]);                       // 現金残高50金額
                    cashBalance.Gaku10yen = ConvertDecimal(row["10yenGaku"]);                       // 現金残高10金額
                    cashBalance.Gaku5yen = ConvertDecimal(row["5yenGaku"]);                         // 現金残高5金額
                    cashBalance.Gaku1yen = ConvertDecimal(row["1yenGaku"]);                         // 現金残高1金額
                    cashBalance.Etcyen = ConvertDecimal(row["Etcyen"]);                             // その他金額
                    cashBalance.Change = ConvertDecimal(row["Change"]);                             // 釣銭準備金
                    cashBalance.TotalGaku = ConvertDecimal(row["DepositGaku"]);                     // 現金残高 現金売上(+)
                    cashBalance.CashDeposit = ConvertDecimal(row["CashDeposit"]);                   // 現金残高 現金入金(+)
                    cashBalance.CashPayment = ConvertDecimal(row["CashPayment"]);                   // 現金残高 現金支払(-)
                    cashBalance.CashBalance = ConvertDecimal(row["CashBalance"]);                   // 現金残高 その他金額～現金残高現金支払(-)までの合計
                    cashBalance.ComputerTotal = ConvertDecimal(row["ComputerTotal"]);               // ｺﾝﾋﾟｭｰﾀ計 現金残高 10,000金額～現金残高1金額までの合計
                    cashBalance.CashShortage = ConvertDecimal(row["CashShortage"]);                 // 現金残高 現金過不足
                    //
                    storeDataSet.CashBalanceTable.Rows.Add(cashBalance);

                    #region 精算処理 総売
                    if (storeDataSet.TotalSalesTable.Where(s => s.SalesNO == cashBalance.SalesNO).FirstOrDefault() == null)
                    {
                        var totalSales = storeDataSet.TotalSalesTable.NewTotalSalesTableRow();
                        totalSales.StaffReceiptPrint = staffReceiptPrint;                               // 担当レシート表記
                        totalSales.RegistDate = cashBalance.RegistDate;                                 // 登録日
                        totalSales.SalesNO = salesNO;                                                   // 売上番号
                        totalSales.SalesNOCount = ConvertDecimal(row["SalesNOCount"]);                  // 総売 伝票数
                        totalSales.CustomerCDCount = ConvertDecimal(row["CustomerCDCount"]);            // 総売 客数(人)
                        totalSales.SalesSUSum = ConvertDecimal(row["SalesSUSum"]);                      // 総売 売上数量
                        totalSales.TotalGakuSum = ConvertDecimal(row["TotalGakuSum"]);                  // 総売 売上金額
                        //
                        storeDataSet.TotalSalesTable.Rows.Add(totalSales);
                    }
                    #endregion // 精算処理 総売

                    #region 精算処理 取引別
                    if (storeDataSet.ByTransactionTable.Where(s => s.SalesNO == cashBalance.SalesNO).FirstOrDefault() == null)
                    {
                        var byTransaction = storeDataSet.ByTransactionTable.NewByTransactionTableRow();
                        byTransaction.StaffReceiptPrint = staffReceiptPrint;                                    // 担当レシート表記
                        byTransaction.RegistDate = cashBalance.RegistDate;                                      // 登録日
                        byTransaction.SalesNO = salesNO;                                                        // 売上番号
                        byTransaction.ForeignTaxableAmount = ConvertDecimal(row["ForeignTaxableAmount"]);       // 取引別 外税対象額
                        byTransaction.TaxableAmount = ConvertDecimal(row["TaxableAmount"]);                     // 取引別 内税対象額
                        byTransaction.TaxExemptionAmount = ConvertDecimal(row["TaxExemptionAmount"]);           // 取引別 非課税対象額
                        byTransaction.TotalWithoutTax = ConvertDecimal(row["TotalWithoutTax"]);                 // 取引別 税抜合計
                        byTransaction.Tax = ConvertDecimal(row["Tax"]);                                         // 取引別 内税
                        byTransaction.OutsideTax = ConvertDecimal(row["OutsideTax"]);                           // 取引別 外税
                        byTransaction.ConsumptionTax = ConvertDecimal(row["ConsumptionTax"]);                   // 取引別 消費税計
                        byTransaction.ForeignTaxableAmount = ConvertDecimal(row["ForeignTaxableAmount"]);       // 取引別 外税対象額
                        byTransaction.TaxIncludedTotal = ConvertDecimal(row["TaxIncludedTotal"]);               // 取引別 税込合計
                        byTransaction.DiscountGaku = ConvertDecimal(row["DiscountGaku"]);                       // 取引別 値引額
                        //
                        storeDataSet.ByTransactionTable.Rows.Add(byTransaction);
                    }
                    #endregion // 精算処理 取引別

                    #region 精算処理 決済別
                    if (storeDataSet.BySettlementTable.Where(s => s.SalesNO == cashBalance.SalesNO).FirstOrDefault() == null)
                    {
                        var bySettlement = storeDataSet.BySettlementTable.NewBySettlementTableRow();
                        bySettlement.StaffReceiptPrint = staffReceiptPrint;                             // 担当レシート表記
                        bySettlement.RegistDate = cashBalance.RegistDate;                               // 登録日
                        bySettlement.SalesNO = salesNO;                                                 // 売上番号
                        bySettlement.DenominationName1 = Convert.ToString(row["DenominationName1"]);    // 決済別 金種区分名1
                        bySettlement.Kingaku1 = ConvertDecimal(row["Kingaku1"]);                        // 決済別 金額1
                        bySettlement.DenominationName2 = Convert.ToString(row["DenominationName2"]);    // 決済別 金種区分名2
                        bySettlement.Kingaku2 = ConvertDecimal(row["Kingaku2"]);                        // 決済別 金額2
                        bySettlement.DenominationName3 = Convert.ToString(row["DenominationName3"]);    // 決済別 金種区分名3
                        bySettlement.Kingaku3 = ConvertDecimal(row["Kingaku3"]);                        // 決済別 金額3
                        bySettlement.DenominationName4 = Convert.ToString(row["DenominationName4"]);    // 決済別 金種区分名4
                        bySettlement.Kingaku4 = ConvertDecimal(row["Kingaku4"]);                        // 決済別 金額4
                        bySettlement.DenominationName5 = Convert.ToString(row["DenominationName5"]);    // 決済別 金種区分名5
                        bySettlement.Kingaku5 = ConvertDecimal(row["Kingaku5"]);                        // 決済別 金額5
                        bySettlement.DenominationName6 = Convert.ToString(row["DenominationName6"]);    // 決済別 金種区分名6
                        bySettlement.Kingaku6 = ConvertDecimal(row["Kingaku6"]);                        // 決済別 金額6
                        bySettlement.DenominationName7 = Convert.ToString(row["DenominationName7"]);    // 決済別 金種区分名7
                        bySettlement.Kingaku7 = ConvertDecimal(row["Kingaku7"]);                        // 決済別 金額7
                        bySettlement.DenominationName8 = Convert.ToString(row["DenominationName8"]);    // 決済別 金種区分名8
                        bySettlement.Kingaku8 = ConvertDecimal(row["Kingaku8"]);                        // 決済別 金額8
                        bySettlement.DenominationName9 = Convert.ToString(row["DenominationName9"]);    // 決済別 金種区分名9
                        bySettlement.Kingaku9 = ConvertDecimal(row["Kingaku9"]);                        // 決済別 金額9
                        bySettlement.DenominationName10 = Convert.ToString(row["DenominationName10"]);  // 決済別 金種区分名10
                        bySettlement.Kingaku10 = ConvertDecimal(row["Kingaku10"]);                      // 決済別 金額10
                        bySettlement.DenominationName11 = Convert.ToString(row["DenominationName11"]);  // 決済別 金種区分名11
                        bySettlement.Kingaku11 = ConvertDecimal(row["Kingaku11"]);                      // 決済別 金額11
                        bySettlement.DenominationName12 = Convert.ToString(row["DenominationName12"]);  // 決済別 金種区分名12
                        bySettlement.Kingaku12 = ConvertDecimal(row["Kingaku12"]);                      // 決済別 金額12
                        bySettlement.DenominationName13 = Convert.ToString(row["DenominationName13"]);  // 決済別 金種区分名13
                        bySettlement.Kingaku13 = ConvertDecimal(row["Kingaku13"]);                      // 決済別 金額13
                        bySettlement.DenominationName14 = Convert.ToString(row["DenominationName14"]);  // 決済別 金種区分名14
                        bySettlement.Kingaku14 = ConvertDecimal(row["Kingaku14"]);                      // 決済別 金額14
                        bySettlement.DenominationName15 = Convert.ToString(row["DenominationName15"]);  // 決済別 金種区分名15
                        bySettlement.Kingaku15 = ConvertDecimal(row["Kingaku15"]);                      // 決済別 金額15
                        bySettlement.DenominationName16 = Convert.ToString(row["DenominationName16"]);  // 決済別 金種区分名16
                        bySettlement.Kingaku16 = ConvertDecimal(row["Kingaku16"]);                      // 決済別 金額16
                        bySettlement.DenominationName17 = Convert.ToString(row["DenominationName17"]);  // 決済別 金種区分名17
                        bySettlement.Kingaku17 = ConvertDecimal(row["Kingaku17"]);                      // 決済別 金額17
                        bySettlement.DenominationName18 = Convert.ToString(row["DenominationName18"]);  // 決済別 金種区分名18
                        bySettlement.Kingaku18 = ConvertDecimal(row["Kingaku18"]);                      // 決済別 金額18
                        bySettlement.DenominationName19 = Convert.ToString(row["DenominationName19"]);  // 決済別 金種区分名19
                        bySettlement.Kingaku19 = ConvertDecimal(row["Kingaku19"]);                      // 決済別 金額19
                        bySettlement.DenominationName20 = Convert.ToString(row["DenominationName20"]);  // 決済別 金種区分名20
                        bySettlement.Kingaku20 = ConvertDecimal(row["Kingaku20"]);                      // 決済別 金額20
                        //
                        storeDataSet.BySettlementTable.Rows.Add(bySettlement);
                    }
                    #endregion // 精算処理 決済別

                    #region 精算処理 入金計
                    if (storeDataSet.DepositTotalTable.Where(s => s.SalesNO == cashBalance.SalesNO).FirstOrDefault() == null)
                    {
                        var depositTotal = storeDataSet.DepositTotalTable.NewDepositTotalTableRow();
                        depositTotal.StaffReceiptPrint = staffReceiptPrint;                             // 担当レシート表記
                        depositTotal.RegistDate = cashBalance.RegistDate;                               // 登録日
                        depositTotal.SalesNO = salesNO;                                                 // 売上番号
                        depositTotal.Transfer = ConvertDecimal(row["DepositTransfer"]);                 // 入金計 振込
                        depositTotal.Cash = ConvertDecimal(row["DepositCash"]);                         // 入金計 現金
                        depositTotal.Check = ConvertDecimal(row["DepositCheck"]);                       // 入金計 小切手
                        depositTotal.Bill = ConvertDecimal(row["DepositBill"]);                         // 入金計 手形
                        depositTotal.Offset = ConvertDecimal(row["DepositOffset"]);                     // 入金計 相殺
                        depositTotal.Adjustment = ConvertDecimal(row["DepositAdjustment"]);             // 入金計 調整
                        //
                        storeDataSet.DepositTotalTable.Rows.Add(depositTotal);
                    }
                    #endregion // 精算処理 入金計

                    #region 精算処理 支払計
                    if (storeDataSet.PaymentTotalTable.Where(s => s.SalesNO == cashBalance.SalesNO).FirstOrDefault() == null)
                    {
                        var paymentTotal = storeDataSet.PaymentTotalTable.NewPaymentTotalTableRow();
                        paymentTotal.StaffReceiptPrint = staffReceiptPrint;                             // 担当レシート表記
                        paymentTotal.RegistDate = cashBalance.RegistDate;                               // 登録日
                        paymentTotal.SalesNO = salesNO;                                                 // 売上番号
                        paymentTotal.Transfer = ConvertDecimal(row["PaymentTransfer"]);                 // 支払計 振込
                        paymentTotal.Cash = ConvertDecimal(row["PaymentCash"]);                         // 支払計 現金
                        paymentTotal.Check = ConvertDecimal(row["PaymentCheck"]);                       // 支払計 小切手
                        paymentTotal.Bill = ConvertDecimal(row["PaymentBill"]);                         // 支払計 手形
                        paymentTotal.Offset = ConvertDecimal(row["PaymentOffset"]);                     // 支払計 振込
                        paymentTotal.Adjustment = ConvertDecimal(row["PaymentAdjustment"]);             // 支払計 相殺
                        //
                        storeDataSet.PaymentTotalTable.Rows.Add(paymentTotal);
                    }
                    #endregion // 精算処理 支払計

                    #region 精算処理 他金額
                    if (storeDataSet.OtherAmountTable.Where(s => s.SalesNO == cashBalance.SalesNO).FirstOrDefault() == null)
                    {
                        var otherAmount = storeDataSet.OtherAmountTable.NewOtherAmountTableRow();
                        otherAmount.StaffReceiptPrint = staffReceiptPrint;                              // 担当レシート表記
                        otherAmount.RegistDate = cashBalance.RegistDate;                                // 登録日
                        otherAmount.SalesNO = salesNO;                                                  // 売上番号

                        // 他金額 返品
                        var value = ConvertDecimal(row["OtherAmountReturns"]);
                        otherAmount.Returns = string.IsNullOrWhiteSpace(value) ? "0" : value;

                        // 他金額 値引
                        value = ConvertDecimal(row["OtherAmountDiscount"]);
                        otherAmount.Discount = string.IsNullOrWhiteSpace(value) ? "0" : value;

                        // 他金額 取消
                        value = ConvertDecimal(row["OtherAmountCancel"]);
                        otherAmount.Cancel = string.IsNullOrWhiteSpace(value) ? "0" : value;

                        // 他金額 配達
                        value = ConvertDecimal(row["OtherAmountDelivery"]);
                        otherAmount.Delivery = string.IsNullOrWhiteSpace(value) ? "0" : value;

                        // 両替回数
                        value = ConvertDecimal(row["ExchangeCount"]);
                        otherAmount.ExchangeCount = string.IsNullOrWhiteSpace(value) ? "0" : value;
                        
                        //
                        storeDataSet.OtherAmountTable.Rows.Add(otherAmount);
                    }
                    #endregion // 精算処理 他金額

                    #region 精算処理 時間帯別(税込)
                    if (storeDataSet.ByTimeZoneTaxIncludedTable.Where(s => s.SalesNO == cashBalance.SalesNO).FirstOrDefault() == null)
                    {
                        var byTimeZoneTaxIncluded = storeDataSet.ByTimeZoneTaxIncludedTable.NewByTimeZoneTaxIncludedTableRow();
                        byTimeZoneTaxIncluded.StaffReceiptPrint = staffReceiptPrint;                                        // 担当レシート表記
                        byTimeZoneTaxIncluded.RegistDate = cashBalance.RegistDate;                                          // 登録日
                        byTimeZoneTaxIncluded.SalesNO = salesNO;                                                            // 売上番号
                        byTimeZoneTaxIncluded.From0000to0100 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0000_0100"]);      // 時間帯別(税込) 00:00～01:00
                        byTimeZoneTaxIncluded.From0100to0200 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0100_0200"]);      // 時間帯別(税込) 01:00～02:00
                        byTimeZoneTaxIncluded.From0200to0300 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0200_0300"]);      // 時間帯別(税込) 02:00～03:00
                        byTimeZoneTaxIncluded.From0300to0400 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0300_0400"]);      // 時間帯別(税込) 03:00～04:00
                        byTimeZoneTaxIncluded.From0400to0500 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0400_0500"]);      // 時間帯別(税込) 04:00～05:00
                        byTimeZoneTaxIncluded.From0500to0600 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0500_0600"]);      // 時間帯別(税込) 05:00～06:00
                        byTimeZoneTaxIncluded.From0600to0700 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0600_0700"]);      // 時間帯別(税込) 06:00～07:00
                        byTimeZoneTaxIncluded.From0700to0800 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0700_0800"]);      // 時間帯別(税込) 07:00～08:00
                        byTimeZoneTaxIncluded.From0800to0900 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0800_0900"]);      // 時間帯別(税込) 08:00～09:00
                        byTimeZoneTaxIncluded.From0900to1000 = ConvertDecimal(row["ByTimeZoneTaxIncluded_0900_1000"]);      // 時間帯別(税込) 09:00～10:00
                        byTimeZoneTaxIncluded.From1000to1100 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1000_1100"]);      // 時間帯別(税込) 10:00～11:00
                        byTimeZoneTaxIncluded.From1100to1200 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1100_1200"]);      // 時間帯別(税込) 11:00～12:00
                        byTimeZoneTaxIncluded.From1200to1300 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1200_1300"]);      // 時間帯別(税込) 12:00～13:00
                        byTimeZoneTaxIncluded.From1300to1400 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1300_1400"]);      // 時間帯別(税込) 13:00～14:00
                        byTimeZoneTaxIncluded.From1400to1500 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1400_1500"]);      // 時間帯別(税込) 14:00～15:00
                        byTimeZoneTaxIncluded.From1500to1600 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1500_1600"]);      // 時間帯別(税込) 15:00～16:00
                        byTimeZoneTaxIncluded.From1600to1700 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1600_1700"]);      // 時間帯別(税込) 16:00～17:00
                        byTimeZoneTaxIncluded.From1700to1800 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1700_1800"]);      // 時間帯別(税込) 17:00～18:00
                        byTimeZoneTaxIncluded.From1800to1900 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1800_1900"]);      // 時間帯別(税込) 18:00～19:00
                        byTimeZoneTaxIncluded.From1900to2000 = ConvertDecimal(row["ByTimeZoneTaxIncluded_1900_2000"]);      // 時間帯別(税込) 19:00～20:00
                        byTimeZoneTaxIncluded.From2000to2100 = ConvertDecimal(row["ByTimeZoneTaxIncluded_2000_2100"]);      // 時間帯別(税込) 20:00～21:00
                        byTimeZoneTaxIncluded.From2100to2200 = ConvertDecimal(row["ByTimeZoneTaxIncluded_2100_2200"]);      // 時間帯別(税込) 21:00～22:00
                        byTimeZoneTaxIncluded.From2200to2300 = ConvertDecimal(row["ByTimeZoneTaxIncluded_2200_2300"]);      // 時間帯別(税込) 22:00～23:00
                        byTimeZoneTaxIncluded.From2300to2400 = ConvertDecimal(row["ByTimeZoneTaxIncluded_2300_2400"]);      // 時間帯別(税込) 23:00～24:00
                        //
                        storeDataSet.ByTimeZoneTaxIncludedTable.Rows.Add(byTimeZoneTaxIncluded);
                    }
                    #endregion // 精算処理 時間帯別(税込)

                    #region 精算処理 時間帯別件数
                    if (storeDataSet.ByTimeZoneSalesTable.Where(s => s.SalesNO == cashBalance.SalesNO).FirstOrDefault() == null)
                    {
                        var byTimeZoneSales = storeDataSet.ByTimeZoneSalesTable.NewByTimeZoneSalesTableRow();
                        byTimeZoneSales.StaffReceiptPrint = staffReceiptPrint;                                              // 担当レシート表記
                        byTimeZoneSales.RegistDate = cashBalance.RegistDate;                                                // 登録日
                        byTimeZoneSales.SalesNO = salesNO;                                                                  // 売上番号
                        byTimeZoneSales.From0000to0100 = ConvertDecimal(row["ByTimeZoneSalesNO_0000_0100"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From0100to0200 = ConvertDecimal(row["ByTimeZoneSalesNO_0100_0200"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From0200to0300 = ConvertDecimal(row["ByTimeZoneSalesNO_0200_0300"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From0300to0400 = ConvertDecimal(row["ByTimeZoneSalesNO_0300_0400"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From0400to0500 = ConvertDecimal(row["ByTimeZoneSalesNO_0400_0500"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From0500to0600 = ConvertDecimal(row["ByTimeZoneSalesNO_0500_0600"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From0600to0700 = ConvertDecimal(row["ByTimeZoneSalesNO_0600_0700"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From0700to0800 = ConvertDecimal(row["ByTimeZoneSalesNO_0700_0800"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From0800to0900 = ConvertDecimal(row["ByTimeZoneSalesNO_0800_0900"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From0900to1000 = ConvertDecimal(row["ByTimeZoneSalesNO_0900_1000"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1000to1100 = ConvertDecimal(row["ByTimeZoneSalesNO_1000_1100"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1100to1200 = ConvertDecimal(row["ByTimeZoneSalesNO_1100_1200"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1200to1300 = ConvertDecimal(row["ByTimeZoneSalesNO_1200_1300"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1300to1400 = ConvertDecimal(row["ByTimeZoneSalesNO_1300_1400"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1400to1500 = ConvertDecimal(row["ByTimeZoneSalesNO_1400_1500"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1500to1600 = ConvertDecimal(row["ByTimeZoneSalesNO_1500_1600"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1600to1700 = ConvertDecimal(row["ByTimeZoneSalesNO_1600_1700"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1700to1800 = ConvertDecimal(row["ByTimeZoneSalesNO_1700_1800"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1800to1900 = ConvertDecimal(row["ByTimeZoneSalesNO_1800_1900"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From1900to2000 = ConvertDecimal(row["ByTimeZoneSalesNO_1900_2000"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From2000to2100 = ConvertDecimal(row["ByTimeZoneSalesNO_2000_2100"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From2100to2200 = ConvertDecimal(row["ByTimeZoneSalesNO_2100_2200"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From2200to2300 = ConvertDecimal(row["ByTimeZoneSalesNO_2200_2300"]);                // 時間帯別件数 23:00～24:00
                        byTimeZoneSales.From2300to2400 = ConvertDecimal(row["ByTimeZoneSalesNO_2300_2400"]);                // 時間帯別件数 23:00～24:00
                        //
                        storeDataSet.ByTimeZoneSalesTable.Rows.Add(byTimeZoneSales);
                    }
                    #endregion // 精算処理 時間帯別件数
                }
                #endregion // 精算処理 現金残高
            }

            storeDataSet.JournalTable.Rows.Clear();
            foreach (var row in journalTable.OrderBy(r => Convert.ToDateTime(r.DateTime)).ThenBy(r => r.DataKind))
            {
                storeDataSet.JournalTable.ImportRow(row);
            }

            #region 出力設定

            if (storeDataSet.StoreTable.Rows.Count > 0)
            {
                // 各明細部印刷有無を印刷フラグで設定
                storeDataSet.StoreTable[0].DispSales = isPrint;                 // 販売
                storeDataSet.StoreTable[0].DispJournalDetail = isPrint;         // ジャーナル詳細

                if (isPrint)
                {
                    // 印刷するフラグON時、各明細部の出力件数が0件の場合は印刷フラグOFF

                    // 販売
                    if (storeDataSet.StoreTable.Count == 0)
                    {
                        storeDataSet.StoreTable[0].DispSales = false;
                    }

                    // ジャーナル詳細
                    if (storeDataSet.JournalTable.Count == 0)
                    {
                        storeDataSet.StoreTable[0].DispJournalDetail = false;
                    }
                }
            }

            #endregion // 出力設定

            return storeDataSet;
        }

        #region 雑入金データ
        /// <summary>
        /// 雑入金データをデータセットに設定
        /// </summary>
        /// <param name="salesNO">売上番号</param>
        /// <param name="jornalTable">ジャーナル詳細データテーブル</param>
        /// <param name="date">雑入金日</param>
        /// <param name="name">雑入金名</param>
        /// <param name="amount">雑入金額</param>
        /// <param name="remark">雑入金備考</param>
        private void SetMiscDepositTable(string salesNO, StoreDataSet.JournalTableDataTable journalTable, object date, object name, object amount, string remark)
        {
            if (date != DBNull.Value)
            {
                var dateTime = Convert.ToString(date);

                var item = journalTable.NewJournalTableRow();
                item.SalseNO = salesNO;
                item.DataKind = (short)JournalDataKind.MiscDeposit;
                item.DispDateTime = dateTime.Substring(0, dateTime.LastIndexOf(":"));
                item.DateTime = dateTime;
                item.Name = Convert.ToString(name);
                item.Amount = ConvertDecimal(amount);
                item.Remark = remark;
                item.Representative = true;

                if(journalTable.AsEnumerable().Where(
                        j => j.SalseNO == item.SalseNO &&
                             j.DataKind == item.DataKind &&
                             j.DispDateTime == item.DispDateTime &&
                             j.DateTime == item.DateTime &&
                             j.Name == item.Name && 
                             j.Amount == item.Amount && 
                             j.Remark == item.Remark &&
                             j.Representative == item.Representative).SingleOrDefault() == null)
                {
                    journalTable.Rows.Add(item);
                }
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
        private void SetDepositTable(string salesNO, StoreDataSet.JournalTableDataTable journalTable, object date, string customerCD, string customerName, object name, object amount, string remark)
        {
            if (date != DBNull.Value)
            {
                var dateTime = Convert.ToString(date);

                var item = journalTable.NewJournalTableRow();
                item.SalseNO = salesNO;
                item.DataKind = (short)JournalDataKind.Deposit;
                item.DispDateTime = dateTime.Substring(0, dateTime.LastIndexOf(":"));
                item.DateTime = dateTime;
                item.CustomerCD = customerCD;
                item.CustomerName = customerName;
                item.Name = Convert.ToString(name);
                item.Amount = ConvertDecimal(amount);
                item.Remark = remark;
                item.Representative = true;

                if (journalTable.AsEnumerable().Where(
                        j => j.SalseNO == item.SalseNO &&
                             j.DataKind == item.DataKind &&
                             j.DispDateTime == item.DispDateTime &&
                             j.DateTime == item.DateTime &&
                             j.CustomerCD ==item.CustomerCD &&
                             j.CustomerName == item.CustomerName &&
                             j.Name == item.Name &&
                             j.Amount == item.Amount &&
                             j.Remark == item.Remark &&
                             j.Representative == item.Representative).SingleOrDefault() == null)
                {
                    journalTable.Rows.Add(item);
                }
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
        private void SetMiscPaymentTable(string salesNO, StoreDataSet.JournalTableDataTable journalTable, object date, object name, object amount, string remark)
        {
            if (date != DBNull.Value)
            {
                var dateTime = Convert.ToString(date);

                var item = journalTable.NewJournalTableRow();
                item.SalseNO = salesNO;
                item.DataKind = (short)JournalDataKind.MiscPayment;
                item.DispDateTime = dateTime.Substring(0, dateTime.LastIndexOf(":"));
                item.DateTime = dateTime;
                item.Name = Convert.ToString(name);
                item.Amount = ConvertDecimal(amount);
                item.Remark = remark;
                item.Representative = true;

                if (journalTable.AsEnumerable().Where(
                        j => j.SalseNO == item.SalseNO && 
                             j.DataKind == item.DataKind &&
                             j.DispDateTime == item.DispDateTime &&
                             j.DateTime == item.DateTime &&
                             j.Name == item.Name &&
                             j.Amount == item.Amount &&
                             j.Remark == item.Remark &&
                             j.Representative == item.Representative).SingleOrDefault() == null)
                {
                    journalTable.Rows.Add(item);
                }
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
        private void SetExchangTable(string salesNO, StoreDataSet.JournalTableDataTable journalTable, object date, object name, object amount, object denomination, object exchangeCount, string remark)
        {
            if (date != DBNull.Value)
            {
                var dateTime = Convert.ToString(date);

                var item = journalTable.NewJournalTableRow();
                item.SalseNO = salesNO;
                item.DataKind = (short)JournalDataKind.Exchange;
                item.DispDateTime = dateTime.Substring(0, dateTime.LastIndexOf(":"));
                item.DateTime = dateTime;
                item.Name = Convert.ToString(name);
                item.Amount = ConvertDecimal(amount);
                item.Denomination = Convert.ToString(denomination);
                item.ExchangeCount = ConvertDecimal(exchangeCount);
                item.Remark = remark;
                item.Representative = true;

                if (journalTable.AsEnumerable().Where(
                        j => j.SalseNO == item.SalseNO && 
                             j.DataKind == item.DataKind &&
                             j.DispDateTime == item.DispDateTime &&
                             j.DateTime == item.DateTime &&
                             j.Name == item.Name &&
                             j.Amount == item.Amount &&
                             j.Denomination == item.Denomination &&
                             j.ExchangeCount == item.ExchangeCount &&
                             j.Remark == item.Remark &&
                             j.Representative == item.Representative).SingleOrDefault() == null)
                {
                    journalTable.Rows.Add(item);
                }
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
        private void SetChangePreparationTable(string salesNO, StoreDataSet.JournalTableDataTable journalTable, object date, object name, object amount, string remark)
        {
            if (date != DBNull.Value)
            {
                var dateTime = Convert.ToString(date);

                var item = journalTable.NewJournalTableRow();
                item.SalseNO = salesNO;
                item.DataKind = (short)JournalDataKind.ChangePreparation;
                item.DispDateTime = dateTime.Substring(0, dateTime.LastIndexOf(":"));
                item.DateTime = dateTime;
                item.Name = Convert.ToString(name);
                item.Amount = ConvertDecimal(amount);
                item.Remark = remark;
                item.Representative = true;

                if (journalTable.AsEnumerable().Where(
                        j => j.SalseNO == item.SalseNO && 
                             j.DataKind == item.DataKind &&
                             j.DispDateTime == item.DispDateTime &&
                             j.DateTime == item.DateTime &&
                             j.Name == item.Name &&
                             j.Amount == item.Amount &&
                             j.Remark == item.Remark &&
                             j.Representative == item.Representative).SingleOrDefault() == null)
                {
                    journalTable.Rows.Add(item);
                }
            }
        }
        #endregion // 釣銭準備データ

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
