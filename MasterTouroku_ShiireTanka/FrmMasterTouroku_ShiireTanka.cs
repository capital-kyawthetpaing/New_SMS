using System;
using Search;
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
            ModeText = "ITEM";
            BindCombo();
            TB_Changedate.Text = bbl.GetDate();
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

        private void FrmMasterTouroku_ShiireTanka_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void shiiresaki_Enter(object sender, EventArgs e)
        {
            shiiresaki.Value1 = "1";
            shiiresaki.ChangeDate = TB_Changedate.Text;
        }

        private bool ErrorCheck()
        {
            if(String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
            {
                bbl.ShowMessage("E102");
                shiiresaki.Focus();
                return false;
            }
            return true;
        }

        private void shiiresaki_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(String.IsNullOrEmpty(shiiresaki.TxtCode.Text))
                {
                    
                }
            }
        }
    }
}
