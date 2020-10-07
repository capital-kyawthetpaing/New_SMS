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

namespace TenzikaiHacchuuJouhouShuturyoku
{
    public partial class FrmTenzikaiHacchuuJouhouShuturyoku : FrmMainForm
    {
        TenzikaiHacchuuJouhouShuturyoku_BL tzbl = new TenzikaiHacchuuJouhouShuturyoku_BL();

        public FrmTenzikaiHacchuuJouhouShuturyoku()
        {
            InitializeComponent();
        }

        private void FrmTenzikaiHacchuuJouhouShuturyoku_Load(object sender, EventArgs e)
        {
            InProgramID = "TenzikaiHacchuuJouhouShuturyoku";

            StartProgram();

            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            F7Visible = false;
            F8Visible = false;
            Btn_F10.Text = "データ出力(F10)";
            F12Visible = false;

            BindCombo();
            ScSupplier.SetFocus(1);
            SetRequiredField();
          
            ModeVisible = false;
        }

        public void BindCombo()
        {
            string ymd = bbl.GetDate();
            cboYear.Bind(ymd);
            cboSeason.Bind(ymd);
        }

        public void SetRequiredField()
        {
            ScSupplier.TxtCode.Require(true);

        }

        protected override void EndSec()
        {
            this.Close();
        }

        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                case 3:
                case 4:                  
                case 5:
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear();
                        ScSupplier.SetFocus(1);
                    }
                    break;
                case 10:
                    F10();    
                    break;
                case 11:                 
                    break;
                case 12:
                    break;
            }
        }

        public void Clear()
        {
            ScSupplier.Clear();
            cboYear.SelectedValue.Equals(-1);
            cboSeason.SelectedValue.Equals(-1);
            ScBrandCD.Clear();
            ScSegmentCD.Clear();
            ScExhibitionCD.Clear();
            ScClient1.Clear();
            ScClient2.Clear();
            rdoCustomer.Checked = true;
            rdoProduct.Checked = false;
        }

        public void F10()
        {

        }

        public bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { ScSupplier.TxtCode }))
                return false;
            else
            {
                if (!ScSupplier.IsExists(2))
                {
                    tzbl.ShowMessage("E101");
                    ScSupplier.SetFocus(1);
                    return false;
                }
            }

            if(cboYear.SelectedValue.ToString() == "-1")
            {               
                tzbl.ShowMessage("E102");
                cboYear.Focus();
                return false;
            }

            if (cboSeason.SelectedValue.ToString() == "-1")
            {
                tzbl.ShowMessage("E102");
                cboSeason.Focus();
                return false;
            }

            if(!string.IsNullOrWhiteSpace(ScBrandCD.TxtCode.Text))
            {
                if (!ScBrandCD.IsExists(2))
                {
                    tzbl.ShowMessage("E101");
                    ScSupplier.SetFocus(1);
                    return false;
                }
                
            }

            if (!string.IsNullOrWhiteSpace(ScSegmentCD.TxtCode.Text))
            {
                if (!ScSegmentCD.IsExists(2))
                {
                    tzbl.ShowMessage("E101");
                    ScSupplier.SetFocus(1);
                    return false;
                }
            }

            if(!string.IsNullOrWhiteSpace(ScClient2.TxtCode.Text))
            {
                int result = ScClient1.TxtCode.Text.CompareTo(ScClient2.TxtCode.Text);
                if (result > 0)
                {
                    tzbl.ShowMessage("E104");
                    ScClient2.SetFocus(1);
                    return false;
                }
            }

            return true;
        }

        private void ScSegmentCD_Enter(object sender, EventArgs e)
        {
            ScSegmentCD.Value1 = "226";
        }


        private void ScSupplier_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(ScSupplier.TxtCode.Text))
            {
                ScSupplier.ChangeDate = bbl.GetDate();
                if (!ScSupplier.SelectData())
                {
                    bbl.ShowMessage("E101");
                    ScSupplier.SetFocus(1);
                }
            }
        }

        private void ScBrandCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(ScBrandCD.TxtCode.Text))
            {
                ScBrandCD.ChangeDate = bbl.GetDate();
                if (!ScBrandCD.SelectData())
                {
                    bbl.ShowMessage("E101");
                    ScBrandCD.SetFocus(1);
                }
            }
        }

        private void ScSegmentCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(ScSegmentCD.TxtCode.Text))
            {
                ScSegmentCD.ChangeDate = bbl.GetDate();
                if (!ScSegmentCD.SelectData())
                {
                    bbl.ShowMessage("E101");
                    ScSegmentCD.SetFocus(1);
                }
            }
        }

        private void ScExhibitionCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(ScExhibitionCD.TxtCode.Text))
            {
                ScExhibitionCD.ChangeDate = bbl.GetDate();
                if (!ScExhibitionCD.SelectData())
                {
                    bbl.ShowMessage("E101");
                    ScExhibitionCD.SetFocus(1);
                }
            }
        }

        private void ScClient2_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(ScClient2.TxtCode.Text))
            {
                int result = ScClient1.TxtCode.Text.CompareTo(ScClient2.TxtCode.Text);
                if(result > 0)
                {
                    bbl.ShowMessage("E104");
                    ScClient2.SetFocus(1);
                }
            }
        }
    }
}
