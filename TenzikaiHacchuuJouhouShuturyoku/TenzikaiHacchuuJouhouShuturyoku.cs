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
using System.IO;
using ClosedXML.Excel;
using System.Diagnostics;

namespace TenzikaiHacchuuJouhouShuturyoku

{
    public partial class FrmTenzikaiHacchuuJouhouShuturyoku : FrmMainForm
    {
        TenzikaiHacchuuJouhouShuturyoku_BL tzbl = new TenzikaiHacchuuJouhouShuturyoku_BL();
        D_TenzikaiJuchuu_Entity dtje = new D_TenzikaiJuchuu_Entity();
        int chk = 0;string filename = string.Empty;

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
            F11Visible = false;

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
            cboYear.Require(true);
            cboSeason.Require(true);
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
            Clear(panel1);
            ScSupplier.Clear();
            //cboYear.SelectedValue.Equals("-1");
            //cboSeason.SelectedValue.Equals("-1");
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
            if(ErrorCheck())
            {
                if (bbl.ShowMessage("Q205") == DialogResult.Yes)
                {
                    dtje = new D_TenzikaiJuchuu_Entity
                    {
                        VendorCD = ScSupplier.TxtCode.Text,
                        LastYearTerm = cboYear.SelectedValue.ToString(),
                        season = cboSeason.SelectedValue.ToString(),
                        CustomerCDFrom = ScClient1.TxtCode.Text,
                        CustomerCDTo = ScClient2.TxtCode.Text,
                        BrandCD = ScBrandCD.TxtCode.Text,
                        SegmentCD = ScSegmentCD.TxtCode.Text,
                        ExhibitionName = ScExhibitionCD.TxtCode.Text,
                        ProgramID = InProgramID,
                        Operator = InOperatorCD,
                        PC = InPcID,
                        ProcessMode = string.Empty,
                        Key = string.Empty
                    };
                    if (rdoCustomer.Checked == true)
                    {
                        chk = 1;
                    }
                    else if (rdoProduct.Checked == true)
                    {
                        chk = 2;
                    }
                    DataTable dttenzi = new DataTable();                   
                    dttenzi = tzbl.D_TenzikaiJuchuu_SelectForExcel(dtje,chk);
                    if(dttenzi.Rows.Count > 0)
                    {
                        filename = dttenzi.Rows[0]["TenzikaiName"].ToString();
                        if (dttenzi.Columns.Contains("TenzikaiName"))
                        {
                            dttenzi.Columns.Remove("TenzikaiName");
                        }
                        string folderPath = "C:\\Excel\\";
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        SaveFileDialog savedialog = new SaveFileDialog();
                        savedialog.Filter = "Excel Files|*.xlsx;";
                        savedialog.Title = "Save";
                        savedialog.FileName = filename;
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
                                    wb.Worksheets.Add(dttenzi, "worksheet");
                                    wb.Worksheet("worksheet").Tables.FirstOrDefault().ShowAutoFilter = false;
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
                        //ScSupplier.SetFocus(1);
                        PreviousCtrl.Focus();
                    }
                }
            }
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

            if (!RequireCheck(new Control[] { cboYear, cboSeason }))
                return false;

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
                    tzbl.ShowMessage("E106");
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
                else
                {
                    cboYear.Focus();
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
            //if (!String.IsNullOrEmpty(ScExhibitionCD.TxtCode.Text))
            //{
            //    ScExhibitionCD.ChangeDate = bbl.GetDate();
            //    if (!ScExhibitionCD.SelectData())
            //    {
            //        bbl.ShowMessage("E101");
            //        ScExhibitionCD.SetFocus(1);
            //    }
            //}
        }

        private void ScClient2_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(ScClient2.TxtCode.Text))
            {
                int result = ScClient1.TxtCode.Text.CompareTo(ScClient2.TxtCode.Text);
                if(result > 0)
                {
                    bbl.ShowMessage("E106");
                    ScClient2.SetFocus(1);
                }
            }
        }

        private void FrmTenzikaiHacchuuJouhouShuturyoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

    }
}
