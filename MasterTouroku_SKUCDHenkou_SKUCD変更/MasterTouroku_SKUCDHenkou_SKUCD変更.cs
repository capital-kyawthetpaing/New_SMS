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
        M_SKUInfo_Entity msi;
        M_SKUPrice_Entity mse;
        M_SKU_Entity mset;
        int type = 0;

        private int min=0;
        private int max = 0;

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
            

            for (int t = 0; t < 50; t++)
            {
                ckM_GridView1.Rows.Add();
                
            }
            EnterVoid();

            panel2.MouseWheel += Panel2_MouseWheel;
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //  if ()
         //   base.OnMouseWheel(e);
        }
        private void Panel2_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = false;
           // return;
          //  MessageBox.Show(e.Delta.ToString());
            
           // MessageBox.Show(panel2.HorizontalScroll.Value.ToString() + Environment.NewLine + ckM_GridView1.HorizontalScrollingOffset + Environment.NewLine + ckM_GridView1.FirstDisplayedScrollingColumnIndex);
            //throw new NotImplementedException();
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x20a)

                return;
            base.WndProc(ref m);
        }
        private void Add_Name_Tag(Control ctl)
        {
            var c = GetAllControls(ctl);
            int k = 0;
            foreach (var con in c)
            {
                if (con is CKM_TextBox ct)
                {
                    if (ct.Name.Contains("cn_") || ct.Name.Contains("sn_"))
                    {
                        ct.UseColorSizMode = true;
                        ct.TabIndex = Convert.ToInt32(ct.Name.Split('_').Last());
                        ct.KeyDown += Ct_KeyDown;
                    }
                    else
                    {
                        ct.Enabled = false;
                       // (ctl.Controls.Find("sc_"))
                    }
                }
                if (con is CheckBox cc && ( cc.Name.Contains("cc_") | cc.Name.Contains("sc_")))
                {
                    cc.TabIndex = Convert.ToInt32(cc.Name.Split('_').Last());
                    cc.CheckedChanged += Cc_CheckedChanged;
                }
                k++;
            }
            panel2.Scroll += Panel2_Scroll;
            panel4.Scroll += Panel2_Scroll;
        }

        private void Panel2_Scroll(object sender, ScrollEventArgs e)
        {
            if ((sender as Panel).Name == "panel2")
            {
                if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                {

                }
            }
            else {

            }
        }

        private void Cc_CheckedChanged(object sender, EventArgs e)
        {
            //Check All of one or more Checked occurred
            var Chk = (sender as CheckBox);
            if (Chk.Checked)
            {

                for (int i = 0; i < 20; i++)
                {
                    var currentControl = this.Controls.Find("sc_" + (i + 1).ToString(), true)[0] as CheckBox;
                    if (currentControl.Checked && currentControl.Enabled && Chk.Name != currentControl.Name && Chk.Name.Contains("sc_"))
                    {
                        MessageBox.Show("Please only check the biggest one");
                        Chk.Checked = false;
                        return;
                    }
                }
                for (int i = 0; i < 50; i++)
                {
                    var currentControl = this.Controls.Find("cc_" + (i + 1).ToString(), true)[0] as CheckBox;
                    if (currentControl.Checked && currentControl.Enabled && Chk.Name != currentControl.Name && Chk.Name.Contains("cc_"))
                    {
                        MessageBox.Show("Please only check the biggest one");
                        Chk.Checked = false;
                        return;
                    }
                }

            }
          //  Check_E229();// Check Biggest
        }

        private void Ct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //IsStop = false;
                //if(!IsStop)Check_E102(); // Check Empty
                //if(!IsStop)Check_E105();  // Check Duplicate
                //if(!IsStop)Check_E228();   // Check unOrder without sorting
                //if(!IsStop)Check_E229(true);    // Check Biggest
            }
        }
        static bool IsStop = false;
        private void Check_E229(bool IsEnterTrigger = false)
        {
            int count = 0;
            string Con = "";
            var id = "";
            if (ActiveControl.Name.Contains("sn_"))
            {
                count = 20;
                Con = "sn_";
            }
            else
            {
                count = 50;
                Con = "cn_";
            }
            if (ActiveControl is CheckBox cbx && cbx.Checked)
            {
                id = cbx.Name.Split('_').Last();


                if (!IsEnterTrigger)
                {
                    if (id != "")
                    {
                        var ckb = (this.Controls.Find(Con + (id).ToString(), true)[0] as CKM_TextBox);
                        if (!string.IsNullOrEmpty(ckb.Text))
                        {
                            for (int h = 0; h < count; h++)
                            {
                                var control = this.Controls.Find(Con + (h + 1).ToString(), true)[0] as CKM_TextBox;
                                if (ckb.Name != control.Name)
                                {
                                    if (String.IsNullOrWhiteSpace(control.Text))
                                    {
                                        if (GetInt((control.Text)) > GetInt(ckb.Text))
                                        {
                                            bbl.ShowMessage("E229");
                                            control.Focus();
                                            IsStop = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (id != "")
                    {
                        var text = 0;
                        var Val = "";
                        for (int l = 0; l < count; l++)
                        {
                            var control = this.Controls.Find(Con + (l + 1).ToString(), true)[0] as CheckBox;
                            if (control.Checked)
                            {
                                Val = control.Name.Split('_').Last();
                                break;
                            }
                        }
                        var ctrl = this.Controls.Find(Con + (Val).ToString(), true)[0] as CKM_TextBox;
                        text = GetInt(ctrl.Text);   ///This Text Must be Biggest  on Check
                        for (int l = 0; l < count; l++)
                        {
                            var txt = (this.Controls.Find(Con + (l + 1).ToString(), true)[0] as CKM_TextBox);
                            if (!string.IsNullOrEmpty(txt.Text) && txt.Name != ctrl.Name)
                            {
                                if (GetInt(txt.Text) > text)
                                {
                                    bbl.ShowMessage("E229");
                                    txt.Focus();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void Check_E102()
        {
            if (ActiveControl is CKM_TextBox ct && (ct.Name.Contains("sn_") | ct.Name.Contains("cn_")))
            {
                if (ct.Name.Contains("sn_"))
                {
                    var c = GetAllControls(panel2);
                    foreach (var con in c)
                    {
                        if (con is CKM_TextBox cst && !cst.Enabled && cst.Name.Contains("so_"+ ct.Name.Split('_').Last()))
                        {
                          //  var ckb = (this.Controls.Find(Con + (id).ToString(), true)[0] as CKM_TextBox);
                            if (!String.IsNullOrEmpty(cst.Text) && String.IsNullOrEmpty(ct.Text))
                            {
                                bbl.ShowMessage("E102");
                                ct.Focus();
                                IsStop = true;
                            }
                        }
                    }
                }
                else if (ct.Name.Contains("cn_"))
                {
                    var c = GetAllControls(panel4);
                    foreach (var con in c)
                    {
                        if (con is CKM_TextBox cst && !cst.Enabled &&cst.Name.Contains("co_"+ ct.Name.Split('_').Last()))
                        {
                            if (!String.IsNullOrEmpty(cst.Text) && String.IsNullOrEmpty(ct.Text))
                            {
                                bbl.ShowMessage("E102");
                                ct.Focus();
                                IsStop = true;
                            }
                        }
                    }
                }
            }
            
        }
        private void Check_E105()
        {
            int count = 0;
            string Con = "";
            if (ActiveControl.Name.Contains("sn_"))
            {
                count = 20;
                Con = "sn_";
            }
            else
            {
                count = 50;
                Con = "cn_";
            }
            if (ActiveControl is CKM_TextBox ctb && ctb.Name.Contains(Con))
            {
                var Currentval = ctb.Text;

                for (int g = 0; g < count; g++)
                {
                    var ckb = this.Controls.Find(Con + (g + 1).ToString(), true)[0] as CKM_TextBox;
                    if (!string.IsNullOrWhiteSpace(ckb.Text) && ckb.Name != ctb.Name)
                    {
                        if (ckb.Text == Currentval)
                        {
                            bbl.ShowMessage("E105");
                            ctb.Focus();
                            //  break;
                            IsStop = true;
                        }
                    }
                }
            }
           
        }
        private void Check_E228()
        {
            int count = 0;
            string Con = "";
            if (ActiveControl.Name.Contains("sn_"))
            {
                count = 20;
                Con = "sn_";
            }
            else
            {
                count = 50;
                Con = "cn_";
            }
            if (ActiveControl is CKM_TextBox ctb && ctb.Name.Contains(Con))
            {

                var lo = new List<int>();
                for (int j = 0; j < count; j++)
                {
                    var ckb = this.Controls.Find(Con + (j+1).ToString(), true)[0] as CKM_TextBox;
                    if (!string.IsNullOrWhiteSpace(ckb.Text))
                    {
                        lo.Add( GetInt(ckb.Text));
                    }
                }
                lo.Sort();
                bool IsSerial = true;
                int h =lo.First();
                int serial = 0;
                foreach (var l in lo)
                {
                   
                    if (l != h)
                    {
                        IsSerial = false;
                        break;
                    }
                    h++;
                }
                if (!IsSerial)
                {
                    bbl.ShowMessage("E228");
                    ctb.Focus();
                    IsStop = true;
                }
            }
        }
        private int GetInt(string val)
        {
            if (!string.IsNullOrWhiteSpace(val))
            {
                return Convert.ToInt32(val);
            }
            return 0;
        }
        private string GetPad(int val)
        {
            if (val != 0)
            {
                return val.ToString().PadLeft(4, '0');
            }
            return "0001";
        }
        private void SetRequiredField()
        {
            Sc_Item.TxtCode.Require(true);
        }
        protected override void EndSec()
        {
            this.Close();
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
                        ClearGrid();
                        lblProductName.Text = "";
                        lblPartNum.Text = "";
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
                    DisablePanel(panelDetail);
                    //EnablePanel(panelDetail);
                    Sc_Item.SearchEnable = true;
                    F9Visible = true;
                    F11Display.Enabled = F11Enable = true;
                    F11Visible = true;
                    F12Enable = false;
                   
                    break;
                
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    break;
                case EOperationMode.UPDATE:
                    Clear(PanelHeader);
                    Clear(panelDetail);
                    EnablePanel(PanelHeader);
                    DisablePanel(panelDetail);
                    Sc_Item.SearchEnable = true;
                    F9Visible = true;
                    F12Enable = false;
                    F11Display.Enabled = F11Enable = true;
                    F11Visible = true;
                    //ClearGrid();
                    break;
            }
            Sc_Item.SetFocus(1);
        }
        private void ClearGrid()
        {
            for (int i = 0; i < ckM_GridView1.Columns.Count; i++)
            {
                for (int j = 0; j < ckM_GridView1.Rows.Count; j++)
                {
                    ckM_GridView1.Rows[j].Cells[i].Value = "";
                }
            }
        }
        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                if (!RequireCheck(new Control[] { Sc_Item.TxtCode }))
                    return false;
                if (string.IsNullOrWhiteSpace(txtDate1.Text))
                {
                    mskubl.ShowMessage("E102");
                    txtDate1.Focus();
                    return false;
                }
                if (type == 1)
                {
                    if (string.IsNullOrWhiteSpace(txtRevDate.Text))
                    {
                        mskubl.ShowMessage("E102");
                        txtRevDate.Focus();
                        return false;
                    }
                }
                if (OperationMode == EOperationMode.INSERT)
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
                    if (string.IsNullOrWhiteSpace(txtRevDate.Text))
                    {
                        mskubl.ShowMessage("E102");
                        txtRevDate.Focus();
                        return false;
                    }

                    mie.ITemCD = Sc_Item.TxtCode.Text;
                    mie.ChangeDate = txtRevDate.Text;
                    dtitem = new DataTable();
                    dtitem = mskubl.M_ITEM_NormalSelect(mie);
                    if (dtitem.Rows.Count == 0)
                    {
                        mskubl.ShowMessage("E133");
                        Sc_Item.SetFocus(1);
                        return false;
                    }
                }
                if (OperationMode == EOperationMode.UPDATE)
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
            return true;
        }

        private bool SelectCheck(int[] arrayList)
        {
            foreach (int s in arrayList)
            {
                if(max < s)
                {
                    return false;
                }
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

        private int getMissingNo(int[] a, int n)
        {
            int i, total = 1;

            for (i = 2; i <= (n + 1); i++)
            {
                total += i;
                total -= a[i - 2];
            }
            return total;
        }

        private void F11Display_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void F11()
        {
            if (OperationMode == EOperationMode.INSERT)
            {
                type = 1;
            }
            else if (OperationMode == EOperationMode.UPDATE)
            {
                type = 2;
            }
            if (ErrorCheck(11))
            {
                mie.ITemCD = Sc_Item.TxtCode.Text;
                
                DataTable dtitem = new DataTable();
                if (type == 2)
                {
                    mie.ChangeDate = txtDate1.Text;
                    dtitem = mskubl.M_ITem_SelectForSKUCDHenkou01(mie);
                }
                else
                {
                    mie.ChangeDate = txtRevDate.Text;
                    dtitem = mskubl.M_ITem_SelectForSKUCDHenkou01(mie);
                }

                DisablePanel(PanelHeader);
                EnablePanel(panelDetail);
                Add_Name_Tag(panel4);
                Add_Name_Tag(panel2);
                F11Visible = false;
                F11Display.Enabled = false;
                F12Enable = true;
                ckM_GridView1.Refresh();
                lblProductName.Text = dtitem.Rows[0]["ItemName"].ToString();
                lblPartNum.Text = dtitem.Rows[0]["MakerItem"].ToString();
                   
                Set_Value(dtitem,panel2);
                Set_Value(dtitem,panel4);
                Set_JanCD(dtitem);
                Set_Check(dtitem);
            }
        }
        private void Set_Check(DataTable dt)
        {

        }
        private void Set_JanCD(DataTable dt)
        {
            dt_Main = dt;
            dt_Main.Columns.Add("NewSizeNo");
            dt_Main.Columns.Add("NewColorNo");
            ckM_GridView1.ShowCellToolTips = false;
            for (int l = 0; l < dt.Rows.Count; l++)
            {
                var c = Convert.ToInt32(dt.Rows[l]["SizeNo"].ToString());
                var r = Convert.ToInt32(dt.Rows[l]["ColorNo"].ToString());
                ckM_GridView1[c-1,r-1].Value = dt.Rows[l]["JanCD"].ToString();
            }
        }
        private void Set_Value(DataTable dt, Panel pnl)
        {
            if (dt.Rows.Count > 0)
            {
                var SizeCount = Convert.ToInt32(dt.Rows[0]["osc"].ToString());
                var ColorCount = Convert.ToInt32(dt.Rows[0]["occ"].ToString());

            var c = GetAllControls(pnl);   /// Size Count Position
                foreach (var con in c)
                {
                    if (con is CKM_TextBox ct && (ct.Name.Contains("sn_") | ct.Name.Contains("cn_")))
                    {
                        if (ct.Name.Contains("sn_"))
                        {
                            if (SizeCount >= Convert.ToInt32(ct.Name.Split('_').Last()))
                            {
                                ct.Text = ct.Name.Split('_').Last().PadLeft(4, '0');
                                (pnl.Controls.Find("so_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox).Text = ct.Text;

                            }
                            else
                            {
                                ct.Text = "";
                                ct.Enabled = false;
                            }
                        }
                        else if (ct.Name.Contains("cn_"))
                        {
                            if (ColorCount >= Convert.ToInt32(ct.Name.Split('_').Last()))
                            {
                                ct.Text = ct.Name.Split('_').Last().PadLeft(4, '0');
                                (pnl.Controls.Find("co_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox).Text = ct.Text;
                            }
                            else
                            {
                                ct.Text = "";
                                ct.Enabled = false;
                            }
                        }
                    }
                }

                if (pnl.Name == "panel2")
                {
                    var dt_SizeName = dt.AsEnumerable().GroupBy(r => r.Field<string>("SizeName")).Select(g => g.First()).CopyToDataTable();
                    int h = 0;
                    foreach (DataRow dr in dt_SizeName.Rows)
                    {
                        h++;
                        (pnl.Controls.Find("sm_" + h.ToString(), true)[0] as CKM_TextBox).Text = dr["SizeName"].ToString();
                        (pnl.Controls.Find("sc_" + h.ToString(), true)[0] as CheckBox).Checked = (dr["DeleteFalg"].ToString()!="0");

                    }
                    for (int a = h; a < 20; a++)
                    {
                        (pnl.Controls.Find("sc_" + (a+1).ToString(), true)[0] as CheckBox).Enabled=false;
                    }
                }
                else
                {
                    var dt_Color = dt.AsEnumerable().Take(ColorCount).CopyToDataTable();
                    int k = 0;
                    foreach (DataRow dr in dt_Color.Rows)
                    {
                        k++;
                        (pnl.Controls.Find("cm_" + k.ToString(), true)[0] as CKM_TextBox).Text = dr["ColorName"].ToString();
                        (pnl.Controls.Find("cc_" + k.ToString(), true)[0] as CheckBox).Checked = (dr["DeleteFlag"].ToString()!="0");
                    }
                    for (int a = k; a < 50; a++)
                    {
                        (pnl.Controls.Find("cc_" + (a+1).ToString(), true)[0] as CheckBox).Enabled = false;
                    }
                }
            }
        }
        private DataTable dt_Main;
        private string C_dt = "";
        private void F12()
        {
            var C_DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            C_dt = C_DateTime;
            if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                return;
            if (ErrorCheck_())
            {
                if ( OperationMode == EOperationMode.UPDATE)
                {
                    var dtxml = bbl.DataTableToXml(D_SKUUpdate());
                    var dtxml_1 = bbl.DataTableToXml(D_SKUChange());
                    var IsUpdate = mskubl.SKUUpdate(dtxml, dtxml_1, C_dt.Split(' ')[0].ToString(), C_dt.Split(' ').Last(), InOperatorCD, InProgramID, InPcID, "変更", (Sc_Item.TxtCode.Text + txtDate1.Text));
                    if (IsUpdate)
                    {
                        bbl.ShowMessage("I101");
                        ChangeMode(EOperationMode.UPDATE);
                     
                       // ckM_GridView1.Enabled = false;
                    }
                }
                else if (OperationMode == EOperationMode.INSERT) {
                    var dtxml = bbl.DataTableToXml(D_SKUUpdate());
                    var dtxml_1 = bbl.DataTableToXml(D_SKUChange());
                    var IsUpdate = mskubl.SKUUpdate(dtxml, dtxml_1, C_dt.Split(' ')[0].ToString(), C_dt.Split(' ').Last(), InOperatorCD, InProgramID, InPcID, "新規", (Sc_Item.TxtCode.Text + txtDate1.Text), "I");
                    if (IsUpdate)
                    {
                        bbl.ShowMessage("I101");
                        ChangeMode(EOperationMode.INSERT);
                        
                        
                       // ckM_GridView1.Enabled = false;
                    }
                }
                ckM_GridView1.RefreshEdit();
                ckM_GridView1.Refresh();
                ClearGrid();
                lblProductName.Text = "";
                lblPartNum.Text = "";
                /////////////////Insert and Update
            }
        }
        private DataTable D_SKUUpdate()
        {
            var dt = new DataTable();
            dt.Columns.Add("InsertOperator");
            dt.Columns.Add("InsertDateTime");
            dt.Columns.Add("AdminNO");
            dt.Columns.Add("JanCD");
            dt.Columns.Add("NewSKUCD");
            dt.Columns.Add("SKUCD"); 
            dt.Columns.Add("ChangeDate");
            dt.Columns.Add("DeleteFlg");
            dt.Columns.Add("NewSizeNo");
            dt.Columns.Add("SizeNo");
            dt.Columns.Add("NewColorNo");
            dt.Columns.Add("ColorNo");
            int h = 0;
            foreach (DataRow r in dt_Main.Rows)
            {
               var s= (this.Controls.Find("sn_" + r["SizeNo"].ToString(), true)[0] as CKM_TextBox);
                var c = (this.Controls.Find("cn_" + r["ColorNo"].ToString(), true)[0] as CKM_TextBox);
                var sc_ = (this.Controls.Find("sc_" + r["SizeNo"].ToString(), true)[0] as CheckBox);
                var cc_ = (this.Controls.Find("cc_" + r["ColorNo"].ToString(), true)[0] as CheckBox);
                r["NewSizeNo"] =  (s.Enabled && !string.IsNullOrWhiteSpace(s.Text)) ? s.Text: s.Text;
                r["NewColorNo"] = (c.Enabled && !string.IsNullOrWhiteSpace(c.Text)) ? c.Text : c.Text;
                
                dt.Rows.Add(new string[] {InOperatorCD, C_dt,r["AdminNo"].ToString(),r["JANCD"].ToString() ,(Sc_Item.TxtCode.Text+s.Text+c.Text).Trim(),r["SKUCD"].ToString(),txtDate1.Text, (!sc_.Checked && !cc_.Checked)? "0":"1",
                    GetInt(r["NewSizeNo"].ToString()).ToString(),GetInt(r["SizeNo"].ToString()).ToString(), GetInt(r["NewColorNO"].ToString()).ToString(),GetInt(r["ColorNo"].ToString()).ToString() });
                h++;
            }
            return dt; 
        }

       
        //private DataTable M_SKUInsert()
        //{
            
        //}
        private DataTable D_SKUChange()
        {
            var dt = new DataTable();
            dt.Columns.Add("InsertOperator");
            dt.Columns.Add("InsertDateTime");
            dt.Columns.Add("ItemCD");
            dt.Columns.Add("SizeColorKBN");
            dt.Columns.Add("NewNo");
            dt.Columns.Add("No");
            dt.Columns.Add("ChangeDate");
            dt.Columns.Add("DeleteFlg");

            for (int u = 0; u < 20; u++)
            {
                var txt = this.Controls.Find("sn_"+(u+1).ToString(), true)[0] as CKM_TextBox;
                var txtOld = this.Controls.Find("so_" + (u + 1).ToString(), true)[0] as CKM_TextBox;
                var chk = this.Controls.Find("sc_" + (u + 1).ToString(), true)[0] as CheckBox;

                if (txt.Enabled)
                {
                    dt.Rows.Add(new string[] {InOperatorCD, C_dt,Sc_Item.TxtCode.Text, "1",GetInt(txt.Text).ToString(),GetInt(txtOld.Text).ToString(),txtDate1.Text,(chk.Checked)? "1": "0"});
                }
            }
            for (int u = 0; u < 50; u++)
            {
                var txt = this.Controls.Find("cn_" + (u + 1).ToString(), true)[0] as CKM_TextBox;
                var txtOld = this.Controls.Find("co_" + (u + 1).ToString(), true)[0] as CKM_TextBox;
                var chk = this.Controls.Find("cc_" + (u + 1).ToString(), true)[0] as CheckBox;

                if (txt.Enabled)
                {
                    dt.Rows.Add(new string[] { InOperatorCD, C_dt, Sc_Item.TxtCode.Text, "2", GetInt(txt.Text).ToString(), GetInt(txtOld.Text).ToString(), txtDate1.Text, (chk.Checked) ? "1" : "0" });
                }
            }
            return dt;
        }
        
        
        private bool  ErrorCheck_()
        {
            try
            {
                //E102
                for (int p = 0; p < 20; p++)
                {
                    var txt = (panel2.Controls.Find("sn_" + (p + 1).ToString(), true)[0] as CKM_TextBox);
                    if (txt.Enabled && String.IsNullOrEmpty(txt.Text))
                    {
                        bbl.ShowMessage("E102");
                        txt.Focus();
                        return false;
                    }
                }
                for (int p = 0; p < 50; p++)
                {
                    var txt = (panel4.Controls.Find("cn_" + (p + 1).ToString(), true)[0] as CKM_TextBox);
                    if (txt.Enabled && String.IsNullOrEmpty(txt.Text))
                    {
                        bbl.ShowMessage("E102");
                        txt.Focus();
                        return false;
                    }
                }
                //E105 //    lstNames.GroupBy(n => n).Any(c => c.Count() > 1);
                List<string> lst = new List<string>();
                for (int p = 0; p < 20; p++)
                {
                    var txt = (panel2.Controls.Find("sn_" + (p + 1).ToString(), true)[0] as CKM_TextBox);
                    if (txt.Enabled && !String.IsNullOrEmpty(txt.Text))
                    {
                        if (lst.Contains(txt.Text))
                        {
                            bbl.ShowMessage("E105");
                            txt.Focus();
                            return false;
                        }
                        else
                            lst.Add(txt.Text);
                    }
                }
                List<string> lst_C = new List<string>();
                for (int p = 0; p < 50; p++)
                {
                    var txt = (panel4.Controls.Find("cn_" + (p + 1).ToString(), true)[0] as CKM_TextBox);
                    if (txt.Enabled && !String.IsNullOrEmpty(txt.Text))
                    {
                        if (lst_C.Contains(txt.Text))
                        {
                            bbl.ShowMessage("E105");
                            txt.Focus();
                            return false;
                        }
                        else
                            lst_C.Add(txt.Text);
                    }
                }
                //E228
                var ls = new List<int>();
                for (int j = 0; j < 20; j++)
                {
                    var ckb = this.Controls.Find("sn_" + (j + 1).ToString(), true)[0] as CKM_TextBox;
                    if (!string.IsNullOrWhiteSpace(ckb.Text) && ckb.Enabled)
                    {
                        ls.Add(GetInt(ckb.Text));
                    }
                }
                ls.Sort();
                bool IsSerial = true;
                int h = ls.First();
                int current_size = 0;
                foreach (var l in ls)
                {

                    if (l != h)
                    {
                        current_size = l;
                        IsSerial = false;
                        break;
                    }
                    h++;
                }
                if (!IsSerial)
                {
                    bbl.ShowMessage("E228");
                    (this.Controls.Find("sn_" + (current_size).ToString(), true)[0] as CKM_TextBox).Focus();
                    return false;
                }
                var lc = new List<int>();
                for (int j = 0; j < 20; j++)
                {
                    var ckb = this.Controls.Find("cn_" + (j + 1).ToString(), true)[0] as CKM_TextBox;
                    if (!string.IsNullOrWhiteSpace(ckb.Text) && ckb.Enabled)
                    {
                        lc.Add(GetInt(ckb.Text));
                    }
                }
                lc.Sort();
                bool IsSerial_Color = true;
                int h_C = lc.First();
                int current_color = 0;
                foreach (var l in ls)
                {

                    if (l != h_C)
                    {
                        current_color = l;
                        IsSerial_Color = false;
                        break;
                    }
                    h_C++;
                }
                if (!IsSerial_Color)
                {
                    bbl.ShowMessage("E228");
                    (this.Controls.Find("cn_" + (current_color).ToString(), true)[0] as CKM_TextBox).Focus();
                    return false;
                }
                //E229
                int val = 0;
                for (int o = 0; o < 20; o++)
                {
                    var c = this.Controls.Find("sc_" + (o + 1).ToString(), true)[0] as CheckBox;
                    if (c.Enabled && c.Checked)
                    {
                        val = o + 1;
                        break;
                    }
                }
                if (val != 0)
                {
                    var maxVal = Convert.ToInt32((this.Controls.Find("sn_" + (val).ToString(), true)[0] as CKM_TextBox).Text);
                    for (int o = 0; o < 20; o++)
                    {
                        var c = this.Controls.Find("sn_" + (o + 1).ToString(), true)[0] as CKM_TextBox;
                      if (c.Enabled)
                        if (maxVal < Convert.ToInt32(c.Text) )
                        {
                            bbl.ShowMessage("E229");
                            c.Focus();
                            return false;
                        }
                    }
                }
                int val_C = 0;
                for (int o = 0; o < 50; o++)
                {
                    var c = this.Controls.Find("cc_" + (o + 1).ToString(), true)[0] as CheckBox;
                    if (c.Enabled && c.Checked)
                    {
                        val_C = o + 1;
                        break;
                    }
                }
                if (val_C != 0)
                {
                    var maxVal_C = Convert.ToInt32((this.Controls.Find("cn_" + (val_C).ToString(), true)[0] as CKM_TextBox).Text);
                    for (int o = 0; o < 50; o++)
                    {
                        var c = this.Controls.Find("cn_" + (o + 1).ToString(), true)[0] as CKM_TextBox;
                        if (c.Enabled)
                            if (maxVal_C < Convert.ToInt32(c.Text) )
                        {
                            bbl.ShowMessage("E229");
                            c.Focus();
                            return false;
                        }
                    }
                }
            }
            catch(Exception ex) {
                var msg = ex.Message;
                return false;
            }
            return true;
        }
        private void Sc_Item_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                //F11();
                if(string.IsNullOrWhiteSpace(Sc_Item.TxtCode.Text))
                {
                    bbl.ShowMessage("E102");
                    Sc_Item.SetFocus(1);
                }
            }
        }

        private void txtDate1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                type = 1;
                //F11();     
                if(string.IsNullOrWhiteSpace(txtDate1.Text))
                {
                    bbl.ShowMessage("E102");
                    txtDate1.Focus();
                }
                else
                {
                    if (OperationMode == EOperationMode.INSERT)
                    {
                        mie.ITemCD = Sc_Item.TxtCode.Text;
                        mie.ChangeDate = txtDate1.Text;
                        DataTable dtitem = new DataTable();
                        dtitem = mskubl.M_ITEM_NormalSelect(mie);
                        if (dtitem.Rows.Count > 0)
                        {
                            mskubl.ShowMessage("E132");
                            Sc_Item.SetFocus(1);
                        }  
                    }
                    else
                    {
                        mie.ITemCD = Sc_Item.TxtCode.Text;
                        mie.ChangeDate = txtDate1.Text;
                        DataTable dtitem = new DataTable();
                        dtitem = mskubl.M_ITEM_NormalSelect(mie);
                        if (dtitem.Rows.Count == 0)
                        {
                            mskubl.ShowMessage("E133");
                            Sc_Item.SetFocus(1);                           
                        }
                    }
                }
               
            }
        }

        private void txtRevDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                type = 2;
                //F11();
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (string.IsNullOrWhiteSpace(txtRevDate.Text))
                    {
                        mskubl.ShowMessage("E102");
                        txtRevDate.Focus();
                    }

                    mie.ITemCD = Sc_Item.TxtCode.Text;
                    mie.ChangeDate = txtRevDate.Text;
                    DataTable dt = new DataTable();
                    dt = mskubl.M_ITEM_NormalSelect(mie);
                    if (dt.Rows.Count == 0)
                    {
                        mskubl.ShowMessage("E133");
                        Sc_Item.SetFocus(1);
                    }
                }
            }
        }

        private void ckM_GridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue == 31)
            {
                return;
            }
            panel4.VerticalScroll.Value = (ckM_GridView1.FirstDisplayedScrollingRowIndex * 18 +1);
            panel2.HorizontalScroll.Value= (ckM_GridView1.FirstDisplayedScrollingColumnIndex * 130 + 1);
        }
        public IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
        }
        private void panel4_Scroll(object sender, ScrollEventArgs e)
        {
          //  MessageBox.Show("New Value is " + e.NewValue + " And Old value is " + e.OldValue + " " + e.ScrollOrientation.ToString());

        }

        private void ckM_TextBox76_TextChanged(object sender, EventArgs e)
        {

        }

        private void MasterTouroku_SKUCDHenkou_SKUCD変更_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void Sc_Item_Load(object sender, EventArgs e)
        {

        }

        private void ckM_GridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void EnterVoid()
        {
            var c = GetAllControls(panel2);
      
            foreach (var con in c)
            {
               
                if (con is CKM_TextBox ct &&  (ct.Name.Contains("sn_")))
                {
                    ct.Enter += Ct_Enter; 
                }
            }
        }

        private void Ct_Enter(object sender, EventArgs e)
        {
           
        }

        private void panel2_Scroll_1(object sender, ScrollEventArgs e)
        {

        }
        //private void panelDetail_Paint(object sender, PaintEventArgs e)
        //{
        //}
        //private void panel3_Paint(object sender, PaintEventArgs e)
        //{
        //}
    }
}
