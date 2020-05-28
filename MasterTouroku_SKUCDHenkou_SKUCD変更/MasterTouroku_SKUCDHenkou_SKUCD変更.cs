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

namespace MasterTouroku_SKUCDHenkou_SKUCD変更
{
    public partial class MasterTouroku_SKUCDHenkou_SKUCD変更 : FrmMainForm
    {
        MasterTouroku_SKUCDHenkou_SKUCD変更_BL mskubl;
        M_ITEM_Entity mie;
        int type = 0;

        public MasterTouroku_SKUCDHenkou_SKUCD変更()
        {
            InitializeComponent();
            mskubl = new MasterTouroku_SKUCDHenkou_SKUCD変更_BL();
            mie = new M_ITEM_Entity();
        }

        private void MasterTouroku_SKUCDHenkou_SKUCD変更_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_SKUCDHenkou_SKUCD変更";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            Sc_Item.SetFocus(1);
            F4Visible = false;
            F5Visible = false;
            F7Visible = false;
            F8Visible = false;
            F10Visible = false;
        }

        private void SetRequiredField()
        {
            Sc_Item.TxtCode.Require(true);
        }
        protected override void EndSec()
        {
            base.EndSec();
        }

        public override void FunctionProcess(int Index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
            switch (Index + 1)
            {
                case 2:
                    ChangeMode(EOperationMode.INSERT);
                    break;
                case 3:
                    ChangeMode(EOperationMode.UPDATE);
                    break;
                case 4:  
                case 5:                 
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeMode(OperationMode);
                        Sc_Item.SetFocus(1);
                    }
                    break;
                case 11:
                    //F11();
                    break;
                case 12:
                    //F12();
                    break;
            }
        }

        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    Clear(PanelHeader);
                    Clear(panelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(panelDetail);
                    Sc_Item.SearchEnable = true;
                    F9Visible = true;
                    F11Display.Enabled = F11Enable = true;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(PanelHeader);
                    Clear(panelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(panelDetail);
                    Sc_Item.SearchEnable = true;
                    F9Visible = true;
                    F12Enable = false;
                    F11Display.Enabled = F11Enable = true;
                    break;
            }
            Sc_Item.SetFocus(1);
        }

        private bool ErrorCheck(int index)
       {
            if(index == 11)
            {
                if (RequireCheck(new Control[] { Sc_Item.TxtCode }))
                    return false;

                if(string.IsNullOrWhiteSpace(txtDate1.Text))
                {
                    mskubl.ShowMessage("E102");
                    txtDate1.Focus();
                    return false;
                }

                if (OperationMode == EOperationMode.INSERT)
                {
                    if(type == 1)
                    {
                        mie.ITemCD = Sc_Item.TxtCode.Text;
                        mie.ChangeDate = txtDate1.Text;
                        DataTable dtitem = new DataTable();
                        dtitem = mskubl.M_ITEM_NormalSelect(mie);
                        if (dtitem.Rows.Count > 0)
                        {
                            mskubl.ShowMessage("E132");
                            Sc_Item.SetFocus(1);
                            return false;
                        }

                    }
                    else if(type == 2)
                    {
                        if (string.IsNullOrWhiteSpace(txtRevDate.Text))
                        {
                            mskubl.ShowMessage("E102");
                            txtRevDate.Focus();
                            return false;
                        }

                        mie.ITemCD = Sc_Item.TxtCode.Text;
                        mie.ChangeDate = txtRevDate.Text;
                        DataTable dtitem = new DataTable();
                        dtitem = mskubl.M_ITEM_NormalSelect(mie);
                        if (dtitem.Rows.Count > 0)
                        {
                            mskubl.ShowMessage("E132");
                            Sc_Item.SetFocus(1);
                            return false;
                        }
                    }
                }
                if(OperationMode == EOperationMode.UPDATE)
                {
                    mie.ITemCD = Sc_Item.TxtCode.Text;
                    mie.ChangeDate = txtDate1.Text;
                    DataTable dtitem = new DataTable();
                    dtitem = mskubl.M_ITEM_NormalSelect(mie);
                    if (dtitem.Rows.Count == 0)
                    {
                        mskubl.ShowMessage("E133");
                        Sc_Item.SetFocus(1);
                        return false;
                    }
                }
            }
            else if(index == 12)
            {
                //if(!string.IsNullOrWhiteSpace(txtoldsize1.Text))
                //{
                //    if(string.IsNullOrWhiteSpace(txtnewsize1.Text))
                //    {
                //        mskubl.ShowMessage("E102");
                //        txtnewsize1.Focus();
                //        return false;
                //    }
                //}
            }

            return true;
       }

        private void F11Display_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void F11()
        {
            if(type == 1)
            {
                if (ErrorCheck(11))
                {

                }
            }
            else if(type == 2)
            {
                if(ErrorCheck(11))
                {

                }
            }
            
        }
    }
}
