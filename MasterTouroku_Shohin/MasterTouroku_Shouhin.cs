using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using Base.Client;
using BL;
using Entity;
using Search;

namespace MasterTouroku_Shouhin
{
    /// <summary>
    /// MasterTouroku_Shouhin 商品マスタ
    /// </summary>
    internal partial class MasterTouroku_Shouhin : FrmMainForm
    {
        private const string ProID = "MasterTouroku_Shouhin";
        private const string ProNm = "商品マスター（ITEM)";
        private const short mc_L_END = 3; // ロック用        

        private enum EIndex : int
        {
            ItemCD,
            ChangeDate
            //SKUCD,
            //JanCD

          , ChkVariousFLG = 0
          , ChkSetKBN
          , ChkWebFlg
          , ChkRealStoreFlg

          , SKUName
          , SKUShortName
          , KanaName
          , EnglishName
          , MakerItem
        , TaniCD

        , ChkVirtualFlg
        , ChkDiscontinueFlg
        , ChkStopFlg
        , ChkDiscountKBN
        , ChkZaikoKBN
        , Chk6

        , ChkPresentKBN
        , ChkSampleKBN
        , ChkCatalogFlg
        , ChkNoNetOrderFlg
        , ChkEDIOrderFlg
        , ChkAutoOrderFlg

        , ChkDirectFlg
        , ChkParcelFlg
        , ChkSoldOutFlg
        , Chk16
        , Chk17
        , ChkSaleExcludedFlg

        , ChkWebStockFlg
        , ChkInventoryAddFlg
        , ChkMakerAddFlg
        , ChkStoreAddFlg
        , Chk27
        , Chk28

        , BrandCD
        , SegmentCD
        , SportsCD
        , MainVendorCD
        , Rack

          //, MakerVendorCD
          
          , CmbReserveCD
          , CmbNoticesCD
          , CmbPostageCD
          , CmbManufactCD
          , CmbConfirmCD
                    
          , CmbTaxRateFLG
          , CmbCostingKBN
          , SaleStartDate
          , WebStartDate

          , PriceOutTax
          , PriceWithTax
          , Rate
          , OrderPriceWithoutTax
          , OrderPriceWithTax

          , CmbOrderAttentionCD
          , OrderAttentionNote

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

          , ColorNO
          , SizeNO
          //, ColorName
          //, SizeName

          , ApprovalDate                
          , DeleteFlg
          , COUNT
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            JANCD,
            RackNO
        }
        private Control[] keyControls;
        private Control[] copyKeyControls;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private ITEM_BL mibl;
        private M_ITEM_Entity mie;
        private DataTable dtSKU;
        private DataTable dtSite;
        private int mFractionKBN;
        private decimal mOldPriceOutTax;
        private decimal mOldRate;
        private decimal mOldOrderPriceWithoutTax;  

        public MasterTouroku_Shouhin()
        {
            InitializeComponent();
        }

        private void MasterTouroku_Shouhin_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                this.SetFunctionLabel(EProMode.MENTE);
                this.InitialControlArray();
                mibl = new ITEM_BL();

                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();

                HeaderTitleText = ProNm;
                Btn_F10.Text = "展開(F10)";

                ScTani.Value1 = MultiPorpose_BL.ID_TANI;
                ScSegmentCD.Value1 = MultiPorpose_BL.ID_SegmentCD;
                ScSports.Value1 = MultiPorpose_BL.ID_SPORTS;
                ScExhibitionSegmentCD.Value1 = MultiPorpose_BL.ID_ExhibitionSegmentCD;

                string ymd = bbl.GetDate();
                M_SalesTax_Entity mste = new M_SalesTax_Entity
                {
                    ChangeDate = ymd
                };
                SalesTax_BL msbl = new SalesTax_BL();
                bool ret = msbl.M_SalesTax_Select(mste);

                mFractionKBN =Convert.ToInt16( mste.FractionKBN);

                M_Store_Entity me = new M_Store_Entity();
                me.StorePlaceKBN = "1";
                me.StoreKBN = "1";
                me.ChangeDate = ymd;
                me.DeleteFlg = "0";

                //【M_Store_SelectByKbn】
                ret = mibl.M_Store_SelectByKbn(me);

                //【M_Souko】
                M_Souko_Entity mse = new M_Souko_Entity();
                mse.SoukoType = "1";
                mse.ChangeDate = ymd;
                mse.StoreCD = me.StoreCD;
                mse.DeleteFlg = "0";
                mse.searchType = "1";

                //[M_Souko_Search]
                ret = mibl.M_Souko_Search(mse);
                SoukoCD = mse.SoukoCD;

                ScRackNo.Value1 = SoukoCD;

                ckM_ComboBox1.Bind(ymd, "2");
                ckM_ComboBox2.Bind(ymd, "2");
                ckM_ComboBox3.Bind(ymd, "2");
                ckM_ComboBox4.Bind(ymd, "2");
                ckM_ComboBox5.Bind(ymd, "2");
                CmbLastYearTerm.Bind(ymd);
                CmbLastSeason.Bind(ymd);

                for (int i = (int)EIndex.CmbTag1; i <= (int)EIndex.CmbTag10; i++)
                {
                    ((CKM_Controls.CKM_ComboBox)detailControls[i]).Bind(ymd);
                    ((CKM_Controls.CKM_ComboBox)detailControls[i]).DropDownStyle = ComboBoxStyle.DropDown;
                    ((CKM_Controls.CKM_ComboBox)detailControls[i]).AcceptKey = true;
                }
                cmbOrderAttentionCD.Bind(ymd, "2");
                BindCombo("KBN", "Name");

                SetEnabled();

                ScITEM.SetFocus(1);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }
        private void BindCombo(string key, string value)
        {
            //0:非課税、1:通常課税、2:軽減課税
            DataTable dt = new DataTable();
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);
            DataRow datarow = dt.NewRow();
            datarow[key] = "0";
            datarow[value] = "非課税";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "1";
            datarow[value] = "通常課税";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "軽減課税";
            dt.Rows.Add(datarow);

            DataRow dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
            cmbTaxRateFLG.DataSource = dt;
            cmbTaxRateFLG.DisplayMember = value;
            cmbTaxRateFLG.ValueMember = key;

            //1:標準原価、2:総平均原価
            dt = new DataTable();
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);

            datarow = dt.NewRow();
            datarow[key] = "1";
            datarow[value] = "標準原価";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "総平均原価";
            dt.Rows.Add(datarow);

            dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
            cmbCostingKBN.DataSource = dt;
            cmbCostingKBN.DisplayMember = value;
            cmbCostingKBN.ValueMember = key;
        }
            private void SetEnabled()
        {
            for(int i = 0; i< (int)EIndex.COUNT; i++)
            {
                switch(i)
                {
                    case (int)EIndex.ChkVirtualFlg:
                    case (int)EIndex.Chk6:
                    //case (int)EIndex.ChkSoldOutFlg:
                    case (int)EIndex.Chk16:
                    case (int)EIndex.Chk17:
                    case (int)EIndex.Chk27:
                    case (int)EIndex.Chk28:
                        detailControls[i].Enabled = false;
                        break;
                }
            }
        }
        private void InitialControlArray()
        {
            keyControls = new Control[] { ScITEM.TxtCode,ScITEM.TxtChangeDate };
            copyKeyControls = new Control[] { ScCopyITEM.TxtCode,ScCopyITEM.TxtChangeDate };
            detailControls = new Control[] {ckM_CheckBox19,ckM_CheckBox20,ckM_CheckBox21,ckM_CheckBox22
                , ckM_TextBox6, ckM_TextBox24, ckM_TextBox20, ckM_TextBox25, ckM_TextBox4, ScTani.TxtCode

                ,ckM_CheckBox1,ckM_CheckBox2,ckM_CheckBox3,ckM_CheckBox4,ckM_CheckBox5,ckM_CheckBox6
                ,ckM_CheckBox7,ckM_CheckBox8,ckM_CheckBox9,ckM_CheckBox10,ckM_CheckBox11,ckM_CheckBox12
                ,ckM_CheckBox13,ckM_CheckBox14,ckM_CheckBox15,ckM_CheckBox16,ckM_CheckBox17,ckM_CheckBox18
                ,ckM_CheckBox23,ckM_CheckBox24,ckM_CheckBox25,ckM_CheckBox26,ckM_CheckBox27,ckM_CheckBox28
                , ScBrand.TxtCode, ScSegmentCD.TxtCode, ScSports.TxtCode, ScVendor.TxtCode, ScRackNo.TxtCode
                ,ckM_ComboBox1,ckM_ComboBox2,ckM_ComboBox3,ckM_ComboBox4,ckM_ComboBox5,cmbTaxRateFLG,cmbCostingKBN
                , ckM_TextBox18, ckM_TextBox17, ckM_TextBox8, ckM_TextBox9, ckM_TextBox5, ckM_TextBox11, ckM_TextBox10
                ,cmbOrderAttentionCD, ckM_TextBox13
                ,ckM_ComboBox8,ckM_ComboBox9,ckM_ComboBox10,ckM_ComboBox11,ckM_ComboBox12,ckM_ComboBox13
                ,ckM_ComboBox14,ckM_ComboBox15,ckM_ComboBox16,ckM_ComboBox17
                ,ScExhibitionSegmentCD.TxtCode, txtOrderLot
                , CmbLastYearTerm, CmbLastSeason, ckM_TextBox22, ckM_TextBox23,TxtRemark
                , ckM_TextBox3, ckM_MultiLineTextBox1,ckM_MultiLineTextBox2,ckM_MultiLineTextBox3,ckM_MultiLineTextBox4
                , ckM_TextBox2, ckM_TextBox1
                ,ckM_TextBox16, checkDeleteFlg
            };
            detailLabels = new CKM_SearchControl[] { ScBrand, ScTani, ScSegmentCD, ScExhibitionSegmentCD, ScSports, ScVendor};
            searchButtons = new Control[] { ScBrand.BtnSearch,ScTani.BtnSearch,ScSegmentCD.BtnSearch, ScExhibitionSegmentCD.BtnSearch, ScSports.BtnSearch,ScVendor.BtnSearch
                ,ScCopyITEM.BtnSearch,ScITEM.BtnSearch };

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
                    case (int)EIndex.ItemCD:
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { keyControls[index] }))
                        {
                            return false;
                        }

                        break;
                    case (int)EIndex.ChangeDate:
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { keyControls[index] }))
                        {
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
                    case (int)EIndex.ItemCD:
                        break;

                    case (int)EIndex.ChangeDate:
                        if (!CheckKey((int)EIndex.ItemCD))
                        {
                            keyControls[(int)EIndex.ItemCD].Focus();
                            return false;
                        }
                        if (!CheckKey(index))
                        {
                            keyControls[index].Focus();
                            return false;
                        }

                        //複写商品CDに入力がある場合、(When there is an input in 複写商品CD)Ｅ１０２
                        //必須入力
                        if (!string.IsNullOrWhiteSpace( copyKeyControls[(int)EIndex.ItemCD].Text))
                        {
                            if (!RequireCheck(new Control[] { copyKeyControls[index] }))
                            {
                                return false;
                            }
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
                //[M_ITEM_Select]
                mie = GetEntity(kbn, 1);
                bool ret = mibl.M_ITEM_Select(mie);

                if (set)
                {
                    dtSKU = mibl.M_SKU_SelectByItemCD(mie);
                    dtSite = mibl.M_Site_SelectByItemCD(mie);
                }
                //・新規モードの場合（In the case of "new" mode）
                if (OperationMode == EOperationMode.INSERT && kbn == 0)
                {
                    //以下の条件で商品マスター(M_ITEM)が存在すればエラー (Error if record exists)Ｅ１３２
                    if (ret)
                    {
                        bbl.ShowMessage("E132");
                        return false;
                    }
                    if (set)
                    {
                        detailControls[(int)EIndex.MakerItem].Text = mie.ITemCD;
                        detailControls[(int)EIndex.ColorNO].Text = "1";//初期値１
                        detailControls[(int)EIndex.SizeNO].Text = "1";//初期値１
                    }
                    return true;
                }
                else
                {
                    //以下の条件で商品マスター(M_ITEM)が存在しなければエラー (Error if record does not exist)Ｅ１３３
                    if (!ret)
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

                        Scr_Clr(1);

                        detailControls[(int)EIndex.SKUName].Text = mie.ITemName;
                        detailControls[(int)EIndex.KanaName].Text = mie.KanaName;
                        detailControls[(int)EIndex.SKUShortName].Text = mie.SKUShortName;
                        detailControls[(int)EIndex.EnglishName].Text = mie.EnglishName;
                        
                        detailControls[(int)EIndex.ColorNO].Text = mie.ColorNO;
                        detailControls[(int)EIndex.SizeNO].Text = mie.SizeNO;
                        InitGrid();
                        foreach (DataRow row in dtSKU.Rows)
                        {
                            int colNo = Convert.ToInt32(row["SizeNO"]);//Size
                            int rowNo = Convert.ToInt32(row["ColorNO"]);//Color

                            dgvDetail[colNo+1, rowNo].Value = row["JANCD"].ToString();
                            dgvDetail[0, rowNo].Value = rowNo.ToString("0000");
                            dgvDetail[1, rowNo].Value = row["ColorName"].ToString();
                            dgvDetail[colNo+1, 0].Value = row["SizeName"].ToString();

                            if(row["VirtualFlg"].ToString().Equals("1"))
                            {
                                dgvDetail[colNo + 1, rowNo].Style.BackColor = Color.HotPink;
                            }

                            if(kbn.Equals(1))
                            {
                                //複写の場合、元データのITEMCDと適用日をセットしなおす 
                                //AdminNOは振りなおす
                                row["AdminNO"] = 0;
                                row["ItemCD"] = keyControls[(int)EIndex.ItemCD].Text;
                                row["ChangeDate"] = keyControls[(int)EIndex.ChangeDate].Text;
                                row["SKUCD"] = keyControls[(int)EIndex.ItemCD].Text + colNo.ToString("0000") + rowNo.ToString("0000");
                            }
                        }
                        if (dtSKU.Rows.Count>0)
                        {
                            detailControls[(int)EIndex.CmbTag1].Text = dtSKU.Rows[0]["TagName1"].ToString();
                            detailControls[(int)EIndex.CmbTag2].Text = dtSKU.Rows[0]["TagName2"].ToString();
                            detailControls[(int)EIndex.CmbTag3].Text = dtSKU.Rows[0]["TagName3"].ToString();
                            detailControls[(int)EIndex.CmbTag4].Text = dtSKU.Rows[0]["TagName4"].ToString();
                            detailControls[(int)EIndex.CmbTag5].Text = dtSKU.Rows[0]["TagName5"].ToString();
                            detailControls[(int)EIndex.CmbTag6].Text = dtSKU.Rows[0]["TagName6"].ToString();
                            detailControls[(int)EIndex.CmbTag7].Text = dtSKU.Rows[0]["TagName7"].ToString();
                            detailControls[(int)EIndex.CmbTag8].Text = dtSKU.Rows[0]["TagName8"].ToString();
                            detailControls[(int)EIndex.CmbTag9].Text = dtSKU.Rows[0]["TagName9"].ToString();
                            detailControls[(int)EIndex.CmbTag10].Text = dtSKU.Rows[0]["TagName10"].ToString();
                        }

                        if (mie.VariousFLG.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkVariousFLG]).Checked = true;
                        if (mie.SetKBN.Equals("1"))
                                ((CheckBox)detailControls[(int)EIndex.ChkSetKBN]).Checked = true;
                        if (mie.PresentKBN.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkPresentKBN]).Checked = true;
                        if (mie.SampleKBN.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkSampleKBN]).Checked = true;
                        if (mie.DiscountKBN.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkDiscountKBN]).Checked = true;

                        if (mie.WebFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkWebFlg]).Checked = true;
                        if (mie.RealStoreFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkRealStoreFlg]).Checked = true;
                        detailControls[(int)EIndex.MainVendorCD].Text = mie.MainVendorCD;
                        CheckDetail((int)EIndex.MainVendorCD);
                        //detailControls[(int)EIndex.MakerVendorCD].Text = mie.MakerVendorCD;
                        detailControls[(int)EIndex.BrandCD].Text = mie.BrandCD;
                        CheckDetail((int)EIndex.BrandCD);
                        if(kbn.Equals(0))
                            detailControls[(int)EIndex.MakerItem].Text = mie.MakerItem;
                        else
                            detailControls[(int)EIndex.MakerItem].Text = mie.ITemCD;

                        detailControls[(int)EIndex.TaniCD].Text = mie.TaniCD;
                        CheckDetail((int)EIndex.TaniCD);
                        detailControls[(int)EIndex.SportsCD].Text = mie.SportsCD;
                        CheckDetail((int)EIndex.SportsCD);
                        detailControls[(int)EIndex.SegmentCD].Text = mie.SegmentCD;
                        CheckDetail((int)EIndex.SegmentCD);
                        detailControls[(int)EIndex.Rack].Text = mie.Rack;

                        if (mie.ZaikoKBN.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkZaikoKBN]).Checked = true;
                        if (mie.VirtualFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkVirtualFlg]).Checked = true;
                        if (mie.DirectFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkDirectFlg]).Checked = true;
                        ((ComboBox)detailControls[(int)EIndex.CmbReserveCD]).SelectedValue = mie.ReserveCD;
                        ((ComboBox)detailControls[(int)EIndex.CmbNoticesCD]).SelectedValue = mie.NoticesCD;
                        ((ComboBox)detailControls[(int)EIndex.CmbPostageCD]).SelectedValue = mie.PostageCD;
                        ((ComboBox)detailControls[(int)EIndex.CmbManufactCD]).SelectedValue = mie.ManufactCD;
                        ((ComboBox)detailControls[(int)EIndex.CmbConfirmCD]).SelectedValue = mie.ConfirmCD;

                        if (mie.WebStockFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkWebStockFlg]).Checked = true;
                        if (mie.StopFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkStopFlg]).Checked = true;
                        if (mie.DiscontinueFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkDiscontinueFlg]).Checked = true;
                        if (mie.InventoryAddFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkInventoryAddFlg]).Checked = true;
                        if (mie.MakerAddFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkMakerAddFlg]).Checked = true;
                        if (mie.StoreAddFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkStoreAddFlg]).Checked = true;
                        if (mie.NoNetOrderFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkNoNetOrderFlg]).Checked = true;
                        if (mie.EDIOrderFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkEDIOrderFlg]).Checked = true;
                        if (mie.CatalogFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkCatalogFlg]).Checked = true;
                        if (mie.ParcelFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkParcelFlg]).Checked = true;
                        if(mie.SoldOutFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkSoldOutFlg]).Checked = true;
                        if (mie.AutoOrderFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkAutoOrderFlg]).Checked = true;
                        if (mie.SaleExcludedFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkSaleExcludedFlg]).Checked = true;

                       ((ComboBox) detailControls[(int)EIndex.CmbTaxRateFLG]).SelectedValue = mie.TaxRateFLG;
                        ((ComboBox)detailControls[(int)EIndex.CmbCostingKBN]).SelectedValue = mie.CostingKBN;
                        detailControls[(int)EIndex.PriceWithTax].Text =bbl.Z_SetStr( mie.PriceWithTax);
                        detailControls[(int)EIndex.PriceOutTax].Text = bbl.Z_SetStr(mie.PriceOutTax);
                        detailControls[(int)EIndex.OrderPriceWithTax].Text = bbl.Z_SetStr(mie.OrderPriceWithTax);
                        detailControls[(int)EIndex.OrderPriceWithoutTax].Text = bbl.Z_SetStr(mie.OrderPriceWithoutTax);
                        detailControls[(int)EIndex.Rate].Text = bbl.Z_SetStr(mie.Rate);
                        detailControls[(int)EIndex.SaleStartDate].Text = mie.SaleStartDate;
                        detailControls[(int)EIndex.WebStartDate].Text = mie.WebStartDate;
                        cmbOrderAttentionCD.SelectedValue = mie.OrderAttentionCD;
                        detailControls[(int)EIndex.OrderAttentionNote].Text = mie.OrderAttentionNote;
                        detailControls[(int)EIndex.CommentInStore].Text = mie.CommentInStore;
                        detailControls[(int)EIndex.CommentOutStore].Text = mie.CommentOutStore;
                        detailControls[(int)EIndex.ExhibitionSegmentCD].Text = mie.ExhibitionSegmentCD;
                        detailControls[(int)EIndex.OrderLot].Text = mie.OrderLot;

                        ((ComboBox)detailControls[(int)EIndex.CmbLastYearTerm]).SelectedValue = mie.LastYearTerm;
                        ((ComboBox)detailControls[(int)EIndex.CmbLastSeason]).SelectedValue = mie.LastSeason;
                        detailControls[(int)EIndex.LastCatalogNO].Text = mie.LastCatalogNO;
                        detailControls[(int)EIndex.LastCatalogPage].Text = mie.LastCatalogPage;
                        detailControls[(int)EIndex.LastCatalogText].Text = mie.LastCatalogText;
                        detailControls[(int)EIndex.LastInstructionsNO].Text = mie.LastInstructionsNO;
                        detailControls[(int)EIndex.LastInstructionsDate].Text = mie.LastInstructionsDate;
                        detailControls[(int)EIndex.WebAddress].Text = mie.WebAddress;

                        //複写時はセットしない項目
                        if (kbn.Equals(0))
                        {
                            detailControls[(int)EIndex.ApprovalDate].Text = mie.ApprovalDate;

                            if (mie.DeleteFlg.Equals("1"))
                                ((CheckBox)detailControls[(int)EIndex.DeleteFlg]).Checked = true;
                        }                               
                    }
                }

                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                {
                    //画面へデータセット後、明細部入力可、キー部入力不可
                    Scr_Lock(3, 3, 0);
                    Scr_Lock(0, 1, 1);

                    SetFuncKeyAll(this, "111111000101");
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
        /// <summary>
        /// F10:展開ボタン押下時
        /// </summary>
        private void Expand(int ColumnIndex, int RowIndex)
        {
            //展開ボタンClick時   
            try
            {
                if (ColumnIndex < 2 || RowIndex < 1)
                    return;

                //ITEMマスタ項目にエラーがないかチェック
                for (int i = 0; i < detailControls.Length; i++)
                    if (CheckDetail(i) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }

                dgvDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);

                //Gridの全ての値についてチェック

                string jancd = dgvDetail.Rows[RowIndex].Cells[ColumnIndex].EditedFormattedValue.ToString().Trim();
                // 13桁の数字であること Ｅ２２０
                if (!string.IsNullOrWhiteSpace(jancd))
                {
                    if (!IsNumeric(jancd) || jancd.Length != 13)
                    {
                        bbl.ShowMessage("E220");
                        dgvDetail.CurrentCell = dgvDetail[ColumnIndex, RowIndex];
                        dgvDetail.Focus();
                        return;
                    }
                }
                else
                {
                    bbl.ShowMessage("E102");
                    dgvDetail.CurrentCell = dgvDetail[ColumnIndex, RowIndex];
                    dgvDetail.Focus();
                    return;
                }

                //サイズ名、カラー名が入力されていること
                if(dgvDetail.Rows[0].Cells[ColumnIndex].Value == null)
                {
                    bbl.ShowMessage("E102");
                    dgvDetail.CurrentCell = dgvDetail[ColumnIndex, 0];
                    dgvDetail.Focus();
                    return;
                }  
                if (dgvDetail.Rows[RowIndex].Cells[1].Value == null)
                {
                    bbl.ShowMessage("E102");
                    dgvDetail.CurrentCell = dgvDetail[1, RowIndex];
                    dgvDetail.Focus();
                    return;
                }

                ChangeItem();

                mie = GetEntity(0);

                using (MasterTouroku_SKU frmSku = new MasterTouroku_SKU(mie, dtSKU, dtSite, OperationMode))
                {
                    frmSku.parSKUCD = mie.ITemCD;
                    frmSku.parJancd = dgvDetail.Rows[RowIndex].Cells[ColumnIndex].Value.ToString().Trim();
                    frmSku.parColorNo = dgvDetail.Rows[RowIndex].Cells[0].Value.ToString();
                    frmSku.parColorName = dgvDetail.Rows[RowIndex].Cells[1].Value.ToString().Trim();
                    frmSku.parSizeNo = dgvDetail.Columns[ColumnIndex].HeaderText;
                    frmSku.parSizeName = dgvDetail.Rows[0].Cells[ColumnIndex].Value.ToString().Trim();
                    frmSku.setSoukoCD = SoukoCD;
                    frmSku.parFractionKBN = mFractionKBN;

                    frmSku.ShowDialog();
                    dtSKU = frmSku.dtSKU;
                    dtSite = frmSku.dtSite;

                    if (frmSku.ExecFlg)
                    {
                        string item = mie.ITemCD;
                        string date = mie.ChangeDate;
                        ChangeOperationMode(EOperationMode.UPDATE);
                        ScITEM.TxtCode.Text = item;
                        ScITEM.TxtChangeDate.Text = date;
                        CheckKey((int)EIndex.ChangeDate);
                        dgvDetail.CurrentCell = dgvDetail[ColumnIndex, RowIndex];
                        dgvDetail.Focus();
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

        private void InitGrid()
        {
            for (int i = (int)EIndex.ColorNO; i <= (int)EIndex.SizeNO; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            if (dgvDetail.Columns.Count.Equals(0))
            {
                //ColorNo
                dgvDetail.Columns.Add("ColorNo" , " ");
                dgvDetail.Columns[0].Width = 50;
                dgvDetail.Columns[0].ReadOnly = true;
                dgvDetail.Columns[0].DefaultCellStyle.BackColor = Color.Silver;

                //ColorName
                dgvDetail.Columns.Add("ColorName", " ");

            }
            if (dgvDetail.Rows.Count.Equals(0))
            {
                //SizeName
                dgvDetail.Rows.Add();
            }


            int colorNo = Convert.ToInt16(bbl.Z_Set(detailControls[(int)EIndex.ColorNO].Text));
            int sizeNo = Convert.ToInt16(bbl.Z_Set(detailControls[(int)EIndex.SizeNO].Text));

            if (colorNo.Equals(0) || sizeNo.Equals(0))
            {
                dgvDetail.Enabled = false;
                return;
            }

            if (dtSKU == null)
                return;

            if (sizeNo + 2 > dgvDetail.Columns.Count)
            {
                int maxCol = sizeNo + 2 - dgvDetail.Columns.Count;
                for (int i = 0; i < maxCol; i++)
                {
                    dgvDetail.Columns.Add("col" + (dgvDetail.Columns.Count - 1).ToString(), (dgvDetail.Columns.Count-1).ToString("0000"));
                }
            }
            else if (sizeNo + 2 < dgvDetail.Columns.Count)
            {
                int maxCol = dgvDetail.Columns.Count - (sizeNo + 2);
                for (int i = 1; i <= maxCol; i++)
                {
                    DataRow[] rows = dtSKU.Select("SizeNo = " + dgvDetail.Columns[dgvDetail.Columns.Count - 1].HeaderText);
                    foreach (DataRow row in rows)
                    {
                        dtSKU.Rows.Remove(row);
                    }
                    dgvDetail.Columns.RemoveAt(dgvDetail.Columns.Count - 1);
                }
            }

            if (colorNo + 1 > dgvDetail.Rows.Count)
            {
                int maxRow = colorNo + 1 - dgvDetail.Rows.Count;
                for (int i = 0; i < maxRow; i++)
                {
                    dgvDetail.Rows.Add();
                    dgvDetail.Rows[dgvDetail.Rows.Count-1].Cells[0].Value = (dgvDetail.Rows.Count - 1).ToString("0000");
            }
            }
            else if (colorNo + 1 < dgvDetail.Rows.Count)
            {
                int maxRow = dgvDetail.Rows.Count - (colorNo + 1);
                for (int i = 1; i <= maxRow; i++)
                {
                    DataRow[] rows = dtSKU.Select("ColorNo = " + dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells[0].Value.ToString());
                    foreach (DataRow row in rows)
                    {
                        dtSKU.Rows.Remove(row);
                    }
                    dgvDetail.Rows.RemoveAt(dgvDetail.Rows.Count - 1);
                }
            }
            
            //並び替えができないようにする
            foreach (DataGridViewColumn c in dgvDetail.Columns)
                c.SortMode = DataGridViewColumnSortMode.NotSortable;

            dgvDetail.Enabled = true;
            dgvDetail.RowHeadersVisible = false;

            if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
            {
                dgvDetail.ReadOnly = false;
            }
            else
            {
                dgvDetail.ReadOnly = true;
            }
            dgvDetail.Rows[0].Cells[0].Style.BackColor = Color.Silver;
            dgvDetail.Rows[0].Cells[1].Style.BackColor = Color.Silver;
            dgvDetail.Rows[0].Cells[0].ReadOnly = true;
            dgvDetail.Rows[0].Cells[1].ReadOnly = true;
            //DataGridView1の左側2列を固定する
            dgvDetail.Columns[1].Frozen = true;
            //DataGridView1の上部1行を固定する
            dgvDetail.Rows[0].Frozen = true;
        }
        private bool CheckDetail(int index)
        {
            bool ret;
            string ymd = ScITEM.ChangeDate;

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.SKUName:
                case (int)EIndex.SKUShortName:
                case (int)EIndex.MakerItem:
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }

                    if (index.Equals((int)EIndex.SKUName) && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.SKUShortName].Text))
                    {
                        detailControls[(int)EIndex.SKUShortName].Text =bbl.LeftB( detailControls[index].Text,40);
                    }
                    break;

                case (int)EIndex.KanaName:
                    break;

                case (int)EIndex.BrandCD:
                    ScBrand.LabelText = "";
                    if (!string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //以下の条件でM_Brandが存在しない場合、エラー

                        //[M_Brand]
                        M_Brand_Entity mbe = new M_Brand_Entity
                        {
                            BrandCD = detailControls[index].Text
                        };
                        Brand_BL bbl = new Brand_BL();
                        ret = bbl.M_Brand_Select(mbe);
                        if (ret)
                        {
                            ScBrand.LabelText = mbe.BrandName;
                        }
                        else
                        {
                            //Ｅ１０１
                            base.bbl.ShowMessage("E101");
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.TaniCD:
                case (int)EIndex.SportsCD:
                case (int)EIndex.SegmentCD:
                case (int)EIndex.ExhibitionSegmentCD:
                    // 入力無くても良い(It is not necessary to input)
                    ((Search.CKM_SearchControl)detailControls[index].Parent).LabelText = "";

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //以下の条件でM_MultiPorposeが存在しない場合、エラー
                    string id = "";
                        if (index.Equals((int)EIndex.TaniCD))
                            id = MultiPorpose_BL.ID_TANI;
                    else if (index.Equals((int)EIndex.SportsCD))
                        id = MultiPorpose_BL.ID_SPORTS;
                    else if(index.Equals((int)EIndex.SegmentCD))
                        id = MultiPorpose_BL.ID_SegmentCD;
                    else if (index.Equals((int)EIndex.ExhibitionSegmentCD))
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
                    // 入力無くても良い(It is not necessary to input)
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
                        SoukoCD = SoukoCD,
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
                case (int)EIndex.ApprovalDate:
                    //入力無くても良い(It is not necessary to input)
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

                case (int)EIndex.ColorNO:
                case (int)EIndex.SizeNO:
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    //変更モードの場合、元の値（Tableから読み込んだ値）未満には変更できない（増加のみOK）
                    if(OperationMode == EOperationMode.UPDATE)
                    {
                        if(index == (int)EIndex.ColorNO)
                        {
                            if(dgvDetail.Rows.Count > bbl.Z_Set( detailControls[index].Text)+1)
                            {
                                //Ｅ２１９
                                bbl.ShowMessage("E219");
                                return false;
                            }
                        }
                        else
                        {
                            if (dgvDetail.Columns.Count > bbl.Z_Set(detailControls[index].Text)+2)
                            {
                                //Ｅ２１９
                                bbl.ShowMessage("E219");
                                return false;
                            }
                        }
                    }

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
                        if(cmbTaxRateFLG.SelectedValue != null)
                        {
                            taxRate = Convert.ToInt16(cmbTaxRateFLG.SelectedValue);
                        }

                        detailControls[(int)EIndex.PriceWithTax].Text = bbl.Z_SetStr(bbl.GetZeikomiKingaku(mOldPriceOutTax, taxRate, out zei, out zeiritsu));
                        //税抜発注額＝税抜定価×（画面.掛率÷100）
                        detailControls[(int)EIndex.OrderPriceWithoutTax].Text = bbl.Z_SetStr(GetResultWithHasuKbn( mFractionKBN, mOldPriceOutTax * bbl.Z_Set(detailControls[(int)EIndex.Rate].Text)/100));
                        //税込発注額＝税抜発注額×（１＋（税率÷100））								
                        detailControls[(int)EIndex.OrderPriceWithTax].Text = bbl.Z_SetStr(bbl.GetZeikomiKingaku(bbl.Z_Set(detailControls[(int)EIndex.OrderPriceWithoutTax].Text), taxRate,out zei));

                    }
                    break;
                case (int)EIndex.Rate: //掛率
                                       //画面上で金額変更された場合
                    if (mOldRate != bbl.Z_Set(detailControls[index].Text))
                    {
                        mOldRate = bbl.Z_Set(detailControls[index].Text);

                        decimal zei = 0;
                        int taxRate = 0;
                        if (cmbTaxRateFLG.SelectedValue != null)
                        {
                            taxRate = Convert.ToInt16(cmbTaxRateFLG.SelectedValue);
                        }

                        //税抜発注額＝税抜定価×（画面.掛率÷100）
                        detailControls[(int)EIndex.OrderPriceWithoutTax].Text = bbl.Z_SetStr(GetResultWithHasuKbn(mFractionKBN, bbl.Z_Set(detailControls[(int)EIndex.PriceOutTax].Text) * bbl.Z_Set(detailControls[(int)EIndex.Rate].Text) / 100));
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
                        if (cmbTaxRateFLG.SelectedValue != null)
                        {
                            taxRate = Convert.ToInt16(cmbTaxRateFLG.SelectedValue);
                        }
                        //税込発注額＝税抜発注額×（１＋（税率÷100））								
                        detailControls[(int)EIndex.OrderPriceWithTax].Text = bbl.Z_SetStr(bbl.GetZeikomiKingaku(bbl.Z_Set(detailControls[(int)EIndex.OrderPriceWithoutTax].Text), taxRate, out zei));
                    }
                    break;

                case (int)EIndex.CmbReserveCD:
                case (int)EIndex.CmbNoticesCD:
                case (int)EIndex.CmbPostageCD:
                case (int)EIndex.CmbManufactCD:
                case (int)EIndex.CmbConfirmCD:
                case (int)EIndex.CmbTaxRateFLG:
                case (int)EIndex.CmbCostingKBN:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        ((CKM_Controls.CKM_ComboBox)detailControls[index]).MoveNext = false;
                        return false;
                    }
                    break;
                case (int)EIndex.CmbTag1:
                case (int)EIndex.CmbTag2:
                case (int)EIndex.CmbTag3:
                case (int)EIndex.CmbTag4:
                case (int)EIndex.CmbTag5:
                case (int)EIndex.CmbTag6:
                case (int)EIndex.CmbTag7:
                case (int)EIndex.CmbTag8:
                case (int)EIndex.CmbTag9:
                case (int)EIndex.CmbTag10:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;                    

                    //重複OK
                    ChangeTag(index);
                    break;
            }

            return true;
        }

        private void ChangeTag(int index)
        {
            string name = "";
            string oldVal = "";
            string newVal = detailControls[index].Text;

            switch(index)
            {
                case (int)EIndex.CmbTag1:
                    name = "1";
                    oldVal = mie.TagName1;
                    mie.TagName1 = newVal;
                    break;
                case (int)EIndex.CmbTag2:
                    name = "2";
                    oldVal = mie.TagName2;
                    mie.TagName2 = newVal;
                    break;
                case (int)EIndex.CmbTag3:
                    name = "3";
                    oldVal = mie.TagName3;
                    mie.TagName3 = newVal;
                    break;
                case (int)EIndex.CmbTag4:
                    name = "4";
                    oldVal = mie.TagName4;
                    mie.TagName4 = newVal;
                    break;
                case (int)EIndex.CmbTag5:
                    name = "5";
                    oldVal = mie.TagName5;
                    mie.TagName5 = newVal;
                    break;
                case (int)EIndex.CmbTag6:
                    name = "6";
                    oldVal = mie.TagName6;
                    mie.TagName6 = newVal;
                    break;
                case (int)EIndex.CmbTag7:
                    name = "7";
                    oldVal = mie.TagName7;
                    mie.TagName7 = newVal;
                    break;
                case (int)EIndex.CmbTag8:
                    name = "8";
                    oldVal = mie.TagName8;
                    mie.TagName8 = newVal;
                    break;
                case (int)EIndex.CmbTag9:
                    name = "9";
                    oldVal = mie.TagName9;
                    mie.TagName9 = newVal;
                    break;
                case (int)EIndex.CmbTag10:
                    name = "10";
                    oldVal = mie.TagName10;
                    mie.TagName10 = newVal;
                    break;
            }

            //ITEM画面のタグをSKU画面のタグに反映
            DataRow[] rows = dtSKU.Select("TagName" + name + " = '" + oldVal + "'");
            foreach (DataRow row in rows)
            {
                row["TagName" + name] = newVal;
            }
        }
        private void ChangeItem()
        {
            if (OperationMode != EOperationMode.INSERT && OperationMode != EOperationMode.UPDATE)
                return;

            //ITEM画面の項目を変更した場合、SKU画面のその項目＝変更前のITEM画面の項目であれば、ITEM画面の内容をSKU画面に反映させる。
            for (int index = 0; index < detailControls.Length; index++)
            {
                string name = "";
                string oldVal = "";
                string newVal = detailControls[index].Text;
                
                switch (index)
                {
                    case (int)EIndex.ApprovalDate: name = "ApprovalDate"; oldVal = "'" + mie.ApprovalDate + "'"; break;
                    case (int)EIndex.ChkVariousFLG: name = "VariousFLG"; oldVal = mie.VariousFLG == null ? "0" : mie.VariousFLG; break;
                    case (int)EIndex.ChkSetKBN: name = "SetKBN"; oldVal = mie.SetKBN == null ? "0" : mie.SetKBN; break;
                    case (int)EIndex.ChkWebFlg: name = "WebFlg"; oldVal = mie.WebFlg == null ? "0" : mie.WebFlg; break;
                    case (int)EIndex.ChkRealStoreFlg: name = "RealStoreFlg"; oldVal = mie.RealStoreFlg == null ? "0" : mie.RealStoreFlg; break;
                    case (int)EIndex.SKUName: name = "SKUName"; oldVal = "'" + mie.ITemName + "'"; break;
                    case (int)EIndex.SKUShortName: name = "SKUShortName"; oldVal = "'" + mie.SKUShortName + "'"; break;
                    case (int)EIndex.KanaName: name = "KanaName"; oldVal = "'" + mie.KanaName + "'"; break;
                    case (int)EIndex.EnglishName: name = "EnglishName"; oldVal = "'" + mie.EnglishName + "'"; break;
                    case (int)EIndex.ChkVirtualFlg: name = "VirtualFlg"; oldVal = mie.VirtualFlg == null ? "0" : mie.VirtualFlg; break;
                    case (int)EIndex.ChkDiscontinueFlg: name = "DiscontinueFlg"; oldVal = mie.DiscontinueFlg == null ? "0" : mie.DiscontinueFlg; break;
                    case (int)EIndex.ChkStopFlg: name = "StopFlg"; oldVal = mie.StopFlg == null ? "0" : mie.StopFlg; break;
                    case (int)EIndex.ChkDiscountKBN: name = "DiscountKBN"; oldVal = mie.DiscountKBN == null ? "0" : mie.DiscountKBN; break;
                    case (int)EIndex.ChkZaikoKBN: name = "ZaikoKBN"; oldVal = mie.ZaikoKBN == null ? "0" : mie.ZaikoKBN; break;
                    case (int)EIndex.ChkPresentKBN: name = "PresentKBN"; oldVal = mie.PresentKBN == null ? "0" : mie.PresentKBN; break;
                    case (int)EIndex.ChkSampleKBN: name = "SampleKBN"; oldVal = mie.SampleKBN == null ? "0" : mie.SampleKBN; break;
                    case (int)EIndex.ChkCatalogFlg: name = "CatalogFlg"; oldVal = mie.CatalogFlg == null ? "0" : mie.CatalogFlg; break;
                    case (int)EIndex.ChkNoNetOrderFlg: name = "NoNetOrderFlg"; oldVal = mie.NoNetOrderFlg == null ? "0" : mie.NoNetOrderFlg; break;
                    case (int)EIndex.ChkEDIOrderFlg: name = "EDIOrderFlg"; oldVal = mie.EDIOrderFlg == null ? "0" : mie.EDIOrderFlg; break;
                    case (int)EIndex.ChkAutoOrderFlg: name = "AutoOrderFlg"; oldVal = mie.AutoOrderFlg == null ? "0" : mie.AutoOrderFlg; break;
                    case (int)EIndex.ChkDirectFlg: name = "DirectFlg"; oldVal = mie.DirectFlg == null ? "0" : mie.DirectFlg; break;
                    case (int)EIndex.ChkParcelFlg: name = "ParcelFlg"; oldVal = mie.ParcelFlg == null ? "0" : mie.ParcelFlg; break;
                    case (int)EIndex.ChkSoldOutFlg: name = "SoldOutFlg"; oldVal = mie.SoldOutFlg == null ? "0" : mie.SoldOutFlg; break;
                    case (int)EIndex.ChkSaleExcludedFlg: name = "SaleExcludedFlg"; oldVal = mie.SaleExcludedFlg == null ? "0" : mie.SaleExcludedFlg; break;
                    case (int)EIndex.ChkWebStockFlg: name = "WebStockFlg"; oldVal = mie.WebStockFlg == null ? "0" : mie.WebStockFlg; break;
                    case (int)EIndex.ChkInventoryAddFlg: name = "InventoryAddFlg"; oldVal = mie.InventoryAddFlg == null ? "0" : mie.InventoryAddFlg; break;
                    case (int)EIndex.ChkMakerAddFlg: name = "MakerAddFlg"; oldVal = mie.MakerAddFlg == null ? "0" : mie.MakerAddFlg; break;
                    case (int)EIndex.ChkStoreAddFlg: name = "StoreAddFlg"; oldVal = mie.StoreAddFlg == null ? "0" : mie.StoreAddFlg; break;
                    case (int)EIndex.MainVendorCD: name = "MainVendorCD"; oldVal = "'" + mie.MainVendorCD + "'"; break;
                    case (int)EIndex.Rack: name = "Rack"; oldVal = "'" + mie.Rack + "'"; break;
                    case (int)EIndex.SaleStartDate: name = "SaleStartDate"; oldVal = "'" + mie.SaleStartDate + "'"; break;
                    case (int)EIndex.WebStartDate: name = "WebStartDate"; oldVal = "'" + mie.WebStartDate + "'"; break;
                    case (int)EIndex.PriceOutTax: name = "PriceOutTax"; oldVal = mie.PriceOutTax == null ? "0" : mie.PriceOutTax; break;
                    case (int)EIndex.PriceWithTax: name = "PriceWithTax"; oldVal = mie.PriceWithTax == null ? "0" : mie.PriceWithTax; break;
                    case (int)EIndex.Rate: name = "Rate"; oldVal = mie.Rate == null ? "0" : mie.Rate; break;
                    case (int)EIndex.OrderPriceWithoutTax: name = "OrderPriceWithoutTax"; oldVal = mie.OrderPriceWithoutTax == null ? "0" : mie.OrderPriceWithoutTax; break;
                    case (int)EIndex.OrderPriceWithTax: name = "OrderPriceWithTax"; oldVal = mie.OrderPriceWithTax == null ? "0" : mie.OrderPriceWithTax; break;
                    case (int)EIndex.CmbOrderAttentionCD: name = "OrderAttentionCD"; oldVal = "'" + mie.OrderAttentionCD + "'"; break;
                    case (int)EIndex.OrderAttentionNote: name = "OrderAttentionNote"; oldVal = "'" + mie.OrderAttentionNote + "'"; break;
                    case (int)EIndex.CmbLastYearTerm: name = "LastYearTerm"; oldVal = "'" + mie.LastYearTerm + "'"; break;
                    case (int)EIndex.CmbLastSeason: name = "LastSeason"; oldVal = "'" + mie.LastSeason + "'"; break;
                    case (int)EIndex.LastCatalogNO: name = "LastCatalogNO"; oldVal = "'" + mie.LastCatalogNO + "'"; break;
                    case (int)EIndex.LastCatalogPage: name = "LastCatalogPage"; oldVal = "'" + mie.LastCatalogPage + "'"; break;
                    case (int)EIndex.LastCatalogText: name = "LastCatalogText"; oldVal = "'" + mie.LastCatalogText + "'"; break;
                    case (int)EIndex.LastInstructionsNO: name = "LastInstructionsNO"; oldVal = "'" + mie.LastInstructionsNO + "'"; break;
                    case (int)EIndex.LastInstructionsDate: name = "LastInstructionsDate"; oldVal = "'" + mie.LastInstructionsDate + "'"; break;
                    case (int)EIndex.CommentInStore: name = "CommentInStore"; oldVal = "'" + mie.CommentInStore + "'"; break;
                    case (int)EIndex.CommentOutStore: name = "CommentOutStore"; oldVal = "'" + mie.CommentOutStore + "'"; break;
                    case (int)EIndex.ExhibitionSegmentCD: name = "ExhibitionSegmentCD"; oldVal = "'" + mie.ExhibitionSegmentCD + "'"; break;
                    case (int)EIndex.OrderLot: name = "OrderLot"; oldVal = mie.OrderLot == null ? "0" : mie.OrderLot; break;
                    case (int)EIndex.WebAddress: name = "WebAddress"; oldVal = "'" + mie.WebAddress + "'"; break;
                    case (int)EIndex.DeleteFlg: name = "DeleteFlg"; oldVal = mie.DeleteFlg == null ? "0" : mie.DeleteFlg; break;
                    case (int)EIndex.MakerItem:
                        name = "MakerItem"; break;
                    case (int)EIndex.TaniCD:
                        name = "TaniCD"; break;
                    case (int)EIndex.BrandCD:
                        name = "BrandCD"; break;
                    case (int)EIndex.SegmentCD:
                        name = "SegmentCD"; break;
                    case (int)EIndex.SportsCD:
                        name = "SportsCD"; break;
                    case (int)EIndex.CmbReserveCD:
                        name = "ReserveCD"; break;
                    case (int)EIndex.CmbNoticesCD:
                        name = "NoticesCD"; break;
                    case (int)EIndex.CmbPostageCD:
                        name = "PostageCD"; break;
                    case (int)EIndex.CmbManufactCD:
                        name = "ManufactCD"; break;
                    case (int)EIndex.CmbConfirmCD:
                        name = "ConfirmCD"; break;
                    case (int)EIndex.CmbTaxRateFLG:
                        name = "TaxRateFLG"; break;
                    case (int)EIndex.CmbCostingKBN:
                        name = "CostingKBN"; break;

                    default:
                        continue;
                }


                switch (index)
                {
                    case (int)EIndex.ChkVariousFLG:
                    case (int)EIndex.ChkSetKBN:
                    case (int)EIndex.ChkWebFlg:
                    case (int)EIndex.ChkRealStoreFlg:
                    case (int)EIndex.ChkVirtualFlg:
                    case (int)EIndex.ChkDiscontinueFlg:
                    case (int)EIndex.ChkStopFlg:
                    case (int)EIndex.ChkDiscountKBN:
                    case (int)EIndex.ChkZaikoKBN:
                    case (int)EIndex.ChkPresentKBN:
                    case (int)EIndex.ChkSampleKBN:
                    case (int)EIndex.ChkCatalogFlg:
                    case (int)EIndex.ChkNoNetOrderFlg:
                    case (int)EIndex.ChkEDIOrderFlg:
                    case (int)EIndex.ChkAutoOrderFlg:
                    case (int)EIndex.ChkDirectFlg:
                    case (int)EIndex.ChkParcelFlg:
                    case (int)EIndex.ChkSoldOutFlg:
                    case (int)EIndex.ChkSaleExcludedFlg:
                    case (int)EIndex.ChkWebStockFlg:
                    case (int)EIndex.ChkInventoryAddFlg:
                    case (int)EIndex.ChkMakerAddFlg:
                    case (int)EIndex.ChkStoreAddFlg:
                    case (int)EIndex.DeleteFlg:
                        newVal = ((CheckBox)detailControls[index]).Checked ? "1" : "0";
                        break;

                    case (int)EIndex.PriceOutTax:
                    case (int)EIndex.PriceWithTax:
                    case (int)EIndex.Rate:
                    case (int)EIndex.OrderPriceWithoutTax:
                    case (int)EIndex.OrderPriceWithTax:
                        oldVal = oldVal.Replace(",", "");
                        newVal= bbl.Z_SetStr(newVal).Replace(",", "");
                        break;
                    case (int)EIndex.CmbOrderAttentionCD:
                    case (int)EIndex.CmbReserveCD:
                    case (int)EIndex.CmbNoticesCD:
                    case (int)EIndex.CmbPostageCD:
                    case (int)EIndex.CmbManufactCD:
                    case (int)EIndex.CmbConfirmCD:
                    case (int)EIndex.CmbTaxRateFLG:
                    case (int)EIndex.CmbCostingKBN:
                    case (int)EIndex.CmbLastYearTerm:
                    case (int)EIndex.CmbLastSeason:
                        if (((ComboBox)detailControls[index]).SelectedIndex > 0)
                            newVal = ((ComboBox)detailControls[index]).SelectedValue.ToString();
                        else
                            newVal = "";
                        break;
                }
                switch (index)
                {
                    case (int)EIndex.MakerItem:
                    case (int)EIndex.TaniCD:
                    case (int)EIndex.BrandCD:
                    case (int)EIndex.SegmentCD:
                    case (int)EIndex.SportsCD:
                    case (int)EIndex.CmbReserveCD:
                    case (int)EIndex.CmbNoticesCD:
                    case (int)EIndex.CmbPostageCD:
                    case (int)EIndex.CmbManufactCD:
                    case (int)EIndex.CmbConfirmCD:
                    case (int)EIndex.CmbTaxRateFLG:
                    case (int)EIndex.CmbCostingKBN:
                        ChangeAll(name, newVal);
                        continue;
                }

                DataRow[] rows = dtSKU.Select(name + " = " + oldVal );
                foreach (DataRow row in rows)
                {
                    row[name] = newVal;
                }


            }
        }
        private void  ChangeAll(string name, string newVal)
        {
            foreach (DataRow row in dtSKU.Rows)
            {
                row[name] = newVal;
            }
        }

        private bool IsSameVal(object val1, object val2)
        {
            bool ret = false;

            if (val1 == null || val2 == null)
                return ret;

            if (val1.ToString().Trim() == val2.ToString().Trim())
                ret = true;

            return ret;
        }
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <param name="kbn"></param>
        /// <returns></returns>
        private M_ITEM_Entity GetEntity(short kbn, short key = 0)
        {
            mie = new M_ITEM_Entity();

            if (kbn == 0)
            {
                mie.ITemCD = keyControls[(int)EIndex.ItemCD].Text;
                mie.ChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
                if (key.Equals(1))
                    return mie;
            }
            else
            {
                mie.ITemCD = copyKeyControls[(int)EIndex.ItemCD].Text;
                mie.ChangeDate = copyKeyControls[(int)EIndex.ChangeDate].Text;
                return mie;
            }

            mie.VariousFLG = ((CheckBox)detailControls[(int)EIndex.ChkVariousFLG]).Checked ? "1" : "0";
            mie.ITemName = detailControls[(int)EIndex.SKUName].Text;
            mie.KanaName = detailControls[(int)EIndex.KanaName].Text;
            mie.SKUShortName = detailControls[(int)EIndex.SKUShortName].Text;
            mie.EnglishName = detailControls[(int)EIndex.EnglishName].Text;

            mie.ColorNO = detailControls[(int)EIndex.ColorNO].Text;
            mie.SizeNO = detailControls[(int)EIndex.SizeNO].Text;
            mie.SetKBN = ((CheckBox)detailControls[(int)EIndex.ChkSetKBN]).Checked ? "1" : "0";
            mie.PresentKBN = ((CheckBox)detailControls[(int)EIndex.ChkPresentKBN]).Checked ? "1" : "0";
            mie.SampleKBN = ((CheckBox)detailControls[(int)EIndex.ChkSampleKBN]).Checked ? "1" : "0";
            mie.DiscountKBN = ((CheckBox)detailControls[(int)EIndex.ChkDiscountKBN]).Checked ? "1" : "0";
            //カラーの入力がひとつの場合に一番上の値
            string ColorName = "";
            if(dgvDetail.Rows.Count.Equals(2) && dgvDetail.Rows[1].Cells[1].Value != null)
            {
                ColorName = dgvDetail.Rows[1].Cells[1].Value.ToString();
            }
            //サイズの入力がひとつの場合に一番左の値
            string SizeName = "";
            if (dgvDetail.Columns.Count.Equals(3) && dgvDetail.Rows[0].Cells[2].Value != null)
            {
                SizeName = dgvDetail.Rows[0].Cells[2].Value.ToString();
            }
            mie.ColorName = ColorName;
            mie.SizeName = SizeName;
     

            mie.WebFlg = ((CheckBox)detailControls[(int)EIndex.ChkWebFlg]).Checked ? "1" : "0";
            mie.RealStoreFlg = ((CheckBox)detailControls[(int)EIndex.ChkRealStoreFlg]).Checked ? "1" : "0";
            mie.MainVendorCD = detailControls[(int)EIndex.MainVendorCD].Text;
            //mie.MakerVendorCD = detailControls[(int)EIndex.MakerVendorCD].Text;
            mie.BrandCD = detailControls[(int)EIndex.BrandCD].Text;
            mie.BrandName = ScBrand.LabelText;
            mie.MakerItem = detailControls[(int)EIndex.MakerItem].Text;
            mie.TaniCD = detailControls[(int)EIndex.TaniCD].Text;
            mie.TaniName= ScTani.LabelText;
            mie.SportsCD = detailControls[(int)EIndex.SportsCD].Text;
            mie.SportsName = ScSports.LabelText;
            mie.SegmentCD= detailControls[(int)EIndex.SegmentCD].Text;
            mie.SegmentName = ScSegmentCD.LabelText;
            mie.ZaikoKBN = ((CheckBox)detailControls[(int)EIndex.ChkZaikoKBN]).Checked ? "1" : "0";
            mie.Rack = detailControls[(int)EIndex.Rack].Text;
            mie.VirtualFlg = ((CheckBox)detailControls[(int)EIndex.ChkVirtualFlg]).Checked ? "1" : "0";
            mie.DirectFlg = ((CheckBox)detailControls[(int)EIndex.ChkDirectFlg]).Checked ? "1" : "0";
            if (((ComboBox)detailControls[(int)EIndex.CmbReserveCD]).SelectedValue != null)
                mie.ReserveCD = ((ComboBox)detailControls[(int)EIndex.CmbReserveCD]).SelectedValue.ToString();
            else
                mie.ReserveCD = "";

            mie.ReserveName = detailControls[(int)EIndex.CmbReserveCD].Text;
            if (((ComboBox)detailControls[(int)EIndex.CmbNoticesCD]).SelectedValue != null)
                mie.NoticesCD = ((ComboBox)detailControls[(int)EIndex.CmbNoticesCD]).SelectedValue.ToString();
            else
                mie.NoticesCD = "";

            mie.NoticesName = detailControls[(int)EIndex.CmbNoticesCD].Text;
            if (((ComboBox)detailControls[(int)EIndex.CmbPostageCD]).SelectedValue != null)
                mie.PostageCD = ((ComboBox)detailControls[(int)EIndex.CmbPostageCD]).SelectedValue.ToString();
            else
                mie.PostageCD = "";

            mie.PostageName = detailControls[(int)EIndex.CmbPostageCD].Text;
            if (((ComboBox)detailControls[(int)EIndex.CmbManufactCD]).SelectedValue != null)
                mie.ManufactCD = ((ComboBox)detailControls[(int)EIndex.CmbManufactCD]).SelectedValue.ToString();
            else
                mie.ManufactCD = "";

            mie.ManufactName = detailControls[(int)EIndex.CmbManufactCD].Text;
            if (((ComboBox)detailControls[(int)EIndex.CmbConfirmCD]).SelectedValue != null)
                mie.ConfirmCD = ((ComboBox)detailControls[(int)EIndex.CmbConfirmCD]).SelectedValue.ToString();
            else
                mie.ConfirmCD = "";

            mie.ConfirmName = detailControls[(int)EIndex.CmbConfirmCD].Text;
            mie.WebStockFlg = ((CheckBox)detailControls[(int)EIndex.ChkWebStockFlg]).Checked ? "1" : "0";
            mie.StopFlg = ((CheckBox)detailControls[(int)EIndex.ChkStopFlg]).Checked ? "1" : "0";
            mie.DiscontinueFlg = ((CheckBox)detailControls[(int)EIndex.ChkDiscontinueFlg]).Checked ? "1" : "0";
            mie.InventoryAddFlg = ((CheckBox)detailControls[(int)EIndex.ChkInventoryAddFlg]).Checked ? "1" : "0";
            mie.MakerAddFlg = ((CheckBox)detailControls[(int)EIndex.ChkMakerAddFlg]).Checked ? "1" : "0";
            mie.StoreAddFlg = ((CheckBox)detailControls[(int)EIndex.ChkStoreAddFlg]).Checked ? "1" : "0";
            mie.NoNetOrderFlg = ((CheckBox)detailControls[(int)EIndex.ChkNoNetOrderFlg]).Checked ? "1" : "0";
            mie.EDIOrderFlg = ((CheckBox)detailControls[(int)EIndex.ChkEDIOrderFlg]).Checked ? "1" : "0";
            mie.CatalogFlg = ((CheckBox)detailControls[(int)EIndex.ChkCatalogFlg]).Checked ? "1" : "0";
            mie.ParcelFlg = ((CheckBox)detailControls[(int)EIndex.ChkParcelFlg]).Checked ? "1" : "0";
            mie.SoldOutFlg = ((CheckBox)detailControls[(int)EIndex.ChkSoldOutFlg]).Checked ? "1" : "0";
            mie.AutoOrderFlg = ((CheckBox)detailControls[(int)EIndex.ChkAutoOrderFlg]).Checked ? "1" : "0";
            mie.TaxRateFLG = ((ComboBox)detailControls[(int)EIndex.CmbTaxRateFLG]).SelectedValue.ToString();
            mie.TaxRateFLGName = detailControls[(int)EIndex.CmbTaxRateFLG].Text;
            mie.CostingKBN = ((ComboBox)detailControls[(int)EIndex.CmbCostingKBN]).SelectedValue.ToString();
            mie.CostingKBNName = detailControls[(int)EIndex.CmbCostingKBN].Text;
            mie.SaleExcludedFlg = ((CheckBox)detailControls[(int)EIndex.ChkSaleExcludedFlg]).Checked ? "1" : "0";
            mie.PriceWithTax = bbl.Z_SetStr( detailControls[(int)EIndex.PriceWithTax].Text);
            mie.PriceOutTax = bbl.Z_SetStr(detailControls[(int)EIndex.PriceOutTax].Text);
            mie.OrderPriceWithTax = bbl.Z_SetStr(detailControls[(int)EIndex.OrderPriceWithTax].Text);
            mie.OrderPriceWithoutTax = bbl.Z_SetStr(detailControls[(int)EIndex.OrderPriceWithoutTax].Text);
            mie.Rate = bbl.Z_SetStr(detailControls[(int)EIndex.Rate].Text);
            mie.SaleStartDate = detailControls[(int)EIndex.SaleStartDate].Text;
            mie.WebStartDate = detailControls[(int)EIndex.WebStartDate].Text;
            mie.OrderAttentionCD = cmbOrderAttentionCD.SelectedIndex > 0 ? cmbOrderAttentionCD.SelectedValue.ToString():"";
            mie.OrderAttentionNote = detailControls[(int)EIndex.OrderAttentionNote].Text;
            mie.CommentInStore = detailControls[(int)EIndex.CommentInStore].Text;
            mie.CommentOutStore = detailControls[(int)EIndex.CommentOutStore].Text;
            mie.ExhibitionSegmentCD = detailControls[(int)EIndex.ExhibitionSegmentCD].Text;
            mie.OrderLot = bbl.Z_SetStr(detailControls[(int)EIndex.OrderLot].Text);
            mie.LastYearTerm = CmbLastYearTerm.SelectedIndex > 0 ? CmbLastYearTerm.SelectedValue.ToString() : ""; 
            mie.LastSeason = CmbLastSeason.SelectedIndex > 0 ? CmbLastSeason.SelectedValue.ToString() : ""; 
            mie.LastCatalogNO = detailControls[(int)EIndex.LastCatalogNO].Text;
            mie.LastCatalogPage = detailControls[(int)EIndex.LastCatalogPage].Text;
            mie.LastCatalogText = detailControls[(int)EIndex.LastCatalogText].Text;
            mie.LastInstructionsNO = detailControls[(int)EIndex.LastInstructionsNO].Text;
            mie.LastInstructionsDate = detailControls[(int)EIndex.LastInstructionsDate].Text;
            mie.WebAddress = detailControls[(int)EIndex.WebAddress].Text;
            mie.ApprovalDate = detailControls[(int)EIndex.ApprovalDate].Text;

            //チェックボックス
            mie.DeleteFlg = checkDeleteFlg.Checked ? "1" : "0";
            mie.UsedFlg = "0";
            mie.InsertOperator = InOperatorCD;
            mie.UpdateOperator = InOperatorCD;
            mie.PC = InPcID;

            mie.TagName1 = detailControls[(int)EIndex.CmbTag1].Text;
            mie.TagName2 = detailControls[(int)EIndex.CmbTag2].Text;
            mie.TagName3 = detailControls[(int)EIndex.CmbTag3].Text;
            mie.TagName4 = detailControls[(int)EIndex.CmbTag4].Text;
            mie.TagName5 = detailControls[(int)EIndex.CmbTag5].Text;
            mie.TagName6 = detailControls[(int)EIndex.CmbTag6].Text;
            mie.TagName7 = detailControls[(int)EIndex.CmbTag7].Text;
            mie.TagName8 = detailControls[(int)EIndex.CmbTag8].Text;
            mie.TagName9 = detailControls[(int)EIndex.CmbTag9].Text;
            mie.TagName10 = detailControls[(int)EIndex.CmbTag10].Text;

            return mie;
        }

        protected override void ExecSec()
        {
            for (int i = 0; i <= (int)EIndex.ChangeDate; i++)
                if (keyControls[i].Enabled)
                {
                    if (CheckKey(i, 0, false) == false)
                    {
                        keyControls[i].Focus();
                        return;
                    }
                }
            if (OperationMode != EOperationMode.DELETE)
            {
                for (int i = 0; i < detailControls.Length; i++)
                    if (CheckDetail(i) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }
            }

            //画面上に重複するJANCDが存在する
            //重複チェック
            M_SKU_Entity me = new M_SKU_Entity();
            me.ITemCD = keyControls[(int)EIndex.ItemCD].Text;
            me.ChangeDate = keyControls[(int)EIndex.ChangeDate].Text;

            dgvDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);

            for (int i = 0; i < dgvDetail.Rows.Count; i++)
            {
                for (int ic = 1; ic < dgvDetail.ColumnCount; ic++)
                {
                    if (i.Equals(0))
                    {
                        if (ic.Equals(1))
                            continue;

                        //サイズ名必須
                        if (dgvDetail.Rows[0].Cells[ic].Value == null)
                        {
                            bbl.ShowMessage("E102");
                            dgvDetail.CurrentCell = dgvDetail[ic, 0];
                            dgvDetail.Focus();
                            return;
                        }
                        else
                        {
                            //サイズ名をセット
                            DataRow[] row = dtSKU.Select("SizeNO = " + dgvDetail.Columns[ic].HeaderText);
                            foreach (DataRow rw in row)
                            {
                                //サイズ名をセット
                                rw["SizeName"] = dgvDetail.Rows[0].Cells[ic].Value.ToString().Trim();
                            }
                        }
                    }
                    else
                    {
                        if (ic.Equals(1))
                        {
                            //カラー名必須
                            if (dgvDetail.Rows[i].Cells[1].Value == null)
                            {
                                bbl.ShowMessage("E102");
                                dgvDetail.CurrentCell = dgvDetail[1, i];
                                dgvDetail.Focus();
                                return;
                            }
                            else
                            {
                                DataRow[] row = dtSKU.Select("ColorNo = " + dgvDetail.Rows[i].Cells[0].Value.ToString());
                                foreach (DataRow rw in row)
                                {
                                    //カラー名をセット
                                    rw["ColorName"] = dgvDetail.Rows[i].Cells[1].Value.ToString().Trim();
                                }                              
                            }
                        }
                        else
                        {
                            int rowIndex = i;
                            int columnIndex = ic;

                            if (dgvDetail.Rows[rowIndex].Cells[columnIndex].Value == null)
                                continue;

                            string jancd = dgvDetail.Rows[rowIndex].Cells[columnIndex].Value.ToString().Trim();

                            // parSKUCD + parSizeNo + parColorNo
                            DataRow[] rows = dtSKU.Select("SizeNo = " + dgvDetail.Columns[columnIndex].HeaderText
                                        + " AND ColorNo = " + dgvDetail.Rows[rowIndex].Cells[0].Value.ToString());
                            foreach(DataRow dr in rows)
                            {
                                dr["SKUCD"] = keyControls[(int)EIndex.ItemCD].Text + dgvDetail.Columns[columnIndex].HeaderText + dgvDetail.Rows[rowIndex].Cells[0].Value.ToString();
                            }

                            if(rows.Length.Equals(0))
                            {
                                //セット品の場合は展開が必須
                                if (!((CheckBox)detailControls[(int)EIndex.ChkSetKBN]).Checked)
                                {

                                    //新規データ
                                    DataRow newrow = dtSKU.NewRow();

                                    //SKU画面のChkAll参照
                                    //データをDataTableに
                                    newrow["ITemCD"] = mie.ITemCD;
                                    newrow["SKUCD"] = keyControls[(int)EIndex.ItemCD].Text + dgvDetail.Columns[columnIndex].HeaderText + dgvDetail.Rows[rowIndex].Cells[0].Value.ToString();
                                    newrow["ChangeDate"] = mie.ChangeDate;
                                    newrow["ColorNO"] = dgvDetail.Rows[rowIndex].Cells[0].Value.ToString();
                                    newrow["SizeNO"] = dgvDetail.Columns[columnIndex].HeaderText;
                                    newrow["ColorName"] = dgvDetail.Rows[rowIndex].Cells[1].Value.ToString().Trim();
                                    newrow["SizeName"] = dgvDetail.Rows[0].Cells[columnIndex].Value.ToString().Trim();

                                    newrow["JanCD"] = jancd;
                                    //newrow["SetAdminCD"] = keyControls[(int)EIndex.SetAdminCD].Text;
                                    //newrow["SetItemCD"] = keyControls[(int)EIndex.SetItemCD].Text;
                                    newrow["SetSKUCD"] = "";
                                    newrow["SetSU"] = 0;
                                    newrow["ApprovalDate"] = detailControls[(int)EIndex.ApprovalDate].Text;

                                    newrow["SKUName"] = detailControls[(int)EIndex.SKUName].Text;
                                    newrow["KanaName"] = detailControls[(int)EIndex.KanaName].Text;
                                    newrow["SKUShortName"] = detailControls[(int)EIndex.SKUShortName].Text;
                                    newrow["EnglishName"] = detailControls[(int)EIndex.EnglishName].Text;
                                    newrow["MakerItem"] = detailControls[(int)EIndex.MakerItem].Text;

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
                                    newrow["OrderLot"] = detailControls[(int)EIndex.OrderLot].Text;
                                    newrow["LastYearTerm"] = detailControls[(int)EIndex.CmbLastYearTerm].Text;
                                    newrow["LastSeason"] = detailControls[(int)EIndex.CmbLastSeason].Text;
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

                                    //if (row.Length == 0)
                                    //{
                                    dtSKU.Rows.Add(newrow);
                                    //}
                                }
                            }

                            me.JanCD = jancd;
                            bool noCheckFlg = false;

                            for (int row = rowIndex + 1; row < dgvDetail.Rows.Count; row++)
                            {
                                for (int col = columnIndex + 1; col < dgvDetail.ColumnCount; col++)
                                {
                                    if (IsSameVal(dgvDetail.Rows[row].Cells[col].Value, jancd))
                                    {
                                        if (bbl.ShowMessage("Q316") != DialogResult.Yes)
                                        {
                                            dgvDetail.CurrentCell = dgvDetail[col, row];
                                            dgvDetail.Focus();
                                            return;
                                        }
                                        else
                                        {
                                            noCheckFlg = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (!noCheckFlg)
                            {
                                //データ上(M_SKU)に重複するJANCDが存在するかどうか
                                bool ret = mibl.M_SKU_SelectByJANCD(me);
                                if (ret)
                                {
                                    if (bbl.ShowMessage("Q316") != DialogResult.Yes)
                                    {
                                        dgvDetail.CurrentCell = dgvDetail[columnIndex, rowIndex];
                                        dgvDetail.Focus();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ChangeItem();

            //更新処理
            mie = GetEntity(0);

            DataTable dt = mibl.GetGridEntity(dtSKU);
            if (dt.Rows.Count == 0)
            {
                //更新対象なし
                bbl.ShowMessage("E189");
                dgvDetail.Focus();
                return;
            }

            DataTable dtS = mibl.GetSiteEntity(dtSite);

            mibl.M_ITEM_Exec(mie,dt,dtS, (short)OperationMode);

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
                case EsearchKbn.JANCD:
                    int col = dgvDetail.CurrentCell.ColumnIndex;
                    int row = dgvDetail.CurrentCell.RowIndex;

                    if (col <= 1 || row <= 0)
                        return;

                    string changeDate = keyControls[(int)EIndex.ChangeDate].Text;

                    using (Search_Product frm = new Search_Product(changeDate))
                    {
                        frm.Mode = "4";
                        frm.ShowDialog();

                        if (!frm.flgCancel)
                        {
                            dgvDetail.Rows[row].Cells[col].Value = frm.JANCD;
                        }
                    }
                    break;

                //case EsearchKbn.RackNO:
                //    //棚番が複数存在する場合 棚番検索画面を起動
                //    D_Stock_Entity dse = new D_Stock_Entity
                //    {
                //        SoukoCD = CboFromSoukoCD.SelectedValue.ToString(),
                //        SoukoName = CboFromSoukoCD.Text,
                //        SKUCD = lblSKUCD.Text,
                //        SKUName = lblSKUName.Text,
                //        AdminNO = mAdminNO
                //    };
                //    Select_RackNO frm = new Select_RackNO(dse);
                //    frm.ShowDialog();

                //    if (!frm.flgCancel)
                //    {
                //        detailControls[(int)EIndex.Rack].Text = frm.parRackNO;
                //    }
                //    break;
            }

        }

        /// <summary>
        /// 画面の初期化
        /// </summary>
        private void InitScr()
        {
            ChangeOperationMode(base.OperationMode);
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

            dgvDetail.Rows.Clear();
            dgvDetail.Columns.Clear();

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
                                ScITEM.SearchEnable = Kbn == 0 ? true : false;
                            else
                            {
                                ScITEM.SearchEnable = false;
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
                            ScCopyITEM.SearchEnable = Kbn == 0 ? true : false;
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
                        {
                            PreviousCtrl.Focus();
                            return;
                        }
                        InitScr();

                        break;
                    }

                    case 8: //F9:検索
                    EsearchKbn kbn = EsearchKbn.Null;
                    if (PreviousCtrl == dgvDetail)
                    {
                        //JANCD検索
                        SearchData(EsearchKbn.JANCD, PreviousCtrl);
                    }
                    //else if (PreviousCtrl.Name.Equals(ScRackNo.Name))
                    //{
                    //    //棚検索
                    //    SearchData(EsearchKbn.RackNO, previousCtrl);
                    //}

                    if (kbn != EsearchKbn.Null)
                        SearchData(kbn, PreviousCtrl);

                    break;
                case 9://F10:展開
                    InitGrid();
                    //if (dgvDetail.CurrentCell == null)
                    //    return;

                    //int ColumnIndex = dgvDetail.CurrentCell.ColumnIndex;
                    //int RowIndex = dgvDetail.CurrentCell.RowIndex;
                    //Expand(ColumnIndex, RowIndex);
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
                            case (int)EIndex.ItemCD:
                                keyControls[(int)EIndex.ChangeDate].Focus();
                                break;

                            case (int)EIndex.ChangeDate:
                                if (OperationMode == EOperationMode.INSERT)
                                {
                                    copyKeyControls[(int)EIndex.ItemCD].Focus();
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
                            case (int)EIndex.ItemCD:
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
                        if(index.Equals((int)EIndex.SizeNO))
                        {
                            InitGrid();
                        }

                        if((index>=(int)EIndex.LastInstructionsNO && index <= (int)EIndex.WebAddress) || index.Equals((int)EIndex.LastCatalogText))
                        {
                            //MultiLineのフォーカス移動はMovenextプロパティにて処理する
                        }
                        else if (detailControls.Length - 1 > index)
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

        private void BtnSubF10_Click(object sender, EventArgs e)
        {
            //展開ボタンClick時   
            try
            {
                InitGrid();

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
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;

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

                if (kbn != EsearchKbn.Null)
                    SearchData(kbn, setCtl);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
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

        private void ScItem_Leave(object sender, EventArgs e)
        {
            ScBrand.ChangeDate = ScITEM.ChangeDate;
            ScSegmentCD.ChangeDate = ScITEM.ChangeDate;
            ScExhibitionSegmentCD.ChangeDate = ScITEM.ChangeDate;
            ScSports.ChangeDate = ScITEM.ChangeDate;
            ScVendor.ChangeDate = ScITEM.ChangeDate;
            ScRackNo.ChangeDate = ScITEM.ChangeDate;
        }
        private void dgvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Expand(e.ColumnIndex, e.RowIndex);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                int columnIndex = e.ColumnIndex;
                int rowIndex = e.RowIndex;

                if (rowIndex.Equals(0) && columnIndex > 1)
                {
                    //サイズ名
                    //必須入力
                    if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                    {
                        dgvDetail.Rows[e.RowIndex].ErrorText =
                            "Size Name must not be empty";
                        bbl.ShowMessage("E102");
                        e.Cancel = true;
                        return;
                    }
                    //20Bまで
                    string str = Encoding.GetEncoding(932).GetByteCount(e.FormattedValue.ToString()).ToString();
                    if (Convert.ToInt32(str) > 20)
                    {
                        MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                        return;
                    }

                    //同じ値のセルが複数あればエラー Ｅ１０５
                    for (int i = 2; i<dgvDetail.Columns.Count; i++)
                    {
                        if(i != columnIndex && IsSameVal(dgvDetail.Rows[0].Cells[i].Value, e.FormattedValue))
                        {
                            bbl.ShowMessage("E105");
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                else if (columnIndex.Equals(1) && rowIndex>0)
                {
                    //カラー名
                    //必須入力
                    if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                    {
                        dgvDetail.Rows[e.RowIndex].ErrorText =
                            "Color Name must not be empty";
                        bbl.ShowMessage("E102");
                        e.Cancel = true;
                        return;
                    }
                    //20Bまで
                    string str = Encoding.GetEncoding(932).GetByteCount(e.FormattedValue.ToString()).ToString();
                    if (Convert.ToInt32(str) > 20)
                    {
                        MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                        return;
                    }

                    //同じ値のセルが複数あればエラー Ｅ１０５
                    for (int i = 1; i < dgvDetail.Rows.Count; i++)
                    {
                        if (i != rowIndex && IsSameVal(dgvDetail.Rows[i].Cells[1].Value, e.FormattedValue))
                        {
                            bbl.ShowMessage("E105");
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                else if (rowIndex > 0 && columnIndex > 1)
                {
                    // JANCD
                    //入力なしＯＫ
                    //重複OK
                    dgvDetail.Rows[rowIndex].Cells[columnIndex].Style.BackColor = rowIndex % 2 != 0 ? Color.FromArgb(221, 235, 247) : Color.White;

                    string jancd = e.FormattedValue.ToString().Trim();

                    //13桁の数字であること Ｅ２２０
                    if (!string.IsNullOrWhiteSpace(jancd))
                    {
                        if (!IsNumeric(jancd) || jancd.Length != 13)
                        {
                            bbl.ShowMessage("E220");
                            e.Cancel = true;
                            return;
                        }
                    }

                    //VirtualFlg
                    DataRow[] rows = dtSKU.Select("SizeNo = " + dgvDetail.Columns[columnIndex].HeaderText 
                                        + " AND ColorNo = " + dgvDetail.Rows[rowIndex].Cells[0].Value.ToString());
                    if(rows.Length > 0 )
                    {
                        rows[0]["JANCD"] = jancd;
                        rows[0]["ColorName"] = dgvDetail.Rows[rowIndex].Cells[1].Value.ToString().Trim();
                        rows[0]["SizeName"] = dgvDetail.Rows[0].Cells[columnIndex].Value.ToString().Trim();

                        if (rows[0]["VirtualFlg"].ToString().Equals("1"))
                            dgvDetail.Rows[rowIndex].Cells[columnIndex].Style.BackColor = Color.HotPink;
                    }
                    

                    M_SKU_Entity me = new M_SKU_Entity();
                    me.ITemCD = keyControls[(int)EIndex.ItemCD].Text;
                    me.ChangeDate = keyControls[(int)EIndex.ChangeDate].Text;                    
                    me.JanCD = jancd;

                    //データ上(M_SKU)に重複するJANCDが存在するかどうか
                    bool ret = mibl.M_SKU_SelectByJANCD(me);
                    if (ret)
                    {
                        if (bbl.ShowMessage("Q316") != DialogResult.Yes)
                        {
                            e.Cancel = true;
                            dgvDetail.CurrentCell = dgvDetail[columnIndex, rowIndex];
                            dgvDetail.Focus();
                            return;
                        }
                    }

                    ////画面上に重複するJANCDが存在する   F12押下時のみのチェックとする　4/13
                    ////重複チェック
                    //for (int i = 1; i < dgvDetail.Rows.Count; i++)
                    //{
                    //    for (int ic = 2; ic < dgvDetail.ColumnCount; ic++)
                    //    {
                    //        if (i == rowIndex && ic == columnIndex)
                    //            continue;

                    //        if (dgvDetail.Rows[i].Cells[ic].Value != null && dgvDetail.Rows[i].Cells[ic].Value.ToString().Equals(jancd))
                    //        {
                    //            if (bbl.ShowMessage("Q316") != DialogResult.Yes)
                    //            {
                    //                e.Cancel = true;
                    //                return;
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int columnIndex = e.ColumnIndex;
                int rowIndex = e.RowIndex;

                if (rowIndex > 0 && columnIndex > 1)
                {
                    //F9:検索使用可
                    //SetFuncKey(this, 8, true);
                    F9Visible = true;
                    //F10:使用可
                    SetFuncKey(this, 9, true);
                }
                else
                {
                    F9Visible = false;
                    //F10:使用不可
                    SetFuncKey(this, 9, false);
                }

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

        private void dgvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.CellStyle.BackColor = Color.Silver;
            }
        }

        #endregion

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     文字列が数値であるかどうかを返します。</summary>
        /// <param name="stTarget">
        ///     検査対象となる文字列。<param>
        /// <returns>
        ///     指定した文字列が数値であれば true。それ以外は false。</returns>
        /// -----------------------------------------------------------------------------
        public static bool IsNumeric(string stTarget)
        {
            ulong dNullable;

            return ulong.TryParse(
                stTarget,
                System.Globalization.NumberStyles.Any,
                null,
                out dNullable
            );
        }

    }
}
