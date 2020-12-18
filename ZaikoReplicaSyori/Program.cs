using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entity;
using System.Data;

namespace ZaikoReplicaSyori
{
    class Program
    {
        static Login_BL loginbl = new Login_BL();
        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();

        static void Main(string[] args)
        {
            Console.Title = "ZaikoReplicaSyori";
            if (loginbl.ReadConfig() == true)
            {
                mmpe.ID = "255";
                mmpe.Key = "1";

            }
        }
    }
}
