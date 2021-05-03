using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using Base.Client;
using System.IO;
using ClosedXML.Excel;
using System.Diagnostics;

namespace MasterShutsuryoku_SKUPrice
{
    public partial class MasterShutsuryoku_SKUPrice : FrmMainForm
    {
        Base_BL bbl;
        M_SKU_Entity msku;
        SKU_BL mbl;
        int type = 0;
        int checkflg = 0;
        int chkUnapprove = 0;

        public MasterShutsuryoku_SKUPrice()
        {
            InitializeComponent();
            bbl = new Base_BL();
            msku = new M_SKU_Entity();
            mbl = new SKU_BL();
        }

        private void MasterShutsuryoku_SKUPrice_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterShutsuryoku_SKUPrice";
            StartProgram();
            BindCombo();
            ModeVisible = false;
            LB_ChangeDate.Text = bbl.GetDate();
            SC_Vendor.SetFocus(1);
            Btn_F12.Text = "Excel出力(F12)";
        }

        private void BindCombo()
        {
            string ymd = bbl.GetDate();
            CB_Year.Bind(ymd);
            CB_Season.Bind(ymd);
            CB_ReserveCD.Bind(ymd);
            CB_NoticesCD.Bind(ymd);
            CB_PostageCD.Bind(ymd);
            CB_OrderAttention.Bind(ymd);
            CB_Tag1.Bind(ymd);
            CB_Tag2.Bind(ymd);
            CB_Tag3.Bind(ymd);
            CB_Tag4.Bind(ymd);
            CB_Tag5.Bind(ymd);
        }


        public override void FunctionProcess(int index)
        {

            switch (index + 1)
            {

                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        CleanData();
                    }
                    break;
                case 10:

                case 12:
                    OutputExecel();
                    break;
            }
        }

        private void CleanData()
        {
            Clear(panel1);
            SC_Vendor.SetFocus(1);
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void SC_Vendor_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SC_Vendor.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(SC_Vendor.TxtCode.Text))
                {
                    if (!SC_Vendor.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_Vendor.SetFocus(1);
                        
                    }
                }
            }
        }

        private void SC_makervendor_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SC_makervendor.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(SC_makervendor.TxtCode.Text))
                {
                    if (!SC_makervendor.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_makervendor.SetFocus(1);
                    }
                }
            }
        }

        private void SC_Brand_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SC_Brand.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(SC_Brand.TxtCode.Text))
                {
                    if (!SC_Brand.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_Brand.SetFocus(1);
                    }
                }
            }

        }

        private void SC_JANCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(SC_JANCD.TxtCode.Text))
            {
                if (!SC_JANCD.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_JANCD.Focus();
                }
            }
        }

        private void SC_SKUCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(SC_SKUCD.TxtCode.Text))
            {
                if (!SC_SKUCD.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_SKUCD.Focus();
                }
            }
        }

        private void SC_char1_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SC_char1.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(SC_char1.TxtCode.Text))
                {
                    if (!SC_char1.SelectData())
                    {
                        SC_char1.Value1 = "203";
                        bbl.ShowMessage("E101");
                        SC_char1.SetFocus(1);
                    }
                }
            }
        }

        private void SC_SportsCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SC_SportsCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(SC_SportsCD.TxtCode.Text))
                {
                    if (!SC_SportsCD.SelectData())
                    {
                        SC_SportsCD.Value1 = "202";
                        bbl.ShowMessage("E101");
                        SC_SportsCD.SetFocus(1);
                    }
                }
            }
        }

        private void MasterShutsuryoku_SKUPrice_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void CK_suru_CheckedChanged(object sender, EventArgs e)
        {
            if (CK_suru.Checked == false)
            {
                RB_Item.Checked = false;
                RB_makeritem.Checked = false;
            }
            else
            {
                RB_Item.Checked = true;
            }
        }

        private void SC_Vendor_Enter(object sender, EventArgs e)
        {
            SC_Vendor.ChangeDate = bbl.GetDate();
            SC_Vendor.Value1 = "1";
        }

        private void SC_makervendor_Enter(object sender, EventArgs e)
        {
            SC_makervendor.ChangeDate = bbl.GetDate();
            SC_makervendor.Value1 = "1";
        }

        private void TB_InsertDateT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_InsertDateF.Text) && !String.IsNullOrEmpty(TB_InsertDateT.Text))
                {
                    if (Convert.ToDateTime(TB_InsertDateF.Text) > Convert.ToDateTime(TB_InsertDateT.Text))
                    {
                        bbl.ShowMessage("E104");
                        TB_InsertDateF.Focus();
                    }
                }
            }
        }

        private void TB_UpdateDateT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_UpdateDateF.Text) && !String.IsNullOrEmpty(TB_UpdateDateT.Text))
                {
                    if (Convert.ToDateTime(TB_UpdateDateF.Text) > Convert.ToDateTime(TB_UpdateDateT.Text))
                    {
                        bbl.ShowMessage("E104");
                        TB_UpdateDateF.Focus();
                    }
                }
            }
        }

        private void TB_ApprovalDateT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(TB_ApprovalDateF.Text) && !String.IsNullOrEmpty(TB_ApprovalDateT.Text))
                {
                    if (Convert.ToDateTime(TB_ApprovalDateF.Text) > Convert.ToDateTime(TB_ApprovalDateT.Text))
                    {
                        bbl.ShowMessage("E104");
                        TB_ApprovalDateF.Focus();
                    }
                }
            }
        }

        private void SC_char1_Enter(object sender, EventArgs e)
        {
            SC_char1.ChangeDate = bbl.GetDate();
            SC_char1.Value1 = "203";
        }

        private void SC_SportsCD_Enter(object sender, EventArgs e)
        {
            SC_SportsCD.ChangeDate = bbl.GetDate();
            SC_SportsCD.Value1 = "202";
        }

        private void OutputExecel()
        {

            if (ErrorCheck())
            {
                if (bbl.ShowMessage("Q203") == DialogResult.Yes)
                {
                    type = (CK_suru.Checked && RB_Item.Checked) ? 1 : (CK_suru.Checked == true && RB_Item.Checked == true) ? 2 : 3;

                    chkUnapprove = CK_UnApprove.Checked ? 1 : 0;
                    //checkflg = RB_all.Checked ? 1 : RB_BaseInfo.Checked ? 2 : RB_attributeinfo.Checked ? 3 : RB_priceinfo.Checked ? 4 : RB_Catloginfo.Checked ? 5 : RB_tagInfo.Checked ? 6 : RB_JanCD.Checked ? 7 : RB_SizeURL.Checked ? 8 : 0;

                    msku = GetData();
                    DataTable dt = new DataTable();
                    dt = mbl.M_SKUPrice_Export(msku, chkUnapprove, type);
                    if (dt.Rows.Count > 0)
                    {
                        

                        DataTable dtnew = dt;
                        string folderPath = "C:\\SMS\\MasterShutsuryoku_SKUPrice\\";
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        SaveFileDialog savedialog = new SaveFileDialog();
                        savedialog.Filter = "Excel Files|*.xlsx;";
                        savedialog.Title = "Save";
                        savedialog.FileName = "MasterShutsuryoku_SKUPrice" + " " + DateTime.Now.ToString(" yyyyMMdd_HHmmss ");
                        savedialog.InitialDirectory = folderPath;

                        savedialog.RestoreDirectory = true;

                        if (savedialog.ShowDialog() == DialogResult.OK)
                        {
                            if (Path.GetExtension(savedialog.FileName).Contains(".xlsx"))
                            {
                                Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
                                Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                                worksheet = workbook.ActiveSheet;
                                worksheet.Name = "Sheet1";

                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    wb.Worksheets.Add(dt, "Sheet1");
                                    wb.SaveAs(savedialog.FileName);
                                    bbl.ShowMessage("I203", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                }

                                Process.Start(Path.GetDirectoryName(savedialog.FileName));
                            }
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E128");
                    }
                }
            }
        }

        private M_SKU_Entity GetData()
        {

            msku = new M_SKU_Entity()
            {
                ChangeDate = LB_ChangeDate.Text,
                MainVendorCD = SC_Vendor.TxtCode.Text,
                MakerVendorCD = SC_makervendor.TxtCode.Text,
                BrandCD = SC_Brand.TxtCode.Text,
                SKUName = TB_Shouhin.Text,
                JanCD = SC_JANCD.TxtCode.Text,
                SKUCD = SC_SKUCD.TxtCode.Text,
                CommentInStore = TB_CommentInStore.Text,
                LastYearTerm = CB_Year.Text,
                LastSeason = CB_Season.Text,
                LastCatalogNO = TB_Catalog.Text,
                ReserveCD = CB_ReserveCD.Text,
                NoticesCD = CB_NoticesCD.Text,
                OrderAttentionCD = CB_OrderAttention.Text,
                PostageCD = CB_PostageCD.Text,
                SportsCD = SC_SportsCD.TxtCode.Text,
                TagName1 = CB_Tag1.Text,
                TagName2 = CB_Tag2.Text,
                TagName3 = CB_Tag3.Text,
                TagName4 = CB_Tag4.Text,
                TagName5 = CB_Tag5.Text,
                LastInstructionsNO = TB_InstructionNo.Text,
                ITemCD = TB_Item.Text,
                MakerItem = TB_MakerItem.Text,
                InputDateFrom = TB_InsertDateF.Text,
                InputDateTo = TB_InsertDateT.Text,
                UpdateDateFrom = TB_UpdateDateF.Text,
                UpdateDateTo = TB_UpdateDateT.Text,
                ApprovalDateFrom = TB_ApprovalDateF.Text,
                ApprovalDateTo = TB_ApprovalDateT.Text,
                Operator = InOperatorCD,
                PC = InPcID,
                ProgramID = InProgramID,
                ProcessMode = ModeText,
                Key = StoreCD + " " + LB_ChangeDate.Text,
            };
            return msku;
        }

        private bool ErrorCheck()
        {
            if (!String.IsNullOrEmpty(SC_Vendor.TxtCode.Text))
            {
                if (!SC_Vendor.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_Vendor.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(SC_makervendor.TxtCode.Text))
            {
                if (!SC_makervendor.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_makervendor.SetFocus(1);
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
            if (!String.IsNullOrEmpty(SC_JANCD.TxtCode.Text))
            {
                if (!SC_JANCD.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_JANCD.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(SC_SKUCD.TxtCode.Text))
            {
                if (!SC_SKUCD.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_SKUCD.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(SC_SportsCD.TxtCode.Text))
            {
                if (!SC_SportsCD.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_SportsCD.SetFocus(1);
                    return false;
                }
            }
            if (!String.IsNullOrEmpty(SC_char1.TxtCode.Text))
            {
                if (!SC_char1.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_char1.SetFocus(1);
                    return false;
                }
            }
            if (!TB_InsertDateF.DateCheck())
            {
                TB_InsertDateF.Focus();
                return false;

            }

            if (!TB_InsertDateT.DateCheck())
            {
                TB_InsertDateT.Focus();
                return false;
            }
            if (!String.IsNullOrEmpty(TB_InsertDateF.Text) && !String.IsNullOrEmpty(TB_InsertDateT.Text))
            {
                if (Convert.ToDateTime(TB_InsertDateF.Text) > Convert.ToDateTime(TB_InsertDateT.Text))
                {
                    bbl.ShowMessage("E104");
                    TB_InsertDateF.Focus();
                    return false;
                }
            }

            if (!TB_UpdateDateF.DateCheck())
            {
                TB_UpdateDateF.Focus();
                return false;
            }
            if (!TB_UpdateDateT.DateCheck())
            {
                TB_UpdateDateT.Focus();
                return false;
            }

            if (!String.IsNullOrEmpty(TB_UpdateDateF.Text) && !String.IsNullOrEmpty(TB_UpdateDateT.Text))
            {
                if (Convert.ToDateTime(TB_UpdateDateF.Text) > Convert.ToDateTime(TB_UpdateDateT.Text))
                {
                    bbl.ShowMessage("E104");
                    TB_UpdateDateF.Focus();
                    return false;
                }
            }

            if (!TB_ApprovalDateF.DateCheck())
            {
                TB_ApprovalDateF.Focus();
                return false;
            }
            if (!TB_ApprovalDateT.DateCheck())
            {
                TB_ApprovalDateT.Focus();
                return false;

            }

            if (!String.IsNullOrEmpty(TB_ApprovalDateF.Text) && !String.IsNullOrEmpty(TB_ApprovalDateT.Text))
            {
                if (Convert.ToDateTime(TB_ApprovalDateF.Text) > Convert.ToDateTime(TB_ApprovalDateT.Text))
                {
                    bbl.ShowMessage("E104");
                    TB_ApprovalDateF.Focus();
                    return false;
                }
            }

            return true;

        }

    }
}
