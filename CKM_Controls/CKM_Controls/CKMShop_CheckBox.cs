using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CKM_Controls
{
    public class CKMShop_CheckBox : CheckBox
    {
        private Color color1 = Color.SteelBlue;
        private Color color2 = Color.DarkBlue;
        private Color m_hovercolor1 = Color.Yellow;
        private Color m_hovercolor2 = Color.DarkOrange;
        private int boxsize = 35;
        private int boxlocatx = 0;
        private int boxlocaty = 0;
        private int angle = 90;
        private int textX = 200;
        private int textY = 200;
        private String text = "";
        private bool IsattachedCaption_ = true;
        public bool IsattachedCaption {
            get { return IsattachedCaption_; }
            set { IsattachedCaption_ = value; Invalidate(); }
        }
        private String DisplayText
        {
            get { return text; }
            set { text = value; Invalidate(); }
        }

        private int TextLocation_X
        {
            get { return textX; }
            set { textX = value; Invalidate(); }
        }
        private int TextLocation_Y
        {
            get { return textY; }
            set { textY = value; Invalidate(); }
        }
        private int BoxSize
        {
            get { return boxsize; }
            set { boxsize = value; Invalidate(); }
        }
        private int BoxLocation_X
        {
            get { return boxlocatx; }
            set { boxlocatx = value; Invalidate(); }
        }
        private int BoxLocation_Y
        {
            get { return boxlocaty; }
            set { boxlocaty = value; Invalidate(); }
        }
        public CKMShop_CheckBox()
        {
            Size = new Size(200,35);
            this.TextAlign = ContentAlignment.MiddleRight;// this.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ForeColor = Color.Black; ;
            this.AutoSize = false;
        }
        protected override void OnPaint(PaintEventArgs e)//PTK Added
        {

            this.OnPaintBackground(e);
            base.OnPaint(e);
          
            this.AutoSize = false;
            text = this.Text;
            Color c1 = Color.Transparent;
            Color c2 = Color.Transparent;
            Point p = new Point(textX, textY);
            SolidBrush frcolor = new SolidBrush(this.ForeColor);
            Rectangle rc = new Rectangle(boxlocatx, boxlocaty, boxsize, boxsize);
            ControlPaint.DrawCheckBox(e.Graphics, rc, this.Checked ? ButtonState.Checked : ButtonState.Normal);
            if (!IsattachedCaption_)
            {
                this.Text = "";
                
                if (Isfocus)
                {
                    
                    Rectangle rcfoc = new Rectangle(0,0,ClientRectangle.Width,ClientRectangle.Height);
                    Color dotted = Color.Black;
                    Pen pn = new Pen(dotted);
                    ControlPaint.DrawBorder(e.Graphics, rcfoc,
                        dotted, 1, ButtonBorderStyle.Dotted,
                        dotted, 1, ButtonBorderStyle.Dotted,
                        dotted, 1, ButtonBorderStyle.Dotted,
                        dotted, 1, ButtonBorderStyle.Dotted);
                }
                Size = new Size(35, 35);
            }
            else{
                
                Size = new Size(Width, 35);
            }
        }
        protected override void OnGotFocus(EventArgs e)
        {
           
            base.OnGotFocus(e);
            Isfocus = true;
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Isfocus = false;
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            Isfocus = true;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            Isfocus = false;
        }

        private bool Isfocus { get; set; } = false;
        protected override void OnSizeChanged(EventArgs e) // PTK added
        {

            boxlocaty = (Size.Height / 2) - (boxsize / 2);
            base.OnSizeChanged(e);
        }
        protected override bool ShowFocusCues
        {
            get { return true; }
        }
        //protected override bool ShowFocusCues
        //{
        //    get { return true; }
        //}

        //public CKMShop_CheckBox()
        //{
        //    TextAlign = ContentAlignment.MiddleLeft;
        //    Height = 27;
        //    Width = 40;
        //    Text = string.Empty;
        //}
        //public override bool AutoSize
        //{
        //    get { return base.AutoSize; }
        //    set { base.AutoSize = false; }
        //}
        //protected override void OnPaint(PaintEventArgs e)
        //{

        //    e.Graphics.Clear(Color.White);
        //    base.OnPaint(e);
        //    int h = this.ClientSize.Height;
        //    Rectangle rc;

        //    if (Size.Height == 24 || Size.Height == 25) // Skipped if Small Black Spot Occurreed
        //    {
        //        rc = new Rectangle(new Point(-1, this.Height / 2 - h / 2), new Size(h + 2 , h + 2 ));

        //    }
        //    else
        //    {
        //        rc = new Rectangle(new Point(0, 1), new Size(h, h));
        //    }
        //    ControlPaint.DrawCheckBox(e.Graphics, rc, Checked ? ButtonState.Checked : ButtonState.Normal);
        //}

        //protected override void OnPaintBackground(PaintEventArgs pevent)
        //{
        //    base.OnPaintBackground(pevent);
        //   // pevent.Graphics.Clear(Color.White);
        //}
        //protected override void OnMouseHover(EventArgs e)
        //{
        //    this.Cursor = Cursors.Hand;
        //    base.OnMouseHover(e);
        //}
        //protected override bool ShowFocusCues
        //{
        //    get { return true; }
        //}
        //protected override void OnGotFocus(EventArgs e)
        //{
        //    base.OnGotFocus(e);
        //}
        //protected override void OnLostFocus(EventArgs e)
        //{
        //    base.OnLostFocus(e);
        //}
    }
}
