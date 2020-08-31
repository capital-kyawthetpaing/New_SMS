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
using Search;
using System.Diagnostics;
using System.IO;
using ClosedXML.Excel;

namespace TenzikaiShouhinJouhouShuturyoku
{
    public partial class frmTenzikaiShouhinJouhouShuturyoku : FrmMainForm
    {
        public frmTenzikaiShouhinJouhouShuturyoku()
        {
            InitializeComponent();
        }

        private void frmTenzikaiShouhinJouhouShuturyoku_Load(object sender, EventArgs e)
        {
            InProgramID = "TenzikaiShouhinJouhouShuturyoku";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            BindCombo();
            Btn_F10.Text = "出力(F10)";
            SetRequiredField();
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
                    }
                    break;
                case 10:
                    F10();
                    break;
            }
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { cbo_Year,cbo_Season }))
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

        private void F10()
        {
            if (ErrorCheck())
            {

            }
        }
        private void scBrandCDFrom_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scBrandCDFrom.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scBrandCDFrom.TxtCode.Text))
                {
                    if (!scBrandCDFrom.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scBrandCDFrom.SetFocus(1);
                    }
                }
            }
        }

        private void scBrandCDTo_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scBrandCDTo.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scBrandCDTo.TxtCode.Text))
                {
                    if (!scBrandCDTo.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scBrandCDTo.SetFocus(1);
                    }
                }
            }
        }

        private void scSegmentCDFrom_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSegmentCDFrom.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSegmentCDFrom.TxtCode.Text))
                {
                    if (!scSegmentCDFrom.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scSegmentCDFrom.SetFocus(1);
                    }
                }
            }
        }

        private void scSegmentCDTo_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSegmentCDTo.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSegmentCDTo.TxtCode.Text))
                {
                    if (!scSegmentCDTo.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scSegmentCDTo.SetFocus(1);
                    }
                }
            }
        }

        private void scBrandCDTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(scBrandCDFrom.TxtCode.Text) && !string.IsNullOrEmpty(scBrandCDTo.TxtCode.Text))
            {
                if (string.Compare(scBrandCDFrom.TxtCode.Text, scBrandCDTo.TxtCode.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    scBrandCDTo.Focus();
                }
            }
        }

        private void scSegmentCDTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(scSegmentCDFrom.TxtCode.Text) && !string.IsNullOrEmpty(scSegmentCDTo.TxtCode.Text))
            {
                if (string.Compare(scSegmentCDFrom.TxtCode.Text, scSegmentCDTo.TxtCode.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    scSegmentCDTo.Focus();
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
        protected override void EndSec()
        {
            this.Close();
        }

        private void frmTenzikaiShouhinJouhouShuturyoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
