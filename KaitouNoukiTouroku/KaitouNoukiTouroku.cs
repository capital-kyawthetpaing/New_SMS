using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace KaitouNoukiTouroku
{
    /// <summary>
    /// KaitouNoukiTouroku 回答納期登録
    /// </summary>
    internal partial class KaitouNoukiTouroku : FrmMainForm
    {
        private const string ProID = "KaitouNoukiTouroku";
        private const string ProNm = "回答納期登録";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            StoreCD,
            ArrivalPlanDateFrom,
            ArrivalPlanDateTo,
            ArrivalPlanMonthFrom,
            ArrivalPlanMonthTo,
            OrderDateFrom,
            OrderDateTo,

            OrderCD,
            OrderNOFrom,
            OrderNOTo,
            ChkMikakutei,
            ArrivalPlanCD,
            CheckBox3,
            CheckBox4,

            Edi,
            ArrivalPlanDate,
            ArrivalPlanMonth,
            ArrivalPlanKbn
                
        }

        //private Control[] keyControls;
        //private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private KaitouNoukiTouroku_BL knbl;
        private D_Order_Entity doe;
        private DataTable dtOrder;
        private DataTable dtForUpdate;  //排他用   
        private string mOldOrderNo = "";    //排他処理のため使用
        private string mOldArrivalPlanNO = "";    //排他処理のため使用


        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridKaitouNouki mGrid = new ClsGridKaitouNouki();
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

            if (ClsGridKaitouNouki.gc_P_GYO <= ClsGridKaitouNouki.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridKaitouNouki.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridKaitouNouki.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridKaitouNouki.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridKaitouNouki.gc_MaxCL;

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
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        {
                            Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            
                                btn.Click += new System.EventHandler(BTN_Detail_Click);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridKaitouNouki.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridKaitouNouki.ColNO.GYONO; i <= (int)ClsGridKaitouNouki.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridKaitouNouki.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridKaitouNouki.ColNO.OrderSu:
                            mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
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
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 0].CellCtl = CHK_EDICK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 0].CellCtl = BTN_Detail_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 0].CellCtl = IMC_KBN_0;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 0].CellCtl =  IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 0].CellCtl = IMN_WEBPR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 0].CellCtl = IMT_JUONO_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 0].CellCtl = IMT_PAYDT_0;    //支払予定日

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 1].CellCtl = CHK_EDICK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 1].CellCtl = BTN_Detail_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 1].CellCtl = IMC_KBN_1;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 1].CellCtl = IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 1].CellCtl = IMN_WEBPR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 1].CellCtl = IMT_REMAK_1;
            
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 1].CellCtl = IMT_JUONO_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 1].CellCtl = IMT_PAYDT_1;    //支払予定日

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 2].CellCtl = BTN_Detail_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 2].CellCtl = IMC_KBN_2;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 2].CellCtl = CHK_EDICK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 2].CellCtl = IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 2].CellCtl = IMN_WEBPR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 2].CellCtl = IMT_JUONO_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 2].CellCtl = IMT_PAYDT_2;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 3].CellCtl = BTN_Detail_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 3].CellCtl = IMC_KBN_3;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 3].CellCtl = CHK_EDICK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 3].CellCtl =  IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 3].CellCtl = IMN_GENER2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 3].CellCtl = IMN_WEBPR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 3].CellCtl = IMT_JUONO_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 3].CellCtl = IMT_PAYDT_3;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 4].CellCtl = BTN_Detail_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 4].CellCtl = IMC_KBN_4;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 4].CellCtl = CHK_EDICK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 4].CellCtl =  IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 4].CellCtl = IMN_GENER2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 4].CellCtl = IMN_WEBPR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 4].CellCtl = IMT_JUONO_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 4].CellCtl = IMT_PAYDT_4;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 5].CellCtl = BTN_Detail_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 5].CellCtl = IMC_KBN_5;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 5].CellCtl = CHK_EDICK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 5].CellCtl =  IMT_JANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 5].CellCtl = IMN_TEIKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 5].CellCtl = IMN_GENER2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 5].CellCtl = IMN_MEMBR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 5].CellCtl = IMN_WEBPR2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 5].CellCtl = IMT_REMAK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 5].CellCtl = IMT_JUONO_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 5].CellCtl = IMT_PAYDT_5;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 6].CellCtl = BTN_Detail_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 6].CellCtl = IMC_KBN_6;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 6].CellCtl = CHK_EDICK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 6].CellCtl = IMT_JANCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 6].CellCtl = IMT_KAIDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 6].CellCtl = IMN_TEIKA_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 6].CellCtl = IMN_GENER2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 6].CellCtl = IMN_MEMBR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 6].CellCtl = IMN_CLINT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 6].CellCtl = IMN_WEBPR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 6].CellCtl = IMN_WEBPR2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 6].CellCtl = IMT_REMAK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 6].CellCtl = IMT_JUONO_6;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 6].CellCtl = IMT_ARIDT_6;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 6].CellCtl = IMT_PAYDT_6;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 7].CellCtl = BTN_Detail_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD,7].CellCtl = IMC_KBN_7;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 7].CellCtl = CHK_EDICK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 7].CellCtl = IMT_JANCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 7].CellCtl = IMT_KAIDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 7].CellCtl = IMN_TEIKA_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 7].CellCtl = IMN_GENER2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 7].CellCtl = IMN_MEMBR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 7].CellCtl = IMN_CLINT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 7].CellCtl = IMN_WEBPR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 7].CellCtl = IMN_WEBPR2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 7].CellCtl = IMT_REMAK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 7].CellCtl = IMT_JUONO_7;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 7].CellCtl = IMT_ARIDT_7;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 7].CellCtl = IMT_PAYDT_7;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 8].CellCtl = BTN_Detail_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 8].CellCtl = IMC_KBN_8;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 8].CellCtl = CHK_EDICK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 8].CellCtl = IMT_JANCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 8].CellCtl = IMT_KAIDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 8].CellCtl = IMN_TEIKA_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 8].CellCtl = IMN_GENER2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 8].CellCtl = IMN_MEMBR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 8].CellCtl = IMN_CLINT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 8].CellCtl = IMN_WEBPR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 8].CellCtl = IMN_WEBPR2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 8].CellCtl = IMT_REMAK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 8].CellCtl = IMT_JUONO_8;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 8].CellCtl = IMT_ARIDT_8;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 8].CellCtl = IMT_PAYDT_8;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 9].CellCtl = BTN_Detail_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 9].CellCtl = IMC_KBN_9;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 9].CellCtl = CHK_EDICK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 9].CellCtl = IMT_JANCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 9].CellCtl = IMT_KAIDT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 9].CellCtl = IMN_TEIKA_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 9].CellCtl = IMN_GENER2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 9].CellCtl = IMN_MEMBR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 9].CellCtl = IMN_CLINT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 9].CellCtl = IMN_WEBPR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 9].CellCtl = IMN_WEBPR2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 9].CellCtl = IMT_REMAK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 9].CellCtl = IMT_JUONO_9;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 9].CellCtl = IMT_ARIDT_9;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 9].CellCtl = IMT_PAYDT_9;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.GYONO, 10].CellCtl = IMT_GYONO_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Btn, 10].CellCtl = BTN_Detail_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, 10].CellCtl = IMC_KBN_10;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.Chk, 10].CellCtl = CHK_EDICK_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderNo, 10].CellCtl = IMT_ITMCD_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.JanCD, 10].CellCtl = IMT_JANCD_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SizeName, 10].CellCtl = IMT_KAIDT_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.SKUName, 10].CellCtl = IMT_ITMNM_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderSu, 10].CellCtl = IMN_TEIKA_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth, 10].CellCtl = IMN_GENER2_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.TaniCD, 10].CellCtl = IMN_MEMBR_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ColorName, 10].CellCtl = IMN_CLINT_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentOutStore, 10].CellCtl = IMN_WEBPR_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.IndividualClientName, 10].CellCtl = IMN_WEBPR2_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.CommentInStore, 10].CellCtl = IMT_REMAK_10;
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.MakerItem, 10].CellCtl = IMT_JUONO_10;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanDate, 10].CellCtl = IMT_ARIDT_10;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.OrderDate, 10].CellCtl = IMT_PAYDT_10;    //支払予定日
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvNxt, (int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvPrv, (int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvNxt, (int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvSet, (int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvSet, (int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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
                            case (int)ClsGridKaitouNouki.ColNO.GYONO:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }
                            case (int)ClsGridKaitouNouki.ColNO.OrderDate:
                            case (int)ClsGridKaitouNouki.ColNO.OrderNo:
                            case (int)ClsGridKaitouNouki.ColNO.TaniCD:
                            case (int)ClsGridKaitouNouki.ColNO.OrderSu:
                            case (int)ClsGridKaitouNouki.ColNO.MakerItem:
                            case (int)ClsGridKaitouNouki.ColNO.JanCD:
                            case (int)ClsGridKaitouNouki.ColNO.SKUName:
                            case (int)ClsGridKaitouNouki.ColNO.SizeName:
                            case (int)ClsGridKaitouNouki.ColNO.ColorName:
                            case (int)ClsGridKaitouNouki.ColNO.CommentInStore:
                            case (int)ClsGridKaitouNouki.ColNO.CommentOutStore:
                            case (int)ClsGridKaitouNouki.ColNO.IndividualClientName:
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
                        case  (int)ClsGridKaitouNouki.ColNO.Chk:
                        case (int)ClsGridKaitouNouki.ColNO.Btn:
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
                            //// 入力可の列の設定
                            //for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            //{
                            //    if (m_EnableCnt - 1 < w_Row)
                            //        break;

                            //    //Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                            //    mGrid.g_MK_State[(int)ClsGridKaitouNouki.ColNO.JanCD, w_Row].Cell_Enabled = true;

                            //}
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            Scr_Lock(0, 0, 0);
                            Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                            this.Vsb_Mei_0.TabStop = false;

                            SetFuncKeyAll(this, "100001001010");

                            detailControls[(int)EIndex.OrderDateTo].Text = bbl.GetDate();
                            ChkMikakutei.Checked = true;
                            ChkMikakutei.Enabled = true;
                            ChkKanbai.Enabled = true;
                            ChkFuyo.Enabled = true;
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

                                // 'OrderNoがある場合、入力可（有効行）
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].OrderNo))
                                {
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    { 
                                        case (int)ClsGridKaitouNouki.ColNO.Btn:    // 
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;

                                            if (mGrid.g_DArray[w_Row].GyoCount > 1)
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Color = System.Drawing.Color.Yellow;
                                            }
                                            else
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                            }
                                            break;
                                        case (int)ClsGridKaitouNouki.ColNO.Chk:    // 
                                        case (int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth:    // 
                                        case (int)ClsGridKaitouNouki.ColNO.ArrivePlanDate:    //入荷予定日
                                        case (int)ClsGridKaitouNouki.ColNO.ArrivePlanCD:    // 
                                            {
                                                if (mGrid.g_DArray[w_Row].GyoCount.Equals(1))
                                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                else
                                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                                break;
                                            }
                                        case (int)ClsGridKaitouNouki.ColNO.JanCD:
                                        case (int)ClsGridKaitouNouki.ColNO.SKUName:    // 
                                        case (int)ClsGridKaitouNouki.ColNO.ColorName:    // 
                                        case (int)ClsGridKaitouNouki.ColNO.SizeName:
                                        case (int)ClsGridKaitouNouki.ColNO.CommentOutStore:    // 
                                        case (int)ClsGridKaitouNouki.ColNO.CommentInStore:    //
                                        case (int)ClsGridKaitouNouki.ColNO.OrderSu:    // 
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
                            //Scr_Lock(0, 1, 1);
                            SetFuncKeyAll(this, "100001000011");
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

        #endregion

        public KaitouNoukiTouroku()
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
                ModeVisible = false;

                // 明細部初期化
                this.S_SetInit_Grid();

                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                knbl = new KaitouNoukiTouroku_BL();
                CboStoreCD.Bind(ymd);
                CboArrivalPlanCD.Bind(ymd);//入荷予定区分
                CboArrivalPlanKbn.Bind(ymd);//入荷予定区分

                for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
                {
                    CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridKaitouNouki.ColNO.ArrivePlanCD, W_CtlRow].CellCtl;
                    sctl.Bind(ymd);
                }

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScOrderNO.Value1 = InOperatorCD;
                ScOrderNO.Value2 = stores;
                ScCopyOrderNO.Value1 = InOperatorCD;
                ScCopyOrderNO.Value2 = stores;
                ScOrder.Value1 = "1";

                Btn_F2.Text = "";
                Btn_F3.Text = "";
                Btn_F4.Text = "";
                Btn_F5.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F10.Text = "";

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = knbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                }

                ChangeOperationMode(EOperationMode.UPDATE);
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
            //keyControls = new Control[] { };
            //keyLabels = new Control[] {  };
            detailControls = new Control[] { CboStoreCD, ckM_TextBox1 ,ckM_TextBox2 ,ckM_TextBox4 ,ckM_TextBox3,ckM_TextBox6,ckM_TextBox5
                    , ScOrder.TxtCode, ScOrderNO.TxtCode, ScCopyOrderNO.TxtCode,ChkMikakutei, CboArrivalPlanCD,ChkKanbai,ChkFuyo                
                    ,ckM_TextBox7,  ckM_TextBox18, ckM_TextBox8,CboArrivalPlanKbn   
                         };
            detailLabels = new Control[] { ScOrder };
            searchButtons = new Control[] { ScOrder.BtnSearch, ScCopyOrderNO.BtnSearch, ScOrderNO.BtnSearch };

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
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
                detailControls[(int)EIndex.Edi].Focus();
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
                mOldOrderNo = "";
                mOldArrivalPlanNO = "";
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
        /// データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set)
        {
            //[D_Order_SelectDataForKaitouNouki]
            doe = GetEntity();

            dtOrder = knbl.D_Order_SelectDataForKaitouNouki(doe);

            //発注(D_Order)に存在しない場合、Error 
            if (dtOrder.Rows.Count == 0)
            {
                bbl.ShowMessage("S013");
                Scr_Clr(1);
                PreviousCtrl.Focus();
                return false;
            }
            else
            {
                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                DeleteExclusive();

                dtForUpdate = new DataTable();
                dtForUpdate.Columns.Add("kbn", Type.GetType("System.String"));
                dtForUpdate.Columns.Add("no", Type.GetType("System.String"));
                
                bool ret;

                //排他処理
                foreach (DataRow row in dtOrder.Rows)
                {
                    if (mOldOrderNo != row["OrderNo"].ToString())
                    {
                        ret = SelectAndInsertExclusive(Exclusive_BL.DataKbn.Hacchu, row["OrderNo"].ToString());
                        if (!ret)
                            return false;

                        mOldOrderNo = row["OrderNo"].ToString();

                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["kbn"] = (int)Exclusive_BL.DataKbn.Hacchu;
                        rowForUpdate["no"] = mOldOrderNo;
                        dtForUpdate.Rows.Add(rowForUpdate);
                    }
                    if (mOldArrivalPlanNO != row["ArrivalPlanNO"].ToString() && !string.IsNullOrWhiteSpace(row["ArrivalPlanNO"].ToString()))
                    {
                        ret = SelectAndInsertExclusive(Exclusive_BL.DataKbn.Nyuka, row["ArrivalPlanNO"].ToString());
                        if (!ret)
                            return false;

                        mOldArrivalPlanNO = row["ArrivalPlanNO"].ToString();

                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["kbn"] = (int)Exclusive_BL.DataKbn.Nyuka;
                        rowForUpdate["no"] = mOldArrivalPlanNO;
                        dtForUpdate.Rows.Add(rowForUpdate);
                    }
                }


                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                m_dataCnt = 0;
                foreach (DataRow row in dtOrder.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return false;
                    }

                    if (row["ROWNUM"].ToString() == "1")
                    {

                        mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                        mGrid.g_DArray[i].OldJanCD = mGrid.g_DArray[i].JanCD;
                        mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                        mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();
                        mGrid.g_DArray[i].OrderNo = row["OrderNo"].ToString();
                        mGrid.g_DArray[i].OrderSu = bbl.Z_SetStr(row["OrderSu"]);
                        mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();   // 
                        mGrid.g_DArray[i].MakerItem = row["MakerItem"].ToString();   // 
                        mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                        mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 

                        mGrid.g_DArray[i].ArrivalPlanDate = row["ArrivalPlanDate"].ToString();

                        if (bbl.Z_Set(row["ArrivalPlanMonth"]) > 0)
                            mGrid.g_DArray[i].ArrivalPlanMonth = bbl.FormatDate(row["ArrivalPlanMonth"].ToString() + "01").Substring(0,7);
                        else
                            mGrid.g_DArray[i].ArrivalPlanMonth = "";

                        mGrid.g_DArray[i].ArrivalPlanCD = row["ArrivalPlanCD"].ToString();
                        mGrid.g_DArray[i].TaniName = row["DestinationKBN"].ToString();
                        mGrid.g_DArray[i].CommentInStore = row["CommentInStore"].ToString();   // 
                        mGrid.g_DArray[i].CommentOutStore = row["CommentOutStore"].ToString();   //         
                        mGrid.g_DArray[i].OrderDate = row["OrderDate"].ToString();

                        //隠し項目
                        mGrid.g_DArray[i].DetailGyoNo =Convert.ToInt16( row["ROWNUM"]);
                        mGrid.g_DArray[i].GyoCount = Convert.ToInt16(row["CNT"]);
                        mGrid.g_DArray[i].OrderRows = row["OrderRows"].ToString();
                        mGrid.g_DArray[i].JuchuuNO = row["JuchuuNO"].ToString();
                        mGrid.g_DArray[i].JuchuuRows = row["JuchuuRows"].ToString();
                        mGrid.g_DArray[i].DestinationSoukoCD = row["DestinationSoukoCD"].ToString();
                        mGrid.g_DArray[i].StockNO = row["StockNO"].ToString();
                        mGrid.g_DArray[i].ReserveNO = row["ReserveNO"].ToString();
                        mGrid.g_DArray[i].ArrivalPlanNO = row["ArrivalPlanNO"].ToString();
                        mGrid.g_DArray[i].num2 = row["Num2"].ToString();
                        mGrid.g_DArray[i].InstructionSu = Convert.ToInt16(row["InstructionSu"]);

                        //変更されたかどうかを退避
                        mGrid.g_DArray[i].OldArrivalPlanDate = mGrid.g_DArray[i].ArrivalPlanDate;
                        mGrid.g_DArray[i].OldArrivalPlanMonth = mGrid.g_DArray[i].ArrivalPlanMonth;
                        mGrid.g_DArray[i].OldArrivalPlanCD = mGrid.g_DArray[i].ArrivalPlanCD != "-1" ? mGrid.g_DArray[i].ArrivalPlanCD : "";
                        mGrid.g_DArray[i].Update = 0;

                        m_dataCnt = i + 1;
                        i++;
                    }
                }

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            S_BodySeigyo(1, 0);
            S_BodySeigyo(1, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }
        protected override void ExecDisp()
        {
            for (int i = 0; i < (int)EIndex.Edi; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            //入荷予定日 （To)							
            //入荷予定月 （To)							
            //発注日     （To)							
            //のどれかの入力があること
            //カーソルは               入荷予定日（To)		
            if(string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ArrivalPlanDateTo].Text) && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ArrivalPlanMonthTo].Text)
                && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.OrderDateTo].Text))
            {
                //Ｅ１８０
                bbl.ShowMessage("E180");
                detailControls[(int)EIndex.ArrivalPlanDateTo].Focus();
                return ;
            }

            bool ret = CheckData(true);
            if(ret)
            {
                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridKaitouNouki.ColNO.ArrivePlanDate);
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
            string fmtYmd = "";

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
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

                case (int)EIndex.OrderDateFrom:
                case (int)EIndex.OrderDateTo:
                case (int)EIndex.ArrivalPlanDateFrom:
                case (int)EIndex.ArrivalPlanDateTo:
                case (int)EIndex.ArrivalPlanMonthFrom:
                case (int)EIndex.ArrivalPlanMonthTo:
                    //発注日のどちらかに入力があった場合に、未確定分、完売、不要のチェックボックスの入力を可能にする																											
                    if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.OrderDateFrom].Text) && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.OrderDateTo].Text))
                    {
                        SetEnabled(EIndex.ChkMikakutei, false);
                        ChkMikakutei.Checked = false;
                        ChkMikakutei.Enabled = false;
                        ChkKanbai.Enabled = false;
                        ChkFuyo.Enabled = false;
                    }

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    string strYmd = "";
                    switch (index) {
                        case (int)EIndex.ArrivalPlanMonthFrom:
                        case (int)EIndex.ArrivalPlanMonthTo:
                            strYmd = bbl.FormatDate(detailControls[index].Text + "/01");

                            break;
                         default:
                            strYmd = bbl.FormatDate(detailControls[index].Text);
                            break;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    switch (index)
                    {
                        case (int)EIndex.ArrivalPlanMonthFrom:
                        case (int)EIndex.ArrivalPlanMonthTo:
                            detailControls[index].Text = strYmd.Substring(0, 7);
                            break;
                        default:
                            detailControls[index].Text = strYmd;
                            break;
                    }

                    //見積日(From) ≧ 見積日(To)である場合Error
                    if (index == (int)EIndex.OrderDateTo || index == (int)EIndex.ArrivalPlanDateTo || index == (int)EIndex.ArrivalPlanMonthTo)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }
                    //発注日のどちらかに入力があった場合に、未確定分、完売、不要のチェックボックスの入力を可能にする																											
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.OrderDateFrom].Text) || !string.IsNullOrWhiteSpace(detailControls[(int)EIndex.OrderDateTo].Text))
                    {
                        ChkMikakutei.Enabled = true;
                        ChkKanbai.Enabled = true;
                        ChkFuyo.Enabled = true;
                    }
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

                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate()
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    bool ret = sbl.M_Vendor_SelectTop1(mve);

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
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }

                    break;

                case (int)EIndex.OrderNOTo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //入力無くても良い(It is not necessary to input)
                    }
                    else
                    {
                        //(From) ≧ (To)である場合Error
                        if (index == (int)EIndex.OrderNOTo)
                        {
                            if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                            {
                                int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                                if (result < 0)
                                {
                                    bbl.ShowMessage("E106");
                                    detailControls[index].Focus();
                                    return false;
                                }
                            }
                        }
                    }
                    break;

                case (int)EIndex.ArrivalPlanCD:
                    //選択は任意。
                    if (CboArrivalPlanKbn.Enabled)
                    {

                    }
                    break;

                case (int)EIndex.ArrivalPlanDate:
                    if (!knbl.ChkArrivePlanDate(detailControls[index].Text,ref fmtYmd))
                    {
                        return false;
                    }
                    detailControls[index].Text = fmtYmd;         
                    break;


                case (int)EIndex.ArrivalPlanMonth:
                    if (!knbl.ChkArrivalPlanMonth(detailControls[index].Text, detailControls[(int)EIndex.ArrivalPlanDate].Text, ref fmtYmd))
                    {
                        return false;
                    }
                    detailControls[index].Text = fmtYmd;
                    break;

                case (int)EIndex.ArrivalPlanKbn:
                    string kbn = "";
                    if (CboArrivalPlanKbn.SelectedIndex > 0)
                        kbn = CboArrivalPlanKbn.SelectedValue.ToString();

                    string num2 = "";   //未使用
                    if (!knbl.ChkArrivalPlanKbn(kbn, detailControls[(int)EIndex.ArrivalPlanDate].Text,detailControls[(int)EIndex.ArrivalPlanMonth].Text, out num2))
                    {
                        CboArrivalPlanKbn.MoveNext = false;
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

            w_Ctrl = detailControls[(int)EIndex.Edi];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvSet, (int)ClsGridKaitouNouki.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }
       
        private bool CheckGrid(int col, int row, bool chkAll=false, bool changeYmd=false)
        {
            string fmtYmd = "";

            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridKaitouNouki.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                        return false;
                }
            }

            switch (col)
            {
                case (int)ClsGridKaitouNouki.ColNO.ArrivePlanDate:
                    //変更されたかどうか
                    if (mGrid.g_DArray[row].OldArrivalPlanDate != mGrid.g_DArray[row].ArrivalPlanDate)
                    {
                        if (!knbl.ChkArrivePlanDate(mGrid.g_DArray[row].ArrivalPlanDate, ref fmtYmd))
                        {
                            return false;
                        }
                        mGrid.g_DArray[row].ArrivalPlanDate = fmtYmd;
                    }
                    break;

                case (int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth:
                    //変更されたかどうか
                    if (mGrid.g_DArray[row].OldArrivalPlanMonth != mGrid.g_DArray[row].ArrivalPlanMonth)
                    {
                        if (!knbl.ChkArrivalPlanMonth(mGrid.g_DArray[row].ArrivalPlanMonth, mGrid.g_DArray[row].ArrivalPlanDate, ref fmtYmd))
                        {
                            return false;
                        }
                        mGrid.g_DArray[row].ArrivalPlanMonth = fmtYmd;
                    }
                    break;

                case (int)ClsGridKaitouNouki.ColNO.ArrivePlanCD:
                    string num2 = "";
                    string kbn =mGrid.g_DArray[row].ArrivalPlanCD == "-1"? "" :mGrid.g_DArray[row].ArrivalPlanCD;
                    if (!knbl.ChkArrivalPlanKbn(kbn, mGrid.g_DArray[row].ArrivalPlanDate, mGrid.g_DArray[row].ArrivalPlanMonth,out num2))
                    {
                        return false;
                    }
                    mGrid.g_DArray[row].num2 = num2;
                    break;
            }            

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        //private bool ChkArrivePlanDate(string inYmd, ref string outYmd )
        //{
        //    //入力無くても良い(It is not necessary to input)
        //    if (string.IsNullOrWhiteSpace(inYmd))
        //    {
        //        outYmd = "";
        //        return true;
        //    }
        //    else
        //    {
        //        outYmd = bbl.FormatDate(inYmd);

        //        //日付として正しいこと(Be on the correct date)Ｅ１０３
        //        if (!bbl.CheckDate(outYmd))
        //        {
        //            //Ｅ１０３
        //            bbl.ShowMessage("E103");
        //            return false;
        //        }
        //        //今日より前の日付はエラー
        //        string sysDate = bbl.GetDate();
        //        int    result = outYmd.CompareTo(sysDate);
        //        if (result < 0)
        //        {
        //            bbl.ShowMessage("E123");
        //            return false;
        //        }
        //    }

        //    return true; ;
        //}

        //private bool ChkArrivalPlanMonth(string inYm,string arrivalPranDate, ref string outYm)
        //{
        //    //入力無くても良い(It is not necessary to input)
        //    if (string.IsNullOrWhiteSpace(inYm))
        //    {
        //        return true;

        //    }
        //    //入荷予定日に入力がある場合、入力されたらエラー
        //    if (!string.IsNullOrWhiteSpace(arrivalPranDate))
        //    {
        //        bbl.ShowMessage("E103");
        //        return false;
        //    }

        //    string ymd = bbl.FormatDate(inYm + "/01");
        //    if (!bbl.CheckDate(ymd))
        //    {
        //        //Ｅ１０３
        //        bbl.ShowMessage("E103");
        //        return false;
        //    }
        //    outYm = ymd.Substring(0, 7);

        //    //今月より前の年月はエラー
        //    string sysDate = bbl.GetDate();
        //    int  result = outYm.CompareTo(sysDate.Substring(0, 7));
        //    if (result < 0)
        //    {
        //        bbl.ShowMessage("E123");
        //        return false;
        //    }
        //    return true;
        //}

        //private bool ChkArrivalPlanKbn(string kbn, string arrivalPlanDate, string arrivalPlanMonth)
        //{
        //    //選択無くてもよい
        //    DataTable dt = null;

        //    if (!string.IsNullOrWhiteSpace(kbn))
        //    {
        //        M_MultiPorpose_Entity me = new M_MultiPorpose_Entity
        //        {
        //            ID = MultiPorpose_BL.ID_ArrivalPlanCD,
        //            Key = kbn
        //        };

        //        MultiPorpose_BL mbl = new MultiPorpose_BL();
        //        dt = mbl.M_MultiPorpose_Select(me);
        //        if (dt.Rows.Count > 0)
        //        { }
        //        else
        //        {
        //            //Ｅ１０１
        //            bbl.ShowMessage("E101");
        //            return false;
        //        }

        //        //入荷予定日に入力がある場合
        //        //区分に入力されたら、M_MultiPorpose.Num2が2以外はエラー
        //        if (!string.IsNullOrWhiteSpace(arrivalPlanDate))
        //        {
        //            if (dt.Rows[0]["Num2"].ToString() != "2")
        //            {
        //                bbl.ShowMessage("E175");
        //                return false;
        //            }

        //        }
        //    }

        //    //年月に入力がある場合
        //    //入力必須
        //    //Num2が1以外はエラー
        //    if (!string.IsNullOrWhiteSpace(arrivalPlanMonth))
        //    {
        //        if (string.IsNullOrWhiteSpace(kbn))
        //        {
        //            //Ｅ１０２
        //            bbl.ShowMessage("E102");
        //            return false;
        //        }
        //        else if (dt.Rows[0]["Num2"].ToString() != "1")
        //        {
        //            bbl.ShowMessage("E175");
        //            return false;
        //        }

        //    }
        //    //入荷予定日、年月にともに入力がない場合
        //    if (string.IsNullOrWhiteSpace(arrivalPlanDate) && string.IsNullOrWhiteSpace(arrivalPlanMonth))
        //    {
        //        //入力必須
        //        if (string.IsNullOrWhiteSpace(kbn))
        //        {
        //            //Ｅ１０２
        //            bbl.ShowMessage("E102");
        //            return false;
        //        }
        //        //Num2が3,4,5,6以外はエラー
        //        else if (dt.Rows[0]["Num2"].ToString() != "3" && dt.Rows[0]["Num2"].ToString() != "4" && dt.Rows[0]["Num2"].ToString() != "5" && dt.Rows[0]["Num2"].ToString() != "6")
        //        {
        //            bbl.ShowMessage("E175");
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Order_Entity GetEntity()
        {
            doe = new D_Order_Entity
            {
                ArrivalPlanDateFrom = detailControls[(int)EIndex.ArrivalPlanDateFrom].Text,
                ArrivalPlanDateTo = detailControls[(int)EIndex.ArrivalPlanDateTo].Text,
                ArrivalPlanMonthFrom = detailControls[(int)EIndex.ArrivalPlanMonthFrom].Text.Replace("/", ""),
                ArrivalPlanMonthTo = detailControls[(int)EIndex.ArrivalPlanMonthTo].Text.Replace("/", ""),
                OrderDateFrom = detailControls[(int)EIndex.OrderDateFrom].Text,
                OrderDateTo = detailControls[(int)EIndex.OrderDateTo].Text,

                StoreCD = CboStoreCD.SelectedValue.ToString(),
                OrderCD = detailControls[(int)EIndex.OrderCD].Text,

                OrderNoFrom = detailControls[(int)EIndex.OrderNOFrom].Text,
                OrderNoTo = detailControls[(int)EIndex.OrderNOTo].Text,
                EDIDate = detailControls[(int)EIndex.Edi].Text,
                ArrivalPlanCD = CboArrivalPlanCD.SelectedIndex > 0 ? CboArrivalPlanCD.SelectedValue.ToString() : ""
            };

            if (ChkMikakutei.Checked)
                doe.ChkMikakutei = 1;
            else
                doe.ChkMikakutei = 0;

            if (ChkKanbai.Checked)
                doe.ChkKanbai = 1;
            else
                doe.ChkKanbai = 0;

            if (ChkFuyo.Checked)
                doe.ChkFuyo = 1;
            else
                doe.ChkFuyo = 0;

            return doe;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("Seq", typeof(int));
            dt.Columns.Add("OrderNO", typeof(string));
            dt.Columns.Add("OrderRows", typeof(int));
            dt.Columns.Add("GyoNO", typeof(int));
            dt.Columns.Add("JuchuuNO", typeof(string));
            dt.Columns.Add("JuchuuRows", typeof(int));
            dt.Columns.Add("StockNO", typeof(string));
            dt.Columns.Add("ReserveNO", typeof(string));
            dt.Columns.Add("ArrivalPlanNO", typeof(string));

            dt.Columns.Add("ArrivalPlanDate", typeof(DateTime));
            dt.Columns.Add("ArrivalPlanMonth", typeof(int));
            dt.Columns.Add("ArrivalPlanCD", typeof(string));
            dt.Columns.Add("CalcuArrivalPlanDate", typeof(DateTime));
            dt.Columns.Add("ArrivalPlanSu", typeof(int));
            dt.Columns.Add("SoukoCD", typeof(string));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("num2", typeof(int));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int seq = 0;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //zが更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].OrderNo) == false && mGrid.g_DArray[RW].Update >= 1)
                {
                    int rowNo = 1;

                    string calcuArrivalPlanDate = GetCalcuArrivalPlanDate(mGrid.g_DArray[RW].ArrivalPlanDate, mGrid.g_DArray[RW].ArrivalPlanMonth, mGrid.g_DArray[RW].ArrivalPlanCD);
                    if (calcuArrivalPlanDate != "")
                        mGrid.g_DArray[RW].CalcuArrivalPlanDate = calcuArrivalPlanDate;
                    else
                        mGrid.g_DArray[RW].CalcuArrivalPlanDate = "";

                    string arrivalPlanDate = mGrid.g_DArray[RW].ArrivalPlanDate;
                    if(mGrid.g_DArray[RW].Update == 2)
                    {
                        arrivalPlanDate = mGrid.g_DArray[RW].FirstArrivalPlanDate;
                    }

                    if (mGrid.g_DArray[RW].ArrivalPlanCD.Equals("-1"))
                        mGrid.g_DArray[RW].ArrivalPlanCD = "";

                   dt.Rows.Add(seq
                        , mGrid.g_DArray[RW].OrderNo
                        , mGrid.g_DArray[RW].OrderRows
                        , rowNo //mGrid.g_DArray[RW].GYONO
                        , mGrid.g_DArray[RW].JuchuuNO == "" ? null : mGrid.g_DArray[RW].JuchuuNO
                        , mGrid.g_DArray[RW].JuchuuRows
                        , mGrid.g_DArray[RW].StockNO == "" ? null : mGrid.g_DArray[RW].StockNO
                        , mGrid.g_DArray[RW].ReserveNO == "" ? null : mGrid.g_DArray[RW].ReserveNO
                        , mGrid.g_DArray[RW].ArrivalPlanNO.Trim() == "" ? null : mGrid.g_DArray[RW].ArrivalPlanNO.Trim()
                        , arrivalPlanDate == "" ? null : arrivalPlanDate
                        , bbl.Z_Set(mGrid.g_DArray[RW].ArrivalPlanMonth.Replace("/", ""))
                        , mGrid.g_DArray[RW].ArrivalPlanCD == "" ? null : mGrid.g_DArray[RW].ArrivalPlanCD
                        , mGrid.g_DArray[RW].CalcuArrivalPlanDate == "" ? null : mGrid.g_DArray[RW].CalcuArrivalPlanDate
                        , mGrid.g_DArray[RW].ArrivalPlanSu ==null ? bbl.Z_Set(mGrid.g_DArray[RW].OrderSu): bbl.Z_Set(mGrid.g_DArray[RW].ArrivalPlanSu)
                        , mGrid.g_DArray[RW].DestinationSoukoCD == "" ? null : mGrid.g_DArray[RW].DestinationSoukoCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                        , mGrid.g_DArray[RW].SKUCD == "" ? null : mGrid.g_DArray[RW].SKUCD
                        , mGrid.g_DArray[RW].JanCD == "" ? null : mGrid.g_DArray[RW].JanCD
                        , mGrid.g_DArray[RW].SKUName == "" ? null : mGrid.g_DArray[RW].SKUName
                        , bbl.Z_Set(mGrid.g_DArray[RW].num2)
                        , mGrid.g_DArray[RW].Update
                        );

                    rowNo++;
                    seq++;

                    //該当するデータを抽出（子画面の行番号順）1行目は明細画面よりセットする
                    DataRow[] selectedRows = dtOrder.Select("OrderNo ='" + mGrid.g_DArray[RW].OrderNo + "' AND OrderRows =" + mGrid.g_DArray[RW].OrderRows + " AND ROWNUM > 1", "ROWNUM");
                    foreach (DataRow row in selectedRows)
                    {
                        //CalcuArrivalPlanDateの算出
                        calcuArrivalPlanDate = GetCalcuArrivalPlanDate(row["ArrivalPlanDate"].ToString(),row["ArrivalPlanMonth"].ToString(), row["ArrivalPlanCD"].ToString());
                        if (calcuArrivalPlanDate != "")
                            row["CalcuArrivalPlanDate"] = calcuArrivalPlanDate;
                        else
                            row["CalcuArrivalPlanDate"] = DBNull.Value;

                        dt.Rows.Add(seq
                       , row["OrderNo"].ToString()
                       , row["OrderRows"].ToString()
                       , rowNo 
                       , row["JuchuuNO"].ToString() == "" ? null : row["JuchuuNO"].ToString()
                       , row["JuchuuRows"].ToString()
                       , row["StockNO"].ToString() == "" ? null : row["StockNO"].ToString()
                       , row["ReserveNO"].ToString() == "" ? null : row["ReserveNO"].ToString()
                       , row["ArrivalPlanNO"].ToString().Trim() == "" ? null : row["ArrivalPlanNO"].ToString().Trim()
                       , row["ArrivalPlanDate"].ToString() == "" ? null : row["ArrivalPlanDate"].ToString()
                       , bbl.Z_Set(row["ArrivalPlanMonth"].ToString().Replace("/", ""))
                       , row["ArrivalPlanCD"].ToString() == "" ? null : row["ArrivalPlanCD"].ToString()
                       , row["CalcuArrivalPlanDate"].ToString() == "" ? null : row["CalcuArrivalPlanDate"].ToString()
                       , bbl.Z_Set(row["ArrivalPlanSu"].ToString())
                       , row["DestinationSoukoCD"].ToString() == "" ? null : row["DestinationSoukoCD"].ToString()
                       , bbl.Z_Set(row["AdminNO"].ToString())
                       , row["SKUCD"].ToString() == "" ? null : row["SKUCD"].ToString()
                       , row["JanCD"].ToString() == "" ? null : row["JanCD"].ToString()
                       , row["SKUName"].ToString() == "" ? null : row["SKUName"].ToString()
                       , bbl.Z_Set(row["num2"].ToString())
                       , row["UpdateFlg"].ToString()
                       );

                        rowNo++;
                        seq++;
                    }                 
                }
            }

            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetCalcuArrivalPlanDate(string arrivalPlanDate , string arrivalPlanMonth,string arrivalPlanCD)
        {
            string calcuArrivalPlanDate = "";

            //入荷予定日≠Nullの場合そのまま、入荷予定日	
            if (!string.IsNullOrWhiteSpace(arrivalPlanDate))
            {
                calcuArrivalPlanDate = arrivalPlanDate;
            }
            else
            {
                //入荷予定年月≠Nullの場合
                if (bbl.Z_Set(arrivalPlanMonth.Replace("/", "")) != 0)
                {
                    //入荷予定区分
                    M_MultiPorpose_Entity me = new M_MultiPorpose_Entity
                    {
                        ID = MultiPorpose_BL.ID_ArrivalPlanCD,
                        Key = arrivalPlanCD
                    };

                    MultiPorpose_BL mbl = new MultiPorpose_BL();
                    DataTable  dt = mbl.M_MultiPorpose_Select(me);
                    if (dt.Rows.Count > 0)
                    {
                        //Num2＝1 の場合
                        //Num1	を日として、年月＋日とする。日＝31の時は月末の意味なので、小の月に注意（例 2月や4月）																												
                        if (dt.Rows[0]["Num2"].ToString() == "1")
                        {
                            string day = string.Format("{0:00}", dt.Rows[0]["Num1"].ToString());
                            string ymd;
                            if (day != "31")
                            {
                                calcuArrivalPlanDate = bbl.FormatDate(arrivalPlanMonth + day);
                            }
                            else
                            {
                                ymd = bbl.FormatDate(arrivalPlanMonth + "01");    //月末を後で計算
                                calcuArrivalPlanDate = Convert.ToDateTime(ymd).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
                            }
                        }
                        else
                        {
                            //Num2≠1 の場合
                        }
                    }
                }
                else
                {
                    //入荷予定年月＝Nullの場合 Null
                }
            }

            return calcuArrivalPlanDate;
        }
        protected override void ExecSec()
        {
            int UpdateRows = 0;

            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].OrderNo) == false)
                {
                    for (int CL = (int)ClsGridKaitouNouki.ColNO.ArrivePlanDate; CL < (int)ClsGridKaitouNouki.ColNO.COUNT; CL++)
                    {
                        if (CheckGrid(CL, RW, true) == false)
                        {
                            //Focusセット処理
                            ERR_FOCUS_GRID_SUB(CL, RW);
                            return;
                        }
                    }
                    //変更されたかどうかを退避
                    if (mGrid.g_DArray[RW].OldArrivalPlanDate != mGrid.g_DArray[RW].ArrivalPlanDate 
                    || mGrid.g_DArray[RW].OldArrivalPlanMonth != mGrid.g_DArray[RW].ArrivalPlanMonth
                    || mGrid.g_DArray[RW].OldArrivalPlanCD != mGrid.g_DArray[RW].ArrivalPlanCD)
                    {
                        if(string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ArrivalPlanDate) && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ArrivalPlanMonth)
                            && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ArrivalPlanCD))
                            mGrid.g_DArray[RW].Update = 2;
                        else
                            mGrid.g_DArray[RW].Update = 1;

                        UpdateRows++;
                    }
                    else if(mGrid.g_DArray[RW].Update != 0)
                    {
                        UpdateRows++;
                    }
                }
            }

            //明細が1件も登録されていなければ、エラー Ｅ１８９
            if (UpdateRows==0)
            {
                //更新対象なし
                bbl.ShowMessage("E189");
                return;
            }

            DataTable dt = GetGridEntity();

            doe.StoreCD = CboStoreCD.SelectedValue.ToString();
            doe.OrderCD = detailControls[(int)EIndex.OrderCD].Text;

            //更新処理
            knbl.Order_Exec(doe,dt, (short)OperationMode, InOperatorCD, InPcID);

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

            detailControls[(int)EIndex.ArrivalPlanDateFrom].Focus();

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
                    else
                    {
                        ctl.Text = "";
                    }
                }

                //顧客情報ALLクリア
                ClearCustomerInfo();

                foreach (Control ctl in detailLabels)
                {
                    ((CKM_SearchControl)ctl).LabelText = "";
                }

            }

            S_Clear_Grid();   //画面クリア（明細部）

        }

        /// <summary>
        /// 仕入先情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            ScOrder.LabelText = "";
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
                            //foreach (Control ctl in keyControls)
                            //{
                            //    ctl.Enabled = Kbn == 0 ? true : false;
                            //}
                            foreach (Control ctl in detailControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            SetEnabled(EIndex.ChkMikakutei, false);
                            ChkMikakutei.Enabled = false;
                            ChkKanbai.Enabled = false;
                            ChkFuyo.Enabled = false;

                            for (int index = 0; index < searchButtons.Length; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;
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

                case 7://F8:
                    break;

                case 9://F10
                    break;

                case 8: //F9:検索
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

                        if (index == (int)EIndex.Edi) //取込日
                            ExecDisp();

                        else if (index == (int)EIndex.ArrivalPlanKbn)
                            Btn_Hanei.Focus();
                            
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
                PreviousCtrl = this.ActiveControl;
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
                PreviousCtrl = this.ActiveControl;

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

                    if (CL == (int)ClsGridKaitouNouki.ColNO.IndividualClientName)
                        if (w_Row == mGrid.g_MK_Max_Row - 1)
                            lastCell = true;


                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    ////手入力時金額を再計算
                    //if (changeFlg)
                    //{
                    //    switch (CL)
                    //    {
                    //        case (int)ClsGridKaitouNouki.ColNO.OrderSu:
                    //        case (int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth:

                    //            //変更された場合、金額再計算 発注単価×発注数
                    //            mGrid.g_DArray[w_Row].ArrivalPlanCD = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[w_Row].ArrivalPlanMonth) * bbl.Z_Set(mGrid.g_DArray[w_Row].OrderSu));
                    //            break;
                    //    }
                    //}

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

        private void ChkMikakutei_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //チェックがONのときのみ入力可
                CboArrivalPlanCD.Enabled = ChkMikakutei.Checked;
                if (!ChkMikakutei.Checked)
                    CboArrivalPlanCD.SelectedIndex = -1;

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
                SetEnabled(EIndex.CheckBox3, ChkKanbai.Checked);
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
                        SetEnabled(EIndex.CheckBox4, ChkFuyo.Checked);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void BtnHanei_Click(object sender, EventArgs e)
        {
            try
            {
                if (!F12Enable)
                    return;

                //明細左端のチェックボックスONの明細に対し入荷予定日をセットする。
                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (mGrid.g_DArray[RW].Chk)
                    {
                        //チェックボックスをOFFにする。
                        mGrid.g_DArray[RW].Chk = false;
                        mGrid.g_DArray[RW].ArrivalPlanDate = detailControls[(int)EIndex.ArrivalPlanDate].Text;
                        mGrid.g_DArray[RW].ArrivalPlanMonth = detailControls[(int)EIndex.ArrivalPlanMonth].Text;
                        if (CboArrivalPlanKbn.SelectedIndex > 0)
                            mGrid.g_DArray[RW].ArrivalPlanCD = CboArrivalPlanKbn.SelectedValue.ToString();
                        else
                            mGrid.g_DArray[RW].ArrivalPlanCD = "";
                    }
                }
                //配列の内容を画面へセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridKaitouNouki.ColNO.ArrivePlanDate);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 明細部在庫ボタンクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Detail_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                //DataTableに画面の内容を反映
                DataRow[] rows = dtOrder.Select("OrderNo ='" + mGrid.g_DArray[w_Row].OrderNo + "' AND OrderRows =" + mGrid.g_DArray[w_Row].OrderRows + " AND ROWNUM =1");
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].ArrivalPlanDate))
                {
                    rows[0]["ArrivalPlanDate"] = mGrid.g_DArray[w_Row].ArrivalPlanDate;
                }
                else
                {
                    rows[0]["ArrivalPlanDate"] = DBNull.Value;
                }
                rows[0]["ArrivalPlanMonth"] = bbl.Z_Set( mGrid.g_DArray[w_Row].ArrivalPlanMonth.Replace("/",""));
                rows[0]["ArrivalPlanCD"] = mGrid.g_DArray[w_Row].ArrivalPlanCD;
                rows[0]["num2"] = bbl.Z_Set(mGrid.g_DArray[w_Row].num2);

                FrmDetail frm = new FrmDetail(ref dtOrder);
                frm.de.JANCD = mGrid.g_DArray[w_Row].JanCD;
                frm.de.MakerItem = mGrid.g_DArray[w_Row].MakerItem;
                frm.de.SKUName = mGrid.g_DArray[w_Row].SKUName;
                frm.de.Suryo = mGrid.g_DArray[w_Row].OrderSu;

                //frm.de.DataRows = rows;   //dtOrder.Select("OrderNo ='" + mGrid.g_DArray[w_Row].OrderNo + "' AND OrderRows =" + mGrid.g_DArray[w_Row].OrderRows , "ROWNUM"); 
                frm.de.OrderNo = mGrid.g_DArray[w_Row].OrderNo;
                frm.de.OrderRows = mGrid.g_DArray[w_Row].OrderRows;
                //frm.de.DtDetail = dtOrder;

               frm.ShowDialog();

                if (!frm.flgCancel)
                {
                    mGrid.g_DArray[w_Row].GyoCount = frm.GyoCount;

                    rows = dtOrder.Select("OrderNo ='" + mGrid.g_DArray[w_Row].OrderNo + "' AND OrderRows =" + mGrid.g_DArray[w_Row].OrderRows + " AND ROWNUM =1");

                    if (rows[0]["UpdateFlg"].ToString() == "2") //行削除データ
                    {
                        if (!string.IsNullOrWhiteSpace(rows[0]["ArrivalPlanDate"].ToString()))
                        {
                            mGrid.g_DArray[w_Row].FirstArrivalPlanDate = Convert.ToDateTime(rows[0]["ArrivalPlanDate"]).ToShortDateString();
                        }
                        else
                        {
                            mGrid.g_DArray[w_Row].FirstArrivalPlanDate = "";
                        }

                        mGrid.g_DArray[w_Row].ArrivalPlanDate = "";
                        mGrid.g_DArray[w_Row].ArrivalPlanMonth = "";
                        mGrid.g_DArray[w_Row].ArrivalPlanCD = "";
                        mGrid.g_DArray[w_Row].ArrivalPlanSu = "0";
                        mGrid.g_DArray[w_Row].num2 = "0";
                        mGrid.g_DArray[w_Row].Update = 2;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(rows[0]["ArrivalPlanDate"].ToString()))
                        {
                            mGrid.g_DArray[w_Row].ArrivalPlanDate = Convert.ToDateTime(rows[0]["ArrivalPlanDate"]).ToShortDateString();
                        }
                        else
                        {
                            mGrid.g_DArray[w_Row].ArrivalPlanDate = "";
                        }
                        if (knbl.Z_Set(rows[0]["ArrivalPlanMonth"]) > 0)
                            mGrid.g_DArray[w_Row].ArrivalPlanMonth = rows[0]["ArrivalPlanMonth"].ToString();
                        else
                            mGrid.g_DArray[w_Row].ArrivalPlanMonth = "";

                        mGrid.g_DArray[w_Row].ArrivalPlanCD = rows[0]["ArrivalPlanCD"].ToString();
                        mGrid.g_DArray[w_Row].num2 = rows[0]["num2"].ToString();
                        mGrid.g_DArray[w_Row].ArrivalPlanSu = knbl.Z_SetStr(rows[0]["ArrivalPlanSu"]);
                        mGrid.g_DArray[w_Row].Update = 1;
                    }

                    S_BodySeigyo(1, 1);

                    //配列の内容を画面へセット
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                }

                //画面に小画面内容を表示

                //dtOrderに反映

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
        #endregion

        private void SetEnabled(EIndex index, bool enabled)
        {
                switch (index)
                {
                    case EIndex.ChkMikakutei:
                        if (enabled)
                        {
                            detailControls[(int)EIndex.ArrivalPlanCD].Enabled = enabled;
                        }
                        else
                        {
                            ((CKM_Controls.CKM_ComboBox)detailControls[(int)EIndex.ArrivalPlanCD]).SelectedIndex = -1;
                            detailControls[(int)EIndex.ArrivalPlanCD].Enabled = enabled;
                        }
                        break;
                }
        }
        private void ChangeCheck(bool check)
        {
            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].OrderNo) == false)
                {
                    if(mGrid.g_MK_State[(int)ClsGridKaitouNouki.ColNO.Chk, RW].Cell_Enabled)
                        mGrid.g_DArray[RW].Chk = check;
                }
            }
            
            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        }
    }
}








