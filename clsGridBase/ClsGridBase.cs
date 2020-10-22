using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GridBase
{
    public class ClsGridBase
    {
        public static Color BKColor = Color.FromArgb(255, 242, 204);
        public static Color GrayColor = Color.Silver;
        public static Color GridColor = Color.FromArgb(221, 235, 247);
        public static Color WHColor = Color.White;
        public static Color MaxGyoColor = Color.Silver;
        public static Color CheckColor = Color.Orange;    //PES [2020/10/22] 

        // マウスホイールの1移動量  Windows の定数 WHEEL_DELTA (値 120) 
        // 取得方法がわからないため、とりあえず固定(初期値)で120にし、READONLYにしていますが
        // 取得方法がわかりましたら、この変数に取得した値をセットする処理を作成し
        // 何らかのタイミング(KIDOUやKYOTUの必ずとおる関数でセット処理を呼出など)でセットするように変更してください。
        public readonly int g_WHEEL_DELTA = 120;

        public class CRS_CTRL
        {
            public System.Windows.Forms.Control CellCtl;

            public void GVal(out string pVal)
            {
                pVal = "";

                try
                {
                    if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    {
                        CKM_Controls.CKM_TextBox w_Edit;
                        w_Edit = (CKM_Controls.CKM_TextBox)CellCtl;
                        pVal = w_Edit.Text;
                    }
                    else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_MultiLineTextBox)))
                    {
                        CKM_Controls.CKM_MultiLineTextBox w_Edit;
                        w_Edit = (CKM_Controls.CKM_MultiLineTextBox)CellCtl;
                        pVal = w_Edit.Text;
                    }
                    else if (CellCtl.GetType().Equals(typeof(System.Windows.Forms.Label)))
                    {
                        System.Windows.Forms.Label w_Label;
                        w_Label = (System.Windows.Forms.Label)CellCtl;
                        pVal = w_Label.Text;
                    }
                    else if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                    {
                        Search.CKM_SearchControl w_Edit;
                        w_Edit = (Search.CKM_SearchControl)CellCtl;
                        pVal = w_Edit.TxtCode.Text;
                    }
                    else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                    {
                        CKM_Controls.CKM_ComboBox w_Edit;
                        w_Edit = (CKM_Controls.CKM_ComboBox)CellCtl;

                        if (w_Edit.SelectedIndex > 0)
                            pVal = w_Edit.SelectedValue.ToString();
                        else
                            pVal = "";
                    }
                }

                catch(Exception ex)
                {
                    var mes = ex.Message;
                }
            }

            public void GVal(out bool pVal)
            {
                pVal = false;
                GridControl.clsGridCheckBox w_Check;

                if (CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                //チェックボックス　のとき
                {
                    w_Check = (GridControl.clsGridCheckBox)CellCtl;
                    pVal = w_Check.Checked;
                }
            }

            public void SVal(string pVal)
            {
                if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                //CKM_TextBoxコントロールのとき
                {
                    CKM_Controls.CKM_TextBox w_Edit;
                    w_Edit = (CKM_Controls.CKM_TextBox)CellCtl;
                    w_Edit.Text = pVal;
                }
                else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_MultiLineTextBox)))
                //MultiLineTextBoxコントロールのとき
                {
                    CKM_Controls.CKM_MultiLineTextBox w_Edit;
                    w_Edit = (CKM_Controls.CKM_MultiLineTextBox)CellCtl;
                    w_Edit.Text = pVal;
                }
                else if (CellCtl.GetType().Equals(typeof(System.Windows.Forms.Label)))
                {
                    System.Windows.Forms.Label w_Label;
                    w_Label = (System.Windows.Forms.Label)CellCtl;
                    w_Label.Text = pVal;
                }
                else if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                {
                    Search.CKM_SearchControl w_Edit;
                    w_Edit = (Search.CKM_SearchControl)CellCtl;
                    w_Edit.TxtCode.Text = pVal;
                }
                else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    CKM_Controls.CKM_ComboBox w_Edit;
                    w_Edit = (CKM_Controls.CKM_ComboBox)CellCtl;
                    if (pVal == null)
                        w_Edit.SelectedValue = "-1";
                    else
                        w_Edit.SelectedValue = pVal;
                }
            }
            public void SVal(bool pVal)
            {
                GridControl.clsGridCheckBox w_Check;

                if (CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                {
                    // チェックボックス　のとき
                    w_Check = (GridControl.clsGridCheckBox)CellCtl;

                    w_Check.Checked = pVal;
                }
            }

            public void SBackColor(System.Drawing.Color pColor)
            {
                if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    CKM_Controls.CKM_TextBox w_Edit;
                    // Editコントロールのとき
                    w_Edit = (CKM_Controls.CKM_TextBox)CellCtl;
                    w_Edit.BackColor = pColor;
                    return;
                }
                else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_MultiLineTextBox)))
                {
                    CKM_Controls.CKM_MultiLineTextBox w_Edit;
                    // Editコントロールのとき
                    w_Edit = (CKM_Controls.CKM_MultiLineTextBox)CellCtl;
                    w_Edit.BackColor = pColor;
                    return;
                }
                else if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                {
                    Search.CKM_SearchControl w_Edit;
                    w_Edit = (Search.CKM_SearchControl)CellCtl;
                    w_Edit.TxtCode.BackColor = pColor;
                }
                else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    CKM_Controls.CKM_ComboBox w_Edit;
                    w_Edit = (CKM_Controls.CKM_ComboBox)CellCtl;
                    w_Edit.BackColor = pColor;
                }
                else if (CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                {
                    GridControl.clsGridCheckBox w_Check;
                    // チェックボックス　のとき
                    w_Check = (GridControl.clsGridCheckBox)CellCtl;
                    w_Check.BackColor = pColor;
                }
                else if (CellCtl.GetType().Equals(typeof(GridControl.clsGridButton)))
                {
                    GridControl.clsGridButton w_Button;
                    // ボタン　のとき
                    w_Button = (GridControl.clsGridButton)CellCtl;
                    w_Button.BackColor = pColor;
                }
                else if (CellCtl.GetType().Equals(typeof(Button)))
                {
                    Button w_Button;
                    // ボタン　のとき
                    w_Button = (Button)CellCtl;
                    w_Button.BackColor = pColor;
                }
            }

            /// <summary>
            /// 未使用
            /// </summary>
            /// <param name="pColor"></param>
            public void SDisabledBackColor(System.Drawing.Color pColor)
            {

                if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    CKM_Controls.CKM_TextBox w_Edit;
                    // Editコントロールのとき
                    w_Edit = (CKM_Controls.CKM_TextBox)CellCtl;
                    //w_Edit.DisabledBackColor = pColor;        Todo:DisabledBackColorをプロパティに追加してもらう？
                    return;
                }
                else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_MultiLineTextBox)))
                {
                    CKM_Controls.CKM_MultiLineTextBox w_Edit;
                    // Editコントロールのとき
                    w_Edit = (CKM_Controls.CKM_MultiLineTextBox)CellCtl;
                    //w_Edit.DisabledBackColor = pColor;        Todo:DisabledBackColorをプロパティに追加してもらう？
                    return;
                }
                else if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                {
                    Search.CKM_SearchControl w_Edit;
                    w_Edit = (Search.CKM_SearchControl)CellCtl;
                    //w_Edit.TxtCode.BackColor = pColor;
                }
            }
            public void SForeColor(System.Drawing.Color pColor)
            {
                if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    CKM_Controls.CKM_TextBox w_Edit;
                    // Editコントロールのとき
                    w_Edit = (CKM_Controls.CKM_TextBox)CellCtl;
                    w_Edit.ForeColor = pColor;
                    return;
                }
                else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_MultiLineTextBox)))
                {
                    CKM_Controls.CKM_MultiLineTextBox w_Edit;
                    // Editコントロールのとき
                    w_Edit = (CKM_Controls.CKM_MultiLineTextBox)CellCtl;
                    w_Edit.ForeColor = pColor;
                    return;
                }
                else if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                {
                    Search.CKM_SearchControl w_Edit;
                    w_Edit = (Search.CKM_SearchControl)CellCtl;
                    w_Edit.TxtCode.ForeColor = pColor;
                }
                else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    CKM_Controls.CKM_ComboBox w_Edit;
                    w_Edit = (CKM_Controls.CKM_ComboBox)CellCtl;
                    w_Edit.ForeColor = pColor;
                }
                else if (CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                {
                    GridControl.clsGridCheckBox w_Check;
                    // チェックボックス　のとき
                    w_Check = (GridControl.clsGridCheckBox)CellCtl;
                    w_Check.ForeColor = pColor;
                }
                else if (CellCtl.GetType().Equals(typeof(GridControl.clsGridButton)))
                {
                    GridControl.clsGridButton w_Button;
                    // ボタン　のとき
                    w_Button = (GridControl.clsGridButton)CellCtl;
                    w_Button.ForeColor = pColor;
                }
                else if (CellCtl.GetType().Equals(typeof(Button)))
                {
                    Button w_Button;
                    // ボタン　のとき
                    w_Button = (Button)CellCtl;
                    w_Button.ForeColor = pColor;
                }
                else if (CellCtl.GetType().Equals(typeof(Label)))
                {
                    Label w_Edit;
                    // Label
                    w_Edit = (Label)CellCtl;
                    w_Edit.ForeColor = pColor;
                    return;
                }
            }
            public void SEnabled(bool pEnabled)
            {
                CellCtl.Enabled = pEnabled;
            }

            public void SReadOnly(bool pReadOnly)
            {

                if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    CKM_Controls.CKM_TextBox w_Edit;
                    //Editコントロールのとき
                    w_Edit = (CKM_Controls.CKM_TextBox)CellCtl;
                    w_Edit.ReadOnly = pReadOnly;
                }
                else if (CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_MultiLineTextBox)))
                {
                    CKM_Controls.CKM_MultiLineTextBox w_Edit;
                    //Editコントロールのとき
                    w_Edit = (CKM_Controls.CKM_MultiLineTextBox)CellCtl;
                    w_Edit.ReadOnly = pReadOnly;
                }
                else if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                {
                    Search.CKM_SearchControl w_Edit;
                    w_Edit = (Search.CKM_SearchControl)CellCtl;
                    w_Edit.TxtCode.ReadOnly = pReadOnly;
                }
            }
            public void SBold(bool pBold)
            {
                if(pBold)　　　　
                {
                    if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                    {
                        Search.CKM_SearchControl w_Edit;
                        w_Edit = (Search.CKM_SearchControl)CellCtl;
                        w_Edit.TxtCode.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Bold);
                    }
                    else
                    {
                        CellCtl.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Bold);

                    }
                }
                else
                {
                    if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                    {
                        Search.CKM_SearchControl w_Edit;
                        w_Edit = (Search.CKM_SearchControl)CellCtl;
                        w_Edit.TxtCode.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);
                    }
                    else
                    {
                        CellCtl.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                    }
                }
            }
            public void SBoldForSmallLarge(bool pBold)
            {
                if (pBold)
                {
                    if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                    {
                        Search.CKM_SearchControl w_Edit;
                        w_Edit = (Search.CKM_SearchControl)CellCtl;
                        w_Edit.TxtCode.Font = new Font("ＭＳ ゴシック", 16, FontStyle.Bold);
                    }
                    else
                    {
                        CellCtl.Font = new Font("ＭＳ ゴシック", 16, FontStyle.Bold);

                    }
                }
                else
                {
                    if (CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                    {
                        Search.CKM_SearchControl w_Edit;
                        w_Edit = (Search.CKM_SearchControl)CellCtl;
                        w_Edit.TxtCode.Font = new Font("ＭＳ ゴシック", 16, FontStyle.Regular);
                    }
                    else
                    {
                        CellCtl.Font = new Font("ＭＳ ゴシック", 16, FontStyle.Regular);

                    }
                }
            }
        }

        public struct ST_State_GridKihon
        {
            public bool Cell_Enabled;      //使用可/不可
            public bool Cell_ReadOnly;     //編集可/不可  (True=不可 False=可)
            public bool Cell_Selectable;   //フォーカスセット可能か
            public Color Cell_ForeColor;       //セルの文字色をセット（TextBoxの場合Enabled=Falseだと文字色が変わらないのでLabel等に変更する必要あり）
            public Color Cell_Color;       //セルの色(行の色とは別に固定色であればセット  表示項目の灰色等)
            public bool Cell_Bold;
            //初期化
            public void SetDefault()
            {
                Cell_Enabled = false;        //使用不可
                Cell_ReadOnly = false;       //編集可
                Cell_Selectable = true;      //フォーカスセット可能
                Cell_ForeColor = Control.DefaultForeColor;
                Cell_Color = System.Drawing.Color.Empty;
                Cell_Bold = false;
            }
        }


        //'行のバックカラーをコレクションにもつ
        //'  Key は、何番目の色か(0～)
        //'   例) 偶数行/奇数行で色分けするなら 奇数行="0"  偶数行="1" (上から順に 奇数→偶数→奇数… ) 
        //'       １行目から順に 赤青黄の繰り返しであるなら 赤="0" 青="1" 黄= "2" (上から順に 赤→青→黄→赤…)
        public List<System.Drawing.Color> g_MK_CtlRowBkColor = new List<System.Drawing.Color>();

        //フォーカス移動用定数
        public enum Gen_MK_FocusMove : int
        {
            MvNxt = 0,       //次へ
            MvPrv = 1,       //前へ
            MvSet = 2        //指定位置へ
        }
        public ST_State_GridKihon[,] g_MK_State;                //(列､行)
        public CRS_CTRL[,] g_MK_Ctrl;                           //(列､行)

        public int[] g_MK_FocusOrder;                         //フォーカス移動順
        public int g_MK_DataValue;                //画面に表示されている範囲の先頭行

        public int g_MK_Max_Row;        //最大行数(データ)
        public int g_MK_Ctl_Row;         //画面上の行数
        public int g_MK_Ctl_Col;        //画面列数
        public int g_MK_MaxValue;        //スクロールValue 可能値最大 


        //'-----------------------------------------------
        //'   g_MK_Ctrl の要素数を確定して使用可能にする
        //'
        //'       引数：  pColSu   列数(INDEXは 0～ 列数-1)
        //'               pRowSu   行数(INDEXは 0～ 行数-1)
        //'-----------------------------------------------
        public void F_CtrlArray_MK(int pColSU, int pRowSU)
        {
            int w_Row;
            int w_Col;

            g_MK_Ctrl = new CRS_CTRL[pColSU , pRowSU ];

            for (w_Row = 0; w_Row <= pRowSU - 1; w_Row++)
                for (w_Col = 0; w_Col <= pColSU - 1; w_Col++)
                {
                    g_MK_Ctrl[w_Col, w_Row] = new CRS_CTRL();
                }
        }

        //'-----------------------------------------------
        //'   指定されたセルに設定するBackColorを返す
        //'
        //'       引数：  pCol   列番号
        //'               pRow   行番号(データ)
        //'-----------------------------------------------
        public System.Drawing.Color F_GetBackColor_MK(int pCol, int pRow)
        {
            int w_ColorNo;
            try
            {
                w_ColorNo = pRow % g_MK_CtlRowBkColor.Count;

                if (g_MK_State[pCol, pRow].Cell_Color.Equals(System.Drawing.Color.Empty))
                    return g_MK_CtlRowBkColor[w_ColorNo] ;
                else
                    return g_MK_State[pCol, pRow].Cell_Color;


                    }
            catch
            {
                return System.Drawing.Color.Empty;
            }
        }
        //'-----------------------------------------------
        //'   指定されたセルに設定するForeColorを返す
        //'
        //'       引数：  pCol   列番号
        //'               pRow   行番号(データ)
        //'-----------------------------------------------
        public System.Drawing.Color F_GetForeColor_MK(int pCol, int pRow)
        {
            try
            {
                return g_MK_State[pCol, pRow].Cell_ForeColor;

            }
            catch
            {
                return System.Drawing.Color.Empty;
            }
        }
        private bool F_MoveFocus_Sub(int pCol, int pRow)
        {
            try
            {
                if (g_MK_State[pCol, pRow].Cell_Enabled && g_MK_State[pCol, pRow].Cell_Selectable)
                    return true;

                else
                    return false;
            }

            catch
            {
                return false;
                     }
        }
        protected void F_FocusSet(out int pCtlCol, out int pCtlRow, out int pValue, int pCol, int pRow,  System.Windows.Forms.VScrollBar pScrool)
        {

            pCtlCol = pCol;
            if (pScrool.Value > pRow)
            {
                //'画面表示部分より上の行にフォーカスを移動したい
                pValue = pRow;
                pCtlRow = pRow - pValue;
            }
            else if (pRow > pScrool.Value + g_MK_Ctl_Row - 1)
            {
                //'画面表示部分より下の行にフォーカスを移動したい

                if (pRow - (g_MK_Ctl_Row - 1) < pScrool.Maximum)
                    pValue = pRow - (g_MK_Ctl_Row - 1);
                else
                    pValue = pScrool.Maximum;

                pCtlRow = pRow - pValue;
            }
            else
            {
                //'スクロールしなくても、画面に行き先がある
                pCtlRow = pRow - pScrool.Value;
                pValue = pScrool.Value;
            }
        }


        //'明細部のフォーカス移動
        //'   pErrSet     行き先がなかった場合にセットする位置
        //'   pErrDest    指定先にいけなかった場合に前後に探す場合、どちらに移動するか 
        //'                   (pDest=MvSetの場合のみ使用 前後に探さないときはMvsetとする) 
        //'   pRow        指定先(省略可 pDest=MvSetの場合のみ使用)
        //'   pCol        指定先(省略可 pDest=MvSetの場合のみ使用)
        public bool F_MoveFocus_MAIN_MK(out int pOKRow, out int pOKCol, out bool pOKFlg, int pDest, int pErrDest, System.Windows.Forms.Control pErrSet, int pLastRow, int pLastCol, System.Windows.Forms.Control pActivCtl, System.Windows.Forms.VScrollBar pScrool, int pRow = -1, int pCol = -1)
        {
            System.Windows.Forms.Control w_MotoControl;
            int w_Dest;
            int w_RowSt = 0;
            int w_ColST = 0;
            int w_ColSt_T = 0;         //最初の行のStart位置
            int w_ColSt_M = 0;         //次の行からのスタート位置
            int w_RowEd = 0;
            int w_ColEd = 0;
            int w_Step = 0;
            int w_LastRow;     //元の位置
            int w_LastCol;
            int w_NowRow;      //チェックしてる場所
            int w_NowCol;      //チェックしてる列(ただし、列番号ではなく 列の移動順のほうの番号)

            w_MotoControl = pActivCtl;
            w_Dest = pDest;
            w_LastRow = pLastRow;
            w_LastCol = pLastCol;

            bool ret = true;
            pOKFlg = false;
            pOKCol = 0;
            pOKRow = 0;

            if (w_Dest == (int)Gen_MK_FocusMove.MvSet)
            {
                //行き先指定のとき、指定場所がフォーカスセット可能かチェック
                if (pRow != -1 && pCol != -1)
                {
                    pOKFlg = F_MoveFocus_Sub(pCol, pRow);


                    if (pOKFlg)
                    {
                        pOKRow = pRow;
                        pOKCol = pCol;
                    }
                    else
                    {
                        w_Dest = pErrDest;    //   'どちらかの方向へ探すか/指定位置が不可のときにはmvSetになる
                        w_LastRow = pRow;
                        w_LastCol = pCol;

                        ret = false;
                    }
                }
            }

            if (pOKFlg == false)
            {
                switch (w_Dest)
                {
                    case (int)Gen_MK_FocusMove.MvPrv:
                        w_RowSt = w_LastRow;
                        w_ColSt_T = Array.IndexOf(g_MK_FocusOrder, w_LastCol);
                        w_ColSt_M = g_MK_Ctl_Col - 1;
                        w_RowEd = 0;
                        w_ColEd = 0;
                        w_Step = -1;

                        break;

                    case (int)Gen_MK_FocusMove.MvNxt:
                        w_RowSt = w_LastRow;
                        w_ColSt_T = Array.IndexOf(g_MK_FocusOrder, w_LastCol);
                        w_ColSt_M = 0;
                        w_RowEd = g_MK_Max_Row - 1;
                        w_ColEd = g_MK_Ctl_Col - 1;
                        w_Step = 1;
                        break;
                }


                if (w_Dest != (int)Gen_MK_FocusMove.MvSet)
                {
                    if (w_Step >= 0)
                    {
                        for (w_NowRow = w_RowSt; w_NowRow <= w_RowEd; w_NowRow += w_Step)
                        {
                            if (w_NowRow == w_RowSt)
                            {
                                w_ColST = w_ColSt_T;     //最初の行
                            }
                            else
                            {
                                w_ColST = w_ColSt_M;
                            }

                            for (w_NowCol = w_ColST; w_NowCol <= w_ColEd; w_NowCol += w_Step)
                            {

                                if (w_NowRow == w_RowSt && w_NowCol == w_ColST)
                                {
                                    //スタート位置はとばす
                                }
                                else
                                {
                                    pOKFlg = F_MoveFocus_Sub(g_MK_FocusOrder[w_NowCol], w_NowRow);
                                    if (pOKFlg)
                                    {
                                        pOKRow = w_NowRow;
                                        pOKCol = g_MK_FocusOrder[w_NowCol];
                                        break;
                                    }
                                }

                            }
                            if (pOKFlg)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (w_NowRow = w_RowSt; w_NowRow >= w_RowEd; w_NowRow += w_Step)
                        {
                            if (w_NowRow == w_RowSt)
                            {
                                w_ColST = w_ColSt_T;     //最初の行
                            }
                            else
                            {
                                w_ColST = w_ColSt_M;
                            }

                            for (w_NowCol = w_ColST; w_NowCol >= w_ColEd; w_NowCol += w_Step)
                            {

                                if (w_NowRow == w_RowSt && w_NowCol == w_ColST)
                                {
                                    //スタート位置はとばす
                                }
                                else
                                {
                                    pOKFlg = F_MoveFocus_Sub(g_MK_FocusOrder[w_NowCol], w_NowRow);
                                    if (pOKFlg)
                                    {
                                        pOKRow = w_NowRow;
                                        pOKCol = g_MK_FocusOrder[w_NowCol];
                                        break;
                                    }
                                }

                            }
                            if (pOKFlg)
                            {
                                break;
                            }
                        }
                    }

                }
            }
            if (pOKFlg == false)
            {
                ret = false;
            }
            return ret;
        }

        //明細部のコントロールか
        //  指定されたコントロールが g_MK_Control に含まれるかどうか判断し、含まれる場合 列、行(g_MK_ControlのINDEX)を返す
        //
        //   pControl    対象のコントロール
        //   pCol        見つかったコントロールが 画面上何列目か(g_MK_ControlのIndex)   戻り値Falseのときは -1
        //   pCtlRow     見つかったコントロールが 画面上何行目か(g_MK_ControlのIndex)   戻り値Falseのときは -1
        //
        //  戻り値       True    配列に含まれる
        //               False   配列に含まれない

        public bool F_Search_Ctrl_MK(System.Windows.Forms.Control pControl, out int pCol, out int pCtlRow)
        {
            int w_CtlRow;
            int w_Col;
            bool ret = false;
            pCol = -1;
            pCtlRow = -1;

            for (w_CtlRow = g_MK_Ctrl.GetLowerBound(1); w_CtlRow <= g_MK_Ctrl.GetUpperBound(1); w_CtlRow++)
            {
                for (w_Col = g_MK_Ctrl.GetLowerBound(0); w_Col <= g_MK_Ctrl.GetUpperBound(0); w_Col++)
                {
                    if (g_MK_Ctrl[w_Col, w_CtlRow].CellCtl.GetType().Equals(pControl))
                    {
                        pCol = w_Col;
                        pCtlRow = w_CtlRow;
                        return true;
                    }
                    else if(g_MK_Ctrl[w_Col, w_CtlRow].CellCtl.Name.Equals(pControl.Name))
                    {
                        pCol = w_Col;
                        pCtlRow = w_CtlRow;
                        return true;
                    }
                    else if (pControl.Parent != null && g_MK_Ctrl[w_Col, w_CtlRow].CellCtl.Name.Equals(pControl.Parent.Name))
                    {
                        pCol = w_Col;
                        pCtlRow = w_CtlRow;
                        return true;
                    }
                }
            }

            return ret;
        }
        // **********************************************************************
        // 単価項目 プロパティセット
        // 単価の桁数をセット
        // 
        // Format/Max/Min等をセット
        // 
        // 引数:          pImnCnt  対象となるコントロール(ImNumberのみ)
        // **********************************************************************
        public void SetProp_TANKA(ref Control pImnCtrl)
        {
            string w_Fmt = string.Empty;

            CKM_Controls.CKM_TextBox w_Edit;
            
            //// 形式
            //w_Fmt = "###,###,##0";  //ﾌｫｰﾏｯﾄの書式
            //                        //pImnCtrl.Format.Digit = w_Fmt;
            //                        //pImnCtrl.DisplayFormat.Digit = w_Fmt;

            // 最大値
            //pImnCtrl.MaxValue = Max_Tan();

            // 最小値
            //pImnCtrl.MinValue = Min_Tan();


            if (pImnCtrl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                w_Edit = (CKM_Controls.CKM_TextBox)pImnCtrl;
                //w_Edit.CKM_Type = CKM_Controls.CKM_TextBox.Type.Price;
                //w_Edit.UseThousandSeparator = true;
                w_Edit.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;
                
                w_Edit.MaxLength = 9+2 ;     //9桁+カンマ2桁分
                w_Edit.IntegerPart = 9;
                w_Edit.DecimalPlace = 0;
            }
        }
        // **********************************************************************
        // 数項目 プロパティセット
        // 数量の桁数をセット
        // 
        // Format/Max/Min等をセット
        // 
        // 引数:          pImnCnt  対象となるコントロール(ImNumberのみ)
        // **********************************************************************
        public void SetProp_SU(int keta, ref Control pImnCtrl)
        {
            string w_Fmt = string.Empty;

            CKM_Controls.CKM_TextBox w_Edit;
            
            if (pImnCtrl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                w_Edit = (CKM_Controls.CKM_TextBox)pImnCtrl;
                //w_Edit.UseThousandSeparator = true;
                w_Edit.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;

                w_Edit.MaxLength = keta + Convert.ToInt32(Math.Ceiling((double)keta / 3)); 
                w_Edit.IntegerPart = keta;
                w_Edit.DecimalPlace = 0;
            }
        }
        public void SetProp_Ritu(int keta, int decimalKeta, ref Control pImnCtrl)
        {
            string w_Fmt = string.Empty;

            CKM_Controls.CKM_TextBox w_Edit;

            if (pImnCtrl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                w_Edit = (CKM_Controls.CKM_TextBox)pImnCtrl;
                //w_Edit.UseThousandSeparator = true;
                w_Edit.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;

                if (decimalKeta == 0)
                    w_Edit.MaxLength = keta + Convert.ToInt32(Math.Ceiling((double)(keta-1) / 3));
                else
                    w_Edit.MaxLength = keta + Convert.ToInt32(Math.Ceiling((double)(keta-1) / 3)) + decimalKeta + 1;

                w_Edit.IntegerPart = keta;
                w_Edit.DecimalPlace = decimalKeta;
            }
        }
    }
}
