using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkDownNyuuryoku
{
//'明細部を構成するコントロールのTagに 行番号(0～) をセットしてください。
//'明細部を構成するコントロールのうち TextBoxコントロール以外はカスタムコントロール(MeisaiControl)を使用してください。(Tabの検知のため)
    class ClsGridMarkDown :  GridBase.ClsGridBase
    {
        internal struct ST_DArray_Grid
        {
            // 画面項目
            internal string GYONO;      // №
            internal bool Chk;
            
            internal string JanCD;                  //JANCD
            internal string ITEMCD;                 //ITEMCD
            internal string SKUName;                //商品名
            internal string ColorName;              //カラー
            internal string SizeName;               //サイズ 
            internal string MakerItem;              //メーカー商品CD
            internal string SKUCD;                  //SKUCD
            internal string PriceOutTax;            //定価（税抜）
            internal string EvaluationPrice;        //最新原価＠
            internal string Rate;                   //掛率
            internal string MarkDownUnitPrice;      //MD変更＠
            internal string MarkDownSagakuPrice;    //MD差額＠
            internal string StockSu;                //在庫数
            internal string CalculationSu;          //対象数
            internal string MarkDownGaku;           //仕入予定額


            //隠し項目
            internal int MDPurchaseRows;                //マークダウン明細連番
            internal int PurchaseRows;                　//仕入明細連番
            internal string AdminNO;                    //AdminNO
            internal decimal NormalCost;                //標準原価
            internal decimal TaxRate;                   //税率
            internal decimal Tax;                   　　//税額
            internal string TaniCD;                     //単位CD
            internal string TaniName;                   //単位名
            internal string InsertOperator;             //新規登録者
            internal string InsertDateTime;             //新規登録日時
            internal string OldEvaluationPrice;         //最新原価＠（退避）
            internal string OldRate;                    //掛率（退避）
            internal string OldMarkDownUnitPrice;       //MD変更＠（退避）
            internal string OldCalculationSu;           //対象数（退避）
            internal string OldMarkDownGaku;           //仕入予定額（退避）
        }

        //列番号定数
        internal enum ColNO : int
        {
            GYONO,
            Chk,

            JanCD,
            ITEMCD,
            SKUName,
            ColorName,          //カラー
            SizeName,           //サイズ
            MakerItem,
            SKUCD,
            PriceOutTax,
            EvaluationPrice,
            Rate,
            MarkDownUnitPrice,
            MarkDownSagakuPrice,
            StockSu,
            CalculationSu,
            MarkDownGaku,

            COUNT
        }
        internal ST_DArray_Grid[] g_DArray = null;
        internal const int gc_MaxCL = (int)ColNO.COUNT;     // ｸﾞﾘｯﾄﾞ最大列数
        internal const int gc_P_GYO = 11;                   // 表示明細行(画面)
        internal const int gMxGyo = 999;                    //画面最大明細行数

        internal short g_VSB_Flg;               // ｽｸﾛｰﾙﾊﾞｰのValueChangedｲﾍﾞﾝﾄの実行判断
        internal short g_InMoveFocus_Flg;       // フォーカス移動中判断 
        internal bool g_GridTabStop = true;     // 明細部のTabStopプロパティ (明細部をひとつのコントロールと考えた際のTabStop) 
        internal bool g_WheelFLG = false;       // MouseWheel では True

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

                w_CtlCol = (int)ColNO.Chk;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Chk);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           //TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // JANCD
                w_CtlCol = (int)ColNO.JanCD;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].JanCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // ITEMCD
                w_CtlCol = (int)ColNO.ITEMCD;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ITEMCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 商品名
                w_CtlCol = (int)ColNO.SKUName;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SKUName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // カラー
                w_CtlCol = (int)ColNO.ColorName;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ColorName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // サイズ
                w_CtlCol = (int)ColNO.SizeName;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SizeName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // メーカー商品CD
                w_CtlCol = (int)ColNO.MakerItem;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].MakerItem);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // SKUCD
                w_CtlCol = (int)ColNO.SKUCD;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SKUCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 定価（税抜）
                w_CtlCol = (int)ColNO.PriceOutTax;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].PriceOutTax);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 最新原価＠
                w_CtlCol = (int)ColNO.EvaluationPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].EvaluationPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 掛率
                w_CtlCol = (int)ColNO.Rate;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Rate);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // MD変更＠
                w_CtlCol = (int)ColNO.MarkDownUnitPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].MarkDownUnitPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // MD差額
                w_CtlCol = (int)ColNO.MarkDownSagakuPrice;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].MarkDownSagakuPrice);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);


                // 在庫数
                w_CtlCol = (int)ColNO.StockSu;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].StockSu);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 対象数
                w_CtlCol = (int)ColNO.CalculationSu;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].CalculationSu);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 仕入予定額
                w_CtlCol = (int)ColNO.MarkDownGaku;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].MarkDownGaku);
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

            for (int w_CtlRow = 0; w_CtlRow <= g_MK_Ctl_Row - 1; w_CtlRow++)            // 画面上の明細番号
            {
                w_Row = w_CtlRow + pStartRow;                // データの行番号

                for (int w_CtlCol = 0; w_CtlCol <= g_MK_Ctl_Col - 1; w_CtlCol++)
                {
                    switch (w_CtlCol)
                    {
                        // 行番号
                        case (int)ColNO.GYONO:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].GYONO);
                            break;
                        //削除チェック
                        case (int)ColNO.Chk:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Chk);
                            break;
                        // JANCD
                        case (int)ColNO.JanCD:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].JanCD);
                            break;
                        // 商品名
                        case (int)ColNO.SKUName:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SKUName);
                            break;
                        // カラー
                        case (int)ColNO.ColorName:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ColorName);
                            break;
                        // サイズ
                        case (int)ColNO.SizeName:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SizeName);
                            break;
                        //メーカー商品CD
                        case (int)ColNO.MakerItem:     
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].MakerItem);
                            break;
                        // SKUCD
                        case (int)ColNO.SKUCD:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SKUCD);
                            break;
                        // 定価（税抜）
                        case (int)ColNO.PriceOutTax:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].PriceOutTax);
                            break;
                        // 最新原価＠
                        case (int)ColNO.EvaluationPrice:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].EvaluationPrice);
                            break;
                        // 掛率
                        case (int)ColNO.Rate:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Rate);
                            break;
                        // MD変更＠
                        case (int)ColNO.MarkDownUnitPrice:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].MarkDownUnitPrice);
                            break;
                        // MD差額
                        case (int)ColNO.MarkDownSagakuPrice:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].MarkDownSagakuPrice);
                            break;
                        // 在庫数
                        case (int)ColNO.StockSu:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].StockSu);
                            break;
                        // 対象数
                        case (int)ColNO.CalculationSu:     
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].CalculationSu);
                            break;
                        // 仕入予定額
                        case (int)ColNO.MarkDownGaku:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].MarkDownGaku);
                            break;
                    }
                }
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
