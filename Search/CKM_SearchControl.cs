using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;
using System.Data;
using CKM_Controls;

namespace Search
{
    public partial class CKM_SearchControl : UserControl
    {
        public delegate void KeyEventHandler(object sender, KeyEventArgs e);
        string FieldsName = string.Empty;
        string TableName = string.Empty;
        string Condition = string.Empty;
        public string test { get; set; }

        [Browsable(true)]
        [Category("CKM Event")]
        [Description("ChangeDateKeyDownEvent")]
        [DisplayName("ChangeDateKeyDownEvent")]
        public event KeyEventHandler ChangeDateKeyDownEvent;

        [Browsable(true)]
        [Category("CKM Event")]
        [Description("CodeKeyDownEvent")]
        [DisplayName("CodeKeyDownEvent")]
        public event KeyEventHandler CodeKeyDownEvent;

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Is Copy")]
        [DisplayName("Is Copy")]
        public bool IsCopy { get; set; } = false;
        public bool SearchEnable
        {
            get { return btnSearch.Enabled; }
            set { btnSearch.Enabled = value; }
        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Use ChangeDate")]
        [DisplayName("Use ChangeDate")]
        public bool UseChangeDate
        {
            get => txtChangeDate.Visible;
            set => txtChangeDate.Visible = value;
        }

        private FontSize Font_Size { get; set; }  //Added by ptk on 11/15/2019 bcoz of requested  by hesaka san
        public enum FontSize
        {
            Normal,
            Small,
            SmallLarge,
            Medium,
            Large,
            XLarge
        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select FontSize")]
        [DisplayName("Font Size")]

        public FontSize TextSize
        {
            get { return Font_Size; }
            set
            {
                Font_Size = value;
                switch (Font_Size)
                {
                    case FontSize.Normal:
                        Adjust_Size(9F, FontStyle.Regular, CKM_TextBox.FontSize.Normal);
                        break;
                    case FontSize.Small:
                        Adjust_Size(10F, FontStyle.Regular, CKM_TextBox.FontSize.Small);
                        break;
                    case FontSize.SmallLarge:
                        Adjust_Size(14F, FontStyle.Regular, CKM_TextBox.FontSize.SmallLarge);
                        break;
                    case FontSize.Medium:
                        Adjust_Size(16F, FontStyle.Regular, CKM_TextBox.FontSize.Medium);
                        break;
                    case FontSize.Large:
                        Adjust_Size(20F, FontStyle.Regular, CKM_TextBox.FontSize.Large);
                        break;
                    case FontSize.XLarge:
                        Adjust_Size(24F, FontStyle.Regular, CKM_TextBox.FontSize.XLarge);
                        break;
                }
            }
        }

        protected void Adjust_Size(float fsz, FontStyle fst, CKM_Controls.CKM_TextBox.FontSize cfs)
        {
            this.Font = new System.Drawing.Font("MS Gothic", fsz, fst);
            txtCode.TextSize = cfs;
            txtChangeDate.TextSize = cfs;
            if (CKM_Controls.CKM_TextBox.FontSize.Normal != cfs)
            {
                btnSearch.Height = txtCode.Height + 2;
                lblName.Height = txtCode.Height - 1;
            }

        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("ChangeDate Width")]
        [DisplayName("ChangeDate Width")]
        public int ChangeDateWidth
        {
            get => txtChangeDate.Width;
            set => txtChangeDate.Width = value;
        }

        private int CodeWidth_S = 100;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Code Width")]
        [DisplayName("Code Width")]

        public int CodeWidth
        {
            get => txtCode.Width;
            set
            {
                txtCode.Width = value;
                CalculateWidth();
            }
            //get => CodeWidth_S;
            //set {
            //    CodeWidth_S = value;
            //    CalculateWidth();
            //}
        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Code Width1")]
        [DisplayName("Code Width1")]

        public int CodeWidth1
        {
            get => txtCode.Width;
            set
            {
                txtCode.Width = value;
                CalculateWidth();
            }
            //get => CodeWidth_S;
            //set
            //{
            //    CodeWidth_S = value;
            //    CalculateWidth();
            //}
        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Name Width")]
        [DisplayName("Name Width")]
        public int NameWidth
        {
            get => lblName.Width;
            set => lblName.Width = value;

        }



        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Check Code is exists or not when press enter")]
        [DisplayName("DataCheck")]
        public bool DataCheck { get; set; } = false;

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set Label Visible")]
        [DisplayName("Label Visible")]
        public bool LabelVisible
        {
            get => lblName.Visible;
            set
            {
                lblName.Visible = value;
                CalculateWidth();
            }
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select Control Type")]
        [DisplayName("TextBox Type")]
        public CKM_Controls.CKM_TextBox.Type Ctrl_Type
        {
            get => txtCode.Ctrl_Type;
            set => txtCode.Ctrl_Type = value;
        }

        private SearchType Search_Type = 0;
        public enum SearchType
        {
            Default,
            倉庫,
            店舗,
            仕入先,
            仕入先PayeeFlg,
            スタッフ,
            銀行口座,
            モール,
            メール文章,  //2020.03.12 add	
            単位,
            銀行,
            銀行支店,
            //2019.6.15 add---------------->
            ブランド,
            SKU_ITEM_CD,
            単価設定,
            見積番号,
            受注番号,
            受注処理番号,
            発注番号,
            発注処理番号, //2020.07.05 add
            売上番号,
            入金番号,
            入金消込番号,
            入荷番号,
            仕入番号,
            移動番号,
            移動依頼番号,
            出荷指示番号,
            出荷番号,
            倉庫棚番,
            得意先,
            得意先_Detail,

            JANCD,
            JANCD_Detail,   //JANCDだけでなくAdminNOも値を取得したい場合は個々のプログラムで検索を実装
            MakerItem,
            SKUCD,
            競技,
            分類,
            大分類,
            中分類,
            小分類,
            //<----------------2019.6.15 add
            //2019.6.19 add-------------->		
            ID,
            Key,

            //2020.02.21 add by etz
            HanyouKeyStart,
            HanyouKeyEnd,
            //<------------2019.6.19 add
            Shipping,// 2019.06.28
            Supplier, // 2019.07.04
            経費番号,
            支払先,
            Carrier,
            EDI処理番号,
            棚番号,
            ピッキング番号,
            展示会商品,
            //<----------- 2019.12.05
            /*ItemMulti,*/
            ITEMMulti,
            SKUMulti,
            JANMulti,
            //........>
            Location,//---2019.12.09
            支払処理,//---2019-12-19
            支払番号検索 ,//2020-01-27
            プログラムID, //SES
            商品分類,//SES
            展示会名,//Added by SES
            JuchuuNO
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select Control Type")]
        [DisplayName("Control Type")]
        public SearchType Stype
        {
            get { return Search_Type; }
            set
            {
                Search_Type = value;
                SetWidth();
            }
        }
        public string Code
        {
            get => txtCode.Text;
            set => txtCode.Text = value;
        }
        public string LabelText
        {
            get => lblName.Text;
            set => lblName.Text = value;
        }

        public string ChangeDate
        {
            get => txtChangeDate.Text;
            set => txtChangeDate.Text = value;
        }

        public CKM_Controls.CKM_TextBox TxtCode
        { get => txtCode; }

        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }

        public Button BtnSearch
        { get => btnSearch; }

        public CKM_Controls.CKM_TextBox TxtChangeDate
        { get => txtChangeDate; }

        Base_BL bbl;
        Search_BL sbl;
        public CKM_SearchControl()
        {
            InitializeComponent();
            this.AutoSize = true;
            this.ImeMode = ImeMode.Disable;
            bbl = new Base_BL();
            sbl = new Search_BL();
        }

        /// <summary>
        /// F9 search Enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCode_Enter(object sender, EventArgs e)
        {
            if (SearchEnable)
            {
                Control ctrl = this;
                do
                {
                    ctrl = ctrl.Parent;
                } while (!(ctrl is Form));

                if (ctrl is FrmMainForm)
                    ((FrmMainForm)ctrl).F9Visible = true;
                if (ctrl is FrmSubForm)
                    ((FrmSubForm)ctrl).F9Visible = true;
            }
            txtCode.BackColor = Color.FromArgb(255, 242, 204);
        }

        /// <summary>
        /// handle F9 search disable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCode_Leave(object sender, EventArgs e)
        {
            //if (IsConsistFullWidth(txtCode.Text))
            //{
            //    ShowErrorMessage("E221");
            //    txtCode.Focus();
            //    txtCode.MoveNext = false;
            //    return;
            //}
            Control ctrl = this;
            do
            {
                ctrl = ctrl.Parent;
            } while (!(ctrl is Form));

            if (ctrl is FrmMainForm)
            {
                if (((FrmMainForm)ctrl).ActiveControl.Name.Equals("BtnF9"))
                    Search();
                else
                {
                    ((FrmMainForm)ctrl).F9Visible = false;
                    txtCode.BackColor = SystemColors.Window;
                }
            }
            else if (ctrl is FrmSubForm)
            {
                if (((FrmSubForm)ctrl).ActiveControl.Name.Equals("BtnF9"))
                    Search();
                else
                {
                    ((FrmSubForm)ctrl).F9Visible = false;
                    txtCode.BackColor = SystemColors.Window;
                }
            }
        }

        private void SetWidth()
        {


            switch (Search_Type)
            {
                case SearchType.倉庫:
                    txtCode.MaxLength = 6;
                    txtCode.Width = 60;
                    lblName.Width = 280;
                    break;
                case SearchType.仕入先:
                    txtCode.MaxLength = 13;
                    txtCode.Width = 100;
                    lblName.Width = 310;
                    break;
                case SearchType.Supplier:
                    TxtCode.MaxLength = 13;
                    TxtCode.Width = 100;
                    lblName.Width = 320;
                    break;
                 case SearchType.仕入先PayeeFlg:
                    txtCode.MaxLength = 13;
                    txtCode.Width = 100;
                    lblName.Width = 310;
                    break;
                case SearchType.店舗:
                    txtCode.MaxLength = 4;
                    txtCode.Width = 40;
                    lblName.Width = 280;
                    break;
                case SearchType.モール:
                    txtCode.MaxLength = 4;
                    txtCode.Width = 30;
                    lblName.Width = 280;
                    break;
                case SearchType.メール文章:
                    txtCode.MaxLength = 5;
                    txtCode.Width = 50;
                    lblName.Width = 280;
                    break;
                case SearchType.単位:
                    txtCode.MaxLength = 3;
                    txtCode.Width = 30;
                    lblName.Width = 140;
                    break;
                case SearchType.銀行口座:
                    txtCode.MaxLength = 3;
                    txtCode.Width = 30;
                    lblName.Width = 350;
                    break;
                case SearchType.スタッフ:
                    txtCode.MaxLength = 10;
                    txtCode.Width = 70;
                    lblName.Width = 250;
                    break;
                case SearchType.銀行:
                    TxtCode.MaxLength = 4;
                    TxtCode.Width = 40;
                    lblName.Width = 350;
                    break;
                //2019.6.15 add---------------->
                case SearchType.ブランド:
                    txtCode.MaxLength = 6;
                    txtCode.Width = 100;
                    lblName.Width = 280;
                    break;
                case SearchType.SKU_ITEM_CD:
                    txtCode.MaxLength = 30;
                    txtCode.Width = 190;
                    lblName.Width = 350;    //仮
                    break;
                case SearchType.単価設定:
                    txtCode.MaxLength = 13;
                    txtCode.Width = 100;
                    lblName.Width = 140;
                    break;
                case SearchType.見積番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.受注番号:
                case SearchType.受注処理番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.発注番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.発注処理番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.売上番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.入金番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.入金消込番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.入荷番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 150;
                    lblName.Width = 600;
                    break;
                case SearchType.移動依頼番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.出荷指示番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 140;
                    lblName.Width = 600;
                    break;
                case SearchType.出荷番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 140;
                    lblName.Width = 600;
                    break;
                case SearchType.仕入番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.移動番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.得意先:
                case SearchType.得意先_Detail:
                    txtCode.MaxLength = 13;
                    txtCode.Width = 100;
                    lblName.Width = 500;    //400では全桁表示されないので変更
                    break;
                case SearchType.JANCD:
                case SearchType.JANCD_Detail:
                    txtCode.MaxLength = 13;
                    txtCode.Width = 110;
                    lblName.Width = 190;
                    txtCode.Ctrl_Type = CKM_TextBox.Type.Number;
                    break;
                case SearchType.MakerItem:
                    txtCode.MaxLength = 30;
                    txtCode.Width = 190;
                    lblName.Width = 350;
                    break;
                case SearchType.SKUCD:
                    txtCode.MaxLength = 30;
                    txtCode.Width = 190;
                    lblName.Width = 350;
                    break;
                case SearchType.競技:
                    TxtCode.MaxLength = 6;
                    TxtCode.Width = 50;
                    lblName.Width = 280;
                    break;
                case SearchType.分類:
                    TxtCode.MaxLength = 6;
                    TxtCode.Width = 50;
                    lblName.Width = 280;
                    break;
                case SearchType.大分類:
                    TxtCode.MaxLength = 4;
                    TxtCode.Width = 40;
                    lblName.Width = 280;
                    break;
                case SearchType.中分類:
                    TxtCode.MaxLength = 4;
                    TxtCode.Width = 40;
                    lblName.Width = 280;
                    break;
                case SearchType.小分類:
                    TxtCode.MaxLength = 4;
                    TxtCode.Width = 40;
                    lblName.Width = 280;
                    break;
                //<----------------2019.6.15 add
                case SearchType.銀行支店:
                    txtCode.MaxLength = 3;
                    txtCode.Width = 40;
                    lblName.Width = 350;
                    break;
                //2019.6.19 add---------------->		
                case SearchType.ID:
                    txtCode.MaxLength = 3;
                    txtCode.Width = 30;
                    lblName.Width = 650;
                    break;
                case SearchType.Key:
                    txtCode.MaxLength = 50;
                    txtCode.Width = 350;
                    lblName.Width = 300;
                    break;
                //2020.02.21 add by etz
                case SearchType.HanyouKeyStart:
                    txtCode.MaxLength = 50;
                    txtCode.Width = 60;
                    lblName.Width = 350;
                    break;
                case SearchType.HanyouKeyEnd:
                    txtCode.MaxLength = 50;
                    txtCode.Width = 60;
                    lblName.Width = 350;
                    break;
                //<---------------2019.6.19 add
                case SearchType.Shipping:  // 2019.06.28
                    txtCode.MaxLength = 6;
                    txtCode.Width = 60;
                    lblName.Width = 280;
                    break;
                case SearchType.経費番号:  // 2019.06.28
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 280;
                    break;
                case SearchType.支払先:  // 2019.06.28
                    txtCode.MaxLength = 13;
                    txtCode.Width = 120;
                    lblName.Width = 280;
                    break;
                case SearchType.Carrier: //2019.10.09
                    txtCode.MaxLength = 6;
                    txtCode.Width = 60;
                    lblName.Width = 280;
                    break;
                case SearchType.EDI処理番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.棚番号:
                    txtCode.MaxLength = 10;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.ピッキング番号:
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;
                    lblName.Width = 600;
                    break;
                case SearchType.ITEMMulti: //2020/09/24
                    txtCode.MaxLength = 309;
                    txtCode.Width = 600;
                    lblName.Width = 280;
                    break;
                case SearchType.SKUMulti: //2020/09/24
                    txtCode.MaxLength = 309;
                    txtCode.Width = 600;
                    lblName.Width = 280;
                    break;
                case SearchType.JANMulti: //2019.12.05
                    txtCode.MaxLength = 139;
                    txtCode.Width = 600;
                    lblName.Width = 280;
                    break;
                case SearchType.Location: //2019.12.09
                    txtCode.MaxLength = 10;
                    txtCode.Width = 100;
                    lblName.Width = 280;
                    break;
                case SearchType.支払処理: //2019-12-19
                    TxtCode.MaxLength = 11;
                    TxtCode.Width = 110;
                    lblName.Width = 300;
                    break;
                case SearchType.支払番号検索:
                    TxtCode.MaxLength = 11;
                    TxtCode.Width = 110;
                    lblName.Width = 300;
                    break;
                case SearchType.プログラムID:
                    TxtCode.MaxLength = 100;
                    TxtCode.Width = 750;
                    lblName.Width = 300;
                    break;

                case SearchType.商品分類://SES
                    TxtCode.MaxLength = 5;
                    TxtCode.Width = 60;
                    lblName.Width = 250;

                    break;

                case SearchType.展示会名://ses 9/7/2020
                    TxtCode.MaxLength = 80;
                    TxtCode.Ctrl_Byte = CKM_TextBox.Bytes.半全角;
                    TxtCode.Width = 480;
                    lblName.Width = 180;
                    break;

                case SearchType.展示会商品:
                    TxtCode.MaxLength = 80;
                    TxtCode.Ctrl_Byte = CKM_TextBox.Bytes.半全角;
                    TxtCode.Width = 500;
                    lblName.Width = 180;

                    break;
                case SearchType.JuchuuNO: // Pyoung Gyi Max Length>11/ txtWidth>100 Change htar tal // by PTK
                    txtCode.MaxLength = 11;
                    txtCode.Width = 100;                
                    break;
            }
            //}
            //else if (System.Diagnostics.Debugger.IsAttached)
            //{


            //}
            TxtCode.Length = TxtCode.MaxLength; //2019.08.28 add
            CalculateWidth();
        }

        /// <summary>
        /// calculate usercontrol width
        /// </summary>
        private void CalculateWidth()
        {
            txtCode.Width = CodeWidth;
            btnSearch.Location = new Point(txtCode.Width - 1, btnSearch.Location.Y);
            lblName.Location = new Point(txtCode.Width + btnSearch.Width - 2, lblName.Location.Y);

            if (lblName.Visible)
                this.Width = txtCode.Width + btnSearch.Width + lblName.Width;
            else
                this.Width = txtCode.Width + btnSearch.Width;
        }

        /// <summary>
        /// calculate code and name width
        /// </summary>

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        public static bool IsConsistFullWidth(string txt)
        {
            var c = txt.ToCharArray();
            foreach (char chr in c)
            {
                if (System.Text.Encoding.GetEncoding("shift_JIS").GetByteCount(chr.ToString()) == 2)
                {
                    return true;
                }

            }
            return false;
        }
        private void ShowErrorMessage(string messageID)
        {
            bbl.ShowMessage(messageID);
        }
        /// <summary>
        /// change focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (this.TxtCode.Ctrl_Byte != CKM_TextBox.Bytes.半全角)
                if (IsConsistFullWidth(txtCode.Text))
                {
                    ShowErrorMessage("E221");
                    txtCode.Focus();
                    txtCode.MoveNext = false;
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCode.Text))
                    lblName.Text = string.Empty;


                CodeKeyDownEvent?.Invoke(this, e);
                txtChangeDate.Focus();
                CheckData();

            }
            else if (e.KeyCode == Keys.F9 && btnSearch.Enabled == true)
            {
                Search();
            }
        }

        /// <summary>
        /// To Check ',' code in Muliti textbox of JANCD,ITEM ,SKU
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            switch (Stype)
            {
                //case SearchType.ITEMMulti:
                //case SearchType.SKUMulti:
                case SearchType.JANMulti:
                    if (TxtCode.Text.Contains(","))
                    {
                        string a = TxtCode.Text;
                        string[] arr = a.Split(',');
                        if (arr.Length >= 10)
                        {
                            bbl.ShowMessage("E187");
                            TxtCode.Focus();
                            return false;
                        }
                    }
                    break;
                case SearchType.ITEMMulti:
                    if (TxtCode.Text.Contains(","))
                    {
                        string a = TxtCode.Text;
                        string[] arr = a.Split(',');
                        if (arr.Length >= 10)
                        {
                            bbl.ShowMessage("E187");
                            TxtCode.Focus();
                            return false;
                        }
                    }
                    break;
                case SearchType.SKUMulti:
                    if (TxtCode.Text.Contains(","))
                    {
                        string a = TxtCode.Text;
                        string[] arr = a.Split(',');
                        if (arr.Length >= 10)
                        {
                            bbl.ShowMessage("E187");
                            TxtCode.Focus();
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// SetFocus
        /// </summary>
        /// <param name="index">1 = txtCode, 2 = txtChangeDate</param>
        public void SetFocus(int index)
        {
            if (index == 1)
                txtCode.Focus();
            else
                txtChangeDate.Focus();
        }
      
        /// <summary>
        /// Change Date KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtChangeDate_KeyDown(object sender, KeyEventArgs e)
        
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                
                if (!string.IsNullOrEmpty(txtChangeDate.Text) && !txtChangeDate.IsFirstTime)
                {
                    if (txtChangeDate.IsCorrectDate && DateCheck(txtChangeDate.Text))
                        ChangeDateKeyDownEvent?.Invoke(this, e);
                }
                else if (txtChangeDate.IsCorrectDate)
                {
                    ChangeDateKeyDownEvent?.Invoke(this, e);
                }
            }
        }
        private bool DateCheck(string Text1 = null)
        {
            bbl = new Base_BL();
            if (!string.IsNullOrWhiteSpace(Text1))
            {
                if (bbl.IsInteger(Text1.Replace("/", "").Replace("-", "")))
                {
                    string day = string.Empty, month = string.Empty, year = string.Empty;
                    if (Text1.Contains("/"))
                    {
                        string[] date = Text1.Split('/');
                        day = date[date.Length - 1].PadLeft(2, '0');
                        month = date[date.Length - 2].PadLeft(2, '0');

                        if (date.Length > 2)
                            year = date[date.Length - 3];

                        Text1 = year + month + day;//  this.Text.Replace("/", "");
                    }
                    else if (Text1.Contains("-"))
                    {
                        string[] date = Text1.Split('-');
                        day = date[date.Length - 1].PadLeft(2, '0');
                        month = date[date.Length - 2].PadLeft(2, '0');

                        if (date.Length > 2)
                            year = date[date.Length - 3];

                        Text1 = year + month + day;//  this.Text.Replace("-", "");
                    }

                    string text = Text1;
                    text = text.PadLeft(8, '0');
                    day = text.Substring(text.Length - 2);
                    month = text.Substring(text.Length - 4).Substring(0, 2);
                    year = Convert.ToInt32(text.Substring(0, text.Length - 4)).ToString();

                    if (month == "00")
                    {
                        month = string.Empty;
                    }
                    if (year == "0")
                    {
                        year = string.Empty;
                    }

                    if (string.IsNullOrWhiteSpace(month))
                        month = DateTime.Now.Month.ToString().PadLeft(2, '0');//if user doesn't input for month,set current month

                    if (string.IsNullOrWhiteSpace(year))
                    {
                        year = DateTime.Now.Year.ToString();//if user doesn't input for year,set current year
                    }
                    else
                    {
                        if (year.Length == 1)
                            year = "200" + year;
                        else if (year.Length == 2)
                            year = "20" + year;
                    }

                    //string strdate = year + "-" + month + "-" + day;  2019.6.11 chg
                    string strdate = year + "/" + month + "/" + day;
                    if (bbl.CheckDate(strdate))
                    {
                        //   IsCorrectDate = true;
                        Text1 = strdate;
                      //  Datetemp = strdate;
                    }
                    else
                    {
                       /// Datetemp = "";
                        //  ShowErrorMessage("E103");
                        return false;
                    }
                }
                else
                {
                  ///  Datetemp = "";
                    //  ShowErrorMessage("E103");
                    return false;
                }
            }

            return true;
        }
        public void Clear()
        {
            Code = LabelText = ChangeDate = string.Empty;
        }

        private void Search()
        {
            //string changedate = string.IsNullOrWhiteSpace(txtChangeDate.Text) ? bbl.GetDate().Replace("/","-") : txtChangeDate.Text;  2019.6.11 chg
            string changedate = string.IsNullOrWhiteSpace(txtChangeDate.Text) ? bbl.GetDate() : txtChangeDate.Text;
            switch (Stype)
            {
                case SearchType.倉庫:
                    FrmSearch_Souko frmss = new FrmSearch_Souko(changedate);
                    frmss.ShowDialog();
                    if (!frmss.flgCancel)
                    {
                        txtCode.Text = frmss.SoukoCD;
                        lblName.Text = frmss.SoukoName;
                        txtChangeDate.Text = frmss.ChangeDate;

                        CheckBasedFormPanel();//PTK added

                    }
                    break;
                case SearchType.JuchuuNO:
                    Search_TenzikaiJuchuuNO juchuu = new Search_TenzikaiJuchuuNO();
                    juchuu.ShowDialog();
                    if (!juchuu.flgCancel)
                    {
                        txtCode.Text = juchuu.OrderNum;
                        
                        CheckBasedFormPanel();//PTK added

                    }
                    break;
                case SearchType.店舗:
                    using (Search_Store frmStore = new Search_Store())
                    {
                        frmStore.parChangeDate = changedate;
                        frmStore.ShowDialog();

                        if (!frmStore.flgCancel)
                        {
                            txtCode.Text = frmStore.parStoreCD;
                            lblName.Text = frmStore.parStoreName;
                            txtChangeDate.Text = frmStore.parChangeDate;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.仕入先:
                    using (Search_Vendor frmVendor = new Search_Vendor(changedate,Value1))
                    {
                        frmVendor.parChangeDate = changedate;
                        frmVendor.ShowDialog();
                        if (!frmVendor.flgCancel)
                        {
                            txtCode.Text = frmVendor.VendorCD;
                            lblName.Text = frmVendor.VendorName;
                            if (UseChangeDate == true)
                                txtChangeDate.Text = frmVendor.ChangeDate;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.仕入先PayeeFlg:
                    using (Search_Vendor frmVendor = new Search_Vendor(changedate, Value1))
                    {
                        frmVendor.parChangeDate = changedate;
                        frmVendor.ShowDialog();
                        if (!frmVendor.flgCancel)
                        {
                            txtCode.Text = frmVendor.VendorCD;
                            lblName.Text = frmVendor.VendorName;
                            if (UseChangeDate == true)
                                txtChangeDate.Text = frmVendor.ChangeDate;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.スタッフ:
                    using (Search_Staff frmStaff = new Search_Staff())
                    {
                        frmStaff.parChangeDate = changedate;
                        frmStaff.ShowDialog();
                        if (!frmStaff.flgCancel)
                        {
                            txtCode.Text = frmStaff.parStaffCD;
                            lblName.Text = frmStaff.parStaffName;
                            if (UseChangeDate == true)
                                txtChangeDate.Text = frmStaff.parChangeDate;

                            CheckBasedFormPanel();//PTK added
                        }                          
                    }
                    break;
                case SearchType.銀行口座:
                    using (Search_Kouza frmKouza = new Search_Kouza())
                    {
                        frmKouza.parChangeDate = changedate;
                        frmKouza.ShowDialog();
                        if (!frmKouza.flgCancel)
                        {
                            txtCode.Text = frmKouza.parKouzaCD;
                            txtChangeDate.Text = frmKouza.parChangeDate;
                            lblName.Text = frmKouza.parKouzaName;
                            CheckBasedFormPanel();//PTK added
                        }

                    }
                    break;
                case SearchType.モール:
                    using (Search_Mall frmMal = new Search_Mall())
                    {
                        frmMal.parID = MultiPorpose_BL.ID_MALL;
                        frmMal.ShowDialog();
                        if (!frmMal.flgCancel)
                        {
                            txtCode.Text = frmMal.parKey;
                            lblName.Text = frmMal.parChar1;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.メール文章:
                    using (Search_MailPattern frmMail = new Search_MailPattern())
                    {
                        frmMail.ShowDialog();
                        if (!frmMail.flgCancel)
                        {
                            txtCode.Text = frmMail.parMailPatternCD;
                            lblName.Text = frmMail.parMailPatternName;
                            CheckBasedFormPanel();//PTK added__
                        }
                    }
                    break;
                case SearchType.単位:
                case SearchType.競技:
                case SearchType.分類:
                    using (Search_HanyouKey frmMulti = new Search_HanyouKey())
                    {
                        frmMulti.parID = Value1;
                        frmMulti.ShowDialog();
                        if (!frmMulti.flgCancel)
                        {
                            txtCode.Text = frmMulti.parKey;
                            lblName.Text = frmMulti.parChar1;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;

                case SearchType.商品分類:
                    using (Search_HanyouKey frmMulti = new Search_HanyouKey())
                    {
                        frmMulti.parID = Value1;
                        frmMulti.ShowDialog();
                        if (!frmMulti.flgCancel)
                        {
                            txtCode.Text = frmMulti.parKey;
                            lblName.Text = frmMulti.parChar1;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;

                case SearchType.銀行:
                    using (FrmSearch_Ginkou frmGinkou = new FrmSearch_Ginkou(changedate))
                    {
                        frmGinkou.ShowDialog();
                        if (!frmGinkou.flgCancel)
                        {
                            txtCode.Text = frmGinkou.BankCD;
                            lblName.Text = frmGinkou.BankName;
                            if (UseChangeDate == true)
                                txtChangeDate.Text = frmGinkou.ChangeDate;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;

                //2019.6.11 add---------------------------------------------------->
                case SearchType.ブランド:
                    using (Search_Brand frmBrand = new Search_Brand())
                    {
                        if (UseChangeDate == true)
                            frmBrand.parChangeDate = changedate;
                        frmBrand.ShowDialog();
                        if (!frmBrand.flgCancel)
                        {
                            txtCode.Text = frmBrand.parBrandCD;
                            lblName.Text = frmBrand.parBrandName;
                            CheckBasedFormPanel();//PTK added
                        }

                    }
                    break;

                //2020.03.23 add by ses---
                case SearchType.プログラムID:
                    using (Search_Program frmProgram = new Search_Program(Value1))
                    {
                        frmProgram.ShowDialog();
                        if (!frmProgram.flgCancel)
                        {
                            txtCode.Text = frmProgram.ProgramID;

                            CheckBasedFormPanel();//PTK added
                        }
                        break;
                    }
                case SearchType.SKU_ITEM_CD:
                    using (Search_Product frmItemCD = new Search_Product(changedate))
                    {
                        frmItemCD.Mode = Value1 == null ? "1" : Value1;
                        frmItemCD.SKUCD = txtCode.Text;
                        frmItemCD.ShowDialog();
                        if (!frmItemCD.flgCancel)
                        {
                            if (frmItemCD.Mode.Equals("1"))
                                txtCode.Text = frmItemCD.ITEM;
                            else if (frmItemCD.Mode.Equals("2"))
                                txtCode.Text = frmItemCD.SKUCD;

                            if (UseChangeDate == true)
                                txtChangeDate.Text = frmItemCD.ChangeDate;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.単価設定:
                    using (Search_TankaSettei frmTankaSettei = new Search_TankaSettei())
                    {
                        frmTankaSettei.parChangeDate = changedate;
                        frmTankaSettei.ShowDialog();

                        if (!frmTankaSettei.flgCancel)
                        {
                            txtCode.Text = frmTankaSettei.parTankaCD;
                            lblName.Text = frmTankaSettei.parTankaName;
                            txtChangeDate.Text = frmTankaSettei.parChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.見積番号:
                    using (Search_Mitsumori frmMitsumori = new Search_Mitsumori(changedate))
                    {
                        frmMitsumori.OperatorCD = Value1;
                        frmMitsumori.AllAvailableStores = Value2;
                        frmMitsumori.ShowDialog();

                        if (!frmMitsumori.flgCancel)
                        {
                            txtCode.Text = frmMitsumori.MitsumoriNo;
                            lblName.Text = frmMitsumori.MitsumoriName;
                            txtChangeDate.Text = frmMitsumori.ChangeDate;
                            CheckBasedFormPanel(); //Added by PTK 
                        }
                    }
                    break;
                case SearchType.受注番号:   
                    using (Search_JuchuuNO frmJuchuu = new Search_JuchuuNO(changedate))
                    {
                        frmJuchuu.OperatorCD = Value1;
                        frmJuchuu.AllAvailableStores = Value2;
                        frmJuchuu.ShowDialog();

                        if (!frmJuchuu.flgCancel)
                        {
                            txtCode.Text = frmJuchuu.JuchuuNO;
                            txtChangeDate.Text = frmJuchuu.ChangeDate;
                            CheckBasedFormPanel(); //Added by PTK

                        }
                    }
                    break;
                case SearchType.受注処理番号:
                    using (Search_JuchuuProcessNO frmJuchuu = new Search_JuchuuProcessNO(changedate))
                    {
                        frmJuchuu.OperatorCD = Value1;
                        frmJuchuu.AllAvailableStores = Value2;
                        frmJuchuu.ShowDialog();

                        if (!frmJuchuu.flgCancel)
                        {
                            txtCode.Text = frmJuchuu.JuchuuProcessNO;
                            txtChangeDate.Text = frmJuchuu.ChangeDate;
                            CheckBasedFormPanel(); //Added by PTK

                        }
                    }
                    break;
                case SearchType.発注番号:
                    using (Search_HacchuuNO frmHacchuu = new Search_HacchuuNO(changedate))
                    {
                        frmHacchuu.OperatorCD = Value1;
                        frmHacchuu.AllAvailableStores = Value2;
                        frmHacchuu.ShowDialog();

                        if (!frmHacchuu.flgCancel)
                        {
                            txtCode.Text = frmHacchuu.OrderNO;
                            txtChangeDate.Text = frmHacchuu.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.発注処理番号:
                    using (Search_HacchuuShoriNO frmHacchuu = new Search_HacchuuShoriNO(changedate))
                    {
                        frmHacchuu.OperatorCD = Value1;
                        frmHacchuu.AllAvailableStores = Value2;
                        frmHacchuu.storeCD = Value3;
                        frmHacchuu.ShowDialog();

                        if (!frmHacchuu.flgCancel)
                        {
                            txtCode.Text = frmHacchuu.HacchuuShoriNO;
                            txtChangeDate.Text = frmHacchuu.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.売上番号:
                    using (Search_TempoUriageNO frmUriage = new Search_TempoUriageNO(changedate))
                    {
                        frmUriage.OperatorCD = Value1;
                        frmUriage.AllAvailableStores = Value2;
                        frmUriage.ShowDialog();

                        if (!frmUriage.flgCancel)
                        {
                            txtCode.Text = frmUriage.SalesNo;
                            txtChangeDate.Text = frmUriage.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.入金番号:
                    using (Search_CollectNO frmNyukin = new Search_CollectNO(changedate))
                    {
                        frmNyukin.OperatorCD = Value1;
                        frmNyukin.AllAvailableStores = Value2;
                        frmNyukin.ShowDialog();
                        if (!frmNyukin.flgCancel)
                        {
                            txtCode.Text = frmNyukin.CollectNO;
                            txtChangeDate.Text = frmNyukin.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.入金消込番号:
                    using (Search_ConfirmNO frmKeshikomi = new Search_ConfirmNO(changedate))
                    {
                        frmKeshikomi.ShowDialog();
                        if (!frmKeshikomi.flgCancel)
                        {
                            txtCode.Text = frmKeshikomi.ConfirmNO;
                            txtChangeDate.Text = frmKeshikomi.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.入荷番号:
                    using (Search_ArrivalNO frmNyuka = new Search_ArrivalNO(changedate))
                    {
                        frmNyuka.OperatorCD = Value1;
                        frmNyuka.AllAvailableStores = Value2;
                        frmNyuka.SoukoCD = Value3;
                        frmNyuka.ShowDialog();
                        if (!frmNyuka.flgCancel)
                        {
                            txtCode.Text = frmNyuka.ArrivalNO;
                            txtChangeDate.Text = frmNyuka.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.仕入番号:
                    using (Search_ShiireNO frmShiire = new Search_ShiireNO(changedate))
                    {
                        frmShiire.OperatorCD = Value1;
                        frmShiire.AllAvailableStores = Value2;
                        frmShiire.ShowDialog();
                        if (!frmShiire.flgCancel)
                        {
                            txtCode.Text = frmShiire.PurchaseNO;
                            txtChangeDate.Text = frmShiire.ChangeDate;
                            CheckBasedFormPanel();
                        }
                    }
                    break;
                case SearchType.移動番号:
                    using (Search_ZaikoIdouNO frmIdo = new Search_ZaikoIdouNO(changedate))
                    {
                        frmIdo.OperatorCD = Value1;
                        frmIdo.AllAvailableStores = Value2;
                        frmIdo.ShowDialog();
                        if (!frmIdo.flgCancel)
                        {
                            txtCode.Text = frmIdo.MoveNO;
                            txtChangeDate.Text = frmIdo.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.移動依頼番号:
                    using (Search_ZaikoIdouIraiNo frmIdouIrai = new Search_ZaikoIdouIraiNo(changedate))
                    {
                        frmIdouIrai.OperatorCD = Value1;
                        frmIdouIrai.AllAvailableStores = Value2;
                        frmIdouIrai.ShowDialog();
                        if (!frmIdouIrai.flgCancel)
                        {
                            txtCode.Text = frmIdouIrai.RequestNO;
                            txtChangeDate.Text = frmIdouIrai.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                    case SearchType.出荷指示番号:
                    using (Search_InstructionNO frmInst = new Search_InstructionNO(changedate))
                    {
                        frmInst.OperatorCD = Value1;
                        frmInst.AllAvailableStores = Value2;
                        frmInst.ShowDialog();
                        if (!frmInst.flgCancel)
                        {
                            txtCode.Text = frmInst.InstructionNO;
                            txtChangeDate.Text = frmInst.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.出荷番号:
                    using (Search_ShippingNO frmShukka = new Search_ShippingNO(changedate))
                    {
                        frmShukka.OperatorCD = Value1;
                        frmShukka.AllAvailableStores = Value2;
                        frmShukka.ShowDialog();
                        if (!frmShukka.flgCancel)
                        {
                            txtCode.Text = frmShukka.ShippingNO;
                            txtChangeDate.Text = frmShukka.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.倉庫棚番:
                    using (Search_ShiireNO frmShiire = new Search_ShiireNO(changedate)) //TOdo:倉庫棚番検索へ　変更
                    {
                        frmShiire.OperatorCD = Value1;
                        frmShiire.AllAvailableStores = Value2;
                        frmShiire.ShowDialog();
                        if (!frmShiire.flgCancel)
                        {
                            txtCode.Text = frmShiire.PurchaseNO;
                            txtChangeDate.Text = frmShiire.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.棚番号:
                    using (Search_Location frmLocation = new Search_Location(changedate))
                    {
                        frmLocation.ChangeDate = changedate;
                        frmLocation.SoukoCD = Value1;
                        frmLocation.ShowDialog();
                        if (!frmLocation.flgCancel)
                        {
                            txtCode.Text = frmLocation.TanaCD;
                            txtChangeDate.Text = frmLocation.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.ピッキング番号:
                    using (Search_PickingNO frmPickingNO = new Search_PickingNO(changedate))
                    {
                        frmPickingNO.ChangeDate = changedate;
                        frmPickingNO.SoukoCD = Value1;
                        frmPickingNO.ShowDialog();
                        if (!frmPickingNO.flgCancel)
                        {
                            txtCode.Text = frmPickingNO.PickingNO;
                            txtChangeDate.Text = frmPickingNO.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.得意先:
                    using (FrmSearch_Customer frmCustomer = new FrmSearch_Customer(changedate,Value1,Value2))
                    {
                       
                        frmCustomer.parChangeDate = changedate;
                        frmCustomer.ShowDialog();

                        if (!frmCustomer.flgCancel)
                        {
                            txtCode.Text = frmCustomer.CustomerCD;
                            lblName.Text = frmCustomer.CustName;
                            if (UseChangeDate == true)
                                txtChangeDate.Text = frmCustomer.ChangeDate;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;

                case SearchType.JANCD:
                    using (Search_Product frmJanCD = new Search_Product(changedate))
                    {
                        frmJanCD.Mode = "4";
                        frmJanCD.JANCD = txtCode.Text;
                        frmJanCD.ShowDialog();

                        if (!frmJanCD.flgCancel)
                        {
                            txtCode.Text = frmJanCD.JANCD;
                            txtChangeDate.Text = frmJanCD.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;

                case SearchType.MakerItem:
                    using (Search_Product frmMakerItem = new Search_Product(changedate))
                    {
                        frmMakerItem.Mode = "3";
                        frmMakerItem.MakerItem = txtCode.Text;
                        frmMakerItem.ShowDialog();
                        if (!frmMakerItem.flgCancel)
                        {
                            txtCode.Text = frmMakerItem.MakerItem;
                            txtChangeDate.Text = frmMakerItem.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;

                case SearchType.SKUCD:
                    using (Search_Product frmSKUCD = new Search_Product(changedate))
                    {
                        frmSKUCD.Mode = "2";
                        frmSKUCD.SKUCD = txtCode.Text;
                        frmSKUCD.ShowDialog();

                        if (!frmSKUCD.flgCancel)
                        {
                            txtCode.Text = frmSKUCD.SKUCD;
                            txtChangeDate.Text = frmSKUCD.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;

                //case SearchType.競技:
                //    using (Search_Mall frmMal = new Search_Mall())
                //    {
                //        frmMal.parID = MultiPorpose_BL.ID_MALL;
                //        frmMal.ShowDialog();
                //        if (!frmMal.flgCancel)
                //        {
                //            txtCode.Text = frmMal.parKey;
                //            lblName.Text = frmMal.parChar1;
                //        }
                //    }
                //    break;
                case SearchType.大分類:
                    using (Search_Mall frmMal = new Search_Mall())
                    {
                        frmMal.parID = MultiPorpose_BL.ID_MALL;
                        frmMal.ShowDialog();
                        if (!frmMal.flgCancel)
                        {
                            txtCode.Text = frmMal.parKey;
                            lblName.Text = frmMal.parChar1;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.中分類:
                    using (Search_Mall frmMal = new Search_Mall())
                    {
                        frmMal.parID = MultiPorpose_BL.ID_MALL;
                        frmMal.ShowDialog();
                        if (!frmMal.flgCancel)
                        {
                            txtCode.Text = frmMal.parKey;
                            lblName.Text = frmMal.parChar1;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.小分類:
                    using (Search_Mall frmMal = new Search_Mall())
                    {
                        frmMal.parID = MultiPorpose_BL.ID_MALL;
                        frmMal.ShowDialog();
                        if (!frmMal.flgCancel)
                        {
                            txtCode.Text = frmMal.parKey;
                            lblName.Text = frmMal.parChar1;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                //<----------------------------------------------------2019.6.11 add
                case SearchType.銀行支店:
                    using (FrmSearch_GinkouShiten frmGinkouShiten = new FrmSearch_GinkouShiten(changedate, Value1, Value2))
                    {
                        frmGinkouShiten.ShowDialog();

                        if (!frmGinkouShiten.flgCancel)
                        {
                            txtCode.Text = frmGinkouShiten.BranchCD;
                            lblName.Text = frmGinkouShiten.BranchName;
                            if (UseChangeDate == true)
                                txtChangeDate.Text = frmGinkouShiten.ChangeDate;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.ID:
                    using (Search_ID frmID = new Search_ID()) 
                    {
                        frmID.ShowDialog();
                        if (!frmID.flgCancel)
                        {
                            TxtCode.Text = frmID.ID;
                            lblName.Text = frmID.IDName;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.Key:
                    using (Search_Key frmKey = new Search_Key(Value1, Value2))
                    {
                        frmKey.ShowDialog();
                        if (!frmKey.flgCancel)
                        {
                            TxtCode.Text = frmKey.KeyCode;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                //2020.02.21 add by etz
                case SearchType.HanyouKeyStart:
                    using (Search_Key frmKey = new Search_Key(Value1, Value2))
                    {
                        frmKey.ShowDialog();
                        if (!frmKey.flgCancel)
                        {
                            TxtCode.Text = frmKey.KeyCode;
                            lblName.Text = frmKey.Char1;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.HanyouKeyEnd:
                    using (Search_Key frmKey = new Search_Key(Value1, Value2))
                    {
                        frmKey.ShowDialog();
                        if (!frmKey.flgCancel)
                        {
                            TxtCode.Text = frmKey.Char2;    //補助科目CD
                            lblName.Text = frmKey.Char3;    //補助科目名
                            Value3 = frmKey.Char1;  //勘定科目CD
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                //case SearchType.Shipping: // 2019.06.28 add
                //    using (Search_Shipping frmshipping = new Search_Shipping())
                //    {
                //        frmshipping.parChangeDate = changedate;
                //        //frmBrand.parMakerCD = 
                //        frmshipping.ShowDialog();
                //        if (!frmshipping.flgCancel)
                //        {
                //            txtCode.Text = frmshipping.ID;
                //        }
                //    }
                //    break;
                case SearchType.Supplier:
                    using (Search_Supplier frmSupplier = new Search_Supplier(changedate))
                    {
                        frmSupplier.parChangeDate = changedate;
                        frmSupplier.ShowDialog();
                        if (!frmSupplier.flgCancel)
                        {
                            txtCode.Text = frmSupplier.ID;
                            lblName.Text = frmSupplier.parName;
                            if (UseChangeDate == true)
                                txtChangeDate.Text = frmSupplier.date;

                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;
                case SearchType.経費番号:
                    using (frmSearch_KeihiNO skeihino = new frmSearch_KeihiNO())
                    {
                        skeihino.ShowDialog();
                        if (!skeihino.flgCancel)
                        {
                            TxtCode.Text = skeihino.ExpenseNumber;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    //frmSearch_KeihiNO skeihino = new frmSearch_KeihiNO();
                    //skeihino.ShowDialog();
                    break;
                case SearchType.Carrier:
                    using (FrmSearch_Carrier carrier = new FrmSearch_Carrier(changedate))
                    {
                        carrier.parChangeDate = changedate;
                        carrier.ShowDialog();
                        if (!carrier.flgCancel)
                        {
                            txtCode.Text = carrier.ID;
                            lblName.Text = carrier.parName;
                            txtChangeDate.Text = carrier.date;
                            CheckBasedFormPanel();//PTK Added
                        }
                    }
                    break;
                case SearchType.EDI処理番号:
                    using (Search_EDIHacchuuNO frmHacchuu = new Search_EDIHacchuuNO(changedate))
                    {
                        frmHacchuu.OperatorCD = Value1;
                        frmHacchuu.AllAvailableStores = Value2;
                        frmHacchuu.ShowDialog();
                        if (!frmHacchuu.flgCancel)
                        {
                            txtCode.Text = frmHacchuu.EDIOrderNO;
                            txtChangeDate.Text = frmHacchuu.ChangeDate;
                            CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;

                case SearchType.JANMulti:
                    using (Search_Product frmJanCD = new Search_Product(changedate))
                    {
                        frmJanCD.Mode = "5";
                        frmJanCD.JANCD = txtCode.Text;
                        frmJanCD.ShowDialog();

                        if (!frmJanCD.flgCancel)
                        {
                            txtCode.Text = frmJanCD.JANCD;
                            txtChangeDate.Text = frmJanCD.ChangeDate;
                            //KTP 2020/06/18
                            //Prevent to Move next control with no record select
                            if(!string.IsNullOrWhiteSpace(TxtCode.Text))
                                CheckBasedFormPanel();//PTK added
                        }
                    }
                    break;

                case SearchType.ITEMMulti:
                    using (Search_Product frmMakerItem = new Search_Product(changedate))
                    {
                        frmMakerItem.Mode = "6";
                        frmMakerItem.ITEM = txtCode.Text;
                        frmMakerItem.ShowDialog();

                        if (!frmMakerItem.flgCancel)
                        {
                            txtCode.Text = frmMakerItem.ITEM;
                            txtChangeDate.Text = frmMakerItem.ChangeDate;
                            if (!string.IsNullOrWhiteSpace(TxtCode.Text))
                                CheckBasedFormPanel();

                        }
                    }
                    break;

                case SearchType.SKUMulti:
                    using (Search_Product frmSKUCD = new Search_Product(changedate))
                    {
                        frmSKUCD.Mode = "7";
                        frmSKUCD.SKUCD = txtCode.Text;
                        frmSKUCD.ShowDialog();

                        if (!frmSKUCD.flgCancel)
                        {
                            txtCode.Text = frmSKUCD.SKUCD;
                            txtChangeDate.Text = frmSKUCD.ChangeDate;
                            CheckBasedFormPanel();
                        }
                    }
                    break;

                case SearchType.支払処理:
                    using (FrmSearch_SiharaiNO frmsiharaino = new FrmSearch_SiharaiNO())
                    {
                        frmsiharaino.ShowDialog();
                        if (!frmsiharaino.flgCancel)
                        {
                            txtCode.Text = frmsiharaino.ID;
                            txtChangeDate.Text = frmsiharaino.date;
                            lblName.Text = frmsiharaino.parName;
                            CheckBasedFormPanel();
                        }
                    }
                    break;
                //case SearchType.Location: // 2019.12.09
                //    using (Search_Location frmLocation = new Search_Location(changedate, Value1))
                //    {
                //        frmLocation.ShowDialog();

                //        if (!frmLocation.flgCancel)
                //        {
                //            txtCode.Text = frmLocation.TanaCD;
                //        }
                //    }
                //    break;
                case SearchType.支払番号検索: // 2019.12.19
                    using (Search_SiharaiShoriNO frmShoriNo = new Search_SiharaiShoriNO())
                    {
                        frmShoriNo.ShowDialog();
                        {
                            if (!frmShoriNo.flgCancel)
                            {
                                txtCode.Text = frmShoriNo.Sc_Code;
                                CheckBasedFormPanel();//PTK added
                            }
                        }
                    }
                    break;

                case SearchType.展示会名://SES added
                    using (Search_Tenzikai frmTenzikai = new Search_Tenzikai(changedate))
                    {
                        frmTenzikai.ShowDialog();
                        if (!frmTenzikai.flgCancel)
                        {
                            txtCode.Text = frmTenzikai.TenzikaiName;
                            CheckBasedFormPanel();
                        }
                    }
                    break;
                case SearchType.展示会商品:
                    using (Search_TenzikaiShouhin frmTenzikaishouhin = new Search_TenzikaiShouhin())
                    {
                        frmTenzikaishouhin.parChangeDate = changedate;

                        //if (UseChangeDate == true)
                        //    frmTenzikaishouhin.parChangeDate = changedate;
                        frmTenzikaishouhin.ShowDialog();
                        if (!frmTenzikaishouhin.flgCancel)
                        {
                            txtCode.Text = frmTenzikaishouhin.parTenzikaiName;
                           // lblName.Text = frmTenzikaishouhin.parSKUName;
                            CheckBasedFormPanel();//PTK added
                        }

                    }
                    break;
            }


            SetFocus(1);
        }

        protected void CheckBasedFormPanel()
        {
            try
            {
                Control ctrl = this;
                do
                {
                    ctrl = ctrl.Parent;
                } while (!(ctrl is Form));
                if (ctrl.GetType().BaseType.Name.Contains("FrmMainForm") || ctrl.GetType().BaseType.Name.Contains("FrmSubForm"))//FrmSubForm   
                {
                    //if (FindParentPanel(this) is Panel)// Commented by PTK bcox Nishikawa san want all Panelheader and Detail ,,,means no to check panel header   2020/05/12
                    //{
                    SendKeys.Send("{ENTER}");
                }
               
                //}
            }
            catch
            {

            }
                
        }
        private static Control FindParentPanel(Control theControl)
        {
            Control rControl = null;

            if (theControl.Parent != null)
            {
                if (theControl.Parent.GetType() == typeof(System.Windows.Forms.Panel))
                {
                    rControl = theControl.Parent;
                    if ((rControl as Panel).Name == "PanelHeader")
                    {
                        rControl = theControl.Parent;
                    }
                    else
                    {
                        rControl = FindParentPanel(theControl.Parent);
                    }
                }
                else

                {
                    rControl = FindParentPanel(theControl.Parent);
                }
            }
            else
            {
                rControl = null;
            }
            return rControl;
        }
        public bool IsExistsDeleteCheck()
        {
            DataTable dtResult = new DataTable();
            switch (Search_Type)
            {

                case SearchType.倉庫:
                    dtResult = bbl.SimpleSelect1("1", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                    break;

                case SearchType.銀行支店:
                    dtResult = bbl.SimpleSelect1("7", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text, Value1);

                    break;

            }

            if (dtResult.Rows.Count > 0)
                if (dtResult.Rows[0]["UsedFlg"].ToString().Equals("1"))
                    return true;
            return false;
        }

        public bool IsExists(int i = 1)
        {
            DataTable dtResult = new DataTable();
            if (i == 1)
            {
                switch (Search_Type)
                {
                    case SearchType.倉庫:
                        dtResult = bbl.SimpleSelect1("1", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        //FieldsName = "1";
                        //TableName = "M_Souko";
                        //Condition = "SoukoCD = '" + TxtCode.Text + "' and " +
                        //           "ChangeDate = '" + (string.IsNullOrWhiteSpace(TxtChangeDate.Text) ? DateTime.Now.ToString("yyyy/MM/dd") : TxtChangeDate.Text.Replace("/", "-")) + "'";
                        //dtResult = bbl.SimpleSelect(FieldsName, TableName, Condition);
                        break;
                    case SearchType.銀行口座:
                        dtResult = bbl.SimpleSelect1("5", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        //FieldsName = "1";
                        //TableName = "M_Kouza";
                        //Condition = TxtCode.Text;
                        //           "ChangeDate = '" + (string.IsNullOrWhiteSpace(TxtChangeDate.Text) ? DateTime.Now.ToString("yyyy/MM/dd") : TxtChangeDate.Text.Replace("/", "-")) + "'";
                        //dtResult = bbl.SimpleSelect(FieldsName, TableName, Condition);
                        break;
                    case SearchType.銀行:
                        dtResult = bbl.SimpleSelect1("3", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        //FieldsName = "1";
                        //TableName = "M_Bank";
                        //Condition = "BankCD = '" + TxtCode.Text + "' and " +
                        //           "ChangeDate = '" + (string.IsNullOrWhiteSpace(TxtChangeDate.Text) ? DateTime.Now.ToString("yyyy/MM/dd") : TxtChangeDate.Text.Replace("/", "-")) + "'";
                        //dtResult = bbl.SimpleSelect(FieldsName, TableName, Condition);
                        break;
                    case SearchType.銀行支店:
                        dtResult = bbl.SimpleSelect1("7", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text, Value1);
                        //FieldsName = "1";
                        //TableName = "M_BankShiten";
                        //Condition = "BranchCD = '" + TxtCode.Text + "' and " + "BankCD = '" + Value1 + "'and " +
                        //           "ChangeDate = '" + (string.IsNullOrWhiteSpace(TxtChangeDate.Text) ? DateTime.Now.ToString("yyyy/MM/dd") : TxtChangeDate.Text.Replace("/", "-")) + "'";

                        //dtResult = bbl.SimpleSelect(FieldsName, TableName, Condition);
                        break;
                    case SearchType.スタッフ:
                        dtResult = bbl.SimpleSelect1("11", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        //FieldsName = "1";
                        //TableName = "M_Staff";
                        //Condition = "StaffCD = '" + TxtCode.Text + "' and " +
                        //           "ChangeDate = '" + (string.IsNullOrWhiteSpace(TxtChangeDate.Text) ? DateTime.Now.ToString("yyyy/MM/dd") : TxtChangeDate.Text.Replace("/", "-")) + "'";
                        //dtResult = bbl.SimpleSelect(FieldsName, TableName, Condition);
                        break;
                    case SearchType.経費番号:
                        dtResult = bbl.SimpleSelect1("10", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        //FieldsName = "1";
                        //TableName = "D_Cost";
                        //Condition = "CostNO = '" + TxtCode.Text + "'";
                        //dtResult = bbl.SimpleSelect(FieldsName, TableName, Condition);
                        break;
                    case SearchType.仕入先:
                        dtResult = bbl.SimpleSelect1("29", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.Carrier:
                        dtResult = bbl.SimpleSelect1("33", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.プログラムID:
                        dtResult = bbl.SimpleSelect1("55", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.得意先://Search_Customer
                        dtResult = bbl.SimpleSelect1("45", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;
                }
            }
            else
            {
                switch (Search_Type)
                {
                    case SearchType.倉庫:
                        dtResult = bbl.SimpleSelect1("2", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.銀行口座:
                        dtResult = bbl.SimpleSelect1("6", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.銀行:
                        dtResult = bbl.SimpleSelect1("4", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.銀行支店:
                        dtResult = bbl.SimpleSelect1("8", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text, Value1);
                        break;
                    case SearchType.仕入先:
                        dtResult = bbl.SimpleSelect1("28", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        break;

                    case SearchType.仕入先PayeeFlg:
                        dtResult = bbl.SimpleSelect1("44", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        break;

                    case SearchType.スタッフ:
                        dtResult = bbl.SimpleSelect1("41", TxtChangeDate.Text.Replace("/", "-"), TxtCode.Text);
                        break;

                    case SearchType.SKUCD:
                        dtResult = bbl.SimpleSelect1("39", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;
                    
                    case SearchType.得意先://Search_Customer
                        dtResult = bbl.SimpleSelect1("45", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.ピッキング番号:
                        dtResult = bbl.SimpleSelect1("46", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;

                    case SearchType.HanyouKeyStart:
                        dtResult = bbl.SimpleSelect1("53", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text,Value1);
                        break;

                    case SearchType.HanyouKeyEnd:
                        dtResult = bbl.SimpleSelect1("54", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text,Value1,Value2);
                        break;
                    case SearchType.ブランド:
                        dtResult = bbl.SimpleSelect1("56", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.競技:
                        dtResult = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text,Value1);
                        break;
                    case SearchType.商品分類:
                        dtResult = bbl.SimpleSelect1("64", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text,Value1);
                        break;
                    case SearchType.SKU_ITEM_CD:
                        dtResult = bbl.SimpleSelect1("65", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.JANCD:
                        dtResult = bbl.SimpleSelect1("66", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.MakerItem:
                        dtResult = bbl.SimpleSelect1("69", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;
                    case SearchType.単価設定:
                        dtResult = bbl.SimpleSelect1("72", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;

                    case SearchType.展示会商品:
                        dtResult = bbl.SimpleSelect1("74", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), TxtCode.Text);
                        break;



                }

            }
            return dtResult.Rows.Count > 0 ? true : false;
        }

        //public bool IsExistsWithMaxChangeDate()
        //{
        //    DataTable dtResult = new DataTable();
        //    switch (Search_Type)
        //    {
        //        case SearchType.Supplier:
        //            M_Vendor_Entity mve = new M_Vendor_Entity
        //            {
        //                VendorCD = txtCode.Text,
        //                ChangeDate = string.IsNullOrWhiteSpace(TxtChangeDate.Text) ? sbl.GetDate() : TxtChangeDate.Text,
        //                DeleteFlg = "0"
        //            };
        //            dtResult = sbl.M_Vendor_IsExists(mve);
        //            break;
        //    }

        //    return dtResult.Rows.Count > 0 ? true : false;
        //}

        //public DataTable SelectWithMaxChangeDate()
        //{
        //    DataTable dtResult = new DataTable();
        //    switch (Search_Type)
        //    {
        //        case SearchType.仕入先:
        //            M_Vendor_Entity mve = new M_Vendor_Entity
        //            {
        //                VendorCD = txtCode.Text,
        //                ChangeDate = string.IsNullOrWhiteSpace(TxtChangeDate.Text) ? sbl.GetDate() : TxtChangeDate.Text,
        //                DeleteFlg = "0"
        //            };
        //            dtResult = sbl.M_Vendor_IsExists(mve);
        //            break;
        //    }

        //    return dtResult;
        //}
        public bool SelectData()
        {
            DataTable dtResult = new DataTable();
            switch (Search_Type)
            {
                case SearchType.銀行:
                    dtResult = bbl.Select_SearchName(TxtChangeDate.Text.Replace("/", "-"), 1, TxtCode.Text);
                    break;
                case SearchType.銀行支店:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 2, Value1, TxtCode.Text);
                    break;
                case SearchType.店舗:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 3, Value1, TxtCode.Text);
                    break;
                case SearchType.仕入先:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 4, TxtCode.Text);
                    break;
                
                case SearchType.スタッフ:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 5, TxtCode.Text);
                    break;
                case SearchType.SKUCD:
                    dtResult = bbl.Select_SearchName(DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), 6, TxtCode.Text);
                    break;
                case SearchType.銀行口座:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 7, TxtCode.Text);
                    break;
                case SearchType.得意先:
                    dtResult = bbl.Select_SearchName(DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), 8, txtCode.Text);
                    break;

                case SearchType.HanyouKeyStart:
                    dtResult = bbl.Select_SearchName(DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), 9, txtCode.Text,Value1);
                    break;
                case SearchType.HanyouKeyEnd:
                    dtResult = bbl.Select_SearchName(DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), 10, txtCode.Text, Value1,Value3);
                    break;
                //20200317
                case SearchType.ブランド:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 11, txtCode.Text, Value1);
                    break;
                case SearchType.競技:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 12, txtCode.Text, Value1);
                    break;
                case SearchType.商品分類:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 13, txtCode.Text, Value1);
                    break;

                case SearchType.仕入先PayeeFlg:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 14, TxtCode.Text);
                    break;

                case SearchType.SKU_ITEM_CD:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 15, txtCode.Text, Value1);
                    break;
                case SearchType.単価設定:
                    dtResult = bbl.Select_SearchName(txtChangeDate.Text.Replace("/", "-"), 16, txtCode.Text, Value1);
                    break;

                case SearchType.展示会商品:
                    dtResult = bbl.Select_SearchName(DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), 17, txtCode.Text);
                    break;
            }
            if (dtResult.Rows.Count > 0)
            {
                lblName.Text = dtResult.Rows[0]["Name"].ToString();
                return true;
            }
            else
            {
                lblName.Text = string.Empty;
                return false;
            }
        }

        public void SetLabelWidth(int width)
        {
            lblName.Width = width;
        }

        
    }
}
