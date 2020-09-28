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
using Entity;
using BL;
using CrystalDecisions.Shared;
using System.IO;
using ClosedXML.Excel;
using System.Diagnostics;
using System.Globalization;

namespace UrikakekinTairyuuHyou
{
    public partial class UrikakekinTairyuuHyou : FrmMainForm
    {
        string todayDate = DateTime.Now.ToString("yyyy/MM/dd");
        M_StoreClose_Entity msce;
        UrikakekinTairyuuHyou_BL ukkthbl;
        public UrikakekinTairyuuHyou()
        {
            InitializeComponent();
        }

        private void UrikakekinTairyuuHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "UrikakekinTairyuuHyou";

            ukkthbl = new UrikakekinTairyuuHyou_BL();


           // SetFunctionLabel(EProMode.MENTE);
            SetFunctionLabel(EProMode.PRINT);
            StartProgram();

            ModeVisible = false;
            Btn_F10.Text = string.Empty;
            Btn_F11.Text = "Excel(F11)";
            BindData();
            SetRequireField();
        }

        private void BindData()
        {
            txtDate.Text = todayDate.Substring(0,todayDate.Length-3);
            txtDate.Focus();
            cboStore.Bind(todayDate, "2");
            cboStore.SelectedValue = StoreCD;
        }

        private void SetRequireField()
        {
            txtDate.Require(true);
            cboStore.Require(true);
        }

        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtDate, cboStore }))
                return false;

            return true;
        }


        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
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

                        Clear();
                        break;
                    }

                case 10: //F11
                    ExcelExport();
                    break;
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }

        private M_StoreClose_Entity GetStoreClose_Data()
        {
            msce = new M_StoreClose_Entity()
            {
                StoreCD = cboStore.SelectedValue.ToString(),
                FiscalYYYYMM = txtDate.Text.Replace("/", ""),
            };
            return msce;
        }

        private void OpenForm(string programID, string YYYYMM)
        {
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "1";
            //string cmdLine =  InOperatorCD + " " + Login_BL.GetHostName() + " " + StoreCD + " " + Mode + " " + YYYYMM;//parameter
            string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + StoreCD + " " + Mode + " " + YYYYMM;
            try
            {
                Process p = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
                p.WaitForExit();
            }
            catch
            {
                //skh_bl.ShowMessage("E138");
            }

        }
        public bool CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();
            if (ukkthbl.M_StoreClose_Check(msce, "4").Rows.Count > 0)
            {
                string ProgramID = "GetsujiSaikenKeisanSyori";

                //残す部分
                //NoFilePathcase
                OpenForm(ProgramID, msce.FiscalYYYYMM);
                //月次処理（債権集計処理）を起動する 
                return true;
            }
            return false;
        }

        public void ExcelExport()
        {
              if(ErrorCheck())
              {
                    CheckBeforeExport();
                    if (bbl.ShowMessage("Q205") == DialogResult.Yes)
                    {
                        msce = new M_StoreClose_Entity();
                        msce = GetStoreClose_Data();
                        DateTime dtime;
                        string[] strmonth = new string[12];
                        string now =txtDate.Text.ToString() + "/01";

                        if (DateTime.TryParseExact(now, "yyyy/MM/dd", null,
                                   DateTimeStyles.None, out dtime))
                        {
                          
                            for (int i = 11; i >= 0; i--)
                            {

                                strmonth[i] = dtime.AddMonths(-i).ToString("yyyy/MM/dd").Substring(0, 7).ToString();
                            }
                        }
                        DataTable dt=ukkthbl.Select_DataToExport(msce);
                        //DataRow dr = dt.NewRow();
                        //dr["CustomerCD"] = "";
                        //dr["CustomerName"] = "";
                        //dr["SaleA"] = "";
                        //dr["11"] = Convert.ToString(strmonth[11]);
                        //dr["10"] = strmonth[10].ToString();
                        //dr["9"] = strmonth[9].ToString();
                        //dr["8"] = strmonth[8].ToString();
                        //dr["7"] = strmonth[7].ToString();
                        //dr["6"] = strmonth[6].ToString();
                        //dr["5"] = strmonth[5].ToString();
                        //dr["4"] = strmonth[4].ToString();
                        //dr["3"] = strmonth[3].ToString();
                        //dr["2"] = strmonth[2].ToString();
                        //dr["1"] = strmonth[1].ToString();
                        //dr["0"] = strmonth[0].ToString();
                        //dt.Rows.Add(dr);

                        if (dt.Rows.Count > 0)
                        {
                            string customerCD = string.Empty;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (customerCD != dt.Rows[i]["CustomerCD"].ToString())
                                    customerCD = dt.Rows[i]["CustomerCD"].ToString();

                                DataTable dtResult = dt.Select("SaleA='売上' and CustomerCD='" + customerCD + "'").CopyToDataTable();
                                if(dtResult.Rows.Count==1)
                                {
                                    dt.Rows[i]["Result"] = dtResult.Rows[0]["Result"].ToString();
                                }

                            }
                            //dt.Columns["CustomerCD"].ColumnName =" ";
                            //dt.Columns["CustomerName"].ColumnName = " ";
                            //dt.Columns["SaleA"].ColumnName = " ";
                            dt.Columns["11"].ColumnName = strmonth[11].ToString();
                            dt.Columns["10"].ColumnName = strmonth[10].ToString();
                            dt.Columns["9"].ColumnName = strmonth[9].ToString();
                            dt.Columns["8"].ColumnName = strmonth[8].ToString();
                            dt.Columns["7"].ColumnName = strmonth[7].ToString();
                            dt.Columns["6"].ColumnName = strmonth[6].ToString();
                            dt.Columns["5"].ColumnName = strmonth[5].ToString();
                            dt.Columns["4"].ColumnName = strmonth[4].ToString();
                            dt.Columns["3"].ColumnName = strmonth[3].ToString();
                            dt.Columns["2"].ColumnName = strmonth[2].ToString();
                            dt.Columns["1"].ColumnName = strmonth[1].ToString();
                            dt.Columns["0"].ColumnName = strmonth[0].ToString();

                            DataTable dtExport = dt;
                            string folderPath = "C:\\SMS\\";
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                            SaveFileDialog savedialog = new SaveFileDialog();
                            savedialog.Filter = "Excel Files|*.xlsx;";
                            savedialog.Title = "Save";
                            savedialog.FileName = "売掛滞留一覧表印刷";
                            savedialog.InitialDirectory = folderPath;

                            savedialog.RestoreDirectory = true;
                            if (savedialog.ShowDialog() == DialogResult.OK)
                            {
                                #region Test
                                //Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
                                //excel.Application.Workbooks.Add(Type.Missing);
                                //excel.Columns.AutoFit();
                                //for(int i=3; i< dtExport.Columns.Count+1; i++)
                                //{
                                //    Microsoft.Office.Interop.Excel.Range xlRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[3, i];
                                //    //xlRange.Font.Bold = -1;
                                //    xlRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                //    xlRange.Borders.Weight = 1d;
                                //    xlRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                //    excel.Cells[3, i] = dtExport.Columns[i - 1].ColumnName;
                                //}

                                ///*For storing Each row and column value to excel sheet*/
                                //for (int i = 1; i < dtExport.Rows.Count; i++)
                                //{
                                //    for (int j = 0; j < dtExport.Columns.Count; j++)
                                //    {
                                //        if (dtExport.Rows[i][j] != null)
                                //        {
                                //            Microsoft.Office.Interop.Excel.Range xlRange = (Microsoft.Office.Interop.Excel.Range)excel.Cells[i + 2, j + 1];
                                //            xlRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                //            xlRange.Borders.Weight = 1d;
                                //            excel.Cells[i + 2, j + 1] = "'"+dtExport.Rows[i][j].ToString();
                                //        }
                                //    }
                                //}
                                //excel.Columns.AutoFit(); // Auto fix the columns size
                                //System.Windows.Forms.Application.DoEvents();
                                //string name = "売掛滞留一覧表印刷";
                                //if (Directory.Exists("C:\\CTR_Data\\")) // Folder dic
                                //{
                                //    excel.ActiveWorkbook.SaveCopyAs("C:\\CTR_Data\\" +name + ".xlsx");
                                //}
                                //else
                                //{
                                //    Directory.CreateDirectory("C:\\CTR_Data\\");
                                //    excel.ActiveWorkbook.SaveCopyAs("C:\\CTR_Data\\" + name + ".xlsx");
                                //}
                                //excel.ActiveWorkbook.Saved = true;
                                //System.Windows.Forms.Application.DoEvents();
                                #endregion

                                if (Path.GetExtension(savedialog.FileName).Contains(".xlsx"))
                                {
                                    Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
                                    Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                                    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                                    worksheet = workbook.ActiveSheet;
                                    worksheet.Name = "worksheet";
                                    Microsoft.Office.Interop.Excel.Range excelRange = worksheet.UsedRange;
                                    excelRange.Cells[6, 13].NumberFormat = "\"$\" #,##0.00"; 

                                    using (XLWorkbook wb = new XLWorkbook())
                                    {
                                        wb.Worksheets.Add(dtExport, "worksheet");
                                        wb.Worksheet("worksheet").Row(1).InsertRowsAbove(1);
                                        wb.Worksheet("worksheet").Row(1).InsertRowsAbove(1);
                                        wb.Worksheet("worksheet").Row(1).InsertRowsAbove(1);
                                        wb.Worksheet("worksheet").Cell(1, 1).Value = "年月：";
                                        wb.Worksheet("worksheet").Cell(2, 1).Value = "店舗:";
                                        wb.Worksheet("worksheet").Cell(1, 2).Value = "'" + strmonth[11].ToString();
                                        wb.Worksheet("worksheet").Cell(1, 3).Value = "～";
                                        wb.Worksheet("worksheet").Cell(1, 4).Value = "'" + strmonth[0].ToString();
                                        wb.Worksheet("worksheet").Cell(2, 2).Value = "'" + cboStore.SelectedValue.ToString();
                                        wb.Worksheet("worksheet").Cell(2, 3).Value = cboStore.Text.ToString();
                                        //wb.Worksheet("worksheet").Columns(4, 1).Delete();
                                        //wb.Worksheet("worksheet").Columns(4, 1).Hide();
                                        //wb.Worksheet("worksheet").Columns(4, 2).Hide();
                                        //wb.Worksheet("worksheet").Columns(4, 3).Hide();
                                        wb.Worksheet("worksheet").Cell(4, 1).Value = " ";
                                        wb.Worksheet("worksheet").Cell(4, 2).Value = " ";
                                        wb.Worksheet("worksheet").Cell(4, 3).Value = " ";
                                        wb.Worksheet("worksheet").Cell(4, 16).Value = "売掛月数";
                                        wb.Worksheet("worksheet").Hide();
                                        //wb.Worksheet("worksheet").Cell(3, 4).Value = "'" + strmonth[11].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 5).Value = "'" + strmonth[10].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 6).Value = "'" + strmonth[9].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 7).Value = "'" + strmonth[8].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 8).Value = "'" + strmonth[7].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 9).Value = "'" + strmonth[6].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 10).Value = "'" + strmonth[5].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 11).Value = "'" + strmonth[4].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 12).Value = "'" + strmonth[3].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 13).Value = "'" + strmonth[2].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 14).Value = "'" + strmonth[1].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 15).Value = "'" + strmonth[0].ToString();
                                        //wb.Worksheet("worksheet").Cell(3, 16).Value = "売掛月数";


                                        //wb.Worksheet("worksheet").SetShowRowColHeaders(true);
                                        wb.Worksheet("worksheet").Tables.FirstOrDefault().ShowAutoFilter = false;
                                       
                                        //wb.Worksheet("worksheet").Tables.FirstOrDefault().ShowHeaderRow = false;
                                        //wb.Worksheet("worksheet").Rows("4").Delete();
                                        wb.SaveAs(savedialog.FileName);
                                        bbl.ShowMessage("I203", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                    }
                                    Process.Start(Path.GetDirectoryName(savedialog.FileName));
                                }
                               // Process.Start(Path.GetDirectoryName(savedialog.FileName));
                            }
                        }
                        else
                        {
                            bbl.ShowMessage("E128");
                            txtDate.Focus();
                        }
                    }
              }
        }

        protected override void PrintSec()
        {
            // レコード定義を行う
            // DataTable table = new DataTable();
            ukkthbl = new UrikakekinTairyuuHyou_BL();
            string header = string.Empty;
            DataTable dtPrint;
            if (PrintMode != EPrintMode.DIRECT)
                return;
            if (ErrorCheck())
            {
                msce = new M_StoreClose_Entity();
                msce = GetStoreClose_Data();

                DateTime now = Convert.ToDateTime(txtDate.Text.ToString() + "/01 00:00:00");
                string[] strmonth=new string[12];
                for(int i=11; i>=0; i--)
                {
                    strmonth[i]= now.AddMonths(-i).ToString().Substring(0, 7);
                }


                dtPrint = ukkthbl.Select_DataToExport(msce);
                //header = "棚入れリスト";
               
                try
                {
                    CheckBeforeExport();
                    if (dtPrint == null || dtPrint.Rows.Count<=0)
                    {
                        bbl.ShowMessage("E128");
                        txtDate.Focus();
                    }
                    else
                    {
                        string customerCD = string.Empty;
                        for (int i = 0; i < dtPrint.Rows.Count; i++)
                        {
                            if (customerCD != dtPrint.Rows[i]["CustomerCD"].ToString())
                                customerCD = dtPrint.Rows[i]["CustomerCD"].ToString();

                            DataTable dtResult = dtPrint.Select("SaleA='売上' and CustomerCD='" + customerCD + "'").CopyToDataTable();
                            if (dtResult.Rows.Count == 1)
                            {
                                dtPrint.Rows[i]["Result"] = dtResult.Rows[0]["Result"].ToString();
                            }

                        }

                        //xsdファイルを保存します。

                        //①保存した.xsdはプロジェクトに追加しておきます。
                        DialogResult ret;
                        UrikakekinTairyuuHyou_Report Report = new UrikakekinTairyuuHyou_Report();

                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:

                                ret = bbl.ShowMessage("Q202");
                                if (ret == DialogResult.No)
                                {
                                    return;
                                }
                                //}

                                // 印字データをセット



                                Report.SetDataSource(dtPrint);
                                Report.Refresh();
                                Report.SetParameterValue("txtStore", cboStore.SelectedValue.ToString() + "  " + cboStore.Text);
                                Report.SetParameterValue("txtMonth11", strmonth[11].ToString());
                                Report.SetParameterValue("txtMonth10", strmonth[10].ToString());
                                Report.SetParameterValue("txtMonth9", strmonth[9].ToString());
                                Report.SetParameterValue("txtMonth8", strmonth[8].ToString());
                                Report.SetParameterValue("txtMonth7", strmonth[7].ToString());
                                Report.SetParameterValue("txtMonth6", strmonth[6].ToString());
                                Report.SetParameterValue("txtMonth5", strmonth[5].ToString());
                                Report.SetParameterValue("txtMonth4", strmonth[4].ToString());
                                Report.SetParameterValue("txtMonth3", strmonth[3].ToString());
                                Report.SetParameterValue("txtMonth2", strmonth[2].ToString());
                                Report.SetParameterValue("txtMonth1", strmonth[1].ToString());
                                Report.SetParameterValue("txtMonth0", strmonth[0].ToString());

                                if (ret == DialogResult.Yes)
                                {
                                    var previewForm = new Viewer();
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

                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                break;

                            case EPrintMode.PDF:
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
                                Report.SetDataSource(dtPrint);
                                Report.Refresh();
                                Report.SetParameterValue("txtSouko", cboStore.SelectedValue.ToString() + "  " + cboStore.Text);
                                Report.SetParameterValue("txtHeader", header);

                                bool result = OutputPDF(filePath, Report);

                                //PDF出力が完了しました。
                                bbl.ShowMessage("I202");

                                break;
                        }
                        //InsertLog(Get_L_Log_Entity(dtPrint));

                    }


                }
                finally
                {
                    txtDate.Focus();
                }
            }
        }

        public void Clear()
        {
            txtDate.Text = todayDate.Substring(0, todayDate.Length - 3); 
            cboStore.SelectedValue = StoreCD;
            txtDate.Focus();
        }

        private void UrikakekinTairyuuHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
