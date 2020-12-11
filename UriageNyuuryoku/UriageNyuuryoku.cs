using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace UriageNyuuryoku
{
    /// <summary>
    /// UriageNyuuryoku 売上入力
    /// </summary>
    internal partial class UriageNyuuryoku : FrmMainForm
    {
        private const string ProID = "UriageNyuuryoku";
        private const string ProNm = "売上入力";
        private const short mc_L_END = 3; // ロック用
        private const string TempoNouhinsyo = "TempoNouhinsyo.exe";        

        private enum EIndex : int
        {
            SalesNO,
            CopySalesNO,
            ckM_CheckBox1,
            MotoSalesNO,
            StoreCD,

            SalesDate = 0,
            ChkSeikyu,
            StaffCD,
            CustomerCD,
            CustomerName,
            CustomerName2,

            PaymentMethodCD,
            NouhinsyoComment,
            ChkPrint,
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
        
        private UriageNyuuryoku_BL mubl;
        private D_Sales_Entity dse;
        private int mTaxFractionKBN;
        private int mTennic;
        private string mMesTxt = "売上番号"; 

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string InOperatorName = "";
        private DataTable dtForUpdate;  //排他用   
        private string mOldBillingNo = "";    //排他処理のため使用
        private string mOldPurchaseNO = "";    //排他処理のため使用
        private string mOldSalesNo = "";    //排他処理のため使用
        private string mOldSalesDate = "";
        private string mOldCustomerCD = "";
        private string mVendorCD = "";
        private string mPayeeCD = "";
        private decimal mZei10;//通常税額(Hidden)
        private decimal mZei8;//軽減税額(Hidden)
        private decimal mSalesHontaiGaku10;
        private decimal mSalesHontaiGaku8;
        private decimal mSalesHontaiGaku0;

        private string mBillingType;         //M_Customer.BillingType
        private string mBillingCD;          //M_Customer.BillingCD
        private string mPaymentMethodCD;    //M_Customer.PaymentMethodCD
        private string mCollectPlanDate;
        private string mPaymentPlanDate;
        private int mTaxTiming;
        private int mOrderTaxTiming;
        private decimal mOrderTax10;//通常税額(Hidden)
        private decimal mOrderTax8;//軽減税額(Hidden)

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridUriage mGrid = new ClsGridUriage();
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

            if (ClsGridUriage.gc_P_GYO <= ClsGridUriage.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridUriage.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridUriage.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridUriage.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridUriage.gc_MaxCL;

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
            mGrid.g_DArray = new ClsGridUriage.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridUriage.ColNO.GYONO; i <= (int)ClsGridUriage.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridUriage.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridUriage.ColNO.SalesSU:
                            mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                            break;
                        case (int)ClsGridUriage.ColNO.SalesHontaiGaku:
                        case (int)ClsGridUriage.ColNO.SalesUnitPrice:
                        case (int)ClsGridUriage.ColNO.SalesGaku:
                        case (int)ClsGridUriage.ColNO.CostUnitPrice:
                        case (int)ClsGridUriage.ColNO.CostGaku:
                        case (int)ClsGridUriage.ColNO.ProfitGaku:
                        case (int)ClsGridUriage.ColNO.OrderUnitPrice:
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
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space1, 0].CellCtl = ckM_TextBox9;

            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.JanCD, 0].CellCtl = SC_ITEM_0;// IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SizeName, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostGaku, 0].CellCtl = IMN_GENKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesGaku, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesHontaiGaku, 0].CellCtl = IMN_TEIKA2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesSU, 0].CellCtl = IMN_GENER_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesUnitPrice, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaniCD, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ColorName, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ProfitGaku, 0].CellCtl = IMN_SALEP_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostUnitPrice, 0].CellCtl = IMN_SALEP2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentOutStore, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.IndividualClientName, 0].CellCtl = IMN_WEBPR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentInStore, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRateDisp, 0].CellCtl = IMT_ZKDIS_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRate, 0].CellCtl = IMT_ZEIRT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space2, 0].CellCtl = IMT_JUONO_0;      //発注番号
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorCD, 0].CellCtl = IMT_VENCD_0;           //発注先
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorName, 0].CellCtl = IMT_VENNM_0;         //発注先名
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderGaku, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.NotPrintFLG, 0].CellCtl = CHK_PRINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.PaymentPlanDate, 0].CellCtl = IMT_SPDAT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderUnitPrice, 0].CellCtl = IMN_ORTAN_0;

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space1, 1].CellCtl = ckM_TextBox10;

            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.JanCD, 1].CellCtl = SC_ITEM_1;//IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SizeName, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostGaku, 1].CellCtl = IMN_GENKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesGaku, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesHontaiGaku, 1].CellCtl = IMN_TEIKA2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesSU, 1].CellCtl = IMN_GENER_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesUnitPrice, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaniCD, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ColorName, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ProfitGaku, 1].CellCtl = IMN_SALEP_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostUnitPrice, 1].CellCtl = IMN_SALEP2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentOutStore, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.IndividualClientName, 1].CellCtl = IMN_WEBPR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentInStore, 1].CellCtl = IMT_REMAK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRateDisp, 1].CellCtl = IMT_ZKDIS_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRate, 1].CellCtl = IMT_ZEIRT_1;
            
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space2, 1].CellCtl = IMT_JUONO_1;      //発注番号
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorCD, 1].CellCtl = IMT_VENCD_1;           //発注先
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorName, 1].CellCtl = IMT_VENNM_1;         //発注先名
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderGaku, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.NotPrintFLG, 1].CellCtl = CHK_PRINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.PaymentPlanDate, 1].CellCtl = IMT_SPDAT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderUnitPrice, 1].CellCtl = IMN_ORTAN_1;

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space1, 2].CellCtl = ckM_TextBox11;

            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.JanCD, 2].CellCtl = SC_ITEM_2;//IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SizeName, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostGaku, 2].CellCtl = IMN_GENKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesGaku, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesHontaiGaku, 2].CellCtl = IMN_TEIKA2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesSU, 2].CellCtl = IMN_GENER_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesUnitPrice, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaniCD, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ColorName, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ProfitGaku, 2].CellCtl = IMN_SALEP_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostUnitPrice, 2].CellCtl = IMN_SALEP2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentOutStore, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.IndividualClientName, 2].CellCtl = IMN_WEBPR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentInStore, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRateDisp, 2].CellCtl = IMT_ZKDIS_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRate, 2].CellCtl = IMT_ZEIRT_2;
          
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space2, 2].CellCtl = IMT_JUONO_2;      //発注番号
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorCD, 2].CellCtl = IMT_VENCD_2;           //発注先
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorName, 2].CellCtl = IMT_VENNM_2;         //発注先名
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderGaku, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.NotPrintFLG, 2].CellCtl = CHK_PRINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.PaymentPlanDate, 2].CellCtl = IMT_SPDAT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderUnitPrice, 2].CellCtl = IMN_ORTAN_2;


            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space1, 3].CellCtl = ckM_TextBox4;

            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.JanCD, 3].CellCtl = SC_ITEM_3;//IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SizeName, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostGaku, 3].CellCtl = IMN_GENKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesGaku, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesHontaiGaku, 3].CellCtl = IMN_TEIKA2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesSU, 3].CellCtl = IMN_GENER_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesUnitPrice, 3].CellCtl = IMN_GENER2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaniCD, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ColorName, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ProfitGaku, 3].CellCtl = IMN_SALEP_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostUnitPrice, 3].CellCtl = IMN_SALEP2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentOutStore, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.IndividualClientName, 3].CellCtl = IMN_WEBPR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentInStore, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRateDisp, 3].CellCtl = IMT_ZKDIS_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRate, 3].CellCtl = IMT_ZEIRT_3;

            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space2, 3].CellCtl = IMT_JUONO_3;      //発注番号
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorCD, 3].CellCtl = IMT_VENCD_3;           //発注先
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorName, 3].CellCtl = IMT_VENNM_3;         //発注先名
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderGaku, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.NotPrintFLG, 3].CellCtl = CHK_PRINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.PaymentPlanDate, 3].CellCtl = IMT_SPDAT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderUnitPrice, 3].CellCtl = IMN_ORTAN_3;


            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space1, 4].CellCtl = ckM_TextBox32;

            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.JanCD, 4].CellCtl = SC_ITEM_4;//IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SizeName, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostGaku, 4].CellCtl = IMN_GENKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesGaku, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesHontaiGaku, 4].CellCtl = IMN_TEIKA2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesSU, 4].CellCtl = IMN_GENER_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesUnitPrice, 4].CellCtl = IMN_GENER2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaniCD, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ColorName, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ProfitGaku, 4].CellCtl = IMN_SALEP_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostUnitPrice, 4].CellCtl = IMN_SALEP2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentOutStore, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.IndividualClientName, 4].CellCtl = IMN_WEBPR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentInStore, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRateDisp, 4].CellCtl = IMT_ZKDIS_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRate, 4].CellCtl = IMT_ZEIRT_4;

            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space2, 4].CellCtl = IMT_JUONO_4;      //発注番号
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorCD, 4].CellCtl = IMT_VENCD_4;           //発注先
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorName, 4].CellCtl = IMT_VENNM_4;         //発注先名
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderGaku, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.NotPrintFLG, 4].CellCtl = CHK_PRINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.PaymentPlanDate, 4].CellCtl = IMT_SPDAT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderUnitPrice, 4].CellCtl = IMN_ORTAN_4;

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space1, 5].CellCtl = ckM_TextBox56;

            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.JanCD, 5].CellCtl = SC_ITEM_5;// IMT_JANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SizeName, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostGaku, 5].CellCtl = IMN_GENKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesGaku, 5].CellCtl = IMN_TEIKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesHontaiGaku, 5].CellCtl = IMN_TEIKA2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesSU, 5].CellCtl = IMN_GENER_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.SalesUnitPrice, 5].CellCtl = IMN_GENER2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaniCD, 5].CellCtl = IMN_MEMBR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ColorName, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.ProfitGaku, 5].CellCtl = IMN_SALEP_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CostUnitPrice, 5].CellCtl = IMN_SALEP2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentOutStore, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.IndividualClientName, 5].CellCtl = IMN_WEBPR2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.CommentInStore, 5].CellCtl = IMT_REMAK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRateDisp, 5].CellCtl = IMT_ZKDIS_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.TaxRate, 5].CellCtl = IMT_ZEIRT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.Space2, 5].CellCtl = IMT_JUONO_5;      //発注番号
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorCD, 5].CellCtl = IMT_VENCD_5;           //発注先
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.VendorName, 5].CellCtl = IMT_VENNM_5;         //発注先名
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderGaku, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.NotPrintFLG, 5].CellCtl = CHK_PRINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.PaymentPlanDate, 5].CellCtl = IMT_SPDAT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridUriage.ColNO.OrderUnitPrice, 5].CellCtl = IMN_ORTAN_5;
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridUriage.Gen_MK_FocusMove.MvNxt, (int)ClsGridUriage.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridUriage.Gen_MK_FocusMove.MvPrv, (int)ClsGridUriage.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridUriage.Gen_MK_FocusMove.MvNxt, (int)ClsGridUriage.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridUriage.Gen_MK_FocusMove.MvSet, (int)ClsGridUriage.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridUriage.Gen_MK_FocusMove.MvSet, (int)ClsGridUriage.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 MouseWheel の処理
        private void S_Grid_0_Event_MouseWheel(int pDelta)
        {
            int w_ToMove = pDelta * (-1) / (int)mGrid.g_WHEEL_DELTA;
            int w_Value;
            int w_MaxValue;

            mGrid.g_WheelFLG = true;

            if (mGrid.g_MK_MaxValue > m_dataCnt - 1)
                w_MaxValue = m_dataCnt - 1;
            else
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
                            case (int)ClsGridUriage.ColNO.GYONO:
                            case (int)ClsGridUriage.ColNO.Space1:
                            case (int)ClsGridUriage.ColNO.Space2:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }
                            case (int)ClsGridUriage.ColNO.SKUCD:
                            case (int)ClsGridUriage.ColNO.SalesHontaiGaku:
                            case (int)ClsGridUriage.ColNO.TaxRateDisp:
                            case (int)ClsGridUriage.ColNO.TaniCD:
                            case (int)ClsGridUriage.ColNO.SalesGaku:
                            case (int)ClsGridUriage.ColNO.TaxRate:
                            case (int)ClsGridUriage.ColNO.CostGaku:
                            case (int)ClsGridUriage.ColNO.ProfitGaku:
                            case (int)ClsGridUriage.ColNO.VendorName:         //発注先名
                            case (int)ClsGridUriage.ColNO.PaymentPlanDate:
                            case (int)ClsGridUriage.ColNO.OrderGaku:
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

                    //// クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    //switch (w_Col)
                    //{
                    //    case (int)ClsGridUriage.ColNO.Site:
                    //    case (int)ClsGridUriage.ColNO.Zaiko:
                    //        {
                    //            // ボタン
                    //            S_Set_Cell_Selectable(w_Col, w_Row, false);
                    //            break;
                    //        }
                    //}
                }
            }

            // データ用配列初期化
            Array.Clear(mGrid.g_DArray, mGrid.g_DArray.GetLowerBound(0), mGrid.g_DArray.GetLength(0));

            // 行番号の初期値セット
            for (w_Row = 0; w_Row <= mGrid.g_MK_Max_Row - 1; w_Row++)
                mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();

            // 画面クリア
            mGrid.S_DispFromArray(0, ref this.Vsb_Mei_0);
            
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
                                mGrid.g_MK_State[(int)ClsGridUriage.ColNO.JanCD, w_Row].Cell_Enabled = true;
                            }
                            
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                keyControls[(int)EIndex.SalesNO].Text = "";
                                keyControls[(int)EIndex.SalesNO].Enabled = false;
                                ScJuchuuNO.BtnSearch.Enabled = false;
                                keyControls[(int)EIndex.CopySalesNO].Enabled = true;
                                ScCopyJuchuuNO.BtnSearch.Enabled = true;
                                //新規Modeの場合のみCheck可能
                                ckM_CheckBox1.Enabled = true;

                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                detailControls[(int)EIndex.SalesDate].Text = bbl.GetDate();
                                F9Visible = false;

                                SetFuncKeyAll(this, "111111001001");
                            }
                            else
                            {
                                keyControls[(int)EIndex.SalesNO].Enabled = true;
                                ScJuchuuNO.BtnSearch.Enabled = true;
                                keyControls[(int)EIndex.CopySalesNO].Text = "";
                                keyControls[(int)EIndex.CopySalesNO].Enabled = false;
                                ScCopyJuchuuNO.BtnSearch.Enabled = false;
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
                                    mGrid.g_MK_State[(int)ClsGridUriage.ColNO.JanCD, w_Row].Cell_Enabled = true;
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridUriage.ColNO.JanCD:
                                        case (int)ClsGridUriage.ColNO.SalesSU:    // 
                                        case (int)ClsGridUriage.ColNO.SalesUnitPrice:    // 
                                        case (int)ClsGridUriage.ColNO.CommentOutStore:    // 
                                        case (int)ClsGridUriage.ColNO.IndividualClientName:    //  
                                        case (int)ClsGridUriage.ColNO.CommentInStore:    //
                                        case (int)ClsGridUriage.ColNO.NotPrintFLG:    // 
                                        case (int)ClsGridUriage.ColNO.OrderUnitPrice:    //  
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                break;
                                            }
                                        case (int)ClsGridUriage.ColNO.SKUName:    // 
                                        case (int)ClsGridUriage.ColNO.ColorName:    // 
                                        case (int)ClsGridUriage.ColNO.SizeName:
                                        case (int)ClsGridUriage.ColNO.VendorCD:           //発注先
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


            if (pCol == (int)ClsGridUriage.ColNO.JanCD || w_AllFlg)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].JanCD))
                {
                    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        if (w_Col == (int)ClsGridUriage.ColNO.JanCD)
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
                            case (int)ClsGridUriage.ColNO.JanCD:
                                //if (mGrid.g_DArray[pRow].salesGyoNO == 0)
                                //{
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                //}
                                //else
                                //{
                                //    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                //    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                //}
                                break;

                            case (int)ClsGridUriage.ColNO.SalesSU:
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

                            case (int)ClsGridUriage.ColNO.SKUName:
                            case (int)ClsGridUriage.ColNO.ColorName:    
                            case (int)ClsGridUriage.ColNO.SizeName:
                            case (int)ClsGridUriage.ColNO.CostUnitPrice:
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

                                
                            case (int)ClsGridUriage.ColNO.SalesUnitPrice:    // 
                            case (int)ClsGridUriage.ColNO.CommentOutStore:    // 
                            case (int)ClsGridUriage.ColNO.IndividualClientName:    //  
                            case (int)ClsGridUriage.ColNO.CommentInStore:    //
                            case (int)ClsGridUriage.ColNO.NotPrintFLG:    //
                            case (int)ClsGridUriage.ColNO.OrderUnitPrice:    //  
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                                                    }
                                break;

                            case (int)ClsGridUriage.ColNO.VendorCD:           //発注先
                                {
                                    if (OperationMode == EOperationMode.INSERT)
                                    {
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                    }
                                    else
                                    {
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                        mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = true;
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                    }
                                }
                                break;

                        }

                    }
                    w_AllFlg = false;
                    
                }
            }
            if (pCol == (int)ClsGridUriage.ColNO.CostUnitPrice)
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

            ////明細が出荷済の売上明細、または、売上済の売上明細である場合、削除できない（F7ボタンを使えないようにする）
            ////出荷済・売上済の場合は行削除不可
            //if (mGrid.g_DArray[w_Row].Syukka.Equals("出荷済") || mGrid.g_DArray[w_Row].Syukka.Equals("売上済"))
            //    return;

            for (int i = w_Row; i < mGrid.g_MK_Max_Row - 1; i++)
            {
             int   w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //次行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i + 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            CalcKin();

            int col = (int)ClsGridUriage.ColNO.JanCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //フォーカスセット
            IMT_DMY_0.Focus();

                //現在行へ
                mGrid.F_MoveFocus((int)ClsGridUriage.Gen_MK_FocusMove.MvSet, (int)ClsGridUriage.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

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

            int col = (int)ClsGridUriage.ColNO.JanCD;

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
                    mGrid.g_DArray[i].PurchaseNO = "";
                    //mGrid.g_DArray[i].BillingNo = "";
                    mGrid.g_DArray[i].salesGyoNO = 0;
                    mGrid.g_DArray[i].copyJuchuGyoNO = 0;
                }

                Grid_NotFocus(col, i);
            }

            //状態もコピー
            // ※ 前行と状態が違うとき注意、この部分変更要 (修正元のあるなしで 入力可能項目が変わる場合など)
            for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                mGrid.g_MK_State[w_Col, w_Row] = mGrid.g_MK_State[w_Col, w_Row - 1];
            }

            string ymd = detailControls[(int)EIndex.SalesDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();
            
            Grid_NotFocus(col, w_Row);
            CalcKin();

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            mGrid.F_MoveFocus((int)ClsGridUriage.Gen_MK_FocusMove.MvSet, (int)ClsGridUriage.Gen_MK_FocusMove.MvSet, IMT_DMY_0, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);
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

            int col = (int)ClsGridUriage.ColNO.JanCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridUriage.Gen_MK_FocusMove.MvSet, (int)ClsGridUriage.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

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
            Grid_NotFocus((int)ClsGridUriage.ColNO.JanCD, RW);

            // 配列の内容を画面にセット
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
        }

        #endregion

        public UriageNyuuryoku()
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
                Btn_F11.Text = "";

                // 明細部初期化
                this.S_SetInit_Grid();

                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();

                mTennic = bbl.GetTennic();

                //売上番号
                //if(mTennic.Equals(1))
                //{
                //    mMesTxt = "売上番号";
                //    LblJuchuNo.Visible = false;
                //    LblCopyJuchuNo.Visible = false;
                //    LblMotoJuchuNo.Visible = false;
                //}
                //else
                //{
                    LblJuchuNoT.Visible = false;
                    LblCopyJuchuNoT.Visible = false;
                    LblMotoJuchuNoT.Visible = false;
                //}

                string ymd = bbl.GetDate();
                mubl = new UriageNyuuryoku_BL();
                CboStoreCD.Bind(ymd);
                CboPaymentMethodCD.Bind(string.Empty);

                string stores = GetAllAvailableStores();
                ScJuchuuNO.Value1 = InOperatorCD;
                ScJuchuuNO.Value2 = stores;
                ScCopyJuchuuNO.Value1 = InOperatorCD;
                ScCopyJuchuuNO.Value2 = stores;
                ScMotoJuchuNo.Value1 = InOperatorCD;
                ScMotoJuchuNo.Value2 = stores;
                ScCustomer.Value1 = "1";

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = mubl.GetDate()
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

                detailControls[(int)EIndex.SalesDate].Text = ymd;

                ////出荷指示登録から起動された場合、照会モードで起動
                ////コマンドライン引数を配列で取得する
                //string[] cmds = System.Environment.GetCommandLineArgs();
                //if (cmds.Length - 1 > (int)ECmdLine.PcID)
                //{
                //    string juchuNO = cmds[(int)ECmdLine.PcID + 1];   //
                //    ChangeOperationMode(EOperationMode.SHOW);
                //    keyControls[(int)EIndex.SalesNO].Text = juchuNO;
                //    CheckKey((int)EIndex.SalesNO, true);
                //}
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
            keyControls = new Control[] {  ScJuchuuNO.TxtCode, ScCopyJuchuuNO.TxtCode, ckM_CheckBox1, ScMotoJuchuNo.TxtCode, CboStoreCD };
            keyLabels = new Control[] {  };
            detailControls = new Control[] { ckM_TextBox1, ckM_CheckBox2, ScStaff.TxtCode
                         , ScCustomer.TxtCode,ckM_TextBox7,ckM_Text_4, CboPaymentMethodCD , ckM_MultiLineTextBox1,ChkPrint };
            detailLabels = new Control[] { ScCustomer,ScStaff};
            searchButtons = new Control[] { ScCustomer.BtnSearch, ScStaff.BtnSearch,};

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
                case (int)EIndex.SalesNO:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

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

                case (int)EIndex.CopySalesNO:
                    if (!string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return CheckData(set, index);

                    break;

                case (int)EIndex.MotoSalesNO:
                    if (!string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return CheckData(set, index);
                    

                    break;

            }

            return true;

        }

        private bool SelectAndInsertExclusive(Exclusive_BL.DataKbn kbn, string No)
        {
            if (OperationMode == EOperationMode.SHOW)
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)kbn,
                Number = No,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                keyControls[(int)EIndex.SalesNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive()
        {
            if (dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                mOldSalesNo = "";
                mOldPurchaseNO = "";
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity
                    {
                        DataKBN = Convert.ToInt16(dr["kbn"]),
                        Number = dr["no"].ToString()
                    };

                    ebl.D_Exclusive_Delete(de);
                }
                return;
            }
        }

        /// <summary>
        /// 売上データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set, int index= (int)EIndex.SalesNO)
        {
            //[D_Sales_SelectDataForUriageNyuuryoku]
            dse = new D_Sales_Entity();

            if(index == (int)EIndex.CopySalesNO)
                dse.SalesNO = keyControls[(int)EIndex.CopySalesNO].Text;
            else if (index == (int)EIndex.MotoSalesNO)
                dse.SalesNO = keyControls[(int)EIndex.MotoSalesNO].Text;
            else
                dse.SalesNO = keyControls[(int)EIndex.SalesNO].Text;

            DataTable dt = mubl.D_Sales_SelectData(dse, (short)OperationMode, (short)mTennic);

            //売上(D_Juchuu)に存在しない場合、Error 「登録されていない売上番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", mMesTxt);
                Scr_Clr(1);
                previousCtrl.Focus();
                return false;
            }
            else
            {
                //このプログラムで作成された売上データで無い場合、Error 「他のプログラムで登録された伝票の為、呼び出せません。」
                if (!dt.Rows[0]["SalesEntryKBN"].ToString().Equals("1"))
                {
                    bbl.ShowMessage("E247", mMesTxt);
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                //DeleteDateTime 「削除された売上番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", mMesTxt);
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                //権限がない場合（以下のSelectができない場合）Error　「権限のない売上番号」
                if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                {
                    bbl.ShowMessage("E139", mMesTxt);
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                bool ret;
                if (index == (int)EIndex.SalesNO)
                {
                    //店舗の締日チェック
                    //店舗締マスターで判断
                    M_StoreClose_Entity mste = new M_StoreClose_Entity
                    {
                        StoreCD = dt.Rows[0]["StoreCD"].ToString(),
                        FiscalYYYYMM = dt.Rows[0]["SalesDate"].ToString().Replace("/", "").Substring(0, 6)
                    };
                    ret = bbl.CheckStoreClose(mste, true, true, false, false, true);
                    if (!ret)
                    {
                        return false;
                    }

                    dse.PurchaseNO = dt.Rows[0]["PurchaseNO"].ToString();
                    dse.StoreCD = dt.Rows[0]["StoreCD"].ToString();

                    //進捗チェック　既に入金消込済みの場合、エラーＥ２４６
                    ret = mubl.CheckSalesData(dse, out string errno, (short)mTennic);
                    if (ret)
                    {
                        if (!string.IsNullOrWhiteSpace(errno))
                        {
                            //警告メッセージを表示する
                            bbl.ShowMessage(errno);
                        }
                    }
                }

                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                DeleteExclusive();

                dtForUpdate = new DataTable();
                dtForUpdate.Columns.Add("kbn", Type.GetType("System.String"));
                dtForUpdate.Columns.Add("no", Type.GetType("System.String"));


                //排他処理
                foreach (DataRow row in dt.Rows)
                {
                    if (mOldSalesNo != row["SalesNo"].ToString())
                    {
                        ret = SelectAndInsertExclusive(Exclusive_BL.DataKbn.Uriage, row["SalesNo"].ToString());
                        if (!ret)
                            return false;

                        mOldSalesNo = row["SalesNo"].ToString();

                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["kbn"] = (int)Exclusive_BL.DataKbn.Uriage;
                        rowForUpdate["no"] = mOldSalesNo;
                        dtForUpdate.Rows.Add(rowForUpdate);
                    }
                    if (mOldPurchaseNO != row["PurchaseNO"].ToString() && !string.IsNullOrWhiteSpace(row["PurchaseNO"].ToString()))
                    {
                        ret = SelectAndInsertExclusive(Exclusive_BL.DataKbn.Shiire, row["PurchaseNO"].ToString());
                        if (!ret)
                            return false;

                        mOldPurchaseNO = row["PurchaseNO"].ToString();

                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["kbn"] = (int)Exclusive_BL.DataKbn.Shiire;
                        rowForUpdate["no"] = mOldPurchaseNO;
                        dtForUpdate.Rows.Add(rowForUpdate);
                    }
                    if (mOldBillingNo != row["BillingNo"].ToString() && !string.IsNullOrWhiteSpace(row["BillingNo"].ToString()))
                    {
                        ret = SelectAndInsertExclusive(Exclusive_BL.DataKbn.Seikyu, row["BillingNo"].ToString());
                        if (!ret)
                            return false;

                        mOldBillingNo = row["BillingNo"].ToString();

                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["kbn"] = (int)Exclusive_BL.DataKbn.Seikyu;
                        rowForUpdate["no"] = mOldBillingNo;
                        dtForUpdate.Rows.Add(rowForUpdate);
                    }
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
                        if (index == (int)EIndex.SalesNO)
                        {
                            detailControls[(int)EIndex.SalesDate].Text = row["SalesDate"].ToString();
                        }
                        else
                        {
                            detailControls[(int)EIndex.SalesDate].Text = bbl.GetDate();
                        }
                        mOldSalesDate = detailControls[(int)EIndex.SalesDate].Text;
                        
                        detailControls[(int)EIndex.StaffCD].Text = row["StaffCD"].ToString();
                        CheckDetail((int)EIndex.StaffCD);

                        detailControls[(int)EIndex.CustomerCD].Text = row["CustomerCD"].ToString();
                        CheckDetail((int)EIndex.CustomerCD);

                        if (detailControls[(int)EIndex.CustomerName].Enabled)
                        {
                            detailControls[(int)EIndex.CustomerName].Text = row["CustomerName"].ToString();
                        }
                        detailControls[(int)EIndex.CustomerName2].Text = row["CustomerName2"].ToString();

                        //【Data Area Footer】
                        CboPaymentMethodCD.SelectedValue = row["PaymentMethodCD"].ToString();// mPaymentMethodCD;
                        detailControls[(int)EIndex.NouhinsyoComment].Text = row["NouhinsyoComment"].ToString();
                        ChkPrint.Checked = true;

                        lblKin5.Text = bbl.Z_SetStr(row["Discount"]);
                        lblKin6.Text = bbl.Z_SetStr(row["SalesGaku"]);
                        lblKin2.Text = bbl.Z_SetStr(row["SalesHontaiGaku"]);
                        lblKin3.Text = bbl.Z_SetStr(row["CostGaku"]);
                        lblKin4.Text = bbl.Z_SetStr(row["ProfitGaku"]);
                        lblKin7.Text = bbl.Z_SetStr(row["PurchaseGaku"]);
                        lblKin10.Text = bbl.Z_SetStr(bbl.Z_Set(row["SalesTax8"])+ bbl.Z_Set(row["SalesTax10"]));

                        if (index == (int)EIndex.SalesNO)
                        {                            
                            mPaymentPlanDate = row["PaymentPlanDate"].ToString();
                            mVendorCD = row["VendorCD"].ToString();
                            mPayeeCD = row["PayeeCD"].ToString();
                        }
                        else
                        {
                            lblKin8.Text = "";
                        }
                        lblKin11.Text = bbl.Z_SetStr(row["SalesTax10"]);
                        lblKin12.Text = bbl.Z_SetStr(row["SalesTax8"]);
                        lblKin8.Text = bbl.Z_SetStr(row["PurchaseTax"]);
                        //lblKin13.Text = bbl.Z_SetStr(row["OrderGaku"]);

                        //明細なしの場合
                        if (bbl.Z_Set(row["SalesRows"]) == 0)
                            break;
                    }

                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    //mGrid.g_DArray[i].OldJanCD = mGrid.g_DArray[i].JanCD;     del 4/24
                    mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                    mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();

                    if (index == (int)EIndex.MotoSalesNO)
                    {
                        mGrid.g_DArray[i].SalesSuu =bbl.Z_SetStr(-1 * bbl.Z_Set(row["SalesSu"]));   //単価算出のため先にセットしておく    
                    }
                    else
                    {
                        mGrid.g_DArray[i].SalesSuu = bbl.Z_SetStr(row["SalesSu"]);   //単価算出のため先にセットしておく    
                    }
                    if (index == (int)EIndex.SalesNO)
                    {
                        mGrid.g_DArray[i].PurchaseNO = row["PurchaseNO"].ToString();
                        mGrid.g_DArray[i].salesGyoNO = Convert.ToInt16(row["SalesRows"].ToString());
                        if (m_MaxJyuchuGyoNo < mGrid.g_DArray[i].salesGyoNO)
                            m_MaxJyuchuGyoNo = mGrid.g_DArray[i].salesGyoNO;
                    }
                    else if (index == (int)EIndex.CopySalesNO)
                    {
                        mGrid.g_DArray[i].copyJuchuGyoNO = Convert.ToInt16(row["SalesRows"].ToString());
                    }
                    

                    CheckGrid((int)ClsGridUriage.ColNO.JanCD, i, true);

                    mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();   // 
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 

                    mGrid.g_DArray[i].NotPrintFLG = row["NotPrintFLG"].ToString() == "1" ? true : false;

                    if (index == (int)EIndex.MotoSalesNO)
                    {
                        mGrid.g_DArray[i].SalesSuu = bbl.Z_SetStr(-1 * bbl.Z_Set(row["SalesSu"]));   //単価算出のため先にセットしておく   
                        mGrid.g_DArray[i].SalesHontaiGaku = bbl.Z_SetStr(-1 * bbl.Z_Set(row["D_SalesHontaiGaku"]));   // 
                        mGrid.g_DArray[i].SalesGaku = bbl.Z_SetStr(-1 * bbl.Z_Set(row["D_SalesGaku"]));   // 
                        mGrid.g_DArray[i].CostGaku = bbl.Z_SetStr(-1 * bbl.Z_Set(row["D_CostGaku"]));   //  
                        mGrid.g_DArray[i].OrderGaku = bbl.Z_SetStr(-1 * bbl.Z_Set(row["D_PurchaseGaku"]));   // 
                    }
                    else
                    {
                        mGrid.g_DArray[i].SalesHontaiGaku = bbl.Z_SetStr(row["D_SalesHontaiGaku"]);   // 
                        mGrid.g_DArray[i].SalesGaku = bbl.Z_SetStr(row["D_SalesGaku"]);   // 
                        mGrid.g_DArray[i].CostGaku = bbl.Z_SetStr(row["D_CostGaku"]);   //  
                        mGrid.g_DArray[i].OrderGaku = bbl.Z_SetStr(row["D_PurchaseGaku"]);   // 
                    }
                    mGrid.g_DArray[i].SalesUnitPrice = bbl.Z_SetStr(row["SalesUnitPrice"]);   // 
                    mGrid.g_DArray[i].CostUnitPrice = bbl.Z_SetStr(row["CostUnitPrice"]);   // 
                    mGrid.g_DArray[i].OrderUnitPrice = bbl.Z_SetStr(row["PurchaserUnitPrice"]);   // 


                    CalcZei(i);

                    if (mGrid.g_DArray[i].TaxRateFLG == 1)
                    {
                        mGrid.g_DArray[i].JuchuTax = bbl.Z_Set(row["D_SalesTax"]);
                    }
                    else if (mGrid.g_DArray[i].TaxRateFLG == 2)
                    {
                        mGrid.g_DArray[i].KeigenTax = bbl.Z_Set(row["D_SalesTax"]);
                    }

                    //mGrid.g_DArray[i].TaniName = bbl.Z_SetStr(row["TaniName"]);   // CheckGridでセット
                    mGrid.g_DArray[i].CommentInStore = row["CommentInStore"].ToString();   // 
                    mGrid.g_DArray[i].CommentOutStore = row["CommentOutStore"].ToString();   // 
                    mGrid.g_DArray[i].IndividualClientName = row["IndividualClientName"].ToString();   // 

                    mGrid.g_DArray[i].ProfitGaku = bbl.Z_SetStr(row["D_ProfitGaku"]);   // 
                    
                    //税額(Hidden)
                    mGrid.g_DArray[i].Tax = bbl.Z_Set(mGrid.g_DArray[i].SalesGaku) - bbl.Z_Set(mGrid.g_DArray[i].SalesHontaiGaku);

                    //複写時以外
                    if (index != (int)EIndex.CopySalesNO)
                    {
                        //進捗チェック　既に売上済み,出荷済み,出荷指示済み,ピッキングリスト完了済み,仕入済み,入荷済み,発注済み警告
                        //bool ret = mibl.CheckJuchuDetailsData(dse.SalesNO, row["SalesRows"].ToString(), out string status, out string status2);
                        //if (ret)
                        //{
                        //    mGrid.g_DArray[i].Nyuka = status;
                        //    mGrid.g_DArray[i].Syukka = status2;
                        //}
                    }
                    mGrid.g_DArray[i].VendorCD = row["VendorCD"].ToString();
                    CheckGrid((int)ClsGridUriage.ColNO.VendorCD, i);
                    mGrid.g_DArray[i].PaymentPlanDate = row["PaymentPlanDate"].ToString();
                    mGrid.g_DArray[i].PurchaseNO = row["PurchaseNO"].ToString();
                    //mGrid.g_DArray[i].BillingNo = row["BillingNo"].ToString();
                    //mGrid.g_DArray[i].KeigenTax = bbl.Z_Set(row["CollectClearDate"]);


                    m_dataCnt = i + 1;
                    Grid_NotFocus((int)ClsGridUriage.ColNO.JanCD, i);
                    i++;
                }

                mOldSalesDate = detailControls[(int)EIndex.SalesDate].Text;

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
                case (int)EIndex.SalesDate:
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
            
                    //店舗の締日チェック
                    //店舗締マスターで判断
                    M_StoreClose_Entity msce = new M_StoreClose_Entity
                    {
                        StoreCD = CboStoreCD.SelectedValue.ToString(),
                        FiscalYYYYMM = detailControls[index].Text.Replace("/", "").Substring(0, 6)
                    };
                    bool ret = bbl.CheckStoreClose(msce, true, true, false, false, true);
                    if (!ret)
                    {
                        return false;
                    }

                    //売上日が変更された場合のチェック処理
                    if (mOldSalesDate != detailControls[index].Text)
                    {
                        for (int i = (int)EIndex.StaffCD; i <= (int)EIndex.COUNT-1; i++)
                            if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                                if (!CheckDependsOnDate(i, true))
                                    return false;

                        //明細部JANCDの再チェック
                        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                        {
                            if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                            {
                                if (CheckGrid((int)ClsGridUriage.ColNO.JanCD, RW, true, true) == false)
                                {
                                    //Focusセット処理
                                    ERR_FOCUS_GRID_SUB((int)ClsGridUriage.ColNO.JanCD, RW);
                                    return false;
                                }
                            }
                        }
                        mOldSalesDate = detailControls[index].Text;
                        ScCustomer.ChangeDate = mOldSalesDate;
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
                        //顧客名2に入力無い場合、先頭20Byteを顧客名2へセット   
                        if (string.IsNullOrWhiteSpace(detailControls[index + 1].Text))
                        {
                            detailControls[index + 1].Text = bbl.LeftB(detailControls[index].Text, 20);
                        }
                    }
                    break;

                case (int)EIndex.CustomerName2:
                    //入力可能の場合 入力必須(Entry required)
                    if (detailControls[index].Enabled && string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
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
            w_Ret = mGrid.F_MoveFocus((int)ClsGridUriage.Gen_MK_FocusMove.MvSet, (int)ClsGridUriage.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        /// <summary>
        /// 日付が変更されたときに必要なチェック処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDependsOnDate(int index, bool ChangeDate = false)
        {
            string ymd = detailControls[(int)EIndex.SalesDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            switch (index)
            {
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
                            ClearCustomerInfo(0);
                            detailControls[index].Focus();
                            return false;
                        }

                        //顧客CDが変更されている場合のみ再セット
                        if (mOldCustomerCD != detailControls[index].Text || ChangeDate)
                        {
                            detailControls[index + 1].Text = mce.CustomerName;
                            textBox1.Text = mce.RemarksInStore;
                            textBox2.Text = mce.RemarksOutStore;
                            CboPaymentMethodCD.SelectedValue = mce.PaymentMethodCD;

                            if (mce.VariousFLG == "1")
                            {
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                {
                                    detailControls[index + 1].Enabled = true;
                                    detailControls[index + 2].Enabled = true;
                                }
                                detailControls[index + 2].Text = "";
                            }
                            else
                            {
                                detailControls[index + 1].Enabled = false;
                                detailControls[index + 2].Text = "";
                                detailControls[index + 2].Enabled = false;
                            }

                            mBillingType = mce.BillingType;
                            mBillingCD = mce.BillingCD;
                            mPaymentMethodCD = mce.PaymentMethodCD;
                            mTaxFractionKBN = Convert.ToInt16(mce.TaxFractionKBN);
                            mTaxTiming = Convert.ToInt16(mce.TaxTiming);

                            //回収予定日←Fnc_PlanDateよりout予定日をSet
                            Fnc_PlanDate_Entity fpe = new Fnc_PlanDate_Entity();
                            fpe.KaisyuShiharaiKbn = "0";    // "0：回収		
                            fpe.CustomerCD = mBillingCD;    //請求CD(Hidden)
                            fpe.ChangeDate = ymd;
                            fpe.TyohaKbn = "0";

                            mCollectPlanDate = bbl.Fnc_PlanDate(fpe);
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(0);
                        detailControls[index].Focus();
                        return false;
                    }
                    
                    mOldCustomerCD = detailControls[index].Text;    //位置確認
                    break;
            }

            return true;
        }
        private bool CheckGrid(int col, int row, bool chkAll=false, bool changeYmd=false)
        {
            bool ret = false;

            string ymd = detailControls[(int)EIndex.SalesDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridUriage.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                        return false;
                }
            }

            switch (col)
            {
                case (int)ClsGridUriage.ColNO.JanCD:
                    //販売単価 複写元売上番号が入力されている場合は、以下のメッセージを表示後、その回答によって扱いを変える
                    if (!string.IsNullOrWhiteSpace(keyControls[(int)EIndex.CopySalesNO].Text))
                    {
                        //基本は再計算しない
                        mGrid.g_DArray[row].NotReCalc = true;

                        if (!chkAll && mGrid.g_DArray[row].copyJuchuGyoNO > 0)
                        {
                            //「複写する売上の金額情報をコピーしますか？」
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
                        mGrid.g_DArray[row].ColorName = selectRow["ColorName"].ToString();
                        mGrid.g_DArray[row].SizeName = selectRow["SizeName"].ToString();
                        mGrid.g_DArray[row].DiscountKbn = Convert.ToInt16(selectRow["DiscountKbn"].ToString());
                        mGrid.g_DArray[row].TaniCD = selectRow["TaniCD"].ToString();
                        mGrid.g_DArray[row].TaniName = selectRow["TaniName"].ToString();
                        mGrid.g_DArray[row].TaxRateFLG = Convert.ToInt16(selectRow["TaxRateFLG"].ToString());

                        mGrid.g_DArray[row].AdminNO = selectRow["AdminNO"].ToString();
                        mGrid.g_DArray[row].VariousFLG = Convert.ToInt16(selectRow["VariousFLG"].ToString());

                        if (OperationMode == EOperationMode.INSERT)
                            mGrid.g_DArray[row].VendorCD = selectRow["MainVendorCD"].ToString();
                        else
                        {
                            mGrid.g_DArray[row].VendorCD = mVendorCD;

                            //支払予定日←Fnc_PlanDateよりout予定日をSet
                            Fnc_PlanDate_Entity fpe = new Fnc_PlanDate_Entity();
                            fpe.KaisyuShiharaiKbn = "1";    // "1";1：支払		
                            fpe.CustomerCD = mPayeeCD;    //支払先CD(Hidden)
                            fpe.ChangeDate = ymd;
                            fpe.TyohaKbn = "0";

                            mGrid.g_DArray[row].PaymentPlanDate = bbl.Fnc_PlanDate(fpe);
                        }
                        mGrid.g_DArray[row].ZaikoKBN = Convert.ToInt16(selectRow["ZaikoKBN"].ToString());
                        mGrid.g_DArray[row].MakerItem = selectRow["MakerItem"].ToString();

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

                        decimal wSuu = bbl.Z_Set(mGrid.g_DArray[row].SalesSuu);


                        if (mGrid.g_DArray[row].NotReCalc != true)
                        {
                            //Function_単価取得.
                            Fnc_UnitPrice_Entity fue = new Fnc_UnitPrice_Entity
                            {
                                AdminNo = mGrid.g_DArray[row].AdminNO,
                                ChangeDate = ymd,
                                CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                                StoreCD = CboStoreCD.SelectedValue.ToString(),
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
                                        mGrid.g_DArray[row].SalesUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeikomiTanka));
                                        break;
                                    case 1:
                                        //販売単価=Function_単価取得.out税抜単価
                                        mGrid.g_DArray[row].SalesUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeinukiTanka));
                                        break;
                                }

                                //原価単価=Function_単価取得.out原価単価	
                                mGrid.g_DArray[row].CostUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka));
                            }
                            else
                            {
                                //販売単価		
                                mGrid.g_DArray[row].SalesUnitPrice = "0";
                                //原価単価
                                mGrid.g_DArray[row].CostUnitPrice = "0";
                            }

                            //	(Form.売上数＝Null	の場合は×１とする)
                            if (wSuu.Equals(0))
                                wSuu = 1;

                            SetSalesGaku(row, wSuu, ymd, fue.ZeinukiTanka);
                            
                            //原価額=Form.売上数≠Nullの場合Function_単価取得.out原価単価×Form.Detail.売上数
                            mGrid.g_DArray[row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].CostUnitPrice) * wSuu);

                            //粗利額=⑦Form.税抜販売額－⑩Form.原価額				
                            mGrid.g_DArray[row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].SalesHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[row].CostGaku));

                        }

                        mGrid.g_DArray[row].OldJanCD = mGrid.g_DArray[row].JanCD;

                        //仕入先、店舗との組み合わせでITEM発注単価マスターかJAN発注単価マスタに存在すること SelectできなければError
                        //以下①②③④の順番でターゲットが大きくなる（①に近いほど、商品が特定されていく）
                        mGrid.g_DArray[row].OrderUnitPrice = GetTanka(row, ymd);
                        mGrid.g_DArray[row].OrderGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].OrderUnitPrice) * bbl.Z_Set(mGrid.g_DArray[row].SalesSuu));

                        //０で無いかつ原価単価＝０の場合、入力された発注単価を原価単価にセットし、原価金額、粗利金額を再計算。
                        if (bbl.Z_Set(mGrid.g_DArray[row].OrderUnitPrice) != 0 && bbl.Z_Set(mGrid.g_DArray[row].CostUnitPrice) == 0)
                        {
                            mGrid.g_DArray[row].CostUnitPrice = mGrid.g_DArray[row].OrderUnitPrice;
                            mGrid.g_DArray[row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].CostUnitPrice) * wSuu);
                        }

                        CalcZei(row);
                        Grid_NotFocus(col, row);

                    }

                    break;

                case (int)ClsGridUriage.ColNO.SKUName:
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

                case (int)ClsGridUriage.ColNO.VendorCD:

                    if (mGrid.g_MK_State[col, row].Cell_Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].VendorCD))
                        {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                            mGrid.g_DArray[row].VendorName = "";
                            return false;
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
                            mGrid.g_DArray[row].PayeeCD = me.PayeeCD;
                            mGrid.g_DArray[row].TaxTiming = me.TaxTiming;
                            mGrid.g_DArray[row].TaxFractionKBN = Convert.ToInt16(me.TaxFractionKBN);
                        }
                        else
                        {
                            //Ｅ１０１  
                            bbl.ShowMessage("E101");
                            mGrid.g_DArray[row].VendorName = "";
                            return false;
                        }
                        //支払先CD(Hidden)が、仕入先マスター(M_Vendor)に存在することSelectできなければError
                        me.VendorCD = me.PayeeCD;
                        ret = vebl.M_Vendor_SelectForPayeeCD(me);

                        if (ret)
                        {
                            //支払予定日←Fnc_PlanDateよりout予定日をSet
                            Fnc_PlanDate_Entity fpe = new Fnc_PlanDate_Entity();
                            fpe.KaisyuShiharaiKbn = "1";    // "1";1：支払		
                            fpe.CustomerCD = me.PayeeCD;    //支払先CD(Hidden)
                            fpe.ChangeDate = ymd;
                            fpe.TyohaKbn = "0";

                            mGrid.g_DArray[row].PaymentPlanDate = bbl.Fnc_PlanDate(fpe);
                        }
                        //締処理済の場合（以下のSelectができる場合）Error
                        //【D_PayCloseHistory】
                        D_PayCloseHistory_Entity dpe = new D_PayCloseHistory_Entity
                        {
                            PayeeCD = me.PayeeCD,
                            PayCloseDate = detailControls[(int)EIndex.SalesDate].Text
                        };
                         ret = mubl.CheckPayCloseHistory(dpe);
                        if (ret)
                        {
                            //Ｅ１７７
                            bbl.ShowMessage("E177");
                            return false;
                        }
                    }
                    break;

                case (int)ClsGridUriage.ColNO.OrderUnitPrice://仕入単価
                    //入力無くても良い(It is not necessary to input)
                    //入力無い場合、0とする（When there is no input, it is set to 0）
                    decimal orderUnitPrice = bbl.Z_Set(mGrid.g_DArray[row].OrderUnitPrice);
                    mGrid.g_DArray[row].OrderUnitPrice = bbl.Z_SetStr(orderUnitPrice);
                    mGrid.g_DArray[row].OrderGaku = bbl.Z_SetStr(orderUnitPrice * bbl.Z_Set(mGrid.g_DArray[row].SalesSuu));

                    //０の場合				メッセージ表示
                    if (orderUnitPrice.Equals(0))
                    {
                        if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                            return false;
                    }
                    //０で無いかつ原価単価＝０の場合場合、入力された発注単価を原価単価にセットし、原価金額、粗利金額を再計算。
                    else if (bbl.Z_Set(mGrid.g_DArray[row].CostUnitPrice) == 0)
                    {
                        mGrid.g_DArray[row].CostUnitPrice = mGrid.g_DArray[row].OrderUnitPrice;
                        mGrid.g_DArray[row].CostGaku = string.Format("{0:#,##0}", orderUnitPrice * bbl.Z_Set(mGrid.g_DArray[row].SalesSuu));
                    }

                    CalcZei(row, true);
                    break;

            }
            switch (col)
            {
                case (int)ClsGridUriage.ColNO.SalesSU:
                    if (mGrid.g_DArray[row].NotPrintFLG)
                    {
                        //新規時は仕入先毎にチェックする
                        int chkRow = row;
                        string su = "";
                        string sircd = mGrid.g_DArray[row].VendorCD;
                        for (int rw = row-1; rw >= 0; rw--)
                        {
                            if (mGrid.g_DArray[rw].VendorCD.Equals(sircd) && !mGrid.g_DArray[rw].NotPrintFLG)
                            {
                                chkRow = rw;
                                su = mGrid.g_DArray[rw].SalesSuu;
                                break;
                            }
                        }
                        if(chkRow.Equals(row))
                        {
                            //一行目にはチェックできない
                            mGrid.g_DArray[row].NotPrintFLG = false;
                        }
                        if (!mGrid.g_DArray[row].SalesSuu.Equals(su))
                        {
                            //Ｅ２１７				
                            bbl.ShowMessage("E217");
                            return false;
                        }
                    }
                    if (mGrid.g_DArray[row].DiscountKbn.Equals(1))
                    {
                        //売上数≧０の場合、Error				
                        if (bbl.Z_Set(mGrid.g_DArray[row].SalesSuu) >= 0)
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

                case (int)ClsGridUriage.ColNO.JanCD:                
                case (int)ClsGridUriage.ColNO.SalesUnitPrice: //販売単価 
                case (int)ClsGridUriage.ColNO.CostUnitPrice: //原価単価
                case (int)ClsGridUriage.ColNO.OrderUnitPrice://発注単価
                    //各金額項目の再計算必要
                    if (chkAll == false)
                        CalcKin();

                        break;

            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        private string GetTanka(int row, string ymd)
        {
            //[M_JANOrderPrice]
            M_JANOrderPrice_Entity mje = new M_JANOrderPrice_Entity
            {

                //①JAN発注単価マスタ（店舗指定なし）
                AdminNO = mGrid.g_DArray[row].AdminNO,
                VendorCD = mGrid.g_DArray[row].VendorCD,
                StoreCD = CboStoreCD.SelectedValue.ToString(),
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
                        StoreCD = CboStoreCD.SelectedValue.ToString()
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
    
        private void SetSalesGaku(int row, decimal wSuu, string ymd, string fZeinukiTanka)
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
                            //税抜販売額＝Function_消費税計算.out金額１×Form.Detail.売上数
                            decimal tanka = bbl.GetZeinukiKingaku(bbl.Z_Set(mGrid.g_DArray[row].SalesUnitPrice), mGrid.g_DArray[row].TaxRateFLG, ymd);

                            mGrid.g_DArray[row].SalesHontaiGaku = string.Format("{0:#,##0}", tanka * wSuu);
                        }
                        else if (mGrid.g_DArray[row].VariousFLG.Equals(0))
                        {
                            //税抜販売額＝Form.売上数＝Nullの場合Function_単価取得.out税抜単価×Form.売上数
                            mGrid.g_DArray[row].SalesHontaiGaku = string.Format("{0:#,##0}", bbl.Z_Set(fZeinukiTanka) * wSuu);
                        }
                        //税込販売額=Form.売上数≠Nullの場合Function_単価取得.out税込単価×Form.Detail.売上数	
                        mGrid.g_DArray[row].SalesGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].SalesUnitPrice) * wSuu);
                    }
                    break;
                case 1:
                    {
                        //税抜販売額
                        if (mGrid.g_DArray[row].VariousFLG.Equals(1))
                        {
                            //税抜販売額←	Form.Detail.販売単価×	Form.Detail.売上数	Form.売上数＝Nullの場合は×１とする		
                            mGrid.g_DArray[row].SalesHontaiGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].SalesUnitPrice) * wSuu);
                        }
                        else if (mGrid.g_DArray[row].VariousFLG.Equals(0))
                        {
                            //税抜販売額←Function_単価取得.out税抜単価×Form.Detail.売上数		Form.売上数＝Nullの場合は×１とする	
                            mGrid.g_DArray[row].SalesHontaiGaku = string.Format("{0:#,##0}", bbl.Z_Set(fZeinukiTanka) * wSuu);
                        }
                        //税込販売額=Function_消費税計算.out金額１                                  
                        mGrid.g_DArray[row].SalesGaku = string.Format("{0:#,##0}", bbl.GetZeikomiKingaku(bbl.Z_Set(mGrid.g_DArray[row].SalesHontaiGaku), mGrid.g_DArray[row].TaxRateFLG, out decimal zei, ymd));
                    }
                    break;
            }
        }

        private void CalcZei(int w_Row, bool changeTanka = false)
        {
            string ymd = detailControls[(int)EIndex.SalesDate].Text;
            decimal wSuu = bbl.Z_Set(mGrid.g_DArray[w_Row].SalesSuu);
            decimal wTanka = bbl.Z_Set(mGrid.g_DArray[w_Row].SalesUnitPrice);

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
                StoreCD = CboStoreCD.SelectedValue.ToString(),
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
            mGrid.g_DArray[w_Row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].SalesHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].CostGaku));

            if (mGrid.g_DArray[w_Row].TaxRateFLG == 1)
            {
                mGrid.g_DArray[w_Row].TaxRateDisp = "税込";

                if (mGrid.g_DArray[w_Row].VariousFLG.Equals(1) || changeTanka)
                {
                    //通常税額=税込販売額－税抜販売額
                    mGrid.g_DArray[w_Row].JuchuTax = bbl.Z_Set(mGrid.g_DArray[w_Row].SalesGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].SalesHontaiGaku);
                }
                else if (mGrid.g_DArray[w_Row].VariousFLG.Equals(0))
                {
                    //通常税額←TaxRateFLG＝1の時のFunction_単価取得.out消費税額×Form.Detail.売上数
                    mGrid.g_DArray[w_Row].JuchuTax = bbl.Z_Set(fue.Zei) * wSuu;
                }

                mGrid.g_DArray[w_Row].KeigenTax = 0;
            }
            else if (mGrid.g_DArray[w_Row].TaxRateFLG == 2)
            {
                mGrid.g_DArray[w_Row].TaxRateDisp = "税込";
                mGrid.g_DArray[w_Row].JuchuTax = 0;
      
                if (mGrid.g_DArray[w_Row].VariousFLG.Equals(1) || changeTanka)
                {
                    //軽減税額=TaxRateFLG＝2の時の税込販売額－税抜販売額
                    mGrid.g_DArray[w_Row].KeigenTax = bbl.Z_Set(mGrid.g_DArray[w_Row].SalesGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].SalesHontaiGaku);
                }
                else if (mGrid.g_DArray[w_Row].VariousFLG.Equals(0))
                {
                    //軽減税額←TaxRateFLG＝2の時のFunction_単価取得.out消費税額×Form.Detail.見積数
                    mGrid.g_DArray[w_Row].KeigenTax = bbl.Z_Set(fue.Zei) * wSuu;
                }
            }
            else
            {
                mGrid.g_DArray[w_Row].TaxRateDisp = "非税";
                mGrid.g_DArray[w_Row].JuchuTax = 0;
                mGrid.g_DArray[w_Row].KeigenTax = 0;
            }

            //仕入税額の算出
            //消費税額(Hidden)←Function_消費税計算.out金額１	
            decimal zei;
            decimal zeiritsu;
            decimal zeikomi = bbl.GetZeikomiKingaku(bbl.Z_Set(mGrid.g_DArray[w_Row].OrderGaku), mGrid.g_DArray[w_Row].TaxRateFLG, out zei, out zeiritsu, ymd);
            //mGrid.g_DArray[w_Row].OrderTax = zei;
            mGrid.g_DArray[w_Row].OrderTax = 0;
            mGrid.g_DArray[w_Row].KeigenOrderTax = 0;

            //通常税率仕入額(Hidden)M_SKU.TaxRateFLG＝1	の時の仕入額
            if (mGrid.g_DArray[w_Row].TaxRateFLG == 1)
            {
                mGrid.g_DArray[w_Row].OrderTax = zei;
            }
            //軽減税率仕入額(Hidden)M_SKU.TaxRateFLG＝2	の時の仕入額
            else if (mGrid.g_DArray[w_Row].TaxRateFLG == 2)
            {
                mGrid.g_DArray[w_Row].KeigenOrderTax = zei;
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
            decimal sumZei10 = 0;       //Form.Detail.発注通常税額のTotal
            decimal sumOrKin = 0;       //Form.Detail.発注額のTotal
            int maxOrderKinRowNo = 0;
            decimal maxOrderKin = 0;

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                {
                    //仕入額
                    mGrid.g_DArray[RW].OrderGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[RW].OrderUnitPrice) * bbl.Z_Set(mGrid.g_DArray[RW].SalesSuu));

                    //粗利額←⑦Form.Detail.税抜販売額－⑩Form.Detail.原価額
                    mGrid.g_DArray[RW].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[RW].CostGaku));

                    if (mGrid.g_DArray[RW].DiscountKbn == 0)
                        kin1 += bbl.Z_Set(mGrid.g_DArray[RW].SalesGaku);
                    else
                        kin5 += bbl.Z_Set(mGrid.g_DArray[RW].SalesGaku);

                    ////Form.Detail.値引区分＝１ の Form.Detail.税抜販売額  ←Form.Detail.税率＝10%の明細が対象
                    ////－Form.Detail.値引区分＝０ の Form.Detail.税抜販売額  										
                    //if (mGrid.g_DArray[RW].DiscountKbn == 1 && mGrid.g_DArray[RW].TaxRateFLG == 1)
                    //    kin2 -= bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku);
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 0 && mGrid.g_DArray[RW].TaxRateFLG == 1)
                    //    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku);
                    ////＋（Form.Detail.値引区分＝１ の Form.Detail.税抜販売額 ←Form.Detail.税率＝８%の明細が対象
                    ////－	Form.Detail.値引区分＝０ の Form.Detail.税抜販売額  
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 1 && mGrid.g_DArray[RW].TaxRateFLG == 2)
                    //    kin2 -= bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku);
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 0 && mGrid.g_DArray[RW].TaxRateFLG == 2)
                    //    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku);
                    ////＋（Form.Detail.値引区分＝１ の Form.Detail.税抜販売額 ←Form.Detail.税率＝それ以外の明細が対象
                    ////－	Form.Detail.値引区分＝０ の Form.Detail.税抜販売額
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 1)
                    //    kin2 -= bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku);
                    //else if (mGrid.g_DArray[RW].DiscountKbn == 0)
                    //    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku);

                    //税抜売上額=Form.Detail.税抜販売額のTotal
                    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku);
                    zei10 += bbl.Z_Set(mGrid.g_DArray[RW].JuchuTax);
                    zei8 += bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax);
                    kin3 += bbl.Z_Set(mGrid.g_DArray[RW].CostGaku);
                    kin4 += bbl.Z_Set(mGrid.g_DArray[RW].ProfitGaku);

                    //M_Customer.TaxTiming＝2:伝票ごと
                    if (mTaxTiming.Equals(2))
                    {
                        if (mGrid.g_DArray[RW].TaxRateFLG.Equals(1))
                        {
                            kin10 += bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku);
                            if (zeiritsu10 == 0 && !string.IsNullOrWhiteSpace( mGrid.g_DArray[RW].TaxRate))
                                zeiritsu10 = Convert.ToInt16(mGrid.g_DArray[RW].TaxRate.Replace("%", ""));
                        }
                        else if (mGrid.g_DArray[RW].TaxRateFLG.Equals(2))
                        {
                            kin8 += bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku);
                            if (zeiritsu8 == 0 && !string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].TaxRate))
                                zeiritsu8 = Convert.ToInt16(mGrid.g_DArray[RW].TaxRate.Replace("%", ""));
                        }

                        if (maxKin < bbl.Z_Set(mGrid.g_DArray[RW].SalesGaku) && mGrid.g_DArray[RW].DiscountKbn == 0)
                        {
                            maxKin = bbl.Z_Set(mGrid.g_DArray[RW].SalesGaku);
                            maxKinRowNo = RW;
                        }
                    }
                    
                    //M_Vendor.TaxTiming(1:明細ごと 2:伝票ごと 3:締ごと)
                    if (!string.IsNullOrWhiteSpace( mGrid.g_DArray[RW].TaxTiming) && mGrid.g_DArray[RW].TaxTiming.Equals(2) && !string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].VendorCD))
                    {
                        decimal zeiShiire10 = 0;      //発注通常税額(Hidden)
                        decimal zeiShiire8 = 0;      //発注軽減税額(Hidden)
                        string sirCd = mGrid.g_DArray[RW].VendorCD;
                        for (int sRw = 0; sRw <= mGrid.g_MK_Max_Row - 1; sRw++)
                        {
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[sRw].JanCD) == false && sirCd.Equals(mGrid.g_DArray[sRw].VendorCD))
                            {
                                if (mGrid.g_DArray[sRw].TaxRateFLG.Equals(1))
                                {
                                    kinOrder10 += bbl.Z_Set(mGrid.g_DArray[sRw].OrderGaku);
                                }
                                else if (mGrid.g_DArray[sRw].TaxRateFLG.Equals(2))
                                {
                                    kinOrder8 += bbl.Z_Set(mGrid.g_DArray[sRw].OrderGaku);
                                }
                                if (maxOrderKin < bbl.Z_Set(mGrid.g_DArray[sRw].OrderGaku))
                                {
                                    maxOrderKin = bbl.Z_Set(mGrid.g_DArray[sRw].OrderGaku);
                                    maxOrderKinRowNo = sRw;
                                }

                                zeiShiire10 += bbl.Z_Set(mGrid.g_DArray[sRw].OrderTax);
                                zeiShiire8 += bbl.Z_Set(mGrid.g_DArray[sRw].KeigenOrderTax);
                            }
                        }

                        kinOrder10 = GetResultWithHasuKbn(mGrid.g_DArray[RW].TaxFractionKBN, kinOrder10 * zeiritsu10 / 100);
                        kinOrder8 = GetResultWithHasuKbn(mGrid.g_DArray[RW].TaxFractionKBN, kinOrder8 * zeiritsu8 / 100);

                        decimal siireSagaku = (kinOrder10 + kinOrder8) - (zeiShiire10 + zeiShiire8);
                        if (siireSagaku != 0)
                        {
                            //※消費税差額＝通常税額(Hidden) ＋ 軽減税額(Hidden)）－ SUM（Form.Detail.通常税額 ＋ Form.Detail.軽減税額）	

                            //明細税込販売額に足し込む。
                            mGrid.g_DArray[maxOrderKinRowNo].OrderGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[maxOrderKinRowNo].OrderGaku) + siireSagaku);

                            //明細消費税額に足し込む。
                            if (mGrid.g_DArray[maxOrderKinRowNo].TaxRateFLG.Equals(1))
                            {
                                mGrid.g_DArray[maxOrderKinRowNo].OrderTax = mGrid.g_DArray[maxOrderKinRowNo].OrderTax + siireSagaku;
                            }
                            else if (mGrid.g_DArray[maxOrderKinRowNo].TaxRateFLG.Equals(2))
                            {
                                mGrid.g_DArray[maxOrderKinRowNo].KeigenOrderTax = mGrid.g_DArray[maxOrderKinRowNo].KeigenOrderTax + siireSagaku;
                            }
                        }
                    }

                    zeiOrder10 += bbl.Z_Set(mGrid.g_DArray[RW].OrderTax);       //差額込みの仕入税額
                    zeiOrder8 += bbl.Z_Set(mGrid.g_DArray[RW].KeigenOrderTax); //差額込みの仕入税額
                    sumOrKin += bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku);        //差額込みの仕入額
                }
            }

            //Footer部
            //値引き額
            lblKin5.Text = string.Format("{0:#,##0}", kin5);
            //税込売上額＝税込売上額－値引額
            lblKin6.Text = string.Format("{0:#,##0}", kin1+kin5);
            //税抜売上額・Form.Detail.税抜販売額のTotal
            lblKin2.Text = string.Format("{0:#,##0}", kin2);
            //mSalesHontaiGaku10 = kin10;
            //mSalesHontaiGaku8 = kin8;
            //mSalesHontaiGaku0 = kin2- kin8 - kin10;

            //原価額・Form.Detail.原価額のTotal
            lblKin3.Text = string.Format("{0:#,##0}", kin3);
            //粗利額・税抜売上額－原価額
            lblKin4.Text = string.Format("{0:#,##0}", kin2- kin3);
            //仕入額
            lblKin7.Text = string.Format("{0:#,##0}", sumOrKin);
            //仕入税額
            lblKin8.Text = string.Format("{0:#,##0}", zeiOrder10 + zeiOrder8);

            //M_Control.Tennic＝0 の場合、
            //M_Control.Tennic＝1 の場合、M_Customer.TaxTiming＝1:明細ごと または 3:締ごと	
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
                //lblKin13.Text = string.Format("{0:#,##0}", sumOrKin);
            }
            //M_Customer.TaxTiming＝2:伝票ごと
            else
            {
                //通常税額(Hidden)=Form.Detail.税抜販売額のTotal×Form.Detail.税率	※端数処理はM_Customerの設定に準ずる		
                //←M_SKU.TaxRateFLG＝1:通常課税の明細が対象  
                kin10 = GetResultWithHasuKbn(mTaxFractionKBN, kin10 * zeiritsu10 / 100);
                kin8 = GetResultWithHasuKbn(mTaxFractionKBN, kin8 * zeiritsu8 / 100);

                decimal sagaku = (kin10 + kin8) - (zei10 + zei8);
                //通常税額(Hidden) ＋ 軽減税額(Hidden)　≠　SUM（Form.Detail.通常税額＋Form.Detail.軽減税額）の場合、
                //以下の計算結果「消費税差額」を明細金額が一番大きい金額（全明細が同じ金額の場合は１行目）の明細税込販売額、明細消費税額に足し込む。
                if (sagaku != 0)
                {
                    //※消費税差額＝通常税額(Hidden) ＋ 軽減税額(Hidden)）－ SUM（Form.Detail.通常税額 ＋ Form.Detail.軽減税額）	

                    //明細税込販売額に足し込む。
                    mGrid.g_DArray[maxKinRowNo].SalesGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[maxKinRowNo].SalesGaku) + sagaku);

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
                //税込売上額・Form.Detail.税込販売額のTotal
                //lblKin1.Text = string.Format("{0:#,##0}", kin1 + sagaku);
                //税込売上額＝税込売上額－値引額
                lblKin6.Text = string.Format("{0:#,##0}", kin1 + sagaku + kin5);
                //消費税額
                lblKin10.Text = string.Format("{0:#,##0}", kin10 + kin8);
                //通常税額(Hidden)・Form.Detail.通常税額のTotal
                lblKin11.Text = string.Format("{0:#,##0}", kin10);
                mZei10 = kin10;
                //軽減税額(Hidden)・Form.Detail.軽減税額のTotal
                lblKin12.Text = string.Format("{0:#,##0}", kin8);
                mZei8 = kin8;

            }
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Sales_Entity GetEntity()
        {
            dse = new D_Sales_Entity();
            dse.SalesNO = keyControls[(int)EIndex.SalesNO].Text;
            dse.PurchaseNO = mOldPurchaseNO;
            dse.BillingNO = mOldBillingNo;
            dse.BillingCD = mBillingCD;
            dse.CollectPlanDate = mCollectPlanDate;
            dse.PaymentPlanDate = mPaymentPlanDate;
            dse.StoreCD = CboStoreCD.SelectedValue.ToString();

            dse.SalesDate = detailControls[(int)EIndex.SalesDate].Text;

            if(ckM_CheckBox1.Checked)
                dse.ReturnFlg = "1";
            else
                dse.ReturnFlg = "0";
            
            dse.StaffCD = detailControls[(int)EIndex.StaffCD].Text;
            dse.CustomerCD = detailControls[(int)EIndex.CustomerCD].Text;
            dse.CustomerName = detailControls[(int)EIndex.CustomerName].Text;
            dse.CustomerName2 = detailControls[(int)EIndex.CustomerName2].Text;
            dse.BillingType = ckM_CheckBox1.Checked ? "1" : mBillingType;
            dse.Discount = bbl.Z_SetStr(lblKin5.Text);
            dse.SalesHontaiGaku = lblKin2.Text;// - (mZei8 + mZei10)).ToString();
            //dse.SalesHontaiGaku10 = lblKin2.Text;
            //dse.SalesHontaiGaku8 = lblKin2.Text;
            //dse.SalesHontaiGaku0 = lblKin2.Text;
            dse.SalesTax8 = mZei8.ToString();
            dse.SalesTax10 = mZei10.ToString();
            dse.SalesGaku = lblKin6.Text;
            dse.CostGaku = bbl.Z_SetStr(lblKin3.Text);
            dse.ProfitGaku = bbl.Z_SetStr(lblKin4.Text);

            dse.PaymentMethodCD = CboPaymentMethodCD.SelectedValue.ToString();           
            dse.NouhinsyoComment = detailControls[(int)EIndex.NouhinsyoComment].Text;

            dse.InsertOperator = InOperatorCD;
            dse.PC = InPcID;
            //dje.OrderHontaiGaku = (bbl.Z_Set(lblKin13.Text) - (mOrderTax8 + mOrderTax10)).ToString();
            //dje.OrderTax8 = mOrderTax8.ToString();
            //dje.OrderTax10 = mOrderTax10.ToString();
            //dje.OrderGaku = bbl.Z_SetStr(lblKin13.Text);

            return dse;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("SalesRows", typeof(int));
            dt.Columns.Add("DisplayRows", typeof(int));
            dt.Columns.Add("SiteSalesRows", typeof(int));
            dt.Columns.Add("NotPrintFLG", typeof(int));
            dt.Columns.Add("AddSalesRows", typeof(int));

            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("MakerItem", typeof(string));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));

            dt.Columns.Add("DiscountKbn", typeof(int));
            dt.Columns.Add("SalesSuu", typeof(int));
            dt.Columns.Add("SalesUnitPrice", typeof(decimal));
            dt.Columns.Add("TaniCD", typeof(string));
            dt.Columns.Add("SalesGaku", typeof(decimal));
            dt.Columns.Add("SalesHontaiGaku", typeof(decimal));
            dt.Columns.Add("SalesTax", typeof(decimal));
            dt.Columns.Add("SalesTaxRitsu", typeof(decimal));
            dt.Columns.Add("CostUnitPrice", typeof(decimal));
            dt.Columns.Add("CostGaku", typeof(decimal));
            dt.Columns.Add("ProfitGaku", typeof(decimal));

            dt.Columns.Add("VendorCD", typeof(string));
            dt.Columns.Add("PaymentPlanDate", typeof(DateTime));
            dt.Columns.Add("CommentOutStore", typeof(string));
            dt.Columns.Add("CommentInStore", typeof(string));
            dt.Columns.Add("IndividualClientName", typeof(string));
            //dt.Columns.Add("ZaikoKBN", typeof(int));
            dt.Columns.Add("PayeeCD", typeof(string));
            dt.Columns.Add("OrderUnitPrice", typeof(decimal));
            dt.Columns.Add("OrderTax", typeof(decimal));
            dt.Columns.Add("OrderKeigenTax", typeof(decimal));
            dt.Columns.Add("OrderGaku", typeof(decimal));

            dt.Columns.Add("PurchaseNO", typeof(string));
            //dt.Columns.Add("BillingNO", typeof(string));
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

                    int notPrintFLG = 0;
                    if (mGrid.g_DArray[RW].NotPrintFLG)
                    {
                        notPrintFLG = 1;
                    }
                    else
                    {
                        AddJuchuuRows = mGrid.g_DArray[RW].salesGyoNO > 0 ? mGrid.g_DArray[RW].salesGyoNO : rowNo;
                    }

                    dt.Rows.Add(mGrid.g_DArray[RW].salesGyoNO > 0 ? mGrid.g_DArray[RW].salesGyoNO : rowNo
                        , mGrid.g_DArray[RW].GYONO
                        , 0
                        , notPrintFLG
                        , notPrintFLG == 1 ? AddJuchuuRows : 0
                        , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                        , mGrid.g_DArray[RW].SKUCD == "" ? null : mGrid.g_DArray[RW].SKUCD
                        , mGrid.g_DArray[RW].JanCD == "" ? null : mGrid.g_DArray[RW].JanCD
                        , mGrid.g_DArray[RW].MakerItem == "" ? null : mGrid.g_DArray[RW].MakerItem
                        , mGrid.g_DArray[RW].SKUName == "" ? null : mGrid.g_DArray[RW].SKUName
                        , mGrid.g_DArray[RW].ColorName == "" ? null : mGrid.g_DArray[RW].ColorName
                        , mGrid.g_DArray[RW].SizeName == "" ? null : mGrid.g_DArray[RW].SizeName

                        , mGrid.g_DArray[RW].DiscountKbn
                        , bbl.Z_Set(mGrid.g_DArray[RW].SalesSuu)
                        , bbl.Z_Set(mGrid.g_DArray[RW].SalesUnitPrice)
                        , mGrid.g_DArray[RW].TaniCD == "" ? null : mGrid.g_DArray[RW].TaniCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].SalesGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].SalesHontaiGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].JuchuTax) + bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].TaxRate.Replace("%", ""))     //税率
                        , bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].CostGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].ProfitGaku)

                        , mGrid.g_DArray[RW].VendorCD == "" ? null : mGrid.g_DArray[RW].VendorCD
                        , mGrid.g_DArray[RW].PaymentPlanDate == "" ? null : mGrid.g_DArray[RW].PaymentPlanDate
                        , mGrid.g_DArray[RW].CommentOutStore == "" ? null : mGrid.g_DArray[RW].CommentOutStore
                        , mGrid.g_DArray[RW].CommentInStore == "" ? null : mGrid.g_DArray[RW].CommentInStore
                        , mGrid.g_DArray[RW].IndividualClientName == "" ? null : mGrid.g_DArray[RW].IndividualClientName
                        //, bbl.Z_Set(mGrid.g_DArray[RW].ZaikoKBN)

                        , mGrid.g_DArray[RW].PayeeCD == "" ? null : mGrid.g_DArray[RW].PayeeCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].OrderUnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].OrderTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].KeigenOrderTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku)
                        , mGrid.g_DArray[RW].PurchaseNO == "" ? null : mGrid.g_DArray[RW].PurchaseNO
                        //, mGrid.g_DArray[RW].BillingNo == "" ? null : mGrid.g_DArray[RW].BillingNo
                        , mGrid.g_DArray[RW].salesGyoNO > 0 ? 1:0
                        );

                    if(mGrid.g_DArray[RW].salesGyoNO ==0)
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

                dse = new D_Sales_Entity();
                dse.SalesNO = keyControls[(int)EIndex.SalesNO].Text;

                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                    {
                        for (int CL = (int)ClsGridUriage.ColNO.JanCD; CL < (int)ClsGridUriage.ColNO.COUNT; CL++)
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

            }

            DataTable dt = GetGridEntity();

            //更新処理
            dse = GetEntity();
            mubl.Sales_Exec(dse, dt, (short)OperationMode);

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");

            if (ChkPrint.Checked　&& OperationMode != EOperationMode.DELETE)
            {
                if (bbl.ShowMessage("Q202") == DialogResult.Yes)
                {
                    string[] sno = dse.SalesNO.Split(',');
                    foreach(string no in sno)
                        if(!string.IsNullOrWhiteSpace(no))
                            //印刷Program(TempoNouhinsyo)を起動
                            ExecPrint(no);
                }
            }

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
                
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
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
            //顧客情報ALLクリア
            ClearCustomerInfo(0);

            foreach (Control ctl in detailLabels)
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }

            mOldSalesDate = "";
            S_Clear_Grid();   //画面クリア（明細部）
            
            lblKin2.Text = "";
            lblKin3.Text = "";
            lblKin4.Text = "";
            lblKin5.Text = "";
            lblKin6.Text = "";
            lblKin7.Text = "";
            lblKin8.Text = "";
            lblKin10.Text = "";
            lblKin11.Text = "";
            lblKin12.Text = "";


            mOrderTaxTiming = 0;
        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo(short kbn)
        {
            mOldCustomerCD = "";
            mTaxFractionKBN = 0;
            mTaxTiming = 0;

            ScCustomer.LabelText = "";
            detailControls[(int)EIndex.CustomerName].Text = "";
            detailControls[(int)EIndex.CustomerName2].Text = "";
            detailControls[(int)EIndex.CustomerName].Enabled = false;
            textBox1.Text = "";
            textBox2.Text = "";
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
                                        break;
                                    default:
                                        detailControls[idx].Enabled = Kbn == 0 ? true : false;
                                        break;
                                }

                            }
                            for (int index = 0; index < searchButtons.Length; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;
                            
                            Pnl_Body.Enabled = Kbn == 0 ? true : false;
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

                    if(w_Col == (int)ClsGridUriage.ColNO.JanCD)
                        //商品検索
                        kbn = EsearchKbn.Product;
                    else if (w_Col == (int)ClsGridUriage.ColNO.VendorCD)
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


                    using (Search_Product frmProduct = new Search_Product(detailControls[(int)EIndex.SalesDate].Text))
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

                            CheckGrid((int)ClsGridUriage.ColNO.JanCD, w_Row, false, true);

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
                            detailControls[(int)EIndex.SalesDate].Focus();
                        }
                        else if (index == (int)EIndex.SalesNO)
                        {
                            if (OperationMode == EOperationMode.UPDATE)
                            {
                                detailControls[(int)EIndex.SalesDate].Focus();
                            }
                        }
                        else if (index == (int)EIndex.CopySalesNO)
                        {
                            detailControls[(int)EIndex.SalesDate].Focus();
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
                       if( index == (int)EIndex.CustomerName2 || (index == (int)EIndex.CustomerName && !detailControls[index + 1].CanFocus) || (index == (int)EIndex.CustomerCD && !detailControls[index + 1].CanFocus) )
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridUriage.ColNO.JanCD);
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

                if (w_Col == (int)ClsGridUriage.ColNO.NotPrintFLG)
                {
                    bool Check = mGrid.g_DArray[w_Row].NotPrintFLG;

                    if (w_Row == 0 && Check)
                        mGrid.g_DArray[w_Row].NotPrintFLG = false;

                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JanCD))
                        mGrid.g_DArray[w_Row].NotPrintFLG = false;

                    //新規時は仕入先毎にチェックする
                    int chkRow = w_Row;
                    string su = "";
                    string sircd = mGrid.g_DArray[w_Row].VendorCD;
                    for (int rw = w_Row - 1; rw >= 0; rw--)
                    {
                        if (mGrid.g_DArray[rw].VendorCD.Equals(sircd) && !mGrid.g_DArray[rw].NotPrintFLG)
                        {
                            chkRow = rw;
                            su = mGrid.g_DArray[rw].SalesSuu;
                            break;
                        }
                    }
                    if (chkRow.Equals(w_Row))
                    {
                        //一行目にはチェックできない
                        mGrid.g_DArray[w_Row].NotPrintFLG = false;
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

                ////明細が出荷済の売上明細、または、売上済の売上明細である場合、削除できない（F7ボタンを使えないようにする）
                ////出荷済・売上済の場合は行削除不可
                //if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].Syukka))
                //{
                //    if (mGrid.g_DArray[w_Row].Syukka.Equals("出荷済") || mGrid.g_DArray[w_Row].Syukka.Equals("売上済"))
                //        Btn_F8.Text = "";
                //    else
                //        Btn_F8.Text = "行削除(F8)";
                //}
                //else
                //    Btn_F8.Text = "行削除(F8)";

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
                            CL = (int)ClsGridUriage.ColNO.JanCD;
                            break;
                        case "IMT_ITMNM":
                            CL = (int)ClsGridUriage.ColNO.SKUName;
                            break;
                        case "CHK_PRINT":
                            CL = (int)ClsGridUriage.ColNO.NotPrintFLG;
                            break;
                        //case "IMT_SPDAT":
                        //    CL = (int)ClsGridUriage.ColNO.PaymentPlanDate;
                        //    break;
                        //case "IMN_ORGAK":
                        //    CL = (int)ClsGridJuchuu.ColNO.OrderGaku;
                        //    break;
                        case "IMN_CLINT":
                            CL = (int)ClsGridUriage.ColNO.ColorName;
                            break;
                        case "IMT_KAIDT":
                            CL = (int)ClsGridUriage.ColNO.SizeName;
                            break;
                        case "IMN_GENER":
                            CL = (int)ClsGridUriage.ColNO.SalesSU;
                            break;
                        case "IMN_GENER2":
                            CL = (int)ClsGridUriage.ColNO.SalesUnitPrice;
                            break;
                        case "IMT_VENCD":
                            CL = (int)ClsGridUriage.ColNO.VendorCD;
                            break;
                        case "IMN_ORTAN":
                            CL = (int)ClsGridUriage.ColNO.OrderUnitPrice;
                            break;
                        case "IMT_ARIDT":
                            CL = (int)ClsGridUriage.ColNO.OrderGaku;
                            break;
                        case "IMN_SALEP2":
                            CL = (int)ClsGridUriage.ColNO.CostUnitPrice;
                            break;
                        case "IMT_REMAK":
                            CL = (int)ClsGridUriage.ColNO.CommentInStore;
                            break;
                        case "IMN_WEBPR":
                            CL = (int)ClsGridUriage.ColNO.CommentOutStore;
                            break;
                        case "IMN_WEBPR2":
                            CL = (int)ClsGridUriage.ColNO.IndividualClientName;
                            //if (w_Row == m_dataCnt - 1)
                            if (w_Row == mGrid.g_MK_Max_Row - 1)
                                lastCell = true;
                            break;

                    }

                    bool changeFlg = false;
                    switch (CL)
                    {
                        case (int)ClsGridUriage.ColNO.SalesSU:
                            if (!mGrid.g_DArray[w_Row].SalesSuu.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridUriage.ColNO.SalesUnitPrice: //販売単価
                            if (!mGrid.g_DArray[w_Row].SalesUnitPrice.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridUriage.ColNO.OrderUnitPrice: //仕入単価
                            if (!mGrid.g_DArray[w_Row].OrderUnitPrice.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridUriage.ColNO.CostUnitPrice: //原価単価
                            if (!mGrid.g_DArray[w_Row].CostUnitPrice.Equals(w_ActCtl.Text))
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
                        decimal wSuu = bbl.Z_Set(mGrid.g_DArray[w_Row].SalesSuu);
                        string ymd = detailControls[(int)EIndex.SalesDate].Text;
                        decimal wTanka;

                        switch (CL)
                        {
                            case (int)ClsGridUriage.ColNO.SalesSU:
                                //数量が整数値かどうかチェック
                                int intSu;
                                if (!int.TryParse(wSuu.ToString(), out intSu))
                                {
                                    bbl.ShowMessage("E102");
                                    return;
                                }

                                //入力された場合、金額再計算
                                //if (wSuu != 0)
                                //{
                                    //Function_単価取得.
                                    Fnc_UnitPrice_Entity fue = new Fnc_UnitPrice_Entity
                                    {
                                        AdminNo = mGrid.g_DArray[w_Row].AdminNO,
                                        ChangeDate = ymd,
                                        CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                                        StoreCD = CboStoreCD.SelectedValue.ToString(),
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
                                                mGrid.g_DArray[w_Row].SalesUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeikomiTanka));
                                                break;
                                            case 1:
                                                //販売単価=Function_単価取得.out税抜単価
                                                mGrid.g_DArray[w_Row].SalesUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeinukiTanka));
                                                break;
                                        }

                                        //原価単価=Function_単価取得.out原価単価	
                                        mGrid.g_DArray[w_Row].CostUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka));
                                    }
                                    else
                                    {
                                        //販売単価		
                                        mGrid.g_DArray[w_Row].SalesUnitPrice = "0";
                                        //原価単価
                                        mGrid.g_DArray[w_Row].CostUnitPrice = "0";
                                    }

                                    SetSalesGaku(w_Row, wSuu, ymd, fue.ZeinukiTanka);

                                    //０で無いかつ原価単価＝０の場合、入力された発注単価を原価単価にセットし、原価金額、粗利金額を再計算。
                                    if (bbl.Z_Set(mGrid.g_DArray[w_Row].OrderUnitPrice) != 0 && bbl.Z_Set(mGrid.g_DArray[w_Row].CostUnitPrice) == 0)
                                    {
                                        mGrid.g_DArray[w_Row].CostUnitPrice = mGrid.g_DArray[w_Row].OrderUnitPrice;
                                    }

                                //原価額←原価単価×Form.Detail.売上数
                                mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].CostUnitPrice) * wSuu);
                                mGrid.g_DArray[w_Row].OrderGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].OrderUnitPrice) * wSuu);

                                    CalcZei(w_Row);
                                //}
                                break;

                            case (int)ClsGridUriage.ColNO.SalesUnitPrice: //販売単価 
                                {
                                    string tanka = mGrid.g_DArray[w_Row].SalesUnitPrice;
                                    if (mTennic.Equals(0))
                                        tanka = bbl.GetZeinukiKingaku(bbl.Z_Set(mGrid.g_DArray[w_Row].SalesUnitPrice), mGrid.g_DArray[w_Row].TaxRateFLG, ymd).ToString();

                                    SetSalesGaku(w_Row, wSuu, ymd, tanka);

                                    CalcZei(w_Row, true);
                                }
                                break;

                            case (int)ClsGridUriage.ColNO.CostUnitPrice: //原価単価
                                //原価額=Form.Detail.原価単価×	Form.Detail.売上数
                                mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].CostUnitPrice) * bbl.Z_Set(mGrid.g_DArray[w_Row].SalesSuu));

                                //粗利額
                                mGrid.g_DArray[w_Row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].SalesHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].CostGaku)); // 

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
                        case (int)ClsGridUriage.ColNO.NotPrintFLG:
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
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecPrint(string no)
        {
            //Form.印刷CheckBox＝onの場合、印刷Program(TempoNouhinsyo)を起動					
            //EXEが存在しない時ｴﾗｰ
            // 実行モジュールと同一フォルダのファイルを取得
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + TempoNouhinsyo;
            if (System.IO.File.Exists(filePath))
            {
                //売上番号,起動区分＝１					
                string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + no + " 1";
                System.Diagnostics.Process.Start(filePath, cmdLine);
            }
            else
            {
                //ファイルなし
            }
        }
        private void CboStoreCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CboStoreCD.SelectedIndex > 0)
                {
                    ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
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

            if (enabled)
            {

            }
            else
            {
                detailControls[(int)EIndex.CustomerName].Text = "";
                detailControls[(int)EIndex.CustomerName2].Text = "";
            }
        }

    }
}








