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

namespace MasterTouroku_Tempo
{
    /// <summary>
    /// MasterTouroku_Tempo 店舗ストアマスタ
    /// </summary>
    internal partial class MasterTouroku_Tempo : FrmMainForm
    {
        private const string ProID = "MasterTouroku_Tempo";
        private const string ProNm = "店舗ストアマスタ";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            StoreCD,
            ChangeDate,
            StoreKBN = 0,
            StorePlaceKBN,
            StoreName,
            MallCD,
            APIKey,
            ZipCD1,
            ZipCD2,
            Address1,
            Address2,
            MailAddress1,
            //MailAddress2,
            //MailAddress3,
            TelphoneNO,
            FaxNO,
            KouzaCD,
            ReceiptPrint,
            ApprovalStaffCD11,
            ApprovalStaffCD12,
            ApprovalStaffCD21,
            ApprovalStaffCD22,
            ApprovalStaffCD31,
            ApprovalStaffCD32,
            DeliveryDate,
            PaymentTerms,
            DeliveryPlace,
            ValidityPeriod,
            Print1,
            Print2,
            Print3,
            Print4,
            Print5,
            Print6,
            MoveMailPatternCD,
            InvoiceNotation,
            Remarks,
            DeleteFlg
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Store,
            Mal,
            Kouza,
            Staff
        }
        private Control[] keyControls;
        private Control[] copyKeyControls;
        private Control[] detailControls;
        private Control[] detailLabels;
        //KTP 2019-06-05 handle from user control
        private Control[] searchButtons;

        private Store_BL mbl;
        private M_Store_Entity mse;

        private string mZipCD = "";

        public MasterTouroku_Tempo()
        {
            InitializeComponent();
        }

        private void MasterTouroku_Tempo_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                //base.InProgramNM = ProNm;
                //KTP 2019-06-05 set programID and Header text on main form 524,525
                //this.Text = base.InProgramID + " " + base.InProgramNM;
                //this.HeaderTitleText = base.InProgramNM;

                this.SetFunctionLabel(EProMode.MENTE);
                this.InitialControlArray();
                mbl = new Store_BL();

                //起動時共通処理
                base.StartProgram();

                M_MultiPorpose_Entity me = new M_MultiPorpose_Entity();
                me.ID = MultiPorpose_BL.ID_TempoKbn;

                Control[] ctls = {  radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9 };
                for (int i = 1; i <= 6; i++)
                {
                    ctls[i - 1].Visible = false;
                    me.Key = i.ToString();

                    MultiPorpose_BL bl = new MultiPorpose_BL();
                    DataTable dt = bl.M_MultiPorpose_Select(me);
                    if (dt.Rows.Count > 0)
                    {
                        ctls[i - 1].Visible = true;
                        ctls[i - 1].Text = bbl.LeftB(dt.Rows[0]["Char1"].ToString(),6);
                    }
                }

                ScStore.SetFocus(1);
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
            keyControls = new Control[] { ScStore.TxtCode,ScStore.TxtChangeDate };
            copyKeyControls = new Control[] { ScCopyStore.TxtCode,ScCopyStore.TxtChangeDate };
            detailControls = new Control[] { panel1,panel2, ckM_TextBox6, ScMall.TxtCode, ckM_TextBox20, ckM_TextBox2, ckM_TextBox1
                                    , ckM_TextBox4, ckM_TextBox7
                                    , ckM_TextBox21, ckM_TextBox22, ckM_TextBox23, ScKouza.TxtCode,ckM_TextBox19, ScStaff11.TxtCode
                                    , ScStaff12.TxtCode, ScStaff21.TxtCode, ScStaff22.TxtCode, ScStaff31.TxtCode, ScStaff32.TxtCode
                                    , ckM_TextBox18, ckM_TextBox17, ckM_TextBox16, ckM_TextBox15
                                    , ckM_TextBox8, ckM_TextBox9, ckM_TextBox11, ckM_TextBox10, ckM_TextBox13, ckM_TextBox12
                                    ,ScMailPatternCD.TxtCode, ckM_TextBox3, TxtRemark};
            detailLabels = new CKM_SearchControl[] { ScMall, ScKouza, ScStaff11, ScStaff12
                                , ScStaff21, ScStaff22, ScStaff31, ScStaff32, ScMailPatternCD};
            searchButtons = new Control[] { ScMall.BtnSearch,ScKouza.BtnSearch, ScStaff11.BtnSearch,ScStaff12.BtnSearch,ScStaff22.BtnSearch,
                ScStaff21.BtnSearch,ScStaff32.BtnSearch,ScStaff31.BtnSearch,ScMailPatternCD.BtnSearch,ScCopyStore.BtnSearch,ScStore.BtnSearch };
                //btnMalCD, btnKozCD, button3, button4, button5
                //                , button6, button7, button8, btnCopyStoreCD, btnStoreCD};

            //イベント付与
            foreach (Control ctl in keyControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyControl_KeyDown);
                //ctl.Enter += new System.EventHandler(KeyControl_Enter);
            }
            foreach (Control ctl in copyKeyControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(CopyKeyControl_KeyDown);
                //ctl.Enter += new System.EventHandler(CopyKeyControl_Enter);
            }
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                //ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

            radioButton1.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
            radioButton2.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
            radioButton3.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);

            radioButton1.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton2.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton3.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton4.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton5.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton6.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton7.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton8.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton9.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);

            //KTP handle from user control
            //foreach (Control ctl in searchButtons)
            //{
            //    ctl.Click += new System.EventHandler(BtnSearch_Click);
            //}
        }

        private bool CheckKey(int index)
        {
            return this.CheckKey(index, 0, true);
        }
        private bool CheckKey(int index, short kbn)
        {
            return this.CheckKey(index, kbn, true);
        }

        /// <summary>
        /// PrimaryKeyのコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="kbn">複写コードの場合:1に設定する</param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckKey(int index, short kbn, bool set)
        {
            if (kbn == 0)
            {
                switch (index)
                {
                    case (int)EIndex.StoreCD:
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (keyControls[index].Text == "")
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }

                        //if (!CheckAvailableStores(keyControls[index].Text))
                        //{ //Ｅ１０２
                        //    bbl.ShowMessage("E141");
                        //    return false;
                        //}

                        break;
                    case (int)EIndex.ChangeDate:
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (keyControls[index].Text == "")
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }

                        keyControls[index].Text = bbl.FormatDate(keyControls[index].Text);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(keyControls[index].Text))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }
                        break;
                }
            }
            else
            {
                //複写キーの場合
                switch (index)
                {
                    case (int)EIndex.StoreCD:
                        //入力なければチェックなし
                        if (copyKeyControls[index].Text == "")
                        {
                            return true;
                        }
                        else
                        {
                            //ストアマスタデータチェック
                        }
                        break;

                    case (int)EIndex.ChangeDate:
                        if (!CheckKey((int)EIndex.StoreCD))
                        {
                            keyControls[(int)EIndex.StoreCD].Focus();
                            return false;
                        }
                        if (!CheckKey(index))
                        {
                            keyControls[index].Focus();
                            return false;
                        }

                        //複写店舗ストアCDに入力がある場合、(When there is an input in 複写店舗ストアCD)Ｅ１０２
                        //必須入力
                        if (!string.IsNullOrWhiteSpace(copyKeyControls[(int)EIndex.StoreCD].Text) && string.IsNullOrWhiteSpace(copyKeyControls[index].Text))
                        {
                            copyKeyControls[index].Focus();
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                        else if (!string.IsNullOrWhiteSpace(copyKeyControls[index].Text))
                        {
                            copyKeyControls[index].Text = bbl.FormatDate(copyKeyControls[index].Text);

                            //日付として正しいこと(Be on the correct date)Ｅ１０３
                            if (!bbl.CheckDate(copyKeyControls[index].Text))
                            {
                                copyKeyControls[index].Focus();
                                bbl.ShowMessage("E103");
                                return false;
                            }
                        }
                        else
                        {
                            //両方とも未入力
                            return true;
                        }
                        break;
                }
            }

            if (index == (int)EIndex.ChangeDate)
            {
                //[M_Store_SelectData]
                mse = GetEntity(kbn);
                DataTable dtStore = mbl.Store_SelectData(mse);

                //・新規モードの場合（In the case of "new" mode）
                if (OperationMode == EOperationMode.INSERT && kbn == 0)
                {
                    //以下の条件で店舗マスター(M_Store)が存在すればエラー (Error if record exists)Ｅ１３２
                    if (dtStore.Rows.Count > 0)
                    {
                        bbl.ShowMessage("E132");
                        return false;
                    }
                    return true;
                }
                else
                {
                    //以下の条件で店舗マスター(M_Store)が存在しなければエラー (Error if record does not exist)Ｅ１３３
                    if (dtStore.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E133");
                        Scr_Clr(1);
                        return false;
                    }
                    else
                    {
                        //画面セットなしの場合、処理正常終了
                        if (set == false)
                        {
                            return true;
                        }

                        //Index調整用
                        int col = 2;

                        for (int i = (int)EIndex.StoreKBN; i <= (int)EIndex.DeleteFlg; i++)
                        {
                            if (i == (int)EIndex.StoreKBN)
                            {
                                //1:実店舗、2:WEB店、3:まとめ店舗
                                switch (dtStore.Rows[0][i + col].ToString())
                                {
                                    case "1":
                                        radioButton1.Checked = true;
                                        break;

                                    case "2":
                                        radioButton2.Checked = true;
                                        break;

                                    case "3":
                                        radioButton3.Checked = true;
                                        break;
                                }

                            }
                            else if (i == (int)EIndex.StorePlaceKBN)
                            {
                                //1:本社本店、2:豊中、3:石橋、4:江坂、5:三宮
                                switch (dtStore.Rows[0]["StorePlaceKBN"].ToString())//i + col
                                {
                                    case "1":
                                        radioButton4.Checked = true;
                                        break;

                                    case "2":
                                        radioButton5.Checked = true;
                                        break;

                                    case "3":
                                        radioButton6.Checked = true;
                                        break;

                                    case "4":
                                        radioButton7.Checked = true;
                                        break;

                                    case "5":
                                        radioButton8.Checked = true;
                                        break;
                                    case "6":
                                        radioButton9.Checked = true;
                                        break;
                                }

                            }
                            else if (i == (int)EIndex.DeleteFlg)
                            {
                                if (dtStore.Rows[0][i + col].ToString() == "1")
                                {
                                    //CheckBoxをONに
                                    checkDeleteFlg.Checked = true;
                                }
                            }
                            else
                            {
                                detailControls[i].Text = dtStore.Rows[0][i + col].ToString();
                            }

                            //名称ラベルに値をセット
                            ScMall.LabelText = dtStore.Rows[0]["MallNM"].ToString();
                            ScKouza.LabelText = dtStore.Rows[0]["KouzaName"].ToString();
                            ScStaff11.LabelText = dtStore.Rows[0]["ApprovalStaffNM11"].ToString();
                            ScStaff12.LabelText = dtStore.Rows[0]["ApprovalStaffNM12"].ToString();
                            ScStaff21.LabelText = dtStore.Rows[0]["ApprovalStaffNM21"].ToString();
                            ScStaff22.LabelText = dtStore.Rows[0]["ApprovalStaffNM22"].ToString();
                            ScStaff31.LabelText = dtStore.Rows[0]["ApprovalStaffNM31"].ToString();
                            ScStaff32.LabelText = dtStore.Rows[0]["ApprovalStaffNM32"].ToString();
                            ScMailPatternCD.LabelText = dtStore.Rows[0]["MailPatternName"].ToString();
                            mZipCD = dtStore.Rows[0]["ZipCD1"].ToString() + dtStore.Rows[0]["ZipCD2"].ToString();
                        }                    
                    }
                }

                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                {
                    //画面へデータセット後、明細部入力可、キー部入力不可
                    Scr_Lock(3, 3, 0);

                    Scr_Lock(0, 1, 1);
                    SetFuncKeyAll(this, "111111000001");
                    btnSubF11.Enabled = false;
                }
                else if (OperationMode == EOperationMode.DELETE)
                {
                    //Scr_Lock(1, 3, 1);
                    SetFuncKeyAll(this, "111111000011");
                }
                else if (OperationMode == EOperationMode.SHOW)
                {
                    SetFuncKeyAll(this, "111111000010");
                }

            }

            return true;

        }

        protected override void ExecDisp()
        {
            for (int i = 0; i < keyControls.Length; i++)
                if (CheckKey(i) == false)
                {
                    keyControls[i].Focus();
                    return;
                }
        }

        private bool CheckDetail(int index)
        {
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.StoreName:
                    //店舗ストア名 入力必須(Entry required)
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;

                case (int)EIndex.MallCD:
                    //モールCD 入力必須(Entry required)
                    if (detailControls[index].Enabled)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }
                   
                        //以下の条件でM_MultiPorposeが存在しない場合、エラー

                        //[M_MultiPorpose]
                        M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity
                        {
                            ID = MultiPorpose_BL.ID_MALL,
                            Key = detailControls[index].Text
                        };
                        MultiPorpose_BL mbl = new MultiPorpose_BL();
                        DataTable dt = mbl.M_MultiPorpose_Select(mme);
                        if (dt.Rows.Count > 0)
                        {
                            ScMall.LabelText = dt.Rows[0]["Char1"].ToString();
                        }
                        else
                        {
                            //Ｅ１０１
                            bbl.ShowMessage("E101");
                            ScMall.LabelText = "";
                            return false;
                        }
                    }
                    break;
                case (int)EIndex.APIKey:
                    if (detailControls[index].Enabled)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }
                        //既に他の店舗で入力されている値の場合、エラー（改定日として有効なレコードで違う店舗で同じAPIKeyがあればエラー）
                        M_Store_Entity me = new M_Store_Entity();
                        me.StoreCD = keyControls[(int)EIndex.StoreCD].Text;
                        me.ChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
                        me.APIKey = detailControls[(int)EIndex.APIKey].Text;
                        if (mbl.Store_SelectByApiKey(me))
                        {
                            //Ｅ１０５
                            bbl.ShowMessage("E105");
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.ZipCD1:
                case (int)EIndex.ZipCD2:
                    //郵便番号1、2 入力必須(Entry required)
                    if (detailControls[index].Enabled )
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }

                        //以下の条件でM_ZipCodeが存在する場合、
                        //[M_ZipCode]
                        M_ZipCode_Entity mze = new M_ZipCode_Entity
                        {
                            ZipCD1 = detailControls[(int)EIndex.ZipCD1].Text,
                            ZipCD2 = detailControls[(int)EIndex.ZipCD2].Text
                        };
                        ZipCode_BL zbl = new ZipCode_BL();
                        //bool ret = zbl.M_ZipCode_Select(mze);
                        DataTable dt = zbl.M_ZipCode_Select(mze);
                        if (dt.Rows.Count > 0 && (detailControls[(int)EIndex.Address1].Text == "" || mZipCD != mze.ZipCD1 + mze.ZipCD2))
                        {
                            detailControls[(int)EIndex.Address1].Text = dt.Rows[0]["Address1"].ToString();   //住所１
                            detailControls[(int)EIndex.Address2].Text = dt.Rows[0]["Address2"].ToString();  //住所２
                        }
                        else
                        {
                            //存在しない場合でもエラーとはしない(It is not an error that record does not exist)
                        }
                        mZipCD = mze.ZipCD1 + mze.ZipCD2;
                    }
                    break;

                case (int)EIndex.Address1:
                case (int)EIndex.TelphoneNO:
                case (int)EIndex.FaxNO:
                    //住所 入力必須(Entry required)
                    if (detailControls[index].Enabled)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.KouzaCD:
                    //口座CD 入力必須(Entry required)
                    if (detailControls[index].Enabled)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }

                        //以下の条件でM_Kouzaが存在しない場合、エラー

                        //[M_Kouza]
                        M_Kouza_Entity mke = new M_Kouza_Entity
                        {
                            KouzaCD = detailControls[index].Text,
                            ChangeDate = keyControls[(int)EIndex.ChangeDate].Text
                        };
                        Kouza_BL kbl = new Kouza_BL();
                        DataTable dt = kbl.M_Kouza_Select(mke);
                        if (dt.Rows.Count > 0)
                        {
                            ScKouza.LabelText = dt.Rows[0]["KouzaName"].ToString();

                            //DeleteFlg = 1の場合、エラー
                            if (dt.Rows[0]["DeleteFlg"].ToString() == "1")
                            {
                                //Ｅ１５８
                                bbl.ShowMessage("E158");
                                return false;
                            }
                        }
                        else
                        {
                            //Ｅ１０１
                            ScKouza.LabelText = "";
                            bbl.ShowMessage("E101");
                            return false;
                        }
                    }
                    break;
                case (int)EIndex.ReceiptPrint:
                    //入力必須(Entry required)
                    if (detailControls[index].Enabled)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }
                    }
                    break;
                case (int)EIndex.MoveMailPatternCD:
                    //入力必須(Entry required)
                    if (detailControls[index].Enabled )
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }
                        //以下の条件でM_MailPatternが存在しない場合、エラー

                        //[M_MailPattern]
                        M_MailPattern_Entity mme = new M_MailPattern_Entity
                        {
                            MailPatternCD = detailControls[index].Text
                        };
                        MailPattern_BL kbl = new MailPattern_BL();
                      bool ret = kbl.M_MailPattern_Select(mme);
                        if (ret)
                        {
                            ScMailPatternCD.LabelText = mme.MailPatternName;
                        }
                        else
                        {
                            //Ｅ１０１
                            ScMailPatternCD.LabelText = "";
                            bbl.ShowMessage("E101");
                            return false;
                        }
                    }
                    break;
                case (int)EIndex.ApprovalStaffCD11:
                case (int)EIndex.ApprovalStaffCD12:
                case (int)EIndex.ApprovalStaffCD21:
                case (int)EIndex.ApprovalStaffCD22:
                case (int)EIndex.ApprovalStaffCD31:
                case (int)EIndex.ApprovalStaffCD32:
                    //一次承認スタッフCD 入力必須(Entry required)
                    if (index == (int)EIndex.ApprovalStaffCD11 && detailControls[index].Enabled && detailControls[index].Text == "")
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    else if (detailControls[index].Text != "")
                    {
                        //以下の条件でM_Staffが存在しない場合、エラー

                        //[M_Staff]
                        M_Staff_Entity mse = new M_Staff_Entity
                        {
                            StaffCD = detailControls[index].Text,
                            ChangeDate = keyControls[(int)EIndex.ChangeDate].Text
                        };
                        Staff_BL mbl = new Staff_BL();
                        bool ret = mbl.M_Staff_Select(mse);
                        if (ret)
                        {
                            if (index == (int)EIndex.ApprovalStaffCD11)
                                ScStaff11.LabelText = mse.StaffName;
                            else if (index == (int)EIndex.ApprovalStaffCD12)
                                ScStaff12.LabelText = mse.StaffName;
                            else if (index == (int)EIndex.ApprovalStaffCD21)
                                ScStaff21.LabelText = mse.StaffName;
                            else if (index == (int)EIndex.ApprovalStaffCD22)
                                ScStaff22.LabelText = mse.StaffName;
                            else if (index == (int)EIndex.ApprovalStaffCD31)
                                ScStaff31.LabelText = mse.StaffName;
                            else if (index == (int)EIndex.ApprovalStaffCD32)
                                ScStaff32.LabelText = mse.StaffName;

                            //以下の条件を満たせばエラー
                            if (mse.LeaveDate != "")
                            {
                                //if ( mse.LeaveDate <= keyControls[(int)EIndex.ChangeDate].Text)
                                int result = mse.LeaveDate.CompareTo(keyControls[(int)EIndex.ChangeDate].Text);
                                if (result <= 0)
                                {
                                    //Ｅ１３５
                                    bbl.ShowMessage("E135");
                                    return false;
                                }
                            }

                            //DeleteFlg = 1の場合、エラー
                            if (mse.DeleteFlg == "1")
                            {
                                //Ｅ１５８
                                bbl.ShowMessage("E158");
                                return false;
                            }
                        }
                        else
                        {
                            //Ｅ１０１
                            bbl.ShowMessage("E101");

                            if (index == (int)EIndex.ApprovalStaffCD11)
                                ScStaff11.LabelText = "";
                            else if (index == (int)EIndex.ApprovalStaffCD12)
                                ScStaff12.LabelText = "";
                            else if (index == (int)EIndex.ApprovalStaffCD21)
                                ScStaff21.LabelText = "";
                            else if (index == (int)EIndex.ApprovalStaffCD22)
                                ScStaff22.LabelText = "";
                            else if (index == (int)EIndex.ApprovalStaffCD31)
                                ScStaff31.LabelText = "";
                            else if (index == (int)EIndex.ApprovalStaffCD32)
                                ScStaff32.LabelText = "";

                            return false;
                        }

                        //重複チェック
                        for (int i = (int)EIndex.ApprovalStaffCD11; i < index; i++)
                        {
                            for (int j = i + 1; j <= index; j++)
                            {
                                if (detailControls[i].Text != "" && detailControls[j].Text != "")
                                    if (detailControls[i].Text == detailControls[j].Text)
                                    {
                                        //Ｅ１０５
                                        bbl.ShowMessage("E105");
                                        return false;
                                    }
                            }
                        }
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <param name="kbn"></param>
        /// <returns></returns>
        private M_Store_Entity GetEntity(short kbn)
        {
            mse = new M_Store_Entity();

            if (kbn == 0)
            {
                mse.StoreCD = keyControls[(int)EIndex.StoreCD].Text;
                mse.ChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
            }
            else
            {
                mse.StoreCD = copyKeyControls[(int)EIndex.StoreCD].Text;
                mse.ChangeDate = copyKeyControls[(int)EIndex.ChangeDate].Text;
                return mse;
            }

            if (radioButton1.Checked)
            {
                mse.StoreKBN = "1";
                if (radioButton4.Checked)
                    mse.StorePlaceKBN = "1";
                else if (radioButton5.Checked)
                    mse.StorePlaceKBN = "2";
                else if (radioButton6.Checked)
                    mse.StorePlaceKBN = "3";
                else if (radioButton7.Checked)
                    mse.StorePlaceKBN = "4";
                else if (radioButton8.Checked)
                    mse.StorePlaceKBN = "5";
                else if (radioButton9.Checked)
                    mse.StorePlaceKBN = "6";
            }
            else if (radioButton2.Checked)
            {
                mse.StoreKBN = "2";
                mse.StorePlaceKBN = "0";
            }
            else if (radioButton3.Checked)
            {
                mse.StoreKBN = "3";
                mse.StorePlaceKBN = "0";
            }


            //Index調整用
            int index = 0;

            mse.StoreName = detailControls[index + (int)EIndex.StoreName].Text;
            mse.MallCD = detailControls[index + (int)EIndex.MallCD].Text;
            mse.APIKey = bbl.Z_SetStr( detailControls[index + (int)EIndex.APIKey].Text);

            mse.ZipCD1 = detailControls[index + (int)EIndex.ZipCD1].Text;
            mse.ZipCD2 = detailControls[index + (int)EIndex.ZipCD2].Text;
            mse.Address1 = detailControls[index + (int)EIndex.Address1].Text;
            mse.Address2 = detailControls[index + (int)EIndex.Address2].Text;
            mse.TelphoneNO = detailControls[index + (int)EIndex.TelphoneNO].Text;
            mse.FaxNO = detailControls[index + (int)EIndex.FaxNO].Text;
            mse.MailAddress1 = detailControls[index + (int)EIndex.MailAddress1].Text;
            mse.ApprovalStaffCD11 = detailControls[index + (int)EIndex.ApprovalStaffCD11].Text;
            mse.ApprovalStaffCD12 = detailControls[index + (int)EIndex.ApprovalStaffCD12].Text;
            mse.ApprovalStaffCD21 = detailControls[index + (int)EIndex.ApprovalStaffCD21].Text;
            mse.ApprovalStaffCD22 = detailControls[index + (int)EIndex.ApprovalStaffCD22].Text;
            mse.ApprovalStaffCD31 = detailControls[index + (int)EIndex.ApprovalStaffCD31].Text;
            mse.ApprovalStaffCD32 = detailControls[index + (int)EIndex.ApprovalStaffCD32].Text;
            mse.DeliveryDate = detailControls[index + (int)EIndex.DeliveryDate].Text;
            mse.PaymentTerms = detailControls[index + (int)EIndex.PaymentTerms].Text;
            mse.DeliveryPlace = detailControls[index + (int)EIndex.DeliveryPlace].Text;
            mse.ValidityPeriod = detailControls[index + (int)EIndex.ValidityPeriod].Text;
            mse.Print1 = detailControls[index + (int)EIndex.Print1].Text;
            mse.Print2 = detailControls[index + (int)EIndex.Print2].Text;
            mse.Print3 = detailControls[index + (int)EIndex.Print3].Text;
            mse.Print4 = detailControls[index + (int)EIndex.Print4].Text;
            mse.Print5 = detailControls[index + (int)EIndex.Print5].Text;
            mse.Print6 = detailControls[index + (int)EIndex.Print6].Text;
            mse.KouzaCD = detailControls[index + (int)EIndex.KouzaCD].Text;
            mse.ReceiptPrint = detailControls[index + (int)EIndex.ReceiptPrint].Text;
            mse.MoveMailPatternCD = detailControls[index + (int)EIndex.MoveMailPatternCD].Text;
            mse.InvoiceNotation = detailControls[index + (int)EIndex.InvoiceNotation].Text;
            mse.Remarks = detailControls[index + (int)EIndex.Remarks].Text;
            //チェックボックス
            if (checkDeleteFlg.Checked)
                mse.DeleteFlg = "1";
            else
                mse.DeleteFlg = "0";

            mse.UsedFlg = "0";
            //mse.InsertOperator = this.gOpeCD;
            //mse.UpdateOperator = this.gOpeCD;

            return mse;
        }

        protected override void ExecSec()
        {
            for (int i = 0; i < keyControls.Length; i++)
                if (CheckKey(i, 0, false) == false)
                {
                    keyControls[i].Focus();
                    return;
                }

            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            Control[] ctls = { radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9 };
            for (int i = 1; i <= 6; i++)
            {
                if (((RadioButton)ctls[i - 1]).Checked)
                {
                    if (!ctls[i - 1].Visible)
                    {
                        bbl.ShowMessage("E111");
                        ctls[0].Focus();
                        return;
                    }
                }
            }

            //更新処理
            mse = GetEntity(0);
            mbl.Store_Exec(mse, (short)OperationMode, InOperatorCD, InPcID);

            //更新後画面クリア
            InitScr();

            if(OperationMode== EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");
        }

        /// <summary>
        /// 検索フォーム起動処理
        /// </summary>
        /// <param name="kbn"></param>
        /// <param name="setCtl"></param>
        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            switch (kbn)
            {
                case EsearchKbn.Store:
                    using (Search_Store frmStore = new Search_Store())
                    {
                        frmStore.parChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
                        frmStore.ShowDialog();

                        if (!frmStore.flgCancel)
                        {
                            setCtl.Text = frmStore.parStoreCD;
                            if (setCtl == keyControls[(int)EIndex.StoreCD])
                                keyControls[(int)EIndex.ChangeDate].Text = frmStore.parChangeDate;
                            else
                                copyKeyControls[(int)EIndex.ChangeDate].Text = frmStore.parChangeDate;
                        }
                    }
                    break;

                case EsearchKbn.Mal:
                    using (Search_Mall frmMal = new Search_Mall())
                    {
                        frmMal.parID = MultiPorpose_BL.ID_MALL;
                        frmMal.ShowDialog();
                        if (!frmMal.flgCancel)
                            setCtl.Text = frmMal.parKey;
                    }
                    break;

                case EsearchKbn.Kouza:
                    using (Search_Kouza frmKouza = new Search_Kouza())
                    {
                        frmKouza.parChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
                        frmKouza.ShowDialog();
                        if (!frmKouza.flgCancel)
                            setCtl.Text = frmKouza.parKouzaCD;
                    }
                    break;

                case EsearchKbn.Staff:
                    using (Search_Staff frmStaff = new Search_Staff())
                    {
                        frmStaff.parChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
                        frmStaff.ShowDialog();
                        if (!frmStaff.flgCancel)
                            setCtl.Text = frmStaff.parStaffCD;
                    }
                    break;

            }

        }

        /// <summary>
        /// 画面の初期化
        /// </summary>
        private void InitScr()
        {
            ChangeOperationMode(base.OperationMode);
            Scr_Clr(0);
        }

        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //if (OldOperationMode != OperationMode)
            //{
                Scr_Clr(0);
            //}

            switch (mode)
            {
                case EOperationMode.INSERT:
                    Scr_Lock(0, mc_L_END, 0);
                    SetFuncKeyAll(this, "111111000001");
                    btnSubF11.Enabled = false;
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:

                    Scr_Lock(0, 0, 0);
                    SetFuncKeyAll(this, "111111001010");
                    btnSubF11.Enabled = true;
                    Scr_Lock(1, mc_L_END, 1);
                    break;

            }

            keyControls[0].Focus(); //店舗ストアCD
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            if (Kbn == 0)
            {
                foreach (Control ctl in keyControls)
                {
                    ctl.Text = "";
                }

                foreach (Control ctl in copyKeyControls)
                {
                    ctl.Text = "";
                }
                
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(RadioButton)))
                {
                    ((RadioButton)ctl).Checked = true;
                }
                else if (ctl.GetType().Equals(typeof(Panel)))
                {
                    radioButton1.Checked = true;
                    radioButton4.Checked = true;
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
            mZipCD = "";
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
                            foreach (Control ctl in keyControls)
                            {
                                ctl.Parent.Enabled = Kbn == 0 ? true : false;
                            }
                            if (base.OperationMode != EOperationMode.INSERT)
                                ScStore.SearchEnable = Kbn == 0 ? true : false;
                            else
                            {
                                ScStore.SearchEnable = false;
                                F9Visible = false;
                            }
                                
                                
                            break;
                        }

                    case 1:
                        {
                            // ｷｰ部(複写)
                            foreach (Control ctl in copyKeyControls)
                            {
                                ctl.Parent.Enabled = Kbn == 0 ? true : false;
                            }
                            ScCopyStore.SearchEnable = Kbn == 0 ? true : false;
                            break;
                        }

                    case 2:
                        {
                            //Fla_HEAD(0).Enabled = Interaction.IIf(Kbn == 0, true, false); // HEAD部
                            break;
                        }

                    case 3:
                        {
                            // 明細部
                            foreach (Control ctl in detailControls)
                            {
                                if (ctl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                                    ctl.Parent.Enabled = Kbn == 0 ? true : false;
                                else
                                    ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            for (int index = 0; index < searchButtons.Length - 2; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;

                            checkDeleteFlg.Enabled = Kbn == 0 ? true : false;

                            if(Kbn.Equals(0))
                                SetEnabled();

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
                    {
                        ChangeOperationMode((EOperationMode)Index);

                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        InitScr();

                        break;
                    }

                //KTP 2019-06-05 F9 button handle from user control
                //case 8: //F9:検索
                //    EsearchKbn kbn = EsearchKbn.Null;

                //    if (Array.IndexOf(keyControls, previousCtrl) == (int)EIndex.StoreCD ||
                //        Array.IndexOf(copyKeyControls, previousCtrl) == (int)EIndex.StoreCD)
                //    {
                //        //店舗検索
                //        kbn = EsearchKbn.Store;
                //    }
                //    else if (Array.IndexOf(detailControls, previousCtrl) == (int)EIndex.MallCD)
                //    {
                //        //汎用検索
                //        kbn = EsearchKbn.Mal;
                //    }
                //    else if (Array.IndexOf(detailControls, previousCtrl) == (int)EIndex.ApprovalStaffCD11 ||
                //        Array.IndexOf(copyKeyControls, previousCtrl) == (int)EIndex.ApprovalStaffCD12 ||
                //        Array.IndexOf(copyKeyControls, previousCtrl) == (int)EIndex.ApprovalStaffCD21 ||
                //        Array.IndexOf(copyKeyControls, previousCtrl) == (int)EIndex.ApprovalStaffCD22 ||
                //        Array.IndexOf(copyKeyControls, previousCtrl) == (int)EIndex.ApprovalStaffCD31 ||
                //        Array.IndexOf(copyKeyControls, previousCtrl) == (int)EIndex.ApprovalStaffCD32)
                //    {
                //        //スタッフ検索
                //        kbn = EsearchKbn.Staff;
                //    }
                //    else if (Array.IndexOf(detailControls, previousCtrl) == (int)EIndex.KouzaCD)
                //    {
                //        //口座検索
                //        kbn = EsearchKbn.Kouza;
                //    }

                //    if (kbn != EsearchKbn.Null)
                //        SearchData(kbn, previousCtrl);

                //    break;

                case 11:    //F12:登録
                    {
                        if (OperationMode == EOperationMode.DELETE)
                        { //Ｑ１０２		
                            if (bbl.ShowMessage("Q102") != DialogResult.Yes)
                                return;
                        }
                        else
                        {
                            //Ｑ１０１		
                            if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                                return;
                        }


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
        private void KeyControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(keyControls, sender);
                    bool ret = CheckKey(index);

                    if (ret)
                    {
                        switch (index)
                        {
                            case (int)EIndex.StoreCD:
                                keyControls[(int)EIndex.ChangeDate].Focus();
                                break;

                            case (int)EIndex.ChangeDate:
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                {
                                    SetEnabled();

                                    if (OperationMode == EOperationMode.INSERT)
                                        copyKeyControls[(int)EIndex.StoreCD].Focus();
                                    else
                                    {
                                        if (radioButton1.Checked)
                                            radioButton1.Focus();
                                        else if (radioButton2.Checked)
                                            radioButton2.Focus();
                                        else if (radioButton3.Checked)
                                            radioButton3.Focus();
                                    }
                                }
                                else
                                {
                                    btnSubF11.Focus();
                                }
                                break;

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

        private void CopyKeyControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(copyKeyControls, sender);
                    bool ret = CheckKey(Array.IndexOf(copyKeyControls, sender), 1);

                    if (ret)
                    {
                        switch (index)
                        {
                            case (int)EIndex.StoreCD:
                                copyKeyControls[(int)EIndex.ChangeDate].Focus();
                                break;

                            case (int)EIndex.ChangeDate:
                                if (OperationMode == EOperationMode.INSERT)
                                {
                                    if (radioButton1.Checked)
                                        radioButton1.Focus();
                                    else if (radioButton2.Checked)
                                        radioButton2.Focus();
                                    else if (radioButton3.Checked)
                                        radioButton3.Focus();
                                }
                                break;

                        }
                    }
                    else
                    {
                        //((Control)sender).Focus();
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
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else if (index.Equals((int)EIndex.StoreName))
                                //カーソルが消えるのを回避
                                if (detailControls[index + 3].CanFocus)
                                    detailControls[index + 3].Focus();
                                else
                                    this.ProcessTabKey(!e.Shift);
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
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    EsearchKbn kbn = EsearchKbn.Null;
            //    Control setCtl = null;

            //    //検索ボタンClick時
            //    if (((Button)sender).Name == ScStore.Name || ((Button)sender).Name == btnCopyStoreCD.Name)
            //    {
            //        //店舗検索
            //        kbn = EsearchKbn.Store;

            //        if (((Button)sender).Name == ScStore.Name)
            //            setCtl = keyControls[(int)EIndex.StoreCD];
            //        else
            //            setCtl = copyKeyControls[(int)EIndex.StoreCD];
            //    }
            //    else if (((Button)sender).Name == btnKozCD.Name)
            //    {
            //        //口座検索
            //        kbn = EsearchKbn.Kouza;

            //        setCtl = detailControls[(int)EIndex.KouzaCD];
            //    }
            //    else if (((Button)sender).Name == btnMalCD.Name)
            //    {
            //        //モール検索
            //        kbn = EsearchKbn.Mal;

            //        setCtl = detailControls[(int)EIndex.MallCD];
            //    }
            //    else if (((Button)sender).Name == button3.Name || ((Button)sender).Name == button4.Name ||
            //            ((Button)sender).Name == button5.Name || ((Button)sender).Name == button6.Name ||
            //            ((Button)sender).Name == button7.Name || ((Button)sender).Name == button8.Name)
            //    {
            //        //スタッフ検索
            //        kbn = EsearchKbn.Staff;

            //        if (((Button)sender).Name == button3.Name)
            //            setCtl = detailControls[(int)EIndex.ApprovalStaffCD11];
            //        else if (((Button)sender).Name == button4.Name)
            //            setCtl = detailControls[(int)EIndex.ApprovalStaffCD12];
            //        else if (((Button)sender).Name == button6.Name)
            //            setCtl = detailControls[(int)EIndex.ApprovalStaffCD21];
            //        else if (((Button)sender).Name == button5.Name)
            //            setCtl = detailControls[(int)EIndex.ApprovalStaffCD22];
            //        else if (((Button)sender).Name == button8.Name)
            //            setCtl = detailControls[(int)EIndex.ApprovalStaffCD21];
            //        else
            //            setCtl = detailControls[(int)EIndex.ApprovalStaffCD22];
            //    }

            //    if (kbn != EsearchKbn.Null)
            //        SearchData(kbn, setCtl);

            //}
            //catch (Exception ex)
            //{
            //    //エラー時共通処理
            //    MessageBox.Show(ex.Message);
            //}
        }

        //private void KeyControl_Enter(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        previousCtrl = this.ActiveControl;

        //        if (OperationMode != EOperationMode.INSERT && Array.IndexOf(keyControls, sender) == (int)EIndex.StoreCD)
        //        {
        //            //SetFuncKey(this, 8, true);
        //        }
        //        else
        //        {
        //            //SetFuncKey(this, 8, false);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void CopyKeyControl_Enter(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        previousCtrl = this.ActiveControl;

        //        //if (Array.IndexOf(copyKeyControls, sender) == (int)EIndex.StoreCD)
        //        //    SetFuncKey(this, 8, true);

        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void DetailControl_Enter(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        previousCtrl = this.ActiveControl;

        //        //SetFuncKey(this, 8, false);

        //        switch (Array.IndexOf(detailControls, sender))
        //        {
        //            case (int)EIndex.MallCD:
        //            case (int)EIndex.ApprovalStaffCD11:
        //            case (int)EIndex.ApprovalStaffCD12:
        //            case (int)EIndex.ApprovalStaffCD21:
        //            case (int)EIndex.ApprovalStaffCD22:
        //            case (int)EIndex.ApprovalStaffCD31:
        //            case (int)EIndex.ApprovalStaffCD32:
        //            case (int)EIndex.KouzaCD:

        //                //SetFuncKey(this, 8, true);
        //                break;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetEnabled();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void SetEnabled()
        {
            if (base.OperationMode == EOperationMode.INSERT || base.OperationMode == EOperationMode.UPDATE)
            {

                if (radioButton1.Checked)
                {
                    //実店舗場所：店舗区分＝「実店舗」の時のみ入力可能。
                    panel2.Enabled = true;
                    detailControls[(int)EIndex.ReceiptPrint].Enabled = true;
                    detailControls[(int)EIndex.ReceiptPrint].Text = "";
                    ScMailPatternCD.Enabled = true;
                    detailControls[(int)EIndex.InvoiceNotation].Enabled = true;
                    detailControls[(int)EIndex.InvoiceNotation].Text = "";

                    //実店舗を選択した場合(When 実店舗 is selected)、以下の項目を入力不可にする (Input is possible)


                    //実店舗場所
                    //郵便番号
                    //住所
                    //メールアドレス
                    //電話番号
                    //FAX番号
                    //銀行口座CD
                    //【発注承認】の全て
                    //【見積初期値】の全て
                    //【伝票住所表記】の全て
                    for (int i = (int)EIndex.ZipCD1; i <= (int)EIndex.Print6; i++)
                    {
                        if (detailControls[i].Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                            detailControls[i].Parent.Enabled = true;
                        else
                            detailControls[i].Enabled = true;
                    }

                    for (int index = 0; index < searchButtons.Length - 2; index++)
                        searchButtons[index].Enabled = true;

                }
                else
                {
                    panel2.Enabled = false;
                    detailControls[(int)EIndex.ReceiptPrint].Enabled = false;
                    ScMailPatternCD.Enabled = false;
                    detailControls[(int)EIndex.InvoiceNotation].Enabled = false;

                    //Web店舗、Webまとめ店舗を選択した場合(When Web店舗 or Webまとめ店舗 is selected)、以下の項目を入力不可にする
                    //実店舗場所
                    //郵便番号
                    //住所
                    //メールアドレス
                    //電話番号
                    //FAX番号
                    //銀行口座CD
                    //【発注承認】の全て
                    //【見積初期値】の全て
                    //【伝票住所表記】の全て
                    for (int i = (int)EIndex.ZipCD1; i <= (int)EIndex.Print6; i++)
                    {
                        if (detailControls[i].Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                            detailControls[i].Parent.Enabled = false;
                        else
                            detailControls[i].Enabled = false;
                        detailControls[i].Text = "";
                    }

                    for (int index = 0; index < searchButtons.Length - 2; index++)
                        searchButtons[index].Enabled = false;

                    for (int index = 1; index < detailLabels.Length; index++)
                        ((CKM_SearchControl)detailLabels[index]).LabelText = "";

                }

                //Web店舗を選択した場合に入力可能にする。
                if (radioButton2.Checked)
                {
                    //モールCD
                    ScMall.Enabled = true;
                    detailControls[(int)EIndex.APIKey].Enabled = true;
                    ScMall.SearchEnable = true;   //モール検索
                }
                else
                {
                    //モールCD
                    ScMall.Enabled = false;
                    detailControls[(int)EIndex.MallCD].Text = "";
                    ScMall.LabelText = "";
                    ScMall.SearchEnable = false;   //モール検索

                    detailControls[(int)EIndex.APIKey].Enabled = false;
                    detailControls[(int)EIndex.APIKey].Text = "";
                }
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
                    //店舗区分
                    if (sender.Equals(radioButton1))
                    {
                        if (radioButton4.Checked)
                            radioButton4.Focus();
                        else if (radioButton5.Checked)
                            radioButton5.Focus();
                        else if (radioButton6.Checked)
                            radioButton6.Focus();
                        else if (radioButton7.Checked)
                            radioButton7.Focus();
                        else if (radioButton8.Checked)
                            radioButton8.Focus();
                        else if (radioButton9.Checked)
                            radioButton9.Focus();

                    }
                    else
                    {
                        detailControls[(int)EIndex.StoreName].Focus();
                    }

                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        private void ScStore_Leave(object sender, EventArgs e)
        {
            ScMall.ChangeDate = ScStore.ChangeDate;
            ScKouza.ChangeDate = ScStore.ChangeDate;
            ScStaff11.ChangeDate = ScStore.ChangeDate;
            ScStaff12.ChangeDate = ScStore.ChangeDate;
            ScStaff21.ChangeDate = ScStore.ChangeDate;
            ScStaff22.ChangeDate = ScStore.ChangeDate;
            ScStaff31.ChangeDate = ScStore.ChangeDate;
            ScStaff32.ChangeDate = ScStore.ChangeDate;
        }
    }
}
