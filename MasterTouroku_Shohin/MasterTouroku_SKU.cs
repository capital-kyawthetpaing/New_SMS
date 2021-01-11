using System;
using System.Data;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;
using Search;

namespace MasterTouroku_Shouhin
{
    /// <summary>
    /// MasterTouroku_Shouhin 商品マスタ
    /// </summary>
    internal partial class MasterTouroku_SKU : FrmMainForm
    {
        private const string ProID = "MasterTouroku_Shouhin";
        private const string ProNm = "商品マスター（SKU)";
        private const short mc_L_END = 3; // ロック用        

        private enum EIndex : int
        {
            //LabelControl
            SKUCD,
            ChangeDate
          , SizeNo
          , SizeName
          , ColorNo
          , ColorName
          , MakerItem
          , BrandCD
          , BrandName
          , TaniCD
          , TaniName
          , SportsCD
          , SportsName
          , ReserveName
          , NoticesName
          , PostageName
          , ManufactName
          , ConfirmName

          , TaxRateFLGName
          , CostingKBNName
                , SegmentCD
                , SegmentName

          //KeyControl
          , JanCD = 0
          , ApprovalDate
          //, SetAdminCD
          //, SetItemCD
          , SetSKUCD
          , SetSU

          //DetailControl
          , ChkVariousFLG = 0
          , ChkSetKBN
          , ChkWebFlg
          , ChkRealStoreFlg

          , SKUName
          , SKUShortName
          , KanaName
          , EnglishName

        , ChkVirtualFlg
        , ChkDiscontinueFlg
        , ChkStopFlg
        , ChkDiscountKBN
        , Chk5
        , Chk6

        , ChkPresentKBN
        , ChkSampleKBN
        , ChkCatalogFlg
        , ChkNoNetOrderFlg
        , ChkEDIOrderFlg
        , ChkAutoOrderFlg

        , ChkDirectFlg
        , ChkParcelFlg
        , ChkZaikoKBN
        , ChkSoldOutFlg
        , Chk17
        , ChkSaleExcludedFlg

        , ChkWebStockFlg
        , ChkInventoryAddFlg
        , ChkMakerAddFlg
        , ChkStoreAddFlg
        , Chk27
        , Chk28

        , MainVendorCD
        , Rack

          , SaleStartDate
          , WebStartDate
          
          , PriceOutTax
          , PriceWithTax
          , Rate
          , OrderPriceWithoutTax
          , OrderPriceWithTax

          , CmbOrderAttentionCD
          , OrderAttentionNote
          , ShouhinCD

          , CmbTag1
            , CmbTag2
            , CmbTag3
            , CmbTag4
            , CmbTag5
            , CmbTag6
            , CmbTag7
            , CmbTag8
            , CmbTag9
            , CmbTag10

                , ExhibitionSegmentCD
                , OrderLot
                , ExhibitionCommonCD

          , CmbLastYearTerm
          , CmbLastSeason
          , LastCatalogNO
          , LastCatalogPage
          , LastCatalogText

          , LastInstructionsDate
          , LastInstructionsNO
          , CommentOutStore
          , CommentInStore          
          , WebAddress
                
          , DeleteFlg
          ,COUNT
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            SKUCD,
        }
        private Control[] keyControls;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private ITEM_BL mibl;
        private M_ITEM_Entity mie;
        private M_SKU_Entity mse;

        private decimal mOldPriceOutTax;
        private decimal mOldRate;
        private decimal mOldOrderPriceWithoutTax;

        //パラメータ
        public string parJancd = "";
        public string parSKUCD = "";
        public string parSizeNo = "";
        public string parColorNo = "";
        public string parSizeName = "";
        public string parColorName = "";
        public string setSoukoCD = "";
        public int parFractionKBN;

        public DataTable dtSKU;
        public DataTable dtSite;

        public bool ExecFlg = false;
        public MasterTouroku_SKU(M_ITEM_Entity me, DataTable dt, DataTable dtS, EOperationMode mode)
        {
            InitializeComponent();

            mie = me;
            dtSKU = dt;
            dtSite = dtS;
            OperationMode = mode;
        }

        private void MasterTouroku_SKU_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                //this.SetFunctionLabel(EProMode.MENTE);
                this.InitialControlArray();
                mibl = new ITEM_BL();

                Btn_F1.Text = "戻る(F1)";
                Btn_F2.Text = "";
                Btn_F3.Text = "";
                Btn_F4.Text = "";
                Btn_F5.Text = "";
                Btn_F6.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F9.Text = "";
                Btn_F10.Text = "";
                Btn_F11.Text = "";
                Btn_F12.Text = "全て登録(F12)";

                ////起動時共通処理
                base.StartProgram();
                HeaderTitleText = ProNm;
                Scr_Clr(0);

                SC_ITEM.ChangeDate = detailLabels[(int)EIndex.ChangeDate].Text;
                ScSKUCD.ChangeDate = detailLabels[(int)EIndex.ChangeDate].Text;
                ScVendor.ChangeDate = detailLabels[(int)EIndex.ChangeDate].Text;
                ScRackNo.ChangeDate = detailLabels[(int)EIndex.ChangeDate].Text;
                ScRackNo.Value1 = setSoukoCD;
                ScExhibitionSegmentCD.ChangeDate = detailLabels[(int)EIndex.ChangeDate].Text;
                ScExhibitionSegmentCD.Value1 = MultiPorpose_BL.ID_ExhibitionSegmentCD;

                string ymd = bbl.GetDate();
                cmbOrderAttentionCD.Bind(ymd, "2");
                CmbLastYearTerm.Bind(ymd);
                CmbLastSeason.Bind(ymd);

                for (int i = (int)EIndex.CmbTag1; i <= (int)EIndex.CmbTag10; i++)
                {
                    ((CKM_Controls.CKM_ComboBox)detailControls[i]).Bind(ymd);
                    ((CKM_Controls.CKM_ComboBox)detailControls[i]).DropDownStyle = ComboBoxStyle.DropDown;
                    ((CKM_Controls.CKM_ComboBox)detailControls[i]).AcceptKey = true;
                }
                mse = GetEntityFromDataTable();
                SetData();

                SetEnabled();
                SetEnabledForSet();
                keyControls[(int)EIndex.JanCD].Enabled = false;
                SetFuncKeyAll(this, "100000000001");

                detailControls[(int)EIndex.SKUName].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        private void SetEnabled()
        {
            for(int i = 0; i< (int)EIndex.COUNT; i++)
            {
                switch(i)
                {
                    case (int)EIndex.ChkVariousFLG:
                    case (int)EIndex.ChkSetKBN:
                    case (int)EIndex.Chk5:
                    case (int)EIndex.Chk6:
                    case (int)EIndex.ChkNoNetOrderFlg:
                    case (int)EIndex.ChkEDIOrderFlg:
                    case (int)EIndex.ChkAutoOrderFlg:
                    case (int)EIndex.ChkDirectFlg:
                    case (int)EIndex.ChkParcelFlg:
                    case (int)EIndex.ChkZaikoKBN:
                    case (int)EIndex.Chk17:
                    case (int)EIndex.ChkWebStockFlg:
                    case (int)EIndex.ChkInventoryAddFlg:
                    case (int)EIndex.ChkMakerAddFlg:
                    case (int)EIndex.ChkStoreAddFlg:
                    case (int)EIndex.Chk27:
                    case (int)EIndex.Chk28:
                        detailControls[i].Enabled = false;
                        break;
                }
            }
        }
        private void SetEnabledForSet()
        {
            //セット品CB ONなら入力可	
            lblSetKBN.Visible = ChkSetKbn.Checked;
            lblSetKBN.ForeColor = System.Drawing.Color.Red;
            keyControls[(int)EIndex.SetSKUCD].Enabled = ChkSetKbn.Checked;
            keyControls[(int)EIndex.SetSU].Enabled = ChkSetKbn.Checked;
            ScSKUCD.BtnSearch.Enabled = ChkSetKbn.Checked;

            if (!ChkSetKbn.Checked)
            {
                //セット品以外の場合は、入力不可
                keyControls[(int)EIndex.SetSKUCD].Text = "";
                keyControls[(int)EIndex.SetSU].Text = "";
            }

        }
        private void InitialControlArray()
        {
            keyControls = new Control[] { SC_ITEM.TxtCode, ckM_TextBox16, ScSKUCD.TxtCode, ckM_TextBox15 };
            detailControls = new Control[] {ckM_CheckBox19,ChkSetKbn,ckM_CheckBox21,ckM_CheckBox22
                , ckM_TextBox6, ckM_TextBox24, ckM_TextBox20, ckM_TextBox25
                ,ChkVirtualFlg,ckM_CheckBox2,ckM_CheckBox3,ckM_CheckBox4,ckM_CheckBox5,ckM_CheckBox6
                ,ckM_CheckBox7,ckM_CheckBox8,ckM_CheckBox9,ckM_CheckBox10,ckM_CheckBox11,ckM_CheckBox12
                ,ckM_CheckBox13,ckM_CheckBox14,ckM_CheckBox15,ckM_CheckBox16,ckM_CheckBox17,ckM_CheckBox18
                ,ckM_CheckBox23,ckM_CheckBox24,ckM_CheckBox25,ckM_CheckBox26,ckM_CheckBox27,ckM_CheckBox28
                ,  ScVendor.TxtCode, ScRackNo.TxtCode
                , ckM_TextBox18, ckM_TextBox17, ckM_TextBox8, ckM_TextBox9, ckM_TextBox5, ckM_TextBox11, ckM_TextBox10
                ,cmbOrderAttentionCD, ckM_TextBox13,ckM_TextBox7
                ,ckM_ComboBox8,ckM_ComboBox9,ckM_ComboBox10,ckM_ComboBox11,ckM_ComboBox12,ckM_ComboBox13
                ,ckM_ComboBox14,ckM_ComboBox15,ckM_ComboBox16,ckM_ComboBox17
                ,ScExhibitionSegmentCD.TxtCode, txtOrderLot, txtExhibitionCommonCD
                , CmbLastYearTerm, CmbLastSeason, ckM_TextBox22, ckM_TextBox23,TxtRemark
                , ckM_TextBox3,ckM_MultiLineTextBox1,ckM_MultiLineTextBox2,ckM_MultiLineTextBox3,ckM_MultiLineTextBox4
            };
            detailLabels = new Control[] {lblSKUCD, lblChangeDate, lblSizeNo, lblSizeName, lblColorNo, lblColorName
                                        ,lblMakerItem, label2, label11, label12, label13, label19, label20, label22, label23
                                        ,label31, label32,label35,label36,label37,label26,label10,     ScVendor };
            searchButtons = new Control[] { ScVendor.BtnSearch,ScSKUCD.BtnSearch };

            //イベント付与
            foreach (Control ctl in keyControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyControl_KeyDown);
                //ctl.Enter += new System.EventHandler(KeyControl_Enter);
            }
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                //ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

        }

        private bool CheckKey(int index)
        {
            return this.CheckKey(index, 0, true);
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
                    case (int)EIndex.JanCD:
                        if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                        {
                            //入力されなければ、架空商品CB をON
                            ChkVirtualFlg.Checked = true;
                            return true;
                        }

                        //13桁の数字であること
                        if (keyControls[index].Text.Length != 13)
                        {
                            bbl.ShowMessage("E220");
                            return false;
                        }

                        break;
                    case (int)EIndex.ApprovalDate:
                        if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                        {
                            return true;
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

                    case (int)EIndex.SetSKUCD:
                        //セット品の場合、入力必須
                        if (ChkSetKbn.Checked)
                        {
                            ScSKUCD.LabelText = "";

                            //入力必須(Entry required)
                            if (!RequireCheck(new Control[] { keyControls[index] }))
                            {
                                return false;
                            }
                            //SKUマスターに存在すること
                            M_SKU_Entity me = new M_SKU_Entity();
                            me.SKUCD = keyControls[index].Text;
                            me.ChangeDate = detailLabels[(int)EIndex.ChangeDate].Text;

                            bool ret = mibl.M_SKU_SelectBySKUCD(me);
                            if (!ret)
                            {
                                bbl.ShowMessage("E133");
                                return false;
                            }
                            else
                            {
                                ScSKUCD.LabelText = me.SKUName;
                            }
                        }

                        break;

                    case (int)EIndex.SetSU:
                        //セット品の場合、入力必須
                        if (ChkSetKbn.Checked)
                        {
                            //入力必須(Entry required)
                            if (!RequireCheck(new Control[] { keyControls[index] }))
                            {
                                return false;
                            }
                        }
                        break;
                }
            }
            return true;

        }

        private void InitGrid()
        {

            dgvDetail.Enabled = true;

            if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
            {
                dgvDetail.ReadOnly = false;
            }
            else
            {
                dgvDetail.ReadOnly = true;
            }
        }
        private bool CheckDetail(int index, bool set=true)
        {
            bool ret;
            string ymd = lblChangeDate.Text;

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.SKUName:
                case (int)EIndex.SKUShortName:
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;

                case (int)EIndex.KanaName:
                    break;

                case (int)EIndex.ExhibitionSegmentCD:
                    // 入力無くても良い(It is not necessary to input)
                    ((Search.CKM_SearchControl)detailControls[index].Parent).LabelText = "";

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //以下の条件でM_MultiPorposeが存在しない場合、エラー
                    string id = "";
                    if (index.Equals((int)EIndex.ExhibitionSegmentCD))
                        id = MultiPorpose_BL.ID_ExhibitionSegmentCD;

                    //[M_MultiPorpose]
                    M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity
                    {
                        ID = id,
                        Key = detailControls[index].Text
                    };
                    MultiPorpose_BL mbl = new MultiPorpose_BL();
                    DataTable dt = mbl.M_MultiPorpose_Select(mme);
                    if (dt.Rows.Count > 0)
                    {
                        ((Search.CKM_SearchControl)detailControls[index].Parent).LabelText = dt.Rows[0]["Char1"].ToString();
                    }
                    else
                    {
                        //Ｅ１０１
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    break;

                case (int)EIndex.OrderLot:
                    //未入力時、1をセット
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        detailControls[index].Text = "1";
                    }
                    break;

                case (int)EIndex.MainVendorCD:
                    ScVendor.LabelText = "";
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //[M_VendorCD_Select]
                    M_Vendor_Entity mce = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    ret = sbl.M_Vendor_SelectTop1(mce);
                    if (ret)
                    {
                        ScVendor.LabelText = mce.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    break;

                case (int)EIndex.Rack:
                    // 入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //【M_Location】
                    M_Location_Entity ml = new M_Location_Entity
                    {
                        SoukoCD = setSoukoCD,
                        TanaCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    ret = mibl.M_Location_SelectData(ml);
                    if (!ret)
                    {
                        //Ｅ２０４
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    break;

                case (int)EIndex.SaleStartDate:
                    case(int)EIndex.WebStartDate:
                case (int)EIndex.LastInstructionsDate:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    ////入力できる範囲内の日付であること
                    //if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    //{
                    //    //Ｅ１１５
                    //    bbl.ShowMessage("E115");
                    //    return false;
                    //}
                    break;
                case (int)EIndex.PriceOutTax: //税抜定価
                    //画面上で金額変更された場合
                    if (mOldPriceOutTax != bbl.Z_Set(detailControls[index].Text))
                    {
                        mOldPriceOutTax = bbl.Z_Set(detailControls[index].Text);

                        //税込定価=税抜定価×	（１＋（税率÷100））						
                        //税率：税率区分LISTBOXで、
                        decimal zei = 0;
                        decimal zeiritsu = 0;
                        int taxRate = 0;
                        if (!string.IsNullOrWhiteSpace(mie.TaxRateFLG))
                        {
                            taxRate = Convert.ToInt16(mie.TaxRateFLG);
                        }

                        detailControls[(int)EIndex.PriceWithTax].Text = bbl.Z_SetStr(bbl.GetZeikomiKingaku(mOldPriceOutTax, taxRate, out zei, out zeiritsu));
                        //税抜発注額＝税抜定価×（画面.掛率÷100）
                        detailControls[(int)EIndex.OrderPriceWithoutTax].Text = bbl.Z_SetStr(GetResultWithHasuKbn(parFractionKBN, mOldPriceOutTax * bbl.Z_Set(detailControls[(int)EIndex.Rate].Text) / 100));
                        //税込発注額＝税抜発注額×（１＋（税率÷100））								
                        detailControls[(int)EIndex.OrderPriceWithTax].Text = bbl.Z_SetStr(bbl.GetZeikomiKingaku(bbl.Z_Set(detailControls[(int)EIndex.OrderPriceWithoutTax].Text), taxRate, out zei));

                    }
                    break;
                case (int)EIndex.Rate: //掛率
                                       //画面上で金額変更された場合
                    if (mOldRate != bbl.Z_Set(detailControls[index].Text))
                    {
                        mOldRate = bbl.Z_Set(detailControls[index].Text);

                        decimal zei = 0;
                        int taxRate = 0;
                        if (!string.IsNullOrWhiteSpace(mie.TaxRateFLG))
                        {
                            taxRate = Convert.ToInt16(mie.TaxRateFLG);
                        }

                        //税抜発注額＝税抜定価×（画面.掛率÷100）
                        detailControls[(int)EIndex.OrderPriceWithoutTax].Text = bbl.Z_SetStr(GetResultWithHasuKbn(parFractionKBN, bbl.Z_Set(detailControls[(int)EIndex.PriceOutTax].Text) * bbl.Z_Set(detailControls[(int)EIndex.Rate].Text) / 100));
                        //税込発注額＝税抜発注額×（１＋（税率÷100））								
                        detailControls[(int)EIndex.OrderPriceWithTax].Text = bbl.Z_SetStr(bbl.GetZeikomiKingaku(bbl.Z_Set(detailControls[(int)EIndex.OrderPriceWithoutTax].Text), taxRate, out zei));
                    }
                    break;
                case (int)EIndex.OrderPriceWithoutTax://税抜発注額
                                                      //画面上で金額変更された場合
                    if (mOldOrderPriceWithoutTax != bbl.Z_Set(detailControls[index].Text))
                    {
                        mOldOrderPriceWithoutTax = bbl.Z_Set(detailControls[index].Text);

                        decimal zei = 0;
                        int taxRate = 0;
                        if (!string.IsNullOrWhiteSpace( mie.TaxRateFLG))
                        {
                            taxRate = Convert.ToInt16(mie.TaxRateFLG);
                        }
                        //税込発注額＝税抜発注額×（１＋（税率÷100））								
                        detailControls[(int)EIndex.OrderPriceWithTax].Text = bbl.Z_SetStr(bbl.GetZeikomiKingaku(bbl.Z_Set(detailControls[(int)EIndex.OrderPriceWithoutTax].Text), taxRate, out zei));
                    }
                    break;

                case (int)EIndex.ShouhinCD:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    if (set)
                    {
                        //サイト商品CDの表エリアで、有効な各行（モール名ありの行）のサイト商品CD＝Nullであれば、入力されたこのサイト商品CDを
                        //その行のサイト商品CDに自動セット
                        string syocd = detailControls[index].Text;
                        for (int row = 0; row < dgvDetail.RowCount; row++)
                        {
                            if (string.IsNullOrWhiteSpace(dgvDetail[1, row].Value.ToString()))
                            {
                                dgvDetail[1, row].Value = syocd;
                            }
                        }
                    }
                    break;
            }

            return true;
        }
        private void SetData()
        {
            //Key
            keyControls[(int)EIndex.JanCD].Text = mse.JanCD;
            keyControls[(int)EIndex.ApprovalDate].Text = mse.ApprovalDate;
            //keyControls[(int)EIndex.SetItemCD].Text = mse.SetItemCD;
            //keyControls[(int)EIndex.SetAdminCD].Text = mse.SetAdminCD;
            //keyControls[(int)EIndex.SetItemCD].Text = mse.SetItemCD;
            keyControls[(int)EIndex.SetSKUCD].Text = mse.SetSKUCD;
            keyControls[(int)EIndex.SetSU].Text = mse.SetSU;

            //Label
            detailLabels[(int)EIndex.SKUCD].Text = mse.SKUCD;
            detailLabels[(int)EIndex.ChangeDate].Text = mse.ChangeDate;
            detailLabels[(int)EIndex.ColorNo].Text = mse.ColorNO;
            detailLabels[(int)EIndex.ColorName].Text = mse.ColorName;
            detailLabels[(int)EIndex.SizeNo].Text = mse.SizeNO;
            detailLabels[(int)EIndex.SizeName].Text = mse.SizeName;
            detailLabels[(int)EIndex.MakerItem].Text = mse.MakerItem;
            detailLabels[(int)EIndex.BrandCD].Text = mse.BrandCD;
            detailLabels[(int)EIndex.BrandName].Text = mse.BrandName;
            detailLabels[(int)EIndex.TaniCD].Text = mse.TaniCD;
            detailLabels[(int)EIndex.TaniName].Text = mse.TaniName;
            detailLabels[(int)EIndex.SportsCD].Text = mse.SportsCD;
            detailLabels[(int)EIndex.SportsName].Text = mse.SportsName;
            detailLabels[(int)EIndex.ReserveName].Text = mse.ReserveName;
            detailLabels[(int)EIndex.NoticesName].Text = mse.NoticesName;
            detailLabels[(int)EIndex.PostageName].Text = mse.PostageName;
            detailLabels[(int)EIndex.ManufactName].Text = mse.ManufactName;
            detailLabels[(int)EIndex.ConfirmName].Text = mse.ConfirmName;

            detailLabels[(int)EIndex.TaxRateFLGName].Text = mse.TaxRateFLGName;
            detailLabels[(int)EIndex.CostingKBNName].Text = mse.CostingKBNName;
            detailLabels[(int)EIndex.SegmentCD].Text = mse.SegmentCD;
            detailLabels[(int)EIndex.SegmentName].Text = mse.SegmentName;

            //Text
            if (mse.VariousFLG.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkVariousFLG]).Checked = true;
            }
            detailControls[(int)EIndex.SKUName].Text = mse.SKUName;
            detailControls[(int)EIndex.KanaName].Text = mse.KanaName;
            detailControls[(int)EIndex.SKUShortName].Text = mse.SKUShortName;
            detailControls[(int)EIndex.EnglishName].Text = mse.EnglishName;

            if (mse.SetKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkSetKBN]).Checked = true;
            }
            if (mse.PresentKBN != null && mse.PresentKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkPresentKBN]).Checked = true;
            }
            if (mse.SampleKBN != null && mse.SampleKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkSampleKBN]).Checked = true;
            }

            if (mse.DiscountKBN != null && mse.DiscountKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkDiscountKBN]).Checked = true;
            }
            if (mse.WebFlg != null && mse.WebFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkWebFlg]).Checked = true;
            }
            if (mse.RealStoreFlg != null && mse.RealStoreFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkRealStoreFlg]).Checked = true;
            }
            if (mse.VirtualFlg != null && mse.VirtualFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkVirtualFlg]).Checked = true;
            }
            if (mse.DirectFlg != null && mse.DirectFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkDirectFlg]).Checked = true;
            }
            if (mse.WebStockFlg != null && mse.WebStockFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkWebStockFlg]).Checked = true;
            }
            if (mse.StopFlg != null && mse.StopFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkStopFlg]).Checked = true;
            }
            if (mse.DiscontinueFlg != null && mse.DiscontinueFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkDiscontinueFlg]).Checked = true;
            }
            if (mse.InventoryAddFlg != null && mse.InventoryAddFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkInventoryAddFlg]).Checked = true;
            }
            if (mse.MakerAddFlg != null && mse.MakerAddFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkMakerAddFlg]).Checked = true;
            }
            if (mse.StoreAddFlg != null && mse.StoreAddFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkStoreAddFlg]).Checked = true;
            }
            if (mse.NoNetOrderFlg != null && mse.NoNetOrderFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkNoNetOrderFlg]).Checked = true;
            }
            if (mse.EDIOrderFlg != null && mse.EDIOrderFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkEDIOrderFlg]).Checked = true;
            }
            if (mse.CatalogFlg != null && mse.CatalogFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkCatalogFlg]).Checked = true;
            }
            if (mse.ParcelFlg != null && mse.ParcelFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkParcelFlg]).Checked = true;
            }
            if (mse.SoldOutFlg != null && mse.SoldOutFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkSoldOutFlg]).Checked = true;
            }
            if (mse.AutoOrderFlg != null && mse.AutoOrderFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkAutoOrderFlg]).Checked = true;
            }
            if (mse.ZaikoKBN != null && mse.ZaikoKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkZaikoKBN]).Checked = true;
            }
            if (mse.SaleExcludedFlg != null && mse.SaleExcludedFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkSaleExcludedFlg]).Checked = true;
            }

            detailControls[(int)EIndex.MainVendorCD].Text = mse.MainVendorCD;
            //detailControls[(int)EIndex.MakerVendorCD].Text = mse.MakerVendorCD;
            
            detailControls[(int)EIndex.Rack].Text = mse.Rack;
            detailControls[(int)EIndex.PriceWithTax].Text =bbl.Z_SetStr( mse.PriceWithTax);
            detailControls[(int)EIndex.PriceOutTax].Text = bbl.Z_SetStr(mse.PriceOutTax);
            detailControls[(int)EIndex.Rate].Text = bbl.Z_SetStr(mse.Rate);
            detailControls[(int)EIndex.OrderPriceWithTax].Text = bbl.Z_SetStr(mse.OrderPriceWithTax);
            detailControls[(int)EIndex.OrderPriceWithoutTax].Text = bbl.Z_SetStr(mse.OrderPriceWithoutTax);
            detailControls[(int)EIndex.SaleStartDate].Text = mse.SaleStartDate;
            detailControls[(int)EIndex.WebStartDate].Text = mse.WebStartDate;
            cmbOrderAttentionCD.SelectedValue = mse.OrderAttentionCD;
            detailControls[(int)EIndex.OrderAttentionNote].Text = mse.OrderAttentionNote;
            detailControls[(int)EIndex.CommentInStore].Text = mse.CommentInStore;
            detailControls[(int)EIndex.CommentOutStore].Text = mse.CommentOutStore;
            detailControls[(int)EIndex.ExhibitionSegmentCD].Text = mse.ExhibitionSegmentCD;
            detailControls[(int)EIndex.OrderLot].Text = bbl.Z_SetStr(mse.OrderLot); 
            detailControls[(int)EIndex.ExhibitionCommonCD].Text = mse.ExhibitionCommonCD;
            detailControls[(int)EIndex.ShouhinCD].Text = mse.ShouhinCD;
            CmbLastYearTerm.SelectedValue = mse.LastYearTerm;
            CmbLastSeason.SelectedValue = mse.LastSeason;
            detailControls[(int)EIndex.LastCatalogNO].Text = mse.LastCatalogNO;
            detailControls[(int)EIndex.LastCatalogPage].Text = mse.LastCatalogPage;
            detailControls[(int)EIndex.LastCatalogText].Text = mse.LastCatalogText;
            detailControls[(int)EIndex.LastInstructionsNO].Text = mse.LastInstructionsNO;
            detailControls[(int)EIndex.LastInstructionsDate].Text = mse.LastInstructionsDate;
            detailControls[(int)EIndex.WebAddress].Text = mse.WebAddress;
            detailControls[(int)EIndex.CmbTag1].Text = mse.TagName1;
            detailControls[(int)EIndex.CmbTag2].Text = mse.TagName2;
            detailControls[(int)EIndex.CmbTag3].Text = mse.TagName3;
            detailControls[(int)EIndex.CmbTag4].Text = mse.TagName4;
            detailControls[(int)EIndex.CmbTag5].Text = mse.TagName5;
            detailControls[(int)EIndex.CmbTag6].Text = mse.TagName6;
            detailControls[(int)EIndex.CmbTag7].Text = mse.TagName7;
            detailControls[(int)EIndex.CmbTag8].Text = mse.TagName8;
            detailControls[(int)EIndex.CmbTag9].Text = mse.TagName9;
            detailControls[(int)EIndex.CmbTag10].Text = mse.TagName10;

            //名称セットのためのCheck処理
            for (int i = 0; i < detailControls.Length; i++)
                switch(i)
                {
                    case (int)EIndex.MainVendorCD:
                    case (int)EIndex.ExhibitionSegmentCD:
                        CheckDetail(i, false);
                        break;
                }

            DataRow[] rows = dtSite.Select("SizeNo = " + parSizeNo + " AND ColorNo = " + parColorNo);           
            DataTable dt = new DataTable();
            if (rows.Length.Equals(0))
            {
                M_ITEM_Entity me = new M_ITEM_Entity();
                me.ITemCD = ""; //登録されていないのであえて
                me.ChangeDate = mie.ChangeDate;
                dt = mibl.M_Site_SelectByItemCD(me);

                foreach (DataRow row in dt.Rows)
                {
                    row["ShouhinCD"] = "";
                }
            }
            else
            {
                dt = dtSite.Clone();
                foreach (DataRow row in rows)
                {
                    dt.ImportRow(row);
                }
            }
            dgvDetail.DataSource = dt;
            if (dt.Rows.Count > 0)
            {
                dgvDetail.ReadOnly = false;
                dgvDetail.Columns[0].ReadOnly = true;
                dgvDetail.Columns[0].DefaultCellStyle.BackColor = System.Drawing.Color.Silver;
                dgvDetail.Columns[1].ReadOnly = false;
                dgvDetail.Refresh();

                dgvDetail.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgvDetail.CurrentCell = dgvDetail[1,0];
                dgvDetail.Enabled = true;
            }
            //else
            //{
            //    //店舗がないことがありえない
            //    dgvDetail.Enabled = false;
            //}
        }
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private M_SKU_Entity GetEntityFromDataTable()
        {
            mse = new M_SKU_Entity
            {
                ColorNO = parColorNo,
                SizeNO = parSizeNo,
                ColorName = parColorName,
                SizeName = parSizeName,
                JanCD = parJancd,
                ITemCD = mie.ITemCD,
                ChangeDate = mie.ChangeDate,
                VariousFLG = mie.VariousFLG,
                SetKBN = mie.SetKBN,
                PresentKBN=mie.PresentKBN,
                SampleKBN=mie.SampleKBN,
                DiscountKBN = mie.DiscountKBN,
                WebFlg = mie.WebFlg,
                RealStoreFlg = mie.RealStoreFlg,
                SKUName=mie.ITemName,
                KanaName=mie.KanaName,
                SKUShortName=mie.SKUShortName,
                EnglishName=mie.EnglishName,
                MakerItem = mie.MakerItem,
                MainVendorCD=mie.MainVendorCD,
                BrandCD = mie.BrandCD,
                BrandName = mie.BrandName,
                TaniCD = mie.TaniCD,
                TaniName = mie.TaniName,
                SportsCD = mie.SportsCD,
                SportsName = mie.SportsName,
                SegmentCD = mie.SegmentCD,
                SegmentName = mie.SegmentName,
                ZaikoKBN = mie.ZaikoKBN,
                NoNetOrderFlg = mie.NoNetOrderFlg,
                EDIOrderFlg = mie.EDIOrderFlg,
                AutoOrderFlg = mie.AutoOrderFlg,
                InventoryAddFlg = mie.InventoryAddFlg,
                MakerAddFlg = mie.MakerAddFlg,
                StoreAddFlg = mie.StoreAddFlg,
                Rack = mie.Rack,
                DirectFlg = mie.DirectFlg,
                ReserveCD = mie.ReserveCD,
                NoticesCD = mie.NoticesCD,
                PostageCD = mie.PostageCD,
                ManufactCD = mie.ManufactCD,
                ConfirmCD = mie.ConfirmCD,
                StopFlg=mie.StopFlg,
                DiscontinueFlg = mie.DiscontinueFlg,
                SoldOutFlg = mie.SoldOutFlg,
                CatalogFlg =mie.CatalogFlg,
                ParcelFlg = mie.ParcelFlg,
                ReserveName = mie.ReserveName,
                NoticesName = mie.NoticesName,
                PostageName = mie.PostageName,
                ManufactName = mie.ManufactName,
                ConfirmName = mie.ConfirmName,
                TaxRateFLG = mie.TaxRateFLG,
                CostingKBN = mie.CostingKBN,
                TaxRateFLGName = mie.TaxRateFLGName,
                CostingKBNName = mie.CostingKBNName,
                PriceWithTax = mie.PriceWithTax,
                PriceOutTax = mie.PriceOutTax,
                OrderPriceWithTax = mie.OrderPriceWithTax,
                OrderPriceWithoutTax = mie.OrderPriceWithoutTax,
                Rate = mie.Rate,
                SaleExcludedFlg = mie.SaleExcludedFlg,
                SaleStartDate = mie.SaleStartDate,
                WebStartDate = mie.WebStartDate,
                OrderAttentionCD = mie.OrderAttentionCD,
                OrderAttentionNote = mie.OrderAttentionNote,
                CommentInStore = mie.CommentInStore,
                CommentOutStore = mie.CommentOutStore,
                ExhibitionSegmentCD = mie.ExhibitionSegmentCD,
                OrderLot = mie.OrderLot,
                //ExhibitionCommonCD = mie.ExhibitionCommonCD,

                LastYearTerm = mie.LastYearTerm,
                LastSeason = mie.LastSeason,
                LastCatalogNO = mie.LastCatalogNO,
                LastCatalogPage = mie.LastCatalogPage,
                LastCatalogText = mie.LastCatalogText,
                LastInstructionsNO = mie.LastInstructionsNO,
                LastInstructionsDate = mie.LastInstructionsDate,
                WebAddress = mie.WebAddress,

                DeleteFlg = mie.DeleteFlg,
                SKUCD = parSKUCD + parSizeNo + parColorNo,
                ApprovalDate = mie.ApprovalDate,
                ShouhinCD = mie.ITemCD, //新規時のみ初期セット

                TagName1 = mie.TagName1,
                TagName2 = mie.TagName2,
                TagName3 = mie.TagName3,
                TagName4 = mie.TagName4,
                TagName5 = mie.TagName5,
                TagName6 = mie.TagName6,
                TagName7 = mie.TagName7,
                TagName8 = mie.TagName8,
                TagName9 = mie.TagName9,
                TagName10 = mie.TagName10
            };

            //DataRow[] row = dtSKU.Select("JANCD = '" + parJancd + "'");
            DataRow[] row = dtSKU.Select("SizeNO = " + parSizeNo + "  AND ColorNo = " + parColorNo);
            if (row.Length == 0)
            {
                return mse;
            }
           
            mse.SKUName = row[0]["SKUName"].ToString();
            mse.KanaName = row[0]["KanaName"].ToString();
            mse.SKUShortName = row[0]["SKUShortName"].ToString();
            mse.EnglishName = row[0]["EnglishName"].ToString();
            mse.ITemCD = row[0]["ItemCD"].ToString();
            //mse.JanCD = row[0]["JanCD"].ToString();
            mse.SetKBN = row[0]["SetKBN"].ToString();
            mse.PresentKBN = row[0]["PresentKBN"].ToString();
            mse.SampleKBN = row[0]["SampleKBN"].ToString();
            mse.DiscountKBN = row[0]["DiscountKBN"].ToString();
            mse.ColorName = row[0]["ColorName"].ToString();
            mse.SizeName = row[0]["SizeName"].ToString();
            //mse.WebFlg = row[0]["WebFlg"].ToString();
            mse.RealStoreFlg = row[0]["RealStoreFlg"].ToString();
            //mse.MainVendorCD = row[0]["MainVendorCD"].ToString();
            //mse.MakerItem = row[0]["MakerItem"].ToString();
            //mse.ZaikoKBN = row[0]["ZaikoKBN"].ToString();
            mse.Rack = row[0]["Rack"].ToString();
            mse.VirtualFlg = row[0]["VirtualFlg"].ToString();
            mse.WebStockFlg = row[0]["WebStockFlg"].ToString();
            mse.StopFlg = row[0]["StopFlg"].ToString();
            mse.DiscontinueFlg = row[0]["DiscontinueFlg"].ToString();
            mse.SoldOutFlg = row[0]["SoldOutFlg"].ToString();
            //mse.InventoryAddFlg = row[0]["InventoryAddFlg"].ToString();
            //mse.MakerAddFlg = row[0]["MakerAddFlg"].ToString();
            //mse.StoreAddFlg = row[0]["StoreAddFlg"].ToString();
            //mse.NoNetOrderFlg = row[0]["NoNetOrderFlg"].ToString();
            //mse.EDIOrderFlg = row[0]["EDIOrderFlg"].ToString();
            mse.CatalogFlg = row[0]["CatalogFlg"].ToString();
            mse.ParcelFlg = row[0]["ParcelFlg"].ToString();
            //mse.AutoOrderFlg = row[0]["AutoOrderFlg"].ToString();
            //mse.TaxRateFLG = row[0]["TaxRateFLG"].ToString();
            //mse.TaxRateFLGName = row[0]["TaxRateFLGName"].ToString();
            //mse.CostingKBN = row[0]["CostingKBN"].ToString();
            //mse.CostingKBNName = row[0]["CostingKBNName"].ToString();
            mse.SaleExcludedFlg = row[0]["SaleExcludedFlg"].ToString();
            mse.PriceWithTax = row[0]["PriceWithTax"].ToString();
            mse.PriceOutTax = row[0]["PriceOutTax"].ToString();
            mse.Rate = row[0]["Rate"].ToString();
            mse.OrderPriceWithTax = row[0]["OrderPriceWithTax"].ToString();
            mse.OrderPriceWithoutTax = row[0]["OrderPriceWithoutTax"].ToString();
            mse.SaleStartDate = row[0]["SaleStartDate"].ToString();
            mse.WebStartDate = row[0]["WebStartDate"].ToString();
            mse.OrderAttentionCD = row[0]["OrderAttentionCD"].ToString();
            mse.OrderAttentionNote = row[0]["OrderAttentionNote"].ToString();
            mse.CommentInStore = row[0]["CommentInStore"].ToString();
            mse.CommentOutStore = row[0]["CommentOutStore"].ToString();
            mse.ExhibitionSegmentCD = row[0]["ExhibitionSegmentCD"].ToString(); 
            mse.OrderLot = row[0]["OrderLot"].ToString(); 
            mse.ExhibitionCommonCD = row[0]["ExhibitionCommonCD"].ToString(); 
            mse.LastYearTerm = row[0]["LastYearTerm"].ToString();
            mse.LastSeason = row[0]["LastSeason"].ToString();
            mse.LastCatalogNO = row[0]["LastCatalogNO"].ToString();
            mse.LastCatalogPage = row[0]["LastCatalogPage"].ToString();
            mse.LastCatalogText = row[0]["LastCatalogText"].ToString();
            mse.LastInstructionsNO = row[0]["LastInstructionsNO"].ToString();
            mse.LastInstructionsDate = row[0]["LastInstructionsDate"].ToString();
            mse.WebAddress = row[0]["WebAddress"].ToString();
            
            if (mse.SetKBN == "1")
            {
                mse.SetAdminCD = row[0]["SetAdminCD"].ToString();
                mse.SetItemCD = row[0]["SetItemCD"].ToString();
                mse.SetSKUCD = row[0]["SetSKUCD"].ToString();
                mse.SetSU = row[0]["SetSU"].ToString();
            }
            else
            {
                mse.SetAdminCD = "";
                mse.SetItemCD = "";
                mse.SetSKUCD = "";
                mse.SetSU = "";
            }
            mse.ApprovalDate = row[0]["ApprovalDate"].ToString();
            mse.ShouhinCD = "";

            mse.TagName1 = string.IsNullOrWhiteSpace(row[0]["TagName1"].ToString()) ? mse.TagName1 : row[0]["TagName1"].ToString();
            mse.TagName2 = string.IsNullOrWhiteSpace(row[0]["TagName2"].ToString()) ? mse.TagName2 : row[0]["TagName2"].ToString();
            mse.TagName3 = string.IsNullOrWhiteSpace(row[0]["TagName3"].ToString()) ? mse.TagName3 : row[0]["TagName3"].ToString();
            mse.TagName4 = string.IsNullOrWhiteSpace(row[0]["TagName4"].ToString()) ? mse.TagName4 : row[0]["TagName4"].ToString();
            mse.TagName5 = string.IsNullOrWhiteSpace(row[0]["TagName5"].ToString()) ? mse.TagName5 : row[0]["TagName5"].ToString();
            mse.TagName6 = string.IsNullOrWhiteSpace(row[0]["TagName6"].ToString()) ? mse.TagName6 : row[0]["TagName6"].ToString();
            mse.TagName7 = string.IsNullOrWhiteSpace(row[0]["TagName7"].ToString()) ? mse.TagName7 : row[0]["TagName7"].ToString();
            mse.TagName8 = string.IsNullOrWhiteSpace(row[0]["TagName8"].ToString()) ? mse.TagName8 : row[0]["TagName8"].ToString();
            mse.TagName9 = string.IsNullOrWhiteSpace(row[0]["TagName9"].ToString()) ? mse.TagName9 : row[0]["TagName9"].ToString();
            mse.TagName10 = string.IsNullOrWhiteSpace(row[0]["TagName10"].ToString()) ? mse.TagName10 : row[0]["TagName10"].ToString();

            //チェックボックス
            if (checkDeleteFlg.Checked)
                mse.DeleteFlg = "1";
            else
                mse.DeleteFlg = "0";

            mse.UsedFlg = "0";
            mse.InsertOperator = InOperatorCD;
            mse.UpdateOperator = InOperatorCD;
            mse.PC = InPcID;

            return mse;
        }

        protected override void ExecSec()
        {
            if (!CheckAll())
                return;

            //更新処理
            DataTable dt   = mibl.GetGridEntity(dtSKU);
            if (dt.Rows.Count == 0)
            {
                //更新対象なし
                bbl.ShowMessage("E102");
                return;
            }
            DataTable dtS = mibl.GetSiteEntity(dtSite);

            mibl.M_ITEM_Exec(mie, dt, dtS, (short)OperationMode);

            ExecFlg = true;

            if (OperationMode== EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");


            this.Close();
        }
        private bool CheckAll()
        {
            for (int i = 0; i < keyControls.Length; i++)
                if (CheckKey(i, 0, false) == false)
                {
                    keyControls[i].Focus();
                    return false;
                }

            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return false;
                }

            //データをDataTableに
            //DataRow[] row = dtSKU.Select("JANCD = '" + parJancd + "'");
            DataRow[] row = dtSKU.Select("SizeNO = " + parSizeNo + "  AND ColorNo = " + parColorNo);
            DataRow newrow = dtSKU.NewRow();
            if (row.Length != 0)
            {                
                newrow = row[0];
            }

            newrow["ITemCD"] = mie.ITemCD;
            newrow["SKUCD"] = detailLabels[(int)EIndex.SKUCD].Text;
            newrow["ChangeDate"] = detailLabels[(int)EIndex.ChangeDate].Text;
            newrow["ColorNO"] = detailLabels[(int)EIndex.ColorNo].Text;
            newrow["SizeNO"] = detailLabels[(int)EIndex.SizeNo].Text;
            newrow["ColorName"] = detailLabels[(int)EIndex.ColorName].Text;
            newrow["SizeName"] = detailLabels[(int)EIndex.SizeName].Text;
            newrow["MakerItem"] = detailLabels[(int)EIndex.MakerItem].Text;

            newrow["JanCD"] =  keyControls[(int)EIndex.JanCD].Text;
            //newrow["SetAdminCD"] = keyControls[(int)EIndex.SetAdminCD].Text;
            //newrow["SetItemCD"] = keyControls[(int)EIndex.SetItemCD].Text;
            newrow["SetSKUCD"] = keyControls[(int)EIndex.SetSKUCD].Text;
            newrow["SetSU"] = bbl.Z_Set(keyControls[(int)EIndex.SetSU].Text);
            newrow["ApprovalDate"] = keyControls[(int)EIndex.ApprovalDate].Text;

            newrow["SKUName"] = detailControls[(int)EIndex.SKUName].Text;
            newrow["KanaName"] = detailControls[(int)EIndex.KanaName].Text;
            newrow["SKUShortName"] = detailControls[(int)EIndex.SKUShortName].Text;
            newrow["EnglishName"] = detailControls[(int)EIndex.EnglishName].Text;
                        
            newrow["VariousFLG"] = ((CheckBox)detailControls[(int)EIndex.ChkVariousFLG]).Checked ? 1 : 0;
            newrow["SetKBN"] = ((CheckBox)detailControls[(int)EIndex.ChkSetKBN]).Checked ? 1 : 0;
            newrow["PresentKBN"] = ((CheckBox)detailControls[(int)EIndex.ChkPresentKBN]).Checked ? 1 : 0;
            newrow["SampleKBN"] = ((CheckBox)detailControls[(int)EIndex.ChkSampleKBN]).Checked ? 1 : 0;
            newrow["DiscountKBN"] = ((CheckBox)detailControls[(int)EIndex.ChkDiscountKBN]).Checked ? 1 : 0;
            
            newrow["WebFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkWebFlg]).Checked ? 1 : 0;
            newrow["RealStoreFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkRealStoreFlg]).Checked ? 1 : 0;
            newrow["MainVendorCD"] = detailControls[(int)EIndex.MainVendorCD].Text;
            //newrow["MakerVendorCD"] = detailControls[(int)EIndex.MakerVendorCD].Text;

            newrow["ZaikoKBN"] = ((CheckBox)detailControls[(int)EIndex.ChkZaikoKBN]).Checked ? 1 : 0; 
            newrow["Rack"] = detailControls[(int)EIndex.Rack].Text;
            newrow["VirtualFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkVirtualFlg]).Checked ? 1 : 0;
            newrow["DirectFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkDirectFlg]).Checked ? 1 : 0;

            newrow["WebStockFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkWebStockFlg]).Checked ? 1 : 0;
            newrow["StopFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkStopFlg]).Checked ? 1 : 0;
            newrow["DiscontinueFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkDiscontinueFlg]).Checked ? 1 : 0;
            newrow["InventoryAddFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkInventoryAddFlg]).Checked ? 1 : 0;
            newrow["MakerAddFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkMakerAddFlg]).Checked ? 1 : 0;
            newrow["StoreAddFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkStoreAddFlg]).Checked ? 1 : 0;
            newrow["NoNetOrderFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkNoNetOrderFlg]).Checked ? 1 : 0;
            newrow["EDIOrderFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkEDIOrderFlg]).Checked ? 1 : 0;
            newrow["CatalogFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkCatalogFlg]).Checked ? 1 : 0;
            newrow["ParcelFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkParcelFlg]).Checked ? 1 : 0;
            newrow["SoldOutFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkSoldOutFlg]).Checked ? 1 : 0;
            newrow["AutoOrderFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkAutoOrderFlg]).Checked ? 1 : 0;

            newrow["SaleExcludedFlg"] = ((CheckBox)detailControls[(int)EIndex.ChkSaleExcludedFlg]).Checked ? 1 : 0;
            newrow["PriceWithTax"] = bbl.Z_Set(detailControls[(int)EIndex.PriceWithTax].Text);
            newrow["PriceOutTax"] = bbl.Z_Set(detailControls[(int)EIndex.PriceOutTax].Text);
            newrow["OrderPriceWithTax"] = bbl.Z_Set(detailControls[(int)EIndex.OrderPriceWithTax].Text);
            newrow["OrderPriceWithoutTax"] = bbl.Z_Set(detailControls[(int)EIndex.OrderPriceWithoutTax].Text);
            newrow["Rate"] = bbl.Z_Set(detailControls[(int)EIndex.Rate].Text);
            newrow["SaleStartDate"] = detailControls[(int)EIndex.SaleStartDate].Text;
            newrow["WebStartDate"] = detailControls[(int)EIndex.WebStartDate].Text;
            newrow["OrderAttentionCD"] = cmbOrderAttentionCD.SelectedIndex > 0 ? cmbOrderAttentionCD.SelectedValue : "";
            newrow["OrderAttentionNote"] = detailControls[(int)EIndex.OrderAttentionNote].Text;
            newrow["CommentInStore"] = detailControls[(int)EIndex.CommentInStore].Text;
            newrow["CommentOutStore"] = detailControls[(int)EIndex.CommentOutStore].Text;
            newrow["ExhibitionSegmentCD"] = detailControls[(int)EIndex.ExhibitionSegmentCD].Text;
            newrow["OrderLot"] = bbl.Z_Set(detailControls[(int)EIndex.OrderLot].Text);
            newrow["ExhibitionCommonCD"] = detailControls[(int)EIndex.ExhibitionCommonCD].Text;
            newrow["LastYearTerm"] = CmbLastYearTerm.SelectedIndex > 0 ? CmbLastYearTerm.SelectedValue : ""; 
            newrow["LastSeason"] = CmbLastSeason.SelectedIndex > 0 ? CmbLastSeason.SelectedValue : ""; 
            newrow["LastCatalogNO"] = detailControls[(int)EIndex.LastCatalogNO].Text;
            newrow["LastCatalogPage"] = detailControls[(int)EIndex.LastCatalogPage].Text;
            newrow["LastCatalogText"] = detailControls[(int)EIndex.LastCatalogText].Text;
            newrow["LastInstructionsNO"] = detailControls[(int)EIndex.LastInstructionsNO].Text;
            newrow["LastInstructionsDate"] = detailControls[(int)EIndex.LastInstructionsDate].Text;
            newrow["WebAddress"] = detailControls[(int)EIndex.WebAddress].Text;
            newrow["TagName1"] = detailControls[(int)EIndex.CmbTag1].Text;
            newrow["TagName2"] = detailControls[(int)EIndex.CmbTag2].Text;
            newrow["TagName3"] = detailControls[(int)EIndex.CmbTag3].Text;
            newrow["TagName4"] = detailControls[(int)EIndex.CmbTag4].Text;
            newrow["TagName5"] = detailControls[(int)EIndex.CmbTag5].Text;
            newrow["TagName6"] = detailControls[(int)EIndex.CmbTag6].Text;
            newrow["TagName7"] = detailControls[(int)EIndex.CmbTag7].Text;
            newrow["TagName8"] = detailControls[(int)EIndex.CmbTag8].Text;
            newrow["TagName9"] = detailControls[(int)EIndex.CmbTag9].Text;
            newrow["TagName10"] = detailControls[(int)EIndex.CmbTag10].Text;

            //チェックボックス
            if (checkDeleteFlg.Checked)
                newrow["DeleteFlg"] = 1;
            else
                newrow["DeleteFlg"] = 0;

            if (row.Length == 0)
            {
                dtSKU.Rows.Add(newrow);
            }

            dgvDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);

            for (int rw = 0; rw < dgvDetail.RowCount; rw++)
            {
                DataRow[] rowS = dtSite.Select("SizeNO = " + parSizeNo + "  AND ColorNo = " + parColorNo + " AND StoreCD = '" + dgvDetail[4, rw].Value.ToString() + "'");
                if (rowS.Length != 0)
                {
                    rowS[0]["ShouhinCD"] = dgvDetail[1, rw].Value.ToString();
                }
                else
                {
                    DataRow newSite = dtSite.NewRow();
                    newSite["SizeNO"] = parSizeNo;
                    newSite["ColorNo"] = parColorNo;
                    newSite["StoreCD"] = dgvDetail[4, rw].Value.ToString();
                    newSite["StoreName"] = dgvDetail[0, rw].Value.ToString();
                    newSite["ShouhinCD"] = dgvDetail[1, rw].Value.ToString();
                    newSite["APIKey"] = bbl.Z_Set(dgvDetail[3, rw].Value);
                    newSite["SiteURL"] = dgvDetail[5, rw].Value.ToString();
                    newSite["AdminNO"] = bbl.Z_Set(dgvDetail[2, rw].Value);
                    dtSite.Rows.Add(newSite);
                }
            }
            return true;
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
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CheckBox)) || ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(RadioButton)))
                {
                    ((RadioButton)ctl).Checked = true;
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

            mOldPriceOutTax = 0;
            mOldRate = 0;
            mOldOrderPriceWithoutTax = 0;

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
                                ScSKUCD.SearchEnable = Kbn == 0 ? true : false;
                            else
                            {
                                ScSKUCD.SearchEnable = false;
                                F9Visible = false;
                            }
                                
                                
                            break;
                        }

                    case 1:
                        {
                            //// ｷｰ部(複写)
                            //foreach (Control ctl in copyKeyControls)
                            //{
                            //    ctl.Enabled = Kbn == 0 ? true : false;
                            //}
                            //ScCopyITEM.SearchEnable = Kbn == 0 ? true : false;
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

                            if(Kbn.Equals(0))
                                SetEnabled();   //不使用項目
                            
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
            switch (Index)
            {
                case 0: // F1:戻る
                    {
                        break;
                    }
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会                    
                case 5: //F6:キャンセル
                    break;

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
        {                        //チェック
            if (!CheckAll())
                return;
            
            this.Close();
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
                            case (int)EIndex.JanCD:
                                keyControls[(int)EIndex.ChangeDate].Focus();
                                break;

                            case (int)EIndex.SetSKUCD:
                            case (int)EIndex.SetSU:
                            case (int)EIndex.ApprovalDate:
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
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

        private void ChkSetKbn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetEnabledForSet();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void ChkVirtualFlg_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblVirtualFlg.Visible = ChkVirtualFlg.Checked;
                lblVirtualFlg.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvDetail_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                var dgv = (DataGridView)sender;
                if (dgv.IsCurrentCellDirty)
                {
                    dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
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
