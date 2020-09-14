using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;

namespace TenzikaiJuchuuJouhouHurikaeShori
{
    /// <summary>
    /// TenzikaiJuchuuJouhouHurikaeShori 展示会受注情報振替処理
    /// </summary>
    internal partial class TenzikaiJuchuuJouhouHurikaeShori : FrmMainForm
    {
        private const string ProID = "TenzikaiJuchuuJouhouHurikaeShori";
        private const string ProNm = "展示会受注情報振替処理";

        private enum EIndex : int
        {
            RdoCreate,
            RdoDelete,
            TorokuDT,
            VendorCD,
            Nendo,
            Season,
            Count
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private TenzikaiJuchuuJouhouHurikaeShori_BL ssbl;

        public TenzikaiJuchuuJouhouHurikaeShori()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.BATCH);
                this.InitialControlArray();

                //起動時共通処理
                base.StartProgram();

                ssbl = new TenzikaiJuchuuJouhouHurikaeShori_BL();
                string ymd = bbl.GetDate();
                cboNendo.Bind(ymd);
                cboSeason.Bind(ymd);

                //基準日：Today 仕入先区分：1
                ScVendor.Value1 = "1";
                ScVendor.ChangeDate = ymd;

                SetFuncKeyAll(this, "100001000001");
                Scr_Clr(0);

                detailControls[(int)EIndex.RdoCreate].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        protected override void ExecSec()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            if (bbl.ShowMessage("Q101") != DialogResult.Yes)
            {
                return;
            }

            M_TenzikaiShouhin_Entity me = new M_TenzikaiShouhin_Entity
            {
                ProcessMode = RdoCreate.Checked ? "1":"3",
                VendorCD = detailControls[(int)EIndex.VendorCD].Text,
                ChangeDate = detailControls[(int)EIndex.TorokuDT].Text,
                LastSeason = cboSeason.SelectedIndex > 0 ? cboSeason.SelectedValue.ToString(): null,
                LastYearTerm = cboNendo.SelectedIndex > 0 ? cboNendo.SelectedValue.ToString() : null,
                Operator = InOperatorCD,
                PC = InPcID
            };

            GvDetail.DataSource = null;

            if (RdoDelete.Checked)
            {
                string errNo = "";
                bool ret = ssbl.CheckTenzikaiJuchuu(me, out errNo);

                if(!string.IsNullOrWhiteSpace( errNo))
                {
                    bbl.ShowMessage(errNo);
                    return;
                }
            }

            //更新処理
            ssbl.D_TenzikaiJuchuu_Exec(me);

            //SKUマスタ未登録の展示会商品が含まれている展示会受注データが存在していた場合、明細部に展示会受注番号を表示
            if (RdoCreate.Checked)
            {
                DataTable dt = ssbl.D_TenzikaiJuchuu_SelectAll(me);
                GvDetail.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GvDetail.CurrentRow.Selected = true;
                    GvDetail.Enabled = true;
                    //GvDetail.Focus();
                }
                else
                {
                    //ssbl.ShowMessage("E128");
                }
            }
            if (RdoDelete.Checked)
            {
                bbl.ShowMessage("I102");
                RdoDelete.Focus();
            }
            else
            {
                bbl.ShowMessage("I101");
                RdoCreate.Focus();
            }
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { RdoCreate, RdoDelete, ckM_TextBox1, ScVendor.TxtCode, cboNendo, cboSeason };
            detailLabels = new Control[] { ScVendor };
            searchButtons = new Control[] { ScVendor.BtnSearch };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }

        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set = true)
        {
            bool ret;
            switch (index)
            {
                case (int)EIndex.TorokuDT:
                    if (detailControls[index].Enabled)
                        {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }

                        detailControls[index].Text = ssbl.FormatDate(detailControls[index].Text);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!ssbl.CheckDate(detailControls[index].Text))
                        {
                            //Ｅ１０３
                            ssbl.ShowMessage("E103");
                            return false;
                        }
                        //入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.VendorCD:
                   
                    if (RdoCreate.Checked)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //顧客情報ALLクリア
                        ClearInfo();
                        return true;
                    }
                    string ymd = bbl.GetDate();

                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        ChangeDate = ymd,
                        VendorFlg="1"
                    };

                    Vendor_BL vbl = new Vendor_BL();
                    ret = vbl.M_Vendor_SelectTop1(mve);
                    if (ret)
                    {
                            ScVendor.LabelText = mve.VendorName;                     
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearInfo();
                        return false;
                    }
                    
                    break;
                case (int)EIndex.Nendo:
                case (int)EIndex.Season:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        //CboStoreCD.MoveNext = false;
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(Panel)) || ctl.GetType().Equals(typeof(CKM_Controls.CKM_RadioButton)))
                {
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
                }
                else
                {
                    ctl.Text = "";
                }
            }

            foreach (Control ctl in detailLabels)
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }

            RdoCreate.Checked = true;
            detailControls[(int)EIndex.TorokuDT].Enabled = false;

            detailControls[(int)EIndex.RdoCreate].Focus();
        }

        /// <summary>
        /// 仕入先情報クリア処理
        /// </summary>
        private void ClearInfo()
        {
            ScVendor.LabelText = "";
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

                        Scr_Clr(0);

                        break;
                    }

                case 11:
                    break;

            }   //switch end

        }

        // ==================================================
        // 終了処理
        // ==================================================
        protected override void EndSec()
        {
            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
        }

        #region "内部イベント"

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
                    int index = Array.IndexOf(detailControls, sender);
                    bool ret = CheckDetail(index);
                    if (ret)
                    {
                        if(index.Equals((int)EIndex.RdoCreate) )
                        {
                            detailControls[(int)EIndex.VendorCD].Focus();
                        }
                        else if (index.Equals((int)EIndex.RdoDelete))
                        {
                            detailControls[(int)EIndex.TorokuDT].Focus();
                        }
                        else if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                        }
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
                PreviousCtrl = this.ActiveControl;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void RdoCreate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                detailControls[(int)EIndex.TorokuDT].Enabled = !RdoCreate.Checked;

                if (!detailControls[(int)EIndex.TorokuDT].Enabled)
                    detailControls[(int)EIndex.TorokuDT].Text = "";
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}








