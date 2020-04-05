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
        private void ckM_SearchControl3_Enter(object sender, EventArgs e)
        {
            ckM_SearchControl3.Value1 = "202";
        }
        private void ckM_SearchControl4_Enter(object sender, EventArgs e)
        {
            ckM_SearchControl4.Value1 = "203";
        }

        private void ckM_SearchControl6_Enter(object sender, EventArgs e)
        {
            ckM_SearchControl6.Value1 = "202";
        }

        private void ckM_SearchControl7_Enter(object sender, EventArgs e)
        {
            ckM_SearchControl7.Value1 = "203";
        }
        private void ckM_SearchControl6_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ckM_SearchControl6.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(ckM_SearchControl6.TxtCode.Text))
                {
                    if (ckM_SearchControl6.SelectData())
                    {
                        ckM_SearchControl6.Value1 = ckM_SearchControl6.TxtCode.Text;
                        ckM_SearchControl6.Value2 = ckM_SearchControl6.LabelText;
                    }
                    else
                    {
                       
                        ckM_SearchControl6.SetFocus(1);
                    }
                }
            }
        }
    

        private void ckM_SearchControl7_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ckM_SearchControl7.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(ckM_SearchControl7.TxtCode.Text))
                {
                    if (ckM_SearchControl7.SelectData())
                    {
                        ckM_SearchControl7.Value1 = ckM_SearchControl7.TxtCode.Text;
                        ckM_SearchControl7.Value2 = ckM_SearchControl7.LabelText;
                    }
                    else
                    {
                        
                        ckM_SearchControl7.SetFocus(1);
                    }
                }
            }
        }

        

        
    }
}
