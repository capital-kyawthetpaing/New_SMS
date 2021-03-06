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
using Search;
using System.Collections;
using System.Data.Linq;
using DL;
using System.Data.SqlClient;

namespace TempoRegi_Master_Nyuuryoku
{
    public partial class TempoRegiMasterNyuuryoku : ShopBaseForm
    {
        M_StoreBottunDetails_Entity sbgd_e;
        TempoRegiMasterNyuuryoku_BL mnrk_bl;
        DataTable dtResult, dtSelect, dtCpy, dtTemp1, dtTemp2;
        String[] buttonarr = new String[100];
        string groupno = "";
        string horizontal;
        string vertical;
        string radiovalue = "";
        string btnNameDetails =string.Empty;
        string BtnName = string.Empty;
        string cd = string.Empty;
        Button btn1 = null, btn2 = null;
        public TempoRegiMasterNyuuryoku()
        {
            mnrk_bl = new TempoRegiMasterNyuuryoku_BL();
            sbgd_e = new M_StoreBottunDetails_Entity();
            dtSelect = new DataTable();
            InitializeComponent();
        }

        private void TempoRegiMasterNyuuryoku_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiMasterNyuuryoku";
            StartProgram();
            this.Text = "マスター入力";
            // SetRequireField();
            GridViewDataBind();
            btnProcess.Enabled = false;
            txtButtomNameUp.Focus();
            //lblSearchName.TextAlign = ContentAlignment.TopLeft;
        }

        private void TempoRegiMasterNyuuryoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
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

            if (mnrk_bl.ShowMessage("Q101") == DialogResult.Yes)
            {

                sbgd_e = GetGroupDetailsData();
                if (mnrk_bl.Button_Details_Insert_Update(sbgd_e))//更新
                {
                    GridViewDataBind();
                    SaveClear();
                    bbl.ShowMessage("I101");
                }
                else
                {
                    bbl.ShowMessage("S001");
                }
            }
            

        }

        private bool ButtonNO_Check()
        {
            if (lblGroupNO.Text.Equals("Btn1_No"))
            {
                //MessageBox.Show("ボタンをお選択してください！", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bbl.ShowMessage("E236");
                txtButtomNameUp.Focus();
                return false; ;
            }
            else if (lblNameNO.Text.Equals(""))
            {
                bbl.ShowMessage("E236");
                txtCD.Focus();
                return false;
            }
            else
                return true;
        }
        protected override void EndSec()
        {
            this.Close();
        }

        private void SaveClear()
        {

            txtButtomNameUp.Text = string.Empty;
            txtBtnNameDown.Text = string.Empty;
            txtCD.Text = string.Empty;
            RdoJanCD.Checked = true;
            RdoJanCD.Enabled = true;
            RdoCustomerCD.Checked = false;
            RdoCustomerCD.Enabled = true;
            lblGroupNO.Text = "";
            lblNameNO.Text = "";
            lblSearchName.Text = "";
            lblAdminNO.Text = "";
            panel2.Refresh();
            for (int k = 1; k <= 99; k++)
            {
                for (int j = 1; j < 3; j++)
                {
                    Button btn2 = this.Controls.Find("btnName" + k.ToString() + j.ToString(), true)[0] as Button;

                    btn2.Text = string.Empty;
                }

            }
        }

        //private void BtnResult(DataTable db, DataTable Temp)
        //{
        //    List 
        //}
        private void buttonName_Click(object sender, EventArgs e)
        {
            ClearData();
            Button btn = (Button)sender;
            /// btn.TabIndex.
            string ctrlName = ((Control)sender).Name;
            groupno = ctrlName.Substring(10);
            lblGroupNO.Visible = true;
            lblGroupNO.Text = groupno + "番目";

            for (int k = 1; k <= 99; k++)
            {
                for (int j = 1; j < 3; j++)
                {
                    Button btn2 = this.Controls.Find("btnName" + k.ToString() + j.ToString(), true)[0] as Button;

                    btn2.Text = string.Empty;
                }

            }

            if (!string.IsNullOrEmpty(btn.Text))
            {
                string str = btn.Text.ToString();
                txtButtomNameUp.Text = btn.Text;
                DataRow[] drBtnName = dtSelect.Select("(btndetailBottunName='' OR btndetailBottunName is null) AND ButtomName=" + "'" + str + "'" + "");
                DataRow[] dr = dtTemp1.Select("GroupNO=" + "" + groupno + "");
                DataRow[] dr2 = dtTemp2.Select("GroupNO=" + "" + groupno + "");
                
                if(drBtnName.Count()==dtSelect.Rows.Count)
                {
                    RdoJanCD.Checked = true;
                    RdoJanCD.Enabled = true;
                    RdoCustomerCD.Enabled = true;
                }
                else
                {
                    if (dr.Count() > 0)
                    {
                        radiovalue = dr[0]["MasterKBN"].ToString();

                        if (radiovalue == "1")
                        {
                            RdoJanCD.Checked = true;
                            RdoJanCD.Enabled = true;
                            RdoCustomerCD.Enabled = false;
                        }
                        else
                        {
                            RdoCustomerCD.Checked = true;
                            RdoCustomerCD.Enabled = true;
                            RdoJanCD.Enabled = false;
                        }
                    }
                }

                
                foreach (DataRow row in dr2)
                {
                    horizontal = row["Horizontal"].ToString();
                    vertical = row["Vertical"].ToString();

                    btn2 = this.Controls.Find("btnName" + horizontal + vertical, true)[0] as Button;
                    btn2.Text = row["btndetailBottunName"].ToString();

                }
            }
            else
            {
                txtButtomNameUp.Focus();
                RdoJanCD.Checked = true;
                RdoJanCD.Enabled = true;
                RdoCustomerCD.Enabled = true;
            }
        }
        private M_StoreBottunDetails_Entity GetGroupDetailsData()
        {
            sbgd_e = new M_StoreBottunDetails_Entity()
            {
                StoreCD = StoreCD,
               // MasterKBN = RdoJanCD.Checked ? "1" : "2",
                dtGroup = dtTemp1,
                dtGpDetails = dtTemp2,
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID,
               // Key = txtButtomNameUp.Text
            };
            return sbgd_e;
        }

        private void SetRequireField()
        {
            //txtButtomNameUp.Require(true);
            txtCD.Require(true);
            //txtBtnNameDown.Require(true);

        }

        private bool ErrorCheck(int type)
        {
            #region NO use
            //if (!RequireCheck(new Control[] { txtButtomNameUp, txtCD }))   // go that focus
            //    return false;

            //if (RdoJanCD.Checked)
            //{
            //    DataTable dtJanCD = new DataTable();
            //    dtJanCD = SelectSKUData(txtCD.Text);
            //    if (dtJanCD.Rows.Count == 0)
            //    {
            //        mnrk_bl.ShowMessage("E101");
            //        txtCD.Focus();
            //        return false;
            //    }
            //}

            //if (RdoCustomerCD.Checked)
            //{
            //    DataTable dtCustCD = new DataTable();
            //    dtCustCD = SelectCustomerData(txtCD.Text);
            //    if (dtCustCD.Rows.Count == 0)
            //    {
            //        mnrk_bl.ShowMessage("E101");
            //        txtCD.Focus();
            //        return false;
            //    }
            //}
            //if (!RequireCheck(new Control[] { txtBtnNameDown }))
            //    return false;

            //ButtonNO_Check
            #endregion

            if(type==1)
            {
                //ButtonNO_Check
                if (lblGroupNO.Text.Equals("Btn1_No"))
                {
                    bbl.ShowMessage("E236");
                    txtButtomNameUp.Focus();
                    return false;
                }

            }

            if(type==2)
            {
                if (lblNameNO.Text.Equals("Btn2_No") || lblNameNO.Text.Equals(""))
                {
                    bbl.ShowMessage("E236");
                    txtCD.Focus();
                    return false;
                }

                if(!string.IsNullOrWhiteSpace(txtCD.Text))
                {
                    if(RdoJanCD.Checked)
                    {
                        DataTable dtSKU = SelectSKUData(txtCD.Text);
                        if (dtSKU.Rows.Count <= 0)
                        {
                            bbl.ShowMessage("E101");
                            txtCD.Focus();
                            return false;
                        }
                    }

                    if (RdoCustomerCD.Checked)
                    {
                        DataTable dtCusto = SelectCustomerData(txtCD.Text);
                        if (dtCusto.Rows.Count <= 0)
                        {
                            bbl.ShowMessage("E101");
                            txtCD.Focus();
                            return false;
                        }

                        else
                        {
                            DataRow[] dr = dtCusto.Select("DeleteFlg=1");
                            if (dr.Count() > 0)
                            {
                                bbl.ShowMessage("E119");
                                txtCD.Focus();
                                return false;
                            }
                        }
                    }
                }
                
                if (!string.IsNullOrWhiteSpace(txtCD.Text) && string.IsNullOrWhiteSpace(txtBtnNameDown.Text))
                {
                    bbl.ShowMessage("E102");
                    txtBtnNameDown.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtCD.Text) && !string.IsNullOrWhiteSpace(txtBtnNameDown.Text))
                {
                    bbl.ShowMessage("E102");
                    txtCD.Focus();
                    return false;
                }
            }


            return true;
        }

        private void GridViewDataBind()
        {
            List<Button> buttongroup = new List<Button>();
            Button btn = null;
            dtSelect = dtResult = mnrk_bl.TempoRegiMasterNyuuryoku_Grid_SelectAll(StoreCD);//M_StoreBottunGroup_Select
            dtCpy = new DataTable();
            dtCpy = dtSelect.DefaultView.ToTable(true, "ButtomName", "GroupNO", "MasterKBN");
            dtTemp1 = dtSelect.DefaultView.ToTable(true, "GroupNO", "ButtomName", "MasterKBN");
            dtTemp2 = dtSelect.DefaultView.ToTable(true, "GroupNO", "MasterKBN", "Vertical", "Horizontal", "btndetailBottunName", "Button","AdminNO","JanCD","CustomerCD");
            RemoveNullColumnFromDataTable(dtTemp2);
            for (int k = 0; k < 100; k++)
            {
                buttonarr[k] = (k + 1).ToString();
            }
            //<Remark>FirstGridviewDisplayData</Reamark>
            if (dtCpy.Rows.Count > 0)
            {
                for (int i = 0; i < dtCpy.Rows.Count; i++)
                {
                    for (int j = 0; j < buttonarr.Length; j++)
                    {
                        if (dtCpy.Rows[i]["GroupNO"].ToString() == buttonarr[j])
                        {
                            btn = this.Controls.Find("buttonName" + (j + 1), true)[0] as Button;//buttonName(1,2,3...)coding
                            btn.Text = dtCpy.Rows[i]["ButtomName"].ToString();//firstGridview_Btn Input 
                        }
                    }
                }
            }

        }
        public static void RemoveNullColumnFromDataTable(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i]["Vertical"] == DBNull.Value && (dt.Rows[i]["Horizontal"] == DBNull.Value))
                    dt.Rows[i].Delete();
            }
            dt.AcceptChanges();
        }
        private void btnDetails_Click(object sender, EventArgs e)
        {

            Button btn = (Button)sender;
            string ctrlName1 = ((Control)sender).Name;
            horizontal = ctrlName1.Substring(7, ctrlName1.Length - 8);
            vertical = ctrlName1.Substring(ctrlName1.Length - 1);
            lblNameNO.Visible = true;
            lblNameNO.Text = string.Empty;
            cd = string.Empty;
            if (lblGroupNO.Text.Equals("Btn1_No")) return;
            groupno = lblGroupNO.Text.Replace("番目", "");

            DataRow[] results = dtTemp2.Select("Horizontal = " + "'" + horizontal + "'" + " AND Vertical = " + "'" + vertical + "'" + " AND GroupNO = " + "'" + groupno + "'");

            if (!string.IsNullOrWhiteSpace(btn.Text))
            {

                txtBtnNameDown.Text = btn.Text;

                if (results.Count() > 0)
                {
                    if (results[0]["Vertical"].ToString() == "1")
                    {
                        lblNameNO.Text = results[0]["Horizontal"].ToString() + "番目の上";
                    }
                    else
                    {
                        lblNameNO.Text = results[0]["Horizontal"].ToString() + "番目の下";
                    } 

                    txtCD.Text = cd =results[0]["Button"].ToString();
                }

                foreach (DataRow row in results)
                {

                    btn2 = this.Controls.Find("btnName" + row["Horizontal"].ToString() + row["Vertical"].ToString(), true)[0] as Button;

                    btn2.Text = row["btndetailBottunName"].ToString();
                }

                DisplayName();

            }
            else
            {
                if (vertical.Equals("1"))
                    lblNameNO.Text = horizontal + "番目の上";
                else
                    lblNameNO.Text = horizontal + "番目の下";

                txtCD.Text = string.Empty;
                txtBtnNameDown.Text = string.Empty;
                lblSearchName.Text = string.Empty;
                txtCD.Focus();
            }
        }

        private void SearchName_Click(object sender, EventArgs e)
        {
            //SearchData();
            SearchDate_ByOption();
        }

        public DataTable SelectSKUData(string janCD)
        {
            return mnrk_bl.Select_M_SKU_Data(janCD);
        }

        private void txtButtomNameUp_TextChanged(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;
        }

        private void txtBtnNameDown_TextChanged(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;
        }

        private void txtCD_TextChanged(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;
        }

        public DataTable SelectCustomerData(string janCD)
        {
            return mnrk_bl.Select_M_Customer_Data(janCD);
        }

        private void ClearData()
        {
            lblGroupNO.Visible = true;
            lblGroupNO.Text = string.Empty;
            lblNameNO.Visible = true;
            lblNameNO.Text = string.Empty;
            txtButtomNameUp.Clear();
            txtBtnNameDown.Text = string.Empty;
            txtCD.Text = string.Empty;
            lblSearchName.Text = string.Empty;

            for (int k = 1; k < 99; k++)
            {
                for (int j = 1; j < 3; j++)
                {
                    Button btn2 = this.Controls.Find("btnName" + k.ToString() + j.ToString(), true)[0] as Button;
                    btn2.Text = string.Empty;
                }

            }
        }

        private void txtCD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchData();
                
            }
        }

        private void SearchDate_ByOption()
        {
            string ymd = bbl.GetDate();
            if (RdoJanCD.Checked)
            {
                TempoRegiShouhinKensaku tpoShouhin = new TempoRegiShouhinKensaku(InOperatorCD);
                tpoShouhin.ShowDialog();
                if(!string.IsNullOrWhiteSpace(tpoShouhin.JANCD))
                {
                    txtCD.Text = tpoShouhin.JANCD;
                    lblAdminNO.Text = tpoShouhin.AdminNO;
                    lblSearchName.Text = tpoShouhin.SKUName;
                }

            }
            else
            {
                TempoRegiKaiinKensaku tgkkk = new TempoRegiKaiinKensaku();
                tgkkk.ShowDialog();

                if (!string.IsNullOrEmpty(tgkkk.CustomerCD))
                {
                    txtCD.Text = tgkkk.CustomerCD;
                    lblSearchName.Text = tgkkk.CustomerName;
                    lblAdminNO.Text = string.Empty;
                }

            }
            lblSearchName.Visible = true;
            txtBtnNameDown.Focus();
        }

        private void SearchData()
        {
            if (!string.IsNullOrWhiteSpace(txtCD.Text))
            {
                if (RdoJanCD.Checked)
                {
                    DataTable dtSKU = SelectSKUData(txtCD.Text);
                    if (dtSKU.Rows.Count > 0)
                    {
                        if (dtSKU.Rows.Count == 1)
                        {
                            txtCD.Text = dtSKU.Rows[0]["JanCD"].ToString();
                            lblAdminNO.Text = dtSKU.Rows[0]["AdminNO"].ToString();
                            lblSearchName.Text =dtSKU.Rows[0]["SKUName"].ToString();
                        }
                        else
                        {
                            frmSearch_SKU frmsku = new frmSearch_SKU(txtCD.Text, dtSKU);
                            frmsku.ShowDialog();
                            if (!frmsku.flgCancel)
                            {
                                lblAdminNO.Text = frmsku.AdminNO;
                                lblSearchName.Text = frmsku.SKUName;
                            }

                        }
                        lblSearchName.Visible = true;
                        txtBtnNameDown.Focus();
                    }
                    else
                    {
                        lblSearchName.Text = string.Empty;
                        lblAdminNO.Text = string.Empty;
                        mnrk_bl.ShowMessage("E101");
                        txtCD.Focus();
                    }



                }
                else if (RdoCustomerCD.Checked)
                {
                    DataTable dtCusto = SelectCustomerData(txtCD.Text);
                    if (dtCusto.Rows.Count > 0)
                    {
                        DataRow[] dr = dtCusto.Select("DeleteFlg=1");
                        if(dr.Count()==0)
                        {
                            if (dtCusto.Rows.Count == 1)
                            {
                                txtCD.Text = dtCusto.Rows[0]["CustomerCD"].ToString();
                                lblSearchName.Text =dtCusto.Rows[0]["CustomerName"].ToString();
                            }
                            lblSearchName.Visible = true;
                            lblAdminNO.Text = string.Empty;
                            txtBtnNameDown.Focus();
                        }
                        else
                        {
                            lblSearchName.Text = "";
                            lblAdminNO.Text = string.Empty;
                            mnrk_bl.ShowMessage("E119");
                            txtCD.Focus();
                        }
                        

                    }
                    else
                    {
                        lblSearchName.Text = "";
                        lblAdminNO.Text = string.Empty;
                        mnrk_bl.ShowMessage("E101");
                        txtCD.Focus();
                    }
                }


                if (!cd.Equals(txtCD.Text.ToString()))
                {
                    cd = txtCD.Text.ToString();
                    string skuName = lblSearchName.Text.ToString();
                    int len = Convert.ToInt32(Encoding.GetEncoding(932).GetByteCount(skuName));
                    while (len > 18)
                    {
                        skuName = skuName.Remove(skuName.Length - 1, 1);
                        len = Convert.ToInt32(Encoding.GetEncoding(932).GetByteCount(skuName));
                    }
                    txtBtnNameDown.Text = skuName;

                }

            }
            else
            {
                lblSearchName.Text =string.Empty;
                lblAdminNO.Text = string.Empty;
                txtBtnNameDown.Focus();
            }
        }

        private void btn_Confrim2_Click(object sender, EventArgs e)
        {
            
            if (ErrorCheck(2))
            {
                btnProcess.Enabled = true;//Save_Button

                //Button btn2;
                BtnName = txtBtnNameDown.Text.Trim();
                groupno = lblGroupNO.Text.Replace("番目", "");
                horizontal = lblNameNO.Text.Replace("番目の上", "").Replace("番目の下", "");
                DataRow[] results = dtTemp2.Select("GroupNo = " + "'" + groupno + "'");
                if (lblNameNO.Text.Contains("上"))
                {
                    vertical = "1";
                }

                else if (lblNameNO.Text.Contains("下"))
                {
                    vertical = "2";
                }

                if (dtTemp2 != null)
                {
                    for (int i = 0; i < dtTemp2.Rows.Count; i++)
                    {
                        DataRow dr = dtTemp2.Rows[i];
                        if (dr["GroupNO"].ToString() == groupno && dr["Horizontal"].ToString() == horizontal && dr["Vertical"].ToString() == vertical)
                        {
                            dr.Delete();
                        }
                    }
                    //delete If data exists            
                    dtTemp2.AcceptChanges();

                    btn2 = this.Controls.Find("btnName" + horizontal + vertical, true)[0] as Button;
                    btn2.Text = txtBtnNameDown.Text;
                    var dn = dtTemp2.NewRow();
                    dn["GroupNO"] = groupno;
                    dn["Horizontal"] = horizontal;
                    dn["Vertical"] = vertical;
                    dn["MasterKBN"] = RdoJanCD.Checked ? "1" : "2";
                    //dn["btndetailBottunName"] =string.IsNullOrWhiteSpace(txtBtnNameDown.Text) ? DBNull.Value.ToString() : txtBtnNameDown.Text.Trim();
                    //dn["Button"] =string.IsNullOrWhiteSpace(txtCD.Text) ? DBNull.Value.ToString(): txtCD.Text.Trim();
                    dn["btndetailBottunName"] = txtBtnNameDown.Text.Trim();
                    dn["Button"] =txtCD.Text.Trim();
                    dn["AdminNO"] = string.IsNullOrWhiteSpace(lblAdminNO.Text) ? 0: Convert.ToInt32(lblAdminNO.Text) ;
                    dn["JanCD"] = RdoJanCD.Checked ? txtCD.Text : string.Empty;
                    dn["CustomerCD"] = RdoCustomerCD.Checked ? txtCD.Text : string.Empty;
                    dtTemp2.Rows.Add(dn);

                }
            }
        }
        private void btnConfirm1_Click(object sender, EventArgs e)
        {
            if(ErrorCheck(1))
            {
                btnProcess.Enabled = true;//Save_Button
                                          //return;            

                string NO = string.Empty;
                BtnName = txtButtomNameUp.Text;
                NO = lblGroupNO.Text;
                groupno = NO.Replace("番目", "");

                


                if (dtTemp1 != null)
                {
                    for (int i = 0; i < dtTemp1.Rows.Count; i++)
                    {
                        DataRow dr = dtTemp1.Rows[i];
                        if (dr["GroupNO"].ToString() == groupno)
                        {
                            dr.Delete();
                            dtTemp1.AcceptChanges();
                        }
                    }

                    btn1 = this.Controls.Find("buttonName" + Convert.ToInt32(groupno), true)[0] as Button;//buttonName(1,2,3...)coding
                    btn1.Text = txtButtomNameUp.Text;//firstGridview_Btn Input 
                    var dn = dtTemp1.NewRow();
                    dn["GroupNO"] = groupno;
                    dn["ButtomName"] = txtButtomNameUp.Text.Trim();
                    dn["MasterKBN"] = RdoJanCD.Checked ? "1" : "2";
                    dtTemp1.Rows.Add(dn);
                }
            }

        }

        private void DisplayName()
        {
            DataTable dt = new DataTable();
            if (RdoJanCD.Checked)
            {
                dt = SelectSKUData(txtCD.Text);
                if (dt.Rows.Count == 1)
                {
                    txtCD.Text = dt.Rows[0]["JanCD"].ToString();
                    lblSearchName.Text =dt.Rows[0]["SKUName"].ToString();
                    lblSearchName.Visible = true;
                }
                else
                {
                    lblSearchName.Text = string.Empty;
                }
            }
            else if (RdoCustomerCD.Checked)
            {
                dt = SelectCustomerData(txtCD.Text);
                if (dt.Rows.Count == 1)
                {
                    txtCD.Text = dt.Rows[0]["CustomerCD"].ToString();
                    lblSearchName.Text = dt.Rows[0]["CustomerName"].ToString();
                    lblSearchName.Visible = true;
                }
                else
                    lblSearchName.Text = string.Empty;
            }
        }
    }
}
