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
using Search;

namespace TempoRegiNyuukinTouroku
{
    public partial class TempoRegiNyuukinNyuuryoku : ShopBaseForm
    {
        TempoRegiNyuukinNyuuryoku_BL trntBL = new TempoRegiNyuukinNyuuryoku_BL();
        D_DepositHistory_Entity ddpe = new D_DepositHistory_Entity();
        D_Collect_Entity dce = new D_Collect_Entity();
        public TempoRegiNyuukinNyuuryoku()
        {
            InitializeComponent();
        }

        private void TempoRegiNyuukinTouroku_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiNyuukinTouroku";
            string data = InOperatorCD;
            StartProgram();
            this.Text = "入金入力";
            SetRequireField();
            BindCombo();
            txtPayment.Focus();
            chkAdvanceFlg.Enabled = false;
        }


        public void BindCombo()
        {
            cboDenominationName.Bind(string.Empty);
        }
        private void SetRequireField()
        {
            txtPayment.Require(true);
            cboDenominationName.Require(true);
        }
        private void DisplayData()
        {
            txtPayment.Focus();
            string data = InOperatorCD;
            SetRequireField();
            BindCombo();
        }

        public bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtPayment, cboDenominationName }))
                return false;

            if (!string.IsNullOrWhiteSpace(txtCustomerCD.Text))
            {
                DataTable dtCust = new DataTable();
                dtCust = trntBL.SimpleSelect1("36", null, txtCustomerCD.Text);
                if (dtCust.Rows.Count < 1)
                {
                    trntBL.ShowMessage("E101");
                    txtCustomerCD.Focus();
                    return false;
                }
            }
            return true;
        }
        protected override void EndSec()
        {
            this.Close();
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

        private void Save()
        {
            if (ErrorCheck())
            {
                if (trntBL.ShowMessage("Q101") == DialogResult.Yes)
                {
                    ddpe = GetDepositEntity();
                    if (trntBL.TempoNyuukinTouroku_D_DepositHistory_InsertUpdate(ddpe))
                    {
                        if (!string.IsNullOrWhiteSpace(txtCustomerCD.Text))
                        {
                            dce = GetDCollectData();
                            trntBL.TempoNyuukinTouroku_D_Collect_Insert(dce);
                        }
                        trntBL.ShowMessage("I101");
                        txtPayment.Clear();
                        txtPayment.Focus();
                        cboDenominationName.SelectedValue = "-1";
                        txtCustomerCD.Clear();
                        lblCustomerName.Text = string.Empty;
                        txtRemarks.Clear();
                        DisplayData();
                        chkAdvanceFlg.Enabled = chkAdvanceFlg.Checked = false;
                    }
                }
                RunConsole();
            }
        }

        private D_DepositHistory_Entity GetDepositEntity()
        {
            ddpe = new D_DepositHistory_Entity
            {
                StoreCD = InOperatorCD,
                DataKBN = "3",
                DenominationCD = cboDenominationName.SelectedValue.ToString(),
                CancelKBN = "0",
                RecoredKBN = "0",
                DepositKBN = "2",
                DepositGaku = txtPayment.Text,
                Remark = txtRemarks.Text,
                AccountingDate = System.DateTime.Now.ToShortDateString(),
                Rows = "0",
                ExchangeMoney = "0",
                ExchangeDenomination = "0",
                ExchangeCount = "0",
                SalesSU = "0",
                SalesUnitPrice = "0",
                SalesGaku = "0",
                SalesTax = "0",
                SalesTaxRate = "0",
                TotalGaku = "0",
                Refund = "0",
                ProperGaku = "0",
                DiscountGaku = "0",
                CustomerCD = string.Empty,
                AdminNO = string.Empty,
                IsIssued = "0",
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID,
                ProcessMode = "登 録",
                Key = txtPayment.Text.ToString()

            };
            return ddpe;
        }

        private D_Collect_Entity GetDCollectData()
        {
            dce = new D_Collect_Entity()
            {
                InputKBN = "3",
                StoreCD = StoreCD,
                StaffCD = InOperatorCD,
                CollectCustomerCD = txtCustomerCD.Text,
                PaymentMethodCD = cboDenominationName.SelectedValue.ToString(),
                CollectAmount = txtPayment.Text,
                FeeDeduction = "0",
                Deduction1 = "0",
                Deduction2 = "0",
                DeductionConfirm = "0",
                ConfirmSource = txtPayment.Text,
                ConfirmAmount = "0",
                Remark = txtRemarks.Text,
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID,
                ProcessMode = "登 録",
                Key = txtPayment.Text.ToString()

            };

            if (chkAdvanceFlg.Checked == true)
                dce.AdvanceFLG = "1";
            else dce.AdvanceFLG = "0";
            return dce;
        }


        private void TempoRegiNyuukinNyuuryoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void btnCustomerCD_Click(object sender, EventArgs e)
        {
            TempoRegiKaiinKensaku kaiinkensaku = new TempoRegiKaiinKensaku();
            kaiinkensaku.ShowDialog();
            if (!(string.IsNullOrWhiteSpace(kaiinkensaku.CustomerCD)) && !(string.IsNullOrWhiteSpace(kaiinkensaku.CustomerName)))
            {
                txtCustomerCD.Text = kaiinkensaku.CustomerCD;
                lblCustomerName.Text = kaiinkensaku.CustomerName;
                //txtCustomerCD.Focus();
            }
        }

        private void txtCustomerCD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtCustomerCD.Text))
                {
                    DataTable dtCust = new DataTable();
                    dtCust = trntBL.SimpleSelect1("36", null, txtCustomerCD.Text);
                    if (dtCust.Rows.Count < 1)
                    {
                        chkAdvanceFlg.Enabled = false;
                        trntBL.ShowMessage("E101");
                        txtCustomerCD.Focus();
                    }
                    else
                    {
                        lblCustomerName.Text = dtCust.Rows[0]["CustomerName"].ToString();
                        chkAdvanceFlg.Enabled = true;
                    }

                }
                else
                {
                    lblCustomerName.Text = string.Empty;
                    chkAdvanceFlg.Enabled = false;
                }
            }
        }

        private void RunConsole()
        {
            string programID = "TempoTorihikiReceipt";
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "2";
            string cmdLine = " " + InOperatorCD + " " + Login_BL.GetHostName()   + " " + Mode ;//parameter
            System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
        }
    }
}

