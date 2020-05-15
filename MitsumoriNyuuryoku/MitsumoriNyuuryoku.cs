using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace MitsumoriNyuuryoku
{
    /// <summary>
    /// MitsumoriNyuuryoku 見積入力
    /// </summary>
    internal partial class MitsumoriNyuuryoku : FrmMainForm
    {
        private const string ProID = "MitsumoriNyuuryoku";
        private const string ProNm = "見積入力";
        private const short mc_L_END = 3; // ロック用
        private const string Mitsumorisyo = "Mitsumorisyo.exe";
        private const string ZaikoSyokai = "ZaikoSyokai.exe";

        private enum EIndex : int
        {
            MitsumoriNO,
            CopyMitsumoriNO,
            StoreCD,

            MitsumoriDate = 0,
            JuchuChanceKbn,
            StaffCD,
            CustomerCD,
            CustomerName,
            AddressBtn,
            CustomerName2,
            AliasKBN,

            MitsumoriName,
            DeliveryDate,
            DeliveryPlace,
            PaymentTerms,
            ValidityPeriod,
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

        private FrmAddress addInfo;
        private MitsumoriNyuuryoku_BL mibl;
        private D_Mitsumori_Entity dme;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string InOperatorName = "";
        private string mOldMitsumoriNo = "";    //排他処理のため使用
        private string mOldMitsumoriDate = "";
        private string mOldCustomerCD = "";
        private decimal mZei10;//通常税額(Hidden)
        private decimal mZei8;//軽減税額(Hidden)

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridMitsumori mGrid = new ClsGridMitsumori();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数


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

            if (ClsGridMitsumori.gc_P_GYO <= ClsGridMitsumori.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridMitsumori.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridMitsumori.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridMitsumori.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridMitsumori.gc_MaxCL;

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
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        {
                            Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;

                            if (w_CtlCol == (int)ClsGridMitsumori.ColNO.Site)
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
            mGrid.g_DArray = new ClsGridMitsumori.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridMitsumori.ColNO.GYONO; i <= (int)ClsGridMitsumori.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridMitsumori.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:
                            mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            break;
                        case (int)ClsGridMitsumori.ColNO.MitsumoriHontaiGaku:
                        case (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice:
                        case (int)ClsGridMitsumori.ColNO.MitsumoriGaku:
                        case (int)ClsGridMitsumori.ColNO.CostUnitPrice:
                        case (int)ClsGridMitsumori.ColNO.CostGaku:
                        case (int)ClsGridMitsumori.ColNO.ProfitGaku:
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
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space1, 0].CellCtl = ckM_TextBox9;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.JanCD, 0].CellCtl = SC_ITEM_0;// IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SizeName, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostGaku, 0].CellCtl = IMN_GENKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriGaku, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriHontaiGaku, 0].CellCtl = IMN_TEIKA2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriSuu, 0].CellCtl = IMN_GENER_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaniCD, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ColorName, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.NotPrintFLG, 0].CellCtl = CHK_PRINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SetKBN, 0].CellCtl = IMN_CLINT2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ProfitGaku, 0].CellCtl = IMN_SALEP_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostUnitPrice, 0].CellCtl = IMN_SALEP2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentOutStore, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.IndividualClientName, 0].CellCtl = IMN_WEBPR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentInStore, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRateDisp, 0].CellCtl = IMT_ZKDIS_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRate, 0].CellCtl = IMT_ZEIRT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Site, 0].CellCtl = BTN_Site_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Zaiko, 0].CellCtl = BTN_Zaiko_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space2, 0].CellCtl = ckM_TextBox17;
            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space1, 1].CellCtl = ckM_TextBox10;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.JanCD, 1].CellCtl = SC_ITEM_1;//IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SizeName, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostGaku, 1].CellCtl = IMN_GENKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriGaku, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriHontaiGaku, 1].CellCtl = IMN_TEIKA2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriSuu, 1].CellCtl = IMN_GENER_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaniCD, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ColorName, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.NotPrintFLG, 1].CellCtl = CHK_PRINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SetKBN, 1].CellCtl = IMN_CLINT2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ProfitGaku, 1].CellCtl = IMN_SALEP_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostUnitPrice, 1].CellCtl = IMN_SALEP2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentOutStore, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.IndividualClientName, 1].CellCtl = IMN_WEBPR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentInStore, 1].CellCtl = IMT_REMAK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRateDisp, 1].CellCtl = IMT_ZKDIS_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRate, 1].CellCtl = IMT_ZEIRT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Site, 1].CellCtl = BTN_Site_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Zaiko, 1].CellCtl = BTN_Zaiko_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space2, 1].CellCtl = ckM_TextBox16;

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space1, 2].CellCtl = ckM_TextBox11;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.JanCD, 2].CellCtl = SC_ITEM_2;//IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SizeName, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostGaku, 2].CellCtl = IMN_GENKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriGaku, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriHontaiGaku, 2].CellCtl = IMN_TEIKA2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriSuu, 2].CellCtl = IMN_GENER_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaniCD, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ColorName, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.NotPrintFLG, 2].CellCtl = CHK_PRINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SetKBN, 2].CellCtl = IMN_CLINT2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ProfitGaku, 2].CellCtl = IMN_SALEP_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostUnitPrice, 2].CellCtl = IMN_SALEP2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentOutStore, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.IndividualClientName, 2].CellCtl = IMN_WEBPR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentInStore, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRateDisp, 2].CellCtl = IMT_ZKDIS_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRate, 2].CellCtl = IMT_ZEIRT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Site, 2].CellCtl = BTN_Site_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Zaiko, 2].CellCtl = BTN_Zaiko_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space2, 2].CellCtl = ckM_TextBox15;

            // 4行目
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space1, 3].CellCtl = ckM_TextBox12;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.JanCD, 3].CellCtl = SC_ITEM_3;// IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SizeName, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostGaku, 3].CellCtl = IMN_GENKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriGaku, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriHontaiGaku, 3].CellCtl = IMN_TEIKA2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriSuu, 3].CellCtl = IMN_GENER_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice, 3].CellCtl = IMN_GENER2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaniCD, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ColorName, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.NotPrintFLG,3].CellCtl = CHK_PRINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SetKBN, 3].CellCtl = IMN_CLINT2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ProfitGaku, 3].CellCtl = IMN_SALEP_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostUnitPrice, 3].CellCtl = IMN_SALEP2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentOutStore, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.IndividualClientName, 3].CellCtl = IMN_WEBPR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentInStore, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRateDisp, 3].CellCtl = IMT_ZKDIS_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRate, 3].CellCtl = IMT_ZEIRT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Site, 3].CellCtl = BTN_Site_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Zaiko, 3].CellCtl = BTN_Zaiko_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space2, 3].CellCtl = ckM_TextBox14;

            // 5行目
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space1, 4].CellCtl = ckM_TextBox8;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.JanCD, 4].CellCtl = SC_ITEM_4;//IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SizeName, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostGaku, 4].CellCtl = IMN_GENKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriGaku, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriHontaiGaku, 4].CellCtl = IMN_TEIKA2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriSuu, 4].CellCtl = IMN_GENER_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice, 4].CellCtl = IMN_GENER2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaniCD, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ColorName, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.NotPrintFLG, 4].CellCtl = CHK_PRINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.SetKBN, 4].CellCtl = IMN_CLINT2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.ProfitGaku, 4].CellCtl = IMN_SALEP_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CostUnitPrice, 4].CellCtl = IMN_SALEP2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentOutStore, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.IndividualClientName, 4].CellCtl = IMN_WEBPR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.CommentInStore, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRateDisp, 4].CellCtl = IMT_ZKDIS_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.TaxRate, 4].CellCtl = IMT_ZEIRT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Site, 4].CellCtl = BTN_Site_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Zaiko, 4].CellCtl = BTN_Zaiko_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMitsumori.ColNO.Space2, 4].CellCtl = ckM_TextBox13;
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvNxt, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvPrv, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvNxt, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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
                            case (int)ClsGridMitsumori.ColNO.GYONO:
                            case (int)ClsGridMitsumori.ColNO.Space1:
                            case (int)ClsGridMitsumori.ColNO.Space2:
                            case (int)ClsGridMitsumori.ColNO.Site:
                            case (int)ClsGridMitsumori.ColNO.Zaiko:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }
                            case (int)ClsGridMitsumori.ColNO.SKUCD:
                            case (int)ClsGridMitsumori.ColNO.SetKBN:
                            case (int)ClsGridMitsumori.ColNO.MitsumoriHontaiGaku:
                            case (int)ClsGridMitsumori.ColNO.TaxRateDisp:
                            case (int)ClsGridMitsumori.ColNO.TaniCD:
                            case (int)ClsGridMitsumori.ColNO.MitsumoriGaku:
                            case (int)ClsGridMitsumori.ColNO.TaxRate:
                            case (int)ClsGridMitsumori.ColNO.CostGaku:
                            case (int)ClsGridMitsumori.ColNO.ProfitGaku:
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
                        case (int)ClsGridMitsumori.ColNO.Site:
                            case(int)ClsGridMitsumori.ColNO.Zaiko:
                            {
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
        // 1...  新規/修正時(画面展開後)
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
                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridMitsumori.ColNO.JanCD:
                                        case (int)ClsGridMitsumori.ColNO.SKUName:    // 
                                        case (int)ClsGridMitsumori.ColNO.ColorName:    // 
                                        case (int)ClsGridMitsumori.ColNO.SizeName:
                                        case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:    // 
                                        case (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice:    // 
                                        case (int)ClsGridMitsumori.ColNO.CostUnitPrice:    // 
                                        case (int)ClsGridMitsumori.ColNO.CommentOutStore:    // 
                                        case (int)ClsGridMitsumori.ColNO.IndividualClientName:    //  
                                        case (int)ClsGridMitsumori.ColNO.CommentInStore:    //
                                        case (int)ClsGridMitsumori.ColNO.Site:    //
                                        case (int)ClsGridMitsumori.ColNO.Zaiko:    // 
                                        case (int)ClsGridMitsumori.ColNO.NotPrintFLG:    // 
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                break;
                                            }
                                    }
                                }
                            }
                            mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                keyControls[(int)EIndex.MitsumoriNO].Text = "";
                                keyControls[(int)EIndex.MitsumoriNO].Enabled = false;
                                ScMitsumoriNO.BtnSearch.Enabled = false;
                                keyControls[(int)EIndex.CopyMitsumoriNO].Enabled = true;
                                ScCopyMitsumoriNO.BtnSearch.Enabled = true;

                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                detailControls[(int)EIndex.MitsumoriDate].Text = bbl.GetDate();
                                F9Visible = false;

                                SetFuncKeyAll(this, "111111001001");
                            }
                            else
                            {
                                keyControls[(int)EIndex.MitsumoriNO].Enabled = true;
                                ScMitsumoriNO.BtnSearch.Enabled = true;
                                keyControls[(int)EIndex.CopyMitsumoriNO].Text = "";
                                keyControls[(int)EIndex.CopyMitsumoriNO].Enabled = false;
                                ScCopyMitsumoriNO.BtnSearch.Enabled = false;

                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                this.Vsb_Mei_0.TabStop = false;

                                SetFuncKeyAll(this, "111111001010");
                            }

                            SetEnabled(false);

                        }
                        break;
                    }

                case 1:
                    {
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;
                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridMitsumori.ColNO.JanCD:
                                        case (int)ClsGridMitsumori.ColNO.ColorName:    // 
                                        case (int)ClsGridMitsumori.ColNO.SizeName:
                                        case (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice:    // 
                                        case (int)ClsGridMitsumori.ColNO.CommentOutStore:    // 
                                        case (int)ClsGridMitsumori.ColNO.IndividualClientName:    //  
                                        case (int)ClsGridMitsumori.ColNO.CommentInStore:    //
                                        case (int)ClsGridMitsumori.ColNO.Site:    //
                                        case (int)ClsGridMitsumori.ColNO.Zaiko:    // 
                                        case (int)ClsGridMitsumori.ColNO.NotPrintFLG:    // 
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                
                                                break;
                                            }
                                        case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:
                                            if (mGrid.g_DArray[w_Row].DiscountKbn == 1)
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = true;
                                            }
                                            else
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = false;
                                            }
                                            break;
                                        case (int)ClsGridMitsumori.ColNO.SKUName:
                                        case (int)ClsGridMitsumori.ColNO.CostUnitPrice:
                                            if (mGrid.g_DArray[w_Row].VariousFLG == 1)
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = false;
                                            }
                                            else
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = true;
                                            }
                                            break;
                                    }
                                }
                                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
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
                                SetFuncKeyAll(this, "111111000011");
                                IMT_DMY_0.Focus();
                                Scr_Lock(0, 0, 1);
                            }
                            else
                            {
                                SetFuncKeyAll(this, "111111000010");
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
            //bool w_AllFlg = false;
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


            //w_AllFlg = true;

            for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                switch (w_Col)
                {
                    case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:
                        if (mGrid.g_DArray[pRow].DiscountKbn == 1)
                        {
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                            mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = true;
                            mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                        }
                        else
                        {
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                            mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = false;
                            mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                        }
                        break;
                    case (int)ClsGridMitsumori.ColNO.SKUName:
                    case (int)ClsGridMitsumori.ColNO.CostUnitPrice:
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

                //画面より配列セット 
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                bool Check = mGrid.g_DArray[w_Row].NotPrintFLG;

                if (w_Row == 0 && Check)
                    mGrid.g_DArray[w_Row].NotPrintFLG = false;

                if (string.IsNullOrWhiteSpace( mGrid.g_DArray[w_Row].JanCD))
                    mGrid.g_DArray[w_Row].NotPrintFLG = false;

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

        // -----------------------------------------------
        // <明細部>行削除処理 Ｆ７
        // -----------------------------------------------
        private void DEL_SUB()
        {
            int w_Row;

            if (mGrid.F_Search_Ctrl_MK(previousCtrl, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            for (int i = w_Row; i < mGrid.g_MK_Max_Row - 1; i++)
            {
             int   w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //次行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i + 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            CalcKin();

            int col = (int)ClsGridMitsumori.ColNO.JanCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //フォーカスセット
            IMT_DMY_0.Focus();

                //現在行へ
                mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

        }

        // -----------------------------------------------
        // <明細部>行複写処理 Ｆ６
        // -----------------------------------------------
        private void CPY_SUB()
        {
            Control w_Act = previousCtrl;
            int w_Row;
            int w_Gyo;            

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

            //コピー行より下の明細を1行ずつずらす（内容コピー）
            for (int i = mGrid.g_MK_Max_Row - 1; i >= w_Row; i--)
            {
                w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //前行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i - 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            //状態もコピー
            // ※ 前行と状態が違うとき注意、この部分変更要 (修正元のあるなしで 入力可能項目が変わる場合など)
            for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                mGrid.g_MK_State[w_Col, w_Row] = mGrid.g_MK_State[w_Col, w_Row - 1];
            }
            CalcKin();

            int col = (int)ClsGridMitsumori.ColNO.JanCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, IMT_DMY_0, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);
        }
        private void ADD_SUB()
        {
            Control w_Act = previousCtrl;
            int w_Row;
            int w_Gyo;

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

            int col = (int)ClsGridMitsumori.ColNO.JanCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

        }
        #endregion

        public MitsumoriNyuuryoku()
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
                base.InProgramID = ProID;

                this.SetFunctionLabel(EProMode.INPUT);
                this.InitialControlArray();
                // 明細部初期化
                this.S_SetInit_Grid();

                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();

                string ymd = bbl.GetDate();
                mibl = new MitsumoriNyuuryoku_BL();
                CboStoreCD.Bind(ymd);
                CboJuchuuChanceKBN.Bind(string.Empty);

                string stores = GetAllAvailableStores();
                ScMitsumoriNO.Value1 = InOperatorCD;
                ScMitsumoriNO.Value2 = stores;
                ScCopyMitsumoriNO.Value1 = InOperatorCD;
                ScCopyMitsumoriNO.Value2 = stores;
                ScCustomer.Value1 = "1";

                ScStaff.TxtCode.Text = InOperatorCD;

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = ymd
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
            keyControls = new Control[] {  ScMitsumoriNO.TxtCode, ScCopyMitsumoriNO.TxtCode , CboStoreCD};
            keyLabels = new Control[] {  };
            detailControls = new Control[] { ckM_TextBox1,CboJuchuuChanceKBN, ScStaff.TxtCode, ScCustomer.TxtCode,ckM_TextBox7,btnAddress, ckM_Text_4, panel1
                            , ckM_TextBox2, ckM_TextBox3, ckM_TextBox4, ckM_TextBox5,ckM_TextBox6,TxtRemark1,TxtRemark2,ChkPrint };
            detailLabels = new Control[] { ScCustomer,ScStaff };
            searchButtons = new Control[] { ScCustomer.BtnSearch};

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
        private bool CheckKey(int index, bool set=true)
        {

            switch (index)
            {
                case (int)EIndex.MitsumoriNO:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        if (OperationMode == EOperationMode.INSERT)
                        {
                            return true;
                        }
                        else
                        { 
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
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
                        return false;
                    }
                    else
                    {
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            bbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }

                    }

                    break;

                case (int)EIndex.CopyMitsumoriNO:
                    if (!string.IsNullOrWhiteSpace(keyControls[(int)EIndex.CopyMitsumoriNO].Text))
                        return CheckData(set, true);

                    break;
            }
            
            return true;

        }

        private bool SelectAndInsertExclusive()
        {
            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT)
                return true;
            
            DeleteExclusive();

            if (string.IsNullOrWhiteSpace(keyControls[(int)EIndex.MitsumoriNO].Text))
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Mitsumori,
                Number = keyControls[(int)EIndex.MitsumoriNO].Text,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(),dt.Rows[0]["Operator"].ToString());
                keyControls[(int)EIndex.MitsumoriNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldMitsumoriNo = keyControls[(int)EIndex.MitsumoriNO].Text;
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
       private void DeleteExclusive()
        {
            if (mOldMitsumoriNo == "")
                return;

            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Mitsumori,
                Number = mOldMitsumoriNo,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);

            mOldMitsumoriNo = "";
        }
        
        /// <summary>
        /// 単価データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set, bool copy=false)
        {
            //[D_Mitsumori_SelectData]
            dme = new D_Mitsumori_Entity();

            if(copy)
                dme.MitsumoriNO = keyControls[(int)EIndex.CopyMitsumoriNO].Text;
            else
                dme.MitsumoriNO = keyControls[(int)EIndex.MitsumoriNO].Text;

            DataTable dt = mibl.D_Mitsumori_SelectData(dme, (short)OperationMode);

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

                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;

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
                        detailControls[(int)EIndex.MitsumoriDate].Text = row["MitsumoriDate"].ToString();
                        mOldMitsumoriDate = detailControls[(int)EIndex.MitsumoriDate].Text;
                        CboJuchuuChanceKBN.SelectedValue = row["JuchuuChanceKBN"];
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

                        if (row["AliasKBN"].ToString() == "1")
                            radioButton1.Checked = true;
                        else
                            radioButton2.Checked = true;


                        detailControls[(int)EIndex.MitsumoriName].Text = row["MitsumoriName"].ToString();
                        detailControls[(int)EIndex.DeliveryDate].Text = row["DeliveryDate"].ToString();
                        detailControls[(int)EIndex.PaymentTerms].Text = row["PaymentTerms"].ToString();
                        detailControls[(int)EIndex.DeliveryPlace].Text = row["DeliveryPlace"].ToString();
                        detailControls[(int)EIndex.ValidityPeriod].Text = row["ValidityPeriod"].ToString();
                        detailControls[(int)EIndex.RemarksOutStore].Text = row["RemarksOutStore"].ToString();
                        detailControls[(int)EIndex.RemarksInStore].Text = row["RemarksInStore"].ToString();

                        lblKin1.Text = bbl.Z_SetStr(row["SUM_MitsumoriGaku"]);
                        lblKin2.Text = bbl.Z_SetStr(row["SUM_MitsumoriHontaiGaku"]); ;
                        lblKin3.Text = bbl.Z_SetStr(row["SUM_CostGaku"]); ;
                        lblKin4.Text = bbl.Z_SetStr(row["SUM_ProfitGaku"]); ;

                        ChkPrint.Checked = true;

                        //明細なしの場合
                        if (bbl.Z_Set(row["MitsumoriRows"]) == 0)
                            break;
                    }

                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    //mGrid.g_DArray[i].OldJanCD = mGrid.g_DArray[i].JanCD; del 4 / 24
                    mGrid.g_DArray[i].AdminNO = row["SKUNO"].ToString();
                    mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();
                    mGrid.g_DArray[i].MitsumoriSuu = bbl.Z_SetStr(row["MitsumoriSuu"]);   //単価算出のため先にセットしておく    
                    CheckGrid((int)ClsGridMitsumori.ColNO.JanCD, i);

                    mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();   // 
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 
                    //if (row["SetKBN"].ToString() == "1")
                    //    mGrid.g_DArray[i].SetKBN = "○";
                    //else
                    //    mGrid.g_DArray[i].SetKBN = "";
                    mGrid.g_DArray[i].NotPrintFLG = row["NotPrintFLG"].ToString() == "1" ? true : false;

                    mGrid.g_DArray[i].MitsumoriHontaiGaku = bbl.Z_SetStr(row["MitsumoriHontaiGaku"]);   // 
                    //mGrid.g_DArray[i].TaxRateDisp = bbl.Z_SetStr(row["MemberPriceOutTax"]);   // 
                    mGrid.g_DArray[i].MitsumoriUnitPrice = bbl.Z_SetStr(row["MitsumoriUnitPrice"]);   // 
                    mGrid.g_DArray[i].MitsumoriGaku = bbl.Z_SetStr(row["MitsumoriGaku"]);   // 
                    mGrid.g_DArray[i].CostUnitPrice = bbl.Z_SetStr(row["CostUnitPrice"]);   // 
                    mGrid.g_DArray[i].CostGaku = bbl.Z_SetStr(row["CostGaku"]);   //  
                    //mGrid.g_DArray[i].MitsumoriTax = bbl.Z_Set(row["CostGaku"]);  CheckGridでセット
                    //mGrid.g_DArray[i].KeigenTax = bbl.Z_Setrow["CostGaku"]);    CheckGridでセット

                    //mGrid.g_DArray[i].TaniName = bbl.Z_SetStr(row["TaniName"]);   // 
                    mGrid.g_DArray[i].CommentInStore = row["CommentInStore"].ToString();   // 
                    mGrid.g_DArray[i].CommentOutStore = row["CommentOutStore"].ToString();   // 
                    mGrid.g_DArray[i].IndividualClientName = row["IndividualClientName"].ToString();   // 

                    mGrid.g_DArray[i].ProfitGaku = bbl.Z_SetStr(row["ProfitGaku"]);   // 

                    m_dataCnt = i + 1;
                    Grid_NotFocus((int)ClsGridMitsumori.ColNO.JanCD, i);
                    i++;
                }

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            if (OperationMode == EOperationMode.UPDATE )
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                detailControls[(int)EIndex.MitsumoriDate].Focus();
            }
            else if(OperationMode == EOperationMode.INSERT)
            {
                //複写コピー後
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
            for (int i = 0; i < keyControls.Length; i++)
                if (CheckKey(i, true) == false)
                {
                    keyControls[i].Focus();
                    return;
                }

            if (OperationMode != EOperationMode.INSERT)
                CheckData(true);
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
                case (int)EIndex.MitsumoriDate:
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
                    if(!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }

                    //見積日が変更された場合のチェック処理
                    if (mOldMitsumoriDate != detailControls[index].Text)
                    {
                        for(int i=(int)EIndex.StaffCD; i<(int)EIndex.CustomerCD; i++)
                            if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                                if (!CheckDependsOnMitsumoriDate(i, true))
                                return false;

                        //明細部JANCDの再チェック
                        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                        {
                            if (CheckGrid((int)ClsGridMitsumori.ColNO.JanCD, RW, false, true) == false)
                            {
                                //Focusセット処理
                                ERR_FOCUS_GRID_SUB((int)ClsGridMitsumori.ColNO.JanCD, RW);
                                return false;
                            }
                        }
                        mOldMitsumoriDate = detailControls[index].Text;
                        ScCustomer.ChangeDate = mOldMitsumoriDate;
                    }

                    break;

                case (int)EIndex.JuchuChanceKbn:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
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

                    if (!CheckDependsOnMitsumoriDate(index))
                        return false;

                    break;

                case (int)EIndex.CustomerCD:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }
                    if (!CheckDependsOnMitsumoriDate(index))
                        return false;

                   
                    break;

                case (int)EIndex.CustomerName:
                    //入力可能の場合 入力必須(Entry required)
                    if (detailControls[index].Enabled && string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    break;

                case (int)EIndex.MitsumoriName:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
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

            w_Ctrl = detailControls[(int)EIndex.ValidityPeriod];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        /// <summary>
        /// 見積日が変更されたときに必要なチェック処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDependsOnMitsumoriDate(int index, bool ChangeMitsumoriDate = false)
        {

            switch (index)
            {
                case (int)EIndex.StaffCD:
                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = detailControls[(int)EIndex.MitsumoriDate].Text
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
                        return false;
                    }
                    break;

                case (int)EIndex.CustomerCD:
                    //[M_Customer_Select]
                    M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Customer_BL sbl = new Customer_BL();
                    //ret = sbl.M_Customer_Select(mce, 1);
                    ret = sbl.M_Customer_Select(mce);

                    if (ret)
                    {

                        //顧客CDが変更されている場合のみ再セット
                        if (mOldCustomerCD != detailControls[index].Text || ChangeMitsumoriDate)
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

                            detailControls[index + 1].Enabled = false;

                            if (mce.VariousFLG == "1")
                            {
                                detailControls[index + 1].Text = mce.CustomerName;
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                    detailControls[index + 1].Enabled = true;
                                detailControls[index + 3].Text = "";
                                textBox1.Text = mce.RemarksInStore;
                                textBox2.Text = mce.RemarksOutStore;
                                lblLastSalesDate.Text = "";
                                lblStoreName.Text = "";
                            }
                            else
                            {
                                detailControls[index + 1].Text = mce.CustomerName;
                                detailControls[index + 3].Text = "";
                                textBox1.Text = mce.RemarksInStore;
                                textBox2.Text = mce.RemarksOutStore;
                                lblLastSalesDate.Text = mce.LastSalesDate;

                                //[M_Store_Select]
                                M_Store_Entity me = new M_Store_Entity
                                {
                                    StoreCD = mce.LastSalesStoreCD,
                                    ChangeDate = mce.LastSalesDate
                                };
                                Store_BL stbl = new Store_BL();
                                DataTable sdt = stbl.M_Store_Select(me);
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
                                radioButton1.Checked = true;
                            }
                            else
                            {
                                radioButton2.Checked = true;
                            }
                            //単価設定
                            //[M_TankaCD]
                            M_TankaCD_Entity mte = new M_TankaCD_Entity
                            {
                                TankaCD = mce.TankaCD,
                                ChangeDate = detailControls[(int)EIndex.MitsumoriDate].Text
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
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }

                    mOldCustomerCD = detailControls[index].Text;    //位置確認

                    break;
            }

            return true;
        }
        private bool CheckGrid(int col, int row, bool chkAll=false, bool changeYmd=false)
        {
            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridMitsumori.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                        return false;
                }
            }

            switch (col)
            {
                case (int)ClsGridMitsumori.ColNO.JanCD:
                    string ymd = detailControls[(int)EIndex.MitsumoriDate].Text;

                    if (!changeYmd)
                    {
                        if (mGrid.g_DArray[row].JanCD == mGrid.g_DArray[row].OldJanCD) //chkAll &&  change
                            return true;
                    }

                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].JanCD))
                        return true;

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
                        if (mGrid.g_DArray[row].DiscountKbn == 1)
                            mGrid.g_DArray[row].MitsumoriSuu = "-1";    //GridNotFocusで入力不可にすること
                        mGrid.g_DArray[row].TaniCD = selectRow["TaniCD"].ToString();
                        mGrid.g_DArray[row].TaniName = selectRow["TaniName"].ToString();
                        mGrid.g_DArray[row].TaxRateFLG = Convert.ToInt16(selectRow["TaxRateFLG"].ToString());

                        mGrid.g_DArray[row].AdminNO = selectRow["AdminNO"].ToString();
                        mGrid.g_DArray[row].VariousFLG = Convert.ToInt16(selectRow["VariousFLG"].ToString());

                        decimal wSuu = bbl.Z_Set(mGrid.g_DArray[row].MitsumoriSuu);
                        
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

                        bool ret = bbl.Fnc_UnitPrice(fue);
                        if (ret)
                        {
                            //販売単価=Function_単価取得.out税込単価		
                            mGrid.g_DArray[row].MitsumoriUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeikomiTanka));

                            //原価単価=Function_単価取得.out原価単価	
                            mGrid.g_DArray[row].CostUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka));
                        }
                        else
                        {
                            //販売単価		
                            mGrid.g_DArray[row].MitsumoriUnitPrice = "0";
                            //原価単価
                            mGrid.g_DArray[row].CostUnitPrice = "0";
                        }

                        //	(Form.見積数＝Null	の場合は×１とする)
                        if (wSuu.Equals(0))
                            wSuu = 1;

                        //税抜販売額
                        if (mGrid.g_DArray[row].VariousFLG.Equals(1))
                        {
                            //（諸口の場合は入力された税込単価から税抜単価を都度計算する）
                            //税抜販売額＝Function_消費税計算.out金額１×Form.Detail.見積数
                            decimal tanka = bbl.GetZeinukiKingaku(bbl.Z_Set(mGrid.g_DArray[row].MitsumoriUnitPrice), mGrid.g_DArray[row].TaxRateFLG, ymd);

                            mGrid.g_DArray[row].MitsumoriHontaiGaku = string.Format("{0:#,##0}", tanka * wSuu);
                        }
                        else if (mGrid.g_DArray[row].VariousFLG.Equals(0))
                        {
                            //税抜販売額＝Form.見積数＝Nullの場合Function_単価取得.out税抜単価×Form.見積数
                            mGrid.g_DArray[row].MitsumoriHontaiGaku = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeinukiTanka) * wSuu);
                        }

                        //税込販売額=Form.見積数≠Nullの場合Function_単価取得.out税込単価×Form.Detail.見積数	
                        mGrid.g_DArray[row].MitsumoriGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].MitsumoriUnitPrice) * wSuu);
                        //原価額=Form.見積数≠Nullの場合Function_単価取得.out原価単価×Form.Detail.見積数
                        mGrid.g_DArray[row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].CostUnitPrice) * wSuu);
                        
                        //粗利額=⑧Form.税抜販売額－⑪Form.原価額				
                        mGrid.g_DArray[row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].MitsumoriHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[row].CostGaku));

                        //税率=Function_単価取得.out税率
                        if (bbl.Z_Set(fue.Zeiritsu) == 0)
                        {
                            mGrid.g_DArray[row].TaxRate = "";
                        }
                        else
                        {
                            mGrid.g_DArray[row].TaxRate = string.Format("{0:#,##0}", bbl.Z_Set(fue.Zeiritsu)) + "%";
                        }

                        if (mGrid.g_DArray[row].TaxRateFLG == 1)
                        {
                            mGrid.g_DArray[row].TaxRateDisp = "税込";
                            //通常税額=M_SKU.TaxRateFLG＝1の時のFunction_単価取得.out消費税額×Form.Detail.見積数
                            mGrid.g_DArray[row].MitsumoriTax = bbl.Z_Set(fue.Zei)*wSuu;
                            //軽減税額=0
                            mGrid.g_DArray[row].KeigenTax = 0;
                        }
                        else  if (mGrid.g_DArray[row].TaxRateFLG == 2)
                        {
                            mGrid.g_DArray[row].TaxRateDisp = "税込";
                            //通常税額=0
                            mGrid.g_DArray[row].MitsumoriTax = 0;
                            //軽減税額=TaxRateFLG＝2の時のFunction_単価取得.out消費税額×Form.Detail.見積数
                            mGrid.g_DArray[row].KeigenTax = bbl.Z_Set(fue.Zei) * wSuu;
                        }
                        else
                        {
                            mGrid.g_DArray[row].TaxRateDisp = "非税";
                            //通常税額=0
                            mGrid.g_DArray[row].MitsumoriTax = 0;
                            //軽減税額=0
                            mGrid.g_DArray[row].KeigenTax = 0;
                        }

                        M_Site_Entity msie = new M_Site_Entity
                        {
                            ItemSKUCD = mGrid.g_DArray[row].SKUCD
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

                        Grid_NotFocus(0, row);   
                    }

                    break;

                case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:
                    if (mGrid.g_DArray[row].NotPrintFLG)
                    {
                        if(mGrid.g_DArray[row].MitsumoriSuu != mGrid.g_DArray[row-1].MitsumoriSuu)
                        {
                            //Ｅ２１７				
                            bbl.ShowMessage("E217");
                            return false;
                        }
                    }

                    //各金額項目の再計算必要
                    if (chkAll == false)
                        CalcKin();

                    break;

                case (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice: //販売単価 
                case (int)ClsGridMitsumori.ColNO.CostUnitPrice: //原価単価

                    //各金額項目の再計算必要
                    if (chkAll == false)
                        CalcKin();

                        break;

            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        private void CalcKin()
        {
            decimal kin1 = 0;
            decimal kin2 = 0;
            decimal kin3 = 0;
            decimal kin4 = 0;
            decimal zei10 = 0;
            decimal zei8 = 0;

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriSuu) != 0)
                {
                    //粗利額←⑦Form.Detail.税抜販売額－⑩Form.Detail.原価額
                    mGrid.g_DArray[RW].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[RW].CostGaku));

                    kin1 += bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriGaku);
                    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriHontaiGaku);
                    zei10 += bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriTax);
                    zei8 += bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax);
                    kin3 += bbl.Z_Set(mGrid.g_DArray[RW].CostGaku);
                    kin4 += bbl.Z_Set(mGrid.g_DArray[RW].ProfitGaku);
                }
            }

            //Footer部
            //見積額・Form.Detail.税込販売額のTotal
            lblKin1.Text=string.Format("{0:#,##0}", kin1);
            //税抜見積額・Form.Detail.税抜販売額のTotal
            lblKin2.Text = string.Format("{0:#,##0}", kin2);
            //通常税額(Hidden)・Form.Detail.通常税額のTotal
            mZei10 =  zei10;
            //軽減税額(Hidden)・Form.Detail.軽減税額のTotal
            mZei8 = zei8;
            //原価額・Form.Detail.原価額のTotal
            lblKin3.Text = string.Format("{0:#,##0}", kin3);
            //粗利額・税抜見積額－原価額
            lblKin4.Text = string.Format("{0:#,##0}", kin4);

        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Mitsumori_Entity GetEntity()
        {
            dme = new D_Mitsumori_Entity();
            dme.MitsumoriNO = keyControls[(int)EIndex.MitsumoriNO].Text;
            dme.StoreCD = CboStoreCD.SelectedValue.ToString();

            dme.MitsumoriDate = detailControls[(int)EIndex.MitsumoriDate].Text;
            dme.JuchuuChanceKBN =CboJuchuuChanceKBN.SelectedValue.ToString();
            dme.StaffCD = detailControls[(int)EIndex.StaffCD].Text;
            dme.CustomerCD = detailControls[(int)EIndex.CustomerCD].Text;
            dme.CustomerName = detailControls[(int)EIndex.CustomerName].Text;
            dme.CustomerName2 = detailControls[(int)EIndex.CustomerName2].Text;
            dme.ZipCD1 = addInfo.ade.ZipCD1;
            dme.ZipCD2 = addInfo.ade.ZipCD2;
            dme.Address1 = addInfo.ade.Address1;
            dme.Address2 = addInfo.ade.Address2;
            dme.Tel11 = addInfo.ade.Tel11;
            dme.Tel12 = addInfo.ade.Tel12;
            dme.Tel13 = addInfo.ade.Tel13;
        
            if (radioButton1.Checked)
            dme.AliasKBN="1";
                    else
                dme.AliasKBN = "2";

            dme.MitsumoriName = detailControls[(int)EIndex.MitsumoriName].Text;
            dme.DeliveryDate = detailControls[(int)EIndex.DeliveryDate].Text;
            dme.PaymentTerms = detailControls[(int)EIndex.PaymentTerms].Text;
            dme.DeliveryPlace = detailControls[(int)EIndex.DeliveryPlace].Text;
            dme.ValidityPeriod = detailControls[(int)EIndex.ValidityPeriod].Text;
            dme.RemarksInStore = detailControls[(int)EIndex.RemarksInStore].Text;
            dme.RemarksOutStore = detailControls[(int)EIndex.RemarksOutStore].Text;

            dme.MitsumoriGaku =bbl.Z_SetStr( lblKin1.Text);
            dme.MitsumoriHontaiGaku = bbl.Z_SetStr(lblKin2.Text);
            dme.MitsumoriTax8= mZei8.ToString();
            dme.MitsumoriTax10= mZei10.ToString();
            dme.CostGaku = bbl.Z_SetStr(lblKin3.Text);
            dme.ProfitGaku = bbl.Z_SetStr(lblKin4.Text);
       
            return dme;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("MitsumoriRows", typeof(int));
            dt.Columns.Add("DisplayRows", typeof(int));
            dt.Columns.Add("SKUNO", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("NotPrintFLG", typeof(int));
            dt.Columns.Add("SetKBN", typeof(int));
            dt.Columns.Add("MitsumoriSuu", typeof(int));
            dt.Columns.Add("MitsumoriUnitPrice", typeof(decimal));
            dt.Columns.Add("TaniCD", typeof(string));
            dt.Columns.Add("MitsumoriGaku", typeof(decimal));
            dt.Columns.Add("MitsumoriHontaiGaku", typeof(decimal));
            dt.Columns.Add("MitsumoriTax", typeof(decimal));
            dt.Columns.Add("MitsumoriTaxRitsu", typeof(decimal));
            dt.Columns.Add("CostUnitPrice", typeof(decimal));
            dt.Columns.Add("CostGaku", typeof(decimal));
            dt.Columns.Add("ProfitGaku", typeof(decimal));
            dt.Columns.Add("CommentInStore", typeof(string));
            dt.Columns.Add("CommentOutStore", typeof(string));
            dt.Columns.Add("IndividualClientName", typeof(string));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //m_dataCntが更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false
                    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SKUName) == false
                    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ColorName) == false
                    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SizeName) == false
                    || bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriSuu) != 0
                    || bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriUnitPrice) != 0
                    || bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice) != 0
                    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].CommentOutStore) == false
                    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].IndividualClientName) == false
                    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].CommentInStore) == false
                    )
                {
                    string setkbn = "1";
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SetKBN))
                        setkbn = "0";
                    int notPrintFLG = 0;
                    if (mGrid.g_DArray[RW].NotPrintFLG)
                        notPrintFLG = 1;

                    dt.Rows.Add(rowNo
                        , rowNo
                        , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                        , mGrid.g_DArray[RW].SKUCD == "" ? null : mGrid.g_DArray[RW].SKUCD
                        , mGrid.g_DArray[RW].JanCD == "" ? null : mGrid.g_DArray[RW].JanCD
                        , mGrid.g_DArray[RW].SKUName == "" ? null : mGrid.g_DArray[RW].SKUName
                        , mGrid.g_DArray[RW].ColorName == "" ? null : mGrid.g_DArray[RW].ColorName
                        , mGrid.g_DArray[RW].SizeName == "" ? null : mGrid.g_DArray[RW].SizeName
                        , notPrintFLG
                        , setkbn
                        , bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriSuu)
                        , bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriUnitPrice)
                        , mGrid.g_DArray[RW].TaniCD == "" ? null : mGrid.g_DArray[RW].TaniCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriHontaiGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriTax)+ bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].TaxRate)     //税率
                        , bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].CostGaku)
                        , bbl.Z_Set(mGrid.g_DArray[RW].ProfitGaku)
                        , mGrid.g_DArray[RW].CommentInStore =="" ? null: mGrid.g_DArray[RW].CommentInStore
                        , mGrid.g_DArray[RW].CommentOutStore == "" ? null : mGrid.g_DArray[RW].CommentOutStore
                        , mGrid.g_DArray[RW].IndividualClientName == "" ? null : mGrid.g_DArray[RW].IndividualClientName
                        //, mGrid.g_DArray[RW].Update
                        );

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
                    //m_dataCntが更新有効行数
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false
                        || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SKUName) == false
                        || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ColorName) == false
                        || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SizeName) == false
                        || bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriSuu) != 0
                        || bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriUnitPrice) != 0
                        || bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice) != 0
                        || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].CommentOutStore) == false
                        || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].IndividualClientName) == false
                        || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].CommentInStore) == false
                        )
                    {
                        for (int CL = (int)ClsGridMitsumori.ColNO.JanCD; CL < (int)ClsGridMitsumori.ColNO.COUNT; CL++)
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
            dme = GetEntity();
            mibl.Mitsumori_Exec(dme,dt, (short)OperationMode, InOperatorCD, InPcID);

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");

            if (ChkPrint.Checked && OperationMode != EOperationMode.DELETE)
            {
                //排他処理を解除
                DeleteExclusive();

                ExecPrint(dme.MitsumoriNO);
            }

            //更新後画面クリア
            ChangeOperationMode(base.OperationMode);
        }

        private void ExecPrint(string no)
        {
            //Form.見積書印刷CheckBox＝onの場合、印刷プログラム(PCAP0260P)						
            //EXEが存在しない時ｴﾗｰ
            // 実行モジュールと同一フォルダのファイルを取得
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + Mitsumorisyo;
            if (System.IO.File.Exists(filePath))
            {
                string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " +no;
                System.Diagnostics.Process.Start(filePath, cmdLine);
            }
            else
            {
                //ファイルなし
            }
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
                    SetInitStoreInfo(StoreCD);

                    detailControls[0].Focus();
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    keyControls[0].Focus();
                    break;

            }

        }
        private void SetInitStoreInfo(string storeCD)
        {
            //[M_Store]
            M_Store_Entity mse2 = new M_Store_Entity
            {
                StoreCD = storeCD,
                ChangeDate = bbl.GetDate()
            };
            Store_BL sbl = new Store_BL();
            DataTable dt = sbl.M_Store_Select(mse2);
            if (dt.Rows.Count > 0)
            {
                detailControls[(int)EIndex.DeliveryDate].Text = dt.Rows[0]["DeliveryDate"].ToString();
                detailControls[(int)EIndex.PaymentTerms].Text = dt.Rows[0]["PaymentTerms"].ToString();
                detailControls[(int)EIndex.DeliveryPlace].Text = dt.Rows[0]["DeliveryPlace"].ToString();
                detailControls[(int)EIndex.ValidityPeriod].Text = dt.Rows[0]["ValidityPeriod"].ToString();
            }
            else
            {
                bbl.ShowMessage("E133");
                EndSec();
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
                        ctl.Text = "";
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
                    radioButton1.Checked = true;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex=-1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                {
                    //顧客情報ALLクリア
                    ClearCustomerInfo();
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

            mOldMitsumoriDate = "";
            S_Clear_Grid();   //画面クリア（明細部）

            lblKin1.Text = "";
            lblKin2.Text = "";
            lblKin3.Text = "";
            lblKin4.Text = "";
            TxtRemark1.Text = "";
            TxtRemark2.Text = "";
            ChkPrint.Checked = false;


        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            mOldCustomerCD = "";

            addInfo = new FrmAddress();

            ScCustomer.LabelText = "";
            detailControls[(int)EIndex.CustomerName].Text = "";
            detailControls[(int)EIndex.CustomerName2].Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            lblLastSalesDate.Text = "";
            lblStoreName.Text = "";
            lblTankaName.Text = "";
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
                            ScMitsumoriNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
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
                            ScCustomer.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            btnAddress.Enabled = Kbn == 0 ? true : false;

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

                    if(w_Col == (int)ClsGridMitsumori.ColNO.JanCD)
                        //商品検索
                        kbn = EsearchKbn.Product;
                    //else if (Array.IndexOf(detailControls, previousCtrl) == (int)EIndex.BrandCD)
                    //{
                    //    //汎用検索
                    //    kbn = EsearchKbn.Brand;
                    //}
                    //else if (Array.IndexOf(detailControls, previousCtrl) == (int)EIndex.TankaCD)
                    //{
                    //    //単価設定検索
                    //    kbn = EsearchKbn.Tanka;
                    //}

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
            DeleteExclusive();

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

                    using (Search_Product frmProduct = new Search_Product(detailControls[(int)EIndex.MitsumoriDate].Text))
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

                            CheckGrid((int)ClsGridMitsumori.ColNO.JanCD, w_Row, false, true);

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
                            detailControls[(int)EIndex.MitsumoriDate].Focus();
                        }
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
                       if( index == (int)EIndex.ValidityPeriod)
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridMitsumori.Gen_MK_FocusMove.MvSet, (int)ClsGridMitsumori.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridMitsumori.ColNO.JanCD);
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
                if (!string.IsNullOrWhiteSpace(url))
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

                    //どの項目か判別
                    int CL=-1;
                    string ctlName = "";
                    if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        ctlName= w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                    else
                        ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));

                    bool lastCell = false;

                    switch (ctlName)
                    {
                        case "SC_ITEM":
                            CL = (int)ClsGridMitsumori.ColNO.JanCD;
                            break;
                        case "IMT_ITMNM":
                            CL = (int)ClsGridMitsumori.ColNO.SKUName;
                            break;
                        case "IMN_CLINT":
                            CL = (int)ClsGridMitsumori.ColNO.ColorName;
                            break;
                        case "IMT_KAIDT":
                            CL = (int)ClsGridMitsumori.ColNO.SizeName;
                            break;
                        case "CHK_PRINT":
                            CL = (int)ClsGridMitsumori.ColNO.NotPrintFLG;
                            break;
                        case "IMN_GENER":
                            CL = (int)ClsGridMitsumori.ColNO.MitsumoriSuu;
                            break;
                        case "IMN_GENER2":
                            CL = (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice;
                            break;
                        case "IMN_SALEP2":
                            CL = (int)ClsGridMitsumori.ColNO.CostUnitPrice;
                            break;
                        case "IMT_REMAK":
                            CL = (int)ClsGridMitsumori.ColNO.CommentInStore;
                            break;
                        case "IMN_WEBPR":
                            CL = (int)ClsGridMitsumori.ColNO.CommentOutStore;
                            break;
                        case "IMN_WEBPR2":
                            CL = (int)ClsGridMitsumori.ColNO.IndividualClientName;
                            //if (w_Row == m_dataCnt - 1)
                            if (w_Row == mGrid.g_MK_Max_Row - 1)
                                lastCell = true;
                            break;

                    }

                    bool changeFlg = false;
                    switch (CL)
                    {
                        case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:
                            if (!mGrid.g_DArray[w_Row].MitsumoriSuu.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice: //販売単価 
                            if (!mGrid.g_DArray[w_Row].MitsumoriUnitPrice.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridMitsumori.ColNO.CostUnitPrice: //原価単価
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
                        decimal wSuu = bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriSuu);
                        string ymd = detailControls[(int)EIndex.MitsumoriDate].Text;
                        decimal wTanka;

                        switch (CL)
                        {
                            case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:
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
                                        StoreCD = CboStoreCD.SelectedValue.ToString(),
                                        SaleKbn = "0",
                                        Suryo = wSuu.ToString()
                                    };

                                    bool ret = bbl.Fnc_UnitPrice(fue);
                                    if (ret)
                                    {
                                        //数量変更時も単価再計算
                                        //販売単価=Function_単価取得.out税込単価		
                                        mGrid.g_DArray[w_Row].MitsumoriUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeikomiTanka));

                                        //原価単価=Function_単価取得.out原価単価	
                                        mGrid.g_DArray[w_Row].CostUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka));
                                    }
                                    else
                                    {
                                        //販売単価		
                                        mGrid.g_DArray[w_Row].MitsumoriUnitPrice = "0";
                                        //原価単価
                                        mGrid.g_DArray[w_Row].CostUnitPrice = "0";
                                    }

                                    //税抜販売額
                                    if (mGrid.g_DArray[w_Row].VariousFLG.Equals(1))
                                    {
                                        //（諸口の場合は入力された税込単価から税抜単価を都度計算する）
                                        //税抜販売額＝Function_消費税計算.out金額１×Form.Detail.見積数
                                        decimal zeinukiTanka = bbl.GetZeinukiKingaku(bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriUnitPrice), mGrid.g_DArray[w_Row].TaxRateFLG, ymd);

                                        mGrid.g_DArray[w_Row].MitsumoriHontaiGaku = string.Format("{0:#,##0}", zeinukiTanka * wSuu);
                                    }
                                    else if (mGrid.g_DArray[w_Row].VariousFLG.Equals(0))
                                    {
                                        //税抜販売額＝Form.見積数＝Nullの場合Function_単価取得.out税抜単価×Form.見積数
                                        mGrid.g_DArray[w_Row].MitsumoriHontaiGaku = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeinukiTanka) * wSuu);
                                    }
                                    
                                    //税込販売額←	Function_単価取得.out税込単価×	Form.Detail.見積数			
                                    mGrid.g_DArray[w_Row].MitsumoriGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriUnitPrice) * wSuu);

                                    //原価額←Function_単価取得.out原価単価×Form.Detail.見積数
                                    mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].CostUnitPrice) * wSuu);


                                    if (mGrid.g_DArray[w_Row].TaxRateFLG == 1)
                                    {
                                        //通常税額←TaxRateFLG＝1の時のFunction_単価取得.out消費税額×Form.Detail.見積数
                                        mGrid.g_DArray[w_Row].MitsumoriTax = bbl.Z_Set(fue.Zei) * wSuu;
                                        mGrid.g_DArray[w_Row].KeigenTax = 0;
                                    }
                                    else if (mGrid.g_DArray[w_Row].TaxRateFLG == 2)
                                    {
                                        mGrid.g_DArray[w_Row].MitsumoriTax = 0;
                                        //軽減税額←TaxRateFLG＝2の時のFunction_単価取得.out消費税額×Form.Detail.見積数
                                        mGrid.g_DArray[w_Row].KeigenTax = bbl.Z_Set(fue.Zei) * wSuu;
                                    }
                                    else
                                    {
                                        mGrid.g_DArray[w_Row].MitsumoriTax = 0;
                                        mGrid.g_DArray[w_Row].KeigenTax = 0;
                                    }
                                }
                                break;

                            case (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice: //販売単価 
                                 wTanka = bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriUnitPrice);
                                //paremetersin計算モード←2
                                //in基準日←Form.見積日
                                //in軽減税率FLG←⑫TaxRateFLG
                                //in金額←Form.Detail.販売単価

                                //税抜販売額←Function_消費税計算.out金額１×Form.Detail.見積数
                                mGrid.g_DArray[w_Row].MitsumoriHontaiGaku = string.Format("{0:#,##0}", bbl.GetZeinukiKingaku(wTanka, mGrid.g_DArray[w_Row].TaxRateFLG, ymd) * wSuu);

                                //税込販売額←Form.Detail.販売単価×Form.Detail.見積数
                                mGrid.g_DArray[w_Row].MitsumoriGaku = string.Format("{0:#,##0}", wTanka * wSuu);

                                if (mGrid.g_DArray[w_Row].TaxRateFLG == 1)
                                {
                                    //Function_単価取得.out消費税額×Form.Detail.見積数
                                    //通常税額←TaxRateFLG＝1の時
                                    mGrid.g_DArray[w_Row].MitsumoriTax = bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriHontaiGaku);
                                    mGrid.g_DArray[w_Row].KeigenTax = 0;
                                }
                                else if (mGrid.g_DArray[w_Row].TaxRateFLG == 2)
                                {
                                    mGrid.g_DArray[w_Row].MitsumoriTax = 0;
                                    //軽減税額←TaxRateFLG＝2の時
                                    mGrid.g_DArray[w_Row].KeigenTax = bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriHontaiGaku);
                                }
                                else
                                {
                                    mGrid.g_DArray[w_Row].MitsumoriTax = 0;
                                    mGrid.g_DArray[w_Row].KeigenTax = 0;
                                }
                                break;

                            case (int)ClsGridMitsumori.ColNO.CostUnitPrice: //原価単価
                                //原価額=Form.Detail.原価単価×	Form.Detail.見積数
                                mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].CostUnitPrice) * bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriSuu));

                                //粗利額
                                mGrid.g_DArray[w_Row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].CostGaku)); // 

                                break;
                        }
                    }

                    if (CL == -1)
                    return;

                    //チェック処理
                    if (CheckGrid(CL, w_Row) == false)
                    {
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

                //検索ボタンClick時
                //if (((Button)sender).Name == btnStoreCD.Name || ((Button)sender).Name == btnCopyStoreCD.Name)
                //{
                    //商品検索
                    kbn = EsearchKbn.Product;
                
                    setCtl = previousCtrl;
                //}

                if (kbn != EsearchKbn.Null)
                    SearchData(kbn, setCtl);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void CboSoukoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (CboSoukoType.SelectedValue.Equals("5"))
            //    ScMakerCD.Enabled = true;
            //else
            //    ScMakerCD.Enabled = false;

            //if (CboSoukoType.Enabled)
            //{
            //    if (CboSoukoType.SelectedValue.Equals("8"))
            //        TxtHikiateOrder.Enabled = false;
            //    else
            //        TxtHikiateOrder.Enabled = true;
            //}
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

        private void BtnAddress_Click(object sender, EventArgs e)
        {
            try
            {
                addInfo.ade.CustomerCD = ScCustomer.TxtCode.Text;
                addInfo.ade.CustomerName = detailControls[(int)EIndex.CustomerName].Text;
                addInfo.ade.CustomerName2 = detailControls[(int)EIndex.CustomerName2].Text;

                addInfo.ShowDialog();

                if (detailControls[(int)EIndex.CustomerName2].CanFocus)
                {
                    detailControls[(int)EIndex.CustomerName2].Focus();
                }
                else
                {
                    if (radioButton1.Checked)
                        radioButton1.Focus();
                    else
                        radioButton2.Focus();
                }
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
                if (CboStoreCD.SelectedIndex > 0)
                {
                    ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
                    SetInitStoreInfo(CboStoreCD.SelectedValue.ToString());
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








