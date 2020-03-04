using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using Base.Client;
using CKM_Controls;


namespace SiiresakiZaikoYoteiHyou
{
    public partial class SiiresakiZaikoYoteiHyou : FrmMainForm
    {
        SiiresakiZaikoYoteiHyou_BL szybl;
        D_MonthlyPurchase_Entity dmpe;
        DataTable dt;

        public SiiresakiZaikoYoteiHyou()
        {
            InitializeComponent();
            szybl = new SiiresakiZaikoYoteiHyou_BL();
            dmpe = new D_MonthlyPurchase_Entity();
            dt = new DataTable();
        }

        private void SiiresakiZaikoYoteiHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "SiiresakiZaikoYoteiHyou";
            StartProgram();
        }
        
        protected override void EndSec()
        {
            this.Close();
        }
        private void BindStore()
        {
            cboStore.Bind(string.Empty, "2");
            cboStore.SelectedValue = StoreCD;
        }
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);
            switch (Index + 1)
            {
                case 6:
                    {
                        if (szybl.ShowMessage("Q004") != DialogResult.Yes)
                            return;
                        break;
                    }
            }
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtTargetDateTo,cboStore }))
                return false;
            if (Convert.ToInt32((txtTargetDateFrom.Text.ToString().Replace("/", ""))) > Convert.ToInt32(txtTargetDateTo.Text.ToString().Replace("/", ""))) //対象期間(From)の方が大きい場合Error
            {
                szybl.ShowMessage("E103");
                return false;
            }
            return true;
        }
        private M_StoreClose_Entity GetStoreClose_Data()
        {
            msce = new M_StoreClose_Entity()
            {
                StoreCD = cboStore.SelectedValue.ToString(),
                FiscalYYYYMM = txtTargetDateFrom.Text.Replace("/", ""),
            };
            return msce;
        }

        private void SiiresakiZaikoYoteiHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
