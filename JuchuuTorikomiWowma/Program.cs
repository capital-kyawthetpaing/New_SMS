using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using System.Data;
using System.Threading;
using System.Net;
using System.IO;
using System.Xml;
using Wowma_API;

namespace JuchuuTorikomiWowma
{
    class Program
    {
        static Login_BL loginbl = new Login_BL();
        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        static D_APIControl_Entity DApiControl_entity = new D_APIControl_Entity();
        static WowmaAPI_BL wowmaAPI_bl = new WowmaAPI_BL();
        static DataTable dtMulti, dtApiControl;
        static Common_API api = new Common_API();
        public static void Main(string[] args)
        {          
                Console.Title = "JuchuuTorikomiWowma_受注取込(Wowma)";
                
                if (loginbl.ReadConfig() == true)
                {
                    mmpe.ID = "302";
                    mmpe.Key = "1";
                    dtMulti = wowmaAPI_bl.M_MultiPorpose_SelectID(mmpe);

                    while (dtMulti.Rows[0]["Num1"].ToString().Equals("0"))
                    {
                        Console.WriteLine("Stop");
                        Thread.Sleep(Convert.ToInt32(dtMulti.Rows[0]["Num1"]) * 1000);
                        dtMulti = wowmaAPI_bl.M_MultiPorpose_SelectID(mmpe);
                    }

                    DApiControl_entity.APIKey = "13";
                    dtApiControl = wowmaAPI_bl.D_APIControl_Select(DApiControl_entity);
                    if (dtApiControl.Rows[0]["State"].ToString().Equals("0"))
                    {
                        Console.WriteLine("Stop");
                        Thread.Sleep(Convert.ToInt32(dtApiControl.Rows[0]["State"]) * 1000);
                        dtApiControl = wowmaAPI_bl.D_APIControl_Select(DApiControl_entity);
                    }
                api.GetOrderDetail_Wowma(DApiControl_entity);
            }
            }
        }
    }

