using System;
using System.Data;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;
using Search;

namespace MasterTouroku_Tokuisaki
{
    /// <summary>
    /// MasterTouroku_Tokuisaki 得意先マスタ
    /// </summary>
    internal partial class MasterTouroku_Tokuisaki : FrmMainForm
    {
        private const string ProID = "MasterTouroku_Tokuisaki";
        private const string ProNm = "得意先マスタ";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            CustomerCD,
            ChangeDate,

            ChkVariousFLG = 0
            , ChkBillingFLG
            , ChkCollectFLG
            , pnlCustomerKBN
            , LastName
            , FirstName
            , CustomerName
            , pnlAliasKBN
            , KanaName
            , LongName1
            , LongName2
            , BirthDate
            , pnlSex
            , ChkCountryKBN
            , CountryName
            , ZipCD1
            , ZipCD2
            , ChkDMFlg
            , Address1
            , Address2
            , Tel11
            , Tel12
            , Tel13
            , Tel21
            , Tel22
            , Tel23
            , MailAddress
            , pnlBillingType
            , BillingCD
            , CollectCD
            , BillingCloseDate
            , cmbCollectPlanMonth
            , CollectPlanDate
            , pnlHolidayKBN
            , RegisteredNumber
            , ChkNoInvoiceFlg
            , pnlTaxPrintKBN
            , cmbTaxTiming
            , cmbTaxFractionKBN
            , cmbAmountFractionKBN
            , cmbPaymentMethodCD
            , KouzaCD
            , cmbPaymentUnit
            , cmbStoreTankaKBN
            , TankaCD
            , ChkAttentionFLG
            , ChkConfirmFLG
            , ConfirmComment
            , cmbCreditLevel
            , CreditCard
            , CreditInsurance
            , CreditDeposit
            , CreditETC
            //, CreditWarningAmount
            , CreditAdditionAmount
            , DisplayOrder
            , AnalyzeCD1
            , AnalyzeCD2
            , AnalyzeCD3
           // , CboStoreCD
            , ChkPointFLG
            , LastPoint
            , WaitingPoint
            , TotalPoint
            , RemarksOutStore
            , RemarksInStore

            , CboStoreCD
            , StaffCD
            , DeleteFlg
            , txtCreditCheckKBN
            , COUNT

            //Label
            , lblKouzaCD
            , lblBillingCD
            , lblCollectCD
            , lblTankaCD
            , lblStaff
            , lblStoreName
            , lblLastSalesDate
            , lblPoint
            , lblMinyukin
            , lblKensu
            , lblCreditAmount
        }
        private Control[] keyControls;
        private Control[] copyKeyControls;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private Customer_BL mbl;
        private M_Customer_Entity mce;

        //private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        public MasterTouroku_Tokuisaki()
        {
            InitializeComponent();
        }

        private void MasterTouroku_Tokuisaki_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                this.SetFunctionLabel(EProMode.MENTE);
                this.InitialControlArray();
                mbl = new Customer_BL();

                //起動時共通処理
                base.StartProgram();

                ////スタッフマスター(M_Staff)に存在すること
                ////[M_Staff]
                //M_Staff_Entity mse = new M_Staff_Entity
                //{
                //    StaffCD = InOperatorCD,
                //    ChangeDate = mbl.GetDate()
                //};
                //Staff_BL bl = new Staff_BL();
                //bool ret = bl.M_Staff_Select(mse);
                //if (ret)
                //{
                //    CboStoreCD.SelectedValue = mse.StoreCD;
                //}

                //	パラメータ	基準日：Form.日付
                ScCustomer.Value1 = "1";
                ScCustomer.Value2 = "";// mse.StoreCD;
                ScCopyCustomer.Value1 = "1";
                ScCopyCustomer.Value2 = ""; //mse.StoreCD;
                ScBillingCD.Value1 = "2";   //2:請求先
                ScBillingCD.Value2 = ""; //mse.StoreCD;
                ScCollectCD.Value1 = "3";   //3:入金請求先
                ScCollectCD.Value2 = ""; //mse.StoreCD;

                string ymd = bbl.GetDate();
                CboStoreCD.Bind(ymd);
                Bind(cmbPaymentMethodCD);
                BindCombo("KBN", "Name");

                ScCustomer.SetFocus(1);
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

            DataTable dt = dbl.M_Denomination_cboSelect(me, kbn);
            BindCombo(combo, "DenominationCD", "DenominationName", dt);
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

        private void BindCombo(string key, string value)
        {
            //1:明細ごと 2:伝票ごと 3:締ごと
            DataTable dt = new DataTable();
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);
            DataRow datarow = dt.NewRow();
            datarow[key] = "1";
            datarow[value] = "売上明細単位";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "売上伝票単位";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "3";
            datarow[value] = "請求書単位";
            dt.Rows.Add(datarow);

            BindCombo(cmbTaxTiming, key, value, dt);

            //1:切捨て 2:四捨五入 3:切上げ
            dt = new DataTable();
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);

            datarow = dt.NewRow();
            datarow[key] = "1";
            datarow[value] = "切捨て";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "四捨五入";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "3";
            datarow[value] = "切上げ";
            dt.Rows.Add(datarow);
            DataTable dt2 = dt.Copy();

            BindCombo(cmbTaxFractionKBN, key, value, dt);

            BindCombo(cmbAmountFractionKBN, key, value, dt2);

            //1:請求,2:売上,3:明細,4:先入
            dt = new DataTable();
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);

            datarow = dt.NewRow();
            datarow[key] = "1";
            datarow[value] = "請求";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "売上";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "3";
            datarow[value] = "売上明細";
            dt.Rows.Add(datarow);

            //datarow = dt.NewRow();
            //datarow[key] = "4";
            //datarow[value] = "先入";
            //dt.Rows.Add(datarow);

            BindCombo(cmbPaymentUnit, key, value, dt);

            //0:当月、1:翌月
            dt = new DataTable();
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);

            datarow = dt.NewRow();
            datarow[key] = "0";
            datarow[value] = "当月";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "1";
            datarow[value] = "翌月";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "翌々月";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "3";
            datarow[value] = "3ヶ月後";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "4";
            datarow[value] = "4ヶ月後";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "5";
            datarow[value] = "5ヶ月後";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "6";
            datarow[value] = "6ヶ月後";
            dt.Rows.Add(datarow);

            BindCombo(cmbCollectPlanMonth, key, value, dt);

            //0:不要、1:対象
            dt = new DataTable();
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);

            datarow = dt.NewRow();
            datarow[key] = "0";
            datarow[value] = "不要";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "1";
            datarow[value] = "対象";
            dt.Rows.Add(datarow);

            //Todo:決定後メンテナンス
            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "XXX";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "3";
            datarow[value] = "YYY";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "4";
            datarow[value] = "ZZZ";
            dt.Rows.Add(datarow);

            BindCombo(cmbCreditLevel, key, value, dt);

            //1:通常、2:3:4:
            dt = new DataTable();
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);

            datarow = dt.NewRow();
            datarow[key] = "1";
            datarow[value] = "通常";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "会員";

            datarow = dt.NewRow();
            datarow[key] = "3";
            datarow[value] = "外商";

            datarow = dt.NewRow();
            datarow[key] = "4";
            datarow[value] = "Sale";

            BindCombo(cmbStoreTankaKBN, key, value, dt);
        }
        private void InitialControlArray()
        {
            keyControls = new Control[] { ScCustomer.TxtCode, ScCustomer.TxtChangeDate };
            copyKeyControls = new Control[] { ScCopyCustomer.TxtCode, ScCopyCustomer.TxtChangeDate };
            detailControls = new Control[] { ChkVariousFLG,ChkBillingFLG,ChkCollectFLG,pnlCustomerKBN
                            ,txtLastName,txtFirstName,txtCustomerName,pnlAliasKBN,txtKanaName,txtLongName1,txtLongName2,txtBirthDate,pnlSex
                            ,ChkCountryKBN,txtCountryName,txtZipCD1,txtZipCD2,ChkDMFlg,txtAddress1,txtAddress2
                            ,txtTel11,txtTel12,txtTel13,txtTel21,txtTel22,txtTel23,txtMailAddress
                            ,pnlBillingType,ScBillingCD.TxtCode,ScCollectCD.TxtCode,txtBillingCloseDate,cmbCollectPlanMonth,txtCollectPlanDate
                            ,pnlHolidayKBN,txtRegisteredNumber
                            ,ChkNoInvoiceFlg,pnlTaxPrintKBN,cmbTaxTiming,cmbTaxFractionKBN,cmbAmountFractionKBN
                            ,cmbPaymentMethodCD,ScKouzaCD.TxtCode,cmbPaymentUnit,cmbStoreTankaKBN,ScTankaCD.TxtCode,ChkAttentionFLG,ChkConfirmFLG,txtConfirmComment
                            ,cmbCreditLevel,txtCreditCard,txtCreditInsurance,txtCreditDeposit,txtCreditETC,txtCreditAdditionAmount,txtDisplayOrder,txtAnalyzeCD1,txtAnalyzeCD2,txtAnalyzeCD3
                            ,ChkPointFLG,txtLastPoint,txtWaitingPoint,txtTotalPoint,txtRemarksOutStore,txtRemarksInStore,
                            CboStoreCD,ScStaff.TxtCode, checkDeleteFlg, txtCreditCheckKBN, txtCreditMessage,  txtFareLevel, txtFare };
            detailLabels = new Control[] { ScKouzaCD, ScBillingCD, ScCollectCD, ScTankaCD, ScStaff, lblStoreName, lblLastSalesDate, lblPoint, lblMinyukin, lblKensu, lblCreditAmount };
            searchButtons = new Control[] { ScKouzaCD.BtnSearch, ScBillingCD.BtnSearch,ScCollectCD.BtnSearch,ScStaff.BtnSearch,
                                            ScTankaCD.BtnSearch,ScCopyCustomer.BtnSearch,ScCustomer.BtnSearch };

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
                if (!ctl.GetType().Equals(typeof(Panel)))
                    ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                //ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

            radioButton1.CheckedChanged += new System.EventHandler(RadioCustomerKBN_CheckedChanged);
            radioButton2.CheckedChanged += new System.EventHandler(RadioCustomerKBN_CheckedChanged);
            radioButton3.CheckedChanged += new System.EventHandler(RadioCustomerKBN_CheckedChanged);
            radioButton4.CheckedChanged += new System.EventHandler(RadioCustomerKBN_CheckedChanged);

            radioButton1.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton2.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton3.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton4.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton5.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton6.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton7.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton8.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton9.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            ckM_RadioButton6.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            ckM_RadioButton2.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            ckM_RadioButton3.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            ckM_RadioButton4.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            ckM_RadioButton5.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            ckM_RadioButton7.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            ckM_RadioButton8.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
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
                    case (int)EIndex.CustomerCD:
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (keyControls[index].Text == "")
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
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
                    case (int)EIndex.CustomerCD:
                        //入力なければチェックなし
                        if (copyKeyControls[index].Text == "")
                        {
                            ScKouzaCD.LabelText = "";
                            return true;
                        }
                        else
                        {
                            //マスタデータチェック

                        }
                        break;

                    case (int)EIndex.ChangeDate:
                        if (!CheckKey((int)EIndex.CustomerCD))
                        {
                            keyControls[(int)EIndex.CustomerCD].Focus();
                            return false;
                        }
                        if (!CheckKey(index))
                        {
                            keyControls[index].Focus();
                            return false;
                        }

                        //複写得意先CDに入力がある場合、(When there is an input in 複写得意先CD)Ｅ１０２
                        //必須入力
                        if (!string.IsNullOrWhiteSpace(copyKeyControls[(int)EIndex.CustomerCD].Text) && string.IsNullOrWhiteSpace(copyKeyControls[index].Text))
                        {
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
                //[M_Customer_SelectData]
                mce = GetEntity(kbn);
                mce.DeleteFlg = "1";    //DeleteFlg=1のデータも取得
                string changeDate = mce.ChangeDate;

                bool ret = mbl.M_Customer_Select(mce);

                //・新規モードの場合（In the case of "new" mode）
                if (OperationMode == EOperationMode.INSERT && kbn == 0)
                {
                    //以下の条件で得意先マスター(M_Customer)が存在すればエラー (Error if record exists)Ｅ１３２
                    if (ret && changeDate.Equals(mce.ChangeDate))
                    {
                        bbl.ShowMessage("E132");
                        return false;
                    }
                    return true;
                }
                else
                {
                    //以下の条件で得意先マスター(M_Customer)が存在しなければエラー (Error if record does not exist)Ｅ１３３
                    if (!ret || !changeDate.Equals(mce.ChangeDate))
                    {
                        bbl.ShowMessage("E133");
                        return false;
                    }
                    else
                    {
                        //画面セットなしの場合、処理正常終了
                        if (set == false)
                        {
                            return true;
                        }

                        Scr_Clr(1);

                        if (mce.VariousFLG.Equals("1"))
                            ChkVariousFLG.Checked = true;
                        if (mce.BillingFLG.Equals("1"))
                            ChkBillingFLG.Checked = true;
                        if (mce.CollectFLG.Equals("1"))
                            ChkCollectFLG.Checked = true;
                        //pnlCustomerKBN    1:店舗会員、2:店舗現金会員、3:団体法人
                        switch (mce.CustomerKBN)
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
                        switch (mce.AliasKBN)   //1:様、2:御中
                        {
                            case "1":
                                radioButton9.Checked = true;
                                break;
                            case "2":
                                radioButton8.Checked = true;
                                break;
                        }
                        switch (mce.Sex)   //1:男、2:女、0:不明
                        {
                            case "1":
                                radioButton5.Checked = true;
                                break;
                            case "2":
                                radioButton6.Checked = true;
                                break;
                            case "0":
                                radioButton7.Checked = true;
                                break;
                        }

                        txtLastName.Text = mce.LastName;
                        txtFirstName.Text = mce.FirstName;
                        txtCustomerName.Text = mce.CustomerName;
                        txtKanaName.Text = mce.KanaName;
                        txtLongName1.Text = mce.LongName1;
                        txtLongName2.Text = mce.LongName2;
                        txtBirthDate.Text = mce.Birthdate;
                        if (mce.CountryKBN.Equals("1"))
                            ChkCountryKBN.Checked = true;

                        txtCountryName.Text = mce.CountryName;
                        txtZipCD1.Text = mce.ZipCD1;
                        txtZipCD2.Text = mce.ZipCD2;
                        if (mce.DMFlg.Equals("0"))  //0ならON	
                            ChkDMFlg.Checked = true;
                        txtAddress1.Text = mce.Address1;
                        txtAddress2.Text = mce.Address2;
                        txtTel11.Text = mce.Tel11;
                        txtTel12.Text = mce.Tel12;
                        txtTel13.Text = mce.Tel13;
                        txtTel21.Text = mce.Tel21;
                        txtTel22.Text = mce.Tel22;
                        txtTel23.Text = mce.Tel23;
                        txtMailAddress.Text = mce.MailAddress;
                        //pnlBillingType.Text = mce.pnlBillingType;
                        switch (mce.BillingType)
                        {
                            case "1":
                                ckM_RadioButton2.Checked = true;
                                break;
                            case "2":
                                ckM_RadioButton3.Checked = true;
                                break;
                        }
                        detailControls[(int)EIndex.BillingCD].Text = mce.BillingCD;
                        CheckDetail((int)EIndex.BillingCD, false);
                        detailControls[(int)EIndex.CollectCD].Text = mce.CollectCD;
                        CheckDetail((int)EIndex.CollectCD, false);
                        txtBillingCloseDate.Text = mce.BillingCloseDate;
                        cmbCollectPlanMonth.SelectedValue = mce.CollectPlanMonth;
                        txtCollectPlanDate.Text = mce.CollectPlanDate;
                        //pnlHolidayKBN.Text 0:前営業日 1:当日 2:次営業日
                        switch (mce.HolidayKBN)
                        {
                            case "0":
                                ckM_RadioButton4.Checked = true;
                                break;
                            case "1":
                                ckM_RadioButton5.Checked = true;
                                break;
                            case "2":
                                ckM_RadioButton6.Checked = true;
                                break;
                        }

                        txtRegisteredNumber.Text = mce.RegisteredNumber;
                        if (mce.NoInvoiceFlg.Equals("1"))
                            ChkNoInvoiceFlg.Checked = true;
                        //pnlTaxPrintKBN.Text 1:内税、2:外税
                        switch (mce.TaxPrintKBN)
                        {
                            case "1":
                                ckM_RadioButton7.Checked = true;
                                break;
                            case "2":
                                ckM_RadioButton8.Checked = true;
                                break;
                        }

                        cmbTaxTiming.SelectedValue = mce.TaxTiming;
                        cmbTaxFractionKBN.SelectedValue = mce.TaxFractionKBN;
                        cmbAmountFractionKBN.SelectedValue = mce.AmountFractionKBN;
                        cmbPaymentMethodCD.SelectedValue = mce.PaymentMethodCD;
                        detailControls[(int)EIndex.KouzaCD].Text = mce.KouzaCD;
                        CheckDetail((int)EIndex.KouzaCD);
                        cmbPaymentUnit.SelectedValue = mce.PaymentUnit;
                        cmbStoreTankaKBN.SelectedValue = mce.StoreTankaKBN;
                        detailControls[(int)EIndex.TankaCD].Text = mce.TankaCD;
                        CheckDetail((int)EIndex.TankaCD);
                        if (mce.AttentionFLG.Equals("1"))
                            ChkAttentionFLG.Checked = true;
                        if (mce.ConfirmFLG.Equals("1"))
                            ChkConfirmFLG.Checked = true;

                        txtConfirmComment.Text = mce.ConfirmComment;
                        cmbCreditLevel.SelectedValue = mce.CreditLevel;
                        txtCreditCard.Text = bbl.Z_SetStr(mce.CreditCard);
                        txtCreditInsurance.Text = bbl.Z_SetStr(mce.CreditInsurance);
                        txtCreditDeposit.Text = bbl.Z_SetStr(mce.CreditDeposit);
                        txtCreditETC.Text = bbl.Z_SetStr(mce.CreditETC);
                        lblCreditAmount.Text = bbl.Z_SetStr(mce.CreditAmount);
                        //txtCreditWarningAmount.Text = bbl.Z_SetStr(mce.CreditWarningAmount);
                        txtCreditAdditionAmount.Text = bbl.Z_SetStr(mce.CreditAdditionAmount);
                        txtCreditCheckKBN.Text = mce.CreditCheckKBN;
                        txtCreditMessage.Text = mce.CreditMessage;
                        txtFareLevel.Text = mce.FareLevel;
                        txtFare.Text = mce.Fare;
                        txtAnalyzeCD1.Text = mce.AnalyzeCD1;
                        txtAnalyzeCD2.Text = mce.AnalyzeCD2;
                        txtAnalyzeCD3.Text = mce.AnalyzeCD3;
                        txtDisplayOrder.Text = mce.DisplayOrder;
                        if (mce.PointFLG.Equals("1"))
                            ChkPointFLG.Checked = true;
                        txtLastPoint.Text = bbl.Z_SetStr(mce.LastPoint);
                        txtWaitingPoint.Text = bbl.Z_SetStr(mce.WaitingPoint);
                        txtTotalPoint.Text = bbl.Z_SetStr(mce.TotalPoint);
                        txtRemarksOutStore.Text = mce.RemarksOutStore;
                        txtRemarksInStore.Text = mce.RemarksInStore;
                        CboStoreCD.SelectedValue = mce.MainStoreCD;
                        detailControls[(int)EIndex.StaffCD].Text = mce.StaffCD;
                        CheckDetail((int)EIndex.StaffCD);

                        lblMinyukin.Text = bbl.Z_SetStr(mce.UnpaidAmount);
                        lblKensu.Text = bbl.Z_SetStr(mce.UnpaidCount);
                        lblStoreName.Text = mce.LastSalesStoreCD;
                        lblLastSalesDate.Text = mce.LastSalesDate;
                        lblPoint.Text = bbl.Z_SetStr(mce.TotalPurchase);

                        if (mce.DeleteFlg.Equals("1"))
                            checkDeleteFlg.Checked = true;

                    }
                }

                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                {
                    //画面へデータセット後、明細部入力可、キー部入力不可
                    Scr_Lock(3, 3, 0);
                    SetEnabled(1);


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

        private bool CheckDetail(int index, bool msg = true)
        {
            bool ret;

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
               // case (int)EIndex.CustomerCD: break;
                //case (int)EIndex.ChangeDate: break;
                case (int)EIndex.ChkVariousFLG:
                case (int)EIndex.ChkBillingFLG:
                case (int)EIndex.ChkCollectFLG:
                case (int)EIndex.pnlCustomerKBN:
                    break;

                case (int)EIndex.LastName:
                case (int)EIndex.FirstName:
                case (int)EIndex.CustomerName:
                    if (detailControls[index].Enabled)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }

                        if (index == (int)EIndex.FirstName && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.CustomerName].Text))
                        {
                            //得意先会員名がNULLの場合 会員 姓 + 1スペース + 名　を自動セット
                            detailControls[(int)EIndex.CustomerName].Text = detailControls[(int)EIndex.LastName].Text + "　" + detailControls[(int)EIndex.FirstName].Text;
                        }
                        else if (index == (int)EIndex.CustomerName && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.LongName1].Text)
                             && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.LongName2].Text))
                        {
                            //正式名１と２がNULLの場合 得意先会員名を正式名１に自動セット
                            detailControls[(int)EIndex.LongName1].Text = detailControls[(int)EIndex.CustomerName].Text;
                        }
                    }
                    break;

                 case (int)EIndex.pnlAliasKBN:
                 break;

                case (int)EIndex.KanaName:
                case (int)EIndex.LongName1:
                    if (detailControls[index].Enabled)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }

                        if (index == (int)EIndex.FirstName && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.CustomerName].Text))
                        {
                            //得意先会員名がNULLの場合 会員 姓 + 1スペース + 名　を自動セット
                            detailControls[(int)EIndex.CustomerName].Text = detailControls[(int)EIndex.LastName].Text + "　" + detailControls[(int)EIndex.FirstName].Text;
                        }
                        else if (index == (int)EIndex.CustomerName && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.LongName1].Text)
                             && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.LongName2].Text))
                        {
                            //正式名１と２がNULLの場合 得意先会員名を正式名１に自動セット
                            detailControls[(int)EIndex.LongName1].Text = detailControls[(int)EIndex.CustomerName].Text;
                        }
                    }
                    break;

                case (int)EIndex.LongName2:
                case (int)EIndex.BirthDate:
                case (int)EIndex.pnlSex:
                case (int)EIndex.ChkCountryKBN:
                case (int)EIndex.CountryName:
                case (int)EIndex.ZipCD1:
                    break;

                case (int)EIndex.ZipCD2:
                    //郵便番号1、2 入力無くても良い(It is not necessary to input)
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ZipCD1].Text))
                    {
                        //郵便番号1に入力された場合 入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }
                    }
                    if (index.Equals((int)EIndex.ZipCD2) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
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
                        if (dt.Rows.Count > 0 && detailControls[(int)EIndex.Address1].Text == "")
                        {
                            detailControls[(int)EIndex.Address1].Text = dt.Rows[0]["Address1"].ToString();   //住所１
                            detailControls[(int)EIndex.Address2].Text = dt.Rows[0]["Address2"].ToString();  //住所２
                        }
                        else
                        {
                            //存在しない場合でもエラーとはしない(It is not an error that record does not exist)
                        }
                    }
                    break;

                case (int)EIndex.ChkDMFlg:
                case (int)EIndex.Address1:
                case (int)EIndex.Address2:
                case (int)EIndex.Tel11:
                case (int)EIndex.Tel12:
                case (int)EIndex.Tel13:
                case (int)EIndex.Tel21:
                case (int)EIndex.Tel22:
                case (int)EIndex.Tel23:
                case (int)EIndex.MailAddress:
                case (int)EIndex.pnlBillingType:
                    break;

                case (int)EIndex.BillingCD:
                case (int)EIndex.CollectCD:
                    SetLabel(index, "");

                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }

                    //新規モードの場合、画面キー部の得意先CDと同じであれば、チェック不要。
                    if (OperationMode == EOperationMode.INSERT && detailControls[index].Text.Equals(keyControls[(int)EIndex.CustomerCD].Text))
                        return true;

                    //以下の条件でM_Customerが存在しない場合、エラー
                    //[M_Customer]
                    M_Customer_Entity me = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = keyControls[(int)EIndex.ChangeDate].Text
                    };
                    short kbn = 1;
                    if (index == (int)EIndex.BillingCD)
                    {
                        me.CustomerKBN = "2";   //2:請求先
                        kbn = 2;
                    }
                    else if (index == (int)EIndex.CollectCD)
                    {
                        me.CustomerKBN = "3";   //3:入金請求先
                        kbn = 3;
                    }
                    ret = mbl.M_Customer_Select(me);
                    if (ret)
                    {
                        SetLabel(index, me.CustomerName);

                        if (!detailControls[index].Text.Equals(keyControls[(int)EIndex.CustomerCD].Text) && msg)
                        {   //DeleteFlg = 1の場合、エラー
                            if (me.DeleteFlg.Equals("1"))
                            {
                                //Ｅ１５８
                                bbl.ShowMessage("E158");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //Ｅ１０１
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    break;
                case (int)EIndex.BillingCloseDate:
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    if (bbl.Z_Set(detailControls[index].Text) < 1 || bbl.Z_Set(detailControls[index].Text) > 31)
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }
                    break;

                case (int)EIndex.cmbCollectPlanMonth:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bbl.ShowMessage("E102");
                        //((CKM_Controls.CKM_ComboBox)detailControls[index]).MoveNext = false;
                        return false;
                    }
                    break;

                case (int)EIndex.CollectPlanDate:
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    if (bbl.Z_Set(detailControls[index].Text) < 1 || bbl.Z_Set(detailControls[index].Text) > 31)
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }
                    break;

                case (int)EIndex.pnlHolidayKBN:
                case (int)EIndex.RegisteredNumber:
                case (int)EIndex.ChkNoInvoiceFlg:
                case (int)EIndex.pnlTaxPrintKBN:
                    break;

                case (int)EIndex.cmbTaxTiming:
                case (int)EIndex.cmbTaxFractionKBN:
                case (int)EIndex.cmbAmountFractionKBN:
                case (int)EIndex.cmbPaymentMethodCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bbl.ShowMessage("E102");
                        //((CKM_Controls.CKM_ComboBox)detailControls[index]).MoveNext = false;
                        return false;
                    }
                    break;

                case (int)EIndex.KouzaCD:
                    ScKouzaCD.LabelText = "";

                    //得意先CD＝入金請求先CDの場合 入力必須
                    if (detailControls[(int)EIndex.BillingCD].Text.Equals(keyControls[(int)EIndex.CustomerCD].Text))
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //以下の条件でM_Kouzaが存在しない場合、エラー
                        //[M_Kouza]
                        M_Kouza_Entity mke = new M_Kouza_Entity
                        {
                            KouzaCD = detailControls[index].Text,
                            ChangeDate = keyControls[(int)EIndex.ChangeDate].Text
                        };
                        Kouza_BL kbl = new Kouza_BL();
                        DataTable dtKoza = kbl.M_Kouza_Select(mke);
                        if (dtKoza.Rows.Count > 0)
                        {
                            //DeleteFlg = 1の場合、エラー
                            if (dtKoza.Rows[0]["DeleteFlg"].ToString() == "1")
                            {
                                //Ｅ１５８
                                bbl.ShowMessage("E158");
                                return false;
                            }
                            ScKouzaCD.LabelText = dtKoza.Rows[0]["KouzaName"].ToString();
                        }
                        else
                        {
                            //Ｅ１０１
                            bbl.ShowMessage("E101");
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.cmbPaymentUnit:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bbl.ShowMessage("E102");
                        //((CKM_Controls.CKM_ComboBox)detailControls[index]).MoveNext = false;
                        return false;
                    }
                    break;

                case (int)EIndex.cmbStoreTankaKBN:
                    //得意先会員区分 Web会員 ON 以外の時
                    if (!radioButton4.Checked)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            ((CKM_Controls.CKM_ComboBox)detailControls[index]).MoveNext = false;
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.TankaCD:
                    ScTankaCD.LabelText = "";
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.TankaCD].Text))
                    {
                        //以下の条件でM_TankaCDが存在しない場合、エラー
                        //[M_TankaCD]
                        M_TankaCD_Entity mme = new M_TankaCD_Entity
                        {
                            TankaCD = detailControls[index].Text,
                            ChangeDate = bbl.GetDate()
                        };
                        TankaCD_BL tbl = new TankaCD_BL();
                        DataTable dtTan = tbl.M_TankaCD_Select(mme);
                        if (dtTan.Rows.Count > 0)
                        {
                            ScTankaCD.LabelText = dtTan.Rows[0]["TankaName"].ToString();
                        }
                        else
                        {
                            //Ｅ１０１
                            bbl.ShowMessage("E101");
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.ChkAttentionFLG:
                case (int)EIndex.ChkConfirmFLG:
                case (int)EIndex.ConfirmComment:
                    break;

                case (int)EIndex.cmbCreditLevel:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bbl.ShowMessage("E102");
                        //((CKM_Controls.CKM_ComboBox)detailControls[index]).MoveNext = false;
                        return false;
                    }
                    break;
                case (int)EIndex.CreditCard:
                case (int)EIndex.CreditInsurance:
                case (int)EIndex.CreditDeposit:
                case (int)EIndex.CreditETC:
               // case (int)EIndex.CreditWarningAmount:
                case (int)EIndex.CreditAdditionAmount:
                case (int)EIndex.DisplayOrder:
                    detailControls[index].Text = bbl.Z_SetStr(detailControls[index].Text);

                    lblCreditAmount.Text = bbl.Z_SetStr(bbl.Z_Set(detailControls[(int)EIndex.CreditCard].Text)
                                                    + bbl.Z_Set(detailControls[(int)EIndex.CreditInsurance].Text)
                                                    + bbl.Z_Set(detailControls[(int)EIndex.CreditDeposit].Text)
                                                    + bbl.Z_Set(detailControls[(int)EIndex.CreditETC].Text));
                    break;

                case (int)EIndex.AnalyzeCD1:
                case (int)EIndex.AnalyzeCD2:
                case (int)EIndex.AnalyzeCD3:
                case (int)EIndex.ChkPointFLG:
                case (int)EIndex.LastPoint:
                case (int)EIndex.WaitingPoint:
                case (int)EIndex.TotalPoint:
                case (int)EIndex.RemarksOutStore:
                case (int)EIndex.RemarksInStore:
                    break;
                
                case (int)EIndex.CboStoreCD:
                    //入力必須(Entry required)
                   // if (!RequireCheck(new Control[] { detailControls[index] }))
                   if(string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bbl.ShowMessage("E102");
                        //((CKM_Controls.CKM_ComboBox)detailControls[index]).MoveNext = false;
                        return false;
                    }
                    break;

                case (int)EIndex.StaffCD:
                    //入力は任意

                    ScStaff.LabelText = "";
                    if (!string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //以下の条件でM_Staffが存在しない場合、エラー
                        //[M_Staff]
                        M_Staff_Entity mse = new M_Staff_Entity
                        {
                            StaffCD = detailControls[index].Text,
                            ChangeDate = keyControls[(int)EIndex.ChangeDate].Text
                        };
                        Staff_BL sbl = new Staff_BL();
                        ret = sbl.M_Staff_Select(mse);
                        if (ret)
                        {
                            ScStaff.Code = mse.StaffCD;        // 2020-12-01 By SYP
                            ScStaff.LabelText = mse.StaffName;

                            ////以下の条件を満たせばエラー
                            //if (mse.LeaveDate != "")
                            //{
                            //    //if ( mse.LeaveDate <= keyControls[(int)EIndex.ChangeDate].Text)
                            //    int result = mse.LeaveDate.CompareTo(keyControls[(int)EIndex.ChangeDate].Text);
                            //    if (result <= 0)
                            //    {
                            //        //Ｅ１３５
                            //        bbl.ShowMessage("E135");
                            //        return false;
                            //    }
                            //}

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
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.DeleteFlg:
                case (int)EIndex.COUNT:
                    break;

                case (int)EIndex.txtCreditCheckKBN:
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            if (!(Convert.ToInt32(detailControls[index].Text.Replace(",","")) >= 0 && Convert.ToInt32(detailControls[index].Text.Replace(",", "")) <= 2))
                            {
                                bbl.ShowMessage("E117");
                                return false;
                            }
                        }
                    }
                    break;

                case (int)EIndex.lblKouzaCD:
                case (int)EIndex.lblBillingCD:
                case (int)EIndex.lblCollectCD:
                case (int)EIndex.lblTankaCD:
                case (int)EIndex.lblStaff:
                case (int)EIndex.lblStoreName:
                case (int)EIndex.lblLastSalesDate:
                case (int)EIndex.lblPoint:
                case (int)EIndex.lblMinyukin:
                case (int)EIndex.lblKensu:
                case (int)EIndex.lblCreditAmount:
                    break;
            }

            return true;
        }

        private void SetLabel(int index, string val)
        {
            switch (index)
            {
                case (int)EIndex.BillingCD:
                    ScBillingCD.LabelText = val;
                    break;
                case (int)EIndex.CollectCD:
                    ScCollectCD.LabelText = val;
                    break;
            }
        }
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <param name="kbn"></param>
        /// <returns></returns>
        private M_Customer_Entity GetEntity(short kbn)
        {
            mce = new M_Customer_Entity();

            if (kbn == 0)
            {
                mce.CustomerCD = keyControls[(int)EIndex.CustomerCD].Text;
                mce.ChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
            }
            else
            {
                mce.CustomerCD = copyKeyControls[(int)EIndex.CustomerCD].Text;
                mce.ChangeDate = copyKeyControls[(int)EIndex.ChangeDate].Text;
                return mce;
            }
            mce.VariousFLG = ChkVariousFLG.Checked ? "1" : "0";
            mce.BillingFLG = ChkBillingFLG.Checked ? "1" : "0";
            mce.CollectFLG = ChkCollectFLG.Checked ? "1" : "0";
            mce.StoreKBN = "2";//1:WEB、2:店舗
            //pnlCustomerKBN.1:店舗会員、2:店舗現金会員、3:団体法人
            if (radioButton1.Checked)
                mce.CustomerKBN = "1";
            else if (radioButton2.Checked)
                mce.CustomerKBN = "2";
            else if (radioButton3.Checked)
                mce.CustomerKBN = "3";
            else if (radioButton4.Checked)
            {
                mce.CustomerKBN = "4";
                mce.StoreKBN = "1";
            }
            mce.LastName = txtLastName.Text;
            mce.FirstName = txtFirstName.Text;
            mce.CustomerName = txtCustomerName.Text;
            //1:様、2:御中
            if (radioButton9.Checked)
                mce.AliasKBN = "1";
            else if (radioButton8.Checked)
                mce.AliasKBN = "2";
            mce.KanaName = txtKanaName.Text;
            mce.LongName1 = txtLongName1.Text;
            mce.LongName2 = txtLongName2.Text;
            mce.Birthdate = txtBirthDate.Text;
            //1:男、2:女、0:不明
            if (radioButton5.Checked)
                mce.Sex = "1";
            else if (radioButton6.Checked)
                mce.Sex = "2";
            else if (radioButton7.Checked)
                mce.Sex = "0";
            mce.CountryKBN = ChkCountryKBN.Checked ? "1" : "0";
            mce.CountryName = txtCountryName.Text;
            mce.ZipCD1 = txtZipCD1.Text;
            mce.ZipCD2 = txtZipCD2.Text;
            mce.DMFlg = ChkDMFlg.Checked ? "0" : "1";   //ONなら0
            mce.Address1 = txtAddress1.Text;
            mce.Address2 = txtAddress2.Text;
            mce.Tel11 = txtTel11.Text;
            mce.Tel12 = txtTel12.Text;
            mce.Tel13 = txtTel13.Text;
            mce.Tel21 = txtTel21.Text;
            mce.Tel22 = txtTel22.Text;
            mce.Tel23 = txtTel23.Text;
            mce.MailAddress = txtMailAddress.Text;
            //mce.pnlBillingType 1:即請求、2:締請求
            if (ckM_RadioButton2.Checked)
                mce.BillingType = "1";
            else if (ckM_RadioButton3.Checked)
                mce.BillingType = "2";
            mce.BillingCD = detailControls[(int)EIndex.BillingCD].Text;
            mce.CollectCD = detailControls[(int)EIndex.CollectCD].Text;
            mce.BillingCloseDate = txtBillingCloseDate.Text;
            mce.CollectPlanMonth = cmbCollectPlanMonth.SelectedValue == null ? "" : cmbCollectPlanMonth.SelectedValue.ToString();
            mce.CollectPlanDate = txtCollectPlanDate.Text;
            //mce.pnlHolidayKBN 0:前営業日 1:当日 2:次営業日
            if (ckM_RadioButton4.Checked)
                mce.HolidayKBN = "0";
            else if (ckM_RadioButton5.Checked)
                mce.HolidayKBN = "1";
            else if (ckM_RadioButton6.Checked)
                mce.HolidayKBN = "2";

            mce.RegisteredNumber = txtRegisteredNumber.Text;
            mce.NoInvoiceFlg = ChkNoInvoiceFlg.Checked ? "1" : "0";
            //mce.pnlTaxPrintKBN 1:内税、2:外税
            if (ckM_RadioButton7.Checked)
                mce.TaxPrintKBN = "1";
            else if (ckM_RadioButton8.Checked)
                mce.TaxPrintKBN = "2";

            mce.TaxTiming = cmbTaxTiming.SelectedValue == null ? "" : cmbTaxTiming.SelectedValue.ToString();
            mce.TaxFractionKBN = cmbTaxFractionKBN.SelectedValue == null ? "" : cmbTaxFractionKBN.SelectedValue.ToString();
            mce.AmountFractionKBN = cmbAmountFractionKBN.SelectedValue == null ? "" : cmbAmountFractionKBN.SelectedValue.ToString();
            mce.PaymentMethodCD = cmbPaymentMethodCD.SelectedValue == null ? "" : cmbPaymentMethodCD.SelectedValue.ToString();
            mce.KouzaCD = detailControls[(int)EIndex.KouzaCD].Text;
            mce.TankaCD = detailControls[(int)EIndex.TankaCD].Text;
            mce.PaymentUnit = cmbPaymentUnit.SelectedValue == null ? "" : cmbPaymentUnit.SelectedValue.ToString();
            mce.StoreTankaKBN = cmbStoreTankaKBN.SelectedValue == null ? "" : cmbStoreTankaKBN.SelectedValue.ToString();
            mce.AttentionFLG = ChkAttentionFLG.Checked ? "1" : "0";
            mce.ConfirmFLG = ChkConfirmFLG.Checked ? "1" : "0";
            mce.ConfirmComment = txtConfirmComment.Text;
            mce.CreditLevel = cmbCreditLevel.SelectedValue == null ? "0" : cmbCreditLevel.SelectedValue.ToString();
            mce.CreditCard = txtCreditCard.Text;
            mce.CreditInsurance = txtCreditInsurance.Text;
            mce.CreditDeposit = txtCreditDeposit.Text;
            mce.CreditETC = txtCreditETC.Text;
            mce.CreditAmount = lblCreditAmount.Text;
            //mce.CreditWarningAmount = txtCreditWarningAmount.Text;
            mce.CreditAdditionAmount = txtCreditAdditionAmount.Text;
            mce.CreditCheckKBN = txtCreditCheckKBN.Text;
            mce.CreditMessage = txtCreditMessage.Text;
            mce.FareLevel = txtFareLevel.Text.Replace(",", "");
            mce.Fare = txtFare.Text.Replace(",", "");
            mce.AnalyzeCD1 = txtAnalyzeCD1.Text;
            mce.AnalyzeCD2 = txtAnalyzeCD2.Text;
            mce.AnalyzeCD3 = txtAnalyzeCD3.Text;
            mce.DisplayOrder = txtDisplayOrder.Text;
            mce.PointFLG = ChkPointFLG.Checked ? "1" : "0";
            mce.LastPoint = bbl.Z_SetStr(txtLastPoint.Text);
            mce.WaitingPoint = bbl.Z_SetStr(txtWaitingPoint.Text);
            mce.TotalPoint = bbl.Z_SetStr(txtTotalPoint.Text);
            mce.RemarksOutStore = txtRemarksOutStore.Text;
            mce.RemarksInStore = txtRemarksInStore.Text;
            mce.MainStoreCD = CboStoreCD.SelectedValue == null ? "" : CboStoreCD.SelectedValue.ToString();
            mce.StaffCD = detailControls[(int)EIndex.StaffCD].Text;

            //チェックボックス
            if (checkDeleteFlg.Checked)
                mce.DeleteFlg = "1";
            else
                mce.DeleteFlg = "0";

            mce.UsedFlg = "0";
            mce.InsertOperator = InOperatorCD;
            mce.PC = InPcID;

            return mce;
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

            //更新処理
            mce = GetEntity(0);
            mbl.Customer_Exec(mce, (short)OperationMode);

            //更新後画面クリア
            InitScr();

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");
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

            keyControls[0].Focus(); //得意先CD
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
                if (ctl.GetType().Equals(typeof(CheckBox)) || ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(RadioButton)) || ctl.GetType().Equals(typeof(CKM_Controls.CKMShop_RadioButton)))
                {
                    ((RadioButton)ctl).Checked = true;
                }
                else if (ctl.GetType().Equals(typeof(Panel)))
                {
                    radioButton1.Checked = true;
                    radioButton5.Checked = true;
                    radioButton9.Checked = true;
                    ckM_RadioButton4.Checked = true;
                    ckM_RadioButton2.Checked = true;
                    ckM_RadioButton7.Checked = true;
                }
                else
                {
                    ctl.Text = "";
                }
            }

            foreach (Control ctl in detailLabels)
            {
                if (ctl.GetType().Equals(typeof(CKM_SearchControl)))
                    ((CKM_SearchControl)ctl).LabelText = "";
                else
                    ctl.Text = "";
            }

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
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            if (base.OperationMode != EOperationMode.INSERT)
                                ScCustomer.SearchEnable = Kbn == 0 ? true : false;
                            else
                            {
                                ScCustomer.SearchEnable = false;
                                F9Visible = false;
                            }


                            break;
                        }

                    case 1:
                        {
                            // ｷｰ部(複写)
                            foreach (Control ctl in copyKeyControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            ScCopyCustomer.SearchEnable = Kbn == 0 ? true : false;
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
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            for (int index = 0; index < searchButtons.Length - 2; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;

                            checkDeleteFlg.Enabled = Kbn == 0 ? true : false;

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
                            case (int)EIndex.CustomerCD:
                                keyControls[(int)EIndex.ChangeDate].Focus();
                                break;

                            case (int)EIndex.ChangeDate:
                                if (OperationMode == EOperationMode.INSERT)
                                {
                                    copyKeyControls[(int)EIndex.CustomerCD].Focus();
                                }
                                else if (OperationMode == EOperationMode.UPDATE)
                                {
                                    detailControls[0].Focus();
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
                            case (int)EIndex.CustomerCD:
                                copyKeyControls[(int)EIndex.ChangeDate].Focus();
                                break;

                            case (int)EIndex.ChangeDate:
                                if (OperationMode == EOperationMode.INSERT)
                                {
                                    detailControls[0].Focus();
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
                            if (index + 1 == (int)EIndex.pnlCustomerKBN)
                            {
                                if (radioButton1.Checked)
                                    radioButton1.Focus();
                                else if (radioButton2.Checked)
                                    radioButton2.Focus();
                                else if (radioButton3.Checked)
                                    radioButton3.Focus();
                                else if (radioButton4.Checked)
                                    radioButton4.Focus();
                            }
                            else if (index + 1 == (int)EIndex.pnlAliasKBN)
                            {
                                if (radioButton9.Checked)
                                    radioButton9.Focus();
                                else if (radioButton8.Checked)
                                    radioButton8.Focus();
                            }
                            else if (index + 1 == (int)EIndex.pnlSex)
                            {
                                if (radioButton5.Checked)
                                    radioButton5.Focus();
                                else if (radioButton6.Checked)
                                    radioButton6.Focus();
                                else if (radioButton7.Checked)
                                    radioButton7.Focus();
                            }
                            else if (index + 1 == (int)EIndex.pnlBillingType)
                            {
                                if (ckM_RadioButton2.Checked)
                                    ckM_RadioButton2.Focus();
                                else if (ckM_RadioButton3.Checked)
                                    ckM_RadioButton3.Focus();
                            }
                            else if (index + 1 == (int)EIndex.pnlHolidayKBN)
                            {
                                if (ckM_RadioButton4.Checked)
                                    ckM_RadioButton4.Focus();
                                else if (ckM_RadioButton5.Checked)
                                    ckM_RadioButton5.Focus();
                                else if (ckM_RadioButton6.Checked)
                                    ckM_RadioButton6.Focus();
                            }
                            else if (index + 1 == (int)EIndex.pnlTaxPrintKBN)
                            {
                                if (ckM_RadioButton7.Checked)
                                    ckM_RadioButton7.Focus();
                                else if (ckM_RadioButton8.Checked)
                                    ckM_RadioButton8.Focus();
                            }
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
        private void SetEnabled(int kbn)
        {
            if (base.OperationMode == EOperationMode.INSERT || base.OperationMode == EOperationMode.UPDATE)
            {
                if (radioButton3.Checked)
                {
                    //団体法人ONの場合
                    //会員姓･名は入力不可
                    detailControls[(int)EIndex.LastName].Enabled = false;    // 2020-12-01 By SYP
                    detailControls[(int)EIndex.LastName].Text = "";
                    detailControls[(int)EIndex.FirstName].Enabled = false;
                    detailControls[(int)EIndex.FirstName].Text = "";
                }
                else
                {
                    detailControls[(int)EIndex.LastName].Enabled = true;
                    detailControls[(int)EIndex.FirstName].Enabled = true;     // 2020-12-01 By SYP
                }
            }
        }
        private void RadioCustomerKBN_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetEnabled(0);
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
                    //請求区分
                    if (((RadioButton)sender).Parent.Equals(pnlBillingType))
                    {
                        detailControls[(int)EIndex.BillingCD].Focus();
                    }
                    else if (((RadioButton)sender).Parent.Equals(pnlCustomerKBN))
                    {
                        if (detailControls[(int)EIndex.LastName].CanFocus)
                            detailControls[(int)EIndex.LastName].Focus();
                        else
                            //あたかもTabキーが押されたかのようにする
                            //Shiftが押されている時は前のコントロールのフォーカスを移動
                            this.ProcessTabKey(!e.Shift);

                    }
                    else if (((RadioButton)sender).Parent.Equals(pnlAliasKBN))
                    {
                        detailControls[(int)EIndex.KanaName].Focus();
                    }
                    else if (((RadioButton)sender).Parent.Equals(pnlHolidayKBN))
                    {
                        detailControls[(int)EIndex.RegisteredNumber].Focus();
                    }
                    else if (((RadioButton)sender).Parent.Equals(pnlSex))
                    {
                        detailControls[(int)EIndex.ChkCountryKBN].Focus();
                    }
                    else if (((RadioButton)sender).Parent.Equals(pnlTaxPrintKBN))
                    {
                        detailControls[(int)EIndex.cmbTaxTiming].Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }
        private void ScStore_Leave(object sender, EventArgs e)
        {
            ScKouzaCD.ChangeDate = ScCustomer.ChangeDate;
            ScBillingCD.ChangeDate = ScCustomer.ChangeDate;
            ScCollectCD.ChangeDate = ScCustomer.ChangeDate;
            ScTankaCD.ChangeDate = ScCustomer.ChangeDate;
            ScStaff.ChangeDate = ScCustomer.ChangeDate;
        }
        #endregion

        private void PanelDetail_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
