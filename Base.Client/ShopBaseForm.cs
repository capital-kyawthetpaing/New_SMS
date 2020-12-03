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

namespace Base.Client
{
    public partial class ShopBaseForm : Form
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
        #endregion

        /// <summary>
        ///     ''' 終了ファンクション
        ///     ''' </summary>
        ///     ''' <remarks>F1を終了とする</remarks>
        public const short FuncEnd = 1;

        protected Base_BL bbl = new Base_BL();
        private Login_BL loginbl = new Login_BL();
        private L_Log_Entity lle;
        protected M_AuthorizationsDetails_Entity made;
        private M_Program_Entity mpe;
        private string[] availableStores;
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
        protected string StoreCD { get; set; }
        protected string ChangeDate { get; set; }
        public Control PreviousCtrl { get; set; }

        protected bool ShowCloseMessage = true;

        /// <summary>
        /// 店舗レジで使用するプリンター名
        /// </summary>
        protected string StorePrinterName { get { return loginbl.StorePrinterName; } }

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

        [Browsable(true)]
        [Category("CKM_Shop Properties")]
        [Description("Type the text of ProcessButton")]
        [DisplayName("btnProcess_Text")]
        public string BtnP_text
        {
            get { return btnProcess.Text; }
            set { btnProcess.Text = value; }
        }
    
        

        public ShopBaseForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            //this.MaximizeBox = false;
        }

        /// <summary>
        /// EXEではなくFormとして起動した場合に必要な処理
        /// </summary>
        public void StartProgramForForm()
        {
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD
            };

            mse = loginbl.M_Staff_InitSelect(mse);

            this.lblOperatorName.Text = mse.StaffName;

            M_Store_Entity mste = new M_Store_Entity
            {
                SysDate = mse.SysDate
            };
            mste = loginbl.M_Store_InitSelect(mse, mste);
            this.lblStoreName.Text = mste.StoreName;
           
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

            if (this.GetCmdLine() == false || loginbl.ReadConfig() == false)
            {
                //起動時エラー    DB接続不可能
                this.Close();
                System.Environment.Exit(0);
            }
            

            //共通処理　Operator 確認
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD
            };
            mse = loginbl.M_Staff_InitSelect(mse);

           // MessageBox.Show(bbl.GetConnectionString());

            this.lblOperatorName.Text = mse.StaffName;
            //this.lblLoginDate.Text = mse.SysDate;
            DataTable dtShopName =  bbl.SimpleSelect1("42", null, "222", "1");
            this.lblShopName.Text = dtShopName.Rows[0]["Char1"].ToString();

            M_Store_Entity mste = new M_Store_Entity
            {
                SysDate = mse.SysDate
            };
            mste=loginbl.M_Store_InitSelect(mse,mste);
            this.lblStoreName.Text = mste.StoreName;
            StoreCD = mste.StoreCD;
            ChangeDate = mste.SysDate;

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
                ProgramID = this.InProgramID,
                StaffCD = InOperatorCD,
                PC = InPcID
            };
           // bbl.ShowMessage(mae.MessageID + "B_" + mae.ProgramID + "_" + mae.StaffCD + "_" + mae.PC);
            made = bbl.M_Authorizations_AccessCheck(mae);
            
            if (made == null)
            {
                
                bbl.ShowMessage(mae.MessageID + "M_" + mae.ProgramID + "_" + mae.StaffCD + "_"+ mae.PC);
                //起動時エラー
                this.Close();
                System.Environment.Exit(0);
            }

            //KTP 2019-05-29 Set ProgrameName, ProgramID
            this.Text = made.ProgramID;
            //lblHeaderTitle.Text = made.ProgramName;

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
            M_StoreAuthorizations_Entity msa = new M_StoreAuthorizations_Entity();
            msa.StoreAuthorizationsCD = made.StoreAuthorizationsCD;

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
            ///Added by ETZ ,To close when StoreAuthorization isn't accessed
            else     
            {
                bbl.ShowMessage(mae.MessageID + "M_Sto");
                //起動時エラー
                this.Close();
                System.Environment.Exit(0);
            }

            //プログラム起動履歴
            //InsertLog(Get_L_Log_Entity(true));

            //KTP 2019-05-29 Program Type Select from M_Authorizations_AccessCheck function
            //Todo:入力プログラム以外を考慮
            //switch (mpe.Type)
            //switch (made.ProgramType)
            //{
            //    case "1":
            //        if (made.Insertable == "1")
            //        {
            //            // 新規ボタン押下処理
            //            FunctionProcess((int)EOperationMode.INSERT);
            //        }
            //        else if (made.Updatable == "1")
            //        {
            //            // 修正ボタン押下処理
            //            FunctionProcess((int)EOperationMode.UPDATE);
            //        }
            //        else if (made.Deletable == "1")
            //        {
            //            // 削除ボタン押下処理
            //            FunctionProcess((int)EOperationMode.DELETE);
            //        }
            //        else
            //        {
            //            // 照会ボタン押下処理
            //            FunctionProcess((int)EOperationMode.SHOW);
            //        }
            //        break;
            //}
        }

        private bool GetCmdLine()
        {
            ////コマンドライン引数を配列で取得する
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

        private void Btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (!string.IsNullOrWhiteSpace(btn.Text))
                {
                    //if(ShowCloseMessage)
                        ButtonFunction(btn.Tag.ToString());
                    //else
                    //    EndSec();
                }
                  
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void ButtonFunction(string buttonTag)
        {
            short Index = Convert.ToInt16(buttonTag);

            // 終了ファンクション
            if (Index + 1 == FuncEnd)
            {
                if(!ShowCloseMessage)
                    EndSec();
                else if (bbl.ShowMessage("Q003") == DialogResult.Yes)
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
                    EndSec();
                }
                else
                {
                    if (PreviousCtrl != null)
                        PreviousCtrl.Focus();
                    else
                        btnClose.Focus();
                }
                return;
            }
            //else if (Index < 5)
            //{
            //    //処理モード変更時  Todo:入力プログラム以外を考慮
            //    if (bbl.ShowMessage("Q101") != DialogResult.Yes)
            //    {
            //        PreviousCtrl.Focus();
            //        return;
            //    }
            //}
            FunctionProcess(Index);
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

        public virtual void FunctionProcess(int Index)
        {

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
                    if (((CKM_ComboBox)c).SelectedValue.Equals("-1"))
                    {
                        bbl.ShowMessage("E102");
                        c.Focus();
                        return false;
                    }
                }
                else if (c is CKMShop_ComboBox)
                {
                    if (((CKMShop_ComboBox)c).SelectedValue.Equals("-1"))
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

        private void btnClose_MouseEnter(object sender, EventArgs e)
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
                else if (ActiveControl is CKMShop_ComboBox)
                {
                    if ((ActiveControl as CKMShop_ComboBox).MoveNext)
                    {
                        if (this.Parent != null)
                            this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        else
                        {
                            (ActiveControl as CKMShop_ComboBox).MoveNext = false;
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
                else if (ActiveControl is CKMShop_CheckBox csc || (ActiveControl is CKMShop_RadioButton csr))
                {


                    this.SelectNextControl(ActiveControl, true, true, true, true);
                }
                else
                {

                    if (this.Parent != null)
                    {
                        this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                    }
                    else if (ActiveControl is CKM_MultiLineTextBox)
                    {
                        var con = (ActiveControl as CKM_MultiLineTextBox);
                        var g = con.Cursorout.Replace("\r", "").Replace("\n", "");
                        var f = con.Cursorin.Replace("\r", "").Replace("\n", "");
                        if (e.Control && e.KeyCode == Keys.Enter)
                        {
                            if (this.Parent != null)
                                this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        }
                        else if (f == g)
                        {
                            if (!con.Mdea)
                            {
                                this.SelectNextControl(ActiveControl, true, true, true, true);
                            }
                            else
                                con.Mdea = false;
                        }

                    }
                    else
                    {
                        this.SelectNextControl(ActiveControl, true, true, true, true);
                    }
                }
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

        private void btnClose_MouseLeave(object sender, EventArgs e)
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