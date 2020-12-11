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
namespace WMasterTouroku_HanbaiTankaTennic
{
    public partial class MasterTouroku_HanbaiTankaTennic : FrmMainForm
    {
        private const string ProID = "MasterTouroku_HanbaiTankaTennic";
        private const string ProNm = "販売単価マスタ(テニック)";
        //private const short mc_L_END = 3;
        SKUPrice_BL spb;
        DataTable dt;
        M_SKUPrice_Entity mse;
        M_SKU_Entity ms;
        private enum EIndex : int
        {
            SKUCD,
            JANCD,
            ChangeDate,
            TekiyouShuuryouDate,
            SKUName,
            UnitPrice,
            StandardSalesUnitPrice,
            Rank1UnitPrice,
            Rank2UnitPrice,
            Rank3UnitPrice,
            Rank4UnitPrice,
            Rank5UnitPrice,
            Remarks
        }
        private Control[] keyControls;
        private Control[] detailControls;
        private System.Windows.Forms.Control previousCtrl;
        public MasterTouroku_HanbaiTankaTennic()
        {
            InitializeComponent();
            spb = new SKUPrice_BL();
            dt = new DataTable();
            mse = new M_SKUPrice_Entity();
            ms = new M_SKU_Entity();
        }
        ClsGridHanbaiTankaTennic mGrid = new ClsGridHanbaiTankaTennic();
        private int m_EnableCnt;
        private int m_dataCnt = 0;
        public string[] DuplicateCheckCol = null;
        private void MasterTouroku_HanbaiTankaTennic_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                SetFunctionLabel(EProMode.MENTE);
                S_SetInit_Grid();
                base.StartProgram();
                txtStartDateFrom.Focus();
                Clear(pnl_Body);
                IMT_ITMNM_7.Text = "";
                Scr_Clr(0);
                BindCombo();
              //  CustomEvent();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void BindCombo()
        {
            string ymd = bbl.GetDate();
            cbo_Year.Bind(ymd);
            cbo_Season.Bind(ymd);
        }
        public IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
        }
        private void Scr_Clr(short Kbn)  /// 0 is initial state
        {
            var ctrl = GetAllControls(pnl_Header);
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
                    else if (ctrl is CKM_RadioButton cr && cr.Name == "ckM_RadioButton1")
                    {
                        cr.Checked = true;
                    }
                }
            }
            S_Clear_Grid();   //画面クリア（明細部）
        }
        private static int TimeInvokeKeyDown = 0;
        private void S_SetInit_Grid()
        {
            int W_CtlRow;

            if (ClsGridHanbaiTankaTennic.gc_P_GYO <= ClsGridHanbaiTankaTennic.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridHanbaiTankaTennic.gMxGyo;
                m_EnableCnt = ClsGridHanbaiTankaTennic.gMxGyo;
            }
            mGrid.g_MK_Ctl_Row = ClsGridHanbaiTankaTennic.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridHanbaiTankaTennic.gc_MaxCL;

            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;
            this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_0.SmallChange = 1;
            this.Vsb_Mei_0.Minimum = 0;
            this.Vsb_Mei_0.Maximum = mGrid.g_MK_MaxValue + this.Vsb_Mei_0.LargeChange - 1;
            S_SetControlArray(); // PTK_M
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++) //PTK_M
            {
                for (int w_CtlCol = 0; w_CtlCol <= mGrid.g_MK_Ctl_Col - 1; w_CtlCol++)
                {
                    if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl != null)
                    {
                        if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                        {
                            CKM_Controls.CKM_TextBox sctl = (CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.Tag = W_CtlRow.ToString();
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            if (w_CtlCol == 3 && W_CtlRow == 0)
                            {
                                TimeInvokeKeyDown++;
                            }
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            
                        }
                    }
                    switch (w_CtlCol)
                    {

                        case (int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Date;
                            break;
                        case (int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Price;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).IntegerPart = 9;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                            break;
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Remarks:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Normal;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).Ctrl_Byte = CKM_TextBox.Bytes.半全角;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).MaxLength = 500;

                            break;
                    }
                }
            } // PTK_M
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];
            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridHanbaiTankaTennic.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            SetMultiColNo();
            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色
            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];
            for (int i = (int)ClsGridHanbaiTankaTennic.ColNO.GYONO; i <= (int)ClsGridHanbaiTankaTennic.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }
            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridHanbaiTankaTennic.ColNO.COUNT; W_CtlCol++)
                {
                    //switch (W_CtlCol)
                    //{
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.MitsumoriSuu:
                    //        mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                    //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                    //        break;
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.MitsumoriUnitPrice:
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice:
                    //        mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                    //        break;
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.MitsumoriHontaiGaku:
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.MitsumoriGaku:
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.CostGaku:
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.ProfitGaku:
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
                    mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                    mGrid.g_DArray[c].JANCD = dr["JanCD"].ToString();
                    mGrid.g_DArray[c].AdminNo = dr["AdminNo"].ToString();
                    mGrid.g_DArray[c].ChangeDate=    mGrid.g_DArray[c].StartChangeDate = dr["StartDate"].ToString();
                    mGrid.g_DArray[c].EndChangeDate = dr["EndDate"].ToString();
                    mGrid.g_DArray[c].UnitPrice = dr["UnitPrice"].ToString();
                    mGrid.g_DArray[c].StandardSalesUnitPrice = dr["StandardSalesUnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank1UnitPrice = dr["Rank1UnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank2UnitPrice = dr["Rank2UnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank3UnitPrice = dr["Rank3UnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank4UnitPrice = dr["Rank4UnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank5UnitPrice = dr["Rank5UnitPrice"].ToString();
                    mGrid.g_DArray[c].ItemName = dr["SKUName"].ToString();
                    mGrid.g_DArray[c].CostUnitPrice = dr["CostUnitPrice"].ToString();
                    mGrid.g_DArray[c].Remarks = dr["Remarks"].ToString();
                    c++;
                }
            }
        }
        private void InitScr()
        {
            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            ChangeOperationMode(base.OperationMode);
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode;
            Scr_Clr(0);
            switch (mode)
            {
                case EOperationMode.INSERT:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    break;
                case EOperationMode.UPDATE:
                    //S_BodySeigyo(1, 1);
                    S_BodySeigyo(0, 0);
                    break;
            }
            //Clear(panel1);
            F12Enable = true;
            txtStartDateFrom.Focus();
        }
        private void S_BodySeigyo(short pKBN, short pGrid)
        {
            int w_Row;

            switch (pKBN)
            {
                case 0:
                    {
                        txtStartDateFrom.Focus();
                        this.Vsb_Mei_0.TabStop = false;
                        SetFuncKeyAll(this, "111111001010");

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
                                if (!String.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SKUCD))
                                {
                                    for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                    {
                                        switch (w_Col)
                                        { 
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Remarks:
                                                {
                                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                    break;
                                                }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            txtStartDateFrom.Focus();
                            SetFuncKeyAll(this, "111111000001");
                            btnDisplay.Enabled = false;
                        }
                        break;
                    }

                case 2:
                    {
                        if (pGrid == 1)
                        {
                            pnl_Body.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            //  Scr_Lock(0, 0, 0);


                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000011");
                                // Scr_Lock(0, 3, 1);
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

                case 3:
                    Scr_Clr(1);
                    break;
                default:
                    {
                        break;
                    }
            }
        }
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            switch (Index + 1)
            {
                case 2:
                    ChangeOperationMode(EOperationMode.INSERT);
                    //ChangeOperationMode(OperationMode);
                    Clear(pnl_Header);
                    //Clear(panel1);
                    RadioButton1.Checked = true;
                    break;
                case 3:
                    ChangeOperationMode(EOperationMode.UPDATE);
                  //  ChangeOperationMode(OperationMode);//
                    Clear(pnl_Header);
                    RadioButton1.Checked = true;
                    InitScr();
                    break;
                case 4:
                    ChangeOperationMode(EOperationMode.DELETE);
                 //   ChangeOperationMode(OperationMode);//
                    Clear(pnl_Header);
                    RadioButton1.Checked = true;
                    InitScr();
                    break;
                case 5:
                    ChangeOperationMode(EOperationMode.SHOW);
                   // ChangeOperationMode(OperationMode);//
                    Clear(pnl_Header);
                    RadioButton1.Checked = true;
                    InitScr();
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeOperationMode(OperationMode);
                        Clear(pnl_Header);
                        RadioButton1.Checked = true;
                        InitScr();
                        F12Enable = true;
                    }
                    else
                        PreviousCtrl.Focus();
                    break;
                case 12:
                    F12();
                    break;
            }
        }
        //private DataTable GetGridEntity()
        //{
        //    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
        //    {
        //        //m_dataCntが更新有効行数
        //        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SKUCD) == false)
        //        {
        //            dt.Rows.Add(mGrid.g_DArray[RW].SKUCD
        //                , mGrid.g_DArray[RW].AdminNo
        //                , mGrid.g_DArray[RW].JANCD
        //                , bbl.Z_Set(mGrid.g_DArray[RW].StartChangeDate)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].EndChangeDate)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].UnitPrice)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].StandardSalesUnitPrice)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].Rank1UnitPrice)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].Rank2UnitPrice)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].Rank3UnitPrice)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].Rank4UnitPrice)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].Rank5UnitPrice)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].ItemName)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice)
        //                , bbl.Z_Set(mGrid.g_DArray[RW].Space1)
        //                , mGrid.g_DArray[RW].Remarks == "" ? null : mGrid.g_DArray[RW].Remarks
        //                //, mGrid.g_DArray[RW].Update
        //                );
        //        }
        //    }
        //    return dt;
        //}
        private DataTable GetdatafromArray()
        {
            var result = new DataTable();
            var dt = new DataTable();
            var colnames = new string[] { "TanKaCD", "StoreCD", "AdminNO", "SKUCD","ChangeDate", "StartChangeDate", "EndChangeDate", "PriceWithoutTax", "SalePriceOutTax", "Remarks", "DeleteFlg", "UsedFlg", "InsertOperartor", "InsertDateTime", "UpdateOperator", "UpdateDateTime" };
            var ColumnNames = new string[] { "SKUCD", "AdminNo", "JANCD","ChangeDate", "StartChangeDate", "EndChangeDate", "UnitPrice", "StandardSalesUnitPrice", "Rank1", "Rank2", "Rank3", "Rank4", "Rank5", "ItemName", "CostUnitPrice", "Remarks" };
            foreach (var col in ColumnNames)
            {
                dt.Columns.Add(col);
            }
            foreach (var col in colnames)
            {
                result.Columns.Add(col);
            }
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //DataRow dr = dt.NewRow();

                // var sd= 
                if (!string.IsNullOrEmpty(mGrid.g_DArray[RW].SKUCD))
                {
                    dt.Rows.Add(
                        mGrid.g_DArray[RW].SKUCD
                          , mGrid.g_DArray[RW].AdminNo
                         , mGrid.g_DArray[RW].JANCD
                         , mGrid.g_DArray[RW].ChangeDate
                         , mGrid.g_DArray[RW].StartChangeDate
                         , mGrid.g_DArray[RW].EndChangeDate
                         , mGrid.g_DArray[RW].UnitPrice
                         , mGrid.g_DArray[RW].StandardSalesUnitPrice
                         , mGrid.g_DArray[RW].Rank1UnitPrice
                         , mGrid.g_DArray[RW].Rank2UnitPrice
                         , mGrid.g_DArray[RW].Rank3UnitPrice
                         , mGrid.g_DArray[RW].Rank4UnitPrice
                         , mGrid.g_DArray[RW].Rank5UnitPrice
                         , mGrid.g_DArray[RW].ItemName
                         , mGrid.g_DArray[RW].CostUnitPrice
                         // , bbl.Z_Set(mGrid.g_DArray[RW].Space1)
                         , mGrid.g_DArray[RW].Remarks == "" ? null : mGrid.g_DArray[RW].Remarks
                         //, mGrid.g_DArray[RW].Update
                         );
                }
                else
                {
                    dt.Rows.Add();
                }
            }

            //foreach (DataRow dr in dt.Rows)
            //{
            //    if ( !string.IsNullOrEmpty(dr["SKUCD"].ToString()) )
            //    {
            //        if (string.IsNullOrEmpty(dr["StartChangeDate"].ToString()))
            //        {
            //            //Show Error Empty
            //        }
            //        else
            //        {
            //            M_SKUPrice_Entity mse = new M_SKUPrice_Entity
            //            {
            //                SKUCD = dr["SKUCD"].ToString(),
            //            };
            //            var dt_Exist = spb.M_SKUPrice_DataSelect(mse);
            //            if (dt_Exist.Rows.Count > 0)
            //            {
            //                return null;
            //            }
            //        }
            //    }
            //}

            var dtnow = DateTime.Now.ToString();
           
            try
            {
                foreach (DataRow r in dt.Rows)
                {

                    var row = r;
                    if (!row.IsNull(1))
                    {
                        if (!String.IsNullOrEmpty(row["StandardSalesUnitPrice"].ToString()))
                        {
                            result.Rows.Add(
                                "0",
                                "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["ChangeDate"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["StandardSalesUnitPrice"].ToString(),
                                row["Remarks"].ToString(),
                                "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (!String.IsNullOrEmpty(row["Rank1"].ToString()))
                        {
                            result.Rows.Add(
                                "1",
                                 "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["ChangeDate"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank1"].ToString(),
                                row["Remarks"].ToString(),
                                 "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (!String.IsNullOrEmpty(row["Rank2"].ToString()))
                        {
                            result.Rows.Add(
                                "2",
                                "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["ChangeDate"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank2"].ToString(),
                                row["Remarks"].ToString(),
                                  "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (!String.IsNullOrEmpty(row["Rank3"].ToString()))
                        {
                            result.Rows.Add(
                                "3",
                                 "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["ChangeDate"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank3"].ToString(),
                                row["Remarks"].ToString(),
                                   "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (!String.IsNullOrEmpty(row["Rank4"].ToString()))
                        {
                            result.Rows.Add(
                                "4",
                                 "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["ChangeDate"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank4"].ToString(),
                                row["Remarks"].ToString(),
                                   "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (!String.IsNullOrEmpty(row["Rank5"].ToString()))
                        {
                            result.Rows.Add(
                                "5",
                                 "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["ChangeDate"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank5"].ToString(),
                                row["Remarks"].ToString(),
                                "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                    }
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;

            }
            
            return result;
        }
        private string Getint(string val)
        {
            try
            {
                return Convert.ToInt32(val.Split('.')[0]).ToString();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return "0";
        }
        private M_SKUPrice_Entity GetSearchInfo()
        {
            mse = new M_SKUPrice_Entity();
            mse.DisplayKBN = RadioButton1.Checked ? "0" : "1";
            mse.StartChangeDate = txtStartDateFrom.Text;
            mse.EndChangeDate = txtStartDateTo.Text;

            return mse;
        }
        private M_SKU_Entity GetInfo()
        {
            ms = new M_SKU_Entity()
            {
                SKUCDFrom = SKUCDFrom.Text,
                SKUCDTo = SKUCDTo.Text,
                BrandCD = scBrandCD.TxtCode.Text,
                SKUName = txtSKUName.Text,
                ExhibitionSegmentCD=scSegmentCD.TxtCode.Text,
                DeleteFlg="0"
            };
            return ms;
        }
        private void F11()
        {
            mse = GetSearchInfo();
            ms = GetInfo();
            dt = spb.M_SKUPrice_HanbaiTankaTennic_Select(mse, ms, (short)OperationMode);
            if (dt.Rows.Count > 0)
            {
                S_Clear_Grid();
                if ((OperationMode == EOperationMode.DELETE) || (OperationMode == EOperationMode.SHOW))
                {
                    SetMultiColNo(dt);
                    S_BodySeigyo(2, 2);
                    mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                    IMT_STADT_0.Focus();
                    return;
                }
                else
                {
                    S_BodySeigyo(3, 0);
                    SetMultiColNo(dt);
                    S_BodySeigyo(1, 1);
                    mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                    IMT_STADT_0.Focus();
                }
            }
            else
            {
                bbl.ShowMessage("E133");
                txtStartDateFrom.Focus();
            }
        }
        private void CustomEvent()
        {
        //    Add(new Control[] { panel10_1, panel10_2, panel8, panel3, panel17, panel4, panel65, panel89, panel113, panel137 });
            
        }
        private void Add(Control[] cont)
        {
            foreach (var ctr in cont)
            {
                var Con = GetAllControls(ctr);
                foreach (var c in Con)
                {
                    if (c is CKM_TextBox ct)
                    {
                        ct.Leave += Ct_Leave;
                        ct.Validated += Validated;
                        //ct.GotFocus += Ct_GotFocus;
                    }
                }
            }
        }
        private void Validated(object sender, EventArgs e)
        {
        //    GridControl_Validated(sender,e);
        }
        private void Ct_Leave(object sender, EventArgs e)
        {//IMT_GYONO_0

            if ((sender as CKM_TextBox).Parent is Panel cp)
            {
                var Con = GetAllControls(cp);
                foreach (var ctr in Con)
                {
                    if (ctr is Label cl)
                    {
                        if (Convert.ToInt32(cl.Text) % 2 == 0)
                        {
                            ((CKM_TextBox)sender).BackColor = ClsGridBase.GridColor;
                        }
                        else {
                            ((CKM_TextBox)sender).Back_Color = CKM_TextBox.CKM_Color.White;
                        }
                        break;
                    }
                }
                mGrid.S_DispToArray(Vsb_Mei_0.Value);
                //   ((CKM_TextBox)sender).BackColor =ClsGridBase.GridColor;
                //if (ct is "")
                //((CKM_TextBox)sender).BackColor = ClsGridBase.GridColor;
                //else
                //    ((CKM_TextBox)sender).BackColor =  Color.White;
            }
        }
        bool IsEntered = false;
        private void GridControl_Validated(object sender, EventArgs e)
        {
            if (IsEntered)
                return;
            Control c = sender as Control;
            if (c is CKM_TextBox ct)
            {
                if (ct.Name.Contains("IMT_STADT_")) // Done
                {
                    if (!bbl.CheckDate(ct.Text))
                    {
                        bbl.ShowMessage("E103");
                        ct.Focus();
                    }
                    if (ActiveControl is CKM_Button cb && cb.Name.Contains("Btn"))
                    {
                        return;
                    }
                    var SKUCD = this.Controls.Find("IMT_ITMCD_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox;
                    M_SKUPrice_Entity mse = new M_SKUPrice_Entity
                    {
                        SKUCD = SKUCD.Text,
                        StartChangeDate = ct.Text
                    };
                    DataTable dt = spb.M_SKUPrice_DataSelect(mse);
                    if (dt.Rows.Count > 0)
                    {
                        bbl.ShowMessage("E105");
                         ct.Focus();
                        return;
                    }
                }
                if (ct.Name.Contains("IMT_ENDDT_"))
                {
                    if (!bbl.CheckDate(ct.Text))
                    {
                        bbl.ShowMessage("E103");
                        ct.Focus();
                    }
                    var StartDate = this.Controls.Find("IMT_STADT_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox;
                    if (string.Compare(StartDate.Text, ct.Text) == 1)
                    {
                        bbl.ShowMessage("E104");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_SSUNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var ssUnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R1UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r1UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R2UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r2UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R3UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r3UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R4UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r4UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R5UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r5UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
            }
            mGrid.S_DispToArray(Vsb_Mei_0.Value);
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
                w_ActCtl.BackColor = ClsGridHanbaiTankaTennic.BKColor;
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
        }  //PTK_M
        private void GridControl_Leave(object sender, EventArgs e)  // PTK_M
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
            var f = TimeInvokeKeyDown;
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
                            case "IMT_STADT":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate;
                                break;
                            case "IMT_ENDDT":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate;
                                break;
                            case "IMN_UNITPRICE":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice;
                                break;
                            case "IMN_SSUNITPRICE":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice;
                                break;
                            case "IMN_R1UNITPRICE":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice;
                                break;
                            case "IMN_R2UNITPRICE":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice;
                                break;
                            case "IMN_R3UNITPRICE":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice;
                                break;
                            case "IMN_R4UNITPRICE":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice;
                                break;
                            case "IMN_R5UNITPRICE":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice;
                                break;
                            case "IMT_REMARK":
                                CL = (int)ClsGridHanbaiTankaTennic.ColNO.Remarks;
                                break;
                        }
                        if (CL == -1)
                            return;
                        mGrid.S_DispToArray(Vsb_Mei_0.Value);
                        //チェック処理
                        if (CheckGrid(CL, w_Row) == false)
                        {
                            if (w_ActCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                            {
                                ((CKM_Controls.CKM_ComboBox)w_ActCtl).MoveNext = false;
                            }

                            //配列の内容を画面へセット
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                            // IMT_DMY_0.Focus();
                            //Focusセット処理
                            w_ActCtl.Focus();
                            return;
                        }

                        if (lastCell)
                        {
                            w_ActCtl.Focus();
                            return;
                        }

                        S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
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
        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false, bool IsExec = false)  // PTK_M
        {
            bool ret = false;

            string ymd = "";

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            if (!chkAll && !changeYmd)  // check length
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridHanbaiTankaTennic.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    {
                        if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                            return false;
                    }

            }
                switch (col)
                {
                    case (int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate:  // Hissu komoku
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].StartChangeDate))
                            {
                                bbl.ShowMessage("E102");
                                //ActiveControl.Focus();
                                return false;
                            }
                            else
                            {
                                if (!bbl.CheckDate(mGrid.g_DArray[row].StartChangeDate))
                                {
                                    bbl.ShowMessage("E103");
                                    return false;
                                }
                                else
                                {

                                if (OperationMode == EOperationMode.INSERT)
                                {
                                    var SKUCD = mGrid.g_DArray[row].SKUCD; //this.Controls.Find("IMT_ITMCD_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox;
                                    M_SKUPrice_Entity mse = new M_SKUPrice_Entity
                                    {
                                        SKUCD = SKUCD,
                                        StartChangeDate = mGrid.g_DArray[row].StartChangeDate
                                    };
                                    DataTable dt = spb.M_SKUPrice_DataSelect(mse);
                                    if (dt.Rows.Count > 0)
                                    {
                                        bbl.ShowMessage("E105");
                                        return false;
                                    }
                                }
                            }
                        }

                     }
                        break;

                    case (int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate:  // Not hissu
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].EndChangeDate))
                        {
                            /// not requred field
                        }
                        else
                        {
                            if (!bbl.CheckDate(mGrid.g_DArray[row].EndChangeDate))
                            {
                                //Ｅ１０３
                                bbl.ShowMessage("E103");
                                return false;
                            }
                            else
                            {
                                var StartDate = mGrid.g_DArray[row].StartChangeDate; //this.Controls.Find("IMT_STADT_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox;
                                if (string.Compare(StartDate, mGrid.g_DArray[row].EndChangeDate) == 1)
                                {
                                    bbl.ShowMessage("E104");
                                    return false;
                                }
                            }
                        }
                        break;
                    case (int)ClsGridHanbaiTankaTennic.ColNO.Remarks:
                        break;
                }
                decimal Su = 0;
                switch (col)
                {
                    case (int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice:
                        Su = bbl.Z_Set(mGrid.g_DArray[row].UnitPrice);
                        mGrid.g_DArray[row].UnitPrice = bbl.Z_SetStr(Su);
                        if (!chkAll)
                            if (Su.Equals(0))
                            {
                                if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                    return false;
                            }
                        break;
                    case (int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice:
                        Su = bbl.Z_Set(mGrid.g_DArray[row].StandardSalesUnitPrice);
                        mGrid.g_DArray[row].StandardSalesUnitPrice = bbl.Z_SetStr(Su);
                        if (!chkAll)
                            if (Su.Equals(0))
                            {
                                if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                    return false;
                        }
                        break;
                    case (int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice:
                        Su = bbl.Z_Set(mGrid.g_DArray[row].Rank1UnitPrice);
                        mGrid.g_DArray[row].Rank1UnitPrice = bbl.Z_SetStr(Su);
                        if (!chkAll)
                            if (Su.Equals(0))
                            {
                                if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                    return false;
                            }
                        break;
                    case (int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice:
                        Su = bbl.Z_Set(mGrid.g_DArray[row].Rank2UnitPrice);
                        mGrid.g_DArray[row].Rank2UnitPrice = bbl.Z_SetStr(Su);
                        if (!chkAll)
                            if (Su.Equals(0))
                            {
                                if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                    return false;
                            }
                        break;
                    case (int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice:
                        Su = bbl.Z_Set(mGrid.g_DArray[row].Rank3UnitPrice);
                        mGrid.g_DArray[row].Rank3UnitPrice = bbl.Z_SetStr(Su);
                        if (!chkAll)
                            if (Su.Equals(0))
                            {
                                if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                    return false;
                            }
                        break;
                    case (int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice:
                        Su = bbl.Z_Set(mGrid.g_DArray[row].Rank4UnitPrice);
                        mGrid.g_DArray[row].Rank4UnitPrice = bbl.Z_SetStr(Su);
                        if (!chkAll)
                            if (Su.Equals(0))
                            {
                                if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                    return false;
                            }
                        break;
                    case (int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice:
                        Su = bbl.Z_Set(mGrid.g_DArray[row].Rank5UnitPrice);
                        mGrid.g_DArray[row].Rank5UnitPrice = bbl.Z_SetStr(Su);
                        if (!chkAll)
                            if (Su.Equals(0))
                            {
                                if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                    return false;
                            }
                        break;
                    case (int)ClsGridHanbaiTankaTennic.ColNO.Remarks:
                        string strR = Encoding.GetEncoding(932).GetByteCount(mGrid.g_DArray[row].Remarks).ToString();
                        if (Convert.ToInt32(strR) > ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, 0].CellCtl).MaxLength)
                        {
                            MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //EnableCell(row, col);
                            mGrid.g_MK_State[col, row].Cell_Enabled = true;
                            mGrid.g_MK_State[col, row].Cell_ReadOnly = false;
                            return false;
                        }
                        break;

                }
      //      mGrid.S_DispToArray(Vsb_Mei_0.Value);
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvNxt, (int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);
            //Control1
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 0].CellCtl = IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 0].CellCtl = IMT_STADT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 0].CellCtl = IMT_ENDDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 0].CellCtl = IMN_UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 0].CellCtl = IMN_SSUNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 0].CellCtl = IMN_R1UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 0].CellCtl = IMN_R2UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 0].CellCtl = IMN_R3UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 0].CellCtl = IMN_R4UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 0].CellCtl = IMN_R5UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 0].CellCtl = IMN_COSTUNPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 0].CellCtl = IMT_REMARK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 0].CellCtl = Space0;
            //Control2
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 1].CellCtl = IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 1].CellCtl = IMT_STADT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 1].CellCtl = IMT_ENDDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 1].CellCtl = IMN_UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 1].CellCtl = IMN_SSUNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 1].CellCtl = IMN_R1UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 1].CellCtl = IMN_R2UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 1].CellCtl = IMN_R3UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 1].CellCtl = IMN_R4UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 1].CellCtl = IMN_R5UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 1].CellCtl = IMN_COSTUNPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 1].CellCtl = IMT_REMARK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 1].CellCtl = Space0;
            //Control3
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 2].CellCtl = IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 2].CellCtl = IMT_STADT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 2].CellCtl = IMT_ENDDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 2].CellCtl = IMN_UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 2].CellCtl = IMN_SSUNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 2].CellCtl = IMN_R1UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 2].CellCtl = IMN_R2UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 2].CellCtl = IMN_R3UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 2].CellCtl = IMN_R4UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 2].CellCtl = IMN_R5UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 2].CellCtl = IMN_COSTUNPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 2].CellCtl = IMT_REMARK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 2].CellCtl = Space1;
           
            //Control4
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 3].CellCtl = IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 3].CellCtl = IMT_STADT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 3].CellCtl = IMT_ENDDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 3].CellCtl = IMN_UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 3].CellCtl = IMN_SSUNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 3].CellCtl = IMN_R1UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 3].CellCtl = IMN_R2UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 3].CellCtl = IMN_R3UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 3].CellCtl = IMN_R4UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 3].CellCtl = IMN_R5UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 3].CellCtl = IMN_COSTUNPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 3].CellCtl = IMT_REMARK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 3].CellCtl = Space2;
            //Control5
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 4].CellCtl = IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 4].CellCtl = IMT_STADT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 4].CellCtl = IMT_ENDDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 4].CellCtl = IMN_UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 4].CellCtl = IMN_SSUNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 4].CellCtl = IMN_R1UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 4].CellCtl = IMN_R2UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 4].CellCtl = IMN_R3UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 4].CellCtl = IMN_R4UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice,4].CellCtl = IMN_R5UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 4].CellCtl = IMN_COSTUNPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 4].CellCtl = IMT_REMARK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 4].CellCtl = Space3;
            //Control6
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 5].CellCtl = IMT_JANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 5].CellCtl = IMT_STADT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 5].CellCtl = IMT_ENDDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 5].CellCtl = IMN_UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 5].CellCtl = IMN_SSUNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 5].CellCtl = IMN_R1UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 5].CellCtl = IMN_R2UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 5].CellCtl = IMN_R3UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 5].CellCtl = IMN_R4UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 5].CellCtl = IMN_R5UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 5].CellCtl = IMN_COSTUNPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 5].CellCtl = IMT_REMARK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 5].CellCtl = Space4;
            //Control7
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 6].CellCtl = IMT_JANCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 6].CellCtl = IMT_STADT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 6].CellCtl = IMT_ENDDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 6].CellCtl = IMN_UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 6].CellCtl = IMN_SSUNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 6].CellCtl = IMN_R1UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 6].CellCtl = IMN_R2UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 6].CellCtl = IMN_R3UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 6].CellCtl = IMN_R4UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 6].CellCtl = IMN_R5UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 6].CellCtl = IMN_COSTUNPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 6].CellCtl = IMT_REMARK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 6].CellCtl = Space5;
            //Control8
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 7].CellCtl = IMT_JANCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 7].CellCtl = IMT_STADT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 7].CellCtl = IMT_ENDDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 7].CellCtl = IMN_UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 7].CellCtl = IMN_SSUNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 7].CellCtl = IMN_R1UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 7].CellCtl = IMN_R2UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 7].CellCtl = IMN_R3UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 7].CellCtl = IMN_R4UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 7].CellCtl = IMN_R5UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 7].CellCtl = IMN_COSTUNPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 7].CellCtl = IMT_REMARK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 7].CellCtl = Space6;
            //Control9
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 8].CellCtl = IMT_JANCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 8].CellCtl = IMT_STADT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 8].CellCtl = IMT_ENDDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 8].CellCtl = IMN_UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 8].CellCtl = IMN_SSUNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 8].CellCtl = IMN_R1UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 8].CellCtl = IMN_R2UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 8].CellCtl = IMN_R3UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 8].CellCtl = IMN_R4UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 8].CellCtl = IMN_R5UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 8].CellCtl = IMN_COSTUNPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 8].CellCtl = IMT_REMARK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 8].CellCtl = Space7;
            //Control10
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 9].CellCtl = IMT_JANCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 9].CellCtl = IMT_STADT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 9].CellCtl = IMT_ENDDT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 9].CellCtl = IMN_UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 9].CellCtl = IMN_SSUNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 9].CellCtl = IMN_R1UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 9].CellCtl = IMN_R2UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 9].CellCtl = IMN_R3UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 9].CellCtl = IMN_R4UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 9].CellCtl = IMN_R5UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 9].CellCtl = IMN_COSTUNPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 9].CellCtl = IMT_REMARK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 9].CellCtl = Space8;
        }
        //private void S_Grid_0_Event_MouseWheel(int pDelta)
        //{
        //    int w_ToMove = pDelta * (-1) / (int)mGrid.g_WHEEL_DELTA;
        //    int w_Value;
        //    int w_MaxValue;

        //    mGrid.g_WheelFLG = true;

        //    if (mGrid.g_MK_MaxValue > m_dataCnt - 1)
        //        w_MaxValue = m_dataCnt - 1;
        //    else
        //        w_MaxValue = mGrid.g_MK_MaxValue;

        //    w_Value = Vsb_Mei_0.Value + w_ToMove;

        //    switch (w_Value)
        //    {
        //        case object _ when w_Value > w_MaxValue:
        //            {
        //                Vsb_Mei_0.Value = w_MaxValue;
        //                break;
        //            }

        //        case object _ when w_Value < Vsb_Mei_0.Minimum:
        //            {
        //                Vsb_Mei_0.Value = Vsb_Mei_0.Minimum;
        //                break;
        //            }

        //        default:
        //            {
        //                Vsb_Mei_0.Value = w_Value;
        //                break;
        //            }
        //    }

        //    mGrid.g_WheelFLG = false;
        //}
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
                    txtStartDateFrom.TabStop = pTabStop;
                else
                    txtStartDateFrom.TabStop = false;

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
        private void Vsb_Mei_0_ValueChanged(object sender, System.EventArgs e)
        {
            int w_Dest = 0;
            int w_Row = 0;
            bool w_IsGrid;

            if (mGrid.g_VSB_Flg == 1)
                return;

            //w_IsGrid = mGrid.F_Search_Ctrl_MK(this.ActiveControl, out int w_Col, out int w_CtlRow);
            //if (w_IsGrid & mGrid.g_InMoveFocus_Flg == 0)
            //{

            //    // 明細部にフォーカスがあるとき
            //    w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //    if (Math.Sign(this.Vsb_Mei_0.Value - mGrid.g_MK_DataValue) == -1)
            //        // 上へスクロール
            //        w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvPrv;
            //    else
            //        // 下へスクロール
            //        w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvNxt;

            //    // 一旦 フォーカスを退避
            //    //IMT_DMY_0.Focus();
            //}

            // 画面の内容を、配列にセット(スクロール前の行に)
            mGrid.S_DispToArray(mGrid.g_MK_DataValue);

            // 配列より画面セット (スクロール後の行)
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);

            //if (w_IsGrid & mGrid.g_InMoveFocus_Flg == 0)
            //{

            //    // 元いた位置にフォーカスをセット(場合によってはロックがかかっているかもしれないのでセットしなおす)
            //    if (w_Dest == (int)ClsGridBase.Gen_MK_FocusMove.MvPrv)
            //    {
            //        if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
            //            // その行の最後から探す
            //            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
            //    }
            //    else if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
            //    {
            //        if (mGrid.g_WheelFLG == true)
            //        {
            //            // まず対象行の先頭からさがし、まったくフォーカス移動先が無ければ
            //            // 最後のフォーカス可能セルに移動
            //            if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, mGrid.g_MK_Ctrl[mGrid.g_MK_FocusOrder[0], w_CtlRow].CellCtl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]) == false)
            //                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvPrv, this.ActiveControl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, mGrid.g_MK_Max_Row - 1, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
            //        }
            //        else
            //            // その行の先頭から探す
            //            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]);
            //    }
            //}

            // 連続スクロールの途中に、画面の表示がおかしくなる現象への対策
            pnl_Body.Refresh();
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
        private M_SKUPrice_Entity SKUPriceEntity()
        {
            mse = new M_SKUPrice_Entity()
            {
                StoreCD = "0000",
                StartChangeDate = IMT_STADT_0.Text,
                EndChangeDate = IMT_ENDDT_0.Text,
                PriceWithTax = "0",
                PriceWithoutTax = "0",
                GeneralRate = "0",
                GeneralPriceWithTax = "0",
                GeneralPriceOutTax="0",
                MemberRate="0",
                MemberPriceWithTax="0",
                MemberPriceOutTax="0",
                ClientRate="0",
                ClientPriceWithTax="0",
                ClientPriceOutTax="0",
                SaleRate="0",
                SalePriceWithTax="0",
                SalePriceOutTax="0",
                WebRate="0",
                WebPriceWithTax="0",
                WebPriceOutTax="0",
                UnitPrice= IMN_UNITPRICE_0.Text,
                StandardSalesUnitPrice= IMN_SSUNITPRICE_0.Text,
                Remarks=IMT_REMARK_0.Text,
                DeleteFlg ="0",
                UsedFlg="0",
                Operator = InOperatorCD,
                ProcessMode =ModeText,
                ProgramID = InProgramID,
                PC = InPcID,
                Key =txtStartDateFrom.Text
            };
            return mse;
        }
        private bool CheckAllGrid()
        {
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++) // GridControl
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SKUCD) == false)
                {
                    for (int CL = (int)ClsGridHanbaiTankaTennic.ColNO.GYONO; CL < (int)ClsGridHanbaiTankaTennic.ColNO.COUNT; CL++)
                    {
                        switch (CL)
                        {
                            case (int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate:
                            case (int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate:
                            case (int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice:
                            case (int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice:
                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice:
                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice:
                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice:
                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice:
                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice:
                            case (int)ClsGridHanbaiTankaTennic.ColNO.Remarks:
                                if (!CheckGrid(CL, RW, true))
                                {
                                    ERR_FOCUS_GRID_SUB(CL, RW);
                                    return false;
                                }
                                break;
                        }
                    }
                }
            }
            return true;
        }
        private void ERR_FOCUS_GRID_SUB(int pCol, int pRow)
        {
            Control w_Ctrl;
            bool w_Ret;
            int w_CtlRow;

            w_CtlRow = pRow - Vsb_Mei_0.Value;

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            w_Ctrl = Btn_F1;  /// Confirmed

            // IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvSet, (int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }
        private void F12()
        {
            if (spb.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
            {
                if (!CheckAllGrid())
                {
                    return;
                }
                mse = SKUPriceEntity();
                switch (OperationMode)
                {
                    case EOperationMode.INSERT:
                            Insert(1);
                            InitScr();
                        break;
                    case EOperationMode.UPDATE:
                        UpdateDelete(2);
                        InitScr();
                        break;
                    case EOperationMode.DELETE: 
                        UpdateDelete(3);
                        InitScr();
                        break;
                }
            }
        }
        private void Insert(int mode)
        {
            var dt = GetdatafromArray();
            //if (dt == null)
            //{
            //    bbl.ShowMessage("E105");  // Start date exist check
            //    PreviousCtrl.Focus();
            //    return;
            //}
            string Xml = spb.DataTableToXml(dt);
            if (spb.M_SKUPrice_Insert_Update(mse, Xml, mode))
            {
                 spb.ShowMessage("I101");
                 Clear(pnl_Header);
                 Clear(pnl_Body);
                 ChangeOperationMode(OperationMode);
            }
            else
            {
                spb.ShowMessage("S001");
            }
        }
        private void UpdateDelete(int mode)
        {
            var dt = GetdatafromArray();
            string Xml = spb.DataTableToXml(dt);
            if (spb.M_SKUPrice_Insert_Update(mse, Xml, mode))
            {
                spb.ShowMessage("I101");
                Clear(pnl_Header);
                Clear(pnl_Body);
                ChangeOperationMode(OperationMode);
            }
            else
            {
                spb.ShowMessage("S001");
            }
        }
       
        protected override void EndSec()
        {
            this.Close();
        }

        private void MasterTouroku_HanbaiTankaTennic_KeyUp(object sender, KeyEventArgs e)
        {
            if (ActiveControl is CKM_TextBox ct)
            {
                //ckM_TextBox4
                if (ct.Name.Contains("_"))
                {
                    if (ct.Name == "ckM_TextBox4" || ct.Name == "ckM_TextBox5")
                    {
                        MoveNextControl(e);
                    }
                  
                }
                else
                    MoveNextControl(e);
            }
            else
                MoveNextControl(e);


        }

        private void txtStartDateTo_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtStartDateFrom.Text) && !string.IsNullOrEmpty(txtStartDateTo.Text))
                {
                    if (string.Compare(txtStartDateFrom.Text, txtStartDateTo.Text) == 1)
                    {
                        bbl.ShowMessage("E104");
                        txtStartDateTo.Focus();
                    }
                }
            }
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void scSegmentCD_CodeKeyDownEvent_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSegmentCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSegmentCD.TxtCode.Text))
                {
                    if (!scSegmentCD.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scSegmentCD.SetFocus(1);
                    }
                }
            }
        }

        private void scSegmentCD_Enter_1(object sender, EventArgs e)
        {
            scSegmentCD.Value1 = "226";
        }

        private void scBrandCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scBrandCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scBrandCD.TxtCode.Text))
                {
                    if (!scBrandCD.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scBrandCD.SetFocus(1);
                    }
                }
            }
        }
       
        private void SKUCDTo_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(SKUCDFrom.TxtCode.Text) && !string.IsNullOrEmpty(SKUCDTo.TxtCode.Text))
                {
                    if (string.Compare(SKUCDFrom.TxtCode.Text, SKUCDTo.TxtCode.Text) == 1)
                    {
                        bbl.ShowMessage("E104");
                        SKUCDTo.Focus();
                    }
                }
            }
        }
    }
}
