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
    public partial class Search_Base : Form
    {
        public const short FuncEnd = 1;
        public Control PreviousCtrl { get; set; }
        protected Base_BL bbl = new Base_BL();
        protected string HeaderTitleText
        {
            set
            {
                this.lblSearch_Name.Text = value;
            }
        }

        public bool flgCancel = false;

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("FormName")]
        [DisplayName("ProgramName")]
        public string ProgramName { get => lblSearch_Name.Text; set => lblSearch_Name.Text = value; }
        public Search_Base()
        {
            InitializeComponent();
            //PutCursor();
        }

        protected virtual void EndSec()
        {
            
        }


        private void Btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (!string.IsNullOrWhiteSpace(btn.Text))
                    ButtonFunction(btn.Tag.ToString());
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
                //if (bbl.ShowMessage("Q003") == DialogResult.Yes)
                flgCancel = true;
                    EndSec();
                //else
                //    PreviousCtrl.Focus();
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
     
        public virtual void FunctionProcess(int Index)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

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
                    if (!CheckHyoujiError(ActiveControl))
                        return;
                    if (this.Parent != null)
                        this.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                    else
                        this.SelectNextControl(ActiveControl, true, true, true, true);
                }
            }
            
        }

        private bool CheckHyoujiError(Control ctr)
        {
            //
            if (ctr.Text.Contains("表示"))
            {
                var ctrl = GetAllControls(this);
                foreach (var ctrlTxt in ctrl)
                {
                    if (ctrlTxt is CKM_GridView cg)
                    {
                        if (cg.DataSource == null)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
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
    }
}
