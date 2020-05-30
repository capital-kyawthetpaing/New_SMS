using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IkkatuHacchuuNyuuryoku
{
    /// <summary>
    /// 明細部を構成するコントロールのTagに 行番号(0～) をセットしてください。
    /// 明細部を構成するコントロールのうち TextBoxコントロール以外はカスタムコントロール(MeisaiControl)を使用してください。(Tabの検知のため)
    /// </summary>
    class ClsGridIkkatuHacchuu : GridBase.ClsGridBase
    {
        #region Struct

        internal const int gc_MaxCL = (int)ColNO.COUNT;       // ｸﾞﾘｯﾄﾞ最大列数
        internal const int gc_P_GYO = 5;        // 表示明細行(画面)
        internal const int gMxGyo = 999;        //画面最大明細行数

        #endregion

        #region enum

        /// <summary>
        /// 列番号定数
        /// </summary>
        internal enum ColNO : int
        {
            GyouNO
           , HacchuuNO
           , TaishouFLG
           , SiiresakiCD
           , SiiresakiName
           , ChokusouFLG
           , NetFLG
           , NounyuusakiName
           , NounyuusakiJuusho
           , JuchuuNO
           , SKUCD
           , JANCD
           , ShouhinName
           , BrandName
           , SizeName
           , ColorName
           , HacchuuChuuiZikou
           , EDIFLG
           , MakerShouhinCD
           , KibouNouki
           , ShanaiBikou
           , ShagaiBikou
           , TaniName
           , HacchuuSuu
           , HacchuuTanka
           , Hacchuugaku
           , NounyuusakiYuubinNO1
           , NounyuusakiYuubinNO2
           , NounyuusakiJuusho1
           , NounyuusakiJuusho2
           , NounyuusakiMailAddress
           , NounyuusakiTELNO
           , NounyuusakiFAXNO
           , SoukoCD
           , TaxRate
           , JuchuuRows
           , VariousFLG
           , AdminNO
           , SKUName
           , Teika
           , Kakeritu
           , HacchuuShouhizeigaku
           , TaniCD
           , OrderRows
           , COUNT
        }

        #endregion

        #region 変数

        /// <summary>
        /// 
        /// </summary>
        internal struct ST_DArray_Grid
        {
            // 画面項目
            internal string GyouNO;
            internal string HacchuuNO;
            internal string SiiresakiCD;
            internal string SiiresakiName;
            internal string ChokusouFLG;
            internal string NetFLG;
            internal string NounyuusakiName;
            internal string NounyuusakiJuusho;
            internal string JuchuuNO;
            internal string SKUCD;
            internal string JANCD;
            internal string ShouhinName;
            internal string BrandName;
            internal string SizeName;
            internal string ColorName;
            internal string HacchuuChuuiZikou;
            internal string EDIFLG;
            internal string MakerShouhinCD;
            internal string KibouNouki;
            internal string ShanaiBikou;
            internal string ShagaiBikou;
            internal string TaniName;
            internal string HacchuuSuu;
            internal string HacchuuTanka;
            internal string Hacchuugaku;
            internal string TaishouFLG;

            //隠し項目
            internal string NounyuusakiYuubinNO1;
            internal string NounyuusakiYuubinNO2;
            internal string NounyuusakiJuusho1;
            internal string NounyuusakiJuusho2;
            internal string NounyuusakiMailAddress;
            internal string NounyuusakiTELNO;
            internal string NounyuusakiFAXNO;
            internal string SoukoCD;
            internal string TaxRate;
            internal string JuchuuRows;
            internal string VariousFLG;
            internal string AdminNO;
            internal string SKUName;
            internal string Teika;
            internal string Kakeritu;
            internal string HacchuuShouhizeigaku;
            internal string TaniCD;
            internal string OrderRows;
        }
        /// <summary>
        /// 
        /// </summary>
        internal ST_DArray_Grid[] g_DArray = null;
        /// <summary>
        /// 
        /// </summary>
        internal short g_VSB_Flg;           // ｽｸﾛｰﾙﾊﾞｰのValueChangedｲﾍﾞﾝﾄの実行判断
        /// <summary>
        /// 
        /// </summary>
        internal short g_InMoveFocus_Flg;   // フォーカス移動中判断 
        /// <summary>
        /// 
        /// </summary>
        internal bool g_GridTabStop = true;  // 明細部のTabStopプロパティ (明細部をひとつのコントロールと考えた際のTabStop) 
        /// <summary>
        /// 
        /// </summary>
        internal bool g_WheelFLG = false;    // MouseWheel では True

        #endregion

        #region メソッド

        /// <summary>
        /// 明細部のコントロールのTabStopを、変数の内容から判断して返す
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        /// <returns></returns>
        internal bool F_GetTabStop(int pCol, int pRow)
        {
            if (g_GridTabStop == false)
                // 明細部全体をTabStop=Falseの状態にしているので、各コントロールもFalse
                return false;
            else
                // 明細部全体をTrueにしているときは、Cell_Selectableに従う　
                return g_MK_State[pCol, pRow].Cell_Selectable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStartRow">行</param>
        /// <param name="pScrool">スクロールバー</param>
        /// <param name="pStCtlRow"></param>
        /// <param name="pEdCtlRow"></param>
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

                //行番号
                w_CtlCol = (int)ColNO.GyouNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].GyouNO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //発注番号
                w_CtlCol = (int)ColNO.HacchuuNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].HacchuuNO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //仕入先コード
                w_CtlCol = (int)ColNO.SiiresakiCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SiiresakiCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //仕入先名
                w_CtlCol = (int)ColNO.SiiresakiName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SiiresakiName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //直送
                w_CtlCol = (int)ColNO.ChokusouFLG;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ChokusouFLG);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //Net
                w_CtlCol = (int)ColNO.NetFLG;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NetFLG);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //納入先
                w_CtlCol = (int)ColNO.NounyuusakiName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NounyuusakiName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //納入先住所
                w_CtlCol = (int)ColNO.NounyuusakiJuusho;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NounyuusakiJuusho);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //受注番号
                w_CtlCol = (int)ColNO.JuchuuNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].JuchuuNO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //SKUCD
                w_CtlCol = (int)ColNO.SKUCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SKUCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //JANCD
                w_CtlCol = (int)ColNO.JANCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].JANCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //商品名
                w_CtlCol = (int)ColNO.ShouhinName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ShouhinName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //ブランド
                w_CtlCol = (int)ColNO.BrandName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].BrandName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //サイズ
                w_CtlCol = (int)ColNO.SizeName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SizeName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //カラー
                w_CtlCol = (int)ColNO.ColorName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ColorName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                
                //発注注意事項
                w_CtlCol = (int)ColNO.HacchuuChuuiZikou;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].HacchuuChuuiZikou);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //EDI
                w_CtlCol = (int)ColNO.EDIFLG;
                if (g_DArray[w_Row].EDIFLG == "True")
                {
                    ((CKM_Controls.CKM_CheckBox)g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl).Checked = true;
                }
                else
                {
                    ((CKM_Controls.CKM_CheckBox)g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl).Checked = false;
                }
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].EDIFLG);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //メーカー商品コード
                w_CtlCol = (int)ColNO.MakerShouhinCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].MakerShouhinCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //希望納期
                w_CtlCol = (int)ColNO.KibouNouki;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].KibouNouki);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //社内備考・コメント
                w_CtlCol = (int)ColNO.ShanaiBikou;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ShanaiBikou);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //社外備考・コメント
                w_CtlCol = (int)ColNO.ShagaiBikou;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ShagaiBikou);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //単位
                w_CtlCol = (int)ColNO.TaniName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].TaniName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //発注数
                w_CtlCol = (int)ColNO.HacchuuSuu;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].HacchuuSuu);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //発注単価
                w_CtlCol = (int)ColNO.HacchuuTanka;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].HacchuuTanka);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //発注額
                w_CtlCol = (int)ColNO.Hacchuugaku;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Hacchuugaku);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //対象
                w_CtlCol = (int)ColNO.TaishouFLG;
                if (g_DArray[w_Row].TaishouFLG == "True")
                {
                    ((CKM_Controls.CKM_CheckBox)g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl).Checked = true;
                }
                else
                {
                    ((CKM_Controls.CKM_CheckBox)g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl).Checked = false;
                }
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].TaishouFLG);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //納入先郵便番号1
                w_CtlCol = (int)ColNO.NounyuusakiYuubinNO1;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NounyuusakiYuubinNO1);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //納入先郵便番号2
                w_CtlCol = (int)ColNO.NounyuusakiYuubinNO2;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NounyuusakiYuubinNO2);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //納入先住所1
                w_CtlCol = (int)ColNO.NounyuusakiJuusho1;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NounyuusakiJuusho1);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //納入先住所2
                w_CtlCol = (int)ColNO.NounyuusakiJuusho2;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NounyuusakiJuusho2);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //納入先メールアドレス
                w_CtlCol = (int)ColNO.NounyuusakiMailAddress;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NounyuusakiMailAddress);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //納入先TELNO
                w_CtlCol = (int)ColNO.NounyuusakiTELNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NounyuusakiTELNO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //納入先FAXNO
                w_CtlCol = (int)ColNO.NounyuusakiFAXNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NounyuusakiFAXNO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //倉庫コード
                w_CtlCol = (int)ColNO.SoukoCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SoukoCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //消費税率
                w_CtlCol = (int)ColNO.TaxRate;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].TaxRate);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //受注明細連番
                w_CtlCol = (int)ColNO.JuchuuRows;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].JuchuuRows);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //VariousFLG
                w_CtlCol = (int)ColNO.VariousFLG;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].VariousFLG);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //AdminNO
                w_CtlCol = (int)ColNO.AdminNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].AdminNO);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //SKUName
                w_CtlCol = (int)ColNO.SKUName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SKUName);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //定価
                w_CtlCol = (int)ColNO.Teika;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Teika);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //掛率
                w_CtlCol = (int)ColNO.Kakeritu;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Kakeritu);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //消費税額
                w_CtlCol = (int)ColNO.HacchuuShouhizeigaku;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].HacchuuShouhizeigaku);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //単位コード
                w_CtlCol = (int)ColNO.TaniCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].TaniCD);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御

                //OrderRows
                w_CtlCol = (int)ColNO.OrderRows;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].OrderRows);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStartRow"></param>
        internal void S_DispToArray(int pStartRow)
        {
            int w_Row;
            int w_CtlRow;
            int w_CtlCol;

            for (w_CtlRow = 0; w_CtlRow <= g_MK_Ctl_Row - 1; w_CtlRow++)            // 画面上の明細番号
            {
                w_Row = w_CtlRow + pStartRow;                // データの行番号

                // 行番号
                w_CtlCol = (int)ColNO.GyouNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].GyouNO);

                // 発注番号
                w_CtlCol = (int)ColNO.HacchuuNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].HacchuuNO);

                // 仕入先コード
                w_CtlCol = (int)ColNO.SiiresakiCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SiiresakiCD);

                // 仕入先名
                w_CtlCol = (int)ColNO.SiiresakiName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SiiresakiName);

                // 直送
                w_CtlCol = (int)ColNO.ChokusouFLG;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ChokusouFLG);

                // Net
                w_CtlCol = (int)ColNO.NetFLG;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NetFLG);

                // 納入先
                w_CtlCol = (int)ColNO.NounyuusakiName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NounyuusakiName);

                // 納入先住所
                w_CtlCol = (int)ColNO.NounyuusakiJuusho;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NounyuusakiJuusho);

                // 受注番号
                w_CtlCol = (int)ColNO.JuchuuNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].JuchuuNO);

                // SKUCD
                w_CtlCol = (int)ColNO.SKUCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SKUCD);

                // JANCD
                w_CtlCol = (int)ColNO.JANCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].JANCD);

                // 商品名
                w_CtlCol = (int)ColNO.ShouhinName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ShouhinName);

                // ブランド
                w_CtlCol = (int)ColNO.BrandName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].BrandName);

                // サイズ
                w_CtlCol = (int)ColNO.SizeName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SizeName);

                // カラー
                w_CtlCol = (int)ColNO.ColorName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ColorName);

                // 発注注意事項
                w_CtlCol = (int)ColNO.HacchuuChuuiZikou;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].HacchuuChuuiZikou);

                // EDI
                w_CtlCol = (int)ColNO.EDIFLG;
                if (((CKM_Controls.CKM_CheckBox)g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl).Checked == true)
                {
                    g_DArray[w_Row].EDIFLG = "True";
                }
                else
                {
                    g_DArray[w_Row].EDIFLG = "False";
                }

                // メーカー商品コード
                w_CtlCol = (int)ColNO.MakerShouhinCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].MakerShouhinCD);

                // 希望納期
                w_CtlCol = (int)ColNO.KibouNouki;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].KibouNouki);

                // 社内備考・コメント
                w_CtlCol = (int)ColNO.ShanaiBikou;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ShanaiBikou);

                // 社外備考・コメント
                w_CtlCol = (int)ColNO.ShagaiBikou;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ShagaiBikou);

                // 単位
                w_CtlCol = (int)ColNO.TaniName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].TaniName);

                // 発注数
                w_CtlCol = (int)ColNO.HacchuuSuu;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].HacchuuSuu);

                // 発注単価
                w_CtlCol = (int)ColNO.HacchuuTanka;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].HacchuuTanka);

                // 発注額
                w_CtlCol = (int)ColNO.Hacchuugaku;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Hacchuugaku);

                // 対象
                w_CtlCol = (int)ColNO.TaishouFLG;
                if (((CKM_Controls.CKM_CheckBox)g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl).Checked == true)
                {
                    g_DArray[w_Row].TaishouFLG = "True";
                }
                else
                {
                    g_DArray[w_Row].TaishouFLG = "False";
                }

                // 納入先郵便番号1
                w_CtlCol = (int)ColNO.NounyuusakiYuubinNO1;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NounyuusakiYuubinNO1);

                // 納入先郵便番号2
                w_CtlCol = (int)ColNO.NounyuusakiYuubinNO2;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NounyuusakiYuubinNO2);

                // 納入先住所1
                w_CtlCol = (int)ColNO.NounyuusakiJuusho1;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NounyuusakiJuusho1);

                // 納入先住所2
                w_CtlCol = (int)ColNO.NounyuusakiJuusho2;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NounyuusakiJuusho2);

                // 納入先メールアドレス
                w_CtlCol = (int)ColNO.NounyuusakiMailAddress;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NounyuusakiMailAddress);

                // 納入先TELNO
                w_CtlCol = (int)ColNO.NounyuusakiTELNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NounyuusakiTELNO);

                // 納入先FAXNO
                w_CtlCol = (int)ColNO.NounyuusakiFAXNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NounyuusakiFAXNO);

                // 倉庫コード
                w_CtlCol = (int)ColNO.SoukoCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SoukoCD);

                // 消費税率
                w_CtlCol = (int)ColNO.TaxRate;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].TaxRate);

                // 受注明細連番
                w_CtlCol = (int)ColNO.JuchuuRows;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].JuchuuRows);

                // VariousFLG
                w_CtlCol = (int)ColNO.VariousFLG;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].VariousFLG);

                // AdminNO
                w_CtlCol = (int)ColNO.AdminNO;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].AdminNO);

                // SKUName
                w_CtlCol = (int)ColNO.SKUName;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SKUName);

                // 定価
                w_CtlCol = (int)ColNO.Teika;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Teika);

                // 掛率
                w_CtlCol = (int)ColNO.Kakeritu;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Kakeritu);

                // 消費税額
                w_CtlCol = (int)ColNO.HacchuuShouhizeigaku;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].HacchuuShouhizeigaku);

                // 単位コード
                w_CtlCol = (int)ColNO.TaniCD;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].TaniCD);

                // OrderRows
                w_CtlCol = (int)ColNO.OrderRows;
                g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].OrderRows);
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
                    else if (g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.GetType().Name.ToUpper() == "BUTTON")
                    {
                    }
                    else
                    {
                        g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.BackColor = GridBase.ClsGridBase.BKColor;
                    }
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
                    {
                        pErrSet.BackColor = GridBase.ClsGridBase.BKColor;
                    }
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
                    {
                        w_MotoControl.BackColor = GridBase.ClsGridBase.BKColor;
                    }
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
                {
                    w_MotoControl.BackColor = GridBase.ClsGridBase.BKColor;
                }
            }

            finally
            {
                g_InMoveFocus_Flg = 0;
            }

            return ret;
        }

        #endregion
    }

    #region 明細レコードクラス

    /// <summary>
    /// 明細１レコードの定義
    /// </summary>
    public class MeisaiRecord
    {
        public string GyouNO { get; set; }
        public string HacchuuNO { get; set; }
        public string SiiresakiCD { get; set; }
        public string SiiresakiName { get; set; }
        public string ChokusouFLG { get; set; }
        public string NetFLG { get; set; }
        public string NounyuusakiName { get; set; }
        public string NounyuusakiJuusho { get; set; }
        public string JuchuuNO { get; set; }
        public string SKUCD { get; set; }
        public string JANCD { get; set; }
        public string ShouhinName { get; set; }
        public string BrandName { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string HacchuuChuuiZikou { get; set; }
        public string EDIFLG { get; set; }
        public string MakerShouhinCD { get; set; }
        public string KibouNouki { get; set; }
        public string ShanaiBikou { get; set; }
        public string ShagaiBikou { get; set; }
        public string TaniName { get; set; }
        public string HacchuuSuu { get; set; }
        public string HacchuuTanka { get; set; }
        public string Hacchuugaku { get; set; }
        public string TaishouFLG { get; set; }
        public string NounyuusakiYuubinNO1 { get; set; }
        public string NounyuusakiYuubinNO2 { get; set; }
        public string NounyuusakiJuusho1 { get; set; }
        public string NounyuusakiJuusho2 { get; set; }
        public string NounyuusakiMailAddress { get; set; }
        public string NounyuusakiTELNO { get; set; }
        public string NounyuusakiFAXNO { get; set; }
        public string SoukoCD { get; set; }
        public string TaxRate { get; set; }
        public string JuchuuRows { get; set; }
        public string VariousFLG { get; set; }
        public string AdminNO { get; set; }
        public string SKUName { get; set; }
        public string Teika { get; set; }
        public string Kakeritu { get; set; }
        public string HacchuuShouhizeigaku { get; set; }
        public string TaniCD { get; set; }
        public string OrderRows { get; set; }
    }

    #endregion
}
