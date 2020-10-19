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
using CKM_Controls;
using System.Diagnostics;
using System.IO;
using ClosedXML.Excel;

namespace TenzikaiShouhinJouhouShuturyoku
{
    public partial class frmTenzikaiShouhinJouhouShuturyoku : FrmMainForm
    {
        TenzikaiShouhinJouhouShuturyoku_BL tzkbl;
        M_TenzikaiShouhin_Entity mte;
        DataTable dt;
        public frmTenzikaiShouhinJouhouShuturyoku()
        {
            InitializeComponent();
            tzkbl = new TenzikaiShouhinJouhouShuturyoku_BL();
            dt = new DataTable();
        }
        private void frmTenzikaiShouhinJouhouShuturyoku_Load(object sender, EventArgs e)
        {
            InProgramID = "TenzikaiShouhinJouhouShuturyoku";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            BindCombo();
            Btn_F10.Text = "出力(F10)";
            F2Visible = false;
            F9Visible = true;
            SetRequiredField();
            scSupplierCD.SetFocus(1);
            ModeVisible = false;
        }
        public void BindCombo()
        {
            string ymd = bbl.GetDate();
            cbo_Year.Bind(ymd);
            cbo_Season.Bind(ymd);
        }
        private void SetRequiredField()
        {
            scSupplierCD.TxtCode.Require(true);
            cbo_Year.Require(true);
            cbo_Season.Require(true);
        }
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);
            switch (Index + 1)
            {
                case 6:
                    {
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;
                        Clear(panelDetail);
                        scSupplierCD.SetFocus(1);
                    }
                    break;
                case 10:
                    F10();
                    break;
            }
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] {scSupplierCD.TxtCode ,cbo_Year,cbo_Season }))
                return false;
            if (!string.IsNullOrEmpty(scBrandCDFrom.TxtCode.Text) && !string.IsNullOrEmpty(scBrandCDTo.TxtCode.Text))
            {
                if (string.Compare(scBrandCDFrom.TxtCode.Text, scBrandCDTo.TxtCode.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    scBrandCDTo.Focus();
                }
            }
            if (!string.IsNullOrEmpty(scSegmentCDFrom.TxtCode.Text) && !string.IsNullOrEmpty(scSegmentCDTo.TxtCode.Text))
            {
                if (string.Compare(scSegmentCDFrom.TxtCode.Text, scSegmentCDTo.TxtCode.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    scSegmentCDTo.Focus();
                }
            }
            return true;
        }
        private M_TenzikaiShouhin_Entity GetData()
        {
            mte = new M_TenzikaiShouhin_Entity()
            {
               VendorCD=scSupplierCD.TxtCode.Text,
               LastYearTerm=cbo_Year.SelectedValue.ToString(),
               LastSeason=cbo_Season.SelectedValue.ToString(),
               BranCDFrom=scBrandCDFrom.TxtCode.Text,
               BrandCDTo=scBrandCDTo.TxtCode.Text,
               SegmentCDFrom=scSegmentCDFrom.TxtCode.Text,
               SegmentCDTo=scSegmentCDTo.TxtCode.Text,
               TenzikaiName= ckM_SearchControl1.TxtCode.Text
            };
            return mte;
        }
        private void F10()
        {
            if (ErrorCheck())
            {
                mte = new M_TenzikaiShouhin_Entity();
                mte = GetData();
                DataTable dt = tzkbl.Rpc_TenzikaiShouhinJouhouShuturyoku(mte);
                if (dt.Rows.Count > 0)
                {
                    string folderPath = "C:\\SES\\";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    SaveFileDialog savedialog = new SaveFileDialog();
                    savedialog.Filter = "Excel Files|*.xlsx;";
                    savedialog.Title = "Save";
                    savedialog.FileName = "展示会商品情報出力";
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
                            worksheet.Name = "worksheet";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, "worksheet");
                                wb.Worksheet("worksheet").Tables.FirstOrDefault().ShowAutoFilter = false;
                                wb.SaveAs(savedialog.FileName);
                                tzkbl.ShowMessage("Q201", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            }
                            Process.Start(Path.GetDirectoryName(savedialog.FileName));
                        }
                    }
                }
                else
                {
                    tzkbl.ShowMessage("E128");
                    scSupplierCD.SetFocus(1);
                }
            }
        }
        private void scSupplierCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSupplierCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSupplierCD.TxtCode.Text))
                {
                    if (!scSupplierCD.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scSupplierCD.SetFocus(1);
                    }
                }
            }
        }
       
        private void scSegmentCDFrom_Enter(object sender, EventArgs e)
        {
            scSegmentCDFrom.Value1 = "226";
        }
        private void scSegmentCDTo_Enter(object sender, EventArgs e)
        {
            scSegmentCDTo.Value1 = "226";
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void frmTenzikaiShouhinJouhouShuturyoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void scBrandCDTo_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(scBrandCDFrom.TxtCode.Text) && !string.IsNullOrEmpty(scBrandCDTo.TxtCode.Text))
            {
                if (string.Compare(scBrandCDFrom.TxtCode.Text, scBrandCDTo.TxtCode.Text) == 1)
                {
                    bbl.ShowMessage("E106");
                    scBrandCDTo.Focus();
                }
            }
        }

        private void scSegmentCDTo_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(scSegmentCDFrom.TxtCode.Text) && !string.IsNullOrEmpty(scSegmentCDTo.TxtCode.Text))
            {
                if (string.Compare(scSegmentCDFrom.TxtCode.Text, scSegmentCDTo.TxtCode.Text) == 1)
                {
                    bbl.ShowMessage("E106");
                    scSegmentCDTo.Focus();
                }
            }
        }
    }
}
