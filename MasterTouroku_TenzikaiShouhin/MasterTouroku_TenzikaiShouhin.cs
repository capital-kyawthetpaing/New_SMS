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
using GridBase;


namespace MasterTouroku_TenzikaiShouhin
{
    public partial class MasterTouroku_TenzikaiShouhin : FrmMainForm
    {
        MasterTouroku_TenzikaiShouhin_BL tbl;
        Base_BL bl;
        M_TenzikaiShouhin_Entity mt;

        public MasterTouroku_TenzikaiShouhin()
        {
            
            InitializeComponent();
            bl = new Base_BL();
            tbl = new MasterTouroku_TenzikaiShouhin_BL();
            mt = new M_TenzikaiShouhin_Entity();
        }

        private void MasterTouroku_TenzikaiShouhin_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_TenzikaiShouhin";
            StartProgram();
            string ymd = bl.GetDate();
            CB_Year.Bind(ymd);
            CB_Season.Bind(ymd);
            CB_copyseason.Bind(ymd);
            CB_Copyyear.Bind(ymd);

        }


        private void MasterTouroku_TenzikaiShouhin_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);

        }
        public override void FunctionProcess(int Index)
        {
            switch (Index + 1)
            {
                case 2:
                    ChangeMode(EOperationMode.INSERT);
                    break;
                case 3:
                    ChangeMode(EOperationMode.UPDATE);
                    break;
                case 4:
                    ChangeMode(EOperationMode.DELETE);
                    break;
                case 5:
                    ChangeMode(EOperationMode.SHOW);
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeMode(OperationMode);
                       // ginKou_CD.SetFocus(1);
                    }
                    break;
                case 11:
                   // F11();
                    break;
                case 12:
                   // F12();
                    break;
            }
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
            SC_copysegmet.Value1 = "226";
        }

        private void BT_Display_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(SC_CopyTenzikai.TxtCode.Text))
            {
                mt = GetData();
                
                DataTable dt = tbl.Mastertoroku_Tenzikaishouhin_Select(mt,"1");
            }
          
        }

        
        public M_TenzikaiShouhin_Entity GetData()
        {
            mt = new M_TenzikaiShouhin_Entity()
            {

                TenzikaiNameCopy = SC_Tenzikai.LabelText,
                VendorCDCopy = SC_Vendor.TxtCode.Text,
                BrandCDCopy = SC_Brand.TxtCode.Text,
                SegmentCDCopy = SC_Segment.TxtCode.Text,
                LastYearTermCopy = CB_Year.Text,
                LastSeasonCopy = CB_Season.Text,
                TenzikaiName = SC_Tenzikai.LabelText,
                VendorCD = SC_Vendor.TxtCode.Text,
                BranCDFrom = SC_Brand.TxtCode.Text,
                SegmentCDFrom = SC_Segment.TxtCode.Text,
                LastYearTerm = CB_Year.Text,
                LastSeason = CB_Season.Text,
            };

            return mt;
        }

        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {

                case EOperationMode.INSERT:
                    F9Visible = false;
                    Clear(PanelHeader);
                    //Clear(PanelDetail);
                    //EnablePanel(PanelHeader);
                    //ginKou_CD.SearchEnable = false;
                    //DisablePanel(PanelDetail);
                    //BtnF11Show.Enabled = F11Enable = true;
                    //copy_ginKou_CD.SearchEnable = true;
                    F9Visible = false;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    //Clear(PanelDetail);
                    //EnablePanel(PanelHeader);
                    //DisablePanel(panelCopy);
                    //Btn_F11.Enabled = true;
                    //BtnF11Show.Enabled = true;
                    //DisablePanel(PanelDetail);
                    //ginKou_CD.SearchEnable = true;
                    //copy_ginKou_CD.SearchEnable = false;
                    F9Visible = true;
                    break;
            }
            //ginKou_CD.SetFocus(1);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {



        }
    }
}
