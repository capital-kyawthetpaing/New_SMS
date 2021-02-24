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
using DL;
using Entity;
using Base.Client;
using CKM_Controls;
using Search;
using System.Threading;
using System.Diagnostics;

namespace TempoRegiNyuukinTouroku
{
    public partial class TempoRegiNyuukinNyuuryoku : ShopBaseForm
    {
        TempoRegiNyuukinNyuuryoku_BL trntBL = new TempoRegiNyuukinNyuuryoku_BL();
        D_DepositHistory_Entity ddpe = new D_DepositHistory_Entity();
        D_Collect_Entity dce = new D_Collect_Entity();

        public TempoRegiNyuukinNyuuryoku()
        {
            Start_Display();
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
            Stop_DisplayService();
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
            DataTable dt = new DataTable();
            dt = trntBL.SimpleSelect1("70", ChangeDate.Replace("/", "-"), StoreCD);
            if (dt.Rows.Count > 0)
            {
                trntBL.ShowMessage("E252");
                return false;
            }
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
            RunDisplay_Service();
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
                    //DataTable dt = new DataTable();
                    //dt = trntBL.SimpleSelect1("70", ChangeDate.Replace("/", "-"), StoreCD);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    trntBL.ShowMessage("E252");
                    //}
                    ddpe = GetDepositEntity();
                    if (trntBL.TempoNyuukinTouroku_D_DepositHistory_InsertUpdate(ddpe))
                    {
                        if (!string.IsNullOrWhiteSpace(txtCustomerCD.Text))
                        {

                            dce = GetDCollectData();
                            trntBL.TempoNyuukinTouroku_D_Collect_Insert(dce);
                        }
                        //  

                        if (Base_DL.iniEntity.IsDM_D30Used)
                        {
                            RunConsole();
                        }
                        else
                        {
                            trntBL.ShowMessage("I101");
                        }
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
                else
                {
                    PreviousCtrl.Focus();
                }

            }
        }

        private D_DepositHistory_Entity GetDepositEntity()
        {
            ddpe = new D_DepositHistory_Entity
            {
                StoreCD = StoreCD,
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
                //CustomerCD = string.Empty,
                CustomerCD = txtCustomerCD.Text,
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
                txtCustomerCD.Focus();
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
            string programID = "TempoRegiTorihikiReceipt";
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "2";
            string depositNo = bbl.SimpleSelect1("52", "", Application.ProductName, "", "").Rows[0]["DepositNO"].ToString();
            string cmdLine = InCompanyCD + " " + InOperatorCD + " " + Login_BL.GetHostName() + " " + Mode + " " + depositNo; //parameter
            try
            {
                ///movedBegin
                try
                {
                    Printer_Open(filePath, programID, cmdLine + "");
                    CDO_Open();
                    //  Parallel.Invoke(() => CDO_Open(), () => Printer_Open(filePath, programID, cmdLine));
                    //Parallel.Invoke(() => CDO_Open(), () => Printer_Open(filePath, programID, cmdLine));
                }
                catch(Exception ex) { MessageBox.Show("Parallel function worked and cant dispose instance. . . " + ex.Message); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Stop_DisplayService();
        }
        protected void CDO_Open()
        {
            if (Base_DL.iniEntity.IsDM_D30Used)
            {
                CashDrawerOpen op = new CashDrawerOpen();  //2020_06_24 
                op.OpenCashDrawer(); //2020_06_24     << PTK
            }
        }
        protected void Printer_Open(string filePath,string programID, string cmdLine)
        {
            try
            {
                //try
                //{
                //    cdo.RemoveDisplay(true);
                //    cdo.RemoveDisplay(true);
                //}
                //catch { }
               // System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
                 var pro = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
                // pro.WaitForExit();
                //try
                //{
                //    cdo.SetDisplay(true, true, "");
                //    cdo.RemoveDisplay(true);
                //    cdo.RemoveDisplay(true);
                //   // cdo.SetDisplay(false, false, "", Up, Lp);
                //}
                //catch
                //{
                //    MessageBox.Show("P0. .  .");
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected async Task RunParallel(string fp, string pi,string cl)
        {
            //Parallel.Invoke(() => CDO_Open(),
            //    () => Printer_Open(fp, pi, cl));
            var task1 = Task.Factory.StartNew(() => CDO_Open());
            var task2 = Task.Factory.StartNew(() => Printer_Open(fp, pi, cl));

            await Task.WhenAll(task1, task2);
        }
        private void frmTempoRegiTsurisenJyunbi_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void RunDisplay_Service()  // Make when we want to run display_service
        {
            try
            {
                //Login_BL bbl_1 = new Login_BL();
                //if (Base_DL.iniEntity.IsDM_D30Used)
                //{
                //    cdo.RemoveDisplay(true);
                //    cdo.RemoveDisplay(true);
                //    bbl_1.Display_Service_Update(true);

                //    bbl_1.Display_Service_Enabled(true);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in removing display. . .");
            }
        }
        private void Stop_DisplayService(bool isForced = true)
        {
            //if (Base_DL.iniEntity.IsDM_D30Used && Process.GetProcessesByName("Display_Service").Count() == 1)
            //{

            //    Login_BL bbl_1 = new Login_BL();
            //    if (bbl_1.ReadConfig())
            //    {
            //        bbl_1.Display_Service_Update(false);
            //        Thread.Sleep(1 * 1000);
            //        bbl_1.Display_Service_Enabled(false);
            //    }
            //    else
            //    {
            //        bbl_1.Display_Service_Update(false);
            //        Thread.Sleep(1 * 1000);
            //        bbl_1.Display_Service_Enabled(false);
            //    }

            //    try
            //    {
            //        Kill("Display_Service");
            //    }
            //    catch (Exception ex)
            //    {
            //        //  MessageBox.Show(ex.StackTrace.ToString());
            //    }
            //    if (isForced && Base_DL.iniEntity.IsDM_D30Used) cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
            //    //Base_DL.iniEntity.CDO_DISPLAY.SetDisplay(true, true,Base_DL.iniEntity.DefaultMessage);
            //}
            //else
            //{
              
            //    if (isForced && Base_DL.iniEntity.IsDM_D30Used) cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
            //}

        }
        private void Start_Display()
        {
            //try
            //{
            //    if (Base_DL.iniEntity.IsDM_D30Used && Process.GetProcessesByName("Display_Service").Count() == 1)
            //    {
            //        Login_BL bbl_1 = new Login_BL();
            //        if (bbl_1.ReadConfig())
            //        {
            //            bbl_1.Display_Service_Update(false);
            //            Thread.Sleep(1 * 1000);
            //            bbl_1.Display_Service_Enabled(false);
            //        }
            //        else
            //        {
            //            bbl_1.Display_Service_Update(false);
            //            Thread.Sleep(1 * 1000);
            //            bbl_1.Display_Service_Enabled(false);
            //        }
            //        Kill("Display_Service");
            //    }

            //}
            //catch (Exception ex) { MessageBox.Show("Cant remove on second time" + ex.StackTrace); }
        }
        protected void Kill(string pth)
        {
            try
            {
                Process[] processCollection = Process.GetProcessesByName(pth.Replace(".exe", ""));
                foreach (Process p in processCollection)
                {
                    p.Kill();
                }

                Process[] processCollections = Process.GetProcessesByName(pth + ".exe");
                foreach (Process p in processCollections)
                {
                    p.Kill();
                }
            }
            catch { }
        }
        EPSON_TM30.CashDrawerOpen cdo = new EPSON_TM30.CashDrawerOpen();
    }
}

