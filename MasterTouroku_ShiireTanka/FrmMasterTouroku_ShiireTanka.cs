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

namespace MasterTouroku_ShiireTanka
{
    public partial class FrmMasterTouroku_ShiireTanka : FrmMainForm
    {
        Base_BL bbl = new Base_BL();
        public FrmMasterTouroku_ShiireTanka()
        {
            InitializeComponent();
        }

        private void FrmMasterTouroku_ShiireTanka_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_ShiireTanka";
            StartProgram();
            

        }
        private void BindCombo()
        {
            string ymd = bbl.GetDate();
            CB_store.Bind(ymd);
            CB_year.Bind(ymd);
            CB_season.Bind(ymd);
        }

        protected override void EndSec()
        {
            this.Close();
        }
    }
}
