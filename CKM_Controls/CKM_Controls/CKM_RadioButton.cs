using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CKM_Controls 
{
    public class CKM_RadioButton : RadioButton
    {
        protected override bool ShowFocusCues
        {
            get { return true; }
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }
        public CKM_RadioButton()
        {
            this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
        }

        
    }
}
