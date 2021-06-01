using System;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;

namespace NayoseSyoriAll
{
    /// <summary>
    /// NayoseSyoriAll 名寄せ処理(全顧客)
    /// </summary>
    internal partial class NayoseSyoriAll : FrmMainForm
    {
        private const string ProID = "NayoseSyoriAll";
        private const string ProNm = "名寄せ処理(全顧客)";
        private const short mc_L_END = 3; // ロック用

        private NayoseSyoriAll_BL nkbl;
        private D_Juchuu_Entity dje;
        private bool mAutoMode = false;

        public NayoseSyoriAll()
        {
            InitializeComponent();

        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.BATCH);

                //起動時共通処理
                base.StartProgram();

                ModeVisible = false;
                Btn_F2.Text = "";
                Btn_F3.Text = "";
                Btn_F3.Text = "";
                Btn_F4.Text = "";
                Btn_F5.Text = "";
                Btn_F6.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F9.Text = "";
                Btn_F10.Text = "";
                Btn_F11.Text = "";

                Btn_F12.Focus();

                nkbl = new NayoseSyoriAll_BL();

                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    string mode = cmds[(int)ECmdLine.PcID + 1];   //

                    if (mode.Equals("1"))
                    {
                        mAutoMode = true;
                        ExecSec();
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Juchuu_Entity GetEntity()
        {
            dje = new D_Juchuu_Entity
            {
                InsertOperator = InOperatorCD,
                PC = InPcID
            };

            return dje;
        }

        protected override void ExecSec()
        {
            if (!mAutoMode)
            {
                //Ｑ００２		
                if (bbl.ShowMessage("Q002") != DialogResult.Yes)
                    return;
            }
            //更新処理
            dje = GetEntity();
            nkbl.NayoseSyoriAll_Exec(dje);

            if (!mAutoMode)
            {
                if (!string.IsNullOrWhiteSpace(dje.ReturnFLG))
                {
                    bbl.ShowMessage(dje.ReturnFLG);
                    return;
                }

                bbl.ShowMessage("I002");
            }
            else
            {
                EndSec();
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
                    {
                        break;
                    }
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会
                case 5: //F6:キャンセル
                    break;

                    //case 11:    //F12:登録
                    //    {
                    //        //Ｑ１０１		
                    //        if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                    //            return;

                    //        this.ExecSec();
                    //        break;
                    //    }
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
    }
}







