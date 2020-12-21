using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entity;
using System.Data;
using System.Globalization;

namespace ZaikoReplicaSyori
{
    class Program
    {
        static Login_BL loginbl = new Login_BL();
        static ZaikoReplicaSyori_BL zrsbl = new ZaikoReplicaSyori_BL();
        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        //static int saveDateNumber;
        //static DateTime tempDate;
        //static string saveDate;
        static void Main(string[] args)
        {
            Console.Title = "ZaikoReplicaSyori";
            if (loginbl.ReadConfig() == true)
            {
                if (zrsbl.D_StockReplica_Insert()) {
                   
                }
            }
        }
    }
}
