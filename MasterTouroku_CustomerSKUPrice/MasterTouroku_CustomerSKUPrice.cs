using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;
using CKM_Controls;

namespace MasterTouroku_CustomerSKUPrice
{
    public partial class FrmMasterTouroku_CustomerSKUPrice : FrmMainForm
    {

        private const string ProID = "MasterTouroku_CustomerSKUPrice";
        private const string ProNm = "顧客別SKU販売単価マスタ";
        MasterTouroku_CustomerSKUPrice_BL mcskupbl;
        M_CustomerSKUPrice_Entity mcskue;

        //private Control[] keyControls;
        //private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        //private Control[] searchButtons;

        private System.Windows.Forms.Control previousCtrl;
        ClsGridCustomerSKUPric mGrid;
        private int m_EnableCnt;
        private int m_dataCnt = 0;
        public string[] DuplicateCheckCol = null;

        private enum EIndex : int
        {
            CustomerCD,
            CustomerName,
            TekiyouKaisiDate,
            TekiyouShuuryouDate,
            AdminNO,
            JANCD,
            SKUCD,
            SKUName,
            SalePriceOutTax,
            Remarks
        }

        public FrmMasterTouroku_CustomerSKUPrice()
        {
            InitializeComponent();
            mGrid = new ClsGridCustomerSKUPric();
            mcskupbl = new MasterTouroku_CustomerSKUPrice_BL();
            mcskue = new M_CustomerSKUPrice_Entity();
        }

        private void FrmMasterTouroku_CustomerSKUPrice_Load(object sender, EventArgs e)
        {
            InProgramID = ProID;
            SetFunctionLabel(EProMode.MENTE);

            //InitialControlArray();

           // S_SetInit_Grid();

            StartProgram();
            txtStartDate.Focus();
            
            //Clear(pnl_Body);
            //Scr_Clr(0);
        }

        //public override void FunctionProcess(int Index)
        //{
        //    base.FunctionProcess(Index);

        //    switch (Index)
        //    {
        //        case 0: // F1:終了
        //            {
        //                break;
        //            }
        //        case 1:     //F2:新規
        //        case 2:     //F3:変更
        //        case 3:     //F4:削除
        //        case 4:     //F5:照会
        //            {
        //                ChangeOperationMode((EOperationMode)Index);

        //                break;
        //            }
        //        case 5: //F6:キャンセル
        //            {
        //                //Ｑ００４				
        //                if (bbl.ShowMessage("Q004") != DialogResult.Yes)
        //                    return;

        //                ChangeOperationMode(base.OperationMode);

        //                break;
        //            }
        //        case 6://F7:行削除
        //           // ADD_SUB();
        //            break;

        //        case 7://F8:行追加
        //            //DEL_SUB();
        //            break;

        //        case 9://F10複写
        //            //CPY_SUB();
        //            break;

        //        //case 8: //F9:検索
        //        //    EsearchKbn kbn = EsearchKbn.Null;
        //        //    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
        //        //    {
        //        //        return;
        //        //    }

        //        //    if (w_Col == (int)ClsGridMitsumori.ColNO.JanCD)
        //        //        //商品検索
        //        //        kbn = EsearchKbn.Product;
                    

        //        //    if (kbn != EsearchKbn.Null)
        //        //        SearchData(kbn, previousCtrl);

        //        //    break;

        //        case 11:    //F12:登録
        //            {
        //                if (OperationMode == EOperationMode.DELETE)
        //                { //Ｑ１０２		
        //                    if (bbl.ShowMessage("Q102") != DialogResult.Yes)
        //                        return;
        //                }
        //                else
        //                {
        //                    //Ｑ１０１		
        //                    if (bbl.ShowMessage("Q101") != DialogResult.Yes)
        //                        return;
        //                }


        //                this.ExecSec();
        //                break;
        //            }
        //    }   //switch end

        //}

        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //排他処理を解除
            //DeleteExclusive();

            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            switch (mode)
            {
                case EOperationMode.INSERT:
                    //ScStaff.TxtCode.Text = InOperatorCD;
                    //ScStaff.LabelText = InOperatorName;
                    //CboStoreCD.SelectedValue = StoreCD;
                    //SetInitStoreInfo(StoreCD);

                    detailControls[0].Focus();
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    //keyControls[0].Focus();
                    break;

            }

        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { txtStartDate, txtEndDate, ScCustomer_Start.TxtCode,ScCustomer_End.TxtCode, ScSKUCD_Start.TxtCode,ScSKUCD_End.TxtCode,txtItemName };

            
            //foreach (Control ctl in detailControls)
            //{
            //    ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
            //    ctl.Enter += new System.EventHandler(DetailControl_Enter);
            //}

            //rdoRecent.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
            //rdoAll.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);

            //rdoRecent.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            //rdoAll.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
        }

        private void S_SetInit_Grid()
        {
            int W_CtlRow;

            if (ClsGridCustomerSKUPric.gc_P_GYO <= ClsGridCustomerSKUPric.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridCustomerSKUPric.gMxGyo;
                m_EnableCnt = ClsGridCustomerSKUPric.gMxGyo;
            }
            mGrid.g_MK_Ctl_Row = ClsGridCustomerSKUPric.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridCustomerSKUPric.gc_MaxCL;

            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;
            this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_0.SmallChange = 1;
            this.Vsb_Mei_0.Minimum = 0;
            this.Vsb_Mei_0.Maximum = mGrid.g_MK_MaxValue + this.Vsb_Mei_0.LargeChange - 1;
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
                        if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                        {
                            //mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);

                        }
                    }

                    //switch (w_CtlCol)
                    //{
                    //    case (int)ClsGridCustomerSKUPric.ColNO.CustomerCD:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.CustomerName:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate:    // 
                    //    case (int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.JANCD:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.SKUCD:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.SKUName:    // 
                    //    case (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.Remarks:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.Space1:


                    //        break;
                    //}
                }
            }
            // 明細部の状態(初期状態) をセット 
            //To be Proceed. . . . 
            //
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];
            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridCustomerSKUPric.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            SetMultiColNo();
            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色
            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];
            for (int i = (int)ClsGridCustomerSKUPric.ColNO.GYONO; i <= (int)ClsGridCustomerSKUPric.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }
            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridCustomerSKUPric.ColNO.COUNT; W_CtlCol++)
                {
                    //switch (W_CtlCol)
                    //{
                    //    case (int)ClsGridCustomerSKUPric.ColNO.MitsumoriSuu:
                    //        mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                    //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                    //        break;
                    //    case (int)ClsGridCustomerSKUPric.ColNO.MitsumoriUnitPrice:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.CostUnitPrice:
                    //        mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                    //        break;
                    //    case (int)ClsGridCustomerSKUPric.ColNO.MitsumoriHontaiGaku:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.MitsumoriGaku:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.CostGaku:
                    //    case (int)ClsGridCustomerSKUPric.ColNO.ProfitGaku:
                    //        mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                    //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                    //        break;
                    //}

                    mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }
            Set_GridTabStop(false);
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);

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
                        case "IMT_SDate":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate;
                            break;
                        case "IMT_EDate":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate;
                            break;
                        case "SC_Customer":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.CustomerCD;
                            break;
                        case "IMT_REMARK":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.Remarks;
                            break;
                        case "SC_ITEM":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.JANCD;
                            break;
                        case "IMT_PRICE":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax;
                            if (w_Row == mGrid.g_MK_Max_Row - 1)
                                lastCell = true;
                            break;
                    }

                    //bool changeFlg = false;
                    //switch (CL)
                    //{
                    //    case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:
                    //        if (!mGrid.g_DArray[w_Row].MitsumoriSuu.Equals(w_ActCtl.Text))
                    //        {
                    //            changeFlg = true;
                    //        }
                    //        break;

                    //    case (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice: //販売単価 
                    //        if (!mGrid.g_DArray[w_Row].MitsumoriUnitPrice.Equals(w_ActCtl.Text))
                    //        {
                    //            changeFlg = true;
                    //        }
                    //        break;

                    //    case (int)ClsGridMitsumori.ColNO.CostUnitPrice: //原価単価
                    //        if (!mGrid.g_DArray[w_Row].CostUnitPrice.Equals(w_ActCtl.Text))
                    //        {
                    //            changeFlg = true;
                    //        }
                    //        break;
                    //}

                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    //手入力時金額を再計算
                    //if (changeFlg)
                    //{
                    //    decimal wSuu = bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriSuu);
                    //    string ymd = detailControls[(int)EIndex.MitsumoriDate].Text;

                    //    switch (CL)
                    //    {
                    //        case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:
                    //            {
                    //                //数量が整数値かどうかチェック
                    //                int intSu;
                    //                if (!int.TryParse(wSuu.ToString(), out intSu))
                    //                {
                    //                    bbl.ShowMessage("E102");
                    //                    return;
                    //                }

                    //                if (wSuu != 0)
                    //                {
                    //                    //Function_単価取得.
                    //                    Fnc_UnitPrice_Entity fue = new Fnc_UnitPrice_Entity
                    //                    {
                    //                        AdminNo = mGrid.g_DArray[w_Row].AdminNO,
                    //                        ChangeDate = ymd,
                    //                        CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                    //                        StoreCD = CboStoreCD.SelectedValue.ToString(),
                    //                        SaleKbn = "0",
                    //                        Suryo = wSuu.ToString()
                    //                    };

                    //                    bool ret = bbl.Fnc_UnitPrice(fue);
                    //                    if (ret)
                    //                    {
                    //                        //数量変更時も単価再計算
                    //                        switch (mTennic)
                    //                        {
                    //                            case 0:
                    //                                //販売単価=Function_単価取得.out税込単価		
                    //                                mGrid.g_DArray[w_Row].MitsumoriUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeikomiTanka));
                    //                                break;
                    //                            case 1:
                    //                                //販売単価=Function_単価取得.out税抜単価
                    //                                mGrid.g_DArray[w_Row].MitsumoriUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeinukiTanka));
                    //                                break;
                    //                        }

                    //                        //原価単価=Function_単価取得.out原価単価	
                    //                        mGrid.g_DArray[w_Row].CostUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka));
                    //                    }
                    //                    else
                    //                    {
                    //                        //販売単価		
                    //                        mGrid.g_DArray[w_Row].MitsumoriUnitPrice = "0";
                    //                        //原価単価
                    //                        mGrid.g_DArray[w_Row].CostUnitPrice = "0";
                    //                    }

                    //                    SetMitsumoriGaku(w_Row, wSuu, ymd, fue.ZeinukiTanka);
                    //                }

                    //                //原価額←Function_単価取得.out原価単価×Form.Detail.見積数
                    //                mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].CostUnitPrice) * wSuu);

                    //                CalcZei(w_Row);
                    //            }
                    //            break;

                    //        case (int)ClsGridMitsumori.ColNO.MitsumoriUnitPrice: //販売単価 
                    //            {
                    //                string tanka = mGrid.g_DArray[w_Row].MitsumoriUnitPrice;
                    //                if (mTennic.Equals(0))
                    //                    tanka = bbl.GetZeinukiKingaku(bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriUnitPrice), mGrid.g_DArray[w_Row].TaxRateFLG, ymd).ToString();

                    //                SetMitsumoriGaku(w_Row, wSuu, ymd, tanka);

                    //                CalcZei(w_Row, true);
                    //            }
                    //            break;

                    //        case (int)ClsGridMitsumori.ColNO.CostUnitPrice: //原価単価
                    //            //原価額=Form.Detail.原価単価×	Form.Detail.見積数
                    //            mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].CostUnitPrice) * bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriSuu));

                    //            //粗利額
                    //            mGrid.g_DArray[w_Row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].MitsumoriHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].CostGaku)); // 

                    //            break;
                    //    }
                    //}

                    if (CL == -1)
                        return;

                //    //チェック処理
                //    if (CheckGrid(CL, w_Row) == false)
                //    {
                //        //配列の内容を画面へセット
                //        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                //        //Focusセット処理
                //        w_ActCtl.Focus();
                //        return;
                //    }

                //    if (lastCell)
                //    {
                //        w_ActCtl.Focus();
                //        return;
                //    }

                //    //あたかもTabキーが押されたかのようにする
                //    //Shiftが押されている時は前のコントロールのフォーカスを移動
                //    //this.ProcessTabKey(!e.Shift);
                //    //行き先がなかったら移動しない
                //    S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                //}
                //else if (e.KeyCode == Keys.Tab)
                //{
                //    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                //    {
                //        return;
                //    }

                //    if (CL == (int)ClsGridCustomerSKUPric.ColNO.NotPrintFLG)
                //        S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void Scr_Clr(short Kbn)  /// 0 is initial state
        {
            var ctrl = GetAllControls(pannel_Header);
            if (Kbn == 0)
            {
                foreach (Control ctl in ctrl)
                {
                    if (ctrl is CKM_TextBox ct)
                    {
                        ct.Text = "";
                    }
                    else if (ctrl is CKM_SearchControl c)
                    {
                        c.TxtChangeDate.Text = "";
                        c.TxtCode.Text = "";
                        c.LabelText = "";
                    }
                    else if (ctrl is CKM_RadioButton cr && cr.Name == "rdoRecent")
                    {
                        cr.Checked = true;
                    }
                }
            }
            //S_Clear_Grid();   //画面クリア（明細部）
        }

        private void SetMultiColNo(DataTable dt = null)
        {
            if (dt == null)
            {
                for (int w_Row = 0; w_Row < 999; w_Row++)
                {
                    mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();
                    //mGrid.g_DArray[w_Row].Rank1UnitPrice = "0";
                }
            }
            else  // set Data from db
            {
                int c = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    mGrid.g_DArray[c].CustomerCD = dr["CustomerCD"].ToString();
                    mGrid.g_DArray[c].CustomerName = dr["CustomerName"].ToString();
                    mGrid.g_DArray[c].TekiyouKaisiDate = dr["TekiyouKaisiDate"].ToString();
                    mGrid.g_DArray[c].TekiyouShuuryouDate = dr["TekiyouShuuryouDate"].ToString();
                    mGrid.g_DArray[c].AdminNO = dr["AdminNo"].ToString();
                    mGrid.g_DArray[c].JANCD = dr["JanCD"].ToString();
                    mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                    mGrid.g_DArray[c].SKUName = dr["SKUName"].ToString(); 
                    mGrid.g_DArray[c].SalePriceOutTax = dr["SalePriceOutTax"].ToString();
                    mGrid.g_DArray[c].Remarks = dr["Remarks"].ToString();
                    c++;
                }
            }
        }

        private void GridControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                //   PreViousColor = ActiveControl.BackColor;
                int w_Row;
                CKM_Controls.CKM_TextBox w_ActCtl;

                w_ActCtl = (CKM_Controls.CKM_TextBox)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                // 背景色m
                // w_ActCtl.BackColor = ClsGridBase.BKColor;  ---PTK

                Grid_Gotfocus((int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));


                //w_ActCtl.Name
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Gotfocus(int pCol, int pRow, int pCtlRow)
        {
            bool W_Del = false;
            bool W_Ret;

            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            //mGrid.g_MK_Ctrl(ClsGridHanbaiTanka.ColNO.DELCK, pCtlRow).GVal(W_Del);

            // ﾌｧﾝｸｼｮﾝﾎﾞﾀﾝ使用可否
            //SetFuncKeyAll(this, "111111000001");  おそらく再度設定不要

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

        private void Set_GridTabStop(bool pTabStop)
        {
            int w_Row;
            int w_Col;
            int w_CtlRow;
            try
            {
                // 画面上にEnable=Trueのコントロールがひとつしかない時はTab,Shift+Tab,↑,↓押下時にKeyExitが発生しないため、
                // 明細がスクロールしなくなる。回避策として、そのパターンが起こりうる時のみ、IMT_DMY_0.TabStopをTrueにする。
                //if (OperationMode == EOperationMode.UPDATE)
                //    txtStartDateFrom.TabStop = pTabStop;
                //else
                //    txtStartDateFrom.TabStop = false;

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
                return;
            }
        }
        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);
            //Control1
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 0].CellCtl = SC_Customer_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 0].CellCtl = IMT_CUSTNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 0].CellCtl = IMT_SDate_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 0].CellCtl = IMT_EDate_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 0].CellCtl = SC_ITEM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 0].CellCtl = IMT_PRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 0].CellCtl = IMT_REMARK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 0].CellCtl = Space_0;
            //Control2
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 1].CellCtl = SC_Customer_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 1].CellCtl = IMT_CUSTNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 1].CellCtl = IMT_SDate_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 1].CellCtl = IMT_EDate_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 1].CellCtl = SC_ITEM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 1].CellCtl = IMT_PRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 1].CellCtl = IMT_REMARK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 1].CellCtl = Space_1;
            //Control3
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 2].CellCtl = SC_Customer_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 2].CellCtl = IMT_CUSTNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 2].CellCtl = IMT_SDate_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 2].CellCtl = IMT_EDate_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 2].CellCtl = SC_ITEM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 2].CellCtl = IMT_PRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 2].CellCtl = IMT_REMARK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 2].CellCtl = Space_2;

            //Control4
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 3].CellCtl = SC_Customer_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 3].CellCtl = IMT_CUSTNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 3].CellCtl = IMT_SDate_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 3].CellCtl = IMT_EDate_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 3].CellCtl = SC_ITEM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 3].CellCtl = IMT_PRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 3].CellCtl = IMT_REMARK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 3].CellCtl = Space_3;

            //Control5
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 4].CellCtl = SC_Customer_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 4].CellCtl = IMT_CUSTNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 4].CellCtl = IMT_SDate_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 4].CellCtl = IMT_EDate_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 4].CellCtl = SC_ITEM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 4].CellCtl = IMT_PRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 4].CellCtl = IMT_REMARK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 4].CellCtl = Space_4;

            //Control6
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 5].CellCtl = SC_Customer_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 5].CellCtl = IMT_CUSTNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 5].CellCtl = IMT_SDate_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 5].CellCtl = IMT_EDate_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 5].CellCtl = SC_ITEM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 5].CellCtl = IMT_PRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 5].CellCtl = IMT_REMARK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 5].CellCtl = Space_5;

            //Control7
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 6].CellCtl = SC_Customer_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 6].CellCtl = IMT_CUSTNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 6].CellCtl = IMT_SDate_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 6].CellCtl = IMT_EDate_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 6].CellCtl = SC_ITEM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 6].CellCtl = IMT_PRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 6].CellCtl = IMT_REMARK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 6].CellCtl = Space_6;

            //Control8
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 7].CellCtl = SC_Customer_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 7].CellCtl = IMT_CUSTNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 7].CellCtl = IMT_SDate_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 7].CellCtl = IMT_EDate_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 7].CellCtl = SC_ITEM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 7].CellCtl = IMT_PRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 7].CellCtl = IMT_REMARK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 7].CellCtl = Space_7;

            //Control9
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 8].CellCtl = SC_Customer_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 8].CellCtl = IMT_CUSTNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 8].CellCtl = IMT_SDate_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 8].CellCtl = IMT_EDate_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 8].CellCtl = SC_ITEM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 8].CellCtl = IMT_PRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 8].CellCtl = IMT_REMARK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 8].CellCtl = Space_8;
            //Control10
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 9].CellCtl = SC_Customer_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 9].CellCtl = IMT_CUSTNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 9].CellCtl = IMT_SDate_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 9].CellCtl = IMT_EDate_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 9].CellCtl = SC_ITEM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 9].CellCtl = IMT_PRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 9].CellCtl = IMT_REMARK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 9].CellCtl = Space_9;
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
                    if (w_Row > m_EnableCnt - 1)
                    {
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.MaxGyoColor;
                    }
                }
            }
            Array.Clear(mGrid.g_DArray, mGrid.g_DArray.GetLowerBound(0), mGrid.g_DArray.GetLength(0));

            // 行番号の初期値セット
            for (w_Row = 0; w_Row <= mGrid.g_MK_Max_Row - 1; w_Row++)
                mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();

            // 画面クリア
            mGrid.S_DispFromArray(0, ref this.Vsb_Mei_0);
        }

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
                                        case (int)ClsGridCustomerSKUPric.ColNO.CustomerCD:
                                        case (int)ClsGridCustomerSKUPric.ColNO.CustomerName:
                                        case (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate:    // 
                                        case (int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate:
                                        case (int)ClsGridCustomerSKUPric.ColNO.JANCD:
                                        case (int)ClsGridCustomerSKUPric.ColNO.SKUCD:
                                        case (int)ClsGridCustomerSKUPric.ColNO.SKUName:    // 
                                        case (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax:
                                        case (int)ClsGridCustomerSKUPric.ColNO.Remarks:
                                        case (int)ClsGridCustomerSKUPric.ColNO.Space1:
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
                            //IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);

                                //Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                //detailControls[(int)EIndex.MitsumoriDate].Text = bbl.GetDate();
                                F9Visible = false;

                                SetFuncKeyAll(this, "111111001001");
                            }
                            else
                            {
                                //keyControls[(int)EIndex.MitsumoriNO].Enabled = true;
                                //ScMitsumoriNO.BtnSearch.Enabled = true;
                                //keyControls[(int)EIndex.CopyMitsumoriNO].Text = "";
                                //keyControls[(int)EIndex.CopyMitsumoriNO].Enabled = false;
                                //ScCopyMitsumoriNO.BtnSearch.Enabled = false;

                                //Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                //this.Vsb_Mei_0.TabStop = false;

                                SetFuncKeyAll(this, "111111001010");
                            }

                            //SetEnabled(false);

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
                                        case (int)ClsGridCustomerSKUPric.ColNO.CustomerCD:
                                        case (int)ClsGridCustomerSKUPric.ColNO.CustomerName:
                                        case (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate:    // 
                                        case (int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate:
                                        case (int)ClsGridCustomerSKUPric.ColNO.JANCD:
                                        case (int)ClsGridCustomerSKUPric.ColNO.SKUCD:
                                        case (int)ClsGridCustomerSKUPric.ColNO.SKUName:    // 
                                        case (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax:
                                        case (int)ClsGridCustomerSKUPric.ColNO.Remarks:
                                        case (int)ClsGridCustomerSKUPric.ColNO.Space1:
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;

                                                break;
                                            }
                                        //case (int)ClsGridMitsumori.ColNO.MitsumoriSuu:
                                            //if (mGrid.g_DArray[w_Row].DiscountKbn == 1)
                                            //{
                                            //    mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                            //    mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = true;
                                            //}
                                            //else
                                            //{
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = false;
                                            //}
                                            break;
                                        //case (int)ClsGridMitsumori.ColNO.SKUName:
                                        //case (int)ClsGridMitsumori.ColNO.CostUnitPrice:
                                        //    if (mGrid.g_DArray[w_Row].VariousFLG == 1)
                                        //    {
                                        //        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                        //        mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = false;
                                        //    }
                                        //    else
                                        //    {
                                        //        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                        //        mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = true;
                                        //    }
                                        //    break;
                                    }
                                }
                                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                            }
                        }
                        else
                        {
                            //IMT_DMY_0.Focus();

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
                            pnl_Body.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            //Scr_Lock(0, 0, 0);
                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000011");
                                //IMT_DMY_0.Focus();
                                Scr_Lock(0, 0, 1);
                            }
                            else
                            {
                                //CboStoreCD.Enabled = false;
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

        private void Scr_Lock(short no1, short no2, short Kbn)
        {
            short i;
            for (i = no1; i <= no2; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            //// ｷｰ部
                            //foreach (Control ctl in keyControls)
                            //{
                            //    ctl.Enabled = Kbn == 0 ? true : false;
                            //}
                            //ScMitsumoriNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
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
                            //ScCustomer.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            //btnAddress.Enabled = Kbn == 0 ? true : false;

                            break;
                        }
                }
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
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

        private void btnDisplay_Click(object sender, EventArgs e)
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

        protected override void ExecDisp()
        {
            if (OperationMode != EOperationMode.INSERT)
            {
                if(ErrorCheck(11))
                {
                    mcskue = GetSKUPriceInfo();
                    DataTable dtSKUPrice = mcskupbl.M_CustomerSKUPriceSelectData(mcskue);
                   
                    if (dtSKUPrice.Rows.Count > 0)
                    {
                        SetMultiColNo(dtSKUPrice);
                    }
                    else
                    {
                        bbl.ShowMessage("E128");
                        Scr_Clr(1);
                        txtStartDate.Focus();
                    }

                }
            }
                CheckData(true);
        }

        public void CheckData(bool set, bool copy= false)
        {
            
        }

        public bool ErrorCheck(int mode)
        {
            if (!txtStartDate.DateCheck())
                return false;
            if (!txtEndDate.DateCheck())
                return false;

            int result = txtStartDate.Text.CompareTo(txtEndDate.Text);
            if (result > 0)
            {
                bbl.ShowMessage("E104");
                txtStartDate.Focus();
                return false;
            }

            int result1 = ScCustomer_Start.TxtCode.Text.CompareTo(ScCustomer_End.TxtCode.Text);
            if (result1 > 0)
            {
                bbl.ShowMessage("E104");
                ScCustomer_Start.SetFocus(1);
                return false;
            }

            int skuresult = ScSKUCD_Start.TxtCode.Text.CompareTo(ScSKUCD_End.TxtCode.Text);
            if (skuresult > 0)
            {
                bbl.ShowMessage("E104");
                ScSKUCD_Start.SetFocus(1);
                return false;
            }
            if(mode==2)
            {

            }
            return true;
        }

        public M_CustomerSKUPrice_Entity GetSKUPriceInfo()
        {
            mcskue = new M_CustomerSKUPrice_Entity
            {
                TekiyouKaisiDate_From = txtStartDate.Text,
                TekiyouKaisiDate_To = txtEndDate.Text,
                CustomerCD_From = ScCustomer_Start.TxtCode.Text,
                CustomerCD_To = ScCustomer_End.TxtCode.Text,
                SKUCD_From = ScSKUCD_Start.TxtCode.Text,
                SKUCD_To = ScSKUCD_End.TxtCode.Text,
                SKUName = txtItemName.Text,
                DisplayKBN = rdoRecent.Checked? "0" : "1",
            };
            return mcskue;
        }
     }
}
