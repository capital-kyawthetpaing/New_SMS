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

namespace FBDataSakusei_FBデータ作成
{
    public partial class FrmFBDataSakusei_FBデータ作成 : FrmMainForm
    {
        FBDataSakusei_BL fbbl = new FBDataSakusei_BL();
        M_Calendar_Entity mce = new M_Calendar_Entity();
        D_Pay_Entity dpe = new D_Pay_Entity();
        D_FBControl_Entity dfbe = new D_FBControl_Entity();
        D_FBData_Entity dfde = new D_FBData_Entity();
        DataTable dtgv = new DataTable();

        public FrmFBDataSakusei_FBデータ作成()
        {
            InitializeComponent();
        }

        private void FrmFBDataSakusei_FBデータ作成_Load(object sender, EventArgs e)
        {
            InProgramID = "FBDataSakusei_FBデータ作成";
            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            //F6Visible = false;
            F7Visible = false;
            F8Visible = false;
            F9Visible = false;

            StartProgram();

            Btn_F12.Text = "出力(F12)";
            ModeVisible = false;

            BindCombo();
            SetRequireField();

            cboProcess.Focus();
            //cboProcess.SelectedIndex = 0;
        }

        public void BindCombo()
        {
            //cboProcess.Items.Insert(-1,"");
            //cboProcess.Items.Add("振込データ作成");
            //cboProcess.Items.Add("振込データ削除");
            //cboProcess.Items.Add("振込データ印刷");
            //cboProcess.SelectedValue = 0;

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Key");
            dt1.Columns.Add("Value");
            dt1.Rows.Add("振込データ作成", "0");
            dt1.Rows.Add("振込データ削除", "1");
            dt1.Rows.Add("振込データ印刷", "2");
            cboProcess.BindCombo("Value", "Key", dt1);
            //cboProcess.DataSource = dt1;
            //cboProcess.DisplayMember = "振込データ作成";
            //cboProcess.ValueMember = "0";

            string ymd = bbl.GetDate();
            cboPayment.Bind(ymd);
        }

       public void SetRequireField()
       {
            cboProcess.Require(true);
            cboPayment.Require(true);
            txtPaymentDate.Require(true);
            txtTransferDate.Require(true);
       }

        protected override void EndSec()
        {
            this.Close();
        }

        public override void FunctionProcess(int Index)
        {
            switch (Index + 1)
            {
                case 2:
                case 3:
                case 4:                  
                case 5:
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear(panel1);
                        cboProcess.Focus();
                    }
                    break;
                case 10:
                    F10();
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    F12();
                    break;
            }
        }

        public void F10()
        {
            if(ErrorCheck(10))
            {
                dpe = new D_Pay_Entity
                {
                    MotoKouzaCD = cboPayment.SelectedValue.ToString(),
                    PayDate = txtPaymentDate.Text,
                    Flg = cboProcess.SelectedValue.ToString(),
                };               
                dtgv = fbbl.D_Pay_SelectForFB(dpe);
                if(dtgv.Rows.Count > 0)
                {
                    gvFBDataSakusei.DataSource = dtgv;
                }
            }
        }

        public void F11()
        {
            if (bbl.ShowMessage("Q202") == DialogResult.Yes)
            {
                
            }
        }

        public void F12()
        {
            if(ErrorCheck(12))
            {
                if(Btn_F12.Text == "出力(F12)")
                {
                    if (bbl.ShowMessage("Q301") == DialogResult.Yes)
                    {
                        dfbe = new D_FBControl_Entity
                        {
                            PayDate = txtPaymentDate.Text,
                            ActualPayDate = txtTransferDate.Text,
                            MotoKouzaCD = cboPayment.SelectedValue.ToString(),
                            StoreCD = InOperatorCD
                        };                        
                        dfde = new D_FBData_Entity
                        {
                            PayeeCD = dtgv.Rows[0]["PayeeCD"].ToString(),
                            PayeeName = dtgv.Rows[0]["VendorName"].ToString(),
                            BankCD = dtgv.Rows[0]["BankCD"].ToString(),
                            BranchCD = dtgv.Rows[0]["BranchCD"].ToString(),
                            KouzaKBN = dtgv.Rows[0]["KouzaKBN"].ToString(),
                            KouzaNO = dtgv.Rows[0]["KouzaNO"].ToString(),
                            KouzaMeigi = dtgv.Rows[0]["KouzaMeigi"].ToString(),
                            PayGaku = dtgv.Rows[0]["transferAcc"].ToString(),
                            TransferGaku = dtgv.Rows[0]["TransferGaku"].ToString(),
                            TransferFee = dtgv.Rows[0]["TransferFeeGaku"].ToString(),
                            TransferFeeKBN = dtgv.Rows[0]["FeeKBN1"].ToString(),
                            StaffCD = InOperatorCD,
                        };
                        dpe = new D_Pay_Entity
                        {                          
                            Flg = cboProcess.SelectedValue.ToString(),
                        };                       
                        if (fbbl.FBDataSakusei_Insert(dfbe, dfde,dpe))
                        {
                            Clear(panel1);
                            BindCombo();
                            cboProcess.Focus();
                        }


                    }
                }
            }
        }

        public bool ErrorCheck(int index)
        {
            if(index == 10)
            {
                if (cboProcess.SelectedValue.ToString() == null)
                {
                    bbl.ShowMessage("E102");
                    cboProcess.Focus();
                    return false;
                }
                else if (cboProcess.SelectedValue.ToString() == "0")
                {
                    Btn_F11.Text = "印刷(F11)";
                    Btn_F12.Text = "出力(F12)";
                }
                else if (cboProcess.SelectedValue.ToString() == "1")
                {
                    Btn_F12.Text = "削除(F12)";
                }
                else
                {
                    Btn_F11.Text = "印刷(F11)";
                }

                if (cboPayment.SelectedValue.ToString() == "-1")
                {
                    bbl.ShowMessage("E102");
                    cboPayment.Focus();
                    return false;
                }

                if (!RequireCheck(new Control[] { txtPaymentDate }))
                    return false;
            }
           
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { txtTransferDate }))
                    return false;
                else
                {
                    mce.CalendarDate = txtTransferDate.Text;
                    DataTable dtC = new DataTable();
                    dtC = fbbl.M_Calendar_SelectForFB(mce);
                    if (dtC.Rows.Count > 0)
                    {
                        if (dtC.Rows[0]["BankDayOff"].ToString() == "1")
                        {
                            bbl.ShowMessage("E157");
                            txtTransferDate.Focus();
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }

        private void txtTransferDate_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(string.IsNullOrWhiteSpace(txtTransferDate.Text))
                {
                    bbl.ShowMessage("E102");
                    txtTransferDate.Focus();                   
                }
                else
                {
                    mce.CalendarDate = txtTransferDate.Text;
                    DataTable dtC = new DataTable();
                    dtC = fbbl.M_Calendar_SelectForFB(mce);
                    if(dtC.Rows.Count > 0)
                    {
                        if(dtC.Rows[0]["BankDayOff"].ToString() == "1")
                        {
                            bbl.ShowMessage("E157");
                            txtTransferDate.Focus();
                        }
                    }
                }
            }
        }

        private void cboProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboProcess.SelectedValue == null)
            //{
            //    bbl.ShowMessage("E102");
            //    cboProcess.Focus();
            //}CboSoukoType.SelectedValue.Equals("5")
            if (cboProcess.SelectedValue.Equals("0"))
            {
                Btn_F11.Text = "印刷(F11)";
                Btn_F12.Text = "出力(F12)";
            }
            else if (cboProcess.SelectedValue.Equals("1"))
            {
                Btn_F12.Text = "削除(F12)";
            }
            else
            {
                Btn_F11.Text = "印刷(F11)";
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F10();
        }

        
    }
}
