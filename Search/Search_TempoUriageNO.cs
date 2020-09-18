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
using Entity;
using BL;

namespace Search
{
    public partial class Search_TempoUriageNO : FrmSubForm
    {
        private const string ProNm = "売上番号検索";

        private enum EIndex : int
        {
            DayStart,
            DayEnd,
            StoreCD,
            StaffCD,
            CustomerCD,
            CustomerName,
        }
        public string OperatorCD = string.Empty;
        public string SalesNo = string.Empty;
        public string ChangeDate = string.Empty;

        private Control[] detailControls;
        D_Sales_Entity dme;
        Search_TempoUriageNO_BL ssbl;

        public Search_TempoUriageNO(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;

            CboStoreCD.Bind(changeDate);
            ScCustomer.Value1 = "1";

            ssbl = new Search_TempoUriageNO_BL();
            
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2,CboStoreCD,ScStaff.TxtCode,ScCustomer.TxtCode, ckM_CustomerName };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
            }
        }
        private void BtnF11_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
            try
            {
                base.FunctionProcess(FuncDisp - 1);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private D_Sales_Entity GetSearchInfo()
        {
            dme = new D_Sales_Entity
            {
                SalesDateFrom = detailControls[(int)EIndex.DayStart].Text,
                SalesDateTo = detailControls[(int)EIndex.DayEnd].Text,
                StaffCD=ScStaff.TxtCode.Text,
                CustomerCD=ScCustomer.TxtCode.Text,
                CustomerName= detailControls[(int)EIndex.CustomerName].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
            };

            return dme;
        }

        private void DgvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    if (e.Control == false)
                    {
                        this.ExecSec();
                    }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void GvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.ExecSec();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        protected override void ExecSec()
        {
            GetData();
            EndSec();
        }

        protected override void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            dme = GetSearchInfo();
            DataTable dt = ssbl.D_Sales_SelectAll(dme);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count>0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
            }
            else
            {
               ssbl.ShowMessage("E128");
                GvDetail.DataSource = null;
                detailControls[0].Focus();
            }
        }
        private void DetailControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    bool ret = CheckDetail(Array.IndexOf(detailControls, sender));
                    if (ret)
                    {
                        if ((int)EIndex.CustomerCD == Array.IndexOf(detailControls, sender))
                            btnSubF11.Focus();
                        else
                            detailControls[Array.IndexOf(detailControls, sender) + 1].Focus();
                    }
                    else
                    {
                        ((Control)sender).Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void RadioButton_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {

                    btnSubF11.Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

        }
        private void CboStoreCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CboStoreCD.SelectedIndex > 0)
                    ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private bool CheckDetail(int index)
        {
            switch (index)
            {
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = ssbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!ssbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        ssbl.ShowMessage("E103");
                        return false;
                    }
                    //売上日(From) ≧ 売上日(To)である場合Error
                    if (index == (int)EIndex.DayEnd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                ssbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;

                case (int)EIndex.StoreCD:
                    if (CboStoreCD.SelectedIndex == -1)
                    {
                        ssbl.ShowMessage("E102");
                        CboStoreCD.Focus();
                        return false;
                    }
                    else
                    {
                        //店舗権限のチェック、引数で処理可能店舗の配列をセットしたい
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            ssbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }

                    }
                    break;

                case (int)EIndex.StaffCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScStaff.LabelText = "";
                        return true;
                    }

                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = ssbl.GetDate() // detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Staff_BL bl = new Staff_BL();
                    bool ret = bl.M_Staff_Select(mse);
                    if (ret)
                    {
                        ScStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        ssbl.ShowMessage("E101");
                        ScStaff.LabelText = "";
                        return false;
                    }
                    break;

                case (int)EIndex.CustomerCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScCustomer.LabelText = "";
                        ckM_CustomerName.Text = "";
                        return true;
                    }

                    //[M_Customer_Select]
                    M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = ssbl.GetDate()     // detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Customer_BL sbl = new Customer_BL();
                    ret = sbl.M_Customer_Select(mce);
                    if (ret)
                    {
                        ScCustomer.LabelText = mce.CustomerName;
                        ckM_CustomerName.Text = mce.CustomerName;
                    }
                    else
                    {
                        ssbl.ShowMessage("E101");
                        ScCustomer.LabelText = "";
                        ckM_CustomerName.Text = "";
                        return false;
                    }

                    break;

            }

            return true;
        }

        private void GetData()
        {
            if(GvDetail.CurrentRow != null &&  GvDetail.CurrentRow.Index >= 0)
            { 
                SalesNo = GvDetail.CurrentRow.Cells["colSalesNo"].Value.ToString();
                //ChangeDate = GvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();
                            }
        }

        /// <summary>
        /// 画面クリア
        /// </summary>
        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
                ctl.Text = "";
            
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                Scr_Clr();

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = OperatorCD,       //パラメータでオペレータCDをセット
                    ChangeDate = ssbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                }

                F9Visible = false;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
    }
}
