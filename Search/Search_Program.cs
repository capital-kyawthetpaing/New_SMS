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
using Search;
using BL;
using Entity;

namespace Search
{
    public partial class Search_Program : FrmSubForm
    {
        public Search_Program()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
        private void F1()
        {
            this.Close();
        }
    }
}
