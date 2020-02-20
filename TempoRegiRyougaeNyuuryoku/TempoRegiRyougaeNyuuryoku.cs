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

namespace TempoRegiRyougaeNyuuryoku
{
    
    public partial class frmTempoRegiRyougaeNyuuryoku : ShopBaseForm
    {
        int countmoney = 0;
        int moneytype = 0;
        string combovalue = "";
        bool valid = false;
        TempoRegiRyougaeNyuuryoku_BL trrnbl;
        D_DepositHistory_Entity mre;
        DataTable dtDepositNO;
        string storeCD;
        public frmTempoRegiRyougaeNyuuryoku()
        {
            InitializeComponent();
            dtDepositNO = new DataTable();
            trrnbl = new TempoRegiRyougaeNyuuryoku_BL();
            mre = new D_DepositHistory_Entity();
        }

        private void frmTempoRegiRyougaeNyuuryoku_Load(object sender, EventArgs e)
        {
            trrnbl = new TempoRegiRyougaeNyuuryoku_BL();
            InProgramID = "TempoRegiRyougaeNyuuryoku";
            
            StartProgram();
            this.Text = "両替入力";
            SetRequireField();
            BindCombo();
            storeCD = StoreCD;
            //displayData();
        }
        public void BindCombo()
        {
            ExchangeDenomination.Bind(string.Empty);
        }
        /// <summary>
        /// お金が正しいかどうかををチェックする
        /// </summary>
        public void displayData()
        {
            int moneyammount = countmoney * moneytype;
            string moneysperate = moneyammount.ToString("#,##0");
            ExchangeLabel.Text = moneysperate;
            if (ExchangeLabel.Text != ExchangeMoney.Text)
            {
                trrnbl.ShowMessage("E181");
                ExchangeCount.Focus();
            }
        }
        private void SetRequireField()
        {
            ExchangeMoney.Require(true);
            ExchangeDenomination.Require(true);
            ExchangeCount.Require(true);
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
                DepositKBN = "4",
                DepositKBN1 = "5",
                CancelKBN = "0",
                RecoredKBN = "0",
                DenominationCD = "",
                Rows = "0",
                SalesSU = "0",
                SalesUnitPrice = "0",
                SalesGaku = "0",
                SalesTax = "0",
                TotalGaku = "0",
                SalesTaxRate = "0",
                Refund = "0",
                ProperGaku="0",//update
                DiscountGaku="0",//update
                CustomerCD="",//update
                IsIssued = "0",
                ExchangeMoney = ExchangeMoney.Text,
                ExchangeDenomination = ExchangeDenomination.SelectedValue.ToString(),
                ExchangeCount = ExchangeCount.Text,
                Remark = Remark.Text,
                StoreCD = storeCD,
                Operator = InOperatorCD,
                ProcessMode = "登録",
                ProgramID = InProgramID,
                PC = InPcID,
                Key = storeCD+" "+ ExchangeMoney.Text

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
                //RunConsole();
                if (ExchangeLabel.Text != ExchangeMoney.Text)
                {
                    trrnbl.ShowMessage("E181");
                    ExchangeCount.Focus();
                }
                else
                {
                    if (trrnbl.ShowMessage("Q101") == DialogResult.Yes)
                    {
                        valid = false;
                        mre = DepositHistoryEnity();
                        if (trrnbl.TempoRegiRyougaeNyuuryoku_Insert_Update(mre))
                        {
                            ExchangeMoney.Clear();
                            ExchangeDenomination.SelectedValue = "-1";
                            ExchangeCount.Clear();
                            ExchangeLabel.Text = "";
                            Remark.Clear();
                            trrnbl.ShowMessage("I101");
                            //RunConsole();
                            ExchangeMoney.Focus();
                        }
                        else
                        {
                            trrnbl.ShowMessage("S001");
                        }
                    }
                    else
                    {
                        PreviousCtrl.Focus();
                        //Remark.Focus();
                    }
                }
            }

        }
        
        private void RunConsole()
        {
            string programID = "TempoRegiTorihikiReceipt";
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "5";
            dtDepositNO = bbl.SimpleSelect1("52", "", Application.ProductName, "", "");
            string DepositeNO = dtDepositNO.Rows[0]["DepositNO"].ToString();
            string cmdLine = " " + InOperatorCD + " " + Login_BL.GetHostName() + " " + Mode + " " + DepositeNO;
            try
            {
                System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            }
            catch
            {

            }

        }
        /// <summary>
        /// 入力必須エラーをチェックする
        /// </summary>
        private bool ErrorCheck()
        {

            if (!RequireCheck(new Control[] { ExchangeMoney, ExchangeDenomination }))   // go that focus
                return false;

            if (ExchangeDenomination.SelectedValue.ToString() == "-1")

            {
                trrnbl.ShowMessage("E102");
                ExchangeDenomination.Focus();
                ExchangeDenomination.MoveNext = false;
                ExchangeCount.MoveNext = false;
                return false;
            }
            if (!RequireCheck(new Control[] { ExchangeCount }))   // go that focus
                return false;

            return true;
        }

        
        /// <summary>
        /// 戻るボタンを押下時formを閉じる
        /// </summary>
        protected override void EndSec()
        {
            this.Close();
        }

        private void ExchangeCount_KeyDown(object sender, KeyEventArgs e)

        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(ExchangeCount.Text))
                {
                    trrnbl.ShowMessage("E102");
                    ExchangeCount.Focus();
                    ExchangeCount.MoveNext = false;
                    
                }
                else
                {
                    countmoney = Convert.ToInt32(ExchangeCount.Text);
                }

                if (ExchangeDenomination.SelectedValue.ToString()=="-1")
                {

                    trrnbl.ShowMessage("E102");
                    ExchangeDenomination.Select();
                    ExchangeDenomination.MoveNext = false;
                    ExchangeCount.MoveNext = false;
                }
                else
                {
                   
                    combovalue = ExchangeDenomination.SelectedValue.ToString();
                    moneytype = Convert.ToInt32(combovalue);
                    
                }
                valid = true;
                displayData();
            }

           
        }
        private void ExchangeDenomination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (valid)
            {
                if (ExchangeDenomination.SelectedValue.ToString() == "-1")
                {

                    trrnbl.ShowMessage("E102");
                    ExchangeDenomination.Focus();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ExchangeCount.Text))
                    {
                        trrnbl.ShowMessage("E102");
                        ExchangeCount.Focus();

                    }
                    else
                    {
                        countmoney = Convert.ToInt32(ExchangeCount.Text);
                    }
                    combovalue = ExchangeDenomination.SelectedValue.ToString();
                    moneytype = Convert.ToInt32(combovalue);

                }
                displayData();
            }
           
        }

        private void frmTempoRegiRyougaeNyuuryoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

      


        //private void ExchangeCount_Leave(object sender, EventArgs e)
        //{

        //    if (string.IsNullOrWhiteSpace(ExchangeCount.Text))
        //    {
        //        trrnbl.ShowMessage("E102");
        //        ExchangeCount.Focus();
        //        ExchangeCount.MoveNext = false;

        //    }
        //    else
        //    {
        //        countmoney = Convert.ToInt32(ExchangeCount.Text);
        //        if (ExchangeDenomination.SelectedValue.ToString() == "-1")
        //        {

        //            trrnbl.ShowMessage("E102");
        //            // ExchangeDenomination.MoveNext = false;
        //            ExchangeDenomination.Select();

        //        }
        //        else
        //        {

        //            combovalue = ExchangeDenomination.SelectedValue.ToString();
        //        moneytype = Convert.ToInt32(combovalue);

        //        //        }


        //    }

        //    displayData();


        //}
    }
}
