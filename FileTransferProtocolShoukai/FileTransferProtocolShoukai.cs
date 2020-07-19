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

namespace FileTransferProtocolShoukai
{
    public partial class FileTransferProtocolShoukai :FrmMainForm
    {
        M_MultiPorpose_Entity mmpe;
        FileTransferProtocol_BL ftpsbl;
        private string mFTPMode1;
        private string mFTPMode2;
        public FileTransferProtocolShoukai()
        {
            InitializeComponent();
        }

        private void FileTransferProtocolShoukai_Load(object sender, EventArgs e)
        {
            InProgramID = "FileTransferProtocolShoukai";
            StartProgram();

            SetFunctionLabel(EProMode.SHOW);
            ftpsbl = new FileTransferProtocol_BL();

            SetEnableVisible();

            ExecSec();

        }


        private void SetEnableVisible()
        {
            ModeVisible = false;
            Btn_F2.Text = string.Empty;
            Btn_F3.Text = string.Empty;
            Btn_F4.Text = string.Empty;
            Btn_F5.Text = string.Empty;

            Btn_F12.Visible = true;
            Btn_F12.Text = "表示(F12)";
            Btn_F11.Text = string.Empty;

            gdvFTPType1.DisabledColumn("colFTPDateTime,colVendor,colFTPFile");
            gdvFTPType2.DisabledColumn("colFTPDateTime2,colVendor2,colFTPFile2");
        }

        protected override void ExecSec()
        {
            try
            {
                ftpsbl = new FileTransferProtocol_BL();

                //【取込履歴】for FTP type 1
                DataTable dt_Num1 = new DataTable();
                mmpe = new M_MultiPorpose_Entity
                {
                    ID = "323",
                    Key = "1"
                };

                dt_Num1 = ftpsbl.M_MultiPorpose_SelectID(mmpe);
                if (dt_Num1.Rows.Count > 0)
                {
                    //[999日間の履歴を保持しています]
                    lblRireki1.Text = dt_Num1.Rows[0]["Num3"].ToString() + "日間の履歴を保持しています";

                    mFTPMode1 = dt_Num1.Rows[0]["Num1"].ToString();

                    if (mFTPMode1 == "1")
                    {
                        //汎用マスター.	数字型１＝１なら、「処理実行中」として青色		
                        lblFTPMode1.Text = "処理実行中";
                        lblFTPMode1.BackColor = Color.DeepSkyBlue;
                    }
                    else
                    {
                        //汎用マスター.	数字型１＝０なら、「処理停止中」として黄色				
                        lblFTPMode1.Text = "処理停止中";
                        lblFTPMode1.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    //Ｅ１０１
                    ftpsbl.ShowMessage("E101");
                    EndSec();
                }

                //【取込履歴】for FTP type 2
                DataTable dt_Num2 = new DataTable();
                mmpe = new M_MultiPorpose_Entity
                {
                    ID = "324",
                    Key = "1"
                };

                dt_Num2 = ftpsbl.M_MultiPorpose_SelectID(mmpe);
                if (dt_Num2.Rows.Count > 0)
                {
                    //[999日間の履歴を保持しています]
                    lblRireki2.Text = dt_Num2.Rows[0]["Num3"].ToString() + "日間の履歴を保持しています";

                    mFTPMode2 = dt_Num2.Rows[0]["Num1"].ToString();

                    if (mFTPMode2 == "1")
                    {
                        //汎用マスター.	数字型１＝１なら、「処理実行中」として青色		
                        lblFTPMode2.Text = "処理実行中";
                        lblFTPMode2.BackColor = Color.DeepSkyBlue;
                    }
                    else
                    {
                        //汎用マスター.	数字型１＝０なら、「処理停止中」として黄色				
                        lblFTPMode2.Text = "処理停止中";
                        lblFTPMode2.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    //Ｅ１０１
                    ftpsbl.ShowMessage("E101");
                    EndSec();
                }

                //履歴データ取得処理 for FTP type 1
                DataTable dt = ftpsbl.D_FTP_SelectAll("1");

                gdvFTPType1.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    gdvFTPType1.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    gdvFTPType1.CurrentRow.Selected = true;
                    gdvFTPType1.Enabled = true;

                    gdvFTPType1.ReadOnly = false;

                }
                //else
                //{
                //    ftpsbl.ShowMessage("E128");
                //}

                //履歴データ取得処理 for FTP type 2
                DataTable dt1 = ftpsbl.D_FTP_SelectAll("2");

                gdvFTPType2.DataSource = dt1;

                if (dt1.Rows.Count > 0)
                {
                    gdvFTPType2.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                    gdvFTPType2.CurrentRow.Selected = true;
                    gdvFTPType2.Enabled = true;

                    gdvFTPType2.ReadOnly = false;

                }
                //else
                //{
                //    ftpsbl.ShowMessage("E128");
                //}

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        public void DataBind()
        {

        }
        protected override void EndSec()
        {
            this.Close();
        }

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
                case 5: //F6:キャンセル
                case 9://F10:印刷
                case 11://F12:最新化
                    ExecSec();
                    break;
            }   //switch end

        }

        private void btnStartType1_Click(object sender, EventArgs e)
        {
            try
            {
                mmpe = new M_MultiPorpose_Entity();

                mmpe.ID = "323";
                mmpe.Key = "1";
                mmpe.UpdateOperator = InOperatorCD;
                mmpe.PC = InPcID;
                //現状、「処理実行中」の場合、何もしない												
                if (lblFTPMode1.Text != "処理実行中")
                {
                    //現用、「処理停止中」(黄色)の場合、「処理実行中」(青色)とする																
                    lblFTPMode1.Text = "処理実行中";
                    mmpe.Num1 = "1";
                    lblFTPMode1.BackColor = Color.DeepSkyBlue;

                }

                ftpsbl.M_MultiPorpose_Update(mmpe);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void btnStartType2_Click(object sender, EventArgs e)
        {
            try
            {
                mmpe = new M_MultiPorpose_Entity();

                mmpe.ID = "324";
                mmpe.Key = "1";
                mmpe.UpdateOperator = InOperatorCD;
                mmpe.PC = InPcID;
                //現状、「処理実行中」の場合、何もしない												
                if (lblFTPMode2.Text != "処理実行中")
                {
                    //現用、「処理停止中」(黄色)の場合、「処理実行中」(青色)とする																
                    lblFTPMode2.Text = "処理実行中";
                    mmpe.Num1 = "1";
                    lblFTPMode2.BackColor = Color.DeepSkyBlue;

                }

                ftpsbl.M_MultiPorpose_Update(mmpe);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void btnStopType1_Click(object sender, EventArgs e)
        {
            try
            {
                mmpe = new M_MultiPorpose_Entity();

                mmpe.ID = "323";
                mmpe.Key = "1";
                mmpe.UpdateOperator = InOperatorCD;
                mmpe.PC = InPcID;

                //現状、「処理停止中」の場合、何もしない												
                if (lblFTPMode1.Text != "処理停止中")
                {
                    //現用、「処理実行中」(青色)の場合、「処理停止中」(黄色)とする																
                    lblFTPMode1.Text = "処理停止中";
                    mmpe.Num1 = "0";
                    lblFTPMode1.BackColor = Color.Yellow;

                }

                ftpsbl.M_MultiPorpose_Update(mmpe);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void btnStopType2_Click(object sender, EventArgs e)
        {
            try
            {
                mmpe = new M_MultiPorpose_Entity();

                mmpe.ID = "324";
                mmpe.Key = "1";
                mmpe.UpdateOperator = InOperatorCD;
                mmpe.PC = InPcID;

                //現状、「処理停止中」の場合、何もしない												
                if (lblFTPMode2.Text != "処理停止中")
                {
                    //現用、「処理実行中」(青色)の場合、「処理停止中」(黄色)とする																
                    lblFTPMode2.Text = "処理停止中";
                    mmpe.Num1 = "0";
                    lblFTPMode2.BackColor = Color.Yellow;

                }

                ftpsbl.M_MultiPorpose_Update(mmpe);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnSubF12_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
            try
            {
                FunctionProcess(FuncExec - 1);

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
