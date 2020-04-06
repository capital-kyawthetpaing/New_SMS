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
        M_OrderRate_Entity moe;
        DataTable dt;
        public frmMasterTouroku_ShiireKakeritsu()
        {
            InitializeComponent();
            mskbl = new MasterTouroku_ShiireKakeritsu_BL();
            moe = new M_OrderRate_Entity();
            dt = new DataTable();
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
            txtRate1.Require(true);
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
                //if(scSupplierCD.IsExists(1))
                //{
                //    mskbl.ShowMessage("E119");
                //    scSupplierCD.SetFocus(1);
                //    return false;
                //}
            //if (!RequireCheck(new Control[] { txtRevisionDate, txtRate1,txtCopy }))
            //    return false;
            return true;
        }

        private M_OrderRate_Entity GetSearchInfo()
        {
            moe = new M_OrderRate_Entity()
            {
                VendorCD = scSupplierCD.TxtCode.Text,
                BrandCD = scBrandCD.TxtCode.Text,
                SportsCD = scSportsCD.TxtCode.Text,
                SegmentCD = scSegmentCD.TxtCode.Text,
                LastSeason = txtLastSeason.Text,
                ChangeDate = txtChangeDate.Text,
                Rate = txtRate.Text
            };
            return moe;
        }

        //private void btnSelectAll_Click(object sender, EventArgs e)
        //{
           
        //}

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
            scSportsCD.Value1 = "202";
        }

        private void ckM_SearchControl7_Enter(object sender, EventArgs e)
        {
            scSegmentCD.Value1 = "203";
        }
        private void scSupplierCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                scSupplierCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSupplierCD.TxtCode.Text))
                {
                    if (scSupplierCD.SelectData())
                    {
                        scSupplierCD.Value1 = scSupplierCD.TxtCode.Text;
                        scSupplierCD.Value2 = scSupplierCD.LabelText;
                    }
                    else
                    {

                        scSupplierCD.SetFocus(1);
                    }
                }
            }
        }

        private void ckM_SearchControl3_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ckM_SearchControl3.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(ckM_SearchControl3.TxtCode.Text))
                {
                    if (ckM_SearchControl3.SelectData())
                    {
                        ckM_SearchControl3.Value1 = ckM_SearchControl3.TxtCode.Text;
                        ckM_SearchControl3.Value2 = ckM_SearchControl3.LabelText;
                    }
                    else
                    {

                        ckM_SearchControl3.SetFocus(1);
                    }
                }
            }
        }

        private void ckM_SearchControl6_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSportsCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSportsCD.TxtCode.Text))
                {
                    if (scSportsCD.SelectData())
                    {
                        scSportsCD.Value1 = scSportsCD.TxtCode.Text;
                        scSportsCD.Value2 = scSportsCD.LabelText;
                    }
                    else
                    {
                        scSportsCD.SetFocus(1);
                    }
                }
            }
        }
    

        private void ckM_SearchControl7_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSegmentCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSegmentCD.TxtCode.Text))
                {
                    if (scSegmentCD.SelectData())
                    {
                        scSegmentCD.Value1 = scSegmentCD.TxtCode.Text;
                        scSegmentCD.Value2 = scSegmentCD.LabelText;
                    }
                    else
                    {
                        scSegmentCD.SetFocus(1);
                    }
                }
            }
        }

       

        private void ckM_SearchControl4_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ckM_SearchControl4.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(ckM_SearchControl4.TxtCode.Text))
                {
                    if (ckM_SearchControl4.SelectData())
                    {
                        ckM_SearchControl4.Value1 = ckM_SearchControl4.TxtCode.Text;
                        ckM_SearchControl4.Value2 = ckM_SearchControl4.LabelText;
                    }
                    else
                    {
                        ckM_SearchControl4.SetFocus(1);
                    }
                }
            }
        }
        private void frmMasterTouroku_ShiireKakeritsu_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void btnSelectAll_Click_1(object sender, EventArgs e)
        {
            if (ErrorCheck())
            {
                moe = GetSearchInfo();
                DataTable dt = mskbl.M_ShiireKakeritsu_Select(moe);
                if (dt.Rows.Count > 0)
                {
                    dgv_ShiireKakeritsu.DataSource = dt;
                }
                else
                {
                    dgv_ShiireKakeritsu.DataSource = null;
                }
            }
        }
    }
}
