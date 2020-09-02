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

namespace MasterTouroku_TenzikaiHanbaiTankaKakeritu
{
    public partial class FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu : FrmMainForm
    {
        M_TenzikaiShouhin_Entity mTSE;
        MasterTouroku_TenzikaiHanbaiTankaKakeritu_BL bl;
        DataTable dtSelect;
        public FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu()
        {
            InitializeComponent();
            mTSE = new M_TenzikaiShouhin_Entity();
            bl = new MasterTouroku_TenzikaiHanbaiTankaKakeritu_BL();
            dtSelect = new DataTable();
        }
        private void FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu_Load(object sender, EventArgs e)
        {

            InProgramID = "MasterTouroku_TenzikaiHanbaiTankaKakeritu";
            StartProgram();
            string ymd = bbl.GetDate();
            CB_Year.Bind(ymd);
            CB_Season.Bind(ymd);
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { SC_Tanka.TxtCode ,TB_Rate})) //Step1
                return false;

            if (!SC_Tanka.IsExists(2))
            {
                bbl.ShowMessage("E101");
                SC_Tanka.SetFocus(1);
                return false;
            }
            if(!String.IsNullOrEmpty(SC_Brand.TxtCode.Text))
            {
                if (!SC_Brand.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_Brand.SetFocus(1);
                    return false;
                }
            }
            
            if (!String.IsNullOrEmpty(Sc_Segment.TxtCode.Text))
            {
                if (!Sc_Segment.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    Sc_Segment.SetFocus(1);
                    return false;
                }
            }
            
            //Console.WriteLine(header.Trim(new Char[] { ' ', '*', '.' }));

            if (!String.IsNullOrEmpty(TB_PriceOutTaxF.Text))
            {
                int str2 = 0;
                int str1 = Convert.ToInt32(TB_PriceOutTaxF.Text.ToString().Replace(",", ""));
                if (string.IsNullOrWhiteSpace(TB_PriceOutTaxT.Text.ToString()))
                    str2 = 0;
                else
                    str2 = Convert.ToInt32(TB_PriceOutTaxT.Text.ToString().Replace(",", ""));
                if (str1 > str2)
                {
                    bbl.ShowMessage("E104");
                    TB_PriceOutTaxT.Focus();
                    return false;
                }
            }


            return true;

        }
      

        private void SC_Tanka_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(SC_Tanka.TxtCode.Text))
                {
                    SC_Tanka.ChangeDate = bbl.GetDate();
                    if (!SC_Tanka.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_Tanka.SetFocus(1);
                    }
                }

            }
        }

        private void SC_Brand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(SC_Brand.TxtCode.Text))
                {
                    if(!SC_Brand.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_Brand.SetFocus(1);
                    }
                    
                }
            }
        }

        private void Sc_Segment_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(Sc_Segment.TxtCode.Text))
                {
                    if(!Sc_Segment.SelectData())
                     {
                        bbl.ShowMessage("E101");
                        Sc_Segment.SetFocus(1);
                    }
                   
                }
            }
        }


        public override void FunctionProcess(int Index)
        {
            switch (Index + 1)
            {
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        CleanData();
                        SC_Tanka.SetFocus(1);
                    }
                    break;
                case 3:
                    ChangeMode(EOperationMode.UPDATE);
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    F12();
                    break;
            }
        }
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
               
                case EOperationMode.UPDATE:
                    Clear(panel1);
                    SC_Tanka.SetFocus(1);
                    break;
            }
            
        }
        private void CleanData()
        {
            Clear(panel1);
            GV_Tenzaishohin.DataSource = null;

        }

        private void SetRequireField()
        {
            SC_Tanka.TxtCode.Require(true);
            TB_Rate.Require(true);
        }
        protected override void EndSec()
        {
            this.Close();
        }

        private void Sc_Segment_Enter(object sender, EventArgs e)
        {
            Sc_Segment.Value1 = "226";
        }

        private void BT_SelectAll_Click(object sender, EventArgs e)
        {
            if (GV_Tenzaishohin.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GV_Tenzaishohin.Rows)
                {

                    row.Cells["CheckBox"].Value = "1";
                }
            }
        }

        private void BT_DeseletAll_Click(object sender, EventArgs e)
        {
            if (GV_Tenzaishohin.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GV_Tenzaishohin.Rows)
                {
                    row.Cells["CheckBox"].Value = "0";
                }
            }
        }

        private void BT_Display_Click(object sender, EventArgs e)
        {
            F11();
        }
        private void F11()
        {
            mTSE = GetTenzikaiShouhinData();
            dtSelect = bl.M_TenzikaiShouhin_Select(mTSE);
            if(dtSelect.Rows.Count >0)
            {
                GV_Tenzaishohin.DataSource = dtSelect;
            }
            else
            {
                GV_Tenzaishohin.DataSource = null;
                bl.ShowMessage("E128");
            }
       }


        private void F12()
        {
            if(dtSelect.Rows.Count >0)
            {
                mTSE = GetTenzikaiShouhinData();
                String dtinsert = bl.DataTableToXml(dtSelect);
                if (bl.InsertUpdate_TenzikaiHanbaiTankaKakeritu(mTSE, dtinsert))
                {
                  bbl.ShowMessage("I101");
                    CleanData();
                    SC_Tanka.SetFocus(1);
                }
                else
                {
                    bbl.ShowMessage("S001");
                }


            }
        }

       private M_TenzikaiShouhin_Entity GetTenzikaiShouhinData()
        {
            mTSE = new M_TenzikaiShouhin_Entity()
            {
                TanKaCD = SC_Tanka.TxtCode.Text,
                BranCDFrom = SC_Brand.TxtCode.Text,
                SegmentCDFrom = Sc_Segment.TxtCode.Text,
                LastYearTerm = CB_Year.SelectedValue.ToString(),
                LastSeason = CB_Season.SelectedValue.ToString(),
                SalePriceOutTaxF = TB_PriceOutTaxF.Text,
                SalePriceOutTaxT = TB_PriceOutTaxT.Text,
                InsertOperator = InOperatorCD,
                PC = InPcID,
                ProgramID = InProgramID,
                Key=InPcID,
            };

            return mTSE;
        }

        private void GV_Tenzaishohin_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch
            {
                MessageBox.Show("Enter valid no");
                GV_Tenzaishohin.RefreshEdit();
            }
        }

        private void BT_Apply_Click(object sender, EventArgs e)
        {
            if(ErrorCheck())
            {
                foreach (DataGridViewRow row in GV_Tenzaishohin.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                    //string celll = row.Cells["CheckBox"].Value.ToString();
                    //string cell = chk.ToString();
                    if (chk.Value == "1")
                    {
                        string itemcd = row.Cells["Rate"].Value.ToString();
                        row.Cells["Rate"].Value = TB_Rate.Text;
                    }

                }
            }
        }
    }
}
