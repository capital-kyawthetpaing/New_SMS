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
    public partial class Search_Mitsumori : FrmSubForm
    {
        private const string ProNm = "見積番号検索";

        private enum EIndex : int
        {
            DayStart,
            DayEnd,
            StoreCD,
            InputStart,
            InputEnd,
            StaffCD,
            CustomerCD,
            CustomerName,
            MitsumoriName,
            JuchuChanceKbn,
        }
        public string OperatorCD = string.Empty;
        public string MitsumoriNo = string.Empty;
        public string ChangeDate = string.Empty;
        public string MitsumoriName = string.Empty;

        private Control[] detailControls;
        D_Mitsumori_Entity dme;
        MitsumoriNyuuryoku_BL ssbl;

        public Search_Mitsumori(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;

            CboStoreCD.Bind(changeDate);
            CboJuchuuChanceKBN.Bind(changeDate);

            ScCustomer.Value1 = "1";

            ssbl = new MitsumoriNyuuryoku_BL();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2,CboStoreCD, ckM_TextBox3, ckM_TextBox4,ScStaff.TxtCode,ScCustomer.TxtCode, ckM_CustomerName, ckM_TextBox5,CboJuchuuChanceKBN };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
            }
        }
        private void btnSubF11_Click(object sender, EventArgs e)
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

        private D_Mitsumori_Entity GetSearchInfo()
        {
            dme = new D_Mitsumori_Entity
            {
                MitsumoriDateFrom = detailControls[(int)EIndex.DayStart].Text,
                MitsumoriDateTo = detailControls[(int)EIndex.DayEnd].Text,
                MitsumoriInputDateFrom = detailControls[(int)EIndex.InputStart].Text,
                MitsumoriInputDateTo = detailControls[(int)EIndex.InputEnd].Text,
                MitsumoriName = detailControls[(int)EIndex.MitsumoriName].Text,
                StaffCD=ScStaff.TxtCode.Text,
                CustomerCD=ScCustomer.TxtCode.Text,
                CustomerName= detailControls[(int)EIndex.CustomerName].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
                JuchuuChanceKBN = CboJuchuuChanceKBN.SelectedValue.Equals("-1") ? string.Empty : CboJuchuuChanceKBN.SelectedValue.ToString(),
            };

            if (ckM_RadioButton1.Checked)
            {
                dme.JuchuuFLG1 = "0";
                dme.JuchuuFLG2 = "1";
            }else if (ckM_RadioButton2.Checked)
            {
                //受注済
                dme.JuchuuFLG1 = "1";
                dme.JuchuuFLG2 = "1";
            }else
            {
                //未受注
                dme.JuchuuFLG1 = "0";
                dme.JuchuuFLG2 = "0";
            }

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
            DataTable dt = ssbl.D_Mitsumori_SelectAll(dme);
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
                        if (detailControls.Length - 1 > Array.IndexOf(detailControls, sender))
                            detailControls[Array.IndexOf(detailControls, sender) + 1].Focus();

                        else
                            ckM_RadioButton1.Focus();
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
                //EndSec();
            }
        }
        private bool CheckDetail(int index)
        {
            switch (index)
            {
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                case (int)EIndex.InputStart:
                case (int)EIndex.InputEnd:
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
                    //見積日(From) ≧ 見積日(To)である場合Error
                    if (index == (int)EIndex.DayEnd || index == (int)EIndex.InputEnd)
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
                    if (CboStoreCD.SelectedValue.Equals("-1"))
                    {
                        ssbl.ShowMessage("E102");
                        CboStoreCD.Focus();
                        return false;
                    }
                    else
                    {
                        //Todo:店舗権限のチェック、引数で処理可能店舗の配列をセットしたい
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

                case (int)EIndex.MitsumoriName:

                    break;
            }

            return true;
        }

        private void GetData()
        {
            if(GvDetail.CurrentRow != null &&  GvDetail.CurrentRow.Index >= 0)
            { 
                MitsumoriNo = GvDetail.CurrentRow.Cells["colMitsumoriNo"].Value.ToString();
                //ChangeDate = GvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();
                MitsumoriName = GvDetail.CurrentRow.Cells["colMitsumoriName"].Value.ToString();
            }
        }

        /// <summary>
        /// 画面クリア
        /// </summary>
        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
                ctl.Text = "";

            ckM_RadioButton1.Checked = true;

        }

        private void Search_Mitsumori_Load(object sender, EventArgs e)
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
                    //ScStaff.LabelText = mse.StaffName;
                }

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
