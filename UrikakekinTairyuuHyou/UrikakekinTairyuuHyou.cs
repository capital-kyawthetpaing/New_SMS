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
            Btn_F2.Text = string.Empty;
            Btn_F9.Text = string.Empty;
            Btn_F10.Text = string.Empty;

            Btn_F11.Text = "出力(F10)";

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
                    ExcelExport(); break;
              
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

        public bool CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();
            if (ukkthbl.M_StoreClose_Check(msce, "1").Rows.Count > 0)
            {
                //    if (bbl.ShowMessage("Q205") == DialogResult.Yes)
                //    {

                //    }
                return true;
            }
            return false;
        }

        public void ExcelExport()
        {
            if(ErrorCheck())
            {
                if(CheckBeforeExport())
                {
                    if (bbl.ShowMessage("Q205") == DialogResult.Yes)
                    {
                        msce = new M_StoreClose_Entity();
                        msce = GetStoreClose_Data();

                        DateTime now = Convert.ToDateTime(txtDate.Text.ToString() + "/01 00:00:00");
                        string[] strmonth = new string[12];
                        for (int i = 11; i >= 0; i--)
                        {
                            strmonth[i] = now.AddMonths(-i).ToString().Substring(0, 7);
                        }

                        DataTable dt=ukkthbl.Select_DataToExport(msce);
                        //DataRow dr = dt.NewRow();
                        //dr["CustomerCD"] = "";
                        //dr["CustomerName"] = "";
                        //dr["SaleA"] = "";
                        //dr["11"] = strmonth[11].ToString();
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
                            DataTable dtExport = dt;
                           // dtExport = ChangeDataColumnName(dtExport);
                            string folderPath = "C:\\SES\\";
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
                                if (Path.GetExtension(savedialog.FileName).Contains(".xlsx"))
                                {
                                    Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
                                    Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                                    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                                    worksheet = workbook.ActiveSheet;
                                    worksheet.Name = "worksheet";
                                    using (XLWorkbook wb = new XLWorkbook())
                                    {
                                        wb.Worksheets.Add(dtExport, "worksheet");
                                        wb.Worksheet("worksheet").Row(1).InsertRowsAbove(1);
                                        wb.Worksheet("worksheet").Row(1).InsertRowsAbove(1);
                                        //wb.Worksheet("worksheet").Row(1).InsertRowsAbove(1);
                                        wb.Worksheet("worksheet").Cell(1, 1).Value = "年月：";
                                        wb.Worksheet("worksheet").Cell(2, 1).Value = "店舗:";
                                        wb.Worksheet("worksheet").Cell(1, 2).Value = strmonth[11].ToString();
                                        wb.Worksheet("worksheet").Cell(1, 3).Value = "～";
                                        wb.Worksheet("worksheet").Cell(1, 4).Value = strmonth[0].ToString();
                                        wb.Worksheet("worksheet").Cell(2, 2).Value = cboStore.SelectedValue.ToString();
                                        wb.Worksheet("worksheet").Cell(2, 3).Value = cboStore.Text.ToString();

                                        wb.Worksheet("worksheet").Cell(3, 4).Value = strmonth[11].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 5).Value = strmonth[10].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 6).Value = strmonth[9].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 7).Value = strmonth[8].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 8).Value = strmonth[7].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 9).Value = strmonth[6].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 10).Value = strmonth[5].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 11).Value = strmonth[4].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 12).Value = strmonth[3].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 13).Value = strmonth[2].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 14).Value = strmonth[1].ToString();
                                        wb.Worksheet("worksheet").Cell(3, 15).Value = strmonth[0].ToString();

                                       
                                        //wb.Worksheet("worksheet").SetShowRowColHeaders(false);
                                        wb.Worksheet("worksheet").Tables.FirstOrDefault().ShowAutoFilter = false;
                                        wb.Worksheet("worksheet").Tables.FirstOrDefault().ShowHeaderRow = false;
                                        wb.SaveAs(savedialog.FileName);
                                        bbl.ShowMessage("I203", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                    }
                                    Process.Start(Path.GetDirectoryName(savedialog.FileName));
                                }
                            }
                        }
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
                    if (dtPrint == null)
                    {
                        return;
                    }
                    //xsdファイルを保存します。

                    //①保存した.xsdはプロジェクトに追加しておきます。
                    DialogResult ret;
                    UrikakekinTairyuuHyou_Report Report = new UrikakekinTairyuuHyou_Report();

                    switch (PrintMode)
                    {
                        case EPrintMode.DIRECT:

                            ret = bbl.ShowMessage("Q202");
                            if (ret == DialogResult.Cancel)
                            {
                                return;
                            }
                            //}

                            // 印字データをセット

                            Report.SetDataSource(dtPrint);
                            Report.Refresh();
                            Report.SetParameterValue("txtStore", cboStore.SelectedValue.ToString() + "  " + cboStore.Text);
                            Report.SetParameterValue("txtMonth11",strmonth[11].ToString());
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
                finally
                {

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
