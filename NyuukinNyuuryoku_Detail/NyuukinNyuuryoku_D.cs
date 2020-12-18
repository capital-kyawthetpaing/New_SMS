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
using Search_Nyuukinmoto;

namespace NyuukinNyuuryoku_Detail
{
    /// <summary>
    /// NyuukinNyuuryoku 入金入力（明細単位）
    /// </summary>
    internal partial class NyuukinNyuuryoku_Detail : FrmMainForm
    {
        private const string ProID = "NyuukinNyuuryoku_Detail";
        private const string ProNm = "入金入力（明細単位）";
        private const short mc_L_END = 3; // ロック用

        public enum EMode:int            
        {
            Null,
            //Seikyu,
            Uriage,
            Detail
        }
        public EMode SetDisplayMode
        {
            set
            {
                mDisplayMode = value;
                switch (value)
                {
                    //case EMode.Seikyu:
                    //    tabPage1.Text = "請求単位";
                    //    lblSalesDate.Text = "";
                    //    lblSalesNO.Text = "";
                    //    lblJuchuuNO.Text = "";
                    //    lblSkuCD.Text = "";
                    //    lblSKUName.Text = "";
                    //    lblCommentInStore.Text = "";
                    //    break;
                    case EMode.Uriage:
                        tabPage1.Text = "売上単位";
                        lblSalesDate.Text = "売上日";
                        lblSalesNO.Text = "売上番号";
                        lblJuchuuNO.Text = "受注番号";
                        lblSkuCD.Text = "SKUCD(１行目)";
                        //lblSKUName.Text = "";
                        //lblCommentInStore.Text = "";
                        break;
                    case EMode.Detail:
                        tabPage1.Text = "明細単位";
                        lblSalesDate.Text = "売上日";
                        lblSalesNO.Text = "売上番号";
                        lblJuchuuNO.Text = "受注番号";
                        lblSkuCD.Text = "SKUCD";
                        //lblSKUName.Text = "商品名";
                        //lblCommentInStore.Text = "備考";
                        break;

                }
            }
        }
        private EMode mDisplayMode = EMode.Null;

        private enum EIndex : int
        {
            CollectNO,
            ConfirmNO,
            StoreCD,

            Radio1,
            TorikomiKbn,
            InputDateFrom,
            InputDateTo,
            Radio2,
            CustomerCD,

            CollectDate = 0,
            CboNyukinKinsyu,
            CboKoza,
            Tegata,
            CollectClearDate,
            StaffCD,
            NyukinGaku,
            FeeDeduction,
            Deduction1,
            Deduction2,
            DeductionConfirm,
            Remark,
            COUNT
        }

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private NyuukinNyuuryoku_BL nnbl;
        private D_Collect_Entity dce;
        private DataTable dtDetail;
        private DataTable dtForUpdate;  //排他用

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldCollectNO = "";    //排他処理のため使用
        private string mOldConfirmNO = "";    //排他処理のため使用
        private string mSystemKBN = "";     //[M_DenominationKBN][SystemKBN]
        private string mStaffName = "";
        private short mKidouMode = 0;
        private bool mConfirmExistsFlg = false;

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridNyuukin_S mGrid = new ClsGridNyuukin_S();
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

            if (ClsGridNyuukin_S.gc_P_GYO <= ClsGridNyuukin_S.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridNyuukin_S.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridNyuukin_S.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridNyuukin_S.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridNyuukin_S.gc_MaxCL;

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
                            check.Click += new System.EventHandler(CHK_Select_Click);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridNyuukin_S.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridNyuukin_S.ColNO.GYONO; i <= (int)ClsGridNyuukin_S.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridNyuukin_S.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridNyuukin_S.ColNO.BillingGaku:
                        case (int)ClsGridNyuukin_S.ColNO.CollectAmount:
                        case (int)ClsGridNyuukin_S.ColNO.ConfirmAmount:
                        case (int)ClsGridNyuukin_S.ColNO.Minyukin:
                            mGrid.SetProp_TANKA( ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
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
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 0].CellCtl = CHK_EDICK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 0].CellCtl =  IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 0].CellCtl = IMT_CUSTM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 0].CellCtl = IMN_WEBPR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 0].CellCtl = IMT_JUONO_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 0].CellCtl = IMT_PAYDT_0;    //支払予定日

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 1].CellCtl = CHK_EDICK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 1].CellCtl = IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 1].CellCtl = IMT_CUSTM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 1].CellCtl = IMN_WEBPR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 1].CellCtl = IMT_REMAK_1;
            
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 1].CellCtl = IMT_JUONO_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 1].CellCtl = IMT_PAYDT_1;    //支払予定日

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 2].CellCtl = CHK_EDICK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 2].CellCtl = IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 2].CellCtl = IMT_CUSTM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 2].CellCtl = IMN_WEBPR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 2].CellCtl = IMT_JUONO_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 2].CellCtl = IMT_PAYDT_2;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 3].CellCtl = CHK_EDICK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 3].CellCtl =  IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 3].CellCtl = IMT_CUSTM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 3].CellCtl = IMN_GENER2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 3].CellCtl = IMN_WEBPR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 3].CellCtl = IMT_JUONO_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 3].CellCtl = IMT_PAYDT_3;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 4].CellCtl = CHK_EDICK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 4].CellCtl =  IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 4].CellCtl = IMT_CUSTM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 4].CellCtl = IMN_GENER2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 4].CellCtl = IMN_WEBPR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 4].CellCtl = IMT_JUONO_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 4].CellCtl = IMT_PAYDT_4;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 5].CellCtl = CHK_EDICK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 5].CellCtl =  IMT_JANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 5].CellCtl = IMT_CUSTM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 5].CellCtl = IMN_TEIKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 5].CellCtl = IMN_GENER2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 5].CellCtl = IMN_MEMBR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 5].CellCtl = IMN_WEBPR2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 5].CellCtl = IMT_REMAK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 5].CellCtl = IMT_JUONO_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 5].CellCtl = IMT_PAYDT_5;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 6].CellCtl = CHK_EDICK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 6].CellCtl = IMT_JANCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 6].CellCtl = IMT_KAIDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 6].CellCtl = IMT_CUSTM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 6].CellCtl = IMN_TEIKA_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 6].CellCtl = IMN_GENER2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 6].CellCtl = IMN_MEMBR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 6].CellCtl = IMN_CLINT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 6].CellCtl = IMN_WEBPR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 6].CellCtl = IMN_WEBPR2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 6].CellCtl = IMT_REMAK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 6].CellCtl = IMT_JUONO_6;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 6].CellCtl = IMT_ARIDT_6;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 6].CellCtl = IMT_PAYDT_6;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 7].CellCtl = CHK_EDICK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 7].CellCtl = IMT_JANCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 7].CellCtl = IMT_KAIDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 7].CellCtl = IMT_CUSTM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 7].CellCtl = IMN_TEIKA_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 7].CellCtl = IMN_GENER2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 7].CellCtl = IMN_MEMBR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 7].CellCtl = IMN_CLINT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 7].CellCtl = IMN_WEBPR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 7].CellCtl = IMN_WEBPR2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 7].CellCtl = IMT_REMAK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 7].CellCtl = IMT_JUONO_7;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 7].CellCtl = IMT_ARIDT_7;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 7].CellCtl = IMT_PAYDT_7;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 8].CellCtl = CHK_EDICK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 8].CellCtl = IMT_JANCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 8].CellCtl = IMT_KAIDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 8].CellCtl = IMT_CUSTM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 8].CellCtl = IMN_TEIKA_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 8].CellCtl = IMN_GENER2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 8].CellCtl = IMN_MEMBR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 8].CellCtl = IMN_CLINT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 8].CellCtl = IMN_WEBPR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 8].CellCtl = IMN_WEBPR2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 8].CellCtl = IMT_REMAK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 8].CellCtl = IMT_JUONO_8;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 8].CellCtl = IMT_ARIDT_8;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 8].CellCtl = IMT_PAYDT_8;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Chk, 9].CellCtl = CHK_EDICK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingNo, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesDate, 9].CellCtl = IMT_JANCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingGaku, 9].CellCtl = IMT_KAIDT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerCD, 9].CellCtl = IMT_CUSTM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CustomerName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.ConfirmAmount, 9].CellCtl = IMN_TEIKA_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.JuchuuNO, 9].CellCtl = IMN_GENER2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SalesNO, 9].CellCtl = IMN_MEMBR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CollectAmount, 9].CellCtl = IMN_CLINT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUName, 9].CellCtl = IMN_WEBPR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.CommentInStore, 9].CellCtl = IMN_WEBPR2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.SKUCD, 9].CellCtl = IMT_REMAK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Store, 9].CellCtl = IMT_JUONO_9;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.Minyukin, 9].CellCtl = IMT_ARIDT_9;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNyuukin_S.ColNO.BillingDate, 9].CellCtl = IMT_PAYDT_9;    //支払予定日

        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvNxt, (int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvPrv, (int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvNxt, (int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvSet, (int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvSet, (int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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
                            case (int)ClsGridNyuukin_S.ColNO.GYONO:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }
                            case (int)ClsGridNyuukin_S.ColNO.BillingDate:
                            case (int)ClsGridNyuukin_S.ColNO.BillingNo:
                            case (int)ClsGridNyuukin_S.ColNO.SalesNO:
                            case (int)ClsGridNyuukin_S.ColNO.JuchuuNO:
                            case (int)ClsGridNyuukin_S.ColNO.Store:
                            case (int)ClsGridNyuukin_S.ColNO.SalesDate:
                            case (int)ClsGridNyuukin_S.ColNO.CustomerCD:
                            case (int)ClsGridNyuukin_S.ColNO.CustomerName:
                            case (int)ClsGridNyuukin_S.ColNO.BillingGaku:
                            case (int)ClsGridNyuukin_S.ColNO.CollectAmount:
                            case (int)ClsGridNyuukin_S.ColNO.Minyukin:
                            case (int)ClsGridNyuukin_S.ColNO.SKUCD:
                            case (int)ClsGridNyuukin_S.ColNO.SKUName:
                            case (int)ClsGridNyuukin_S.ColNO.CommentInStore:
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
                        case  (int)ClsGridNyuukin_S.ColNO.Chk:
                            {
                                // チェック
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
                                Scr_Lock(0, mc_L_END, 1);
                                Scr_Lock(2, 2, 0);
                                btnNyuukinmoto.Enabled = ckM_RadioButton2.Checked; 
                                btnSubF11.Enabled = true;

                                SetFuncKeyAll(this, "111111000011");
                            }
                            else
                            {
                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                Scr_Lock(0, 0, 0);
                                this.Vsb_Mei_0.TabStop = false;
                                btnNyuukinmoto.Enabled = false;
                                btnSubF11.Enabled = false;

                                SetFuncKeyAll(this, "111111001001");
                            }
                        }
                        break;
                    }

                case 1: //データ展開後
                    {
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;

                                // BillingNoがある場合、入力可（有効行）
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].BillingNo))
                                {
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridNyuukin_S.ColNO.Chk:    // 
                                        case (int)ClsGridNyuukin_S.ColNO.ConfirmAmount:    // 
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                break;
                                            }
                                        default:
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
                            Scr_Lock(3, 3, 0);
                            Scr_Lock(0, 2, 1);
                            keyControls[(int)EIndex.StoreCD].Enabled = false;
                            SetEnabled();
                            SetEnabledForMode();
                            btnSubF11.Enabled = false;
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
                                Scr_Lock(1, 3, 1);
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
            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            // ﾌｧﾝｸｼｮﾝﾎﾞﾀﾝ使用可否

            // 検索ﾎﾞﾀﾝ使用不可.解除
            SetFuncKey(this, 8, false);
        }        

        #endregion

        public NyuukinNyuuryoku_Detail()
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

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                nnbl = new NyuukinNyuuryoku_BL();
                CboStoreCD.Bind(ymd);
                CboTorikomi.Bind(ymd);//
                cboDenomination.Bind(ymd);//
                cboKouza.Bind(ymd);

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScCollectNO.Value1 = InOperatorCD;
                ScCollectNO.Value2 = stores;
                ScCustomer.Value1 = "3";

                //Btn_F4.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F10.Text = "";

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = nnbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                    mStaffName = mse.StaffName;
                }

                //	パラメータ	基準日：Form.日付	店舗：Form.店舗		得意先区分：3
                ScCustomer.Value2 = mse.StoreCD;
                ScCustomer.Value1 = "3";

                if (mDisplayMode == EMode.Null)
                {
                    SetDisplayMode = EMode.Uriage;
                }

                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    string mode = cmds[(int)ECmdLine.PcID + 1];   //

                    if (mode.Equals("0"))
                    {
                        mKidouMode = 1;
                    }
                    else
                    {
                        ChangeOperationMode(EOperationMode.UPDATE);
                        string collectNO = cmds[(int)ECmdLine.PcID + 2];   //

                        if (mode.Equals("9"))
                        {
                            mKidouMode = 9;
                            keyControls[(int)EIndex.CollectNO].Text = collectNO;
                            CheckKey((int)EIndex.CollectNO, true);
                        }
                        else if (mode.Equals("10"))
                        {
                            mKidouMode = 10;
                            string confirmNO = cmds[(int)ECmdLine.PcID + 3];
                            keyControls[(int)EIndex.ConfirmNO].Text = confirmNO;
                            CheckKey((int)EIndex.ConfirmNO, true);
                        }
                        SetFocus();
                    }                    

                    Btn_F2.Text = "";
                    Btn_F3.Text = "";
                    Btn_F4.Text = "";
                    Btn_F5.Text = "";
                    Btn_F6.Text = "";

                }
                //ChangeOperationMode(EOperationMode.INSERT);
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
            keyControls = new Control[] { ScCollectNO.TxtCode, ScConfirmNO.TxtCode, CboStoreCD,ckM_RadioButton1,  CboTorikomi, ckM_TextBox1 ,ckM_TextBox2
                    ,ckM_RadioButton2, ScCustomer.TxtCode };
            keyLabels = new Control[] { ScCustomer };
            detailControls = new Control[] { ckM_TextBox6 ,cboDenomination ,cboKouza,ckM_TextBox3, ckM_TextBox8, ScStaff.TxtCode
                    , ckM_TextBox4,ckM_TextBox5,ckM_TextBox9,ckM_TextBox10,ckM_TextBox7, TxtRemark1
                         };
            detailLabels = new Control[] {  ScStaff };
            searchButtons = new Control[] { ScCustomer.BtnSearch, ScStaff.BtnSearch };

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

        private bool SelectAndInsertExclusive(string No, Exclusive_BL.DataKbn dataKbn)
        {
            if (OperationMode == EOperationMode.SHOW )
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)dataKbn,
                Number = No,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(),dt.Rows[0]["Operator"].ToString());
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);

                if(dataKbn == Exclusive_BL.DataKbn.Nyukin)
                    mOldCollectNO = keyControls[(int)EIndex.CollectNO].Text;
                else if(dataKbn == Exclusive_BL.DataKbn.NyukinKeshikomk)
                    mOldConfirmNO = keyControls[(int)EIndex.ConfirmNO].Text;

                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive()
        {
            if (mOldCollectNO == "" && mOldConfirmNO == "" && dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                        D_Exclusive_Entity de = new D_Exclusive_Entity
                        {
                            DataKBN = Convert.ToInt16(dr["kbn"]),
                            Number = dr["no"].ToString()
                        };

                        ebl.D_Exclusive_Delete(de);
                }
            }

            if (mOldConfirmNO != "" )
            {
                D_Exclusive_Entity dee = new D_Exclusive_Entity
                {
                    DataKBN = (int)Exclusive_BL.DataKbn.NyukinKeshikomk,
                    Number = mOldConfirmNO 
                };

                bool ret = ebl.D_Exclusive_Delete(dee);
            }
            if (mOldCollectNO != "")
            {
                D_Exclusive_Entity dee = new D_Exclusive_Entity
                {
                    DataKBN = (int)Exclusive_BL.DataKbn.Nyukin,
                    Number = mOldCollectNO 
                };

                bool ret = ebl.D_Exclusive_Delete(dee);
            }
            mOldCollectNO = "";
            mOldConfirmNO = "";
        }

        private void SetFocusAfterErr()
        {
            Scr_Clr(1);
            previousCtrl.Focus();
        }

        /// <summary>
        /// データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <param name="kbn">0:通常時,1:入金元検索の戻り値が請求番号の場合,2:入金元検索の戻り値が請求番号でない場合</param>
        /// <returns></returns>
        private bool CheckData( bool set, int index, short kbn = 0, string no = "")
        {
            DeleteExclusive();

            if (OperationMode == EOperationMode.INSERT)
            {
                dce = GetEntity();

                dtDetail = nnbl.SelectDataForNyukin(dce);

                if (dtDetail.Rows.Count == 0)
                {
                    ////該当データなし
                    //bbl.ShowMessage("E128");
                    //SetFocusAfterErr();
                    //return false;
                }
                else
                {

                }

                //請求データの排他ロック
                //明細にデータをセット
                int gyono = 0;
                string breakKey = "";
                dtForUpdate = new DataTable();
                dtForUpdate.Columns.Add("kbn", Type.GetType("System.String"));
                dtForUpdate.Columns.Add("no", Type.GetType("System.String"));
                Exclusive_BL.DataKbn dataKbn = Exclusive_BL.DataKbn.Seikyu;

                //排他処理
                foreach (DataRow row in dtDetail.Rows)
                {
                    DataRow[] rows = dtForUpdate.Select("no = '" + row["BillingNO"].ToString() + "'");
                    if (breakKey.Equals(row["BillingNO"].ToString() ))
                    {
                        continue;
                    }
                    else if (rows.Length > 0)
                    {
                        continue;
                    }
                    else
                    {
                        breakKey = row["BillingNO"].ToString();
                    }

                    //テーブル転送仕様Ｙに従って排他テーブルに追加	
                    bool ret = SelectAndInsertExclusive(breakKey, dataKbn);
                    if (!ret)
                    {
                        SetFocusAfterErr();
                        return false;
                    }
                    gyono++;
                    // データを追加
                    DataRow rowForUpdate;
                    rowForUpdate = dtForUpdate.NewRow();
                    rowForUpdate["kbn"] = (int)dataKbn;
                    rowForUpdate["no"] = breakKey;
                    dtForUpdate.Rows.Add(rowForUpdate);
                }

                //取込種別の場合
                if (ckM_RadioButton1.Checked)
                {
                    dataKbn = Exclusive_BL.DataKbn.Kessai;
                    breakKey = "";

                    //排他処理
                    foreach (DataRow row in dtDetail.Rows)
                    {
                        DataRow[] rows = dtForUpdate.Select("no = '" + row["WebCollectNO"].ToString() + "'");
                        if (breakKey.Equals(row["WebCollectNO"].ToString()))
                        {
                            continue;
                        }
                        else if (rows.Length > 0)
                        {
                            continue;
                        }
                        else
                        {
                            breakKey = row["WebCollectNO"].ToString();
                        }

                        //テーブル転送仕様Ｙに従って排他テーブルに追加	
                        bool ret = SelectAndInsertExclusive(breakKey, dataKbn);
                        if (!ret)
                        {
                            SetFocusAfterErr();
                            return false;
                        }
                        gyono++;
                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["kbn"] = (int)dataKbn;
                        rowForUpdate["no"] = breakKey;
                        dtForUpdate.Rows.Add(rowForUpdate);
                    }
                }
            }
            else
            {
                //[D_Collect]
                dce = new D_Collect_Entity
                {
                    CollectNO = keyControls[(int)EIndex.CollectNO].Text,
                    ConfirmNO = keyControls[(int)EIndex.ConfirmNO].Text
                };

                dtDetail = nnbl.D_Collect_SelectData(dce);


                string mes = "";
                Exclusive_BL.DataKbn dataKbn = Exclusive_BL.DataKbn.Nyukin;
                if (index == (int)EIndex.CollectNO)
                {
                    mes = "入金番号";
                }
                else
                {
                    mes = "入金消込番号";
                    dataKbn = Exclusive_BL.DataKbn.NyukinKeshikomk;
                }

                //入金(D_Collect)に存在しない場合、Error 「登録されていない入金番号」
                if (dtDetail.Rows.Count == 0)
                {
                    if (OperationMode == EOperationMode.INSERT)
                    {

                    }
                    else
                    {
                        bbl.ShowMessage("E138", mes);
                    }

                    SetFocusAfterErr();
                    return false;
                }
                else
                {
                    if (OperationMode == EOperationMode.INSERT)
                    {
                        //画面転送表02に従ってデータ取得/画面表示(Display screen information according to "画面転送表02")

                    }
                    else
                    {
                        //DeleteDateTime 「削除された入金番号」
                        if (!string.IsNullOrWhiteSpace(dtDetail.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            bbl.ShowMessage("E140", mes);
                            SetFocusAfterErr();
                            return false;
                        }
                        //取得した入金データの入金日が、入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(dtDetail.Rows[0]["CollectDate"].ToString()))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            SetFocusAfterErr();
                            return false;
                        }
                        //権限がない場合（以下のSelectができない場合）Error　「権限のない入金番号」
                        if (!base.CheckAvailableStores(dtDetail.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E139", mes);
                            SetFocusAfterErr();
                            return false;
                        }
                        //修正・削除モードの場合、                        
                        if (OperationMode != EOperationMode.SHOW )
                        {
                            //この消込の入金番号で、もっと最新の消込番号が存在すれば、修正・削除不可
                            if (index == (int)EIndex.ConfirmNO)
                            {
                                dce.CollectNO = dtDetail.Rows[0]["CollectNO"].ToString();
                                bool exist = nnbl.D_PaymentConfirm_Select(dce);
                                if (exist)
                                {
                                    bbl.ShowMessage("E271");
                                    SetFocusAfterErr();
                                    return false;
                                }
                            }
                            //単独起動で（＝入金照会からの起動でなくメニューからの直接起動）「変更」「削除」モードの場合
                            else if (index == (int)EIndex.CollectNO && mKidouMode.Equals(0))
                            {
                                //この入金番号で消込が一度もない場合（下記のSelectでレコードが無い場合）
                                mConfirmExistsFlg = bbl.Z_Set(dtDetail.Rows[0]["ConfirmCount"]) > 0 ? true : false;

                                if (OperationMode == EOperationMode.DELETE && mConfirmExistsFlg)
                                {
                                    bbl.ShowMessage("E272");
                                    SetFocusAfterErr();
                                    return false;
                                }
                            }
                        }

                        //入金番号の場合
                        //Errorでない場合、画面転送表01に従ってデータ取得/画面表示(Display screen information according to "画面転送表01")

                        //入金消込番号の場合
                        //Errorでない場合、画面転送表02に従ってデータ取得/画面表示(Display screen information according to "画面転送表02")
                        //取込種別
                        if (index == (int)EIndex.ConfirmNO && string.IsNullOrWhiteSpace(dtDetail.Rows[0]["CollectCustomerCD"].ToString()))
                        {
                            bbl.ShowMessage("E138", mes);
                            SetFocusAfterErr();
                            return false;
                        }
                        
                        //テーブル転送仕様Ｙに従って排他テーブルに追加	
                        bool ret = SelectAndInsertExclusive(keyControls[index].Text, dataKbn);
                        if (!ret)
                        {
                            SetFocusAfterErr();
                            return false;
                        }
                    }
                }
            }

            //画面セットなしの場合、処理正常終了
            if (set == false)
            {
                return true;
            }

            Scr_Clr(1);   //画面クリア（明細部）

            if (OperationMode == EOperationMode.INSERT)
            {
                if (dtDetail.Rows.Count > 0)
                {
                    //取込種別
                    if (ckM_RadioButton1.Checked)
                    {
                        detailControls[(int)EIndex.NyukinGaku].Text = bbl.Z_SetStr(dtDetail.Rows[0]["ImportAmount"]);
                        lblKin1.Text = bbl.Z_SetStr(dtDetail.Rows[0]["ImportAmount"]);
                        ckM_RadioButton1.Tag = dtDetail.Rows[0]["WebCollectNO"];
                    }
                    //入金顧客
                    else
                    {
                        if (kbn >= 1)
                        {
                            CboStoreCD.SelectedValue = dtDetail.Rows[0]["StoreCD"].ToString();
                            //明細の今回入金額の合計をセットする
                            //明細にデータセット後
                            detailControls[(int)EIndex.NyukinGaku].Text = "0";
                        }
                        else
                        {
                            detailControls[(int)EIndex.NyukinGaku].Text = "0";
                            lblKin1.Text = "0";
                        }
                        //lblKin1.Text = bbl.Z_SetStr(dtDetail.Rows[0]["SumConfirmAmount"]);
                    }
                    detailControls[(int)EIndex.FeeDeduction].Text = "0";
                    detailControls[(int)EIndex.Deduction1].Text = "0";
                    detailControls[(int)EIndex.Deduction2].Text = "0";
                    detailControls[(int)EIndex.DeductionConfirm].Text = "0";
                    cboKouza.Bind(bbl.GetDate());
                }
                else
                {
                    string ymd = bbl.GetDate();
                    detailControls[(int)EIndex.CollectDate].Text = ymd;
                    detailControls[(int)EIndex.CollectClearDate].Text = ymd;
                    detailControls[(int)EIndex.StaffCD].Text = InOperatorCD;
                    ScStaff.LabelText = mStaffName;
                }
            }
            else
            {
                keyControls[(int)EIndex.CollectNO].Text = dtDetail.Rows[0]["CollectNO"].ToString();
                keyControls[(int)EIndex.ConfirmNO].Text = dtDetail.Rows[0]["ConfirmNO"].ToString();

                //Errorでない場合、画面転送表01に従ってデータ取得/画面表示
                //取込種別
                if (string.IsNullOrWhiteSpace(dtDetail.Rows[0]["CollectCustomerCD"].ToString()))
                {
                    ckM_RadioButton1.Checked = true;
                    CboTorikomi.SelectedValue = dtDetail.Rows[0]["WebCollectType"];
                    ckM_RadioButton1.Tag = dtDetail.Rows[0]["WebCollectNO"];
                }
                //入金顧客
                else
                {
                    ckM_RadioButton2.Checked = true;
                    keyControls[(int)EIndex.CustomerCD].Text = dtDetail.Rows[0]["CollectCustomerCD"].ToString();
                    ScCustomer.LabelText = dtDetail.Rows[0]["CustomerName"].ToString();
                }

                CboStoreCD.SelectedValue = dtDetail.Rows[0]["StoreCD"].ToString();
                detailControls[(int)EIndex.CollectDate].Text = dtDetail.Rows[0]["CollectDate"].ToString();
                cboDenomination.SelectedValue = dtDetail.Rows[0]["PaymentMethodCD"].ToString();

                cboKouza.DataSource = null;
                cboKouza.Bind(dtDetail.Rows[0]["CollectDate"].ToString());
                cboKouza.SelectedValue = dtDetail.Rows[0]["KouzaCD"].ToString();
                detailControls[(int)EIndex.Tegata].Text = dtDetail.Rows[0]["BillDate"].ToString();
                detailControls[(int)EIndex.StaffCD].Text = dtDetail.Rows[0]["StaffCD"].ToString();
                ScStaff.LabelText = dtDetail.Rows[0]["StaffName"].ToString();
                detailControls[(int)EIndex.NyukinGaku].Text = bbl.Z_SetStr(dtDetail.Rows[0]["CollectAmount"]);
                detailControls[(int)EIndex.FeeDeduction].Text = bbl.Z_SetStr(dtDetail.Rows[0]["FeeDeduction"]);
                detailControls[(int)EIndex.Deduction1].Text = bbl.Z_SetStr(dtDetail.Rows[0]["Deduction1"]);
                detailControls[(int)EIndex.Deduction2].Text = bbl.Z_SetStr(dtDetail.Rows[0]["Deduction2"]);
                detailControls[(int)EIndex.DeductionConfirm].Text = bbl.Z_SetStr(dtDetail.Rows[0]["DeductionConfirm"]);
                detailControls[(int)EIndex.Remark].Text = dtDetail.Rows[0]["Remark"].ToString();
                lblKin1.Text = bbl.Z_SetStr(dtDetail.Rows[0]["ConfirmSource"]);
                if (index == (int)EIndex.CollectNO)
                {
                    lblKin2.Text = bbl.Z_SetStr(dtDetail.Rows[0]["ConfirmAmount"]);
                }
                else
                {
                    //入金消込番号　入力時
                    lblKin2.Text = bbl.Z_SetStr(dtDetail.Rows[0]["ConfirmAmount"]);
                    detailControls[(int)EIndex.CollectClearDate].Text = dtDetail.Rows[0]["CollectClearDate"].ToString();
                }
                //lblKin3.Text = bbl.Z_SetStr(dtDetail.Rows[0]["ConfirmZan"]);

                //新規消込モード
                if (mKidouMode.Equals(9))
                {
                    detailControls[(int)EIndex.CollectClearDate].Text = bbl.GetDate(); //dtDetail.Rows[0]["CollectClearDate"].ToString();

                    //その時点の入金データの最新状況を表示する+画面転送表05①に従ってデータ取得/画面表示する
                    dce = GetEntity();
                    dtDetail = nnbl.SelectDataForNyukin(dce);

                    return DispFromDataTable(index, kbn, no);
                }
            }

            //入金顧客
            if (index == (int)EIndex.CollectNO && OperationMode != EOperationMode.INSERT && ckM_RadioButton2.Checked)
            {
                AfterDisp();

                //明細セットなし
                return true;
            }

            return DispFromDataTable(index, kbn, no);
        }

        /// <summary>
        /// 明細にデータをセット
        /// </summary>
        /// <param name="index"></param>
        /// <param name="kbn">0:通常時,1:入金元検索の戻り値が請求番号の場合,2:入金元検索の戻り値が請求番号でない場合</param>
        /// <param name="no"></param>
        /// <returns></returns>
        private bool DispFromDataTable(int index, short kbn = 0, string no = "")
        {
            int i = -1;
            m_dataCnt = 0;
            string breakKey = "";

            S_Clear_Grid();   //画面クリア（明細部）

            if (dtDetail is null)
                return true; 

            foreach (DataRow row in dtDetail.Rows)
            {
                //使用可能行数を超えた場合エラー
                if (i + 1 > m_EnableCnt - 1)
                {
                    bbl.ShowMessage("E178", m_EnableCnt.ToString());
                    mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                    return false;
                }

                bool isBreak = false;

                switch (mDisplayMode)
                {
                    //case EMode.Seikyu:
                    //    //CollectNO, BillingCloseDate, BillingNO
                    //    if (breakKey != row["CollectNO"].ToString() + row["BillingCloseDate"].ToString() + row["BillingNO"].ToString())
                    //    {
                    //        isBreak = true;
                    //        breakKey = row["CollectNO"].ToString() + row["BillingCloseDate"].ToString() + row["BillingNO"].ToString();
                    //    }
                    //    break;

                    case EMode.Uriage:
                        //CollectNO, BillingCloseDate, BillingNO, SalesNO
                        if (breakKey != row["BillingCloseDate"].ToString() + row["BillingNO"].ToString()
                            + row["SalesNO"].ToString())
                        {
                            isBreak = true;
                            breakKey = row["BillingCloseDate"].ToString() + row["BillingNO"].ToString()
                                + row["SalesNO"].ToString();
                        }
                        break;

                    case EMode.Detail:
                        isBreak = true;
                        break;
                }

                if (isBreak)
                {
                    i++;
                    m_dataCnt = i + 1;

                    if (kbn.Equals(1))
                    {
                        if (i == 0)
                        {
                            detailControls[(int)EIndex.NyukinGaku].Text = "0";
                        }

                        //D_Billing.BillingNO=戻り値③.請求番号の場合、
                        if (row["BillingNo"].ToString().Equals(no))
                        {
                            mGrid.g_DArray[i].Chk = true;
                            mGrid.g_DArray[i].ConfirmAmount = bbl.Z_SetStr(row["ConfirmAmount"]);   //今回入金額

                            if (OperationMode == EOperationMode.INSERT)
                                mGrid.g_DArray[i].OldConfirmAmount = "0";
                            else
                                mGrid.g_DArray[i].OldConfirmAmount = bbl.Z_SetStr(row["OldConfirmAmount"]);   //修正前今回入金額

                            detailControls[(int)EIndex.NyukinGaku].Text = bbl.Z_SetStr(bbl.Z_Set(detailControls[(int)EIndex.NyukinGaku].Text) + bbl.Z_Set(row["ConfirmAmount"]));                            
                            lblKin1.Text = detailControls[(int)EIndex.NyukinGaku].Text;
                        }
                        else
                        {
                            mGrid.g_DArray[i].Chk = false;
                            mGrid.g_DArray[i].ConfirmAmount = "0";   //今回入金額
                        }
                    }
                    else if (kbn.Equals(2))
                    {
                        if (i == 0)
                        {
                            detailControls[(int)EIndex.NyukinGaku].Text = "0";
                        }

                        //D_BillingDetails.SalesNO=戻り値③.伝票番号の場合、
                        if (row["SalesNO"].ToString().Equals(no))
                        {
                            mGrid.g_DArray[i].Chk = true;
                            mGrid.g_DArray[i].ConfirmAmount = bbl.Z_SetStr(row["ConfirmAmount"]);   //今回入金額

                            if (OperationMode == EOperationMode.INSERT)
                                mGrid.g_DArray[i].OldConfirmAmount = "0";
                            else
                                mGrid.g_DArray[i].OldConfirmAmount = bbl.Z_SetStr(row["OldConfirmAmount"]);   //修正前今回入金額

                            detailControls[(int)EIndex.NyukinGaku].Text = bbl.Z_SetStr(bbl.Z_Set(detailControls[(int)EIndex.NyukinGaku].Text) + bbl.Z_Set(row["ConfirmAmount"]));
                            lblKin1.Text = detailControls[(int)EIndex.NyukinGaku].Text;
                        }
                        else
                        {
                            //入金消込番号　入力時
                            if (index == (int)EIndex.ConfirmNO)
                            {
                                mGrid.g_DArray[i].Chk = true;
                            }
                            else
                            {
                                mGrid.g_DArray[i].Chk = false;
                            }
                            mGrid.g_DArray[i].ConfirmAmount = "0";   //今回入金額
                        }
                    }
                    else
                    {
                        mGrid.g_DArray[i].Chk = true;
                        mGrid.g_DArray[i].ConfirmAmount = bbl.Z_SetStr(row["NowCollectAmount"]);   //今回入金額

                        if (OperationMode == EOperationMode.INSERT || mKidouMode.Equals(9))
                            mGrid.g_DArray[i].OldConfirmAmount = "0";
                        else
                            mGrid.g_DArray[i].OldConfirmAmount = bbl.Z_SetStr(row["OldConfirmAmount"]);   //修正前今回入金額
                    }
                    mGrid.g_DArray[i].BillingDate = row["BillingCloseDate"].ToString();
                    mGrid.g_DArray[i].BillingNo = row["BillingNo"].ToString();
                    mGrid.g_DArray[i].Store = row["DetailStoreName"].ToString();   // 
                    mGrid.g_DArray[i].CustomerCD = row["BillingCustomerCD"].ToString();   //
                    mGrid.g_DArray[i].CustomerName = row["BillingCustomerName"].ToString();   //

                    mGrid.g_DArray[i].BillingGaku = bbl.Z_SetStr(row["BillingGaku"]);   // 

                    mGrid.g_DArray[i].CollectAmount = bbl.Z_SetStr(row["D_CollectAmount"]);   // 
                    mGrid.g_DArray[i].Minyukin = bbl.Z_SetStr(row["Minyukin"]);

                    switch (mDisplayMode)
                    {
                        //case EMode.Seikyu:
                        //    break;

                        case EMode.Uriage:
                        case EMode.Detail:
                            mGrid.g_DArray[i].SalesNO = row["SalesNO"].ToString();
                            mGrid.g_DArray[i].JuchuuNO = row["JuchuuNO"].ToString();
                            mGrid.g_DArray[i].SalesDate = row["SalesDate"].ToString();

                            mGrid.g_DArray[i].CommentInStore = row["CommentInStore"].ToString();
                            mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();
                            mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();   // 
                            break;
                    }

                    //隠し項目
                    mGrid.g_DArray[i].CollectPlanNO = row["CollectPlanNO"].ToString();
                    mGrid.g_DArray[i].CollectPlanRows = row["CollectPlanRows"].ToString();

                    //取込種別
                    if (ckM_RadioButton1.Checked)
                    {
                        mGrid.g_DArray[i].WebCollectNO = row["WebCollectNO"].ToString();
                        mGrid.g_DArray[i].WebCollectType = row["WebCollectType"].ToString();
                        if (OperationMode != EOperationMode.INSERT)
                        {
                            keyControls[(int)EIndex.ConfirmNO].Text = row["ConfirmNO"].ToString();
                            mGrid.g_DArray[i].ConfirmAmount = bbl.Z_SetStr(row["NowCollectAmount"]);   //今回入金額
                        }
                    }

                    mGrid.g_DArray[i].Update = 0;

                }
                else
                {
                    //金額を足しこむ
                    mGrid.g_DArray[i].BillingGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[i].BillingGaku) + bbl.Z_Set(row["BillingGaku"]));   // 
                    mGrid.g_DArray[i].ConfirmAmount = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[i].ConfirmAmount) + bbl.Z_Set(row["NowCollectAmount"]));
                    mGrid.g_DArray[i].CollectAmount = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[i].CollectAmount) + bbl.Z_Set(row["D_CollectAmount"]));   // 
                    mGrid.g_DArray[i].Minyukin = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[i].Minyukin) + bbl.Z_Set(row["Minyukin"]));

                    if (kbn >= 1)
                    {
                        if (mGrid.g_DArray[i].Chk)
                        {
                            detailControls[(int)EIndex.NyukinGaku].Text = bbl.Z_SetStr(bbl.Z_Set(detailControls[(int)EIndex.NyukinGaku].Text) + bbl.Z_Set(row["ConfirmAmount"]));
                            lblKin1.Text = detailControls[(int)EIndex.NyukinGaku].Text;
                        }
                        else
                        {
                            mGrid.g_DArray[i].ConfirmAmount = "0";   //今回入金額
                        }
                    }
                }
            }

            btnUriage.Enabled = true;
            btnDetail.Enabled = true;

            AfterDisp();

            return true;
        }
        private void AfterDisp()
        {
            //金額計算
            CalcKin();

            mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.DELETE)
            {
                S_BodySeigyo(2, 0);
                S_BodySeigyo(2, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            }
            else
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            }
        }
        protected override void ExecDisp()
        {
            for (int i = 0; i <= (int)EIndex.CustomerCD; i++)
                if (CheckKey(i) == false)
                {
                    keyControls[i].Focus();
                    return;
                }

            bool ret = CheckData(true, -1);
            if(ret)
            {
                detailControls[(int)EIndex.CollectDate].Focus();
            }
        }

        /// <summary>
        /// Key部チェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        private bool CheckKey(int index, bool set = true)
        {
            switch (index)
            {
                case (int)EIndex.CollectNO:
                case (int)EIndex.ConfirmNO:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return true;

                    bool ret = CheckData(set, index);
                    if (!ret)
                        return false;
                    break;

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

                        ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
                    }

                    break;

                case (int)EIndex.TorikomiKbn:
                //対象：取込種別の場合、入力必須(Entry required)
                if(ckM_RadioButton1.Checked)
                    {
                        //選択必須(Entry required)
                        if (!RequireCheck(new Control[] { keyControls[index] }))
                        {
                            CboTorikomi.MoveNext = false;
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.InputDateFrom:
                case (int)EIndex.InputDateTo:
                    //対象：取込種別の場合、入力必須(Entry required)
                    if (ckM_RadioButton1.Checked)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { keyControls[index] }))
                        {
                            return false;
                        }

                        string strYmd = "";
                        strYmd = bbl.FormatDate(keyControls[index].Text);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(strYmd))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }

                        keyControls[index].Text = strYmd;

                        //(From) ≧ (To)である場合Error
                        if (index == (int)EIndex.InputDateTo)
                        {
                            if (!string.IsNullOrWhiteSpace(keyControls[index - 1].Text) && !string.IsNullOrWhiteSpace(keyControls[index].Text))
                            {
                                int result = keyControls[index].Text.CompareTo(keyControls[index - 1].Text);
                                if (result < 0)
                                {
                                    bbl.ShowMessage("E104");
                                    return false;
                                }
                            }
                        }
                    }
                    break;

                case (int)EIndex.CustomerCD:
                    //対象：入金顧客の場合、入力必須(Entry required)
                    if (ckM_RadioButton2.Checked)
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { keyControls[index] }))
                        {        
                            //情報ALLクリア
                            ClearCustomerInfo();
                            return false;
                        }
                        //[M_Customer_Select]
                        M_Customer_Entity mve = new M_Customer_Entity
                        {
                            CustomerCD = keyControls[index].Text,
                            ChangeDate = bbl.GetDate()
                        };
                        Customer_BL sbl = new Customer_BL();
                        ret = sbl.M_Customer_Select(mve);

                        if (ret)
                        {
                            if (mve.DeleteFlg == "1")
                            {
                                bbl.ShowMessage("E119");
                                //顧客情報ALLクリア
                                ClearCustomerInfo();
                                return false;
                            }
                            ScCustomer.LabelText = mve.CustomerName;
                        }
                        else
                        {
                            bbl.ShowMessage("E101");
                            //顧客情報ALLクリア
                            ClearCustomerInfo();
                            return false;
                        }
                    }
                    break;
            }
            return true;

        }

        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set = true)
        {
            string strYmd = "";
            bool ret;

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.CollectDate:
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    strYmd = bbl.FormatDate(detailControls[index].Text);
                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    detailControls[index].Text = strYmd;

                    //入力できる範囲内の日付であること
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }

                    //取込種別=入金顧客
                    if (ckM_RadioButton2.Checked)
                    {
                        //締処理済の場合（以下のSelectができる場合）Error
                        //その店舗、請求先でその入金日を含む期間ですでに請求処理が済んでいる
                        //【D_BillingProcessing】
                        D_BillingProcessing_Entity dbe = new D_BillingProcessing_Entity
                        {
                            StoreCD = CboStoreCD.SelectedValue.ToString(),
                            BillingDate = detailControls[index].Text
                        };
                        ret = nnbl.CheckBillingDate(dbe);
                        if (ret)
                        {
                            //Ｅ１７７
                            bbl.ShowMessage("E177");
                            return false;
                        }
                    }
                    //店舗の締日チェック
                    //店舗締マスターで判断
                    M_StoreClose_Entity mse = new M_StoreClose_Entity
                    {
                        StoreCD = CboStoreCD.SelectedValue.ToString(),
                        FiscalYYYYMM = detailControls[index].Text.Replace("/", "").Substring(0, 6)
                    };
                    ret = bbl.CheckStoreClose(mse, false, false, true, false, false);
                    if (!ret)
                    {
                        return false;
                    }

                    ScCustomer.ChangeDate = strYmd;
                    break;

                case (int)EIndex.CboNyukinKinsyu:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        cboDenomination.MoveNext = false;
                        return false;
                    }
                    SetEnabled();
                    break;

                case (int)EIndex.Tegata:
                    //入金金種=小切手、手形（M_DenominationKBN.SystemKBN=6、11）の場合、入力必須(Entry required)
                    if (mSystemKBN.Equals("6") || mSystemKBN.Equals("11"))
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            return false;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    strYmd = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    detailControls[index].Text = strYmd;
                    break;

                case (int)EIndex.CollectClearDate:
                    if (ckM_RadioButton2.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                            return true;

                        strYmd = bbl.FormatDate(detailControls[index].Text);
                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(strYmd))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }
                        detailControls[index].Text = strYmd;

                        //入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            return false;
                        }
                        //締処理済の場合（以下のSelectができる場合）Error
                        //その店舗、請求先でその入金日を含む期間ですでに請求処理が済んでいる
                        //【D_BillingProcessing】
                        D_BillingProcessing_Entity dbe2 = new D_BillingProcessing_Entity
                        {
                            StoreCD = CboStoreCD.SelectedValue.ToString(),
                            BillingDate = detailControls[index].Text
                        };
                        ret = nnbl.CheckBillingDate(dbe2);
                        if (ret)
                        {
                            //Ｅ１７７
                            bbl.ShowMessage("E177");
                            return false;
                        }

                        //店舗の締日チェック
                        //店舗締マスターで判断
                        M_StoreClose_Entity msce = new M_StoreClose_Entity
                        {
                            StoreCD = CboStoreCD.SelectedValue.ToString(),
                            FiscalYYYYMM = detailControls[index].Text.Replace("/", "").Substring(0, 6)
                        };
                        ret = bbl.CheckStoreClose(msce, false, false, true, false, false);
                        if (!ret)
                        {
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.CboKoza:
                    //入金金種=振込（M_DenominationKBN.SystemKBN=5）の場合、入力必須(Entry required)
                    if (mSystemKBN.Equals("5"))
                    {
                        //入力必須(Entry required)
                        if (!RequireCheck(new Control[] { detailControls[index] }))
                        {
                            cboKouza.MoveNext = false;
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.StaffCD:
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }

                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mste = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate() 
                    };
                    Staff_BL sbl = new Staff_BL();
                     ret = sbl.M_Staff_Select(mste);
                    if (ret)
                    {
                        ScStaff.LabelText = mste.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScStaff.LabelText = "";
                        return false;
                    }
                    break;

                case (int)EIndex.NyukinGaku:
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    //検索画面あり(With search screen)（入金元検索）

                    //入力値を消込原資額に加算(消込原資額＝入金額＋手数料＋その他額(＋)－その他額(－))
                    lblKin1.Text = bbl.Z_SetStr(bbl.Z_Set(detailControls[index].Text) + bbl.Z_Set(detailControls[(int)EIndex.FeeDeduction].Text) 
                            + bbl.Z_Set(detailControls[(int)EIndex.Deduction1].Text) - bbl.Z_Set(detailControls[(int)EIndex.Deduction2].Text) 
                            //- bbl.Z_Set(detailControls[(int)EIndex.DeductionConfirm].Text)
                            );
                    if (set)
                    {
                        ret = CalcKin();

                        if (!ret)
                        {
                            detailControls[index].Focus();
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.FeeDeduction:
                case (int)EIndex.Deduction1:
                case (int)EIndex.Deduction2:
                case (int)EIndex.DeductionConfirm:
                    //入力値を消込原資額に加算(消込原資額＝入金額＋手数料＋その他額(＋)－その他額(－)－その他消込)
                    lblKin1.Text = bbl.Z_SetStr(bbl.Z_Set(detailControls[(int)EIndex.NyukinGaku].Text) + bbl.Z_Set(detailControls[(int)EIndex.FeeDeduction].Text)
                            + bbl.Z_Set(detailControls[(int)EIndex.Deduction1].Text) - bbl.Z_Set(detailControls[(int)EIndex.Deduction2].Text) 
                            //- bbl.Z_Set(detailControls[(int)EIndex.DeductionConfirm].Text)
                            );
                    if (set)
                    {
                        ret = CalcKin();

                        if (!ret)
                        {
                            detailControls[index].Focus();
                            return false;
                        }
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

            w_Ctrl = detailControls[(int)EIndex.COUNT-1];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvSet, (int)ClsGridNyuukin_S.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }
       
        private bool CheckGrid(int col, int row, bool chkAll=false, bool changeYmd=false)
        {
            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridNyuukin_S.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                        return false;
                }
            }

            switch (col)
            {
                case (int)ClsGridNyuukin_S.ColNO.ConfirmAmount: //今回入金額
                    if (mGrid.g_DArray[row].Chk)
                    {
                        //チェックボックスONの場合、入力必須(Entry required)
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ConfirmAmount))
                        {
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    //今回入金額＞未入金額(明細の請求額－入金済額)の場合、Error
                    //if (bbl.Z_Set(mGrid.g_DArray[row].ConfirmAmount) > bbl.Z_Set(mGrid.g_DArray[row].BillingGaku)- bbl.Z_Set(mGrid.g_DArray[row].CollectAmount)) //今回入金額＞未入金額の場合、Error
                    //今回入金額＞未入金額の場合、Error
                    if (bbl.Z_Set(mGrid.g_DArray[row].ConfirmAmount) > bbl.Z_Set(mGrid.g_DArray[row].Minyukin))
                    {
                        //Ｅ１４３
                        bbl.ShowMessage("E143", "未入金額", "大きい");
                        return false;
                    }
                    //入力後、以下の各項目に計算後の値をセット
                    if(!chkAll)
                    { 
                       bool ret = CalcKin();

                        if (!ret)
                        {
                            return false;
                        }
                    }
                    break;
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Collect_Entity GetEntity()
        {
            dce = new D_Collect_Entity
            {
                StoreCD = CboStoreCD.SelectedValue.ToString(),
                InputDateFrom = keyControls[(int)EIndex.InputDateFrom].Text,
                InputDateTo = keyControls[(int)EIndex.InputDateTo].Text,
                CollectCustomerCD = keyControls[(int)EIndex.CustomerCD].Text,
            };

            if (ckM_RadioButton1.Checked)
            {
                dce.RdoSyubetsu = 1;
                dce.WebCollectType = CboTorikomi.SelectedValue.ToString();
            }
            else
            {
                dce.RdoSyubetsu = 2;
            }

            return dce;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("GyoNO", typeof(int));
            dt.Columns.Add("ConfirmNO", typeof(string));
            dt.Columns.Add("WebCollectNO", typeof(string));
            dt.Columns.Add("WebCollectType", typeof(string));
            dt.Columns.Add("CollectPlanNO", typeof(int));
            dt.Columns.Add("CollectPlanRows", typeof(int));
            
            dt.Columns.Add("ConfirmAmount", typeof(decimal));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            dce = new D_Collect_Entity
            {
                StoreCD = CboStoreCD.SelectedValue.ToString(),
                CollectNO = keyControls[(int)EIndex.CollectNO].Text,
                ConfirmNO = keyControls[(int)EIndex.ConfirmNO].Text,
                CollectCustomerCD = keyControls[(int)EIndex.CustomerCD].Text,

                CollectDate = detailControls[(int)EIndex.CollectDate].Text,
                CollectAmount = bbl.Z_SetStr(detailControls[(int)EIndex.NyukinGaku].Text),
                ConfirmAmount = bbl.Z_SetStr(lblKin2.Text),
                ConfirmSource = bbl.Z_SetStr(lblKin1.Text),
                PaymentMethodCD = cboDenomination.SelectedValue.ToString(),
                KouzaCD = cboKouza.SelectedValue.ToString(),
                BillDate = detailControls[(int)EIndex.Tegata].Text,
                CollectClearDate = detailControls[(int)EIndex.CollectClearDate].Text,

                FeeDeduction = detailControls[(int)EIndex.FeeDeduction].Text,
                Deduction1 = detailControls[(int)EIndex.Deduction1].Text,
                Deduction2 = detailControls[(int)EIndex.Deduction2].Text,
                DeductionConfirm = detailControls[(int)EIndex.DeductionConfirm].Text,
                Remark = detailControls[(int)EIndex.Remark].Text,
                StaffCD = detailControls[(int)EIndex.StaffCD].Text,
                KidouMode = mKidouMode,

                Operator = InOperatorCD,
                PC = InPcID
            };
            if (ckM_RadioButton1.Checked)
            {
                dce.WebCollectType = CboTorikomi.SelectedValue.ToString();
                dce.WebCollectNO = ckM_RadioButton1.Tag.ToString();
            }

            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;
            int seq = 0;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (mGrid.g_DArray[RW].Chk)
                {
                    if (mDisplayMode == EMode.Detail)
                    {
                        dt.Rows.Add(rowNo
                                    , mGrid.g_DArray[RW].BillingNo  //未使用
                                    , mGrid.g_DArray[RW].WebCollectNO == "" ? null : mGrid.g_DArray[RW].WebCollectNO
                                    , mGrid.g_DArray[RW].WebCollectType == "" ? null : mGrid.g_DArray[RW].WebCollectType
                                    , bbl.Z_Set(mGrid.g_DArray[RW].CollectPlanNO)
                                    , bbl.Z_Set(mGrid.g_DArray[RW].CollectPlanRows)

                                    , bbl.Z_Set(mGrid.g_DArray[RW].ConfirmAmount)
                                    , mGrid.g_DArray[RW].Update
                                    );

                        rowNo++;
                        seq++;
                    }
                    else if (mDisplayMode == EMode.Uriage)
                    {
                        //該当するデータを抽出（子画面の行番号順）1行目は明細画面よりセットする
                        DataRow[] selectedRows = dtDetail.Select("SalesNO ='" + mGrid.g_DArray[RW].SalesNO + "'", "CollectPlanRows");
                        decimal SumConfirmAmount = bbl.Z_Set(mGrid.g_DArray[RW].ConfirmAmount);

                        foreach (DataRow row in selectedRows)
                        {
                            //ConfirmAmountの算出
                            decimal ConfirmAmount = 0;
                            if (bbl.Z_Set(row["Minyukin"]) <= SumConfirmAmount)
                                ConfirmAmount = bbl.Z_Set(row["Minyukin"]);
                            else
                                ConfirmAmount = SumConfirmAmount;

                            SumConfirmAmount = SumConfirmAmount - ConfirmAmount;

                            //if (bbl.Z_Set(mGrid.g_DArray[RW].CollectPlanNO) == bbl.Z_Set(row["CollectPlanNO"]) &&
                            //    bbl.Z_Set(mGrid.g_DArray[RW].CollectPlanRows) == bbl.Z_Set(row["CollectPlanRows"]))
                            //    continue;

                            string WebCollectNO = "";
                            string WebCollectType = "";
                            if(ckM_RadioButton1.Checked)
                            {
                                WebCollectNO = row["WebCollectNO"].ToString();
                                WebCollectType = row["WebCollectType"].ToString();
                            }

                            dt.Rows.Add(rowNo
                                       , row["BillingNo"].ToString() == "" ? null : row["BillingNo"].ToString()
                                       , WebCollectNO == "" ? null : WebCollectNO
                                       , WebCollectType == "" ? null : WebCollectType
                                       , bbl.Z_Set(row["CollectPlanNO"])
                                       , bbl.Z_Set(row["CollectPlanRows"])
                                       , ConfirmAmount
                                       , 0
                                       );

                            rowNo++;
                            seq++;

                            if (SumConfirmAmount.Equals(0))
                                break;
                        }
                    }
                }
            }

            return dt;
        }
    
        protected override void ExecSec()
        {
            //照会モードでは使用不可
            if (OperationMode == EOperationMode.SHOW)
                return;

            for (int i = 0; i < (int)EIndex.COUNT; i++)
                if (CheckDetail(i,false) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            int count = 0;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (mGrid.g_DArray[RW].Chk)
                {
                    count++;
                    break;
                }
            }

            //画面明細.チェックボックスONの明細が１つでも存在するとき、入力必須
            if (count > 0)
            {
                //入力必須(Entry required)
                if (!RequireCheck(new Control[] { detailControls[(int)EIndex.CollectClearDate] }))
                {
                    return;
                }
            }

            if (OperationMode == EOperationMode.INSERT || !string.IsNullOrWhiteSpace(keyControls[(int)EIndex.ConfirmNO].Text))
            {
                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (mGrid.g_DArray[RW].Chk)
                    {
                        for (int CL = (int)ClsGridNyuukin_S.ColNO.ConfirmAmount; CL < (int)ClsGridNyuukin_S.ColNO.COUNT; CL++)
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
                //ChkAll時は金額計算しないため最後に1回
                CalcKin();

                //ヘッダ.消込原資額≠SUM(明細.今回入金額)の場合、エラー
                if (bbl.Z_Set(lblKin1.Text) < bbl.Z_Set(lblSumKin3.Text))
                {
                    // Ｅ１９６
                    if (bbl.ShowMessage("E196", "明細今回入金額の合計", "消込原資額－その他消込額以下") != DialogResult.Yes)
                    {
                        //明細の先頭項目へ
                        mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridNyuukin_S.ColNO.ConfirmAmount);
                        return;
                    }
                }
            }
            else
            {
                CalcKin();
            }

            //残額＜０になれば、エラー
            if (bbl.Z_Set(lblKin3.Text) < 0)
            {
                bbl.ShowMessage("E273", "入金額（消込可能額）");
                detailControls[(int)EIndex.NyukinGaku].Focus();
                return;
            }

            DataTable dt = GetGridEntity();

            //if (OperationMode == EOperationMode.INSERT)
            //{
            //    if (dt.Rows.Count == 0)
            //    {
            //        //更新対象なし
            //        bbl.ShowMessage("E102");
            //        return;
            //    }
            //}

            //更新処理
            nnbl.D_Collect_Exec(dce,dt, (short)OperationMode);

            bbl.ShowMessage("I101");

            if (mKidouMode.Equals(0))
                //更新後画面クリア
                ChangeOperationMode(OperationMode);
            else
                EndSec();
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //排他処理を解除
            DeleteExclusive();

            Scr_Clr(0);

            SetDisplayMode = EMode.Uriage;
            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            switch (mode)
            {
                case EOperationMode.INSERT:
                    keyControls[(int)EIndex.Radio1].Focus();
                    F9Visible = false;
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
                    if (ctl.Equals(CboStoreCD))
                        continue;

                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                    {
                        ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
                    }
                    else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)) || ctl.GetType().Equals(typeof(Button)))
                    {

                    }
                    else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_RadioButton)))
                    {

                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }

                //顧客情報ALLクリア
                ClearCustomerInfo();

                foreach (Control ctl in keyLabels)
                {
                    ((CKM_SearchControl)ctl).LabelText = "";
                }

                ckM_RadioButton1.Checked = true;
                label2.Visible = ckM_RadioButton2.Checked;
                detailControls[(int)EIndex.CollectClearDate].Visible = ckM_RadioButton2.Checked;

                dtDetail = null;
                btnUriage.Enabled = false;
                btnDetail.Enabled = false;
                mConfirmExistsFlg = false;
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl.Equals(CboStoreCD))
                    continue;

                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)) || ctl.GetType().Equals(typeof(Button)))
                {

                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_RadioButton)))
                {

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

            lblKin1.Text = "";
            lblKin2.Text = "";
            lblKin3.Text = "";
            mSystemKBN = "";

            S_Clear_Grid();   //画面クリア（明細部）
            lblSumKin1.Text = "";
            lblSumKin2.Text = "";
            lblSumKin3.Text = "";
            lblSumKin4.Text = "";

        }
        private bool CalcKin()
        {
            decimal kin1 = 0;
            decimal kin2 = 0;
            decimal kin3 = 0;
            decimal kin4 = 0;
                       
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].CollectPlanNO))
                {
                    //明細.未入金額 = 明細.請求額 - 明細.入金済額 - 明細.今回入金額
                    mGrid.g_DArray[RW].Minyukin = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[RW].BillingGaku) - bbl.Z_Set(mGrid.g_DArray[RW].CollectAmount));
                    kin1 += bbl.Z_Set(mGrid.g_DArray[RW].BillingGaku);
                    kin2 += bbl.Z_Set(mGrid.g_DArray[RW].CollectAmount);
                    kin3 += bbl.Z_Set(mGrid.g_DArray[RW].ConfirmAmount);
                    kin4 += bbl.Z_Set(mGrid.g_DArray[RW].Minyukin);
                }
            }

            //Footer部
            lblSumKin1.Text = string.Format("{0:#,##0}", kin1);
            lblSumKin2.Text = string.Format("{0:#,##0}", kin2);
            //フッタ.今回入金額合計 = SUM(明細.今回入金額)
            lblSumKin3.Text = string.Format("{0:#,##0}", kin3);
            //フッタ.未入金額合計 = フッタ.請求額合計 - フッタ.入金済額合計 - SUM(明細.今回入金額)
            lblSumKin4.Text = string.Format("{0:#,##0}", kin4);

            //ヘッダ.消込額 = SUM(明細.今回入金額)
            lblKin2.Text = string.Format("{0:#,##0}", kin3);
            //lblKin2.Text = string.Format("{0:#,##0}", 0);
            //ヘッダ.残額 = ヘッダ.消込原資額 - SUM(明細.今回入金額)-その他消込額
            //残額を計算（残額＝消込原資額－消込額－その他消込）
            lblKin3.Text = string.Format("{0:#,##0}",bbl.Z_Set(lblKin1.Text) - kin3 - bbl.Z_Set(detailControls[(int)EIndex.DeductionConfirm].Text));

            return true;
        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            ScCustomer.LabelText = "";
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
                            keyControls[(int)EIndex.CollectNO].Enabled = Kbn == 0 ? true : false;
                            ScCollectNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            keyControls[(int)EIndex.ConfirmNO].Enabled = Kbn == 0 ? true : false;
                            ScConfirmNO.BtnSearch.Enabled = Kbn == 0 ? true : false;

                            if(OperationMode == EOperationMode.UPDATE || OperationMode == EOperationMode.SHOW || Kbn == 0)
                                keyControls[(int)EIndex.StoreCD].Enabled = Kbn == 0 ? true : false;
                            break;
                        }

                    case 1:
                        {
                            // ｷｰ部(複写)
                            break;
                        }

                    case 2:
                        {
                            // ｷｰ部
                            for (int index=(int)EIndex.Radio1; index <= (int)EIndex.CustomerCD; index++)
                            {
                                keyControls[index].Enabled = Kbn == 0 ? true : false;
                            }
                            ScCustomer.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            //if (Kbn.Equals(1))
                            //    btnNyuukinmoto.Enabled = false;
                        }
                        break;
                    case 3:
                        {
                            foreach (Control ctl in detailControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            ScStaff.BtnSearch.Enabled = Kbn == 0 ? true : false;

                            btnAutoSelect.Enabled = Kbn == 0 ? true : false;
                            Btn_SelectAll.Enabled = Kbn == 0 ? true : false;
                            Btn_NoSelect.Enabled = Kbn == 0 ? true : false;

                            // 明細部
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
                case 6://F7:
                    break;

                case 7://F8:行追加
                    break;
                case 8: //F9:検索
                    if (previousCtrl != null)
                    {
                        int idx = Array.IndexOf(detailControls, previousCtrl);
                        if (idx == (int)EIndex.NyukinGaku)
                            btnNyuukinmoto_Click(this, new EventArgs());
                    }
                    break;
                case 9://(F10)
                    ExecDisp();
                    break;
                case 11:    //F12:登録
                    {
                        //Ｑ１０１		
                        if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                            return;

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
                        if ((index == (int)EIndex.CollectNO || index == (int)EIndex.ConfirmNO) && detailControls[(int)EIndex.CollectDate].CanFocus)
                            detailControls[(int)EIndex.CollectDate].Focus();

                        else if (index == (int)EIndex.InputDateTo || index == (int)EIndex.CustomerCD) //取込日
                            btnSubF11.Focus();

                        else if (index == (int)EIndex.StoreCD)
                        {
                            if (ckM_RadioButton1.Checked)
                                ckM_RadioButton1.Focus();
                            else
                                ckM_RadioButton2.Focus();
                        }
                        else if (keyControls.Length - 1 > index)
                        {
                            if (keyControls[index + 1].CanFocus)
                            {
                                keyControls[index + 1].Focus();
                            }
                            else if (OperationMode == EOperationMode.UPDATE)
                            {
                                SetFocus();
                                
                            }
                            else
                            {
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
                            }
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

                        if (index == (int)EIndex.Remark)
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridNyuukin_S.ColNO.ConfirmAmount);

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

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int index = Array.IndexOf(detailControls, sender);

                if(index == (int)EIndex.NyukinGaku && OperationMode == EOperationMode.INSERT)
                {
                    F9Visible = true;
                    Btn_F9.Enabled = true;
                }
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

                    //bool changeFlg = false;
                    //if (w_ActCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    //    changeFlg = ((CKM_Controls.CKM_TextBox)w_ActCtl).Modified;
                    //else if (w_ActCtl.GetType().Equals(typeof(CKM_SearchControl)))
                    //    changeFlg = ((CKM_SearchControl)w_ActCtl).TxtCode.Modified;

                    bool lastCell = false;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridNyuukin_S.ColNO.CommentInStore)
                        if (w_Row == mGrid.g_MK_Max_Row - 1)
                            lastCell = true;


                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    ////手入力時金額を再計算
                    //if (changeFlg)
                    //{
                    //    switch (CL)
                    //    {
                    //        case (int)ClsGridNyuukin_S.ColNO.OrderSu:
                    //        case (int)ClsGridNyuukin_S.ColNO.ArrivalPlanMonth:

                    //            //変更された場合、金額再計算 発注単価×発注数
                    //            mGrid.g_DArray[w_Row].ArrivalPlanCD = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[w_Row].ArrivalPlanMonth) * bbl.Z_Set(mGrid.g_DArray[w_Row].OrderSu));
                    //            break;
                    //    }
                    //}

                    //チェック処理
                    if (CheckGrid(CL, w_Row) == false)
                    {
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
        private void BtnSubF11_Click(object sender, EventArgs e)
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
        /// 明細部チェックボックスクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CHK_Select_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;
                
                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                if (mGrid.g_DArray[w_Row].Chk)
                {
                    //今回入金額（＝明細.請求額 - 明細.入金済額）をセット。
                    mGrid.g_DArray[w_Row].ConfirmAmount = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[w_Row].BillingGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].CollectAmount));
                }
                else
                {
                    mGrid.g_DArray[w_Row].ConfirmAmount = "0";
                }

                //「【Details】今回入金額」の各項目の計算実行。
                CalcKin();

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
        
        private void Btn_SelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                //明細チェックボックスONかつ今回入金額に未入金額をセット。
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
                //明細チェックボックスOFFかつ今回入金額にゼロをセット。
                ChangeCheck(false);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void BtnAutoSelect_Click(object sender, EventArgs e)
        {
            try
            {   
                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //ヘッダ.入金額に到達するまで、明細上から(回収予定日が古い順に)今回入金額（＝明細.請求額 - 明細.入金済額）をセット。
                decimal wNyukin = bbl.Z_Set(detailControls[(int)EIndex.NyukinGaku].Text);


                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].BillingNo) == false)
                    {
                        decimal wKin = bbl.Z_Set(mGrid.g_DArray[RW].BillingGaku) - bbl.Z_Set(mGrid.g_DArray[RW].CollectAmount);
                        if (wNyukin <= 0)
                        {
                            mGrid.g_DArray[RW].Chk = false;
                            mGrid.g_DArray[RW].ConfirmAmount = "0";
                        }
                        else if (wNyukin >= wKin)
                        {
                            mGrid.g_DArray[RW].Chk = true;
                            mGrid.g_DArray[RW].ConfirmAmount = bbl.Z_SetStr(wKin);
                            wNyukin = wNyukin - wKin;
                        }
                        else
                        {
                            mGrid.g_DArray[RW].Chk = true;
                            mGrid.g_DArray[RW].ConfirmAmount = bbl.Z_SetStr(wNyukin);
                            wNyukin = 0;
                        }
                    }
                }
                //配列の内容を画面へセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                //「【Details】今回入金額」の各項目の計算実行。
                CalcKin();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        //private void btnSeikyu_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (mDisplayMode != EMode.Seikyu)
        //        {
        //            SetDisplayMode = EMode.Seikyu;

        //            //明細部を表示しなおす

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //        //EndSec();
        //    }
        //}

        private void btnUriage_Click(object sender, EventArgs e)
        {
            try
            {
                if (mDisplayMode != EMode.Uriage)
                {
                    SetDisplayMode = EMode.Uriage;
               
                    //明細部を表示しなおす
                    DispFromDataTable(-1);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (mDisplayMode != EMode.Detail)
                {
                    SetDisplayMode = EMode.Detail;

                    //明細部を表示しなおす
                    DispFromDataTable(-1);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void cboDenomination_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                    SetEnabled();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (OperationMode == EOperationMode.INSERT || !mKidouMode.Equals(0))
                {
                    btnNyuukinmoto.Enabled = ckM_RadioButton2.Checked;

                    //ScConfirmNO.Visible = ckM_RadioButton2.Checked;
                    //ckM_Label18.Visible = ckM_RadioButton2.Checked;
                    label2.Visible = ckM_RadioButton2.Checked;
                    detailControls[(int)EIndex.CollectClearDate].Visible = ckM_RadioButton2.Checked;
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
        /// 入金元検索クリック時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNyuukinmoto_Click(object sender, EventArgs e)
        {
            try
            {
                using (Search_Nyuukinmoto.Search_Nyuukinmoto frmSearch = new Search_Nyuukinmoto.Search_Nyuukinmoto())
                {
                    frmSearch.Kidou = 1;
                    //frmSearch.Customer = ScCustomer.LabelText;
                    frmSearch.ShowDialog();

                    if (!frmSearch.flgCancel)
                    {
                        keyControls[(int)EIndex.CustomerCD].Text = frmSearch.Customer;
                        bool ret = CheckKey((int)EIndex.CustomerCD);
                        if (!ret)
                            return;

                        if (frmSearch.Kbn.Equals("0"))
                        {
                            //戻り値①＝0の場合、画面転送表03に従ってデータ取得/画面表示
                            ret = CheckData(true, -1, 1, frmSearch.DenNO);
                        }
                        else
                        {
                            //戻り値①＝1の場合、画面転送表04に従ってデータ取得/画面表示
                            ret = CheckData(true, -1, 2, frmSearch.DenNO);
                        }
                        if (ret)
                        {
                            detailControls[(int)EIndex.CollectDate].Focus();
                        }
                    }
                    //detailControls[(int)EIndex.NyukinGaku].Text = bbl.Z_SetStr(frmSearch.BillingGaku);
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
                    ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
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
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].BillingNo) == false)
                {
                    mGrid.g_DArray[RW].Chk = check;

                    if(check)
                    {
                        //明細チェックボックスONかつ今回入金額に未入金額をセット。
                        mGrid.g_DArray[RW].ConfirmAmount = mGrid.g_DArray[RW].Minyukin;
                    }
                    else
                    {
                        //明細チェックボックスOFFかつ今回入金額にゼロをセット。
                        mGrid.g_DArray[RW].ConfirmAmount = "0";
                    }
                }
            }

            //「【Details】今回入金額」の各項目の計算実行。
            CalcKin();

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        }
        private void SetEnabled()
        {
            if (cboDenomination.SelectedIndex <= 0)
            {
                return;
            }

            M_DenominationKBN_Entity me = new M_DenominationKBN_Entity();
            me.DenominationCD = cboDenomination.SelectedValue.ToString();
            DenominationKBN_BL bl = new DenominationKBN_BL();
            bool ret = bl.M_DenominationKBN_Select(me);
            if (ret)
                mSystemKBN = me.SystemKBN;

            //入金金種=振込（M_DenominationKBN.SystemKBN=5）の場合 画面.銀行口座を入力可能にする。
            if (mSystemKBN.Equals("5"))
            {
                cboKouza.Enabled = true;
            }
            else
            {
                cboKouza.Enabled = false;
                cboKouza.SelectedIndex = 0;
            }
            //入金金種=小切手、手形（M_DenominationKBN.SystemKBN=6、11）の場合 画面.手形等決済日を入力可能にする。
            if (mSystemKBN.Equals("6") || mSystemKBN.Equals("11"))
            {
                detailControls[(int)EIndex.Tegata].Enabled = true;
            }
            else
            {
                detailControls[(int)EIndex.Tegata].Enabled = false;
                detailControls[(int)EIndex.Tegata].Text = "";
            }
        }
        private void SetEnabledForMode()
        {
            //通常起動
            if (mKidouMode.Equals(0) && OperationMode == EOperationMode.INSERT)
                return;

            if (mKidouMode.Equals(0) && mConfirmExistsFlg == false)
                return;

            //入金照会からの新規消込または修正時
            //　入金日、入金金種も入力不可
            detailControls[(int)EIndex.CollectDate].Enabled = false;
            cboDenomination.Enabled = false;
            //　入金額、手数料～その他(-)、その他消込額も入力不可
            for (int i=(int)EIndex.NyukinGaku; i<= (int)EIndex.DeductionConfirm; i++)
            {
                detailControls[i].Enabled = false;
            }

        }
        private void SetFocus()
        {
            for (int i = (int)EIndex.CboKoza; i <= (int)EIndex.Remark; i++)
            {
                if (detailControls[i].CanFocus)
                {
                    detailControls[i].Focus();
                    return;
                }
            }
            //明細の先頭項目へ
            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridNyuukin_S.ColNO.ConfirmAmount);
        }
    }
}








