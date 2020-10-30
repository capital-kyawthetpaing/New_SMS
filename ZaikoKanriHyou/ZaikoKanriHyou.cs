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
using CrystalDecisions.Shared;

namespace ZaikoKanriHyou
{
    public partial class ZaikoKanriHyou : FrmMainForm
    {
        ZaikoKanriHyou_BL zkhbl;
        DataTable dt;
        CrystalDecisions.Windows.Forms.CrystalReportViewer crv;
        D_Purchase_Details_Entity dpde;
        D_MonthlyStock_Entity dmse;
        M_StoreClose_Entity msce;
        M_StoreAuthorizations_Entity msae = new M_StoreAuthorizations_Entity();
        int chk = 0;
        
        public ZaikoKanriHyou()
        {
            InitializeComponent();
            zkhbl = new ZaikoKanriHyou_BL();
        }

        private void ZaikoKanriHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "ZaikoKanriHyou";
            StartProgram();
            SetRequiredField();
            SetFunctionLabel(EProMode.PRINT);
            Btn_F10.Text = "";
            cboSouko.Bind(string.Empty, "");
            cboSouko.SelectedValue = SoukoCD;
            scMakerShohinCD.CodeWidth = 600;
            txtTargetDate.Text = DateTime.Now.ToString("yyyy/MM");
            Btn_F11.Text = "";
        }

        private void SetRequiredField()
        {
            txtTargetDate.Require(true);
        }

        protected override void EndSec()
        {
            this.Close();
        }
        public void Clear()
        {
            Clear(panelDetail);
            txtTargetDate.Text = DateTime.Now.ToString("yyyy/MM");
            cboSouko.SelectedValue = SoukoCD;
            txtTargetDate.Focus();
        }
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);
            switch(Index+1)
            {
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear();
                    }
                    break;
            }
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtTargetDate}))
                return false;
            if (!txtTargetDate.YearMonthCheck())
                return false;
            if ((chkRelatedPrinting.Checked == true))
            {
                if (!((rdoITEM.Checked == true) || (rdoProductCD.Checked == true)))
                {
                    zkhbl.ShowMessage("E102");
                    return false;
                }
            }
            msae.StoreAuthorizationsCD = StoreAuthorizationsCD;
            msae.ChangeDate = StoreAuthorizationsChangeDate;
            msae.StoreCD = cboSouko.SelectedValue.ToString();
            DataTable dtAuthorization = new DataTable();
            dtAuthorization = zkhbl.M_StoreAuthorizations_Select(msae);
            if (dtAuthorization.Rows.Count == 0)
            {
                zkhbl.ShowMessage("E139");
                cboSouko.Focus();
                return false;
            }
            return true;
        }
      
        private D_Purchase_Details_Entity PurchaseDetailInfo()
        {
            int year = Convert.ToInt32(txtTargetDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(txtTargetDate.Text.Substring(5, 2));
            string lastday = "/" + DateTime.DaysInMonth(year, month).ToString();
            dpde = new D_Purchase_Details_Entity()
            {
                ChangeDate = txtTargetDate.Text + lastday,
                MakerItemCD= scMakerShohinCD.TxtCode.Text,
                ItemCD = scITEM.TxtCode.Text,
                SKUCD = scSKUCD.TxtCode.Text,
                JanCD = scJANCD.TxtCode.Text,
                //MakerItemCD = scITEM.TxtCode.Text,
                ITemName = txtSKUName.Text
            };
            return dpde;
        }
        private D_MonthlyStock_Entity MonthlyStockInfo()
        {
            int year = Convert.ToInt32(txtTargetDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(txtTargetDate.Text.Substring(5, 2));
            string lastday = "/" + DateTime.DaysInMonth(year, month).ToString();
            dmse = new D_MonthlyStock_Entity()
            {
                YYYYMM = txtTargetDate.Text.Replace("/", ""),
                SoukoCD = cboSouko.SelectedValue.ToString(),
                TargetDateFrom = txtTargetDate.Text + "/01",
                TargetDateTo = txtTargetDate.Text + lastday
            };
            return dmse;
        }
        private M_StoreClose_Entity GetStoreClose_Data()
        {
            msce = new M_StoreClose_Entity()
            {
                StoreCD = cboSouko.SelectedValue.ToString(),
                FiscalYYYYMM = txtTargetDate.Text.Replace("/", ""),
            };
            return msce;
        }
        private L_Log_Entity Get_L_Log_Entity()
        {
            L_Log_Entity lle = new L_Log_Entity();

            if (chkRelatedPrinting.Checked == true)
                chk = 1;
            else chk = 0;

            lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "",
                KeyItem = txtTargetDate.Text + "," + cboSouko.AccessibilityObject.Name + "," + chk.ToString()
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
                dpde = new D_Purchase_Details_Entity();
                dmse = new D_MonthlyStock_Entity();
                dpde = PurchaseDetailInfo();
                dmse = MonthlyStockInfo();
                if (chkRelatedPrinting.Checked == true)
                {
                    if(rdoITEM.Checked==true)
                    {
                        chk = 1;
                    }
                    else if(rdoProductCD.Checked==true)
                    {
                        chk = 2;
                    }
                }
                else
                {
                    chk = 3;
                }
                DataTable dt = zkhbl.RPC_ZaikoKanriHyou(dpde, dmse,chk);
                
                if (dt.Rows.Count > 0)
                {
                     
                    try
                    {
                        ZaikoKanriHyou_Report zkh_Report = new ZaikoKanriHyou_Report();
                        DialogResult DResult;
                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:
                                DResult = bbl.ShowMessage("Q201");
                                if (DResult == DialogResult.No)
                                {
                                    return;
                                }
                                zkh_Report.SetDataSource(dt);
                                zkh_Report.Refresh();
                                zkh_Report.SetParameterValue("lblDate", txtTargetDate.Text);
                                zkh_Report.SetParameterValue("lblSouko", cboSouko.SelectedValue.ToString() + "   " + cboSouko.Text);
                                zkh_Report.SetParameterValue("lblToday", dt.Rows[0]["Today"].ToString() + "  " + DateTime.Now.ToString("HH:mm"));
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
                                    var vr = new Viewer();
                                    vr.CrystalReportViewer1.ShowPrintButton = true;
                                    vr.CrystalReportViewer1.ReportSource = zkh_Report;
                                    vr.ShowDialog();
                                }
                                else
                                {
                                    CrystalDecisions.Shared.PageMargins margin = zkh_Report.PrintOptions.PageMargins;
                                    margin.leftMargin = DefaultMargin.Left;
                                    margin.topMargin = DefaultMargin.Top;
                                    margin.bottomMargin = DefaultMargin.Bottom;
                                    margin.rightMargin = DefaultMargin.Right;
                                    zkh_Report.PrintOptions.ApplyPageMargins(margin); 
                                    System.Drawing.Printing.PageSettings ps;
                                    try
                                    {
                                        System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();

                                        CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();

                                        System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();
                                        zkh_Report.PrintOptions.PrinterName = "\\\\dataserver\\Canon LBP2900";
                                        System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                                        zkh_Report.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;

                                        zkh_Report.PrintOptions.PrinterDuplex = PrinterDuplex.Simplex;

                                        zkh_Report.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
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
                        txtTargetDate.Focus();
                    }
                }
                else
                {
                    zkhbl.ShowMessage("E128");
                    txtTargetDate.Focus();
                }
            }
        }

        private void CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();
            DataTable dt = zkhbl.M_StoreClose_Check(msce, "3");
            if (dt.Rows.Count > 0)
            {
                string ProgramID = "GetsujiZaikoKeisanSyori";
                RunConsole(ProgramID, msce.FiscalYYYYMM);
            }
        }
        private void RunConsole(string programID, string YYYYMM)
        {
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "1";
            string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + StoreCD + " " + " " + Mode + " " + YYYYMM;//parameter
            //System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            Process p = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            p.WaitForExit();
        }

        private void ZaikoKanriHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void scJANCD_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void chkRelatedPrinting_CheckedChanged(object sender, EventArgs e)
        {
            if(chkRelatedPrinting.Checked==true)
            {
                rdoITEM.Checked = true;
            }
            else
            {
                rdoITEM.Checked = false;
                rdoProductCD.Checked = false;
            }
        }
    }
}
