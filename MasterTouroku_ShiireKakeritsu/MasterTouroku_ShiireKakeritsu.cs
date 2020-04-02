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
            if (!RequireCheck(new Control[] { txtRevisionDate, txtRate }))
                return false;
            return true;
        }

        private void scSupplierCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                //F11();
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void frmMasterTouroku_ShiireKakeritsu_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
