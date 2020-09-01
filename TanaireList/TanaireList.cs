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
using CrystalDecisions.Shared;

namespace TanaireList
{
    public partial class FrmTanaireList : FrmMainForm
    {
        TanaireList_BL tnlbl;
        D_Stock_Entity dse;
        string todayDate = DateTime.Now.ToString("yyyy/MM/dd");
        public FrmTanaireList()
        {
            InitializeComponent();
            //this.KeyUp += FrmTanaireList_KeyUp ;
            tnlbl = new TanaireList_BL();
        }

        private void FrmTanaireList_Load(object sender, EventArgs e)
        {
            InProgramID = "TanaireList";

            //SetFunctionLabel(EProMode.MENTE);
            this.SetFunctionLabel(EProMode.PRINT);
            StartProgram();
            KeyUp += FrmTanaireList_KeyUp;

            BindData();

            ModeVisible = false;
            Btn_F2.Text = string.Empty;
            Btn_F10.Text = string.Empty;

            SetRequireField();
        }

        private void SetRequireField()
        {
            txtEndDate.Require(true);
            cboSouko.Require(true);
        }

        public void BindData()
        {
            string ymd = bbl.GetDate();
            txtStartDate.Focus();
            txtEndDate.Text = ymd;
            cboSouko.Bind(ymd);

        }


        #region ButtonClick
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

                case 11:
                    break;
            }
        }
        

        //public void F12()
        //{
        //    tnlbl = new TanaireList_BL();
            
        //    if (ErrorCheck(12))
        //    {
        //        if (tnlbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
        //        {
        //            dse = GetDStockEntity();
        //            //if (string.IsNullOrWhiteSpace(ScSKUCD.Code))
        //            //    dtPrint = tnlbl.SelectDataForPrint(dse,"1");
        //            //else
        //            //    dtPrint = tnlbl.SelectDataForPrint(dse,"2");

        //            //PrintSec();
        //        }
        //        else
        //            PreviousCtrl.Focus();

        //    }

        //}


        protected override void PrintSec()
        {
            // レコード定義を行う
            // DataTable table = new DataTable();
            tnlbl = new TanaireList_BL();
            string header = string.Empty;
            DataTable dtPrint=new DataTable();
            if (ErrorCheck())
            {
                    dse = GetDStockEntity();
                    if (!string.IsNullOrWhiteSpace(ScSKUCD.Code))
                    {
                        dtPrint = tnlbl.SelectDataForPrint(dse, "1");
                        header = "棚入れリスト(履歴）";
                    }
                    else
                    {
                        dtPrint = tnlbl.SelectDataForPrint(dse, "2");
                        header = "棚入れリスト";

                    }

                    try
                    {
                        if (dtPrint.Rows.Count <= 0 || dtPrint == null)
                        {
                            bbl.ShowMessage("E128");
                            txtStartDate.Focus();
                        }
                        else
                        {
                            
                        
                            //xsdファイルを保存します。

                            //①保存した.xsdはプロジェクトに追加しておきます。
                            DialogResult ret;
                            TanaireList_Report Report = new TanaireList_Report();

                            switch (PrintMode)
                            {
                                case EPrintMode.DIRECT:

                                    ret = bbl.ShowMessage("Q202");
                                    if (ret == DialogResult.No)
                                    {
                                        return;
                                    }
                                    //}

                                    // 印字データをセット

                                    Report.SetDataSource(dtPrint);
                                    Report.Refresh();
                                    Report.SetParameterValue("txtSouko", cboSouko.SelectedValue.ToString() + "  " + cboSouko.Text);
                                    Report.SetParameterValue("txtHeader", header);

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
                                    Report.SetParameterValue("txtSouko", cboSouko.SelectedValue.ToString() + "  " + cboSouko.Text);
                                    Report.SetParameterValue("txtHeader", header);

                                    bool result = OutputPDF(filePath, Report);

                                    //PDF出力が完了しました。
                                    bbl.ShowMessage("I202");

                                    break;
                            }
                            InsertLog(Get_L_Log_Entity(dtPrint));
                        }
                        //else
                        //{
                        //    bbl.ShowMessage("E128");
                        //    txtStartDate.Focus();
                        //  }

                    }
                    finally
                    {

                    }
            }
        }

        private D_Stock_Entity GetDStockEntity()
        {
            string regFlg = string.Empty;
            string locateFlg = string.Empty;
            if (chkUnregistered.Checked && chkRegistered.Checked)
                regFlg = "両方";
            else if (chkUnregistered.Checked)
                regFlg = "未登録";
            else if (chkRegistered.Checked)
                regFlg = "登録済";

            if (chkLocationAri.Checked  && chkLocationNashi.Checked)
                locateFlg = "両方";
            else if (chkLocationAri.Checked)
                locateFlg = "あり";
            else if (chkLocationNashi.Checked)
                locateFlg = "なし";
            

            dse = new D_Stock_Entity
            {
                SoukoCD = cboSouko.SelectedValue.ToString(),
                SKUCD = ScSKUCD.Code,
                ArrivalStartDate =txtStartDate.Text,
                ArrivalEndDate=txtEndDate.Text,
                RegisterFlg=regFlg,  
                LocationFlg=locateFlg,
            };
            return dse;
         }

        
        private bool ErrorCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtStartDate.Text);
                DateTime dt2 = Convert.ToDateTime(txtEndDate.Text);

                if (dt1 > dt2)
                {
                    tnlbl.ShowMessage("E104");
                    txtEndDate.Focus();
                    return false;
                }

            }
            if (!RequireCheck(new Control[] { txtEndDate,cboSouko}))
                return false;
            if(!cboSouko.IsExists(cboSouko.SelectedValue.ToString(),"Souko"))
            {
                tnlbl.ShowMessage("E128");
                txtStartDate.Focus();
                return false;
            }
            if(string.IsNullOrWhiteSpace(ScSKUCD.Code))
                if(chkRegistered.Checked==false && chkUnregistered.Checked==false)
                {
                    tnlbl.ShowMessage("E111");
                    chkUnregistered.Focus();
                    return false;
                }
            if (!string.IsNullOrWhiteSpace(ScSKUCD.Code) && !ScSKUCD.IsExists(2))
            {
                tnlbl.ShowMessage("E101");
                ScSKUCD.SetFocus(1);
                return false;
            }
            return true;
        }

        private L_Log_Entity Get_L_Log_Entity(DataTable dtPrint)
        {
            L_Log_Entity lle = new L_Log_Entity();
            string item = string.Empty;
            foreach(DataRow row in  dtPrint.Rows)
            {
                item += "," + row["RackNO"].ToString();
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

        protected override void EndSec()
        {
            this.Close();
        }
        #endregion

        public void Clear()
        {

            txtStartDate.Text = string.Empty;
            txtEndDate.Text = todayDate;

            BindData();

            chkUnregistered.Checked = true;
            chkRegistered.Checked = false;
            chkLocationAri.Checked = true;
            chkLocationNashi.Checked = true;
            Clear(panel2);

        }

        private void FrmTanaireList_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void ScSKUCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(ScSKUCD.TxtCode.Text))
                {
                    if (!ScSKUCD.SelectData())
                    {
                        tnlbl.ShowMessage("E101");
                        ScSKUCD.SetFocus(1);
                    }

                }
                else
                    Clear(panel2);

            }
        }

        private void txtEndDate_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
                {
                    DateTime dt1 = Convert.ToDateTime(txtStartDate.Text);
                    DateTime dt2 = Convert.ToDateTime(txtEndDate.Text);

                    if (dt1 > dt2)
                    {
                        tnlbl.ShowMessage("E104");
                        txtEndDate.Focus();
                    }
                    else
                        cboSouko.Focus();
                }
            }
        }
    }
}
