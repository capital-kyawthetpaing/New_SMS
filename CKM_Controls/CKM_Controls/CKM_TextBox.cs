using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using BL;

namespace CKM_Controls
{
    public class CKM_TextBox : TextBox
    {
        public bool isClosing = false;
        private Type CtrlType { get; set; }
        public enum Type
        {
            Normal = 0,
            Date = 1,//check dateformat
            Price = 2,//check price format
            Number = 3,//check integer only
            Time = 4,
            YearMonth = 5 //2019/01 format

        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select Control Type")]
        [DisplayName("Control Type")]
        public Type Ctrl_Type
        {
            get { return CtrlType; }
            set
            {
                CtrlType = value;
                SetTextAlign();
                ChangeInputMethod();
            }
        }

        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Always)]
        //[Obsolete("just cast me to avoid all this hiding...", true)]
        public  int length_ = 10;
      //  [Browsable(true), EditorBrowsable(EditorBrowsableState.Never)]
      ////  [Obsolete("Hiding...", false)]
      //  [Category("CKM Properties")]
      //  [Description("Set Max Length")]
      //  [DisplayName("MaximumLength(Byte Count)")]
        public int Length
        {
            get { return MaxLength; }
            set {
              //  length_ = value;
                //MaxLength = value;
            }

        }
        //public override int MaxLength    // PTK Added 
        //{
        //    get { return length_; }
        //    set
        //    {
        //        length_ = value;
        //    }

        //}

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
                ChangeInputMethod();
            }
        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Max Length After Decimal Point")]
        [DisplayName("Decimal Place")]
        public int DecimalPlace { get; set; } = 0;

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Max Length Before Decimal Point")]
        [DisplayName("Integer Part")]
        public int IntegerPart { get; set; } = 0;

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Allow Minus")]
        [DisplayName("AllowMinus")]
        public bool AllowMinus { get; set; } = false;

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Border Color")]
        [DisplayName("Border Color")]
        public bool BorderColor { get;

            set; } = false;


        public Color ClientColor { get; set; } = SystemColors.Window;
        private bool IsRequire { get; set; } = false;
        private bool IsReversecheck { get; set; } = false;
        public bool IsCorrectDate { get; set; } = true;
        public bool IsFirstTime { get; set; } = true;
        public bool IsShop { get; set; } = false;
        public bool IsNumber { get; set; } = true;

        public bool isEnterKeyDown { get; set; } = false;
        public bool isMaxLengthErr { get; set; } = false;

        private TextBox txt = null;
        private TextBox txt1 = null;

        public bool MoveNext { get; set; } = true;
        public bool UseColorSizMode { get; set; } = false;
        Base_BL bbl;

        private CKM_Color BackGroundColor { get; set; }
        public enum CKM_Color
        {
            White,
            Green,
            DarkGreen,
            DarkGrey
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select Background Color")]
        [DisplayName("Back Color")]
        public CKM_Color Back_Color
        {
            get { return BackGroundColor; }
            set
            {

                BackGroundColor = value;
                switch (BackGroundColor)
                {
                    case CKM_Color.White:
                        this.BackColor = Color.White;
                        break;
                    case CKM_Color.Green:
                        this.BackColor = Color.FromArgb(226, 239, 218);
                        break;
                    case CKM_Color.DarkGreen:
                        this.BackColor = Color.FromArgb(84, 130, 53);
                        break;
                    case CKM_Color.DarkGrey:
                        this.BackColor = Color.DarkGray;
                        break;

                }

                //PTK Added 
                ClientColor = BackColor ;
            }
        }
        private FontSize Font_Size { get; set; }
        public enum FontSize
        {
            Normal,
            Small,
            SmallLarge,
            FMedium,
            Medium,
            Large,
            XLarge,
            XXLarge
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
                        this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.Small:
                        this.Font = new System.Drawing.Font("MS Gothic", 10F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.SmallLarge:
                        this.Font = new System.Drawing.Font("MS Gothic", 16F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.FMedium:
                        this.Font = new System.Drawing.Font("MS Gothic", 22F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.Medium:
                        this.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.Large:
                        this.Font = new System.Drawing.Font("MS Gothic", 24F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.XLarge:
                        this.Font = new System.Drawing.Font("MS Gothic", 30F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.XXLarge:
                        this.Font = new System.Drawing.Font("MS Gothic", 40F, System.Drawing.FontStyle.Regular);
                        break;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CKM_TextBox()
        {



            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            bbl = new Base_BL();

            //ToolTip tt = new ToolTip();
            //tt.SetToolTip(this, "CKM");
           
            //tt.IsBalloon = true;
            //tt.Show("Test ToolTip", this, 0, 0, 0);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (!Enabled)
                //if (Name.Contains("ararigaku"))
                //    BackColor = Color.DarkGray;
                //else
                    this.BackColor = SystemColors.Control;
            else
                this.BackColor = SystemColors.Window;

            base.OnEnabledChanged(e);

            //BackColor = ClientColor; PTK  if HSK request, only to open

            //base.OnEnabledChanged(e);

            //if (!Enabled)
            //    if (FindMainForm(this) == null)  //Shop Base Check  (null ==true)  //PTK added for Shop
            //    {
            //        this.BackColor = Color.White;
            //    }
            //    else
            //    {
            //        this.BackColor = SystemColors.Control;
            //    }
            //else

            //    this.BackColor = SystemColors.Window;
            // BackColor = ClientColor; 
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //if (Text.Replace("/", "").Length > 6)
            //{
            //    e.Handled = false;
            //}
            //else
            base.OnKeyUp(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)

        
        {
            //if (char.IsDigit(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Space || char.IsLetter(e.KeyChar)) // PTK Added

            //{
            //    if (Text.Length >= MaxLength)
            //    {
            //        e.Handled = true;
            //        return;
            //    }
            //}
            //Key Price Check   by PTK mdf date 08/05/2019
            if (Ctrl_Type == Type.Price)
            {

                if (char.IsDigit(e.KeyChar) || (((e.KeyChar == (char)Keys.Back) || e.KeyChar == ',') || e.KeyChar == '.') || (e.KeyChar == '\u0016' || e.KeyChar == '\u0001' || e.KeyChar == '\u0003' || e.KeyChar == '\u0018') || (e.KeyChar == '-' && AllowMinus) || (e.KeyChar == (char)Keys.Enter))
                {
                    e.Handled = false;
                }

                else
                {
                    e.Handled = true;
                }
            }

            //Key Number Check by PTK mdf date 08/07/2019
            else if (CtrlType == Type.Number)
            {
                if (char.IsDigit(e.KeyChar) || (((e.KeyChar == (char)Keys.Back)) || (e.KeyChar == '-' && AllowMinus)) || (e.KeyChar == '\u0016' || e.KeyChar == '\u0001' || e.KeyChar == '\u0003' || e.KeyChar == '\u0018') || (e.KeyChar == (char)Keys.Enter))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else if (CtrlType == Type.YearMonth)
            {
                if (char.IsDigit(e.KeyChar) || (((e.KeyChar == (char)Keys.Back)) || (e.KeyChar == '-' && AllowMinus)) || (e.KeyChar == '\u0016' || e.KeyChar == '\u0001' || e.KeyChar == '\u0003' || e.KeyChar == '\u0018') || (e.KeyChar == (char)Keys.Enter))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {

                e.Handled = false;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
         
            if (e.KeyCode == Keys.Enter)
            {
                isEnterKeyDown = true;
                MoveNext = true;
                isMaxLengthErr = false;
                if (txt == null)
                {
                    if ((IsRequire && string.IsNullOrWhiteSpace(Text)))
                    {
                        if (this.Parent is UserControl)
                        {
                            UserControl uc = this.Parent as UserControl;
                            (uc.Controls.Find("lblName", true)[0] as Label).Text = string.Empty;
                        }

                        ShowErrorMessage("E102");
                        return;
                    }
                    else if (UseColorSizMode && !string.IsNullOrWhiteSpace(Text))
                    {
                        Text = GetVal(Text);
                    }
                    else if (CtrlType == Type.Date && !DateCheck())
                        return;
                    else if (CtrlType == Type.Number && !NumberCheck())
                        return;
                    else if (CtrlType == Type.Price && !PriceCheck())
                        return;
                    else if (CtrlType == Type.YearMonth && !YearMonthCheck())
                        return;
                    else if ((CtrlType == Type.Normal || CtrlType == Type.Number))
                    {
                        if (this.Parent is UserControl)  /// PTK like in Search Con // Master Con
                        {
                            if (CtrlType == Type.Normal)
                            {
                                string str1 = Encoding.GetEncoding(932).GetByteCount(Text).ToString();
                                if (Convert.ToInt32(str1) > MaxLength)//lenght_ //Added by PTK
                                {
                                    MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    isMaxLengthErr = true;
                                    this.Focus();
                                    return;
                                }

                            }
                            if (CtrlByte == Bytes.半角)//CtrlByte.Equals("半角")
                            {
                                int byteCount = Encoding.GetEncoding("Shift_JIS").GetByteCount(Text);
                                int onebyteCount = System.Text.ASCIIEncoding.ASCII.GetByteCount(Text);
                                if (onebyteCount != byteCount)
                                {
                                    MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    isMaxLengthErr = true;
                                    this.Focus();
                                    return;
                                }
                            }

                        }
                        else // PTK Added 
                        {
                            Control_Check();
                        }
                    }
                    base.OnKeyDown(e);
                }
               
                else if (!string.IsNullOrWhiteSpace(txt.Text))
                {
                    if (IsRequire && string.IsNullOrWhiteSpace(Text))
                    {
                        if (this.Parent is UserControl)
                        {
                            UserControl uc = this.Parent as UserControl;
                            (uc.Controls.Find("lblName", true)[0] as Label).Text = string.Empty;
                        }

                        ShowErrorMessage("E102");
                        return;
                    }
                    else if (CtrlType == Type.Date && !DateCheck())
                        return;
                    else if (CtrlType == Type.Number && !NumberCheck())
                        return;
                    else if (CtrlType == Type.YearMonth && !YearMonthCheck())
                        return;

                    base.OnKeyDown(e);
                }
                if (IsReversecheck && string.IsNullOrWhiteSpace(txt1.Text))
                {
                    if (!string.IsNullOrWhiteSpace(Text))
                    {
                        if (txt1.Parent is UserControl)
                        {
                            UserControl uc = this.Parent as UserControl;
                            (uc.Controls.Find("lblName", true)[0] as Label).Text = string.Empty;
                        }

                        bbl.ShowMessage("E102");
                        txt1.Focus();
                    }
                    else if (CtrlType == Type.Date && !DateCheck())
                        return;
                    else if (CtrlType == Type.Number && !NumberCheck())
                        return;
                    else if (CtrlType == Type.YearMonth && !YearMonthCheck())
                        return;
                    base.OnKeyDown(e);
                }
            }


            else
                base.OnKeyDown(e);
        }
        private string GetVal(string val)
        {
            int t = 0;
            try
            {
                t = Convert.ToInt32(val);
            }
            catch
            {

            }
            if (t == 0)
            {
                bbl.ShowMessage("PLease enter the correct size or color no. . . ");
                this.Focus();
                //return;
            }
            return GetPad(t);
        }
        private int GetInt(string val)
        {
            if (!string.IsNullOrWhiteSpace(val))
            {
                try
                {
                    return Convert.ToInt32(val);
                }
                catch {
                    return Convert.ToInt32(0);
                }
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
        protected void Control_Check()
        {
            if (CtrlByte == Bytes.半全角)
            {
                string str = Encoding.GetEncoding(932).GetByteCount(Text).ToString();
                if (Convert.ToInt32(str) > MaxLength)//lenght_ //Added by PTK
                {
                    MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isMaxLengthErr = true;
                    this.Focus();
                    return;
                }
            }
            if (CtrlByte == Bytes.半角)//CtrlByte.Equals("半角")
            {
                int byteCount = Encoding.GetEncoding("Shift_JIS").GetByteCount(Text);
                int onebyteCount = System.Text.ASCIIEncoding.ASCII.GetByteCount(Text);
                if (onebyteCount != byteCount)
                {
                    MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isMaxLengthErr = true;
                    this.Focus();
                    return;
                }
            }
        }
        public bool YearMonthCheck()  /// To be proceeded by PTK
        {
            Text = Text.Replace("/", "");
            if (Text == "0")
            {
                ShowErrorMessage("E103");
                return false;
            }
            else if (Text.Length == 1)
            {
                this.Text = DateTime.Now.Year.ToString() + "/" + Text.PadLeft(2, '0');

            }
            else if (Text.Length == 2)
            {
                if ((Convert.ToInt32(Text)) > 12 || (Convert.ToInt32(Text)) == 0)
                {
                    ShowErrorMessage("E103");
                    return false;
                }
                else
                    this.Text = DateTime.Now.Year.ToString() + "/" + Text.PadLeft(2, '0');
            }
            else if (Text.Length == 4 || Text.Length == 3)
            {
                ShowErrorMessage("E103");

                return false;
            }
            else if (Text.Length == 6)
            {
                //(str.Length - 4), 4
                var yr = Text.Substring(0, 4).ToString();
                var mn = Text.Substring(Text.Length - 2, 2).ToString().PadLeft(2, '0').ToString();
                if (mn.Length == 2)
                {
                    if ((Convert.ToInt32(mn)) > 12 || (Convert.ToInt32(mn)) == 0)
                    {
                        ShowErrorMessage("E103");
                        return false;
                    }
                }
                if ((Convert.ToInt32(yr)) <= 999)
                {
                    ShowErrorMessage("E103");
                    return false;
                }
                this.Text = yr + "/" + mn;
            }
            else if (Text.Length == 5)
            {
                var yr = Text.Substring(0, 4);
                var mn = Text.Substring(Text.Length - 1, 1).PadLeft(2, '0');
                if (mn.Length == 2)
                {
                    if ((Convert.ToInt32(mn)) > 12 || (Convert.ToInt32(mn)) == 0)
                    {
                        ShowErrorMessage("E103");
                        return false;
                    }
                }
                if ((Convert.ToInt32(yr)) <= 999)
                {
                    ShowErrorMessage("E103");
                    return false;
                }
                this.Text = yr + "/" + mn;
            }
            return true;
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            if(!isClosing)
            {
                if (Check())
                    base.OnValidating(e);
                else
                    e.Cancel = true;
            }

        }
        public string FindMainForm(Control theControl)
        {
            string c = null;
            if (theControl.Parent != null)
            {
                if (theControl.Parent.GetType().BaseType.Name.Contains("Shop"))
                {
                    return null;
                }
                else
                {
                    c = FindMainForm(theControl.Parent);
                }
            }
            else
            {
                c = "";
            }
            return c;
        }

        public static bool IsConsistFullWidth(string txt)
        {
            var c = txt.ToCharArray();
            foreach (char chr in c)
            {
                if (Encoding.GetEncoding("shift_JIS").GetByteCount(chr.ToString()) == 2)
                {
                    return true;
                }

            }
            return false;
        }
        protected override void OnLeave(EventArgs e)
        {
            if (FindMainForm(this) == null)  //Shop Base Check  (null ==true)
            {
                if ((this.Parent is Panel pnl && pnl.Name == "pnl_Body"))
                {

                }
               else
                this.BackColor = Color.FromArgb(226, 239, 218);
            }
            else
            {
                this.BackColor = SystemColors.Window;
            }

            //if (this.Parent is  Panel pnl&&  pnl.Name == "pnl_Body")
            //{
            //    this.BackColor = Color.Red; ; 
            //}
           
            base.OnLeave(e);
           /// var f =BackColor= ClientColor;  //PTK temm removed
        }

        protected override void OnGotFocus(EventArgs e)
        {

            MoveNext = false;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            this.BackColor = Color.FromArgb(255, 242, 204);
            base.OnEnter(e);

            var g = TabIndex;
        }

        private void TxtLeave()
        {
            if (Ctrl_Type == Type.Price)
            {
                string value = Text.Replace(",", "");
                int num;
                int a = Convert.ToInt32(this.DecimalPlace);

                if (Int32.TryParse(value, out num))
                {
                    if (!Text.Equals("0"))
                        Text = string.Format("{0:#,#}", num);
                }
                else if (string.IsNullOrWhiteSpace(value))
                    Text = "0";
                else
                {
                    Text = string.Format("{0:#,#}", value);

                    string[] p = Text.Split('.');
                    if (a != p[1].Length)
                    {
                        bbl.ShowMessage("E118");
                        this.Focus();
                    }
                    else
                    {
                        Text = p[0].ToString();
                        if (Int32.TryParse(Text, out num))
                        {
                            if (!Text.Equals("0"))
                                Text = string.Format("{0:#,#}", num);
                            this.Text = Text + "." + p[1].ToString();
                        }
                    }
                }
            }


            this.BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Calculate Textbox width by FullWidth/HalfWidth * MaxLength
        /// </summary>
        private void CalculateWidth()
        {
            //int divider = CtrlByte == Bytes.半全角 ? 2 : 1;
            //this.MaxLength = length / divider;

            //int l1 = this.Ctrl_Byte == Bytes.半角 ? 10 : 13;
            //this.Width = (l1 * Length) / divider;
            //this.Length = length_;
            //this.MaxLength = length_;
        }

        /// <summary>
        /// set text align while Choosing ctrl_type
        /// </summary>
        private void SetTextAlign()
        {
            switch (CtrlType)
            {
                case Type.Normal:
                    this.TextAlign = HorizontalAlignment.Left;
                    break;
                case Type.Date:
                    this.TextAlign = HorizontalAlignment.Center;
                    this.MaxLength = 10;
                    break;
                case Type.Time:
                    this.TextAlign = HorizontalAlignment.Center;
                    this.MaxLength = 8;
                    break;
                case Type.Price:
                    this.TextAlign = HorizontalAlignment.Right;
                    MaxLength = 20;
                    break;
                case Type.Number:
                    this.TextAlign = HorizontalAlignment.Left;
                    break;
            }
        }

        private void ChangeInputMethod()
        {
            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                if (lang.LayoutName.Equals("Japanese"))
                {
                    InputLanguage.CurrentInputLanguage = lang;
                    break;
                }
            }
        }

        private bool Check()
        {
            if (!string.IsNullOrWhiteSpace(this.Text))
            {
                switch (Ctrl_Type)
                {
                    case Type.Date:
                        return DateCheck();
                    case Type.Number:
                        return NumberCheck();
                    case Type.Price:
                        //if (IsInteger(this.Text.Replace(",", "")) || IsDouble(this.Text.Replace(",", "")))
                        //{
                        //    //e.Cancel = false;
                        //}
                        //else
                        //{
                        //    //e.Cancel = true;
                        //    bbl.ShowMessage("E118");
                        //    this.Focus();
                        //    this.SelectionStart = 0;
                        //    this.SelectionLength = this.Text.Length;
                        //    return false;
                        //}
                        if (PriceCheck())
                        {
                            if (this.AllowMinus == true && this.Text.Contains("-"))
                            {
                                this.ForeColor = Color.Red;
                            }
                            else
                            {
                                this.ForeColor = DefaultForeColor;
                            }
                            return true;
                        }
                        else
                            return false;
                    case Type.Time:
                        return TimeCheck();
                    case Type.YearMonth:
                        return YearMonthCheck();

                }
            }
            return true;
        }

        /// <summary>
        /// Check Date
        /// </summary>
        public bool DateCheck()
        {
            bbl = new Base_BL();
            if (!string.IsNullOrWhiteSpace(this.Text))
            {
                if (bbl.IsInteger(this.Text.Replace("/", "").Replace("-", "")))
                {
                    string day = string.Empty, month = string.Empty, year = string.Empty;
                    if (this.Text.Contains("/"))
                    {
                        string[] date = this.Text.Split('/');
                        day = date[date.Length - 1].PadLeft(2, '0');
                        month = date[date.Length - 2].PadLeft(2, '0');

                        if (date.Length > 2)
                            year = date[date.Length - 3];

                        this.Text = year + month + day;//  this.Text.Replace("/", "");
                    }
                    else if (this.Text.Contains("-"))
                    {
                        string[] date = this.Text.Split('-');
                        day = date[date.Length - 1].PadLeft(2, '0');
                        month = date[date.Length - 2].PadLeft(2, '0');

                        if (date.Length > 2)
                            year = date[date.Length - 3];

                        this.Text = year + month + day;//  this.Text.Replace("-", "");
                    }

                    string text = this.Text;
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
                        IsCorrectDate = true;
                        this.Text = strdate;
                    }
                    else
                    {
                        ShowErrorMessage("E103");
                        return false;
                    }
                }
                else
                {
                    ShowErrorMessage("E103");
                    return false;
                }
            }

            return true;
        }

        private bool TimeCheck()
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                string hour = string.Empty;
                string minutes = string.Empty;
                string seconds = string.Empty;

                string temp = Text;

                temp = temp.Contains(":") ? temp : System.Text.RegularExpressions.Regex.Replace(temp, ".{2}", "$0:");
                temp = temp.TrimEnd(':');

                string[] strtime = temp.Split(':');
                if (strtime.Length > 3)
                {
                    bbl.ShowMessage("E103");
                    Focus();
                    return false;
                }
                else
                {
                    hour = strtime[0].Trim().PadLeft(2, '0');
                    minutes = strtime.Length > 1 ? strtime[1].Trim().PadLeft(2, '0') : "00";
                    seconds = strtime.Length > 2 ? strtime[2].Trim().PadLeft(2, '0') : "00";

                    if (!IsCorrectTime(hour, minutes, seconds))
                    {
                        bbl.ShowMessage("E103");
                        Focus();
                        return false;
                    }

                    Text = hour + ":" + minutes + ":" + seconds;
                }
            }

            return true;
        }

        private bool NumberCheck()
        {

            if (!string.IsNullOrWhiteSpace(Text) && !bbl.IsInteger(this.Text))
            {
                IsNumber = false;
                ShowErrorMessage("E118");
                return false;
            }
            MoveNext = true;
            return true;
        }
        public static bool IsInteger(string value)
        {
            value = value.Replace("-", "");
            if (Int64.TryParse(value, out Int64 Num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool IsCorrectTime(string hour, string minutes, string seconds)
        {
            if (Convert.ToInt32(hour) > 23 || Convert.ToInt32(minutes) > 59 || Convert.ToInt32(seconds) > 59)
                return false;
            return true;
        }

        private bool PriceCheck()
        {
            string first = string.Empty;//111.00 > 111
            string last = string.Empty;//111.00 > 00
            Text = Text.Replace(",", "");
            if (!string.IsNullOrWhiteSpace(Text))
            {

                if (DecimalPlace <= 0)//No Decimal.Integer price only
                {
                    if (!IsInteger(Text))
                    {
                        ShowErrorMessage("E118");
                        MoveNext = false;
                        return false;
                    }
                    //if (Text.Contains("-")) // PTk added bcos (-) on later indexing  // 11/22/2019
                    //{
                    //    if ((Text.Substring(0, 1).ToString() != "-") )
                    //    {
                    //        ShowErrorMessage("E118");
                    //        MoveNext = false;
                    //        return false;
                    //    }
                    //}
                    if (Text.Contains("-")) // PTk added bcos (-) on later indexing  // 11/22/2019
                    {
                        if ((Text.Substring(0, 1).ToString() != "-") || (Text.Substring(1, Text.Length - 1).Contains("-")))
                        {
                            ShowErrorMessage("E118");
                            MoveNext = false;
                            return false;
                        }

                    }
                    if (Text.Replace("-", "").Length > IntegerPart)
                    {
                        ShowErrorMessage("E142");
                        MoveNext = false;
                        return false;
                    }

                    first = Text;
                    first = string.IsNullOrWhiteSpace(first) ? "0" : string.Format("{0:#,#}", Convert.ToInt32(first));
                    Text = (string.IsNullOrWhiteSpace(first) ? "0" : first);

                    MoveNext = true;
                    return true;
                }
                else
                {
                    if (!Text.Contains("."))
                    {
                        if (!IsInteger(Text) && !string.IsNullOrWhiteSpace(Text))
                        {
                            ShowErrorMessage("E118");
                            MoveNext = false;
                            return false;
                        }
                        else
                        {
                            first = Text;
                            last = string.Empty;//decimal part

                            if (first.Length > IntegerPart)
                            {
                                ShowErrorMessage("E142");
                                MoveNext = false;
                                return false;
                            }

                            if (last.Length < DecimalPlace)
                                last = last.PadRight(DecimalPlace, '0');//fill require decimal place with 0

                            first = string.IsNullOrWhiteSpace(first) ? "0" : string.Format("{0:#,#}", Convert.ToInt32(first));
                            Text = (string.IsNullOrWhiteSpace(first) ? "0" : first) + "." + last.ToString();
                        }
                    }
                    else
                    {
                        if (Text.Count(f => f == '.') > 1)//decimal occurs more than one
                        {
                            ShowErrorMessage("E118");
                            MoveNext = false;
                            return false;
                        }

                        if (IsInteger(Text.Replace(".", string.Empty)))
                        {
                            first = Text.Split('.')[0];//integer part
                            last = Text.Split('.')[1];//decimal part

                            if (last.Length > DecimalPlace)
                            {
                                ShowErrorMessage("E142");
                                MoveNext = false;
                                return false;
                            }
                            if (last.Length < DecimalPlace)
                                last = last.PadRight(DecimalPlace, '0');//fill require decimal place with 0


                            if (first.Replace("-", "").Length > IntegerPart || last.Length > DecimalPlace)
                            {
                                ShowErrorMessage("E142");
                                MoveNext = false;
                                return false;
                            }

                            first = string.IsNullOrWhiteSpace(first) ? "0" : string.Format("{0:#,#}", Convert.ToInt32(first));

                            Text = (string.IsNullOrWhiteSpace(first) ? "0" : first) + "." + last.ToString();
                        }
                        else
                        {
                            ShowErrorMessage("E118");
                            MoveNext = false;
                            return false;
                        }
                    }
                }

            }
            MoveNext = true;
            return true;

        }

        private string MinusFilter(string it)
        {

            return "";
        }

        private void ShowErrorMessage(string messageID)
        {
            bbl.ShowMessage(messageID);
            MoveNext = false;
            this.SelectionStart = 0;
            this.SelectionLength = this.Text.Length;
        }

        //private bool IsInteger(string value)
        //{
        //    value = value.Replace("-", "");
        //    if (Int32.TryParse(value, out int Num))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private bool IsDouble(string value)
        {
            double dou;
            if (Double.TryParse(value, out dou))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Require(bool value, TextBox txt = null)
        {
            IsRequire = value;
            this.txt = txt;
        }
        public void ReverseCheck(bool value, TextBox txt = null)
        {
            IsReversecheck = value;
            txt1 = txt;
        }

        public bool GetForm(string formID)
        {
            FormCollection frms = Application.OpenForms;
            foreach (Form frm in frms)
            {
                if (frm.Text.Equals(formID))
                {
                    return true;
                }
            }

            return false;
        }

        public void Change_backColor()
        {
            this.BackColor = Color.Black; ;
        }
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //if (BorderColor)
            //{
            //    BorderStyle = BorderStyle.None;
            //    Pen p = new Pen(Color.Red);
            //    Graphics g = e.Graphics;
            //    int variance = 3;
            //    g.DrawRectangle(p, new Rectangle(Location.X - variance, Location.Y - variance, Width + variance, Height + variance));

            //}
            base.OnPaint(e);
        }

        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);
        }
    }
}
