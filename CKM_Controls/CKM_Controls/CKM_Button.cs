using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace CKM_Controls
{
    public class CKM_Button : Button
    {
        private bool BtnSize = false;

        private CKM_FontSize FontSize { get; set; }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set Default Width")]
        [DisplayName("Set Default Width")]
        public bool DefaultBtnSize
        {
            get => BtnSize;
            set
            {
                BtnSize = value;
                if(value)
                {
                    this.Width = 118;
                    this.Height = 28;
                }
            }
        }

        private CKM_Color BackGroundColor { get; set; }
        public enum CKM_Color
        {
            Default,
            Yellow,
            Orange,
            Green,
            DarkGreen,
            White,
            Pink
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select Back Color")]
        [DisplayName("Back Color")]
        public CKM_Color BackgroundColor
        {
            get { return BackGroundColor; }
            set
            {
                BackGroundColor = value;
                switch (BackGroundColor)
                {
                    case CKM_Color.Default:
                        this.BackColor = Color.FromArgb(191, 191, 191);
                        break;
                    case CKM_Color.Yellow:
                        this.BackColor = Color.FromArgb(255, 242, 204);
                        break;
                    case CKM_Color.Orange:
                        this.BackColor = Color.FromArgb(255, 224, 192);
                        break;
                    case CKM_Color.Green:
                        this.BackColor = Color.FromArgb(192, 255, 192);
                        break;
                    case CKM_Color.DarkGreen:
                        this.BackColor = Color.FromArgb(169, 208, 142);
                        break;
                    case CKM_Color.White:
                        this.BackColor = Color.FromArgb(255, 255, 255);
                        break;
                    case CKM_Color.Pink:
                        this.BackColor = Color.FromArgb(255, 192, 255);
                        break;
                }
            }
        }

        public enum CKM_FontSize :int
        {
            Normal,
            XSmall,
            Small,
            Medium,
            Large,
            XLarge
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select FontSize")]
        [DisplayName("Font Size")]
        public CKM_FontSize Font_Size
        {
            get { return FontSize; }
            set
            {
                FontSize = value;
                switch (FontSize)
                {
                    case CKM_FontSize.Normal:
                        this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
                        break;
                    case CKM_FontSize.XSmall:
                        this.Font = new System.Drawing.Font("MS Gothic", 14F, System.Drawing.FontStyle.Bold);
                        break;
                    case CKM_FontSize.Small:
                        this.Font = new System.Drawing.Font("MS Gothic", 22F, System.Drawing.FontStyle.Bold);
                        break;
                    case CKM_FontSize.Medium:
                        this.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
                        break;
                    case CKM_FontSize.Large:
                        this.Font = new System.Drawing.Font("MS Gothic", 28F, System.Drawing.FontStyle.Bold);
                        break;
                    case CKM_FontSize.XLarge:
                        this.Font = new System.Drawing.Font("MS Gothic", 30F, System.Drawing.FontStyle.Bold);
                        break;
                }
            }
        }

        public CKM_Button()
        {
            this.Cursor = Cursors.Hand;
            this.BackColor = Color.FromArgb(191, 191, 191);
            this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FlatAppearance.BorderColor = Color.Black;
            this.Margin = new Padding(1);
        }
    }
}
