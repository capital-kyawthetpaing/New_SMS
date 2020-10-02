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
        private const string ProID = "MasterTouroku_TenzikaiShouhin";
        private const string ProNm = "展示会受注登録";
        private const short mc_L_END = 3; // ロック用
        private const string TempoNouhinsyo = "TenzikaiJuchuuTourou.exe";
        Base_BL bl;
        M_TenzikaiShouhin_Entity mt;
        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
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

        private void BindCombo_Details()
        {
          
        }
        private void MasterTouroku_TenzikaiShouhin_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);

        }

      

        private void InitialControlArray()
        {

            detailControls = new Control[] { SC_Tenzikai.TxtCode, SC_Brand.TxtCode,SC_Segment.TxtCode,SC_Vendor.TxtCode,SC_CopyTenzikai.TxtCode,SC_CopyVendor.TxtCode,
                SC_copybrand.TxtCode,SC_copysegmet.TxtCode,CB_Season,CB_Year,CB_Copyyear,CB_copyseason,TB_InsertDateTimeF};
            detailLabels = new Control[] { };
            searchButtons = new Control[] { SC_Tenzikai.BtnSearch, SC_Vendor.BtnSearch, SC_Brand.BtnSearch, SC_Segment.BtnSearch, SC_CopyTenzikai.BtnSearch ,
                                            SC_CopyVendor.BtnSearch,SC_copybrand.BtnSearch,SC_copysegmet.BtnSearch };
                                         
            //  sc_Tenji.KeyDown += TenzikaiJuchuuTourou_KeyDown;
        }

    }
}
