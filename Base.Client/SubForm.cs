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
                else
                {
                    if (this.Parent != null)
                        this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                    else
                        this.SelectNextControl(ActiveControl, true, true, true, true);
                }
            }
        }
    }
}
