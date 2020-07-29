using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;

namespace Search
{
    public partial class Search_Location : FrmSubForm
    {
        private const string ProNm = "棚検索";

        private enum EIndex : int
        {
            SoukoCD,
            COUNT
        }
         
        public string OperatorCD = string.Empty;
        public string TanaCD = string.Empty;
        public string SoukoCD = string.Empty;
        public string ChangeDate = string.Empty;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private Control[] detailControls;
        M_Location_Entity mle;
        ZaikoIdouNyuuryoku_BL zibl;

        public Search_Location(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;            

            zibl = new ZaikoIdouNyuuryoku_BL();
        }

        public Search_Location(string changeDate,string soukoCD)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;

            CboFromSoukoCD.SelectedValue =SoukoCD= soukoCD;
            zibl = new ZaikoIdouNyuuryoku_BL();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] {CboFromSoukoCD };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
        private void BtnF11_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
            try
            {
                base.FunctionProcess(FuncDisp - 1);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private M_Location_Entity GetSearchInfo()
        {
            mle = new M_Location_Entity
            {
                ChangeDate = ckM_Label1.Text,
                SoukoCD = CboFromSoukoCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboFromSoukoCD.SelectedValue.ToString(),
            };
            return mle;
        }

        private void DgvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    if (e.Control == false)
                    {
                        this.ExecSec();
                    }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void GvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.ExecSec();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            switch (Index)
            {
                case 0: // F1:終了
                case 1:     //F2:
                case 2:     //F3:
                case 3:     //F4:
                case 4:     //F5:
                case 5: //F6:
                case 6://F7:
                case 7://F8:
                case 11:    //F12:
                    {
                        break;
                    }

                case 8: //F9:検索
                   
                    break;

            }   //switch end

        }
        protected override void ExecSec()
        {
            GetData();
            EndSec();
        }

        protected override void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
            {
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            }

            mle = GetSearchInfo();
            DataTable dt = zibl.M_Location_SelectAll(mle);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count>0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
            }
            else
            {
               zibl.ShowMessage("E128");
                GvDetail.DataSource = null;
            }
        }
        private void DetailControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    bool ret = CheckDetail(Array.IndexOf(detailControls, sender));
                    if (ret)
                    {
                        if (detailControls.Length - 1 > Array.IndexOf(detailControls, sender))
                            detailControls[Array.IndexOf(detailControls, sender) + 1].Focus();

                        else
                            btnSubF11.Focus();
                    }
                    else
                    {
                        ((Control)sender).Focus();
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
        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private bool CheckDetail(int index)
        {
            switch (index)
            {
                case (int)EIndex.SoukoCD:

                    break;  

            }

            return true;
        }

        private void GetData()
        {
            if(GvDetail.CurrentRow != null &&  GvDetail.CurrentRow.Index >= 0)
            { 
                TanaCD = GvDetail.CurrentRow.Cells["colTanaCD"].Value.ToString();
            }
        }

        /// <summary>
        /// 画面クリア
        /// </summary>
        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
                ctl.Text = "";

            //初期値設定
            if (DateTime.TryParse(ChangeDate, out DateTime dt))
                ckM_Label1.Text = ChangeDate;
            else
                ckM_Label1.Text = zibl.GetDate();

            if (!string.IsNullOrWhiteSpace(SoukoCD))
            {
                CboFromSoukoCD.SelectedValue = SoukoCD;
                CboFromSoukoCD.Enabled = false;
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                //コンボボックス初期化
                string ymd = zibl.GetDate();
                CboFromSoukoCD.Bind(string.Empty,"");

                Scr_Clr();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
    }
}
