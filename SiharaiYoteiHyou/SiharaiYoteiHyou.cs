using Base.Client;
using BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using ElencySolutions.CsvHelper;
using System.Diagnostics;
using CrystalDecisions.Shared;

namespace SiharaiYoteiHyou
{
    public partial class SiharaiYoteiHyou : FrmMainForm
    {
        private const string ProNm = "支払予定表";
        //string StoreAuthen_CD = "";
        //string StoreAuthen_ChangeDate = "";
        //string StoreCD = "";
        //string ClosedStatus = "";
        //string PaymentStatus = "";
        //string expense = "";
        //string purchase = "";
        //string ymd;
        bool combo_focus;
        Base_BL bbl = new Base_BL();
        SiharaiYoteiHyou_BL shyhbl;
        D_PayPlan_Entity dppe;
        private string StartUpKBN = "";
        DataTable dtCSV,dtResult;
        public SiharaiYoteiHyou()
        {
            InitializeComponent();
            dtCSV = new DataTable();
            dtResult = new DataTable();
        }

        private void SiharaiYoteiHyou_Load(object sender, EventArgs e)
        {
            shyhbl = new SiharaiYoteiHyou_BL();
            InProgramID = "SiharaiYoteiHyou";
            SetFunctionLabel(EProMode.PRINT);//PrintSec 
            StartProgram();
            base.Btn_F10.Text = "CSV(F10)";
            base.Btn_F11.Text = "PDF(F11)";
            base.InProgramNM = ProNm;
            BindCombo();
            RequiredField();
        }

        private void RequiredField()
        {
            RdoCloseStsSumi.Checked = true;
            RdoUnpaid.Checked = true;
            chkExpense.Checked = true;
            chkPurchase.Checked = true;
            //scPaymentDestinaion.TxtCode.Require(true);
            comboStore.Require(true);
        }

        /// <summary>
        /// 店舗データを取得してコンボボックスに表示する処理
        /// </summary>
        public void BindCombo()
        {
            comboStore.Bind(string.Empty, "2");
            comboStore.SelectedValue = StoreCD;
        }
        /// <summary>
        /// エラーチェック処理
        /// </summary>
        private bool ErrorCheck()
        {
            /// <remarks>支払予定日(from)は支払予定日(To)より大きいの場合、エラーになる処理</remarks>
            if (!string.IsNullOrWhiteSpace(txtPaymentDueDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPaymentDueDateTo.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtPaymentDueDateFrom.Text);
                DateTime dt2 = Convert.ToDateTime(txtPaymentDueDateTo.Text);

                if (dt1 > dt2)
                {
                    shyhbl.ShowMessage("E104");
                    txtPaymentDueDateTo.Focus();
                    return false;
                }

            }

            if (!string.IsNullOrEmpty(scPaymentDestinaion.TxtCode.Text))
            {
                if (!scPaymentDestinaion.IsExists(2))
                {
                    shyhbl.ShowMessage("E101");
                    scPaymentDestinaion.SetFocus(1);
                    return false;
                }
            }
           
            /// <remarks>両方チェックが入っていない場合、エラーになる処理</remarks>
            if (chkExpense.Checked == false && chkPurchase.Checked == false)
            {
                shyhbl.ShowMessage("E111");
                chkPurchase.Focus();
                return false;
            }

            if (!RequireCheck(new Control[] { comboStore }))   // go that focus
                return false;

            /// <remarks>店舗名を選択した場合、権限があるかとかをチェックする処理</remarks>
            if (!base.CheckAvailableStores(comboStore.SelectedValue.ToString()))
            {
                shyhbl.ShowMessage("E139");
                comboStore.Focus();
                combo_focus = true;
                return false;
            }
            return true;
        }
        /// <summary>
        /// F1からF12までボタンをクリックする場合管理する処理
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int index)

        {
            base.FunctionProcess(index);
            switch (index + 1)
            {

                case 6: //F6:キャンセル		
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                        ClearDetail();
                    break;
                //case 10:
                //             F10();
                //    break;

                case 11://Excel出力
                    break;
                    //case 12://印刷
                    //    break;
            }

        }
        /// <summary>
        ///画面クリア処理
        /// </summary>
        private void ClearDetail()
        {
            txtPaymentDueDateFrom.Text = string.Empty;
            txtPaymentDueDateTo.Text = string.Empty;
            scPaymentDestinaion.Clear();
            comboStore.Text = string.Empty;
            RdoCloseStsSumi.Checked = true;
            radioClosedStatusAll.Checked = false;
            RdoUnpaid.Checked = true;
            radioPaymentStatusAll.Checked = false;
            chkPurchase.Checked = true;
            chkExpense.Checked = true;
            txtPaymentDueDateFrom.Focus();
            comboStore.SelectedValue = StoreCD;
        }
        /// <summary>
        /// アプリケーションを終了処理
        /// </summary>
        protected override void EndSec()
        {
            this.Close();
        }
        /// <summary>
        /// 支払先検索にEenterする場合、選択出来ないかとかをチェックする処理///
        /// </summary>
        private void scPaymentDestinaion_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scPaymentDestinaion.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scPaymentDestinaion.TxtCode.Text))
                {
                    if (scPaymentDestinaion.SelectData())
                    {
                        scPaymentDestinaion.Value1 = scPaymentDestinaion.TxtCode.Text;
                        scPaymentDestinaion.Value2 = scPaymentDestinaion.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        scPaymentDestinaion.SetFocus(1);
                    }
                }

            }
        }
        private D_PayPlan_Entity GetSearchInfo()
        {
            dppe = new D_PayPlan_Entity
            {
                PayeeCD = scPaymentDestinaion.TxtCode.Text,
                PaymentDueDateFrom = txtPaymentDueDateFrom.Text,
                PaymenetDueDateTo = txtPaymentDueDateTo.Text,
                CloseStatusSumi = RdoCloseStsSumi.Checked ? "1" : "0",
                PaymentStatusUnpaid = RdoUnpaid.Checked ? "1" : "0",
                Purchase = chkPurchase.Checked ? "1" : "0",
                Expense = chkExpense.Checked ? "1" : "0",
                StoreCD = comboStore.SelectedValue.ToString()
            };

            return dppe;
        }

        /// <summary>
        /// 入金予定表データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private DataTable CheckData(int type)
        {
            DataTable dt = null;
            if (ErrorCheck())
            {
                dppe = GetSearchInfo();
                dt = shyhbl.D_PayPlan_SelectForPrint(dppe, type);
                //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
                if (dt.Rows.Count == 0)
                {
                    bbl.ShowMessage("E128");
                    return null;
                }
            }

            return dt;
        }

        /// <summary>
        /// <Remark>PDF出力F11</Remark>
        /// <Remark>印刷する処理F12</Remark>
        /// </summary>
        protected override void PrintSec()
        {
            // レコード定義を行う   
            try
            {
               
                //DataTable table = CheckData(1);
                //if (table == null)
                //{
                //    return;
                //}
                DialogResult ret;
                SiharaiYoteiHyou_Report Report = new SiharaiYoteiHyou_Report();

                switch (PrintMode)
                {

                    case EPrintMode.DIRECT:
                        dtResult = CheckData(1);
                        if (dtResult == null) return;

                        if (StartUpKBN == "1")
                        {
                            ret = DialogResult.No;
                        }
                        else
                        {
                            //Q202 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                            ret = bbl.ShowMessage("Q201");
                            if (ret == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                        Report.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        Report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;

                        // 印字データをセット
                        Report.SetDataSource(dtResult);
                        Report.Refresh();
                        Report.SetParameterValue("StoreCD", comboStore.SelectedValue.ToString() + "  " + comboStore.Text);
                        Report.SetParameterValue("DateFrom", txtPaymentDueDateFrom.Text);
                        Report.SetParameterValue("DateTo", txtPaymentDueDateTo.Text);
                        Report.SetParameterValue("PrintDate", System.DateTime.Now.ToString("yyyy/MM/dd") + " " + System.DateTime.Now.ToString("hh:mm"));

                        if (ret == DialogResult.Yes)
                        {
                            //プレビュー
                            var previewForm = new Viewer();
                            previewForm.CrystalReportViewer1.ShowPrintButton = true;
                            previewForm.CrystalReportViewer1.ReportSource = Report;
                            previewForm.ShowDialog();
                        }
                        else
                        {
                            //int marginLeft = 360;
                            CrystalDecisions.Shared.PageMargins margin = Report.PrintOptions.PageMargins;
                            margin.leftMargin = DefaultMargin.Left; // mmの指定をtwip単位に変換する
                            margin.topMargin = DefaultMargin.Top;
                            margin.bottomMargin = DefaultMargin.Bottom;//mmToTwip(marginLeft);
                            margin.rightMargin = DefaultMargin.Right;
                            Report.PrintOptions.ApplyPageMargins(margin);     /// Error Now
                            // プリンタに印刷
                            System.Drawing.Printing.PageSettings ps;
                            try
                            {
                                System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();

                                CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();

                                System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();



                                Report.PrintOptions.PrinterName = "\\\\dataserver\\Canon LBP2900";
                                System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                                Report.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;

                                Report.PrintOptions.PrinterDuplex = PrinterDuplex.Simplex;

                                Report.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
                                // Print the report. Set the startPageN and endPageN 
                                // parameters to 0 to print all pages. 
                                //Report.PrintToPrinter(1, false, 0, 0);
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        break;
                    case EPrintMode.CSV:
                        dtCSV = CheckData(2);
                        if (dtCSV == null) return;
                        try
                        {
                            DialogResult DResult;
                            DResult = bbl.ShowMessage("Q203");
                            if (DResult == DialogResult.Yes)
                            {
                                ////LoacalDirectory
                                string folderPath = "C:\\CSV\\";
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }
                                SaveFileDialog savedialog = new SaveFileDialog();
                                savedialog.Filter = "CSV|*.csv";
                                savedialog.Title = "Save";
                                savedialog.FileName = "支払予定表";
                                savedialog.InitialDirectory = folderPath;
                                savedialog.RestoreDirectory = true;
                                if (savedialog.ShowDialog() == DialogResult.OK)
                                {
                                    if (Path.GetExtension(savedialog.FileName).Contains("csv"))
                                    {
                                        CsvWriter csvwriter = new CsvWriter();
                                        csvwriter.WriteCsv(dtCSV, savedialog.FileName, Encoding.GetEncoding(932));
                                    }
                                    Process.Start(Path.GetDirectoryName(savedialog.FileName));
                                    shyhbl.ShowMessage("I203");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                        break;
                    case EPrintMode.PDF:

                        dtResult = CheckData(1);
                        if (dtResult == null) return;

                        if (bbl.ShowMessage("Q204") != DialogResult.Yes)
                        {
                            return;
                        }
                        string filePath = "";
                        if (!ShowSaveFileDialog(InProgramNM, out filePath))
                        {
                            return;
                        }

                        // 印字データをセット
                        Report.SetDataSource(dtResult);
                        Report.Refresh();
                        Report.SetParameterValue("StoreCD", comboStore.SelectedValue.ToString() + "  " + comboStore.Text);
                        Report.SetParameterValue("DateFrom", txtPaymentDueDateFrom.Text);
                        Report.SetParameterValue("DateTo", txtPaymentDueDateTo.Text);
                        Report.SetParameterValue("PrintDate", System.DateTime.Now.ToString("yyyy/MM/dd") + " " + System.DateTime.Now.ToString("hh:mm"));

                        bool result = OutputPDF(filePath, Report);

                        //PDF出力が完了しました。
                        bbl.ShowMessage("I202");
                        break;
                }

                //プログラム実行履歴
                InsertLog(Get_L_Log_Entity());

                //ClearDetail();
            }
            finally
            {

            }
        }

        /// <summary>
        /// get Log information
        /// print log
        /// </summary>
        private L_Log_Entity Get_L_Log_Entity()
        {

            L_Log_Entity lle = new L_Log_Entity();
            DataTable table = CheckData(1);
            string item = table.Rows[0]["PayPlanNO"].ToString();
            for (int i = 1; i < table.Rows.Count; i++)
            {
                item += "," + table.Rows[i]["PayPlanNO"].ToString();
            }

            lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "",
                KeyItem = item
            };

            return lle;
        }
        private void SiharaiYoteiHyou_KeyUp(object sender, KeyEventArgs e)
        {
            if(combo_focus == true)
            {
                comboStore.Focus();
                combo_focus = false;
            }
            else { MoveNextControl(e); }
        }

        /// <summary>
        /// <Remark>Parameter Field in Search_Vendor</Remark>
        /// </summary>
        private void scPaymentDestinaion_Enter(object sender, EventArgs e)
        {
            scPaymentDestinaion.Value1 = "2";
            scPaymentDestinaion.ChangeDate = bbl.GetDate();
        }

        private void comboStore_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode ==Keys.Enter)
            {
                if (!base.CheckAvailableStores(comboStore.SelectedValue.ToString()))
                {
                    shyhbl.ShowMessage("E139");
                    comboStore.Focus();
                    combo_focus = true;
                }
            }
        }
        private void txtPaymentDueDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                /// <remarks>支払予定日(from)は支払予定日(To)より大きいの場合、エラーになる処理</remarks>
                if (!string.IsNullOrWhiteSpace(txtPaymentDueDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPaymentDueDateTo.Text))
                {
                    DateTime dt1 = Convert.ToDateTime(txtPaymentDueDateFrom.Text);
                    DateTime dt2 = Convert.ToDateTime(txtPaymentDueDateTo.Text);

                    if (dt1 > dt2)
                    {
                        shyhbl.ShowMessage("E104");
                        txtPaymentDueDateTo.Focus();
                    }
                }
            }
        }
    }
}
