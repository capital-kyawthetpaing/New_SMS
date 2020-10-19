using Base.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;
using BL;


namespace Search
{
    public partial class Search_TenzikaiShouhin : FrmSubForm
    {
        M_TenzikaiShouhin_Entity mt = new M_TenzikaiShouhin_Entity();
        Search_TenzikaiShouhin_BL bl;
            Base_BL bbl;
        public Search_TenzikaiShouhin()
        {
            InitializeComponent();
            bbl = new Base_BL();
            bl = new Search_TenzikaiShouhin_BL();
        }

        public string parJanCD= "";
        public string parSkuCD = "";
        public string parSKUName = "";
        public string parChangeDate = "";
        private void Search_TenzikaiShouhin_Load(object sender, EventArgs e)
        {
            string ymd = bbl.GetDate();
            LB_ChangeDate.Text = ymd;
            CB_Year.Bind(ymd);
            CB_Season.Bind(ymd);
            HeaderTitleText = "展示会商品検索";
            SetRequiredField();
        }
        private void SetRequiredField()
        {
            CB_Year.Require(true);
            CB_Season.Require(true);
        }
        public override void FunctionProcess(int index)
        {
            
            switch (index + 1)
            {
                case 11:
                    F11();
                    break;
                case 12:
                    GetData();
                    break;

            }
        }
        private void BT_Display_Click(object sender, EventArgs e)
        {
            F11();
        }  

        private void F11()
        {
            if (ErrorCheck())
            {
                mt = GetDataEntity();
                DataTable dt = bl.M_Tenzikaishouhin_Search(mt);
                if (dt.Rows.Count > 0)
                {
                    GV_TZshouhin.DataSource = dt;
                    GV_TZshouhin.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GV_TZshouhin.CurrentRow.Selected = true;
                    GV_TZshouhin.Enabled = true;
                    GV_TZshouhin.Focus();
                }
                else
                {
                    GV_TZshouhin.DataSource = null;
                    bbl.ShowMessage("E128");
                }
            }
        }

        public M_TenzikaiShouhin_Entity GetDataEntity()
        {
            mt = new M_TenzikaiShouhin_Entity()
            {
                TenzikaiName=TB_Tenziname.Text,
                VendorCD=SC_Vendor.TxtCode.Text,
                LastYearTerm=CB_Year.SelectedValue.ToString(),
                LastSeason=CB_Season.SelectedValue.ToString(),
                SKUName=TB_SKUname.Text,
                BranCDFrom= SC_Brand.TxtCode.Text,
                SegmentCDFrom=SC_segment.TxtCode.Text,
                InsertDateFrom=TB_InsertDateTimeF.Text,
                InsertDateTo=TB_InsertDateTimeT.Text,
                UpdateDateFrom=TB_UpdateDateTimeF.Text,
                UpdateDateTo=TB_UpdateDateTimeT.Text
            };
            return mt;
        }

        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { CB_Year, CB_Season })) //Step1
                return false;
            if (!String.IsNullOrEmpty(SC_Vendor.TxtCode.Text))
            {
                if (!SC_Vendor.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_Vendor.SetFocus(1);
                    return false;
                }
            }

            if (!String.IsNullOrEmpty(SC_Brand.TxtCode.Text))
            {
                if (!SC_Brand.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_Brand.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(SC_segment.TxtCode.Text))
            {
                if (!SC_segment.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_segment.SetFocus(1);
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(TB_InsertDateTimeF.Text) && !string.IsNullOrWhiteSpace(TB_InsertDateTimeT.Text))
            {
                DateTime dt1 = Convert.ToDateTime(TB_InsertDateTimeF.Text);
                DateTime dt2 = Convert.ToDateTime(TB_InsertDateTimeT.Text);

                if (dt1 > dt2)
                {
                    bbl.ShowMessage("E104");
                    TB_InsertDateTimeT.Focus();
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(TB_UpdateDateTimeF.Text) && !string.IsNullOrWhiteSpace(TB_UpdateDateTimeT.Text))
            {
                DateTime dt1 = Convert.ToDateTime(TB_UpdateDateTimeF.Text);
                DateTime dt2 = Convert.ToDateTime(TB_UpdateDateTimeT.Text);

                if (dt1 > dt2)
                {
                    bbl.ShowMessage("E104");
                    TB_UpdateDateTimeT.Focus();
                    return false;
                }
            }
            return true;
        }
       
        private void GV_TZshouhin_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GetData();
        }  

        private void GetData()
        {
            try
            {
                if (GV_TZshouhin.CurrentRow != null && GV_TZshouhin.CurrentRow.Index >= 0)
                {
                    parJanCD= GV_TZshouhin.CurrentRow.Cells["JANCD"].Value.ToString();
                    parSkuCD = GV_TZshouhin.CurrentRow.Cells["SKUCD"].Value.ToString();
                    parSKUName = GV_TZshouhin.CurrentRow.Cells["SKUName"].Value.ToString();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

       

        private void GV_TZshouhin_KeyDown(object sender, KeyEventArgs e)
        {
            GetData();
        }

      

        private void SC_Vendor_Enter(object sender, EventArgs e)
        {
            SC_Vendor.Value1 = "1";
        }

        
        private void SC_segment_Enter(object sender, EventArgs e)
        {
            SC_segment.Value1 = "226";
        }

        private void Search_TenzikaiShouhin_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void SC_Vendor_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
                if (!String.IsNullOrEmpty(SC_Vendor.TxtCode.Text))
                {
                    SC_Vendor.ChangeDate = bbl.GetDate();
                    if (!SC_Vendor.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_Vendor.SetFocus(1);
                    }
                }
        }

        private void SC_Brand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(SC_Brand.TxtCode.Text))
            {
                SC_Brand.ChangeDate = bbl.GetDate();
                if (!SC_Brand.SelectData())
                {
                    bbl.ShowMessage("E101");
                    SC_Brand.SetFocus(1);
                }
            }
        }

        private void SC_segment_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(SC_segment.TxtCode.Text))
            {
                SC_segment.ChangeDate = bbl.GetDate();
                if (!SC_segment.SelectData())
                {
                    bbl.ShowMessage("E101");
                    SC_segment.SetFocus(1);
                }
            }
        }

        private void TB_InsertDateTimeT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_InsertDateTimeF.Text) && !String.IsNullOrEmpty(TB_InsertDateTimeT.Text))
                {
                    if (Convert.ToDateTime(TB_InsertDateTimeF.Text) > Convert.ToDateTime(TB_InsertDateTimeT.Text))
                    {
                        bbl.ShowMessage("E104");
                        TB_InsertDateTimeT.Focus();
                    }
                }
            }
        }

        private void TB_UpdateDateTimeT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_UpdateDateTimeF.Text) && !String.IsNullOrEmpty(TB_UpdateDateTimeT.Text))
                {
                    if (Convert.ToDateTime(TB_UpdateDateTimeF.Text) > Convert.ToDateTime(TB_UpdateDateTimeT.Text))
                    {
                        bbl.ShowMessage("E104");
                        TB_UpdateDateTimeT.Focus();
                    }
                }
            }
        }
    }
}
