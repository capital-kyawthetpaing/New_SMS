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
namespace TenzikaiJuchuuTourou
{
   internal partial class TenzikaiJuchuuTourou : FrmMainForm
    {
        private const string ProID = "TenzikaiJuchuuTourou";
        private const string ProNm = "展示会受注登録";
        private const short mc_L_END = 3; // ロック用
        private const string TempoNouhinsyo = "TenzikaiJuchuuTourou.exe";
        public TenzikaiJuchuuTourou()
        {
            InitializeComponent();
        }

        private void TenzikaiJuchuuTourou_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.INPUT);
               // this.InitialControlArray();

                // 明細部初期化
                //this.S_SetInit_Grid();

                //Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();

                //コンボボックス初期化
                //  string ymd = bbl.GetDate();
                // tubl = new TempoUriageNyuuryoku_BL();
                //  CboStoreCD.Bind(ymd);

                //検索用のパラメータ設定
                //    ScCustomerCD.Value1 = "1";
                //   ScCustomerCD.Value2 = "";

                Btn_F11.Text="";


               // ChangeOperationMode(EOperationMode.INSERT);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();
            }
        }
        protected void EndSec()
        {
            this.Close();
        }
    }
}
