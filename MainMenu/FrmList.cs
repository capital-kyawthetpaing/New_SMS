using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class FrmList : Form
    {
        public DataTable dt { get; set; } =null;
        public FrmList(DataTable dts)
        {
            InitializeComponent();
            dt = null;
            dt = dts;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        } 
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                Check(true);
            }
            else
            {
                Check(false); 
            }
        }
        private void Check(bool flg)
        { 
            foreach (DataGridViewRow gr in dataGridView1.Rows)
            { 
                gr.Cells["colCheck"].Value = flg;
            }
        }
        private void FrmList_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dt;
        }
    }
}
