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
    public partial class Search_PickingNO : FrmSubForm
    {
        private const string ProNm = "ピッキング番号検索";

        private enum EIndex : int
        {
            DayStart,
            DayEnd,
            SoukoCD,
            COUNT
        }

        public string OperatorCD = string.Empty;
        public string PickingNO = string.Empty;
        public string ChangeDate = string.Empty;
        public string SoukoCD = string.Empty;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private Control[] detailControls;
        D_Picking_Entity dpe;
        PickingNyuuryoku_BL pnbl;

        public Search_PickingNO(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;
            ChangeDate = changeDate;

            pnbl = new PickingNyuuryoku_BL();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2,CboSoukoCD};

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

        private D_Picking_Entity GetSearchInfo()
        {
            dpe = new D_Picking_Entity
            {
                PrintDateTimeFrom = detailControls[(int)EIndex.DayStart].Text,
                PrintDateTimeTo = detailControls[(int)EIndex.DayEnd].Text,
                SoukoCD = CboSoukoCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboSoukoCD.SelectedValue.ToString(),
            };

            return dpe;
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
            bool exists = false;
            for (int i = 0; i < detailControls.Length; i++)
            {
                ///2121/06/18 change for task 2837
                //if (i != (int)EIndex.SoukoCD &&  !string.IsNullOrWhiteSpace(detailControls[i].Text))
                //    exists = true;

                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            }

            //if(!exists)   ///2121/06/18 change for task 2837
            //{
            //    pnbl.ShowMessage("E111");
            //    detailControls[0].Focus();
            //    return;
            //}

            dpe = GetSearchInfo();
            DataTable dt = pnbl.D_Picking_SelectAll(dpe);
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
               pnbl.ShowMessage("E128");
                GvDetail.DataSource = null;
                detailControls[0].Focus();
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
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = pnbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!pnbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        pnbl.ShowMessage("E103");
                        return false;
                    }
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.DayEnd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                pnbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;

                case (int)EIndex.SoukoCD:
                    //選択必須(Entry required)
                    if (!RequireComboCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;

               
            }

            return true;
        }
        private bool RequireComboCheck(Control[] ctrl)
        {
            foreach (Control c in ctrl)
            {
                if (c is ComboBox)
                {
                    if (((ComboBox)c).SelectedIndex.Equals(-1))
                    {
                        pnbl.ShowMessage("E102");
                        c.Focus();
                        return false;
                    }
                    if (((ComboBox)c).SelectedValue.Equals("-1"))
                    {
                        pnbl.ShowMessage("E102");
                        c.Focus();
                        return false;
                    }
                }
            }
            return true;
        }
        private void GetData()
        {
            if(GvDetail.CurrentRow != null &&  GvDetail.CurrentRow.Index >= 0)
            { 
                PickingNO = GvDetail.CurrentRow.Cells["colPickingNO"].Value.ToString();
                //ChangeDate = GvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();
            }
        }

        /// <summary>
        /// 画面クリア
        /// </summary>
        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
                ctl.Text = "";


        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                Scr_Clr();
                detailControls[(int)EIndex.DayStart].Text = pnbl.GetDate();
                detailControls[(int)EIndex.DayEnd].Text = pnbl.GetDate();
                //CboSoukoCD.SelectedValue = SoukoCD;
                CboSoukoCD.Bind(ChangeDate, OperatorCD);
                if (CboSoukoCD.Items.Count>1)
                    CboSoukoCD.SelectedIndex = 1;

                F9Visible = false;
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
