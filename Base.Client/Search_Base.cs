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
    }
}
