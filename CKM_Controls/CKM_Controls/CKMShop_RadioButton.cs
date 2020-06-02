using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CKM_Controls
{
    public class CKMShop_RadioButton : RadioButton
    {

        Color clr1, clr2;
        private Color color1 = Color.White;
        private Color color2 = Color.White;
        private Color m_hovercolor1 = Color.Yellow;
        private Color m_hovercolor2 = Color.DarkOrange;
        private int color1Transparent = 0;
        private int color2Transparent = 0;
        private int boxsize = 30;
        private int boxlocatx = -2;
        private int boxlocaty = 2;
        private int angle = 90;
        private int textX = 500;
        private int textY = 500;
        private String text = "";

        private String DisplayText
        {
            get { return text; }
            set { text = value; Invalidate(); }
        }
        //public Color StartColor
        //{
        //    get { return color1; }
        //    set { color1 = value; Invalidate(); }
        //}
        //public Color EndColor
        //{
        //    get { return color2; }
        //    set { color2 = value; Invalidate(); }
        //}
        private int Transparent1
        {
            get { return color1Transparent; }
            set
            {
                color1Transparent = value;
                if (color1Transparent > 255)
                {
                    color1Transparent = 255;
                    Invalidate();
                }
                else
                    Invalidate();
            }
        }

        private int Transparent2
        {
            get { return color2Transparent; }
            set
            {
                color2Transparent = value;
                if (color2Transparent > 255)
                {
                    color2Transparent = 255;
                    Invalidate();
                }
                else
                    Invalidate();
            }
        }

        private int GradientAngle
        {
            get { return angle; }
            set { angle = value; Invalidate(); }
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
        }// PTK Added
        public CKMShop_RadioButton() //PTK addedd
        {
            //this.text = "Label";
            Size = new Size(178,41);
            this.TextAlign = ContentAlignment.MiddleRight;
            this.ForeColor = Color.Black; 
            this.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
        }
        protected override void OnPaint(PaintEventArgs e)  //PTK added
        {
            this.OnPaintBackground(e);
            base.OnPaint(e);
            this.AutoSize = false;
            text = this.Text;
            Color c1 = Color.Transparent;
            Color c2 = Color.Transparent;
            Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, c1, c2, angle);
            SolidBrush frcolor = new SolidBrush(Color.Black);
            e.Graphics.FillRectangle(b, ClientRectangle);
            e.Graphics.DrawString(text, new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold), frcolor, new Point(textX, textY));
            Rectangle rc = new Rectangle(boxlocatx, boxlocaty, boxsize, boxsize);
            ControlPaint.DrawRadioButton(e.Graphics, rc, this.Checked ? ButtonState.Checked : ButtonState.Normal);
            b.Dispose();
        }
        protected override void OnSizeChanged(EventArgs e) // PTK added
        {
           
                boxlocaty = (Size.Height / 2) - (boxsize / 2);
            base.OnSizeChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }
        protected override bool ShowFocusCues
        {
            get { return true; }
        }

    }
}
