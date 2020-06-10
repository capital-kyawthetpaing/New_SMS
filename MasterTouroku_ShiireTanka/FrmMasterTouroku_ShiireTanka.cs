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
        DataTable dtc;
        string choiceq = "";
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

            }
        }
       
        private void EnabledPanelContents(Panel panel, bool enabled)
        {
            foreach (Control item in panel.Controls)
            {
                item.Enabled = enabled;
            }
        }
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

        private void segment_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
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

        private void segment_Enter(object sender, EventArgs e)
        {
            segment.Value1 = "203";
        }

        private void brand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
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

        private void sportC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
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

        private void sportC_Enter(object sender, EventArgs e)
        {
            sportC.Value1 = "202";
        }

        private void segmentC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
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

        private void segmentC_Enter(object sender, EventArgs e)
        {
            segmentC.Value1 = "203";
        }
        private void makershohinC_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
                if(makershohinC.SelectData())
                {
                    makershohinC.Value1 = makershohinC.TxtCode.Text;
                    makershohinC.Value2 = makershohinC.LabelText;
                }
                else
                {
                    bbl.ShowMessage("E102");
                    makershohinC.SetFocus(1);
                }
            }
        }
        private void itemcd_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (itemcd.SelectData())
                {
                    itemcd.Value1 = itemcd.TxtCode.Text;
                    itemcd.Value2 = itemcd.LabelText;
                }
                else
                {
                    bbl.ShowMessage("E101");
                    itemcd.SetFocus(1);
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
        private void F11()
        {
            //if(ErrorCheck())
            //{
               
                m_IOE = GetItemorder();
                m_IE = GetItem();
                brand.Clear();
                sport.Clear();
                segment.Clear();
                CB_year.Text = string.Empty;
                CB_season.Text = string.Empty;
                TB_date_condition.Text = string.Empty;
                makershohin.Clear();
             dt = bl.M_ItemOrderPrice_Insert(m_IOE, m_IE);
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
            shiiresaki.Clear();
            RB_zenten.Checked = true;
            RB_item.Checked = true;
            TB_headerdate.Text = bbl.GetDate();
            brand.Clear();
            sport.Clear();
            segment.Clear();
            CB_year.Text = String.Empty;
            CB_season.Text = string.Empty;
            TB_date_condition.Text = string.Empty;
            makershohin.Clear();
            itemcd.Clear();
            TB_date_add.Text = string.Empty;
            LB_priceouttax1.Text = string.Empty;
            TB_rate.Text = string.Empty;
            TB_pricewithouttax.Text = string.Empty;
            brandC.Clear();
            sportC.Clear();
            segment.Clear();
            CB_year.Text = string.Empty;
            cb_seasonC.Text = string.Empty;
            TB_dateC.Text = string.Empty;
            makershohinC.Clear();
            TB_dateE.Text = string.Empty;
            TB_rate_E.Text = string.Empty;
            GV_item.Refresh();
           
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
        private void Btn_display_Click(object sender, EventArgs e)
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
            }
            else
            {
                panel4.Enabled = true;
                panel5.Enabled = true;
                this.ブランド.Visible = true;
                this.シーズン.Visible = true;
                this.年度.Visible = true;
                this.商品分類.Visible = true;
                this.競技.Visible = true;
                this.サイズ.Visible = false;
                this.カラー.Visible = false;
                this.SKUCD.Visible = false;
                
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
            //if (ErrorCheck())
            //{
            //m_IOE = GetItemorder();
            //m_IE = GetItem();
            //DataTable dt = bl.M_ItemOrderPrice_Insert(m_IOE, m_IE);
            DataTable dataTable = new DataTable();
                string selectq="";
                selectq = "  ItemCD = '" + itemcd.TxtCode.Text + "'";
                selectq += " and ChangeDate = '" + TB_date_add.Text + "'";
                selectq += " and Rate = '" + TB_rate.Text + "'";
                selectq += " and PriceOutTax = '" + LB_priceouttax.Text + "'";
                selectq += " and PriceWithoutTax = '" + TB_pricewithouttax.Text + "'";
                if (GV_item.DataSource !=null)
                {
                dv.RowFilter = selectq;
                if(dv.Count >0)
                {
                    bbl.ShowMessage("E224");
                }
                else
                {
                    DataRow row1;
                    if (GV_item.DataSource != null)
                    {
                        row1 = dt.NewRow();
                        row1["Tempkey"] = "1";
                        row1["CheckBox"] = "0";
                        row1["BrandCD"] = brand.TxtCode.Text;
                        row1["SportsCD"] = sport.TxtCode.Text;
                        row1["SegmentCD"] = segment.TxtCode.Text;
                        row1["LastYearTerm"] = CB_year.Text;
                        row1["LastSeason"] = CB_season.Text;
                        row1["MakerItem"] = makershohin.TxtCode.Text;
                        row1["ItemCD"] = itemcd.TxtCode.Text;
                        row1["ChangeDate"] = "2020-06-08";
                        row1["Rate"] = TB_rate.Text;
                        row1["PriceOutTax"] = LB_priceouttax.Text;
                        row1["PriceWithoutTax"] = TB_pricewithouttax.Text;
                        row1["Tempkey"] = "1";
                        dt.Rows.Add(row1);
                        dt.AcceptChanges();
                        GV_item.Refresh();
                        GV_item.DataSource = dt;
                       // dv.RowStateFilter = DataViewRowState.ModifiedCurrent;
                        dv.RowStateFilter = DataViewRowState.Unchanged;
                    }
                    else
                    {
                        GV_item.Rows.Add(false, brand.TxtCode.Text, sport.TxtCode.Text,"", "", "", "", "", "", "", "", "", "");
                    }
                }
            }
            //}
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
                DataRow[] dr = dtc.Select(choiceq + dateq);
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
            }
        }

        private void btn_releaseall_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in GV_item.Rows)
            {
                row.Cells["ck"].Value = "0";
            }
        }

        private void btn_choiceCopy_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(TB_dateE.Text))
            {
                string date = "";
                date = " and ChangeDate = '" + TB_dateE.Text + "'";
                date += " and CheckBox =1";
                string copyq = choiceq +date;
                DataRow[] dr = dt.Select(copyq);
                if(dr.Length <0)
                {
                    for(int i=0;i< dr.Length;i++)
                    {
                        //GV_item.Rows.Add(false, brandC.TxtCode.Text, sportC.TxtCode.Text,segmentC.TxtCode.Text
                        //    ,CB_yearC.Text ,cb_seasonC.Text
                        //    ,makershohinC.TxtCode.Text,, "", "", "", "", "");
                    }
                  
                }

            }
        }
    }
}
