using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempoRegiHanbaiRireki
{
    public partial class FrmSelect : Form
    {
        public int btnSelect = 0;
        public FrmSelect()
        {
            InitializeComponent();
        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            btnSelect = 1;
            this.Close();
        }

        private void ckM_Button2_Click(object sender, EventArgs e)
        {
            btnSelect = 2;
            this.Close();
        }
    }
}
