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
using CKM_Controls;

namespace TairyuZaikoHyou
{
    public partial class FrmTairyuZaikoHyou : FrmMainForm
    {
        TairyuZaikoHyou_BL tzkbl = new TairyuZaikoHyou_BL();
        M_Vendor_Entity mve = new M_Vendor_Entity();
        M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        D_Stock_Entity dse = new D_Stock_Entity();
        M_StoreClose_Entity msce = new M_StoreClose_Entity();
        M_SKU_Entity mskue = new M_SKU_Entity();
        M_SKUInfo_Entity info = new M_SKUInfo_Entity();
        M_SKUTag_Entity mtage = new M_SKUTag_Entity();

        public FrmTairyuZaikoHyou()
        {
            InitializeComponent();
        }

        private void FrmTairyuZaikoHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "TairyuZaikoHyou";
            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            F7Visible = false;
            F8Visible = false;
            F10Visible = false;
            F11Visible = false;

            StartProgram();

            BindCombo();
            SetRequireField();
            Btn_F12.Text = "出力(F12)";
            ModeVisible = false;

        }
        public void BindCombo()
        {
            string ymd = bbl.GetDate();
            cboWarehouse.Bind(string.Empty,"");
            CboYear.Bind(ymd);
            cboSeason.Bind(ymd);
            cboReservation.Bind(ymd);
            cboNotices.Bind(ymd);
            cboOrder.Bind(ymd);
            cboPostage.Bind(ymd);
            cboTag1.Bind(ymd);
            cboTag2.Bind(ymd);
            cboTag3.Bind(ymd);
            cboTag4.Bind(ymd);
            cboTag5.Bind(ymd);

        }

        public void SetRequireField()
        {
            txtTargetDays.Require(true);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        public override void FunctionProcess(int Index)
        {
            switch (Index + 1)
            {
                case 2:
                case 3:
                case 4:
                    break;
                case 5:
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear(panelDetail);
                        txtTargetDays.Focus();
                    }
                    break;
                case 10:                 
                    break;
                case 11:               
                    break;
                case 12:
                    F12();
                    break;
            }
        }

        public bool ErrorCheck()
        {
           if(String.IsNullOrWhiteSpace(txtTargetDays.Text))
            {
                tzkbl.ShowMessage("E102");
                txtTargetDays.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(Sc_Maker.TxtCode.Text))
            {
                if (!Sc_Maker.IsExists(2))
                {
                    tzkbl.ShowMessage("E101");
                    Sc_Maker.SetFocus(1);
                    return false;
                }
            }

            if(!string.IsNullOrEmpty(Sc_Sports.TxtCode.Text))
            {
                mmpe.Key = Sc_Sports.TxtCode.Text;
                mmpe.ID = "202";
                DataTable dtCompetition = new DataTable();
                dtCompetition = tzkbl.M_Multiporpose_CharSelect(mmpe);
                if (dtCompetition.Rows.Count == 0)
                {
                    tzkbl.ShowMessage("E101");
                    Sc_Sports.SetFocus(1);
                    return false;
                }
                else
                {
                    Sc_Sports.LabelText = dtCompetition.Rows[0]["Char1"].ToString();
                }
            }

            if(rdoItem.Checked)
            {
                if(string.IsNullOrWhiteSpace(txtItem.Text))
                {
                    tzkbl.ShowMessage("E102");
                    txtItem.Focus();
                    return false;
                }
                else
                {

                }
            }

            if(rdoProductCD.Checked)
            {
                if(string.IsNullOrWhiteSpace(txtManufactureCD.Text))
                {
                    tzkbl.ShowMessage("E102");
                    txtManufactureCD.Focus();
                    return false;
                }
            }

            return true;
        }
        public void F12()
        {
            //if (PrintMode != EPrintMode.DIRECT)
            //    return;

            if (ErrorCheck())
            {
                string[] strlist = txtRemarks.Text.Split(',');
                dse = new D_Stock_Entity
                {
                    AdminNO = txtTargetDays.Text,
                    SoukoCD = cboWarehouse.SelectedValue.ToString(),
                    RackNOFrom = txtStorageFrom.Text,
                    RackNOTo = txtStorageTo.Text,
                    Keyword1 = (strlist.Length > 0) ? strlist[0].ToString() : "",
                    Keyword2 = (strlist.Length > 1) ? strlist[1].ToString() : "",
                    Keyword3 = (strlist.Length > 2) ? strlist[2].ToString() : "",
                };

                mskue = new M_SKU_Entity
                {
                    MainVendorCD = Sc_Maker.TxtCode.Text,
                    BrandCD = Sc_Brand.TxtCode.Text,
                    SKUName = txtProductName.Text,
                    JanCD = txtJANCD.Text,
                    SKUCD = txtSKUCD.Text,
                    ITemCD = txtItem.Text,
                    MakerItem = txtManufactureCD.Text,
                    SportsCD = Sc_Sports.TxtCode.Text,
                    ReserveCD = cboReservation.SelectedValue.ToString(),
                    NoticesCD = cboNotices.SelectedValue.ToString(),
                    PostageCD = cboPostage.SelectedValue.ToString(),
                    OrderAttentionCD = cboOrder.ToString()
                };

                info = new M_SKUInfo_Entity
                {
                    YearTerm = CboYear.SelectedValue.ToString(),
                    Season = cboSeason.SelectedValue.ToString(),
                    CatalogNO = txtCatalog.Text,
                    InstructionsNO = txtInstructionNo.Text,
                };

                mtage = new M_SKUTag_Entity
                {
                    TagName1 = cboTag1.SelectedValue.ToString(),
                    TagName2 = cboTag2.SelectedValue.ToString(),
                    TagName3 = cboTag3.SelectedValue.ToString(),
                    TagName4 = cboTag4.SelectedValue.ToString(),
                    TagName5 = cboTag5.SelectedValue.ToString()
                };

                DataTable dtSelect = new DataTable();
                dtSelect = tzkbl.D_StockSelectForTairyuzaikohyo(dse, mskue, info, mtage);
                if (dtSelect.Rows.Count > 0)
                {
                    //CheckBeforeExport();
                    try
                    {

                    }
                    finally
                    {
                        //画面はそのまま
                        txtTargetDays.Focus();
                    }
                }
            }
        }

        private void CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();

            if (tzkbl.M_StoreClose_Check(msce, "2").Rows.Count > 0)
            {
                string ProgramID = "GetsujiZaikoKeisanSyori";
                RunConsole(ProgramID, msce.FiscalYYYYMM);
            }
        }
        private void RunConsole(string programID, string YYYYMM)
        {
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "1";
            string cmdLine = " " + InOperatorCD + " " + Login_BL.GetHostName() + " " + StoreCD + " " + " " + Mode + " " + YYYYMM;//parameter
            System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
        }

        private M_StoreClose_Entity GetStoreClose_Data()
        {
            string m = DateTime.Now.Month.ToString();
            if (m.Length == 1)
            {
                m = 0 + DateTime.Now.Month.ToString();
            }
            string y = DateTime.Now.Year.ToString();
            //txtTargetDateFrom.Text = a.ToString().Substring(0, 7);
           string day  = y + "/" + m;
            msce = new M_StoreClose_Entity()
            {
                StoreCD = StoreAuthorizationsCD,
                FiscalYYYYMM = day.Replace("/",""),
            };
            return msce;
        }

        private void FrmTairyuZaikoHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void Sc_Maker_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            Sc_Maker.ChangeDate = bbl.GetDate();
            if (!string.IsNullOrEmpty(Sc_Maker.TxtCode.Text))
            {
                if (Sc_Maker.SelectData())
                {
                    Sc_Maker.Value1 = Sc_Maker.TxtCode.Text;
                    Sc_Maker.Value2 = Sc_Maker.LabelText;
                }
                else
                {
                    bbl.ShowMessage("E101");
                    Sc_Maker.SetFocus(1);
                }
            }

        }

        private void Sc_Maker_Enter(object sender, EventArgs e)
        {
            Sc_Maker.Value1 = "1";//仕入先区分：1
        }
        private void Sc_Competition_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(Sc_Sports.TxtCode.Text))
                {
                    mmpe.Key = Sc_Sports.TxtCode.Text;
                    mmpe.ID = "202";
                    DataTable dtCompetition = new DataTable();
                    dtCompetition = tzkbl.M_Multiporpose_CharSelect(mmpe);
                    if (dtCompetition.Rows.Count == 0)
                    {
                        tzkbl.ShowMessage("E101");
                        Sc_Sports.SetFocus(1);
                    }
                    else
                    {
                        Sc_Sports.LabelText = dtCompetition.Rows[0]["Char1"].ToString();
                    }
                }
            }
        }

        private void Sc_Sports_Enter(object sender, EventArgs e)
        {
            Sc_Sports.ChangeDate = bbl.GetDate();
            Sc_Sports.Value1 = "202";
        }

        private void chkPrint_CheckedChanged(object sender, EventArgs e)
        {
            if(chkPrint.Checked == true)
            {
                rdoItem.Checked = true;
            }
            else
            {
                rdoItem.Checked = false;
                rdoProductCD.Checked = false;
            }
        }
    }
}
