using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CKM_Controls
{
    public class CKM_MultiLineTextBox : TextBox
    {
        public  bool Mdea { get; set; } = false;
        public bool Mfocus { get; set; } = false;
        public bool MoveNext { get; set; } = true;
        public KeyEventArgs ke;
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
                this.length = value;
                CalculateWidth();
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
                ChangeInputMethod();
            }
        }

        private int Row_Count = 5;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set Row Count")]
        [DisplayName("Row Count")]
        public int RowCount
        {
            get => Row_Count;
            set
            {
                Row_Count = value;
                CalculateHeight();
            }
        }

        private CKM_Color BackGroundColor { get; set; }
        public enum CKM_Color
        {
            White,
            Green,
            DarkGreen
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
                }
            }
        }
        private FontSize Font_Size { get; set; }
        public enum FontSize
        {
            Normal,
            Small,
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
                        this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.Small:
                        this.Font = new System.Drawing.Font("MS Gothic", 10F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.Medium:
                        this.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.Large:
                        this.Font = new System.Drawing.Font("MS Gothic", 20F, System.Drawing.FontStyle.Regular);
                        break;
                    case FontSize.XLarge:
                        this.Font = new System.Drawing.Font("MS Gothic", 30F, System.Drawing.FontStyle.Regular);
                        break;
                }
            }
        }
        public CKM_MultiLineTextBox()
        {
            this.Multiline = true;
            CalculateWidth();
            CalculateHeight();
            ChangeInputMethod();
        }

        private void CalculateWidth()
        {
            //int divider = CtrlByte == Bytes.半全角 ? 2 : 1;
            //MaxLength = length / divider;

            //int l1 = this.Ctrl_Byte == Bytes.半角 ? 10 : 13;
            //this.Width = (l1 * (Length / RowCount)) / divider;
            this.MaxLength = length;
        }

        private void CalculateHeight()
        {
            this.Height = RowCount * 19;
        }

        protected override void OnEnter(EventArgs e)
        {
           
            this.BackColor = Color.FromArgb(255, 242, 204);
            this.Select(length, length);
            base.OnEnter(e);
            Cursorin = Text;

        }
        bool Isselected = false;


        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
        }
        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);
        }
        protected  bool Over_CheckBytes()
        {
            if (Ctrl_Byte == Bytes.半全角)
            {
               // var h = Encoding.GetEncoding(932).GetByteCount("１２３２３１２２２２２２２２わわわわわわ").ToString();
                string str = Encoding.GetEncoding(932).GetByteCount(Text.Replace("\r\n","")).ToString();
                if ((Convert.ToInt32(str)) > length)
                {
                    return true;
                }
            }
            return false;
        }
        protected override void OnLeave(EventArgs e)
        {                
            base.OnLeave(e);
            if (FindMainForm(this) == null)  //Shop Base Check  (null ==true)
            {
                this.BackColor = Color.FromArgb(226, 239, 218);
            }
            else
            {
                this.BackColor = SystemColors.Window;
            }
            //base.OnLeave(e);
            //this.BackColor = Color.White;
            //Cursorout = Text;
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

            this.ImeMode = ImeMode.Hiragana;
        }

        //protected override void OnEnabledChanged(EventArgs e)
        //{
        //    base.OnEnabledChanged(e);
        //    if (!Enabled)
        //        this.BackColor = SystemColors.Control;
        //    else
        //        //  this.BackColor = SystemColors.Window;
        //        this.BackColor = Color.White;
        //}
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
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            if (!Enabled)
                if (FindMainForm(this) == null)  //Shop Base Check  (null ==true)
                {
                    this.BackColor = Color.White;
                }
                else
                {
                    this.BackColor = SystemColors.Control;
                }
            else
                this.BackColor = SystemColors.Window;
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }

        //private void CKM_MultiLineTextBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //  ///  base.OnKeyDown(e);
        //}

        protected override void OnKeyPress(KeyPressEventArgs e)
        { 
            //if (Over_CheckBytes() )
            //{
            //    if (Char.IsLetterOrDigit(e.KeyChar))
            //    {
            //        e.Handled = true;
            //    }
            //}
            
           
           // var f = Text;
            base.OnKeyPress(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {

            if (e.Alt && e.KeyCode == Keys.Enter)    // Wait PTK , Not Confirm
            {
                SendKeys.Send("^{ENTER}");
                return;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (Over_CheckBytes())
                {
                    //  e.Cancel = true;
                    MoveNext = false;
                   // SelectAll();
                    DialogResult ok = MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Mdea = true;
                }
                else
                {
                    Mdea = false;
                    Cursorin = Text;
                    MoveNext = true;
                   // return;
                }
            }
          //  var f = this.Text;
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                {
                    ke = e;
                    Cursorout = Text;
                }
            if (Isselected)
            {
                Isselected = false;
            }
            base.OnKeyUp(e);
        }

        public string Cursorin = "";
        public string Cursorout = "";
        protected override void OnGotFocus(EventArgs e)
        {
            F_focus = true;
            base.OnGotFocus(e);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            F_focus = false;
            // MoveNext = false;
            Cursorout = Text;
            Form d = this.FindForm();
            RaiseKeyEvent(null, ke);   
            
        }
        public bool F_focus { get; set; } = false;
        public event EventHandler ThresholdReached;

        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = ThresholdReached;
            handler?.Invoke(this, e);
            
        }
       

    }
}
