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
    public partial class Search_InstructionNO : FrmSubForm
    {
        private const string ProNm = "出荷指示番号検索";

        private enum EIndex : int
        {
            SoukoCD,
            DayStart,
            DayEnd,
            PlanDateStart,
            PlanDateEnd,
            Chk1,
            Chk2,
            Chk3,
            Chk4,
            Chk5,
            DeliveryName,
            CarrierCD,
            JuchuuNO,
            StaffCD,
            COUNT
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product
        }

        public string OperatorCD = string.Empty;
        public string InstructionNO = string.Empty;
        public string ChangeDate = string.Empty;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private Control[] detailControls;
        D_Instruction_Entity die;
        ShukkaShijiTouroku_BL snbl;

        public Search_InstructionNO(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;

            CboSouko.Bind(changeDate);

            snbl = new ShukkaShijiTouroku_BL();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { CboSouko,ckM_TextBox1, ckM_TextBox2, ckM_TextBox3, ckM_TextBox4
               ,ckM_CheckBox1,ckM_CheckBox2,ckM_CheckBox3,ckM_CheckBox4,ckM_CheckBox5
               , ckM_TextBox5, CboCarrier,  ScJuchuuNO.TxtCode ,ScStaff.TxtCode};

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

        private D_Instruction_Entity GetSearchInfo()
        {
            die = new D_Instruction_Entity
            {
                InstructionDateFrom = detailControls[(int)EIndex.DayStart].Text,
                InstructionDateTo = detailControls[(int)EIndex.DayEnd].Text,
                DeliveryPlanDateFrom = detailControls[(int)EIndex.PlanDateStart].Text,
                DeliveryPlanDateTo = detailControls[(int)EIndex.PlanDateEnd].Text,
                DeliveryName = detailControls[(int)EIndex.DeliveryName].Text,
                PrintStaffCD =ScStaff.TxtCode.Text,
                JuchuuNO = ScJuchuuNO.TxtCode.Text,
                DeliverySoukoCD = CboSouko.SelectedIndex>0 ? CboSouko.SelectedValue.ToString() : string.Empty,
                CarrierCD = CboCarrier.SelectedIndex > 0 ? CboCarrier.SelectedValue.ToString() : string.Empty,
            };
            if (ckM_CheckBox1.Checked)
                die.Chk1 = 1;
            else
                die.Chk1 = 0;

            if (ckM_CheckBox2.Checked)
                die.Chk2 = 1;
            else
                die.Chk2 = 0;

            if (ckM_CheckBox3.Checked)
                die.Chk3 = 1;
            else
                die.Chk3 = 0;

            if (ckM_CheckBox4.Checked)
                die.Chk4 = 1;
            else
                die.Chk4 = 0;

            if (ckM_CheckBox5.Checked)
                die.Chk5 = 1;
            else
                die.Chk5 = 0;

            return die;
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
                if (i != (int)EIndex.SoukoCD &&  !string.IsNullOrWhiteSpace(detailControls[i].Text))
                    exists = true;

                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            }

            if(!exists)
            {
                snbl.ShowMessage("E111");
                detailControls[0].Focus();
                return;
            }

            die = GetSearchInfo();
            DataTable dt = snbl.D_Instruction_SelectAll(die);
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
               snbl.ShowMessage("E128");
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
                case (int)EIndex.PlanDateStart:
                case (int)EIndex.PlanDateEnd:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = snbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!snbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        snbl.ShowMessage("E103");
                        return false;
                    }
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.DayEnd || index == (int)EIndex.PlanDateEnd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                snbl.ShowMessage("E104");
                                return false;
                            }
                        }
                    }

                    break;

                case (int)EIndex.SoukoCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;

                case (int)EIndex.StaffCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScStaff.LabelText = "";
                        return true;
                    }

                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = snbl.GetDate() // detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Staff_BL bl = new Staff_BL();
                    bool ret = bl.M_Staff_Select(mse);
                    if (ret)
                    {
                        ScStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        snbl.ShowMessage("E101");
                        ScStaff.LabelText = "";
                        return false;
                    }
                    break;

                case (int)EIndex.JuchuuNO:
                    //入力された場合
                    if (!string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //受注(D_Juchuu)の存在しない場合エラー
                        D_Juchuu_Entity de = new D_Juchuu_Entity();
                        de.JuchuuNO = detailControls[index].Text;
                        TempoJuchuuNyuuryoku_BL tbl = new TempoJuchuuNyuuryoku_BL();
                        DataTable dt = tbl.D_Juchu_SelectData(de, 2);
                        if(dt.Rows.Count == 0)
                        {
                            bbl.ShowMessage("E138", "受注番号");
                            return false;
                        }
                        else
                        {
                            //DeleteDateTime 「削除された受注番号」
                            if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                            {
                                bbl.ShowMessage("E140", "受注番号");
                                return false;
                            }
                        }
                    }
                        break;
            }

            return true;
        }

        private void GetData()
        {
            if(GvDetail.CurrentRow != null &&  GvDetail.CurrentRow.Index >= 0)
            { 
                InstructionNO = GvDetail.CurrentRow.Cells["colInstructionNO"].Value.ToString();
                //ChangeDate = GvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();
            }
        }

        /// <summary>
        /// 画面クリア
        /// </summary>
        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = true;
                }
                else
                {
                    ctl.Text = "";
                }
            }


        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                Scr_Clr();

                string ymd = snbl.GetDate();
                snbl = new ShukkaShijiTouroku_BL();
                CboSouko.Bind(ymd);
                CboCarrier.Bind(ymd);

                ScJuchuuNO.Value1 = OperatorCD;
                ScJuchuuNO.Value2 = AllAvailableStores;

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = OperatorCD,
                    ChangeDate = snbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    //[M_Souko_Search]
                    M_Souko_Entity me = new M_Souko_Entity
                    {
                        StoreCD = mse.StoreCD,
                        SoukoType = "3",
                        ChangeDate = ymd,
                        DeleteFlg = "0",
                        searchType = "2"
                    };

                    DataTable mdt = snbl.M_Souko_Search(me);
                    if (mdt.Rows.Count > 0)
                    {
                        CboSouko.SelectedValue = mdt.Rows[0]["SoukoCD"];
                    }
                }

                for (int i = (int)EIndex.DayStart; i <= (int)EIndex.DayEnd; i++)
                {
                    detailControls[i].Text = ymd;
                }
                detailControls[(int)EIndex.StaffCD].Text = OperatorCD;
                ScStaff.LabelText = mse.StaffName;

                detailControls[0].Focus();
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
