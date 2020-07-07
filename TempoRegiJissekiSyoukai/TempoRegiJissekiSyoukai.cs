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

namespace TempoRegiJissekiSyoukai
{
    public partial class TempoRegiJissekiSyoukai : ShopBaseForm
    {
        private const int GYO_CNT = 10;

        private TempoRegiJissekiSyoukai_BL tprg_Jisseki_Bl = new TempoRegiJissekiSyoukai_BL();
        private DataTable dtSales;
        
        public TempoRegiJissekiSyoukai()
        {
            InitializeComponent();
        }
        private void TempoRegiJissekiSyoukai_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = "TempoRegiJissekiSyoukai";
                StartProgram();

                btnProcess.Visible = false;
                BtnP_text = "";

                SetRequireField();
                AddHandler();

                txtDate.Text = "";
                Clear(pnlDetail);

                txtDate.Focus();
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

            btnUp.Enabled = false;
            btnDown.Enabled = false;
        }
        private void SetRequireField()
        {
            txtDate.Require(true);
        }

        private void DispFromSales()
        {
            D_Sales_Entity dse = new D_Sales_Entity();
            dse.SalesDate = txtDate.Text;
            
            dtSales = tprg_Jisseki_Bl.D_Sales_Select(dse);

            if (dtSales.Rows.Count > 0)
            {
                //【Data Area Detail】
                DispFromDataTable();
            }
            else
            {
                Clear(pnlDetail);
                tprg_Jisseki_Bl.ShowMessage("E128");
                txtDate.Focus();
            }
        }

        private void DispFromDataTable(int gyoNo = 1)
        {

            Clear(pnlDetail);

            for (int i=1; i<= GYO_CNT; i++)
            {
                int index = gyoNo - 2 + i;

                if (dtSales.Rows.Count <= index)
                    break;

                DataRow row = dtSales.Rows[index];

                Color foreCl = Color.Black;

                //lblDtGyoをさがす。子コントロールも検索する。
                Control[] cs = this.Controls.Find("lblDtGyo" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = (index + 1).ToString();
                }

                cs = this.Controls.Find("lblStoreName" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["StoreName"].ToString();
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblJyukin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["JuchuuGaku"]);
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblCardUrikin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["CardUriageGaku"]);
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblGenUrikin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["CashUriageGaku"]);
                    cs[0].ForeColor = foreCl;

                }
                cs = this.Controls.Find("lblKakeUrikin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["KakeUriageGaku"]);
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblSonotakin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["HokaUriageGaku"]);
                    cs[0].ForeColor = foreCl;
                }

                cs = this.Controls.Find("lblDayUrikin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["SalesGakuDay"]);
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblMonthUrikin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["SalesGakuMonth"]);
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblDayYoskin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["BudgetGakuDay"]);
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblMonthYoskin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + bbl.Z_SetStr(row["BudgetGakuMonth"]);
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblDayRitu" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = bbl.Z_Set(row["RituDay"]).ToString("F1") + "%";
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblMonthRitu" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = bbl.Z_Set(row["RituMonth"]).ToString("F1") + "%";
                    cs[0].ForeColor = foreCl;
                }
                cs = this.Controls.Find("lblCusCount" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = bbl.Z_SetStr(row["CSCount"]);
                    cs[0].ForeColor = foreCl;
                }
            }

            btnUp.Enabled = true;
            btnDown.Enabled = true;
        }

        protected override void EndSec()
        {
            this.Close();
        }
        private bool ErrorCheck(int kbn = 0)
        {
            if (kbn == 1)
            {
                //txtDate
                if (!RequireCheck(new Control[] { txtDate }))
                {
                    return false;
                }
                
                //画面転送表01に従って、画面情報を表示
                DispFromSales();
            }

            return true;
        }
        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);

            switch (index)
            {
                case 1 : // F1:終了
                    {
                        break;
                    }
            }
        }

        private bool Save(int kbn = 0)
        {
            if (ErrorCheck(kbn))
            {
                return true;
            }
            return false;
        }
        
        private bool CheckWidth(int type)
        {
            switch(type)
            {
                case 1:
                    string str = Encoding.GetEncoding(932).GetByteCount(txtDate.Text).ToString();
                    if (Convert.ToInt32(str) > 10)
                    {
                        MessageBox.Show("Bytes Count is Over!!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Focus();
                        return false;
                    }
                    break;

                case 2:
                    int byteCount = Encoding.UTF8.GetByteCount(txtDate.Text);//FullWidth_Case
                    int onebyteCount= System.Text.ASCIIEncoding.ASCII.GetByteCount(txtDate.Text);//HalfWidth_Case
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

        private void txtDate_KeyDown(object sender, KeyEventArgs e)
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
                        txtDate.SelectAll();
                        //////あたかもTabキーが押されたかのようにする
                        //////Shiftが押されている時は前のコントロールのフォーカスを移動
                        ////ProcessTabKey(!e.Shift);
                    }
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
                if (dtSales.Rows.Count >= gyoNo + GYO_CNT)
                {
                    DispFromDataTable(gyoNo + GYO_CNT);

                    int index = Convert.ToInt16(btnDown.Tag);
                    if (dtSales.Rows.Count > index + GYO_CNT)
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

                ChangeBackColor(lblsender);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeBackColor(Label lblsender)
        {
            for (int i = 1; i <= GYO_CNT; i++)
            {
                //lblDtGyoをさがす。子コントロールも検索する。
                Control[] cs = this.Controls.Find("lblDtGyo" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].BackColor = Color.Transparent;
                }
            }

            lblsender.BackColor = Color.FromArgb(255, 242, 204);
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
    }
}
