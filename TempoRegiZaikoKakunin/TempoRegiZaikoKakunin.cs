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
using Search;

namespace TempoRegiZaikoKakunin
{
    public partial class frmTempoRegiZaikoKakunin : ShopBaseForm
    {
        TempoRegiZaikoKakunin_BL zaikobl = new TempoRegiZaikoKakunin_BL();
        TempoRegiZaikoKakunin_Entity kne = new TempoRegiZaikoKakunin_Entity();

        public string data = string.Empty;
        public string CompanyCD = string.Empty;
        public string JanCD = string.Empty;
        public string PcID = string.Empty;
        public string OperatorCD = string.Empty;


        public frmTempoRegiZaikoKakunin()
        {
            InitializeComponent();
        }
        
        private void frmTempoRegiZaikoKakunin_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiZaikoKakunin";
            
            InPcID = PcID;
            InOperatorCD = OperatorCD;
            InCompanyCD = CompanyCD;
            string data = InOperatorCD;
            StartProgram();

            //if (!string.IsNullOrWhiteSpace(JanCD))
            //{
            //    txtJanCD.Text = JanCD;
            //}
            this.Text = "在庫確認";
            txtJanCD.Focus();
            //SetRequireField();
            chkColorSize.Checked = false;
            BtnP_text = "決定";

            string[] cmds = System.Environment.GetCommandLineArgs();
            if (cmds.Length - 1 > (int)ECmdLine.PcID)
            {
                string shiireNO = cmds[(int)ECmdLine.PcID + 1];   //
                //ChangeOperationMode(EOperationMode.UPDATE);
                txtJanCD.Text = shiireNO;
               // CheckKey((int)EIndex.PurchaseNO, true);
            }


        }

        private void SetRequireField()
        {
            txtJanCD.Require(true);
        }

        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:                   
                    DataSending();
                    break;
            }
        }

        public void DataSending()
        {
            if (dgvZaikokakunin.CurrentRow != null && dgvZaikokakunin.CurrentRow.Index >= 0)
            {
                //  data = dgvZaikokakunin.CurrentRow.Cells["colProduct"].Value.ToString();
                data = txtJanCD.Text;
                this.Close();
            }
        }

        public bool ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(txtJanCD.Text))
            {
                zaikobl.ShowMessage("E102");
                txtJanCD.Focus();
                return false;
            }
            //else
            //{
            //    kne.JanCD = txtJanCD.Text;
            //    DataTable dtname = new DataTable();

            //}
            
            return true;
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void btnInquery_Click(object sender, EventArgs e)
        {
            SelectData();
        }

        private void SelectData()
        {
            if (ErrorCheck())
            {
                kne.JanCD = txtJanCD.Text;
                kne.dataCheck = chkColorSize.Checked ? "1" : "0";
                kne.Operator = InOperatorCD;
                DataTable dt = new DataTable();
                dt = zaikobl.D_Stock_DataSelect(kne);
                dgvZaikokakunin.DataSource = dt;
                
            }
        }
        private void txtJanCD_KeyDown(object sender, KeyEventArgs e)
        {
            zaikobl = new TempoRegiZaikoKakunin_BL();
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtJanCD.Text))
                {
                    DataTable dtSKU = SelectSKUData(txtJanCD.Text);
                    if (dtSKU.Rows.Count > 0)
                    {
                        
                        if (dtSKU.Rows.Count == 1)
                        {
                            txtJanCD.Text = dtSKU.Rows[0]["JanCD"].ToString();
                            lblItemName.Text = dtSKU.Rows[0]["SKUName"].ToString();
                            lblColorSize.Text = dtSKU.Rows[0]["ColorName"].ToString() + " . " + dtSKU.Rows[0]["SizeName"].ToString();
                            lblColorSize.Visible = true;
                            lblItemName.Visible = true;
                        }                          
                        else
                        {
                            
                            frmSearch_SKU frmsku = new frmSearch_SKU(txtJanCD.Text, dtSKU);
                            frmsku.ShowDialog();
                            if(!frmsku.flgCancel)
                            {
                                lblItemName.Text = frmsku.SKUName;
                                lblColorSize.Text = frmsku.SizeColorName;
                                lblColorSize.Visible = true;
                                lblItemName.Visible = true;
                            }                           
                        }
                        SelectData();
                    }
                    else
                    {
                        zaikobl.ShowMessage("E101");
                        txtJanCD.Focus();                        
                    }
                }
                else
                {
                    zaikobl.ShowMessage("E102");
                    lblItemName.Text = string.Empty;
                    lblColorSize.Text = string.Empty;
                    txtJanCD.Focus();
                    
                }
            }
        }

        public DataTable SelectSKUData(string janCD)
        {
            zaikobl = new TempoRegiZaikoKakunin_BL();
            return zaikobl.Select_M_SKU_Data(janCD);
        }

        private void frmTempoRegiZaikoKakunin_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        //private void ckM_Button1_Click(object sender, EventArgs e)
        //{
        //    TempoRegiZaikoKakunin_Search form = new TempoRegiZaikoKakunin_Search();
        //    form.ShowDialog();
        //}

        private void dgvZaikokakunin_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataSending();
        }
    }
}
