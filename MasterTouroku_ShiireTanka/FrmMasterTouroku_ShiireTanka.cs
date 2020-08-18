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
using System.Data.OleDb;
using System.Globalization;

namespace MasterTouroku_ShiireTanka
{
    public partial class FrmMasterTouroku_ShiireTanka : FrmMainForm
    {
        bool cb_focus = false;
        M_ItemOrderPrice_Entity m_IOE;
        M_ITEM_Entity m_IE;
        MasterTouroku_ShiireTanka_BL bl;
        DataView dv;
        DataTable dt= new DataTable();
        DataTable dtsku=new DataTable();
        DataTable deldt=new DataTable();
        DataTable dtdeljan = new DataTable();
        DataTable dtview;
        string choiceq = "";
        string operatorCd;
        string btn ;
        DataTable dtExcel;
        string vendorcd, storecd;
        public FrmMasterTouroku_ShiireTanka()
        {
            InitializeComponent();
            bl = new MasterTouroku_ShiireTanka_BL();
            m_IOE=new M_ItemOrderPrice_Entity();
            m_IE=new M_ITEM_Entity();
            dv = new DataView();
            this.KeyPreview = true;
           // GV_item.PreviewKeyDown =
        }
        private void FrmMasterTouroku_ShiireTanka_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_ShiireTanka";
            StartProgram();
            RB_itemandsku_Check();
            BindCombo();
            SetRequireField();
            shiiresaki.SetFocus(1);
            this.segment.CodeWidth = 90;
            this.sport.NameWidth = 260;
            this.segmentC.CodeWidth = 90;
            this.sportC.NameWidth = 260;
            operatorCd = InOperatorCD;
            TB_headerdate.Text = bbl.GetDate();
            GV_item.DisabledColumn("ブランド,競技,商品分類,年度,シーズン,メーカー品番,ITEM,商品名,サイズ,カラー,SKUCD,定価");
            itemcd.CodeWidth = 100;
            itemcd.NameWidth = 280;
            LB_priceouttax.Text = "";
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
                        shiiresaki.SetFocus(1);
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
                    shiiresaki.ChangeDate = bl.GetDate();
                    if (shiiresaki.SelectData())
                    {
                        //shiiresaki.Value1 = shiiresaki.TxtCode.Text;
                        //shiiresaki.Value2 = shiiresaki.LabelText;
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
            sport.Value1 ="202";
            sport.ChangeDate = bbl.GetDate();
            //sport.Value3 = "202";
        }
        private void sport_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(sport.TxtCode.Text))
                {
                    sport.ChangeDate = bl.GetDate();
                    if (sport.SelectData())
                    {
                        sport.Value1 ="202";
                        //sport.Value2 = sport.LabelText;
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
                    segment.ChangeDate = bl.GetDate();
                    if (segment.SelectData())
                    {
                        segment.Value1 = "203";
                        //segment.Value2 = segment.LabelText;
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
            segment.ChangeDate = bbl.GetDate();
        }
        private void brand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                brand.ChangeDate = bl.GetDate();
                if (!string.IsNullOrEmpty(brand.TxtCode.Text))
                {
                    if (!brand.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        brand.SetFocus(1);
                    }
                    
                }
            }
        }
        private void makershohin_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(!String.IsNullOrEmpty(makershohin.TxtCode.Text))
                {
                    if (!makershohin.IsExists(2))
                    {
                        bl.ShowMessage("E101");
                        makershohin.Focus();
                    }
                }
            }
        }
        private void brandC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(brandC.TxtCode.Text))
                {
                    brandC.ChangeDate = bl.GetDate();
                    if (!brandC.SelectData())
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
                    sportC.ChangeDate = bl.GetDate();
                    if (sportC.SelectData())
                    {
                        sportC.Value1 = "202";
                        //sportC.Value2 = sportC.LabelText;
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
            sportC.ChangeDate = bl.GetDate();
        }
        private void segmentC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(segmentC.TxtCode.Text))
                {
                    segmentC.ChangeDate = bl.GetDate();
                    if (segmentC.SelectData())
                    {
                        segmentC.Value1 = "203";
                        //segmentC.Value2 = segmentC.LabelText;
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
            segment.ChangeDate = bl.GetDate();
        }
        private void makershohinC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(makershohinC.TxtCode.Text))
                {
                    if (!makershohinC.IsExists(2))
                    {
                        bl.ShowMessage("E101");
                        makershohinC.Focus();
                    }
                }
            }
        }
        private void itemcd_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(itemcd.TxtCode.Text))
                {
                    itemcd.ChangeDate = bl.GetDate();
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
       
        private void makershohin_Enter(object sender, EventArgs e)
        {
            makershohin.Value1 = "3";
            makershohin.ChangeDate = bl.GetDate();
        }
        private void makershohinC_Enter(object sender, EventArgs e)
        {
            makershohinC.Value1 = "3";
            makershohinC.ChangeDate = bl.GetDate();
        }
        #endregion
        private bool ErrorCheckMain()
        {
            if (!RequireCheck(new Control[] { shiiresaki.TxtCode,TB_headerdate,CB_store })) //Step1
                return false;
           
            if (!shiiresaki.IsExists(2))
            {
                bbl.ShowMessage("E101");
                shiiresaki.Focus();
                return false;

            }
            if (RB_koten.Checked == true)
            {
                if (!base.CheckAvailableStores(CB_store.SelectedValue.ToString()))
                {
                    bbl.ShowMessage("E141");
                    CB_store.Focus();
                    cb_focus = true;
                    return false;
                }
            }
            return true;
        } 
        private bool ErrorCheckSubdisplay()
        {
            if (!String.IsNullOrEmpty(brand.TxtCode.Text))
            {
                if (!brand.IsExists(2))
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
                    //bbl.ShowMessage("E101");
                    //makershohin.Focus();
                    //return false;
                }
            }
           

            return true;
        }
        private bool ErrorCheckAdd()
        {
            if (RB_item.Checked == true)
            {
                if(!RequireCheck(new Control[] {itemcd.TxtCode,TB_date_add,LB_priceouttax,TB_rate,TB_pricewithouttax }))
                    return false;
                
                if (!itemcd.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    itemcd.Focus();
                    return false;
                }
            }
            return true;
        }

        private void SetRequireField()
        {
            shiiresaki.TxtCode.Require(true);
            TB_headerdate.Require(true);
            CB_store.Require(true);
            itemcd.TxtCode.Require(true);
            TB_date_add.Require(true);
            TB_rate.Require(true);
            TB_pricewithouttax.Require(true);

        }

        private bool ErrorCheckChoice()
        {

            if (!String.IsNullOrEmpty(brandC.TxtCode.Text))
            {
                if (!brandC.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    brandC.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(sportC.TxtCode.Text))
            {
                if (!sportC.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    sportC.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(segmentC.TxtCode.Text))
            {
                if (!segmentC.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    segmentC.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(makershohinC.TxtCode.Text))
            {
                if (!makershohinC.IsExists(2))
                {
                    //bbl.ShowMessage("E101");
                    //makershohinC.Focus();
                    //return false;
                }
            }
            return true;
        }

        private bool ErrorCheckUpdate()
        {
            if (!RequireCheck(new Control[] { TB_rate_E }))
                return false;
            return true;
        }

        private bool ErrorCheckCopy()
        {
            if (!RequireCheck(new Control[] { TB_dateE,TB_rate_E })) //Step1
                return false;

           
            //if (!RequireCheck(new Control[] { shiiresaki, TB_headerdate, CB_store })) //Step1
            //    return false;
            return true;
        }
        private void F11()
        {
            if (ErrorCheckMain())
            {
                btn = "1";
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
                if (deldt != null)
                {
                    deldt.Rows.Clear();
                }
                deldt = bl.MastertorokuShiiretanka_Select(m_IOE); ;
                m_IOE.Display = "1";

                if (dtdeljan != null)
                {
                    dtdeljan.Rows.Clear();
                }
                dtdeljan = bl.MastertorokuShiiretanka_Select(m_IOE);
                dtsku = bl.MastertorokuShiiretanka_Select(m_IOE);
                dv = new DataView(dt);
                GV_item.DataSource = dv;
               
                //dtdeljan = dtsku;
            }
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
                InsertOperator =  InOperatorCD,
                ProcessMode=ModeText,
                ProgramID=InProgramID,
                Key=shiiresaki.TxtCode.Text,
                PC=InPcID

            };
            return m_IOE;
        }
        private M_ItemOrderPrice_Entity Getdata()
        {
            m_IOE = new M_ItemOrderPrice_Entity
            {
                VendorCD = shiiresaki.TxtCode.Text,
                StoreCD = CB_store.SelectedValue.ToString(),
                ChangeDate=TB_headerdate.Text,
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
                PriceOutTax=LB_priceouttax.Text
                
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
            //Clear(panel5);
            //Clear(panel3);
            Clear(panel3);
            LB_priceouttax.Text = "";
            RB_zenten.Checked = true;
            RB_item.Checked = true;
            RB_current.Checked = true;
            TB_headerdate.Text = bbl.GetDate();
            CB_store.SelectedValue = "0000";
            shiiresaki.SetFocus(1);
            GV_item.Refresh();
            GV_item.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Clear();
            }
            if (dtsku.Rows.Count > 0)
            {
                dtsku.Rows.Clear();
            }
            
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
            RB_item.Checked = true;
            Clear(panel3);
            LB_priceouttax.Text = "";
            //GV_item.Refresh();
            GV_item.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Clear();
            }
            if (dtsku.Rows.Count > 0)
            {
                dtsku.Rows.Clear();
            }
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
                this.サイズ.Visible = true;
                this.サイズ.Width = 155;
                this.カラー.Visible = true;
                this.カラー.Width = 155;
                this.SKUCD.Visible = true;
                this.SKUCD.Width = 200;
                this.商品名.Width = 352;
                //itemcd.Clear();
                //TB_date_add.Clear();
                //TB_rate.Clear();
                //TB_pricewithouttax.Clear();
                //TB_dateE.Clear();
                //TB_rate_E.Clear();
                Clear(panel4);
                Clear(panel5);
                GV_item.Refresh();
                BT_Capture.Visible = false;
                ModeText = "SKU";
                GV_item.DataSource = dtsku;
            }
            else
            {
               
                ModeText = "ITEM";
                panel4.Enabled = true;
                panel5.Enabled = true;
                this.SKUCD.Width = 0;
                this.ブランド.Visible = true;
                this.シーズン.Visible = true;
                this.年度.Visible = true;
                this.商品分類.Visible = true;
                this.競技.Visible = true;
                this.サイズ.Visible = false;
                this.サイズ.Width = 0;
                this.カラー.Visible = false;
                this.カラー.Width = 0;
                this.SKUCD.Visible = false;
                this.商品名.Width = 320;
                GV_item.Refresh();
                BT_Capture.Visible = true ;
                if (btn == "1" || btn == "2")
                {
                    GV_item.DataSource = dt;
                }
                else
                {
                    GV_item.DataSource = dtview;
                }
            }
        }
        private void RB_itemandsku_Check()
        {
            if (RB_sku.Checked == true)
            {
                panel4.Enabled = false;
                panel5.Enabled = false;
                this.ブランド.Visible = false;
                this.シーズン.Visible = false;
                this.年度.Visible = false;
                this.商品分類.Visible = false;
                this.競技.Visible = false;

                //this.GV_item.Location = new System.Drawing.Point(89, 346);かーら

                //this.GV_item.Size = new System.Drawing.Size(1500, 280);
                this.サイズ.Visible = true;
                this.サイズ.Width = 175;
                this.カラー.Visible = true;
                this.カラー.Width = 130;
                this.SKUCD.Visible = true;
                this.SKUCD.Width = 120;
                GV_item.Refresh();
                BT_Capture.Visible = false;
                ModeText = "SKU";
                //if (!String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
                //{
                GV_item.DataSource = dtsku;
                //}
            }
            else
            {
                //this.GV_item.Size = new System.Drawing.Size(1560, 280);
                ModeText = "ITEM";
                panel4.Enabled = true;
                panel5.Enabled = true;
                this.SKUCD.Width = 150;
                this.ブランド.Visible = true;
                this.シーズン.Visible = true;
                this.年度.Visible = true;
                this.商品分類.Visible = true;
                this.競技.Visible = true;
                this.サイズ.Visible = false;
                this.サイズ.Width = 0;
                this.カラー.Visible = false;
                this.カラー.Width = 0;
                this.SKUCD.Visible = false;
                GV_item.Refresh();
                BT_Capture.Visible = true;
                //if (!String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
                //{
                if (btn == "1" || btn =="2")
                {
                    GV_item.DataSource = dt;
                }
                else
                {
                    GV_item.DataSource = dtview;
                    //GV_item.DataSource = dv;
                }
            }
        } 
        private void TB_rate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //CalPrice();
                if (!String.IsNullOrEmpty(LB_priceouttax.Text))
                {

                    //TB_orderprice.Text = Convert.ToString(listprice * (rate * con));
                    //TB_orderprice.Text= string.Format("{0:#,##0}", Convert.ToInt64((listprice * (rate * con))));
                    if (!String.IsNullOrEmpty(TB_rate.Text))
                    {
                        decimal rate = Convert.ToDecimal(TB_rate.Text);
                        decimal con = (decimal)0.01;
                        decimal listprice = Convert.ToDecimal(LB_priceouttax.Text);
                        TB_pricewithouttax.Text = Math.Round(listprice * (rate * con)).ToString();
                    }
                }
            }

        }
        private void CalPrice()
        {

            //if (!String.IsNullOrEmpty(LB_priceouttax.Text))
            //{

                //TB_orderprice.Text = Convert.ToString(listprice * (rate * con));
                //TB_orderprice.Text= string.Format("{0:#,##0}", Convert.ToInt64((listprice * (rate * con))));
                //if (!String.IsNullOrEmpty(TB_rate.Text))
                //{
                
                //    decimal rate = Convert.ToDecimal(rate);
                //    decimal con = (decimal)0.01;
                //    decimal listprice = Convert.ToDecimal(LB_priceouttax.Text);
                //    TB_pricewithouttax.Text = Math.Round(listprice * (rate * con)).ToString();
                //}
            //}
        }
        private void btn_add_Click(object sender, EventArgs e)
        {
            if (ErrorCheckMain())
            { 
            if (ErrorCheckAdd())
            {
                bool dvadd = false;
                DataTable dataTable = new DataTable();
                string selectq = "";
                string dateq = "";
                //selectq = " VendorCD ='" + shiiresaki.TxtCode.Text + "'";
                //selectq += " and StoreCD ='" + CB_store.SelectedValue.ToString() + "'";
                selectq += "  ItemCD = '" + itemcd.TxtCode.Text + "'";
                dateq += "  ChangeDate = '" + TB_date_add.Text + "'";
                string ItemName = "";
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
                        m_IE = new M_ITEM_Entity();
                        m_IE.ITemCD = itemcd.TxtCode.Text;
                        DataTable dtadd = bl.M_ITEM_SelectBy_ItemCD(m_IE);
                        if (dtadd.Rows.Count > 0)
                        {
                            DataRow row1 = null;
                            for (int i = 0; i < dtadd.Rows.Count; i++)
                            {
                                if (btn == "1" || btn == "2")
                                {
                                    row1 = dt.NewRow();
                                }
                                else
                                {

                                    row1 = dtview.NewRow();
                                }
                                row1["VendorCD"] = shiiresaki.TxtCode.Text;
                                row1["StoreCD"] = CB_store.SelectedValue.ToString();
                                row1["Tempkey"] = "1";
                                row1["CheckBox"] = "0";
                                row1["BrandCD"] = dtadd.Rows[i]["BrandCD"];
                                row1["BrandName"] = dtadd.Rows[i]["BrandName"];
                                row1["SportsCD"] = dtadd.Rows[i]["SportsCD"];
                                row1["Char1"] = dtadd.Rows[i]["Char1"];
                                row1["SegmentCD"] = dtadd.Rows[i]["SegmentCD"];
                                row1["SegmentCDName"] = dtadd.Rows[i]["SegmentCDName"];
                                row1["LastYearTerm"] = dtadd.Rows[i]["LastYearTerm"];
                                row1["LastSeason"] = dtadd.Rows[i]["LastSeason"];
                                row1["MakerItem"] = dtadd.Rows[i]["MakerItem"];
                                row1["ItemCD"] = itemcd.TxtCode.Text;
                                ItemName = dtadd.Rows[i]["ItemName"].ToString();
                                row1["ItemName"] = ItemName;
                                row1["ChangeDate"] = TB_date_add.Text;
                                row1["Rate"] = TB_rate.Text;
                                row1["PriceOutTax"] = LB_priceouttax.Text;
                                row1["PriceWithoutTax"] = TB_pricewithouttax.Text;
                                row1["Tempkey"] = "1";
                                row1["InsertOperator"] = operatorCd;
                                row1["InsertDateTime"] = bbl.GetDate();
                                row1["UpdateOperator"] = operatorCd;
                                row1["UpdateDateTime"] = bbl.GetDate();
                                if (btn == "1" || btn == "2")
                                {
                                    dt.Rows.Add(row1);
                                    dt.AcceptChanges();
                                    GV_item.Refresh();
                                    GV_item.DataSource = dt;
                                }
                                else
                                {
                                    dtview.Rows.Add(row1);
                                    dt.ImportRow(row1);
                                    dt.AcceptChanges();
                                    dtview.AcceptChanges();
                                    dt.AcceptChanges();
                                    dtview.AcceptChanges();
                                    GV_item.Refresh();
                                    GV_item.DataSource = dtview;
                                }
                            }
                            m_IOE = Getdata();
                            m_IOE.Display = "1";
                            DataRow[] drskuadd;
                            drskuadd = dtsku.Select(selectq + " and  ChangeDate = '" + TB_date_add.Text + "'");
                            DataTable dtskuup = bl.M_SKU_SelectFor_SKU_Update("", "", itemcd.TxtCode.Text, TB_date_add.Text, "3");
                            if (drskuadd.Length > 0)
                            {
                                for (int i = 0; i < drskuadd.Length; i++)
                                {
                                    drskuadd[i]["ItemCD"] = itemcd.TxtCode.Text;
                                    drskuadd[i]["ChangeDate"] = TB_date_add.Text;
                                    drskuadd[i]["Rate"] = TB_rate.Text;
                                    drskuadd[i]["PriceOutTax"] = LB_priceouttax.Text;
                                    drskuadd[i]["PriceWithoutTax"] = TB_pricewithouttax.Text;
                                    drskuadd[i]["InsertOperator"] = operatorCd;
                                    drskuadd[i]["InsertDateTime"] = bbl.GetDate();
                                    drskuadd[i]["UpdateOperator"] = operatorCd;
                                    drskuadd[i]["UpdateDateTime"] = bbl.GetDate();
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
                                        rowsku["VendorCD"] = shiiresaki.TxtCode.Text;
                                        rowsku["StoreCD"] = CB_store.SelectedValue.ToString();
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
                                        rowsku["ItemName"] = ItemName;
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
        }
        }
        private void btn_subdisplay_Click(object sender, EventArgs e)
        {
            if (ErrorCheckSubdisplay())
            {

                string SubDisplayquery = "";
                btn = "0";
                bool operand = false;
                if (!String.IsNullOrEmpty(brand.TxtCode.Text))
                {
                    SubDisplayquery = "BrandCD = '" + brand.TxtCode.Text + "'";
                    operand = true;
                }
               
                if (!String.IsNullOrEmpty(segment.TxtCode.Text))
                {
                    if(operand)
                    {
                        SubDisplayquery += " and ";
                    }
                    operand = true;
                    SubDisplayquery += "SegmentCD = '" + segment.TxtCode.Text + "'";
                }
                
                if (!String.IsNullOrEmpty(CB_year.Text))
                {
                    if (operand)
                    {
                        SubDisplayquery += " and ";
                    }
                    operand = true;
                    SubDisplayquery += " LastYearTerm = '" + CB_year.Text + "'";
                }
               
                if (!String.IsNullOrEmpty(CB_season.Text))
                {
                    if (operand)
                    {
                        SubDisplayquery += " and ";
                    }
                    operand = true;
                    SubDisplayquery += " LastSeason = '" + CB_season.Text + "'";
                }
              
                if (!String.IsNullOrEmpty(sport.TxtCode.Text))
                {
                    if (operand)
                    {
                        SubDisplayquery += " and ";
                    }
                    operand = true;
                    SubDisplayquery += "SportsCD = '" + sport.TxtCode.Text + "'";
                }
               
                if (!String.IsNullOrEmpty(makershohin.TxtCode.Text))
                {
                    if (operand)
                    {
                        SubDisplayquery += " and ";
                    }
                    operand = true;
                    SubDisplayquery += "MakerItem = '" + makershohin.TxtCode.Text + "'";
                }
                
                if(!String.IsNullOrEmpty(TB_date_condition.Text))
                {
                    if (operand)
                    {
                        SubDisplayquery += " and ";
                    }
                    operand = true;
                    if (RB_current.Checked)
                    {
                        
                        SubDisplayquery += " ChangeDate <= '" + TB_date_condition.Text + "'";
                    }
                    else
                    {
                        SubDisplayquery += "ChangeDate = '" + TB_date_condition.Text + "'";
                    }
                }
                
                //query += " and SegmentCD = '" + segment.TxtCode.Text + "'";
                //query += " and LastYearTerm = '" + CB_year.Text + "'";
                //query += " and LastSeason = '" + CB_season.Text + "'";
                //query += " and MakerItem = '" + makershohin.TxtCode.Text + "'";
                //query += " and ChangeDate = '" + TB_date_condition.Text + "'";


                if (GV_item.DataSource != null)
                {
                    dv.RowFilter = SubDisplayquery;
                    dtview = dv.ToTable();
                    GV_item.DataSource = dv;
                }
            }
        }
        private void btn_choice_Click(object sender, EventArgs e)
        {
            if (ErrorCheckChoice())
            {
                string dateq = "";
                choiceq = "";
                bool opand = false;
                if (!String.IsNullOrEmpty(brandC.TxtCode.Text))
                {
                    choiceq = "BrandCD = '" + brandC.TxtCode.Text + "'";
                    opand = true;
                }
                if (!String.IsNullOrEmpty(segmentC.TxtCode.Text))
                {
                    if(opand)
                    {
                        choiceq += " and ";
                    }
                    opand = true;
                    choiceq += " SegmentCD = '" + segmentC.TxtCode.Text + "'";
                }
                if (!String.IsNullOrEmpty(CB_yearC.Text))
                {
                    if (opand)
                    {
                        choiceq += " and ";
                    }
                    opand = true;
                    choiceq += " LastYearTerm = '" + CB_yearC.Text + "'";
                }
                if (!String.IsNullOrEmpty(cb_seasonC.Text))
                {
                    if (opand)
                    {
                        choiceq += " and ";
                    }
                    opand = true;
                    choiceq += " LastSeason = '" + cb_seasonC.Text + "'";
                }
                if (!String.IsNullOrEmpty(sportC.TxtCode.Text))
                {
                    if (opand)
                    {
                        choiceq += " and ";
                    }
                    opand = true;
                    choiceq += " SportsCD = '" + sportC.TxtCode.Text + "'";
                }
                if (!String.IsNullOrEmpty(makershohinC.TxtCode.Text))
                {
                    if (opand)
                    {
                        choiceq += " and ";
                    }
                    opand = true;
                    choiceq += " MakerItem = '" + makershohinC.TxtCode.Text + "'";
                }
                if (!String.IsNullOrEmpty(TB_dateC.Text))
                {
                    if (opand)
                    {
                        choiceq += " and ";
                    }
                    opand = true;
                    dateq += " ChangeDate = '" + TB_dateC.Text + "'";
                }
                if (GV_item.Rows.Count > 0)
                {
                    DataRow[] dr=null;

                    if (RB_item.Checked)
                    {
                        if (btn == "1"  || btn =="2")
                        {
                            dr = dt.Select(choiceq + dateq);
                        }
                        else
                        {
                            dr = dtview.Select(choiceq + dateq);
                            //dr = dv.FindRows(choiceq + dateq);
                        }
                        //dr = dtsku.Select(choiceq + dateq);

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
                        CheckFun();
                        if (btn == "1" || btn == "2")
                        {
                            dt.AcceptChanges();
                        }
                        else
                        {
                            GV_item.DataSource = dtview;
                        }
                        brandC.Clear();
                        sportC.Clear();
                        segmentC.Clear();
                        GV_item.Refresh();
                        CB_yearC.Text = string.Empty;
                        cb_seasonC.Text = string.Empty;
                        TB_dateC.Text = string.Empty;
                        makershohinC.Clear();
                    }
                }
            }
        }
        private void btn_selectall_Click(object sender, EventArgs e)
        {
            if (GV_item.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataGridViewRow row in GV_item.Rows)
                {
                    
                    row.Cells["ck"].Value = "1";
                    if(btn == "0")  // for subdisplay
                    {
                        dtview.Rows[count]["CheckBox"] = "1";
                        count++;
                    }
                }
                CheckFun();
            }
            //    String itemdata = bl.DataTableToXml(dt);
            //    String skudata = bl.DataTableToXml(dtsku);
            //    DataTable dtdata = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata, "", TB_headerdate.Text, "2");

            //    if (dtdata.Rows.Count > 0)
            //    {
            //        for (int j = 0; j < dtdata.Rows.Count; j++)
            //        {
            //            string itemcd = dtdata.Rows[j]["ItemCD"].ToString();
            //            string qskuupdate = " ItemCD = '" + itemcd + "'";
            //            qskuupdate += " and ChangeDate <= '" + TB_headerdate.Text + "'";
            //            DataRow[] drte = dtsku.Select(qskuupdate);

            //            if (drte.Length > 0)
            //            {
                        
            //                for (int i = 0; i < drte.Length; i++)
            //                {
            //                    drte[i]["CheckBox"] = "1";
            //                }
            //            }
            //        }
            //}
        //}
        }
        private void btn_releaseall_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in GV_item.Rows)
            {
                //String itemdata = bl.DataTableToXml(dt);
                //String skudata = bl.DataTableToXml(dtsku);
                //DataTable dtdata = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata,"", TB_headerdate.Text, "2");


                //if (dtdata.Rows.Count > 0)
                //{
                //string itemcd = dtdata.Rows[0]["ItemCD"].ToString();
                //string qskuupdate = " ItemCD = '" + itemcd + "'";
                //qskuupdate += " and ChangeDate <= '" + TB_headerdate.Text + "'";
                DataRow[] drte = dtsku.Select(" CheckBox =1");
                if (drte.Length > 0)
                {
                    for (int i = 0; i < drte.Length; i++)
                    {
                        drte[i]["CheckBox"] = "0";
                    }
                }
                //}
                row.Cells["ck"].Value = "0";
              
            }
        }
        private void btn_Copy_Click(object sender, EventArgs e)
        {

            if (ErrorCheckMain())
            {
                if (GV_item.Rows.Count > 0)
            {

                //if (!String.IsNullOrEmpty(TB_dateE.Text)  && !String.IsNullOrEmpty(TB_rate_E.Text))
                if (ErrorCheckCopy())
                    {
                        string date = "";
                        date = "  ChangeDate = '" + TB_dateE.Text + "'";
                        date += " and CheckBox = 1";
                        string itemCd = "";
                        
                        DataRow[] dr;
                        if (btn == "1" || btn == "2")
                        {
                            dr = dt.Select(date);
                        }
                        else
                        {
                            dr = dtview.Select(date);
                        }

                        if (dr.Length == 0)
                        {
                            string q = "CheckBox =1";
                            DataRow[] dr1;
                            if (btn == "1" || btn == "2")
                            {
                                dr1 = dt.Select(q);
                            }
                            else
                            {
                                dr1 = dtview.Select(q);
                            }

                            if (dr1.Length > 0)
                            {
                                DataTable dt1 = dr1.CopyToDataTable();
                                if (dt1.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        itemCd = dt1.Rows[i]["ItemCD"].ToString();
                                        dt1.Rows[i]["ChangeDate"] = TB_dateE.Text;
                                        dt1.Rows[i]["Rate"] = TB_rate_E.Text;
                                        decimal rate = Convert.ToDecimal(TB_rate_E.Text);
                                        decimal con = (decimal)0.01;
                                        decimal listprice = Convert.ToDecimal(dt1.Rows[i]["PriceOutTax"]);
                                        dt1.Rows[i]["PriceWithoutTax"] = Math.Round(listprice * (rate * con)).ToString();
                                    }
                                    DataRow[] drskuscopy;
                                    string copyq = " ItemCD = '" + itemCd + " 'and ChangeDate = '" + TB_dateE.Text + "'";
                                    drskuscopy = dtsku.Select(copyq);
                                    String datat;
                                    if (btn == "1" || btn == "2")
                                    {
                                        datat = bl.DataTableToXml(dt);
                                    }
                                    else
                                    {
                                        datat = bl.DataTableToXml(dtview);
                                    }
                                    if (drskuscopy.Length > 0)
                                    {
                                        
                                        for (int i = 0; i < drskuscopy.Length; i++)
                                            {
                                            drskuscopy[i]["Rate"] = TB_rate_E.Text;
                                            drskuscopy[i]["PriceOutTax"] = drskuscopy[i]["PriceOutTax"];
                                            drskuscopy[i]["InsertOperator"] = operatorCd;
                                            drskuscopy[i]["InsertDateTime"] = bbl.GetDate();
                                            drskuscopy[i]["UpdateOperator"] = operatorCd;
                                            drskuscopy[i]["UpdateDateTime"] = bbl.GetDate();
                                            decimal rate = Convert.ToDecimal(TB_rate_E.Text);
                                            decimal con = (decimal)0.01;
                                            decimal listprice = Convert.ToDecimal(drskuscopy[i]["PriceOutTax"]);
                                            drskuscopy[i]["PriceWithoutTax"] = Math.Round(listprice * (rate * con)).ToString();
                                        }
                                    }
                                    else
                                    {
                                        DataTable dtr = bl.M_SKU_SelectFor_SKU_Update(datat, "", "", "", "1");
                                        if (dtr.Rows.Count > 0)
                                        {
                                            DataRow rowsku;
                                            rowsku = dtsku.NewRow();
                                            rowsku["Tempkey"] = "1";
                                            rowsku["CheckBox"] = "0";
                                            rowsku["VendorCD"] = shiiresaki.TxtCode.Text;
                                            rowsku["StoreCD"] = CB_store.SelectedValue.ToString();
                                            rowsku["AdminNO"] = dtr.Rows[0]["AdminNO"];
                                            rowsku["SKUCD"] = dtr.Rows[0]["SKUCD"];
                                            rowsku["SizeName"] = dtr.Rows[0]["SizeName"];
                                            rowsku["ColorName"] = dtr.Rows[0]["ColorName"];
                                            rowsku["LastYearTerm"] = dtr.Rows[0]["LastYearTerm"];
                                            rowsku["LastSeason"] = dtr.Rows[0]["LastSeason"];
                                            rowsku["MakerItem"] = dtr.Rows[0]["MakerItem"];
                                            rowsku["ItemCD"] = dtr.Rows[0]["ItemCD"];
                                            rowsku["ItemName"] = dtr.Rows[0]["ItemName"];
                                            rowsku["ChangeDate"] = TB_dateE.Text;
                                            rowsku["Rate"] = TB_rate_E.Text;
                                            decimal rate = Convert.ToDecimal(TB_rate_E.Text);
                                            decimal con = (decimal)0.01;
                                            decimal listprice = Convert.ToDecimal(dtr.Rows[0]["PriceOutTax"]);
                                            rowsku["PriceOutTax"] = dtr.Rows[0]["PriceOutTax"];
                                            rowsku["PriceWithoutTax"] = Math.Round(listprice * (rate * con)).ToString();
                                            
                                            rowsku["InsertOperator"] = operatorCd;
                                            rowsku["InsertDateTime"] = bbl.GetDate();
                                            rowsku["UpdateOperator"] = operatorCd;
                                            rowsku["UpdateDateTime"] = bbl.GetDate();
                                            dtsku.Rows.Add(rowsku);
                                            dtsku.AcceptChanges();
                                           
                                        }
                                    }
                                    if (btn == "1" || btn == "2")
                                    {

                                        dt.Merge(dt1);
                                    }
                                    else
                                    {
                                        dtview.Merge(dt1);
                                        GV_item.Refresh();
                                        GV_item.DataSource = dtview;
                                        dt.Merge(dt1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            bbl.ShowMessage("E224");
                            ///.Focus();
                        }
                    }
                    else
                    {
                        //bbl.ShowMessage("");
                        TB_dateE.Focus();
                    }
                }
            }
        }
        private void btn_update_Click(object sender, EventArgs e)
        {
            if (ErrorCheckMain())
            {
                if (GV_item.Rows.Count > 0)
                {

                    if (ErrorCheckUpdate())
                    {
                        string updateq = "CheckBox = 1";
                        DataRow[] drupdate;
                        if (btn == "1" || btn == "2")
                        {
                            drupdate = dt.Select(updateq);
                        }
                        else
                        {
                            drupdate = dtview.Select(updateq);

                        }
                        //GV_item.DataSource = dtview;
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
                            String itemdata;

                            if (btn == "1" || btn == "2")
                            {
                                itemdata = bl.DataTableToXml(dt);
                            }
                            else
                            {
                                GV_item.DataSource = dtview;
                                itemdata = bl.DataTableToXml(dtview);
                            }

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
                                        drte[i]["Rate"] = TB_rate_E.Text;
                                        decimal rate = Convert.ToDecimal(dtdata.Rows[0]["Rate"]);
                                        decimal con = (decimal)0.01;
                                        //string priceouttax = drskuscopy[i]["PriceOutTax"].ToString();
                                        decimal listprice = Convert.ToDecimal(drte[0]["PriceOutTax"]);
                                        drte[i]["PriceWithoutTax"] = Math.Round(listprice * (rate * con)).ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (GV_item.Rows.Count > 0)
            {
            DataRow[] rows;
                if (btn == "1" || btn == "2")
                {
                    rows = dt.Select(" CheckBox =1");
                }
                else
                {
                    rows = dtview.Select(" CheckBox =1");
                }
                
                int rowcount = 0;

                foreach (DataRow row in rows)
                {
                    //dtview.Rows.Remove(row);
                    String itemdata;
                    if (btn == "1" || btn == "2")
                    {
                        itemdata = bl.DataTableToXml(dt);
                    }
                    else
                    {
                        itemdata = bl.DataTableToXml(dtview);
                    }

                String skudata = bl.DataTableToXml(dtsku);
                DataTable dtskudel = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata, "", TB_headerdate.Text, "2");
                if (dtskudel.Rows.Count > 0)
                {
                    string itemcd = dtskudel.Rows[0]["ItemCD"].ToString();
                    string qskuupdate = " ItemCD = '" + itemcd + "'";
                    qskuupdate += " and ChangeDate <= '" + TB_headerdate.Text + "'";
                    DataRow[] drte = dtsku.Select(qskuupdate);
                    if (drte.Length > 0)
                    {
                        foreach (DataRow rowd in drte)
                            dtsku.Rows.Remove(rowd);
                    }
                }
                    
                   //dt.Rows.Remove(row);
                   //if(btn == "0")
                   // {
                   //     dtview.Rows.Remove(row);
                   //     //dt.Rows.Remove(row);
                   // }
                    if (btn == "1" || btn == "2")
                    {
                        dt.Rows.Remove(row);
                    }
                    else
                    {
                        dtview.Rows.Remove(row);
                    }
                }
                if (btn == "0")
                {

                    DataRow[] rowsdt = dt.Select(" CheckBox =1");
                    foreach (DataRow dr in rowsdt)
                    {
                        dt.Rows.Remove(dr);
                    }
                }
            }
        }
        private void F12()
        {
            if (ErrorCheckMain())
            {
                if (dt.Rows.Count > 0)
                {

                    m_IOE = GetItemorder();
                    String deletedata = bl.DataTableToXml(deldt);
                    String tbdeljan = bl.DataTableToXml(dtdeljan);
                    String itemdata = bl.DataTableToXml(dt);
                    String skudata = bl.DataTableToXml(dtsku);
                    //DataTable dst=bl.Mastertoroku_Shiretanka_Insert(itemdata, skudata, deletedata, tbdeljan, m_IOE);
                    if (bl.Mastertoroku_Shiretanka_Insert(itemdata, skudata, deletedata, tbdeljan, m_IOE))
                    {
                        bl.ShowMessage("I101");
                        Clear();
                    }
                    //string storecd=CB_store.SelectedValue.ToString()
                    //if(btn == "2")
                    //{
                    //DataTable dtr = bl.Mastertoroku_Shiretanka_Insert(itemdata, skudata,m_IOE);
                    //}
                    //else
                    //{
                    //    DataTable dtr = bl.Mastertoroku_Shiretanka_Insert(itemdata, skudata, m_IOE);
                    //}
                    //}
                }
                else
                {
                    bbl.ShowMessage("E128");
                }
            }
        }
        private void GV_item_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 0)
                {
                    int count = 0;
                    //string ck = GV_item.Rows[e.RowIndex].Cells["ck"].State.ToString();
                    string ck1 = GV_item.Rows[e.RowIndex].Cells["ck"].Value.ToString();
                    if (ck1 == "0")
                    {

                        GV_item.Rows[e.RowIndex].Cells["ck"].Value = "1";
                        if (btn == "0")  // for subdisplay
                        {
                            dtview.Rows[count]["CheckBox"] = "1";
                            count++;
                        }
                        CheckFun();
                    }
                    else
                    {
                        String itemdata;

                        if (btn == "1" || btn =="2")
                        {
                            itemdata = bl.DataTableToXml(dt);
                        }
                        else
                        {
                            itemdata = bl.DataTableToXml(dtview);
                        }
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
                                    drte[i]["CheckBox"] = "0";
                                }
                            }
                        }
                    }
                }
            }
        }
        private void CheckFun()
        {
            String itemdata;
          
            if (btn == "1" || btn =="2")
            {
                itemdata = bl.DataTableToXml(dt);
            }
            else
            {
                itemdata = bl.DataTableToXml(dtview);
            }
            String skudata = bl.DataTableToXml(dtsku);
            DataTable dtdata = bl.M_SKU_SelectFor_SKU_Update(itemdata, skudata, "", TB_headerdate.Text, "2");

            if (dtdata.Rows.Count > 0)
            {
                for (int j = 0; j < dtdata.Rows.Count; j++)
                {
                    string itemcd = dtdata.Rows[j]["ItemCD"].ToString();
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
        private void BT_Capture_Click(object sender, EventArgs e)
        {
            //this.定価.DefaultCellStyle.Format = "N0";
            btn = "2";
            //if (!String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
            //{
                //vendorcd = shiiresaki.TxtCode.Text;
                //storecd = CB_store.SelectedValue.ToString();
                if (ErrorCheckExcel())
                {
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows.Clear();
                    }
                    if (dtsku.Rows.Count > 0)
                    {
                        dtsku.Rows.Clear();
                    }
                    if (deldt != null)
                    {
                        deldt.Rows.Clear();
                    }
                    if (dtdeljan != null)
                    {
                        dtdeljan.Rows.Clear();
                    }
                    GV_item.DataSource = null;
                    //dtExcel.Rows[0].Delete();
                    //DataTable dtE = new DataTable();
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add("VendorCD");
                        dt.Columns.Add("StoreCD");
                        dt.Columns.Add("Tempkey");
                        dt.Columns.Add("CheckBox");
                        dt.Columns.Add("ItemCD");
                        dt.Columns.Add("ItemName");
                        dt.Columns.Add("MakerItem");
                        dt.Columns.Add("SportsCD");
                        dt.Columns.Add("SegmentCD");
                        dt.Columns.Add("SegmentCDName");
                        dt.Columns.Add("Char1");
                        dt.Columns.Add("BrandCD");
                        dt.Columns.Add("BrandName");
                        dt.Columns.Add("LastYearTerm");
                        dt.Columns.Add("LastSeason");
                        dt.Columns.Add("ChangeDate");
                        dt.Columns.Add("Rate");
                        dt.Columns.Add("PriceOutTax");
                        dt.Columns.Add("PriceWithoutTax");
                        dt.Columns.Add("InsertOperator");
                        dt.Columns.Add("InsertDateTime");
                        dt.Columns.Add("UpdateOperator");
                        dt.Columns.Add("UpdateDateTime");
                    }
                if (dtExcel != null)
                {
                    for (int i = 0; i < dtExcel.Rows.Count; i++)
                    {
                        //DataRow row = dtExcel.Rows[i];

                        //if (row[0] != DBNull.Value)
                        //{
                        m_IE = new M_ITEM_Entity();
                        m_IE.ITemCD = dtExcel.Rows[i][2].ToString();
                        DataTable dtadd = bl.M_ITEM_SelectBy_ItemCD(m_IE);
                        string dateExcel = "";
                        if (!String.IsNullOrEmpty(dtExcel.Rows[i][2].ToString()))
                        {
                            if (dtadd.Rows.Count > 0)
                            {
                                DataRow row1;
                                row1 = dt.NewRow();
                                string dates = dtExcel.Rows[i][3].ToString();
                                row1["VendorCD"] = shiiresaki.TxtCode.Text;
                                row1["StoreCD"] = CB_store.SelectedValue.ToString();
                                row1["ItemCD"] = dtExcel.Rows[i][2].ToString();
                                row1["CheckBox"] = "0";
                                row1["Tempkey"] = "1";
                                row1["ItemName"] = dtadd.Rows[0]["ItemName"];
                                row1["BrandCD"] = dtadd.Rows[0]["BrandCD"];
                                row1["BrandName"] = dtadd.Rows[0]["BrandName"];
                                row1["SportsCD"] = dtadd.Rows[0]["SportsCD"];
                                row1["Char1"] = dtadd.Rows[0]["Char1"];
                                row1["SegmentCD"] = dtadd.Rows[0]["SegmentCD"];
                                row1["SegmentCDName"] = dtadd.Rows[0]["SegmentCDName"];
                                row1["LastYearTerm"] = dtadd.Rows[0]["LastYearTerm"];
                                row1["LastSeason"] = dtadd.Rows[0]["LastSeason"];
                                row1["MakerItem"] = dtadd.Rows[0]["MakerItem"];
                                if (String.IsNullOrEmpty(dtExcel.Rows[i][4].ToString()))
                                {
                                    row1["Rate"] = "0";
                                }
                                else
                                {
                                    row1["Rate"] = dtExcel.Rows[i][4].ToString();
                                }
                                DateTime datee = Convert.ToDateTime(dtExcel.Rows[i][3].ToString());
                                row1["ChangeDate"] = datee.ToString("yyyy/MM/dd");
                                row1["PriceOutTax"] = dtadd.Rows[0]["PriceOutTax"].ToString();
                                row1["PriceWithoutTax"] = dtExcel.Rows[i][5].ToString();
                                row1["InsertOperator"] = operatorCd;
                                row1["InsertDateTime"] = bbl.GetDate();
                                row1["UpdateOperator"] = operatorCd;
                                row1["UpdateDateTime"] = bbl.GetDate();
                                dt.Rows.Add(row1);
                                GV_item.DataSource = dt;
                                deldt = dt;

                                dv = new DataView(dt);

                                DataTable dtskuExcel = bl.M_SKU_SelectFor_SKU_Update("", "", dtExcel.Rows[i][2].ToString(), dtExcel.Rows[i][3].ToString(), "3");
                                if (dtskuExcel.Rows.Count > 0)
                                {
                                    if (dtsku.Columns.Count == 0)
                                    {
                                        dtsku.Columns.Add("VendorCD");
                                        dtsku.Columns.Add("StoreCD");
                                        dtsku.Columns.Add("Tempkey");
                                        dtsku.Columns.Add("CheckBox");
                                        dtsku.Columns.Add("AdminNO");
                                        dtsku.Columns.Add("ItemCD");
                                        dtsku.Columns.Add("ITemName");
                                        dtsku.Columns.Add("MakerItem");
                                        dtsku.Columns.Add("SportsCD");
                                        dtsku.Columns.Add("SegmentCD");
                                        dtsku.Columns.Add("SizeName");
                                        dtsku.Columns.Add("ColorName");
                                        dtsku.Columns.Add("BrandCD");
                                        dtsku.Columns.Add("SKUCD");
                                        dtsku.Columns.Add("LastYearTerm");
                                        dtsku.Columns.Add("LastSeason");
                                        dtsku.Columns.Add("ChangeDate");
                                        dtsku.Columns.Add("Rate");
                                        dtsku.Columns.Add("PriceOutTax");
                                        dtsku.Columns.Add("PriceWithoutTax");
                                        dtsku.Columns.Add("InsertOperator");
                                        dtsku.Columns.Add("InsertDateTime");
                                        dtsku.Columns.Add("UpdateOperator");
                                        dtsku.Columns.Add("UpdateDateTime");

                                    }
                                    if (dtskuExcel.Rows.Count > 0)
                                    {
                                        DataRow rowsku1;
                                        rowsku1 = dtsku.NewRow();
                                        string datessku = dtExcel.Rows[i][3].ToString();
                                        rowsku1["VendorCD"] = shiiresaki.TxtCode.Text;
                                        rowsku1["StoreCD"] = CB_store.SelectedValue.ToString();
                                        rowsku1["Tempkey"] = "1";
                                        rowsku1["ITemCD"] = dtExcel.Rows[i][2].ToString();
                                        rowsku1["CheckBox"] = "0";
                                        rowsku1["ITemName"] = dtadd.Rows[0]["ItemName"];
                                        rowsku1["AdminNO"] = dtskuExcel.Rows[0]["AdminNO"];
                                        rowsku1["SKUCD"] = dtskuExcel.Rows[0]["SKUCD"];
                                        rowsku1["BrandCD"] = dtskuExcel.Rows[0]["BrandCD"];
                                        rowsku1["SportsCD"] = dtskuExcel.Rows[0]["SportsCD"];
                                        rowsku1["SegmentCD"] = dtskuExcel.Rows[0]["SegmentCD"];
                                        rowsku1["ColorName"] = dtskuExcel.Rows[0]["ColorName"];
                                        rowsku1["SizeName"] = dtskuExcel.Rows[0]["SizeName"];
                                        rowsku1["LastYearTerm"] = dtskuExcel.Rows[0]["LastYearTerm"];
                                        rowsku1["LastSeason"] = dtskuExcel.Rows[0]["LastSeason"];
                                        rowsku1["MakerItem"] = dtskuExcel.Rows[0]["MakerItem"];
                                        if (String.IsNullOrEmpty(dtExcel.Rows[i][4].ToString()))
                                        {
                                            rowsku1["Rate"] = "0";
                                        }
                                        else
                                        {
                                            rowsku1["Rate"] = dtExcel.Rows[i][4].ToString();
                                        }
                                        DateTime datesku = Convert.ToDateTime(dtExcel.Rows[i][3].ToString());
                                        rowsku1["ChangeDate"] = datesku.ToString("yyyy/MM/dd");
                                        rowsku1["PriceOutTax"] = dtskuExcel.Rows[0]["PriceOutTax"];
                                        rowsku1["PriceWithoutTax"] = dtExcel.Rows[i][5].ToString();
                                        rowsku1["InsertOperator"] = operatorCd;
                                        rowsku1["InsertDateTime"] = bbl.GetDate();
                                        rowsku1["UpdateOperator"] = operatorCd;
                                        rowsku1["UpdateDateTime"] = bbl.GetDate();
                                        dtsku.Rows.Add(rowsku1);
                                        dtdeljan = dtsku;
                                    }
                                }

                            }
                        }
                        //}
                    }
                    //Clear(panel3);
                    //RB_zenten.Checked = true;
                    //RB_item.Checked = true;
                    //RB_current.Checked = true;
                    //TB_headerdate.Text = bbl.GetDate();
                    //CB_store.SelectedValue = "0000";
                }

            }
            //}
            //else
            //{
            //    bbl.ShowMessage("E102");
            //}
        }
        private bool ErrorCheckExcel()
        {
            if (!RequireCheck(new Control[] { shiiresaki.TxtCode })) //Step1
                return false;
            

            if (!shiiresaki.IsExists(2))
            {
                bbl.ShowMessage("E101");
                shiiresaki.Focus();
                return false;

            }
            string filePath = string.Empty;
            string fileExt = string.Empty;
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    dtExcel = new DataTable();
                    dtExcel = ReadExcel(filePath, fileExt);

                    string todaydate = bl.GetDate();

                    //int index = dtExcel.Columns["店舗CD"].Ordinal;
                    if (dtExcel.Rows.Count > 0)
                    {
                        string vc = dtExcel.Columns[0].ToString();

                        if (dtExcel.Columns[0].ToString() != "仕入先CD")
                        {
                            bl.ShowMessage("E137");
                            return false;
                        }

                        //if (dtExcel.Columns["店舗CD"].Ordinal != 1)
                        //{
                        //    bl.ShowMessage("E137");
                        //    return false;
                        //}
                        if (dtExcel.Columns[1].ToString() != "店舗CD")
                        {
                            bl.ShowMessage("E137");
                            return false;
                        }
                        if (dtExcel.Columns[2].ToString() != "ITEM")
                        {
                            bl.ShowMessage("E137");
                            return false;
                        }
                        if (dtExcel.Columns[3].ToString() != "改定日")
                        {
                            bl.ShowMessage("E137");
                            return false;
                        }
                        if (dtExcel.Columns[4].ToString() != "掛率")
                        {
                            bl.ShowMessage("E137");
                            return false;
                        }
                        if (dtExcel.Columns[5].ToString() != "発注単価")
                        {
                            bl.ShowMessage("E137");
                            return false;
                        }
                        dtExcel.AcceptChanges();
                        bool errRow1 = false;
                        for (int i = 0; i < dtExcel.Rows.Count; i++)
                        {

                            errRow1 = false;
                            //dtExcel.AcceptChanges();
                            DataRow row = dtExcel.Rows[i];

                            if (row[0] != DBNull.Value)
                            {
                                //break;
                            //    continue;
                            //}

                            if (dtExcel.Rows[i][0].ToString() != shiiresaki.TxtCode.Text)
                            {
                                bl.ShowMessage("E230");
                                errRow1 = true;
                                dtExcel.Rows[i].Delete();

                                continue;
                                //goto nextloop;
                            }
                            string storecd = dtExcel.Rows[i][1].ToString();
                            if (!String.IsNullOrEmpty(storecd) && storecd != "0000")
                            {
                                DataTable dtResult = bl.Select_SearchName(todaydate.Replace("/", "-"), 3, storecd);
                                if (dtResult.Rows.Count == 0)
                                {
                                    bl.ShowMessage("E138");
                                    errRow1 = true;
                                    dtExcel.Rows[i].Delete();
                                    continue;

                                }
                                else if (!base.CheckAvailableStores(storecd))
                                {
                                    bbl.ShowMessage("E141");
                                    errRow1 = true;
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                            }
                            if (!String.IsNullOrEmpty(dtExcel.Rows[i][2].ToString()))
                            {
                                DataTable dtResult = bbl.Select_SearchName(todaydate.Replace("/", "-"), 15, dtExcel.Rows[i][2].ToString());
                                if (dtResult.Rows.Count == 0)
                                {
                                    bl.ShowMessage("E138");
                                    errRow1 = true;
                                    dtExcel.Rows[i].Delete();
                                    continue;
                                }
                            }
                            if (String.IsNullOrEmpty(dtExcel.Rows[i][3].ToString()))
                            {

                                bl.ShowMessage("E103");
                                errRow1 = true;
                                dtExcel.Rows[i].Delete();
                                continue;
                            }
                            string dates = dtExcel.Rows[i][3].ToString();
                            DateTime dt;
                            string[] formats = { "yyyy/MM/dd hh:mm:ss tt" };
                            if (!DateTime.TryParseExact(dates, formats,
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out dt))
                            {
                                bl.ShowMessage("E103");
                                errRow1 = true;
                                dtExcel.Rows[i].Delete();
                                continue;
                            }
                            //string type = dates.GetType.ToString();

                            //var dateTime1 = DateTime.FromOADate(date).ToString("yyyy/MM/dd");

                            //double date = double.Parse(dates);
                            //var dateTime = DateTime.FromOADate(date).ToString("yyyy/MM/dd");
                        }
                        else
                        {
                                dtExcel.Rows[i].Delete();
                        }

                            
                    }
                        //DataTable dtnewex = dtExcel;
                        dtExcel.AcceptChanges();
                        //DataTable dtnewex = dtExcel;
                    }
                    else
                    {
                        bl.ShowMessage("E137");
                        return false;
                    }
                }
                else
                {
                    bl.ShowMessage("E137");
                    return false;
                }
               
            }
            else
            {
                return false;
            }
            return true;
        }
        private void GV_item_KeyPressOne(object sender, KeyPressEventArgs e)
        {
            int i = GV_item.CurrentCell.ColumnIndex;
            int j = GV_item.Columns["掛率"].Index;
            //eval = true;
            if (ActiveControl is DataGridViewTextBoxEditingControl dge)
            {
                if (GV_item.CurrentCell.ColumnIndex == GV_item.Columns["掛率"].Index)
                {
                    string val = GV_item.Rows[GV_item.CurrentCell.RowIndex].Cells["掛率"].Value.ToString();
                    eval = true;
                }
                if (GV_item.CurrentCell.ColumnIndex == GV_item.Columns["発注単価"].Index)
                {
                    //string val = GV_item.Rows[GV_item.CurrentCell.RowIndex].Cells["掛率"].Value.ToString();
                    //eval = true;
                }
            }
        }
        private  bool eval = false;
        private void GV_item_Press_(object sender, PreviewKeyDownEventArgs e)
        {

            if (ActiveControl is DataGridViewTextBoxEditingControl dge)
            {
                string val = GV_item.Rows[GV_item.CurrentCell.RowIndex].Cells["掛率"].Value.ToString();

                if (e.KeyCode == Keys.Enter)
                {
                    eval = true;
                }
                else
                    eval = false;
            }
        }
        private void GV_item_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if ((sender as CKM_Controls.CKM_GridView).CurrentCell.OwningColumn.Name == "掛率")
            {
                //e.Control.KeyPress += new KeyPressEventHandler(GV_item_KeyPressOne);
                //e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(GV_item_Press_);
                e.Control.KeyPress += new KeyPressEventHandler(GV_item_KeyPressOne);
                e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(GV_item_Press_);
            }
        }
        private void GV_item_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //if (1==2)
            //if (e.RowIndex != -1)
            //{
            //    if (e.ColumnIndex == GV_item.Columns["改定日"].Index)
            //    {
            //        //if (e.KeyCode == Keys.Enter)
            //        //{
            //        //}
            //    }
            //    if (e.ColumnIndex == GV_item.Columns["掛率"].Index)
            //    {
            //        if (eval)
            //        {
            //            string ratevlue= GV_item.Rows[e.RowIndex].Cells["掛率"].Value.ToString();
            //            if (ratevlue.Contains("."))
            //            {
                           
            //            }
            //            else
            //            {
            //                if (ratevlue.Length > 3)
            //                {
            //                    MessageBox.Show("enter valid no");
            //                    ///GV_item.Rows[e.RowIndex].Cells["掛率"].Selected = true;
            //                    //GV_item.CurrentCell = GV_item.Rows[e.RowIndex].Cells[e.ColumnIndex];



            //                    //this.掛率.DefaultCellStyle.Format = "N0";
            //                    GV_item.CurrentCell = GV_item.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //                    GV_item.RefreshEdit();
            //                    GV_item.CurrentCell.Value = "0";
            //                    return;

            //                }
            //                //else
            //                //{
            //                //    this.掛率.DefaultCellStyle.Format = "N2";
            //                //}
            //            }
            //            string priceouttax = GV_item.Rows[e.RowIndex].Cells["定価"].Value.ToString();
            //            string rateproce = GV_item.Rows[e.RowIndex].Cells["掛率"].Value.ToString();
            //            decimal rate = Convert.ToDecimal(rateproce);
            //            decimal con = (decimal)0.01;
            //            decimal listprice = Convert.ToDecimal(priceouttax);
            //            GV_item.Rows[e.RowIndex].Cells["発注単価"].Value = Math.Round(listprice * (rate * con)).ToString();
            //        }
            //        eval = false;
            //    }
            //}


        }
        private void GV_item_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == GV_item.Columns["掛率"].Index)
            {
                //eval = false;
            }
        }

        private void GV_item_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //try
            //{
               
                //if (Convert.ToInt32(GV_item.CurrentCell.EditedFormattedValue) < 256 && Convert.ToInt32(GV_item.CurrentCell.EditedFormattedValue) > 0)
                //{}
               
            //}
            //catch(Exception ex)
            //{
               
            //    MessageBox.Show("Enter valid no");
            //    GV_item.RefreshEdit();
            //}
          
           
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            //Provider = Microsoft.ACE.OLEDB.12.0; Data Source = c:\myFolder\myExcel2007file.xlsx;
            //Extended Properties = "Excel 12.0 Xml;HDR=YES;IMEX=1";
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=No;IMEX=1';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    //sheet.Cells.NumberFormat = "@";
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dtexcel;
        }

    }
}




