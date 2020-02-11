using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entity;
using System.Data;
using System.Threading;
using System.Net;
using System.IO;
using System.Collections;

namespace EDIKaitouNoukiTouroku
{
    class Program
    {
        static EDIKaitouNouki_Batch api = new EDIKaitouNouki_Batch();
        static EDIKaitouNoukiBatch_BL ediAPI_bl = new EDIKaitouNoukiBatch_BL();
        static Login_BL loginbl = new Login_BL();
        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();

        static DataTable dtMulti;
        static void Main(string[] args)
        {
            Console.Title = "EDIKaitouNoukiTouroku";

            if (loginbl.ReadConfig() == true)
            {
                mmpe.ID = MultiPorpose_BL.ID_EDI;
                mmpe.Key = "1";
                dtMulti = ediAPI_bl.M_MultiPorpose_SelectID(mmpe);

                //【起動可否確認】
                //汎用マスター.	数字型１＝０なら、処理終了
                if (dtMulti.Rows[0]["Num1"].ToString().Equals("0"))
                    return;

                //汎用マスター.文字型１で設定されたドライブ＆フォルダー内に存在するサブフォルダーを順に確認し、そのフォルダー内に存在する																														
                //CSVファイル、Excelファイルを読み込む
                mmpe.Char1 = dtMulti.Rows[0]["Char1"].ToString();
                mmpe.Char2 = dtMulti.Rows[0]["Char2"].ToString();

                //api.OperatorCD = 
                //【データ取得】
                api.Import(mmpe);

            }
        }
    }
}
