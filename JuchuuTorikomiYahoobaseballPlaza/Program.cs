using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL;
using Entity;
using System.Data;
using System.Threading;
using Yahoo_API;
namespace JuchuuTorikomiYahoobaseballPlaza
{
    class Program
    {
        static YahooAPI_BL yahooAPI_bl = new YahooAPI_BL();
        static Login_BL loginbl = new Login_BL();

        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        static D_APIControl_Entity DApiControl_entity = new D_APIControl_Entity();
        static M_API_Entity MApi_entity = new M_API_Entity();
        static Common_API api = new Common_API();
        static string Shopname = "bb-plaza";//racket,luckpiece,sportsplaza,baseball,honpo,
        static DataTable dtMulti, dtApiControl;
        static void Main(string[] args)
        {
            Console.Title = "JuchuuTorikomiYahoobaseballPlaza_受注取込(YahoobaseballPlaza)";
            Console.WriteLine("Started " + Shopname + " . . . ");
            if (loginbl.ReadConfig() == true)
            {
                mmpe.ID = "302";
                mmpe.Key = "1";
                dtMulti = yahooAPI_bl.M_MultiPorpose_SelectID(mmpe);

                while (dtMulti.Rows[0]["Num1"].ToString().Equals("0"))
                {
                    Console.WriteLine("Stop");
                    Thread.Sleep(Convert.ToInt32(dtMulti.Rows[0]["Num1"]) * 1000);
                    dtMulti = yahooAPI_bl.M_MultiPorpose_SelectID(mmpe); // select columns from M_Multiporpose table
                }

                DApiControl_entity.APIKey = "2";
                dtApiControl = yahooAPI_bl.D_APIControl_Select(DApiControl_entity);
                DApiControl_entity.LastGetDateTime = dtApiControl.Rows[0]["UpdateDateTime"].ToString();
                while (dtApiControl.Rows[0]["State"].ToString().Equals("0"))
                {
                    Console.WriteLine("Stop");
                    Thread.Sleep(Convert.ToInt32(dtApiControl.Rows[0]["State"]) * 1000);
                    dtApiControl = yahooAPI_bl.D_APIControl_Select(DApiControl_entity); // select columns from D_APIControl table
                }
                api.ShopName = Shopname;
                api.Search_GetOrderDetail(DApiControl_entity);

            }
        }
    }
}
