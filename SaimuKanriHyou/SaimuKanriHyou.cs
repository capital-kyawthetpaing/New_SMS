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
using Search;
using System.Diagnostics;
using System.IO;
using ElencySolutions.CsvHelper;
using CrystalDecisions.Shared;

namespace SaimuKanriHyou
{
    public partial class frmSaimuKanriHyou : FrmMainForm
    {
        SaimuKanriHyou_BL saimukanriBL;
        D_MonthlyDebt_Entity mde;
        M_StoreClose_Entity msce;
        CrystalDecisions.Windows.Forms.CrystalReportViewer crv;
        Viewer vr;
        DataTable dtExport;
        int chk = 0;
        public frmSaimuKanriHyou()
        {
            InitializeComponent();
            vr = new Viewer();
        }

        private void frmSaimuKanriHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "SaimuKanriHyou";
            saimukanriBL = new SaimuKanriHyou_BL();
            SetFunctionLabel(EProMode.PRINT);
            StartProgram();
            SetRequireField();
            Btn_F11.Text = "Excel(F11)";
            Btn_F10.Text = "";
            cboStoreAuthorizations.Bind(string.Empty, "2");
            cboStoreAuthorizations.SelectedValue = StoreCD;
            cboStoreAuthorizations.KeyDown += cboStoreAuthorizations_KeyDown;
            txtTargetYear.Text = System.DateTime.Now.ToString("yyyy/MM");
            txtTargetYear.Focus();
        }

        private void SetRequireField()
        {
            cboStoreAuthorizations.Require(true);
            txtTargetYear.Require(true);
        }

        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index + 1)
            {
                case 6:
                    {			
                        if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                        {
                            Clear();
                        }
                        break;
                    }
                case 11:
                    F11();
                    break;
            }
        }

        protected override void EndSec()
        {
            this.Close();
        }

        public void Clear()
        {
            Clear(panelDetail);
            cboStoreAuthorizations.SelectedValue = StoreCD;
            txtTargetYear.Text = System.DateTime.Now.ToString("yyyy/MM");
            txtTargetYear.Focus();
        }

        /// <summary>
        /// ErrorCheck before F11 and F12 
        /// </summary>
        /// <returns></returns>
        private bool ErrorCheck()
        {
            //if (!txtTargetYear.DateCheck()) //added by ETZ   // edit 
            //    return false;


            if (!RequireCheck(new Control[] { cboStoreAuthorizations, txtTargetYear }))
                return false;
         
            if (!base.CheckAvailableStores(cboStoreAuthorizations.SelectedValue.ToString())) // Check for 店舗 (ComboBox)
            {
                saimukanriBL.ShowMessage("E141");
                cboStoreAuthorizations.Focus();
                return false;
            }

            if (saimukanriBL.SimpleSelect1("43", txtTargetYear.Text + "/01").Rows.Count < 1) // Check for 対象年月
            {
                saimukanriBL.ShowMessage("E103");
                txtTargetYear.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(scVendor.Code))
            {
                DataTable dtVendor = saimukanriBL.SimpleSelect1("44", txtTargetYear.Text + "/01", scVendor.Code);
                if (dtVendor.Rows.Count < 1)
                {
                    saimukanriBL.ShowMessage("E101");
                    scVendor.SetFocus(1);
                    return false;
                }
                else scVendor.LabelText = dtVendor.Rows[0]["VendorName"].ToString();
            }
            return true;
        }

        //private bool CheckStoreCD()
        //{
        //    if (!cboStoreAuthorizations.IsExists(StoreAuthorizationsCD, "StoreAuthorization", StoreAuthorizationsChangeDate, InProgramID, cboStoreAuthorizations.SelectedValue.ToString()))
        //        return false;
        //    return true;
        //}

        private void txtTargetYear_KeyDown(object sender, KeyEventArgs e) // Check for 対象年月
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtTargetYear.Text))
                {
                    if (saimukanriBL.SimpleSelect1("43", txtTargetYear.Text + "/01").Rows.Count < 1)
                    {
                        saimukanriBL.ShowMessage("E103");
                        txtTargetYear.Focus();
                    }
                }
            }
        }

        private void scVendor_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(scVendor.Code))
                {
                    DataTable dtVendor = saimukanriBL.SimpleSelect1("44", txtTargetYear.Text + "/01", scVendor.Code);
                    if (dtVendor.Rows.Count < 1)
                    {
                        saimukanriBL.ShowMessage("E101");
                        scVendor.SetFocus(1);
                    }
                    else scVendor.LabelText = dtVendor.Rows[0]["VendorName"].ToString();
                }
            }
        }
        
        private void cboStoreAuthorizations_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!base.CheckAvailableStores(cboStoreAuthorizations.SelectedValue.ToString()))
                {
                    saimukanriBL.ShowMessage("E141");
                    cboStoreAuthorizations.MoveNext = false;
                    cboStoreAuthorizations.Focus();
                }
            }
        }

        private void F11()
        {
            if (ErrorCheck())
            {
                mde = GetMonthlyDebt_Data();
                if (chkBalancePrint.Checked == true)
                    chk = 1;
                else chk = 0;
                dtExport = saimukanriBL.D_MonthlyDebt_CSV_Report(mde,chk);
                
                if (dtExport.Rows.Count > 0)
                {
                    CheckBeforeExport();
                    DataTable dtCsv = new DataTable();
                    dtCsv = CreateDatatable();
                    for (int i = 0; dtExport.Rows.Count > i; i++)
                    {
                        dtCsv.Rows.Add();
                        dtCsv.Rows[i]["年月"] = txtTargetYear.Text;
                        dtCsv.Rows[i]["店舗CD"] = dtExport.Rows[i]["StoreCD"].ToString();
                        dtCsv.Rows[i]["店舗名"] = dtExport.Rows[i]["StoreName"].ToString();
                        dtCsv.Rows[i]["支払先CD"] = dtExport.Rows[i]["PayeeCD"].ToString();
                        dtCsv.Rows[i]["支払先名"] = dtExport.Rows[i]["VendorName"].ToString();
                        dtCsv.Rows[i]["前月債務残額"] = dtExport.Rows[i]["LastBalanceGaku"].ToString();
                        dtCsv.Rows[i]["仕入額"] = dtExport.Rows[i]["HontaiGaku"].ToString();
                        dtCsv.Rows[i]["消費税額"] = dtExport.Rows[i]["TaxGaku"].ToString();
                        dtCsv.Rows[i]["当月支払額"] = dtExport.Rows[i]["PayGaku"].ToString();
                        dtCsv.Rows[i]["うち相殺額"] = dtExport.Rows[i]["OffsetGaku"].ToString();
                        dtCsv.Rows[i]["当月債務額"] = dtExport.Rows[i]["BalanceGaku"].ToString();
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
                else
                {
                    saimukanriBL.ShowMessage("E128");
                }
            }
        }

        protected override void PrintSec()
        {
            if (PrintMode != EPrintMode.DIRECT)
                return;

            if (ErrorCheck())
            {
                // レコード定義を行う
                mde = GetMonthlyDebt_Data();
                if (chkBalancePrint.Checked == true)
                    chk = 1;
                else chk = 0;


                CheckBeforeExport();
                dtExport = saimukanriBL.D_MonthlyDebt_CSV_Report(mde, chk);
                if(dtExport.Rows.Count > 0 )
                {
                    //CheckBeforeExport();
                    try
                    {
                        SaimuKanriKyou_Report smkh_Report = new SaimuKanriKyou_Report();
                        DialogResult DResult;
                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:
                                DResult = bbl.ShowMessage("Q201");
                                if (DResult == DialogResult.No)
                                {
                                    return;
                                }
                                // 印字データをセット
                                smkh_Report.SetDataSource(dtExport);
                                smkh_Report.Refresh();
                                smkh_Report.SetParameterValue("lblYearMonth", txtTargetYear.Text);
                                smkh_Report.SetParameterValue("lblStore", dtExport.Rows[0]["StoreCD"].ToString() + "  " + dtExport.Rows[0]["StoreName"].ToString());
                                smkh_Report.SetParameterValue("lblToday", dtExport.Rows[0]["Today"].ToString() + "  " + dtExport.Rows[0]["Now"].ToString());
                                crv = vr.CrystalReportViewer1;
                                crv.ReportSource = smkh_Report;
                                //vr.ShowDialog();

                                try
                                {

                                    //  crv = vr.CrystalReportViewer1;
                                }
                                catch (Exception ex)
                                {
                                    var msg = ex.Message;
                                }
                                //out log before print
                                if (DResult == DialogResult.Yes)
                                {
                                    //印刷処理プレビュー
                                    vr.CrystalReportViewer1.ShowPrintButton = true;
                                    vr.CrystalReportViewer1.ReportSource = smkh_Report;

                                    vr.ShowDialog();

                                }
                                else
                                {
                                    //int marginLeft = 360;
                                    CrystalDecisions.Shared.PageMargins margin = smkh_Report.PrintOptions.PageMargins;
                                    margin.leftMargin = DefaultMargin.Left; // mmの指定をtwip単位に変換する
                                    margin.topMargin = DefaultMargin.Top;
                                    margin.bottomMargin = DefaultMargin.Bottom;//mmToTwip(marginLeft);
                                    margin.rightMargin = DefaultMargin.Right;
                                    smkh_Report.PrintOptions.ApplyPageMargins(margin);     /// Error Now
                                    // プリンタに印刷
                                    System.Drawing.Printing.PageSettings ps;
                                    try
                                    {
                                        System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();

                                        CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();

                                        System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();



                                        smkh_Report.PrintOptions.PrinterName = "\\\\dataserver\\Canon LBP2900";
                                        System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                                        smkh_Report.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;

                                        smkh_Report.PrintOptions.PrinterDuplex = PrinterDuplex.Simplex;

                                        smkh_Report.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
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
                        txtTargetYear.Focus();
                    }
                }
                else
                {
                    saimukanriBL.ShowMessage("E128");
                }
            }
        }

        private void CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();

            if (saimukanriBL.M_StoreClose_Check(msce, "2").Rows.Count > 0)
            {
                string ProgramID = "GetsujiSaimuKeisanSyori";
                RunConsole(ProgramID, msce.FiscalYYYYMM);
            }
        }

        private void RunConsole(string programID, string YYYYMM)
        {
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "1";
            //string cmdLine =  InOperatorCD + " " + Login_BL.GetHostName() + " " + StoreCD + " " + Mode + " " + YYYYMM;//parameter
            string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + StoreCD + " " + Mode + " " + YYYYMM;
            Process p=System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");

            p.WaitForExit();          

        }

        #region GetDataInfo
        private D_MonthlyDebt_Entity GetMonthlyDebt_Data()
        {
            mde = new D_MonthlyDebt_Entity()
            {
                ChangeDate =  txtTargetYear.Text + "/01",
                YYYYMM = txtTargetYear.Text.Replace("/",""),
                StoreCD = cboStoreAuthorizations.SelectedValue.ToString(),
                PayeeCD = scVendor.Code
            };
            return mde;
        }

        private M_StoreClose_Entity GetStoreClose_Data()
        {
            msce = new M_StoreClose_Entity()
            {
                StoreCD = cboStoreAuthorizations.SelectedValue.ToString(),
                FiscalYYYYMM = txtTargetYear.Text.Replace("/", ""),
            };
            return msce;
        }

        private L_Log_Entity Get_L_Log_Entity()
        {
            L_Log_Entity lle = new L_Log_Entity();
           
            if (chkBalancePrint.Checked == true)
                chk = 1;
            else chk = 0;
          
            lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "",
                KeyItem = txtTargetYear.Text + "," + cboStoreAuthorizations.AccessibilityObject.Name + "," + chk.ToString()
            };

            return lle;
        }
        #endregion

        private void frmSaimuKanriHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private DataTable CreateDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("年月");
            dt.Columns.Add("店舗CD");
            dt.Columns.Add("店舗名");
            dt.Columns.Add("支払先CD");
            dt.Columns.Add("支払先名");
            dt.Columns.Add("前月債務残額");
            dt.Columns.Add("仕入額");
            dt.Columns.Add("消費税額");
            dt.Columns.Add("当月支払額");
            dt.Columns.Add("うち相殺額");
            dt.Columns.Add("当月債務額");
            return dt;
        }

        private void scVendor_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTargetYear.Text))
            {
                int year = Convert.ToInt32(txtTargetYear.Text.Substring(0, 4));
                int month = Convert.ToInt32(txtTargetYear.Text.Substring(5, 2));
                string lastday = "/" + DateTime.DaysInMonth(year, month).ToString();
                scVendor.ChangeDate = txtTargetYear.Text + lastday;
                scVendor.Value1 = "2";
            }
                    
        }
    }
    
}
