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


namespace MasterTouroku_TenzikaiShouhin
{
    public partial class MasterTouroku_TenzikaiShouhin : FrmMainForm
    {


        Base_BL bl;

        public MasterTouroku_TenzikaiShouhin()
        {
            
            InitializeComponent();
            bl = new Base_BL();
        }

        private void MasterTouroku_TenzikaiShouhin_Load(object sender, EventArgs e)
        {
            string ymd = bl.GetDate();
            //CB_Year.Bind(ymd);
            //CB_Season.Bind(ymd);
            //CB_copyseason.Bind(ymd);
            //CB_Copyyear.Bind(ymd);

        }


        private void MasterTouroku_TenzikaiShouhin_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);

        }
        public override void FunctionProcess(int Index)
        {
           


        }
        protected override void EndSec()
        {
            this.Close();
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

        private void SC_Segment_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(SC_Segment.TxtCode.Text))
            {
                SC_Segment.ChangeDate = bbl.GetDate();
                if (!SC_Segment.SelectData())
                {
                    bbl.ShowMessage("E101");
                    SC_Segment.SetFocus(1);
                }
            }
        }

        private void SC_Tenzikai_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(SC_Tenzikai.TxtCode.Text))
            {
                SC_Tenzikai.ChangeDate = bbl.GetDate();
                if (!SC_Tenzikai.SelectData())
                {
                    bbl.ShowMessage("E101");
                    SC_Tenzikai.SetFocus(1);
                }
            }
        }

        private void SC_Vendor_Enter(object sender, EventArgs e)
        {
            SC_Vendor.Value1 = "1";
        }

        private void SC_Segment_Enter(object sender, EventArgs e)
        {
            SC_Segment.Value1 = "226";
        }

        private void SC_CopyTenzikai_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(SC_CopyTenzikai.TxtCode.Text))
            {
                SC_CopyTenzikai.ChangeDate = bbl.GetDate();
                if (!SC_CopyTenzikai.SelectData())
                {
                    bbl.ShowMessage("E101");
                    SC_CopyTenzikai.SetFocus(1);
                }
            }
        }

        private void SC_CopyVendor_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(SC_CopyVendor.TxtCode.Text))
            {
                SC_CopyVendor.ChangeDate = bbl.GetDate();
                if (!SC_CopyVendor.SelectData())
                {
                    bbl.ShowMessage("E101");
                    SC_CopyVendor.SetFocus(1);
                }
            }
        }

        private void SC_copybrand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(SC_copybrand.TxtCode.Text))
            {
                SC_copybrand.ChangeDate = bbl.GetDate();
                if (!SC_copybrand.SelectData())
                {
                    bbl.ShowMessage("E101");
                    SC_copybrand.SetFocus(1);
                }
            }
        }

        private void SC_copoysegmet_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(SC_Tenzikai.TxtCode.Text))
            {
                SC_Tenzikai.ChangeDate = bbl.GetDate();
                if (!SC_Tenzikai.SelectData())
                {
                    bbl.ShowMessage("E101");
                    SC_Tenzikai.SetFocus(1);
                }
            }
        }

        private void SC_CopyVendor_Enter(object sender, EventArgs e)
        {
            SC_CopyVendor.Value1 = "1";
        }

        private void SC_copoysegmet_Enter(object sender, EventArgs e)
        {
            SC_copoysegmet.Value1 = "226";
        }
    }
}
