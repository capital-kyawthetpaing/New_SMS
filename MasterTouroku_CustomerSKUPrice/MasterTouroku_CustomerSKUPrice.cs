using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using GridBase;


namespace MasterTouroku_CustomerSKUPrice
{
    public partial class FrmMasterTouroku_CustomerSKUPrice : FrmMainForm
    {

        private const string ProID = "MasterTouroku_CustomerSKUPrice";
        private const string ProNm = "顧客別SKU販売単価マスタ";

        private Control[] keyControls;
        private Control[] detailControls;
        private System.Windows.Forms.Control previousCtrl;
        ClsGridCustomerSKUPric mGrid = new ClsGridCustomerSKUPric();
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
        }

        private void FrmMasterTouroku_CustomerSKUPrice_Load(object sender, EventArgs e)
        {
            InProgramID = ProID;
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            txtStartDate.Focus();
           // S_SetInit_Grid();
            Clear(pnl_Body);
        }

        //private void S_SetInit_Grid()
        //{
        //    int W_CtlRow;

        //    if (ClsGridCustomerSKUPric.gc_P_GYO <= ClsGridCustomerSKUPric.gMxGyo)
        //    {
        //        mGrid.g_MK_Max_Row = ClsGridCustomerSKUPric.gMxGyo;
        //        m_EnableCnt = ClsGridCustomerSKUPric.gMxGyo;
        //    }
        //    mGrid.g_MK_Ctl_Row = ClsGridCustomerSKUPric.gc_P_GYO;
        //    mGrid.g_MK_Ctl_Col = ClsGridCustomerSKUPric.gc_MaxCL;

        //    mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;
        //    this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
        //    this.Vsb_Mei_0.SmallChange = 1;
        //    this.Vsb_Mei_0.Minimum = 0;
        //    this.Vsb_Mei_0.Maximum = mGrid.g_MK_MaxValue + this.Vsb_Mei_0.LargeChange - 1;
        //    S_SetControlArray();
        //    for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
        //    {
        //        for (int w_CtlCol = 0; w_CtlCol <= mGrid.g_MK_Ctl_Col - 1; w_CtlCol++)
        //        {
        //            if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl != null)
        //            {
        //                if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
        //                {
        //                    mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
        //                    //  mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
        //                    //  mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);//GridControl_Validated
        //                    //  mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Validated += new EventHandler(GridControl_Validated);

        //                }
        //            }

        //            switch (w_CtlCol)
        //            {
        //                case (int)ClsGridCustomerSKUPric.ColNO.CustomerCD:
        //                case (int)ClsGridCustomerSKUPric.ColNO.CustomerName:
        //                case (int)ClsGridCustomerSKUPric.ColNO.SKUCD:
        //                case (int)ClsGridCustomerSKUPric.ColNO.JANCD:
        //                case (int)ClsGridCustomerSKUPric.ColNO.StartChangeDate:    // 
        //                case (int)ClsGridCustomerSKUPric.ColNO.EndChangeDate:    // 
        //                case (int)ClsGridCustomerSKUPric.ColNO.UnitPrice:    // 
        //                case (int)ClsGridCustomerSKUPric.ColNO.StandardSalesUnitPrice:    // 
        //                case (int)ClsGridCustomerSKUPric.ColNO.Rank1UnitPrice:    // 
        //                case (int)ClsGridCustomerSKUPric.ColNO.Rank2UnitPrice:    // 
        //                case (int)ClsGridCustomerSKUPric.ColNO.Rank3UnitPrice:    // 
        //                case (int)ClsGridCustomerSKUPric.ColNO.Rank4UnitPrice:    // 
        //                case (int)ClsGridCustomerSKUPric.ColNO.Rank5UnitPrice:    // 
        //                case (int)ClsGridCustomerSKUPric.ColNO.ItemName:    //  
        //                case (int)ClsGridCustomerSKUPric.ColNO.CostUnitPrice:
        //                case (int)ClsGridCustomerSKUPric.ColNO.Remarks:
        //                case (int)ClsGridCustomerSKUPric.ColNO.Space1:
        //                    //if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
        //                    //    ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).CKM_Reqired = true;

        //                    break;
        //            }
        //        }
        //    }
        //    // 明細部の状態(初期状態) をセット 
        //    //To be Proceed. . . . 
        //    //
        //    mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];
        //    // データ保持用配列の宣言
        //    mGrid.g_DArray = new ClsGridCustomerSKUPric.ST_DArray_Grid[mGrid.g_MK_Max_Row];

        //    SetMultiColNo();
        //    // 行の色
        //    // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
        //    mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
        //    mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色
        //    // フォーカス移動順(表示列も含めて、すべての列を設定する)
        //    mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];
        //    for (int i = (int)ClsGridCustomerSKUPric.ColNO.GYONO; i <= (int)ClsGridCustomerSKUPric.ColNO.COUNT - 1; i++)
        //    {
        //        mGrid.g_MK_FocusOrder[i] = i;
        //    }
        //    int tabindex = 1;

        //    // 項目の形式セット
        //    for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
        //    {
        //        for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridCustomerSKUPric.ColNO.COUNT; W_CtlCol++)
        //        {
        //            //switch (W_CtlCol)
        //            //{
        //            //    case (int)ClsGridCustomerSKUPric.ColNO.MitsumoriSuu:
        //            //        mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
        //            //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
        //            //        break;
        //            //    case (int)ClsGridCustomerSKUPric.ColNO.MitsumoriUnitPrice:
        //            //    case (int)ClsGridCustomerSKUPric.ColNO.CostUnitPrice:
        //            //        mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
        //            //        break;
        //            //    case (int)ClsGridCustomerSKUPric.ColNO.MitsumoriHontaiGaku:
        //            //    case (int)ClsGridCustomerSKUPric.ColNO.MitsumoriGaku:
        //            //    case (int)ClsGridCustomerSKUPric.ColNO.CostGaku:
        //            //    case (int)ClsGridCustomerSKUPric.ColNO.ProfitGaku:
        //            //        mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
        //            //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
        //            //        break;
        //            //}

        //            mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
        //            tabindex++;
        //        }
        //    }
        //    Set_GridTabStop(false);
        //    mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);

        //}

        //private void S_SetControlArray()
        //{
        //    mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);
        //    //Control1
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 0].CellCtl = SC_Customer_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 0].CellCtl = IMT_CUSTNM_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 0].CellCtl = IMT_SDate_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 0].CellCtl = IMT_EDate_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 0].CellCtl = SC_ITEM_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 0].CellCtl = IMT_ITMCD_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 0].CellCtl = IMT_PRICE_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 0].CellCtl = IMT_REMARK_0;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 0].CellCtl = Space_0;
        //    //Control2
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 1].CellCtl = SC_Customer_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 1].CellCtl = IMT_CUSTNM_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 1].CellCtl = IMT_SDate_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 1].CellCtl = IMT_EDate_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 1].CellCtl = SC_ITEM_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 1].CellCtl = IMT_ITMCD_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 1].CellCtl = IMT_PRICE_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 1].CellCtl = IMT_REMARK_1;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 1].CellCtl = Space_1;
        //    //Control3
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 2].CellCtl = SC_Customer_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 2].CellCtl = IMT_CUSTNM_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 2].CellCtl = IMT_SDate_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 2].CellCtl = IMT_EDate_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 2].CellCtl = SC_ITEM_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 2].CellCtl = IMT_ITMCD_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 2].CellCtl = IMT_PRICE_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 2].CellCtl = IMT_REMARK_2;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 2].CellCtl = Space_2;

        //    //Control4
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 3].CellCtl = SC_Customer_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 3].CellCtl = IMT_CUSTNM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 3].CellCtl = IMT_SDate_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 3].CellCtl = IMT_EDate_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 3].CellCtl = SC_ITEM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 3].CellCtl = IMT_PRICE_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 3].CellCtl = IMT_REMARK_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 3].CellCtl = Space_3;

        //    //Control5
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 3].CellCtl = SC_Customer_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 3].CellCtl = IMT_CUSTNM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 3].CellCtl = IMT_SDate_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 3].CellCtl = IMT_EDate_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 3].CellCtl = SC_ITEM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 3].CellCtl = IMT_PRICE_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 3].CellCtl = IMT_REMARK_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 3].CellCtl = Space_3;

        //    //Control6
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 3].CellCtl = SC_Customer_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 3].CellCtl = IMT_CUSTNM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 3].CellCtl = IMT_SDate_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 3].CellCtl = IMT_EDate_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 3].CellCtl = SC_ITEM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 3].CellCtl = IMT_PRICE_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 3].CellCtl = IMT_REMARK_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 3].CellCtl = Space_3;

        //    //Control7
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 3].CellCtl = SC_Customer_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 3].CellCtl = IMT_CUSTNM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 3].CellCtl = IMT_SDate_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 3].CellCtl = IMT_EDate_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 3].CellCtl = SC_ITEM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 3].CellCtl = IMT_PRICE_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 3].CellCtl = IMT_REMARK_3;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 3].CellCtl = Space_3;

        //    //Control8
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 7].CellCtl = IMT_ITMCD_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 7].CellCtl = IMT_JANCD_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.StartChangeDate, 7].CellCtl = IMT_STADT_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.EndChangeDate, 7].CellCtl = IMT_ENDDT_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.UnitPrice, 7].CellCtl = IMN_UNITPRICE_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.StandardSalesUnitPrice, 7].CellCtl = IMN_SSUNITPRICE_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank1UnitPrice, 7].CellCtl = IMN_R1UNITPRICE_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank2UnitPrice, 7].CellCtl = IMN_R2UNITPRICE_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank3UnitPrice, 7].CellCtl = IMN_R3UNITPRICE_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank4UnitPrice, 7].CellCtl = IMN_R4UNITPRICE_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank5UnitPrice, 7].CellCtl = IMN_R5UNITPRICE_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.ItemName, 7].CellCtl = IMT_ITMNM_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CostUnitPrice, 7].CellCtl = IMN_COSTUNPRICE_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 7].CellCtl = IMT_REMARK_7;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 7].CellCtl = Space7;
        //    //Control9
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 8].CellCtl = IMT_ITMCD_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 8].CellCtl = IMT_JANCD_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.StartChangeDate, 8].CellCtl = IMT_STADT_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.EndChangeDate, 8].CellCtl = IMT_ENDDT_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.UnitPrice, 8].CellCtl = IMN_UNITPRICE_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.StandardSalesUnitPrice, 8].CellCtl = IMN_SSUNITPRICE_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank1UnitPrice, 8].CellCtl = IMN_R1UNITPRICE_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank2UnitPrice, 8].CellCtl = IMN_R2UNITPRICE_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank3UnitPrice, 8].CellCtl = IMN_R3UNITPRICE_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank4UnitPrice, 8].CellCtl = IMN_R4UNITPRICE_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Rank5UnitPrice, 8].CellCtl = IMN_R5UNITPRICE_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.ItemName, 8].CellCtl = IMT_ITMNM_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CostUnitPrice, 8].CellCtl = IMN_COSTUNPRICE_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 8].CellCtl = IMT_REMARK_8;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 8].CellCtl = Space8;
        //    //Control10
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 9].CellCtl = SC_Customer_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 9].CellCtl = IMT_CUSTNM_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 9].CellCtl = IMT_SDate_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 9].CellCtl = IMT_EDate_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 9].CellCtl = SC_ITEM_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 9].CellCtl = IMT_ITMCD_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 9].CellCtl = IMT_ITMNM_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 9].CellCtl = IMT_PRICE_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 9].CellCtl = IMT_REMARK_9;
        //    mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 9].CellCtl = Space_9;
        //}

    }
}
