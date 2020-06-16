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
using CKM_Controls;

namespace TempoRegiKaiinTouroku_ShopForm
{
    public partial class Frm_TempoRegiKaiinTouroku_ShopForm : ShopBaseForm
    {
        TempoRegiKaiinTouroku_BL tprg_Kaiin_BL = new TempoRegiKaiinTouroku_BL();
        M_Customer_Entity cust = new M_Customer_Entity();
        M_ZipCode_Entity mze;
        public Frm_TempoRegiKaiinTouroku_ShopForm(int delflg ,string CustomerCD)
        {
            InitializeComponent();
            KeyUp += Form_KeyUp;

            if (delflg == 1)
            {
                lblCustomerNo.Text = CustomerCD;
                PanelDelete.Visible = false;
                rdoMale.Checked = true;
                chkSend.Checked = true;
            }               
              
            else
            {
                lblCustomerNo.Text = CustomerCD;
                DisplayData();
            }
                
        }
        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        private void TempoRegiKaiinTouroku_ShopForm_Load(object sender, EventArgs e)
        {            
            InProgramID = "TempoRegiKaiinTouroku";            
            StartProgram();
            SetRequireField();
            lblCustomerNo.Focus();
            
        }
        private void SetRequireField()
        {
            //txtkaiinNo.Require(true);
            txtFirstName.Require(true);
            txtLastName.Require(true);
            txtKanaName.Require(true);
            //必ず
            txtPostalCode.Require(true);
            txtPostalCode1.Require(true);
            txtAddress1.Require(true);
            txtAddress2.Require(true);

        }
        protected override void EndSec()
        {
            this.Close();
        }
        private bool ErrorCheck()
        {
            if(!RequireCheck(new Control[] { txtFirstName,txtLastName,txtKanaName}))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtTelNo1.Text) ||
                    !string.IsNullOrWhiteSpace(txtTelNo2.Text) ||
                    !string.IsNullOrWhiteSpace(txtTelNo3.Text)
                    )
            {
                if (!CompletePhoneNo(1))
                    return false;
            }

            if (!string.IsNullOrWhiteSpace(txthomeTelNo1.Text) ||
                     !string.IsNullOrWhiteSpace(txthomeTelNo2.Text) ||
                     !string.IsNullOrWhiteSpace(txthomeTelNo3.Text)
                     )
            {
                if(!CompletePhoneNo(2))
                return false;
            }
            else
            {
                if(!CompletePhoneNo(1))
                return false;
            }
            
            if(chkSend.Checked)
            {
                if (!RequireCheck(new Control[] { txtMailAddress, txtMailAddress2 }))
                {
                    return false;
                }
            }
            if (txtMailAddress.Text != txtMailAddress2.Text)
            {
                tprg_Kaiin_BL.ShowMessage("E174");
                txtMailAddress2.Focus();
                return false;
            }

            if (!RequireCheck(new Control[] {txtPostalCode,txtPostalCode1, txtAddress1, txtAddress2 }))
            {
                return false;
            }

            return true;
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
           if(ErrorCheck())
            {
                if (tprg_Kaiin_BL.ShowMessage("Q101") == DialogResult.Yes)
                {
                    cust = new M_Customer_Entity();

                    cust = GetTempoRegiKaiinToroku();

                    if (chkDelete.Visible == false)
                    {
                        InsertUpdate(1);

                    }
                    else if (chkDelete.Visible == true)
                    {
                        InsertUpdate(2);
                    }
                }
                else
                    //PreviousCtrl = this.ActiveControl;
                PreviousCtrl.Focus();

            }
        }
        private bool DisplayData()
        {           
            cust.CustomerCD = lblCustomerNo.Text;
            cust = tprg_Kaiin_BL.M_Customer_Select(cust);
            if(cust != null)
            {
                txtAddress1.Text = cust.CustomerCD;
                txtFirstName.Text = cust.FirstName;
                txtLastName.Text = cust.LastName;
                txtGroupName.Text = cust.GroupName;
                txtKanaName.Text = cust.KanaName;
                rdoMale.Checked = cust.Sex.Equals("1") ? true : false;
                rdoFemale.Checked = cust.Sex.Equals("2") ? true : false;
                txtBirthDate.Text = cust.Birthdate;
                txtTelNo1.Text = cust.TelephoneNo1;
                txtTelNo2.Text = cust.TelephoneNo2;
                txtTelNo3.Text = cust.TelephoneNo3;
                txthomeTelNo1.Text = cust.HomephoneNo1;
                txthomeTelNo2.Text = cust.HomephoneNo2;
                txthomeTelNo3.Text = cust.HomephoneNo3;
                txtMailAddress.Text = cust.MailAddress;
                txtMailAddress2.Text = cust.MailAddress2;
                txtPostalCode.Text = cust.ZipCD1;
                txtPostalCode1.Text = cust.ZipCD2;
                txtAddress1.Text = cust.Address1;
                txtAddress2.Text = cust.Address2;
                chkDelete.Checked = cust.DeleteFlg.Equals("1") ? true : false ;
                chkSend.Checked = cust.DMFlg.Equals("1") ? true : false ;
                txtFirstName.Focus();

                return true;
            }
            return false;
        }
        private M_Customer_Entity GetTempoRegiKaiinToroku()
        {
            cust = new M_Customer_Entity()
            {
                CustomerCD = lblCustomerNo.Text,
                StroeCD=StoreCD,
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                CustomerName = txtFirstName.Text + txtLastName.Text,
                LongName1 = txtFirstName.Text + txtLastName.Text,
                KanaName = txtKanaName.Text,
                StoreKBN = "2",
                CustomerKBN = "1",
                AliasKBN ="1",
                BillingType = "1",
                GroupName = txtGroupName.Text,
                BillingFLG = "1",
                CollectFLG = "1",
                BillingCD=lblCustomerNo.Text,
                CollectCD=lblCustomerNo.Text,
                Birthdate = txtBirthDate.Text,
                Sex = rdoMale.Checked ? "1" : "2",
                TelephoneNo1=txtTelNo1.Text,
                TelephoneNo2=txtTelNo2.Text,
                TelephoneNo3=txtTelNo3.Text,
                HomephoneNo1=txthomeTelNo1.Text,
                HomephoneNo2 =txthomeTelNo2.Text,
                HomephoneNo3=txthomeTelNo3.Text,
                ZipCD1=txtPostalCode.Text,
                ZipCD2=txtPostalCode1.Text,
                Address1=txtAddress1.Text,
                Address2=txtAddress2.Text,
                MailAddress = txtMailAddress.Text,
                MailAddress2=txtMailAddress2.Text,
                TankaCD= "0000000000000",
                PointFLG="1",
                MainStoreCD=StoreCD,
                StaffCD=InOperatorCD,
                BillingCloseDate="31",
                CollectPlanDate="31",
                TaxFractionKBN="1",
                AmountFractionKBN="1",
                PaymentUnit="2",
                DMFlg=chkSend.Checked? "1" : "0",
                DeleteFlg=chkDelete.Checked? "1" : "0",
                Operator=InOperatorCD,
                ProgramID = InProgramID,
                Key = lblCustomerNo.Text ,
                PC = InPcID,
            };

            return cust;
        }
        private void InsertUpdate(int mode)
        {

            if(tprg_Kaiin_BL.M_Customer_Insert_Update(cust,mode))
            {               
                EndSec();
                tprg_Kaiin_BL.ShowMessage("I101");
            }
            else
            {
                tprg_Kaiin_BL.ShowMessage("S001");
            }
        }
        private void btnZipCD_Click(object sender, EventArgs e)
        {                       
            if (string.IsNullOrEmpty(txtPostalCode.Text))
            {
                tprg_Kaiin_BL.ShowMessage("E102");
                txtPostalCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPostalCode1.Text))
            {
                tprg_Kaiin_BL.ShowMessage("E102");
                txtPostalCode1.Focus();
                return;
            }

            mze = new M_ZipCode_Entity();
            mze.ZipCD1 = txtPostalCode.Text;
            mze.ZipCD2 = txtPostalCode1.Text;
            if (CheckZipCD(mze))
                SetAddress();
        }
        private void txtZipCD_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                CKM_TextBox txt = sender as CKM_TextBox;
                if (!string.IsNullOrEmpty(txt.Text))
                {
                    if (CheckZipCD(txt))
                        SetAddress();
                        MoveNextControl(e);
                }
            }
        }
        private void SetAddress()
        {
            if (!string.IsNullOrWhiteSpace(txtPostalCode.Text) && !string.IsNullOrWhiteSpace(txtPostalCode1.Text))
            {
                mze = new M_ZipCode_Entity();
                mze.ZipCD1 = txtPostalCode.Text;
                mze.ZipCD2 = txtPostalCode1.Text;
                               
                DataTable dt = tprg_Kaiin_BL.M_ZipCode_AddressSelect(mze);

                if (dt.Rows.Count > 0)
                {
                    txtAddress1.Text = dt.Rows[0]["Address1"].ToString();
                    txtAddress2.Text = dt.Rows[0]["Address2"].ToString();
                }
            }
           
        }        
        private bool CheckZipCD(CKM_TextBox txt)
        {

            M_ZipCode_Entity mze = new M_ZipCode_Entity
            {
                ZipCD1 = (txt == txtPostalCode) ? txt.Text : string.Empty,
                ZipCD2 = (txt == txtPostalCode1) ? txt.Text : string.Empty
            };
            
            if (!tprg_Kaiin_BL.M_ZipCode_Select(mze))
            {
                tprg_Kaiin_BL.ShowMessage("E101");
                txt.Focus();
                txt.MoveNext = false;
                return false;
            }
            return true;
        }
        private bool CheckZipCD(M_ZipCode_Entity mze)
        {
            if (!tprg_Kaiin_BL.M_ZipCode_Select(mze))
            {
                tprg_Kaiin_BL.ShowMessage("E101");
                txtPostalCode.Focus();
                return false;
            }
            return true;
        }
        private void ConfirmMail_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keys.Enter==e.KeyCode)
            {
                CKM_TextBox txt = sender as CKM_TextBox;

                if(txtMailAddress.Text != txt.Text)
                {                   
                        tprg_Kaiin_BL.ShowMessage("E174");                             
                }
            }
        }
        private void TelephoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keys.Enter == e.KeyCode)
            {
                if (!string.IsNullOrWhiteSpace(txtTelNo1.Text) ||
                    !string.IsNullOrWhiteSpace(txtTelNo2.Text) ||
                    !string.IsNullOrWhiteSpace(txtTelNo3.Text)
                    )
                    CompletePhoneNo(1);
            }
        }
        private bool CompletePhoneNo(int type)
        {
            switch(type)
            {
                case 1:
                    //Array arr[] = new Array { txtTelNo1, txtTelNo2 };
                    Array arrTel = new TextBox[] { txtTelNo1,txtTelNo2,txtTelNo3 };

                    foreach(TextBox txt in arrTel)
                    {
                        if(string.IsNullOrWhiteSpace(txt.Text))
                        {
                            tprg_Kaiin_BL.ShowMessage("E102");
                            txt.Focus();
                            return false;
                        }
                    }
                    return true;
                case 2:
                    Array arrHome = new TextBox[] { txthomeTelNo1, txthomeTelNo2, txthomeTelNo3 };
                    foreach (TextBox txt in arrHome)
                    {
                        if (string.IsNullOrWhiteSpace(txt.Text))
                        {
                            tprg_Kaiin_BL.ShowMessage("E102");
                            txt.Focus();
                            return false;
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }
        private void HomePhoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keys.Enter==e.KeyCode)
            {
                if (!string.IsNullOrWhiteSpace(txthomeTelNo1.Text) ||
                    !string.IsNullOrWhiteSpace(txthomeTelNo2.Text) ||
                    !string.IsNullOrWhiteSpace(txthomeTelNo3.Text)
                    )
                    CompletePhoneNo(2);
                else
                {
                    CompletePhoneNo(1);
                }
            }
        }
    }
}
