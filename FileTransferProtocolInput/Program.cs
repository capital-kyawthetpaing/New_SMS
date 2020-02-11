using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entity;
using System.Data;

namespace FileTransferProtocolInput
{
    class Program
    {
        static Login_BL loginbl = new Login_BL();
        static FileTransferProtocol_BL ftpsbl = new FileTransferProtocol_BL();
        static FileTransferProtocolInput ftpi = new FileTransferProtocolInput();
        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        static DataTable dtMulti;
        static void Main(string[] args)
        {
           
            Console.Title = "FileTransferProtocolInput";

            if (loginbl.ReadConfig() == true)
            {
                mmpe.ID = "324";
                mmpe.Key = "1";
                dtMulti = ftpsbl.M_MultiPorpose_SelectID(mmpe);

                //【起動可否確認】
                //汎用マスター.	数字型１＝０なら、処理終了
                if (dtMulti.Rows[0]["Num1"].ToString().Equals("0"))
                    return;

                ftpi.FTPImport();
            }
        }
    }
}
