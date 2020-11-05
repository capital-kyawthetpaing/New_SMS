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
using ClosedXML.Excel;
using CrystalDecisions.Shared;

namespace SiiresakiZaikoYoteiHyou
{
    public partial class SiiresakiZaikoYoteiHyou : FrmMainForm
    {
        SiiresakiZaikoYoteiHyou_BL szybl;
        D_MonthlyPurchase_Entity dmpe;
        DataTable dt;
        CrystalDecisions.Windows.Forms.CrystalReportViewer crv;
        M_StoreClose_Entity msce;

        public SiiresakiZaikoYoteiHyou()
        {
            InitializeComponent();
            szybl = new SiiresakiZaikoYoteiHyou_BL();
            dmpe = new D_MonthlyPurchase_Entity();
            dt = new DataTable();
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
            txtTargetDateTo.Focus();
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
            txtTargetDateFrom.Text = DateTime.Now.ToString("yyyy/MM");
            txtTargetDateTo.Text = DateTime.Now.ToString("yyyy/MM");
            txtTargetDateTo.Focus();
            cboStore.SelectedValue = StoreCD;
        }
        
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);
            switch (Index + 1)
            {
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear();
                    }
                    break;
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

        protected DataTable ChangeDataColumnName(DataTable dtAdd)
        {
            dtAdd.Columns["VendorCD"].ColumnName = "仕入先";
            dtAdd.Columns["VendorName"].ColumnName = "仕入先名";
            dtAdd.Columns["LastMonthQuantity"].ColumnName = "前月残";
            dtAdd.Columns["LastMonthAmount"].ColumnName = "前月残額";
            dtAdd.Columns["ThisMonthPurchaseQ"].ColumnName = "仕入";
            dtAdd.Columns["ThisMonthPurchaseA"].ColumnName = "当月仕入額";
            dtAdd.Columns["ThisMonthCustPurchaseQ"].ColumnName = "うち客注";
            dtAdd.Columns["ThisMonthCustPurchaseA"].ColumnName = "当月客注仕入額";
            dtAdd.Columns["ThisMonthPurchasePlanQ"].ColumnName = "仕入予定";
            dtAdd.Columns["ThisMonthPurchasePlanA"].ColumnName = "当月仕入予定額";
            dtAdd.Columns["ThisMonthSalesQ"].ColumnName = "売上";
            dtAdd.Columns["ThisMonthSalesA"].ColumnName = "'当月売上額";
            dtAdd.Columns["ThisMonthCustSalesQ"].ColumnName = "'うち客注";
            dtAdd.Columns["ThisMonthCustSalesA"].ColumnName = "当月客注売上額";
            dtAdd.Columns["ThisMonthSalesPlanQ"].ColumnName = "売上予定";
            dtAdd.Columns["ThisMonthSalesPlanA"].ColumnName = "当月売上予定額";
            dtAdd.Columns["ThisMonthReturnsQ"].ColumnName = "返品";
            dtAdd.Columns["ThisMonthReturnsA"].ColumnName = "当月返品額";
            dtAdd.Columns["ThisMonthReturnsPlanQ"].ColumnName = "返品予定";
            dtAdd.Columns["ThisMonthReturnsPlanA"].ColumnName = "当月返品予定額";
            dtAdd.Columns["ThisMonthPlanQuantity"].ColumnName = "当月予定残";
            dtAdd.Columns["ThisMonthPlanAmount"].ColumnName = "当月予定残額";
            //dtAdd.Columns.RemoveAt(2);
            return dtAdd;
        }
        private void F11()
        {
            if (ErrorCheck())
            {
                dmpe = new D_MonthlyPurchase_Entity();
                dmpe = GetData();
                DataTable dt = szybl.RPC_SiiresakiZaikoYoteiHyou(dmpe);
               // dt.Columns.Remove("Today");
                if (dt.Rows.Count > 0)
                {
                    DataTable dtExport = dt;
                    dtExport = ChangeDataColumnName(dtExport);
                    string folderPath = "C:\\SES\\";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    SaveFileDialog savedialog = new SaveFileDialog();
                    savedialog.Filter = "Excel Files|*.xlsx;";
                    savedialog.Title = "Save";
                    InProgramNM= "仕入先在庫予定表";
                    string cmdLine = InProgramNM + " " + DateTime.Now.ToString(" yyyyMMdd_HHmmss ") + " " + InOperatorCD;
                    savedialog.FileName = cmdLine;
                    savedialog.InitialDirectory = folderPath;
                    savedialog.RestoreDirectory = true;
                    if (savedialog.ShowDialog() == DialogResult.OK)
                    {
                        if (Path.GetExtension(savedialog.FileName).Contains(".xlsx"))
                        {
                            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
                            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                            worksheet = workbook.ActiveSheet;
                            worksheet.Name = "worksheet";
                            Microsoft.Office.Interop.Excel.Range excelRange = worksheet.UsedRange;
                            excelRange.NumberFormat = "#,###,###";//
                            //excelRange.Worksheet.ListObjects["worksheet"].TableStyle = "none";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dtExport,"worksheet");
                                wb.Worksheet("worksheet").Row(1).InsertRowsAbove(1);
                                wb.Worksheet("worksheet").Row(1).InsertRowsAbove(1);
                                wb.Worksheet("worksheet").Cell(1,1).Value = "年月：";

                                wb.Worksheet("worksheet").Cell(2, 1).Value = "店舗:";
                                wb.Worksheet("worksheet").Cell(1, 2).Value = "'" + txtTargetDateFrom.Text;
                                wb.Worksheet("worksheet").Cell(1, 3).Value = "～";
                                wb.Worksheet("worksheet").Cell(1, 4).Value = "'" + txtTargetDateTo.Text;
                                wb.Worksheet("worksheet").Cell(2, 2).Value = cboStore.SelectedValue.ToString();
                                wb.Worksheet("worksheet").Cell(2, 3).Value = cboStore.Text.ToString();
                                wb.Worksheet("worksheet").Row(3).InsertRowsAbove(1);
                                wb.Worksheet("worksheet").Row(4).CopyTo(wb.Worksheet("worksheet").Row(3));
                                wb.Worksheet("worksheet").Row(4).Delete();
                                wb.Worksheet("worksheet").ShowGridLines = false;
                                wb.Worksheet("worksheet").Tables.FirstOrDefault().ShowAutoFilter = false;
                                wb.Worksheet("worksheet").Tables.FirstOrDefault().Theme = XLTableTheme.None;
                                wb.SaveAs(savedialog.FileName);
                                szybl.ShowMessage("I203", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            }
                            Process.Start(Path.GetDirectoryName(savedialog.FileName));
                        }
                    }
                }
                else
                {
                    szybl.ShowMessage("E128");
                    txtTargetDateTo.Focus();
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
                CheckBeforeExport();
                dmpe = new D_MonthlyPurchase_Entity();
                dmpe = GetData();
                DataTable dt = szybl.RPC_SiiresakiZaikoYoteiHyou(dmpe);
                if (dt.Rows.Count > 0)
                {
                     //CheckBeforeExport();
                    try
                    {
                        SiiresakiZaikoYoteiHyou_Report szy_Report = new SiiresakiZaikoYoteiHyou_Report();
                        DialogResult DResult;
                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:
                                DResult = bbl.ShowMessage("Q201");
                                if (DResult == DialogResult.No)
                                {
                                    return;
                                }
                                szy_Report.SetDataSource(dt);
                                szy_Report.Refresh();
                                szy_Report.SetParameterValue("lblDateFrom", txtTargetDateFrom.Text);
                                szy_Report.SetParameterValue("lblDateTo",txtTargetDateTo.Text);
                                szy_Report.SetParameterValue("lblStore", cboStore.SelectedValue.ToString() + "   " + cboStore.Text);
                                //szy_Report.SetParameterValue("lblToday", dt.Rows[0]["Today"].ToString());
                                szy_Report.SetParameterValue("lblToday", DateTime.Now.ToString("yyyy/MM/dd") + "  " + DateTime.Now.ToString("HH:mm"));
                                try
                                {

                                }
                                catch (Exception ex)
                                {
                                    var msg = ex.Message;
                                }
                                if (DResult == DialogResult.Yes)
                                {
                                    var vr = new Viewer();
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
                else
                {
                    szybl.ShowMessage("E128");
                    txtTargetDateTo.Focus();
                }
            }
        }

        private void CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();

            if (szybl.M_StoreClose_Check(msce, "5").Rows.Count > 0)
            {
                string ProgramID = "GetsujiZaikoKeisanSyori";
                string ProgramID1 = "GetsujiShiireKeisanSyori";
                RunConsole(ProgramID,ProgramID1, msce.FiscalYYYYMM);
            }
        }
        private void RunConsole(string programID,string programID1, string FiscalYYYYMM)
        {
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "1";
            string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + StoreCD + " " + " " + Mode + " " + FiscalYYYYMM;//parameter
            //string str = "GetsujiZaikoKeisanSyori,GetsujiShiireKeisanSyori";
            //System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            //System.Diagnostics.Process.Start(filePath+@"\"+str.Substring(0,22)+".exe",cmdLine+"");
            //System.Diagnostics.Process.Start(filePath+@"\"+str.Substring(24,23)+".exe",cmdLine+"");
            Process p1 = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            Process p2= System.Diagnostics.Process.Start(filePath + @"\" + programID1 + ".exe", cmdLine + "");
            p2.WaitForExit();
            p1.WaitForExit();
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
