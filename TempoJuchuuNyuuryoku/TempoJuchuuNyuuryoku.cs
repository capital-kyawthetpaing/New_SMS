using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace TempoJuchuuNyuuryoku
{
    /// <summary>
    /// TempoJuchuuNyuuryoku 店舗受注入力
    /// </summary>
    internal partial class TempoJuchuuNyuuryoku : FrmMainForm
    {
        private const string ProID = "TempoJuchuuNyuuryoku";
        private const string ProNm = "店舗受注入力";
        private const short mc_L_END = 3; // ロック用
        private const string ZaikoSyokai = "ZaikoSyokai.exe";        

        private enum EIndex : int
        {
            JuchuuNO,
            CopyJuchuuNO,
            MitsumoriNO,
            ckM_CheckBox1,
            MotoJuchuuNO,
            StoreCD,

            JuchuuDate = 0,
            SoukoName,
            StaffCD,
            CustomerCD,
            CustomerName,
            AddressBtn,
            CustomerName2,
            AliasKBN,
            Tel1,
            Tel2,
            Tel3,

            DeliveryCD,
            DeliveryName,
            AddressBtn2,
            DeliveryName2,
            AliasKBN2,
            Tel1D,
            Tel2D,
            Tel3D,

            PaymentMethodCD,
            //Discount,
            //Point,
            SalesDate,
            FirstPaypentPlanDate,
            NouhinsyoComment,

            RemarksOutStore,
            RemarksInStore,
            COUNT
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product,
            Vendor
        }

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private FrmAddress addInfo;
        private TempoJuchuuNyuuryoku_BL mibl;
        private D_Juchuu_Entity dje;
        private int mTaxFractionKBN;
        private int mTennic;
        private string mMesTxt = "受注番号"; 

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string InOperatorName = "";
        private string mTemporaryReserveNO = "";
        private string mOldJyuchuNo = "";    //排他処理のため使用
        private string mOldJyuchuDate = "";
        private string mOldCustomerCD = "";
        private string mOldDeliveryCD = "";
        private decimal mZei10;//通常税額(Hidden)
        private decimal mZei8;//軽減税額(Hidden)
        private string mPaymentMethodCD;    //M_Customer.PaymentMethodCD
        private int mTaxTiming;
        private int mOrderTaxTiming;
        private decimal mOrderTax10;//通常税額(Hidden)
        private decimal mOrderTax8;//軽減税額(Hidden)

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridJuchuu mGrid = new ClsGridJuchuu();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_MaxJyuchuGyoNo;

        #region -- 明細部をグリッドのように扱うための関数 ↓--------------------

        // ----------------------------------------------------------------
        // 明細部初期化
        // 画面のコントロールを配列にセット
        // 固定色/フォーカスセット不可(クリックでのみフォーカスセット)/使用可 等の初期設定
        // Tab等によるフォーカス移動順の設定
        // 行の色の繰り返し単位と色を設定
        // ----------------------------------------------------------------
        private void S_SetInit_Grid()
        {
            int W_CtlRow;

            if (ClsGridJuchuu.gc_P_GYO <= ClsGridJuchuu.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridJuchuu.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridJuchuu.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridJuchuu.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridJuchuu.gc_MaxCL;

            // スクロールが取れるValueの最大値 (画面の最下行にデータの最下行が来た時点の Value)
            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;

            // スクロールの設定
            this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_0.SmallChange = 1;
            this.Vsb_Mei_0.Minimum = 0;
            // Valueはこの値までとることは不可能にしないといけないが、LargeChangeの分を余分に入れないとスクロールバーを使用して最後までスクロールすることができなくなる
            this.Vsb_Mei_0.Maximum = mGrid.g_MK_MaxValue + this.Vsb_Mei_0.LargeChange - 1;


            // コントロールを配列にセット
            S_SetControlArray();

            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int w_CtlCol = 0; w_CtlCol <= mGrid.g_MK_Ctl_Col - 1; w_CtlCol++)
                {
                    if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl != null)
                    {
                        if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        {
                            Search.CKM_SearchControl sctl = (Search.CKM_SearchControl)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.TxtCode.Enter += new System.EventHandler(GridControl_Enter);
                            sctl.TxtCode.Leave += new System.EventHandler(GridControl_Leave);
                            sctl.TxtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            sctl.TxtCode.Tag = W_CtlRow.ToString();
                            sctl.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                        {
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                        {
                            CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.Enter += new System.EventHandler(GridControl_Enter);
                            sctl.Leave += new System.EventHandler(GridControl_Leave);
                            sctl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            sctl.Tag = W_CtlRow.ToString();
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        {
                            Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;

                            if(w_CtlCol == (int)ClsGridJuchuu.ColNO.Site)
                                btn.Click += new System.EventHandler(BTN_Site_Click);
                            else
                                btn.Click += new System.EventHandler(BTN_Zaiko_Click);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                        {
                            GridControl.clsGridCheckBox check = (GridControl.clsGridCheckBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            check.Enter += new System.EventHandler(GridControl_Enter);
                            check.Leave += new System.EventHandler(GridControl_Leave);
                            check.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            check.Click += new System.EventHandler(CHK_Print_Click);
                        }
                    }

                    //switch (w_CtlCol)
                    //{
                    //    case (int)ClsGridMitsumori.ColNO.SizeName:
                    //    case (int)ClsGridMitsumori.ColNO.MitsumoriHontaiGaku:
                    //    case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:    // 
                    //    case (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice:    // 
                    //    case (int)ClsGridMitsumori.ColNO.TaniCD:    // 
                    //    case (int)ClsGridMitsumori.ColNO.ColorName:    // 
                    //    case (int)ClsGridMitsumori.ColNO.SetKBN:    // 
                    //    case (int)ClsGridMitsumori.ColNO.ProfitGaku:    // 
                    //    case (int)ClsGridMitsumori.ColNO.CostUnitPrice:    // 
                    //    case (int)ClsGridMitsumori.ColNO.CommentOutStore:    // 
                    //    case (int)ClsGridMitsumori.ColNO.IndividualClientName:    //  
                    //        //if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    //        //    ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).CKM_Reqired = true;

                    //        break;
                    //}
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridJuchuu.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridJuchuu.ColNO.GYONO; i <= (int)ClsGridJuchuu.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridJuchuu.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridJuchuu.ColNO.JuchuuSuu:
                            mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                            break;
                        case (int)ClsGridJuchuu.ColNO.JuchuuHontaiGaku:
                        case (int)ClsGridJuchuu.ColNO.JuchuuUnitPrice:
                        case (int)ClsGridJuchuu.ColNO.JuchuuGaku:
                        case (int)ClsGridJuchuu.ColNO.CostUnitPrice:
                        case (int)ClsGridJuchuu.ColNO.CostGaku:
                        case (int)ClsGridJuchuu.ColNO.ProfitGaku:
                        case (int)ClsGridJuchuu.ColNO.OrderUnitPrice:
                        //case (int)ClsGridJuchuu.ColNO.OrderGaku:
                            mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                            break;
                    }

                    mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }

            // TabStopﾌﾟﾛﾊﾟﾃｨの初期状態をセット
            Set_GridTabStop(false);

            // 明細部クリア
            //S_Clear_Grid();   Scr_Clr処理で
        }

        // 明細部のコントロールを配列にセット
        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Space1, 0].CellCtl = ckM_TextBox9;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Space2, 0].CellCtl = ckM_TextBox12;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Space3, 0].CellCtl = ckM_TextBox17;

            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SKUCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JanCD, 0].CellCtl = SC_ITEM_0;// IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SizeName, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CostGaku, 0].CellCtl = IMN_GENKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuGaku, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuHontaiGaku, 0].CellCtl = IMN_TEIKA2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuSuu, 0].CellCtl = IMN_GENER_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuUnitPrice, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.TaniCD, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ColorName, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SetKBN, 0].CellCtl = IMN_CLINT2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ProfitGaku, 0].CellCtl = IMN_SALEP_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CostUnitPrice, 0].CellCtl = IMN_SALEP2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CommentOutStore, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.IndividualClientName, 0].CellCtl = IMN_WEBPR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CommentInStore, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.TaxRateDisp, 0].CellCtl = IMT_ZKDIS_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.TaxRate, 0].CellCtl = IMT_ZEIRT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Site, 0].CellCtl = BTN_Site_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Zaiko, 0].CellCtl = BTN_Zaiko_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SoukoName, 0].CellCtl = IMC_SOKNM_0;          //出荷倉庫
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuOrderNO, 0].CellCtl = IMT_JUONO_0;      //発注番号
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.VendorCD, 0].CellCtl = IMT_VENCD_0;           //発注先
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.VendorName, 0].CellCtl = IMT_VENNM_0;         //発注先名
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.PaymentPlanDate, 0].CellCtl = IMT_PAYDT_0;    //支払予定日
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CollectClearDate, 0].CellCtl = IMT_COLDT_0;   //入金日
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Nyuka, 0].CellCtl = IMT_NYUKA_0;   //入荷進捗
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Syukka, 0].CellCtl = IMT_SYUKA_0;   //出荷進捗
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Hikiate, 0].CellCtl = IMT_HIKAT_0;   //引当表示
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.NotPrintFLG, 0].CellCtl = CHK_PRINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ChkTyokuso, 0].CellCtl = CHK_Tyokuso_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ChkFuyo, 0].CellCtl = CHK_FUYO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ChkExpress, 0].CellCtl = CHK_Express_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ShippingPlanDate, 0].CellCtl = IMT_SPDAT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.OrderUnitPrice, 0].CellCtl = IMN_ORTAN_0;
            //mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.OrderGaku, 0].CellCtl = IMN_ORGAK_0;

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Space1, 1].CellCtl = ckM_TextBox10;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Space2, 1].CellCtl = ckM_TextBox4;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Space3, 1].CellCtl = ckM_TextBox16;

            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SKUCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JanCD, 1].CellCtl = SC_ITEM_1;//IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SizeName, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CostGaku, 1].CellCtl = IMN_GENKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuGaku, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuHontaiGaku, 1].CellCtl = IMN_TEIKA2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuSuu, 1].CellCtl = IMN_GENER_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuUnitPrice, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.TaniCD, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ColorName, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SetKBN, 1].CellCtl = IMN_CLINT2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ProfitGaku, 1].CellCtl = IMN_SALEP_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CostUnitPrice, 1].CellCtl = IMN_SALEP2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CommentOutStore, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.IndividualClientName, 1].CellCtl = IMN_WEBPR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CommentInStore, 1].CellCtl = IMT_REMAK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.TaxRateDisp, 1].CellCtl = IMT_ZKDIS_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.TaxRate, 1].CellCtl = IMT_ZEIRT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Site, 1].CellCtl = BTN_Site_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Zaiko, 1].CellCtl = BTN_Zaiko_1;

            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SoukoName, 1].CellCtl = IMC_SOKNM_1;          //出荷倉庫
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuOrderNO, 1].CellCtl = IMT_JUONO_1;      //発注番号
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.VendorCD, 1].CellCtl = IMT_VENCD_1;           //発注先
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.VendorName, 1].CellCtl = IMT_VENNM_1;         //発注先名
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.PaymentPlanDate, 1].CellCtl = IMT_PAYDT_1;    //支払予定日
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CollectClearDate, 1].CellCtl = IMT_COLDT_1;   //入金日
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Nyuka, 1].CellCtl = IMT_NYUKA_1;   //入荷進捗
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Syukka, 1].CellCtl = IMT_SYUKA_1;   //出荷進捗
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Hikiate, 1].CellCtl = IMT_HIKAT_1;   //引当表示
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.NotPrintFLG, 1].CellCtl = CHK_PRINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ChkTyokuso, 1].CellCtl = CHK_Tyokuso_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ChkFuyo, 1].CellCtl = CHK_FUYO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ChkExpress, 1].CellCtl = CHK_Express_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ShippingPlanDate, 1].CellCtl = IMT_SPDAT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.OrderUnitPrice, 1].CellCtl = IMN_ORTAN_1;
            //mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.OrderGaku, 1].CellCtl = IMN_ORGAK_1;

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Space1, 2].CellCtl = ckM_TextBox11;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Space2, 2].CellCtl = ckM_TextBox3;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Space3, 2].CellCtl = ckM_TextBox15;

            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SKUCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JanCD, 2].CellCtl = SC_ITEM_2;//IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SizeName, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CostGaku, 2].CellCtl = IMN_GENKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuGaku, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuHontaiGaku, 2].CellCtl = IMN_TEIKA2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuSuu, 2].CellCtl = IMN_GENER_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuUnitPrice, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.TaniCD, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ColorName, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SetKBN, 2].CellCtl = IMN_CLINT2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ProfitGaku, 2].CellCtl = IMN_SALEP_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CostUnitPrice, 2].CellCtl = IMN_SALEP2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CommentOutStore, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.IndividualClientName, 2].CellCtl = IMN_WEBPR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CommentInStore, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.TaxRateDisp, 2].CellCtl = IMT_ZKDIS_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.TaxRate, 2].CellCtl = IMT_ZEIRT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Site, 2].CellCtl = BTN_Site_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Zaiko, 2].CellCtl = BTN_Zaiko_2;

            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SoukoName, 2].CellCtl = IMC_SOKNM_2;          //出荷倉庫
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.JuchuuOrderNO, 2].CellCtl = IMT_JUONO_2;      //発注番号
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.VendorCD, 2].CellCtl = IMT_VENCD_2;           //発注先
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.VendorName, 2].CellCtl = IMT_VENNM_2;         //発注先名
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.PaymentPlanDate, 2].CellCtl = IMT_PAYDT_2;    //支払予定日
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.CollectClearDate, 2].CellCtl = IMT_COLDT_2;   //入金日
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Nyuka, 2].CellCtl = IMT_NYUKA_2;   //入荷進捗
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Syukka, 2].CellCtl = IMT_SYUKA_2;   //出荷進捗
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.Hikiate, 2].CellCtl = IMT_HIKAT_2;   //引当表示
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.NotPrintFLG, 2].CellCtl = CHK_PRINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ChkTyokuso, 2].CellCtl = CHK_Tyokuso_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ChkFuyo, 2].CellCtl = CHK_FUYO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ChkExpress, 2].CellCtl = CHK_Express_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.ShippingPlanDate, 2].CellCtl = IMT_SPDAT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.OrderUnitPrice, 2].CellCtl = IMN_ORTAN_2;
            //mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.OrderGaku, 2].CellCtl = IMN_ORGAK_2;
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvNxt, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvPrv, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvNxt, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 MouseWheel の処理
        private void S_Grid_0_Event_MouseWheel(int pDelta)
        {
            int w_ToMove = pDelta * (-1) / (int)mGrid.g_WHEEL_DELTA;
            int w_Value;
            int w_MaxValue;

            mGrid.g_WheelFLG = true;

            //if (mGrid.g_MK_MaxValue > m_dataCnt - 1)
            //    w_MaxValue = m_dataCnt - 1;
            //else
                w_MaxValue = mGrid.g_MK_MaxValue;

            w_Value = Vsb_Mei_0.Value + w_ToMove;

            switch (w_Value)
            {
                case object _ when w_Value > w_MaxValue:
                    {
                        Vsb_Mei_0.Value = w_MaxValue;
                        break;
                    }

                case object _ when w_Value < Vsb_Mei_0.Minimum:
                    {
                        Vsb_Mei_0.Value = Vsb_Mei_0.Minimum;
                        break;
                    }

                default:
                    {
                        Vsb_Mei_0.Value = w_Value;
                        break;
                    }
            }

            mGrid.g_WheelFLG = false;
        }
        private void S_Clear_Grid()
        {
            int w_Row;
            int w_Col;

            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
            {
                for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                {

                    // 配列を初期化
                    mGrid.g_MK_State[w_Col, w_Row].SetDefault();
                    // Selectable=Trueに戻ったので、下記S_DispFromArrayで TabStopがTRUEになる。そのため同期を取るためFLG更新
                    mGrid.g_GridTabStop = true;

                    if (w_Row <= m_EnableCnt - 1)
                    {

                        // 固定色の列はその色を設定
                        switch (w_Col)
                        {
                            case (int)ClsGridJuchuu.ColNO.GYONO:
                            case (int)ClsGridJuchuu.ColNO.Space1:
                            case (int)ClsGridJuchuu.ColNO.Space2:
                            case (int)ClsGridJuchuu.ColNO.Space3:
                            case (int)ClsGridJuchuu.ColNO.Site:
                            case (int)ClsGridJuchuu.ColNO.Zaiko:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }
                            case (int)ClsGridJuchuu.ColNO.SKUCD:
                            case (int)ClsGridJuchuu.ColNO.SetKBN:
                            case (int)ClsGridJuchuu.ColNO.JuchuuHontaiGaku:
                            case (int)ClsGridJuchuu.ColNO.TaxRateDisp:
                            case (int)ClsGridJuchuu.ColNO.Hikiate:
                            case (int)ClsGridJuchuu.ColNO.TaniCD:
                            case (int)ClsGridJuchuu.ColNO.JuchuuGaku:
                            case (int)ClsGridJuchuu.ColNO.TaxRate:
                            case (int)ClsGridJuchuu.ColNO.CostGaku:
                            case (int)ClsGridJuchuu.ColNO.ProfitGaku:
                            case (int)ClsGridJuchuu.ColNO.JuchuuOrderNO:      //発注番号
                            case (int)ClsGridJuchuu.ColNO.VendorName:         //発注先名
                            case (int)ClsGridJuchuu.ColNO.PaymentPlanDate:    //支払予定日
                            case (int)ClsGridJuchuu.ColNO.CollectClearDate:   //入金日
                            case (int)ClsGridJuchuu.ColNO.Nyuka:
                            case (int)ClsGridJuchuu.ColNO.Syukka:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = true;
                                    break;
                                }
                        }
                    }

                    // 使用不可行は固定色を設定
                    if (w_Row > m_EnableCnt - 1)
                    {
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.MaxGyoColor;
                    }

                    // クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    switch (w_Col)
                    {
                        case (int)ClsGridJuchuu.ColNO.Site:
                        case (int)ClsGridJuchuu.ColNO.Zaiko:
                            {
                                // ボタン
                                S_Set_Cell_Selectable(w_Col, w_Row, false);
                                break;
                            }
                    }
                }
            }

            // データ用配列初期化
            Array.Clear(mGrid.g_DArray, mGrid.g_DArray.GetLowerBound(0), mGrid.g_DArray.GetLength(0));

            // 行番号の初期値セット
            for (w_Row = 0; w_Row <= mGrid.g_MK_Max_Row - 1; w_Row++)
                mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();

            // 画面クリア
            mGrid.S_DispFromArray(0, ref this.Vsb_Mei_0);

            Btn_SelectAll.Enabled = false;
            Btn_NoSelect.Enabled = false;
        }

        // -------------------------------------------------------------
        // 明細部TabStopを一括で切り替える(TabStopがTrueのコントロールがない時、KeyExitが発生しなくなるため)
        // -------------------------------------------------------------
        private void Set_GridTabStop(bool pTabStop)
        {
            int w_Row;
            int w_Col;
            int w_CtlRow;

            try
            {
                // 画面上にEnable=Trueのコントロールがひとつしかない時はTab,Shift+Tab,↑,↓押下時にKeyExitが発生しないため、
                // 明細がスクロールしなくなる。回避策として、そのパターンが起こりうる時のみ、IMT_DMY_0.TabStopをTrueにする。
                if (OperationMode == EOperationMode.UPDATE)
                    IMT_DMY_0.TabStop = pTabStop;
                else
                    IMT_DMY_0.TabStop = false;

                // 状態が変わらない時は処理を抜ける
                if (mGrid.g_GridTabStop == pTabStop)
                    return;

                // TabStopの状態を退避
                mGrid.g_GridTabStop = pTabStop;

                // TabStopﾌﾟﾛﾊﾟﾃｨをセット
                for (w_CtlRow = 0; w_CtlRow <= mGrid.g_MK_Ctl_Row - 1; w_CtlRow++)
                {
                    w_Row = w_CtlRow + Vsb_Mei_0.Value;

                    for (w_Col = 0; w_Col <= mGrid.g_MK_Ctl_Col - 1; w_Col++)
                        mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl.TabStop = mGrid.F_GetTabStop(w_Col, w_Row);
                }
            }

            catch
            {
                // 例外が発生した時は抜ける
                return;
            }
        }

        // -------------------------------------------------------------
        // 明細部Selectableを切り替え、TabStopの同期も取る
        // -------------------------------------------------------------
        private void S_Set_Cell_Selectable(int pCol, int pRow, bool pSelectable)
        {
            int w_CtlRow;
            w_CtlRow = pRow - Vsb_Mei_0.Value;

            mGrid.g_MK_State[pCol, pRow].Cell_Selectable = pSelectable;

            // 全行クリアなどのときは、画面範囲のみTABINDEX制御
            if (w_CtlRow < mGrid.g_MK_Ctl_Row & w_CtlRow > 0)
                mGrid.g_MK_Ctrl[pCol, w_CtlRow].CellCtl.TabStop = mGrid.F_GetTabStop(pCol, pRow);
        }

        // -------------------------------------------------------------
        // Tab移動可/不可の制御
        // -------------------------------------------------------------
        private void S_Selectable(int pCol, int pRow)
        {
        }

        private void Vsb_Mei_0_ValueChanged(object sender, System.EventArgs e)
        {
            int w_Dest = 0;
            int w_Row = 0;
            bool w_IsGrid;

            if (mGrid.g_VSB_Flg == 1)
                return;

            w_IsGrid = mGrid.F_Search_Ctrl_MK(this.ActiveControl, out int w_Col, out int w_CtlRow);
            if (w_IsGrid & mGrid.g_InMoveFocus_Flg == 0)
            {

                // 明細部にフォーカスがあるとき
                w_Row = w_CtlRow + Vsb_Mei_0.Value;

                if (Math.Sign(this.Vsb_Mei_0.Value - mGrid.g_MK_DataValue) == -1)
                    // 上へスクロール
                    w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvPrv;
                else
                    // 下へスクロール
                    w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvNxt;

                // 一旦 フォーカスを退避
                IMT_DMY_0.Focus();
            }

            // 画面の内容を、配列にセット(スクロール前の行に)
            mGrid.S_DispToArray(mGrid.g_MK_DataValue);

            // 配列より画面セット (スクロール後の行)
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);

            if (w_IsGrid & mGrid.g_InMoveFocus_Flg == 0)
            {

                // 元いた位置にフォーカスをセット(場合によってはロックがかかっているかもしれないのでセットしなおす)
                if (w_Dest == (int)ClsGridBase.Gen_MK_FocusMove.MvPrv)
                {
                    if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                        // その行の最後から探す
                        mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                }
                else if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                {
                    if (mGrid.g_WheelFLG == true)
                    {
                        // まず対象行の先頭からさがし、まったくフォーカス移動先が無ければ
                        // 最後のフォーカス可能セルに移動
                        if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, mGrid.g_MK_Ctrl[mGrid.g_MK_FocusOrder[0], w_CtlRow].CellCtl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]) == false)
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvPrv, this.ActiveControl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, mGrid.g_MK_Max_Row - 1, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                    }
                    else
                        // その行の先頭から探す
                        mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]);
                }
            }

            // 連続スクロールの途中に、画面の表示がおかしくなる現象への対策
            Pnl_Body.Refresh();
        }

        private void Vsb_Mei_0_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            S_Grid_0_Event_MouseWheel(e.Delta);
        }

        // ---------------------------------------------
        // ボディ部の状態制御
        // 
        // 引数    pKBN    0...  新規/修正/削除時(モード選択時)
        // 1...  修正時(画面展開後)
        // 2...  照会/削除時(画面展開後)明細入力不可、スクロールのみ
        // pGrid 0...  明細の各プロパティ以外    
        // 1...  明細部の各プロパティ(先に設定しておいてから、実際にpGrid=0で PanelのEnable制御等を行う)
        // ---------------------------------------------
        private void S_BodySeigyo(short pKBN, short pGrid)
        {
            int w_Row;

            switch (pKBN)
            {
                case 0:
                    {
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;

                                // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                                mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.JanCD, w_Row].Cell_Enabled = true;
                                //for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                //{
                                //    switch (w_Col)
                                //    {
                                //        case (int)ClsGridJuchuu.ColNO.JanCD:
                                //case (int)ClsGridJuchuu.ColNO.SKUName:    // 
                                //case (int)ClsGridJuchuu.ColNO.ColorName:    // 
                                //case (int)ClsGridJuchuu.ColNO.SizeName:
                                //case (int)ClsGridJuchuu.ColNO.MitsumoriSuu:    // 
                                //case (int)ClsGridJuchuu.ColNO.MitsumoriUnitPrice:    // 
                                //case (int)ClsGridJuchuu.ColNO.CommentOutStore:    // 
                                //case (int)ClsGridJuchuu.ColNO.IndividualClientName:    //  
                                //case (int)ClsGridJuchuu.ColNO.CommentInStore:    //
                                //case (int)ClsGridJuchuu.ColNO.Site:    //
                                //case (int)ClsGridJuchuu.ColNO.Zaiko:    // 

                                //case (int)ClsGridJuchuu.ColNO.SoukoName:          //出荷倉庫
                                //case (int)ClsGridJuchuu.ColNO.VendorCD:           //発注先
                                //case (int)ClsGridJuchuu.ColNO.ArrivePlanDate:    //入荷予定日
                                //        {
                                //            mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                //            break;
                                //        }
                                //}
                                //}
                            }
                            
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                keyControls[(int)EIndex.JuchuuNO].Text = "";
                                keyControls[(int)EIndex.JuchuuNO].Enabled = false;
                                ScJuchuuNO.BtnSearch.Enabled = false;
                                keyControls[(int)EIndex.CopyJuchuuNO].Enabled = true;
                                ScCopyJuchuuNO.BtnSearch.Enabled = true;
                                keyControls[(int)EIndex.MitsumoriNO].Enabled = true;
                                ScMitsumoriNO.BtnSearch.Enabled = true;
                                //新規Modeの場合のみCheck可能
                                ckM_CheckBox1.Enabled = true;

                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                detailControls[(int)EIndex.JuchuuDate].Text = bbl.GetDate();
                                F9Visible = false;

                                SetFuncKeyAll(this, "111111001001");
                            }
                            else
                            {
                                keyControls[(int)EIndex.JuchuuNO].Enabled = true;
                                ScJuchuuNO.BtnSearch.Enabled = true;
                                keyControls[(int)EIndex.CopyJuchuuNO].Text = "";
                                keyControls[(int)EIndex.CopyJuchuuNO].Enabled = false;
                                ScCopyJuchuuNO.BtnSearch.Enabled = false;
                                keyControls[(int)EIndex.MitsumoriNO].Enabled = false;
                                ScMitsumoriNO.BtnSearch.Enabled = false;
                                ckM_CheckBox1.Enabled = false;

                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                this.Vsb_Mei_0.TabStop = false;

                                SetFuncKeyAll(this, "111111001010");
                            }

                            SetEnabled(false);

                        }
                        break;
                    }

                case 1: //修正時
                    {
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;

                                // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JanCD))
                                {
                                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.JanCD, w_Row].Cell_Enabled = true;
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {

                                        case (int)ClsGridJuchuu.ColNO.JuchuuSuu:    // 
                                        case (int)ClsGridJuchuu.ColNO.JuchuuUnitPrice:    // 
                                        case (int)ClsGridJuchuu.ColNO.CommentOutStore:    // 
                                        case (int)ClsGridJuchuu.ColNO.IndividualClientName:    //  
                                        case (int)ClsGridJuchuu.ColNO.CommentInStore:    //
                                        case (int)ClsGridJuchuu.ColNO.Site:    //
                                        case (int)ClsGridJuchuu.ColNO.Zaiko:    // 
                                        case (int)ClsGridJuchuu.ColNO.NotPrintFLG:    // 
                                        case (int)ClsGridJuchuu.ColNO.ChkTyokuso:    // 
                                        case (int)ClsGridJuchuu.ColNO.ChkFuyo:    // 
                                        case (int)ClsGridJuchuu.ColNO.ChkExpress:    // 
                                        case (int)ClsGridJuchuu.ColNO.ShippingPlanDate:    //
                                        case (int)ClsGridJuchuu.ColNO.OrderUnitPrice:    //  
                                        //case (int)ClsGridJuchuu.ColNO.OrderGaku:    //  
                                                                                         //case (int)ClsGridJuchuu.ColNO.SoukoName:          //出荷倉庫
                                                                                         //case (int)ClsGridJuchuu.ColNO.VendorCD:           //発注先
                                                                                         //case (int)ClsGridJuchuu.ColNO.ArrivePlanDate:    //入荷予定日
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                break;
                                            }
                                        case (int)ClsGridJuchuu.ColNO.JanCD:
                                        case (int)ClsGridJuchuu.ColNO.SKUName:    // 
                                        case (int)ClsGridJuchuu.ColNO.ColorName:    // 
                                        case (int)ClsGridJuchuu.ColNO.SizeName:
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            //画面へデータセット後、明細部入力可、キー部入力不可
                            Scr_Lock(2, 3, 0);
                            Scr_Lock(0, 1, 1);
                            SetFuncKeyAll(this, "111111000001");
                        }
                        break;
                    }

                case 2:
                    {
                        if (pGrid == 1)
                        {
                            // 使用可項目無  明細部スクロールのみ可
                            // IMT_DMY_0.Focus()
                            //SetFuncKeyAll(this, "000010000101", "11100000");
                            Pnl_Body.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            //Scr_Lock(0, 0, 0);
                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000001");
                                IMT_DMY_0.Focus();
                                Scr_Lock(0, 0, 1);
                            }
                            else
                            {
                                CboStoreCD.Enabled = false;
                                SetFuncKeyAll(this, "111111000000");
                            }

                            // 削除時のみ、明細を参照できるように、スクロールバーのTabStopをTrueにする
                            this.Vsb_Mei_0.TabStop = true;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void Grid_Gotfocus(int pCol, int pRow, int pCtlRow)
        {
            bool W_Del = false;
            bool W_Ret;

            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            //mGrid.g_MK_Ctrl(ClsGridMitsumori.ColNO.DELCK, pCtlRow).GVal(W_Del);

            // ﾌｧﾝｸｼｮﾝﾎﾞﾀﾝ使用可否
            SetFuncKeyAll(this, "111111111111");

            // 検索ﾎﾞﾀﾝ使用不可.解除
            if (W_Del == true)
                W_Ret = false;
            else
                switch (pCol)
                {
                    default:
                        {
                            W_Ret = false;
                            break;
                        }
                }
            SetFuncKey(this, 8, W_Ret);
        }

        private void Grid_NotFocus(int pCol, int pRow)
        {
            // ﾌｫｰｶｽをはじく
            int w_Col;
            bool w_AllFlg = false;
            int w_CtlRow;

            if (OperationMode >= EOperationMode.DELETE)
                return;

            if (m_EnableCnt - 1 < pRow)
                return;

            w_CtlRow = pRow - Vsb_Mei_0.Value;
            if (w_CtlRow >= 0 && w_CtlRow <= mGrid.g_MK_Ctl_Row - 1)
                //画面範囲の時
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0, w_CtlRow, w_CtlRow);


            if (pCol == (int)ClsGridJuchuu.ColNO.JanCD || w_AllFlg)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].JanCD))
                {
                    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        if (w_Col == (int)ClsGridJuchuu.ColNO.JanCD)
                            //JANCD使用可
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                        else
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                    }

                    w_AllFlg = false;
                }
                else
                {
                    //JANCD入力時
                    w_AllFlg = true;

                    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        switch (w_Col)
                        {
                            case (int)ClsGridJuchuu.ColNO.JanCD:
                                if (mGrid.g_DArray[pRow].juchuGyoNO == 0)
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                }
                                else
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                }
                                break;

                            case (int)ClsGridJuchuu.ColNO.JuchuuSuu:
                                //if (mGrid.g_DArray[pRow].DiscountKbn == 1)
                                //{
                                //    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                //    mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = true;
                                //}
                                //else
                                //{
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = false;
                                //}
                                break;

                            case (int)ClsGridJuchuu.ColNO.SKUName:
                            case (int)ClsGridJuchuu.ColNO.ColorName:    
                            case (int)ClsGridJuchuu.ColNO.SizeName:
                            case (int)ClsGridJuchuu.ColNO.CostUnitPrice:
                                if (mGrid.g_DArray[pRow].VariousFLG == 1)
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = false;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                }
                                else
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = true;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                }
                                break;

                                
                            case (int)ClsGridJuchuu.ColNO.JuchuuUnitPrice:    // 
                            case (int)ClsGridJuchuu.ColNO.CommentOutStore:    // 
                            case (int)ClsGridJuchuu.ColNO.IndividualClientName:    //  
                            case (int)ClsGridJuchuu.ColNO.CommentInStore:    //
                            case (int)ClsGridJuchuu.ColNO.NotPrintFLG:    //
                            case (int)ClsGridJuchuu.ColNO.ChkTyokuso:    //
                            case (int)ClsGridJuchuu.ColNO.ChkFuyo:    //
                            case (int)ClsGridJuchuu.ColNO.Site:    //
                            case (int)ClsGridJuchuu.ColNO.Zaiko:    // 
                            case (int)ClsGridJuchuu.ColNO.ChkExpress:    // 
                            case (int)ClsGridJuchuu.ColNO.ShippingPlanDate:    //
                            case (int)ClsGridJuchuu.ColNO.OrderUnitPrice:    //  
                            //case (int)ClsGridJuchuu.ColNO.OrderGaku:    //  
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                                                    }
                                break;

                            case (int)ClsGridJuchuu.ColNO.VendorCD:           //発注先
                            case (int)ClsGridJuchuu.ColNO.ArrivePlanDate:    //入荷予定日
                                {
                                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].Nyuka))
                                    {
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                    }
                                    else
                                    {
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                    }
                                }
                                break;

                            case (int)ClsGridJuchuu.ColNO.SoukoName:          //出荷倉庫
                                {
                                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].Syukka))
                                    {
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                    }
                                    else
                                    {
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                    }
                                }
                                break;

                        }

                    }
                    w_AllFlg = false;
                    
                }
            }
            if (pCol == (int)ClsGridJuchuu.ColNO.Nyuka)
            {
                if (mGrid.g_DArray[pRow].Nyuka == "")
                {
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_Enabled = true;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_ReadOnly = false;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_Bold = false;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_Enabled = true;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_ReadOnly = false;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_Bold = false;
                }
                else
                {
                    //入荷予定日,発注先 入力不可
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_Enabled = false;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_ReadOnly = true;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_Bold = true;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_Enabled = false;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_ReadOnly = true;
                    mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_Bold = true;
                }
            }
            else if (pCol == (int)ClsGridJuchuu.ColNO.CostUnitPrice)
            {
                if (mGrid.g_DArray[pRow].VariousFLG == 1)
                {
                    mGrid.g_MK_State[pCol, pRow].Cell_Enabled = true;
                    mGrid.g_MK_State[pCol, pRow].Cell_ReadOnly = false;
                    mGrid.g_MK_State[pCol, pRow].Cell_Bold = false;
                }
                else
                {
                    mGrid.g_MK_State[pCol, pRow].Cell_Enabled = false;
                    mGrid.g_MK_State[pCol, pRow].Cell_ReadOnly = true;
                    mGrid.g_MK_State[pCol, pRow].Cell_Bold = true;
                }
            }


        }

        // -----------------------------------------------
        // <明細部>行削除処理 Ｆ７
        // -----------------------------------------------
        private void DEL_SUB()
        {
            int w_Row;

            Control w_Act = ActiveControl;
            if (ActiveControl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                w_Act = previousCtrl;

            if (mGrid.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細が出荷済の受注明細、または、売上済の受注明細である場合、削除できない（F7ボタンを使えないようにする）
            //出荷済・売上済の場合は行削除不可
            if (mGrid.g_DArray[w_Row].Syukka == "出荷済" || mGrid.g_DArray[w_Row].Syukka =="売上済")
                return;

            for (int i = w_Row; i < mGrid.g_MK_Max_Row - 1; i++)
            {
             int   w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //次行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i + 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            CalcKin();

            int col = (int)ClsGridJuchuu.ColNO.JanCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //フォーカスセット
            IMT_DMY_0.Focus();

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

        }

        // -----------------------------------------------
        // <明細部>行複写処理 Ｆ６
        // -----------------------------------------------
        private void CPY_SUB()
        {
            Control w_Act = ActiveControl;
            int w_Row;
            int w_Gyo;

            w_Act = this.ActiveControl;

            if (ActiveControl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                w_Act = previousCtrl;

            if (mGrid.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //先頭行のとき、複写不可
            if (w_Row == 0)
                return;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            int col = (int)ClsGridJuchuu.ColNO.JanCD;

            //コピー行より下の明細を1行ずつずらす（内容コピー）
            for (int i = mGrid.g_MK_Max_Row - 1; i >= w_Row; i--)
            {
                w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                mGrid.g_DArray[i] = mGrid.g_DArray[i - 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号

                if (i.Equals(w_Row))
                {
                    //前の行をコピーしてできた新しい行
                    mGrid.g_DArray[i].JuchuuNO = "";
                    mGrid.g_DArray[i].juchuGyoNO = 0;
                    mGrid.g_DArray[i].copyJuchuGyoNO = 0;
                    mGrid.g_DArray[i].KariHikiateNO = "";
                }

                Grid_NotFocus(col, i);
            }

            //状態もコピー
            // ※ 前行と状態が違うとき注意、この部分変更要 (修正元のあるなしで 入力可能項目が変わる場合など)
            for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                mGrid.g_MK_State[w_Col, w_Row] = mGrid.g_MK_State[w_Col, w_Row - 1];
            }

            string ymd = detailControls[(int)EIndex.JuchuuDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            CheckHikiate(w_Row, ymd);
            Grid_NotFocus(col, w_Row);
            CalcKin();

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, IMT_DMY_0, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);
        }
        private void ADD_SUB()
        {
            Control w_Act = ActiveControl;
            int w_Row;
            int w_Gyo;

            if (ActiveControl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                w_Act = previousCtrl;

            if (mGrid.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }
            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //追加行より下の明細を1行ずつずらす（内容コピー）
            for (int i = mGrid.g_MK_Max_Row - 1; i > w_Row; i--)
            {
                w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //前行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i - 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            w_Gyo = Convert.ToInt16(mGrid.g_DArray[w_Row].GYONO);
            // 一行クリア
            Array.Clear(mGrid.g_DArray, w_Row, 1);
            //退避内容を戻す
            mGrid.g_DArray[w_Row].GYONO = w_Gyo.ToString();          //行番号

            CalcKin();

            int col = (int)ClsGridJuchuu.ColNO.JanCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

        }
        private void Grid_Gyo_Clr(int RW)  // 明細部１行クリア
        {
            string w_Gyo;

            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            w_Gyo = mGrid.g_DArray[RW].GYONO;

            // 一行クリア
            Array.Clear(mGrid.g_DArray, RW, 1);

            mGrid.g_DArray[RW].GYONO = w_Gyo;

            //ZEI_SUB(); // 消費税計算
            //Kin_Kei(); // 再計算

            // JanCD列以外入力不可 (JanCDを入力した時点で他の列が入力可になるため)
            Grid_NotFocus((int)ClsGridJuchuu.ColNO.JanCD, RW);

            // 配列の内容を画面にセット
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
        }

        #endregion

        public TempoJuchuuNyuuryoku()
        {
            InitializeComponent();

            //ホイールイベントの追加  
            this.Vsb_Mei_0.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.Vsb_Mei_0_MouseWheel);

        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.INPUT);
                this.InitialControlArray();
                lblDisp.Visible = true;
                Btn_F11.Text = "引当(F11)";

                addInfo = new FrmAddress();

                // 明細部初期化
                this.S_SetInit_Grid();

                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();

                mTennic = bbl.GetTennic();

                //受注処理番号
                if(mTennic.Equals(1))
                {
                    mMesTxt = "受注処理番号";
                    ScJuchuuNO.Stype = CKM_SearchControl.SearchType.受注処理番号;
                    ScCopyJuchuuNO.Stype = CKM_SearchControl.SearchType.受注処理番号;
                    ScMotoJuchuNo.Stype = CKM_SearchControl.SearchType.受注処理番号;
                    LblJuchuNo.Visible = false;
                    LblCopyJuchuNo.Visible = false;
                    LblMotoJuchuNo.Visible = false;
                }
                else
                {
                    LblJuchuNoT.Visible = false;
                    LblCopyJuchuNoT.Visible = false;
                    LblMotoJuchuNoT.Visible = false;
                }

                string ymd = bbl.GetDate();
                mibl = new TempoJuchuuNyuuryoku_BL();
                CboStoreCD.Bind(ymd);
                CboSoukoName.Bind(ymd);
                CboPaymentMethodCD.Bind(string.Empty);

                string stores = GetAllAvailableStores();
                ScJuchuuNO.Value1 = InOperatorCD;
                ScJuchuuNO.Value2 = stores;
                ScCopyJuchuuNO.Value1 = InOperatorCD;
                ScCopyJuchuuNO.Value2 = stores;
                ScMitsumoriNO.Value1 = InOperatorCD;
                ScMitsumoriNO.Value2 = stores;
                ScMotoJuchuNo.Value1 = InOperatorCD;
                ScMotoJuchuNo.Value2 = stores;
                ScCustomer.Value1 = "1";
                ScDeliveryCD.Value1 = "1";


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
                    ScStaff.LabelText = mse.StaffName;
                }
                InOperatorName = mse.StaffName;
                StoreCD = mse.StoreCD;  //初期値を退避

                detailControls[(int)EIndex.JuchuuDate].Text = ymd;

                for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
                {
                    CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SoukoName, W_CtlRow].CellCtl;
                    sctl.Bind(ymd);
                }

                //出荷指示登録・店舗受注照会から起動された場合、照会モードで起動
                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    string juchuNO = cmds[(int)ECmdLine.PcID + 1];   //
                    ChangeOperationMode(EOperationMode.SHOW);
                    keyControls[(int)EIndex.JuchuuNO].Text = juchuNO;
                    CheckKey((int)EIndex.JuchuuNO, true);
                }
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
            keyControls = new Control[] {  ScJuchuuNO.TxtCode, ScCopyJuchuuNO.TxtCode, ScMitsumoriNO.TxtCode, ckM_CheckBox1, ScMotoJuchuNo.TxtCode, CboStoreCD };
            keyLabels = new Control[] {  };
            detailControls = new Control[] { ckM_TextBox1,CboSoukoName, ScStaff.TxtCode
                         , ScCustomer.TxtCode,ckM_TextBox7,btnAddress, ckM_Text_4, panel1, ckM_TextBox18 , ckM_TextBox8 , ckM_TextBox13
                         , ScDeliveryCD.TxtCode,ckM_TextBox21,btnAddress2, ckM_TextBox22, panel2, ckM_TextBox20 , ckM_TextBox14 , ckM_TextBox19
                         , CboPaymentMethodCD  , ckM_TextBox5,ckM_TextBox6
                         , ckM_MultiLineTextBox1,TxtRemark1,TxtRemark2 };
            detailLabels = new Control[] { ScCustomer,ScStaff, ScDeliveryCD };
            searchButtons = new Control[] { ScCustomer.BtnSearch, ScStaff.BtnSearch,ScDeliveryCD.BtnSearch};

            //イベント付与
            foreach (Control ctl in keyControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyControl_KeyDown);
                ctl.Enter += new System.EventHandler(KeyControl_Enter);
            }
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

            radioSama.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
            radioOnchu.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);

            radioSama.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioOnchu.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioOnchuD.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButtonD_KeyDown);
            radioSamaD.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButtonD_KeyDown);
        }
        
        /// <summary>
        /// PrimaryKeyのコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckKey(int index, bool set=true)
        {

            switch (index)
            {
                case (int)EIndex.JuchuuNO:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    
                    //排他処理
                    bool ret = SelectAndInsertExclusive();
                    if (!ret)
                        return false;

                    return CheckData(set);

                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { keyControls[index] }))
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
                            CboStoreCD.Focus();
                            return false;
                        }

                    }

                    break;

                case (int)EIndex.CopyJuchuuNO:
                    if (!string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return CheckData(set, index);

                    break;

                case (int)EIndex.MotoJuchuuNO:
                    if (!string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return CheckData(set, index);

                    break;

                case (int)EIndex.MitsumoriNO:
                    if (!string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return CheckDataMitsumori(set);

                    break;

            }

            return true;

        }

        private bool SelectAndInsertExclusive()
        {
            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT)
                return true;
            
            DeleteExclusive();

            if (string.IsNullOrWhiteSpace(keyControls[(int)EIndex.JuchuuNO].Text))
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Jyuchu,
                Number = keyControls[(int)EIndex.JuchuuNO].Text,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(),dt.Rows[0]["Operator"].ToString());
                keyControls[(int)EIndex.JuchuuNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldJyuchuNo = keyControls[(int)EIndex.JuchuuNO].Text;
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
       private void DeleteExclusive()
        {
            if (mOldJyuchuNo == "")
                return;

            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Jyuchu,
                Number = mOldJyuchuNo,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);

            mOldJyuchuNo = "";
        }
        
        /// <summary>
        /// 受注データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set, int index= (int)EIndex.JuchuuNO)
        {
            //[D_Juchu_SelectData]
            dje = new D_Juchuu_Entity();

            if(index == (int)EIndex.CopyJuchuuNO)
                dje.JuchuuNO = keyControls[(int)EIndex.CopyJuchuuNO].Text;
            else if (index == (int)EIndex.MotoJuchuuNO)
                dje.JuchuuNO = keyControls[(int)EIndex.MotoJuchuuNO].Text;
            else
                dje.JuchuuNO = keyControls[(int)EIndex.JuchuuNO].Text;

            DataTable dt = mibl.D_Juchu_SelectData(dje, (short)OperationMode, (short)mTennic);

            //受注(D_Juchuu)に存在しない場合、Error 「登録されていない受注番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", mMesTxt);
                Scr_Clr(1);
                previousCtrl.Focus();
                return false;
            }
            else
            {
                //DeleteDateTime 「削除された受注番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", mMesTxt);
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                //権限がない場合（以下のSelectができない場合）Error　「権限のない受注番号」
                if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                {
                    bbl.ShowMessage("E139", mMesTxt);
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }
                if (index == (int)EIndex.JuchuuNO)
                {
                    //進捗チェック　既に売上済み,出荷済み,出荷指示済み,ピッキングリスト完了済み,仕入済み,入荷済み,発注済み警告
                    bool ret = mibl.CheckJuchuData(dje.JuchuuNO, out string errno, (short)mTennic);
                    if (ret)
                    {
                        if (!string.IsNullOrWhiteSpace(errno))
                        {
                            //警告メッセージを表示する
                            bbl.ShowMessage(errno);

                            //E165：売上済み
                            if (errno.Equals("E165"))
                            {
                                lblDisp.Text = "売上済";
                            }
                        }
                    }
                    //【削除モードの時】
                    //出荷済・売上済の明細を含んだ受注は、そのものの削除を不可
                    if (OperationMode==EOperationMode.DELETE)
                    {
                        //E159:出荷済み、E165：売上済み
                        if ( errno.Equals("E165"))
                        {
                            //売上済明細が存在するので、その受注データの削除も許さない
                            bbl.ShowMessage("E168");
                            previousCtrl.Focus();
                            return false;
                        }else if(errno.Equals("E159"))
                        {
                            //出荷済明細が存在するので、その受注データの削除も許さない
                            bbl.ShowMessage("E167");
                            previousCtrl.Focus();
                            return false;
                        }
                    }

                    //D_TemporaryReserveをDelete
                    mibl.DeleteTemporaryReserve(dje);
                }

                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                m_dataCnt = 0;
                m_MaxJyuchuGyoNo = 0;

                foreach (DataRow row in dt.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString()); 
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return false;
                    }

                    if (i == 0)
                    {
                        CboStoreCD.SelectedValue = row["StoreCD"];

                        //明細にデータをセット
                        if (index == (int)EIndex.JuchuuNO)
                        {
                            detailControls[(int)EIndex.JuchuuDate].Text = row["JuchuuDate"].ToString();
                        }
                        else
                        {
                            detailControls[(int)EIndex.JuchuuDate].Text = bbl.GetDate();
                        }
                        mOldJyuchuDate = detailControls[(int)EIndex.JuchuuDate].Text;

                        CboSoukoName.SelectedValue = row["SoukoCD"];
                        detailControls[(int)EIndex.StaffCD].Text = row["StaffCD"].ToString();
                        CheckDetail((int)EIndex.StaffCD);

                        detailControls[(int)EIndex.CustomerCD].Text = row["CustomerCD"].ToString();
                        CheckDetail((int)EIndex.CustomerCD);

                        if (detailControls[(int)EIndex.CustomerName].Enabled)
                        {
                            detailControls[(int)EIndex.CustomerName].Text = row["CustomerName"].ToString();
                            addInfo.ade.ZipCD1 = row["ZipCD1"].ToString();
                            addInfo.ade.ZipCD2 = row["ZipCD2"].ToString();
                            addInfo.ade.Address1 = row["Address1"].ToString();
                            addInfo.ade.Address2 = row["Address2"].ToString();
                            addInfo.ade.Tel11 = row["Tel11"].ToString();
                            addInfo.ade.Tel12 = row["Tel12"].ToString();
                            addInfo.ade.Tel13 = row["Tel13"].ToString();
                        }
                        detailControls[(int)EIndex.CustomerName2].Text = row["CustomerName2"].ToString();
                        detailControls[(int)EIndex.Tel1].Text = row["Tel11"].ToString();
                        detailControls[(int)EIndex.Tel2].Text = row["Tel12"].ToString();
                        detailControls[(int)EIndex.Tel3].Text = row["Tel13"].ToString();

                        if (row["AliasKBN"].ToString() == "1")
                            radioSama.Checked = true;
                        else
                            radioOnchu.Checked = true;

                        detailControls[(int)EIndex.DeliveryCD].Text = row["DeliveryCD"].ToString();
                        CheckDetail((int)EIndex.DeliveryCD);

                        if (detailControls[(int)EIndex.DeliveryName].Enabled)
                        {
                            detailControls[(int)EIndex.DeliveryName].Text = row["DeliveryName"].ToString();
                            addInfo.adeD.ZipCD1 = row["DeliveryZipCD1"].ToString();
                            addInfo.adeD.ZipCD2 = row["DeliveryZipCD2"].ToString();
                            addInfo.adeD.Address1 = row["DeliveryAddress1"].ToString();
                            addInfo.adeD.Address2 = row["DeliveryAddress2"].ToString();
                            addInfo.adeD.Tel11 = row["DeliveryTel11"].ToString();
                            addInfo.adeD.Tel12 = row["DeliveryTel12"].ToString();
                            addInfo.adeD.Tel13 = row["DeliveryTel13"].ToString();
                        }
                        detailControls[(int)EIndex.DeliveryName2].Text = row["DeliveryName2"].ToString();
                        detailControls[(int)EIndex.Tel1D].Text = row["DeliveryTel11"].ToString();
                        detailControls[(int)EIndex.Tel2D].Text = row["DeliveryTel12"].ToString();
                        detailControls[(int)EIndex.Tel3D].Text = row["DeliveryTel13"].ToString();

                        if (row["DeliveryAliasKBN"].ToString() == "1")
                            radioSamaD.Checked = true;
                        else
                            radioOnchuD.Checked = true;

                        //【Data Area Footer】
                        CboPaymentMethodCD.SelectedValue = row["PaymentMethodCD"].ToString();// mPaymentMethodCD;
                        //detailControls[(int)EIndex.Point].Text = bbl.Z_SetStr(row["Point"]);
                        detailControls[(int)EIndex.SalesDate].Text = row["SalesDate"].ToString();
                        detailControls[(int)EIndex.FirstPaypentPlanDate].Text = row["FirstPaypentPlanDate"].ToString();
                        detailControls[(int)EIndex.NouhinsyoComment].Text = row["NouhinsyoComment"].ToString();
                        detailControls[(int)EIndex.RemarksInStore].Text = row["CommentInStore"].ToString();
                        detailControls[(int)EIndex.RemarksOutStore].Text = row["CommentOutStore"].ToString();

                        lblKin1.Text = bbl.Z_SetStr(row["SUM_JuchuuGaku"]);
                        lblKin5.Text = bbl.Z_SetStr(row["Discount"]);
                        lblKin6.Text = bbl.Z_SetStr(row["HanbaiGaku"]);
                        lblKin2.Text = bbl.Z_SetStr(row["HanbaiHontaiGaku"]);
                        lblKin3.Text = bbl.Z_SetStr(row["SUM_CostGaku"]);
                        lblKin4.Text = bbl.Z_SetStr(row["SUM_ProfitGaku"]);
                        lblKin7.Text = bbl.Z_SetStr(row["InvoiceGaku"]);
                        lblKin10.Text = bbl.Z_SetStr(bbl.Z_Set(row["HanbaiTax8"])+ bbl.Z_Set(row["HanbaiTax10"]));

                        if (index == (int)EIndex.JuchuuNO)
                        {
                            if (string.IsNullOrWhiteSpace(row["D_Sales_SalesDate"].ToString()))
                            {
                                detailControls[(int)EIndex.SalesDate].Text = row["SalesPlanDate"].ToString();
                            }
                            else
                            {
                                detailControls[(int)EIndex.SalesDate].Text = row["D_Sales_SalesDate"].ToString();
                            }
                            lblKin8.Text = bbl.Z_SetStr(row["CollectGaku"]);
                        }
                        else
                        {
                            detailControls[(int)EIndex.SalesDate].Text = "";
                            lblKin8.Text = "";
                        }
                        lblKin9.Text = lblKin7.Text;    
                        lblKin11.Text = bbl.Z_SetStr(row["HanbaiTax10"]);
                        lblKin12.Text = bbl.Z_SetStr(row["HanbaiTax8"]);
                        //lblKin13.Text = bbl.Z_SetStr(row["OrderGaku"]);

                        if (index == (int)EIndex.JuchuuNO)
                        {
                            lblDay1.Text = row["BillingCloseDate"].ToString();
                            lblDay2.Text = row["CollectPlanDate"].ToString();
                            lblDay3.Text = row["CollectClearDate"].ToString();
                        }
                        else
                        {
                            lblDay1.Text = "";
                            lblDay2.Text = "";
                            lblDay3.Text = "";
                        }

                        //明細なしの場合
                        if (bbl.Z_Set(row["JuchuuRows"]) == 0)
                            break;
                    }

                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    //mGrid.g_DArray[i].OldJanCD = mGrid.g_DArray[i].JanCD;     del 4/24
                    mGrid.g_DArray[i].AdminNO = row["SKUNO"].ToString();
                    mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();

                    if (index == (int)EIndex.MotoJuchuuNO)
                    {
                        mGrid.g_DArray[i].JuchuuSuu =bbl.Z_SetStr(-1 * bbl.Z_Set(row["JuchuuSuu"]));   //単価算出のため先にセットしておく    
                    }
                    else
                    {
                        mGrid.g_DArray[i].JuchuuSuu = bbl.Z_SetStr(row["JuchuuSuu"]);   //単価算出のため先にセットしておく    
                    }
                    if (index == (int)EIndex.JuchuuNO)
                    {
                        mGrid.g_DArray[i].JuchuuNO = row["JuchuuNO"].ToString();
                        mGrid.g_DArray[i].juchuGyoNO = Convert.ToInt16(row["JuchuuRows"].ToString());
                        if (m_MaxJyuchuGyoNo < mGrid.g_DArray[i].juchuGyoNO)
                            m_MaxJyuchuGyoNo = mGrid.g_DArray[i].juchuGyoNO;
                    }
                    else if (index == (int)EIndex.CopyJuchuuNO)
                    {
                        mGrid.g_DArray[i].copyJuchuGyoNO = Convert.ToInt16(row["JuchuuRows"].ToString());
                    }

                    mGrid.g_DArray[i].SoukoName = row["M_SoukoCD"].ToString();  //引当処理のため

                    CheckGrid((int)ClsGridJuchuu.ColNO.JanCD, i, true);

                    mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();   // 
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 
                    //if (row["SetKBN"].ToString() == "1")
                    //    mGrid.g_DArray[i].SetKBN = "○";
                    //else
                    //    mGrid.g_DArray[i].SetKBN = "";
                    mGrid.g_DArray[i].NotPrintFLG = row["NotPrintFLG"].ToString() == "1" ? true : false;
                    mGrid.g_DArray[i].ChkTyokuso = row["DirectFLG"].ToString() == "1" ? true : false;
                    mGrid.g_DArray[i].ChkFuyo = row["NotOrderFLG"].ToString() == "1" ? true : false;
                    mGrid.g_DArray[i].ChkExpress = row["ExpressFLG"].ToString() == "1" ? true : false;

                    mGrid.g_DArray[i].JuchuuHontaiGaku = bbl.Z_SetStr(row["JuchuuHontaiGaku"]);   // 
                    //mGrid.g_DArray[i].TaxRateDisp = bbl.Z_SetStr(row["MemberPriceOutTax"]);   // 
                    mGrid.g_DArray[i].JuchuuUnitPrice = bbl.Z_SetStr(row["JuchuuUnitPrice"]);   // 
                    mGrid.g_DArray[i].JuchuuGaku = bbl.Z_SetStr(row["JuchuuGaku"]);   // 
                    mGrid.g_DArray[i].CostUnitPrice = bbl.Z_SetStr(row["CostUnitPrice"]);   // 
                    mGrid.g_DArray[i].CostGaku = bbl.Z_SetStr(row["CostGaku"]);   //  
                    mGrid.g_DArray[i].OrderUnitPrice = bbl.Z_SetStr(row["OrderUnitPrice"]);   // 
                    //mGrid.g_DArray[i].OrderGaku = bbl.Z_SetStr(row["D_OrderGaku"]);   // 

                    CalcZei(i);

                    //mGrid.g_DArray[i].TaniName = bbl.Z_SetStr(row["TaniName"]);   // CheckGridでセット
                    mGrid.g_DArray[i].CommentInStore = row["D_CommentInStore"].ToString();   // 
                    mGrid.g_DArray[i].CommentOutStore = row["D_CommentOutStore"].ToString();   // 
                    mGrid.g_DArray[i].IndividualClientName = row["IndividualClientName"].ToString();   // 

                    mGrid.g_DArray[i].ProfitGaku = bbl.Z_SetStr(row["ProfitGaku"]);   // 
                    
                    //税額(Hidden)
                    mGrid.g_DArray[i].Tax = bbl.Z_Set(mGrid.g_DArray[i].JuchuuGaku) - bbl.Z_Set(mGrid.g_DArray[i].JuchuuHontaiGaku);
                    if (mGrid.g_DArray[i].TaxRateFLG.Equals(1))
                    {
                        mGrid.g_DArray[i].JuchuTax = bbl.Z_Set(row["JuchuuTax"]);
                    }
                    else if (mGrid.g_DArray[i].TaxRateFLG.Equals(2))
                    {
                        mGrid.g_DArray[i].KeigenTax = bbl.Z_Set(row["JuchuuTax"]);
                    }                                  

                    //複写時以外
                    if (index != (int)EIndex.CopyJuchuuNO)
                    {
                        //進捗チェック　既に売上済み,出荷済み,出荷指示済み,ピッキングリスト完了済み,仕入済み,入荷済み,発注済み警告
                        bool ret = mibl.CheckJuchuDetailsData(dje.JuchuuNO, row["JuchuuRows"].ToString(), out string status, out string status2);
                        if (ret)
                        {
                            mGrid.g_DArray[i].Nyuka = status;
                            mGrid.g_DArray[i].Syukka = status2;
                        }
                    }
                    mGrid.g_DArray[i].JuchuuOrderNO = row["JuchuuOrderNO"].ToString();
                    mGrid.g_DArray[i].VendorCD = row["VendorCD"].ToString();
                    if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[i].VendorCD))
                        CheckGrid((int)ClsGridJuchuu.ColNO.VendorCD, i);
                    else
                        mGrid.g_DArray[i].VendorName = "";
                    mGrid.g_DArray[i].ArrivePlanDate = row["ArrivePlanDate"].ToString();
                    mGrid.g_DArray[i].PaymentPlanDate = row["PaymentPlanDate"].ToString();
                    mGrid.g_DArray[i].CollectClearDate = row["D_CollectClearDate"].ToString();
                    mGrid.g_DArray[i].ShippingPlanDate = row["ShippingPlanDate"].ToString();
                    mGrid.g_DArray[i].OldShippingPlanDate = mGrid.g_DArray[i].ShippingPlanDate;

                    //mGrid.g_DArray[i].KeigenTax = bbl.Z_Set(row["CollectClearDate"]);


                    m_dataCnt = i + 1;
                    Grid_NotFocus((int)ClsGridJuchuu.ColNO.JanCD, i);
                    i++;
                }

                mOldJyuchuDate = detailControls[(int)EIndex.JuchuuDate].Text;

                CalcKin();

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            if (OperationMode == EOperationMode.UPDATE )
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            }
            else if(OperationMode == EOperationMode.INSERT)
            {
                //複写コピー後
                //画面へデータセット後、明細部入力可
                Scr_Lock(2, 3, 0);
                S_BodySeigyo(0, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            }
            else
            {
                S_BodySeigyo(2, 0);
                S_BodySeigyo(2, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                previousCtrl.Focus();
            }

            return true;
        }

        /// <summary>
        /// 見積データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDataMitsumori(bool set)
        {
            //[D_Mitsumori_SelectData]
            D_Mitsumori_Entity dme = new D_Mitsumori_Entity();
            MitsumoriNyuuryoku_BL mbl = new MitsumoriNyuuryoku_BL();

            dme.MitsumoriNO = keyControls[(int)EIndex.MitsumoriNO].Text;

            DataTable dt = mbl.D_Mitsumori_SelectData(dme, (short)OperationMode);

            //以下の条件で見積入力ーが存在しなければエラー (Error if record does not exist)「登録されていない見積番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", "見積番号");
                Scr_Clr(1);
                previousCtrl.Focus();
                return false;
            }
            else
            {
                //DeleteDateTime 「削除された見積番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", "見積番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                //権限がない場合（以下のSelectができない場合）Error　「権限のない見積番号」
                if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                {
                    bbl.ShowMessage("E139", "見積番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                DataRow[] selectRow=  dt.Select("JanCD is not null");
                if(selectRow.Length == 0)
                {
                    bbl.ShowMessage("E138", "見積番号");   
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                bool reCalc = false;

                foreach (DataRow row in dt.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return false;
                    }

                    if (i == 0)
                    {
                        CboStoreCD.SelectedValue = row["StoreCD"];

                        //明細にデータをセット
                        string ymd = bbl.GetDate();
                        detailControls[(int)EIndex.JuchuuDate].Text = ymd;
                        mOldJyuchuDate = ymd;

                        //[M_Souko_Select]
                        M_Souko_Entity me = new M_Souko_Entity
                        {
                            StoreCD = row["StoreCD"].ToString(),
                            SoukoType = "3",
                            ChangeDate = ymd,
                            DeleteFlg="0"
                        };
                        
                        DataTable sdt = mibl.M_Souko_SelectForMitsumori(me);
                        if (sdt.Rows.Count > 0)
                        {
                            CboSoukoName.SelectedValue  = sdt.Rows[0]["SoukoCD"];
                        }
                        else
                        {
                            CboSoukoName.SelectedValue = "";
                        }
                        
                        detailControls[(int)EIndex.StaffCD].Text = row["StaffCD"].ToString();
                        CheckDetail((int)EIndex.StaffCD);

                        detailControls[(int)EIndex.CustomerCD].Text = row["CustomerCD"].ToString();
                        CheckDetail((int)EIndex.CustomerCD);

                        if (detailControls[(int)EIndex.CustomerName].Enabled)
                        {
                            detailControls[(int)EIndex.CustomerName].Text = row["CustomerName"].ToString();
                            detailControls[(int)EIndex.CustomerName2].Text = row["CustomerName2"].ToString();
                            addInfo.ade.ZipCD1 = row["ZipCD1"].ToString();
                            addInfo.ade.ZipCD2 = row["ZipCD2"].ToString();
                            addInfo.ade.Address1 = row["Address1"].ToString();
                            addInfo.ade.Address2 = row["Address2"].ToString();
                            addInfo.ade.Tel11 = row["Tel11"].ToString();
                            addInfo.ade.Tel12 = row["Tel12"].ToString();
                            addInfo.ade.Tel13 = row["Tel13"].ToString();
                        }

                        detailControls[(int)EIndex.Tel1].Text = row["Tel11"].ToString();
                        detailControls[(int)EIndex.Tel2].Text = row["Tel12"].ToString();
                        detailControls[(int)EIndex.Tel3].Text = row["Tel13"].ToString();

                        if (row["AliasKBN"].ToString() == "1")
                            radioSama.Checked = true;
                        else
                            radioOnchu.Checked = true;

                        //【Footer】
                        detailControls[(int)EIndex.RemarksInStore].Text = row["RemarksInStore"].ToString();
                        detailControls[(int)EIndex.RemarksOutStore].Text = row["RemarksOutStore"].ToString();

                        lblKin5.Text ="0";          //請求額
                        //ckM_TextBox2.Text = "0";    //ポイント
                        ckM_TextBox5.Text = "" ;//売上日
                        lblKin8.Text = "0";//入金額
                        ckM_TextBox6.Text = ""; //入金予定日
                        lblDay1.Text = "";
                        lblDay2.Text = "";
                        lblDay3.Text = "";

                        //【Data Area Footer】
                        ////[M_MultiPorpose]
                        //MultiPorpose_BL mpbl = new MultiPorpose_BL();
                        //M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity
                        //{
                        //    ID = MultiPorpose_BL.ID_PaymentMethod,
                        //    Key = mPaymentMethodCD
                        //};
                        //DataTable dtMm = mpbl.M_MultiPorpose_Select(mme);
                        //if(dtMm.Rows.Count>0)
                        //{
                        //    CboPaymentMethodCD.SelectedValue = mme.Char1;
                        //}
                        CboPaymentMethodCD.SelectedValue = mPaymentMethodCD;
                        detailControls[(int)EIndex.NouhinsyoComment].Text = row["RemarksOutStore"].ToString();

                        //明細なしの場合
                        if (bbl.Z_Set(row["MitsumoriRows"]) == 0)
                            break;
                    }
                    if (string.IsNullOrWhiteSpace(row["JanCD"].ToString()))
                        continue;

                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    //mGrid.g_DArray[i].OldJanCD = mGrid.g_DArray[i].JanCD;
                    mGrid.g_DArray[i].AdminNO = row["SKUNO"].ToString();
                    mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();
                    mGrid.g_DArray[i].JuchuuSuu = bbl.Z_SetStr(row["MitsumoriSuu"]);   //単価算出のため先にセットしておく    
                    CheckGrid((int)ClsGridJuchuu.ColNO.JanCD, i, true);

                    mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();   // 
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 
                    //if (row["SetKBN"].ToString() == "1")
                    //    mGrid.g_DArray[i].SetKBN = "○";
                    //else
                    //    mGrid.g_DArray[i].SetKBN = "";
                    mGrid.g_DArray[i].NotPrintFLG = row["NotPrintFLG"].ToString() == "1" ? true : false;
                    //mGrid.g_DArray[i].TaxRateDisp = bbl.Z_SetStr(row["MemberPriceOutTax"]);   // 

                    //画面表示時にメッセージ表示
                    if (i == 0)
                    {
                        if (bbl.ShowMessage("Q303") == DialogResult.Yes)
                            reCalc = true;
                    }

                    if(!reCalc)
                    {
                        mGrid.g_DArray[i].JuchuuUnitPrice = bbl.Z_SetStr(row["MitsumoriUnitPrice"]);   //販売単価
                        mGrid.g_DArray[i].JuchuuGaku = bbl.Z_SetStr(row["MitsumoriGaku"]);   //税込販売額
                        mGrid.g_DArray[i].JuchuuHontaiGaku = bbl.Z_SetStr(row["MitsumoriHontaiGaku"]);   //税抜販売額
                        mGrid.g_DArray[i].NotReCalc = true;
                    }
                    mGrid.g_DArray[i].SoukoName = CboSoukoName.SelectedValue.ToString();

                    //[M_SKULastCost]
                    M_SKULastCost_Entity msce = new M_SKULastCost_Entity
                    {
                        AdminNO = mGrid.g_DArray[i].AdminNO
                        ,SoukoCD = mGrid.g_DArray[i].SoukoName    //Todo:必要か確認。たぶん必要
                    };
                    bool ret=mibl.M_SKULastCost_Select(msce);
                    if (ret)
                    {
                        mGrid.g_DArray[i].CostUnitPrice = bbl.Z_SetStr(msce.LastCost);
                        mGrid.g_DArray[i].GetCostUnitPrice = bbl.Z_Set(msce.LastCost);
                        mGrid.g_DArray[i].CostGaku = bbl.Z_SetStr(bbl.Z_Set(msce.LastCost) * bbl.Z_Set(row["MitsumoriSuu"]));   //  
                    }

                    CalcZei(i);

                    //mGrid.g_DArray[i].TaniName = bbl.Z_SetStr(row["TaniName"]);   // 
                    mGrid.g_DArray[i].CommentInStore = row["CommentInStore"].ToString();   // 
                    mGrid.g_DArray[i].CommentOutStore = row["CommentOutStore"].ToString();   // 
                    mGrid.g_DArray[i].IndividualClientName = row["IndividualClientName"].ToString();   // 

                    //粗利額
                    mGrid.g_DArray[i].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[i].JuchuuHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[i].CostGaku)); // 
                    //税額
                    mGrid.g_DArray[i].Tax = bbl.Z_Set(mGrid.g_DArray[i].JuchuuGaku) - bbl.Z_Set(mGrid.g_DArray[i].JuchuuHontaiGaku);
                    mGrid.g_DArray[i].OrderUnitPrice = "0";   //発注単価

                    i++;
                }

                CalcKin();

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            if (OperationMode == EOperationMode.UPDATE)
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridJuchuu.ColNO.JanCD);
            }
            else if (OperationMode == EOperationMode.INSERT)
            {
                //複写コピー後
                //画面へデータセット後、明細部入力可
                Scr_Lock(2, 3, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            }
            else
            {
                S_BodySeigyo(2, 0);
                S_BodySeigyo(2, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                previousCtrl.Focus();
            }

            return true;
        }
        protected override void ExecDisp()
        {
            //引当処理
            string ymd = detailControls[(int)EIndex.JuchuuDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();
        
            dje = new D_Juchuu_Entity();
            dje.JuchuuNO = keyControls[(int)EIndex.JuchuuNO].Text == "" ? mTemporaryReserveNO : keyControls[(int)EIndex.JuchuuNO].Text;
        
            //D_TemporaryReserveをDelete
            mibl.DeleteTemporaryReserve(dje);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                    CheckHikiate(RW, ymd);
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        }

        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set=true)
        {

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.JuchuuDate:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        detailControls[index].Focus();
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if(!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        detailControls[index].Focus();
                        return false;
                    }

                    //受注日が変更された場合のチェック処理
                    if (mOldJyuchuDate != detailControls[index].Text)
                    {
                        for (int i = (int)EIndex.StaffCD; i <= (int)EIndex.DeliveryCD; i++)
                            if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                                if (!CheckDependsOnDate(i, true))
                                    return false;

                        //明細部JANCDの再チェック
                        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                        {
                            if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                            {
                                if (CheckGrid((int)ClsGridJuchuu.ColNO.JanCD, RW, true, true) == false)
                                {
                                    //Focusセット処理
                                    ERR_FOCUS_GRID_SUB((int)ClsGridJuchuu.ColNO.JanCD, RW);
                                    return false;
                                }
                            }
                        }
                        mOldJyuchuDate = detailControls[index].Text;
                        ScCustomer.ChangeDate = mOldJyuchuDate;
                        ScDeliveryCD.ChangeDate = mOldJyuchuDate;
                    }

                    break;

                case (int)EIndex.SoukoName:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboSoukoName.MoveNext = false;
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                    {
                        CboSoukoName.MoveNext = false;
                        return false;
                    }                        
                    break;

                case (int)EIndex.StaffCD:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }

                    if (!CheckDependsOnDate(index))
                        return false;

                    break;

                case (int)EIndex.CustomerCD:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(0);
                        detailControls[index].Focus();
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                        return false;

                   
                    break;

                case (int)EIndex.CustomerName:
                case (int)EIndex.DeliveryName:
                    //入力可能の場合 入力必須(Entry required)
                    if (detailControls[index].Enabled)
                    {
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            detailControls[index].Focus();
                            return false;
                        }
                        //顧客名2に入力無い場合、先頭20Byteを顧客名2へセット   delete 4/24
                        if (string.IsNullOrWhiteSpace(detailControls[index + 2].Text))
                        {
                            detailControls[index + 2].Text = bbl.LeftB(detailControls[index].Text, 20);
                        }
                    }
                    break;

                case (int)EIndex.CustomerName2:
                case (int)EIndex.DeliveryName2:
                    //入力可能の場合 入力必須(Entry required)
                    if (detailControls[index].Enabled && string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;

                case (int)EIndex.DeliveryCD:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(1);
                        detailControls[index].Focus();
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                        return false;

                    break;

                case (int)EIndex.PaymentMethodCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboPaymentMethodCD.MoveNext = false;
                        return false;
                    }
                    break;

                //case (int)EIndex.Point:
                //    //入力された場合 ポイント＞	Form.Header.残ポイントの場合、Error				
                //    if(bbl.Z_Set(detailControls[index].Text)>0)
                //        if(bbl.Z_Set(detailControls[index].Text) > bbl.Z_Set(lblPoint.Text))
                //        {
                //            //Ｅ１４６
                //            bbl.ShowMessage("E146");
                //            detailControls[index].Focus();
                //            return false;
                //        }

                //    break;

                case (int)EIndex.SalesDate:
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
                        detailControls[index].Focus();
                        return false;
                    }

                    //入力できる範囲内の日付であること
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        detailControls[index].Focus();
                        return false;
                    }

                    //Form.Detail.入荷予定日＞form.売上予定日の場合、
                    bool errFlg = false;
                    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                    {
                        if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ArrivePlanDate))
                        {
                            int result = detailControls[index].Text.CompareTo(mGrid.g_DArray[RW].ArrivePlanDate);
                            if (result < 0)
                            {
                                errFlg = true;
                                break;
                            }
                        }
                    }
                    if (errFlg)
                    {
                        //「入荷予定日が売上予定日より後になっている商品があります。このまま処理を進めますか？」
                        if (bbl.ShowMessage("Q304") != DialogResult.Yes)
                        {
                            detailControls[index].Focus();
                            //売上予定日にCursorを移動
                            return false;
                        }
                    }
                        break;

                case (int)EIndex.FirstPaypentPlanDate:
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
                        detailControls[index].Focus();
                        return false;
                    }

                    break;

            }

            return true;
        }

        // ********************************************
        // ERR時のSETFOCUS  ERR_FOCUS_GRID_SUB
        // 
        // *******************************************
        private void ERR_FOCUS_GRID_SUB(int pCol, int pRow)
        {
            Control w_Ctrl;
            bool w_Ret;
            int w_CtlRow;

            w_CtlRow = pRow - Vsb_Mei_0.Value;

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            w_Ctrl = detailControls[(int)EIndex.PaymentMethodCD];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridJuchuu.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        /// <summary>
        /// 日付が変更されたときに必要なチェック処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDependsOnDate(int index, bool ChangeDate = false)
        {
            string ymd = detailControls[(int)EIndex.JuchuuDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            switch (index)
            {
                case (int)EIndex.SoukoName:
                    //[M_Souko_Select]
                    M_Souko_Entity me = new M_Souko_Entity
                    {
                        SoukoCD = CboSoukoName.SelectedValue.ToString(),
                        ChangeDate = ymd,
                        DeleteFlg = "0"
                    };

                    DataTable mdt = mibl.M_Souko_IsExists(me);
                    if (mdt.Rows.Count > 0)
                    {
                        if (!base.CheckAvailableStores(mdt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E141");
                            detailControls[index].Focus();
                            return false;
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        detailControls[index].Focus();
                        return false;
                    }

                    break;

                case (int)EIndex.StaffCD:
                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    Staff_BL bl = new Staff_BL();
                    bool ret = bl.M_Staff_Select(mse);
                    if (ret)
                    {
                        ScStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScStaff.LabelText = "";
                        detailControls[index].Focus();
                        return false;
                    }
                    break;


                case (int)EIndex.CustomerCD:
                case (int)EIndex.DeliveryCD:
                    short kbn = 0;
                    if (index.Equals((int)EIndex.DeliveryCD))
                        kbn = 1;

                    //[M_Customer_Select]
                    M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    Customer_BL sbl = new Customer_BL();
                    //ret = sbl.M_Customer_Select(mce, 1);
                    ret = sbl.M_Customer_Select(mce);

                    if (ret)
                    {
                        if (mce.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            //顧客情報ALLクリア
                            ClearCustomerInfo(kbn);
                            detailControls[index].Focus();
                            return false;
                        }

                        if (kbn.Equals(0))
                        {
                            //顧客CDが変更されている場合のみ再セット
                            if (mOldCustomerCD != detailControls[index].Text || ChangeDate)
                            {
                                //住所情報セット
                                addInfo.ade.VariousFLG = mce.VariousFLG;
                                addInfo.ade.ZipCD1 = mce.ZipCD1;
                                addInfo.ade.ZipCD2 = mce.ZipCD2;
                                addInfo.ade.Address1 = mce.Address1;
                                addInfo.ade.Address2 = mce.Address2;
                                addInfo.ade.Tel11 = mce.Tel11;
                                addInfo.ade.Tel12 = mce.Tel12;
                                addInfo.ade.Tel13 = mce.Tel13;

                                detailControls[index + 5].Text = mce.Tel11;
                                detailControls[index + 6].Text = mce.Tel12;
                                detailControls[index + 7].Text = mce.Tel13;

                                detailControls[index + 1].Text = mce.CustomerName;
                                textBox1.Text = mce.RemarksInStore;
                                textBox2.Text = mce.RemarksOutStore;
                                CboPaymentMethodCD.SelectedValue = mce.PaymentMethodCD;

                                if (mce.VariousFLG == "1")
                                {
                                    if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                    {
                                        detailControls[index + 1].Enabled = true;
                                        detailControls[index + 3].Enabled = true;
                                    }
                                    detailControls[index + 3].Text = "";
                                    lblLastSalesDate.Text = "";
                                    lblStoreName.Text = "";
                                    lblPoint.Text = "";
                                }
                                else
                                {
                                    detailControls[index + 1].Enabled = false;
                                    detailControls[index + 3].Text = "";
                                    detailControls[index + 3].Enabled = false;
                                    lblLastSalesDate.Text = mce.LastSalesDate;
                                    lblPoint.Text = bbl.Z_SetStr(mce.LastPoint);

                                    //[M_Store_Select]
                                    M_Store_Entity mste = new M_Store_Entity
                                    {
                                        StoreCD = mce.LastSalesStoreCD,
                                        ChangeDate = mce.LastSalesDate
                                    };
                                    Store_BL stbl = new Store_BL();
                                    DataTable sdt = stbl.M_Store_Select(mste);
                                    if (sdt.Rows.Count > 0)
                                    {
                                        lblStoreName.Text = sdt.Rows[0]["StoreName"].ToString();
                                    }
                                    else
                                    {
                                        lblStoreName.Text = "";
                                    }
                                }
                                //敬称
                                if (mce.AliasKBN == "1")
                                {
                                    radioSama.Checked = true;
                                }
                                else
                                {
                                    radioOnchu.Checked = true;
                                }
                                mPaymentMethodCD = mce.PaymentMethodCD;
                                mTaxFractionKBN = Convert.ToInt16(mce.TaxFractionKBN);
                                mTaxTiming = Convert.ToInt16(mce.TaxTiming);

                                //単価設定
                                //[M_TankaCD]
                                M_TankaCD_Entity mte = new M_TankaCD_Entity
                                {
                                    TankaCD = mce.TankaCD,
                                    ChangeDate = ymd
                                };
                                TankaCD_BL mbl = new TankaCD_BL();
                                DataTable dt = mbl.M_TankaCD_Select(mte);
                                if (dt.Rows.Count > 0)
                                {
                                    lblTankaName.Text = dt.Rows[0]["TankaName"].ToString();
                                }
                                else
                                {
                                    lblTankaName.Text = "";
                                }


                                if (string.IsNullOrWhiteSpace(ScDeliveryCD.TxtCode.Text))
                                {
                                    ScDeliveryCD.TxtCode.Text = ScCustomer.TxtCode.Text;
                                    CheckDetail((int)EIndex.DeliveryCD);
                                    detailControls[(int)EIndex.DeliveryName].Text = detailControls[(int)EIndex.CustomerName].Text;
                                    detailControls[(int)EIndex.DeliveryName2].Text = detailControls[(int)EIndex.CustomerName2].Text;
                                    if (radioSama.Checked)
                                        radioSamaD.Checked = true;
                                    else
                                        radioOnchuD.Checked = true;
                                    detailControls[(int)EIndex.Tel1D].Text = detailControls[(int)EIndex.Tel1].Text;
                                    detailControls[(int)EIndex.Tel2D].Text = detailControls[(int)EIndex.Tel2].Text;
                                    detailControls[(int)EIndex.Tel3D].Text = detailControls[(int)EIndex.Tel3].Text;

                                    addInfo.adeD.ZipCD1 = addInfo.ade.ZipCD1;
                                    addInfo.adeD.ZipCD2 = addInfo.ade.ZipCD2;
                                    addInfo.adeD.Address1 = addInfo.ade.Address1;
                                    addInfo.adeD.Address2 = addInfo.ade.Address2;
                                }
                            }
                        }
                        else
                        {
                            //配送先CDが変更されている場合のみ再セット
                            if (mOldDeliveryCD != detailControls[index].Text || ChangeDate)
                            {
                                //住所情報セット
                                addInfo.adeD.VariousFLG = mce.VariousFLG;
                                addInfo.adeD.ZipCD1 = mce.ZipCD1;
                                addInfo.adeD.ZipCD2 = mce.ZipCD2;
                                addInfo.adeD.Address1 = mce.Address1;
                                addInfo.adeD.Address2 = mce.Address2;
                                addInfo.adeD.Tel11 = mce.Tel11;
                                addInfo.adeD.Tel12 = mce.Tel12;
                                addInfo.adeD.Tel13 = mce.Tel13;

                                detailControls[index + 5].Text = mce.Tel11;
                                detailControls[index + 6].Text = mce.Tel12;
                                detailControls[index + 7].Text = mce.Tel13;

                                if (mce.VariousFLG == "1")
                                {
                                    detailControls[index + 1].Text = mce.CustomerName;
                                    if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                    {
                                        detailControls[index + 1].Enabled = true;
                                        detailControls[index + 3].Enabled = true;
                                    }
                                    detailControls[index + 3].Text = "";                                    
                                }
                                else
                                {
                                    detailControls[index + 1].Text = mce.CustomerName;
                                    detailControls[index + 1].Enabled = false;
                                    detailControls[index + 3].Text = "";
                                    detailControls[index + 3].Enabled = false;
                                }
                                //敬称
                                if (mce.AliasKBN == "1")
                                {
                                    radioSamaD.Checked = true;
                                }
                                else
                                {
                                    radioOnchuD.Checked = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(kbn);
                        detailControls[index].Focus();
                        return false;
                    }

                    if (kbn.Equals(0))
                        mOldCustomerCD = detailControls[index].Text;    //位置確認
                    else
                        mOldDeliveryCD = detailControls[index].Text;
                    break;
            }

            return true;
        }
        private bool CheckGrid(int col, int row, bool chkAll=false, bool changeYmd=false)
        {
            bool ret = false;

            string ymd = detailControls[(int)EIndex.JuchuuDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridJuchuu.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                        return false;
                }
            }

            switch (col)
            {
                case (int)ClsGridJuchuu.ColNO.JanCD:
                    //販売単価 複写元受注番号が入力されている場合は、以下のメッセージを表示後、その回答によって扱いを変える
                    if (!string.IsNullOrWhiteSpace(keyControls[(int)EIndex.CopyJuchuuNO].Text))
                    {
                        //基本は再計算しない
                        mGrid.g_DArray[row].NotReCalc = true;

                        if (!chkAll && mGrid.g_DArray[row].copyJuchuGyoNO > 0)
                        {
                            //「複写する受注の金額情報をコピーしますか？」
                            if (bbl.ShowMessage("Q318") != DialogResult.Yes)
                            {
                                //単価再計算するように
                                mGrid.g_DArray[row].NotReCalc = false;
                                mGrid.g_DArray[row].OldJanCD = "";
                            }
                        }
                        if (changeYmd)
                            //単価再計算するように
                            mGrid.g_DArray[row].NotReCalc = false;
                    }

                    if (!changeYmd)
                    {
                        if (mGrid.g_DArray[row].JanCD == mGrid.g_DArray[row].OldJanCD)      //chkAll &&  change
                            return true;
                    }

                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].JanCD))
                    {
                        //入力が無い場合(If there is no input)その明細で、JANCDを除く他の項目は全て入力不可とする
                        Grid_Gyo_Clr(row);
                        return true;
                    }
                    //入力がある場合、SKUマスターに存在すること
                    //[M_SKU]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        JanCD = mGrid.g_DArray[row].JanCD,
                        ChangeDate = ymd
                    };

                    if (mGrid.g_DArray[row].JanCD == mGrid.g_DArray[row].OldJanCD || string.IsNullOrWhiteSpace(mGrid.g_DArray[row].OldJanCD))
                    {
                        mse.SKUCD = mGrid.g_DArray[row].SKUCD;
                        mse.AdminNO = mGrid.g_DArray[row].AdminNO;
                    }

                    if (mGrid.g_DArray[row].JanCD != mGrid.g_DArray[row].OldJanCD)
                    {
                        //JANCD変更時は単価再計算するように
                        mGrid.g_DArray[row].NotReCalc = false;
                    }

                    SKU_BL mbl = new SKU_BL();
                    DataTable dt = mbl.M_SKU_SelectAll(mse);
                    DataRow selectRow = null;

                    if (dt.Rows.Count == 0)
                    {
                        //Ｅ１０１
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        selectRow = dt.Rows[0];
                    }
                    else {
                        //JANCDでSKUCDが複数存在する場合（If there is more than one）
                        using (Select_SKU frmSKU = new Select_SKU())
                        {
                            frmSKU.parJANCD = dt.Rows[0]["JanCD"].ToString();
                            frmSKU.parChangeDate = ymd;
                            frmSKU.ShowDialog();

                            if (!frmSKU.flgCancel)
                            {
                                selectRow = dt.Select(" AdminNO = " + frmSKU.parAdminNO)[0];
                            }
                        }

                    }

                    if (selectRow != null)
                    {
                        //JANCDでSKUCDが１つだけ存在する場合（If there is only one）
                        mGrid.g_DArray[row].AdminNO = selectRow["AdminNO"].ToString();
                        mGrid.g_DArray[row].SKUCD = selectRow["SKUCD"].ToString();
                        mGrid.g_DArray[row].SKUName = selectRow["SKUName"].ToString();
                        mGrid.g_DArray[row].SetKBN = selectRow["SetKBN"].ToString() == "1" ? "〇" : "";
                        mGrid.g_DArray[row].ColorName = selectRow["ColorName"].ToString();
                        mGrid.g_DArray[row].SizeName = selectRow["SizeName"].ToString();
                        mGrid.g_DArray[row].DiscountKbn = Convert.ToInt16(selectRow["DiscountKbn"].ToString());
                        mGrid.g_DArray[row].TaniCD = selectRow["TaniCD"].ToString();
                        mGrid.g_DArray[row].TaniName = selectRow["TaniName"].ToString();
                        mGrid.g_DArray[row].TaxRateFLG = Convert.ToInt16(selectRow["TaxRateFLG"].ToString());

                        mGrid.g_DArray[row].AdminNO = selectRow["AdminNO"].ToString();
                        mGrid.g_DArray[row].VariousFLG = Convert.ToInt16(selectRow["VariousFLG"].ToString());

                        mGrid.g_DArray[row].VendorCD = selectRow["MainVendorCD"].ToString();

                        mGrid.g_DArray[row].ZaikoKBN = Convert.ToInt16(selectRow["ZaikoKBN"].ToString());
                        mGrid.g_DArray[row].KariHikiateNO = "";
                        mGrid.g_DArray[row].MakerItem = selectRow["MakerItem"].ToString();
                        mGrid.g_DArray[row].ChkTyokuso = selectRow["DirectFlg"].ToString() == "1" ? true : false;

                        //[M_Vender]    
                        M_Vendor_Entity mve = new M_Vendor_Entity
                        {
                            VendorCD = mGrid.g_DArray[row].VendorCD,
                            ChangeDate = ymd,
                            DeleteFlg = "0"
                        };
                        Vendor_BL vbl = new Vendor_BL();

                        ret = vbl.M_Vendor_SelectTop1(mve);
                        if (ret)
                            mGrid.g_DArray[row].VendorName = mve.VendorName;

                        decimal wSuu = bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu);

                        if (mGrid.g_DArray[row].NotReCalc != true)
                        {
                            //Function_単価取得.
                            Fnc_UnitPrice_Entity fue = new Fnc_UnitPrice_Entity
                            {
                                AdminNo = mGrid.g_DArray[row].AdminNO,
                                ChangeDate = ymd,
                                CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                                StoreCD = CboStoreCD.SelectedIndex > 0 ? CboStoreCD.SelectedValue.ToString() : "",
                                SaleKbn = "0",
                                Suryo = wSuu.ToString()
                            };

                            ret = bbl.Fnc_UnitPrice(fue);
                            if (ret)
                            {
                                switch (mTennic)
                                {
                                    case 0:
                                        //販売単価=Function_単価取得.out税込単価		
                                        mGrid.g_DArray[row].JuchuuUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeikomiTanka));
                                        break;
                                    case 1:
                                        //販売単価=Function_単価取得.out税抜単価
                                        mGrid.g_DArray[row].JuchuuUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeinukiTanka));
                                        break;
                                }

                                //原価単価=Function_単価取得.out原価単価	
                                mGrid.g_DArray[row].CostUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka));
                                mGrid.g_DArray[row].GetCostUnitPrice = bbl.Z_Set(fue.GenkaTanka);
                            }
                            else
                            {
                                //販売単価		
                                mGrid.g_DArray[row].JuchuuUnitPrice = "0";
                                //原価単価
                                mGrid.g_DArray[row].CostUnitPrice = "0";
                                mGrid.g_DArray[row].GetCostUnitPrice = 0;
                            }

                            //	(Form.受注数＝Null	の場合は×１とする)
                            if (wSuu.Equals(0))
                                wSuu = 1;

                            SetJuchuuGaku(row, wSuu, ymd, fue.ZeinukiTanka);
                            
                            //原価額=Form.受注数≠Nullの場合Function_単価取得.out原価単価×Form.Detail.受注数
                            mGrid.g_DArray[row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].CostUnitPrice) * wSuu);

                            //粗利額=⑦Form.税抜販売額－⑩Form.原価額				
                            mGrid.g_DArray[row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].JuchuuHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[row].CostGaku));

                            ////税率=Function_単価取得.out税率
                            //if (bbl.Z_Set(fue.Zeiritsu) == 0)
                            //{
                            //    mGrid.g_DArray[row].TaxRate = "";
                            //}
                            //else
                            //{
                            //    mGrid.g_DArray[row].TaxRate = string.Format("{0:#,##0}", bbl.Z_Set(fue.Zeiritsu)) + "%";
                            //}

                            CalcZei(row);
                        }

                        M_Site_Entity msie = new M_Site_Entity
                        {
                            AdminNO = mGrid.g_DArray[row].AdminNO
                        };
                        Site_BL msbl = new Site_BL();
                        ret = msbl.M_Site_Select(msie);
                        if (ret)
                            mGrid.g_DArray[row].Site = msie.SiteURL;
                        else
                        {
                            msie.ItemSKUCD = mse.ITemCD;
                            ret = msbl.M_Site_Select(msie);
                            if (ret)
                                mGrid.g_DArray[row].Site = msie.SiteURL;
                            else
                                mGrid.g_DArray[row].Site = "";
                        }

                        mGrid.g_DArray[row].OldJanCD = mGrid.g_DArray[row].JanCD;

                        //明細の出荷倉庫CDに、Header部の出荷倉庫CD、倉庫名をセットする。
                        mGrid.g_DArray[row].SoukoName = CboSoukoName.SelectedIndex > 0 ? CboSoukoName.SelectedValue.ToString() : "";

                        CheckHikiate(row, ymd);

                        //仕入先、店舗との組み合わせでITEM発注単価マスターかJAN発注単価マスタに存在すること SelectできなければError
                        //以下①②③④の順番でターゲットが大きくなる（①に近いほど、商品が特定されていく）
                        mGrid.g_DArray[row].OrderUnitPrice = GetTanka(row, ymd);
                        //mGrid.g_DArray[row].OrderGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].OrderUnitPrice) * bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu));

                        //０で無いかつ原価単価＝０の場合、入力された発注単価を原価単価にセットし、原価金額、粗利金額を再計算。
                        if (bbl.Z_Set(mGrid.g_DArray[row].OrderUnitPrice) != 0 && bbl.Z_Set(mGrid.g_DArray[row].CostUnitPrice) == 0)
                        {
                            mGrid.g_DArray[row].CostUnitPrice = mGrid.g_DArray[row].OrderUnitPrice;
                            mGrid.g_DArray[row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].CostUnitPrice) * wSuu);
                        }

                        Grid_NotFocus(col, row);

                    }

                    break;

                case (int)ClsGridJuchuu.ColNO.SKUName:
                    if (mGrid.g_MK_State[col, row].Cell_Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SKUName))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    break;

                case (int)ClsGridJuchuu.ColNO.SoukoName:
                    if (mGrid.g_MK_State[col, row].Cell_Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SoukoName))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    M_Souko_Entity msoe = new M_Souko_Entity
                    {
                        SoukoCD = mGrid.g_DArray[row].SoukoName,
                        ChangeDate = ymd,
                        DeleteFlg = "0"
                    };

                    DataTable msdt = mibl.M_Souko_IsExists(msoe);
                    if (msdt.Rows.Count > 0)
                        if (!base.CheckAvailableStores(msdt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E145");
                            return false;
                        }

                    ret = CheckHikiate(row, ymd);

                    break;

                case (int)ClsGridJuchuu.ColNO.VendorCD:
                    mGrid.g_DArray[row].VendorName = "";

                    if (mGrid.g_MK_State[col, row].Cell_Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].VendorCD))
                        {
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].Nyuka) && mGrid.g_DArray[row].Hikiate != "引当OK")
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[row].VendorCD))
                    {
                        M_Vendor_Entity me = new M_Vendor_Entity
                        {
                            VendorCD = mGrid.g_DArray[row].VendorCD,
                            ChangeDate = ymd,
                            DeleteFlg = "0"
                        };
                        Vendor_BL vebl = new Vendor_BL();

                        ret = vebl.M_Vendor_SelectTop1(me);
                        if (ret)
                        {
                            mGrid.g_DArray[row].VendorName = me.VendorName;
                            mOrderTaxTiming = Convert.ToInt16(me.TaxTiming);
                        }
                        else
                        {
                            //Ｅ１０１  
                            bbl.ShowMessage("E101");
                            return false;
                        }
                    }
                    break;

                case (int)ClsGridJuchuu.ColNO.ChkTyokuso:
                    bool Check = mGrid.g_DArray[row].ChkTyokuso;
                    if (Check)
                    {
                        mGrid.g_DArray[row].ArrivePlanDate = "";
                        mGrid.g_DArray[row].KariHikiateNO = "";
                    }
                    break;

                case (int)ClsGridJuchuu.ColNO.ShippingPlanDate:
                    //入力できる場合(When input is possible)
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ShippingPlanDate))
                    {
                    }
                    else
                    {
                        mGrid.g_DArray[row].ShippingPlanDate = bbl.FormatDate(mGrid.g_DArray[row].ShippingPlanDate);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(mGrid.g_DArray[row].ShippingPlanDate))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }
                        //入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(mGrid.g_DArray[row].ShippingPlanDate))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            return false;
                        }

                        if (mGrid.g_DArray[row].ShippingPlanDate != mGrid.g_DArray[row].OldShippingPlanDate)
                        {
                            //入力された出荷予定日から以下のルールで希望納期を計算する
                            //希望納期=出荷予定日の前日
                            mGrid.g_DArray[row].DesiredDeliveryDate = mibl.GetNouki(mGrid.g_DArray[row].ShippingPlanDate, CboStoreCD.SelectedValue.ToString());

                            mGrid.g_DArray[row].OldShippingPlanDate = mGrid.g_DArray[row].ShippingPlanDate;
                        }
                    }
                    break;
                case (int)ClsGridJuchuu.ColNO.ArrivePlanDate:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ArrivePlanDate))
                    {
                    }
                    else
                    {
                        mGrid.g_DArray[row].ArrivePlanDate = bbl.FormatDate(mGrid.g_DArray[row].ArrivePlanDate);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(mGrid.g_DArray[row].ArrivePlanDate))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }
                        //入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(mGrid.g_DArray[row].ArrivePlanDate))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            return false;
                        }

                    }
                    break;

                case (int)ClsGridJuchuu.ColNO.OrderUnitPrice://発注単価
                    //入力無くても良い(It is not necessary to input)
                    //入力無い場合、0とする（When there is no input, it is set to 0）
                    decimal orderUnitPrice = bbl.Z_Set(mGrid.g_DArray[row].OrderUnitPrice);
                    mGrid.g_DArray[row].OrderUnitPrice = bbl.Z_SetStr(orderUnitPrice);

                    //０の場合				メッセージ表示
                    if (orderUnitPrice.Equals(0))
                    {
                        if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                            return false;
                    }
                    //０で無いかつ原価単価＝０の場合場合、入力された発注単価を原価単価にセットし、原価金額、粗利金額を再計算。
                    else if (bbl.Z_Set(mGrid.g_DArray[row].CostUnitPrice) == 0 || mGrid.g_DArray[row].GetCostUnitPrice == 0)//!chkAll && 
                    {
                        mGrid.g_DArray[row].CostUnitPrice = mGrid.g_DArray[row].OrderUnitPrice;
                        mGrid.g_DArray[row].CostGaku = string.Format("{0:#,##0}", orderUnitPrice * bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu));
                    }
                    //mGrid.g_DArray[row].OrderGaku = bbl.Z_SetStr(orderUnitPrice * bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu));
                    break;

                //case (int)ClsGridJuchuu.ColNO.OrderGaku://発注
                //    //入力無くても良い(It is not necessary to input)
                //    //入力無い場合、0とする（When there is no input, it is set to 0）０を許す。
                //    mGrid.g_DArray[row].OrderGaku = bbl.Z_SetStr(mGrid.g_DArray[row].OrderGaku);

                //    break;

            }

            switch (col)
            {
                case (int)ClsGridJuchuu.ColNO.JuchuuSuu:
                    if (mGrid.g_DArray[row].NotPrintFLG)
                    {
                        if (mGrid.g_DArray[row].JuchuuSuu != mGrid.g_DArray[row - 1].JuchuuSuu)
                        {
                            //Ｅ２１７				
                            bbl.ShowMessage("E217");
                            return false;
                        }
                    }
                    if (mGrid.g_DArray[row].DiscountKbn.Equals(1))
                    {
                        //見積数≧０の場合、Error				
                        if (bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu) >= 0)
                        {
                            //Ｅ２４１				
                            bbl.ShowMessage("E241");
                            return false;
                        }
                    }
                    //各金額項目の再計算必要
                    if (chkAll == false)
                        CalcKin();

                    break;

                case (int)ClsGridJuchuu.ColNO.JanCD:                
                case (int)ClsGridJuchuu.ColNO.JuchuuUnitPrice: //販売単価 
                case (int)ClsGridJuchuu.ColNO.CostUnitPrice: //原価単価
                case (int)ClsGridJuchuu.ColNO.OrderUnitPrice://発注単価

                    //各金額項目の再計算必要
                    if (chkAll == false)
                        CalcKin();

                        break;

            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        /// <summary>
        /// 引当可能かCheckする【引当可能確認ロジック】
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool CheckHikiate(int row, string ymd)
        {
            bool ret = false;

            //在庫区分=1→画面明細.直送checkbox＝offの場合
            //if (mGrid.g_DArray[row].ZaikoKBN == 1)
            if (mGrid.g_DArray[row].ChkTyokuso == false)
            {              
                //Function_商品引当.
                Fnc_Reserve_Entity fre = new Fnc_Reserve_Entity
                {
                    AdminNO = mGrid.g_DArray[row].AdminNO,
                    ChangeDate = ymd,
                    StoreCD = CboStoreCD.SelectedIndex > 0 ? CboStoreCD.SelectedValue.ToString() : "",
                    SoukoCD = mGrid.g_DArray[row].SoukoName,  
                    Suryo = bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu).ToString(),
                    DenType = "1",  //1(受注)
                    DenNo = mGrid.g_DArray[row].JuchuuNO,  // keyControls[(int)EIndex.JuchuuNO].Text,
                    DenGyoNo = mGrid.g_DArray[row].juchuGyoNO.ToString(),
                    KariHikiateNo = mGrid.g_DArray[row].KariHikiateNO
                };

                //Form.受注番号＝Nullの場合
                if(mTemporaryReserveNO=="")
                {
                    mTemporaryReserveNO = mibl.GetTemporaryReserveNO(fre.DenNo);

                    if (mTennic.Equals(1) && !string.IsNullOrWhiteSpace(fre.DenNo))
                        mTemporaryReserveNO = "";
                }

                if (string.IsNullOrWhiteSpace(fre.DenNo))
                {
                    fre.DenNo = mTemporaryReserveNO;
                }
                if (fre.DenGyoNo == "0")
                {
                    //if (mTennic.Equals(1))
                    //    fre.DenGyoNo = "1";
                    //else
                        fre.DenGyoNo = (row + 1).ToString();
                }

                ret = bbl.Fnc_Reserve(fre);
                if(ret)
                {
                    int result;

                    if (!string.IsNullOrWhiteSpace(fre.LastDay))
                    {
                        //ただし、			out引当最終日 ≦ Today の場合、Null		
                        result = fre.LastDay.CompareTo(bbl.GetDate());
                        if (result <= 0)
                        {
                            mGrid.g_DArray[row].ArrivePlanDate = "";
                        }
                        else
                        {
                            mGrid.g_DArray[row].ArrivePlanDate = fre.LastDay;
                        }
                    }
                    if (fre.Result == "1")
                    {
                        mGrid.g_DArray[row].Hikiate = "引当OK";

                        if (mGrid.g_DArray[row].ZaikoKBN == 1)
                        {
                            mGrid.g_DArray[row].VendorCD = "";
                            mGrid.g_DArray[row].VendorName = "";
                        }

                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ArrivePlanDate))
                        {
                            if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.SalesDate].Text))
                                detailControls[(int)EIndex.SalesDate].Text = bbl.GetDate();
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.SalesDate].Text))
                            {
                                result = detailControls[(int)EIndex.SalesDate].Text.CompareTo(mGrid.g_DArray[row].ArrivePlanDate);
                                if (result < 0)
                                {
                                    detailControls[(int)EIndex.SalesDate].Text=mGrid.g_DArray[row].ArrivePlanDate;
                                }
                            }
                        }
                    }
                    else if (fre.Result == "2")
                    {
                        mGrid.g_DArray[row].Hikiate = "一部OK";
                        detailControls[(int)EIndex.SalesDate].Text = "";
                    }
                    else
                    {
                        mGrid.g_DArray[row].Hikiate = "引当NG";
                        detailControls[(int)EIndex.SalesDate].Text = "";
                    }
                    mGrid.g_DArray[row].KariHikiateNO = fre.OutKariHikiateNo;
                }
            }

            return ret;
        }
        private string GetTanka(int row, string ymd)
        {
            //[M_JANOrderPrice]
            M_JANOrderPrice_Entity mje = new M_JANOrderPrice_Entity
            {

                //①JAN発注単価マスタ（店舗指定なし）
                AdminNO = mGrid.g_DArray[row].AdminNO,
                VendorCD = mGrid.g_DArray[row].VendorCD,
                StoreCD = CboStoreCD.SelectedIndex > 0 ? CboStoreCD.SelectedValue.ToString() :"",
                ChangeDate = ymd
            };

            JANOrderPrice_BL jbl = new JANOrderPrice_BL();
            bool ret = jbl.M_JANOrderPrice_Select(mje);
            if (ret)
            {
                return bbl.Z_SetStr(mje.PriceWithoutTax);
            }
            else
            {
                //②	JAN発注単価マスタ（店舗指定あり）
                mje.StoreCD = "0000";

                ret = jbl.M_JANOrderPrice_Select(mje);
                if (ret)
                {
                    return bbl.Z_SetStr(mje.PriceWithoutTax);
                }
                else
                {
                    //[M_ItemOrderPrice]
                    M_ItemOrderPrice_Entity mje2 = new M_ItemOrderPrice_Entity
                    {

                        //③	ITEM発注単価マスター（店舗指定あり）	
                        MakerItem = mGrid.g_DArray[row].MakerItem,
                        VendorCD = mGrid.g_DArray[row].VendorCD,
                        ChangeDate = ymd,
                        StoreCD = CboStoreCD.SelectedIndex >0 ? CboStoreCD.SelectedValue.ToString():""
                    };

                    ItemOrderPrice_BL ibl = new ItemOrderPrice_BL();
                    ret = ibl.M_ItemOrderPrice_Select(mje2);
                    if (ret)
                    {
                        return bbl.Z_SetStr(mje2.PriceWithoutTax);
                    }
                    else
                    {
                        //④	ITEM発注単価マスター（店舗指定なし）
                        mje2.StoreCD = "0000";
                        ret = ibl.M_ItemOrderPrice_Select(mje2);
                        if (ret)
                        {
                            return bbl.Z_SetStr(mje2.PriceWithoutTax);
                        }
                        else
                        {
                            //mGrid.g_DArray[row].OldJanCD = "";

                            //bbl.ShowMessage("E170");
                            return "0";
                        }
                    }
                }
            }
        }
    
        private void SetJuchuuGaku(int row, decimal wSuu, string ymd, string fZeinukiTanka)
        {

            //税抜販売額
            switch (mTennic)
            {
                case 0:
                    {
                        //税抜販売額
                        if (mGrid.g_DArray[row].VariousFLG.Equals(1))
                        {
                            //（諸口の場合は入力された税込単価から税抜単価を都度計算する）
                            //税抜販売額＝Function_消費税計算.out金額１×Form.Detail.受注数
                            decimal tanka = bbl.GetZeinukiKingaku(bbl.Z_Set(mGrid.g_DArray[row].JuchuuUnitPrice), mGrid.g_DArray[row].TaxRateFLG, ymd);

                            mGrid.g_DArray[row].JuchuuHontaiGaku = string.Format("{0:#,##0}", tanka * wSuu);
                        }
                        else if (mGrid.g_DArray[row].VariousFLG.Equals(0))
                        {
                            //税抜販売額＝Form.受注数＝Nullの場合Function_単価取得.out税抜単価×Form.受注数
                            mGrid.g_DArray[row].JuchuuHontaiGaku = string.Format("{0:#,##0}", bbl.Z_Set(fZeinukiTanka) * wSuu);
                        }
                        //税込販売額=Form.受注数≠Nullの場合Function_単価取得.out税込単価×Form.Detail.受注数	
                        mGrid.g_DArray[row].JuchuuGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].JuchuuUnitPrice) * wSuu);
                    }
                    break;
                case 1:
                    {
                        //税抜販売額
                        if (mGrid.g_DArray[row].VariousFLG.Equals(1))
                        {
                            //税抜販売額←	Form.Detail.販売単価×	Form.Detail.受注数	Form.受注数＝Nullの場合は×１とする		
                            mGrid.g_DArray[row].JuchuuHontaiGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].JuchuuUnitPrice) * wSuu);
                        }
                        else if (mGrid.g_DArray[row].VariousFLG.Equals(0))
                        {
                            //税抜販売額←Function_単価取得.out税抜単価×Form.Detail.受注数		Form.受注数＝Nullの場合は×１とする	
                            mGrid.g_DArray[row].JuchuuHontaiGaku = string.Format("{0:#,##0}", bbl.Z_Set(fZeinukiTanka) * wSuu);
                        }
                        //税込販売額=Function_消費税計算.out金額１                                  
                        mGrid.g_DArray[row].JuchuuGaku = string.Format("{0:#,##0}", bbl.GetZeikomiKingaku(bbl.Z_Set(mGrid.g_DArray[row].JuchuuHontaiGaku), mGrid.g_DArray[row].TaxRateFLG, out decimal zei, ymd));
                    }
                    break;
            }
        }

        private void CalcZei(int w_Row, bool changeTanka = false)
        {
            string ymd = detailControls[(int)EIndex.JuchuuDate].Text;
            decimal wSuu = bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuSuu);
            decimal wTanka = bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuUnitPrice);

            //paremetersin計算モード←2
            //in基準日←Form.見積日
            //in軽減税率FLG←⑫TaxRateFLG
            //in金額←Form.Detail.販売単価
            //Function_単価取得.
            Fnc_UnitPrice_Entity fue = new Fnc_UnitPrice_Entity
            {
                AdminNo = mGrid.g_DArray[w_Row].AdminNO,
                ChangeDate = ymd,
                CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                StoreCD = CboStoreCD.SelectedIndex > 0 ? CboStoreCD.SelectedValue.ToString() :"",
                SaleKbn = "0",
                Suryo = wSuu.ToString()
            };

            bool ret = bbl.Fnc_UnitPrice(fue);

            //税率=Function_単価取得.out税率
            if (bbl.Z_Set(fue.Zeiritsu) == 0)
            {
                mGrid.g_DArray[w_Row].TaxRate = "";
            }
            else
            {
                mGrid.g_DArray[w_Row].TaxRate = string.Format("{0:#,##0}", bbl.Z_Set(fue.Zeiritsu)) + "%";
            }

            //粗利額←⑦Form.Detail.税抜販売額－⑩Form.Detail.原価額
            mGrid.g_DArray[w_Row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].CostGaku));

            if (mGrid.g_DArray[w_Row].TaxRateFLG == 0)
            {
                mGrid.g_DArray[w_Row].TaxRateDisp = "非税";
                mGrid.g_DArray[w_Row].JuchuTax = 0;
                mGrid.g_DArray[w_Row].KeigenTax = 0;
            }
            else
            {
                if (mTennic.Equals(1))
                {
                    mGrid.g_DArray[w_Row].TaxRateDisp = "税抜";
                }
                else
                {
                    mGrid.g_DArray[w_Row].TaxRateDisp = "税込";
                }

                if (mGrid.g_DArray[w_Row].TaxRateFLG == 1)
                {
                    if (mGrid.g_DArray[w_Row].VariousFLG.Equals(1) || changeTanka)
                    {
                        //通常税額=税込販売額－税抜販売額
                        mGrid.g_DArray[w_Row].JuchuTax = bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuHontaiGaku);
                    }
                    else if (mGrid.g_DArray[w_Row].VariousFLG.Equals(0))
                    {
                        //通常税額←TaxRateFLG＝1の時のFunction_単価取得.out消費税額×Form.Detail.受注数
                        mGrid.g_DArray[w_Row].JuchuTax = bbl.Z_Set(fue.Zei) * wSuu;
                    }

                    mGrid.g_DArray[w_Row].KeigenTax = 0;
                }
                else if (mGrid.g_DArray[w_Row].TaxRateFLG == 2)
                {
                    mGrid.g_DArray[w_Row].JuchuTax = 0;

                    if (mGrid.g_DArray[w_Row].VariousFLG.Equals(1) || changeTanka)
                    {
                        //軽減税額=TaxRateFLG＝2の時の税込販売額－税抜販売額
                        mGrid.g_DArray[w_Row].KeigenTax = bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuHontaiGaku);
                    }
                    else if (mGrid.g_DArray[w_Row].VariousFLG.Equals(0))
                    {
                        //軽減税額←TaxRateFLG＝2の時のFunction_単価取得.out消費税額×Form.Detail.見積数
                        mGrid.g_DArray[w_Row].KeigenTax = bbl.Z_Set(fue.Zei) * wSuu;
                    }
                }
            }
        }
        private void CalcKin()
        {
            decimal kin1 = 0;
            decimal kin2 = 0;
            decimal kin3 = 0;
            decimal kin4 = 0;
            decimal kin5 = 0;
            decimal zei10 = 0;
            decimal zei8 = 0;

            //M_Vendor.TaxTiming＝2:伝票ごと
            decimal kin10 = 0;
            decimal kin8 = 0;
            int zeiritsu10 = 0;
            int zeiritsu8 = 0;
            int maxKinRowNo = 0;
            decimal maxKin = 0;

            decimal kinOrder10 = 0;      //発注通常税額(Hidden)
            decimal kinOrder8 = 0;      //発注軽減税額(Hidden)
            decimal zeiOrder10 = 0;      //発注通常税額(Hidden)
            decimal zeiOrder8 = 0;      //発注軽減税額(Hidden)
            decimal sumZei10 = 0; //Form.Detail.発注通常税額のTotal
            decimal sumOrKin = 0;//Form.Detail.発注額のTotal
            int maxOrderKinRowNo = 0;
            decimal maxOrderKin = 0;

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                {    
                    //原価額←原価単価×Form.Detail.数
                    mGrid.g_DArray[RW].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice) * bbl.Z_Set(mGrid.g_DArray[RW].JuchuuSuu));
                    //粗利額←⑦Form.Detail.税抜販売額－⑩Form.Detail.原価額
                    mGrid.g_DArray[RW].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[RW].CostGaku));

                    if (mGrid.g_DArray[RW].DiscountKbn == 0)
                        kin1 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuuGaku);
                    else
                        kin5 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuuGaku);

                    ////Form.Detail.値引区分＝１ の Form.Detail.税抜販売額  ←Form.Detail.税率＝10%の明細が対象
                    ////－Form.Detail.値引区分＝０ の Form.Detail.税抜販売額  										
                    //if (mGrid.g_DArray[RW].DiscountKbn == 1 && mGrid.g_DArray[RW].TaxRateFLG == 1)
                    //    kin2 -= bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku);
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 0 && mGrid.g_DArray[RW].TaxRateFLG == 1)
                    //    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku);
                    ////＋（Form.Detail.値引区分＝１ の Form.Detail.税抜販売額 ←Form.Detail.税率＝８%の明細が対象
                    ////－	Form.Detail.値引区分＝０ の Form.Detail.税抜販売額  
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 1 && mGrid.g_DArray[RW].TaxRateFLG == 2)
                    //    kin2 -= bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku);
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 0 && mGrid.g_DArray[RW].TaxRateFLG == 2)
                    //    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku);
                    ////＋（Form.Detail.値引区分＝１ の Form.Detail.税抜販売額 ←Form.Detail.税率＝それ以外の明細が対象
                    ////－	Form.Detail.値引区分＝０ の Form.Detail.税抜販売額
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 1)
                    //    kin2 -= bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku);
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 0)
                    //    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku);

                    //税抜売上額=Form.Detail.税抜販売額のTotal
                    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku);
                    zei10 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuTax);
                    zei8 += bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax);
                    kin3 += bbl.Z_Set(mGrid.g_DArray[RW].CostGaku);
                    kin4 += bbl.Z_Set(mGrid.g_DArray[RW].ProfitGaku);
                    //sumOrKin += bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku);
                    //zeiOrder10 += bbl.Z_Set(mGrid.g_DArray[RW].OrderTax);
                    //zeiOrder8 += bbl.Z_Set(mGrid.g_DArray[RW].KeigenOrderTax);

                    //M_Vendor.TaxTiming＝2:伝票ごと
                    if (mTaxTiming.Equals(2))
                    {
                        if (mGrid.g_DArray[RW].TaxRateFLG.Equals(1))
                        {
                            kin10 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku);
                            if (zeiritsu10 == 0 && !string.IsNullOrWhiteSpace( mGrid.g_DArray[RW].TaxRate))
                                zeiritsu10 = Convert.ToInt16(mGrid.g_DArray[RW].TaxRate.Replace("%", ""));
                        }
                        else if (mGrid.g_DArray[RW].TaxRateFLG.Equals(2))
                        {
                            kin8 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku);
                            if (zeiritsu8 == 0 && !string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].TaxRate))
                                zeiritsu8 = Convert.ToInt16(mGrid.g_DArray[RW].TaxRate.Replace("%", ""));
                        }

                        if (maxKin < bbl.Z_Set(mGrid.g_DArray[RW].JuchuuGaku) && mGrid.g_DArray[RW].DiscountKbn == 0)
                        {
                            maxKin = bbl.Z_Set(mGrid.g_DArray[RW].JuchuuGaku);
                            maxKinRowNo = RW;
                        }
                    }
                    //if(mOrderTaxTiming.Equals(2))
                    //{
                    //    if (mGrid.g_DArray[RW].TaxRateFLG.Equals(1))
                    //    {
                    //        kinOrder10 += bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku);
                    //    }
                    //    else if (mGrid.g_DArray[RW].TaxRateFLG.Equals(2))
                    //    {
                    //        kinOrder8 += bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku);
                    //    }
                    //    if (maxOrderKin < bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku))
                    //    {
                    //        maxOrderKin = bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku);
                    //        maxOrderKinRowNo = RW;
                    //    }
                    //}
                }
            }

            //Footer部
            //税込受注額・Form.Detail.税込販売額のTotal
            lblKin1.Text=string.Format("{0:#,##0}", kin1);
            //値引き額
            lblKin5.Text = string.Format("{0:#,##0}", kin5);
            //税込売上額＝税込受注額－値引額
            lblKin6.Text = string.Format("{0:#,##0}", kin1 + kin5);
            //税抜売上額・Form.Detail.税抜販売額のTotal
            lblKin2.Text = string.Format("{0:#,##0}", kin2);
            //原価額・Form.Detail.原価額のTotal
            lblKin3.Text = string.Format("{0:#,##0}", kin3);
            //粗利額・税抜売上額－原価額
            lblKin4.Text = string.Format("{0:#,##0}", kin2- kin3);

            //請求額＝税込売上額－ポイント利用
            lblKin7.Text = string.Format("{0:#,##0}", kin1 + kin5);// - bbl.Z_Set(detailControls[(int)EIndex.Point].Text));

            //M_Control.Tennic＝0 の場合、
            //M_Control.Tennic＝1 の場合、M_Vendor.TaxTiming＝1:明細ごと または 3:締ごと	
            if (mTennic.Equals(0) || (mTennic.Equals(1) && (mTaxTiming.Equals(1) || mTaxTiming.Equals(3))))
            {
                //消費税額
                lblKin10.Text = string.Format("{0:#,##0}", zei10 + zei8);
                //通常税額(Hidden)・Form.Detail.通常税額のTotal
                lblKin11.Text = string.Format("{0:#,##0}", zei10);
                mZei10 = zei10;
                //軽減税額(Hidden)・Form.Detail.軽減税額のTotal
                lblKin12.Text = string.Format("{0:#,##0}", zei8);
                mZei8 = zei8;
                lblKin13.Text = string.Format("{0:#,##0}", sumOrKin);
            }
            //M_Vendor.TaxTiming＝2:伝票ごと
            else
            {
                //通常税額(Hidden)=Form.Detail.税抜販売額のTotal×Form.Detail.税率	※端数処理はM_Customerの設定に準ずる		
                //←M_SKU.TaxRateFLG＝1:通常課税の明細が対象  
                kin10 = GetResultWithHasuKbn(mTaxFractionKBN, kin10 * zeiritsu10 / 100);
                kin8 = GetResultWithHasuKbn(mTaxFractionKBN, kin8 * zeiritsu8 / 100);

                kinOrder10 = GetResultWithHasuKbn(mTaxFractionKBN, kinOrder10 * zeiritsu10 / 100);
                kinOrder8 = GetResultWithHasuKbn(mTaxFractionKBN, kinOrder8 * zeiritsu8 / 100);


                decimal sagaku = (kin10 + kin8) - (zei10 + zei8);
                //通常税額(Hidden) ＋ 軽減税額(Hidden)　≠　SUM（Form.Detail.通常税額＋Form.Detail.軽減税額）の場合、
                //以下の計算結果「消費税差額」を明細金額が一番大きい金額（全明細が同じ金額の場合は１行目）の明細税込販売額、明細消費税額に足し込む。
                if (sagaku != 0)
                {
                    //※消費税差額＝通常税額(Hidden) ＋ 軽減税額(Hidden)）－ SUM（Form.Detail.通常税額 ＋ Form.Detail.軽減税額）	

                    //明細税込販売額に足し込む。
                    mGrid.g_DArray[maxKinRowNo].JuchuuGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[maxKinRowNo].JuchuuGaku) + sagaku);

                    //明細消費税額に足し込む。
                    if (mGrid.g_DArray[maxKinRowNo].TaxRateFLG.Equals(1))
                    {
                        mGrid.g_DArray[maxKinRowNo].JuchuTax = mGrid.g_DArray[maxKinRowNo].JuchuTax + sagaku;
                    }
                    else if (mGrid.g_DArray[maxKinRowNo].TaxRateFLG.Equals(2))
                    {
                        mGrid.g_DArray[maxKinRowNo].KeigenTax = mGrid.g_DArray[maxKinRowNo].KeigenTax + sagaku;
                    }
                }
                //税込受注額・Form.Detail.税込販売額のTotal
                lblKin1.Text = string.Format("{0:#,##0}", kin1 + sagaku);
                //税込売上額＝税込受注額－値引額
                lblKin6.Text = string.Format("{0:#,##0}", kin1 + sagaku + kin5);
                //請求額＝税込売上額＝税込受注額－値引額
                lblKin7.Text = string.Format("{0:#,##0}", kin1 + sagaku + kin5);
                //消費税額
                lblKin10.Text = string.Format("{0:#,##0}", kin10 + kin8);
                //通常税額(Hidden)・Form.Detail.通常税額のTotal
                lblKin11.Text = string.Format("{0:#,##0}", kin10);
                mZei10 = kin10;
                //軽減税額(Hidden)・Form.Detail.軽減税額のTotal
                lblKin12.Text = string.Format("{0:#,##0}", kin8);
                mZei8 = kin8;

                //sagaku = (kinOrder10 + kinOrder8) - (zeiOrder10 + zeiOrder8);
                //if (sagaku != 0)
                //{
                //    //以下の計算結果「発注消費税差額」を明細金額が一番大きい金額（全明細が同じ金額の場合は１行目）の明細発注額、明細発注消費税額に足し込む。
                //    mGrid.g_DArray[maxOrderKinRowNo].OrderGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[maxOrderKinRowNo].OrderGaku) + sagaku);
                    
                //    //明細消費税額に足し込む。
                //    if (mGrid.g_DArray[maxOrderKinRowNo].TaxRateFLG.Equals(1))
                //    {
                //        mGrid.g_DArray[maxOrderKinRowNo].OrderTax = mGrid.g_DArray[maxOrderKinRowNo].OrderTax + sagaku;
                //    }
                //    else if (mGrid.g_DArray[maxOrderKinRowNo].TaxRateFLG.Equals(2))
                //    {
                //        mGrid.g_DArray[maxOrderKinRowNo].KeigenOrderTax = mGrid.g_DArray[maxOrderKinRowNo].KeigenOrderTax + sagaku;
                //    }
                //}
                //lblKin13.Text = string.Format("{0:#,##0}", sumOrKin + sagaku);
            }

            //未入金額＝請求額－入金額
            lblKin9.Text = string.Format("{0:#,##0}", kin1 + kin5 - bbl.Z_Set(lblKin8.Text));


        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Juchuu_Entity GetEntity()
        {
            dje = new D_Juchuu_Entity();
            dje.JuchuuNO = keyControls[(int)EIndex.JuchuuNO].Text;

            if (mTennic.Equals(1))
                dje.JuchuuProcessNO = keyControls[(int)EIndex.JuchuuNO].Text;

            dje.StoreCD = CboStoreCD.SelectedValue.ToString();

            dje.JuchuuDate = detailControls[(int)EIndex.JuchuuDate].Text;

            if(ckM_CheckBox1.Checked)
                dje.ReturnFLG = "1";
            else
                dje.ReturnFLG = "0";

            dje.SoukoCD =CboSoukoName.SelectedValue.ToString();
            dje.StaffCD = detailControls[(int)EIndex.StaffCD].Text;
            dje.CustomerCD = detailControls[(int)EIndex.CustomerCD].Text;
            dje.CustomerName = detailControls[(int)EIndex.CustomerName].Text;
            dje.CustomerName2 = detailControls[(int)EIndex.CustomerName2].Text;
            dje.ZipCD1 = addInfo.ade.ZipCD1;
            dje.ZipCD2 = addInfo.ade.ZipCD2;
            dje.Address1 = addInfo.ade.Address1;
            dje.Address2 = addInfo.ade.Address2;
            dje.Tel11 = detailControls[(int)EIndex.Tel1].Text;
            dje.Tel12 = detailControls[(int)EIndex.Tel2].Text;
            dje.Tel13 = detailControls[(int)EIndex.Tel3].Text;

            if (radioSama.Checked)
                dje.AliasKBN="1";
                    else
                dje.AliasKBN = "2";

            dje.DeliveryCD = detailControls[(int)EIndex.DeliveryCD].Text;
            dje.DeliveryName = detailControls[(int)EIndex.DeliveryName].Text;
            dje.DeliveryName2 = detailControls[(int)EIndex.DeliveryName2].Text;
            dje.DeliveryZipCD1 = addInfo.adeD.ZipCD1;
            dje.DeliveryZipCD2 = addInfo.adeD.ZipCD2;
            dje.DeliveryAddress1 = addInfo.adeD.Address1;
            dje.DeliveryAddress2 = addInfo.adeD.Address2;
            dje.DeliveryTel11 = detailControls[(int)EIndex.Tel1D].Text;
            dje.DeliveryTel12 = detailControls[(int)EIndex.Tel2D].Text;
            dje.DeliveryTel13 = detailControls[(int)EIndex.Tel3D].Text;

            if (radioSamaD.Checked)
                dje.DeliveryAliasKBN = "1";
            else
                dje.DeliveryAliasKBN = "2";

            dje.JuchuuGaku = bbl.Z_SetStr(lblKin1.Text);
            dje.Discount = bbl.Z_SetStr(lblKin5.Text);
            dje.HanbaiHontaiGaku = (bbl.Z_Set(lblKin1.Text)-(mZei8+ mZei10)).ToString();
            dje.HanbaiTax8 = mZei8.ToString();
            dje.HanbaiTax10 = mZei10.ToString();
            dje.HanbaiGaku = (bbl.Z_Set(lblKin1.Text) - bbl.Z_Set(lblKin5.Text)).ToString();
            dje.CostGaku = bbl.Z_SetStr(lblKin3.Text);
            dje.ProfitGaku = bbl.Z_SetStr(lblKin4.Text);
            dje.Point = "0";  // bbl.Z_SetStr(detailControls[(int)EIndex.Point].Text);
            dje.InvoiceGaku = bbl.Z_SetStr(lblKin7.Text);

            dje.PaymentMethodCD = CboPaymentMethodCD.SelectedValue.ToString();
            dje.PaymentPlanNO = "0";
            dje.SalesPlanDate = detailControls[(int)EIndex.SalesDate].Text;
            dje.FirstPaypentPlanDate = detailControls[(int)EIndex.FirstPaypentPlanDate].Text;
            dje.LastPaymentPlanDate = detailControls[(int)EIndex.FirstPaypentPlanDate].Text;
            dje.CommentOutStore = detailControls[(int)EIndex.RemarksOutStore].Text;
            dje.CommentInStore = detailControls[(int)EIndex.RemarksInStore].Text;

            dje.MitsumoriNO = keyControls[(int)EIndex.MitsumoriNO].Text;
            dje.NouhinsyoComment = detailControls[(int)EIndex.NouhinsyoComment].Text;

            dje.InsertOperator = InOperatorCD;
            dje.PC = InPcID;
            //dje.OrderHontaiGaku = (bbl.Z_Set(lblKin13.Text) - (mOrderTax8 + mOrderTax10)).ToString();
            //dje.OrderTax8 = mOrderTax8.ToString();
            //dje.OrderTax10 = mOrderTax10.ToString();
            //dje.OrderGaku = bbl.Z_SetStr(lblKin13.Text);

            return dje;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("JuchuuRows", typeof(int));
            dt.Columns.Add("DisplayRows", typeof(int));
            dt.Columns.Add("SiteJuchuuRows", typeof(int));
            dt.Columns.Add("NotPrintFLG", typeof(int));
            dt.Columns.Add("AddJuchuuRows", typeof(int));
            dt.Columns.Add("NotOrderFLG", typeof(int));
            dt.Columns.Add("DirectFLG", typeof(int));
            dt.Columns.Add("SKUNO", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("SetKBN", typeof(int));
            dt.Columns.Add("JuchuuSuu", typeof(int));
            dt.Columns.Add("JuchuuUnitPrice", typeof(decimal));
            dt.Columns.Add("TaniCD", typeof(string));
            dt.Columns.Add("JuchuuGaku", typeof(decimal));
            dt.Columns.Add("JuchuuHontaiGaku", typeof(decimal));
            dt.Columns.Add("JuchuuTax", typeof(decimal));
            dt.Columns.Add("JuchuuTaxRitsu", typeof(decimal));
            dt.Columns.Add("CostUnitPrice", typeof(decimal));
            dt.Columns.Add("CostGaku", typeof(decimal));
            dt.Columns.Add("ProfitGaku", typeof(decimal));
            dt.Columns.Add("SoukoCD", typeof(string));
            dt.Columns.Add("VendorCD", typeof(string));
            dt.Columns.Add("ArrivePlanDate", typeof(DateTime));
            dt.Columns.Add("CommentOutStore", typeof(string));
            dt.Columns.Add("CommentInStore", typeof(string));
            dt.Columns.Add("IndividualClientName", typeof(string));
            dt.Columns.Add("ZaikoKBN", typeof(int));
            dt.Columns.Add("TemporaryNO", typeof(string));

            dt.Columns.Add("ExpressFLG", typeof(int));
            dt.Columns.Add("ShippingPlanDate", typeof(DateTime));
            dt.Columns.Add("OrderUnitPrice", typeof(decimal));
            dt.Columns.Add("JuchuuNO", typeof(string));
            //dt.Columns.Add("OrderTax", typeof(decimal));
            //dt.Columns.Add("OrderTaxRitsu", typeof(decimal));
            //dt.Columns.Add("OrderGaku", typeof(decimal));
            dt.Columns.Add("DesiredDeliveryDate", typeof(string));

            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            if(OperationMode== EOperationMode.UPDATE)
            {
                rowNo = m_MaxJyuchuGyoNo + 1;
            }

            int AddJuchuuRows = 0;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //zが更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                {
                    string setkbn = "1";
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SetKBN))
                        setkbn = "0";

                    int notPrintFLG = 0;
                    if (mGrid.g_DArray[RW].NotPrintFLG)
                    {
                        notPrintFLG = 1;
                    }
                    else
                    {
                        AddJuchuuRows = mGrid.g_DArray[RW].juchuGyoNO > 0 ? mGrid.g_DArray[RW].juchuGyoNO : rowNo;
                    }

                    dt.Rows.Add(mGrid.g_DArray[RW].juchuGyoNO > 0 ? mGrid.g_DArray[RW].juchuGyoNO : rowNo
                        , mGrid.g_DArray[RW].GYONO
                        , 0
                        , notPrintFLG
                        , notPrintFLG == 1 ? AddJuchuuRows : 0
                        , mGrid.g_DArray[RW].ChkFuyo ? 1 : 0
                        , mGrid.g_DArray[RW].ChkTyokuso ? 1 : 0
                        , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                        , mGrid.g_DArray[RW].SKUCD == "" ? null : mGrid.g_DArray[RW].SKUCD
                        , mGrid.g_DArray[RW].JanCD == "" ? null : mGrid.g_DArray[RW].JanCD
                        , mGrid.g_DArray[RW].SKUName == "" ? null : mGrid.g_DArray[RW].SKUName
                        , mGrid.g_DArray[RW].ColorName == "" ? null : mGrid.g_DArray[RW].ColorName
                        , mGrid.g_DArray[RW].SizeName == "" ? null : mGrid.g_DArray[RW].SizeName
                        , setkbn
                        , bbl.Z_Set(mGrid.g_DArray[RW].JuchuuSuu)
                        , bbl.Z_Set(mGrid.g_DArray[RW].JuchuuUnitPrice)
                        , mGrid.g_DArray[RW].TaniCD == "" ? null : mGrid.g_DArray[RW].TaniCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].JuchuuGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].JuchuuHontaiGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].JuchuTax) + bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].TaxRate.Replace("%", ""))     //税率
                        , bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].CostGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].ProfitGaku)
                        , mGrid.g_DArray[RW].SoukoName == "" ? null : mGrid.g_DArray[RW].SoukoName
                        , mGrid.g_DArray[RW].VendorCD == "" ? null : mGrid.g_DArray[RW].VendorCD
                        , mGrid.g_DArray[RW].ArrivePlanDate == "" ? null : mGrid.g_DArray[RW].ArrivePlanDate
                        , mGrid.g_DArray[RW].CommentOutStore == "" ? null : mGrid.g_DArray[RW].CommentOutStore
                        , mGrid.g_DArray[RW].CommentInStore == "" ? null : mGrid.g_DArray[RW].CommentInStore
                        , mGrid.g_DArray[RW].IndividualClientName == "" ? null : mGrid.g_DArray[RW].IndividualClientName
                        , bbl.Z_Set(mGrid.g_DArray[RW].ZaikoKBN)
                        , mGrid.g_DArray[RW].KariHikiateNO == "" ? null : mGrid.g_DArray[RW].KariHikiateNO
                        , mGrid.g_DArray[RW].ChkExpress ? 1 : 0
                        , mGrid.g_DArray[RW].ShippingPlanDate == "" ? null : mGrid.g_DArray[RW].ShippingPlanDate
                        , bbl.Z_Set(mGrid.g_DArray[RW].OrderUnitPrice)
                        , mGrid.g_DArray[RW].JuchuuNO == "" ? null : mGrid.g_DArray[RW].JuchuuNO
                        //, bbl.Z_Set(mGrid.g_DArray[RW].OrderTax)
                        //, bbl.Z_Set(mGrid.g_DArray[RW].OrderTaxRitsu)
                        //, bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku)
                        , mGrid.g_DArray[RW].DesiredDeliveryDate == "" ? null : mGrid.g_DArray[RW].DesiredDeliveryDate
                        , mGrid.g_DArray[RW].juchuGyoNO > 0 ? 1:0
                        );

                    if(mGrid.g_DArray[RW].juchuGyoNO ==0)
                    rowNo++;
                }
            }

            return dt;
        }
        protected override void ExecSec()
        {

            if (OperationMode == EOperationMode.INSERT)
            {
                if (CheckKey((int)EIndex.StoreCD, false) == false)
                {
                    return;
                }
            }
            else
            {
                for (int i = 0; i < keyControls.Length; i++)
                    if (CheckKey(i, false) == false)
                    {
                        keyControls[i].Focus();
                        return;
                    }
            }

            if (OperationMode != EOperationMode.DELETE)
            {
                for (int i = 0; i < detailControls.Length; i++)
                    if (CheckDetail(i, false) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }

                dje = new D_Juchuu_Entity();
                dje.JuchuuNO = keyControls[(int)EIndex.JuchuuNO].Text == "" ? mTemporaryReserveNO : keyControls[(int)EIndex.JuchuuNO].Text;

                //D_TemporaryReserveをDelete
                mibl.DeleteTemporaryReserve(dje);

                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                    {
                        for (int CL = (int)ClsGridJuchuu.ColNO.JanCD; CL < (int)ClsGridJuchuu.ColNO.COUNT; CL++)
                        {
                            if (CheckGrid(CL, RW, true) == false)
                            {
                                //Focusセット処理
                                ERR_FOCUS_GRID_SUB(CL, RW);
                                return;
                            }
                        }
                    }
                }

                ////各金額項目の再計算必要
                CalcKin();

                //配列の内容を画面へセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            }

            //M_Control.Tennic＝1 の場合、
            if (mTennic.Equals(1))
            {
                //与信チェックを行う
                //Function_与信チェックより以下情報を取得する
                Fnc_Credit_Entity fce = new Fnc_Credit_Entity
                {
                    Operator = InOperatorCD,
                    PC = InPcID,
                    ChangeDate = detailControls[(int)EIndex.JuchuuDate].Text,
                    CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                };

                bool ret = bbl.Fnc_Credit(fce);
                if(ret)
                {
                    //out与信チェック区分 0:なし、1:警告、2:エラー
                    if(fce.CreditCheckKBN.Equals("0"))
                    {
                        //●与信チェック区分＝0の場合、処理なし 次工程へ

                    }
                    else if (fce.CreditCheckKBN.Equals("1"))
                    {
                        //●与信チェック区分＝1の場合、
                        //●与信限度額<●総債権額＋Footer.税込売上額の場合、警告表示			
                        if(bbl.Z_Set(fce.CreditAmount) < bbl.Z_Set(fce.SaikenGaku) + bbl.Z_Set(lblKin6.Text))
                        {
                            if (bbl.ShowMessage("Q324", bbl.Z_SetStr(fce.CreditAmount), bbl.Z_SetStr(fce.SaikenGaku), bbl.Z_SetStr(lblKin6.Text), fce.CreditMessage) != DialogResult.Yes)
                                return;
                        }
                    }
                    else if (fce.CreditCheckKBN.Equals("2"))
                    {
                        //●与信チェック区分＝2の場合、
                        //●与信限度額<●総債権額＋Footer.税込売上額の場合、警告表示			
                        if (bbl.Z_Set(fce.CreditAmount) < bbl.Z_Set(fce.SaikenGaku) + bbl.Z_Set(lblKin6.Text))
                        {
                            bbl.ShowMessage("E261", bbl.Z_SetStr(fce.CreditAmount), bbl.Z_SetStr(fce.SaikenGaku), bbl.Z_SetStr(lblKin6.Text), fce.CreditMessage);
                            return;
                        }
                    }

                }
            }

            DataTable dt = GetGridEntity();

            //更新処理
            dje = GetEntity();
            mibl.Juchu_Exec(dje,dt, (short)OperationMode);

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");

            //更新後画面クリア
            ChangeOperationMode(OperationMode);
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //排他処理を解除
            DeleteExclusive();

            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            switch (mode)
            {
                case EOperationMode.INSERT:

                    ScStaff.TxtCode.Text = InOperatorCD;
                    ScStaff.LabelText = InOperatorName;
                    CboStoreCD.SelectedValue = StoreCD;

                    detailControls[0].Focus();
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    keyControls[0].Focus();
                    break;

            }

        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            //カスタムコントロールのLeave処理を先に走らせるため
            IMT_DMY_0.Focus();

            if (Kbn == 0)
            {
                foreach (Control ctl in keyControls)
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

                foreach (Control ctl in keyLabels)
                {
                    ((CKM_SearchControl)ctl).LabelText = "";
                }

                lblDisp.Text = "未売上";
                mTemporaryReserveNO = "";
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(Panel)))
                {
                    radioSama.Checked = true;
                    radioSamaD.Checked = true;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex=-1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                {
                    //顧客情報ALLクリア
                    ClearCustomerInfo(0);
                    ClearCustomerInfo(1);
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

            mOldJyuchuDate = "";
            S_Clear_Grid();   //画面クリア（明細部）

            lblKin1.Text = "";
            lblKin2.Text = "";
            lblKin3.Text = "";
            lblKin4.Text = "";
            lblKin5.Text = "";
            lblKin6.Text = "";
            lblKin7.Text = "";
            lblKin8.Text = "";
            lblKin9.Text = "";
            lblKin10.Text = "";
            lblKin11.Text = "";
            lblKin12.Text = "";
            lblKin13.Text = "";
            lblDay1.Text = "";
            lblDay2.Text = "";
            lblDay3.Text = "";

            mOrderTaxTiming = 0;
        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo(short kbn)
        {
            addInfo.ClearAddressInfo(kbn);

            if (kbn.Equals(0))
            {
                mOldCustomerCD = "";
                mTaxFractionKBN = 0;
                mTaxTiming = 0;

                //addInfo = new FrmAddress();

                ScCustomer.LabelText = "";
                detailControls[(int)EIndex.CustomerName].Text = "";
                detailControls[(int)EIndex.CustomerName2].Text = "";
                detailControls[(int)EIndex.CustomerName].Enabled = false;
                textBox1.Text = "";
                textBox2.Text = "";
                lblLastSalesDate.Text = "";
                lblStoreName.Text = "";
                lblTankaName.Text = "";
                lblPoint.Text = "";
            }
            else
            {
                mOldDeliveryCD= "";

                ScDeliveryCD.LabelText = "";
                detailControls[(int)EIndex.DeliveryName].Text = "";
                detailControls[(int)EIndex.DeliveryName2].Text = "";
                detailControls[(int)EIndex.DeliveryName].Enabled = false;
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
                            ScJuchuuNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            //返品チェックがONのときのみ入力可
                            ScMotoJuchuNo.Enabled = false;
                            ScMotoJuchuNo.BtnSearch.Enabled = false;
                            
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
                            for (int idx = 0; idx < (int)EIndex.COUNT; idx++)
                            {
                                switch (idx)
                                {
                                    case (int)EIndex.CustomerName:
                                    case (int)EIndex.CustomerName2:
                                    case (int)EIndex.DeliveryName:
                                    case (int)EIndex.DeliveryName2:
                                        break;
                                    default:
                                        detailControls[idx].Enabled = Kbn == 0 ? true : false;
                                        break;
                                }

                            }
                            for (int index = 0; index < searchButtons.Length; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;

                            btnAddress.Enabled = Kbn == 0 ? true : false;
                            BtnSubF11.Enabled = Kbn == 0 ? true : false;
                            Pnl_Body.Enabled = Kbn == 0 ? true : false;
                            Btn_SelectAll.Enabled = Kbn == 0 ? true : false;
                            Btn_NoSelect.Enabled = Kbn == 0 ? true : false;
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

                        ChangeOperationMode(base.OperationMode);

                        break;
                    }
                case 6://F7:行削除
                    ADD_SUB();
                    break;

                case 7://F8:行追加
                    DEL_SUB();
                    break;

                case 9://F10複写
                    CPY_SUB();
                    break;

                case 8: //F9:検索
                    EsearchKbn kbn = EsearchKbn.Null;
                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if(w_Col == (int)ClsGridJuchuu.ColNO.JanCD)
                        //商品検索
                        kbn = EsearchKbn.Product;
                    else if (w_Col == (int)ClsGridJuchuu.ColNO.VendorCD)
                        //仕入先検索
                        kbn = EsearchKbn.Vendor;

                    if (kbn != EsearchKbn.Null)
                        SearchData(kbn, previousCtrl);

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
            try
            {
                DeleteExclusive();
            }
            catch(Exception ex)
            {
                //例外は無視する
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
        }

        /// <summary>
        /// 検索フォーム起動処理
        /// </summary>
        /// <param name="kbn"></param>
        /// <param name="setCtl"></param>
        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            int w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            switch (kbn)
            {
                case EsearchKbn.Product:


                    using (Search_Product frmProduct = new Search_Product(detailControls[(int)EIndex.JuchuuDate].Text))
                    {
                        frmProduct.SKUCD = mGrid.g_DArray[w_Row].SKUCD;
                        //frmProduct.ITEM = mGrid.g_DArray[w_Row].item;
                        //frmProduct.MakerItem = mGrid.g_DArray[w_Row].SKUCD;
                        frmProduct.JANCD = mGrid.g_DArray[w_Row].JanCD;
                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].JanCD = frmProduct.JANCD;
                            mGrid.g_DArray[w_Row].OldJanCD = frmProduct.JANCD;
                            mGrid.g_DArray[w_Row].SKUCD = frmProduct.SKUCD;
                            mGrid.g_DArray[w_Row].AdminNO = frmProduct.AdminNO;

                            CheckGrid((int)ClsGridJuchuu.ColNO.JanCD, w_Row, false, true);

                            //配列の内容を画面へセット
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;

                case EsearchKbn.Vendor:
                    //if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].VendorCD))
                    //    CheckGrid((int)ClsGridJuchuu.ColNO.VendorCD, w_Row, false, true);

                    ////配列の内容を画面へセット
                    //mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                    break;
            }

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
                        if (index == (int)EIndex.StoreCD)
                        {
                            detailControls[(int)EIndex.JuchuuDate].Focus();
                        }
                        else if (index == (int)EIndex.JuchuuNO)
                        {
                            if (OperationMode == EOperationMode.UPDATE)
                            {
                                detailControls[(int)EIndex.JuchuuDate].Focus();
                            }
                        }
                        else if (index == (int)EIndex.CopyJuchuuNO || index == (int)EIndex.MitsumoriNO)
                        {
                            detailControls[(int)EIndex.JuchuuDate].Focus();
                        }
                        else
                        {
                            keyControls[index + 1].Focus();
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
                       if( index == (int)EIndex.Tel3D)
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridJuchuu.ColNO.JanCD);
                        else if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);                          
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

        private void KeyControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                SetFuncKeyAll(this, "111111001011");
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                SetFuncKeyAll(this, "111111001011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 明細部チェックボックスクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CHK_Print_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }

                //画面より配列セット 
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                if (w_Col == (int)ClsGridJuchuu.ColNO.NotPrintFLG)
                {
                    bool Check = mGrid.g_DArray[w_Row].NotPrintFLG;

                    if (w_Row == 0 && Check)
                        mGrid.g_DArray[w_Row].NotPrintFLG = false;

                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JanCD))
                        mGrid.g_DArray[w_Row].NotPrintFLG = false;
                }
                else if (w_Col == (int)ClsGridJuchuu.ColNO.ChkTyokuso)
                {
                    //画面明細.直送checkbox＝onの場合
                    bool Check = mGrid.g_DArray[w_Row].ChkTyokuso;
                    if(Check)
                    {
                        mGrid.g_DArray[w_Row].ArrivePlanDate = "";
                        mGrid.g_DArray[w_Row].KariHikiateNO = "";
                    }

                }

                //配列の内容を画面へセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void GridControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                // 背景色
                w_ActCtl.BackColor = ClsGridBase.BKColor;

                //明細が出荷済の受注明細、または、売上済の受注明細である場合、削除できない（F7ボタンを使えないようにする）
                //出荷済・売上済の場合は行削除不可
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].Syukka))
                {
                    if (mGrid.g_DArray[w_Row].Syukka.Equals("出荷済") || mGrid.g_DArray[w_Row].Syukka.Equals("売上済"))
                        Btn_F8.Text = "";
                    else
                        Btn_F8.Text = "行削除(F8)";
                }
                else
                    Btn_F8.Text = "行削除(F8)";

                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }

                Grid_Gotfocus(w_Col, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void GridControl_Leave(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }
                // 背景色
                w_ActCtl.BackColor = mGrid.F_GetBackColor_MK(w_Col, w_Row);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void GridControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {

                    //どの項目か判別
                    int CL = -1;
                    string ctlName = "";
                    if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        ctlName = w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                    else
                        ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));

                    bool lastCell = false;

                    switch (ctlName)
                    {
                        case "SC_ITEM":
                            CL = (int)ClsGridJuchuu.ColNO.JanCD;
                            break;
                        case "IMT_ITMNM":
                            CL = (int)ClsGridJuchuu.ColNO.SKUName;
                            break;
                        case "CHK_PRINT":
                            CL = (int)ClsGridJuchuu.ColNO.NotPrintFLG;
                            break;
                        case "CHK_FUYO":
                            CL = (int)ClsGridJuchuu.ColNO.ChkFuyo;
                            break;
                        case "CHK_Express":
                            CL = (int)ClsGridJuchuu.ColNO.ChkExpress;
                            break;
                        case "CHK_Tyokuso":
                            CL = (int)ClsGridJuchuu.ColNO.ChkTyokuso;
                            break;
                        case "IMT_SPDAT":
                            CL = (int)ClsGridJuchuu.ColNO.ShippingPlanDate;
                            break;
                        case "IMN_ORTAN":
                            CL = (int)ClsGridJuchuu.ColNO.OrderUnitPrice;
                            break;
                        //case "IMN_ORGAK":
                        //    CL = (int)ClsGridJuchuu.ColNO.OrderGaku;
                        //    break;
                        case "IMN_CLINT":
                            CL = (int)ClsGridJuchuu.ColNO.ColorName;
                            break;
                        case "IMT_KAIDT":
                            CL = (int)ClsGridJuchuu.ColNO.SizeName;
                            break;
                        case "IMN_GENER":
                            CL = (int)ClsGridJuchuu.ColNO.JuchuuSuu;
                            break;
                        case "IMN_GENER2":
                            CL = (int)ClsGridJuchuu.ColNO.JuchuuUnitPrice;
                            break;
                        case "IMC_SOKNM":
                            CL = (int)ClsGridJuchuu.ColNO.SoukoName;
                            break;
                        case "IMT_VENCD":
                            CL = (int)ClsGridJuchuu.ColNO.VendorCD;
                            break;
                        case "IMT_ARIDT":
                            CL = (int)ClsGridJuchuu.ColNO.ArrivePlanDate;
                            break;
                        case "IMN_SALEP2":
                            CL = (int)ClsGridJuchuu.ColNO.CostUnitPrice;
                            break;
                        case "IMT_REMAK":
                            CL = (int)ClsGridJuchuu.ColNO.CommentInStore;
                            break;
                        case "IMN_WEBPR":
                            CL = (int)ClsGridJuchuu.ColNO.CommentOutStore;
                            break;
                        case "IMN_WEBPR2":
                            CL = (int)ClsGridJuchuu.ColNO.IndividualClientName;
                            //if (w_Row == m_dataCnt - 1)
                            if (w_Row == mGrid.g_MK_Max_Row - 1)
                                lastCell = true;
                            break;

                    }

                    bool changeFlg = false;
                    switch (CL)
                    {
                        case (int)ClsGridJuchuu.ColNO.JuchuuSuu:
                            if (mGrid.g_DArray[w_Row].JuchuuSuu != w_ActCtl.Text)
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridJuchuu.ColNO.JuchuuUnitPrice: //販売単価
                            if (mGrid.g_DArray[w_Row].JuchuuUnitPrice != w_ActCtl.Text)
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridJuchuu.ColNO.CostUnitPrice: //原価単価
                            if (mGrid.g_DArray[w_Row].CostUnitPrice != w_ActCtl.Text)
                            {
                                changeFlg = true;
                            }
                            break;
                    }

                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    //手入力時金額を再計算
                    if (changeFlg)
                    {
                        decimal wSuu = bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuSuu);
                        string ymd = detailControls[(int)EIndex.JuchuuDate].Text;
                        decimal wTanka;

                        switch (CL)
                        {
                            case (int)ClsGridJuchuu.ColNO.JuchuuSuu:
                                //数量が整数値かどうかチェック
                                int intSu;
                                if (!int.TryParse(wSuu.ToString(), out intSu))
                                {
                                    bbl.ShowMessage("E102");
                                    return;
                                }

                                //入力された場合、金額再計算
                                if (wSuu != 0)
                                {
                                    //Function_単価取得.
                                    Fnc_UnitPrice_Entity fue = new Fnc_UnitPrice_Entity
                                    {
                                        AdminNo = mGrid.g_DArray[w_Row].AdminNO,
                                        ChangeDate = ymd,
                                        CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                                        StoreCD = CboStoreCD.SelectedIndex >0? CboStoreCD.SelectedValue.ToString():"",
                                        SaleKbn = "0",
                                        Suryo = wSuu.ToString()
                                    };

                                    bool ret = bbl.Fnc_UnitPrice(fue);
                                    if (ret)
                                    {
                                        //数量変更時も単価再計算
                                        switch (mTennic)
                                        {
                                            case 0:
                                                //販売単価=Function_単価取得.out税込単価		
                                                mGrid.g_DArray[w_Row].JuchuuUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeikomiTanka));
                                                break;
                                            case 1:
                                                //販売単価=Function_単価取得.out税抜単価
                                                mGrid.g_DArray[w_Row].JuchuuUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeinukiTanka));
                                                break;
                                        }

                                        //原価単価=Function_単価取得.out原価単価	
                                        if (bbl.Z_Set(fue.GenkaTanka) ==0)
                                            mGrid.g_DArray[w_Row].CostUnitPrice = mGrid.g_DArray[w_Row].OrderUnitPrice;
                                        else
                                            mGrid.g_DArray[w_Row].CostUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka));
                                    }
                                    else
                                    {
                                        //販売単価		
                                        mGrid.g_DArray[w_Row].JuchuuUnitPrice = "0";
                                        //原価単価
                                        mGrid.g_DArray[w_Row].CostUnitPrice = "0";
                                    }

                                    SetJuchuuGaku(w_Row, wSuu, ymd, fue.ZeinukiTanka);

                                    //原価額←Function_単価取得.out原価単価×Form.Detail.見積数
                                    mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka) * wSuu);
                                    //mGrid.g_DArray[w_Row].OrderGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].OrderUnitPrice) * wSuu);

                                    CalcZei(w_Row);
                                }
                                break;

                            case (int)ClsGridJuchuu.ColNO.JuchuuUnitPrice: //販売単価 
                                {
                                    string tanka = mGrid.g_DArray[w_Row].JuchuuUnitPrice;
                                    if (mTennic.Equals(0))
                                        tanka = bbl.GetZeinukiKingaku(bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuUnitPrice), mGrid.g_DArray[w_Row].TaxRateFLG, ymd).ToString();

                                    SetJuchuuGaku(w_Row, wSuu, ymd, tanka);

                                    CalcZei(w_Row, true);
                                }
                                break;

                            case (int)ClsGridJuchuu.ColNO.CostUnitPrice: //原価単価
                                //原価額=Form.Detail.原価単価×	Form.Detail.受注数
                                mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].CostUnitPrice) * bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuSuu));

                                //粗利額
                                mGrid.g_DArray[w_Row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].CostGaku)); // 

                                break;
                        }
                    }

                    //if (CL == -1)
                    //return;

                    //チェック処理
                    if (CheckGrid(CL, w_Row) == false)
                    {
                        if (w_ActCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                        {
                            ((CKM_Controls.CKM_ComboBox)w_ActCtl).MoveNext = false;
                        }

                        //配列の内容を画面へセット
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                        IMT_DMY_0.Focus();
                        //Focusセット処理
                        w_ActCtl.Focus();
                        return;
                    }

                    if (lastCell)
                    {
                        w_ActCtl.Focus();
                        return;
                    }

                    //あたかもTabキーが押されたかのようにする
                    //Shiftが押されている時は前のコントロールのフォーカスを移動
                    //this.ProcessTabKey(!e.Shift);
                    //行き先がなかったら移動しない
                    S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    switch (CL)
                    {
                        case (int)ClsGridJuchuu.ColNO.NotPrintFLG:
                        case (int)ClsGridJuchuu.ColNO.ChkExpress:
                        case (int)ClsGridJuchuu.ColNO.ChkFuyo:
                        case (int)ClsGridJuchuu.ColNO.ChkTyokuso:
                            if (e.Shift)
                                S_Grid_0_Event_ShiftTab(CL, w_Row, w_ActCtl, w_ActCtl);
                            else
                                S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);

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
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;
                Control sc = ((Control)sender).Parent;

                //検索ボタンClick時
                if (((Search.CKM_SearchControl)sc).Name.Substring(0,3).Equals("SC_"))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
                }
                else if (((Search.CKM_SearchControl)sc).Name.Substring(0, 9).Equals("IMT_VENCD"))
                {
                    //仕入先検索
                    kbn = EsearchKbn.Vendor ;
                }
                setCtl = previousCtrl;
                

                if (kbn != EsearchKbn.Null)
                    SearchData(kbn, setCtl);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                
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
                       detailControls[(int)EIndex.AliasKBN + 1].Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }
        private void RadioButtonD_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    detailControls[(int)EIndex.AliasKBN2 + 1].Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnAddress_Click(object sender, EventArgs e)
        {
            try
            {
                addInfo.ade.CustomerCD = ScCustomer.TxtCode.Text;
                addInfo.ade.CustomerName = detailControls[(int)EIndex.CustomerName].Text;
                addInfo.ade.CustomerName2 = detailControls[(int)EIndex.CustomerName2].Text;
                addInfo.ade.Tel11 = detailControls[(int)EIndex.Tel1].Text;
                addInfo.ade.Tel12 = detailControls[(int)EIndex.Tel2].Text;
                addInfo.ade.Tel13 = detailControls[(int)EIndex.Tel3].Text;
                addInfo.kbn = 0;
                addInfo.ShowDialog();

                detailControls[(int)EIndex.Tel1].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }
        private void BtnAddressD_Click(object sender, EventArgs e)
        {
            try
            {
                addInfo.adeD.CustomerCD = ScDeliveryCD.TxtCode.Text;
                addInfo.adeD.CustomerName = detailControls[(int)EIndex.DeliveryName].Text;
                addInfo.adeD.CustomerName2 = detailControls[(int)EIndex.DeliveryName2].Text;
                addInfo.adeD.Tel11 = detailControls[(int)EIndex.Tel1D].Text;
                addInfo.adeD.Tel12 = detailControls[(int)EIndex.Tel2D].Text;
                addInfo.adeD.Tel13 = detailControls[(int)EIndex.Tel3D].Text;
                addInfo.kbn = 1;
                addInfo.ShowDialog();

                detailControls[(int)EIndex.Tel1D].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }
        private void CkM_CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //返品チェックがONのときのみ入力可
                CKM_SearchControl edit = ScMotoJuchuNo;
                bool enabled;
                if (ckM_CheckBox1.Checked)
                {
                    enabled = true;
                }
                else
                {
                    enabled = false;
                }
                edit.Enabled = enabled;
                edit.BtnSearch.Enabled = enabled;

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }


        }
        private void CboSoukoName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //選択された出荷倉庫(倉庫CD)が変わった場合、明細の出荷倉庫CDも同じ倉庫CDに変更する。
                if (CboSoukoName.SelectedIndex > 0)
                {
                    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                    {
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                        {
                            mGrid.g_DArray[RW].SoukoName = CboSoukoName.SelectedValue.ToString();
                        }
                    }
                    //配列の内容を画面にセット
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
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

        private void Btn_SelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheck(true);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void Btn_NoSelect_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheck(false);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        /// <summary>
        /// 明細部サイトボタンクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Site_Click(object sender, EventArgs e)
        {
            try
            {

                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                string url = mGrid.g_DArray[w_Row].Site;
                if(!string.IsNullOrWhiteSpace(url))
                {
                    //urlを標準のブラウザで開いて表示する
                    System.Diagnostics.Process.Start(url);
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
        /// 明細部在庫ボタンクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Zaiko_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                string adminNo = mGrid.g_DArray[w_Row].AdminNO;

                //在庫照会を該当商品をパラメータに起動します
                //Form.印刷CheckBox＝onの場合、印刷プログラム(店舗納品書:TempoNouhinsyo)						
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + ZaikoSyokai;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + adminNo;
                    System.Diagnostics.Process.Start(filePath, cmdLine);
                }
                else
                {
                    //ファイルなし
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
                {
                    ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
                    ScDeliveryCD.Value2 = CboStoreCD.SelectedValue.ToString();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void SetEnabled(bool enabled)
        {
            detailControls[(int)EIndex.CustomerName].Enabled = enabled;
            detailControls[(int)EIndex.CustomerName2].Enabled = enabled;

            detailControls[(int)EIndex.DeliveryName].Enabled = enabled;
            detailControls[(int)EIndex.DeliveryName2].Enabled = enabled;
            if (enabled)
            {

            }
            else
            {
                detailControls[(int)EIndex.CustomerName].Text = "";
                detailControls[(int)EIndex.CustomerName2].Text = "";
                detailControls[(int)EIndex.DeliveryName].Text = "";
                detailControls[(int)EIndex.DeliveryName2].Text = "";
            }
        }

        private void ChangeCheck(bool check)
        {
            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                {
                    mGrid.g_DArray[RW].ChkFuyo = check;
                }
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        }
    }
}








