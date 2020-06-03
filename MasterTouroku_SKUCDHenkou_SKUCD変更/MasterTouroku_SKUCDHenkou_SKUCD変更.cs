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
using CKM_Controls;
using System.Collections;

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
                    F11();
                    break;
                case 12:
                    F12();
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
                    //DisablePanel(panelDetail);
                    EnablePanel(panelDetail);
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


                //string[] b = new string[] { };
                //ArrayList myArryList = new ArrayList();
                string[] myArryList = new string[10];
                for (int i = 0; i < 10; i++)
                {
                    var sizeNewtxtbox_ = Controls.Find("txtnewsize" + (i + 1).ToString(), true)[0] as CKM_TextBox;
                    var sizeOldtxtbox_ = Controls.Find("txtoldsize" + (i + 1).ToString(), true)[0] as CKM_TextBox;
                    var sizeCheckbox_ = Controls.Find("ckM_CheckBox" + (i + 1).ToString(), true)[0] as CKM_TextBox;
                    /// doooooooooo 
                    /// 
                    if(!string.IsNullOrWhiteSpace(sizeOldtxtbox_.Text))
                    {
                        if (string.IsNullOrWhiteSpace(sizeNewtxtbox_.Text))
                        {
                            mskubl.ShowMessage("E102");
                            sizeNewtxtbox_.Focus();
                            return false;
                        }
                    }
                    myArryList[i] = sizeNewtxtbox_.Text;
                    //myArryList.Add(sizeNewtxtbox_);
                   
                    //if (sizeNewtxtbox_.)
                    //if (string.Compare(textBox1.Text, textBox2.Text, true) == 0)
                }
                if (HasDuplicates(myArryList))
                {
                    mskubl.ShowMessage("E105");
                    txtnewsize10.Focus();
                    return false;
                }
                //if(getMissingNo(myArryList,10))
                //{

                //}

               
            }

            return true;
        }

        private bool HasDuplicates(string [] arrayList)
        {
            List<string> vals = new List<string>();
            bool returnValue = false;
            foreach (string s in arrayList)
            {               
                if (!string.IsNullOrWhiteSpace(s))
                {
                    if (vals.Contains(s))
                    {
                        returnValue = true;
                        break;
                    }
                    vals.Add(s);
                }
            }
            return returnValue;
        }

        //private bool getMissingNo(int[] a, int n)
        //{
        //    int total = (n + 1) * (n + 2) / 2;

        //    for (int i = 0; i < n; i++)
        //        total -= a[i];

        //    return true;
        //}


        private void F11Display_Click(object sender, EventArgs e)
        {
            if(OperationMode == EOperationMode.INSERT)
            {
                type = 1;
            }
            else if(OperationMode == EOperationMode.UPDATE)
            {
                type = 2;
            }
            F11();
        }

        private void F11()
        {
           
            if (ErrorCheck(11))
            {

            }
          
        }

        private void F12()
        {
            if(ErrorCheck(12))
            {

            }
        }

        private void Sc_Item_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {

            }
        }

        private void txtDate1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                type = 1;
                F11();
            }
        }

        private void txtRevDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                type = 2;
                F11();
            }
        }

    }
}
