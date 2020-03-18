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

            SetFunctionLabel(EProMode.MENTE);
            SetFunctionLabel(EProMode.PRINT);
            StartProgram();

            ModeVisible = false;
            Btn_F2.Text = string.Empty;
            Btn_F9.Text = string.Empty;
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
                    ExcelExport(); break;
                case 11:
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
                if(!CheckBeforeExport())
                {
                    if (bbl.ShowMessage("Q205") == DialogResult.Yes)
                    {
                        msce = new M_StoreClose_Entity();
                        msce = GetStoreClose_Data();
                        DataTable dt=ukkthbl.Select_DataToExport(msce);
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
            if (ErrorCheck())
            {
                msce = new M_StoreClose_Entity();
                msce = GetStoreClose_Data();
                
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
                           // Report.SetParameterValue("txtHeader", header);

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
            txtDate.Text = string.Empty;
            cboStore.SelectedValue = StoreCD;
        }
    }
}
