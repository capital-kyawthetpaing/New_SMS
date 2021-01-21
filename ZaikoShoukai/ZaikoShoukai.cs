using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;
using Search;
using System.Diagnostics;
//using CsvHelper;
using System.IO;
using ClosedXML.Excel;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace ZaikoShoukai
{
    public partial class ZaikoShoukai : FrmMainForm
    {
        Base_BL bbl = new Base_BL();
        M_SKU_Entity msku_Entity;
        M_SKUInfo_Entity msInfo_Entity;
        M_SKUTag_Entity msT_Entity;
        D_Stock_Entity ds_Entity;
        ZaikoShoukai_BL zaibl;
        DataTable dtData;
        string adminno = "", soukocd = "";
        string shohinmei, color, size, item, skucd, brand, jancd, makercd,changedate,soukoname;
        int type = 0;
        int chktype = 0;
        int chkunApprove = 0;
        public  ZaikoShoukai()
        {
            InitializeComponent();
            zaibl = new ZaikoShoukai_BL();
            dtData = new DataTable();
        }
        private void ZaikoShoukai_Load(object sender, EventArgs e)
        {
            InProgramID = "ZaikoShoukai";
            StartProgram();
            BindCombo();
            LB_ChangeDate.Text =bbl.GetDate();
            ckM_RB_or.Checked = true;
            ModeVisible = false;
            base.Btn_F10.Text = "CSV(F10)";
            CB_Soko.Focus();
            this.colSKUCD.Frozen = true;
            this.商品名.Frozen = true;
            this.カラー.Frozen = true;
            this.サイズ.Frozen = true;
            this.Shiiresaki.NameWidth = 369;
            this.Maker.NameWidth = 369;
            this.SearchBrand.NameWidth = 369;
            this.Sports.NameWidth = 341;
            this.CB_Tagu1.AcceptKey = true;
            this.CB_Tagu2.AcceptKey = true;
            this.CB_Tagu3.AcceptKey = true;
            this.CB_Tagu4.AcceptKey = true;
            this.CB_Tagu5.AcceptKey = true;
            AddCol();
            //GV_Zaiko.DisabledColumn("colSKUCD,商品名,カラー,サイズ,店舗名,SoukoName,棚番,現在庫,入荷予定,引当可能,メーカー,JANCD,colBrand,ITEM,メーカー商品CD,最速入荷日,基準在庫,販売定価,標準原価");
        }

        protected override void EndSec()
        {
            this.Close();
        }
        private void BindCombo()
        {
            string ymd = bbl.GetDate();
            CB_Soko.Bind(ymd,StoreCD);
            CB_Soko.SelectedValue = SoukoCD;
            CB_year.Bind(ymd);
            CB_Season.Bind(ymd);
            CB_ReserveCD.Bind(ymd);
            CB_NoticesCD.Bind(ymd);
            CB_PostageCD.Bind(ymd);
            CB_OrderAttentionCD.Bind(ymd);
            CB_Tagu1.Bind(ymd);
            CB_Tagu1.Bind(ymd);
            CB_Tagu2.Bind(ymd);
            CB_Tagu3.Bind(ymd);
            CB_Tagu4.Bind(ymd);
            CB_Tagu5.Bind(ymd);
        }
        private void F11()
        {
            if (ErrorCheck())
            {
                type = (CKB_searchsuru.Checked && RB_item.Checked) ? 1 : (CKB_searchsuru.Checked && RB_Makashohincd.Checked) ? 2 : 3;

                chktype = ckM_CKB_suru.Checked ? 1 : 0;

                chkunApprove = chk_uncheckApprove.Checked ? 1 : 0;
                
                string[]  store = TB_Bikokeyword.ToString().Split(','); 
                msku_Entity = GetDataEntity();
                msInfo_Entity = GetInfoEntity();
                msT_Entity = GetTagEntity();
                ds_Entity = GetStockEntity();

                dtData = zaibl.ZaikoShoukai_Search(msku_Entity, msInfo_Entity, msT_Entity,ds_Entity, type ,chktype, chkunApprove);
              
                if (dtData.Rows.Count > 0)
                {
                    GV_Zaiko.Refresh();
                    GV_Zaiko.DataSource = dtData;
                }
                else
                {
                    GV_Zaiko.DataSource = null;
                    bbl.ShowMessage("E128");
                    dtData.Clear();
                }
            }
        }
        private void AddCol()
        {
            if (dtData.Columns.Count == 0)
            {
                dtData.Columns.Add("AdminNO");
                dtData.Columns.Add("SKUCD");
                dtData.Columns.Add("商品名");
                dtData.Columns.Add("カラー");
                dtData.Columns.Add("サイズ");
                dtData.Columns.Add("店舗CD");
                dtData.Columns.Add("店舗名");
                dtData.Columns.Add("倉庫CD");
                dtData.Columns.Add("倉庫名");
                dtData.Columns.Add("棚番");
                dtData.Columns.Add("在庫数");
                dtData.Columns.Add("入荷予定数");
                dtData.Columns.Add("引当可能数");
                dtData.Columns.Add("メーカー在庫数");
                dtData.Columns.Add("JANCD");
                dtData.Columns.Add("ブランドCD");
                dtData.Columns.Add("colBrand");
                dtData.Columns.Add("ITEM");
                dtData.Columns.Add("メーカー商品CD");
                dtData.Columns.Add("最速入荷日");
                dtData.Columns.Add("基準在庫");
                dtData.Columns.Add("販売定価");
                dtData.Columns.Add("標準原価");
            }
        }
        public M_SKU_Entity GetDataEntity()
        {
            msku_Entity = new M_SKU_Entity()
            {
                ChangeDate=LB_ChangeDate.Text,
                MainVendorCD = Shiiresaki.TxtCode.Text,
                MakerVendorCD = Maker.TxtCode.Text,
                BrandCD = SearchBrand.TxtCode.Text,
                SKUName = TB_Shohinmei.Text,
                JanCD=jan.TxtCode.Text,
                SKUCD=sku.TxtCode.Text,
                MakerItem= TB_mekashohinCD.Text,
                ITemCD=TB_item.Text,
                CommentInStore= TB_Bikokeyword.Text,
                ReserveCD = CB_ReserveCD.SelectedValue.ToString(),
                NoticesCD = CB_NoticesCD.SelectedValue.ToString(),
                PostageCD = CB_PostageCD.SelectedValue.ToString(),
                OrderAttentionCD = CB_OrderAttentionCD.SelectedValue.ToString(),
                SportsCD =Sports.TxtCode.Text,
                InputDateFrom=TB_ShinkitorokuF.Text,
                InputDateTo=TB_ShinkitorokuT.Text,
                UpdateDateFrom= TＢ_SaiShuhenkobiF.Text,
                UpdateDateTo= TB_SaiShuhenkobiT.Text,
                ApprovalDateFrom=TB_ShoninbiF.Text,
                ApprovalDateTo=TB_ShoninbiT.Text,
            };
            return msku_Entity;
        }
        public M_SKUInfo_Entity GetInfoEntity()
        {
            msInfo_Entity = new M_SKUInfo_Entity()
            {
                YearTerm = CB_year.SelectedValue.ToString(),
                Season=CB_Season.SelectedValue.ToString(),
                CatalogNO=TB_Catalog.Text.ToString(),
                InstructionsNO=TB_Shijishobengo.Text,
            };
            return msInfo_Entity;
        }
        public M_SKUTag_Entity GetTagEntity()
        {
            msT_Entity = new M_SKUTag_Entity()
            {
                TagName1 = CB_Tagu1.Text,
                TagName2 =CB_Tagu2.Text,
                TagName3=CB_Tagu3.Text,
                TagName4=CB_Tagu4.Text,
                TagName5=CB_Tagu5.Text,
            };
            return msT_Entity;
        }
        public  D_Stock_Entity GetStockEntity()
        {
            ds_Entity = new D_Stock_Entity()
            {
                SoukoCD = CB_Soko.SelectedValue.ToString(),
                RackNOFrom=TB_RackNoF.Text,
                RackNOTo=TB_RackNoT.Text,

            };
            return ds_Entity;
        }
        public override void FunctionProcess(int index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
            switch (index + 1)
            {
                
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        CanCelData();
                    }
                    break;
                case 10:
                    if (GV_Zaiko.DataSource != null)
                    {
                        if (bbl.ShowMessage("Q203") == DialogResult.Yes)
                        {
                            Excel();
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E128");
                    }
                    break;
                case 11:
                   F11();
                    break;
            }
        }
        private void CanCelData()
        {
            CB_Soko.SelectedIndex = 1;
            CB_year.Text = string.Empty;
            CB_OrderAttentionCD.Text = string.Empty;
            CB_ReserveCD.Text = String.Empty;
            CB_PostageCD.Text = String.Empty;
            CB_Season.Text = string.Empty;
            CB_NoticesCD.Text = string.Empty;
            CB_Tagu1.Text = String.Empty;
            CB_Tagu2.Text = String.Empty;
            CB_Tagu3.Text = String.Empty;
            CB_Tagu4.Text = String.Empty;
            CB_Tagu5.Text = String.Empty;
            TB_item.Text = string.Empty;
            TB_mekashohinCD.Text = string.Empty;
            TB_Bikokeyword.Text = string.Empty;
            TB_mekashohinCD.Text = string.Empty;
            TB_Catalog.Text = string.Empty;
            TB_item.Text = string.Empty;
            TB_Shijishobengo.Text = string.Empty;
            TB_ShinkitorokuF.Text = String.Empty;
            TB_ShinkitorokuT.Text = string.Empty;
            TB_Shohinmei.Text = string.Empty;
            TB_ShoninbiF.Text = string.Empty;
            TB_ShoninbiT.Text = string.Empty;
            TB_RackNoF.Text = string.Empty;
            TB_RackNoT.Text = string.Empty;
            TＢ_SaiShuhenkobiF.Text = string.Empty;
            TB_SaiShuhenkobiT.Text = String.Empty;
            jan.Clear();
            sku.Clear();
            Shiiresaki.Clear();
            SearchBrand.Clear();
            Maker.Clear();
            Sports.Clear();
            ckM_RB_or.Checked = true;
            ckM_RB_and.Checked = false;
            chk_uncheckApprove.Checked = false;
            ckM_CKB_suru.Checked = false;
            CKB_searchsuru.Checked = false;
            RB_item.Checked = false;
            RB_Makashohincd.Checked = false;
            CB_Soko.Focus();
            GV_Zaiko.DataSource = null;
            dtData.Clear();
           
        }
        private void Excel()
        {
                if (!ErrorCheck())
                {
                    return;
                }

                if (dtData.Columns.Contains("AdminNO"))
                {
                    dtData.Columns.Remove("AdminNO");
                }
                string folderPath = "C:\\Excel\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Filter = "Excel Files|*.xlsx;";
                savedialog.Title = "Save";
                savedialog.FileName = "ZaikoShoukai";
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
                            wb.Worksheets.Add(dtData, "worksheet");
                            wb.SaveAs(savedialog.FileName);
                            bbl.ShowMessage("I203", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        }

                        Process.Start(Path.GetDirectoryName(savedialog.FileName));
                    }
                }
        }
        private void ZaikoShoukai_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        private void TB_RackNoT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_RackNoF.Text) && !String.IsNullOrEmpty(TB_RackNoT.Text))
                {
                    if (String.Compare(TB_RackNoF.Text, TB_RackNoT.Text) == 1)
                    {
                        bbl.ShowMessage("E106");
                    }
                }
            }
        }
        private void Shiiresaki_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Shiiresaki.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(Shiiresaki.TxtCode.Text))
                {
                    if (!Shiiresaki.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        Shiiresaki.SetFocus(1);
                        //Shiiresaki.Value1 = Shiiresaki.TxtCode.Text;
                        //Shiiresaki.Value2 = Shiiresaki.LabelText;
                    }
                    //else
                    //{
                    //    bbl.ShowMessage("E101");
                    //    Shiiresaki.SetFocus(1);
                    //}
                }
            }
        }
        private void Maker_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Maker.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(Maker.TxtCode.Text))
                {
                    if (!Maker.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        Maker.SetFocus(1);
                    }
                   
                }
            }
        }
        private void SearchBrand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchBrand.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(SearchBrand.TxtCode.Text))
                {
                    if (!SearchBrand.SelectData())
                    {
                        //SearchBrand.Value1 = SearchBrand.TxtCode.Text;
                        //SearchBrand.Value2 = SearchBrand.LabelText;
                        bbl.ShowMessage("E101");
                        SearchBrand.SetFocus(1);
                    }
                    //else
                    //{
                    //    bbl.ShowMessage("E101");
                    //    SearchBrand.SetFocus(1);
                    //}
                }
            }
        }
        private void jan_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(jan.TxtCode.Text))
            {
                if (!jan.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    jan.Focus();
                }
            }
        }
        private void Shiiresaki_Enter(object sender, EventArgs e)
        {
            Shiiresaki.ChangeDate = bbl.GetDate();
            Shiiresaki.Value1 = "1";
        }
        private void Maker_Enter(object sender, EventArgs e)
        {
            Maker.ChangeDate = bbl.GetDate();
            Maker.Value1 = "1";
        }
        private void sku_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(!String.IsNullOrEmpty(sku.TxtCode.Text))
            {
                
                if(!sku.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    sku.Focus();
                }
            }
        }
        private void CKB_searchsuru_CheckedChanged(object sender, EventArgs e)
        {
            if(CKB_searchsuru.Checked == false)
            {
                RB_item.Checked = false;
                RB_Makashohincd.Checked = false;
            }
            else
            {
                RB_item.Checked = true;
            }
        }
        private void Sports_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Sports.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(Sports.TxtCode.Text))
                {
                    if (!Sports.SelectData())
                    {
                        Sports.Value1 = "202";
                        bbl.ShowMessage("E101");
                        Sports.SetFocus(1);
                    }
                }
            }
        }
        private void Sports_Enter(object sender, EventArgs e)
        {
            Sports.ChangeDate = bbl.GetDate();
            Sports.Value1 = "202";
        }
        private void TB_ShinkitorokuT_KeyDown(object sender, KeyEventArgs e)

        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_ShinkitorokuF.Text) && !String.IsNullOrEmpty(TB_ShinkitorokuT.Text))
                {
                    if (Convert.ToDateTime(TB_ShinkitorokuF.Text) > Convert.ToDateTime(TB_ShinkitorokuT.Text))
                    {
                        bbl.ShowMessage("E104");
                        TB_ShinkitorokuF.Focus();
                    }
                }
            }
        }
        private void TB_SaiShuhenkobiT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TＢ_SaiShuhenkobiF.Text) && !String.IsNullOrEmpty(TB_SaiShuhenkobiT.Text))
                {
                    if (Convert.ToDateTime(TＢ_SaiShuhenkobiF.Text) > Convert.ToDateTime(TB_SaiShuhenkobiT.Text))
                    {
                        bbl.ShowMessage("E104");
                        TＢ_SaiShuhenkobiF.Focus();
                    }
                }
            }
        }
        private void TB_ShoninbiT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_ShoninbiF.Text) && !String.IsNullOrEmpty(TB_ShoninbiT.Text))
                {
                    if (Convert.ToDateTime(TB_ShoninbiF.Text) > Convert.ToDateTime(TB_ShoninbiT.Text))
                    {
                        bbl.ShowMessage("E104");
                        TB_ShoninbiF.Focus();
                    }
                }
            }
        }
        private void ckM_BT_hyoji_Click(object sender, EventArgs e)
        {
            F11();
        }
        private void GV_Zaiko_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == GV_Zaiko.Columns["btnPlan"].Index)
                {
                    //商品名,カラー,サイズ,店舗名,SoukoName,棚番,現在庫,入荷予定,引当可能,メーカー,JANCD,ブランド,ITEM,メーカー商品CD,最速入荷日,基準在庫,販売定価,標準原価"
                    adminno = GV_Zaiko.Rows[e.RowIndex].Cells["AdNO"].Value.ToString();
                    soukocd =GV_Zaiko.Rows[e.RowIndex].Cells["sokucd"].Value.ToString();
                    //soukoname = GV_Zaiko.Rows[e.RowIndex].Cells[""].Value.ToString();
                    skucd = GV_Zaiko.Rows[e.RowIndex].Cells["colSKUCD"].Value.ToString();
                    shohinmei = GV_Zaiko.Rows[e.RowIndex].Cells["商品名"].Value.ToString();
                    color = GV_Zaiko.Rows[e.RowIndex].Cells["カラー"].Value.ToString();
                    size = GV_Zaiko.Rows[e.RowIndex].Cells["サイズ"].Value.ToString();
                    jancd = GV_Zaiko.Rows[e.RowIndex].Cells["colJan"].Value.ToString();
                    brand = GV_Zaiko.Rows[e.RowIndex].Cells["colBrand"].Value.ToString();
                    item = GV_Zaiko.Rows[e.RowIndex].Cells["colItem"].Value.ToString();
                    makercd = GV_Zaiko.Rows[e.RowIndex].Cells["メーカー商品CD"].Value.ToString();
                    changedate = LB_ChangeDate.Text;
                    Search_PlanArrival frmVendor = new Search_PlanArrival(adminno, skucd, shohinmei, color, size, jancd, brand, item, makercd, changedate, soukocd, soukoname, StoreCD);
                    frmVendor.ShowDialog();
                }
            }
        }
        private bool ErrorCheck()
        {
            if (!String.IsNullOrEmpty(Shiiresaki.TxtCode.Text))
            {
                if (!Shiiresaki.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    Shiiresaki.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(Maker.TxtCode.Text))
            {
                if (!Maker.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    Maker.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(SearchBrand.TxtCode.Text))
            {
                if (!SearchBrand.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SearchBrand.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(jan.TxtCode.Text))
            {
                if(!jan.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    jan.SetFocus(1);
                    return false;
                }
            }
            if(!String.IsNullOrEmpty(sku.TxtCode.Text))
            {
                if(!sku.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    sku.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(Sports.TxtCode.Text))
            {
                if (!Sports.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    Sports.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(TB_ShinkitorokuF.Text) && !String.IsNullOrEmpty(TB_ShinkitorokuT.Text))
            {
                if (Convert.ToDateTime(TB_ShinkitorokuF.Text) > Convert.ToDateTime(TB_ShinkitorokuT.Text))
                {
                    bbl.ShowMessage("E104");
                    TB_ShinkitorokuF.Focus();
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(TB_RackNoF.Text) && !String.IsNullOrEmpty(TB_RackNoT.Text))
            {
                if (String.Compare(TB_RackNoF.Text, TB_RackNoT.Text) == 1)
                {
                    bbl.ShowMessage("E106");
                    TB_RackNoF.Focus();
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(TＢ_SaiShuhenkobiF.Text) && !String.IsNullOrEmpty(TB_SaiShuhenkobiT.Text))
            {
                if (Convert.ToDateTime(TＢ_SaiShuhenkobiF.Text) > Convert.ToDateTime(TB_SaiShuhenkobiT.Text))
                {
                    bbl.ShowMessage("E104");
                    TＢ_SaiShuhenkobiF.Focus();
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(TB_ShoninbiF.Text) && !String.IsNullOrEmpty(TB_ShoninbiT.Text))
            {
                if (Convert.ToDateTime(TB_ShoninbiF.Text) > Convert.ToDateTime(TB_ShoninbiT.Text))
                {
                    bbl.ShowMessage("E104");
                    TB_ShoninbiF.Focus();
                    return false;
                }
            }
            return true;
        }
    }
}
