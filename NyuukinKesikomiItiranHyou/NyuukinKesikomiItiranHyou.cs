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
using Entity;

namespace NyuukinKesikomiItiranHyou
{
    public partial class NyuukinKesikomiItiranHyou : FrmMainForm
    {
        NyuukinKesikomiItiranHyou_BL nkih_bl;

        public NyuukinKesikomiItiranHyou()
        {
            InitializeComponent();
            nkih_bl = new NyuukinKesikomiItiranHyou_BL();
        }

        private void NyuukinKesikomiItiranHyou_Load(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            StartProgram();
            SetFunctionLabel(EProMode.PRINT);
            SetRequireField();
            Btn_F11.Text = string.Empty;
            Btn_F10.Text = string.Empty;

            //Bind ComboBoxes
            cboStoreAuthorizations.Bind(string.Empty, "2");
            cboStoreAuthorizations.SelectedValue = StoreCD;
            cboWebCollectType.Bind(string.Empty);
        }

        private void SetRequireField()
        {
            cboStoreAuthorizations.Require(true);
        }
    }
}
