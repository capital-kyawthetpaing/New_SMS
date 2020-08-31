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

namespace MasterTouroku_TenzikaiHanbaiTankaKakeritu
{
    public partial class FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu : FrmMainForm
    {
        public FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu()
        {
            InitializeComponent();
        }
        private void FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu_Load(object sender, EventArgs e)
        {

            InProgramID = "MasterTouroku_TenzikaiHanbaiTankaKakeritu";
            StartProgram();
            //if (ErrorCheck())
            //{

            //}
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { SC_Tanka.TxtCode })) //Step1
                return false;

            if (!SC_Tanka.IsExists(2))
            {
                bbl.ShowMessage("E101");
                SC_Tanka.SetFocus(1);
                return false;
            }

            if (!SC_Brand.IsExists(2))
            {
                bbl.ShowMessage("E101");
                SC_Brand.SetFocus(1);
                return false;
            }

            if (!Sc_Segment.IsExists(2))
            {
                bbl.ShowMessage("E101");
                Sc_Segment.SetFocus(1);
                return false;
            }
            //Console.WriteLine(header.Trim(new Char[] { ' ', '*', '.' }));

            if (!String.IsNullOrEmpty(TB_PriceOutTaxF.Text))
            {
                string strF = TB_PriceOutTaxF.Text.Trim(new Char[] { ',' });
            }
            return true;

        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void SC_Tanka_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if(ErrorCheck())
                {

                }
                if(!String.IsNullOrEmpty(SC_Tanka.TxtCode.Text))
                {
                    SC_Tanka.ChangeDate = bbl.GetDate();
                    if(!SC_Tanka.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_Tanka.SetFocus(1);
                    }
                }

            }
        }

        private void SC_Brand_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if(!String.IsNullOrEmpty(SC_Brand.TxtCode.Text))
                {
                    bbl.ShowMessage("E101");
                    SC_Brand.SetFocus(1);
                }
            }
        }

        private void Sc_Segment_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(!String.IsNullOrEmpty(Sc_Segment.TxtCode.Text))
                {
                    bbl.ShowMessage("E101");
                    Sc_Segment.SetFocus(1);
                }
            }

        }

       
    }
}
