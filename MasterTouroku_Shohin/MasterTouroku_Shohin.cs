using System;
using System.Data;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;
using Search;

namespace MasterTouroku_Shohin
{
    /// <summary>
    /// MasterTouroku_Shohin 商品ストアマスタ
    /// </summary>
    internal partial class MasterTouroku_Shohin : FrmMainForm
    {
        private const string ProID = "MasterTouroku_Shohin";
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

          , ColorNO
          , SizeNO
          //, ColorName
          //, SizeName

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
        , Chk15
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
        , TaniCD
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
          
          , PriceWithTax
          , PriceOutTax
          , OrderPriceWithTax
          , OrderPriceWithoutTax

          , OrderAttentionCD
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

          , LastYearTerm
          , LastSeason
          , LastCatalogNO
          , LastCatalogPage
          , LastCatalogText
          , LastInstructionsNO
          , LastInstructionsDate
                , InstructionsInfo  //マスタに項目がない
          , CommentInStore
          , CommentOutStore
          
          , WebAddress
          //, SetAdminCD
          //, SetItemCD
          //, SetSKUCD
          //, SetSU
          //, ApprovalDate
                
          , DeleteFlg
          ,COUNT
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            ITem,
        }
        private Control[] keyControls;
        private Control[] copyKeyControls;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private ITEM_BL mibl;
        private M_ITEM_Entity mie;
        private DataTable dtSKU;

        //private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        public MasterTouroku_Shohin()
        {
            InitializeComponent();
        }

        private void MasterTouroku_Shohin_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                this.SetFunctionLabel(EProMode.MENTE);
                this.InitialControlArray();
                mibl = new ITEM_BL();

                //起動時共通処理
                base.StartProgram();

                ScTani.Value1 = MultiPorpose_BL.ID_TANI;
                ScSports.Value1 = MultiPorpose_BL.ID_SPORTS;

                string ymd = bbl.GetDate();

                ckM_ComboBox1.Bind(ymd);
                ckM_ComboBox2.Bind(ymd);
                ckM_ComboBox3.Bind(ymd);
                ckM_ComboBox4.Bind(ymd);
                ckM_ComboBox5.Bind(ymd);
                ckM_ComboBox6.Bind(ymd);
                ckM_ComboBox7.Bind(ymd);
                ckM_ComboBox8.Bind(ymd);
                ckM_ComboBox9.Bind(ymd);
                ckM_ComboBox10.Bind(ymd);
                ckM_ComboBox11.Bind(ymd);
                ckM_ComboBox12.Bind(ymd);
                ckM_ComboBox13.Bind(ymd);
                ckM_ComboBox14.Bind(ymd);
                ckM_ComboBox15.Bind(ymd);
                ckM_ComboBox16.Bind(ymd);
                ckM_ComboBox17.Bind(ymd);

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

        private void SetEnabled()
        {
            for(int i = 0; i< (int)EIndex.COUNT; i++)
            {
                switch(i)
                {
                    case (int)EIndex.Chk6:
                    case (int)EIndex.Chk15:
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
                , ckM_TextBox6, ckM_TextBox24, ckM_TextBox20, ckM_TextBox25, ckM_TextBox4
                , ckM_TextBox2, ckM_TextBox1

                ,ckM_CheckBox1,ckM_CheckBox2,ckM_CheckBox3,ckM_CheckBox4,ckM_CheckBox5,ckM_CheckBox6
                ,ckM_CheckBox7,ckM_CheckBox8,ckM_CheckBox9,ckM_CheckBox10,ckM_CheckBox11,ckM_CheckBox12
                ,ckM_CheckBox13,ckM_CheckBox14,ckM_CheckBox15,ckM_CheckBox16,ckM_CheckBox17,ckM_CheckBox18
                ,ckM_CheckBox23,ckM_CheckBox24,ckM_CheckBox25,ckM_CheckBox26,ckM_CheckBox27,ckM_CheckBox28
                , ScBrand.TxtCode, ScTani.TxtCode, ScSports.TxtCode, ScVendor.TxtCode, ckM_TextBox21
                ,ckM_ComboBox1,ckM_ComboBox2,ckM_ComboBox3,ckM_ComboBox4,ckM_ComboBox5,ckM_ComboBox6,ckM_ComboBox7
                , ckM_TextBox18, ckM_TextBox17, ckM_TextBox8, ckM_TextBox9, ckM_TextBox11, ckM_TextBox10
                ,ckM_ComboBox18, ckM_TextBox13
                ,ckM_ComboBox8,ckM_ComboBox9,ckM_ComboBox10,ckM_ComboBox11,ckM_ComboBox12,ckM_ComboBox13
                ,ckM_ComboBox14,ckM_ComboBox15,ckM_ComboBox16,ckM_ComboBox17
                
                , ckM_TextBox19, ckM_TextBox14, ckM_TextBox22, ckM_TextBox23,TxtRemark
                , ckM_TextBox12, ckM_TextBox3,ckM_MultiLineTextBox1,ckM_MultiLineTextBox2,ckM_MultiLineTextBox3,ckM_MultiLineTextBox4
            };
            detailLabels = new CKM_SearchControl[] { ScBrand, ScTani, ScSports, ScVendor};
            searchButtons = new Control[] { ScBrand.BtnSearch,ScTani.BtnSearch, ScSports.BtnSearch,ScVendor.BtnSearch
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
                        //入力なければチェックなし
                        if (copyKeyControls[index].Text == "")
                        {
                            ScCopyITEM.LabelText = "";
                            return true;
                        }
                        else
                        {
                            //マスタデータチェック

                        }
                        break;

                    case (int)EIndex.ChangeDate:
                        //複写商品CDに入力がある場合、(When there is an input in 複写商品CD)Ｅ１０２
                        //必須入力
                        if (copyKeyControls[(int)EIndex.ItemCD].Text != "" )
                        {
                            if (!RequireCheck(new Control[] { copyKeyControls[index] }))
                            {
                                return false;
                            }
                        }

                        copyKeyControls[index].Text = bbl.FormatDate(copyKeyControls[index].Text);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(copyKeyControls[index].Text))
                        {
                            bbl.ShowMessage("E103");
                            return false;
                        }
                        break;
                }
            }

            if (index == (int)EIndex.ChangeDate)
            {
                //[M_ITEM_Select]
                mie = GetEntity(kbn);
                bool ret = mibl.M_ITEM_Select(mie);
                dtSKU = mibl.M_SKU_SelectByItemCD(mie);

                //・新規モードの場合（In the case of "new" mode）
                if (OperationMode == EOperationMode.INSERT && kbn == 0)
                {
                    //以下の条件で商品マスター(M_ITEM)が存在すればエラー (Error if record exists)Ｅ１３２
                    if (ret)
                    {
                        bbl.ShowMessage("E132");
                        return false;
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
                        detailControls[(int)EIndex.ItemCD].Text = mie.ITemCD;
                        detailControls[(int)EIndex.ColorNO].Text = mie.ColorNO;
                        detailControls[(int)EIndex.SizeNO].Text = mie.SizeNO;
                        InitGrid();
                        foreach (DataRow row in dtSKU.Rows)
                        {
                            int colNo = Convert.ToInt32(row["SizeNO"]);//Size
                            int rowNo = Convert.ToInt32(row["ColorNO"]);//Color

                            dgvDetail[colNo-1, rowNo-1].Value = row["SKUCD"].ToString();
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

                        //detailControls[(int)EIndex.ColorName].Text = mie.ColorName;
                        //detailControls[(int)EIndex.SizeName].Text = mie.SizeName;

                        if (mie.WebFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkWebFlg]).Checked = true;
                        if (mie.RealStoreFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.ChkRealStoreFlg]).Checked = true;
                        detailControls[(int)EIndex.MainVendorCD].Text = mie.MainVendorCD;
                        //detailControls[(int)EIndex.MakerVendorCD].Text = mie.MakerVendorCD;
                        detailControls[(int)EIndex.BrandCD].Text = mie.BrandCD;
                        detailControls[(int)EIndex.MakerItem].Text = mie.MakerItem;
                        detailControls[(int)EIndex.TaniCD].Text = mie.TaniCD;
                        detailControls[(int)EIndex.SportsCD].Text = mie.SportsCD;
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
                        detailControls[(int)EIndex.SaleStartDate].Text = mie.SaleStartDate;
                        detailControls[(int)EIndex.WebStartDate].Text = mie.WebStartDate;
                        detailControls[(int)EIndex.OrderAttentionCD].Text = mie.OrderAttentionCD;
                        detailControls[(int)EIndex.OrderAttentionNote].Text = mie.OrderAttentionNote;
                        detailControls[(int)EIndex.CommentInStore].Text = mie.CommentInStore;
                        detailControls[(int)EIndex.CommentOutStore].Text = mie.CommentOutStore;
                        detailControls[(int)EIndex.LastYearTerm].Text = mie.LastYearTerm;
                        detailControls[(int)EIndex.LastSeason].Text = mie.LastSeason;
                        detailControls[(int)EIndex.LastCatalogNO].Text = mie.LastCatalogNO;
                        detailControls[(int)EIndex.LastCatalogPage].Text = mie.LastCatalogPage;
                        detailControls[(int)EIndex.LastCatalogText].Text = mie.LastCatalogText;
                        detailControls[(int)EIndex.LastInstructionsNO].Text = mie.LastInstructionsNO;
                        detailControls[(int)EIndex.LastInstructionsDate].Text = mie.LastInstructionsDate;
                        detailControls[(int)EIndex.WebAddress].Text = mie.WebAddress;

                        if (mie.DeleteFlg.Equals("1"))
                            ((CheckBox)detailControls[(int)EIndex.DeleteFlg]).Checked = true;
                               
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
                MasterTouroku_SKU frmSku = new MasterTouroku_SKU(mie, dtSKU);
                
                frmSku.parSkucd = dgvDetail.Rows[RowIndex].Cells[ColumnIndex].Value.ToString();
                frmSku.ShowDialog();


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
            int colorNo = Convert.ToInt16(bbl.Z_Set(detailControls[(int)EIndex.ColorNO].Text));
            int sizeNo = Convert.ToInt16(bbl.Z_Set(detailControls[(int)EIndex.SizeNO].Text));

            if (colorNo.Equals(0) || sizeNo.Equals(0))
            {
                dgvDetail.Enabled = false;
                return;
            }
            if (colorNo > dgvDetail.Columns.Count)
            {
                for (int i = 0; i < colorNo - dgvDetail.Columns.Count; i++)
                {
                    dgvDetail.Columns.Add("col" + i.ToString(), "カラー名" + i.ToString());
                }
            }
            else if (colorNo < dgvDetail.Columns.Count)
            {
                for (int i = 1; i < dgvDetail.Columns.Count - colorNo; i++)
                {
                    dgvDetail.Columns.RemoveAt(dgvDetail.Columns.Count - 1);
                }
            }

            if (sizeNo > dgvDetail.Rows.Count)
            {
                int maxRow = sizeNo - dgvDetail.Rows.Count;
                for (int i = 0; i < maxRow; i++)
                {
                    dgvDetail.Rows.Add();
                }
            }
            else if (sizeNo < dgvDetail.Rows.Count)
            {
                int maxRow = dgvDetail.Rows.Count - sizeNo;
                for (int i = 1; i < maxRow; i++)
                {
                    dgvDetail.Rows.RemoveAt(dgvDetail.Rows.Count - 1);
                }
            }

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
        private bool CheckDetail(int index)
        {
            bool ret;

            switch (index)
            {
                case (int)EIndex.SKUName:
                    //商品ストア名 入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;

                case (int)EIndex.BrandCD:
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
                            ScBrand.LabelText = "";
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.TaniCD:
                case (int)EIndex.SportsCD:
                    ////入力必須(Entry required)
                    //if (detailControls[index].Enabled && string.IsNullOrWhiteSpace( detailControls[index].Text))
                    //{
                    //    //Ｅ１０２
                    //    bbl.ShowMessage("E102");
                    //    return false;
                    //}
                    //else 

                    ((Search.CKM_SearchControl)detailControls[index]).LabelText = "";

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //以下の条件でM_MultiPorposeが存在しない場合、エラー
                    string id = "";
                        if (index.Equals((int)EIndex.TaniCD))
                            id = MultiPorpose_BL.ID_TANI;
                        else
                            id = MultiPorpose_BL.ID_SPORTS;

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
                          ((Search.CKM_SearchControl)  detailControls[index]).LabelText = dt.Rows[0]["Char1"].ToString();
                        }
                        else
                        {
                            //Ｅ１０１
                            bbl.ShowMessage("E101");
                            return false;
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
                        ChangeDate = bbl.GetDate()
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

                case (int)EIndex.ColorNO:
                case (int)EIndex.SizeNO:
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    InitGrid();

                    break;

                case (int)EIndex.CmbTaxRateFLG:
                case (int)EIndex.CmbCostingKBN:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;

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
        /// <param name="kbn"></param>
        /// <returns></returns>
        private M_ITEM_Entity GetEntity(short kbn)
        {
            mie = new M_ITEM_Entity();

            if (kbn == 0)
            {
                mie.ITemCD = keyControls[(int)EIndex.ItemCD].Text;
                mie.ChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
            }
            else
            {
                mie.ITemCD = copyKeyControls[(int)EIndex.ItemCD].Text;
                mie.ChangeDate = copyKeyControls[(int)EIndex.ChangeDate].Text;
                return mie;
            }

            //Index調整用
            int index = 0;

            mie.VariousFLG = ((CheckBox)detailControls[index + (int)EIndex.ChkVariousFLG]).Checked ? "1" : "0";
            mie.ITemName = detailControls[index + (int)EIndex.SKUName].Text;
            mie.KanaName = detailControls[index + (int)EIndex.KanaName].Text;
            mie.SKUShortName = detailControls[index + (int)EIndex.SKUShortName].Text;
            mie.EnglishName = detailControls[index + (int)EIndex.EnglishName].Text;

            mie.ColorNO = detailControls[index + (int)EIndex.ColorNO].Text;
            mie.SizeNO = detailControls[index + (int)EIndex.SizeNO].Text;
            mie.SetKBN = ((CheckBox)detailControls[index + (int)EIndex.ChkSetKBN]).Checked ? "1" : "0";
            mie.PresentKBN = ((CheckBox)detailControls[index + (int)EIndex.ChkPresentKBN]).Checked ? "1" : "0";
            mie.SampleKBN = ((CheckBox)detailControls[index + (int)EIndex.ChkSampleKBN]).Checked ? "1" : "0";
            mie.DiscountKBN = ((CheckBox)detailControls[index + (int)EIndex.ChkDiscountKBN]).Checked ? "1" : "0";
            //mie.ColorName = detailControls[index + (int)EIndex.ColorName].Text;
            //mie.SizeName = detailControls[index + (int)EIndex.SizeName].Text;
            mie.WebFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkWebFlg]).Checked ? "1" : "0";
            mie.RealStoreFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkRealStoreFlg]).Checked ? "1" : "0";
            mie.MainVendorCD = detailControls[index + (int)EIndex.MainVendorCD].Text;
            //mie.MakerVendorCD = detailControls[index + (int)EIndex.MakerVendorCD].Text;
            mie.BrandCD = detailControls[index + (int)EIndex.BrandCD].Text;
            mie.MakerItem = detailControls[index + (int)EIndex.MakerItem].Text;
            mie.TaniCD = detailControls[index + (int)EIndex.TaniCD].Text;
            mie.SportsCD = detailControls[index + (int)EIndex.SportsCD].Text;
            mie.ZaikoKBN = ((CheckBox)detailControls[index + (int)EIndex.ChkZaikoKBN]).Checked ? "1" : "0";
            mie.Rack = detailControls[index + (int)EIndex.Rack].Text;
            mie.VirtualFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkVirtualFlg]).Checked ? "1" : "0";
            mie.DirectFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkDirectFlg]).Checked ? "1" : "0";
            mie.ReserveCD = detailControls[index + (int)EIndex.CmbReserveCD].Text;
            mie.NoticesCD = detailControls[index + (int)EIndex.CmbNoticesCD].Text;
            mie.PostageCD = detailControls[index + (int)EIndex.CmbPostageCD].Text;
            mie.ManufactCD = detailControls[index + (int)EIndex.CmbManufactCD].Text;
            mie.ConfirmCD = detailControls[index + (int)EIndex.CmbConfirmCD].Text;
            mie.WebStockFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkWebStockFlg]).Checked ? "1" : "0";
            mie.StopFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkStopFlg]).Checked ? "1" : "0";
            mie.DiscontinueFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkDiscontinueFlg]).Checked ? "1" : "0";
            mie.InventoryAddFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkInventoryAddFlg]).Checked ? "1" : "0";
            mie.MakerAddFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkMakerAddFlg]).Checked ? "1" : "0";
            mie.StoreAddFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkStoreAddFlg]).Checked ? "1" : "0";
            mie.NoNetOrderFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkNoNetOrderFlg]).Checked ? "1" : "0";
            mie.EDIOrderFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkEDIOrderFlg]).Checked ? "1" : "0";
            mie.CatalogFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkCatalogFlg]).Checked ? "1" : "0";
            mie.ParcelFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkParcelFlg]).Checked ? "1" : "0";
            mie.AutoOrderFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkAutoOrderFlg]).Checked ? "1" : "0";
            mie.TaxRateFLG = detailControls[index + (int)EIndex.CmbTaxRateFLG].Text;
            mie.CostingKBN = detailControls[index + (int)EIndex.CmbCostingKBN].Text;
            mie.SaleExcludedFlg = ((CheckBox)detailControls[index + (int)EIndex.ChkSaleExcludedFlg]).Checked ? "1" : "0";
            mie.PriceWithTax = detailControls[index + (int)EIndex.PriceWithTax].Text;
            mie.PriceOutTax = detailControls[index + (int)EIndex.PriceOutTax].Text;
            mie.OrderPriceWithTax = detailControls[index + (int)EIndex.OrderPriceWithTax].Text;
            mie.OrderPriceWithoutTax = detailControls[index + (int)EIndex.OrderPriceWithoutTax].Text;
            mie.SaleStartDate = detailControls[index + (int)EIndex.SaleStartDate].Text;
            mie.WebStartDate = detailControls[index + (int)EIndex.WebStartDate].Text;
            mie.OrderAttentionCD = detailControls[index + (int)EIndex.OrderAttentionCD].Text;
            mie.OrderAttentionNote = detailControls[index + (int)EIndex.OrderAttentionNote].Text;
            mie.CommentInStore = detailControls[index + (int)EIndex.CommentInStore].Text;
            mie.CommentOutStore = detailControls[index + (int)EIndex.CommentOutStore].Text;
            mie.LastYearTerm = detailControls[index + (int)EIndex.LastYearTerm].Text;
            mie.LastSeason = detailControls[index + (int)EIndex.LastSeason].Text;
            mie.LastCatalogNO = detailControls[index + (int)EIndex.LastCatalogNO].Text;
            mie.LastCatalogPage = detailControls[index + (int)EIndex.LastCatalogPage].Text;
            mie.LastCatalogText = detailControls[index + (int)EIndex.LastCatalogText].Text;
            mie.LastInstructionsNO = detailControls[index + (int)EIndex.LastInstructionsNO].Text;
            mie.LastInstructionsDate = detailControls[index + (int)EIndex.LastInstructionsDate].Text;
            mie.WebAddress = detailControls[index + (int)EIndex.WebAddress].Text;

            //チェックボックス
            mie.DeleteFlg = checkDeleteFlg.Checked ? "1" : "0";
            mie.UsedFlg = "0";
            mie.InsertOperator = InOperatorCD;
            mie.UpdateOperator = InOperatorCD;
            mie.PC = InPcID;

            return mie;
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
            mie = GetEntity(0);

            DataTable dt            = GetGridEntity();
            if (dt.Rows.Count == 0)
            {
                //更新対象なし
                bbl.ShowMessage("E102");
                return;
            }

            mibl.M_ITEM_Exec(mie,dt, (short)OperationMode);

            //更新後画面クリア
            InitScr();

            if(OperationMode== EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");
        }
        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("JuchuuRows", typeof(int));
            dt.Columns.Add("ShippingSu", typeof(int));
            dt.Columns.Add("SalesSU", typeof(int));
            dt.Columns.Add("SalesGaku", typeof(decimal));
            dt.Columns.Add("SalesTax", typeof(decimal));
            dt.Columns.Add("ZaikoKBN", typeof(int));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            foreach (DataRow row in dtSKU.Rows)
            {
                if (bbl.Z_Set(row["ShippingSu"]) > 0)
                    dt.Rows.Add(row["JuchuuRows"]
                        , bbl.Z_Set(row["ShippingSu"])
                        , 0     //bbl.Z_Set(row["SalesSU"]) 未使用
                        , bbl.Z_Set(row["SalesGaku"])
                        , bbl.Z_Set(row["SalesTax"])
                        , bbl.Z_Set(row["ZaikoKBN"])
                        , 0
                        );

            }

            return dt;
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
                case EsearchKbn.ITem:
                    using (Search_Store frmStore = new Search_Store())
                    {
                        frmStore.parChangeDate = keyControls[(int)EIndex.ChangeDate].Text;
                        frmStore.ShowDialog();

                        if (!frmStore.flgCancel)
                        {
                            setCtl.Text = frmStore.parStoreCD;
                            if (setCtl == keyControls[(int)EIndex.ItemCD])
                                keyControls[(int)EIndex.ChangeDate].Text = frmStore.parChangeDate;
                            else
                                copyKeyControls[(int)EIndex.ChangeDate].Text = frmStore.parChangeDate;
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

                            if(Kbn.Equals(1))
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
                            return;

                        InitScr();

                        break;
                    }

                //case 8: //F9:検索
                //    EsearchKbn kbn = EsearchKbn.Null;

                //    if (Array.IndexOf(keyControls, previousCtrl) == (int)EIndex.StoreCD ||
                //        Array.IndexOf(copyKeyControls, previousCtrl) == (int)EIndex.StoreCD)
                //    {
                //        //商品検索
                //        kbn = EsearchKbn.Store;
                //    }

                //    if (kbn != EsearchKbn.Null)
                //        SearchData(kbn, previousCtrl);

                //    break;
                case 9://F10:展開
                    if (dgvDetail.CurrentCell == null)
                        return;

                    int ColumnIndex = dgvDetail.CurrentCell.ColumnIndex;
                    int RowIndex = dgvDetail.CurrentCell.RowIndex;
                    Expand(ColumnIndex, RowIndex);
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
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
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
                                    ckM_CheckBox1.Focus();
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

        private void BtnSubF10_Click(object sender, EventArgs e)
        {
            //展開ボタンClick時   
            try
            {
                FunctionProcess(9);

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

        private void ScStore_Leave(object sender, EventArgs e)
        {
            ScBrand.ChangeDate = ScITEM.ChangeDate;
            ScTani.ChangeDate = ScITEM.ChangeDate;
            ScSports.ChangeDate = ScITEM.ChangeDate;
            ScVendor.ChangeDate = ScITEM.ChangeDate;
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

        #endregion

    }
}
