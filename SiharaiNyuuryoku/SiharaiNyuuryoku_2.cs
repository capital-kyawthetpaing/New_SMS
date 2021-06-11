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
        M_Kouza_Entity mkze = new M_Kouza_Entity();
        DataTable dtSiharai1 = new DataTable();
        DataTable dtSiharai2 = new DataTable();
        M_Kouza_Entity mke = new M_Kouza_Entity();
        Kouza_BL kbl = new Kouza_BL();

        public DataTable dtGdv = new DataTable();
        public DataTable dtDetails = new DataTable();

        DataTable dtIDName1 = new DataTable();
        DataTable dtIDName2 = new DataTable();
        string type = string.Empty;string kouzaCD = string.Empty;string payeeCD = string.Empty;string payPlanDate = string.Empty;
        public SiharaiNyuuryoku_2(String KouzaCD,String PayeeCD,String PayPlanDate, DataTable dt,DataTable dt1)
        {
            InitializeComponent();
            kouzaCD = KouzaCD;
            payeeCD = PayeeCD;
            payPlanDate = PayPlanDate;
            if(dt!=null)
            {
                dtGdv = dt; 
                DataRow[] tblROWS = dt.Select("PayeeCD = '" + PayeeCD + "'" + "and PayPlanDate = '" + PayPlanDate + "'");
                if (tblROWS.Length > 0)
                    dtSiharai1 = tblROWS.CopyToDataTable();
            }
            
            if (dt1!=null)
            {
                dtDetails = dt1;
                DataRow[] tblROWS1 = dt1.Select("PayeeCD = '" + PayeeCD + "'" + "and PayPlanDate = '" + PayPlanDate + "'");
                if (tblROWS1.Length > 0)
                    dtSiharai2 = tblROWS1.CopyToDataTable();
            }
        }

        private void SiharaiNyuuryoku_2_Load(object sender, EventArgs e)
        {
            #region close
            //F9Visible = false;
            //if (type == "1")
            //{
            //    DataTable dt2 = new DataTable();
            //    dt2 = shnbl.D_Pay_Select2(dpe);
            //    if (dt2.Rows.Count > 0)
            //    {
            //        txtPaymentDueDate.Text = dt2.Rows[0]["PayDate"].ToString();
            //        txtPaymentDestination.Text = dt2.Rows[0]["PayeeCD"].ToString();
            //        lblPaymentDestination.Text = dt2.Rows[0]["VendorName"].ToString();
            //        txtTransferAmount.Text = dt2.Rows[0]["TransferGaku"].ToString();
            //        SC_BankCD.TxtCode.Text = dt2.Rows[0]["BankCD"].ToString();
            //        SC_BankCD.LabelText = dt2.Rows[0]["BankName"].ToString();
            //        SC_BranchCD.TxtCode.Text = dt2.Rows[0]["BranchCD"].ToString();
            //        SC_BranchCD.LabelText = dt2.Rows[0]["BranchName"].ToString();
            //        txtKouzaKBN.Text = dt2.Rows[0]["KouzaKBN"].ToString();
            //        txtAccNo.Text = dt2.Rows[0]["KouzaNO"].ToString();
            //        txtMeigi.Text = dt2.Rows[0]["KouzaMeigi"].ToString();
            //        txtFeeKBN.Text = dt2.Rows[0]["FeeKBN"].ToString();
            //        txtAmount.Text = dt2.Rows[0]["TransferFeeGaku"].ToString();
            //        txtCash.Text = dt2.Rows[0]["CashGaku"].ToString();
            //        txtOffsetGaku.Text = dt2.Rows[0]["OffsetGaku"].ToString();
            //        txtBill.Text = dt2.Rows[0]["BillGaku"].ToString();
            //        txtBillNo.Text = dt2.Rows[0]["BillNO"].ToString();
            //        txtBillDate.Text = dt2.Rows[0]["BillDate"].ToString();
            //        txtElectronicBone.Text = dt2.Rows[0]["ERMCGaku"].ToString();
            //        txtElectronicRecordNo.Text = dt2.Rows[0]["ERMCNO"].ToString();
            //        txtSettlementDate2.Text = dt2.Rows[0]["ERMCDate"].ToString();
            //        txtOther1.Text = dt2.Rows[0]["OtherGaku1"].ToString();
            //        SC_HanyouKeyStart1.TxtCode.Text = dt2.Rows[0]["Account1"].ToString();
            //        SC_HanyouKeyEnd1.TxtCode.Text = dt2.Rows[0]["SubAccount1"].ToString();
            //        txtOther2.Text = dt2.Rows[0]["OtherGaku2"].ToString();
            //        SC_HanyouKeyStart2.TxtCode.Text = dt2.Rows[0]["Account2"].ToString();
            //        SC_HanyouKeyEnd2.TxtCode.Text = dt2.Rows[0]["SubAccount2"].ToString();
            //    }
            //    DataTable dt3 = new DataTable();
            //    dt3 = shnbl.D_Pay_Select3(dpe);
            //    dgvSearchPayment.DataSource = dt3;
            //}
            //else if (type == "2")
            //{
            //    DataTable dt4 = new DataTable();

            //    dt4 = shnbl.D_Pay_SelectForPayPlanDate2(dppe);
            //    if (dt4.Rows.Count > 0)
            //    {
            //        txtPaymentDueDate.Text = dt4.Rows[0]["PayPlanDate"].ToString();
            //        txtPaymentDestination.Text = dt4.Rows[0]["PayeeCD"].ToString();
            //        lblPaymentDestination.Text = dt4.Rows[0]["VendorName"].ToString();
            //        //txtTransferAmount.Text = dt4.Rows[0]["Number"].ToString();
            //        SC_BankCD.TxtCode.Text = dt4.Rows[0]["BankCD"].ToString();
            //        SC_BankCD.LabelText = dt4.Rows[0]["BankName"].ToString();
            //        SC_BranchCD.TxtCode.Text = dt4.Rows[0]["BranchCD"].ToString();
            //        SC_BranchCD.LabelText = dt4.Rows[0]["BranchName"].ToString();
            //        txtKouzaKBN.Text = dt4.Rows[0]["KouzaKBN"].ToString();
            //        txtAccNo.Text = dt4.Rows[0]["KouzaNO"].ToString();
            //        txtMeigi.Text = dt4.Rows[0]["KouzaMeigi"].ToString();
            //        txtFeeKBN.Text = dt4.Rows[0]["FeeKBN"].ToString();
            //        txtAmount.Text = dt4.Rows[0]["Fee"].ToString();
            //        txtCash.Text = dt4.Rows[0]["CashGaku"].ToString();
            //        txtOffsetGaku.Text = dt4.Rows[0]["OffsetGaku"].ToString();
            //        txtBill.Text = dt4.Rows[0]["BillGaku"].ToString();
            //        txtBillNo.Text = dt4.Rows[0]["BillNO"].ToString();
            //        txtBillDate.Text = dt4.Rows[0]["BillDate"].ToString();
            //        txtElectronicBone.Text = dt4.Rows[0]["ERMCGaku"].ToString();
            //        txtElectronicRecordNo.Text = dt4.Rows[0]["ERMCNO"].ToString();
            //        txtSettlementDate2.Text = dt4.Rows[0]["ERMCDate"].ToString();
            //        txtOther1.Text = dt4.Rows[0]["OtherGaku1"].ToString();
            //        SC_HanyouKeyStart1.TxtCode.Text = dt4.Rows[0]["Account1"].ToString();
            //        SC_HanyouKeyEnd1.TxtCode.Text = dt4.Rows[0]["SubAccount1"].ToString();
            //        txtOther2.Text = dt4.Rows[0]["OtherGaku2"].ToString();
            //        SC_HanyouKeyStart2.TxtCode.Text = dt4.Rows[0]["Account2"].ToString();
            //        SC_HanyouKeyEnd2.TxtCode.Text = dt4.Rows[0]["SubAccount2"].ToString();
            //    }
            //}
            #endregion

            //SetFunctionLabel(EProMode.MENTE);

            InProgramID = "SiharaiNyuuryoku_2";
            Btn_F5.Text = "ｷｬﾝｾﾙ(F5)";
            StartProgram();

            BindData();

            LabelDataBind();

            SelectKeyData();

            txtMeigi.Focus();

            //SetRequireField();
        }

        private void BindData()
        {
            txtPayPlanDate.Text = dtSiharai1.Rows[0]["PayPlanDate"].ToString();
            txtPayeeCD.Text = dtSiharai1.Rows[0]["PayeeCD"].ToString();
            lblVendorName.Text = dtSiharai2.Rows[0]["VendorName"].ToString();
            if(dtSiharai1.Rows.Count>=0 || dtSiharai1!=null)
                dgvSearchPayment.DataSource = dtSiharai1;
            //if ()
            //{
            //    //SC_BankCD.TxtCode.Text = dtSiharai1.Rows[0]["BankCD"].ToString();
            //    //SC_BankCD.LabelText = dtSiharai1.Rows[0]["BankName"].ToString();
            //    //SC_BranchCD.TxtCode.Text = dtSiharai1.Rows[0]["BranchCD"].ToString();
            //    //SC_BranchCD.LabelText = dtSiharai1.Rows[0]["BranchName"].ToString();
            //    //txtKouzaKBN.Text = dtSiharai1.Rows[0]["KouzaKBN"].ToString();
            //    //txtAccNo.Text = dtSiharai1.Rows[0]["KouzaNO"].ToString();
            //    //txtMeigi.Text = dtSiharai1.Rows[0]["KouzaMeigi"].ToString();
            //    //txtFeeKBN.Text = dtSiharai1.Rows[0]["FeeKBN"].ToString();
            //    //txtAmount.Text = dtSiharai1.Rows[0]["Fee"].ToString();
            //    //txtCash.Text = dtSiharai1.Rows[0]["CashGaku"].ToString();
            //    //txtOffsetGaku.Text = dtSiharai1.Rows[0]["OffsetGaku"].ToString();
            //    //txtBill.Text = dtSiharai1.Rows[0]["BillGaku"].ToString();
            //    //txtBillNo.Text = dtSiharai1.Rows[0]["BillNO"].ToString();
            //    //txtBillDate.Text = dtSiharai1.Rows[0]["BillDate"].ToString();
            //    //txtElectronicBone.Text = dtSiharai1.Rows[0]["ERMCGaku"].ToString();
            //    //txtElectronicRecordNo.Text = dtSiharai1.Rows[0]["ERMCNO"].ToString();
            //    //txtSettlementDate2.Text = dtSiharai1.Rows[0]["ERMCDate"].ToString();
            //    //txtOther1.Text = dtSiharai1.Rows[0]["OtherGaku1"].ToString();
            //    //SC_HanyouKeyStart1.TxtCode.Text = dtSiharai1.Rows[0]["Account1"].ToString();
            //    //SC_HanyouKeyEnd1.TxtCode.Text = dtSiharai1.Rows[0]["SubAccount1"].ToString();
            //    //txtOther2.Text = dtSiharai1.Rows[0]["OtherGaku2"].ToString();
            //    //SC_HanyouKeyStart2.TxtCode.Text = dtSiharai1.Rows[0]["Account2"].ToString();
            //    //SC_HanyouKeyEnd2.TxtCode.Text = dtSiharai1.Rows[0]["SubAccount2"].ToString();

            //dgvSearchPayment.DataSource = dtSiharai1;
            // }

            if (dtSiharai2.Rows.Count >= 0 || dtSiharai2 != null)
            {
                dgvSearchPayment.DataSource = dtSiharai1;
                txtTransferAmount.Text = dtSiharai2.Rows[0]["TransferGaku"].ToString();
                SC_BankCD.TxtCode.Text = dtSiharai2.Rows[0]["BankCD"].ToString();
                SC_BankCD.LabelText = dtSiharai2.Rows[0]["BankName"].ToString();
                SC_BranchCD.TxtCode.Text = dtSiharai2.Rows[0]["BranchCD"].ToString();
                SC_BranchCD.LabelText = dtSiharai2.Rows[0]["BranchName"].ToString();
                txtKouzaKBN.Text = dtSiharai2.Rows[0]["KouzaKBN"].ToString();
                txtAccNo.Text = dtSiharai2.Rows[0]["KouzaNO"].ToString();
                txtMeigi.Text = dtSiharai2.Rows[0]["KouzaMeigi"].ToString();
                txtFeeKBN.Text = dtSiharai2.Rows[0]["FeeKBN"].ToString();
                SC_KouzaCD.TxtCode.Text= dtSiharai2.Rows[0]["KouzaCD"].ToString();
                SC_KouzaCD.LabelText = dtSiharai2.Rows[0]["KouzaName"].ToString();
                txtAmount.Text = dtSiharai2.Rows[0]["Fee"].ToString();
                txtCash.Text = dtSiharai2.Rows[0]["CashGaku"].ToString();
                txtOffsetGaku.Text = dtSiharai2.Rows[0]["OffsetGaku"].ToString();
                txtBill.Text = dtSiharai2.Rows[0]["BillGaku"].ToString();
                txtBillNo.Text = dtSiharai2.Rows[0]["BillNO"].ToString();
                txtBillDate.Text = dtSiharai2.Rows[0]["BillDate"].ToString();
                txtElectronicBone.Text = dtSiharai2.Rows[0]["ERMCGaku"].ToString();
                txtElectronicRecordNo.Text = dtSiharai2.Rows[0]["ERMCNO"].ToString();
                txtSettlementDate2.Text = dtSiharai2.Rows[0]["ERMCDate"].ToString();
                txtOther1.Text = dtSiharai2.Rows[0]["OtherGaku1"].ToString();
                SC_HanyouKeyStart1.TxtCode.Text = dtSiharai2.Rows[0]["Account1"].ToString();
                SC_HanyouKeyStart1.LabelText = dtSiharai2.Rows[0]["start1"].ToString();
                SC_HanyouKeyEnd1.TxtCode.Text = dtSiharai2.Rows[0]["SubAccount1"].ToString();
                SC_HanyouKeyEnd1.LabelText = dtSiharai2.Rows[0]["end1label"].ToString();
                txtOther2.Text = dtSiharai2.Rows[0]["OtherGaku2"].ToString();
                SC_HanyouKeyStart2.TxtCode.Text = dtSiharai2.Rows[0]["Account2"].ToString();
                SC_HanyouKeyStart2.LabelText = dtSiharai2.Rows[0]["start2"].ToString();
                SC_HanyouKeyEnd2.TxtCode.Text = dtSiharai2.Rows[0]["SubAccount2"].ToString();
                SC_HanyouKeyEnd2.LabelText = dtSiharai2.Rows[0]["end2label"].ToString();

            }
            F9Visible = false;
        }

        private void SelectKeyData()
        {
            dtIDName1=shnbl.M_Multipurpose_SelectIDName("217");
            dtIDName2 = shnbl.M_Multipurpose_SelectIDName("218");
        }

        private void SetRequireField()
        {
            if (Convert.ToInt32(txtTransferAmount.Text)>0)
            {
                SC_BankCD.TxtCode.Require(true);
                SC_BranchCD.TxtCode.Require(true);
                txtKouzaKBN.Require(true);
                txtAccNo.Require(true);
                txtMeigi.Require(true);
                txtFeeKBN.Require(true);
                txtAmount.Require(true);

            }

            if(Convert.ToInt32(txtBill.Text)>0)
            {
                txtBillNo.Require(true);
                txtBillDate.Require(true);

            }
            if(Convert.ToInt32(txtElectronicBone.Text)>0)
            {
                txtElectronicRecordNo.Require(true);
                txtSettlementDate2.Require(true);
            }
            if(Convert.ToInt32(txtOther1.Text)>=0)
            {
                SC_HanyouKeyStart1.TxtCode.Require(true);
                SC_HanyouKeyEnd1.TxtCode.Require(true);
            }
            if (Convert.ToInt32(txtOther2.Text)>0)
            {
                SC_HanyouKeyStart2.TxtCode.Require(true);
                SC_HanyouKeyEnd2.TxtCode.Require(true);
            }
        }

        public void LabelDataBind()
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
            lblPayGaku.Text =lblPayGaku1.Text= sum3.ToString("#,##0");
            lblUnpaidAmount.Text = sum4.ToString("#,##0");
            txtTransferAmount.Text = sum3.ToString();
        }


        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int Index)
        {
            if (Index + 1 == 5)
                Clear();
            if (Index + 1 == 12)
            {
                  if (ErrorCheck())
                  if (bbl.ShowMessage("Q106") == DialogResult.Yes)//error message added by ses 2021/06/11
                  {
                     SendData();
                  }
                else
                {
                   PreviousCtrl.Focus();
                }
            }
        }


        public void SendData()
        {
            if(dtGdv.Rows.Count>0)
            {
                DataRow[] tblROWS = dtGdv.Select("PayeeCD = '" + payeeCD + "'" + "and PayPlanDate = '" + payPlanDate + "'");
                foreach(DataRow row in tblROWS)
                {
                    dtGdv.Rows.Remove(row);
                    dtGdv.AcceptChanges();
                }
                dtGdv.Merge(dtSiharai1);
                    
            }
            if (dtDetails.Rows.Count > 0)
            {
                /*dtSiharai2=*/
                SetData();
                DataRow[] tblROWS = dtDetails.Select("PayeeCD = '" + payeeCD + "'" + "and PayPlanDate = '" + payPlanDate + "'");
                foreach (DataRow row in tblROWS)
                {
                    dtDetails.Rows.Remove(row);
                    dtDetails.AcceptChanges();
                }
                dtDetails.Merge(dtSiharai2);               
            }
            this.Close();
        }

        private void SetData()
        {
            dtSiharai2.Rows[0]["TransferGaku"] = txtTransferAmount.Text.ToString() ;
            dtSiharai2.Rows[0]["BankCD"] = SC_BankCD.TxtCode.Text.ToString();
            dtSiharai2.Rows[0]["BankName"] = SC_BankCD.LabelText.ToString();
            dtSiharai2.Rows[0]["BranchCD"] = SC_BranchCD.TxtCode.Text.ToString();
            dtSiharai2.Rows[0]["BranchName"] = SC_BranchCD.LabelText.ToString();
            dtSiharai2.Rows[0]["KouzaKBN"] = txtKouzaKBN.Text.ToString();
            dtSiharai2.Rows[0]["KouzaNO"] = txtAccNo.Text.ToString();
            dtSiharai2.Rows[0]["KouzaMeigi"] = txtMeigi.Text.ToString();
            dtSiharai2.Rows[0]["FeeKBN"] = txtFeeKBN.Text.ToString();
            dtSiharai2.Rows[0]["Fee"] = txtAmount.Text.ToString();
            dtSiharai2.Rows[0]["KouzaCD"] = SC_KouzaCD.TxtCode.Text.ToString();
            dtSiharai2.Rows[0]["KouzaName"] = SC_KouzaCD.LabelText.ToString();
            dtSiharai2.Rows[0]["CashGaku"] = txtCash.Text.ToString();
            dtSiharai2.Rows[0]["OffsetGaku"] = txtOffsetGaku.Text.ToString();
            dtSiharai2.Rows[0]["BillGaku"] = txtBill.Text.ToString();
            dtSiharai2.Rows[0]["BillNO"] = txtBillNo.Text.ToString();
            dtSiharai2.Rows[0]["BillDate"] = txtBillDate.Text.Replace('/','-').ToString();

            dtSiharai2.Rows[0]["ERMCGaku"] = txtElectronicBone.Text.ToString();
            dtSiharai2.Rows[0]["ERMCNO"] = txtElectronicRecordNo.Text.ToString();
            dtSiharai2.Rows[0]["ERMCDate"] = txtSettlementDate2.Text.ToString();

            dtSiharai2.Rows[0]["OtherGaku1"] = txtOther1.Text.ToString();
            dtSiharai2.Rows[0]["Account1"] = SC_HanyouKeyStart1.TxtCode.Text.ToString();
            //dtSiharai2.Rows[0]["start1"] = SC_HanyouKeyStart1.LabelText;
            dtSiharai2.Rows[0]["SubAccount1"] = SC_HanyouKeyEnd1.TxtCode.Text.ToString();
            //dtSiharai2.Rows[0]["end1label"] = SC_HanyouKeyEnd1.LabelText;

            dtSiharai2.Rows[0]["OtherGaku2"] = txtOther2.Text.ToString();
            dtSiharai2.Rows[0]["Account2"] = SC_HanyouKeyStart2.TxtCode.Text.ToString();
            //dtSiharai2.Rows[0]["start2"] = SC_HanyouKeyStart2.LabelText;
            dtSiharai2.Rows[0]["SubAccount2"] = SC_HanyouKeyEnd2.TxtCode.Text.ToString();
            //dtSiharai2.Rows[0]["end2label"] = SC_HanyouKeyEnd2.LabelText;
            dtSiharai2.AcceptChanges();
            //return dtSiharai2;
        }
        public void Clear()
        {
            txtPayPlanDate.Text = string.Empty;
            txtPayeeCD.Text = string.Empty;
            lblVendorName.Text = string.Empty;
            lblPayGaku1.Text = string.Empty;
            txtTransferAmount.Text = string.Empty;
            SC_BankCD.Clear();
            SC_BranchCD.Clear();
            txtKouzaKBN.Text = string.Empty;
            txtAccNo.Text = string.Empty;
            txtMeigi.Text = string.Empty;
            txtFeeKBN.Text = string.Empty;
            SC_KouzaCD.Clear();
            txtAmount.Text = string.Empty;
            txtCash.Text = string.Empty;
            txtOffsetGaku.Text = string.Empty;
            txtBill.Text = string.Empty;
            txtBillNo.Text = string.Empty;
            txtBillDate.Text = string.Empty;
            txtElectronicBone.Text = string.Empty;
            txtElectronicRecordNo.Text = string.Empty;
            txtSettlementDate2.Text = string.Empty;
            txtOther1.Text = string.Empty;
            SC_HanyouKeyStart1.Clear();
            SC_HanyouKeyStart2.Clear();
            txtOther2.Text = string.Empty;
            SC_HanyouKeyEnd1.Clear();
            SC_HanyouKeyEnd2.Clear();
            //ses added 2021/06/11
            dgvSearchPayment.ClearSelection();
            dgvSearchPayment.DataSource = null;
            lblPayPlanGaku.Text = string.Empty;
            lblPayGaku1.Text = string.Empty;
            lblPayGaku.Text = string.Empty;
            lblPayComfirmGaku.Text = string.Empty;
        }

        /// <summary>
        /// Error Check of F12
        /// </summary>
        /// <returns></returns>
        public bool ErrorCheck()
        {
            if (Convert.ToInt64(txtTransferAmount.Text) != 0)//> to != 2021/06/10 SES changed
            {
                if (!RequireCheck(new Control[] { SC_BankCD.TxtCode }))
                    return false;

                SC_BankCD.ChangeDate = DateTime.Today.ToShortDateString();
                if (!SC_BankCD.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_BankCD.SetFocus(1);
                    return false;
                }

                if (!RequireCheck(new Control[] { SC_BranchCD.TxtCode }))
                    return false;

                SC_BranchCD.ChangeDate = DateTime.Today.ToShortDateString();
                SC_BranchCD.Value1 = SC_BankCD.TxtCode.Text;
                if (!SC_BranchCD.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_BranchCD.SetFocus(1);
                    return false;
                }

                if (!RequireCheck(new Control[] { txtKouzaKBN }))
                    return false;
                if (txtKouzaKBN.Text != "1")
                {
                    if (txtKouzaKBN.Text != "2")
                    {
                        bbl.ShowMessage("E101");
                        txtKouzaKBN.Focus();
                        return false;
                    }

                }

                if (!RequireCheck(new Control[] { txtAccNo, txtMeigi, txtFeeKBN }))
                    return false;
                if (!txtFeeKBN.Text.Equals("1"))
                {
                    if (!txtFeeKBN.Text.Equals("2"))
                    {
                        bbl.ShowMessage("E101");
                        txtFeeKBN.Focus();
                        return false;
                    }
                }
                //ses
                if (!RequireCheck(new Control[] { SC_KouzaCD.TxtCode }))
                    return false;

                mke.ChangeDate = DateTime.Today.ToShortDateString();
                mke.KouzaCD = SC_KouzaCD.TxtCode.Text;
                DataTable dtKouza = new DataTable();
                dtKouza = kbl.M_Kouza_Select(mke);
                if (dtKouza.Rows.Count == 0)
                {
                    bbl.ShowMessage("E128");
                    SC_KouzaCD.SetFocus(1);
                }
                else
                {
                    SC_KouzaCD.LabelText = dtKouza.Rows[0]["KouzaName"].ToString();
                }
                //ses
                if (!RequireCheck(new Control[] { txtAmount }))
                    return false;
            }

            if (Convert.ToInt32(txtBill.Text) != 0)//> to != 2021/06/10 SES changed
            {
                if (!RequireCheck(new Control[] { txtBillNo,txtBillDate}))
                    return false;
            }

            if (Convert.ToInt32(txtElectronicBone.Text) != 0)//> to != 2021/06/10 SES changed
            {
                if (!RequireCheck(new Control[] { txtElectronicRecordNo,txtSettlementDate2}))
                    return false;
            }

            if (Convert.ToInt32(txtOther1.Text) != 0)//> to != 2021/06/10 SES changed
            {
                if (!RequireCheck(new Control[] { SC_HanyouKeyStart1.TxtCode }))
                    return false;

                if (!SC_HanyouKeyStart1.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_HanyouKeyStart1.SetFocus(1);
                    return false;
                }

                if (!RequireCheck(new Control[] { SC_HanyouKeyEnd1.TxtCode }))
                    return false;

                if (!SC_HanyouKeyEnd1.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_HanyouKeyEnd1.SetFocus(1);
                    return false;
                }
            }

            if (Convert.ToInt32(txtOther2.Text) != 0)//> to != 2021/06/10 SES changed
            {
                if (!RequireCheck(new Control[] { SC_HanyouKeyStart2.TxtCode }))
                    return false;
                if (!SC_HanyouKeyStart2.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_HanyouKeyStart2.SetFocus(1);
                    return false;
                }

                if (!RequireCheck(new Control[] { SC_HanyouKeyEnd2.TxtCode }))
                    return false;
                if (!SC_HanyouKeyEnd2.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_HanyouKeyEnd2.SetFocus(1);
                    return false;
                }
            }
            //E195 error check added by ses 2021/06/11
            if (Convert.ToInt64(txtTransferAmount.Text) != Convert.ToInt64(txtCash.Text + txtOffsetGaku.Text+ txtBill.Text + txtElectronicBone.Text + txtOther1.Text + txtOther2.Text))
            {
                bbl.ShowMessage("E195");
                txtTransferAmount.Focus();
            }
            return true;
        }
       
        #region Enter event of Search Control
        private void SC_HanyouKeyStart1_Enter(object sender, EventArgs e)
        {
            SC_HanyouKeyStart1.Value1 = dtIDName1.Rows[0]["ID"].ToString();
            SC_HanyouKeyStart1.Value2 = dtIDName1.Rows[0]["IDName"].ToString();
        }

        private void SC_HanyouKeyEnd1_Enter(object sender, EventArgs e)
        {
            SC_HanyouKeyEnd1.Value1 = dtIDName2.Rows[0]["ID"].ToString();
            SC_HanyouKeyEnd1.Value2 = dtIDName2.Rows[0]["IDName"].ToString();
            SC_HanyouKeyEnd1.Value3 = SC_HanyouKeyStart1.TxtCode.Text;
        }

        private void SC_HanyouKeyStart2_Enter(object sender, EventArgs e)
        {
            SC_HanyouKeyStart2.Value1 = dtIDName1.Rows[0]["ID"].ToString();
            SC_HanyouKeyStart2.Value2 = dtIDName1.Rows[0]["IDName"].ToString();
        }

        private void SC_HanyouKeyEnd2_Enter(object sender, EventArgs e)
        {
            SC_HanyouKeyEnd2.Value1 = dtIDName2.Rows[0]["ID"].ToString();
            SC_HanyouKeyEnd2.Value2 = dtIDName2.Rows[0]["IDName"].ToString();
            SC_HanyouKeyEnd2.Value3 = SC_HanyouKeyStart2.TxtCode.Text;
        }
        #endregion


        /// <summary>
        /// Leave event of Bank Search Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SC_BankCD_Leave(object sender, EventArgs e)
        {
            SC_BranchCD.Value1 = SC_BankCD.TxtCode.Text;
            SC_BranchCD.Value2 = SC_BankCD.LabelText;
        }


        #region  KeyDown event

        /// <summary>
        /// Fee calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SC_KeyDown(object sender, KeyEventArgs e)
        {
            Select_KouzaFee();
        }

        private void SC_BankCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Convert.ToInt32(txtTransferAmount.Text)>0 && !string.IsNullOrWhiteSpace(SC_BankCD.TxtCode.Text))
                {
                    SC_BankCD.ChangeDate = DateTime.Today.ToShortDateString();
                    if (SC_BankCD.SelectData())
                    {
                        SC_BranchCD.Value1 = SC_BankCD.TxtCode.Text;
                        SC_BranchCD.Value2 = SC_BankCD.LabelText;

                        Select_KouzaFee();

                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        SC_BankCD.SetFocus(1);
                    }

                }
            }
        }
       
        private void SC_BranchCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               
                if (!string.IsNullOrWhiteSpace(SC_BranchCD.TxtCode.Text))
                {
                    SC_BranchCD.ChangeDate = DateTime.Today.ToShortDateString();
                    SC_BranchCD.Value1 = SC_BankCD.TxtCode.Text;
                    if (!SC_BranchCD.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_BranchCD.SetFocus(1);
                    }
                    else
                    {
                        Select_KouzaFee();
                    }

                }
                else
                {
                    bbl.ShowMessage("E101");
                    SC_BranchCD.SetFocus(1);
                }

            }
        }
        private void SC_KouzaCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                mke.ChangeDate = DateTime.Today.ToShortDateString();
                mke.KouzaCD = SC_KouzaCD.TxtCode.Text;
                Kouza_BL kbl = new Kouza_BL();
                DataTable dtKouza = new DataTable();
                dtKouza = kbl.M_Kouza_Select(mke);
                if (dtKouza.Rows.Count == 0)
                {
                    bbl.ShowMessage("E128");
                    SC_KouzaCD.SetFocus(1);
                }
                else
                {
                    SC_KouzaCD.LabelText = dtKouza.Rows[0]["KouzaName"].ToString();
                }
            }
        }

        private void SC_HanyouKeyStart1_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(SC_HanyouKeyStart1.TxtCode.Text))
                {
                    if (!SC_HanyouKeyStart1.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_HanyouKeyStart1.SetFocus(1);
                    }
                    else
                    {
                        SC_HanyouKeyStart1.Value1 = dtIDName1.Rows[0]["ID"].ToString();
                        SC_HanyouKeyStart1.Value2 = dtIDName1.Rows[0]["IDName"].ToString();
                    }

                }
            }
        }

        private void SC_HanyouKeyStart2_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(SC_HanyouKeyStart2.TxtCode.Text))
                {
                    if (!SC_HanyouKeyStart2.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_HanyouKeyStart2.SetFocus(1);
                    }
                    else
                    {
                        SC_HanyouKeyStart2.Value1 = dtIDName1.Rows[0]["ID"].ToString();
                        SC_HanyouKeyStart2.Value2 = dtIDName1.Rows[0]["IDName"].ToString();
                    }

                }
            }

        }

        private void SC_HanyouKeyEnd1_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(SC_HanyouKeyEnd1.TxtCode.Text))
                {
                    SC_HanyouKeyEnd1.Value2 = SC_HanyouKeyStart1.TxtCode.Text;
                    if (!SC_HanyouKeyEnd1.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_HanyouKeyEnd1.SetFocus(1);
                    }
                    else
                    {
                        SC_HanyouKeyEnd1.Value1 = dtIDName2.Rows[0]["ID"].ToString();
                        SC_HanyouKeyEnd1.Value2 = dtIDName2.Rows[0]["IDName"].ToString();
                        SC_HanyouKeyEnd1.Value3 = SC_HanyouKeyStart1.TxtCode.Text;
                    }

                }
            }
        }

        private void SC_HanyouKeyEnd2_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(SC_HanyouKeyEnd2.TxtCode.Text))
                {
                    SC_HanyouKeyEnd2.Value2 = SC_HanyouKeyStart2.TxtCode.Text;
                    if (!SC_HanyouKeyEnd2.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        SC_HanyouKeyEnd2.SetFocus(1);
                    }
                    else
                    {
                        SC_HanyouKeyEnd2.Value1 = dtIDName2.Rows[0]["ID"].ToString();
                        SC_HanyouKeyEnd2.Value2 = dtIDName2.Rows[0]["IDName"].ToString();
                        SC_HanyouKeyEnd2.Value3 = SC_HanyouKeyStart2.TxtCode.Text;
                    }

                }
            }
        }

        #endregion

        public void Select_KouzaFee()
        {
            if (!string.IsNullOrWhiteSpace(SC_BankCD.TxtCode.Text) && !string.IsNullOrWhiteSpace(SC_BranchCD.TxtCode.Text)
                             && !string.IsNullOrWhiteSpace(txtFeeKBN.Text) && txtAmount.Text.Equals("0"))
            {
                mkze = new M_Kouza_Entity
                {
                    KouzaCD = kouzaCD,
                    BankCD = SC_BankCD.TxtCode.Text,
                    BranchCD = SC_BranchCD.TxtCode.Text,
                    Amount = lblPayGaku.Text.Replace(",", ""),

                };
                DataTable dt=shnbl.M_Kouza_FeeSelect(mkze);
                txtTransferAmount.Text = dt.Rows[0]["Fee"].ToString();
            }
            else
            {
                bbl.ShowMessage("E102");
                txtTransferAmount.Focus();
            }

        }


        protected override void EndSec()
        {
            this.Close();
        }

        private void SiharaiNyuuryoku_2_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void dgvSearchPayment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Maintained_CheckClick(sender, e);
        }

        protected void Maintained_CheckClick(object sender, DataGridViewCellEventArgs e)
       {
            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                if ((sender as DataGridView).CurrentCell is DataGridViewCheckBoxCell)
                {
                    if ((Convert.ToBoolean(dgvSearchPayment.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue) == true))
                    {
                        DataGridViewCheckBoxCell chk1 = dgvSearchPayment.Rows[e.RowIndex].Cells["colChk"] as DataGridViewCheckBoxCell;
                        dgvSearchPayment.Rows[e.RowIndex].Cells["colUnpaidAmount1"].Value = Convert.ToInt32(dgvSearchPayment.Rows[e.RowIndex].Cells["colPayPlanGaku"].Value.ToString()) - Convert.ToInt32(dgvSearchPayment.Rows[e.RowIndex].Cells["colPayConfirmGaku"].Value.ToString());
                        dgvSearchPayment.Rows[e.RowIndex].Cells["colPayConfirmGaku"].Value = "0";
                    }
                    else
                    {
                        DataGridViewCheckBoxCell chk1 = dgvSearchPayment.Rows[e.RowIndex].Cells["colChk"] as DataGridViewCheckBoxCell;
                        dgvSearchPayment.Rows[e.RowIndex].Cells["colUnpaidAmount1"].Value = "0";
                        dgvSearchPayment.Rows[e.RowIndex].Cells["colPayConfirmGaku"].Value = Convert.ToInt32(dgvSearchPayment.Rows[e.RowIndex].Cells["colPayPlanGaku"].Value.ToString()) - Convert.ToInt32(dgvSearchPayment.Rows[e.RowIndex].Cells["colPayConfirmGaku"].Value.ToString());
                    }

                    LabelDataBind();
                }

            }
        }


        private void dgvSearchPayment_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //if(dgvSearchPayment.CurrentRow.Index>-1)
            //{
            //    if (dgvSearchPayment.CurrentCell == dgvSearchPayment.CurrentRow.Cells["colUnpaidAmount1"])
            //    {
            //        DataGridViewRow row = dgvSearchPayment.CurrentRow;
            //        string unpaidAmount1 = row.Cells["colUnpaidAmount1"].Value.ToString();
            //        if (string.IsNullOrWhiteSpace(unpaidAmount1))
            //        {
            //            bbl.ShowMessage("E102");
            //        }
            //    }
           // }
            
        }

        private void dgvSearchPayment_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSearchPayment.CurrentRow.Index > -1)
            {
                if (dgvSearchPayment.CurrentCell == dgvSearchPayment.CurrentRow.Cells["colUnpaidAmount1"])
                {
                    DataGridViewRow row = dgvSearchPayment.CurrentRow;

                    if (string.IsNullOrWhiteSpace(row.Cells["colUnpaidAmount1"].Value.ToString()))
                    {
                        bbl.ShowMessage("E102");
                        dgvSearchPayment.CurrentCell = dgvSearchPayment.CurrentRow.Cells["colPayConfirmGaku"];
                    }
                    else if (Convert.ToInt32(row.Cells["colUnpaidAmount1"].Value.ToString()) > Convert.ToInt32(row.Cells["colUnpaidAmount2"].Value.ToString()) || Convert.ToInt32(row.Cells["colUnpaidAmount1"].Value.ToString()) < 0)
                    //else if(row.Cells["colUnpaidAmount1"].Value > row.Cells["col"])
                    {
                        bbl.ShowMessage("E143");
                        dgvSearchPayment.CurrentCell = dgvSearchPayment.CurrentRow.Cells["colPayConfirmGaku"];
                    }
                    else
                    {
                        row.Cells["colUnpaidAmount2"].Value = Convert.ToInt32(row.Cells["colPayPlanGaku"].Value.ToString()) - Convert.ToInt32(row.Cells["colPayConfirmGaku"].Value.ToString()) - Convert.ToInt32(row.Cells["colUnpaidAmount1"].Value.ToString());
                    }
                        LabelDataBind();
                }
            }
        }

        
    }
}
