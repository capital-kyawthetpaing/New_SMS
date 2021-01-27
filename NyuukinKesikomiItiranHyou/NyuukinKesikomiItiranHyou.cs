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
using CrystalDecisions.Shared;

namespace NyuukinKesikomiItiranHyou
{
    public partial class NyuukinKesikomiItiranHyou : FrmMainForm
    {
        NyuukinKesikomiItiranHyou_BL nkih_bl;
        D_Collect_Entity dce;
        DataTable dtReport;
        Viewer vr;
        public NyuukinKesikomiItiranHyou()
        {
            InitializeComponent();
            nkih_bl = new NyuukinKesikomiItiranHyou_BL();
            vr = new Viewer();
        }

        private void NyuukinKesikomiItiranHyou_Load(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            StartProgram();
            SetFunctionLabel(EProMode.PRINT);
            SetRequireField();
            Btn_F11.Text = string.Empty;
            Btn_F10.Text = string.Empty;

            //Bind ComboBoxes
            cboStoreAuthorizations.Bind(string.Empty, "2");
            cboStoreAuthorizations.SelectedValue = StoreCD;
            cboWebCollectType.Bind(string.Empty);
        }

        private void SetRequireField()
        {
            cboStoreAuthorizations.Require(true);
        }

        private void NyuukinKesikomiItiranHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        /// <summary>
        /// Clear data of Panel Detail 
        /// </summary>
        public void Clear()
        {
            Clear(panelDetail);
            txtCollectDateF.Focus();
        }

        /// <summary>
        /// Functions processes for F1,F6
        /// </summary>
        /// <param name="index"></param>
        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index + 1)
            {
                case 6: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                        {
                            Clear();
                        }
                        break;
                    }
            }
            txtCollectDateF.Focus();
        }


        #region  Check Errors before Export Report
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { cboStoreAuthorizations }))
                return false;
            if (!txtCollectDateF.DateCheck())
                return false;
            if (!txtCollectDateT.DateCheck())
                return false;
            
            //if (Convert.ToInt32((txtCollectDateF.Text.ToString().Replace("/", ""))) > Convert.ToInt32(txtCollectDateT.Text.ToString().Replace("/", ""))) //対象期間(From)の方が大きい場合Error
            //{
            //    nkih_bl.ShowMessage("E103");
            //    txtCollectDateF.Focus();
            //    return false;
            //}

            //if (Convert.ToInt32((txtInputDateF.Text.ToString().Replace("/", ""))) > Convert.ToInt32(txtInputDateT.Text.ToString().Replace("/", ""))) //対象期間(From)の方が大きい場合Error
            //{
            //    nkih_bl.ShowMessage("E103");
            //    txtInputDateF.Focus();
            //    return false;
            //}


            if (!string.IsNullOrWhiteSpace(txtCollectDateF.Text) && !string.IsNullOrWhiteSpace(txtCollectDateT.Text))
            {
                if (Convert.ToInt32((txtCollectDateF.Text.ToString().Replace("/", ""))) > Convert.ToInt32(txtCollectDateT.Text.ToString().Replace("/", ""))) //対象期間(From)の方が大きい場合Error
                {
                    nkih_bl.ShowMessage("E104");
                    txtCollectDateT.Focus();
                }
            }

            if (!txtInputDateF.DateCheck())
                return false;
            if (!txtInputDateT.DateCheck())
                return false;

            if (!string.IsNullOrWhiteSpace(txtInputDateF.Text) && !string.IsNullOrWhiteSpace(txtInputDateT.Text))
            {
                if (Convert.ToInt32((txtInputDateF.Text.ToString().Replace("/", ""))) > Convert.ToInt32(txtInputDateT.Text.ToString().Replace("/", ""))) //対象期間(From)の方が大きい場合Error
                {
                    nkih_bl.ShowMessage("E104");
                    txtInputDateT.Focus();
                }
            }
            if (!string.IsNullOrEmpty(ScCollectCustomerCD.Code))
            {
                if (!ScCollectCustomerCD.IsExists())
                {
                    nkih_bl.ShowMessage("E101");
                    ScCollectCustomerCD.SetFocus(1);
                    return false;
                }
            }
            return true;
        }

        private void txtCollectDateT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtCollectDateF.Text) && !string.IsNullOrWhiteSpace(txtCollectDateT.Text))
                {
                    if (Convert.ToInt32((txtCollectDateF.Text.ToString().Replace("/", ""))) > Convert.ToInt32(txtCollectDateT.Text.ToString().Replace("/", ""))) //対象期間(From)の方が大きい場合Error
                    {
                        nkih_bl.ShowMessage("E104");
                        txtCollectDateT.Focus();
                    }
                }
            }
        }

        private void txtInputDateT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtInputDateF.Text) && !string.IsNullOrWhiteSpace(txtInputDateT.Text))
                {
                    if (Convert.ToInt32((txtInputDateF.Text.ToString().Replace("/", ""))) > Convert.ToInt32(txtInputDateT.Text.ToString().Replace("/", ""))) //対象期間(From)の方が大きい場合Error
                    {
                        nkih_bl.ShowMessage("E104");
                        txtInputDateT.Focus();
                    }
                }
            }
        }

        private void ScCollectCustomerCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScCollectCustomerCD.Code))
                {
                    if (!ScCollectCustomerCD.SelectData())
                    {
                        nkih_bl.ShowMessage("E101");
                        ScCollectCustomerCD.SetFocus(1);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Print Report on F12 Click
        /// </summary>
        protected override void PrintSec()
        {
            if (PrintMode != EPrintMode.DIRECT)
                return;

            if (ErrorCheck())
            {
                dce = D_Collect_data();
                dtReport = new DataTable();
                dtReport = nkih_bl.NyuukinKesikomiItiranHyou_Report(dce);

                if (dtReport.Rows.Count > 0)
                {
                    try
                    {
                        NyuukinKesikomiItiranHyou_Report Nkh_report = new NyuukinKesikomiItiranHyou_Report();
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
                                Nkh_report.SetDataSource(dtReport);
                                Nkh_report.Refresh();
                                Nkh_report.SetParameterValue("lblStore", cboStoreAuthorizations.SelectedValue.ToString() + " " + cboStoreAuthorizations.Text);
                                Nkh_report.SetParameterValue("lblToday", DateTime.Now.ToString("yyyy/MM/dd") + "  " + DateTime.Now.ToString("HH:mm"));
                                Nkh_report.SetParameterValue("lblWebCollectType", cboWebCollectType.Text);
                                vr.CrystalReportViewer1.ReportSource = Nkh_report;
                               
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
                                    vr.CrystalReportViewer1.ReportSource = Nkh_report;

                                    vr.ShowDialog();

                                }
                                else
                                {
                                    //int marginLeft = 360;
                                    CrystalDecisions.Shared.PageMargins margin = Nkh_report.PrintOptions.PageMargins;
                                    margin.leftMargin = DefaultMargin.Left; // mmの指定をtwip単位に変換する
                                    margin.topMargin = DefaultMargin.Top;
                                    margin.bottomMargin = DefaultMargin.Bottom;//mmToTwip(marginLeft);
                                    margin.rightMargin = DefaultMargin.Right;
                                    Nkh_report.PrintOptions.ApplyPageMargins(margin);     /// Error Now
                                    // プリンタに印刷
                                    System.Drawing.Printing.PageSettings ps;
                                    try
                                    {
                                        System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();

                                        CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();

                                        System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();



                                        Nkh_report.PrintOptions.PrinterName = "\\\\dataserver\\Canon LBP2900";
                                        System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                                        Nkh_report.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;

                                        Nkh_report.PrintOptions.PrinterDuplex = PrinterDuplex.Simplex;

                                        Nkh_report.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                break;

                        }
                        //プログラム実行履歴
                        //InsertLog(Get_L_Log_Entity());
                    }
                    finally
                    {
                        //画面はそのまま
                        txtCollectDateF.Focus();
                    }
                }

            }
        }

        private D_Collect_Entity D_Collect_data()
        {
            dce = new D_Collect_Entity
            {
                CollectDateFrom = txtCollectDateF.Text,
                CollectDateTo = txtCollectDateT.Text,
                InputDateFrom = txtInputDateF.Text,
                InputDateTo = txtInputDateT.Text,
                WebCollectType = cboWebCollectType.SelectedValue.ToString() == "-1" ? "": cboWebCollectType.SelectedValue.ToString(),
                CollectCustomerCD = ScCollectCustomerCD.Code
            };
            return dce;
        }

        private void ScCollectCustomerCD_Enter(object sender, EventArgs e)
        {
            ScCollectCustomerCD.Value2 = cboStoreAuthorizations.SelectedValue.Equals("-1") ? "" : cboStoreAuthorizations.SelectedValue.ToString();
            ScCollectCustomerCD.Value1 = "3";
        }

       
    }
}
