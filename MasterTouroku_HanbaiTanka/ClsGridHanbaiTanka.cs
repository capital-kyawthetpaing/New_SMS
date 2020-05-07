using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterTouroku_HanbaiTanka
{
//'明細部を構成するコントロールのTagに 行番号(0～) をセットしてください。
//'明細部を構成するコントロールのうち TextBoxコントロール以外はカスタムコントロール(MeisaiControl)を使用してください。(Tabの検知のため)
    class ClsGridHanbaiTanka :  GridBase.ClsGridBase
    {
        internal struct ST_DArray_HanbaiTanka
        {
            // 画面項目
            internal string GYONO;      // №
            internal string ITemCD;      // SKUCD
            internal string JanCD;      // 
            internal string ChangeDate;      // 
            internal string GenkaWithoutTax;      // 
            internal string ITemName;      // 
            internal string PriceWithTax;      //定価(税込)
            internal string PriceWithoutTax;      // 
            internal string GeneralPriceWithTax;      //一般(税込) 
            internal string GeneralPriceOutTax;      // 
            internal string MemberPriceWithTax;      //会員(税込) 
            internal string MemberPriceOutTax;      // 
            internal string ClientPriceWithTax;      //外商(税込) 
            internal string ClientPriceOutTax;      // 
            internal string SalePriceWithTax;      //Sale(税込) 
            internal string SalePriceOutTax;      // 
            internal string WebPriceWithTax;      //WEB(税込) 
            internal string WebPriceOutTax;      //  
            internal string Remarks;      // 

            //隠し項目
            internal int Update;        //単価マスタに存在するデータは1を（UPDATEデータ）
            internal int OldUpdate;    //単価マスタに存在するデータは1を（UPDATEデータ）
            internal string OldChangeDate;
            internal string AdminNo;      //2019.10.16 add
            internal int TaxRateFLG;
        }

        //列番号定数
        internal enum ColNO : int
        {
            GYONO,
            ITemCD,
            JanCD,
            ChangeDate,
            GenkaWithoutTax,
            ITemName,
            PriceWithTax,    //
            PriceWithoutTax,    // 
            GeneralPriceWithTax,    // 
            GeneralPriceOutTax,    // 
            MemberPriceWithTax,    // 
            MemberPriceOutTax,    // 
            ClientPriceWithTax,    // 
            ClientPriceOutTax,    // 
            SalePriceWithTax,    // 
            SalePriceOutTax,    // 
            WebPriceWithTax,    // 
            WebPriceOutTax,    //  
            Remarks,    //
            COUNT
        }
        internal ST_DArray_HanbaiTanka[] g_DArray=null;
        internal const int gc_MaxCL = (int)ColNO.COUNT;       // ｸﾞﾘｯﾄﾞ最大列数
        internal const int gc_P_GYO = 10;        // 表示明細行(画面)
        internal const int gMxGyo = 999;        //画面最大明細行数

        internal short g_VSB_Flg;           // ｽｸﾛｰﾙﾊﾞｰのValueChangedｲﾍﾞﾝﾄの実行判断
        internal short g_InMoveFocus_Flg;   // フォーカス移動中判断 
        internal bool g_GridTabStop = true;  // 明細部のTabStopプロパティ (明細部をひとつのコントロールと考えた際のTabStop) 
        internal bool g_WheelFLG = false;    // MouseWheel では True

        // 明細部のコントロールのTabStopを、変数の内容から判断して返す
        internal bool F_GetTabStop(int pCol, int pRow)
        {
            if (g_GridTabStop == false)
                // 明細部全体をTabStop=Falseの状態にしているので、各コントロールもFalse
                return false;
            else
                // 明細部全体をTrueにしているときは、Cell_Selectableに従う　
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
                // 省略されたら、画面の最後の行まで
                pEdCtlRow = g_MK_Ctl_Row - 1;

            for (w_CtlRow = pStCtlRow; w_CtlRow <= pEdCtlRow; w_CtlRow++)           // 画面上の明細番号
            {
                w_Row = w_CtlRow + pStartRow;                // データの行番号

                // 行番号
                w_CtlCol = (int)ColNO.GYONO;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].GYONO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                // 
                w_CtlCol = (int)ColNO.ITemCD;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ITemCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.JanCD;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].JanCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.ChangeDate;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ChangeDate);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.GenkaWithoutTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].GenkaWithoutTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.ITemName;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ITemName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.PriceWithTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].PriceWithTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.PriceWithoutTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].PriceWithoutTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 休日チェック
                w_CtlCol = (int)ColNO.GeneralPriceWithTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].GeneralPriceWithTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 客数
                w_CtlCol = (int)ColNO.GeneralPriceOutTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].GeneralPriceOutTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 客単価
                w_CtlCol = (int)ColNO.MemberPriceWithTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].MemberPriceWithTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.MemberPriceOutTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].MemberPriceOutTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.ClientPriceWithTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ClientPriceWithTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.ClientPriceOutTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ClientPriceOutTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.SalePriceWithTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SalePriceWithTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.SalePriceOutTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SalePriceOutTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.WebPriceWithTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].WebPriceWithTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.WebPriceOutTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].WebPriceOutTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 備考
                w_CtlCol = (int)ColNO.Remarks;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Remarks);
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
                w_Row = w_CtlRow + pStartRow;                // データの行番号

                // 行番号
                w_CtlCol = (int)ColNO.GYONO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].GYONO);

                // 
                w_CtlCol = (int)ColNO.ITemCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ITemCD);

                // 
                w_CtlCol = (int)ColNO.JanCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].JanCD);

                // 
                w_CtlCol = (int)ColNO.ChangeDate;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ChangeDate);

                // 
                w_CtlCol = (int)ColNO.GenkaWithoutTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].GenkaWithoutTax);
                //g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ROUJIVal);

                // 
                w_CtlCol = (int)ColNO.ITemName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ITemName);

                // 
                w_CtlCol = (int)ColNO.PriceWithTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].PriceWithTax);
                //g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].TANKAVal);

                // 
                w_CtlCol = (int)ColNO.PriceWithoutTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].PriceWithoutTax);
                //g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].KYSU2Val);

                // 
                w_CtlCol = (int)ColNO.GeneralPriceWithTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].GeneralPriceWithTax);
                //g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].TANK2Val);

                // 
                w_CtlCol = (int)ColNO.GeneralPriceOutTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].GeneralPriceOutTax);
                //g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].URIA2Val);

                // 
                w_CtlCol = (int)ColNO.MemberPriceWithTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].MemberPriceWithTax);
                //g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ROJI2Val);

                // 
                w_CtlCol = (int)ColNO.MemberPriceOutTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].MemberPriceOutTax);

                // 
                w_CtlCol = (int)ColNO.ClientPriceWithTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ClientPriceWithTax);

                // 
                w_CtlCol = (int)ColNO.ClientPriceOutTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ClientPriceOutTax);

                // 
                w_CtlCol = (int)ColNO.SalePriceWithTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SalePriceWithTax);

                // 
                w_CtlCol = (int)ColNO.SalePriceOutTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SalePriceOutTax);

                // 
                w_CtlCol = (int)ColNO.WebPriceWithTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].WebPriceWithTax);

                // 
                w_CtlCol = (int)ColNO.WebPriceOutTax;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].WebPriceOutTax);

                // 備考
                w_CtlCol = (int)ColNO.Remarks;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Remarks);
            }
        }

        // 明細部のフォーカス移動
        // pErrSet     行き先がなかった場合にセットする位置
        // pErrDest    指定先にいけなかった場合に前後に探す場合、どちらに移動するか 
        // (pDest=MvSetの場合のみ使用 前後に探さないときはMvsetとする) 
        // pRow        指定先(省略可 pDest=MvSetの場合のみ使用)
        // pCol        指定先(省略可 pDest=MvSetの場合のみ使用)
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
                    F_FocusSet(out int w_CtlCol,out int w_CtlRow,out int w_Value, w_OkCol, w_OkRow, pScrool);

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
