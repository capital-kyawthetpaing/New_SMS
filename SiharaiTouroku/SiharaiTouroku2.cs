﻿using System;
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

namespace SiharaiTouroku
{
    public partial class SiharaiTouroku_2 : FrmMainForm
    {
        private const string ProID = "SiharaiTouroku";

        public bool flgCancel = false;

        Search_Payment_BL spbl = new Search_Payment_BL();
        SiharaiTouroku_BL shnbl = new SiharaiTouroku_BL();
        D_Pay_Entity dpe = new D_Pay_Entity();
        //D_PayPlan_Entity dppe = new D_PayPlan_Entity();
        //M_Kouza_Entity mkze = new M_Kouza_Entity();
        //DataTable dtSiharai1 = new DataTable();
        DataTable dtSiharai2 = new DataTable();

        public DataTable dtGdv = new DataTable();
        public DataTable dtDetails = new DataTable();

        DataTable dtIDName1 = new DataTable();
        DataTable dtIDName2 = new DataTable();
        string type = string.Empty;
        string kouzaCD = string.Empty;
        string payeeCD = string.Empty;
        string payPlanDate = string.Empty;
        public SiharaiTouroku_2(D_Pay_Entity dpe1, DataTable dt, DataTable dtDetail)
        {
            InitializeComponent();
            dpe = dpe1;

            kouzaCD = dpe.MotoKouzaCD;
            payeeCD = dpe.PayeeCD;
            payPlanDate = dpe.PayPlanDate;

            dtGdv = dt;

            dtDetails = dtDetail;
            DataRow[] tblROWS1 = dtDetail.Select("PayeeCD = '" + payeeCD + "'" + "and PayPlanDate = '" + payPlanDate + "'");
            if (tblROWS1.Length > 0)
                dtSiharai2 = tblROWS1.CopyToDataTable();
        }

        private void SiharaiTouroku_2_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;

                Btn_F1.Text = "戻る(F1)";
                Btn_F2.Text = "";
                Btn_F3.Text = "";
                Btn_F4.Text = "";
                Btn_F5.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F9.Text = "";
                Btn_F10.Text = "";
                Btn_F11.Text = "";
                Btn_F12.Text = "登録(F12)";

                //起動時共通処理
                base.StartProgram();

                BindData();

                LabelDataBind();

                SelectKeyData();

                txtTransferAmount.Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        private void BindData()
        {
            txtPayPlanDate.Text = dpe.PayPlanDate;
            txtPayeeCD.Text = dpe.PayeeCD;
            lblVendorName.Text = dpe.PayeeName;
            dgvSearchPayment.DataSource = dtSiharai2;

            DataRow[] tblROWS = dtGdv.Select("PayeeCD = '" + payeeCD + "'" + "and PayPlanDate = '" + payPlanDate + "'");
            if (tblROWS.Length > 0)
            //dtSiharai1 = tblROWS.CopyToDataTable();
            {
                txtTransferAmount.Text = bbl.Z_SetStr(tblROWS[0]["TransferGaku"]);
                SC_BankCD.TxtCode.Text = tblROWS[0]["BankCD"].ToString();
                SC_BankCD.LabelText = tblROWS[0]["BankName"].ToString();
                SC_BranchCD.TxtCode.Text = tblROWS[0]["BranchCD"].ToString();
                SC_BranchCD.LabelText = tblROWS[0]["BranchName"].ToString();
                txtKouzaKBN.Text = tblROWS[0]["KouzaKBN"].ToString();
                txtAccNo.Text = tblROWS[0]["KouzaNO"].ToString();
                txtMeigi.Text = tblROWS[0]["KouzaMeigi"].ToString();
                txtFeeKBN.Text = tblROWS[0]["FeeKBNVal"].ToString();
                //txtAmount.Text = tblROWS[0]["Fee"].ToString();
                txtAmount.Text = bbl.Z_SetStr(tblROWS[0]["TransferFeeGaku"]);
                txtCash.Text = bbl.Z_SetStr(tblROWS[0]["CashGaku"]);
                txtOffsetGaku.Text = bbl.Z_SetStr(tblROWS[0]["OffsetGaku"]);
                txtBill.Text = bbl.Z_SetStr(tblROWS[0]["BillGaku"]);
                txtBillNo.Text = tblROWS[0]["BillNO"].ToString();
                txtBillDate.Text = tblROWS[0]["BillDate"].ToString();
                txtElectronicBone.Text = tblROWS[0]["ERMCGaku"].ToString();
                txtElectronicRecordNo.Text = tblROWS[0]["ERMCNO"].ToString();
                txtSettlementDate2.Text = tblROWS[0]["ERMCDate"].ToString();
                txtOther1.Text = tblROWS[0]["OtherGaku1"].ToString();
                SC_HanyouKeyStart1.TxtCode.Text = tblROWS[0]["Account1"].ToString();
                SC_HanyouKeyStart1.LabelText = tblROWS[0]["start1"].ToString();
                SC_HanyouKeyEnd1.TxtCode.Text = tblROWS[0]["SubAccount1"].ToString();
                SC_HanyouKeyEnd1.LabelText = tblROWS[0]["end1label"].ToString();
                txtOther2.Text = tblROWS[0]["OtherGaku2"].ToString();
                SC_HanyouKeyStart2.TxtCode.Text = tblROWS[0]["Account2"].ToString();
                SC_HanyouKeyStart2.LabelText = tblROWS[0]["start2"].ToString();
                SC_HanyouKeyEnd2.TxtCode.Text = tblROWS[0]["SubAccount2"].ToString();
                SC_HanyouKeyEnd2.LabelText = tblROWS[0]["end2label"].ToString();
            }
        }

        private void SelectKeyData()
        {
            //dtIDName1 = shnbl.M_Multipurpose_SelectIDName("217");
            //dtIDName2 = shnbl.M_Multipurpose_SelectIDName("218");

            //SC_HanyouKeyStart1.Value1 = dtIDName1.Rows[0]["ID"].ToString();
            //SC_HanyouKeyStart1.Value2 = dtIDName1.Rows[0]["IDName"].ToString();

            //SC_HanyouKeyStart2.Value1 = dtIDName1.Rows[0]["ID"].ToString();
            //SC_HanyouKeyStart2.Value2 = dtIDName1.Rows[0]["IDName"].ToString();

            ////SC_HanyouKeyEnd1.Value1 = dtIDName2.Rows[0]["ID"].ToString();
            //SC_HanyouKeyEnd1.Value2 = dtIDName2.Rows[0]["IDName"].ToString();

            //SC_HanyouKeyEnd2.Value1 = dtIDName2.Rows[0]["ID"].ToString();
            //SC_HanyouKeyEnd2.Value2 = dtIDName2.Rows[0]["IDName"].ToString();
        }

        private bool CheckRequireField()
        {
            if (bbl.Z_Set(txtTransferAmount.Text) > 0)
            {
                if (!RequireCheck(new Control[] { SC_BankCD.TxtCode, SC_BranchCD.TxtCode, txtKouzaKBN, txtAccNo, txtMeigi , txtFeeKBN , txtAmount }))
                    return false;
            }

            if(bbl.Z_Set(txtBill.Text) > 0)
            {
                if (!RequireCheck(new Control[] { txtBillNo, txtBillDate }))
                    return false;
            }

            if(bbl.Z_Set(txtElectronicBone.Text) > 0)
            {
                if (!RequireCheck(new Control[] { txtElectronicRecordNo, txtSettlementDate2 }))
                    return false;
            }
            if(bbl.Z_Set(txtOther1.Text)>0)
            {
                if (!RequireCheck(new Control[] { SC_HanyouKeyStart1.TxtCode, SC_HanyouKeyEnd1.TxtCode }))
                    return false;
            }
            if (bbl.Z_Set(txtOther2.Text) > 0)
            {
                if (!RequireCheck(new Control[] { SC_HanyouKeyStart2.TxtCode, SC_HanyouKeyEnd2.TxtCode }))
                    return false;
            }
            return true;
        }
        private bool CheckBankCD(bool F12 = false)
        {
            SC_BankCD.LabelText = "";

            if (!string.IsNullOrWhiteSpace(SC_BankCD.TxtCode.Text))
            {
                SC_BankCD.ChangeDate = dpe.PayDate;
                if (SC_BankCD.SelectData())
                {
                    SC_BranchCD.Value1 = SC_BankCD.TxtCode.Text;
                    SC_BranchCD.Value2 = SC_BankCD.LabelText;

                    if (!F12)
                        Select_KouzaFee();

                }
                else
                {
                    bbl.ShowMessage("E101");
                    SC_BankCD.SetFocus(1);
                }
            }

            return true;
        }
        private bool CheckBranchCD(bool F12 = false)
        {
            SC_BranchCD.LabelText = "";

            if (!string.IsNullOrWhiteSpace(SC_BranchCD.TxtCode.Text))
            {
                SC_BranchCD.ChangeDate = dpe.PayDate;
                SC_BranchCD.Value1 = SC_BankCD.TxtCode.Text;
                if (!SC_BranchCD.SelectData())
                {
                    bbl.ShowMessage("E101");
                    SC_BranchCD.SetFocus(1);
                }
                else
                {
                    if (!F12)
                        Select_KouzaFee();
                }
            }


            return true;
        }
        private bool CheckKBN(short kbn, bool F12 = false)
        {
            switch (kbn)
            {
                case 1://KouzaKBN
                    if (!string.IsNullOrWhiteSpace(txtKouzaKBN.Text))
                    {

                        if (txtKouzaKBN.Text != "1")
                        {
                            if (txtKouzaKBN.Text != "2")
                            {
                                bbl.ShowMessage("E101");
                                txtKouzaKBN.Focus();
                                return false;
                            }
                        }
                    }
                    break;

                case 2: //FeeKBN
                    if (!string.IsNullOrWhiteSpace(txtFeeKBN.Text))
                    {
                        if (!txtFeeKBN.Text.Equals("1"))
                        {
                            if (!txtFeeKBN.Text.Equals("2"))
                            {
                                bbl.ShowMessage("E101");
                                txtFeeKBN.Focus();
                                return false;
                            }
                        }
                        if (!F12)
                            Select_KouzaFee();
                    }
                    break;

            }
            return true;
        }
        private bool CheckHanyo(Search.CKM_SearchControl sc)
        {
            sc.LabelText = "";

            if (!string.IsNullOrWhiteSpace(sc.TxtCode.Text))
            {
                if (!sc.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    sc.SetFocus(1);
                    return false;
                }
            }

            return true;
        }
        public void LabelDataBind()
        {
            decimal sum1 = 0;
            decimal sum2 = 0;
            decimal sum3 = 0;
            decimal sum4 = 0;

            for (int i = 0; i < dgvSearchPayment.Rows.Count; ++i)
            {
                sum1 += bbl.Z_Set(dgvSearchPayment.Rows[i].Cells[3].Value);
                sum2 += bbl.Z_Set(dgvSearchPayment.Rows[i].Cells[4].Value);
                sum3 += bbl.Z_Set(dgvSearchPayment.Rows[i].Cells[5].Value);
                sum4 += bbl.Z_Set(dgvSearchPayment.Rows[i].Cells[6].Value);
            }
            lblPayPlanGaku.Text = sum1.ToString("#,##0");
            lblPayComfirmGaku.Text = sum2.ToString("#,##0");
            lblPayGaku.Text =lblPayGaku1.Text= sum3.ToString("#,##0");
            lblUnpaidAmount.Text = sum4.ToString("#,##0");
            txtTransferAmount.Text = bbl.Z_SetStr(sum3);
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
                if(ErrorCheck())
                    SendData();
            }
                
        }


        public void SendData()
        {

            if(dtGdv.Rows.Count>0)
            {
                SetData();

            }
            if (dtDetails.Rows.Count > 0)
            {
                /*dtSiharai2=*/
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
            DataRow[] tblROWS = dtGdv.Select("PayeeCD = '" + payeeCD + "'" + "and PayPlanDate = '" + payPlanDate + "'");
            //foreach(DataRow row in tblROWS)
            //{
            //    dtGdv.Rows.Remove(row);
            //    dtGdv.AcceptChanges();
            //}
            //dtGdv.Merge(dtSiharai1);

            tblROWS[0]["TransferGaku"] = bbl.Z_Set(txtTransferAmount.Text) ;
            tblROWS[0]["BankCD"] = SC_BankCD.TxtCode.Text;
            tblROWS[0]["BankName"] = SC_BankCD.LabelText;
            tblROWS[0]["BranchCD"] = SC_BranchCD.TxtCode.Text;
            tblROWS[0]["BranchName"] = SC_BranchCD.LabelText;
            tblROWS[0]["KouzaKBN"] = txtKouzaKBN.Text;
            tblROWS[0]["KouzaNO"] = txtAccNo.Text;
            tblROWS[0]["KouzaMeigi"] = txtMeigi.Text;
            tblROWS[0]["FeeKBNVal"] = txtFeeKBN.Text;
            //tblROWS[0]["Fee"] = txtAmount.Text;
            tblROWS[0]["TransferFeeGaku"] = bbl.Z_Set(txtAmount.Text);

            tblROWS[0]["CashGaku"] = bbl.Z_Set(txtCash.Text);
            tblROWS[0]["OffsetGaku"] = bbl.Z_Set(txtOffsetGaku.Text);
            tblROWS[0]["BillGaku"] = bbl.Z_Set(txtBill.Text);
            tblROWS[0]["BillNO"] = txtBillNo.Text;
            tblROWS[0]["BillDate"] = txtBillDate.Text;

            tblROWS[0]["ERMCGaku"] = bbl.Z_Set(txtElectronicBone.Text);
            tblROWS[0]["ERMCNO"] = txtElectronicRecordNo.Text;
            tblROWS[0]["ERMCDate"] = txtSettlementDate2.Text;

            tblROWS[0]["OtherGaku1"] = bbl.Z_Set(txtOther1.Text);
            tblROWS[0]["Account1"] = SC_HanyouKeyStart1.TxtCode.Text;
            //tblROWS[0]["start1"] = SC_HanyouKeyStart1.LabelText;
            tblROWS[0]["SubAccount1"] = SC_HanyouKeyEnd1.TxtCode.Text;
            //tblROWS[0]["end1label"] = SC_HanyouKeyEnd1.LabelText;

            tblROWS[0]["OtherGaku2"] = bbl.Z_Set(txtOther2.Text);
            tblROWS[0]["Account2"] = SC_HanyouKeyStart2.TxtCode.Text;
            //tblROWS[0]["start2"] = SC_HanyouKeyStart2.LabelText;
            tblROWS[0]["SubAccount2"] = SC_HanyouKeyEnd2.TxtCode.Text;
            //tblROWS[0]["end2label"] = SC_HanyouKeyEnd2.LabelText;
            //dtGdv.AcceptChanges();
            //return dtGdv;
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
        }

        /// <summary>
        /// Error Check of F12
        /// </summary>
        /// <returns></returns>
        public bool ErrorCheck()
        {
            if (!CheckRequireField())
            {
                return false;
            }
            if (!CheckBankCD(true))
            {
                return false;
            }
            if (!CheckBranchCD(true))
            {
                return false;
            }
            if (!CheckKBN(1))
            {
                return false;
            }
            if (!CheckKBN(2, true))
            {
                return false;
            }
            if (!CheckHanyo(SC_HanyouKeyStart1))
            {
                return false;
            }
            if (!CheckHanyo(SC_HanyouKeyEnd1))
            {
                return false;
            }
            if (!CheckHanyo(SC_HanyouKeyStart2))
            {
                return false;
            }
            if (!CheckHanyo(SC_HanyouKeyEnd2))
            {
                return false;
            }

           return true;
        }       

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
        private void SC_HanyouKeyStart1_Leave(object sender, EventArgs e)
        {
            SC_HanyouKeyEnd1.Value3 = SC_HanyouKeyStart1.TxtCode.Text;
        }
        private void SC_HanyouKeyStart2_Leave(object sender, EventArgs e)
        {
            SC_HanyouKeyEnd2.Value3 = SC_HanyouKeyStart2.TxtCode.Text;
        }
        #region  KeyDown event

        /// <summary>
        /// Fee calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SC_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    Select_KouzaFee();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void SC_BankCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckBankCD())
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void SC_BranchCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckBranchCD())
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void txtFeeKBN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckKBN(2))
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void txtKouzaKBN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckKBN(1))
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }


        private void SC_HanyouKeyStart1_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckHanyo(SC_HanyouKeyStart1))
                    {
                        return;
                    }
                    SC_HanyouKeyEnd1.Value3 = SC_HanyouKeyStart1.TxtCode.Text;
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void SC_HanyouKeyStart2_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {

            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckHanyo(SC_HanyouKeyStart2))
                    {
                        return;
                    }
                    SC_HanyouKeyEnd2.Value3 = SC_HanyouKeyStart2.TxtCode.Text;
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

        }

        private void SC_HanyouKeyEnd1_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {

            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckHanyo(SC_HanyouKeyEnd1))
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void SC_HanyouKeyEnd2_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (!CheckHanyo(SC_HanyouKeyEnd2))
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        #endregion

        public void Select_KouzaFee()
        {
            if (!string.IsNullOrWhiteSpace(SC_BankCD.TxtCode.Text) && !string.IsNullOrWhiteSpace(SC_BranchCD.TxtCode.Text)
                             && !string.IsNullOrWhiteSpace(txtFeeKBN.Text) && txtAmount.Text.Equals("0"))
            {
                M_Kouza_Entity mkze = new M_Kouza_Entity
                {
                    KouzaCD = kouzaCD,
                    BankCD = SC_BankCD.TxtCode.Text,
                    BranchCD = SC_BranchCD.TxtCode.Text,
                    Amount = lblPayGaku.Text.Replace(",", ""),

                };
                DataTable dt=shnbl.M_Kouza_FeeSelect(mkze);
                txtTransferAmount.Text =bbl.Z_SetStr(dt.Rows[0]["Fee"]);
            }

        }

        protected override void EndSec()
        {
            flgCancel = true;
            this.Close();
        }

        private void SiharaiTouroku_2_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void dgvSearchPayment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Maintained_CheckClick(sender, e);
        }

        protected void Maintained_CheckClick(object sender, DataGridViewCellEventArgs e)
       {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if ((sender as DataGridView).CurrentCell is DataGridViewCheckBoxCell)
                {
                    if ((Convert.ToBoolean(dgvSearchPayment.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue) == true))
                    {
                        DataGridViewCheckBoxCell chk1 = dgvSearchPayment.Rows[e.RowIndex].Cells["colChk"] as DataGridViewCheckBoxCell;
                        dgvSearchPayment.Rows[e.RowIndex].Cells["colUnpaidAmount1"].Value = bbl.Z_Set(dgvSearchPayment.Rows[e.RowIndex].Cells["colPayPlanGaku"].Value.ToString()) - bbl.Z_Set(dgvSearchPayment.Rows[e.RowIndex].Cells["colPayConfirmGaku"].Value.ToString());
                        dgvSearchPayment.Rows[e.RowIndex].Cells["colPayConfirmGaku"].Value = "0";
                    }
                    else
                    {
                        DataGridViewCheckBoxCell chk1 = dgvSearchPayment.Rows[e.RowIndex].Cells["colChk"] as DataGridViewCheckBoxCell;
                        dgvSearchPayment.Rows[e.RowIndex].Cells["colUnpaidAmount1"].Value = "0";
                        dgvSearchPayment.Rows[e.RowIndex].Cells["colPayConfirmGaku"].Value = bbl.Z_Set(dgvSearchPayment.Rows[e.RowIndex].Cells["colPayPlanGaku"].Value.ToString()) - bbl.Z_Set(dgvSearchPayment.Rows[e.RowIndex].Cells["colPayConfirmGaku"].Value.ToString());
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
            if (dgvSearchPayment.CurrentRow.Index >= 0)
            {
                if (dgvSearchPayment.CurrentCell == dgvSearchPayment.CurrentRow.Cells["colUnpaidAmount1"])
                {
                    DataGridViewRow row = dgvSearchPayment.CurrentRow;

                    if (string.IsNullOrWhiteSpace(row.Cells["colUnpaidAmount1"].Value.ToString()))
                    {
                        bbl.ShowMessage("E102");
                        dgvSearchPayment.CurrentCell = dgvSearchPayment.CurrentRow.Cells["colPayConfirmGaku"];
                    }
                    else if (bbl.Z_Set(row.Cells["colUnpaidAmount1"].Value.ToString()) > bbl.Z_Set(row.Cells["colUnpaidAmount2"].Value.ToString()) || bbl.Z_Set(row.Cells["colUnpaidAmount1"].Value.ToString()) < 0)
                    //else if(row.Cells["colUnpaidAmount1"].Value > row.Cells["col"])
                    {
                        bbl.ShowMessage("E143");
                        dgvSearchPayment.CurrentCell = dgvSearchPayment.CurrentRow.Cells["colPayConfirmGaku"];
                    }
                    else
                    {
                        row.Cells["colUnpaidAmount2"].Value = bbl.Z_Set(row.Cells["colPayPlanGaku"].Value.ToString()) - bbl.Z_Set(row.Cells["colPayConfirmGaku"].Value.ToString()) - bbl.Z_Set(row.Cells["colUnpaidAmount1"].Value.ToString());
                    }
                        LabelDataBind();
                }
            }
        }

    }
}
