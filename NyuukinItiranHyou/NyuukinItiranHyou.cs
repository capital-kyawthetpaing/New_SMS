
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
using Entity;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
namespace NyuukinItiranHyou
{
    public partial class NyuukinItiranHyou :FrmMainForm
    {
        DataTable dtlog;
        CrystalDecisions.Windows.Forms.CrystalReportViewer vr;
        Nyuukin_Ichihanyou_BL nih;
        Nyuukin_Ichihanyou_Entity nie;
        Viewer previewForm;
        public NyuukinItiranHyou()
        {
            InitializeComponent();
            this.KeyUp += NyuukinItiranHyou_KeyUp;
            nih = new Nyuukin_Ichihanyou_BL();
            previewForm = new Viewer();
            nie = new Nyuukin_Ichihanyou_Entity();
            dtlog = new DataTable();
        }

        private void NyuukinItiranHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void NyuukinItiranHyou_Load_1(object sender, EventArgs e)
        {
            try
            {
                InProgramID = "NyuukinItiranHyou";
                this.Text = "入金一覧表印刷";
                StartProgram();
                SetFunctionLabel(EProMode.PRINT);
                BindCombo();
                paymentstart.Focus();
                rdb_one.Checked = true;
                F9Visible = true;
                F10Visible = false;
                F11Visible = false;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();
            }
        }

        private bool ErrorCheck()   // each Error Check
        {

            if (String.IsNullOrWhiteSpace(cbo_store.Text))
            {
                bbl.ShowMessage("E102");
                cbo_store.Focus();
                return false;
            }
            //if (nih.Check_PaymentStartDate(paymentstart.Text))  // paymentstart  // paymentend // paymentinputstart // paymentinputend   need to check on enter
            //{ 

            //}
            return true;
        }
        private void BindCombo()
        {
            string data = InOperatorCD;
            string date = DateTime.Today.ToShortDateString();
            try
            {
                cbo_store.Bind(date, data);
                cbo_torikomi.Bind(date, data);
            }
            catch
            {
                bbl.ShowMessage("E139");   // When no data inside combo
            }
        }
        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index)
            {
                case 0: // F1:終了
                    {
                        if (bbl.ShowMessage("Q003") != DialogResult.Yes)
                            return;

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
                        if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                            Clear_Data();
                        else
                            return;

                        break;
                    }

                case 11:
                    break;

            }   //switch end
        }
        protected void Clear_Data()
        {
            paymentend.Text = "";
            paymentstart.Text = "";
            paymentinputend.Text = "";
            paymentinputstart.Text = "";
            search_customer.TxtCode.Text = "";
            search_customer.LabelText = "";
            rdb_one.Checked = true;
            
            BindCombo();
        }
        private void ckM_SearchControl2_Load(object sender, EventArgs e)
        {

        }

        protected Nyuukin_Ichihanyou_Entity GetNyuukinData()
        {
            nie = new Nyuukin_Ichihanyou_Entity();
            nie.paymentstart = paymentstart.Text;
            nie.paymentend = paymentend.Text;
            nie.cbo_store = cbo_store.Text;
            nie.paymentinputstart = paymentinputstart.Text;
            nie.paymentinputend = paymentinputend.Text;
            nie.cbo_torikomi = String.IsNullOrWhiteSpace(cbo_torikomi.Text)? null : cbo_torikomi.SelectedValue.ToString().Equals("-1") ?  string.Empty : cbo_torikomi.SelectedValue.ToString();
            nie.search_customer = search_customer.TxtCode.Text;
            nie.rdb_all = rdb_all.Checked ? "1" : "0";
            return nie;
        }
        protected override void PrintSec()

        {
            // レコード定義を行う
            // DataTable table = new DataTable();



            if (ErrorCheck())
            {
                    nie = GetNyuukinData();
                    DataTable table = dtlog = nih.getPrintData(nie);

                    try
                    {
                        if (table == null)
                        {
                            return;
                        }

                        DialogResult ret;
                        var Report =new DataSet.Nyuukin_Ichihanyou();

                    switch (PrintMode)
                    {
                        case EPrintMode.DIRECT:
                            ret = bbl.ShowMessage("Q202");
                            if (ret == DialogResult.Cancel)
                            {
                                return;
                            }
                            Report.SetDataSource(table);
                            Report.Refresh();
                            Report.SetParameterValue("txtStoreName", cbo_store.SelectedValue.ToString() + "  " + nie.cbo_store);
                            Report.SetParameterValue("txtDateTime", table.Rows[0]["yyyymmdd"].ToString() + "   " + table.Rows[0]["mmss"].ToString());
                            vr = previewForm.CrystalReportViewer1;
                            if (ret == DialogResult.Yes)
                            {
                                previewForm.CrystalReportViewer1.ShowPrintButton = true;
                                previewForm.CrystalReportViewer1.ReportSource = Report;
                                previewForm.ShowDialog();
                            }
                            else     /// //Still Not Working because of Applymargin and Printer not Setting up  (PTK Will Solve)
                            {
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
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            break;

                        case EPrintMode.PDF:
                            bbl.ShowMessage("I202");

                            break;
                    }
                    var dt = new DataTable();
                    dt.Columns.Add("StoreCD");
                    for (int i = 0; i < dtlog.Rows.Count; i++)
                    {
                        if (!String.IsNullOrWhiteSpace(dtlog.Rows[i]["StoreCD"].ToString()) && !dtlog.Rows[i]["StoreCD"].Equals(null)) 
                        dt.Rows.Add(dtlog.Rows[i]["StoreCD"].ToString());
                    }
                    dt.AcceptChanges();
                    nih.L_Log_Insert_Print(new string[] { InOperatorCD, InPcID, InProgramID }, dt);
                   

                }
                    finally
                    {
                        paymentstart.Focus();
                    }
                   
                    
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }
    }



}
