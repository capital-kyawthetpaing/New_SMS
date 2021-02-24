using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace ShukkaShijiTouroku
{
    /// <summary>
    /// ShukkaShijiTouroku 出荷指示登録
    /// </summary>
    internal partial class ShukkaShijiTouroku : FrmMainForm
    {
        private const string ProID = "ShukkaShijiTouroku";
        private const string ProNm = "出荷指示登録";
        private const string JuchuuNyuuryoku = "TempoJuchuuNyuuryoku.exe";
        private const short mc_L_END = 3; // ロック用


        private enum EIndex : int
        {
            StoreCD,
            PlanDateFrom,
            PlanDateTo,
            Chk1,
            Chk2,
            Chk3,
            Chk4,
            Chk5,
            DeliveryName,

            ChkHakkozumi,
            ChkShukkazumi,

            DeliveryPlanDate,
            CarrierCD,
            COUNT
        }

        //private Control[] keyControls;
        //private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private ShukkaShijiTouroku_BL ssbl;
        private D_Instruction_Entity die;
        private DataTable dtInstruction;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldInstructionNO = "";    //排他処理のため使用


        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridShukka mGrid = new ClsGridShukka();
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

            if (ClsGridShukka.gc_P_GYO <= ClsGridShukka.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridShukka.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridShukka.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridShukka.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridShukka.gc_MaxCL;

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

                            if (w_CtlCol == (int)ClsGridShukka.ColNO.BtnJuchu)
                                btn.Click += new System.EventHandler(BTN_Juchu_Click);
                            else
                                btn.Click += new System.EventHandler(BTN_Detail_Click);

                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridShukka.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridShukka.ColNO.GYONO; i <= (int)ClsGridShukka.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridShukka.ColNO.COUNT; W_CtlCol++)
                {
                    //switch (W_CtlCol)
                    //{
                    //    case (int)ClsGridShukka.ColNO.Space:
                    //        mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                    //        break;
                    //}

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
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 0].CellCtl = CHK_EDICK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 0].CellCtl = BTN_Detail_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 0].CellCtl = BTN_Detail2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 0].CellCtl = IMC_KBN_0;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 0].CellCtl = CHK_Tekiyo_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 0].CellCtl = CHK_Sokujitu_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 0].CellCtl = IMN_WEBPR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 0].CellCtl = CHK_Shikyu_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 0].CellCtl = LBL_DEDAY_0;// IMT_PAYDT_0;    //支払予定日

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 1].CellCtl = CHK_EDICK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 1].CellCtl = BTN_Detail_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 1].CellCtl = BTN_Detail2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 1].CellCtl = IMC_KBN_1;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 1].CellCtl = CHK_Tekiyo_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 1].CellCtl = CHK_Sokujitu_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 1].CellCtl = IMN_WEBPR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 1].CellCtl = IMT_REMAK_1;

            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 1].CellCtl = CHK_Shikyu_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 1].CellCtl = LBL_DEDAY_1;//IMT_PAYDT_1;    //支払予定日

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 2].CellCtl = BTN_Detail_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 2].CellCtl = BTN_Detail2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 2].CellCtl = IMC_KBN_2;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 2].CellCtl = CHK_EDICK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 2].CellCtl = CHK_Tekiyo_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 2].CellCtl = CHK_Sokujitu_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 2].CellCtl = IMN_WEBPR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 2].CellCtl = CHK_Shikyu_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 2].CellCtl = LBL_DEDAY_2;//IMT_PAYDT_2;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 3].CellCtl = BTN_Detail_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 3].CellCtl = BTN_Detail2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 3].CellCtl = IMC_KBN_3;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 3].CellCtl = CHK_EDICK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 3].CellCtl = CHK_Tekiyo_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 3].CellCtl = CHK_Sokujitu_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 3].CellCtl = IMN_WEBPR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 3].CellCtl = CHK_Shikyu_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 3].CellCtl = LBL_DEDAY_3;// IMT_PAYDT_3;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 4].CellCtl = BTN_Detail_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 4].CellCtl = BTN_Detail2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 4].CellCtl = IMC_KBN_4;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 4].CellCtl = CHK_EDICK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 4].CellCtl = CHK_Tekiyo_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 4].CellCtl = CHK_Sokujitu_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 4].CellCtl = IMN_WEBPR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 4].CellCtl = CHK_Shikyu_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 4].CellCtl = LBL_DEDAY_4;// IMT_PAYDT_4;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 5].CellCtl = BTN_Detail_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 5].CellCtl = BTN_Detail2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 5].CellCtl = IMC_KBN_5;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 5].CellCtl = CHK_EDICK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 5].CellCtl = CHK_Tekiyo_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 5].CellCtl = IMN_TEIKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 5].CellCtl = CHK_Sokujitu_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 5].CellCtl = IMN_MEMBR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 5].CellCtl = IMN_WEBPR2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 5].CellCtl = IMT_REMAK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 5].CellCtl = CHK_Shikyu_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 5].CellCtl = LBL_DEDAY_5;//IMT_PAYDT_5;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 6].CellCtl = BTN_Detail_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 6].CellCtl = BTN_Detail2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 6].CellCtl = IMC_KBN_6;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 6].CellCtl = CHK_EDICK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 6].CellCtl = CHK_Tekiyo_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 6].CellCtl = IMT_KAIDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 6].CellCtl = IMN_TEIKA_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 6].CellCtl = CHK_Sokujitu_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 6].CellCtl = IMN_MEMBR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 6].CellCtl = IMN_CLINT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 6].CellCtl = IMN_WEBPR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 6].CellCtl = IMN_WEBPR2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 6].CellCtl = IMT_REMAK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 6].CellCtl = CHK_Shikyu_6;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 6].CellCtl = IMT_ARIDT_6;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 6].CellCtl = LBL_DEDAY_6;//IMT_PAYDT_6;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 7].CellCtl = BTN_Detail_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 7].CellCtl = BTN_Detail2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 7].CellCtl = IMC_KBN_7;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 7].CellCtl = CHK_EDICK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 7].CellCtl = CHK_Tekiyo_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 7].CellCtl = IMT_KAIDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 7].CellCtl = IMN_TEIKA_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 7].CellCtl = CHK_Sokujitu_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 7].CellCtl = IMN_MEMBR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 7].CellCtl = IMN_CLINT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 7].CellCtl = IMN_WEBPR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 7].CellCtl = IMN_WEBPR2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 7].CellCtl = IMT_REMAK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 7].CellCtl = CHK_Shikyu_7;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 7].CellCtl = IMT_ARIDT_7;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 7].CellCtl = LBL_DEDAY_7;// IMT_PAYDT_7;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 8].CellCtl = BTN_Detail_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 8].CellCtl = BTN_Detail2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 8].CellCtl = IMC_KBN_8;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 8].CellCtl = CHK_EDICK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 8].CellCtl = CHK_Tekiyo_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 8].CellCtl = IMT_KAIDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 8].CellCtl = IMN_TEIKA_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 8].CellCtl = CHK_Sokujitu_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 8].CellCtl = IMN_MEMBR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 8].CellCtl = IMN_CLINT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 8].CellCtl = IMN_WEBPR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 8].CellCtl = IMN_WEBPR2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 8].CellCtl = IMT_REMAK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 8].CellCtl = CHK_Shikyu_8;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 8].CellCtl = IMT_ARIDT_8;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 8].CellCtl = LBL_DEDAY_8;// IMT_PAYDT_8;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnJuchu, 9].CellCtl = BTN_Detail_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.BtnDetail, 9].CellCtl = BTN_Detail2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, 9].CellCtl = IMC_KBN_9;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSyukka, 9].CellCtl = CHK_EDICK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.JuchuNo, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkTekiyo, 9].CellCtl = CHK_Tekiyo_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.PrintDate, 9].CellCtl = IMT_KAIDT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryAddress1, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.Space, 9].CellCtl = IMN_TEIKA_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkSokujitu, 9].CellCtl = CHK_Sokujitu_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.InstructionNO, 9].CellCtl = IMN_MEMBR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ShippingDate, 9].CellCtl = IMN_CLINT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentOutStore, 9].CellCtl = IMN_WEBPR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryName, 9].CellCtl = IMN_WEBPR2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CommentInStore, 9].CellCtl = IMT_REMAK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.ChkShikyu, 9].CellCtl = CHK_Shikyu_9;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DeliveryPlanDate, 9].CellCtl = IMT_ARIDT_9;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.DecidedDeliveryDate, 9].CellCtl = LBL_DEDAY_9;// IMT_PAYDT_9;    //支払予定日
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridShukka.Gen_MK_FocusMove.MvNxt, (int)ClsGridShukka.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridShukka.Gen_MK_FocusMove.MvPrv, (int)ClsGridShukka.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridShukka.Gen_MK_FocusMove.MvNxt, (int)ClsGridShukka.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridShukka.Gen_MK_FocusMove.MvSet, (int)ClsGridShukka.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridShukka.Gen_MK_FocusMove.MvSet, (int)ClsGridShukka.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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
                            case (int)ClsGridShukka.ColNO.GYONO:
                            case (int)ClsGridShukka.ColNO.Space:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }
                            case (int)ClsGridShukka.ColNO.DecidedDeliveryDate:
                            case (int)ClsGridShukka.ColNO.JuchuNo:
                            case (int)ClsGridShukka.ColNO.InstructionNO:
                            case (int)ClsGridShukka.ColNO.DeliveryAddress1:
                            case (int)ClsGridShukka.ColNO.DeliveryName:
                            case (int)ClsGridShukka.ColNO.PrintDate:
                            case (int)ClsGridShukka.ColNO.ShippingDate:
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
                    //    case  (int)ClsGridShukka.ColNO.ChkSyukka:
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

            ckM_Button1.Tag = "0";
            ckM_Button2.Tag = "0";
            ckM_Button3.Tag = "0";
            ckM_Button4.Tag = "0";

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

                                //DeliveryPlanNOがある場合、入力可（有効行）
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].DeliveryPlanNO))
                                {
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridShukka.ColNO.BtnJuchu:    // 
                                        case (int)ClsGridShukka.ColNO.BtnDetail:    //  
                                            //Form. 出荷指示種別(Hidden)＝２の場合は、クリックしても処理不要
                                            if (mGrid.g_DArray[w_Row].InstructionKBN.Equals(2))
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                            else
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                            break;

                                        case (int)ClsGridShukka.ColNO.ChkSyukka:    //
                                        case (int)ClsGridShukka.ColNO.ChkTekiyo:    // 
                                        case (int)ClsGridShukka.ColNO.ChkSokujitu:    // 
                                        case (int)ClsGridShukka.ColNO.ChkShikyu:    //  
                                        case (int)ClsGridShukka.ColNO.DeliveryPlanDate:    //入荷予定日
                                        case (int)ClsGridShukka.ColNO.CarrierName:    // 
                                        case (int)ClsGridShukka.ColNO.CommentInStore:    // 
                                        case (int)ClsGridShukka.ColNO.CommentOutStore:    // 
                                            {
                                                //出荷データが存在する時、入力不可
                                                if (mGrid.g_DArray[w_Row].Kbn.Equals(2))
                                                {
                                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                                }
                                                else
                                                {
                                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                }                                    
                                            }
                                            break;
                                        case (int)ClsGridShukka.ColNO.DecidedDeliveryDate:
                                            if (mGrid.g_DArray[w_Row].RedFont.Equals(1))
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ForeColor = Color.Red;
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                //mGrid.g_MK_State[w_Col, w_Row].Cell_Color = Color.Red;
                                            }
                                            else
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ForeColor = Color.Black;
                                                //mGrid.g_MK_State[w_Col, w_Row].Cell_Color = mGrid.F_GetBackColor_MK(w_Col, w_Row);
                                            }
                                            break;
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
                                SetFuncKeyAll(this, "100001000001");
                                IMT_DMY_0.Focus();
                                Scr_Lock(0, 0, 1);
                            }
                            else
                            {
                                SetFuncKeyAll(this, "100001000000");
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

            SetFuncKey(this, 8, false);
        }

        #endregion

        public ShukkaShijiTouroku()
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

                //起動時共通処理
                base.StartProgram();

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                ssbl = new ShukkaShijiTouroku_BL();
                CboStoreCD.Bind(ymd);
                ckM_ComboBox1.Bind(ymd);//

                for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
                {
                    CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridShukka.ColNO.CarrierName, W_CtlRow].CellCtl;
                    sctl.Bind(ymd);
                }

                Btn_F2.Text = "";
                Btn_F3.Text = "";
                Btn_F4.Text = "";
                Btn_F5.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F9.Text = "";
                Btn_F10.Text = "";
                
                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = ssbl.GetDate()
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
            detailControls = new Control[] { CboStoreCD, ckM_TextBox1 ,ckM_TextBox2 
                     ,ckM_CheckBox1,ckM_CheckBox2,ckM_CheckBox3,ckM_CheckBox4,ckM_CheckBox5
                     ,ckM_TextBox5, ChkHakkozumi,ChkSyukkazumi,ckM_TextBox18, ckM_ComboBox1
                         };
            detailLabels = new Control[] { };
            searchButtons = new Control[] { };

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }

        private bool SelectAndInsertExclusive(string orderNo)
        {
            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT)
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.SyukkaShiji,
                Number = orderNo,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                detailControls[(int)EIndex.CarrierCD].Focus();
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
        private void DeleteExclusive(DataTable dtForUpdate = null)
        {
            if (dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                mOldInstructionNO = "";
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    if (mOldInstructionNO != dr["InstructionNO"].ToString())
                    {
                        D_Exclusive_Entity de = new D_Exclusive_Entity
                        {
                            DataKBN = (int)Exclusive_BL.DataKbn.SyukkaShiji,
                            Number = dr["InstructionNO"].ToString()
                        };

                        ebl.D_Exclusive_Delete(de);

                        mOldInstructionNO = dr["InstructionNO"].ToString();
                    }
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
            DeleteExclusive(dtInstruction);

            //[D_Instruction_SelectData]
            die = GetEntity();

            //Errorでない場合、画面転送表01と画面転送表02をUNIONして、画面表示
            dtInstruction = ssbl.D_Instruction_SelectData(die);

            if (dtInstruction.Rows.Count == 0)
            {
                Scr_Clr(1);
                //該当データなし
                bbl.ShowMessage("E128");
                previousCtrl.Focus();
                return false;
            }
            else
            {
                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                m_dataCnt = 0;

                string ymd = bbl.GetDate();

                foreach (DataRow row in dtInstruction.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return false;
                    }

                    if (!string.IsNullOrWhiteSpace(row["InstructionNO"].ToString()) && mOldInstructionNO != row["InstructionNO"].ToString())
                    {
                        bool ret = SelectAndInsertExclusive(row["InstructionNO"].ToString());
                        if (!ret)
                            return false;

                        mOldInstructionNO = row["InstructionNO"].ToString();
                    }
                    if (row["DeliveryPlanDate"].ToString().Equals(ymd))
                        mGrid.g_DArray[i].RedFont = 1;
                    else
                        mGrid.g_DArray[i].RedFont = 0;

                    mGrid.g_DArray[i].DeliveryPlanDate = row["DeliveryPlanDate"].ToString();

                    //OntheDayFLG＝1の時、ON
                    if (bbl.Z_Set(row["OntheDayFLG"]) == 1)
                        mGrid.g_DArray[i].ChkSokujitu = true;
                    else
                        mGrid.g_DArray[i].ChkSokujitu = false;

                    //ExpressFLG ＝1の時、ON
                    if (bbl.Z_Set(row["ExpressFLG"]) == 1)
                        mGrid.g_DArray[i].ChkShikyu = true;
                    else
                        mGrid.g_DArray[i].ChkShikyu = false;

                    mGrid.g_DArray[i].DecidedDeliveryDate = row["DecidedDeliveryDate"].ToString();
                    mGrid.g_DArray[i].JuchuNo = row["Number"].ToString();
                    mGrid.g_DArray[i].DeliveryAddress1 = row["DeliveryAddress1"].ToString();   // 
                    mGrid.g_DArray[i].DeliveryName = row["DeliveryName"].ToString();
                    mGrid.g_DArray[i].CarrierName = row["CarrierCD"].ToString();   // 
                    mGrid.g_DArray[i].CommentInStore = row["CommentInStore"].ToString();   // 
                    mGrid.g_DArray[i].CommentOutStore = row["CommentOutStore"].ToString();   //
                    mGrid.g_DArray[i].ShippingDate = row["ShippingDate"].ToString();   // 
                    mGrid.g_DArray[i].PrintDate = row["PrintDate"].ToString();   // 
                    mGrid.g_DArray[i].InstructionNO = row["InstructionNO"].ToString();

                    //隠し項目
                    mGrid.g_DArray[i].InstructionKBN = Convert.ToInt16(row["InstructionKBN"]);
                    mGrid.g_DArray[i].DeliveryPlanNO = row["DeliveryPlanNO"].ToString();
                    mGrid.g_DArray[i].Kbn = Convert.ToInt16(row["KBN"]);
                    mGrid.g_DArray[i].Update = 0;

                    m_dataCnt = i + 1;
                    i++;
                }

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            S_BodySeigyo(1, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            S_BodySeigyo(1, 0);

            return true;
        }
        protected override void ExecDisp()
        {
            //Form.出荷予定日(From)～Form.出荷先名
            for (int i = 0; i < (int)EIndex.DeliveryName; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            //Form.通常(右記以外)～Form.店舗間移動														
            //CheckBox＝ON				
            //がひとつも無ければ、エラー									
            //通常(右記以外)CheckBoxへ	            
            if (ckM_CheckBox1.Checked || ckM_CheckBox2.Checked || ckM_CheckBox3.Checked || ckM_CheckBox4.Checked || ckM_CheckBox5.Checked)
            {

            }
            else {
                //Ｅ１８０
                bbl.ShowMessage("E111");
                detailControls[(int)EIndex.Chk1].Focus();
                return;
            }

            bool ret = CheckData(true);
            if (ret)
            {
                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridShukka.ColNO.ChkSyukka);
            }
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

                case (int)EIndex.PlanDateFrom:
                case (int)EIndex.PlanDateTo:
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

                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.PlanDateTo)
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
                    break;

                case (int)EIndex.DeliveryPlanDate:
                    //入力無くても良い(It is not necessary to input)
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

                case (int)EIndex.CarrierCD:

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

            w_Ctrl = detailControls[(int)EIndex.CarrierCD];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridShukka.Gen_MK_FocusMove.MvSet, (int)ClsGridShukka.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false)
        {
            string fmtYmd = "";

            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridShukka.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                        return false;
                }
            }

            switch (col)
            {
                case (int)ClsGridShukka.ColNO.DeliveryPlanDate:
                    string val = mGrid.g_DArray[row].DeliveryPlanDate;

                    //Form.Detail.出荷CheckBox＝ONの時、
                    if (mGrid.g_DArray[row].ChkSyukka)
                    {
                        //入力必須(Entry required)
                        if (string.IsNullOrWhiteSpace(val))
                        {
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(val))
                        {
                            return true;
                        }
                    }

                    fmtYmd = bbl.FormatDate(val);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(fmtYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    mGrid.g_DArray[row].DeliveryPlanDate = fmtYmd;
                    break;

                case (int)ClsGridShukka.ColNO.CarrierName:
                    //Form.Detail.出荷CheckBox＝ONの時、
                    if (mGrid.g_DArray[row].ChkSyukka)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].CarrierName) || mGrid.g_DArray[row].CarrierName.Equals("-1"))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
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
        private D_Instruction_Entity GetEntity()
        {
            die = new D_Instruction_Entity
            {
                DeliveryPlanDateFrom = detailControls[(int)EIndex.PlanDateFrom].Text,
                DeliveryPlanDateTo = detailControls[(int)EIndex.PlanDateTo].Text,
                DeliveryName = detailControls[(int)EIndex.DeliveryName].Text.Replace("/", ""),
                StoreCD = CboStoreCD.SelectedValue.ToString(),

            };

            if (ckM_CheckBox1.Checked)
                die.Chk1 = 1;
            else
                die.Chk1 = 0;

            if (ckM_CheckBox2.Checked)
                die.Chk2 = 1;
            else
                die.Chk2 = 0;

            if (ckM_CheckBox3.Checked)
                die.Chk3 = 1;
            else
                die.Chk3 = 0;

            if (ckM_CheckBox4.Checked)
                die.Chk4 = 1;
            else
                die.Chk4 = 0;

            if (ckM_CheckBox5.Checked)
                die.Chk5 = 1;
            else
                die.Chk5 = 0;

            if (ChkHakkozumi.Checked)
                die.ChkHakkozumi = 1;
            else
                die.ChkHakkozumi = 0;

            if (ChkSyukkazumi.Checked)
                die.ChkSyukkazumi = 1;
            else
                die.ChkSyukkazumi = 0;

            return die;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("InstructionNO", typeof(string));
            dt.Columns.Add("InstructionKBN", typeof(int));
            dt.Columns.Add("DeliveryPlanNO", typeof(string));
            dt.Columns.Add("GyoNO", typeof(int));
            dt.Columns.Add("DeliveryPlanDate", typeof(DateTime));
            dt.Columns.Add("CarrierCD", typeof(string));
            dt.Columns.Add("CommentOutStore", typeof(string));
            dt.Columns.Add("CommentInStore", typeof(string));
            dt.Columns.Add("OntheDayFLG", typeof(int));
            dt.Columns.Add("ExpressFLG", typeof(int));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (mGrid.g_DArray[RW].ChkSyukka)
                {
                    dt.Rows.Add(mGrid.g_DArray[RW].InstructionNO == "" ? null : mGrid.g_DArray[RW].InstructionNO
                         , bbl.Z_Set(mGrid.g_DArray[RW].InstructionKBN)
                         , mGrid.g_DArray[RW].DeliveryPlanNO
                         , rowNo //mGrid.g_DArray[RW].GYONO
                         , mGrid.g_DArray[RW].DeliveryPlanDate == "" ? null : mGrid.g_DArray[RW].DeliveryPlanDate
                         , mGrid.g_DArray[RW].CarrierName == "" ? null : mGrid.g_DArray[RW].CarrierName
                         , mGrid.g_DArray[RW].CommentOutStore == "" ? null : mGrid.g_DArray[RW].CommentOutStore
                         , mGrid.g_DArray[RW].CommentInStore == "" ? null : mGrid.g_DArray[RW].CommentInStore
                         , mGrid.g_DArray[RW].ChkSokujitu ? 1 : 0
                         , mGrid.g_DArray[RW].ChkShikyu ? 1 : 0
                         , mGrid.g_DArray[RW].Update
                         );

                    rowNo++;
                }
            }

            return dt;
        }

        protected override void ExecSec()
        {

            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //Form.Detail.出荷CheckBox＝ONの行のエラーチェック
                if (mGrid.g_DArray[RW].ChkSyukka)
                {
                    for (int CL = (int)ClsGridShukka.ColNO.DeliveryPlanDate; CL < (int)ClsGridShukka.ColNO.COUNT; CL++)
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

            DataTable dt = GetGridEntity();

            //明細が1件も登録されていなければ、エラー Ｅ１８９
            if (dt.Rows.Count == 0)
            {
                //更新対象なし
                bbl.ShowMessage("E189");
                return;
            }

            die.StoreCD = CboStoreCD.SelectedValue.ToString();
            //die.OrderCD = detailControls[(int)EIndex.OrderCD].Text;
            die.InsertOperator = InOperatorCD;
            die.PC = InPcID;

            //更新処理
            ssbl.D_Instruction_Exec(die, dt);

            bbl.ShowMessage("I101");

            //更新後画面クリア
            ChangeOperationMode(OperationMode);
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //排他処理を解除
            DeleteExclusive(dtInstruction);

            Scr_Clr(0);

            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            S_BodySeigyo(0, 0);

            detailControls[(int)EIndex.PlanDateFrom].Focus();

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

                foreach (Control ctl in detailLabels)
                {
                    ((CKM_SearchControl)ctl).LabelText = "";
                }

            }

            S_Clear_Grid();   //画面クリア（明細部）

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

                case 7://F8:行追加
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
                DeleteExclusive(dtInstruction);
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

                        if (index == (int)EIndex.DeliveryName) //取込日
                            btnSubF11.Focus();

                        else if (index == (int)EIndex.COUNT - 1)
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

                    if (CL == (int)ClsGridShukka.ColNO.DeliveryName)
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
        /// 明細部チェックボックスクリック時処理
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



            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
    
        /// <summary>
        /// 明細部受注ボタンクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Juchu_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                //「受注入力」を照会モードで起動（引数：該当行の受注番号）
                string juchuNo = mGrid.g_DArray[w_Row].JuchuNo;

                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + JuchuuNyuuryoku;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + juchuNo;
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
        private void Btn_Shukkka_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheck(true, (int)ClsGridShukka.ColNO.ChkSyukka);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void Btn_Tekiyo_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheck(true, (int)ClsGridShukka.ColNO.ChkTekiyo);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void Btn_Sokujitsu_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheck(true, (int)ClsGridShukka.ColNO.ChkSokujitu);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void Btn_Shikyu_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeCheck(true, (int)ClsGridShukka.ColNO.ChkShikyu);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void BTN_Detail_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                //画面転送表03に従い、第2画面を起動
                FrmShukkaShiji frm = new FrmShukkaShiji(ProID);
                frm.DeliveryPlanNO = mGrid.g_DArray[w_Row].DeliveryPlanNO;
                frm.JuchuNO = mGrid.g_DArray[w_Row].JuchuNo;

                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void Btn_Hanei_Click(object sender, EventArgs e)
        {
            try
            {
                    //出荷予定日のチェック
                bool ret =    CheckDetail((int)EIndex.DeliveryPlanDate);
                if (!ret)
                {
                    detailControls[(int)EIndex.DeliveryPlanDate].Focus();
                    return;
                }
                    ChangeCheck(true);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        #endregion

        private void ChangeCheck(bool check, int index = 0)
        {
            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                switch (index)
                {
                    case 0:
                    if (mGrid.g_DArray[RW].ChkTekiyo)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.DeliveryPlanDate].Text))
                            mGrid.g_DArray[RW].DeliveryPlanDate = detailControls[(int)EIndex.DeliveryPlanDate].Text;
                        if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.CarrierCD].Text))
                            mGrid.g_DArray[RW].CarrierName = ckM_ComboBox1.SelectedValue.ToString();
                    }
                        break;

                    case (int)ClsGridShukka.ColNO.ChkSyukka:
                        if (mGrid.g_MK_State[index, RW].Cell_Enabled)
                            mGrid.g_DArray[RW].ChkSyukka = ckM_Button1.Tag.Equals("0") ? true : false;

                        break;
                    case (int)ClsGridShukka.ColNO.ChkTekiyo:
                        if (mGrid.g_MK_State[index, RW].Cell_Enabled)
                            mGrid.g_DArray[RW].ChkTekiyo = ckM_Button2.Tag.Equals("0") ? true : false;
                        break;
                    case (int)ClsGridShukka.ColNO.ChkSokujitu:
                        if (mGrid.g_MK_State[index, RW].Cell_Enabled)
                            mGrid.g_DArray[RW].ChkSokujitu = ckM_Button3.Tag.Equals("0") ? true : false;
                        break;
                    case (int)ClsGridShukka.ColNO.ChkShikyu:
                        if (mGrid.g_MK_State[index, RW].Cell_Enabled)
                            mGrid.g_DArray[RW].ChkShikyu = ckM_Button4.Tag.Equals("0") ? true : false;
                        break;
                    
                }
            }
            
            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //ボタンのSwitch　ON/OFF
            switch (index)
            {
                case (int)ClsGridShukka.ColNO.ChkSyukka:
                    ckM_Button1.Tag = ckM_Button1.Tag.Equals("0") ? "1" : "0";

                    break;
                case (int)ClsGridShukka.ColNO.ChkTekiyo:
                    ckM_Button2.Tag = ckM_Button2.Tag.Equals("0") ? "1" : "0";
                    break;
                case (int)ClsGridShukka.ColNO.ChkSokujitu:
                    ckM_Button3.Tag = ckM_Button3.Tag.Equals("0") ? "1" : "0";
                    break;
                case (int)ClsGridShukka.ColNO.ChkShikyu:
                    ckM_Button4.Tag = ckM_Button4.Tag.Equals("0") ? "1" : "0";
                    break;
            }
        }

    }
}








