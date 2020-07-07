using System;
using Search;
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
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
using ClosedXML.Excel;

namespace MasterTouroku_ShiireTanka
{
    public partial class FrmMasterTouroku_ShiireTanka : FrmMainForm
    {
        Base_BL bbl = new Base_BL();
        bool cb_focus = false;
        M_ItemOrderPrice_Entity m_IOE;
        M_ITEM_Entity m_IE;
        MasterTouroku_ShiireTanka_BL bl;
        DataView dv;
        DataTable dt;
        DataTable dtsku;
        DataTable dtc;
        string choiceq = "";
        string operatorCd;
        public FrmMasterTouroku_ShiireTanka()
        {
            InitializeComponent();
            bl = new MasterTouroku_ShiireTanka_BL();
            m_IOE=new M_ItemOrderPrice_Entity();
            m_IE=new M_ITEM_Entity();
            dv = new DataView();
        }
        private void FrmMasterTouroku_ShiireTanka_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_ShiireTanka";
            StartProgram();
            ModeText = "ITEM";
            BindCombo();
           
            operatorCd = InOperatorCD;
            TB_headerdate.Text = bbl.GetDate();
        }
        private void BindCombo()
        {
            string ymd = bbl.GetDate();
            CB_store.Bind(ymd);
            if(RB_koten.Checked == true)
            {
                CB_store.SelectedValue = StoreCD;
                CB_store.Enabled = true;
            }
            else
            {
                CB_store.SelectedValue = "0000";
                CB_store.Enabled = false;
            }
            if (RB_sku.Checked == true)
            {
                panel4.Enabled = false;
                panel5.Enabled = false;
                GV_item.Hide();
                this.GV_item.Location = new System.Drawing.Point(89, 346);
            }
            else
            {
                panel4.Enabled = true;
                panel5.Enabled = true;
                GV_item.Show();
            }
            CB_year.Bind(ymd);
            CB_season.Bind(ymd);
            CB_yearC.Bind(ymd);
            cb_seasonC.Bind(ymd);
        }
        public override void FunctionProcess(int Index)
        {
            switch (Index + 1)
            {
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear();
                    }
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    F12();
                    break;
            }
        }
       
        //private void EnabledPanelContents(Panel panel, bool enabled)
        //{
        //    foreach (Control item in panel.Controls)
        //    {
        //        item.Enabled = enabled;
        //    }
        //}
        protected override void EndSec()
        {
            this.Close();
        }
        private void FrmMasterTouroku_ShiireTanka_KeyUp(object sender, KeyEventArgs e)
        {
            if (cb_focus == false)
            { MoveNextControl(e); }
            else
            {
                CB_store.Focus();
                cb_focus = false;
            }
        } 
        #region Search_Control
        private void shiiresaki_Enter(object sender, EventArgs e)
        {
            shiiresaki.Value1 = "1";
            shiiresaki.ChangeDate = TB_headerdate.Text;
        }
        private void shiiresaki_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
                {
                    if (shiiresaki.SelectData())
                    {
                        shiiresaki.Value1 = shiiresaki.TxtCode.Text;
                        shiiresaki.Value2 = shiiresaki.LabelText;
                        DataTable dtdeflg = bbl.Select_SearchName(TB_headerdate.Text, 4, shiiresaki.TxtCode.Text);
                        string deflg = "";
                        if (dtdeflg.Rows.Count > 0)
                        {
                            deflg = dtdeflg.Rows[0]["DeleteFlg"].ToString();
                        }
                        if (deflg == "1")
                        {
                            bbl.ShowMessage("E119");
                            shiiresaki.Focus();
                        }
                        else
                        {
                            F11();
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        shiiresaki.SetFocus(1);
                    }
                }
            }
        }
        private void sport_Enter(object sender, EventArgs e)
        {
            sport.Value1 = "202";
        }
        private void sport_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(sport.TxtCode.Text))
                {
                    if (sport.SelectData())
                    {
                        sport.Value1 = sport.TxtCode.Text;
                        sport.Value2 = sport.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        sport.SetFocus(1);
                    }
                }
            }
        }
        private void segment_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(!string.IsNullOrEmpty(segment.TxtCode.Text))
                 {
                    if (segment.SelectData())
                    {
                        segment.Value1 = segment.TxtCode.Text;
                        segment.Value2 = segment.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        segment.SetFocus(1);
                    }
                }
            }
        }
        private void segment_Enter(object sender, EventArgs e)
        {
            segment.Value1 = "203";
        }
        private void brand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(brand.TxtCode.Text))
                {
                    if (brand.SelectData())
                    {
                        brand.Value1 = brand.TxtCode.Text;
                        brand.Value2 = brand.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        brand.SetFocus(1);
                    }
                }
            }
        }
        private void makershohin_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode==Keys.Enter)
            //{
            //    if(makershohin.SelectData())
            //    {
            //        makershohin.Value1 = makershohin.TxtCode.Text;
            //        makershohin.Value2 = makershohin.LabelText;
            //    }
            //    else
            //    {
            //        bbl.ShowMessage("E101");
            //        makershohin.SetFocus(1);
            //    }
            //}
        }
        private void brandC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(brandC.TxtCode.Text))
                {
                    if (brandC.SelectData())
                    {
                        brandC.Value1 = brandC.TxtCode.Text;
                        brandC.Value2 = brandC.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        brandC.SetFocus(1);
                    }
                }
            }
        }
        private void sportC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(sportC.TxtCode.Text))
                {
                    if (sportC.SelectData())
                    {
                        sportC.Value1 = sportC.TxtCode.Text;
                        sportC.Value2 = sportC.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        sportC.SetFocus(1);
                    }
                }
            }
        }
        private void sportC_Enter(object sender, EventArgs e)
        {
            sportC.Value1 = "202";
        }
        private void segmentC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(segmentC.TxtCode.Text))
                {
                    if (segmentC.SelectData())
                    {
                        segmentC.Value1 = segmentC.TxtCode.Text;
                        segmentC.Value2 = segmentC.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        segmentC.SetFocus(1);
                    }
                }
            }
            
        }
        private void segmentC_Enter(object sender, EventArgs e)
        {
            segmentC.Value1 = "203";
        }
        private void makershohinC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode== Keys.Enter)
            //{
            //    if(makershohinC.SelectData())
            //    {
            //        makershohinC.Value1 = makershohinC.TxtCode.Text;
            //        makershohinC.Value2 = makershohinC.LabelText;
            //    }
            //    else
            //    {
            //        bbl.ShowMessage("E102");
            //        makershohinC.SetFocus(1);
            //    }
            //}
        }
        private void itemcd_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(itemcd.TxtCode.Text))
                {
                    if (itemcd.SelectData())
                    {
                        itemcd.Value1 = itemcd.TxtCode.Text;
                        itemcd.Value2 = itemcd.LabelText;
                        DataTable dtdeflg = bbl.Select_SearchName(TB_headerdate.Text, 15, itemcd.TxtCode.Text);
                        string deflg = "";
                        if (dtdeflg.Rows.Count > 0)
                        {
                            deflg = dtdeflg.Rows[0]["DeleteFlg"].ToString();
                        }
                        if (deflg == "1")
                        {
                            bbl.ShowMessage("E119");
                            itemcd.Focus();
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        itemcd.SetFocus(1);
                    }
                }
            }
        }
        private void itemcd_Enter(object sender, EventArgs e)
        {
            itemcd.Value1 = "1";
            itemcd.ChangeDate = bbl.GetDate();
        }
        #endregion
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { shiiresaki })) //Step1
                return false;

            if (String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
            {
                bbl.ShowMessage("E102");
                shiiresaki.Focus();
                return false;
            }
            if (!shiiresaki.IsExists(2))
            {
                {
                    bbl.ShowMessage("E101");
                    shiiresaki.Focus();
                    return false;
                }
            }
            if (string.IsNullOrEmpty(TB_headerdate.Text))
            {
                bbl.ShowMessage("E102");
                TB_headerdate.Focus();
                return false;
            }
            if (RB_koten.Checked == true)
            {
                if (String.IsNullOrEmpty(CB_store.Text))
                {
                    bbl.ShowMessage("E102");
                    CB_store.Focus();
                    cb_focus = true;
                    return false;

                }
                if (!base.CheckAvailableStores(CB_store.SelectedValue.ToString()))
                {
                    bbl.ShowMessage("E141");
                    CB_store.Focus();
                    cb_focus = true;
                    return false;
                }
            }

            if(!String.IsNullOrEmpty(brand.TxtCode.Text))
            {
                if(!brand.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    brand.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(sport.TxtCode.Text))
            {
                if (!sport.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    sport.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(segment.TxtCode.Text))
            {
                if (!segment.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    segment.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(makershohin.TxtCode.Text))
            {
                if (!makershohin.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    makershohin.Focus();
                    return false;
                }
            }
            if(RB_item.Checked== true)
            {
                if (string.IsNullOrEmpty(itemcd.TxtCode.Text))
                {
                    bbl.ShowMessage("E102");
                    itemcd.SetFocus(1);
                    return false;
                }
                if(!itemcd.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    itemcd.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(TB_date_add.Text))
                {
                    bbl.ShowMessage("E102");
                    TB_date_add.Focus();
                    return false;
                }
                if(String.IsNullOrEmpty(LB_priceouttax.Text))
                {
                    bbl.ShowMessage("E102");
                    LB_priceouttax.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(TB_rate.Text))
                {
                    bbl.ShowMessage("E102");
                    TB_rate.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(TB_pricewithouttax.Text))
                {
                    bbl.ShowMessage("E102");
                    TB_pricewithouttax.Focus();
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(makershohinC.TxtCode.Text))
            {
                if (!makershohinC.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    makershohinC.SetFocus(1);
                    return false;
                }
            }
            return true;
        }

        private void Excel()
        {

            //if (!ErrorCheck())
            //{
            //    return;
            //}

            
            string folderPath = "C:\\Excel\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Filter = "Excel Files|*.xlsx;";
            savedialog.Title = "Save";
            
            savedialog.FileName = "発注単価マスタ" + System.DateTime.Today.ToString("yyyyMMdd");
            savedialog.InitialDirectory = folderPath;

            savedialog.RestoreDirectory = true;

            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(savedialog.FileName).Contains(".xlsx"))
                {
                    Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                    worksheet = workbook.ActiveSheet;
                    worksheet.Name = "worksheet";

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "worksheet");
                        wb.SaveAs(savedialog.FileName);
                        bbl.ShowMessage("I203", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }

                    Process.Start(Path.GetDirectoryName(savedialog.FileName));
                }
            }
        }

        private void ExportCSV()
        {
            //if (!ErrorCheck())
            //{
            //    return;
            //}
            if (bbl.ShowMessage("Q203") == DialogResult.No)
            {
                return;
            }

            //if (!ShowSaveFileDialog("発注単価マスタ", out filePath,1))
            //{
            //    return;
            //}
            if (GV_item.DataSource != null)

            {
                string csv = string.Empty;
                //string filePath = "";
                string folderPath = "C:\\Excel\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Filter = "Excel Files|*.xlsx;";
                savedialog.Title = "Save";

                savedialog.FileName = "発注単価マスタ" + System.DateTime.Today.ToString("yyyyMMdd");
                savedialog.InitialDirectory = folderPath;

                savedialog.RestoreDirectory = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    //Build the CSV file data as a Comma separated string.
                    
                    //LoacalDirectory
                    //string folderPath = "C:\\Excel\\";
                    FileInfo logFileInfo = new FileInfo(folderPath);
                    DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);

                    if (!logDirInfo.Exists) logDirInfo.Create();

                    //Add the Header row for CSV file.
                    foreach (DataGridViewColumn column in GV_item.Columns)
                    {
                        //if(column.HeaderText!="")
                        csv += column.HeaderText + ',';
                    }
                    //Add new line.
                    csv += "\r\n";

                    //Adding the Rows
                    foreach (DataGridViewRow row in GV_item.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            //Add the Data rows.
                            if (cell.Value == null)
                                cell.Value = "";
                            //csv += cell.Value.ToString().Replace(",", ";")+ ',';
                            csv += cell.Value.ToString().Replace(",", "") + ',';
                        }

                        //Add new line.
                        csv += "\r\n";
                    }

                    //Exporting to CSV.            
                    File.WriteAllText(savedialog.FileName, csv, Encoding.GetEncoding(932));
                    bl.ShowMessage("I203");
                    //MessageBox.Show("CSV 出力が完了します。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    bl.ShowMessage("E138");
                    // MessageBox.Show("There is no data for CSV Export", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void F11()
        {

            //m_IOE = GetItemorder();
                 m_IOE = Getdata();
                m_IE = GetItem();
                brand.Clear();
                sport.Clear();
                segment.Clear();
                CB_year.Text = string.Empty;
                CB_season.Text = string.Empty;
                TB_date_condition.Text = string.Empty;
                makershohin.Clear();
                dt = bl.MastertorokuShiiretanka_Select(m_IOE);
                m_IOE.Display = "1";
                dtsku = bl.MastertorokuShiiretanka_Select(m_IOE);
                dv = new DataView(dt);
                GV_item.DataSource = dv;
                dtc = dt;
        }
        private M_ItemOrderPrice_Entity GetItemorder()
        {
            m_IOE = new M_ItemOrderPrice_Entity
            {
                VendorCD = shiiresaki.TxtCode.Text,
                StoreCD = CB_store.SelectedValue.ToString(),
                MakerItem = makershohin.TxtCode.Text,
                Rate = TB_rate.Text,
                ChangeDate = TB_date_condition.Text,
                Headerdate=TB_headerdate.Text,
                PriceWithoutTax=TB_pricewithouttax.Text,
                Display = RB_item.Checked ? "0" : "1",
                InsertOperator =  InOperatorCD
            };
            return m_IOE;
        }
        private M_ItemOrderPrice_Entity Getdata()
        {
            m_IOE = new M_ItemOrderPrice_Entity
            {
                VendorCD = shiiresaki.TxtCode.Text,
                StoreCD = CB_store.SelectedValue.ToString(),
                Display = RB_item.Checked ? "0" : "1",
            };
            return m_IOE;
        }
        private M_ITEM_Entity GetItem()
        {
            m_IE = new M_ITEM_Entity
            {
                SportsCD=sport.TxtCode.Text,
                SegmentCD=segment.TxtCode.Text,
                BrandCD=brand.TxtCode.Text,
                LastYearTerm=CB_year.Text,
                LastSeason=CB_season.Text,
                AddDate=TB_date_add.Text,
                ITemCD= itemcd.TxtCode.Text,
                PriceOutTax=LB_priceouttax1.Text
                
            };
            return m_IE;
        }
        private void TB_date_add_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (String.IsNullOrEmpty(TB_date_add.Text))
                {
                    TB_date_add.Text = TB_headerdate.Text;
                }
                if (string.IsNullOrEmpty(itemcd.TxtCode.Text))
                {
                    bbl.ShowMessage("E102");
                    itemcd.SetFocus(1);
                }
                else
                {
                    if (!itemcd.IsExists(2))
                    {
                        bbl.ShowMessage("E101");
                        itemcd.Focus();
                    }
                    else
                    {
                        m_IE = GetItem();
                        DataTable dt = bl.M_ITem_ItemNandPriceoutTax_Select(m_IE);
                        if (dt.Rows.Count > 0)
                        {
                            itemcd.LabelText = dt.Rows[0]["ItemName"].ToString();
                            //LB_priceouttax.Text = dt.Rows[0]["PriceOutTax"].ToString();
                            Decimal dd = Convert.ToDecimal(dt.Rows[0]["PriceOutTax"]);
                            if (dd != 0)
                            {
                                LB_priceouttax.Text = string.Format("{0:0,0}", dd);
                            }
                            else
                            {
                                LB_priceouttax.Text = Convert.ToInt32(dt.Rows[0]["PriceOutTax"]).ToString();
                            }
                        }
                    }
                }
            }
        }
        private void Clear()
        {
            Clear(panel1);
            Clear(panel2);
            Clear(panel5);
            Clear(panel3);
            RB_zenten.Checked = true;
            RB_item.Checked = true;
            RB_current.Checked = true;
            TB_headerdate.Text = bbl.GetDate();
            CB_store.SelectedValue = "0000";
            GV_item.Refresh();
            GV_item.DataSource = null;
            dt.Rows.Clear();
            dtsku.Rows.Clear();

            //string aa;
        }
        private void RB_koten_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_koten.Checked == true)
            {
                CB_store.SelectedValue = StoreCD;
                CB_store.Enabled = true;
            }
            else
            {
                
                CB_store.SelectedValue = "0000";
                CB_store.Enabled = false;
            }

        }
        private void CB_store_KeyDown(object sender, KeyEventArgs e)
        {
            if (RB_koten.Checked == true)
            {
                if (!base.CheckAvailableStores(CB_store.SelectedValue.ToString()))
                {
                    bbl.ShowMessage("E141");
                    CB_store.Focus();
                    cb_focus = true;
                }
            }
        }
        private void Btn_F11_Click(object sender, EventArgs e)
        {
            F11();
        }
        private void RB_item_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_sku.Checked == true)
            {
                panel4.Enabled = false;
                panel5.Enabled = false;
                this.ブランド.Visible = false;
                this.シーズン.Visible = false;
                this.年度.Visible = false;
                this.商品分類.Visible =false;
                this.競技.Visible =false;

                //this.GV_item.Location = new System.Drawing.Point(89, 346);かーら

                //this.GV_sku.Size = new System.Drawing.Size(1560, 280);
                this.サイズ.Visible = true;
                this.カラー.Visible = true;
                this.SKUCD.Visible = true;
                GV_item.Refresh();
                if(!String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
                {
                    GV_item.DataSource = dtsku;
                }
            }
            else
            {
                panel4.Enabled = true;
                panel5.Enabled = true;
                this.SKUCD.Width = 150;
                this.ブランド.Visible = true;
                this.シーズン.Visible = true;
                this.年度.Visible = true;
                this.商品分類.Visible = true;
                this.競技.Visible = true;
                this.サイズ.Visible = false;
                this.カラー.Visible = false;
                this.SKUCD.Visible = false;
                GV_item.Refresh();
                
                if (!String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
                {
                    GV_item.DataSource = dt;
                }
            }
        }
        private void TB_rate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(LB_priceouttax.Text))
                {
                   
                    //TB_orderprice.Text = Convert.ToString(listprice * (rate * con));
                    //TB_orderprice.Text= string.Format("{0:#,##0}", Convert.ToInt64((listprice * (rate * con))));
                   if(!String.IsNullOrEmpty(TB_rate.Text))
                   {
                        decimal rate = Convert.ToDecimal(TB_rate.Text);
                        decimal con = (decimal)0.01;
                        decimal listprice = Convert.ToDecimal(LB_priceouttax.Text);
                        TB_pricewithouttax.Text = Math.Round(listprice * (rate * con)).ToString();
                   }
                }
            }
        }
        private void btn_add_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            string selectq = "";
            string dateq = "";
            //selectq = " VendorCD ='" + shiiresaki.TxtCode.Text + "'";
            //selectq += " and StoreCD ='" + CB_store.SelectedValue.ToString() + "'";
            selectq += "  ItemCD = '" + itemcd.TxtCode.Text + "'";
            dateq += "  ChangeDate = '" + TB_date_add.Text + "'";

            //selectq += " and Rate = '" + TB_rate.Text + "'";
            //selectq += " and PriceOutTax = '" + LB_priceouttax.Text + "'";
            //selectq += " and PriceWithoutTax = '" + TB_pricewithouttax.Text + "'";
            if (GV_item.DataSource != null)
            {
                DataRow[] dradd;
                dradd = dt.Select(selectq + " and " + dateq);
                if (dradd.Length > 0)//if (dv.Count >0)
                {
                    bbl.ShowMessage("E224");
                }
                else
                {
                    DataTable dtadd = dt.Select(selectq).CopyToDataTable();
                    //dradd = dt.Select(selectq);
                    DataRow row1 = null;
                    for (int i = 0; i < dtadd.Rows.Count; i++)
                    {
                        row1 = dt.NewRow();
                        row1["Tempkey"] = "1";
                        row1["CheckBox"] = "0";
                        row1["BrandName"] = dtadd.Rows[i]["BrandName"];
                        row1["Char1"] = dtadd.Rows[i]["Char1"];
                        row1["SegmentCDName"] = dtadd.Rows[i]["SegmentCDName"];
                        row1["LastYearTerm"] = dtadd.Rows[i]["LastYearTerm"];
                        row1["LastSeason"] = dtadd.Rows[i]["LastSeason"];
                        row1["MakerItem"] = dtadd.Rows[i]["MakerItem"];
                        row1["ItemCD"] = itemcd.TxtCode.Text;
                        row1["ItemName"] = dtadd.Rows[i]["ItemName"];
                        row1["ChangeDate"] = TB_date_add.Text;
                        row1["Rate"] = TB_rate.Text;
                        row1["PriceOutTax"] = LB_priceouttax.Text;
                        row1["PriceWithoutTax"] = TB_pricewithouttax.Text;
                        row1["Tempkey"] = "1";
                        row1["InsertOperator"] = operatorCd;
                        row1["InsertDateTime"] = bbl.GetDate();
                        row1["UpdateOperator"] = operatorCd;
                        row1["UpdateDateTime"] = bbl.GetDate();
                        dt.Rows.Add(row1);
                        dt.AcceptChanges();
                        GV_item.Refresh();
                        GV_item.DataSource = dt;
                        dv.RowStateFilter = DataViewRowState.CurrentRows;
                    }
                    m_IOE = Getdata();
                    m_IOE.Display = "1";
                    DataRow[] drskuadd;
                    drskuadd = dtsku.Select(selectq + " and  ChangeDate = '" + TB_date_add.Text + "'");
                    DataTable dtskuup = bl.M_SKU_SelectFor_SKU_Update("", "", itemcd.TxtCode.Text, TB_date_add.Text, "3");

                    if (drskuadd.Length > 0)
                    {
                        if (dtskuup.Rows.Count > 0)
                        {
                            for (int i = 0; i < drskuadd.Length; i++)
                            {
                                dtskuup.Rows[i]["ItemCD"] = itemcd.TxtCode.Text;
                                dtskuup.Rows[i]["ChangeDate"] = TB_date_add.Text;
                                dtskuup.Rows[i]["Rate"] = TB_rate.Text;
                                dtskuup.Rows[i]["PriceOutTax"] = LB_priceouttax.Text;
                                dtskuup.Rows[i]["PriceWithoutTax"] = TB_pricewithouttax.Text;
                                dtskuup.Rows[i]["InsertOperator"] = operatorCd;
                                dtskuup.Rows[i]["InsertDateTime"] = bbl.GetDate();
                                dtskuup.Rows[i]["UpdateOperator"] = operatorCd;
                                dtskuup.Rows[i]["UpdateDateTime"] = bbl.GetDate();
                            }
                        }
                    }
                    else
                    {
                        if (dtskuup.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtskuup.Rows.Count; i++)
                            {
                                DataRow rowsku;
                                rowsku = dtsku.NewRow();
                                rowsku["Tempkey"] = "1";
                                rowsku["CheckBox"] = "0";
                                rowsku["AdminNO"] = dtskuup.Rows[i]["AdminNO"];
                                rowsku["SKUCD"] = dtskuup.Rows[i]["SKUCD"];
                                rowsku["SizeName"] = dtskuup.Rows[i]["SizeName"];
                                rowsku["ColorName"] = dtskuup.Rows[i]["ColorName"];
                                rowsku["LastYearTerm"] = dtskuup.Rows[i]["LastYearTerm"];
                                rowsku["LastSeason"] = dtskuup.Rows[i]["LastSeason"];
                                rowsku["MakerItem"] = dtskuup.Rows[i]["MakerItem"];
                                rowsku["ItemCD"] = itemcd.TxtCode.Text;
                                rowsku["ChangeDate"] = TB_date_add.Text;
                                rowsku["Rate"] = TB_rate.Text;
                                rowsku["PriceOutTax"] = LB_priceouttax.Text;
                                rowsku["PriceWithoutTax"] = TB_pricewithouttax.Text;
                                rowsku["InsertOperator"] = operatorCd;
                                rowsku["InsertDateTime"] = bbl.GetDate();
                                rowsku["UpdateOperator"] = operatorCd;
                                rowsku["UpdateDateTime"] = bbl.GetDate();
                                dtsku.Rows.Add(rowsku);
                                dtsku.AcceptChanges();
                            }
                        }
                    }
                    //else
                    //{
                    //    GV_item.Rows.Add(false, dtadd.Rows[0]["BrandName"].ToString(),
                    //         dtadd.Rows[0]["Char1"].ToString(), dtadd.Rows[0]["SegmentCDName"].ToString(), dtadd.Rows[0]["LastYearTerm"].ToString()
                    //         , dtadd.Rows[0]["LastSeason"].ToString(), dtadd.Rows[0]["MakerItem"].ToString(), dtadd.Rows[0]["ItemName"].ToString()
                    //          , TB_date_add.Text, LB_priceouttax.Text, TB_rate.Text, TB_pricewithouttax.Text
                    //          );
                    //}
                }
            }
        }

        private void btn_displaymain_Click(object sender, EventArgs e)
        {
            string query;
            if (String.IsNullOrEmpty(brand.TxtCode.Text))
            {
                query = "BrandCD is Null";
            }
            else
            {
                query = "BrandCD = '" + brand.TxtCode.Text + "'";
            }
            if (String.IsNullOrEmpty(segment.TxtCode.Text))
            {
                query += " and SegmentCD is Null";
            }
            else
            {
                query += " and SegmentCD = '" + segment.TxtCode.Text + "'";
            }
            if (String.IsNullOrEmpty(CB_year.Text))
            {
                query += " and LastYearTerm is Null";
            }
            else
            {
                query += " and LastYearTerm = '" + CB_year.Text + "'";
            }
            if (String.IsNullOrEmpty(CB_season.Text))
            {
                query += " and LastSeason is Null";
            }
            else
            {
                query += " and LastSeason = '" + CB_season.Text + "'";
            }
            if (String.IsNullOrEmpty(sport.TxtCode.Text))
            {
                query += " and SportsCD is Null";
            }
            else
            {
                query += " and SportsCD = '" + sport.TxtCode.Text + "'";
            }
            if (String.IsNullOrEmpty(makershohin.TxtCode.Text))
            {
                query += " and MakerItem is Null";
            }
            else
            {
                query += " and MakerItem = '" + makershohin.TxtCode.Text + "'";
            }
            if ((RB_current.Checked == true) && String.IsNullOrEmpty(TB_date_condition.Text))
            {
                query += " and ChangeDate <= '" + TB_headerdate.Text + "'";
            }
            else
            {
                query += " and ChangeDate = '" + TB_date_condition.Text + "'";
            }
            //query += " and SegmentCD = '" + segment.TxtCode.Text + "'";
            //query += " and LastYearTerm = '" + CB_year.Text + "'";
            //query += " and LastSeason = '" + CB_season.Text + "'";
            //query += " and MakerItem = '" + makershohin.TxtCode.Text + "'";
            //query += " and ChangeDate = '" + TB_date_condition.Text + "'";
           
            
            if (GV_item.DataSource != null)
            {
                dv.RowFilter = query;
                dtc = dv.ToTable();
                GV_item.DataSource = dv;
                //dv.RowStateFilter = DataViewRowState.CurrentRows;
            }
        }
        private void btn_choice_Click(object sender, EventArgs e)
        {
            string dateq = "";
            if (String.IsNullOrEmpty(brandC.TxtCode.Text))
            {
                choiceq = "BrandCD is Null";
            }
            else
            {
                choiceq = "BrandCD = '" + brandC.TxtCode.Text + "'";
            }
            if (String.IsNullOrEmpty(segmentC.TxtCode.Text))
            {
                choiceq += " and SegmentCD is Null";
            }
            else
            {
                choiceq += " and SegmentCD = '" + segmentC.TxtCode.Text + "'";
            }
            if (String.IsNullOrEmpty(CB_yearC.Text))
            {
                choiceq += " and LastYearTerm is Null";
            }
            else
            {
                choiceq += " and LastYearTerm = '" + CB_yearC.Text + "'";
            }
            if (String.IsNullOrEmpty(cb_seasonC.Text))
            {
                choiceq += " and LastSeason is Null";
            }
            else
            {
                choiceq += " and LastSeason = '" + cb_seasonC.Text + "'";
            }
            if (String.IsNullOrEmpty(sportC.TxtCode.Text))
            {
                choiceq += " and SportsCD is Null";
            }
            else
            {
                choiceq += " and SportsCD = '" + sportC.TxtCode.Text + "'";
            }
            if (String.IsNullOrEmpty(makershohinC.TxtCode.Text))
            {
                choiceq += " and MakerItem is Null";
            }
            else
            {
                choiceq += " and MakerItem = '" + makershohinC.TxtCode.Text + "'";
            }
            if(String.IsNullOrEmpty(TB_dateC.Text))
            {
                dateq = " and ChangeDate is Null";
            }
            else
            {
                dateq += " and ChangeDate = '" + TB_dateC.Text + "'";
            }
            if (GV_item.Rows.Count >0)
            {
                DataRow[] dr;

                if(RB_item.Checked)
                {
                    dr = dtc.Select(choiceq + dateq);
                }
                else
                {
                    dr = dtsku.Select(choiceq + dateq);
                }

                if (dr.Length > 0)
                {
                    int i;
                    for (i = 0; i < dr.Length; i++)
                    {
                        dr[i]["CheckBox"] = "1";
                    }
                    GV_item.Refresh();
                    brandC.Clear();
                    sportC.Clear();
                    segmentC.Clear();
                    CB_yearC.Text = string.Empty;
                    cb_seasonC.Text = string.Empty;
                    TB_dateC.Text = string.Empty;
                    makershohinC.Clear();
                }
            }
        }
        private void btn_selectall_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in GV_item.Rows)
            {
                row.Cells["ck"].Value = "1";
                String itemdata = bl.DataTableToXml(dt);
                String skudata = bl.DataTableToXml(dtsku);
                DataTable dtdata = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata,"", TB_headerdate.Text, "2");


                if (dtdata.Rows.Count > 0)
                {
                    string itemcd = dtdata.Rows[0]["ItemCD"].ToString();
                    string qskuupdate = " ItemCD = '" + itemcd + "'";
                    qskuupdate += " and ChangeDate <= '" + TB_headerdate.Text + "'";
                    DataRow[] drte = dtsku.Select(qskuupdate);
                    if (drte.Length > 0)
                    {
                        for (int i = 0; i < drte.Length; i++)
                        {
                            drte[i]["CheckBox"] = "1";
                        }
                    }
                }
            }
        }
        private void btn_releaseall_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in GV_item.Rows)
            {
                String itemdata = bl.DataTableToXml(dt);
                String skudata = bl.DataTableToXml(dtsku);
                DataTable dtdata = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata,"", TB_headerdate.Text, "2");


                if (dtdata.Rows.Count > 0)
                {
                    string itemcd = dtdata.Rows[0]["ItemCD"].ToString();
                    string qskuupdate = " ItemCD = '" + itemcd + "'";
                    qskuupdate += " and ChangeDate <= '" + TB_headerdate.Text + "'";
                    DataRow[] drte = dtsku.Select(qskuupdate);
                    if (drte.Length > 0)
                    {
                        for (int i = 0; i < drte.Length; i++)
                        {
                            drte[i]["CheckBox"] = "0";
                        }
                    }
                }
                row.Cells["ck"].Value = "0";
            }
        }
        private void btn_Copy_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(TB_dateE.Text))
            {
                string date = "";
                date = "  ChangeDate = '" + TB_dateE.Text + "'";
                date += " and CheckBox = 1";
                string copyq = "";
                if (!string.IsNullOrEmpty(choiceq))
                {
                    copyq = choiceq + " and " +date;
                }
                else
                {
                    copyq = date;
                }
                DataRow[] dr = dt.Select(copyq);
                if( dr.Length ==0 )
                {
                    string q = "CheckBox =1";
                    DataRow[] dr1;
                    dr1 = dt.Select(q);
                    if (dr1.Length > 0)
                    { 
                    DataTable dt1 = dr1.CopyToDataTable();
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dt1.Rows[i]["ChangeDate"] = TB_dateE.Text;
                            //dt1.Rows[i]["Rate"] = TB_rate_E.Text;
                            //decimal rate = Convert.ToDecimal(TB_rate_E.Text);
                            //decimal con = (decimal)0.01;
                            //decimal listprice = Convert.ToDecimal(dt1.Rows[i]["PriceOutTax"]);
                            //dt1.Rows[i]["PriceWithoutTax"] = Math.Round(listprice * (rate * con)).ToString();
                        }
                         DataRow[] drskuscopy;
                            drskuscopy = dtsku.Select("  ChangeDate = '" + TB_dateE.Text + "'");
                            String datat = bl.DataTableToXml(dt);
                            //string storecd=CB_store.SelectedValue.ToString()
                            DataTable dtr = bl.M_SKU_SelectFor_SKU_Update(datat,"","","","1");
                            if (drskuscopy.Length >0)
                            {
                                if(dtr.Rows.Count >0)
                                {
                                    for (int i = 0; i < drskuscopy.Length; i++)
                                    {
                                        dtr.Rows[i]["ItemCD"] = dtr.Rows[i]["ItemCD"];
                                        dtr.Rows[i]["ChangeDate"] = TB_dateE.Text;
                                        dtr.Rows[i]["Rate"] = TB_rate_E.Text;
                                        //dtr.Rows[i]["CheckBox"] = "1";
                                        //drskuscopy[i]["PriceOutTax"] = LB_priceouttax.Text;
                                        //drskuscopy[i]["PriceWithoutTax"] = TB_pricewithouttax.Text;
                                        //drskuscopy[i]["InsertOperator"] = operatorCd;
                                        //drskuscopy[i]["InsertDateTime"] = bbl.GetDate();
                                        //drskuscopy[i]["UpdateOperator"] = operatorCd;
                                        //drskuscopy[i]["UpdateDateTime"] = bbl.GetDate();
                                        decimal rate = Convert.ToDecimal(TB_rate_E.Text);
                                        decimal con = (decimal)0.01;
                                        //string priceouttax = drskuscopy[i]["PriceOutTax"].ToString();
                                        decimal listprice = Convert.ToDecimal(drskuscopy[i]["PriceOutTax"]);
                                        dtr.Rows[i]["PriceWithoutTax"] = Math.Round(listprice * (rate * con)).ToString();
                                    }
                                    dtsku.Merge(dtr);
                                }
                            }
                            else
                            {
                                if (dtr.Rows.Count > 0)
                                {
                                    DataRow rowsku;
                                    rowsku = dtsku.NewRow();
                                    rowsku["Tempkey"] = "1";
                                    rowsku["CheckBox"] = "0";
                                    //rowsku["AdminNO"] = dtr.Rows[0]["AdminNO"];
                                    //rowsku["SKUCD"] = dtr.Rows[0]["SKUCD"];
                                    //rowsku["SizeName"] = dtr.Rows[0]["SizeName"];
                                    //rowsku["ColorName"] = dtr.Rows[0]["ColorName"];
                                    //rowsku["LastYearTerm"] = dtr.Rows[0]["LastYearTerm"];
                                    //rowsku["LastSeason"] = dtr.Rows[0]["LastSeason"];
                                    rowsku["MakerItem"] = dtr.Rows[0]["MakerItem"];
                                    rowsku["ItemCD"] = dtr.Rows[0]["ItemCD"];
                                    rowsku["ItemName"] = dtr.Rows[0]["ItemName"];
                                    rowsku["ChangeDate"] = TB_dateE.Text;
                                    //rowsku["Rate"] = TB_rate_E.Text;
                                    //decimal rate = Convert.ToDecimal(TB_rate_E.Text);
                                    //decimal con = (decimal)0.01;
                                    ////string priceouttax = drskuscopy[i]["PriceOutTax"].ToString();
                                    //decimal listprice = Convert.ToDecimal(dtr.Rows[0]["PriceOutTax"]);
                                    //rowsku["PriceWithoutTax"] = Math.Round(listprice * (rate * con)).ToString();
                                    rowsku["InsertOperator"] = operatorCd;
                                    rowsku["InsertDateTime"] = bbl.GetDate();
                                    rowsku["UpdateOperator"] = operatorCd;
                                    rowsku["UpdateDateTime"] = bbl.GetDate();
                                    dtsku.Rows.Add(rowsku);
                                    dtsku.AcceptChanges();
                                    //GV_item.Refresh();
                                    //GV_item.DataSource = dt;
                                    //dv.RowStateFilter = DataViewRowState.CurrentRows;
                                }
                            }
                            dt.Merge(dt1);
                        }
                    }
                }
                else
                {
                    bbl.ShowMessage("E224");
                    TB_dateE.Focus();
                }
            }
        }
        private void btn_update_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(TB_rate_E.Text))
            {
                string updateq = "CheckBox = 1";
                DataRow[] drupdate = dt.Select(updateq);
                //DataTable dtupdate = dt.Select(updateq).CopyToDataTable();
                //if (dtupdate.Rows.Count > 0)
                //{
                //    dtupdate["Rate"] = TB_rate.Text;
                //    //drupdate["PriceOutTax"] = LB_priceouttax.Text;
                //}
                if (drupdate.Length > 0)
                {
                    for (int i = 0; i < drupdate.Length; i++)
                    {
                        drupdate[i]["Rate"] = TB_rate_E.Text;
                        decimal rate = Convert.ToDecimal(TB_rate_E.Text);
                        decimal con = (decimal)0.01;
                        decimal listprice = Convert.ToDecimal(drupdate[i]["PriceOutTax"]);
                        drupdate[i]["PriceWithoutTax"] = Math.Round(listprice * (rate * con)).ToString();
                    }

                    String itemdata = bl.DataTableToXml(dt);
                    String skudata = bl.DataTableToXml(dtsku);
                    DataTable dtdata = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata,"", TB_headerdate.Text,"2");
                    if(dtdata.Rows.Count >0)
                    {
                        string itemcd = dtdata.Rows[0]["ItemCD"].ToString();
                        string qskuupdate = " ItemCD = '" + itemcd + "'";
                        qskuupdate += " and ChangeDate <= '" + TB_headerdate.Text + "'";
                        DataRow[] drte = dtsku.Select(qskuupdate);
                        if (drte.Length > 0)
                        {
                            for (int i = 0; i < drte.Length; i++)
                            {
                                drte[i]["Rate"] = dtdata.Rows[0]["Rate"];
                                decimal rate = Convert.ToDecimal(dtdata.Rows[0]["Rate"]);
                                decimal con = (decimal)0.01;
                                //string priceouttax = drskuscopy[i]["PriceOutTax"].ToString();
                                decimal listprice = Convert.ToDecimal(dtdata.Rows[0]["PriceOutTax"]);
                                drte[i]["PriceWithoutTax"] = Math.Round(listprice * (rate * con)).ToString();
                            }
                        }
                    }
                }
            }
        }
        private void btn_delete_Click(object sender, EventArgs e)
        {
            DataRow[] rows  = dt.Select(" CheckBox =1");

            foreach (DataRow row in rows)
            {
               
                String itemdata = bl.DataTableToXml(dt);
                String skudata = bl.DataTableToXml(dtsku);
                DataTable dtskudel = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata,"", TB_headerdate.Text, "2");
                if (dtskudel.Rows.Count > 0)
                {
                    string itemcd = dtskudel.Rows[0]["ItemCD"].ToString();
                    string qskuupdate = " ItemCD = '" + itemcd + "'";
                    qskuupdate += " and ChangeDate <= '" + TB_headerdate.Text + "'";
                    DataRow[] drte = dtsku.Select(qskuupdate);
                    if (drte.Length > 0)
                    {
                        foreach(DataRow rowd in drte)
                        dtsku.Rows.Remove(rowd);
                    }
                }
                dt.Rows.Remove(row);
            }
                
                
        }
        private void F12()
        {
            if(dt.Rows.Count >0)
            {
                //string insertq="Insert into M_ItemOrderPrice valuse("
                String   itemdata = bl.DataTableToXml(dt);
                String skudata = bl.DataTableToXml(dtsku);
                //string storecd=CB_store.SelectedValue.ToString()
                DataTable dtr = bl.Mastertoroku_Shiretanka_Insert(itemdata, skudata, shiiresaki.TxtCode.Text,CB_store.SelectedValue.ToString());
            }
        }
        private void GV_item_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 0)
                {
                    string ck = GV_item.Rows[e.RowIndex].Cells["ck"].State.ToString();
                    //string ck = GV_item.Rows[e.RowIndex].Cells["ck"].
                    string ck1 = GV_item.Rows[e.RowIndex].Cells["ck"].Value.ToString();
                    if (ck1 == "0")
                    {

                        GV_item.Rows[e.RowIndex].Cells["ck"].Value = "1";
                        String itemdata = bl.DataTableToXml(dt);
                        String skudata = bl.DataTableToXml(dtsku);
                        DataTable dtdata = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata, "", TB_headerdate.Text, "2");
                        if (dtdata.Rows.Count > 0)
                        {
                            string itemcd = dtdata.Rows[0]["ItemCD"].ToString();
                            string qskuupdate = " ItemCD = '" + itemcd + "'";
                            qskuupdate += " and ChangeDate <= '" + TB_headerdate.Text + "'";
                            DataRow[] drte = dtsku.Select(qskuupdate);
                            if (drte.Length > 0)
                            {
                                for (int i = 0; i < drte.Length; i++)
                                {
                                    drte[i]["CheckBox"] = "1";
                                }
                            }
                        }
                    }
                    else
                    {
                        String itemdata = bl.DataTableToXml(dt);
                        String skudata = bl.DataTableToXml(dtsku);
                        DataTable dtdata = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata, "", TB_headerdate.Text, "2");
                        if (dtdata.Rows.Count > 0)
                        {
                            string itemcd = dtdata.Rows[0]["ItemCD"].ToString();
                            string qskuupdate = " ItemCD = '" + itemcd + "'";
                            qskuupdate += " and ChangeDate <= '" + TB_headerdate.Text + "'";
                            GV_item.Rows[e.RowIndex].Cells["ck"].Value = "0";
                            DataRow[] drte = dtsku.Select(qskuupdate);
                            if (drte.Length > 0)
                            {
                                for (int i = 0; i < drte.Length; i++)
                                {
                                    drte[i]["CheckBox"] = "1";
                                }
                            }
                        }
                    }
                }
            }
        }
        private void BT_Capture_Click(object sender, EventArgs e)
        {
            //Excel();
            //ExportCSV();
        }
    }
}
