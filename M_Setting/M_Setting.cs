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
using BL;
using Entity;
using Search;

namespace M_Setting
{
    public partial class M_Setting : FrmMainForm
    {
        private const string ProID = "M_Setting";
        private const string ProNm = "権限設定";
        private Menu_BL mbl;
        public M_Setting()
        {
            InitializeComponent();
        }

        private void M_Setting_Load(object sender, EventArgs e)
        {
            mbl = new Menu_BL();
            InProgramID = ProID;
            InProgramNM = ProNm;
            this.SetFunctionLabel(EProMode.INPUT);
            base.StartProgram();
            Btn_F2.Text = Btn_F4.Text = Btn_F5.Text=  Btn_F7.Text = Btn_F8.Text = Btn_F9.Text = Btn_F10.Text = "";
            string[] cmds = System.Environment.GetCommandLineArgs();
            if (cmds.Length - 1 > (int)ECmdLine.PcID)
            {
                string juchuNO = cmds[(int)ECmdLine.PcID + 1];   //
                ChangeOperationMode(EOperationMode.UPDATE);
               // this.ModeText = "更新";
            }
            sc_Staff.Stype =CKM_SearchControl.SearchType.スタッフ;
            ChangeOperationMode(EOperationMode.UPDATE);
            sc_Staff.TxtCode.Focus();
        }
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            switch (Index)
            {
                case 0: // F1:終了
                    {
                        break;
                    }
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会
                    {
                      
                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;
                        
                      
                            ChangeOperationMode(EOperationMode.UPDATE);
                            sc_Staff.TxtCode.Focus();
                        
                        break;
                    }
                case 6://F7:行削除
                    break;

                case 7://F8:行追加
                    break;

                case 9://F10複写
                    break;
                case 8: //F9:検索
                    break;
                case 10: //F9:検索
                    F11();
                    break;
                case 11:    //F12:登録
                    {
                        if (OperationMode == EOperationMode.UPDATE)
                        { 
                            //Ｑ１０１		
                            if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                                return;
                        }
                        this.ExecSec();
                        break;
                    }
            }
        }
        protected override void ExecSec()
        {
            //colAll AdminKBN
            if ( !IsCheckedOne("AdminKBN"))
            {
                MessageBox.Show("Please check at least one operator as Admin");
                return;
            }
            if (!IsCheckedOne("colAll"))
            {
                MessageBox.Show("Please choose at least one operator to update");
                return;
            }
            var dt = DTres();
            string xml = bbl.DataTableToXml(dt);
            var res = mbl.SettingPermissionUpdate(InOperatorCD, bbl.GetDate(), xml);
            if (res)
            {
                bbl.ShowMessage("I101");
                ChangeOperationMode(EOperationMode.UPDATE);
            }
        }
        private DataTable DTres()
        {
           var dt= dgvsetting.DataSource as DataTable;
            return dt;
        }
        private bool IsCheckedOne(string col)
        {
            foreach (DataGridViewRow dr in dgvsetting.Rows)
            {
                if (((dr.Cells[col] as DataGridViewCheckBoxCell).Value is true))
                {
                    return true;
                }
            }
            return false;
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)
            
            switch (mode)
            {
                case EOperationMode.INSERT:
                    
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                   // EnablePanel(PanelHeader);
                    if (mode == EOperationMode.UPDATE)
                    {
                        this.ModeText = "修正";
                        F12Enable = false;
                        sc_Staff.TxtCode.Text = "";
                        sc_Staff.LabelText = "";
                        ckM_RadioButton1.Checked = true;
                      chk_adm.Checked= ckM_CheckBox1.Checked= ckM_CheckBox2.Checked=  ckM_CheckBox3.Checked = ckM_CheckBox4.Checked = ckM_CheckBox5.Checked = ckM_CheckBox6.Checked = false;
                        dgvsetting.DataSource = null;
                     F11Enable =  PanelHeader.Enabled = PanelSearch.Enabled = true;
                    }
                    //else
                    //    F12Enable = true;
                    break;

            }


        }
        protected override void EndSec()
        {
            this.Close();
        }

        private void BT_Display_Click(object sender, EventArgs e)
        {
            F11();
        }
        private void F11()
        {
           
            var dtres = mbl.SettingGetAllPermission(sc_Staff.TxtCode.Text , chk_adm.Checked ? "1" : null , ckM_CheckBox1.Checked ? "1":null,  ckM_CheckBox2.Checked ? "1" : null );
            dtres.Columns.Add("AdminKBN", typeof(bool));
            dtres.Columns.Add("SettingKBN", typeof(bool));
            dtres.Columns.Add("DefaultKBN", typeof(bool));
            dtres.Columns.Add("colAll", typeof(bool));
            foreach (DataRow dr in dtres.Rows)
            {
                dr["AdminKBN"]  = dr["Admin"].ToString() == "True" ? true : false;
                dr["SettingKBN"] = dr["Setting"].ToString() == "True" ? true : false;
                dr["DefaultKBN"] = dr["Default"].ToString() == "True" ? true : false;
                dr["colAll"] = false;
            }
            if (dtres.Rows.Count > 0)
            {
                dgvsetting.DataSource = dtres; F12Enable = true;
              F11Enable=  PanelHeader.Enabled= PanelSearch.Enabled = false;
                
            }
            else
                bbl.ShowMessage("E101");
        }

        private void adminfl_Click(object sender, EventArgs e)
        {
            if ((sender as CKM_Controls.CKM_CheckBox).Checked)
            {
                Checkall("AdminKBN", true);
               // Checkall("SettingKBN", true);
                ckM_CheckBox4.Checked = true;
            }
            else
                Checkall("AdminKBN", false);
        }
        private void Checkall(string col, bool flg)
        {
            foreach (DataGridViewRow dr in dgvsetting.Rows)
            {
                (dr.Cells[col] as DataGridViewCheckBoxCell).Value = flg ? 1 : 0; 
            }
        }
        private void settingfl_Click(object sender, EventArgs e)
        {
            if ((sender as CKM_Controls.CKM_CheckBox).Checked)
                Checkall("SettingKBN", true);
            else
                Checkall("SettingKBN", false);
        }

        private void Defaultfl_Click(object sender, EventArgs e)
        {
            if ((sender as CKM_Controls.CKM_CheckBox).Checked)
                Checkall("DefaultKBN", true);
            else
                Checkall("DefaultKBN", false);
        }

        private void dgvsetting_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            var ed = e;
        }

        private void sc_Staff_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(sc_Staff.TxtCode.Text))
            //{
            //    //Ｅ１０２
            //    bbl.ShowMessage("E102");
            //    sc_Staff.TxtCode.Focus();
            //    return ;
            //}
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = sc_Staff.TxtCode.Text,
                ChangeDate = bbl.GetDate()
            };
            Staff_BL bl = new Staff_BL();
            bool staff = bl.M_Staff_Select(mse);
            if (staff)
            {
                (sc_Staff).LabelText = mse.StaffName;
            }
           
        }

        private void M_Setting_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void ckM_CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CKM_Controls.CKM_CheckBox).Checked)
                Checkall("colAll", true);
            else
                Checkall("colAll", false);
        }

        private void dgvsetting_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >=0 && e.ColumnIndex == 1)
            {
                if (dgvsetting.CurrentCell is DataGridViewCheckBoxCell)
                {
                    if ((dgvsetting.CurrentCell as DataGridViewCheckBoxCell).EditedFormattedValue is true)
                    {
                      (  dgvsetting.Rows[e.RowIndex].Cells[e.ColumnIndex + 1] as DataGridViewCheckBoxCell).Value = true;
                    }
                }


            }
            
        }

        private void ckM_RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = false;
        }

        private void ckM_RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            if (ckM_RadioButton2.Checked)
            chk_adm.Checked = ckM_CheckBox1.Checked = ckM_CheckBox2.Checked = true;
            else
                chk_adm.Checked = ckM_CheckBox1.Checked = ckM_CheckBox2.Checked = false;
        }
    }
}
