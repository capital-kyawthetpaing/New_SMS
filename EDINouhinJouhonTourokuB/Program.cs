using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entity;
using System.Data;

namespace EDINouhinJouhonTourokuB
{
    class Program
    {
        static Login_BL loginbl = new Login_BL();
        static EDINouhinJouhou_Batch edijh = new EDINouhinJouhou_Batch();
        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        static EDINouhinJouhon_Batch_BL edinjh_bl = new EDINouhinJouhon_Batch_BL();

        static DataTable dtMulti;
        static void Main(string[] args)
        {
            Console.Title = "EDINouhinJouhouTouroku";

            if (loginbl.ReadConfig() == true)
            {
                mmpe.ID ="322";
                mmpe.Key = "1";
                dtMulti = edinjh_bl.M_MultiPorpose_SelectID(mmpe);

                //【起動可否確認】
                //汎用マスター.	数字型１＝０なら、処理終了
                if (dtMulti.Rows[0]["Num1"].ToString().Equals("0"))
                    return;

                //汎用マスター.文字型１で設定されたドライブ＆フォルダー内に存在するサブフォルダーを順に確認し、そのフォルダー内に存在する																														
                //CSVファイル、Excelファイルを読み込む
                mmpe.Char1 = dtMulti.Rows[0]["Char1"].ToString();
                mmpe.Char2 = dtMulti.Rows[0]["Char2"].ToString();

                edijh.Import(mmpe);
            }

        }
    }
}
