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
using Search;

namespace ZaikoShoukai 
{
    public partial class ZaikoShoukai : FrmMainForm
    {
        Base_BL bbl = new Base_BL();

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
            ckM_CB_Soko.Focus();
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void BindCombo()
        {
            ckM_CB_Soko.Bind(String.Empty,"");
            ckM_CB_Soko.SelectedIndex = 1;
            ckM_CB_nen_do.Bind(String.Empty, "");
            ckM_CB_Season.Bind(String.Empty, "");
            ckM_CB_ko_yaku.Bind(String.Empty, "");
            ckM_CB_Toku_ki.Bind(String.Empty, "");
            ckM_CB_Oku_ryo.Bind(String.Empty, "");
            ckM_CB_Hatsuchu.Bind(String.Empty, "");
            ckM_CB_Tagu1.Bind(String.Empty, "");
            ckM_CB_Tagu1.Bind(String.Empty, "");
            ckM_CB_Tagu2.Bind(string.Empty, "");
            ckM_CB_Tagu3.Bind(string.Empty, "");
            ckM_CB_Tagu4.Bind(string.Empty, "");
            ckM_CB_Tagu5.Bind(string.Empty, "");
        }
        //private bool ErrorCheck()
        //{
            
        //   // return true;
        //}
        private void F11()
        {
            
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
            ckM_CB_Soko.SelectedIndex = 1;
            ckM_CB_nen_do.Text = string.Empty;
            ckM_CB_Hatsuchu.Text = string.Empty;
            ckM_CB_ko_yaku.Text = String.Empty;
            ckM_CB_Oku_ryo.Text = String.Empty;
            ckM_CB_Season.Text = string.Empty;
            ckM_CB_Toku_ki.Text = string.Empty;
            ckM_CB_Tagu1.Text = String.Empty;
            ckM_CB_Tagu2.Text = String.Empty;
            ckM_CB_Tagu3.Text = String.Empty;
            ckM_CB_Tagu4.Text = String.Empty;
            ckM_CB_Tagu5.Text = String.Empty;
            ckM_TB_mekashohinCD.Text = string.Empty;
            ckM_TB_item.Text = string.Empty;
            ckM_TB_Bikokeyword.Text = string.Empty;
            ckM_TB_item.Text = string.Empty;
            ckM_TB_Katarogu.Text = string.Empty;
            ckM_TB_mekashohinCD.Text = string.Empty;
            ckM_TB_Shijishobengo.Text = string.Empty;
            ckM_TB_ShinkitorokuF.Text = String.Empty;
            ckM_TB_ShinkitorokuT.Text = string.Empty;
            ckM_TB_Shohinmei.Text = string.Empty;
            ckM_TB_ShoninbiF.Text = string.Empty;
            ckM_TB_ShoninbiT.Text = string.Empty;
            ckM_TB_TanabanFrom.Text = string.Empty;
            ckM_TB_TanabanTo.Text = string.Empty;
            ckM_TＢ_SaiShuhenkobiF.Text = string.Empty;
            ckM_TＢ_SaiShuhenkobiT.Text = String.Empty;
            ckM_TB_Jancd.Text = string.Empty;
            ckM_TB_Skucd.Text = string.Empty;
            ckM_Search_Shiiresaki.Clear();
            ckM_Search_Brand.Clear();
            ckM_Search_Meka.Clear();
            ckM_Search_Kei_waza.Clear();
            ckM_RB_or.Checked = true;
            ckM_RB_and.Checked = false;
            ckM_CKB_Mishohin.Checked = false;
            ckM_CKB_suru.Checked = false;
            ckM_CKB_searchsuru.Checked = false;
            ckM_RB_Searchitem.Checked = false;
            ckM_RB_Makashohincd.Checked = false;
            ckM_CB_Soko.Focus();
        }
        private void ckM_TB_TanabanTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(ckM_TB_TanabanFrom.Text) && !String.IsNullOrEmpty(ckM_TB_TanabanTo.Text))
                {
                    if(String.Compare(ckM_TB_TanabanFrom.Text,ckM_TB_TanabanTo.Text) ==1)
                    {
                        bbl.ShowMessage("106");
                    }
                }
            }
        }
        private void ckM_TB_ShinkitorokuT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(ckM_TB_ShinkitorokuF.Text) && !String.IsNullOrEmpty(ckM_TB_ShinkitorokuT.Text))
                {
                    if (Convert.ToDateTime(ckM_TB_ShinkitorokuF.Text) > Convert.ToDateTime(ckM_TB_ShinkitorokuT.Text))
                    {
                        bbl.ShowMessage("E104");
                    }
                }
            }
        }
        private void ckM_TＢ_SaiShuhenkobiT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(ckM_TＢ_SaiShuhenkobiF.Text) && !String.IsNullOrEmpty(ckM_TＢ_SaiShuhenkobiT.Text))
                {
                    if (Convert.ToDateTime(ckM_TＢ_SaiShuhenkobiF.Text) > Convert.ToDateTime(ckM_TＢ_SaiShuhenkobiT.Text))
                    {
                        bbl.ShowMessage("E104");
                    }
                }
            }
        }
        private void ckM_TB_ShoninbiT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(ckM_TB_ShoninbiF.Text) && !String.IsNullOrEmpty(ckM_TB_ShoninbiT.Text))
                {
                    if (Convert.ToDateTime(ckM_TB_ShoninbiF.Text) > Convert.ToDateTime(ckM_TB_ShoninbiT.Text))
                    {
                        bbl.ShowMessage("E104");
                    }
                }
            }
        }
        private void ckM_Search_Shiiresaki_CodeKeyDownEvent(object sender, KeyEventArgs e)
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
        private void ckM_Search_Meka_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ckM_Search_Meka.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(ckM_Search_Meka.TxtCode.Text))
                {
                    if (ckM_Search_Meka.SelectData())
                    {
                        ckM_Search_Meka.Value1 = ckM_Search_Meka.TxtCode.Text;
                        ckM_Search_Meka.Value2 = ckM_Search_Meka.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ckM_Search_Meka.SetFocus(1);
                    }
                }
            }
        }
        private void ckM_Search_Brand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ckM_Search_Brand.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(ckM_Search_Brand.TxtCode.Text))
                {
                    if (ckM_Search_Brand.SelectData())
                    {
                        ckM_Search_Brand.Value1 = ckM_Search_Brand.TxtCode.Text;
                        ckM_Search_Brand.Value2 = ckM_Search_Brand.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ckM_Search_Brand.SetFocus(1);
                    }
                }
            }
        }
        private void ckM_Search_Kei_waza_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ckM_Search_Kei_waza.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(ckM_Search_Kei_waza.TxtCode.Text))
                {
                    if (ckM_Search_Kei_waza.SelectData())
                    {
                        ckM_Search_Kei_waza.Value1 = ckM_Search_Kei_waza.TxtCode.Text;
                        ckM_Search_Kei_waza.Value2 = ckM_Search_Kei_waza.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ckM_Search_Kei_waza.SetFocus(1);
                    }
                }
            }
        }
        private void ZaikoShoukai_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        private void ckM_TB_Bikokeyword_KeyDown(object sender, KeyEventArgs e)
        {
            string st = ckM_TB_Bikokeyword.Text;
            if (e.KeyCode == Keys.Enter)
            {
                
            }
        }
        private void ckM_Search_Kei_waza_Enter(object sender, EventArgs e)
        {
            ckM_Search_Kei_waza.Value1 = "202";
        }
    }

}
