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
using Search;

namespace MasterTouroku_ShiireKakeritsu
{
    public partial class frmMasterTouroku_ShiireKakeritsu : FrmMainForm
    {
        MasterTouroku_ShiireKakeritsu_BL mskbl;
        public frmMasterTouroku_ShiireKakeritsu()
        {
            InitializeComponent();
            mskbl = new MasterTouroku_ShiireKakeritsu_BL();
        }

        private void frmMasterTouroku_ShiireKakeritsu_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_ShiireKakeritsu";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            SetRequiredField();
            scSupplierCD.SetFocus(1);
        }
        private void SetRequiredField()
        {
            scSupplierCD.TxtCode.Require(true);
            txtRevisionDate.Require(true);
            txtRate.Require(true);
        }
        protected override void EndSec()
        {
            this.Close();
        }
        public void Clear()
        {
            Clear(panelDetail);
            scSupplierCD.SetFocus(1);
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { scSupplierCD.TxtCode }))
                return false;
                if (!scSupplierCD.IsExists(1))
                {
                    mskbl.ShowMessage("E101");
                    scSupplierCD.SetFocus(1);
                    return false;
                }
                if(scSupplierCD.IsExists(1))
                {
                    mskbl.ShowMessage("E119");
                    scSupplierCD.SetFocus(1);
                    return false;
                }
            if (!RequireCheck(new Control[] { txtRevisionDate, txtRate,txtCopy }))
                return false;
            return true;
        }

        private void scSupplierCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                
            }
        }
        //public override void FunctionProcess(int index)
        //{
        //    if (index + 1 == 12)
        //    {
        //        GetData();
        //    }
        //}
        //private void GetData()
        //{

        //}

        private void frmMasterTouroku_ShiireKakeritsu_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            Checkstate(true);
        }
        private void Checkstate(bool flag)
        {
            foreach (DataGridViewRow row1 in dgv_ShiireKakeritsu.Rows)
            {
                row1.Cells["colChk"].Value = flag;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }
}
