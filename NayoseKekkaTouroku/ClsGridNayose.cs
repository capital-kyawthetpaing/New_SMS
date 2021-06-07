using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NayoseKekkaTouroku
{
//'明細部を構成するコントロールのTagに 行番号(0～) をセットしてください。
//'明細部を構成するコントロールのうち TextBoxコントロール以外はカスタムコントロール(MeisaiControl)を使用してください。(Tabの検知のため)
    class ClsGridNayose :  GridBase.ClsGridBase
    {
        internal struct ST_DArray_Grid
        {
            // 画面項目
            internal string GYONO;      // №
            internal bool ChkNayose;

            internal string SiteNm;
            internal string NAME;
            internal string MAIL;      // 
            internal string TEL;     //
            internal string ZIP;
            internal string ADD11;      // 
            internal string ADD12;
            internal string Client;      //
            internal string NAME2;// 
            internal string TEL2;      // 
            internal string ZIP2;// 
            internal string ADD21;//
            internal string ADD22;// 
            internal string MAIL2;
            
            internal string Space1;      //
            internal string Space2;      //

            //隠し項目
            internal string AttentionFLG;        //M_Customer.AttentionFLG
            internal string JuchuuNO;
            internal bool ChkOldNayose;
        }

        //列番号定数
        internal enum ColNO : int
        {
            GYONO,
            ChkNayose,

            SiteNm,
            NAME,
            ZIP,
            TEL,    //
            Space1,
            MAIL,    //
            ADD11,
            ADD12,
            Client,
            NAME2,
            ZIP2,
            TEL2,
            Space2,
            MAIL2,    //
            ADD21,
            ADD22,
            

            COUNT
        }
        internal ST_DArray_Grid[] g_DArray=null;
        internal const int gc_MaxCL = (int)ColNO.COUNT;       // ｸﾞﾘｯﾄﾞ最大列数
        internal const int gc_P_GYO = 6;        // 表示明細行(画面)
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
                
                w_CtlCol = (int)ColNO.NAME;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NAME);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                w_CtlCol = (int)ColNO.SiteNm;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].SiteNm);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                w_CtlCol = (int)ColNO.TEL;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].TEL);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                w_CtlCol = (int)ColNO.ZIP;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ZIP);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 
                w_CtlCol = (int)ColNO.ADD11;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ADD11);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                // 
                w_CtlCol = (int)ColNO.ADD12;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ADD12);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                // 備考
                w_CtlCol = (int)ColNO.MAIL;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].MAIL);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                w_CtlCol = (int)ColNO.NAME2;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].NAME2);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SForeColor(F_GetForeColor_MK(w_CtlCol, w_Row));   //赤字になる場合
                // 
                w_CtlCol = (int)ColNO.MAIL2;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].MAIL2);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SForeColor(F_GetForeColor_MK(w_CtlCol, w_Row));   //赤字になる場合

                w_CtlCol = (int)ColNO.ZIP2;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ZIP2);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SForeColor(F_GetForeColor_MK(w_CtlCol, w_Row));   //赤字になる場合
                // 
                w_CtlCol = (int)ColNO.ADD21;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ADD21);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SForeColor(F_GetForeColor_MK(w_CtlCol, w_Row));   //赤字になる場合
                // 
                w_CtlCol = (int)ColNO.ADD22;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ADD22);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SForeColor(F_GetForeColor_MK(w_CtlCol, w_Row));   //赤字になる場合

                // 
                w_CtlCol = (int)ColNO.TEL2;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].TEL2);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SForeColor(F_GetForeColor_MK(w_CtlCol, w_Row));   //赤字になる場合

                // 
                w_CtlCol = (int)ColNO.Client;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Client);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SForeColor(F_GetForeColor_MK(w_CtlCol, w_Row));   //赤字になる場合

                w_CtlCol = (int)ColNO.ChkNayose;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].ChkNayose);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           //TABSTOP制御

                // 
                w_CtlCol = (int)ColNO.Space1;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Space1);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                w_CtlCol = (int)ColNO.Space2;

                g_MK_Ctrl[w_CtlCol, w_CtlRow].SVal(g_DArray[w_Row].Space2);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SEnabled(g_MK_State[w_CtlCol, w_Row].Cell_Enabled);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SReadOnly(g_MK_State[w_CtlCol, w_Row].Cell_ReadOnly);
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SDisabledBackColor(F_GetBackColor_MK(w_CtlCol, w_Row));
                g_MK_Ctrl[w_CtlCol, w_CtlRow].CellCtl.TabStop = F_GetTabStop(w_CtlCol, w_Row);           // TABSTOP制御
                g_MK_Ctrl[w_CtlCol, w_CtlRow].SBold(g_MK_State[w_CtlCol, w_Row].Cell_Bold);

                //
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
                        case (int)ColNO.NAME:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NAME);
                            break;
                        case (int)ColNO.MAIL:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].MAIL);
                            break;
                        case (int)ColNO.ZIP:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ZIP);
                            break;
                        case (int)ColNO.SiteNm:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].SiteNm);
                            break;
                        case (int)ColNO.ChkNayose:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ChkNayose);
                            break;
                        // 
                        case (int)ColNO.ADD11:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ADD11);
                            break;
                        // 
                        case (int)ColNO.ADD12:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ADD12);
                            break;
                        // 
                        case (int)ColNO.TEL:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].TEL);
                            break;
                        // 
                        case (int)ColNO.Client:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Client);
                            break;
                        // 
                        case (int)ColNO.NAME2:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].NAME2);
                            break;
                        case (int)ColNO.MAIL2:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].MAIL2);
                            break;
                        case (int)ColNO.ZIP2:     //メーカー商品CD
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ZIP2);
                            break;
                        case (int)ColNO.TEL2:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].TEL2);
                            break;
                        case (int)ColNO.ADD21:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ADD21);
                            break;
                        case (int)ColNO.ADD22:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].ADD22);
                            break;
                        // 
                        case (int)ColNO.Space1:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Space1);
                            break;

                        case (int)ColNO.Space2:
                            g_MK_Ctrl[w_CtlCol, w_CtlRow].GVal(out g_DArray[w_Row].Space2);
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
