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

namespace PickingList
{
    public partial class FrmPickingList : FrmMainForm
    {
        string todayDate = DateTime.Now.ToString("yyyy/MM/dd");    
        D_Picking_Entity dpe1,dpe2,dpe3,dpe4;
        PickingList_BL plbl;
        public FrmPickingList() 
        {
            InitializeComponent();
        }   

        private void FrmPickingList_Load(object sender, EventArgs e)
        {
            InProgramID = "PickingList";

            this.SetFunctionLabel(EProMode.SHOW);
            this.SetFunctionLabel(EProMode.PRINT);
            plbl = new PickingList_BL();

            StartProgram();
            PageloadBind();
            ModeVisible = false;
            BindData();

        }

        public void PageloadBind()
        {  
            chkUnissued1.Focus();
            DisablePanel(panel1);
            DisablePanel(panel2);

            txtDateFrom2.Enabled = false;
            txtDateTo2.Enabled = false;

            Btn_F9.Text = string.Empty;  
            Btn_F10.Text = string.Empty;
            Btn_F11.Text = string.Empty;
        }
        public void BindData()
        {
            txtDateTo1.Text = todayDate;
            cboSouko.Bind(todayDate);
            cboSouko.SelectedValue = SoukoCD;

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

                        Scr_Clr(0);
                        break;
                    }

                case 11://F12:最新化
                    //PrintSec();
                   // chkUnissued1.Focus();
                    break;
            }
        }


        protected override void PrintSec()
        {
            DataTable dtPrintData1 = new DataTable();
            DataTable dtPrintData2 = new DataTable();
            DataTable dtPrintData3 = new DataTable();
            DataTable dtPrintData4 = new DataTable();
            if (ErrorCheck())
            {
                if (chkUnissued1.Checked == true)
                {
                    dpe1 = new D_Picking_Entity
                    {
                        SoukoCD = cboSouko.SelectedValue.ToString(),
                        StoreCD = StoreCD,
                        ShippingPlanDateFrom = txtDateFrom1.Text,
                        ShippingPlanDateTo = txtDateTo1.Text,
                        ShippingDate = txtShipmentDate.Text,
                        InsertOperator = InOperatorCD,

                    };
                    dtPrintData1 = plbl.PickingList_InsertUpdateSelect_Check1(dpe1);
                }

                if (chkUnissued2.Checked == true)
                {
                    dpe2 = new D_Picking_Entity
                    {
                        SoukoCD = cboSouko.SelectedValue.ToString(),
                        PickingNO = ScPickingNo1.TxtCode.Text,
                        InsertOperator = InOperatorCD,

                    };
                    dtPrintData2 = plbl.PickingList_InsertUpdateSelect_Check2(dpe2);
                }

                if (chkReissued1.Checked == true)
                {
                    dpe3 = new D_Picking_Entity
                    {
                        SoukoCD = cboSouko.SelectedValue.ToString(),
                        StoreCD=StoreCD,
                        ShippingPlanDateFrom = txtDateFrom2.Text,
                        ShippingPlanDateTo = txtDateTo2.Text,
                        InsertOperator = InOperatorCD,

                    };
                    dtPrintData3 = plbl.PickingList_InsertUpdateSelect_Check3(dpe3);
                }

                if (chkReissued2.Checked == true)
                {
                    dpe4 = new D_Picking_Entity
                    {
                        SoukoCD = cboSouko.SelectedValue.ToString(),
                        PickingNO = ScPickingNo2.TxtCode.Text,
                        InsertOperator = InOperatorCD,

                    };
                    dtPrintData4 = plbl.PickingList_InsertUpdateSelect_Check2(dpe4);
                }


                // レコード定義を行う

                try
                {

                    if (dtPrintData1 == null && dtPrintData2 == null && dtPrintData3 == null && dtPrintData4 == null)
                    {
                        return;
                    }

                    if (chkUnissued1.Checked == true && dtPrintData1.Rows.Count > 0)
                    {

                        DialogResult ret;
                        PickingList_Report Report = new PickingList_Report();

                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:



                                //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                                ret = plbl.ShowMessage("Q208");
                                if (ret == DialogResult.Cancel)
                                {
                                    return;
                                }
                                Report.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                Report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                                // 印字データをセット
                                Report.SetDataSource(dtPrintData1);
                                Report.Refresh();
                                Report.SetParameterValue("txtSouko", cboSouko.SelectedValue.ToString() + "  " + cboSouko.Text);
                                Report.SetParameterValue("pickingNO", dtPrintData1.Rows[0]["PickingNO"].ToString());

                                if (ret == DialogResult.Yes)
                                {
                                    //プレビュー
                                    var previewForm = new Viewer();
                                    previewForm.CrystalReportViewer1.ShowPrintButton = true;
                                    previewForm.CrystalReportViewer1.ReportSource = Report;

                                    previewForm.ShowDialog();
                                }
                                else
                                {
                                    int marginLeft = 360;
                                    CrystalDecisions.Shared.PageMargins margin = Report.PrintOptions.PageMargins;
                                    margin.leftMargin = marginLeft; // mmの指定をtwip単位に変換する
                                    margin.topMargin = marginLeft;
                                    margin.bottomMargin = marginLeft;//mmToTwip(marginLeft);
                                    margin.rightMargin = marginLeft;
                                    Report.PrintOptions.ApplyPageMargins(margin);
                                    // プリンタに印刷
                                    Report.PrintToPrinter(0, false, 0, 0);
                                }
                                break;

                            case EPrintMode.PDF:
                                if (plbl.ShowMessage("Q204") != DialogResult.Yes)
                                {
                                    return;
                                }
                                string filePath = "";
                                if (!ShowSaveFileDialog(InProgramNM, out filePath))
                                {
                                    return;
                                }

                                // 印字データをセット
                                Report.SetDataSource(dtPrintData1);
                                Report.Refresh();

                                bool result = OutputPDF(filePath, Report);

                                //PDF出力が完了しました。
                                plbl.ShowMessage("I202");

                                break;
                        }

                        plbl.D_Picking_Update(dtPrintData1.Rows[0]["PickingNO"].ToString(), InOperatorCD);

                    }

                    if (chkUnissued2.Checked == true && dtPrintData2.Rows.Count > 0)
                    {

                        DialogResult ret;
                        PickingList_Report Report2 = new PickingList_Report();

                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:



                                //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                                ret = plbl.ShowMessage("Q208");
                                if (ret == DialogResult.Cancel)
                                {
                                    return;
                                }
                                Report2.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                Report2.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                                // 印字データをセット
                                Report2.SetDataSource(dtPrintData2);
                                Report2.Refresh();
                                Report2.SetParameterValue("txtSouko", cboSouko.SelectedValue.ToString() + "  " + cboSouko.Text);
                                Report2.SetParameterValue("pickingNO", dtPrintData2.Rows[0]["PickingNO"].ToString());

                                if (ret == DialogResult.Yes)
                                {
                                    //プレビュー
                                    var previewForm = new Viewer();
                                    previewForm.CrystalReportViewer1.ShowPrintButton = true;
                                    previewForm.CrystalReportViewer1.ReportSource = Report2;
                                    //previewForm.CrystalReportViewer1.Zoom(1);

                                    previewForm.ShowDialog();
                                }
                                else
                                {
                                    int marginLeft = 360;
                                    CrystalDecisions.Shared.PageMargins margin = Report2.PrintOptions.PageMargins;
                                    margin.leftMargin = marginLeft; // mmの指定をtwip単位に変換する
                                    margin.topMargin = marginLeft;
                                    margin.bottomMargin = marginLeft;//mmToTwip(marginLeft);
                                    margin.rightMargin = marginLeft;
                                    Report2.PrintOptions.ApplyPageMargins(margin);
                                    // プリンタに印刷
                                    Report2.PrintToPrinter(0, false, 0, 0);
                                }
                                break;

                            case EPrintMode.PDF:
                                if (plbl.ShowMessage("Q204") != DialogResult.Yes)
                                {
                                    return;
                                }
                                string filePath = "";
                                if (!ShowSaveFileDialog(InProgramNM, out filePath))
                                {
                                    return;
                                }

                                // 印字データをセット
                                Report2.SetDataSource(dtPrintData2);
                                Report2.Refresh();

                                bool result = OutputPDF(filePath, Report2);

                                //PDF出力が完了しました。
                                plbl.ShowMessage("I202");

                                break;
                        }

                        plbl.D_Picking_Update(dtPrintData2.Rows[0]["PickingNO"].ToString(), InOperatorCD);
                    }

                    if (chkReissued1.Checked == true && dtPrintData3.Rows.Count > 0)
                    {

                        DialogResult ret;
                        PickingList_Motori_Report Reportm = new PickingList_Motori_Report();

                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:



                                //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                                ret = plbl.ShowMessage("Q208");
                                if (ret == DialogResult.Cancel)
                                {
                                    return;
                                }
                                Reportm.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                Reportm.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                                // 印字データをセット
                                Reportm.SetDataSource(dtPrintData3);
                                Reportm.Refresh();
                                Reportm.SetParameterValue("txtSouko", cboSouko.SelectedValue.ToString() + "  " + cboSouko.Text);
                                Reportm.SetParameterValue("pickingNO", dtPrintData3.Rows[0]["PickingNO"].ToString());

                                if (ret == DialogResult.Yes)
                                {
                                    //プレビュー
                                    var previewForm = new Viewer();
                                    previewForm.CrystalReportViewer1.ShowPrintButton = true;
                                    previewForm.CrystalReportViewer1.ReportSource = Reportm;

                                    previewForm.ShowDialog();
                                }
                                else
                                {
                                    int marginLeft = 360;
                                    CrystalDecisions.Shared.PageMargins margin = Reportm.PrintOptions.PageMargins;
                                    margin.leftMargin = marginLeft; // mmの指定をtwip単位に変換する
                                    margin.topMargin = marginLeft;
                                    margin.bottomMargin = marginLeft;//mmToTwip(marginLeft);
                                    margin.rightMargin = marginLeft;
                                    Reportm.PrintOptions.ApplyPageMargins(margin);
                                    // プリンタに印刷
                                    Reportm.PrintToPrinter(0, false, 0, 0);
                                }
                                break;

                            case EPrintMode.PDF:
                                if (plbl.ShowMessage("Q204") != DialogResult.Yes)
                                {
                                    return;
                                }
                                string filePath = "";
                                if (!ShowSaveFileDialog(InProgramNM, out filePath))
                                {
                                    return;
                                }

                                // 印字データをセット
                                Reportm.SetDataSource(dtPrintData3);
                                Reportm.Refresh();

                                bool result = OutputPDF(filePath, Reportm);

                                //PDF出力が完了しました。
                                plbl.ShowMessage("I202");

                                break;
                        }


                        plbl.D_Picking_Update(dtPrintData3.Rows[0]["PickingNO"].ToString(), InOperatorCD);
                    }

                    if (chkReissued2.Checked == true && dtPrintData4.Rows.Count > 0)
                    {

                        DialogResult ret;
                        PickingList_Motori_Report Reportm2 = new PickingList_Motori_Report();

                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:



                                //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                                ret = plbl.ShowMessage("Q208");
                                if (ret == DialogResult.Cancel)
                                {
                                    return;
                                }
                                Reportm2.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                                Reportm2.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                                // 印字データをセット
                                Reportm2.SetDataSource(dtPrintData4);
                                Reportm2.Refresh();
                                Reportm2.SetParameterValue("txtSouko", cboSouko.SelectedValue.ToString() + "  " + cboSouko.Text);
                                Reportm2.SetParameterValue("pickingNO", dtPrintData4.Rows[0]["PickingNO"].ToString());

                                if (ret == DialogResult.Yes)
                                {
                                    //プレビュー
                                    var previewForm = new Viewer();
                                    previewForm.CrystalReportViewer1.ShowPrintButton = true;
                                    previewForm.CrystalReportViewer1.ReportSource = Reportm2;

                                    previewForm.ShowDialog();
                                }
                                else
                                {
                                    int marginLeft = 360;
                                    CrystalDecisions.Shared.PageMargins margin = Reportm2.PrintOptions.PageMargins;
                                    margin.leftMargin = marginLeft; // mmの指定をtwip単位に変換する
                                    margin.topMargin = marginLeft;
                                    margin.bottomMargin = marginLeft;//mmToTwip(marginLeft);
                                    margin.rightMargin = marginLeft;
                                    Reportm2.PrintOptions.ApplyPageMargins(margin);
                                    // プリンタに印刷
                                    Reportm2.PrintToPrinter(0, false, 0, 0);
                                }
                                break;

                            case EPrintMode.PDF:
                                if (plbl.ShowMessage("Q204") != DialogResult.Yes)
                                {
                                    return;
                                }
                                string filePath = "";
                                if (!ShowSaveFileDialog(InProgramNM, out filePath))
                                {
                                    return;
                                }

                                // 印字データをセット
                                Reportm2.SetDataSource(dtPrintData4);
                                Reportm2.Refresh();

                                bool result = OutputPDF(filePath, Reportm2);

                                //PDF出力が完了しました。
                                plbl.ShowMessage("I202");

                                break;
                        }

                        plbl.D_Picking_Update(dtPrintData4.Rows[0]["PickingNO"].ToString(), InOperatorCD);

                    }
                }
                finally
                {

                }
            }

        }
        protected override void EndSec()
        {
            this.Close();
        }

        private void Scr_Clr(short Kbn)
        {
            chkUnissued1.Checked = true;
            chkReissued1.Checked = false;

            txtDateFrom1.Text = string.Empty;
            txtDateTo1.Text = todayDate;
            txtShipmentDate.Text = string.Empty;
            ScPickingNo1.TxtCode.Text = string.Empty;

            chkUnissued2.Checked = false;
            chkReissued2.Checked = false;

            txtDateFrom2.Text = string.Empty;
            txtDateTo2.Text = string.Empty;
            ScPickingNo2.TxtCode.Text = string.Empty;

            DisablePanel(panel1);
            DisablePanel(panel2);
        }

        public bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { cboSouko }))
                return false;

            if (chkUnissued1.Checked == true)
            {
                int result = txtDateFrom1.Text.CompareTo(txtDateTo1.Text);
                if (result >= 0)
                {
                    bbl.ShowMessage("E104");
                    txtDateFrom1.Focus();
                    return false;
                }
            }
            if (chkUnissued2.Checked==true)
                if (!ScPickingNo1.IsExists(2))
                {
                    bbl.ShowMessage("E128");
                    return false;
                }
            if(string.IsNullOrWhiteSpace(txtDateTo1.Text) && string.IsNullOrWhiteSpace(txtShipmentDate.Text))
            {
                bbl.ShowMessage("E202", "出荷予定日(To)", "出荷予定日");
                return false;
            }
            if ((!string.IsNullOrWhiteSpace(txtDateFrom1.Text) || !string.IsNullOrWhiteSpace(txtDateTo1.Text)) && !string.IsNullOrWhiteSpace(txtShipmentDate.Text))
            {
                bbl.ShowMessage("E188", "出荷予定日(To)", "出荷予定日");
                return false;
            }

            if (chkReissued2.Checked == true)
                if (!ScPickingNo2.IsExists(2))
                {
                    bbl.ShowMessage("E128");
                    return false;
                }
            if (chkReissued1.Checked == true)
            {
                int result = txtDateFrom2.Text.CompareTo(txtDateTo2.Text);
                if (result >= 0)
                {
                    bbl.ShowMessage("E104");
                    txtDateFrom2.Focus();
                    return false;
                }
            }



            return true;
        }

        private void chkUnissued1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnissued1.Checked == true)
            {
                txtDateFrom1.Enabled = true;
                txtDateTo1.Enabled = true;
                txtDateTo1.Text = todayDate;

            }
            else
            {
                txtDateFrom1.Enabled = false;
                txtDateTo1.Enabled = false;
                txtDateTo1.Text = string.Empty;
            }
        }

        private void FrmPickingList_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void chkReissued1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReissued1.Checked == true)
            {
                EnablePanel(panel1);
            }
            else
            {
                DisablePanel(panel1);
            }
        }

        private void chkUnissued2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnissued2.Checked == true)
            {
                txtDateFrom2.Enabled = true;
                txtDateTo2.Enabled = true;
                txtDateTo2.Text = todayDate;

            }
            else
            {
                txtDateFrom2.Enabled = false;
                txtDateTo2.Enabled = false;
                txtDateTo2.Text = string.Empty;
            }
        }

        private void chkReissued2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReissued2.Checked == true)
            {
                EnablePanel(panel2);
            }
            else
            {
                DisablePanel(panel2);
            }
        }
    }
}
