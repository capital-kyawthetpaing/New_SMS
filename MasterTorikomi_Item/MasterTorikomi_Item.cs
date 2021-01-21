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
namespace MasterTorikomi_Item
{
    public partial class MasterTorikomi_Item : Base.Client.FrmMainForm
    {
        public MasterTorikomi_Item()
        {
            InitializeComponent();
        }
        private void MasterTorikomi_Item_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTorikomi_Item";
            StartProgram();
            FalseKey();
            this.KeyUp += MasterTorikomi_Item_KeyUp;
        }

        private void MasterTorikomi_Item_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void FalseKey()
        {
            F2Visible = F3Visible=  F4Visible= F5Visible=F7Visible=F8Visible=F9Visible=F10Visible= F11Visible= false;
        }
    }
}
