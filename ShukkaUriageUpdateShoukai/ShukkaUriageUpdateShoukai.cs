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

namespace ShukkaUriageUpdateShoukai
{
    /// <summary>
    /// ShukkaUriageUpdateShoukai 出荷売上データ更新照会
    /// </summary>
    internal partial class ShukkaUriageUpdateShoukai : FrmMainForm
    {
        private const string ProID = "ShukkaUriageUpdateShoukai";
        private const string ProNm = "出荷売上データ更新照会";
        private const string BatchProID = "ShukkaUriageUpdateShoukaiB.exe";
        private enum EIndex : int
        {
            Time       
        }

        private Control[] detailControls;    
        private ShukkaUriageUpdate_BL sbl;    
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避 

        public ShukkaUriageUpdateShoukai()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.SHOW);   //照会プログラムとして起動
                this.InitialControlArray();

                Btn_F6.Text = "";
                Btn_F9.Text = "";
                Btn_F12.Text = "登録(F12)";

                //起動時共通処理
                base.StartProgram();
                
                sbl = new ShukkaUriageUpdate_BL();

                //【受信履歴】
                //画面転送表01に従って、更新履歴情報を表示
                ExecDisp();

                detailControls[0].Focus(); 
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1};

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
        
        /// <summary>
        /// コードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDetail(int index)
        {
            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
            if (string.IsNullOrWhiteSpace(detailControls[index].Text))
            {
                //Ｅ１０２
                bbl.ShowMessage("E102");
                return false;
            }
            detailControls[index].Text = FormatTime(detailControls[index].Text);

            //時刻として正しい
            if (!CheckTime(detailControls[index].Text))
            {
                //Ｅ２１５
                bbl.ShowMessage("E215");
                return false;
            }

            return true;

        }
        private string FormatTime(string time)
        {
            string formatStr = "";

            formatStr =bbl.Z_Set( time.Replace(":", "")).ToString("00:00");

            return formatStr;
        }
        private bool CheckTime(string time)
        {
            string strTime = bbl.GetDate() + " " + time + ":00";
            string format = "yyyy/MM/dd HH:mm:ss";
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            System.Globalization.DateTimeStyles dts = System.Globalization.DateTimeStyles.None;
            DateTime dateTime;

            if (DateTime.TryParseExact(strTime, format, ci, dts, out dateTime))
            {
                return true;
            }

            return false;
        }
        protected override void ExecDisp()
        {
            try
            {
                //【取込履歴】
                M_MultiPorpose_Entity me = new M_MultiPorpose_Entity
                {
                    ID = MultiPorpose_BL.ID_ShukkaUriageUpdate,
                    Key = "1"
                };

                MultiPorpose_BL mbl = new MultiPorpose_BL();
                DataTable dt = mbl.M_MultiPorpose_Select(me);
                if (dt.Rows.Count > 0)
                {
                    string time = Convert.ToInt16(dt.Rows[0]["Num1"]).ToString("D4");
                    ckM_TextBox1.Text = time.Substring(0, 2) + ":" + time.Substring(2, 2);
                }
                else
                {
                    ////Ｅ１０１
                    //bbl.ShowMessage("E101");
                    //EndSec();
                    ckM_TextBox1.Text = "";
                }

                Scr_Clr(0);

                //更新履歴情報データ取得処理
                dt = sbl.D_Shipping_SelectAllForShoukai();

                GvDetail.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    GvDetail.CurrentRow.Selected = true;
                    GvDetail.Enabled = true;

                    GvDetail.Focus();
                }
                else
                {
                    //bbl.ShowMessage("E128");
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        protected override void ExecSec()
        {
            try
            {
                bool ret = CheckDetail(0);

                if (!ret)
                {
                    return;
                }

                //テーブル転送仕様Ａに従って、汎用マスタ情報を更新（データが存在しなければInsert、データが存在する場合はUpdate）
                M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity();
                mme.ID = MultiPorpose_BL.ID_ShukkaUriageUpdate;
                mme.Key = "1";
                mme.Num1 = ckM_TextBox1.Text.Replace(":", "");
                mme.UpdateOperator = InOperatorCD;
                mme.PC = InPcID;

                sbl.M_MultiPorpose_Update(mme);

                //更新後画面そのまま
                detailControls[0].Focus();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
    
        }
        
        private void CheckProcess()
        {
            System.Diagnostics.Process[] ps =
             System.Diagnostics.Process.GetProcessesByName(BatchProID);

            if (ps.Length > 0)
            {
                //MessageBox.Show(strProcessName + "を検出しました。");
            }
            else
            {
                //BatchProIDを起動
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + BatchProID;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID;
                    System.Diagnostics.Process.Start(filePath, cmdLine);
                }
                else
                {
                    //ファイルなし
                }
            }

        }
        private void  UpdateM_MultiPorpose()
        {
            M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity();
            mme.ID = MultiPorpose_BL.ID_ShukkaUriageUpdate;
            mme.Key = "1";
            mme.UpdateOperator = InOperatorCD;
            mme.PC = InPcID;

            //if (mEdiMode == "1")
            //{
            //    //　記憶していた初期表示値が１
            //    //＆画面・在庫SKS連携処理の処理モードが「処理停止中」の場合
            //    if(lblEdiMode.Text== "処理停止中")
            //    {
            //        mme.Num1 = "0";
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    //　記憶していた初期表示値が０
            //    //＆画面・在庫SKS連携処理の処理モードが「処理実行中」の場合
            //    if (lblEdiMode.Text == "処理実行中")
            //    {
            //        mme.Num1 = "1";
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            //sbl.M_MultiPorpose_Update(mme);
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {

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
                case 0:     // F1:終了
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
                case 9://F10:印刷
                    ExecDisp();

                    this.Cursor = Cursors.WaitCursor;
                    this.PrintMode = EPrintMode.DIRECT;
                    PrintSec();
                    this.Cursor = Cursors.Default;
                    break;

                case 11://F12:最新化
                    ExecSec();
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
                           BtnSubF11.Focus();
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
        private void BtnSubF11_Click(object sender, EventArgs e)
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

        #endregion

    }
}








