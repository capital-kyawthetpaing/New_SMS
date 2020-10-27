using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WMasterTouroku_HanbaiTankaTennic
{
    class ClsGridHanbaiTankaTennic : GridBase.ClsGridBase
    {
        internal struct ST_DArray_Grid
        {
            internal string GYONO;
            internal string SKUCD;
            internal string JANCD;
            internal string StartChangeDate;
            internal string EndChangeDate;
            internal string UnitPrice;
            internal string StandardSalesUnitPrice;
            internal string Rank1UnitPrice;
            internal string Rank2UnitPrice;
            internal string Rank3UnitPrice;
            internal string Rank4UnitPrice;
            internal string Rank5UnitPrice;
            internal string ItemName;
            internal string CostUnitPrice;
            internal string Remarks;
            internal string Space1;
            //hide
            internal string AdminNo;
            internal string ChangeDate;
            internal int DeleteFlg;
            internal int UsedFLG;
            internal int TaxRateFLG;
        }
        internal enum ColNO : int
        {
            GYONO,
            SKUCD,
            JANCD,
            StartChangeDate,
            EndChangeDate,
            UnitPrice,
            StandardSalesUnitPrice,
            Rank1UnitPrice,
            Rank2UnitPrice,
            Rank3UnitPrice,
            Rank4UnitPrice,
            Rank5UnitPrice,
            ItemName,
            CostUnitPrice,
            Remarks,
            Space1,
            COUNT
        }
        internal ST_DArray_Grid[] g_DArray = null;
        internal const int gc_MaxCL = (int)ColNO.COUNT; 
        internal const int gc_P_GYO = 10; 
        internal const int gMxGyo = 999;
        internal short g_VSB_Flg;         
        internal short g_InMoveFocus_Flg;   
        internal bool g_GridTabStop = true;
        internal bool g_WheelFLG = false;
        internal bool F_GetTabStop(int pCol, int pRow)
        {
            if (g_GridTabStop == false)
                return false;
            else
                return g_MK_State[pCol, pRow].Cell_Selectable;
        }
        internal void S_DispFromArray(int pStartRow, ref VScrollBar pScrool, int pStCtlRow = 0, int pEdCtlRow = -1)
        {
            int w_Row;
            int w_CtlRow;
            int w_CtlCol;

            if (pStartRow != pScrool.Value)
            {
                g_VSB_Flg = 1;
                pScrool.Value = pStartRow;
                g_VSB_Flg = 0;
            }

            g_MK_DataValue = pStartRow;

            if (pEdCtlRow == -1)
                pEdCtlRow = g_MK_Ctl_Row - 1;

            for (w_CtlRow = pStCtlRow; w_CtlRow <= pEdCtlRow; w_CtlRow++) 
            {
                w_Row = w_CtlRow + pStartRow;
               //Gyono
                w_CtlCol = (int)ColNO.GYONO;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].GYONO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //skuCD
                w_CtlCol = (int)ColNO.SKUCD;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SKUCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // JanCD
                w_CtlCol = (int)ColNO.JANCD;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].JANCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
               
                //SChangeDate
                w_CtlCol = (int)ColNO.StartChangeDate;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].StartChangeDate);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // EChangeDate
                w_CtlCol = (int)ColNO.EndChangeDate;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].EndChangeDate);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // UnitPrice
                w_CtlCol = (int)ColNO.UnitPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].UnitPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                //SSUnitPrice 
                w_CtlCol = (int)ColNO.StandardSalesUnitPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].StandardSalesUnitPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                //R1UnitPrice 
                w_CtlCol = (int)ColNO.Rank1UnitPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Rank1UnitPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                //R2UnitPrice
                w_CtlCol = (int)ColNO.Rank2UnitPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Rank2UnitPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // R3UnitPrice
                w_CtlCol = (int)ColNO.Rank3UnitPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Rank3UnitPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // R4UnitPrice
                w_CtlCol = (int)ColNO.Rank4UnitPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Rank4UnitPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                //R5UnitPrice 
                w_CtlCol = (int)ColNO.Rank5UnitPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Rank5UnitPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                //ItemName 
                w_CtlCol = (int)ColNO.ItemName;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ItemName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // CostUnitPrice
                w_CtlCol = (int)ColNO.CostUnitPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].CostUnitPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                //Remarks 
                w_CtlCol = (int)ColNO.Remarks;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Remarks);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                //space
                w_CtlCol = (int)ColNO.Space1;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Space1);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

            }
        }
        internal void S_DispToArray(int pStartRow)
        {
            int w_Row;
            int w_CtlRow;
            int w_CtlCol;
            for (w_CtlRow = 0; w_CtlRow <= g_MK_Ctl_Row - 1; w_CtlRow++)            // 画面上の明細番号
            {
                w_Row = w_CtlRow + pStartRow;
                //gyoNo
                w_CtlCol = (int)ColNO.GYONO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].GYONO);
                //SkuCD
                w_CtlCol = (int)ColNO.SKUCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SKUCD);
                //Jancd
                w_CtlCol = (int)ColNO.JANCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].JANCD);
                //SChangeDate
                w_CtlCol = (int)ColNO.StartChangeDate;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].StartChangeDate);
                //EChangeDate
                w_CtlCol = (int)ColNO.EndChangeDate;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].EndChangeDate);
                //UnitPrice
                w_CtlCol = (int)ColNO.UnitPrice;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].UnitPrice);
                //SSUnitPrice
                w_CtlCol = (int)ColNO.StandardSalesUnitPrice;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].StandardSalesUnitPrice);
                //R1UnitPrice
                w_CtlCol = (int)ColNO.Rank1UnitPrice;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Rank1UnitPrice);
                //R2UnitPrice
                w_CtlCol = (int)ColNO.Rank2UnitPrice;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Rank2UnitPrice);
                //R3UnitPrice
                w_CtlCol = (int)ColNO.Rank3UnitPrice;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Rank3UnitPrice);
                //R4UnitPrice
                w_CtlCol = (int)ColNO.Rank4UnitPrice;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Rank4UnitPrice);
                //R5UnitPrice
                w_CtlCol = (int)ColNO.Rank5UnitPrice;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Rank5UnitPrice);
                //ItemName
                w_CtlCol = (int)ColNO.ItemName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ItemName);
                //CostUP
                w_CtlCol = (int)ColNO.CostUnitPrice;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].CostUnitPrice);
                //Remarks
                w_CtlCol = (int)ColNO.Remarks;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Remarks);
            }
        }
        internal bool F_MoveFocus(int pDest, int pErrDest, Control pErrSet, int pLastRow, int pLastCol, Control pActivCtl, VScrollBar pScrool, int pRow = -1, int pCol = -1)
        {
            Control w_MotoControl;

            w_MotoControl = pActivCtl;
            g_InMoveFocus_Flg = 1;

            F_MoveFocus_MAIN_MK(out int w_OkRow, out int w_OkCol, out bool w_OkFlg, pDest, pErrDest, pErrSet, pLastRow, pLastCol, pActivCtl, pScrool, pRow, pCol);
            bool ret = w_OkFlg;

            try
            {
                if (w_OkFlg == true)
                {
                    // 行き先があれば移動
                    F_FocusSet(out int w_CtlCol, out int w_CtlRow, out int w_Value, w_OkCol, w_OkRow, pScrool);

                    // 必要ならスクロール
                    if (w_Value != pScrool.Value)
                    {
                        pScrool.Value = w_Value;
                        S_DispFromArray(pScrool.Value, ref pScrool);
                    }

                    // フォーカス移動
                    g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.Focus();
                    if (g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.GetType().Name.ToUpper() == "CLSMEISAIBUTTON")
                    {
                    }
                    else
                        g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.BackColor = GridBase.ClsGridBase.BKColor;
                }
                else
                    // 行き先が見つかってないとき、指定先があればそこへ移動
                    if (pErrSet.CanFocus)
                {
                    ret = true;
                    pErrSet.Focus();
                    if (pErrSet.GetType().Name.ToUpper() == "CLSMEISAIBUTTON")
                    {
                    }
                    else if (pErrSet.GetType().Name.ToUpper() == "RADIOBUTTON")
                    {
                    }
                    else
                        pErrSet.BackColor = GridBase.ClsGridBase.BKColor;
                }
                else
                {
                    ret = false;
                    w_MotoControl.Focus();
                    if (w_MotoControl.GetType().Name.ToUpper() == "CLSMEISAIBUTTON")
                    {
                    }
                    else
                        w_MotoControl.BackColor = GridBase.ClsGridBase.BKColor;
                }
            }
            catch
            {
                ret = false;
                w_MotoControl.Focus();
                if (w_MotoControl.GetType().Name.ToUpper() == "CLSMEISAIBUTTON")
                {
                }
                else
                    w_MotoControl.BackColor = GridBase.ClsGridBase.BKColor;
            }

            finally
            {
                g_InMoveFocus_Flg = 0;
            }

            return ret;
        }

    }
}
