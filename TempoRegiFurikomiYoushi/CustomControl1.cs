using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempoRegiFurikomiYoushi
{
    public partial class CustomControl1 : RadioButton
    {
        public CustomControl1()
        {
            InitializeComponent();
            this.Appearance = System.Windows.Forms.Appearance.Button;
            this.BackColor = Color.Transparent;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FlatAppearance.BorderColor = Color.RoyalBlue;
            this.FlatAppearance.BorderSize = 2;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.OnPaintBackground(e);
            using (var path = new GraphicsPath())
            {
                var c = e.Graphics.ClipBounds;
                var r = this.ClientRectangle;
                r.Inflate(-FlatAppearance.BorderSize, -FlatAppearance.BorderSize);
                path.AddEllipse(r);
                e.Graphics.SetClip(path);
                base.OnPaint(e);
                e.Graphics.SetClip(c);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                if (this.Checked)
                {
                    using (var p = new Pen(FlatAppearance.BorderColor,
                                           FlatAppearance.BorderSize))
                    {
                        e.Graphics.DrawEllipse(p, r);
                    }
                }
            }
        }
    }
}
