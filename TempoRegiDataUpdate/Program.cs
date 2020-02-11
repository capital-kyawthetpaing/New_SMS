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
using Base.Client;

namespace TempoRegiDataUpdate
{
    class Program
    {
        static TempoRegiDataUpdate exe = new TempoRegiDataUpdate();
        static Login_BL loginbl = new Login_BL();
        /// <summary>
        /// コマンドライン引数
        /// </summary>
        static string InCompanyCD { get; set; }
        static string InOperatorCD { get; set; }
        static string InOperatorNM { get; set; }
        static string InPcID { get; set; }
        static string InStoreCD { get; set; }
        static string InSaleNO { get; set; }
        static string InOperationMode { get; set; }
        static void Main(string[] args)
        {
            Console.Title = "TempoRegiDataUpdate";

            if (loginbl.ReadConfig() == true)
            {
                if (GetCmdLine() == false)
                    return;

                D_Sales_Entity dse = new D_Sales_Entity();
                dse.SalesNO = InSaleNO;
                dse.Operator = InOperatorCD;
                dse.PC = InPcID;
                dse.StoreCD = InStoreCD;

                exe.ExecUpdate(dse, Convert.ToInt32( InOperationMode));

            }
        }

        static bool GetCmdLine()
        {
            //コマンドライン引数を配列で取得する
            string[] cmds = System.Environment.GetCommandLineArgs();

            //コマンドライン引数を列挙する
            for (int count = 0; count < cmds.Length; count++)
            {
                if (count == (int)ShopBaseForm.ECmdLine.CompanyCD)
                {
                    InCompanyCD = cmds[(int)ShopBaseForm.ECmdLine.CompanyCD];
                }
                else if (count == (int)ShopBaseForm.ECmdLine.OperatorCD)
                {
                    InOperatorCD = cmds[(int)ShopBaseForm.ECmdLine.OperatorCD];
                }
                else if (count == (int)ShopBaseForm.ECmdLine.PcID)
                {
                    InPcID = cmds[(int)ShopBaseForm.ECmdLine.PcID];
                }
                else if (count == (int)ShopBaseForm.ECmdLine.PcID+1)
                {
                    InStoreCD = cmds[(int)ShopBaseForm.ECmdLine.PcID+1];
                }
                else if (count == (int)ShopBaseForm.ECmdLine.PcID+2)
                {
                    InSaleNO = cmds[(int)ShopBaseForm.ECmdLine.PcID+2];
                }
                else if (count == (int)ShopBaseForm.ECmdLine.PcID + 3)
                {
                    InOperationMode = cmds[(int)ShopBaseForm.ECmdLine.PcID + 3];
                }
            }

            if (InOperatorCD.Trim() == "")
            {
                //オペレータコードを取得できませんでした
                return false;
            }
            else if (InPcID.Trim() == "")
            {
                //コンピュータ名を取得できませんでした
                return false;
            }
            else if (InCompanyCD.Trim() == "")
            {
                //入力営業所コードを取得できませんでした
                return false;
            }
            else if (InStoreCD.Trim() == "")
            {
                //入力店舗コードを取得できませんでした
                return false;
            }
            return true;
        }
    }
}
