using CKM_Controls;
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
using System.Runtime.InteropServices; //EXCEL出力(必要)	
using Microsoft.Office.Interop;//EXCEL出力(必要)

namespace Base.Client
{
    public partial class FrmSubForm : Form
    {
        #region "公開定数"

        /// <summary>
        ///     ''' 表示ファンクション
        ///     ''' </summary>
        ///     ''' <remarks>F11を表示とする</remarks>
        protected const short FuncDisp = 11;
        /// <summary>
        ///     ''' 確定ファンクション
        ///     ''' </summary>
        ///     ''' <remarks>F12を確定とする</remarks>
        protected const short FuncExec = 12;
        /// <summary>
        ///     ''' 終了ファンクション
        ///     ''' </summary>
        ///     ''' <remarks>F1を終了とする</remarks>
        protected const short FuncEnd = 1;


        #endregion

        public Control PreviousCtrl { get; set; }

        #region"公開プロパティ"
        /// <summary>
        ///     ''' HeaderTitleText
        ///     ''' </summary>
        ///     ''' <value>lblHeaderTitleの名称</value>
        ///     ''' <remarks>lblHeaderTitleの名称を変更</remarks>
        protected string HeaderTitleText
        {
            set
            {
                lblHeaderTitle.Text = value;
            }
        }

        protected string BtnF9Text
        {
            set
            {
                this.BtnF9.Text = value;
            }
        }
        protected string BtnF12Text
        {
            set
            {
                this.BtnF12.Text = value;
            }
        }

        public bool flgCancel = false;

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("FormName")]
        [DisplayName("ProgramName")]
        public string ProgramName { get => lblHeaderTitle.Text; set => lblHeaderTitle.Text = value; }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Panel Header's Height")]
        [DisplayName("Header Height")]
        public int PanelHeaderHeight
        {
            get => PanelTop.Height;
            set => PanelTop.Height = value;
        }


        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set F9 Button Visible")]
        [DisplayName("F9 Visible")]
        public bool F9Visible
        {
            get => BtnF9.Visible;
            set => BtnF9.Visible = value;
        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set F11 Button Visible")]
        [DisplayName("F11 Visible")]
        public bool F11Visible
        {
            get => BtnF11.Visible;
            set => BtnF11.Visible = value;
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set F12 Button Visible")]
        [DisplayName("F12 Visible")]
        public bool F12Visible
        {
            get => BtnF12.Visible;
            set => BtnF12.Visible = value;
        }
        public string AllAvailableStores
        {
            get
            {
                return mAllAvailableStores;
            }
            set
            {
                mAllAvailableStores = value;
                if (mAllAvailableStores != null)
                    availableStores = mAllAvailableStores.Split(',');
            }
        }

        protected bool BtnF12Visible
        {
            set
            {
                BtnF12.Visible = value;
            }
        }
        #endregion

        #region "内部定数"

        #endregion

        #region 内部変数
        private string mAllAvailableStores;
        private string[] availableStores;
        private TextBox txt = null;
        protected Base_BL bbl = new Base_BL();

        #endregion


        #region "オーバライド可能イベントハンドラ"
        /// <summary>
        ///     ''' 表示押下時
        ///     ''' </summary>
        ///     ''' <remarks>プログラムでオーバーライドする</remarks>
        protected virtual void ExecDisp()
        {
        }
        /// <summary>
        ///     ''' 確定押下時
        ///     ''' </summary>
        ///     ''' <remarks>プログラムでオーバーライドする</remarks>
        protected virtual void ExecSec()
        {
        }
        /// <summary>
        ///     ''' 終了押下時
        ///     ''' </summary>
        ///     ''' <remarks>
        ///     ''' 終了処理を実行
        ///     ''' </remarks>
        protected void EndSec()
        {
            this.Close();
        }
        #endregion

        public FrmSubForm()
        {
            InitializeComponent();
        }

        #region "共通部品メソッド"
        /// <summary>
        /// 起動時共通処理
        ///         
        /// check Authorization 
        /// insert to log(form open) 
        /// </summary>
        /// <remarks>プログラム起動時に呼び出してください</remarks>
        protected void StartProgram()
        {



        }

        public void SetFuncKey(object Obj, short dmyIndex, bool flg)
        {
            short Index = dmyIndex;
            switch (dmyIndex)
            {
                case 10:
                    Index = 2;
                    break;
                case 11:
                    Index = 3;
                    break;
            }
            Control[] Obj_Btn = { this.BtnF1, this.BtnF11, this.BtnF12 };

            // ラベルのテキストが空白の場合は使用不可とする
            if (flg && Obj_Btn[Index].Text == "")
                flg = false;

            Obj_Btn[Index].Enabled = flg;

        }
        #endregion

        /// <summary>
        /// load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubMainForm_Load(object sender, EventArgs e)
        {
            this.lblHeaderTitle.BackColor = Color.FromArgb(112, 173, 71);
           

        }

        /// <summary>
        /// All Function Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                ButtonFunction(btn.Tag.ToString());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        /// <summary>
        /// Buttonclick + function Key 
        /// </summary>
        /// <param name="buttonTag"></param>
        private void ButtonFunction(string buttonTag)
        {
            short Index = Convert.ToInt16(buttonTag);

            // 終了ファンクション
            if (Index + 1 == FuncEnd)
            {
                //if (bbl.ShowMessage("Q003") == DialogResult.Yes)
                flgCancel = true;
                EndSec();
                return;
            }
            

            FunctionProcess(Index);

        }

        /// <summary>
        /// handle Function Key F1 to F12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubForm_KeyDown(object sender, KeyEventArgs e)
    {
            try
            {
                short KeyCode = System.Convert.ToInt16(e.KeyCode);
                short Shift = 0;
                if ((e.Modifiers & Keys.Shift) == Keys.Shift)
                    Shift = 1;

                if (Shift == 0)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.F1:
                        case Keys.F2:
                        case Keys.F3:
                        case Keys.F4:
                        case Keys.F5:
                        case Keys.F6:
                        case Keys.F7:
                        case Keys.F8:
                        case Keys.F9:
                        case Keys.F10:
                        case Keys.F11:
                        case Keys.F12:
                            Control[] targets = PanelFooter.Controls.Find("btn" + e.KeyCode.ToString(), true);
                            if (targets.Length == 0)
                            {
                                return;
                            }
                            Button btn = targets[0] as Button;
                            if (btn.Enabled)
                                ButtonFunction(btn.Tag.ToString());
                            break;
                        case Keys.Enter:
                            if (this.Name != "Search_TenzikaiShouhin" && Name !="Search_Tenzikai")
                            MoveNextControl(e);
                            break;
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

        //入力可能店舗チェック
        protected bool CheckAvailableStores(string storeCD)
        {
            if (Array.IndexOf(availableStores, storeCD) >= 0)
                return true;

            else
                return false;
        }


        protected bool RequireCheck(Control[] ctrl, TextBox txt = null)
        {
            this.txt = txt;
            foreach (Control c in ctrl)
            {
                if (c is CKM_TextBox)
                {
                    if (txt == null)
                    {
                        if (string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
                        {
                            bbl.ShowMessage("E102");
                            c.Focus();
                            return false;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(txt.Text))
                    {
                        if (string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
                        {
                            bbl.ShowMessage("E102");
                            c.Focus();
                            return false;
                        }
                    }
                }
                else if (c is CKM_ComboBox)
                {
                    if (((CKM_ComboBox)c).SelectedIndex.Equals(-1))
                    {
                        bbl.ShowMessage("E102");
                        c.Focus();
                        return false;
                    }
                    if (((CKM_ComboBox)c).SelectedValue.Equals("-1"))
                    {
                        bbl.ShowMessage("E102");
                        c.Focus();
                        return false;
                    }
                }
            }
            return true;
        }

        public virtual void FunctionProcess(int Index)
        {

            // 実行ファンクション
            if (Index + 1 == FuncExec)
            {
                this.Cursor = Cursors.WaitCursor;
                ExecSec();
                this.Cursor = Cursors.Default;
                return;
            }
            else if (Index + 1 == FuncDisp)
            {
                this.Cursor = Cursors.WaitCursor;
                ExecDisp();
                this.Cursor = Cursors.Default;
                return;
            }

        }
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                const int CS_NOCLOSE = 0x200;

                System.Windows.Forms.CreateParams createParam = base.CreateParams;
                createParam.ClassStyle = createParam.ClassStyle | CS_NOCLOSE;

                return createParam;
            }
        }

        public void MoveNextControl(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ActiveControl is CKM_TextBox)
                {
                    if ((ActiveControl as CKM_TextBox).MoveNext)
                    {
                        if (this.Parent != null)
                            this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        else
                        {
                            (ActiveControl as CKM_TextBox).MoveNext = false;
                            this.SelectNextControl(ActiveControl, true, true, true, true);
                        }
                    }
                }
                else if (ActiveControl is CKM_ComboBox)
                {
                    if ((ActiveControl as CKM_ComboBox).MoveNext)
                    {
                        if (this.Parent != null)
                            this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        else
                        {
                            (ActiveControl as CKM_ComboBox).MoveNext = false;
                            this.SelectNextControl(ActiveControl, true, true, true, true);
                        }
                    }
                }
                else if (ActiveControl is UserControl)
                {
                    UserControl sc = ActiveControl as UserControl;
                    Control ctrl = sc.Controls.Find("txtChangeDate", true)[0];
                    if ((sc.ActiveControl as CKM_TextBox).MoveNext)
                    {
                        if (sc.ActiveControl == ctrl)
                            sc.SelectNextControl(sc.ActiveControl, true, true, true, true);
                        else
                        {
                            if (!ctrl.Visible)
                                this.SelectNextControl(ActiveControl, true, true, true, true);
                            else
                                sc.SelectNextControl(sc.ActiveControl, true, true, true, true);
                        }
                    }
                }
                else if (ActiveControl is CKM_GridView)
                { }

                else if (ActiveControl is Panel)
                {

                }
                else if (ActiveControl is CKMShop_ComboBox cb)
                {
                    
                }
                else
                {
                    if (this.Parent != null)
                        this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                    else
                        this.SelectNextControl(ActiveControl, true, true, true, true);
                }
            }
        }

        #region "MainForm.csと同じ内容　EXCEL出力のためのFunciton"	
        /// <summary>	
        /// 「名前を付けて保存」ダイアログでファイルを保存する	
        /// </summary>	
        /// <param name="initFileName"></param>	
        /// <param name="outputFileName"></param>	
        /// <param name="kbn">0:フィルターなし,1:Excel</param>	
        /// <returns>MainForm.csと同じ</returns>	
        protected bool ShowSaveFileDialog(string initFileName, string InOperatorCD, out string outputFileName, int kbn = 0)
        {
            outputFileName = "";
            //SaveFileDialogクラスのインスタンスを作成	
            SaveFileDialog sfd = new SaveFileDialog();
            initFileName = initFileName + DateTime.Now.ToString(" yyyyMMdd_HHmmss ") + InOperatorCD;
            //はじめのファイル名を指定する	
            //はじめに「ファイル名」で表示される文字列を指定する	
            sfd.FileName = initFileName;
            //はじめに表示されるフォルダを指定する	
            //sfd.InitialDirectory = @"C:\";	
            //[ファイルの種類]に表示される選択肢を指定する	
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される	
            //sfd.Filter = "HTMLファイル(*.html;*.htm)|*.html;*.htm|すべてのファイル(*.*)|*.*";	
            if (kbn == 1)
            {
                // 例：Excelファイルを開く場合（Office2003と2007両対応したい）	
                sfd.Filter = "Excelファイル(*.xls;*.xlsx)|*.xls;*.xlsx";
            }
            //[ファイルの種類]ではじめに選択されるものを指定する	
            //2番目の「すべてのファイル」が選択されているようにする	
            //sfd.FilterIndex = 2;	
            //タイトルを設定する	
            sfd.Title = "保存先のファイルを選択してください";
            //ダイアログを表示する	
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する	
                Console.WriteLine(sfd.FileName);
                outputFileName = sfd.FileName;
                return true;
            }
            return false;
        }
        protected void OutputExecel(DataGridView dgv, string EXCEL_SAVE_PATH)
        {
            if (dgv.Rows.Count > 0)
            {// Excelを参照設定する必要があります	
             // [参照の追加],[COM],[Microsoft Excel *.* Object Library]	
             // Imports Microsoft.Office.Interop (必要)	
             // Imports System.Runtime.InteropServices (必要)	
             //Excel出力	
             // EXCEL関連オブジェクトの定義	
                Microsoft.Office.Interop.Excel.Application objExcel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook objWorkBook = objExcel.Workbooks.Add();
                //Microsoft.Office.Interop.Excel.Worksheet objSheet = null/* TODO Change to default(_) if this is not a reference type */;	
                try
                {
                    objExcel.Visible = false;
                    // 現在日時を取得	
                    //string timestanpText = bbl.GetDate();// String.Format(DateTime.Now, "yyyyMMddHHmmss");	
                    //// 実行モジュールと同一フォルダのファイルを取得	
                    //System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);	
                    //string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + InProgramID;	
                    // 保存ディレクトリとファイル名を設定	
                    string saveFileName = EXCEL_SAVE_PATH;
                    //saveFileName = objExcel.GetSaveAsFilename(InitialFilename: EXCEL_SAVE_PATH + "ファイル名_" + timestanpText, FileFilter: "Excel File (*.xlsx),*.xlsx");	
                    //// 保存先ディレクトリの設定が有効の場合はブックを保存	
                    ////if (saveFileName != "False")	
                    //objWorkBook.SaveAs(Filename: saveFileName);	
                    // シートの最大表示列項目数	
                    int columnMaxNum = dgv.Columns.Count - 1;
                    // シートの最大表示行項目数	
                    int rowMaxNum = dgv.Rows.Count - 1;
                    // 項目名格納用リストを宣言	
                    List<string> columnList = new List<string>();
                    // 項目名を取得	
                    for (int i = 0; i <= (columnMaxNum); i++)
                        columnList.Add(dgv.Columns[i].HeaderCell.Value.ToString());
                    // セルのデータ取得用二次元配列を宣言	
                    string[,] v = new string[rowMaxNum + 1, columnMaxNum + 1];
                    for (int row = 0; row <= rowMaxNum; row++)
                    {
                        for (int col = 0; col <= columnMaxNum; col++)
                        {
                            if (dgv.Rows[row].Cells[col].Value == null == false)
                                // セルに値が入っている場合、二次元配列に格納	
                                v[row, col] = dgv.Rows[row].Cells[col].Value.ToString();
                        }
                    }
                    // EXCELに項目名を転送	
                    for (int i = 1; i <= dgv.Columns.Count; i++)
                    {
                        // シートの一行目に項目を挿入	
                        objWorkBook.Sheets[1].Cells[1, i] = columnList[i - 1];
                        // 罫線を設定	
                        objWorkBook.Sheets[1].Cells[1, i].Borders.LineStyle = true;
                        // 項目の表示行に背景色を設定	
                        //objWorkBook.Sheets[1].Cells(1, i).Interior.Color = Information.RGB(140, 140, 140);	
                        // 文字のフォントを設定	
                        //objWorkBook.Sheets[1].Cells(1, i).Font.Color = Information.RGB(255, 255, 255);	
                        objWorkBook.Sheets[1].Cells(1, i).Font.Bold = true;
                    }
                    // EXCELにデータを範囲指定で転送	
                    //string data = "A2:" + 'A' + (columnMaxNum + (dgv.Rows.Count + 1)).ToString();	
                    objWorkBook.Sheets[1].Range[objWorkBook.Sheets[1].Cells[2, 1], objWorkBook.Sheets[1].Cells[dgv.Rows.Count + 1, columnMaxNum + 1]] = v;
                    // データの表示範囲に罫線を設定	
                    objWorkBook.Sheets[1].Range[objWorkBook.Sheets[1].Cells[2, 1], objWorkBook.Sheets[1].Cells[dgv.Rows.Count + 1, columnMaxNum + 1]].Borders.LineStyle = true;
                    //// エクセル表示	
                    //objExcel.Visible = true;	
                    objWorkBook.SaveAs(saveFileName);
                    // クローズ	
                    objWorkBook.Close(false);
                    objExcel.Quit();
                }
                finally
                {
                    // EXCEL解放	
                    Marshal.ReleaseComObject(objWorkBook);
                    Marshal.ReleaseComObject(objExcel);
                    objWorkBook = null/* TODO Change to default(_) if this is not a reference type */;
                    objExcel = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
        }
        #endregion


    }
}
