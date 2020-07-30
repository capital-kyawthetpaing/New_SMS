using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace HacchuuNyuuryoku
{
    /// <summary>
    /// HacchuuNyuuryoku 発注入力
    /// </summary>
    internal partial class HacchuuNyuuryoku : FrmMainForm
    {
        private const string ProID = "HacchuuNyuuryoku";
        private const string ProNm = "発注入力";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            OrderNO,
            CopyOrderNO,
            CheckBox2,
            CheckBox1,
            MotoOrderNO,
            StoreCD,

            OrderDate = 0,
            StaffCD,
            OrderCD,
            OrderName,
            AliasKBN,
            CheckBox3,
            SoukoName,
            CheckBox4,
            DestinationName,

            ZipCD1,
            ZipCD2,
            Address1,
            Address2,
            Tel,
            Fax,

            RemarksOutStore,
            RemarksInStore
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

        private HacchuuNyuuryoku_BL hnbl;
        private D_Order_Entity doe;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mZipCD = "";
        private string InOperatorName = "";
        private string mOldOrderNo = "";    //排他処理のため使用
        private string mOldOrderDate = "";
        private string mOldOrderCD = "";
        private decimal mZei10;//通常税額(Hidden)
        private decimal mZei8;//軽減税額(Hidden)

        private bool mSyoninsya;
        private int W_ApprovalStageFLG;
        private int mAmountFractionKBN;
        private int mTaxFractionKBN;
        private int mTaxTiming;
        private bool mSyouninKidou = false;
        private string mArrivalPlanDate = "";

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridHacchuu mGrid = new ClsGridHacchuu();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_MaxOrderGyoNo;

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

            if (ClsGridHacchuu.gc_P_GYO <= ClsGridHacchuu.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridHacchuu.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridHacchuu.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridHacchuu.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridHacchuu.gc_MaxCL;

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
                            check.Click += new System.EventHandler(CHK_Del_Click);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridHacchuu.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridHacchuu.ColNO.GYONO; i <= (int)ClsGridHacchuu.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridHacchuu.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridHacchuu.ColNO.OrderSu:
                            mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            break;
                        case (int)ClsGridHacchuu.ColNO.Rate:
                            mGrid.SetProp_Ritu(3, 2, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            break;
                        case (int)ClsGridHacchuu.ColNO.OrderUnitPrice:
                        case (int)ClsGridHacchuu.ColNO.PriceOutTax:
                        case (int)ClsGridHacchuu.ColNO.OrderGaku:
                            mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
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
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 0].CellCtl = CHK_DELCK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.EDIOrderFlg, 0].CellCtl = CHK_EDICK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.JanCD, 0].CellCtl = SC_ITEM_0;// IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SizeName, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.PriceOutTax, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.Rate, 0].CellCtl = IMN_TEIKA2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderSu, 0].CellCtl = IMN_GENER_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderUnitPrice, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.TaniCD, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ColorName, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderGaku, 0].CellCtl = IMN_SALEP2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentOutStore, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.IndividualClientName, 0].CellCtl = IMN_WEBPR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentInStore, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.MakerItem, 0].CellCtl = IMT_JUONO_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.DesiredDeliveryDate, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ArrivePlanDate, 0].CellCtl = IMT_PAYDT_0;    //支払予定日

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 1].CellCtl = CHK_DELCK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.EDIOrderFlg, 1].CellCtl = CHK_EDICK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.JanCD, 1].CellCtl = SC_ITEM_1;//IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SizeName, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.PriceOutTax, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.Rate, 1].CellCtl = IMN_TEIKA2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderSu, 1].CellCtl = IMN_GENER_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderUnitPrice, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.TaniCD, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ColorName, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderGaku, 1].CellCtl = IMN_SALEP2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentOutStore, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.IndividualClientName, 1].CellCtl = IMN_WEBPR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentInStore, 1].CellCtl = IMT_REMAK_1;

            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.MakerItem, 1].CellCtl = IMT_JUONO_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.DesiredDeliveryDate, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ArrivePlanDate, 1].CellCtl = IMT_PAYDT_1;    //支払予定日

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 2].CellCtl = CHK_DELCK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.EDIOrderFlg, 2].CellCtl = CHK_EDICK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.JanCD, 2].CellCtl = SC_ITEM_2;//IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SizeName, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.PriceOutTax, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.Rate, 2].CellCtl = IMN_TEIKA2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderSu, 2].CellCtl = IMN_GENER_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderUnitPrice, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.TaniCD, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ColorName, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderGaku, 2].CellCtl = IMN_SALEP2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentOutStore, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.IndividualClientName, 2].CellCtl = IMN_WEBPR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentInStore, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.MakerItem, 2].CellCtl = IMT_JUONO_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.DesiredDeliveryDate, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ArrivePlanDate, 2].CellCtl = IMT_PAYDT_2;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 3].CellCtl = CHK_DELCK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.EDIOrderFlg, 3].CellCtl = CHK_EDICK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.JanCD, 3].CellCtl = SC_ITEM_3;// IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SizeName, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.PriceOutTax, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.Rate, 3].CellCtl = IMN_TEIKA2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderSu, 3].CellCtl = IMN_GENER_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderUnitPrice, 3].CellCtl = IMN_GENER2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.TaniCD, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ColorName, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderGaku, 3].CellCtl = IMN_SALEP2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentOutStore, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.IndividualClientName, 3].CellCtl = IMN_WEBPR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentInStore, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.MakerItem, 3].CellCtl = IMT_JUONO_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.DesiredDeliveryDate, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ArrivePlanDate, 3].CellCtl = IMT_PAYDT_3;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 4].CellCtl = CHK_DELCK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.EDIOrderFlg, 4].CellCtl = CHK_EDICK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.JanCD, 4].CellCtl = SC_ITEM_4;// IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SizeName, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.PriceOutTax, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.Rate, 4].CellCtl = IMN_TEIKA2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderSu, 4].CellCtl = IMN_GENER_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderUnitPrice, 4].CellCtl = IMN_GENER2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.TaniCD, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ColorName, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderGaku, 4].CellCtl = IMN_SALEP2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentOutStore, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.IndividualClientName, 4].CellCtl = IMN_WEBPR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentInStore, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.MakerItem, 4].CellCtl = IMT_JUONO_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.DesiredDeliveryDate, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ArrivePlanDate, 4].CellCtl = IMT_PAYDT_4;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 5].CellCtl = CHK_DELCK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.EDIOrderFlg, 5].CellCtl = CHK_EDICK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.JanCD, 5].CellCtl = SC_ITEM_5;// IMT_JANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SizeName, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.PriceOutTax, 5].CellCtl = IMN_TEIKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.Rate, 5].CellCtl = IMN_TEIKA2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderSu, 5].CellCtl = IMN_GENER_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderUnitPrice, 5].CellCtl = IMN_GENER2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.TaniCD, 5].CellCtl = IMN_MEMBR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ColorName, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderGaku, 5].CellCtl = IMN_SALEP2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentOutStore, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.IndividualClientName, 5].CellCtl = IMN_WEBPR2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentInStore, 5].CellCtl = IMT_REMAK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.MakerItem, 5].CellCtl = IMT_JUONO_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.DesiredDeliveryDate, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ArrivePlanDate, 5].CellCtl = IMT_PAYDT_5;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 6].CellCtl = CHK_DELCK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.EDIOrderFlg, 6].CellCtl = CHK_EDICK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUCD, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.JanCD, 6].CellCtl = SC_ITEM_6;// IMT_JANCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SizeName, 6].CellCtl = IMT_KAIDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.PriceOutTax, 6].CellCtl = IMN_TEIKA_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.Rate, 6].CellCtl = IMN_TEIKA2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderSu, 6].CellCtl = IMN_GENER_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderUnitPrice, 6].CellCtl = IMN_GENER2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.TaniCD, 6].CellCtl = IMN_MEMBR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ColorName, 6].CellCtl = IMN_CLINT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderGaku, 6].CellCtl = IMN_SALEP2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentOutStore, 6].CellCtl = IMN_WEBPR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.IndividualClientName, 6].CellCtl = IMN_WEBPR2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentInStore, 6].CellCtl = IMT_REMAK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.MakerItem, 6].CellCtl = IMT_JUONO_6;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.DesiredDeliveryDate, 6].CellCtl = IMT_ARIDT_6;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ArrivePlanDate, 6].CellCtl = IMT_PAYDT_6;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 7].CellCtl = CHK_DELCK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.EDIOrderFlg, 7].CellCtl = CHK_EDICK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUCD, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.JanCD, 7].CellCtl = SC_ITEM_7;// IMT_JANCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SizeName, 7].CellCtl = IMT_KAIDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.SKUName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.PriceOutTax, 7].CellCtl = IMN_TEIKA_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.Rate, 7].CellCtl = IMN_TEIKA2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderSu, 7].CellCtl = IMN_GENER_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderUnitPrice, 7].CellCtl = IMN_GENER2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.TaniCD, 7].CellCtl = IMN_MEMBR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ColorName, 7].CellCtl = IMN_CLINT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.OrderGaku, 7].CellCtl = IMN_SALEP2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentOutStore, 7].CellCtl = IMN_WEBPR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.IndividualClientName, 7].CellCtl = IMN_WEBPR2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.CommentInStore, 7].CellCtl = IMT_REMAK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.MakerItem, 7].CellCtl = IMT_JUONO_7;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.DesiredDeliveryDate, 7].CellCtl = IMT_ARIDT_7;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ArrivePlanDate, 7].CellCtl = IMT_PAYDT_7;    //支払予定日
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHacchuu.Gen_MK_FocusMove.MvNxt, (int)ClsGridHacchuu.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHacchuu.Gen_MK_FocusMove.MvPrv, (int)ClsGridHacchuu.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHacchuu.Gen_MK_FocusMove.MvNxt, (int)ClsGridHacchuu.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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
                            case (int)ClsGridHacchuu.ColNO.GYONO:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }

                            case (int)ClsGridHacchuu.ColNO.SKUCD:
                            case (int)ClsGridHacchuu.ColNO.TaniCD:
                            case (int)ClsGridHacchuu.ColNO.PriceOutTax:
                            case (int)ClsGridHacchuu.ColNO.ArrivePlanDate:    //支払予定日
                            case (int)ClsGridHacchuu.ColNO.IndividualClientName:
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
                    //    case object _ when ClsGridMitsumori.ColNO.DELCK:
                    //        {
                    //            // 削除チェック
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

                                //Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                                mGrid.g_MK_State[(int)ClsGridHacchuu.ColNO.JanCD, w_Row].Cell_Enabled = true;

                            }
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                keyControls[(int)EIndex.OrderNO].Text = "";
                                keyControls[(int)EIndex.OrderNO].Enabled = false;
                                ScOrderNO.BtnSearch.Enabled = false;
                                keyControls[(int)EIndex.CopyOrderNO].Enabled = true;
                                ScCopyOrderNO.BtnSearch.Enabled = true;

                                //新規Modeの場合のみCheck可能
                                ckM_CheckBox1.Enabled = true;
                                ckM_CheckBox2.Enabled = true;

                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                detailControls[(int)EIndex.OrderDate].Text = bbl.GetDate();
                                F9Visible = false;
                                Chk_Souko.Checked = true;

                                if (BtnSubF11.Enabled)
                                    SetFuncKeyAll(this, "111111001011");
                                else
                                    SetFuncKeyAll(this, "111111001001");
                            }
                            else
                            {
                                keyControls[(int)EIndex.OrderNO].Enabled = true;
                                ScOrderNO.BtnSearch.Enabled = true;
                                keyControls[(int)EIndex.CopyOrderNO].Text = "";
                                keyControls[(int)EIndex.CopyOrderNO].Enabled = false;
                                ScCopyOrderNO.BtnSearch.Enabled = false;
                                ckM_CheckBox1.Enabled = false;
                                ckM_CheckBox2.Enabled = false;

                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                this.Vsb_Mei_0.TabStop = false;

                                SetFuncKeyAll(this, "111111001000");
                            }
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
                                    mGrid.g_MK_State[(int)ClsGridHacchuu.ColNO.JanCD, w_Row].Cell_Enabled = true;
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        //case (int)ClsGridHacchuu.ColNO.ChkDel:    // 
                                        case (int)ClsGridHacchuu.ColNO.EDIOrderFlg:    // 
                                        case (int)ClsGridHacchuu.ColNO.Rate:    // 
                                        case (int)ClsGridHacchuu.ColNO.OrderSu:    // 
                                        case (int)ClsGridHacchuu.ColNO.OrderUnitPrice:    // 
                                        case (int)ClsGridHacchuu.ColNO.CommentOutStore:    // 
                                        case (int)ClsGridHacchuu.ColNO.CommentInStore:    //
                                        case (int)ClsGridHacchuu.ColNO.DesiredDeliveryDate:    //入荷予定日
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                break;
                                            }
                                        case (int)ClsGridHacchuu.ColNO.JanCD:
                                        case (int)ClsGridHacchuu.ColNO.SKUName:    // 
                                        case (int)ClsGridHacchuu.ColNO.ColorName:    // 
                                        case (int)ClsGridHacchuu.ColNO.SizeName:
                                        case (int)ClsGridHacchuu.ColNO.MakerItem:
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
                            SetBtnSubF11Enabled(BtnSubF11.Enabled);
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
            //SetFuncKeyAll(this, "111111111111");
            SetFuncKey(this, 6, true);
            SetFuncKey(this, 7, true);
            SetFuncKey(this, 9, true);

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


            //if (pCol == (int)ClsGridHacchuu.ColNO.JanCD || pCol == (int)ClsGridHacchuu.ColNO.ChkDel)
            //{
            //    if (mGrid.g_DArray[pRow].DELCK == true)
            //    {
            //        // 削除チェック時は、JanCDは フォーカスセット可 変更不可
            //        mGrid.g_MK_State[(int)ClsGridHacchuu.ColNO.JanCD, pRow].Cell_ReadOnly = true;


            //        //削除チェック時、ReadOnlyの列以外 使用不可
            //        for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            //        {
            //            if (mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly)
            //            {
            //                //READONLYの列(JanCD または 取消区分)は使用可
            //            }
            //            else if (pCol == (int)ClsGridHacchuu.ColNO.ChkDel)
            //            {
            //                //削除チェックは使用可
            //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //            }
            //            else
            //            {
            //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
            //            }
            //        }
            //        return;
            //    }
            //    else {
            //        w_AllFlg = true;
            //        //削除チェックOFFは、JanCDは 元に戻す
            //        mGrid.g_MK_State[(int)ClsGridHacchuu.ColNO.JanCD, pRow].Cell_ReadOnly = false;
            //    }
            //}


            if (pCol == (int)ClsGridHacchuu.ColNO.JanCD || w_AllFlg == true)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].JanCD))
                {
                    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        if (w_Col == (int)ClsGridHacchuu.ColNO.JanCD)// || pCol == (int)ClsGridHacchuu.ColNO.ChkDel)
                        {//JANCD使用可
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                        }
                        else
                        {
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                        }
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
                            case (int)ClsGridHacchuu.ColNO.JanCD:
                                if (mGrid.g_DArray[pRow].hacchuGyoNO == 0)
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

                            case (int)ClsGridHacchuu.ColNO.OrderSu:
                                //入荷予定日(D_OrderDetails.ArrivePlanDate)≠	Nullの場合、入力不可				
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].ArrivePlanDate))
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

                            case (int)ClsGridHacchuu.ColNO.SKUName:
                            case (int)ClsGridHacchuu.ColNO.ColorName:
                            case (int)ClsGridHacchuu.ColNO.SizeName:
                            case (int)ClsGridHacchuu.ColNO.MakerItem:
                                //修正時は変更不可
                                if (mGrid.g_DArray[pRow].VariousFLG == 1 && mGrid.g_DArray[pRow].hacchuGyoNO == 0)
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Color = System.Drawing.Color.Empty;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                }
                                else
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                }
                                break;

                            //case (int)ClsGridHacchuu.ColNO.ChkDel:
                            case (int)ClsGridHacchuu.ColNO.EDIOrderFlg:    // 
                            case (int)ClsGridHacchuu.ColNO.Rate:
                            case (int)ClsGridHacchuu.ColNO.OrderGaku:
                            case (int)ClsGridHacchuu.ColNO.OrderUnitPrice:    // 
                            case (int)ClsGridHacchuu.ColNO.CommentOutStore:    // 
                            case (int)ClsGridHacchuu.ColNO.CommentInStore:    //
                            case (int)ClsGridHacchuu.ColNO.DesiredDeliveryDate:    //希望納期
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                }
                                break;

                        }

                    }
                    w_AllFlg = false;

                    //w_AllFlg = true;
                }
            }
            if (pCol == (int)ClsGridHacchuu.ColNO.OrderSu)
            {
                //入荷予定日(D_OrderDetails.ArrivePlanDate)≠	Nullの場合、入力不可				
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].ArrivePlanDate))
                {
                    mGrid.g_MK_State[pCol, pRow].Cell_Enabled = true;
                }
                else
                {
                    mGrid.g_MK_State[pCol, pRow].Cell_Enabled = false;
                }
            }
            else if (pCol == (int)ClsGridHacchuu.ColNO.OrderGaku)
            {
                if (mGrid.g_DArray[pRow].VariousFLG == 1)
                {
                    mGrid.g_MK_State[pCol, pRow].Cell_Enabled = true;
                }
                else
                {
                    mGrid.g_MK_State[pCol, pRow].Cell_Enabled = false;
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

            //ただし、その明細が入荷予定日ありの明細である場合、削除できない（F7ボタンを使えないようにする）
            if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].ArrivePlanDate))
                return;

            for (int i = w_Row; i < mGrid.g_MK_Max_Row - 1; i++)
            {
                int w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //次行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i + 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            CalcKin();

            int col = (int)ClsGridHacchuu.ColNO.JanCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //フォーカスセット
            IMT_DMY_0.Focus();

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridHacchuu.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

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

            int col = (int)ClsGridHacchuu.ColNO.JanCD;

            //コピー行より下の明細を1行ずつずらす（内容コピー）
            for (int i = mGrid.g_MK_Max_Row - 1; i >= w_Row; i--)
            {
                w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //前行をコピー (修正元行№以外)
                int w_MOTNO = mGrid.g_DArray[w_Row].hacchuGyoNO;      //修正元行№ 退避
                mGrid.g_DArray[i] = mGrid.g_DArray[i - 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
                mGrid.g_DArray[w_Row].hacchuGyoNO = w_MOTNO;      //修正元行№

                Grid_NotFocus(col, i);
            }

            //状態もコピー
            // ※ 前行と状態が違うとき注意、この部分変更要 (修正元のあるなしで 入力可能項目が変わる場合など)
            for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                mGrid.g_MK_State[w_Col, w_Row] = mGrid.g_MK_State[w_Col, w_Row - 1];
            }
            Grid_NotFocus(col, w_Row);
            CalcKin();

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            mGrid.F_MoveFocus((int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, IMT_DMY_0, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);
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

            int col = (int)ClsGridHacchuu.ColNO.JanCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridHacchuu.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

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
            Grid_NotFocus((int)ClsGridHacchuu.ColNO.JanCD, RW);

            // 配列の内容を画面にセット
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
        }

        #endregion

        public HacchuuNyuuryoku()
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

                lblDisp.Visible = true;
                Btn_F11.Text = "承認(F11)";

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                hnbl = new HacchuuNyuuryoku_BL();
                CboStoreCD.Bind(ymd);
                CboSoukoName.Bind(ymd);

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScOrderNO.Value1 = InOperatorCD;
                ScOrderNO.Value2 = stores;
                ScCopyOrderNO.Value1 = InOperatorCD;
                ScCopyOrderNO.Value2 = stores;
                ScMotoOrderNo.Value1 = InOperatorCD;
                ScMotoOrderNo.Value2 = stores;

                ScStaff.TxtCode.Text = InOperatorCD;

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = hnbl.GetDate()
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

                SetInitSoukoName();

                //承認用データを抽出
                //W_ApprovalStageFLG = hnbl.GetApprovalStageFLG(InOperatorCD, StoreCD);

                detailControls[(int)EIndex.OrderDate].Text = ymd;

                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    mSyouninKidou = true;
                    Btn_F2.Text = "";
                    Btn_F3.Text = "";
                    Btn_F4.Text = "";
                    Btn_F5.Text = "";
                    Btn_F6.Text = "";

                    ChangeOperationMode(EOperationMode.UPDATE);

                    //↑Paramete起動されるのは、発注承認入力からのみ。
                    keyControls[(int)EIndex.OrderNO].Text = cmds[cmds.Length - 1];

                    //画面転送表01に従って、画面情報を表示
                    //この場合、その発注番号のみを表示・処理し、登録ボタン押下でメッセージなくプログラム終了。
                    CheckKey((int)EIndex.OrderNO, true);

                }

                if (W_ApprovalStageFLG.Equals(9))
                {
                    //最終承認者の場合は承認済みにする
                    SetlblDisp("承認");
                }

                detailControls[(int)EIndex.OrderDate].Focus();  //仕入先？新規の場合は発注日？
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }
        private void SetInitSoukoName()
        {
            //[M_Souko_SelectForNyuuka]
            M_Souko_Entity me = new M_Souko_Entity
            {
                StoreCD = CboStoreCD.SelectedValue != null ? CboStoreCD.SelectedValue.ToString() : "",
                SoukoType = "3", //店舗Main倉庫
                ChangeDate = bbl.GetDate(),
                DeleteFlg = "0"
            };
            DataTable sdt = hnbl.M_Souko_Select(me);
            if (sdt.Rows.Count > 0)
            {
                CboSoukoName.SelectedValue = sdt.Rows[0]["SoukoCD"];
            }
        }
        private void InitialControlArray()
        {
            keyControls = new Control[] { ScOrderNO.TxtCode, ScCopyOrderNO.TxtCode, ckM_CheckBox2, ckM_CheckBox1, ScMotoOrderNo.TxtCode, CboStoreCD };
            keyLabels = new Control[] { };
            detailControls = new Control[] { ckM_TextBox1, ScStaff.TxtCode, ScOrder.TxtCode, ckM_Text_4, panel1
                        ,Chk_Souko, CboSoukoName, ckM_CheckBox4, ckM_TextBox9, ckM_TextBox20, ckM_TextBox19,  ckM_TextBox14, ckM_TextBox7, ckM_TextBox18 , ckM_TextBox8
                         ,TxtRemark1,TxtRemark2 };
            detailLabels = new Control[] { ScOrder, ScStaff };
            searchButtons = new Control[] { ScOrder.BtnSearch, ScStaff.BtnSearch };

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

            radioButton1.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
            radioButton2.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);

            radioButton1.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton2.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
        }

        /// <summary>
        /// PrimaryKeyのコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckKey(int index, bool set = true)
        {

            switch (index)
            {
                case (int)EIndex.OrderNO:
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

                case (int)EIndex.CopyOrderNO:
                    if (!string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return CheckData(set, index);

                    break;

                case (int)EIndex.MotoOrderNO:
                    if (!string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return CheckData(set, index);

                    break;
            }

            return true;

        }

        private bool SelectAndInsertExclusive()
        {
            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT)
                return true;

            DeleteExclusive();

            if (string.IsNullOrWhiteSpace(keyControls[(int)EIndex.OrderNO].Text))
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Hacchu,
                Number = keyControls[(int)EIndex.OrderNO].Text,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                keyControls[(int)EIndex.OrderNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldOrderNo = keyControls[(int)EIndex.OrderNO].Text;
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive()
        {
            if (mOldOrderNo == "")
                return;

            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Hacchu,
                Number = mOldOrderNo,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);

            mOldOrderNo = "";
        }

        /// <summary>
        /// 受注データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set, int index = (int)EIndex.OrderNO)
        {
            //[D_Order_SelectData]
            doe = new D_Order_Entity();

            if (index == (int)EIndex.CopyOrderNO)
                doe.OrderNO = keyControls[(int)EIndex.CopyOrderNO].Text;
            else if (index == (int)EIndex.MotoOrderNO)
                doe.OrderNO = keyControls[(int)EIndex.MotoOrderNO].Text;
            else
                doe.OrderNO = keyControls[(int)EIndex.OrderNO].Text;

            DataTable dt = hnbl.D_Order_SelectData(doe, (short)OperationMode);

            //発注(D_Order)に存在しない場合、Error 「登録されていない発注番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", "発注番号");
                Scr_Clr(1);
                previousCtrl.Focus();
                return false;
            }
            else
            {
                //DeleteDateTime 「削除された発注番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", "発注番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                //権限がない場合（以下のSelectができない場合）Error　「権限のない発注番号」
                if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                {
                    bbl.ShowMessage("E139", "発注番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                if (index == (int)EIndex.OrderNO && OperationMode != EOperationMode.SHOW)
                {
                    //上位承認者が承認行為を行った後に下位承認者が承認しようとした場合
                    //（以下のSelectができる場合）、Error「上位承認者によって既に承認済みです。」	
                    bool ret = hnbl.CheckSyonin(doe.OrderNO, InOperatorCD, out string errno2);
                    if (ret)
                    {
                        if (!string.IsNullOrWhiteSpace(errno2))
                        {
                            bbl.ShowMessage(errno2);
                            previousCtrl.Focus();
                            return false;
                        }
                    }

                    //進捗チェック　既に出荷済み,出荷指示済み,ピッキングリスト完了済み,仕入済み,入荷済み警告
                    ret = hnbl.CheckHacchuData(doe.OrderNO, out string errno);
                    if (ret)
                    {
                        if (!string.IsNullOrWhiteSpace(errno))
                        {
                            //警告メッセージを表示する
                            bbl.ShowMessage(errno);
                        }
                    }
                    //【削除モードの時】
                    //出荷済の明細を含んだ発注は、そのものの削除を不可
                    if (OperationMode == EOperationMode.DELETE)
                    {
                        //E159:出荷済み
                        if (errno.Equals("E159"))
                        {
                            //出荷済明細が存在するので、その発注データの削除も許さない
                            bbl.ShowMessage("E167");
                            previousCtrl.Focus();
                            return false;
                        }
                    }
                }
                else if (index == (int)EIndex.MotoOrderNO)
                {
                    //DeleteDateTime 「削除された発注番号」
                    if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                    {
                        bbl.ShowMessage("E140", "発注番号");
                        Scr_Clr(1);
                        previousCtrl.Focus();
                        return false;
                    }
                    else if(dt.Rows[0]["ReturnFLG"].ToString().Equals("1") || dt.Rows[0]["ReturnFLG"].ToString().Equals("2"))
                    {
                        if (dt.Rows[0]["ReturnFLG"].ToString().Equals("1"))
                            bbl.ShowMessage("E253");
                        else
                            bbl.ShowMessage("E254");
                        Scr_Clr(1);
                        previousCtrl.Focus();
                        return false;
                    }
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
                m_MaxOrderGyoNo = 0;

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
                        if (bbl.Z_Set( row["ReturnFLG"]).Equals(2))
                        {
                            ckM_CheckBox2.Checked = true;
                        }
                        else if (bbl.Z_Set(row["ReturnFLG"]).Equals(1))
                        {
                            ckM_CheckBox1.Checked = true;
                        }

                        CboStoreCD.SelectedValue = row["StoreCD"];

                        //明細にデータをセット
                        if (index == (int)EIndex.OrderNO)
                        {
                            detailControls[(int)EIndex.OrderDate].Text = row["OrderDate"].ToString();
                        }
                        else
                        {
                            detailControls[(int)EIndex.OrderDate].Text = bbl.GetDate();
                        }
                        mOldOrderDate = detailControls[(int)EIndex.OrderDate].Text;

                        detailControls[(int)EIndex.StaffCD].Text = row["StaffCD"].ToString();
                        CheckDetail((int)EIndex.StaffCD);

                        detailControls[(int)EIndex.OrderCD].Text = row["OrderCD"].ToString();
                        CheckDetail((int)EIndex.OrderCD);
                        detailControls[(int)EIndex.OrderName].Text = row["OrderPerson"].ToString();

                        if (row["DestinationKBN"].ToString() == "2") //自社倉庫
                        {
                            Chk_Souko.Checked = true;
                            CboSoukoName.SelectedValue = row["DestinationSoukoCD"];
                            ckM_CheckBox4.Checked = false;
                        }
                        else
                        {
                            ckM_CheckBox4.Checked = true;
                            detailControls[(int)EIndex.DestinationName].Text = row["DestinationName"].ToString();
                            Chk_Souko.Checked = false;
                        }

                        detailControls[(int)EIndex.ZipCD1].Text = row["DestinationZip1CD"].ToString();
                        detailControls[(int)EIndex.ZipCD2].Text = row["DestinationZip2CD"].ToString();
                        mZipCD = detailControls[(int)EIndex.ZipCD1].Text + detailControls[(int)EIndex.ZipCD2].Text;
                        detailControls[(int)EIndex.Address1].Text = row["DestinationAddress1"].ToString();
                        detailControls[(int)EIndex.Address2].Text = row["DestinationAddress2"].ToString();
                        detailControls[(int)EIndex.Tel].Text = row["DestinationTelphoneNO"].ToString();
                        detailControls[(int)EIndex.Fax].Text = row["DestinationFaxNO"].ToString();

                        if (index == (int)EIndex.OrderNO)
                        {
                            //その発注が「申請」「承認中」の場合に表示＆利用可能。以外は表示しない。
                            //申請中、承認中の場合
                            int mApprovalStageFLG = Convert.ToInt16(row["ApprovalStageFLG"].ToString());
                            if (mApprovalStageFLG >= 1 && mApprovalStageFLG < 9 && W_ApprovalStageFLG >= mApprovalStageFLG && mSyoninsya)
                                SetBtnSubF11Enabled(true);
                            else
                                SetBtnSubF11Enabled(false);

                            switch (mApprovalStageFLG)
                            {
                                case 0:
                                    SetlblDisp("却下");
                                    break;

                                case 1:
                                    SetlblDisp("申請中");
                                    break;

                                default:
                                    if (mApprovalStageFLG >=9)
                                        SetlblDisp("承認済");
                                    else
                                       SetlblDisp("承認中");
                                    break;
                            }
                        }
                        else
                        {
                            //複写時
                            SetBtnSubF11Enabled(true);
                            if (W_ApprovalStageFLG.Equals(9))
                            {
                                //最終承認者の場合は承認済みにする
                                SetlblDisp("承認");
                            }
                            else
                            {
                                SetlblDisp("申請中");
                            }
                        }

                        if (row["AliasKBN"].ToString() == "1")
                            radioButton2.Checked = true;
                        else
                            radioButton1.Checked = true;


                        //【Data Area Footer】
                        detailControls[(int)EIndex.RemarksInStore].Text = row["CommentInStore"].ToString();
                        detailControls[(int)EIndex.RemarksOutStore].Text = row["CommentOutStore"].ToString();

                        //lblKin1.Text = bbl.Z_SetStr(row["SUM_JuchuuGaku"]);
                        //lblKin2.Text = bbl.Z_SetStr(row["HanbaiHontaiGaku"]);
                        //lblKin3.Text = bbl.Z_SetStr(row["SUM_CostGaku"]);

                        //明細なしの場合
                        if (bbl.Z_Set(row["OrderRows"]) == 0)
                            break;
                    }

                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    //mGrid.g_DArray[i].OldJanCD = mGrid.g_DArray[i].JanCD; del
                    mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                    mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();

                    if (index == (int)EIndex.MotoOrderNO)
                    {
                        mGrid.g_DArray[i].OrderSu = bbl.Z_SetStr(-1 * bbl.Z_Set(row["OrderSu"]));   //単価算出のため先にセットしておく    
                    }
                    else
                    {
                        mGrid.g_DArray[i].OrderSu = bbl.Z_SetStr(row["OrderSu"]);   //単価算出のため先にセットしておく    
                    }

                    CheckGrid((int)ClsGridHacchuu.ColNO.JanCD, i, true);

                    mGrid.g_DArray[i].SKUName = row["ItemName"].ToString();   // 
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 
                    mGrid.g_DArray[i].MakerItem = row["MakerItem"].ToString();

                    mGrid.g_DArray[i].Rate = string.Format("{0:#,##0.00}", bbl.Z_Set(row["Rate"]));   // 
                    mGrid.g_DArray[i].OrderUnitPrice = bbl.Z_SetStr(row["OrderUnitPrice"]);   // 
                    //mGrid.g_DArray[i].PriceOutTax = bbl.Z_SetStr(row["PriceOutTax"]);   //  CheckGridでセット

                    if (ckM_CheckBox1.Checked)  //返品のときのみ数量*(-1)
                    {
                        mGrid.g_DArray[i].OrderGaku = bbl.Z_SetStr(-1 * bbl.Z_Set(row["OrderHontaiGaku"]));   // 
                    }
                    else
                    {
                        mGrid.g_DArray[i].OrderGaku = bbl.Z_SetStr(row["OrderHontaiGaku"]);   // 
                    }
                    CheckGrid((int)ClsGridHacchuu.ColNO.OrderGaku, i, true);
                    //mGrid.g_DArray[i].MitsumoriTax = bbl.Z_Set(row["CostGaku"]);  CheckGridでセット
                    //mGrid.g_DArray[i].KeigenTax = bbl.Z_Setrow["CostGaku"]);    CheckGridでセット

                    //mGrid.g_DArray[i].TaniName = bbl.Z_SetStr(row["TaniName"]);   // CheckGridでセット
                    mGrid.g_DArray[i].CommentInStore = row["D_CommentInStore"].ToString();   // 
                    mGrid.g_DArray[i].CommentOutStore = row["D_CommentOutStore"].ToString();   // 

                    if (row["EDIFLG"].ToString() == "1")
                        mGrid.g_DArray[i].EDIOrderFlg = true;

                    //税額(Hidden)
                    //mGrid.g_DArray[i].Tax = bbl.Z_Set(mGrid.g_DArray[i].PriceOutTax) - bbl.Z_Set(mGrid.g_DArray[i].Rate);

                    mGrid.g_DArray[i].DesiredDeliveryDate = row["DesiredDeliveryDate"].ToString();

                    if (index == (int)EIndex.OrderNO)
                    {
                        mGrid.g_DArray[i].ArrivePlanDate = row["ArrivePlanDate"].ToString();
                        mGrid.g_DArray[i].hacchuGyoNO = Convert.ToInt16(row["OrderRows"].ToString());
                        if (m_MaxOrderGyoNo < mGrid.g_DArray[i].hacchuGyoNO)
                            m_MaxOrderGyoNo = mGrid.g_DArray[i].hacchuGyoNO;
                    }
                    m_dataCnt = i + 1;
                    Grid_NotFocus((int)ClsGridHacchuu.ColNO.JanCD, i);
                    i++;
                }

                mOldOrderDate = detailControls[(int)EIndex.OrderDate].Text;

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
            }
            CalcKin();

            if (OperationMode == EOperationMode.UPDATE)
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            }
            else if (OperationMode == EOperationMode.INSERT)
            {
                //複写コピー後
                S_BodySeigyo(0, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                detailControls[(int)EIndex.OrderDate].Focus();
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
            /// ボタンを押すたびに「承認」「却下」のモードを変更する。
            /// その発注が「申請」「承認中」の場合に表示＆利用可能。以外は表示しない。
            /// 「承認」を初期表示。
            SetApprovalStage();
        }

        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set = true)
        {
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.OrderDate:
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

                    //発注日が変更された場合のチェック処理
                    if (mOldOrderDate != detailControls[index].Text)
                    {
                        for (int i = (int)EIndex.StaffCD; i < (int)EIndex.OrderCD; i++)
                            if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                                if (!CheckDependsOnOrderDate(i, true))
                                    return false;

                        //明細部JANCDの再チェック
                        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                        {
                            if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                            {
                                if (CheckGrid((int)ClsGridHacchuu.ColNO.JanCD, RW, false, true) == false)
                                {
                                    //Focusセット処理
                                    ERR_FOCUS_GRID_SUB((int)ClsGridHacchuu.ColNO.JanCD, RW);
                                    return false;
                                }
                            }
                        }
                        mOldOrderDate = detailControls[index].Text;
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

                    if (!CheckDependsOnOrderDate(index))
                        return false;

                    break;

                case (int)EIndex.OrderCD:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }
                    if (!CheckDependsOnOrderDate(index))
                        return false;

                    break;

                case (int)EIndex.OrderName:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //入力無くても良い(It is not necessary to input)
                    }
                    else
                    {
                        //入力された場合、様ON
                        radioButton1.Checked = true;
                    }
                    break;

                case (int)EIndex.SoukoName:
                    //入力できる場合(When input is possible)
                    if (CboSoukoName.Enabled)
                    {
                        //選択必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            CboSoukoName.MoveNext = false;
                            return false;
                        }

                        //if (!CheckDependsOnMitsumoriDate(index))
                        //    return false;
                    }
                    break;

                case (int)EIndex.CheckBox4:
                    if (Chk_Souko.Checked == false && ckM_CheckBox4.Checked == false)
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    break;

                case (int)EIndex.DestinationName:
                    //入力可能の場合 入力必須(Entry required)
                    if (detailControls[index].Enabled && string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    break;

                case (int)EIndex.ZipCD1:
                    break;

                case (int)EIndex.ZipCD2:
                    //郵便番号1または2に入力があった場合
                    if (!string.IsNullOrWhiteSpace(detailControls[index].Text) || !string.IsNullOrWhiteSpace(detailControls[index - 1].Text))
                    {
                        //郵便番号変換マスター(M_ZipCode)に存在すること
                        //[M_ZipCode]
                        M_ZipCode_Entity mze = new M_ZipCode_Entity
                        {
                            ZipCD1 = detailControls[index - 1].Text,
                            ZipCD2 = detailControls[index].Text
                        };
                        ZipCode_BL mbl = new ZipCode_BL();
                        bool ret = mbl.M_ZipCode_SelectData(mze);
                        if (ret)
                        {
                            if (mZipCD != mze.ZipCD1 + mze.ZipCD2)
                            {
                                detailControls[index + 1].Text = mze.Address1;
                                detailControls[index + 2].Text = mze.Address2;
                            }
                        }
                        else
                        {
                            //Ｅ１０１
                            mbl.ShowMessage("E101");
                            return false;
                        }
                        mZipCD = mze.ZipCD1 + mze.ZipCD2;
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
            w_Ret = mGrid.F_MoveFocus((int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridHacchuu.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        /// <summary>
        /// 発注日が変更されたときに必要なチェック処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDependsOnOrderDate(int index, bool ChangeMitsumoriDate = false)
        {
            string ymd = detailControls[(int)EIndex.OrderDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            switch (index)
            {
                //case (int)EIndex.SoukoName:
                //    //[M_Souko_Select]
                //    M_Souko_Entity me = new M_Souko_Entity
                //    {
                //        SoukoCD = CboSoukoName.SelectedValue.ToString(),
                //        ChangeDate = ymd,
                //        DeleteFlg = "0"
                //    };

                //    DataTable mdt = mibl.M_Souko_IsExists(me);
                //    if (mdt.Rows.Count > 0)
                //    {
                //        if (!base.CheckAvailableStores(mdt.Rows[0]["StoreCD"].ToString()))
                //        {
                //            bbl.ShowMessage("E141");
                //            detailControls[index].Focus();
                //            return false;
                //        }
                //    }
                //    else
                //    {
                //        bbl.ShowMessage("E101");
                //        detailControls[index].Focus();
                //        return false;
                //    }

                //    break;

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

                case (int)EIndex.OrderCD:
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
                            ClearCustomerInfo();
                            return false;
                        }
                        ScOrder.LabelText = mve.VendorName;
                        mAmountFractionKBN = Convert.ToInt16(mve.AmountFractionKBN);
                        mTaxFractionKBN = Convert.ToInt16(mve.TaxFractionKBN);
                        mTaxTiming = Convert.ToInt16(mve.TaxTiming);

                        if (mOldOrderCD != detailControls[index].Text)
                        {
                            radioButton2.Checked = true;

                            if (mve.EDIFlg == "1")
                            {
                                lblEDI.Text = "EDI発注(FTP)";
                            }
                            else if (mve.EDIFlg == "2")
                            {
                                lblEDI.Text = "EDI発注(Mail)";
                            }
                            else
                            {
                                lblEDI.Text = "";
                            }
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }

                    mOldOrderCD = detailControls[index].Text;    //位置確認

                    break;
            }

            return true;
        }
        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false)
        {
            bool ret = false;

            string ymd = detailControls[(int)EIndex.OrderDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if(w_CtlRow < ClsGridHacchuu.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    {
                        if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                            return false;
                    }
            }

            switch (col)
            {
                case (int)ClsGridHacchuu.ColNO.JanCD:
                    if (!changeYmd)
                    {
                        if (mGrid.g_DArray[row].JanCD == mGrid.g_DArray[row].OldJanCD)   //chkAll &&  change
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

                    SKU_BL mbl = new SKU_BL();
                    DataTable dt = mbl.M_SKU_SelectAll(mse);
                    DataRow selectRow = null;

                    if (dt.Rows.Count == 0)
                    {
                        mGrid.g_DArray[row].OldJanCD = "";

                        //Ｅ１０１
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        selectRow = dt.Rows[0];
                    }
                    else
                    {
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
                        //Set品の場合
                        if (selectRow["SetKBN"].ToString() == "1")
                        {
                            mGrid.g_DArray[row].OldJanCD = "";

                            //Ｅ１７２
                            bbl.ShowMessage("E172");
                            return false;
                        }
                        //値引商品の場合
                        if (selectRow["DiscountKBN"].ToString() == "1")
                        {
                            mGrid.g_DArray[row].OldJanCD = "";

                            //Ｅ１７３
                            bbl.ShowMessage("E173");
                            return false;
                        }

                        //JANCDでSKUCDが１つだけ存在する場合（If there is only one）
                        mGrid.g_DArray[row].AdminNO = selectRow["AdminNO"].ToString();
                        mGrid.g_DArray[row].MakerItem = selectRow["MakerItem"].ToString();
                        mGrid.g_DArray[row].SKUCD = selectRow["SKUCD"].ToString();
                        mGrid.g_DArray[row].SKUName = selectRow["SKUName"].ToString();
                        mGrid.g_DArray[row].ColorName = selectRow["ColorName"].ToString();
                        mGrid.g_DArray[row].SizeName = selectRow["SizeName"].ToString();
                        mGrid.g_DArray[row].DiscountKbn = Convert.ToInt16(selectRow["DiscountKbn"].ToString());
                        mGrid.g_DArray[row].TaniCD = selectRow["TaniCD"].ToString();
                        mGrid.g_DArray[row].TaniName = selectRow["TaniName"].ToString();
                        mGrid.g_DArray[row].TaxRateFLG = Convert.ToInt16(selectRow["TaxRateFLG"].ToString());

                        mGrid.g_DArray[row].VariousFLG = Convert.ToInt16(selectRow["VariousFLG"].ToString());
                        mGrid.g_DArray[row].ZaikoKBN = Convert.ToInt16(selectRow["ZaikoKBN"].ToString());

                        mGrid.g_DArray[row].PriceOutTax = bbl.Z_SetStr(selectRow["PriceOutTax"]);
                        mGrid.g_DArray[row].IndividualClientName = selectRow["OrderAttentionNote"].ToString();
                        if (selectRow["EDIOrderFlg"].ToString() == "1")
                            mGrid.g_DArray[row].EDIOrderFlg = true;
                        else
                            mGrid.g_DArray[row].EDIOrderFlg = false;

                        //VariousFLG＝0	の場合	
                        if (mGrid.g_DArray[row].VariousFLG == 0)
                        {
                            //仕入先との組み合わせでITEM発注単価マスターかJAN発注単価マスタに存在すること SelectできなければError
                            //①②③④の順番でターゲットが大きくなる（①に近いほど、商品が特定されていく）

                            //[M_JANOrderPrice]
                            M_JANOrderPrice_Entity mje = new M_JANOrderPrice_Entity
                            {

                                //①JAN発注単価マスタ（店舗指定なし）
                                AdminNO = mGrid.g_DArray[row].AdminNO,
                                VendorCD = detailControls[(int)EIndex.OrderCD].Text,
                                StoreCD = CboStoreCD.SelectedValue.ToString(),
                                ChangeDate = ymd
                            };

                            JANOrderPrice_BL jbl = new JANOrderPrice_BL();
                            ret = jbl.M_JANOrderPrice_Select(mje);
                            if (ret)
                            {
                                mGrid.g_DArray[row].OrderUnitPrice = bbl.Z_SetStr(mje.PriceWithoutTax);
                            }
                            else
                            {
                                //②	JAN発注単価マスタ（店舗指定あり）
                                mje.StoreCD = "0000";

                                ret = jbl.M_JANOrderPrice_Select(mje);
                                if (ret)
                                {
                                    mGrid.g_DArray[row].OrderUnitPrice = bbl.Z_SetStr(mje.PriceWithoutTax);
                                }
                                else
                                {
                                    //[M_ItemOrderPrice]
                                    M_ItemOrderPrice_Entity mje2 = new M_ItemOrderPrice_Entity
                                    {

                                        //③	ITEM発注単価マスター（店舗指定あり）	
                                        MakerItem = mGrid.g_DArray[row].MakerItem,
                                        VendorCD = detailControls[(int)EIndex.OrderCD].Text,
                                        ChangeDate = ymd,
                                        StoreCD = CboStoreCD.SelectedValue.ToString()
                                    };

                                    ItemOrderPrice_BL ibl = new ItemOrderPrice_BL();
                                    ret = ibl.M_ItemOrderPrice_Select(mje2);
                                    if (ret)
                                    {
                                        mGrid.g_DArray[row].OrderUnitPrice = bbl.Z_SetStr(mje2.PriceWithoutTax);
                                    }
                                    else
                                    {
                                        //④	ITEM発注単価マスター（店舗指定なし）
                                        mje2.StoreCD = "0000";
                                        ret = ibl.M_ItemOrderPrice_Select(mje2);
                                        if (ret)
                                        {
                                            mGrid.g_DArray[row].OrderUnitPrice = bbl.Z_SetStr(mje2.PriceWithoutTax);
                                        }
                                        else
                                        {
                                            mGrid.g_DArray[row].OldJanCD = "";

                                            bbl.ShowMessage("E170");
                                            return false;
                                        }
                                    }
                                }
                            }
                            //発注単価×発注数	
                            mGrid.g_DArray[row].OrderGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].OrderUnitPrice) * bbl.Z_Set(mGrid.g_DArray[row].OrderSu));
                        }

                        mGrid.g_DArray[row].OldJanCD = mGrid.g_DArray[row].JanCD;
                        Grid_NotFocus(col, row);
                    }

                    break;

                case (int)ClsGridHacchuu.ColNO.SKUName:
                    //入力できる場合(When input is possible)
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

                case (int)ClsGridHacchuu.ColNO.ColorName:
                    //入力できる場合(When input is possible)
                    if (mGrid.g_MK_State[col, row].Cell_Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ColorName))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    break;

                case (int)ClsGridHacchuu.ColNO.SizeName:
                    //入力できる場合(When input is possible)
                    if (mGrid.g_MK_State[col, row].Cell_Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SizeName))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    break;

                case (int)ClsGridHacchuu.ColNO.MakerItem:
                    //入力できる場合(When input is possible)
                    if (mGrid.g_MK_State[col, row].Cell_Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].MakerItem))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    break;

                case (int)ClsGridHacchuu.ColNO.Rate:
                    //入力無くても良い(It is not necessary to input)
                    //入力無い場合、0とする（When there is no input, it is set to 0）
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].Rate))
                    {
                        mGrid.g_DArray[row].Rate = "0";
                    }
                    else if (bbl.Z_Set(mGrid.g_DArray[row].Rate) != 0)
                    {
                        //発注単価＝定価×掛率÷100	端数処理はM_Vendor.AmountFractionKBN
                        int tanka = GetResultWithHasuKbn(mAmountFractionKBN, bbl.Z_Set(mGrid.g_DArray[row].PriceOutTax) * bbl.Z_Set(mGrid.g_DArray[row].Rate) / 100);
                        mGrid.g_DArray[row].OrderUnitPrice = bbl.Z_SetStr(tanka);

                        //変更された場合、金額再計算 発注単価×発注数
                        mGrid.g_DArray[row].OrderGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].OrderUnitPrice) * bbl.Z_Set(mGrid.g_DArray[row].OrderSu));

                        CalcZei(row, ymd);
                    }
                    break;

                case (int)ClsGridHacchuu.ColNO.DesiredDeliveryDate:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].JanCD))
                    {
                    }
                    else
                    {
                        mGrid.g_DArray[row].DesiredDeliveryDate = bbl.FormatDate(mGrid.g_DArray[row].DesiredDeliveryDate);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(mGrid.g_DArray[row].DesiredDeliveryDate))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }
                        //入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(mGrid.g_DArray[row].DesiredDeliveryDate))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            return false;
                        }
                        //発注日以上の日であること
                        int result = mGrid.g_DArray[row].DesiredDeliveryDate.CompareTo(ymd);
                        if (result < 0)
                        {
                            bbl.ShowMessage("E104");
                            return false;
                        }
                    }
                    break;

                case (int)ClsGridHacchuu.ColNO.OrderSu:
                    //入力無い場合、0とする（When there is no input, it is set to 0）
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].OrderSu))
                    {
                        mGrid.g_DArray[row].OrderSu = "0";
                        //Ｑ３０５
                        if (bbl.ShowMessage("Q305") != DialogResult.OK)
                            return false;
                    }
                    if (ckM_CheckBox1.Checked)  //返品のときのみ数量*(-1)
                    {
                        mGrid.g_DArray[row].OrderSu = bbl.Z_SetStr(-1 * Math.Abs( bbl.Z_Set(mGrid.g_DArray[row].OrderSu)));   // 
                        //変更された場合、金額再計算 発注単価×発注数
                        mGrid.g_DArray[row].OrderGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].OrderUnitPrice) * bbl.Z_Set(mGrid.g_DArray[row].OrderSu));
                    }

                    if (!chkAll)
                        CalcZei(row, ymd);

                    break;

                case (int)ClsGridHacchuu.ColNO.OrderUnitPrice:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].OrderUnitPrice))
                    {
                        mGrid.g_DArray[row].OrderUnitPrice = "0";
                        //Ｑ３０６
                        if (bbl.ShowMessage("Q306") != DialogResult.OK)
                            return false;
                    }
                    if (!chkAll)
                        CalcZei(row, ymd);

                    break;

                case (int)ClsGridHacchuu.ColNO.OrderGaku:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].OrderGaku))
                    {
                        //０を許す。
                        mGrid.g_DArray[row].OrderGaku = "0";
                    }

                    CalcZei(row, ymd);

                    break;
            }

            switch (col)
            {
                //case (int)ClsGridHacchuu.ColNO.JanCD:
                case (int)ClsGridHacchuu.ColNO.OrderSu:
                case (int)ClsGridHacchuu.ColNO.Rate:
                case (int)ClsGridHacchuu.ColNO.OrderUnitPrice: //販売単価 
                case (int)ClsGridHacchuu.ColNO.OrderGaku: //発注額

                    //各金額項目の再計算必要
                    if (chkAll == false)
                        CalcKin();

                    break;

            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        private void CalcZei(int row, string ymd)
        {
            M_SalesTax_Entity mste = new M_SalesTax_Entity
            {
                ChangeDate = ymd
            };

            SalesTax_BL msbl = new SalesTax_BL();

          bool  ret = msbl.M_SalesTax_Select(mste);

            if (mGrid.g_DArray[row].TaxRateFLG == 1)
            {
                mGrid.g_DArray[row].TaxRate = bbl.Z_Set(mste.TaxRate1);
                ///通常税額←M_SKU.TaxRateFLG＝1	の時の発注額×税率×(1/100)			
                mGrid.g_DArray[row].HacchuTax = GetResultWithHasuKbn(mTaxFractionKBN, bbl.Z_Set(mGrid.g_DArray[row].OrderGaku) * mGrid.g_DArray[row].TaxRate / 100);
                mGrid.g_DArray[row].KeigenTax = 0;
            }
            else if (mGrid.g_DArray[row].TaxRateFLG == 2)
            {
                mGrid.g_DArray[row].TaxRate = bbl.Z_Set(mste.TaxRate2);
                mGrid.g_DArray[row].HacchuTax = 0;
                //軽減税額←M_SKU.TaxRateFLG＝2の時の発注額×税率×(1/100)			
                mGrid.g_DArray[row].KeigenTax = GetResultWithHasuKbn(mTaxFractionKBN, bbl.Z_Set(mGrid.g_DArray[row].OrderGaku) * mGrid.g_DArray[row].TaxRate / 100);
            }
            else
            {
                mGrid.g_DArray[row].TaxRate = 0;
                mGrid.g_DArray[row].HacchuTax = 0;
                mGrid.g_DArray[row].KeigenTax = 0;
            }
        }

        /// <summary>
        /// Footer部 金額計算処理
        /// </summary>
        private void CalcKin()
        {
            decimal kin1 = 0;
            decimal zei10 = 0;
            decimal zei8 = 0;

            //M_Vendor.TaxTiming＝2:伝票ごと
            decimal kin10 = 0;
            decimal kin8 = 0;
            int zeiritsu10 = 0;
            int zeiritsu8 = 0;
            int maxKinRowNo = 0;
            decimal maxKin = 0;

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                {
                    kin1 += bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku);
                    zei10 += bbl.Z_Set(mGrid.g_DArray[RW].HacchuTax);
                    zei8 += bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax);

                    //M_Vendor.TaxTiming＝2:伝票ごと
                    if (mTaxTiming.Equals(2))
                    {
                        if (mGrid.g_DArray[RW].TaxRateFLG.Equals(1))
                        {
                            kin10 += bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku);
                            if(zeiritsu10 == 0)
                                zeiritsu10 = Convert.ToInt16(mGrid.g_DArray[RW].TaxRate);
                        }
                        else if (mGrid.g_DArray[RW].TaxRateFLG.Equals(2))
                        {
                            kin8 += bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku);
                            if(zeiritsu8 == 0)
                                zeiritsu8 = Convert.ToInt16(mGrid.g_DArray[RW].TaxRate);
                        }

                        if (maxKin < bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku))
                        {
                            maxKin = bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku);
                            maxKinRowNo = RW;
                        }
                    }
                }
            }

            //Footer部
            lblKin1.Text = string.Format("{0:#,##0}", kin1);

            //M_Vendor.TaxTiming＝1:明細ごと または 3:締ごと	
            if (mTaxTiming.Equals(1) || mTaxTiming.Equals(3))
            {
                lblKin2.Text = string.Format("{0:#,##0}", zei10 + zei8);
                lblKin3.Text = string.Format("{0:#,##0}", kin1 + zei10 + zei8);

                mZei10 = zei10;
                mZei8 = zei8;
            }
            //M_Vendor.TaxTiming＝2:伝票ごと
            else
            {
                //通常税額(Hidden)=Form.Detail.発注額のTotal×Form.Detail.税率	※端数処理はM_Customerの設定に準ずる		
                //←M_SKU.TaxRateFLG＝1:通常課税の明細が対象
                kin10 = GetResultWithHasuKbn(mTaxFractionKBN, kin10 * zeiritsu10 / 100);
                kin8 = GetResultWithHasuKbn(mTaxFractionKBN, kin8 * zeiritsu8 / 100);

                decimal sagaku = (kin10 + kin8) - (zei10 + zei8);
                //通常税額(Hidden) ＋ 軽減税額(Hidden)　≠　SUM（Form.Detail.通常税額＋Form.Detail.軽減税額）の場合、
                //以下の計算結果「消費税差額」を明細金額が一番大きい金額（全明細が同じ金額の場合は１行目）の明細発注額、明細消費税額に足し込む。
                if (sagaku != 0)
                {
                    //※消費税差額＝通常税額(Hidden) ＋ 軽減税額(Hidden)）－ SUM（Form.Detail.通常税額 ＋ Form.Detail.軽減税額）	
                    if (mGrid.g_DArray[maxKinRowNo].TaxRateFLG.Equals(1))
                    {
                        mGrid.g_DArray[maxKinRowNo].HacchuTax = mGrid.g_DArray[maxKinRowNo].HacchuTax + sagaku;                        
                    }
                    else if (mGrid.g_DArray[maxKinRowNo].TaxRateFLG.Equals(2))
                    {
                        mGrid.g_DArray[maxKinRowNo].KeigenTax = mGrid.g_DArray[maxKinRowNo].KeigenTax + sagaku;
                    }
                }
                lblKin2.Text = string.Format("{0:#,##0}", kin10 + kin8 );
                lblKin3.Text = string.Format("{0:#,##0}", kin1 + kin10 + kin8);

                mZei10 = kin10;
                mZei8 = kin8;

            }

        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Order_Entity GetEntity()
        {
            doe = new D_Order_Entity();
            doe.OrderNO = keyControls[(int)EIndex.OrderNO].Text;
            doe.StoreCD = CboStoreCD.SelectedValue.ToString();

            doe.OrderDate = detailControls[(int)EIndex.OrderDate].Text;

            if (ckM_CheckBox1.Checked)
                doe.ReturnFLG = "1";
            else if (ckM_CheckBox2.Checked)
                doe.ReturnFLG = "2";
            else
                doe.ReturnFLG = "0";

            if (CboSoukoName.SelectedIndex <= 0)
            {
                doe.DestinationSoukoCD = "";
            }
            else
            {
                doe.DestinationSoukoCD = CboSoukoName.SelectedValue.ToString();
            }
            doe.StaffCD = detailControls[(int)EIndex.StaffCD].Text;
            doe.OrderCD = detailControls[(int)EIndex.OrderCD].Text;
            doe.OrderPerson = detailControls[(int)EIndex.OrderName].Text;

            if (Chk_Souko.Checked)  //自社倉庫
                doe.DestinationKBN = "2";
            else
                doe.DestinationKBN = "1";

            doe.DestinationName = detailControls[(int)EIndex.DestinationName].Text;
            doe.DestinationZip1CD = detailControls[(int)EIndex.ZipCD1].Text;
            doe.DestinationZip2CD = detailControls[(int)EIndex.ZipCD2].Text;
            doe.DestinationAddress1 = detailControls[(int)EIndex.Address1].Text;
            doe.DestinationAddress2 = detailControls[(int)EIndex.Address2].Text;
            doe.DestinationTelphoneNO = detailControls[(int)EIndex.Tel].Text;
            doe.DestinationFaxNO = detailControls[(int)EIndex.Fax].Text;

            if (radioButton1.Checked)
                doe.AliasKBN = "2";
            else
                doe.AliasKBN = "1";

            doe.OrderGaku = (bbl.Z_Set(lblKin1.Text) + (mZei8 + mZei10)).ToString();
            doe.OrderHontaiGaku = bbl.Z_Set(lblKin1.Text).ToString();
            doe.OrderTax8 = mZei8.ToString();
            doe.OrderTax10 = mZei10.ToString();

            doe.CommentOutStore = detailControls[(int)EIndex.RemarksOutStore].Text;
            doe.CommentInStore = detailControls[(int)EIndex.RemarksInStore].Text;

            doe.ArrivalPlanDate = mArrivalPlanDate;

            if (BtnSubF11.Enabled)
                doe.ApprovalEnabled = "1";
            else
                doe.ApprovalEnabled = "0";

            doe.ApprovalStageFLG = W_ApprovalStageFLG.ToString();
            if (lblDisp.Text == "却下")
            {
                doe.ApprovalStageFLG = "-1";
            }

            return doe;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("OrderRows", typeof(int));
            dt.Columns.Add("DisplayRows", typeof(int));
            dt.Columns.Add("SKUNO", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("MakerItem", typeof(string));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("OrderSuu", typeof(int));
            dt.Columns.Add("OrderUnitPrice", typeof(decimal));
            dt.Columns.Add("TaniCD", typeof(string));
            dt.Columns.Add("PriceOutTax", typeof(decimal));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("OrderGaku", typeof(decimal));
            dt.Columns.Add("OrderHontaiGaku", typeof(decimal));
            dt.Columns.Add("OrderTax", typeof(decimal));
            dt.Columns.Add("OrderTaxRitsu", typeof(int));
            dt.Columns.Add("SoukoCD", typeof(string));
            dt.Columns.Add("DirectFLG", typeof(int));
            dt.Columns.Add("EDIFLG", typeof(int));
            dt.Columns.Add("DesiredDeliveryDate", typeof(DateTime));
            dt.Columns.Add("CommentOutStore", typeof(string));
            dt.Columns.Add("CommentInStore", typeof(string));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            if (OperationMode == EOperationMode.UPDATE)
            {
                rowNo = m_MaxOrderGyoNo + 1;
            }

            string soukoCD = "";
            if (CboSoukoName.SelectedIndex > 0)
            {
                soukoCD = CboSoukoName.SelectedValue.ToString();
            }
            int directFLG = 0;
            if (ckM_CheckBox4.Checked)
                directFLG = 1;

            mArrivalPlanDate = "";

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //zが更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                {
                    int EDIFLG = 0;
                    if (mGrid.g_DArray[RW].EDIOrderFlg)
                        EDIFLG = 1;

                    if (mArrivalPlanDate.CompareTo(mGrid.g_DArray[RW].DesiredDeliveryDate) < 0)
                        mArrivalPlanDate = mGrid.g_DArray[RW].DesiredDeliveryDate;

                    dt.Rows.Add(mGrid.g_DArray[RW].hacchuGyoNO > 0 ? mGrid.g_DArray[RW].hacchuGyoNO : rowNo
                        , mGrid.g_DArray[RW].GYONO
                        , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                        , mGrid.g_DArray[RW].SKUCD == "" ? null : mGrid.g_DArray[RW].SKUCD
                        , mGrid.g_DArray[RW].JanCD == "" ? null : mGrid.g_DArray[RW].JanCD
                        , mGrid.g_DArray[RW].MakerItem == "" ? null : mGrid.g_DArray[RW].MakerItem
                        , mGrid.g_DArray[RW].SKUName == "" ? null : mGrid.g_DArray[RW].SKUName
                        , mGrid.g_DArray[RW].ColorName == "" ? null : mGrid.g_DArray[RW].ColorName
                        , mGrid.g_DArray[RW].SizeName == "" ? null : mGrid.g_DArray[RW].SizeName
                        , bbl.Z_Set(mGrid.g_DArray[RW].OrderSu)
                        , bbl.Z_Set(mGrid.g_DArray[RW].OrderUnitPrice)
                        , mGrid.g_DArray[RW].TaniCD == "" ? null : mGrid.g_DArray[RW].TaniCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].PriceOutTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].Rate)
                        , bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku) + bbl.Z_Set(mGrid.g_DArray[RW].HacchuTax) + bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].OrderGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].HacchuTax) + bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].TaxRate)
                        , soukoCD == "" ? null : soukoCD
                        , directFLG
                        , EDIFLG
                        , mGrid.g_DArray[RW].DesiredDeliveryDate == "" ? null : mGrid.g_DArray[RW].DesiredDeliveryDate
                        , mGrid.g_DArray[RW].CommentOutStore == "" ? null : mGrid.g_DArray[RW].CommentOutStore
                        , mGrid.g_DArray[RW].CommentInStore == "" ? null : mGrid.g_DArray[RW].CommentInStore
                        , mGrid.g_DArray[RW].hacchuGyoNO > 0 ? 1 : 0
                        );

                    if (mGrid.g_DArray[RW].hacchuGyoNO == 0)
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

                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                    {
                        for (int CL = (int)ClsGridHacchuu.ColNO.JanCD; CL < (int)ClsGridHacchuu.ColNO.COUNT; CL++)
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
            doe = GetEntity();
            hnbl.Order_Exec(doe, dt, (short)OperationMode, InOperatorCD, InPcID);

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");

            if (mSyouninKidou)
            {
                //メッセージ表示なし
                EndSec();
                return;
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

        private void SetApprovalStage(bool init = false)
        {
            //「承認」を初期表示。
            //ボタンを押すたびに「承認」「却下」のモードを変更する。
            if (init)
            {
                if (W_ApprovalStageFLG.Equals(9))
                {
                    //最終承認者の場合は承認済みにする
                    SetlblDisp("承認");
                }
                else
                {
                    SetlblDisp("承認中");
                }
            }
            else if (lblDisp.Text == "承認中" || lblDisp.Text == "承認")
            {
                SetlblDisp("却下");
            }
            else
            {
                SetlblDisp("承認");
            }
            ////F11ボタンは、
            ////その発注が「申請」「承認中」の場合に表示＆利用可能。以外は表示しない。
            //if (lblDisp.Text == "申請" || lblDisp.Text == "承認中")
            //    BtnSubF11.Enabled = true;
            //else
            //    BtnSubF11.Enabled = false;
        }
        private void SetlblDisp(string text)
        {
            lblDisp.Text = text;
            lblDisp.BackColor = Color.LightPink;

            switch (text)
            {
                case "申請":
                case "申請中":
                case "承認":
                case "承認中":
                    lblDisp.BackColor = Color.LemonChiffon;
                    break;
                case "却下":
                    lblDisp.BackColor = Color.Plum;
                    break;
                case "承認済":
                    lblDisp.BackColor = Color.PowderBlue;
                    break;
                default:
                    lblDisp.Text = "";
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

                SetApprovalStage(true);
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(Panel)))
                {
                    radioButton1.Checked = true;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
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
            ClearCustomerInfo();
            mZipCD = "";

            foreach (Control ctl in detailLabels)
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }

            mOldOrderDate = "";
            S_Clear_Grid();   //画面クリア（明細部）

            lblKin1.Text = "";
            lblKin2.Text = "";
            lblKin3.Text = "";

            SetEnabled(EIndex.CheckBox3, false);
            SetEnabled(EIndex.CheckBox4, false);
        }

        /// <summary>
        /// 仕入先情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            mOldOrderCD = "";
            ScOrder.LabelText = "";
            lblEDI.Text = "";
            mAmountFractionKBN = 0;
            mTaxFractionKBN = 0;
            mTaxTiming = 0;
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
                            //返品チェックがONのときのみ入力可
                            ScMotoOrderNo.Enabled = false;
                            ScMotoOrderNo.BtnSearch.Enabled = false;

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

                            //SetBtnSubF11Enabled(Kbn == 0 ? true : false);
                            Pnl_Body.Enabled = Kbn == 0 ? true : false;

                            SetEnabled(EIndex.CheckBox3, Chk_Souko.Checked);
                            SetEnabled(EIndex.CheckBox4, ckM_CheckBox4.Checked);
                            break;
                        }
                }
            }
        }

        private void SetBtnSubF11Enabled(bool enabled)
        {
            if (OperationMode == EOperationMode.DELETE || OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT)
                enabled = false;

            BtnSubF11.Enabled = enabled;
            SetFuncKey(this, 10, enabled);
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

                    switch (w_Col)
                    {
                        case (int)ClsGridHacchuu.ColNO.JanCD:
                            //商品検索
                            kbn = EsearchKbn.Product;
                            break;
                    }
                    if (kbn != EsearchKbn.Null)
                        SearchData(kbn, previousCtrl);

                    break;

                case 10:

                    break;

                case 11:    //F12:登録
                    {
                        if (OperationMode == EOperationMode.DELETE)
                        { //Ｑ１０２		
                            if (bbl.ShowMessage("Q102") != DialogResult.Yes)
                                return;
                        }
                        else if (mSyouninKidou)
                        {
                            //メッセージ表示なし
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
            catch (Exception ex)
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
            switch (kbn)
            {
                case EsearchKbn.Product:
                    int w_Row;

                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    w_Row = w_CtlRow + Vsb_Mei_0.Value;

                    //画面より配列セット 
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    using (Search_Product frmProduct = new Search_Product(detailControls[(int)EIndex.OrderDate].Text))
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
                            
                            if (CheckGrid((int)ClsGridHacchuu.ColNO.JanCD, w_Row, false, true) == false)
                            {
                                //Focusセット処理
                                ERR_FOCUS_GRID_SUB((int)ClsGridHacchuu.ColNO.JanCD, w_Row);
                                return;
                            }
                            //配列の内容を画面へセット
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                            SendKeys.Send("{ENTER}");
                        }
                    }
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
                            detailControls[(int)EIndex.OrderDate].Focus();
                        }
                        else if (index == (int)EIndex.OrderNO)
                            if (OperationMode == EOperationMode.UPDATE)
                                detailControls[(int)EIndex.OrderDate].Focus();
                            else
                                keyControls[index + 1].Focus();
                        else if (index == (int)EIndex.CopyOrderNO)
                            if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                                keyControls[index + 1].Focus();
                            else
                                detailControls[(int)EIndex.OrderDate].Focus();
                        else if (keyControls[index + 1].CanFocus)
                            keyControls[index + 1].Focus();
                        else
                            //あたかもTabキーが押されたかのようにする
                            //Shiftが押されている時は前のコントロールのフォーカスを移動
                            ProcessTabKey(!e.Shift);
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
                        if (index == (int)EIndex.Fax)
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridHacchuu.ColNO.JanCD);
                        else if (Chk_Souko.Checked && index == (int)EIndex.SoukoName)
                        {
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridHacchuu.ColNO.JanCD);
                        }
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

        private void KeyControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                //SetFuncKeyAll(this, "111111001011");
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
                //SetFuncKeyAll(this, "111111001011");
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

                //ただし、その明細が入荷予定日ありの明細である場合、削除できない（F7ボタンを使えないようにする）
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].ArrivePlanDate))
                {
                    Btn_F7.Enabled = false;
                }
                else
                    Btn_F7.Enabled = true;

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

                    bool lastCell = false;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridHacchuu.ColNO.IndividualClientName)
                        if (w_Row == mGrid.g_MK_Max_Row - 1)
                            lastCell = true;

                    bool changeFlg = false;
                    switch (CL)
                    {
                        case (int)ClsGridHacchuu.ColNO.OrderSu:
                            if (!mGrid.g_DArray[w_Row].OrderSu.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridHacchuu.ColNO.Rate:
                            if (!mGrid.g_DArray[w_Row].Rate.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridHacchuu.ColNO.OrderUnitPrice:
                            if (!mGrid.g_DArray[w_Row].OrderUnitPrice.Equals(w_ActCtl.Text))
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
                        switch (CL)
                        {
                            case (int)ClsGridHacchuu.ColNO.OrderSu:
                            case (int)ClsGridHacchuu.ColNO.Rate:
                            case (int)ClsGridHacchuu.ColNO.OrderUnitPrice:

                                //変更された場合、金額再計算 発注単価×発注数
                                mGrid.g_DArray[w_Row].OrderGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[w_Row].OrderUnitPrice) * bbl.Z_Set(mGrid.g_DArray[w_Row].OrderSu));
                                break;
                        }
                    }

                    //チェック処理
                    if (CheckGrid(CL, w_Row) == false)
                    {
                        //配列の内容を画面へセット
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                        //Focusセット処理
                        w_ActCtl.Focus();
                        return;
                    }

                    //データ呼び出し時に何度もCalcKinをReadしないようにするため移動
                    if (CL == (int)ClsGridHacchuu.ColNO.JanCD)
                        CalcKin();

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

                    if (CL == (int)ClsGridHacchuu.ColNO.EDIOrderFlg)
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
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;
                Control sc = ((Control)sender).Parent;

                //検索ボタンClick時
                if (((Search.CKM_SearchControl)sc).Name.Substring(0, 3).Equals("SC_"))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
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

        private void CkM_CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //返品チェックがONのときのみ入力可
                CKM_SearchControl edit = ScMotoOrderNo;
                if (ckM_CheckBox1.Checked)
                {
                    edit.Enabled = true;
                    edit.BtnSearch.Enabled = true;
                    ckM_CheckBox2.Checked = false;
                }
                else if (!ckM_CheckBox2.Checked)
                {
                    edit.Enabled = false;
                    edit.BtnSearch.Enabled = false;
                }


            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void CkM_CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CKM_SearchControl edit = ScMotoOrderNo;
                if (ckM_CheckBox2.Checked)
                {
                    edit.Enabled = true;
                    edit.BtnSearch.Enabled = true;
                    ckM_CheckBox1.Checked = false;
                }
                else if (!ckM_CheckBox1.Checked)
                {
                    edit.Enabled = false;
                    edit.BtnSearch.Enabled = false;
                }

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
                ////選択された出荷倉庫(倉庫CD)が変わった場合、明細の出荷倉庫CDも同じ倉庫CDに変更する。
                //if (CboStoreCD.SelectedValue != null)
                //    if (!CboStoreCD.SelectedValue.Equals("-1"))
                //    {
                //        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                //        {
                //            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                //            {
                //                mGrid.g_DArray[RW].SoukoName = CboSoukoName.SelectedValue.ToString();
                //            }
                //        }
                //        //配列の内容を画面にセット
                //        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                //    }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void CboStoreCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //オペレータが申請者（店舗ストアマスタの発注承認スタッフ以外）の場合、使用不可。
                if (CboStoreCD.SelectedIndex > 0)
                {
                    string ymd = detailControls[(int)EIndex.OrderDate].Text;

                    if (string.IsNullOrWhiteSpace(ymd))
                        ymd = bbl.GetDate();

                    W_ApprovalStageFLG = hnbl.GetApprovalStageFLG(InOperatorCD, CboStoreCD.SelectedValue.ToString());

                    mSyoninsya = false;
                    //[M_Store_SelectData]
                    M_Store_Entity mse = new M_Store_Entity
                    {
                        StoreCD = CboStoreCD.SelectedValue.ToString(),
                        ChangeDate = ymd
                    };
                    Store_BL sbl = new Store_BL();
                    DataTable dt = sbl.M_Store_Select(mse);
                    if (dt.Rows.Count > 0)
                    {
                        if (InOperatorCD.Equals(dt.Rows[0]["ApprovalStaffCD11"].ToString()) || InOperatorCD.Equals(dt.Rows[0]["ApprovalStaffCD12"].ToString())
                            || InOperatorCD.Equals(dt.Rows[0]["ApprovalStaffCD21"].ToString()) || InOperatorCD.Equals(dt.Rows[0]["ApprovalStaffCD22"].ToString())
                            || InOperatorCD.Equals(dt.Rows[0]["ApprovalStaffCD31"].ToString()) || InOperatorCD.Equals(dt.Rows[0]["ApprovalStaffCD32"].ToString()))
                        {
                            mSyoninsya = true;
                        }

                        SetBtnSubF11Enabled(mSyoninsya);


                    }

                    if (OperationMode == EOperationMode.INSERT && Chk_Souko.Checked)
                        SetInitSoukoName();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// その発注が「申請」「承認中」の場合に表示＆利用可能。以外は表示しない。
        /// 「承認」を初期表示。
        /// ボタンを押すたびに「承認」「却下」のモードを変更する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                string adminNo = mGrid.g_DArray[w_Row].AdminNO;



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
                SetEnabled(EIndex.CheckBox3, Chk_Souko.Checked);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void CkM_CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetEnabled(EIndex.CheckBox4, ckM_CheckBox4.Checked);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void BtnAddress_Click(object sender, EventArgs e)
        {
            try
            {
                CheckDetail((int)EIndex.ZipCD2);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void SetEnabled(EIndex index, bool enabled)
        {

            if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
            {
                switch (index)
                {
                    case EIndex.CheckBox3:
                        if (enabled)
                        {
                            detailControls[(int)EIndex.SoukoName].Enabled = enabled;

                            ckM_CheckBox4.Checked = !enabled;
                        }
                        else
                        {
                            ((CKM_Controls.CKM_ComboBox)detailControls[(int)EIndex.SoukoName]).SelectedIndex = -1;
                            detailControls[(int)EIndex.SoukoName].Enabled = enabled;
                        }
                        break;

                    case EIndex.CheckBox4:
                        if (enabled)
                        {
                            for (int i = (int)EIndex.DestinationName; i <= (int)EIndex.Fax; i++)
                            {
                                detailControls[i].Enabled = enabled;
                            }

                            Chk_Souko.Checked = !enabled;
                        }
                        else
                        {
                            for (int i = (int)EIndex.DestinationName; i <= (int)EIndex.Fax; i++)
                            {
                                detailControls[i].Enabled = enabled;
                                    detailControls[i].Text = "";
                            }
                        }
                        break;
                }
            }
        }

    }
}








