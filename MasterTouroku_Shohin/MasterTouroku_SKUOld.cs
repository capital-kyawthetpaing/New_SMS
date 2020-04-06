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
    /// MasterTouroku_Shouhin 商品ストアマスタ
    /// </summary>
    internal partial class MasterTouroku_SKUOld : FrmMainForm
    {
        private const string ProID = "MasterTouroku_Shouhin";
        private const string ProNm = "商品マスター（SKU)";
        private const short mc_L_END = 3; // ロック用

        //private M_SKU_Entity[] TableSku;

        private enum EIndex : int
        {
            SKUCD,
            ChangeDate
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

          , JanCD=0
          , ColorName
          , SizeName
          , SetAdminCD
          , SetItemCD
          , SetSKUCD
          , SetSU

          , ChkVariousFLG 
          , ChkSetKBN
          , ChkWebFlg
          , ChkRealStoreFlg
          , SKUName
          , KanaName
          , SKUShortName
          , EnglishName

          //, ColorNO
          //, SizeNO

          , MakerItem
          , ChkVirtualFlg
        , ChkDiscontinueFlg
          , ChkStopFlg
        , ChkDiscountKBN
        , ChkZaikoKBN

        , ChkPresentKBN
        , ChkSampleKBN
        , ChkCatalogFlg
        , ChkNoNetOrderFlg
        , ChkEDIOrderFlg
        , ChkAutoOrderFlg

        , ChkDirectFlg
        , ChkParcelFlg
        , ChkSaleExcludedFlg

        , ChkWebStockFlg
        , ChkInventoryAddFlg
        , ChkMakerAddFlg
        , ChkStoreAddFlg

        , MainVendorCD
          , Rack

          , MakerVendorCD
          

          , SaleStartDate
        , WebStartDate

          , LastYearTerm
          , LastSeason
          , LastCatalogNO
          , LastCatalogPage
          , LastCatalogText
          , LastInstructionsNO
          , LastInstructionsDate
          
          , PriceWithTax
          , PriceOutTax
          , OrderPriceWithTax
          , OrderPriceWithoutTax

          , OrderAttentionCD
          , OrderAttentionNote
          , CommentInStore
          , CommentOutStore
          
          , WebAddress
          //, ApprovalDate
                
          , DeleteFlg
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            SKUCD
        }
        private Control[] keyControls;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private SKU_BL mbl;
        private M_ITEM_Entity mie;
        private M_SKU_Entity mse;
        private DataTable dtSKU;

        //パラメータ
        public string parSkucd = "";

        //private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        public MasterTouroku_SKUOld(M_ITEM_Entity me, DataTable dt)
        {
            InitializeComponent();

            mie = me;
            dtSKU = dt;
        }

        private void MasterTouroku_SKU_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                //this.SetFunctionLabel(EProMode.MENTE);
                this.InitialControlArray();
                mbl = new SKU_BL();

                ////起動時共通処理
                //base.StartProgram();
                Scr_Clr(0);
                GetEntityFromDataTable();
                SetData();

               detailControls[0].Focus();
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
            detailControls = new Control[] { ckM_CheckBox1,ckM_CheckBox2,ckM_CheckBox3,ckM_CheckBox4,ckM_CheckBox5,ckM_CheckBox6
                ,ckM_CheckBox7,ckM_CheckBox8,ckM_CheckBox9,ckM_CheckBox10,ckM_CheckBox11,ckM_CheckBox12
                ,ckM_CheckBox13,ckM_CheckBox14,ckM_CheckBox15,ckM_CheckBox16,ckM_CheckBox17,ckM_CheckBox18


                , ckM_TextBox6, ckM_TextBox20, ckM_TextBox24, ckM_TextBox25
                                    , ScMall.TxtCode, ckM_TextBox2, ckM_TextBox1
                                    , ckM_TextBox4, ckM_TextBox7
                                    , ckM_TextBox21, ckM_TextBox22, ckM_TextBox23, ScKouza.TxtCode, ScStaff11.TxtCode
                                    , ScStaff12.TxtCode, ScStaff21.TxtCode, ScStaff22.TxtCode, ScStaff31.TxtCode, ScStaff32.TxtCode
                                    , ckM_TextBox18, ckM_TextBox17, ckM_TextBox16, ckM_TextBox15
                                    , ckM_TextBox8, ckM_TextBox9, ckM_TextBox11, ckM_TextBox10, ckM_TextBox13, ckM_TextBox12
                                    , TxtRemark};
            detailLabels = new CKM_SearchControl[] { ScMall, ScKouza, ScStaff11, ScStaff12
                                , ScStaff21, ScStaff22, ScStaff31, ScStaff32};
            searchButtons = new Control[] { ScMall.BtnSearch,ScKouza.BtnSearch, ScStaff11.BtnSearch,ScStaff12.BtnSearch,ScStaff22.BtnSearch,
                ScStaff21.BtnSearch,ScStaff32.BtnSearch,ScStaff31.BtnSearch,ScCopyStore.BtnSearch,ScStore.BtnSearch };

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
        private void SetData()
        {
            //Label
            detailLabels[(int)EIndex.SKUCD].Text = mse.SKUCD;
            detailLabels[(int)EIndex.ChangeDate].Text = mse.ChangeDate;
            detailLabels[(int)EIndex.BrandCD].Text = mse.BrandCD;
            detailLabels[(int)EIndex.BrandName].Text = mse.BrandName;
            detailLabels[(int)EIndex.TaniCD].Text = mse.TaniCD;
            detailLabels[(int)EIndex.TaniName].Text = mse.TaniCD;
            detailLabels[(int)EIndex.SportsCD].Text = mse.SportsCD;
            detailLabels[(int)EIndex.SportsName].Text = mse.SportsName;
            detailLabels[(int)EIndex.ReserveName].Text = mse.ReserveName;
            detailLabels[(int)EIndex.NoticesName].Text = mse.NoticesName;
            detailLabels[(int)EIndex.PostageName].Text = mse.PostageName;
            detailLabels[(int)EIndex.ManufactName].Text = mse.ManufactName;
            detailLabels[(int)EIndex.ConfirmName].Text = mse.ConfirmName;

            detailLabels[(int)EIndex.TaxRateFLGName].Text = mse.TaxRateFLGName;
            detailLabels[(int)EIndex.CostingKBNName].Text = mse.CostingKBNName;

            //Text
            if (mse.VariousFLG.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkVariousFLG]).Checked = true;
            }
            detailControls[(int)EIndex.SKUName].Text = mse.SKUName;
            detailControls[(int)EIndex.KanaName].Text = mse.KanaName;
            detailControls[(int)EIndex.SKUShortName].Text = mse.SKUShortName;
            detailControls[(int)EIndex.EnglishName].Text = mse.EnglishName;
            //detailControls[(int)EIndex.SetItemCD].Text = mse.ITemCD;
            //detailControls[(int)EIndex.ColorNO].Text = mse.ColorNO;
            //detailControls[(int)EIndex.SizeNO].Text = mse.SizeNO;
            detailControls[(int)EIndex.JanCD].Text = mse.JanCD;

            if (mse.SetKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkSetKBN]).Checked = true;
            }
            if (mse.PresentKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkPresentKBN]).Checked = true;
            }
            if (mse.SampleKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkSampleKBN]).Checked = true;
            }

            if (mse.DiscountKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkDiscountKBN]).Checked = true;
            }
            if (mse.WebFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkWebFlg]).Checked = true;
            }
            if (mse.RealStoreFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkRealStoreFlg]).Checked = true;
            }
            if (mse.VirtualFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkVirtualFlg]).Checked = true;
            }
            if (mse.DirectFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkDirectFlg]).Checked = true;
            }
            if (mse.WebStockFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkWebStockFlg]).Checked = true;
            }
            if (mse.StopFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkStopFlg]).Checked = true;
            }
            if (mse.DiscontinueFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkDiscontinueFlg]).Checked = true;
            }
            if (mse.InventoryAddFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkInventoryAddFlg]).Checked = true;
            }
            if (mse.MakerAddFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkMakerAddFlg]).Checked = true;
            }
            if (mse.StoreAddFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkStoreAddFlg]).Checked = true;
            }
            if (mse.NoNetOrderFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkNoNetOrderFlg]).Checked = true;
            }
            if (mse.EDIOrderFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkEDIOrderFlg]).Checked = true;
            }
            if (mse.CatalogFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkCatalogFlg]).Checked = true;
            }
            if (mse.ParcelFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkParcelFlg]).Checked = true;
            }
            if (mse.AutoOrderFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkAutoOrderFlg]).Checked = true;
            }
            if (mse.ZaikoKBN.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkZaikoKBN]).Checked = true;
            }
            if (mse.SaleExcludedFlg.Equals("1"))
            {
                ((CheckBox)detailControls[(int)EIndex.ChkSaleExcludedFlg]).Checked = true;
            }

            detailControls[(int)EIndex.ColorName].Text = mse.ColorName;
            detailControls[(int)EIndex.SizeName].Text = mse.SizeName;
            detailControls[(int)EIndex.MainVendorCD].Text = mse.MainVendorCD;
            detailControls[(int)EIndex.MakerVendorCD].Text = mse.MakerVendorCD;

            detailControls[(int)EIndex.MakerItem].Text = mse.MakerItem;
            detailControls[(int)EIndex.Rack].Text = mse.Rack;
            detailControls[(int)EIndex.TaxRateFLGName].Text = mse.TaxRateFLG;
            detailControls[(int)EIndex.CostingKBNName].Text = mse.CostingKBN;
            detailControls[(int)EIndex.PriceWithTax].Text = mse.PriceWithTax;
            detailControls[(int)EIndex.PriceOutTax].Text = mse.PriceOutTax;
            detailControls[(int)EIndex.OrderPriceWithTax].Text = mse.OrderPriceWithTax;
            detailControls[(int)EIndex.OrderPriceWithoutTax].Text = mse.OrderPriceWithoutTax;
            detailControls[(int)EIndex.SaleStartDate].Text = mse.SaleStartDate;
            detailControls[(int)EIndex.WebStartDate].Text = mse.WebStartDate;
            detailControls[(int)EIndex.OrderAttentionCD].Text = mse.OrderAttentionCD;
            detailControls[(int)EIndex.OrderAttentionNote].Text = mse.OrderAttentionNote;
            detailControls[(int)EIndex.CommentInStore].Text = mse.CommentInStore;
            detailControls[(int)EIndex.CommentOutStore].Text = mse.CommentOutStore;
            detailControls[(int)EIndex.LastYearTerm].Text = mse.LastYearTerm;
            detailControls[(int)EIndex.LastSeason].Text = mse.LastSeason;
            detailControls[(int)EIndex.LastCatalogNO].Text = mse.LastCatalogNO;
            detailControls[(int)EIndex.LastCatalogPage].Text = mse.LastCatalogPage;
            detailControls[(int)EIndex.LastCatalogText].Text = mse.LastCatalogText;
            detailControls[(int)EIndex.LastInstructionsNO].Text = mse.LastInstructionsNO;
            detailControls[(int)EIndex.LastInstructionsDate].Text = mse.LastInstructionsDate;
            detailControls[(int)EIndex.WebAddress].Text = mse.WebAddress;
            detailControls[(int)EIndex.SetAdminCD].Text = mse.SetAdminCD;
            detailControls[(int)EIndex.SetItemCD].Text = mse.SetItemCD;
            detailControls[(int)EIndex.SetSKUCD].Text = mse.SetSKUCD;
            detailControls[(int)EIndex.SetSU].Text = mse.SetSU;
            //detailControls[(int)EIndex.ApprovalDate].Text = mse.ApprovalDate;
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
                    case (int)EIndex.JanCD:
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

            if (index == (int)EIndex.ChangeDate)
            {
                //[M_SKU_Select]
                mse = GetEntity(kbn);
                DataTable dtSKU = new DataTable(); 
                    bool ret = mbl.M_SKU_Select(mse);

                //・新規モードの場合（In the case of "new" mode）
                if (OperationMode == EOperationMode.INSERT && kbn == 0)
                {
                    //以下の条件で商品マスター(M_Store)が存在すればエラー (Error if record exists)Ｅ１３２
                    if (dtSKU.Rows.Count > 0)
                    {
                        bbl.ShowMessage("E132");
                        return false;
                    }
                }
                else
                {
                    //以下の条件で商品マスター(M_Store)が存在しなければエラー (Error if record does not exist)Ｅ１３３
                    if (dtSKU.Rows.Count == 0)
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

                        //for (int i = (int)EIndex.StoreKBN; i <= (int)EIndex.DeleteFlg; i++)
                        //{
                        //    if (i == (int)EIndex.StoreKBN)
                        //    {
                        //        //1:実商品、2:WEB店、3:まとめ商品
                        //        switch (dtSKU.Rows[0][i + col].ToString())
                        //        {
                        //            case "1":
                        //                radioButton1.Checked = true;
                        //                break;

                        //            case "2":
                        //                radioButton2.Checked = true;
                        //                break;

                        //            case "3":
                        //                radioButton3.Checked = true;
                        //                break;
                        //        }

                        //    }
                        //    else if (i == (int)EIndex.StorePlaceKBN)
                        //    {
                        //        //1:本社本店、2:豊中、3:石橋、4:江坂、5:三宮
                        //        switch (dtSKU.Rows[0]["StorePlaceKBN"].ToString())//i + col
                        //        {
                        //            case "1":
                        //                radioButton4.Checked = true;
                        //                break;

                        //            case "2":
                        //                radioButton5.Checked = true;
                        //                break;

                        //            case "3":
                        //                radioButton6.Checked = true;
                        //                break;

                        //            case "4":
                        //                radioButton7.Checked = true;
                        //                break;

                        //            case "5":
                        //                radioButton8.Checked = true;
                        //                break;

                        //        }

                        //    }
                        //    else if (i == (int)EIndex.DeleteFlg)
                        //    {
                        //        if (dtSKU.Rows[0][i + col].ToString() == "1")
                        //        {
                        //            //CheckBoxをONに
                        //            checkDeleteFlg.Checked = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        detailControls[i].Text = dtSKU.Rows[0][i + col].ToString();
                        //    }

                        

                        //}                    
                    }
                }

                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                {
                    //画面へデータセット後、明細部入力可、キー部入力不可
                    Scr_Lock(3, 3, 0);
                    SetEnabled();

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
            switch (index)
            {
                case (int)EIndex.SKUName:
                    //商品ストア名 入力必須(Entry required)
                    if (detailControls[index].Text == "")
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    break;

                case (int)EIndex.BrandCD:
                    //ブランドCD 入力必須(Entry required)
                    if (detailControls[index].Enabled && detailControls[index].Text == "")
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    else if (detailControls[index].Text != "")
                    {
                        ////以下の条件でM_MultiPorposeが存在しない場合、エラー

                        ////[M_MultiPorpose]
                        //M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity
                        //{
                        //    ID = MultiPorpose_BL.ID_MALL,
                        //    Key = detailControls[index].Text
                        //};
                        //MultiPorpose_BL mbl = new MultiPorpose_BL();
                        //DataTable dt = mbl.M_MultiPorpose_Select(mme);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    ScMall.LabelText = dt.Rows[0]["Char1"].ToString();
                        //}
                        //else
                        //{
                        //    //Ｅ１０１
                        //    bbl.ShowMessage("E101");
                        //    ScMall.LabelText = "";
                        //    return false;
                        //}
                    }
                    break;

            

                //case (int)EIndex.Address1:
                //case (int)EIndex.TelphoneNO:
                //case (int)EIndex.FaxNO:
                //    //住所 入力必須(Entry required)
                //    if (detailControls[index].Enabled && detailControls[index].Text == "")
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        return false;
                //    }
                //    break;

                //case (int)EIndex.KouzaCD:
                //    //口座CD 入力必須(Entry required)
                //    if (detailControls[index].Enabled && detailControls[index].Text == "")
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        return false;
                //    }
                //    else if (detailControls[index].Text != "")
                //    {
                //        //以下の条件でM_Kouzaが存在しない場合、エラー

                //        //[M_Kouza]
                //        M_Kouza_Entity mke = new M_Kouza_Entity
                //        {
                //            KouzaCD = detailControls[index].Text,
                //            ChangeDate = keyControls[(int)EIndex.ChangeDate].Text
                //        };
                //        Kouza_BL kbl = new Kouza_BL();
                //        DataTable dt = kbl.M_Kouza_Select(mke);
                //        if (dt.Rows.Count > 0)
                //        {
                //            ScKouza.LabelText = dt.Rows[0]["KouzaName"].ToString();

                //            //DeleteFlg = 1の場合、エラー
                //            if (dt.Rows[0]["DeleteFlg"].ToString() == "1")
                //            {
                //                //Ｅ１５８
                //                bbl.ShowMessage("E158");
                //                return false;
                //            }
                //        }
                //        else
                //        {
                //            //Ｅ１０１
                //            ScKouza.LabelText = "";
                //            bbl.ShowMessage("E101");
                //            return false;
                //        }
                //    }
                //    break;

                //case (int)EIndex.ApprovalStaffCD11:
                //case (int)EIndex.ApprovalStaffCD12:
                //case (int)EIndex.ApprovalStaffCD21:
                //case (int)EIndex.ApprovalStaffCD22:
                //case (int)EIndex.ApprovalStaffCD31:
                //case (int)EIndex.ApprovalStaffCD32:
                //    //一次承認スタッフCD 入力必須(Entry required)
                //    if (index == (int)EIndex.ApprovalStaffCD11 && detailControls[index].Enabled && detailControls[index].Text == "")
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        return false;
                //    }
                //    else if (detailControls[index].Text != "")
                //    {
                //        //以下の条件でM_Staffが存在しない場合、エラー

                //        //[M_Staff]
                //        M_Staff_Entity mse = new M_Staff_Entity
                //        {
                //            StaffCD = detailControls[index].Text,
                //            ChangeDate = keyControls[(int)EIndex.ChangeDate].Text
                //        };
                //        Staff_BL mbl = new Staff_BL();
                //        bool ret = mbl.M_Staff_Select(mse);
                //        if (ret)
                //        {
                //            if (index == (int)EIndex.ApprovalStaffCD11)
                //                ScStaff11.LabelText = mse.StaffName;
                //            else if (index == (int)EIndex.ApprovalStaffCD12)
                //                ScStaff12.LabelText = mse.StaffName;
                //            else if (index == (int)EIndex.ApprovalStaffCD21)
                //                ScStaff21.LabelText = mse.StaffName;
                //            else if (index == (int)EIndex.ApprovalStaffCD22)
                //                ScStaff22.LabelText = mse.StaffName;
                //            else if (index == (int)EIndex.ApprovalStaffCD31)
                //                ScStaff31.LabelText = mse.StaffName;
                //            else if (index == (int)EIndex.ApprovalStaffCD32)
                //                ScStaff32.LabelText = mse.StaffName;

                //            //以下の条件を満たせばエラー
                //            if (mse.LeaveDate != "")
                //            {
                //                //if ( mse.LeaveDate <= keyControls[(int)EIndex.ChangeDate].Text)
                //                int result = mse.LeaveDate.CompareTo(keyControls[(int)EIndex.ChangeDate].Text);
                //                if (result <= 0)
                //                {
                //                    //Ｅ１３５
                //                    bbl.ShowMessage("E135");
                //                    return false;
                //                }
                //            }

                //            //DeleteFlg = 1の場合、エラー
                //            if (mse.DeleteFlg == "1")
                //            {
                //                //Ｅ１５８
                //                bbl.ShowMessage("E158");
                //                return false;
                //            }
                //        }
                //        else
                //        {
                //            //Ｅ１０１
                //            bbl.ShowMessage("E101");

                //            if (index == (int)EIndex.ApprovalStaffCD11)
                //                ScStaff11.LabelText = "";
                //            else if (index == (int)EIndex.ApprovalStaffCD12)
                //                ScStaff12.LabelText = "";
                //            else if (index == (int)EIndex.ApprovalStaffCD21)
                //                ScStaff21.LabelText = "";
                //            else if (index == (int)EIndex.ApprovalStaffCD22)
                //                ScStaff22.LabelText = "";
                //            else if (index == (int)EIndex.ApprovalStaffCD31)
                //                ScStaff31.LabelText = "";
                //            else if (index == (int)EIndex.ApprovalStaffCD32)
                //                ScStaff32.LabelText = "";

                //            return false;
                //        }

                //        //重複チェック
                //        for (int i = (int)EIndex.ApprovalStaffCD11; i < index; i++)
                //        {
                //            for (int j = i + 1; j <= index; j++)
                //            {
                //                if (detailControls[i].Text != "" && detailControls[j].Text != "")
                //                    if (detailControls[i].Text == detailControls[j].Text)
                //                    {
                //                        //Ｅ１０５
                //                        bbl.ShowMessage("E105");
                //                        return false;
                //                    }
                //            }
                //        }
                //    }
                //    break;
            }

            return true;
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private M_SKU_Entity GetEntityFromDataTable()
        {
            mse = new M_SKU_Entity();
            DataRow[] row = dtSKU.Select("SKUCD = '" + parSkucd + "'"); 
            if (row.Length == 0)
            {
                return mse;
            }

            mse.JanCD = row[0]["JanCD"].ToString();
            mse.ChangeDate = row[0]["ChangeDate"].ToString();

            mse.VariousFLG = row[0]["VariousFLG"].ToString();
            mse.SKUName = row[0]["SKUName"].ToString();
            mse.KanaName = row[0]["KanaName"].ToString();
            mse.SKUShortName = row[0]["SKUShortName"].ToString();
            mse.EnglishName = row[0]["EnglishName"].ToString();
            //mse.ITemCD = row[0]["ItemCD"].ToString();
            //mse.ColorNO = row[0]["ColorNO"].ToString();
            //mse.SizeNO = row[0]["SizeNO"].ToString();
            mse.JanCD = row[0]["JanCD"].ToString();
            mse.SetKBN = row[0]["SetKBN"].ToString();
            mse.PresentKBN = row[0]["PresentKBN"].ToString();
            mse.SampleKBN = row[0]["SampleKBN"].ToString();
            mse.DiscountKBN = row[0]["DiscountKBN"].ToString();
            mse.ColorName = row[0]["ColorName"].ToString();
            mse.SizeName = row[0]["SizeName"].ToString();
            mse.WebFlg = row[0]["WebFlg"].ToString();
            mse.RealStoreFlg = row[0]["RealStoreFlg"].ToString();
            mse.MainVendorCD = row[0]["MainVendorCD"].ToString();
            mse.MakerVendorCD = row[0]["MakerVendorCD"].ToString();
            mse.BrandCD = row[0]["BrandCD"].ToString();
            mse.MakerItem = row[0]["MakerItem"].ToString();
            mse.TaniCD = row[0]["TaniCD"].ToString();
            mse.SportsCD = row[0]["SportsCD"].ToString();
            mse.ZaikoKBN = row[0]["ZaikoKBN"].ToString();
            mse.Rack = row[0]["Rack"].ToString();
            mse.VirtualFlg = row[0]["VirtualFlg"].ToString();
            mse.DirectFlg = row[0]["DirectFlg"].ToString();
            mse.ReserveCD = row[0]["ReserveName"].ToString();
            mse.NoticesCD = row[0]["NoticesName"].ToString();
            mse.PostageCD = row[0]["PostageName"].ToString();
            mse.ManufactCD = row[0]["ManufactName"].ToString();
            mse.ConfirmCD = row[0]["ConfirmName"].ToString();
            mse.WebStockFlg = row[0]["WebStockFlg"].ToString();
            mse.StopFlg = row[0]["StopFlg"].ToString();
            mse.DiscontinueFlg = row[0]["DiscontinueFlg"].ToString();
            mse.InventoryAddFlg = row[0]["InventoryAddFlg"].ToString();
            mse.MakerAddFlg = row[0]["MakerAddFlg"].ToString();
            mse.StoreAddFlg = row[0]["StoreAddFlg"].ToString();
            mse.NoNetOrderFlg = row[0]["NoNetOrderFlg"].ToString();
            mse.EDIOrderFlg = row[0]["EDIOrderFlg"].ToString();
            mse.CatalogFlg = row[0]["CatalogFlg"].ToString();
            mse.ParcelFlg = row[0]["ParcelFlg"].ToString();
            mse.AutoOrderFlg = row[0]["AutoOrderFlg"].ToString();
            mse.TaxRateFLG = row[0]["TaxRateFLGName"].ToString();
            mse.CostingKBN = row[0]["CostingKBNName"].ToString();
            mse.SaleExcludedFlg = row[0]["SaleExcludedFlg"].ToString();
            mse.PriceWithTax = row[0]["PriceWithTax"].ToString();
            mse.PriceOutTax = row[0]["PriceOutTax"].ToString();
            mse.OrderPriceWithTax = row[0]["OrderPriceWithTax"].ToString();
            mse.OrderPriceWithoutTax = row[0]["OrderPriceWithoutTax"].ToString();
            mse.SaleStartDate = row[0]["SaleStartDate"].ToString();
            mse.WebStartDate = row[0]["WebStartDate"].ToString();
            mse.OrderAttentionCD = row[0]["OrderAttentionCD"].ToString();
            mse.OrderAttentionNote = row[0]["OrderAttentionNote"].ToString();
            mse.CommentInStore = row[0]["CommentInStore"].ToString();
            mse.CommentOutStore = row[0]["CommentOutStore"].ToString();
            mse.LastYearTerm = row[0]["LastYearTerm"].ToString();
            mse.LastSeason = row[0]["LastSeason"].ToString();
            mse.LastCatalogNO = row[0]["LastCatalogNO"].ToString();
            mse.LastCatalogPage = row[0]["LastCatalogPage"].ToString();
            mse.LastCatalogText = row[0]["LastCatalogText"].ToString();
            mse.LastInstructionsNO = row[0]["LastInstructionsNO"].ToString();
            mse.LastInstructionsDate = row[0]["LastInstructionsDate"].ToString();
            mse.WebAddress = row[0]["WebAddress"].ToString();
            mse.SetAdminCD = row[0]["SetAdminCD"].ToString();
            mse.SetItemCD = row[0]["SetItemCD"].ToString();
            mse.SetSKUCD = row[0]["SetSKUCD"].ToString();
            mse.SetSU = row[0]["SetSU"].ToString();
            //mse.ApprovalDate = row[0]["ApprovalDate"].ToString();

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
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <param name="kbn"></param>
        /// <returns></returns>
        private M_SKU_Entity GetEntity(short kbn)
        {
            mse = new M_SKU_Entity();

            if (kbn == 0)
            {
                mse.JanCD = keyControls[(int)EIndex.JanCD].Text;
                mse.ChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
            }

            //if (radioButton1.Checked)
            //{
            //    mse.StoreKBN = "1";
            //    if (radioButton4.Checked)
            //        mse.StorePlaceKBN = "1";
            //    else if (radioButton5.Checked)
            //        mse.StorePlaceKBN = "2";
            //    else if (radioButton6.Checked)
            //        mse.StorePlaceKBN = "3";
            //    else if (radioButton7.Checked)
            //        mse.StorePlaceKBN = "4";
            //    else if (radioButton8.Checked)
            //        mse.StorePlaceKBN = "5";
            //}
            //else if (radioButton2.Checked)
            //{
            //    mse.StoreKBN = "2";
            //    mse.StorePlaceKBN = "0";
            //}
            //else if (radioButton3.Checked)
            //{
            //    mse.StoreKBN = "3";
            //    mse.StorePlaceKBN = "0";
            //}


            //Index調整用
            int index = 0;

            mse.VariousFLG = detailControls[index + (int)EIndex.ChkVariousFLG].Text;
            mse.SKUName = detailControls[index + (int)EIndex.SKUName].Text;
            mse.KanaName = detailControls[index + (int)EIndex.KanaName].Text;
            mse.SKUShortName = detailControls[index + (int)EIndex.SKUShortName].Text;
            mse.EnglishName = detailControls[index + (int)EIndex.EnglishName].Text;
            //mse.ITemCD = detailControls[index + (int)EIndex.ItemCD].Text;
            //mse.ColorNO = detailControls[index + (int)EIndex.ColorNO].Text;
            //mse.SizeNO = detailControls[index + (int)EIndex.SizeNO].Text;
            mse.JanCD = detailControls[index + (int)EIndex.JanCD].Text;
            mse.SetKBN = detailControls[index + (int)EIndex.ChkSetKBN].Text;
            mse.PresentKBN = detailControls[index + (int)EIndex.ChkPresentKBN].Text;
            mse.SampleKBN = detailControls[index + (int)EIndex.ChkSampleKBN].Text;
            mse.DiscountKBN = detailControls[index + (int)EIndex.ChkDiscountKBN].Text;
            mse.ColorName = detailControls[index + (int)EIndex.ColorName].Text;
            mse.SizeName = detailControls[index + (int)EIndex.SizeName].Text;
            mse.WebFlg = detailControls[index + (int)EIndex.ChkWebFlg].Text;
            mse.RealStoreFlg = detailControls[index + (int)EIndex.ChkRealStoreFlg].Text;
            mse.MainVendorCD = detailControls[index + (int)EIndex.MainVendorCD].Text;
            mse.MakerVendorCD = detailControls[index + (int)EIndex.MakerVendorCD].Text;
            mse.BrandCD = detailControls[index + (int)EIndex.BrandCD].Text;
            mse.MakerItem = detailControls[index + (int)EIndex.MakerItem].Text;
            mse.TaniCD = detailControls[index + (int)EIndex.TaniCD].Text;
            mse.SportsCD = detailControls[index + (int)EIndex.SportsCD].Text;
            mse.ZaikoKBN = detailControls[index + (int)EIndex.ChkZaikoKBN].Text;
            mse.Rack = detailControls[index + (int)EIndex.Rack].Text;
            mse.VirtualFlg = detailControls[index + (int)EIndex.ChkVirtualFlg].Text;
            mse.DirectFlg = detailControls[index + (int)EIndex.ChkDirectFlg].Text;
            mse.ReserveCD = detailControls[index + (int)EIndex.ReserveName].Text;
            mse.NoticesCD = detailControls[index + (int)EIndex.NoticesName].Text;
            mse.PostageCD = detailControls[index + (int)EIndex.PostageName].Text;
            mse.ManufactCD = detailControls[index + (int)EIndex.ManufactName].Text;
            mse.ConfirmCD = detailControls[index + (int)EIndex.ConfirmName].Text;
            mse.WebStockFlg = detailControls[index + (int)EIndex.ChkWebStockFlg].Text;
            mse.StopFlg = detailControls[index + (int)EIndex.ChkStopFlg].Text;
            mse.DiscontinueFlg = detailControls[index + (int)EIndex.ChkDiscontinueFlg].Text;
            mse.InventoryAddFlg = detailControls[index + (int)EIndex.ChkInventoryAddFlg].Text;
            mse.MakerAddFlg = detailControls[index + (int)EIndex.ChkMakerAddFlg].Text;
            mse.StoreAddFlg = detailControls[index + (int)EIndex.ChkStoreAddFlg].Text;
            mse.NoNetOrderFlg = detailControls[index + (int)EIndex.ChkNoNetOrderFlg].Text;
            mse.EDIOrderFlg = detailControls[index + (int)EIndex.ChkEDIOrderFlg].Text;
            mse.CatalogFlg = detailControls[index + (int)EIndex.ChkCatalogFlg].Text;
            mse.ParcelFlg = detailControls[index + (int)EIndex.ChkParcelFlg].Text;
            mse.AutoOrderFlg = detailControls[index + (int)EIndex.ChkAutoOrderFlg].Text;
            mse.TaxRateFLG = detailControls[index + (int)EIndex.TaxRateFLGName].Text;
            mse.CostingKBN = detailControls[index + (int)EIndex.CostingKBNName].Text;
            mse.SaleExcludedFlg = detailControls[index + (int)EIndex.ChkSaleExcludedFlg].Text;
            mse.PriceWithTax = detailControls[index + (int)EIndex.PriceWithTax].Text;
            mse.PriceOutTax = detailControls[index + (int)EIndex.PriceOutTax].Text;
            mse.OrderPriceWithTax = detailControls[index + (int)EIndex.OrderPriceWithTax].Text;
            mse.OrderPriceWithoutTax = detailControls[index + (int)EIndex.OrderPriceWithoutTax].Text;
            mse.SaleStartDate = detailControls[index + (int)EIndex.SaleStartDate].Text;
            mse.WebStartDate = detailControls[index + (int)EIndex.WebStartDate].Text;
            mse.OrderAttentionCD = detailControls[index + (int)EIndex.OrderAttentionCD].Text;
            mse.OrderAttentionNote = detailControls[index + (int)EIndex.OrderAttentionNote].Text;
            mse.CommentInStore = detailControls[index + (int)EIndex.CommentInStore].Text;
            mse.CommentOutStore = detailControls[index + (int)EIndex.CommentOutStore].Text;
            mse.LastYearTerm = detailControls[index + (int)EIndex.LastYearTerm].Text;
            mse.LastSeason = detailControls[index + (int)EIndex.LastSeason].Text;
            mse.LastCatalogNO = detailControls[index + (int)EIndex.LastCatalogNO].Text;
            mse.LastCatalogPage = detailControls[index + (int)EIndex.LastCatalogPage].Text;
            mse.LastCatalogText = detailControls[index + (int)EIndex.LastCatalogText].Text;
            mse.LastInstructionsNO = detailControls[index + (int)EIndex.LastInstructionsNO].Text;
            mse.LastInstructionsDate = detailControls[index + (int)EIndex.LastInstructionsDate].Text;
            mse.WebAddress = detailControls[index + (int)EIndex.WebAddress].Text;
            mse.SetAdminCD = detailControls[index + (int)EIndex.SetAdminCD].Text;
            mse.SetItemCD = detailControls[index + (int)EIndex.SetItemCD].Text;
            mse.SetSKUCD = detailControls[index + (int)EIndex.SetSKUCD].Text;
            mse.SetSU = detailControls[index + (int)EIndex.SetSU].Text;
            //mse.ApprovalDate = detailControls[index + (int)EIndex.ApprovalDate].Text;

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
            mse = GetEntity(0);
            mbl.M_SKU_Exec(mse, (short)OperationMode);

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
                case EsearchKbn.SKUCD:
                    using (Search_Store frmStore = new Search_Store())
                    {
                        frmStore.parChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
                        frmStore.ShowDialog();

                        if (!frmStore.flgCancel)
                        {
                            setCtl.Text = frmStore.parStoreCD;
                            if (setCtl == keyControls[(int)EIndex.SKUCD])
                                keyControls[(int)EIndex.ChangeDate].Text = frmStore.parChangeDate;
                        }
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

            keyControls[0].Focus(); //商品ストアCD
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
                if (ctl.GetType().Equals(typeof(CheckBox)))
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
                ((CKM_SearchControl)ctl).LabelText = "";
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

                //KTP 2019-06-05 F9 button handle from user control
                //case 8: //F9:検索
                //    EsearchKbn kbn = EsearchKbn.Null;

                //    if (Array.IndexOf(keyControls, previousCtrl) == (int)EIndex.StoreCD ||
                //        Array.IndexOf(copyKeyControls, previousCtrl) == (int)EIndex.StoreCD)
                //    {
                //        //商品検索
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
                            case (int)EIndex.JanCD:
                                keyControls[(int)EIndex.ChangeDate].Focus();
                                break;

                            case (int)EIndex.ChangeDate:
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                {
                                    //CheckKey内で移動先を決定しフォーカスセット
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
            //        //商品検索
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
                //if (radioButton1.Checked)
                //{
                //    //実商品場所：商品区分＝「実商品」の時のみ入力可能。
                //    panel2.Enabled = true;
             
                //    //実商品を選択した場合(When 実商品 is selected)、以下の項目を入力不可にする (Input is possible)
                //    //モールCD
                //    //detailControls[(int)EIndex.MallCD].Enabled = false;
                //    //detailControls[(int)EIndex.MallCD].Text = "";
                //    ScMall.LabelText = "";

                //    ////実商品場所
                //    ////郵便番号
                //    ////住所
                //    ////メールアドレス
                //    ////電話番号
                //    ////FAX番号
                //    ////銀行口座CD
                //    ////【発注承認】の全て
                //    ////【見積初期値】の全て
                //    ////【伝票住所表記】の全て
                //    //for (int i = (int)EIndex.ZipCD1; i <= (int)EIndex.Remarks; i++)
                //    //{
                //    //    detailControls[i].Enabled = true;
                //    //}

                //    //for (int index = 0; index < searchButtons.Length - 2; index++)
                //    //    searchButtons[index].Enabled = true;

                //    //ScMall.SearchEnable = false;   //モール検索
                //}
                //else
                //{
                //    panel2.Enabled = false;

                //    //モールCD
                //    //detailControls[(int)EIndex.MallCD].Enabled = true;

                //    //Web商品、Webまとめ商品を選択した場合(When Web商品 or Webまとめ商品 is selected)、以下の項目を入力不可にする
                //    ////実商品場所
                //    ////郵便番号
                //    ////住所
                //    ////メールアドレス
                //    ////電話番号
                //    ////FAX番号
                //    ////銀行口座CD
                //    ////【発注承認】の全て
                //    ////【見積初期値】の全て
                //    ////【伝票住所表記】の全て
                //    //for (int i = (int)EIndex.ZipCD1; i <= (int)EIndex.Remarks; i++)
                //    //{
                //    //    detailControls[i].Enabled = false;
                //    //    detailControls[i].Text = "";
                //    //}

                //    //for (int index = 0; index < searchButtons.Length - 2; index++)
                //    //    searchButtons[index].Enabled = false;

                //    //ScMall.SearchEnable = true;   //モール検索
                //}
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
