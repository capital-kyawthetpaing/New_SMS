using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NyuukinNyuuryoku_Detail
{
//'明細部を構成するコントロールのTagに 行番号(0～) をセットしてください。
//'明細部を構成するコントロールのうち TextBoxコントロール以外はカスタムコントロール(MeisaiControl)を使用してください。(Tabの検知のため)
    class ClsGridNyuukin_S:  GridBase.ClsGridBase
    {
        internal struct ST_DArray_Grid
        {
            // 画面項目
            internal bool Chk;
            internal string GYONO;      // №
            internal string BillingDate;     //発注日
            internal string BillingNo;      // 
            internal string Store;
            internal string SalesDate;      // 
            internal string SalesNO;      //
            internal string JuchuuNO;      //

            internal string CustomerCD;
            internal string CustomerName;      // 
            internal string BillingGaku;      // 
            internal string CollectAmount;      //入金済額
            internal string ConfirmAmount;      //今回入金額＝消込額
            internal string Minyukin;

            internal string SKUCD;      // 
            internal string SKUName;      //
            internal string CommentInStore;      // 

            //隠し項目
            internal string CollectPlanNO;
            internal string CollectPlanRows;
            internal string WebCollectNO;
            internal string WebCollectType;
            internal string OldConfirmAmount;      //今回入金額

            internal int Update;
        }

        //列番号定数
        internal enum ColNO : int
        {
            GYONO,
            Chk,
            BillingDate,    //請求日
            BillingNo,
            Store,
            SalesDate,

            SalesNO,
            JuchuuNO,

            CustomerCD,
            CustomerName,
            BillingGaku,           //
            CollectAmount,          //入金済額
            ConfirmAmount,          //今回入金額＝消込額
            Minyukin,     //

            SKUCD,    //
            SKUName,    // 
            CommentInStore,    //  備考

            COUNT
        }
        internal ST_DArray_Grid[] g_DArray=null;
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
            BL.Base_BL bbl = new BL.Base_BL();

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
                w_CtlCol = (int)ColNO.BillingNo;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].BillingNo);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.Store;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Store);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.SalesDate;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SalesDate);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.BillingGaku;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].BillingGaku);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.CustomerCD;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].CustomerCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.CustomerName;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].CustomerName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.ConfirmAmount;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ConfirmAmount);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御    
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
              
                //マイナスPrice対応
                if (bbl.Z_Set(g_DArray[w_Row].ConfirmAmount) < 0)
                {
                    g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.ForeColor = System.Drawing.SystemColors.WindowText;
                }

                // 販売単価
                w_CtlCol = (int)ColNO.JuchuuNO;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].JuchuuNO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 単位
                w_CtlCol = (int)ColNO.SalesNO;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SalesNO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.CollectAmount;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].CollectAmount);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                w_CtlCol = (int)ColNO.Chk;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Chk);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           //TABSTOP制御

                // 
                w_CtlCol = (int)ColNO.SKUName;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SKUName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.CommentInStore;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].CommentInStore);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 備考
                w_CtlCol = (int)ColNO.SKUCD;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SKUCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                //

                w_CtlCol = (int)ColNO.Minyukin;    //入荷予定日

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Minyukin);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                w_CtlCol = (int)ColNO.BillingDate;    //支払予定日

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].BillingDate);
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

                //チェック
                w_CtlCol = (int)ColNO.Chk;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Chk);

                // 
                w_CtlCol = (int)ColNO.BillingNo;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].BillingNo);

                // 
                w_CtlCol = (int)ColNO.SalesDate;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SalesDate);

                // 
                w_CtlCol = (int)ColNO.BillingGaku;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].BillingGaku);

                // 
                w_CtlCol = (int)ColNO.CustomerCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].CustomerCD);

                // 
                w_CtlCol = (int)ColNO.CustomerName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].CustomerName);

                // 
                w_CtlCol = (int)ColNO.ConfirmAmount;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ConfirmAmount);
                
                // 
                w_CtlCol = (int)ColNO.JuchuuNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].JuchuuNO);

                // 
                w_CtlCol = (int)ColNO.SalesNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SalesNO);

                // 
                w_CtlCol = (int)ColNO.CollectAmount;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].CollectAmount);

                // 
                w_CtlCol = (int)ColNO.SKUName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SKUName);

                // 
                w_CtlCol = (int)ColNO.CommentInStore;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].CommentInStore);

                // 備考
                w_CtlCol = (int)ColNO.SKUCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SKUCD);

                w_CtlCol = (int)ColNO.Store;      //メーカー商品CD
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Store);

                w_CtlCol = (int)ColNO.Minyukin;     //希望納期
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Minyukin);

                w_CtlCol = (int)ColNO.BillingDate;    //入荷予定日
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].BillingDate);

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
                    else if (g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.GetType().Name.ToUpper() == "BUTTON")
                    { }
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
                    else if (pErrSet.GetType().Name.ToUpper() == "BUTTON")
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
                    else if (w_MotoControl.GetType().Name.ToUpper() == "BUTTON")
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
                else if (w_MotoControl.GetType().Name.ToUpper() == "BUTTON")
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
