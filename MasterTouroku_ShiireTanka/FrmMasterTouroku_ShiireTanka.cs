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

namespace MasterTouroku_ShiireTanka
{
    public partial class FrmMasterTouroku_ShiireTanka : FrmMainForm
    {
        Base_BL bbl = new Base_BL();
        bool cb_focus = false;
        public FrmMasterTouroku_ShiireTanka()
        {
            InitializeComponent();
        }
        private void FrmMasterTouroku_ShiireTanka_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_ShiireTanka";
            StartProgram();
            ModeText = "ITEM";
            BindCombo();
            TB_Changedate.Text = bbl.GetDate();
        }
        private void BindCombo()
        {
            string ymd = bbl.GetDate();
            CB_store.Bind(ymd);
            if(RB_koten.Checked == true)
            {
                CB_store.SelectedValue = StoreCD;
            }
            CB_year.Bind(ymd);
            CB_season.Bind(ymd);
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
        private bool ErrorCheck()
        {
            if (String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
            {
                bbl.ShowMessage("E102");
                shiiresaki.Focus();
                return false;
            }
            else
            {
                if (shiiresaki.IsExists(2))
                {
                    {
                        bbl.ShowMessage("E102");
                        shiiresaki.Focus();
                        return false;
                    }
                }
            }
           
            if(string.IsNullOrEmpty(TB_Changedate.Text))
            {
                bbl.ShowMessage("E102");
                TB_Changedate.Focus();
                return false;
            }
            
            if(RB_koten.Checked == true )  
            {
                if(String.IsNullOrEmpty(CB_store.Text))
                {
                    bbl.ShowMessage("E102");
                }
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
            shiiresaki.ChangeDate = TB_Changedate.Text;
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
        #endregion
        private void F11()
        {
            if(ErrorCheck())
            {

            }

        }
        private void Clear()
        {
            shiiresaki.Clear();
            RB_zenten.Checked = true;
            RB_item.Checked = true;
            TB_Changedate.Text = bbl.GetDate();
            brand.Clear();
            sport.Clear();
            segment.Clear();
            CB_year.Text = String.Empty;
            CB_season.Text = string.Empty;
            TB_date_condition.Text = string.Empty;
            makershohin.Clear();
            itemcd.Clear();
            TB_date_add.Text = string.Empty;
            LB_listprice.Text = string.Empty;
            TB_rate.Text = string.Empty;
            TB_orderprice.Text = string.Empty;
            brandC.Clear();
            sportC.Clear();
            segment.Clear();
            CB_year.Text = string.Empty;
            cb_seasonC.Text = string.Empty;
            TB_dateC.Text = string.Empty;
            makershohinC.Clear();
            TB_dateE.Text = string.Empty;
            TB_rate_E.Text = string.Empty;
            
        }

        private void RB_koten_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_koten.Checked == true)
            {
                CB_store.SelectedValue = StoreCD;
            }
            else
            {
                CB_store.Text = string.Empty;
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
    }
}
