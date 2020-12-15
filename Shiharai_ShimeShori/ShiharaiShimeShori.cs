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
using CKM_Controls;
using BL;
using Search;
using Entity;

namespace Shiharai_ShimeShori
{
    public partial class Shiharai_ShimeShori : FrmMainForm
    {
        Shiharai_Shimeshori_BL sss_bl;
        D_PayCloseHistory_Entity dpch_entity;
        D_PayPlan_Entity dpp_e;
        Base_BL bbl;
        Exclusive_BL e_bl;
        D_Exclusive_Entity de_e;
        DataTable dtDisplay;
        string Num = string.Empty;
        string PayCloseNo = string.Empty;
        string EProgram, EOperator, EPc = String.Empty;
        public Shiharai_ShimeShori()
        {
            InitializeComponent();
            dtDisplay = new DataTable();
            dpch_entity = new D_PayCloseHistory_Entity();
            sss_bl = new Shiharai_Shimeshori_BL();
            bbl = new Base_BL();
            dpp_e = new D_PayPlan_Entity();
            de_e = new D_Exclusive_Entity();
            e_bl = new Exclusive_BL();
         
        }
        private void ShiharaiShimeShori_Load(object sender, EventArgs e)
        {
            InProgramID = "Shiharai_ShimeShori";
            SetFunctionLabel(EProMode.INPUT);
            StartProgram();
            BindCombo();            
            RequireFields();
            this.ModeVisible = false;
            this.ModeText = "修正";
            F7Visible = false;
            F8Visible = false;
            F10Visible = false;
            txtPayCloseDate.Text = bbl.GetDate();
            DeleteExclusive();
        }
        private void RequireFields()
        {
            txtPayCloseDate.Require(true);
            //ScPaymentCD.TxtCode.Require(true);           
        }

        private void DeleteExclusive()
        {
            de_e = GetExclusiveData();
            e_bl.D_Exclusive_DeleteByKBN(de_e);
        }
        private void BindCombo()
        {
            cboProcessType.Items.Add("支払締");
            cboProcessType.Items.Add("支払締キャンセル");
        }
        public override void FunctionProcess(int index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
            switch (index + 1)
            {
                
                case 2:
                    ChangeMode(EOperationMode.INSERT);
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeMode(EOperationMode.INSERT);
                        cboProcessType.Focus();
                        DeleteExclusive();
                    }
                    break;
                case 12:
                    F12();
                    break;
            }
        }

        private D_PayPlan_Entity GetPayPlan()
        {
            dpp_e=new D_PayPlan_Entity()
            {
                PayCloseDate=txtPayCloseDate.Text,
                PayeeCD= Shiiresaki.TxtCode.Text

            };
            return dpp_e;
        }


        //private D_Exclusive_Entity GetExclusiveData(int type)
        //{
        //    if(type ==1)
        //    {
        //        de_e = new D_Exclusive_Entity()
        //        {
        //            DataKBN = 9,
        //            Number = Num,
        //            Program = this.InProgramID,
        //            Operator = this.InOperatorCD,
        //            PC = this.InPcID

        //        };
        //        return de_e;
        //    }
        //    else
        //    {
        //        de_e = new D_Exclusive_Entity()
        //        {
        //            DataKBN = 9,
        //            Number = PayCloseNo,
        //            Program = this.InProgramID,
        //            Operator = this.InOperatorCD,
        //            PC = this.InPcID
        //        };
        //        return de_e;
        //    }

        //}

        private D_Exclusive_Entity GetExclusiveData()
        {
                de_e = new D_Exclusive_Entity()
                {
                    DataKBN = 9,
                    Program = this.InProgramID,
                    Operator = this.InOperatorCD,
                    PC = this.InPcID
                };
                return de_e;
        }

        private void F12()
        {
            DataTable dTNo = new DataTable();
            dTNo.Columns.Add("Number");
            DataTable dTPayCloseNo = new DataTable();
            dTPayCloseNo.Columns.Add("PayCloseNo");
            bool check = false;
            bool checkc = false;
            bool checkS = false;
            bool checkEC = false;
            DataTable dtno = new DataTable();
            DataTable dtpayno = new DataTable();
            string ItemType = cboProcessType.Text;
            dpp_e = GetPayPlan();
            DataTable payShime = sss_bl.D_PayPlanValue_Select(dpp_e, "3");
            DataTable payShimeCancel = sss_bl.D_PayPlanValue_Select(dpp_e, "4");
            switch (ItemType)
            {
                case "支払締":
                    if (ErrorCheck(1))
                    {
                        if(payShime.Rows.Count >0)
                        {
                            for(int k=0;k < payShime.Rows.Count;k ++)
                            {
                                dpp_e.PayeeCD = payShime.Rows[k]["PayeeCD"].ToString();
                                var dtEN = sss_bl.D_PayPlanValue_Select(dpp_e, "5");
                                if (dtEN.Rows.Count > 0)
                                {
                                    EProgram = dtEN.Rows[0]["Program"].ToString();
                                    EOperator = dtEN.Rows[0]["Operator"].ToString();
                                    EPc = dtEN.Rows[0]["PC"].ToString();
                                    sss_bl.ShowMessage("S004", EProgram, EOperator, EPc);
                                    cboProcessType.Focus();
                                   // return;
                                }
                                else
                                {
                                    dtno = sss_bl.D_PayPlanValue_Select(dpp_e, "1");
                                    if (dtno.Rows.Count > 0)
                                    {
                                        Num = dtno.Rows[0]["Number"].ToString();
                                    }
                                    de_e = new D_Exclusive_Entity()
                                    {
                                        DataKBN = 9,
                                        Number = Num,
                                        Program = this.InProgramID,
                                        Operator = this.InOperatorCD,
                                        PC = this.InPcID
                                    };
                                    dTNo.Rows.Add(Num);
                                    e_bl.D_Exclusive_Insert(de_e);
                                    checkS = true;
                                }
                            }
                        }
                        if (bbl.ShowMessage("Q101") == DialogResult.Yes)
                        {
                            dpp_e = GetPayPlan();
                            DataTable dtpayee = sss_bl.D_PayPlanValue_Select(dpp_e, "3");
                            if (dtpayee.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtpayee.Rows.Count; i++)
                                {

                                    dpch_entity = GetDataEntity();
                                    dpch_entity.PaymentCD = dtpayee.Rows[i]["PayeeCD"].ToString();
                                    if (sss_bl.Insert_ShiHaRaiShime_PaymentClose(dpch_entity, 1))
                                    {
                                       check = true;
                                        de_e.DataKBN = 9;
                                        if(checkS)
                                        {
                                            if (dTNo.Rows.Count > 0)
                                            {
                                                de_e.Number = dTNo.Rows[i]["Number"].ToString();
                                                e_bl.D_Exclusive_Delete(de_e);
                                            }
                                        }
                                    }
                                }
                                if(check)
                                {
                                    sss_bl.ShowMessage("I101");
                                    ChangeMode(EOperationMode.INSERT);
                                   
                                }
                            }
                        }
                    }
                    break;

                case "支払締キャンセル":
                    if (ErrorCheck(2))
                    {
                        if(payShimeCancel.Rows.Count > 0)

                        {
                            for(int d=0;d < payShimeCancel.Rows.Count; d ++ )
                            {
                                dpp_e.PayeeCD = payShimeCancel.Rows[d]["PayeeCD"].ToString();
                                var dtEP = sss_bl.D_PayPlanValue_Select(dpp_e, "6");
                                if (dtEP.Rows.Count > 0)
                                {
                                    EProgram = dtEP.Rows[0]["Program"].ToString();
                                    EOperator = dtEP.Rows[0]["Operator"].ToString();
                                    EPc = dtEP.Rows[0]["PC"].ToString();
                                    sss_bl.ShowMessage("S004", EProgram, EOperator, EPc);
                                    cboProcessType.Focus();
                                    
                                }
                                else
                                {
                                    dtpayno = sss_bl.D_PayPlanValue_Select(dpp_e, "2");
                                    if (dtpayno.Rows.Count > 0)
                                    {
                                        PayCloseNo = dtpayno.Rows[0]["PayCloseNo"].ToString();
                                    }

                                    de_e = new D_Exclusive_Entity()
                                    {
                                        DataKBN = 9,
                                        Number = PayCloseNo,
                                        Program = this.InProgramID,
                                        Operator = this.InOperatorCD,
                                        PC = this.InPcID
                                    };
                                    dTPayCloseNo.Rows.Add(PayCloseNo);
                                    e_bl.D_Exclusive_Insert(de_e);
                                    checkEC = true;
                                }
                            }
                        }

                        if (bbl.ShowMessage("Q102") == DialogResult.Yes)
                        {
                            dpp_e = GetPayPlan();
                            DataTable dtpayee = sss_bl.D_PayPlanValue_Select(dpp_e, "4");
                            if (dtpayee.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtpayee.Rows.Count; i++)
                                {
                                    dpch_entity = GetDataEntity();
                                    dpch_entity.PaymentCD = dtpayee.Rows[i]["PayeeCD"].ToString();
                                    if (sss_bl.Insert_ShiHaRaiShime_PaymentClose(dpch_entity, 2))
                                    {
                                        checkc = true;
                                        de_e.DataKBN = 9;
                                        if(checkEC)
                                        {
                                        if(dTPayCloseNo .Rows.Count >0)
                                        {
                                            de_e.Number =dTPayCloseNo.Rows[i]["PayCloseNo"].ToString() ;
                                            e_bl.D_Exclusive_Delete(de_e);
                                        }
                                        }
                                    }
                                }
                                if(checkc)
                                {
                                    sss_bl.ShowMessage("I101");
                                    ChangeMode(EOperationMode.INSERT);
                                    
                                }
                            }
                        }
                    }
                    break;
            }
        }
        private void F11()
        {
           
            dpch_entity = GetDataEntity();
            BindGrid();
            
        }
        private void BindGrid()
        {
            dgvPaymentClose.ClearSelection();
           // dpch_entity = GetDataEntity();
            
            DataTable dtPayCost = sss_bl.D_PayClose_Search(dpch_entity);
            if (dtPayCost.Rows.Count > 0)
            {
                dgvPaymentClose.Refresh();
                dgvPaymentClose.DataSource = dtPayCost;
                dgvPaymentClose.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dgvPaymentClose.CurrentRow.Selected = true;
                dgvPaymentClose.Enabled = true;
                dgvPaymentClose.Focus();
            }
            else
            {
                dgvPaymentClose.DataSource = null;
                //bbl.ShowMessage("E128");
            }
        }
        private bool ErrorCheck(int Type)
        {
            dpch_entity = GetDataEntity();
            dpp_e = GetPayPlan();
            if (!string.IsNullOrEmpty(Shiiresaki.TxtCode.Text))
            {
                if (!Shiiresaki.IsExists(2))
                {
                    sss_bl.ShowMessage("E101");
                    Shiiresaki.SetFocus(1);
                    return false;
                }
            }

            switch (Type)
            {
                case 1:
                    if (!RequireCheck(new Control[] { txtPayCloseDate })) 
                        return false;


                    if (sss_bl.Select_PaymentClose(dpch_entity, 2))
                    {
                        sss_bl.ShowMessage("S014");
                        cboProcessType.Focus();
                        return false;
                    }

                    if (!sss_bl.Select_PaymentClose(dpch_entity, 1))
                    {
                        sss_bl.ShowMessage("S013");
                        cboProcessType.Focus();
                        return false;
                    }

                    //var dtEN = sss_bl.D_PayPlanValue_Select(dpp_e, "5");
                    //if (dtEN.Rows.Count >0)
                    //{
                    //    EProgram = dtEN.Rows[0]["Program"].ToString();
                    //    EOperator= dtEN.Rows[0]["Operator"].ToString();
                    //    EPc= dtEN.Rows[0]["PC"].ToString();
                    //    sss_bl.ShowMessage("S004",EProgram,EOperator,EPc);
                    //    cboProcessType.Focus();
                    //    return false;
                    //}
                    break;



                case 2:
                    if (!RequireCheck(new Control[] { txtPayCloseDate })) 
                        return false;

                    if (sss_bl.Select_PaymentClose(dpch_entity, 4))
                    {
                        sss_bl.ShowMessage("S015");
                        cboProcessType.Focus();
                        return false;
                    }
                    if (!sss_bl.Select_PaymentClose(dpch_entity, 3))
                    {
                        sss_bl.ShowMessage("S013");
                        cboProcessType.Focus();
                        return false;
                    }

                    //var dtEP = sss_bl.D_PayPlanValue_Select(dpp_e, "6");
                    //if (dtEP.Rows.Count > 0)
                    //{
                    //    EProgram = dtEP.Rows[0]["Program"].ToString();
                    //    EOperator = dtEP.Rows[0]["Operator"].ToString();
                    //    EPc = dtEP.Rows[0]["PC"].ToString();
                    //    sss_bl.ShowMessage("S004", EProgram, EOperator, EPc);
                    //    cboProcessType.Focus();
                    //    return false;
                    //}
                    break;
            }
            return true;
        }
        /// <summary>
        /// Get Data Value Bind
        /// </summary>
        /// <returns>Value Return</returns>
        private D_PayCloseHistory_Entity GetDataEntity()
        {
            dpch_entity = new D_PayCloseHistory_Entity()
            {
                PaymentDate = txtPayCloseDate.Text,
                PaymentCD = Shiiresaki.TxtCode.Text,
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                Key = Shiiresaki.TxtCode.Text + " " + txtPayCloseDate.Text,
                PC = InPcID,
                StoreCD=StoreCD
            };

            return dpch_entity;
        }
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    cboProcessType.Text = string.Empty;
                    cboProcessType.SelectedText = "支払締";
                    txtPayCloseDate.Text = string.Empty;
                    Shiiresaki.TxtCode.Text = string.Empty;
                    Shiiresaki.LabelText = string.Empty;
                    txtPayCloseDate.Text = bbl.GetDate();
                    F2Visible = false;
                    F3Visible = false;
                    F4Visible = false;
                    F5Visible = false;
                    F9Visible = false;
                    F11Visible = false;
                    dgvPaymentClose.DataSource = null;
                    cboProcessType.Focus();
                    break;
            }
        }

        protected override void EndSec()
        {
            DeleteExclusive();
            this.Close();
        }

        /// <summary>
        /// MoveNext Cursor Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShiharaiShimeShori_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        /// <summary>
        /// Vendor code and Name Case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
     
        private void txtPayCloseDate_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                DataTable dtResult = new DataTable();
                dtResult = sss_bl.Check_PayCloseDate(txtPayCloseDate.Text);
                if (dtResult.Rows.Count==0)
                {
                    sss_bl.ShowMessage("E115");
                    txtPayCloseDate.Focus();
                }
            }
        }
        private void dgvPaymentClose_Paint(object sender, PaintEventArgs e)
        {
            dgvPaymentClose.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPaymentClose.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPaymentClose.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPaymentClose.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPaymentClose.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            string[] monthes = { "","支払先" };
            for (int j = 2; j < 4;)
            {
                Rectangle r1 = this.dgvPaymentClose.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.dgvPaymentClose.GetCellDisplayRectangle(j + 1, -1, true).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dgvPaymentClose.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(monthes[j / 2],
                this.dgvPaymentClose.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.dgvPaymentClose.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 3;
            }
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            //F11();
        }
        private void Shiiresaki_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Shiiresaki.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(Shiiresaki.TxtCode.Text))
                {
                    if (Shiiresaki.SelectData())
                    {
                        //Shiiresaki.Value1 = Shiiresaki.TxtCode.Text;
                        //Shiiresaki.Value2 = Shiiresaki.LabelText;
                        F11();
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        Shiiresaki.SetFocus(1);
                        dgvPaymentClose.DataSource = null;
                    }
                }
                else { F11(); }
            }
        }
        private void Shiiresaki_Enter(object sender, EventArgs e)
        {
            Shiiresaki.ChangeDate = String.IsNullOrEmpty(txtPayCloseDate.Text) ? bbl.GetDate() : txtPayCloseDate.Text;
            Shiiresaki.Value1 = "3";
        }

        
    }
}
