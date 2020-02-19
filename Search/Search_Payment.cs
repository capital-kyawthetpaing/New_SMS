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
using BL;

namespace Search
{
    public partial class Search_Payment : FrmSubForm
    {
        Search_Payment_BL spbl;
        public Search_Payment()
        {
            InitializeComponent();
           
        }

        private void Search_Payment_Load(object sender, EventArgs e)
        {
            spbl = new Search_Payment_BL();
            F9Visible = false;
        }
    }
}
