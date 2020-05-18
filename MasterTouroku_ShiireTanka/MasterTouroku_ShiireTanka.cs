using Base.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Search;
using BL;

namespace MasterTouroku_ShiireTanka
{


    public partial class FrmMasterTouroku_ShiireTanka : FrmMainForm
    {

        Base_BL bbl = new Base_BL();
        public FrmMasterTouroku_ShiireTanka()
        {
            InitializeComponent();
        }

        private void sport_Enter(object sender, EventArgs e)
        {
            sport.Value1 = "202";
            sport.ChangeDate = bbl.GetDate();
        }

        private void FrmMasterTouroku_ShiireTanka_Load(object sender, EventArgs e)
        {
           
        }
    }
}
