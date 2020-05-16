using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using BL;
using Entity;
using Base.Client;
using Search;

namespace EDIHacchuu
{
    /// <summary>
    /// EDIHacchuu EDI発注
    /// </summary>
    internal partial class EDIHacchuu : FrmMainForm
    {
        private const string ProID = "EDIHacchuu";
        private const string ProNm = "EDI発注";

        private enum EIndex : int
        {
            StoreCD,
            SyoriNO,
            OrderDateFrom,
            OrderDateTo,
            Vendor,
            Staff,
            OrderNO,
            ChkMisyounin,
        }

        private enum RIndex : int
        {
            NotOutput,
            ReOutput
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        private Control[] detailRadios;

        private EDIHacchuu_BL mibl;
        private D_Order_Entity doe;
        private D_EDIOrder_Entity dee;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避


        public EDIHacchuu()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM=ProNm;

                this.SetFunctionLabel(EProMode.BATCH);
                this.InitialControlArray();
                base.Btn_F12.Text = "データ出力(F12)";           
                
                //起動時共通処理
                base.StartProgram();

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                CboStoreCD.Bind(ymd);

                mibl = new EDIHacchuu_BL();

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScOrderNO.Value1 = InOperatorCD;
                ScOrderNO.Value2 = stores;
                ScEDIOrderNO.Value1 = InOperatorCD;
                ScEDIOrderNO.Value2 = stores;
                ScVendor.Value1 = "1";

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

                SetFuncKeyAll(this, "100001000001");
                Scr_Clr(0);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { CboStoreCD,  ScEDIOrderNO.TxtCode, ckM_TextBox1 ,ckM_TextBox2 ,ScVendor.TxtCode
                                            , ScStaff.TxtCode, ScOrderNO.TxtCode, ChkMisyounin
                                            };
            detailLabels = new Control[] { ScVendor, ScStaff };
            searchButtons = new Control[] { ScEDIOrderNO.BtnSearch, ScVendor.BtnSearch, ScStaff.BtnSearch, ScOrderNO.BtnSearch };
            detailRadios = new Control[] {  RdoNotOutput, RdoReOutput};

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

            foreach (Control ctl in detailRadios)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            }
        }     

        
   
        /// <summary>
        /// エラーチェック処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData()
        {
            for (int i = 0; i < detailControls.Length; i++)
            {
                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return false;
                }
            }

            DataTable dt;
            if (RdoNotOutput.Checked)
            {
                doe = GetEntityForOrder();
                dt = mibl.D_Order_SelectAllForEDIHacchuu(doe);

                //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
                if (dt.Rows.Count == 0)
                {
                    bbl.ShowMessage("E133");
                    previousCtrl.Focus();
                    return false;
                }
            }             

            return true;
        }
        protected override void ExecSec()
        {
            bool ret = CheckData();
            if (ret == false)
            {
                return;
            }

　          if (RdoNotOutput.Checked)
            {
                doe.Operator = this.InOperatorCD;
                doe.PC = this.InPcID;

                dee = new D_EDIOrder_Entity();

                ret = mibl.PRC_EDIOrder_Insert(doe,dee);
            }
            else
            {
                dee.Operator = this.InOperatorCD;
                dee.PC = this.InPcID;
                ret = mibl.PRC_EDIOrder_MailInsert(dee);
            }

            this.Cursor = Cursors.Default;

            if (ret == false)
            {
                bbl.ShowMessage("S002");
            }
            else
            {
                bbl.ShowMessage("I002");
            }

            //更新後画面そのまま
            detailControls[0].Focus();
        }
        
        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set=true)
        {
            bool ret;
            DataTable dt;
            EDIHacchuu_BL ble = new EDIHacchuu_BL();

            switch (index)
            {
                case (int)EIndex.StoreCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bbl.ShowMessage("E102");
                        CboStoreCD.Focus();
                        return false;
                    }
                    else
                    {
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            bbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }

                    }

                    break;

                case (int)EIndex.SyoriNO:
                    //入力不可の場合チェックなし
                    if (!detailControls[index].Enabled)
                    {
                        return true;
                    }

                    //必須入力
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //EDI発注(D_EDIOrder)に存在すること
                    //[D_EDIOrder]
                    dee = GetEntityForEDIOrder();
                    dt = ble.D_EDIOrder_Select(dee);
                    if (dt.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E138", "EDI処理番号");
                        return false;
                    }
                    else
                    {
                        //権限がない場合（以下のSelectができない場合）Error　「権限のないEDI発注番号」
                        if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E139", "EDI処理番号");
                            return false;
                        }
                        break;
                    }

                case (int)EIndex.OrderDateFrom:
                case (int)EIndex.OrderDateTo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    string strYmd = "";
                    switch (index)
                    {
                        default:
                            strYmd = bbl.FormatDate(detailControls[index].Text);
                            break;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    switch (index)
                    {
                        default:
                            detailControls[index].Text = strYmd;
                            break;
                    }

                    //発注日(From) ≧ 発注日(To)である場合Error
                    if (index == (int)EIndex.OrderDateTo)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }
                    
                    break;

                case (int)EIndex.Vendor:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //情報ALLクリア
                        ClearCustomerInfo();
                        return true;
                    }

                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate()
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    ret = sbl.M_Vendor_SelectTop1(mve);

                    if (ret)
                    {                        
                        ScVendor.LabelText = mve.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }

                    break;

                case (int)EIndex.Staff:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //情報ALLクリア
                        ClearStaffInfo();
                        return true;
                    }

                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate()
                    };
                    Staff_BL bl = new Staff_BL();
                    ret = bl.M_Staff_Select(mse);
                    if (ret)
                    {
                        if (mse.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            //顧客情報ALLクリア
                            ClearStaffInfo();
                            return false;
                        }
                        ScStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //情報ALLクリア
                        ClearStaffInfo();
                        return false;
                    }
                    break;

                case (int)EIndex.OrderNO:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //発注(D_Order)に存在すること
                    //[D_Order]
                    string orderNo = detailControls[index].Text;
                    dt = ble.D_Order_SelectForEDIHacchuu(orderNo);
                    if (dt.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E138", "発注番号");
                        return false;
                    }
                    else
                    {
                        //DeleteDateTime 「削除された発注番号」
                        if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            bbl.ShowMessage("E140", "発注番号");
                            return false;
                        }

                        //権限がない場合（以下のSelectができない場合）Error　「権限のないEDI発注番号」
                        if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E139", "発注番号");
                            return false;
                        }
                        
                        break;
                    }

            }

            return true;
        }
        /// <summary>
        /// 発注先情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            ScVendor.LabelText = "";
        }

        /// <summary>
        /// スタッフ情報クリア処理
        /// </summary>
        private void ClearStaffInfo()
        {
            ScStaff.LabelText = "";
        }

        private D_Order_Entity GetEntityForOrder()
        {
            doe = new D_Order_Entity
            {
                StoreCD = CboStoreCD.SelectedValue.ToString(),
                OrderDateFrom = detailControls[(int)EIndex.OrderDateFrom].Text,
                OrderDateTo = detailControls[(int)EIndex.OrderDateTo].Text,
                OrderCD = detailControls[(int)EIndex.Vendor].Text,
                StaffCD = detailControls[(int)EIndex.Staff].Text,
                OrderNO = detailControls[(int)EIndex.OrderNO].Text,
            };

            if (ChkMisyounin.Checked)
                doe.ChkMisyonin = 1;
            else
                doe.ChkMisyonin = 0;
            return doe;
        }

        private D_EDIOrder_Entity GetEntityForEDIOrder()
        {
            dee = new D_EDIOrder_Entity
            {
                EDIOrderNO = detailControls[(int)EIndex.SyoriNO].Text
            };

            return dee;
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
                else if (ctl.GetType().Equals(typeof(Panel)))
                {
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex=-1;
                }
                else
                {
                    ctl.Text = "";
                }
            }

            foreach (Control ctl in detailLabels)
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }


            //初期値セット
            string ymd = mibl.GetDate();

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

            //[M_Store]
            M_Store_Entity mse2 = new M_Store_Entity
            {
                StoreCD = mse.StoreCD,
                ChangeDate = ymd
            };
            Store_BL sbl = new Store_BL();
            DataTable dt = sbl.M_Store_Select(mse2);
            if (dt.Rows.Count > 0)
            {
            }
            else
            {
                bbl.ShowMessage("E133");
                EndSec();
            }

            //出力対象
            RdoNotOutput.Checked = true;
            ScEDIOrderNO.Enabled = false;

            //発注日
            ckM_TextBox1.Text = ymd;
            ckM_TextBox2.Text = ymd;


            detailControls[0].Focus();
        }

        private void Scr_Lock(short no1, short no2, short Kbn)
        {
            short i;
            for (i = no1; i <= no2; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            // ｷｰ部
                            break;
                        }

                    case 1:
                        {
                            // ｷｰ部(複写)
                            break;
                        }

                    case 2:
                        {
                            break;
                        }

                    case 3:
                        {
                            // 明細部
                            foreach (Control ctl in detailControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            ScEDIOrderNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            ScVendor.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            ScStaff.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            ScOrderNO.BtnSearch.Enabled = Kbn == 0 ? true : false;

                            break;
                        }
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
                case 0: // F1:終了
                    {
                        break;
                    }
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会
                case 5:     //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        Scr_Clr(0);

                        break;
                    }

                case 11:   //F12:データ出力
                    break;
                    
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
                        if (detailControls.Length - 1 > index)
                        {
                            if(index== (int)EIndex.ChkMisyounin)
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                            else if (index == (int)EIndex.StoreCD)
                                this.ProcessTabKey(!e.Shift);
                            else if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                        }
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

        private void BtnSubF12_Click(object sender, EventArgs e)
        {
            //データ出力ボタンClick時   
            try
            {
                base.FunctionProcess(FuncExec - 1);

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

                int index = Array.IndexOf(detailControls, sender);
                switch(index)
                    {
                    case (int)EIndex.SyoriNO:
                    case (int)EIndex.Vendor:
                    case (int)EIndex.Staff:
                    case (int)EIndex.OrderNO:
                        F9Visible = true;
                        break;

                    default:
                        F9Visible = false;
                        break;
                }
                //SetFuncKeyAll(this, "111111000011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
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
                    int index = Array.IndexOf(detailRadios, sender);
                    switch (index)
                    {
                        case (int)RIndex.NotOutput:
                            detailControls[(int)EIndex.OrderDateFrom].Focus();
                            break;
                        case (int)RIndex.ReOutput:
                            detailControls[(int)EIndex.SyoriNO].Focus();
                            break;
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

        private void RdoReOutput_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //チェックがONのときのみ入力可               
                ScEDIOrderNO.Enabled = RdoReOutput.Checked;
                
                ckM_TextBox1.Enabled = !RdoReOutput.Checked;
                ckM_TextBox2.Enabled = !RdoReOutput.Checked;
                ScVendor.Enabled = !RdoReOutput.Checked;
                ScStaff.Enabled = !RdoReOutput.Checked;
                ScOrderNO.Enabled = !RdoReOutput.Checked;
                ChkMisyounin.Enabled = !RdoReOutput.Checked;

                if (RdoReOutput.Checked)
                {
                    ckM_TextBox1.Text = "";
                    ckM_TextBox2.Text = "";
                    ScVendor.TxtCode.Text = "";
                    ScStaff.TxtCode.Text = "";
                    ScOrderNO.TxtCode.Text = "";
                    ChkMisyounin.Checked = false;
                }
                else
                {
                    ScEDIOrderNO.TxtCode.Text = "";
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


    }
}








