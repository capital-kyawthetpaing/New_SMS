using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace Search_Nyuukinmoto
{
    /// <summary>
    /// Search_Nyuukinmoto 入金元検索
    /// </summary>
    public partial class Search_Nyuukinmoto : FrmMainForm
    {
        private const string ProID = "Search_Nyuukinmoto";
        private const string ProNm = "入金元検索";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            BillingGakuStart,
            BillingGakuEnd,
            StoreCD,
            CustomerName,
            Chk1,
            Chk2,
            InputStart,
            InputEnd,
            Rdo1,
            Rdo2
        }

        private enum EColNo : int
        {
            Btn,     //
            OrderNO,
            OrderDate,
            Status,
            LastDay,
            LastOpe,
            Store,
            Operator,
            Vendor,
            SKUName,

            COUNT
        }
        //単独起動でない場合、明細(Detail)にCursorがある場合、その明細(Detail)の以下情報を元の画面に戻す。																																				
	    //表示単位：請求書の場合、戻り値①0　戻り値②顧客 戻り値③請求番号
        //表示単位：納品書の場合、戻り値①1　戻り値②顧客 戻り値③伝票番号
        public int Kidou = 0;   //単独起動の場合０、他からの起動の場合１
        public string Kbn = "0";
        public string BillingGaku = string.Empty;
        public string DenNO = string.Empty;
        public string Customer = string.Empty;
        public string ChangeDate = string.Empty;

        private Control[] detailControls;

        D_Billing_Entity dbe;
        NyuukinNyuuryoku_BL nnbl;
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避        

        public Search_Nyuukinmoto()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.SHOW);   //照会プログラムとして起動
                this.InitialControlArray();

                //起動時共通処理
                base.StartProgram();
                Btn_F6.Text = "";

                if(Kidou.Equals(1))
                    {
                    //単独起動でない場合、ファンクション名は「戻る」
                    Btn_F1.Text = "F1:戻る";
                    Btn_F12.Text = "F12:確定";
                }
                //コンボボックス初期化
                string ymd = bbl.GetDate();
                nnbl = new NyuukinNyuuryoku_BL();
                CboStoreCD.Bind(ymd);

                Scr_Clr(0);

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = ymd
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                }

                detailControls[0].Focus(); 
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }
        private void Bind(CKM_Controls.CKM_ComboBox combo, int kbn = 0)
        {
            DenominationKBN_BL dbl = new DenominationKBN_BL();
            M_DenominationKBN_Entity me = new M_DenominationKBN_Entity();
            if (kbn.Equals(0))
                me.SystemKBN = "2";

            DataTable dtNyukinhouhou = dbl.BindKbn(me, kbn);
            BindCombo(combo, "DenominationCD", "DenominationName", dtNyukinhouhou);
            ;
        }
        private void BindCombo(CKM_Controls.CKM_ComboBox combo, string key, string value, DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
            combo.DataSource = dt;
            combo.DisplayMember = value;
            combo.ValueMember = key;
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2,CboStoreCD, ckM_CustomerName,ckM_CheckBox1,ckM_CheckBox2
                , ckM_TextBox3, ckM_TextBox4,ckM_RadioButton1,ckM_RadioButton2};

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
        
        /// <summary>
        /// コードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDetail(int index)
        {
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.BillingGakuStart:
                case (int)EIndex.BillingGakuEnd:
                    ////入力必須(Entry required)	入力なければError(If there is no input, an error)			
                    //if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    //{
                    //    nnbl.ShowMessage("E102");
                    //    return false;
                    //}
                    //表示単位が納品書の場合、項目名は「売上額」
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.BillingGakuEnd)
                    {
                        if (nnbl.Z_Set(detailControls[index - 1].Text) > nnbl.Z_Set(detailControls[index].Text))
                        {
                            //Ｅ１０６
                            nnbl.ShowMessage("E104");
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.InputStart:
                case (int)EIndex.InputEnd:
                    //入金状況：入金済ONの場合、入力必須(Entry required)
                    if(ckM_CheckBox2.Checked)
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            nnbl.ShowMessage("E102");
                            return false;
                        }

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = nnbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!nnbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        nnbl.ShowMessage("E103");
                        return false;
                    }
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.InputEnd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                nnbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //入金日ToがNullの場合、入金日Fromを入金日Toにコピー
                        if (string.IsNullOrWhiteSpace(detailControls[index + 1].Text))
                        {
                            detailControls[index + 1].Text = detailControls[index].Text;
                        }
                    }

                    break;

                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { CboStoreCD }))
                    {
                        CboStoreCD.MoveNext = false;
                        return false;
                    }
                    else
                    {
                        //店舗権限のチェック、引数で処理可能店舗の配列をセットしたい
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            CboStoreCD.MoveNext = false;
                            nnbl.ShowMessage("E141");
                            return false;
                        }

                    }
                    break;

                case (int)EIndex.CustomerName:
                    ////入力必須(Entry required)
                    //if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    //{
                    //    nnbl.ShowMessage("E102");
                    //    return false;
                    //}

                    break;

                case (int)EIndex.Chk2:

                    //両方チェックが入っていない場合、Error(If both are not checked, Error)
                    if (!ckM_CheckBox1.Checked && !ckM_CheckBox2.Checked)
                    {
                        //Ｅ１１１
                        nnbl.ShowMessage("E111");
                        ckM_CheckBox1.Focus();
                        return false;
                    }
                    break;
            }

            return true;

        }
        protected override void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            //請求済額（From）、請求済額（To)、顧客名のどれかに入力がなければエラー
            if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.BillingGakuStart].Text) &&
                string.IsNullOrWhiteSpace(detailControls[(int)EIndex.BillingGakuEnd].Text) &&
                string.IsNullOrWhiteSpace(detailControls[(int)EIndex.CustomerName].Text) )
            {
                nnbl.ShowMessage("E188", "請求済額、顧客名");
                detailControls[(int)EIndex.BillingGakuStart].Focus();
                return;
            }

            dbe = GetSearchInfo();
            DataTable dt = nnbl.D_Billing_SelectForSearch(dbe);

            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
            }
            else
            {
                nnbl.ShowMessage("E128");
            }
        }

        protected override void ExecSec()
        {
            try
            {
                GetData();
                EndSec();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Billing_Entity GetSearchInfo()
        {
            dbe = new D_Billing_Entity
            {
                BillingGakuFrom = detailControls[(int)EIndex.BillingGakuStart].Text,
                BillingGakuTo = detailControls[(int)EIndex.BillingGakuEnd].Text,
                CollectDateFrom = detailControls[(int)EIndex.InputStart].Text,
                CollectDateTo = detailControls[(int)EIndex.InputEnd].Text,
                CustomerName = detailControls[(int)EIndex.CustomerName].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
            };
            if (ckM_RadioButton1.Checked)
            {
                dbe.RdoDispKbn = 1;
            }
            else
            {
                dbe.RdoDispKbn = 2;
            }
            if (ckM_CheckBox1.Checked)
            {
                dbe.ChkMinyukin = "1";
            }
            else
            {
                dbe.ChkMinyukin = "0";
            }
            if (ckM_CheckBox2.Checked)
            {
                dbe.ChkNyukinzumi = "1";
            }
            else
            {
                dbe.ChkNyukinzumi = "0";
            }

            return dbe;
        }
        private void GetData()
        {
            if (GvDetail.CurrentRow != null && GvDetail.CurrentRow.Index >= 0)
            {
                if (ckM_RadioButton2.Checked)
                    Kbn = "1";

                DenNO = GvDetail.CurrentRow.Cells["colBillingNO"].Value.ToString();
                Customer =  GvDetail.CurrentRow.Cells["colCustomer"].Value.ToString();
                BillingGaku = GvDetail.CurrentRow.Cells["colBillingGaku"].Value.ToString();
                //ChangeDate = GvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();
            }
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)) || ctl.GetType().Equals(typeof(Button)))
                {

                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_RadioButton)))
                {

                }
                else
                {
                    ctl.Text = "";
                }
            }

            //未入金ON　入金済OFF						
            ckM_CheckBox1.Checked = true;
            ckM_RadioButton1.Checked = true;
        }

        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            switch (Index)
            {
                case 0:     // F1:終了
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会
                    {
                        break;
                    }

                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        break;
                    }

                case 11:    //F12:登録
                    {
                        this.ExecSec();
                        break;
                    }
            }   //switch end

        }

        // ==================================================
        // 終了処理
        // ==================================================
        protected override void EndSec()
        {
            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
        }

        #region "内部イベント"
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckM_RadioButton1.Checked)
                {
                    ckM_Label1.Text = "請求済額";

                    GvDetail.Columns[3].HeaderText = "請求日";
                    GvDetail.Columns[4].HeaderText = "請求番号";
                    GvDetail.Columns[5].HeaderText = "請求残額";
                }
                else
                {
                    //表示単位：納品書の場合、売上額
                    ckM_Label1.Text = "　売上額";

                    //表示単位：納品書の場合、売上日
                    //表示単位：納品書の場合、伝票番号
                    //表示単位：納品書の場合、売上額
                    GvDetail.Columns[3].HeaderText = "売上日";
                    GvDetail.Columns[4].HeaderText = "伝票番号";
                    GvDetail.Columns[5].HeaderText = "売上額";
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
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
                    int index = Array.IndexOf(detailControls, sender);
                    bool ret = CheckDetail(index);

                    if (ret)
                    {
                        if(index == (int)EIndex.InputEnd)
                        {
                            if (ckM_RadioButton1.Checked)
                                ckM_RadioButton1.Focus();
                            else
                                ckM_RadioButton2.Focus();
                        }
                        else if(index == (int)EIndex.Rdo1 || index == (int)EIndex.Rdo2)
                        {
                            BtnSubF11.Focus();
                        }

                        else                        if (detailControls.Length - 1 > Array.IndexOf(detailControls, sender))
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
      
        private void BtnSubF11_Click(object sender, EventArgs e)
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

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnF11_Click(object sender, EventArgs e)
        {
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

        private void GvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if(Kidou.Equals(1))
                    this.ExecSec();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        #endregion

    }
}








