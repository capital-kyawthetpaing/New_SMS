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

namespace SiharaiNyuuryoku
{
    public partial class SiharaiNyuuryoku_2 : FrmMainForm
    {
        Search_Payment_BL spbl = new Search_Payment_BL();
        SiharaiNyuuryoku_BL shnbl = new SiharaiNyuuryoku_BL();
        D_Pay_Entity dpe = new D_Pay_Entity();
        D_PayPlan_Entity dppe = new D_PayPlan_Entity();
        string type = string.Empty;
        public SiharaiNyuuryoku_2(String PayeeCD,String PayPlanDate, DataTable dt,DataTable dt1=null)
        {
            InitializeComponent();

            //dpe.LargePayNO = LargePayNO;
            //dpe.PayNo = PayNo;
            //dpe.PayeeCD = PayeeCD;
            //dpe.PayPlanDate = PayPlanDate;

            //dppe.PayPlanDate = PayPlanDate;
            //dppe.PayeeCD = PayeeCD;
            //dppe.PaymentTotal =
            //type = no;

        }

        private void SiharaiNyuuryoku_2_Load(object sender, EventArgs e)
        {
            F9Visible = false;
            if (type == "1")
            {
                DataTable dt2 = new DataTable();
                dt2 = shnbl.D_Pay_Select2(dpe);
                if (dt2.Rows.Count > 0)
                {
                    txtPaymentDueDate.Text = dt2.Rows[0]["PayDate"].ToString();
                    txtPaymentDestination.Text = dt2.Rows[0]["PayeeCD"].ToString();
                    lblPaymentDestination.Text = dt2.Rows[0]["VendorName"].ToString();
                    txtTransferAmount.Text = dt2.Rows[0]["TransferGaku"].ToString();
                    SC_BankCD.TxtCode.Text = dt2.Rows[0]["BankCD"].ToString();
                    SC_BankCD.LabelText = dt2.Rows[0]["BankName"].ToString();
                    SC_BranchCD.TxtCode.Text = dt2.Rows[0]["BranchCD"].ToString();
                    SC_BranchCD.LabelText = dt2.Rows[0]["BranchName"].ToString();
                    txtKouzaKBN.Text = dt2.Rows[0]["KouzaKBN"].ToString();
                    txtAccNo.Text = dt2.Rows[0]["KouzaNO"].ToString();
                    txtMeigi.Text = dt2.Rows[0]["KouzaMeigi"].ToString();
                    txtFeeKBN.Text = dt2.Rows[0]["FeeKBN"].ToString();
                    txtAmount.Text = dt2.Rows[0]["TransferFeeGaku"].ToString();
                    txtCash.Text = dt2.Rows[0]["CashGaku"].ToString();
                    txtOffsetGaku.Text = dt2.Rows[0]["OffsetGaku"].ToString();
                    txtBill.Text = dt2.Rows[0]["BillGaku"].ToString();
                    txtBillNo.Text = dt2.Rows[0]["BillNO"].ToString();
                    txtBillDate.Text = dt2.Rows[0]["BillDate"].ToString();
                    txtElectronicBone.Text = dt2.Rows[0]["ERMCGaku"].ToString();
                    txtElectronicRecordNo.Text = dt2.Rows[0]["ERMCNO"].ToString();
                    txtSettlementDate2.Text = dt2.Rows[0]["ERMCDate"].ToString();
                    txtOther1.Text = dt2.Rows[0]["OtherGaku1"].ToString();
                    SC_HanyouKeyStart1.TxtCode.Text = dt2.Rows[0]["Account1"].ToString();
                    SC_HanyouKeyEnd1.TxtCode.Text = dt2.Rows[0]["SubAccount1"].ToString();
                    txtOther2.Text = dt2.Rows[0]["OtherGaku2"].ToString();
                    SC_HanyouKeyStart2.TxtCode.Text = dt2.Rows[0]["Account2"].ToString();
                    SC_HanyouKeyEnd2.TxtCode.Text = dt2.Rows[0]["SubAccount2"].ToString();
                }
                DataTable dt3 = new DataTable();
                dt3 = shnbl.D_Pay_Select3(dpe);
                dgvSearchPayment.DataSource = dt3;
            }
            else if (type == "2")
            {
                DataTable dt4 = new DataTable();

                dt4 = shnbl.D_Pay_SelectForPayPlanDate2(dppe);
                if (dt4.Rows.Count > 0)
                {
                    txtPaymentDueDate.Text = dt4.Rows[0]["PayPlanDate"].ToString();
                    txtPaymentDestination.Text = dt4.Rows[0]["PayeeCD"].ToString();
                    lblPaymentDestination.Text = dt4.Rows[0]["VendorName"].ToString();
                    //txtTransferAmount.Text = dt4.Rows[0]["Number"].ToString();
                    SC_BankCD.TxtCode.Text = dt4.Rows[0]["BankCD"].ToString();
                    SC_BankCD.LabelText = dt4.Rows[0]["BankName"].ToString();
                    SC_BranchCD.TxtCode.Text = dt4.Rows[0]["BranchCD"].ToString();
                    SC_BranchCD.LabelText = dt4.Rows[0]["BranchName"].ToString();
                    txtKouzaKBN.Text = dt4.Rows[0]["KouzaKBN"].ToString();
                    txtAccNo.Text = dt4.Rows[0]["KouzaNO"].ToString();
                    txtMeigi.Text = dt4.Rows[0]["KouzaMeigi"].ToString();
                    txtFeeKBN.Text = dt4.Rows[0]["FeeKBN"].ToString();
                    txtAmount.Text = dt4.Rows[0]["Fee"].ToString();
                    txtCash.Text = dt4.Rows[0]["CashGaku"].ToString();
                    txtOffsetGaku.Text = dt4.Rows[0]["OffsetGaku"].ToString();
                    txtBill.Text = dt4.Rows[0]["BillGaku"].ToString();
                    txtBillNo.Text = dt4.Rows[0]["BillNO"].ToString();
                    txtBillDate.Text = dt4.Rows[0]["BillDate"].ToString();
                    txtElectronicBone.Text = dt4.Rows[0]["ERMCGaku"].ToString();
                    txtElectronicRecordNo.Text = dt4.Rows[0]["ERMCNO"].ToString();
                    txtSettlementDate2.Text = dt4.Rows[0]["ERMCDate"].ToString();
                    txtOther1.Text = dt4.Rows[0]["OtherGaku1"].ToString();
                    SC_HanyouKeyStart1.TxtCode.Text = dt4.Rows[0]["Account1"].ToString();
                    SC_HanyouKeyEnd1.TxtCode.Text = dt4.Rows[0]["SubAccount1"].ToString();
                    txtOther2.Text = dt4.Rows[0]["OtherGaku2"].ToString();
                    SC_HanyouKeyStart2.TxtCode.Text = dt4.Rows[0]["Account2"].ToString();
                    SC_HanyouKeyEnd2.TxtCode.Text = dt4.Rows[0]["SubAccount2"].ToString();
                }
            }

            LabelDataBind();
        }

        private void SetRequireField()
        {
            if(Convert.ToInt32(txtTransferAmount.Text)>0)
                SC_BankCD.TxtCode.Require(true);
        }

        private void LabelDataBind()
        {
            int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0;
            for (int i = 0; i < dgvSearchPayment.Rows.Count; ++i)
            {
                sum1 += Convert.ToInt32(dgvSearchPayment.Rows[i].Cells[3].Value);
                sum2 += Convert.ToInt32(dgvSearchPayment.Rows[i].Cells[4].Value);
                sum3 += Convert.ToInt32(dgvSearchPayment.Rows[i].Cells[5].Value);
                sum4 += Convert.ToInt32(dgvSearchPayment.Rows[i].Cells[6].Value);
            }
            lblPayPlanGaku.Text = sum1.ToString("#,##0");
            lblPayComfirmGaku.Text = sum2.ToString("#,##0");
            lblPayGaku.Text = sum3.ToString("#,##0");
            lblUnpaidAmount.Text = sum4.ToString("#,##0");
            //lblPayGaku1.Text = lblPayGaku.Text ;
        }

        private void SC_HanyouKeyStart1_Enter(object sender, EventArgs e)
        {
            SC_HanyouKeyStart1.Value1 = "217";
            SC_HanyouKeyStart1.Value2 = "IDName";
        }

        private void SC_HanyouKeyStart2_Load(object sender, EventArgs e)
        {
            SC_HanyouKeyStart2.Value1 = "217";
            SC_HanyouKeyStart2.Value2 = "IDName";
        }

        private void SC_HanyouKeyEnd1_Load(object sender, EventArgs e)
        {
            SC_HanyouKeyEnd1.Value1 = "218";
            SC_HanyouKeyEnd1.Value2 = "IDName";
        }

        private void SC_HanyouKeyEnd2_Load(object sender, EventArgs e)
        {
            SC_HanyouKeyEnd2.Value1 = "218";
            SC_HanyouKeyEnd2.Value2 = "IDName";
        }

       public bool ErrorCheck()
        {
            if (Convert.ToInt32(txtTransferAmount.Text) > 0)
            {
                if (!RequireCheck(new Control[] { SC_BankCD.TxtCode}))
                    return false;
            }
                return true;
        }
    }
}
