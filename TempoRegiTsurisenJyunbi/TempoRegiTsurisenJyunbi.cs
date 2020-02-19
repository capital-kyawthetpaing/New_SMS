using Base.Client;
using BL;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempoRegiTsurisenJyunbi
{
    public partial class frmTempoRegiTsurisenJyunbi : ShopBaseForm
    {
        TempoRegiTsurisenJyunbi_BL trtjb;
        D_DepositHistory_Entity mre;
        string storeCD;
        public frmTempoRegiTsurisenJyunbi()
        {
            InitializeComponent();
            // Form that has a button on it
            //button1.PreviewKeyDown += new PreviewKeyDownEventHandler(button1_PreviewKeyDown);
            //button1.KeyDown += new KeyEventHandler(button1_KeyDown);
            //button1.ContextMenuStrip = new ContextMenuStrip();
            //// Add items to ContextMenuStrip
            //button1.ContextMenuStrip.Items.Add("One");
            //button1.ContextMenuStrip.Items.Add("Two");
            //button1.ContextMenuStrip.Items.Add("Three");
        }
        //void button1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.Down:
        //        case Keys.Up:
        //            if (button1.ContextMenuStrip != null)
        //            {
        //                button1.ContextMenuStrip.Show(button1,
        //                    new Point(0, button1.Height), ToolStripDropDownDirection.BelowRight);
        //            }
        //            break;
        //    }
        //}
        private void button1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                    e.IsInputKey = true;
                    break;
            }
        }
        private void frmTempoRegiTsurisenJyunbi_Load(object sender, EventArgs e)
        {
            trtjb = new TempoRegiTsurisenJyunbi_BL();
            InProgramID = "TempoRegiTsurisenJyunbi";

            StartProgram();
            this.Text = "釣銭準備入力";
            SetRequireField();
            BindCombo();
            storeCD = StoreCD;
        }

        public void BindCombo()
        {
            DenominationCD.Bind(string.Empty);
        }
        
        
        private void SetRequireField()
        {
            DepositGaku.Require(true);
            DenominationCD.Require(true);
        }
        public override void FunctionProcess(int index)
        {

            switch (index + 1)
            {
                case 2:
                    Save();
                    break;

            }
        }

        private D_DepositHistory_Entity DepositHistoryEnity()
        {
            mre = new D_DepositHistory_Entity
            {
                DataKBN = "3",
                DepositKBN = "6",
                CancelKBN = "0",
                RecoredKBN = "0",
                Rows = "0",
                AdminNO = "",
                AccountingDate = System.DateTime.Now.ToShortDateString(),
                SalesTaxRate = "0",
                DepositGaku = DepositGaku.Text,
                DenominationCD = "",
                Remark = Remark.Text,
                ExchangeMoney = "0",
                ExchangeDenomination = "",
                ExchangeCount = "0",
                SalesSU = "0",
                SalesUnitPrice = "0",
                SalesGaku = "0",
                SalesTax = "0",
                TotalGaku = "0",
                Refund = "0",
                IsIssued = "0",
                StoreCD = storeCD,
                CustomerCD="",
                DiscountGaku="0",
                ProperGaku="0",
                Operator = InOperatorCD,
                ProcessMode = "登録",
                ProgramID = InProgramID,
                PC = InPcID,
                Key = storeCD + " " + DepositGaku.Text

            };
            return mre;
        }

        /// <summary>
        /// 登録ボタンを押下時データベースにInsertする
        /// </summary>
        public void Save()
        {

            if (ErrorCheck())
            {
                if (trtjb.ShowMessage("Q101") == DialogResult.Yes)
                {
                    mre = DepositHistoryEnity();
                    if (trtjb.TempoRegiTsurisenJyunbi_Insert_Update(mre))
                    {
                        DepositGaku.Clear();
                        DenominationCD.SelectedValue = "-1";
                        Remark.Clear();
                        trtjb.ShowMessage("I101");
                        DepositGaku.Focus();

                    }
                    else
                    {
                        trtjb.ShowMessage("S001");
                    }
                }
                else
                {
                    DepositGaku.Focus();
                   // PreviousCtrl.Focus();
                   //Remark.Focus();
                }
            }

        }
        protected override void EndSec()
        {
            this.Close();
        }

        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { DepositGaku, DenominationCD }))   // go that focus
                return false;

            if (string.IsNullOrWhiteSpace(DenominationCD.SelectedValue.ToString()))
            {
                trtjb.ShowMessage("E102");
                DenominationCD.Focus();
                return false;
            }

            return true;
        }

        private void frmTempoRegiTsurisenJyunbi_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
