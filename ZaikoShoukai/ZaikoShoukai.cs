using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;

namespace ZaikoShoukai
{
    public partial class ZaikoShoukai : FrmMainForm
    {
        public ZaikoShoukai()
        {
            InitializeComponent();
        }

        private void ZaikoShoukai_Load(object sender, EventArgs e)
        {
            InProgramID = "ZaikoYoteiHyou";
            StartProgram();
        }
       
        protected override void EndSec()
        {
            this.Close();
        }
    }
}
