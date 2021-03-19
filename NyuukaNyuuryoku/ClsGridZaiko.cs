using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NyuukaNyuuryoku
{
//'明細部を構成するコントロールのTagに 行番号(0～) をセットしてください。
//'明細部を構成するコントロールのうち TextBoxコントロール以外はカスタムコントロール(MeisaiControl)を使用してください。(Tabの検知のため)
    class ClsGridZaiko :  GridBase.ClsGridBase
    {
        internal struct ST_DArray_Grid
        {
            // 画面項目
            internal string GYONO;      // №
            internal bool Check;
            internal string Number;      // 
            internal string RowNo;     // 
            internal string SURYO;      //入荷数
            internal string OrderSu;      //
            internal string ReserveSu;      //予定数
            internal string DirectFlg;      //
            internal string DeliveryPlanDate;
            internal string Customer;
            internal bool ChkFinish;

            //隠し項目
            internal string CustomerCD;//(Hidden)
            internal string VendorCD;//(Hidden)   更新に必要
            internal string VendorName;//(Hidden)  
            internal string ArrivalPlanNO;    //(Hidden)
            internal string StockNO;        //(Hidden)
            internal string ReserveNO;
            internal string ArrivalPlanKBN;

            //COPY時に必要
            internal string OrderUnitPrice;
            internal string PriceOutTax;
            internal string Rate;
            internal string TaniCD;
            internal string OrderTaxRitsu;
            internal string OrderWayKBN;
            internal string AliasKBN;
            internal string OldSURYO;      //修正前入荷数
            internal bool isCopy;
        }

        //列番号定数
        internal enum ColNO : int
        {
            GYONO,
            Check,
            Number,
            RowNo,    //
            Customer,    //  
            OrderSu,    //
            ReserveSu,      // 引当数
            SURYO,           // 入荷数
            DirectFlg,                 // 単位 
            DeliveryPlanDate,     //出荷予定日
            ChkFinish,
            COUNT
        }
        internal ST_DArray_Grid[] g_DArray=null;
        internal const int gc_MaxCL = (int)ColNO.COUNT;       // ｸﾞﾘｯﾄﾞ最大列数
        internal const int gc_P_GYO = 3;        // 表示明細行(画面)
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
                w_CtlCol = (int)ColNO.Number;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Number);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBoldForSmallLarge(g_MK_State[w_CtlCol, w_Row].Cell_Bold);


                //発注数
                w_CtlCol = (int)ColNO.SURYO;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SURYO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBoldForSmallLarge(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 販売単価
                w_CtlCol = (int)ColNO.OrderSu;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].OrderSu);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBoldForSmallLarge(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 単位
                w_CtlCol = (int)ColNO.DirectFlg;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].DirectFlg);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBoldForSmallLarge(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 発注額
                w_CtlCol = (int)ColNO.ReserveSu;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ReserveSu);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBoldForSmallLarge(g_MK_State[w_CtlCol, w_Row].Cell_Bold);


                w_CtlCol = (int)ColNO.Check;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Check);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           //TABSTOP制御

                // 
                w_CtlCol = (int)ColNO.Customer;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Customer);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBoldForSmallLarge(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                //

                w_CtlCol = (int)ColNO.DeliveryPlanDate;    //入荷予定日

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].DeliveryPlanDate);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBoldForSmallLarge(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                w_CtlCol = (int)ColNO.RowNo;    //支払予定日

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].RowNo);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBoldForSmallLarge(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                w_CtlCol = (int)ColNO.ChkFinish;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ChkFinish);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           //TABSTOP制御
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

                ////削除チェック
                //w_CtlCol = (int)ColNO.ChkDel;
                //g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].DELCK);

                //EDIチェック
                w_CtlCol = (int)ColNO.Check;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Check);
                // 
                w_CtlCol = (int)ColNO.Number;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Number);

                // 
                w_CtlCol = (int)ColNO.SURYO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SURYO);

                // 
                w_CtlCol = (int)ColNO.OrderSu;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].OrderSu);

                // 
                w_CtlCol = (int)ColNO.DirectFlg;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].DirectFlg);

                // 
                w_CtlCol = (int)ColNO.ReserveSu;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ReserveSu);

                // 
                w_CtlCol = (int)ColNO.Customer;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Customer);

                w_CtlCol = (int)ColNO.DeliveryPlanDate;     
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].DeliveryPlanDate);

                w_CtlCol = (int)ColNO.ChkFinish;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ChkFinish);

                w_CtlCol = (int)ColNO.RowNo;    
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].RowNo);

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
