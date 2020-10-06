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

namespace TenzikaiHacchuuJouhouShuturyoku
{
    public partial class FrmTenzikaiHacchuuJouhouShuturyoku : FrmMainForm
    {
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
                if (!ScSupplier.SelectData())
                {
                    //mtsbl.ShowMessage("E101");
                    ScSupplier.SetFocus(1);
                    return false;
                }
            }

            if(cboYear.SelectedValue.ToString() == "-1")
            {
                //102
                cboYear.Focus();
                return false;
            }

            if (cboSeason.SelectedValue.ToString() == "-1")
            {
                //102
                cboSeason.Focus();
                return false;
            }

            if(!string.IsNullOrWhiteSpace(ScBrandCD.TxtCode.Text))
            {
                if (!ScBrandCD.SelectData())
                {
                    //mtsbl.ShowMessage("E101");
                    ScSupplier.SetFocus(1);
                    return false;
                }
            }

            return true;
        }

    }
}
