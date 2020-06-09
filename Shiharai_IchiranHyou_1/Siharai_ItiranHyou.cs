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
using Search;
using BL;
using DL;
using Entity;
using System.IO;
using CrystalDecisions.Shared;

namespace Shiharai_IchiranHyou
{
    public partial class SiharaiItiranHyou : FrmMainForm
    {
        Base_BL bbl;
        Shiharai_ItiranHyou_BL Ichiran_BL ;
        D_Pay_Entity dpe;       
        DataTable dt, dtResult;
        Viewer previewForm;
        CrystalDecisions.Windows.Forms.CrystalReportViewer vr;
        public SiharaiItiranHyou()
        {
            InitializeComponent();
            bbl = new Base_BL();
            Ichiran_BL = new Shiharai_ItiranHyou_BL();
            dpe = new D_Pay_Entity();
            dt = new DataTable();
            dtResult = new DataTable();
            previewForm = new Viewer();
        }
        private void SiharaiItiranHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "SiharaiItiranHyou";
            SetFunctionLabel(EProMode.PRINT);
            base.Btn_F9.Text = "";
            base.Btn_F10.Text = "";
            base.Btn_F11.Text = "CSV(F11)";
            StartProgram();
            SC_Payment.TxtCode.Require(true);
            //SetFuncKeyAll(this, "100001000011");
            
        }
        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index + 1)
            {

                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear(PanelHeader);
                        txtPurchaseDateFrom.Focus();
                    }
                    break;
                case 11:
                    F11();                   
                    break;
            }
        }
        protected override void PrintSec()
        {
            if (PrintMode != EPrintMode.DIRECT)
                return;

            //各項目のError Check
            if (ErrorCheck())
            {
                // レコード定義を行う
                // DataTable table = new DataTable();
                // レコード定義を行う
                dt = CheckData();
                if (dt == null) return;
                try
                {     
                    
                    ShiraraiItiranHyou_Report Report = new ShiraraiItiranHyou_Report();
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
                                Report.SetDataSource(dt);
                                Report.Refresh();
                                Report.SetParameterValue("PrintDate", System.DateTime.Now.ToString("yyyy/MM/dd") + " " + System.DateTime.Now.ToString("hh:mm"));
                                vr = previewForm.CrystalReportViewer1;
                                if (DResult == DialogResult.Yes)
                                {
                                    //プレビュー
                                    //var PreView = new Viewer();
                                    previewForm.CrystalReportViewer1.ShowPrintButton = true;
                                    previewForm.CrystalReportViewer1.ReportSource = Report;
                                    previewForm.ShowDialog();
                                }
                                else
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
                    // 画面はそのまま
                    txtPurchaseDateFrom.Focus();
                }
            }
        }
        private D_Pay_Entity GetDataInfo()
        {
            dpe = new D_Pay_Entity()
            {
                PurchaseDateFrom=txtPurchaseDateFrom.Text,
                PurchaseDateTo=txtPurChaseDateTo.Text,
                PayeeCD=SC_Payment.TxtCode.Text,
                StaffCD=SC_Staff.TxtCode.Text
            };
            return dpe;

        }      
        protected override void EndSec()
        {
            this.Close();
        }
        private void F11()
        {
            if(ErrorCheck())
            {                
                dtResult = CheckData();


                if (dtResult == null) return;
                //dpe = GetDataInfo();
                //dtResult= Ichiran_BL.ItiranHyou_SelectForPrint(dpe);
                try
                {
                    DialogResult DResult;
                    DResult = bbl.ShowMessage("Q203");
                    //if(DResult== DialogResult.OK)ses
                    //{
                        //LoacalDirectory
                        string folderPath = "C:\\CSV\\";
                        FileInfo logFileInfo = new FileInfo(folderPath);
                        DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                        if (!logDirInfo.Exists) logDirInfo.Create();

                        //ExportCSVFile
                        ToCSV(dtResult, folderPath);
                    //}             
                }
               finally
                {
                    //画面はそのまま
                    txtPurchaseDateFrom.Focus();
                }

                }
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { SC_Payment.TxtCode }))
                return false;

            if (!DateCheck())
            {
                bbl.ShowMessage("E104");
                txtPurchaseDateFrom.Focus();
                return false;
            }
           
            return true;
        }               
        private DataTable CheckData()
        {
            DataTable dt = null;
            if (ErrorCheck())
            {
                dpe = GetDataInfo();
                dt = Ichiran_BL.ItiranHyou_SelectForPrint(dpe);
                //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
                if (dt.Rows.Count == 0)
                {
                    bbl.ShowMessage("E128");
                    return null;
                }
            }

            return dt;
        }
        private L_Log_Entity Get_L_Log_Entity()
        {

            L_Log_Entity lle = new L_Log_Entity();
            DataTable table = CheckData();
            string item = string.Empty;
            //string item = table.Rows[0]["PayNO"].ToString();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                item +="," +  table.Rows[i]["PayNO"].ToString() ;
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
        private void Siharai_ItiranHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        /// <summary>
        /// <Remark>Enterを押す場合、二つの日付のため、チェックする</Remark>
        /// </summary>
        private void txtPurChaseDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!DateCheck())
                {
                    bbl.ShowMessage("E104");
                    txtPurchaseDateFrom.Focus();
                }
            }
        }
        private void SC_Payment_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SC_Payment.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(SC_Payment.TxtCode.Text))
                {
                    if (SC_Payment.SelectData())
                    {
                        SC_Payment.Value1 = SC_Payment.TxtCode.Text;
                        SC_Payment.Value2 = SC_Payment.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        SC_Payment.SetFocus(1);
                    }
                }
            }
        }        
        private void SC_Staff_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
           if(e.KeyCode==Keys.Enter)
            {
                SC_Staff.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(SC_Staff.TxtCode.Text))
                {
                    if (SC_Staff.SelectData())
                    {
                        SC_Staff.Value1 = SC_Staff.TxtCode.Text;
                        SC_Staff.Value2 = SC_Staff.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        SC_Staff.SetFocus(1);
                    }
                }
            }
        }
        private bool DateCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtPurchaseDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPurChaseDateTo.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtPurchaseDateFrom.Text);
                DateTime dt2 = Convert.ToDateTime(txtPurChaseDateTo.Text);

                if (dt1 >= dt2)
                {                   
                    return false;
                }
            }
            return true;
        }
        private DataTable ColumnAdd()
        {
            DataTable dtcol = new DataTable();
            dtcol.Columns.Add("支払日");
            dtcol.Columns.Add("支払先");
            dtcol.Columns.Add("支払額");
            dtcol.Columns.Add("支払番号");
            dtcol.Columns.Add("支払処理番号");
            dtcol.Columns.Add("振込");
            dtcol.Columns.Add("現金");
            dtcol.Columns.Add("手形");
            dtcol.Columns.Add("電債");
            dtcol.Columns.Add("相殺");
            dtcol.Columns.Add("その他①");
            dtcol.Columns.Add("その他②");
            dtcol.Columns.Add("支払額1");
            dtcol.Columns.Add("支払額2");
            dtcol.Columns.Add("支払額3");
            dtcol.Columns.Add("支払額4");
            dtcol.Columns.Add("支払額5");
            dtcol.Columns.Add("支払額6");
            dtcol.Columns.Add("支払額7");
            dtcol.Columns.Add("銀行");
            dtcol.Columns.Add("支払番号現金");
            dtcol.Columns.Add("支払番号手形日付");
            dtcol.Columns.Add("支払番号電債");
            dtcol.Columns.Add("支払番号相殺");
            dtcol.Columns.Add("支払番号その他①");
            dtcol.Columns.Add("支払番号その他②");
            dtcol.Columns.Add("銀行支店");
            dtcol.Columns.Add("処理番号現金");
            dtcol.Columns.Add("処理番号手形日付");
            dtcol.Columns.Add("処理番号電債");
            dtcol.Columns.Add("処理番号相殺");
            dtcol.Columns.Add("処理番号その他①");
            dtcol.Columns.Add("処理番号その他②");
            return dtcol;
        }

        private void SC_Payment_Enter(object sender, EventArgs e)
        {
            SC_Payment.Value1 = "0";
            SC_Payment.ChangeDate = txtPurChaseDateTo.Text;
        }

        private void ToCSV(DataTable dtDataTable, string folderPath)
        {
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            DataTable dtCol = new DataTable();
            dtCol = ColumnAdd();
            //Column headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                csv += dtCol.Columns[i].ToString() + ',';
            }
            csv += "\r\n";
            //Insert Rows
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            csv += value.ToString().Replace(",", "") + ',';
                            // value = String.Format("\"{0}\"", value);
                        }
                        else
                        {
                            csv += value.ToString() + ',';
                        }
                    }
                    else
                    {
                        csv += "" + ',';
                    }
                }
                csv += "\r\n";
            }

            //Exporting to CSV.            
            File.WriteAllText(folderPath + "支払一覧表-   " + System.DateTime.Now.ToString("yyyy-MM-dd") + "." + "csv", csv, Encoding.GetEncoding(932));
            Ichiran_BL.ShowMessage("I203");
            //MessageBox.Show("完了しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
