﻿using System;
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
        Viewer vr;
        D_Purchase_Details_Entity dpde;
        D_MonthlyStock_Entity dmse;
        M_StoreClose_Entity msce;
        int chk = 0;
        
        public ZaikoKanriHyou()
        {
            InitializeComponent();
            zkhbl = new ZaikoKanriHyou_BL();
            vr = new Viewer();
        }

        private void ZaikoKanriHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "ZaikoKanriHyou";
            StartProgram();
            SetRequiredField();
            SetFunctionLabel(EProMode.PRINT);
            Btn_F10.Text = "";
            cboSouko.Bind(string.Empty);
            cboSouko.SelectedValue = SoukoCD;
            F11Visible = false;
            scITEM.CodeWidth = 600;
            scSKUCD.CodeWidth = 600;
            scMakerShohinCD.CodeWidth = 600;
            txtTargetDate.Text = DateTime.Now.ToString("yyyy/MM");
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
            txtTargetDate.Focus();
            cboSouko.SelectedValue = SoukoCD;
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
            if ((chkRelatedPrinting.Checked == true))
            {
                if (!((rdoITEM.Checked == true) || (rdoProductCD.Checked == true)))
                {
                    zkhbl.ShowMessage("E102");
                    return false;
                }
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
                ItemCD = scMakerShohinCD.Code,
                SKUCD = scSKUCD.Code,
                JanCD = scJANCD.Code,
                MakerItemCD = scITEM.Code,
                ITemName = txtSKUName.Text
            };
            return dpde;
        }
        private D_MonthlyStock_Entity MonthlyStockInfo()
        {
            dmse = new D_MonthlyStock_Entity()
            {
                SoukoCD = cboSouko.SelectedValue.ToString(),
                YYYYMM = txtTargetDate.Text.Replace("/", "")
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
                   // CheckBeforeExport();
                    try
                    {
                        ZaikoKanriHyou_Report zkh_Report = new ZaikoKanriHyou_Report();
                        DialogResult DResult;
                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:
                                DResult = bbl.ShowMessage("Q201");
                                if (DResult == DialogResult.Cancel)
                                {
                                    return;
                                }
                                zkh_Report.SetDataSource(dt);
                                zkh_Report.Refresh();
                                zkh_Report.SetParameterValue("lblDate", txtTargetDate.Text);
                                zkh_Report.SetParameterValue("lblSouko", cboSouko.SelectedValue.ToString() + "   " + cboSouko.AccessibilityObject.Name);
                                zkh_Report.SetParameterValue("lblToday", dt.Rows[0]["Today"].ToString() + "  " + dt.Rows[0]["Now"].ToString());
                                //crv = vr.CrystalReportViewer1;
                                //crv.ReportSource = zkh_Report;
                                //vr.ShowDialog();
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
                                    //印刷処理プレビュー
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
            }
        }

        private void CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();

            if (zkhbl.M_StoreClose_Check(msce, "2").Rows.Count > 0)
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
            string cmdLine = " " + InOperatorCD + " " + Login_BL.GetHostName() + " " + StoreCD + " " + " " + Mode + " " + YYYYMM;//parameter
            System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
        }

        private void ZaikoKanriHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void scJANCD_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
