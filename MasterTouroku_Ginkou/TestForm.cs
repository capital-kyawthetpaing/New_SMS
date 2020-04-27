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
namespace MasterTouroku_Ginkou
{
    public partial class TestForm :  FrmMainForm
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            ckM_TextBox2.Enabled = true;
        }

        private void ckM_Button2_Click(object sender, EventArgs e)
        {
            ckM_TextBox2.Enabled = false;
        }
    }
}
