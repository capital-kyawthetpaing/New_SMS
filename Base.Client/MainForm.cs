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
using Entity;
using CKM_Controls;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.Runtime.InteropServices; //EXCEL出力(必要)
using Microsoft.Office.Interop;//EXCEL出力(必要)
using Search;

namespace Base.Client
{
    public partial class FrmMainForm : Form
    {

        #region "公開定数"
        /// <summary>
        /// コマンドライン引数列挙体
        /// </summary>
        /// <remarks></remarks>
        public enum ECmdLine : short
        {
            ExeID,

            CompanyCD,

            OperatorCD,

            PcID
        }

        public enum EProMode : short
        {
            /// <summary>
            ///          入力プログラム
            ///          </summary>
            ///          <remarks></remarks>
            INPUT,
            /// <summary>
            ///          マスタメンテ
            ///          </summary>
            ///          <remarks></remarks>
            MENTE,
            /// <summary>
            /// 照会
            /// </summary>
            SHOW,
            ///<summary>
            /// バッチプログラム
            ///</summary>
            BATCH,
            /// <summary>
            ///          帳票プログラム
            ///          </summary>
            ///          <remarks></remarks>
            PRINT,
            /// <summary>
            ///          メニュー
            ///          </summary>
            ///          <remarks></remarks>
            MENU,
            /// <summary>
            ///         ''' 受注取込
            ///         ''' </summary>
            ///         ''' <remarks></remarks>
            JUCHUTORIKOMIAPI,
            /// <summary>
            ///         ''' Functions Buttons for KehiNyuuryoku Form
            ///         ''' </summary>
            ///         ''' <remarks></remarks>
            KehiNyuuryoku
        }

        /// <summary>
        ///     ''' 処理モード
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        public enum EOperationMode : short
        {
            /// <summary>
            /// 未設定
            /// </summary>
            NULL,
            /// <summary>
            /// 新規
            /// </summary>
            INSERT,
            /// <summary>
            /// 更新
            /// </summary>
            UPDATE,
            /// <summary>
            /// 削除
            /// </summary>
            DELETE,
            /// <summary>
            /// 照会
            /// </summary>
            SHOW
        }
        /// <summary>
        ///     ''' 表示ファンクション
        ///     ''' </summary>
        ///     ''' <remarks>F11を表示とする</remarks>
        protected const short FuncDisp = 11;
        /// <summary>
        ///     ''' 登録・印刷ファンクション
        ///     ''' </summary>
        ///     ''' <remarks>F12を登録とする</remarks>
        public const short FuncExec = 12;
        /// <summary>
        ///     ''' 終了ファンクション
        ///     ''' </summary>
        ///     ''' <remarks>F1を終了とする</remarks>
        public const short FuncEnd = 1;


        /// <summary>
        ///     ''' 印刷モード
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        public enum EPrintMode : short
        {
            // 初期値
            NULL,
            // 電子帳票(PDF)
            CSV = 9,
            // 電子データ(CSV)
            PDF = 10,
            // 直接印刷
            DIRECT = 11,
        }
        /// <summary>
        ///     ''' 端数区分
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        public enum HASU_KBN : short
        {
            NULL,
            /// <summary>
            ///         ''' 1:切捨て
            ///         ''' </summary>
            ///         ''' <remarks></remarks>
            KIRISUTE,
            /// <summary>
            ///         ''' 2:四捨五入
            ///         ''' </summary>
            ///         ''' <remarks></remarks>
            SISYAGONYU,
            /// <summary>
            ///         ''' 3:切上げ
            ///         ''' </summary>
            ///         ''' <remarks></remarks>
            KIRIAGE,
        }
        #endregion

        #region"公開プロパティ"
        public EOperationMode OperationMode
        {
            get
            {
                return _OperationMode;
            }
            set
            {
                _OldOperationMode = _OperationMode;
                _OperationMode = value;

                //KTP 2019-06-06 change color on mode change
                switch (value)
                {
                    case EOperationMode.INSERT:
                        if (this._ProMode == EProMode.MENTE || this._ProMode == EProMode.INPUT || this._ProMode == EProMode.KehiNyuuryoku)
                        //this.ModeText = "登録";

                        //else
                        {
                            this.ModeText = "新規";
                            lblMode.BackColor = INSERT_MODE_COLOR;
                        }


                        break;

                    case EOperationMode.UPDATE:
                        if (this._ProMode == EProMode.MENTE || this._ProMode == EProMode.INPUT || this._ProMode == EProMode.KehiNyuuryoku)
                        {
                            this.ModeText = "修正";
                            lblMode.BackColor = UPDATE_MODE_COLOR;
                        }

                        break;

                    case EOperationMode.DELETE:
                        if (this._ProMode == EProMode.MENTE || this._ProMode == EProMode.INPUT || this._ProMode == EProMode.KehiNyuuryoku)
                        {
                            this.ModeText = "削除";
                            lblMode.BackColor = DELETE_MODE_COLOR;
                        }
                        break;

                    case EOperationMode.SHOW:
                        if (this._ProMode == EProMode.MENTE || this._ProMode == EProMode.INPUT || this._ProMode == EProMode.KehiNyuuryoku)
                        {
                            this.ModeText = "照会";
                            lblMode.BackColor = SHOW_MODE_COLOR;
                        }
                        break;
                }
            }
        }

        public EOperationMode OldOperationMode
        {
            get
            {
                return _OldOperationMode;
            }
        }

        /// <summary>
        ///     ''' HeaderTitleText
        ///     ''' </summary>
        ///     ''' <value>lblHeaderTitleの名称</value>
        ///     ''' <remarks>lblHeaderTitleの名称を変更</remarks>
        public string HeaderTitleText
        {
            set
            {
                lblHeaderTitle.Text = value;
            }
        }
        /// <summary>
        ///     ''' ModeText
        ///     ''' </summary>
        ///     ''' <value>ModeTextの名称</value>
        ///     ''' <remarks>ModeTextの名称を変更</remarks>
        public string ModeText
        {
            //KTP 2019-03-06 to get mode text
            get { return lblMode.Text; }
            set
            {
                lblMode.Text = value;
            }
        }

        /// <summary>
        ///     ''' ModeVisible
        ///     ''' </summary>
        ///     ''' <value>Modeの使用可否</value>
        ///     ''' <remarks>Modeの使用可否を変更</remarks>
        public bool ModeVisible
        {
            get
            {
                return lblMode.Visible;
            }
            set
            {
                lblMode.Visible = value;
            }
        }

        public System.Drawing.Color ModeColor
        {
            get
            {
                return lblMode.BackColor;
            }
            set
            {
                lblMode.BackColor = value;
            }
        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("FormName")]
        [DisplayName("ProgramID")]
        public override string Text { get => base.Text; set => base.Text = value; }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Panel Header's Height")]
        [DisplayName("Header Height")]
        public int PanelHeaderHeight
        {
            get => panelTop.Height;
            set => panelTop.Height = value;
        }

        private bool F9 = true;
        //ssa
        private bool F2 = true;
        private bool F3 = true;
        private bool F4 = true;
        private bool F5 = true;
        private bool F6 = true;
        private bool F7 = true;
        private bool F8 = true;
        private bool F10 = true;
        private bool F11 = true;//
        private bool F12 = true;//07/10追加
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set F9 Button Visible")]
        [DisplayName("F9 Visible")]
        public bool F9Visible
        {
            get => F9;
            set
            {
                F9 = value;
                ButtonVisible(BtnF9, value, "検索(F9)");
            }
        }
        public bool F2Visible
        {
            get => F2;
            set
            {
                F2 = value;
                ButtonVisible(BtnF2, value, "新規(F2)");
            }
        }
        public bool F3Visible
        {
            get => F3;
            set
            {
                F3 = value;
                ButtonVisible(BtnF3, value, "変更(F3)");
            }
        }
        public bool F4Visible
        {
            get => F4;
            set
            {
                F4 = value;
                ButtonVisible(BtnF4, value, "削除(F4)");
            }
        }
        public bool F5Visible
        {
            get => F5;
            set
            {
                F5 = value;
                ButtonVisible(BtnF5, value, "照会(F5)");
            }
        }

        public bool F6Visible
        {
            get => F6;
            set
            {
                F6 = value;
                ButtonVisible(BtnF6, value, "ｷｬﾝｾﾙ(F6)");
            }
        }

        public bool F7Visible
        {
            get => F7;
            set
            {
                F7 = value;
                ButtonVisible(BtnF7, value, "行追加(F7)");
            }
        }

        public bool F8Visible
        {
            get => F8;
            set
            {
                F8 = value;
                ButtonVisible(BtnF8, value, "行削除(F8)");
            }
        }

        public bool F10Visible
        {
            get => F10;
            set
            {
                F10 = value;
                ButtonVisible(BtnF10, value, "在庫変更(F10)");
            }
        }
        public bool F11Visible
        {
            get => F11;
            set
            {
                F11 = value;
                ButtonVisible(BtnF11, value, "表示(F11)");
            }
        }
        public bool F12Visible
        {
            get => F12;
            set
            {
                F12 = value;
                ButtonVisible(BtnF12, value, "登録(F12)");
            }
        }

        public Control PreviousCtrl { get; set; }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set F9 Button Visible")]
        [DisplayName("F9 Enable")]
        public bool F11Enable
        {
            get => BtnF11.Enabled;
            set => BtnF11.Enabled = value;
        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set F12 Button Visible")]
        [DisplayName("F12 Enable")]
        public bool F12Enable
        {
            get => BtnF12.Enabled;
            set => BtnF12.Enabled = value;
        }

        /// <summary>
        /// 印刷モード
        /// </summary>
        public EPrintMode PrintMode = EPrintMode.NULL;

        #endregion

        #region "内部定数"
        /// <summary>
        ///     ''' 新規モード時の背景色
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        private static System.Drawing.Color INSERT_MODE_COLOR = Color.FromArgb(255, 192, 0);

        /// <summary>
        ///     ''' 修正モード時の背景色
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        private static System.Drawing.Color UPDATE_MODE_COLOR = Color.FromArgb(255, 255, 0);

        /// <summary>
        ///     ''' 削除モード時の背景色
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        private static System.Drawing.Color DELETE_MODE_COLOR = Color.FromArgb(255, 0, 0);

        /// <summary>
        ///     ''' 照会モード時の背景色
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        private static System.Drawing.Color SHOW_MODE_COLOR = Color.FromArgb(0, 176, 240);


        ///// <summary>
        ///// INIファイル名
        ///// </summary>
        ///// 
        //private const string IniFileName = "CKM.ini";
        #endregion

        #region 内部変数

        // 処理モード
        private EProMode _ProMode = EProMode.INPUT;
        private EOperationMode _OperationMode = EOperationMode.NULL;
        private EOperationMode _OldOperationMode = EOperationMode.NULL;

        protected Base_BL bbl = new Base_BL();
        private Login_BL loginbl = new Login_BL();
        private L_Log_Entity lle;
        protected M_AuthorizationsDetails_Entity made;
        private M_Program_Entity mpe;
        private string[] availableStores = null;
        private TextBox txt = null;
        private TextBox txt1 = null;

        /// <summary>
        /// コマンドライン引数
        /// </summary>
        protected string InCompanyCD { get; set; }
        protected string InOperatorCD { get; set; }
        private string InOperatorNM { get; set; }
        protected string InPcID { get; set; }
        protected string InProgramID { get; set; }
        protected string InProgramNM { get; set; }
        protected string StoreAuthorizationsCD { get; set; }
        protected string StoreAuthorizationsChangeDate { get; set; }
        protected string StoreCD { get; set; }

        protected string SoukoCD { get; set; }

        protected CKM_Button Btn_F1 { get => BtnF1; set => BtnF1 = value; }
        protected CKM_Button Btn_F2 { get => BtnF2; set => BtnF2 = value; }

        protected CKM_Button Btn_F3 { get => BtnF3; set => BtnF3 = value; }
        protected CKM_Button Btn_F4 { get => BtnF4; set => BtnF4 = value; }
        protected CKM_Button Btn_F5 { get => BtnF5; set => BtnF5 = value; }
        protected CKM_Button Btn_F6 { get => BtnF6; set => BtnF6 = value; }
        protected CKM_Button Btn_F7 { get => BtnF7; set => BtnF7 = value; }
        protected CKM_Button Btn_F8 { get => BtnF8; set => BtnF8 = value; }
        protected CKM_Button Btn_F9 { get => BtnF9; set => BtnF9 = value; }
        protected CKM_Button Btn_F10 { get => BtnF10; set => BtnF10 = value; }
        protected CKM_Button Btn_F11 { get => BtnF11; set => BtnF11 = value; }
        protected CKM_Button Btn_F12 { get => BtnF12; set => BtnF12 = value; }

        #endregion


        #region "オーバライド可能イベントハンドラ"
        /// <summary>
        ///     ''' 印刷・プレビュー・CSV出力押下時
        ///     ''' </summary>
        ///     ''' <remarks>
        ///     ''' レポート処理を実行
        ///     ''' 各帳票プログラムで必ずオーバーライドする
        ///     ''' </remarks>
        protected virtual void PrintSec()
        {
        }
        /// <summary>
        ///     ''' 表示押下時
        ///     ''' </summary>
        ///     ''' <remarks>プログラムでオーバーライドする</remarks>
        protected virtual void ExecDisp()
        {
        }
        /// <summary>
        ///     ''' 実行・登録押下時
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
        ///     ''' 各プログラムで必ずオーバーライドする
        ///     ''' </remarks>
        protected virtual void EndSec()
        {
        }
        #endregion

        public FrmMainForm()
        {
            InitializeComponent();
        }

        #region "共通部品メソッド"

        /// <summary>
        /// ボータンを見えるか、見えないかの設定
        /// </summary>
        /// <param name="btn">ボタン</param>
        /// <param name="visible">見える=true、見えない=false</param>
        private void ButtonVisible(Button btn, bool visible, string btnText)
        {
            if (visible)
            {
                btn.Text = btnText;
                btn.TabStop = true;
            }
            else
            {
                btn.Text = string.Empty;
                btn.TabStop = false;
            }
        }

        //KTP 2019-05-29 to handle disable enable Clear
        //Disable All Control within param panel
        public void DisablePanel(Panel panel)
        {
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is CKM_MultiLineTextBox)
                    ((CKM_MultiLineTextBox)ctrl).Enabled = false;
                else if (ctrl is CKM_TextBox)
                {
                    ((CKM_TextBox)ctrl).Enabled = false;
                    //if (!Enabled)
                    ((CKM_TextBox)ctrl).BackColor = SystemColors.Control;
                    //else
                    //    this.BackColor = SystemColors.Window;
                }
                else if (ctrl is ComboBox)
                    ((ComboBox)ctrl).Enabled = false;
                else if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Enabled = false;
                else if (ctrl is CKM_SearchControl csc)
                {
                   csc.Enabled=csc.TxtChangeDate.Enabled = false;
                    csc.TxtCode.BackColor = csc.TxtChangeDate.BackColor=SystemColors.Control;
                }
                else if (ctrl is CKM_GridView)
                    ((CKM_GridView)ctrl).DisabledColumn("*");
                else if (ctrl is CKM_RadioButton)
                    ((CKM_RadioButton)ctrl).Enabled = false;
                else if (ctrl is  CKM_CheckBox)
                    ((CKM_CheckBox)ctrl).Enabled = false;
                else if (ctrl is Panel)
                    DisablePanel(ctrl as Panel);
                else if (ctrl is Button)
                    ((Button)ctrl).Enabled = false;
                else if (ctrl is CKM_Button)
                    ((CKM_Button)ctrl).Enabled = false;
            }
        }

        //Enable All Control within param panel
        public void EnablePanel(Panel panel)
        {
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is CKM_MultiLineTextBox)
                    ((CKM_MultiLineTextBox)ctrl).Enabled = true;
                else if (ctrl is CKM_TextBox)
                {
                    ((CKM_TextBox)ctrl).Enabled = true;
                    //if (!Enabled)
                    //    this.BackColor = SystemColors.Control;
                    //else
                    ((CKM_TextBox)ctrl).BackColor = SystemColors.Window;
                }
                else if (ctrl is ComboBox)
                    ((ComboBox)ctrl).Enabled = true;
                else if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Enabled = true;
                else if (ctrl is CKM_SearchControl csc)
                {
                    csc.Enabled = true;
                    csc.TxtCode.Enabled =csc.TxtChangeDate.Enabled= true;
                    csc.TxtChangeDate.BackColor = csc.TxtCode.BackColor = SystemColors.Window;
                }
                else if (ctrl is CKM_GridView)
                    ((CKM_GridView)ctrl).EnabledColumn("*");
                else if (ctrl is CKM_RadioButton )
                    ((CKM_RadioButton)ctrl).Enabled = true;
                else if (ctrl is CKM_CheckBox )
                    ((CKM_CheckBox)ctrl).Enabled = true;
                else if (ctrl is Panel)
                    EnablePanel(ctrl as Panel);

            }
        }

        //Clear All Control within param panel
        public void Clear(Panel panel)
        {
            IEnumerable<Control> c = GetAllControls(panel);
            foreach (Control ctrl in c)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                if (ctrl is ComboBox)
                    ((ComboBox)ctrl).SelectedValue = "-1";
                if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Checked = false;
                if (ctrl is DataGridView)
                {
                    if (((DataGridView)ctrl).DataSource is DataTable dtGrid)
                        dtGrid.Rows.Clear();
                }
                if (ctrl is UserControl)
                {
                    foreach (Control c1 in ctrl.Controls)
                    {
                        if (c1 is TextBox)
                            ((TextBox)c1).Text = string.Empty;
                        if (c1 is Label)
                            ((Label)c1).Text = string.Empty;
                    }
                }

            }
        }

        /// <summary>
        /// Get All control 
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
        }

        /// <summary>
        /// 起動時共通処理
        ///         
        /// check Authorization 
        /// insert to log(form open) 
        /// </summary>
        /// <remarks>プログラム起動時に呼び出してください</remarks>
        protected void StartProgram()
        {
            //共通処理　受取パラメータ、接続情報
            //コマンドライン引数より情報取得
            //Iniファイルより情報取得
            //this.GetCmdLine() == false ||

            //KTP 2019-06-14
            //if (!System.Diagnostics.Debugger.IsAttached)//check Debugger Mode for testing
            //{
            if (this.GetCmdLine() == false || loginbl.ReadConfig() == false)
            {
                //起動時エラー    DB接続不可能
                this.Close();
                System.Environment.Exit(0);
            }
            //}
            //else
            //{
            //    //Debugger Mode
            //    InOperatorCD = "0001";

            //    if (loginbl.ReadConfig() == false)
            //    {
            //        //起動時エラー    DB接続不可能
            //        this.Close();
            //        System.Environment.Exit(0);
            //    }
            //}


            //共通処理　Operator 確認
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD
            };

            mse = loginbl.M_Staff_InitSelect(mse);

            this.lblOperatorName.Text = mse.StaffName;
            this.lblLoginDate.Text = mse.SysDate;
            this.StoreCD = lblStoreCD.Text = mse.StoreCD;

            /// For Default Souko Bind
            M_Staff_Entity mse1 = new M_Staff_Entity
            {
                StaffCD = InOperatorCD
            };
            mse1 = loginbl.M_Souko_InitSelect(mse1);
            this.SoukoCD = mse1.SoukoCD;

            //共通処理　プログラム
            //KTP - 2019-05-29 ProgramのチェックはM_Authorizations_AccessCheck にやっています。
            //[M_Program]
            //mpe = new M_Program_Entity
            //{
            //    ProgramID = InProgramID,
            //    Type = "1"  //仮
            //};

            //bool ret = loginbl.M_Program_InitSelect(mpe);
            //if (ret == false)
            //{
            //    //Ｓ０１２
            //    bbl.ShowMessage("S012");
            //    //起動時エラー
            //    this.Close();
            //    System.Environment.Exit(0);
            //}

            //Authorizations判断
            M_Authorizations_Entity mae;
            mae = new M_Authorizations_Entity
            {
                ProgramID = lblProgramID.Text = this.InProgramID,
                StaffCD = InOperatorCD,
                PC = InPcID
            };

            made = bbl.M_Authorizations_AccessCheck(mae);

            if (made == null)
            {
                bbl.ShowMessage(mae.MessageID);
                //起動時エラー
                this.Close();
                System.Environment.Exit(0);
            }

            //KTP 2019-05-29 Set ProgrameName, ProgramID
            this.Text = made.ProgramID;
            lblHeaderTitle.Text = made.ProgramName;
            this.StoreAuthorizationsCD = lblStoreAuthoCD.Text = made.StoreAuthorizationsCD;
            this.StoreAuthorizationsChangeDate = lblStoreAuthorizationChangeDate.Text = made.StoreAuthorization_ChangeDate;


            //KTP 2019-05-29 M_Authorizations_AccessCheckにやっています。 line no 513 to 521
            //Program type判断
            //switch (mpe.Type)
            //{
            //    case "1":

            //        if (made.Insertable == "0" && made.Updatable == "0" && made.Deletable == "0" && made.Inquirable == "0")
            //        {
            //            //Ｓ００３
            //            bbl.ShowMessage("S003");
            //            //起動時エラー
            //            this.Close();
            //            System.Environment.Exit(0);
            //        }
            //        break;

            //    case "2":
            //        if (made.Printable == "0")
            //        {
            //            //Ｓ００３
            //            bbl.ShowMessage("S003");
            //            //起動時エラー
            //            this.Close();
            //            System.Environment.Exit(0);
            //        }
            //        break;

            //    case "3":
            //        if (made.Printable == "0" && made.Outputable == "0")
            //        {
            //            //Ｓ００３
            //            bbl.ShowMessage("S003");
            //            //起動時エラー
            //            this.Close();
            //            System.Environment.Exit(0);
            //        }
            //        break;

            //    case "4":
            //        if (made.Outputable == "0")
            //        {
            //            //Ｓ００３
            //            bbl.ShowMessage("S003");
            //            //起動時エラー
            //            this.Close();
            //            System.Environment.Exit(0);
            //        }
            //        break;

            //    case "5":
            //        if (made.Inquirable == "0")
            //        {
            //            //Ｓ００３
            //            bbl.ShowMessage("S003");
            //            //起動時エラー
            //            this.Close();
            //            System.Environment.Exit(0);
            //        }
            //        break;

            //    case "6":
            //        if (made.Runable == "0")
            //        {
            //            //Ｓ００３
            //            bbl.ShowMessage("S003");
            //            //起動時エラー
            //            this.Close();
            //            System.Environment.Exit(0);
            //        }
            //        break;
            //}

            //処理可能店舗
            //[M_StoreAuthorizations]
            M_StoreAuthorizations_Entity msa = new M_StoreAuthorizations_Entity
            {
                StoreAuthorizationsCD = made.StoreAuthorizationsCD
            };

            DataTable dt = bbl.M_StoreAuthorizations_Select(msa);
            if (dt.Rows.Count > 0)
            {
                availableStores = new string[dt.Rows.Count];
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    availableStores[i] = row["StoreCD"].ToString();
                    i++;
                }
            }

            //プログラム起動履歴
            //InsertLog(Get_L_Log_Entity(true));

            //KTP 2019-05-29 Program Type Select from M_Authorizations_AccessCheck function
            //Todo:入力プログラム以外を考慮
            //switch (mpe.Type)
            switch (made.ProgramType)
            {
                case "1":
                    if (made.Insertable == "1")
                    {
                        // 新規ボタン押下処理
                        FunctionProcess((int)EOperationMode.INSERT);
                    }
                    else if (made.Updatable == "1")
                    {
                        // 修正ボタン押下処理
                        FunctionProcess((int)EOperationMode.UPDATE);
                    }
                    else if (made.Deletable == "1")
                    {
                        // 削除ボタン押下処理
                        FunctionProcess((int)EOperationMode.DELETE);
                    }
                    else
                    {
                        // 照会ボタン押下処理
                        FunctionProcess((int)EOperationMode.SHOW);
                    }
                    break;
            }
        }

        /// <summary>
        ///     ''' ファンクションラベルを一括で編集
        ///     ''' </summary>
        ///     ''' <param name="mode">eProMode</param>
        /// <remarks>プログラム起動時に呼び出してください</remarks>
        protected void SetFunctionLabel(EProMode mode)
        {
            this._ProMode = mode;

            switch (mode)
            {
                case EProMode.INPUT:
                    {
                        this.BtnF7.Text = "行追加(F7)";
                        this.BtnF8.Text = "行削除(F8)";
                        this.BtnF10.Text = "行複写(F10)";
                        this.BtnF11.Text = "表示(F11)";
                        break;
                    }

                case EProMode.MENTE:
                    {
                        this.BtnF2.Text = "新規(F2)";
                        this.BtnF7.Text = "";
                        this.BtnF8.Text = "";
                        this.BtnF10.Text = "";
                    }
                    break;

                case EProMode.SHOW:
                    {
                        this.BtnF2.Text = "";
                        this.BtnF3.Text = "";
                        this.BtnF4.Text = "";
                        this.BtnF5.Text = "";
                        this.BtnF7.Text = "";
                        this.BtnF8.Text = "";
                        this.BtnF10.Text = "";
                        this.BtnF11.Text = "表示(F11)";
                        this.BtnF12.Text = "";
                        this.ModeVisible = false;
                        break;
                    }

                case EProMode.BATCH:
                    {
                        this.BtnF2.Text = "";
                        this.BtnF3.Text = "";
                        this.BtnF4.Text = "";
                        this.BtnF5.Text = "";
                        this.BtnF7.Text = "";
                        this.BtnF8.Text = "";
                        this.BtnF10.Text = "";
                        this.BtnF11.Text = "";
                        this.ModeVisible = false;
                        break;
                    }
                case EProMode.PRINT:
                    {
                        this.BtnF2.Text = "";
                        this.BtnF3.Text = "";
                        this.BtnF4.Text = "";
                        this.BtnF5.Text = "";
                        this.BtnF7.Text = "";
                        this.BtnF8.Text = "";
                        this.BtnF10.Text = "CSV(F10)";
                        this.BtnF11.Text = "PDF(F11)";
                        this.BtnF12.Text = "印刷(F12)";
                        this.ModeVisible = false;
                        break;
                    }

                case EProMode.MENU:
                    {
                        this.BtnF1.Text = "";
                        this.BtnF2.Text = "";
                        this.BtnF3.Text = "";
                        this.BtnF4.Text = "";
                        this.BtnF5.Text = "";
                        this.BtnF6.Text = "";
                        this.BtnF7.Text = "";
                        this.BtnF8.Text = "";
                        this.BtnF10.Text = "";
                        this.BtnF11.Text = "ﾛｸﾞｱｳﾄ(F11)";
                        this.lblHeaderTitle.Visible = false;
                        this.ModeVisible = false;
                        break;
                    }
                case EProMode.JUCHUTORIKOMIAPI:
                    {

                        this.BtnF2.Text = "";
                        this.BtnF3.Text = "";
                        this.BtnF4.Text = "";
                        this.BtnF5.Text = "";
                        this.BtnF6.Text = "";
                        this.BtnF7.Text = "";
                        this.BtnF8.Text = "";
                        this.BtnF9.Text = "";
                        this.BtnF10.Text = "";
                        this.BtnF11.Text = "表示(F11)";
                        this.BtnF12.Text = "印刷(F12)";
                        this.lblMode.Visible = false;
                        break;
                    }
                case EProMode.KehiNyuuryoku:
                    {
                        this.BtnF12.Text = "登録(F12)";
                        //this.lblHeaderTitle.Visible = false;
                        //this.ModeVisible = true;
                        break;
                    }
            }
        }
        public void SetFuncKeyAll(object Obj, string strBuf)
        {
            this.SetFuncKeyAll(Obj, strBuf, "");
        }

        public void SetFuncKeyAll(object Obj, string strBuf, string strBuf2)
        {
            // ファンクションキーの有効／無効の設定（全指定）
            // strBuf ← 0(有効)か1(無効)を順番に指定

            int X;
            //Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString s = new Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(1);
            string s = "";

            for (X = 1; X <= 12; X++)
            {
                //s.Value = Microsoft.VisualBasic.Strings.Mid(strBuf, X, 1);
                s = Microsoft.VisualBasic.Strings.Mid(strBuf, X, 1);
                //if (s.Value == "1")
                if (s == "1")
                    SetFuncKey(Obj, (short)(X - 1), true);
                else
                    SetFuncKey(Obj, (short)(X - 1), false);
            }

            if (Microsoft.VisualBasic.Strings.Len(strBuf2) == 8)
            {
                for (X = 1; X <= 8; X++)
                {
                    //s.Value = Microsoft.VisualBasic.Strings.Mid(strBuf2, X, 1);
                    s = Microsoft.VisualBasic.Strings.Mid(strBuf2, X, 1);
                    //if (s.Value == "1")
                    if (s == "1")
                        SetFuncKey(Obj, (short)(X + 11), true);
                    else
                        SetFuncKey(Obj, (short)(X + 11), false);
                }
            }
        }

        public void SetFuncKey(object Obj, short Index, bool flg)
        {
            Control[] Obj_Btn = { this.BtnF1,this.BtnF2,this.BtnF3,this.BtnF4,this.BtnF5,this.BtnF6
                    ,this.BtnF7,this.BtnF8,this.BtnF9,this.BtnF10,this.BtnF11,this.BtnF12 };

            // ラベルのテキストが空白の場合は使用不可とする
            if (flg && Obj_Btn[Index].Text == "")
                flg = false;

            //switch (mpe.Type)
            //KTP 2019-05-29 Program Type Select from M_Authorizations_AccessCheck function
            switch (made.ProgramType)
            {
                case "1":
                    if (Index == (int)EOperationMode.INSERT && made.Insertable == "0")
                        flg = false;
                    if (Index == (int)EOperationMode.UPDATE && made.Updatable == "0")
                        flg = false;
                    if (Index == (int)EOperationMode.DELETE && made.Deletable == "0")
                        flg = false;
                    if (Index == (int)EOperationMode.SHOW && made.Inquirable == "0")
                        flg = false;

                    break;

                case "3":
                    //Todo:未検証
                    if (Index == (int)EPrintMode.DIRECT && made.Printable == "0")
                    {
                        flg = false;
                    }

                    if (Index == (int)EPrintMode.PDF && made.Printable == "0")
                    {
                        flg = false;
                    }

                    if (Index == (int)EPrintMode.CSV && made.Outputable == "0")
                    {
                        flg = false;
                    }

                    break;
            }

            //KTP 2019-06-05 neglet for F9
            if (Index != 8)
                Obj_Btn[Index].Enabled = flg;

        }
        //*********************************************************************
        /// <summary> mmの値をtwipに変換する
        /// </summary>
        /// <param name="mmValue">長さ(mm)</param>
        /// <returns>             長さ(twip)</returns>
        //*********************************************************************
        public int mmToTwip(int mmValue)
        {
            return (int)(mmValue * 56.6929);
        }

        /// <summary>
        /// 「名前を付けて保存」ダイアログでファイルを保存する
        /// </summary>
        /// <param name="initFileName"></param>
        /// <param name="outputFileName"></param>
        /// <param name="kbn">0:フィルターなし,1:Excel</param>
        /// <returns></returns>
        public bool ShowSaveFileDialog(string initFileName, out string outputFileName, int kbn = 0)
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
                sfd.DefaultExt = "xlsx";
            }
            if (kbn == 2) // Shiiretankateireishou by PTk 2020/11/05
            {
                // 例：Excelファイルを開く場合（Office2003と2007両対応したい）
                sfd.Filter = "Excelファイル(*.xls;)|*.xls;";
                sfd.DefaultExt = "xls";
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

        /// <summary>
        /// PDF出力処理
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="report">レポートオブジェクト</param>
        /// <returns></returns>
        public bool OutputPDF(string filePath, ReportClass report)
        {
            // PDF形式でファイル出力
            try
            {
                string fileName = "";
                if (System.IO.Path.GetExtension(filePath).ToLower() != ".pdf")
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(filePath) + ".pdf";
                }

                // 出力先ファイル名を指定
                CrystalDecisions.Shared.DiskFileDestinationOptions fileOption;
                fileOption = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                fileOption.DiskFileName = System.IO.Path.GetDirectoryName(filePath) + "\\" + fileName;

                // 外部ファイル出力をPDF出力として定義する
                CrystalDecisions.Shared.ExportOptions option;
                option = report.ExportOptions;
                option.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                option.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                option.FormatOptions = new CrystalDecisions.Shared.PdfRtfWordFormatOptions();
                option.DestinationOptions = fileOption;

                // pdfとして外部ファイル出力を行う
                report.Export();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMainForm_Load(object sender, EventArgs e)
        {
            //this.lblHeaderTitle.BackColor = Color.FromArgb(112, 173, 71);
        }


        /// <summary>
        /// コマンドライン引数取得処理
        /// </summary>
        private bool GetCmdLine()
        {
            //コマンドライン引数を配列で取得する
            string[] cmds = System.Environment.GetCommandLineArgs();

            //コマンドライン引数を列挙する
            for (int count = 0; count < cmds.Length; count++)
            {
                if (count == (int)ECmdLine.CompanyCD)
                {
                    InCompanyCD = cmds[(int)ECmdLine.CompanyCD];
                }
                else if (count == (int)ECmdLine.OperatorCD)
                {
                    InOperatorCD = cmds[(int)ECmdLine.OperatorCD];
                }
                else if (count == (int)ECmdLine.PcID)
                {
                    InPcID = cmds[(int)ECmdLine.PcID];
                }
            }

            if (InOperatorCD.Trim() == "")
            {
                //オペレータコードを取得できませんでした
                return false;
            }
            else if (InPcID.Trim() == "")
            {
                //コンピュータ名を取得できませんでした
                return false;
            }
            else if (InCompanyCD.Trim() == "")
            {
                //入力営業所コードを取得できませんでした
                return false;
            }
            return true;
        }

        /// <summary>
        /// get Log information
        /// form close log
        /// </summary>
        /// <returns>return L_Log_Entity</returns>
        private L_Log_Entity Get_L_Log_Entity()
        {
            lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "Close"
            };
            return lle;
        }

        /// <summary>
        /// get Log information
        /// form open log
        /// </summary>
        /// <returns>return L_Log_Entity</returns>
        private L_Log_Entity Get_L_Log_Entity(bool start)
        {
            lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "Open"
            };
            return lle;
        }
        /// <summary>
        /// insert to Log table
        /// </summary>
        /// <param name="lle"></param>
        protected void InsertLog(L_Log_Entity lle)
        {
            bbl.L_Log_Insert(lle);
        }

        /// <summary>
        /// Insert to log table on form closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                InsertLog(Get_L_Log_Entity());
            }
            catch (Exception ex)
            {
                //例外は無視する
                //DB接続できなかった場合もあるので
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

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
                Button btn = (Button)sender;//新規(F2) 変更(F3) 削除(F4) 照会(F5) ｷｬﾝｾﾙ(F6)
                if (!string.IsNullOrWhiteSpace(btn.Text))
                {
                    if (btn.Text == "終了(F1)" || btn.Text == "新規(F2)" || btn.Text ==  "変更(F3)"|| btn.Text == "削除(F4)"|| btn.Text == "照会(F5)"|| btn.Text == "ｷｬﾝｾﾙ(F6)")
                    {
                        ButtonFunction(btn.Tag.ToString());
                    }
                    else
                    {
                        if (TxtCode_FullWidth())
                        {
                            ButtonFunction(btn.Tag.ToString());
                        }
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
                if (bbl.ShowMessage("Q003") == DialogResult.Yes)
                {

                    var ctrl = GetAllControls(this);
                    

                    foreach (var ctrlTxt in ctrl)
                    {
                        if(ctrlTxt is CKM_TextBox)
                        {
                            ((CKM_TextBox)ctrlTxt).isClosing = true;
                        }
                        //ctrlTxt.isClosing = true;
                    }
                    EndSec();
                }
                else
                {
                    if (PreviousCtrl != null)
                        PreviousCtrl.Focus();
                    else
                        BtnF1.Focus();
                }
                return;
            }
            else if (Index < 5)
            {
                if (this._ProMode != EProMode.SHOW)
                {
                    //処理モード変更時  Todo:入力プログラム以外を考慮
                    if (bbl.ShowMessage("Q005") != DialogResult.Yes)
                    {
                        PreviousCtrl.Focus();
                        return;
                    }
                }
            }

            FunctionProcess(Index);

        }

        /// <summary>
        /// handle Function Key F1 to F12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMainForm_KeyDown(object sender, KeyEventArgs e)
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
                            Button btn = PanelFooter.Controls.Find("btn" + e.KeyCode.ToString(), true)[0] as Button;
                            if (btn.Enabled)

                            {
                                
                                if (e.KeyCode == Keys.F12)
                                {
                                    if (TxtCode_FullWidth(e))
                                    {
                                        PreviousCtrl = ActiveControl;
                                        if (!string.IsNullOrWhiteSpace(btn.Text))
                                            ButtonFunction(btn.Tag.ToString());
                                    }
                                }
                                else
                                {
                                    PreviousCtrl = ActiveControl;
                                    if (!string.IsNullOrWhiteSpace(btn.Text))
                                        ButtonFunction(btn.Tag.ToString());

                                    if (e.KeyCode.Equals(Keys.F10))
                                        e.Handled = true;

                                }
                             
                            }
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
        public bool TxtCode_FullWidth(KeyEventArgs e=null)  // PTk Added 4/21/2020
        {
            
            Base_BL bbl = new Base_BL();
            UserControl sc = ActiveControl as UserControl;

            var c = GetAllControls(this);
            for (int i = 0; i < c.Count(); i++)
            {
                var Con = c.ElementAt(i) as UserControl;
                var ConTxt= c.ElementAt(i) as CKM_TextBox;
                if (Con is CKM_SearchControl)
                {
                    var gc = (Con as CKM_SearchControl);
                    try
                    {
                        Control ctrl = Con.Controls.Find("txtCode", true)[0];
                        if (gc.TxtCode.Ctrl_Byte != CKM_TextBox.Bytes.半全角)
                        {
                            if (IsConsistFullWidth((ctrl as CKM_TextBox).Text)) // Half  // PTK
                            {
                                bbl.ShowMessage("E221");
                                (ctrl as CKM_TextBox).Focus();
                                (ctrl as CKM_TextBox).MoveNext = false;
                                return false;
                            }
                        }
                        else // Hald/Full
                        {
                            IsMaxLeng(gc.TxtCode);
                        }
                    }
                    catch
                    {
                        return true;
                    }
                }
                else if(ConTxt is CKM_TextBox)
                {
                    (ConTxt as CKM_TextBox).isMaxLengthErr = false; 

                    if (!ConTxt.Enabled)    //2020/5/18 add
                        continue;

                    if ((((ConTxt as CKM_TextBox).Ctrl_Type == CKM_TextBox.Type.Normal) || (ConTxt as CKM_TextBox).Ctrl_Type == CKM_TextBox.Type.Number) && (ConTxt as CKM_TextBox).Ctrl_Byte == CKM_TextBox.Bytes.半全角)
                    {
                        string str = Encoding.GetEncoding(932).GetByteCount((ConTxt as CKM_TextBox).Text).ToString();
                        if (Convert.ToInt32(str) > (ConTxt as CKM_TextBox).Length)
                        {
                            MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            (ConTxt as CKM_TextBox).isMaxLengthErr = true;
                            (ConTxt as CKM_TextBox).Focus();
                            return false;
                        }
                    }
                    if ((((ConTxt as CKM_TextBox).Ctrl_Type == CKM_TextBox.Type.Normal) || (ConTxt as CKM_TextBox).Ctrl_Type == CKM_TextBox.Type.Number) && (ConTxt as CKM_TextBox).Ctrl_Byte == CKM_TextBox.Bytes.半角)
                    {
                        int byteCount = Encoding.GetEncoding("Shift_JIS").GetByteCount((ConTxt as CKM_TextBox).Text);
                        int onebyteCount = System.Text.ASCIIEncoding.ASCII.GetByteCount((ConTxt as CKM_TextBox).Text);
                        if (onebyteCount != byteCount)
                        {
                            MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            (ConTxt as CKM_TextBox).isMaxLengthErr = true;
                            (ConTxt as CKM_TextBox).Focus();
                            return false;
                        }
                    }

                }
            }
            return true;
        }
        private void IsMaxLeng(CKM_TextBox ConTxt) // PTK
        {
            (ConTxt as CKM_TextBox).isMaxLengthErr = false;

            if (!ConTxt.Enabled)    //2020/5/18 add
                return ; ;

            if ((((ConTxt as CKM_TextBox).Ctrl_Type == CKM_TextBox.Type.Normal) || (ConTxt as CKM_TextBox).Ctrl_Type == CKM_TextBox.Type.Number) && (ConTxt as CKM_TextBox).Ctrl_Byte == CKM_TextBox.Bytes.半全角)
            {
                string str = Encoding.GetEncoding(932).GetByteCount((ConTxt as CKM_TextBox).Text).ToString();
                if (Convert.ToInt32(str) > (ConTxt as CKM_TextBox).MaxLength)
                {
                    MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    (ConTxt as CKM_TextBox).isMaxLengthErr = true;
                    (ConTxt as CKM_TextBox).Focus();
                    return ;
                }
            }
            if ((((ConTxt as CKM_TextBox).Ctrl_Type == CKM_TextBox.Type.Normal) || (ConTxt as CKM_TextBox).Ctrl_Type == CKM_TextBox.Type.Number) && (ConTxt as CKM_TextBox).Ctrl_Byte == CKM_TextBox.Bytes.半角)
            {
                int byteCount = Encoding.GetEncoding("Shift_JIS").GetByteCount((ConTxt as CKM_TextBox).Text);
                int onebyteCount = System.Text.ASCIIEncoding.ASCII.GetByteCount((ConTxt as CKM_TextBox).Text);
                if (onebyteCount != byteCount)
                {
                    MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    (ConTxt as CKM_TextBox).isMaxLengthErr = true;
                    (ConTxt as CKM_TextBox).Focus();
                    return ;
                }
            }
           
        }
        public static bool IsConsistFullWidth(string txt)
        {
            var c = txt.ToCharArray();
            foreach (char chr in c)
            {
                if (System.Text.Encoding.GetEncoding("shift_JIS").GetByteCount(chr.ToString()) == 2)
                {
                    return true;
                }

            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Index"></param>
        public virtual void FunctionProcess(int Index)
        {

            switch (this._ProMode)
            {
                case EProMode.INPUT:
                case EProMode.MENTE: // 入力プログラム
                    {
                        switch (Index)
                        {
                            case (short)EOperationMode.INSERT: // F2：新規
                                {
                                    this.lblMode.BackColor = INSERT_MODE_COLOR;
                                    break;
                                }

                            case (short)EOperationMode.UPDATE: // F3：修正
                                {
                                    this.lblMode.BackColor = UPDATE_MODE_COLOR;
                                    break;
                                }

                            case (short)EOperationMode.DELETE: // F4：削除
                                {
                                    this.lblMode.BackColor = DELETE_MODE_COLOR;
                                    break;
                                }
                            case (short)EOperationMode.SHOW: // F5：表示
                                {
                                    this.lblMode.BackColor = SHOW_MODE_COLOR;
                                    break;
                                }

                            // 表示ファンクション
                            case FuncDisp - 1:
                                {
                                    this.Cursor = Cursors.WaitCursor;
                                    ExecDisp();
                                    this.Cursor = Cursors.Default;
                                    break;
                                }
                        }

                        break;
                    }

                case EProMode.PRINT: // 帳票プログラム
                    {
                        switch (Index)
                        {
                            case (short)EPrintMode.DIRECT: // F12：印刷
                                {
                                    this.PrintMode = EPrintMode.DIRECT;
                                    break;
                                }
                            case (short)EPrintMode.CSV: // F10：CSV
                                {
                                    this.PrintMode = EPrintMode.CSV;
                                    break;
                                }
                            case (short)EPrintMode.PDF: // F11：PDF
                                {
                                    this.PrintMode = EPrintMode.PDF;
                                    break;
                                }

                            default:
                                {
                                    this.PrintMode = EPrintMode.NULL;
                                    return;
                                }
                        }
                        // 実行ファンクション
                        if (this.PrintMode != EPrintMode.NULL)
                        {
                            // 実行処理
                            this.Cursor = Cursors.WaitCursor;
                            PrintSec();
                            this.Cursor = Cursors.Default;
                        }
                        break;
                    }

                case EProMode.SHOW: // 照会
                    {
                        // 実行ファンクション
                        if (Index + 1 == FuncDisp)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            ExecDisp();
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        break;
                    }

                case EProMode.BATCH: // バッチプログラム
                    {
                        // 実行ファンクション
                        if (Index + 1 == FuncExec)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            ExecSec();
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        break;
                    }
            }
        }
        //入力可能店舗チェック
        protected bool CheckAvailableStores(string storeCD)
        {
                if (availableStores != null && Array.IndexOf(availableStores, storeCD) >= 0)
                    return true;

                else
                    return false;
            
        }
        protected string GetAllAvailableStores()
        {
            if (availableStores == null)
            {
                return "";
            }
            string ret = availableStores[0];

            for (int i = 1; i < availableStores.Length; i++)
            {
                ret += "," + availableStores[i];
            }
            return ret;
        }
        public static int GetResultWithHasuKbn(int kbn, decimal d)
        {
            int result = 0;
            try
            {
                switch (kbn)
                {
                    case (int)HASU_KBN.KIRIAGE:
                        if (d < 0)
                        {
                            result = -1 * Convert.ToInt32(Math.Ceiling(-d));
                        }
                        else
                        {
                            result = Convert.ToInt32(Math.Ceiling(d));
                        }
                        break;

                    case (int)HASU_KBN.KIRISUTE:
                        result = Convert.ToInt32(Math.Truncate(d));
                        break;

                    case (int)HASU_KBN.SISYAGONYU:
                        result = Convert.ToInt32(Math.Round(d, MidpointRounding.AwayFromZero));
                        break;
                }
            }
            catch
            {
                //桁数エラー
                result = 0;
            }

            return result;
        }
        // フォーム右上の閉じるボタンを無効にする
        // CreateParams プロパティをオーバーライドする
        // 「閉じる」ボタンが無効状態となり、押すことができなくなります。システムメニューの「閉じる」も表示されなくなり、
        // 「Alt」+「F4」キーも無効になります。
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
                else if (c is CKMShop_ComboBox)
                {
                    if (((CKMShop_ComboBox)c).SelectedIndex <= 0)
                    {
                        bbl.ShowMessage("E102");
                        c.Focus();
                        return false;
                    }
                }
            }
            return true;
        }
        protected bool ReverseRequireCheck(Control[] ctrl, TextBox txt = null)
        {
            txt1 = txt;
            foreach (Control c in ctrl)
            {
                if (c is CKM_TextBox)
                {
                    if (string.IsNullOrWhiteSpace(txt1.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
                        {
                            bbl.ShowMessage("E102");
                            txt1.Focus();
                            return false;
                        }
                    }

                }
            }
            return true;
        }
        //protected bool RequireCheck(Control[] ctrl, TextBox txt = null)
        //{
        //    this.txt = txt;
        //    foreach (Control c in ctrl)
        //    {
        //        if (c is CKM_TextBox)
        //        {
        //            if (txt == null)
        //            {
        //                if (string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
        //                {
        //                    //  if (((CKM_TextBox)c).Name != "txtChangeDate")
        //                    //if (txt.Name != "txtChangeDate")
        //                    //{
        //                        bbl.ShowMessage("E102");
        //                        c.Focus();
        //                    ((CKM_TextBox)c).IsFirstTime = false;
        //                    //}
        //                    return false;
        //                    //bbl.ShowMessage("E102");
        //                    //c.Focus();
        //                    //return false;
        //                }
        //            }
        //            else if (!string.IsNullOrWhiteSpace(txt.Text))
        //            {
        //                if (string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
        //                {
        //                    ////  if (((CKM_TextBox)c).Name != "txtChangeDate")
        //                    //if (txt.Name != "txtChangeDate")
        //                    //  {

        //                        bbl.ShowMessage("E102");
        //                        c.Focus();
        //                    ((CKM_TextBox)c).IsFirstTime = false;
        //                    //}
        //                    return false;
        //                }

        //            }
        //        }
        //        else if (c is CKM_ComboBox)
        //        {
        //            if (((CKM_ComboBox)c).SelectedIndex.Equals(-1))
        //            {
        //                bbl.ShowMessage("E102");
        //                c.Focus();
        //                return false;
        //            }
        //            if (((CKM_ComboBox)c).SelectedValue.Equals("-1"))
        //            {
        //                bbl.ShowMessage("E102");
        //                c.Focus();
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        //protected bool ReverseRequireCheck(Control[] ctrl, TextBox txt = null)
        //{
        //    txt1 = txt;
        //    foreach (Control c in ctrl)
        //    {
        //        if (c is CKM_TextBox)
        //        {
        //            if (string.IsNullOrWhiteSpace(txt1.Text))
        //            {
        //                if (!string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
        //                {
        //                    //   if (((CKM_TextBox)c).Name != "txtChangeDate")
        //                    //if (txt.Name != "txtChangeDate")
        //                    //{
        //                    ((CKM_TextBox)c).IsFirstTime = false;
        //                    bbl.ShowMessage("E102");
        //                        txt1.Focus();
        //                    //}
        //                    return false;
        //                }
        //            }

        //        }
        //    }
        //    return true;
        //}
        //protected bool RequireCheck(Control[] ctrl, TextBox txt = null)
        //{
        //    this.txt = txt;
        //    foreach (Control c in ctrl)
        //    {
        //        if (c is CKM_TextBox)
        //        {
        //            if (txt == null)
        //            {
        //                if (string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
        //                {
        //                    bbl.ShowMessage("E102");
        //                    c.Focus();
        //                    return false;
        //                }
        //            }

        //            else if (string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
        //            {
        //                bbl.ShowMessage("E102");
        //                c.Focus();
        //                return false;
        //            }
        //            //else if (!string.IsNullOrWhiteSpace(txt.Text))
        //            //{
        //            //    //if (((CKM_TextBox)c).Name == "txtChangeDate")
        //            //    //{
        //            //    //    if (!DateCheck(((CKM_TextBox)c).Text))
        //            //    //    {
        //            //    //        if (Datetemp == "")
        //            //    //        {
        //            //    //            return false;
        //            //    //        }


        //            //    //    }
        //            //    //    else
        //            //    //    {
        //            //    //        SetTxtInMain(Datetemp, ((CKM_TextBox)c).Parent.Name);

        //            //    //    }

        //            //    //}
        //            //    if (string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
        //            //    {
        //            //        bbl.ShowMessage("E102");
        //            //        c.Focus();
        //            //        return false;
        //            //    }

        //            //}
        //        }
        //        else if (c is CKM_ComboBox)
        //        {
        //            if (((CKM_ComboBox)c).SelectedIndex.Equals(-1))
        //            {
        //                bbl.ShowMessage("E102");
        //                c.Focus();
        //                return false;
        //            }
        //            if (((CKM_ComboBox)c).SelectedValue.Equals("-1"))
        //            {
        //                bbl.ShowMessage("E102");
        //                c.Focus();
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}
        //protected bool ReverseRequireCheck(Control[] ctrl, TextBox txt = null)
        //{
        //    txt1 = txt;
        //    foreach (Control c in ctrl)
        //    {
        //        if (c is CKM_TextBox)
        //        {
        //            if (string.IsNullOrWhiteSpace(txt1.Text))
        //            {
        //                //if (((CKM_TextBox)c).Name == "txtChangeDate")
        //                //{
        //                //    if (!DateCheck(((CKM_TextBox)c).Text))
        //                //    {

        //                //        if (Datetemp == "")
        //                //        {
        //                //            return false;
        //                //        }
        //                //    }else
        //                //        SetTxtInMain(Datetemp, ((CKM_TextBox)c).Parent.Name);


        //                //}
        //                if (!string.IsNullOrWhiteSpace(((CKM_TextBox)c).Text))
        //                {
        //                  //  Datetemp = "";
        //                    bbl.ShowMessage("E102");
        //                    txt1.Focus();
        //                    return false;
        //                }


        //            }

        //        }
        //    }
        //    return true;
        //}
        string Datetemp = "";
        private void BtnF1_MouseEnter(object sender, EventArgs e)
        {
            var ctrl = GetAllControls(this);


            foreach (var ctrlTxt in ctrl)
            {
                if (ctrlTxt is CKM_TextBox)
                {
                    ((CKM_TextBox)ctrlTxt).isClosing = true;
                }
                //ctrlTxt.isClosing = true;
            }
            PreviousCtrl = this.ActiveControl;
        }



        private Form IsmaxTabIndex(KeyEventArgs e, Control cr) //PTK
        {
            Form fr = null;
            if (cr.Parent is Panel)
            {
                int maxtab = 0;
                foreach (Control c in cr.Parent.Controls)
                {
                    if (c is CKM_TextBox || c is CKM_ComboBox || c is CKM_MultiLineTextBox)
                    {
                        maxtab = c.TabIndex;

                    }
                }

                return null;

            }
            else
            {
                if (cr.Parent is null)
                    return null;
                fr = IsmaxTabIndex(e, cr.Parent);
            }
            return fr;

        }
        public void MoveNextControl(KeyEventArgs e)  //PTK  Addedd// if Something Changed, Discuss with PTK


        {

            IsmaxTabIndex(e, ActiveControl);
            if (e.KeyCode == Keys.F12)
            {
                if (ActiveControl is UserControl)
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
                    else
                        return;
                }
            }
            if (e.KeyCode == Keys.Menu || e.KeyCode == Keys.ProcessKey)
            {
                return;
            }
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
                else if (ActiveControl is CKM_MultiLineTextBox)
                {
                    var con = (ActiveControl as CKM_MultiLineTextBox);
                    var g = con.Cursorout.Replace("\r", "").Replace("\n", "");
                    var f = con.Cursorin.Replace("\r", "").Replace("\n", "");
                    //var g = (ActiveControl as CKM_MultiLineTextBox).Cursorout.Replace("\r", "").Replace("\n", "");
                    //var f = (ActiveControl as CKM_MultiLineTextBox).Cursorin.Replace("\r", "").Replace("\n", "");
                    if (e.Control && e.KeyCode == Keys.Enter)
                    {
                        if (this.Parent != null)
                            this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        else    /// Just a While  Wait not Confirm by PTK  For (Alt+Enter)  Link to Multiline_Textbox
                        {
                            (ActiveControl as CKM_MultiLineTextBox).Focus();
                            // (ActiveControl as CKM_MultiLineTextBox).MoveNext = false;
                        }
                    }
                    else if (f == g)
                    {
                        // var d = PreviousCtrl;
                        if (!con.Mdea && !con.F_focus)
                        {
                            this.SelectNextControl(ActiveControl, true, true, true, true);

                        }
                        else
                        {
                            con.Mdea = false;
                        }
                    }
                    else
                    {

                    }


                }
            
                else if ((ActiveControl is TextBox))
                {
                    if ((ActiveControl as TextBox).Multiline)
                    {
                        // Make 
                    }
                    else
                    {
                        if (ActiveControl.Parent.Parent is CKM_GridView)
                        {
                        var f=    (this.ActiveControl as TextBox).Text;
                        }
                        else
                        this.SelectNextControl(ActiveControl, true, true, true, true);
                    }
                }
                else if ((ActiveControl is CKM_RadioButton) || (ActiveControl is CKM_CheckBox))
                {
                    this.SelectNextControl(ActiveControl, true, true, true, true);
                    //CheckBoxやRadioButtonのフォーカス移動を自動にされると制御できないため　2020/5/25
                    //f comment on these place, we can't move another control from radio button or checkbox on Enter Key Press. 2020/06/01
                    //this.SelectNextControl(ActiveControl, true, true, true, true);
                }
            }
         

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

        private void FrmMainForm_KeyUp(object sender, KeyEventArgs e)
        {
            //MoveNextControl(e);
        }

        
        private bool DateCheck(string Text1 = null)
        {
            bbl = new Base_BL();
            if (!string.IsNullOrWhiteSpace(Text1))
            {
                if (bbl.IsInteger(Text1.Replace("/", "").Replace("-", "")))
                {
                    string day = string.Empty, month = string.Empty, year = string.Empty;
                    if (Text1.Contains("/"))
                    {
                        string[] date = Text1.Split('/');
                        day = date[date.Length - 1].PadLeft(2, '0');
                        month = date[date.Length - 2].PadLeft(2, '0');

                        if (date.Length > 2)
                            year = date[date.Length - 3];

                        Text1 = year + month + day;//  this.Text.Replace("/", "");
                    }
                    else if (Text1.Contains("-"))
                    {
                        string[] date = Text1.Split('-');
                        day = date[date.Length - 1].PadLeft(2, '0');
                        month = date[date.Length - 2].PadLeft(2, '0');

                        if (date.Length > 2)
                            year = date[date.Length - 3];

                        Text1 = year + month + day;//  this.Text.Replace("-", "");
                    }

                    string text = Text1;
                    text = text.PadLeft(8, '0');
                    day = text.Substring(text.Length - 2);
                    month = text.Substring(text.Length - 4).Substring(0, 2);
                    year = Convert.ToInt32(text.Substring(0, text.Length - 4)).ToString();

                    if (month == "00")
                    {
                        month = string.Empty;
                    }
                    if (year == "0")
                    {
                        year = string.Empty;
                    }

                    if (string.IsNullOrWhiteSpace(month))
                        month = DateTime.Now.Month.ToString().PadLeft(2, '0');//if user doesn't input for month,set current month

                    if (string.IsNullOrWhiteSpace(year))
                    {
                        year = DateTime.Now.Year.ToString();//if user doesn't input for year,set current year
                    }
                    else
                    {
                        if (year.Length == 1)
                            year = "200" + year;
                        else if (year.Length == 2)
                            year = "20" + year;
                    }

                    //string strdate = year + "-" + month + "-" + day;  2019.6.11 chg
                    string strdate = year + "/" + month + "/" + day;
                    if (bbl.CheckDate(strdate))
                    {
                        //   IsCorrectDate = true;
                        Text1 = strdate;
                        Datetemp = strdate;
                    }
                    else
                    {
                        Datetemp = "";
                      //  ShowErrorMessage("E103");
                        return false;
                    }
                }
                else
                {
                    Datetemp = "";
                  //  ShowErrorMessage("E103");
                    return false;
                }
            }

            return true;
        }

        protected void SetTxtInMain(string Date, string CtrName)
        {
            var c = GetAllControls(this);
            for (int i = 0; i < c.Count(); i++)
            {
                var Con = c.ElementAt(i) as UserControl;
                if (Con is CKM_SearchControl  && Con.Name == CtrName)
                {
                    try
                    {
                        Control ctrl = Con.Controls.Find("txtChangeDate", true)[0];
                        //if (IsConsistFullWidth((ctrl as CKM_TextBox).Text))
                        //{
                        //    bbl.ShowMessage("E221");
                        //    //(ctrl as CKM_TextBox).Focus();
                        //    //(ctrl as CKM_TextBox).MoveNext = false;
                        //  return ;
                        //}
                        ctrl.Text = Date;
                        return;
                    }
                    catch
                    {
                        return ;
                    }
                }
            }
            }

        private void ShowErrorMessage(string messageID)
        {
            bbl.ShowMessage(messageID);
            //MoveNext = false;
            //this.SelectionStart = 0;
            //this.SelectionLength = this.Text.Length;
        }

        private void BtnF1_MouseLeave(object sender, EventArgs e)
        {
            var ctrl = GetAllControls(this);

            foreach (var ctrlTxt in ctrl)
            {
                if (ctrlTxt is CKM_TextBox)
                {
                    ((CKM_TextBox)ctrlTxt).isClosing = false;
                }
                //ctrlTxt.isClosing = true;
            }
        }
    }
}
