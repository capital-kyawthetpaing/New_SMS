using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using BL;
using System.Data;
using Entity;
using System.Runtime.InteropServices;

namespace CKM_Controls
{
    public partial class CKMShop_ComboBox : ComboBox
    {
        object UpdateData ;
        bool IsFocus = false;
        int[] c = new int[] { 0, 0, 0 };
        private CboType type = 0;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("tableName")]
        [DisplayName("Type")]
        public CboType Cbo_Type
        {
            get => type;
            set => type = value;
        }
        public enum CboType
        {
            Default,
            店舗ストア,
            倉庫種別,
            部門,
            メニュー,
            処理権限,
            店舗権限,
            役職,
            データ種別,
            貨幣金種名,
            両替元額,
            店舗名,
            金種名,
            倉庫,
            入金方法,
            入荷倉庫
        }

        private int length = 10;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set Max Length")]
        [DisplayName("MaxLength")]
        public int Length
        {
            get { return length; }
            set
            {
                length = value;
                CalculateWidth();
            }
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("it will set max item inside dropdown!  ")]
        [DisplayName("MaxDropDownItems")]
        public int MaxItem
        {
            get { return MaxDropDownItems; }
            set
            {
                MaxDropDownItems = value;
            }
        }
        private int Iheight = 26;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("it will set Item height")]
        [DisplayName("ItemHeight")]
        public int ItemHeight_
        {
            get { return Iheight; }
            set
            {
                Iheight = value;
                CalculateHeight();
            }
        }


        private Bytes CtrlByte { get; set; }
        public enum Bytes
        {
            半角 = 0,
            半全角 = 1
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Full width or Half width")]
        [DisplayName("Character Type")]
        public Bytes Ctrl_Byte
        {
            get { return CtrlByte; }
            set
            {
                CtrlByte = value;
                CalculateWidth();
            }
        }
        public float FontSize = 26;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Font Size")]
        [DisplayName("Font Size")]
        public float FontSize_
        {
            get { return FontSize; }
            set { FontSize = value; AddSize(); }

        }


        public Align Alignment { get; set; }
        public enum Align
        {
            //Right = 0,
            //Left = 1,
            //Top = 2,
            //Bottom = 3,
            //Center = 4
            center = 0,
            left = 1,
            right = 2
        }

        public Align cboalign
        {
            get { return Alignment;
            }
            set { Alignment = value;
                PutAlignment();
            }

        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("TextAlign")]
        [DisplayName("TextAlign")]
        public Align ComboAlign
        {
            get { return Alignment; }
            set { Alignment = value;

            }

        }

        protected void AddSize()
        {
            Font = new Font("MS Gothic", FontSize_, FontStyle.Regular);
        }
        private bool IsRequire { get; set; } = false;

        public bool MoveNext { get; set; } = true;
        Base_BL bbl;

        public void PutAlignment()
        {
            // StringAlignment prevalue;
            if (Alignment == Align.center)
            {
                sa = StringAlignment.Center;

            }
            else if (Alignment == Align.left)
            {
                sa = StringAlignment.Near;
            }
            else if (Alignment == Align.right)
            {
                sa = StringAlignment.Far;
            }

        }

        public CKMShop_ComboBox()
        {
            bbl = new Base_BL();
            AutoCompleteMode = AutoCompleteMode.Append;
            AutoCompleteSource = AutoCompleteSource.ListItems;
            DrawMode = DrawMode.OwnerDrawFixed;
            IsFocus = false;
            sf = new StringFormat();
            sf.Alignment = sa;
            DrawItem += new DrawItemEventHandler(EnableDisplayCombo_DrawItem);
          //  UpdateData = DataSource;

        }
        public StringFormat sf;
        public StringAlignment sa;
        protected override void OnDrawItem(DrawItemEventArgs e)
        {

            base.OnDrawItem(e);
        }

        //sf.LineAlignment = StringAlignment.Center;

        public void EnableDisplayCombo_DrawItem(object sender, DrawItemEventArgs e)
        {
            System.Drawing.Graphics g = e.Graphics;
            Rectangle r = e.Bounds;

            if (e.Index >= 0)
            {

                //sf.Alignment = StringAlignment.Center;
                string label = string.Empty; ;
                if (Items[e.Index] is DataRowView drv)
                    label = drv.Row[1].ToString();

                // This is how we draw a disabled control
                if (e.State == (DrawItemState.Disabled | DrawItemState.NoAccelerator | DrawItemState.NoFocusRect | DrawItemState.ComboBoxEdit))
                {
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), r);
                    g.DrawString(label, e.Font, Brushes.Black, r, sf);
                    e.DrawFocusRectangle();
                }
                // This is how we draw the items in an enabled control that aren't in focus
                else if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                    g.DrawString(label, e.Font, Brushes.Black, r, sf);
                    e.DrawFocusRectangle();
                }
                // This is how we draw the focused items
                else if (e.State == (DrawItemState.NoAccelerator))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                    g.DrawString(label, e.Font, Brushes.Black, r, sf);
                    e.DrawFocusRectangle();
                }
                else
                {
                    if (IsFocus)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 242, 204)), r);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(226, 239, 218)), r);

                    }
                    g.DrawString(label, e.Font, Brushes.Black, r, sf);
                    e.DrawFocusRectangle();
                    this.Cursor = Cursors.Hand;
                }
            }
            g.Dispose();



        }

        protected override void OnEnter(EventArgs e)
        {
            this.BackColor = Color.FromArgb(255, 242, 204);
            base.OnEnter(e);
        }
        protected override void OnGotFocus(EventArgs e)
        {
            //DataBindings[0].ReadValue();
            //RefreshItems();
            //Refresh();
            this.BackColor = Color.FromArgb(255, 242, 204);
            base.OnGotFocus(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            this.BackColor = Color.FromArgb(226, 239, 218);
            base.OnLeave(e);
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (!Enabled)
                this.BackColor = Color.White;
            else
                this.BackColor = Color.FromArgb(226, 239, 218);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (IsRequire && string.IsNullOrWhiteSpace(Text))
                {
                    ShowErrorMessage("E102");
                    MoveNext = false;
                    return;
                }
                else
                {
                    MoveNext = true;

                    base.OnKeyDown(e);
                }
            }
            else if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.Up || e.KeyCode == Keys.Right || e.KeyCode == Keys.Left || e.KeyCode == Keys.Enter) || (e.KeyCode != Keys.Delete))
            {
                e.Handled = false;
                return;
            }
            else
            {
                e.Handled = true;
                return;
            }
        }
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)

       {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                    e.IsInputKey = true;
                    break;
            }
            var dt = (UpdateData as DataTable); // Added logic for cant able to edit by ptk
                                                //RefreshItems();
                                                //Refresh();
                                                //Update();
                
            if (e.IsInputKey)
            {

                //var t = Text;
                ////Dispose();
                //if (SelectedIndex == 0 || SelectedIndex == -1)
                //{
                //    if (string.IsNullOrWhiteSpace(t))
                //    {
                //        if (e.KeyCode == Keys.Down)
                //        {
                //         //   SelectedValue = 1;
                //        }
                //    }
                //    // (Convert.ToInt32(dt.Select("Char1 = '" + Text + "'").CopyToDataTable().Rows[0]["Key"].ToString()) + 1)
                //}   
                //else
                //{
                //    if (e.KeyCode == Keys.Down)
                //    {
                //        if (SelectedIndex == dt.Rows.Count-1)   // Reduce for 0
                //        {
                //            SelectedIndex = SelectedIndex;
                //        }
                //        else
                //        {
                //            SelectedIndex += 1;
                //        }
                //    }
                //    else // Key Up
                //    {
                //        SelectedIndex -= 1;
                //    }

                //}
                //SelectedIndex = 1;
              //  Update();
            }

        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Enter && e.KeyChar !=  (char)Keys.Up  && e.KeyChar != (char)Keys.Down) || (e.KeyChar == (Char)Keys.Delete))
            {
                e.Handled = true;
                return;
            }
        }
        private void ShowErrorMessage(string messageID)
        {
            bbl.ShowMessage(messageID);
            MoveNext = false;
            this.SelectionStart = 0;
            this.SelectionLength = this.Text.Length;
        }
        private void CalculateWidth()
        {
            int divider = CtrlByte == Bytes.半全角 ? 2 : 1;
            MaxLength = length / divider;
            
            int l1 = Ctrl_Byte == Bytes.半角 ? 9 : 14;
            Width = (l1 * Length) / divider;
        }

        private void CalculateHeight()
        {
            this.ItemHeight = Iheight;
            float csize;
          //  DropDownHeight = 10 * ItemHeight;
            //if (((ItemHeight*18 / 24) ).ToString().Contains("."))
            //{
            //    csize = float.Parse(Convert.ToInt32(((ItemHeight  * 18) / 24).ToString().Split('.').First()).ToString());
            //}
            //else
            //{
            //    csize = float.Parse(((ItemHeight*18)/24).ToString()) ;
            //}
            
            
            Font = new Font("Meiryo UI", Font.Size, Font.Style);
        }

        public void Bind(string changeDate, string type = null)
        {
            M_Store_Entity mse = new M_Store_Entity
            {
                ChangeDate = changeDate.Replace("/", "-"),
                DeleteFlg = "0",
                Type = type,
                StoreCD = type
            };
            switch (Cbo_Type)
            {
                case CboType.店舗ストア:
                    Store_BL sbl = new Store_BL();                 
                    DataTable dtStore = sbl.BindStore(mse);
                    BindCombo("StoreCD", "StoreName", dtStore);
                    break;
                case CboType.倉庫種別:
                    MultiPorpose_BL mpbl = new MultiPorpose_BL();
                    M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity();
                    mme.ID = mpbl.ID_Store;
                    DataTable dtSoukoType = mpbl.M_MultiPorpose_SoukoTypeSelect(mme);
                    BindCombo("Key", "IDName", dtSoukoType);
                    break;
                case CboType.部門:
                    Staff_BL staffBL1 = new Staff_BL();
                    DataTable dtBMNCD = staffBL1.BindBMN();
                    BindCombo("Key", "Char1", dtBMNCD);
                    break;
                case CboType.メニュー:
                    Staff_BL staffBL2 = new Staff_BL();
                    DataTable dtMenu = staffBL2.BindMenu();
                    BindCombo("MenuID", "MenuName", dtMenu);
                    break;
                case CboType.処理権限:
                    Staff_BL staffBL3 = new Staff_BL();
                    DataTable dtAuthor = staffBL3.BindAuthorization();
                    BindCombo("AuthorizationsCD", "AuthorizationsName", dtAuthor);
                    break;
                case CboType.店舗権限:
                    Staff_BL staffBL4 = new Staff_BL();
                    DataTable dtStoreAuthor = staffBL4.BindStoreAuthorization();
                    BindCombo("StoreAuthorizationsCD", "StoreAuthorizationsName", dtStoreAuthor);
                    break;
                case CboType.役職:
                    Staff_BL staffBL5 = new Staff_BL();
                    DataTable dtPosition = staffBL5.BindPosition();
                    BindCombo("Key", "Char1", dtPosition);
                    break;
                case CboType.データ種別:
                    MultiPorpose_BL mpBL = new MultiPorpose_BL();
                    DataTable dtKBN = mpBL.BindSeqKBN();
                    BindCombo("Column1", "Char1", dtKBN);
                    break;
                case CboType.貨幣金種名:
                    MultiPorpose_BL mppbl = new MultiPorpose_BL();
                    DataTable dtDenomination = mppbl.BindSeqDepo();
                    BindCombo("Key", "Char1", dtDenomination);
                    break;
                case CboType.両替元額:
                    MultiPorpose_BL mmpbl = new MultiPorpose_BL();
                    DataTable dtExchangeDenomination = mmpbl.BindSeqDepo();
                    BindCombo("Num1", "Char1", dtExchangeDenomination);
                    break;
                case CboType.店舗名:
                    Store_BL storebl = new Store_BL();                 
                    DataTable storetb = storebl.BindData(mse);
                    BindCombo("StoreCD", "StoreName", storetb);
                    break;
                case CboType.金種名:
                    DenominationKBN_BL dbl = new DenominationKBN_BL();
                    DataTable dtdeno = dbl.SimpleSelect1("35");
                    BindCombo("DenominationCD", "DenominationName", dtdeno);
                    break;
                case CboType.倉庫:
                    Search_Souko_BL msbl = new Search_Souko_BL();
                    M_Souko_Entity mske = new M_Souko_Entity();
                    mske.DeleteFlg = "0";
                    //mske.SoukoType = "3"; 3,4の両方へ変更
                    mske.ChangeDate = changeDate;
                    DataTable dtSouko = msbl.M_Souko_Bind(mske);
                    BindCombo("SoukoCD", "SoukoName", dtSouko);
                    break;
                case CboType.入荷倉庫:
                    NyuukaNyuuryoku_BL nnbl = new NyuukaNyuuryoku_BL();
                    M_Souko_Entity msoe = new M_Souko_Entity();
                    msoe.ChangeDate = changeDate;
                    msoe.DeleteFlg = "0";
                    DataTable dtNSouko = nnbl.M_Souko_BindForNyuuka(msoe);
                    BindCombo("SoukoCD", "SoukoName", dtNSouko);
                    break;
            }
        }

        private void BindCombo(string key, string value, DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
           UpdateData= DataSource = dt;
            DisplayMember = value;
            ValueMember = key;
           
        }
        public void Require(bool value)
        {
            IsRequire = value;
        }
        

       
             




    }
}
