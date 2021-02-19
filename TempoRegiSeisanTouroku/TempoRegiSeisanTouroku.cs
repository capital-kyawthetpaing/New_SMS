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

namespace TempoRegiSeisanTouroku
{
    public partial class frmTempoRegiSeisanTouroku : ShopBaseForm
    {
        TempoRegiSeisanTouroku_BL seisanbl = new TempoRegiSeisanTouroku_BL();
        D_DepositHistory_Entity dphe = new D_DepositHistory_Entity();
        D_Sales_Entity dse = new D_Sales_Entity();
        D_Juchuu_Entity dje = new D_Juchuu_Entity();
        D_StoreCalculation_Entity dsce = new D_StoreCalculation_Entity();

        decimal a = 0, b = 0, c = 0, d = 0, e = 0,f= 0 , g = 0, total = 0 ,balance = 0, storage = 0;

        private void frmTempoRegiSeisanTouroku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    
        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txt10000.Text))
                    txt10000.Text = "0";
                if (string.IsNullOrWhiteSpace(txt5000.Text))
                    txt5000.Text = "0";
                if (string.IsNullOrWhiteSpace(txt2000.Text))
                    txt2000.Text = "0";
                if (string.IsNullOrWhiteSpace(txt1000.Text))
                    txt1000.Text = "0";
                if (string.IsNullOrWhiteSpace(txt500.Text))
                    txt500.Text = "0";
                if (string.IsNullOrWhiteSpace(txt100.Text))
                    txt100.Text = "0";
                if (string.IsNullOrWhiteSpace(txt50.Text))
                    txt50.Text = "0";
                if (string.IsNullOrWhiteSpace(txt10.Text))
                    txt10.Text = "0";
                if (string.IsNullOrWhiteSpace(txt5.Text))
                    txt5.Text = "0";
                if (string.IsNullOrWhiteSpace(txt1.Text))
                    txt1.Text = "0";
                if (string.IsNullOrWhiteSpace(txtotheramount.Text))
                    txtotheramount.Text = "0";
               
                balance = (Convert.ToDecimal(txt10000.Text)*10000) + (Convert.ToDecimal(txt5000.Text)*5000) + (Convert.ToDecimal(txt2000.Text)*2000) + (Convert.ToDecimal(txt1000.Text)*1000)+
                    (Convert.ToDecimal(txt500.Text)*500) + (Convert.ToDecimal(txt100.Text)*100) + (Convert.ToDecimal(txt50.Text)*50) + (Convert.ToDecimal(txt10.Text)*10) +
                    (Convert.ToDecimal(txt5.Text)*5) + (Convert.ToDecimal(txt1.Text)*1) + Convert.ToDecimal(txtotheramount.Text);
                //lblCashBalance.Text = balance.ToString();
                string first = string.Empty;
                first = balance.ToString(); 
                first = string.IsNullOrWhiteSpace(first) ? "0" : string.Format("{0:#,#}", Convert.ToInt64(first));
                lblCashBalance.Text = (string.IsNullOrWhiteSpace(first) ? "0" : first);

                //string total = txtTotal.Text;

                string cash = string.Empty;
                storage = balance - Convert.ToDecimal(txtTotal.Text);
                cash = storage.ToString();
                cash = string.IsNullOrWhiteSpace(cash) ? "0" : string.Format("{0:#,#}", Convert.ToInt64(cash));
                lblCashStorage.Text = "¥ " + (string.IsNullOrWhiteSpace(cash) ? "0" : cash);
                //lblCashStorage.Text = (string.IsNullOrWhiteSpace(cash) ? "0" : first);
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        public frmTempoRegiSeisanTouroku()
        {
            InitializeComponent();
        }

        private void frmTempoRegiSeisanTouroku_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiSeisanTouroku";
            
            StartProgram();

            lblDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            //lblDate.Text = DateTime.Today.ToShortDateString();
            DisplayData();

            //SelectData();

            txt10000.Focus();
            Text = "精算登録";
        }

        public override void FunctionProcess(int index)  
        {
            switch (index + 1)
            {
                case 2:
                    if (ErrorCheck())
                    {
                        if (chkDel.Checked == false)
                        {
                            if (bbl.ShowMessage("Q101") == DialogResult.Yes)
                            {
                                InsertUpdate();
                            }
                        }
                        else if (chkDel.Checked == true)
                        {
                            Delete();
                        }
                    }              
                    break;
            }
        }

        public void DisplayData()
        {
            string data = StoreCD;
            string date = DateTime.Now.ToString("yyyy/MM/dd");

            #region D_StoreCalculation
            dsce.StoreCD = data;
            dsce.CalculationDate = date;
            DataTable dtStore = new DataTable();
            dtStore = seisanbl.D_Store_Calculation_SelectForSeiSan(dsce);
            if(dtStore.Rows.Count > 0 )
            {
                txt10000.Text = dtStore.Rows[0]["Yen10000"].ToString();
                txt5000.Text = dtStore.Rows[0]["Yen5000"].ToString();
                txt2000.Text = dtStore.Rows[0]["Yen2000"].ToString();
                txt1000.Text = dtStore.Rows[0]["Yen1000"].ToString();
                txt500.Text = dtStore.Rows[0]["Yen500"].ToString();
                txt100.Text = dtStore.Rows[0]["Yen100"].ToString();
                txt50.Text = dtStore.Rows[0]["Yen50"].ToString();
                txt10.Text = dtStore.Rows[0]["Yen10"].ToString();
                txt5.Text = dtStore.Rows[0]["Yen5"].ToString();
                txt1.Text = dtStore.Rows[0]["Yen1"].ToString();
                txtotheramount.Text = dtStore.Rows[0]["OtherYen"].ToString();
                lblCashBalance.Text = dtStore.Rows[0]["TotalCash"].ToString();

                chkDel.Enabled = true;
            }
            else
            {
                txt10000.Text = "0";
                txt5000.Text = "0";
                txt2000.Text = "0";
                txt1000.Text = "0";
                txt500.Text = "0";
                txt100.Text = "0";
                txt50.Text = "0";
                txt10.Text = "0";
                txt5.Text = "0";
                txt1.Text = "0";
                txtotheramount.Text = "0";
                lblCashBalance.Text = "0";

                chkDel.Enabled = false;
            }
            #endregion

            #region D_DepositHistory
            dphe.StoreCD = data;
            dphe.ChangeDate = date;
            DataTable dtdeposit = new DataTable();
            dtdeposit = seisanbl.D_DepositHistory_SelectForSeisan(dphe);
            if (dtdeposit.Rows.Count > 0)
            {
                var depostiArr = dtdeposit.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();
                for (int i = 0; i < depostiArr.Length; i++)
                {
                    depostiArr[i] = string.Format("{0:#,##0}", int.Parse(depostiArr[i]));
                }

                //txtChange.Text = dtdeposit.Rows[0]["Change"].ToString();
                //txtCashSale.Text = dtdeposit.Rows[0]["CashSale"].ToString();
                //txtGift.Text = dtdeposit.Rows[0]["Gift"].ToString();
                //txtDeposit.Text = dtdeposit.Rows[0]["CashDeposit"].ToString();
                //txtPayment.Text = dtdeposit.Rows[0]["CashPayment"].ToString();
                //txtTotal.Text = dtdeposit.Rows[0]["CashTotal"].ToString();

                txtChange.Text = depostiArr[0];
                txtCashSale.Text = depostiArr[1];
                txtGift.Text = depostiArr[2];
                txtDeposit.Text = depostiArr[3];
                txtPayment.Text = depostiArr[4];
             
                string total = string.Empty;
                total = (Convert.ToDecimal(txtChange.Text) + Convert.ToDecimal(txtCashSale.Text) - Convert.ToDecimal(txtGift.Text)
                + Convert.ToDecimal(txtDeposit.Text) - Convert.ToDecimal(txtPayment.Text)).ToString();
                if(total == "0")
                {
                    txtTotal.Text = "0";
                }
                else
                {
                    total = string.IsNullOrWhiteSpace(total) ? "0" : string.Format("{0:#,#}", Convert.ToInt64(total));
                    txtTotal.Text = (string.IsNullOrWhiteSpace(total) ? "0" : total);
                }


                //txtTransfer.Text = dtdeposit.Rows[0]["DepositTransfer"].ToString();
                //txtDepositCash.Text = dtdeposit.Rows[0]["DepositCash"].ToString();
                //txtDepositCheck.Text = dtdeposit.Rows[0]["DepositCheck"].ToString();
                //txtDepositBill.Text = dtdeposit.Rows[0]["DepositBill"].ToString();
                //txtDepositOffset.Text = dtdeposit.Rows[0]["DepositOffset"].ToString();
                //txtdepositAdjustment.Text = dtdeposit.Rows[0]["DepositAdjustment"].ToString();
                //txtDepositTotal.Text = dtdeposit.Rows[0]["DepositTotal"].ToString();

                txtTransfer.Text = depostiArr[5];
                txtDepositCash.Text = depostiArr[6];
                txtDepositCheck.Text = depostiArr[7];
                txtDepositBill.Text = depostiArr[8];
                txtDepositOffset.Text = depostiArr[9];
                txtdepositAdjustment.Text = depostiArr[10];

                string deposittatal = string.Empty;
                deposittatal = (Convert.ToDecimal(txtTransfer.Text) + Convert.ToDecimal(txtDepositCash.Text) +
                                       Convert.ToDecimal(txtDepositCheck.Text) + Convert.ToDecimal(txtDepositBill.Text) +
                                        Convert.ToDecimal(txtDepositOffset.Text) + Convert.ToDecimal(txtdepositAdjustment.Text)).ToString();
                if(deposittatal == "0")
                {
                    txtDepositTotal.Text = "0";
                }
                else
                {
                    deposittatal = string.IsNullOrWhiteSpace(deposittatal) ? "0" : string.Format("{0:#,#}", Convert.ToInt64(deposittatal));
                    txtDepositTotal.Text = (string.IsNullOrWhiteSpace(deposittatal) ? "0" : deposittatal);
                }

                //txtReturn.Text = dtdeposit.Rows[0]["DepositReturns"].ToString();
                //txtDiscount.Text = dtdeposit.Rows[0]["DepositDiscount"].ToString();

                //txtCancel.Text = dtdeposit.Rows[0]["DepositCancel"].ToString();

                //txtPaymentTransfer.Text = dtdeposit.Rows[0]["PaymentTransfer"].ToString();
                //txtPaymentCash.Text = dtdeposit.Rows[0]["PaymentCash"].ToString();
                //txtPaymentCheck.Text = dtdeposit.Rows[0]["PaymentCheck"].ToString();
                //txtPaymentBill.Text = dtdeposit.Rows[0]["PaymentBill"].ToString();
                //txtPaymentOffset.Text = dtdeposit.Rows[0]["PaymentOffset"].ToString();
                //txtPaymentadjustment.Text = dtdeposit.Rows[0]["PaymentAdjustment"].ToString();
                //txtPaymentTotal.Text = dtdeposit.Rows[0]["TotalPayment"].ToString();

                txtReturn.Text = depostiArr[12];
                txtDiscount.Text = depostiArr[12];
                txtCancel.Text = depostiArr[14];
                txtPaymentTransfer.Text = depostiArr[15];
                txtPaymentCash.Text = depostiArr[16];
                txtPaymentCheck.Text = depostiArr[17];
                txtPaymentBill.Text = depostiArr[18];
                txtPaymentOffset.Text = depostiArr[19];
                txtPaymentadjustment.Text = depostiArr[20];
                txtPaymentTotal.Text = depostiArr[21];

                //lblCashStorage.Text = (Convert.ToDecimal(lblCashBalance.Text) - Convert.ToDecimal(txtTotal.Text)).ToString();
                //        lblCashStorage.Text = "¥" + dtcash.Rows[0]["CashStorage"].ToString();
            }
            else
            {
                txtChange.Text = "0";
                txtCashSale.Text = "0";
                txtGift.Text = "0";
                txtDeposit.Text = "0";
                txtPayment.Text = "0";
                txtTotal.Text = "0";

                txtTransfer.Text = "0";
                txtDepositCash.Text = "0";
                txtDepositCheck.Text = "0";
                txtDepositBill.Text = "0";
                txtDepositOffset.Text = "0";
                txtdepositAdjustment.Text = "0";
                txtDepositTotal.Text = "0";
                txtReturn.Text = "0";
                txtDiscount.Text = "0";
                txtCancel.Text = "0";

                txtPaymentTransfer.Text = "0";
                txtPaymentCash.Text = "0";
                txtPaymentCheck.Text = "0";
                txtPaymentBill.Text = "0";
                txtPaymentOffset.Text = "0";
                txtPaymentadjustment.Text = "0";
                txtPaymentTotal.Text = "0";

                lblCashStorage.Text = "￥ 0";
            }
           
            string cash = string.Empty;
            if(string.IsNullOrWhiteSpace(lblCashBalance.Text))
            {
                lblCashBalance.Text = "0";
            }
            cash = (Convert.ToDecimal(lblCashBalance.Text) - Convert.ToDecimal(txtTotal.Text)).ToString();
            cash = string.IsNullOrWhiteSpace(cash) ? "0" : string.Format("{0:#,#}", Convert.ToInt64(cash));
            lblCashStorage.Text = "¥ " + (string.IsNullOrWhiteSpace(cash) ? "0" : cash);
            #endregion



            #region D_Sale
            dse.StoreCD = data;
            dse.ChangeDate = date;
            DataTable dtsale = new DataTable();
            dtsale = seisanbl.D_Sale_SelectForSeisan(dse);
            if(dtsale .Rows.Count > 0)
            {
                
                var stringArr = dtsale.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();
                for(int i = 0; i<stringArr.Length; i++)
                {
                    stringArr[i] = string.Format("{0:#,##0}", int.Parse(stringArr[i]));
                }

                //txtSlipsNum.Text = dtsale.Rows[0]["SlipNum"].ToString();
                ////txtCustomerNum.Text = dtcash.Rows[0]["NumOfCustomer"].ToString();
                //txtTotalSales.Text = dtsale.Rows[0]["TotalSales"].ToString();
                //txtamount8.Text = dtsale.Rows[0]["Amount8"].ToString();
                //txtamount10.Text = dtsale.Rows[0]["Amount10"].ToString();
                //txtTaxamount.Text = dtsale.Rows[0]["TaxAmount"].ToString();
                //txtsaletax.Text = dtsale.Rows[0]["SalesExcludingTax"].ToString();
                //txtforeigntax8.Text = dtsale.Rows[0]["Foreigntax8"].ToString();
                //txtforeigntax10.Text = dtsale.Rows[0]["Foreigntax10"].ToString();
                //txtconsumptiontax.Text = dtsale.Rows[0]["Consumpitontax"].ToString();
                //txttaxincludesale.Text = dtsale.Rows[0]["TaxIncludeSales"].ToString();
                //txtSaleCash.Text = dtsale.Rows[0]["Cash"].ToString();
                //txtHanging.Text = dtsale.Rows[0]["Hanging"].ToString();
                //txtVISA.Text = dtsale.Rows[0]["VISA"].ToString();
                //txtJCB.Text = dtsale.Rows[0]["JCB"].ToString();


                txtSlipsNum.Text = stringArr[0];
                //txtCustomerNum.Text = dtcash.Rows[0]["NumOfCustomer"].ToString();
                txtTotalSales.Text = stringArr[1];
                txtamount8.Text = stringArr[2];
                txtamount10.Text = stringArr[3];
                txtTaxamount.Text = stringArr[4];
                txtsaletax.Text = stringArr[5];
                txtforeigntax8.Text = stringArr[6];
                txtforeigntax10.Text = stringArr[7];
                txtconsumptiontax.Text = stringArr[8];
                txttaxincludesale.Text = stringArr[9];
                txtSaleCash.Text = stringArr[10];
                txtHanging.Text = stringArr[11];
                txtVISA.Text = stringArr[12];
                txtJCB.Text = stringArr[13];
                txtOther.Text = stringArr[14];


            }
            else
            {
                txtSlipsNum.Text = "0";
                //txtCustomerNum.Text = "0";
                txtTotalSales.Text = "0";
                txtamount8.Text = "0";
                txtamount10.Text = "0";
                txtTaxamount.Text = "0";
                txtsaletax.Text = "0";
                txtforeigntax8.Text = "0";
                txtforeigntax10.Text = "0";
                txtconsumptiontax.Text = "0";
                txttaxincludesale.Text = "0";
                txtSaleCash.Text = "0";
                txtHanging.Text = "0";
                txtVISA.Text = "0";
                txtJCB.Text = "0";
                txtOther.Text = "0";
            }
            #endregion

            #region D_Juchuu
            dje.StoreCD = data;
            dje.ChangeDate = date;
            DataTable dtjuchuu = new DataTable();
            dtjuchuu = seisanbl.D_Juchuu_SelectForSeisan(dje);
            if(dtjuchuu.Rows.Count > 0)
            {
                txtCustomerNum.Text = dtjuchuu.Rows[0]["NumOfCustomer"].ToString();
            }
            else
            {
                txtCustomerNum.Text = "0";
            }
            #endregion
            
        }

        //public void DisplayData()
        //{
        //    string data = InOperatorCD;
        //    string date = DateTime.Now.ToString("yyyy/MM/dd");
        //    dsce.StoreCD = data;
        //    dsce.CalculationDate = date;
        //    DataTable dtcash = new DataTable();
        //    dtcash = seisanbl.D_StoreCalculation_Select(dsce);
        //    if(dtcash.Rows.Count > 0)
        //    {
        //        #region Tab1
        //        txt10000.Text = dtcash.Rows[0]["Yen10000"].ToString();
        //        txt5000.Text = dtcash.Rows[0]["Yen5000"].ToString();
        //        txt2000.Text = dtcash.Rows[0]["Yen2000"].ToString();
        //        txt1000.Text = dtcash.Rows[0]["Yen1000"].ToString();
        //        txt500.Text = dtcash.Rows[0]["Yen500"].ToString();
        //        txt100.Text = dtcash.Rows[0]["Yen100"].ToString();
        //        txt50.Text = dtcash.Rows[0]["Yen50"].ToString();
        //        txt10.Text = dtcash.Rows[0]["Yen10"].ToString();
        //        txt5.Text = dtcash.Rows[0]["Yen5"].ToString();
        //        txt1.Text = dtcash.Rows[0]["Yen1"].ToString();
        //        txtotheramount.Text = dtcash.Rows[0]["OtherYen"].ToString();
        //        lblCashBalance.Text = dtcash.Rows[0]["TotalCash"].ToString();

        //        txtChange.Text = dtcash.Rows[0]["Change"].ToString();
        //        txtCashSale.Text = dtcash.Rows[0]["CashSale"].ToString();
        //        txtGift.Text = dtcash.Rows[0]["Gift"].ToString();
        //        txtDeposit.Text = dtcash.Rows[0]["CashDeposit"].ToString();
        //        txtPayment.Text = dtcash.Rows[0]["CashPayment"].ToString();
        //        txtTotal.Text = dtcash.Rows[0]["CashTotal"].ToString();
        //        //txtTotal.Text = (Convert.ToDecimal(txtChange.Text) + Convert.ToDecimal(txtCashSale.Text) - Convert.ToDecimal(txtGift.Text)
        //        //+ Convert.ToDecimal(txtDeposit.Text) - Convert.ToDecimal(txtPayment.Text)).ToString();

        //        txtSlipsNum.Text = dtcash.Rows[0]["SlipNum"].ToString();
        //        txtCustomerNum.Text = dtcash.Rows[0]["NumOfCustomer"].ToString();
        //        txtTotalSales.Text = dtcash.Rows[0]["TotalSales"].ToString();

        //        //lblCashStorage.Text = (Convert.ToDecimal(lblCashBalance.Text) - Convert.ToDecimal(txtTotal.Text)).ToString();
        //        lblCashStorage.Text = "¥" + dtcash.Rows[0]["CashStorage"].ToString();

        //        #endregion

        //        #region Tab2

        //        txtamount8.Text = dtcash.Rows[0]["Amount8"].ToString();
        //        txtamount10.Text = dtcash.Rows[0]["Amount10"].ToString();
        //        txtTaxamount.Text = dtcash.Rows[0]["TaxAmount"].ToString();
        //        txtsaletax.Text = dtcash.Rows[0]["SalesExcludingTax"].ToString();
        //        txtforeigntax8.Text = dtcash.Rows[0]["Foreigntax8"].ToString();
        //        txtforeigntax10.Text = dtcash.Rows[0]["Foreigntax10"].ToString();
        //        txtconsumptiontax.Text = dtcash.Rows[0]["Consumpitontax"].ToString();
        //        txttaxincludesale.Text = dtcash.Rows[0]["TaxIncludeSales"].ToString();
        //        txtSaleCash.Text = dtcash.Rows[0]["Cash"].ToString();
        //        txtHanging.Text = dtcash.Rows[0]["Hanging"].ToString();
        //        txtVISA.Text = dtcash.Rows[0]["VISA"].ToString();
        //        txtJCB.Text = dtcash.Rows[0]["JCB"].ToString();

        //        txtTransfer.Text = dtcash.Rows[0]["DepositTransfer"].ToString();
        //        txtDepositCash.Text = dtcash.Rows[0]["DepositCash"].ToString();
        //        txtDepositCheck.Text = dtcash.Rows[0]["DepositCheck"].ToString();
        //        txtDepositBill.Text = dtcash.Rows[0]["DepositBill"].ToString();
        //        txtDepositOffset.Text = dtcash.Rows[0]["DepositOffset"].ToString();
        //        txtdepositAdjustment.Text = dtcash.Rows[0]["DepositAdjustment"].ToString();
        //        txtDepositTotal.Text = dtcash.Rows[0]["DepositTotal"].ToString();
        //        txtReturn.Text = dtcash.Rows[0]["DepositReturns"].ToString();
        //        txtDiscount.Text = dtcash.Rows[0]["DepositDiscount"].ToString();
        //        txtCancel.Text = dtcash.Rows[0]["DepositCancel"].ToString();

        //        txtPaymentTransfer.Text = dtcash.Rows[0]["PaymentTransfer"].ToString();
        //        txtPaymentCash.Text = dtcash.Rows[0]["PaymentCash"].ToString();
        //        txtPaymentCheck.Text = dtcash.Rows[0]["PaymentCheck"].ToString();
        //        txtPaymentBill.Text = dtcash.Rows[0]["PaymentBill"].ToString();
        //        txtPaymentOffset.Text = dtcash.Rows[0]["PaymentOffset"].ToString();
        //        txtPaymentadjustment.Text = dtcash.Rows[0]["PaymentAdjustment"].ToString();
        //        txtPaymentTotal.Text = dtcash.Rows[0]["TotalPayment"].ToString();

        //        #endregion   
        //    } 
        //    else
        //    {
        //        #region Tab1
        //        txt10000.Text = "0";
        //        txt5000.Text = "0";
        //        txt2000.Text = "0";
        //        txt1000.Text = "0";
        //        txt500.Text = "0";
        //        txt100.Text = "0";
        //        txt50.Text = "0";
        //        txt10.Text = "0";
        //        txt5.Text = "0";
        //        txt1.Text = "0";
        //        txtotheramount.Text = "0";
        //        lblCashBalance.Text = "0";

        //        txtChange.Text = "0";
        //        txtCashSale.Text = "0";
        //        txtGift.Text = "0";
        //        txtDeposit.Text = "0";
        //        txtPayment.Text = "0";
        //        txtTotal.Text = "0";

        //        txtSlipsNum.Text = "0";
        //        txtCustomerNum.Text = "0";
        //        txtTotalSales.Text = "0";

        //        lblCashStorage.Text = "￥ 0";

        //        #endregion

        //        #region Tab2

        //        txtamount8.Text = "0";
        //        txtamount10.Text = "0";
        //        txtTaxamount.Text = "0";
        //        txtsaletax.Text = "0";
        //        txtforeigntax8.Text = "0";
        //        txtforeigntax10.Text = "0";
        //        txtconsumptiontax.Text = "0";
        //        txttaxincludesale.Text = "0";
        //        txtSaleCash.Text = "0";
        //        txtHanging.Text = "0";
        //        txtVISA.Text = "0";
        //        txtJCB.Text = "0";

        //        txtTransfer.Text = "0";
        //        txtDepositCash.Text = "0";
        //        txtDepositCheck.Text = "0";
        //        txtDepositBill.Text = "0";
        //        txtDepositOffset.Text = "0";
        //        txtdepositAdjustment.Text = "0";
        //        txtDepositTotal.Text = "0";
        //        txtReturn.Text = "0";
        //        txtDiscount.Text = "0";
        //        txtCancel.Text = "0";

        //        txtPaymentTransfer.Text = "0";
        //        txtPaymentCash.Text = "0";
        //        txtPaymentCheck.Text = "0";
        //        txtPaymentBill.Text = "0";
        //        txtPaymentOffset.Text = "0";
        //        txtPaymentadjustment.Text = "0";
        //        txtPaymentTotal.Text = "0";
        //        #endregion   
        //    }

        //}

        private void SelectData()
        {
            #region Tab1
            
            DataTable dt = new DataTable();
            dt = seisanbl.D_StoreData_Select();
            if (dt.Rows.Count > 0)
                txtChange.Text = dt.Rows[0]["Change"].ToString();
            else
                txtChange.Text = "0";
            a = Convert.ToDecimal(txtChange.Text);

            dphe.DepositKBN = "1";
            dphe.DepositKBN1 = "0";
            DataTable dtgaku1 = new DataTable();
            dtgaku1 = seisanbl.D_DepositＨistory_Gaku_Select(dphe);
            if (dtgaku1.Rows.Count > 0)
                txtCashSale.Text = dtgaku1.Rows[0]["DepositGaku"].ToString();
            else
            {             
                txtCashSale.Text = "0";
            }
            if (string.IsNullOrWhiteSpace(txtCashSale.Text))
                txtCashSale.Text = "0";

            b = Convert.ToDecimal(txtCashSale.Text);

            dphe.DepositKBN = "7";
            dphe.DepositKBN1 = "0";
            DataTable dtgaku2 = new DataTable();
            dtgaku2 = seisanbl.D_DepositＨistory_Gaku_Select(dphe);
            if (dtgaku2.Rows.Count > 0)
                txtGift.Text = dtgaku2.Rows[0]["DepositGaku"].ToString();
            else
            {              
                txtGift.Text = "0";              
            }
            if (string.IsNullOrWhiteSpace(txtGift.Text))
            {
                txtGift.Text = "0";
            }

            c = Convert.ToDecimal(txtGift.Text);

            dphe.DepositKBN = "2";
            dphe.DepositKBN1 = "4";
            DataTable dtgaku3 = new DataTable();
            dtgaku3 = seisanbl.D_DepositＨistory_Gaku_Select(dphe);
            if (dtgaku3.Rows.Count > 0)
                txtDeposit.Text = dtgaku3.Rows[0]["DepositGaku"].ToString();
            else
            {               
                txtDeposit.Text = "0";
            }
            if (string.IsNullOrWhiteSpace(txtDeposit.Text))
                txtDeposit.Text = "0";

            d = Convert.ToDecimal(txtDeposit.Text);

            dphe.DepositKBN = "3";
            dphe.DepositKBN1 = "5";
            DataTable dtgaku4 = new DataTable();
            dtgaku4 = seisanbl.D_DepositＨistory_Gaku_Select(dphe);
            if (dtgaku4.Rows.Count > 0)
                txtPayment.Text = dtgaku4.Rows[0]["DepositGaku"].ToString();
            else
            {             
                    txtPayment.Text = "0";        
            }
            if (string.IsNullOrWhiteSpace(txtPayment.Text))
                txtPayment.Text = "0";

            e = Convert.ToDecimal(txtPayment.Text);

            total = a + b - c + d - e;
            txtTotal.Text = total.ToString();

            dse.StoreCD = StoreCD;
            DataTable dtsale = new DataTable();
            dtsale = seisanbl.D_Sales_DataSelect(dse);
            if (dtsale.Rows.Count > 0)
            {
                txtSlipsNum.Text = dtsale.Rows[0]["SalesNo"].ToString();
                txtTotalSales.Text = dtsale.Rows[0]["SalesGaku"].ToString();
            }   
            else
            {
                txtSlipsNum.Text = "0";
                txtTotalSales.Text = "0";
            }

            if (string.IsNullOrWhiteSpace(txtSlipsNum.Text))
                txtSlipsNum.Text = "0";
            if (string.IsNullOrWhiteSpace(txtTotalSales.Text))
                txtTotalSales.Text = "0";

            dje.StoreCD = StoreCD;
            DataTable dtJuchuu = new DataTable();
            dtJuchuu = seisanbl.D_JuChuu_DataSelect(dje);
            if (dtJuchuu.Rows.Count > 0)
                txtCustomerNum.Text = dtJuchuu.Rows[0]["JuchuuNo"].ToString();
            else
            { 
                txtCustomerNum.Text = "0";
            }

            if (string.IsNullOrWhiteSpace(txtCustomerNum.Text))
                txtCustomerNum.Text = "0";

            #endregion
            //dse.StoreCD = StoreCD;
            //DataTable dtsale1 = new DataTable();
            //dtsale1 = seisanbl.D_Sales_DataSelect(dse);
            //if (dtsale.Rows.Count > 0)

            #region Tab2

            dse.StoreCD = StoreCD;
            DataTable dtsale1 = new DataTable();
            dtsale1 = seisanbl.D_Sales_DataSelect(dse);
            if (dtsale1.Rows.Count > 0 )
            {
                txtamount8.Text = dtsale1.Rows[0]["Amount8"].ToString();
                txtamount10.Text = dtsale1.Rows[0]["Amount10"].ToString();
                txtTaxamount.Text = dtsale1.Rows[0]["TaxAmount"].ToString();
                txtsaletax.Text = dtsale1.Rows[0]["SaleTax"].ToString();
                txtforeigntax8.Text = dtsale1.Rows[0]["ForeignTax8"].ToString();
                txtforeigntax10.Text = dtsale1.Rows[0]["ForeignTax10"].ToString();
                f = Convert.ToDecimal(txtforeigntax8.Text) + Convert.ToDecimal(txtforeigntax10.Text);
                txtconsumptiontax.Text = f.ToString();
                g = Convert.ToDecimal(txtsaletax.Text) + Convert.ToDecimal(txtconsumptiontax.Text);               
                txttaxincludesale.Text = g.ToString();
            }
            else
            {
                txtamount8.Text = "0";
                txtamount10.Text = "0";
                txtTaxamount.Text = "0";
                txtsaletax.Text = "0";
                txtforeigntax8.Text = "0";
                txtforeigntax10.Text = "0";
                f = Convert.ToDecimal(txtforeigntax8.Text) + Convert.ToDecimal(txtforeigntax10.Text);
                txtconsumptiontax.Text = f.ToString();
                g = Convert.ToDecimal(txtsaletax.Text) + Convert.ToDecimal(txtconsumptiontax.Text);
                txttaxincludesale.Text = g.ToString();
            }
               
            dphe.DepositKBN = "2";
            dphe.DepositKBN1 = "4";
            DataTable dtgaku5 = new DataTable();
            dtgaku5 = seisanbl.D_DepositＨistory_Gaku_Select(dphe);
            if (dtgaku5.Rows.Count > 0)
            {
                txtTransfer.Text = dtgaku5.Rows[0]["DepositGaku"].ToString();
                txtDepositCash.Text = dtgaku5.Rows[0]["DepositGaku"].ToString();
                txtDepositCheck.Text = dtgaku5.Rows[0]["DepositGaku"].ToString();
                txtDepositBill.Text = dtgaku5.Rows[0]["DepositGaku"].ToString();
                txtDepositOffset.Text = dtgaku5.Rows[0]["DepositGaku"].ToString();
                txtdepositAdjustment.Text = dtgaku5.Rows[0]["DepositGaku"].ToString();
            }
            else
            {
                txtTransfer.Text = "0";
                txtDepositCash.Text = "0";
                txtDepositCheck.Text = "0";
                txtDepositBill.Text = "0";
                txtDepositOffset.Text = "0";
                txtdepositAdjustment.Text = "0";
            }

            //txtDepositTotal.Text = dtgaku1.Rows[0]["DepositGaku"].ToString();
            //txtDepositPaymentTotal.Text = dtgaku1.Rows[0]["DepositGaku"].ToString();

            dphe.DepositKBN = "9";
            DataTable dtgaku7 = new DataTable();
            dtgaku7 = seisanbl.D_DepositＨistory_Gaku_Select(dphe);
            if (dtgaku7.Rows.Count > 0)
            {
                txtReturn.Text = dtgaku7.Rows[0]["DepositGaku"].ToString();
            }

            dphe.DepositKBN = "2";
            DataTable dtgaku9 = new DataTable();
            dtgaku9 = seisanbl.D_DepositＨistory_Gaku_Select(dphe);
            if (dtgaku9.Rows.Count > 0)
            {
                txtDiscount.Text = dtgaku9.Rows[0]["DepositGaku"].ToString();
            }

            dphe.DepositKBN = "8";
            DataTable dtgaku8 = new DataTable();
            dtgaku8 = seisanbl.D_DepositＨistory_Gaku_Select(dphe);
            if (dtgaku8.Rows.Count > 0)
            {
                txtCancel.Text = dtgaku8.Rows[0]["DepositGaku"].ToString();
            }

            dphe.DepositKBN = "3";
            dphe.DepositKBN1 = "5";
            DataTable dtgaku6 = new DataTable();
            dtgaku6 = seisanbl.D_DepositＨistory_Gaku_Select(dphe);
            if (dtgaku6.Rows.Count > 0)
            {
                txtPaymentTransfer.Text = dtgaku6.Rows[0]["DepositGaku"].ToString();
                txtPaymentCash.Text = dtgaku6.Rows[0]["DepositGaku"].ToString();
                txtPaymentCheck.Text = dtgaku6.Rows[0]["DepositGaku"].ToString();
                txtPaymentBill.Text = dtgaku6.Rows[0]["DepositGaku"].ToString();
                txtPaymentOffset.Text = dtgaku6.Rows[0]["DepositGaku"].ToString();
                txtPaymentadjustment.Text = dtgaku6.Rows[0]["DepositGaku"].ToString();
            }
            else
            {
                txtPaymentTransfer.Text = "0";
                txtPaymentCash.Text = "0";
                txtPaymentCheck.Text = "0";
                txtPaymentBill.Text = "0";
                txtPaymentOffset.Text = "0";
                txtPaymentadjustment.Text = "0";
            }
                
         #endregion 
        }
        
        private void InsertUpdate()
        {          
            dsce = GetStoreCalculation();       
            if (seisanbl.D_StoreCalculation_Insert_Update(dsce))
            {
                chkDel.Enabled = true;
                seisanbl.ShowMessage("I101");
                tabControl1.SelectedIndex = 0;
            }           
        }

        private void Delete()
        {
            string data = InOperatorCD;
            string date = DateTime.Now.ToString("yyyy/MM/dd");          
            dsce.StoreCD = data;
            dsce.CalculationDate = date;
            dsce.InsertOperator = InOperatorCD;
            dsce.ProgramID = InProgramID;
            dsce.PC = InPcID;
            if (seisanbl.D_StoreCalculation_Delete(dsce))
            {                
                seisanbl.ShowMessage("I102");
                txt10000.Text = "0";
                txt5000.Text = "0";
                txt2000.Text = "0";
                txt1000.Text = "0";
                txt500.Text = "0";
                txt100.Text = "0";
                txt50.Text = "0";
                txt10.Text = "0";
                txt5.Text = "0";
                txt1.Text = "0";
                txtotheramount.Text = "0";
                lblCashBalance.Text = "0";
                //lblCashStorage.Text = "¥ " + "0";

                string cash = string.Empty;
                if (string.IsNullOrWhiteSpace(lblCashBalance.Text))
                {
                    lblCashBalance.Text = "0";
                }
                cash = (Convert.ToDecimal(lblCashBalance.Text) - Convert.ToDecimal(txtTotal.Text)).ToString();
                cash = string.IsNullOrWhiteSpace(cash) ? "0" : string.Format("{0:#,#}", Convert.ToInt64(cash));
                lblCashStorage.Text = "¥ " + (string.IsNullOrWhiteSpace(cash) ? "0" : cash);

                chkDel.Checked = false;
                chkDel.Enabled = false;
                tabControl1.SelectedIndex = 0;
            }
        }

        private D_StoreCalculation_Entity GetStoreCalculation()
        {
            string date = DateTime.Today.ToString();
            dsce = new D_StoreCalculation_Entity
            {
                StoreCD = StoreCD,
                Change = txtChange.Text.Replace(",",""),
                Yen10000 = txt10000.Text.Replace(",", ""),
                Yen5000 = txt5000.Text.Replace(",", ""),
                Yen2000 = txt2000.Text.Replace(",", ""),
                Yen1000 = txt1000.Text.Replace(",", ""),
                Yen500 = txt500.Text.Replace(",", ""),
                Yen100 = txt100.Text.Replace(",", ""),
                Yen50 = txt50.Text.Replace(",", ""),
                Yen10 = txt10.Text.Replace(",", ""),
                Yen5 = txt5.Text.Replace(",", ""),
                Yen1 = txt1.Text.Replace(",", ""),
                EtcYen = txtotheramount.Text.Replace(",", ""),
                InsertOperator = InOperatorCD,            
                ProgramID = InProgramID,
                PC = InPcID
            };
            return dsce;
        }

        private bool ErrorCheck()
        {
             if (!RequireCheck(new Control[] { txt10000 }))
                 return false;               

             if (!RequireCheck(new Control[] { txt5000 }))
                 return false;                      

             if (!RequireCheck(new Control[] { txt2000 }))
                 return false;

            if (!RequireCheck(new Control[] { txt1000 }))
                return false;

            if (!RequireCheck(new Control[] { txt500 }))
                 return false;   

             if (!RequireCheck(new Control[] { txt100 }))
                 return false;           

             if (!RequireCheck(new Control[] { txt50 }))
                 return false;
  
             if (!RequireCheck(new Control[] { txt10 }))
                 return false;

            if (!RequireCheck(new Control[] { txt5 }))
                return false;

            if (!RequireCheck(new Control[] { txt1 }))
                 return false;           

             if (!RequireCheck(new Control[] { txtotheramount }))
                    return false; 

            return true;
        }

        protected override void EndSec()
        {
            this.Close();
        }

    }
}
