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
using Search;

namespace ZaikoShoukai 
{
    public partial class ZaikoShoukai : FrmMainForm
    {
        Base_BL bbl = new Base_BL();
        M_SKU_Entity msku_Entity;
        M_SKUInfo_Entity msInfo_Entity;
        M_SKUTag_Entity msT_Entity;

        public ZaikoShoukai()
        {
            InitializeComponent();
        }
        private void ZaikoShoukai_Load(object sender, EventArgs e)
        {
            InProgramID = "ZaikoShoukai";
            StartProgram();
            BindCombo();
            ckM_LB_Kijun.Text = Convert.ToDateTime(DateTime.Today).ToShortDateString();
            ckM_RB_or.Checked = true;
            ModeVisible = false;
            CB_Soko.Focus();
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void BindCombo()
        {
            CB_Soko.Bind(String.Empty,"");
            CB_Soko.SelectedIndex = 1;
            CB_nen_do.Bind(String.Empty, "");
            CB_Season.Bind(String.Empty, "");
            CB_ko_yaku.Bind(String.Empty, "");
            CB_Toku_ki.Bind(String.Empty, "");
            CB_Oku_ryo.Bind(String.Empty, "");
            CB_Hatsuchu.Bind(String.Empty, "");
            CB_Tagu1.Bind(String.Empty, "");
            CB_Tagu1.Bind(String.Empty, "");
            CB_Tagu2.Bind(string.Empty, "");
            CB_Tagu3.Bind(string.Empty, "");
            CB_Tagu4.Bind(string.Empty, "");
            CB_Tagu5.Bind(string.Empty, "");
        }
        //private bool ErrorCheck()
        //{
            
        //   // return true;
        //}
        private void F11()
        {
            msku_Entity = GetDataEntity();
            msInfo_Entity = GetInfoEntity();
            msT_Entity = GetTagEntity();
        }

        private M_SKU_Entity GetDataEntity()
        {
            msku_Entity = new M_SKU_Entity()
            {
                MainVendorCD = ckM_Search_Shiiresaki.TxtCode.Text,
                MakerVendorCD = Maker.TxtCode.Text,
                BrandCD = Brand.TxtCode.Text,
                SKUName = TB_Shohinmei.Text,
                JanCD=TB_Jancd.Text,
                SKUCD=TB_Skucd.Text,
                MakerItem=TB_mekashohinCD.Text,
                ITemCD=TB_item.Text,
                CommentInStore=TB_Bikokeyword.Text,
                ReserveCD=CB_ko_yaku.Text,
                NoticesCD=CB_Toku_ki.Text,
                PostageCD=CB_Oku_ryo.Text,
                OrderAttentionCD=CB_Hatsuchu.Text,
                SportsCD=Kei_waza.TxtCode.Text,
                InsertDateTime= TB_ShinkitorokuF.Text,
                
               // InsertDateTime=ShinkitorokuT.Text,
               UpdateDateFrom= TＢ_SaiShuhenkobiF.Text,
               UpdateDateTo= TB_SaiShuhenkobiT.Text,
               ApprovalDateFrom=TB_ShoninbiF.Text,
               ApprovalDateTo=TB_ShoninbiT.Text,
            };
            return msku_Entity;
        }

        private M_SKUInfo_Entity GetInfoEntity()
        {
            msInfo_Entity = new M_SKUInfo_Entity()
            {
                YearTerm = CB_nen_do.Text,
                Season=CB_Season.Text,
                CatalogNO=TB_Catalog.Text,
                InstructionsNO=TB_Shijishobengo.Text,
            };
            return msInfo_Entity;
        }

        private M_SKUTag_Entity GetTagEntity()
        {
            msT_Entity = new M_SKUTag_Entity()
            {
                TagName1 = CB_Tagu1.Text,
            };
            return msT_Entity;
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
               
            }
        }
        private void CanCelData()
        {
            CB_Soko.SelectedIndex = 1;
            CB_nen_do.Text = string.Empty;
            CB_Hatsuchu.Text = string.Empty;
            CB_ko_yaku.Text = String.Empty;
            CB_Oku_ryo.Text = String.Empty;
            CB_Season.Text = string.Empty;
            CB_Toku_ki.Text = string.Empty;
            CB_Tagu1.Text = String.Empty;
            CB_Tagu2.Text = String.Empty;
            CB_Tagu3.Text = String.Empty;
            CB_Tagu4.Text = String.Empty;
            CB_Tagu5.Text = String.Empty;
            TB_mekashohinCD.Text = string.Empty;
            TB_item.Text = string.Empty;
            TB_Bikokeyword.Text = string.Empty;
            TB_item.Text = string.Empty;
            TB_Catalog.Text = string.Empty;
            TB_mekashohinCD.Text = string.Empty;
            TB_Shijishobengo.Text = string.Empty;
            TB_ShinkitorokuF.Text = String.Empty;
            ShinkitorokuT.Text = string.Empty;
            TB_Shohinmei.Text = string.Empty;
            TB_ShoninbiF.Text = string.Empty;
            TB_ShoninbiT.Text = string.Empty;
            TB_TanabanFrom.Text = string.Empty;
            TB_TanabanTo.Text = string.Empty;
            TＢ_SaiShuhenkobiF.Text = string.Empty;
            TB_SaiShuhenkobiT.Text = String.Empty;
            TB_Jancd.Text = string.Empty;
            TB_Skucd.Text = string.Empty;
            ckM_Search_Shiiresaki.Clear();
            Brand.Clear();
            Maker.Clear();
            Kei_waza.Clear();
            ckM_RB_or.Checked = true;
            ckM_RB_and.Checked = false;
            ckM_CKB_Mishohin.Checked = false;
            ckM_CKB_suru.Checked = false;
            ckM_CKB_searchsuru.Checked = false;
            ckM_RB_Searchitem.Checked = false;
            ckM_RB_Makashohincd.Checked = false;
            CB_Soko.Focus();
        }
       
        private void ZaikoShoukai_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
       
        private void TB_TanabanTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_TanabanFrom.Text) && !String.IsNullOrEmpty(TB_TanabanTo.Text))
                {
                    if (String.Compare(TB_TanabanFrom.Text, TB_TanabanTo.Text) == 1)
                    {
                        bbl.ShowMessage("106");
                    }
                }
            }
        }

        private void Shiiresaki_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ckM_Search_Shiiresaki.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(ckM_Search_Shiiresaki.TxtCode.Text))
                {
                    if (ckM_Search_Shiiresaki.SelectData())
                    {

                        ckM_Search_Shiiresaki.Value1 = ckM_Search_Shiiresaki.TxtCode.Text;
                        ckM_Search_Shiiresaki.Value2 = ckM_Search_Shiiresaki.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ckM_Search_Shiiresaki.SetFocus(1);
                    }
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
                    if (Maker.SelectData())
                    {
                        Maker.Value1 = Maker.TxtCode.Text;
                        Maker.Value2 = Maker.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        Maker.SetFocus(1);
                    }
                }
            }
        }

        private void Brand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Brand.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(Brand.TxtCode.Text))
                {
                    if (Brand.SelectData())
                    {
                        Brand.Value1 = Brand.TxtCode.Text;
                        Brand.Value2 = Brand.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        Brand.SetFocus(1);
                    }
                }
            }
        }

        private void Kei_waza_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Kei_waza.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(Kei_waza.TxtCode.Text))
                {
                    if (Kei_waza.SelectData())
                    {
                        Kei_waza.Value1 = Kei_waza.TxtCode.Text;
                        Kei_waza.Value2 = Kei_waza.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        Kei_waza.SetFocus(1);
                    }
                }
            }
        }

        private void Kei_waza_Enter(object sender, EventArgs e)
        {
            Kei_waza.Value1 = "202";
        }

        private void ShinkitorokuT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_ShinkitorokuF.Text) && !String.IsNullOrEmpty(ShinkitorokuT.Text))
                {
                    if (Convert.ToDateTime(TB_ShinkitorokuF.Text) > Convert.ToDateTime(ShinkitorokuT.Text))
                    {
                        bbl.ShowMessage("E104");
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
                    }
                }
            }
        }
    }
}
