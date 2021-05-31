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
using GridBase;

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

        private DataTable dtForUpdate;      //排他用   
        private string mOldJuchuNO = "";    //排他処理のため使用
        private string mOldPickingDate = "";

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

                string ymd = bbl.GetDate();
                nkbl = new NayoseSyoriAll_BL();
                            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        private bool SelectAndInsertExclusive(Exclusive_BL.DataKbn kbn, string No)
        {
            if (OperationMode == EOperationMode.SHOW)
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)kbn,
                Number = No,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                PreviousCtrl.Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive()
        {
            if (dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                mOldJuchuNO = "";
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity
                    {
                        DataKBN = Convert.ToInt16(dr["kbn"]),
                        Number = dr["no"].ToString()
                    };

                    ebl.D_Exclusive_Delete(de);
                }
                return;
            }
        }
        /// <summary>
        /// データ取得処理
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            M_Customer_Entity mce = new M_Customer_Entity();
            mce.StoreKBN = "1";//1（WEB)
            DataTable dtCustomer = nkbl.M_Customer_SelectForNayose(mce);

            //[D_Juchu_SelectForNayose]
            //dje = GetSearchInfo();

            DataTable dt = nkbl.D_Juchu_SelectForNayose(dje);

            //ヘッダ部.名寄せ実施日＝空白　の時
            //画面転送表01に従って、画面情報を表示

            //ヘッダ部.名寄せ実施日<>空白　の時
            //画面転送表02に従って、画面情報を表示

            //テーブル転送仕様Ｙに従って、排他テーブルにレコード追加
            DeleteExclusive();

            dtForUpdate = new DataTable();
            dtForUpdate.Columns.Add("kbn", Type.GetType("System.String"));
            dtForUpdate.Columns.Add("no", Type.GetType("System.String"));

            bool ret;
            //排他処理
            foreach (DataRow row in dt.Rows)
            {                
                if (mOldJuchuNO != row["JuchuNo"].ToString() && !string.IsNullOrWhiteSpace(row["JuchuNo"].ToString()))
                {
                    ret = SelectAndInsertExclusive(Exclusive_BL.DataKbn.Jyuchu, row["JuchuNo"].ToString());
                    if (!ret)
                        return false;

                    mOldJuchuNO = row["JuchuNo"].ToString();

                    // データを追加
                    DataRow rowForUpdate;
                    rowForUpdate = dtForUpdate.NewRow();
                    rowForUpdate["kbn"] = (int)Exclusive_BL.DataKbn.Jyuchu;
                    rowForUpdate["no"] = mOldJuchuNO;
                    dtForUpdate.Rows.Add(rowForUpdate);
                }
            }


            //ピッキング(D_Juchuu)に存在しない場合、Error 「登録されていないピッキング番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", "ピッキング番号");
                PreviousCtrl.Focus();
                return false;
            }
            else
            {
                //DeleteDateTime 「削除されたピッキング番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", "ピッキング番号");
                    PreviousCtrl.Focus();
                    return false;
                }

                ////権限がない場合（以下のSelectができない場合）Error　「権限のないピッキング番号」
                //if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                //{
                //    bbl.ShowMessage("E139", "ピッキング番号");
                //    Scr_Clr(1);
                //    previousCtrl.Focus();
                //    return false;
                //}
            }
            return true;
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
      
            //更新処理
            dje = GetEntity();
            //nkbl.NayoseSyoriAll_Exec(dje,dt);

            //ログファイルへの更新
            bbl.L_Log_Insert(Get_L_Log_Entity());

                bbl.ShowMessage("I101");
           
        }
        /// <summary>
        /// get Log information
        /// print log
        /// </summary>
        private L_Log_Entity Get_L_Log_Entity()
        {
            ////画面指定項目をカンマ編集で羅列（ex."2019/07/01,2019/7/31,ABCDEFG,未出力"）
            //string item = keyControls[0].Text;
            //for (int i = 1; i <= (int)EIndex.JanCD; i++)
            //{
            //    item += "," + keyControls[i].Text;
            //}

            L_Log_Entity lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "寄せ結果登録",
                KeyItem = ""
            };

            return lle;
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

                case 11:    //F12:登録
                    {
                        //Ｑ１０１		
                        if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                            return;

                        this.ExecSec();
                        break;
                    }
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








