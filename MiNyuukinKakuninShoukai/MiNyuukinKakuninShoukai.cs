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

namespace MiNyuukinKakuninShoukai
{
    /// <summary>
    /// MiNyuukinKakuninShoukai 未入金確認照会
    /// </summary>
    internal partial class MiNyuukinKakuninShoukai : FrmMainForm
    {
        private const string ProID = "MiNyuukinKakuninShoukai";
        private const string ProNm = "未入金確認照会";
        private const short mc_L_END = 3; // ロック用
        private const string Hacchuu = "HacchuuNyuuryoku.exe";

        private enum EIndex : int
        {
            StoreCD,
            ShippingDateFrom,
            ShippingDateTo,
            CheckBox1,
            CheckBox2,
            PaymentMethodCD,
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

        private Control[] detailControls;
       
        private MiNyuukinKakuninShoukai_BL mibl;
        private D_CollectPlan_Entity doe;        
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避        

        public MiNyuukinKakuninShoukai()
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
                Btn_F9.Text = "";

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                mibl = new MiNyuukinKakuninShoukai_BL();
                CboStoreCD.Bind(ymd);
                Bind(CboPaymentMethodCD, 2);

                Scr_Clr(0);

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = mibl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                }

                //detailControls[(int)EIndex.ShippingDateFrom].Text = ymd;
                //detailControls[(int)EIndex.ShippingDateTo].Text = ymd;

                detailControls[(int)EIndex.StoreCD].Focus(); 
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
            detailControls = new Control[] { CboStoreCD, ckM_TextBox1, ckM_TextBox2, ckM_CheckBox1, ckM_CheckBox2, CboPaymentMethodCD };

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
                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboStoreCD.MoveNext = false;
                        return false;
                    }
                    else
                    {
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            CboStoreCD.MoveNext = false;
                            bbl.ShowMessage("E141");
                            return false;
                        }
                    }

                    break;
                case (int)EIndex.ShippingDateFrom:
                case (int)EIndex.ShippingDateTo:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //大小チェック
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.ShippingDateTo)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                return false;
                            }
                        }
                    }
                    break;

                case (int)EIndex.CheckBox2:
                    if(!ckM_CheckBox1.Checked && !ckM_CheckBox2.Checked)
                    {
                        //Ｅ１１１
                        bbl.ShowMessage("E111");
                        return false;
                    }
                    break;

                case (int)EIndex.PaymentMethodCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboPaymentMethodCD.MoveNext = false;
                        return false;
                    }
                    else
                    {
                      
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
            
            //更新処理
            doe = GetEntity();
            DataTable dt = mibl.D_CollectPlan_SelectAllForSyokai(doe);

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
                bbl.ShowMessage("E128");
            }
        }

        protected override void ExecSec()
        {
            try
            {
                //詳細画面を起動します
                FrmDetail frm = new FrmDetail();
                doe.CollectPlanNO = GvDetail.CurrentRow.Cells["CollectPlanNO"].Value.ToString();
                frm.dt = mibl.D_CollectPlan_SelectAllForSyosai(doe);
                if(frm.dt.Rows.Count >0 )
                    frm.ShowDialog();
                else
                {
                    mibl.ShowMessage("E128");
                }

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
        private D_CollectPlan_Entity GetEntity()
        {
            doe = new D_CollectPlan_Entity();
            doe.StoreCD = CboStoreCD.SelectedValue.ToString();

            doe.DateFrom = detailControls[(int)EIndex.ShippingDateFrom].Text;
            doe.DateTo = detailControls[(int)EIndex.ShippingDateTo].Text;
            doe.Kbn = 1;

            if (ckM_CheckBox1.Checked)
                if (!ckM_CheckBox2.Checked)
                    doe.Kbn = 2;

            if (ckM_CheckBox2.Checked)
                if (!ckM_CheckBox1.Checked)
                    doe.Kbn = 3;

            doe.PaymentMethodCD = CboPaymentMethodCD.SelectedValue.ToString();

            return doe;
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            if (Kbn == 0)
            {
                foreach (Control ctl in detailControls)
                {
                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }
                
            }

            ckM_CheckBox1.Checked = true;
            ckM_CheckBox2.Checked = true;
            if (CboPaymentMethodCD.DataSource != null)
            {
                if (((DataTable)CboPaymentMethodCD.DataSource).Rows.Count >= 2)
                {
                    CboPaymentMethodCD.SelectedIndex = 1;
                }
            }
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
                        if (index == (int)EIndex.PaymentMethodCD)
                        {
                           BtnSubF11.Focus();
                        }
                        else
                            detailControls[index + 1].Focus();

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
        
        //private void DgvDetail_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.KeyCode == Keys.Enter)
        //            if (e.Control == false)
        //            {
        //                this.ExecSec();
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //        //EndSec();
        //    }
        //}

        private void GvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == (int)EColNo.Btn)
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
        private void GvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //セルの列を確認
            if (e.ColumnIndex == (int)EColNo.Status )
            {
                ////セルの値により、背景色を変更する
                //switch (e.Value)
                //{
                //    case "承認済":
                //        e.CellStyle.BackColor = ClsGridBase.GridColor;
                //        break;
                //    case "承認中":
                //    case "申請":
                //        e.CellStyle.BackColor = Color.PaleGoldenrod;
                //        break;
                //    case "却下":
                //        e.CellStyle.BackColor = Color.Pink;
                //        break;

                //}
            }
        }

        #endregion

    }
}








