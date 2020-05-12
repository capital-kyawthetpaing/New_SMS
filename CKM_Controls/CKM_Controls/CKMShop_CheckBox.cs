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
        public CKMShop_CheckBox()
        {
            TextAlign = ContentAlignment.MiddleLeft;
            Height = 27;
            Width = 40;
            Text = string.Empty;
        }
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = false; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            e.Graphics.Clear(Color.White);
            base.OnPaint(e);
            int h = this.ClientSize.Height;
            Rectangle rc;

            if (Size.Height == 24 || Size.Height == 25) // Skipped if Small Black Spot Occurreed
            {
                rc = new Rectangle(new Point(-1, this.Height / 2 - h / 2), new Size(h + 2 , h + 2 ));

            }
            else
            {
                rc = new Rectangle(new Point(0, 1), new Size(h, h));
            }
            ControlPaint.DrawCheckBox(e.Graphics, rc, Checked ? ButtonState.Checked : ButtonState.Normal);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            pevent.Graphics.Clear(Color.White);
        }
        protected override void OnMouseHover(EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            base.OnMouseHover(e);
        }
        protected override bool ShowFocusCues
        {
            get { return true; }
        }

    }
}
