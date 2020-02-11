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
using Search;

namespace TempoRegiGenkaKakunin
{
    public partial class FrmTempoRegiGenkaKakunin : ShopBaseForm
    {
        TempoRegiGenkaKakunin_BL tempoRegiGenkaKakunin_bl=new TempoRegiGenkaKakunin_BL();  
        TempoRegiZaikoKakunin_Entity kne=new TempoRegiZaikoKakunin_Entity();
        string janCD = string.Empty;
        public FrmTempoRegiGenkaKakunin()
        {
            InitializeComponent();
        }
        private void FrmTempoRegiGenkaKakunin_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiGenkaKakunin";
            StartProgram();
            this.Text = "原価確認";
           // SetRequireField();
            txtJanCD.Focus();
            chkColorSize.Checked = false;
            BtnP_text = "決定";  
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

        private void btnInquiry_Click(object sender, EventArgs e)
        {
            Select_Data();
        }

        private void txtJanCD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            { 
                if (!string.IsNullOrWhiteSpace(txtJanCD.Text))
                {
                    DataTable dtSKU = SelectSKUData(txtJanCD.Text);
                    if(dtSKU.Rows.Count>0)
                    {
                        if (dtSKU.Rows.Count == 1)
                        {
                            txtJanCD.Text = dtSKU.Rows[0]["JanCD"].ToString();
                            lblItemName.Text = dtSKU.Rows[0]["SKUName"].ToString();
                            lblColorSize.Text = dtSKU.Rows[0]["ColorName"].ToString() + " . " + dtSKU.Rows[0]["SizeName"].ToString();
                            if (lblItemName.Width > 300)
                            {
                                lblItemName.MaximumSize = new Size(760, 50);
                            }
                            lblColorSize.Visible = true;
                            lblItemName.Visible = true;
                        } 
                        else
                        {
                            frmSearch_SKU frmsku = new frmSearch_SKU(txtJanCD.Text, dtSKU);
                            frmsku.ShowDialog();

                            if (!frmsku.flgCancel)
                            {
                                lblItemName.Text = frmsku.SKUName;
                                lblColorSize.Text = frmsku.SizeColorName;                               
                                lblColorSize.Visible = true;
                                lblItemName.Visible = true;
                            }                               

                        }
                        
                    }
                    else
                    {
                        tempoRegiGenkaKakunin_bl.ShowMessage("E101");
                        txtJanCD.Focus();
                    }
                }
                else
                {
                    tempoRegiGenkaKakunin_bl.ShowMessage("E102");
                    lblItemName.Text = string.Empty;
                    lblColorSize.Text = string.Empty;
                    txtJanCD.Focus();
                }
            }
        }

        private void GvSKU_DoubleClick(object sender, EventArgs e)
        {
            DataSending();
        }
   
        public DataTable SelectSKUData(string janCD)
        {
           return tempoRegiGenkaKakunin_bl.Select_M_SKU_Data(janCD);
        }

        public void DataSending()
        {
            if (GvSKU.CurrentRow != null && GvSKU.CurrentRow.Index >= 0)
            {
                //janCD = GvSKU.CurrentRow.Cells["colJanCD"].Value.ToString();
                janCD = txtJanCD.Text;
                this.Close();
            }
        }     
        
        private void Select_Data()
        {
            if (ErrorCheck())
            {
                kne.JanCD = txtJanCD.Text;
                kne.dataCheck = chkColorSize.Checked ? "1" : "0";
                kne.StoreCD = StoreCD;
                DataTable dt = new DataTable();
                dt = tempoRegiGenkaKakunin_bl.M_MakerVendor_DataSelect(kne);
                GvSKU.DataSource = dt;
            }
        }

        public bool ErrorCheck()
        {
            if (string.IsNullOrWhiteSpace(txtJanCD.Text))
            {
                tempoRegiGenkaKakunin_bl.ShowMessage("E102");
                txtJanCD.Focus();
                return false;
            }

            return true;
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void FrmTempoRegiGenkaKakunin_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }

}
