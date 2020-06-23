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
using DL;
using CrystalDecisions.Shared;
using System.IO;
using ElencySolutions.CsvHelper;
using System.Diagnostics;

namespace SaikenKanriHyou
{
    public partial class SaikenKanriHyou : FrmMainForm
    {
        Base_BL bbl = new Base_BL();
        D_MonthlyClaims_Entity dmc_e = new D_MonthlyClaims_Entity();
        SaikenKanriHyou_BL skh_bl;
        DataTable dtResult, dtCSV, dtlog, dtS_check;
        string targetDate = string.Empty;
        CrystalDecisions.Windows.Forms.CrystalReportViewer crvr;
        Viewer vr;
        public SaikenKanriHyou()
        {
            try
            {
                InitializeComponent();
                dtResult = new DataTable();
                skh_bl = new SaikenKanriHyou_BL();
                vr = new Viewer();
                dtlog = new DataTable();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void SaikenKanriHyou_Load(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            SetFunctionLabel(EProMode.PRINT);
            base.Btn_F9.Text = "";
            base.Btn_F10.Text = "";
            base.Btn_F11.Text = "出力(F11)";
            StartProgram();
            cbo_Store.Require(true);
            txtTargetdate.Require(true);
            rdo_BillAddress.Checked = true;
            BindStore();
            // this.cbo_Store.SelectedIndexChanged += Cbo_Store_SelectedIndexChanged;
            cbo_Store.KeyDown += cbo_Store_KeyDown; // 2020-06-19
            txtTargetdate.Text = System.DateTime.Now.ToString("yyyy/MM");  // 2020-06-19
            txtTargetdate.Focus();
        }
        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index + 1)
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

        #region CSVファイル出力
        private void F11()
        {
            if (ErrorCheck())
            {

                M_StoreCheck();//exeRun

                dtCSV = new DataTable();
                dtCSV = CheckData();
                dtCSV.Columns["CustomerCD"].ColumnName = "顧客CD";

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
                        savedialog.FileName = "債権管理表";
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
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        #endregion

        #region Update Case[Console RunTime]
        private void M_StoreCheck()
        {
            dmc_e = GetDataInfo();
            dtS_check = new DataTable();

            dtS_check = skh_bl.M_StoreCheck_Select(dmc_e, 1);//M_StoreClose_(StoreCD)Datacheck
            if (dtS_check.Rows.Count > 0)
            {
                string ProgramID = "GetsujiSaikenKeisanSyori";

                //残す部分
                //NoFilePathcase
                OpenForm(ProgramID, dmc_e.YYYYMM);
                //月次処理（債権集計処理）を起動する                    
            }

        }
        #endregion
        private void OpenForm(string programID, string YYYYMM)
        {
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "1";
            string cmdLine =  InOperatorCD + " " + Login_BL.GetHostName() + " " + StoreCD + " " + Mode + " " + YYYYMM;//parameter
            try
            {
                System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            }
            catch
            {
                //skh_bl.ShowMessage("E138");
            }

        }

        #region 店舗にとってデータを取る
        private void BindStore()
        {
            cbo_Store.Bind(string.Empty, "2");
            cbo_Store.SelectedValue = StoreCD;
        }
        #endregion

        /// <summary>
        /// <Remark>Comboで選択する場合、許可するかどうかエラー　チェックする事</Remark>
        /// <Remark>許可ない場合　エラー「E141」になる</Remark>
        /// </summary>
        //private void Cbo_Store_SelectedIndexChanged(object sender, EventArgs e) //2020-06-19 saw
        //{
        //    //店舗権限のチェック、引数で処理可能店舗の配列をセットしたい
        //    if (!cbo_Store.SelectedValue.Equals("-1"))
        //    {
        //        if (!base.CheckAvailableStores(cbo_Store.SelectedValue.ToString()))
        //        {
        //            skh_bl.ShowMessage("E141");
        //            cbo_Store.Focus();
        //        }
        //    }
        //    else
        //    {
        //        skh_bl.ShowMessage("E102");
        //        cbo_Store.Focus();
        //    }

        //}

        private void cbo_Store_KeyDown(object sender, KeyEventArgs e) // 2020-06-19 saw
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!base.CheckAvailableStores(cbo_Store.SelectedValue.ToString()))
                {
                    skh_bl.ShowMessage("E141");
                    cbo_Store.MoveNext = false;
                    cbo_Store.Focus();
                }
            }
        }
        #region DataErrorCheck
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtTargetdate, cbo_Store }))
            {
                return false;
            }

            //店舗権限のチェック、引数で処理可能店舗の配列をセットしたい
            if (!base.CheckAvailableStores(cbo_Store.SelectedValue.ToString()))
            {
                skh_bl.ShowMessage("E141");
                cbo_Store.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(sc_Customer.TxtCode.Text))
            {
                DataTable dt = new DataTable(); //2020-06-22 saw
                dt = skh_bl.SimpleSelect1("67", sc_Customer.ChangeDate, sc_Customer.Code); //2020-06-22 saw
                // if (!sc_Customer.IsExists(1))
                if (dt.Rows.Count < 1)
                {
                    skh_bl.ShowMessage("E101");
                    sc_Customer.LabelText = string.Empty;
                    sc_Customer.SetFocus(1);
                    return false;
                }
                else //2020-06-22 saw
                {
                    if (rdo_BillAddress.Checked == true && dt.Rows[0]["CollectFLG"].ToString().Equals("0"))
                    {
                        skh_bl.ShowMessage("E242");
                        sc_Customer.LabelText = string.Empty;
                        sc_Customer.SetFocus(1);
                        return false;
                    }
                    else if (rdo_Sale.Checked == true && dt.Rows[0]["BillingFLG"].ToString().Equals("0"))
                    {
                        skh_bl.ShowMessage("E243");
                        sc_Customer.LabelText = string.Empty;
                        sc_Customer.SetFocus(1);
                        return false;
                    }
                    //else
                    //{
                    //    sc_Customer.Value1 = sc_Customer.TxtCode.Text;
                    //    sc_Customer.Value2 = sc_Customer.LabelText;
                    //}
                }
            }

            return true;
        }
        #endregion

        #region 印刷するボタンPrintSec
        protected override void PrintSec()
        {
            if (PrintMode != EPrintMode.DIRECT)
                return;

            if (ErrorCheck())
            {
                // レコード定義を行う
                dtResult = CheckData();
                M_StoreCheck();
                //dmc_e = GetDataInfo();
                //dtResult = skh_bl.D_MonthlyClaims_Select(dmc_e);

                //if (dtResult == null)
                //{
                //    return;
                //}
                if (dtResult.Rows.Count > 0) // 2020-06-19 saw
                {
                    //exeRun
                    
                    try
                    {
                        SaikenKanriHyou_Report skh_Report = new SaikenKanriHyou_Report();
                        DialogResult DResult;
                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:
                                DResult = bbl.ShowMessage("Q201");
                                if (DResult == DialogResult.Cancel)
                                {
                                    return;
                                }
                                // 印字データをセット
                                skh_Report.SetDataSource(dtResult);
                                skh_Report.Refresh();
                                skh_Report.SetParameterValue("YYYYMM", txtTargetdate.Text);
                                skh_Report.SetParameterValue("PrintDateTime", System.DateTime.Now.ToString("yyyy/MM/dd") + " " + System.DateTime.Now.ToString("hh:mm"));


                                crvr = vr.CrystalReportViewer1;
                                //out log before print
                                if (DResult == DialogResult.Yes)
                                {
                                    //印刷処理プレビュー
                                    vr.CrystalReportViewer1.ShowPrintButton = true;
                                    vr.CrystalReportViewer1.ReportSource = skh_Report;
                                    vr.ShowDialog();

                                }
                                else
                                {
                                    //int marginLeft = 360;
                                    CrystalDecisions.Shared.PageMargins margin = skh_Report.PrintOptions.PageMargins;
                                    margin.leftMargin = DefaultMargin.Left; // mmの指定をtwip単位に変換する
                                    margin.topMargin = DefaultMargin.Top;
                                    margin.bottomMargin = DefaultMargin.Bottom;//mmToTwip(marginLeft);
                                    margin.rightMargin = DefaultMargin.Right;
                                    skh_Report.PrintOptions.ApplyPageMargins(margin);     /// Error Now
                                    // プリンタに印刷
                                    System.Drawing.Printing.PageSettings ps;
                                    try
                                    {
                                        System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();

                                        CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();

                                        System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();



                                        skh_Report.PrintOptions.PrinterName = "\\\\dataserver\\Canon LBP2900";
                                        System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                                        skh_Report.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;

                                        skh_Report.PrintOptions.PrinterDuplex = PrinterDuplex.Simplex;

                                        skh_Report.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
                                        // Print the report. Set the startPageN and endPageN 
                                        // parameters to 0 to print all pages. 
                                        //Report.PrintToPrinter(1, false, 0, 0);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                break;

                        }

                        //プログラム実行履歴
                        InsertLog(Get_L_Log_Entity());


                    }
                    finally
                    {
                        //画面はそのまま
                        txtTargetdate.Focus();
                    }
                }
            }
        }

        #endregion
        private D_MonthlyClaims_Entity GetDataInfo()
        {
            dmc_e = new D_MonthlyClaims_Entity()
            {
                TargetDate = skh_bl.GetDate(txtTargetdate.Text),
                CustomerCD = sc_Customer.TxtCode.Text,
                PrintType = rdo_BillAddress.Checked ? "1" : "2",
                StoreCD = cbo_Store.SelectedValue.ToString(),
                chk_do = chk_Check.Checked ? "1" : "0",
                YYYYMM = txtTargetdate.Text.Replace("/", "")

            };
            return dmc_e;
        }
        #region SelectData
        private DataTable CheckData()
        {
            DataTable dt = null;
            if (ErrorCheck())
            {
                dmc_e = GetDataInfo();
                dt = skh_bl.D_MonthlyClaims_Select(dmc_e);
                //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
                if (dt.Rows.Count == 0)
                {
                    bbl.ShowMessage("E128");
                    //return null;
                }
            }

            return dt;
        }
        #endregion
        private L_Log_Entity Get_L_Log_Entity()
        {

            L_Log_Entity lle = new L_Log_Entity();
            DataTable table = CheckData();
            string item = txtTargetdate.Text;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                item += "," + table.Rows[i]["CustomerCD"].ToString();
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
        private void sc_Customer_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                targetDate = !string.IsNullOrWhiteSpace(txtTargetdate.Text) ? skh_bl.GetDate(txtTargetdate.Text) : bbl.GetDate();
                sc_Customer.ChangeDate = targetDate;
                if (!string.IsNullOrEmpty(sc_Customer.TxtCode.Text))
                {
                    DataTable dt = new DataTable();
                    dt = skh_bl.SimpleSelect1("67", targetDate, sc_Customer.Code);
                    if (dt.Rows.Count < 1)
                    {
                        skh_bl.ShowMessage("E101");
                        sc_Customer.LabelText = string.Empty;
                        sc_Customer.SetFocus(1);
                    }
                    else
                    {
                        if (rdo_BillAddress.Checked == true && dt.Rows[0]["CollectFLG"].ToString().Equals("0"))
                        {
                            skh_bl.ShowMessage("E242");
                            sc_Customer.LabelText = string.Empty;
                            sc_Customer.SetFocus(1);
                        }
                        else if (rdo_Sale.Checked == true && dt.Rows[0]["BillingFLG"].ToString().Equals("0"))
                        {
                            skh_bl.ShowMessage("E243");
                            sc_Customer.LabelText = string.Empty;
                            sc_Customer.SetFocus(1);
                        }
                        else
                        {
                            //sc_Customer.Value1 = sc_Customer.TxtCode.Text;
                            //sc_Customer.Value2 = sc_Customer.LabelText;
                            sc_Customer.Code = dt.Rows[0]["CustomerCD"].ToString();
                            sc_Customer.LabelText = dt.Rows[0]["CustomerName"].ToString();
                        }
                    }
                }
            }
        }
        private void sc_Customer_Enter(object sender, EventArgs e)
        {
            if (rdo_BillAddress.Checked)
                sc_Customer.Value1 = "2";
            else if (rdo_Sale.Checked)
                sc_Customer.Value1 = "1";

            //sc_Customer.ChangeDate = skh_bl.GetDate(txtTargetdate.Text);
            sc_Customer.ChangeDate = !string.IsNullOrWhiteSpace(txtTargetdate.Text) ? skh_bl.GetDate(txtTargetdate.Text) : bbl.GetDate(); //2020-06-22 saw
            sc_Customer.Value2 = cbo_Store.SelectedValue.Equals("-1") ? "" : cbo_Store.SelectedValue.ToString();
        }

        private void SaikenKanriHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void Clear()
        {
            Clear(panelDetail);
            cbo_Store.SelectedValue = StoreCD;
            rdo_BillAddress.Checked = true;
            txtTargetdate.Text = System.DateTime.Now.ToString("yyyy/MM");
            txtTargetdate.Focus();

        }
    }
}
