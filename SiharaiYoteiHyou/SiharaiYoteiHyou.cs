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


namespace SiharaiYoteiHyou
{
    public partial class SiharaiYoteiHyou : FrmMainForm
    {
        //string StoreAuthen_CD = "";
        //string StoreAuthen_ChangeDate = "";
        //string StoreCD = "";
        //string ClosedStatus = "";
        //string PaymentStatus = "";
        //string expense = "";
        //string purchase = "";
        //string ymd;
        Base_BL bbl = new Base_BL();
        SiharaiYoteiHyou_BL shyhbl;
        D_PayPlan_Entity dppe;
        private string StartUpKBN = "";
        public SiharaiYoteiHyou()
        {
            InitializeComponent();
        }

        private void SiharaiYoteiHyou_Load(object sender, EventArgs e)
        {
            shyhbl = new SiharaiYoteiHyou_BL();
            InProgramID = "SiharaiYoteiHyou";
            SetFunctionLabel(EProMode.PRINT);
            StartProgram();
            
            base.Btn_F11.Text = "Excel(F11)";
            base.Btn_F10.Text = "";
            BindCombo();
            RdoCloseStsSumi.Checked = true;
            RdoUnpaid.Checked = true;
            chkExpense.Checked = true;
            chkPurchase.Checked = true;
            this.comboStore.SelectedIndexChanged += ComboStore_SelectedIndexChanged;
            
            

            //if (radioClosedStatusSumi.Checked == true)
            //{
            //    ClosedStatus = "1";
            //}
            //else
            //{
            //    ClosedStatus = "0";
            //}
            //if (radioPaymentStatusUnpaid.Checked == true)
            //{
            //    PaymentStatus = "1";
            //}
            //else
            //{
            //    PaymentStatus = "0";
            //}
            //if (chkExpense.Checked == true)
            //{
            //    expense = "1";
            //}
            //else
            //{
            //    expense = "0";
            //}
            //if (chkPurchase.Checked == true)
            //{
            //    purchase = "1";
            //}
            //else
            //{
            //    purchase = "0";
            //}
        }

        private void ComboStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!comboStore.SelectedValue.Equals("-1"))
            {
                if (!base.CheckAvailableStores(comboStore.SelectedValue.ToString()))
                {
                    shyhbl.ShowMessage("E139");
                    comboStore.Focus();
                }
            }
        }

        /// <summary>
        /// 店舗データを取得してコンボボックスに表示する処理
        /// </summary>
        public void BindCombo()
        {
            comboStore.Bind(string.Empty, "2");
        }
        /// <summary>
        /// エラーチェック処理
        /// </summary>
        private bool ErrorCheck()
        {          
            if (!RequireCheck(new Control[] { scPaymentDestinaion.TxtCode }))
                return false;

            /// <remarks>店舗名を選択した場合、権限があるかとかをチェックする処理</remarks>
            if (!base.CheckAvailableStores(comboStore.SelectedValue.ToString()))
            {
                shyhbl.ShowMessage("E139");
                comboStore.Focus();
                return false;
            }

            /// <remarks>支払予定日(from)は支払予定日(To)より大きいの場合、エラーになる処理</remarks>
            if (!string.IsNullOrWhiteSpace(txtPaymentDueDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPaymentDueDateTo.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtPaymentDueDateFrom.Text);
                DateTime dt2 = Convert.ToDateTime(txtPaymentDueDateTo.Text);

                if (dt1 >= dt2)
                {
                    shyhbl.ShowMessage("E104");
                    txtPaymentDueDateFrom.Focus();
                    return false;
                }

            }

            /// <remarks>両方チェックが入っていない場合、エラーになる処理</remarks>
            if(chkExpense.Checked==false && chkPurchase.Checked==false)
            {
                shyhbl.ShowMessage("E111");
                return false;
            }

            if (!RequireCheck(new Control[] { comboStore }))   // go that focus
                return false;

            return true;
        }
        /// <summary>
        /// F1からF12までボタンをクリックする場合管理する処理
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int index)

        {
            base.FunctionProcess(index);
            switch (index)
            {
               
                case 5: //F6:キャンセル		
                        if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                        ClearDetail();
                        break;
            
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
            scPaymentDestinaion.Clear();
            radioClosedStatusAll.Checked = false;
            //radioClosedStatusSumi.Checked = false;
            radioPaymentStatusAll.Checked = false;
            //radioPaymentStatusUnpaid.Checked = false;
            //chkExpense.Checked = false;
            //chkPurchase.Checked = false;
            txtPaymentDueDateTo.Text = "";
            txtPaymentDueDateFrom.Text = "";
            comboStore.SelectedValue= -1;
            txtPaymentDueDateFrom.Focus();
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
                StoreCD = "",
                PayeeCD = scPaymentDestinaion.TxtCode.Text,
                PaymentDueDateFrom = txtPaymentDueDateFrom.Text,
                PaymenetDueDateTo = txtPaymentDueDateTo.Text,
                CloseStatusSumi = RdoCloseStsSumi.Checked ?  "1" : "0",
                PaymentStatusUnpaid = RdoUnpaid.Checked ?  "1" : "0",
                Purchase = chkPurchase.Checked ? "1" : "0",
                Expense = chkExpense.Checked ? "1" : "0"
            };

            return dppe;
        }

        /// <summary>
        /// 入金予定表データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private DataTable CheckData()
        {
            DataTable dt = null;
            if (ErrorCheck())
            {
                dppe = GetSearchInfo();
                 dt= shyhbl.D_PayPlan_SelectForPrint(dppe);
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
        ///印刷する処理
        /// </summary>
        protected override void PrintSec()
        {
            // レコード定義を行う
            DataTable table = CheckData();

            try
            {
                if (table == null)
                {
                    return;
                }
                //xsdファイルを保存します。

                //DB　---→　xsd　----→　クリスタルレポート

                //というデータの流れになります
                //table.TableName = ProID;
                //table.WriteXmlSchema("DataTable" + ProID + ".xsd");

                //①保存した.xsdはプロジェクトに追加しておきます。
                DialogResult ret;
                SiharaiYoteiHyou_Report Report = new SiharaiYoteiHyou_Report();

                //DataTableのDetailOnが１かどうかで詳細セクションを印字するかどうかの設定を
                //している（セクションエキスパート）

                switch (PrintMode)
                {
                    case EPrintMode.DIRECT:
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
                        Report.SetDataSource(table);
                        Report.Refresh();
                        Report.SetParameterValue("StoreCD", comboStore.SelectedValue.ToString() + "  " + comboStore.Text);
                        Report.SetParameterValue("DateFrom", txtPaymentDueDateFrom.Text);
                        Report.SetParameterValue("DateTo", txtPaymentDueDateTo.Text);
                        Report.SetParameterValue("PrintDate", System.DateTime.Now.ToString("yyyy/MM/dd") + " " + System.DateTime.Now.ToString("hh:mm"));

                        if (ret == DialogResult.Yes)
                        {
                            //プレビュー
                            var previewForm = new Viewer();
                            //previewForm.CrystalReportViewer1.
                            previewForm.CrystalReportViewer1.ShowPrintButton = true;
                            previewForm.CrystalReportViewer1.ReportSource = Report;
                            //previewForm.CrystalReportViewer1.Zoom(1);

                            previewForm.ShowDialog();
                        }
                        else
                        {
                            int marginLeft = 360;
                            CrystalDecisions.Shared.PageMargins margin = Report.PrintOptions.PageMargins;
                            margin.leftMargin = marginLeft; // mmの指定をtwip単位に変換する
                            margin.topMargin = marginLeft;
                            margin.bottomMargin = marginLeft;//mmToTwip(marginLeft);
                            margin.rightMargin = marginLeft;
                            Report.PrintOptions.ApplyPageMargins(margin);
                            // プリンタに印刷
                            Report.PrintToPrinter(0, false, 0, 0);
                        }
                        break;

                    case EPrintMode.PDF:
                        if (bbl.ShowMessage("Q205") != DialogResult.Yes)
                        {
                            return;
                        }
                        string filePath = "C:\\Csv\\";
                        //string filePath = "";
                        //if (!ShowSaveFileDialog(InProgramNM, out filePath))
                        //{
                        //    return;
                        //}

                        // 印字データをセット
                        Report.SetDataSource(table);
                        Report.Refresh();
                        Report.SetParameterValue("StoreCD", comboStore.SelectedValue.ToString() + "  " + comboStore.Text);
                        Report.SetParameterValue("DateFrom", txtPaymentDueDateFrom.Text);
                        Report.SetParameterValue("DateTo", txtPaymentDueDateTo.Text);
                        Report.SetParameterValue("PrintDate", System.DateTime.Now.ToString("yyyy/MM/dd") + " " + System.DateTime.Now.ToString("hh:mm"));

                        //bool result = OutputPDF(filePath, Report);
                        bool result1 = ExcelOutput(filePath, Report);

                        //Excel出力が完了しました。
                        bbl.ShowMessage("I203");

                        break;
                }

                //プログラム実行履歴
                InsertLog(Get_L_Log_Entity());
            }
            finally
            {

            }

            ClearDetail();
        }

        /// <summary>
        /// get Log information
        /// print log
        /// </summary>
        private L_Log_Entity Get_L_Log_Entity()
        {

            L_Log_Entity lle = new L_Log_Entity();
            DataTable table = CheckData();
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

        /// <summary>
        /// Excel出力処理
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="report">レポートオブジェクト</param>
        /// <returns></returns>
        public bool ExcelOutput(string filePath, ReportClass report)
        {
            // PDF形式でファイル出力
            try
            {
                string fileName = "ExcelExport";
                //if (System.IO.Path.GetExtension(filePath).ToLower() != ".xlsx")
                //{
                //    fileName = System.IO.Path.GetFileNameWithoutExtension(filePath) + ".xlsx";
                //}

                // 出力先ファイル名を指定
                CrystalDecisions.Shared.DiskFileDestinationOptions fileOption;
                fileOption = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                fileOption.DiskFileName = System.IO.Path.GetDirectoryName(filePath) + "\\" + fileName + ".xlsx";

                // 外部ファイル出力をExcel出力として定義する
                CrystalDecisions.Shared.ExportOptions option;
                option = report.ExportOptions;
                option.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                option.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
                option.FormatOptions = new CrystalDecisions.Shared.ExcelFormatOptions();
                option.DestinationOptions = fileOption;

                // excelとして外部ファイル出力を行う
                report.Export();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        private void SiharaiYoteiHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        /// <summary>
        /// <Remark>Parameter Field in Search_Vendor</Remark>
        /// </summary>
        private void scPaymentDestinaion_Enter(object sender, EventArgs e)
        {
            scPaymentDestinaion.Value1 = "2";
            scPaymentDestinaion.ChangeDate = txtPaymentDueDateTo.Text;
        }
    }
}
