using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace HenpinNyuuryoku
{
    /// <summary>
    /// HenpinNyuuryoku 返品入力
    /// </summary>
    internal partial class HenpinNyuuryoku : FrmMainForm
    {
        private const string ProID = "HenpinNyuuryoku";
        private const string ProNm = "返品入力";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            PurchaseNO,
            StoreCD,

            CalledVendorCD = 0,
            PurchaseDate,
            PaymentPlanDate,
            StaffCD,

            ArrivalDateFrom,
            ArrivalDateTo,
            Maker,
            VendorDeliveryNo,
            RemarksInStore,
            //AdjustmentGaku,
            PurchaseTax
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product
        }

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private HenpinNyuuryoku_BL snbl;
        private D_Purchase_Entity dpe;
        private D_Stock_Entity dse;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldPurchaseNO = "";    //排他処理のため使用
        private string mOldPurchaseDate = "";
        private string mOldVendorCD = "";
        private string InStoreCD = "";

        private string mPayeeCD = "";
        private string mTaxTiming;

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridShiire mGrid = new ClsGridShiire();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_MaxPurchaseGyoNo;

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

            if (ClsGridShiire.gc_P_GYO <= ClsGridShiire.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridShiire.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridShiire.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridShiire.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridShiire.gc_MaxCL;

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
                            //sctl.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
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
                            check.Click += new System.EventHandler(CHK_Del_Click);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridShiire.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridShiire.ColNO.GYONO; i <= (int)ClsGridShiire.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridShiire.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridShiire.ColNO.PurchaseSu:
                        case (int)ClsGridShiire.ColNO.ReturnPlanSu:
                            mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                            break;
                        case (int)ClsGridShiire.ColNO.PurchaserUnitPrice:
                        case (int)ClsGridShiire.ColNO.AdjustmentGaku:
                        case (int)ClsGridShiire.ColNO.PurchaseGaku:
                        case (int)ClsGridShiire.ColNO.CalculationGaku:
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
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 0].CellCtl = CHK_DELCK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Chk, 0].CellCtl = CHK_DELCK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.JanCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SizeName, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Space, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ReturnPlanSu, 0].CellCtl = IMN_TEIKA2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseSu, 0].CellCtl = IMN_GENER_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaserUnitPrice, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ColorName, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.AdjustmentGaku, 0].CellCtl = IMN_SALEP2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentOutStore, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentInStore, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.MakerItem, 0].CellCtl = IMT_JUONO_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ExpectReturnDate, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CalculationGaku, 0].CellCtl = IMN_CALGK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseGaku, 0].CellCtl = IMN_SIRGK_0;

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 1].CellCtl = CHK_DELCK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Chk, 1].CellCtl = CHK_DELCK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.JanCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SizeName, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Space, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ReturnPlanSu, 1].CellCtl = IMN_TEIKA2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseSu, 1].CellCtl = IMN_GENER_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaserUnitPrice, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ColorName, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.AdjustmentGaku, 1].CellCtl = IMN_SALEP2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentOutStore, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentInStore, 1].CellCtl = IMT_REMAK_1;
            
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.MakerItem, 1].CellCtl = IMT_JUONO_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ExpectReturnDate, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CalculationGaku, 1].CellCtl = IMN_CALGK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseGaku, 1].CellCtl = IMN_SIRGK_1;
            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 2].CellCtl = CHK_DELCK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Chk, 2].CellCtl = CHK_DELCK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.JanCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SizeName, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Space, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ReturnPlanSu, 2].CellCtl = IMN_TEIKA2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseSu, 2].CellCtl = IMN_GENER_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaserUnitPrice, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ColorName, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.AdjustmentGaku, 2].CellCtl = IMN_SALEP2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentOutStore, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentInStore, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.MakerItem, 2].CellCtl = IMT_JUONO_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ExpectReturnDate, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CalculationGaku, 2].CellCtl = IMN_CALGK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseGaku, 2].CellCtl = IMN_SIRGK_2;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 3].CellCtl = CHK_DELCK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Chk, 3].CellCtl = CHK_DELCK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.JanCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SizeName, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Space, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ReturnPlanSu, 3].CellCtl = IMN_TEIKA2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseSu, 3].CellCtl = IMN_GENER_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaserUnitPrice, 3].CellCtl = IMN_GENER2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ColorName, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.AdjustmentGaku, 3].CellCtl = IMN_SALEP2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentOutStore, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentInStore, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.MakerItem, 3].CellCtl = IMT_JUONO_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ExpectReturnDate, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CalculationGaku, 3].CellCtl = IMN_CALGK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseGaku, 3].CellCtl = IMN_SIRGK_3;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 4].CellCtl = CHK_DELCK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Chk, 4].CellCtl = CHK_DELCK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.JanCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SizeName, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Space, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ReturnPlanSu, 4].CellCtl = IMN_TEIKA2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseSu, 4].CellCtl = IMN_GENER_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaserUnitPrice, 4].CellCtl = IMN_GENER2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ColorName, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.AdjustmentGaku, 4].CellCtl = IMN_SALEP2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentOutStore, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentInStore, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.MakerItem, 4].CellCtl = IMT_JUONO_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ExpectReturnDate, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CalculationGaku, 4].CellCtl = IMN_CALGK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseGaku, 4].CellCtl = IMN_SIRGK_4;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 5].CellCtl = CHK_DELCK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Chk, 5].CellCtl = CHK_DELCK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.JanCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SizeName, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Space, 5].CellCtl = IMN_TEIKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ReturnPlanSu, 5].CellCtl = IMN_TEIKA2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseSu, 5].CellCtl = IMN_GENER_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaserUnitPrice, 5].CellCtl = IMN_GENER2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ColorName, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.AdjustmentGaku, 5].CellCtl = IMN_SALEP2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentOutStore, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentInStore, 5].CellCtl = IMT_REMAK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.MakerItem, 5].CellCtl = IMT_JUONO_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ExpectReturnDate, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CalculationGaku, 5].CellCtl = IMN_CALGK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseGaku, 5].CellCtl = IMN_SIRGK_5;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 6].CellCtl = CHK_DELCK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Chk, 6].CellCtl = CHK_DELCK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.JanCD, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SizeName, 6].CellCtl = IMT_KAIDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SKUName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Space, 6].CellCtl = IMN_TEIKA_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ReturnPlanSu, 6].CellCtl = IMN_TEIKA2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseSu, 6].CellCtl = IMN_GENER_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaserUnitPrice, 6].CellCtl = IMN_GENER2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ColorName, 6].CellCtl = IMN_CLINT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.AdjustmentGaku, 6].CellCtl = IMN_SALEP2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentOutStore, 6].CellCtl = IMN_WEBPR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentInStore, 6].CellCtl = IMT_REMAK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.MakerItem, 6].CellCtl = IMT_JUONO_6;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ExpectReturnDate, 6].CellCtl = IMT_ARIDT_6;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CalculationGaku, 6].CellCtl = IMN_CALGK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseGaku, 6].CellCtl = IMN_SIRGK_6;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 7].CellCtl = CHK_DELCK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Chk, 7].CellCtl = CHK_DELCK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.JanCD, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SizeName, 7].CellCtl = IMT_KAIDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SKUName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Space, 7].CellCtl = IMN_TEIKA_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ReturnPlanSu, 7].CellCtl = IMN_TEIKA2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseSu, 7].CellCtl = IMN_GENER_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaserUnitPrice, 7].CellCtl = IMN_GENER2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ColorName, 7].CellCtl = IMN_CLINT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.AdjustmentGaku, 7].CellCtl = IMN_SALEP2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentOutStore, 7].CellCtl = IMN_WEBPR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentInStore, 7].CellCtl = IMT_REMAK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.MakerItem, 7].CellCtl = IMT_JUONO_7;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ExpectReturnDate, 7].CellCtl = IMT_ARIDT_7;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CalculationGaku, 7].CellCtl = IMN_CALGK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseGaku, 7].CellCtl = IMN_SIRGK_7;

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 8].CellCtl = CHK_DELCK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Chk, 8].CellCtl = CHK_DELCK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.JanCD, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SizeName, 8].CellCtl = IMT_KAIDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.SKUName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.Space, 8].CellCtl = IMN_TEIKA_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ReturnPlanSu, 8].CellCtl = IMN_TEIKA2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseSu, 8].CellCtl = IMN_GENER_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaserUnitPrice, 8].CellCtl = IMN_GENER2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ColorName, 8].CellCtl = IMN_CLINT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.AdjustmentGaku, 8].CellCtl = IMN_SALEP2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentOutStore, 8].CellCtl = IMN_WEBPR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CommentInStore, 8].CellCtl = IMT_REMAK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.MakerItem, 8].CellCtl = IMT_JUONO_8;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.ExpectReturnDate, 8].CellCtl = IMT_ARIDT_8;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.CalculationGaku, 8].CellCtl = IMN_CALGK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShiire.ColNO.PurchaseGaku, 8].CellCtl = IMN_SIRGK_8;
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridShiire.Gen_MK_FocusMove.MvNxt, (int)ClsGridShiire.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridShiire.Gen_MK_FocusMove.MvPrv, (int)ClsGridShiire.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridShiire.Gen_MK_FocusMove.MvNxt, (int)ClsGridShiire.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridShiire.Gen_MK_FocusMove.MvSet, (int)ClsGridShiire.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridShiire.Gen_MK_FocusMove.MvSet, (int)ClsGridShiire.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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
                        ChangeBackColor(w_Row);                        
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
                    //    case (int)ClsGridShiire.ColNO.Chk:
                    //        {
                    //            // チェック
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
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                keyControls[(int)EIndex.PurchaseNO].Text = "";
                                keyControls[(int)EIndex.PurchaseNO].Enabled = false;
                                ScOrderNO.BtnSearch.Enabled = false;

                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                detailControls[(int)EIndex.PurchaseDate].Text = bbl.GetDate();
                                ScStaff.TxtCode.Text = InOperatorCD;
                                CheckDetail((int)EIndex.StaffCD);

                                Btn_SelectAll.Enabled = true;
                                Btn_NoSelect.Enabled = true;
                                btnSubF10.Enabled = true;
                                btnSubF11.Enabled = true;

                                SetFuncKeyAll(this, "111111001111");
                            }
                            else
                            {
                                keyControls[(int)EIndex.PurchaseNO].Enabled = true;
                                ScOrderNO.BtnSearch.Enabled = true;

                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                this.Vsb_Mei_0.TabStop = false;

                                Btn_SelectAll.Enabled = false;
                                Btn_NoSelect.Enabled = false;
                                btnSubF10.Enabled = false;
                                btnSubF11.Enabled = false;

                                SetFuncKeyAll(this, "111111001000");
                            }
                                                    }
                        CboStoreCD.SelectedValue = InStoreCD;

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

                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].StockNO))
                                {
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridShiire.ColNO.Chk:    // 
                                            if (OperationMode == EOperationMode.INSERT)
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                            }
                                            break;

                                        case (int)ClsGridShiire.ColNO.PurchaseSu:    // 
                                        case (int)ClsGridShiire.ColNO.PurchaserUnitPrice:    // 
                                        case (int)ClsGridShiire.ColNO.AdjustmentGaku:    // 
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
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


            if (pCol == (int)ClsGridShiire.ColNO.Chk)
            {
                //チェック時、ReadOnlyの列以外 使用可
                for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                {
                    if (!mGrid.g_DArray[pRow].Chk)
                    {
                        for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                        {
                            if (OperationMode == EOperationMode.INSERT)
                            {
                                if (w_Col == (int)ClsGridShiire.ColNO.Chk)
                                    //使用可
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                else
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                            }
                            else
                            {
                                switch (w_Col)
                                {
                                    case (int)ClsGridShiire.ColNO.PurchaseSu:
                                    case (int)ClsGridShiire.ColNO.PurchaserUnitPrice:
                                    case (int)ClsGridShiire.ColNO.AdjustmentGaku:
                                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                        break;
                                }
                            }
                        }

                        w_AllFlg = false;
                    }
                    else
                    {
                        //Chk入力時
                        w_AllFlg = true;
                        if (mGrid.g_DArray[pRow].Chk)
                        {
                            switch (w_Col)
                            {
                                case (int)ClsGridShiire.ColNO.Chk:
                                case (int)ClsGridShiire.ColNO.PurchaseSu:
                                case (int)ClsGridShiire.ColNO.PurchaserUnitPrice:
                                case (int)ClsGridShiire.ColNO.AdjustmentGaku:
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    break;
                            }
                        }
                    }
                }
                return;
            }
        }

        #endregion

        public HenpinNyuuryoku()
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

                // 明細部初期化
                this.S_SetInit_Grid();
                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();
                
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F10.Text = "納品ﾃﾞｰﾀ(F10)";
                Btn_F11.Text = "表示(F11)";

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                snbl = new HenpinNyuuryoku_BL();
                CboStoreCD.Bind(ymd);

                //検索用のパラメータ設定
                string stores= GetAllAvailableStores();
                ScOrderNO.Value1 = InOperatorCD;
                ScOrderNO.Value2 = stores;
                ScVendorCD.Value1 = "1";

                ScStaff.TxtCode.Text = InOperatorCD;

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = snbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                    InStoreCD = mse.StoreCD;
                    ScStaff.LabelText = mse.StaffName;
                }
                //他のプログラムから起動された場合、照会モードで起動
                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    string shiireNO = cmds[(int)ECmdLine.PcID + 1];   //
                    ChangeOperationMode(EOperationMode.UPDATE);
                    keyControls[(int)EIndex.PurchaseNO].Text = shiireNO;
                    CheckKey((int)EIndex.PurchaseNO, true);
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
            keyControls = new Control[] {  ScOrderNO.TxtCode,  CboStoreCD };
            keyLabels = new Control[] {  };
            detailControls = new Control[] { ScVendorCD.TxtCode, ckM_TextBox1, ckM_TextBox2, ScStaff.TxtCode
                         ,ckM_TextBox4, ckM_TextBox3,ScMaker.TxtCode, ckM_TextBox18                          
                         ,TxtRemark1, ckM_TextBox5 };
            detailLabels = new Control[] { ScVendorCD, ScStaff , ScMaker };
            searchButtons = new Control[] { ScVendorCD.BtnSearch, ScStaff.BtnSearch};

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
                case (int)EIndex.PurchaseNO:
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
            }

            return true;

        }

        private bool SelectAndInsertExclusive()
        {
            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT)
                return true;
            
            DeleteExclusive();

            if (string.IsNullOrWhiteSpace(keyControls[(int)EIndex.PurchaseNO].Text))
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Shiire,
                Number = keyControls[(int)EIndex.PurchaseNO].Text,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(),dt.Rows[0]["Operator"].ToString());
                keyControls[(int)EIndex.PurchaseNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldPurchaseNO = keyControls[(int)EIndex.PurchaseNO].Text;
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
       private void DeleteExclusive()
        {
            if (mOldPurchaseNO == "")
                return;

            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Shiire,
                Number = mOldPurchaseNO,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);

            mOldPurchaseNO = "";
        }
        
        /// <summary>
        /// 仕入データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set, int index= (int)EIndex.PurchaseNO)
        {
            //[D_Purchase_SelectDataF]
            dpe = new D_Purchase_Entity();
            dpe.PurchaseNO = keyControls[(int)EIndex.PurchaseNO].Text;

            DataTable dt = snbl.D_Purchase_SelectDataH(dpe, (short)OperationMode);

            //仕入(D_Purchase)に存在しない場合、Error 「登録されていない返品番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", "返品番号");
                Scr_Clr(1);
                previousCtrl.Focus();
                return false;
            }
            else
            {
                if(bbl.Z_Set(dt.Rows[0]["ProcessKBN"]) == 1)
                {
                    bbl.ShowMessage("E260");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }
                else if (bbl.Z_Set(dt.Rows[0]["ProcessKBN"]) ==2)
                {
                    bbl.ShowMessage("E259");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }
                //DeleteDateTime 「削除された返品番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", "返品番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                //権限がない場合（以下のSelectができない場合）Error　「権限のない返品番号」
                if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                {
                    bbl.ShowMessage("E139", "返品番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                if (index == (int)EIndex.PurchaseNO)
                {
                    //締処理済チェック　D_PayPlanに、支払締番号がセットされていれば、エラー（下記のSelectができたらエラー）
                    bool ret = snbl.CheckPayPlanData(dpe.PurchaseNO, out string errno);
                    if (ret)
                    {
                        if (!string.IsNullOrWhiteSpace(errno))
                        {
                            //Ｅ１７６　締処理済みの為、変更・削除できません
                            bbl.ShowMessage(errno);
                        }
                    }
                }

                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                //Errorでない場合、画面転送表01に従って画面表示

                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                m_dataCnt = 0;
                m_MaxPurchaseGyoNo = 0;

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
                        detailControls[(int)EIndex.PurchaseDate].Text = row["PurchaseDate"].ToString();
                        mOldPurchaseDate = detailControls[(int)EIndex.PurchaseDate].Text;

                        detailControls[(int)EIndex.StaffCD].Text = row["StaffCD"].ToString();
                        CheckDetail((int)EIndex.StaffCD);

                        detailControls[(int)EIndex.CalledVendorCD].Text = row["CalledVendorCD"].ToString();
                        CheckDetail((int)EIndex.CalledVendorCD);
                        detailControls[(int)EIndex.PaymentPlanDate].Text = row["PaymentPlanDate"].ToString();

                        //【Data Area Footer】
                        detailControls[(int)EIndex.RemarksInStore].Text = row["CommentInStore"].ToString();
                        detailControls[(int)EIndex.PurchaseTax].Text = bbl.Z_SetStr(row["PurchaseTax"]);

                        //lblKin1.Text = bbl.Z_SetStr(row["AdjustmentGaku"]);
                        lblKin2.Text = bbl.Z_SetStr(row["CalculationGaku"]);
                        lblKin3.Text = bbl.Z_SetStr(row["PurchaseGaku"]);
                        lblKin4.Text = bbl.Z_SetStr(row["AdjustmentGaku"]);

                        //明細なしの場合
                        if (bbl.Z_Set(row["PurchaseRows"]) == 0)
                            break;
                    }

                    mGrid.g_DArray[i].ExpectReturnDate = row["ExpectReturnDate"].ToString();
                    mGrid.g_DArray[i].MakerItem = row["MakerItem"].ToString();
                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                    mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();
                    mGrid.g_DArray[i].PurchaseSu = bbl.Z_SetStr(row["PurchaseSu"]);   //単価算出のため先にセットしておく    
                    mGrid.g_DArray[i].OldPurchaseSu = Convert.ToInt16(row["PurchaseSu"]);

                    CheckGrid((int)ClsGridShiire.ColNO.JanCD, i);

                    mGrid.g_DArray[i].TaniCD = row["TaniCD"].ToString();   // 
                    mGrid.g_DArray[i].TaniName = row["TaniName"].ToString();   // 
                    mGrid.g_DArray[i].SKUName = row["ItemName"].ToString();   // 
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 

                    mGrid.g_DArray[i].ReturnPlanSu = bbl.Z_SetStr(row["ReturnPlanSu"]);   // 
                    mGrid.g_DArray[i].PurchaserUnitPrice = bbl.Z_SetStr(row["PurchaserUnitPrice"]);
                    mGrid.g_DArray[i].AdjustmentGaku = bbl.Z_SetStr(bbl.Z_Set(row["D_AdjustmentGaku"]));   // 
                    mGrid.g_DArray[i].PurchaseGaku = bbl.Z_SetStr(row["D_PurchaseGaku"]);   // 
                    mGrid.g_DArray[i].CalculationGaku = bbl.Z_SetStr(row["D_CalculationGaku"]);   //        
                    mGrid.g_DArray[i].CommentInStore = row["Remark"].ToString();

                    //税額(Hidden)
                    mGrid.g_DArray[i].PurchaseTax = bbl.Z_Set(row["D_PurchaseTax"]);
                    mGrid.g_DArray[i].PurchaseRows = Convert.ToInt16(row["PurchaseRows"]);
                    mGrid.g_DArray[i].TaxRitsu = Convert.ToInt16(row["TaxRitsu"]);
                    mGrid.g_DArray[i].DeliveryNo = row["DeliveryNo"].ToString();
                    mGrid.g_DArray[i].OrderNO = row["OrderNO"].ToString();
                    mGrid.g_DArray[i].OrderRows = row["OrderRows"].ToString();
                    mGrid.g_DArray[i].WarehousingNO = row["WarehousingNO"].ToString();
                    mGrid.g_DArray[i].StockNO = row["StockNO"].ToString();
                    
                    if (m_MaxPurchaseGyoNo < mGrid.g_DArray[i].PurchaseRows)
                        m_MaxPurchaseGyoNo = mGrid.g_DArray[i].PurchaseRows;

                    m_dataCnt = i + 1;
                    Grid_NotFocus((int)ClsGridShiire.ColNO.Chk, i);
                    i++;
                }

                mOldPurchaseDate = detailControls[(int)EIndex.PurchaseDate].Text;

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }
            CalcKin();

            if (OperationMode == EOperationMode.UPDATE )
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            }
            //else if(OperationMode == EOperationMode.INSERT)
            //{
            //    //複写コピー後
            //    S_BodySeigyo(0, 1);
            //    //配列の内容を画面にセット
            //    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            //    detailControls[(int)EIndex.PurchaseDate].Focus();
            //}
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
            ExecDispNohin(0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kbn"></param>
        private void ExecDispNohin(short kbn)
        {
            //抽出条件エリアのエラーチェック
            for (int i = (int)EIndex.CalledVendorCD; i <= (int)EIndex.VendorDeliveryNo; i++)
            {
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            }

            dse = GetSearchInfo();
            dse.F10 = kbn;

            DataTable dt = new DataTable();
            dt = snbl.D_Stock_SelectAllForShiireH(dse);

            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E128");
                previousCtrl.Focus();
                return;
            }
            else
            {
                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                m_dataCnt = 0;

                //エラーが無ければ、画面転送表02に従って、Detail情報を表示
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //使用可能行数を超えた場合エラー
                        if (i > m_EnableCnt - 1)
                        {
                            bbl.ShowMessage("E178", m_EnableCnt.ToString());
                            mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                            return;
                        }

                        mGrid.g_DArray[i].ExpectReturnDate = row["ExpectReturnDate"].ToString();
                        mGrid.g_DArray[i].DeliveryNo = row["DeliveryNo"].ToString();
                        mGrid.g_DArray[i].MakerItem = row["MakerItem"].ToString();
                        mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                        mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                        mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();
                        mGrid.g_DArray[i].PurchaseSu = bbl.Z_SetStr(row["PurchaseSu"]);   //単価算出のため先にセットしておく    
                        mGrid.g_DArray[i].OldPurchaseSu = Convert.ToInt16(row["PurchaseSu"]);

                        CheckGrid((int)ClsGridShiire.ColNO.JanCD, i);

                        mGrid.g_DArray[i].TaniCD = row["TaniCD"].ToString();   // 
                        mGrid.g_DArray[i].TaniName = row["TaniName"].ToString();   // 
                        mGrid.g_DArray[i].SKUName = row["ItemName"].ToString();   // 
                        mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                        mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 

                        mGrid.g_DArray[i].ReturnPlanSu = bbl.Z_SetStr(row["ReturnPlanSu"]);   // 
                        mGrid.g_DArray[i].PurchaserUnitPrice = bbl.Z_SetStr(row["PurchaserUnitPrice"]);
                        mGrid.g_DArray[i].AdjustmentGaku = bbl.Z_SetStr(bbl.Z_Set(row["D_AdjustmentGaku"]));   // 
                        mGrid.g_DArray[i].PurchaseGaku = bbl.Z_SetStr(row["D_PurchaseGaku"]);   // 
                        mGrid.g_DArray[i].CalculationGaku = bbl.Z_SetStr(row["D_CalculationGaku"]);   // 

                        mGrid.g_DArray[i].CommentInStore = row["D_CommentInStore"].ToString();   // 
                        mGrid.g_DArray[i].CommentOutStore = row["D_CommentOutStore"].ToString();   // 

                        //Form.検索項目の「メーカー商品CD」＝Detail.メーカー商品CD(≠NULL) の時、CheckBoxをOn						
                        if (!string.IsNullOrWhiteSpace(row["MakerItem"].ToString()) && row["MakerItem"].ToString().Equals(detailControls[(int)EIndex.Maker].Text))
                            mGrid.g_DArray[i].Chk = true;
                        //Or Form.検索項目の「納品書番号」＝Detail.納品書番号(≠NULL)の時、CheckBoxをOn																																														
                        if (!string.IsNullOrWhiteSpace(row["DeliveryNo"].ToString()) && row["DeliveryNo"].ToString().Equals(detailControls[(int)EIndex.VendorDeliveryNo].Text))
                            mGrid.g_DArray[i].Chk = true;

                        if (mGrid.g_DArray[i].Chk)
                            ChangeBackColor(i, 1);

                        //Form.発注単価＝Form.単価の時(呼び出し時は常に同じ＝差異なし)

                        //税額(Hidden)
                        //mGrid.g_DArray[i].PurchaseTax = bbl.Z_Set(row["PurchaseTax"]);    計算が必要
                        mGrid.g_DArray[i].PurchaseRows = 0;     //Convert.ToInt16(row["PurchaseRows"]);
                        mGrid.g_DArray[i].OrderNO = row["OrderNO"].ToString();
                        mGrid.g_DArray[i].OrderRows = row["OrderRows"].ToString();
                        mGrid.g_DArray[i].StockNO = row["StockNO"].ToString();
                        mGrid.g_DArray[i].WarehousingNO = row["WarehousingNO"].ToString();
                        mGrid.g_DArray[i].TaxRitsu = Convert.ToInt16(row["TaxRitsu"]);

                        m_dataCnt = i + 1;
                        Grid_NotFocus((int)ClsGridShiire.ColNO.Chk, i);
                        i++;
                    }

                    mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

                }
                CalcKin();

                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridShiire.ColNO.Chk);
            }
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
                case (int)EIndex.PurchaseDate:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }

                    //締処理済の場合（以下のSelectができる場合）Error
                    //【D_PayCloseHistory】
                    D_PayCloseHistory_Entity dpe = new D_PayCloseHistory_Entity
                    {
                        PayeeCD = mPayeeCD,
                        PayCloseDate = detailControls[index].Text
                    };
                    bool ret = snbl.CheckPayCloseHistory(dpe);
                    if (ret)
                    {
                        //Ｅ１７７
                        bbl.ShowMessage("E177");
                        return false;
                    }

                    //店舗の締日チェック
                    //店舗締マスターで判断
                    M_StoreClose_Entity mste = new M_StoreClose_Entity
                    {
                        StoreCD = CboStoreCD.SelectedValue.ToString(),
                        FiscalYYYYMM = detailControls[index].Text.Replace("/", "").Substring(0, 6)
                    };
                    ret = bbl.CheckStoreClose(mste,false,true,false,false,true);
                    if (!ret)
                    {
                        return false;
                    }

                    //仕入日が変更された場合のチェック処理
                    if (mOldPurchaseDate != detailControls[index].Text)
                    {
                        for (int i = (int)EIndex.CalledVendorCD; i < (int)EIndex.StaffCD; i++)
                            if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                                if (!CheckDependsOnDate(i, true))
                                    return false;

                        mOldPurchaseDate = detailControls[index].Text;
                        ScVendorCD.ChangeDate = mOldPurchaseDate;
                    }

                    break;

                case (int)EIndex.PaymentPlanDate:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }
                    //仕入日以降であること
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.PurchaseDate].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        int result = detailControls[index].Text.CompareTo(detailControls[(int)EIndex.PurchaseDate].Text);
                        if (result < 0)
                        {
                            //Ｅ１４３「返品日より小さい値は入力できません」
                            bbl.ShowMessage("E143", "返品日", "小さい");
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.StaffCD:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    if (!CheckDependsOnDate(index))
                        return false;

                    break;
                    
                case (int)EIndex.CalledVendorCD:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //情報ALLクリア
                        ClearCustomerInfo((EIndex)index);
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                        return false;

                    break;

                case (int)EIndex.ArrivalDateFrom:
                case (int)EIndex.ArrivalDateTo:
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
                    //(From) ≧ (To)である場合Error
                    if (index==(int)EIndex.ArrivalDateTo && !string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                        if (result < 0)
                        {
                            bbl.ShowMessage("E104");
                            return false;
                        }
                    }

                    break;

                case (int)EIndex.Maker:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    string ymd = detailControls[(int)EIndex.PurchaseDate].Text;

                    if (string.IsNullOrWhiteSpace(ymd))
                        ymd = bbl.GetDate();


                    //[M_SKU]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        MakerItem = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    SKU_BL mbl = new SKU_BL();
                    ret  = mbl.M_SKU_SelectByMaker(mse);
                    if (ret)
                    {
                        ScMaker.LabelText = mse.SKUName;
                    }
                    else
                    {
                        ScMaker.LabelText = "";
                        //Ｅ１０１
                        bbl.ShowMessage("E101");
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

            w_Ctrl = detailControls[(int)EIndex.RemarksInStore];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridShiire.Gen_MK_FocusMove.MvSet, (int)ClsGridShiire.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        /// <summary>
        /// 日付が変更されたときに必要なチェック処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDependsOnDate(int index, bool ChangeDate = false)
        {
            string ymd = detailControls[(int)EIndex.PurchaseDate].Text;

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

                case (int)EIndex.CalledVendorCD:
                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        VendorFlg = "1",
                        ChangeDate = ymd
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    ret = sbl.M_Vendor_SelectTop1(mve);

                    if (ret)
                    {
                        if (mve.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            //顧客情報ALLクリア
                            ClearCustomerInfo((EIndex)index);
                            return false;
                        }

                            ScVendorCD.LabelText = mve.VendorName;
                            mPayeeCD = mve.PayeeCD;
                            mTaxTiming = mve.TaxTiming;
                            //mAmountFractionKBN = Convert.ToInt16(mve.AmountFractionKBN);
                            //mTaxFractionKBN = Convert.ToInt16(mve.TaxFractionKBN);

                            //支払先CD(Hidden)が、仕入先マスター(M_Vendor)に存在することSelectできなければError
                            mve.VendorCD = mPayeeCD;
                            ret = sbl.M_Vendor_SelectForPayeeCD(mve);

                            if (ret)
                            {
                                //(1:明細ごと 2:伝票ごと 3:締ごと)
                                if (mTaxTiming.Equals("1"))
                                {
                                    lblZei.Text = "明細単位";
                                }
                                else if (mTaxTiming.Equals("2"))
                                {
                                    lblZei.Text = "伝票単位";
                                }
                                else if (mTaxTiming.Equals("3"))
                                {
                                    lblZei.Text = "締単位";
                                }
                                else
                                {
                                    lblZei.Text = "";
                                }
                                //支払予定日←Fnc_PlanDateよりout予定日をSet
                                if (detailControls[index].Text != mOldVendorCD || ChangeDate)
                                {
                                    Fnc_PlanDate_Entity fpe = new Fnc_PlanDate_Entity();
                                    fpe.KaisyuShiharaiKbn = "1";    // "1";1：支払		
                                    fpe.CustomerCD = mPayeeCD;    //支払先CD(Hidden)
                                    fpe.ChangeDate = ymd;
                                    fpe.TyohaKbn = "0";

                                    detailControls[(int)EIndex.PaymentPlanDate].Text = bbl.Fnc_PlanDate(fpe);

                                    mOldVendorCD = detailControls[index].Text;
                                }
                            
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo((EIndex)index);
                        return false;
                    }
                    break;
            }

            return true;
        }
        private bool CheckGrid(int col, int row, bool chkAll=false, bool changeYmd=false)
        {
            string ymd = detailControls[(int)EIndex.PurchaseDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridShiire.gc_P_GYO && w_CtlRow >=0)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                        return false;
                }
            }

            switch (col)
            {
                case (int)ClsGridShiire.ColNO.JanCD:
                    //[M_SKU]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        JanCD = mGrid.g_DArray[row].JanCD,
                        AdminNO = mGrid.g_DArray[row].AdminNO,
                        ChangeDate = ymd
                    };

                    SKU_BL mbl = new SKU_BL();
                    DataTable dt = mbl.M_SKU_SelectAll(mse);
                    if (dt.Rows.Count > 0)
                    {
                        mGrid.g_DArray[row].DiscountKbn = Convert.ToInt16(dt.Rows[0]["DiscountKbn"].ToString());
                        mGrid.g_DArray[row].TaxRateFLG = Convert.ToInt16(dt.Rows[0]["TaxRateFLG"].ToString());
                        mGrid.g_DArray[row].VariousFLG = Convert.ToInt16(dt.Rows[0]["VariousFLG"].ToString());
                        mGrid.g_DArray[row].ZaikoKBN = Convert.ToInt16(dt.Rows[0]["ZaikoKBN"].ToString());
                    }
                    if (mGrid.g_DArray[row].TaxRateFLG == 1)
                    {
                        mGrid.g_DArray[row].TaxRateName = "税込";
                    }
                    else if (mGrid.g_DArray[row].TaxRateFLG == 2)
                    {
                        mGrid.g_DArray[row].TaxRateName = "税込";
                    }
                    else
                    {
                        mGrid.g_DArray[row].TaxRateName = "非税";
                    }

                    break;

                case (int)ClsGridShiire.ColNO.PurchaseSu:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].PurchaseSu))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    //当初表示された値（D_Stock.ReturnPlanSu －ReturnSu）より大きな値の入力はエラーにする
                    if(bbl.Z_Set(mGrid.g_DArray[row].PurchaseSu) > bbl.Z_Set(mGrid.g_DArray[row].OldPurchaseSu))
                    {
                        bbl.ShowMessage("E268");
                        return false;
                    }
                    break;

                case (int)ClsGridShiire.ColNO.PurchaserUnitPrice:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].PurchaserUnitPrice))
                    {
                        mGrid.g_DArray[row].PurchaserUnitPrice = "0";
                    }
                    Grid_NotFocus(col, row);

                    //配列の内容を画面へセット
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                    break;

                case (int)ClsGridShiire.ColNO.AdjustmentGaku:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].AdjustmentGaku))
                    {
                        //０を許す。
                        mGrid.g_DArray[row].AdjustmentGaku = "0";
                    }                    
                    break;
            }

            switch (col)
            {
                case (int)ClsGridShiire.ColNO.PurchaseSu:
                case (int)ClsGridShiire.ColNO.PurchaserUnitPrice: //単価 
                case (int)ClsGridShiire.ColNO.AdjustmentGaku: //調整額
                    //入力された場合、以下を再計算
                    //計算仕入額←	form.仕入数	×	form.仕入単価
                    mGrid.g_DArray[row].CalculationGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].PurchaseSu) * bbl.Z_Set(mGrid.g_DArray[row].PurchaserUnitPrice));
                    //計算仕入額＋調整額＝仕入額
                    mGrid.g_DArray[row].PurchaseGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].CalculationGaku) + bbl.Z_Set(mGrid.g_DArray[row].AdjustmentGaku)); 
                     //消費税額(Hidden)←Function_消費税計算.out金額１	
                     decimal zei;
                    decimal zeikomi = bbl.GetZeikomiKingaku(bbl.Z_Set(mGrid.g_DArray[row].PurchaseGaku), mGrid.g_DArray[row].TaxRateFLG,out zei, ymd);
                    mGrid.g_DArray[row].PurchaseTax = zei;
                    mGrid.g_DArray[row].PurchaseGaku10 = 0;
                    mGrid.g_DArray[row].PurchaseGaku8 = 0;

                    //通常税率仕入額(Hidden)M_SKU.TaxRateFLG＝1	の時の仕入額
                    if (mGrid.g_DArray[row].TaxRateFLG.Equals(1))
                    {
                        mGrid.g_DArray[row].PurchaseGaku10 =bbl.Z_Set( mGrid.g_DArray[row].PurchaseGaku);
                    }
                    //軽減税率仕入額(Hidden)M_SKU.TaxRateFLG＝2	の時の仕入額
                    else if (mGrid.g_DArray[row].TaxRateFLG.Equals(2))
                    {
                        mGrid.g_DArray[row].PurchaseGaku8 = bbl.Z_Set(mGrid.g_DArray[row].PurchaseGaku);
                    }

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
        /// Footer部 金額計算処理
        /// </summary>
        private void CalcKin(bool chkAll = false)
        {	
            decimal kin2 = 0;   //計算仕入額=Form.Detail.計算仕入額	のTotal
            decimal kin3 = 0;   //仕入額= Form.Detail.仕入額のTotal	
            decimal kin4 = 0;   //調整額= Form.Detail.調整額のTotal	
            decimal sumZei = 0;

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (mGrid.g_DArray[RW].Chk || OperationMode != EOperationMode.INSERT) 
                {
                    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].CalculationGaku);
                    kin3 += bbl.Z_Set(mGrid.g_DArray[RW].PurchaseGaku);
                    kin4 += bbl.Z_Set(mGrid.g_DArray[RW].AdjustmentGaku);
                    sumZei += bbl.Z_Set(mGrid.g_DArray[RW].PurchaseTax);
                }
            }

            //Footer部
            lblKin2.Text = string.Format("{0:#,##0}", kin2);
            lblKin3.Text = string.Format("{0:#,##0}", kin3);
            lblKin4.Text = string.Format("{0:#,##0}", kin4);

            if(!chkAll)
                detailControls[(int)EIndex.PurchaseTax].Text = string.Format("{0:#,##0}", sumZei);
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Purchase_Entity GetEntity()
        {
            dpe = new D_Purchase_Entity
            {
                PurchaseNO = keyControls[(int)EIndex.PurchaseNO].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString(),

                CalledVendorCD = detailControls[(int)EIndex.CalledVendorCD].Text,
                PayeeCD = mPayeeCD,
                PurchaseDate = detailControls[(int)EIndex.PurchaseDate].Text,
                PaymentPlanDate = detailControls[(int)EIndex.PaymentPlanDate].Text,
                StaffCD = detailControls[(int)EIndex.StaffCD].Text,

                CalculationGaku = bbl.Z_SetStr(lblKin2.Text),
                AdjustmentGaku =lblKin4.Text,
                PurchaseGaku = bbl.Z_SetStr(lblKin3.Text),
                PurchaseTax = detailControls[(int)EIndex.PurchaseTax].Text,

                ExpectedDateFrom = detailControls[(int)EIndex.ArrivalDateFrom].Text,
                ExpectedDateTo = detailControls[(int)EIndex.ArrivalDateTo].Text,

                CommentInStore = detailControls[(int)EIndex.RemarksInStore].Text,
                InsertOperator = InOperatorCD,
                PC = InPcID
            };

            return dpe;
        }
        private D_Stock_Entity GetSearchInfo()
        {
            dse = new D_Stock_Entity
            {
                ExpectReturnDateFrom = detailControls[(int)EIndex.ArrivalDateFrom].Text,
                ExpectReturnDateTo = detailControls[(int)EIndex.ArrivalDateTo].Text,
                VendorCD = detailControls[(int)EIndex.CalledVendorCD].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
            };
            return dse;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("PurchaseRows", typeof(int));
            dt.Columns.Add("DisplayRows", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("MakerName", typeof(string));
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("Remark", typeof(string));

            dt.Columns.Add("PurchaseSu", typeof(int));
            dt.Columns.Add("OldPurchaseSu", typeof(int));
            dt.Columns.Add("TaniCD", typeof(string));
            dt.Columns.Add("TaniName", typeof(string));

            dt.Columns.Add("PurchaserUnitPrice", typeof(decimal));
            dt.Columns.Add("CalculationGaku", typeof(decimal));
            dt.Columns.Add("AdjustmentGaku", typeof(decimal));
            dt.Columns.Add("PurchaseGaku", typeof(decimal));
            dt.Columns.Add("PurchaseTax", typeof(decimal));
            dt.Columns.Add("TaxRitsu", typeof(int));
            //dt.Columns.Add("CommentOutStore", typeof(string));
            //dt.Columns.Add("CommentInStore", typeof(string));
            //dt.Columns.Add("OrderUnitPrice", typeof(decimal));
            dt.Columns.Add("OrderNO", typeof(string));
            dt.Columns.Add("OrderRows", typeof(int));
            //dt.Columns.Add("DifferenceFlg", typeof(int));
            dt.Columns.Add("DeliveryNo", typeof(int));
            dt.Columns.Add("StockNO", typeof(string));
            dt.Columns.Add("WarehousingNO", typeof(int));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            if(OperationMode== EOperationMode.UPDATE)
            {
                rowNo = m_MaxPurchaseGyoNo + 1;
            }

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].StockNO))
                    break;

                if (mGrid.g_DArray[RW].Chk || OperationMode !=  EOperationMode.INSERT) 
                    {
                    dt.Rows.Add(mGrid.g_DArray[RW].PurchaseRows > 0 ? mGrid.g_DArray[RW].PurchaseRows : rowNo
                        , mGrid.g_DArray[RW].GYONO
                        , mGrid.g_DArray[RW].SKUCD == "" ? null : mGrid.g_DArray[RW].SKUCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                        , mGrid.g_DArray[RW].JanCD == "" ? null : mGrid.g_DArray[RW].JanCD
                        , mGrid.g_DArray[RW].MakerItem == "" ? null : mGrid.g_DArray[RW].MakerItem
                        , mGrid.g_DArray[RW].SKUName == "" ? null : mGrid.g_DArray[RW].SKUName
                        , mGrid.g_DArray[RW].ColorName == "" ? null : mGrid.g_DArray[RW].ColorName
                        , mGrid.g_DArray[RW].SizeName == "" ? null : mGrid.g_DArray[RW].SizeName
                        , mGrid.g_DArray[RW].CommentInStore == "" ? null : mGrid.g_DArray[RW].CommentInStore
                        , bbl.Z_Set(mGrid.g_DArray[RW].PurchaseSu)
                        , bbl.Z_Set(mGrid.g_DArray[RW].OldPurchaseSu)
                        , mGrid.g_DArray[RW].TaniCD == "" ? null : mGrid.g_DArray[RW].TaniCD
                        , mGrid.g_DArray[RW].TaniName == "" ? null : mGrid.g_DArray[RW].TaniName
                        , bbl.Z_Set(mGrid.g_DArray[RW].PurchaserUnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].CalculationGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].AdjustmentGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].PurchaseGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].PurchaseTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].TaxRitsu)
                        //, mGrid.g_DArray[RW].CommentOutStore == "" ? null : mGrid.g_DArray[RW].CommentOutStore
                        //, bbl.Z_Set(mGrid.g_DArray[RW].Space)
                        , mGrid.g_DArray[RW].OrderNO == "" ? null : mGrid.g_DArray[RW].OrderNO
                        , bbl.Z_Set(mGrid.g_DArray[RW].OrderRows)
                        //, DifferenceFlg
                        , mGrid.g_DArray[RW].DeliveryNo == "" ? null : mGrid.g_DArray[RW].DeliveryNo
                        , mGrid.g_DArray[RW].StockNO
                        , bbl.Z_Set(mGrid.g_DArray[RW].WarehousingNO)
                        , mGrid.g_DArray[RW].PurchaseRows > 0 ? 1:0
                        );

                    if(mGrid.g_DArray[RW].PurchaseRows == 0)
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
                for (int i = 0; i <= (int)EIndex.StaffCD; i++)
                    if (CheckDetail(i, false) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }

                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].StockNO) == false)
                    {
                        for (int CL = (int)ClsGridShiire.ColNO.Chk; CL < (int)ClsGridShiire.ColNO.COUNT; CL++)
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

                //各金額項目の再計算必要
                CalcKin(true);


                //※調整税額の確認
                //【Footer1】調整額の、明細合計値と入力値が異なる場合
                decimal sumKin = 0;
                decimal sumZei = 0;
                int maxGakuRow = 0;
                decimal maxGaku = 0;
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (mGrid.g_DArray[RW].Chk || OperationMode != EOperationMode.INSERT)
                    {
                        sumKin += bbl.Z_Set(mGrid.g_DArray[RW].AdjustmentGaku);
                        sumZei += bbl.Z_Set(mGrid.g_DArray[RW].PurchaseTax);

                        if (bbl.Z_Set(mGrid.g_DArray[RW].PurchaseGaku) > maxGaku)
                        {
                            maxGaku = bbl.Z_Set(mGrid.g_DArray[RW].PurchaseGaku);
                            maxGakuRow = RW;
                        }
                    }
                }
                //「はい」押下時のみ、以下の処理へ
                if (bbl.Z_Set(lblKin4.Text) != sumKin)
                {
                    //Ｑ３１４
                    if (bbl.ShowMessage("Q314") != DialogResult.Yes)
                        return;
                }
                //※消費税額の確認
                //【Footer1】税額の、計算値と入力値が異なる場合
                //「はい」押下時のみ、以下の処理へ
                if (bbl.Z_Set(detailControls[(int)EIndex.PurchaseTax].Text) != sumZei)
                {
                    //Ｑ３１０
                    if (bbl.ShowMessage("Q310") != DialogResult.Yes)
                        return;

                    //Form.Hedder.「税計算」が、伝票単位の場合
                    if (mTaxTiming.Equals("2"))
                    {
                        //  Form.Detail.消費税額(Hidden)のSUMと仕入ヘッダの消費税額が
                        //　異なる場合は、Form.TaxRateFLG(Hidden)≠0の仕入本体額が最大
                        //　（同額の場合は明細行の小さい方）に差額をセットする
                        mGrid.g_DArray[maxGakuRow].PurchaseTax += bbl.Z_Set(detailControls[(int)EIndex.PurchaseTax].Text) - sumZei;
                    }
                }
            }

            DataTable dt = GetGridEntity();

            //明細が1件も登録されていなければ、エラー Ｅ１８９
            if (dt.Rows.Count == 0)
            {
                //更新対象なし
                bbl.ShowMessage("E189");
                return;
            }

            //更新処理
            dpe = GetEntity();
            snbl.Purchase_Exec(dpe, dt, (short)OperationMode);

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
                else if (ctl.GetType().Equals(typeof(Panel)))
                {

                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex=-1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)) || ctl.GetType().Equals(typeof(Button)))
                {

                }
                else
                {
                    ctl.Text = "";
                }
            }

            //顧客情報ALLクリア
            ClearCustomerInfo(EIndex.CalledVendorCD);

            foreach (Control ctl in detailLabels)
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }

            mOldPurchaseDate = "";
            S_Clear_Grid();   //画面クリア（明細部）

            lblZei.Text = "";
            lblKin2.Text = "";
            lblKin3.Text = "";
            lblKin4.Text = "";
        }

        /// <summary>
        /// 仕入先情報クリア処理
        /// </summary>
        private void ClearCustomerInfo(EIndex index)
        {
                mOldVendorCD = "";
                mPayeeCD = "";
                mTaxTiming = "";
                ScVendorCD.LabelText = "";
                lblZei.Text = "";           
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
                            ScOrderNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
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
                            for (int index = 0; index < searchButtons.Length; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;
     
                            Pnl_Body.Enabled = Kbn == 0 ? true : false;

                            //抽出条件エリア	※新規時のみ、入力可能
                            if (OperationMode != EOperationMode.INSERT)
                            {
                                for(int index = (int)EIndex.ArrivalDateFrom; index<= (int)EIndex.VendorDeliveryNo; index++)
                                {
                                    detailControls[index].Enabled = false;
                                }
                                ScMaker.BtnSearch.Enabled = false;
                            }
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

                case 8: //F9:検索
                    //EsearchKbn kbn = EsearchKbn.Null;
                    //if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    //{
                    //    return;
                    //}

                    //switch (w_Col)
                    //{
                    //    case (int)ClsGridShiire.ColNO.JanCD:
                    //        //商品検索
                    //        kbn = EsearchKbn.Product;
                    //        break;
                    //}
                    //if (kbn != EsearchKbn.Null)
                    //    SearchData(kbn, previousCtrl);

                    break;
                case 9://納品データ(F10)
                    ExecDispNohin(1);
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

        ///// <summary>
        ///// 検索フォーム起動処理
        ///// </summary>
        ///// <param name="kbn"></param>
        ///// <param name="setCtl"></param>
        //private void SearchData(EsearchKbn kbn, Control setCtl)
        //{
        //    switch (kbn)
        //    {
        //        case EsearchKbn.Product:
        //            int w_Row;

        //            if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
        //            {
        //                return;
        //            }

        //            w_Row = w_CtlRow + Vsb_Mei_0.Value;

        //            //画面より配列セット 
        //            mGrid.S_DispToArray(Vsb_Mei_0.Value);

        //            using (Search_Product frmProduct = new Search_Product(detailControls[(int)EIndex.PurchaseDate].Text))
        //            {
        //                frmProduct.SKUCD = mGrid.g_DArray[w_Row].SKUCD;
        //                //frmProduct.ITEM = mGrid.g_DArray[w_Row].item;
        //                frmProduct.JANCD = mGrid.g_DArray[w_Row].JanCD;
        //                frmProduct.ShowDialog();

        //                if (!frmProduct.flgCancel)
        //                {
        //                    mGrid.g_DArray[w_Row].JanCD = frmProduct.JANCD;
        //                    mGrid.g_DArray[w_Row].SKUCD = frmProduct.SKUCD;
        //                    mGrid.g_DArray[w_Row].AdminNO = frmProduct.AdminNO;

        //                    //配列の内容を画面へセット
        //                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        //                }
        //            }
        //            break;

              

        //    }

        //}

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
                            detailControls[(int)EIndex.CalledVendorCD].Focus();
                        }
                        else if(index == (int)EIndex.PurchaseNO)
                            if(OperationMode == EOperationMode.UPDATE)
                                detailControls[(int)EIndex.CalledVendorCD].Focus();
                        else
                            keyControls[index + 1].Focus();

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
                        if (index == (int)EIndex.VendorDeliveryNo)
                            btnSubF10.Focus();
                        else if (OperationMode != EOperationMode.INSERT &&  index == (int)EIndex.StaffCD)
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridShiire.ColNO.PurchaseSu);
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
        private void BtnSubF10_Click(object sender, EventArgs e)
        {
            //新規登録時のみ使用可
            //納品ボタンClick時   
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
        private void KeyControl_Enter(object sender, EventArgs e)
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
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int w_Row;
                    Control w_ActCtl;

                    w_ActCtl = (Control)sender;
                    w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                    bool lastCell = false;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridShiire.ColNO.AdjustmentGaku)
                        if (w_Row == mGrid.g_MK_Max_Row - 1)
                            lastCell = true;

                    bool changeFlg = false;
                    switch (CL)
                    {
                        case (int)ClsGridShiire.ColNO.PurchaseSu:
                            if (mGrid.g_DArray[w_Row].PurchaseSu != w_ActCtl.Text)
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridShiire.ColNO.PurchaserUnitPrice:
                            if (mGrid.g_DArray[w_Row].PurchaserUnitPrice != w_ActCtl.Text)
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridShiire.ColNO.AdjustmentGaku:
                            if (mGrid.g_DArray[w_Row].AdjustmentGaku != w_ActCtl.Text)
                            {
                                changeFlg = true;
                            }
                            break;
                    }

                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);


                    //チェック処理
                    if (CheckGrid(CL, w_Row) == false)
                    {
                        //Focusセット処理
                        w_ActCtl.Focus();
                        return;
                    }

                    //手入力時金額を再計算
                    if (changeFlg)
                    {
                        switch (CL)
                        {
                            case (int)ClsGridShiire.ColNO.PurchaseSu:
                            case (int)ClsGridShiire.ColNO.PurchaserUnitPrice:
                            case (int)ClsGridShiire.ColNO.AdjustmentGaku:
                                //Form.Hedder.税計算が「明細単位」・「締単位」の場合
                                if (mTaxTiming == "1" || mTaxTiming == "3")
                                {
                                    //Form.Detail.消費税額(Hidden)(仕入額が変わった時のみ再計算、表示）
                                    decimal sumZei = 0;
                                    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                                    {
                                        if (mGrid.g_DArray[RW].Chk || OperationMode != EOperationMode.INSERT)
                                        {
                                            sumZei += bbl.Z_Set(mGrid.g_DArray[RW].PurchaseTax);
                                        }
                                    }
                                    detailControls[(int)EIndex.PurchaseTax].Text = bbl.Z_SetStr(sumZei);
                                }
                                else
                                {
                                    decimal sumKin10 = 0;
                                    decimal sumKin8 = 0;
                                    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                                    {
                                        if (mGrid.g_DArray[RW].Chk || OperationMode != EOperationMode.INSERT)
                                        {
                                            sumKin10 += bbl.Z_Set(mGrid.g_DArray[RW].PurchaseGaku10);
                                            sumKin8 += bbl.Z_Set(mGrid.g_DArray[RW].PurchaseGaku8);
                                        }
                                    }

                                    //Form.Hedder.税計算が「伝票単位」の場合(仕入額が変わった時のみ再計算、表示）
                                    //下記(A)＋(B)
                                    //(A)Function_消費税計算.out金額１	
                                    //in計算モード                  ←	1
                                    //in基準日                      ←	Form.仕入日
                                    //in軽減税率FLG                 ←	1
                                    //in金額                        ←	通常税率仕入額(Hidden)のTotal
                                    decimal zei;
                                    decimal kin = bbl.GetZeikomiKingaku(sumKin10, mGrid.g_DArray[w_Row].TaxRateFLG, out zei, detailControls[(int)EIndex.PurchaseDate].Text);

                                    //(B)Function_消費税計算.out金額１
                                    decimal zei2;
                                    decimal kin2 = bbl.GetZeikomiKingaku(sumKin8, mGrid.g_DArray[w_Row].TaxRateFLG, out zei2, detailControls[(int)EIndex.PurchaseDate].Text);
                                    detailControls[(int)EIndex.PurchaseTax].Text = bbl.Z_SetStr(zei+zei2);

                                }

                                if (CL == (int)ClsGridShiire.ColNO.AdjustmentGaku)
                                {
                                    //Form.Detail.調整額のTotal	(調整額が変わった時のみ再計算、表示）	
                                    decimal sumKin = 0;
                                    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                                    {
                                        if (mGrid.g_DArray[RW].Chk || OperationMode != EOperationMode.INSERT)
                                        {
                                            sumKin += bbl.Z_Set(mGrid.g_DArray[RW].AdjustmentGaku);
                                        }
                                    }

                                    lblKin4.Text = bbl.Z_SetStr(sumKin);
                                }
                                break;
                        }
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

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        //private void BtnSearch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        EsearchKbn kbn = EsearchKbn.Null;
        //        Control setCtl = null;
        //        Control sc = ((Control)sender).Parent;

        //        //検索ボタンClick時
        //        if (((Search.CKM_SearchControl)sc).Name.Substring(0,3).Equals("SC_"))
        //        {
        //            //商品検索
        //            kbn = EsearchKbn.Product;
        //        }
                
        //            setCtl = previousCtrl;
                

        //        if (kbn != EsearchKbn.Null)
        //            SearchData(kbn, setCtl);

        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        /// <summary>
        /// 明細部削除チェックボックスクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CHK_Del_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;
               
                //画面より配列セット 
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                bool Check = mGrid.g_DArray[w_Row].Chk;
                ChangeCheck(Check, w_Row);

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

        /// <summary>
        /// 自社倉庫CheckBox　CheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkM_CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //SetEnabled(EIndex.CheckBox3, ckM_CheckBox3.Checked);
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
                //新規登録時のみ、使用可
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
                //新規登録時のみ、使用可
                ChangeCheck(false);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        #endregion
        private void ChangeCheck(bool check)
        {
            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].StockNO) == false)
                {
                    mGrid.g_DArray[RW].Chk = check;
                    ChangeCheck(check, RW);
                }
            }
            CalcKin();
        }

        private void ChangeCheck(bool check, int w_Row)
        {

            //Onの時、行の背景色を黄色にする 新規登録時のみ、使用可
            if (check)
            {
                ChangeBackColor(w_Row, 1);
                //Onの時、仕入数以降、入力可
                CheckGrid((int)ClsGridShiire.ColNO.PurchaseSu, w_Row);
            }
            else
            {
                //Offの時、仕入数以降、入力不可
                ChangeBackColor(w_Row);
                //チェックOFFの場合、返品数０にして調整額０、返品額０にしてください
                mGrid.g_DArray[w_Row].PurchaseSu = "0";
                mGrid.g_DArray[w_Row].AdjustmentGaku = "0";
                mGrid.g_DArray[w_Row].PurchaseGaku = "0";
                CheckGrid((int)ClsGridShiire.ColNO.PurchaseSu, w_Row);
            }

            Grid_NotFocus((int)ClsGridShiire.ColNO.Chk, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        }
        private void ChangeBackColor(int w_Row, short kbn = 0)
        {
            Color backCL = GridBase.ClsGridBase.GrayColor;

            if (kbn == 1)
            {
                backCL = Color.Yellow;
            }
            for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                switch (w_Col)
                {
                    case (int)ClsGridShiire.ColNO.GYONO:
                    case (int)ClsGridShiire.ColNO.Space:
                        {
                            mGrid.g_MK_State[w_Col, w_Row].Cell_Color = backCL;
                            break;
                        }
                    case (int)ClsGridShiire.ColNO.ExpectReturnDate:
                    case (int)ClsGridShiire.ColNO.MakerItem:
                    case (int)ClsGridShiire.ColNO.JanCD:
                    case (int)ClsGridShiire.ColNO.SKUName:
                    case (int)ClsGridShiire.ColNO.SizeName:
                    case (int)ClsGridShiire.ColNO.ColorName:
                    case (int)ClsGridShiire.ColNO.CommentInStore:
                    case (int)ClsGridShiire.ColNO.CommentOutStore:
                    case (int)ClsGridShiire.ColNO.ReturnPlanSu:
                    case (int)ClsGridShiire.ColNO.CalculationGaku:
                    case (int)ClsGridShiire.ColNO.PurchaseGaku:
                        {
                            mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = true;

                            if (kbn == 1)
                            {
                                mGrid.g_MK_State[w_Col, w_Row].Cell_Color = backCL;
                            }
                            else
                            {
                                mGrid.g_MK_State[w_Col, w_Row].Cell_Color = System.Drawing.Color.Empty;
                            }
                            break;
                        }
                }
            }
        }
        }
}








