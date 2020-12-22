using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entity;
using System.Data;

namespace MailRecieve
{
    class Program
    {
        static Login_BL loginbl = new Login_BL();
        static MailSend_BL msbl = new MailSend_BL();
        static MailRecieve mail = new MailRecieve();
        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        static DataTable dtMulti;

        static void Main(string[] args)
        {
            Console.Title = "FileTransferProtocolInput";

            //if (loginbl.ReadConfig() == true)
            //{
            //    mmpe.ID = "328";
            //    mmpe.Key = "1";
            //    dtMulti = msbl.M_MultiPorpose_SelectID(mmpe);

            //    //【起動可否確認】
            //    //汎用マスター.	数字型１＝０なら、処理終了
            //    if (dtMulti.Rows[0]["Num2"].ToString().Equals("0"))
            //        return;

            //    DataTable dtMail=msbl.ReceiveMailServer();
            
                mail.MailRead();

            //}
        }
    }
}
