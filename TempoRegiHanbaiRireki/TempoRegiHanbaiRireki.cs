using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using Entity;
using Search;
using BL;
using Microsoft.VisualBasic;

namespace TempoRegiHanbaiRireki
{
    public partial class TempoRegiHanbaiRireki : ShopBaseForm
    {
        private const string TempoShukkaNyuuryoku = "TempoShukkaNyuuryoku.exe";
        private const int GYO_CNT = 10;

        private TempoRegiHanbaiRireki_BL tprg_Hanbai_Bl = new TempoRegiHanbaiRireki_BL();
        private DataTable dtJuchu;

        public TempoRegiHanbaiRireki()
        {
            InitializeComponent();
        }
        private void TempoRegiHanbaiRireki_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = "TempoRegiHanbaiRireki";
                StartProgram();

                btnClose.Text = "終  了";
                
                SetRequireField();
                AddHandler();

                txtCustomerNo.Text = "";
                lblCusName.Text = "";
                lblLastPoint.Text = "";
                Clear(pnlDetails);

                lblCusName.TextAlign = ContentAlignment.TopLeft;

                txtCustomerNo.Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();
            }
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

        /// <summary>
        /// Get All control 
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
        }

        private void Clear(Panel panel)
        {
            IEnumerable<Control> c = GetAllControls(panel);
            foreach (Control ctrl in c)
            {
                if (ctrl is Label)
                    ((Label)ctrl).Text = string.Empty;

                if(ctrl is Button)
                    ((Button)ctrl).Text = string.Empty;

                ctrl.Tag = "";
            }
        }
        private void SetRequireField()
        {
            txtCustomerNo.Require(true);
        }

        private void DispFromJuchu()
        {
            D_Juchuu_Entity dje = new D_Juchuu_Entity();
            dje.CustomerCD = txtCustomerNo.Text;
            
            dtJuchu = tprg_Hanbai_Bl.D_Juchu_Select(dje);

            if (dtJuchu.Rows.Count > 0)
            {
                //【Data Area Detail】
                DispFromDataTable();
            }
            else
            {
                //該当データなし
                bbl.ShowMessage("E128");
                //EndSec();
            }
        }

        private void DispFromDataTable(int gyoNo = 1)
        {

            Clear(pnlDetails);

            for (int i=1; i<= GYO_CNT; i++)
            {
                int index = gyoNo - 2 + i;

                if (dtJuchu.Rows.Count <= index)
                    break;

                DataRow row = dtJuchu.Rows[index];

                Color foreCl = Color.Black;

                //★出荷可能（引当が終わっているが、まだ出荷していない）
                //Sum(ShippingPossibleSU）＞	Sum(ShippingSu）なら、文字を赤色にする			
                if (bbl.Z_Set(row["ShippingPossibleSU"]) > 0)
                    foreCl = Color.Red;

                //★出荷可能②（直送）＆未売上	
                if(row["DirectFLG"].ToString().Equals("1") && !string.IsNullOrWhiteSpace( row["OrderNO"].ToString()))
                {
                    if(bbl.Z_Set(row["JuchuuSu"]) > bbl.Z_Set(row["SalesSU"]))
                    {
                        foreCl = Color.Red;
                    }
                }

                //★売上済（売上が終わっている）
                //D_JuchuuDetails.JuchuuSu≦Sum(SalesSU）なら、文字を青色にする
                if (bbl.Z_Set(row["JuchuuZan"]) <= 0)
                    foreCl = Color.RoyalBlue;

                //lblDtGyoをさがす。子コントロールも検索する。
                Control[] cs = this.Controls.Find("lblDtGyo" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = (index + 1).ToString();
                }

                cs = this.Controls.Find("lblDtSKUName" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["SKUName"].ToString();
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblDtColorSize" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["ColorSizeName"].ToString();
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblJuchuuDate" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    if (row["ROWNUM"].ToString() == "1")
                        cs[0].Text = row["JuchuuDate"].ToString();
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblStoreName" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    if(row["ROWNUM"].ToString() == "1") 
                        cs[0].Text = row["StoreName"].ToString() + " " + row["StaffName"].ToString();
                    cs[0].ForeColor = foreCl;

                }
                cs = this.Controls.Find("lblJANCD" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["JANCD"].ToString();
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblJuchuuNO" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    if (row["ROWNUM"].ToString() == "1")
                        cs[0].Text = row["JuchuuNO"].ToString();
                    if(row["ROWNUMS"].ToString()=="1")
                        cs[0].Text +="/" + row["SalesNO"].ToString();

                    cs[0].ForeColor = foreCl;
                                        
                }

                cs = this.Controls.Find("lblDtSSu" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = bbl.Z_SetStr(row["JuchuuSuu"]);
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblDtKin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["JuchuuGaku"]);
                    cs[0].ForeColor = foreCl;
                }
                //ckmShop_Label5.ForeColor = foreCl;
            }
        }

        protected override void EndSec()
        {
            this.Close();
        }
        private bool ErrorCheck(int kbn = 0)
        {
            if (kbn == 1)
            {
                //txtCustomerNo
                if (!RequireCheck(new Control[] { txtCustomerNo }))
                {
                    return false;
                }

                if (!CheckWidth(1))
                    return false;

                if (!CheckWidth(2))
                    return false;

                //得意先マスター(M_Customer)に存在すること
                //[M_Customer_Select]
                M_Customer_Entity mce = new M_Customer_Entity
                {
                    CustomerCD = txtCustomerNo.Text,
                    ChangeDate = bbl.GetDate()
                };
                Customer_BL sbl = new Customer_BL();
                bool ret = sbl.M_Customer_Select(mce);
                if (ret)
                {
                    lblCusName.Text = mce.CustomerName;
                    lblLastPoint.Text = bbl.Z_SetStr( mce.LastPoint);
                }
                else
                {
                    lblCusName.Text = "";
                }

                //画面転送表01に従って、画面情報を表示
                DispFromJuchu();
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
     
        private bool Save(int kbn = 0)
        {
            if(ErrorCheck(kbn))
            {
                if (kbn == 0)
                {
                    if (btnDown.Tag == null)
                        return false;

                    int index = Convert.ToInt16(btnDown.Tag);

                    if (dtJuchu.Rows[index]["StoreCD"].ToString() != StoreCD)
                    {
                        bbl.ShowMessage("E147");
                        return false;
                    }

                    string JuchuuNO = dtJuchu.Rows[index]["JuchuuNO"].ToString();
                    bool miuri = false;
                    bool urizumi = false;

                    //未売上の明細が存在するか確認 (ShippingPossibleSU = SUM(DR.ShippingPossibleSU) - SUM(DR.ShippingSu))
                    DataRow[] rows = dtJuchu.Select("JuchuuNO = '" + JuchuuNO + "' AND ShippingPossibleSU > 0");
                    if (rows.Length > 0)
                        miuri = true;

                    //売上済の明細が存在するか確認
                    int salesCnt = Convert.ToInt16(dtJuchu.Rows[index]["CNT"]);
                    string salesNo = dtJuchu.Rows[index]["SalesNO"].ToString();

                    if (salesCnt > 0)
                        urizumi = true;

                    //未売上明細あり＆売上済明細ありの場合
                    if(miuri && urizumi)
                    {
                        FrmSelect frm = new FrmSelect();
                        frm.ShowDialog();
                        int select = frm.btnSelect;

                        switch (select)
                        {
                            case 1:
                                //未売上明細を出荷・売上する　を選択された場合
                                //TempRegiShukkaNyuuryoku を起動。
                                ExecTempoShukkaNyuuryoku(txtCustomerNo.Text, JuchuuNO);
                                break;

                            case 2:
                                //既に出荷・売上した明細を見る
                                //Count(D_SalesDetails.SalesNO)>1なら（売上が何件かある⇒選択画面で選んでもらう）					
                                //画面転送表02
                                if (salesCnt > 1)
                                {
                                    FrmSelectSales frmS = new FrmSelectSales(JuchuuNO);
                                    frmS.ShowDialog();
                                    ExecTempoShukkaNyuuryoku(txtCustomerNo.Text, JuchuuNO, frmS.mSalesNo);
                                }

                                //Count(D_SalesDetails.SalesNO)＝1なら（売上が１件しかない）			
                            else    if (salesCnt == 1)
                                {
                                    //TempRegiShukkaNyuuryoku を起動。
                                    ExecTempoShukkaNyuuryoku(txtCustomerNo.Text, JuchuuNO, salesNo);
                                }
                                break;
                        }
                    }
                    else if(miuri)
                    {
                        //未売上明細あり＆売上済明細なしの場合
                        //TempRegiShukkaNyuuryoku を起動。
                        ExecTempoShukkaNyuuryoku(txtCustomerNo.Text, JuchuuNO);
                    }
                    else if(urizumi)
                    {
                        //未売上明細なし＆売上済明細ありの場合
                        //Count(D_SalesDetails.SalesNO)>1なら（売上が何件かある⇒選択画面で選んでもらう）					
                        //画面転送表02
                        if (salesCnt > 1)
                        {
                            FrmSelectSales frmS = new FrmSelectSales(JuchuuNO);
                            frmS.ShowDialog();
                            if(!frmS.flgCancel)
                                ExecTempoShukkaNyuuryoku(txtCustomerNo.Text, JuchuuNO, frmS.mSalesNo);
                        }

                        //Count(D_SalesDetails.SalesNO)	＝1なら（売上が１件しかない）
                       else if (salesCnt == 1)
                        {
                            //TempRegiShukkaNyuuryoku を起動。
                            ExecTempoShukkaNyuuryoku(txtCustomerNo.Text, JuchuuNO, salesNo);
                        }
                    }
                }

                return true;
            }
            return false;
        }
        private void ExecTempoShukkaNyuuryoku(string customerCd, string juchuno, string salesNo="")
        {
            try
            {
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + TempoShukkaNyuuryoku;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + StoreCD + " " + customerCd + " " + juchuno + " " + salesNo;
                    System.Diagnostics.Process.Start(filePath, cmdLine);
                }
                else
                {
                    //ファイルなし
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private  bool CheckWidth(int type)
        {
            switch(type)
            {
                case 1:
                    string str = Encoding.GetEncoding(932).GetByteCount(txtCustomerNo.Text).ToString();
                    if (Convert.ToInt32(str) > 13)
                    {
                        MessageBox.Show("Bytes Count is Over!!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Focus();
                        return false;
                    }
                    break;

                case 2:
                    int byteCount = Encoding.UTF8.GetByteCount(txtCustomerNo.Text);//FullWidth_Case
                    int onebyteCount= System.Text.ASCIIEncoding.ASCII.GetByteCount(txtCustomerNo.Text);//HalfWidth_Case
                    if (onebyteCount!=byteCount)
                    {
                        MessageBox.Show("Bytes Count is Over!!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Focus();
                        return false;
                    }                  
                  
                    break;
            }           
            return true;
        }

        private void txtCustomerNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                     if(Save(1))
                    {
                        //あたかもTabキーが押されたかのようにする
                        //Shiftが押されている時は前のコントロールのフォーカスを移動
                        ProcessTabKey(!e.Shift);
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void txtShippingSu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                 if(   Save(2))
                        //あたかもTabキーが押されたかのようにする
                        //Shiftが押されている時は前のコントロールのフォーカスを移動
                        ProcessTabKey(!e.Shift);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
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
                if (dtJuchu.Rows.Count >= gyoNo + GYO_CNT)
                {
                    DispFromDataTable(gyoNo + GYO_CNT);

                    int index = Convert.ToInt16(btnDown.Tag);
                    if (dtJuchu.Rows.Count > index + GYO_CNT)
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

        private void lblDtGyo_Click(object sender, EventArgs e)
        {
            try
            {
                Label lblsender = (Label)sender;

                if (string.IsNullOrWhiteSpace(lblsender.Text))
                    return;

                int index =Convert.ToInt16(lblsender.Text)-1;

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

            int i = Convert.ToInt16( lblsender.Name.Substring(8));

            Control[] cs = this.Controls.Find("lblGyoSelect" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblDtSKUName" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblDtColorSize" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblJuchuuDate" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblStoreName" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;

            }
            cs = this.Controls.Find("lblJANCD" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblJuchuuNO" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }

            cs = this.Controls.Find("lblDtSSu" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
            cs = this.Controls.Find("lblDtKin" + i.ToString(), true);
            if (cs.Length > 0)
            {
                cs[0].BackColor = backCl;
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                Save(1);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        //private void btnCustomerNo_Click(object sender, EventArgs e)
        //{

        //}

        private void lblGyoSelect1_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo1, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect2_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo2, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect3_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo3, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect4_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo4, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect5_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo5, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect6_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo6, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect7_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo7, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect8_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo8, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect9_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo9, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect10_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo10, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            TempoRegiKaiinKensaku tgkkk = new TempoRegiKaiinKensaku();
            tgkkk.ShowDialog();

            if (!string.IsNullOrEmpty(tgkkk.CustomerCD))
            {
                txtCustomerNo.Text = tgkkk.CustomerCD;
            }
        }
    }
}
