using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using DL;
using Entity;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace KeihiltiranHyou
{

    public partial class FrmKeihiltiranHyou : FrmMainForm
    {
        CrystalDecisions.Windows.Forms.CrystalReportViewer vr;
        string PCname;
        D_Cost_Entity dce, dce2;
        KeihiltiranHyou_BL kbl;
        string[] lst;
        Viewer previewForm;
        KeihiltiranHyou_BL kthbl = new KeihiltiranHyou_BL();
        DataTable dtlog;

       // D_Cost_Entity dce;
        public FrmKeihiltiranHyou()
        {
            InitializeComponent();

            try
            {
                lst = new string[] { };
                previewForm = new Viewer();
                dce = new D_Cost_Entity();
                this.KeyUp += FrmKeihiltiranHyou_KeyUp;
                kbl = new KeihiltiranHyou_BL();
                dce2 = new D_Cost_Entity();
                vr = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
                dtlog = new DataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
       protected void Loadreport()
        {
          
            // DataTable dt = null;
            // rep.SetParameterValue("txtPageNo","");
            //rep.SetParameterValue("txtStoreName", dce.Store);
            //rep.SetParameterValue("txtDateTime", "2019/09/09  12:12");
            //rep.SetParameterValue("CostGaku", "999,999,999");
            ////crystalReportViewer1.ReportSource = null;   // Last time by PTK 
            ////crystalReportViewer1.ReportSource = rep;    // Last time by PTK
            //previewForm.CrystalReportViewer1.ShowPrintButton = true;
            //previewForm.CrystalReportViewer1.ReportSource = Report;
            //previewForm.ShowDialog();
        }
        private void FrmKeihiltiranHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void FrmKeihiltiranHyou_Load(object sender, EventArgs e)
        {

         //   AllEvent_PrintLog();
            try
            {
                try
                {
                    PCname = this.InPcID;
                    InProgramID = "KeihiltiranHyou";
                    PCname = InPcID;
                    this.Text = "経費一覧表";
                    //起動時共通処理
                    StartProgram();
                    //SetFunctionLabel(EProMode.MENTE);
                    SetFunctionLabel(EProMode.PRINT);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                var dt = DateTime.Now;
                var val = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0').ToString() + "/" + dt.Day.ToString().PadLeft(2, '0').ToString();
                txtRecordFrom.Text = val;
                txtRecordTo.Text = val;
                try
                {
                    BindCombo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //cboStoreName.Require(true);
                txtRecordFrom.Focus();
                F9Visible = false;
                F10Visible = false;
                F11Visible = false;
            }
            catch(Exception ex) {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();
            }
        }

        private void BindCombo()
        {
            string data = InOperatorCD;
            string date = DateTime.Today.ToShortDateString();
            // cboStoreName.Bind(date, data);
            cboStoreName.Bind(string.Empty, "2");
            cboStoreName.SelectedValue = StoreCD;
        }

        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index)
            {
                case 0: // F1:終了
                    {
                        break;
                    }
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会
                    {
                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        break;
                    }

                case 11:
                    break;

            }   //switch end
        }

        protected override void PrintSec()
        {
            // レコード定義を行う
            // DataTable table = new DataTable();



            if (ErrorCheck())
            {
                //if (kthbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                //{
                    dce2 = GetCostData();
                    DataTable table = dtlog = kbl.getPrintData(dce2);

                    try
                    {
                        if (table == null)
                        {
                            return;
                        }

                        DialogResult ret;
                        Dataset.Keihichiranhyou Report = new Dataset.Keihichiranhyou();

                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:
                                //if (StartUpKBN == "1")
                                //{
                                //    ret = DialogResult.No;
                                //}
                                //else
                                //{
                                //Q202 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                                ret = bbl.ShowMessage("Q202");
                                if (ret == DialogResult.Cancel)
                                {
                                    return;
                                }
                            //}

                            // 印字データをセット
                            try
                            {
                                Report.SetDataSource(table);
                            }
                            catch(Exception ex) {
                                var exc = ex.StackTrace;
                            }
                                Report.Refresh();
                                Report.SetParameterValue("txtStoreName", cboStoreName.SelectedValue.ToString() + "  " + dce2.Store);
                                Report.SetParameterValue("txtDateTime", table.Rows[0]["yyyymmdd"].ToString() + "   " + table.Rows[0]["mmss"].ToString());
                                vr = previewForm.CrystalReportViewer1;
                                AllEvent_PrintLog();
                                if (ret == DialogResult.Yes)
                                {
                                    previewForm.CrystalReportViewer1.ShowPrintButton = true;
                                    previewForm.CrystalReportViewer1.ReportSource = Report;
                                    previewForm.ShowDialog();
                                }
                                else     /// //Still Not Working because of Applymargin and Printer not Setting up  (PTK Will Solve)
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

                            case EPrintMode.PDF:
                                //if (bbl.ShowMessage("Q204") != DialogResult.Yes)
                                //{
                                //    return;
                                //}
                                //string filePath = "";
                                //if (!ShowSaveFileDialog(InProgramNM, out filePath))
                                //{
                                //    return;
                                //}

                                // 印字データをセット
                                //Report.SetDataSource(table);
                                // Report.Refresh();

                                //  bool result = OutputPDF(filePath, Report);

                                //PDF出力が完了しました。
                                bbl.ShowMessage("I202");

                                break;
                        }

                        //更新処理
                        //tableの請求番号だけ
                        //  mibl.D_Billing_Update(dse, dtForUpdate, InOperatorCD, InPcID);

                    }
                    finally
                    {
                        // DeleteExclusive(dtForUpdate);
                    }

                    //更新後画面そのまま
                    //  detailControls[1].Focus();
                //}
            }
        }
        private void F12()
        {
            if (ErrorCheck())
            {
                if (kthbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    DisplayData();
                }
            } 
        }

        private bool ErrorCheck()
        {
            if(!string.IsNullOrWhiteSpace(txtRecordFrom.Text))
            {
                M_Calendar_Entity mce = new M_Calendar_Entity();
                //mce.CalendarDate = txtRecordFrom.Text;
                //DataTable dcFrom = new DataTable();
                //dcFrom = kthbl.CalendarSelect(mce);
                //if(dcFrom.Rows.Count > 0)
                //{
                //    txtRecordFrom.Focus();
                //    return false;
                //}

                if(!string.IsNullOrWhiteSpace(txtRecordTo.Text))
                {
                    //mce.CalendarDate = txtRecordTo.Text;
                    //DataTable dcTo = new DataTable();
                    //dcTo = kthbl.CalendarSelect(mce);
                    //if (dcTo.Rows.Count > 0)
                    //{
                    //    txtRecordTo.Focus();
                    //    return false;
                    //}

                    int result = txtRecordFrom.Text.CompareTo(txtRecordTo.Text);
                    if (result > 0)
                    {
                        kthbl.ShowMessage("E104");
                        txtRecordTo.Focus();
                        return false;
                    }
                }
                if (cboStoreName.SelectedValue.ToString() == "-1")
                {
                    kthbl.ShowMessage("E102");
                    cboStoreName.Focus();
                    return false;
                }
                //else
                //{
                //    //M_StoreAuthorizations_Entity  mae = new M_StoreAuthorizations_Entity();
                //    //mae.StoreAuthorizationsCD = made.AuthorizationsCD;
                //    //mae.ChangeDate = made.ChangeDate;
                //    //mae.ProgramID = "KeihiltiranHyou";
                //    //mae.StoreCD = InOperatorCD;
                //    //DataTable dtstoreAuto = new DataTable();

                //}

                if(!string.IsNullOrWhiteSpace(txtPaymentTo.Text))
                {
                    int result = txtPaymentFrom.Text.CompareTo(txtPaymentTo.Text);
                    if (result > 0)
                    {
                        kthbl.ShowMessage("E104");
                        txtPaymentTo.Focus();
                        return false;
                    }
                }
                if(!string .IsNullOrWhiteSpace (txtExpenseTo.Text))
                {
                    int result = txtExpenseFrom.Text.CompareTo(txtExpenseTo.Text);
                    if (result > 0)
                    {
                        kthbl.ShowMessage("E104");
                        txtExpenseTo.Focus();
                        return false;
                    }
                }

            }

            return true;
        }
        protected D_Cost_Entity GetCostData()
        {
            D_Cost_Entity de = new D_Cost_Entity();
            de.RecordedDateFrom = txtRecordFrom.Text;
            de.RecordedDateTo = txtRecordTo.Text;
            de.PaymentDateFrom = txtPaymentFrom.Text;
            de.PaymentDateTo = txtPaymentTo.Text;
            de.ExpanseEntryDateFrom = txtExpenseFrom.Text;
            de.ExpanseEntryDateTo = txtExpenseTo.Text;
            de.Expense_TimeFrom = expense_timefrom.Text;
            de.ExPense_TimeTo = expense_timeto.Text;
            de.PrintTarget = rdb_unpaid.Checked ? "0" : rdb_paid.Checked ? "1" : "";
            de.Store = cboStoreName.Text;
            return de;
        }
        private void DisplayData()
        {
            //keihiPrint print = new keihiPrint(GetCostData(), new string[] { InOperatorCD, InProgramID, InPcID, "PRINT", "001" });
            //print.ShowDialog();   // Previous Style is Like that by PTK   // Later hesaka Format
        }

        protected override void EndSec()
        {
            this.Close();
        }
        public void AllEvent_PrintLog()
        {

            foreach (ToolStrip ts in vr.Controls.OfType<ToolStrip>())
            {
                foreach (ToolStripButton tsb in ts.Items.OfType<ToolStripButton>())
                {

                    if (tsb.ToolTipText.ToLower().Contains("print"))
                    {

                        tsb.Click += new EventHandler(printButton_Click);
                    }
                }
            }

        }
        private void printButton_Click(object sender, EventArgs e)
        {
            var dt =new DataTable();
            dt.Columns.Add("CostNo");
            L_Log_DL ldl = new L_Log_DL();
            for (int i =0; i < dtlog.Rows.Count; i ++)
            {
                dt.Rows.Add(dtlog.Rows[i]["CostNo"].ToString());
            }
            dt.AcceptChanges();
            if (ldl.L_Log_Insert_Print(new string[] { InOperatorCD, InPcID,InProgramID }, dt))
            {
                //Finished Log recorded
            }
        }
    }
}

