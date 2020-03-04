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
            //cboStore.Bind(string.Empty);
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
        //private void BindStore()
        //{
        //    cboStore.Bind(string.Empty, "2");
        //    cboStore.SelectedValue = StoreCD;
        //}
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
                    //CheckBeforeExport();
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
                                szy_Report.SetParameterValue("lblDate", txtTargetDateFrom.Text);
                                szy_Report.SetParameterValue("lblSouko", cboStore.SelectedValue.ToString() + "   " + cboStore.AccessibilityObject.Name);
                                szy_Report.SetParameterValue("lblToday", dt.Rows[0]["Today"].ToString() + "  " + dt.Rows[0]["Now"].ToString());
                                try
                                {

                                }
                                catch (Exception ex)
                                {
                                    var msg = ex.Message;
                                }
                                //out log before print
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
        private void RunConsole(string FiscalYYYYMM)
        {
            string programID = "GetsujiZaikoKeisanSyori";
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "1";
            string cmdLine = " " + InOperatorCD + " " + Login_BL.GetHostName() + " " + StoreCD + " " + " " + Mode + " " +FiscalYYYYMM;//parameter
            System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
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
