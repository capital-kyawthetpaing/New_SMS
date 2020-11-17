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
using CKM_Controls;
using System.IO;
using ClosedXML.Excel;
using System.Diagnostics;
using ElencySolutions.CsvHelper;

namespace TairyuZaikoHyou
{
    public partial class FrmTairyuZaikoHyou : FrmMainForm
    {
        TairyuZaikoHyou_BL tzkbl = new TairyuZaikoHyou_BL();
        M_Vendor_Entity mve = new M_Vendor_Entity();
        M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        D_Stock_Entity dse = new D_Stock_Entity();
        M_StoreClose_Entity msce = new M_StoreClose_Entity();
        M_SKU_Entity mskue = new M_SKU_Entity();
        M_SKUInfo_Entity info = new M_SKUInfo_Entity();
        M_SKUTag_Entity mtage = new M_SKUTag_Entity();

        public FrmTairyuZaikoHyou()
        {
            InitializeComponent();
        }

        private void FrmTairyuZaikoHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "TairyuZaikoHyou";
            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            F7Visible = false;
            F8Visible = false;
            F10Visible = false;
            F11Visible = false;

            StartProgram();
            
            BindCombo();
            SetRequireField();
            Btn_F12.Text = "出力(F12)";
            ModeVisible = false;

            rdoOR.Checked = true;
            rdoAND.Checked = false;
            chkPrint.Checked = false;
            rdoItem.Checked = false;
            rdoProductCD.Checked = false;

        }
        public void BindCombo()
        {
            string ymd = bbl.GetDate();
            cboWarehouse.Bind(string.Empty,"");
            CboYear.Bind(ymd);
            cboSeason.Bind(ymd);
            cboReservation.Bind(ymd);
            cboNotices.Bind(ymd);
            cboOrder.Bind(ymd);
            cboPostage.Bind(ymd);
            cboTag1.Bind(ymd);
            cboTag2.Bind(ymd);
            cboTag3.Bind(ymd);
            cboTag4.Bind(ymd);
            cboTag5.Bind(ymd);

        }

        public void SetRequireField()
        {
            txtTargetDays.Require(true);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        public override void FunctionProcess(int Index)
        {
            switch (Index + 1)
            {
                case 2:
                case 3:
                case 4:
                    break;
                case 5:
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear(panelDetail);
                        txtTargetDays.Focus();
                    }
                    break;
                case 10:                 
                    break;
                case 11:               
                    break;
                case 12:
                    F12();
                    break;
            }
        }


        public bool ErrorCheck()
        {
           //if(String.IsNullOrWhiteSpace(txtTargetDays.Text))
           // {
           //     tzkbl.ShowMessage("E102");
           //     txtTargetDays.Focus();
           //     return false;
           // }

            if (!string.IsNullOrEmpty(Sc_Maker.TxtCode.Text))
            {
                if (!Sc_Maker.IsExists(2))
                {
                    tzkbl.ShowMessage("E101");
                    Sc_Maker.SetFocus(1);
                    return false;
                }
            }

            if(!string.IsNullOrEmpty(Sc_Sports.TxtCode.Text))
            {
                mmpe.Key = Sc_Sports.TxtCode.Text;
                mmpe.ID = "202";
                DataTable dtCompetition = new DataTable();
                dtCompetition = tzkbl.M_Multiporpose_CharSelect(mmpe);
                if (dtCompetition.Rows.Count == 0)
                {
                    tzkbl.ShowMessage("E101");
                    Sc_Sports.SetFocus(1);
                    return false;
                }
                else
                {
                    Sc_Sports.LabelText = dtCompetition.Rows[0]["Char1"].ToString();
                }
            }

            //if(rdoItem.Checked)
            //{
            //    if(string.IsNullOrWhiteSpace(txtItem.Text))
            //    {
            //        tzkbl.ShowMessage("E102");
            //        txtItem.Focus();
            //        return false;
            //    }               
            //}

            //if(rdoProductCD.Checked)
            //{
            //    if(string.IsNullOrWhiteSpace(txtManufactureCD.Text))
            //    {
            //        tzkbl.ShowMessage("E102");
            //        txtManufactureCD.Focus();
            //        return false;
            //    }
            //}

            return true;
        }
        public void F12()
        {
            //if (PrintMode != EPrintMode.DIRECT)
            //    return;

            if (ErrorCheck())
            {
                string[] strlist = txtRemarks.Text.Split(',');
                dse = new D_Stock_Entity
                {
                    AdminNO = txtTargetDays.Text,
                    SoukoCD = cboWarehouse.SelectedValue.ToString(),
                    RackNOFrom = txtStorageFrom.Text,
                    RackNOTo = txtStorageTo.Text,
                    Keyword1 = (strlist.Length > 0) ? strlist[0].ToString() : "",
                    Keyword2 = (strlist.Length > 1) ? strlist[1].ToString() : "",
                    Keyword3 = (strlist.Length > 2) ? strlist[2].ToString() : "",   
                };
                if(dse.SoukoCD == "-1")
                {
                    dse.SoukoCD = null;
                }

                mskue = new M_SKU_Entity
                {
                    MainVendorCD = Sc_Maker.TxtCode.Text,
                    BrandCD = Sc_Brand.TxtCode.Text,
                    SKUName = txtProductName.Text,
                    JanCD = ScJanCD.TxtCode.Text,
                    SKUCD = ScSKUCD.TxtCode.Text,
                    ITemCD = txtItem.Text,
                    MakerItem = txtManufactureCD.Text,
                    SportsCD = Sc_Sports.TxtCode.Text,
                    ReserveCD = cboReservation.SelectedValue.ToString(),
                    NoticesCD = cboNotices.SelectedValue.ToString(),
                    PostageCD = cboPostage.SelectedValue.ToString(),
                    OrderAttentionCD = cboOrder.SelectedValue.ToString()
                };
                if(mskue.ReserveCD == "-1")
                {
                    mskue.ReserveCD = null;
                }
                if(mskue.NoticesCD == "-1")
                {
                    mskue.NoticesCD = null;
                }
                if (mskue.PostageCD == "-1")
                {
                    mskue.PostageCD = null;
                }
                if (mskue.OrderAttentionCD == "-1")
                {
                    mskue.OrderAttentionCD = null;
                }

                info = new M_SKUInfo_Entity
                {
                    YearTerm = CboYear.SelectedValue.ToString(),
                    Season = cboSeason.SelectedValue.ToString(),
                    CatalogNO = txtCatalog.Text,
                    InstructionsNO = txtInstructionNo.Text,
                };

                if(info.YearTerm == "-1")
                {
                    info.YearTerm = null;
                }
                if (info.Season == "-1")
                {
                    info.Season = null;
                }

                mtage = new M_SKUTag_Entity
                {
                    TagName1 = cboTag1.SelectedValue.ToString(),
                    TagName2 = cboTag2.SelectedValue.ToString(),
                    TagName3 = cboTag3.SelectedValue.ToString(),
                    TagName4 = cboTag4.SelectedValue.ToString(),
                    TagName5 = cboTag5.SelectedValue.ToString()
                };
                
                if(mtage.TagName1 == "-1")
                {
                    mtage.TagName1 = null;
                }
                if (mtage.TagName2 == "-1")
                {
                    mtage.TagName2 = null;
                }
                if (mtage.TagName3 == "-1")
                {
                    mtage.TagName3 = null;
                }
                if (mtage.TagName4 == "-1")
                {
                    mtage.TagName4 = null;
                }
                if (mtage.TagName5 == "-1")
                {
                    mtage.TagName5 = null;
                }

                if (rdoOR.Checked == true)
                {
                    dse.type = "1";
                }
                else
                {
                    dse.type = "2";
                }

                DataTable dtSelect = new DataTable();
                dtSelect = tzkbl.D_StockSelectForTairyuzaikohyo(dse, mskue, info, mtage);
                if (dtSelect.Rows.Count > 0)
                {
                    CheckBeforeExport();
                    if (bbl.ShowMessage("Q201") == DialogResult.Yes)
                    {
                        try
                        {
                            ChangeDataColumnName(dtSelect);

                            string Folderpath = "C:\\CSV\\";
                            if (!string.IsNullOrWhiteSpace(Folderpath))
                            {
                                if (!Directory.Exists(Folderpath))
                                {
                                    Directory.CreateDirectory(Folderpath);
                                }
                                #region CSV,Excel create and save00
                                SaveFileDialog savedialog = new SaveFileDialog();
                                savedialog.Filter = "Csv|*.csv|Excel|*.xls";

                                savedialog.Title = "Save";
                                InProgramNM = "滞留在庫表";
                                string cmdLine = InProgramNM + " " + DateTime.Now.ToString(" yyyyMMdd_HHmmss ") + " " + InOperatorCD;
                                savedialog.FileName = cmdLine;
                                savedialog.InitialDirectory = Folderpath;
                                savedialog.RestoreDirectory = true;
                                if (savedialog.ShowDialog() == DialogResult.OK)
                                {
                                    if (Path.GetExtension(savedialog.FileName).Contains("csv"))
                                    {

                                        ////after your loop
                                        //File.WriteAllText(Folderpath, csv.ToString());
                                        var utf8WithoutBom = new System.Text.UTF8Encoding(false);
                                        using (StreamWriter writer = new StreamWriter(Folderpath + cmdLine + ".csv", false, utf8WithoutBom))
                                        {
                                            WriteDataTable(dtSelect, writer, true);
                                        }

                                        //CsvWriter csvwriter = new CsvWriter();
                                        //    csvwriter.WriteCsv(dtSelect, savedialog.FileName, Encoding.GetEncoding(932));                                  
                                    }
                                    else
                                    {
                                        XLWorkbook wb = new XLWorkbook();
                                        wb.Worksheets.Add(dtSelect, "Sheet1");
                                        wb.SaveAs(savedialog.FileName);
                                    }

                                    Process.Start(Path.GetDirectoryName(savedialog.FileName));
                                }
                                #endregion
                            }

                        }
                        finally
                        {
                            txtTargetDays.Focus();
                        }
                    }
                  
                }
                else
                {
                    tzkbl.ShowMessage("E128");
                    txtTargetDays.Focus();
                }
            }
        }

        public void WriteDataTable(DataTable dt, TextWriter writer, bool includeHeaders)
        {

            string[] item1 = new string[1];
            item1[0] = "滞留在庫表：";
            writer.WriteLine(String.Join(",", item1));

            string[] item2 = new string[2];
            item2[0] = "対象日数:";
            item2[1] = txtTargetDays.Text;
            writer.WriteLine(String.Join(",", item2));

            string[] item3 = new string[2];
            item3[0] = "倉庫:";
            item2[1] = cboWarehouse.SelectedValue.ToString();
            writer.WriteLine(String.Join(",", item3));

            if (includeHeaders)
            {
                List<string> headerValues = new List<string>();

                foreach (DataColumn column in dt.Columns)
                {
                    headerValues.Add(column.ColumnName);
                }
                StringBuilder builder = new StringBuilder();
                writer.WriteLine(String.Join(",", headerValues.ToArray()));
            }

            string[] item4 = null;
            foreach (DataRow row in dt.Rows)
            {
                item4 = row.ItemArray.Select(o => o.ToString()).ToArray();
                writer.WriteLine(String.Join(",", item4));
            }

            writer.Flush();
        }

        protected DataTable ChangeDataColumnName(DataTable dtAdd)
        {
            dtAdd.Columns["DaysCalculation"].ColumnName = "滞留日数";
            dtAdd.Columns["SKUCD"].ColumnName = "SKUCD";
            dtAdd.Columns["JanCD"].ColumnName = "JANCD";
            dtAdd.Columns["SKUName"].ColumnName = "商品名";
            dtAdd.Columns["ColorName"].ColumnName = "カラー";
            dtAdd.Columns["SizeName"].ColumnName = "サイズ";
            dtAdd.Columns["BrandName"].ColumnName = "ブランド";
            dtAdd.Columns["Char1"].ColumnName = "競技";
            dtAdd.Columns["ArrivalDate"].ColumnName = "最終入荷日";
            dtAdd.Columns["ShippingDate"].ColumnName = "最終出荷日";
            dtAdd.Columns["StockSu"].ColumnName = "在庫数";
            
            //dtAdd.Columns.RemoveAt(2);
            return dtAdd;
        }

        private void CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();

            if (tzkbl.M_StoreClose_Check(msce, "3").Rows.Count > 0)
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

        private M_StoreClose_Entity GetStoreClose_Data()
        {
            string m = DateTime.Now.Month.ToString();
            if (m.Length == 1)
            {
                m = 0 + DateTime.Now.Month.ToString();
            }
            string y = DateTime.Now.Year.ToString();
            //txtTargetDateFrom.Text = a.ToString().Substring(0, 7);
           string day  = y + "/" + m;
            msce = new M_StoreClose_Entity()
            {
                StoreCD = cboWarehouse.SelectedValue.ToString(),
                FiscalYYYYMM = day.Replace("/",""),
            };
            return msce;
        }

        private void FrmTairyuZaikoHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void Sc_Maker_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            Sc_Maker.ChangeDate = bbl.GetDate();
            if (!string.IsNullOrEmpty(Sc_Maker.TxtCode.Text))
            {
                if (Sc_Maker.SelectData())
                {
                    Sc_Maker.Value1 = Sc_Maker.TxtCode.Text;
                    Sc_Maker.Value2 = Sc_Maker.LabelText;
                }
                else
                {
                    bbl.ShowMessage("E101");
                    Sc_Maker.SetFocus(1);
                }
            }

        }

        private void Sc_Maker_Enter(object sender, EventArgs e)
        {
            Sc_Maker.Value1 = "1";//仕入先区分：1
        }
        private void Sc_Competition_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(Sc_Sports.TxtCode.Text))
                {
                    mmpe.Key = Sc_Sports.TxtCode.Text;
                    mmpe.ID = "202";
                    DataTable dtCompetition = new DataTable();
                    dtCompetition = tzkbl.M_Multiporpose_CharSelect(mmpe);
                    if (dtCompetition.Rows.Count == 0)
                    {
                        tzkbl.ShowMessage("E101");
                        Sc_Sports.SetFocus(1);
                    }
                    else
                    {
                        Sc_Sports.LabelText = dtCompetition.Rows[0]["Char1"].ToString();
                    }
                }
            }
        }

        private void Sc_Sports_Enter(object sender, EventArgs e)
        {
            Sc_Sports.ChangeDate = bbl.GetDate();
            Sc_Sports.Value1 = "202";
        }

        private void chkPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPrint.Checked == true)
            {
                rdoItem.Checked = true;
            }
            else
            {
                rdoItem.Checked = false;
                rdoProductCD.Checked = false;
            }
        }

        private void rdoItem_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoItem.Checked == true)
            {
                if(chkPrint.Checked == false)
                {
                    tzkbl.ShowMessage("E102");
                    chkPrint.Focus();
                }
            }
        }

        private void rdoProductCD_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoProductCD.Checked == true)
            {
                if (chkPrint.Checked == false)
                {
                    tzkbl.ShowMessage("E102");
                    chkPrint.Focus();
                }
            }
        }
    }
}
