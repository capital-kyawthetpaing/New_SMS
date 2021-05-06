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

        private const int GYO_CNT = 8;
        DataTable dt = new DataTable();

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
            AddHandler();

            //if (!string.IsNullOrWhiteSpace(JanCD))
            //{
            //    txtJanCD.Text = JanCD;
            //}
            Clear(pnlDetails);

            this.Text = "在庫確認";
            txtbin.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtJanCD.Text = string.Empty;
            txtbin.Focus();
            //SetRequireField();
            chkColorSize.Checked = false;
            BtnP_text = "決　定";

            string[] cmds = System.Environment.GetCommandLineArgs();
            if (cmds.Length - 1 > (int)ECmdLine.PcID)
            {
                string shiireNO = cmds[(int)ECmdLine.PcID + 1];   //
                //ChangeOperationMode(EOperationMode.UPDATE);
                txtJanCD.Text = shiireNO;
                // CheckKey((int)EIndex.PurchaseNO, true);
            }
            //DataGridViewColumn column = dgvZaikokakunin.Columns[-1];
            //column.Width = 70;
         

        }

        private void AddHandler()
        {
            for (int i = 1; i <= GYO_CNT; i++)
            {
                //lblDtGyoをさがす。子コントロールも検索する。
                Control[] cs = this.Controls.Find("lblDtGyo" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Click += new System.EventHandler(this.lblDtGyo_Click);
                }
            }
        }

        private void SetRequireField()
        {
            //txtJanCD.Require(true);
        }

        private void lblDtGyo_Click(object sender, EventArgs e)
        {
            try
            {
                Label lblsender = (Label)sender;

                if (string.IsNullOrWhiteSpace(lblsender.Text))
                    return;

                int index = Convert.ToInt16(lblsender.Text) - 1;

                //選択行のindexを保持
                btnDown.Tag = index;

                ClearBackColor(pnlDetails);

                ChangeBackColor(lblsender);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearBackColor(Panel panel)
        {
            IEnumerable<Control> c = GetAllControls(panel);
            foreach (Control ctrl in c)
            {
                if (ctrl is Label)
                    ((Label)ctrl).BackColor = Color.Transparent;
                if (ctrl is Panel)
                    ((Panel)ctrl).BackColor = Color.Transparent;

            }
        }

        private void ChangeBackColor(Label lblsender)
        {
            Color backCl = Color.FromArgb(255, 242, 204);

            lblsender.BackColor = backCl;

            int i = Convert.ToInt16(lblsender.Name.Substring(8));

            Control[] cs = this.Controls.Find("lblGyoSelect" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblWarehouse" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblProductName" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }

            cs = this.Controls.Find("lblProductNum" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }

            cs = this.Controls.Find("lblJANCD" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblColorSize" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;

            }
            cs = this.Controls.Find("lblStockDate" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblQuantity" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }

            cs = this.Controls.Find("lblPossibleNum" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }        
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

        private void Clear(Panel panel)
        {
            IEnumerable<Control> c = GetAllControls(panel);
            foreach (Control ctrl in c)
            {
                if (ctrl is Label)
                    ((Label)ctrl).Text = string.Empty;

                if (ctrl is Button)
                    ((Button)ctrl).Text = string.Empty;

                ctrl.Tag = "";
            }
        }

        public void DataSending()
        {
            //if (dgvZaikokakunin.CurrentRow != null && dgvZaikokakunin.CurrentRow.Index >= 0)
            //{
            //    //  data = dgvZaikokakunin.CurrentRow.Cells["colProduct"].Value.ToString();
            //    data = txtJanCD.Text;
            //    this.Close();
            //}23.4.2021
        }

        public bool ErrorCheck()
        {
            //if (string.IsNullOrWhiteSpace(txtJanCD.Text))
            //{
            //    zaikobl.ShowMessage("E102");
            //    txtJanCD.Focus();
            //    return false;
            //}
            //else
            //{
            //    kne.JanCD = txtJanCD.Text;
            //    DataTable dtname = new DataTable();

            //}
            if(string.IsNullOrWhiteSpace(txtbin.Text) && string.IsNullOrWhiteSpace(txtProductName.Text) &&
                string.IsNullOrWhiteSpace(txtJanCD.Text))
            {
                zaikobl.ShowMessage("E111");
                txtJanCD.Focus();
                return false;
            }

            return true;
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void btnInquery_Click(object sender, EventArgs e)
        {
            //SelectData();
            if (string.IsNullOrWhiteSpace(txtbin.Text) && string.IsNullOrWhiteSpace(txtProductName.Text) &&
               string.IsNullOrWhiteSpace(txtJanCD.Text))
            {
                zaikobl.ShowMessage("E111");
                txtJanCD.Focus();
            }
            else if (!string.IsNullOrWhiteSpace(txtJanCD.Text))
            {
                DataTable dtSKU = SelectSKUData(txtJanCD.Text);
                if (dtSKU.Rows.Count > 0)
                {

                    if (dtSKU.Rows.Count == 1)
                    {
                        txtJanCD.Text = dtSKU.Rows[0]["JanCD"].ToString();
                        //lblItemName.Text = dtSKU.Rows[0]["SKUName"].ToString();
                        //lblColorSize.Text = dtSKU.Rows[0]["ColorName"].ToString() + " . " + dtSKU.Rows[0]["SizeName"].ToString();
                        //lblColorSize.Visible = true;
                        //lblItemName.Visible = true;
                    }
                    else
                    {

                        frmSearch_SKU frmsku = new frmSearch_SKU(txtJanCD.Text, dtSKU);
                        frmsku.ShowDialog();
                        if (!frmsku.flgCancel)
                        {
                            //lblItemName.Text = frmsku.SKUName;
                            //lblColorSize.Text = frmsku.SizeColorName;
                            //lblColorSize.Visible = true;
                            //lblItemName.Visible = true;
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
            //else
            //{
            //    zaikobl.ShowMessage("E102");
            //    //lblItemName.Text = string.Empty;
            //    //lblColorSize.Text = string.Empty;
            //    txtJanCD.Focus();

            //}
        }

        private void SelectData()
        {
            if (ErrorCheck())
            {
                kne.JanCD = txtJanCD.Text;
                kne.dataCheck = chkColorSize.Checked ? "1" : "0";
                kne.Operator = InOperatorCD;
                kne.ItemCD = txtbin.Text;
                kne.SKUName = txtProductName.Text;
                
                dt = zaikobl.D_Stock_DataSelect(kne);
                if(dt.Rows.Count > 0)
                {
                    //dgvZaikokakunin.DataSource = dt;23.4.2021
                    DispFromDataTable();
                }
                else
                {
                    //dgvZaikokakunin.DataSource = string.Empty;23.4.2021
                    Clear(pnlDetails);
                    zaikobl.ShowMessage("E128");
                    txtJanCD.Focus();
                }
                          
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
                            txtProductName.Text = dtSKU.Rows[0]["SKUName"].ToString();
                            //lblItemName.Text = dtSKU.Rows[0]["SKUName"].ToString();
                            //lblColorSize.Text = dtSKU.Rows[0]["ColorName"].ToString() + " . " + dtSKU.Rows[0]["SizeName"].ToString();
                            //lblColorSize.Visible = true;
                            //lblItemName.Visible = true;
                        }                          
                        else
                        {
                            
                            frmSearch_SKU frmsku = new frmSearch_SKU(txtJanCD.Text, dtSKU);
                            frmsku.ShowDialog();
                            if(!frmsku.flgCancel)
                            {
                                txtProductName.Text = frmsku.SKUName;
                                //lblItemName.Text = frmsku.SKUName;
                                //lblColorSize.Text = frmsku.SizeColorName;
                                //lblColorSize.Visible = true;
                                //lblItemName.Visible = true;
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
                    if (string.IsNullOrWhiteSpace(txtbin.Text) && string.IsNullOrWhiteSpace(txtProductName.Text))
                    {
                        zaikobl.ShowMessage("E111");
                        txtJanCD.Focus();
                    }
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

        private void btnProductName_Click(object sender, EventArgs e)
        {
            TempoRegiShouhinKensaku frmshouhin = new TempoRegiShouhinKensaku(InOperatorCD);
            frmshouhin.ShowDialog();
            txtJanCD.Text = frmshouhin.JANCD;
            txtProductName.Text = frmshouhin.SKUName;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                int gyoNo = Convert.ToInt16(lblDtGyo1.Text);
                if (gyoNo - GYO_CNT > 0)
                    DispFromDataTable(gyoNo - GYO_CNT);
                else
                    DispFromDataTable();

                int index = Convert.ToInt16(btnDown.Tag);
                if (index - GYO_CNT >= 0)
                {
                    btnDown.Tag = index - GYO_CNT;
                }
                else
                {
                    btnDown.Tag = 0;
                    ChangeBackColor(lblDtGyo1);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                int gyoNo = Convert.ToInt16(lblDtGyo1.Text);
                if (dt.Rows.Count >= gyoNo + GYO_CNT)
                {
                    DispFromDataTable(gyoNo + GYO_CNT);

                    int index = Convert.ToInt16(btnDown.Tag);
                    if (dt.Rows.Count > index + GYO_CNT)
                    {
                        btnDown.Tag = index + GYO_CNT;
                    }
                    else
                    {
                        btnDown.Tag = gyoNo + GYO_CNT - 1;

                        ChangeBackColor(lblDtGyo1);
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void DispFromDataTable(int gyoNo = 1)
        {

            Clear(pnlDetails);

            for (int i = 1; i <= GYO_CNT; i++)
            {
                int index = gyoNo - 2 + i;

                if (dt.Rows.Count <= index)
                    break;

                DataRow row = dt.Rows[index];

                //lblDtGyoをさがす。子コントロールも検索する。
                Control[] cs = this.Controls.Find("lblDtGyo" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = (index + 1).ToString();
                }

                cs = this.Controls.Find("lblWarehouse" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["SoukoName"].ToString();                  
                }
                cs = this.Controls.Find("lblProductNum" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["ItemCD"].ToString();
                }
                cs = this.Controls.Find("lblProductName" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["SKUName"].ToString();
                }
                cs = this.Controls.Find("lblJANCD" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["JanCD"].ToString();

                }
                cs = this.Controls.Find("lblColorSize" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["ColorSize"].ToString();                  
                }
                cs = this.Controls.Find("lblStockDate" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["StockDate"].ToString();
                }

                cs = this.Controls.Find("lblQuantity" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["StockNum"].ToString();
                }

                cs = this.Controls.Find("lblPossibleNum" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["KanoSu"].ToString();
                }
               
            }
        }

    }
}
