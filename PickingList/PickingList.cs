using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        D_Picking_Entity dpe1,dpe2,dpe3,dpe4,dpe;
        PickingList_BL plbl;
        int result = -1;
        
        public FrmPickingList() 
        {
            InitializeComponent();
        }
        //Barcode font 
        [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
                                         string lpFileName);
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
            //SetRequiredField();

            //検索用のパラメータ設定
            string stores = GetAllAvailableStores();
            ScPickingNo1.Value1 = InOperatorCD;
            ScPickingNo1.Value2 = stores;
            ScPickingNo2.Value1 = InOperatorCD;
            ScPickingNo2.Value2 = stores;

            //BarCode font Install
            var fontDestination = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts), "IDAutomationHC39M Code 39 Barcode.ttf");
            if (!File.Exists(fontDestination))
            {
                string fileName = "IDAutomationHC39M Code 39 Barcode.ttf";
                string path = Path.Combine(Environment.CurrentDirectory, @"Font\", fileName);
                result = AddFontResource(path);
            }
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

        private void SetRequiredField()
        {
            if (chkReissued1.Checked == true)
                ScPickingNo1.TxtCode.Require(true);
            if (chkReissued2.Checked == true)
                ScPickingNo2.TxtCode.Require(true);
            if(chkUnissued2.Checked==true)
                txtDateTo2.Require(true);
        }
        public void BindData()
        {
            string ymd = bbl.GetDate();
            txtDateTo1.Text = ymd;
            cboSouko.Bind(ymd);
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
                        ShippingPlanDateFrom = txtDateFrom1.Text.Replace('/','-'),
                        ShippingPlanDateTo = txtDateTo1.Text.Replace('/', '-'),
                        ShippingDate = txtShipmentDate.Text,
                        InsertOperator = InOperatorCD,
                        ProgramID=this.InProgramID,
                        PC=InPcID,
                    };
                    dtPrintData1 = plbl.PickingList_InsertUpdateSelect_Check1(dpe1);
                }

                if (chkReissued1.Checked == true)
                {
                    dpe2 = new D_Picking_Entity
                    {
                        SoukoCD = cboSouko.SelectedValue.ToString(), 
                        PickingNO = ScPickingNo1.TxtCode.Text,
                        InsertOperator = InOperatorCD,

                    };
                    dtPrintData2 = plbl.PickingList_InsertUpdateSelect_Check2(dpe2);
                }

                if (chkUnissued2.Checked == true)
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

                    if (chkUnissued1.Checked == true)
                    {
                       if(dtPrintData1.Rows.Count > 0)
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

                            dpe = new D_Picking_Entity
                            {
                                InsertOperator = InOperatorCD,
                                ProgramID = this.InProgramID,
                                PC = InPcID,
                            };
                            plbl.D_Picking_Update(dtPrintData1.Rows[0]["PickingNO"].ToString(), dpe);

                        }
                        else
                        {
                            plbl.ShowMessage("E128");
                        }

                    }
                       

                    if (chkReissued1.Checked == true)
                    {
                        if (dtPrintData2.Rows.Count > 0)
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

                            dpe = new D_Picking_Entity
                            {
                                InsertOperator = InOperatorCD,
                                ProgramID = this.InProgramID,
                                PC = InPcID,
                            };

                            plbl.D_Picking_Update(dtPrintData2.Rows[0]["PickingNO"].ToString(), dpe);
                        }
                        else
                        {
                            bbl.ShowMessage("E128");
                        }
                    }
                            

                    if (chkUnissued2.Checked == true)
                    {
                        if (dtPrintData3.Rows.Count > 0)
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

                            dpe = new D_Picking_Entity
                            {
                                InsertOperator = InOperatorCD,
                                ProgramID = this.InProgramID,
                                PC = InPcID,
                            };

                            plbl.D_Picking_Update(dtPrintData3.Rows[0]["PickingNO"].ToString(), dpe);
                        }
                        else
                        {
                            bbl.ShowMessage("E128");
                        }
                    }
                           

                    if (chkReissued2.Checked == true)
                    {
                        if (dtPrintData4.Rows.Count > 0)
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

                            dpe = new D_Picking_Entity
                            {
                                InsertOperator = InOperatorCD,
                                ProgramID = this.InProgramID,
                                PC = InPcID,
                            };

                            plbl.D_Picking_Update(dtPrintData4.Rows[0]["PickingNO"].ToString(), dpe);

                        }
                        else
                        {
                            bbl.ShowMessage("E128");
                        }
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

            cboSouko.SelectedValue = SoukoCD;

            txtDateFrom1.Text = string.Empty;
            txtDateTo1.Text = todayDate;
            txtShipmentDate.Text = string.Empty;
            ScPickingNo1.TxtCode.Text = string.Empty;

            chkUnissued2.Checked = false;
            chkReissued2.Checked = false;

            txtDateFrom2.Text = string.Empty;
            txtDateTo2.Text = string.Empty;
            ScPickingNo2.TxtCode.Text = string.Empty;

            chkUnissued1.Focus();
            DisablePanel(panel1);
            DisablePanel(panel2);
        }

        public bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { cboSouko }))
                return false;

            if (chkUnissued1.Checked == true)
            {
                if (!txtDateFrom1.DateCheck())
                    return false;
                if (!txtDateTo1.DateCheck())
                    return false;
                int result = txtDateFrom1.Text.CompareTo(txtDateTo1.Text);
                if (result > 0 || (!string.IsNullOrWhiteSpace(txtDateFrom1.Text) && string.IsNullOrWhiteSpace(txtDateTo1.Text)))
                {
                    bbl.ShowMessage("E104");
                    txtDateTo1.Focus();
                    return false;
                }

                if (!txtShipmentDate.DateCheck())
                    return false;
                if (string.IsNullOrWhiteSpace(txtDateTo1.Text) && string.IsNullOrWhiteSpace(txtShipmentDate.Text))
                {
                    bbl.ShowMessage("E202", "出荷予定日", "出荷予定日");
                    txtDateTo1.Focus();
                    return false;
                }
                if ( !string.IsNullOrWhiteSpace(txtDateTo1.Text) && !string.IsNullOrWhiteSpace(txtShipmentDate.Text))
                {
                    bbl.ShowMessage("E188", "出荷予定日", "出荷予定日");
                    txtShipmentDate.Focus();
                    return false;
                }
            }


            if (chkReissued1.Checked == true)
            {
                if (!RequireCheck(new Control[] { ScPickingNo1.TxtCode }))
                    return false;

                if (!ScPickingNo1.IsExists(2))
                {
                    bbl.ShowMessage("E128");
                    ScPickingNo1.SetFocus(1);
                    return false;
                }
                DataTable dtPickingKBN = plbl.Pickinglist_Select(ScPickingNo1.TxtCode.Text);
                if (dtPickingKBN.Rows[0]["PickingKBN"].ToString() == "2")
                {
                    bbl.ShowMessage("E279");
                    ScPickingNo1.SetFocus(1);
                    return false;
                }

                //if (!string.IsNullOrWhiteSpace(txtDateTo1.Text))
                //{
                    
                //}
            }

            if (chkUnissued2.Checked == true)
            {
                if (!txtDateFrom2.DateCheck())
                    return false;
                if (!RequireCheck(new Control[] { txtDateTo2 }))
                    return false;

                if (!txtDateTo2.DateCheck())
                    return false;
                int result = txtDateFrom2.Text.CompareTo(txtDateTo2.Text);
                if (result > 0 || (!string.IsNullOrWhiteSpace(txtDateFrom2.Text) && string.IsNullOrWhiteSpace(txtDateTo2.Text)))
                {
                    bbl.ShowMessage("E104");
                    txtDateTo2.Focus();
                    return false;
                }
            }

            if (chkReissued2.Checked==true)
            {
                if (!RequireCheck(new Control[] { ScPickingNo2.TxtCode }))
                    return false;
                if (!ScPickingNo2.IsExists(2))
                {
                    bbl.ShowMessage("E128");
                    ScPickingNo2.SetFocus(1);
                    return false;
                }

                DataTable dtPickingKBN = plbl.Pickinglist_Select(ScPickingNo2.TxtCode.Text);
                if (dtPickingKBN.Rows[0]["PickingKBN"].ToString() == "1")
                {
                    bbl.ShowMessage("E279");
                    ScPickingNo2.SetFocus(1);
                    return false;
                }
            }
           

            return true;
        }

        private void FrmPickingList_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }


        //private void ScPickingNo1_Enter(object sender, EventArgs e)
        //{
        //    ScPickingNo1.Value1 = cboSouko.SelectedValue.ToString();
        //}

        //private void ScPickingNo2_Enter(object sender, EventArgs e)
        //{
        //    ScPickingNo2.Value1 = cboSouko.SelectedValue.ToString();
        //}


        private void txtDateTo1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (chkUnissued1.Checked == true && !string.IsNullOrWhiteSpace(txtDateTo1.Text))
                {
                    int result = txtDateFrom1.Text.CompareTo(txtDateTo1.Text);
                    if (result > 0 || (!string.IsNullOrWhiteSpace(txtDateFrom1.Text) && string.IsNullOrWhiteSpace(txtDateTo1.Text)))
                    {
                        bbl.ShowMessage("E104");
                        txtDateTo1.Focus();
                    }
                }

            }
        }

        private void txtDateTo2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (chkUnissued2.Checked == true)
                {
                    if (string.IsNullOrWhiteSpace(txtDateTo2.Text))
                    {
                        bbl.ShowMessage("E102");
                        txtDateTo2.Focus();
                    }

                    if(!string.IsNullOrWhiteSpace(txtDateTo2.Text))
                    {
                        int result = txtDateFrom2.Text.CompareTo(txtDateTo2.Text);
                        if (result > 0 || (!string.IsNullOrWhiteSpace(txtDateFrom2.Text) && string.IsNullOrWhiteSpace(txtDateTo2.Text)))
                        {
                            bbl.ShowMessage("E104");
                            txtDateTo2.Focus();
                        }
                    }
                }

            }
        }

        private void txtShipmentDate_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtDateTo1.Text) && string.IsNullOrWhiteSpace(txtShipmentDate.Text))
                {
                    bbl.ShowMessage("E202", "出荷予定日", "出荷予定日");
                    txtShipmentDate.Focus();
                }
                if ((!string.IsNullOrWhiteSpace(txtDateFrom1.Text) || !string.IsNullOrWhiteSpace(txtDateTo1.Text)) && !string.IsNullOrWhiteSpace(txtShipmentDate.Text))
                {
                    bbl.ShowMessage("E188", "出荷予定日", "出荷予定日");
                    txtShipmentDate.Focus();
                }
            }

        }

        private void ScPickingNo1_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScPickingNo1.TxtCode.Text))
                {
                    if (!ScPickingNo1.IsExists(2))
                    {
                        bbl.ShowMessage("E128");
                        ScPickingNo1.SetFocus(1);
                    }

                    DataTable dtPickingKBN = plbl.Pickinglist_Select(ScPickingNo1.TxtCode.Text);
                    if (dtPickingKBN.Rows[0]["PickingKBN"].ToString()=="2")
                    {
                        bbl.ShowMessage("E279");
                        ScPickingNo1.SetFocus(1);
                    }
                }
                else
                {
                    bbl.ShowMessage("E102");
                    ScPickingNo1.SetFocus(1);
                }
            }
        }

        private void ScPickingNo2_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScPickingNo2.TxtCode.Text))
                {
                    if (!ScPickingNo2.IsExists(2))
                    {
                        bbl.ShowMessage("E128");
                        ScPickingNo2.SetFocus(1);
                    }
                    DataTable dtPickingKBN = plbl.Pickinglist_Select(ScPickingNo2.TxtCode.Text);
                    if (dtPickingKBN.Rows[0]["PickingKBN"].ToString() == "1")
                    {
                        bbl.ShowMessage("E279");
                        ScPickingNo2.SetFocus(1);
                    }
                }
                else
                {
                    bbl.ShowMessage("E102");
                    ScPickingNo1.SetFocus(1);
                }
            }
        }

        private void chkUnissued1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnissued1.Checked == true)
            {
                chkReissued1.Checked = false;
                chkUnissued2.Checked = false;
                chkReissued2.Checked = false;

                txtDateFrom1.Enabled = true;
                txtDateTo1.Enabled = true;
                txtDateTo1.Text = todayDate;
                txtShipmentDate.Enabled = true;

                DisablePanel(panel1);
                DisablePanel(panel2);

            }
            else
            {
                //chkReissued1.Checked = true;
                //chkUnissued2.Checked = true;
                //chkReissued2.Checked = true;

                txtDateFrom1.Enabled = false;
                txtDateTo1.Enabled = false;
                txtDateFrom1.Text = string.Empty;
                txtDateTo1.Text = string.Empty;
                txtShipmentDate.Text = string.Empty;
            }
        }

        private void chkReissued1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReissued1.Checked == true)
            {
                chkUnissued1.Checked = false;
                chkUnissued2.Checked = false;
                chkReissued2.Checked = false;

                txtDateFrom1.Enabled = false;
                txtDateTo1.Enabled = false;
                txtShipmentDate.Enabled = false;
                txtDateFrom2.Enabled = false;
                txtDateTo2.Enabled = false;

                EnablePanel(panel1);
                DisablePanel(panel2);
            }
            else
            {
                ScPickingNo1.TxtCode.Text = string.Empty;
                DisablePanel(panel1);
            }
        }

        private void chkUnissued2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnissued2.Checked == true)
            {
                chkUnissued1.Checked = false;
                chkReissued1.Checked = false;
                chkReissued2.Checked = false;

                txtDateFrom1.Enabled = false;
                txtDateTo1.Enabled = false;
                txtShipmentDate.Enabled = false;

                txtDateFrom2.Enabled = true;
                txtDateTo2.Enabled = true;
                txtDateTo2.Text = todayDate;

                DisablePanel(panel1);
                DisablePanel(panel2);
            }
            else
            {
                txtDateFrom2.Enabled = false;
                txtDateTo2.Enabled = false;
                txtDateFrom2.Text = string.Empty;
                txtDateTo2.Text = string.Empty;
            }
        }

        private void chkReissued2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReissued2.Checked == true)
            {
                chkUnissued1.Checked = false;
                chkReissued1.Checked = false;
                chkUnissued2.Checked = false;

                txtDateFrom1.Enabled = false;
                txtDateTo1.Enabled = false;
                txtShipmentDate.Enabled = false;
                txtDateFrom2.Enabled = false;
                txtDateTo2.Enabled = false;

                DisablePanel(panel1);
                EnablePanel(panel2);
            }
            else
            {
                ScPickingNo2.TxtCode.Text = string.Empty;
                DisablePanel(panel2);
            }
        }
    }
}
