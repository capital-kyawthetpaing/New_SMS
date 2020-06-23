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
using System.Drawing;


namespace Search
{
    public partial class frmSearch_KeihiNO : FrmSubForm
    {
        //string paid = "";
        //string unpaid = "";
        //string staffcd = "";
        Search_KeihiNO_BL skhnobl;
        D_Cost_Entity dcoste;
        Base_BL bbl ;
        public string ExpenseNumber;
        //private Control[] detailControls;
        public frmSearch_KeihiNO()
        {
            InitializeComponent();           
            //InitialControlArray();            
        }
        //更新 ssa
        private void frmSearch_KeihiNO_Load(object sender, EventArgs e)
        {           
            ExpenseNumber = string.Empty;
            skhnobl = new Search_KeihiNO_BL();
            bbl = new Base_BL();
            F9Visible = false;
            //chkPaid.Checked = true;
            //chkUnpaid.Checked = true;
            //chkTeiki.Checked = true;
            chkUnpaid.Checked = true;
            F11Visible = false;
        }
        //ZCO
        //private void InitialControlArray()
        //{
        //    detailControls = new Control[] { txtRecordDateFrom, txtRecordDateTo,SearchStaff.TxtCode, txtEntryDateFrom, txtEntryDateTo,chkTeiki,
        //        txtPaymentDueDateFrom,txtPaymentDueDateTo,searchPaymentDes.TxtCode,chkPaid,chkUnpaid,txtPaymentDateFrom,txtPaymentDateTo};

        //    foreach (Control ctl in detailControls)
        //    {
        //        ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
        //        //ctl.Enter += new System.EventHandler(DetailControl_Enter);
        //    }


        //}
        //private void DetailControl_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        //Enterキー押下時処理
        //        //Returnキーが押されているか調べる
        //        //AltかCtrlキーが押されている時は、本来の動作をさせる
        //        if ((e.KeyCode == Keys.Return) &&
        //            ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
        //        {
        //            //bool ret = CheckDetail(Array.IndexOf(detailControls, sender));
        //           // if (ret)
        //            //{
        //                if (detailControls.Length - 1 > Array.IndexOf(detailControls, sender))
        //                    detailControls[Array.IndexOf(detailControls, sender) + 1].Focus();

        //                else
        //                    //あたかもTabキーが押されたかのようにする
        //                    //Shiftが押されている時は前のコントロールのフォーカスを移動
        //                    this.ProcessTabKey(!e.Shift);
        //            //}
        //            //else
        //            //{
        //              //  ((Control)sender).Focus();
        //            //}
        //        }
        //        else
        //        {
        //            ((Control)sender).Focus();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //        //EndSec();
        //    }
        //}
        private D_Cost_Entity GetSearchInfo()
        {
            dcoste = new D_Cost_Entity
            {
                RecordedDateFrom = txtRecordDateFrom.Text,
                RecordedDateTo = txtRecordDateTo.Text,
                ExpanseEntryDateFrom = txtEntryDateFrom.Text,
                ExpanseEntryDateTo = txtEntryDateTo.Text,
                PaymentDueDateFrom = txtPaymentDueDateFrom.Text,
                PaymentDueDateTo = txtPaymentDueDateTo.Text,
                StaffCD = scStaffCD.TxtCode.Text,
                PayeeCD = PaymentCD.TxtCode.Text,
                Paid = chkPaid.Checked ? "1" : "0", //更新 ssa
                Unpaid = chkUnpaid.Checked ? "0" : "1", //更新 ssa
                RegularlyFLG = chkTeiki.Checked ? "1" : "0",
                 PaymentDateFrom =txtPaymentDateFrom.Text,
                PaymentDateTo=txtPaymentDateTo.Text
            };

                   return dcoste;
        }  
 
        private void F11()
        {   
            //更新 ssa
            dgvCostSearch.ClearSelection();
            if (ErrorCheck())//エラーチェック事
            { 
                //<Remark>フォームデータを取る事</Remark>           
                dcoste = GetSearchInfo();
                //<Remark>データベースに登録したデータを取る事</Remark>       
                DataTable dtCost = skhnobl.D_Cost_Search(dcoste);
                for (int i = 0; i < dtCost.Rows.Count; i++)
                {
                   
                    string R_Flag = dtCost.Rows[i]["RegularlyFLG"].ToString();
                    if (R_Flag.Equals("1")) 
                    //if (dtCost.Rows[i]["RegularlyFLG"].ToString() == "1")//zco
                    {
                        dtCost.Rows[i]["RegularlyFLG"] = "〇";
                    }
                    else
                    {
                        dtCost.Rows[i]["RegularlyFLG"] = "";
                    }
                }
                if (dtCost.Rows.Count > 0)
                {
                    dgvCostSearch.Refresh();
                    dgvCostSearch.DataSource = dtCost;
                    dgvCostSearch.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    dgvCostSearch.CurrentRow.Selected = true;
                    dgvCostSearch.Enabled = true;
                    dgvCostSearch.Focus();
                }
                else
                {
                    skhnobl.ShowMessage("E128"); //更新 ssa
                    dgvCostSearch.DataSource = null; 
                }
            }

        }
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                              
                case 11:
                    F11();
                    break;
                case 12:
                    GetData();
                    break;
            }
        }
        /// <summary>
        /// 表示されたデータを選択して確定(F12)ボタンを押下時選択されたデータを取得する
        /// </summary>
        private void GetData()
        {
            try
            {
                if (dgvCostSearch.CurrentRow != null && dgvCostSearch.CurrentRow.Index >= 0)
                {
                    ExpenseNumber = dgvCostSearch.CurrentRow.Cells["ExpenseNo"].Value.ToString();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
           
        }
        private bool ErrorCheck()
        {
            if (!string.IsNullOrWhiteSpace(txtRecordDateFrom.Text) && !string.IsNullOrWhiteSpace(txtRecordDateTo.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtRecordDateFrom.Text);
                DateTime dt2 = Convert.ToDateTime(txtRecordDateTo.Text);
             
                if (dt1>dt2)
                {
                    skhnobl.ShowMessage("E104");
                    txtRecordDateTo.Focus();
                    return false;
                }

            }
            if (!string.IsNullOrWhiteSpace(txtEntryDateFrom.Text) && !string.IsNullOrWhiteSpace(txtEntryDateTo.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtEntryDateFrom.Text);
                DateTime dt2 = Convert.ToDateTime(txtEntryDateTo.Text);
                
                if (dt1 > dt2)
                {
                    skhnobl.ShowMessage("E104");
                    txtEntryDateTo.Focus();
                    return false;
                }

            }
            if (!string.IsNullOrWhiteSpace(txtPaymentDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPaymentDateTo.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtPaymentDateFrom.Text);
                DateTime dt2 = Convert.ToDateTime(txtPaymentDateTo.Text);
               
                if (dt1 > dt2)
                {
                    skhnobl.ShowMessage("E104");
                    txtPaymentDateTo.Focus();
                    return false;
                }

            }
            if (!string.IsNullOrWhiteSpace(txtPaymentDueDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPaymentDueDateTo.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtPaymentDueDateFrom.Text);
                DateTime dt2 = Convert.ToDateTime(txtPaymentDueDateTo.Text);
                
                if (dt1 > dt2)
                {
                    skhnobl.ShowMessage("E104");
                    txtPaymentDueDateTo.Focus();
                    return false;
                }

            }
            if(!String.IsNullOrEmpty(PaymentCD.TxtCode.Text))
            {
                if (!PaymentCD.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    PaymentCD.SetFocus(1);
                    return false;
                }
            }
           
            //<remark>支払済、未支払のどちらもチェックが入っていない場合、エラー<remark>
            if (chkPaid.Checked == false && chkUnpaid.Checked == false)
            {
                skhnobl.ShowMessage("E111");
                chkPaid.Focus();
                return false;
            }
            if (!String.IsNullOrEmpty(scStaffCD.TxtCode.Text))
            {
                if (!scStaffCD.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    scStaffCD.SetFocus(1);
                    return false;
                }
            }
           
            //zco
            //<remark>支払済、未支払両方チェックする場合、エラー<remark>
            //if (chkPaid.Checked == true && chkUnpaid.Checked == true)
            //{
            //    MessageBox.Show("Please! Check only one Checkbox.", "Message");
            //    chkPaid.Focus();
            //    return false;
            //}

            return true;
        }
        private void btnSubF11_Click(object sender, EventArgs e)
        {
            F11();
        }
        //ZCO
        //private void chkPaid_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkPaid.Checked == true)
        //    {
        //        paid = "1";
        //        unpaid = "";
        //    }

        //}

        //private void chkUnpaid_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkUnpaid.Checked == true)
        //    {
        //        unpaid = "0";
        //        paid = "";
        //    }
        //}
        private void frmSearch_KeihiNO_KeyDown(object sender, KeyEventArgs e)
        {
        //    if (e.KeyCode == Keys.F11)
        //        F11();
        }

        private void dgvCostSearch_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }      

        private void dgvCostSearch_Paint(object sender, PaintEventArgs e)
        {

            string[] monthes = { "経費番号", "計上日", "経費入力日", "定期", "支払先","","担当スタッフ"};
            dgvCostSearch.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCostSearch.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCostSearch.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCostSearch.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCostSearch.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCostSearch.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            for (int j = 4; j < 8;)
            {
                Rectangle r1 = this.dgvCostSearch.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.dgvCostSearch.GetCellDisplayRectangle(j + 1, -1, true).Width;
                //int w2 = this.dgvYuubinBangou.GetCellDisplayRectangle(j + 2, -1, true).Width;
                r1.X += 2;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;
                dgvCostSearch.ColumnHeadersDefaultCellStyle.Font= new Font(dgvCostSearch.Font, FontStyle.Bold);
                e.Graphics.FillRectangle(new SolidBrush(this.dgvCostSearch.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(monthes[j],
                this.dgvCostSearch.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.dgvCostSearch.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 2;
            }
        }
        private void dgvCostSearch_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
          
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
            }
        }
        private void frmSearch_KeihiNO_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        private void SearchStaff_KeyDown(object sender, KeyEventArgs e)
        { 
        }
        private void scStaffCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scStaffCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scStaffCD.TxtCode.Text))
                {
                    if (scStaffCD.SelectData())
                    {
                        scStaffCD.Value1 = scStaffCD.TxtCode.Text;
                        scStaffCD.Value2 = scStaffCD.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        scStaffCD.SetFocus(1);
                    }
                }
            }
        }
      
        private void PaymentCD_Enter(object sender, EventArgs e)
        {
            PaymentCD.Value1 = "2";
            PaymentCD.ChangeDate = bbl.GetDate();
        }

        private void PaymentCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PaymentCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(PaymentCD.TxtCode.Text))
                {
                    if (PaymentCD.SelectData())
                    {

                        PaymentCD.Value1 = PaymentCD.TxtCode.Text;
                        PaymentCD.Value2 = PaymentCD.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        PaymentCD.SetFocus(1);
                    }
                }
            }
        }

        private void txtRecordDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtRecordDateFrom.Text) && !string.IsNullOrWhiteSpace(txtRecordDateTo.Text))
                {
                    DateTime dt1 = Convert.ToDateTime(txtRecordDateFrom.Text);
                    DateTime dt2 = Convert.ToDateTime(txtRecordDateTo.Text);

                    if (dt1 > dt2)
                    {
                        skhnobl.ShowMessage("E104");
                        txtRecordDateTo.Focus();
                    }
                }
            }
        }

        private void txtEntryDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtEntryDateFrom.Text) && !string.IsNullOrWhiteSpace(txtEntryDateTo.Text))
                {
                    DateTime dt1 = Convert.ToDateTime(txtEntryDateFrom.Text);
                    DateTime dt2 = Convert.ToDateTime(txtEntryDateTo.Text);

                    if (dt1 > dt2)
                    {
                        skhnobl.ShowMessage("E104");
                        txtEntryDateTo.Focus();

                    }
                }
            }
        }

        private void txtPaymentDueDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtPaymentDueDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPaymentDueDateTo.Text))
                {
                    DateTime dt1 = Convert.ToDateTime(txtPaymentDueDateFrom.Text);
                    DateTime dt2 = Convert.ToDateTime(txtPaymentDueDateTo.Text);

                    if (dt1 > dt2)
                    {
                        skhnobl.ShowMessage("E104");
                        txtPaymentDueDateTo.Focus();
                    }
                }
            }
        }

        private void txtPaymentDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtPaymentDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPaymentDateTo.Text))
                {
                    DateTime dt1 = Convert.ToDateTime(txtPaymentDateFrom.Text);
                    DateTime dt2 = Convert.ToDateTime(txtPaymentDateTo.Text);

                    if (dt1 > dt2)
                    {
                        skhnobl.ShowMessage("E104");
                        txtPaymentDateTo.Focus();
                    }
                }
            }
        }
    }
}
