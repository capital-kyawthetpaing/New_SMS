using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using Base.Client;
using CKM_Controls;
using System.Diagnostics;
using System.IO;
using ElencySolutions.CsvHelper;
using CrystalDecisions.Shared;

namespace SiiresakiZaikoYoteiHyou
{
    public partial class SiiresakiZaikoYoteiHyou : FrmMainForm
    {
        SiiresakiZaikoYoteiHyou_BL szybl;
        D_MonthlyPurchase_Entity dmpe;
        DataTable dt;
        Viewer vr;
        CrystalDecisions.Windows.Forms.CrystalReportViewer crv;
        M_StoreClose_Entity msce;

        public SiiresakiZaikoYoteiHyou()
        {
            InitializeComponent();
            szybl = new SiiresakiZaikoYoteiHyou_BL();
            dmpe = new D_MonthlyPurchase_Entity();
            dt = new DataTable();
            vr = new Viewer();
        }

        private void SiiresakiZaikoYoteiHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "SiiresakiZaikoYoteiHyou";
            StartProgram();
            SetRequiredField();
            SetFunctionLabel(EProMode.PRINT);
            cboStore.Bind(string.Empty, "2");
            cboStore.SelectedValue = StoreCD;
            F9Visible = false;
            F10Visible = false;
            Btn_F11.Text = "Excel(F11)";
            txtTargetDateFrom.Text = DateTime.Now.ToString("yyyy/MM");
            txtTargetDateTo.Text = DateTime.Now.ToString("yyyy/MM");
        }
        private void SetRequiredField()
        {
            txtTargetDateTo.Require(true);
            cboStore.Require(true);
        }

        protected override void EndSec()
        {
            this.Close();
        }
        public void Clear()
        {
            Clear(panalDetail);
            txtTargetDateTo.Focus();
        }
        
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);
            switch (Index + 1)
            {
                case 6:
                    {
                        if (szybl.ShowMessage("Q004") != DialogResult.Yes)
                            return;
                        break;
                    }
                case 11:
                    F11();
                    break;
            }
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtTargetDateTo, cboStore }))
                return false;
            if (!string.IsNullOrWhiteSpace(txtTargetDateFrom.Text) && !string.IsNullOrWhiteSpace(txtTargetDateTo.Text))
            {
                if (string.Compare(txtTargetDateFrom.Text, txtTargetDateTo.Text) == 1)
                {
                    szybl.ShowMessage("E104");
                    return false;
                }
            }
            return true;
        }
       
        private D_MonthlyPurchase_Entity GetData()
        {
            dmpe = new D_MonthlyPurchase_Entity()
            {
                StoreCD=cboStore.SelectedValue.ToString(),
                YYYYMMS=txtTargetDateFrom.Text.Replace("/", ""),
                YYYYMME=txtTargetDateTo.Text.Replace("/", "")
            };
            return dmpe;
        }
        private M_StoreClose_Entity GetStoreClose_Data()
        {
            msce = new M_StoreClose_Entity()
            {
                StoreCD = cboStore.SelectedValue.ToString(),
                FiscalYYYYMM = txtTargetDateFrom.Text.Replace("/", ""),
            };
            return msce;
        }
        private DataTable CreateDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("仕入先CD");
            dt.Columns.Add("仕入先名");
            dt.Columns.Add("前月残数");
            dt.Columns.Add("前月残額");
            dt.Columns.Add("当月仕入数");
            dt.Columns.Add("当月仕入額");
            dt.Columns.Add("当月客注仕入数");
            dt.Columns.Add("当月客注仕入額");
            dt.Columns.Add("当月仕入予定数");
            dt.Columns.Add("当月仕入予定額");
            dt.Columns.Add("当月売上数");
            dt.Columns.Add("'当月売上額");
            dt.Columns.Add("'当月客注売上数");
            dt.Columns.Add("当月客注売上額");
            dt.Columns.Add("当月売上予定数");
            dt.Columns.Add("当月売上予定額");
            dt.Columns.Add("当月返品数");
            dt.Columns.Add("当月返品額");
            dt.Columns.Add("当月返品予定数");
            dt.Columns.Add("当月返品予定額");
            dt.Columns.Add("当月予定残数");
            dt.Columns.Add("当月予定残額");
            return dt;
        }

        private void F11()
        {
            if (ErrorCheck())
            {
                dmpe = GetData();
               
                dt = szybl.RPC_SiiresakiZaikoYoteiHyou(dmpe);
                //CheckBeforeExport();

                if (dt.Rows.Count > 0)
                {
                    DataTable dtCsv = new DataTable();
                    dtCsv = CreateDatatable();
                    for (int i = 0; dt.Rows.Count > i; i++)
                    {
                        dtCsv.Rows.Add();
                        dtCsv.Rows[i]["仕入先CD"] = dt.Rows[i]["VendorCD"].ToString();
                        dtCsv.Rows[i]["仕入先名"] = dt.Rows[i]["VendorName"].ToString();
                        dtCsv.Rows[i]["前月残数"] = dt.Rows[i]["LastMonthQuantity"].ToString();
                        dtCsv.Rows[i]["前月残額"] = dt.Rows[i]["LastMonthAmount"].ToString();
                        dtCsv.Rows[i]["当月仕入数"] = dt.Rows[i]["ThisMonthPurchaseQ"].ToString();
                        dtCsv.Rows[i]["当月仕入額"] = dt.Rows[i]["ThisMonthPurchaseA"].ToString();
                        dtCsv.Rows[i]["当月客注仕入数"] = dt.Rows[i]["ThisMonthCustPurchaseQ"].ToString();
                        dtCsv.Rows[i]["当月客注仕入額"] = dt.Rows[i]["ThisMonthCustPurchaseA"].ToString();
                        dtCsv.Rows[i]["当月仕入予定数"] = dt.Rows[i]["ThisMonthPurchasePlanQ"].ToString();
                        dtCsv.Rows[i]["当月仕入予定額"] = dt.Rows[i]["ThisMonthPurchasePlanA"].ToString();
                        dtCsv.Rows[i]["当月売上数"] = dt.Rows[i]["ThisMonthSalesQ"].ToString();
                        dtCsv.Rows[i]["'当月売上額"] = dt.Rows[i]["ThisMonthSalesA"].ToString();
                        dtCsv.Rows[i]["'当月客注売上数"] = dt.Rows[i]["ThisMonthCustSalesQ"].ToString();
                        dtCsv.Rows[i]["当月客注売上額"] = dt.Rows[i]["ThisMonthCustSalesA"].ToString();
                        dtCsv.Rows[i]["当月売上予定数"] = dt.Rows[i]["ThisMonthSalesPlanQ"].ToString();
                        dtCsv.Rows[i]["当月売上予定額"] = dt.Rows[i]["ThisMonthSalesPlanA"].ToString();
                        dtCsv.Rows[i]["当月返品数"] = dt.Rows[i]["ThisMonthReturnsQ"].ToString();
                        dtCsv.Rows[i]["当月返品額"] = dt.Rows[i]["ThisMonthReturnsA"].ToString();
                        dtCsv.Rows[i]["当月返品予定数"] = dt.Rows[i]["ThisMonthReturnsPlanQ"].ToString();
                        dtCsv.Rows[i]["当月返品予定額"] = dt.Rows[i]["ThisMonthReturnsPlanA"].ToString();
                        dtCsv.Rows[i]["当月予定残数"] = dt.Rows[i]["ThisMonthPlanQuantity"].ToString();
                        dtCsv.Rows[i]["当月予定残額"] = dt.Rows[i]["ThisMonthPlanAmount"].ToString();
                    }
                    try
                    {
                        DialogResult DResult;
                        DResult = bbl.ShowMessage("Q201");
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
                            savedialog.FileName = "債務管理表";
                            savedialog.InitialDirectory = folderPath;
                            savedialog.RestoreDirectory = true;
                            if (savedialog.ShowDialog() == DialogResult.OK)
                            {
                                if (Path.GetExtension(savedialog.FileName).Contains("csv"))
                                {
                                    CsvWriter csvwriter = new CsvWriter();
                                    csvwriter.WriteCsv(dtCsv, savedialog.FileName, Encoding.GetEncoding(932));
                                }
                                Process.Start(Path.GetDirectoryName(savedialog.FileName));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private L_Log_Entity Get_L_Log_Entity()
        {

            L_Log_Entity lle = new L_Log_Entity();
            lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "",
                KeyItem = txtTargetDateFrom.Text + "," + cboStore.AccessibilityObject.Name + ","
            };

            return lle;
        }

        protected override void PrintSec()
        {
            if (PrintMode != EPrintMode.DIRECT)
                return;
            if (ErrorCheck())
            {
                dmpe = new D_MonthlyPurchase_Entity();
                dmpe = GetData();
                DataTable dt = szybl.RPC_SiiresakiZaikoYoteiHyou(dmpe);

                if (dt.Rows.Count > 0)
                {
                    // CheckBeforeExport();
                    try
                    {
                        SiiresakiZaikoYoteiHyou_Report szy_Report = new SiiresakiZaikoYoteiHyou_Report();
                        DialogResult DResult;
                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:
                                DResult = bbl.ShowMessage("Q201");
                                if (DResult == DialogResult.Cancel)
                                {
                                    return;
                                }
                                szy_Report.SetDataSource(dt);
                                szy_Report.Refresh();
                                szy_Report.SetParameterValue("lblDateFrom", txtTargetDateFrom.Text);
                                szy_Report.SetParameterValue("lblDateTo",txtTargetDateTo.Text);
                                szy_Report.SetParameterValue("lblStore", cboStore.SelectedValue.ToString() + "   " + cboStore.AccessibilityObject.Name);
                                szy_Report.SetParameterValue("lblToday", dt.Rows[0]["Today"].ToString());
                                try
                                {

                                }
                                catch (Exception ex)
                                {
                                    var msg = ex.Message;
                                }
                                if (DResult == DialogResult.Yes)
                                {
                                    vr.CrystalReportViewer1.ShowPrintButton = true;
                                    vr.CrystalReportViewer1.ReportSource = szy_Report;
                                    vr.ShowDialog();
                                }
                                else
                                {
                                    CrystalDecisions.Shared.PageMargins margin = szy_Report.PrintOptions.PageMargins;
                                    margin.leftMargin = DefaultMargin.Left;
                                    margin.topMargin = DefaultMargin.Top;
                                    margin.bottomMargin = DefaultMargin.Bottom;
                                    margin.rightMargin = DefaultMargin.Right;
                                    szy_Report.PrintOptions.ApplyPageMargins(margin);
                                    System.Drawing.Printing.PageSettings ps;
                                    try
                                    {
                                        System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();

                                        CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();

                                        System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();
                                        szy_Report.PrintOptions.PrinterName = "\\\\dataserver\\Canon LBP2900";
                                        System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                                        szy_Report.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;

                                        szy_Report.PrintOptions.PrinterDuplex = PrinterDuplex.Simplex;

                                        szy_Report.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                break;
                        }
                        InsertLog(Get_L_Log_Entity());
                    }
                    finally
                    {
                        txtTargetDateTo.Focus();
                    }
                }
            }
        }

        private void CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();

            if (szybl.M_StoreClose_Check(msce, "2").Rows.Count > 0)
            {
                string ProgramID = "GetsujiZaikoKeisanSyori,GetsujiShiireKeisanSyori";
                RunConsole(ProgramID, dmpe.YYYYMM);
            }
        }
        private void RunConsole(string programID, string YYYYMM)
        {
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "1";
            string cmdLine = " " + InOperatorCD + " " + Login_BL.GetHostName() + " " + StoreCD + " " + " " + Mode + " " + YYYYMM;//parameter
            string str = "GetsujiZaikoKeisanSyori,GetsujiShiireKeisanSyori";
            //System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            System.Diagnostics.Process.Start(filePath+@"\"+str.Substring(0,22)+".exe",cmdLine+"");
            System.Diagnostics.Process.Start(filePath+@"\"+str.Substring(24,23)+".exe",cmdLine+"");
            //System.Diagnostics.Process.Start(filePath+@"\"+programID+".exe",cmdLine+"");
        }
       
        private void SiiresakiZaikoYoteiHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void txtTargetDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtTargetDateFrom.Text) && !string.IsNullOrWhiteSpace(txtTargetDateTo.Text))
                {
                    if (string.Compare(txtTargetDateFrom.Text, txtTargetDateTo.Text) == 1)
                    {
                        szybl.ShowMessage("E104");
                        txtTargetDateTo.Focus();
                    }
                }
            }
        }
    }
}
