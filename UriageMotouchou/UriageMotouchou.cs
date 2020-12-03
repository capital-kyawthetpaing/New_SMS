using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using CrystalDecisions.Shared;
using Entity;

namespace UriageMotouchou
{
    public partial class UriageMotouchou : FrmMainForm
    {
        UriageMotochou_Entity ume;
        Base_BL bbl;
        UriageMotochou_BL umbl;
        L_Log_Entity lle ;
        DataTable dtReport,dtCheck;
        CrystalDecisions.Windows.Forms.CrystalReportViewer crvr;
        M_StoreClose_Entity msce;
        public UriageMotouchou()
        {
            InitializeComponent();
            ume = new UriageMotochou_Entity();
            lle = new L_Log_Entity();
            bbl = new Base_BL();
            umbl = new UriageMotochou_BL();
            dtReport = new DataTable();
            dtCheck = new DataTable();
            
        }

        private void UriageMotouchou_Load(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            SetFunctionLabel(EProMode.PRINT);
            base.Btn_F9.Text = "";
            base.Btn_F10.Text = "";
            base.Btn_F11.Text = "";
            StartProgram();
            BindStore();
            chkYes.Checked = true;           
            //RequireField
            txtTagetFrom.Require(true);
            cboStore.Require(true);

        }

        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index + 1)
            {
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear(Panel_Details);
                        chkYes.Checked = true;
                        txtTagetFrom.Focus();
                        cboStore.SelectedValue = StoreCD;
                    }
                    break;
            }
        }

        #region 店舗にとってデータを取る
        private void BindStore()
        {
            cboStore.Bind(string.Empty, "2");
            cboStore.SelectedValue = StoreCD;
        }
        #endregion
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtTagetFrom, cboStore }))
                return false;

            if(!chkYes.Checked && !chkNo.Checked)
            {
                bbl.ShowMessage("E111");
                return false;
            }

            if (!string.IsNullOrEmpty(sc_Customer.TxtCode.Text))
            {
                if (!sc_Customer.IsExists(2))
                {
                    umbl.ShowMessage("E101");
                    sc_Customer.SetFocus(1);
                    return false;
                }
            }
            return true;
        }

        private M_StoreClose_Entity GetStoreClose_Data()
        {
            msce = new M_StoreClose_Entity()
            {
                StoreCD = cboStore.SelectedValue.ToString(),
                FiscalYYYYMM = txtTagetFrom.Text.Replace("/", ""),
            };
            return msce;
        }
        #region 印刷するボタンPrintSec
        protected override void PrintSec()
        {
            if (ErrorCheck())
            {
                if (PrintMode != EPrintMode.DIRECT)
                    return;

                ume = GetDataInfo();
                CheckBeforeExport();
                dtReport = umbl.UriageMotochou_PrintSelect(ume);
                if (dtReport.Rows.Count > 0)
                {
                    //msce = new M_StoreClose_Entity();
                    //msce = GetStoreClose_Data();
                    //if (umbl.M_StoreClose_Check(msce, "1").Rows.Count > 0)
                    //{
                    //    string ProgramID = "GetsujiZaikoKeisanSyori";
                    //    OpenForm(ProgramID, msce.FiscalYYYYMM);
                    //    PrintDataSelect();
                    //}
                    PrintDataSelect();
                }
                else
                {
                    umbl.ShowMessage("E128");
                    txtTagetFrom.Focus();
                }
            }
        }
        private void CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();
            if (umbl.M_StoreClose_Check(msce, "4").Rows.Count > 0)
            {
                string ProgramID = "GetsujiSaikenKeisanSyori";
                RunConsole(ProgramID, msce.FiscalYYYYMM);
            }
        }
        //private void OpenForm(string programID, string YYYYMM)
        //{
        //    System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //    string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
        //    string Mode = "1";
        //    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + StoreCD + " " + " " + Mode + " " + YYYYMM;
        //    System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
        //}
        private void RunConsole(string programID, string YYYYMM)
        {
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "1";
            string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + StoreCD + " " + " " + Mode + " " + YYYYMM;
            Process p= System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            p.WaitForExit();
        }
        private void PrintDataSelect()
        {
            try
            {
                UriageMotouchou_Report umtc_Report = new UriageMotouchou_Report();
                DialogResult DResult;
                switch (PrintMode)
                {
                    case EPrintMode.DIRECT:
                        DResult = bbl.ShowMessage("Q201");
                        if (DResult == DialogResult.No)
                        {
                            return;
                        }
                        umtc_Report.SetDataSource(dtReport);
                        umtc_Report.Refresh();
                        umtc_Report.SetParameterValue("YYYYMMF", txtTagetFrom.Text);
                        umtc_Report.SetParameterValue("PrintDateTime", System.DateTime.Now.ToString("yyyy/MM/dd") + " " + System.DateTime.Now.ToString("HH:mm"));
                        umtc_Report.SetParameterValue("CustomerCD", sc_Customer.TxtCode.Text);
                        umtc_Report.SetParameterValue("CustName", sc_Customer.LabelText);
                        umtc_Report.SetParameterValue("StoreCD", cboStore.Text);
                        //crvr = vr.CrystalReportViewer1;

                        if (DResult == DialogResult.Yes)
                        {
                            var vr = new Viewer();
                            vr.CrystalReportViewer1.ShowPrintButton = true;
                            vr.CrystalReportViewer1.ReportSource = umtc_Report;
                            vr.ShowDialog();
                        }
                        else
                        {
                            //int marginLeft = 360;
                            CrystalDecisions.Shared.PageMargins margin = umtc_Report.PrintOptions.PageMargins;
                            margin.leftMargin = DefaultMargin.Left; // mmの指定をtwip単位に変換する
                            margin.topMargin = DefaultMargin.Top;
                            margin.bottomMargin = DefaultMargin.Bottom;//mmToTwip(marginLeft);
                            margin.rightMargin = DefaultMargin.Right;
                            umtc_Report.PrintOptions.ApplyPageMargins(margin);     /// Error Now
                            // プリンタに印刷
                            System.Drawing.Printing.PageSettings ps;
                            try
                            {
                                System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();

                                CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();

                                System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();



                                umtc_Report.PrintOptions.PrinterName = "\\\\dataserver\\Canon LBP2900";
                                System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                                umtc_Report.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;

                                umtc_Report.PrintOptions.PrinterDuplex = PrinterDuplex.Simplex;

                                umtc_Report.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
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
                txtTagetFrom.Focus();
            }
        }

        #endregion

        private L_Log_Entity Get_L_Log_Entity()
        {
            lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "",
                KeyItem = txtTagetFrom.Text + "_S " + sc_Customer.TxtCode.Text + " " + cboStore.SelectedValue.ToString()
            };

            return lle;
        }

        private UriageMotochou_Entity GetDataInfo()
        {
            string Todate = string.Empty;
            //if (!string.IsNullOrWhiteSpace(txtTargetTo.Text))
            //{
            //    int year = Convert.ToInt32(txtTargetTo.Text.Substring(0, 4));
            //    int month = Convert.ToInt32(txtTargetTo.Text.Substring(5, 2));
            //    string lastday = "/" + DateTime.DaysInMonth(year, month).ToString();
            //    Todate = txtTargetTo.Text + lastday;
            //}

            ume = new UriageMotochou_Entity()
            {
                YYYYMMFrom = txtTagetFrom.Text.Replace("/", ""),
                //YYYYMMTo = txtTargetTo.Text.Replace("/", ""),
                CustomerCD = sc_Customer.TxtCode.Text,
                StoreCD = cboStore.SelectedValue.ToString(),
                TargetDateFrom = txtTagetFrom.Text + "/01",
                //TargetDateTo = Todate,
                ChkValue =CheckValue()
            };
            return ume;
        }

        public string CheckValue()
        {
            string chk = string.Empty;

             if(chkYes.Checked && !chkNo.Checked)
            {
                chk ="1";
                return chk;
            }
            else if (chkNo.Checked && !chkYes.Checked)
            {
                chk = "2";
                return chk;
            }
            else
            {
                return string.Empty;
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void UriageMotouchou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void sc_Customer_Enter(object sender, EventArgs e)
        {
            sc_Customer.Value1 = "1";
            //sc_Customer.Value2 = cboStore.SelectedValue.Equals("-1") ? "" : cboStore.SelectedValue.ToString();
            // sc_Customer.ChangeDate = txtTagetFrom.Text;//月初
            sc_Customer.ChangeDate = umbl.GetDate(txtTagetFrom.Text);
        }
        private void sc_Customer_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sc_Customer.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(sc_Customer.TxtCode.Text))
                {
                    if (sc_Customer.SelectData())
                    {
                        sc_Customer.Value1 = sc_Customer.TxtCode.Text;
                        sc_Customer.Value2 = sc_Customer.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        sc_Customer.SetFocus(1);
                    }
                }
            }
        }
    }
}
