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

namespace ShukkaUriageUpdate
{
    class Program
    {
        static ShukkaUriageUpdate api = new ShukkaUriageUpdate();
        static EDIKaitouNoukiBatch_BL ediAPI_bl = new EDIKaitouNoukiBatch_BL();
        static Login_BL loginbl = new Login_BL();
        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();

        static DataTable dtMulti;
        static void Main(string[] args)
        {
            Console.Title = "ShukkaUriageUpdate";

            if (loginbl.ReadConfig() == true)
            {
                mmpe.ID = MultiPorpose_BL.ID_ShukkaUriageUpdate;
                mmpe.Key = "1";
                dtMulti = ediAPI_bl.M_MultiPorpose_SelectID(mmpe);

                //【起動可否確認】
                //条件で獲得できる、汎用マスター.数字型１を、99:99の時刻形式に変換する
                string time = Convert.ToInt16(dtMulti.Rows[0]["Num1"]).ToString("D4");
                time = time.Substring(0, 2) + ":" + time.Substring(2, 2);
                //今の時間が、その時刻より先であれば、以下の処理を行う。
                string now = dtMulti.Rows[0]["HHMISS"].ToString().Substring(0, 5);

                int result = time.CompareTo(now);
                if (result > 0)
                {
                    return ;
                }

                //【データ更新】
                D_Sales_Entity de = new D_Sales_Entity();
                de.PC = Login_BL.GetHostName();
                de.Operator = "Administrator";
                api.ExecUpdate(de);

            }
        }
    }
}
