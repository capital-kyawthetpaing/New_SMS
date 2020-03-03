using BL;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempoRegiHanbaiRireki
{
    public partial class FrmSelectSales : Form
    {
        private const int GYO_CNT = 5;

        public string mSalesNo;
        private string mJuchuNo; 
        private TempoRegiHanbaiRireki_BL tbl = new TempoRegiHanbaiRireki_BL();
        private DataTable dtSales;
        public bool flgCancel = false;

        public FrmSelectSales(string JuchuuNO)
        {
            InitializeComponent();
            mJuchuNo = JuchuuNO;
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

        private void FrmSelectSales_Load(object sender, EventArgs e)
        {
            AddHandler();

            D_Sales_Entity dje = new D_Sales_Entity();
            dje.JuchuuNO = mJuchuNo;

            dtSales = tbl.D_Sales_Select(dje);

            if (dtSales.Rows.Count > 0)
            {
                //【Data Area Detail】
                DispFromDataTable();

                btnDown.Tag = "0";
                lblDtGyo1.BackColor = Color.FromArgb(255, 242, 204);
            }
            else
            {
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

        private void DispFromDataTable(int gyoNo = 1)
        {

            Clear(pnlDetails);

            for (int i = 1; i <= GYO_CNT; i++)
            {
                int index = gyoNo - 2 + i;

                if (dtSales.Rows.Count <= index)
                    break;

                DataRow row = dtSales.Rows[index];

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
                }
                cs = this.Controls.Find("lblDtColorSize" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["ColorSizeName"].ToString();
                }
                cs = this.Controls.Find("lblJuchuuDate" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    if (row["ROWNUM"].ToString() == "1")
                        cs[0].Text = row["SalesDate"].ToString();
                }
                cs = this.Controls.Find("lblStoreName" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    if (row["ROWNUM"].ToString() == "1")
                        cs[0].Text = row["StoreName"].ToString() + " " + row["StaffName"].ToString();
                }
                cs = this.Controls.Find("lblJANCD" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["JANCD"].ToString();
                }
                cs = this.Controls.Find("lblJuchuuNO" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    if (row["ROWNUM"].ToString() == "1")
                        cs[0].Text = row["SalesNO"].ToString();
                }
                cs = this.Controls.Find("lblDtSSu" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = tbl.Z_SetStr(row["SalesSu"]);
                }
                cs = this.Controls.Find("lblDtKin" + i.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = "\\" + tbl.Z_SetStr(row["SalesGaku"]);
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

                if (ctrl is Button)
                    ((Button)ctrl).Text = string.Empty;

                ctrl.Tag = "";
            }
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
        private void EndSec()
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                flgCancel = true;
                EndSec();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try {

                int index = Convert.ToInt16(btnDown.Tag);
                string salesNo = dtSales.Rows[index]["SalesNO"].ToString();

                mSalesNo = salesNo;

                EndSec();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }
    }
}
