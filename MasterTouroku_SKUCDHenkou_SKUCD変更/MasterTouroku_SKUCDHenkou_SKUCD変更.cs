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

namespace MasterTouroku_SKUCDHenkou_SKUCD変更
{
    public partial class MasterTouroku_SKUCDHenkou_SKUCD変更 : FrmMainForm
    {
        public MasterTouroku_SKUCDHenkou_SKUCD変更()
        {
            InitializeComponent();
        }

        private void MasterTouroku_SKUCDHenkou_SKUCD変更_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_SKUCDHenkou_SKUCD変更";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            Sc_Item.SetFocus(1);
        }

        private void SetRequiredField()
        {
            Sc_Item.TxtCode.Require(true);
        }
        protected override void EndSec()
        {
            base.EndSec();
        }
    }
}
