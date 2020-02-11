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
using Rakuten_API;

namespace JuchuuTorikomiRakutenBaseballPlaza
{
    class Program
    {
        static RakutenAPI_BL rakutenAPI_bl = new RakutenAPI_BL();
        static Login_BL loginbl = new Login_BL();

        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        static D_APIControl_Entity DApiControl_entity = new D_APIControl_Entity();
        static M_API_Entity MApi_entity = new M_API_Entity();
        static Common_API api = new Common_API();

        static DataTable dtMulti, dtApiControl;
        static void Main(string[] args)
        {
            Console.Title = "JuchuuTorikomiRakutenBaseballPlaza_受注取込(RakutenBaseballPlaza)";

            if (loginbl.ReadConfig() == true)
            {
                mmpe.ID = "302";
                mmpe.Key = "1";
                dtMulti = rakutenAPI_bl.M_MultiPorpose_SelectID(mmpe);

                while (dtMulti.Rows[0]["Num1"].ToString().Equals("0"))
                {
                    Console.WriteLine("Stop");
                    Thread.Sleep(Convert.ToInt32(dtMulti.Rows[0]["Num1"]) * 1000);
                    dtMulti = rakutenAPI_bl.M_MultiPorpose_SelectID(mmpe); // select columns from M_Multiporpose table
                }

                DApiControl_entity.APIKey = "24";
                dtApiControl = rakutenAPI_bl.D_APIControl_Select(DApiControl_entity);
                while (dtApiControl.Rows[0]["State"].ToString().Equals("0"))
                {
                    Console.WriteLine("Stop");
                    Thread.Sleep(Convert.ToInt32(dtApiControl.Rows[0]["State"]) * 1000);
                    dtApiControl = rakutenAPI_bl.D_APIControl_Select(DApiControl_entity); // select columns from D_APIControl table
                }

                api.Search_GetOrderDetail(DApiControl_entity);

            }
        }
    }
}
